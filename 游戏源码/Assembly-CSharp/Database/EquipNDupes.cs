using System;
using STRINGS;

namespace Database
{
	// Token: 0x02002195 RID: 8597
	public class EquipNDupes : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x0600B6B4 RID: 46772 RVA: 0x00115BF8 File Offset: 0x00113DF8
		public EquipNDupes(AssignableSlot equipmentSlot, int numToEquip)
		{
			this.equipmentSlot = equipmentSlot;
			this.numToEquip = numToEquip;
		}

		// Token: 0x0600B6B5 RID: 46773 RVA: 0x0045A470 File Offset: 0x00458670
		public override bool Success()
		{
			int num = 0;
			foreach (MinionIdentity minionIdentity in Components.MinionIdentities.Items)
			{
				Equipment equipment = minionIdentity.GetEquipment();
				if (equipment != null && equipment.IsSlotOccupied(this.equipmentSlot))
				{
					num++;
				}
			}
			return num >= this.numToEquip;
		}

		// Token: 0x0600B6B6 RID: 46774 RVA: 0x0045A4F0 File Offset: 0x004586F0
		public void Deserialize(IReader reader)
		{
			string id = reader.ReadKleiString();
			this.equipmentSlot = Db.Get().AssignableSlots.Get(id);
			this.numToEquip = reader.ReadInt32();
		}

		// Token: 0x0600B6B7 RID: 46775 RVA: 0x0045A528 File Offset: 0x00458728
		public override string GetProgress(bool complete)
		{
			int num = 0;
			foreach (MinionIdentity minionIdentity in Components.MinionIdentities.Items)
			{
				Equipment equipment = minionIdentity.GetEquipment();
				if (equipment != null && equipment.IsSlotOccupied(this.equipmentSlot))
				{
					num++;
				}
			}
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.CLOTHE_DUPES, complete ? this.numToEquip : num, this.numToEquip);
		}

		// Token: 0x04009501 RID: 38145
		private AssignableSlot equipmentSlot;

		// Token: 0x04009502 RID: 38146
		private int numToEquip;
	}
}
