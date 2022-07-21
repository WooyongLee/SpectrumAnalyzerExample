using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CustomSpectrumAnalyzer
{
    public class SettingViewModel : ViewModelBase
    {
        public ICommand ResetMarkerCommand { get; set; }
        public ICommand SettingApplyCommand { get; set; }

        private double centerFreq;
        private double span;
        private double viewerRefLv;

        public double CenterFreq
        {
            get { return centerFreq; }
            set { centerFreq = value; NotifyPropertyChanged("CenterFreq"); }
        }

        public double Span
        {
            get { return span; }
            set { span = value; NotifyPropertyChanged("Span"); }
        }

        public double ViewerRefLv
        {
            get { return viewerRefLv; }
            set { viewerRefLv = value; NotifyPropertyChanged("ViewerRefLv"); }
        }

        public SettingViewModel()
        {
            CenterFreq = 3650.01;
            span = 150;
            viewerRefLv = 0;

            SettingApplyCommand = new RelayCommand(OnSettingApplied);
            ResetMarkerCommand = new RelayCommand(OnResetMarker);
        }

        private void OnSettingApplied()
        {
            // Send Message of Setting Applied
            WeakReferenceMessenger.Default.Send(new SettingMessage(true)
            {
                ControlName = "SettingApplied " + GetTimeStamp(),
                SettingParam = new SettingParameter(ESettingCommandType.Applied) 
                { 
                    CenterFreq = this.CenterFreq, 
                    Span = this.Span, 
                    ViewerRefLv = this.ViewerRefLv 
                },
            });
        }

        private void OnResetMarker()
        {
            WeakReferenceMessenger.Default.Send(new SettingMessage(true) 
            {
                ControlName = "ResetMarker " + GetTimeStamp(),
                SettingParam = new SettingParameter(ESettingCommandType.ResetMarker)
            });

            WeakReferenceMessenger.Default.Send(new MarkerMessage(true)
            {
                MarkerCommandType = EMarkerCommandType.Clear,
            });
        }

        private string GetTimeStamp()
        {
            return DateTime.Now.ToString("hh.mm.ss.ffffff");
        }
    }
}
