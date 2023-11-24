using ProcessList.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace ProcessList.ViewModel
{
    public class ProcessViewModel : ViewModelBase
    {
        private DispatcherTimer _dispatcherTimer;
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

        public ICommand RefreshCommand { get; set; }

        public ProcessViewModel()
        {
            Processes = new ObservableCollection<ProcessModel>();
            IsProgressBarIndeterminate = false;
            ProgressBarVisibility = Visibility.Collapsed;
            Interval = 10;
            RefreshCommand = new RelayCommand(Refresh);
            _dispatcherTimer = new DispatcherTimer();
            _dispatcherTimer.Tick += (sender, e) => Refresh(default!);
            Refresh(default!);
        }

        private void LoadProcesses(object sender, DoWorkEventArgs e)
        {
            List<ProcessModel> processes = new List<ProcessModel>();

            Parallel.ForEach(Process.GetProcesses(), process =>
            {
                processes.Add(new ProcessModel(process));
            });

            Processes = new ObservableCollection<ProcessModel>(processes);
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
    }
}
