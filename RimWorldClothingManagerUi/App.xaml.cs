using Prism.Ioc;
using RimWorldClothingManagerUi.Views;
using System.Windows;

namespace RimWorldClothingManagerUi
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }
    }
}
