﻿using System;
using TUNING;

namespace STRINGS
{
	public class ROOMS
	{
		public class CATEGORY
		{
			public class NONE
			{
				public static LocString NAME = "None";
			}

			public class FOOD
			{
				public static LocString NAME = "Dining";
			}

			public class SLEEP
			{
				public static LocString NAME = "Sleep";
			}

			public class RECREATION
			{
				public static LocString NAME = "Recreation";
			}

			public class BATHROOM
			{
				public static LocString NAME = "Washroom";
			}

			public class HOSPITAL
			{
				public static LocString NAME = "Medical";
			}

			public class INDUSTRIAL
			{
				public static LocString NAME = "Industrial";
			}

			public class AGRICULTURAL
			{
				public static LocString NAME = "Agriculture";
			}

			public class PARK
			{
				public static LocString NAME = "Parks";
			}

			public class SCIENCE
			{
				public static LocString NAME = "Science";
			}
		}

		public class TYPES
		{
			public static LocString CONFLICTED = "Conflicted Room";

			public class NEUTRAL
			{
				public static LocString NAME = "Miscellaneous Room";

				public static LocString DESCRIPTION = "An enclosed space with plenty of potential and no dedicated use.";

				public static LocString EFFECT = "- No effect";

				public static LocString TOOLTIP = "This area has walls and doors but no dedicated use";
			}

			public class LATRINE
			{
				public static LocString NAME = "Latrine";

				public static LocString DESCRIPTION = "It's a step up from doing one's business in full view of the rest of the colony.\n\nUsing a toilet in an enclosed room will improve Duplicants' Morale.";

				public static LocString EFFECT = "- Morale bonus";

				public static LocString TOOLTIP = "Using a toilet in an enclosed room will improve Duplicants' Morale";
			}

			public class PLUMBEDBATHROOM
			{
				public static LocString NAME = "Washroom";

				public static LocString DESCRIPTION = "A sanctuary of personal hygiene.\n\nUsing a fully plumbed Washroom will improve Duplicants' Morale.";

				public static LocString EFFECT = "- Morale bonus";

				public static LocString TOOLTIP = "Using a fully plumbed Washroom will improve Duplicants' Morale";
			}

			public class BARRACKS
			{
				public static LocString NAME = "Barracks";

				public static LocString DESCRIPTION = "A basic communal sleeping area for up-and-coming colonies.\n\nSleeping in Barracks will improve Duplicants' Morale.";

				public static LocString EFFECT = "- Morale bonus";

				public static LocString TOOLTIP = "Sleeping in Barracks will improve Duplicants' Morale";
			}

			public class BEDROOM
			{
				public static LocString NAME = "Luxury Barracks";

				public static LocString DESCRIPTION = "An upscale communal sleeping area full of things that greatly enhance quality of rest for occupants.\n\nSleeping in a Luxury Barracks will improve Duplicants' Morale.";

				public static LocString EFFECT = "- Morale bonus";

				public static LocString TOOLTIP = "Sleeping in a Luxury Barracks will improve Duplicants' Morale";
			}

			public class PRIVATE_BEDROOM
			{
				public static LocString NAME = "Private Bedroom";

				public static LocString DESCRIPTION = "A comfortable, roommate-free retreat where tired Duplicants can get uninterrupted rest.\n\nSleeping in a Private Bedroom will greatly improve Duplicants' Morale.";

				public static LocString EFFECT = "- Morale bonus";

				public static LocString TOOLTIP = "Sleeping in a Private Bedroom will greatly improve Duplicants' Morale";
			}

			public class MESSHALL
			{
				public static LocString NAME = "Mess Hall";

				public static LocString DESCRIPTION = "A simple dining room setup that's easy to improve upon.\n\nEating at a mess table in a Mess Hall will increase Duplicants' Morale.";

				public static LocString EFFECT = "- Morale bonus";

				public static LocString TOOLTIP = "Eating at a Mess Table in a Mess Hall will improve Duplicants' Morale";
			}

			public class KITCHEN
			{
				public static LocString NAME = "Kitchen";

