using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using System;
using System.Windows;
using System.Windows.Input;

namespace CustomSpectrumAnalyzer
{
    public class SpectrumViewModel : ViewModelBase
    {
        public static double CanvasWidth = 0;
        public static double CanvasHeight = 0;

        public static Point ClickedPoint;
        public static Point DraggedPoint;

        public static readonly double WidthOffset = 10;
        public static readonly double HeightOffset = 10;

        public static readonly int LengthOfData = 1000;

        public static int MarkerNum = 1;

        // Y 관련
        public static readonly int CrossLineSize = 10;
        public static readonly int YInterval = 10;

        public ICommand AddMarkerCommand { get; set; }
        public ICommand EditMarkerCommand { get; set; }

        private SettingParameter settingParam;
        public SettingParameter SettingParam
        {
            get { return settingParam; }
            set { SetProperty(ref settingParam, value); }
        }

        private double lineX;
        public double LineX
        {
            get { return lineX; }
            set { lineX = value; NotifyPropertyChanged("LineX"); }
        }

        public SpectrumViewModel()
        {
            WeakReferenceMessenger.Default.Register<SettingMessage>(this, OnSettingMessage);

            AddMarkerCommand = new RelayCommand(OnAddMarker);
            EditMarkerCommand = new RelayCommand(OnEditMarker);
        }

        // Marker 위치 수정
        private void OnEditMarker()
        {
            if (SettingParam == null)
            {
                return;
            }

            WeakReferenceMessenger.Default.Send(new MarkerMessage(true)
            {
                MarkerNum = MarkerNum,
                Frequency = GetFrequencyAtScreen(DraggedPoint.X),
                Amplitude = GetAmplitudeAtScreen(DraggedPoint.Y),
                MarkerCommandType = EMarkerCommandType.Update,
            });
        }

        // MouseDown Command Function
        private void OnAddMarker()
        {
            if (SettingParam == null)
            {
                return;
            }

            LineX = ClickedPoint.X;
            var freq = GetFrequencyAtScreen(ClickedPoint.X);
            var amp = GetAmplitudeAtScreen(ClickedPoint.Y); // 실제 x에 대응되는 y값을 취해야 할듯

            WeakReferenceMessenger.Default.Send(new MarkerMessage(true)
            {
                MarkerNum = MarkerNum,
                Frequency = freq,
                Amplitude = amp,
                MarkerCommandType = EMarkerCommandType.Create,
            }) ;
        }

        private void OnSettingMessage(object recipient, SettingMessage message)
        {
            SettingParam = message.SettingParam;
            // To Do :: Setting Message에 따라 분기하여 Canvas에 도시할 것
        }

        #region Screen <-> Real Data Static Convert
        static readonly double yOffset = 0.25;
        static readonly double yRatioOffset = 0.973;
        public static double GetScaledX(int x)
        {
            return x * (CanvasWidth - WidthOffset) / LengthOfData;
        }

        public static double GetScaledY(float data, double viewerRefLv)
        {
            double y = CanvasHeight;

            // view ref level에 따라 scaled 된 y가 조정됨
            // Revision To Do 
            double yScaledRatio = (viewerRefLv - (double)data + yOffset) / (CrossLineSize * YInterval) * yRatioOffset;

            // 전체 그래프 높이를 기준으로 조정된 Y값 반환
            return y * yScaledRatio + yOffset;
        }

        /// <summary>
        /// X Index 반환
        /// </summary>
        /// <param name="xPoint"></param>
        /// <returns></returns>
        public static int GetXIndexAtScreen(double xPoint)
        {
            return (int)(xPoint * LengthOfData / (CanvasWidth - WidthOffset));
        }

        /// <summary>
        /// 화면상에서 찍은 x 위치를 실제 Frequency로 반환
        /// </summary>
        /// <param name="xPoint">Clicked x Position At Screen </param>
        /// <returns>Frequency</returns>
        public double GetFrequencyAtScreen(double xPoint)
        {
            int x = GetXIndexAtScreen(xPoint);
            double freq = SettingParam.CenterFreq - SettingParam.Span / 2 + SettingParam.Span / LengthOfData * x;
            return freq;
        }

        /// <summary>
        /// 화면상에서 찍은 y 위치를 실제 Viewer Reference Level로 반환
        /// </summary>
        /// <param name="yPoint">Clicked y Position At Screen</param>
        /// <returns>Amplitude</returns>
        public double GetAmplitudeAtScreen(double yPoint)
        {
            double amp = SettingParam.ViewerRefLv + yOffset - ((yPoint - yOffset) * CrossLineSize * YInterval) / (yRatioOffset * CanvasHeight);

            return amp;
        }
        #endregion
    }
}
