using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPF.WPF.MVVM.ViewModel
{
    public class TransitionViewModel : ObservableObject, IProcessViewModel
    {
        public string DisplayName { get; set; }
        public bool Yes { get; set; }
        public bool No { get; set; }
        public bool Uncertain { get; set; }

        public TransitionViewModel()
        {
            DisplayName = "Transition";
            Yes = false;
            No = false;
            Uncertain = false;
        }
    }
}
