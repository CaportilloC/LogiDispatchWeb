LogiDispatchWeb – Interfaz Web MVC para Gestión de Órdenes
===========================================================

Descripción
-----------
Este proyecto representa la capa de presentación (frontend) del sistema LogiDispatch. Está desarrollado en ASP.NET Core MVC 9, y se conecta al backend desacoplado LogiDispatchAPI, que expone todos los servicios REST necesarios para operar el flujo de órdenes de despacho en una empresa logística.

La decisión de implementar esta solución como una aplicación web independiente responde a la necesidad de estructurar una arquitectura orientada a servicios desacoplados, facilitando su evolución futura, escalabilidad, mantenibilidad y despliegue en entornos cloud.

Sitio en producción:
https://pruebatecnica12.azurewebsites.net/

Swagger del API conectado:
https://logidispatch-api-cap-dev.agreeablebush-c6fa1930.southcentralus.azurecontainerapps.io/swagger/index.html

Decisión tecnológica: .NET 9
----------------------------
Se optó por usar .NET 9 (versión STS) ya que permite:
- Mejorar el rendimiento y tiempos de respuesta del frontend.
- Agilizar procesos de customización visual o funcional en el corto plazo.
- Disfrutar de las ventajas de la evolución de ASP.NET Core con mejoras en seguridad, estabilidad, rendimiento y compatibilidad.
- Potenciar la interfaz sin afectar el core lógico del negocio alojado en el backend.

.NET 9 brinda una base moderna para aplicar cambios iterativos rápidos, especialmente útil para MVPs o pruebas técnicas.

Tecnologías utilizadas
----------------------
- ASP.NET Core MVC 9
- Bootstrap 5
- Leaflet.js (para renderizar mapas)
- SweetAlert2
- C# 12

Funcionalidades disponibles
---------------------------
- Crear una nueva orden indicando cliente, productos, cantidades y coordenadas de origen/destino.
- Actualizar una orden existente, permitiendo modificar productos, cantidades, coordenadas y status.
- Eliminar órdenes mediante soft delete (eliminación lógica sin pérdida de datos).
- Restaurar órdenes previamente eliminadas.
- Visualizar todas las órdenes activas por filtros de estado o eliminadas por cliente.
- Ver detalle completo de una orden, incluyendo distancia, costo y ubicación geográfica.
- Renderizado visual de la ruta de la orden en un mapa interactivo (Leaflet).
- Reporte web resumido por cliente, agrupando órdenes por intervalos de distancia definidos.
- Exportación del reporte resumido a archivo Excel.
- Reporte detallado por cliente con los ítems y cantidades de cada orden.

Instrucciones para levantar el proyecto localmente
--------------------------------------------------

1. Clona el repositorio (rama main):

   git clone -b main https://github.com/CaportilloC/LogiDispatchWeb.git

2. Accede al directorio del proyecto:

   cd LogiDispatchWeb

3. Restaura dependencias y ejecuta la aplicación:

   dotnet restore
   dotnet run

4. Accede a la aplicación local en tu navegador:

   https://localhost:7290/ (o el puerto que aplique en tu caso)

Notas adicionales
-----------------
- La arquitectura desacoplada permite reemplazar o mejorar visualmente este frontend sin afectar la lógica de negocio.
- El proyecto está actualmente desplegado en un App Service de Azure en ambiente productivo.
- Está preparado para integrarse con pipelines de CI/CD, aunque por razones de tiempo, no se alcanzó a implementar la automatización completa en esta versión.

Autor
-----
Desarrollado por Christian Alexander Portillo – CAP / Offzet  
Desarrollador de Software FullStack SemiSenior  
GitHub: https://github.com/CaportilloC
