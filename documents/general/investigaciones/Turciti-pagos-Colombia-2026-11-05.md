# Turciti: pagos, impuestos y validación comercial

Fecha de corte: 5 de noviembre de 2026, hora de Colombia.

Estado: investigación documental y ejemplos de planificación. No se activó un marketplace, integró una API, efectuó una compra o dispersión de prueba ni contrató a un proveedor. La posibilidad técnica de cobrar y transferir no acredita autorización contractual para recaudar por negocios independientes.

## 1. Necesidad y capacidades investigadas

El usuario quiere que las personas compren dentro de Turciti mediante varios medios de pago y que el negocio reciba en su cuenta bancaria. Prefiere evitar que cada local se registre en otra pasarela. También quiere explorar un recargo al comprador, sin descontar la comisión de Turciti del precio base acordado con el comercio.

Hay dos modelos distintos:
- Split Payments: un cobro se reparte entre participantes habilitados por la pasarela. El split de ePayco requiere cuentas verificadas y asociadas.
- Recaudo más Payouts: Turciti recibe el saldo del cobro en su cuenta de la pasarela y posteriormente ordena pagar a la cuenta bancaria del negocio. Evita exigir una cuenta ePayco al beneficiario bancario, pero no es un abono directo comprador-negocio ni elimina los datos y controles del beneficiario.

Antes de producción faltan admisión del modelo de marketplace, activación, credenciales y alcance de la API, confirmaciones verificables, disponibilidad real de fondos, conciliación, impuestos, conocimiento de contrapartes y reglas de devoluciones y contracargos.

Fuentes:
- https://docs.epayco.com/docs/split-descripcion
- https://epayco.com/pagos-divididos/
- https://docs.epayco.com/docs/qu%C3%A9-es-epayco-payouts
- https://epayco.com/terminos-y-condiciones-generales-2026/

## 2. Tarifas documentadas

Todas las cantidades son pesos colombianos. IVA del 19% sobre tarifas gravadas en las simulaciones; no equivale a sumar 19% a toda venta indiscriminadamente.

| Proveedor / producto | Cobro publicado | Dispersión bancaria publicada | Condiciones relevantes |
| --- | --- | --- | --- |
| ePayco agregador con Davivienda vinculada | 2,64% + $690 + IVA | Payouts: $2.400 + IVA = $2.856 por orden bancaria | Mensualidad de Payouts $0. Vincular Davivienda no elimina el costo del Payout a terceros. |
| ePayco con otros bancos | 3,29% + $700 + IVA | Payouts: misma tarifa bancaria observada en el panel | No confundir retiro de la propia cuenta y pago a proveedores. |
| Wompi Plan Avanzado y Wompi Cuenta, tarifa general | 2,65% + $700 + IVA | ($1.849 + 0,4% del monto transferido) + IVA | Los planes mensuales asociados a cuentas Bancolombia son otro producto; no se usaron aquí. |
| Kushki | Cotización | Cotización | Se verificaron cobro y Payouts por API en Colombia; no se obtuvo una oferta numérica. Publica posibles mínimos de facturación según país y comercio. No afirmar ahorro frente a Wompi. |

Confirmación de ePayco Payouts: la página pública anuncia $2.400 sin aclarar IVA. El usuario aportó el 5 de noviembre de 2026 una captura del panel autenticado de ePayco que muestra expresamente $2.400 COP + IVA tanto en la columna Davivienda/DaviPlata como en otros bancos. Esto resuelve la duda del IVA del ejemplo: $2.856. La captura también muestra mensualidad $0 y transferencias a cuentas ePayco gratis. No se probó el servicio ni se publicó la captura del usuario en el repositorio.

La captura anuncia abonos inmediatos en la columna Davivienda/DaviPlata y ciclos ACH para otros bancos. Son condiciones mostradas, no tiempos medidos ni garantía de que el saldo de una venta esté disponible inmediatamente.

