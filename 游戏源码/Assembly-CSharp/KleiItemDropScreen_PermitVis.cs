using System;
using System.Collections;
using UnityEngine;

// Token: 0x02001D4A RID: 7498
public class KleiItemDropScreen_PermitVis : KMonoBehaviour
{
	// Token: 0x06009C9B RID: 40091 RVA: 0x003C4F78 File Offset: 0x003C3178
	public void ConfigureWith(DropScreenPresentationInfo info)
	{
		this.ResetState();
		this.equipmentVis.gameObject.SetActive(false);
		this.fallbackVis.gameObject.SetActive(false);
		if (info.UseEquipmentVis)
		{
			this.equipmentVis.gameObject.SetActive(true);
			this.equipmentVis.ConfigureWith(info);
			return;
		}
		this.fallbackVis.gameObject.SetActive(true);
		this.fallbackVis.ConfigureWith(info);
	}

	// Token: 0x06009C9C RID: 40092 RVA: 0x00105ED1 File Offset: 0x001040D1
	public Promise AnimateIn()
	{
		return Updater.RunRoutine(this, this.AnimateInRoutine());
	}

	// Token: 0x06009C9D RID: 40093 RVA: 0x00105EDF File Offset: 0x001040DF
	public Promise AnimateOut()
	{
		return Updater.RunRoutine(this, this.AnimateOutRoutine());
	}

	// Token: 0x06009C9E RID: 40094 RVA: 0x00105EED File Offset: 0x001040ED
	private IEnumerator AnimateInRoutine()
	{
		this.root.gameObject.SetActive(true);
		yield return Updater.Ease(delegate(Vector3 v3)
		{
			this.root.transform.localScale = v3;
		}, this.root.transform.localScale, Vector3.one, 0.5f, Easing.EaseOutBack, -1f);
		yield break;
	}

	// Token: 0x06009C9F RID: 40095 RVA: 0x00105EFC File Offset: 0x001040FC
	private IEnumerator AnimateOutRoutine()
	{
		yield return Updater.Ease(delegate(Vector3 v3)
		{
			this.root.transform.localScale = v3;
		}, this.root.transform.localScale, Vector3.zero, 0.25f, null, -1f);
		this.root.gameObject.SetActive(true);
		yield break;
	}

	// Token: 0x06009CA0 RID: 40096 RVA: 0x00105F0B File Offset: 0x0010410B
	public void ResetState()
	{
		this.root.transform.localScale = Vector3.zero;
	}

	// Token: 0x04007AD2 RID: 31442
	[SerializeField]
	private RectTransform root;

	// Token: 0x04007AD3 RID: 31443
	[Header("Different Permit Visualizers")]
	[SerializeField]
	private KleiItemDropScreen_PermitVis_Fallback fallbackVis;

	// Token: 0x04007AD4 RID: 31444
	[SerializeField]
	private KleiItemDropScreen_PermitVis_DupeEquipment equipmentVis;
}
