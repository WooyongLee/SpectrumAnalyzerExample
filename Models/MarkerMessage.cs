using Microsoft.Toolkit.Mvvm.Messaging.Messages;
using System.ComponentModel;

namespace CustomSpectrumAnalyzer
{
    public class MarkerMessage : ValueChangedMessage<bool>, INotifyPropertyChanged
    {
        public EMarkerCommandType MarkerCommandType { get; set; }

        public int MarkerNum { get; set;  }

        private double frequency;
        public double Frequency { get { return frequency; } set { frequency = value; OnPropertyChanged("Frequency"); } }

        private double amplitude;
        public double Amplitude { get { return amplitude; } set { amplitude = value; OnPropertyChanged("Amplitude"); } }

        public MarkerMessage(bool value) : base(value)
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
