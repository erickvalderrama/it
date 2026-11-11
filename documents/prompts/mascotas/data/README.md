# Catálogo colombiano DIVIPOLA

- El conjunto DIVIPOLA Códigos municipios proviene del Departamento Administrativo Nacional de Estadística, DANE. Su ID en el Portal Nacional de Datos Abiertos es gdxc-w37w. [Página oficial](https://www.datos.gov.co/Mapas-Nacionales/DIVIPOLA-C-digos-municipios/gdxc-w37w)
- La fuente tiene corte del 30 de diciembre de 2024 según la descripción. El publicador actualizó las filas el 24 de enero de 2025. La exportación se descargó el 9 de noviembre de 2026. [JSON oficial completo](https://www.datos.gov.co/api/views/gdxc-w37w/rows.json?accessType=DOWNLOAD)
- El conjunto y sus CSV derivados tienen licencia Creative Commons Atribución-CompartirIgual 4.0 Internacional. Los metadatos del portal la identifican como CC_40_BY_SA. [Licencia CC BY-SA 4.0](https://creativecommons.org/licenses/by-sa/4.0/)
- Para generar los archivos, se extrajeron los nombres, códigos y tipos territoriales, se agregó el país CO y se ordenaron los registros por código. El listado departamental agrupa las localidades por código oficial. Se conservan la escritura y las tildes de la fuente. Las coordenadas y los metadatos internos del portal se excluyen de los CSV.

| Archivo | Filas de datos | Uso |
| --- | --- | --- |
| colombia-departments.csv | 33 | Códigos de nivel departamental, con Bogotá D. C. |
| colombia-divipola-localities.csv | 1.122 | 1.103 registros de tipo Municipio, una Isla y 18 Áreas no municipalizadas según la fuente. |

Los CSV usan UTF-8. Importar department_code y locality_code como texto de dos y cinco caracteres respectivamente. Conservar los ceros iniciales al abrirlos en hojas de cálculo. Ejemplos verificados en la exportación incluyen Barranquilla 08001, Bogotá D. C. 11001 y Santa Marta 47001.

Integrar el catálogo por identificador de país y código oficial según el modelo aprobado. Configurar las ubicaciones elegibles para registro y publicación conservando su tipo. El backend valida la relación de país, departamento y localidad. Una actualización del catálogo conserva la trazabilidad de cuentas y anuncios asociados a códigos anteriores.

Las 1.122 filas consecutivas del JSON oficial se procesaron con códigos de localidad únicos. Cada código de localidad coincide con el prefijo del departamento y cada código departamental se asocia a un único nombre en la exportación.
