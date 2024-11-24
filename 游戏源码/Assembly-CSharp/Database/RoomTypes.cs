using System;
using System.Collections.Generic;
using STRINGS;

namespace Database;

public class RoomTypes : ResourceSet<RoomType>
{
	public struct RoomTypeQueryResult
	{
		public RoomType Type;

		public RoomType.RoomIdentificationResult SatisfactionRating;
	}

	public RoomType Neutral;

	public RoomType Latrine;

	public RoomType PlumbedBathroom;

	public RoomType Barracks;

	public RoomType Bedroom;

	public RoomType PrivateBedroom;

	public RoomType MessHall;

	public RoomType Kitchen;

	public RoomType GreatHall;

	public RoomType Hospital;

	public RoomType MassageClinic;

	public RoomType PowerPlant;

	public RoomType Farm;

	public RoomType CreaturePen;

	public RoomType MachineShop;

	public RoomType RecRoom;

	public RoomType Park;

	public RoomType NatureReserve;

	public RoomType Laboratory;

	public RoomTypes(ResourceSet parent)
		: base("RoomTypes", parent)
	{
		Initialize();
		Neutral = Add(new RoomType("Neutral", ROOMS.TYPES.NEUTRAL.NAME, ROOMS.TYPES.NEUTRAL.DESCRIPTION, ROOMS.TYPES.NEUTRAL.TOOLTIP, ROOMS.TYPES.NEUTRAL.EFFECT, Db.Get().RoomTypeCategories.None, null, null, new RoomDetails.Detail[4]
		{
			RoomDetails.SIZE,
			RoomDetails.BUILDING_COUNT,
			RoomDetails.CREATURE_COUNT,
			RoomDetails.PLANT_COUNT
		}));
		PlumbedBathroom = Add(new RoomType("PlumbedBathroom", ROOMS.TYPES.PLUMBEDBATHROOM.NAME, ROOMS.TYPES.PLUMBEDBATHROOM.DESCRIPTION, ROOMS.TYPES.PLUMBEDBATHROOM.TOOLTIP, ROOMS.TYPES.PLUMBEDBATHROOM.EFFECT, Db.Get().RoomTypeCategories.Bathroom, RoomConstraints.FLUSH_TOILET, new RoomConstraints.Constraint[5]
		{
			RoomConstraints.ADVANCEDWASHSTATION,
			RoomConstraints.NO_OUTHOUSES,
			RoomConstraints.NO_INDUSTRIAL_MACHINERY,
			RoomConstraints.MINIMUM_SIZE_12,
			RoomConstraints.MAXIMUM_SIZE_64
		}, new RoomDetails.Detail[2]
		{
			RoomDetails.SIZE,
			RoomDetails.BUILDING_COUNT
		}, 1, null, single_assignee: false, priority_building_use: false, new string[1] { "RoomBathroom" }, 2));
		Latrine = Add(new RoomType("Latrine", ROOMS.TYPES.LATRINE.NAME, ROOMS.TYPES.LATRINE.DESCRIPTION, ROOMS.TYPES.LATRINE.TOOLTIP, ROOMS.TYPES.LATRINE.EFFECT, Db.Get().RoomTypeCategories.Bathroom, RoomConstraints.TOILET, new RoomConstraints.Constraint[4]
		{
			RoomConstraints.WASH_STATION,
			RoomConstraints.NO_INDUSTRIAL_MACHINERY,
			RoomConstraints.MINIMUM_SIZE_12,
			RoomConstraints.MAXIMUM_SIZE_64
		}, new RoomDetails.Detail[2]
		{
			RoomDetails.SIZE,
			RoomDetails.BUILDING_COUNT
		}, 1, new RoomType[1] { PlumbedBathroom }, single_assignee: false, priority_building_use: false, new string[1] { "RoomLatrine" }, 1));
		PrivateBedroom = Add(new RoomType("Private Bedroom", ROOMS.TYPES.PRIVATE_BEDROOM.NAME, ROOMS.TYPES.PRIVATE_BEDROOM.DESCRIPTION, ROOMS.TYPES.PRIVATE_BEDROOM.TOOLTIP, ROOMS.TYPES.PRIVATE_BEDROOM.EFFECT, Db.Get().RoomTypeCategories.Sleep, RoomConstraints.LUXURY_BED_SINGLE, new RoomConstraints.Constraint[7]
		{
			RoomConstraints.NO_COTS,
			RoomConstraints.MINIMUM_SIZE_24,
			RoomConstraints.MAXIMUM_SIZE_64,
			RoomConstraints.CEILING_HEIGHT_4,
			RoomConstraints.DECORATIVE_ITEM_2,
			RoomConstraints.NO_INDUSTRIAL_MACHINERY,
			RoomConstraints.IS_BACKWALLED
		}, new RoomDetails.Detail[2]
		{
			RoomDetails.SIZE,
			RoomDetails.BUILDING_COUNT
		}, 1, null, single_assignee: false, priority_building_use: false, new string[1] { "RoomPrivateBedroom" }, 5));
		Bedroom = Add(new RoomType("Bedroom", ROOMS.TYPES.BEDROOM.NAME, ROOMS.TYPES.BEDROOM.DESCRIPTION, ROOMS.TYPES.BEDROOM.TOOLTIP, ROOMS.TYPES.BEDROOM.EFFECT, Db.Get().RoomTypeCategories.Sleep, RoomConstraints.HAS_LUXURY_BED, new RoomConstraints.Constraint[6]
		{
			RoomConstraints.NO_COTS,
			RoomConstraints.NO_INDUSTRIAL_MACHINERY,
			RoomConstraints.MINIMUM_SIZE_12,
			RoomConstraints.MAXIMUM_SIZE_64,
			RoomConstraints.DECORATIVE_ITEM,
			RoomConstraints.CEILING_HEIGHT_4
		}, new RoomDetails.Detail[2]
		{
			RoomDetails.SIZE,
			RoomDetails.BUILDING_COUNT
		}, 1, new RoomType[1] { PrivateBedroom }, single_assignee: false, priority_building_use: false, new string[1] { "RoomBedroom" }, 4));
		Barracks = Add(new RoomType("Barracks", ROOMS.TYPES.BARRACKS.NAME, ROOMS.TYPES.BARRACKS.DESCRIPTION, ROOMS.TYPES.BARRACKS.TOOLTIP, ROOMS.TYPES.BARRACKS.EFFECT, Db.Get().RoomTypeCategories.Sleep, RoomConstraints.HAS_BED, new RoomConstraints.Constraint[3]
		{
			RoomConstraints.NO_INDUSTRIAL_MACHINERY,
			RoomConstraints.MINIMUM_SIZE_12,
			RoomConstraints.MAXIMUM_SIZE_64
		}, new RoomDetails.Detail[2]
		{
			RoomDetails.SIZE,
			RoomDetails.BUILDING_COUNT
		}, 1, new RoomType[2] { Bedroom, PrivateBedroom }, single_assignee: false, priority_building_use: false, new string[1] { "RoomBarracks" }, 3));
		GreatHall = Add(new RoomType("GreatHall", ROOMS.TYPES.GREATHALL.NAME, ROOMS.TYPES.GREATHALL.DESCRIPTION, ROOMS.TYPES.GREATHALL.TOOLTIP, ROOMS.TYPES.GREATHALL.EFFECT, Db.Get().RoomTypeCategories.Food, RoomConstraints.MESS_STATION_SINGLE, new RoomConstraints.Constraint[5]
		{
			RoomConstraints.NO_INDUSTRIAL_MACHINERY,
			RoomConstraints.MINIMUM_SIZE_32,
			RoomConstraints.MAXIMUM_SIZE_120,
			RoomConstraints.DECORATIVE_ITEM_SCORE_20,
			RoomConstraints.REC_BUILDING
		}, new RoomDetails.Detail[2]
		{
			RoomDetails.SIZE,
			RoomDetails.BUILDING_COUNT
		}, 1, null, single_assignee: false, priority_building_use: false, new string[1] { "RoomGreatHall" }, 6));
		MessHall = Add(new RoomType("MessHall", ROOMS.TYPES.MESSHALL.NAME, ROOMS.TYPES.MESSHALL.DESCRIPTION, ROOMS.TYPES.MESSHALL.TOOLTIP, ROOMS.TYPES.MESSHALL.EFFECT, Db.Get().RoomTypeCategories.Food, RoomConstraints.MESS_STATION_SINGLE, new RoomConstraints.Constraint[3]
		{
			RoomConstraints.NO_INDUSTRIAL_MACHINERY,
			RoomConstraints.MINIMUM_SIZE_12,
			RoomConstraints.MAXIMUM_SIZE_64
		}, new RoomDetails.Detail[2]
		{
			RoomDetails.SIZE,
			RoomDetails.BUILDING_COUNT
		}, 1, new RoomType[1] { GreatHall }, single_assignee: false, priority_building_use: false, new string[1] { "RoomMessHall" }, 5));
		Kitchen = Add(new RoomType("Kitchen", ROOMS.TYPES.KITCHEN.NAME, ROOMS.TYPES.KITCHEN.DESCRIPTION, ROOMS.TYPES.KITCHEN.TOOLTIP, ROOMS.TYPES.KITCHEN.EFFECT, Db.Get().RoomTypeCategories.Food, RoomConstraints.SPICE_STATION, new RoomConstraints.Constraint[5]
		{
			RoomConstraints.COOK_TOP,
			RoomConstraints.REFRIGERATOR,
			RoomConstraints.NO_MESS_STATION,
			RoomConstraints.MINIMUM_SIZE_12,
			RoomConstraints.MAXIMUM_SIZE_96
		}, new RoomDetails.Detail[2]
		{
			RoomDetails.SIZE,
			RoomDetails.BUILDING_COUNT
		}, 1, null, single_assignee: false, priority_building_use: false, null, 7));
		MassageClinic = Add(new RoomType("MassageClinic", ROOMS.TYPES.MASSAGE_CLINIC.NAME, ROOMS.TYPES.MASSAGE_CLINIC.DESCRIPTION, ROOMS.TYPES.MASSAGE_CLINIC.TOOLTIP, ROOMS.TYPES.MASSAGE_CLINIC.EFFECT, Db.Get().RoomTypeCategories.Hospital, RoomConstraints.MASSAGE_TABLE, new RoomConstraints.Constraint[4]
		{
			RoomConstraints.NO_INDUSTRIAL_MACHINERY,
			RoomConstraints.DECORATIVE_ITEM,
			RoomConstraints.MINIMUM_SIZE_12,
			RoomConstraints.MAXIMUM_SIZE_64
		}, new RoomDetails.Detail[2]
		{
			RoomDetails.SIZE,
			RoomDetails.BUILDING_COUNT
		}, 2, null, single_assignee: true, priority_building_use: true, null, 9));
		Hospital = Add(new RoomType("Hospital", ROOMS.TYPES.HOSPITAL.NAME, ROOMS.TYPES.HOSPITAL.DESCRIPTION, ROOMS.TYPES.HOSPITAL.TOOLTIP, ROOMS.TYPES.HOSPITAL.EFFECT, Db.Get().RoomTypeCategories.Hospital, RoomConstraints.CLINIC, new RoomConstraints.Constraint[5]
		{
			RoomConstraints.TOILET,
			RoomConstraints.MESS_STATION_SINGLE,
			RoomConstraints.NO_INDUSTRIAL_MACHINERY,
			RoomConstraints.MINIMUM_SIZE_12,
			RoomConstraints.MAXIMUM_SIZE_96
		}, new RoomDetails.Detail[2]
		{
			RoomDetails.SIZE,
			RoomDetails.BUILDING_COUNT
		}, 2, null, single_assignee: true, priority_building_use: true, null, 10));
		PowerPlant = Add(new RoomType("PowerPlant", ROOMS.TYPES.POWER_PLANT.NAME, ROOMS.TYPES.POWER_PLANT.DESCRIPTION, ROOMS.TYPES.POWER_PLANT.TOOLTIP, ROOMS.TYPES.POWER_PLANT.EFFECT, Db.Get().RoomTypeCategories.Industrial, RoomConstraints.POWER_STATION, new RoomConstraints.Constraint[2]
		{
			RoomConstraints.MINIMUM_SIZE_12,
			RoomConstraints.MAXIMUM_SIZE_96
		}, new RoomDetails.Detail[2]
		{
			RoomDetails.SIZE,
			RoomDetails.BUILDING_COUNT
		}, 2, null, single_assignee: true, priority_building_use: true, null, 11));
		Farm = Add(new RoomType("Farm", ROOMS.TYPES.FARM.NAME, ROOMS.TYPES.FARM.DESCRIPTION, ROOMS.TYPES.FARM.TOOLTIP, ROOMS.TYPES.FARM.EFFECT, Db.Get().RoomTypeCategories.Agricultural, RoomConstraints.FARM_STATION, new RoomConstraints.Constraint[2]
		{
			RoomConstraints.MINIMUM_SIZE_12,
			RoomConstraints.MAXIMUM_SIZE_96
		}, new RoomDetails.Detail[2]
		{
			RoomDetails.SIZE,
			RoomDetails.BUILDING_COUNT
		}, 2, null, single_assignee: true, priority_building_use: true, null, 12));
		CreaturePen = Add(new RoomType("CreaturePen", ROOMS.TYPES.CREATUREPEN.NAME, ROOMS.TYPES.CREATUREPEN.DESCRIPTION, ROOMS.TYPES.CREATUREPEN.TOOLTIP, ROOMS.TYPES.CREATUREPEN.EFFECT, Db.Get().RoomTypeCategories.Agricultural, RoomConstraints.RANCH_STATION, new RoomConstraints.Constraint[2]
		{
			RoomConstraints.MINIMUM_SIZE_12,
			RoomConstraints.MAXIMUM_SIZE_96
		}, new RoomDetails.Detail[3]
		{
			RoomDetails.SIZE,
			RoomDetails.BUILDING_COUNT,
			RoomDetails.CREATURE_COUNT
		}, 2, null, single_assignee: true, priority_building_use: true, null, 13));
		Laboratory = Add(new RoomType("Laboratory", ROOMS.TYPES.LABORATORY.NAME, ROOMS.TYPES.LABORATORY.DESCRIPTION, ROOMS.TYPES.LABORATORY.TOOLTIP, ROOMS.TYPES.LABORATORY.EFFECT, Db.Get().RoomTypeCategories.Science, RoomConstraints.SCIENCE_BUILDINGS, new RoomConstraints.Constraint[4]
		{
			RoomConstraints.LIGHT,
			RoomConstraints.NO_INDUSTRIAL_MACHINERY,
			RoomConstraints.MINIMUM_SIZE_32,
			RoomConstraints.MAXIMUM_SIZE_120
		}, new RoomDetails.Detail[2]
		{
			RoomDetails.SIZE,
			RoomDetails.BUILDING_COUNT
		}, 2, null, single_assignee: true, priority_building_use: true, null, 14));
		MachineShop = new RoomType("MachineShop", ROOMS.TYPES.MACHINE_SHOP.NAME, ROOMS.TYPES.MACHINE_SHOP.DESCRIPTION, ROOMS.TYPES.MACHINE_SHOP.TOOLTIP, ROOMS.TYPES.MACHINE_SHOP.EFFECT, Db.Get().RoomTypeCategories.Industrial, RoomConstraints.MACHINE_SHOP, new RoomConstraints.Constraint[2]
		{
			RoomConstraints.MINIMUM_SIZE_12,
			RoomConstraints.MAXIMUM_SIZE_96
		}, new RoomDetails.Detail[2]
		{
			RoomDetails.SIZE,
			RoomDetails.BUILDING_COUNT
		}, 2, null, single_assignee: true, priority_building_use: true, null, 14);
		RecRoom = Add(new RoomType("RecRoom", ROOMS.TYPES.REC_ROOM.NAME, ROOMS.TYPES.REC_ROOM.DESCRIPTION, ROOMS.TYPES.REC_ROOM.TOOLTIP, ROOMS.TYPES.REC_ROOM.EFFECT, Db.Get().RoomTypeCategories.Recreation, RoomConstraints.REC_BUILDING, new RoomConstraints.Constraint[4]
		{
			RoomConstraints.NO_INDUSTRIAL_MACHINERY,
			RoomConstraints.DECORATIVE_ITEM,
			RoomConstraints.MINIMUM_SIZE_12,
			RoomConstraints.MAXIMUM_SIZE_96
		}, new RoomDetails.Detail[2]
		{
			RoomDetails.SIZE,
			RoomDetails.BUILDING_COUNT
		}, 0, null, single_assignee: true, priority_building_use: true, null, 15));
		NatureReserve = Add(new RoomType("NatureReserve", ROOMS.TYPES.NATURERESERVE.NAME, ROOMS.TYPES.NATURERESERVE.DESCRIPTION, ROOMS.TYPES.NATURERESERVE.TOOLTIP, ROOMS.TYPES.NATURERESERVE.EFFECT, Db.Get().RoomTypeCategories.Park, RoomConstraints.PARK_BUILDING, new RoomConstraints.Constraint[4]
		{
			RoomConstraints.WILDPLANTS,
			RoomConstraints.NO_INDUSTRIAL_MACHINERY,
			RoomConstraints.MINIMUM_SIZE_32,
			RoomConstraints.MAXIMUM_SIZE_120
		}, new RoomDetails.Detail[4]
		{
			RoomDetails.SIZE,
			RoomDetails.BUILDING_COUNT,
			RoomDetails.CREATURE_COUNT,
			RoomDetails.PLANT_COUNT
		}, 1, null, single_assignee: false, priority_building_use: false, new string[1] { "RoomNatureReserve" }, 17));
		Park = Add(new RoomType("Park", ROOMS.TYPES.PARK.NAME, ROOMS.TYPES.PARK.DESCRIPTION, ROOMS.TYPES.PARK.TOOLTIP, ROOMS.TYPES.PARK.EFFECT, Db.Get().RoomTypeCategories.Park, RoomConstraints.PARK_BUILDING, new RoomConstraints.Constraint[4]
		{
			RoomConstraints.WILDPLANT,
			RoomConstraints.NO_INDUSTRIAL_MACHINERY,
			RoomConstraints.MINIMUM_SIZE_12,
			RoomConstraints.MAXIMUM_SIZE_64
		}, new RoomDetails.Detail[4]
		{
			RoomDetails.SIZE,
			RoomDetails.BUILDING_COUNT,
			RoomDetails.CREATURE_COUNT,
			RoomDetails.PLANT_COUNT
		}, 1, new RoomType[1] { NatureReserve }, single_assignee: false, priority_building_use: false, new string[1] { "RoomPark" }, 16));
	}

