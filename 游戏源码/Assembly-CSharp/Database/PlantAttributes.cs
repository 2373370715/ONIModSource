using System;
using Klei.AI;

namespace Database
{
	// Token: 0x02002158 RID: 8536
	public class PlantAttributes : ResourceSet<Klei.AI.Attribute>
	{
		// Token: 0x0600B5C1 RID: 46529 RVA: 0x00452C98 File Offset: 0x00450E98
		public PlantAttributes(ResourceSet parent) : base("PlantAttributes", parent)
		{
			this.WiltTempRangeMod = base.Add(new Klei.AI.Attribute("WiltTempRangeMod", false, Klei.AI.Attribute.Display.Normal, false, 1f, null, null, null, null));
			this.WiltTempRangeMod.SetFormatter(new PercentAttributeFormatter());
			this.YieldAmount = base.Add(new Klei.AI.Attribute("YieldAmount", false, Klei.AI.Attribute.Display.Normal, false, 0f, null, null, null, null));
			this.YieldAmount.SetFormatter(new PercentAttributeFormatter());
			this.HarvestTime = base.Add(new Klei.AI.Attribute("HarvestTime", false, Klei.AI.Attribute.Display.Normal, false, 0f, null, null, null, null));
			this.HarvestTime.SetFormatter(new StandardAttributeFormatter(GameUtil.UnitClass.Time, GameUtil.TimeSlice.None));
			this.DecorBonus = base.Add(new Klei.AI.Attribute("DecorBonus", false, Klei.AI.Attribute.Display.Normal, false, 0f, null, null, null, null));
			this.DecorBonus.SetFormatter(new StandardAttributeFormatter(GameUtil.UnitClass.SimpleInteger, GameUtil.TimeSlice.None));
			this.MinLightLux = base.Add(new Klei.AI.Attribute("MinLightLux", false, Klei.AI.Attribute.Display.Normal, false, 0f, null, null, null, null));
			this.MinLightLux.SetFormatter(new StandardAttributeFormatter(GameUtil.UnitClass.Lux, GameUtil.TimeSlice.None));
			this.FertilizerUsageMod = base.Add(new Klei.AI.Attribute("FertilizerUsageMod", false, Klei.AI.Attribute.Display.Normal, false, 1f, null, null, null, null));
			this.FertilizerUsageMod.SetFormatter(new PercentAttributeFormatter());
			this.MinRadiationThreshold = base.Add(new Klei.AI.Attribute("MinRadiationThreshold", false, Klei.AI.Attribute.Display.Normal, false, 0f, null, null, null, null));
			this.MinRadiationThreshold.SetFormatter(new RadsPerCycleAttributeFormatter());
			this.MaxRadiationThreshold = base.Add(new Klei.AI.Attribute("MaxRadiationThreshold", false, Klei.AI.Attribute.Display.Normal, false, 0f, null, null, null, null));
			this.MaxRadiationThreshold.SetFormatter(new RadsPerCycleAttributeFormatter());
		}

		// Token: 0x040093C6 RID: 37830
		public Klei.AI.Attribute WiltTempRangeMod;

		// Token: 0x040093C7 RID: 37831
		public Klei.AI.Attribute YieldAmount;

		// Token: 0x040093C8 RID: 37832
		public Klei.AI.Attribute HarvestTime;

		// Token: 0x040093C9 RID: 37833
		public Klei.AI.Attribute DecorBonus;

		// Token: 0x040093CA RID: 37834
		public Klei.AI.Attribute MinLightLux;

		// Token: 0x040093CB RID: 37835
		public Klei.AI.Attribute FertilizerUsageMod;

		// Token: 0x040093CC RID: 37836
		public Klei.AI.Attribute MinRadiationThreshold;

		// Token: 0x040093CD RID: 37837
		public Klei.AI.Attribute MaxRadiationThreshold;
	}
}
