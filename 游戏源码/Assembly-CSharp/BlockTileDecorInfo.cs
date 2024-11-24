using System;
using Rendering;
using UnityEngine;

// Token: 0x02001A86 RID: 6790
public class BlockTileDecorInfo : ScriptableObject
{
	// Token: 0x06008DFB RID: 36347 RVA: 0x0036F6D8 File Offset: 0x0036D8D8
	public void PostProcess()
	{
		if (this.decor != null && this.atlas != null && this.atlas.items != null)
		{
			for (int i = 0; i < this.decor.Length; i++)
			{
				if (this.decor[i].variants != null && this.decor[i].variants.Length != 0)
				{
					for (int j = 0; j < this.decor[i].variants.Length; j++)
					{
						bool flag = false;
						foreach (TextureAtlas.Item item in this.atlas.items)
						{
							string text = item.name;
							int num = text.IndexOf("/");
							if (num != -1)
							{
								text = text.Substring(num + 1);
							}
							if (this.decor[i].variants[j].name == text)
							{
								this.decor[i].variants[j].atlasItem = item;
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							DebugUtil.LogErrorArgs(new object[]
							{
								base.name,
								"/",
								this.decor[i].name,
								"could not find ",
								this.decor[i].variants[j].name,
								"in",
								this.atlas.name
							});
						}
					}
				}
			}
		}
	}

	// Token: 0x04006AAB RID: 27307
	public TextureAtlas atlas;

	// Token: 0x04006AAC RID: 27308
	public TextureAtlas atlasSpec;

	// Token: 0x04006AAD RID: 27309
	public int sortOrder;

	// Token: 0x04006AAE RID: 27310
	public BlockTileDecorInfo.Decor[] decor;

	// Token: 0x02001A87 RID: 6791
	[Serializable]
	public struct ImageInfo
	{
		// Token: 0x04006AAF RID: 27311
		public string name;

		// Token: 0x04006AB0 RID: 27312
		public Vector3 offset;

		// Token: 0x04006AB1 RID: 27313
		[NonSerialized]
		public TextureAtlas.Item atlasItem;
	}

	// Token: 0x02001A88 RID: 6792
	[Serializable]
	public struct Decor
	{
		// Token: 0x04006AB2 RID: 27314
		public string name;

		// Token: 0x04006AB3 RID: 27315
		[EnumFlags]
		public BlockTileRenderer.Bits requiredConnections;

		// Token: 0x04006AB4 RID: 27316
		[EnumFlags]
		public BlockTileRenderer.Bits forbiddenConnections;

		// Token: 0x04006AB5 RID: 27317
		public float probabilityCutoff;

		// Token: 0x04006AB6 RID: 27318
		public BlockTileDecorInfo.ImageInfo[] variants;

		// Token: 0x04006AB7 RID: 27319
		public int sortOrder;
	}
}
