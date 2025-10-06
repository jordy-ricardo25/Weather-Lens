# ☀️ WeatherLens

**WeatherLens** is a Flutter-based mobile application that allows users to visualize and analyze weather conditions using **NASA** and meteorological data sources.  
It includes an interactive map, climate predictions, and environmental data visualization tools.

---

## 📱 Features

- Interactive map using Flutter Map and Carto basemaps.  
- Location search with suggestions for U.S. states.  
- Date selector for climate data queries.  
- Climate data visualization in a draggable bottom sheet.  
- Option to input historical latitude and longitude manually.  
- Data export and download functionality.  
- Integration-ready for backend climate APIs.

---

## 🧰 Installation

1. Clone the repository:

    ```bash
    git clone https://github.com/your-username/weatherlens.git
    ```

2. Navigate to the project folder:

    ```bash
    cd weatherlens
    ```

3. Install dependencies:

    ```bash
    flutter pub get
    ```

4. Run the application:

    ```bash
    flutter run
    ```

---

## 🏗️ Building APK

To build a release APK:

```bash
flutter build apk --release
```

The generated APK will be located in:

```
build/app/outputs/flutter-apk/app-release.apk
```

---

## 📁 Folder Structure

```
lib/
│
├── main.dart                # Entry point
├── screens/
│   ├── home_screen.dart     # Main screen with map and UI
│
├── widgets/
│   ├── map_widget.dart           # Interactive map widget
│   ├── bottom_sheet_widget.dart  # Bottom sheet for displaying data
│   ├── date_picker.dart          # Custom date picker widget
│   ├── info_dialog_content.dart  # About dialog content
│
└── auxiliar/
    └── weather_api.dart     # API logic for creating locations and queries
```

---

## 📦 Dependencies

Main dependencies used in this project:

- `flutter_map`
- `latlong2`
- `intl`
- `http`
- `bottom_sheet`
- `path_provider`
- `csv`
- `share_plus`

---

## 👨‍💻 Developer Information

Developed by **PARADOJA J** as part of the **NASA 2025 Climate Challenge**  
and academic research at **ESPOCH, Ecuador**.

---

# 🌦️ WeatherLens API & Xtractor

**WeatherLens** also includes a modular backend platform designed to extract, store, and predict meteorological variables from open scientific sources such as **NASA GES DISC**.

The system consists of two main components:

- **WeatherLens API** — A RESTful API built with **ASP.NET Core 8**, following a clean architecture pattern with **Entity Framework** and **PostgreSQL**.  
- **Weather Xtractor** — A data extraction engine that retrieves massive datasets from remote APIs (NASA, Giovanni, OPeNDAP, etc.) to feed the prediction models and database.

---

## 🧩 Project Structure

```
WeatherLens/
├── Entities/
│   ├── User.cs
│   ├── Location.cs
│   ├── WeatherQuery.cs
│   ├── WeatherVariable.cs
│   ├── WeatherQueryVariable.cs
│   ├── WeatherResult.cs
│
├── Controllers/
│   ├── PredictionsController.cs
│   └── WeatherController.cs
│
├── Helpers/
│   └── WeatherPredictionHelper.cs
│
├── Data/
│   ├── AppDbContext.cs
│   └── Repositories/
│
└── Xtractor/
    ├── Logs.txt
    ├── USA_States.csv
    ├── Extractor.cs
    └── NASARequestBuilder.cs
```

---

## ⚙️ WeatherLens API

### 🧠 Overview

The API manages users, locations, weather queries, meteorological variables, and prediction results.  
It integrates an **ML.NET**-based model that estimates **probabilities of extreme conditions** using the following core variables:

- 🌧️ `Rainf` — Rainfall (mm/h)  
- 🌡️ `Tair` — Air Temperature (K)  
- 💧 `Qair` — Specific Humidity (kg/kg)  
- 🌬️ `Wind_N`, `Wind_E` — Wind Components (m/s)  
- ☀️ `SWdown` — Downward Shortwave Radiation (W/m²)  
- 🌫️ `LWdown` — Downward Longwave Radiation (W/m²)

---

### 🧾 Main Entities

| Entity | Description | Table |
|--------|--------------|--------|
| **User** | Represents a registered user with name, email, and role. | `users` |
| **Location** | Defines a geographic coordinate (latitude, longitude). | `locations` |
| **WeatherQuery** | A user's request for weather data on a specific date and location. | `weather_queries` |
| **WeatherVariable** | Meteorological variable definition. | `weather_variables` |
| **WeatherQueryVariable** | N:M relationship between queries and variables. | `weather_query_variables` |
| **WeatherResult** | Prediction results including probability of extreme events. | `weather_results` |

---

### 🧩 Endpoints

| Method | Route | Description |
|--------|--------|-------------|
| `POST` | `/api/predictions` | Generates a weather prediction based on date and coordinates. |
| `GET` | `/api/locations` | Lists all registered locations. |
| `GET` | `/api/variables` | Retrieves available climate variables. |
| `GET` | `/api/queries/{id}` | Fetches details of a specific weather query. |

---

### 📦 Example Request

```bash
POST /api/predictions
Content-Type: application/json

{
  "date": "2025-10-05T00:00:00Z",
  "latitude": -1.672,
  "longitude": -78.654
}
```

#### Example Response

```json
{
  "location": "Riobamba, EC",
  "variables": [
    { "name": "Tair", "value": 295.6, "unit": "K" },
    { "name": "Rainf", "value": 0.12, "unit": "mm/h" }
  ],
  "extreme_probability": 0.23
}
```

---

## 🛰️ Weather Xtractor

### 🧠 Purpose

The **Xtractor** automates the retrieval of time-series meteorological data from NASA servers (**GES DISC / Giovanni / OPeNDAP**), iterating through geographic points and climate variables.

- **Source:** [https://hydro1.gesdisc.eosdis.nasa.gov](https://hydro1.gesdisc.eosdis.nasa.gov)  
- **Format:** ASCII / CSV  
- **Period:** User-defined range (e.g., 2022–2025)

### 🗂️ Log Summary

The file `Logs.txt` contains extraction results:

```
✅ Rainf: 43849 records retrieved
✅ Tair: 43849 records retrieved
✅ Qair: 43849 records retrieved
✅ Wind_N: 43849 records retrieved
✅ Wind_E: 43849 records retrieved
✅ SWdown: 43849 records retrieved
✅ LWdown: 43849 records retrieved
```

Some states may show `0 records retrieved`, indicating unavailable NASA coverage or missing data for the requested coordinates.

---

## 🧪 Tech Stack

| Layer | Technology |
|-------|-------------|
| Mobile | Flutter 3 |
| Backend | ASP.NET Core 8 |
| ORM | Entity Framework Core |
| Database | PostgreSQL |
| ML | ML.NET (Averaged Perceptron Binary Classifier) |
| API Docs | Swagger / OpenAPI |
| Data Source | NASA GES DISC (OPeNDAP / Giovanni) |

---

## 🚀 How to Run

### 🧱 API Setup

```bash
dotnet restore
dotnet build
dotnet run
```

Swagger UI will be available at:  
👉 `https://localhost:5001/swagger`

### 🛰️ Run Xtractor

```bash
dotnet run --project ./Xtractor
```

Logs will be saved to `Logs.txt` with extraction summaries per variable and location.

---

## 📄 License

MIT License © 2025 WeatherLens Team
