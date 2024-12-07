using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Axes;
using WpfNed.Model;
using System.Linq;
using WpfNed;
using OxyPlot.Annotations;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System.IO;

public class ProfitReportVM : INotifyPropertyChanged
{
    private ReportModel rm;
    private PlotModel _profitPlotModel;
    private PlotModel _typeProfitPlotModel;

    public ProfitReportVM()
    {
        rm = new ReportModel();
        GenerateReportCommand = new RelayCommand(GenerateReport);
        GenerateTypeReportCommand = new RelayCommand(GenerateTypeProfitReport);
        ProfitPlotModel = new PlotModel { Title = "Отчет по прибыли" };
        TypeProfitPlotModel = new PlotModel { Title = "Соотношение прибыли по типам объектов" };
        SelectedYear = 2024;
        SelectedYearType = 2024;
        ExportProfitReportToPdfCommand = new RelayCommand(() => ExportPlotToPdf(ProfitPlotModel, "ProfitReport.pdf"));
        ExportTypeProfitReportToPdfCommand = new RelayCommand(() => ExportPlotToPdf(TypeProfitPlotModel, "TypeProfitReport.pdf"));
    }
    private int _selectedYear;

    public int SelectedYear
    {
        get => _selectedYear;
        set
        {
            _selectedYear = value;
            OnPropertyChanged(nameof(SelectedYear));
        }
    }

    private int _selectedYearType;

    public int SelectedYearType
    {
        get => _selectedYearType;
        set
        {
            _selectedYearType = value;
            OnPropertyChanged(nameof(SelectedYearType));
        }
    }
    public PlotModel ProfitPlotModel
    {
        get => _profitPlotModel;
        set
        {
            _profitPlotModel = value;
            OnPropertyChanged();
        }
    }
    public PlotModel TypeProfitPlotModel
    {
        get => _typeProfitPlotModel;
        set
        {
            _typeProfitPlotModel = value;
            OnPropertyChanged();
        }
    }

    public ICommand GenerateReportCommand { get; }
    public ICommand GenerateTypeReportCommand { get; }
    public ICommand ExportProfitReportToPdfCommand { get; }
    public ICommand ExportTypeProfitReportToPdfCommand { get; }

    private void GenerateReport()
    {
        if (SelectedYear < 2024 || SelectedYear > DateTime.Now.Year)
        {
            MessageBox.Show("Нет данных за введенный промежуток", "Ошибка!");
        }
        else
        {
            var year = SelectedYear;
            var monthlyProfit = rm.GetMonthlyProfit(year);
            var plotModel = new PlotModel { Title = $"Отчет по прибыли по контрактам, заключенным в {year} году" };

            var series = new BarSeries
            {
                Title = "Прибыль",
                StrokeColor = OxyColors.Black,
                StrokeThickness = 1
            };

            double maxProfit = 0; 
            foreach (var month in Enumerable.Range(1, 12))
            {
                double profit = (double)monthlyProfit[month];
                series.Items.Add(new BarItem(profit));
                if (profit > maxProfit)
                {
                    maxProfit = profit;
                }
                var textAnnotation = new TextAnnotation
                {
                    Text = profit.ToString("C"),
                    TextPosition = new DataPoint(profit, month - 1), 
                    Stroke = OxyColors.Transparent, 
                    TextColor = OxyColors.Black, 
                    FontSize = 10, 
                    TextVerticalAlignment = OxyPlot.VerticalAlignment.Middle,  
                    TextHorizontalAlignment = OxyPlot.HorizontalAlignment.Left 
                };
                plotModel.Annotations.Add(textAnnotation);
            }

            plotModel.Series.Add(series);
            plotModel.Axes.Add(new CategoryAxis
            {
                Position = AxisPosition.Left, 
                Key = "Months",
                ItemsSource = new[] { "Янв", "Фев", "Март", "Апр", "Май", "Июнь", "Июль", "Авг", "Сен", "Окт", "Ноя", "Дек" },
                Angle = 0,  
                FontSize = 10, 
            });

            plotModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Bottom, 
                Minimum = 0,  
                Maximum = maxProfit + 5000000, 
                Title = "Прибыль",
                FontSize = 10,
                LabelFormatter = (value) => value.ToString("0"),  
                MajorStep = 2000000, 
            });

