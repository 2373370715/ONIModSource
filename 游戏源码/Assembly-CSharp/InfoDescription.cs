using System;
using UnityEngine;

// Token: 0x02001D01 RID: 7425
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/InfoDescription")]
public class InfoDescription : KMonoBehaviour
{
	// Token: 0x17000A3D RID: 2621
	// (get) Token: 0x06009AF4 RID: 39668 RVA: 0x00104CBD File Offset: 0x00102EBD
	// (set) Token: 0x06009AF3 RID: 39667 RVA: 0x00104C96 File Offset: 0x00102E96
	public string DescriptionLocString
	{
		get
		{
			return this.descriptionLocString;
		}
		set
		{
			this.descriptionLocString = value;
			if (this.descriptionLocString != null)
			{
				this.description = Strings.Get(this.descriptionLocString);
			}
		}
	}

	// Token: 0x06009AF5 RID: 39669 RVA: 0x003BC994 File Offset: 0x003BAB94
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (!string.IsNullOrEmpty(this.nameLocString))
		{
			this.displayName = Strings.Get(this.nameLocString);
		}
		if (!string.IsNullOrEmpty(this.descriptionLocString))
		{
			this.description = Strings.Get(this.descriptionLocString);
		}
	}

	// Token: 0x0400791F RID: 31007
	public string nameLocString = "";

	// Token: 0x04007920 RID: 31008
	private string descriptionLocString = "";

	// Token: 0x04007921 RID: 31009
	public string description;

	// Token: 0x04007922 RID: 31010
	public string effect = "";

	// Token: 0x04007923 RID: 31011
	public string displayName;
}
