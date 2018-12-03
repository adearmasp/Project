1-He creado 2 librerias con el objetivo de separar el modelo de datos y el trabajo con entity framawork
con el objetivo de poder reutilizar recurso en la posterioridad, y para crear poca dependencia.


1.1-En Store se encuentra los modelos y las interfaces que contienen los métodos a implementar 
para la inyección de dependencias.

1.2-En Store.Data se encuentra las 2 clases que implementan a las interfaces creadas en la librería Store y el manejo 
de entity framework

2.0- El proyecto Store.API evidentemente es la api que esta implementada

3.0 El proyecto WebStore es una app web que consume la api atravez de un servicio, aca aún queda un poco de trabajo, 
por realizar lo relacionado con la autenticación, permisos de roles, unos detalles con la foto,
la validación de propiedades con mayor rigurosidad, y el trabajo del consumo de la api, solo he implementado un método, 
en CustomerController el CustomerList que ya esta consumiendo de la api.