				public static LocString DESCRIPTION = "A cooking area equipped to take meals to the next level.\n\nAdding ingredients from a Spice Grinder to foods cooked on an Electric Grill or Gas Range provides a variety of positive benefits.";

				public static LocString EFFECT = "- Enables Spice Grinder use";

				public static LocString TOOLTIP = "Using a Spice Grinder in a Kitchen adds benefits to foods cooked on Electric Grill or Gas Range";
			}

			public class GREATHALL
			{
				public static LocString NAME = "Great Hall";

				public static LocString DESCRIPTION = "A great place to eat, with great decor and great company. Great!\n\nEating in a Great Hall will significantly improve Duplicants' Morale.";

				public static LocString EFFECT = "- Morale bonus";

				public static LocString TOOLTIP = "Eating in a Great Hall will significantly improve Duplicants' Morale";
			}

			public class HOSPITAL
			{
				public static LocString NAME = "Hospital";

				public static LocString DESCRIPTION = "A dedicated medical facility that helps minimize recovery time.\n\nSick Duplicants assigned to medical buildings located within a Hospital are also less likely to spread Disease.";

				public static LocString EFFECT = "- Quarantine sick Duplicants";

				public static LocString TOOLTIP = "Sick Duplicants assigned to medical buildings located within a Hospital are less likely to spread Disease";
			}

			public class MASSAGE_CLINIC
			{
				public static LocString NAME = "Massage Clinic";

				public static LocString DESCRIPTION = "A soothing space with a very relaxing ambience, especially when well-decorated.\n\nReceiving massages at a Massage Clinic will significantly improve Stress reduction.";

				public static LocString EFFECT = "- Massage stress relief bonus";

				public static LocString TOOLTIP = "Receiving massages at a Massage Clinic will significantly improve Stress reduction";
			}

			public class POWER_PLANT
			{
				public static LocString NAME = "Power Plant";

				public static LocString DESCRIPTION = "The perfect place for Duplicants to flex their Electrical Engineering skills.\n\nGenerators built within a Power Plant can be tuned up using power control stations to improve their power production.";

				public static LocString EFFECT = "- Enables Power Control Station use";

				public static LocString TOOLTIP = "Generators built within a Power Plant can be tuned up using Power Control Stations to improve their power production";
			}

			public class MACHINE_SHOP
			{
				public static LocString NAME = "Machine Shop";

				public static LocString DESCRIPTION = "It smells like elbow grease.\n\nDuplicants working in a Machine Shop can maintain buildings and increase their production speed.";

				public static LocString EFFECT = "- Increased fabrication efficiency";

				public static LocString TOOLTIP = "Duplicants working in a Machine Shop can maintain buildings and increase their production speed";
			}

			public class FARM
			{
				public static LocString NAME = "Greenhouse";

				public static LocString DESCRIPTION = "An enclosed agricultural space best utilized by Duplicants with Crop Tending skills.\n\nCrops grown within a Greenhouse can be tended with Farm Station fertilizer to increase their growth speed.";

				public static LocString EFFECT = "- Enables Farm Station use";

				public static LocString TOOLTIP = "Crops grown within a Greenhouse can be tended with Farm Station fertilizer to increase their growth speed";
			}

			public class CREATUREPEN
			{
				public static LocString NAME = "Stable";

				public static LocString DESCRIPTION = "Critters don't mind it here, as long as things don't get too crowded.\n\nStabled critters can be tended to in order to improve their happiness, hasten their domestication and increase their production.\n\nEnables the use of Grooming Stations, Shearing Stations, Critter Condos, Critter Fountains and Milking Stations.";

				public static LocString EFFECT = "- Critter taming and mood bonus";

				public static LocString TOOLTIP = "A stable enables Grooming Station, Critter Condo, Critter Fountain, Shearing Station and Milking Station use";
			}

			public class REC_ROOM
			{
				public static LocString NAME = "Recreation Room";

				public static LocString DESCRIPTION = "Where Duplicants go to mingle with off-duty peers and indulge in a little R&R.\n\nScheduled Downtime will further improve Morale for Duplicants visiting a Recreation Room.";

