using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Reflection;
using System.Diagnostics;

namespace sweepnet
{
    public static class Version
    {
        public const int build = 59;//сборка
        public const int major = 2;
        public const int minor = 0;
        public const String p = " RC1";
        private static int should_stop;
        public static bool single = false;

        public static void Stop()
        {
            should_stop = 1;
        }
        public static String GetName()
        {
            return "Sweep-Net ";
        }
        public static String GetVersion()
        {
            return major+"."+minor+p;
        }

   

        public static bool IsSingleInstance()
        {
            //Get a reference to the current process
            Process proc = Process.GetCurrentProcess();

            //Get all running processes with the same name
            Process[] procs = Process.GetProcessesByName(proc.ProcessName);

            //Loop through all matching processes
            for (int i = 0; i < procs.Length; i++)
            {
                //Make sure we're testing against another process
                if (procs[i].Id != proc.Id)
                {
                    //Check for matching exe locations
                    if (procs[i].MainModule.FileName == proc.MainModule.FileName)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static string GetRandomString()
        {
            string path = Path.GetRandomFileName();
            path = path.Replace(".", ""); // Remove period.
            return path;
        }
        public static void CheckUpdates(int sleep=0)
        {
            /* 
           Thread t = new Thread(delegate() {
            //   MessageBox.Show(Environment.GetCommandLineArgs()[0]);
               Thread.Sleep(sleep * 1000);
               if (s == 0)
               {

                   try
                   {
                     HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://toolserver.org/~haffman/patserv/check.php");
                       request.UserAgent = major + ";" + minor + ";" + build;
                       HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                       Stream receiveStream = response.GetResponseStream();
                       StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
                       String data = readStream.ReadToEnd();
                       response.Close();
                       readStream.Close();
                       data = data.Trim();
                      // MessageBox.Show(data);
                       if (data != "" && data.Length > 10)
                       {
                           String[] p = data.Split(new string[] { "\n" },
               StringSplitOptions.None);
                           String msg = "";
                          // if (!p[2].Equals("")) { p[2] = " " + p[2]; }
                           if (!p[0].Equals(major + "")) { msg = i18n.Get("0705")+" "; } // Доступно крупное обновление до 
                           else if (!p[1].Equals(minor + "")) { msg = i18n.Get("0706") + " "; } // Доступно обновление до 
                           else if (!p[3].Equals(build + "")) { msg = i18n.Get("0707") + " "; } // Доступна новая сборка 
                           if (msg != ""&&Convert.ToInt32(p[3])>build)
                           {
                               msg += String.Format(i18n.Get("0708"), p[0], p[1], p[2], p[3]);//"версии " + p[0] + "." + p[1] + "" + p[2] + "; сборка " + p[3] + "\r\n";
                               //msg += "(ваша текущая версия - "+GetVersion()+")\r\n";
                               if (p.Length > 4) //есть информация о нововведниях
                               {
                                   msg += i18n.Get("0709"); //"Нововведения:\r\n";
                                   for (int a = 4; a < p.Length; a++)
                                   {
                                       msg += " - " + p[a] + "\r\n";
                                   }
                               }
                               msg += i18n.Get("0710"); //"Обновление загрузится и установится автоматически. Вы согласны?";
                               var result = MessageBox.Show(msg, String.Format(i18n.Get("0711"), p[0], p[1], p[2]), //"Доступно обновление " + p[0] + "." + p[1] + "" + p[2] + "!",
                                            MessageBoxButtons.YesNo,
                                            MessageBoxIcon.Question);
                               if (result == DialogResult.Yes)
                               {
                                   //MessageBox.Show("Сейчас начнется загрузка!");
                                   WebClient webClient = new WebClient();//@Environment.GetCommandLineArgs()[0])
                                   String p0, p1;
                                   p0 = GetRandomString() + ".exe";
                                   p1 = GetRandomString() + ".bat";
                                   webClient.DownloadFile("http://toolserver.org/~haffman/patserv/latest.exe", p0);


                                   MessageBox.Show(i18n.Get("0712"));

                                   System.IO.TextWriter BatFile = File.CreateText(p1);

                                   Process pi = Process.GetCurrentProcess();
                                   string assemblyName = pi.ProcessName + ".exe";
                                   BatFile.WriteLine("sleep -m 1200"); //url
                                   BatFile.WriteLine("TASKKILL /pid " + pi.Id);
                                   BatFile.WriteLine("DEL " + assemblyName);
                                   BatFile.WriteLine("rename " + p0 + " " + assemblyName);
                                   BatFile.WriteLine("start /i "+assemblyName);
                                   BatFile.WriteLine("DEL " + p1);
                                   BatFile.Close();
                               //    BatFile.Dispose();
                                   webClient.Dispose();



                                   //Process.Start(p1);

                                   ProcessStartInfo processInfo = new ProcessStartInfo();
                                  // processInfo.Verb = "runas";
                                   processInfo.FileName = p1;

                                   try
                                   {
                                       Process.Start(processInfo);
                                   }
                                   catch { }
                                  // commandtorun(p1);

                                   foreach (Form form in Application.OpenForms)
                                   {
                                       form.Close();
                                   }
                                    
                                   Environment.Exit(-1);
                                   //Application.Restart();
                               }
                           }

                       }
                       else
                       {
                           if (sleep == 0)
                           {
                               MessageBox.Show(i18n.Get("0713"));
                           }
                       }
                       
                  }
                   catch (Exception e) { if (sleep == 0) { MessageBox.Show(e.Message); } }
               }
           });
           t.IsBackground = true;
           t.Start();*/
        }
    }
}
