namespace Calculator.Settings;

public class CalculatorSettings
{
    public const int DEFAULT_ROUND_TO = 10;

    public double Left = int.MaxValue;
    public double Top = int.MaxValue;
    public double Width = 247;
    public double Height = 288;
    public int RoundTo = DEFAULT_ROUND_TO;
    public bool CopyResultToClipboard = true;
}