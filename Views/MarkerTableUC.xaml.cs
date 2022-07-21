using System.Collections.Generic;
using System.Windows.Controls;

namespace CustomSpectrumAnalyzer
{
    /// <summary>
    /// MarkerTableUC.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MarkerTableUC : UserControl
    {
        public MarkerTableUC()
        {
            InitializeComponent();
            this.DataContext = App.Current.Services.GetService(typeof(MarkerViewModel));

            listBox.ItemsSource = ((MarkerViewModel)DataContext).MarkerParamList;
        }
    }
}
