using System;
using Klei.AI;

namespace Database
{
	// Token: 0x02002130 RID: 8496
	public class CritterAttributes : ResourceSet<Klei.AI.Attribute>
	{
		// Token: 0x0600B50E RID: 46350 RVA: 0x0044B090 File Offset: 0x00449290
		public CritterAttributes(ResourceSet parent) : base("CritterAttributes", parent)
		{
			this.Happiness = base.Add(new Klei.AI.Attribute("Happiness", Strings.Get("STRINGS.CREATURES.STATS.HAPPINESS.NAME"), "", Strings.Get("STRINGS.CREATURES.STATS.HAPPINESS.TOOLTIP"), 0f, Klei.AI.Attribute.Display.General, false, "ui_icon_happiness", null, null));
			this.Happiness.SetFormatter(new StandardAttributeFormatter(GameUtil.UnitClass.SimpleInteger, GameUtil.TimeSlice.None));
			this.Metabolism = base.Add(new Klei.AI.Attribute("Metabolism", false, Klei.AI.Attribute.Display.Details, false, 100f, "ui_icon_metabolism", null, null, null));
			this.Metabolism.SetFormatter(new ToPercentAttributeFormatter(100f, GameUtil.TimeSlice.None));
		}

		// Token: 0x0400918C RID: 37260
		public Klei.AI.Attribute Happiness;

		// Token: 0x0400918D RID: 37261
		public Klei.AI.Attribute Metabolism;
	}
}
