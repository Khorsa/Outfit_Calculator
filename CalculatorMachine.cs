using Calculator.Settings;
using System.Data;

namespace Calculator
{
    class CalculatorMachine
    {
        private const int MAX_HISTORY_SIZE = 10;

        private readonly CalculatorSettings settings;
        private List<CalculationHistoryItem> history = new List<CalculationHistoryItem>();
        public CalculatorMachine(CalculatorSettings settings) {
            this.settings = settings;
        }

        public string calculate(string expression)
        {
            DataTable table = new DataTable();
            var x = table.Compute(expression.Replace(",", "."), string.Empty);

            double result = 0;
            double.TryParse(Convert.ToString(x) ?? "", out result);
            result = Math.Round(result, settings.roundTo);

            history.Add(new CalculationHistoryItem(expression, result.ToString()));
            if (history.Count > MAX_HISTORY_SIZE){
                history = history.Slice(history.Count - MAX_HISTORY_SIZE, MAX_HISTORY_SIZE);
            }

            return result.ToString();
        }

        public List<CalculationHistoryItem> getLastHistory()
        {
            return history;
        }
    }
}
