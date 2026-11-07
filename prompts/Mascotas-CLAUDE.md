# CLAUDE.md — Proyecto Mascotas

## Objetivo y contexto
Eres un colaborador senior de desarrollo. Implementa cambios mantenibles, comprobables y ajustados al alcance solicitado.
El fundador tiene experiencia en C#, .NET, frontend, backend, servidores e infraestructura. Responde en español, breve y directo.

## Stack confirmado
- Backend: C# con ASP.NET Core Web API.
- Base de datos: PostgreSQL; acceso con Entity Framework Core y Npgsql.
- Aplicación móvil multiplataforma: Flutter con Dart, para Android e iOS.
- Gestión de estado: respeta la existente; si el proyecto aún no tiene una definida, usa Riverpod como propuesta inicial.
- Antes de instalar dependencias, comprueba versiones existentes y compatibilidad en documentación oficial. Registra versiones concretas en la configuración del proyecto.

## Regla de oro: modelo entidad-relación
El modelo entidad-relación aprobado es la fuente de verdad de la persistencia y tiene prioridad sobre preferencias de arquitectura o convenciones.
1. Localiza el modelo vigente y registra aquí su ruta. Ruta del modelo aprobado: PENDIENTE DE IDENTIFICAR.
2. Respeta entidades, tablas, columnas, tipos, precisión, nulabilidad, claves, relaciones, cardinalidades, restricciones y reglas de borrado definidas.
3. No inventes tablas, campos, relaciones o valores persistidos para facilitar una pantalla, una librería o un patrón.
4. No agregues automáticamente tablas de autenticación, auditoría, saldo, seguimiento o borrado lógico si no están contempladas.
5. Puedes adaptar nombres de clases y DTOs mediante mapeos explícitos, conservando los nombres y contratos de la base de datos.
6. Si falta el modelo para una tarea de persistencia, identifica el bloqueo y solicita ese insumo; avanza solo en trabajo independiente que no invente el esquema.
7. Si una funcionalidad requiere cambiarlo, explica el cambio concreto y su impacto antes de implementarlo. Con autorización ya existente, actualiza modelo, entidades, mapeos, migración y pruebas de forma coordinada; no pidas otra confirmación por el mismo cambio.
8. Si el modelo, las migraciones y la base existente discrepan, reporta la diferencia. No elijas silenciosamente una versión ni sobrescribas datos.
9. No ejecutes borrados, recreaciones o migraciones sobre bases reales sin autorización para esa operación. No uses EnsureCreated para sustituir migraciones en producción.
10. Los módulos funcionales de este documento NO autorizan crear tablas fuera del modelo.

## Arquitectura limpia del backend
Empieza con un monolito modular y estas responsabilidades:
- Mascotas.Domain: entidades, objetos de valor, invariantes y reglas de negocio. Sin referencias a EF Core, ASP.NET, proveedores externos o Flutter.
- Mascotas.Application: casos de uso, contratos, DTOs, validación e interfaces de servicios y persistencia. Depende de Domain.
- Mascotas.Infrastructure: DbContext, mapeos Fluent API, repositorios, migraciones e integraciones externas. Implementa los contratos de Application.
- Mascotas.Api: endpoints, autenticación/autorización HTTP, validación de entrada y traducción de errores. Invoca Application; registra Infrastructure en el punto de composición de dependencias.
- Tests: pruebas relevantes de dominio, casos de uso e integración con PostgreSQL.

Mantén reglas de negocio fuera de controllers, endpoints y repositorios.
Organiza por funcionalidad dentro de cada capa. Añade abstracciones cuando resuelvan una necesidad concreta.
No expongas entidades de persistencia como contratos públicos de la API.
Usa operaciones asíncronas para E/S, CancellationToken donde aplique, manejo uniforme de errores y consultas paginadas.
Evita consultas N+1; proyecta solo lo necesario. Las optimizaciones deben conservar el modelo aprobado.

## Arquitectura limpia de Flutter
Organiza por funcionalidades:
- lib/app: arranque, navegación, composición de dependencias y tema.
- lib/core: infraestructura y componentes realmente compartidos.
- lib/features/<funcionalidad>/domain: modelos de dominio, contratos de repositorios y casos de uso necesarios, sin dependencias de widgets, HTTP o almacenamiento.
- lib/features/<funcionalidad>/data: DTOs, serialización, fuentes de datos y repositorios que implementan los contratos del dominio.
- lib/features/<funcionalidad>/presentation: páginas, widgets, estado y controladores de presentación.

La UI no llama directamente al cliente HTTP ni decide reglas financieras o de negocio.
Separa modelos de API, dominio y presentación cuando sus responsabilidades difieran.
Mantén estado fuera de widgets visuales; usa estado local solo para interacción visual local.
Diseña estados de carga, vacío, error, reintento y éxito.
Reutiliza widgets pequeños por responsabilidad; comparte entre funcionalidades solo lo que tenga un uso real.

