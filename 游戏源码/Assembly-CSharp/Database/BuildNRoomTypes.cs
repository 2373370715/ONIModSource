using System;
using System.Collections.Generic;
using STRINGS;

namespace Database
{
	// Token: 0x02002191 RID: 8593
	public class BuildNRoomTypes : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x0600B6A3 RID: 46755 RVA: 0x00115B4F File Offset: 0x00113D4F
		public BuildNRoomTypes(RoomType roomType, int numToCreate = 1)
		{
			this.roomType = roomType;
			this.numToCreate = numToCreate;
		}

		// Token: 0x0600B6A4 RID: 46756 RVA: 0x0045A0E0 File Offset: 0x004582E0
		public override bool Success()
		{
			int num = 0;
			using (List<Room>.Enumerator enumerator = Game.Instance.roomProber.rooms.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.roomType == this.roomType)
					{
						num++;
					}
				}
			}
			return num >= this.numToCreate;
		}

		// Token: 0x0600B6A5 RID: 46757 RVA: 0x0045A154 File Offset: 0x00458354
		public void Deserialize(IReader reader)
		{
			string id = reader.ReadKleiString();
			this.roomType = Db.Get().RoomTypes.Get(id);
			this.numToCreate = reader.ReadInt32();
		}

		// Token: 0x0600B6A6 RID: 46758 RVA: 0x0045A18C File Offset: 0x0045838C
		public override string GetProgress(bool complete)
		{
			int num = 0;
			using (List<Room>.Enumerator enumerator = Game.Instance.roomProber.rooms.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.roomType == this.roomType)
					{
						num++;
					}
				}
			}
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.BUILT_N_ROOMS, this.roomType.Name, complete ? this.numToCreate : num, this.numToCreate);
		}

		// Token: 0x040094FD RID: 38141
		private RoomType roomType;

		// Token: 0x040094FE RID: 38142
		private int numToCreate;
	}
}
