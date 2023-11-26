using ProcessList.Model;
using ProcessList.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace ProcessList.ViewModel
{
    public class ProcessViewModel : ViewModelBase
    {
        private DispatcherTimer _dispatcherTimer;
        private List<ProcessModel> _allProcesses;
        private ObservableCollection<ProcessModel> _processes;
        public ObservableCollection<ProcessModel> Processes
        {
            get => _processes;
            set
            {
                _processes = value;
                OnPropertyChanged();
            }
        }

        private bool _isProgressBarIndeterminate;
        public bool IsProgressBarIndeterminate
        {
            get => _isProgressBarIndeterminate;
            set
            {
                if (_isProgressBarIndeterminate != value)
                {
                    _isProgressBarIndeterminate = value;
                    OnPropertyChanged();
                }
            }
        }

        private Visibility _progressBarVisibility;
        public Visibility ProgressBarVisibility
        {
            get => _progressBarVisibility;
            set
            {
                if (_progressBarVisibility != value)
                {
                    _progressBarVisibility = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _interval;
        public int Interval
        {
            get => _interval;
            set
            {
                _interval = value;
                OnPropertyChanged();
            }
        }

        private bool _autoRefreshChecked;
        public bool AutoRefreshChecked
        {
            get => _autoRefreshChecked;
            set
            {
                _autoRefreshChecked = value;
                OnPropertyChanged();
                AutoRefresh();
            }
        }

        private string _filterValue;
        public string FilterValue
        {
            get => _filterValue;
            set
            {
                _filterValue = value;
                OnPropertyChanged();
                FilterProcesses(default!);
            }
        }

        private ProcessPriorityClass _selectedPriority;
        public ProcessPriorityClass SelectedPriority
        {
            get => _selectedPriority;
            set
            {
                _selectedPriority = value;
                OnPropertyChanged();
            }
        }

        private ProcessModel _selectedProcess;
        public ProcessModel SelectedProcess
        {
            get => _selectedProcess;
            set
            {
                _selectedProcess = value;
                GetProcessDetails();
                OnPropertyChanged();
            }
        }

        private Dictionary<string, object?> _selectedProcessDetails;
        public Dictionary<string, object?> SelectedProcessDetails
        {
            get => _selectedProcessDetails;
            set
            {
                _selectedProcessDetails = value;
                OnPropertyChanged();
            }
        }

        private Dictionary<string, List<string>?> _selectedProcessModules;
        public Dictionary<string, List<string>?> SelectedProcessModules
        {
            get => _selectedProcessModules;
            set
            {
                _selectedProcessModules = value;
                OnPropertyChanged();
            }
        }

        public List<ProcessPriorityClass> Priorities { get; set; }

        public ICommand RefreshCommand { get; set; }
        public ICommand KillSelectedProcessCommand {  get; set; }
        public ICommand SetPriorityCommand { get; set; }

        public ProcessViewModel()
        {
            Processes = new ObservableCollection<ProcessModel>();
            IsProgressBarIndeterminate = false;
            ProgressBarVisibility = Visibility.Collapsed;
            Interval = 15;
            RefreshCommand = new RelayCommand(Refresh);
            KillSelectedProcessCommand = new RelayCommand(KillSelectedProcess);
            SetPriorityCommand = new RelayCommand(SetPriority);
            _dispatcherTimer = new DispatcherTimer();
            _dispatcherTimer.Tick += (sender, e) => Refresh(default!);
            Priorities = new List<ProcessPriorityClass>(
                (IEnumerable<ProcessPriorityClass>)Enum.GetValues(typeof(ProcessPriorityClass)));
            SelectedPriority = Priorities[0];
            Refresh(default!);
        }

        private void LoadProcesses(object sender, DoWorkEventArgs e)
        {
            _allProcesses = new List<ProcessModel>();

            Parallel.ForEach(Process.GetProcesses(), process =>
            {
                _allProcesses.Add(new ProcessModel(process));
            });

            FilterValue = default!;
            UpdateProcesses(_allProcesses);
        }

        private void UpdateProcesses(List<ProcessModel> updatedProcesses)
        {
            Processes = new ObservableCollection<ProcessModel>(updatedProcesses);
            SelectedProcess = default!;
            SelectedProcessDetails = default!;
            SelectedProcessModules = default!;
        }

        private void Refresh(object obj)
        {
            IsProgressBarIndeterminate = true;
            ProgressBarVisibility = Visibility.Visible;

            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += LoadProcesses;

            worker.RunWorkerCompleted += (sender, e) =>
            {
                IsProgressBarIndeterminate = false;
                ProgressBarVisibility = Visibility.Collapsed;
            };

            worker.RunWorkerAsync();
        }

        private void AutoRefresh()
        {
            if (AutoRefreshChecked)
            {
                _dispatcherTimer.Interval = TimeSpan.FromSeconds(Interval);
                _dispatcherTimer.Start();
            }
            else
            {
                _dispatcherTimer.Stop();
            }
        }

        private void KillSelectedProcess(object obj)
        {
            try
            {
                var selectedProcessName = SelectedProcess.Name;
                SelectedProcess.ProcessObject.Kill();
                _allProcesses.Remove(SelectedProcess);
                Processes.Remove(SelectedProcess);
                MessageBox.Show($"Process {selectedProcessName} killed successfully.",
                    "Information",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (Exception)
            {
                MessageBox.Show("Can't kill selected process.",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void SetPriority(object obj)
        {
            try
            {
                SelectedProcess.ProcessObject.PriorityClass = SelectedPriority;
                var updatedProcess = new ProcessModel(SelectedProcess.ProcessObject);
                int index = Processes.IndexOf(SelectedProcess);
                Processes[index] = updatedProcess;
                _allProcesses[index] = updatedProcess;
                SelectedProcess = updatedProcess;

                MessageBox.Show($"Successfully changed priority of process {SelectedProcess.Name}.",
                    "Information",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (Exception)
            {
                MessageBox.Show("Can't change priority of selected process.",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void FilterProcesses(object obj)
        {
            if (FilterValue != null)
            {
                List<ProcessModel> filteredProcesses = new List<ProcessModel>(_allProcesses);
                foreach (ProcessModel process in filteredProcesses.ToList())
                {
                    if (!process.Name!.ToLower().Contains(FilterValue.ToLower()))
                        filteredProcesses.Remove(process);
                }

                UpdateProcesses(filteredProcesses);
            }
        }

        private void GetProcessDetails()
        {
            if (SelectedProcess != null)
            {
                SelectedProcessDetails = new Dictionary<string, object?>
                {
                    {"Name", SelectedProcess.Name },
                    {"Id", SelectedProcess.Id },
                    {"Priority", SelectedProcess.Priority },
                    {"Threads number", SelectedProcess.ThreadsNumber },
                    {"Memory usage",
                     SelectedProcess.PhysicalMemoryUsage != null ? SelectedProcess.PhysicalMemoryUsage.ToString()  + " MB" : "N/A" },
                    {"Total processor time", 
                     SelectedProcess.TotalProcessorTimeMinutes != null ? SelectedProcess.TotalProcessorTimeMinutes.ToString() + " min" : "N/A" },
                    {"CPU Usage", SelectedProcess.CpuUsage }
                };

                SelectedProcessModules = new Dictionary<string, List<string>?>
                {
                    { "Modules", ProcessUtils.GetProcessModules(SelectedProcess.ProcessObject) }
                };
            }
        }
    }
}
