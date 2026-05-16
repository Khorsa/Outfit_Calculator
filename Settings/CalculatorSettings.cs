/*
 * Copyright (C) 2026 Roman Stolyarov <rshome@mail.ru>
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 */
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