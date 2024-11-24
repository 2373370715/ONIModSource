using System;
using Klei.AI;

// Token: 0x02001BDA RID: 7130
public interface IAmountDisplayer
{
	// Token: 0x06009455 RID: 37973
	string GetValueString(Amount master, AmountInstance instance);

	// Token: 0x06009456 RID: 37974
	string GetDescription(Amount master, AmountInstance instance);

	// Token: 0x06009457 RID: 37975
	string GetTooltip(Amount master, AmountInstance instance);

	// Token: 0x170009A7 RID: 2471
	// (get) Token: 0x06009458 RID: 37976
	IAttributeFormatter Formatter { get; }
}
