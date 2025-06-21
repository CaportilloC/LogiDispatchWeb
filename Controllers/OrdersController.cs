using ClosedXML.Excel;
using LogiDispatchWeb.Models.DTOs;
using LogiDispatchWeb.Models.ViewModels;
using LogiDispatchWeb.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LogiDispatchWeb.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly ICustomerService _customerService;
        private readonly IProductService _productService;

        public OrdersController(
            IOrderService orderService,
            ICustomerService customerService,
            IProductService productService)
        {
            _orderService = orderService;
            _customerService = customerService;
            _productService = productService;
        }

        public async Task<IActionResult> Index(int status = 1)
        {
            SetStatusList(status);

            var allOrders = await _orderService.GetAllAsync();

            if (allOrders == null || !allOrders.Any())
            {
                ViewData["ErrorMessage"] = "No hay órdenes disponibles o ocurrió un error al cargarlas.";
                return View(new List<OrderDto>());
            }

            var filtered = allOrders.Where(o =>
                (status == 1 && o.Status == "Created") ||
                (status == 2 && o.Status == "InTransit") ||
                (status == 3 && o.Status == "Completed")
            ).ToList();

            return View(filtered);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var order = await _orderService.GetByIdAsync(id);

            if (order == null)
                return NotFound();

            return View(order);
        }

        public async Task<IActionResult> Create()
        {
            await LoadSelectListsAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateOrderRequest request)
        {
            if (!ModelState.IsValid)
            {
                await LoadSelectListsAsync();
                return View(request);
            }

            var response = await _orderService.CreateAsync(request);

            if (response?.Succeeded == true)
            {
                TempData["SuccessMessage"] = "Orden creada correctamente.";
                return RedirectToAction(nameof(Index));
            }

            ViewData["ErrorMessage"] = response?.Message ?? "Ocurrió un error al crear la orden.";
            await LoadSelectListsAsync();
            return View(request);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var order = await _orderService.GetByIdAsync(id);
            if (order == null)
                return NotFound();

            var model = new UpdateOrderRequest
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                Items = order.Items,
                OriginLatitude = order.OriginLatitude,
                OriginLongitude = order.OriginLongitude,
                DestinationLatitude = order.DestinationLatitude,
                DestinationLongitude = order.DestinationLongitude,
                Status = order.Status switch
                {
                    "Created" => 1,
                    "InTransit" => 2,
                    "Completed" => 3,
                    _ => 1
                }
            };

            ViewBag.StatusList = new SelectList(new List<SelectListItem>
            {
                new() { Value = "1", Text = "Creada" },
                new() { Value = "2", Text = "En tránsito" },
                new() { Value = "3", Text = "Completada" }
            }, "Value", "Text", model.Status);

            await LoadSelectListsAsync();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateOrderRequest model)
        {
            if (!ModelState.IsValid)
            {
                SetStatusList(model.Status);
                await LoadSelectListsAsync();
                return View(model);
            }

            var result = await _orderService.UpdateAsync(model);

            if (result == null || !result.Succeeded)
            {
                ViewData["ErrorMessage"] = result?.Message ?? "No se pudo actualizar la orden. Inténtalo de nuevo.";
                SetStatusList(model.Status);
                await LoadSelectListsAsync();
                return View(model);
            }

            TempData["SuccessMessage"] = "Orden actualizada correctamente.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _orderService.DeleteAsync(id);

            if (!success)
                TempData["ErrorMessage"] = "No se pudo eliminar la orden.";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Deleted()
        {
            var deletedOrders = await _orderService.GetDeletedAsync();
            return View(deletedOrders);
        }

        public async Task<IActionResult> Restore(Guid id)
        {
            var success = await _orderService.RestoreAsync(id);

            if (!success)
                TempData["ErrorMessage"] = "No se pudo restaurar la orden.";

            return RedirectToAction(nameof(Deleted));
        }

        private async Task LoadSelectListsAsync()
        {
            var customers = await _customerService.GetAllCustomersAsync();
            var products = await _productService.GetAllProductsAsync();

            ViewBag.Customers = new SelectList(customers, "Id", "Name");
            ViewBag.Products = products;
        }

        private void SetStatusList(int selected)
        {
            ViewBag.StatusList = new SelectList(new List<SelectListItem>
            {
                new() { Value = "1", Text = "Creada" },
                new() { Value = "2", Text = "En tránsito" },
                new() { Value = "3", Text = "Completada" }
            }, "Value", "Text", selected);
        }

        public async Task<IActionResult> Report()
        {
            var orders = await _orderService.GetAllAsync();

            var grouped = orders
                .GroupBy(o => o.CustomerName)
                .Select(group => new OrderReportSummaryViewModel
                {
                    Customer = group.Key,
                    Intervals = AgruparPorIntervalos(group)
                }).ToList();

            return View(grouped);
        }

        private static string GetDistanceRange(decimal distance)
        {
            if (distance >= 1 && distance <= 50) return "1–50 km";
            if (distance >= 51 && distance <= 200) return "51–200 km";
            if (distance >= 201 && distance <= 500) return "201–500 km";
            if (distance >= 501 && distance <= 1000) return "501–1000 km";
            return "Fuera de rango";
        }

        public async Task<IActionResult> ExportToExcel()
        {
            var orders = await _orderService.GetAllAsync();

            var grouped = orders
                .GroupBy(o => o.CustomerName)
                .Select(group => new
                {
                    Customer = group.Key,
                    Intervals = AgruparPorIntervalos(group)
                }).ToList();

            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Reporte de Órdenes");
            var header = new[] { "Cliente", "1–50 km", "51–200 km", "201–500 km", "501–1000 km" };

            for (int i = 0; i < header.Length; i++)
            {
                worksheet.Cell(1, i + 1).Value = header[i];
                worksheet.Cell(1, i + 1).Style.Font.Bold = true;
                worksheet.Cell(1, i + 1).Style.Fill.BackgroundColor = XLColor.LightBlue;
            }

            // Contenido
            int row = 2;
            foreach (var item in grouped)
            {
                worksheet.Cell(row, 1).Value = item.Customer;
                worksheet.Cell(row, 2).Value = item.Intervals.ContainsKey("1–50 km") ? item.Intervals["1–50 km"] : 0;
                worksheet.Cell(row, 3).Value = item.Intervals.ContainsKey("51–200 km") ? item.Intervals["51–200 km"] : 0;
                worksheet.Cell(row, 4).Value = item.Intervals.ContainsKey("201–500 km") ? item.Intervals["201–500 km"] : 0;
                worksheet.Cell(row, 5).Value = item.Intervals.ContainsKey("501–1000 km") ? item.Intervals["501–1000 km"] : 0;
                row++;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;

            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Reporte_Ordenes.xlsx");
        }

        public async Task<IActionResult> ReportDetallado()
        {
            var orders = await _orderService.GetAllAsync();
            return View(orders); // usa List<OrderDto>
        }

        public async Task<IActionResult> ExportDetalladoToExcel()
        {
            var orders = await _orderService.GetAllAsync();
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Órdenes Detalladas");

            // Header
            var headers = new[] { "Orden", "Cliente", "Teléfono", "Fecha", "Producto", "Cantidad", "Estado" };
            for (int i = 0; i < headers.Length; i++)
            {
                worksheet.Cell(1, i + 1).Value = headers[i];
                worksheet.Cell(1, i + 1).Style.Font.Bold = true;
                worksheet.Cell(1, i + 1).Style.Fill.BackgroundColor = XLColor.LightBlue;
            }

            // Content
            int row = 2;
            foreach (var order in orders)
            {
                if (order.Items != null && order.Items.Any())
                {
                    foreach (var item in order.Items)
                    {
                        worksheet.Cell(row, 1).Value = order.Id.ToString().Substring(0, 5).ToUpper();
                        worksheet.Cell(row, 2).Value = order.CustomerName;
                        worksheet.Cell(row, 3).Value = order.CustomerPhone;
                        worksheet.Cell(row, 4).Value = order.CreatedAt.ToShortDateString();
                        worksheet.Cell(row, 5).Value = item.ProductName;
                        worksheet.Cell(row, 6).Value = item.Quantity;
                        worksheet.Cell(row, 7).Value = order.Status;
                        row++;
                    }
                }
                else
                {
                    worksheet.Cell(row, 1).Value = order.Id.ToString().Substring(0, 5).ToUpper();
                    worksheet.Cell(row, 2).Value = order.CustomerName;
                    worksheet.Cell(row, 3).Value = order.CustomerPhone;
                    worksheet.Cell(row, 4).Value = order.CreatedAt.ToShortDateString();
                    worksheet.Cell(row, 5).Value = "(sin productos)";
                    worksheet.Cell(row, 6).Value = "-";
                    worksheet.Cell(row, 7).Value = order.Status;
                    row++;
                }
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;

            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ReporteDetallado.xlsx");
        }

        private static Dictionary<string, int> AgruparPorIntervalos(IEnumerable<OrderDto> orders)
        {
            return orders
                .GroupBy(o => GetDistanceRange(o.DistanceKm))
                .ToDictionary(g => g.Key, g => g.Count());
        }
    }
}
