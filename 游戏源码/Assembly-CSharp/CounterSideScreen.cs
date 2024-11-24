using System;
using STRINGS;
using UnityEngine;

// Token: 0x02001F5C RID: 8028
public class CounterSideScreen : SideScreenContent, IRender200ms
{
	// Token: 0x0600A972 RID: 43378 RVA: 0x0010D160 File Offset: 0x0010B360
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x0600A973 RID: 43379 RVA: 0x0040173C File Offset: 0x003FF93C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.resetButton.onClick += this.ResetCounter;
		this.incrementMaxButton.onClick += this.IncrementMaxCount;
		this.decrementMaxButton.onClick += this.DecrementMaxCount;
		this.incrementModeButton.onClick += this.ToggleMode;
		this.advancedModeToggle.onClick += this.ToggleAdvanced;
		this.maxCountInput.onEndEdit += delegate()
		{
			this.UpdateMaxCountFromTextInput(this.maxCountInput.currentValue);
		};
		this.UpdateCurrentCountLabel(this.targetLogicCounter.currentCount);
	}

	// Token: 0x0600A974 RID: 43380 RVA: 0x0010E0FB File Offset: 0x0010C2FB
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<LogicCounter>() != null;
	}

	// Token: 0x0600A975 RID: 43381 RVA: 0x004017EC File Offset: 0x003FF9EC
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		this.maxCountInput.minValue = 1f;
		this.maxCountInput.maxValue = 10f;
		this.targetLogicCounter = target.GetComponent<LogicCounter>();
		this.UpdateCurrentCountLabel(this.targetLogicCounter.currentCount);
		this.UpdateMaxCountLabel(this.targetLogicCounter.maxCount);
		this.advancedModeCheckmark.enabled = this.targetLogicCounter.advancedMode;
	}

	// Token: 0x0600A976 RID: 43382 RVA: 0x0010E109 File Offset: 0x0010C309
	public void Render200ms(float dt)
	{
		if (this.targetLogicCounter == null)
		{
			return;
		}
		this.UpdateCurrentCountLabel(this.targetLogicCounter.currentCount);
	}

	// Token: 0x0600A977 RID: 43383 RVA: 0x00401864 File Offset: 0x003FFA64
	private void UpdateCurrentCountLabel(int value)
	{
		string text = value.ToString();
		if (value == this.targetLogicCounter.maxCount)
		{
			text = UI.FormatAsAutomationState(text, UI.AutomationState.Active);
		}
		else
		{
			text = UI.FormatAsAutomationState(text, UI.AutomationState.Standby);
		}
		this.currentCount.text = (this.targetLogicCounter.advancedMode ? string.Format(UI.UISIDESCREENS.COUNTER_SIDE_SCREEN.CURRENT_COUNT_ADVANCED, text) : string.Format(UI.UISIDESCREENS.COUNTER_SIDE_SCREEN.CURRENT_COUNT_SIMPLE, text));
	}

	// Token: 0x0600A978 RID: 43384 RVA: 0x0010E12B File Offset: 0x0010C32B
	private void UpdateMaxCountLabel(int value)
	{
		this.maxCountInput.SetAmount((float)value);
	}

	// Token: 0x0600A979 RID: 43385 RVA: 0x0010E13A File Offset: 0x0010C33A
	private void UpdateMaxCountFromTextInput(float newValue)
	{
		this.SetMaxCount((int)newValue);
	}

	// Token: 0x0600A97A RID: 43386 RVA: 0x0010E144 File Offset: 0x0010C344
	private void IncrementMaxCount()
	{
		this.SetMaxCount(this.targetLogicCounter.maxCount + 1);
	}

	// Token: 0x0600A97B RID: 43387 RVA: 0x0010E159 File Offset: 0x0010C359
	private void DecrementMaxCount()
	{
		this.SetMaxCount(this.targetLogicCounter.maxCount - 1);
	}

	// Token: 0x0600A97C RID: 43388 RVA: 0x004018D4 File Offset: 0x003FFAD4
	private void SetMaxCount(int newValue)
	{
		if (newValue > 10)
		{
			newValue = 1;
		}
		if (newValue < 1)
		{
			newValue = 10;
		}
		if (newValue < this.targetLogicCounter.currentCount)
		{
			this.targetLogicCounter.currentCount = newValue;
		}
		this.targetLogicCounter.maxCount = newValue;
		this.UpdateCounterStates();
		this.UpdateMaxCountLabel(newValue);
	}

	// Token: 0x0600A97D RID: 43389 RVA: 0x0010E16E File Offset: 0x0010C36E
	private void ResetCounter()
	{
		this.targetLogicCounter.ResetCounter();
	}

	// Token: 0x0600A97E RID: 43390 RVA: 0x0010E17B File Offset: 0x0010C37B
	private void UpdateCounterStates()
	{
		this.targetLogicCounter.SetCounterState();
		this.targetLogicCounter.UpdateLogicCircuit();
		this.targetLogicCounter.UpdateVisualState(true);
		this.targetLogicCounter.UpdateMeter();
	}

	// Token: 0x0600A97F RID: 43391 RVA: 0x000A5E40 File Offset: 0x000A4040
	private void ToggleMode()
	{
	}

	// Token: 0x0600A980 RID: 43392 RVA: 0x00401924 File Offset: 0x003FFB24
	private void ToggleAdvanced()
	{
		this.targetLogicCounter.advancedMode = !this.targetLogicCounter.advancedMode;
		this.advancedModeCheckmark.enabled = this.targetLogicCounter.advancedMode;
		this.UpdateCurrentCountLabel(this.targetLogicCounter.currentCount);
		this.UpdateCounterStates();
	}

	// Token: 0x04008546 RID: 34118
	public LogicCounter targetLogicCounter;

	// Token: 0x04008547 RID: 34119
	public KButton resetButton;

	// Token: 0x04008548 RID: 34120
	public KButton incrementMaxButton;

	// Token: 0x04008549 RID: 34121
	public KButton decrementMaxButton;

	// Token: 0x0400854A RID: 34122
	public KButton incrementModeButton;

	// Token: 0x0400854B RID: 34123
	public KToggle advancedModeToggle;

	// Token: 0x0400854C RID: 34124
	public KImage advancedModeCheckmark;

	// Token: 0x0400854D RID: 34125
	public LocText currentCount;

	// Token: 0x0400854E RID: 34126
	[SerializeField]
	private KNumberInputField maxCountInput;
}
