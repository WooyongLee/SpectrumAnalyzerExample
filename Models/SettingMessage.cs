using Microsoft.Toolkit.Mvvm.Messaging.Messages;

namespace CustomSpectrumAnalyzer
{
    public class SettingMessage : ValueChangedMessage<bool>
    {
        public string ControlName { get; set; }

        public SettingParameter SettingParam { get; set; }

        public SettingMessage(bool value) : base(value)
        {

        }
    }
}
