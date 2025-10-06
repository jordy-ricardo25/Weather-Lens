import 'package:flutter/material.dart';
import 'package:latlong2/latlong.dart';

class HistoricalRegisterWidget extends StatefulWidget {
  final LatLng? selectedLocation;
  final Function(LatLng?) onShowRegister;

  const HistoricalRegisterWidget({
    super.key,
    required this.selectedLocation,
    required this.onShowRegister,
  });

  @override
  State<HistoricalRegisterWidget> createState() =>
      _HistoricalRegisterWidgetState();
}

class _HistoricalRegisterWidgetState extends State<HistoricalRegisterWidget> {
  final TextEditingController _latController = TextEditingController();
  final TextEditingController _lonController = TextEditingController();

  @override
  void dispose() {
    _latController.dispose();
    _lonController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    final hasLocation = widget.selectedLocation != null;

    return AlertDialog(
      title: const Text("Historical Register"),
      content: Column(
        mainAxisSize: MainAxisSize.min,
        children: [
          if (!hasLocation) ...[
            TextField(
              controller: _latController,
              decoration: const InputDecoration(labelText: "Latitude"),
              keyboardType: TextInputType.number,
            ),
            TextField(
              controller: _lonController,
              decoration: const InputDecoration(labelText: "Longitude"),
              keyboardType: TextInputType.number,
            ),
            const SizedBox(height: 12),
            const Text(
              "Enter coordinates manually since no point is selected.",
              style: TextStyle(fontSize: 12, color: Colors.black54),
            ),
          ] else
            Text(
              "Using selected point:\n"
              "Lat: ${widget.selectedLocation!.latitude.toStringAsFixed(4)}\n"
              "Lon: ${widget.selectedLocation!.longitude.toStringAsFixed(4)}",
              textAlign: TextAlign.center,
            ),
        ],
      ),
      actions: [
        TextButton(
          onPressed: () => Navigator.pop(context),
          child: const Text("Cancel"),
        ),
        ElevatedButton(
          onPressed: () {
            LatLng? coords;

            if (hasLocation) {
              coords = widget.selectedLocation;
            } else {
              final lat = double.tryParse(_latController.text);
              final lon = double.tryParse(_lonController.text);

              if (lat == null || lon == null) {
                ScaffoldMessenger.of(context).showSnackBar(
                  const SnackBar(
                    content: Text("Please enter valid latitude and longitude."),
                  ),
                );
                return;
              }
              coords = LatLng(lat, lon);
            }

            widget.onShowRegister(coords);
            Navigator.pop(context);
          },
          style: ElevatedButton.styleFrom(
            backgroundColor: const Color.fromARGB(255, 70, 45, 120),
          ),
          child: const Text("Show Register"),
        ),
      ],
    );
  }
}
