using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using Utilities;

namespace ReduxLauncher
{
    class WebDirectory
    {
        public string URL;

        public List<string> AddressIndex = new List<string>();
        public List<string> NameIndex = new List<string>();

        public List<WebDirectory> SubDirectories = new List<WebDirectory>();

        public WebDirectory(string url)
        {
            URL = url;
        }
    }

    class PatchHandler
    {
        static readonly string patchNotesUrl = @"https://www.uo-redux.com/patchnotes.txt";
        static readonly string versionUrl = @"https://www.uo-redux.com/version.txt";
        static readonly string masterUrl = @"https://www.uo-redux.com/data/";

        static WebClient webClient = new WebClient();

        byte[] patchBytes;
        byte[] versionBytes;

        Interface launcherInterface;

        int version = -1;

        static int filesDownloaded = 0;

        static WebDirectory rootDirectory = null;

        public PatchHandler(Interface i)
        {
            launcherInterface = i;

            GatherData();
            ObtainCurrentVersion();

            webClient.UseDefaultCredentials = true;

            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += new UnhandledExceptionEventHandler(GlobalErrorHandler);
        }

        private void GlobalErrorHandler(object sender, UnhandledExceptionEventArgs e)
        {
            LogHandler.LogGlobalErrors(e.ToString());
        }

        void ObtainCurrentVersion()
        {
            if (Int32.TryParse(VersionData(), out version))
            {
                //Need to compare to user's current version.
            }
        }

        void ConstructIndex(WebDirectory directory)
        {
            string[] index = ParseFolderIndex(directory.URL);

            if (index != null)
            {
                for (int i = 0; i < index.Length; i++)
                {
                    directory.AddressIndex.Add(directory.URL + index[i]);
                    directory.NameIndex.Add(index[i]);
                }
            }
        }

        void GatherData()
        {
            try
            {
                patchBytes = webClient.DownloadData(patchNotesUrl);
                versionBytes = webClient.DownloadData(versionUrl);
            }

            catch (Exception e)
            {
                LogHandler.LogErrors(e.ToString());
            }
        }

        public string PatchData()
        {
            return System.Text.Encoding.UTF8.GetString(patchBytes);
        }

        public string VersionData()
        {
            return System.Text.Encoding.UTF8.GetString(versionBytes);
        }

        public async Task InitializeDownload()
        {
            if (rootDirectory == null)
            {
                rootDirectory = GenerateDirectory(masterUrl);
                ConstructIndex(rootDirectory);
            }

            if (rootDirectory != null)
            {
                await DownloadIndexedFiles(rootDirectory);
            }
        }

        WebDirectory GenerateDirectory(string url)
        {
            WebDirectory directory = new WebDirectory(url);        
            return directory;
        }        

        public async Task DownloadIndexedFiles(object o)
        {          
            try
            {
                WebDirectory directory = o as WebDirectory;

                while (directory.AddressIndex.Count > 0)
                {
                    string address = directory.AddressIndex[0];
                    string path = directory.NameIndex[0];

                    await Task.WhenAll(DownloadFile(directory, address));

                }

                for (int i = 0; i < directory.SubDirectories.Count; i++)
                {
                    launcherInterface.UpdatePatchNotes
                        ("Parsing Subdirectory: \n" + directory.SubDirectories[i].URL);

                    ConstructIndex(directory.SubDirectories[i]);
                    await DownloadIndexedFiles(directory.SubDirectories[i]);
                }
            }

            catch (Exception e)
            {
                LogHandler.LogErrors(e.ToString());
            }          
        }

        async Task DownloadFile(WebDirectory directory, string address)
        {
            try
            {
                webClient.DownloadFileCompleted += 
                    new AsyncCompletedEventHandler(DownloadFinished_Callback);
                webClient.DownloadProgressChanged += new
                    DownloadProgressChangedEventHandler(DownloadProgress_Callback);

                string path = address.Substring(masterUrl.Length);

                if (path.Contains('/'))
                {
                    string[]  splitPath = path.Split('/');

                    int tempLength = 0;

                    if (splitPath.Length == 1)
                        tempLength = splitPath[0].Length;

                    else
                    {
                        for (int i = 0; i < splitPath.Length - 1; i++)
                        {
                            tempLength += splitPath[i].Length;
                        }
                    }

                    string folderName = path.Substring(0, tempLength +1);

                    QueryDirectory(folderName);
                }

                launcherInterface.UpdatePatchNotes(string.Format("Downloading File ({0}): " + 
                    (address.Remove(address.IndexOf(masterUrl), masterUrl.Length)), filesDownloaded));

                await webClient.DownloadFileTaskAsync(new Uri(address), path);

                filesDownloaded++;

                directory.NameIndex.RemoveAt(0);
                directory.AddressIndex.RemoveAt(0);
            }

            catch (Exception e)
            {
                LogHandler.LogErrors(e.ToString());

                if (e.InnerException != null)
                    LogHandler.LogErrors(e.InnerException.ToString());
            } 
        }

        private void QueryDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        private void DownloadFinished_Callback(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                LogHandler.LogErrors(e.Error.ToString());
                LogHandler.LogErrors(e.Error.InnerException.ToString());
            }
        }

        void DownloadProgress_Callback(object sender, DownloadProgressChangedEventArgs e)
        {
            if (launcherInterface.InvokeRequired)
            {
                launcherInterface.Invoke(new MethodInvoker(delegate
                    {
                        if (launcherInterface.ProgressBar().Value
                             < launcherInterface.ProgressBar().Maximum)
                        {
                            launcherInterface.ProgressBar().PerformStep();
                        }

                        else
                            launcherInterface.ProgressBar().Value
                                = launcherInterface.ProgressBar().Minimum;
                    }));
            }

            else
            {
                if (launcherInterface.ProgressBar().Value
                        < launcherInterface.ProgressBar().Maximum)
                {
                    launcherInterface.ProgressBar().PerformStep();
                }

                else
                    launcherInterface.ProgressBar().Value
                        = launcherInterface.ProgressBar().Minimum;
            }
        }

        public string[] ParseFolderIndex(string url)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Timeout = 3 * 60 * 1000;
                request.KeepAlive = true;

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    List<string> fileLocations = new List<string>();
                    string line;

                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        while ((line = reader.ReadLine()) != null)
                        {
                            int index = line.IndexOf("<a href=");

                            if (index >= 0)
                            {
                                string[] segments = line.Substring(index).Split('\"');

                                if (!segments[1].Contains("/"))
                                {
                                    fileLocations.Add(segments[1]);
                                    launcherInterface.UpdatePatchNotes("File Found: " + segments[1]);
                                }

                                else
                                {
                                    if (segments[1] != "../")
                                    {
                                        rootDirectory.SubDirectories.Add(new WebDirectory(url + segments[1]));
                                        launcherInterface.UpdatePatchNotes("Directory Found: " + segments[1].Replace("/", string.Empty));
                                    }
                                }
                            }

                            else if (line.Contains("</pre"))
                                break;
                        }
                    }

                    response.Dispose();
                    return fileLocations.ToArray<string>();
                }

                else return new string[0];
            }

            catch (Exception e)
            {
                LogHandler.LogErrors(e.ToString());
                return null;
            }
        }
    }
}
