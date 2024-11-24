using System;

namespace STRINGS
{
	// Token: 0x02003892 RID: 14482
	public class ROBOTS
	{
		// Token: 0x0400DB67 RID: 56167
		public static LocString CATEGORY_NAME = "Robots";

		// Token: 0x02003893 RID: 14483
		public class STATS
		{
			// Token: 0x02003894 RID: 14484
			public class INTERNALBATTERY
			{
				// Token: 0x0400DB68 RID: 56168
				public static LocString NAME = "Rechargeable Battery";

				// Token: 0x0400DB69 RID: 56169
				public static LocString TOOLTIP = "When this bot's battery runs out it must temporarily stop working to go recharge";
			}

			// Token: 0x02003895 RID: 14485
			public class INTERNALCHEMICALBATTERY
			{
				// Token: 0x0400DB6A RID: 56170
				public static LocString NAME = "Chemical Battery";

				// Token: 0x0400DB6B RID: 56171
				public static LocString TOOLTIP = "This bot will shut down permanently when its battery runs out";
			}

			// Token: 0x02003896 RID: 14486
			public class INTERNALBIOBATTERY
			{
				// Token: 0x0400DB6C RID: 56172
				public static LocString NAME = "Biofuel";

				// Token: 0x0400DB6D RID: 56173
				public static LocString TOOLTIP = "This bot will shut down permanently when its biofuel runs out";
			}

			// Token: 0x02003897 RID: 14487
			public class INTERNALELECTROBANK
			{
				// Token: 0x0400DB6E RID: 56174
				public static LocString NAME = "Power Bank";

