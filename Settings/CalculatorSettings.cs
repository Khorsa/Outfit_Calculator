using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Settings
{
    public class CalculatorSettings
    {
        public const int DEFAULT_ROUND_TO = 10;

        public double left = int.MaxValue;
        public double top = int.MaxValue;
        public double width = 247;
        public double height = 288;
        public int roundTo = DEFAULT_ROUND_TO;
    }
}
