using System;
using System.Collections.Generic;
using Database;
using STRINGS;
using UnityEngine;

public static class RoomConstraints
{
	public static class ConstraintTags
	{
		public static List<Tag> AllTags = new List<Tag>();

		public static Tag BedType = AllTags.AddAndReturn("BedType".ToTag());

		public static Tag LuxuryBedType = AllTags.AddAndReturn("LuxuryBedType".ToTag());

		public static Tag ToiletType = AllTags.AddAndReturn("ToiletType".ToTag());

		public static Tag FlushToiletType = AllTags.AddAndReturn("FlushToiletType".ToTag());

		public static Tag MessTable = AllTags.AddAndReturn("MessTable".ToTag());

		public static Tag Clinic = AllTags.AddAndReturn("Clinic".ToTag());

		public static Tag WashStation = AllTags.AddAndReturn("WashStation".ToTag());

		public static Tag AdvancedWashStation = AllTags.AddAndReturn("AdvancedWashStation".ToTag());

		public static Tag ScienceBuilding = AllTags.AddAndReturn("ScienceBuilding".ToTag());

		public static Tag LightSource = AllTags.AddAndReturn("LightSource".ToTag());

		public static Tag MassageTable = AllTags.AddAndReturn("MassageTable".ToTag());

		public static Tag DeStressingBuilding = AllTags.AddAndReturn("DeStressingBuilding".ToTag());

		public static Tag IndustrialMachinery = AllTags.AddAndReturn("IndustrialMachinery".ToTag());

		public static Tag PowerStation = AllTags.AddAndReturn("PowerStation".ToTag());

		public static Tag FarmStationType = AllTags.AddAndReturn("FarmStationType".ToTag());

		public static Tag CreatureRelocator = AllTags.AddAndReturn("CreatureRelocator".ToTag());

		public static Tag RanchStationType = AllTags.AddAndReturn("RanchStationType".ToTag());

		public static Tag SpiceStation = AllTags.AddAndReturn("SpiceStation".ToTag());

		public static Tag CookTop = AllTags.AddAndReturn("CookTop".ToTag());

		public static Tag Refrigerator = AllTags.AddAndReturn("Refrigerator".ToTag());

		public static Tag RecBuilding = AllTags.AddAndReturn("RecBuilding".ToTag());

		public static Tag MachineShopType = AllTags.AddAndReturn("MachineShopType".ToTag());

		public static Tag Park = AllTags.AddAndReturn("Park".ToTag());

		public static Tag NatureReserve = AllTags.AddAndReturn("NatureReserve".ToTag());

		public static Tag Decor20 = AllTags.AddAndReturn("Decor20".ToTag());

		public static Tag RocketInterior = AllTags.AddAndReturn("RocketInterior".ToTag());

		public static Tag Decoration = AllTags.AddAndReturn(GameTags.Decoration);

		public static Tag WarmingStation = AllTags.AddAndReturn("WarmingStation".ToTag());

		public static string GetRoomConstraintLabelText(Tag tag)
		{
			StringEntry result = null;
			string text = "STRINGS.ROOMS.CRITERIA." + tag.ToString().ToUpper() + ".NAME";
			if (!Strings.TryGet(new StringKey(text), out result))
			{
				return ROOMS.CRITERIA.IN_CODE_ERROR.text.Replace("{0}", text);
			}
			return result;
		}
	}

	public class Constraint
	{
		public string name;

		public string description;

		public int times_required = 1;

		public Func<Room, bool> room_criteria;

		public Func<KPrefabID, bool> building_criteria;

		public List<Constraint> stomp_in_conflict;

		public Constraint(Func<KPrefabID, bool> building_criteria, Func<Room, bool> room_criteria, int times_required = 1, string name = "", string description = "", List<Constraint> stomp_in_conflict = null)
		{
			this.room_criteria = room_criteria;
			this.building_criteria = building_criteria;
			this.times_required = times_required;
			this.name = name;
			this.description = description;
			this.stomp_in_conflict = stomp_in_conflict;
		}

		public bool isSatisfied(Room room)
		{
			int num = 0;
			if (room_criteria != null && !room_criteria(room))
			{
				return false;
			}
			if (building_criteria != null)
			{
				int num2 = 0;
				while (num < times_required && num2 < room.buildings.Count)
				{
					KPrefabID kPrefabID = room.buildings[num2];
					if (!(kPrefabID == null) && building_criteria(kPrefabID))
					{
						num++;
					}
					num2++;
				}
				int num3 = 0;
				while (num < times_required && num3 < room.plants.Count)
				{
					KPrefabID kPrefabID2 = room.plants[num3];
					if (!(kPrefabID2 == null) && building_criteria(kPrefabID2))
					{
						num++;
					}
					num3++;
				}
				return num >= times_required;
			}
			return true;
		}
	}

