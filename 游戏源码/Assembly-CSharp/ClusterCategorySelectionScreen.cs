using System;
using ProcGen;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

public class ClusterCategorySelectionScreen : NewGameFlowScreen
{
	[Serializable]
	public class ButtonConfig
	{
		public MultiToggle button;

		public Image headerImage;

		public LocText headerLabel;

		public Image selectionFrame;

		public KAnimControllerBase kanim;

		private string hoverDescriptionText;

		private LocText descriptionArea;

		public void Init(LocText descriptionArea, string hoverDescriptionText, string headerText)
		{
			this.descriptionArea = descriptionArea;
			this.hoverDescriptionText = hoverDescriptionText;
			headerLabel.SetText(headerText);
			MultiToggle multiToggle = button;
			multiToggle.onEnter = (System.Action)Delegate.Combine(multiToggle.onEnter, new System.Action(OnHoverEnter));
			MultiToggle multiToggle2 = button;
			multiToggle2.onExit = (System.Action)Delegate.Combine(multiToggle2.onExit, new System.Action(OnHoverExit));
			HierarchyReferences component = button.GetComponent<HierarchyReferences>();
			headerImage = component.GetReference<RectTransform>("HeaderBackground").GetComponent<Image>();
			selectionFrame = component.GetReference<RectTransform>("SelectionFrame").GetComponent<Image>();
		}

		private void OnHoverEnter()
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Mouseover"));
			selectionFrame.SetAlpha(1f);
			headerImage.color = new Color(0.7019608f, 31f / 85f, 8f / 15f, 1f);
			descriptionArea.text = hoverDescriptionText;
		}

		private void OnHoverExit()
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Mouseover"));
			selectionFrame.SetAlpha(0f);
			headerImage.color = new Color(0.30980393f, 29f / 85f, 0.38431373f, 1f);
			descriptionArea.text = UI.FRONTEND.CLUSTERCATEGORYSELECTSCREEN.BLANK_DESC;
		}
	}

	public ButtonConfig vanillaStyle;

	public ButtonConfig classicStyle;

	public ButtonConfig spacedOutStyle;

	public ButtonConfig eventStyle;

	[SerializeField]
	private LocText descriptionArea;

	[SerializeField]
	private KButton closeButton;

	[SerializeField]
	private RectTransform panel;

	protected override void OnSpawn()
	{
		base.OnSpawn();
		closeButton.onClick += base.NavigateBackward;
		int num = 0;
		foreach (ClusterLayout value in SettingsCache.clusterLayouts.clusterCache.Values)
		{
			if (value.clusterCategory == ClusterLayout.ClusterCategory.Special)
			{
				num++;
			}
		}
		if (num > 0)
		{
			eventStyle.button.gameObject.SetActive(value: true);
			eventStyle.Init(descriptionArea, UI.FRONTEND.CLUSTERCATEGORYSELECTSCREEN.EVENT_DESC, UI.FRONTEND.CLUSTERCATEGORYSELECTSCREEN.EVENT_TITLE);
			MultiToggle button = eventStyle.button;
			button.onClick = (System.Action)Delegate.Combine(button.onClick, (System.Action)delegate
			{
				OnClickOption(ClusterLayout.ClusterCategory.Special);
			});
		}
		if (DlcManager.IsExpansion1Active())
		{
			classicStyle.button.gameObject.SetActive(value: true);
			classicStyle.Init(descriptionArea, UI.FRONTEND.CLUSTERCATEGORYSELECTSCREEN.CLASSIC_DESC, UI.FRONTEND.CLUSTERCATEGORYSELECTSCREEN.CLASSIC_TITLE);
			MultiToggle button2 = classicStyle.button;
			button2.onClick = (System.Action)Delegate.Combine(button2.onClick, (System.Action)delegate
			{
				OnClickOption(ClusterLayout.ClusterCategory.SpacedOutVanillaStyle);
			});
			spacedOutStyle.button.gameObject.SetActive(value: true);
			spacedOutStyle.Init(descriptionArea, UI.FRONTEND.CLUSTERCATEGORYSELECTSCREEN.SPACEDOUT_DESC, UI.FRONTEND.CLUSTERCATEGORYSELECTSCREEN.SPACEDOUT_TITLE);
			MultiToggle button3 = spacedOutStyle.button;
			button3.onClick = (System.Action)Delegate.Combine(button3.onClick, (System.Action)delegate
			{
				OnClickOption(ClusterLayout.ClusterCategory.SpacedOutStyle);
			});
			panel.sizeDelta = ((num > 0) ? new Vector2(622f, panel.sizeDelta.y) : new Vector2(480f, panel.sizeDelta.y));
		}
		else
		{
			vanillaStyle.button.gameObject.SetActive(value: true);
			vanillaStyle.Init(descriptionArea, UI.FRONTEND.CLUSTERCATEGORYSELECTSCREEN.VANILLA_DESC, UI.FRONTEND.CLUSTERCATEGORYSELECTSCREEN.VANILLA_TITLE);
			MultiToggle button4 = vanillaStyle.button;
			button4.onClick = (System.Action)Delegate.Combine(button4.onClick, (System.Action)delegate
			{
				OnClickOption(ClusterLayout.ClusterCategory.Vanilla);
			});
			panel.sizeDelta = new Vector2(480f, panel.sizeDelta.y);
			eventStyle.kanim.Play("lab_asteroid_standard");
		}
	}

	private void OnClickOption(ClusterLayout.ClusterCategory clusterCategory)
	{
		Deactivate();
		DestinationSelectPanel.ChosenClusterCategorySetting = (int)clusterCategory;
		NavigateForward();
	}
}
