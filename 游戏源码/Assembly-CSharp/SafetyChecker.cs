using System;

// Token: 0x02000803 RID: 2051
public class SafetyChecker
{
	// Token: 0x17000108 RID: 264
	// (get) Token: 0x060024A9 RID: 9385 RVA: 0x000B7E39 File Offset: 0x000B6039
	// (set) Token: 0x060024AA RID: 9386 RVA: 0x000B7E41 File Offset: 0x000B6041
	public SafetyChecker.Condition[] conditions { get; private set; }

	// Token: 0x060024AB RID: 9387 RVA: 0x000B7E4A File Offset: 0x000B604A
	public SafetyChecker(SafetyChecker.Condition[] conditions)
	{
		this.conditions = conditions;
	}

	// Token: 0x060024AC RID: 9388 RVA: 0x001CA484 File Offset: 0x001C8684
	public int GetSafetyConditions(int cell, int cost, SafetyChecker.Context context, out bool all_conditions_met)
	{
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < this.conditions.Length; i++)
		{
			SafetyChecker.Condition condition = this.conditions[i];
			if (condition.callback(cell, cost, context))
			{
				num |= condition.mask;
				num2++;
			}
		}
		all_conditions_met = (num2 == this.conditions.Length);
		return num;
	}

	// Token: 0x02000804 RID: 2052
	public struct Condition
	{
		// Token: 0x17000109 RID: 265
		// (get) Token: 0x060024AD RID: 9389 RVA: 0x000B7E59 File Offset: 0x000B6059
		// (set) Token: 0x060024AE RID: 9390 RVA: 0x000B7E61 File Offset: 0x000B6061
		public SafetyChecker.Condition.Callback callback { readonly get; private set; }

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x060024AF RID: 9391 RVA: 0x000B7E6A File Offset: 0x000B606A
		// (set) Token: 0x060024B0 RID: 9392 RVA: 0x000B7E72 File Offset: 0x000B6072
		public int mask { readonly get; private set; }

		// Token: 0x060024B1 RID: 9393 RVA: 0x000B7E7B File Offset: 0x000B607B
		public Condition(string id, int condition_mask, SafetyChecker.Condition.Callback condition_callback)
		{
			this = default(SafetyChecker.Condition);
			this.callback = condition_callback;
			this.mask = condition_mask;
		}

		// Token: 0x02000805 RID: 2053
		// (Invoke) Token: 0x060024B3 RID: 9395
		public delegate bool Callback(int cell, int cost, SafetyChecker.Context context);
	}

	// Token: 0x02000806 RID: 2054
	public struct Context
	{
		// Token: 0x060024B6 RID: 9398 RVA: 0x001CA4E4 File Offset: 0x001C86E4
		public Context(KMonoBehaviour cmp)
		{
			this.cell = Grid.PosToCell(cmp);
			this.navigator = cmp.GetComponent<Navigator>();
			this.oxygenBreather = cmp.GetComponent<OxygenBreather>();
			this.minionBrain = cmp.GetComponent<MinionBrain>();
			this.temperatureTransferer = cmp.GetComponent<SimTemperatureTransfer>();
			this.primaryElement = cmp.GetComponent<PrimaryElement>();
			this.worldID = this.navigator.GetMyWorldId();
		}

		// Token: 0x040018C2 RID: 6338
		public Navigator navigator;

		// Token: 0x040018C3 RID: 6339
		public OxygenBreather oxygenBreather;

		// Token: 0x040018C4 RID: 6340
		public SimTemperatureTransfer temperatureTransferer;

		// Token: 0x040018C5 RID: 6341
		public PrimaryElement primaryElement;

		// Token: 0x040018C6 RID: 6342
		public MinionBrain minionBrain;

		// Token: 0x040018C7 RID: 6343
		public int worldID;

		// Token: 0x040018C8 RID: 6344
		public int cell;
	}
}