				public static LocString EFFECT = "- Morale bonus";

				public static LocString TOOLTIP = "Scheduled Downtime will further improve Morale for Duplicants visiting a Recreation Room";
			}

			public class PARK
			{
				public static LocString NAME = "Park";

				public static LocString DESCRIPTION = "A little greenery goes a long way.\n\nPassing through natural spaces throughout the day will raise the Morale of Duplicants.";

				public static LocString EFFECT = "- Morale bonus";

				public static LocString TOOLTIP = "Passing through natural spaces throughout the day will raise the Morale of Duplicants";
			}

			public class NATURERESERVE
			{
				public static LocString NAME = "Nature Reserve";

				public static LocString DESCRIPTION = "A lot of greenery goes an even longer way.\n\nPassing through a Nature Reserve will grant higher Morale bonuses to Duplicants than a Park.";

				public static LocString EFFECT = "- Morale bonus";

				public static LocString TOOLTIP = "A Nature Reserve will grant higher Morale bonuses to Duplicants than a Park";
			}

			public class LABORATORY
			{
				public static LocString NAME = "Laboratory";

				public static LocString DESCRIPTION = "Where wild hypotheses meet rigorous scientific experimentation.\n\nScience stations built in a Laboratory function more efficiently.\n\nA Laboratory enables the use of the Geotuner and the Mission Control Station.";

				public static LocString EFFECT = "- Efficiency bonus";

				public static LocString TOOLTIP = "Science buildings built in a Laboratory function more efficiently\n\nA Laboratory enables Geotuner and Mission Control Station use";
			}

			public class PRIVATE_BATHROOM
			{
				public static LocString NAME = "Private Bathroom";

				public static LocString DESCRIPTION = "Finally, a place to truly be alone with one's thoughts.\n\nDuplicants relieve even more Stress when using the toilet in a Private Bathroom than in a Latrine.";

				public static LocString EFFECT = "- Stress relief bonus";

				public static LocString TOOLTIP = "Duplicants relieve even more stress when using the toilet in a Private Bathroom than in a Latrine";
			}
		}

		public class CRITERIA
		{
			public static LocString HEADER = "<b>Requirements:</b>";

			public static LocString NEUTRAL_TYPE = "Enclosed by wall tile";

			public static LocString POSSIBLE_TYPES_HEADER = "Possible Room Types";

			public static LocString NO_TYPE_CONFLICTS = "Remove conflicting buildings";

			public static LocString IN_CODE_ERROR = "String Key Not Found: {0}";

			public class CRITERIA_FAILED
			{
				public static LocString MISSING_BUILDING = "Missing {0}";

				public static LocString FAILED = "{0}";
			}

			public static class DECORATION
			{
				public static LocString NAME = UI.FormatAsLink("Decor item", "BUILDCATEGORYREQUIREMENTCLASSDECORATION");
			}

			public class CEILING_HEIGHT
			{
				public static LocString NAME = "Minimum height: {0} tiles";

				public static LocString DESCRIPTION = "Must have a ceiling height of at least {0} tiles";
			}

			public class MINIMUM_SIZE
			{
				public static LocString NAME = "Minimum size: {0} tiles";

				public static LocString DESCRIPTION = "Must have an area of at least {0} tiles";
			}

			public class MAXIMUM_SIZE
			{
				public static LocString NAME = "Maximum size: {0} tiles";

				public static LocString DESCRIPTION = "Must have an area no larger than {0} tiles";
			}

			public class INDUSTRIALMACHINERY
			{
				public static LocString NAME = UI.FormatAsLink("Industrial machinery", "BUILDCATEGORYREQUIREMENTCLASSINDUSTRIALMACHINERY");
			}

			public class HAS_BED
			{
				public static LocString NAME = "One or more " + UI.FormatAsLink("beds", "BUILDCATEGORYREQUIREMENTCLASSBEDTYPE");

				public static LocString DESCRIPTION = "Requires at least one Cot or Comfy Bed";
			}

			public class HAS_LUXURY_BED
			{
				public static LocString NAME = "One or more " + UI.FormatAsLink("Comfy Beds", "LUXURYBED");

