using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using YamlDotNet.Core.Tokens;
using YamlDotNet.Serialization;

namespace Calculator
{
    /// <summary>
    /// Логика взаимодействия для Calculator.xaml
    /// </summary>
    public partial class CalculatorForm : Window
    {
        private CalculatorMachine calculator;
        bool resetOnCipher = false;
        string decimalSeparator = ".";
        private double _baseFontSize = 16;

        public CalculatorForm()
        {
            calculator = new CalculatorMachine();
            CultureInfo currentCulture = CultureInfo.CurrentCulture;
            decimalSeparator = currentCulture.NumberFormat.NumberDecimalSeparator;

            LocalizationHelper.LanguageChanged += LanguageChanged;

            InitializeComponent();
            UpdateFontSize();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateFontSize();
        }

        private void UpdateFontSize()
        {
            double FontSize = _baseFontSize * (Height / 400); // 400 - базовая высота окна

            List<Button> buttons = new List<Button>();
            FindAllButtons(this, buttons);
            foreach(Button b in buttons)
            {
                b.FontSize = FontSize;
            }
            ResultBox.FontSize = FontSize * 1.6;
            
        }

        private void FindAllButtons(DependencyObject parent, List<Button> buttons)
        {
            if (parent == null) return;
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
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
                Debug.WriteLine(e.Key);

                CalculatorKey? key = CalculatorKeysConverter.fromKeyboard(e);

                process(key);
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender == null)
            {
                return;
            }
            string buttonText = (sender as Button).Content.ToString();
            CalculatorKey? key = CalculatorKeysConverter.fromButton(buttonText);

            process(key);

        }

        private void process(CalculatorKey? key)
        {
            if (key == null) return;

            try
            {
                if (key == CalculatorKey.D0) addSymbol("0");
                if (key == CalculatorKey.D1) addSymbol("1");
                if (key == CalculatorKey.D2) addSymbol("2");
                if (key == CalculatorKey.D3) addSymbol("3");
                if (key == CalculatorKey.D4) addSymbol("4");
                if (key == CalculatorKey.D5) addSymbol("5");
                if (key == CalculatorKey.D6) addSymbol("6");
                if (key == CalculatorKey.D7) addSymbol("7");
                if (key == CalculatorKey.D8) addSymbol("8");
                if (key == CalculatorKey.D9) addSymbol("9");
                if (key == CalculatorKey.Dot) addSymbol(decimalSeparator);
                resetOnCipher = false;

                if (key == CalculatorKey.Plus) ResultBox.Text = calculator.calculate(ResultBox.Text) + "+";
                if (key == CalculatorKey.Minus) ResultBox.Text = calculator.calculate(ResultBox.Text) + "-";
                if (key == CalculatorKey.Mult) ResultBox.Text = calculator.calculate(ResultBox.Text) + "*";
                if (key == CalculatorKey.Divide) ResultBox.Text = calculator.calculate(ResultBox.Text) + "/";

                if (key == CalculatorKey.Square)
                {
                    decimal x = Convert.ToDecimal(calculator.calculate(ResultBox.Text));
                    ResultBox.Text = (x * x).ToString();
                    resetOnCipher = true;
                }

                if (key == CalculatorKey.Sqrt)
                {
                    double x = Convert.ToDouble(calculator.calculate(ResultBox.Text));
                    ResultBox.Text = Math.Sqrt(x).ToString();
                    resetOnCipher = true;
                }

                if (key == CalculatorKey.OneDivX)
                {
                    double x = Convert.ToDouble(calculator.calculate(ResultBox.Text));
                    ResultBox.Text = (1 / x).ToString();
                    resetOnCipher = true;
                }

                if (key == CalculatorKey.Ln)
                {
                    double x = Convert.ToDouble(calculator.calculate(ResultBox.Text));
                    ResultBox.Text = Math.Log(x).ToString();
                    resetOnCipher = true;
                }

                if (key == CalculatorKey.Back && ResultBox.Text.Length > 0)
                {
                    ResultBox.Text = ResultBox.Text[..^1];
                }

                if (key == CalculatorKey.PlusMinus && ResultBox.Text.Length > 0)
                {
                    ResultBox.Text = calculator.calculate(ResultBox.Text);
                    if (ResultBox.Text[..1] == "-")
                    {
                        ResultBox.Text = ResultBox.Text[1..];
                    }
                    else
                    {
                        ResultBox.Text = "-" + ResultBox.Text;
                    }
                }

                if (key == CalculatorKey.Reset) ResultBox.Text = "";

                if (key == CalculatorKey.Result)
                {
                    ResultBox.Text = calculator.calculate(ResultBox.Text);
                    resetOnCipher = true;
                }


                if (key == CalculatorKey.Copy)
                {
                    Clipboard.SetText(ResultBox.Text);
                    animateResultBox(Brushes.LightGreen);
                }

                if (key == CalculatorKey.Cut)
                {
                    Clipboard.SetText(ResultBox.Text);
                    animateResultBox(Brushes.OrangeRed);
                    ResultBox.Text = "";
                }
                if (key == CalculatorKey.Paste)
                {
                    ResultBox.Text = Clipboard.GetText();
                    animateResultBox(Brushes.LightYellow);
                }
            }
            catch
            {
                ResultBox.Text = "Ошибка";
            }

        }


        private void animateResultBox(SolidColorBrush color, int duration = 200, int delay = 100)
        {
            var background = color.Clone();
            ResultBox.Background = background;

            ColorAnimation fadeOutAnimation = new ColorAnimation
            {
                To = Colors.White,
                Duration = TimeSpan.FromMilliseconds(200),
                BeginTime = TimeSpan.FromMilliseconds(100)
            };

            ResultBox.Background.BeginAnimation(SolidColorBrush.ColorProperty, fadeOutAnimation);
        }


        private void addSymbol(string s)
        {
            if (resetOnCipher)
            {
                ResultBox.Text = s;
            }
            else
            {
                ResultBox.Text += s;
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
        }
    }
}
