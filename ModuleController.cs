using Calculator.Commands;
using OutfitTool.Common;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Drawing;
using Calculator.Settings;
using System.Globalization;

namespace Calculator
{
    internal class ModuleController : ModuleControllerInterface
    {
        public static Window? calculatorWindow = null;
        private SettingsManager<CalculatorSettings> settingsManager;

        public ModuleController() 
        {
            this.settingsManager = new SettingsManager<CalculatorSettings>();
        }


        public List<CommandInterface> getCommandList()
        {
            return new List<CommandInterface>([
                new ToggleCommand()
            ]);
        }

        public void init()
        {
            var settings = this.settingsManager.LoadSettings();

            calculatorWindow = new CalculatorForm();

            if (settings.left < 0 || settings.left > SystemParameters.WorkArea.Width - settings.width)
            {
                settings.left = SystemParameters.WorkArea.Width - settings.width;
            }

            if (settings.top < 0 || settings.top > SystemParameters.WorkArea.Height - settings.height)
            {
                settings.top = SystemParameters.WorkArea.Height - settings.height;
            }

            calculatorWindow.Top = settings.top;
            calculatorWindow.Left = settings.left;
            calculatorWindow.Width = settings.width;
            calculatorWindow.Height = settings.height;

            return;
        }

        public void setLanguage(string language)
        {
            LocalizationHelper.Language = new CultureInfo(language);
        }

        public void shutdown()
        {
            if (calculatorWindow != null)
            {
                // Запоминаем положение окна
                var settings = this.settingsManager.LoadSettings();

                settings.top = calculatorWindow.Top;
                settings.left = calculatorWindow.Left;
                settings.width = calculatorWindow.Width;
                settings.height = calculatorWindow.Height;
                this.settingsManager.SaveSettings(settings);

                calculatorWindow.Close();
            }
            return;
        }

        public BitmapImage? getTaskbarIcon()
        {
            return null;
        }
        public string? getTaskbarIconText()
        {
            return null;
        }

        public Notification? popNotification()
        {
            return null;
        }

        public void openSettings()
        {
        }

    }
}
