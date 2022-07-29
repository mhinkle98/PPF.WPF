using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Input;
using PPF.WPF.MVVM.ViewModel;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.IO;

namespace PPF.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public MainWindow(MainWindowViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }

        private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void MaximizeButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow.WindowState == WindowState.Maximized)
            {
                Application.Current.MainWindow.WindowState = WindowState.Normal;
            }
            else
            {
                Application.Current.MainWindow.WindowState = WindowState.Maximized;
            }
        }

        private void CloseButton_OnClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindowViewModel.Instance.WorkingWithFile)
            {
                MessageBoxResult msg = MessageBox.Show("Do you want to save your work?", "Save?", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
                if (msg == MessageBoxResult.Yes)
                {
                    SaveFileDialog save = new SaveFileDialog();
                    save.ShowDialog();
                }
                else if (msg == MessageBoxResult.Cancel)
                {
                    return;
                }
            }

            MainWindowViewModel.New();
            /*MainWindowViewModel.Instance.ProcessViewModels.Clear();
            MainWindowViewModel.Instance = new MainWindowViewModel();
            MainWindow newWindow = new MainWindow(MainWindowViewModel.Instance);
            Application.Current.MainWindow = newWindow;
            newWindow.Show();
            this.Close();*/
        }

        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            // preliminary code here
            if (MainWindowViewModel.Instance.WorkingWithFile)
            {
                MessageBoxResult msg = MessageBox.Show("Do you want to save your work?", "Save?", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
                if (msg == MessageBoxResult.Yes)
                {
                    SaveFileDialog save = new SaveFileDialog();
                    save.ShowDialog();
                    MainWindowViewModel.Open();
                }
                else if (msg == MessageBoxResult.Cancel)
                {
                    return;
                }
            }
            MainWindowViewModel.Open();

            
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Save();
        }

        private void SaveAsButton_Click(object sender, RoutedEventArgs e)
        {
            SaveAs();
        }

        public void Open()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.DefaultExt = ".vo2";
            dlg.Filter = "VO2 Files (.vo2)|*.vo2";
            if (dlg.ShowDialog() == true)
            {
                MainWindowViewModel.Instance.ProcessViewModels.Clear();
                var newObject = JsonConvert.DeserializeObject<MainWindowViewModel>(File.ReadAllText(dlg.FileName), new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Objects,
                    NullValueHandling = NullValueHandling.Ignore
                }
                );

                MainWindowViewModel.Instance = new MainWindowViewModel();
                MainWindowViewModel.Instance.CurrentProcessViewModel = MainWindowViewModel.Instance.ProcessViewModels[0];
                /*MainWindow newWindow = new MainWindow();
                Application.Current.MainWindow = newWindow;
                newWindow.Show();
                // Finally, should designate that we are working with a file already saved
                // Save function then automatically will overwrite file
                MainWindowViewModel.Instance.WorkingWithFile = true;
                this.Close();*/
            }
        }

        public void Save()
        {
            var jsonString = JsonConvert.SerializeObject(MainWindowViewModel.Instance, typeof(MainWindowViewModel), new JsonSerializerSettings()
            {

                PreserveReferencesHandling = PreserveReferencesHandling.All,
                NullValueHandling = NullValueHandling.Ignore,
                TypeNameHandling = TypeNameHandling.Objects
            });

            if (MainWindowViewModel.Instance.WorkingWithFile && MainWindowViewModel.Instance.filename != null)
            {
                File.WriteAllText(MainWindowViewModel.Instance.filename, jsonString);
            }
            else
            {
                SaveAs();
            }
        }

        public void SaveAs()
        {
            var jsonString = JsonConvert.SerializeObject(MainWindowViewModel.Instance, typeof(MainWindowViewModel), new JsonSerializerSettings()
            {

                PreserveReferencesHandling = PreserveReferencesHandling.All,
                NullValueHandling = NullValueHandling.Ignore,
                TypeNameHandling = TypeNameHandling.Objects
            });

            SaveFileDialog dlg = new SaveFileDialog();
            dlg.DefaultExt = ".vo2";
            dlg.Filter = "VO2 Files (.vo2)|*.vo2";

            if (dlg.ShowDialog() == true)
            {
                MainWindowViewModel.Instance.filename = dlg.FileName;
                File.WriteAllText(MainWindowViewModel.Instance.filename, jsonString);
                MainWindowViewModel.Instance.WorkingWithFile = true;
            }
        }
    }
}
