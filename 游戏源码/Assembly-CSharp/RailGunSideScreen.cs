using System;
using STRINGS;
using UnityEngine;

// Token: 0x02001FAC RID: 8108
public class RailGunSideScreen : SideScreenContent
{
	// Token: 0x0600AB5B RID: 43867 RVA: 0x00409DAC File Offset: 0x00407FAC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.unitsLabel.text = GameUtil.GetCurrentMassUnit(false);
		this.slider.onDrag += delegate()
		{
			this.ReceiveValueFromSlider(this.slider.value);
		};
		this.slider.onPointerDown += delegate()
		{
			this.ReceiveValueFromSlider(this.slider.value);
		};
		this.slider.onMove += delegate()
		{
			this.ReceiveValueFromSlider(this.slider.value);
		};
		this.numberInput.onEndEdit += delegate()
		{
			this.ReceiveValueFromInput(this.numberInput.currentValue);
		};
		this.numberInput.decimalPlaces = 1;
	}

	// Token: 0x0600AB5C RID: 43868 RVA: 0x0010F444 File Offset: 0x0010D644
	protected override void OnCmpDisable()
	{
		base.OnCmpDisable();
		if (this.selectedGun)
		{
			this.selectedGun = null;
		}
	}

	// Token: 0x0600AB5D RID: 43869 RVA: 0x0010F460 File Offset: 0x0010D660
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		if (this.selectedGun)
		{
			this.selectedGun = null;
		}
	}

	// Token: 0x0600AB5E RID: 43870 RVA: 0x0010F47C File Offset: 0x0010D67C
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<RailGun>() != null;
	}

	// Token: 0x0600AB5F RID: 43871 RVA: 0x00409E40 File Offset: 0x00408040
	public override void SetTarget(GameObject new_target)
	{
		if (new_target == null)
		{
			global::Debug.LogError("Invalid gameObject received");
			return;
		}
		this.selectedGun = new_target.GetComponent<RailGun>();
		if (this.selectedGun == null)
		{
			global::Debug.LogError("The gameObject received does not contain a RailGun component");
			return;
		}
		this.targetRailgunHEPStorageSubHandle = this.selectedGun.Subscribe(-1837862626, new Action<object>(this.UpdateHEPLabels));
		this.slider.minValue = this.selectedGun.MinLaunchMass;
		this.slider.maxValue = this.selectedGun.MaxLaunchMass;
		this.slider.value = this.selectedGun.launchMass;
		this.unitsLabel.text = GameUtil.GetCurrentMassUnit(false);
		this.numberInput.minValue = this.selectedGun.MinLaunchMass;
		this.numberInput.maxValue = this.selectedGun.MaxLaunchMass;
		this.numberInput.currentValue = Mathf.Max(this.selectedGun.MinLaunchMass, Mathf.Min(this.selectedGun.MaxLaunchMass, this.selectedGun.launchMass));
		this.UpdateMaxCapacityLabel();
		this.numberInput.Activate();
		this.UpdateHEPLabels(null);
	}

	// Token: 0x0600AB60 RID: 43872 RVA: 0x0010F48A File Offset: 0x0010D68A
	public override void ClearTarget()
	{
		if (this.targetRailgunHEPStorageSubHandle != -1 && this.selectedGun != null)
		{
			this.selectedGun.Unsubscribe(this.targetRailgunHEPStorageSubHandle);
			this.targetRailgunHEPStorageSubHandle = -1;
		}
		this.selectedGun = null;
	}

	// Token: 0x0600AB61 RID: 43873 RVA: 0x00409F7C File Offset: 0x0040817C
	public void UpdateHEPLabels(object data = null)
	{
		if (this.selectedGun == null)
		{
			return;
		}
		string text = BUILDINGS.PREFABS.RAILGUN.SIDESCREEN_HEP_REQUIRED;
		text = text.Replace("{current}", this.selectedGun.CurrentEnergy.ToString());
		text = text.Replace("{required}", this.selectedGun.EnergyCost.ToString());
		this.hepStorageInfo.text = text;
	}

	// Token: 0x0600AB62 RID: 43874 RVA: 0x0010F4C2 File Offset: 0x0010D6C2
	private void ReceiveValueFromSlider(float newValue)
	{
		this.UpdateMaxCapacity(newValue);
	}

	// Token: 0x0600AB63 RID: 43875 RVA: 0x0010F4C2 File Offset: 0x0010D6C2
	private void ReceiveValueFromInput(float newValue)
	{
		this.UpdateMaxCapacity(newValue);
	}

	// Token: 0x0600AB64 RID: 43876 RVA: 0x0010F4CB File Offset: 0x0010D6CB
	private void UpdateMaxCapacity(float newValue)
	{
		this.selectedGun.launchMass = newValue;
		this.slider.value = newValue;
		this.UpdateMaxCapacityLabel();
		this.selectedGun.Trigger(161772031, null);
	}

	// Token: 0x0600AB65 RID: 43877 RVA: 0x0010F4FC File Offset: 0x0010D6FC
	private void UpdateMaxCapacityLabel()
	{
		this.numberInput.SetDisplayValue(this.selectedGun.launchMass.ToString());
	}

	// Token: 0x040086A9 RID: 34473
	public GameObject content;

	// Token: 0x040086AA RID: 34474
	private RailGun selectedGun;

	// Token: 0x040086AB RID: 34475
	public LocText DescriptionText;

	// Token: 0x040086AC RID: 34476
	[Header("Slider")]
	[SerializeField]
	private KSlider slider;

	// Token: 0x040086AD RID: 34477
	[Header("Number Input")]
	[SerializeField]
	private KNumberInputField numberInput;

	// Token: 0x040086AE RID: 34478
	[SerializeField]
	private LocText unitsLabel;

	// Token: 0x040086AF RID: 34479
	[SerializeField]
	private LocText hepStorageInfo;

	// Token: 0x040086B0 RID: 34480
	private int targetRailgunHEPStorageSubHandle = -1;
}