	public static Constraint CEILING_HEIGHT_4 = new Constraint(null, (Room room) => 1 + room.cavity.maxY - room.cavity.minY >= 4, 1, string.Format(ROOMS.CRITERIA.CEILING_HEIGHT.NAME, "4"), string.Format(ROOMS.CRITERIA.CEILING_HEIGHT.DESCRIPTION, "4"));

	public static Constraint CEILING_HEIGHT_6 = new Constraint(null, (Room room) => 1 + room.cavity.maxY - room.cavity.minY >= 6, 1, string.Format(ROOMS.CRITERIA.CEILING_HEIGHT.NAME, "6"), string.Format(ROOMS.CRITERIA.CEILING_HEIGHT.DESCRIPTION, "6"));

	public static Constraint MINIMUM_SIZE_12 = new Constraint(null, (Room room) => room.cavity.numCells >= 12, 1, string.Format(ROOMS.CRITERIA.MINIMUM_SIZE.NAME, "12"), string.Format(ROOMS.CRITERIA.MINIMUM_SIZE.DESCRIPTION, "12"));

	public static Constraint MINIMUM_SIZE_24 = new Constraint(null, (Room room) => room.cavity.numCells >= 24, 1, string.Format(ROOMS.CRITERIA.MINIMUM_SIZE.NAME, "24"), string.Format(ROOMS.CRITERIA.MINIMUM_SIZE.DESCRIPTION, "24"));

	public static Constraint MINIMUM_SIZE_32 = new Constraint(null, (Room room) => room.cavity.numCells >= 32, 1, string.Format(ROOMS.CRITERIA.MINIMUM_SIZE.NAME, "32"), string.Format(ROOMS.CRITERIA.MINIMUM_SIZE.DESCRIPTION, "32"));

	public static Constraint MAXIMUM_SIZE_64 = new Constraint(null, (Room room) => room.cavity.numCells <= 64, 1, string.Format(ROOMS.CRITERIA.MAXIMUM_SIZE.NAME, "64"), string.Format(ROOMS.CRITERIA.MAXIMUM_SIZE.DESCRIPTION, "64"));

	public static Constraint MAXIMUM_SIZE_96 = new Constraint(null, (Room room) => room.cavity.numCells <= 96, 1, string.Format(ROOMS.CRITERIA.MAXIMUM_SIZE.NAME, "96"), string.Format(ROOMS.CRITERIA.MAXIMUM_SIZE.DESCRIPTION, "96"));

	public static Constraint MAXIMUM_SIZE_120 = new Constraint(null, (Room room) => room.cavity.numCells <= 120, 1, string.Format(ROOMS.CRITERIA.MAXIMUM_SIZE.NAME, "120"), string.Format(ROOMS.CRITERIA.MAXIMUM_SIZE.DESCRIPTION, "120"));

	public static Constraint NO_INDUSTRIAL_MACHINERY = new Constraint(null, delegate(Room room)
	{
		foreach (KPrefabID building in room.buildings)
		{
			if (building.HasTag(ConstraintTags.IndustrialMachinery))
			{
				return false;
			}
		}
		return true;
	}, 1, ROOMS.CRITERIA.NO_INDUSTRIAL_MACHINERY.NAME, ROOMS.CRITERIA.NO_INDUSTRIAL_MACHINERY.DESCRIPTION);

	public static Constraint NO_COTS = new Constraint(null, delegate(Room room)
	{
		foreach (KPrefabID building2 in room.buildings)
		{
			if (building2.HasTag(ConstraintTags.BedType) && !building2.HasTag(ConstraintTags.LuxuryBedType))
			{
				return false;
			}
		}
		return true;
	}, 1, ROOMS.CRITERIA.NO_COTS.NAME, ROOMS.CRITERIA.NO_COTS.DESCRIPTION);

	public static Constraint NO_LUXURY_BEDS = new Constraint(null, delegate(Room room)
	{
		foreach (KPrefabID building3 in room.buildings)
		{
			if (building3.HasTag(ConstraintTags.LuxuryBedType))
			{
				return false;
			}
		}
		return true;
	}, 1, ROOMS.CRITERIA.NO_COTS.NAME, ROOMS.CRITERIA.NO_COTS.DESCRIPTION);

