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

public class ProfitReportVM : INotifyPropertyChanged
{
    private ReportModel rm;
    private PlotModel _profitPlotModel;

    public ProfitReportVM()
    {
        rm = new ReportModel();
        GenerateReportCommand = new RelayCommand(GenerateReport);
        ProfitPlotModel = new PlotModel { Title = "Отчет по прибыли" };
    }

    //private DateTime _selectedYear = new DateTime(2024, 1, 1);

    //public DateTime SelectedYear
    //{
    //    get => _selectedYear; 
    //    set
    //    {
    //        if (value.Year < 2000) 
    //        {
    //            _selectedYear = new DateTime(2000, 1, 1);  
    //        }
    //        else if (value.Year > 2099) 
    //        {
    //            _selectedYear = new DateTime(2099, 1, 1);
    //        }
    //        else
    //        {
    //            _selectedYear = new DateTime(value.Year, 1, 1); 
    //        }
    //        OnPropertyChanged();
    //    }
    //}
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


    public PlotModel ProfitPlotModel
    {
        get => _profitPlotModel;
        set
        {
            _profitPlotModel = value;
            OnPropertyChanged();
        }
    }

    public ICommand GenerateReportCommand { get; }

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

            // Оси и категории
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
  
    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
