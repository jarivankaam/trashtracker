window.mapHelper = {
    initMap: function (predictions) {
        const heatmapData = predictions.map(p => new google.maps.LatLng(p.lat, p.lng));

        const map = new google.maps.Map(document.getElementById("map"), {
            center: new google.maps.LatLng(37.774546, -122.433523),
            zoom: 13,
            mapTypeId: "satellite"
        });

        new google.maps.Marker({
            position: { lat: 37.785, lng: -122.435 },
            map,
            title: "testmarker"
        });

        const heatmap = new google.maps.visualization.HeatmapLayer({
            data: heatmapData
        });
        heatmap.setMap(map);
    }
};
