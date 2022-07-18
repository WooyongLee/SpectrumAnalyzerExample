﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomSpectrumAnalyzer
{
    public class SettingParameter
    {
        public ESettingCommandType CommandType { get; set; }

        public double CenterFreq { get; set; }

        public double Span { get; set; }

        public double ViewerRefLv { get; set; }

        public SettingParameter(ESettingCommandType eSettingCommand)
        {
            this.CommandType = eSettingCommand;
        }
    }
}