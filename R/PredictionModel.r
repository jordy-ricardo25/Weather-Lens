# ===============================
# modelo_prediccion.R
# ===============================

library(tidyverse)
library(sf)
library(lubridate)
library(tigris)
library(jsonlite)

# Leer argumentos de línea de comando
args <- commandArgs(trailingOnly = TRUE)
if (length(args) < 9) {
  stop("Uso: Rscript modelo_prediccion.R <fecha> <hora> <latitud> <longitud> <temperatura> <humedad> <viento> <rad_corta> <rad_larga>")
}

# Asignar argumentos
fecha            <- as.Date(args[1])
hora             <- as.numeric(args[2])
latitud          <- as.numeric(args[3])
longitud         <- as.numeric(args[4])
temperatura      <- as.numeric(args[5])
humedad          <- as.numeric(args[6])
velocidad_viento <- as.numeric(args[7])
radiacion_corta  <- as.numeric(args[8])
radiacion_larga  <- as.numeric(args[9])

# Cargar dataset (debe existir en el mismo directorio)
datos_promedios <- read.csv("DataSeries.csv")

# Centroides
centroides_tigris <- states(cb = TRUE, resolution = "20m") %>%
  st_centroid() %>%
  select(NAME, geometry) %>%
  mutate(
    latitud = st_coordinates(.)[,2],
    longitud = st_coordinates(.)[,1],
    estado = NAME
  ) %>%
  as_tibble() %>%
  select(estado, latitud, longitud) %>%
  filter(estado %in% state.name)

centroides_manual <- tibble(
  estado = c("Alabama", "Florida", "Louisiana", "Mississippi", "Texas"),
  latitud = c(32.318231, 27.664827, 31.244823, 32.354668, 31.968599),
  longitud = c(-86.902298, -81.515754, -92.145024, -89.398528, -99.901813)
)

centroides <- centroides_tigris %>%
  filter(!estado %in% centroides_manual$estado) %>%
  bind_rows(centroides_manual)

# --- Procesamiento principal ---
mes_usuario <- month(fecha)
dia_ano_usuario <- yday(fecha)
anio_usuario <- year(fecha)

sin_hora <- sin(2 * pi * hora / 24)
cos_hora <- cos(2 * pi * hora / 24)
sin_dia <- sin(2 * pi * dia_ano_usuario / 365)
cos_dia <- cos(2 * pi * dia_ano_usuario / 365)

distancias <- centroides %>%
  mutate(dist = sqrt((latitud - !!latitud)^2 + (longitud - !!longitud)^2)) %>%
  arrange(dist)
estado_cercano <- distancias$estado[1]

datos_estado <- datos_promedios %>% filter(estado == estado_cercano)

col_precip <- if ("precipitacion" %in% colnames(datos_estado)) "precipitacion" else
  if ("Rainf" %in% colnames(datos_estado)) "Rainf" else NULL
col_viento_n <- if ("viento_n" %in% colnames(datos_estado)) "viento_n" else
  if ("Wind_N" %in% colnames(datos_estado)) "Wind_N" else NULL
col_viento_e <- if ("viento_e" %in% colnames(datos_estado)) "viento_e" else
  if ("Wind_E" %in% colnames(datos_estado)) "Wind_E" else NULL
col_temperatura <- if ("temperatura" %in% colnames(datos_estado)) "temperatura" else
  if ("Tair" %in% colnames(datos_estado)) "Tair" else NULL
col_humedad <- if ("humedad" %in% colnames(datos_estado)) "humedad" else
  if ("Qair" %in% colnames(datos_estado)) "Qair" else NULL
col_rad_corta <- if ("radiacion_corta" %in% colnames(datos_estado)) "radiacion_corta" else
  if ("SWdown" %in% colnames(datos_estado)) "SWdown" else NULL
col_rad_larga <- if ("radiacion_larga" %in% colnames(datos_estado)) "radiacion_larga" else
  if ("LWdown" %in% colnames(datos_estado)) "LWdown" else NULL

datos_estado <- datos_estado %>%
  mutate(
    precip_binaria = as.numeric(.data[[col_precip]] > 0),
    velocidad_viento = if (!is.null(col_viento_n) && !is.null(col_viento_e))
      sqrt(.data[[col_viento_n]]^2 + .data[[col_viento_e]]^2) else NA
  )

predictors <- intersect(c(col_temperatura, col_humedad, "velocidad_viento",
                          col_rad_corta, col_rad_larga,
                          "sin_hora", "cos_hora", "sin_dia", "cos_dia"),
                        colnames(datos_estado))

formula <- as.formula(paste("precip_binaria ~", paste(predictors, collapse = " + ")))
modelo <- glm(formula, family = binomial(link = "probit"), data = datos_estado)

entrada <- tibble(
  sin_hora = sin_hora,
  cos_hora = cos_hora,
  sin_dia = sin_dia,
  cos_dia = cos_dia,
  !!col_temperatura := temperatura,
  !!col_humedad := humedad,
  velocidad_viento = velocidad_viento,
  !!col_rad_corta := radiacion_corta,
  !!col_rad_larga := radiacion_larga
)

prob <- predict(modelo, newdata = entrada, type = "response")

clasificacion <- if (prob <= 0.25) "Cielo despejado" else
  if (prob <= 0.50) "Parcialmente nublado" else
    if (prob <= 0.75) "Probabilidad de lluvia" else "Lluvia alta o tormenta"

# Salida JSON
resultado <- list(
  estado = estado_cercano,
  probabilidad = prob,
  clasificacion = clasificacion
)

cat(toJSON(resultado, pretty = TRUE, auto_unbox = TRUE))