Excepción importante de recaudo ePayco: su página de tarifas indica PSE por menos de $60.000 a $2.000 + IVA = $2.380. Las tablas siguientes modelan TARJETA; no aplicarles automáticamente esa misma comisión a PSE.

La portada y la página de tarifas de ePayco publican información distinta sobre retiros de fondos propios. No trasladar sus retiros gratuitos a los Payouts para negocios ni sumar retiro propio y Payout como si ambos fueran inevitables en la misma ruta.

Fuentes:
- https://epayco.com/tarifas/
- https://epayco.com/payouts/tarifas/
- https://epayco.com/
- https://wompi.com/es/co/planes-tarifas/
- https://wompi.com/es/co/soluciones/payouts
- https://docs.wompi.co/docs/colombia/consulta-saldos-pagos-a-terceros/
- https://docs.kushki.com/co/
- https://docs.kushki.com/co/payouts/transfer/transfer-out/
- https://www.kushkipagos.com/tarifas-comisiones

## 3. Otras alternativas revisadas

- Nequi Negocios: los enlaces de pago se apoyan en Wompi. Se revisaron tarifas de enlaces de 1,5% + IVA para Nequi/Bancolombia, 1,99% + IVA para tarjetas y 2,69% + IVA para PSE de otros bancos. No son una cotización de API ni un split automático para Turciti. El negocio debe activar su servicio; QR gratuito no demuestra confirmación por API o reparto gratuito.
  https://ayuda.nequi.com.co/hc/es/articles/38968957037069-Tarifas-de-la-app-Nequi-Negocios
  https://www.nequi.com.co/negocios/apis
- dLocal Go: el split exige cuentas aprobadas de participantes. dLocal for Platforms es un producto distinto con alta por API y verificación integrada; disponibilidad de KYC, activación y tarifas para Colombia deben confirmarse comercialmente.
  https://docs.dlocal.com/docs/onboarding-process-platforms
  https://docs.dlocal.com/docs/onboard-user-accounts-platforms
- Cobre: publica recaudo por PSE, Nequi, Bancolombia y Bre-B, y pagos bancarios/billeteras por API. Tarifa para Turciti no cotizada; no se confirmó que fuera más barata ni se atribuyeron tarjetas a esa oferta.
  https://www.cobre.com/es-co
- Payválida: publica 2,7% + $790 + IVA de recaudo y ofrece API de desembolsos bancarios. No se obtuvo tarifa de Payouts ni resulta más barata que las tarifas generales anteriores solo por el cobro.
  https://payvalida.com/colombia/tarifas-colombia/
  https://payvalida.com/colombia/nuestras-soluciones-colombia/
- Mercado Pago Marketplace, Bold y Openpay se exploraron como alternativas, sin verificar una oferta completa más barata que resolviera simultáneamente los requisitos de Turciti. No se eligieron ni se asume disponibilidad de un split o abono bancario específico.

## 4. Datos, verificación y automatización

El beneficiario no necesita necesariamente abrir una cuenta en la pasarela para recibir un Payout a su banco. Sí hace falta dar de alta correctamente su información y comprobar titularidad. La plataforma operadora se vincula, activa el servicio y responde por los controles que le exige el proveedor.

La API de Wompi documenta identificación, nombre, correo, banco, tipo y número de cuenta, importe, referencia y cuenta de origen. Estos campos no se deben atribuir sin comprobarlos al esquema exacto de ePayco. Nequi puede tener una ruta específica: verificar disponibilidad y costos; no asumir que todas las cuentas de ahorro incluyen automáticamente billeteras.

Un pago debe confirmarse desde el servidor, validando referencia, importe, firma o mecanismo indicado e idempotencia. La redirección del comprador al navegador no basta como confirmación definitiva. No marcar pedidos pagados por una captura de pantalla o por una transferencia pendiente.

Fuentes:
- https://docs.wompi.co/docs/colombia/introduccion-pagos-a-terceros/
- https://docs.wompi.co/docs/colombia/que-es-pagos-a-terceros/
- https://docs.epayco.com/docs/checkout-respuesta-y-confirmacion
- https://epayco.com/terminos-y-condiciones-generales-2026/