	public static Constraint NO_OUTHOUSES = new Constraint(null, delegate(Room room)
	{
		foreach (KPrefabID building4 in room.buildings)
		{
			if (building4.HasTag(ConstraintTags.ToiletType) && !building4.HasTag(ConstraintTags.FlushToiletType))
			{
				return false;
			}
		}
		return true;
	}, 1, ROOMS.CRITERIA.NO_OUTHOUSES.NAME, ROOMS.CRITERIA.NO_OUTHOUSES.DESCRIPTION);

	public static Constraint NO_MESS_STATION = new Constraint(null, delegate(Room room)
	{
		bool flag2 = false;
		int num11 = 0;
		while (!flag2 && num11 < room.buildings.Count)
		{
			flag2 = room.buildings[num11].HasTag(ConstraintTags.MessTable);
			num11++;
		}
		return !flag2;
	}, 1, ROOMS.CRITERIA.NO_MESS_STATION.NAME, ROOMS.CRITERIA.NO_MESS_STATION.DESCRIPTION);

	public static Constraint HAS_LUXURY_BED = new Constraint((KPrefabID bc) => bc.HasTag(ConstraintTags.LuxuryBedType), null, 1, ROOMS.CRITERIA.HAS_LUXURY_BED.NAME, ROOMS.CRITERIA.HAS_LUXURY_BED.DESCRIPTION);

	public static Constraint HAS_BED = new Constraint((KPrefabID bc) => bc.HasTag(ConstraintTags.BedType) && !bc.HasTag(ConstraintTags.Clinic), null, 1, ROOMS.CRITERIA.HAS_BED.NAME, ROOMS.CRITERIA.HAS_BED.DESCRIPTION);

	public static Constraint SCIENCE_BUILDINGS = new Constraint((KPrefabID bc) => bc.HasTag(ConstraintTags.ScienceBuilding), null, 2, ROOMS.CRITERIA.SCIENCE_BUILDINGS.NAME, ROOMS.CRITERIA.SCIENCE_BUILDINGS.DESCRIPTION);

	public static Constraint BED_SINGLE = new Constraint((KPrefabID bc) => bc.HasTag(ConstraintTags.BedType) && !bc.HasTag(ConstraintTags.Clinic), delegate(Room room)
	{
		short num9 = 0;
		int num10 = 0;
		while (num9 < 2 && num10 < room.buildings.Count)
		{
			if (room.buildings[num10].HasTag(ConstraintTags.BedType))
			{
				num9++;
			}
			num10++;
		}
		return num9 == 1;
	}, 1, ROOMS.CRITERIA.BED_SINGLE.NAME, ROOMS.CRITERIA.BED_SINGLE.DESCRIPTION);

	public static Constraint LUXURY_BED_SINGLE = new Constraint((KPrefabID bc) => bc.HasTag(ConstraintTags.LuxuryBedType), delegate(Room room)
	{
		short num7 = 0;
		int num8 = 0;
		while (num7 <= 2 && num8 < room.buildings.Count)
		{
			if (room.buildings[num8].HasTag(ConstraintTags.LuxuryBedType))
			{
				num7++;
			}
			num8++;
		}
		return num7 == 1;
	}, 1, ROOMS.CRITERIA.LUXURYBEDTYPE.NAME, ROOMS.CRITERIA.LUXURYBEDTYPE.DESCRIPTION);

	public static Constraint BUILDING_DECOR_POSITIVE = new Constraint(delegate(KPrefabID bc)
	{
		DecorProvider component7 = bc.GetComponent<DecorProvider>();
		return (component7 != null && component7.baseDecor > 0f) ? true : false;
	}, null, 1, ROOMS.CRITERIA.BUILDING_DECOR_POSITIVE.NAME, ROOMS.CRITERIA.BUILDING_DECOR_POSITIVE.DESCRIPTION);

	public static Constraint DECORATIVE_ITEM = new Constraint((KPrefabID bc) => bc.HasTag(GameTags.Decoration), null, 1, string.Format(ROOMS.CRITERIA.DECORATIVE_ITEM.NAME, 1), string.Format(ROOMS.CRITERIA.DECORATIVE_ITEM.DESCRIPTION, 1));

	public static Constraint DECORATIVE_ITEM_2 = new Constraint((KPrefabID bc) => bc.HasTag(GameTags.Decoration), null, 2, string.Format(ROOMS.CRITERIA.DECORATIVE_ITEM.NAME, 2), string.Format(ROOMS.CRITERIA.DECORATIVE_ITEM.DESCRIPTION, 2));

