using System;
using System.Collections;
using STRINGS;
using UnityEngine;

// Token: 0x02001FEF RID: 8175
public class ValveSideScreen : SideScreenContent
{
	// Token: 0x0600AD98 RID: 44440 RVA: 0x00412820 File Offset: 0x00410A20
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.unitsLabel.text = GameUtil.AddTimeSliceText(UI.UNITSUFFIXES.MASS.GRAM, GameUtil.TimeSlice.PerSecond);
		this.flowSlider.onReleaseHandle += this.OnReleaseHandle;
		this.flowSlider.onDrag += delegate()
		{
			this.ReceiveValueFromSlider(this.flowSlider.value);
		};
		this.flowSlider.onPointerDown += delegate()
		{
			this.ReceiveValueFromSlider(this.flowSlider.value);
		};
		this.flowSlider.onMove += delegate()
		{
			this.ReceiveValueFromSlider(this.flowSlider.value);
			this.OnReleaseHandle();
		};
		this.numberInput.onEndEdit += delegate()
		{
			this.ReceiveValueFromInput(this.numberInput.currentValue);
		};
		this.numberInput.decimalPlaces = 1;
	}

	// Token: 0x0600AD99 RID: 44441 RVA: 0x00111052 File Offset: 0x0010F252
	public void OnReleaseHandle()
	{
		this.targetValve.ChangeFlow(this.targetFlow);
	}

	// Token: 0x0600AD9A RID: 44442 RVA: 0x00111065 File Offset: 0x0010F265
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<Valve>() != null;
	}

	// Token: 0x0600AD9B RID: 44443 RVA: 0x004128D0 File Offset: 0x00410AD0
	public override void SetTarget(GameObject target)
	{
		this.targetValve = target.GetComponent<Valve>();
		if (this.targetValve == null)
		{
			global::Debug.LogError("The target object does not have a Valve component.");
			return;
		}
		this.flowSlider.minValue = 0f;
		this.flowSlider.maxValue = this.targetValve.MaxFlow;
		this.flowSlider.value = this.targetValve.DesiredFlow;
		this.minFlowLabel.text = GameUtil.GetFormattedMass(0f, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.Gram, true, "{0:0.#}");
		this.maxFlowLabel.text = GameUtil.GetFormattedMass(this.targetValve.MaxFlow, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.Gram, true, "{0:0.#}");
		this.numberInput.minValue = 0f;
		this.numberInput.maxValue = this.targetValve.MaxFlow * 1000f;
		this.numberInput.SetDisplayValue(GameUtil.GetFormattedMass(Mathf.Max(0f, this.targetValve.DesiredFlow), GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.Gram, false, "{0:0.#####}"));
		this.numberInput.Activate();
	}

	// Token: 0x0600AD9C RID: 44444 RVA: 0x00111073 File Offset: 0x0010F273
	private void ReceiveValueFromSlider(float newValue)
	{
		newValue = Mathf.Round(newValue * 1000f) / 1000f;
		this.UpdateFlowValue(newValue);
	}

	// Token: 0x0600AD9D RID: 44445 RVA: 0x004129E4 File Offset: 0x00410BE4
	private void ReceiveValueFromInput(float input)
	{
		float newValue = input / 1000f;
		this.UpdateFlowValue(newValue);
		this.targetValve.ChangeFlow(this.targetFlow);
	}

	// Token: 0x0600AD9E RID: 44446 RVA: 0x00111090 File Offset: 0x0010F290
	private void UpdateFlowValue(float newValue)
	{
		this.targetFlow = newValue;
		this.flowSlider.value = newValue;
		this.numberInput.SetDisplayValue(GameUtil.GetFormattedMass(newValue, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.Gram, false, "{0:0.#####}"));
	}

	// Token: 0x0600AD9F RID: 44447 RVA: 0x001110BE File Offset: 0x0010F2BE
	private IEnumerator SettingDelay(float delay)
	{
		float startTime = Time.realtimeSinceStartup;
		float currentTime = startTime;
		while (currentTime < startTime + delay)
		{
			currentTime += Time.unscaledDeltaTime;
			yield return SequenceUtil.WaitForEndOfFrame;
		}
		this.OnReleaseHandle();
		yield break;
	}

	// Token: 0x04008833 RID: 34867
	private Valve targetValve;

	// Token: 0x04008834 RID: 34868
	[Header("Slider")]
	[SerializeField]
	private KSlider flowSlider;

	// Token: 0x04008835 RID: 34869
	[SerializeField]
	private LocText minFlowLabel;

	// Token: 0x04008836 RID: 34870
	[SerializeField]
	private LocText maxFlowLabel;

	// Token: 0x04008837 RID: 34871
	[Header("Input Field")]
	[SerializeField]
	private KNumberInputField numberInput;

	// Token: 0x04008838 RID: 34872
	[SerializeField]
	private LocText unitsLabel;

	// Token: 0x04008839 RID: 34873
	private float targetFlow;
}
