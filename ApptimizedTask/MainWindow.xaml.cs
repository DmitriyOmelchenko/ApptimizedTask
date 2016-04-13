using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace ApptimizedTask
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
        }
        //creating delta file
        private void CreateDeltaFile(FileInfo BaseFile, string directoryPath)
        {

            int deltaFileNumber = 1;
            int currentIndex = 0;
            int nextIndex=50000000;
            Byte[] info = File.ReadAllBytes(BaseFile.FullName);
            // Create the file.
            do
            {
                if (nextIndex > info.Length)
                    nextIndex = info.Length;
               using (FileStream fs = File.Create(directoryPath+@"\"+deltaFileNumber, 50000000))
                {
                    // Add some information to the file.
                    fs.Write(info.Skip(currentIndex).Take(nextIndex).ToArray(), 0, info.Skip(currentIndex).Take(nextIndex).ToArray().Length);
                }
                deltaFileNumber++;
                currentIndex = nextIndex;
                nextIndex += nextIndex;
            } while (currentIndex< info.LongLength);
            
        }
        private void OpendButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            //create directory for delta file
             if (!Directory.Exists(Environment.CurrentDirectory + @"\Test"))
             Directory.CreateDirectory(Environment.CurrentDirectory + @"\Test");
            if (openFileDialog.ShowDialog() == true)
            {
               FileInfo BaseFile = new FileInfo(openFileDialog.FileName);
                // 
                if (BaseFile.Length==0)
                {
                    MessageBox.Show("File is Empty");
                    return;
                }
               
                    CreateDeltaFile(BaseFile, Environment.CurrentDirectory + @"\Test");
                MessageBox.Show("Delta file create");
            }
        }
    }
}
