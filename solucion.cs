// Solucion: Abigail Geronimo
// Situacion resuelta: Situacion 2 -
// Patron aplicado: Strategy

using System;

namespace BibliotecaMunicipal
{
   
    public interface ICalculadoraMulta
    {
        decimal Calcular(int diasAtraso);
        bool BloqueaNuevosPrestamos(int diasAtraso);
    }

    public class MultaSocioInfantil : ICalculadoraMulta
    {
        public decimal Calcular(int diasAtraso) => 0m;

        public bool BloqueaNuevosPrestamos(int diasAtraso) => diasAtraso > 0;
    }

    public class MultaSocioAdulto : ICalculadoraMulta
    {
        private const decimal TarifaPorDia = 2m;

        public decimal Calcular(int diasAtraso) => diasAtraso * TarifaPorDia;

        public bool BloqueaNuevosPrestamos(int diasAtraso) => diasAtraso > 0;
    }

    public class MultaSocioTerceraEdad : ICalculadoraMulta
    {
        private const decimal TarifaPorDia = 1m;
        private const decimal Tope = 20m;

        public decimal Calcular(int diasAtraso)
        {
            decimal monto = diasAtraso * TarifaPorDia;
            return monto > Tope ? Tope : monto;
        }

        public bool BloqueaNuevosPrestamos(int diasAtraso) => diasAtraso > 0;
    }

    
    public class CalculadorDeMulta
    {
        private readonly ICalculadoraMulta _estrategia;

        public CalculadorDeMulta(ICalculadoraMulta estrategia)
        {
            _estrategia = estrategia;
        }

        public decimal CalcularMulta(Prestamo prestamo)
        {
            return _estrategia.Calcular(prestamo.DiasAtraso);
        }

        public bool DebeBloquear(Prestamo prestamo)
        {
            return _estrategia.BloqueaNuevosPrestamos(prestamo.DiasAtraso);
        }
    }

    public enum TipoSocio { Infantil, Adulto, TerceraEdad }

    public class Prestamo
    {
        public string CodigoLibro { get; set; }
        public string NombreSocio { get; set; }
        public int DiasAtraso { get; set; }
        public TipoSocio TipoSocio { get; set; }
    }

 