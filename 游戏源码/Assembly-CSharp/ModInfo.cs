using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

// Token: 0x02000496 RID: 1174
[Serializable]
public struct ModInfo
{
	// Token: 0x04000DD2 RID: 3538
	[JsonConverter(typeof(StringEnumConverter))]
	public ModInfo.Source source;

	// Token: 0x04000DD3 RID: 3539
	[JsonConverter(typeof(StringEnumConverter))]
	public ModInfo.ModType type;

	// Token: 0x04000DD4 RID: 3540
	public string assetID;

	// Token: 0x04000DD5 RID: 3541
	public string assetPath;

	// Token: 0x04000DD6 RID: 3542
	public bool enabled;

	// Token: 0x04000DD7 RID: 3543
	public bool markedForDelete;

	// Token: 0x04000DD8 RID: 3544
	public bool markedForUpdate;

	// Token: 0x04000DD9 RID: 3545
	public string description;

	// Token: 0x04000DDA RID: 3546
	public ulong lastModifiedTime;

	// Token: 0x02000497 RID: 1175
	public enum Source
	{
		// Token: 0x04000DDC RID: 3548
		Local,
		// Token: 0x04000DDD RID: 3549
		Steam,
		// Token: 0x04000DDE RID: 3550
		Rail
	}

	// Token: 0x02000498 RID: 1176
	public enum ModType
	{
		// Token: 0x04000DE0 RID: 3552
		WorldGen,
		// Token: 0x04000DE1 RID: 3553
		Scenario,
		// Token: 0x04000DE2 RID: 3554
		Mod
	}
}
