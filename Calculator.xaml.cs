using Calculator.Settings;
using OutfitTool.API1;
using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;


namespace Calculator;

/// <summary>
/// Логика взаимодействия для Calculator.xaml
/// </summary>
public partial class CalculatorForm
{
    private const double MIN_HEIGHT = 300;
    private const double MIN_WIDTH = 250;

    private readonly CalculatorMachine _calculator;

    private bool _resetOnCipher;
    private readonly string _decimalSeparator;
    private const double BASE_FONT_SIZE = 16;

    private readonly SettingsManager<CalculatorSettings> _settingsManager = new();
    private readonly CalculatorSettings? _settings;

    private string CalculateTextBox {
        get => ResultBox.Text;
        set
        {
            ResultBox.Text = value;
            if (_settings is not { CopyResultToClipboard: true } || !_resetOnCipher) return;
            try
            {
                Clipboard.SetText(value);
            }
            catch
            {
                // ignored
            }

            Thread.Sleep(30); // Обход проблемы переспамливания Clipboard
            AnimateResultBox(Brushes.LightGreen, 100, 0);
        }
    }

    public CalculatorForm()
    {
        _settings = _settingsManager.LoadSettings();

        _calculator = new CalculatorMachine(_settings);
        var currentCulture = CultureInfo.CurrentCulture;
        _decimalSeparator = currentCulture.NumberFormat.NumberDecimalSeparator;

        LocalizationHelper.LanguageChanged += LanguageChanged;

        InitializeComponent();
        UpdateHistory();

        if (_settings != null)
        {
            FixCoordinates(
                ref _settings.Top,
                ref _settings.Left,
                ref _settings.Width,
                ref _settings.Height
            );

            this.Top = _settings.Top;
            this.Left = _settings.Left;
            this.Width = _settings.Width;
            this.Height = _settings.Height;
        }
        UpdateStatusRound();
        UpdateStatusSettings();

        UpdateFontSize();
    }

