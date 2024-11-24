using System;
using STRINGS;

namespace Database
{
	// Token: 0x0200215E RID: 8542
	public class RoomTypeCategories : ResourceSet<RoomTypeCategory>
	{
		// Token: 0x0600B5D8 RID: 46552 RVA: 0x00453AD4 File Offset: 0x00451CD4
		private RoomTypeCategory Add(string id, string name, string colorName, string icon)
		{
			RoomTypeCategory roomTypeCategory = new RoomTypeCategory(id, name, colorName, icon);
			base.Add(roomTypeCategory);
			return roomTypeCategory;
		}

		// Token: 0x0600B5D9 RID: 46553 RVA: 0x00453AF8 File Offset: 0x00451CF8
		public RoomTypeCategories(ResourceSet parent) : base("RoomTypeCategories", parent)
		{
			base.Initialize();
			this.None = this.Add("None", ROOMS.CATEGORY.NONE.NAME, "roomNone", "unknown");
			this.Food = this.Add("Food", ROOMS.CATEGORY.FOOD.NAME, "roomFood", "ui_room_food");
			this.Sleep = this.Add("Sleep", ROOMS.CATEGORY.SLEEP.NAME, "roomSleep", "ui_room_sleep");
			this.Recreation = this.Add("Recreation", ROOMS.CATEGORY.RECREATION.NAME, "roomRecreation", "ui_room_recreational");
			if (DlcManager.IsContentSubscribed("DLC3_ID"))
			{
				this.Bionic = this.Add("Bionic", ROOMS.CATEGORY.BIONIC.NAME, "roomBionic", "ui_room_bionicupkeep");
			}
			this.Bathroom = this.Add("Bathroom", ROOMS.CATEGORY.BATHROOM.NAME, "roomBathroom", "ui_room_bathroom");
			this.Hospital = this.Add("Hospital", ROOMS.CATEGORY.HOSPITAL.NAME, "roomHospital", "ui_room_hospital");
			this.Industrial = this.Add("Industrial", ROOMS.CATEGORY.INDUSTRIAL.NAME, "roomIndustrial", "ui_room_industrial");
			this.Agricultural = this.Add("Agricultural", ROOMS.CATEGORY.AGRICULTURAL.NAME, "roomAgricultural", "ui_room_agricultural");
			this.Park = this.Add("Park", ROOMS.CATEGORY.PARK.NAME, "roomPark", "ui_room_park");
			this.Science = this.Add("Science", ROOMS.CATEGORY.SCIENCE.NAME, "roomScience", "ui_room_science");
		}

		// Token: 0x040093F5 RID: 37877
		public RoomTypeCategory None;

		// Token: 0x040093F6 RID: 37878
		public RoomTypeCategory Food;

		// Token: 0x040093F7 RID: 37879
		public RoomTypeCategory Sleep;

		// Token: 0x040093F8 RID: 37880
		public RoomTypeCategory Recreation;

		// Token: 0x040093F9 RID: 37881
		public RoomTypeCategory Bathroom;

		// Token: 0x040093FA RID: 37882
		public RoomTypeCategory Bionic;

		// Token: 0x040093FB RID: 37883
		public RoomTypeCategory Hospital;

		// Token: 0x040093FC RID: 37884
		public RoomTypeCategory Industrial;

		// Token: 0x040093FD RID: 37885
		public RoomTypeCategory Agricultural;

		// Token: 0x040093FE RID: 37886
		public RoomTypeCategory Park;

		// Token: 0x040093FF RID: 37887
		public RoomTypeCategory Science;
	}
}
