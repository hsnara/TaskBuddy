using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TaskBuddyClassLibrary.Models;

namespace TaskBuddyWinClient
{
    internal class TaskDescriptionForm : Form
    {
        private int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public TaskDescriptionForm(TaskBuddyTask task)
        {
            this.Id = task.Id;
            this.Title = task.Title;
            this.Description = task.Description;
            InitializeComponent();
        }
        public TaskDescriptionForm()
        {
            InitializeComponent();
            this.Text = "New task";
        }

        private TextBox textBox1;
        private TextBox textBox2;
        private Button btnSave;
        private Button btnCancel;
        private Label label1;
        private Label label2;

        private void InitializeComponent()
        {
            label1 = new Label();
            label2 = new Label();
            textBox1 = new TextBox();
            textBox2 = new TextBox();
            btnSave = new Button();
            btnCancel = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(54, 15);
            label1.TabIndex = 2;
            label1.Text = "Task Title";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 65);
            label2.Name = "label2";
            label2.Size = new Size(95, 15);
            label2.TabIndex = 3;
            label2.Text = "Task Description:";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(12, 27);
            textBox1.Name = "textBox1";
            textBox1.Text = Title;
            textBox1.Size = new Size(260, 23);
            textBox1.TabIndex = 0;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(12, 83);
            textBox2.Multiline = true;
            textBox2.Name = "textBox2";
            textBox2.Text = Description;
            textBox2.Size = new Size(260, 121);
            textBox2.TabIndex = 1;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(197, 210);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(75, 23);
            btnSave.TabIndex = 3;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // btnSave
            // 
            btnCancel.Location = new Point(12, 210);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 23);
            btnCancel.TabIndex = 3;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // TaskDescriptionForm
            // 
            ClientSize = new Size(288, 256);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(textBox1);
            Controls.Add(textBox2);
            Controls.Add(btnSave);
            Controls.Add(btnCancel);
            Name = "TaskDescriptionForm";
            Text = "Task Id: " + Id;
            ResumeLayout(false);
            PerformLayout();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            this.Title = textBox1.Text;
            this.Description = textBox2.Text;
            DialogResult = DialogResult.OK;
            Close();
        }

    }
}