	public static Constraint DECORATIVE_ITEM_SCORE_20 = new Constraint((KPrefabID bc) => bc.HasTag(GameTags.Decoration) && bc.HasTag(ConstraintTags.Decor20), null, 1, ROOMS.CRITERIA.DECOR20.NAME, ROOMS.CRITERIA.DECOR20.DESCRIPTION);

	public static Constraint POWER_STATION = new Constraint((KPrefabID bc) => bc.HasTag(ConstraintTags.PowerStation), null, 1, ROOMS.CRITERIA.POWERSTATION.NAME, ROOMS.CRITERIA.POWERSTATION.DESCRIPTION);

	public static Constraint FARM_STATION = new Constraint((KPrefabID bc) => bc.HasTag(ConstraintTags.FarmStationType), null, 1, ROOMS.CRITERIA.FARMSTATIONTYPE.NAME, ROOMS.CRITERIA.FARMSTATIONTYPE.DESCRIPTION);

	public static Constraint RANCH_STATION = new Constraint((KPrefabID bc) => bc.HasTag(ConstraintTags.RanchStationType), null, 1, ROOMS.CRITERIA.RANCHSTATIONTYPE.NAME, ROOMS.CRITERIA.RANCHSTATIONTYPE.DESCRIPTION);

	public static Constraint SPICE_STATION = new Constraint((KPrefabID bc) => bc.HasTag(ConstraintTags.SpiceStation), null, 1, ROOMS.CRITERIA.SPICESTATION.NAME, ROOMS.CRITERIA.SPICESTATION.DESCRIPTION);

	public static Constraint COOK_TOP = new Constraint((KPrefabID bc) => bc.HasTag(ConstraintTags.CookTop), null, 1, ROOMS.CRITERIA.COOKTOP.NAME, ROOMS.CRITERIA.COOKTOP.DESCRIPTION);

	public static Constraint REFRIGERATOR = new Constraint((KPrefabID bc) => bc.HasTag(ConstraintTags.Refrigerator), null, 1, ROOMS.CRITERIA.REFRIGERATOR.NAME, ROOMS.CRITERIA.REFRIGERATOR.DESCRIPTION);

	public static Constraint REC_BUILDING = new Constraint((KPrefabID bc) => bc.HasTag(ConstraintTags.RecBuilding), null, 1, ROOMS.CRITERIA.RECBUILDING.NAME, ROOMS.CRITERIA.RECBUILDING.DESCRIPTION);

	public static Constraint MACHINE_SHOP = new Constraint((KPrefabID bc) => bc.HasTag(ConstraintTags.MachineShopType), null, 1, ROOMS.CRITERIA.MACHINESHOPTYPE.NAME, ROOMS.CRITERIA.MACHINESHOPTYPE.DESCRIPTION);

	public static Constraint LIGHT = new Constraint(null, delegate(Room room)
	{
		foreach (KPrefabID creature in room.cavity.creatures)
		{
			if (creature != null && creature.GetComponent<Light2D>() != null)
			{
				return true;
			}
		}
		foreach (KPrefabID building5 in room.buildings)
		{
			if (!(building5 == null))
			{
				Light2D component5 = building5.GetComponent<Light2D>();
				if (component5 != null)
				{
					RequireInputs component6 = building5.GetComponent<RequireInputs>();
					if (component5.enabled || (component6 != null && component6.RequirementsMet))
					{
						return true;
					}
				}
			}
		}
		return false;
	}, 1, ROOMS.CRITERIA.LIGHTSOURCE.NAME, ROOMS.CRITERIA.LIGHTSOURCE.DESCRIPTION);

	public static Constraint DESTRESSING_BUILDING = new Constraint((KPrefabID bc) => bc.HasTag(ConstraintTags.DeStressingBuilding), null, 1, ROOMS.CRITERIA.DESTRESSINGBUILDING.NAME, ROOMS.CRITERIA.DESTRESSINGBUILDING.DESCRIPTION);

	public static Constraint MASSAGE_TABLE = new Constraint((KPrefabID bc) => bc.IsPrefabID(ConstraintTags.MassageTable), null, 1, ROOMS.CRITERIA.MASSAGE_TABLE.NAME, ROOMS.CRITERIA.MASSAGE_TABLE.DESCRIPTION);

