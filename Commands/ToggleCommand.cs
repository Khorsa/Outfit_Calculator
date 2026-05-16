/*
 * Copyright (C) 2026 Roman Stolyarov <rshome@mail.ru>
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 */
using System.Reflection;
using System.Windows.Media.Imaging;
using System.Windows;
using OutfitTool.API1;
using OutfitTool.API1.Dto;

namespace Calculator.Commands;

class ToggleCommand: ICommand
{
    public string Name => "ShowHide";
    public string Description => LocalizationHelper.GetString("ShowHide");
    public CommandContextMenu ContextMenu => GetContextMenu();

    private CommandContextMenu GetContextMenu()
    {
        string resourceName = "calculator.png";
        return new CommandContextMenu(
            LocalizationHelper.GetString("Calculator"),
            new BitmapImage(
                new Uri("pack://application:,,,/" + Assembly.GetExecutingAssembly().GetName().Name + ";component/Resources/" + resourceName)
            )
        );
    }

    public void Run()
    {
        if (ModuleController.CalculatorWindow == null)
        {
            return;
        }

        if (ModuleController.CalculatorWindow.Visibility == Visibility.Visible)
        {
            ModuleController.CalculatorWindow.Hide();
        }
        else
        {
            ModuleController.CalculatorWindow.Topmost = true;
            ModuleController.CalculatorWindow.Show();
            ModuleController.CalculatorWindow.Activate();
            ModuleController.CalculatorWindow.Focus();
        }
    }
}