using System;
using System.Collections.Generic;
using ProcGen;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001C15 RID: 7189
public class ClusterCategorySelectionScreen : NewGameFlowScreen
{
	// Token: 0x06009570 RID: 38256 RVA: 0x0039C5F4 File Offset: 0x0039A7F4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.closeButton.onClick += base.NavigateBackward;
		int num = 0;
		using (Dictionary<string, ClusterLayout>.ValueCollection.Enumerator enumerator = SettingsCache.clusterLayouts.clusterCache.Values.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.clusterCategory == ClusterLayout.ClusterCategory.Special)
				{
					num++;
				}
			}
		}
		if (num > 0)
		{
			this.eventStyle.button.gameObject.SetActive(true);
			this.eventStyle.Init(this.descriptionArea, UI.FRONTEND.CLUSTERCATEGORYSELECTSCREEN.EVENT_DESC, UI.FRONTEND.CLUSTERCATEGORYSELECTSCREEN.EVENT_TITLE);
			MultiToggle button = this.eventStyle.button;
			button.onClick = (System.Action)Delegate.Combine(button.onClick, new System.Action(delegate()
			{
				this.OnClickOption(ClusterLayout.ClusterCategory.Special);
			}));
		}
		if (DlcManager.IsExpansion1Active())
		{
			this.classicStyle.button.gameObject.SetActive(true);
			this.classicStyle.Init(this.descriptionArea, UI.FRONTEND.CLUSTERCATEGORYSELECTSCREEN.CLASSIC_DESC, UI.FRONTEND.CLUSTERCATEGORYSELECTSCREEN.CLASSIC_TITLE);
			MultiToggle button2 = this.classicStyle.button;
			button2.onClick = (System.Action)Delegate.Combine(button2.onClick, new System.Action(delegate()
			{
				this.OnClickOption(ClusterLayout.ClusterCategory.SpacedOutVanillaStyle);
			}));
			this.spacedOutStyle.button.gameObject.SetActive(true);
			this.spacedOutStyle.Init(this.descriptionArea, UI.FRONTEND.CLUSTERCATEGORYSELECTSCREEN.SPACEDOUT_DESC, UI.FRONTEND.CLUSTERCATEGORYSELECTSCREEN.SPACEDOUT_TITLE);
			MultiToggle button3 = this.spacedOutStyle.button;
			button3.onClick = (System.Action)Delegate.Combine(button3.onClick, new System.Action(delegate()
			{
				this.OnClickOption(ClusterLayout.ClusterCategory.SpacedOutStyle);
			}));
			this.panel.sizeDelta = ((num > 0) ? new Vector2(622f, this.panel.sizeDelta.y) : new Vector2(480f, this.panel.sizeDelta.y));
			return;
		}
		this.vanillaStyle.button.gameObject.SetActive(true);
		this.vanillaStyle.Init(this.descriptionArea, UI.FRONTEND.CLUSTERCATEGORYSELECTSCREEN.VANILLA_DESC, UI.FRONTEND.CLUSTERCATEGORYSELECTSCREEN.VANILLA_TITLE);
		MultiToggle button4 = this.vanillaStyle.button;
		button4.onClick = (System.Action)Delegate.Combine(button4.onClick, new System.Action(delegate()
		{
			this.OnClickOption(ClusterLayout.ClusterCategory.Vanilla);
		}));
		this.panel.sizeDelta = new Vector2(480f, this.panel.sizeDelta.y);
		this.eventStyle.kanim.Play("lab_asteroid_standard", KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x06009571 RID: 38257 RVA: 0x00101538 File Offset: 0x000FF738
	private void OnClickOption(ClusterLayout.ClusterCategory clusterCategory)
	{
		this.Deactivate();
		DestinationSelectPanel.ChosenClusterCategorySetting = (int)clusterCategory;
		base.NavigateForward();
	}

	// Token: 0x04007405 RID: 29701
	public ClusterCategorySelectionScreen.ButtonConfig vanillaStyle;

	// Token: 0x04007406 RID: 29702
	public ClusterCategorySelectionScreen.ButtonConfig classicStyle;

	// Token: 0x04007407 RID: 29703
	public ClusterCategorySelectionScreen.ButtonConfig spacedOutStyle;

	// Token: 0x04007408 RID: 29704
	public ClusterCategorySelectionScreen.ButtonConfig eventStyle;

	// Token: 0x04007409 RID: 29705
	[SerializeField]
	private LocText descriptionArea;

	// Token: 0x0400740A RID: 29706
	[SerializeField]
	private KButton closeButton;

	// Token: 0x0400740B RID: 29707
	[SerializeField]
	private RectTransform panel;

	// Token: 0x02001C16 RID: 7190
	[Serializable]
	public class ButtonConfig
	{
		// Token: 0x06009577 RID: 38263 RVA: 0x0039C8B0 File Offset: 0x0039AAB0
		public void Init(LocText descriptionArea, string hoverDescriptionText, string headerText)
		{
			this.descriptionArea = descriptionArea;
			this.hoverDescriptionText = hoverDescriptionText;
			this.headerLabel.SetText(headerText);
			MultiToggle multiToggle = this.button;
			multiToggle.onEnter = (System.Action)Delegate.Combine(multiToggle.onEnter, new System.Action(this.OnHoverEnter));
			MultiToggle multiToggle2 = this.button;
			multiToggle2.onExit = (System.Action)Delegate.Combine(multiToggle2.onExit, new System.Action(this.OnHoverExit));
			HierarchyReferences component = this.button.GetComponent<HierarchyReferences>();
			this.headerImage = component.GetReference<RectTransform>("HeaderBackground").GetComponent<Image>();
			this.selectionFrame = component.GetReference<RectTransform>("SelectionFrame").GetComponent<Image>();
		}

		// Token: 0x06009578 RID: 38264 RVA: 0x0039C960 File Offset: 0x0039AB60
		private void OnHoverEnter()
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Mouseover", false));
			this.selectionFrame.SetAlpha(1f);
			this.headerImage.color = new Color(0.7019608f, 0.3647059f, 0.53333336f, 1f);
			this.descriptionArea.text = this.hoverDescriptionText;
		}

		// Token: 0x06009579 RID: 38265 RVA: 0x0039C9C4 File Offset: 0x0039ABC4
		private void OnHoverExit()
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Mouseover", false));
			this.selectionFrame.SetAlpha(0f);
			this.headerImage.color = new Color(0.30980393f, 0.34117648f, 0.38431373f, 1f);
			this.descriptionArea.text = UI.FRONTEND.CLUSTERCATEGORYSELECTSCREEN.BLANK_DESC;
		}

		// Token: 0x0400740C RID: 29708
		public MultiToggle button;

		// Token: 0x0400740D RID: 29709
		public Image headerImage;

		// Token: 0x0400740E RID: 29710
		public LocText headerLabel;

		// Token: 0x0400740F RID: 29711
		public Image selectionFrame;

		// Token: 0x04007410 RID: 29712
		public KAnimControllerBase kanim;

		// Token: 0x04007411 RID: 29713
		private string hoverDescriptionText;

		// Token: 0x04007412 RID: 29714
		private LocText descriptionArea;
	}
}
