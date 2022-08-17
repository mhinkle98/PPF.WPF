using System.Windows;
using System.Windows.Input;

namespace PPF.WPF.MVVM.View
{
    /// <summary>
    /// Interaction logic for RenameWindow.xaml
    /// </summary>
    public partial class RenameWindow : Window
    {
        public bool DidGetResult;
        
        public string ChartName;
        public RenameWindow(string defaultName = "NewChart")
        {
            InitializeComponent();
            ChartName = defaultName;
            ChartInput.Text = ChartName;
            DidGetResult = false;
        }


        private void OKButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (ChartInput.Text != "")
            {
                ChartName = ChartInput.Text;
            }

            DidGetResult = true;
            Close();
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ChartInput_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;
            if (ChartInput.Text != "")
            {
                ChartName = ChartInput.Text;
            }

            DidGetResult = true;
            Close();
        }
    }
}
