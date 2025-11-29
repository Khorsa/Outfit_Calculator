using Calculator.Settings;
using System.Data;
using System.Globalization;

namespace Calculator;

class CalculatorMachine(CalculatorSettings settings)
{
    private const int MAX_HISTORY_SIZE = 10;

    public const string SQRT = "√";      // Квадратный корень
    public const string SQUARE = "²";    // Квадрат числа
    public const string ONE_DIV_X = "⅟";   // 1/x
    public const string LN = "Ln";       // Натуральный логарифм

    private List<CalculationHistoryItem> _history = new();

    public string Calculate(string expression)
    {
        expression = NormalizeExpression(expression);

        double result = CalculateString(expression);

        result = Math.Round(result, settings.RoundTo);

        if (expression == result.ToString(CultureInfo.InvariantCulture)) return result.ToString(CultureInfo.InvariantCulture);
        _history.Add(new CalculationHistoryItem(expression, result.ToString(CultureInfo.InvariantCulture)));
        if (_history.Count > MAX_HISTORY_SIZE)
        {
            _history = _history.Slice(_history.Count - MAX_HISTORY_SIZE, MAX_HISTORY_SIZE);
        }

        return result.ToString(CultureInfo.InvariantCulture);
    }

    public List<CalculationHistoryItem> GetLastHistory()
    {
        return _history;
    }


    /**
     * Предварительные вычисления, в основном по унарным операторам
     */
    private double CalculateString(string expression)
    {
        // Выделяем выражение без унарного оператора;
        // Вычисляем его;
        // Применяем к результату оператор.

        string expressionToCalculate = expression;
        string? unaryOperator = null;

        if (expression.IndexOf(CalculatorMachine.SQRT, StringComparison.Ordinal) == 0)
        {
            unaryOperator = CalculatorMachine.SQRT;
            expressionToCalculate = expression.Substring(unaryOperator.Length);
        }
        if (expression.IndexOf(CalculatorMachine.LN, StringComparison.Ordinal) == 0) { 
            unaryOperator = CalculatorMachine.LN;
            expressionToCalculate = expression.Substring(unaryOperator.Length);
        }
        if (expression.IndexOf(CalculatorMachine.SQUARE, StringComparison.Ordinal) == expression.Length - 1)
        {
            unaryOperator = CalculatorMachine.SQUARE;
            expressionToCalculate = expression.Substring(0, expression.Length - unaryOperator.Length);
        }
        if (expression.IndexOf(CalculatorMachine.ONE_DIV_X, StringComparison.Ordinal) == 0)
        {
            unaryOperator = CalculatorMachine.ONE_DIV_X;
            expressionToCalculate = expression.Substring(unaryOperator.Length);
        }

        double result = SimpleCalculate(expressionToCalculate);

        if (unaryOperator == CalculatorMachine.SQRT)
        {
            result = Math.Sqrt(result);
        }
        if (unaryOperator == CalculatorMachine.LN)
        {
            result = Math.Log(result);
        }
        if (unaryOperator == CalculatorMachine.SQUARE)
        {
            result *= result;
        }
        if (unaryOperator == CalculatorMachine.ONE_DIV_X)
        {
            result = 1/ result;
        }

        return result;
    }


    private string NormalizeExpression(string expression)
    {
        return expression.Replace(",", ".").Replace(" ", "");
    }

    private double SimpleCalculate(string expression)
    {
        DataTable table = new DataTable();
        var x = table.Compute(expression.Replace('×', '*'), string.Empty);
        double.TryParse(Convert.ToString(x) ?? "", out var result);

        return result;
    }


}