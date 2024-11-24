﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using KSerialization;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/SaveManager")]
public class SaveManager : KMonoBehaviour
{
			public event Action<SaveLoadRoot> onRegister;

			public event Action<SaveLoadRoot> onUnregister;

	protected override void OnPrefabInit()
	{
		Assets.RegisterOnAddPrefab(new Action<KPrefabID>(this.OnAddPrefab));
	}

	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Assets.UnregisterOnAddPrefab(new Action<KPrefabID>(this.OnAddPrefab));
	}

	private void OnAddPrefab(KPrefabID prefab)
	{
		if (prefab == null)
		{
			return;
		}
		Tag saveLoadTag = prefab.GetSaveLoadTag();
		this.prefabMap[saveLoadTag] = prefab.gameObject;
	}

	public Dictionary<Tag, List<SaveLoadRoot>> GetLists()
	{
		return this.sceneObjects;
	}

	private List<SaveLoadRoot> GetSaveLoadRootList(SaveLoadRoot saver)
	{
		KPrefabID component = saver.GetComponent<KPrefabID>();
		if (component == null)
		{
			DebugUtil.LogErrorArgs(saver.gameObject, new object[]
			{
				"All savers must also have a KPrefabID on them but",
				saver.gameObject.name,
				"does not have one."
			});
			return null;
		}
		List<SaveLoadRoot> list;
		if (!this.sceneObjects.TryGetValue(component.GetSaveLoadTag(), out list))
		{
			list = new List<SaveLoadRoot>();
			this.sceneObjects[component.GetSaveLoadTag()] = list;
		}
		return list;
	}

	public void Register(SaveLoadRoot root)
	{
		List<SaveLoadRoot> saveLoadRootList = this.GetSaveLoadRootList(root);
		if (saveLoadRootList == null)
		{
			return;
		}
		saveLoadRootList.Add(root);
		if (this.onRegister != null)
		{
			this.onRegister(root);
		}
	}

	public void Unregister(SaveLoadRoot root)
	{
		if (this.onRegister != null)
		{
			this.onUnregister(root);
		}
		List<SaveLoadRoot> saveLoadRootList = this.GetSaveLoadRootList(root);
		if (saveLoadRootList == null)
		{
			return;
		}
		saveLoadRootList.Remove(root);
	}

	public GameObject GetPrefab(Tag tag)
	{
		GameObject result = null;
		if (this.prefabMap.TryGetValue(tag, out result))
		{
			return result;
		}
		DebugUtil.LogArgs(new object[]
		{
			"Item not found in prefabMap",
			"[" + tag.Name + "]"
		});
		return null;
	}

	public void Save(BinaryWriter writer)
	{
		writer.Write(SaveManager.SAVE_HEADER);
		writer.Write(7);
		writer.Write(34);
		int num = 0;
		foreach (KeyValuePair<Tag, List<SaveLoadRoot>> keyValuePair in this.sceneObjects)
		{
			if (keyValuePair.Value.Count > 0)
			{
				num++;
			}
		}
		writer.Write(num);
		this.orderedKeys.Clear();
		this.orderedKeys.AddRange(this.sceneObjects.Keys);
		this.orderedKeys.Remove(SaveGame.Instance.PrefabID());
		this.orderedKeys = (from a in this.orderedKeys
		orderby a.Name == "StickerBomb"
		select a).ToList<Tag>();
		this.orderedKeys = (from a in this.orderedKeys
		orderby a.Name.Contains("UnderConstruction")
		select a).ToList<Tag>();
		this.Write(SaveGame.Instance.PrefabID(), new List<SaveLoadRoot>(new SaveLoadRoot[]
		{
			SaveGame.Instance.GetComponent<SaveLoadRoot>()
		}), writer);
		foreach (Tag key in this.orderedKeys)
		{
			List<SaveLoadRoot> list = this.sceneObjects[key];
			if (list.Count > 0)
			{
				foreach (SaveLoadRoot saveLoadRoot in list)
				{
					if (!(saveLoadRoot == null) && saveLoadRoot.GetComponent<SimCellOccupier>() != null)
					{
						this.Write(key, list, writer);
						break;
					}
				}
			}
		}
		foreach (Tag key2 in this.orderedKeys)
		{
			List<SaveLoadRoot> list2 = this.sceneObjects[key2];
			if (list2.Count > 0)
			{
				foreach (SaveLoadRoot saveLoadRoot2 in list2)
				{
					if (!(saveLoadRoot2 == null) && saveLoadRoot2.GetComponent<SimCellOccupier>() == null)
					{
						this.Write(key2, list2, writer);
						break;
					}
				}
			}
		}
	}

	private void Write(Tag key, List<SaveLoadRoot> value, BinaryWriter writer)
	{
		int count = value.Count;
		Tag tag = key;
		writer.WriteKleiString(tag.Name);
		writer.Write(count);
		long position = writer.BaseStream.Position;
		int value2 = -1;
		writer.Write(value2);
		long position2 = writer.BaseStream.Position;
		foreach (SaveLoadRoot saveLoadRoot in value)
		{
			if (saveLoadRoot != null)
			{
				saveLoadRoot.Save(writer);
			}
			else
			{
				DebugUtil.LogWarningArgs(new object[]
				{
					"Null game object when saving"
				});
			}
		}
		long position3 = writer.BaseStream.Position;
		long num = position3 - position2;
		writer.BaseStream.Position = position;
		writer.Write((int)num);
		writer.BaseStream.Position = position3;
	}

	public bool Load(IReader reader)
	{
		char[] array = reader.ReadChars(SaveManager.SAVE_HEADER.Length);
		if (array == null || array.Length != SaveManager.SAVE_HEADER.Length)
		{
			return false;
		}
		for (int i = 0; i < SaveManager.SAVE_HEADER.Length; i++)
		{
			if (array[i] != SaveManager.SAVE_HEADER[i])
			{
				return false;
			}
		}
		int num = reader.ReadInt32();
		int num2 = reader.ReadInt32();
		if (num != 7 || num2 > 34)
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				string.Format("SAVE FILE VERSION MISMATCH! Expected {0}.{1} but got {2}.{3}", new object[]
				{
					7,
					34,
					num,
					num2
				})
			});
			return false;
		}
		this.ClearScene();
		try
		{
			int num3 = reader.ReadInt32();
			for (int j = 0; j < num3; j++)
			{
				string text = reader.ReadKleiString();
				int num4 = reader.ReadInt32();
				int length = reader.ReadInt32();
				Tag key = TagManager.Create(text);
				GameObject prefab;
				if (!this.prefabMap.TryGetValue(key, out prefab))
				{
					DebugUtil.LogWarningArgs(new object[]
					{
						"Could not find prefab '" + text + "'"
					});
					reader.SkipBytes(length);
				}
				else
				{
					List<SaveLoadRoot> value = new List<SaveLoadRoot>(num4);
					this.sceneObjects[key] = value;
					for (int k = 0; k < num4; k++)
					{
						SaveLoadRoot x = SaveLoadRoot.Load(prefab, reader);
						if (SaveManager.DEBUG_OnlyLoadThisCellsObjects == -1 && x == null)
						{
							global::Debug.LogError("Error loading data [" + text + "]");
							return false;
						}
					}
				}
			}
		}
		catch (Exception ex)
		{
			DebugUtil.LogErrorArgs(new object[]
			{
				"Error deserializing prefabs\n\n",
				ex.ToString()
			});
			throw ex;
		}
		return true;
	}

	private void ClearScene()
	{
		foreach (KeyValuePair<Tag, List<SaveLoadRoot>> keyValuePair in this.sceneObjects)
		{
			foreach (SaveLoadRoot saveLoadRoot in keyValuePair.Value)
			{
				UnityEngine.Object.Destroy(saveLoadRoot.gameObject);
			}
		}
		this.sceneObjects.Clear();
	}

	public const int SAVE_MAJOR_VERSION_LAST_UNDOCUMENTED = 7;

	public const int SAVE_MAJOR_VERSION = 7;

	public const int SAVE_MINOR_VERSION_EXPLICIT_VALUE_TYPES = 4;

	public const int SAVE_MINOR_VERSION_LAST_UNDOCUMENTED = 7;

	public const int SAVE_MINOR_VERSION_MOD_IDENTIFIER = 8;

	public const int SAVE_MINOR_VERSION_FINITE_SPACE_RESOURCES = 9;

	public const int SAVE_MINOR_VERSION_COLONY_REQ_ACHIEVEMENTS = 10;

	public const int SAVE_MINOR_VERSION_TRACK_NAV_DISTANCE = 11;

	public const int SAVE_MINOR_VERSION_EXPANDED_WORLD_INFO = 12;

	public const int SAVE_MINOR_VERSION_BASIC_COMFORTS_FIX = 13;

	public const int SAVE_MINOR_VERSION_PLATFORM_TRAIT_NAMES = 14;

	public const int SAVE_MINOR_VERSION_ADD_JOY_REACTIONS = 15;

	public const int SAVE_MINOR_VERSION_NEW_AUTOMATION_WARNING = 16;

	public const int SAVE_MINOR_VERSION_ADD_GUID_TO_HEADER = 17;

	public const int SAVE_MINOR_VERSION_EXPANSION_1_INTRODUCED = 20;

	public const int SAVE_MINOR_VERSION_CONTENT_SETTINGS = 21;

	public const int SAVE_MINOR_VERSION_COLONY_REQ_REMOVE_SERIALIZATION = 22;

	public const int SAVE_MINOR_VERSION_ROTTABLE_TUNING = 23;

	public const int SAVE_MINOR_VERSION_LAUNCH_PAD_SOLIDITY = 24;

	public const int SAVE_MINOR_VERSION_BASE_GAME_MERGEDOWN = 25;

	public const int SAVE_MINOR_VERSION_FALLING_WATER_WORLDIDX_SERIALIZATION = 26;

	public const int SAVE_MINOR_VERSION_ROCKET_RANGE_REBALANCE = 27;

	public const int SAVE_MINOR_VERSION_ENTITIES_WRONG_LAYER = 28;

	public const int SAVE_MINOR_VERSION_TAGBITS_REWORK = 29;

	public const int SAVE_MINOR_VERSION_ACCESSORY_SLOT_UPGRADE = 30;

	public const int SAVE_MINOR_VERSION_GEYSER_CAN_BE_RENAMED = 31;

	public const int SAVE_MINOR_VERSION_SPACE_SCANNERS_TELESCOPES = 32;

	public const int SAVE_MINOR_VERSION_U50_CRITTERS = 33;

	public const int SAVE_MINOR_VERSION_DLC_ADD_ONS = 34;

	public const int SAVE_MINOR_VERSION = 34;

	private Dictionary<Tag, GameObject> prefabMap = new Dictionary<Tag, GameObject>();

	private Dictionary<Tag, List<SaveLoadRoot>> sceneObjects = new Dictionary<Tag, List<SaveLoadRoot>>();

	public static int DEBUG_OnlyLoadThisCellsObjects = -1;

	private static readonly char[] SAVE_HEADER = new char[]
	{
		'K',
		'S',
		'A',
		'V'
	};

	private List<Tag> orderedKeys = new List<Tag>();

	private enum BoundaryTag : uint
	{
		Component = 3735928559U,
		Prefab = 3131961357U,
		Complete = 3735929054U
	}
}
