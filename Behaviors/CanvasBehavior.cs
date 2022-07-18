using Microsoft.Xaml.Behaviors;
using System.Windows;

namespace CustomSpectrumAnalyzer
{
    // Spectrum을 보여줄 Canvas에 대한 Behavior 정의
    public class SpectrumCanvasBehavior : Behavior<SpectrumCanvasUC>
    {
        #region Dependency Properties
        public SettingParameter SettingParam
        {
            get { return (SettingParameter)GetValue(SettingParamProperty); }
            set { SetValue(SettingParamProperty, value); }
        }

        // nameof keyword :: 변수 이름을 상수로 반환함
        public static readonly DependencyProperty SettingParamProperty
            = DependencyProperty.Register(nameof(SettingParam), typeof(SettingParameter), typeof(SpectrumCanvasBehavior), new PropertyMetadata(null, ViewerRefLvChanged));

        private static void ViewerRefLvChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var behavior = (SpectrumCanvasBehavior)d;
            SettingParameter param = e.NewValue as SettingParameter;

            if (param.CommandType == ESettingCommandType.Applied)
            {
                behavior.SetOptions(param);
            }

            else if ( param.CommandType == ESettingCommandType.ResetMarker)
            {
                behavior.ResetMarker();
            }
        }

        #endregion 
        
        private void SetOptions(SettingParameter param)
        {
            AssociatedObject.SetOptions(param);
        }

        private void ResetMarker()
        {
            AssociatedObject.ResetAllMarker();
        }
    }
}
