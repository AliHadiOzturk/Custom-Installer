using IWshRuntimeLibrary;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MyInstaller
{
    [RunInstaller(true)]
    public partial class Installer : System.Configuration.Install.Installer
    {
        public Installer()
        {
            InitializeComponent();
        }
        public override void Install(IDictionary stateSaver)
        {
            //StreamWriter sw = new StreamWriter("C:\\Temp\\StemPOSOnBeforeInstall.txt", false);

            ////sw.WriteLine("savedState count : " + savedState.Count.ToString());
            ////sw.WriteLine("savedState keys : " + savedState.Keys.Count.ToString());
            ////sw.WriteLine("savedState values : " + savedState.Values.Count.ToString());
            //sw.WriteLine("USER TYPE : " + Context.Parameters["USERTYPE"].ToString());
            //sw.WriteLine("USER TYPE : " + Context.Parameters["version"]);

            //foreach (string k in Context.Parameters.Keys)
            //{
            //    sw.WriteLine("ContextKey [" + k + "]=" + Context.Parameters[k].ToString());
            //}

            //sw.Flush();
            //sw.Close();
            base.Install(stateSaver);
        }

        protected override void OnAfterInstall(IDictionary savedState)
        {
            var targetDir = Context.Parameters["targetdir"];
            targetDir = targetDir.Substring(0, targetDir.Length - 1);
            var userType = Context.Parameters["USERTYPE"].ToString();
            userType = userType.Substring(0, userType.Length - 2);
            
            //System.IO.File.AppendAllText("C:\\Temp\\StemPOSOnAfterInstall.txt", "UserType " + userType + " \n" + "TargetDir" + targetDir + "\n");
            if (userType == "1")
            {
                string shortcutLocation = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Stem POS Server.lnk");
                WshShell shell = new WshShell();
                IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutLocation);

                shortcut.Description = "Stem POS Kısayolu";   // The description of the shortcut
                shortcut.WorkingDirectory = targetDir;
                shortcut.TargetPath = targetDir + "Stem.exe";                 // The path of the file that will launch when the shortcut is run
                shortcut.Save();
            }
            else
            {
                System.IO.File.Delete(targetDir + "Stem.exe");
            }
            var arguments = "";
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            if (Environment.Is64BitOperatingSystem)
            {
                arguments = $"/C \"{targetDir}Client\\Stem POS.exe\"";
                startInfo.Arguments = arguments;
                //System.IO.File.AppendAllText("C:\\Temp\\StemPOSOnAfterInstall.txt", "Arguments " + arguments + "\n");
            }
            else
            {
                arguments = $"/C \"{targetDir}Client\\Stem POS 32Bit.exe\"";
                startInfo.Arguments = arguments;
                //System.IO.File.AppendAllText("C:\\Temp\\StemPOSOnAfterInstall.txt", "Arguments " + arguments + "\n");
            }
            startInfo.Arguments = arguments;
            process.StartInfo = startInfo;
            process.Start();

            //System.IO.File.AppendAllText("C:\\Temp\\StemPOSOnAfterInstall.txt", "AfterInstall Finished");

            base.OnAfterInstall(savedState);
        }

        protected override void OnAfterUninstall(IDictionary savedState)
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            System.IO.File.Delete(Path.Combine(desktopPath, "Stem POS Server.lnk"));
            base.OnAfterUninstall(savedState);
        }

        //protected override void OnBeforeInstall(IDictionary savedState)
        //{
        //    StreamWriter sw = new StreamWriter("C:\\Temp\\StemPOSOnBeforeInstall.txt", false);

        //    sw.WriteLine("savedState count : " + savedState.Count.ToString());
        //    sw.WriteLine("savedState keys : " + savedState.Keys.Count.ToString());
        //    sw.WriteLine("savedState values : " + savedState.Values.Count.ToString());
        //    sw.WriteLine("USER TYPE : " + Context.Parameters["USERTYPE"].ToString());
        //    sw.WriteLine("USER TYPE : " + Context.Parameters["version"]);

        //    foreach (string k in savedState.Keys)
        //    {
        //        sw.WriteLine("savedState key[" + k + "]= " + savedState[k].ToString());
        //    }

        //    writeContext(sw);

        //    sw.Flush();
        //    sw.Close();
        //    base.OnBeforeInstall(savedState);
        //}
        //public override void Commit(IDictionary savedState)
        //{
        //    base.Commit(savedState);
        //}
        //protected override void OnCommitting(IDictionary savedState)
        //{
        //    base.OnCommitting(savedState);
        //    StreamWriter sw = new StreamWriter("C:\\Temp\\StemPOS.txt", false);

        //    //sw.WriteLine("savedState count : " + savedState.Count.ToString());
        //    //sw.WriteLine("savedState keys : " + savedState.Keys.Count.ToString());
        //    //sw.WriteLine("savedState values : " + savedState.Values.Count.ToString());
        //    //sw.WriteLine("USER TYPE : " + Context.Parameters["USERTYPE"].ToString());
        //    //sw.WriteLine("USER TYPE : " + Context.Parameters["version"]);

        //    foreach (string k in Context.Parameters.Keys)
        //    {
        //        sw.WriteLine("ContextKey [" + k + "]=" + Context.Parameters[k].ToString());
        //    }

        //    //foreach (string k in savedState.Keys)
        //    //{
        //    //    sw.WriteLine("savedState key[" + k + "]= " + savedState[k].ToString());
        //    //}

        //    //writeContext(sw);

        //    sw.Flush();
        //    sw.Close();
        //}
    }
}
