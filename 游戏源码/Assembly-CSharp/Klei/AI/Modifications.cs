using System;
using System.Collections.Generic;
using System.IO;
using KSerialization;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02003B7C RID: 15228
	[SerializationConfig(MemberSerialization.OptIn)]
	public class Modifications<ModifierType, InstanceType> : ISaveLoadableDetails where ModifierType : Resource where InstanceType : ModifierInstance<ModifierType>
	{
		// Token: 0x17000C28 RID: 3112
		// (get) Token: 0x0600EA69 RID: 60009 RVA: 0x0013CB04 File Offset: 0x0013AD04
		public int Count
		{
			get
			{
				return this.ModifierList.Count;
			}
		}

		// Token: 0x0600EA6A RID: 60010 RVA: 0x0013CB11 File Offset: 0x0013AD11
		public IEnumerator<InstanceType> GetEnumerator()
		{
			return this.ModifierList.GetEnumerator();
		}

		// Token: 0x17000C29 RID: 3113
		// (get) Token: 0x0600EA6B RID: 60011 RVA: 0x0013CB23 File Offset: 0x0013AD23
		// (set) Token: 0x0600EA6C RID: 60012 RVA: 0x0013CB2B File Offset: 0x0013AD2B
		public GameObject gameObject { get; private set; }

		// Token: 0x17000C2A RID: 3114
		public InstanceType this[int idx]
		{
			get
			{
				return this.ModifierList[idx];
			}
		}

		// Token: 0x0600EA6E RID: 60014 RVA: 0x0013CB42 File Offset: 0x0013AD42
		public ComponentType GetComponent<ComponentType>()
		{
			return this.gameObject.GetComponent<ComponentType>();
		}

		// Token: 0x0600EA6F RID: 60015 RVA: 0x0013CB4F File Offset: 0x0013AD4F
		public void Trigger(GameHashes hash, object data = null)
		{
			this.gameObject.GetComponent<KPrefabID>().Trigger((int)hash, data);
		}

		// Token: 0x0600EA70 RID: 60016 RVA: 0x004C9FE4 File Offset: 0x004C81E4
		public virtual InstanceType CreateInstance(ModifierType modifier)
		{
			return default(InstanceType);
		}

		// Token: 0x0600EA71 RID: 60017 RVA: 0x0013CB63 File Offset: 0x0013AD63
		public Modifications(GameObject go, ResourceSet<ModifierType> resources = null)
		{
			this.resources = resources;
			this.gameObject = go;
		}

		// Token: 0x0600EA72 RID: 60018 RVA: 0x0013CB84 File Offset: 0x0013AD84
		public virtual InstanceType Add(InstanceType instance)
		{
			this.ModifierList.Add(instance);
			return instance;
		}

		// Token: 0x0600EA73 RID: 60019 RVA: 0x004C9FFC File Offset: 0x004C81FC
		public virtual void Remove(InstanceType instance)
		{
			for (int i = 0; i < this.ModifierList.Count; i++)
			{
				if (this.ModifierList[i] == instance)
				{
					this.ModifierList.RemoveAt(i);
					instance.OnCleanUp();
					return;
				}
			}
		}

		// Token: 0x0600EA74 RID: 60020 RVA: 0x0013CB93 File Offset: 0x0013AD93
		public bool Has(ModifierType modifier)
		{
			return this.Get(modifier) != null;
		}

		// Token: 0x0600EA75 RID: 60021 RVA: 0x004CA050 File Offset: 0x004C8250
		public InstanceType Get(ModifierType modifier)
		{
			foreach (InstanceType instanceType in this.ModifierList)
			{
				if (instanceType.modifier == modifier)
				{
					return instanceType;
				}
			}
			return default(InstanceType);
		}

		// Token: 0x0600EA76 RID: 60022 RVA: 0x004CA0C4 File Offset: 0x004C82C4
		public InstanceType Get(string id)
		{
			foreach (InstanceType instanceType in this.ModifierList)
			{
				if (instanceType.modifier.Id == id)
				{
					return instanceType;
				}
			}
			return default(InstanceType);
		}

		// Token: 0x0600EA77 RID: 60023 RVA: 0x004CA13C File Offset: 0x004C833C
		public void Serialize(BinaryWriter writer)
		{
			writer.Write(this.ModifierList.Count);
			foreach (InstanceType instanceType in this.ModifierList)
			{
				writer.WriteKleiString(instanceType.modifier.Id);
				long position = writer.BaseStream.Position;
				writer.Write(0);
				long position2 = writer.BaseStream.Position;
				Serializer.SerializeTypeless(instanceType, writer);
				long position3 = writer.BaseStream.Position;
				long num = position3 - position2;
				writer.BaseStream.Position = position;
				writer.Write((int)num);
				writer.BaseStream.Position = position3;
			}
		}

		// Token: 0x0600EA78 RID: 60024 RVA: 0x004CA21C File Offset: 0x004C841C
		public void Deserialize(IReader reader)
		{
			int num = reader.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				string text = reader.ReadKleiString();
				int num2 = reader.ReadInt32();
				int position = reader.Position;
				InstanceType instanceType = this.Get(text);
				if (instanceType == null && this.resources != null)
				{
					ModifierType modifierType = this.resources.TryGet(text);
					if (modifierType != null)
					{
						instanceType = this.CreateInstance(modifierType);
					}
				}
				if (instanceType == null)
				{
					if (text != "Condition")
					{
						DebugUtil.LogWarningArgs(new object[]
						{
							this.gameObject.name,
							"Missing modifier: " + text
						});
					}
					reader.SkipBytes(num2);
				}
				else if (!(instanceType is ISaveLoadable))
				{
					reader.SkipBytes(num2);
				}
				else
				{
					Deserializer.DeserializeTypeless(instanceType, reader);
					if (reader.Position != position + num2)
					{
						DebugUtil.LogWarningArgs(new object[]
						{
							"Expected to be at offset",
							position + num2,
							"but was only at offset",
							reader.Position,
							". Skipping to catch up."
						});
						reader.SkipBytes(position + num2 - reader.Position);
					}
				}
			}
		}

		// Token: 0x0400E5C3 RID: 58819
		public List<InstanceType> ModifierList = new List<InstanceType>();

		// Token: 0x0400E5C5 RID: 58821
		private ResourceSet<ModifierType> resources;
	}
}
