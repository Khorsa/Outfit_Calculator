using OutfitTool.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows;

namespace Calculator.Commands
{
    class ToggleCommand: CommandInterface
    {
        public string Name => "ShowHide";
        public string Description => LocalizationHelper.GetString("ShowHide");
        public CommandContextMenu? ContextMenu => getContextMenu();

        public CommandContextMenu? getContextMenu()
        {
            string resourceName = "calculator.png";
            return new CommandContextMenu(
                LocalizationHelper.GetString("Calculator"),
                new BitmapImage(
                    new Uri("pack://application:,,,/" + Assembly.GetExecutingAssembly().GetName().Name + ";component/Resources/" + resourceName)
                    )
                );
        }

        public void run()
        {
            if (ModuleController.calculatorWindow == null)
            {
                return;
            }

            if (ModuleController.calculatorWindow.Visibility == Visibility.Visible)
            {
                ModuleController.calculatorWindow.Hide();
            }
            else
            {
                ModuleController.calculatorWindow.Topmost = true;
                ModuleController.calculatorWindow.Show();
                ModuleController.calculatorWindow.Activate();
                ModuleController.calculatorWindow.Focus();
            }
        }
    }
}
