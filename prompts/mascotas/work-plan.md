# Plan de trabajo de Mascotas

Decisiones registradas el 9 de noviembre de 2026. Proyecto para un primer lanzamiento en Colombia con Flutter para web, Android e iOS, Riverpod como opción inicial, C# con .NET y PostgreSQL. La memoria técnica conserva arquitectura limpia, código en inglés, textos base en constantes, pruebas unitarias, de integración, E2E y de regresión visual, seguridad, máximo 699 líneas por archivo de código y el modelo entidad-relación aprobado como regla de oro. [Memoria técnica](../Mascotas-CLAUDE.md)

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

Usar la especificación de implementación para convertir las entregas en tareas, con contratos de API y datos, recorridos UI/UX, estados y casos de aceptación. Las rutas y los límites propuestos se ajustan al código existente y al modelo aprobado. [Especificación de fase 1](phase-1-specification.md)

### Entregas en orden de dependencia

| Entrega | Trabajo | Flujo E2E de salida |
| --- | --- | --- |
| 1A. Acceso y catálogo | Registro breve, Colombia preseleccionado, selección de municipio, roles, alta Business y aceptación de términos por versión. | Una persona se registra con Colombia preseleccionada, selecciona y cambia su ciudad, completa un alta Business y acepta una nueva versión de términos. |
| 1B. Publicar y revisar | Ficha de adopción con fotos, borradores, búsqueda, filtros por ciudad y origen, revisión por IA y cola manual. | Una publicación básica aprobada aparece con el filtro de ciudad y publicante. Se evalúan el texto y todas las fotos. Los casos dudosos llegan a Superadmin y el contenido sexual o vulgar se rechaza. |
| 1C. Cobro e historial | Versiones de precios, promoción Business, mensualidad, compra de anuncio pagado, ePayco y facturación electrónica. | Una compra de prueba conserva precio y términos aceptados. El pago confirmado envía el anuncio a revisión. Las correcciones se reenvían con el mismo pago. Se comprueban un solo cobro aplicado, la visibilidad tras aprobación y la factura, incluso con confirmaciones repetidas. |
| 1D. Operación y piloto | Panel con usuarios registrados y activos, administración de anuncios, cobros, facturas y reclamos. Prueba de interfaz web y móvil y piloto con organizaciones. | Superadmin puede moderar, consultar la operación y recuperar una factura pendiente. Se verifican dos ciudades, los tres roles y el aislamiento entre cuentas. Pasan las comparaciones visuales de las pantallas clave. |

### Registro, ciudades y roles

- El registro personal pide nombre, correo, contraseña, país y ciudad. Colombia aparece preseleccionada como país habilitado. El selector agrupa ciudades por departamento y guarda su código DIVIPOLA. Para el piloto, se propone verificar el correo antes de publicar y ofrecer recuperación de contraseña.
- Guardar país, región y municipio como referencias del catálogo del modelo aprobado. Usar los códigos oficiales como texto para conservar ceros iniciales. La ciudad del anuncio puede diferir de la ciudad del perfil cuando corresponde a la ubicación de la mascota.
- Importar los dos CSV versionados, conservar los nombres y tipos de DANE y controlar cuáles ubicaciones se muestran en el selector. Actualizar el catálogo por código y versión de fuente para preservar las referencias históricas. Colombia permanece como país operativo de esta fase. Habilitar otros países por configuración y por catálogo.
- Usar los roles **User**, **Business** y **Superadmin**. Business cubre tanto fundaciones como tiendas y, en fase 3, transportistas habilitados. **Foundation** y **Store** son tipos de perfil, con capacidades autorizadas por recurso.
- En el alta Business y en la conversión de una cuenta existente, la primera elección es **tienda** o **fundación**. Conservar la misma cuenta y sus operaciones. Otorgar acceso Business después de las verificaciones que correspondan. Una fundación puede operar sin tienda y solicitar la capacidad de tienda posteriormente.
- Al inscribirse como tienda o habilitar tienda desde un perfil, pedir tipo de persona y los soportes solicitados. Persona natural presenta cédula y RUT. Persona jurídica presenta NIT y RUT. Validar representación cuando proceda. Proteger documentos y limitar su consulta a personas autorizadas. La verificación se activa al solicitar tienda, con el registro personal separado de este trámite.
- En fase 1 se permite inscribir organizaciones y preparar perfiles de tienda para la promoción. La venta de artículos y las reservas de servicios se habilitan en fase 2. Superadmin gestiona altas, revisión, precios, términos y acceso mediante permisos y auditoría.

Catálogo oficial descargado y validado el 9 de noviembre de 2026. Incluye 33 códigos de nivel departamental, con Bogotá D. C., y 1.122 registros territoriales. Su corte es el 30 de diciembre de 2024. [Departamentos](data/colombia-departments.csv) · [Localidades DIVIPOLA](data/colombia-divipola-localities.csv) · [Fuente, licencia e importación](data/README.md)

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
- Los términos identifican al publicante, tienda o prestador responsable. El publicante responde por datos y contenido del anuncio, la tienda por pedidos y entregas y el prestador por su ejecución. Mascotas cobra por sus funciones y atiende sus obligaciones de publicación, cobro, datos y soporte según su participación efectiva. Aplicar derechos obligatorios y reclamos sin exoneraciones generales. [Términos de Mascotas](legal/terms-and-conditions.md) · [Privacidad](legal/privacy-policy.md) · [Estatuto del Consumidor](https://www1.funcionpublica.gov.co/eva/gestornormativo/norma_pdf.php?i=44306) · [Criterio de la SIC sobre portales de contacto](https://sedeelectronica.sic.gov.co/publicaciones/boletin-juridico/concepto/redes-sociales-como-portales-de-contacto)
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
- Usar recargas para pagos pequeños. Separar el saldo del cliente destinado a compras del saldo de comisiones de Business y de las obligaciones por pagar a terceros. Documentar finalidad, vencimiento si aplica, devoluciones y costo de recaudo y abono. La puesta en marcha requiere validar admisión del proveedor y tratamiento del dinero, facturación y reglas de recarga. [Diseño de recargas](../../investigaciones/Mascotas-ManosDelBarrio-recargas-2026-11-06.md)
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

## Dependencias para comenzar

1. Alinear el modelo entidad-relación con ubicación, perfiles Business, versiones de precio y de documentos, aceptación, moderación y cobro. Registrar su ubicación en la memoria del proyecto.
2. Aprobar reglas del piloto, alcance y vigencia de la promoción, importe de los productos propios, emisor de facturas y condiciones de devolución.
3. Completar operador, contacto, tratamiento de datos y versiones legales antes del lanzamiento. Probar ePayco, facturación y revisión de IA con credenciales y entornos autorizados.
4. Ejecutar en orden 1A, 1B, 1C y 1D. Seleccionar la siguiente fase tras validar la adopción y la capacidad de operación.
