using System;
using STRINGS;
using UnityEngine;

// Token: 0x02001F6B RID: 8043
[AddComponentMenu("KMonoBehaviour/scripts/FilterSideScreenRow")]
public class FilterSideScreenRow : SingleItemSelectionRow
{
	// Token: 0x17000AD0 RID: 2768
	// (get) Token: 0x0600A9BC RID: 43452 RVA: 0x0010E415 File Offset: 0x0010C615
	public override string InvalidTagTitle
	{
		get
		{
			return UI.UISIDESCREENS.FILTERSIDESCREEN.NO_SELECTION;
		}
	}

	// Token: 0x0600A9BD RID: 43453 RVA: 0x0010E421 File Offset: 0x0010C621
	protected override void SetIcon(Sprite sprite, Color color)
	{
		if (this.icon != null)
		{
			this.icon.gameObject.SetActive(false);
		}
	}
}
