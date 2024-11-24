using System;
using UnityEngine;

// Token: 0x02001F3C RID: 7996
public class CapacityControlSideScreen : SideScreenContent
{
	// Token: 0x0600A8C9 RID: 43209 RVA: 0x003FE448 File Offset: 0x003FC648
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.unitsLabel.text = this.target.CapacityUnits;
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

	// Token: 0x0600A8CA RID: 43210 RVA: 0x0010DA01 File Offset: 0x0010BC01
	public override bool IsValidForTarget(GameObject target)
	{
		return !target.GetComponent<IUserControlledCapacity>().IsNullOrDestroyed() || target.GetSMI<IUserControlledCapacity>() != null;
	}

	// Token: 0x0600A8CB RID: 43211 RVA: 0x003FE4E0 File Offset: 0x003FC6E0
	public override void SetTarget(GameObject new_target)
	{
		if (new_target == null)
		{
			global::Debug.LogError("Invalid gameObject received");
			return;
		}
		this.target = new_target.GetComponent<IUserControlledCapacity>();
		if (this.target == null)
		{
			this.target = new_target.GetSMI<IUserControlledCapacity>();
		}
		if (this.target == null)
		{
			global::Debug.LogError("The gameObject received does not contain a IThresholdSwitch component");
			return;
		}
		this.slider.minValue = this.target.MinCapacity;
		this.slider.maxValue = this.target.MaxCapacity;
		this.slider.value = this.target.UserMaxCapacity;
		this.slider.GetComponentInChildren<ToolTip>();
		this.unitsLabel.text = this.target.CapacityUnits;
		this.numberInput.minValue = this.target.MinCapacity;
		this.numberInput.maxValue = this.target.MaxCapacity;
		this.numberInput.currentValue = Mathf.Max(this.target.MinCapacity, Mathf.Min(this.target.MaxCapacity, this.target.UserMaxCapacity));
		this.numberInput.Activate();
		this.UpdateMaxCapacityLabel();
	}

	// Token: 0x0600A8CC RID: 43212 RVA: 0x0010DA1B File Offset: 0x0010BC1B
	private void ReceiveValueFromSlider(float newValue)
	{
		this.UpdateMaxCapacity(newValue);
	}

	// Token: 0x0600A8CD RID: 43213 RVA: 0x0010DA1B File Offset: 0x0010BC1B
	private void ReceiveValueFromInput(float newValue)
	{
		this.UpdateMaxCapacity(newValue);
	}

	// Token: 0x0600A8CE RID: 43214 RVA: 0x0010DA24 File Offset: 0x0010BC24
	private void UpdateMaxCapacity(float newValue)
	{
		this.target.UserMaxCapacity = newValue;
		this.slider.value = newValue;
		this.UpdateMaxCapacityLabel();
	}

	// Token: 0x0600A8CF RID: 43215 RVA: 0x003FE610 File Offset: 0x003FC810
	private void UpdateMaxCapacityLabel()
	{
		this.numberInput.SetDisplayValue(this.target.UserMaxCapacity.ToString());
	}

	// Token: 0x040084B0 RID: 33968
	private IUserControlledCapacity target;

	// Token: 0x040084B1 RID: 33969
	[Header("Slider")]
	[SerializeField]
	private KSlider slider;

	// Token: 0x040084B2 RID: 33970
	[Header("Number Input")]
	[SerializeField]
	private KNumberInputField numberInput;

	// Token: 0x040084B3 RID: 33971
	[SerializeField]
	private LocText unitsLabel;
}
