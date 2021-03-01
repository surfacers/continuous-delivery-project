using System.Windows;
using Unity;

namespace Hurace.RaceControl
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IUnityContainer Container = StartupConfig.ConfigureDependencies();
    }
}
