import 'package:flutter/material.dart';

class InfoScreenContent extends StatelessWidget {
  const InfoScreenContent({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: Column(
        children: [
          // Encabezado del diálogo
          Container(
            decoration: const BoxDecoration(
              color: Color.fromARGB(255, 160, 154, 163),
            ),
            padding: const EdgeInsets.symmetric(vertical: 16, horizontal: 20),
            width: double.infinity,
            child: const Row(
              mainAxisAlignment: MainAxisAlignment.spaceBetween,
              children: [
                Text(
                  "About Us",
                  style: TextStyle(
                    fontSize: 22,
                    fontWeight: FontWeight.bold,
                    color: Colors.white,
                  ),
                ),
                Icon(Icons.info_outline, color: Colors.white, size: 28),
              ],
            ),
          ),

          // Contenido scrollable del diálogo
          Expanded(
            child: SingleChildScrollView(
              padding: const EdgeInsets.all(16.0),
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  const Text(
                    "Acerca de Nosotros",
                    style: TextStyle(
                        fontSize: 24,
                        fontWeight: FontWeight.bold,
                        color: Colors.blueAccent),
                  ),
                  const SizedBox(height: 8),
                  const Text(
                    "Estudiantes de la Escuela Superior Politécnica de Chimborazo (ESPOCH), unimos los clubes AironCloud, Statistics y el IEEE Computer Society (Student Branch ESPOCH) para crear soluciones tecnológicas innovadoras.",
                    style: TextStyle(fontSize: 16, height: 1.5),
                  ),
                  const SizedBox(height: 16),

                  const Text(
                    "🎯 Objetivo de la App",
                    style:
                        TextStyle(fontSize: 20, fontWeight: FontWeight.bold),
                  ),
                  const SizedBox(height: 8),
                  const Text(
                    "Crear un panel personalizado con datos de la NASA (GES DISC) para consultar probabilidades de condiciones climáticas en una ubicación y fecha específicas.",
                    style: TextStyle(fontSize: 16, height: 1.5),
                  ),
                  const SizedBox(height: 12),
                  Wrap(
                    spacing: 8,
                    runSpacing: 8,
                    children: const [
                      _FeatureChip(icon: Icons.location_on, label: 'Seleccionar ubicación'),
                      _FeatureChip(icon: Icons.calendar_today, label: 'Elegir fecha'),
                      _FeatureChip(icon: Icons.download, label: 'Descargar datos'),
                    ],
                  ),
                  const SizedBox(height: 16),

                  const Text(
                    "⚙️ Detalles Técnicos",
                    style:
                        TextStyle(fontSize: 20, fontWeight: FontWeight.bold),
                  ),
                  const SizedBox(height: 8),
                  Wrap(
                    spacing: 8,
                    runSpacing: 8,
                    children: const [
                      _TechChip(icon: Icons.code, label: 'Flutter', color: Colors.blue),
                      _TechChip(icon: Icons.cloud, label: '.NET Backend', color: Colors.purple),
                      _TechChip(icon: Icons.analytics, label: 'R (Regresión)', color: Colors.green),
                      _TechChip(icon: Icons.storage, label: 'GES DISC API', color: Colors.indigo),
                    ],
                  ),
                  const SizedBox(height: 24),

                  const Text(
                    "👨‍💻 Equipo: ParadoxJ4",
                    style:
                        TextStyle(fontSize: 22, fontWeight: FontWeight.bold),
                  ),
                  const SizedBox(height: 12),
                  const _TeamMember(
                      name: 'Juan Quezada',
                      role: 'Capitán',
                      icon: Icons.engineering,
                      color: Colors.amber),
                  const _TeamMember(
                      name: 'Ricardo Carrión',
                      role: 'Arquitecto / Desarrollador',
                      icon: Icons.laptop_chromebook,
                      color: Colors.green),
                  const _TeamMember(
                      name: 'Aracelly Escudero',
                      role: 'Analista de Datos',
                      icon: Icons.bar_chart,
                      color: Colors.blue),
                  const _TeamMember(
                      name: 'Jordi Touriz',
                      role: 'Desarrollador Front End',
                      icon: Icons.web,
                      color: Colors.purple),
                  const _TeamMember(
                      name: 'Jordy Vele',
                      role: 'Desarrollador Front End',
                      icon: Icons.web,
                      color: Colors.purple),
                  const _TeamMember(
                      name: 'Edison Remache',
                      role: 'Analista General',
                      icon: Icons.build_circle,
                      color: Colors.orange),
                  const SizedBox(height: 16),
                ],
              ),
            ),
          ),

          // Botón para cerrar
          Padding(
            padding: const EdgeInsets.all(12.0),
            child: ElevatedButton(
              onPressed: () => Navigator.pop(context),
              style: ElevatedButton.styleFrom(
                backgroundColor: Colors.blueAccent,
                foregroundColor: Colors.white,
                shape: RoundedRectangleBorder(
                    borderRadius: BorderRadius.circular(12)),
              ),
              child: const Text("Cerrar"),
            ),
          ),
        ],
      ),
    );
  }
}

class _FeatureChip extends StatelessWidget {
  final IconData icon;
  final String label;
  const _FeatureChip({required this.icon, required this.label});

  @override
  Widget build(BuildContext context) {
    return Chip(
      avatar: Icon(icon, color: Colors.blueAccent),
      label: Text(label),
      backgroundColor: Colors.blue[50],
    );
  }
}

class _TechChip extends StatelessWidget {
  final IconData icon;
  final String label;
  final Color color;
  const _TechChip({required this.icon, required this.label, required this.color});

  @override
  Widget build(BuildContext context) {
    return Chip(
      avatar: Icon(icon, color: color),
      label: Text(label),
      backgroundColor: color.withOpacity(0.1),
    );
  }
}

class _TeamMember extends StatelessWidget {
  final String name;
  final String role;
  final IconData icon;
  final Color color;

  const _TeamMember({
    required this.name,
    required this.role,
    required this.icon,
    required this.color,
  });

  @override
  Widget build(BuildContext context) {
    return Card(
      margin: const EdgeInsets.symmetric(vertical: 6),
      child: ListTile(
        leading: Icon(icon, color: color, size: 36),
        title: Text(name,
            style: const TextStyle(fontWeight: FontWeight.bold, fontSize: 16)),
        subtitle: Text(role),
      ),
    );
  }
}
