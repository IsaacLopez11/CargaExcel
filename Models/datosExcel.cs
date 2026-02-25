using System.Numerics;

namespace cargaExcel.Models
{
    public class Persona{
        public required string  TipoDoc {get;set;}
        public BigInteger NumDoc {get;set;}
        public required string Nombre {get;set;}
        public DateTime FechaNac {get;set;}
        public decimal Sueldo {get;set;}
        public required string CiudadRes {get;set;}
    }
}