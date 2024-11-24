﻿using System;
using System.Collections.Generic;
using STRINGS;

namespace Database
{
	// Token: 0x0200215F RID: 8543
	public class RoomTypes : ResourceSet<RoomType>
	{
		// Token: 0x0600B5DA RID: 46554 RVA: 0x00453CBC File Offset: 0x00451EBC
		public RoomTypes(ResourceSet parent) : base("RoomTypes", parent)
		{
			base.Initialize();
			this.Neutral = base.Add(new RoomType("Neutral", ROOMS.TYPES.NEUTRAL.NAME, ROOMS.TYPES.NEUTRAL.DESCRIPTION, ROOMS.TYPES.NEUTRAL.TOOLTIP, ROOMS.TYPES.NEUTRAL.EFFECT, Db.Get().RoomTypeCategories.None, null, null, new RoomDetails.Detail[]
			{
				RoomDetails.SIZE,
				RoomDetails.BUILDING_COUNT,
				RoomDetails.CREATURE_COUNT,
				RoomDetails.PLANT_COUNT
			}, 0, null, false, false, null, 0));
			this.PlumbedBathroom = base.Add(new RoomType("PlumbedBathroom", ROOMS.TYPES.PLUMBEDBATHROOM.NAME, ROOMS.TYPES.PLUMBEDBATHROOM.DESCRIPTION, ROOMS.TYPES.PLUMBEDBATHROOM.TOOLTIP, ROOMS.TYPES.PLUMBEDBATHROOM.EFFECT, Db.Get().RoomTypeCategories.Bathroom, RoomConstraints.FLUSH_TOILET, new RoomConstraints.Constraint[]
			{
				RoomConstraints.ADVANCEDWASHSTATION,
				RoomConstraints.NO_OUTHOUSES,
				RoomConstraints.NO_INDUSTRIAL_MACHINERY,
				RoomConstraints.MINIMUM_SIZE_12,
				RoomConstraints.MAXIMUM_SIZE_64
			}, new RoomDetails.Detail[]
			{
				RoomDetails.SIZE,
				RoomDetails.BUILDING_COUNT
			}, 1, null, false, false, new string[]
			{
				"RoomBathroom"
			}, 2));
			this.Latrine = base.Add(new RoomType("Latrine", ROOMS.TYPES.LATRINE.NAME, ROOMS.TYPES.LATRINE.DESCRIPTION, ROOMS.TYPES.LATRINE.TOOLTIP, ROOMS.TYPES.LATRINE.EFFECT, Db.Get().RoomTypeCategories.Bathroom, RoomConstraints.TOILET, new RoomConstraints.Constraint[]
			{
				RoomConstraints.WASH_STATION,
				RoomConstraints.NO_INDUSTRIAL_MACHINERY,
				RoomConstraints.MINIMUM_SIZE_12,
				RoomConstraints.MAXIMUM_SIZE_64
			}, new RoomDetails.Detail[]
			{
				RoomDetails.SIZE,
				RoomDetails.BUILDING_COUNT
			}, 1, new RoomType[]
			{
				this.PlumbedBathroom
			}, false, false, new string[]
			{
				"RoomLatrine"
			}, 1));
			this.PrivateBedroom = base.Add(new RoomType("Private Bedroom", ROOMS.TYPES.PRIVATE_BEDROOM.NAME, ROOMS.TYPES.PRIVATE_BEDROOM.DESCRIPTION, ROOMS.TYPES.PRIVATE_BEDROOM.TOOLTIP, ROOMS.TYPES.PRIVATE_BEDROOM.EFFECT, Db.Get().RoomTypeCategories.Sleep, RoomConstraints.LUXURY_BED_SINGLE, new RoomConstraints.Constraint[]
			{
				RoomConstraints.NO_COTS,
				RoomConstraints.MINIMUM_SIZE_24,
				RoomConstraints.MAXIMUM_SIZE_64,
				RoomConstraints.CEILING_HEIGHT_4,
				RoomConstraints.DECORATIVE_ITEM_2,
				RoomConstraints.NO_INDUSTRIAL_MACHINERY,
				RoomConstraints.IS_BACKWALLED
			}, new RoomDetails.Detail[]
			{
				RoomDetails.SIZE,
				RoomDetails.BUILDING_COUNT
			}, 1, null, false, false, new string[]
			{
				"RoomPrivateBedroom"
			}, 5));
			this.Bedroom = base.Add(new RoomType("Bedroom", ROOMS.TYPES.BEDROOM.NAME, ROOMS.TYPES.BEDROOM.DESCRIPTION, ROOMS.TYPES.BEDROOM.TOOLTIP, ROOMS.TYPES.BEDROOM.EFFECT, Db.Get().RoomTypeCategories.Sleep, RoomConstraints.HAS_LUXURY_BED, new RoomConstraints.Constraint[]
			{
				RoomConstraints.NO_COTS,
				RoomConstraints.NO_INDUSTRIAL_MACHINERY,
				RoomConstraints.MINIMUM_SIZE_12,
				RoomConstraints.MAXIMUM_SIZE_64,
				RoomConstraints.DECORATIVE_ITEM,
				RoomConstraints.CEILING_HEIGHT_4
			}, new RoomDetails.Detail[]
			{
				RoomDetails.SIZE,
				RoomDetails.BUILDING_COUNT
			}, 1, new RoomType[]
			{
				this.PrivateBedroom
			}, false, false, new string[]
			{
				"RoomBedroom"
			}, 4));
			this.Barracks = base.Add(new RoomType("Barracks", ROOMS.TYPES.BARRACKS.NAME, ROOMS.TYPES.BARRACKS.DESCRIPTION, ROOMS.TYPES.BARRACKS.TOOLTIP, ROOMS.TYPES.BARRACKS.EFFECT, Db.Get().RoomTypeCategories.Sleep, RoomConstraints.HAS_BED, new RoomConstraints.Constraint[]
			{
				RoomConstraints.NO_INDUSTRIAL_MACHINERY,
				RoomConstraints.MINIMUM_SIZE_12,
				RoomConstraints.MAXIMUM_SIZE_64
			}, new RoomDetails.Detail[]
			{
				RoomDetails.SIZE,
				RoomDetails.BUILDING_COUNT
			}, 1, new RoomType[]
			{
				this.Bedroom,
				this.PrivateBedroom
			}, false, false, new string[]
			{
				"RoomBarracks"
			}, 3));
			this.GreatHall = base.Add(new RoomType("GreatHall", ROOMS.TYPES.GREATHALL.NAME, ROOMS.TYPES.GREATHALL.DESCRIPTION, ROOMS.TYPES.GREATHALL.TOOLTIP, ROOMS.TYPES.GREATHALL.EFFECT, Db.Get().RoomTypeCategories.Food, RoomConstraints.MESS_STATION_SINGLE, new RoomConstraints.Constraint[]
			{
				RoomConstraints.NO_INDUSTRIAL_MACHINERY,
				RoomConstraints.MINIMUM_SIZE_32,
				RoomConstraints.MAXIMUM_SIZE_120,
				RoomConstraints.DECORATIVE_ITEM_SCORE_20,
				RoomConstraints.REC_BUILDING
			}, new RoomDetails.Detail[]
			{
				RoomDetails.SIZE,
				RoomDetails.BUILDING_COUNT
			}, 1, null, false, false, new string[]
			{
				"RoomGreatHall"
			}, 6));
			this.MessHall = base.Add(new RoomType("MessHall", ROOMS.TYPES.MESSHALL.NAME, ROOMS.TYPES.MESSHALL.DESCRIPTION, ROOMS.TYPES.MESSHALL.TOOLTIP, ROOMS.TYPES.MESSHALL.EFFECT, Db.Get().RoomTypeCategories.Food, RoomConstraints.MESS_STATION_SINGLE, new RoomConstraints.Constraint[]
			{
				RoomConstraints.NO_INDUSTRIAL_MACHINERY,
				RoomConstraints.MINIMUM_SIZE_12,
				RoomConstraints.MAXIMUM_SIZE_64
			}, new RoomDetails.Detail[]
			{
				RoomDetails.SIZE,
				RoomDetails.BUILDING_COUNT
			}, 1, new RoomType[]
			{
				this.GreatHall
			}, false, false, new string[]
			{
				"RoomMessHall"
			}, 5));
			this.Kitchen = base.Add(new RoomType("Kitchen", ROOMS.TYPES.KITCHEN.NAME, ROOMS.TYPES.KITCHEN.DESCRIPTION, ROOMS.TYPES.KITCHEN.TOOLTIP, ROOMS.TYPES.KITCHEN.EFFECT, Db.Get().RoomTypeCategories.Food, RoomConstraints.SPICE_STATION, new RoomConstraints.Constraint[]
			{
				RoomConstraints.COOK_TOP,
				RoomConstraints.REFRIGERATOR,
				RoomConstraints.NO_MESS_STATION,
				RoomConstraints.MINIMUM_SIZE_12,
				RoomConstraints.MAXIMUM_SIZE_96
			}, new RoomDetails.Detail[]
			{
				RoomDetails.SIZE,
				RoomDetails.BUILDING_COUNT
			}, 1, null, false, false, null, 7));
			this.MassageClinic = base.Add(new RoomType("MassageClinic", ROOMS.TYPES.MASSAGE_CLINIC.NAME, ROOMS.TYPES.MASSAGE_CLINIC.DESCRIPTION, ROOMS.TYPES.MASSAGE_CLINIC.TOOLTIP, ROOMS.TYPES.MASSAGE_CLINIC.EFFECT, Db.Get().RoomTypeCategories.Hospital, RoomConstraints.MASSAGE_TABLE, new RoomConstraints.Constraint[]
			{
				RoomConstraints.NO_INDUSTRIAL_MACHINERY,
				RoomConstraints.DECORATIVE_ITEM,
				RoomConstraints.MINIMUM_SIZE_12,
				RoomConstraints.MAXIMUM_SIZE_64
			}, new RoomDetails.Detail[]
			{
				RoomDetails.SIZE,
				RoomDetails.BUILDING_COUNT
			}, 2, null, true, true, null, 9));
			this.Hospital = base.Add(new RoomType("Hospital", ROOMS.TYPES.HOSPITAL.NAME, ROOMS.TYPES.HOSPITAL.DESCRIPTION, ROOMS.TYPES.HOSPITAL.TOOLTIP, ROOMS.TYPES.HOSPITAL.EFFECT, Db.Get().RoomTypeCategories.Hospital, RoomConstraints.CLINIC, new RoomConstraints.Constraint[]
			{
				RoomConstraints.TOILET,
				RoomConstraints.MESS_STATION_SINGLE,
				RoomConstraints.NO_INDUSTRIAL_MACHINERY,
				RoomConstraints.MINIMUM_SIZE_12,
				RoomConstraints.MAXIMUM_SIZE_96
			}, new RoomDetails.Detail[]
			{
				RoomDetails.SIZE,
				RoomDetails.BUILDING_COUNT
			}, 2, null, true, true, null, 10));
			if (DlcManager.IsContentSubscribed("DLC3_ID"))
			{
				this.BionicUpkeep = base.Add(new RoomType("BionicUpkeep", ROOMS.TYPES.BIONICUPKEEP.NAME, ROOMS.TYPES.BIONICUPKEEP.DESCRIPTION, ROOMS.TYPES.BIONICUPKEEP.TOOLTIP, ROOMS.TYPES.BIONICUPKEEP.EFFECT, Db.Get().RoomTypeCategories.Bionic, RoomConstraints.BIONIC_GUNKEMPTIER, new RoomConstraints.Constraint[]
				{
					RoomConstraints.BIONIC_LUBRICATION,
					RoomConstraints.MINIMUM_SIZE_12,
					RoomConstraints.MAXIMUM_SIZE_64
				}, new RoomDetails.Detail[]
				{
					RoomDetails.SIZE,
					RoomDetails.BUILDING_COUNT
				}, 1, null, false, false, new string[]
				{
					"RoomBionicUpkeep"
				}, 11));
			}
			this.PowerPlant = base.Add(new RoomType("PowerPlant", ROOMS.TYPES.POWER_PLANT.NAME, ROOMS.TYPES.POWER_PLANT.DESCRIPTION, ROOMS.TYPES.POWER_PLANT.TOOLTIP, ROOMS.TYPES.POWER_PLANT.EFFECT, Db.Get().RoomTypeCategories.Industrial, RoomConstraints.POWER_STATION, new RoomConstraints.Constraint[]
			{
				RoomConstraints.MINIMUM_SIZE_12,
				RoomConstraints.MAXIMUM_SIZE_120
			}, new RoomDetails.Detail[]
			{
				RoomDetails.SIZE,
				RoomDetails.BUILDING_COUNT
			}, 2, null, true, true, null, 12));
			this.Farm = base.Add(new RoomType("Farm", ROOMS.TYPES.FARM.NAME, ROOMS.TYPES.FARM.DESCRIPTION, ROOMS.TYPES.FARM.TOOLTIP, ROOMS.TYPES.FARM.EFFECT, Db.Get().RoomTypeCategories.Agricultural, RoomConstraints.FARM_STATION, new RoomConstraints.Constraint[]
			{
				RoomConstraints.MINIMUM_SIZE_12,
				RoomConstraints.MAXIMUM_SIZE_96
			}, new RoomDetails.Detail[]
			{
				RoomDetails.SIZE,
				RoomDetails.BUILDING_COUNT
			}, 2, null, true, true, null, 13));
			this.CreaturePen = base.Add(new RoomType("CreaturePen", ROOMS.TYPES.CREATUREPEN.NAME, ROOMS.TYPES.CREATUREPEN.DESCRIPTION, ROOMS.TYPES.CREATUREPEN.TOOLTIP, ROOMS.TYPES.CREATUREPEN.EFFECT, Db.Get().RoomTypeCategories.Agricultural, RoomConstraints.RANCH_STATION, new RoomConstraints.Constraint[]
			{
				RoomConstraints.MINIMUM_SIZE_12,
				RoomConstraints.MAXIMUM_SIZE_96
			}, new RoomDetails.Detail[]
			{
				RoomDetails.SIZE,
				RoomDetails.BUILDING_COUNT,
				RoomDetails.CREATURE_COUNT
			}, 2, null, true, true, null, 14));
			this.Laboratory = base.Add(new RoomType("Laboratory", ROOMS.TYPES.LABORATORY.NAME, ROOMS.TYPES.LABORATORY.DESCRIPTION, ROOMS.TYPES.LABORATORY.TOOLTIP, ROOMS.TYPES.LABORATORY.EFFECT, Db.Get().RoomTypeCategories.Science, RoomConstraints.SCIENCE_BUILDINGS, new RoomConstraints.Constraint[]
			{
				RoomConstraints.LIGHT,
				RoomConstraints.NO_INDUSTRIAL_MACHINERY,
				RoomConstraints.MINIMUM_SIZE_32,
				RoomConstraints.MAXIMUM_SIZE_120
			}, new RoomDetails.Detail[]
			{
				RoomDetails.SIZE,
				RoomDetails.BUILDING_COUNT
			}, 2, null, true, true, null, 15));
			this.MachineShop = new RoomType("MachineShop", ROOMS.TYPES.MACHINE_SHOP.NAME, ROOMS.TYPES.MACHINE_SHOP.DESCRIPTION, ROOMS.TYPES.MACHINE_SHOP.TOOLTIP, ROOMS.TYPES.MACHINE_SHOP.EFFECT, Db.Get().RoomTypeCategories.Industrial, RoomConstraints.MACHINE_SHOP, new RoomConstraints.Constraint[]
			{
				RoomConstraints.MINIMUM_SIZE_12,
				RoomConstraints.MAXIMUM_SIZE_96
			}, new RoomDetails.Detail[]
			{
				RoomDetails.SIZE,
				RoomDetails.BUILDING_COUNT
			}, 2, null, true, true, null, 16);
			this.RecRoom = base.Add(new RoomType("RecRoom", ROOMS.TYPES.REC_ROOM.NAME, ROOMS.TYPES.REC_ROOM.DESCRIPTION, ROOMS.TYPES.REC_ROOM.TOOLTIP, ROOMS.TYPES.REC_ROOM.EFFECT, Db.Get().RoomTypeCategories.Recreation, RoomConstraints.REC_BUILDING, new RoomConstraints.Constraint[]
			{
				RoomConstraints.NO_INDUSTRIAL_MACHINERY,
				RoomConstraints.DECORATIVE_ITEM,
				RoomConstraints.MINIMUM_SIZE_12,
				RoomConstraints.MAXIMUM_SIZE_96
			}, new RoomDetails.Detail[]
			{
				RoomDetails.SIZE,
				RoomDetails.BUILDING_COUNT
			}, 0, null, true, true, null, 17));
			this.NatureReserve = base.Add(new RoomType("NatureReserve", ROOMS.TYPES.NATURERESERVE.NAME, ROOMS.TYPES.NATURERESERVE.DESCRIPTION, ROOMS.TYPES.NATURERESERVE.TOOLTIP, ROOMS.TYPES.NATURERESERVE.EFFECT, Db.Get().RoomTypeCategories.Park, RoomConstraints.PARK_BUILDING, new RoomConstraints.Constraint[]
			{
				RoomConstraints.WILDPLANTS,
				RoomConstraints.NO_INDUSTRIAL_MACHINERY,
				RoomConstraints.MINIMUM_SIZE_32,
				RoomConstraints.MAXIMUM_SIZE_120
			}, new RoomDetails.Detail[]
			{
				RoomDetails.SIZE,
				RoomDetails.BUILDING_COUNT,
				RoomDetails.CREATURE_COUNT,
				RoomDetails.PLANT_COUNT
			}, 1, null, false, false, new string[]
			{
				"RoomNatureReserve"
			}, 19));
			this.Park = base.Add(new RoomType("Park", ROOMS.TYPES.PARK.NAME, ROOMS.TYPES.PARK.DESCRIPTION, ROOMS.TYPES.PARK.TOOLTIP, ROOMS.TYPES.PARK.EFFECT, Db.Get().RoomTypeCategories.Park, RoomConstraints.PARK_BUILDING, new RoomConstraints.Constraint[]
			{
				RoomConstraints.WILDPLANT,
				RoomConstraints.NO_INDUSTRIAL_MACHINERY,
				RoomConstraints.MINIMUM_SIZE_12,
				RoomConstraints.MAXIMUM_SIZE_64
			}, new RoomDetails.Detail[]
			{
				RoomDetails.SIZE,
				RoomDetails.BUILDING_COUNT,
				RoomDetails.CREATURE_COUNT,
				RoomDetails.PLANT_COUNT
			}, 1, new RoomType[]
			{
				this.NatureReserve
			}, false, false, new string[]
			{
				"RoomPark"
			}, 18));
		}

