using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

public static class Path
{
    public static string OutPath;
    public static string InPath;
    public static string SourcePath;
    public static string LoadBinaryPath;

    public static System.Collections.Generic.LinkedList<string> ExcelList = new System.Collections.Generic.LinkedList<string>();
}

namespace ExcelLoad
{
    static class Program
    {
        
        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            string OutTemp="";
            string Directory = System.Environment.CurrentDirectory + "\\"; 
            CControlIniFile ControlIniFile = new CControlIniFile(Directory);
            ControlIniFile.GetString("Path", "Out", "", out OutTemp, 1024, "Config.ini");
            Path.OutPath = OutTemp;
            ControlIniFile.GetString("Path", "In", "", out OutTemp, 1024, "Config.ini");
            Path.InPath = OutTemp;
            ControlIniFile.GetString("Path", "CppFile", "",out OutTemp, 1024, "Config.ini");
            Path.SourcePath = OutTemp;
            ControlIniFile.GetString("Path", "LoadBinaryPath", "",out OutTemp, 1024, "Config.ini");
            Path.LoadBinaryPath = OutTemp;
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
                }
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Form1 form = new Form1();
            form.label1.Text = "Success";
            Application.Run(form);

        }
    }
}
