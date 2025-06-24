using Calculator.Commands;
using Calculator.Settings;
using OutfitTool.Common;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using YamlDotNet.Serialization;

namespace Calculator
{
    internal class ModuleController : ModuleControllerInterface
    {
        public static CalculatorForm? calculatorWindow = null;

        public ModuleController() 
        {
        }

        public List<CommandInterface> getCommandList()
        {
            return new List<CommandInterface>([
                new ToggleCommand()
            ]);
        }

        public void init()
        {
            calculatorWindow = new CalculatorForm();
        }

        public void setLanguage(string language)
        {
            LocalizationHelper.Language = new CultureInfo(language);
        }

        public void shutdown()
        {
            if (calculatorWindow != null)
            {
                calculatorWindow.SaveWindowSettings();
                calculatorWindow.Close(); 
            }
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
