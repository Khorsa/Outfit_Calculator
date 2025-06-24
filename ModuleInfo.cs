using OutfitTool.Common;

namespace Calculator
{
    internal class ModuleInfo : ModuleInfoInterface
    {
        public string Name => "calculator";
        public string AssemblyName => "Calculator";
        public string DisplayName => "Calculator";
        public string Description => "Калькулятор";
        public ModuleVersion Version => new ModuleVersion(0, 2, "pre-alpha");
        public ModuleVersion Require => new ModuleVersion(3, 0);
        public string Changes => "Первая версия";
        public string Author => "Stolyarov Roman";
        public string AuthorContacts => "rshome@mail.ru";
    }
}
