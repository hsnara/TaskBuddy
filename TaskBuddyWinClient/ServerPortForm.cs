using System;
using System.Security.Policy;
using System.Windows.Forms;
using TaskBuddyClassLibrary.Models;
using TaskBuddyClassLibrary.Exceptions;
using System.Net.Sockets;
using System.Net;
using System.Text.RegularExpressions;

namespace TaskBuddyWinClient
{
    public partial class ServerPortForm : Form
    {
        public int ServerPort
        {
            get { return _port; }
            set { _port = value; }
        }
        public string ServerAddress
        {
            get { return _url; }
            set { _url = value; }
        }

        private string _url;
        private int _port;

        public TextBox textBoxServer;
        private Button btnCancel;
        public TextBox textBoxPort;

        public ServerPortForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            label1 = new Label();
            label2 = new Label();
            textBoxServer = new TextBox();
            textBoxPort = new TextBox();
            buttonOK = new Button();
            btnCancel = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 15);
            label1.Name = "label1";
            label1.Size = new Size(39, 15);
            label1.TabIndex = 0;
            label1.Text = "Server";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 42);
            label2.Name = "label2";
            label2.Size = new Size(29, 15);
            label2.TabIndex = 1;
            label2.Text = "Port";
            // 
            // textBoxServer
            // 
            textBoxServer.Location = new Point(60, 12);
            textBoxServer.Name = "textBoxServer";
            textBoxServer.Size = new Size(170, 23);
            textBoxServer.TabIndex = 2;
            // 
            // textBoxPort
            // 
            textBoxPort.Location = new Point(60, 39);
            textBoxPort.Name = "textBoxPort";
            textBoxPort.Size = new Size(170, 23);
            textBoxPort.TabIndex = 3;
            // 
            // buttonOK
            // 
            buttonOK.DialogResult = DialogResult.OK;
            buttonOK.Location = new Point(155, 68);
            buttonOK.Name = "buttonOK";
            buttonOK.Size = new Size(75, 23);
            buttonOK.TabIndex = 4;
            buttonOK.Text = "OK";
            buttonOK.UseVisualStyleBackColor = true;
            buttonOK.Click += buttonOK_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(60, 68);
            btnCancel.Name = "button1";
            btnCancel.Size = new Size(75, 23);
            btnCancel.TabIndex = 5;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // ServerPortForm
            // 
            AcceptButton = buttonOK;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = buttonOK;
            ClientSize = new Size(242, 102);
            Controls.Add(btnCancel);
            Controls.Add(buttonOK);
            Controls.Add(textBoxPort);
            Controls.Add(textBoxServer);
            Controls.Add(label2);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ServerPortForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Server and Port";
            ResumeLayout(false);
            PerformLayout();
        }

        private Label label1;
        private Label label2;
        private Button buttonOK;

        private void buttonOK_Click(object sender, EventArgs e)
        {
            
            
            try
            {
                string url = textBoxServer.Text;
                if (string.IsNullOrEmpty(url))
                {
                    throw new TaskBuddyException($"Invalid hostname.\nInput: {textBoxServer.Text}");
                }

                if (Uri.CheckHostName(url) == UriHostNameType.IPv4)
                {
                    if (!Regex.IsMatch(url, @"^(?:[0-9]{1,3}\.){3}[0-9]{1,3}$"))
                    {
                        throw new TaskBuddyException($"Invalid IPv4 address.\nInput: {url}");
                    }
                }
                else if (Uri.CheckHostName(url) == UriHostNameType.Dns)
                {
                    if (!Regex.IsMatch(url, @"^(([a-zA-Z0-9]|[a-zA-Z0-9][a-zA-Z0-9\-]*[a-zA-Z0-9])\.)*([A-Za-z0-9]|[A-Za-z0-9][A-Za-z0-9\-]*[A-Za-z0-9])$"))
                    {
                        throw new TaskBuddyException($"Invalid domain name.\nInput: {url}");
                    }
                }

                _url = url;



                int port = textBoxPort.Text != "" ? int.Parse(textBoxPort.Text) : 0;
                if (port <= 0 || port > 65535)
                {
                    throw new TaskBuddyException($"Invalid port number. Please enter a value between 1 and 65535.\nInput: {textBoxPort.Text}");
                }
                _port = port;
                DialogResult = DialogResult.OK;
                Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DialogResult = DialogResult.None;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

    }
}