	public static Constraint MESS_STATION_SINGLE = new Constraint((KPrefabID bc) => bc.HasTag(ConstraintTags.MessTable), null, 1, ROOMS.CRITERIA.MESSTABLE.NAME, ROOMS.CRITERIA.MESSTABLE.DESCRIPTION, new List<Constraint> { REC_BUILDING });

	public static Constraint TOILET = new Constraint((KPrefabID bc) => bc.HasTag(ConstraintTags.ToiletType), null, 1, ROOMS.CRITERIA.TOILETTYPE.NAME, ROOMS.CRITERIA.TOILETTYPE.DESCRIPTION);

	public static Constraint FLUSH_TOILET = new Constraint((KPrefabID bc) => bc.HasTag(ConstraintTags.FlushToiletType), null, 1, ROOMS.CRITERIA.FLUSHTOILETTYPE.NAME, ROOMS.CRITERIA.FLUSHTOILETTYPE.DESCRIPTION);

	public static Constraint WASH_STATION = new Constraint((KPrefabID bc) => bc.HasTag(ConstraintTags.WashStation), null, 1, ROOMS.CRITERIA.WASHSTATION.NAME, ROOMS.CRITERIA.WASHSTATION.DESCRIPTION);

	public static Constraint ADVANCEDWASHSTATION = new Constraint((KPrefabID bc) => bc.HasTag(ConstraintTags.AdvancedWashStation), null, 1, ROOMS.CRITERIA.ADVANCEDWASHSTATION.NAME, ROOMS.CRITERIA.ADVANCEDWASHSTATION.DESCRIPTION);

	public static Constraint CLINIC = new Constraint((KPrefabID bc) => bc.HasTag(ConstraintTags.Clinic), null, 1, ROOMS.CRITERIA.CLINIC.NAME, ROOMS.CRITERIA.CLINIC.DESCRIPTION, new List<Constraint> { TOILET, FLUSH_TOILET, MESS_STATION_SINGLE });

	public static Constraint PARK_BUILDING = new Constraint((KPrefabID bc) => bc.HasTag(ConstraintTags.Park), null, 1, ROOMS.CRITERIA.PARK.NAME, ROOMS.CRITERIA.PARK.DESCRIPTION);

	public static Constraint ORIGINALTILES = new Constraint(null, (Room room) => 1 + room.cavity.maxY - room.cavity.minY >= 4);

	public static Constraint IS_BACKWALLED = new Constraint(null, delegate(Room room)
	{
		bool flag = true;
		int num4 = (room.cavity.maxX - room.cavity.minX + 1) / 2 + 1;
		int num5 = 0;
		while (flag && num5 < num4)
		{
			int x = room.cavity.minX + num5;
			int x2 = room.cavity.maxX - num5;
			int num6 = room.cavity.minY;
			while (flag && num6 <= room.cavity.maxY)
			{
				int cell = Grid.XYToCell(x, num6);
				int cell2 = Grid.XYToCell(x2, num6);
				if (Game.Instance.roomProber.GetCavityForCell(cell) == room.cavity)
				{
					GameObject gameObject = Grid.Objects[cell, 2];
					flag &= gameObject != null && !gameObject.HasTag(GameTags.UnderConstruction);
				}
				if (Game.Instance.roomProber.GetCavityForCell(cell2) == room.cavity)
				{
					GameObject gameObject2 = Grid.Objects[cell2, 2];
					flag &= gameObject2 != null && !gameObject2.HasTag(GameTags.UnderConstruction);
				}
				if (!flag)
				{
					return false;
				}
				num6++;
			}
			num5++;
		}
		return flag;
	}, 1, ROOMS.CRITERIA.IS_BACKWALLED.NAME, ROOMS.CRITERIA.IS_BACKWALLED.DESCRIPTION);

	public static Constraint WILDANIMAL = new Constraint(null, (Room room) => room.cavity.creatures.Count + room.cavity.eggs.Count > 0, 1, ROOMS.CRITERIA.WILDANIMAL.NAME, ROOMS.CRITERIA.WILDANIMAL.DESCRIPTION);

	public static Constraint WILDANIMALS = new Constraint(null, delegate(Room room)
	{
		int num3 = 0;
		foreach (KPrefabID creature2 in room.cavity.creatures)
		{
			if (creature2.HasTag(GameTags.Creatures.Wild))
			{
				num3++;
			}
		}
		return num3 >= 2;
	}, 1, ROOMS.CRITERIA.WILDANIMALS.NAME, ROOMS.CRITERIA.WILDANIMALS.DESCRIPTION);

