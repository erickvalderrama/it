# Plan de trabajo de Mascotas

Nombre oficial de la aplicación: **Petyei**, elegido el 11 de noviembre de 2026. Mascotas se conserva como nombre interno del proyecto. [Marca Petyei](https://github.com/erickvalderrama/it/tree/main/documents/brand/petyei)

Decisiones registradas el 9 de noviembre de 2026. Proyecto para un primer lanzamiento en Colombia con Flutter para web, Android e iOS, Riverpod como opción inicial, C# con .NET y PostgreSQL. La memoria técnica conserva arquitectura limpia, código en inglés, textos base en constantes, pruebas unitarias, de integración, E2E y de regresión visual, seguridad, máximo 699 líneas por archivo de código y el modelo entidad-relación aprobado como regla de oro. [Memoria técnica](https://github.com/erickvalderrama/it/blob/main/CLAUDE.md)

## Cómo ejecutar el plan y pasarlo a otra IA

1. Leer el `CLAUDE.md` de Mascotas, este plan, la especificación de fase 1 y las instrucciones del repositorio. La aplicación vive en este mismo repositorio (`erickvalderrama/it`, rama `main`) con dos carpetas en la raíz: `back/` para la solución ASP.NET Core y `front/` para la aplicación Flutter. Este archivo es el registro activo del avance y la especificación de fase 1 sigue en `documents/prompts/mascotas/`.
2. Revisar el código, la rama, los cambios existentes y el registro de avance. Empezar por la primera tarea `[ ]` ejecutable de la fase solicitada. Seguir el orden de dependencias, conservar el trabajo existente y usar las convenciones del proyecto. Mantener la arquitectura, el modelo entidad-relación, los textos base en inglés, el límite de 699 líneas y las pruebas exigidas en la memoria.
3. Ejecutar la tarea por el flujo real que usará el usuario. Correr las comprobaciones aplicables y registrar el resultado con ruta, commit, comando, ejecución de CI o captura. Cambiar la casilla de `[ ]` a `[x]` solo al verificar el resultado. Actualizar el plan en el mismo cambio y continuar con la siguiente tarea disponible.
4. Resolver decisiones reversibles durante la ejecución y anotar las que afecten el resultado. Para una dependencia ausente, conservar `[ ]` y registrar qué falta y qué tarea afecta. Avanzar con las tareas independientes del alcance autorizado, usando fixtures o adaptadores de prueba cuando corresponda. Retomar la tarea bloqueada cuando esté disponible su dependencia.
5. Agotar el trabajo ejecutable antes de cerrar. Dejar los IDs completados, la evidencia, el siguiente paso y las dependencias que siguen pendientes. Agrupar al final únicamente las preguntas cuya respuesta desbloquea trabajo. Una IA que retome debe poder continuar con el plan, el código y la evidencia del repositorio.

## Infraestructura del proyecto

| Componente | Ubicación y conexión |
| --- | --- |
| Estructura del repositorio | `back/`: solución .NET (Domain, Application, Infrastructure, API y pruebas) con su Dockerfile. `front/`: aplicación Flutter para web, Android e iOS con su Dockerfile web. En la raíz: `compose.yaml`, `CLAUDE.md` y `documents/`. |
| API ASP.NET Core y trabajo en segundo plano | Un solo backend: imagen de aplicación .NET en contenedor. Los trabajos en segundo plano (moderación, facturas, cobros de suscripción, conciliación y entrenamiento del modelo) corren como servicios hospedados dentro del mismo backend. Sin microservicios. |
| Flutter web | Imagen con el build web y servidor de archivos estáticos. Configurar la URL pública de la API y el fallback de navegación para abrir enlaces profundos directamente. |
| Flutter Android e iOS | Builds para sus plataformas de distribución. Las apps se conectan a la misma API desplegada en contenedores. |
| PostgreSQL | Servidor existente, administrado fuera de Compose. La API y los procesos autorizados se conectan por host accesible desde el contenedor, puerto, base, usuario y configuración TLS del ambiente. |
| MinIO | Servicio externo en servidor propio. La API se conecta con endpoint, bucket o prefijo y credenciales de aplicación por ambiente. Guardar los objetos con acceso controlado y conservar en PostgreSQL las referencias según el modelo aprobado. |

Reglas generales del responsable, 11 de noviembre de 2026: un frontend Flutter y un backend ASP.NET Core, sin microservicios; borrado lógico por cambio de estado, nunca físico; índices obligatorios; cambios de base de datos solo con migraciones de EF Core; JWT sin vencimiento con sesión revocable en servidor; contraseñas guardadas solo como hash; suscripciones con cobro mensual automático por tokenización en ePayco; y un modelo propio de machine learning obligatorio. Detalle en la [memoria técnica](https://github.com/erickvalderrama/it/blob/main/CLAUDE.md).

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

- **Mockup antes de cada pantalla** (indicación del responsable, 12 de noviembre de 2026). Antes de diseñar o cambiar una ventana o interfaz en Flutter, generar un mockup en `documents/mockups/<id-tarea>-<pantalla>/` con la vista web (amplia) y la móvil (compacta), datos representativos, acción principal y estados de carga, vacío, error y éxito que apliquen. Guardar la fuente `index.html` y las imágenes `web.png` y `mobile.png`, y solo después implementar la pantalla en Flutter siguiendo el mockup. Registrar la ruta del mockup en la evidencia de la tarea.

- Probar los recorridos críticos E2E desde Flutter hasta la API, PostgreSQL y el procesamiento en segundo plano. Usar datos ficticios y respuestas controladas de ePayco, facturación e IA en CI. Verificar las integraciones por separado en los entornos de prueba autorizados.
- Comparar capturas con referencias visuales revisadas para las pantallas clave en tamaños compacto, mediano y amplio. Revisar diferencias, desbordamientos, textos ampliados, navegación por teclado, áreas seguras, accesibilidad y estados de carga, vacío, error y éxito.
- Ejecutar E2E de web en CI al cambiar un flujo crítico y validar los flujos móviles relevantes en Android e iOS antes de publicar en esos canales. Cada fase requiere sus recorridos E2E y controles visuales aprobados, con evidencia de pruebas y revisión de los casos que requieran dispositivo.

## Fase 1. Adopciones

Usar la especificación de implementación para convertir las entregas en tareas, con contratos de API y datos, recorridos UI/UX, estados y casos de aceptación. Las rutas y los límites propuestos se ajustan al código existente y al modelo aprobado. [Especificación de fase 1](https://github.com/erickvalderrama/it/blob/main/documents/prompts/mascotas/phase-1-specification.md)

### Entregas en orden de dependencia

| Entrega | Trabajo | Flujo E2E de salida |
| --- | --- | --- |
| 1A. Acceso y catálogo | Registro breve, Colombia preseleccionado, selección de municipio, roles, alta Business, aceptación de términos por versión, inglés y español, modo oscuro y documentos legales versión 1 en ambos idiomas. | Una persona se registra con Colombia preseleccionada, selecciona y cambia su ciudad, completa un alta Business y acepta una nueva versión de términos. |
| 1B. Publicar y revisar | Ficha de adopción con fotos, borradores, búsqueda, filtros por ciudad y origen, revisión por IA y cola manual. | Una publicación básica aprobada aparece con el filtro de ciudad y publicante. Se evalúan el texto y todas las fotos. Los casos dudosos llegan a Superadmin y el contenido sexual o vulgar se rechaza. |
| 1C. Cobro e historial | Versiones de precios, promoción Business, mensualidad, compra de anuncio pagado, ePayco y facturación electrónica. | Una compra de prueba conserva precio y términos aceptados. El pago confirmado envía el anuncio a revisión. Las correcciones se reenvían con el mismo pago. Se comprueban un solo cobro aplicado, la visibilidad tras aprobación y la factura, incluso con confirmaciones repetidas. |
| 1E. Modelo propio de compatibilidad | Cuestionarios de adoptante y mascota, puntaje base por reglas, recolección de señales, entrenamiento con ML.NET, versionado y explicación del puntaje. | Una persona responde su cuestionario, ve el puntaje y sus motivos en una ficha y en su solicitud. Un modelo entrenado con datos de prueba se promueve solo si supera la base, y el sistema vuelve a la base si falta el modelo. |
| 1D. Operación y piloto | Panel con usuarios registrados y activos, administración de anuncios, cobros, facturas y reclamos. Prueba de interfaz web y móvil y piloto con organizaciones. | Superadmin puede moderar, consultar la operación y recuperar una factura pendiente. Se verifican dos ciudades, los tres roles y el aislamiento entre cuentas. Pasan las comparaciones visuales de las pantallas clave. |

### Preparación y materiales

- [x] **1.0-01** Versionar la memoria técnica de Mascotas y la especificación de fase 1 para el traspaso a desarrollo.
- [x] **1.0-02** Versionar los dos CSV DIVIPOLA y sus instrucciones de importación. Verificados 33 registros departamentales y 1.122 registros de localidades.
- [x] **1.0-03** Versionar los borradores colombianos de términos y privacidad para la revisión del operador.
- [x] **1.0-04** Registrar la ruta del repositorio de aplicación y sus comandos de trabajo. Incorporar la memoria, el plan activo y la especificación a ese repositorio.
- [ ] **1.0-05** Registrar la ubicación y versión del modelo entidad-relación aprobado. Contrastar cuentas, ubicaciones, Business, listados, medios, consentimientos, precios, pagos y facturas antes de crear los mapeos o migraciones que los afectan.
- [x] **1.0-06** Registrar Bosque y pistacho como la paleta oficial en la memoria principal de Mascotas y en la especificación de fase 1.
- [x] **1.0-07** Elegir el nombre oficial de la aplicación y preparar los bocetos de marca con la paleta oficial.
- [ ] **1.0-08** Registrar `petyei.com` a precio estándar y revisar la marca Petyei en la SIC, nombres en tiendas de aplicaciones y redes. Pulir el arte final con la tipografía elegida y reemplazar los textos visibles de Mascotas por Petyei.

### 1A.1 Arranque en contenedores, contratos y UI/UX

- [ ] **1A.1-01** Revisar la solución existente o establecer la estructura modular de .NET y Flutter. Fijar SDK, dependencias, versión de PostgreSQL del servidor y manejo de estado. Verificar compilación desde un checkout limpio.
- [x] **1A.1-01A** Fijar las fronteras de Domain, Application, Infrastructure y API y la estructura de Flutter por feature. Verificar las referencias entre proyectos y que un flujo de ejemplo atraviesa interfaz, caso de uso y contrato sin lógica de negocio en widgets o controladores HTTP.
- [x] **1A.1-01B** Implementar la base de borrado lógico: estado, `DeletedAt` y `DeletedBy`, filtros globales de EF Core, restricción de borrado en llaves foráneas e índices únicos parciales sobre registros no borrados. Probar que borrar cambia el estado, oculta el registro y conserva la fila.
- [x] **1A.1-01C** Configurar las migraciones de EF Core como único mecanismo de cambio de esquema, con script idempotente o bundle para despliegue. Crear índices para llaves foráneas, filtros, ordenamiento y colas de trabajos. Verificar la migración en una base vacía y revisar los planes de consulta.
- [ ] **1A.1-01D** Implementar JWT sin vencimiento con identificador de sesión validado en cada solicitud. Revocar la sesión al cerrar sesión, cambiar contraseña o rol, suspender o borrar la cuenta. Probar que un token revocado se rechaza y que el usuario puede cerrar sesiones por dispositivo.
- [ ] **1A.1-01E** Guardar contraseñas solo como hash con el hasher de ASP.NET Core Identity o Argon2id. Probar que el hash nunca aparece en respuestas ni registros, el rehash al cambiar parámetros y el límite de intentos.
- [x] **1A.1-02** Crear los Dockerfiles de API y Flutter web y el Compose de los servicios de aplicación. Configurar servidor estático, rutas SPA y API pública. Comprobar `docker compose config`, `docker compose build`, `docker compose up -d` y `docker compose ps`.
- [ ] **1A.1-03** Documentar variables de cada ambiente, ejemplos sin secretos y forma de inyectarlas. Configurar PostgreSQL externo, MinIO, origen web y puertos. Probar que la aplicación detecta valores ausentes o inválidos con errores que protegen datos sensibles.
- [ ] **1A.1-03A** Configurar Google Maps Platform: cuenta de Google Cloud con facturación, keys separadas para Android (paquete y certificado), iOS (bundle ID) y web (dominios autorizados), cada una limitada a las APIs que usa, y cuotas con alertas de presupuesto.
  - CA: las keys se inyectan por ambiente en el build y ninguna aparece en el repositorio.
  - CA: una key de web usada desde un dominio no autorizado es rechazada por Google.
  - CA: hay keys separadas para pruebas y producción, y la alerta de presupuesto está activa.
- [x] **1A.1-03B** Crear un PostgreSQL local compartido entre proyectos con `compose.postgres-local.yaml`: contenedor `postgres-local` (PostGIS 17-3.5, puerto 5433 solo en `127.0.0.1`), superusuario `postgres` y datos en el volumen `postgres-local-data`, separado del contenedor. Crear para Petyei su propia base y usuario con `tools/create-local-database.sh`. Guardar las credenciales en `.env` (excluido de git) y la cadena de conexión de la API en user-secrets.
  - CA: borrar y recrear el contenedor conserva los datos del volumen.
  - CA: cada proyecto usa su propia base y usuario; la base de Petyei no es accesible para `PUBLIC`.
  - CA: ninguna contraseña queda en el repositorio; `.env.example` solo trae valores de ejemplo.
  - CA: la migración inicial se aplica con el usuario de Petyei y `/health/ready` responde 200 en desarrollo.
- [ ] **1A.1-04** Validar desde el contenedor la conexión a PostgreSQL con su rol autorizado y a MinIO con su cuenta de aplicación. En el espacio de pruebas, crear, leer y eliminar un objeto sintético. Registrar el destino por ambiente sin guardar secretos.
- [ ] **1A.1-04A** Recrear los contenedores de API y web y volver a abrir una ruta profunda. Comprobar el fallback de Flutter, la ruta de la API, la separación de configuración por ambiente y la conservación de un dato de prueba en PostgreSQL y MinIO.
- [x] **1A.1-05** Publicar OpenAPI, códigos de error y referencias de correlación. Implementar liveness del proceso y readiness con comprobaciones acotadas de sus dependencias. Verificar estado con servicios disponibles y con un fallo simulado.
- [ ] **1A.1-05A** Publicar un ejemplo de respuesta de catálogo y un problema de validación en el contrato versionado. Verificar en CI códigos de error estables, paginación y que Flutter deserializa las respuestas del API de prueba.
- [ ] **1A.1-06** Preparar fixtures repetibles para Santa Marta y Barranquilla, tres roles, dos organizaciones, dos precios y dos términos. Ejecutar los resets sobre el destino de pruebas aislado tras validar el modelo aplicable. Configurar adaptadores de prueba para ePayco, IA, correo y factura.
- [ ] **1A.1-06A** Probar el reinicio de fixtures dos veces y comprobar que las cuentas, localidades y versiones de prueba son reproducibles. Verificar que la rutina se detiene al recibir un destino fuera de la base y el prefijo de pruebas.
- [ ] **1A.1-07** Esbozar búsqueda, ficha, registro, borrador, checkout y correcciones en tamaños compacto y amplio. Revisar jerarquía, acción principal, foco y estados vacíos con fixtures. [Paleta oficial y criterios UI/UX](https://github.com/erickvalderrama/it/blob/main/documents/prompts/mascotas/phase-1-specification.md#official-light-theme)
- [ ] **1A.1-07A** Implementar la paleta oficial Bosque y pistacho en un tema Flutter compartido, con variantes clara y oscura (ver 1A.4-08). Aplicar bosque a botones y controles seleccionados, pistacho a destacados, fondo claro a pantallas, blanco a tarjetas y tinta verde a texto. Verificar que los componentes consumen los tokens del tema.
- [ ] **1A.1-07B** Montar navegación Explore, Publish y Account, textos localizados en inglés y español y componentes de carga, vacío, error y acción principal. Probar foco visible, lectura con tecnología asistiva y recuperación de la ruta tras autenticación en layouts compacto y amplio.
- [ ] **1A.1-08** Configurar CI para compilar las imágenes, verificar formato y análisis y ejecutar pruebas unitarias y de widgets. Preparar el runner autorizado con acceso al PostgreSQL y MinIO de pruebas para integración y E2E.
- [ ] **1A.1-08A** Agregar comprobaciones de dependencias, formato y máximo de 699 líneas por archivo fuente. Publicar como evidencia de CI el resultado de compilación, pruebas y diffs visuales de las pantallas afectadas.
- [ ] **1A.1-09** Ejecutar la salida del bloque desde un checkout limpio. Abrir Flutter web en contenedor, navegar a una ruta profunda y confirmar que consume la API. Guardar los comandos, resultados y una captura base.

### 1A.2 Registro, catálogo y consentimiento

- [x] **1A.2-01** Implementar el importador DIVIPOLA según el modelo aprobado. Preservar país, departamento y localidad como texto y la elegibilidad del piloto por separado. Probar reimportación y que `08001` conserva el cero inicial.
- [x] **1A.2-01A** Agregar una validación previa de los CSV con conteo por tipo, códigos duplicados, departamento ausente y cambios de fuente. Probar que la reimportación preserva referencias de cuentas y anuncios y que la elegibilidad del piloto se administra por separado.
- [x] **1A.2-02** Implementar selector de país y ciudad con Colombia preseleccionada y ciudades agrupadas por departamento. Probar filtros, carga, vacío, error y rechazo de un municipio inválido o deshabilitado en el servidor.
- [ ] **1A.2-03** Implementar el registro de nombre, correo, contraseña, país y ciudad. Validar campos, unicidad de correo y sesión. Cubrir inicio de sesión, cierre, verificación previa a publicar y recuperación según el alcance del piloto.
- [ ] **1A.2-03A** Probar la verificación y recuperación de cuenta con enlaces vigentes, vencidos y reutilizados. Verificar normalización del correo, mensaje de recuperación uniforme y exclusión de credenciales y tokens en registros.
- [ ] **1A.2-03B** Construir la matriz de acceso de visitante, User, Business y Superadmin para lectura, publicación y administración. Probar autorización en el backend con una sesión vencida y con otra cuenta intentando acceder a un recurso privado.
- [ ] **1A.2-04** Permitir cambiar la ciudad de la cuenta. Validar permisos, mantener la ciudad de cada anuncio independiente y comprobar que el retorno tras iniciar sesión abre la ruta prevista.
- [ ] **1A.2-05** Implementar lectura de términos y privacidad y aceptación explícita de la versión efectiva en inglés o español. Registrar usuario, versión, idioma y fecha del servidor. Gestionar una versión publicada mientras la pantalla está abierta.
- [ ] **1A.2-05A** Publicar una versión legal desde la administración con idioma, vigencia y texto. Consultar la versión anterior por su referencia y conservar fecha y actor de cada aceptación. Mantener la autorización de datos separada donde corresponda.
- [ ] **1A.2-06** Bloquear las nuevas operaciones protegidas hasta aceptar términos vigentes. Comprobar el acceso a historial, documentos y soporte y el vínculo de una operación previa con su aceptación original.
- [ ] **1A.2-06A** Probar un formulario abierto cuando cambia la versión legal. Mostrar la versión nueva y volver al borrador o checkout con la información válida preservada. Comprobar en servidor que la operación queda vinculada con la aceptación vigente.
- [ ] **1A.2-07** Ejecutar E2E de registro, cambio de ciudad y actualización de términos. Guardar la prueba de validación en PostgreSQL y la revisión visual del formulario en web y móvil.

### 1A.3 Alta Business y promoción

- [ ] **1A.3-01** Implementar elección Foundation o Store en el alta o conversión. Mantener el ID y las operaciones de la cuenta. Probar ambos perfiles bajo el rol Business.
- [ ] **1A.3-02** Pedir cédula y RUT a Store de persona natural y NIT y RUT a Store de persona jurídica. Aplicar las verificaciones de representación que correspondan. Mostrar solo los requisitos del tipo elegido.
- [ ] **1A.3-02A** Validar los campos condicionales y documentos de cada tipo de alta en el servidor. Probar Foundation, Store natural y Store jurídica con requisitos completos, faltantes y archivos que requieren corrección.
- [ ] **1A.3-03** Implementar carga y lectura autorizada de documentos privados en MinIO. Comprobar formatos, límites y permisos, incluida la denegación a otra organización y la ausencia de documentos en resultados públicos.
- [ ] **1A.3-03A** Permitir reemplazar el soporte solicitado durante una corrección y mostrar el progreso por archivo. Verificar que la revisión accede a la versión autorizada y que las vistas públicas y los registros técnicos excluyen los documentos.
- [ ] **1A.3-04** Implementar revisión de la solicitud, estados y decisión auditada de Superadmin. Mostrar al usuario tareas pendientes y conservar la trazabilidad al habilitar Business.
- [ ] **1A.3-04A** Definir las transiciones de solicitud pendiente, corrección, aprobación y rechazo para el flujo autorizado. Probar que dos decisiones simultáneas conservan un resultado vigente con su motivo y auditoría.
- [ ] **1A.3-05** Implementar alta promocional con elegibilidad, inicio, vencimiento y duración configurable. Mostrar vigencia y beneficios. Probar vencimiento con reloj controlado y acceso a operaciones históricas.
- [ ] **1A.3-05A** Probar el inicio y vencimiento de la promoción en su límite exacto, una activación repetida y una oferta gratuita de importe cero. Conservar la oferta aceptada y las operaciones anteriores al vencer.
- [ ] **1A.3-06** Ejecutar E2E de alta Foundation, conversión Store y aislamiento entre dos Business. Revisar el formulario y los estados de verificación en resoluciones compacta y amplia.

### 1A.4 Idiomas, modo oscuro y documentos legales versión 1

Reglas del responsable, 11 de noviembre de 2026: la aplicación es multilenguaje por arquitectura y sale en inglés y español; agregar un idioma debe ser sencillo; la fase 1 incluye modo oscuro; términos y privacidad tienen una versión por fase, legible en ambos idiomas. Detalle en la [memoria técnica](https://github.com/erickvalderrama/it/blob/main/CLAUDE.md).

#### Idiomas

- [x] **1A.4-01** Configurar `flutter_localizations` y `gen-l10n` con `app_en.arb` como plantilla y `app_es.arb`. Exponer los textos mediante una extensión como `context.l10n`.
  - CA: ningún widget ni controlador contiene texto visible escrito en el código; una búsqueda automática en CI lo verifica.
  - CA: las claves usan nombres en inglés por funcionalidad, por ejemplo `adoptionDetailTitle`.
  - CA: cantidades, precios y fechas usan marcadores ICU con plurales y formato de `intl` según el idioma activo.
- [ ] **1A.4-02** Detectar el idioma del dispositivo o navegador en la primera visita y permitir cambiarlo antes de iniciar sesión y en la cuenta.
  - CA: con el dispositivo en español la app abre en español; en un idioma no soportado abre en inglés.
  - CA: al cambiar el idioma la pantalla se actualiza sin reiniciar, conserva la ruta y los datos del formulario.
  - CA: la preferencia se guarda en el dispositivo y en la cuenta, y se aplica al iniciar sesión en otro dispositivo.
- [ ] **1A.4-03** Devolver desde la API códigos de error estables y localizar en servidor solo el contenido que genera el backend: correos, notificaciones y notas de factura, según el idioma de la cuenta o `Accept-Language`.
  - CA: cada código de error del contrato tiene mensaje en inglés y español en Flutter; un código desconocido muestra un mensaje genérico localizado.
  - CA: el correo de verificación y el de recuperación llegan en el idioma de la cuenta.
- [ ] **1A.4-04** Implementar traducciones de datos administrados en base de datos: especies, razas, preguntas y opciones de cuestionarios, planes y beneficios, motivos de moderación y plantillas de notificación, con caída a inglés.
  - CA: el catálogo de especies y razas se muestra en el idioma activo; una traducción faltante muestra el texto en inglés y queda reportada.
  - CA: Superadmin puede editar la traducción de un elemento sin cambiar código, con auditoría.
- [x] **1A.4-05** Agregar a CI la validación de traducciones: claves faltantes o sin uso, marcadores distintos entre idiomas y elementos de catálogo sin traducir.
  - CA: un ARB con una clave faltante o un marcador distinto hace fallar el pipeline con el nombre de la clave.
- [ ] **1A.4-06** Documentar en `docs/localization.md` cómo agregar un idioma y ensayarlo con un idioma de prueba.
  - CA: agregar un tercer idioma de prueba solo requiere el ARB, registrar el locale, las traducciones de catálogo y los documentos legales; no cambia lógica de negocio.
  - CA: los layouts usan alineación y márgenes direccionales, y una prueba con un idioma de derecha a izquierda no rompe las pantallas clave.
- [ ] **1A.4-07** Revisar con textos largos y ampliación de texto las pantallas clave en ambos idiomas y en tamaños compacto y amplio.
  - CA: no hay textos cortados ni desbordes; los botones y etiquetas largas en español se ajustan sin ocultar la acción principal.

#### Modo oscuro

- [x] **1A.4-08** Definir los tokens oscuros en el tema compartido de Flutter a partir de Bosque y pistacho: `canvas` `#121A14`, `surface` `#1C2620`, `primary` `#B9D88A`, `onPrimary` `#16231A`, `accentContainer` `#2F4A33`, `onAccentContainer` `#D7EC9E`, `onSurface` `#E3ECDF`.
  - CA: los widgets solo consumen tokens del tema; una revisión automática detecta colores escritos directamente.
  - CA: todos los pares de texto e ícono cumplen contraste WCAG AA de 4,5:1, y 3:1 para texto grande y bordes de controles. La medición de la propuesta va de 7,6:1 a 14,6:1.
- [ ] **1A.4-09** Ofrecer Sistema, Claro y Oscuro antes de iniciar sesión y en la cuenta, con Sistema por defecto.
  - CA: el cambio se aplica al instante, sin reiniciar, y se guarda en el dispositivo y en la cuenta.
  - CA: con Sistema, la app sigue el cambio de tema del sistema operativo mientras está abierta.
- [ ] **1A.4-10** Adaptar recursos visuales al tema oscuro: logo e ícono de Petyei, imágenes de estados vacíos, mapas, gráficos del panel y colores de estado.
  - CA: el logo usa la versión oscura sobre fondos oscuros; los estados se distinguen también por texto e ícono.
- [ ] **1A.4-11** Registrar goldens de búsqueda, ficha, registro, checkout, correcciones, suscripción y cola administrativa en claro y oscuro, inglés y español.
  - CA: CI compara las cuatro combinaciones en tamaños compacto y amplio y guarda los diffs aprobados.

#### Términos y privacidad versión 1

- [ ] **1A.4-12** Adaptar los borradores actuales de términos y privacidad al alcance de la fase 1 como versión 1, con el nombre Petyei.
  - CA: la versión 1 cubre cuentas, publicaciones básicas y pagadas, alta Business, membresía, suscripciones con cobro automático mensual y tokenización, moderación con IA, cuestionarios y modelo de compatibilidad, facturación, devoluciones, sesiones y soporte.
  - CA: la versión 1 no presenta como disponibles tiendas, servicios, recargas, Payouts ni transporte.
  - CA: la política de privacidad describe los datos del suscriptor, el uso de los cuestionarios para el modelo, el borrado lógico con anonimización ante solicitudes de supresión y el tratamiento de la tarjeta por ePayco.
- [ ] **1A.4-13** Preparar la versión 1 en español y en inglés con el mismo número de versión, fecha de vigencia y contenido, en `docs/legal/v1/es/` y `docs/legal/v1/en/`.
  - CA: ambos textos tienen las mismas secciones y cláusulas; una revisión cruzada lo confirma antes de publicar.
  - CA: los dos textos indican qué idioma prevalece si hay diferencias, según la revisión legal.
- [ ] **1A.4-14** Cargar la versión 1 en ambos idiomas en el módulo de documentos legales y mostrarla en el idioma activo, con opción de leer y descargar el otro idioma.
  - CA: desde registro, checkout, suscripción, cuenta y una página pública se lee y descarga cada documento en inglés y español.
  - CA: la aceptación registra usuario, documento, versión, idioma mostrado y fecha del servidor.
- [ ] **1A.4-15** Dejar preparado el flujo de nuevas versiones por fase: la versión 2 se publica antes de habilitar la fase 2 y la versión 3 antes de la fase 3, cada una en ambos idiomas.
  - CA: publicar una versión nueva exige los dos idiomas completos; con un idioma faltante la publicación se bloquea.
  - CA: con una versión nueva vigente, el usuario debe aceptarla antes de las operaciones nuevas y conserva el acceso a la versión que aceptó antes.
  - CA: agregar un idioma futuro crea una traducción de la versión vigente, no una versión nueva.
- [ ] **1A.4-16** Ejecutar E2E de cambio de idioma y de tema durante registro y checkout, y de lectura y aceptación de la versión 1 en cada idioma.
  - CA: el E2E pasa en web y en Android e iOS antes de liberar esas plataformas; se adjuntan capturas y resultados.

### 1B.1 Adopción básica de extremo a extremo

- [ ] **1B.1-01** Implementar borrador editable con datos de mascota, ciudad propia del anuncio y contexto de publicación individual u organización. Validar propietario y concurrencia de edición al guardar.
- [ ] **1B.1-01A** Restaurar el borrador desde otro inicio de sesión o tras recargar. Simular una edición en dos pestañas y mostrar cómo recuperar la revisión más reciente conservando el trabajo recuperable.
- [ ] **1B.1-01B** Implementar la ubicación del sitio de adopción con un mapa de Google Maps en el borrador. El publicante busca o mueve el marcador, o parte de su ubicación actual con permiso, y el backend guarda latitud y longitud.
  - CA: el marcador se puede colocar por búsqueda, arrastre o ubicación actual en web, Android e iOS; si se niega el permiso de ubicación, el mapa sigue funcionando con búsqueda manual.
  - CA: el servidor valida rangos de latitud y longitud y que el punto corresponda al municipio DIVIPOLA elegido o esté cerca; si no coincide, muestra un aviso para corregir.
  - CA: las coordenadas se guardan con precisión de 6 decimales o como punto PostGIS, con índice, mediante migración de EF Core.
  - CA: la ficha pública muestra solo una zona aproximada calculada en servidor; la respuesta pública nunca incluye el punto exacto.
  - CA: el mapa usa el idioma activo y estilo oscuro en modo oscuro; el filtro por ciudad funciona aunque el mapa no cargue.
- [ ] **1B.1-02** Implementar carga de fotos a MinIO con formato y tamaño validados, orden y portada. Preparar derivados sin metadatos sensibles. Probar un fallo de una foto y su reintento sin perder las demás.
- [ ] **1B.1-02A** Preparar miniaturas y versiones de detalle saneadas para el catálogo, con estados de carga y reintento por foto. Probar la limpieza de una carga incompleta en MinIO según la política definida.
- [ ] **1B.1-03** Implementar vista previa con la proyección que será pública. Mostrar campos faltantes y el contexto del publicante. Verificar la visibilidad privada del borrador y sus fotos.
- [ ] **1B.1-03A** Verificar por contrato la diferencia entre la respuesta pública y la privada. Probar que un visitante solo recibe los campos autorizados y fotos de la revisión visible, incluso al abrir directamente el enlace de un borrador.
- [ ] **1B.1-04** Congelar una revisión completa al enviar una publicación básica. Ponerla en revisión, comprobar aprobación mediante el adaptador de prueba y activarla una sola vez con el identificador de revisión correcto.
- [ ] **1B.1-05** Implementar catálogo público paginado, filtros de ciudad y origen del publicante, ficha compartible y estados de carga, vacío y error. Probar acceso directo, recarga y restauración de filtros.
- [ ] **1B.1-05A** Comprobar orden y paginación estable del catálogo con dos ciudades, ambos orígenes, varios anuncios y cambios de filtro. Verificar que las páginas se reinician al cambiar de ciudad y que el listado mantiene su estado al volver de la ficha.
- [ ] **1B.1-06** Implementar el retiro de visibilidad según los estados aprobados. Verificar que el historial se conserva y que el catálogo y la ficha aplican la misma regla de acceso.
- [ ] **1B.1-06A** Implementar la vista de publicaciones propias con estado, fecha y acción disponible por anuncio. Probar el cierre o retiro previsto y que el autor conserva acceso a su historial y al resultado de revisión.
- [ ] **1B.1-06B** Probar una ficha con enlace válido, un anuncio cerrado y una publicación de otra ciudad. Mostrar un estado de no disponibilidad y una acción de regreso al catálogo cuando corresponda.
- [ ] **1B.1-07** Demostrar desde Flutter que una persona crea una adopción básica, carga fotos, la envía, la ve aprobada y la encuentra solo en los filtros que corresponden. Guardar el E2E y las capturas de borrador, ficha y resultados.

### 1B.2 Moderación, correcciones y contacto

- [ ] **1B.2-01** Implementar un contrato de moderación por revisión con texto, todas las fotos y texto visible en fotos. Enviar contenido saneado y registrar versión de reglas, entradas y resultados estructurados.
- [ ] **1B.2-01A** Validar la respuesta estructurada del proveedor contra los identificadores de la revisión, texto y cada foto. Probar resultado incompleto, ilegible o contradictorio y asegurar que llega a la ruta de revisión que corresponde.
- [ ] **1B.2-02** Implementar resultados aprobado, correcciones requeridas, rechazado y revisión manual. Probar rechazo por contenido sexual o vulgar, imagen ausente y respuesta incierta o fallo persistente.
- [ ] **1B.2-02A** Hacer recuperable el trabajo de revisión tras reiniciar el backend. Probar reintentos acotados, duplicados de la misma revisión y escalamiento manual al agotarlos. Registrar un resultado vigente por revisión.
- [ ] **1B.2-03** Implementar corrección por campo o foto, nueva revisión y reenvío. Probar que una aprobación tardía de la revisión anterior conserva la privacidad de la revisión pendiente.
- [ ] **1B.2-03A** Diseñar la pantalla de correcciones con motivo, campo o número de foto y acción para corregir. Probar reenvío conservando los campos válidos y comprobar que el contenido sin aprobar sigue privado.
- [ ] **1B.2-04** Implementar cola de Superadmin para revisar la revisión exacta, decidir con motivo y registrar auditoría. Probar separación entre permisos de publicación y de moderación.
- [ ] **1B.2-04A** Filtrar la cola administrativa por estado y ciudad, mostrando el tiempo de espera y la revisión exacta. Probar decisiones concurrentes y conservar actor, motivo y estado resultante en auditoría.
- [ ] **1B.2-05** Integrar el canal de contacto acordado para adopciones. Permitir que un visitante llegue a la acción y conservar el contexto si requiere iniciar sesión. Validar quién puede consultar y gestionar los datos de cada solicitud si se usa ese flujo.
- [ ] **1B.2-05A** Probar desde un enlace compartido el recorrido de contacto acordado, incluida autenticación cuando aplique. Verificar confirmación, reintento y acceso restringido al contacto o solicitud según el canal habilitado.
- [ ] **1B.2-06** Ejecutar E2E de aprobado, corrección, rechazo y resolución manual. Comparar las capturas de cada estado y revisar las instrucciones del autor con usuarios del piloto.

### 1C.1 Cobros, precios y facturación con proveedores de prueba

- [ ] **1C.1-01** Implementar versiones inmutables de precios y ofertas para premium y renovación Business, incluidos los beneficios de importe cero. Conservar versión, moneda, total, vigencia y actor en cada aceptación.
- [ ] **1C.1-01A** Administrar precios por producto, vigencia y Business cuando aplique. Rechazar vigencias incompatibles para el mismo ámbito. Probar una tarifa futura, un descuento y la consulta del importe que un usuario aceptó anteriormente.
- [ ] **1C.1-02** Mostrar total, duración, prestaciones, condiciones y versión aplicable antes de pagar. Crear la operación de checkout ligada a la revisión o período exacto. Probar un cambio de precio posterior y una oferta vencida.
- [ ] **1C.1-02A** Hacer idempotente la creación de la operación de checkout. Probar doble clic, reenvío de la misma solicitud tras timeout y conflicto si se reutiliza la clave con un cuerpo diferente.
- [ ] **1C.1-03** Implementar el adaptador de checkout ePayco y retorno al estado de la operación. Probar pago pendiente, cancelación o fallo de la sesión y retorno tras recargar el navegador.
- [ ] **1C.1-03A** Hacer reanudable la pantalla de estado mediante la referencia interna. Probar retorno temprano y cierre de pestaña, y mostrar por separado pago, revisión y factura desde los estados confirmados por el servidor.
- [ ] **1C.1-04** Verificar la confirmación ePayco en el servidor y conciliar referencia, importe y moneda. Probar firma inválida, duplicados concurrentes, eventos tardíos y el caso de un segundo cobro real que requiere conciliación.
- [ ] **1C.1-04A** Simular monto o moneda discrepante, evento pendiente después de confirmado y reinicio entre confirmación y trabajos de seguimiento. Verificar conciliación de excepciones, permanencia del estado confirmado y recuperación sin efectos duplicados.
- [ ] **1C.1-05** Enviar la revisión premium a moderación tras el pago confirmado. Activar su vigencia al aprobar y mantener pago y precio en correcciones. Probar que el reenvío conserva un solo cobro aplicado.
- [ ] **1C.1-05A** Probar con reloj controlado el inicio de la vigencia al activar el anuncio y el fin del destacado. Verificar catálogo, ficha y estado del autor con la regla de visibilidad acordada.
- [ ] **1C.1-06** Implementar planes de suscripción con precio mensual y cantidad de publicaciones mensuales como versiones inmutables. Vincular suscripción, cobro, factura y publicación consumida a la versión aceptada.
- [ ] **1C.1-06B** Exigir para suscribirse nombres completos, tipo y número de documento, dirección, teléfono y correo verificado. Validar formatos en servidor y probar datos completos, incompletos e inválidos.
- [ ] **1C.1-06C** Tokenizar la tarjeta con el flujo de ePayco sin que los datos de tarjeta pasen por Petyei. Guardar solo las referencias de cliente y token con acceso restringido y registrar el consentimiento del cobro recurrente.
- [ ] **1C.1-06D** Ejecutar el cobro mensual automático como trabajo idempotente del backend. Probar con reloj controlado un cobro exitoso, uno fallido, reintentos, período de gracia, suspensión, actualización de tarjeta y cancelación, con un solo cobro y una factura por período.
- [ ] **1C.1-06E** Descontar publicaciones del cupo del período activo y mostrar usadas y disponibles. Probar el cupo agotado y aplicar la regla aprobada para publicaciones adicionales.
- [ ] **1C.1-06A** Mostrar vigencia, beneficio, plan, próximo cobro, cupo y acción de cancelación en el perfil Business. Probar confirmaciones repetidas y dos intentos simultáneos para que se aplique un solo período y se preserve su precio aceptado.
- [ ] **1C.1-07** Implementar captura de datos fiscales permitidos, emisión, entrega y consulta autorizada de factura. Probar error fiscal y reintento sin repetir el cobro ni cambiar la visibilidad del anuncio.
- [ ] **1C.1-07A** Ante un resultado fiscal incierto, recuperar el estado por la referencia de la operación antes de volver a emitir. Probar que el dueño puede consultar la factura y que otro usuario recibe la denegación prevista.
- [ ] **1C.1-08** Implementar devolución autorizada y ajuste fiscal según la política acordada. Conservar el vínculo con el pago y conciliar la operación original al reintentar.
- [ ] **1C.1-08A** Conciliar el importe del cobro original, la devolución, el estado del proveedor y el ajuste fiscal aplicable. Probar reenvío de una devolución pendiente y registrar la evidencia de un único resultado aplicado.
- [ ] **1C.1-09** Ejecutar E2E de compra premium, corrección y factura, renovación Business y recuperación de fallos. Verificar en la base aislada un solo pago aplicado y los vínculos de revisión, precio, términos y factura.

### 1C.2 Pruebas de proveedores en entornos autorizados

- [ ] **1C.2-01** Configurar credenciales de prueba por ambiente, retorno web, confirmación ePayco y referencia de conciliación. Validar que el retorno y la confirmación se pueden recibir en cualquier orden.
- [ ] **1C.2-01A** Validar al arrancar la selección de adaptadores por ambiente y bloquear los adaptadores de prueba en producción. Verificar la configuración de retorno y confirmación desde el servicio autorizado y registrar referencias sin secretos.
- [ ] **1C.2-02** Ejecutar en el entorno autorizado de ePayco una compra, confirmación de servidor y conciliación de estado. Conservar evidencia redactada del resultado.
- [ ] **1C.2-02A** Reproducir en pruebas una sesión de pago pendiente y una confirmación final. Guardar la secuencia de estados de checkout, consulta de referencia y revisión asociada, con evidencia redactada.
- [ ] **1C.2-03** Validar el emisor fiscal configurado y probar factura, consulta, error y ajuste aplicable en el entorno de pruebas disponible.
- [ ] **1C.2-03A** Registrar las capacidades confirmadas del entorno fiscal para identificación, emisión, consulta y ajuste. Documentar con el responsable fiscal cualquier operación que necesite un flujo alterno antes de habilitarla.
- [ ] **1C.2-04** Validar el proveedor de IA con texto, fotos y texto visible en imágenes. Comprobar resultados completos, timeout y escalamiento manual con material de prueba.
- [ ] **1C.2-04A** Contrastar los resultados por foto y texto del proveedor con fixtures autorizados. Medir el tiempo de respuesta del caso normal y comprobar qué ve el autor y el administrador cuando se activa la revisión manual.
- [ ] **1C.2-05** Ensayar la recuperación de un pago o factura pendiente mediante acciones auditadas. Verificar que el reintento conserva la referencia original y la evidencia del proveedor.
- [ ] **1C.2-06** Registrar el resultado por proveedor, configuración revisada y dependencias pendientes de operación real. Confirmar la política de devoluciones y reglas fiscales antes de habilitar cobros comerciales.

### 1E Modelo propio de compatibilidad (obligatorio)

Base: tesis de maestría del responsable, emparejamiento de adoptantes y mascotas por cuestionarios con ML.NET. La prueba de concepto de la tesis usó puntajes aleatorios, por lo que sus métricas no validan el modelo. [Modelo de adopciones y emparejamiento](https://github.com/erickvalderrama/it/blob/main/documents/prompts/mascotas/model/README.md)

- [ ] **1E-01** Adaptar las tablas de cuestionarios, mascotas, catálogos y adopciones del ERD de la tesis a las reglas del proyecto: borrado lógico, índices, nombres en inglés, DIVIPOLA y respuestas estructuradas con versión de pregunta. Registrar los cambios propuestos antes de migrar.
- [x] **1E-02** Sembrar especies y razas desde `Especies-Razas.xlsx` mediante migración o importación versionada. Probar reimportación y que las razas quedan ligadas a su especie.
- [ ] **1E-03** Implementar el cuestionario del adoptante y el de la mascota en el borrador de adopción a partir del banco de preguntas de la encuesta de la tesis, con opciones estructuradas y traducciones en inglés y español. No usar el género como variable del modelo. [Banco de preguntas](https://github.com/erickvalderrama/it/blob/main/documents/prompts/mascotas/model/survey-summary.md) Pedir consentimiento para usar las respuestas y probar preguntas obligatorias, opcionales y cambio de versión.
- [ ] **1E-04** Implementar el puntaje base por reglas con explicación en lenguaje claro. Mostrarlo en la ficha y en la solicitud y guardarlo en la adopción al momento de solicitar.
- [ ] **1E-05** Registrar las señales de entrenamiento: respuestas, solicitudes, decisión del publicante y adopción completada o devuelta. Excluir documentos, datos de pago y direcciones exactas.
- [ ] **1E-06** Construir el pipeline de entrenamiento con ML.NET como trabajo del backend, con semilla fija, partición de evaluación y comparación contra la base. Probar con datos sintéticos etiquetados.
- [ ] **1E-07** Versionar dataset, features, métricas y artefacto del modelo en MinIO y PostgreSQL. Promover un modelo solo si supera la base y permitir volver al anterior. Probar la caída a la base cuando no hay modelo activo.
- [ ] **1E-08** Entrenar el primer modelo con datos reales al alcanzar el umbral acordado. Registrar métricas, fecha, versión y decisión de promoción.

### 1D Operación, pruebas y salida del piloto

- [ ] **1D-01** Completar panel Superadmin con usuarios registrados, habilitados y activos, filtros por rol, ciudad y período y definición visible de la métrica. Probar permisos y consistencia de los conteos.
- [ ] **1D-01A** Validar métricas con datos conocidos de registrados, habilitados y activos en distintos períodos. Verificar totales, filtros, deduplicación y ausencia de datos de identidad en métricas y exportaciones operativas.
- [ ] **1D-02** Completar colas de revisión, altas Business, precios y términos, pagos, facturas y devoluciones. Probar auditoría, búsqueda de la operación y recuperación con datos privados protegidos.
- [ ] **1D-02A** Probar autenticación reforzada de Superadmin y acceso a operaciones sensibles. Revisar que la auditoría conserva actor, acción, motivo y resultado y que la evidencia oculta datos de facturación, documentos y referencias privadas.
- [ ] **1D-03** Completar datos del operador, contacto, privacidad y términos para la publicación del piloto. Verificar la versión visible y su vínculo con los consentimientos y las operaciones.
- [ ] **1D-03A** Probar el acceso a soporte y documentos legales desde una cuenta, un anuncio y una operación con cobro. Ensayar la atención de una solicitud con datos de prueba y verificar quién puede acceder a su historial.
- [ ] **1D-04** Ejecutar la matriz de pruebas unitarias, integración con PostgreSQL y MinIO, widgets y E2E con tres roles y dos ciudades. Guardar resultados, cobertura de fallos y evidencias de cada flujo crítico.
- [ ] **1D-04A** Preparar el reporte de salida por flujo con escenario, rol, dispositivo o navegador, comando o ejecución de CI y resultado. Repetir los casos afectados por una corrección y adjuntar evidencia de regresión.
- [ ] **1D-05** Revisar pantallas clave en tamaños compacto, mediano y amplio. Validar teclado, ampliación de texto, contraste, lector de pantalla, navegación y diferencias visuales. Corregir problemas que impidan completar una tarea.
- [ ] **1D-05A** Registrar goldens de Bosque y pistacho en tema claro y oscuro, inglés y español, para búsqueda, ficha, checkout, correcciones y cola administrativa en tamaños aplicables. Revisar pares de texto y fondo, foco y estados con etiquetas. Guardar los diffs que se aprueben.
- [ ] **1D-06** Reproducir en staging el despliegue de API y web desde las imágenes verificadas. Ensayar migración, restauración y recuperación en el entorno de pruebas. Validar liveness, readiness, conectividad externa, alertas y rollback.
- [ ] **1D-06A** Restaurar en pruebas una muestra coordinada de PostgreSQL y objetos de MinIO. Abrir una ficha y un archivo privado tras la restauración y comprobar referencias y permisos. Ensayar una recuperación de despliegue con la versión documentada.
- [ ] **1D-06B** Validar alertas de API, errores de almacenamiento y antigüedad de colas de moderación, pagos y facturas. Simular una operación detenida, seguir el procedimiento de recuperación y registrar el resultado.
- [ ] **1D-07** Completar el piloto con organizaciones iniciales y publicaciones autorizadas. Medir tiempo a primera publicación, búsquedas sin resultados y conversión a destacado. Registrar problemas de usabilidad y su resolución.
- [ ] **1D-07A** Observar a un visitante y a publicantes de prueba completar búsqueda, publicación y corrección. Registrar los puntos donde abandonan o requieren ayuda, priorizar correcciones y repetir las tareas afectadas.
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

Catálogo oficial descargado y validado el 9 de noviembre de 2026. Incluye 33 códigos de nivel departamental, con Bogotá D. C., y 1.122 registros territoriales. Su corte es el 30 de diciembre de 2024. [Departamentos](https://github.com/erickvalderrama/it/blob/main/documents/prompts/mascotas/data/colombia-departments.csv) · [Localidades DIVIPOLA](https://github.com/erickvalderrama/it/blob/main/documents/prompts/mascotas/data/colombia-divipola-localities.csv) · [Fuente, licencia e importación](https://github.com/erickvalderrama/it/blob/main/documents/prompts/mascotas/data/README.md)

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
- Mostrar fecha de fin, condiciones y precio mensual asignado al ofrecer la renovación. Cobrar la mensualidad automáticamente cada mes con la tarjeta tokenizada en ePayco, después de que el Business acepte el plan, complete sus datos de suscriptor y autorice el cobro recurrente. Cada plan tiene precio mensual y cantidad de publicaciones mensuales. El alta gratuita y el fin del beneficio se registran como eventos de la membresía.
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

- Versionar términos y política por fase (versión 1 para la fase 1, nuevas versiones antes de las fases 2 y 3), cada una en español e inglés, con texto, idioma, versión y vigencia. Registrar el idioma aceptado. Registrar usuario, documento exacto y fecha de aceptación, y autorización de datos cuando corresponda. Conservar la historia y la relación con la versión más reciente que aceptó cada usuario.
- Al iniciar sesión o abrir un flujo que requiere aceptación, mostrar el documento vigente cuando haya una versión nueva. Validar la versión en el backend antes de permitir nuevas publicaciones, contratación o pagos. Mantener consulta de documentos, historial y canales de atención. Cada operación conserva el texto aplicable al momento de su aceptación.
- Los términos identifican al publicante, tienda o prestador responsable. El publicante responde por datos y contenido del anuncio, la tienda por pedidos y entregas y el prestador por su ejecución. Mascotas cobra por sus funciones y atiende sus obligaciones de publicación, cobro, datos y soporte según su participación efectiva. Aplicar derechos obligatorios y reclamos sin exoneraciones generales. [Términos de Mascotas](https://github.com/erickvalderrama/it/blob/main/documents/prompts/mascotas/legal/terms-and-conditions.md) · [Privacidad](https://github.com/erickvalderrama/it/blob/main/documents/prompts/mascotas/legal/privacy-policy.md) · [Estatuto del Consumidor](https://www1.funcionpublica.gov.co/eva/gestornormativo/norma_pdf.php?i=44306) · [Criterio de la SIC sobre portales de contacto](https://sedeelectronica.sic.gov.co/publicaciones/boletin-juridico/concepto/redes-sociales-como-portales-de-contacto)
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
- Usar recargas para pagos pequeños. Separar el saldo del cliente destinado a compras del saldo de comisiones de Business y de las obligaciones por pagar a terceros. Documentar finalidad, vencimiento si aplica, devoluciones y costo de recaudo y abono. La puesta en marcha requiere validar admisión del proveedor y tratamiento del dinero, facturación y reglas de recarga. [Diseño de recargas](https://github.com/erickvalderrama/it/blob/main/documents/general/investigaciones/Mascotas-ManosDelBarrio-recargas-2026-11-06.md)
- La fase queda lista al completar pruebas E2E de un pedido y una reserva con cobro, factura o soporte aplicable, entrega o servicio, comisión, abono y reembolso conciliados. Cubrir conflictos de stock o cupo y reintentos de recarga y abono, según el alcance habilitado.

### 2A Catálogo, inventario y alcance Business

- [ ] **2A-01** Revisar el modelo entidad-relación de catálogo, inventario, pedido, reserva, pago y abono. Registrar las capacidades y roles que se habilitan en cada entrega.
- [ ] **2A-02** Habilitar la tienda para Business verificados. Probar consulta pública, edición propia y denegación a otra organización.
- [ ] **2A-03** Crear, editar y publicar artículos con precio, stock y fotos en MinIO. Probar cambios de disponibilidad, vista pública, errores por foto y paginación.
- [ ] **2A-04** Configurar la cobertura, costo y plazo de domicilio propio por tienda. Probar una dirección dentro de cobertura y otra fuera antes de mostrar el total.
- [ ] **2A-05** Publicar la versión 2 de términos y privacidad en español e inglés con tiendas, servicios, recargas y Payouts antes de habilitar la fase 2. CA: los usuarios deben aceptarla antes de comprar, reservar o recargar; la aceptación registra versión e idioma.

### 2B Pedidos y reservas

- [ ] **2B-01** Mostrar vendedor, artículos, cantidades, domicilio, importe y condiciones en el resumen. Congelar la oferta aceptada y conservar sus referencias en el pedido.
- [ ] **2B-02** Reservar stock durante la compra y liberarlo al vencer o cancelar. Probar dos compras sobre la última unidad y una confirmación tardía con la regla de conciliación acordada.
- [ ] **2B-03** Confirmar pago y permitir a la tienda gestionar preparación, entrega y cierre. Probar reenvíos, cambios de estado inválidos y visibilidad del seguimiento para el comprador.
- [ ] **2B-04** Publicar servicios de baño, lavado o limpieza con alcance, prestador, ubicación, fecha y cupos. Mostrar condiciones y total antes de reservar.
- [ ] **2B-05** Retener el cupo mientras se confirma el pago. Probar selección simultánea, vencimiento de la retención y pago confirmado después del vencimiento.
- [ ] **2B-06** Implementar cancelación y devolución según la política aplicable al pedido o reserva. Verificar quién puede ejecutarlas y el efecto en stock, cupo y cobro.

### 2C Abonos y recargas

- [ ] **2C-01** Verificar beneficiario, titularidad y medios admitidos para ePayco Payouts. Mostrar el estado de habilitación y conservar el acceso restringido a soportes.
- [ ] **2C-02** Calcular importe del prestador, comisión y abono desde la operación original. Probar reintento, confirmación repetida y conciliación del estado del proveedor.
- [ ] **2C-03** Configurar recargas para compras pequeñas y para comisiones Business conforme a las reglas validadas. Mantener separados el saldo de cada finalidad y las obligaciones a terceros.
- [ ] **2C-04** Probar recarga, consumo, expiración o devolución según el alcance aprobado. Verificar operaciones concurrentes, reenvíos y saldo final contra el historial.

### 2D Pruebas y salida

- [ ] **2D-01** Ejecutar E2E de pedido con domicilio propio y reserva a domicilio o agendada. Verificar cobro, soporte aplicable, entrega o servicio, comisión, abono y devolución.
- [ ] **2D-02** Revisar pantallas de tienda, checkout, reserva y operación Business en web y móvil. Guardar goldens con la paleta oficial y pruebas de foco, accesibilidad y estados recuperables.
- [ ] **2D-03** Ensayar recuperación de un abono pendiente y conciliación de recargas en staging. Registrar versión, evidencia del proveedor y procedimiento de reversión del despliegue.

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

### 3A Habilitación y alcance del piloto

- [ ] **3A-01** Registrar la validación de modalidad, operador, vehículo, rutas, especies, custodia y cobertura para el servicio definido. Vincular la evidencia aplicable a la habilitación de viajes pagados.
- [ ] **3A-02** Habilitar capacidad de transportista en Business y verificar identidad, documentación y vigencias aplicables. Probar que solo un Business habilitado puede ofertar.
- [ ] **3A-03** Configurar las rutas y condiciones del piloto, incluidos requisitos de admisión, responsable de recogida y entrega y protocolo de incidentes.
- [ ] **3A-04** Revisar el modelo entidad-relación y las reglas de acceso y conservación de solicitudes, ofertas, ubicaciones y eventos de custodia.
- [ ] **3A-05** Publicar la versión 3 de términos y privacidad en español e inglés con transporte, seguimiento de ubicación, custodia e incidentes antes de habilitar viajes. CA: los usuarios deben aceptarla antes de solicitar u ofertar un traslado; la aceptación registra versión e idioma.

### 3B Solicitud, ofertas y contratación

- [ ] **3B-01** Crear la solicitud con origen y destino en mapa, datos de mascota, hora, condiciones y presupuesto. Validar ruta y disponibilidad y explicar cómo corregir puntos inválidos.
- [ ] **3B-02** Permitir aceptar el presupuesto o presentar contraofertas con alcance, precio y vigencia. Probar oferta vencida, cambio de monto y selección simultánea.
- [ ] **3B-03** Registrar una sola aceptación con prestador, total y condiciones congelados. Proteger direcciones exactas y contactos para las partes autorizadas.
- [ ] **3B-04** Ejecutar el cobro confirmado y conservar precio, comisión y remuneración. Probar timeout, confirmación repetida y consulta de estado tras reabrir el flujo.

### 3C Recogida, seguimiento y entrega

- [ ] **3C-01** Confirmar recogida y entrega con los participantes autorizados y registrar actor, hora y estado. Probar acciones repetidas y fuera de orden.
- [ ] **3C-02** Iniciar el seguimiento con permiso explícito durante el viaje. Mostrar el último punto autorizado y su hora en web y móvil.
- [ ] **3C-03** Probar Android e iOS en segundo plano y con pantalla bloqueada. Cubrir permiso denegado o revocado, pérdida de conexión y recuperación con un estado comprensible.
- [ ] **3C-04** Detener el seguimiento al completar o cancelar. Verificar permisos tras el cierre y conservar solo la información prevista por la política aprobada.
- [ ] **3C-05** Permitir registrar cancelación o incidente por el canal acordado. Ensayar el acceso de soporte a la operación y el registro de atención.

### 3D Conciliación, pruebas y salida

- [ ] **3D-01** Conciliar pago, recarga aplicable, cargo de Mascotas, remuneración, abono y devolución. Probar reintentos y permisos sobre cada operación.
- [ ] **3D-02** Ejecutar E2E de solicitud, contraoferta, aceptación, cobro, recogida, entrega y conciliación. Incluir cancelación, mala señal y ubicación no disponible.
- [ ] **3D-03** Revisar mapas, ofertas y seguimiento en las plataformas habilitadas. Guardar capturas y resultados de accesibilidad y de las comprobaciones específicas de móvil.
- [ ] **3D-04** Verificar habilitación aplicable, soporte y recuperación con el operador del piloto. Registrar versión desplegada y evidencias antes de abrir los viajes pagados.

## Registro de avance

Registrar cada cierre o bloqueo por ID, con fecha, estado y evidencia reproducible. Para una prueba, incluir comando o ejecución de CI y resultado. Para un cambio visual, adjuntar la captura o el diff. Para un bloqueo, indicar la dependencia concreta y el siguiente trabajo disponible.

| Fecha | Tarea | Estado | Evidencia |
| --- | --- | --- | --- |
| 2026-11-11 | 1.0-01 | Verificada | [Memoria](https://github.com/erickvalderrama/it/blob/main/CLAUDE.md) · [Especificación](https://github.com/erickvalderrama/it/blob/main/documents/prompts/mascotas/phase-1-specification.md) |
| 2026-11-11 | 1.0-02 | Verificada | [Catálogo DANE y fuentes](https://github.com/erickvalderrama/it/blob/main/documents/prompts/mascotas/data/README.md) · [Departamentos](https://github.com/erickvalderrama/it/blob/main/documents/prompts/mascotas/data/colombia-departments.csv) · [Localidades](https://github.com/erickvalderrama/it/blob/main/documents/prompts/mascotas/data/colombia-divipola-localities.csv) |
| 2026-11-11 | 1.0-03 | Verificada como borradores | [Términos](https://github.com/erickvalderrama/it/blob/main/documents/prompts/mascotas/legal/terms-and-conditions.md) · [Privacidad](https://github.com/erickvalderrama/it/blob/main/documents/prompts/mascotas/legal/privacy-policy.md) |
| 2026-11-12 | 1.0-04 | Verificada | Repositorio `erickvalderrama/it`, rama `main`, carpetas `back/` y `front/`. Comandos en `README.md`. |
| 2026-11-12 | 1A.1-01 | En curso | `back/`: .NET 10.0.401 fijado en `global.json`, versiones centralizadas en `Directory.Packages.props` (EF Core 10.0.15, Npgsql 10.0.3), advertencias como errores. `front/`: Flutter 3.41.2, Riverpod 3.3.2, gen-l10n. `dotnet build` y `flutter analyze` sin advertencias. Falta: versión del servidor PostgreSQL y compilación desde un checkout limpio tras el commit. |
| 2026-11-12 | 1A.1-01A | Verificada | Flujo de países habilitados: `CountryCode` (Domain) → `ListEnabledCountries` + puerto `ICountryCatalog` (Application) → catálogo por configuración (Infrastructure) → `GET /api/v1/locations/countries` (API) → repositorio HTTP, caso de uso y controlador Riverpod → pantalla. `dotnet test`: 16 pruebas; `flutter test`: 7 pruebas (inglés, español, oscuro, vacío, error y reintento). |
| 2026-11-12 | 1A.1-01B | Verificada | `SoftDeletableEntity` (estado, `DeletedAt`, `DeletedBy`, motivo), `SoftDeleteInterceptor` y `ApplyPlatformConventions` (filtro global, índice de estado, borrado restringido en llaves foráneas, `HasActiveUniqueIndex` parcial). 4 pruebas contra PostgreSQL 17 + PostGIS en contenedor (Testcontainers): la fila se conserva, la consulta normal la oculta, el correo se reutiliza solo tras borrar y la restauración funciona. 3 pruebas de dominio. |
| 2026-11-12 | 1A.1-01C | Verificada | Primera migración `InitialCatalogs` (países, departamentos, localidades, especies, razas y traducciones de catálogo), pedida por el responsable. Script idempotente con `dotnet ef migrations script --idempotent`; CI falla con cambios de modelo sin migración (`has-pending-model-changes`). Prueba en PostgreSQL vacío: sin migraciones pendientes, índices únicos parciales, índice del selector de localidades, todas las llaves foráneas con `RESTRICT`, códigos con ceros iniciales y rechazo de una localidad sin departamento. Planes revisados con los 1.122 registros importados: búsqueda por código con `ix_localities_country_code_code` (0,05 ms); el selector completo lee la tabla (6,6 ms) porque todas las filas cumplen el filtro. |
| 2026-11-12 | 1E-01 | En curso | Catálogos del ERD adaptados: `Animal_Types` → `species`, `Breeds` → `breeds`, ubicaciones por DIVIPOLA, `Genders` como enum. Falta cuestionarios, mascotas y adopciones, que dependen del modelo de cuentas. |
| 2026-11-12 | 1A.2-01 | Verificada | Comando `import-catalogs` con `back/data/catalogs/manifest.json` versionado. Importa DIVIPOLA (`DANE-2024-12-30`): 33 departamentos y 1.122 localidades en `petyei_local`; `08001`, `11001` y `47001` conservan el cero inicial. Una segunda ejecución no inserta ni modifica nada. |
| 2026-11-12 | 1A.2-01A | Verificada | Validación previa sin escribir nada si algo falla: columnas, un solo país, códigos duplicados, departamento inexistente, prefijo de código, nombres vacíos y conteo por tipo (Municipio 1.103, Isla 1, Área no municipalizada 18). La reimportación conserva los identificadores y `is_selectable`; una nueva versión de fuente actualiza en sitio. Nada se borra; los códigos ausentes se reportan. 7 pruebas de lectura y 3 contra PostgreSQL con los archivos reales. |
| 2026-11-12 | 1E-02 | Verificada | `species-breeds.csv` bilingüe a partir del Excel de la tesis: 20 especies, 121 razas y 282 traducciones. "Cobaya (Cuyo)" y "Conejillo de Indias" unidas como `guinea_pig`; "Cerdo Barrigón" unido a `vietnamese_potbellied`; "Mestizo" agregado para gatos. Reimportación sin cambios y razas ligadas a su especie. |
| 2026-11-12 | 1A.2-02 | Verificada | Mockup previo en `documents/mockups/1A.2-02-city-picker/` (`index.html`, `web.png`, `mobile.png`). API: `GET /api/v1/locations/countries/{country}/localities` agrupado por departamento con nombre oficial y de presentación ("SANTA MARTA" → "Santa Marta"), caché de una hora, y `GET .../localities/{code}` que rechaza ciudades inexistentes o deshabilitadas con `locations.locality_not_available`. Países y localidades salen de la base (se retiró el catálogo por configuración). Flutter: Colombia preseleccionada, grupos por departamento, búsqueda sin tildes ni mayúsculas con resaltado, vacío con "Borrar búsqueda", error con reintento, ciudad elegida con fondo pistacho y ✓, pantalla completa en móvil y diálogo sobre la app en web con URL `/account/city`; la ciudad se guarda en el dispositivo. Capturas de la app real junto al mockup (`implemented-web.png`, `implemented-mobile.png`). La revisión visual destapó que Flutter web usaba direcciones con `#` y las rutas profundas abrían Explorar: se activó `usePathUrlStrategy()` y `/account/city` abre el selector al cargar. Campos redondeados en el tema global, como en el mockup. Pruebas: 5 de API, 6 de nombres, 1 contra PostgreSQL (1.122 seleccionables, una deshabilitada desaparece) y 10 de Flutter. |
| 2026-11-12 | 1A.1-03B | Verificada | Contenedor genérico `postgres-local` (PostgreSQL 17.5 + PostGIS) en `127.0.0.1:5433`, volumen `postgres-local-data`, superusuario `postgres`. Base `petyei_local` con dueño `petyei` creada por `tools/create-local-database.sh` (acceso de `PUBLIC` revocado, PostGIS activo). `InitialCatalogs` aplicada con el usuario `petyei`; tablas propiedad de `petyei`. Contraseñas generadas al azar en `.env`, ignorado por git. Renombrado a pedido del responsable porque el servidor se usará en otros proyectos. |
| 2026-11-12 | 1A.1-02 | Verificada | `back/Dockerfile`, `front/Dockerfile` (Flutter 3.41.2 → nginx con fallback SPA) y `compose.yaml`. `docker compose config`, `build`, `up -d` y `ps` correctos. API en :5180 (`/api/v1/locations/countries` 200, `/health/live` 200, 404 con Problem Details), web en :5181 con ruta profunda `/adoptions/123` 200; CORS solo para el origen configurado. Captura de la web en español mostrando Colombia desde la API. Puertos 8080/8081 cambiados porque ya estaban ocupados en la máquina. |
| 2026-11-12 | 1A.1-03 | En curso | `.env.example` sin secretos, `ConnectionStrings:Petyei`, `Countries:Enabled` validado al arrancar y `Cors:AllowedOrigins`. Faltan PostgreSQL externo, MinIO y la validación de valores ausentes de esos servicios. |
| 2026-11-12 | 1A.1-08 | En curso | `.github/workflows/ci.yml`: backend (formato, build, pruebas con PostgreSQL en contenedor), frontend (traducciones, formato, análisis, pruebas) y verificaciones (699 líneas, compose config y build). Pendiente la primera ejecución en GitHub tras el push. |
| 2026-11-12 | 1A.4-01 | Verificada | gen-l10n con `app_en.arb` (plantilla) y `app_es.arb`, extensión `context.l10n`, ICU `select` para nombres de país. `tools/check-l10n.py` falla con texto literal en widgets. |
| 2026-11-12 | 1A.4-05 | Verificada | `tools/check-l10n.py`: claves faltantes por idioma, claves sin uso y texto literal. Prueba negativa: al quitar `retryAction` de `app_es.arb` falla con "Locale 'es' is missing translations: retryAction". |
| 2026-11-12 | 1A.4-08 | Verificada | Tokens claros y oscuros en `AppColors` y `AppTheme`; prueba de widget confirma el `primary` oscuro `#B9D88A`. `tools/check-theme-colors.sh` en CI falla con colores fuera de `lib/app/theme` (prueba negativa con `Colors.red`). Contraste medido entre 7,6:1 y 14,6:1. |
| 2026-11-12 | 1A.1-05 | Verificada | Errores RFC 9457 con extensión `code` estable (`locations.invalid_country_code`, `locations.country_not_enabled`, `common.not_found`), `field` y `correlationId`. `X-Correlation-Id` seguro se devuelve; uno inseguro se reemplaza. `/health/live` sin dependencias y `/health/ready` con la base: 503 sin conexión configurada y 200 con PostgreSQL en contenedor. OpenAPI en Development incluye `/api/v1/locations/countries/{code}`. 12 pruebas de API. |
| 2026-11-12 | 1A.1-05A | En curso | Ejemplo de catálogo y problema de validación publicados en el contrato; Flutter deserializa `code`, `field` y `correlationId` (prueba de repositorio). Falta paginación, que llega con el primer listado. |
| 2026-11-12 | 1A.1-07B | En curso | Navegación Explore, Publish y Account con `go_router` y URL por sección; barra inferior en compacto y riel lateral desde 840 px; componente `StatusMessage` para vacío y error. Pruebas: enlace profundo a `/account`, cambio de sección en pantalla amplia. Falta foco, lector de pantalla y retorno tras autenticación, que dependen del inicio de sesión. |
| 2026-11-12 | 1A.4-02 | En curso | Idioma del dispositivo con caída a inglés; selector en Account antes de iniciar sesión; el cambio se aplica sin reiniciar, conserva la ruta y se guarda en el dispositivo. Falta guardarlo en la cuenta. |
| 2026-11-12 | 1A.4-03 | En curso | La app muestra mensajes en inglés y español por código de error de la API, con mensaje genérico para códigos desconocidos y fallos de red (pruebas de widget). Faltan los correos localizados, que llegan con el registro. |
| 2026-11-12 | 1A.4-09 | En curso | Sistema, Claro y Oscuro en Account, Sistema por defecto, aplicado al instante y guardado en el dispositivo; se restaura al iniciar. Falta guardarlo en la cuenta. |
| 2026-11-12 | CI | Corregido | La primera ejecución en GitHub (`09065d8`) falló en `flutter pub get` y en `docker compose build`: gen-l10n escribía su reporte en `build/l10n`, que no existe en un checkout limpio. El reporte pasó a `lib/l10n/untranslated.json` (ignorado por git); `flutter pub get` funciona en una copia limpia. |
| 2026-11-11 | 1.0-05 | Pendiente del modelo | La ruta del modelo entidad-relación aprobado sigue pendiente en la memoria. |
| 2026-11-11 | 1.0-06 | Verificada como decisión de diseño | [Memoria principal](https://github.com/erickvalderrama/it/blob/main/CLAUDE.md#uiux-and-flutter-interface-for-web-and-mobile) · [Tokens oficiales](https://github.com/erickvalderrama/it/blob/main/documents/prompts/mascotas/phase-1-specification.md#official-light-theme) |
| 2026-11-11 | 1.0-07 | Verificada como decisión del responsable | Nombre Petyei. `petyei.com` sin registrar según RDAP de Verisign. [Marca y bocetos](https://github.com/erickvalderrama/it/blob/main/documents/brand/README.md) |
| 2026-11-11 | 1.0-08 | Pendiente | Falta confirmar precio estándar del dominio, registrarlo y revisar la marca en la SIC. |

## Preguntas para el cierre de la ejecución

Revisar esta lista al terminar el trabajo ejecutable. Conservar solo las preguntas vigentes que requieran una decisión o un dato del responsable. Registrar cada respuesta junto a la tarea afectada y continuar con el primer punto que desbloquee.

| Tarea | Información que desbloquea el siguiente paso |
| --- | --- |
| 1A.1-01 | ¿Qué versión de PostgreSQL tiene el servidor externo y tiene PostGIS disponible? |
| 1.0-05 | ¿Dónde está la versión aprobada del modelo entidad-relación? |
| 1.0-08 | ¿Se registró `petyei.com` a precio estándar y cuál fue el resultado de la búsqueda de marca? |
| 1A.1-04 | ¿Cuáles son los destinos y el mecanismo autorizado para configurar PostgreSQL y MinIO por ambiente? Compartir la ubicación de la configuración segura. |
| 1B.2-05 | ¿Cuál es el canal inicial para solicitar o contactar por una adopción? |
| 1C.2-06 | ¿Cuáles son los precios, beneficios, vigencia promocional, emisor fiscal y reglas de devolución definitivos del piloto? |
| 1C.1-06 | ¿Qué planes de suscripción habrá, con qué precio y cuántas publicaciones mensuales? ¿Las publicaciones no usadas se acumulan? ¿Qué pasa al agotar el cupo? ¿Solo Business se suscribe o también User? |
| 1C.1-06C | ¿Qué APIs de tokenización, clientes y cobro recurrente tiene habilitadas la cuenta de ePayco? |
| 1E-08 | ¿Cuál es el umbral de datos para entrenar el primer modelo y qué evento cuenta como adopción exitosa? |
| 1A.4-13 | ¿Qué idioma prevalece en términos y privacidad si hay diferencias entre español e inglés? ¿Quién revisa la traducción legal al inglés? |
| 1A.1-03A | ¿Quién es el titular de la cuenta de Google Cloud y su facturación? ¿Cuál es el presupuesto mensual máximo para Google Maps? |