				public static LocString DESCRIPTION = "Requires at least one Comfy Bed";
			}

			public class LUXURYBEDTYPE
			{
				public static LocString NAME = "Single " + UI.FormatAsLink("Comfy Bed", "LUXURYBED");

				public static LocString DESCRIPTION = "Must have no more than one Comfy Bed";
			}

			public class BED_SINGLE
			{
				public static LocString NAME = "Single " + UI.FormatAsLink("beds", "BUILDCATEGORYREQUIREMENTCLASSBEDTYPE");

				public static LocString DESCRIPTION = "Must have no more than one Cot or Comfy Bed";
			}

			public class IS_BACKWALLED
			{
				public static LocString NAME = "Has backwall tiles";

				public static LocString DESCRIPTION = "Must be covered in backwall tiles";
			}

			public class NO_COTS
			{
				public static LocString NAME = "No " + UI.FormatAsLink("Cots", "BED");

				public static LocString DESCRIPTION = "Room cannot contain a Cot";
			}

			public class NO_LUXURY_BEDS
			{
				public static LocString NAME = "No " + UI.FormatAsLink("Comfy Beds", "LUXURYBED");

				public static LocString DESCRIPTION = "Room cannot contain a Comfy Bed";
			}

			public class BEDTYPE
			{
				public static LocString NAME = UI.FormatAsLink("Beds", "BUILDCATEGORYREQUIREMENTCLASSBEDTYPE");

				public static LocString DESCRIPTION = "Requires two or more Cots or Comfy Beds";
			}

			public class BUILDING_DECOR_POSITIVE
			{
				public static LocString NAME = "Positive " + UI.FormatAsLink("decor", "BUILDCATEGORYREQUIREMENTCLASSDECORATION");

				public static LocString DESCRIPTION = "Requires at least one building with positive decor";
			}

			public class DECORATIVE_ITEM
			{
				public static LocString NAME = UI.FormatAsLink("Decor item", "BUILDCATEGORYREQUIREMENTCLASSDECORATION") + " ({0})";

				public static LocString DESCRIPTION = "Requires {0} or more Decor items";
			}

			public class DECOR20
			{
				// Note: this type is marked as 'beforefieldinit'.
				static DECOR20()
				{
					string str = "Requires a decorative item with a minimum Decor value of ";
					int amount = BUILDINGS.DECOR.BONUS.TIER3.amount;
					ROOMS.CRITERIA.DECOR20.DESCRIPTION = str + amount.ToString();
				}

				public static LocString NAME = UI.FormatAsLink("Fancy decor item", "BUILDCATEGORYREQUIREMENTCLASSDECORATION");

				public static LocString DESCRIPTION;
			}

			public class CLINIC
			{
				public static LocString NAME = UI.FormatAsLink("Medical equipment", "BUILDCATEGORYREQUIREMENTCLASSCLINIC");

				public static LocString DESCRIPTION = "Requires one or more Sick Bays or Disease Clinics";
			}

			public class POWERSTATION
			{
				public static LocString NAME = UI.FormatAsLink("Power Control Station", "POWERCONTROLSTATION");

				public static LocString DESCRIPTION = "Requires a single Power Control Station";
			}

			public class FARMSTATIONTYPE
			{
				public static LocString NAME = UI.FormatAsLink("Farm Station", "FARMSTATION");

				public static LocString DESCRIPTION = "Requires a single Farm Station";
			}

			public class CREATURERELOCATOR
			{
				public static LocString NAME = UI.FormatAsLink("Critter relocator", "BUILDCATEGORYREQUIREMENTCLASSCREATURERELOCATOR");

				public static LocString DESCRIPTION = "Requires a single Critter Drop-Off or Fish Release";
			}

			public class CREATURE_FEEDER
			{
				public static LocString NAME = UI.FormatAsLink("Critter Feeder", "CREATUREFEEDER");

				public static LocString DESCRIPTION = "Requires a single Critter Feeder";
			}

