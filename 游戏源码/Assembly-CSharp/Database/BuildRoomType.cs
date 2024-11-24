using System;
using System.Collections.Generic;
using STRINGS;

namespace Database
{
	// Token: 0x02002190 RID: 8592
	public class BuildRoomType : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x0600B69F RID: 46751 RVA: 0x00115B24 File Offset: 0x00113D24
		public BuildRoomType(RoomType roomType)
		{
			this.roomType = roomType;
		}

		// Token: 0x0600B6A0 RID: 46752 RVA: 0x0045A04C File Offset: 0x0045824C
		public override bool Success()
		{
			using (List<Room>.Enumerator enumerator = Game.Instance.roomProber.rooms.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.roomType == this.roomType)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600B6A1 RID: 46753 RVA: 0x0045A0B4 File Offset: 0x004582B4
		public void Deserialize(IReader reader)
		{
			string id = reader.ReadKleiString();
			this.roomType = Db.Get().RoomTypes.Get(id);
		}

		// Token: 0x0600B6A2 RID: 46754 RVA: 0x00115B33 File Offset: 0x00113D33
		public override string GetProgress(bool complete)
		{
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.BUILT_A_ROOM, this.roomType.Name);
		}

		// Token: 0x040094FC RID: 38140
		private RoomType roomType;
	}
}
