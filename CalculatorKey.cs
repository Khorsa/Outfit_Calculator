using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    public enum CalculatorKey
    {
        Result,

        D0,
        D1,
        D2,
        D3,
        D4,
        D5,
        D6,
        D7,
        D8,
        D9,

        Dot,

        Plus,
        Minus,
        Mult,
        Divide,

        Back,
        PlusMinus,

        Reset,  // Сброс
        Sqrt,   // Квадратный корень
        Square, // Квадрат числа
        OneDivX,// 1/x
        Ln,     // Натуральный логарифм

        Copy,
        Paste,
        Cut
    }
}
