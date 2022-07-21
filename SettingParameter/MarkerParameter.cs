using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomSpectrumAnalyzer
{
    public class MarkerParameter
    {
        public int NumOfMarker { get; set; }

        // Targeting X Frequency
        public double Frequency { get; set; }

        // Y Amplitude (dBm)
        public double Amp { get; set; }
    }
}