			public class RANCHSTATIONTYPE
			{
				public static LocString NAME = UI.FormatAsLink("Ranching building", "BUILDCATEGORYREQUIREMENTCLASSRANCHSTATIONTYPE");

				public static LocString DESCRIPTION = "Requires a single Grooming Station, Critter Condo, Critter Fountain, Shearing Station or Milking Station";
			}

			public class SPICESTATION
			{
				public static LocString NAME = UI.FormatAsLink("Spice Grinder", "SPICEGRINDER");

				public static LocString DESCRIPTION = "Requires a single Spice Grinder";
			}

			public class COOKTOP
			{
				public static LocString NAME = UI.FormatAsLink("Cooking station", "BUILDCATEGORYREQUIREMENTCLASSCOOKTOP");

				public static LocString DESCRIPTION = "Requires a single Electric Grill or Gas Range";
			}

			public class REFRIGERATOR
			{
				public static LocString NAME = UI.FormatAsLink("Refrigerator", "REFRIGERATOR");

				public static LocString DESCRIPTION = "Requires a single Refrigerator";
			}

			public class RECBUILDING
			{
				public static LocString NAME = UI.FormatAsLink("Recreational building", "BUILDCATEGORYREQUIREMENTCLASSRECBUILDING");

				public static LocString DESCRIPTION = "Requires one or more recreational buildings";
			}

			public class PARK
			{
				public static LocString NAME = UI.FormatAsLink("Park Sign", "PARKSIGN");

				public static LocString DESCRIPTION = "Requires one or more Park Signs";
			}

			public class MACHINESHOPTYPE
			{
				public static LocString NAME = "Mechanics Station";

				public static LocString DESCRIPTION = "Requires requires one or more Mechanics Stations";
			}

			public class FOOD_BOX
			{
				public static LocString NAME = "Food storage";

				public static LocString DESCRIPTION = "Requires one or more Ration Boxes or Refrigerators";
			}

			public class LIGHTSOURCE
			{
				public static LocString NAME = UI.FormatAsLink("Light source", "BUILDCATEGORYREQUIREMENTCLASSLIGHTSOURCE");

				public static LocString DESCRIPTION = "Requires one or more light sources";
			}

			public class DESTRESSINGBUILDING
			{
				public static LocString NAME = UI.FormatAsLink("De-Stressing Building", "MASSAGETABLE");

				public static LocString DESCRIPTION = "Requires one or more De-Stressing buildings";
			}

			public class MASSAGE_TABLE
			{
				public static LocString NAME = UI.FormatAsLink("Massage Table", "MASSAGETABLE");

				public static LocString DESCRIPTION = "Requires one or more Massage Tables";
			}

			public class MESSTABLE
			{
				public static LocString NAME = UI.FormatAsLink("Mess Table", "DININGTABLE");

				public static LocString DESCRIPTION = "Requires a single Mess Table";
			}

			public class NO_MESS_STATION
			{
				public static LocString NAME = "No " + UI.FormatAsLink("Mess Table", "DININGTABLE");

				public static LocString DESCRIPTION = "Cannot contain a Mess Table";
			}

			public class MESS_STATION_MULTIPLE
			{
				public static LocString NAME = UI.FormatAsLink("Mess Tables", "DININGTABLE");

				public static LocString DESCRIPTION = "Requires two or more Mess Tables";
			}

			public class RESEARCH_STATION
			{
				public static LocString NAME = UI.FormatAsLink("Research station", "BUILDCATEGORYREQUIREMENTCLASSRESEARCH_STATION");

				public static LocString DESCRIPTION = "Requires one or more Research Stations or Super Computers";
			}

			public class TOILETTYPE
			{
				public static LocString NAME = UI.FormatAsLink("Toilet", "BUILDCATEGORYREQUIREMENTCLASSTOILETTYPE");

				public static LocString DESCRIPTION = "Requires one or more Outhouses or Lavatories";
			}

			public class FLUSHTOILETTYPE
			{
				public static LocString NAME = UI.FormatAsLink("Flush Toilet", "BUILDCATEGORYREQUIREMENTCLASSFLUSHTOILETTYPE");

