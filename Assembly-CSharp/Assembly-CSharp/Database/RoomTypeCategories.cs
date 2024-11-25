using STRINGS;

namespace Database {
    public class RoomTypeCategories : ResourceSet<RoomTypeCategory> {
        public RoomTypeCategory Agricultural;
        public RoomTypeCategory Bathroom;
        public RoomTypeCategory Bionic;
        public RoomTypeCategory Food;
        public RoomTypeCategory Hospital;
        public RoomTypeCategory Industrial;
        public RoomTypeCategory None;
        public RoomTypeCategory Park;
        public RoomTypeCategory Recreation;
        public RoomTypeCategory Science;
        public RoomTypeCategory Sleep;

        public RoomTypeCategories(ResourceSet parent) : base("RoomTypeCategories", parent) {
            base.Initialize();
            None       = Add("None",       ROOMS.CATEGORY.NONE.NAME,       "roomNone",       "unknown");
            Food       = Add("Food",       ROOMS.CATEGORY.FOOD.NAME,       "roomFood",       "ui_room_food");
            Sleep      = Add("Sleep",      ROOMS.CATEGORY.SLEEP.NAME,      "roomSleep",      "ui_room_sleep");
            Recreation = Add("Recreation", ROOMS.CATEGORY.RECREATION.NAME, "roomRecreation", "ui_room_recreational");
            if (DlcManager.IsContentSubscribed("DLC3_ID"))
                Bionic = Add("Bionic", ROOMS.CATEGORY.BIONIC.NAME, "roomBionic", "ui_room_bionicupkeep");

            Bathroom   = Add("Bathroom",   ROOMS.CATEGORY.BATHROOM.NAME,   "roomBathroom",   "ui_room_bathroom");
            Hospital   = Add("Hospital",   ROOMS.CATEGORY.HOSPITAL.NAME,   "roomHospital",   "ui_room_hospital");
            Industrial = Add("Industrial", ROOMS.CATEGORY.INDUSTRIAL.NAME, "roomIndustrial", "ui_room_industrial");
            Agricultural = Add("Agricultural",
                               ROOMS.CATEGORY.AGRICULTURAL.NAME,
                               "roomAgricultural",
                               "ui_room_agricultural");

            Park    = Add("Park",    ROOMS.CATEGORY.PARK.NAME,    "roomPark",    "ui_room_park");
            Science = Add("Science", ROOMS.CATEGORY.SCIENCE.NAME, "roomScience", "ui_room_science");
        }

        private RoomTypeCategory Add(string id, string name, string colorName, string icon) {
            var roomTypeCategory = new RoomTypeCategory(id, name, colorName, icon);
            base.Add(roomTypeCategory);
            return roomTypeCategory;
        }
    }
}