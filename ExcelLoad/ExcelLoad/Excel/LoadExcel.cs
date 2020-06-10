using System;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;
using System.Collections;
using System.Collections.Generic;

public static class Path
{
    public static string OutPath;
    public static string InPath;
    public static string SourcePath;
    public static string LoadBinaryPath;

    public static LinkedList<string> ExcelList = new LinkedList<string>();
}

public class CSheetData
{
    public System.Collections.ArrayList VarArray = new System.Collections.ArrayList();
    public System.Collections.ArrayList TypeArray = new System.Collections.ArrayList();
    public int IndexCount = 0;
};

public class CSheetDataInfo
{
    public string Name;
    public CSheetData SheetData;

}
public class CExcelData
{
    //public System.Collections.Hashtable SheetDataHash = new System.Collections.Hashtable();
    public System.Collections.ArrayList SheetDatas = new System.Collections.ArrayList();
}

class CLoadExcel
{
    Excel.Application Application = new Excel.Application();

    public CExcelData m_ExcelData = new CExcelData();

    public void CleanUp()
    {
        Application.Quit();
    }
    string MakePath(System.IO.FileInfo FileInfo)
    {
        string strPath = Path.OutPath;
        string FileName = FileInfo.Name;
        int Index = FileName.IndexOf(".");
        FileName = FileName.Substring(0, Index);
        FileName += ".bytes";
        strPath += FileName;
        return strPath;
    }
    void InitRow(CSheetData SheetData, Excel.Range Range, ref int MaxRow)
    {
        for (int RowCount = 1; RowCount <= Range.Rows.Count + 1; ++RowCount)
        {
            if ((Range.Cells[RowCount, 1] as Excel.Range).Value2 == null)
            {
                MaxRow = RowCount-1;
                SheetData.IndexCount = RowCount - 3;
                break;
            }
        }
    }
    ArrayList InitColoumn(Excel.Range Range, ref int MaxColumn)
    {
        ArrayList NotReadColumns = new ArrayList();
        for (int ColumnCount = 1; ColumnCount <= Range.Columns.Count + 1; ++ColumnCount)
        {
            if ((Range.Cells[1, ColumnCount] as Excel.Range).Value2 == null)
            {
                MaxColumn = ColumnCount - 1;
                break;
            }
            else
            {
                string Value = (Range.Cells[1, ColumnCount] as Excel.Range).Value2;
                if (Value.Contains("#"))
                    NotReadColumns.Add(ColumnCount);
            }
        }
        return NotReadColumns;
    }
    public void LoadExcel(System.IO.FileInfo FileInfo)
    {
        string str;
        double Num;
        int RowCount = 0;
        int ColumnCount = 0;
        int MaxColumn = 0;
        int MaxRow = 0;

        string strPath = MakePath(FileInfo);

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
            CSheetDataInfo SheetDataInfo = new CSheetDataInfo();
            SheetDataInfo.Name = WorkSheet.Name;
            SheetDataInfo.SheetData = SheetData;
            m_ExcelData.SheetDatas.Add(SheetDataInfo);
            
            InitRow(SheetData, Range, ref MaxRow);
            ArrayList NotReadColumns = InitColoumn(Range, ref MaxColumn);

            for (RowCount = 1; RowCount <= MaxRow; ++RowCount)
            {
                for (ColumnCount = 1; ColumnCount <= MaxColumn; ++ColumnCount)
                {
                    if (NotReadColumns.Contains(ColumnCount))
                        continue;
                    // 공백일때 0으로 값 초기화
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

                    // 변수명
                    if (RowCount == 1)
                    {
                        SheetData.VarArray.Add((Range.Cells[RowCount, ColumnCount] as Excel.Range).Value2);
                    }
                    // 자료형
                    else if(RowCount == 2)
                    {
                        SheetData.TypeArray.Add((Range.Cells[RowCount, ColumnCount] as Excel.Range).Value2);
                    }
                    else
                    {
                        int TypeCheckIndex = ColumnCount - 1 - NotReadColumns.Count;
                        if (SheetData.TypeArray[TypeCheckIndex].ToString().Contains("int"))
                        {
                            Num = (double)(Range.Cells[RowCount, ColumnCount] as Excel.Range).Value2;
                            bw.Write((int)Num);
                        }
                        else if(SheetData.TypeArray[TypeCheckIndex].ToString().Contains("float"))
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