            ProfitPlotModel = plotModel;
        }
    }
    private void GenerateTypeProfitReport()
    {
        if (SelectedYearType < 2024 || SelectedYearType > DateTime.Now.Year)
        {
            MessageBox.Show("Нет данных за введенный промежуток", "Ошибка!");
            return;
        }

        var year = SelectedYearType;
        var typeProfit = rm.GetProfitByType(year);
        var plotModel = new PlotModel { Title = $"Соотношение прибыли по типам объектов в {year} году" };

        var pieSeries = new PieSeries
        {
            StrokeThickness = 2,
            InsideLabelPosition = 0.5,
            AngleSpan = 360,
            StartAngle = 0
        };

        foreach (var type in typeProfit)
        {
            pieSeries.Slices.Add(new PieSlice(type.Key, type.Value)
            {
                IsExploded = false, 
                Fill = type.Key == "Квартира" ? OxyColors.SkyBlue : OxyColors.LightGreen
            });
        }

        plotModel.Series.Add(pieSeries);
        TypeProfitPlotModel = plotModel;
    }
    private void ExportPlotToPdf(PlotModel plotModel, string pdfFileName)
    {
        if (plotModel == null || plotModel.Series.Count == 0)
        {
            MessageBox.Show("Диаграмма отсутствует или не построена.", "Ошибка");
            return;
        }
        var imagePath = SavePlotAsImage(plotModel, "tempPlot.png");
        if (imagePath != null)
        {
            AddImageToPdf(imagePath, pdfFileName);
            File.Delete(imagePath);
        }
    }

    private void AddImageToPdf(string imagePath, string pdfFileName)
    {
        try
        {
            var document = new PdfDocument();
            var page = document.AddPage();
            const double margin = 20; 
            var imageWidth = page.Width - 2 * margin;
            var imageHeight = page.Height;

            using (var image = XImage.FromFile(imagePath))
            {
                double aspectRatio = image.PixelWidth / (double)image.PixelHeight;
                if (imageWidth / imageHeight > aspectRatio)
                {
                    imageWidth = imageHeight * aspectRatio;
                }
                else
                {
                    imageHeight = imageWidth / aspectRatio;
                }
                double x = (page.Width - imageWidth) / 2;
                double y = (page.Height - imageHeight) / 2;
                var gfx = XGraphics.FromPdfPage(page);
                gfx.DrawImage(image, x, y, imageWidth, imageHeight);
            }

            var pdfPath = Path.Combine(Environment.CurrentDirectory, pdfFileName);
            document.Save(pdfPath);

            MessageBox.Show($"Диаграмма успешно экспортирована в {pdfPath}.", "Успех");
            OpenPdf(pdfPath);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка при создании PDF: {ex.Message}", "Ошибка");
        }
    }

    private void OpenPdf(string pdfPath)
    {
        try
        {
            var process = new System.Diagnostics.Process
            {
                StartInfo = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = pdfPath,
                    UseShellExecute = true
                }
            };
            process.Start();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка при открытии PDF: {ex.Message}", "Ошибка");
        }
    }

    private string SavePlotAsImage(PlotModel plotModel, string fileName)
    {
        try
        {
            const int width = 1200;
            const int height = 600;

            var pngExporter = new OxyPlot.SkiaSharp.PngExporter
            {
                Width = width,
                Height = height,
            };
            var imagePath = Path.Combine(Environment.CurrentDirectory, fileName);
            using (var stream = File.OpenWrite(imagePath))
            {
                pngExporter.Export(plotModel, stream);
            }

            return imagePath;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка сохранения изображения: {ex.Message}", "Ошибка");
            return null;
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
