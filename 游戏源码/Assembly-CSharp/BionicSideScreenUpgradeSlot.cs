using System;
using System.Collections;
using STRINGS;
using UnityEngine;

// Token: 0x02001F36 RID: 7990
public class BionicSideScreenUpgradeSlot : KMonoBehaviour
{
	// Token: 0x17000ABF RID: 2751
	// (get) Token: 0x0600A8A1 RID: 43169 RVA: 0x0010D84F File Offset: 0x0010BA4F
	// (set) Token: 0x0600A8A0 RID: 43168 RVA: 0x0010D846 File Offset: 0x0010BA46
	public BionicUpgradesMonitor.UpgradeComponentSlot upgradeSlot { get; private set; }

	// Token: 0x0600A8A2 RID: 43170 RVA: 0x0010D857 File Offset: 0x0010BA57
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		MultiToggle multiToggle = this.toggle;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(this.OnSlotClicked));
	}

	// Token: 0x0600A8A3 RID: 43171 RVA: 0x003FDBB4 File Offset: 0x003FBDB4
	public void Setup(BionicUpgradesMonitor.UpgradeComponentSlot upgradeSlot)
	{
		if (this.upgradeSlot != null)
		{
			BionicUpgradesMonitor.UpgradeComponentSlot upgradeSlot2 = this.upgradeSlot;
			upgradeSlot2.OnAssignedUpgradeChanged = (Action<BionicUpgradesMonitor.UpgradeComponentSlot>)Delegate.Remove(upgradeSlot2.OnAssignedUpgradeChanged, new Action<BionicUpgradesMonitor.UpgradeComponentSlot>(this.OnAssignedUpgradeChanged));
		}
		this.upgradeSlot = upgradeSlot;
		if (upgradeSlot != null)
		{
			upgradeSlot.OnAssignedUpgradeChanged = (Action<BionicUpgradesMonitor.UpgradeComponentSlot>)Delegate.Combine(upgradeSlot.OnAssignedUpgradeChanged, new Action<BionicUpgradesMonitor.UpgradeComponentSlot>(this.OnAssignedUpgradeChanged));
		}
		this.Refresh();
	}

	// Token: 0x0600A8A4 RID: 43172 RVA: 0x0010D886 File Offset: 0x0010BA86
	private void OnAssignedUpgradeChanged(BionicUpgradesMonitor.UpgradeComponentSlot slot)
	{
		this.Refresh();
	}

	// Token: 0x0600A8A5 RID: 43173 RVA: 0x003FDC24 File Offset: 0x003FBE24
	public void Refresh()
	{
		if (this.wattageLabelEffectCoroutine != null)
		{
			base.StopCoroutine(this.wattageLabelEffectCoroutine);
			this.wattageLabelEffectCoroutine = null;
		}
		this.label.color = this.standardColor;
		if (this.upgradeSlot != null && this.upgradeSlot.HasUpgradeInstalled)
		{
			this.icon.sprite = Def.GetUISprite(this.upgradeSlot.installedUpgradeComponent.gameObject, "ui", false).first;
			this.icon.Opacity(1f);
			float currentWattage = this.upgradeSlot.installedUpgradeComponent.CurrentWattage;
			float potentialWattage = this.upgradeSlot.installedUpgradeComponent.PotentialWattage;
			bool flag = currentWattage != 0f;
			string str = flag ? ("<b>" + ((currentWattage >= 0f) ? "+" : "-") + "</b>") : "";
			this.label.SetText(string.Format(BionicSideScreenUpgradeSlot.TEXT_UPGRADE_WATTS, str + GameUtil.GetFormattedWattage((currentWattage != 0f) ? currentWattage : potentialWattage, GameUtil.WattageFormatterUnit.Automatic, true)));
			this.label.Opacity((currentWattage != 0f) ? 1f : 0.5f);
			this.icon.gameObject.SetActive(true);
			if (flag && base.gameObject.activeInHierarchy)
			{
				this.wattageLabelEffectCoroutine = base.StartCoroutine(this.UpgradeInUse_WattageLabelEffects());
			}
			this.tooltip.SizingSetting = ToolTip.ToolTipSizeSetting.MaxWidthWrapContent;
			if (flag)
			{
				string text = this.activeColorTooltip.ToHexString();
				str = string.Concat(new string[]
				{
					"<color=#",
					text,
					"><b>",
					(currentWattage >= 0f) ? "+" : "-",
					"</b>"
				});
				this.tooltip.SetSimpleTooltip(string.Format(BionicSideScreenUpgradeSlot.TEXT_TOOLTIP_INSTALLED_IN_USE, this.upgradeSlot.installedUpgradeComponent.GetProperName(), str + GameUtil.GetFormattedWattage(currentWattage, GameUtil.WattageFormatterUnit.Automatic, true) + "</color>", this.upgradeSlot.installedUpgradeComponent.GetComponent<InfoDescription>().description));
			}
			else
			{
				this.tooltip.SetSimpleTooltip(string.Format(BionicSideScreenUpgradeSlot.TEXT_TOOLTIP_INSTALLED_NOT_IN_USE, this.upgradeSlot.installedUpgradeComponent.GetProperName(), GameUtil.GetFormattedWattage(potentialWattage, GameUtil.WattageFormatterUnit.Automatic, true), this.upgradeSlot.installedUpgradeComponent.GetComponent<InfoDescription>().description));
			}
		}
		else if (this.upgradeSlot != null && this.upgradeSlot.HasUpgradeComponentAssigned && !this.upgradeSlot.GetAssignableSlotInstance().IsUnassigning())
		{
			this.icon.sprite = Def.GetUISprite(this.upgradeSlot.assignedUpgradeComponent.gameObject, "ui", false).first;
			this.icon.Opacity(0.5f);
			this.label.SetText(BionicSideScreenUpgradeSlot.TEXT_UPGRADE_ASSIGNED_NOT_INSTALLED);
			this.label.Opacity(1f);
			this.icon.gameObject.SetActive(true);
			this.tooltip.SizingSetting = ToolTip.ToolTipSizeSetting.MaxWidthWrapContent;
			this.tooltip.SetSimpleTooltip(string.Format(BionicSideScreenUpgradeSlot.TEXT_TOOLTIP_ASSIGNED, this.upgradeSlot.assignedUpgradeComponent.GetProperName()));
		}
		else
		{
			this.tooltip.SizingSetting = ToolTip.ToolTipSizeSetting.DynamicWidthNoWrap;
			this.tooltip.SetSimpleTooltip(BionicSideScreenUpgradeSlot.TEXT_TOOLTIP_EMPTY);
			this.label.SetText(BionicSideScreenUpgradeSlot.TEXT_NO_UPGRADE_INSTALLED);
			this.label.Opacity(1f);
			this.icon.gameObject.SetActive(false);
		}
		this.SetSelected(this._isSelected);
	}

	// Token: 0x0600A8A6 RID: 43174 RVA: 0x0010D88E File Offset: 0x0010BA8E
	private void OnSlotClicked()
	{
		Action<BionicSideScreenUpgradeSlot> onClick = this.OnClick;
		if (onClick == null)
		{
			return;
		}
		onClick(this);
	}

	// Token: 0x0600A8A7 RID: 43175 RVA: 0x003FDFAC File Offset: 0x003FC1AC
	public void SetSelected(bool isSelected)
	{
		this._isSelected = isSelected;
		bool flag = this.upgradeSlot != null && this.upgradeSlot.HasUpgradeComponentAssigned && !this.upgradeSlot.GetAssignableSlotInstance().IsUnassigning();
		bool flag2 = flag && this.upgradeSlot.assignedUpgradeComponent.Rarity == BionicUpgradeComponentConfig.RarityType.Special;
		this.toggle.ChangeState((isSelected ? 1 : 0) + (flag ? 2 : 0) + ((flag && flag2) ? 2 : 0));
	}

	// Token: 0x0600A8A8 RID: 43176 RVA: 0x0010D8A1 File Offset: 0x0010BAA1
	private IEnumerator UpgradeInUse_WattageLabelEffects()
	{
		while (base.gameObject.activeInHierarchy)
		{
			float t = (Mathf.Sin(((this.inUseAnimationDuration <= 0f) ? 0f : (Time.time / this.inUseAnimationDuration * 2f * 3.1415927f)) - 1.5707964f) + 1f) * 0.5f;
			Color color = Color.Lerp(this.standardColor, this.activeColor, t);
			this.label.color = color;
			yield return null;
		}
		yield break;
	}

	// Token: 0x0400848E RID: 33934
	public static string TEXT_NO_UPGRADE_INSTALLED = UI.UISIDESCREENS.BIONIC_SIDE_SCREEN.UPGRADE_SLOT_EMPTY;

	// Token: 0x0400848F RID: 33935
	public static string TEXT_UPGRADE_ASSIGNED_NOT_INSTALLED = UI.UISIDESCREENS.BIONIC_SIDE_SCREEN.UPGRADE_SLOT_ASSIGNED;

	// Token: 0x04008490 RID: 33936
	public static string TEXT_UPGRADE_WATTS = UI.UISIDESCREENS.BIONIC_SIDE_SCREEN.UPGRADE_SLOT_WATTAGE;

	// Token: 0x04008491 RID: 33937
	public static string TEXT_TOOLTIP_EMPTY = UI.UISIDESCREENS.BIONIC_SIDE_SCREEN.TOOLTIP.SLOT_EMPTY;

	// Token: 0x04008492 RID: 33938
	public static string TEXT_TOOLTIP_ASSIGNED = UI.UISIDESCREENS.BIONIC_SIDE_SCREEN.TOOLTIP.SLOT_ASSIGNED;

	// Token: 0x04008493 RID: 33939
	public static string TEXT_TOOLTIP_INSTALLED_NOT_IN_USE = UI.UISIDESCREENS.BIONIC_SIDE_SCREEN.TOOLTIP.SLOT_INSTALLED_NOT_IN_USE;

	// Token: 0x04008494 RID: 33940
	public static string TEXT_TOOLTIP_INSTALLED_IN_USE = UI.UISIDESCREENS.BIONIC_SIDE_SCREEN.TOOLTIP.SLOT_INSTALLED_IN_USE;

	// Token: 0x04008495 RID: 33941
	public MultiToggle toggle;

	// Token: 0x04008496 RID: 33942
	public KImage icon;

	// Token: 0x04008497 RID: 33943
	public LocText label;

	// Token: 0x04008498 RID: 33944
	public ToolTip tooltip;

	// Token: 0x04008499 RID: 33945
	[Header("Effects settings")]
	public float inUseAnimationDuration = 0.5f;

	// Token: 0x0400849A RID: 33946
	public Color standardColor = Color.black;

	// Token: 0x0400849B RID: 33947
	public Color activeColor = Color.blue;

	// Token: 0x0400849C RID: 33948
	public Color activeColorTooltip = Color.blue;

	// Token: 0x0400849D RID: 33949
	public Action<BionicSideScreenUpgradeSlot> OnClick;

	// Token: 0x0400849F RID: 33951
	private bool _isSelected;

	// Token: 0x040084A0 RID: 33952
	private Coroutine wattageLabelEffectCoroutine;
}
