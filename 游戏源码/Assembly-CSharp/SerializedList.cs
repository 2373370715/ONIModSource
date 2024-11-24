using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using KSerialization;

// Token: 0x02001E00 RID: 7680
[SerializationConfig(MemberSerialization.OptIn)]
public class SerializedList<ItemType>
{
	// Token: 0x17000A6C RID: 2668
	// (get) Token: 0x0600A0C6 RID: 41158 RVA: 0x0010886A File Offset: 0x00106A6A
	public int Count
	{
		get
		{
			return this.items.Count;
		}
	}

	// Token: 0x0600A0C7 RID: 41159 RVA: 0x00108877 File Offset: 0x00106A77
	public IEnumerator<ItemType> GetEnumerator()
	{
		return this.items.GetEnumerator();
	}

	// Token: 0x17000A6D RID: 2669
	public ItemType this[int idx]
	{
		get
		{
			return this.items[idx];
		}
	}

	// Token: 0x0600A0C9 RID: 41161 RVA: 0x00108897 File Offset: 0x00106A97
	public void Add(ItemType item)
	{
		this.items.Add(item);
	}

	// Token: 0x0600A0CA RID: 41162 RVA: 0x001088A5 File Offset: 0x00106AA5
	public void Remove(ItemType item)
	{
		this.items.Remove(item);
	}

	// Token: 0x0600A0CB RID: 41163 RVA: 0x001088B4 File Offset: 0x00106AB4
	public void RemoveAt(int idx)
	{
		this.items.RemoveAt(idx);
	}

	// Token: 0x0600A0CC RID: 41164 RVA: 0x001088C2 File Offset: 0x00106AC2
	public bool Contains(ItemType item)
	{
		return this.items.Contains(item);
	}

	// Token: 0x0600A0CD RID: 41165 RVA: 0x001088D0 File Offset: 0x00106AD0
	public void Clear()
	{
		this.items.Clear();
	}

	// Token: 0x0600A0CE RID: 41166 RVA: 0x003D6148 File Offset: 0x003D4348
	[OnSerializing]
	private void OnSerializing()
	{
		MemoryStream memoryStream = new MemoryStream();
		BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
		binaryWriter.Write(this.items.Count);
		foreach (ItemType itemType in this.items)
		{
			binaryWriter.WriteKleiString(itemType.GetType().FullName);
			long position = binaryWriter.BaseStream.Position;
			binaryWriter.Write(0);
			long position2 = binaryWriter.BaseStream.Position;
			Serializer.SerializeTypeless(itemType, binaryWriter);
			long position3 = binaryWriter.BaseStream.Position;
			long num = position3 - position2;
			binaryWriter.BaseStream.Position = position;
			binaryWriter.Write((int)num);
			binaryWriter.BaseStream.Position = position3;
		}
		memoryStream.Flush();
		this.serializationBuffer = memoryStream.ToArray();
	}

	// Token: 0x0600A0CF RID: 41167 RVA: 0x003D6248 File Offset: 0x003D4448
	[OnDeserialized]
	private void OnDeserialized()
	{
		if (this.serializationBuffer == null)
		{
			return;
		}
		FastReader fastReader = new FastReader(this.serializationBuffer);
		int num = fastReader.ReadInt32();
		for (int i = 0; i < num; i++)
		{
			string text = fastReader.ReadKleiString();
			int num2 = fastReader.ReadInt32();
			int position = fastReader.Position;
			Type type = Type.GetType(text);
			if (type == null)
			{
				DebugUtil.LogWarningArgs(new object[]
				{
					"Type no longer exists: " + text
				});
				fastReader.SkipBytes(num2);
			}
			else
			{
				ItemType itemType;
				if (typeof(ItemType) != type)
				{
					itemType = (ItemType)((object)Activator.CreateInstance(type));
				}
				else
				{
					itemType = default(ItemType);
				}
				Deserializer.DeserializeTypeless(itemType, fastReader);
				if (fastReader.Position != position + num2)
				{
					DebugUtil.LogWarningArgs(new object[]
					{
						"Expected to be at offset",
						position + num2,
						"but was only at offset",
						fastReader.Position,
						". Skipping to catch up."
					});
					fastReader.SkipBytes(position + num2 - fastReader.Position);
				}
				this.items.Add(itemType);
			}
		}
	}

	// Token: 0x04007D94 RID: 32148
	[Serialize]
	private byte[] serializationBuffer;

	// Token: 0x04007D95 RID: 32149
	private List<ItemType> items = new List<ItemType>();
}
