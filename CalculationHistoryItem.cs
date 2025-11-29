namespace Calculator;

internal class CalculationHistoryItem(string expression, string result)
{
    public string Expression { get; } = expression;
    public string Result { get; } = result;
}