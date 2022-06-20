using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;


namespace YamlYaml
{
    public class FtpClient
    {
        public struct FtpSettings
        {
            public string Address { get; set; }
            public uint Port { get; set; }
            public string Login { get; set; }
            public string Password { get; set; }
        }



        public bool IsConnected { get; set; }


        //private FtpWebRequest _req;
        //private FtpWebResponse _resp;
        private TcpClient tcpClient;
        private StreamReader sr;
        private FtpSettings site;

        /*
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

         */

        public FtpClient()
        {
            tcpClient = new TcpClient();
            IsConnected = false;


        }

        //public FtpClient(FtpSettings site)
        //{
        //    //_site = site;
        //    //Uri _uri = new Uri(string.Format("ftp://{0}:{1}/", _site.Address, _site.Port));
        //    //_req = (FtpWebRequest)WebRequest.Create(_uri);

        //    tcpClient = new TcpClient();
        //    IsConnected = false;


        //}

        //public FtpClient(string address, string login, string password, uint port = 21)
        //{
        //    //Uri _uri = new Uri(string.Format("ftp://{0}:{1}/", address, port));
        //    //_site.Address = address;
        //    //_site.Port = port;
        //    //_site.Login = login;
        //    //_site.Password = password;
        //    //_req = (FtpWebRequest)WebRequest.Create(_uri);

        //    tcpClient = new TcpClient();
        //    IsConnected = false;
        //}

        public void Connect(FtpSettings settings)
        {
            if(IsConnected)
                return;

            if(tcpClient == null)
                return;

            tcpClient.Connect(settings.Address, (int)settings.Port);
            
        }

        public void Connect(string serverName, uint port)
        {
            if(IsConnected)
                return;
        }

        ///////////////////////////////////////////////////////////////////////
        private string Flush()
        {
            try
            {

            }
            catch { }

            return string.Empty;
        }
    }
}
