using System.Windows;
using SciChart.Charting.Visuals;

namespace PPF.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            //base.OnStartup(e);
            // Set this code once in App.xaml.cs or application startup
            SciChartSurface.SetRuntimeLicenseKey("3lEheOP4Yskl2G7tutQdU5JapEWQiR0T25R3QQE8MGH9hlZPcJJgyJPS9pORtOiZ0GHWcgTmGxxVPj+UeUQIJpr61a3wWf4/w6u9JxrAE8i/ePKggF8PjQQ+mX+JoIb0q4R7Dw5LnIJKI3QgFcUOSrOUVz0ntw9idvoXHO5sb460ookpf8abnutqQrV4xh1agExdpGSclWah8M1WdvlfxCdHH6C9sfTWA1Jlm8KAeZz6CvsgfAPh1ozwq+Mqg5tNDrJWKl0AjCpzLIMzJDLNC9jx7104k/3ojQKTDgrGLhihAsbAiz51gM3aW5j7oaHwk5MC3ikVtyDjRB+IfZM4vtglnnItnBt4v9e7dNqTXRbftmEGe812L+kCQdbYnUaGqOLGZUWfeOaZvSK7y6QH9qWT5T2m09LqStWGyt5Z3NHfxl2xNLKipxUZ6/dlVvtPn/9ABA/tv4Iyp3OEEwi1uky6tA/Qx9DKdnULEeVlY13q/VB1Eci1ka++ebixDA==");
            MainWindow app = new MainWindow();
            MainWindowViewModel context = new MainWindowViewModel();
            app.DataContext = context;
            app.Show();
        }
    }
}
