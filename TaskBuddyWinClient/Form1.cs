using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using TaskBuddyClassLibrary.Models;
using TaskBuddyClassLibrary.Services;
using TaskBuddyClassLibrary.Exceptions;

namespace TaskBuddyWinClient
{
    public partial class Form1 : Form
    {

        private TaskService _taskService = new TaskService();

        public class Config
        {
            public string ServerAddress { get; set; }
            public int ServerPort { get; set; }
            public List<TaskBuddyTask> Tasks { get; set; }
        }

        public Form1()
        {
            InitializeComponent();
            lblServerStatus.Text = "Server: Offline";

            // Abrufen und Ausgeben der Aufgabenliste
            if (File.Exists("config.json"))
            {
                var json = File.ReadAllText("config.json");
                var config = JsonConvert.DeserializeObject<Config>(json);
                if (config != null && config.Tasks is not null)
                {
                    SetServerStatusLabel(config.ServerAddress + ":" + config.ServerPort);
                    UpdateTaskList(config.Tasks);
                    _taskService = new TaskService(config.ServerAddress, config.ServerPort);
                }
            }
            else
            {
                ServerPortForm serverPortForm = new ServerPortForm();
                if (serverPortForm.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        _taskService.ServerPort = serverPortForm.ServerPort;
                        _taskService.ServerAddress = serverPortForm.ServerAddress;
                        SetServerStatusLabel(_taskService.ServerAddress + ":" + _taskService.ServerPort);
                        var tasks = _taskService.GetTasks();

                        var config = new Config
                        {
                            ServerAddress = _taskService.ServerAddress,
                            ServerPort = _taskService.ServerPort,
                            Tasks = tasks
                        };
                        var json = JsonConvert.SerializeObject(config);
                        File.WriteAllText("config.json", json);

                        if (tasks is null)
                        {
                            lblServerStatus.Text = "Failed to connect";
                        }
                        else
                        {
                            UpdateTaskList(tasks);
                        }

                        refreshList();
                    }
                    catch (TaskBuddyException ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    

                    
                }



            }
        }

        private void InitializeComponent()
        {
            checkedListBox1 = new CheckedListBox();
            btnSettings = new Button();
            lblServerStatus = new Label();
            btnRefresh = new Button();
            btnEdit = new Button();
            btnNewTask = new Button();
            btnDelete = new Button();
            SuspendLayout();
            // 
            // checkedListBox1
            // 
            checkedListBox1.FormattingEnabled = true;
            checkedListBox1.Location = new Point(12, 6);
            checkedListBox1.Name = "checkedListBox1";
            checkedListBox1.Size = new Size(281, 130);
            checkedListBox1.TabIndex = 0;
            checkedListBox1.ItemCheck += CheckedListBox1_ItemCheck;
            // 
            // btnSettings
            // 
            btnSettings.Location = new Point(12, 142);
            btnSettings.Name = "btnSettings";
            btnSettings.Size = new Size(75, 23);
            btnSettings.TabIndex = 1;
            btnSettings.Text = "Settings";
            btnSettings.UseVisualStyleBackColor = true;
            btnSettings.Click += btnSettings_Click;
            // 
            // lblServerStatus
            // 
            lblServerStatus.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lblServerStatus.Location = new Point(12, 211);
            lblServerStatus.Name = "lblServerStatus";
            lblServerStatus.Size = new Size(281, 23);
            lblServerStatus.TabIndex = 2;
            lblServerStatus.Text = "Server: Offline";
            // 
            // btnRefresh
            // 
            btnRefresh.Location = new Point(218, 180);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(75, 23);
            btnRefresh.TabIndex = 3;
            btnRefresh.Text = "Refresh";
            btnRefresh.UseVisualStyleBackColor = true;
            btnRefresh.Click += btnRefresh_Click;
            // 
            // btnEdit
            // 
            btnEdit.Location = new Point(115, 142);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new Size(75, 23);
            btnEdit.TabIndex = 4;
            btnEdit.Text = "Edit";
            btnEdit.UseVisualStyleBackColor = true;
            btnEdit.Click += btnEdit_Click;
            // 
            // btnNewTask
            // 
            btnNewTask.Location = new Point(218, 142);
            btnNewTask.Name = "btnNewTask";
            btnNewTask.Size = new Size(75, 23);
            btnNewTask.TabIndex = 5;
            btnNewTask.Text = "New Task";
            btnNewTask.UseVisualStyleBackColor = true;
            btnNewTask.Click += btnNewTask_Click;
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(115, 180);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(75, 23);
            btnDelete.TabIndex = 6;
            btnDelete.Text = "Delete";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(305, 243);
            Controls.Add(btnDelete);
            Controls.Add(btnNewTask);
            Controls.Add(btnEdit);
            Controls.Add(btnRefresh);
            Controls.Add(lblServerStatus);
            Controls.Add(btnSettings);
            Controls.Add(checkedListBox1);
            Name = "Form1";
            Text = "TaskBuddy";
            ResumeLayout(false);
        }

