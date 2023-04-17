using System.Diagnostics;
using System.Reflection.Metadata;
using System.Windows.Input;
using TaskBuddyClassLibrary.Models;
using TaskBuddyClassLibrary.Services;

namespace TaskBuddyAndroidApp
{
    
    public partial class MainPage : ContentPage
    {
        public List<TaskBuddyTask> Tasks { get; set; }

        private TaskService _taskService;

        public MainPage()
        {
            InitializeComponent();
            Tasks = new List<TaskBuddyTask>();
            listView.ItemsSource = Tasks;
        }

        private async void OnRefreshClicked(object sender, EventArgs e)
        {

            SemanticScreenReader.Announce(RefreshBtn.Text);
            Tasks = await _taskService.GetTasksAsync();
            
            listView.ItemsSource = Tasks;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            var serverAddress = Preferences.Get("serverAddress", "");
            var serverPort = Preferences.Get("serverPort", 0);
            if (string.IsNullOrEmpty(serverAddress) || serverPort == 0)
            {
                var page = new ServerPortPage();
                await Navigation.PushAsync(page);
                await page.WaitUntilSavedAsync();
                _taskService = new TaskService(page.ServerAddress, page.ServerPort);
                Preferences.Set("serverAddress", page.ServerAddress);
                Preferences.Set("serverPort", page.ServerPort);
            }
            else
            {
                _taskService = new TaskService(serverAddress, serverPort);
            }

            Tasks = await _taskService.GetTasksAsync();
            listView.ItemsSource = Tasks;
        }


        private async void OnSettingClicked(object sender, EventArgs e)
        {
            var page = new ServerPortPage(
                Preferences.Get("serverAddress", ""),
                Preferences.Get("serverPort", 0)
            );
            await Navigation.PushAsync(page);
            await page.WaitUntilSavedAsync();
            _taskService = new TaskService(page.ServerAddress, page.ServerPort);
            Preferences.Set("serverAddress", page.ServerAddress);
            Preferences.Set("serverPort", page.ServerPort);
            Tasks = await _taskService.GetTasksAsync();
        }

        private async void OnTaskSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var task = e.SelectedItem as TaskBuddyTask;
            if (task == null)
                return;

            var page = new TaskEditPage(task);
            await Navigation.PushAsync(page);

            listView.SelectedItem = null;
        }
        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
            {
                return;
            }

            TaskBuddyTask task = (TaskBuddyTask)e.SelectedItem;

            // Öffne eine neue Seite, um die Details der Aufgabe anzuzeigen
            await Navigation.PushAsync(new TaskEditPage(task));

            // Setze die Auswahl auf null, damit der gleiche Eintrag erneut ausgewählt werden kann
            listView.SelectedItem = null;
        }

        private async void OnAddTaskClicked(object sender, EventArgs e)
        {
            var task = new TaskBuddyTask();
            var page = new TaskEditPage(task, true);
            await Navigation.PushAsync(page);
        }
        
        private async void OnCheckBoxChanged(object sender, CheckedChangedEventArgs e)
        {
            if (!(sender is CheckBox toggledCheckbox))
            {
                return;
            }

            if (!(toggledCheckbox.BindingContext is TaskBuddyTask task))
            {
                return;
            }

            task.IsComplete = e.Value;

            await _taskService.PutTaskAsync(task);

            // Deselect the item
            listView.SelectedItem = null;
        }
        public Command DeleteCommand => new Command(async (parameter) =>
        {
            try
            {
                var task = parameter as TaskBuddyTask;
                await _taskService.DeleteTaskAsync(task);
                if (task != null)
                {
                    await _taskService.DeleteTaskAsync(task);
                    Tasks = await _taskService.GetTasksAsync();
                    listView.ItemsSource = Tasks;
                }
            }
            catch (Exception ex) {
                Debug.WriteLine(ex.Message);
                // Oder eine benutzerdefinierte Fehlermeldung anzeigen
                await Application.Current.MainPage.DisplayAlert("Error", "An error occurred: " + ex.Message, "OK");
            }
        });
        public ICommand DeleteTaskCommand => new Command<TaskBuddyTask>(async (task) =>
        {
            bool answer = await DisplayAlert("Confirmation", "Are you sure you want to delete this task?", "Yes", "No");

            if (answer)
            {
                await _taskService.DeleteTaskAsync(task);
                Tasks.Remove(task);
            }
        });



    }
}