	public Assignables[] GetAssignees(Room room)
	{
		if (room == null)
		{
			return new Assignables[0];
		}
		RoomType roomType = room.roomType;
		if (roomType.primary_constraint == null)
		{
			return new Assignables[0];
		}
		List<Assignables> list = new List<Assignables>();
		foreach (KPrefabID building in room.buildings)
		{
			if (building == null || !roomType.primary_constraint.building_criteria(building))
			{
				continue;
			}
			Assignable component = building.GetComponent<Assignable>();
			if (component.assignee == null)
			{
				continue;
			}
			foreach (Ownables owner in component.assignee.GetOwners())
			{
				if (!list.Contains(owner))
				{
					list.Add(owner);
				}
			}
		}
		return list.ToArray();
	}

	public RoomType GetRoomTypeForID(string id)
	{
		foreach (RoomType resource in resources)
		{
			if (resource.Id == id)
			{
				return resource;
			}
		}
		return null;
	}

	public RoomType GetRoomType(Room room)
	{
		foreach (RoomType resource in resources)
		{
			if (resource == Neutral || resource.isSatisfactory(room) != 0)
			{
				continue;
			}
			bool flag = false;
			foreach (RoomType resource2 in resources)
			{
				if (resource != resource2 && resource2 != Neutral && HasAmbiguousRoomType(room, resource, resource2))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				return resource;
			}
		}
		return Neutral;
	}

