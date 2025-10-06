import 'package:flutter/material.dart';
import 'package:flutter_map/flutter_map.dart';
import 'package:latlong2/latlong.dart';

class MapWidget extends StatelessWidget {
  final MapController mapController;
  final LatLng? selectedLocation;
  final Function(double, double) onLocationSelected;

  const MapWidget({
    super.key,
    required this.mapController,
    required this.selectedLocation,
    required this.onLocationSelected,
  });

  @override
  Widget build(BuildContext context) {
    return FlutterMap(
      mapController: mapController,
      options: MapOptions(
        initialCenter: selectedLocation ?? LatLng(39.8283, -98.5795), // Centro de EE.UU.
        initialZoom: 4.3,
        onTap: (tapPosition, point) {
          onLocationSelected(point.latitude, point.longitude);
        },
      ),
      children: [
        TileLayer(
          urlTemplate:
              'https://{s}.basemaps.cartocdn.com/rastertiles/voyager/{z}/{x}/{y}.png',
          subdomains: const ['a', 'b', 'c', 'd'],
          userAgentPackageName: 'com.example.app',
        ),
        if (selectedLocation != null)
          MarkerLayer(
            markers: [
              Marker(
                point: selectedLocation!,
                width: 50,
                height: 50,
                child: const Icon(
                  Icons.location_on,
                  color: Colors.redAccent,
                  size: 35,
                ),
              ),
            ],
          ),
      ],
    );
  }
}
