using Microsoft.Xaml.Behaviors;
using System;
using System.Windows;
using System.Windows.Input;

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

        public ICommand EditCommand
        {
            get { return (ICommand)GetValue(EditCommandProperty); }
            set { SetValue(EditCommandProperty, value); }
        }
        
        public static readonly DependencyProperty EditCommandProperty = 
            DependencyProperty.Register(nameof(EditCommand), typeof(ICommand), typeof(SpectrumCanvasBehavior), new PropertyMetadata(null, EnterCommandChanged));

        public ICommand AddMarkerCommand
        {
            get { return (ICommand)GetValue(AddMarkerCommandProperty); }
            set { SetValue(AddMarkerCommandProperty, value); }
        }

        public static readonly DependencyProperty AddMarkerCommandProperty =
            DependencyProperty.Register(nameof(AddMarkerCommand), typeof(ICommand), typeof(SpectrumCanvasBehavior), new PropertyMetadata(null, AddMarkerCommandChanged));

        private static void AddMarkerCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var behavior = (SpectrumCanvasBehavior)d;
        }

        private static void EnterCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var behavior = (SpectrumCanvasBehavior)d;
        }

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

        #region Behavior Override
        protected override void OnAttached()
        {
            // Event Handler 추가
            AssociatedObject.MarkerThumb.DragDelta += MarkerThumb_DragDelta;
            AssociatedObject.SpectrumPolyline.MouseDown += SpectrumPolyline_MouseDown;
        }

        protected override void OnDetaching()
        {
            // EventHandler 제거
            AssociatedObject.MarkerThumb.DragDelta -= MarkerThumb_DragDelta;
            AssociatedObject.SpectrumPolyline.MouseDown -= SpectrumPolyline_MouseDown;
        }
        #endregion

        private void SpectrumPolyline_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AssociatedObject.AddMarker(e);
            AddMarkerCommand.Execute(e);
        }


        private void MarkerThumb_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            AssociatedObject.MarkerDragged(sender, e);
            EditCommand.Execute(e);
        }
    }
}