    private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        UpdateFontSize();
    }

    private void UpdateFontSize()
    {
        var fontSize = BASE_FONT_SIZE * (Height / 400); // 400 - базовая высота окна

        var buttons = new List<Button>();
        FindAllButtons(this, buttons);
        foreach(var b in buttons)
        {
            b.FontSize = fontSize;
        }
        ResultBox.FontSize = fontSize * 1.6;
            
    }

    private void FindAllButtons(DependencyObject? parent, List<Button> buttons)
    {
        if (parent == null) return;
        for (var i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
        {
            var child = VisualTreeHelper.GetChild(parent, i);
            if (child is Button button)
            {
                buttons.Add(button);
            }
            FindAllButtons(child, buttons);
        }
    }

    private void Window_KeyUp(object sender, KeyEventArgs e)
    {
        if (this.Visibility == Visibility.Visible)
        {
            CalculatorKey? key = CalculatorKeysConverter.FromKeyboard(e);
            Process(key);
        }
    }
    private void Button_Click(object? sender, RoutedEventArgs e)
    {
        if (sender == null)
        {
            return;
        }
        var buttonText = (sender as Button)?.Content.ToString();
        var key = CalculatorKeysConverter.FromButton(buttonText);

        Process(key);

    }

    private void Process(CalculatorKey? key)
    {
        if (key == null) return;

        try
        {
            if (key == CalculatorKey.D0) AddSymbol("0");
            if (key == CalculatorKey.D1) AddSymbol("1");
            if (key == CalculatorKey.D2) AddSymbol("2");
            if (key == CalculatorKey.D3) AddSymbol("3");
            if (key == CalculatorKey.D4) AddSymbol("4");
            if (key == CalculatorKey.D5) AddSymbol("5");
            if (key == CalculatorKey.D6) AddSymbol("6");
            if (key == CalculatorKey.D7) AddSymbol("7");
            if (key == CalculatorKey.D8) AddSymbol("8");
            if (key == CalculatorKey.D9) AddSymbol("9");
            if (key == CalculatorKey.Dot) AddSymbol(_decimalSeparator);

            if (key == CalculatorKey.Paste)
            {
                Debug.WriteLine(_resetOnCipher);
                if (_resetOnCipher)
                {
                    _resetOnCipher = false;
                    CalculateTextBox = Clipboard.GetText();
                    _resetOnCipher = true;
                    AnimateResultBox(Brushes.LightYellow);
                }
                else
                {
                    _resetOnCipher = true;
                    CalculateTextBox = _calculator.Calculate(CalculateTextBox + Clipboard.GetText());
                }

            }

            _resetOnCipher = false;

            if (key == CalculatorKey.Plus) CalculateTextBox = _calculator.Calculate(CalculateTextBox) + "+";
            if (key == CalculatorKey.Minus) CalculateTextBox = _calculator.Calculate(CalculateTextBox) + "-";
            if (key == CalculatorKey.Mult) CalculateTextBox = _calculator.Calculate(CalculateTextBox) + "×";
            if (key == CalculatorKey.Divide) CalculateTextBox = _calculator.Calculate(CalculateTextBox) + "/";

            if (key == CalculatorKey.Square)
            {
                _resetOnCipher = true;
                CalculateTextBox = NeedBracket(CalculateTextBox) ? 
                    _calculator.Calculate("(" + CalculateTextBox + ")" + CalculatorMachine.SQUARE) :
                    _calculator.Calculate(CalculateTextBox + CalculatorMachine.SQUARE);
            }

            if (key == CalculatorKey.Sqrt)
            {
                _resetOnCipher = true;
                CalculateTextBox = NeedBracket(CalculateTextBox) ?
                    _calculator.Calculate(CalculatorMachine.SQRT + "(" + CalculateTextBox + ")") :
                    _calculator.Calculate(CalculatorMachine.SQRT + CalculateTextBox);
            }

            if (key == CalculatorKey.OneDivX)
            {
                _resetOnCipher = true;
                CalculateTextBox = NeedBracket(CalculateTextBox) ?
                    _calculator.Calculate(CalculatorMachine.ONE_DIV_X + "(" + CalculateTextBox + ")") :
                    _calculator.Calculate(CalculatorMachine.ONE_DIV_X + CalculateTextBox);
            }

            if (key == CalculatorKey.Ln)
            {
                _resetOnCipher = true;
                CalculateTextBox = NeedBracket(CalculateTextBox) ?
                    _calculator.Calculate(CalculatorMachine.LN + "(" + CalculateTextBox + ")") :
                    _calculator.Calculate(CalculatorMachine.LN + CalculateTextBox);
            }

            if (key == CalculatorKey.Back && CalculateTextBox.Length > 0)
            {
                CalculateTextBox = CalculateTextBox[..^1];
            }

            if (key == CalculatorKey.PlusMinus && CalculateTextBox.Length > 0)
            {
                _resetOnCipher = true;
                CalculateTextBox = NeedBracket(CalculateTextBox) ?
                    _calculator.Calculate("-(" + CalculateTextBox + ")") :
                    _calculator.Calculate("-" + CalculateTextBox);
            }

            if (key == CalculatorKey.Reset) CalculateTextBox = "";

            if (key == CalculatorKey.Result)
            {
                if (CalculateTextBox != "")
                {
                    _resetOnCipher = true;
                    CalculateTextBox = _calculator.Calculate(CalculateTextBox);
                }
            }


            if (key == CalculatorKey.Copy)
            {
                _resetOnCipher = true;
                Clipboard.SetText(CalculateTextBox);
                AnimateResultBox(Brushes.LightGreen);
            }

            if (key == CalculatorKey.Cut)
            {
                _resetOnCipher = true;
                Clipboard.SetText(CalculateTextBox);
                AnimateResultBox(Brushes.OrangeRed);
                _resetOnCipher = false;
                CalculateTextBox = "";
            }
        }
        catch
        {
            CalculateTextBox = "Ошибка";
        }
    }


    private void AnimateResultBox(SolidColorBrush color, int duration = 200, int delay = 100)
    {
        var background = color.Clone();
        ResultBox.Background = background;

        ColorAnimation fadeOutAnimation = new ColorAnimation
        {
            To = Colors.LightCyan,
            Duration = TimeSpan.FromMilliseconds(duration),
            BeginTime = TimeSpan.FromMilliseconds(delay)
        };

        ResultBox.Background.BeginAnimation(SolidColorBrush.ColorProperty, fadeOutAnimation);
    }


    private void AddSymbol(string s)
    {
        if (_resetOnCipher)
        {
            CalculateTextBox = s;
        }
        else
        {
            CalculateTextBox += s;
        }
    }

    protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
    {
        e.Cancel = true;
        this.Hide();
    }

    private void LanguageChanged(Object? sender, EventArgs e)
    {
        ResourceDictionary oldDict = (from d in this.Resources.MergedDictionaries
            where d.Source != null && d.Source.OriginalString.Contains("Languages/lang.")
            select d).First();
        if (oldDict != null)
        {
            int ind = this.Resources.MergedDictionaries.IndexOf(oldDict);
            this.Resources.MergedDictionaries.Remove(oldDict);
            this.Resources.MergedDictionaries.Insert(ind, LocalizationHelper.CurrentDictionary);
        }
        else
        {
            this.Resources.MergedDictionaries.Add(LocalizationHelper.CurrentDictionary);
        }

        UpdateStatusRound();
        UpdateStatusSettings();
    }

    private static void FixCoordinates(ref double top, ref double left, ref double width, ref double height)
    {
        if (width < MIN_WIDTH) width = MIN_WIDTH;
        if (height < MIN_HEIGHT) height = MIN_HEIGHT;

        if (width > SystemParameters.WorkArea.Width) width = SystemParameters.WorkArea.Width;
        if (height > SystemParameters.WorkArea.Height) height = SystemParameters.WorkArea.Height;


        if (top < 0) top = 0;
        if (left < 0) left = 0;

        if (top > SystemParameters.WorkArea.Height - height) top = SystemParameters.WorkArea.Height - height;
        if (left > SystemParameters.WorkArea.Width - width) left = SystemParameters.WorkArea.Width - width;
    }

    public void SaveWindowSettings()
    {
        if (_settings != null)
        {
            _settings.Top = Top;
            _settings.Left = Left;
            _settings.Width = Width;
            _settings.Height = Height;
            _settingsManager.SaveSettings(_settings);
        }
    }

    private void RoundItem_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not MenuItem menuItem) return;
        if (_settings == null) return;
        _settings.RoundTo = ConvertStringToInt(menuItem.Header.ToString());
        UpdateStatusRound();
        _settingsManager.SaveSettings(_settings);
    }

    private void UpdateStatusRound()
    {
        StatusRoundText.Text = LocalizationHelper.GetString("RoundTo") + ": " + _settings?.RoundTo;
        foreach (MenuItem item in StatusRoundSelector.Items)
        {
            item.IsChecked = item.Header.ToString() == _settings?.RoundTo.ToString();
        }
    }
    private void UpdateStatusSettings()
    {
        foreach (MenuItem item in StatusSettingsSelector.Items)
        {
            if (item.Tag.ToString() != nameof(_settings.CopyResultToClipboard)) continue;
            if (_settings != null) item.IsChecked = _settings.CopyResultToClipboard;
        }
    }


    private int ConvertStringToInt(string? s)
    {
        if (s != null && int.TryParse(s, out _)) {
            return Convert.ToInt32(s);
        }
        return CalculatorSettings.DEFAULT_ROUND_TO;
    }

    private void Window_Activated(object sender, EventArgs e)
    {
        AnimateResultBox(Brushes.Black, 600, 0);
    }

    private void ResultBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        UpdateHistory();
    }

    private void UpdateHistory()
    {
        if (_calculator.GetLastHistory().Count == 0)
        {
            LastHistoryItemText.Text = "";
            return;
        }
        var lastItem = _calculator.GetLastHistory().Last();
        LastHistoryItemText.Text = lastItem.Expression + " = " + lastItem.Result;


        StatusHistorySelector.Items.Clear();
        foreach (var item in _calculator.GetLastHistory()) 
        {
            var menuItem = new MenuItem
            {
                Header = item.Expression + " = " + item.Result,
                Tag = item
            };
            menuItem.Click += HistoryItemClick;

            StatusHistorySelector.Items.Add(menuItem);
        }
    }

    private void HistoryItemClick(object sender, RoutedEventArgs e)
    {
        if ((sender as MenuItem)?.Tag is not CalculationHistoryItem) return;
        var calculateTextBox = ((sender as MenuItem)?.Tag as CalculationHistoryItem)?.Result;
        if (calculateTextBox != null) CalculateTextBox = calculateTextBox;
    }

    private bool NeedBracket(string expression)
    {
        return !Regex.IsMatch(expression, @"^[0-9.,]*$");
    }

    private void LastHistoryItemText_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (_calculator.GetLastHistory().Count == 0)
        {
            return;
        }
        var lastItem = _calculator.GetLastHistory().Last();
        CalculateTextBox = lastItem.Result;
    }

    private void StatusSettings_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not MenuItem item || item.Tag.ToString() != nameof(_settings.CopyResultToClipboard)) return;
        if (_settings != null)
        {
            _settings.CopyResultToClipboard = !_settings.CopyResultToClipboard;
            _settingsManager.SaveSettings(_settings);
        }

        UpdateStatusSettings();
    }
}