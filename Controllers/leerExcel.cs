using OfficeOpenXml;
using cargaExcel.Models;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using System.Numerics;

namespace CargaExcel.Controllers{
    
    public class CargaExcelController{
        private List<Persona> personas = new List<Persona>();
        private List<string> errores = new List<string>();

        public List<Persona> ProcesarExcel(string ruta){

            ExcelPackage.License.SetNonCommercialPersonal("Isaac Lopez");
            using (var package = new ExcelPackage(new FileInfo(ruta)))
            {
                ExcelWorksheet hoja = package.Workbook.Worksheets[0];
                int filas = hoja.Dimension.Rows;

                for (int fila = 2; fila <= filas; fila++)
                {
                    string tipoDocumento = hoja.Cells[fila,1].Text.Trim().ToUpper();
                    if (tipoDocumento != "CC" 
                    || tipoDocumento != "CE" 
                    || tipoDocumento != "RC"
                    || tipoDocumento != "TI"
                    || tipoDocumento != "PPT"
                    || tipoDocumento != "PEP"
                    || tipoDocumento != "PT"
                    )
                    {
                        errores.Add($"Fila: {fila}, tipo de documento es inválido");
                        continue;
                    }

                    BigInteger numDoc = BigInteger.Parse(hoja.Cells[fila,2].Text);
                    if (numDoc == 0
                    || BigInteger.IsNegative(numDoc)
                    )
                    {
                        errores.Add($"Fila: {fila}, tiene número de docmento inválido");
                    }

                    string fechaNac = hoja.Cells[fila,4].Text;
                    DateTime fecha;

                    bool esFechaValida = DateTime.TryParse(
                        fechaNac, 
                        new System.Globalization.CultureInfo("es-CO"), 
                        System.Globalization.DateTimeStyles.None, 
                        out fecha
                    );

                    if (esFechaValida)
                    {
                        if (fecha > DateTime.Now
                        || DateTime.Now.Year - fecha.Year > 150
                        )
                        {
                            errores.Add($"Fila {fila}: La fecha de nacimiento es inválida.");
                            continue;
                        }
                    }


                    decimal sueldo = 0;
                    bool esNumeroValido = decimal.TryParse(
                        hoja.Cells[fila, 5].Text,
                        NumberStyles.Any,
                        new CultureInfo("es-CO"),
                        out sueldo
                    );

                    if (!esNumeroValido)
                    {
                        errores.Add($"Fila {fila}: Sueldo inválido."); 
                        continue;
                    }

                    if (sueldo < 0)
                    {
                        errores.Add($"Fila {fila}: Sueldo negativo."); 
                        continue;
                    }


                    string nombre = hoja.Cells[fila,3].Text;
                    if (string.IsNullOrEmpty(nombre))
                    {
                        errores.Add($"Fila {fila}, nombre inválido");
                    }


                    string CiudadRes = hoja.Cells[fila,6].Text.ToUpper();
                    if (string.IsNullOrEmpty(CiudadRes))
                    {
                        errores.Add($"Fila {fila}, Ciudad de residencia inválida o vacía");
                    }

                    personas.Add(new Persona
                    {
                        TipoDoc = tipoDocumento,
                        NumDoc = numDoc,
                        Nombre = nombre,
                        FechaNac = fecha,
                        Sueldo = sueldo                      
                    });
                }
                
            }

            return personas;
        }    



    }

}