				public static LocString DESCRIPTION = "Requires one or more Lavatories";
			}

			public class NO_OUTHOUSES
			{
				public static LocString NAME = "No " + UI.FormatAsLink("Outhouses", "OUTHOUSE");

				public static LocString DESCRIPTION = "Cannot contain basic Outhouses";
			}

			public class WASHSTATION
			{
				public static LocString NAME = UI.FormatAsLink("Wash station", "BUILDCATEGORYREQUIREMENTCLASSWASHSTATION");

				public static LocString DESCRIPTION = "Requires one or more Wash Basins, Sinks, Hand Sanitizers, or Showers";
			}

			public class ADVANCEDWASHSTATION
			{
				public static LocString NAME = UI.FormatAsLink("Plumbed wash station", "BUILDCATEGORYREQUIREMENTCLASSWASHSTATION");

				public static LocString DESCRIPTION = "Requires one or more Sinks, Hand Sanitizers, or Showers";
			}

			public class NO_INDUSTRIAL_MACHINERY
			{
				public static LocString NAME = "No " + UI.FormatAsLink("industrial machinery", "BUILDCATEGORYREQUIREMENTCLASSINDUSTRIALMACHINERY");

				public static LocString DESCRIPTION = "Cannot contain any building labeled Industrial Machinery";
			}

			public class WILDANIMAL
			{
				public static LocString NAME = "Wildlife";

				public static LocString DESCRIPTION = "Requires at least one wild critter";
			}

			public class WILDANIMALS
			{
				public static LocString NAME = "More wildlife";

				public static LocString DESCRIPTION = "Requires two or more wild critters";
			}

			public class WILDPLANT
			{
				public static LocString NAME = "Two wild plants";

				public static LocString DESCRIPTION = "Requires two or more wild plants";
			}

			public class WILDPLANTS
			{
				public static LocString NAME = "Four wild plants";

				public static LocString DESCRIPTION = "Requires four or more wild plants";
			}

			public class SCIENCEBUILDING
			{
				public static LocString NAME = UI.FormatAsLink("Science building", "BUILDCATEGORYREQUIREMENTCLASSSCIENCEBUILDING");

				public static LocString DESCRIPTION = "Requires one or more science buildings";
			}

			public class SCIENCE_BUILDINGS
			{
				public static LocString NAME = "Two " + UI.FormatAsLink("science buildings", "BUILDCATEGORYREQUIREMENTCLASSSCIENCEBUILDING");

				public static LocString DESCRIPTION = "Requires two or more science buildings";
			}

			public class ROCKETINTERIOR
			{
				public static LocString NAME = UI.FormatAsLink("Rocket interior", "BUILDCATEGORYREQUIREMENTCLASSROCKETINTERIOR");

				public static LocString DESCRIPTION = "Must be built inside a rocket";
			}

			public class WARMINGSTATION
			{
				public static LocString NAME = UI.FormatAsLink("Warming station", "BUILDCATEGORYREQUIREMENTCLASSWARMINGSTATION");

				public static LocString DESCRIPTION = "Raises the ambient temperature";
			}
		}

		public class DETAILS
		{
			public static LocString HEADER = "Room Details";

			public class ASSIGNED_TO
			{
				public static LocString NAME = "<b>Assignments:</b>\n{0}";

				public static LocString UNASSIGNED = "Unassigned";
			}

			public class AVERAGE_TEMPERATURE
			{
				public static LocString NAME = "Average temperature: {0}";
			}

			public class AVERAGE_ATMO_MASS
			{
				public static LocString NAME = "Average air pressure: {0}";
			}

			public class SIZE
			{
				public static LocString NAME = "Room size: {0} Tiles";
			}

			public class BUILDING_COUNT
			{
				public static LocString NAME = "Buildings: {0}";
			}

			public class CREATURE_COUNT
			{
				public static LocString NAME = "Critters: {0}";
			}

			public class PLANT_COUNT
			{
				public static LocString NAME = "Plants: {0}";
			}
		}

		public class EFFECTS
		{
			public static LocString HEADER = "<b>Effects:</b>";
		}
	}
}
