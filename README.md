# parcial2
P2.1  Decisiones de diseño
Situación 1  Notificaciones al vencer un préstamo
Patrón: Observer (Publicador/Suscriptor)

El módulo de préstamos hoy conoce y llama uno por uno a cada interesado (correo, morosidad, recepción), y la dirección ya anunció que la lista seguirá creciendo ("multas municipales" y "quién sabe qué más"). Si se mantiene así, cada interesado nuevo obliga a abrir y modificar el módulo de préstamos, violando el principio de abierto/cerrado y arriesgando romper la lógica de negocio del préstamo por un cambio que no le pertenece. Con Observer, el préstamo solo emite un evento "venció sin devolución" a una lista de observadores registrados; agregar el sistema de multas es suscribir un observador nuevo, sin tocar el módulo de préstamos. Strategy no resuelve esto porque aquí no hay una única acción intercambiable, sino varias acciones independientes que deben dispararse todas ante el mismo evento.

Situación 2 — Cálculo de multa por tipo de socio
Patrón: Strategy

El cálculo de multa varía según el tipo de socio (infantil, adulto, tercera edad) y las reglas cambian cada año por decisión del concejo. Hoy esa lógica vive en un if/else duplicado en préstamos y en reportes: cada cambio de regla obliga a tocar dos lugares y arriesga que ambos módulos queden inconsistentes entre sí. Con Strategy, cada regla de cálculo es una clase intercambiable detrás de una interfaz común; agregar o modificar una regla no toca ni préstamos ni reportes, y ambos módulos reutilizan la misma estrategia, eliminando la duplicación. Observer no aplica porque no hay un evento que deba avisarse a varios interesados, sino un algoritmo que cambia según el tipo de dato.

Situación 3 — Integración con el Sistema Estatal de Bibliotecas
Patrón: Adapter

El servicio externo (PushRecord(jsonPayload, isoDate, originCode)) no es modificable, usa nombres en inglés, otro formato de fecha y códigos que el dominio de la biblioteca no maneja. Si el catálogo llama directo a ese servicio, toda la lógica de negocio queda acoplada a un vocabulario ajeno, y cada nueva versión del servicio se propaga por todo el sistema, siendo costoso de mantener. Con Adapter se crea una única clase que traduce el contrato del dominio (p. ej. RegistrarActualizacionDeCatalogo) hacia el contrato externo (PushRecord con su formato y códigos), aislando el cambio a un solo punto. Strategy no aplica porque no hay variantes intercambiables de un mismo algoritmo interno, sino una incompatibilidad de interfaces entre dos sistemas distintos.

P2.3 — La conexión SOLID
La implementación de P2.2 (Strategy para el cálculo de multas) rescata el Principio de Abierto/Cerrado (OCP): el módulo CalculadorDeMulta está abierto a extensión (se puede agregar una nueva regla, por ejemplo MultaConvenioInstitucional, creando una nueva clase que implemente ICalculadoraMulta) pero cerrado a modificación, porque CalculadorDeMulta nunca cambia su código, solo recibe una implementación distinta de ICalculadoraMulta inyectada por constructor. Esto se ve concretamente en el método CalcularMulta(Prestamo prestamo), que delega en _estrategia.Calcular(...) sin ningún if/else sobre el tipo de socio.
