# TecnoGas Hogar

## Producto

Prototipo de portal interno para **TecnoGas Hogar**, empresa peruana de mantenimiento e
instalación de artefactos a gas en el hogar. El personal de atención registra las solicitudes
de servicio de los clientes y las consulta en un listado centralizado.

## Registro

- **Register:** app UI interna (herramienta de administración). El diseño SIRVE al producto.
- **Audiencia:** personal de atención interno, uso diario en escritorio y móvil.
- **Mood:** confiable, limpio, eficiente. Azul corporativo (gas/energía) con acento ámbar.

## Alcance del prototipo

- Registrar una solicitud de servicio (Insert).
- Listar solicitudes registradas (Select).
- Persistencia en SQLite con Entity Framework Core.
- Sin autenticación. Sin Update/Delete.

## Página principal (superficie en foco)

Listado de solicitudes (`/SolicitudServicio`) y formulario de registro
(`/SolicitudServicio/Create`).