## 5. Ejemplos con recargo hipotético del 10%

El negocio recibe B y el comprador paga 1,10 x B. Turciti asume el recaudo y una transferencia por compra. Se incluyen IVA de las tarifas y Payout bancario ePayco de $2.856. No se incluyen todavía retenciones, GMF adicional, IVA propio del servicio, ICA, renta ni operación. La diferencia NO es ganancia neta.

| Precio y abono al negocio | Comprador paga | Recaudo + Payout ePayco | Diferencia ePayco | Recaudo + Payout Wompi | Diferencia Wompi |
| ---: | ---: | ---: | ---: | ---: | ---: |
| $5.000 | $5.500 | $3.849,89 | -$3.349,89 | $3.230,55 | -$2.730,55 |
| $10.000 | $11.000 | $4.022,68 | -$3.022,68 | $3.427,80 | -$2.427,80 |
| $20.000 | $22.000 | $4.368,25 | -$2.368,25 | $3.822,28 | -$1.822,28 |
| $30.000 | $33.000 | $4.713,83 | -$1.713,83 | $4.216,77 | -$1.216,77 |
| $50.000 | $55.000 | $5.404,98 | -$404,98 | $5.005,74 | -$5,74 |
| $100.000 | $110.000 | $7.132,86 | $2.867,14 | $6.978,16 | $3.021,84 |

Fórmulas empleadas:
- Recaudo ePayco = (0,0264 x importe cobrado + 690) x 1,19.
- Payout ePayco = 2.400 x 1,19.
- Recaudo Wompi = (0,0265 x importe cobrado + 700) x 1,19.
- Payout Wompi = (1.849 + 0,004 x importe transferido) x 1,19.
- Redondeo de cada tarifa a centavos para los ejemplos; la liquidación real puede aplicar su regla de redondeo.

Agrupación: diez ventas de base $20.000 al MISMO negocio, cobrando $22.000 por venta y transfiriendo una sola vez $200.000:
- ePayco: recaudo $15.122,50 + Payout $2.856 = $17.978,50; diferencia restante $2.021,50 antes de los demás conceptos.
- Wompi: recaudo $15.267,70 + Payout $3.152,31 = $18.420,01; diferencia $1.579,99 antes de los demás conceptos.
- Se ahorra la parte fija por transferencia, no la comisión de cada cobro ni necesariamente el componente porcentual del Payout.
- Un lote con diez beneficiarios o diez transferencias al mismo negocio sigue pudiendo generar diez cargos. No equivale a consolidar las ventas en un único abono.

## 6. Impuestos y retenciones: alcance real

No existe una cifra universal con "todos los impuestos" sin conocer RUT, régimen ordinario/SIMPLE, CIIU, territorio, tipo de producto y forma de facturación. No se obtuvo ese perfil tributario completo.