## Límite obligatorio de archivos
- Cada archivo de código fuente debe tener MENOS DE 700 LÍNEAS: máximo 699.
- Incluye widgets, páginas, controllers, casos de uso, entidades, mapeos, pruebas y migraciones.
- Cuenta también comentarios y líneas vacías.
- Si un archivo va a superar el límite, divídelo por responsabilidades antes de terminar el cambio.
- No eludas la regla minificando, acumulando instrucciones en una línea o creando divisiones sin sentido.
- No edites código generado para aparentar cumplimiento; ajusta sus fuentes o generación. Si no puede cumplir, informa el bloqueo concreto, sin declarar cumplida la regla.
- Dependencias externas, binarios y cachés no son código fuente propio.
- Incorpora una comprobación automática del límite en las verificaciones del proyecto.
- Una pantalla compuesta por varios widgets debe ser legible; no traslades toda su complejidad a otro archivo monolítico.

## Alcance funcional de Mascotas
- Adopciones: publicaciones básicas, perfiles e información de contacto.
- Publicaciones de adopción premium: el publicante paga a la app por destacar un anuncio. Definir precio, duración y beneficio; no es venta del animal ni garantía de adopción.
- Transporte exclusivo de mascotas: origen A, destino B, fecha, condiciones de la mascota, presupuesto, aceptaciones o contraofertas y elección explícita del cliente.
- Servicios para mascotas: baño, limpieza, peluquería y belleza a domicilio. La atención en casa necesita una ubicación; no fuerces un destino de transporte.
- Perfiles de clientes y prestadores, ofertas con inclusiones, seguimiento de estados y valoraciones de operaciones reales.
- Recargas: priorizar saldo del cliente para varias compras pequeñas. Separarlo del saldo del prestador que prepaga comisiones.
Implementa por entregas pequeñas. No incorpores funcionalidad adicional solo porque parezca habitual en un marketplace.

## Pagos, saldo y consistencia
- El backend calcula y valida importes, cargos, permisos y transiciones. Flutter muestra el resultado autorizado.
- Usa representación monetaria exacta y reglas explícitas de moneda y redondeo, coherentes con el modelo y el contrato de API; no uses punto flotante para cálculos de dinero.
- Confirma pagos desde el backend con el mecanismo documentado del proveedor. No aceptes redirecciones o capturas como prueba suficiente.
- Haz idempotentes confirmaciones, recargas, consumos, devoluciones y órdenes de abono. Protege actualizaciones concurrentes con operaciones atómicas acordes al modelo.
- Separa saldo disponible, reservado y consumido; dinero que corresponde a la app y al prestador. Si el modelo no permite representarlo, plantea la modificación antes de persistir.
- No cobres la misma comisión desde el saldo y desde el abono al prestador.
- Las recargas no eliminan porcentajes de pasarela, impuestos ni costos de dispersión. El saldo sin consumir no es utilidad libre.
- Mantén ePayco como integración desacoplada; activación, condiciones comerciales y cobro por canal deben confirmarse antes de producción.
- Usa datos de prueba y sandbox mientras corresponda; no declares una integración operativa sin verificarla.

## Transporte y ubicación
- El seguimiento del prestador comienza mediante una acción explícita asociada a un servicio activo y termina al finalizarlo o cancelarlo.
- Solicita únicamente los permisos y la precisión que necesita cada rol.
- Evalúa el plugin, compatibilidad y licencia antes de incorporarlo. Flutter no elimina las restricciones de Android/iOS.
- Prueba en dispositivos reales: pantalla bloqueada, segundo plano, pérdida de red, recuperación de conexión y finalización del servicio.
- Diseña envío con reintentos y control de duplicados; muestra cuándo fue la última ubicación recibida.
- No garantices seguimiento continuo si el sistema o el usuario termina la app.
- Compartir ubicación no demuestra recogida o entrega: registra la confirmación de los responsables.

## Seguridad y calidad
- Autoriza por operación y por recurso: conocer un ID no permite consultar o modificar datos de otro usuario.
- Guarda secretos en configuración segura del entorno; no en Git, widgets o logs.
- Protege direcciones, ubicación y datos de pago. Evita registrar información sensible.
- Implementa pruebas que comprueben reglas reales: permisos, estados, contratos, persistencia y pagos sin duplicados. No añadas pruebas que solo repitan la implementación.
- Ejecuta las comprobaciones disponibles y pertinentes: dotnet build/test, dart format, flutter analyze y flutter test.
- Informa qué ejecutaste, qué pasó y qué no pudiste comprobar; no inventes resultados.

## Forma de trabajar
Antes de cambiar código, lee este archivo, instrucciones aplicables, modelo entidad-relación y código relacionado.
Resuelve decisiones rutinarias dentro del alcance autorizado y conserva los cambios existentes del usuario.
Para cada tarea: identifica el impacto en capas y modelo, implementa una entrega concreta y verifica compilación, comportamiento relevante y límite de líneas.
Mantén sincronizados documentación, contratos y migraciones cuando el cambio los afecte.
No construyas toda la aplicación por iniciativa propia al recibir este archivo: espera una tarea concreta y ejecútala dentro de estas reglas.
Al terminar, resume qué cambió, archivos principales, validaciones y bloqueos reales.
