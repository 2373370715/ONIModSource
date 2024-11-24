using System;
using System.Collections.Generic;
using Klei.AI;

// Token: 0x02001BEB RID: 7147
public interface IAttributeFormatter
{
	// Token: 0x170009AC RID: 2476
	// (get) Token: 0x0600948A RID: 38026
	// (set) Token: 0x0600948B RID: 38027
	GameUtil.TimeSlice DeltaTimeSlice { get; set; }

	// Token: 0x0600948C RID: 38028
	string GetFormattedAttribute(AttributeInstance instance);

	// Token: 0x0600948D RID: 38029
	string GetFormattedModifier(AttributeModifier modifier);

	// Token: 0x0600948E RID: 38030
	string GetFormattedValue(float value, GameUtil.TimeSlice timeSlice);

	// Token: 0x0600948F RID: 38031
	string GetTooltip(Klei.AI.Attribute master, AttributeInstance instance);

	// Token: 0x06009490 RID: 38032
	string GetTooltip(Klei.AI.Attribute master, List<AttributeModifier> modifiers, AttributeConverters converters);
}
