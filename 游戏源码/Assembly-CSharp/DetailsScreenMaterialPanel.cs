using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001CB4 RID: 7348
public class DetailsScreenMaterialPanel : TargetScreen
{
	// Token: 0x06009971 RID: 39281 RVA: 0x000A65EC File Offset: 0x000A47EC
	public override bool IsValidForTarget(GameObject target)
	{
		return true;
	}

	// Token: 0x06009972 RID: 39282 RVA: 0x00103E39 File Offset: 0x00102039
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.openChangeMaterialPanelButton.onClick += delegate()
		{
			this.OpenMaterialSelectionPanel();
			this.RefreshMaterialSelectionPanel();
			this.RefreshOrderChangeMaterialButton();
		};
	}

	// Token: 0x06009973 RID: 39283 RVA: 0x003B4F0C File Offset: 0x003B310C
	public override void SetTarget(GameObject target)
	{
		if (this.selectedTarget != null)
		{
			this.selectedTarget.Unsubscribe(this.subHandle);
		}
		this.building = null;
		base.SetTarget(target);
		if (target == null)
		{
			return;
		}
		this.materialSelectionPanel.gameObject.SetActive(false);
		this.orderChangeMaterialButton.ClearOnClick();
		this.orderChangeMaterialButton.isInteractable = false;
		this.CloseMaterialSelectionPanel();
		this.building = this.selectedTarget.GetComponent<Building>();
		bool flag = Db.Get().TechItems.IsTechItemComplete(this.building.Def.PrefabID) || DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive;
		this.openChangeMaterialPanelButton.isInteractable = (target.GetComponent<Reconstructable>() != null && target.GetComponent<Reconstructable>().AllowReconstruct && flag);
		this.openChangeMaterialPanelButton.GetComponent<ToolTip>().SetSimpleTooltip(flag ? "" : string.Format(UI.PRODUCTINFO_REQUIRESRESEARCHDESC, Db.Get().TechItems.GetTechFromItemID(this.building.Def.PrefabID).Name));
		this.Refresh(null);
		this.subHandle = target.Subscribe(954267658, new Action<object>(this.RefreshOrderChangeMaterialButton));
		Game.Instance.Subscribe(1980521255, new Action<object>(this.Refresh));
	}

	// Token: 0x06009974 RID: 39284 RVA: 0x003B507C File Offset: 0x003B327C
	private void OpenMaterialSelectionPanel()
	{
		this.openChangeMaterialPanelButton.gameObject.SetActive(false);
		this.materialSelectionPanel.gameObject.SetActive(true);
		this.RefreshMaterialSelectionPanel();
		if (this.selectedTarget != null && this.building != null)
		{
			this.materialSelectionPanel.SelectSourcesMaterials(this.building);
		}
	}

	// Token: 0x06009975 RID: 39285 RVA: 0x00103E58 File Offset: 0x00102058
	private void CloseMaterialSelectionPanel()
	{
		this.currentMaterialDescriptionRow.gameObject.SetActive(true);
		this.openChangeMaterialPanelButton.gameObject.SetActive(true);
		this.materialSelectionPanel.gameObject.SetActive(false);
	}

	// Token: 0x06009976 RID: 39286 RVA: 0x00103E8D File Offset: 0x0010208D
	public override void OnDeselectTarget(GameObject target)
	{
		base.OnDeselectTarget(target);
		this.Refresh(null);
	}

	// Token: 0x06009977 RID: 39287 RVA: 0x00103E9D File Offset: 0x0010209D
	private void Refresh(object data = null)
	{
		this.RefreshCurrentMaterial();
		this.RefreshMaterialSelectionPanel();
	}

	// Token: 0x06009978 RID: 39288 RVA: 0x003B50E0 File Offset: 0x003B32E0
	private void RefreshCurrentMaterial()
	{
		if (this.selectedTarget == null)
		{
			return;
		}
		CellSelectionObject component = this.selectedTarget.GetComponent<CellSelectionObject>();
		Element element = (component == null) ? this.selectedTarget.GetComponent<PrimaryElement>().Element : component.element;
		global::Tuple<Sprite, Color> uisprite = Def.GetUISprite(element, "ui", false);
		this.currentMaterialIcon.sprite = uisprite.first;
		this.currentMaterialIcon.color = uisprite.second;
		if (component == null)
		{
			this.currentMaterialLabel.SetText(element.name + " x " + GameUtil.GetFormattedMass(this.selectedTarget.GetComponent<PrimaryElement>().Mass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
		}
		else
		{
			this.currentMaterialLabel.SetText(element.name);
		}
		this.currentMaterialDescription.SetText(element.description);
		List<Descriptor> materialDescriptors = GameUtil.GetMaterialDescriptors(element);
		if (materialDescriptors.Count > 0)
		{
			Descriptor item = default(Descriptor);
			item.SetupDescriptor(ELEMENTS.MATERIAL_MODIFIERS.EFFECTS_HEADER, ELEMENTS.MATERIAL_MODIFIERS.TOOLTIP.EFFECTS_HEADER, Descriptor.DescriptorType.Effect);
			materialDescriptors.Insert(0, item);
			this.descriptorPanel.gameObject.SetActive(true);
			this.descriptorPanel.SetDescriptors(materialDescriptors);
			return;
		}
		this.descriptorPanel.gameObject.SetActive(false);
	}

	// Token: 0x06009979 RID: 39289 RVA: 0x003B522C File Offset: 0x003B342C
	private void RefreshMaterialSelectionPanel()
	{
		if (this.selectedTarget == null)
		{
			return;
		}
		this.materialSelectionPanel.ClearSelectActions();
		if (!(this.building == null) && !(this.building is BuildingUnderConstruction))
		{
			this.materialSelectionPanel.ConfigureScreen(this.building.Def.CraftRecipe, (BuildingDef data) => true, (BuildingDef data) => "");
			this.materialSelectionPanel.AddSelectAction(new MaterialSelector.SelectMaterialActions(this.RefreshOrderChangeMaterialButton));
			Reconstructable component = this.selectedTarget.GetComponent<Reconstructable>();
			if (component != null && component.ReconstructRequested)
			{
				if (!this.materialSelectionPanel.gameObject.activeSelf)
				{
					this.OpenMaterialSelectionPanel();
					this.materialSelectionPanel.RefreshSelectors();
				}
				this.materialSelectionPanel.ForceSelectPrimaryTag(component.PrimarySelectedElementTag);
			}
		}
		this.confirmChangeRow.transform.SetAsLastSibling();
	}

	// Token: 0x0600997A RID: 39290 RVA: 0x00103EAB File Offset: 0x001020AB
	private void RefreshOrderChangeMaterialButton()
	{
		this.RefreshOrderChangeMaterialButton(null);
	}

	// Token: 0x0600997B RID: 39291 RVA: 0x003B5348 File Offset: 0x003B3548
	private void RefreshOrderChangeMaterialButton(object data = null)
	{
		if (this.selectedTarget == null)
		{
			return;
		}
		Reconstructable reconstructable = this.selectedTarget.GetComponent<Reconstructable>();
		bool flag = this.materialSelectionPanel.CurrentSelectedElement != null;
		this.orderChangeMaterialButton.isInteractable = (flag && this.building.GetComponent<PrimaryElement>().Element.tag != this.materialSelectionPanel.CurrentSelectedElement);
		this.orderChangeMaterialButton.ClearOnClick();
		this.orderChangeMaterialButton.onClick += delegate()
		{
			reconstructable.RequestReconstruct(this.materialSelectionPanel.CurrentSelectedElement);
			this.RefreshOrderChangeMaterialButton();
		};
		this.orderChangeMaterialButton.GetComponentInChildren<LocText>().SetText(reconstructable.ReconstructRequested ? UI.USERMENUACTIONS.RECONSTRUCT.CANCEL_RECONSTRUCT : UI.USERMENUACTIONS.RECONSTRUCT.REQUEST_RECONSTRUCT);
		this.orderChangeMaterialButton.GetComponent<ToolTip>().SetSimpleTooltip(reconstructable.ReconstructRequested ? UI.USERMENUACTIONS.RECONSTRUCT.CANCEL_RECONSTRUCT_TOOLTIP : UI.USERMENUACTIONS.RECONSTRUCT.REQUEST_RECONSTRUCT_TOOLTIP);
	}

	// Token: 0x040077AC RID: 30636
	[Header("Current Material")]
	[SerializeField]
	private Image currentMaterialIcon;

	// Token: 0x040077AD RID: 30637
	[SerializeField]
	private RectTransform currentMaterialDescriptionRow;

	// Token: 0x040077AE RID: 30638
	[SerializeField]
	private LocText currentMaterialLabel;

	// Token: 0x040077AF RID: 30639
	[SerializeField]
	private LocText currentMaterialDescription;

	// Token: 0x040077B0 RID: 30640
	[SerializeField]
	private DescriptorPanel descriptorPanel;

	// Token: 0x040077B1 RID: 30641
	[Header("Change Material")]
	[SerializeField]
	private MaterialSelectionPanel materialSelectionPanel;

	// Token: 0x040077B2 RID: 30642
	[SerializeField]
	private RectTransform confirmChangeRow;

	// Token: 0x040077B3 RID: 30643
	[SerializeField]
	private KButton orderChangeMaterialButton;

	// Token: 0x040077B4 RID: 30644
	[SerializeField]
	private KButton openChangeMaterialPanelButton;

	// Token: 0x040077B5 RID: 30645
	private int subHandle = -1;

	// Token: 0x040077B6 RID: 30646
	private Building building;
}