				// Token: 0x0400DB6F RID: 56175
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"When this bot's ",
					UI.PRE_KEYWORD,
					"Power Bank",
					UI.PST_KEYWORD,
					" runs out, it will stop working until a fully charged one is delivered"
				});
			}
		}

		// Token: 0x02003898 RID: 14488
		public class ATTRIBUTES
		{
			// Token: 0x02003899 RID: 14489
			public class INTERNALBATTERYDELTA
			{
				// Token: 0x0400DB70 RID: 56176
				public static LocString NAME = "Rechargeable Battery Drain";

				// Token: 0x0400DB71 RID: 56177
				public static LocString TOOLTIP = "The rate at which battery life is depleted";
			}
		}

		// Token: 0x0200389A RID: 14490
		public class STATUSITEMS
		{
			// Token: 0x0200389B RID: 14491
			public class CANTREACHSTATION
			{
				// Token: 0x0400DB72 RID: 56178
				public static LocString NAME = "Unreachable Dock";

				// Token: 0x0400DB73 RID: 56179
				public static LocString DESC = "Obstacles are preventing {0} from heading home";

				// Token: 0x0400DB74 RID: 56180
				public static LocString TOOLTIP = "Obstacles are preventing {0} from heading home";
			}

			// Token: 0x0200389C RID: 14492
			public class MOVINGTOCHARGESTATION
			{
				// Token: 0x0400DB75 RID: 56181
				public static LocString NAME = "Traveling to Dock";

				// Token: 0x0400DB76 RID: 56182
				public static LocString DESC = "{0} is on its way home to recharge";

				// Token: 0x0400DB77 RID: 56183
				public static LocString TOOLTIP = "{0} is on its way home to recharge";
			}

			// Token: 0x0200389D RID: 14493
			public class LOWBATTERY
			{
				// Token: 0x0400DB78 RID: 56184
				public static LocString NAME = "Low Battery";

				// Token: 0x0400DB79 RID: 56185
				public static LocString DESC = "{0}'s battery is low and needs to recharge";

				// Token: 0x0400DB7A RID: 56186
				public static LocString TOOLTIP = "{0}'s battery is low and needs to recharge";
			}

			// Token: 0x0200389E RID: 14494
			public class LOWBATTERYNOCHARGE
			{
				// Token: 0x0400DB7B RID: 56187
				public static LocString NAME = "Low Battery";

				// Token: 0x0400DB7C RID: 56188
				public static LocString DESC = "{0}'s battery is low\n\nThe internal battery cannot be recharged and this robot will cease functioning after it is depleted.";

				// Token: 0x0400DB7D RID: 56189
				public static LocString TOOLTIP = "{0}'s battery is low\n\nThe internal battery cannot be recharged and this robot will cease functioning after it is depleted.";
			}

			// Token: 0x0200389F RID: 14495
			public class DEADBATTERY
			{
				// Token: 0x0400DB7E RID: 56190
				public static LocString NAME = "Shut Down";

				// Token: 0x0400DB7F RID: 56191
				public static LocString DESC = "RIP {0}\n\n{0}'s battery has been depleted and cannot be recharged";

				// Token: 0x0400DB80 RID: 56192
				public static LocString TOOLTIP = "RIP {0}\n\n{0}'s battery has been depleted and cannot be recharged";
			}

			// Token: 0x020038A0 RID: 14496
			public class DEADBATTERYFLYDO
			{
				// Token: 0x0400DB81 RID: 56193
				public static LocString NAME = "Shut Down";

				// Token: 0x0400DB82 RID: 56194
				public static LocString DESC = "{0}'s battery has been depleted\n\n{0} will resume function when a new battery has been delivered";

				// Token: 0x0400DB83 RID: 56195
				public static LocString TOOLTIP = "{0}'s battery has been depleted\n\n{0} will resume function when a new battery has been delivered";
			}

			// Token: 0x020038A1 RID: 14497
			public class DUSTBINFULL
			{
				// Token: 0x0400DB84 RID: 56196
				public static LocString NAME = "Dust Bin Full";

				// Token: 0x0400DB85 RID: 56197
				public static LocString DESC = "{0} must return to its dock to unload";

				// Token: 0x0400DB86 RID: 56198
				public static LocString TOOLTIP = "{0} must return to its dock to unload";
			}

			// Token: 0x020038A2 RID: 14498
			public class WORKING
			{
				// Token: 0x0400DB87 RID: 56199
				public static LocString NAME = "Working";

				// Token: 0x0400DB88 RID: 56200
				public static LocString DESC = "{0} is working diligently. Great job, {0}!";

				// Token: 0x0400DB89 RID: 56201
				public static LocString TOOLTIP = "{0} is working diligently. Great job, {0}!";
			}

			// Token: 0x020038A3 RID: 14499
			public class UNLOADINGSTORAGE
			{
				// Token: 0x0400DB8A RID: 56202
				public static LocString NAME = "Unloading";

				// Token: 0x0400DB8B RID: 56203
				public static LocString DESC = "{0} is emptying out its dust bin";

				// Token: 0x0400DB8C RID: 56204
				public static LocString TOOLTIP = "{0} is emptying out its dust bin";
			}

			// Token: 0x020038A4 RID: 14500
			public class CHARGING
			{
				// Token: 0x0400DB8D RID: 56205
				public static LocString NAME = "Charging";

				// Token: 0x0400DB8E RID: 56206
				public static LocString DESC = "{0} is recharging its battery";

				// Token: 0x0400DB8F RID: 56207
				public static LocString TOOLTIP = "{0} is recharging its battery";
			}

			// Token: 0x020038A5 RID: 14501
			public class REACTPOSITIVE
			{
				// Token: 0x0400DB90 RID: 56208
				public static LocString NAME = "Happy Reaction";

				// Token: 0x0400DB91 RID: 56209
				public static LocString DESC = "This bot saw something nice!";

				// Token: 0x0400DB92 RID: 56210
				public static LocString TOOLTIP = "This bot saw something nice!";
			}

			// Token: 0x020038A6 RID: 14502
			public class REACTNEGATIVE
			{
				// Token: 0x0400DB93 RID: 56211
				public static LocString NAME = "Bothered Reaction";

				// Token: 0x0400DB94 RID: 56212
				public static LocString DESC = "This bot saw something upsetting";

				// Token: 0x0400DB95 RID: 56213
				public static LocString TOOLTIP = "This bot saw something upsetting";
			}
		}

		// Token: 0x020038A7 RID: 14503
		public class MODELS
		{
			// Token: 0x020038A8 RID: 14504
			public class MORB
			{
				// Token: 0x0400DB96 RID: 56214
				public static LocString NAME = UI.FormatAsLink("Biobot", "STORYTRAITMORBROVER");

				// Token: 0x0400DB97 RID: 56215
				public static LocString DESC = "A Pathogen-Fueled Extravehicular Geo-Exploratory Guidebot (model Y), aka \"P.E.G.G.Y.\"\n\nIt can be assigned basic building tasks and digging duties in hazardous environments.";

				// Token: 0x0400DB98 RID: 56216
				public static LocString CODEX_DESC = "The pathogen-fueled guidebot is designed to maximize a colony's chances of surviving in hostile environments by meeting three core outcomes:\n\n1. Filtration and removal of toxins from environment;\n2. Safe disposal of filtered toxins through conversion into usable biofuel;\n3. Creation of geo-exploration equipment for colony expansion with minimal colonist endangerment.\n\nThe elements aggregated during this process may result in the unintentional spread of contaminants. Specialized training required for safe handling.";
			}

			// Token: 0x020038A9 RID: 14505
			public class SCOUT
			{
				// Token: 0x0400DB99 RID: 56217
				public static LocString NAME = "Rover";

				// Token: 0x0400DB9A RID: 56218
				public static LocString DESC = "A curious bot that can remotely explore new " + UI.CLUSTERMAP.PLANETOID_KEYWORD + " locations.";
			}

			// Token: 0x020038AA RID: 14506
			public class SWEEPBOT
			{
				// Token: 0x0400DB9B RID: 56219
				public static LocString NAME = "Sweepy";

				// Token: 0x0400DB9C RID: 56220
				public static LocString DESC = string.Concat(new string[]
				{
					"An automated sweeping robot.\n\nSweeps up ",
					UI.FormatAsLink("Solid", "ELEMENTS_SOLID"),
					" debris and ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" spills and stores the material back in its ",
					UI.FormatAsLink("Sweepy Dock", "SWEEPBOTSTATION"),
					"."
				});
			}

			// Token: 0x020038AB RID: 14507
			public class FLYDO
			{
				// Token: 0x0400DB9D RID: 56221
				public static LocString NAME = "Flydo";

				// Token: 0x0400DB9E RID: 56222
				public static LocString DESC = "A programmable delivery robot.\n\nPicks up " + UI.FormatAsLink("Solid", "ELEMENTS_SOLID") + " objects for delivery to selected destinations.";
			}
		}
	}
}
