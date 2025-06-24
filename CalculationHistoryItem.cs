using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    internal class CalculationHistoryItem
    {
        public CalculationHistoryItem(string Expression, string Result) 
        {
            this.Expression = Expression;
            this.Result = Result;
        }

        public string Expression { get; set; }
        public string Result { get; set; }
    }
}