		// Token: 0x0600B5DB RID: 46555 RVA: 0x00454950 File Offset: 0x00452B50
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
			foreach (KPrefabID kprefabID in room.buildings)
			{
				if (!(kprefabID == null) && roomType.primary_constraint.building_criteria(kprefabID))
				{
					Assignable component = kprefabID.GetComponent<Assignable>();
					if (component.assignee != null)
					{
						foreach (Ownables item in component.assignee.GetOwners())
						{
							if (!list.Contains(item))
							{
								list.Add(item);
							}
						}
					}
				}
			}
			return list.ToArray();
		}

		// Token: 0x0600B5DC RID: 46556 RVA: 0x00454A50 File Offset: 0x00452C50
		public RoomType GetRoomTypeForID(string id)
		{
			foreach (RoomType roomType in this.resources)
			{
				if (roomType.Id == id)
				{
					return roomType;
				}
			}
			return null;
		}

		// Token: 0x0600B5DD RID: 46557 RVA: 0x00454AB4 File Offset: 0x00452CB4
		public RoomType GetRoomType(Room room)
		{
			foreach (RoomType roomType in this.resources)
			{
				if (roomType != this.Neutral && roomType.isSatisfactory(room) == RoomType.RoomIdentificationResult.all_satisfied)
				{
					bool flag = false;
					foreach (RoomType roomType2 in this.resources)
					{
						if (roomType != roomType2 && roomType2 != this.Neutral && this.HasAmbiguousRoomType(room, roomType, roomType2))
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						return roomType;
					}
				}
			}
			return this.Neutral;
		}

