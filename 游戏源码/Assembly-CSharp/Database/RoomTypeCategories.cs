using STRINGS;

namespace Database;

public class RoomTypeCategories : ResourceSet<RoomTypeCategory>
{
	public RoomTypeCategory None;

	public RoomTypeCategory Food;

	public RoomTypeCategory Sleep;

	public RoomTypeCategory Recreation;

	public RoomTypeCategory Bathroom;

	public RoomTypeCategory Hospital;

	public RoomTypeCategory Industrial;

	public RoomTypeCategory Agricultural;

	public RoomTypeCategory Park;

	public RoomTypeCategory Science;

	private RoomTypeCategory Add(string id, string name, string colorName, string icon)
	{
		RoomTypeCategory roomTypeCategory = new RoomTypeCategory(id, name, colorName, icon);
		Add(roomTypeCategory);
		return roomTypeCategory;
	}

	public RoomTypeCategories(ResourceSet parent)
		: base("RoomTypeCategories", parent)
	{
		Initialize();
		None = Add("None", ROOMS.CATEGORY.NONE.NAME, "roomNone", "unknown");
		Food = Add("Food", ROOMS.CATEGORY.FOOD.NAME, "roomFood", "ui_room_food");
		Sleep = Add("Sleep", ROOMS.CATEGORY.SLEEP.NAME, "roomSleep", "ui_room_sleep");
		Recreation = Add("Recreation", ROOMS.CATEGORY.RECREATION.NAME, "roomRecreation", "ui_room_recreational");
		Bathroom = Add("Bathroom", ROOMS.CATEGORY.BATHROOM.NAME, "roomBathroom", "ui_room_bathroom");
		Hospital = Add("Hospital", ROOMS.CATEGORY.HOSPITAL.NAME, "roomHospital", "ui_room_hospital");
		Industrial = Add("Industrial", ROOMS.CATEGORY.INDUSTRIAL.NAME, "roomIndustrial", "ui_room_industrial");
		Agricultural = Add("Agricultural", ROOMS.CATEGORY.AGRICULTURAL.NAME, "roomAgricultural", "ui_room_agricultural");
		Park = Add("Park", ROOMS.CATEGORY.PARK.NAME, "roomPark", "ui_room_park");
		Science = Add("Science", ROOMS.CATEGORY.SCIENCE.NAME, "roomScience", "ui_room_science");
	}
}
