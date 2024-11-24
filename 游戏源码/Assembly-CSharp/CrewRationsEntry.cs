using System;
using Klei.AI;
using UnityEngine;

// Token: 0x02001D9E RID: 7582
public class CrewRationsEntry : CrewListEntry
{
	// Token: 0x06009E8A RID: 40586 RVA: 0x00107401 File Offset: 0x00105601
	public override void Populate(MinionIdentity _identity)
	{
		base.Populate(_identity);
		this.rationMonitor = _identity.GetSMI<RationMonitor.Instance>();
		this.Refresh();
	}

	// Token: 0x06009E8B RID: 40587 RVA: 0x003CC234 File Offset: 0x003CA434
	public override void Refresh()
	{
		base.Refresh();
		this.rationsEatenToday.text = GameUtil.GetFormattedCalories(this.rationMonitor.GetRationsAteToday(), GameUtil.TimeSlice.None, true);
		if (this.identity == null)
		{
			return;
		}
		foreach (AmountInstance amountInstance in this.identity.GetAmounts())
		{
			float min = amountInstance.GetMin();
			float max = amountInstance.GetMax();
			float num = max - min;
			string str = Mathf.RoundToInt((num - (max - amountInstance.value)) / num * 100f).ToString();
			if (amountInstance.amount == Db.Get().Amounts.Stress)
			{
				this.currentStressText.text = amountInstance.GetValueString();
				this.currentStressText.GetComponent<ToolTip>().toolTip = amountInstance.GetTooltip();
				this.stressTrendImage.SetValue(amountInstance);
			}
			else if (amountInstance.amount == Db.Get().Amounts.Calories)
			{
				this.currentCaloriesText.text = str + "%";
				this.currentCaloriesText.GetComponent<ToolTip>().toolTip = amountInstance.GetTooltip();
			}
			else if (amountInstance.amount == Db.Get().Amounts.HitPoints)
			{
				this.currentHealthText.text = str + "%";
				this.currentHealthText.GetComponent<ToolTip>().toolTip = amountInstance.GetTooltip();
			}
		}
	}

	// Token: 0x04007C49 RID: 31817
	public KButton incRationPerDayButton;

	// Token: 0x04007C4A RID: 31818
	public KButton decRationPerDayButton;

	// Token: 0x04007C4B RID: 31819
	public LocText rationPerDayText;

	// Token: 0x04007C4C RID: 31820
	public LocText rationsEatenToday;

	// Token: 0x04007C4D RID: 31821
	public LocText currentCaloriesText;

	// Token: 0x04007C4E RID: 31822
	public LocText currentStressText;

	// Token: 0x04007C4F RID: 31823
	public LocText currentHealthText;

	// Token: 0x04007C50 RID: 31824
	public ValueTrendImageToggle stressTrendImage;

	// Token: 0x04007C51 RID: 31825
	private RationMonitor.Instance rationMonitor;
}
