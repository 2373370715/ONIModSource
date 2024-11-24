using System;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001FF1 RID: 8177
public class WarpPortalSideScreen : SideScreenContent
{
	// Token: 0x0600ADAB RID: 44459 RVA: 0x00412AA4 File Offset: 0x00410CA4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.buttonLabel.SetText(UI.UISIDESCREENS.WARPPORTALSIDESCREEN.BUTTON);
		this.cancelButtonLabel.SetText(UI.UISIDESCREENS.WARPPORTALSIDESCREEN.CANCELBUTTON);
		this.button.onClick += this.OnButtonClick;
		this.cancelButton.onClick += this.OnCancelClick;
		this.Refresh(null);
	}

	// Token: 0x0600ADAC RID: 44460 RVA: 0x0011112A File Offset: 0x0010F32A
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<WarpPortal>() != null;
	}

	// Token: 0x0600ADAD RID: 44461 RVA: 0x00412B18 File Offset: 0x00410D18
	public override void SetTarget(GameObject target)
	{
		WarpPortal component = target.GetComponent<WarpPortal>();
		if (component == null)
		{
			global::Debug.LogError("Target doesn't have a WarpPortal associated with it.");
			return;
		}
		this.target = component;
		target.GetComponent<Assignable>().OnAssign += new Action<IAssignableIdentity>(this.Refresh);
		this.Refresh(null);
	}

	// Token: 0x0600ADAE RID: 44462 RVA: 0x00412B68 File Offset: 0x00410D68
	private void Update()
	{
		if (this.progressBar.activeSelf)
		{
			RectTransform rectTransform = this.progressBar.GetComponentsInChildren<Image>()[1].rectTransform;
			float num = this.target.rechargeProgress / 3000f;
			rectTransform.sizeDelta = new Vector2(rectTransform.transform.parent.GetComponent<LayoutElement>().minWidth * num, 24f);
			this.progressLabel.text = GameUtil.GetFormattedPercent(num * 100f, GameUtil.TimeSlice.None);
		}
	}

	// Token: 0x0600ADAF RID: 44463 RVA: 0x00111138 File Offset: 0x0010F338
	private void OnButtonClick()
	{
		if (this.target.ReadyToWarp)
		{
			this.target.StartWarpSequence();
			this.Refresh(null);
		}
	}

	// Token: 0x0600ADB0 RID: 44464 RVA: 0x00111159 File Offset: 0x0010F359
	private void OnCancelClick()
	{
		this.target.CancelAssignment();
		this.Refresh(null);
	}

	// Token: 0x0600ADB1 RID: 44465 RVA: 0x00412BE4 File Offset: 0x00410DE4
	private void Refresh(object data = null)
	{
		this.progressBar.SetActive(false);
		this.cancelButton.gameObject.SetActive(false);
		if (!(this.target != null))
		{
			this.label.text = UI.UISIDESCREENS.WARPPORTALSIDESCREEN.IDLE;
			this.button.gameObject.SetActive(false);
			return;
		}
		if (this.target.ReadyToWarp)
		{
			this.label.text = UI.UISIDESCREENS.WARPPORTALSIDESCREEN.WAITING;
			this.button.gameObject.SetActive(true);
			this.cancelButton.gameObject.SetActive(true);
			return;
		}
		if (this.target.IsConsumed)
		{
			this.button.gameObject.SetActive(false);
			this.progressBar.SetActive(true);
			this.label.text = UI.UISIDESCREENS.WARPPORTALSIDESCREEN.CONSUMED;
			return;
		}
		if (this.target.IsWorking)
		{
			this.label.text = UI.UISIDESCREENS.WARPPORTALSIDESCREEN.UNDERWAY;
			this.button.gameObject.SetActive(false);
			this.cancelButton.gameObject.SetActive(true);
			return;
		}
		this.label.text = UI.UISIDESCREENS.WARPPORTALSIDESCREEN.IDLE;
		this.button.gameObject.SetActive(false);
	}

	// Token: 0x04008840 RID: 34880
	[SerializeField]
	private LocText label;

	// Token: 0x04008841 RID: 34881
	[SerializeField]
	private KButton button;

	// Token: 0x04008842 RID: 34882
	[SerializeField]
	private LocText buttonLabel;

	// Token: 0x04008843 RID: 34883
	[SerializeField]
	private KButton cancelButton;

	// Token: 0x04008844 RID: 34884
	[SerializeField]
	private LocText cancelButtonLabel;

	// Token: 0x04008845 RID: 34885
	[SerializeField]
	private WarpPortal target;

	// Token: 0x04008846 RID: 34886
	[SerializeField]
	private GameObject contents;

	// Token: 0x04008847 RID: 34887
	[SerializeField]
	private GameObject progressBar;

	// Token: 0x04008848 RID: 34888
	[SerializeField]
	private LocText progressLabel;
}
