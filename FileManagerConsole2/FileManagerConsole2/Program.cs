using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FileManagerConsole2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to DJHaski's File manager!");
            Console.Write("Add directory path: ");
            string rootPath = Convert.ToString(Console.ReadLine());
            rootPath = @"E:\Csharp-stuff";
            MainF(rootPath);
            /*
            string[] dirs = Directory.GetDirectories(rootPath,"*", SearchOption.AllDirectories);

            foreach(string dir in dirs)
            {
                Console.WriteLine(dir);
            }*/
        }

        static void MainF(string rootPath)
        {
            Console.WriteLine("");
            Console.Write("Type your command(to see all commands type 'help'): ");
            string action = Convert.ToString(Console.ReadLine());
            Console.WriteLine("");
            if (action == "cd")
            {
                Console.Write("New directory path: ");
                string newdir = Convert.ToString(Console.ReadLine());
                ChangeDirectory(rootPath, newdir);
            }
            if(action == "help")
            {
                Console.WriteLine("Type 'cd' to change directory");
                Console.WriteLine("Type 'gd' to get directories");
                Console.WriteLine("Type 'gf' to get files");
                Console.WriteLine("Type 'ga' to get both(dirs and files)");
                Console.WriteLine("Type 'crd' to create directory");
                Console.WriteLine("Type 'crf' to create file");
                Console.WriteLine("Type 'gp' to current path");
                Console.WriteLine("Type 'main' to get on start page");
                Console.WriteLine("Type 'exit' to get exit programm");
                action = Convert.ToString(Console.ReadLine());
                if(action == "main")
                {
                    MainF(rootPath);
                }
                if (action == "exit")
                {
                    Console.Write("Goodbye!");
                    Environment.FailFast("User Exit");
                }
                
            }
            if (action == "gd")
            {
                GetDirectories(rootPath);
            }
            if (action == "gp")
            {
                GetPath(rootPath);
            }
            if(action == "gf")
            {
                GetFiles(rootPath);
            }
            if(action == "ga")
            {
                GetAll(rootPath);
            }
            if(action == "exit")
            {
                Console.Write("Goodbye!");
                Environment.FailFast("User Exit");
            }
            if(action == "crf")
            {
                Console.Write("Type file name: ");
                string newfile = Convert.ToString(Console.ReadLine());
                CreateFile(rootPath, newfile);
            }
            if(action == "crd")
            {
                Console.Write("Type directory name: ");
                string newdir = Convert.ToString(Console.ReadLine());
                CreateDirectory(rootPath,newdir);
            }

            if (action == "dd")
            {
                Console.Write("Type directory name: ");
                string dirname = Convert.ToString(Console.ReadLine());
                DeleteDirectory(rootPath, dirname);
            }
            if (action == "df")
            {
                Console.Write("Type file name: ");
                string filename = Convert.ToString(Console.ReadLine());
                DeleteFile(rootPath, filename);
            }
            if (action == "of")
            {
                Console.Write("Type file name: ");
                string filename = Convert.ToString(Console.ReadLine());
                OpenFile(rootPath, filename);
            }
            if (action == "od")
            {
                Console.Write("Type directory name: ");
                string dirname = Convert.ToString(Console.ReadLine());
                OpenDirectory(rootPath, dirname);
            }

        }
        static void GetPath(string rootPath)
        {
            Console.WriteLine("Current root is: " + rootPath);
            MainF(rootPath);
        }
        static void GetDirectories(string rootPath)
        {
            string[] dirs = Directory.GetDirectories(rootPath, "*", SearchOption.TopDirectoryOnly);
            Console.WriteLine("Folders in " + rootPath + ":");
            foreach (string dir in dirs)
            {
                Console.WriteLine(Path.GetFileName(dir));
            }
            MainF(rootPath);

        }
        static void CreateDirectory(string rootPath, string newdir)
        {
            //rootPath=Console.ReadLine();
            bool directoryExists = Directory.Exists(rootPath+@"\"+newdir);
            if (directoryExists)
            {
                Console.WriteLine("The directory exists");
            }
            else
            {
                Console.WriteLine("No such directore found. Creating new.");
                
                Directory.CreateDirectory(rootPath + @"\" + newdir);
            }
            MainF(rootPath);
        }
        static void ChangeDirectory(string rootPath,string newdir)
        {
            rootPath = newdir;
            MainF(rootPath);

        }
        static void GetFiles(string rootPath)
        {
            var files = Directory.GetFiles(rootPath, "*.*", SearchOption.TopDirectoryOnly);
            foreach (string file in files)
            {
               
                var info = new FileInfo(file);
                Console.WriteLine($"{Path.GetFileName(file)} : {info.Length} bytes");
            }
            MainF(rootPath);
        }
        static void CreateFile(string rootPath, string newfile)
        {
            bool fileExists = File.Exists(rootPath + @"\" + newfile);
            if (fileExists)
            {
                Console.WriteLine("The file exists");
            }
            else
            {
                Console.WriteLine("No such file found. Creating new.");

                File.Create(rootPath + @"\" + newfile);
            }
            MainF(rootPath);
        }
        static void GetAll(string rootPath)
        {
            var files = Directory.GetFiles(rootPath, "*.*", SearchOption.TopDirectoryOnly);
            string[] dirs = Directory.GetDirectories(rootPath, "*", SearchOption.TopDirectoryOnly);
            Console.WriteLine("Folders in " + rootPath + ":");
            foreach (string dir in dirs)
            {
                Console.WriteLine(Path.GetFileNameWithoutExtension(dir));
            }
            Console.WriteLine("Files: ");
            foreach (string file in files)
            {
                
                var info = new FileInfo(file);
                Console.WriteLine($"{Path.GetFileName(file)} : {info.Length} bytes");
            }
            MainF(rootPath);
        }

        static void DeleteFile(string rootPath, string filename)
        {
            bool fileExists = File.Exists(rootPath + @"\" + filename);
            if (fileExists)
            {
                Console.WriteLine("The file exists. Deleating");
                File.Delete(rootPath + @"\" + filename);
            }
            else
            {
                Console.WriteLine("No such file found.");
            }
            MainF(rootPath);
        }
        static void DeleteDirectory(string rootPath, string dirname)
        {
            bool directoryExists = Directory.Exists(rootPath + @"\" + dirname);
            if (directoryExists)
            {
                Console.WriteLine("The directory exists.Deleting");
                Directory.Delete(rootPath + @"\" + dirname);
            }
            else
            {
                Console.WriteLine("No such directore found"); 
            }
            MainF(rootPath);
        }
        static void OpenFile(string rootPath, string filename)
        {
            bool fileExists = File.Exists(rootPath + @"\" + filename);
            if (fileExists)
            {
                Console.WriteLine("The file exists. Opening");
                File.OpenRead(rootPath + @"\" + filename);
            }
            else
            {
                Console.WriteLine("No such file found.");
            }
            MainF(rootPath);
        }
        static void OpenDirectory(string rootPath, string dirname)
        {
            bool directoryExists = Directory.Exists(rootPath + @"\" + dirname);
            if (directoryExists)
            {
                Console.WriteLine("The directory exists.Opening");
                System.Diagnostics.Process.Start(rootPath + @"\" + dirname);
            }
            else
            {
                Console.WriteLine("No such directore found");
            }
            MainF(rootPath);
        }

    }
}
