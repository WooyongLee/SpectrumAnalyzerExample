using Microsoft.Toolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;
using System.Linq;

namespace CustomSpectrumAnalyzer
{
    public class MarkerViewModel : ViewModelBase
    {
        // Marker List : 
        // Item : M1~12(Max), Freq, Amp
        public ObservableCollection<MarkerParameter> MarkerParamList { get; set; }

        private MarkerParameter markerParam;
        public MarkerParameter MarkerParam
        {
            get { return markerParam; }
            set { SetProperty(ref markerParam, value); }
        }

        public MarkerViewModel()
        {
            MarkerParamList = new ObservableCollection<MarkerParameter>();

            // Register Setting Message Receiver
            WeakReferenceMessenger.Default.Register<MarkerMessage>(this, OnMarkerInfoChanged);
        }

        private void OnMarkerInfoChanged(object recipient, MarkerMessage message)
        {
            if ( message.MarkerCommandType == EMarkerCommandType.Create)
            {
                if (IsDuplicateMarker(message.MarkerNum))
                {
                    return;
                }

                if (MarkerParamList.Count < 6)
                {
                    MarkerParamList.Add(new MarkerParameter()
                    {
                        NumOfMarker = message.MarkerNum,
                        Frequency = message.Frequency,
                        Amp = message.Amplitude,
                    });
                }
            } // end if ( message.MarkerCommandType == EMarkerCommandType.Create)

            else if ( message.MarkerCommandType == EMarkerCommandType.Update)
            {
                int findIndex = MarkerParamList.ToList().FindIndex(x => x.NumOfMarker == message.MarkerNum);

                if (findIndex != -1)
                {
                    MarkerParamList[findIndex].Frequency = message.Frequency;
                    MarkerParamList[findIndex].Frequency = message.Amplitude;
                }
                //var item = MarkerParamList.FirstOrDefault(x => x.NumOfMarker == message.MarkerNum);
                //if (item != null )
                //{
                //    item.Frequency = message.Frequency;
                //    item.Amp = message.Amplitude;
                //}
            } // end else if ( message.MarkerCommandType == EMarkerCommandType.Update)

            else if (message.MarkerCommandType == EMarkerCommandType.Clear)
            {
                MarkerParamList.Clear();
            }
        }

        private bool IsDuplicateMarker(int numOfMarker)
        {
            bool ret = false;
            
            if ( MarkerParamList.ToList().Find(x => x.NumOfMarker == numOfMarker) != null)
            {
                ret = true;
            }

            return ret;
        }
    }
}
