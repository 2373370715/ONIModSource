using System;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001E3A RID: 7738
public class ModeSelectScreen : NewGameFlowScreen
{
	// Token: 0x0600A220 RID: 41504 RVA: 0x0010937B File Offset: 0x0010757B
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.LoadWorldAndClusterData();
	}

	// Token: 0x0600A221 RID: 41505 RVA: 0x003DC04C File Offset: 0x003DA24C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		HierarchyReferences component = this.survivalButton.GetComponent<HierarchyReferences>();
		this.survivalButtonHeader = component.GetReference<RectTransform>("HeaderBackground").GetComponent<Image>();
		this.survivalButtonSelectionFrame = component.GetReference<RectTransform>("SelectionFrame").GetComponent<Image>();
		MultiToggle multiToggle = this.survivalButton;
		multiToggle.onEnter = (System.Action)Delegate.Combine(multiToggle.onEnter, new System.Action(this.OnHoverEnterSurvival));
		MultiToggle multiToggle2 = this.survivalButton;
		multiToggle2.onExit = (System.Action)Delegate.Combine(multiToggle2.onExit, new System.Action(this.OnHoverExitSurvival));
		MultiToggle multiToggle3 = this.survivalButton;
		multiToggle3.onClick = (System.Action)Delegate.Combine(multiToggle3.onClick, new System.Action(this.OnClickSurvival));
		HierarchyReferences component2 = this.nosweatButton.GetComponent<HierarchyReferences>();
		this.nosweatButtonHeader = component2.GetReference<RectTransform>("HeaderBackground").GetComponent<Image>();
		this.nosweatButtonSelectionFrame = component2.GetReference<RectTransform>("SelectionFrame").GetComponent<Image>();
		MultiToggle multiToggle4 = this.nosweatButton;
		multiToggle4.onEnter = (System.Action)Delegate.Combine(multiToggle4.onEnter, new System.Action(this.OnHoverEnterNosweat));
		MultiToggle multiToggle5 = this.nosweatButton;
		multiToggle5.onExit = (System.Action)Delegate.Combine(multiToggle5.onExit, new System.Action(this.OnHoverExitNosweat));
		MultiToggle multiToggle6 = this.nosweatButton;
		multiToggle6.onClick = (System.Action)Delegate.Combine(multiToggle6.onClick, new System.Action(this.OnClickNosweat));
		this.closeButton.onClick += base.NavigateBackward;
	}

	// Token: 0x0600A222 RID: 41506 RVA: 0x003DC1D0 File Offset: 0x003DA3D0
	private void OnHoverEnterSurvival()
	{
		KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Mouseover", false));
		this.survivalButtonSelectionFrame.SetAlpha(1f);
		this.survivalButtonHeader.color = new Color(0.7019608f, 0.3647059f, 0.53333336f, 1f);
		this.descriptionArea.text = UI.FRONTEND.MODESELECTSCREEN.SURVIVAL_DESC;
	}

	// Token: 0x0600A223 RID: 41507 RVA: 0x003DC238 File Offset: 0x003DA438
	private void OnHoverExitSurvival()
	{
		KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Mouseover", false));
		this.survivalButtonSelectionFrame.SetAlpha(0f);
		this.survivalButtonHeader.color = new Color(0.30980393f, 0.34117648f, 0.38431373f, 1f);
		this.descriptionArea.text = UI.FRONTEND.MODESELECTSCREEN.BLANK_DESC;
	}

	// Token: 0x0600A224 RID: 41508 RVA: 0x00109389 File Offset: 0x00107589
	private void OnClickSurvival()
	{
		this.Deactivate();
		CustomGameSettings.Instance.SetSurvivalDefaults();
		base.NavigateForward();
	}

	// Token: 0x0600A225 RID: 41509 RVA: 0x001093A1 File Offset: 0x001075A1
	private void LoadWorldAndClusterData()
	{
		if (ModeSelectScreen.dataLoaded)
		{
			return;
		}
		CustomGameSettings.Instance.LoadClusters();
		Global.Instance.modManager.Report(base.gameObject);
		ModeSelectScreen.dataLoaded = true;
	}

	// Token: 0x0600A226 RID: 41510 RVA: 0x003DC2A0 File Offset: 0x003DA4A0
	private void OnHoverEnterNosweat()
	{
		KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Mouseover", false));
		this.nosweatButtonSelectionFrame.SetAlpha(1f);
		this.nosweatButtonHeader.color = new Color(0.7019608f, 0.3647059f, 0.53333336f, 1f);
		this.descriptionArea.text = UI.FRONTEND.MODESELECTSCREEN.NOSWEAT_DESC;
	}

	// Token: 0x0600A227 RID: 41511 RVA: 0x003DC308 File Offset: 0x003DA508
	private void OnHoverExitNosweat()
	{
		KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Mouseover", false));
		this.nosweatButtonSelectionFrame.SetAlpha(0f);
		this.nosweatButtonHeader.color = new Color(0.30980393f, 0.34117648f, 0.38431373f, 1f);
		this.descriptionArea.text = UI.FRONTEND.MODESELECTSCREEN.BLANK_DESC;
	}

	// Token: 0x0600A228 RID: 41512 RVA: 0x001093D0 File Offset: 0x001075D0
	private void OnClickNosweat()
	{
		this.Deactivate();
		CustomGameSettings.Instance.SetNosweatDefaults();
		base.NavigateForward();
	}

	// Token: 0x04007E74 RID: 32372
	[SerializeField]
	private MultiToggle nosweatButton;

	// Token: 0x04007E75 RID: 32373
	private Image nosweatButtonHeader;

	// Token: 0x04007E76 RID: 32374
	private Image nosweatButtonSelectionFrame;

	// Token: 0x04007E77 RID: 32375
	[SerializeField]
	private MultiToggle survivalButton;

	// Token: 0x04007E78 RID: 32376
	private Image survivalButtonHeader;

	// Token: 0x04007E79 RID: 32377
	private Image survivalButtonSelectionFrame;

	// Token: 0x04007E7A RID: 32378
	[SerializeField]
	private LocText descriptionArea;

	// Token: 0x04007E7B RID: 32379
	[SerializeField]
	private KButton closeButton;

	// Token: 0x04007E7C RID: 32380
	[SerializeField]
	private KBatchedAnimController nosweatAnim;

	// Token: 0x04007E7D RID: 32381
	[SerializeField]
	private KBatchedAnimController survivalAnim;

	// Token: 0x04007E7E RID: 32382
	private static bool dataLoaded;
}
