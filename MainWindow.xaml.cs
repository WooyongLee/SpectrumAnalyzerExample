using System.Windows;

namespace CustomSpectrumAnalyzer
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = App.Current.Services.GetService(typeof(MainViewModel));
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //this.GeneratePoints();

            //RefreshButton_Click(null, null);
        }
    }

    //public class ControlAdonrner : Adorner
    //{
    //    public ControlAdonrner(UIElement adornedElement) : base(adornedElement)
    //    {
    //        // 사용법
    //        // var myAdornerLayer = AdornerLayer.GetAdornerLayer(myTextBox);
    //        // myAdornerLayer.Add(new ControlAdonrner(myTextBox));
    //    }

    //    // A common way to implement an adorner's rendering behavior is to override the OnRender
    //    // method, which is called by the layout system as part of a rendering pass.
    //    protected override void OnRender(DrawingContext drawingContext)
    //    {
    //        Rect adornedElementRect = new Rect(this.AdornedElement.DesiredSize);

    //        // Some arbitrary drawing implements.
    //        SolidColorBrush renderBrush = new SolidColorBrush(Colors.Green);
    //        renderBrush.Opacity = 0.2;
    //        Pen renderPen = new Pen(new SolidColorBrush(Colors.Navy), 1.5);
    //        double renderRadius = 5.0;

    //        // Draw a circle at each corner.
    //        drawingContext.DrawEllipse(renderBrush, renderPen, adornedElementRect.TopLeft, renderRadius, renderRadius);
    //        drawingContext.DrawEllipse(renderBrush, renderPen, adornedElementRect.TopRight, renderRadius, renderRadius);
    //        drawingContext.DrawEllipse(renderBrush, renderPen, adornedElementRect.BottomLeft, renderRadius, renderRadius);
    //        drawingContext.DrawEllipse(renderBrush, renderPen, adornedElementRect.BottomRight, renderRadius, renderRadius);
    //    }
    //}
}
