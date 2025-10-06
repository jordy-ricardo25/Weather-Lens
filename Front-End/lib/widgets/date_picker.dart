import 'package:flutter/material.dart';
import 'package:intl/intl.dart';

class DateTimePicker extends StatefulWidget {
  /// Callback que retorna fecha, hora y los datos meteorológicos ingresados
  final Function(DateTime, Map<String, double>) onDataConfirmed;

  const DateTimePicker({
    super.key,
    required this.onDataConfirmed,
  });

  @override
  State<DateTimePicker> createState() => _DateTimePickerState();
}

class _DateTimePickerState extends State<DateTimePicker> {
  DateTime? selectedDateTime;

  final TextEditingController precipitationController = TextEditingController();
  final TextEditingController temperatureController = TextEditingController();
  final TextEditingController humidityController = TextEditingController();
  final TextEditingController windController = TextEditingController();
  final TextEditingController shortRadController = TextEditingController();
  final TextEditingController longRadController = TextEditingController();

  Future<void> _selectDateTime(BuildContext context) async {
    // 🗓️ Seleccionar fecha
    final DateTime? selectedDate = await showDatePicker(
      context: context,
      initialDate: DateTime.now(),
      firstDate: DateTime(2000),
      lastDate: DateTime(2101),
    );

    if (selectedDate == null) return;

    // ⏰ Seleccionar hora
    final TimeOfDay? selectedTime = await showTimePicker(
      context: context,
      initialTime: TimeOfDay.now(),
    );

    if (selectedTime == null) return;

    // 🧩 Combinar fecha y hora
    setState(() {
      selectedDateTime = DateTime(
        selectedDate.year,
        selectedDate.month,
        selectedDate.day,
        selectedTime.hour,
        selectedTime.minute,
      );
    });
  }

  void _openDataDialog(BuildContext context) {
    showDialog(
      context: context,
      builder: (context) {
        return AlertDialog(
          shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(16)),
          title: const Text("🧭 Insert Meteorogical Data"),
          content: SingleChildScrollView(
            child: Column(
              children: [
                ElevatedButton.icon(
                  icon: const Icon(Icons.calendar_today),
                  label: Text(
                    selectedDateTime == null
                        ? "Select date and time"
                        : DateFormat('yyyy-MM-dd HH:mm').format(selectedDateTime!),
                  ),
                  onPressed: () => _selectDateTime(context),
                ),
                const SizedBox(height: 15),
                _buildInputField("Precipitation (mm)", precipitationController),
                _buildInputField("Temperature (°C)", temperatureController),
                _buildInputField("Humidity (%)", humidityController),
                _buildInputField("Wind (km/h)", windController),
                _buildInputField("Short Radiation (W/m²)", shortRadController),
                _buildInputField("Long Radiation (W/m²)", longRadController),
              ],
            ),
          ),
          actions: [
            TextButton(
              child: const Text("Cancel"),
              onPressed: () => Navigator.pop(context),
            ),
            ElevatedButton(
              child: const Text("Confirm"),
              onPressed: () {
                if (selectedDateTime == null) {
                  ScaffoldMessenger.of(context).showSnackBar(
                    const SnackBar(content: Text("Please select date and time")),
                  );
                  return;
                }

                final data = {
                  "precipitacion": double.tryParse(precipitationController.text) ?? 0,
                  "temperatura": double.tryParse(temperatureController.text) ?? 0,
                  "humedad": double.tryParse(humidityController.text) ?? 0,
                  "viento": double.tryParse(windController.text) ?? 0,
                  "radiacion_corta": double.tryParse(shortRadController.text) ?? 0,
                  "radiacion_larga": double.tryParse(longRadController.text) ?? 0,
                };

                widget.onDataConfirmed(selectedDateTime!, data);
                Navigator.pop(context);
              },
            ),
          ],
        );
      },
    );
  }

  @override
  Widget build(BuildContext context) {
    return ElevatedButton.icon(
      style: ElevatedButton.styleFrom(
        backgroundColor: Colors.deepPurpleAccent,
        padding: const EdgeInsets.symmetric(vertical: 14, horizontal: 20),
        shape: RoundedRectangleBorder(
          borderRadius: BorderRadius.circular(12),
        ),
      ),
      icon: const Icon(Icons.cloud, color: Colors.white),
      label: const Text(
        "Insert Extra Data",
        style: TextStyle(color: Colors.white, fontSize: 16),
      ),
      onPressed: () => _openDataDialog(context),
    );
  }

  Widget _buildInputField(String label, TextEditingController controller) {
    return Padding(
      padding: const EdgeInsets.symmetric(vertical: 6),
      child: TextField(
        controller: controller,
        keyboardType: TextInputType.number,
        decoration: InputDecoration(
          labelText: label,
          border: const OutlineInputBorder(),
        ),
      ),
    );
  }
}
