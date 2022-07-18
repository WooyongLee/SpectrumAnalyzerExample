using Microsoft.Toolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            Init();
        }

        private void Init()
        {
            // Register Setting Message Receiver
            WeakReferenceMessenger.Default.Register<MarkerMessage>(this, OnMarkerInfoChanged);
        }

        private void OnMarkerInfoChanged(object recipient, MarkerMessage message)
        {

        }

    }
}
