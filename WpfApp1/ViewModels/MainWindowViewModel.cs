using ChatAppService.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TaskManager.Models;
using Whatsapp.Commands;

namespace TaskManager.ViewModels
{
    public class MainWindowViewModel : ServiceINotifyPropertyChanged
    {
        private ObservableCollection<ProcessModel> processes1;

        public ObservableCollection<ProcessModel> processes { get => processes1; set { processes1 = value; OnPropertyChanged(); } }

        public static List<string>? _blackList { get; set; }
        public ICommand CreateBtnCommand { get; set; }
        public ICommand EndBtnCommand { get; set; }
        public ICommand AddBtnCommand { get; set; }
        public ICommand SearchBtnCommand { get; set; }
        public MainWindowViewModel()
        {
            _blackList = new();
            processes = new ObservableCollection<ProcessModel>();
            FillDatas();
            CreateBtnCommand = new Command(ExecuteCreateBtnCommand, CanExecuteCreateBtnCommand);
            EndBtnCommand = new Command(ExecuteEndBtnCommand, CanExecuteEndBtnCommand);
            AddBtnCommand = new Command(ExecuteAddBtnCommand, CanExecuteAddBtnCommand);
            SearchBtnCommand = new Command(ExecuteSearchBtnCommand, CanExecuteSearchBtnCommand);
        }

        private bool CanExecuteSearchBtnCommand(object obj) =>
                !string.IsNullOrEmpty(((TextBox)(((Grid)obj)?.FindName("txtbox")))?.Text);

        private void ExecuteSearchBtnCommand(object obj)
        {
            if (((Button)(((Grid)obj)?.FindName("searchBtn")))?.Content == "All Process")
            {
                ((Button)(((Grid)obj).FindName("searchBtn"))).Content = "search";
                FillDatas(); return;

            }
            var pro = processes.FirstOrDefault(p => p.Name.ToLower().Contains(((TextBox)((Grid)obj)?.FindName("txtbox")).Text.ToLower()));
            if (pro is null)
            {
                MessageBox.Show("is not exists");
                return;
            }
            processes.Clear();
            processes.Add(pro);
            ((Button)(((Grid)obj).FindName("searchBtn"))).Content = "All Process";

        }

        private bool CanExecuteAddBtnCommand(object obj) =>
                !string.IsNullOrEmpty(((TextBox)obj).Text);
        private void ExecuteAddBtnCommand(object obj) =>
            _blackList?.Add(obj?.ToString()?.ToLower()!);

        private bool CanExecuteEndBtnCommand(object obj) =>
                obj is not null;


        private void ExecuteEndBtnCommand(object obj)
        {
            Process.GetProcessById((obj as ProcessModel)!.Id).Kill();
            FillDatas();
        }

        private bool CanExecuteCreateBtnCommand(object obj) =>
                !string.IsNullOrEmpty(((TextBox)obj).Text);
        private void ExecuteCreateBtnCommand(object obj)
        {
            if (_blackList!.Contains(obj?.ToString()?.ToLower()!))
            {
                MessageBox.Show("the process added Balck List");
                return;
            }
            try
            {
                Process.Start(((TextBox)obj).Text);
                FillDatas();
            }
            catch (Exception ex)
            {
                MessageBox.Show("not found");
            }
        }

        private void FillDatas()
        {
            processes.Clear();
            var allProcesses = Process.GetProcesses();
            foreach (var item in allProcesses)
                processes.Add(new ProcessModel()
                {
                    Id = item.Id,
                    Name = item.ProcessName,
                    CountHandle = item.HandleCount,
                    ThreadHandle = item.Threads.Count,
                    SessionId = item.SessionId
                }); ;
        }
    }
}
