using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    class CalculatorMachine
    {
        public string calculate(string expression)
        {
            DataTable table = new DataTable();
            var result = table.Compute(expression.Replace(",", "."), string.Empty);
            return Convert.ToString(result);
        }
    }
}
