using System.Windows.Controls;

namespace CustomSpectrumAnalyzer
{
    /// <summary>
    /// SettingPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SettingPage : UserControl
    {
        public SettingPage()
        {
            InitializeComponent();
            this.DataContext = App.Current.Services.GetService(typeof(SettingViewModel));
        }
    }
}
