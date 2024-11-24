using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001FDC RID: 8156
public class SpecialCargoBayClusterSideScreen : ReceptacleSideScreen
{
	// Token: 0x0600ACD0 RID: 44240 RVA: 0x0010D160 File Offset: 0x0010B360
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x0600ACD1 RID: 44241 RVA: 0x001105F7 File Offset: 0x0010E7F7
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<SpecialCargoBayClusterReceptacle>() != null;
	}

	// Token: 0x0600ACD2 RID: 44242 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
	protected override bool RequiresAvailableAmountToDeposit()
	{
		return false;
	}

	// Token: 0x0600ACD3 RID: 44243 RVA: 0x00110605 File Offset: 0x0010E805
	protected override void UpdateState(object data)
	{
		base.UpdateState(data);
		this.SetDescriptionSidescreenFoldState(this.targetReceptacle != null && this.targetReceptacle.Occupant == null);
	}

	// Token: 0x0600ACD4 RID: 44244 RVA: 0x0040F74C File Offset: 0x0040D94C
	protected override void SetResultDescriptions(GameObject go)
	{
		base.SetResultDescriptions(go);
		if (this.targetReceptacle != null && this.targetReceptacle.Occupant != null)
		{
			this.descriptionLabel.SetText("");
			this.SetDescriptionSidescreenFoldState(false);
			return;
		}
		this.SetDescriptionSidescreenFoldState(true);
	}

	// Token: 0x0600ACD5 RID: 44245 RVA: 0x00110636 File Offset: 0x0010E836
	public void SetDescriptionSidescreenFoldState(bool visible)
	{
		this.descriptionContent.minHeight = (visible ? this.descriptionLayoutDefaultSize : 0f);
	}

	// Token: 0x040087A0 RID: 34720
	public LayoutElement descriptionContent;

	// Token: 0x040087A1 RID: 34721
	public float descriptionLayoutDefaultSize = -1f;
}
