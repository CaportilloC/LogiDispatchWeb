document.addEventListener('DOMContentLoaded', () => {
    const container = document.getElementById("productsContainer");
    const products = window.editOrderData.products;
    const existingItems = window.editOrderData.items;

    function addProduct(productId = null, quantity = 1) {
        const index = container.children.length;
        const div = document.createElement("div");
        div.className = "row align-items-end mb-2";

        const optionsHtml = products.map(p => {
            const selected = p.id === productId ? 'selected' : '';
            return `<option value="${p.id}" ${selected}>${p.name}</option>`;
        }).join('');

        div.innerHTML = `
            <div class="col-md-6">
                <label class="form-label">Producto</label>
                <select name="Items[${index}].ProductId" class="form-select">
                    ${optionsHtml}
                </select>
            </div>
            <div class="col-md-4">
                <label class="form-label">Cantidad</label>
                <input type="number" name="Items[${index}].Quantity" class="form-control" min="1" value="${quantity}" />
            </div>
            <div class="col-md-2">
                <button type="button" class="btn btn-danger" onclick="this.closest('.row').remove()">X</button>
            </div>
        `;
        container.appendChild(div);
    }

    window.addProduct = addProduct; // expone la función globalmente

    // Inicializar productos existentes
    if (Array.isArray(existingItems)) {
        existingItems.forEach(item => addProduct(item.productId, item.quantity));
    }

    // Validación: al menos un producto
    document.querySelector('form').addEventListener('submit', function (e) {
        const items = container.querySelectorAll('select[name^="Items["]');
        if (items.length === 0) {
            e.preventDefault();
            Swal.fire({
                icon: 'warning',
                title: 'Productos requeridos',
                text: 'Debe agregar al menos un producto antes de actualizar la orden.',
            });
        }
    });

    // Mapa Leaflet
    const originLat = parseFloat(document.getElementById("OriginLatitude").value);
    const originLng = parseFloat(document.getElementById("OriginLongitude").value);
    const destLat = parseFloat(document.getElementById("DestinationLatitude").value);
    const destLng = parseFloat(document.getElementById("DestinationLongitude").value);

    const map = L.map('map').setView([originLat, originLng], 8);
    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        attribution: '&copy; OpenStreetMap contributors'
    }).addTo(map);

    let originMarker = L.marker([originLat, originLng], { draggable: true }).addTo(map).bindPopup('Origen').openPopup();
    let destinationMarker = L.marker([destLat, destLng], { draggable: true }).addTo(map).bindPopup('Destino').openPopup();

    originMarker.on('dragend', e => updateCoords('OriginLatitude', 'OriginLongitude', e.target.getLatLng()));
    destinationMarker.on('dragend', e => updateCoords('DestinationLatitude', 'DestinationLongitude', e.target.getLatLng()));

    function updateCoords(latId, lngId, latlng) {
        document.getElementById(latId).value = latlng.lat.toFixed(6);
        document.getElementById(lngId).value = latlng.lng.toFixed(6);
    }

    // Error popup si aplica
    if (window.editOrderData.errorMessage) {
        Swal.fire({
            icon: 'error',
            title: 'Error',
            text: window.editOrderData.errorMessage,
            confirmButtonText: 'OK'
        });
    }
});
