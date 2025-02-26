using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using YamlDotNet.Core.Tokens;
using Key = System.Windows.Input.Key;

namespace Calculator
{
    class CalculatorKeysConverter
    {
        public static CalculatorKey? fromKeyboard(KeyEventArgs e)
        {
            Key key = e.Key;
            if (
                Keyboard.IsKeyDown(Key.LeftAlt)
                || Keyboard.IsKeyDown(Key.RightAlt)
                || Keyboard.IsKeyDown(Key.LWin)
                || Keyboard.IsKeyDown(Key.RWin)
            )
            {
                return null;
            }

            bool isShiftPressed = Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift);
            bool isControlPressed = Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl);

            var keyConvertArrayShift = new Dictionary<Key, CalculatorKey>
            {
                { Key.D8, CalculatorKey.Mult },
                { Key.Enter, CalculatorKey.Result },
            };

            var keyConvertArrayControl = new Dictionary<Key, CalculatorKey>
            {
                { Key.C, CalculatorKey.Copy },
                { Key.V, CalculatorKey.Paste },
                { Key.X, CalculatorKey.Cut },
            };

            var keyConvertArray = new Dictionary<Key, CalculatorKey>
            {
                { Key.D0, CalculatorKey.D0},
                { Key.D1, CalculatorKey.D1},
                { Key.D2, CalculatorKey.D2},
                { Key.D3, CalculatorKey.D3},
                { Key.D4, CalculatorKey.D4},
                { Key.D5, CalculatorKey.D5},
                { Key.D6, CalculatorKey.D6},
                { Key.D7, CalculatorKey.D7},
                { Key.D8, CalculatorKey.D8},
                { Key.D9, CalculatorKey.D9},

                { Key.NumPad0, CalculatorKey.D0},
                { Key.NumPad1, CalculatorKey.D1},
                { Key.NumPad2, CalculatorKey.D2},
                { Key.NumPad3, CalculatorKey.D3},
                { Key.NumPad4, CalculatorKey.D4},
                { Key.NumPad5, CalculatorKey.D5},
                { Key.NumPad6, CalculatorKey.D6},
                { Key.NumPad7, CalculatorKey.D7},
                { Key.NumPad8, CalculatorKey.D8},
                { Key.NumPad9, CalculatorKey.D9},

                { Key.OemMinus, CalculatorKey.Minus},
                { Key.OemPlus, CalculatorKey.Plus},
                { Key.Subtract, CalculatorKey.Minus},
                { Key.Add, CalculatorKey.Plus},

                { Key.OemQuestion, CalculatorKey.Divide},
                { Key.OemPeriod, CalculatorKey.Dot},


                { Key.Back, CalculatorKey.Back},
                { Key.Return, CalculatorKey.Result},
                { Key.Delete, CalculatorKey.Reset},

                { Key.Divide, CalculatorKey.Divide},
                { Key.Multiply, CalculatorKey.Mult},
                { Key.Decimal, CalculatorKey.Dot},
            };

            if (isShiftPressed && keyConvertArrayShift.ContainsKey(key))
            {
                return keyConvertArrayShift[key];
            }
            else if (isControlPressed && keyConvertArrayControl.ContainsKey(key))
            {
                return keyConvertArrayControl[key];
            }
            else if (keyConvertArray.ContainsKey(key))
            {
                return keyConvertArray[key];
            }

            return null;
        }

        internal static CalculatorKey? fromButton(string? buttonText)
        {
            var buttonConvertArrayShift = new Dictionary<string, CalculatorKey>
            {
                { "←", CalculatorKey.Back },
                { "C", CalculatorKey.Reset },
                { "/", CalculatorKey.Divide },
                { "*", CalculatorKey.Mult },
                { "–", CalculatorKey.Minus },
                { "Ln", CalculatorKey.Ln },
                { "7", CalculatorKey.D7 },
                { "8", CalculatorKey.D8},
                { "9", CalculatorKey.D9 },
                { "+", CalculatorKey.Plus },
                { "1/x", CalculatorKey.OneDivX},
                { "4", CalculatorKey.D4 },
                { "5", CalculatorKey.D5 },
                { "6", CalculatorKey.D6 },
                { "x²", CalculatorKey.Square },
                { "1", CalculatorKey.D1 },
                { "2", CalculatorKey.D2 },
                { "3", CalculatorKey.D3 },
                { "=", CalculatorKey.Result },
                { "√", CalculatorKey.Sqrt },
                { "±", CalculatorKey.PlusMinus },
                { "0", CalculatorKey.D0 },
                { ".", CalculatorKey.Dot },
            };

            if (buttonText != null && buttonConvertArrayShift.ContainsKey(buttonText))
            {
                return buttonConvertArrayShift[buttonText];
            }

            return null;
        }
    }
}
