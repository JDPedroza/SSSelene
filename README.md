# SSSelene
# Prueba de Desarrollo: Sistema de Gestión de Ventas
Este repositorio contiene el código fuente y la documentación correspondiente a la prueba de desarrollo para la creación de un Sistema de Gestión de Ventas para una empresa de productos. El sistema ha sido implementado utilizando C# .NET y una base de datos SQL Server para el almacenamiento de datos. Además, se incluye información sobre cómo instalar y ejecutar la aplicación en IIS localmente.

# Caso de Uso
La empresa requiere un software que permita a los usuarios realizar las siguientes funciones:

# Usuario Administrador
Ingresar, editar, visualizar y eliminar productos.
Visualizar las ventas de todos los vendedores, filtrando por intervalos de fecha.
Realizar consultas SQL específicas.
# Usuario Vendedor
Registrar ventas, incluyendo información sobre productos, cantidad, impuesto, fecha y precio final.
Funcionalidades Implementadas
Gestión de Productos:

Los usuarios administradores pueden agregar, editar, ver y eliminar productos.
La pantalla de productos solicita información como Nombre, Código, Valor, Fecha de Creación, Proveedor y Foto del Producto.
Registro de Ventas:

Los usuarios vendedores pueden registrar ventas.
La pantalla de ventas solicita información como Nombre del Producto, Cantidad, Valor del Impuesto, Fecha de la Venta.
El precio final se calcula automáticamente (Cantidad * Precio).
Se registra el ID del vendedor que realizó la venta.
Visualización de Ventas:

Los usuarios administradores pueden ver las ventas de todos los vendedores.
La pantalla de ventas permite filtrar por intervalos de fecha (Fecha Inicio y Fecha Fin).
Gestión de Proveedores:

Se mantiene una tabla de proveedores con campos como Nombre, NIT, Teléfono, Celular y Email.
Consultas SQL:

Se han implementado las consultas solicitadas en SQL:
Ordenar vendedores con mejores ventas en un intervalo de fecha.
Listar productos más vendidos en un intervalo de fecha.
Pantalla de información detallada del producto, incluyendo detalles del proveedor.
Instalación y Ejecución
Para ejecutar la aplicación en el servidor IIS local, sigue estos pasos:

1. Clona este repositorio en tu máquina local.
2. Abre la solución en Visual Studio.
3. Configura la conexión a la base de datos SQL Server en el archivo de configuración.
4. Compila la solución para generar el ejecutable.
5. Publica la aplicación en IIS siguiendo los pasos adecuados para tu versión de IIS.
6. Accede a la aplicación a través de tu navegador web.
