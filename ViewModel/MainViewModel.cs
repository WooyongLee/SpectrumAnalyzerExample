using Microsoft.Toolkit.Mvvm.Messaging;

namespace CustomSpectrumAnalyzer
{
    public class MainViewModel : ViewModelBase
    {
        private string controlName;
        public string ControlName
        {
            get { return controlName; }
            set { SetProperty(ref controlName, value); }
        }

        public MainViewModel()
        {
        }
    }
}
