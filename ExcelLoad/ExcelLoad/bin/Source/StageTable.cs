using System;
using System.IO;
using ArrayList = System.Collections.ArrayList;
using UnityEngine;

public class CStageInfo
{
public int Index;
public int Enemy;
public int Boss;

public void Load(BinaryReader BinaryReader)
{
Index =  BinaryReader.ReadInt32();
Enemy =  BinaryReader.ReadInt32();
Boss =  BinaryReader.ReadInt32();
}
};

public class CStageTable
{
public CStageInfo[] StageInfo = new CStageInfo[12];
public void Load(TextAsset InTextAsset)
{
    Stream kStream = new MemoryStream(InTextAsset.bytes);
    BinaryReader BinaryReader = new BinaryReader(kStream);
for(int i=0; i<12; ++i)
{
 StageInfo[i] = new CStageInfo();
 StageInfo[i].Load(BinaryReader);
}
}
};



































































