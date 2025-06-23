window.mapHelper = {
    initMap: function (predictions) {
        if (!predictions || predictions.length === 0) {
            console.error("No predictions provided to initMap.");
            return;
        }

        // Groepeer trash op afgeronde locatie (4 decimalen ~11m precisie)
        const grouped = {};
        predictions.forEach(p => {
            const latKey = p.lat.toFixed(4);
            const lngKey = p.lng.toFixed(4);
            const key = `${latKey},${lngKey}`;

            if (!grouped[key]) {
                grouped[key] = {
                    lat: parseFloat(latKey),
                    lng: parseFloat(lngKey),
                    amount: 0
                };
            }

            grouped[key].amount += p.amount || 1; // fallback naar 1 als amount ontbreekt
        });

        const mapInstance = new google.maps.Map(document.getElementById("map"), {
            center: new google.maps.LatLng(51.571915, 4.768323),
            zoom: 13,
            mapTypeId: "satellite"
        });

        // Heatmapdata
        const heatmapData = Object.values(grouped).map(g => new google.maps.LatLng(g.lat, g.lng));
        const heatmap = new google.maps.visualization.HeatmapLayer({
            data: heatmapData,
            radius: 20,
            opacity: 0.7
        });
        heatmap.setMap(mapInstance);

        // Markers op hotspots met > 100 stuks
        Object.values(grouped).forEach(g => {
            if (g.amount > 10) {
                new google.maps.Marker({
                    position: { lat: g.lat, lng: g.lng },
                    map: mapInstance,
                    title: `${g.amount} pieces of trash`,
                    icon: {
                        url: "https://maps.google.com/mapfiles/ms/icons/red-dot.png",
                        scaledSize: new google.maps.Size(40, 40)
                    }
                });
            }
        });
    }
};

window.initMap = window.mapHelper.initMap;
