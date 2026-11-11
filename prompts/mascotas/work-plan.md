# Plan de trabajo de Mascotas

Decisiones registradas el 9 de noviembre de 2026. Proyecto para un primer lanzamiento en Colombia con Flutter para web, Android e iOS, Riverpod como opción inicial, C# con .NET y PostgreSQL. La memoria técnica conserva arquitectura limpia, código en inglés, textos base en constantes, pruebas unitarias, de integración, E2E y de regresión visual, seguridad, máximo 699 líneas por archivo de código y el modelo entidad-relación aprobado como regla de oro. [Memoria técnica](https://github.com/erickvalderrama/it/blob/main/prompts/Mascotas-CLAUDE.md)

## Cómo ejecutar el plan y pasarlo a otra IA

1. Leer el `CLAUDE.md` de Mascotas, este plan, la especificación de fase 1 y las instrucciones del repositorio. Al trasladar los archivos al proyecto de aplicación, usar `CLAUDE.md` en la raíz, `docs/planning/work-plan.md` para el avance y `docs/specifications/phase-1.md` para los contratos y pantallas. Usar esa copia del plan como registro activo.
2. Revisar el código, la rama, los cambios existentes y el registro de avance. Empezar por la primera tarea `[ ]` ejecutable de la fase solicitada. Seguir el orden de dependencias, conservar el trabajo existente y usar las convenciones del proyecto. Mantener la arquitectura, el modelo entidad-relación, los textos base en inglés, el límite de 699 líneas y las pruebas exigidas en la memoria.
3. Ejecutar la tarea por el flujo real que usará el usuario. Correr las comprobaciones aplicables y registrar el resultado con ruta, commit, comando, ejecución de CI o captura. Cambiar la casilla de `[ ]` a `[x]` solo al verificar el resultado. Actualizar el plan en el mismo cambio y continuar con la siguiente tarea disponible.
4. Resolver decisiones reversibles durante la ejecución y anotar las que afecten el resultado. Para una dependencia ausente, conservar `[ ]` y registrar qué falta y qué tarea afecta. Avanzar con las tareas independientes del alcance autorizado, usando fixtures o adaptadores de prueba cuando corresponda. Retomar la tarea bloqueada cuando esté disponible su dependencia.
5. Agotar el trabajo ejecutable antes de cerrar. Dejar los IDs completados, la evidencia, el siguiente paso y las dependencias que siguen pendientes. Agrupar al final únicamente las preguntas cuya respuesta desbloquea trabajo. Una IA que retome debe poder continuar con el plan, el código y la evidencia del repositorio.

## Infraestructura del proyecto

| Componente | Ubicación y conexión |
| --- | --- |
| API ASP.NET Core y trabajo en segundo plano | Imagen de aplicación .NET y ejecución en contenedores. Ejecutar los trabajos en el servicio de la API o en un worker de la misma aplicación según el diseño implementado. |
| Flutter web | Imagen con el build web y servidor de archivos estáticos. Configurar la URL pública de la API y el fallback de navegación para abrir enlaces profundos directamente. |
| Flutter Android e iOS | Builds para sus plataformas de distribución. Las apps se conectan a la misma API desplegada en contenedores. |
| PostgreSQL | Servidor existente, administrado fuera de Compose. La API y los procesos autorizados se conectan por host accesible desde el contenedor, puerto, base, usuario y configuración TLS del ambiente. |
| MinIO | Servicio externo en servidor propio. La API se conecta con endpoint, bucket o prefijo y credenciales de aplicación por ambiente. Guardar los objetos con acceso controlado y conservar en PostgreSQL las referencias según el modelo aprobado. |

