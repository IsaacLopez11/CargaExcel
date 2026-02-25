// See https://aka.ms/new-console-template for more information
using cargaExcel.Controllers;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hola desde main");
        var excelFile = new CargaExcelController();
        excelFile.ProcesarExcel("/home/isaac/Documentos/prueba.xlsx");

        


    }
}