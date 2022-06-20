using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;


namespace YamlYaml
{
	public partial class Form1 : Form
	{
		static readonly string APP_TITLE = "YamlYaml Editor";
		BackgroundWorker worker;
		//FtpWebRequest ftpClient;

		List<string> dirs = new List<string>();

		bool isConnected;

		private enum WorkStatus { IDLE = 0, UPLOADING, DOWNLOADING  }

		private WorkStatus workerStatus;

		struct FtpSettings
		{
			public string Login { get; set; }
			public string Password { get; set; }
			public string Address { get; set; }
			public uint Port { get; set; }
			public string Filename { get; set; }
			public string FullName { get; set; }
		}

		static FtpSettings ftpSettings;

		public Form1()
		{
			InitializeComponent();

			this.Text = APP_TITLE;
			this.MinimumSize = new Size(800, 600);

            worker = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };

            worker.DoWork += Worker_DoWork;
			worker.ProgressChanged += Worker_ProgressChanged;
			worker.RunWorkerCompleted += Worker_RunWorkerCompleted;

			this.FormClosing += Form1_FormClosing;

			pass_toolStripTextBox.TextBox.UseSystemPasswordChar = true;

			workerStatus = WorkStatus.IDLE;
			status_toolStripStatusLabel.Text = "Ready";
		}

		bool InputsEmpty()
		{
			return (address_toolStripTextBox.TextLength != 0 && login_toolStripTextBox.TextLength != 0 && pass_toolStripTextBox.TextLength != 0);
		}

		private List<string> GetAllFtpFiles(string parentFolderpath)
		{
			try
			{
				status_toolStripStatusLabel.Text = "Connecting";

				Uri uri = new Uri(string.Format("ftp://{0}:{1}{2}", ftpSettings.Address, ftpSettings.Port, parentFolderpath));
				FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(uri);
				ftpRequest.Credentials = new NetworkCredential(ftpSettings.Login, ftpSettings.Password);
				ftpRequest.Method = WebRequestMethods.Ftp.ListDirectory;
				FtpWebResponse response = (FtpWebResponse)ftpRequest.GetResponse();
				StreamReader streamReader = new StreamReader(response.GetResponseStream());

				List<string> _dirs = new List<string>();

				string line = streamReader.ReadLine();
				while(!string.IsNullOrEmpty(line))
				{
					var lineArr = line.Split('/');
					line = lineArr[lineArr.Count() - 1];
					_dirs.Add(line);
					line = streamReader.ReadLine();
				}
				streamReader.Close();
				response.Close();
				return _dirs;
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message);
				throw;
			}
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			isConnected = false;

			address_toolStripTextBox.Text = "127.0.0.1";
			login_toolStripTextBox.Text = "admin";
			pass_toolStripTextBox.Text = "admin1";
		}

		private void Form1_FormClosing(object? sender, FormClosingEventArgs e)
		{
			if(isConnected)
			{
				if(worker.IsBusy)
				{
					if(workerStatus == WorkStatus.UPLOADING)
					{

					}

					if(workerStatus == WorkStatus.DOWNLOADING)
					{

					}
				}
			}
		}

		private void login_toolStripButton_Click(object sender, EventArgs e)
		{
			if(!isConnected)
			{
                if(!InputsEmpty())
                {
                    MessageBox.Show("Cannot connect! Server credentials are not valid.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                ftpSettings.Address = address_toolStripTextBox.TextBox.Text;
				ftpSettings.Port = 14147;
                ftpSettings.Login = login_toolStripTextBox.TextBox.Text;
                ftpSettings.Password = pass_toolStripTextBox.TextBox.Text;
                worker.RunWorkerAsync();

				
			}
		}

		private void Worker_DoWork(object? sender, DoWorkEventArgs e)
		{
			//if(worker.CancellationPending) { }

			string[] names = GetAllFtpFiles("/").ToArray();

			//e.Result = names;
        }

		private void Worker_ProgressChanged(object? sender, ProgressChangedEventArgs e)
		{
			progress_toolStripProgressBar.Value = e.ProgressPercentage;
		}

		private void Worker_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
		{
			workerStatus = WorkStatus.IDLE;
			status_toolStripStatusLabel.Text = "Completed";

			//dirs = (List<string>)e.Result;

			//if(dirs.Count > 0)
			//{
			//	foreach(string name in dirs)
			//	{
			//		treeView.Nodes.Add(name);
			//	}
			//}
		}
	}
}
