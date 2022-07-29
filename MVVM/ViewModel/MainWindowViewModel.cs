using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;
using Newtonsoft.Json;
using PPF.WPF.MVVM.Model;
using PPF.WPF.MVVM.ViewModel;
using SciChart.Charting.Model.DataSeries;


namespace PPF.WPF
{
    public class MainWindowViewModel : ObservableObject
    {
        private string _sampleName;
        public string SampleName
        {
            get => _sampleName;
            set
            {
                _sampleName = value;
                OnPropertyChanged("SampleName");
            }
        }

        private bool _workingWithFile;
        public bool WorkingWithFile
        {
            get => _workingWithFile;
            set
            {
                _workingWithFile = value;
                OnPropertyChanged("WorkingWithFile");
            }
        }

        public string filename;

        private ICommand _changePageCommand;

        private IProcessViewModel _currentProcessViewModel;
        private ObservableCollection<IProcessViewModel> _processViewModels;

        public SputterViewModel Sputter;
        public TransitionViewModel Transition;
        public SpectrometryViewModel Spectrometry;
        public InterferometryViewModel Interferometry;
        
        /// <summary>
        /// Basic Main Window for new projects
        /// </summary>
        public MainWindowViewModel()
        {
            // TODO : instance could be not null, need to check for any differences
            if (Instance != null)
            {
                ProcessViewModels = Instance.ProcessViewModels;
                Instance = this;
                return;
            }

            Sputter = new SputterViewModel();
            Transition = new TransitionViewModel();
            Spectrometry = new SpectrometryViewModel();
            Interferometry = new InterferometryViewModel();
            ProcessViewModels = new ObservableCollection<IProcessViewModel>() { Sputter, Transition, Spectrometry, Interferometry };
            Instance = this;
        }

        public MainWindowViewModel(MainWindowViewModel newFile)
        {
            var models = new ObservableCollection<IProcessViewModel>();
            
            foreach (var model in newFile.ProcessViewModels)
            {
                models.Add(model);
            }
            ProcessViewModels = models;
            CurrentProcessViewModel = ProcessViewModels[0];
            Instance = this;
        }

        public static MainWindowViewModel Instance { get; set; }

        public ICommand ChangePageCommand
        {
            get
            {
                if (_changePageCommand == null)
                {
                    _changePageCommand = new RelayCommand(
                        p => ChangeViewModel((IProcessViewModel)p),
                        p => p is IProcessViewModel);
                }
                return _changePageCommand;
            }
        }

        public ObservableCollection<IProcessViewModel> ProcessViewModels
        {
            get
            {
                if (_processViewModels == null)
                {
                    _processViewModels = new ObservableCollection<IProcessViewModel>();
                }
                return _processViewModels;
            }
            set
            {
                _processViewModels = value;
                OnPropertyChanged("ProcessViewModels");
            }
        }

        public IProcessViewModel CurrentProcessViewModel
        {
            get
            {
                return _currentProcessViewModel;
            }
            set
            {
                if (_currentProcessViewModel != value)
                {
                    _currentProcessViewModel = value;
                    OnPropertyChanged("CurrentProcessViewModel");
                }
            }
        }

        private void ChangeViewModel(IProcessViewModel viewModel)
        {
            if (!ProcessViewModels.Contains(viewModel))
                ProcessViewModels.Add(viewModel);

            CurrentProcessViewModel = ProcessViewModels
                .FirstOrDefault(vm => vm == viewModel);
        }

        /// <summary>
        /// TODO : This does not work
        /// </summary>
        public static void New()
        {
            //Instance.ProcessViewModels.Clear();
            Instance = new MainWindowViewModel();
        }
        
        /// <summary>
        /// TODO: fix the opening of xydataseries here: <see cref="Spectrometry"/>, <see cref="Interferometry"/>
        /// </summary>
        public static void Open()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.DefaultExt = ".vo2";
            dlg.Filter = "VO2 Files (.vo2)|*.vo2";
            if (dlg.ShowDialog() == true)
            {
                Instance.ProcessViewModels.Clear();
                var newObject = JsonConvert.DeserializeObject<MainWindowViewModel>(File.ReadAllText(dlg.FileName), new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Objects,
                    NullValueHandling = NullValueHandling.Ignore
                }
                );

                //var tempSpec = new SpectrometryViewModel();

                foreach (var chart in newObject.Spectrometry.Charts)
                {
                    chart.ChartDataSeries = new XyDataSeries<double, double>();
                    var dataSerializable = chart.ChartDataSerializable;
                    foreach (var data in dataSerializable)
                    {
                        chart.ChartDataSeries.XValues.Add(data.Key);
                        chart.ChartDataSeries.YValues.Add(data.Value);
                    }
                }
                
                foreach (var chart in newObject.Interferometry.Charts)
                {
                    chart.ChartDataSeries = new XyDataSeries<double, double>();
                    var dataSerializable = chart.ChartDataSerializable;
                    foreach (var data in dataSerializable)
                    {
                        chart.ChartDataSeries.XValues.Add(data.Key);
                        chart.ChartDataSeries.YValues.Add(data.Value);
                    }
                }

                newObject.ProcessViewModels[2] = newObject.Spectrometry;
                newObject.ProcessViewModels[3] = newObject.Interferometry;
                Instance.ProcessViewModels = newObject.ProcessViewModels;
                
                

                // Finally, should designate that we are working with a file already saved
                // Save function then automatically will overwrite file
                Instance.WorkingWithFile = true;
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

            //if (Instance.WorkingWithFile && Instance.filename != null)
            if (Instance.filename != null)
            {
                File.WriteAllText(Instance.filename, jsonString);
            }
            else
            {
                SaveAs();
            }
        }

        public void SaveAs()
        {
            var jsonString = JsonConvert.SerializeObject(Instance, typeof(MainWindowViewModel), new JsonSerializerSettings()
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
                File.WriteAllText(Instance.filename, jsonString);
                MainWindowViewModel.Instance.WorkingWithFile = true;
            }
        }
    }
}
