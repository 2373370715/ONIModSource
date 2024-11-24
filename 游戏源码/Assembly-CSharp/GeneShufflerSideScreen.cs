using System;
using STRINGS;
using UnityEngine;

// Token: 0x02001F6E RID: 8046
public class GeneShufflerSideScreen : SideScreenContent
{
	// Token: 0x0600A9C7 RID: 43463 RVA: 0x0010E4C0 File Offset: 0x0010C6C0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.button.onClick += this.OnButtonClick;
		this.Refresh();
	}

	// Token: 0x0600A9C8 RID: 43464 RVA: 0x0010E4E5 File Offset: 0x0010C6E5
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<GeneShuffler>() != null;
	}

	// Token: 0x0600A9C9 RID: 43465 RVA: 0x004029D0 File Offset: 0x00400BD0
	public override void SetTarget(GameObject target)
	{
		GeneShuffler component = target.GetComponent<GeneShuffler>();
		if (component == null)
		{
			global::Debug.LogError("Target doesn't have a GeneShuffler associated with it.");
			return;
		}
		this.target = component;
		this.Refresh();
	}

	// Token: 0x0600A9CA RID: 43466 RVA: 0x00402A08 File Offset: 0x00400C08
	private void OnButtonClick()
	{
		if (this.target.WorkComplete)
		{
			this.target.SetWorkTime(0f);
			return;
		}
		if (this.target.IsConsumed)
		{
			this.target.RequestRecharge(!this.target.RechargeRequested);
			this.Refresh();
		}
	}

	// Token: 0x0600A9CB RID: 43467 RVA: 0x00402A60 File Offset: 0x00400C60
	private void Refresh()
	{
		if (!(this.target != null))
		{
			this.contents.SetActive(false);
			return;
		}
		if (this.target.WorkComplete)
		{
			this.contents.SetActive(true);
			this.label.text = UI.UISIDESCREENS.GENESHUFFLERSIDESREEN.COMPLETE;
			this.button.gameObject.SetActive(true);
			this.buttonLabel.text = UI.UISIDESCREENS.GENESHUFFLERSIDESREEN.BUTTON;
			return;
		}
		if (this.target.IsConsumed)
		{
			this.contents.SetActive(true);
			this.button.gameObject.SetActive(true);
			if (this.target.RechargeRequested)
			{
				this.label.text = UI.UISIDESCREENS.GENESHUFFLERSIDESREEN.CONSUMED_WAITING;
				this.buttonLabel.text = UI.UISIDESCREENS.GENESHUFFLERSIDESREEN.BUTTON_RECHARGE_CANCEL;
				return;
			}
			this.label.text = UI.UISIDESCREENS.GENESHUFFLERSIDESREEN.CONSUMED;
			this.buttonLabel.text = UI.UISIDESCREENS.GENESHUFFLERSIDESREEN.BUTTON_RECHARGE;
			return;
		}
		else
		{
			if (this.target.IsWorking)
			{
				this.contents.SetActive(true);
				this.label.text = UI.UISIDESCREENS.GENESHUFFLERSIDESREEN.UNDERWAY;
				this.button.gameObject.SetActive(false);
				return;
			}
			this.contents.SetActive(false);
			return;
		}
	}

	// Token: 0x04008584 RID: 34180
	[SerializeField]
	private LocText label;

	// Token: 0x04008585 RID: 34181
	[SerializeField]
	private KButton button;

	// Token: 0x04008586 RID: 34182
	[SerializeField]
	private LocText buttonLabel;

	// Token: 0x04008587 RID: 34183
	[SerializeField]
	private GeneShuffler target;

	// Token: 0x04008588 RID: 34184
	[SerializeField]
	private GameObject contents;
}
