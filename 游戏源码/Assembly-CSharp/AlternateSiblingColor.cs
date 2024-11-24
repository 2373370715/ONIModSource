using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001BD9 RID: 7129
public class AlternateSiblingColor : KMonoBehaviour
{
	// Token: 0x06009451 RID: 37969 RVA: 0x003952C8 File Offset: 0x003934C8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		int siblingIndex = base.transform.GetSiblingIndex();
		this.RefreshColor(siblingIndex % 2 == 0);
	}

	// Token: 0x06009452 RID: 37970 RVA: 0x0010096B File Offset: 0x000FEB6B
	private void RefreshColor(bool evenIndex)
	{
		if (this.image == null)
		{
			return;
		}
		this.image.color = (evenIndex ? this.evenColor : this.oddColor);
	}

	// Token: 0x06009453 RID: 37971 RVA: 0x00100998 File Offset: 0x000FEB98
	private void Update()
	{
		if (this.mySiblingIndex != base.transform.GetSiblingIndex())
		{
			this.mySiblingIndex = base.transform.GetSiblingIndex();
			this.RefreshColor(this.mySiblingIndex % 2 == 0);
		}
	}

	// Token: 0x04007315 RID: 29461
	public Color evenColor;

	// Token: 0x04007316 RID: 29462
	public Color oddColor;

	// Token: 0x04007317 RID: 29463
	public Image image;

	// Token: 0x04007318 RID: 29464
	private int mySiblingIndex;
}
