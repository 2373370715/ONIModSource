using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

// Token: 0x02000AB1 RID: 2737
public class OldNoteEntriesV5
{
	// Token: 0x06003300 RID: 13056 RVA: 0x00204D70 File Offset: 0x00202F70
	public void Deserialize(BinaryReader reader)
	{
		int num = reader.ReadInt32();
		for (int i = 0; i < num; i++)
		{
			OldNoteEntriesV5.NoteStorageBlock item = default(OldNoteEntriesV5.NoteStorageBlock);
			item.Deserialize(reader);
			this.storageBlocks.Add(item);
		}
	}

	// Token: 0x04002252 RID: 8786
	public List<OldNoteEntriesV5.NoteStorageBlock> storageBlocks = new List<OldNoteEntriesV5.NoteStorageBlock>();

	// Token: 0x02000AB2 RID: 2738
	[StructLayout(LayoutKind.Explicit)]
	public struct NoteEntry
	{
		// Token: 0x04002253 RID: 8787
		[FieldOffset(0)]
		public int reportEntryId;

		// Token: 0x04002254 RID: 8788
		[FieldOffset(4)]
		public int noteHash;

		// Token: 0x04002255 RID: 8789
		[FieldOffset(8)]
		public float value;
	}

	// Token: 0x02000AB3 RID: 2739
	[StructLayout(LayoutKind.Explicit)]
	public struct NoteEntryArray
	{
		// Token: 0x1700021B RID: 539
		// (get) Token: 0x06003302 RID: 13058 RVA: 0x000C1441 File Offset: 0x000BF641
		public int StructSizeInBytes
		{
			get
			{
				return Marshal.SizeOf(typeof(OldNoteEntriesV5.NoteEntry));
			}
		}

		// Token: 0x04002256 RID: 8790
		[FieldOffset(0)]
		public byte[] bytes;

		// Token: 0x04002257 RID: 8791
		[FieldOffset(0)]
		public OldNoteEntriesV5.NoteEntry[] structs;
	}

	// Token: 0x02000AB4 RID: 2740
	public struct NoteStorageBlock
	{
		// Token: 0x06003303 RID: 13059 RVA: 0x000C1452 File Offset: 0x000BF652
		public void Deserialize(BinaryReader reader)
		{
			this.entryCount = reader.ReadInt32();
			this.entries.bytes = reader.ReadBytes(this.entries.StructSizeInBytes * this.entryCount);
		}

		// Token: 0x04002258 RID: 8792
		public int entryCount;

		// Token: 0x04002259 RID: 8793
		public OldNoteEntriesV5.NoteEntryArray entries;
	}
}
