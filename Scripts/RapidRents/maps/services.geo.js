(function () {
    "use strict";

    angular.module(APPNAME)
        .factory('$geoService', GeoServiceFactory);

    GeoServiceFactory.$inject = ['$baseService'];

    function GeoServiceFactory($baseService) {
        var svc = this;

        svc.markers = null;

        svc.geoCoder = null;
        svc.latLng = null;
        svc.loc = null;

        svc.animation = google.maps.Animation;
        svc.getLatLng = _getLatLng;
        svc.getMap = _getMap;
        svc.getMarker = _getMarker;
        svc.markerFilter = _markerFilter;
        svc.placeMarkers = _placeMarkers;
        svc.setCenter = _setCenter;

        _init();

        function _init() {
            svc.geoCoder = new google.maps.Geocoder();
        }

        function _getLatLng(address, onSuccess, onError) {
            svc.latLng = {};

            svc.geoCoder.geocode({ 'address': address }, response);

            function response(results, status) {
                if (status === google.maps.GeocoderStatus.OK) {
                    svc.loc = results[0].geometry.location;

                    svc.latLng.lat = svc.loc.lat();
                    svc.latLng.lng = svc.loc.lng();

                    onSuccess(svc.latLng);
                } else {
                    onError(status);
                }
            }
        }

        function _setCenter(map, coordinates) {
            var centerView = map.setCenter({ lat: coordinates.lat, lng: coordinates.lng });
            map.setZoom(12);

            var mkr = {};

            var markerSettings = {
                position: centerView,
                map: map,
                animation: google.maps.Animation.DROP,
            };

            mkr = new google.maps.Marker(markerSettings);

            return mkr;
        }

        function _placeMarkers(map, arrProp, providerFx, customMarker, customPreference) {

            var counter = arrProp.length;
            var markers = {};

            for (var i = 0; i < counter; i++) {

                var currentMarkerData = arrProp[i];
                var newMarker = _getMarker(map, currentMarkerData, providerFx, customMarker, customPreference);

                markers[currentMarkerData.id] = newMarker;
            }
            return markers;
        }

        function _getMarker(map, singleItem, providerFx, customMarker, customPreference) {
            var mkr = {};
            var customMkr;
            var uiAnimation;
            var customUIAnim;
            var customUIIcon;

            if (singleItem) {
                customMkr = customMarker(singleItem);
            }

            else if (customPreference && !singleItem) {
                var markerCounter = customPreference.length;

                for (var i = 0; i < markerCounter; i++) {
                    if (customPreference[i].jsName == "markerAnimation") {
                        uiAnimation = customPreference[i].value;
                    }
                }
                customUIAnim = _markerFilter(uiAnimation);
                customMkr = { animation: customUIAnim, icon: customUIIcon };
            }

            var defaultMarkerSettings = {
                position: { lat: singleItem.latitude, lng: singleItem.longitude },
                map: map,
                animation: svc.animation.DROP
            };

            var mSettings = $baseService.merge({}, defaultMarkerSettings, customMkr);

            mkr = new google.maps.Marker(mSettings);

            if (singleItem.id) {
                mkr._rapidRentsId = singleItem.id;
            }

            infoWindow(mkr, singleItem, providerFx);
            return mkr;
        }

        function infoWindow(mkr, singleItem, contentProviderFx) {
            var info = {};
            var infoWindowOptions = {};
            infoWindowOptions.content = contentProviderFx(singleItem);
            info = new google.maps.InfoWindow(infoWindowOptions);

            mkr.addListener('mouseover', mouseOver);
            mkr.addListener('mouseout', mouseOut);
            mkr._rapidRentsInfoWindow = info;
        };

        function mouseOver() {
            var marker = this;
            marker._rapidRentsInfoWindow.open(marker.get('map'), marker);
        }

        function mouseOut() {
            var marker = this;
            marker._rapidRentsInfoWindow.close(marker.get('map'), marker);
        }

        function _getMap(argument1, settings) {
            return new google.maps.Map(argument1, settings);
        }

        function _markerFilter(uiAnimation) {
            switch (uiAnimation) {
                case "BOUNCE":
                    return 1;
                case "DROP":
                    return 2;
                case "KP":
                    return 4;
                case "MP":
                    return 3;
            }
        }

        return svc;
    }
})();
