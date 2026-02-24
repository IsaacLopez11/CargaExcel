using System.Numerics;

namespace cargaExcel.Models
{
    public class Persona{
        public string TipoDoc {get;set;}
        public BigInteger NumDoc {get;set;}
        public string Nombre {get;set;}
        public DateTime FechaNac {get;set;}
        public decimal Sueldo {get;set;}
        public string CiudadRes {get;set;}
    }
}