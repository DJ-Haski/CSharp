using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private string filePath = "D:";
        private bool isFile = false;
        private string currentlySelectedItemName = "";
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            filePathTextBox.Text = filePath;
            loadFilesAndDirectories();
        }
        private void label1_Click(object sender, EventArgs e)
        {
            
        }
        public void loadFilesAndDirectories()
        {
            DirectoryInfo fileList;
            string tempFilePath = "";
            FileAttributes fileAttr;

            try
            {
                if (isFile)
                {
                    tempFilePath = filePath + "/" + currentlySelectedItemName;
                    FileInfo fileDetails = new FileInfo(tempFilePath);
                    FileNameLabel.Text = fileDetails.Name;
                    FileTypeLabel.Text = fileDetails.Extension;
                    fileAttr = File.GetAttributes(tempFilePath);
                    Process.Start(tempFilePath);
                }
                else
                {
                    fileAttr = File.GetAttributes(filePath);

                }
                if ((fileAttr & FileAttributes.Directory) == FileAttributes.Directory)
                {

                    fileList = new DirectoryInfo(filePath);
                    FileInfo[] files = fileList.GetFiles(); // получаем все файлы
                    DirectoryInfo[] dirs = fileList.GetDirectories(); //получаем все директории

                    listView1.Items.Clear();

                    for (int i = 0; i < files.Length; i++)
                    {
                        listView1.Items.Add(files[i].Name, 0);
                    }
                    for (int i = 0; i < dirs.Length; i++)
                    {
                        listView1.Items.Add(dirs[i].Name,1);
                    }
                }
                else
                {
                    FileNameLabel.Text = this.currentlySelectedItemName;
                }
            }
            catch (Exception e)
            {

            }
        }
        public void loadButtonAction()
        {
            removeBackSlash();
            filePath = filePathTextBox.Text;
            loadFilesAndDirectories();
            isFile = false;
            
        }
        public void removeBackSlash()
        {
            string path = filePathTextBox.Text;
            if(path.LastIndexOf("/") == path.Length - 1)
            {
                filePathTextBox.Text = path.Substring(0, path.Length - 1);
            }
        }

        public void goBack()//функция назад
        {
            try
            {
                string path = filePathTextBox.Text;
                path = path.Substring(0, path.LastIndexOf("/"));
                this.isFile = false;
                filePathTextBox.Text = path;
                removeBackSlash();
            }
            catch(Exception e)
            {

            }
        }
        private void goButton_Click(object sender, EventArgs e)//кнопка вперед
        {
            loadButtonAction();
        }

        private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            currentlySelectedItemName = e.Item.Text;

            FileAttributes fileAttr = File.GetAttributes(filePath + "/" + currentlySelectedItemName);
            if ((fileAttr & FileAttributes.Directory) == FileAttributes.Directory)
            {
                isFile = true;
                filePathTextBox.Text = filePath + "/" + currentlySelectedItemName;
            }
            else
            {
                isFile = false;
            }
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            loadButtonAction();
        }

        private void backButton_Click(object sender, EventArgs e)//кнопка назад
        {
            goBack();
            loadButtonAction();
        }
    }
}
