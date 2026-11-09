using SmartSupportRag.Api.DTOs.Policies;
using SmartSupportRag.Api.Entities;

namespace SmartSupportRag.Api.Data;

public static class SeedData
{
    public static IReadOnlyList<IngestPolicyRequest> GetInitialPolicies() =>
    [
        new IngestPolicyRequest
        {
            Title = "Política General de Devoluciones y Reembolsos",
            Category = PolicyCategory.Devoluciones,
            Content = """
            Los clientes disponen de un plazo máximo de 30 días calendario contados a partir de la fecha de entrega confirmada por la transportadora para solicitar la devolución de productos estándar. Para productos adquiridos en campañas de liquidación o etiqueta roja, el plazo máximo de devolución se reduce estrictamente a 7 días calendario.

            Para que una devolución sea aprobada, el artículo debe encontrarse sin uso, conservar todos sus sellos de seguridad intactos, incluir sus accesorios originales, manuales y empaque de fábrica en perfecto estado. No se aceptan devoluciones de software con licencia activada, tarjetas de regalo digitales, audífonos in-ear abiertos ni productos de cuidado personal por razones de higiene.

            Una vez recibido el producto en nuestro centro de distribución, el equipo de control de calidad realizará la inspección técnica en un plazo de 48 horas hábiles. Si el dictamen es favorable, el reembolso se procesará automáticamente al mismo método de pago original: para pagos con tarjeta de crédito o débito el abono demora entre 5 y 10 días hábiles bancarios, mientras que para pagos con billetera digital o saldo en plataforma el reintegro se acredita en menos de 24 horas.

            Si la devolución se debe a un error de despacho o defecto de origen, el costo del flete de retorno corre al 100% por cuenta de la empresa. Si la devolución obedece a un cambio de opinión o error de selección de talla o modelo por parte del comprador, se descontará una tarifa plana de recolección logística de $15.00 USD del monto total a reembolsar.
            """
        },

        new IngestPolicyRequest
        {
            Title = "Manual de Cobertura de Garantías Técnicas",
            Category = PolicyCategory.Garantias,
            Content = """
            Todos los equipos electrónicos nuevos comercializados directamente en la plataforma cuentan con una garantía estándar de fábrica de 12 meses contra defectos de manufactura, componentes internos y fallos de ensamblaje. Los equipos reacondicionados certificados cuentan con una garantía limitada de 90 días calendario bajo las mismas condiciones técnicas.

            La garantía cubre sin costo alguno la reparación con repuestos originales en talleres autorizados o, en caso de que el equipo no sea reparable tras dos ingresos consecutivos por la misma falla, el reemplazo directo por una unidad nueva de iguales características o la emisión de una nota de crédito por el valor íntegro de la factura.

            Quedan expresamente excluidos de la cobertura de garantía los daños físicos provocados por caídas, golpes, pantallas rotas o fisuradas, derrames de líquidos, sulfatación por humedad, sobrecargas eléctricas por uso de cargadores genéricos no certificados, o cualquier intento de apertura y manipulación del sello de garantía por personal ajeno al servicio técnico oficial.

            El desgaste natural de consumibles como baterías (cuya capacidad se mantenga por encima del 80% durante el primer año), cables externos, almohadillas y carcasas cosméticas tampoco está cubierto por la garantía estándar, salvo que el cliente haya adquirido adicionalmente el plan de Protección Extendida contra Daños Accidentales al momento de la compra.
            """
        },

        new IngestPolicyRequest
        {
            Title = "Tiempos, Tarifas y Condiciones de Envío",
            Category = PolicyCategory.Envios,
            Content = """
            Los pedidos confirmados y con pago validado antes de las 14:00 horas (hora local) de lunes a viernes se procesan y despachan el mismo día hábil. Las órdenes registradas después de las 14:00 horas, fines de semana o días festivos nacionales inician su preparación el siguiente día hábil.

            Ofrecemos tres modalidades de entrega: Envío Estándar con un tiempo estimado de 3 a 5 días hábiles en zonas urbanas principales y de 6 a 9 días hábiles en zonas rurales o periféricas; Envío Express 24 Horas disponible exclusivamente en áreas metropolitanas por un cargo fijo de $9.90 USD; y Retiro en Tienda o Locker Inteligente sin costo adicional en un plazo de 24 a 48 horas tras la confirmación.

            El envío estándar es completamente gratuito para todos los carritos de compra cuyo subtotal supere los $75.00 USD después de aplicar cupones de descuento. Para compras inferiores a dicho monto, la tarifa de envío estándar se calcula automáticamente entre $4.50 USD y $8.00 USD según el peso volumétrico y código postal de destino.

            La empresa transportadora realizará hasta un máximo de 2 intentos de entrega en el domicilio registrado. Si tras el segundo intento no se logra localizar al destinatario, el paquete permanecerá 5 días hábiles en la sucursal logística más cercana antes de retornar al almacén central, generando un cargo administrativo de reenvío en caso de solicitar un nuevo despacho.
            """
        },

        new IngestPolicyRequest
        {
            Title = "Términos y Condiciones de Uso Comercial",
            Category = PolicyCategory.Terminos,
            Content = """
            Para realizar compras en la plataforma el usuario debe ser mayor de 18 años y contar con capacidad legal para contratar. Cada cliente es responsable de mantener actualizados sus datos de facturación; una vez emitida una factura electrónica con RUC o identificación fiscal, no se admiten refacturaciones ni cambios de razón social en meses contables cerrados.

            Los precios publicados incluyen los impuestos de ley aplicables y se garantizan únicamente al completar el pago exitoso de la orden; agregar artículos al carrito de compras no congela el precio ni reserva el inventario disponible. Los cupones promocionales son de uso único por cuenta, no son acumulables entre sí ni canjeables por dinero en efectivo.

            En eventos de alta demanda o por discrepancias extraordinarias de inventario, si un producto pagado quedara sin stock físico, notificaremos al cliente en un plazo máximo de 24 horas ofreciendo dos alternativas: esperar la reposición prioritaria con un bono de compensación del 10% sobre el valor del artículo, o recibir el reembolso inmediato del 100% más un cupón de $10.00 USD para su próxima compra.

            Queda prohibida la compra mayorista con fines de reventa no autorizada a través de cuentas de consumidor final. La plataforma se reserva el derecho de limitar la compra a un máximo de 3 unidades de una misma referencia tecnológica por cliente y por mes calendario.
            """
        },

        new IngestPolicyRequest
        {
            Title = "Políticas de Seguridad de Cuentas y Prevención de Fraude",
            Category = PolicyCategory.Seguridad,
            Content = """
            Por seguridad del titular, toda cuenta que registre 5 intentos fallidos consecutivos de inicio de sesión será bloqueada temporalmente durante 30 minutos. Para restablecer el acceso de forma inmediata o cambiar el correo electrónico principal asociado a la cuenta, el usuario deberá completar la validación de segundo factor (2FA) mediante código OTP enviado a su teléfono móvil verificado.

            Las compras que superen los $1,000.00 USD o que presenten patrones inusuales de geolocalización pasan por un filtro preventivo de validación antifraude. En estos casos, el área de seguridad puede retener preventivamente el despacho hasta por 24 horas hábiles para confirmar la titularidad de la transacción mediante una micro-transacción verificable o confirmación directa con el banco emisor.

            Nuestro equipo de soporte jamás solicitará contraseñas, códigos OTP de un solo uso, números completos de tarjetas ni códigos CVV por chat, correo electrónico o llamada telefónica. Si un cliente reporta un cargo no reconocido o el acceso no autorizado a su perfil, el agente de soporte debe ejecutar de inmediato el protocolo de congelamiento de cuenta, revocar las sesiones activas y anular cualquier orden en tránsito que aún no haya sido entregada.

            De acuerdo con la normativa de protección de datos personales, el titular puede solicitar la eliminación definitiva de su cuenta y la anonimización de su historial en cualquier momento, siempre que no existan órdenes en curso, disputas bancarias abiertas ni saldos pendientes de liquidación.
            """
        }
    ];
}
