using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

public class DetailsScreenMaterialPanel : TargetScreen
{
	[Header("Current Material")]
	[SerializeField]
	private Image currentMaterialIcon;

	[SerializeField]
	private RectTransform currentMaterialDescriptionRow;

	[SerializeField]
	private LocText currentMaterialLabel;

	[SerializeField]
	private LocText currentMaterialDescription;

	[SerializeField]
	private DescriptorPanel descriptorPanel;

	[Header("Change Material")]
	[SerializeField]
	private MaterialSelectionPanel materialSelectionPanel;

	[SerializeField]
	private RectTransform confirmChangeRow;

	[SerializeField]
	private KButton orderChangeMaterialButton;

	[SerializeField]
	private KButton openChangeMaterialPanelButton;

	private int subHandle = -1;

	private Building building;

	public override bool IsValidForTarget(GameObject target)
	{
		return true;
	}

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		openChangeMaterialPanelButton.onClick += delegate
		{
			OpenMaterialSelectionPanel();
			RefreshMaterialSelectionPanel();
			RefreshOrderChangeMaterialButton();
		};
	}

	public override void SetTarget(GameObject target)
	{
		if (selectedTarget != null)
		{
			selectedTarget.Unsubscribe(subHandle);
		}
		building = null;
		base.SetTarget(target);
		if (!(target == null))
		{
			materialSelectionPanel.gameObject.SetActive(value: false);
			orderChangeMaterialButton.ClearOnClick();
			orderChangeMaterialButton.isInteractable = false;
			CloseMaterialSelectionPanel();
			building = selectedTarget.GetComponent<Building>();
			bool flag = Db.Get().TechItems.IsTechItemComplete(building.Def.PrefabID) || DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive;
			openChangeMaterialPanelButton.isInteractable = target.GetComponent<Reconstructable>() != null && target.GetComponent<Reconstructable>().AllowReconstruct && flag;
			openChangeMaterialPanelButton.GetComponent<ToolTip>().SetSimpleTooltip(flag ? "" : string.Format(UI.PRODUCTINFO_REQUIRESRESEARCHDESC, Db.Get().TechItems.GetTechFromItemID(building.Def.PrefabID).Name));
			Refresh();
			subHandle = target.Subscribe(954267658, RefreshOrderChangeMaterialButton);
			Game.Instance.Subscribe(1980521255, Refresh);
		}
	}

	private void OpenMaterialSelectionPanel()
	{
		openChangeMaterialPanelButton.gameObject.SetActive(value: false);
		materialSelectionPanel.gameObject.SetActive(value: true);
		RefreshMaterialSelectionPanel();
		if (selectedTarget != null && building != null)
		{
			materialSelectionPanel.SelectSourcesMaterials(building);
		}
	}

	private void CloseMaterialSelectionPanel()
	{
		currentMaterialDescriptionRow.gameObject.SetActive(value: true);
		openChangeMaterialPanelButton.gameObject.SetActive(value: true);
		materialSelectionPanel.gameObject.SetActive(value: false);
	}

	public override void OnDeselectTarget(GameObject target)
	{
		base.OnDeselectTarget(target);
		Refresh();
	}

	private void Refresh(object data = null)
	{
		RefreshCurrentMaterial();
		RefreshMaterialSelectionPanel();
	}

	private void RefreshCurrentMaterial()
	{
		if (!(selectedTarget == null))
		{
			CellSelectionObject component = selectedTarget.GetComponent<CellSelectionObject>();
			Element element = ((component == null) ? selectedTarget.GetComponent<PrimaryElement>().Element : component.element);
			Tuple<Sprite, Color> uISprite = Def.GetUISprite(element);
			currentMaterialIcon.sprite = uISprite.first;
			currentMaterialIcon.color = uISprite.second;
			if (component == null)
			{
				currentMaterialLabel.SetText(element.name + " x " + GameUtil.GetFormattedMass(selectedTarget.GetComponent<PrimaryElement>().Mass));
			}
			else
			{
				currentMaterialLabel.SetText(element.name);
			}
			currentMaterialDescription.SetText(element.description);
			List<Descriptor> materialDescriptors = GameUtil.GetMaterialDescriptors(element);
			if (materialDescriptors.Count > 0)
			{
				Descriptor item = default(Descriptor);
				item.SetupDescriptor(ELEMENTS.MATERIAL_MODIFIERS.EFFECTS_HEADER, ELEMENTS.MATERIAL_MODIFIERS.TOOLTIP.EFFECTS_HEADER);
				materialDescriptors.Insert(0, item);
				descriptorPanel.gameObject.SetActive(value: true);
				descriptorPanel.SetDescriptors(materialDescriptors);
			}
			else
			{
				descriptorPanel.gameObject.SetActive(value: false);
			}
		}
	}

	private void RefreshMaterialSelectionPanel()
	{
		if (selectedTarget == null)
		{
			return;
		}
		materialSelectionPanel.ClearSelectActions();
		if (!(building == null) && !(building is BuildingUnderConstruction))
		{
			materialSelectionPanel.ConfigureScreen(building.Def.CraftRecipe, (BuildingDef data) => true, (BuildingDef data) => "");
			materialSelectionPanel.AddSelectAction(RefreshOrderChangeMaterialButton);
			Reconstructable component = selectedTarget.GetComponent<Reconstructable>();
			if (component != null && component.ReconstructRequested)
			{
				if (!materialSelectionPanel.gameObject.activeSelf)
				{
					OpenMaterialSelectionPanel();
					materialSelectionPanel.RefreshSelectors();
				}
				materialSelectionPanel.ForceSelectPrimaryTag(component.PrimarySelectedElementTag);
			}
		}
		confirmChangeRow.transform.SetAsLastSibling();
	}

	private void RefreshOrderChangeMaterialButton()
	{
		RefreshOrderChangeMaterialButton(null);
	}

	private void RefreshOrderChangeMaterialButton(object data = null)
	{
		if (!(selectedTarget == null))
		{
			Reconstructable reconstructable = selectedTarget.GetComponent<Reconstructable>();
			bool flag = materialSelectionPanel.CurrentSelectedElement != null;
			orderChangeMaterialButton.isInteractable = flag && building.GetComponent<PrimaryElement>().Element.tag != materialSelectionPanel.CurrentSelectedElement;
			orderChangeMaterialButton.ClearOnClick();
			orderChangeMaterialButton.onClick += delegate
			{
				reconstructable.RequestReconstruct(materialSelectionPanel.CurrentSelectedElement);
				RefreshOrderChangeMaterialButton();
			};
			orderChangeMaterialButton.GetComponentInChildren<LocText>().SetText(reconstructable.ReconstructRequested ? UI.USERMENUACTIONS.RECONSTRUCT.CANCEL_RECONSTRUCT : UI.USERMENUACTIONS.RECONSTRUCT.REQUEST_RECONSTRUCT);
			orderChangeMaterialButton.GetComponent<ToolTip>().SetSimpleTooltip(reconstructable.ReconstructRequested ? UI.USERMENUACTIONS.RECONSTRUCT.CANCEL_RECONSTRUCT_TOOLTIP : UI.USERMENUACTIONS.RECONSTRUCT.REQUEST_RECONSTRUCT_TOOLTIP);
		}
	}
}