Usar Dockerfiles y Docker Compose para arrancar los servicios de Mascotas. Inyectar la configuración sensible en ejecución y documentar variables con ejemplos sin valores reales. Probar DNS, red y TLS desde los contenedores en el ambiente autorizado. Usar una base y credenciales de pruebas aisladas en PostgreSQL y un bucket o prefijo de pruebas en MinIO. Ejecutar migraciones y restablecer fixtures solo sobre el destino de pruebas previsto o el destino expresamente autorizado. [Redes de Docker Compose](https://docs.docker.com/compose/how-tos/networking/) · [MinIO SDK para .NET](https://docs.min.io/aistor/developers/sdk/dotnet/)

## Orden de lanzamiento

| Fase | Resultado para el usuario | Operación de cobro |
| --- | --- | --- |
| 1. Adopciones | Buscar y publicar adopciones en varias ciudades, elegir un anuncio pagado y registrar una organización. Administrar revisión, precios y usuarios. | ePayco para publicaciones pagadas y mensualidades Business. Facturación electrónica de los servicios propios del operador. |
| 2. Tiendas y servicios | Comprar artículos con domicilio gestionado por la tienda y contratar baño, lavado o limpieza a domicilio o por reserva. | Cobro por pedido o reserva, recargas para compras y cargos pequeños y ePayco Payouts para abonos a terceros. |
| 3. Transporte A→B | Pedir un traslado de mascota, comparar ofertas, contratar a un Business autorizado y seguir el trayecto. | Pago del traslado y recargas para pagos o comisiones pequeñas, con abonos y conciliación por operación. |

El catálogo admite varias ciudades desde la fase 1. La salida comercial propuesta concentra la captación inicial en Santa Marta y permite probar los filtros en otros municipios habilitados.

### Canal para salir rápido

Para el piloto, se propone lanzar primero la web adaptable con ePayco para los cobros digitales de la fase 1. La misma interfaz Flutter se prepara para móvil. Antes de ofrecer destacados o acceso digital de pago dentro de las aplicaciones distribuidas por Apple y Google Play, revisar las reglas de compra y los canales permitidos para Colombia. Apple incluye los anuncios destacados que se muestran en la propia app dentro de su regla de compras digitales. Revisar por separado el cobro de productos y servicios físicos de las fases siguientes. [Apple App Review Guidelines](https://developer.apple.com/app-store/review/guidelines/) · [Política de pagos de Google Play](https://support.google.com/googleplay/android-developer/answer/9858738?hl=es)

## Calidad y criterios de salida

- Probar los recorridos críticos E2E desde Flutter hasta la API, PostgreSQL y el procesamiento en segundo plano. Usar datos ficticios y respuestas controladas de ePayco, facturación e IA en CI. Verificar las integraciones por separado en los entornos de prueba autorizados.
- Comparar capturas con referencias visuales revisadas para las pantallas clave en tamaños compacto, mediano y amplio. Revisar diferencias, desbordamientos, textos ampliados, navegación por teclado, áreas seguras, accesibilidad y estados de carga, vacío, error y éxito.
- Ejecutar E2E de web en CI al cambiar un flujo crítico y validar los flujos móviles relevantes en Android e iOS antes de publicar en esos canales. Cada fase requiere sus recorridos E2E y controles visuales aprobados, con evidencia de pruebas y revisión de los casos que requieran dispositivo.

## Fase 1. Adopciones

Usar la especificación de implementación para convertir las entregas en tareas, con contratos de API y datos, recorridos UI/UX, estados y casos de aceptación. Las rutas y los límites propuestos se ajustan al código existente y al modelo aprobado. [Especificación de fase 1](https://github.com/erickvalderrama/it/blob/main/prompts/mascotas/phase-1-specification.md)

### Entregas en orden de dependencia

| Entrega | Trabajo | Flujo E2E de salida |
| --- | --- | --- |
| 1A. Acceso y catálogo | Registro breve, Colombia preseleccionado, selección de municipio, roles, alta Business y aceptación de términos por versión. | Una persona se registra con Colombia preseleccionada, selecciona y cambia su ciudad, completa un alta Business y acepta una nueva versión de términos. |
| 1B. Publicar y revisar | Ficha de adopción con fotos, borradores, búsqueda, filtros por ciudad y origen, revisión por IA y cola manual. | Una publicación básica aprobada aparece con el filtro de ciudad y publicante. Se evalúan el texto y todas las fotos. Los casos dudosos llegan a Superadmin y el contenido sexual o vulgar se rechaza. |
| 1C. Cobro e historial | Versiones de precios, promoción Business, mensualidad, compra de anuncio pagado, ePayco y facturación electrónica. | Una compra de prueba conserva precio y términos aceptados. El pago confirmado envía el anuncio a revisión. Las correcciones se reenvían con el mismo pago. Se comprueban un solo cobro aplicado, la visibilidad tras aprobación y la factura, incluso con confirmaciones repetidas. |
| 1D. Operación y piloto | Panel con usuarios registrados y activos, administración de anuncios, cobros, facturas y reclamos. Prueba de interfaz web y móvil y piloto con organizaciones. | Superadmin puede moderar, consultar la operación y recuperar una factura pendiente. Se verifican dos ciudades, los tres roles y el aislamiento entre cuentas. Pasan las comparaciones visuales de las pantallas clave. |

### Preparación y materiales

- [x] **1.0-01** Versionar la memoria técnica de Mascotas y la especificación de fase 1 para el traspaso a desarrollo.
- [x] **1.0-02** Versionar los dos CSV DIVIPOLA y sus instrucciones de importación. Verificados 33 registros departamentales y 1.122 registros de localidades.
- [x] **1.0-03** Versionar los borradores colombianos de términos y privacidad para la revisión del operador.
- [ ] **1.0-04** Registrar la ruta del repositorio de aplicación y sus comandos de trabajo. Incorporar la memoria, el plan activo y la especificación a ese repositorio.
- [ ] **1.0-05** Registrar la ubicación y versión del modelo entidad-relación aprobado. Contrastar cuentas, ubicaciones, Business, listados, medios, consentimientos, precios, pagos y facturas antes de crear los mapeos o migraciones que los afectan.

### 1A.1 Arranque en contenedores, contratos y UI/UX

- [ ] **1A.1-01** Revisar la solución existente o establecer la estructura modular de .NET y Flutter. Fijar SDK, dependencias, versión de PostgreSQL del servidor y manejo de estado. Verificar compilación desde un checkout limpio.
- [ ] **1A.1-02** Crear los Dockerfiles de API y Flutter web y el Compose de los servicios de aplicación. Configurar servidor estático, rutas SPA y API pública. Comprobar `docker compose config`, `docker compose build`, `docker compose up -d` y `docker compose ps`.
- [ ] **1A.1-03** Documentar variables de cada ambiente, ejemplos sin secretos y forma de inyectarlas. Configurar PostgreSQL externo, MinIO, origen web y puertos. Probar que la aplicación detecta valores ausentes o inválidos con errores que protegen datos sensibles.
- [ ] **1A.1-04** Validar desde el contenedor la conexión a PostgreSQL con su rol autorizado y a MinIO con su cuenta de aplicación. En el espacio de pruebas, crear, leer y eliminar un objeto sintético. Registrar el destino por ambiente sin guardar secretos.
- [ ] **1A.1-05** Publicar OpenAPI, códigos de error y referencias de correlación. Implementar liveness del proceso y readiness con comprobaciones acotadas de sus dependencias. Verificar estado con servicios disponibles y con un fallo simulado.
- [ ] **1A.1-06** Preparar fixtures repetibles para Santa Marta y Barranquilla, tres roles, dos organizaciones, dos precios y dos términos. Ejecutar los resets sobre el destino de pruebas aislado tras validar el modelo aplicable. Configurar adaptadores de prueba para ePayco, IA, correo y factura.
- [ ] **1A.1-07** Esbozar búsqueda, ficha, registro, borrador, checkout y correcciones en tamaños compacto y amplio. Montar el tema, textos centralizados y componentes compartidos. Revisar jerarquía, acción principal, foco y estados vacíos con fixtures. [Paleta inicial y criterios UI/UX](https://github.com/erickvalderrama/it/blob/main/prompts/mascotas/phase-1-specification.md#uiux-design-and-usability)
- [ ] **1A.1-08** Configurar CI para compilar las imágenes, verificar formato y análisis y ejecutar pruebas unitarias y de widgets. Preparar el runner autorizado con acceso al PostgreSQL y MinIO de pruebas para integración y E2E.
- [ ] **1A.1-09** Ejecutar la salida del bloque desde un checkout limpio. Abrir Flutter web en contenedor, navegar a una ruta profunda y confirmar que consume la API. Guardar los comandos, resultados y una captura base.

### 1A.2 Registro, catálogo y consentimiento

- [ ] **1A.2-01** Implementar el importador DIVIPOLA según el modelo aprobado. Preservar país, departamento y localidad como texto y la elegibilidad del piloto por separado. Probar reimportación y que `08001` conserva el cero inicial.
- [ ] **1A.2-02** Implementar selector de país y ciudad con Colombia preseleccionada y ciudades agrupadas por departamento. Probar filtros, carga, vacío, error y rechazo de un municipio inválido o deshabilitado en el servidor.
- [ ] **1A.2-03** Implementar el registro de nombre, correo, contraseña, país y ciudad. Validar campos, unicidad de correo y sesión. Cubrir inicio de sesión, cierre, verificación previa a publicar y recuperación según el alcance del piloto.
- [ ] **1A.2-04** Permitir cambiar la ciudad de la cuenta. Validar permisos, mantener la ciudad de cada anuncio independiente y comprobar que el retorno tras iniciar sesión abre la ruta prevista.
- [ ] **1A.2-05** Implementar lectura de términos y privacidad y aceptación explícita de la versión efectiva. Registrar usuario, versión y fecha del servidor. Gestionar una versión publicada mientras la pantalla está abierta.
- [ ] **1A.2-06** Bloquear las nuevas operaciones protegidas hasta aceptar términos vigentes. Comprobar el acceso a historial, documentos y soporte y el vínculo de una operación previa con su aceptación original.
- [ ] **1A.2-07** Ejecutar E2E de registro, cambio de ciudad y actualización de términos. Guardar la prueba de validación en PostgreSQL y la revisión visual del formulario en web y móvil.

### 1A.3 Alta Business y promoción

- [ ] **1A.3-01** Implementar elección Foundation o Store en el alta o conversión. Mantener el ID y las operaciones de la cuenta. Probar ambos perfiles bajo el rol Business.
- [ ] **1A.3-02** Pedir cédula y RUT a Store de persona natural y NIT y RUT a Store de persona jurídica. Aplicar las verificaciones de representación que correspondan. Mostrar solo los requisitos del tipo elegido.
- [ ] **1A.3-03** Implementar carga y lectura autorizada de documentos privados en MinIO. Comprobar formatos, límites y permisos, incluida la denegación a otra organización y la ausencia de documentos en resultados públicos.
- [ ] **1A.3-04** Implementar revisión de la solicitud, estados y decisión auditada de Superadmin. Mostrar al usuario tareas pendientes y conservar la trazabilidad al habilitar Business.
- [ ] **1A.3-05** Implementar alta promocional con elegibilidad, inicio, vencimiento y duración configurable. Mostrar vigencia y beneficios. Probar vencimiento con reloj controlado y acceso a operaciones históricas.
- [ ] **1A.3-06** Ejecutar E2E de alta Foundation, conversión Store y aislamiento entre dos Business. Revisar el formulario y los estados de verificación en resoluciones compacta y amplia.

### 1B.1 Adopción básica de extremo a extremo

- [ ] **1B.1-01** Implementar borrador editable con datos de mascota, ciudad propia del anuncio y contexto de publicación individual u organización. Validar propietario y concurrencia de edición al guardar.
- [ ] **1B.1-02** Implementar carga de fotos a MinIO con formato y tamaño validados, orden y portada. Preparar derivados sin metadatos sensibles. Probar un fallo de una foto y su reintento sin perder las demás.
- [ ] **1B.1-03** Implementar vista previa con la proyección que será pública. Mostrar campos faltantes y el contexto del publicante. Verificar la visibilidad privada del borrador y sus fotos.
- [ ] **1B.1-04** Congelar una revisión completa al enviar una publicación básica. Ponerla en revisión, comprobar aprobación mediante el adaptador de prueba y activarla una sola vez con el identificador de revisión correcto.
- [ ] **1B.1-05** Implementar catálogo público paginado, filtros de ciudad y origen del publicante, ficha compartible y estados de carga, vacío y error. Probar acceso directo, recarga y restauración de filtros.
- [ ] **1B.1-06** Implementar el retiro de visibilidad según los estados aprobados. Verificar que el historial se conserva y que el catálogo y la ficha aplican la misma regla de acceso.
- [ ] **1B.1-07** Demostrar desde Flutter que una persona crea una adopción básica, carga fotos, la envía, la ve aprobada y la encuentra solo en los filtros que corresponden. Guardar el E2E y las capturas de borrador, ficha y resultados.

### 1B.2 Moderación, correcciones y contacto

- [ ] **1B.2-01** Implementar un contrato de moderación por revisión con texto, todas las fotos y texto visible en fotos. Enviar contenido saneado y registrar versión de reglas, entradas y resultados estructurados.
- [ ] **1B.2-02** Implementar resultados aprobado, correcciones requeridas, rechazado y revisión manual. Probar rechazo por contenido sexual o vulgar, imagen ausente y respuesta incierta o fallo persistente.
- [ ] **1B.2-03** Implementar corrección por campo o foto, nueva revisión y reenvío. Probar que una aprobación tardía de la revisión anterior conserva la privacidad de la revisión pendiente.
- [ ] **1B.2-04** Implementar cola de Superadmin para revisar la revisión exacta, decidir con motivo y registrar auditoría. Probar separación entre permisos de publicación y de moderación.
- [ ] **1B.2-05** Integrar el canal de contacto acordado para adopciones. Permitir que un visitante llegue a la acción y conservar el contexto si requiere iniciar sesión. Validar quién puede consultar y gestionar los datos de cada solicitud si se usa ese flujo.
- [ ] **1B.2-06** Ejecutar E2E de aprobado, corrección, rechazo y resolución manual. Comparar las capturas de cada estado y revisar las instrucciones del autor con usuarios del piloto.

### 1C.1 Cobros, precios y facturación con proveedores de prueba

- [ ] **1C.1-01** Implementar versiones inmutables de precios y ofertas para premium y renovación Business, incluidos los beneficios de importe cero. Conservar versión, moneda, total, vigencia y actor en cada aceptación.
- [ ] **1C.1-02** Mostrar total, duración, prestaciones, condiciones y versión aplicable antes de pagar. Crear la operación de checkout ligada a la revisión o período exacto. Probar un cambio de precio posterior y una oferta vencida.
- [ ] **1C.1-03** Implementar el adaptador de checkout ePayco y retorno al estado de la operación. Probar pago pendiente, cancelación o fallo de la sesión y retorno tras recargar el navegador.
- [ ] **1C.1-04** Verificar la confirmación ePayco en el servidor y conciliar referencia, importe y moneda. Probar firma inválida, duplicados concurrentes, eventos tardíos y el caso de un segundo cobro real que requiere conciliación.
- [ ] **1C.1-05** Enviar la revisión premium a moderación tras el pago confirmado. Activar su vigencia al aprobar y mantener pago y precio en correcciones. Probar que el reenvío conserva un solo cobro aplicado.
- [ ] **1C.1-06** Implementar renovación mensual manual de Business con la tarifa asignada. Verificar vigencia, pago confirmado, histórico de precios y aplicación idempotente a un solo período.
- [ ] **1C.1-07** Implementar captura de datos fiscales permitidos, emisión, entrega y consulta autorizada de factura. Probar error fiscal y reintento sin repetir el cobro ni cambiar la visibilidad del anuncio.
- [ ] **1C.1-08** Implementar devolución autorizada y ajuste fiscal según la política acordada. Conservar el vínculo con el pago y conciliar la operación original al reintentar.
- [ ] **1C.1-09** Ejecutar E2E de compra premium, corrección y factura, renovación Business y recuperación de fallos. Verificar en la base aislada un solo pago aplicado y los vínculos de revisión, precio, términos y factura.

### 1C.2 Pruebas de proveedores en entornos autorizados

- [ ] **1C.2-01** Configurar credenciales de prueba por ambiente, retorno web, confirmación ePayco y referencia de conciliación. Validar que el retorno y la confirmación se pueden recibir en cualquier orden.
- [ ] **1C.2-02** Ejecutar en el entorno autorizado de ePayco una compra, confirmación de servidor y conciliación de estado. Conservar evidencia redactada del resultado.
- [ ] **1C.2-03** Validar el emisor fiscal configurado y probar factura, consulta, error y ajuste aplicable en el entorno de pruebas disponible.
- [ ] **1C.2-04** Validar el proveedor de IA con texto, fotos y texto visible en imágenes. Comprobar resultados completos, timeout y escalamiento manual con material de prueba.
- [ ] **1C.2-05** Ensayar la recuperación de un pago o factura pendiente mediante acciones auditadas. Verificar que el reintento conserva la referencia original y la evidencia del proveedor.
- [ ] **1C.2-06** Registrar el resultado por proveedor, configuración revisada y dependencias pendientes de operación real. Confirmar la política de devoluciones y reglas fiscales antes de habilitar cobros comerciales.

### 1D Operación, pruebas y salida del piloto

- [ ] **1D-01** Completar panel Superadmin con usuarios registrados, habilitados y activos, filtros por rol, ciudad y período y definición visible de la métrica. Probar permisos y consistencia de los conteos.
- [ ] **1D-02** Completar colas de revisión, altas Business, precios y términos, pagos, facturas y devoluciones. Probar auditoría, búsqueda de la operación y recuperación con datos privados protegidos.
- [ ] **1D-03** Completar datos del operador, contacto, privacidad y términos para la publicación del piloto. Verificar la versión visible y su vínculo con los consentimientos y las operaciones.
- [ ] **1D-04** Ejecutar la matriz de pruebas unitarias, integración con PostgreSQL y MinIO, widgets y E2E con tres roles y dos ciudades. Guardar resultados, cobertura de fallos y evidencias de cada flujo crítico.
- [ ] **1D-05** Revisar pantallas clave en tamaños compacto, mediano y amplio. Validar teclado, ampliación de texto, contraste, lector de pantalla, navegación y diferencias visuales. Corregir problemas que impidan completar una tarea.
- [ ] **1D-06** Reproducir en staging el despliegue de API y web desde las imágenes verificadas. Ensayar migración, restauración y recuperación en el entorno de pruebas. Validar liveness, readiness, conectividad externa, alertas y rollback.
- [ ] **1D-07** Completar el piloto con organizaciones iniciales y publicaciones autorizadas. Medir tiempo a primera publicación, búsquedas sin resultados y conversión a destacado. Registrar problemas de usabilidad y su resolución.
- [ ] **1D-08** Verificar la salida web de fase 1 con E2E, regresión visual, soporte y operación de cobro disponibles para el alcance habilitado. Antes de liberar Android o iOS, ejecutar sus pruebas y confirmar el canal de cobro aplicable.
- [ ] **1D-09** Registrar versión, commit, resultados, decisiones y dependencias restantes. Actualizar la siguiente tarea de la fase autorizada conforme a los resultados del piloto.

### Registro, ciudades y roles

- El registro personal pide nombre, correo, contraseña, país y ciudad. Colombia aparece preseleccionada como país habilitado. El selector agrupa ciudades por departamento y guarda su código DIVIPOLA. Para el piloto, se propone verificar el correo antes de publicar y ofrecer recuperación de contraseña.
- Guardar país, región y municipio como referencias del catálogo del modelo aprobado. Usar los códigos oficiales como texto para conservar ceros iniciales. La ciudad del anuncio puede diferir de la ciudad del perfil cuando corresponde a la ubicación de la mascota.
- Importar los dos CSV versionados, conservar los nombres y tipos de DANE y controlar cuáles ubicaciones se muestran en el selector. Actualizar el catálogo por código y versión de fuente para preservar las referencias históricas. Colombia permanece como país operativo de esta fase. Habilitar otros países por configuración y por catálogo.
- Usar los roles **User**, **Business** y **Superadmin**. Business cubre tanto fundaciones como tiendas y, en fase 3, transportistas habilitados. **Foundation** y **Store** son tipos de perfil, con capacidades autorizadas por recurso.
- En el alta Business y en la conversión de una cuenta existente, la primera elección es **tienda** o **fundación**. Conservar la misma cuenta y sus operaciones. Otorgar acceso Business después de las verificaciones que correspondan. Una fundación puede operar sin tienda y solicitar la capacidad de tienda posteriormente.
- Al inscribirse como tienda o habilitar tienda desde un perfil, pedir tipo de persona y los soportes solicitados. Persona natural presenta cédula y RUT. Persona jurídica presenta NIT y RUT. Validar representación cuando proceda. Proteger documentos y limitar su consulta a personas autorizadas. La verificación se activa al solicitar tienda, con el registro personal separado de este trámite.
- En fase 1 se permite inscribir organizaciones y preparar perfiles de tienda para la promoción. La venta de artículos y las reservas de servicios se habilitan en fase 2. Superadmin gestiona altas, revisión, precios, términos y acceso mediante permisos y auditoría.

Catálogo oficial descargado y validado el 9 de noviembre de 2026. Incluye 33 códigos de nivel departamental, con Bogotá D. C., y 1.122 registros territoriales. Su corte es el 30 de diciembre de 2024. [Departamentos](https://github.com/erickvalderrama/it/blob/main/prompts/mascotas/data/colombia-departments.csv) · [Localidades DIVIPOLA](https://github.com/erickvalderrama/it/blob/main/prompts/mascotas/data/colombia-divipola-localities.csv) · [Fuente, licencia e importación](https://github.com/erickvalderrama/it/blob/main/prompts/mascotas/data/README.md)

### Publicaciones y revisión

- Permitir publicar y pagar una publicación de adopción destacada a **Superadmin**, **User** y **Business**. El publicante paga a Mascotas por el anuncio o su visibilidad durante el plazo informado. La decisión de adopción y la entrega de la mascota las gestiona quien ofrece la adopción.
- Guardar el origen del anuncio como cuenta personal u organización en el momento de publicar. El listado filtra por ciudad y por particular o Business, incluida fundación. Una conversión posterior a Business conserva el origen de los anuncios previos. Mostrar anunciante, estado del anuncio y vigencia del destacado.
- Preparar una ficha pública de la mascota con fotos, especie, ubicación aproximada, requisitos de adopción y canal de solicitud. Separar los datos públicos de direcciones, documentos y datos de contacto reservados.
- **Cobrar antes de revisar el anuncio premium.** Mostrar precio, vigencia, reglas de publicación y condiciones de devolución en el resumen. Cuando el servidor confirme el pago de ePayco, enviar la versión exacta del anuncio a revisión y mostrar su estado. Una publicación básica se envía a revisión al completar su registro de publicación.
- La IA revisa **el texto, todas las imágenes y el texto visible dentro de ellas**. Usar una API aprobada, como OpenAI API, con un clasificador multimodal y las comprobaciones adicionales que necesiten las reglas de Mascotas. Validar cobertura de la política sexual para cada modalidad, revisar cada foto por separado y enviar a revisión manual si falta cobertura. Enviar solo contenido necesario, con metadatos retirados. Mantener documentos, teléfonos, direcciones y datos de facturación fuera de la revisión. [Documentación de moderación de OpenAI](https://developers.openai.com/api/docs/guides/moderation)

| Resultado de la revisión | Acción |
| --- | --- |
| Aprobado | La IA aprueba cuando el texto y todas las fotos pasan las reglas y están completos. En premium, publicar el anuncio y empezar a contar la vigencia del destacado al activarlo con pago confirmado. |
| Correcciones requeridas | Enviar el motivo, el campo o número de foto y cómo corregirlo. El publicante edita y reenvía con el mismo pago. Revisar todo el contenido de la nueva versión. |
| Rechazado | La IA rechaza la versión ante evidencia clara de vulgaridad, cualquier contenido sexual en texto o imágenes, u otra prohibición de la política. Mantener esa versión oculta, comunicar el motivo y permitir corregir cuando proceda. |
| Revisión manual | Superadmin decide si la IA tiene dudas, una imagen no se pudo analizar, las respuestas se contradicen o hay un fallo persistente. Registrar la decisión y su motivo. |

- Mantener el anuncio nuevo fuera de los resultados hasta que esté aprobado y activo. Las correcciones conservan el pago, el precio y la versión legal de la compra. Repetir el análisis en cada nueva versión e impedir que una respuesta tardía active una versión anterior. Registrar versión de contenido y reglas, evidencia mínima del resultado por texto y foto, modelo, fecha y resolución. Evitar reproducir material prohibido en notificaciones y limitar su acceso.
- Para el piloto, se propone devolver íntegramente el cobro si el anuncio se cierra con rechazo definitivo antes de activarse. Tramitar la devolución por el canal aplicable y registrar el ajuste fiscal correspondiente. Confirmar con ePayco qué medios admiten reversión en plataforma y preparar una devolución conciliada por los canales habilitados para los demás. Informar antes de pagar la política, plazos de corrección y devolución. [Condiciones de ePayco](https://epayco.com/terminos-y-condiciones-generales-2026/) · [Medios de pago y reversiones](https://ayuda.epayco.com/en/articles/2018-a-qu-mtodos-de-pago-se-les-puede-hacer-reversiones-con-epayco)

### Alta promocional y mensualidad Business

- Inscribir gratis a organizaciones, fundaciones y futuros comercios elegibles. Configurar inicio, fin, alcance y elegibilidad de la promoción. **Un año gratis** es la duración de ejemplo y se administra como configuración. Para el piloto, se propone contar la promoción desde que se habilita el perfil Business.
- Mostrar fecha de fin, condiciones y precio mensual asignado al ofrecer la renovación. Cobrar la mensualidad cuando el Business acepte el precio y confirme el pago. Se propone comenzar con renovación mensual manual mediante ePayco. El alta gratuita y el fin del beneficio se registran como eventos de la membresía.
- Mantener separados la membresía Business y el cobro de cada anuncio destacado. Cuando un plan incluya un beneficio de publicación, aplicar sus condiciones al calcular el importe final. Definir los permisos que dependen de la membresía vigente y mantener disponibles los trámites y obligaciones de operaciones anteriores.

### Historial de precios y relación con el importe aceptado

Usar una **tabla independiente de versiones de precio** en el modelo entidad-relación. Los nombres de esta sección describen el modelo lógico para revisar. La estructura física se define en el modelo aprobado.

| Concepto lógico | Qué registra |
| --- | --- |
| PriceVersion | Concepto, monto, moneda, ámbito o Business asignado, vigencia, versión y autor del cambio. Conceptos iniciales de publicación y mensualidad. |
| BusinessMembership | Business, vigencia de membresía, beneficio promocional y referencia de la tarifa que se ofrece para el siguiente período. |
| PriceAcceptance | Usuario, operación, versión exacta de tarifa, total informado, moneda, componentes y fecha de aceptación. |
| Payment, publicación y factura | Referencia de la aceptación y del importe aplicable. Vincular la publicación o vigencia que activa el pago confirmado y su factura. |

Crear una versión nueva al cambiar un monto. Conservar la versión y el total aceptados para cada publicación o pago, incluido el precio cero o el descuento de una oferta gratuita. Permitir consultar el último precio aceptado por usuario y concepto. Mantener referencias de las operaciones previas al cambiar precios. El servidor valida el importe en el momento de la aceptación y lo congela durante la vigencia informada de la oferta. Si vence antes de completar el checkout, solicitar aceptación del nuevo resumen y reconciliar cualquier confirmación tardía. Aplicar la versión gratuita o el beneficio promocional cuando corresponda.

### ePayco y facturación

- Integrar ePayco por el backend. Al verificar una confirmación, comparar firma o mecanismo documentado, referencia, estado, moneda e importe contra la operación guardada. Aplicar el resultado de forma idempotente. La pantalla consulta el estado confirmado en el servidor. [Confirmación de ePayco](https://docs.epayco.com/docs/checkout-respuesta-y-confirmacion)
- En el checkout de un anuncio pagado solicitar los datos fiscales aplicables. Para una factura a nombre del comprador, usar nombre o razón social, tipo y número de identificación y correo cuando elija esa entrega. Aplicar la consulta DIAN y sus alternativas, incluida la facturación a consumidor final cuando corresponda, según la habilitación del facturador. Teléfono y dirección se gestionan como datos separados cuando tienen una finalidad de contacto o prestación y con sus permisos. La expedición de la factura se rige por los datos permitidos por DIAN. [Resolución DIAN 227 de 2025](https://normograma.dian.gov.co/dian/compilacion/docs/resolucion_dian_0227_2025.htm) · [Datos del adquirente](https://www.dian.gov.co/Prensa/Paginas/NG-Comunicado-de-Prensa-026-2025.aspx)
- Implementar facturación electrónica de los cobros propios de Mascotas, incluidos anuncios y mensualidad. Definir emisor, concepto, impuestos aplicables y momento de expedición con el responsable fiscal. Usar el canal de facturación DIAN o un proveedor habilitado elegido para el proyecto. Registrar referencia fiscal, CUFE cuando corresponda, estado de validación, entrega, reintentos y correcciones o notas que procedan. La constancia de ePayco respalda el pago y se vincula con el documento fiscal.
- Mostrar cobros pendientes, aprobados y por conciliar y permitir recuperar la factura desde la operación. Una falla fiscal conserva el pago registrado y genera un trámite de resolución. Mostrar devolución pendiente o completada cuando aplique y conciliarla con su nota o ajuste fiscal. La habilitación comercial de cobros requiere confirmar configuración de ePayco y operación de facturación en el ambiente correspondiente.

### Términos, privacidad y panel

- Versionar términos y política con texto, idioma, versión y vigencia. Registrar usuario, documento exacto y fecha de aceptación, y autorización de datos cuando corresponda. Conservar la historia y la relación con la versión más reciente que aceptó cada usuario.
- Al iniciar sesión o abrir un flujo que requiere aceptación, mostrar el documento vigente cuando haya una versión nueva. Validar la versión en el backend antes de permitir nuevas publicaciones, contratación o pagos. Mantener consulta de documentos, historial y canales de atención. Cada operación conserva el texto aplicable al momento de su aceptación.
- Los términos identifican al publicante, tienda o prestador responsable. El publicante responde por datos y contenido del anuncio, la tienda por pedidos y entregas y el prestador por su ejecución. Mascotas cobra por sus funciones y atiende sus obligaciones de publicación, cobro, datos y soporte según su participación efectiva. Aplicar derechos obligatorios y reclamos sin exoneraciones generales. [Términos de Mascotas](https://github.com/erickvalderrama/it/blob/main/prompts/mascotas/legal/terms-and-conditions.md) · [Privacidad](https://github.com/erickvalderrama/it/blob/main/prompts/mascotas/legal/privacy-policy.md) · [Estatuto del Consumidor](https://www1.funcionpublica.gov.co/eva/gestornormativo/norma_pdf.php?i=44306) · [Criterio de la SIC sobre portales de contacto](https://sedeelectronica.sic.gov.co/publicaciones/boletin-juridico/concepto/redes-sociales-como-portales-de-contacto)
- Panel Superadmin con registrados, habilitados y activos, filtros por ciudad, período y rol, cola de revisión, publicaciones vigentes, altas Business, vencimientos, precios y operaciones de pago y factura. **Activos en 30 días** es la métrica inicial propuesta de cuentas con una autenticación exitosa o actividad acreditada en el período. Mostrar la definición junto al contador.
- Probar el flujo de registro y aprobación, aislamiento de cuentas, validación de municipio, aceptación tras cambio de términos, historial ante cambio de tarifa, confirmación de pago repetida, revisión de todas las imágenes, rechazo sexual o vulgar, correcciones sin segundo cobro y devolución por rechazo definitivo. Validar unidades en C# y Dart, permisos y persistencia en integración y pantallas web y móvil.

### Ideas pequeñas para acelerar el piloto

1. Ficha pública con filtro por ciudad y enlace compartible por WhatsApp y otros canales. Pedir cuenta al publicar o tramitar una solicitud, según el flujo.
2. Botón para marcar adoptado y fecha de última actualización. Retirar anuncios vencidos de los resultados activos y conservar su historial.
3. Reporte de anuncio con motivo y cola de revisión del mismo panel. Reutilizar el flujo manual de moderación.
4. Alta asistida de las primeras fundaciones y promoción administrada desde el panel. Usar publicaciones autorizadas para comprobar búsquedas reales.
5. Vista previa de ficha y resumen del precio antes de aceptar. Medir tiempo hasta primera publicación, solicitudes de adopción y conversiones a destacado.

Priorizar los puntos que mejoren publicación, búsqueda y revisión. Los extras entran por separado después de comprobar el recorrido completo de registro a anuncio visible y factura.

## Fase 2. Tiendas y servicios para mascotas

- Habilitar el catálogo, stock, pedidos y domicilio propio de Business verificados como tienda. La confirmación identifica vendedor, costo de entrega, plazo, importe de Mascotas y parte que corresponde a la tienda.
- Permitir baño, lavado y limpieza a domicilio o por reserva con prestador, ubicación, fecha, cupo, precio y condiciones. Confirmar disponibilidad y pago, tratar expiraciones y cancelaciones y conciliar reembolsos.
- Activar ePayco Payouts para los abonos a terceros admitidos por la cuenta y el contrato. Verificar beneficiarios, titularidad, monto, comisión, estado, reintentos y conciliación. Separar el cobro en checkout del abono posterior al prestador. [ePayco Payouts](https://docs.epayco.com/docs/qu%C3%A9-es-epayco-payouts) · [Condiciones de ePayco](https://epayco.com/terminos-y-condiciones-generales-2026/)
- Usar recargas para pagos pequeños. Separar el saldo del cliente destinado a compras del saldo de comisiones de Business y de las obligaciones por pagar a terceros. Documentar finalidad, vencimiento si aplica, devoluciones y costo de recaudo y abono. La puesta en marcha requiere validar admisión del proveedor y tratamiento del dinero, facturación y reglas de recarga. [Diseño de recargas](https://github.com/erickvalderrama/it/blob/main/investigaciones/Mascotas-ManosDelBarrio-recargas-2026-11-06.md)
- La fase queda lista al completar pruebas E2E de un pedido y una reserva con cobro, factura o soporte aplicable, entrega o servicio, comisión, abono y reembolso conciliados. Cubrir conflictos de stock o cupo y reintentos de recarga y abono, según el alcance habilitado.

## Fase 3. Transporte de mascotas A→B

- Agregar la capacidad de transportista a Business tras la verificación aplicable. Recoger origen A, destino B, mascota, condiciones, hora, responsables, presupuesto y ofertas. El cliente selecciona prestador y acepta alcance y total.
- Confirmar recogida y entrega. Durante el traslado, compartir ubicación con las partes autorizadas y detener la recolección al cerrar o cancelar. Probar segundo plano, bloqueo de pantalla, permisos, mala señal y último punto recibido.
- Integrar cobros del viaje y recargas para pagos o comisiones pequeñas. Mantener separados precio aceptado, pago del cliente, remuneración del transportista, cargo de Mascotas y abono conciliado. Incorporar cancelaciones, incidentes y atención documentada.
- La prueba E2E de salida cubre solicitud, contraoferta, aceptación, cobro, recogida, entrega y conciliación. Verificar también permisos denegados, pérdida de conexión y recuperación del seguimiento autorizado.

### Validación legal previa a viajes pagados en Colombia

La Ley 336 distingue el transporte privado de necesidades propias y la prestación de servicios de transporte público por empresas habilitadas. El encuadre de un traslado remunerado contratado por la plataforma depende de la modalidad, vehículo, ruta y quién ejecuta el servicio. La guía citada por MinTransporte se refiere a mascotas en servicios de transporte público. Para Mascotas hace falta verificar el supuesto específico del traslado A→B, incluida la mascota sin acompañante. [Ley 336 de 1996](https://www.secretariasenado.gov.co/senado/basedoc/ley_0336_1996.html) · [Concepto de MinTransporte](https://mintransporte.gov.co/info/mintransporte/media/anexos/1jXlUARo.pdf)

| Verificación | Resultado requerido para operar |
| --- | --- |
| Modalidad y alcance | Definir si se traslada solo la mascota o también viaja su responsable, el tipo de vehículo y si el recorrido es urbano o intermunicipal. Validar modalidad, operador habilitado, permisos y reglas locales con la autoridad competente. |
| Prestador y vehículo | Verificar identidad, documentos del vehículo y del conductor, habilitación que corresponda y alcance de la cobertura de daños y custodia de la mascota. Registrar condiciones, vigencia y exclusiones de pólizas o coberturas. |
| Bienestar y custodia | Establecer condiciones de admisión, guacal o sujeción adecuada, ventilación, higiene, agua, descanso, entrega a persona autorizada y protocolo frente a enfermedad, fuga, accidente o retraso. Aplicar el deber de cuidado durante el traslado previsto en el artículo 33 de la Ley 84. |
| Contrato y recaudo | Identificar quién contrata, recoge, custodia, entrega, responde y factura. Definir la intervención de Mascotas, reclamos, cancelaciones, abonos y tratamiento de recargas. |
| Datos y seguimiento | Solicitar permisos, limitar acceso a la ubicación y definir conservación y respuesta a solicitudes de datos. |

El ICA indica que la movilización nacional de perros y gatos está exenta del Certificado de Inspección Sanitaria del ICA. La verificación del servicio comercial y el bienestar animal se resuelven por su propio alcance. [ICA](https://www.ica.gov.co/importacion-y-exportacion/requisitos-para-importar-mascotas/movilizacion-a-nivel-nacional-de-perros-y-gatos) · [Ley 84 de 1989](https://normograma.invima.gov.co/normograma/compilacion/docs/ley_0084_1989.htm) · [Ley 1774 de 2016](https://www.funcionpublica.gov.co/eva/gestornormativo/norma.php?i=68135)

Para el piloto, se propone limitar las rutas y especies admitidas y trabajar con un operador cuya habilitación y cobertura se hayan verificado para el servicio definido. Habilitar viajes pagados después de cerrar modalidad, contrato, documentación y protocolo de incidentes.

## Registro de avance

Registrar cada cierre o bloqueo por ID, con fecha, estado y evidencia reproducible. Para una prueba, incluir comando o ejecución de CI y resultado. Para un cambio visual, adjuntar la captura o el diff. Para un bloqueo, indicar la dependencia concreta y el siguiente trabajo disponible.

| Fecha | Tarea | Estado | Evidencia |
| --- | --- | --- | --- |
| 2026-11-11 | 1.0-01 | Verificada | [Memoria](https://github.com/erickvalderrama/it/blob/main/prompts/Mascotas-CLAUDE.md) · [Especificación](https://github.com/erickvalderrama/it/blob/main/prompts/mascotas/phase-1-specification.md) |
| 2026-11-11 | 1.0-02 | Verificada | [Catálogo DANE y fuentes](https://github.com/erickvalderrama/it/blob/main/prompts/mascotas/data/README.md) · [Departamentos](https://github.com/erickvalderrama/it/blob/main/prompts/mascotas/data/colombia-departments.csv) · [Localidades](https://github.com/erickvalderrama/it/blob/main/prompts/mascotas/data/colombia-divipola-localities.csv) |
| 2026-11-11 | 1.0-03 | Verificada como borradores | [Términos](https://github.com/erickvalderrama/it/blob/main/prompts/mascotas/legal/terms-and-conditions.md) · [Privacidad](https://github.com/erickvalderrama/it/blob/main/prompts/mascotas/legal/privacy-policy.md) |
| 2026-11-11 | 1.0-04 | Pendiente de ubicación | El repositorio de aplicación aún no está identificado en la memoria. |
| 2026-11-11 | 1.0-05 | Pendiente del modelo | La ruta del modelo entidad-relación aprobado sigue pendiente en la memoria. |

## Preguntas para el cierre de la ejecución

Revisar esta lista al terminar el trabajo ejecutable. Conservar solo las preguntas vigentes que requieran una decisión o un dato del responsable. Registrar cada respuesta junto a la tarea afectada y continuar con el primer punto que desbloquee.

| Tarea | Información que desbloquea el siguiente paso |
| --- | --- |
| 1.0-04 | ¿Cuál es el repositorio y la rama de trabajo de la aplicación? |
| 1.0-05 | ¿Dónde está la versión aprobada del modelo entidad-relación? |
| 1A.1-04 | ¿Cuáles son los destinos y el mecanismo autorizado para configurar PostgreSQL y MinIO por ambiente? Compartir la ubicación de la configuración segura. |
| 1B.2-05 | ¿Cuál es el canal inicial para solicitar o contactar por una adopción? |
| 1C.2-06 | ¿Cuáles son los precios, beneficios, vigencia promocional, emisor fiscal y reglas de devolución definitivos del piloto? |
