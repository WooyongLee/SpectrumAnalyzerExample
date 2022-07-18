using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.ComponentModel;
using System.Diagnostics;

namespace CustomSpectrumAnalyzer
{
    public class ViewModelBase : ObservableObject, INavigationAware
    {
        #region INotifyPropertyChanged 구현부
        public event PropertyChangedEventHandler PropertyChanged;

        // 각 Property 이름으로 지정해 놓고 UI 쪽으로 변경에 대한 이벤트 구현
        protected void NotifyPropertyChanged(string propertyName = "")
        {
            this.VerifyPropertyName(propertyName);

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public void VerifyPropertyName(string propertyName)
        {
            // Verify that the property name matches a real,  
            // public, instance property on this object.
            if (TypeDescriptor.GetProperties(this)[propertyName] == null)
            {
                string msg = "Invalid property name: " + propertyName;

                throw new Exception(msg);
            }
        }

        // Complete to Navigating
        public void OnNavigating(object sender, object navigationEventArgs)
        {
        }

        // Start To Navigate
        public void OnNavigated(object sender, object navigationEventArgs)
        {
        }
        #endregion
    }
}
