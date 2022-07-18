using Microsoft.Toolkit.Mvvm.Messaging.Messages;

namespace CustomSpectrumAnalyzer
{
    public class MarkerMessage : ValueChangedMessage<bool>
    {
        public MarkerMessage(bool value) : base(value)
        {
        }
    }
}
