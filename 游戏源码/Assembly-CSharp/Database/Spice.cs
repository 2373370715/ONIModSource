using System;
using Klei.AI;
using UnityEngine;

namespace Database
{
	// Token: 0x02002167 RID: 8551
	public class Spice : Resource
	{
		// Token: 0x17000BB8 RID: 3000
		// (get) Token: 0x0600B5F0 RID: 46576 RVA: 0x00115425 File Offset: 0x00113625
		// (set) Token: 0x0600B5F1 RID: 46577 RVA: 0x0011542D File Offset: 0x0011362D
		public AttributeModifier StatBonus { get; private set; }

		// Token: 0x17000BB9 RID: 3001
		// (get) Token: 0x0600B5F2 RID: 46578 RVA: 0x00115436 File Offset: 0x00113636
		// (set) Token: 0x0600B5F3 RID: 46579 RVA: 0x0011543E File Offset: 0x0011363E
		public AttributeModifier FoodModifier { get; private set; }

		// Token: 0x17000BBA RID: 3002
		// (get) Token: 0x0600B5F4 RID: 46580 RVA: 0x00115447 File Offset: 0x00113647
		// (set) Token: 0x0600B5F5 RID: 46581 RVA: 0x0011544F File Offset: 0x0011364F
		public AttributeModifier CalorieModifier { get; private set; }

		// Token: 0x17000BBB RID: 3003
		// (get) Token: 0x0600B5F6 RID: 46582 RVA: 0x00115458 File Offset: 0x00113658
		// (set) Token: 0x0600B5F7 RID: 46583 RVA: 0x00115460 File Offset: 0x00113660
		public Color PrimaryColor { get; private set; }

		// Token: 0x17000BBC RID: 3004
		// (get) Token: 0x0600B5F8 RID: 46584 RVA: 0x00115469 File Offset: 0x00113669
		// (set) Token: 0x0600B5F9 RID: 46585 RVA: 0x00115471 File Offset: 0x00113671
		public Color SecondaryColor { get; private set; }

		// Token: 0x17000BBD RID: 3005
		// (get) Token: 0x0600B5FB RID: 46587 RVA: 0x00115483 File Offset: 0x00113683
		// (set) Token: 0x0600B5FA RID: 46586 RVA: 0x0011547A File Offset: 0x0011367A
		public string Image { get; private set; }

		// Token: 0x17000BBE RID: 3006
		// (get) Token: 0x0600B5FD RID: 46589 RVA: 0x00115494 File Offset: 0x00113694
		// (set) Token: 0x0600B5FC RID: 46588 RVA: 0x0011548B File Offset: 0x0011368B
		public string[] DlcIds { get; private set; } = DlcManager.AVAILABLE_ALL_VERSIONS;

		// Token: 0x0600B5FE RID: 46590 RVA: 0x004562B4 File Offset: 0x004544B4
		public Spice(ResourceSet parent, string id, Spice.Ingredient[] ingredients, Color primaryColor, Color secondaryColor, AttributeModifier foodMod = null, AttributeModifier statBonus = null, string imageName = "unknown", string[] dlcID = null) : base(id, parent, null)
		{
			if (dlcID != null)
			{
				this.DlcIds = dlcID;
			}
			this.StatBonus = statBonus;
			this.FoodModifier = foodMod;
			this.Ingredients = ingredients;
			this.Image = imageName;
			this.PrimaryColor = primaryColor;
			this.SecondaryColor = secondaryColor;
			for (int i = 0; i < this.Ingredients.Length; i++)
			{
				this.TotalKG += this.Ingredients[i].AmountKG;
			}
		}

		// Token: 0x04009452 RID: 37970
		public readonly Spice.Ingredient[] Ingredients;

		// Token: 0x04009453 RID: 37971
		public readonly float TotalKG;

		// Token: 0x02002168 RID: 8552
		public class Ingredient : IConfigurableConsumerIngredient
		{
			// Token: 0x0600B5FF RID: 46591 RVA: 0x0011549C File Offset: 0x0011369C
			public float GetAmount()
			{
				return this.AmountKG;
			}

			// Token: 0x0600B600 RID: 46592 RVA: 0x001154A4 File Offset: 0x001136A4
			public Tag[] GetIDSets()
			{
				return this.IngredientSet;
			}

			// Token: 0x04009456 RID: 37974
			public Tag[] IngredientSet;

			// Token: 0x04009457 RID: 37975
			public float AmountKG;
		}
	}
}
