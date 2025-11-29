using OutfitTool.API1;
using OutfitTool.API1.Dto;

namespace Calculator;

internal class ModuleInfo : IModuleInfo
{
    public string Name => "calculator";
    public string AssemblyName => "Calculator";
    public string DisplayName => "Calculator";
    public string Description => "Калькулятор";
    public ModuleVersion Version => new (0, 1, "alpha");
    public string Changes => "Тестовая версия";
    public string Author => "Stolyarov Roman";
    public string AuthorContacts => "rshome@mail.ru";
    public int RequireApiVersion => 1;
}