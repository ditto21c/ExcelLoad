using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExcelLoad
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            label1.Text = "Start Load Excel";

            LoadiniFile();

            label1.Text = "Success Load .ini";

            LoadExcelFilesAndCreateCsFiles();

            label1.Text = "Success Load Excel";

            Close();
        }

        void LoadiniFile()
        {
            string OutTemp = "";
            string Directory = System.Environment.CurrentDirectory + "\\";
            CControlIniFile ControlIniFile = new CControlIniFile(Directory);
            ControlIniFile.GetString("Path", "Out", "", out OutTemp, 1024, "Config.ini");
            Path.OutPath = OutTemp;
            ControlIniFile.GetString("Path", "In", "", out OutTemp, 1024, "Config.ini");
            Path.InPath = OutTemp;
            ControlIniFile.GetString("Path", "CppFile", "", out OutTemp, 1024, "Config.ini");
            Path.SourcePath = OutTemp;
            ControlIniFile.GetString("Path", "LoadBinaryPath", "", out OutTemp, 1024, "Config.ini");
            Path.LoadBinaryPath = OutTemp;
        }
        void LoadExcelFilesAndCreateCsFiles()
        {
            System.IO.DirectoryInfo directory = new System.IO.DirectoryInfo(Path.InPath);
            foreach (System.IO.FileInfo FileInfo in directory.GetFiles())
            {
                if (FileInfo.Extension == ".xlsx")
                {
                    CLoadExcel LoadExcel = new CLoadExcel();
                    string FileName = FileInfo.Name;
                    int Index = FileName.IndexOf(".");
                    FileName = FileName.Substring(0, Index);

                    Path.ExcelList.AddLast(FileName);
                    LoadExcel.LoadExcel(FileInfo);

                    CreateCpp CreateCpp = new CreateCpp();
                    CreateCpp.CreateFile(Path.SourcePath, FileName, Path.LoadBinaryPath, LoadExcel);

                    label1.Text = "Success Load " + FileInfo.Name;
                }
            }
        }
       
    }
}
