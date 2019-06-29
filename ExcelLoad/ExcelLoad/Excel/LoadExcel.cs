using System;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;

public static class Path
{
    public static string OutPath;
    public static string InPath;
    public static string SourcePath;
    public static string LoadBinaryPath;

    public static System.Collections.Generic.LinkedList<string> ExcelList = new System.Collections.Generic.LinkedList<string>();
}

public class CSheetData
{
    public System.Collections.ArrayList VarArray = new System.Collections.ArrayList();
    public System.Collections.ArrayList TypeArray = new System.Collections.ArrayList();
    public int IndexCount = 0;
};

public class CExcelData
{
    public System.Collections.Hashtable SheetDataHash = new System.Collections.Hashtable();
}

class CLoadExcel
{
    Excel.Application Application = new Excel.Application();

    public CExcelData m_ExcelData = new CExcelData();

    public void CleanUp()
    {
        Application.Quit();
    }
        
    public void LoadExcel(System.IO.FileInfo FileInfo)
    {
        string str;
        double Num;
        int RowCount = 0;
        int ColumnCount = 0;
        int MaxColumn = 0;
        int MaxRow = 0;

        string strPath = Path.OutPath;
        string FileName = FileInfo.Name;
        int Index = FileName.IndexOf(".");
        FileName = FileName.Substring(0, Index);
        FileName += ".bytes";
        strPath += FileName;

        FileStream fs = new FileStream(strPath, FileMode.Create);
        BinaryWriter bw = new BinaryWriter(fs);

        Excel.Workbook WorkBook = Application.Workbooks.Open(FileInfo.FullName, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
        
        for (int i=0; i<WorkBook.Worksheets.Count; ++i)
        {
            Excel.Worksheet WorkSheet = WorkBook.Worksheets.get_Item(i + 1);
            if (WorkSheet.Name.Contains("#"))
                continue;

            Excel.Range Range = WorkSheet.UsedRange;

            CSheetData SheetData = new CSheetData();
            m_ExcelData.SheetDataHash.Add(WorkSheet.Name, SheetData);

            for (RowCount = 1; RowCount <= Range.Rows.Count+1; ++RowCount)
            {
                if ((Range.Cells[RowCount, 1] as Excel.Range).Value2 == null)
                {
                    //bw.Write(RowCount -3);
                    MaxRow = RowCount;
                    SheetData.IndexCount = RowCount - 3;
                    break;
                }
            }


            for (RowCount = 1; RowCount <= Range.Rows.Count; ++RowCount)
            {
                if (MaxRow == RowCount)
                    break;

                for (ColumnCount = 1; ColumnCount <= Range.Columns.Count; ColumnCount++)
                {
                    if (RowCount == 1 && (Range.Cells[RowCount, ColumnCount] as Excel.Range).Value2 == null)
                        MaxColumn = ColumnCount;
                    else if (MaxColumn != 0 && MaxColumn <= ColumnCount)
                        break;
                    else if ((Range.Cells[RowCount, ColumnCount] as Excel.Range).Value2 == null)
                    {
                        if (SheetData.TypeArray[ColumnCount - 1].ToString().Contains("int"))
                        {
                            bw.Write((int)0);
                        }
                        else if ( SheetData.TypeArray[ColumnCount - 1].ToString().Contains("float"))
                        {
                            bw.Write((float)0.0f);
                        }
                        else
                        {
                            bw.Write("");
                        }
                        continue;
                    }

                    if (RowCount == 1)
                    {
                        SheetData.VarArray.Add((Range.Cells[RowCount, ColumnCount] as Excel.Range).Value2);
                    }
                    else if(RowCount == 2)
                    {
                        SheetData.TypeArray.Add((Range.Cells[RowCount, ColumnCount] as Excel.Range).Value2);
                    }
                    else
                    {
                        if (SheetData.TypeArray[ColumnCount - 1].ToString().Contains("int"))
                        {
                            Num = (double)(Range.Cells[RowCount, ColumnCount] as Excel.Range).Value2;
                            bw.Write((int)Num);
                        }
                        else if(SheetData.TypeArray[ColumnCount - 1].ToString().Contains("float"))
                        {
                            Num = (double)(Range.Cells[RowCount, ColumnCount] as Excel.Range).Value2;
                            bw.Write((float)Num);
                        }
                        else
                        {
                            str = (Range.Cells[RowCount, ColumnCount] as Excel.Range).Value2;
                            bw.Write(str);
                        }
                    }
                }
            }
        }

        WorkBook.Close();
        bw.Close();
        fs.Close();
    }
}
