using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomSpectrumAnalyzer
{
    // Navigation Start-Stop 시점을 ViewModel에 알려주는 Interface
    public interface INavigationAware
    {
        void OnNavigating(object sender, object navigationEventArgs);
        void OnNavigated(object sender, object navigationEventArgs);
    }
}