	public bool HasAmbiguousRoomType(Room room, RoomType suspected_type, RoomType potential_type)
	{
		RoomType.RoomIdentificationResult roomIdentificationResult = potential_type.isSatisfactory(room);
		RoomType.RoomIdentificationResult roomIdentificationResult2 = suspected_type.isSatisfactory(room);
		if (roomIdentificationResult == RoomType.RoomIdentificationResult.all_satisfied && roomIdentificationResult2 == RoomType.RoomIdentificationResult.all_satisfied)
		{
			if (potential_type.priority > suspected_type.priority)
			{
				return true;
			}
			if (suspected_type.upgrade_paths != null && Array.IndexOf(suspected_type.upgrade_paths, potential_type) != -1)
			{
				return true;
			}
			if (potential_type.upgrade_paths != null && Array.IndexOf(potential_type.upgrade_paths, suspected_type) != -1)
			{
				return false;
			}
		}
		if (roomIdentificationResult != RoomType.RoomIdentificationResult.primary_unsatisfied)
		{
			if (suspected_type.upgrade_paths != null && Array.IndexOf(suspected_type.upgrade_paths, potential_type) != -1)
			{
				return false;
			}
			if (suspected_type.primary_constraint != potential_type.primary_constraint)
			{
				bool flag = false;
				if (suspected_type.primary_constraint.stomp_in_conflict != null && suspected_type.primary_constraint.stomp_in_conflict.Contains(potential_type.primary_constraint))
				{
					flag = true;
				}
				else if (suspected_type.additional_constraints != null)
				{
					RoomConstraints.Constraint[] additional_constraints = suspected_type.additional_constraints;
					foreach (RoomConstraints.Constraint constraint in additional_constraints)
					{
						if (constraint == potential_type.primary_constraint || (constraint.stomp_in_conflict != null && constraint.stomp_in_conflict.Contains(potential_type.primary_constraint)))
						{
							flag = true;
							break;
						}
					}
				}
				if (flag)
				{
					return false;
				}
				return true;
			}
			suspected_type = Neutral;
		}
		return false;
	}

	public RoomTypeQueryResult[] GetPossibleRoomTypes(Room room)
	{
		RoomTypeQueryResult[] array = new RoomTypeQueryResult[Count];
		int num = 0;
		foreach (RoomType resource in resources)
		{
			if (resource != Neutral)
			{
				RoomType.RoomIdentificationResult roomIdentificationResult = resource.isSatisfactory(room);
				if (roomIdentificationResult != RoomType.RoomIdentificationResult.primary_unsatisfied)
				{
					array[num] = new RoomTypeQueryResult
					{
						Type = resource,
						SatisfactionRating = roomIdentificationResult
					};
					num++;
				}
			}
		}
		if (num == 0)
		{
			array[num] = new RoomTypeQueryResult
			{
				Type = Neutral,
				SatisfactionRating = RoomType.RoomIdentificationResult.all_satisfied
			};
			num++;
		}
		Array.Resize(ref array, num);
		return array;
	}
}