	public static Constraint WILDPLANT = new Constraint(null, delegate(Room room)
	{
		int num2 = 0;
		foreach (KPrefabID plant in room.cavity.plants)
		{
			if (plant != null)
			{
				BasicForagePlantPlanted component3 = plant.GetComponent<BasicForagePlantPlanted>();
				ReceptacleMonitor component4 = plant.GetComponent<ReceptacleMonitor>();
				if (component4 != null && !component4.Replanted)
				{
					num2++;
				}
				else if (component3 != null)
				{
					num2++;
				}
			}
		}
		return num2 >= 2;
	}, 1, ROOMS.CRITERIA.WILDPLANT.NAME, ROOMS.CRITERIA.WILDPLANT.DESCRIPTION);

	public static Constraint WILDPLANTS = new Constraint(null, delegate(Room room)
	{
		int num = 0;
		foreach (KPrefabID plant2 in room.cavity.plants)
		{
			if (plant2 != null)
			{
				BasicForagePlantPlanted component = plant2.GetComponent<BasicForagePlantPlanted>();
				ReceptacleMonitor component2 = plant2.GetComponent<ReceptacleMonitor>();
				if (component2 != null && !component2.Replanted)
				{
					num++;
				}
				else if (component != null)
				{
					num++;
				}
			}
		}
		return num >= 4;
	}, 1, ROOMS.CRITERIA.WILDPLANTS.NAME, ROOMS.CRITERIA.WILDPLANTS.DESCRIPTION);

	public static Tag AddAndReturn(this List<Tag> list, Tag tag)
	{
		list.Add(tag);
		return tag;
	}

	public static string RoomCriteriaString(Room room)
	{
		string text = "";
		RoomType roomType = room.roomType;
		if (roomType != Db.Get().RoomTypes.Neutral)
		{
			text = string.Concat(text, "<b>", ROOMS.CRITERIA.HEADER, "</b>");
			text = text + "\n    • " + roomType.primary_constraint.name;
			if (roomType.additional_constraints != null)
			{
				Constraint[] additional_constraints = roomType.additional_constraints;
				foreach (Constraint constraint in additional_constraints)
				{
					text = ((!constraint.isSatisfied(room)) ? (text + "\n<color=#F44A47FF>    • " + constraint.name + "</color>") : (text + "\n    • " + constraint.name));
				}
			}
			return text;
		}
		RoomTypes.RoomTypeQueryResult[] possibleRoomTypes = Db.Get().RoomTypes.GetPossibleRoomTypes(room);
		text += ((possibleRoomTypes.Length > 1) ? string.Concat("<b>", ROOMS.CRITERIA.POSSIBLE_TYPES_HEADER, "</b>") : "");
		RoomTypes.RoomTypeQueryResult[] array = possibleRoomTypes;
		for (int i = 0; i < array.Length; i++)
		{
			RoomTypes.RoomTypeQueryResult roomTypeQueryResult = array[i];
			RoomType type = roomTypeQueryResult.Type;
			if (type == Db.Get().RoomTypes.Neutral)
			{
				continue;
			}
			if (text != "")
			{
				text += "\n";
			}
			text = text + "<b><color=#BCBCBC>    • " + type.Name + "</b> (" + type.primary_constraint.name + ")</color>";
			if (roomTypeQueryResult.SatisfactionRating == RoomType.RoomIdentificationResult.all_satisfied)
			{
				bool flag = false;
				RoomTypes.RoomTypeQueryResult[] array2 = possibleRoomTypes;
				for (int j = 0; j < array2.Length; j++)
				{
					RoomType type2 = array2[j].Type;
					if (type2 != type && type2 != Db.Get().RoomTypes.Neutral && Db.Get().RoomTypes.HasAmbiguousRoomType(room, type, type2))
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					text += string.Format("\n<color=#F44A47FF>{0}{1}{2}</color>", "    ", "    • ", ROOMS.CRITERIA.NO_TYPE_CONFLICTS);
				}
				continue;
			}
			Constraint[] additional_constraints = type.additional_constraints;
			foreach (Constraint constraint2 in additional_constraints)
			{
				if (!constraint2.isSatisfied(room))
				{
					string empty = string.Empty;
					empty = ((constraint2.building_criteria == null) ? string.Format(ROOMS.CRITERIA.CRITERIA_FAILED.FAILED, constraint2.name) : string.Format(ROOMS.CRITERIA.CRITERIA_FAILED.MISSING_BUILDING, constraint2.name));
					text = text + "\n<color=#F44A47FF>        • " + empty + "</color>";
				}
			}
		}
		return text;
	}
}