| Partida | Tratamiento |
| --- | --- |
| IVA de la pasarela | Se incluyó 19% sobre comisiones gravadas. Puede ser descontable si procede y está soportado; no restarlo como costo definitivo y, además, ignorar su descuento sin explicarlo. |
| IVA del servicio de Turciti | Si es un servicio gravado a cargo de un responsable, se modela 19%. Si el recargo ya lo incluye, IVA = recargo x 19/119. No sumar otro 19% completo sobre ese mismo precio final. El tratamiento depende también de si actúa como intermediario a nombre propio o por cuenta y nombre de otro. |
| IVA o consumo del producto | Depende de lo vendido y del comercio; puede estar ya incluido en el precio. No aplicar IVA del 19% a todos los restaurantes, cafés, experiencias o productos por defecto. |
| Retención de renta | El simulador ePayco muestra 1,5% sobre base sin impuestos. Su aplicación exige revisar régimen, operación y reglas vigentes. Es anticipo, no comisión de la pasarela ni pérdida fiscal automática. |
| ReteIVA | El simulador ePayco modela 15% del IVA de la venta. Es un anticipo que afecta caja y se trata en la declaración correspondiente; no sumarlo de nuevo como impuesto definitivo. |
| ICA y reteICA | Actividad, territorialidad y régimen determinan tasa y base. En Santa Marta el Acuerdo 018 de 2025 fija desde 2026, por ejemplo, 7 por mil para servicios informáticos y de información; no se verificó que esa sea la clasificación aplicable a Turciti. La retención se acredita contra el impuesto que corresponda. |
| Avisos, tableros y sobretasa bomberil | Revisar hechos generadores y régimen municipal. No se cuantificaron sin clasificación y obligaciones confirmadas. |
| Renta ordinaria / SIMPLE | La tarifa general de sociedades en ordinario es 35% de la renta gravable anual, no 35% de cada venta ni de todo el recaudo de terceros. SIMPLE es otra liquidación; no apilar ambos sistemas. |
| Autorretenciones | Revisar calidad del contribuyente y CIIU con normas vigentes. No se fijó una tasa para la empresa operadora. |
| GMF | 4 por mil cuando se causa. Hay reglas y tratamientos específicos para pagos electrónicos y dispersión hasta comercios; no sumar automáticamente un segundo GMF. La página pública de Wompi no identifica por sí sola su componente del 0,4% como ese impuesto. Verificar liquidación y contrato antes de añadirlo o darlo por incluido. |
| FONTUR | Evaluar si Turciti es aportante por su actividad turística. La tarifa general publicada es 2,5 por mil; las plataformas de servicios turísticos tienen reglas de base que deben revisarse. No se presupuestó como obligatorio para todo negocio. |

Retenciones del ingreso del comercio y del ingreso propio deben atribuirse y certificarse correctamente. Si Turciti transfiere el precio completo sin descontar al negocio las retenciones que le correspondan, puede tener que financiar ese desfase. Un mandato y una factura de intermediación no se sustituyen por llamar a todos los recaudos "comisión".

Fuentes:
- https://epayco.com/calculadora-comisiones/
- https://normograma.dian.gov.co/dian/compilacion/docs/oficio_dian_0082_2023.htm
- https://normograma.dian.gov.co/dian/compilacion/docs/oficio_dian_27842_2019.htm
- https://normograma.dian.gov.co/dian/compilacion/docs/oficio_dian_28570_2019.htm
- https://normograma.dian.gov.co/dian/compilacion/docs/estatuto_tributario.htm
- https://normograma.dian.gov.co/dian/compilacion/docs/ley_2277_2022.htm
- https://www.santamarta.gov.co/documentos/acuerdo-no-018-del-30-de-diciembre-de-2025
- https://www.fontur.com.co/es/preguntas-frecuentes

## 7. Escenario de caja conversado, no liquidación tributaria

Supuestos especialmente restrictivos: régimen ordinario, responsables de IVA, producto y servicio al 19% con impuesto incluido, recargo del 10%, retenciones sobre todo el cobro y abono íntegro al negocio. Se añade, SOLO COMO ESCENARIO NO CONFIRMADO para la cuenta, GMF de 0,4% sobre el valor transferido.

Retenciones modeladas = 1,5% x (cobro / 1,19) + 15% x (cobro x 19/119).
Caja modelada = cobro - abono al negocio - recaudo - Payout - retenciones - GMF supuesto.

| Negocio recibe | Comprador paga | IVA del recargo, incluido y no restado de nuevo | Retenciones modeladas | GMF supuesto adicional | Caja ePayco | Caja Wompi |
| ---: | ---: | ---: | ---: | ---: | ---: | ---: |
| $5.000 | $5.500 | $79,83 | $201,05 | $20 | -$3.570,94 | -$2.951,60 |
| $10.000 | $11.000 | $159,66 | $402,11 | $40 | -$3.464,79 | -$2.869,91 |
| $20.000 | $22.000 | $319,33 | $804,20 | $80 | -$3.252,45 | -$2.706,48 |
| $30.000 | $33.000 | $478,99 | $1.206,31 | $120 | -$3.040,14 | -$2.543,08 |
| $50.000 | $55.000 | $798,32 | $2.010,51 | $200 | -$2.615,49 | -$2.216,25 |
| $100.000 | $110.000 | $1.596,64 | $4.021,00 | $400 | -$1.553,86 | -$1.399,16 |

