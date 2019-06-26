using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class CEnemy
{
	public int Index;
	public string Prefab;

	public void Load(BinaryReader BinaryReader)
	{
		Index = BinaryReader.ReadInt32();
		Prefab = BinaryReader.ReadString();
	}
};

public class CPlayer
{
	public int Index;
	public string Prefab;

	public void Load(BinaryReader BinaryReader)
	{
		Index = BinaryReader.ReadInt32();
		Prefab = BinaryReader.ReadString();
	}
};

public class CEnemySheet
{
	public CEnemy[] vec = new CEnemy[4];
	public Dictionary<int, CEnemy> map = new Dictionary<int, CEnemy>();
};
public class CPlayerSheet
{
	public CPlayer[] vec = new CPlayer[4];
	public Dictionary<int, CPlayer> map = new Dictionary<int, CPlayer>();
};
public class CCharacterTable
{
	public CEnemySheet EnemySheet = new CEnemySheet();
	public CPlayerSheet PlayerSheet = new CPlayerSheet();

	public void Load()
	{
		TextAsset Asset = Resources.Load<TextAsset>("TextAssets/Tables/CharacterTable");
		Stream stream = new MemoryStream(Asset.bytes);
		BinaryReader BinaryReader = new BinaryReader(stream);
		for(int i=0; i<4; ++i)
		{
			EnemySheet.vec[i] = new CEnemy();
			EnemySheet.vec[i].Load(BinaryReader);
			int key =EnemySheet.vec[i].Index;
			EnemySheet.map.Add(key, EnemySheet.vec[i]); 
		}
		for(int i=0; i<4; ++i)
		{
			PlayerSheet.vec[i] = new CPlayer();
			PlayerSheet.vec[i].Load(BinaryReader);
			int key =PlayerSheet.vec[i].Index;
			PlayerSheet.map.Add(key, PlayerSheet.vec[i]); 
		}
	}
};



































