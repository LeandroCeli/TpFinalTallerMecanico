-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 06-11-2025 a las 23:37:08
-- Versión del servidor: 10.4.32-MariaDB
-- Versión de PHP: 8.0.30

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `tallermecanicodb`
--

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `cliente`
--

CREATE TABLE `cliente` (
  `Id` int(11) NOT NULL,
  `Nombre` varchar(100) NOT NULL,
  `Apellido` varchar(120) NOT NULL,
  `Dni` varchar(20) NOT NULL,
  `Telefono` varchar(50) DEFAULT NULL,
  `Email` varchar(150) DEFAULT NULL,
  `Direccion` varchar(200) DEFAULT NULL,
  `FechaRegistro` datetime DEFAULT current_timestamp(),
  `Activo` tinyint(1) DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Volcado de datos para la tabla `cliente`
--

INSERT INTO `cliente` (`Id`, `Nombre`, `Apellido`, `Dni`, `Telefono`, `Email`, `Direccion`, `FechaRegistro`, `Activo`) VALUES
(32, 'Carlos', 'Pérez', '30123456', '266400001', 'carlos.perez@mail.com', 'Av. Libertador 1200', '2025-11-05 23:39:51', 1),
(33, 'María', 'Gómez', '32145879', '266400002', 'maria.gomez@mail.com', 'San Martín 450', '2025-11-05 23:42:14', 1),
(34, 'Jorge', 'Rodríguez', '28965847', '266400003', 'jorge.rodriguez@mail.com', 'Mitre 880', '2025-11-05 23:45:42', 1),
(35, 'Laura', 'Fernández', '29587412', '266400004', 'laura.fernandez@mail.com', 'Rivadavia 1020', '2025-11-06 08:56:55', 1),
(36, 'Lucía', 'Martínez', '33458712', '266400005', 'lucia.martinez@mail.com', 'Belgrano 300', '2025-11-06 09:06:59', 1),
(37, 'Sergio', 'López', '30214587', '266400006', 'sergio.lopez@mail.com', 'Italia 560', '2025-11-06 09:12:14', 1),
(38, 'Gabriela', 'Ramos', '31654789', '266400007', 'gabriela.ramos@mail.com', 'Bolívar 250', '2025-11-06 09:15:06', 1),
(39, 'Pablo', 'Domínguez', '27896547', '266400008', 'pablo.dominguez@mail.com', 'España 730', '2025-11-06 09:16:23', 1),
(40, 'Andrés', 'Morales', '29236547', '266400010', 'andres.morales@mail.com', 'Junín 540', '2025-11-06 09:18:04', 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `rol`
--

CREATE TABLE `rol` (
  `Id` int(11) NOT NULL,
  `Nombre` varchar(50) NOT NULL,
  `Descripcion` text DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Volcado de datos para la tabla `rol`
--

INSERT INTO `rol` (`Id`, `Nombre`, `Descripcion`) VALUES
(1, 'Admin', 'Acceso completo al sistema'),
(2, 'Recepcionista', 'Encargado de atención al cliente '),
(3, 'Mecánico', 'Responsable de diagnóstico y reparación de vehículos');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `servicio`
--

CREATE TABLE `servicio` (
  `Id` int(11) NOT NULL,
  `Nombre` varchar(100) NOT NULL,
  `Descripcion` text DEFAULT NULL,
  `CostoBase` decimal(10,2) NOT NULL DEFAULT 0.00,
  `DuracionMinutos` int(11) DEFAULT 30,
  `IncluyeInsumos` tinyint(1) DEFAULT 0,
  `Activo` tinyint(1) DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Volcado de datos para la tabla `servicio`
--

INSERT INTO `servicio` (`Id`, `Nombre`, `Descripcion`, `CostoBase`, `DuracionMinutos`, `IncluyeInsumos`, `Activo`) VALUES
(1, 'Cambio de aceite y filtro', 'Reemplazo de aceite del motor y filtro correspondiente a nafta-aire-aceite-habitáculo\n', 100.00, 30, 1, 1),
(2, 'Diagnóstico electrónico', 'Escaneo de fallas con escáner OBD y revisión de sensores', 50000.00, 30, 0, 1),
(3, 'Revisión de suspensión', 'Chequeo de amortiguadores, bujes y componentes de dirección', 25000.00, 30, 0, 1),
(4, 'Recarga de aire acondicionado', 'Carga de gas refrigerante y prueba de funcionamiento', 50000.00, 30, 1, 1),
(5, 'Cambio de bujías', 'Reemplazo de bujías y ajuste de encendido', 50000.00, 30, 1, 1),
(6, 'Limpieza de inyectores', 'Descarbonización y limpieza de inyectores con equipo específico\n', 45000.00, 30, 1, 1),
(7, 'Revisión pre-ITV', 'Control general para pasar la inspección técnica vehicular', 850000.00, 30, 0, 1),
(8, 'Reemplazo de correa de distribución', 'Cambio de correa y tensores según kilometraje recomendado', 100000.00, 30, 1, 1),
(9, 'Reparación de sistema de escape', 'Soldadura o reemplazo de caño de escape, silenciador o catalizador', 600000.00, 30, 0, 1),
(10, 'Reemplazo de amortiguadores', 'Reemplazo de rótulas, extremos, bujes y alineación', 80000.00, 30, 1, 1),
(11, 'Limpieza de radiador', 'Desarme, limpieza interna y externa, y recarga de refrigerante', 50000.00, 30, 1, 1),
(12, 'Reemplazo de filtro de aire', 'Cambio de filtro de aire del motor', 20000.00, 30, 1, 1),
(13, 'Revisión de sistema de carga', 'Control de alternador, batería y cables', 35000.00, 30, 0, 1),
(14, 'Reemplazo de líquido refrigerante', 'Vaciado, limpieza y recarga del circuito de refrigeración', 50000.00, 30, 1, 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `trabajo`
--

CREATE TABLE `trabajo` (
  `Id` int(11) NOT NULL,
  `VehiculoId` int(11) NOT NULL,
  `UsuarioId` int(11) NOT NULL,
  `FechaEntrega` datetime DEFAULT NULL,
  `Estado` enum('Pendiente','En Proceso','Finalizado','Cancelado') DEFAULT 'Pendiente',
  `Observaciones` text DEFAULT NULL,
  `KilometrajeSalida` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Volcado de datos para la tabla `trabajo`
--

INSERT INTO `trabajo` (`Id`, `VehiculoId`, `UsuarioId`, `FechaEntrega`, `Estado`, `Observaciones`, `KilometrajeSalida`) VALUES
(9, 19, 1, '2025-11-06 09:22:21', 'Finalizado', 'Mantenimiento', 95000),
(10, 20, 1, '2025-11-06 09:24:14', 'Finalizado', 'mantenimiento en sistema de refrigeración ', 87000),
(11, 22, 1, '2025-11-06 19:21:49', 'Finalizado', 'Mantenimiento por kilometraje + mantenimiento general ', 120100),
(12, 31, 1, '2025-11-06 19:23:22', 'Finalizado', 'cambio por tiempo', 49060);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `trabajoservicio`
--

CREATE TABLE `trabajoservicio` (
  `Id` int(11) NOT NULL,
  `TrabajoId` int(11) NOT NULL,
  `ServicioId` int(11) NOT NULL,
  `CostoAplicado` decimal(10,2) DEFAULT 0.00,
  `Observaciones` text DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Volcado de datos para la tabla `trabajoservicio`
--

INSERT INTO `trabajoservicio` (`Id`, `TrabajoId`, `ServicioId`, `CostoAplicado`, `Observaciones`) VALUES
(9, 9, 6, 0.00, NULL),
(10, 10, 11, 0.00, NULL),
(11, 10, 14, 0.00, NULL),
(12, 11, 8, 0.00, NULL),
(13, 11, 6, 0.00, NULL),
(14, 12, 1, 0.00, NULL);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `usuario`
--

CREATE TABLE `usuario` (
  `Id` int(11) NOT NULL,
  `Nombre` varchar(100) NOT NULL,
  `Apellido` varchar(100) NOT NULL,
  `Email` varchar(150) NOT NULL,
  `Password` varchar(255) NOT NULL,
  `RolId` int(11) NOT NULL,
  `Avatar` varchar(255) DEFAULT NULL,
  `FechaRegistro` datetime DEFAULT current_timestamp(),
  `Activo` tinyint(1) DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Volcado de datos para la tabla `usuario`
--

INSERT INTO `usuario` (`Id`, `Nombre`, `Apellido`, `Email`, `Password`, `RolId`, `Avatar`, `FechaRegistro`, `Activo`) VALUES
(1, 'Leandro', 'Celi', 'admin@tallerGestion.com', '$2a$12$U/NcyQvJYjrVys3iCfP6wuEKvDanOdTB1/GoUd6zUksXt.5xI4luO', 1, NULL, '2025-11-01 10:41:26', 1),
(4, 'Cristian', 'Race', 'mecanico@tallerGestion.com', '$2a$11$kWqYH2tgg3nnTWYmJPdg1uHzWfSsa9iJb9KHqUswXSsMyCQZVzuLu', 3, NULL, '2025-11-06 09:46:48', 1),
(5, 'Calos', 'Test', 'recepcion@tallerGestion.com', '$2a$11$KQ7E1UMe.9UyIOKF0eHZZ.15ONu.q4qoregCPXA/3ZUnlEWusSfMO', 2, NULL, '2025-11-06 09:47:36', 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `vehiculo`
--

CREATE TABLE `vehiculo` (
  `Id` int(11) NOT NULL,
  `ClienteId` int(11) NOT NULL,
  `Patente` varchar(15) NOT NULL,
  `Marca` varchar(50) DEFAULT NULL,
  `Modelo` varchar(50) DEFAULT NULL,
  `Anio` int(11) DEFAULT NULL,
  `Color` varchar(50) DEFAULT NULL,
  `Kilometraje` int(11) DEFAULT NULL,
  `Tipo` varchar(50) DEFAULT NULL,
  `FechaRegistro` datetime DEFAULT current_timestamp(),
  `Activo` tinyint(1) DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Volcado de datos para la tabla `vehiculo`
--

INSERT INTO `vehiculo` (`Id`, `ClienteId`, `Patente`, `Marca`, `Modelo`, `Anio`, `Color`, `Kilometraje`, `Tipo`, `FechaRegistro`, `Activo`) VALUES
(19, 32, 'ABC123', 'Toyota', 'Corolla', 2018, 'Gris', 95000, 'Personal', '2025-11-05 23:40:34', 1),
(20, 32, 'ACB789', 'Ford', 'Focus', 2017, 'Negro', 87000, 'Personal', '2025-11-05 23:41:27', 1),
(21, 33, 'ACD147', 'Ford', 'Ranger', 2021, 'Rojo', 64000, 'Trabajo', '2025-11-05 23:42:58', 1),
(22, 33, 'ABD456', 'Toyota', 'Hilux', 2019, 'BLANCO', 120100, 'Trabajo', '2025-11-05 23:44:54', 1),
(23, 34, 'ACE258', 'Fiat', 'Cronos', 2019, 'Azul', 52000, 'Personal', '2025-11-05 23:46:13', 1),
(24, 35, 'ADF369', 'Chevrolet', 'Onix', 2020, 'Gris', 41000, 'Personal', '2025-11-06 08:57:45', 1),
(25, 35, 'ADG741', 'Chevrolet', 'S10', 2022, 'Blanco', 29000, 'Trabajo', '2025-11-06 08:58:24', 1),
(26, 36, 'ADH852', 'Renault', 'Kangoo', 2016, 'b', 133000, 'Trabajo', '2025-11-06 09:11:34', 1),
(27, 37, 'ADJ963', 'Volkswagen', 'Golf', 2019, 'Negro', 76000, 'Personal', '2025-11-06 09:12:54', 1),
(28, 37, 'ADK159', 'Volkswagen', 'Amarok', 2020, 'Gris', 99000, 'Trabajo', '2025-11-06 09:13:32', 1),
(29, 38, 'ADL357', 'Peugeot', '208', 2018, 'Rojo', 82000, 'Personal', '2025-11-06 09:15:43', 1),
(30, 39, 'ADM258', 'Citroën', 'C4', 2021, 'Gris', 37000, 'Personal', '2025-11-06 09:16:53', 1),
(31, 40, 'ADO753', 'Toyota', 'yaris', 2019, 'Blanco', 49060, 'Personal', '2025-11-06 09:20:38', 1);

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `cliente`
--
ALTER TABLE `cliente`
  ADD PRIMARY KEY (`Id`);

--
-- Indices de la tabla `rol`
--
ALTER TABLE `rol`
  ADD PRIMARY KEY (`Id`),
  ADD UNIQUE KEY `Nombre` (`Nombre`);

--
-- Indices de la tabla `servicio`
--
ALTER TABLE `servicio`
  ADD PRIMARY KEY (`Id`);

--
-- Indices de la tabla `trabajo`
--
ALTER TABLE `trabajo`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `VehiculoId` (`VehiculoId`),
  ADD KEY `UsuarioId` (`UsuarioId`);

--
-- Indices de la tabla `trabajoservicio`
--
ALTER TABLE `trabajoservicio`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `TrabajoId` (`TrabajoId`),
  ADD KEY `ServicioId` (`ServicioId`);

--
-- Indices de la tabla `usuario`
--
ALTER TABLE `usuario`
  ADD PRIMARY KEY (`Id`),
  ADD UNIQUE KEY `Email` (`Email`);

--
-- Indices de la tabla `vehiculo`
--
ALTER TABLE `vehiculo`
  ADD PRIMARY KEY (`Id`),
  ADD UNIQUE KEY `Patente` (`Patente`),
  ADD KEY `ClienteId` (`ClienteId`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `cliente`
--
ALTER TABLE `cliente`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=41;

--
-- AUTO_INCREMENT de la tabla `rol`
--
ALTER TABLE `rol`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT de la tabla `servicio`
--
ALTER TABLE `servicio`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=15;

--
-- AUTO_INCREMENT de la tabla `trabajo`
--
ALTER TABLE `trabajo`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=13;

--
-- AUTO_INCREMENT de la tabla `trabajoservicio`
--
ALTER TABLE `trabajoservicio`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=15;

--
-- AUTO_INCREMENT de la tabla `usuario`
--
ALTER TABLE `usuario`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT de la tabla `vehiculo`
--
ALTER TABLE `vehiculo`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=32;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `trabajo`
--
ALTER TABLE `trabajo`
  ADD CONSTRAINT `trabajo_ibfk_1` FOREIGN KEY (`VehiculoId`) REFERENCES `vehiculo` (`Id`) ON DELETE CASCADE,
  ADD CONSTRAINT `trabajo_ibfk_2` FOREIGN KEY (`UsuarioId`) REFERENCES `usuario` (`Id`);

--
-- Filtros para la tabla `trabajoservicio`
--
ALTER TABLE `trabajoservicio`
  ADD CONSTRAINT `trabajoservicio_ibfk_1` FOREIGN KEY (`TrabajoId`) REFERENCES `trabajo` (`Id`) ON DELETE CASCADE,
  ADD CONSTRAINT `trabajoservicio_ibfk_2` FOREIGN KEY (`ServicioId`) REFERENCES `servicio` (`Id`);

--
-- Filtros para la tabla `vehiculo`
--
ALTER TABLE `vehiculo`
  ADD CONSTRAINT `vehiculo_ibfk_1` FOREIGN KEY (`ClienteId`) REFERENCES `cliente` (`Id`) ON DELETE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