Estos saldos NO son pérdidas contables finales: incluyen anticipos y un GMF supuesto. Tampoco son resultados después de todos los impuestos: faltan la liquidación de IVA con descontables, ICA, renta o SIMPLE, posibles autorretenciones y operación. Si no hay GMF adicional, sumar esa columna a la caja. Si la base, producto o régimen difieren, recalcular; no copiar los resultados como política fiscal.

## 8. Hipótesis comercial del 20% y volumen

Se propuso probar un recargo TOTAL del 20%, IVA incluido cuando corresponda, en compras desde $20.000, agrupando al menos $100.000 de abonos por comercio. No se adoptó como precio definitivo ni se prometió rentabilidad.

Ejemplo ilustrativo con ePayco:
- Cuatro ventas de base $30.000; cada comprador paga $36.000.
- Total cobrado $144.000; un abono de $120.000 al negocio.
- Recaudo de las cuatro compras $7.808,32; Payout $2.856.
- Diferencia antes de retenciones y GMF $13.335,68.
- Aplicando el escenario restrictivo anterior: retenciones $5.263,84 y GMF supuesto $480.
- Caja modelada $7.591,84 por el conjunto, aproximadamente $1.897,96 por compra. En Wompi, aproximadamente $1.902,90.
- No llamar a esos $1.900 ganancia después de todos los impuestos. RUT, atribución fiscal, devoluciones, adquisición de usuarios y operación siguen pendientes.

Precio de planificación: $30.000 al negocio y $36.000 al comprador. No es un promedio medido de Turciti. Se revisaron ejemplos públicos de Santa Marta: cafés calientes $6.000–$15.000, comidas rápidas $25.000–$35.000 y recorridos de referencia $45.000–$99.000; no son una muestra representativa ni un pronóstico.
- https://tatianacaballerocompany.com/menu
- https://hotelsanhaplus.com/restaurante-bunkuany/
- https://lajunglatoursas.com/city-tour-santa-marta/
- https://www.panoramicbussmr.com/reserva

Volumen puramente hipotético: 100 usuarios activos diarios x 10% que compra / 10 negocios = una compra diaria por negocio, si cada comprador hace una compra y se distribuyen de ese modo. No son métricas existentes. Cuatro ventas por local podrían tardar cuatro días en ese supuesto; agrupar por fecha sin volumen suficiente no ahorra la transferencia fija.

El precio final de Turciti debe seguir siendo atractivo frente a comprar directamente en el local. No justificar un descuento mostrando el precio sin el recargo.

## 9. Validación exploratoria aportada por el usuario

El 5 de noviembre de 2026 el usuario reportó haber consultado a DOS personas. Ambas indicaron que no irían a un lugar a comprar por coleccionar medallas, salvo que reciban un descuento o acumulen uno.

Registrar esto como entrevistas exploratorias:
- No hubo una compra, prueba A/B ni repetición observada.
- No se recabaron ni publicaron nombres, edades u otros datos identificativos.
- Orienta la prueba hacia descuento inmediato o acumulable con precio final y beneficio claros.
- Medallas, personaje y álbum quedan como complementos opcionales para la prueba comercial.
- Todavía no demuestra qué descuento, qué producto, qué comisión ni qué plazo de pago aceptan usuarios y negocios.
- Probar con ofertas y fondos reales solo tras definir contratos, facturación, devolución y la admisión del flujo por el proveedor.

La capacidad reutilizable para otras ideas es la red de comercios con catálogo, compras, beneficios, códigos, conciliación y pagos; su estado es diseño/investigación, no implementación comprobada.
