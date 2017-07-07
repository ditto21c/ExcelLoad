using System;
using System.IO;
using ArrayList = System.Collections.ArrayList;
using UnityEngine;

public class CCharInfo
{
	public int Index;
	public string Prefab;
	public int CharType;
	public int HP;
	public int CharWidth;
	public float MoveSpeed;
	public int AttackDamage;
	public ArrayList Test1;
	public ArrayList Test2;
	public int AttackRange;
	public float AttackSpeed;
	public int Exp;
	public float AfterAttackIdleTermTime;
	public int Pay;
	public ArrayList Test;

	public void Load(BinaryReader BinaryReader)
	{
		Index = BinaryReader.ReadInt32();
		Prefab = BinaryReader.ReadString();
		CharType = BinaryReader.ReadInt32();
		HP = BinaryReader.ReadInt32();
		CharWidth = BinaryReader.ReadInt32();
		MoveSpeed = BinaryReader.ReadSingle();
		AttackDamage = BinaryReader.ReadInt32();
		for(int i=0; i<3; ++i)
		{
			Test1[i] = BinaryReader.ReadInt32();
		}
		for(int i=0; i<3; ++i)
		{
			Test2[i] = BinaryReader.ReadString();
		}
		AttackRange = BinaryReader.ReadInt32();
		AttackSpeed = BinaryReader.ReadSingle();
		Exp = BinaryReader.ReadInt32();
		AfterAttackIdleTermTime = BinaryReader.ReadSingle();
		Pay = BinaryReader.ReadInt32();
		for(int i=0; i<3; ++i)
		{
			Test[i] = BinaryReader.ReadInt32();
		}
	}
};

public class CCharacterTable
{
	public CCharInfo[] CharInfo = new CCharInfo[8];
	public void Load(TextAsset InTextAsset)
	{
		Stream kStream = new MemoryStream(InTextAsset.bytes);
		BinaryReader BinaryReader = new BinaryReader(kStream);
		for(int i=0; i<8; ++i)
		{
			CharInfo[i] = new CCharInfo();
			CharInfo[i].Load(BinaryReader);
		}
	}
};


































