using System;
using System.Collections.Generic;

namespace Database
{
	// Token: 0x02002197 RID: 8599
	public class ProduceXEngeryWithoutUsingYList : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x0600B6BC RID: 46780 RVA: 0x00115C0E File Offset: 0x00113E0E
		public ProduceXEngeryWithoutUsingYList(float amountToProduce, List<Tag> disallowedBuildings)
		{
			this.disallowedBuildings = disallowedBuildings;
			this.amountToProduce = amountToProduce;
			this.usedDisallowedBuilding = false;
		}

		// Token: 0x0600B6BD RID: 46781 RVA: 0x0045A818 File Offset: 0x00458A18
		public override bool Success()
		{
			float num = 0f;
			foreach (KeyValuePair<Tag, float> keyValuePair in Game.Instance.savedInfo.powerCreatedbyGeneratorType)
			{
				if (!this.disallowedBuildings.Contains(keyValuePair.Key))
				{
					num += keyValuePair.Value;
				}
			}
			return num / 1000f > this.amountToProduce;
		}

		// Token: 0x0600B6BE RID: 46782 RVA: 0x0045A8A0 File Offset: 0x00458AA0
		public override bool Fail()
		{
			foreach (Tag key in this.disallowedBuildings)
			{
				if (Game.Instance.savedInfo.powerCreatedbyGeneratorType.ContainsKey(key))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600B6BF RID: 46783 RVA: 0x0045A90C File Offset: 0x00458B0C
		public void Deserialize(IReader reader)
		{
			int num = reader.ReadInt32();
			this.disallowedBuildings = new List<Tag>(num);
			for (int i = 0; i < num; i++)
			{
				string name = reader.ReadKleiString();
				this.disallowedBuildings.Add(new Tag(name));
			}
			this.amountProduced = (float)reader.ReadDouble();
			this.amountToProduce = (float)reader.ReadDouble();
			this.usedDisallowedBuilding = (reader.ReadByte() > 0);
		}

		// Token: 0x0600B6C0 RID: 46784 RVA: 0x0045A97C File Offset: 0x00458B7C
		public float GetProductionAmount(bool complete)
		{
			if (complete)
			{
				return this.amountToProduce * 1000f;
			}
			float num = 0f;
			foreach (KeyValuePair<Tag, float> keyValuePair in Game.Instance.savedInfo.powerCreatedbyGeneratorType)
			{
				if (!this.disallowedBuildings.Contains(keyValuePair.Key))
				{
					num += keyValuePair.Value;
				}
			}
			return num;
		}

		// Token: 0x04009507 RID: 38151
		public List<Tag> disallowedBuildings = new List<Tag>();

		// Token: 0x04009508 RID: 38152
		public float amountToProduce;

		// Token: 0x04009509 RID: 38153
		private float amountProduced;

		// Token: 0x0400950A RID: 38154
		private bool usedDisallowedBuilding;
	}
}
