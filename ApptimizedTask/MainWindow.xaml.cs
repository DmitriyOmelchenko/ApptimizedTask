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
using System.Windows.Threading;



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
        private int CreateDeltaFile(FileInfo BaseFile, string directoryPath)
        {

            int deltaFileNumber = 0;

            // Create the file.
            using (BinaryReader binaryReader = new BinaryReader(BaseFile.OpenRead()))
            {

                int bytesRead;
                byte[] buffer = new byte[50000000];
                while ((bytesRead = binaryReader.Read(buffer, 0, buffer.Length)) > 0)
                {
                    deltaFileNumber++;
                    using (BinaryWriter binaryWriter = new BinaryWriter(File.Create(directoryPath + @"\" + deltaFileNumber, bytesRead)))
                    {
                        // Add some information to the file.
                        binaryWriter.Write(buffer, 0, bytesRead);
                    }
                }
            }
            return deltaFileNumber;
        }
        private async void OpendButton_Click(object sender, RoutedEventArgs e)
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

              var count=  await Dispatcher.InvokeAsync(() => CreateDeltaFile(BaseFile, Environment.CurrentDirectory + @"\Test"), DispatcherPriority.Background);
                MessageBox.Show(String.Format("{0} delta files was create",count));
            }
        }
    }
}
