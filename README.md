LogiDispatchWeb � Interfaz Web MVC para Gesti�n de �rdenes
===========================================================

Descripci�n
-----------
Este proyecto representa la capa de presentaci�n (frontend) del sistema LogiDispatch. Est� desarrollado en ASP.NET Core MVC 9, y se conecta al backend desacoplado LogiDispatchAPI, que expone todos los servicios REST necesarios para operar el flujo de �rdenes de despacho en una empresa log�stica.

La decisi�n de implementar esta soluci�n como una aplicaci�n web independiente responde a la necesidad de estructurar una arquitectura orientada a servicios desacoplados, facilitando su evoluci�n futura, escalabilidad, mantenibilidad y despliegue en entornos cloud.

Sitio en producci�n:
https://pruebatecnica12.azurewebsites.net/

Swagger del API conectado:
https://logidispatch-api-cap-dev.agreeablebush-c6fa1930.southcentralus.azurecontainerapps.io/swagger/index.html

Decisi�n tecnol�gica: .NET 9
----------------------------
Se opt� por usar .NET 9 (versi�n STS) ya que permite:
- Mejorar el rendimiento y tiempos de respuesta del frontend.
- Agilizar procesos de customizaci�n visual o funcional en el corto plazo.
- Disfrutar de las ventajas de la evoluci�n de ASP.NET Core con mejoras en seguridad, estabilidad, rendimiento y compatibilidad.
- Potenciar la interfaz sin afectar el core l�gico del negocio alojado en el backend.

.NET 9 brinda una base moderna para aplicar cambios iterativos r�pidos, especialmente �til para MVPs o pruebas t�cnicas.

Tecnolog�as utilizadas
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
- Eliminar �rdenes mediante soft delete (eliminaci�n l�gica sin p�rdida de datos).
- Restaurar �rdenes previamente eliminadas.
- Visualizar todas las �rdenes activas por filtros de estado o eliminadas por cliente.
- Ver detalle completo de una orden, incluyendo distancia, costo y ubicaci�n geogr�fica.
- Renderizado visual de la ruta de la orden en un mapa interactivo (Leaflet).
- Reporte web resumido por cliente, agrupando �rdenes por intervalos de distancia definidos.
- Exportaci�n del reporte resumido a archivo Excel.
- Reporte detallado por cliente con los �tems y cantidades de cada orden.

Instrucciones para levantar el proyecto localmente
--------------------------------------------------

1. Clona el repositorio (rama main):

   git clone -b main https://github.com/CaportilloC/LogiDispatchWeb.git

2. Accede al directorio del proyecto:

   cd LogiDispatchWeb

3. Restaura dependencias y ejecuta la aplicaci�n:

   dotnet restore
   dotnet run

4. Accede a la aplicaci�n local en tu navegador:

   https://localhost:7290/ (o el puerto que aplique en tu caso)

Notas adicionales
-----------------
- La arquitectura desacoplada permite reemplazar o mejorar visualmente este frontend sin afectar la l�gica de negocio.
- El proyecto est� actualmente desplegado en un App Service de Azure en ambiente productivo.
- Est� preparado para integrarse con pipelines de CI/CD, aunque por razones de tiempo, no se alcanz� a implementar la automatizaci�n completa en esta versi�n.

Autor
-----
Desarrollado por Christian Alexander Portillo � CAP / Offzet  
Desarrollador de Software FullStack SemiSenior  
GitHub: https://github.com/CaportilloC
