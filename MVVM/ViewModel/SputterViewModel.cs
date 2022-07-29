using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PPF.WPF.MVVM.ViewModel
{
    public class SputterViewModel : ObservableObject, IProcessViewModel
    {
        private uint imageCount = 0;
        public string DisplayName { get; set; }
        private string _notes;
        public string Notes { get => _notes; set { _notes = value; OnPropertyChanged("Notes"); } }
        public bool CanGoLeft { get; set; }

        public string LeftButtonCursor { get; set; }

        public SputterViewModel()
        {
            DisplayName = "Sputter";
            Notes = "Add Notes";
            LeftButtonCursor = "Hand";
        }

        public void AddImage()
        {
            if (imageCount >= 2)
            {
                CanGoLeft = true;
            }
        }
    }
}
