
document.addEventListener("DOMContentLoaded", function () {
    const products = window.createOrderData.products || [];
    const itemsFromModel = window.createOrderData.itemsFromModel || [];
    const errorMessage = window.createOrderData.errorMessage || null;

    const container = document.getElementById("productsContainer");

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

    window.addProduct = addProduct;

    const map = L.map('map').setView([4.7, -74.1], 6);
    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        attribution: '&copy; OpenStreetMap contributors'
    }).addTo(map);

    let originMarker = null;
    let destinationMarker = null;

    map.on('click', function (e) {
        if (!originMarker) {
            originMarker = L.marker(e.latlng, { draggable: true }).addTo(map).bindPopup('Origen').openPopup();
            setCoords('OriginLatitude', 'OriginLongitude', e.latlng);
            originMarker.on('dragend', e => {
                setCoords('OriginLatitude', 'OriginLongitude', e.target.getLatLng());
            });
        } else if (!destinationMarker) {
            destinationMarker = L.marker(e.latlng, { draggable: true }).addTo(map).bindPopup('Destino').openPopup();
            setCoords('DestinationLatitude', 'DestinationLongitude', e.latlng);
            destinationMarker.on('dragend', e => {
                setCoords('DestinationLatitude', 'DestinationLongitude', e.target.getLatLng());
            });
        }
    });

    function setCoords(latId, lngId, latlng) {
        document.getElementById(latId).value = latlng.lat.toFixed(6).replace(",", ".");
        document.getElementById(lngId).value = latlng.lng.toFixed(6).replace(",", ".");
    }

    document.querySelector('form').addEventListener('submit', function (e) {
        const items = container.querySelectorAll('select[name^="Items["]');
        if (items.length === 0) {
            e.preventDefault();
            Swal.fire({
                icon: 'warning',
                title: 'Productos requeridos',
                text: 'Debe agregar al menos un producto antes de guardar la orden.',
            });
        }
    });

    if (!isNaN(parseFloat(document.getElementById("OriginLatitude").value))) {
        const originLat = parseFloat(document.getElementById("OriginLatitude").value);
        const originLng = parseFloat(document.getElementById("OriginLongitude").value);
        originMarker = L.marker([originLat, originLng], { draggable: true }).addTo(map).bindPopup('Origen').openPopup();
        originMarker.on('dragend', e => {
            setCoords('OriginLatitude', 'OriginLongitude', e.target.getLatLng());
        });
    }

    if (!isNaN(parseFloat(document.getElementById("DestinationLatitude").value))) {
        const destLat = parseFloat(document.getElementById("DestinationLatitude").value);
        const destLng = parseFloat(document.getElementById("DestinationLongitude").value);
        destinationMarker = L.marker([destLat, destLng], { draggable: true }).addTo(map).bindPopup('Destino').openPopup();
        destinationMarker.on('dragend', e => {
            setCoords('DestinationLatitude', 'DestinationLongitude', e.target.getLatLng());
        });
    }

    if (Array.isArray(itemsFromModel)) {
        for (const item of itemsFromModel) {
            addProduct(item.productId, item.quantity);
        }
    }

    if (errorMessage) {
        Swal.fire({
            icon: 'error',
            title: 'Error',
            text: errorMessage,
            confirmButtonText: 'OK'
        });
    }
});
