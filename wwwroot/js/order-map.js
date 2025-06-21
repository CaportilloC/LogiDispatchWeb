function initOrderMap(origin, destination) {
    const map = L.map('map').setView(origin, 6);

    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        attribution: '&copy; OpenStreetMap contributors'
    }).addTo(map);

    L.marker(origin).addTo(map).bindPopup('Origen').openPopup();
    L.marker(destination).addTo(map).bindPopup('Destino');

    const bounds = L.latLngBounds([origin, destination]);
    map.fitBounds(bounds, { padding: [50, 50] });
}