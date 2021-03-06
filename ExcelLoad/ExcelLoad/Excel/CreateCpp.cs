﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using File = System.IO.File;

class CreateCpp
{
    public void CreateFile(string Path, string FileName, string BinaryFilePath, CLoadExcel LoadExcel)
    {
        string TempFileName = Path + FileName + ".cs";
        //File.Create(TempFileName);

        int Index = -1;
        string[] str = new string[10000];

        str[++Index] = "using System;";
        str[++Index] = "using System.IO;";
        str[++Index] = "using System.Collections;";
        str[++Index] = "using System.Collections.Generic;";
        str[++Index] = "using UnityEngine;";

        ++Index; // new Line

        string TempStr = "";

        foreach(var sheetDataInfo in LoadExcel.m_ExcelData.SheetDatas)
        {
            var SheetDataInfo = sheetDataInfo as CSheetDataInfo;
            str[++Index] = "public class C" + SheetDataInfo.Name;
            CSheetData SheetData = SheetDataInfo.SheetData;

            str[++Index] = "{";

            // 변수
            for(int i=0; i< SheetData.TypeArray.Count; ++i)
            {
                if (TempStr != (string)SheetData.VarArray[i])
                {
                    str[++Index] = "\tpublic ";

                    bool bArrayList = false;
                    if (SheetData.TypeArray[i].ToString().Contains("List"))
                    {
                        bArrayList = true;
                        str[Index] += SheetData.TypeArray[i].ToString();
                    }
                    else
                        str[Index] += (string)SheetData.TypeArray[i];

                    str[Index] += " ";
                    str[Index] += (string)SheetData.VarArray[i];
                    
                    str[Index] += bArrayList ? " = new " + SheetData.TypeArray[i].ToString() + "();" : ";";
                    TempStr = (string)SheetData.VarArray[i];
                }
            }

            ++Index; // new Line

            int ArrayCount = 0;
            str[++Index] = "\tpublic void Load(BinaryReader BinaryReader)";
            str[++Index] = "\t{";

            for (int i = 0; i < SheetData.TypeArray.Count; ++i)
            {
                if (TempStr != (string)SheetData.VarArray[i])
                {
                    if(0 < ArrayCount)
                    {
                        str[++Index] = "\t\tfor(int i=0; i<" + Convert.ToString(ArrayCount+1) + "; ++i)";
                        str[++Index] = "\t\t{";
                        str[++Index] += "\t\t\t" + ((string)SheetData.VarArray[i-1] + ".Add(");
                        if (SheetData.TypeArray[i - 1].ToString().Contains("string"))
                        {
                            str[Index] += "BinaryReader.ReadString());";
                        }
                        else if (SheetData.TypeArray[i - 1].ToString().Contains("int"))
                        {
                            str[Index] += "BinaryReader.ReadInt32());";
                        }
                        else if (SheetData.TypeArray[i - 1].ToString().Contains("float"))
                        {
                            str[Index] += "BinaryReader.ReadSingle());";
                        }
                        str[++Index] = "\t\t}";
                    }

                    ArrayCount = 0;
                    if (!SheetData.TypeArray[i].ToString().Contains("List"))
                    {
                        str[++Index] += "\t\t" + ((string)SheetData.VarArray[i] + " = ");
                        if ((string)SheetData.TypeArray[i] == "string")
                        {
                            str[Index] += "BinaryReader.ReadString();";
                        }
                        else if ((string)SheetData.TypeArray[i] == "int")
                        {
                            str[Index] += "BinaryReader.ReadInt32();";
                        }
                        else if ((string)SheetData.TypeArray[i] == "float")
                        {
                            str[Index] += "BinaryReader.ReadSingle();";
                        }
                    }
                    else
                    {

                    }
                    TempStr = (string)SheetData.VarArray[i];
                }
                else
                    ArrayCount++;
            }
            if (0 < ArrayCount)
            {
                str[++Index] = "\t\tfor(int i=0; i<" + Convert.ToString(ArrayCount+1) + "; ++i)";
                str[++Index] = "\t\t{";
                str[++Index] += "\t\t\t" + ((string)SheetData.VarArray[SheetData.TypeArray.Count - 1] + ".Add(");
                if (SheetData.TypeArray[SheetData.TypeArray.Count - 1].ToString().Contains("string"))
                {
                    str[Index] += "BinaryReader.ReadString());";
                }
                else if (SheetData.TypeArray[SheetData.TypeArray.Count - 1].ToString().Contains("int"))
                {
                    str[Index] += "BinaryReader.ReadInt32());";
                }
                else if (SheetData.TypeArray[SheetData.TypeArray.Count - 1].ToString().Contains("float"))
                {
                    str[Index] += "BinaryReader.ReadSingle());";
                }
                str[++Index] = "\t\t}";
            }

           // str[++Index] = "}"; // loop End

            str[++Index] = "\t}";

            str[++Index] = "};";
            ++Index; // new Line
        }

       

        foreach (var sheetData in LoadExcel.m_ExcelData.SheetDatas)
        {
            CSheetDataInfo SheetDataInfo = sheetData as CSheetDataInfo;
            str[++Index] = "public class C" + SheetDataInfo.Name + "Sheet";
            str[++Index] = "{";

            CSheetData SheetData = SheetDataInfo.SheetData;
            string className = "C" + SheetDataInfo.Name;
            str[++Index] = "\tpublic C" + SheetDataInfo.Name + "[] vec = new C" + SheetDataInfo.Name + "[" + Convert.ToString(SheetData.IndexCount) + "]" + ";";
            str[++Index] = "\tpublic Dictionary<int, C" + SheetDataInfo.Name + "> map = new Dictionary<int, C" + SheetDataInfo.Name + ">();";

            str[++Index] = "};";
        }

        str[++Index] = "public class C" + FileName;
        str[++Index] = "{";

        foreach (var sheetData in LoadExcel.m_ExcelData.SheetDatas)
        {
            CSheetDataInfo SheetDataInfo = sheetData as CSheetDataInfo;
            CSheetData SheetData = SheetDataInfo.SheetData;
            str[++Index] = "\tpublic C" + SheetDataInfo.Name + "Sheet " + SheetDataInfo.Name + "Sheet = new C" + SheetDataInfo.Name + "Sheet();";
        }
        ++Index; // new Line

        str[++Index] = "\tpublic void Load()";
        str[++Index] = "\t{";
        str[++Index] = "\t\tTextAsset Asset = Resources.Load<TextAsset>(\"TextAssets/Tables/" + FileName + "\");";
        str[++Index] = "\t\tStream stream = new MemoryStream(Asset.bytes);";
        str[++Index] = "\t\tBinaryReader BinaryReader = new BinaryReader(stream);";

        foreach (var sheetDataInfo in LoadExcel.m_ExcelData.SheetDatas)
        {
            CSheetDataInfo SheetDataInfo = sheetDataInfo as CSheetDataInfo;
            CSheetData SheetData = SheetDataInfo.SheetData;
            str[++Index] = "\t\tfor(int i=0; i<" + Convert.ToString(SheetData.IndexCount) + "; ++i)";
            str[++Index] = "\t\t{";
            str[++Index] = "\t\t\t" + SheetDataInfo.Name + "Sheet." + "vec[i] = new C" + SheetDataInfo.Name + "();";
            str[++Index] = "\t\t\t" + SheetDataInfo.Name + "Sheet.vec[i].Load(BinaryReader);";
            str[++Index] = "\t\t\tint key =" + SheetDataInfo.Name + "Sheet.vec[i]." + (string)SheetData.VarArray[0] + ";";
            str[++Index] = "\t\t\t" + SheetDataInfo.Name + "Sheet.map.Add(key, " + SheetDataInfo.Name + "Sheet.vec[i]); ";
            str[++Index] = "\t\t}";
        }
        str[++Index] = "\t}";


        str[++Index] = "};";

        File.WriteAllLines(TempFileName, str);
    }
}
