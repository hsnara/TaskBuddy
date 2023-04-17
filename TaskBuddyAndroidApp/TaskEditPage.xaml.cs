using Microsoft.Maui.Controls;
using System.ComponentModel;
using System.Globalization;
using System.Threading.Tasks;
using TaskBuddyClassLibrary.Models;
using TaskBuddyClassLibrary.Services;

namespace TaskBuddyAndroidApp
{
    [DesignTimeVisible(false)]
    public partial class TaskEditPage : ContentPage
    {
        private readonly TaskService _taskService;
        private readonly TaskBuddyTask _task;
        private readonly bool IsNew;
        public string Header { get; set; }

        public TaskEditPage(TaskBuddyTask task, bool isNew=false)
        {
            InitializeComponent();
            _task = task;
            _taskService = new TaskService("192.168.0.11", 9980);
            this.IsNew = isNew;
            Header = isNew ? "New Task" : "Edit Task";
            BindingContext = this;
            DeleteBtn.IsVisible = !isNew;
        }

        public string TitleText
        {
            get => _task.Title;
            set => _task.Title = value;
        }

        public string DescriptionText
        {
            get => _task.Description;
            set => _task.Description = value;
        }

        public bool IsCompletedValue
        {
            get => _task.IsComplete;
            set => _task.IsComplete = value;
        }
        public Command SaveCommand => new Command(async () =>
        {
            if(IsNew)
            {
                await _taskService.PostTaskAsync(_task);
            }
            else
            {
                await _taskService.PutTaskAsync(_task);
            }
            await Navigation.PopAsync();
        });

        public Command CancelCommand => new Command(async () =>
        {
            await Navigation.PopAsync();
        });

        public Command DeleteTaskCommand => new Command(async () =>
        {
            await _taskService.DeleteTaskAsync(_task);
            await Navigation.PopAsync();
        });


    }
}
