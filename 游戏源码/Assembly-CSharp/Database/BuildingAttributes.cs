using System;
using Klei.AI;

namespace Database
{
	// Token: 0x0200211C RID: 8476
	public class BuildingAttributes : ResourceSet<Klei.AI.Attribute>
	{
		// Token: 0x0600B40D RID: 46093 RVA: 0x0043DA30 File Offset: 0x0043BC30
		public BuildingAttributes(ResourceSet parent) : base("BuildingAttributes", parent)
		{
			this.Decor = base.Add(new Klei.AI.Attribute("Decor", true, Klei.AI.Attribute.Display.General, false, 0f, null, null, null, null));
			this.DecorRadius = base.Add(new Klei.AI.Attribute("DecorRadius", true, Klei.AI.Attribute.Display.General, false, 0f, null, null, null, null));
			this.NoisePollution = base.Add(new Klei.AI.Attribute("NoisePollution", true, Klei.AI.Attribute.Display.General, false, 0f, null, null, null, null));
			this.NoisePollutionRadius = base.Add(new Klei.AI.Attribute("NoisePollutionRadius", true, Klei.AI.Attribute.Display.General, false, 0f, null, null, null, null));
			this.Hygiene = base.Add(new Klei.AI.Attribute("Hygiene", true, Klei.AI.Attribute.Display.General, false, 0f, null, null, null, null));
			this.Comfort = base.Add(new Klei.AI.Attribute("Comfort", true, Klei.AI.Attribute.Display.General, false, 0f, null, null, null, null));
			this.OverheatTemperature = base.Add(new Klei.AI.Attribute("OverheatTemperature", true, Klei.AI.Attribute.Display.General, false, 0f, null, null, null, null));
			this.OverheatTemperature.SetFormatter(new StandardAttributeFormatter(GameUtil.UnitClass.Temperature, GameUtil.TimeSlice.ModifyOnly));
			this.FatalTemperature = base.Add(new Klei.AI.Attribute("FatalTemperature", true, Klei.AI.Attribute.Display.General, false, 0f, null, null, null, null));
			this.FatalTemperature.SetFormatter(new StandardAttributeFormatter(GameUtil.UnitClass.Temperature, GameUtil.TimeSlice.ModifyOnly));
		}

		// Token: 0x04008E84 RID: 36484
		public Klei.AI.Attribute Decor;

		// Token: 0x04008E85 RID: 36485
		public Klei.AI.Attribute DecorRadius;

		// Token: 0x04008E86 RID: 36486
		public Klei.AI.Attribute NoisePollution;

		// Token: 0x04008E87 RID: 36487
		public Klei.AI.Attribute NoisePollutionRadius;

		// Token: 0x04008E88 RID: 36488
		public Klei.AI.Attribute Hygiene;

		// Token: 0x04008E89 RID: 36489
		public Klei.AI.Attribute Comfort;

		// Token: 0x04008E8A RID: 36490
		public Klei.AI.Attribute OverheatTemperature;

		// Token: 0x04008E8B RID: 36491
		public Klei.AI.Attribute FatalTemperature;
	}
}
