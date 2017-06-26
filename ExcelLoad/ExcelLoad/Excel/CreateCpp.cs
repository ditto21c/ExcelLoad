using System;
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
        string[] str = new string[100];

        str[++Index] = "using System;";
        str[++Index] = "using System.IO;";
        str[++Index] = "using ArrayList = System.Collections.ArrayList;";
        str[++Index] = "using UnityEngine;";

        ++Index; // new Line

        string TempStr = "";

        foreach(var Key in LoadExcel.m_ExcelData.SheetDataHash.Keys)
        {
            str[++Index] = "public class C" + Key;
            CSheetData SheetData = (CSheetData)LoadExcel.m_ExcelData.SheetDataHash[Key];

            str[++Index] = "{";

            for(int i=0; i< SheetData.TypeArray.Count; ++i)
            {
                if (TempStr != (string)SheetData.VarArray[i])
                {
                    str[++Index] = "public ";

                    if (SheetData.TypeArray[i].ToString().Contains("ArrayList"))
                        str[Index] += "ArrayList";
                    else
                        str[Index] += (string)SheetData.TypeArray[i];

                    str[Index] += " ";
                    str[Index] += (string)SheetData.VarArray[i];
                    str[Index] += ";";
                    TempStr = (string)SheetData.VarArray[i];
                }
            }

            ++Index; // new Line

            int ArrayCount = 0;
            str[++Index] = "public void Load(BinaryReader BinaryReader)";
            str[++Index] = "{";

            for (int i = 0; i < SheetData.TypeArray.Count; ++i)
            {
                if (TempStr != (string)SheetData.VarArray[i])
                {
                    if(0 < ArrayCount)
                    {
                        str[++Index] = "for(int i=0; i<" + Convert.ToString(ArrayCount+1) + "; ++i)";
                        str[++Index] = "{";
                        str[++Index] += ((string)SheetData.VarArray[i-1] + "[i] = ");
                        if (SheetData.TypeArray[i - 1].ToString().Contains("string"))
                        {
                            str[Index] += " BinaryReader.ReadString();";
                        }
                        else if (SheetData.TypeArray[i - 1].ToString().Contains("int"))
                        {
                            str[Index] += " BinaryReader.ReadInt32();";
                        }
                        else if (SheetData.TypeArray[i - 1].ToString().Contains("float"))
                        {
                            str[Index] += " BinaryReader.ReadSingle();";
                        }
                        str[++Index] = "}";
                    }

                    ArrayCount = 0;
                    if (!SheetData.TypeArray[i].ToString().Contains("ArrayList"))
                    {
                        str[++Index] += ((string)SheetData.VarArray[i] + " = ");
                        if ((string)SheetData.TypeArray[i] == "string")
                        {
                            str[Index] += " BinaryReader.ReadString();";
                        }
                        else if ((string)SheetData.TypeArray[i] == "int")
                        {
                            str[Index] += " BinaryReader.ReadInt32();";
                        }
                        else if ((string)SheetData.TypeArray[i] == "float")
                        {
                            str[Index] += " BinaryReader.ReadSingle();";
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
                str[++Index] = "for(int i=0; i<" + Convert.ToString(ArrayCount+1) + "; ++i)";
                str[++Index] = "{";
                str[++Index] += ((string)SheetData.VarArray[SheetData.TypeArray.Count - 1] + "[i] = ");
                if (SheetData.TypeArray[SheetData.TypeArray.Count - 1].ToString().Contains("string"))
                {
                    str[Index] += " BinaryReader.ReadString();";
                }
                else if (SheetData.TypeArray[SheetData.TypeArray.Count - 1].ToString().Contains("int"))
                {
                    str[Index] += " BinaryReader.ReadInt32();";
                }
                else if (SheetData.TypeArray[SheetData.TypeArray.Count - 1].ToString().Contains("float"))
                {
                    str[Index] += " BinaryReader.ReadSingle();";
                }
                str[++Index] = "}";
            }

           // str[++Index] = "}"; // loop End

            str[++Index] = "}";

            str[++Index] = "};";
            ++Index; // new Line
        }

        str[++Index] = "public class C" + FileName;
        str[++Index] = "{";
        foreach (var Key in LoadExcel.m_ExcelData.SheetDataHash.Keys)
        {
            CSheetData SheetData = (CSheetData)LoadExcel.m_ExcelData.SheetDataHash[Key];
            str[++Index] = "public C" + Key + "[] " + Key + " = new C" + Key + "[" + Convert.ToString(SheetData.IndexCount) + "]" + ";";

        }

        str[++Index] = "public void Load(TextAsset InTextAsset)";
        str[++Index] = "{";
        str[++Index] = "    Stream kStream = new MemoryStream(InTextAsset.bytes);";
        str[++Index] = "    BinaryReader BinaryReader = new BinaryReader(kStream);";

        foreach (var Key in LoadExcel.m_ExcelData.SheetDataHash.Keys)
        {
            CSheetData SheetData = (CSheetData)LoadExcel.m_ExcelData.SheetDataHash[Key];
            str[++Index] = "for(int i=0; i<" + Convert.ToString(SheetData.IndexCount) + "; ++i)";
            str[++Index] = "{";
            str[++Index] = " " + Key + "[i] = new C" + Key + "();";
            str[++Index] = " " + Key + "[i].Load(BinaryReader);";
            str[++Index] = "}";
        }
        str[++Index] = "}";


        str[++Index] = "};";

        File.WriteAllLines(TempFileName, str);
    }
}
