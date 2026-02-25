using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Data;
using System.IO;
using OfficeOpenXml; // si usas EPPlus

namespace FrontCargaExcel;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

 private async void OnUploadClick(object? sender, RoutedEventArgs e)
{
    var dialog = new OpenFileDialog
    {
        Title = "Selecciona un archivo Excel",
        Filters =
        {
            new FileDialogFilter
            {
                Name = "Excel Files",
                Extensions = { "xlsx" }
            }
        }
    };

    var result = await dialog.ShowAsync(this);

    if (result != null && result.Length > 0)
    {
        var path = result[0];

        var table = LeerExcel(path);

        var grid = this.FindControl<DataGrid>("ExcelGrid");
        grid.ItemsSource = table.DefaultView;
    }
}

    private DataTable LeerExcel(string path)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        using var package = new ExcelPackage(new FileInfo(path));
        var worksheet = package.Workbook.Worksheets[0];

        var table = new DataTable();

        for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
            table.Columns.Add(worksheet.Cells[1, col].Text);

        for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
        {
            var newRow = table.NewRow();
            for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
                newRow[col - 1] = worksheet.Cells[row, col].Text;

            table.Rows.Add(newRow);
        }

        return table;
    }
}