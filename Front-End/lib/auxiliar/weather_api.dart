import 'dart:convert';
import 'package:http/http.dart' as http;
import 'package:flutter/material.dart';

Future<Map<String, dynamic>?> getPrediction(
  DateTime date,
  double latitude,
  double longitude,
) async {
  final url = Uri.parse('http://192.168.0.16:5114/api/Predictions');

  final response = await http.post(
    url,
    headers: {'Content-Type': 'application/json', 'Accept': 'application/json'},
    body: jsonEncode({
      'date': date.toUtc().toIso8601String(),
      'latitude': latitude,
      'longitude': longitude,
    }),
  );

  if (response.statusCode == 200 || response.statusCode == 201) {
    debugPrint('Prediccion enviada correctamente');
    return jsonDecode(response.body) as Map<String, dynamic>;
  }

  return null;
}
