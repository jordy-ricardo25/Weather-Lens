import 'package:flutter/material.dart';
import 'package:latlong2/latlong.dart';
import 'dart:math';

class BottomSheetWidget extends StatefulWidget {
  final LatLng? location;
  final String date;

  const BottomSheetWidget({
    super.key,
    this.location,
    required this.date,
  });

  @override
  State<BottomSheetWidget> createState() => _BottomSheetWidgetState();
}

class _BottomSheetWidgetState extends State<BottomSheetWidget> {
  late int temperature;
  late int humidity;
  late int wind;
  late int rainProb;

  final Random _random = Random();



  // 🔹 Cuatro listas de imágenes (una por contenedor)
  final List<String> hImages = [
    'assets/h1.jpg',
    'assets/h2.jpg',
    'assets/h3.jpg',
  ];

  final List<String> vImages = [
    'assets/v1.jpg',
    'assets/v2.jpg',
    'assets/v3.jpg',
  ];

  final List<String> rImages = [
    'assets/r1.jpg',
    'assets/r2.jpg',
    'assets/r3.jpg',
  ];

  final List<String> tImages = [
    'assets/t1.jpg',
    'assets/t2.jpg',
    'assets/t3.jpg',
  ];

  // 🔹 Variables para las imágenes seleccionadas
  late String hSelected;
  late String vSelected;
  late String rSelected;
  late String tSelected;

  @override
  void initState() {
    super.initState();

    // 🔸 Generar datos de clima aleatorios
    temperature = 20 + _random.nextInt(10);
    humidity = 50 + _random.nextInt(30);
    wind = 5 + _random.nextInt(15);
    rainProb = _random.nextInt(80);

    // 🔸 Seleccionar una imagen aleatoria por lista
    hSelected = hImages[_random.nextInt(hImages.length)];
    vSelected = vImages[_random.nextInt(vImages.length)];
    rSelected = rImages[_random.nextInt(rImages.length)];
    tSelected = tImages[_random.nextInt(tImages.length)];
  }

  @override
  Widget build(BuildContext context) {
    return Container(
      decoration: const BoxDecoration(
        color: Color.fromARGB(255, 240, 238, 243),
        borderRadius: BorderRadius.vertical(top: Radius.circular(25)),
      ),
      padding: const EdgeInsets.all(35.0),
      height: MediaQuery.of(context).size.height * 0.80,
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          const Text(
            "🌤️ Climate Data Overview",
            style: TextStyle(
              fontSize: 20,
              fontWeight: FontWeight.bold,
              color: Color.fromARGB(255, 55, 52, 66),
            ),
          ),
          const SizedBox(height: 8),

          // 📍 Ubicación y fecha
          if (widget.location != null)
            Text(
              "📍 Location: (${widget.location!.latitude.toStringAsFixed(3)}, ${widget.location!.longitude.toStringAsFixed(3)})",
              style: const TextStyle(fontSize: 14, color: Colors.black87),
            ),
          Text(
            "📅 Date: ${widget.date}",
            style: const TextStyle(fontSize: 14, color: Colors.black87),
          ),
          const SizedBox(height: 16),

          const Text(
            "Predicted Conditions:",
            style: TextStyle(
              fontWeight: FontWeight.bold,
              color: Color.fromARGB(255, 55, 52, 66),
            ),
          ),
          const SizedBox(height: 10),

          Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              Text("🌡️ Temperature: $temperature°C",
                  style: const TextStyle(fontSize: 16)),
              Text("💧 Humidity: $humidity%",
                  style: const TextStyle(fontSize: 16)),
              Text("🌬️ Wind: $wind km/h",
                  style: const TextStyle(fontSize: 16)),
              Text("☔ Probability of Rain: $rainProb%",
                  style: const TextStyle(fontSize: 16)),
            ],
          ),

          const SizedBox(height: 20),

          const Text(
            "Visual Data:",
            style: TextStyle(
              fontWeight: FontWeight.bold,
              color: Color.fromARGB(255, 55, 52, 66),
            ),
          ),
          const SizedBox(height: 10),

          // 🔹 Grid de 4 imágenes (una aleatoria por lista)
          Expanded(
            child: GridView.count(
              crossAxisCount: 2,
              mainAxisSpacing: 10,
              crossAxisSpacing: 10,
              childAspectRatio: 1.2,
              children: [
                _buildImageCard(hSelected, "Humidity Data (H)"),
                _buildImageCard(vSelected, "Visibility Data (V)"),
                _buildImageCard(rSelected, "Rain Data (R)"),
                _buildImageCard(tSelected, "Temperature Data (T)"),
              ],
            ),
          ),

          const SizedBox(height: 10),

          // 🔽 Botón de descarga (opcional)
          Center(
            child: ElevatedButton.icon(
              style: ElevatedButton.styleFrom(
                backgroundColor: Colors.deepPurpleAccent,
                padding:
                    const EdgeInsets.symmetric(vertical: 14, horizontal: 20),
                shape: RoundedRectangleBorder(
                  borderRadius: BorderRadius.circular(12),
                ),
              ),
              onPressed: () {
                // TODO: lógica para exportar o descargar
              },
              icon: const Icon(Icons.download, color: Colors.white),
              label: const Text(
                "Download Data",
                style: TextStyle(color: Colors.white, fontSize: 16),
              ),
            ),
          ),
        ],
      ),
    );
  }

  // 🔹 Método auxiliar para mostrar cada imagen con texto
  Widget _buildImageCard(String assetPath, String label) {
    return Container(
      decoration: BoxDecoration(
        color: Colors.white,
        borderRadius: BorderRadius.circular(16),
        boxShadow: const [
          BoxShadow(
            color: Colors.black12,
            blurRadius: 6,
            offset: Offset(2, 3),
          ),
        ],
      ),
      child: Column(
        mainAxisAlignment: MainAxisAlignment.center,
        children: [
          Expanded(
            child: ClipRRect(
              borderRadius:
                  const BorderRadius.vertical(top: Radius.circular(16)),
              child: Image.asset(
                assetPath,
                fit: BoxFit.cover,
                width: double.infinity,
              ),
            ),
          ),
          const SizedBox(height: 6),
          Text(
            label,
            textAlign: TextAlign.center,
            style: const TextStyle(
              fontWeight: FontWeight.w600,
              color: Color.fromARGB(255, 55, 52, 66),
            ),
          ),
          const SizedBox(height: 8),
        ],
      ),
    );
  }
}