        private async void btnEdit_Click(object sender, EventArgs e)
        {
            if (checkedListBox1.SelectedItem is TaskBuddyTask task)
            {
                var taskDescriptionForm = new TaskDescriptionForm(task);
                if (taskDescriptionForm.ShowDialog() == DialogResult.OK)
                {
                    task.Title = taskDescriptionForm.Title;
                    task.Description = taskDescriptionForm.Description;
                    var result = await _taskService.PutTaskAsync(task);
                    if (result)
                    {
                        MessageBox.Show("Task successfully changed.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("An error occurred.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    refreshList();
                }
            }
            else
            {
                MessageBox.Show("Please select a task.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void CheckedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (checkedListBox1.SelectedItem is TaskBuddyTask task)
            {
                if (e.NewValue == CheckState.Checked)
                {
                    task.IsComplete = true;
                    var result = _taskService.PutTask(task);
                    if (result)
                    {
                        SetServerStatusLabel(task.Title + " marked as done!");
                    }
                    else
                    {
                        MessageBox.Show("An error occurred.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else if (e.NewValue == CheckState.Unchecked)
                {
                    task.IsComplete = false;
                    var result = await _taskService.PutTaskAsync(task);
                    if (result)
                    {
                        SetServerStatusLabel(task.Title + " marked as incomplete!");
                    }
                    else
                    {
                        MessageBox.Show("An error occurred.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                refreshList();
            }
        }


        private void btnSettings_Click(object sender, EventArgs e)
        {
            var serverPortForm = new ServerPortForm();

            if (serverPortForm.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    _taskService.ServerPort = serverPortForm.ServerPort;
                    _taskService.ServerAddress = serverPortForm.ServerAddress;
                    var tasks = _taskService.GetTasks();

                    var config = new Config
                    {
                        ServerAddress = _taskService.ServerAddress,
                        ServerPort = _taskService.ServerPort,
                        Tasks = tasks
                    };
                    var json = JsonConvert.SerializeObject(config);
                    File.WriteAllText("config.json", json);
                    SetServerStatusLabel(_taskService.ServerAddress + ":" + _taskService.ServerPort);
                    if (tasks is null)
                    {
                        SetServerStatusLabel("Failed to connect");
                    }
                    else
                    {
                        UpdateTaskList(tasks);
                    }

                }
                catch(TaskBuddyException ex) 
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                

                
            }

        }

        private void UpdateTaskList(List<TaskBuddyTask> tasks)
        {
            checkedListBox1.Items.Clear();
            foreach (TaskBuddyTask taskItem in tasks)
            {
                checkedListBox1.Items.Add(taskItem);

                if (taskItem.IsComplete)
                {
                    checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(taskItem), true);
                }
            }
        }


        private void SetServerStatusLabel(string serverName)
        {
            lblServerStatus.Text = $"Connected to: {serverName}";
        }

        private async void refreshList()
        {
            var tasks = await _taskService.GetTasksAsync();
            if (tasks is null)
            {
                lblServerStatus.Text = "Failed to refresh";
            }
            else
            {
                UpdateTaskList(tasks);
            }
        }
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            refreshList();
        }

        private async void btnNewTask_Click(object sender, EventArgs e)
        {
            TaskBuddyTask newTask = new TaskBuddyTask();

            TaskDescriptionForm taskDescriptionForm = new TaskDescriptionForm();
            if (taskDescriptionForm.ShowDialog() == DialogResult.OK)
            {
                newTask.Title = taskDescriptionForm.Title;
                newTask.Description = taskDescriptionForm.Description;
                var result = await _taskService.PostTaskAsync(newTask);
                if (result)
                {
                    SetServerStatusLabel(newTask.Title + " created successfully!");
                    MessageBox.Show("Task successfully created.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("An error occurred.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                refreshList();
            }
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (checkedListBox1.SelectedItem is TaskBuddyTask task)
            {
                var result = await _taskService.DeleteTaskAsync(task);
                if (result)
                {
                    MessageBox.Show("Task successfully removed.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("An error occurred.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                refreshList();
            }
            else
            {
                MessageBox.Show("Please select a task.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}