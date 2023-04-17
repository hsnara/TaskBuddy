namespace TaskBuddyAndroidApp;

public partial class ServerPortPage : ContentPage
{
    public string ServerAddress => serverAddressEntry.Text;

    public int ServerPort => int.TryParse(serverPortEntry.Text, out var result) ? result : 0;

    private readonly TaskCompletionSource<bool> _savedTcs = new TaskCompletionSource<bool>();
    public ServerPortPage(string server = default, int port = 0)
	{
		InitializeComponent();
        serverAddressEntry.Text = server;
        serverPortEntry.Text = port.ToString();
        BindingContext = this;
    }
    public async Task WaitUntilSavedAsync()
    {
        await _savedTcs.Task;
    }

    private void OnSaveClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(ServerAddress) || ServerPort == 0)
        {
            DisplayAlert("Error", "Please enter server address and port.", "OK");
            return;
        }

        if (ServerPort <= 0 || ServerPort > 65535)
        {
            DisplayAlert("Error", "Invalid port number.", "OK");
            return;
        }

        _savedTcs.SetResult(true);
        Navigation.PopAsync();
    }
}