using Microsoft.Toolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomSpectrumAnalyzer
{
    public class SpectrumViewModel : ViewModelBase
    {
        private SettingParameter settingParam;
        public SettingParameter SettingParam
        {
            get { return settingParam; }
            set { SetProperty(ref settingParam, value); }
        }

        public SpectrumViewModel()
        {
            WeakReferenceMessenger.Default.Register<SettingMessage>(this, OnSettingMessage);
        }

        private void OnSettingMessage(object recipient, SettingMessage message)
        {
            SettingParam = message.SettingParam;
            // To Do :: Setting Message에 따라 분기하여 Canvas에 도시할 것
        }
    }
}