		// Token: 0x0600B5DE RID: 46558 RVA: 0x00454B80 File Offset: 0x00452D80
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
				if (suspected_type.upgrade_paths != null && Array.IndexOf<RoomType>(suspected_type.upgrade_paths, potential_type) != -1)
				{
					return true;
				}
				if (potential_type.upgrade_paths != null && Array.IndexOf<RoomType>(potential_type.upgrade_paths, suspected_type) != -1)
				{
					return false;
				}
			}
			if (roomIdentificationResult != RoomType.RoomIdentificationResult.primary_unsatisfied)
			{
				if (suspected_type.upgrade_paths != null && Array.IndexOf<RoomType>(suspected_type.upgrade_paths, potential_type) != -1)
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
						foreach (RoomConstraints.Constraint constraint in suspected_type.additional_constraints)
						{
							if (constraint == potential_type.primary_constraint || (constraint.stomp_in_conflict != null && constraint.stomp_in_conflict.Contains(potential_type.primary_constraint)))
							{
								flag = true;
								break;
							}
						}
					}
					return !flag;
				}
				suspected_type = this.Neutral;
			}
			return false;
		}

		// Token: 0x0600B5DF RID: 46559 RVA: 0x00454CA8 File Offset: 0x00452EA8
		public RoomTypes.RoomTypeQueryResult[] GetPossibleRoomTypes(Room room)
		{
			RoomTypes.RoomTypeQueryResult[] array = new RoomTypes.RoomTypeQueryResult[this.Count];
			int num = 0;
			foreach (RoomType roomType in this.resources)
			{
				if (roomType != this.Neutral)
				{
					RoomType.RoomIdentificationResult roomIdentificationResult = roomType.isSatisfactory(room);
					if (roomIdentificationResult != RoomType.RoomIdentificationResult.primary_unsatisfied)
					{
						array[num] = new RoomTypes.RoomTypeQueryResult
						{
							Type = roomType,
							SatisfactionRating = roomIdentificationResult
						};
						num++;
					}
				}
			}
			if (num == 0)
			{
				array[num] = new RoomTypes.RoomTypeQueryResult
				{
					Type = this.Neutral,
					SatisfactionRating = RoomType.RoomIdentificationResult.all_satisfied
				};
				num++;
			}
			Array.Resize<RoomTypes.RoomTypeQueryResult>(ref array, num);
			return array;
		}

		// Token: 0x04009400 RID: 37888
		public RoomType Neutral;

		// Token: 0x04009401 RID: 37889
		public RoomType Latrine;

		// Token: 0x04009402 RID: 37890
		public RoomType PlumbedBathroom;

		// Token: 0x04009403 RID: 37891
		public RoomType Barracks;

		// Token: 0x04009404 RID: 37892
		public RoomType Bedroom;

		// Token: 0x04009405 RID: 37893
		public RoomType PrivateBedroom;

		// Token: 0x04009406 RID: 37894
		public RoomType MessHall;

		// Token: 0x04009407 RID: 37895
		public RoomType Kitchen;

		// Token: 0x04009408 RID: 37896
		public RoomType GreatHall;

		// Token: 0x04009409 RID: 37897
		public RoomType Hospital;

		// Token: 0x0400940A RID: 37898
		public RoomType MassageClinic;

		// Token: 0x0400940B RID: 37899
		public RoomType PowerPlant;

		// Token: 0x0400940C RID: 37900
		public RoomType Farm;

		// Token: 0x0400940D RID: 37901
		public RoomType CreaturePen;

		// Token: 0x0400940E RID: 37902
		public RoomType MachineShop;

		// Token: 0x0400940F RID: 37903
		public RoomType RecRoom;

		// Token: 0x04009410 RID: 37904
		public RoomType Park;

		// Token: 0x04009411 RID: 37905
		public RoomType NatureReserve;

		// Token: 0x04009412 RID: 37906
		public RoomType Laboratory;

		// Token: 0x04009413 RID: 37907
		public RoomType BionicUpkeep;

		// Token: 0x02002160 RID: 8544
		public struct RoomTypeQueryResult
		{
			// Token: 0x04009414 RID: 37908
			public RoomType Type;

			// Token: 0x04009415 RID: 37909
			public RoomType.RoomIdentificationResult SatisfactionRating;
		}
	}
}
