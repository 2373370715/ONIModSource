using System;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001FE0 RID: 8160
public class TelescopeSideScreen : SideScreenContent
{
	// Token: 0x0600ACEF RID: 44271 RVA: 0x00110797 File Offset: 0x0010E997
	public TelescopeSideScreen()
	{
		this.refreshDisplayStateDelegate = new Action<object>(this.RefreshDisplayState);
	}

	// Token: 0x0600ACF0 RID: 44272 RVA: 0x0040FFF0 File Offset: 0x0040E1F0
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.selectStarmapScreen.onClick += delegate()
		{
			ManagementMenu.Instance.ToggleStarmap();
		};
		SpacecraftManager.instance.Subscribe(532901469, this.refreshDisplayStateDelegate);
		this.RefreshDisplayState(null);
	}

	// Token: 0x0600ACF1 RID: 44273 RVA: 0x001107B1 File Offset: 0x0010E9B1
	protected override void OnCmpEnable()
	{
		base.OnCmpEnable();
		this.RefreshDisplayState(null);
		this.target = SelectTool.Instance.selected.GetComponent<KMonoBehaviour>().gameObject;
	}

	// Token: 0x0600ACF2 RID: 44274 RVA: 0x001107DA File Offset: 0x0010E9DA
	protected override void OnCmpDisable()
	{
		base.OnCmpDisable();
		if (this.target)
		{
			this.target = null;
		}
	}

	// Token: 0x0600ACF3 RID: 44275 RVA: 0x001107F6 File Offset: 0x0010E9F6
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		if (this.target)
		{
			this.target = null;
		}
	}

	// Token: 0x0600ACF4 RID: 44276 RVA: 0x00110812 File Offset: 0x0010EA12
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<Telescope>() != null;
	}

	// Token: 0x0600ACF5 RID: 44277 RVA: 0x0041004C File Offset: 0x0040E24C
	private void RefreshDisplayState(object data = null)
	{
		if (SelectTool.Instance.selected == null)
		{
			return;
		}
		if (SelectTool.Instance.selected.GetComponent<Telescope>() == null)
		{
			return;
		}
		if (!SpacecraftManager.instance.HasAnalysisTarget())
		{
			this.DescriptionText.text = "<b><color=#FF0000>" + UI.UISIDESCREENS.TELESCOPESIDESCREEN.NO_SELECTED_ANALYSIS_TARGET + "</color></b>";
			return;
		}
		string text = UI.UISIDESCREENS.TELESCOPESIDESCREEN.ANALYSIS_TARGET_SELECTED;
		this.DescriptionText.text = text;
	}

	// Token: 0x040087B9 RID: 34745
	public KButton selectStarmapScreen;

	// Token: 0x040087BA RID: 34746
	public Image researchButtonIcon;

	// Token: 0x040087BB RID: 34747
	public GameObject content;

	// Token: 0x040087BC RID: 34748
	private GameObject target;

	// Token: 0x040087BD RID: 34749
	private Action<object> refreshDisplayStateDelegate;

	// Token: 0x040087BE RID: 34750
	public LocText DescriptionText;
}
