using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CustomSpectrumAnalyzer
{ 
    public class SettingBehavior : Behavior<SettingPage>
    {
        #region Control Name Component
        public string ControlName
        {
            get { return (string)GetValue(ControlNameProperty); }
            set { SetValue(ControlNameProperty, value); }
        }

        public static readonly DependencyProperty ControlNameProperty =
            DependencyProperty.Register(nameof(ControlName), typeof(string), typeof(SettingBehavior), new PropertyMetadata(null, ControlNameChanged));

        private static void ControlNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var behavior = (SettingBehavior)d;
            behavior.ResloveControl();
        }

        // ControlName을 Instance화
        private void ResloveControl()
        {
            if (string.IsNullOrEmpty(ControlName))
            {
                AssociatedObject.Content = null;
            }

            else
            {
                // GetType 이용하기 위한 AssemblyQualifiedName 필요
                // Type이 항상 Null로 들어오는데 확인 필요함 
                var type = Type.GetType($"WpfFramework.Control.{ControlName}, WpfFramework, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
                if (type == null)
                {
                    return;
                }
                var control = App.Current.Services.GetService(type);
                AssociatedObject.Content = control;
            }
        }
        #endregion
    }
}
