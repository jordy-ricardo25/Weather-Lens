import 'package:flutter/material.dart';
import 'package:latlong2/latlong.dart';
import 'package:flutter_map/flutter_map.dart';
import 'package:intl/intl.dart';
import '/widgets/date_picker.dart';
import '/widgets/bottom_sheet_widget.dart';
import '/widgets/map_widget.dart';
import '/widgets/info_dialog_content.dart';
import '../auxiliar/weather_api.dart';
import '/widgets/historical_register_widget.dart';

class HomeScreen extends StatefulWidget {
  const HomeScreen({super.key});

  @override
  _HomeScreenState createState() => _HomeScreenState();
}

class _HomeScreenState extends State<HomeScreen> {
  LatLng? _selectedLocation;
  String _selectedDateStr = 'Select Date';
  DateTime? _selectedDate;
  final TextEditingController _searchController = TextEditingController();
  List<String> _suggestions = [];
  final _isSearching = false;

  // 📍 Controlador del mapa
  final MapController _mapController = MapController();

  // 📍 Lista de ubicaciones simuladas (puedes luego reemplazar por API)
  final Map<String, LatLng> _locations = {
    'Florida': LatLng(27.9944, -81.7603),
    'Mississippi': LatLng(32.3547, -89.3985),
    'Louisiana': LatLng(30.9843, -91.9623),
    'Texas': LatLng(31.9686, -99.9018),
    'Alabama': LatLng(32.3182, -86.9023),
  };

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text("WeatherLens"),
        actions: [
          IconButton(
            icon: const Icon(Icons.info_outline),
            onPressed: _showAboutDialog,
          ),
        ],
      ),
      body: Stack(
        children: [
          // 🌍 Mapa principal
          Column(
            children: [
              Padding(
                padding: const EdgeInsets.all(8.0),
                child: Column(
                  children: [
                    Row(
                      children: [
                        Expanded(
                          child: TextField(
                            controller: _searchController,
                            decoration: InputDecoration(
                              labelText: "Search a city...",
                              border: const OutlineInputBorder(),
                              suffixIcon: _isSearching
                                  ? const Padding(
                                      padding: EdgeInsets.all(10),
                                      child: SizedBox(
                                        width: 18,
                                        height: 18,
                                        child: CircularProgressIndicator(
                                          strokeWidth: 2,
                                        ),
                                      ),
                                    )
                                  : IconButton(
                                      icon: const Icon(Icons.clear),
                                      onPressed: () {
                                        setState(() {
                                          _searchController.clear();
                                          _suggestions.clear();
                                        });
                                      },
                                    ),
                            ),
                            onChanged: (query) {
                              _updateSuggestions(query);
                            },
                          ),
                        ),
                        IconButton(
                          icon: const Icon(Icons.search),
                          onPressed: () {
                            _performSearch(_searchController.text);
                          },
                        ),
                      ],
                    ),

                    // 🔽 Lista de sugerencias
                    if (_suggestions.isNotEmpty)
                      Container(
                        decoration: BoxDecoration(
                          color: Colors.white,
                          borderRadius: BorderRadius.circular(8),
                          boxShadow: const [
                            BoxShadow(
                              color: Colors.black12,
                              blurRadius: 6,
                              offset: Offset(0, 3),
                            ),
                          ],
                        ),
                        constraints: const BoxConstraints(maxHeight: 160),
                        child: ListView.builder(
                          shrinkWrap: true,
                          itemCount: _suggestions.length,
                          itemBuilder: (context, index) {
                            final suggestion = _suggestions[index];
                            return ListTile(
                              leading: const Icon(Icons.location_on_outlined),
                              title: Text(suggestion),
                              onTap: () {
                                _selectSuggestion(suggestion);
                              },
                            );
                          },
                        ),
                      ),
                  ],
                ),
              ),

              // 🗺️ Widget del mapa
              Expanded(
                child: MapWidget(
                  mapController: _mapController, // 👈 pasamos el controlador
                  selectedLocation: _selectedLocation,
                  onLocationSelected: (lat, lon) {
                    setState(() {
                      _selectedDate = null;
                      _selectedDateStr = 'Select Date';
                      _selectedLocation = LatLng(lat, lon);
                    });
                  },
                ),
              ),
            ],
          ),

          // 📦 Panel deslizable inferior
          DraggableScrollableSheet(
            initialChildSize: 0.1,
            minChildSize: 0.1,
            maxChildSize: 0.45,
            builder: (context, scrollController) {
              return Container(
                decoration: const BoxDecoration(
                  color: Color.fromARGB(255, 160, 154, 163),
                  borderRadius: BorderRadius.vertical(top: Radius.circular(20)),
                ),
                padding: const EdgeInsets.symmetric(
                  vertical: 16,
                  horizontal: 20,
                ),
                child: SingleChildScrollView(
                  controller: scrollController,
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.center,
                    children: [
                      Container(
                        width: 40,
                        height: 5,
                        margin: const EdgeInsets.only(bottom: 12),
                        decoration: BoxDecoration(
                          color: Colors.white54,
                          borderRadius: BorderRadius.circular(10),
                        ),
                      ),
                      const Text(
                        "Select Date and Analyze",
                        style: TextStyle(
                          fontSize: 18,
                          fontWeight: FontWeight.bold,
                          color: Colors.white,
                        ),
                      ),
                      const SizedBox(height: 16),

                      DateTimePicker(
                        onDataConfirmed: (selectedDateTime, data) async {
                          _selectedDate = selectedDateTime;

                          if (_selectedDate != null &&
                              _selectedLocation != null) {
                            final p = await getPrediction(
                              _selectedDate!,
                              _selectedLocation!.latitude,
                              _selectedLocation!.longitude,
                            );

                            debugPrint(p.toString());
                          }

                          setState(() {
                            _selectedDateStr = DateFormat(
                              'MMMM dd, yyyy – HH:mm',
                            ).format(selectedDateTime);
                          });
                        },
                      ),
                      Text(
                        'Selected Date: $_selectedDateStr',
                        style: const TextStyle(
                          color: Colors.white,
                          fontWeight: FontWeight.w500,
                        ),
                      ),
                      const SizedBox(height: 20),
                      ElevatedButton.icon(
                        icon: const Icon(Icons.history),
                        label: const Text("Historical Register"),
                        style: ElevatedButton.styleFrom(
                          backgroundColor: const Color.fromARGB(
                            255,
                            70,
                            45,
                            120,
                          ),
                          foregroundColor: Colors.white,
                          shape: RoundedRectangleBorder(
                            borderRadius: BorderRadius.circular(10),
                          ),
                          padding: const EdgeInsets.symmetric(
                            horizontal: 20,
                            vertical: 12,
                          ),
                        ),
                        onPressed: () {
                          showDialog(
                            context: context,
                            builder: (context) => HistoricalRegisterWidget(
                              selectedLocation: _selectedLocation,
                              onShowRegister: (coords) {
                                setState(() {
                                  _selectedLocation = coords;
                                  // 👇 muestra la imagen histórica
                                });
                              },
                            ),
                          );
                        },
                      ),
                      ElevatedButton(
                        style: ElevatedButton.styleFrom(
                          backgroundColor: const Color.fromARGB(
                            255,
                            43,
                            33,
                            71,
                          ),
                          shape: RoundedRectangleBorder(
                            borderRadius: BorderRadius.circular(12),
                          ),
                          padding: const EdgeInsets.symmetric(
                            vertical: 14,
                            horizontal: 24,
                          ),
                        ),
                        onPressed: () {
                          showModalBottomSheet(
                            context: context,
                            backgroundColor: Colors.white,
                            shape: const RoundedRectangleBorder(
                              borderRadius: BorderRadius.vertical(
                                top: Radius.circular(25),
                              ),
                            ),
                            builder: (context) => BottomSheetWidget(
                              location: _selectedLocation,
                              date: _selectedDateStr,
                            ),
                          );
                        },
                        child: const Text(
                          "Analyze Climate Data",
                          style: TextStyle(color: Colors.white, fontSize: 16),
                        ),
                      ),
                    ],
                  ),
                ),
              );
            },
          ),
        ],
      ),
      bottomNavigationBar: const BottomAppBar(
        child: Padding(
          padding: EdgeInsets.all(8.0),
          child: Text("Developed by PARADOJA J", textAlign: TextAlign.center),
        ),
      ),
    );
  }

  // 🔹 Muestra la ventana de información
  void _showAboutDialog() {
    showDialog(
      context: context,
      builder: (context) => Dialog(
        insetPadding: const EdgeInsets.all(16),
        backgroundColor: Colors.white.withOpacity(0.95),
        shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(20)),
        child: SizedBox(
          width: double.maxFinite,
          height: MediaQuery.of(context).size.height * 0.8,
          child: const InfoScreenContent(),
        ),
      ),
    );
  }

  // 🔍 Actualiza las sugerencias
  void _updateSuggestions(String query) {
    if (query.isEmpty) {
      setState(() {
        _suggestions.clear();
      });
      return;
    }

    setState(() {
      _suggestions = _locations.keys
          .where((city) => city.toLowerCase().contains(query.toLowerCase()))
          .toList();
    });
  }

  // 🔍 Ejecuta la búsqueda al presionar la lupa
  void _performSearch(String query) {
    if (_locations.containsKey(query)) {
      _selectSuggestion(query);
    } else {
      ScaffoldMessenger.of(
        context,
      ).showSnackBar(SnackBar(content: Text('No results found for "$query"')));
    }
  }

  // 📍 Selección de sugerencia o búsqueda
  void _selectSuggestion(String city) {
    final coords = _locations[city];
    if (coords != null) {
      setState(() {
        _selectedLocation = coords;
        _searchController.text = city;
        _suggestions.clear();
      });

      // 👇 Centra el mapa en la ciudad seleccionada
      _mapController.move(coords, 10.5); // Zoom configurable
    }
  }

  // 🌐 Ejemplo de llamada API
}
