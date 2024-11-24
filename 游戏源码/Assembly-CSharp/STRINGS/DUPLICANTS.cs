using System;
using TUNING;

namespace STRINGS
{
	// Token: 0x020033F6 RID: 13302
	public class DUPLICANTS
	{
		// Token: 0x0400CFB9 RID: 53177
		public static LocString RACE_PREFIX = "Species: {0}";

		// Token: 0x0400CFBA RID: 53178
		public static LocString RACE = "Duplicant";

		// Token: 0x0400CFBB RID: 53179
		public static LocString MODELTITLE = "Species: ";

		// Token: 0x0400CFBC RID: 53180
		public static LocString NAMETITLE = "Name: ";

		// Token: 0x0400CFBD RID: 53181
		public static LocString GENDERTITLE = "Gender: ";

		// Token: 0x0400CFBE RID: 53182
		public static LocString ARRIVALTIME = "Age: ";

		// Token: 0x0400CFBF RID: 53183
		public static LocString ARRIVALTIME_TOOLTIP = "This {1} was printed on <b>Cycle {0}</b>";

		// Token: 0x0400CFC0 RID: 53184
		public static LocString DESC_TOOLTIP = "About {0}s";

		// Token: 0x020033F7 RID: 13303
		public class MODEL
		{
			// Token: 0x020033F8 RID: 13304
			public class STANDARD
			{
				// Token: 0x0400CFC1 RID: 53185
				public static LocString NAME = "Standard Duplicant";
			}

			// Token: 0x020033F9 RID: 13305
			public class BIONIC
			{
				// Token: 0x0400CFC2 RID: 53186
				public static LocString NAME = "Bionic Duplicant";

				// Token: 0x0400CFC3 RID: 53187
				public static LocString NAME_TOOLTIP = "This Duplicant is a curious combination of organic and inorganic parts";
			}

			// Token: 0x020033FA RID: 13306
			public class REMOTEWORKER
			{
				// Token: 0x0400CFC4 RID: 53188
				public static LocString NAME = "Remote Worker";

				// Token: 0x0400CFC5 RID: 53189
				public static LocString DESC = "A remotely operated work robot.\n\nIt performs chores as instructed by a " + UI.FormatAsLink("Remote Controller", "REMOTEWORKTERMINAL") + " on the same planetoid.";
			}
		}

		// Token: 0x020033FB RID: 13307
		public class GENDER
		{
			// Token: 0x020033FC RID: 13308
			public class MALE
			{
				// Token: 0x0400CFC6 RID: 53190
				public static LocString NAME = "M";

				// Token: 0x020033FD RID: 13309
				public class PLURALS
				{
					// Token: 0x0400CFC7 RID: 53191
					public static LocString ONE = "he";

					// Token: 0x0400CFC8 RID: 53192
					public static LocString TWO = "his";
				}
			}

			// Token: 0x020033FE RID: 13310
			public class FEMALE
			{
				// Token: 0x0400CFC9 RID: 53193
				public static LocString NAME = "F";

				// Token: 0x020033FF RID: 13311
				public class PLURALS
				{
					// Token: 0x0400CFCA RID: 53194
					public static LocString ONE = "she";

					// Token: 0x0400CFCB RID: 53195
					public static LocString TWO = "her";
				}
			}

			// Token: 0x02003400 RID: 13312
			public class NB
			{
				// Token: 0x0400CFCC RID: 53196
				public static LocString NAME = "X";

				// Token: 0x02003401 RID: 13313
				public class PLURALS
				{
					// Token: 0x0400CFCD RID: 53197
					public static LocString ONE = "they";

					// Token: 0x0400CFCE RID: 53198
					public static LocString TWO = "their";
				}
			}
		}

		// Token: 0x02003402 RID: 13314
		public class STATS
		{
			// Token: 0x02003403 RID: 13315
			public class SUBJECTS
			{
				// Token: 0x0400CFCF RID: 53199
				public static LocString DUPLICANT = "Duplicant";

				// Token: 0x0400CFD0 RID: 53200
				public static LocString DUPLICANT_POSSESSIVE = "Duplicant's";

				// Token: 0x0400CFD1 RID: 53201
				public static LocString DUPLICANT_PLURAL = "Duplicants";

				// Token: 0x0400CFD2 RID: 53202
				public static LocString CREATURE = "critter";

				// Token: 0x0400CFD3 RID: 53203
				public static LocString CREATURE_POSSESSIVE = "critter's";

				// Token: 0x0400CFD4 RID: 53204
				public static LocString CREATURE_PLURAL = "critters";

				// Token: 0x0400CFD5 RID: 53205
				public static LocString PLANT = "plant";

				// Token: 0x0400CFD6 RID: 53206
				public static LocString PLANT_POSESSIVE = "plant's";

				// Token: 0x0400CFD7 RID: 53207
				public static LocString PLANT_PLURAL = "plants";
			}

			// Token: 0x02003404 RID: 13316
			public class BIONICINTERNALBATTERY
			{
				// Token: 0x0400CFD8 RID: 53208
				public static LocString NAME = "Power Banks";

				// Token: 0x0400CFD9 RID: 53209
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"A Bionic Duplicant with zero remaining ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" will become incapacitated until replacement ",
					UI.PRE_KEYWORD,
					"Power Banks",
					UI.PST_KEYWORD,
					" are installed"
				});
			}

			// Token: 0x02003405 RID: 13317
			public class BIONICOXYGENTANK
			{
				// Token: 0x0400CFDA RID: 53210
				public static LocString NAME = "Oxygen Tank";

				// Token: 0x0400CFDB RID: 53211
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Bionic Duplicants have internal ",
					UI.PRE_KEYWORD,
					"Oxygen",
					UI.PST_KEYWORD,
					" tanks that enable them to work in low breathability areas"
				});
			}

			// Token: 0x02003406 RID: 13318
			public class BIONICOIL
			{
				// Token: 0x0400CFDC RID: 53212
				public static LocString NAME = "Oil";

				// Token: 0x0400CFDD RID: 53213
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Bionic Duplicants will slow down significantly when ",
					UI.PRE_KEYWORD,
					"Oil",
					UI.PST_KEYWORD,
					" levels reach zero\n\nDuplicants can obtain ",
					UI.PRE_KEYWORD,
					"Phyto Oil",
					UI.PST_KEYWORD,
					" or ",
					UI.PRE_KEYWORD,
					"Crude Oil",
					UI.PST_KEYWORD,
					" at the ",
					UI.PRE_KEYWORD,
					"Lubrication Station",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x02003407 RID: 13319
			public class BIONICGUNK
			{
				// Token: 0x0400CFDE RID: 53214
				public static LocString NAME = "Gunk";

				// Token: 0x0400CFDF RID: 53215
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Bionic Duplicants become ",
					UI.PRE_KEYWORD,
					"Stressed",
					UI.PST_KEYWORD,
					" when too much ",
					UI.PRE_KEYWORD,
					"Liquid Gunk",
					UI.PST_KEYWORD,
					" builds up in their bionic parts\n\nRegular visits to the ",
					UI.PRE_KEYWORD,
					"Gunk Extractor",
					UI.PST_KEYWORD,
					" are required"
				});
			}

			// Token: 0x02003408 RID: 13320
			public class BREATH
			{
				// Token: 0x0400CFE0 RID: 53216
				public static LocString NAME = "Breath";

				// Token: 0x0400CFE1 RID: 53217
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"A Duplicant with zero remaining ",
					UI.PRE_KEYWORD,
					"Breath",
					UI.PST_KEYWORD,
					" will begin suffocating"
				});
			}

			// Token: 0x02003409 RID: 13321
			public class STAMINA
			{
				// Token: 0x0400CFE2 RID: 53218
				public static LocString NAME = "Stamina";

				// Token: 0x0400CFE3 RID: 53219
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Duplicants will pass out from fatigue when ",
					UI.PRE_KEYWORD,
					"Stamina",
					UI.PST_KEYWORD,
					" reaches zero"
				});
			}

			// Token: 0x0200340A RID: 13322
			public class CALORIES
			{
				// Token: 0x0400CFE4 RID: 53220
				public static LocString NAME = "Calories";

				// Token: 0x0400CFE5 RID: 53221
				public static LocString TOOLTIP = "This {1} can burn <b>{0}</b> before starving";
			}

			// Token: 0x0200340B RID: 13323
			public class TEMPERATURE
			{
				// Token: 0x0400CFE6 RID: 53222
				public static LocString NAME = "Body Temperature";

				// Token: 0x0400CFE7 RID: 53223
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"A healthy Duplicant's ",
					UI.PRE_KEYWORD,
					"Body Temperature",
					UI.PST_KEYWORD,
					" is <b>{1}</b>"
				});

				// Token: 0x0400CFE8 RID: 53224
				public static LocString TOOLTIP_DOMESTICATEDCRITTER = string.Concat(new string[]
				{
					"This critter's ",
					UI.PRE_KEYWORD,
					"Body Temperature",
					UI.PST_KEYWORD,
					" is <b>{1}</b>"
				});
			}

			// Token: 0x0200340C RID: 13324
			public class EXTERNALTEMPERATURE
			{
				// Token: 0x0400CFE9 RID: 53225
				public static LocString NAME = "External Temperature";

				// Token: 0x0400CFEA RID: 53226
				public static LocString TOOLTIP = "This Duplicant's environment is <b>{0}</b>";
			}

			// Token: 0x0200340D RID: 13325
			public class DECOR
			{
				// Token: 0x0400CFEB RID: 53227
				public static LocString NAME = "Decor";

				// Token: 0x0400CFEC RID: 53228
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Duplicants become stressed in areas with ",
					UI.PRE_KEYWORD,
					"Decor",
					UI.PST_KEYWORD,
					" lower than their expectations\n\nOpen the ",
					UI.FormatAsOverlay("Decor Overlay", global::Action.Overlay8),
					" to view current ",
					UI.PRE_KEYWORD,
					"Decor",
					UI.PST_KEYWORD,
					" values"
				});

				// Token: 0x0400CFED RID: 53229
				public static LocString TOOLTIP_CURRENT = "\n\nCurrent Environmental Decor: <b>{0}</b>";

				// Token: 0x0400CFEE RID: 53230
				public static LocString TOOLTIP_AVERAGE_TODAY = "\nAverage Decor This Cycle: <b>{0}</b>";

				// Token: 0x0400CFEF RID: 53231
				public static LocString TOOLTIP_AVERAGE_YESTERDAY = "\nAverage Decor Last Cycle: <b>{0}</b>";
			}

			// Token: 0x0200340E RID: 13326
			public class STRESS
			{
				// Token: 0x0400CFF0 RID: 53232
				public static LocString NAME = "Stress";

				// Token: 0x0400CFF1 RID: 53233
				public static LocString TOOLTIP = "Duplicants exhibit their Stress Reactions at one hundred percent " + UI.PRE_KEYWORD + "Stress" + UI.PST_KEYWORD;
			}

			// Token: 0x0200340F RID: 13327
			public class RADIATIONBALANCE
			{
				// Token: 0x0400CFF2 RID: 53234
				public static LocString NAME = "Absorbed Rad Dose";

				// Token: 0x0400CFF3 RID: 53235
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Duplicants accumulate Rads in areas with ",
					UI.PRE_KEYWORD,
					"Radiation",
					UI.PST_KEYWORD,
					" and recover when using the toilet\n\nOpen the ",
					UI.FormatAsOverlay("Radiation Overlay", global::Action.Overlay15),
					" to view current ",
					UI.PRE_KEYWORD,
					"Rad",
					UI.PST_KEYWORD,
					" readings"
				});

				// Token: 0x0400CFF4 RID: 53236
				public static LocString TOOLTIP_CURRENT_BALANCE = string.Concat(new string[]
				{
					"Duplicants accumulate Rads in areas with ",
					UI.PRE_KEYWORD,
					"Radiation",
					UI.PST_KEYWORD,
					" and recover when using the toilet\n\nOpen the ",
					UI.FormatAsOverlay("Radiation Overlay", global::Action.Overlay15),
					" to view current ",
					UI.PRE_KEYWORD,
					"Rad",
					UI.PST_KEYWORD,
					" readings"
				});

				// Token: 0x0400CFF5 RID: 53237
				public static LocString CURRENT_EXPOSURE = "Current Exposure: {0}/cycle";

				// Token: 0x0400CFF6 RID: 53238
				public static LocString CURRENT_REJUVENATION = "Current Rejuvenation: {0}/cycle";
			}

			// Token: 0x02003410 RID: 13328
			public class BLADDER
			{
				// Token: 0x0400CFF7 RID: 53239
				public static LocString NAME = "Bladder";

				// Token: 0x0400CFF8 RID: 53240
				public static LocString TOOLTIP = "Duplicants make \"messes\" if no toilets are available at one hundred percent " + UI.PRE_KEYWORD + "Bladder" + UI.PST_KEYWORD;
			}

			// Token: 0x02003411 RID: 13329
			public class HITPOINTS
			{
				// Token: 0x0400CFF9 RID: 53241
				public static LocString NAME = "Health";

				// Token: 0x0400CFFA RID: 53242
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"When Duplicants reach zero ",
					UI.PRE_KEYWORD,
					"Health",
					UI.PST_KEYWORD,
					" they become incapacitated and require rescuing\n\nWhen critters reach zero ",
					UI.PRE_KEYWORD,
					"Health",
					UI.PST_KEYWORD,
					", they will die immediately"
				});
			}

			// Token: 0x02003412 RID: 13330
			public class SKIN_THICKNESS
			{
				// Token: 0x0400CFFB RID: 53243
				public static LocString NAME = "Skin Thickness";
			}

			// Token: 0x02003413 RID: 13331
			public class SKIN_DURABILITY
			{
				// Token: 0x0400CFFC RID: 53244
				public static LocString NAME = "Skin Durability";
			}

			// Token: 0x02003414 RID: 13332
			public class DISEASERECOVERYTIME
			{
				// Token: 0x0400CFFD RID: 53245
				public static LocString NAME = "Disease Recovery";
			}

			// Token: 0x02003415 RID: 13333
			public class TRUNKHEALTH
			{
				// Token: 0x0400CFFE RID: 53246
				public static LocString NAME = "Trunk Health";

				// Token: 0x0400CFFF RID: 53247
				public static LocString TOOLTIP = "Tree branches will die if they do not have a healthy trunk to grow from";
			}
		}

		// Token: 0x02003416 RID: 13334
		public class DEATHS
		{
			// Token: 0x02003417 RID: 13335
			public class GENERIC
			{
				// Token: 0x0400D000 RID: 53248
				public static LocString NAME = "Death";

				// Token: 0x0400D001 RID: 53249
				public static LocString DESCRIPTION = "{Target} has died.";
			}

			// Token: 0x02003418 RID: 13336
			public class FROZEN
			{
				// Token: 0x0400D002 RID: 53250
				public static LocString NAME = "Frozen";

				// Token: 0x0400D003 RID: 53251
				public static LocString DESCRIPTION = "{Target} has frozen to death.";
			}

			// Token: 0x02003419 RID: 13337
			public class SUFFOCATION
			{
				// Token: 0x0400D004 RID: 53252
				public static LocString NAME = "Suffocation";

				// Token: 0x0400D005 RID: 53253
				public static LocString DESCRIPTION = "{Target} has suffocated to death.";
			}

			// Token: 0x0200341A RID: 13338
			public class STARVATION
			{
				// Token: 0x0400D006 RID: 53254
				public static LocString NAME = "Starvation";

				// Token: 0x0400D007 RID: 53255
				public static LocString DESCRIPTION = "{Target} has starved to death.";
			}

			// Token: 0x0200341B RID: 13339
			public class OVERHEATING
			{
				// Token: 0x0400D008 RID: 53256
				public static LocString NAME = "Overheated";

				// Token: 0x0400D009 RID: 53257
				public static LocString DESCRIPTION = "{Target} overheated to death.";
			}

			// Token: 0x0200341C RID: 13340
			public class DROWNED
			{
				// Token: 0x0400D00A RID: 53258
				public static LocString NAME = "Drowned";

				// Token: 0x0400D00B RID: 53259
				public static LocString DESCRIPTION = "{Target} has drowned.";
			}

			// Token: 0x0200341D RID: 13341
			public class EXPLOSION
			{
				// Token: 0x0400D00C RID: 53260
				public static LocString NAME = "Explosion";

				// Token: 0x0400D00D RID: 53261
				public static LocString DESCRIPTION = "{Target} has died in an explosion.";
			}

			// Token: 0x0200341E RID: 13342
			public class COMBAT
			{
				// Token: 0x0400D00E RID: 53262
				public static LocString NAME = "Slain";

				// Token: 0x0400D00F RID: 53263
				public static LocString DESCRIPTION = "{Target} succumbed to their wounds after being incapacitated.";
			}

			// Token: 0x0200341F RID: 13343
			public class FATALDISEASE
			{
				// Token: 0x0400D010 RID: 53264
				public static LocString NAME = "Succumbed to Disease";

				// Token: 0x0400D011 RID: 53265
				public static LocString DESCRIPTION = "{Target} has died of a fatal illness.";
			}

			// Token: 0x02003420 RID: 13344
			public class RADIATION
			{
				// Token: 0x0400D012 RID: 53266
				public static LocString NAME = "Irradiated";

				// Token: 0x0400D013 RID: 53267
				public static LocString DESCRIPTION = "{Target} perished from excessive radiation exposure.";
			}

			// Token: 0x02003421 RID: 13345
			public class HITBYHIGHENERGYPARTICLE
			{
				// Token: 0x0400D014 RID: 53268
				public static LocString NAME = "Struck by Radbolt";

				// Token: 0x0400D015 RID: 53269
				public static LocString DESCRIPTION = string.Concat(new string[]
				{
					"{Target} was struck by a radioactive ",
					UI.PRE_KEYWORD,
					"Radbolt",
					UI.PST_KEYWORD,
					" and perished."
				});
			}
		}

		// Token: 0x02003422 RID: 13346
		public class CHORES
		{
			// Token: 0x0400D016 RID: 53270
			public static LocString NOT_EXISTING_TASK = "Not Existing";

			// Token: 0x0400D017 RID: 53271
			public static LocString IS_DEAD_TASK = "Dead";

			// Token: 0x02003423 RID: 13347
			public class THINKING
			{
				// Token: 0x0400D018 RID: 53272
				public static LocString NAME = "Ponder";

				// Token: 0x0400D019 RID: 53273
				public static LocString STATUS = "Pondering";

				// Token: 0x0400D01A RID: 53274
				public static LocString TOOLTIP = "This Duplicant is mulling over what they should do next";
			}

			// Token: 0x02003424 RID: 13348
			public class ASTRONAUT
			{
				// Token: 0x0400D01B RID: 53275
				public static LocString NAME = "Space Mission";

				// Token: 0x0400D01C RID: 53276
				public static LocString STATUS = "On space mission";

				// Token: 0x0400D01D RID: 53277
				public static LocString TOOLTIP = "This Duplicant is exploring the vast universe";
			}

			// Token: 0x02003425 RID: 13349
			public class DIE
			{
				// Token: 0x0400D01E RID: 53278
				public static LocString NAME = "Die";

				// Token: 0x0400D01F RID: 53279
				public static LocString STATUS = "Dying";

				// Token: 0x0400D020 RID: 53280
				public static LocString TOOLTIP = "Fare thee well, brave soul";
			}

			// Token: 0x02003426 RID: 13350
			public class ENTOMBED
			{
				// Token: 0x0400D021 RID: 53281
				public static LocString NAME = "Entombed";

				// Token: 0x0400D022 RID: 53282
				public static LocString STATUS = "Entombed";

				// Token: 0x0400D023 RID: 53283
				public static LocString TOOLTIP = "Entombed Duplicants are at risk of suffocating and must be dug out by others in the colony";
			}

			// Token: 0x02003427 RID: 13351
			public class BEINCAPACITATED
			{
				// Token: 0x0400D024 RID: 53284
				public static LocString NAME = "Incapacitated";

				// Token: 0x0400D025 RID: 53285
				public static LocString STATUS = "Dying";

				// Token: 0x0400D026 RID: 53286
				public static LocString TOOLTIP = "This Duplicant will die soon if they do not receive assistance";
			}

			// Token: 0x02003428 RID: 13352
			public class BEOFFLINE
			{
				// Token: 0x0400D027 RID: 53287
				public static LocString NAME = "Powerless";

				// Token: 0x0400D028 RID: 53288
				public static LocString STATUS = "Powerless";

				// Token: 0x0400D029 RID: 53289
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant does not have enough ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" to function"
				});
			}

			// Token: 0x02003429 RID: 13353
			public class BEBATTERYSAVEMODE
			{
				// Token: 0x0400D02A RID: 53290
				public static LocString NAME = "Standby Mode";

				// Token: 0x0400D02B RID: 53291
				public static LocString STATUS = "Standby Mode";

				// Token: 0x0400D02C RID: 53292
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is resting\n\nTheir ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" consumption is at a minimum"
				});
			}

			// Token: 0x0200342A RID: 13354
			public class GENESHUFFLE
			{
				// Token: 0x0400D02D RID: 53293
				public static LocString NAME = "Use Neural Vacillator";

				// Token: 0x0400D02E RID: 53294
				public static LocString STATUS = "Using Neural Vacillator";

				// Token: 0x0400D02F RID: 53295
				public static LocString TOOLTIP = "This Duplicant is being experimented on!";
			}

			// Token: 0x0200342B RID: 13355
			public class MIGRATE
			{
				// Token: 0x0400D030 RID: 53296
				public static LocString NAME = "Use Teleporter";

				// Token: 0x0400D031 RID: 53297
				public static LocString STATUS = "Using Teleporter";

				// Token: 0x0400D032 RID: 53298
				public static LocString TOOLTIP = "This Duplicant's molecules are hurtling through the air!";
			}

			// Token: 0x0200342C RID: 13356
			public class DEBUGGOTO
			{
				// Token: 0x0400D033 RID: 53299
				public static LocString NAME = "DebugGoTo";

				// Token: 0x0400D034 RID: 53300
				public static LocString STATUS = "DebugGoTo";
			}

			// Token: 0x0200342D RID: 13357
			public class DISINFECT
			{
				// Token: 0x0400D035 RID: 53301
				public static LocString NAME = "Disinfect";

				// Token: 0x0400D036 RID: 53302
				public static LocString STATUS = "Going to disinfect";

				// Token: 0x0400D037 RID: 53303
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Buildings can be disinfected to remove contagious ",
					UI.PRE_KEYWORD,
					"Germs",
					UI.PST_KEYWORD,
					" from their surface"
				});
			}

			// Token: 0x0200342E RID: 13358
			public class EQUIPPINGSUIT
			{
				// Token: 0x0400D038 RID: 53304
				public static LocString NAME = "Equip Exosuit";

				// Token: 0x0400D039 RID: 53305
				public static LocString STATUS = "Equipping exosuit";

				// Token: 0x0400D03A RID: 53306
				public static LocString TOOLTIP = "This Duplicant is putting on protective gear";
			}

			// Token: 0x0200342F RID: 13359
			public class STRESSIDLE
			{
				// Token: 0x0400D03B RID: 53307
				public static LocString NAME = "Antsy";

				// Token: 0x0400D03C RID: 53308
				public static LocString STATUS = "Antsy";

				// Token: 0x0400D03D RID: 53309
				public static LocString TOOLTIP = "This Duplicant is a workaholic and gets stressed when they have nothing to do";
			}

			// Token: 0x02003430 RID: 13360
			public class MOVETO
			{
				// Token: 0x0400D03E RID: 53310
				public static LocString NAME = "Move to";

				// Token: 0x0400D03F RID: 53311
				public static LocString STATUS = "Moving to location";

				// Token: 0x0400D040 RID: 53312
				public static LocString TOOLTIP = "This Duplicant was manually directed to move to a specific location";
			}

			// Token: 0x02003431 RID: 13361
			public class ROCKETENTEREXIT
			{
				// Token: 0x0400D041 RID: 53313
				public static LocString NAME = "Rocket Recrewing";

				// Token: 0x0400D042 RID: 53314
				public static LocString STATUS = "Recrewing Rocket";

				// Token: 0x0400D043 RID: 53315
				public static LocString TOOLTIP = "This Duplicant is getting into (or out of) their assigned rocket";
			}

			// Token: 0x02003432 RID: 13362
			public class DROPUNUSEDINVENTORY
			{
				// Token: 0x0400D044 RID: 53316
				public static LocString NAME = "Drop Inventory";

				// Token: 0x0400D045 RID: 53317
				public static LocString STATUS = "Dropping unused inventory";

				// Token: 0x0400D046 RID: 53318
				public static LocString TOOLTIP = "This Duplicant is dropping carried items they no longer need";
			}

			// Token: 0x02003433 RID: 13363
			public class PEE
			{
				// Token: 0x0400D047 RID: 53319
				public static LocString NAME = "Relieve Self";

				// Token: 0x0400D048 RID: 53320
				public static LocString STATUS = "Relieving self";

				// Token: 0x0400D049 RID: 53321
				public static LocString TOOLTIP = "This Duplicant didn't find a toilet in time. Oops";
			}

			// Token: 0x02003434 RID: 13364
			public class EXPELLGUNK
			{
				// Token: 0x0400D04A RID: 53322
				public static LocString NAME = "Expel Liquid Gunk";

				// Token: 0x0400D04B RID: 53323
				public static LocString STATUS = "Expelling liquid gunk";

				// Token: 0x0400D04C RID: 53324
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant didn't get to a ",
					UI.PRE_KEYWORD,
					"Gunk Extractor",
					UI.PST_KEYWORD,
					" in time. Urgh"
				});
			}

			// Token: 0x02003435 RID: 13365
			public class OILCHANGE
			{
				// Token: 0x0400D04D RID: 53325
				public static LocString NAME = "Refill Oil";

				// Token: 0x0400D04E RID: 53326
				public static LocString STATUS = "Refilling oil";

				// Token: 0x0400D04F RID: 53327
				public static LocString TOOLTIP = "This Duplicant is making sure their internal mechanisms stay lubricated";
			}

			// Token: 0x02003436 RID: 13366
			public class BREAK_PEE
			{
				// Token: 0x0400D050 RID: 53328
				public static LocString NAME = "Downtime: Use Toilet";

				// Token: 0x0400D051 RID: 53329
				public static LocString STATUS = "Downtime: Going to use toilet";

				// Token: 0x0400D052 RID: 53330
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant has scheduled ",
					UI.PRE_KEYWORD,
					"Downtime",
					UI.PST_KEYWORD,
					" and is using their break to go to the toilet\n\nDuplicants have to use the toilet at least once per day"
				});
			}

			// Token: 0x02003437 RID: 13367
			public class STRESSVOMIT
			{
				// Token: 0x0400D053 RID: 53331
				public static LocString NAME = "Stress Vomit";

				// Token: 0x0400D054 RID: 53332
				public static LocString STATUS = "Stress vomiting";

				// Token: 0x0400D055 RID: 53333
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Some people deal with ",
					UI.PRE_KEYWORD,
					"Stress",
					UI.PST_KEYWORD,
					" better than others"
				});
			}

			// Token: 0x02003438 RID: 13368
			public class UGLY_CRY
			{
				// Token: 0x0400D056 RID: 53334
				public static LocString NAME = "Ugly Cry";

				// Token: 0x0400D057 RID: 53335
				public static LocString STATUS = "Ugly crying";

				// Token: 0x0400D058 RID: 53336
				public static LocString TOOLTIP = "This Duplicant is having a healthy cry to alleviate their " + UI.PRE_KEYWORD + "Stress" + UI.PST_KEYWORD;
			}

			// Token: 0x02003439 RID: 13369
			public class STRESSSHOCK
			{
				// Token: 0x0400D059 RID: 53337
				public static LocString NAME = "Shock";

				// Token: 0x0400D05A RID: 53338
				public static LocString STATUS = "Shocking";

				// Token: 0x0400D05B RID: 53339
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant's inability to handle ",
					UI.PRE_KEYWORD,
					"Stress",
					UI.PST_KEYWORD,
					" is pretty shocking"
				});
			}

			// Token: 0x0200343A RID: 13370
			public class BINGE_EAT
			{
				// Token: 0x0400D05C RID: 53340
				public static LocString NAME = "Binge Eat";

				// Token: 0x0400D05D RID: 53341
				public static LocString STATUS = "Binge eating";

				// Token: 0x0400D05E RID: 53342
				public static LocString TOOLTIP = "This Duplicant is attempting to eat their emotions due to " + UI.PRE_KEYWORD + "Stress" + UI.PST_KEYWORD;
			}

			// Token: 0x0200343B RID: 13371
			public class BANSHEE_WAIL
			{
				// Token: 0x0400D05F RID: 53343
				public static LocString NAME = "Banshee Wail";

				// Token: 0x0400D060 RID: 53344
				public static LocString STATUS = "Wailing";

				// Token: 0x0400D061 RID: 53345
				public static LocString TOOLTIP = "This Duplicant is emitting ear-piercing shrieks to relieve pent-up " + UI.PRE_KEYWORD + "Stress" + UI.PST_KEYWORD;
			}

			// Token: 0x0200343C RID: 13372
			public class EMOTEHIGHPRIORITY
			{
				// Token: 0x0400D062 RID: 53346
				public static LocString NAME = "Express Themselves";

				// Token: 0x0400D063 RID: 53347
				public static LocString STATUS = "Expressing themselves";

				// Token: 0x0400D064 RID: 53348
				public static LocString TOOLTIP = "This Duplicant needs a moment to express their feelings, then they'll be on their way";
			}

			// Token: 0x0200343D RID: 13373
			public class HUG
			{
				// Token: 0x0400D065 RID: 53349
				public static LocString NAME = "Hug";

				// Token: 0x0400D066 RID: 53350
				public static LocString STATUS = "Hugging";

				// Token: 0x0400D067 RID: 53351
				public static LocString TOOLTIP = "This Duplicant is enjoying a big warm hug";
			}

			// Token: 0x0200343E RID: 13374
			public class FLEE
			{
				// Token: 0x0400D068 RID: 53352
				public static LocString NAME = "Flee";

				// Token: 0x0400D069 RID: 53353
				public static LocString STATUS = "Fleeing";

				// Token: 0x0400D06A RID: 53354
				public static LocString TOOLTIP = "Run away!";
			}

			// Token: 0x0200343F RID: 13375
			public class RECOVERBREATH
			{
				// Token: 0x0400D06B RID: 53355
				public static LocString NAME = "Recover Breath";

				// Token: 0x0400D06C RID: 53356
				public static LocString STATUS = "Recovering breath";

				// Token: 0x0400D06D RID: 53357
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02003440 RID: 13376
			public class RECOVERFROMHEAT
			{
				// Token: 0x0400D06E RID: 53358
				public static LocString NAME = "Recover from Heat";

				// Token: 0x0400D06F RID: 53359
				public static LocString STATUS = "Recovering from heat";

				// Token: 0x0400D070 RID: 53360
				public static LocString TOOLTIP = "This Duplicant's trying to cool down";
			}

			// Token: 0x02003441 RID: 13377
			public class RECOVERWARMTH
			{
				// Token: 0x0400D071 RID: 53361
				public static LocString NAME = "Recover from Cold";

				// Token: 0x0400D072 RID: 53362
				public static LocString STATUS = "Recovering from cold";

				// Token: 0x0400D073 RID: 53363
				public static LocString TOOLTIP = "This Duplicant's trying to warm up";
			}

			// Token: 0x02003442 RID: 13378
			public class MOVETOQUARANTINE
			{
				// Token: 0x0400D074 RID: 53364
				public static LocString NAME = "Move to Quarantine";

				// Token: 0x0400D075 RID: 53365
				public static LocString STATUS = "Moving to quarantine";

				// Token: 0x0400D076 RID: 53366
				public static LocString TOOLTIP = "This Duplicant will isolate themselves to keep their illness away from the colony";
			}

			// Token: 0x02003443 RID: 13379
			public class ATTACK
			{
				// Token: 0x0400D077 RID: 53367
				public static LocString NAME = "Attack";

				// Token: 0x0400D078 RID: 53368
				public static LocString STATUS = "Attacking";

				// Token: 0x0400D079 RID: 53369
				public static LocString TOOLTIP = "Chaaaarge!";
			}

			// Token: 0x02003444 RID: 13380
			public class CAPTURE
			{
				// Token: 0x0400D07A RID: 53370
				public static LocString NAME = "Wrangle";

				// Token: 0x0400D07B RID: 53371
				public static LocString STATUS = "Wrangling";

				// Token: 0x0400D07C RID: 53372
				public static LocString TOOLTIP = "Duplicants that possess the Critter Ranching Skill can wrangle most critters without traps";
			}

			// Token: 0x02003445 RID: 13381
			public class SINGTOEGG
			{
				// Token: 0x0400D07D RID: 53373
				public static LocString NAME = "Sing To Egg";

				// Token: 0x0400D07E RID: 53374
				public static LocString STATUS = "Singing to egg";

				// Token: 0x0400D07F RID: 53375
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"A gentle lullaby from a supportive Duplicant encourages developing ",
					UI.PRE_KEYWORD,
					"Eggs",
					UI.PST_KEYWORD,
					"\n\nIncreases ",
					UI.PRE_KEYWORD,
					"Incubation Rate",
					UI.PST_KEYWORD,
					"\n\nDuplicants must possess the ",
					DUPLICANTS.ROLES.RANCHER.NAME,
					" skill to sing to an egg"
				});
			}

			// Token: 0x02003446 RID: 13382
			public class USETOILET
			{
				// Token: 0x0400D080 RID: 53376
				public static LocString NAME = "Use Toilet";

				// Token: 0x0400D081 RID: 53377
				public static LocString STATUS = "Going to use toilet";

				// Token: 0x0400D082 RID: 53378
				public static LocString TOOLTIP = "Duplicants have to use the toilet at least once per day";
			}

			// Token: 0x02003447 RID: 13383
			public class WASHHANDS
			{
				// Token: 0x0400D083 RID: 53379
				public static LocString NAME = "Wash Hands";

				// Token: 0x0400D084 RID: 53380
				public static LocString STATUS = "Washing hands";

				// Token: 0x0400D085 RID: 53381
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Good hygiene removes ",
					UI.PRE_KEYWORD,
					"Germs",
					UI.PST_KEYWORD,
					" and prevents the spread of ",
					UI.PRE_KEYWORD,
					"Disease",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x02003448 RID: 13384
			public class SLIP
			{
				// Token: 0x0400D086 RID: 53382
				public static LocString NAME = "Slip";

				// Token: 0x0400D087 RID: 53383
				public static LocString STATUS = "Slipping";

				// Token: 0x0400D088 RID: 53384
				public static LocString TOOLTIP = "Slippery surfaces can cause Duplicants to fall \"seat over tea kettle\"";
			}

			// Token: 0x02003449 RID: 13385
			public class CHECKPOINT
			{
				// Token: 0x0400D089 RID: 53385
				public static LocString NAME = "Wait at Checkpoint";

				// Token: 0x0400D08A RID: 53386
				public static LocString STATUS = "Waiting at Checkpoint";

				// Token: 0x0400D08B RID: 53387
				public static LocString TOOLTIP = "This Duplicant is waiting for permission to pass";
			}

			// Token: 0x0200344A RID: 13386
			public class TRAVELTUBEENTRANCE
			{
				// Token: 0x0400D08C RID: 53388
				public static LocString NAME = "Enter Transit Tube";

				// Token: 0x0400D08D RID: 53389
				public static LocString STATUS = "Entering Transit Tube";

				// Token: 0x0400D08E RID: 53390
				public static LocString TOOLTIP = "Nyoooom!";
			}

			// Token: 0x0200344B RID: 13387
			public class SCRUBORE
			{
				// Token: 0x0400D08F RID: 53391
				public static LocString NAME = "Scrub Ore";

				// Token: 0x0400D090 RID: 53392
				public static LocString STATUS = "Scrubbing ore";

				// Token: 0x0400D091 RID: 53393
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Material ore can be scrubbed to remove ",
					UI.PRE_KEYWORD,
					"Germs",
					UI.PST_KEYWORD,
					" present on its surface"
				});
			}

			// Token: 0x0200344C RID: 13388
			public class EAT
			{
				// Token: 0x0400D092 RID: 53394
				public static LocString NAME = "Eat";

				// Token: 0x0400D093 RID: 53395
				public static LocString STATUS = "Going to eat";

				// Token: 0x0400D094 RID: 53396
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Duplicants eat to replenish their ",
					UI.PRE_KEYWORD,
					"Calorie",
					UI.PST_KEYWORD,
					" stores"
				});
			}

			// Token: 0x0200344D RID: 13389
			public class RELOADELECTROBANK
			{
				// Token: 0x0400D095 RID: 53397
				public static LocString NAME = "Power Up";

				// Token: 0x0400D096 RID: 53398
				public static LocString STATUS = "Looking for power banks";

				// Token: 0x0400D097 RID: 53399
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Bionic Duplicants need ",
					UI.PRE_KEYWORD,
					"Power Banks",
					UI.PST_KEYWORD,
					" to function"
				});
			}

			// Token: 0x0200344E RID: 13390
			public class FINDOXYGENSOURCEITEM
			{
				// Token: 0x0400D098 RID: 53400
				public static LocString NAME = "Seek Oxygen Refill";

				// Token: 0x0400D099 RID: 53401
				public static LocString STATUS = "Looking for oxygen refills";

				// Token: 0x0400D09A RID: 53402
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Bionic Duplicants are fitted with internal ",
					UI.PRE_KEYWORD,
					"Oxygen",
					UI.PST_KEYWORD,
					" tanks that must be refilled"
				});
			}

			// Token: 0x0200344F RID: 13391
			public class BIONICABSORBOXYGEN
			{
				// Token: 0x0400D09B RID: 53403
				public static LocString NAME = "Refill Oxygen Tank";

				// Token: 0x0400D09C RID: 53404
				public static LocString STATUS = "Refilling oxygen tank";

				// Token: 0x0400D09D RID: 53405
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Bionic Duplicants automatically refill their internal ",
					UI.PRE_KEYWORD,
					"Oxygen",
					UI.PST_KEYWORD,
					" tanks when levels get too low"
				});
			}

			// Token: 0x02003450 RID: 13392
			public class UNLOADELECTROBANK
			{
				// Token: 0x0400D09E RID: 53406
				public static LocString NAME = "Offload";

				// Token: 0x0400D09F RID: 53407
				public static LocString STATUS = "Offloading empty power banks";

				// Token: 0x0400D0A0 RID: 53408
				public static LocString TOOLTIP = "Bionic Duplicants automatically offload depleted " + UI.PRE_KEYWORD + "Power Banks" + UI.PST_KEYWORD;
			}

			// Token: 0x02003451 RID: 13393
			public class SEEKANDINSTALLUPGRADE
			{
				// Token: 0x0400D0A1 RID: 53409
				public static LocString NAME = "Retrieve Booster";

				// Token: 0x0400D0A2 RID: 53410
				public static LocString STATUS = "Retrieving booster";

				// Token: 0x0400D0A3 RID: 53411
				public static LocString TOOLTIP = "This Duplicant is on its way to retrieve a booster that was assigned to them";
			}

			// Token: 0x02003452 RID: 13394
			public class VOMIT
			{
				// Token: 0x0400D0A4 RID: 53412
				public static LocString NAME = "Vomit";

				// Token: 0x0400D0A5 RID: 53413
				public static LocString STATUS = "Vomiting";

				// Token: 0x0400D0A6 RID: 53414
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Vomiting produces ",
					ELEMENTS.DIRTYWATER.NAME,
					" and can spread ",
					UI.PRE_KEYWORD,
					"Disease",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x02003453 RID: 13395
			public class RADIATIONPAIN
			{
				// Token: 0x0400D0A7 RID: 53415
				public static LocString NAME = "Radiation Aches";

				// Token: 0x0400D0A8 RID: 53416
				public static LocString STATUS = "Feeling radiation aches";

				// Token: 0x0400D0A9 RID: 53417
				public static LocString TOOLTIP = "Radiation Aches are a symptom of " + DUPLICANTS.DISEASES.RADIATIONSICKNESS.NAME;
			}

			// Token: 0x02003454 RID: 13396
			public class COUGH
			{
				// Token: 0x0400D0AA RID: 53418
				public static LocString NAME = "Cough";

				// Token: 0x0400D0AB RID: 53419
				public static LocString STATUS = "Coughing";

				// Token: 0x0400D0AC RID: 53420
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Coughing is a symptom of ",
					DUPLICANTS.DISEASES.SLIMESICKNESS.NAME,
					" and spreads airborne ",
					UI.PRE_KEYWORD,
					"Germs",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x02003455 RID: 13397
			public class WATERDAMAGEZAP
			{
				// Token: 0x0400D0AD RID: 53421
				public static LocString NAME = "Glitch";

				// Token: 0x0400D0AE RID: 53422
				public static LocString STATUS = "Glitching";

				// Token: 0x0400D0AF RID: 53423
				public static LocString TOOLTIP = "Glitching is a symptom of Bionic Duplicant systems malfunctioning due to contact with incompatible " + UI.PRE_KEYWORD + "Liquids" + UI.PST_KEYWORD;
			}

			// Token: 0x02003456 RID: 13398
			public class SLEEP
			{
				// Token: 0x0400D0B0 RID: 53424
				public static LocString NAME = "Sleep";

				// Token: 0x0400D0B1 RID: 53425
				public static LocString STATUS = "Sleeping";

				// Token: 0x0400D0B2 RID: 53426
				public static LocString TOOLTIP = "Zzzzzz...";
			}

			// Token: 0x02003457 RID: 13399
			public class NARCOLEPSY
			{
				// Token: 0x0400D0B3 RID: 53427
				public static LocString NAME = "Narcoleptic Nap";

				// Token: 0x0400D0B4 RID: 53428
				public static LocString STATUS = "Narcoleptic napping";

				// Token: 0x0400D0B5 RID: 53429
				public static LocString TOOLTIP = "Zzzzzz...";
			}

			// Token: 0x02003458 RID: 13400
			public class FLOORSLEEP
			{
				// Token: 0x0400D0B6 RID: 53430
				public static LocString NAME = "Sleep on Floor";

				// Token: 0x0400D0B7 RID: 53431
				public static LocString STATUS = "Sleeping on floor";

				// Token: 0x0400D0B8 RID: 53432
				public static LocString TOOLTIP = "Zzzzzz...\n\nSleeping on the floor will give Duplicants a " + DUPLICANTS.MODIFIERS.SOREBACK.NAME;
			}

			// Token: 0x02003459 RID: 13401
			public class TAKEMEDICINE
			{
				// Token: 0x0400D0B9 RID: 53433
				public static LocString NAME = "Take Medicine";

				// Token: 0x0400D0BA RID: 53434
				public static LocString STATUS = "Taking medicine";

				// Token: 0x0400D0BB RID: 53435
				public static LocString TOOLTIP = "This Duplicant is taking a dose of medicine to ward off " + UI.PRE_KEYWORD + "Disease" + UI.PST_KEYWORD;
			}

			// Token: 0x0200345A RID: 13402
			public class GETDOCTORED
			{
				// Token: 0x0400D0BC RID: 53436
				public static LocString NAME = "Visit Doctor";

				// Token: 0x0400D0BD RID: 53437
				public static LocString STATUS = "Visiting doctor";

				// Token: 0x0400D0BE RID: 53438
				public static LocString TOOLTIP = "This Duplicant is visiting a doctor to receive treatment";
			}

			// Token: 0x0200345B RID: 13403
			public class DOCTOR
			{
				// Token: 0x0400D0BF RID: 53439
				public static LocString NAME = "Treat Patient";

				// Token: 0x0400D0C0 RID: 53440
				public static LocString STATUS = "Treating patient";

				// Token: 0x0400D0C1 RID: 53441
				public static LocString TOOLTIP = "This Duplicant is trying to make one of their peers feel better";
			}

			// Token: 0x0200345C RID: 13404
			public class DELIVERFOOD
			{
				// Token: 0x0400D0C2 RID: 53442
				public static LocString NAME = "Deliver Food";

				// Token: 0x0400D0C3 RID: 53443
				public static LocString STATUS = "Delivering food";

				// Token: 0x0400D0C4 RID: 53444
				public static LocString TOOLTIP = "Under thirty minutes or it's free";
			}

			// Token: 0x0200345D RID: 13405
			public class SHOWER
			{
				// Token: 0x0400D0C5 RID: 53445
				public static LocString NAME = "Shower";

				// Token: 0x0400D0C6 RID: 53446
				public static LocString STATUS = "Showering";

				// Token: 0x0400D0C7 RID: 53447
				public static LocString TOOLTIP = "This Duplicant is having a refreshing shower";
			}

			// Token: 0x0200345E RID: 13406
			public class SIGH
			{
				// Token: 0x0400D0C8 RID: 53448
				public static LocString NAME = "Sigh";

				// Token: 0x0400D0C9 RID: 53449
				public static LocString STATUS = "Sighing";

				// Token: 0x0400D0CA RID: 53450
				public static LocString TOOLTIP = "Ho-hum.";
			}

			// Token: 0x0200345F RID: 13407
			public class RESTDUETODISEASE
			{
				// Token: 0x0400D0CB RID: 53451
				public static LocString NAME = "Rest";

				// Token: 0x0400D0CC RID: 53452
				public static LocString STATUS = "Resting";

				// Token: 0x0400D0CD RID: 53453
				public static LocString TOOLTIP = "This Duplicant isn't feeling well and is taking a rest";
			}

			// Token: 0x02003460 RID: 13408
			public class HEAL
			{
				// Token: 0x0400D0CE RID: 53454
				public static LocString NAME = "Heal";

				// Token: 0x0400D0CF RID: 53455
				public static LocString STATUS = "Healing";

				// Token: 0x0400D0D0 RID: 53456
				public static LocString TOOLTIP = "This Duplicant is taking some time to recover from their wounds";
			}

			// Token: 0x02003461 RID: 13409
			public class STRESSACTINGOUT
			{
				// Token: 0x0400D0D1 RID: 53457
				public static LocString NAME = "Lash Out";

				// Token: 0x0400D0D2 RID: 53458
				public static LocString STATUS = "Lashing out";

				// Token: 0x0400D0D3 RID: 53459
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is having a ",
					UI.PRE_KEYWORD,
					"Stress",
					UI.PST_KEYWORD,
					"-induced tantrum"
				});
			}

			// Token: 0x02003462 RID: 13410
			public class RELAX
			{
				// Token: 0x0400D0D4 RID: 53460
				public static LocString NAME = "Relax";

				// Token: 0x0400D0D5 RID: 53461
				public static LocString STATUS = "Relaxing";

				// Token: 0x0400D0D6 RID: 53462
				public static LocString TOOLTIP = "This Duplicant is taking it easy";
			}

			// Token: 0x02003463 RID: 13411
			public class STRESSHEAL
			{
				// Token: 0x0400D0D7 RID: 53463
				public static LocString NAME = "De-Stress";

				// Token: 0x0400D0D8 RID: 53464
				public static LocString STATUS = "De-stressing";

				// Token: 0x0400D0D9 RID: 53465
				public static LocString TOOLTIP = "This Duplicant taking some time to recover from their " + UI.PRE_KEYWORD + "Stress" + UI.PST_KEYWORD;
			}

			// Token: 0x02003464 RID: 13412
			public class EQUIP
			{
				// Token: 0x0400D0DA RID: 53466
				public static LocString NAME = "Equip";

				// Token: 0x0400D0DB RID: 53467
				public static LocString STATUS = "Moving to equip";

				// Token: 0x0400D0DC RID: 53468
				public static LocString TOOLTIP = "This Duplicant is putting on a piece of equipment";
			}

			// Token: 0x02003465 RID: 13413
			public class LEARNSKILL
			{
				// Token: 0x0400D0DD RID: 53469
				public static LocString NAME = "Learn Skill";

				// Token: 0x0400D0DE RID: 53470
				public static LocString STATUS = "Learning skill";

				// Token: 0x0400D0DF RID: 53471
				public static LocString TOOLTIP = "This Duplicant is learning a new " + UI.PRE_KEYWORD + "Skill" + UI.PST_KEYWORD;
			}

			// Token: 0x02003466 RID: 13414
			public class UNLEARNSKILL
			{
				// Token: 0x0400D0E0 RID: 53472
				public static LocString NAME = "Unlearn Skills";

				// Token: 0x0400D0E1 RID: 53473
				public static LocString STATUS = "Unlearning skills";

				// Token: 0x0400D0E2 RID: 53474
				public static LocString TOOLTIP = "This Duplicant is unlearning " + UI.PRE_KEYWORD + "Skills" + UI.PST_KEYWORD;
			}

			// Token: 0x02003467 RID: 13415
			public class RECHARGE
			{
				// Token: 0x0400D0E3 RID: 53475
				public static LocString NAME = "Recharge Equipment";

				// Token: 0x0400D0E4 RID: 53476
				public static LocString STATUS = "Recharging equipment";

				// Token: 0x0400D0E5 RID: 53477
				public static LocString TOOLTIP = "This Duplicant is recharging their equipment";
			}

			// Token: 0x02003468 RID: 13416
			public class UNEQUIP
			{
				// Token: 0x0400D0E6 RID: 53478
				public static LocString NAME = "Unequip";

				// Token: 0x0400D0E7 RID: 53479
				public static LocString STATUS = "Moving to unequip";

				// Token: 0x0400D0E8 RID: 53480
				public static LocString TOOLTIP = "This Duplicant is removing a piece of their equipment";
			}

			// Token: 0x02003469 RID: 13417
			public class MOURN
			{
				// Token: 0x0400D0E9 RID: 53481
				public static LocString NAME = "Mourn";

				// Token: 0x0400D0EA RID: 53482
				public static LocString STATUS = "Mourning";

				// Token: 0x0400D0EB RID: 53483
				public static LocString TOOLTIP = "This Duplicant is mourning the loss of a friend";
			}

			// Token: 0x0200346A RID: 13418
			public class WARMUP
			{
				// Token: 0x0400D0EC RID: 53484
				public static LocString NAME = "Warm Up";

				// Token: 0x0400D0ED RID: 53485
				public static LocString STATUS = "Going to warm up";

				// Token: 0x0400D0EE RID: 53486
				public static LocString TOOLTIP = "This Duplicant got too cold and is going somewhere to warm up";
			}

			// Token: 0x0200346B RID: 13419
			public class COOLDOWN
			{
				// Token: 0x0400D0EF RID: 53487
				public static LocString NAME = "Cool Off";

				// Token: 0x0400D0F0 RID: 53488
				public static LocString STATUS = "Going to cool off";

				// Token: 0x0400D0F1 RID: 53489
				public static LocString TOOLTIP = "This Duplicant got too hot and is going somewhere to cool off";
			}

			// Token: 0x0200346C RID: 13420
			public class EMPTYSTORAGE
			{
				// Token: 0x0400D0F2 RID: 53490
				public static LocString NAME = "Empty Storage";

				// Token: 0x0400D0F3 RID: 53491
				public static LocString STATUS = "Going to empty storage";

				// Token: 0x0400D0F4 RID: 53492
				public static LocString TOOLTIP = "This Duplicant is taking items out of storage";
			}

			// Token: 0x0200346D RID: 13421
			public class ART
			{
				// Token: 0x0400D0F5 RID: 53493
				public static LocString NAME = "Decorate";

				// Token: 0x0400D0F6 RID: 53494
				public static LocString STATUS = "Going to decorate";

				// Token: 0x0400D0F7 RID: 53495
				public static LocString TOOLTIP = "This Duplicant is going to work on their art";
			}

			// Token: 0x0200346E RID: 13422
			public class MOP
			{
				// Token: 0x0400D0F8 RID: 53496
				public static LocString NAME = "Mop";

				// Token: 0x0400D0F9 RID: 53497
				public static LocString STATUS = "Going to mop";

				// Token: 0x0400D0FA RID: 53498
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Mopping removes ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					" from the floor and bottles them for transport"
				});
			}

			// Token: 0x0200346F RID: 13423
			public class RELOCATE
			{
				// Token: 0x0400D0FB RID: 53499
				public static LocString NAME = "Relocate";

				// Token: 0x0400D0FC RID: 53500
				public static LocString STATUS = "Going to relocate";

				// Token: 0x0400D0FD RID: 53501
				public static LocString TOOLTIP = "This Duplicant is moving a building to a new location";
			}

			// Token: 0x02003470 RID: 13424
			public class TOGGLE
			{
				// Token: 0x0400D0FE RID: 53502
				public static LocString NAME = "Change Setting";

				// Token: 0x0400D0FF RID: 53503
				public static LocString STATUS = "Going to change setting";

				// Token: 0x0400D100 RID: 53504
				public static LocString TOOLTIP = "This Duplicant is going to change the settings on a building";
			}

			// Token: 0x02003471 RID: 13425
			public class RESCUEINCAPACITATED
			{
				// Token: 0x0400D101 RID: 53505
				public static LocString NAME = "Rescue Friend";

				// Token: 0x0400D102 RID: 53506
				public static LocString STATUS = "Rescuing friend";

				// Token: 0x0400D103 RID: 53507
				public static LocString TOOLTIP = "This Duplicant is rescuing another Duplicant that has been incapacitated";
			}

			// Token: 0x02003472 RID: 13426
			public class REPAIR
			{
				// Token: 0x0400D104 RID: 53508
				public static LocString NAME = "Repair";

				// Token: 0x0400D105 RID: 53509
				public static LocString STATUS = "Going to repair";

				// Token: 0x0400D106 RID: 53510
				public static LocString TOOLTIP = "This Duplicant is fixing a broken building";
			}

			// Token: 0x02003473 RID: 13427
			public class DECONSTRUCT
			{
				// Token: 0x0400D107 RID: 53511
				public static LocString NAME = "Deconstruct";

				// Token: 0x0400D108 RID: 53512
				public static LocString STATUS = "Going to deconstruct";

				// Token: 0x0400D109 RID: 53513
				public static LocString TOOLTIP = "This Duplicant is deconstructing a building";
			}

			// Token: 0x02003474 RID: 13428
			public class RESEARCH
			{
				// Token: 0x0400D10A RID: 53514
				public static LocString NAME = "Research";

				// Token: 0x0400D10B RID: 53515
				public static LocString STATUS = "Going to research";

				// Token: 0x0400D10C RID: 53516
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is working on the current ",
					UI.PRE_KEYWORD,
					"Research",
					UI.PST_KEYWORD,
					" focus"
				});
			}

			// Token: 0x02003475 RID: 13429
			public class ANALYZEARTIFACT
			{
				// Token: 0x0400D10D RID: 53517
				public static LocString NAME = "Artifact Analysis";

				// Token: 0x0400D10E RID: 53518
				public static LocString STATUS = "Going to analyze artifacts";

				// Token: 0x0400D10F RID: 53519
				public static LocString TOOLTIP = "This Duplicant is analyzing " + UI.PRE_KEYWORD + "Artifacts" + UI.PST_KEYWORD;
			}

			// Token: 0x02003476 RID: 13430
			public class ANALYZESEED
			{
				// Token: 0x0400D110 RID: 53520
				public static LocString NAME = "Seed Analysis";

				// Token: 0x0400D111 RID: 53521
				public static LocString STATUS = "Going to analyze seeds";

				// Token: 0x0400D112 RID: 53522
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is analyzing ",
					UI.PRE_KEYWORD,
					"Seeds",
					UI.PST_KEYWORD,
					" to find mutations"
				});
			}

			// Token: 0x02003477 RID: 13431
			public class RETURNSUIT
			{
				// Token: 0x0400D113 RID: 53523
				public static LocString NAME = "Dock Exosuit";

				// Token: 0x0400D114 RID: 53524
				public static LocString STATUS = "Docking exosuit";

				// Token: 0x0400D115 RID: 53525
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is plugging an ",
					UI.PRE_KEYWORD,
					"Exosuit",
					UI.PST_KEYWORD,
					" in for refilling"
				});
			}

			// Token: 0x02003478 RID: 13432
			public class GENERATEPOWER
			{
				// Token: 0x0400D116 RID: 53526
				public static LocString NAME = "Generate Power";

				// Token: 0x0400D117 RID: 53527
				public static LocString STATUS = "Going to generate power";

				// Token: 0x0400D118 RID: 53528
				public static LocString TOOLTIP = "This Duplicant is producing electrical " + UI.PRE_KEYWORD + "Power" + UI.PST_KEYWORD;
			}

			// Token: 0x02003479 RID: 13433
			public class HARVEST
			{
				// Token: 0x0400D119 RID: 53529
				public static LocString NAME = "Harvest";

				// Token: 0x0400D11A RID: 53530
				public static LocString STATUS = "Going to harvest";

				// Token: 0x0400D11B RID: 53531
				public static LocString TOOLTIP = "This Duplicant is harvesting usable materials from a mature " + UI.PRE_KEYWORD + "Plant" + UI.PST_KEYWORD;
			}

			// Token: 0x0200347A RID: 13434
			public class UPROOT
			{
				// Token: 0x0400D11C RID: 53532
				public static LocString NAME = "Uproot";

				// Token: 0x0400D11D RID: 53533
				public static LocString STATUS = "Going to uproot";

				// Token: 0x0400D11E RID: 53534
				public static LocString TOOLTIP = "This Duplicant is uprooting a plant to retrieve a " + UI.PRE_KEYWORD + "Seed" + UI.PST_KEYWORD;
			}

			// Token: 0x0200347B RID: 13435
			public class CLEANTOILET
			{
				// Token: 0x0400D11F RID: 53535
				public static LocString NAME = "Clean Outhouse";

				// Token: 0x0400D120 RID: 53536
				public static LocString STATUS = "Going to clean";

				// Token: 0x0400D121 RID: 53537
				public static LocString TOOLTIP = "This Duplicant is cleaning out the " + BUILDINGS.PREFABS.OUTHOUSE.NAME;
			}

			// Token: 0x0200347C RID: 13436
			public class EMPTYDESALINATOR
			{
				// Token: 0x0400D122 RID: 53538
				public static LocString NAME = "Empty Desalinator";

				// Token: 0x0400D123 RID: 53539
				public static LocString STATUS = "Going to clean";

				// Token: 0x0400D124 RID: 53540
				public static LocString TOOLTIP = "This Duplicant is emptying out the " + BUILDINGS.PREFABS.DESALINATOR.NAME;
			}

			// Token: 0x0200347D RID: 13437
			public class LIQUIDCOOLEDFAN
			{
				// Token: 0x0400D125 RID: 53541
				public static LocString NAME = "Use Fan";

				// Token: 0x0400D126 RID: 53542
				public static LocString STATUS = "Going to use fan";

				// Token: 0x0400D127 RID: 53543
				public static LocString TOOLTIP = "This Duplicant is attempting to cool down the area";
			}

			// Token: 0x0200347E RID: 13438
			public class ICECOOLEDFAN
			{
				// Token: 0x0400D128 RID: 53544
				public static LocString NAME = "Use Fan";

				// Token: 0x0400D129 RID: 53545
				public static LocString STATUS = "Going to use fan";

				// Token: 0x0400D12A RID: 53546
				public static LocString TOOLTIP = "This Duplicant is attempting to cool down the area";
			}

			// Token: 0x0200347F RID: 13439
			public class PROCESSCRITTER
			{
				// Token: 0x0400D12B RID: 53547
				public static LocString NAME = "Process Critter";

				// Token: 0x0400D12C RID: 53548
				public static LocString STATUS = "Going to process critter";

				// Token: 0x0400D12D RID: 53549
				public static LocString TOOLTIP = "This Duplicant is processing " + UI.PRE_KEYWORD + "Critters" + UI.PST_KEYWORD;
			}

			// Token: 0x02003480 RID: 13440
			public class COOK
			{
				// Token: 0x0400D12E RID: 53550
				public static LocString NAME = "Cook";

				// Token: 0x0400D12F RID: 53551
				public static LocString STATUS = "Going to cook";

				// Token: 0x0400D130 RID: 53552
				public static LocString TOOLTIP = "This Duplicant is cooking " + UI.PRE_KEYWORD + "Food" + UI.PST_KEYWORD;
			}

			// Token: 0x02003481 RID: 13441
			public class COMPOUND
			{
				// Token: 0x0400D131 RID: 53553
				public static LocString NAME = "Compound Medicine";

				// Token: 0x0400D132 RID: 53554
				public static LocString STATUS = "Going to compound medicine";

				// Token: 0x0400D133 RID: 53555
				public static LocString TOOLTIP = "This Duplicant is fabricating " + UI.PRE_KEYWORD + "Medicine" + UI.PST_KEYWORD;
			}

			// Token: 0x02003482 RID: 13442
			public class TRAIN
			{
				// Token: 0x0400D134 RID: 53556
				public static LocString NAME = "Train";

				// Token: 0x0400D135 RID: 53557
				public static LocString STATUS = "Training";

				// Token: 0x0400D136 RID: 53558
				public static LocString TOOLTIP = "This Duplicant is busy training";
			}

			// Token: 0x02003483 RID: 13443
			public class MUSH
			{
				// Token: 0x0400D137 RID: 53559
				public static LocString NAME = "Mush";

				// Token: 0x0400D138 RID: 53560
				public static LocString STATUS = "Going to mush";

				// Token: 0x0400D139 RID: 53561
				public static LocString TOOLTIP = "This Duplicant is producing " + UI.PRE_KEYWORD + "Food" + UI.PST_KEYWORD;
			}

			// Token: 0x02003484 RID: 13444
			public class COMPOSTWORKABLE
			{
				// Token: 0x0400D13A RID: 53562
				public static LocString NAME = "Compost";

				// Token: 0x0400D13B RID: 53563
				public static LocString STATUS = "Going to compost";

				// Token: 0x0400D13C RID: 53564
				public static LocString TOOLTIP = "This Duplicant is dropping off organic material at the " + BUILDINGS.PREFABS.COMPOST.NAME;
			}

			// Token: 0x02003485 RID: 13445
			public class FLIPCOMPOST
			{
				// Token: 0x0400D13D RID: 53565
				public static LocString NAME = "Flip";

				// Token: 0x0400D13E RID: 53566
				public static LocString STATUS = "Going to flip compost";

				// Token: 0x0400D13F RID: 53567
				public static LocString TOOLTIP = BUILDINGS.PREFABS.COMPOST.NAME + "s need to be flipped in order for their contents to compost";
			}

			// Token: 0x02003486 RID: 13446
			public class DEPRESSURIZE
			{
				// Token: 0x0400D140 RID: 53568
				public static LocString NAME = "Depressurize Well";

				// Token: 0x0400D141 RID: 53569
				public static LocString STATUS = "Going to depressurize well";

				// Token: 0x0400D142 RID: 53570
				public static LocString TOOLTIP = BUILDINGS.PREFABS.OILWELLCAP.NAME + "s need to be periodically depressurized to function";
			}

			// Token: 0x02003487 RID: 13447
			public class FABRICATE
			{
				// Token: 0x0400D143 RID: 53571
				public static LocString NAME = "Fabricate";

				// Token: 0x0400D144 RID: 53572
				public static LocString STATUS = "Going to fabricate";

				// Token: 0x0400D145 RID: 53573
				public static LocString TOOLTIP = "This Duplicant is crafting something";
			}

			// Token: 0x02003488 RID: 13448
			public class BUILD
			{
				// Token: 0x0400D146 RID: 53574
				public static LocString NAME = "Build";

				// Token: 0x0400D147 RID: 53575
				public static LocString STATUS = "Going to build";

				// Token: 0x0400D148 RID: 53576
				public static LocString TOOLTIP = "This Duplicant is constructing a new building";
			}

			// Token: 0x02003489 RID: 13449
			public class BUILDDIG
			{
				// Token: 0x0400D149 RID: 53577
				public static LocString NAME = "Construction Dig";

				// Token: 0x0400D14A RID: 53578
				public static LocString STATUS = "Going to construction dig";

				// Token: 0x0400D14B RID: 53579
				public static LocString TOOLTIP = "This Duplicant is making room for a planned construction task by performing this dig";
			}

			// Token: 0x0200348A RID: 13450
			public class DIG
			{
				// Token: 0x0400D14C RID: 53580
				public static LocString NAME = "Dig";

				// Token: 0x0400D14D RID: 53581
				public static LocString STATUS = "Going to dig";

				// Token: 0x0400D14E RID: 53582
				public static LocString TOOLTIP = "This Duplicant is digging out a tile";
			}

			// Token: 0x0200348B RID: 13451
			public class FETCH
			{
				// Token: 0x0400D14F RID: 53583
				public static LocString NAME = "Deliver";

				// Token: 0x0400D150 RID: 53584
				public static LocString STATUS = "Delivering";

				// Token: 0x0400D151 RID: 53585
				public static LocString TOOLTIP = "This Duplicant is delivering materials where they need to go";

				// Token: 0x0400D152 RID: 53586
				public static LocString REPORT_NAME = "Deliver to {0}";
			}

			// Token: 0x0200348C RID: 13452
			public class JOYREACTION
			{
				// Token: 0x0400D153 RID: 53587
				public static LocString NAME = "Joy Reaction";

				// Token: 0x0400D154 RID: 53588
				public static LocString STATUS = "Overjoyed";

				// Token: 0x0400D155 RID: 53589
				public static LocString TOOLTIP = "This Duplicant is taking a moment to relish in their own happiness";

				// Token: 0x0400D156 RID: 53590
				public static LocString REPORT_NAME = "Overjoyed Reaction";
			}

			// Token: 0x0200348D RID: 13453
			public class ROCKETCONTROL
			{
				// Token: 0x0400D157 RID: 53591
				public static LocString NAME = "Rocket Control";

				// Token: 0x0400D158 RID: 53592
				public static LocString STATUS = "Controlling rocket";

				// Token: 0x0400D159 RID: 53593
				public static LocString TOOLTIP = "This Duplicant is keeping their spacecraft on course";

				// Token: 0x0400D15A RID: 53594
				public static LocString REPORT_NAME = "Rocket Control";
			}

			// Token: 0x0200348E RID: 13454
			public class STORAGEFETCH
			{
				// Token: 0x0400D15B RID: 53595
				public static LocString NAME = "Store Materials";

				// Token: 0x0400D15C RID: 53596
				public static LocString STATUS = "Storing materials";

				// Token: 0x0400D15D RID: 53597
				public static LocString TOOLTIP = "This Duplicant is moving materials into storage for later use";

				// Token: 0x0400D15E RID: 53598
				public static LocString REPORT_NAME = "Store {0}";
			}

			// Token: 0x0200348F RID: 13455
			public class EQUIPMENTFETCH
			{
				// Token: 0x0400D15F RID: 53599
				public static LocString NAME = "Store Equipment";

				// Token: 0x0400D160 RID: 53600
				public static LocString STATUS = "Storing equipment";

				// Token: 0x0400D161 RID: 53601
				public static LocString TOOLTIP = "This Duplicant is transporting equipment for storage";

				// Token: 0x0400D162 RID: 53602
				public static LocString REPORT_NAME = "Store {0}";
			}

			// Token: 0x02003490 RID: 13456
			public class REPAIRFETCH
			{
				// Token: 0x0400D163 RID: 53603
				public static LocString NAME = "Repair Supply";

				// Token: 0x0400D164 RID: 53604
				public static LocString STATUS = "Supplying repair materials";

				// Token: 0x0400D165 RID: 53605
				public static LocString TOOLTIP = "This Duplicant is delivering materials to where they'll be needed to repair buildings";
			}

			// Token: 0x02003491 RID: 13457
			public class RESEARCHFETCH
			{
				// Token: 0x0400D166 RID: 53606
				public static LocString NAME = "Research Supply";

				// Token: 0x0400D167 RID: 53607
				public static LocString STATUS = "Supplying research materials";

				// Token: 0x0400D168 RID: 53608
				public static LocString TOOLTIP = "This Duplicant is delivering materials where they'll be needed to conduct " + UI.PRE_KEYWORD + "Research" + UI.PST_KEYWORD;
			}

			// Token: 0x02003492 RID: 13458
			public class EXCAVATEFOSSIL
			{
				// Token: 0x0400D169 RID: 53609
				public static LocString NAME = "Excavate Fossil";

				// Token: 0x0400D16A RID: 53610
				public static LocString STATUS = "Excavating a fossil";

				// Token: 0x0400D16B RID: 53611
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is excavating a ",
					UI.PRE_KEYWORD,
					"Fossil",
					UI.PST_KEYWORD,
					" site"
				});
			}

			// Token: 0x02003493 RID: 13459
			public class ARMTRAP
			{
				// Token: 0x0400D16C RID: 53612
				public static LocString NAME = "Arm Trap";

				// Token: 0x0400D16D RID: 53613
				public static LocString STATUS = "Arming a trap";

				// Token: 0x0400D16E RID: 53614
				public static LocString TOOLTIP = "This Duplicant is arming a trap";
			}

			// Token: 0x02003494 RID: 13460
			public class FARMFETCH
			{
				// Token: 0x0400D16F RID: 53615
				public static LocString NAME = "Farming Supply";

				// Token: 0x0400D170 RID: 53616
				public static LocString STATUS = "Supplying farming materials";

				// Token: 0x0400D171 RID: 53617
				public static LocString TOOLTIP = "This Duplicant is delivering farming materials where they're needed to tend " + UI.PRE_KEYWORD + "Crops" + UI.PST_KEYWORD;
			}

			// Token: 0x02003495 RID: 13461
			public class FETCHCRITICAL
			{
				// Token: 0x0400D172 RID: 53618
				public static LocString NAME = "Life Support Supply";

				// Token: 0x0400D173 RID: 53619
				public static LocString STATUS = "Supplying critical materials";

				// Token: 0x0400D174 RID: 53620
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is delivering materials required to perform ",
					UI.PRE_KEYWORD,
					"Life Support",
					UI.PST_KEYWORD,
					" Errands"
				});

				// Token: 0x0400D175 RID: 53621
				public static LocString REPORT_NAME = "Life Support Supply to {0}";
			}

			// Token: 0x02003496 RID: 13462
			public class MACHINEFETCH
			{
				// Token: 0x0400D176 RID: 53622
				public static LocString NAME = "Operational Supply";

				// Token: 0x0400D177 RID: 53623
				public static LocString STATUS = "Supplying operational materials";

				// Token: 0x0400D178 RID: 53624
				public static LocString TOOLTIP = "This Duplicant is delivering materials to where they'll be needed for machine operation";

				// Token: 0x0400D179 RID: 53625
				public static LocString REPORT_NAME = "Operational Supply to {0}";
			}

			// Token: 0x02003497 RID: 13463
			public class COOKFETCH
			{
				// Token: 0x0400D17A RID: 53626
				public static LocString NAME = "Cook Supply";

				// Token: 0x0400D17B RID: 53627
				public static LocString STATUS = "Supplying cook ingredients";

				// Token: 0x0400D17C RID: 53628
				public static LocString TOOLTIP = "This Duplicant is delivering materials required to cook " + UI.PRE_KEYWORD + "Food" + UI.PST_KEYWORD;
			}

			// Token: 0x02003498 RID: 13464
			public class DOCTORFETCH
			{
				// Token: 0x0400D17D RID: 53629
				public static LocString NAME = "Medical Supply";

				// Token: 0x0400D17E RID: 53630
				public static LocString STATUS = "Supplying medical resources";

				// Token: 0x0400D17F RID: 53631
				public static LocString TOOLTIP = "This Duplicant is delivering the materials that will be needed to treat sick patients";

				// Token: 0x0400D180 RID: 53632
				public static LocString REPORT_NAME = "Medical Supply to {0}";
			}

			// Token: 0x02003499 RID: 13465
			public class FOODFETCH
			{
				// Token: 0x0400D181 RID: 53633
				public static LocString NAME = "Store Food";

				// Token: 0x0400D182 RID: 53634
				public static LocString STATUS = "Storing food";

				// Token: 0x0400D183 RID: 53635
				public static LocString TOOLTIP = "This Duplicant is moving edible resources into proper storage";

				// Token: 0x0400D184 RID: 53636
				public static LocString REPORT_NAME = "Store {0}";
			}

			// Token: 0x0200349A RID: 13466
			public class POWERFETCH
			{
				// Token: 0x0400D185 RID: 53637
				public static LocString NAME = "Power Supply";

				// Token: 0x0400D186 RID: 53638
				public static LocString STATUS = "Supplying power materials";

				// Token: 0x0400D187 RID: 53639
				public static LocString TOOLTIP = "This Duplicant is delivering materials to where they'll be needed for " + UI.PRE_KEYWORD + "Power" + UI.PST_KEYWORD;

				// Token: 0x0400D188 RID: 53640
				public static LocString REPORT_NAME = "Power Supply to {0}";
			}

			// Token: 0x0200349B RID: 13467
			public class FABRICATEFETCH
			{
				// Token: 0x0400D189 RID: 53641
				public static LocString NAME = "Fabrication Supply";

				// Token: 0x0400D18A RID: 53642
				public static LocString STATUS = "Supplying fabrication materials";

				// Token: 0x0400D18B RID: 53643
				public static LocString TOOLTIP = "This Duplicant is delivering materials required to fabricate new objects";

				// Token: 0x0400D18C RID: 53644
				public static LocString REPORT_NAME = "Fabrication Supply to {0}";
			}

			// Token: 0x0200349C RID: 13468
			public class BUILDFETCH
			{
				// Token: 0x0400D18D RID: 53645
				public static LocString NAME = "Construction Supply";

				// Token: 0x0400D18E RID: 53646
				public static LocString STATUS = "Supplying construction materials";

				// Token: 0x0400D18F RID: 53647
				public static LocString TOOLTIP = "This delivery will provide materials to a planned construction site";
			}

			// Token: 0x0200349D RID: 13469
			public class FETCHCREATURE
			{
				// Token: 0x0400D190 RID: 53648
				public static LocString NAME = "Relocate Critter";

				// Token: 0x0400D191 RID: 53649
				public static LocString STATUS = "Relocating critter";

				// Token: 0x0400D192 RID: 53650
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is moving a ",
					UI.PRE_KEYWORD,
					"Critter",
					UI.PST_KEYWORD,
					" to a new location"
				});
			}

			// Token: 0x0200349E RID: 13470
			public class FETCHRANCHING
			{
				// Token: 0x0400D193 RID: 53651
				public static LocString NAME = "Ranching Supply";

				// Token: 0x0400D194 RID: 53652
				public static LocString STATUS = "Supplying ranching materials";

				// Token: 0x0400D195 RID: 53653
				public static LocString TOOLTIP = "This Duplicant is delivering materials for ranching activities";
			}

			// Token: 0x0200349F RID: 13471
			public class TRANSPORT
			{
				// Token: 0x0400D196 RID: 53654
				public static LocString NAME = "Sweep";

				// Token: 0x0400D197 RID: 53655
				public static LocString STATUS = "Going to sweep";

				// Token: 0x0400D198 RID: 53656
				public static LocString TOOLTIP = "Moving debris off the ground and into storage improves colony " + UI.PRE_KEYWORD + "Decor" + UI.PST_KEYWORD;
			}

			// Token: 0x020034A0 RID: 13472
			public class MOVETOSAFETY
			{
				// Token: 0x0400D199 RID: 53657
				public static LocString NAME = "Find Safe Area";

				// Token: 0x0400D19A RID: 53658
				public static LocString STATUS = "Finding safer area";

				// Token: 0x0400D19B RID: 53659
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is ",
					UI.PRE_KEYWORD,
					"Idle",
					UI.PST_KEYWORD,
					" and looking for somewhere safe and comfy to chill"
				});
			}

			// Token: 0x020034A1 RID: 13473
			public class PARTY
			{
				// Token: 0x0400D19C RID: 53660
				public static LocString NAME = "Party";

				// Token: 0x0400D19D RID: 53661
				public static LocString STATUS = "Partying";

				// Token: 0x0400D19E RID: 53662
				public static LocString TOOLTIP = "This Duplicant is partying hard";
			}

			// Token: 0x020034A2 RID: 13474
			public class REMOTEWORK
			{
				// Token: 0x0400D19F RID: 53663
				public static LocString NAME = "Remote Work";

				// Token: 0x0400D1A0 RID: 53664
				public static LocString STATUS = "Working remotely";

				// Token: 0x0400D1A1 RID: 53665
				public static LocString TOOLTIP = "This Duplicant's body is here, but their work is elsewhere";
			}

			// Token: 0x020034A3 RID: 13475
			public class POWER_TINKER
			{
				// Token: 0x0400D1A2 RID: 53666
				public static LocString NAME = "Tinker";

				// Token: 0x0400D1A3 RID: 53667
				public static LocString STATUS = "Tinkering";

				// Token: 0x0400D1A4 RID: 53668
				public static LocString TOOLTIP = "Tinkering with buildings improves their functionality";
			}

			// Token: 0x020034A4 RID: 13476
			public class RANCH
			{
				// Token: 0x0400D1A5 RID: 53669
				public static LocString NAME = "Ranch";

				// Token: 0x0400D1A6 RID: 53670
				public static LocString STATUS = "Ranching";

				// Token: 0x0400D1A7 RID: 53671
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is tending to a ",
					UI.PRE_KEYWORD,
					"Critter",
					UI.PST_KEYWORD,
					"'s well-being"
				});

				// Token: 0x0400D1A8 RID: 53672
				public static LocString REPORT_NAME = "Deliver to {0}";
			}

			// Token: 0x020034A5 RID: 13477
			public class CROP_TEND
			{
				// Token: 0x0400D1A9 RID: 53673
				public static LocString NAME = "Tend";

				// Token: 0x0400D1AA RID: 53674
				public static LocString STATUS = "Tending plant";

				// Token: 0x0400D1AB RID: 53675
				public static LocString TOOLTIP = "Tending to plants increases their " + UI.PRE_KEYWORD + "Growth Rate" + UI.PST_KEYWORD;
			}

			// Token: 0x020034A6 RID: 13478
			public class DEMOLISH
			{
				// Token: 0x0400D1AC RID: 53676
				public static LocString NAME = "Demolish";

				// Token: 0x0400D1AD RID: 53677
				public static LocString STATUS = "Demolishing object";

				// Token: 0x0400D1AE RID: 53678
				public static LocString TOOLTIP = "Demolishing an object removes it permanently";
			}

			// Token: 0x020034A7 RID: 13479
			public class IDLE
			{
				// Token: 0x0400D1AF RID: 53679
				public static LocString NAME = "Idle";

				// Token: 0x0400D1B0 RID: 53680
				public static LocString STATUS = "Idle";

				// Token: 0x0400D1B1 RID: 53681
				public static LocString TOOLTIP = "This Duplicant cannot reach any pending " + UI.PRE_KEYWORD + "Errands" + UI.PST_KEYWORD;
			}

			// Token: 0x020034A8 RID: 13480
			public class PRECONDITIONS
			{
				// Token: 0x0400D1B2 RID: 53682
				public static LocString HEADER = "The selected {Selected} could:";

				// Token: 0x0400D1B3 RID: 53683
				public static LocString SUCCESS_ROW = "{Duplicant} -- {Rank}";

				// Token: 0x0400D1B4 RID: 53684
				public static LocString CURRENT_ERRAND = "Current Errand";

				// Token: 0x0400D1B5 RID: 53685
				public static LocString RANK_FORMAT = "#{0}";

				// Token: 0x0400D1B6 RID: 53686
				public static LocString FAILURE_ROW = "{Duplicant} -- {Reason}";

				// Token: 0x0400D1B7 RID: 53687
				public static LocString CONTAINS_OXYGEN = "Not enough Oxygen";

				// Token: 0x0400D1B8 RID: 53688
				public static LocString IS_PREEMPTABLE = "Already assigned to {Assignee}";

				// Token: 0x0400D1B9 RID: 53689
				public static LocString HAS_URGE = "No current need";

				// Token: 0x0400D1BA RID: 53690
				public static LocString IS_VALID = "Invalid";

				// Token: 0x0400D1BB RID: 53691
				public static LocString IS_PERMITTED = "Not permitted";

				// Token: 0x0400D1BC RID: 53692
				public static LocString IS_ASSIGNED_TO_ME = "Not assigned to {Selected}";

				// Token: 0x0400D1BD RID: 53693
				public static LocString IS_IN_MY_WORLD = "Outside world";

				// Token: 0x0400D1BE RID: 53694
				public static LocString IS_CELL_NOT_IN_MY_WORLD = "Already there";

				// Token: 0x0400D1BF RID: 53695
				public static LocString IS_IN_MY_ROOM = "Outside {Selected}'s room";

				// Token: 0x0400D1C0 RID: 53696
				public static LocString IS_PREFERRED_ASSIGNABLE = "Not preferred assignment";

				// Token: 0x0400D1C1 RID: 53697
				public static LocString IS_PREFERRED_ASSIGNABLE_OR_URGENT_BLADDER = "Not preferred assignment";

				// Token: 0x0400D1C2 RID: 53698
				public static LocString HAS_SKILL_PERK = "Requires learned skill";

				// Token: 0x0400D1C3 RID: 53699
				public static LocString IS_MORE_SATISFYING = "Low priority";

				// Token: 0x0400D1C4 RID: 53700
				public static LocString CAN_CHAT = "Unreachable";

				// Token: 0x0400D1C5 RID: 53701
				public static LocString IS_NOT_RED_ALERT = "Unavailable in Red Alert";

				// Token: 0x0400D1C6 RID: 53702
				public static LocString NO_DEAD_BODIES = "Unburied Duplicant";

				// Token: 0x0400D1C7 RID: 53703
				public static LocString NOT_A_ROBOT = "Unavailable to Robots";

				// Token: 0x0400D1C8 RID: 53704
				public static LocString NOT_A_BIONIC = "Unavailable to Bionic Duplicants";

				// Token: 0x0400D1C9 RID: 53705
				public static LocString VALID_MOURNING_SITE = "Nowhere to mourn";

				// Token: 0x0400D1CA RID: 53706
				public static LocString HAS_PLACE_TO_STAND = "Nowhere to stand";

				// Token: 0x0400D1CB RID: 53707
				public static LocString IS_SCHEDULED_TIME = "Not allowed by schedule";

				// Token: 0x0400D1CC RID: 53708
				public static LocString CAN_MOVE_TO = "Unreachable";

				// Token: 0x0400D1CD RID: 53709
				public static LocString CAN_PICKUP = "Cannot pickup";

				// Token: 0x0400D1CE RID: 53710
				public static LocString IS_AWAKE = "{Selected} is sleeping";

				// Token: 0x0400D1CF RID: 53711
				public static LocString IS_STANDING = "{Selected} must stand";

				// Token: 0x0400D1D0 RID: 53712
				public static LocString IS_MOVING = "{Selected} is not moving";

				// Token: 0x0400D1D1 RID: 53713
				public static LocString IS_OFF_LADDER = "{Selected} is busy climbing";

				// Token: 0x0400D1D2 RID: 53714
				public static LocString NOT_IN_TUBE = "{Selected} is busy in transit";

				// Token: 0x0400D1D3 RID: 53715
				public static LocString HAS_TRAIT = "Missing required trait";

				// Token: 0x0400D1D4 RID: 53716
				public static LocString IS_OPERATIONAL = "Not operational";

				// Token: 0x0400D1D5 RID: 53717
				public static LocString IS_MARKED_FOR_DECONSTRUCTION = "Being deconstructed";

				// Token: 0x0400D1D6 RID: 53718
				public static LocString IS_NOT_BURROWED = "Is not burrowed";

				// Token: 0x0400D1D7 RID: 53719
				public static LocString IS_CREATURE_AVAILABLE_FOR_RANCHING = "No Critters Available";

				// Token: 0x0400D1D8 RID: 53720
				public static LocString IS_CREATURE_AVAILABLE_FOR_FIXED_CAPTURE = "Pen Status OK";

				// Token: 0x0400D1D9 RID: 53721
				public static LocString IS_MARKED_FOR_DISABLE = "Building Disabled";

				// Token: 0x0400D1DA RID: 53722
				public static LocString IS_FUNCTIONAL = "Not functioning";

				// Token: 0x0400D1DB RID: 53723
				public static LocString IS_OVERRIDE_TARGET_NULL_OR_ME = "DebugIsOverrideTargetNullOrMe";

				// Token: 0x0400D1DC RID: 53724
				public static LocString NOT_CHORE_CREATOR = "DebugNotChoreCreator";

				// Token: 0x0400D1DD RID: 53725
				public static LocString IS_GETTING_MORE_STRESSED = "{Selected}'s stress is decreasing";

				// Token: 0x0400D1DE RID: 53726
				public static LocString IS_ALLOWED_BY_AUTOMATION = "Automated";

				// Token: 0x0400D1DF RID: 53727
				public static LocString CAN_DO_RECREATION = "Not Interested";

				// Token: 0x0400D1E0 RID: 53728
				public static LocString DOES_SUIT_NEED_RECHARGING_IDLE = "Suit is currently charged";

				// Token: 0x0400D1E1 RID: 53729
				public static LocString DOES_SUIT_NEED_RECHARGING_URGENT = "Suit is currently charged";

				// Token: 0x0400D1E2 RID: 53730
				public static LocString HAS_SUIT_MARKER = "No Suit Checkpoint";

				// Token: 0x0400D1E3 RID: 53731
				public static LocString ALLOWED_TO_DEPRESSURIZE = "Not currently overpressure";

				// Token: 0x0400D1E4 RID: 53732
				public static LocString IS_STRESS_ABOVE_ACTIVATION_RANGE = "{Selected} is not stressed right now";

				// Token: 0x0400D1E5 RID: 53733
				public static LocString IS_NOT_ANGRY = "{Selected} is too angry";

				// Token: 0x0400D1E6 RID: 53734
				public static LocString IS_NOT_BEING_ATTACKED = "{Selected} is in combat";

				// Token: 0x0400D1E7 RID: 53735
				public static LocString IS_CONSUMPTION_PERMITTED = "Disallowed by consumable permissions";

				// Token: 0x0400D1E8 RID: 53736
				public static LocString CAN_CURE = "No applicable illness";

				// Token: 0x0400D1E9 RID: 53737
				public static LocString TREATMENT_AVAILABLE = "No treatable illness";

				// Token: 0x0400D1EA RID: 53738
				public static LocString DOCTOR_AVAILABLE = "No doctors available\n(Duplicants cannot treat themselves)";

				// Token: 0x0400D1EB RID: 53739
				public static LocString IS_OKAY_TIME_TO_SLEEP = "No current need";

				// Token: 0x0400D1EC RID: 53740
				public static LocString IS_NARCOLEPSING = "{Selected} is currently napping";

				// Token: 0x0400D1ED RID: 53741
				public static LocString IS_FETCH_TARGET_AVAILABLE = "No pending deliveries";

				// Token: 0x0400D1EE RID: 53742
				public static LocString EDIBLE_IS_NOT_NULL = "Consumable Permission not allowed";

				// Token: 0x0400D1EF RID: 53743
				public static LocString HAS_MINGLE_CELL = "Nowhere to Mingle";

				// Token: 0x0400D1F0 RID: 53744
				public static LocString EXCLUSIVELY_AVAILABLE = "Building Already Busy";

				// Token: 0x0400D1F1 RID: 53745
				public static LocString BLADDER_FULL = "Bladder isn't full";

				// Token: 0x0400D1F2 RID: 53746
				public static LocString BLADDER_NOT_FULL = "Bladder too full";

				// Token: 0x0400D1F3 RID: 53747
				public static LocString CURRENTLY_PEEING = "Currently Peeing";

				// Token: 0x0400D1F4 RID: 53748
				public static LocString HAS_BALLOON_STALL_CELL = "Has a location for a Balloon Stall";

				// Token: 0x0400D1F5 RID: 53749
				public static LocString IS_MINION = "Must be a Duplicant";

				// Token: 0x0400D1F6 RID: 53750
				public static LocString IS_ROCKET_TRAVELLING = "Rocket must be travelling";

				// Token: 0x0400D1F7 RID: 53751
				public static LocString REMOTE_CHORE_SUBCHORE_PRECONDITIONS = "No Eligible Remote Chores";

				// Token: 0x0400D1F8 RID: 53752
				public static LocString REMOTE_CHORE_NO_REMOTE_DOCK = "No Dock Assigned";

				// Token: 0x0400D1F9 RID: 53753
				public static LocString REMOTE_CHORE_DOCK_INOPERABLE = "Remote Worker Dock Unusable";

				// Token: 0x0400D1FA RID: 53754
				public static LocString REMOTE_CHORE_NO_REMOTE_WORKER = "No Remote Worker at Dock";

				// Token: 0x0400D1FB RID: 53755
				public static LocString REMOTE_CHORE_DOCK_UNAVAILABLE = "Remote Worker Already Busy";
			}
		}

		// Token: 0x020034A9 RID: 13481
		public class SKILLGROUPS
		{
			// Token: 0x020034AA RID: 13482
			public class MINING
			{
				// Token: 0x0400D1FC RID: 53756
				public static LocString NAME = "Digger";
			}

			// Token: 0x020034AB RID: 13483
			public class BUILDING
			{
				// Token: 0x0400D1FD RID: 53757
				public static LocString NAME = "Builder";
			}

			// Token: 0x020034AC RID: 13484
			public class FARMING
			{
				// Token: 0x0400D1FE RID: 53758
				public static LocString NAME = "Farmer";
			}

			// Token: 0x020034AD RID: 13485
			public class RANCHING
			{
				// Token: 0x0400D1FF RID: 53759
				public static LocString NAME = "Rancher";
			}

			// Token: 0x020034AE RID: 13486
			public class COOKING
			{
				// Token: 0x0400D200 RID: 53760
				public static LocString NAME = "Cooker";
			}

			// Token: 0x020034AF RID: 13487
			public class ART
			{
				// Token: 0x0400D201 RID: 53761
				public static LocString NAME = "Decorator";
			}

			// Token: 0x020034B0 RID: 13488
			public class RESEARCH
			{
				// Token: 0x0400D202 RID: 53762
				public static LocString NAME = "Researcher";
			}

			// Token: 0x020034B1 RID: 13489
			public class SUITS
			{
				// Token: 0x0400D203 RID: 53763
				public static LocString NAME = "Suit Wearer";
			}

			// Token: 0x020034B2 RID: 13490
			public class HAULING
			{
				// Token: 0x0400D204 RID: 53764
				public static LocString NAME = "Supplier";
			}

			// Token: 0x020034B3 RID: 13491
			public class TECHNICALS
			{
				// Token: 0x0400D205 RID: 53765
				public static LocString NAME = "Operator";
			}

			// Token: 0x020034B4 RID: 13492
			public class MEDICALAID
			{
				// Token: 0x0400D206 RID: 53766
				public static LocString NAME = "Doctor";
			}

			// Token: 0x020034B5 RID: 13493
			public class BASEKEEPING
			{
				// Token: 0x0400D207 RID: 53767
				public static LocString NAME = "Tidier";
			}

			// Token: 0x020034B6 RID: 13494
			public class ROCKETRY
			{
				// Token: 0x0400D208 RID: 53768
				public static LocString NAME = "Pilot";
			}
		}

		// Token: 0x020034B7 RID: 13495
		public class CHOREGROUPS
		{
			// Token: 0x020034B8 RID: 13496
			public class ART
			{
				// Token: 0x0400D209 RID: 53769
				public static LocString NAME = "Decorating";

				// Token: 0x0400D20A RID: 53770
				public static LocString DESC = string.Concat(new string[]
				{
					"Sculpt or paint to improve colony ",
					UI.PRE_KEYWORD,
					"Decor",
					UI.PST_KEYWORD,
					"."
				});

				// Token: 0x0400D20B RID: 53771
				public static LocString ARCHETYPE_NAME = "Decorator";
			}

			// Token: 0x020034B9 RID: 13497
			public class COMBAT
			{
				// Token: 0x0400D20C RID: 53772
				public static LocString NAME = "Attacking";

				// Token: 0x0400D20D RID: 53773
				public static LocString DESC = "Fight wild " + UI.FormatAsLink("Critters", "CREATURES") + ".";

				// Token: 0x0400D20E RID: 53774
				public static LocString ARCHETYPE_NAME = "Attacker";
			}

			// Token: 0x020034BA RID: 13498
			public class LIFESUPPORT
			{
				// Token: 0x0400D20F RID: 53775
				public static LocString NAME = "Life Support";

				// Token: 0x0400D210 RID: 53776
				public static LocString DESC = string.Concat(new string[]
				{
					"Maintain ",
					BUILDINGS.PREFABS.ALGAEHABITAT.NAME,
					"s, ",
					BUILDINGS.PREFABS.AIRFILTER.NAME,
					"s, and ",
					BUILDINGS.PREFABS.WATERPURIFIER.NAME,
					"s to support colony life."
				});

				// Token: 0x0400D211 RID: 53777
				public static LocString ARCHETYPE_NAME = "Life Supporter";
			}

			// Token: 0x020034BB RID: 13499
			public class TOGGLE
			{
				// Token: 0x0400D212 RID: 53778
				public static LocString NAME = "Toggling";

				// Token: 0x0400D213 RID: 53779
				public static LocString DESC = "Enable or disable buildings, adjust building settings, and set or flip switches and sensors.";

				// Token: 0x0400D214 RID: 53780
				public static LocString ARCHETYPE_NAME = "Toggler";
			}

			// Token: 0x020034BC RID: 13500
			public class COOK
			{
				// Token: 0x0400D215 RID: 53781
				public static LocString NAME = "Cooking";

				// Token: 0x0400D216 RID: 53782
				public static LocString DESC = string.Concat(new string[]
				{
					"Operate ",
					UI.PRE_KEYWORD,
					"Food",
					UI.PST_KEYWORD,
					" preparation buildings."
				});

				// Token: 0x0400D217 RID: 53783
				public static LocString ARCHETYPE_NAME = "Cooker";
			}

			// Token: 0x020034BD RID: 13501
			public class RESEARCH
			{
				// Token: 0x0400D218 RID: 53784
				public static LocString NAME = "Researching";

				// Token: 0x0400D219 RID: 53785
				public static LocString DESC = string.Concat(new string[]
				{
					"Use ",
					UI.PRE_KEYWORD,
					"Research Stations",
					UI.PST_KEYWORD,
					" to unlock new technologies."
				});

				// Token: 0x0400D21A RID: 53786
				public static LocString ARCHETYPE_NAME = "Researcher";
			}

			// Token: 0x020034BE RID: 13502
			public class REPAIR
			{
				// Token: 0x0400D21B RID: 53787
				public static LocString NAME = "Repairing";

				// Token: 0x0400D21C RID: 53788
				public static LocString DESC = "Repair damaged buildings.";

				// Token: 0x0400D21D RID: 53789
				public static LocString ARCHETYPE_NAME = "Repairer";
			}

			// Token: 0x020034BF RID: 13503
			public class FARMING
			{
				// Token: 0x0400D21E RID: 53790
				public static LocString NAME = "Farming";

				// Token: 0x0400D21F RID: 53791
				public static LocString DESC = string.Concat(new string[]
				{
					"Gather crops from mature ",
					UI.PRE_KEYWORD,
					"Plants",
					UI.PST_KEYWORD,
					"."
				});

				// Token: 0x0400D220 RID: 53792
				public static LocString ARCHETYPE_NAME = "Farmer";
			}

			// Token: 0x020034C0 RID: 13504
			public class RANCHING
			{
				// Token: 0x0400D221 RID: 53793
				public static LocString NAME = "Ranching";

				// Token: 0x0400D222 RID: 53794
				public static LocString DESC = "Tend to domesticated " + UI.FormatAsLink("Critters", "CREATURES") + ".";

				// Token: 0x0400D223 RID: 53795
				public static LocString ARCHETYPE_NAME = "Rancher";
			}

			// Token: 0x020034C1 RID: 13505
			public class BUILD
			{
				// Token: 0x0400D224 RID: 53796
				public static LocString NAME = "Building";

				// Token: 0x0400D225 RID: 53797
				public static LocString DESC = "Construct new buildings.";

				// Token: 0x0400D226 RID: 53798
				public static LocString ARCHETYPE_NAME = "Builder";
			}

			// Token: 0x020034C2 RID: 13506
			public class HAULING
			{
				// Token: 0x0400D227 RID: 53799
				public static LocString NAME = "Supplying";

				// Token: 0x0400D228 RID: 53800
				public static LocString DESC = "Run resources to critical buildings and urgent storage.";

				// Token: 0x0400D229 RID: 53801
				public static LocString ARCHETYPE_NAME = "Supplier";
			}

			// Token: 0x020034C3 RID: 13507
			public class STORAGE
			{
				// Token: 0x0400D22A RID: 53802
				public static LocString NAME = "Storing";

				// Token: 0x0400D22B RID: 53803
				public static LocString DESC = "Fill storage buildings with resources when no other errands are available.";

				// Token: 0x0400D22C RID: 53804
				public static LocString ARCHETYPE_NAME = "Storer";
			}

			// Token: 0x020034C4 RID: 13508
			public class RECREATION
			{
				// Token: 0x0400D22D RID: 53805
				public static LocString NAME = "Relaxing";

				// Token: 0x0400D22E RID: 53806
				public static LocString DESC = "Use leisure facilities, chat with other Duplicants, and relieve Stress.";

				// Token: 0x0400D22F RID: 53807
				public static LocString ARCHETYPE_NAME = "Relaxer";
			}

			// Token: 0x020034C5 RID: 13509
			public class BASEKEEPING
			{
				// Token: 0x0400D230 RID: 53808
				public static LocString NAME = "Tidying";

				// Token: 0x0400D231 RID: 53809
				public static LocString DESC = "Sweep, mop, and disinfect objects within the colony.";

				// Token: 0x0400D232 RID: 53810
				public static LocString ARCHETYPE_NAME = "Tidier";
			}

			// Token: 0x020034C6 RID: 13510
			public class DIG
			{
				// Token: 0x0400D233 RID: 53811
				public static LocString NAME = "Digging";

				// Token: 0x0400D234 RID: 53812
				public static LocString DESC = "Mine raw resources.";

				// Token: 0x0400D235 RID: 53813
				public static LocString ARCHETYPE_NAME = "Digger";
			}

			// Token: 0x020034C7 RID: 13511
			public class MEDICALAID
			{
				// Token: 0x0400D236 RID: 53814
				public static LocString NAME = "Doctoring";

				// Token: 0x0400D237 RID: 53815
				public static LocString DESC = "Treat sick and injured Duplicants.";

				// Token: 0x0400D238 RID: 53816
				public static LocString ARCHETYPE_NAME = "Doctor";
			}

			// Token: 0x020034C8 RID: 13512
			public class MASSAGE
			{
				// Token: 0x0400D239 RID: 53817
				public static LocString NAME = "Relaxing";

				// Token: 0x0400D23A RID: 53818
				public static LocString DESC = "Take breaks for massages.";

				// Token: 0x0400D23B RID: 53819
				public static LocString ARCHETYPE_NAME = "Relaxer";
			}

			// Token: 0x020034C9 RID: 13513
			public class MACHINEOPERATING
			{
				// Token: 0x0400D23C RID: 53820
				public static LocString NAME = "Operating";

				// Token: 0x0400D23D RID: 53821
				public static LocString DESC = "Operating machinery for production, fabrication, and utility purposes.";

				// Token: 0x0400D23E RID: 53822
				public static LocString ARCHETYPE_NAME = "Operator";
			}

			// Token: 0x020034CA RID: 13514
			public class SUITS
			{
				// Token: 0x0400D23F RID: 53823
				public static LocString ARCHETYPE_NAME = "Suit Wearer";
			}

			// Token: 0x020034CB RID: 13515
			public class ROCKETRY
			{
				// Token: 0x0400D240 RID: 53824
				public static LocString NAME = "Rocketry";

				// Token: 0x0400D241 RID: 53825
				public static LocString DESC = "Pilot rockets";

				// Token: 0x0400D242 RID: 53826
				public static LocString ARCHETYPE_NAME = "Pilot";
			}
		}

		// Token: 0x020034CC RID: 13516
		public class STATUSITEMS
		{
			// Token: 0x020034CD RID: 13517
			public class SLIPPERING
			{
				// Token: 0x0400D243 RID: 53827
				public static LocString NAME = "Slipping";

				// Token: 0x0400D244 RID: 53828
				public static LocString TOOLTIP = "This Duplicant is losing their balance on a slippery surface\n\nIt's not fun";
			}

			// Token: 0x020034CE RID: 13518
			public class WAXEDFORTRANSITTUBE
			{
				// Token: 0x0400D245 RID: 53829
				public static LocString NAME = "Smooth Rider";

				// Token: 0x0400D246 RID: 53830
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant slapped on some ",
					ELEMENTS.MILKFAT.NAME,
					" before starting their commute\n\nThis boosts their ",
					BUILDINGS.PREFABS.TRAVELTUBE.NAME,
					" travel speed by {0}"
				});
			}

			// Token: 0x020034CF RID: 13519
			public class ARMINGTRAP
			{
				// Token: 0x0400D247 RID: 53831
				public static LocString NAME = "Arming trap";

				// Token: 0x0400D248 RID: 53832
				public static LocString TOOLTIP = "This Duplicant is arming a trap";
			}

			// Token: 0x020034D0 RID: 13520
			public class GENERIC_DELIVER
			{
				// Token: 0x0400D249 RID: 53833
				public static LocString NAME = "Delivering resources to {Target}";

				// Token: 0x0400D24A RID: 53834
				public static LocString TOOLTIP = "This Duplicant is transporting materials to <b>{Target}</b>";
			}

			// Token: 0x020034D1 RID: 13521
			public class COUGHING
			{
				// Token: 0x0400D24B RID: 53835
				public static LocString NAME = "Yucky Lungs Coughing";

				// Token: 0x0400D24C RID: 53836
				public static LocString TOOLTIP = "Hey! Do that into your elbow\n• Coughing fit was caused by " + DUPLICANTS.MODIFIERS.CONTAMINATEDLUNGS.NAME;
			}

			// Token: 0x020034D2 RID: 13522
			public class WEARING_PAJAMAS
			{
				// Token: 0x0400D24D RID: 53837
				public static LocString NAME = "Wearing " + UI.FormatAsLink("Pajamas", "SLEEP_CLINIC_PAJAMAS");

				// Token: 0x0400D24E RID: 53838
				public static LocString TOOLTIP = "This Duplicant can now produce " + UI.FormatAsLink("Dream Journals", "DREAMJOURNAL") + " when sleeping";
			}

			// Token: 0x020034D3 RID: 13523
			public class DREAMING
			{
				// Token: 0x0400D24F RID: 53839
				public static LocString NAME = "Dreaming";

				// Token: 0x0400D250 RID: 53840
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is adventuring through their own subconscious\n\nDreams are caused by wearing ",
					UI.FormatAsLink("Pajamas", "SLEEP_CLINIC_PAJAMAS"),
					"\n\n",
					UI.FormatAsLink("Dream Journal", "DREAMJOURNAL"),
					" will be ready in {time}"
				});
			}

			// Token: 0x020034D4 RID: 13524
			public class FOSSILHUNT
			{
				// Token: 0x020034D5 RID: 13525
				public class WORKEREXCAVATING
				{
					// Token: 0x0400D251 RID: 53841
					public static LocString NAME = "Excavating Fossil";

					// Token: 0x0400D252 RID: 53842
					public static LocString TOOLTIP = "This Duplicant is carefully uncovering a " + UI.FormatAsLink("Fossil", "FOSSIL");
				}
			}

			// Token: 0x020034D6 RID: 13526
			public class SLEEPING
			{
				// Token: 0x0400D253 RID: 53843
				public static LocString NAME = "Sleeping";

				// Token: 0x0400D254 RID: 53844
				public static LocString TOOLTIP = "This Duplicant is recovering stamina";

				// Token: 0x0400D255 RID: 53845
				public static LocString TOOLTIP_DISTURBER = "\n\nThey were sleeping peacefully until they were disturbed by <b>{Disturber}</b>";
			}

			// Token: 0x020034D7 RID: 13527
			public class SLEEPINGEXHAUSTED
			{
				// Token: 0x0400D256 RID: 53846
				public static LocString NAME = "Unscheduled Nap";

				// Token: 0x0400D257 RID: 53847
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Cold ",
					UI.PRE_KEYWORD,
					"Temperatures",
					UI.PST_KEYWORD,
					" or lack of rest depleted this Duplicant's ",
					UI.PRE_KEYWORD,
					"Stamina",
					UI.PST_KEYWORD,
					"\n\nThey didn't have enough energy to make it to bedtime"
				});
			}

			// Token: 0x020034D8 RID: 13528
			public class SLEEPINGPEACEFULLY
			{
				// Token: 0x0400D258 RID: 53848
				public static LocString NAME = "Sleeping peacefully";

				// Token: 0x0400D259 RID: 53849
				public static LocString TOOLTIP = "This Duplicant is getting well-deserved, quality sleep\n\nAt this rate they're sure to feel " + UI.FormatAsLink("Well Rested", "SLEEP") + " tomorrow morning";
			}

			// Token: 0x020034D9 RID: 13529
			public class SLEEPINGBADLY
			{
				// Token: 0x0400D25A RID: 53850
				public static LocString NAME = "Sleeping badly";

				// Token: 0x0400D25B RID: 53851
				public static LocString TOOLTIP = "This Duplicant's having trouble falling asleep due to noise from <b>{Disturber}</b>\n\nThey're going to feel a bit " + UI.FormatAsLink("Unrested", "SLEEP") + " tomorrow morning";
			}

			// Token: 0x020034DA RID: 13530
			public class SLEEPINGTERRIBLY
			{
				// Token: 0x0400D25C RID: 53852
				public static LocString NAME = "Can't sleep";

				// Token: 0x0400D25D RID: 53853
				public static LocString TOOLTIP = "This Duplicant was woken up by noise from <b>{Disturber}</b> and can't get back to sleep\n\nThey're going to feel " + UI.FormatAsLink("Dead Tired", "SLEEP") + " tomorrow morning";
			}

			// Token: 0x020034DB RID: 13531
			public class SLEEPINGINTERRUPTEDBYLIGHT
			{
				// Token: 0x0400D25E RID: 53854
				public static LocString NAME = "Interrupted Sleep: Bright Light";

				// Token: 0x0400D25F RID: 53855
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant can't sleep because the ",
					UI.PRE_KEYWORD,
					"Lights",
					UI.PST_KEYWORD,
					" are still on"
				});
			}

			// Token: 0x020034DC RID: 13532
			public class SLEEPINGINTERRUPTEDBYNOISE
			{
				// Token: 0x0400D260 RID: 53856
				public static LocString NAME = "Interrupted Sleep: Snoring Friend";

				// Token: 0x0400D261 RID: 53857
				public static LocString TOOLTIP = "This Duplicant is having trouble sleeping thanks to a certain noisy someone";
			}

			// Token: 0x020034DD RID: 13533
			public class SLEEPINGINTERRUPTEDBYFEAROFDARK
			{
				// Token: 0x0400D262 RID: 53858
				public static LocString NAME = "Interrupted Sleep: Afraid of Dark";

				// Token: 0x0400D263 RID: 53859
				public static LocString TOOLTIP = "This Duplicant is having trouble sleeping because of their fear of the dark";
			}

			// Token: 0x020034DE RID: 13534
			public class SLEEPINGINTERRUPTEDBYMOVEMENT
			{
				// Token: 0x0400D264 RID: 53860
				public static LocString NAME = "Interrupted Sleep: Bed Jostling";

				// Token: 0x0400D265 RID: 53861
				public static LocString TOOLTIP = "This Duplicant was woken up because their bed was moved";
			}

			// Token: 0x020034DF RID: 13535
			public class SLEEPINGINTERRUPTEDBYCOLD
			{
				// Token: 0x0400D266 RID: 53862
				public static LocString NAME = "Interrupted Sleep: Cold Room";

				// Token: 0x0400D267 RID: 53863
				public static LocString TOOLTIP = "This Duplicant is having trouble sleeping because this room is too cold";
			}

			// Token: 0x020034E0 RID: 13536
			public class REDALERT
			{
				// Token: 0x0400D268 RID: 53864
				public static LocString NAME = "Red Alert!";

				// Token: 0x0400D269 RID: 53865
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"The colony is in a state of ",
					UI.PRE_KEYWORD,
					"Red Alert",
					UI.PST_KEYWORD,
					". Duplicants will not eat, sleep, use the bathroom, or engage in leisure activities while the ",
					UI.PRE_KEYWORD,
					"Red Alert",
					UI.PST_KEYWORD,
					" is active"
				});
			}

			// Token: 0x020034E1 RID: 13537
			public class ROLE
			{
				// Token: 0x0400D26A RID: 53866
				public static LocString NAME = "{Role}: {Progress} Mastery";

				// Token: 0x0400D26B RID: 53867
				public static LocString TOOLTIP = "This Duplicant is working as a <b>{Role}</b>\n\nThey have <b>{Progress}</b> mastery of this job";
			}

			// Token: 0x020034E2 RID: 13538
			public class LOWOXYGEN
			{
				// Token: 0x0400D26C RID: 53868
				public static LocString NAME = "Oxygen low";

				// Token: 0x0400D26D RID: 53869
				public static LocString TOOLTIP = "This Duplicant is working in a low breathability area";

				// Token: 0x0400D26E RID: 53870
				public static LocString NOTIFICATION_NAME = "Low " + ELEMENTS.OXYGEN.NAME + " area entered";

				// Token: 0x0400D26F RID: 53871
				public static LocString NOTIFICATION_TOOLTIP = "These Duplicants are working in areas with low " + ELEMENTS.OXYGEN.NAME + ":";
			}

			// Token: 0x020034E3 RID: 13539
			public class SEVEREWOUNDS
			{
				// Token: 0x0400D270 RID: 53872
				public static LocString NAME = "Severely injured";

				// Token: 0x0400D271 RID: 53873
				public static LocString TOOLTIP = "This Duplicant is badly hurt";

				// Token: 0x0400D272 RID: 53874
				public static LocString NOTIFICATION_NAME = "Severely injured";

				// Token: 0x0400D273 RID: 53875
				public static LocString NOTIFICATION_TOOLTIP = "These Duplicants are badly hurt and require medical attention";
			}

			// Token: 0x020034E4 RID: 13540
			public class INCAPACITATED
			{
				// Token: 0x0400D274 RID: 53876
				public static LocString NAME = "Incapacitated: {CauseOfIncapacitation}\nTime until death: {TimeUntilDeath}\n";

				// Token: 0x0400D275 RID: 53877
				public static LocString TOOLTIP = "This Duplicant is near death!\n\nAssign them to a Triage Cot for rescue";

				// Token: 0x0400D276 RID: 53878
				public static LocString NOTIFICATION_NAME = "Incapacitated";

				// Token: 0x0400D277 RID: 53879
				public static LocString NOTIFICATION_TOOLTIP = "These Duplicants are near death.\nA " + BUILDINGS.PREFABS.MEDICALCOT.NAME + " is required for rescue:";
			}

			// Token: 0x020034E5 RID: 13541
			public class BIONICOFFLINEINCAPACITATED
			{
				// Token: 0x0400D278 RID: 53880
				public static LocString NAME = "Incapacitated: Powerless";

				// Token: 0x0400D279 RID: 53881
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is non-functional!\n\nDeliver a charged ",
					UI.PRE_KEYWORD,
					"Power Bank",
					UI.PST_KEYWORD,
					" and reboot their systems to revive them"
				});

				// Token: 0x0400D27A RID: 53882
				public static LocString NOTIFICATION_NAME = "Bionic Duplicant Incapacitated";

				// Token: 0x0400D27B RID: 53883
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"These Bionic Duplicants are non-functional.\n\nA charged ",
					UI.PRE_KEYWORD,
					"Power Bank",
					UI.PST_KEYWORD,
					" and full reboot by a skilled Duplicant are required for rescue:"
				});
			}

			// Token: 0x020034E6 RID: 13542
			public class BIONICWANTSOILCHANGE
			{
				// Token: 0x0400D27C RID: 53884
				public static LocString NAME = "Low Oil";

				// Token: 0x0400D27D RID: 53885
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is almost out of ",
					UI.PRE_KEYWORD,
					"Oil",
					UI.PST_KEYWORD,
					" and needs to visit a ",
					UI.PRE_KEYWORD,
					"Lubrication Station",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x020034E7 RID: 13543
			public class BIONICWAITINGFORREBOOT
			{
				// Token: 0x0400D27E RID: 53886
				public static LocString NAME = "Awaiting Reboot";

				// Token: 0x0400D27F RID: 53887
				public static LocString TOOLTIP = "This Duplicant needs someone to reboot their bionic systems so they can get back to work";
			}

			// Token: 0x020034E8 RID: 13544
			public class BIONICBEINGREBOOTED
			{
				// Token: 0x0400D280 RID: 53888
				public static LocString NAME = "Reboot in progress";

				// Token: 0x0400D281 RID: 53889
				public static LocString TOOLTIP = "This Duplicant's bionic systems are being rebooted";
			}

			// Token: 0x020034E9 RID: 13545
			public class BIONICREQUIRESSKILLPERK
			{
				// Token: 0x0400D282 RID: 53890
				public static LocString NAME = "Skill-Required Operation";

				// Token: 0x0400D283 RID: 53891
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Only Duplicants with the following ",
					UI.PRE_KEYWORD,
					"Skills",
					UI.PST_KEYWORD,
					" can reboot this Duplicant's bionic systems:\n\n{Skills}"
				});
			}

			// Token: 0x020034EA RID: 13546
			public class BEDUNREACHABLE
			{
				// Token: 0x0400D284 RID: 53892
				public static LocString NAME = "Cannot reach bed";

				// Token: 0x0400D285 RID: 53893
				public static LocString TOOLTIP = "This Duplicant cannot reach their bed";

				// Token: 0x0400D286 RID: 53894
				public static LocString NOTIFICATION_NAME = "Unreachable bed";

				// Token: 0x0400D287 RID: 53895
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"These Duplicants cannot sleep because their ",
					UI.PRE_KEYWORD,
					"Beds",
					UI.PST_KEYWORD,
					" are beyond their reach:"
				});
			}

			// Token: 0x020034EB RID: 13547
			public class COLD
			{
				// Token: 0x0400D288 RID: 53896
				public static LocString NAME = "Chilly surroundings";

				// Token: 0x0400D289 RID: 53897
				public static LocString TOOLTIP = "This Duplicant cannot retain enough heat to stay warm and may be under-insulated for this area\n\nThey will begin to recover shortly after they leave this area\n\nStress: <b>{StressModification}</b>\nStamina: <b>{StaminaModification}</b>\nAthletics: <b>{AthleticsModification}</b>\n\nCurrent Environmental Exchange: <b>{currentTransferWattage}</b>\n\nInsulation Thickness: {conductivityBarrier}";
			}

			// Token: 0x020034EC RID: 13548
			public class EXITINGCOLD
			{
				// Token: 0x0400D28A RID: 53898
				public static LocString NAME = "Shivering";

				// Token: 0x0400D28B RID: 53899
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant was recently exposed to cold ",
					UI.PRE_KEYWORD,
					"Temperatures",
					UI.PST_KEYWORD,
					" and wants to warm up\n\nWithout a warming station, it will take {0} for them to recover\n\nStress: <b>{StressModification}</b>\nStamina: <b>{StaminaModification}</b>\nAthletics: <b>{AthleticsModification}</b>"
				});
			}

			// Token: 0x020034ED RID: 13549
			public class DAILYRATIONLIMITREACHED
			{
				// Token: 0x0400D28C RID: 53900
				public static LocString NAME = "Daily calorie limit reached";

				// Token: 0x0400D28D RID: 53901
				public static LocString TOOLTIP = "This Duplicant has consumed their allotted " + UI.FormatAsLink("Rations", "FOOD") + " for the day";

				// Token: 0x0400D28E RID: 53902
				public static LocString NOTIFICATION_NAME = "Daily calorie limit reached";

				// Token: 0x0400D28F RID: 53903
				public static LocString NOTIFICATION_TOOLTIP = "These Duplicants have consumed their allotted " + UI.FormatAsLink("Rations", "FOOD") + " for the day:";
			}

			// Token: 0x020034EE RID: 13550
			public class DOCTOR
			{
				// Token: 0x0400D290 RID: 53904
				public static LocString NAME = "Treating Patient";

				// Token: 0x0400D291 RID: 53905
				public static LocString STATUS = "This Duplicant is going to administer medical care to an ailing friend";
			}

			// Token: 0x020034EF RID: 13551
			public class HOLDINGBREATH
			{
				// Token: 0x0400D292 RID: 53906
				public static LocString NAME = "Holding breath";

				// Token: 0x0400D293 RID: 53907
				public static LocString TOOLTIP = "This Duplicant cannot breathe in their current location";
			}

			// Token: 0x020034F0 RID: 13552
			public class RECOVERINGBREATH
			{
				// Token: 0x0400D294 RID: 53908
				public static LocString NAME = "Recovering breath";

				// Token: 0x0400D295 RID: 53909
				public static LocString TOOLTIP = "This Duplicant held their breath too long and needs a moment";
			}

			// Token: 0x020034F1 RID: 13553
			public class HOT
			{
				// Token: 0x0400D296 RID: 53910
				public static LocString NAME = "Toasty surroundings";

				// Token: 0x0400D297 RID: 53911
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant cannot let off enough ",
					UI.PRE_KEYWORD,
					"Heat",
					UI.PST_KEYWORD,
					" to stay cool and may be over-insulated for this area\n\nThey will begin to recover shortly after they leave this area\n\nStress Modification: <b>{StressModification}</b>\nStamina: <b>{StaminaModification}</b>\nAthletics: <b>{AthleticsModification}</b>\n\nCurrent Environmental Exchange: <b>{currentTransferWattage}</b>\n\nInsulation Thickness: {conductivityBarrier}"
				});
			}

			// Token: 0x020034F2 RID: 13554
			public class EXITINGHOT
			{
				// Token: 0x0400D298 RID: 53912
				public static LocString NAME = "Sweaty";

				// Token: 0x0400D299 RID: 53913
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant was recently exposed to hot ",
					UI.PRE_KEYWORD,
					"Temperatures",
					UI.PST_KEYWORD,
					" and wants to cool down\n\nWithout a cooling station, it will take {0} for them to recover\n\nStress: <b>{StressModification}</b>\nStamina: <b>{StaminaModification}</b>\nAthletics: <b>{AthleticsModification}</b>"
				});
			}

			// Token: 0x020034F3 RID: 13555
			public class HUNGRY
			{
				// Token: 0x0400D29A RID: 53914
				public static LocString NAME = "Hungry";

				// Token: 0x0400D29B RID: 53915
				public static LocString TOOLTIP = "This Duplicant would really like something to eat";
			}

			// Token: 0x020034F4 RID: 13556
			public class POORDECOR
			{
				// Token: 0x0400D29C RID: 53916
				public static LocString NAME = "Drab decor";

				// Token: 0x0400D29D RID: 53917
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is depressed by the lack of ",
					UI.PRE_KEYWORD,
					"Decor",
					UI.PST_KEYWORD,
					" in this area"
				});
			}

			// Token: 0x020034F5 RID: 13557
			public class POORQUALITYOFLIFE
			{
				// Token: 0x0400D29E RID: 53918
				public static LocString NAME = "Low Morale";

				// Token: 0x0400D29F RID: 53919
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"The bad in this Duplicant's life is starting to outweigh the good\n\nImproved amenities and additional ",
					UI.PRE_KEYWORD,
					"Downtime",
					UI.PST_KEYWORD,
					" would help improve their ",
					UI.PRE_KEYWORD,
					"Morale",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x020034F6 RID: 13558
			public class POOR_FOOD_QUALITY
			{
				// Token: 0x0400D2A0 RID: 53920
				public static LocString NAME = "Lousy Meal";

				// Token: 0x0400D2A1 RID: 53921
				public static LocString TOOLTIP = "The last meal this Duplicant ate didn't quite meet their expectations";
			}

			// Token: 0x020034F7 RID: 13559
			public class GOOD_FOOD_QUALITY
			{
				// Token: 0x0400D2A2 RID: 53922
				public static LocString NAME = "Decadent Meal";

				// Token: 0x0400D2A3 RID: 53923
				public static LocString TOOLTIP = "The last meal this Duplicant ate exceeded their expectations!";
			}

			// Token: 0x020034F8 RID: 13560
			public class NERVOUSBREAKDOWN
			{
				// Token: 0x0400D2A4 RID: 53924
				public static LocString NAME = "Nervous breakdown";

				// Token: 0x0400D2A5 RID: 53925
				public static LocString TOOLTIP = UI.PRE_KEYWORD + "Stress" + UI.PST_KEYWORD + " has completely eroded this Duplicant's ability to function";

				// Token: 0x0400D2A6 RID: 53926
				public static LocString NOTIFICATION_NAME = "Nervous breakdown";

				// Token: 0x0400D2A7 RID: 53927
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"These Duplicants have cracked under the ",
					UI.PRE_KEYWORD,
					"Stress",
					UI.PST_KEYWORD,
					" and need assistance:"
				});
			}

			// Token: 0x020034F9 RID: 13561
			public class STRESSED
			{
				// Token: 0x0400D2A8 RID: 53928
				public static LocString NAME = "Stressed";

				// Token: 0x0400D2A9 RID: 53929
				public static LocString TOOLTIP = "This Duplicant is feeling the pressure";

				// Token: 0x0400D2AA RID: 53930
				public static LocString NOTIFICATION_NAME = "High stress";

				// Token: 0x0400D2AB RID: 53931
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"These Duplicants are ",
					UI.PRE_KEYWORD,
					"Stressed",
					UI.PST_KEYWORD,
					" and need to unwind:"
				});
			}

			// Token: 0x020034FA RID: 13562
			public class NORATIONSAVAILABLE
			{
				// Token: 0x0400D2AC RID: 53932
				public static LocString NAME = "No food available";

				// Token: 0x0400D2AD RID: 53933
				public static LocString TOOLTIP = "There's nothing in the colony for this Duplicant to eat";

				// Token: 0x0400D2AE RID: 53934
				public static LocString NOTIFICATION_NAME = "No food available";

				// Token: 0x0400D2AF RID: 53935
				public static LocString NOTIFICATION_TOOLTIP = "These Duplicants have nothing to eat:";
			}

			// Token: 0x020034FB RID: 13563
			public class QUARANTINEAREAUNREACHABLE
			{
				// Token: 0x0400D2B0 RID: 53936
				public static LocString NAME = "Cannot reach quarantine";

				// Token: 0x0400D2B1 RID: 53937
				public static LocString TOOLTIP = "This Duplicant cannot reach their quarantine zone";

				// Token: 0x0400D2B2 RID: 53938
				public static LocString NOTIFICATION_NAME = "Unreachable quarantine";

				// Token: 0x0400D2B3 RID: 53939
				public static LocString NOTIFICATION_TOOLTIP = "These Duplicants cannot reach their assigned quarantine zones:";
			}

			// Token: 0x020034FC RID: 13564
			public class QUARANTINED
			{
				// Token: 0x0400D2B4 RID: 53940
				public static LocString NAME = "Quarantined";

				// Token: 0x0400D2B5 RID: 53941
				public static LocString TOOLTIP = "This Duplicant has been isolated from the colony";
			}

			// Token: 0x020034FD RID: 13565
			public class RATIONSUNREACHABLE
			{
				// Token: 0x0400D2B6 RID: 53942
				public static LocString NAME = "Cannot reach food";

				// Token: 0x0400D2B7 RID: 53943
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"There is ",
					UI.PRE_KEYWORD,
					"Food",
					UI.PST_KEYWORD,
					" in the colony that this Duplicant cannot reach"
				});

				// Token: 0x0400D2B8 RID: 53944
				public static LocString NOTIFICATION_NAME = "Unreachable food";

				// Token: 0x0400D2B9 RID: 53945
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"These Duplicants cannot access the colony's ",
					UI.PRE_KEYWORD,
					"Food",
					UI.PST_KEYWORD,
					":"
				});
			}

			// Token: 0x020034FE RID: 13566
			public class RATIONSNOTPERMITTED
			{
				// Token: 0x0400D2BA RID: 53946
				public static LocString NAME = "Food Type Not Permitted";

				// Token: 0x0400D2BB RID: 53947
				public static LocString TOOLTIP = "This Duplicant is not allowed to eat any of the " + UI.FormatAsLink("Food", "FOOD") + " in their reach\n\nEnter the <color=#833A5FFF>CONSUMABLES</color> <color=#F44A47><b>[F]</b></color> to adjust their food permissions";

				// Token: 0x0400D2BC RID: 53948
				public static LocString NOTIFICATION_NAME = "Unpermitted food";

				// Token: 0x0400D2BD RID: 53949
				public static LocString NOTIFICATION_TOOLTIP = "These Duplicants' <color=#833A5FFF>CONSUMABLES</color> <color=#F44A47><b>[F]</b></color> permissions prevent them from eating any of the " + UI.FormatAsLink("Food", "FOOD") + " within their reach:";
			}

			// Token: 0x020034FF RID: 13567
			public class ROTTEN
			{
				// Token: 0x0400D2BE RID: 53950
				public static LocString NAME = "Rotten";

				// Token: 0x0400D2BF RID: 53951
				public static LocString TOOLTIP = "Gross!";
			}

			// Token: 0x02003500 RID: 13568
			public class STARVING
			{
				// Token: 0x0400D2C0 RID: 53952
				public static LocString NAME = "Starving";

				// Token: 0x0400D2C1 RID: 53953
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is about to die and needs ",
					UI.PRE_KEYWORD,
					"Food",
					UI.PST_KEYWORD,
					"!"
				});

				// Token: 0x0400D2C2 RID: 53954
				public static LocString NOTIFICATION_NAME = "Starvation";

				// Token: 0x0400D2C3 RID: 53955
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"These Duplicants are starving and will die if they can't find ",
					UI.PRE_KEYWORD,
					"Food",
					UI.PST_KEYWORD,
					":"
				});
			}

			// Token: 0x02003501 RID: 13569
			public class STRESS_SIGNAL_AGGRESIVE
			{
				// Token: 0x0400D2C4 RID: 53956
				public static LocString NAME = "Frustrated";

				// Token: 0x0400D2C5 RID: 53957
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is trying to keep their cool\n\nImprove this Duplicant's ",
					UI.PRE_KEYWORD,
					"Morale",
					UI.PST_KEYWORD,
					" before they destroy something to let off steam"
				});
			}

			// Token: 0x02003502 RID: 13570
			public class STRESS_SIGNAL_BINGE_EAT
			{
				// Token: 0x0400D2C6 RID: 53958
				public static LocString NAME = "Stress Cravings";

				// Token: 0x0400D2C7 RID: 53959
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is consumed by hunger\n\nImprove this Duplicant's ",
					UI.PRE_KEYWORD,
					"Morale",
					UI.PST_KEYWORD,
					" before they eat all the colony's ",
					UI.PRE_KEYWORD,
					"Food",
					UI.PST_KEYWORD,
					" stores"
				});
			}

			// Token: 0x02003503 RID: 13571
			public class STRESS_SIGNAL_UGLY_CRIER
			{
				// Token: 0x0400D2C8 RID: 53960
				public static LocString NAME = "Misty Eyed";

				// Token: 0x0400D2C9 RID: 53961
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is trying and failing to swallow their emotions\n\nImprove this Duplicant's ",
					UI.PRE_KEYWORD,
					"Morale",
					UI.PST_KEYWORD,
					" before they have a good ugly cry"
				});
			}

			// Token: 0x02003504 RID: 13572
			public class STRESS_SIGNAL_VOMITER
			{
				// Token: 0x0400D2CA RID: 53962
				public static LocString NAME = "Stress Burp";

				// Token: 0x0400D2CB RID: 53963
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Sort of like having butterflies in your stomach, except they're burps\n\nImprove this Duplicant's ",
					UI.PRE_KEYWORD,
					"Morale",
					UI.PST_KEYWORD,
					" before they start to stress vomit"
				});
			}

			// Token: 0x02003505 RID: 13573
			public class STRESS_SIGNAL_BANSHEE
			{
				// Token: 0x0400D2CC RID: 53964
				public static LocString NAME = "Suppressed Screams";

				// Token: 0x0400D2CD RID: 53965
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is fighting the urge to scream\n\nImprove this Duplicant's ",
					UI.PRE_KEYWORD,
					"Morale",
					UI.PST_KEYWORD,
					" before they start wailing uncontrollably"
				});
			}

			// Token: 0x02003506 RID: 13574
			public class STRESS_SIGNAL_STRESS_SHOCKER
			{
				// Token: 0x0400D2CE RID: 53966
				public static LocString NAME = "Dangerously Frayed";

				// Token: 0x0400D2CF RID: 53967
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant's hanging by a thread...except the thread is a live wire\n\nImprove this Duplicant's ",
					UI.PRE_KEYWORD,
					"Morale",
					UI.PST_KEYWORD,
					" before they zap someone"
				});
			}

			// Token: 0x02003507 RID: 13575
			public class ENTOMBEDCHORE
			{
				// Token: 0x0400D2D0 RID: 53968
				public static LocString NAME = "Entombed";

				// Token: 0x0400D2D1 RID: 53969
				public static LocString TOOLTIP = "This Duplicant needs someone to help dig them out!";

				// Token: 0x0400D2D2 RID: 53970
				public static LocString NOTIFICATION_NAME = "Entombed";

				// Token: 0x0400D2D3 RID: 53971
				public static LocString NOTIFICATION_TOOLTIP = "These Duplicants are trapped:";
			}

			// Token: 0x02003508 RID: 13576
			public class EARLYMORNING
			{
				// Token: 0x0400D2D4 RID: 53972
				public static LocString NAME = "Early Bird";

				// Token: 0x0400D2D5 RID: 53973
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is jazzed to start the day\n• All ",
					UI.PRE_KEYWORD,
					"Attributes",
					UI.PST_KEYWORD,
					" <b>+2</b> in the morning"
				});
			}

			// Token: 0x02003509 RID: 13577
			public class NIGHTTIME
			{
				// Token: 0x0400D2D6 RID: 53974
				public static LocString NAME = "Night Owl";

				// Token: 0x0400D2D7 RID: 53975
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is more efficient on a nighttime ",
					UI.PRE_KEYWORD,
					"Schedule",
					UI.PST_KEYWORD,
					"\n• All ",
					UI.PRE_KEYWORD,
					"Attributes",
					UI.PST_KEYWORD,
					" <b>+3</b> at night"
				});
			}

			// Token: 0x0200350A RID: 13578
			public class METEORPHILE
			{
				// Token: 0x0400D2D8 RID: 53976
				public static LocString NAME = "Rock Fan";

				// Token: 0x0400D2D9 RID: 53977
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is <i>really</i> into meteor showers\n• All ",
					UI.PRE_KEYWORD,
					"Attributes",
					UI.PST_KEYWORD,
					" <b>+3</b> during meteor showers"
				});
			}

			// Token: 0x0200350B RID: 13579
			public class SUFFOCATING
			{
				// Token: 0x0400D2DA RID: 53978
				public static LocString NAME = "Suffocating";

				// Token: 0x0400D2DB RID: 53979
				public static LocString TOOLTIP = "This Duplicant cannot breathe!";

				// Token: 0x0400D2DC RID: 53980
				public static LocString NOTIFICATION_NAME = "Suffocating";

				// Token: 0x0400D2DD RID: 53981
				public static LocString NOTIFICATION_TOOLTIP = "These Duplicants cannot breathe:";
			}

			// Token: 0x0200350C RID: 13580
			public class TIRED
			{
				// Token: 0x0400D2DE RID: 53982
				public static LocString NAME = "Tired";

				// Token: 0x0400D2DF RID: 53983
				public static LocString TOOLTIP = "This Duplicant could use a nice nap";
			}

			// Token: 0x0200350D RID: 13581
			public class IDLE
			{
				// Token: 0x0400D2E0 RID: 53984
				public static LocString NAME = "Idle";

				// Token: 0x0400D2E1 RID: 53985
				public static LocString TOOLTIP = "This Duplicant cannot reach any pending errands";

				// Token: 0x0400D2E2 RID: 53986
				public static LocString NOTIFICATION_NAME = "Idle";

				// Token: 0x0400D2E3 RID: 53987
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"These Duplicants cannot reach any pending ",
					UI.PRE_KEYWORD,
					"Errands",
					UI.PST_KEYWORD,
					":"
				});
			}

			// Token: 0x0200350E RID: 13582
			public class IDLEINROCKETS
			{
				// Token: 0x0400D2E4 RID: 53988
				public static LocString NAME = "Idle";

				// Token: 0x0400D2E5 RID: 53989
				public static LocString TOOLTIP = "This Duplicant cannot reach any pending errands";

				// Token: 0x0400D2E6 RID: 53990
				public static LocString NOTIFICATION_NAME = "Idle";

				// Token: 0x0400D2E7 RID: 53991
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"These Duplicants cannot reach any pending ",
					UI.PRE_KEYWORD,
					"Errands",
					UI.PST_KEYWORD,
					":"
				});
			}

			// Token: 0x0200350F RID: 13583
			public class FIGHTING
			{
				// Token: 0x0400D2E8 RID: 53992
				public static LocString NAME = "In combat";

				// Token: 0x0400D2E9 RID: 53993
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is attacking a ",
					UI.PRE_KEYWORD,
					"Critter",
					UI.PST_KEYWORD,
					"!"
				});

				// Token: 0x0400D2EA RID: 53994
				public static LocString NOTIFICATION_NAME = "Combat!";

				// Token: 0x0400D2EB RID: 53995
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"These Duplicants have engaged a ",
					UI.PRE_KEYWORD,
					"Critter",
					UI.PST_KEYWORD,
					" in combat:"
				});
			}

			// Token: 0x02003510 RID: 13584
			public class FLEEING
			{
				// Token: 0x0400D2EC RID: 53996
				public static LocString NAME = "Fleeing";

				// Token: 0x0400D2ED RID: 53997
				public static LocString TOOLTIP = "This Duplicant is trying to escape something scary!";

				// Token: 0x0400D2EE RID: 53998
				public static LocString NOTIFICATION_NAME = "Fleeing!";

				// Token: 0x0400D2EF RID: 53999
				public static LocString NOTIFICATION_TOOLTIP = "These Duplicants are trying to escape:";
			}

			// Token: 0x02003511 RID: 13585
			public class DEAD
			{
				// Token: 0x0400D2F0 RID: 54000
				public static LocString NAME = "Dead: {Death}";

				// Token: 0x0400D2F1 RID: 54001
				public static LocString TOOLTIP = "This Duplicant definitely isn't sleeping";
			}

			// Token: 0x02003512 RID: 13586
			public class LASHINGOUT
			{
				// Token: 0x0400D2F2 RID: 54002
				public static LocString NAME = "Lashing out";

				// Token: 0x0400D2F3 RID: 54003
				public static LocString TOOLTIP = "This Duplicant is breaking buildings to relieve their " + UI.PRE_KEYWORD + "Stress" + UI.PST_KEYWORD;

				// Token: 0x0400D2F4 RID: 54004
				public static LocString NOTIFICATION_NAME = "Lashing out";

				// Token: 0x0400D2F5 RID: 54005
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"These Duplicants broke buildings to relieve their ",
					UI.PRE_KEYWORD,
					"Stress",
					UI.PST_KEYWORD,
					":"
				});
			}

			// Token: 0x02003513 RID: 13587
			public class MOVETOSUITNOTREQUIRED
			{
				// Token: 0x0400D2F6 RID: 54006
				public static LocString NAME = "Exiting Exosuit area";

				// Token: 0x0400D2F7 RID: 54007
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is leaving an area where a ",
					UI.PRE_KEYWORD,
					"Suit",
					UI.PST_KEYWORD,
					" was required"
				});
			}

			// Token: 0x02003514 RID: 13588
			public class NOROLE
			{
				// Token: 0x0400D2F8 RID: 54008
				public static LocString NAME = "No Job";

				// Token: 0x0400D2F9 RID: 54009
				public static LocString TOOLTIP = "This Duplicant does not have a Job Assignment\n\nEnter the " + UI.FormatAsManagementMenu("Jobs Panel", "[J]") + " to view all available Jobs";
			}

			// Token: 0x02003515 RID: 13589
			public class DROPPINGUNUSEDINVENTORY
			{
				// Token: 0x0400D2FA RID: 54010
				public static LocString NAME = "Dropping objects";

				// Token: 0x0400D2FB RID: 54011
				public static LocString TOOLTIP = "This Duplicant is dropping what they're holding";
			}

			// Token: 0x02003516 RID: 13590
			public class MOVINGTOSAFEAREA
			{
				// Token: 0x0400D2FC RID: 54012
				public static LocString NAME = "Moving to safe area";

				// Token: 0x0400D2FD RID: 54013
				public static LocString TOOLTIP = "This Duplicant is finding a less dangerous place";
			}

			// Token: 0x02003517 RID: 13591
			public class TOILETUNREACHABLE
			{
				// Token: 0x0400D2FE RID: 54014
				public static LocString NAME = "Unreachable toilet";

				// Token: 0x0400D2FF RID: 54015
				public static LocString TOOLTIP = "This Duplicant cannot reach a functioning " + UI.FormatAsLink("Outhouse", "OUTHOUSE") + " or " + UI.FormatAsLink("Lavatory", "FLUSHTOILET");

				// Token: 0x0400D300 RID: 54016
				public static LocString NOTIFICATION_NAME = "Unreachable toilet";

				// Token: 0x0400D301 RID: 54017
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"These Duplicants cannot reach a functioning ",
					UI.FormatAsLink("Outhouse", "OUTHOUSE"),
					" or ",
					UI.FormatAsLink("Lavatory", "FLUSHTOILET"),
					":"
				});
			}

			// Token: 0x02003518 RID: 13592
			public class NOUSABLETOILETS
			{
				// Token: 0x0400D302 RID: 54018
				public static LocString NAME = "Toilet out of order";

				// Token: 0x0400D303 RID: 54019
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"The only ",
					UI.FormatAsLink("Outhouses", "OUTHOUSE"),
					" or ",
					UI.FormatAsLink("Lavatories", "FLUSHTOILET"),
					" in this Duplicant's reach are out of order"
				});

				// Token: 0x0400D304 RID: 54020
				public static LocString NOTIFICATION_NAME = "Toilet out of order";

				// Token: 0x0400D305 RID: 54021
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"These Duplicants want to use an ",
					UI.FormatAsLink("Outhouse", "OUTHOUSE"),
					" or ",
					UI.FormatAsLink("Lavatory", "FLUSHTOILET"),
					" that is out of order:"
				});
			}

			// Token: 0x02003519 RID: 13593
			public class NOTOILETS
			{
				// Token: 0x0400D306 RID: 54022
				public static LocString NAME = "No Outhouses";

				// Token: 0x0400D307 RID: 54023
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"There are no ",
					UI.FormatAsLink("Outhouses", "OUTHOUSE"),
					" available for this Duplicant\n\n",
					UI.FormatAsLink("Outhouses", "OUTHOUSE"),
					" can be built from the ",
					UI.FormatAsBuildMenuTab("Plumbing Tab", global::Action.Plan5)
				});

				// Token: 0x0400D308 RID: 54024
				public static LocString NOTIFICATION_NAME = "No Outhouses built";

				// Token: 0x0400D309 RID: 54025
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					UI.FormatAsLink("Outhouses", "OUTHOUSE"),
					" can be built from the ",
					UI.FormatAsBuildMenuTab("Plumbing Tab", global::Action.Plan5),
					".\n\nThese Duplicants are in need of an ",
					UI.FormatAsLink("Outhouse", "OUTHOUSE"),
					":"
				});
			}

			// Token: 0x0200351A RID: 13594
			public class FULLBLADDER
			{
				// Token: 0x0400D30A RID: 54026
				public static LocString NAME = "Full bladder";

				// Token: 0x0400D30B RID: 54027
				public static LocString TOOLTIP = "This Duplicant would really appreciate an " + UI.FormatAsLink("Outhouse", "OUTHOUSE") + " or " + UI.FormatAsLink("Lavatory", "FLUSHTOILET");
			}

			// Token: 0x0200351B RID: 13595
			public class STRESSFULLYEMPTYINGOIL
			{
				// Token: 0x0400D30C RID: 54028
				public static LocString NAME = "Expelling gunk";

				// Token: 0x0400D30D RID: 54029
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Bionic Duplicant couldn't get to a ",
					UI.FormatAsLink("Gunk Extractor", "GUNKEMPTIER"),
					" in time and got desperate\n\n",
					UI.FormatAsLink("Gunk Extractors", "GUNKEMPTIER"),
					" can be built from the ",
					UI.FormatAsBuildMenuTab("Plumbing Tab", global::Action.Plan5)
				});

				// Token: 0x0400D30E RID: 54030
				public static LocString NOTIFICATION_NAME = "Expelled gunk";

				// Token: 0x0400D30F RID: 54031
				public static LocString NOTIFICATION_TOOLTIP = "The " + UI.FormatAsTool("Mop Tool", global::Action.Mop) + " can be used to clean up Duplicant-related \"spills\"\n\nThese Duplicants made messes that require cleaning up:\n";
			}

			// Token: 0x0200351C RID: 13596
			public class STRESSFULLYEMPTYINGBLADDER
			{
				// Token: 0x0400D310 RID: 54032
				public static LocString NAME = "Making a mess";

				// Token: 0x0400D311 RID: 54033
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This poor Duplicant couldn't find an ",
					UI.FormatAsLink("Outhouse", "OUTHOUSE"),
					" in time and is super embarrassed\n\n",
					UI.FormatAsLink("Outhouses", "OUTHOUSE"),
					" can be built from the ",
					UI.FormatAsBuildMenuTab("Plumbing Tab", global::Action.Plan5)
				});

				// Token: 0x0400D312 RID: 54034
				public static LocString NOTIFICATION_NAME = "Made a mess";

				// Token: 0x0400D313 RID: 54035
				public static LocString NOTIFICATION_TOOLTIP = "The " + UI.FormatAsTool("Mop Tool", global::Action.Mop) + " can be used to clean up Duplicant-related \"spills\"\n\nThese Duplicants made messes that require cleaning up:\n";
			}

			// Token: 0x0200351D RID: 13597
			public class WASHINGHANDS
			{
				// Token: 0x0400D314 RID: 54036
				public static LocString NAME = "Washing hands";

				// Token: 0x0400D315 RID: 54037
				public static LocString TOOLTIP = "This Duplicant is washing their hands";
			}

			// Token: 0x0200351E RID: 13598
			public class SHOWERING
			{
				// Token: 0x0400D316 RID: 54038
				public static LocString NAME = "Showering";

				// Token: 0x0400D317 RID: 54039
				public static LocString TOOLTIP = "This Duplicant is gonna be squeaky clean";
			}

			// Token: 0x0200351F RID: 13599
			public class RELAXING
			{
				// Token: 0x0400D318 RID: 54040
				public static LocString NAME = "Relaxing";

				// Token: 0x0400D319 RID: 54041
				public static LocString TOOLTIP = "This Duplicant's just taking it easy";
			}

			// Token: 0x02003520 RID: 13600
			public class VOMITING
			{
				// Token: 0x0400D31A RID: 54042
				public static LocString NAME = "Throwing up";

				// Token: 0x0400D31B RID: 54043
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant has unceremoniously hurled as the result of a ",
					UI.FormatAsLink("Disease", "DISEASE"),
					"\n\nDuplicant-related \"spills\" can be cleaned up using the ",
					UI.PRE_KEYWORD,
					"Mop Tool",
					UI.PST_KEYWORD,
					" ",
					UI.FormatAsHotKey(global::Action.Mop)
				});

				// Token: 0x0400D31C RID: 54044
				public static LocString NOTIFICATION_NAME = "Throwing up";

				// Token: 0x0400D31D RID: 54045
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"The ",
					UI.FormatAsTool("Mop Tool", global::Action.Mop),
					" can be used to clean up Duplicant-related \"spills\"\n\nA ",
					UI.PRE_KEYWORD,
					"Disease",
					UI.PST_KEYWORD,
					" has caused these Duplicants to throw up:"
				});
			}

			// Token: 0x02003521 RID: 13601
			public class STRESSVOMITING
			{
				// Token: 0x0400D31E RID: 54046
				public static LocString NAME = "Stress vomiting";

				// Token: 0x0400D31F RID: 54047
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is relieving their ",
					UI.PRE_KEYWORD,
					"Stress",
					UI.PST_KEYWORD,
					" all over the floor\n\nDuplicant-related \"spills\" can be cleaned up using the ",
					UI.PRE_KEYWORD,
					"Mop Tool",
					UI.PST_KEYWORD,
					" ",
					UI.FormatAsHotKey(global::Action.Mop)
				});

				// Token: 0x0400D320 RID: 54048
				public static LocString NOTIFICATION_NAME = "Stress vomiting";

				// Token: 0x0400D321 RID: 54049
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"The ",
					UI.FormatAsTool("Mop Tool", global::Action.Mop),
					" can used to clean up Duplicant-related \"spills\"\n\nThese Duplicants became so ",
					UI.PRE_KEYWORD,
					"Stressed",
					UI.PST_KEYWORD,
					" they threw up:"
				});
			}

			// Token: 0x02003522 RID: 13602
			public class RADIATIONVOMITING
			{
				// Token: 0x0400D322 RID: 54050
				public static LocString NAME = "Radiation vomiting";

				// Token: 0x0400D323 RID: 54051
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is sick due to ",
					UI.PRE_KEYWORD,
					"Radiation",
					UI.PST_KEYWORD,
					" poisoning.\n\nDuplicant-related \"spills\" can be cleaned up using the ",
					UI.PRE_KEYWORD,
					"Mop Tool",
					UI.PST_KEYWORD,
					" ",
					UI.FormatAsHotKey(global::Action.Mop)
				});

				// Token: 0x0400D324 RID: 54052
				public static LocString NOTIFICATION_NAME = "Radiation vomiting";

				// Token: 0x0400D325 RID: 54053
				public static LocString NOTIFICATION_TOOLTIP = "The " + UI.FormatAsTool("Mop Tool", global::Action.Mop) + " can clean up Duplicant-related \"spills\"\n\nRadiation Sickness caused these Duplicants to throw up:";
			}

			// Token: 0x02003523 RID: 13603
			public class HASDISEASE
			{
				// Token: 0x0400D326 RID: 54054
				public static LocString NAME = "Feeling ill";

				// Token: 0x0400D327 RID: 54055
				public static LocString TOOLTIP = "This Duplicant has contracted a " + UI.FormatAsLink("Disease", "DISEASE") + " and requires recovery time at a " + UI.FormatAsLink("Sick Bay", "DOCTORSTATION");

				// Token: 0x0400D328 RID: 54056
				public static LocString NOTIFICATION_NAME = "Illness";

				// Token: 0x0400D329 RID: 54057
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"These Duplicants have contracted a ",
					UI.FormatAsLink("Disease", "DISEASE"),
					" and require recovery time at a ",
					UI.FormatAsLink("Sick Bay", "DOCTORSTATION"),
					":"
				});
			}

			// Token: 0x02003524 RID: 13604
			public class BODYREGULATINGHEATING
			{
				// Token: 0x0400D32A RID: 54058
				public static LocString NAME = "Regulating temperature at: {TempDelta}";

				// Token: 0x0400D32B RID: 54059
				public static LocString TOOLTIP = "This Duplicant is regulating their internal " + UI.PRE_KEYWORD + "Temperature" + UI.PST_KEYWORD;
			}

			// Token: 0x02003525 RID: 13605
			public class BODYREGULATINGCOOLING
			{
				// Token: 0x0400D32C RID: 54060
				public static LocString NAME = "Regulating temperature at: {TempDelta}";

				// Token: 0x0400D32D RID: 54061
				public static LocString TOOLTIP = "This Duplicant is regulating their internal " + UI.PRE_KEYWORD + "Temperature" + UI.PST_KEYWORD;
			}

			// Token: 0x02003526 RID: 13606
			public class BREATHINGO2
			{
				// Token: 0x0400D32E RID: 54062
				public static LocString NAME = "Inhaling {ConsumptionRate} O<sub>2</sub>";

				// Token: 0x0400D32F RID: 54063
				public static LocString TOOLTIP = "Duplicants require " + UI.FormatAsLink("Oxygen", "OXYGEN") + " to live";
			}

			// Token: 0x02003527 RID: 13607
			public class EMITTINGCO2
			{
				// Token: 0x0400D330 RID: 54064
				public static LocString NAME = "Exhaling {EmittingRate} CO<sub>2</sub>";

				// Token: 0x0400D331 RID: 54065
				public static LocString TOOLTIP = "Duplicants breathe out " + UI.FormatAsLink("Carbon Dioxide", "CARBONDIOXIDE");
			}

			// Token: 0x02003528 RID: 13608
			public class PICKUPDELIVERSTATUS
			{
				// Token: 0x0400D332 RID: 54066
				public static LocString NAME = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.NAME;

				// Token: 0x0400D333 RID: 54067
				public static LocString TOOLTIP = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.TOOLTIP;
			}

			// Token: 0x02003529 RID: 13609
			public class STOREDELIVERSTATUS
			{
				// Token: 0x0400D334 RID: 54068
				public static LocString NAME = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.NAME;

				// Token: 0x0400D335 RID: 54069
				public static LocString TOOLTIP = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.TOOLTIP;
			}

			// Token: 0x0200352A RID: 13610
			public class CLEARDELIVERSTATUS
			{
				// Token: 0x0400D336 RID: 54070
				public static LocString NAME = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.NAME;

				// Token: 0x0400D337 RID: 54071
				public static LocString TOOLTIP = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.TOOLTIP;
			}

			// Token: 0x0200352B RID: 13611
			public class STOREFORBUILDDELIVERSTATUS
			{
				// Token: 0x0400D338 RID: 54072
				public static LocString NAME = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.NAME;

				// Token: 0x0400D339 RID: 54073
				public static LocString TOOLTIP = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.TOOLTIP;
			}

			// Token: 0x0200352C RID: 13612
			public class STOREFORBUILDPRIORITIZEDDELIVERSTATUS
			{
				// Token: 0x0400D33A RID: 54074
				public static LocString NAME = "Allocating {Item} to {Target}";

				// Token: 0x0400D33B RID: 54075
				public static LocString TOOLTIP = "This Duplicant is delivering materials to a <b>{Target}</b> construction errand";
			}

			// Token: 0x0200352D RID: 13613
			public class BUILDDELIVERSTATUS
			{
				// Token: 0x0400D33C RID: 54076
				public static LocString NAME = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.NAME;

				// Token: 0x0400D33D RID: 54077
				public static LocString TOOLTIP = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.TOOLTIP;
			}

			// Token: 0x0200352E RID: 13614
			public class BUILDPRIORITIZEDSTATUS
			{
				// Token: 0x0400D33E RID: 54078
				public static LocString NAME = "Building {Target}";

				// Token: 0x0400D33F RID: 54079
				public static LocString TOOLTIP = "This Duplicant is constructing a <b>{Target}</b>";
			}

			// Token: 0x0200352F RID: 13615
			public class FABRICATEDELIVERSTATUS
			{
				// Token: 0x0400D340 RID: 54080
				public static LocString NAME = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.NAME;

				// Token: 0x0400D341 RID: 54081
				public static LocString TOOLTIP = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.TOOLTIP;
			}

			// Token: 0x02003530 RID: 13616
			public class USEITEMDELIVERSTATUS
			{
				// Token: 0x0400D342 RID: 54082
				public static LocString NAME = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.NAME;

				// Token: 0x0400D343 RID: 54083
				public static LocString TOOLTIP = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.TOOLTIP;
			}

			// Token: 0x02003531 RID: 13617
			public class STOREPRIORITYDELIVERSTATUS
			{
				// Token: 0x0400D344 RID: 54084
				public static LocString NAME = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.NAME;

				// Token: 0x0400D345 RID: 54085
				public static LocString TOOLTIP = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.TOOLTIP;
			}

			// Token: 0x02003532 RID: 13618
			public class STORECRITICALDELIVERSTATUS
			{
				// Token: 0x0400D346 RID: 54086
				public static LocString NAME = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.NAME;

				// Token: 0x0400D347 RID: 54087
				public static LocString TOOLTIP = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.TOOLTIP;
			}

			// Token: 0x02003533 RID: 13619
			public class COMPOSTFLIPSTATUS
			{
				// Token: 0x0400D348 RID: 54088
				public static LocString NAME = "Going to flip compost";

				// Token: 0x0400D349 RID: 54089
				public static LocString TOOLTIP = "This Duplicant is going to flip the " + BUILDINGS.PREFABS.COMPOST.NAME;
			}

			// Token: 0x02003534 RID: 13620
			public class DECONSTRUCTDELIVERSTATUS
			{
				// Token: 0x0400D34A RID: 54090
				public static LocString NAME = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.NAME;

				// Token: 0x0400D34B RID: 54091
				public static LocString TOOLTIP = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.TOOLTIP;
			}

			// Token: 0x02003535 RID: 13621
			public class TOGGLEDELIVERSTATUS
			{
				// Token: 0x0400D34C RID: 54092
				public static LocString NAME = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.NAME;

				// Token: 0x0400D34D RID: 54093
				public static LocString TOOLTIP = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.TOOLTIP;
			}

			// Token: 0x02003536 RID: 13622
			public class EMPTYSTORAGEDELIVERSTATUS
			{
				// Token: 0x0400D34E RID: 54094
				public static LocString NAME = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.NAME;

				// Token: 0x0400D34F RID: 54095
				public static LocString TOOLTIP = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.TOOLTIP;
			}

			// Token: 0x02003537 RID: 13623
			public class HARVESTDELIVERSTATUS
			{
				// Token: 0x0400D350 RID: 54096
				public static LocString NAME = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.NAME;

				// Token: 0x0400D351 RID: 54097
				public static LocString TOOLTIP = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.TOOLTIP;
			}

			// Token: 0x02003538 RID: 13624
			public class SLEEPDELIVERSTATUS
			{
				// Token: 0x0400D352 RID: 54098
				public static LocString NAME = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.NAME;

				// Token: 0x0400D353 RID: 54099
				public static LocString TOOLTIP = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.TOOLTIP;
			}

			// Token: 0x02003539 RID: 13625
			public class EATDELIVERSTATUS
			{
				// Token: 0x0400D354 RID: 54100
				public static LocString NAME = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.NAME;

				// Token: 0x0400D355 RID: 54101
				public static LocString TOOLTIP = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.TOOLTIP;
			}

			// Token: 0x0200353A RID: 13626
			public class WARMUPDELIVERSTATUS
			{
				// Token: 0x0400D356 RID: 54102
				public static LocString NAME = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.NAME;

				// Token: 0x0400D357 RID: 54103
				public static LocString TOOLTIP = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.TOOLTIP;
			}

			// Token: 0x0200353B RID: 13627
			public class REPAIRDELIVERSTATUS
			{
				// Token: 0x0400D358 RID: 54104
				public static LocString NAME = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.NAME;

				// Token: 0x0400D359 RID: 54105
				public static LocString TOOLTIP = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.TOOLTIP;
			}

			// Token: 0x0200353C RID: 13628
			public class REPAIRWORKSTATUS
			{
				// Token: 0x0400D35A RID: 54106
				public static LocString NAME = "Repairing {Target}";

				// Token: 0x0400D35B RID: 54107
				public static LocString TOOLTIP = "This Duplicant is fixing the <b>{Target}</b>";
			}

			// Token: 0x0200353D RID: 13629
			public class BREAKDELIVERSTATUS
			{
				// Token: 0x0400D35C RID: 54108
				public static LocString NAME = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.NAME;

				// Token: 0x0400D35D RID: 54109
				public static LocString TOOLTIP = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.TOOLTIP;
			}

			// Token: 0x0200353E RID: 13630
			public class BREAKWORKSTATUS
			{
				// Token: 0x0400D35E RID: 54110
				public static LocString NAME = "Breaking {Target}";

				// Token: 0x0400D35F RID: 54111
				public static LocString TOOLTIP = "This Duplicant is going totally bananas on the <b>{Target}</b>!";
			}

			// Token: 0x0200353F RID: 13631
			public class EQUIPDELIVERSTATUS
			{
				// Token: 0x0400D360 RID: 54112
				public static LocString NAME = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.NAME;

				// Token: 0x0400D361 RID: 54113
				public static LocString TOOLTIP = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.TOOLTIP;
			}

			// Token: 0x02003540 RID: 13632
			public class COOKDELIVERSTATUS
			{
				// Token: 0x0400D362 RID: 54114
				public static LocString NAME = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.NAME;

				// Token: 0x0400D363 RID: 54115
				public static LocString TOOLTIP = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.TOOLTIP;
			}

			// Token: 0x02003541 RID: 13633
			public class MUSHDELIVERSTATUS
			{
				// Token: 0x0400D364 RID: 54116
				public static LocString NAME = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.NAME;

				// Token: 0x0400D365 RID: 54117
				public static LocString TOOLTIP = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.TOOLTIP;
			}

			// Token: 0x02003542 RID: 13634
			public class PACIFYDELIVERSTATUS
			{
				// Token: 0x0400D366 RID: 54118
				public static LocString NAME = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.NAME;

				// Token: 0x0400D367 RID: 54119
				public static LocString TOOLTIP = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.TOOLTIP;
			}

			// Token: 0x02003543 RID: 13635
			public class RESCUEDELIVERSTATUS
			{
				// Token: 0x0400D368 RID: 54120
				public static LocString NAME = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.NAME;

				// Token: 0x0400D369 RID: 54121
				public static LocString TOOLTIP = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.TOOLTIP;
			}

			// Token: 0x02003544 RID: 13636
			public class RESCUEWORKSTATUS
			{
				// Token: 0x0400D36A RID: 54122
				public static LocString NAME = "Rescuing {Target}";

				// Token: 0x0400D36B RID: 54123
				public static LocString TOOLTIP = "This Duplicant is saving <b>{Target}</b> from certain peril!";
			}

			// Token: 0x02003545 RID: 13637
			public class MOPDELIVERSTATUS
			{
				// Token: 0x0400D36C RID: 54124
				public static LocString NAME = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.NAME;

				// Token: 0x0400D36D RID: 54125
				public static LocString TOOLTIP = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.TOOLTIP;
			}

			// Token: 0x02003546 RID: 13638
			public class DIGGING
			{
				// Token: 0x0400D36E RID: 54126
				public static LocString NAME = "Digging";

				// Token: 0x0400D36F RID: 54127
				public static LocString TOOLTIP = "This Duplicant is excavating raw resources";
			}

			// Token: 0x02003547 RID: 13639
			public class EATING
			{
				// Token: 0x0400D370 RID: 54128
				public static LocString NAME = "Eating {Target}";

				// Token: 0x0400D371 RID: 54129
				public static LocString TOOLTIP = "This Duplicant is having a meal";
			}

			// Token: 0x02003548 RID: 13640
			public class CLEANING
			{
				// Token: 0x0400D372 RID: 54130
				public static LocString NAME = "Cleaning {Target}";

				// Token: 0x0400D373 RID: 54131
				public static LocString TOOLTIP = "This Duplicant is cleaning the <b>{Target}</b>";
			}

			// Token: 0x02003549 RID: 13641
			public class LIGHTWORKEFFICIENCYBONUS
			{
				// Token: 0x0400D374 RID: 54132
				public static LocString NAME = "Lit Workspace";

				// Token: 0x0400D375 RID: 54133
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Better visibility from the ",
					UI.PRE_KEYWORD,
					"Light",
					UI.PST_KEYWORD,
					" is allowing this Duplicant to work faster:\n    {0}"
				});

				// Token: 0x0400D376 RID: 54134
				public static LocString NO_BUILDING_WORK_ATTRIBUTE = "{0} Speed";
			}

			// Token: 0x0200354A RID: 13642
			public class LABORATORYWORKEFFICIENCYBONUS
			{
				// Token: 0x0400D377 RID: 54135
				public static LocString NAME = "Lab Workspace";

				// Token: 0x0400D378 RID: 54136
				public static LocString TOOLTIP = "Working in a Laboratory is allowing this Duplicant to work faster:\n    {0}";

				// Token: 0x0400D379 RID: 54137
				public static LocString NO_BUILDING_WORK_ATTRIBUTE = "{0} Speed";
			}

			// Token: 0x0200354B RID: 13643
			public class PICKINGUP
			{
				// Token: 0x0400D37A RID: 54138
				public static LocString NAME = "Picking up {Target}";

				// Token: 0x0400D37B RID: 54139
				public static LocString TOOLTIP = "This Duplicant is retrieving <b>{Target}</b>";
			}

			// Token: 0x0200354C RID: 13644
			public class MOPPING
			{
				// Token: 0x0400D37C RID: 54140
				public static LocString NAME = "Mopping";

				// Token: 0x0400D37D RID: 54141
				public static LocString TOOLTIP = "This Duplicant is cleaning up a nasty spill";
			}

			// Token: 0x0200354D RID: 13645
			public class ARTING
			{
				// Token: 0x0400D37E RID: 54142
				public static LocString NAME = "Decorating";

				// Token: 0x0400D37F RID: 54143
				public static LocString TOOLTIP = "This Duplicant is hard at work on their art";
			}

			// Token: 0x0200354E RID: 13646
			public class MUSHING
			{
				// Token: 0x0400D380 RID: 54144
				public static LocString NAME = "Mushing {Item}";

				// Token: 0x0400D381 RID: 54145
				public static LocString TOOLTIP = "This Duplicant is cooking a <b>{Item}</b>";
			}

			// Token: 0x0200354F RID: 13647
			public class COOKING
			{
				// Token: 0x0400D382 RID: 54146
				public static LocString NAME = "Cooking {Item}";

				// Token: 0x0400D383 RID: 54147
				public static LocString TOOLTIP = "This Duplicant is cooking up a tasty <b>{Item}</b>";
			}

			// Token: 0x02003550 RID: 13648
			public class RESEARCHING
			{
				// Token: 0x0400D384 RID: 54148
				public static LocString NAME = "Researching {Tech}";

				// Token: 0x0400D385 RID: 54149
				public static LocString TOOLTIP = "This Duplicant is intently researching <b>{Tech}</b> technology";
			}

			// Token: 0x02003551 RID: 13649
			public class RESEARCHING_FROM_POI
			{
				// Token: 0x0400D386 RID: 54150
				public static LocString NAME = "Unlocking Research";

				// Token: 0x0400D387 RID: 54151
				public static LocString TOOLTIP = "This Duplicant is unlocking crucial technology";
			}

			// Token: 0x02003552 RID: 13650
			public class MISSIONCONTROLLING
			{
				// Token: 0x0400D388 RID: 54152
				public static LocString NAME = "Mission Controlling";

				// Token: 0x0400D389 RID: 54153
				public static LocString TOOLTIP = "This Duplicant is guiding a " + UI.PRE_KEYWORD + "Rocket" + UI.PST_KEYWORD;
			}

			// Token: 0x02003553 RID: 13651
			public class STORING
			{
				// Token: 0x0400D38A RID: 54154
				public static LocString NAME = "Storing {Item}";

				// Token: 0x0400D38B RID: 54155
				public static LocString TOOLTIP = "This Duplicant is putting <b>{Item}</b> away in <b>{Target}</b>";
			}

			// Token: 0x02003554 RID: 13652
			public class LOADINGELECTROBANK
			{
				// Token: 0x0400D38C RID: 54156
				public static LocString NAME = "Loading {Item}";

				// Token: 0x0400D38D RID: 54157
				public static LocString TOOLTIP = "This Duplicant is loading an <b>{Item}</b> into a <b>{Target}</b>";
			}

			// Token: 0x02003555 RID: 13653
			public class BUILDING
			{
				// Token: 0x0400D38E RID: 54158
				public static LocString NAME = "Building {Target}";

				// Token: 0x0400D38F RID: 54159
				public static LocString TOOLTIP = "This Duplicant is constructing a <b>{Target}</b>";
			}

			// Token: 0x02003556 RID: 13654
			public class EQUIPPING
			{
				// Token: 0x0400D390 RID: 54160
				public static LocString NAME = "Equipping {Target}";

				// Token: 0x0400D391 RID: 54161
				public static LocString TOOLTIP = "This Duplicant is equipping a <b>{Target}</b>";
			}

			// Token: 0x02003557 RID: 13655
			public class WARMINGUP
			{
				// Token: 0x0400D392 RID: 54162
				public static LocString NAME = "Warming up";

				// Token: 0x0400D393 RID: 54163
				public static LocString TOOLTIP = "This Duplicant got too cold and is trying to warm up";
			}

			// Token: 0x02003558 RID: 13656
			public class GENERATINGPOWER
			{
				// Token: 0x0400D394 RID: 54164
				public static LocString NAME = "Generating power";

				// Token: 0x0400D395 RID: 54165
				public static LocString TOOLTIP = "This Duplicant is using the <b>{Target}</b> to produce electrical " + UI.PRE_KEYWORD + "Power" + UI.PST_KEYWORD;
			}

			// Token: 0x02003559 RID: 13657
			public class HARVESTING
			{
				// Token: 0x0400D396 RID: 54166
				public static LocString NAME = "Harvesting {Target}";

				// Token: 0x0400D397 RID: 54167
				public static LocString TOOLTIP = "This Duplicant is gathering resources from a <b>{Target}</b>";
			}

			// Token: 0x0200355A RID: 13658
			public class UPROOTING
			{
				// Token: 0x0400D398 RID: 54168
				public static LocString NAME = "Uprooting {Target}";

				// Token: 0x0400D399 RID: 54169
				public static LocString TOOLTIP = "This Duplicant is digging up a <b>{Target}</b>";
			}

			// Token: 0x0200355B RID: 13659
			public class EMPTYING
			{
				// Token: 0x0400D39A RID: 54170
				public static LocString NAME = "Emptying {Target}";

				// Token: 0x0400D39B RID: 54171
				public static LocString TOOLTIP = "This Duplicant is removing materials from the <b>{Target}</b>";
			}

			// Token: 0x0200355C RID: 13660
			public class TOGGLING
			{
				// Token: 0x0400D39C RID: 54172
				public static LocString NAME = "Change {Target} setting";

				// Token: 0x0400D39D RID: 54173
				public static LocString TOOLTIP = "This Duplicant is changing the <b>{Target}</b>'s setting";
			}

			// Token: 0x0200355D RID: 13661
			public class DECONSTRUCTING
			{
				// Token: 0x0400D39E RID: 54174
				public static LocString NAME = "Deconstructing {Target}";

				// Token: 0x0400D39F RID: 54175
				public static LocString TOOLTIP = "This Duplicant is deconstructing the <b>{Target}</b>";
			}

			// Token: 0x0200355E RID: 13662
			public class DEMOLISHING
			{
				// Token: 0x0400D3A0 RID: 54176
				public static LocString NAME = "Demolishing {Target}";

				// Token: 0x0400D3A1 RID: 54177
				public static LocString TOOLTIP = "This Duplicant is demolishing the <b>{Target}</b>";
			}

			// Token: 0x0200355F RID: 13663
			public class DISINFECTING
			{
				// Token: 0x0400D3A2 RID: 54178
				public static LocString NAME = "Disinfecting {Target}";

				// Token: 0x0400D3A3 RID: 54179
				public static LocString TOOLTIP = "This Duplicant is disinfecting <b>{Target}</b>";
			}

			// Token: 0x02003560 RID: 13664
			public class FABRICATING
			{
				// Token: 0x0400D3A4 RID: 54180
				public static LocString NAME = "Fabricating {Item}";

				// Token: 0x0400D3A5 RID: 54181
				public static LocString TOOLTIP = "This Duplicant is crafting a <b>{Item}</b>";
			}

			// Token: 0x02003561 RID: 13665
			public class PROCESSING
			{
				// Token: 0x0400D3A6 RID: 54182
				public static LocString NAME = "Refining {Item}";

				// Token: 0x0400D3A7 RID: 54183
				public static LocString TOOLTIP = "This Duplicant is refining <b>{Item}</b>";
			}

			// Token: 0x02003562 RID: 13666
			public class SPICING
			{
				// Token: 0x0400D3A8 RID: 54184
				public static LocString NAME = "Spicing Food";

				// Token: 0x0400D3A9 RID: 54185
				public static LocString TOOLTIP = "This Duplicant is making a tasty meal even tastier";
			}

			// Token: 0x02003563 RID: 13667
			public class CLEARING
			{
				// Token: 0x0400D3AA RID: 54186
				public static LocString NAME = "Sweeping {Target}";

				// Token: 0x0400D3AB RID: 54187
				public static LocString TOOLTIP = "This Duplicant is sweeping away <b>{Target}</b>";
			}

			// Token: 0x02003564 RID: 13668
			public class STUDYING
			{
				// Token: 0x0400D3AC RID: 54188
				public static LocString NAME = "Analyzing";

				// Token: 0x0400D3AD RID: 54189
				public static LocString TOOLTIP = "This Duplicant is conducting a field study of a Natural Feature";
			}

			// Token: 0x02003565 RID: 13669
			public class INSTALLINGELECTROBANK
			{
				// Token: 0x0400D3AE RID: 54190
				public static LocString NAME = "Rescuing Bionic Friend";

				// Token: 0x0400D3AF RID: 54191
				public static LocString TOOLTIP = "This Duplicant is rebooting a powerless Bionic Duplicant";
			}

			// Token: 0x02003566 RID: 13670
			public class SOCIALIZING
			{
				// Token: 0x0400D3B0 RID: 54192
				public static LocString NAME = "Socializing";

				// Token: 0x0400D3B1 RID: 54193
				public static LocString TOOLTIP = "This Duplicant is using their break to hang out";
			}

			// Token: 0x02003567 RID: 13671
			public class BIONICEXPLORERBOOSTER
			{
				// Token: 0x0400D3B2 RID: 54194
				public static LocString NOTIFICATION_NAME = "Dowsing Complete: Geyser Discovered";

				// Token: 0x0400D3B3 RID: 54195
				public static LocString NOTIFICATION_TOOLTIP = "Click to see the geyser recently discovered by a Bionic Duplicant";

				// Token: 0x0400D3B4 RID: 54196
				public static LocString NAME = "Dowsing {0}";

				// Token: 0x0400D3B5 RID: 54197
				public static LocString TOOLTIP = "This Duplicant's always gathering geodata\n\nWhen dowsing is complete, a new geyser will be revealed in the world";
			}

			// Token: 0x02003568 RID: 13672
			public class MINGLING
			{
				// Token: 0x0400D3B6 RID: 54198
				public static LocString NAME = "Mingling";

				// Token: 0x0400D3B7 RID: 54199
				public static LocString TOOLTIP = "This Duplicant is using their break to chat with friends";
			}

			// Token: 0x02003569 RID: 13673
			public class NOISEPEACEFUL
			{
				// Token: 0x0400D3B8 RID: 54200
				public static LocString NAME = "Peace and Quiet";

				// Token: 0x0400D3B9 RID: 54201
				public static LocString TOOLTIP = "This Duplicant has found a quiet place to concentrate";
			}

			// Token: 0x0200356A RID: 13674
			public class NOISEMINOR
			{
				// Token: 0x0400D3BA RID: 54202
				public static LocString NAME = "Loud Noises";

				// Token: 0x0400D3BB RID: 54203
				public static LocString TOOLTIP = "This area is a bit too loud for comfort";
			}

			// Token: 0x0200356B RID: 13675
			public class NOISEMAJOR
			{
				// Token: 0x0400D3BC RID: 54204
				public static LocString NAME = "Cacophony!";

				// Token: 0x0400D3BD RID: 54205
				public static LocString TOOLTIP = "It's very, very loud in here!";
			}

			// Token: 0x0200356C RID: 13676
			public class LOWIMMUNITY
			{
				// Token: 0x0400D3BE RID: 54206
				public static LocString NAME = "Under the Weather";

				// Token: 0x0400D3BF RID: 54207
				public static LocString TOOLTIP = "This Duplicant has a weakened immune system and will become ill if it reaches zero";

				// Token: 0x0400D3C0 RID: 54208
				public static LocString NOTIFICATION_NAME = "Low Immunity";

				// Token: 0x0400D3C1 RID: 54209
				public static LocString NOTIFICATION_TOOLTIP = "These Duplicants are at risk of becoming sick:";
			}

			// Token: 0x0200356D RID: 13677
			public abstract class TINKERING
			{
				// Token: 0x0400D3C2 RID: 54210
				public static LocString NAME = "Tinkering";

				// Token: 0x0400D3C3 RID: 54211
				public static LocString TOOLTIP = "This Duplicant is making functional improvements to a building";
			}

			// Token: 0x0200356E RID: 13678
			public class CONTACTWITHGERMS
			{
				// Token: 0x0400D3C4 RID: 54212
				public static LocString NAME = "Contact with {Sickness} Germs";

				// Token: 0x0400D3C5 RID: 54213
				public static LocString TOOLTIP = "This Duplicant has encountered {Sickness} Germs and is at risk of dangerous exposure if contact continues\n\n<i>" + UI.CLICK(UI.ClickType.Click) + " to jump to last contact location</i>";
			}

			// Token: 0x0200356F RID: 13679
			public class EXPOSEDTOGERMS
			{
				// Token: 0x0400D3C6 RID: 54214
				public static LocString TIER1 = "Mild Exposure";

				// Token: 0x0400D3C7 RID: 54215
				public static LocString TIER2 = "Medium Exposure";

				// Token: 0x0400D3C8 RID: 54216
				public static LocString TIER3 = "Exposure";

				// Token: 0x0400D3C9 RID: 54217
				public static readonly LocString[] EXPOSURE_TIERS = new LocString[]
				{
					DUPLICANTS.STATUSITEMS.EXPOSEDTOGERMS.TIER1,
					DUPLICANTS.STATUSITEMS.EXPOSEDTOGERMS.TIER2,
					DUPLICANTS.STATUSITEMS.EXPOSEDTOGERMS.TIER3
				};

				// Token: 0x0400D3CA RID: 54218
				public static LocString NAME = "{Severity} to {Sickness} Germs";

				// Token: 0x0400D3CB RID: 54219
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant has been exposed to a concentration of {Sickness} Germs and is at risk of waking up sick on their next shift\n\nExposed {Source}\n\nRate of Contracting {Sickness}: {Chance}\n\nResistance Rating: {Total}\n    • Base {Sickness} Resistance: {Base}\n    • ",
					DUPLICANTS.ATTRIBUTES.GERMRESISTANCE.NAME,
					": {Dupe}\n    • {Severity} Exposure: {ExposureLevelBonus}\n\n<i>",
					UI.CLICK(UI.ClickType.Click),
					" to jump to last exposure location</i>"
				});
			}

			// Token: 0x02003570 RID: 13680
			public class GASLIQUIDEXPOSURE
			{
				// Token: 0x0400D3CC RID: 54220
				public static LocString NAME_MINOR = "Eye Irritation";

				// Token: 0x0400D3CD RID: 54221
				public static LocString NAME_MAJOR = "Major Eye Irritation";

				// Token: 0x0400D3CE RID: 54222
				public static LocString TOOLTIP = "Ah, it stings!\n\nThis poor Duplicant got a faceful of an irritating gas or liquid";

				// Token: 0x0400D3CF RID: 54223
				public static LocString TOOLTIP_EXPOSED = "Current exposure to {element} is {rate} eye irritation";

				// Token: 0x0400D3D0 RID: 54224
				public static LocString TOOLTIP_RATE_INCREASE = "increasing";

				// Token: 0x0400D3D1 RID: 54225
				public static LocString TOOLTIP_RATE_DECREASE = "decreasing";

				// Token: 0x0400D3D2 RID: 54226
				public static LocString TOOLTIP_RATE_STAYS = "maintaining";

				// Token: 0x0400D3D3 RID: 54227
				public static LocString TOOLTIP_EXPOSURE_LEVEL = "Time Remaining: {time}";
			}

			// Token: 0x02003571 RID: 13681
			public class BEINGPRODUCTIVE
			{
				// Token: 0x0400D3D4 RID: 54228
				public static LocString NAME = "Super Focused";

				// Token: 0x0400D3D5 RID: 54229
				public static LocString TOOLTIP = "This Duplicant is focused on being super productive right now";
			}

			// Token: 0x02003572 RID: 13682
			public class BALLOONARTISTPLANNING
			{
				// Token: 0x0400D3D6 RID: 54230
				public static LocString NAME = "Balloon Artist";

				// Token: 0x0400D3D7 RID: 54231
				public static LocString TOOLTIP = "This Duplicant is planning to hand out balloons in their downtime";
			}

			// Token: 0x02003573 RID: 13683
			public class BALLOONARTISTHANDINGOUT
			{
				// Token: 0x0400D3D8 RID: 54232
				public static LocString NAME = "Balloon Artist";

				// Token: 0x0400D3D9 RID: 54233
				public static LocString TOOLTIP = "This Duplicant is handing out balloons to other Duplicants";
			}

			// Token: 0x02003574 RID: 13684
			public class EXPELLINGRADS
			{
				// Token: 0x0400D3DA RID: 54234
				public static LocString NAME = "Cleansing Rads";

				// Token: 0x0400D3DB RID: 54235
				public static LocString TOOLTIP = "This Duplicant is, uh... \"expelling\" absorbed radiation from their system";
			}

			// Token: 0x02003575 RID: 13685
			public class ANALYZINGGENES
			{
				// Token: 0x0400D3DC RID: 54236
				public static LocString NAME = "Analyzing Plant Genes";

				// Token: 0x0400D3DD RID: 54237
				public static LocString TOOLTIP = "This Duplicant is peering deep into the genetic code of an odd seed";
			}

			// Token: 0x02003576 RID: 13686
			public class ANALYZINGARTIFACT
			{
				// Token: 0x0400D3DE RID: 54238
				public static LocString NAME = "Analyzing Artifact";

				// Token: 0x0400D3DF RID: 54239
				public static LocString TOOLTIP = "This Duplicant is studying an artifact";
			}

			// Token: 0x02003577 RID: 13687
			public class RANCHING
			{
				// Token: 0x0400D3E0 RID: 54240
				public static LocString NAME = "Ranching";

				// Token: 0x0400D3E1 RID: 54241
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is tending to a ",
					UI.PRE_KEYWORD,
					"Critter",
					UI.PST_KEYWORD,
					"'s well-being"
				});
			}

			// Token: 0x02003578 RID: 13688
			public class CARVING
			{
				// Token: 0x0400D3E2 RID: 54242
				public static LocString NAME = "Carving {Target}";

				// Token: 0x0400D3E3 RID: 54243
				public static LocString TOOLTIP = "This Duplicant is carving away at a <b>{Target}</b>";
			}

			// Token: 0x02003579 RID: 13689
			public class DATARAINERPLANNING
			{
				// Token: 0x0400D3E4 RID: 54244
				public static LocString NAME = "Rainmaker";

				// Token: 0x0400D3E5 RID: 54245
				public static LocString TOOLTIP = "This Duplicant is planning to dish out microchips in their downtime";
			}

			// Token: 0x0200357A RID: 13690
			public class DATARAINERRAINING
			{
				// Token: 0x0400D3E6 RID: 54246
				public static LocString NAME = "Rainmaker";

				// Token: 0x0400D3E7 RID: 54247
				public static LocString TOOLTIP = "This Duplicant is making it \"rain\" microchips";
			}

			// Token: 0x0200357B RID: 13691
			public class ROBODANCERPLANNING
			{
				// Token: 0x0400D3E8 RID: 54248
				public static LocString NAME = "Flash Mobber";

				// Token: 0x0400D3E9 RID: 54249
				public static LocString TOOLTIP = "This Duplicant is planning to show off their dance moves in their downtime";
			}

			// Token: 0x0200357C RID: 13692
			public class ROBODANCERDANCING
			{
				// Token: 0x0400D3EA RID: 54250
				public static LocString NAME = "Flash Mobber";

				// Token: 0x0400D3EB RID: 54251
				public static LocString TOOLTIP = "This Duplicant is showing off their dance moves to other Duplicants";
			}

			// Token: 0x0200357D RID: 13693
			public class BIONICCRITICALBATTERY
			{
				// Token: 0x0400D3EC RID: 54252
				public static LocString NAME = "Critical Power Level";

				// Token: 0x0400D3ED RID: 54253
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant's ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" is dangerously low\n\nThey will become incapacitated unless new ",
					UI.PRE_KEYWORD,
					"Power Banks",
					UI.PST_KEYWORD,
					" are delivered"
				});

				// Token: 0x0400D3EE RID: 54254
				public static LocString NOTIFICATION_NAME = "Critical Power Level";

				// Token: 0x0400D3EF RID: 54255
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"These Duplicants will become incapacitated if they can't find ",
					UI.PRE_KEYWORD,
					"Power Banks",
					UI.PST_KEYWORD,
					":"
				});
			}

			// Token: 0x0200357E RID: 13694
			public class REMOTEWORKER
			{
				// Token: 0x0200357F RID: 13695
				public class ENTERINGDOCK
				{
					// Token: 0x0400D3F0 RID: 54256
					public static LocString NAME = "Docking";

					// Token: 0x0400D3F1 RID: 54257
					public static LocString TOOLTIP = "This remote worker is entering its dock";
				}

				// Token: 0x02003580 RID: 13696
				public class UNREACHABLEDOCK
				{
					// Token: 0x0400D3F2 RID: 54258
					public static LocString NAME = "Unreachable Dock";

					// Token: 0x0400D3F3 RID: 54259
					public static LocString TOOLTIP = "This remote worker cannot reach its dock";
				}

				// Token: 0x02003581 RID: 13697
				public class NOHOMEDOCK
				{
					// Token: 0x0400D3F4 RID: 54260
					public static LocString NAME = "No Dock";

					// Token: 0x0400D3F5 RID: 54261
					public static LocString TOOLTIP = "This remote worker has no home dock and will self-destruct";
				}

				// Token: 0x02003582 RID: 13698
				public class POWERSTATUS
				{
					// Token: 0x0400D3F6 RID: 54262
					public static LocString NAME = "[{CHARGE} Power Remaining ({RATIO})]";

					// Token: 0x0400D3F7 RID: 54263
					public static LocString TOOLTIP = string.Concat(new string[]
					{
						"This remote worker has {CHARGE} remaining power\n\nWhen ",
						UI.PRE_KEYWORD,
						"Power",
						UI.PST_KEYWORD,
						" gets low, it will return to its dock to recharge"
					});
				}

				// Token: 0x02003583 RID: 13699
				public class LOWPOWER
				{
					// Token: 0x0400D3F8 RID: 54264
					public static LocString NAME = "Low Power";

					// Token: 0x0400D3F9 RID: 54265
					public static LocString TOOLTIP = string.Concat(new string[]
					{
						"This remote worker has low ",
						UI.PRE_KEYWORD,
						"Power",
						UI.PST_KEYWORD,
						"\n\nIt will recharge at its dock before accepting new chores"
					});
				}

				// Token: 0x02003584 RID: 13700
				public class OUTOFPOWER
				{
					// Token: 0x0400D3FA RID: 54266
					public static LocString NAME = "No Power";

					// Token: 0x0400D3FB RID: 54267
					public static LocString TOOLTIP = string.Concat(new string[]
					{
						"This remote worker cannot function without ",
						UI.PRE_KEYWORD,
						"Power",
						UI.PST_KEYWORD,
						"\n\nIt must be returned to its dock"
					});
				}

				// Token: 0x02003585 RID: 13701
				public class HIGHGUNK
				{
					// Token: 0x0400D3FC RID: 54268
					public static LocString NAME = "Gunk Clogged";

					// Token: 0x0400D3FD RID: 54269
					public static LocString TOOLTIP = string.Concat(new string[]
					{
						"This remote worker will dock to remove ",
						UI.PRE_KEYWORD,
						"Gunk",
						UI.PST_KEYWORD,
						" buildup before accepting new chores"
					});
				}

				// Token: 0x02003586 RID: 13702
				public class FULLGUNK
				{
					// Token: 0x0400D3FE RID: 54270
					public static LocString NAME = "Full of Gunk";

					// Token: 0x0400D3FF RID: 54271
					public static LocString TOOLTIP = string.Concat(new string[]
					{
						"This remote worker cannot function due to excessive ",
						UI.PRE_KEYWORD,
						"Gunk",
						UI.PST_KEYWORD,
						" buildup\n\nIt must be returned to its dock"
					});
				}

				// Token: 0x02003587 RID: 13703
				public class LOWOIL
				{
					// Token: 0x0400D400 RID: 54272
					public static LocString NAME = "Low Oil";

					// Token: 0x0400D401 RID: 54273
					public static LocString TOOLTIP = "This remote worker is low on oil\n\nIt will dock to refuel before accepting new chores";
				}

				// Token: 0x02003588 RID: 13704
				public class OUTOFOIL
				{
					// Token: 0x0400D402 RID: 54274
					public static LocString NAME = "No Oil";

					// Token: 0x0400D403 RID: 54275
					public static LocString TOOLTIP = string.Concat(new string[]
					{
						"This remote worker cannot function without ",
						UI.PRE_KEYWORD,
						"Oil",
						UI.PST_KEYWORD,
						"\n\nIt must be returned to its dock"
					});
				}

				// Token: 0x02003589 RID: 13705
				public class RECHARGING
				{
					// Token: 0x0400D404 RID: 54276
					public static LocString NAME = "Recharging";

					// Token: 0x0400D405 RID: 54277
					public static LocString TOOLTIP = "This remote worker is recharging its capacitor";
				}

				// Token: 0x0200358A RID: 13706
				public class OILING
				{
					// Token: 0x0400D406 RID: 54278
					public static LocString NAME = "Refilling Oil";

					// Token: 0x0400D407 RID: 54279
					public static LocString TOOLTIP = "This remote worker is lubricating its joints";
				}

				// Token: 0x0200358B RID: 13707
				public class DRAINING
				{
					// Token: 0x0400D408 RID: 54280
					public static LocString NAME = "Draining Gunk";

					// Token: 0x0400D409 RID: 54281
					public static LocString TOOLTIP = "This remote worker is unclogging its gears";
				}
			}
		}

		// Token: 0x0200358C RID: 13708
		public class DISEASES
		{
			// Token: 0x0400D40A RID: 54282
			public static LocString CURED_POPUP = "Cured of {0}";

			// Token: 0x0400D40B RID: 54283
			public static LocString INFECTED_POPUP = "Became infected by {0}";

			// Token: 0x0400D40C RID: 54284
			public static LocString ADDED_POPFX = "{0}: {1} Germs";

			// Token: 0x0400D40D RID: 54285
			public static LocString NOTIFICATION_TOOLTIP = "{0} contracted {1} from: {2}";

			// Token: 0x0400D40E RID: 54286
			public static LocString GERMS = "Germs";

			// Token: 0x0400D40F RID: 54287
			public static LocString GERMS_CONSUMED_DESCRIPTION = "A count of the number of germs this Duplicant is host to";

			// Token: 0x0400D410 RID: 54288
			public static LocString RECUPERATING = "Recuperating";

			// Token: 0x0400D411 RID: 54289
			public static LocString INFECTION_MODIFIER = "Recently consumed {0} ({1})";

			// Token: 0x0400D412 RID: 54290
			public static LocString INFECTION_MODIFIER_SOURCE = "Fighting off {0} from {1}";

			// Token: 0x0400D413 RID: 54291
			public static LocString INFECTED_MODIFIER = "Suppressed immune system";

			// Token: 0x0400D414 RID: 54292
			public static LocString LEGEND_POSTAMBLE = "\n•  Select an infected object for more details";

			// Token: 0x0400D415 RID: 54293
			public static LocString ATTRIBUTE_MODIFIER_SYMPTOMS = "{0}: {1}";

			// Token: 0x0400D416 RID: 54294
			public static LocString ATTRIBUTE_MODIFIER_SYMPTOMS_TOOLTIP = "Modifies {0} by {1}";

			// Token: 0x0400D417 RID: 54295
			public static LocString DEATH_SYMPTOM = "Death in {0} if untreated";

			// Token: 0x0400D418 RID: 54296
			public static LocString DEATH_SYMPTOM_TOOLTIP = "Without medical treatment, this Duplicant will die of their illness in {0}";

			// Token: 0x0400D419 RID: 54297
			public static LocString RESISTANCES_PANEL_TOOLTIP = "{0}";

			// Token: 0x0400D41A RID: 54298
			public static LocString IMMUNE_FROM_MISSING_REQUIRED_TRAIT = "Immune: Does not have {0}";

			// Token: 0x0400D41B RID: 54299
			public static LocString IMMUNE_FROM_HAVING_EXLCLUDED_TRAIT = "Immune: Has {0}";

			// Token: 0x0400D41C RID: 54300
			public static LocString IMMUNE_FROM_HAVING_EXCLUDED_EFFECT = "Immunity: Has {0}";

			// Token: 0x0400D41D RID: 54301
			public static LocString CONTRACTION_PROBABILITY = "{0} of {1}'s exposures to these germs will result in {2}";

			// Token: 0x0200358D RID: 13709
			public class STATUS_ITEM_TOOLTIP
			{
				// Token: 0x0400D41E RID: 54302
				public static LocString TEMPLATE = "{InfectionSource}{Duration}{Doctor}{Fatality}{Cures}{Bedrest}\n\n\n{Symptoms}";

				// Token: 0x0400D41F RID: 54303
				public static LocString DESCRIPTOR = "<b>{0} {1}</b>\n";

				// Token: 0x0400D420 RID: 54304
				public static LocString SYMPTOMS = "{0}\n";

				// Token: 0x0400D421 RID: 54305
				public static LocString INFECTION_SOURCE = "Contracted by: {0}\n";

				// Token: 0x0400D422 RID: 54306
				public static LocString DURATION = "Time to recovery: {0}\n";

				// Token: 0x0400D423 RID: 54307
				public static LocString CURES = "Remedies taken: {0}\n";

				// Token: 0x0400D424 RID: 54308
				public static LocString NOMEDICINETAKEN = "Remedies taken: None\n";

				// Token: 0x0400D425 RID: 54309
				public static LocString FATALITY = "Fatal if untreated in: {0}\n";

				// Token: 0x0400D426 RID: 54310
				public static LocString BEDREST = "Sick Bay assignment will allow faster recovery\n";

				// Token: 0x0400D427 RID: 54311
				public static LocString DOCTOR_REQUIRED = "Sick Bay assignment required for recovery\n";

				// Token: 0x0400D428 RID: 54312
				public static LocString DOCTORED = "Received medical treatment, recovery speed is increased\n";
			}

			// Token: 0x0200358E RID: 13710
			public class MEDICINE
			{
				// Token: 0x0400D429 RID: 54313
				public static LocString SELF_ADMINISTERED_BOOSTER = "Self-Administered: Anytime";

				// Token: 0x0400D42A RID: 54314
				public static LocString SELF_ADMINISTERED_BOOSTER_TOOLTIP = "Duplicants can give themselves this medicine, whether they are currently sick or not";

				// Token: 0x0400D42B RID: 54315
				public static LocString SELF_ADMINISTERED_CURE = "Self-Administered: Sick Only";

				// Token: 0x0400D42C RID: 54316
				public static LocString SELF_ADMINISTERED_CURE_TOOLTIP = "Duplicants can give themselves this medicine, but only while they are sick";

				// Token: 0x0400D42D RID: 54317
				public static LocString DOCTOR_ADMINISTERED_BOOSTER = "Doctor Administered: Anytime";

				// Token: 0x0400D42E RID: 54318
				public static LocString DOCTOR_ADMINISTERED_BOOSTER_TOOLTIP = "Duplicants can receive this medicine at a {Station}, whether they are currently sick or not\n\nThey cannot give it to themselves and must receive it from a friend with " + UI.PRE_KEYWORD + "Doctoring Skills" + UI.PST_KEYWORD;

				// Token: 0x0400D42F RID: 54319
				public static LocString DOCTOR_ADMINISTERED_CURE = "Doctor Administered: Sick Only";

				// Token: 0x0400D430 RID: 54320
				public static LocString DOCTOR_ADMINISTERED_CURE_TOOLTIP = "Duplicants can receive this medicine at a {Station}, but only while they are sick\n\nThey cannot give it to themselves and must receive it from a friend with " + UI.PRE_KEYWORD + "Doctoring Skills" + UI.PST_KEYWORD;

				// Token: 0x0400D431 RID: 54321
				public static LocString BOOSTER = UI.FormatAsLink("Immune Booster", "IMMUNE SYSTEM");

				// Token: 0x0400D432 RID: 54322
				public static LocString BOOSTER_TOOLTIP = "Boosters can be taken by both healthy and sick Duplicants to prevent potential disease";

				// Token: 0x0400D433 RID: 54323
				public static LocString CURES_ANY = "Alleviates " + UI.FormatAsLink("All Diseases", "DISEASE");

				// Token: 0x0400D434 RID: 54324
				public static LocString CURES_ANY_TOOLTIP = string.Concat(new string[]
				{
					"This is a nonspecific ",
					UI.PRE_KEYWORD,
					"Disease",
					UI.PST_KEYWORD,
					" treatment that can be taken by any sick Duplicant"
				});

				// Token: 0x0400D435 RID: 54325
				public static LocString CURES = "Alleviates {0}";

				// Token: 0x0400D436 RID: 54326
				public static LocString CURES_TOOLTIP = "This medicine is used to treat {0} and can only be taken by sick Duplicants";
			}

			// Token: 0x0200358F RID: 13711
			public class SEVERITY
			{
				// Token: 0x0400D437 RID: 54327
				public static LocString BENIGN = "Benign";

				// Token: 0x0400D438 RID: 54328
				public static LocString MINOR = "Minor";

				// Token: 0x0400D439 RID: 54329
				public static LocString MAJOR = "Major";

				// Token: 0x0400D43A RID: 54330
				public static LocString CRITICAL = "Critical";
			}

			// Token: 0x02003590 RID: 13712
			public class TYPE
			{
				// Token: 0x0400D43B RID: 54331
				public static LocString PATHOGEN = "Illness";

				// Token: 0x0400D43C RID: 54332
				public static LocString AILMENT = "Ailment";

				// Token: 0x0400D43D RID: 54333
				public static LocString INJURY = "Injury";
			}

			// Token: 0x02003591 RID: 13713
			public class TRIGGERS
			{
				// Token: 0x0400D43E RID: 54334
				public static LocString EATCOMPLETEEDIBLE = "May cause {Diseases}";

				// Token: 0x02003592 RID: 13714
				public class TOOLTIPS
				{
					// Token: 0x0400D43F RID: 54335
					public static LocString EATCOMPLETEEDIBLE = "May cause {Diseases}";
				}
			}

			// Token: 0x02003593 RID: 13715
			public class INFECTIONSOURCES
			{
				// Token: 0x0400D440 RID: 54336
				public static LocString INTERNAL_TEMPERATURE = "Extreme internal temperatures";

				// Token: 0x0400D441 RID: 54337
				public static LocString TOXIC_AREA = "Exposure to toxic areas";

				// Token: 0x0400D442 RID: 54338
				public static LocString FOOD = "Eating a germ-covered {0}";

				// Token: 0x0400D443 RID: 54339
				public static LocString AIR = "Breathing germ-filled {0}";

				// Token: 0x0400D444 RID: 54340
				public static LocString SKIN = "Skin contamination";

				// Token: 0x0400D445 RID: 54341
				public static LocString UNKNOWN = "Unknown source";
			}

			// Token: 0x02003594 RID: 13716
			public class DESCRIPTORS
			{
				// Token: 0x02003595 RID: 13717
				public class INFO
				{
					// Token: 0x0400D446 RID: 54342
					public static LocString FOODBORNE = "Contracted via ingestion\n" + UI.HORIZONTAL_RULE;

					// Token: 0x0400D447 RID: 54343
					public static LocString FOODBORNE_TOOLTIP = string.Concat(new string[]
					{
						"Duplicants may contract this ",
						UI.PRE_KEYWORD,
						"Disease",
						UI.PST_KEYWORD,
						" by ingesting ",
						UI.PRE_KEYWORD,
						"Food",
						UI.PST_KEYWORD,
						" contaminated with these ",
						UI.PRE_KEYWORD,
						"Germs",
						UI.PST_KEYWORD
					});

					// Token: 0x0400D448 RID: 54344
					public static LocString AIRBORNE = "Contracted via inhalation\n" + UI.HORIZONTAL_RULE;

					// Token: 0x0400D449 RID: 54345
					public static LocString AIRBORNE_TOOLTIP = string.Concat(new string[]
					{
						"Duplicants may contract this ",
						UI.PRE_KEYWORD,
						"Disease",
						UI.PST_KEYWORD,
						" by breathing ",
						ELEMENTS.OXYGEN.NAME,
						" containing these ",
						UI.PRE_KEYWORD,
						"Germs",
						UI.PST_KEYWORD
					});

					// Token: 0x0400D44A RID: 54346
					public static LocString SKINBORNE = "Contracted via physical contact\n" + UI.HORIZONTAL_RULE;

					// Token: 0x0400D44B RID: 54347
					public static LocString SKINBORNE_TOOLTIP = string.Concat(new string[]
					{
						"Duplicants may contract this ",
						UI.PRE_KEYWORD,
						"Disease",
						UI.PST_KEYWORD,
						" by touching objects contaminated with these ",
						UI.PRE_KEYWORD,
						"Germs",
						UI.PST_KEYWORD
					});

					// Token: 0x0400D44C RID: 54348
					public static LocString SUNBORNE = "Contracted via environmental exposure\n" + UI.HORIZONTAL_RULE;

					// Token: 0x0400D44D RID: 54349
					public static LocString SUNBORNE_TOOLTIP = string.Concat(new string[]
					{
						"Duplicants may contract this ",
						UI.PRE_KEYWORD,
						"Disease",
						UI.PST_KEYWORD,
						" through exposure to hazardous environments"
					});

					// Token: 0x0400D44E RID: 54350
					public static LocString GROWS_ON = "Multiplies in:";

					// Token: 0x0400D44F RID: 54351
					public static LocString GROWS_ON_TOOLTIP = string.Concat(new string[]
					{
						"These substances allow ",
						UI.PRE_KEYWORD,
						"Germs",
						UI.PST_KEYWORD,
						" to spread and reproduce"
					});

					// Token: 0x0400D450 RID: 54352
					public static LocString NEUTRAL_ON = "Survives in:";

					// Token: 0x0400D451 RID: 54353
					public static LocString NEUTRAL_ON_TOOLTIP = UI.PRE_KEYWORD + "Germs" + UI.PST_KEYWORD + " will survive contact with these substances, but will not reproduce";

					// Token: 0x0400D452 RID: 54354
					public static LocString DIES_SLOWLY_ON = "Inhibited by:";

					// Token: 0x0400D453 RID: 54355
					public static LocString DIES_SLOWLY_ON_TOOLTIP = string.Concat(new string[]
					{
						"Contact with these substances will slowly reduce ",
						UI.PRE_KEYWORD,
						"Germ",
						UI.PST_KEYWORD,
						" numbers"
					});

					// Token: 0x0400D454 RID: 54356
					public static LocString DIES_ON = "Killed by:";

					// Token: 0x0400D455 RID: 54357
					public static LocString DIES_ON_TOOLTIP = string.Concat(new string[]
					{
						"Contact with these substances kills ",
						UI.PRE_KEYWORD,
						"Germs",
						UI.PST_KEYWORD,
						" over time"
					});

					// Token: 0x0400D456 RID: 54358
					public static LocString DIES_QUICKLY_ON = "Disinfected by:";

					// Token: 0x0400D457 RID: 54359
					public static LocString DIES_QUICKLY_ON_TOOLTIP = "Contact with these substances will quickly kill these " + UI.PRE_KEYWORD + "Germs" + UI.PST_KEYWORD;

					// Token: 0x0400D458 RID: 54360
					public static LocString GROWS = "Multiplies";

					// Token: 0x0400D459 RID: 54361
					public static LocString GROWS_TOOLTIP = "Doubles germ count every {0}";

					// Token: 0x0400D45A RID: 54362
					public static LocString NEUTRAL = "Survives";

					// Token: 0x0400D45B RID: 54363
					public static LocString NEUTRAL_TOOLTIP = "Germ count remains static";

					// Token: 0x0400D45C RID: 54364
					public static LocString DIES_SLOWLY = "Inhibited";

					// Token: 0x0400D45D RID: 54365
					public static LocString DIES_SLOWLY_TOOLTIP = "Halves germ count every {0}";

					// Token: 0x0400D45E RID: 54366
					public static LocString DIES = "Dies";

					// Token: 0x0400D45F RID: 54367
					public static LocString DIES_TOOLTIP = "Halves germ count every {0}";

					// Token: 0x0400D460 RID: 54368
					public static LocString DIES_QUICKLY = "Disinfected";

					// Token: 0x0400D461 RID: 54369
					public static LocString DIES_QUICKLY_TOOLTIP = "Halves germ count every {0}";

					// Token: 0x0400D462 RID: 54370
					public static LocString GROWTH_FORMAT = "    • {0}";

					// Token: 0x0400D463 RID: 54371
					public static LocString TEMPERATURE_RANGE = "Temperature range: {0} to {1}";

					// Token: 0x0400D464 RID: 54372
					public static LocString TEMPERATURE_RANGE_TOOLTIP = string.Concat(new string[]
					{
						"These ",
						UI.PRE_KEYWORD,
						"Germs",
						UI.PST_KEYWORD,
						" can survive ",
						UI.PRE_KEYWORD,
						"Temperatures",
						UI.PST_KEYWORD,
						" between <b>{0}</b> and <b>{1}</b>\n\nThey thrive in ",
						UI.PRE_KEYWORD,
						"Temperatures",
						UI.PST_KEYWORD,
						" between <b>{2}</b> and <b>{3}</b>"
					});

					// Token: 0x0400D465 RID: 54373
					public static LocString PRESSURE_RANGE = "Pressure range: {0} to {1}\n";

					// Token: 0x0400D466 RID: 54374
					public static LocString PRESSURE_RANGE_TOOLTIP = string.Concat(new string[]
					{
						"These ",
						UI.PRE_KEYWORD,
						"Germs",
						UI.PST_KEYWORD,
						" can survive between <b>{0}</b> and <b>{1}</b> of pressure\n\nThey thrive in pressures between <b>{2}</b> and <b>{3}</b>"
					});
				}
			}

			// Token: 0x02003596 RID: 13718
			public class ALLDISEASES
			{
				// Token: 0x0400D467 RID: 54375
				public static LocString NAME = "All Diseases";
			}

			// Token: 0x02003597 RID: 13719
			public class NODISEASES
			{
				// Token: 0x0400D468 RID: 54376
				public static LocString NAME = "NO";
			}

			// Token: 0x02003598 RID: 13720
			public class FOODPOISONING
			{
				// Token: 0x0400D469 RID: 54377
				public static LocString NAME = UI.FormatAsLink("Food Poisoning", "FOODPOISONING");

				// Token: 0x0400D46A RID: 54378
				public static LocString LEGEND_HOVERTEXT = "Food Poisoning Germs present\n";

				// Token: 0x0400D46B RID: 54379
				public static LocString DESC = "Food and drinks tainted with Food Poisoning germs are unsafe to consume, as they cause vomiting and other...bodily unpleasantness.";
			}

			// Token: 0x02003599 RID: 13721
			public class SLIMELUNG
			{
				// Token: 0x0400D46C RID: 54380
				public static LocString NAME = UI.FormatAsLink("Slimelung", "SLIMELUNG");

				// Token: 0x0400D46D RID: 54381
				public static LocString LEGEND_HOVERTEXT = "Slimelung Germs present\n";

				// Token: 0x0400D46E RID: 54382
				public static LocString DESC = string.Concat(new string[]
				{
					"Slimelung germs are found in ",
					UI.FormatAsLink("Slime", "SLIMEMOLD"),
					" and ",
					UI.FormatAsLink("Polluted Oxygen", "CONTAMINATEDOXYGEN"),
					". Inhaling these germs can cause Duplicants to cough and struggle to breathe."
				});
			}

			// Token: 0x0200359A RID: 13722
			public class POLLENGERMS
			{
				// Token: 0x0400D46F RID: 54383
				public static LocString NAME = UI.FormatAsLink("Floral Scent", "POLLENGERMS");

				// Token: 0x0400D470 RID: 54384
				public static LocString LEGEND_HOVERTEXT = "Floral Scent allergens present\n";

				// Token: 0x0400D471 RID: 54385
				public static LocString DESC = "Floral Scent allergens trigger excessive sneezing fits in Duplicants who possess the Allergies trait.";
			}

			// Token: 0x0200359B RID: 13723
			public class ZOMBIESPORES
			{
				// Token: 0x0400D472 RID: 54386
				public static LocString NAME = UI.FormatAsLink("Zombie Spores", "ZOMBIESPORES");

				// Token: 0x0400D473 RID: 54387
				public static LocString LEGEND_HOVERTEXT = "Zombie Spores present\n";

				// Token: 0x0400D474 RID: 54388
				public static LocString DESC = "Zombie Spores are a parasitic brain fungus released by " + UI.FormatAsLink("Sporechids", "EVIL_FLOWER") + ". Duplicants who touch or inhale the spores risk becoming infected and temporarily losing motor control.";
			}

			// Token: 0x0200359C RID: 13724
			public class RADIATIONPOISONING
			{
				// Token: 0x0400D475 RID: 54389
				public static LocString NAME = UI.FormatAsLink("Radioactive Contamination", "RADIATIONPOISONING");

				// Token: 0x0400D476 RID: 54390
				public static LocString LEGEND_HOVERTEXT = "Radioactive contamination present\n";

				// Token: 0x0400D477 RID: 54391
				public static LocString DESC = string.Concat(new string[]
				{
					"Items tainted with Radioactive Contaminants emit low levels of ",
					UI.FormatAsLink("Radiation", "RADIATION"),
					" that can cause ",
					UI.FormatAsLink("Radiation Sickness", "RADIATIONSICKNESS"),
					". They are unaffected by pressure or temperature, but do degrade over time."
				});
			}

			// Token: 0x0200359D RID: 13725
			public class FOODSICKNESS
			{
				// Token: 0x0400D478 RID: 54392
				public static LocString NAME = UI.FormatAsLink("Food Poisoning", "FOODSICKNESS");

				// Token: 0x0400D479 RID: 54393
				public static LocString DESCRIPTION = "This Duplicant's last meal wasn't exactly food safe";

				// Token: 0x0400D47A RID: 54394
				public static LocString VOMIT_SYMPTOM = "Vomiting";

				// Token: 0x0400D47B RID: 54395
				public static LocString VOMIT_SYMPTOM_TOOLTIP = string.Concat(new string[]
				{
					"Duplicants periodically vomit throughout the day, producing additional ",
					UI.PRE_KEYWORD,
					"Germs",
					UI.PST_KEYWORD,
					" and losing ",
					UI.PRE_KEYWORD,
					"Calories",
					UI.PST_KEYWORD
				});

				// Token: 0x0400D47C RID: 54396
				public static LocString DESCRIPTIVE_SYMPTOMS = "Nonlethal. A Duplicant's body \"purges\" from both ends, causing extreme fatigue.";

				// Token: 0x0400D47D RID: 54397
				public static LocString DISEASE_SOURCE_DESCRIPTOR = "Currently infected with {2}.\n\nThis Duplicant will produce {1} when vomiting.";

				// Token: 0x0400D47E RID: 54398
				public static LocString DISEASE_SOURCE_DESCRIPTOR_TOOLTIP = "This Duplicant will vomit approximately every <b>{0}</b>\n\nEach time they vomit, they will release <b>{1}</b> and lose " + UI.PRE_KEYWORD + "Calories" + UI.PST_KEYWORD;
			}

			// Token: 0x0200359E RID: 13726
			public class SLIMESICKNESS
			{
				// Token: 0x0400D47F RID: 54399
				public static LocString NAME = UI.FormatAsLink("Slimelung", "SLIMESICKNESS");

				// Token: 0x0400D480 RID: 54400
				public static LocString DESCRIPTION = "This Duplicant's chest congestion is making it difficult to breathe";

				// Token: 0x0400D481 RID: 54401
				public static LocString COUGH_SYMPTOM = "Coughing";

				// Token: 0x0400D482 RID: 54402
				public static LocString COUGH_SYMPTOM_TOOLTIP = string.Concat(new string[]
				{
					"Duplicants periodically cough up ",
					ELEMENTS.CONTAMINATEDOXYGEN.NAME,
					", producing additional ",
					UI.PRE_KEYWORD,
					"Germs",
					UI.PST_KEYWORD
				});

				// Token: 0x0400D483 RID: 54403
				public static LocString DESCRIPTIVE_SYMPTOMS = "Lethal without medical treatment. Duplicants experience coughing and shortness of breath.";

				// Token: 0x0400D484 RID: 54404
				public static LocString DISEASE_SOURCE_DESCRIPTOR = "Currently infected with {2}.\n\nThis Duplicant will produce <b>{1}</b> when coughing.";

				// Token: 0x0400D485 RID: 54405
				public static LocString DISEASE_SOURCE_DESCRIPTOR_TOOLTIP = "This Duplicant will cough approximately every <b>{0}</b>\n\nEach time they cough, they will release <b>{1}</b>";
			}

			// Token: 0x0200359F RID: 13727
			public class ZOMBIESICKNESS
			{
				// Token: 0x0400D486 RID: 54406
				public static LocString NAME = UI.FormatAsLink("Zombie Spores", "ZOMBIESICKNESS");

				// Token: 0x0400D487 RID: 54407
				public static LocString DESCRIPTIVE_SYMPTOMS = "Duplicants lose much of their motor control and experience extreme discomfort.";

				// Token: 0x0400D488 RID: 54408
				public static LocString DESCRIPTION = "Fungal spores have infiltrated the Duplicant's head and are sending unnatural electrical impulses to their brain";

				// Token: 0x0400D489 RID: 54409
				public static LocString LEGEND_HOVERTEXT = "Area Causes Zombie Spores\n";
			}

			// Token: 0x020035A0 RID: 13728
			public class ALLERGIES
			{
				// Token: 0x0400D48A RID: 54410
				public static LocString NAME = UI.FormatAsLink("Allergic Reaction", "ALLERGIES");

				// Token: 0x0400D48B RID: 54411
				public static LocString DESCRIPTIVE_SYMPTOMS = "Allergens cause excessive sneezing fits";

				// Token: 0x0400D48C RID: 54412
				public static LocString DESCRIPTION = "Pollen and other irritants are causing this poor Duplicant's immune system to overreact, resulting in needless sneezing and congestion";
			}

			// Token: 0x020035A1 RID: 13729
			public class SUNBURNSICKNESS
			{
				// Token: 0x0400D48D RID: 54413
				public static LocString NAME = UI.FormatAsLink("Sunburn", "SUNBURNSICKNESS");

				// Token: 0x0400D48E RID: 54414
				public static LocString DESCRIPTION = "Extreme sun exposure has given this Duplicant a nasty burn.";

				// Token: 0x0400D48F RID: 54415
				public static LocString LEGEND_HOVERTEXT = "Area Causes Sunburn\n";

				// Token: 0x0400D490 RID: 54416
				public static LocString SUNEXPOSURE = "Sun Exposure";

				// Token: 0x0400D491 RID: 54417
				public static LocString DESCRIPTIVE_SYMPTOMS = "Nonlethal. Duplicants experience temporary discomfort due to dermatological damage.";
			}

			// Token: 0x020035A2 RID: 13730
			public class RADIATIONSICKNESS
			{
				// Token: 0x0400D492 RID: 54418
				public static LocString NAME = UI.FormatAsLink("Radioactive Contaminants", "RADIATIONSICKNESS");

				// Token: 0x0400D493 RID: 54419
				public static LocString DESCRIPTIVE_SYMPTOMS = "Extremely lethal. This Duplicant is not expected to survive.";

				// Token: 0x0400D494 RID: 54420
				public static LocString DESCRIPTION = string.Concat(new string[]
				{
					"This Duplicant is leaving a trail of ",
					UI.PRE_KEYWORD,
					"Radiation",
					UI.PST_KEYWORD,
					" behind them."
				});

				// Token: 0x0400D495 RID: 54421
				public static LocString LEGEND_HOVERTEXT = "Area Causes Radiation Sickness\n";

				// Token: 0x0400D496 RID: 54422
				public static LocString DESC = DUPLICANTS.DISEASES.RADIATIONPOISONING.DESC;
			}

			// Token: 0x020035A3 RID: 13731
			public class PUTRIDODOUR
			{
				// Token: 0x0400D497 RID: 54423
				public static LocString NAME = UI.FormatAsLink("Trench Stench", "PUTRIDODOUR");

				// Token: 0x0400D498 RID: 54424
				public static LocString DESCRIPTION = "\nThe pungent odor wafting off this Duplicant is nauseating to their peers";

				// Token: 0x0400D499 RID: 54425
				public static LocString CRINGE_EFFECT = "Smelled a putrid odor";

				// Token: 0x0400D49A RID: 54426
				public static LocString LEGEND_HOVERTEXT = "Trench Stench Germs Present\n";
			}
		}

		// Token: 0x020035A4 RID: 13732
		public class MODIFIERS
		{
			// Token: 0x0400D49B RID: 54427
			public static LocString MODIFIER_FORMAT = UI.PRE_KEYWORD + "{0}" + UI.PST_KEYWORD + ": {1}";

			// Token: 0x0400D49C RID: 54428
			public static LocString IMMUNITY_FORMAT = UI.PRE_KEYWORD + "{0}" + UI.PST_KEYWORD;

			// Token: 0x0400D49D RID: 54429
			public static LocString TIME_REMAINING = "Time Remaining: {0}";

			// Token: 0x0400D49E RID: 54430
			public static LocString TIME_TOTAL = "\nDuration: {0}";

			// Token: 0x0400D49F RID: 54431
			public static LocString EFFECT_IMMUNITIES_HEADER = UI.PRE_POS_MODIFIER + "Immune to:" + UI.PST_POS_MODIFIER;

			// Token: 0x0400D4A0 RID: 54432
			public static LocString EFFECT_HEADER = UI.PRE_POS_MODIFIER + "Effects:" + UI.PST_POS_MODIFIER;

			// Token: 0x020035A5 RID: 13733
			public class SKILLLEVEL
			{
				// Token: 0x0400D4A1 RID: 54433
				public static LocString NAME = "Skill Level";
			}

			// Token: 0x020035A6 RID: 13734
			public class ROOMPARK
			{
				// Token: 0x0400D4A2 RID: 54434
				public static LocString NAME = "Park";

				// Token: 0x0400D4A3 RID: 54435
				public static LocString TOOLTIP = "This Duplicant recently passed through a Park\n\nWow, nature sure is neat!";
			}

			// Token: 0x020035A7 RID: 13735
			public class ROOMNATURERESERVE
			{
				// Token: 0x0400D4A4 RID: 54436
				public static LocString NAME = "Nature Reserve";

				// Token: 0x0400D4A5 RID: 54437
				public static LocString TOOLTIP = "This Duplicant recently passed through a splendid Nature Reserve\n\nWow, nature sure is neat!";
			}

			// Token: 0x020035A8 RID: 13736
			public class ROOMLATRINE
			{
				// Token: 0x0400D4A6 RID: 54438
				public static LocString NAME = "Latrine";

				// Token: 0x0400D4A7 RID: 54439
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant used an ",
					BUILDINGS.PREFABS.OUTHOUSE.NAME,
					" in a ",
					UI.PRE_KEYWORD,
					"Latrine",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x020035A9 RID: 13737
			public class ROOMBATHROOM
			{
				// Token: 0x0400D4A8 RID: 54440
				public static LocString NAME = "Washroom";

				// Token: 0x0400D4A9 RID: 54441
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant used a ",
					BUILDINGS.PREFABS.FLUSHTOILET.NAME,
					" in a ",
					UI.PRE_KEYWORD,
					"Washroom",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x020035AA RID: 13738
			public class ROOMBIONICUPKEEP
			{
				// Token: 0x0400D4AA RID: 54442
				public static LocString NAME = "Workshop";

				// Token: 0x0400D4AB RID: 54443
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant used a ",
					BUILDINGS.PREFABS.GUNKEMPTIER.NAME,
					" in a ",
					UI.PRE_KEYWORD,
					"Workshop",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x020035AB RID: 13739
			public class FRESHOIL
			{
				// Token: 0x0400D4AC RID: 54444
				public static LocString NAME = "Fresh Oil";

				// Token: 0x0400D4AD RID: 54445
				public static LocString TOOLTIP = "This Duplicant recently used a " + BUILDINGS.PREFABS.OILCHANGER.NAME + " and feels pretty slick" + UI.PST_KEYWORD;
			}

			// Token: 0x020035AC RID: 13740
			public class ROOMBARRACKS
			{
				// Token: 0x0400D4AE RID: 54446
				public static LocString NAME = "Barracks";

				// Token: 0x0400D4AF RID: 54447
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant slept in the ",
					UI.PRE_KEYWORD,
					"Barracks",
					UI.PST_KEYWORD,
					" last night and feels refreshed"
				});
			}

			// Token: 0x020035AD RID: 13741
			public class ROOMBEDROOM
			{
				// Token: 0x0400D4B0 RID: 54448
				public static LocString NAME = "Luxury Barracks";

				// Token: 0x0400D4B1 RID: 54449
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant slept in a ",
					UI.PRE_KEYWORD,
					"Luxury Barracks",
					UI.PST_KEYWORD,
					" last night and feels extra refreshed"
				});
			}

			// Token: 0x020035AE RID: 13742
			public class ROOMPRIVATEBEDROOM
			{
				// Token: 0x0400D4B2 RID: 54450
				public static LocString NAME = "Private Bedroom";

				// Token: 0x0400D4B3 RID: 54451
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant slept in a ",
					UI.PRE_KEYWORD,
					"Private Bedroom",
					UI.PST_KEYWORD,
					" last night and feels super refreshed"
				});
			}

			// Token: 0x020035AF RID: 13743
			public class BEDHEALTH
			{
				// Token: 0x0400D4B4 RID: 54452
				public static LocString NAME = "Bed Rest";

				// Token: 0x0400D4B5 RID: 54453
				public static LocString TOOLTIP = "This Duplicant will incrementally heal over while on " + UI.PRE_KEYWORD + "Bed Rest" + UI.PST_KEYWORD;
			}

			// Token: 0x020035B0 RID: 13744
			public class BEDSTAMINA
			{
				// Token: 0x0400D4B6 RID: 54454
				public static LocString NAME = "Sleeping in a cot";

				// Token: 0x0400D4B7 RID: 54455
				public static LocString TOOLTIP = "This Duplicant's sleeping arrangements are adequate";
			}

			// Token: 0x020035B1 RID: 13745
			public class LUXURYBEDSTAMINA
			{
				// Token: 0x0400D4B8 RID: 54456
				public static LocString NAME = "Sleeping in a comfy bed";

				// Token: 0x0400D4B9 RID: 54457
				public static LocString TOOLTIP = "This Duplicant loves their snuggly bed";
			}

			// Token: 0x020035B2 RID: 13746
			public class BARRACKSSTAMINA
			{
				// Token: 0x0400D4BA RID: 54458
				public static LocString NAME = "Barracks";

				// Token: 0x0400D4BB RID: 54459
				public static LocString TOOLTIP = "This Duplicant shares sleeping quarters with others";
			}

			// Token: 0x020035B3 RID: 13747
			public class LADDERBEDSTAMINA
			{
				// Token: 0x0400D4BC RID: 54460
				public static LocString NAME = "Sleeping in a ladder bed";

				// Token: 0x0400D4BD RID: 54461
				public static LocString TOOLTIP = "This Duplicant's sleeping arrangements are adequate";
			}

			// Token: 0x020035B4 RID: 13748
			public class BEDROOMSTAMINA
			{
				// Token: 0x0400D4BE RID: 54462
				public static LocString NAME = "Private Bedroom";

				// Token: 0x0400D4BF RID: 54463
				public static LocString TOOLTIP = "This lucky Duplicant has their own private bedroom";
			}

			// Token: 0x020035B5 RID: 13749
			public class ROOMMESSHALL
			{
				// Token: 0x0400D4C0 RID: 54464
				public static LocString NAME = "Mess Hall";

				// Token: 0x0400D4C1 RID: 54465
				public static LocString TOOLTIP = "This Duplicant's most recent meal was eaten in a " + UI.PRE_KEYWORD + "Mess Hall" + UI.PST_KEYWORD;
			}

			// Token: 0x020035B6 RID: 13750
			public class ROOMGREATHALL
			{
				// Token: 0x0400D4C2 RID: 54466
				public static LocString NAME = "Great Hall";

				// Token: 0x0400D4C3 RID: 54467
				public static LocString TOOLTIP = "This Duplicant's most recent meal was eaten in a fancy " + UI.PRE_KEYWORD + "Great Hall" + UI.PST_KEYWORD;
			}

			// Token: 0x020035B7 RID: 13751
			public class ENTITLEMENT
			{
				// Token: 0x0400D4C4 RID: 54468
				public static LocString NAME = "Entitlement";

				// Token: 0x0400D4C5 RID: 54469
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Duplicants will demand better ",
					UI.PRE_KEYWORD,
					"Decor",
					UI.PST_KEYWORD,
					" and accommodations with each Expertise level they gain"
				});
			}

			// Token: 0x020035B8 RID: 13752
			public class HOMEOSTASIS
			{
				// Token: 0x0400D4C6 RID: 54470
				public static LocString NAME = "Homeostasis";
			}

			// Token: 0x020035B9 RID: 13753
			public class WARMAIR
			{
				// Token: 0x0400D4C7 RID: 54471
				public static LocString NAME = "Toasty Surroundings";
			}

			// Token: 0x020035BA RID: 13754
			public class COLDAIR
			{
				// Token: 0x0400D4C8 RID: 54472
				public static LocString NAME = "Chilly Surroundings";

				// Token: 0x0400D4C9 RID: 54473
				public static LocString CAUSE = "Duplicants tire quickly and lose body heat in cold environments";
			}

			// Token: 0x020035BB RID: 13755
			public class CLAUSTROPHOBIC
			{
				// Token: 0x0400D4CA RID: 54474
				public static LocString NAME = "Claustrophobic";

				// Token: 0x0400D4CB RID: 54475
				public static LocString TOOLTIP = "This Duplicant recently found themselves in an upsettingly cramped space";

				// Token: 0x0400D4CC RID: 54476
				public static LocString CAUSE = "This Duplicant got so good at their job that they became claustrophobic";
			}

			// Token: 0x020035BC RID: 13756
			public class VERTIGO
			{
				// Token: 0x0400D4CD RID: 54477
				public static LocString NAME = "Vertigo";

				// Token: 0x0400D4CE RID: 54478
				public static LocString TOOLTIP = "This Duplicant had to climb a tall ladder that left them dizzy and unsettled";

				// Token: 0x0400D4CF RID: 54479
				public static LocString CAUSE = "This Duplicant got so good at their job they became bad at ladders";
			}

			// Token: 0x020035BD RID: 13757
			public class UNCOMFORTABLEFEET
			{
				// Token: 0x0400D4D0 RID: 54480
				public static LocString NAME = "Aching Feet";

				// Token: 0x0400D4D1 RID: 54481
				public static LocString TOOLTIP = "This Duplicant recently walked across floor without tile, much to their chagrin";

				// Token: 0x0400D4D2 RID: 54482
				public static LocString CAUSE = "This Duplicant got so good at their job that their feet became sensitive";
			}

			// Token: 0x020035BE RID: 13758
			public class PEOPLETOOCLOSEWHILESLEEPING
			{
				// Token: 0x0400D4D3 RID: 54483
				public static LocString NAME = "Personal Bubble Burst";

				// Token: 0x0400D4D4 RID: 54484
				public static LocString TOOLTIP = "This Duplicant had to sleep too close to others and it was awkward for them";

				// Token: 0x0400D4D5 RID: 54485
				public static LocString CAUSE = "This Duplicant got so good at their job that they stopped being comfortable sleeping near other people";
			}

			// Token: 0x020035BF RID: 13759
			public class RESTLESS
			{
				// Token: 0x0400D4D6 RID: 54486
				public static LocString NAME = "Restless";

				// Token: 0x0400D4D7 RID: 54487
				public static LocString TOOLTIP = "This Duplicant went a few minutes without working and is now completely awash with guilt";

				// Token: 0x0400D4D8 RID: 54488
				public static LocString CAUSE = "This Duplicant got so good at their job that they forgot how to be comfortable doing anything else";
			}

			// Token: 0x020035C0 RID: 13760
			public class UNFASHIONABLECLOTHING
			{
				// Token: 0x0400D4D9 RID: 54489
				public static LocString NAME = "Fashion Crime";

				// Token: 0x0400D4DA RID: 54490
				public static LocString TOOLTIP = "This Duplicant had to wear something that was an affront to fashion";

				// Token: 0x0400D4DB RID: 54491
				public static LocString CAUSE = "This Duplicant got so good at their job that they became incapable of tolerating unfashionable clothing";
			}

			// Token: 0x020035C1 RID: 13761
			public class BURNINGCALORIES
			{
				// Token: 0x0400D4DC RID: 54492
				public static LocString NAME = "Homeostasis";
			}

			// Token: 0x020035C2 RID: 13762
			public class EATINGCALORIES
			{
				// Token: 0x0400D4DD RID: 54493
				public static LocString NAME = "Eating";
			}

			// Token: 0x020035C3 RID: 13763
			public class TEMPEXCHANGE
			{
				// Token: 0x0400D4DE RID: 54494
				public static LocString NAME = "Environmental Exchange";
			}

			// Token: 0x020035C4 RID: 13764
			public class CLOTHING
			{
				// Token: 0x0400D4DF RID: 54495
				public static LocString NAME = "Clothing";
			}

			// Token: 0x020035C5 RID: 13765
			public class CRYFACE
			{
				// Token: 0x0400D4E0 RID: 54496
				public static LocString NAME = "Cry Face";

				// Token: 0x0400D4E1 RID: 54497
				public static LocString TOOLTIP = "This Duplicant recently had a crying fit and it shows";

				// Token: 0x0400D4E2 RID: 54498
				public static LocString CAUSE = string.Concat(new string[]
				{
					"Obtained from the ",
					UI.PRE_KEYWORD,
					"Ugly Crier",
					UI.PST_KEYWORD,
					" stress reaction"
				});
			}

			// Token: 0x020035C6 RID: 13766
			public class WARMTOUCH
			{
				// Token: 0x0400D4E3 RID: 54499
				public static LocString NAME = "Frost Resistant";

				// Token: 0x0400D4E4 RID: 54500
				public static LocString TOOLTIP = "This Duplicant recently visited a warming station, sauna, or hot tub\n\nThey are impervious to cold as a result";

				// Token: 0x0400D4E5 RID: 54501
				public static LocString PROVIDERS_NAME = "Frost Resistance";

				// Token: 0x0400D4E6 RID: 54502
				public static LocString PROVIDERS_TOOLTIP = string.Concat(new string[]
				{
					"Using this building provides temporary immunity to ",
					UI.PRE_KEYWORD,
					"Chilly Surroundings",
					UI.PST_KEYWORD,
					" and ",
					UI.PRE_KEYWORD,
					"Soggy Feet",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x020035C7 RID: 13767
			public class REFRESHINGTOUCH
			{
				// Token: 0x0400D4E7 RID: 54503
				public static LocString NAME = "Heat Resistant";

				// Token: 0x0400D4E8 RID: 54504
				public static LocString TOOLTIP = "This Duplicant recently visited a cooling station and is totally unbothered by heat as a result";
			}

			// Token: 0x020035C8 RID: 13768
			public class GUNKSICK
			{
				// Token: 0x0400D4E9 RID: 54505
				public static LocString NAME = "Gunk Extraction Required";

				// Token: 0x0400D4EA RID: 54506
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant needs to visit a ",
					UI.PRE_KEYWORD,
					"Gunk Extractor",
					UI.PST_KEYWORD,
					" as soon as possible\n\nThey will use a toilet as a last resort"
				});
			}

			// Token: 0x020035C9 RID: 13769
			public class EXPELLINGGUNK
			{
				// Token: 0x0400D4EB RID: 54507
				public static LocString NAME = "Making a mess";

				// Token: 0x0400D4EC RID: 54508
				public static LocString TOOLTIP = "This Duplicant just couldn't hold it all in anymore";
			}

			// Token: 0x020035CA RID: 13770
			public class CLOGGINGTOILET
			{
				// Token: 0x0400D4ED RID: 54509
				public static LocString NAME = "Clogging a toilet";

				// Token: 0x0400D4EE RID: 54510
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is clogging a toilet with ",
					UI.PRE_KEYWORD,
					"Liquid Gunk",
					UI.PST_KEYWORD,
					"\n\nThey really should have used a ",
					UI.PRE_KEYWORD,
					"Gunk Extractor",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x020035CB RID: 13771
			public class GUNKHUNGOVER
			{
				// Token: 0x0400D4EF RID: 54511
				public static LocString NAME = "Gunk Mouth";

				// Token: 0x0400D4F0 RID: 54512
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant recently expelled built-up ",
					UI.PRE_KEYWORD,
					"Liquid Gunk",
					UI.PST_KEYWORD,
					" and can still taste it"
				});
			}

			// Token: 0x020035CC RID: 13772
			public class NOLUBRICATION
			{
				// Token: 0x0400D4F1 RID: 54513
				public static LocString NAME = "Grinding Gears";

				// Token: 0x0400D4F2 RID: 54514
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant's out of ",
					UI.PRE_KEYWORD,
					"Oil",
					UI.PST_KEYWORD,
					" and cannot function properly\n\nThey need to visit a ",
					UI.PRE_KEYWORD,
					"Lubrication Station",
					UI.PST_KEYWORD,
					" as soon as possible"
				});
			}

			// Token: 0x020035CD RID: 13773
			public class BIONICOFFLINE
			{
				// Token: 0x0400D4F3 RID: 54515
				public static LocString NAME = "Powerless";

				// Token: 0x0400D4F4 RID: 54516
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is non-functional!\n\nDeliver a charged ",
					UI.PRE_KEYWORD,
					"Power Bank",
					UI.PST_KEYWORD,
					" and reboot their systems to revive them"
				});
			}

			// Token: 0x020035CE RID: 13774
			public class WATERDAMAGE
			{
				// Token: 0x0400D4F5 RID: 54517
				public static LocString NAME = "Liquid Damage";

				// Token: 0x0400D4F6 RID: 54518
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant's bionic parts recently came into contact with incompatible ",
					UI.PRE_KEYWORD,
					"Liquids",
					UI.PST_KEYWORD,
					"\n\nProlonged exposure could have serious ",
					UI.PRE_KEYWORD,
					"Health",
					UI.PST_KEYWORD,
					" consequences"
				});
			}

			// Token: 0x020035CF RID: 13775
			public class SLIPPED
			{
				// Token: 0x0400D4F7 RID: 54519
				public static LocString NAME = "Slipped";

				// Token: 0x0400D4F8 RID: 54520
				public static LocString TOOLTIP = "This Duplicant recently lost their footing on a slippery floor and feels embarrassed";
			}

			// Token: 0x020035D0 RID: 13776
			public class BIONICBATTERYSAVEMODE
			{
				// Token: 0x0400D4F9 RID: 54521
				public static LocString NAME = "Peaceful Processing";

				// Token: 0x0400D4FA RID: 54522
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is enjoying a reprieve from the demands of high-wattage life\n\n",
					UI.PRE_KEYWORD,
					"Stress",
					UI.PST_KEYWORD,
					" is reduced while in Standby Mode"
				});
			}

			// Token: 0x020035D1 RID: 13777
			public class DUPLICANTGOTMILK
			{
				// Token: 0x0400D4FB RID: 54523
				public static LocString NAME = "Extra Hydrated";

				// Token: 0x0400D4FC RID: 54524
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant recently drank ",
					UI.PRE_KEYWORD,
					"Brackene",
					UI.PST_KEYWORD,
					". It's helping them relax"
				});
			}

			// Token: 0x020035D2 RID: 13778
			public class SOILEDSUIT
			{
				// Token: 0x0400D4FD RID: 54525
				public static LocString NAME = "Soiled Suit";

				// Token: 0x0400D4FE RID: 54526
				public static LocString TOOLTIP = "This Duplicant's suit needs to be emptied of waste\n\n(Preferably soon)";

				// Token: 0x0400D4FF RID: 54527
				public static LocString CAUSE = "Obtained when a Duplicant wears a suit filled with... \"fluids\"";
			}

			// Token: 0x020035D3 RID: 13779
			public class SHOWERED
			{
				// Token: 0x0400D500 RID: 54528
				public static LocString NAME = "Showered";

				// Token: 0x0400D501 RID: 54529
				public static LocString TOOLTIP = "This Duplicant recently had a shower and feels squeaky clean!";
			}

			// Token: 0x020035D4 RID: 13780
			public class SOREBACK
			{
				// Token: 0x0400D502 RID: 54530
				public static LocString NAME = "Sore Back";

				// Token: 0x0400D503 RID: 54531
				public static LocString TOOLTIP = "This Duplicant feels achy from sleeping on the floor last night and would like a bed";

				// Token: 0x0400D504 RID: 54532
				public static LocString CAUSE = "Obtained by sleeping on the ground";
			}

			// Token: 0x020035D5 RID: 13781
			public class GOODEATS
			{
				// Token: 0x0400D505 RID: 54533
				public static LocString NAME = "Soul Food";

				// Token: 0x0400D506 RID: 54534
				public static LocString TOOLTIP = "This Duplicant had a yummy home cooked meal and is totally stuffed";

				// Token: 0x0400D507 RID: 54535
				public static LocString CAUSE = "Obtained by eating a hearty home cooked meal";

				// Token: 0x0400D508 RID: 54536
				public static LocString DESCRIPTION = "Duplicants find this home cooked meal is emotionally comforting";
			}

			// Token: 0x020035D6 RID: 13782
			public class HOTSTUFF
			{
				// Token: 0x0400D509 RID: 54537
				public static LocString NAME = "Hot Stuff";

				// Token: 0x0400D50A RID: 54538
				public static LocString TOOLTIP = "This Duplicant had an extremely spicy meal and is both exhilarated and a little " + UI.PRE_KEYWORD + "Stressed" + UI.PST_KEYWORD;

				// Token: 0x0400D50B RID: 54539
				public static LocString CAUSE = "Obtained by eating a very spicy meal";

				// Token: 0x0400D50C RID: 54540
				public static LocString DESCRIPTION = "Duplicants find this spicy meal quite invigorating";
			}

			// Token: 0x020035D7 RID: 13783
			public class WARMTOUCHFOOD
			{
				// Token: 0x0400D50D RID: 54541
				public static LocString NAME = "Frost Resistant: Spicy Diet";

				// Token: 0x0400D50E RID: 54542
				public static LocString TOOLTIP = "This Duplicant ate spicy food and feels so warm inside that they don't even notice the cold right now";

				// Token: 0x0400D50F RID: 54543
				public static LocString CAUSE = "Obtained by eating a very spicy meal";

				// Token: 0x0400D510 RID: 54544
				public static LocString DESCRIPTION = string.Concat(new string[]
				{
					"Eating this provides temporary immunity to ",
					UI.PRE_KEYWORD,
					"Chilly Surroundings",
					UI.PST_KEYWORD,
					" and ",
					UI.PRE_KEYWORD,
					"Soggy Feet",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x020035D8 RID: 13784
			public class SEAFOODRADIATIONRESISTANCE
			{
				// Token: 0x0400D511 RID: 54545
				public static LocString NAME = "Radiation Resistant: Aquatic Diet";

				// Token: 0x0400D512 RID: 54546
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant ate sea-grown foods, which boost ",
					UI.PRE_KEYWORD,
					"Radiation",
					UI.PST_KEYWORD,
					" resistance"
				});

				// Token: 0x0400D513 RID: 54547
				public static LocString CAUSE = "Obtained by eating sea-grown foods like fish or lettuce";

				// Token: 0x0400D514 RID: 54548
				public static LocString DESCRIPTION = string.Concat(new string[]
				{
					"Eating this improves ",
					UI.PRE_KEYWORD,
					"Radiation",
					UI.PST_KEYWORD,
					" resistance"
				});
			}

			// Token: 0x020035D9 RID: 13785
			public class RECENTLYPARTIED
			{
				// Token: 0x0400D515 RID: 54549
				public static LocString NAME = "Partied Hard";

				// Token: 0x0400D516 RID: 54550
				public static LocString TOOLTIP = "This Duplicant recently attended a great party!";
			}

			// Token: 0x020035DA RID: 13786
			public class NOFUNALLOWED
			{
				// Token: 0x0400D517 RID: 54551
				public static LocString NAME = "Fun Interrupted";

				// Token: 0x0400D518 RID: 54552
				public static LocString TOOLTIP = "This Duplicant is upset a party was rejected";
			}

			// Token: 0x020035DB RID: 13787
			public class CONTAMINATEDLUNGS
			{
				// Token: 0x0400D519 RID: 54553
				public static LocString NAME = "Yucky Lungs";

				// Token: 0x0400D51A RID: 54554
				public static LocString TOOLTIP = "This Duplicant got a big nasty lungful of " + ELEMENTS.CONTAMINATEDOXYGEN.NAME;
			}

			// Token: 0x020035DC RID: 13788
			public class MINORIRRITATION
			{
				// Token: 0x0400D51B RID: 54555
				public static LocString NAME = "Minor Eye Irritation";

				// Token: 0x0400D51C RID: 54556
				public static LocString TOOLTIP = "A gas or liquid made this Duplicant's eyes sting a little";

				// Token: 0x0400D51D RID: 54557
				public static LocString CAUSE = "Obtained by exposure to a harsh liquid or gas";
			}

			// Token: 0x020035DD RID: 13789
			public class MAJORIRRITATION
			{
				// Token: 0x0400D51E RID: 54558
				public static LocString NAME = "Major Eye Irritation";

				// Token: 0x0400D51F RID: 54559
				public static LocString TOOLTIP = "Woah, something really messed up this Duplicant's eyes!\n\nCaused by exposure to a harsh liquid or gas";

				// Token: 0x0400D520 RID: 54560
				public static LocString CAUSE = "Obtained by exposure to a harsh liquid or gas";
			}

			// Token: 0x020035DE RID: 13790
			public class FRESH_AND_CLEAN
			{
				// Token: 0x0400D521 RID: 54561
				public static LocString NAME = "Refreshingly Clean";

				// Token: 0x0400D522 RID: 54562
				public static LocString TOOLTIP = "This Duplicant took a warm shower and it was great!";

				// Token: 0x0400D523 RID: 54563
				public static LocString CAUSE = "Obtained by taking a comfortably heated shower";
			}

			// Token: 0x020035DF RID: 13791
			public class BURNED_BY_SCALDING_WATER
			{
				// Token: 0x0400D524 RID: 54564
				public static LocString NAME = "Scalded";

				// Token: 0x0400D525 RID: 54565
				public static LocString TOOLTIP = "Ouch! This Duplicant showered or was doused in water that was way too hot";

				// Token: 0x0400D526 RID: 54566
				public static LocString CAUSE = "Obtained by exposure to hot water";
			}

			// Token: 0x020035E0 RID: 13792
			public class STRESSED_BY_COLD_WATER
			{
				// Token: 0x0400D527 RID: 54567
				public static LocString NAME = "Numb";

				// Token: 0x0400D528 RID: 54568
				public static LocString TOOLTIP = "Brr! This Duplicant was showered or doused in water that was way too cold";

				// Token: 0x0400D529 RID: 54569
				public static LocString CAUSE = "Obtained by exposure to icy water";
			}

			// Token: 0x020035E1 RID: 13793
			public class SMELLEDSTINKY
			{
				// Token: 0x0400D52A RID: 54570
				public static LocString NAME = "Smelled Stinky";

				// Token: 0x0400D52B RID: 54571
				public static LocString TOOLTIP = "This Duplicant got a whiff of a certain somebody";
			}

			// Token: 0x020035E2 RID: 13794
			public class STRESSREDUCTION
			{
				// Token: 0x0400D52C RID: 54572
				public static LocString NAME = "Receiving Massage";

				// Token: 0x0400D52D RID: 54573
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant's ",
					UI.PRE_KEYWORD,
					"Stress",
					UI.PST_KEYWORD,
					" is just melting away"
				});
			}

			// Token: 0x020035E3 RID: 13795
			public class STRESSREDUCTION_CLINIC
			{
				// Token: 0x0400D52E RID: 54574
				public static LocString NAME = "Receiving Clinic Massage";

				// Token: 0x0400D52F RID: 54575
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Clinical facilities are improving the effectiveness of this massage\n\nThis Duplicant's ",
					UI.PRE_KEYWORD,
					"Stress",
					UI.PST_KEYWORD,
					" is just melting away"
				});
			}

			// Token: 0x020035E4 RID: 13796
			public class UGLY_CRYING
			{
				// Token: 0x0400D530 RID: 54576
				public static LocString NAME = "Ugly Crying";

				// Token: 0x0400D531 RID: 54577
				public static LocString TOOLTIP = "This Duplicant is having a cathartic ugly cry as a result of " + UI.PRE_KEYWORD + "Stress" + UI.PST_KEYWORD;

				// Token: 0x0400D532 RID: 54578
				public static LocString NOTIFICATION_NAME = "Ugly Crying";

				// Token: 0x0400D533 RID: 54579
				public static LocString NOTIFICATION_TOOLTIP = "These Duplicants became so " + UI.FormatAsLink("Stressed", "STRESS") + " they broke down crying:";
			}

			// Token: 0x020035E5 RID: 13797
			public class BINGE_EATING
			{
				// Token: 0x0400D534 RID: 54580
				public static LocString NAME = "Insatiable Hunger";

				// Token: 0x0400D535 RID: 54581
				public static LocString TOOLTIP = "This Duplicant is stuffing their face as a result of " + UI.PRE_KEYWORD + "Stress" + UI.PST_KEYWORD;

				// Token: 0x0400D536 RID: 54582
				public static LocString NOTIFICATION_NAME = "Binge Eating";

				// Token: 0x0400D537 RID: 54583
				public static LocString NOTIFICATION_TOOLTIP = "These Duplicants became so " + UI.FormatAsLink("Stressed", "STRESS") + " they began overeating:";
			}

			// Token: 0x020035E6 RID: 13798
			public class BANSHEE_WAILING
			{
				// Token: 0x0400D538 RID: 54584
				public static LocString NAME = "Deafening Shriek";

				// Token: 0x0400D539 RID: 54585
				public static LocString TOOLTIP = "This Duplicant is wailing at the top of their lungs as a result of " + UI.PRE_KEYWORD + "Stress" + UI.PST_KEYWORD;

				// Token: 0x0400D53A RID: 54586
				public static LocString NOTIFICATION_NAME = "Banshee Wailing";

				// Token: 0x0400D53B RID: 54587
				public static LocString NOTIFICATION_TOOLTIP = "These Duplicants became so " + UI.FormatAsLink("Stressed", "STRESS") + " they began wailing:";
			}

			// Token: 0x020035E7 RID: 13799
			public class STRESSSHOCKER
			{
				// Token: 0x0400D53C RID: 54588
				public static LocString NAME = "Deafening Shriek";

				// Token: 0x0400D53D RID: 54589
				public static LocString TOOLTIP = "This Duplicant is wailing at the top of their lungs as a result of " + UI.PRE_KEYWORD + "Stress" + UI.PST_KEYWORD;

				// Token: 0x0400D53E RID: 54590
				public static LocString NOTIFICATION_NAME = "Banshee Wailing";

				// Token: 0x0400D53F RID: 54591
				public static LocString NOTIFICATION_TOOLTIP = "These Duplicants became so " + UI.FormatAsLink("Stressed", "STRESS") + " they began wailing:";
			}

			// Token: 0x020035E8 RID: 13800
			public class BANSHEE_WAILING_RECOVERY
			{
				// Token: 0x0400D540 RID: 54592
				public static LocString NAME = "Guzzling Air";

				// Token: 0x0400D541 RID: 54593
				public static LocString TOOLTIP = "This Duplicant needs a little extra oxygen to catch their breath";
			}

			// Token: 0x020035E9 RID: 13801
			public class METABOLISM_CALORIE_MODIFIER
			{
				// Token: 0x0400D542 RID: 54594
				public static LocString NAME = "Metabolism";

				// Token: 0x0400D543 RID: 54595
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					UI.PRE_KEYWORD,
					"Metabolism",
					UI.PST_KEYWORD,
					" determines how quickly a critter burns ",
					UI.PRE_KEYWORD,
					"Calories",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x020035EA RID: 13802
			public class WORKING
			{
				// Token: 0x0400D544 RID: 54596
				public static LocString NAME = "Working";

				// Token: 0x0400D545 RID: 54597
				public static LocString TOOLTIP = "This Duplicant is working up a sweat";
			}

			// Token: 0x020035EB RID: 13803
			public class UNCOMFORTABLESLEEP
			{
				// Token: 0x0400D546 RID: 54598
				public static LocString NAME = "Sleeping Uncomfortably";

				// Token: 0x0400D547 RID: 54599
				public static LocString TOOLTIP = "This Duplicant collapsed on the floor from sheer exhaustion";
			}

			// Token: 0x020035EC RID: 13804
			public class MANAGERIALDUTIES
			{
				// Token: 0x0400D548 RID: 54600
				public static LocString NAME = "Managerial Duties";

				// Token: 0x0400D549 RID: 54601
				public static LocString TOOLTIP = "Being a manager is stressful";
			}

			// Token: 0x020035ED RID: 13805
			public class MANAGEDCOLONY
			{
				// Token: 0x0400D54A RID: 54602
				public static LocString NAME = "Managed Colony";

				// Token: 0x0400D54B RID: 54603
				public static LocString TOOLTIP = "A Duplicant is in the colony manager job";
			}

			// Token: 0x020035EE RID: 13806
			public class BIONIC_WATTS
			{
				// Token: 0x0400D54C RID: 54604
				public static LocString NAME = "Wattage";

				// Token: 0x0400D54D RID: 54605
				public static LocString ESTIMATED_LIFE_TIME_REMAINING = string.Concat(new string[]
				{
					"Estimated ",
					UI.PRE_KEYWORD,
					"Power Bank",
					UI.PST_KEYWORD,
					" lifetime remaining: {0}"
				});

				// Token: 0x0400D54E RID: 54606
				public static LocString CURRENT_WATTAGE_TOOLTIP_LABEL = "Current Wattage: {0}";

				// Token: 0x0400D54F RID: 54607
				public static LocString POTENTIAL_EXTRA_WATTAGE_TOOLTIP_LABEL = "Potential Wattage: {0}";

				// Token: 0x0400D550 RID: 54608
				public static LocString STANDARD_ACTIVE_TEMPLATE = "{0}: {1}";

				// Token: 0x0400D551 RID: 54609
				public static LocString STANDARD_INACTIVE_TEMPLATE = "{0}: {1}";

				// Token: 0x0400D552 RID: 54610
				public static LocString SAVING_MODE_TEMPLATE = "{0} ({1}): {2}";

				// Token: 0x0400D553 RID: 54611
				public static LocString BASE_NAME = "Base";

				// Token: 0x0400D554 RID: 54612
				public static LocString SAVING_MODE_NAME = "Standby Mode";
			}

			// Token: 0x020035EF RID: 13807
			public class FLOORSLEEP
			{
				// Token: 0x0400D555 RID: 54613
				public static LocString NAME = "Sleeping On Floor";

				// Token: 0x0400D556 RID: 54614
				public static LocString TOOLTIP = "This Duplicant is uncomfortably recovering " + UI.PRE_KEYWORD + "Stamina" + UI.PST_KEYWORD;
			}

			// Token: 0x020035F0 RID: 13808
			public class PASSEDOUTSLEEP
			{
				// Token: 0x0400D557 RID: 54615
				public static LocString NAME = "Exhausted";

				// Token: 0x0400D558 RID: 54616
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Lack of rest depleted this Duplicant's ",
					UI.PRE_KEYWORD,
					"Stamina",
					UI.PST_KEYWORD,
					"\n\nThey passed out from the fatigue"
				});
			}

			// Token: 0x020035F1 RID: 13809
			public class SLEEP
			{
				// Token: 0x0400D559 RID: 54617
				public static LocString NAME = "Sleeping";

				// Token: 0x0400D55A RID: 54618
				public static LocString TOOLTIP = "This Duplicant is recovering " + UI.PRE_KEYWORD + "Stamina" + UI.PST_KEYWORD;
			}

			// Token: 0x020035F2 RID: 13810
			public class SLEEPCLINIC
			{
				// Token: 0x0400D55B RID: 54619
				public static LocString NAME = "Nodding Off";

				// Token: 0x0400D55C RID: 54620
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is losing ",
					UI.PRE_KEYWORD,
					"Stamina",
					UI.PST_KEYWORD,
					" because of their ",
					UI.PRE_KEYWORD,
					"Pajamas",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x020035F3 RID: 13811
			public class RESTFULSLEEP
			{
				// Token: 0x0400D55D RID: 54621
				public static LocString NAME = "Sleeping Peacefully";

				// Token: 0x0400D55E RID: 54622
				public static LocString TOOLTIP = "This Duplicant is getting a good night's rest";
			}

			// Token: 0x020035F4 RID: 13812
			public class SLEEPY
			{
				// Token: 0x0400D55F RID: 54623
				public static LocString NAME = "Sleepy";

				// Token: 0x0400D560 RID: 54624
				public static LocString TOOLTIP = "This Duplicant is getting tired";
			}

			// Token: 0x020035F5 RID: 13813
			public class HUNGRY
			{
				// Token: 0x0400D561 RID: 54625
				public static LocString NAME = "Hungry";

				// Token: 0x0400D562 RID: 54626
				public static LocString TOOLTIP = "This Duplicant is ready for lunch";
			}

			// Token: 0x020035F6 RID: 13814
			public class STARVING
			{
				// Token: 0x0400D563 RID: 54627
				public static LocString NAME = "Starving";

				// Token: 0x0400D564 RID: 54628
				public static LocString TOOLTIP = "This Duplicant needs to eat something, soon";
			}

			// Token: 0x020035F7 RID: 13815
			public class HOT
			{
				// Token: 0x0400D565 RID: 54629
				public static LocString NAME = "Hot";

				// Token: 0x0400D566 RID: 54630
				public static LocString TOOLTIP = "This Duplicant is uncomfortably warm";
			}

			// Token: 0x020035F8 RID: 13816
			public class COLD
			{
				// Token: 0x0400D567 RID: 54631
				public static LocString NAME = "Cold";

				// Token: 0x0400D568 RID: 54632
				public static LocString TOOLTIP = "This Duplicant is uncomfortably cold";
			}

			// Token: 0x020035F9 RID: 13817
			public class CARPETFEET
			{
				// Token: 0x0400D569 RID: 54633
				public static LocString NAME = "Tickled Tootsies";

				// Token: 0x0400D56A RID: 54634
				public static LocString TOOLTIP = "Walking on carpet has made this Duplicant's day a little more luxurious";
			}

			// Token: 0x020035FA RID: 13818
			public class WETFEET
			{
				// Token: 0x0400D56B RID: 54635
				public static LocString NAME = "Soggy Feet";

				// Token: 0x0400D56C RID: 54636
				public static LocString TOOLTIP = "This Duplicant recently stepped in " + UI.PRE_KEYWORD + "Liquid" + UI.PST_KEYWORD;

				// Token: 0x0400D56D RID: 54637
				public static LocString CAUSE = "Obtained by walking through liquid";
			}

			// Token: 0x020035FB RID: 13819
			public class SOAKINGWET
			{
				// Token: 0x0400D56E RID: 54638
				public static LocString NAME = "Sopping Wet";

				// Token: 0x0400D56F RID: 54639
				public static LocString TOOLTIP = "This Duplicant was recently submerged in " + UI.PRE_KEYWORD + "Liquid" + UI.PST_KEYWORD;

				// Token: 0x0400D570 RID: 54640
				public static LocString CAUSE = "Obtained from submergence in liquid";
			}

			// Token: 0x020035FC RID: 13820
			public class POPPEDEARDRUMS
			{
				// Token: 0x0400D571 RID: 54641
				public static LocString NAME = "Popped Eardrums";

				// Token: 0x0400D572 RID: 54642
				public static LocString TOOLTIP = "This Duplicant was exposed to an over-pressurized area that popped their eardrums";
			}

			// Token: 0x020035FD RID: 13821
			public class ANEWHOPE
			{
				// Token: 0x0400D573 RID: 54643
				public static LocString NAME = "New Hope";

				// Token: 0x0400D574 RID: 54644
				public static LocString TOOLTIP = "This Duplicant feels pretty optimistic about their new home";
			}

			// Token: 0x020035FE RID: 13822
			public class MEGABRAINTANKBONUS
			{
				// Token: 0x0400D575 RID: 54645
				public static LocString NAME = "Maximum Aptitude";

				// Token: 0x0400D576 RID: 54646
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is smarter and stronger than usual thanks to the ",
					UI.PRE_KEYWORD,
					"Somnium Synthesizer",
					UI.PST_KEYWORD,
					" "
				});
			}

			// Token: 0x020035FF RID: 13823
			public class PRICKLEFRUITDAMAGE
			{
				// Token: 0x0400D577 RID: 54647
				public static LocString NAME = "Ouch!";

				// Token: 0x0400D578 RID: 54648
				public static LocString TOOLTIP = "This Duplicant ate a raw " + UI.FormatAsLink("Bristle Berry", "PRICKLEFRUIT") + " and it gave their mouth ouchies";
			}

			// Token: 0x02003600 RID: 13824
			public class NOOXYGEN
			{
				// Token: 0x0400D579 RID: 54649
				public static LocString NAME = "No Oxygen";

				// Token: 0x0400D57A RID: 54650
				public static LocString TOOLTIP = "There is no breathable air in this area";
			}

			// Token: 0x02003601 RID: 13825
			public class LOWOXYGEN
			{
				// Token: 0x0400D57B RID: 54651
				public static LocString NAME = "Low Oxygen";

				// Token: 0x0400D57C RID: 54652
				public static LocString TOOLTIP = "The air is thin in this area";
			}

			// Token: 0x02003602 RID: 13826
			public class MOURNING
			{
				// Token: 0x0400D57D RID: 54653
				public static LocString NAME = "Mourning";

				// Token: 0x0400D57E RID: 54654
				public static LocString TOOLTIP = "This Duplicant is grieving the loss of a friend";
			}

			// Token: 0x02003603 RID: 13827
			public class NARCOLEPTICSLEEP
			{
				// Token: 0x0400D57F RID: 54655
				public static LocString NAME = "Narcoleptic Nap";

				// Token: 0x0400D580 RID: 54656
				public static LocString TOOLTIP = "This Duplicant just needs to rest their eyes for a second";
			}

			// Token: 0x02003604 RID: 13828
			public class BADSLEEP
			{
				// Token: 0x0400D581 RID: 54657
				public static LocString NAME = "Unrested: Too Bright";

				// Token: 0x0400D582 RID: 54658
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant tossed and turned all night because a ",
					UI.PRE_KEYWORD,
					"Light",
					UI.PST_KEYWORD,
					" was left on where they were trying to sleep"
				});
			}

			// Token: 0x02003605 RID: 13829
			public class BADSLEEPAFRAIDOFDARK
			{
				// Token: 0x0400D583 RID: 54659
				public static LocString NAME = "Unrested: Afraid of Dark";

				// Token: 0x0400D584 RID: 54660
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant didn't get much sleep because they were too anxious about the lack of ",
					UI.PRE_KEYWORD,
					"Light",
					UI.PST_KEYWORD,
					" to relax"
				});
			}

			// Token: 0x02003606 RID: 13830
			public class BADSLEEPMOVEMENT
			{
				// Token: 0x0400D585 RID: 54661
				public static LocString NAME = "Unrested: Bed Jostling";

				// Token: 0x0400D586 RID: 54662
				public static LocString TOOLTIP = "This Duplicant was woken up when a friend climbed on their ladder bed";
			}

			// Token: 0x02003607 RID: 13831
			public class BADSLEEPCOLD
			{
				// Token: 0x0400D587 RID: 54663
				public static LocString NAME = "Unrested: Cold Bedroom";

				// Token: 0x0400D588 RID: 54664
				public static LocString TOOLTIP = "This Duplicant was shivering instead of sleeping";
			}

			// Token: 0x02003608 RID: 13832
			public class TERRIBLESLEEP
			{
				// Token: 0x0400D589 RID: 54665
				public static LocString NAME = "Dead Tired: Snoring Friend";

				// Token: 0x0400D58A RID: 54666
				public static LocString TOOLTIP = "This Duplicant didn't get any shuteye last night because of all the racket from a friend's snoring";
			}

			// Token: 0x02003609 RID: 13833
			public class PEACEFULSLEEP
			{
				// Token: 0x0400D58B RID: 54667
				public static LocString NAME = "Well Rested";

				// Token: 0x0400D58C RID: 54668
				public static LocString TOOLTIP = "This Duplicant had a blissfully quiet sleep last night";
			}

			// Token: 0x0200360A RID: 13834
			public class CENTEROFATTENTION
			{
				// Token: 0x0400D58D RID: 54669
				public static LocString NAME = "Center of Attention";

				// Token: 0x0400D58E RID: 54670
				public static LocString TOOLTIP = "This Duplicant feels like someone's watching over them...";
			}

			// Token: 0x0200360B RID: 13835
			public class INSPIRED
			{
				// Token: 0x0400D58F RID: 54671
				public static LocString NAME = "Inspired";

				// Token: 0x0400D590 RID: 54672
				public static LocString TOOLTIP = "This Duplicant has had a creative vision!";
			}

			// Token: 0x0200360C RID: 13836
			public class NEWCREWARRIVAL
			{
				// Token: 0x0400D591 RID: 54673
				public static LocString NAME = "New Friend";

				// Token: 0x0400D592 RID: 54674
				public static LocString TOOLTIP = "This Duplicant is happy to see a new face in the colony";
			}

			// Token: 0x0200360D RID: 13837
			public class UNDERWATER
			{
				// Token: 0x0400D593 RID: 54675
				public static LocString NAME = "Underwater";

				// Token: 0x0400D594 RID: 54676
				public static LocString TOOLTIP = "This Duplicant's movement is slowed";
			}

			// Token: 0x0200360E RID: 13838
			public class NIGHTMARES
			{
				// Token: 0x0400D595 RID: 54677
				public static LocString NAME = "Nightmares";

				// Token: 0x0400D596 RID: 54678
				public static LocString TOOLTIP = "This Duplicant was visited by something in the night";
			}

			// Token: 0x0200360F RID: 13839
			public class WASATTACKED
			{
				// Token: 0x0400D597 RID: 54679
				public static LocString NAME = "Recently assailed";

				// Token: 0x0400D598 RID: 54680
				public static LocString TOOLTIP = "This Duplicant is stressed out after having been attacked";
			}

			// Token: 0x02003610 RID: 13840
			public class LIGHTWOUNDS
			{
				// Token: 0x0400D599 RID: 54681
				public static LocString NAME = "Light Wounds";

				// Token: 0x0400D59A RID: 54682
				public static LocString TOOLTIP = "This Duplicant sustained injuries that are a bit uncomfortable";
			}

			// Token: 0x02003611 RID: 13841
			public class MODERATEWOUNDS
			{
				// Token: 0x0400D59B RID: 54683
				public static LocString NAME = "Moderate Wounds";

				// Token: 0x0400D59C RID: 54684
				public static LocString TOOLTIP = "This Duplicant sustained injuries that are affecting their ability to work";
			}

			// Token: 0x02003612 RID: 13842
			public class SEVEREWOUNDS
			{
				// Token: 0x0400D59D RID: 54685
				public static LocString NAME = "Severe Wounds";

				// Token: 0x0400D59E RID: 54686
				public static LocString TOOLTIP = "This Duplicant sustained serious injuries that are impacting their work and well-being";
			}

			// Token: 0x02003613 RID: 13843
			public class LIGHTWOUNDSCRITTER
			{
				// Token: 0x0400D59F RID: 54687
				public static LocString NAME = "Light Wounds";

				// Token: 0x0400D5A0 RID: 54688
				public static LocString TOOLTIP = "This Critter sustained injuries that are a bit uncomfortable";
			}

			// Token: 0x02003614 RID: 13844
			public class MODERATEWOUNDSCRITTER
			{
				// Token: 0x0400D5A1 RID: 54689
				public static LocString NAME = "Moderate Wounds";

				// Token: 0x0400D5A2 RID: 54690
				public static LocString TOOLTIP = "This Critter sustained injuries that are really affecting their health";
			}

			// Token: 0x02003615 RID: 13845
			public class SEVEREWOUNDSCRITTER
			{
				// Token: 0x0400D5A3 RID: 54691
				public static LocString NAME = "Severe Wounds";

				// Token: 0x0400D5A4 RID: 54692
				public static LocString TOOLTIP = "This Critter sustained serious injuries that could prove life-threatening";
			}

			// Token: 0x02003616 RID: 13846
			public class SANDBOXMORALEADJUSTMENT
			{
				// Token: 0x0400D5A5 RID: 54693
				public static LocString NAME = "Sandbox Morale Adjustment";

				// Token: 0x0400D5A6 RID: 54694
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant has had their ",
					UI.PRE_KEYWORD,
					"Morale",
					UI.PST_KEYWORD,
					" temporarily adjusted using the Sandbox tools"
				});
			}

			// Token: 0x02003617 RID: 13847
			public class ROTTEMPERATURE
			{
				// Token: 0x0400D5A7 RID: 54695
				public static LocString UNREFRIGERATED = "Unrefrigerated";

				// Token: 0x0400D5A8 RID: 54696
				public static LocString REFRIGERATED = "Refrigerated";

				// Token: 0x0400D5A9 RID: 54697
				public static LocString FROZEN = "Frozen";
			}

			// Token: 0x02003618 RID: 13848
			public class ROTATMOSPHERE
			{
				// Token: 0x0400D5AA RID: 54698
				public static LocString CONTAMINATED = "Contaminated Air";

				// Token: 0x0400D5AB RID: 54699
				public static LocString NORMAL = "Normal Atmosphere";

				// Token: 0x0400D5AC RID: 54700
				public static LocString STERILE = "Sterile Atmosphere";
			}

			// Token: 0x02003619 RID: 13849
			public class BASEROT
			{
				// Token: 0x0400D5AD RID: 54701
				public static LocString NAME = "Base Decay Rate";
			}

			// Token: 0x0200361A RID: 13850
			public class FULLBLADDER
			{
				// Token: 0x0400D5AE RID: 54702
				public static LocString NAME = "Full Bladder";

				// Token: 0x0400D5AF RID: 54703
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant's ",
					UI.PRE_KEYWORD,
					"Bladder",
					UI.PST_KEYWORD,
					" is full"
				});
			}

			// Token: 0x0200361B RID: 13851
			public class DIARRHEA
			{
				// Token: 0x0400D5B0 RID: 54704
				public static LocString NAME = "Diarrhea";

				// Token: 0x0400D5B1 RID: 54705
				public static LocString TOOLTIP = "This Duplicant's gut is giving them some trouble";

				// Token: 0x0400D5B2 RID: 54706
				public static LocString CAUSE = "Obtained by eating a disgusting meal";

				// Token: 0x0400D5B3 RID: 54707
				public static LocString DESCRIPTION = "Most Duplicants experience stomach upset from this meal";
			}

			// Token: 0x0200361C RID: 13852
			public class STRESSFULYEMPTYINGBLADDER
			{
				// Token: 0x0400D5B4 RID: 54708
				public static LocString NAME = "Making a mess";

				// Token: 0x0400D5B5 RID: 54709
				public static LocString TOOLTIP = "This Duplicant had no choice but to empty their " + UI.PRE_KEYWORD + "Bladder" + UI.PST_KEYWORD;
			}

			// Token: 0x0200361D RID: 13853
			public class REDALERT
			{
				// Token: 0x0400D5B6 RID: 54710
				public static LocString NAME = "Red Alert!";

				// Token: 0x0400D5B7 RID: 54711
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"The ",
					UI.PRE_KEYWORD,
					"Red Alert",
					UI.PST_KEYWORD,
					" is stressing this Duplicant out"
				});
			}

			// Token: 0x0200361E RID: 13854
			public class FUSSY
			{
				// Token: 0x0400D5B8 RID: 54712
				public static LocString NAME = "Fussy";

				// Token: 0x0400D5B9 RID: 54713
				public static LocString TOOLTIP = "This Duplicant is hard to please";
			}

			// Token: 0x0200361F RID: 13855
			public class WARMINGUP
			{
				// Token: 0x0400D5BA RID: 54714
				public static LocString NAME = "Warming Up";

				// Token: 0x0400D5BB RID: 54715
				public static LocString TOOLTIP = "This Duplicant is trying to warm back up";
			}

			// Token: 0x02003620 RID: 13856
			public class COOLINGDOWN
			{
				// Token: 0x0400D5BC RID: 54716
				public static LocString NAME = "Cooling Down";

				// Token: 0x0400D5BD RID: 54717
				public static LocString TOOLTIP = "This Duplicant is trying to cool back down";
			}

			// Token: 0x02003621 RID: 13857
			public class DARKNESS
			{
				// Token: 0x0400D5BE RID: 54718
				public static LocString NAME = "Darkness";

				// Token: 0x0400D5BF RID: 54719
				public static LocString TOOLTIP = "Eep! This Duplicant doesn't like being in the dark!";
			}

			// Token: 0x02003622 RID: 13858
			public class STEPPEDINCONTAMINATEDWATER
			{
				// Token: 0x0400D5C0 RID: 54720
				public static LocString NAME = "Stepped in polluted water";

				// Token: 0x0400D5C1 RID: 54721
				public static LocString TOOLTIP = "Gross! This Duplicant stepped in something yucky";
			}

			// Token: 0x02003623 RID: 13859
			public class WELLFED
			{
				// Token: 0x0400D5C2 RID: 54722
				public static LocString NAME = "Well fed";

				// Token: 0x0400D5C3 RID: 54723
				public static LocString TOOLTIP = "This Duplicant feels satisfied after having a big meal";
			}

			// Token: 0x02003624 RID: 13860
			public class STALEFOOD
			{
				// Token: 0x0400D5C4 RID: 54724
				public static LocString NAME = "Bad leftovers";

				// Token: 0x0400D5C5 RID: 54725
				public static LocString TOOLTIP = "This Duplicant is in a bad mood from having to eat stale " + UI.PRE_KEYWORD + "Food" + UI.PST_KEYWORD;
			}

			// Token: 0x02003625 RID: 13861
			public class ATEFROZENFOOD
			{
				// Token: 0x0400D5C6 RID: 54726
				public static LocString NAME = "Ate frozen food";

				// Token: 0x0400D5C7 RID: 54727
				public static LocString TOOLTIP = "This Duplicant is in a bad mood from having to eat deep-frozen " + UI.PRE_KEYWORD + "Food" + UI.PST_KEYWORD;
			}

			// Token: 0x02003626 RID: 13862
			public class SMELLEDPUTRIDODOUR
			{
				// Token: 0x0400D5C8 RID: 54728
				public static LocString NAME = "Smelled a putrid odor";

				// Token: 0x0400D5C9 RID: 54729
				public static LocString TOOLTIP = "This Duplicant got a whiff of something unspeakably foul";
			}

			// Token: 0x02003627 RID: 13863
			public class VOMITING
			{
				// Token: 0x0400D5CA RID: 54730
				public static LocString NAME = "Vomiting";

				// Token: 0x0400D5CB RID: 54731
				public static LocString TOOLTIP = "Better out than in, as they say";
			}

			// Token: 0x02003628 RID: 13864
			public class BREATHING
			{
				// Token: 0x0400D5CC RID: 54732
				public static LocString NAME = "Breathing";
			}

			// Token: 0x02003629 RID: 13865
			public class HOLDINGBREATH
			{
				// Token: 0x0400D5CD RID: 54733
				public static LocString NAME = "Holding breath";
			}

			// Token: 0x0200362A RID: 13866
			public class RECOVERINGBREATH
			{
				// Token: 0x0400D5CE RID: 54734
				public static LocString NAME = "Recovering breath";
			}

			// Token: 0x0200362B RID: 13867
			public class ROTTING
			{
				// Token: 0x0400D5CF RID: 54735
				public static LocString NAME = "Rotting";
			}

			// Token: 0x0200362C RID: 13868
			public class DEAD
			{
				// Token: 0x0400D5D0 RID: 54736
				public static LocString NAME = "Dead";
			}

			// Token: 0x0200362D RID: 13869
			public class TOXICENVIRONMENT
			{
				// Token: 0x0400D5D1 RID: 54737
				public static LocString NAME = "Toxic environment";
			}

			// Token: 0x0200362E RID: 13870
			public class RESTING
			{
				// Token: 0x0400D5D2 RID: 54738
				public static LocString NAME = "Resting";
			}

			// Token: 0x0200362F RID: 13871
			public class INTRAVENOUS_NUTRITION
			{
				// Token: 0x0400D5D3 RID: 54739
				public static LocString NAME = "Intravenous Feeding";
			}

			// Token: 0x02003630 RID: 13872
			public class CATHETERIZED
			{
				// Token: 0x0400D5D4 RID: 54740
				public static LocString NAME = "Catheterized";

				// Token: 0x0400D5D5 RID: 54741
				public static LocString TOOLTIP = "Let's leave it at that";
			}

			// Token: 0x02003631 RID: 13873
			public class NOISEPEACEFUL
			{
				// Token: 0x0400D5D6 RID: 54742
				public static LocString NAME = "Peace and Quiet";

				// Token: 0x0400D5D7 RID: 54743
				public static LocString TOOLTIP = "This Duplicant has found a quiet place to concentrate";
			}

			// Token: 0x02003632 RID: 13874
			public class NOISEMINOR
			{
				// Token: 0x0400D5D8 RID: 54744
				public static LocString NAME = "Loud Noises";

				// Token: 0x0400D5D9 RID: 54745
				public static LocString TOOLTIP = "This area is a bit too loud for comfort";
			}

			// Token: 0x02003633 RID: 13875
			public class NOISEMAJOR
			{
				// Token: 0x0400D5DA RID: 54746
				public static LocString NAME = "Cacophony!";

				// Token: 0x0400D5DB RID: 54747
				public static LocString TOOLTIP = "It's very, very loud in here!";
			}

			// Token: 0x02003634 RID: 13876
			public class MEDICALCOT
			{
				// Token: 0x0400D5DC RID: 54748
				public static LocString NAME = "Triage Cot Rest";

				// Token: 0x0400D5DD RID: 54749
				public static LocString TOOLTIP = "Bedrest is improving this Duplicant's physical recovery time";
			}

			// Token: 0x02003635 RID: 13877
			public class MEDICALCOTDOCTORED
			{
				// Token: 0x0400D5DE RID: 54750
				public static LocString NAME = "Receiving treatment";

				// Token: 0x0400D5DF RID: 54751
				public static LocString TOOLTIP = "This Duplicant is receiving treatment for their physical injuries";
			}

			// Token: 0x02003636 RID: 13878
			public class DOCTOREDOFFCOTEFFECT
			{
				// Token: 0x0400D5E0 RID: 54752
				public static LocString NAME = "Runaway Patient";

				// Token: 0x0400D5E1 RID: 54753
				public static LocString TOOLTIP = "Tsk tsk!\nThis Duplicant cannot receive treatment while out of their medical bed!";
			}

			// Token: 0x02003637 RID: 13879
			public class POSTDISEASERECOVERY
			{
				// Token: 0x0400D5E2 RID: 54754
				public static LocString NAME = "Feeling better";

				// Token: 0x0400D5E3 RID: 54755
				public static LocString TOOLTIP = "This Duplicant is up and about, but they still have some lingering effects from their " + UI.PRE_KEYWORD + "Disease" + UI.PST_KEYWORD;

				// Token: 0x0400D5E4 RID: 54756
				public static LocString ADDITIONAL_EFFECTS = "This Duplicant has temporary immunity to diseases from having beaten an infection";
			}

			// Token: 0x02003638 RID: 13880
			public class IMMUNESYSTEMOVERWHELMED
			{
				// Token: 0x0400D5E5 RID: 54757
				public static LocString NAME = "Immune System Overwhelmed";

				// Token: 0x0400D5E6 RID: 54758
				public static LocString TOOLTIP = "This Duplicant's immune system is slowly being overwhelmed by a high concentration of germs";
			}

			// Token: 0x02003639 RID: 13881
			public class MEDICINE_GENERICPILL
			{
				// Token: 0x0400D5E7 RID: 54759
				public static LocString NAME = "Placebo";

				// Token: 0x0400D5E8 RID: 54760
				public static LocString TOOLTIP = ITEMS.PILLS.PLACEBO.DESC;

				// Token: 0x0400D5E9 RID: 54761
				public static LocString EFFECT_DESC = string.Concat(new string[]
				{
					"Applies the ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" effect"
				});
			}

			// Token: 0x0200363A RID: 13882
			public class MEDICINE_BASICBOOSTER
			{
				// Token: 0x0400D5EA RID: 54762
				public static LocString NAME = ITEMS.PILLS.BASICBOOSTER.NAME;

				// Token: 0x0400D5EB RID: 54763
				public static LocString TOOLTIP = ITEMS.PILLS.BASICBOOSTER.DESC;
			}

			// Token: 0x0200363B RID: 13883
			public class MEDICINE_INTERMEDIATEBOOSTER
			{
				// Token: 0x0400D5EC RID: 54764
				public static LocString NAME = ITEMS.PILLS.INTERMEDIATEBOOSTER.NAME;

				// Token: 0x0400D5ED RID: 54765
				public static LocString TOOLTIP = ITEMS.PILLS.INTERMEDIATEBOOSTER.DESC;
			}

			// Token: 0x0200363C RID: 13884
			public class MEDICINE_BASICRADPILL
			{
				// Token: 0x0400D5EE RID: 54766
				public static LocString NAME = ITEMS.PILLS.BASICRADPILL.NAME;

				// Token: 0x0400D5EF RID: 54767
				public static LocString TOOLTIP = ITEMS.PILLS.BASICRADPILL.DESC;
			}

			// Token: 0x0200363D RID: 13885
			public class MEDICINE_INTERMEDIATERADPILL
			{
				// Token: 0x0400D5F0 RID: 54768
				public static LocString NAME = ITEMS.PILLS.INTERMEDIATERADPILL.NAME;

				// Token: 0x0400D5F1 RID: 54769
				public static LocString TOOLTIP = ITEMS.PILLS.INTERMEDIATERADPILL.DESC;
			}

			// Token: 0x0200363E RID: 13886
			public class SUNLIGHT_PLEASANT
			{
				// Token: 0x0400D5F2 RID: 54770
				public static LocString NAME = "Bright and Cheerful";

				// Token: 0x0400D5F3 RID: 54771
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"The strong natural ",
					UI.PRE_KEYWORD,
					"Light",
					UI.PST_KEYWORD,
					" is making this Duplicant feel light on their feet"
				});
			}

			// Token: 0x0200363F RID: 13887
			public class SUNLIGHT_BURNING
			{
				// Token: 0x0400D5F4 RID: 54772
				public static LocString NAME = "Intensely Bright";

				// Token: 0x0400D5F5 RID: 54773
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"The bright ",
					UI.PRE_KEYWORD,
					"Light",
					UI.PST_KEYWORD,
					" is significantly improving this Duplicant's mood, but prolonged exposure may result in burning"
				});
			}

			// Token: 0x02003640 RID: 13888
			public class TOOKABREAK
			{
				// Token: 0x0400D5F6 RID: 54774
				public static LocString NAME = "Downtime";

				// Token: 0x0400D5F7 RID: 54775
				public static LocString TOOLTIP = "This Duplicant has a bit of time off from work to attend to personal matters";
			}

			// Token: 0x02003641 RID: 13889
			public class SOCIALIZED
			{
				// Token: 0x0400D5F8 RID: 54776
				public static LocString NAME = "Socialized";

				// Token: 0x0400D5F9 RID: 54777
				public static LocString TOOLTIP = "This Duplicant had some free time to hang out with buddies";
			}

			// Token: 0x02003642 RID: 13890
			public class GOODCONVERSATION
			{
				// Token: 0x0400D5FA RID: 54778
				public static LocString NAME = "Pleasant Chitchat";

				// Token: 0x0400D5FB RID: 54779
				public static LocString TOOLTIP = "This Duplicant recently had a chance to chat with a friend";
			}

			// Token: 0x02003643 RID: 13891
			public class WORKENCOURAGED
			{
				// Token: 0x0400D5FC RID: 54780
				public static LocString NAME = "Appreciated";

				// Token: 0x0400D5FD RID: 54781
				public static LocString TOOLTIP = "Someone saw how hard this Duplicant was working and gave them a compliment\n\nThis Duplicant feels great about themselves now!";
			}

			// Token: 0x02003644 RID: 13892
			public class ISSTICKERBOMBING
			{
				// Token: 0x0400D5FE RID: 54782
				public static LocString NAME = "Sticker Bombing";

				// Token: 0x0400D5FF RID: 54783
				public static LocString TOOLTIP = "This Duplicant is slapping stickers onto everything!\n\nEveryone's gonna love these";
			}

			// Token: 0x02003645 RID: 13893
			public class ISSPARKLESTREAKER
			{
				// Token: 0x0400D600 RID: 54784
				public static LocString NAME = "Sparkle Streaking";

				// Token: 0x0400D601 RID: 54785
				public static LocString TOOLTIP = "This Duplicant is currently Sparkle Streaking!\n\nBaa-ling!";
			}

			// Token: 0x02003646 RID: 13894
			public class SAWSPARKLESTREAKER
			{
				// Token: 0x0400D602 RID: 54786
				public static LocString NAME = "Sparkle Flattered";

				// Token: 0x0400D603 RID: 54787
				public static LocString TOOLTIP = "A Sparkle Streaker's sparkles dazzled this Duplicant\n\nThis Duplicant has a spring in their step now!";
			}

			// Token: 0x02003647 RID: 13895
			public class ISJOYSINGER
			{
				// Token: 0x0400D604 RID: 54788
				public static LocString NAME = "Yodeling";

				// Token: 0x0400D605 RID: 54789
				public static LocString TOOLTIP = "This Duplicant is currently Yodeling!\n\nHow melodious!";
			}

			// Token: 0x02003648 RID: 13896
			public class HEARDJOYSINGER
			{
				// Token: 0x0400D606 RID: 54790
				public static LocString NAME = "Serenaded";

				// Token: 0x0400D607 RID: 54791
				public static LocString TOOLTIP = "A Yodeler's singing thrilled this Duplicant\n\nThis Duplicant works at a higher tempo now!";
			}

			// Token: 0x02003649 RID: 13897
			public class ISROBODANCER
			{
				// Token: 0x0400D608 RID: 54792
				public static LocString NAME = "Doing the Robot";

				// Token: 0x0400D609 RID: 54793
				public static LocString TOOLTIP = "This Duplicant is dancing like everybody's watching\n\nThey're a flash mob of one!";
			}

			// Token: 0x0200364A RID: 13898
			public class SAWROBODANCER
			{
				// Token: 0x0400D60A RID: 54794
				public static LocString NAME = "Hyped";

				// Token: 0x0400D60B RID: 54795
				public static LocString TOOLTIP = "A Flash Mobber's dance moves wowed this Duplicant\n\nThis Duplicant feels amped up now!";
			}

			// Token: 0x0200364B RID: 13899
			public class HASBALLOON
			{
				// Token: 0x0400D60C RID: 54796
				public static LocString NAME = "Balloon Buddy";

				// Token: 0x0400D60D RID: 54797
				public static LocString TOOLTIP = "A Balloon Artist gave this Duplicant a balloon!\n\nThis Duplicant feels super crafty now!";
			}

			// Token: 0x0200364C RID: 13900
			public class GREETING
			{
				// Token: 0x0400D60E RID: 54798
				public static LocString NAME = "Saw Friend";

				// Token: 0x0400D60F RID: 54799
				public static LocString TOOLTIP = "This Duplicant recently saw a friend in the halls and got to say \"hi\"\n\nIt wasn't even awkward!";
			}

			// Token: 0x0200364D RID: 13901
			public class HUGGED
			{
				// Token: 0x0400D610 RID: 54800
				public static LocString NAME = "Hugged";

				// Token: 0x0400D611 RID: 54801
				public static LocString TOOLTIP = "This Duplicant recently received a hug from a friendly critter\n\nIt was so fluffy!";
			}

			// Token: 0x0200364E RID: 13902
			public class ARCADEPLAYING
			{
				// Token: 0x0400D612 RID: 54802
				public static LocString NAME = "Gaming";

				// Token: 0x0400D613 RID: 54803
				public static LocString TOOLTIP = "This Duplicant is playing a video game\n\nIt looks like fun!";
			}

			// Token: 0x0200364F RID: 13903
			public class PLAYEDARCADE
			{
				// Token: 0x0400D614 RID: 54804
				public static LocString NAME = "Played Video Games";

				// Token: 0x0400D615 RID: 54805
				public static LocString TOOLTIP = "This Duplicant recently played video games and is feeling like a champ";
			}

			// Token: 0x02003650 RID: 13904
			public class DANCING
			{
				// Token: 0x0400D616 RID: 54806
				public static LocString NAME = "Dancing";

				// Token: 0x0400D617 RID: 54807
				public static LocString TOOLTIP = "This Duplicant is showing off their best moves.";
			}

			// Token: 0x02003651 RID: 13905
			public class DANCED
			{
				// Token: 0x0400D618 RID: 54808
				public static LocString NAME = "Recently Danced";

				// Token: 0x0400D619 RID: 54809
				public static LocString TOOLTIP = "This Duplicant had a chance to cut loose!\n\nLeisure activities increase Duplicants' " + UI.PRE_KEYWORD + "Morale" + UI.PST_KEYWORD;
			}

			// Token: 0x02003652 RID: 13906
			public class JUICER
			{
				// Token: 0x0400D61A RID: 54810
				public static LocString NAME = "Drank Juice";

				// Token: 0x0400D61B RID: 54811
				public static LocString TOOLTIP = "This Duplicant had delicious fruity drink!\n\nLeisure activities increase Duplicants' " + UI.PRE_KEYWORD + "Morale" + UI.PST_KEYWORD;
			}

			// Token: 0x02003653 RID: 13907
			public class ESPRESSO
			{
				// Token: 0x0400D61C RID: 54812
				public static LocString NAME = "Drank Espresso";

				// Token: 0x0400D61D RID: 54813
				public static LocString TOOLTIP = "This Duplicant had delicious drink!\n\nLeisure activities increase Duplicants' " + UI.PRE_KEYWORD + "Morale" + UI.PST_KEYWORD;
			}

			// Token: 0x02003654 RID: 13908
			public class MECHANICALSURFBOARD
			{
				// Token: 0x0400D61E RID: 54814
				public static LocString NAME = "Stoked";

				// Token: 0x0400D61F RID: 54815
				public static LocString TOOLTIP = "This Duplicant had a rad experience on a surfboard.\n\nLeisure activities increase Duplicants' " + UI.PRE_KEYWORD + "Morale" + UI.PST_KEYWORD;
			}

			// Token: 0x02003655 RID: 13909
			public class MECHANICALSURFING
			{
				// Token: 0x0400D620 RID: 54816
				public static LocString NAME = "Surfin'";

				// Token: 0x0400D621 RID: 54817
				public static LocString TOOLTIP = "This Duplicant is surfin' some artificial waves!";
			}

			// Token: 0x02003656 RID: 13910
			public class SAUNA
			{
				// Token: 0x0400D622 RID: 54818
				public static LocString NAME = "Steam Powered";

				// Token: 0x0400D623 RID: 54819
				public static LocString TOOLTIP = "This Duplicant just had a relaxing time in a sauna\n\nLeisure activities increase Duplicants' " + UI.PRE_KEYWORD + "Morale" + UI.PST_KEYWORD;
			}

			// Token: 0x02003657 RID: 13911
			public class SAUNARELAXING
			{
				// Token: 0x0400D624 RID: 54820
				public static LocString NAME = "Relaxing";

				// Token: 0x0400D625 RID: 54821
				public static LocString TOOLTIP = "This Duplicant is relaxing in a sauna";
			}

			// Token: 0x02003658 RID: 13912
			public class HOTTUB
			{
				// Token: 0x0400D626 RID: 54822
				public static LocString NAME = "Hot Tubbed";

				// Token: 0x0400D627 RID: 54823
				public static LocString TOOLTIP = "This Duplicant recently unwound in a Hot Tub\n\nLeisure activities increase Duplicants' " + UI.PRE_KEYWORD + "Morale" + UI.PST_KEYWORD;
			}

			// Token: 0x02003659 RID: 13913
			public class HOTTUBRELAXING
			{
				// Token: 0x0400D628 RID: 54824
				public static LocString NAME = "Relaxing";

				// Token: 0x0400D629 RID: 54825
				public static LocString TOOLTIP = "This Duplicant is unwinding in a hot tub\n\nThey sure look relaxed";
			}

			// Token: 0x0200365A RID: 13914
			public class SODAFOUNTAIN
			{
				// Token: 0x0400D62A RID: 54826
				public static LocString NAME = "Soda Filled";

				// Token: 0x0400D62B RID: 54827
				public static LocString TOOLTIP = "This Duplicant just enjoyed a bubbly beverage\n\nLeisure activities increase Duplicants' " + UI.PRE_KEYWORD + "Morale" + UI.PST_KEYWORD;
			}

			// Token: 0x0200365B RID: 13915
			public class VERTICALWINDTUNNELFLYING
			{
				// Token: 0x0400D62C RID: 54828
				public static LocString NAME = "Airborne";

				// Token: 0x0400D62D RID: 54829
				public static LocString TOOLTIP = "This Duplicant is having an exhilarating time in the wind tunnel\n\nWhoosh!";
			}

			// Token: 0x0200365C RID: 13916
			public class VERTICALWINDTUNNEL
			{
				// Token: 0x0400D62E RID: 54830
				public static LocString NAME = "Wind Swept";

				// Token: 0x0400D62F RID: 54831
				public static LocString TOOLTIP = "This Duplicant recently had an exhilarating wind tunnel experience\n\nLeisure activities increase Duplicants' " + UI.PRE_KEYWORD + "Morale" + UI.PST_KEYWORD;
			}

			// Token: 0x0200365D RID: 13917
			public class BEACHCHAIRRELAXING
			{
				// Token: 0x0400D630 RID: 54832
				public static LocString NAME = "Totally Chill";

				// Token: 0x0400D631 RID: 54833
				public static LocString TOOLTIP = "This Duplicant is totally chillin' in a beach chair";
			}

			// Token: 0x0200365E RID: 13918
			public class BEACHCHAIRLIT
			{
				// Token: 0x0400D632 RID: 54834
				public static LocString NAME = "Sun Kissed";

				// Token: 0x0400D633 RID: 54835
				public static LocString TOOLTIP = "This Duplicant had an amazing experience at the Beach\n\nLeisure activities increase Duplicants' " + UI.PRE_KEYWORD + "Morale" + UI.PST_KEYWORD;
			}

			// Token: 0x0200365F RID: 13919
			public class BEACHCHAIRUNLIT
			{
				// Token: 0x0400D634 RID: 54836
				public static LocString NAME = "Passably Relaxed";

				// Token: 0x0400D635 RID: 54837
				public static LocString TOOLTIP = "This Duplicant just had a mediocre beach experience\n\nLeisure activities increase Duplicants' " + UI.PRE_KEYWORD + "Morale" + UI.PST_KEYWORD;
			}

			// Token: 0x02003660 RID: 13920
			public class TELEPHONECHAT
			{
				// Token: 0x0400D636 RID: 54838
				public static LocString NAME = "Full of Gossip";

				// Token: 0x0400D637 RID: 54839
				public static LocString TOOLTIP = "This Duplicant chatted on the phone with at least one other Duplicant\n\nLeisure activities increase Duplicants' " + UI.PRE_KEYWORD + "Morale" + UI.PST_KEYWORD;
			}

			// Token: 0x02003661 RID: 13921
			public class TELEPHONEBABBLE
			{
				// Token: 0x0400D638 RID: 54840
				public static LocString NAME = "Less Anxious";

				// Token: 0x0400D639 RID: 54841
				public static LocString TOOLTIP = "This Duplicant got some things off their chest by talking to themselves on the phone\n\nLeisure activities increase Duplicants' " + UI.PRE_KEYWORD + "Morale" + UI.PST_KEYWORD;
			}

			// Token: 0x02003662 RID: 13922
			public class TELEPHONELONGDISTANCE
			{
				// Token: 0x0400D63A RID: 54842
				public static LocString NAME = "Sociable";

				// Token: 0x0400D63B RID: 54843
				public static LocString TOOLTIP = "This Duplicant is feeling sociable after chatting on the phone with someone across space\n\nLeisure activities increase Duplicants' " + UI.PRE_KEYWORD + "Morale" + UI.PST_KEYWORD;
			}

			// Token: 0x02003663 RID: 13923
			public class EDIBLEMINUS3
			{
				// Token: 0x0400D63C RID: 54844
				public static LocString NAME = "Grisly Meal";

				// Token: 0x0400D63D RID: 54845
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"The food this Duplicant last ate was ",
					UI.PRE_KEYWORD,
					"Grisly",
					UI.PST_KEYWORD,
					"\n\nThey hope their next meal will be better"
				});
			}

			// Token: 0x02003664 RID: 13924
			public class EDIBLEMINUS2
			{
				// Token: 0x0400D63E RID: 54846
				public static LocString NAME = "Terrible Meal";

				// Token: 0x0400D63F RID: 54847
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"The food this Duplicant last ate was ",
					UI.PRE_KEYWORD,
					"Terrible",
					UI.PST_KEYWORD,
					"\n\nThey hope their next meal will be better"
				});
			}

			// Token: 0x02003665 RID: 13925
			public class EDIBLEMINUS1
			{
				// Token: 0x0400D640 RID: 54848
				public static LocString NAME = "Poor Meal";

				// Token: 0x0400D641 RID: 54849
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"The food this Duplicant last ate was ",
					UI.PRE_KEYWORD,
					"Poor",
					UI.PST_KEYWORD,
					"\n\nThey hope their next meal will be a little better"
				});
			}

			// Token: 0x02003666 RID: 13926
			public class EDIBLE0
			{
				// Token: 0x0400D642 RID: 54850
				public static LocString NAME = "Standard Meal";

				// Token: 0x0400D643 RID: 54851
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"The food this Duplicant last ate was ",
					UI.PRE_KEYWORD,
					"Average",
					UI.PST_KEYWORD,
					"\n\nThey thought it was sort of okay"
				});
			}

			// Token: 0x02003667 RID: 13927
			public class EDIBLE1
			{
				// Token: 0x0400D644 RID: 54852
				public static LocString NAME = "Good Meal";

				// Token: 0x0400D645 RID: 54853
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"The food this Duplicant last ate was ",
					UI.PRE_KEYWORD,
					"Good",
					UI.PST_KEYWORD,
					"\n\nThey thought it was pretty good!"
				});
			}

			// Token: 0x02003668 RID: 13928
			public class EDIBLE2
			{
				// Token: 0x0400D646 RID: 54854
				public static LocString NAME = "Great Meal";

				// Token: 0x0400D647 RID: 54855
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"The food this Duplicant last ate was ",
					UI.PRE_KEYWORD,
					"Great",
					UI.PST_KEYWORD,
					"\n\nThey thought it was pretty good!"
				});
			}

			// Token: 0x02003669 RID: 13929
			public class EDIBLE3
			{
				// Token: 0x0400D648 RID: 54856
				public static LocString NAME = "Superb Meal";

				// Token: 0x0400D649 RID: 54857
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"The food this Duplicant last ate was ",
					UI.PRE_KEYWORD,
					"Superb",
					UI.PST_KEYWORD,
					"\n\nThey thought it was really good!"
				});
			}

			// Token: 0x0200366A RID: 13930
			public class EDIBLE4
			{
				// Token: 0x0400D64A RID: 54858
				public static LocString NAME = "Ambrosial Meal";

				// Token: 0x0400D64B RID: 54859
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"The food this Duplicant last ate was ",
					UI.PRE_KEYWORD,
					"Ambrosial",
					UI.PST_KEYWORD,
					"\n\nThey thought it was super tasty!"
				});
			}

			// Token: 0x0200366B RID: 13931
			public class DECORMINUS1
			{
				// Token: 0x0400D64C RID: 54860
				public static LocString NAME = "Last Cycle's Decor: Ugly";

				// Token: 0x0400D64D RID: 54861
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant thought the overall ",
					UI.PRE_KEYWORD,
					"Decor",
					UI.PST_KEYWORD,
					" yesterday was downright depressing"
				});
			}

			// Token: 0x0200366C RID: 13932
			public class DECOR0
			{
				// Token: 0x0400D64E RID: 54862
				public static LocString NAME = "Last Cycle's Decor: Poor";

				// Token: 0x0400D64F RID: 54863
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant thought the overall ",
					UI.PRE_KEYWORD,
					"Decor",
					UI.PST_KEYWORD,
					" yesterday was quite poor"
				});
			}

			// Token: 0x0200366D RID: 13933
			public class DECOR1
			{
				// Token: 0x0400D650 RID: 54864
				public static LocString NAME = "Last Cycle's Decor: Mediocre";

				// Token: 0x0400D651 RID: 54865
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant had no strong opinions about the colony's ",
					UI.PRE_KEYWORD,
					"Decor",
					UI.PST_KEYWORD,
					" yesterday"
				});
			}

			// Token: 0x0200366E RID: 13934
			public class DECOR2
			{
				// Token: 0x0400D652 RID: 54866
				public static LocString NAME = "Last Cycle's Decor: Average";

				// Token: 0x0400D653 RID: 54867
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant thought the overall ",
					UI.PRE_KEYWORD,
					"Decor",
					UI.PST_KEYWORD,
					" yesterday was pretty alright"
				});
			}

			// Token: 0x0200366F RID: 13935
			public class DECOR3
			{
				// Token: 0x0400D654 RID: 54868
				public static LocString NAME = "Last Cycle's Decor: Nice";

				// Token: 0x0400D655 RID: 54869
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant thought the overall ",
					UI.PRE_KEYWORD,
					"Decor",
					UI.PST_KEYWORD,
					" yesterday was quite nice!"
				});
			}

			// Token: 0x02003670 RID: 13936
			public class DECOR4
			{
				// Token: 0x0400D656 RID: 54870
				public static LocString NAME = "Last Cycle's Decor: Charming";

				// Token: 0x0400D657 RID: 54871
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant thought the overall ",
					UI.PRE_KEYWORD,
					"Decor",
					UI.PST_KEYWORD,
					" yesterday was downright charming!"
				});
			}

			// Token: 0x02003671 RID: 13937
			public class DECOR5
			{
				// Token: 0x0400D658 RID: 54872
				public static LocString NAME = "Last Cycle's Decor: Gorgeous";

				// Token: 0x0400D659 RID: 54873
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant thought the overall ",
					UI.PRE_KEYWORD,
					"Decor",
					UI.PST_KEYWORD,
					" yesterday was fantastic\n\nThey love what I've done with the place!"
				});
			}

			// Token: 0x02003672 RID: 13938
			public class BREAK1
			{
				// Token: 0x0400D65A RID: 54874
				public static LocString NAME = "One Shift Break";

				// Token: 0x0400D65B RID: 54875
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant has had one ",
					UI.PRE_KEYWORD,
					"Downtime",
					UI.PST_KEYWORD,
					" shift in the last cycle"
				});
			}

			// Token: 0x02003673 RID: 13939
			public class BREAK2
			{
				// Token: 0x0400D65C RID: 54876
				public static LocString NAME = "Two Shift Break";

				// Token: 0x0400D65D RID: 54877
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant has had two ",
					UI.PRE_KEYWORD,
					"Downtime",
					UI.PST_KEYWORD,
					" shifts in the last cycle"
				});
			}

			// Token: 0x02003674 RID: 13940
			public class BREAK3
			{
				// Token: 0x0400D65E RID: 54878
				public static LocString NAME = "Three Shift Break";

				// Token: 0x0400D65F RID: 54879
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant has had three ",
					UI.PRE_KEYWORD,
					"Downtime",
					UI.PST_KEYWORD,
					" shifts in the last cycle"
				});
			}

			// Token: 0x02003675 RID: 13941
			public class BREAK4
			{
				// Token: 0x0400D660 RID: 54880
				public static LocString NAME = "Four Shift Break";

				// Token: 0x0400D661 RID: 54881
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant has had four ",
					UI.PRE_KEYWORD,
					"Downtime",
					UI.PST_KEYWORD,
					" shifts in the last cycle"
				});
			}

			// Token: 0x02003676 RID: 13942
			public class BREAK5
			{
				// Token: 0x0400D662 RID: 54882
				public static LocString NAME = "Five Shift Break";

				// Token: 0x0400D663 RID: 54883
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant has had five ",
					UI.PRE_KEYWORD,
					"Downtime",
					UI.PST_KEYWORD,
					" shifts in the last cycle"
				});
			}

			// Token: 0x02003677 RID: 13943
			public class POWERTINKER
			{
				// Token: 0x0400D664 RID: 54884
				public static LocString NAME = "Engie's Tune-Up";

				// Token: 0x0400D665 RID: 54885
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"A skilled Duplicant has improved this generator's ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" output efficiency\n\nApplying this effect consumed one ",
					UI.PRE_KEYWORD,
					ITEMS.INDUSTRIAL_PRODUCTS.POWER_STATION_TOOLS.NAME,
					UI.PST_KEYWORD
				});
			}

			// Token: 0x02003678 RID: 13944
			public class FARMTINKER
			{
				// Token: 0x0400D666 RID: 54886
				public static LocString NAME = "Farmer's Touch";

				// Token: 0x0400D667 RID: 54887
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"A skilled Duplicant has encouraged this ",
					UI.PRE_KEYWORD,
					"Plant",
					UI.PST_KEYWORD,
					" to grow a little bit faster\n\nApplying this effect consumed one dose of ",
					UI.PRE_KEYWORD,
					ITEMS.INDUSTRIAL_PRODUCTS.FARM_STATION_TOOLS.NAME,
					UI.PST_KEYWORD
				});
			}

			// Token: 0x02003679 RID: 13945
			public class MACHINETINKER
			{
				// Token: 0x0400D668 RID: 54888
				public static LocString NAME = "Engie's Jerry Rig";

				// Token: 0x0400D669 RID: 54889
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"A skilled Duplicant has jerry rigged this ",
					UI.PRE_KEYWORD,
					"Generator",
					UI.PST_KEYWORD,
					" to temporarily run faster"
				});
			}

			// Token: 0x0200367A RID: 13946
			public class SPACETOURIST
			{
				// Token: 0x0400D66A RID: 54890
				public static LocString NAME = "Visited Space";

				// Token: 0x0400D66B RID: 54891
				public static LocString TOOLTIP = "This Duplicant went on a trip to space and saw the wonders of the universe";
			}

			// Token: 0x0200367B RID: 13947
			public class SUDDENMORALEHELPER
			{
				// Token: 0x0400D66C RID: 54892
				public static LocString NAME = "Morale Upgrade Helper";

				// Token: 0x0400D66D RID: 54893
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant will receive a temporary ",
					UI.PRE_KEYWORD,
					"Morale",
					UI.PST_KEYWORD,
					" bonus to buffer the new ",
					UI.PRE_KEYWORD,
					"Morale",
					UI.PST_KEYWORD,
					" system introduction"
				});
			}

			// Token: 0x0200367C RID: 13948
			public class EXPOSEDTOFOODGERMS
			{
				// Token: 0x0400D66E RID: 54894
				public static LocString NAME = "Food Poisoning Exposure";

				// Token: 0x0400D66F RID: 54895
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant was exposed to ",
					DUPLICANTS.DISEASES.FOODPOISONING.NAME,
					" Germs and is at risk of developing the ",
					UI.PRE_KEYWORD,
					"Disease",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x0200367D RID: 13949
			public class EXPOSEDTOSLIMEGERMS
			{
				// Token: 0x0400D670 RID: 54896
				public static LocString NAME = "Slimelung Exposure";

				// Token: 0x0400D671 RID: 54897
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant was exposed to ",
					DUPLICANTS.DISEASES.SLIMELUNG.NAME,
					" and is at risk of developing the ",
					UI.PRE_KEYWORD,
					"Disease",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x0200367E RID: 13950
			public class EXPOSEDTOZOMBIESPORES
			{
				// Token: 0x0400D672 RID: 54898
				public static LocString NAME = "Zombie Spores Exposure";

				// Token: 0x0400D673 RID: 54899
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant was exposed to ",
					DUPLICANTS.DISEASES.ZOMBIESPORES.NAME,
					" and is at risk of developing the ",
					UI.PRE_KEYWORD,
					"Disease",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x0200367F RID: 13951
			public class FEELINGSICKFOODGERMS
			{
				// Token: 0x0400D674 RID: 54900
				public static LocString NAME = "Contracted: Food Poisoning";

				// Token: 0x0400D675 RID: 54901
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant contracted ",
					DUPLICANTS.DISEASES.FOODSICKNESS.NAME,
					" after a recent ",
					UI.PRE_KEYWORD,
					"Germ",
					UI.PST_KEYWORD,
					" exposure and will begin exhibiting symptoms shortly"
				});
			}

			// Token: 0x02003680 RID: 13952
			public class FEELINGSICKSLIMEGERMS
			{
				// Token: 0x0400D676 RID: 54902
				public static LocString NAME = "Contracted: Slimelung";

				// Token: 0x0400D677 RID: 54903
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant contracted ",
					DUPLICANTS.DISEASES.SLIMESICKNESS.NAME,
					" after a recent ",
					UI.PRE_KEYWORD,
					"Germ",
					UI.PST_KEYWORD,
					" exposure and will begin exhibiting symptoms shortly"
				});
			}

			// Token: 0x02003681 RID: 13953
			public class FEELINGSICKZOMBIESPORES
			{
				// Token: 0x0400D678 RID: 54904
				public static LocString NAME = "Contracted: Zombie Spores";

				// Token: 0x0400D679 RID: 54905
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant contracted ",
					DUPLICANTS.DISEASES.ZOMBIESICKNESS.NAME,
					" after a recent ",
					UI.PRE_KEYWORD,
					"Germ",
					UI.PST_KEYWORD,
					" exposure and will begin exhibiting symptoms shortly"
				});
			}

			// Token: 0x02003682 RID: 13954
			public class SMELLEDFLOWERS
			{
				// Token: 0x0400D67A RID: 54906
				public static LocString NAME = "Smelled Flowers";

				// Token: 0x0400D67B RID: 54907
				public static LocString TOOLTIP = "A pleasant " + DUPLICANTS.DISEASES.POLLENGERMS.NAME + " wafted over this Duplicant and brightened their day";
			}

			// Token: 0x02003683 RID: 13955
			public class HISTAMINESUPPRESSION
			{
				// Token: 0x0400D67C RID: 54908
				public static LocString NAME = "Antihistamines";

				// Token: 0x0400D67D RID: 54909
				public static LocString TOOLTIP = "This Duplicant's allergic reactions have been suppressed by medication";
			}

			// Token: 0x02003684 RID: 13956
			public class FOODSICKNESSRECOVERY
			{
				// Token: 0x0400D67E RID: 54910
				public static LocString NAME = "Food Poisoning Antibodies";

				// Token: 0x0400D67F RID: 54911
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant recently recovered from ",
					DUPLICANTS.DISEASES.FOODSICKNESS.NAME,
					" and is temporarily immune to the ",
					UI.PRE_KEYWORD,
					"Disease",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x02003685 RID: 13957
			public class SLIMESICKNESSRECOVERY
			{
				// Token: 0x0400D680 RID: 54912
				public static LocString NAME = "Slimelung Antibodies";

				// Token: 0x0400D681 RID: 54913
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant recently recovered from ",
					DUPLICANTS.DISEASES.SLIMESICKNESS.NAME,
					" and is temporarily immune to the ",
					UI.PRE_KEYWORD,
					"Disease",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x02003686 RID: 13958
			public class ZOMBIESICKNESSRECOVERY
			{
				// Token: 0x0400D682 RID: 54914
				public static LocString NAME = "Zombie Spores Antibodies";

				// Token: 0x0400D683 RID: 54915
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant recently recovered from ",
					DUPLICANTS.DISEASES.ZOMBIESICKNESS.NAME,
					" and is temporarily immune to the ",
					UI.PRE_KEYWORD,
					"Disease",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x02003687 RID: 13959
			public class MESSTABLESALT
			{
				// Token: 0x0400D684 RID: 54916
				public static LocString NAME = "Salted Food";

				// Token: 0x0400D685 RID: 54917
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant had the luxury of using ",
					UI.PRE_KEYWORD,
					ITEMS.INDUSTRIAL_PRODUCTS.TABLE_SALT.NAME,
					UI.PST_KEYWORD,
					" with their last meal at a ",
					BUILDINGS.PREFABS.DININGTABLE.NAME
				});
			}

			// Token: 0x02003688 RID: 13960
			public class RADIATIONEXPOSUREMINOR
			{
				// Token: 0x0400D686 RID: 54918
				public static LocString NAME = "Minor Radiation Sickness";

				// Token: 0x0400D687 RID: 54919
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"A bit of ",
					UI.PRE_KEYWORD,
					"Radiation",
					UI.PST_KEYWORD,
					" exposure has made this Duplicant feel sluggish"
				});
			}

			// Token: 0x02003689 RID: 13961
			public class RADIATIONEXPOSUREMAJOR
			{
				// Token: 0x0400D688 RID: 54920
				public static LocString NAME = "Major Radiation Sickness";

				// Token: 0x0400D689 RID: 54921
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Significant ",
					UI.PRE_KEYWORD,
					"Radiation",
					UI.PST_KEYWORD,
					" exposure has left this Duplicant totally exhausted"
				});
			}

			// Token: 0x0200368A RID: 13962
			public class RADIATIONEXPOSUREEXTREME
			{
				// Token: 0x0400D68A RID: 54922
				public static LocString NAME = "Extreme Radiation Sickness";

				// Token: 0x0400D68B RID: 54923
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Dangerously high ",
					UI.PRE_KEYWORD,
					"Radiation",
					UI.PST_KEYWORD,
					" exposure is making this Duplicant wish they'd never been printed"
				});
			}

			// Token: 0x0200368B RID: 13963
			public class RADIATIONEXPOSUREDEADLY
			{
				// Token: 0x0400D68C RID: 54924
				public static LocString NAME = "Deadly Radiation Sickness";

				// Token: 0x0400D68D RID: 54925
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Extreme ",
					UI.PRE_KEYWORD,
					"Radiation",
					UI.PST_KEYWORD,
					" exposure has incapacitated this Duplicant"
				});
			}

			// Token: 0x0200368C RID: 13964
			public class CHARGING
			{
				// Token: 0x0400D68E RID: 54926
				public static LocString NAME = "Charging";

				// Token: 0x0400D68F RID: 54927
				public static LocString TOOLTIP = "This lil bot is charging its internal battery";
			}

			// Token: 0x0200368D RID: 13965
			public class BOTSWEEPING
			{
				// Token: 0x0400D690 RID: 54928
				public static LocString NAME = "Sweeping";

				// Token: 0x0400D691 RID: 54929
				public static LocString TOOLTIP = "This lil bot is picking up debris from the floor";
			}

			// Token: 0x0200368E RID: 13966
			public class BOTMOPPING
			{
				// Token: 0x0400D692 RID: 54930
				public static LocString NAME = "Mopping";

				// Token: 0x0400D693 RID: 54931
				public static LocString TOOLTIP = "This lil bot is clearing liquids from the ground";
			}

			// Token: 0x0200368F RID: 13967
			public class SCOUTBOTCHARGING
			{
				// Token: 0x0400D694 RID: 54932
				public static LocString NAME = "Charging";

				// Token: 0x0400D695 RID: 54933
				public static LocString TOOLTIP = ROBOTS.MODELS.SCOUT.NAME + " is happily charging inside " + BUILDINGS.PREFABS.SCOUTMODULE.NAME;
			}

			// Token: 0x02003690 RID: 13968
			public class CRYOFRIEND
			{
				// Token: 0x0400D696 RID: 54934
				public static LocString NAME = "Motivated By Friend";

				// Token: 0x0400D697 RID: 54935
				public static LocString TOOLTIP = "This Duplicant feels motivated after meeting a long lost friend";
			}

			// Token: 0x02003691 RID: 13969
			public class BONUSDREAM1
			{
				// Token: 0x0400D698 RID: 54936
				public static LocString NAME = "Good Dream";

				// Token: 0x0400D699 RID: 54937
				public static LocString TOOLTIP = "This Duplicant had a good dream and is feeling psyched!";
			}

			// Token: 0x02003692 RID: 13970
			public class BONUSDREAM2
			{
				// Token: 0x0400D69A RID: 54938
				public static LocString NAME = "Really Good Dream";

				// Token: 0x0400D69B RID: 54939
				public static LocString TOOLTIP = "This Duplicant had a really good dream and is full of possibilities!";
			}

			// Token: 0x02003693 RID: 13971
			public class BONUSDREAM3
			{
				// Token: 0x0400D69C RID: 54940
				public static LocString NAME = "Great Dream";

				// Token: 0x0400D69D RID: 54941
				public static LocString TOOLTIP = "This Duplicant had a great dream last night and periodically remembers another great moment they previously forgot";
			}

			// Token: 0x02003694 RID: 13972
			public class BONUSDREAM4
			{
				// Token: 0x0400D69E RID: 54942
				public static LocString NAME = "Dream Inspired";

				// Token: 0x0400D69F RID: 54943
				public static LocString TOOLTIP = "This Duplicant is inspired from all the unforgettable dreams they had";
			}

			// Token: 0x02003695 RID: 13973
			public class BONUSRESEARCH
			{
				// Token: 0x0400D6A0 RID: 54944
				public static LocString NAME = "Inspired Learner";

				// Token: 0x0400D6A1 RID: 54945
				public static LocString TOOLTIP = "This Duplicant is looking forward to some learning";
			}

			// Token: 0x02003696 RID: 13974
			public class BONUSTOILET1
			{
				// Token: 0x0400D6A2 RID: 54946
				public static LocString NAME = "Small Comforts";

				// Token: 0x0400D6A3 RID: 54947
				public static LocString TOOLTIP = "This Duplicant visited the {building} and appreciated the small comforts";
			}

			// Token: 0x02003697 RID: 13975
			public class BONUSTOILET2
			{
				// Token: 0x0400D6A4 RID: 54948
				public static LocString NAME = "Greater Comforts";

				// Token: 0x0400D6A5 RID: 54949
				public static LocString TOOLTIP = "This Duplicant used a " + BUILDINGS.PREFABS.OUTHOUSE.NAME + "and liked how comfortable it felt";
			}

			// Token: 0x02003698 RID: 13976
			public class BONUSTOILET3
			{
				// Token: 0x0400D6A6 RID: 54950
				public static LocString NAME = "Small Luxury";

				// Token: 0x0400D6A7 RID: 54951
				public static LocString TOOLTIP = "This Duplicant visited a " + ROOMS.TYPES.LATRINE.NAME + " and feels they could get used to this luxury";
			}

			// Token: 0x02003699 RID: 13977
			public class BONUSTOILET4
			{
				// Token: 0x0400D6A8 RID: 54952
				public static LocString NAME = "Luxurious";

				// Token: 0x0400D6A9 RID: 54953
				public static LocString TOOLTIP = "This Duplicant feels endless luxury from the " + ROOMS.TYPES.PRIVATE_BATHROOM.NAME;
			}

			// Token: 0x0200369A RID: 13978
			public class BONUSDIGGING1
			{
				// Token: 0x0400D6AA RID: 54954
				public static LocString NAME = "Hot Diggity!";

				// Token: 0x0400D6AB RID: 54955
				public static LocString TOOLTIP = "This Duplicant did a lot of excavating and is really digging digging";
			}

			// Token: 0x0200369B RID: 13979
			public class BONUSSTORAGE
			{
				// Token: 0x0400D6AC RID: 54956
				public static LocString NAME = "Something in Store";

				// Token: 0x0400D6AD RID: 54957
				public static LocString TOOLTIP = "This Duplicant stored something in a " + BUILDINGS.PREFABS.STORAGELOCKER.NAME + " and is feeling organized";
			}

			// Token: 0x0200369C RID: 13980
			public class BONUSBUILDER
			{
				// Token: 0x0400D6AE RID: 54958
				public static LocString NAME = "Accomplished Builder";

				// Token: 0x0400D6AF RID: 54959
				public static LocString TOOLTIP = "This Duplicant has built many buildings and has a sense of accomplishment!";
			}

			// Token: 0x0200369D RID: 13981
			public class BONUSOXYGEN
			{
				// Token: 0x0400D6B0 RID: 54960
				public static LocString NAME = "Fresh Air";

				// Token: 0x0400D6B1 RID: 54961
				public static LocString TOOLTIP = "This Duplicant breathed in some fresh air and is feeling refreshed";
			}

			// Token: 0x0200369E RID: 13982
			public class BONUSGENERATOR
			{
				// Token: 0x0400D6B2 RID: 54962
				public static LocString NAME = "Exercised";

				// Token: 0x0400D6B3 RID: 54963
				public static LocString TOOLTIP = "This Duplicant ran in a Generator and has benefited from the exercise";
			}

			// Token: 0x0200369F RID: 13983
			public class BONUSDOOR
			{
				// Token: 0x0400D6B4 RID: 54964
				public static LocString NAME = "Open and Shut";

				// Token: 0x0400D6B5 RID: 54965
				public static LocString TOOLTIP = "This Duplicant closed a door and appreciates the privacy";
			}

			// Token: 0x020036A0 RID: 13984
			public class BONUSHITTHEBOOKS
			{
				// Token: 0x0400D6B6 RID: 54966
				public static LocString NAME = "Hit the Books";

				// Token: 0x0400D6B7 RID: 54967
				public static LocString TOOLTIP = "This Duplicant did some research and is feeling smarter";
			}

			// Token: 0x020036A1 RID: 13985
			public class BONUSLITWORKSPACE
			{
				// Token: 0x0400D6B8 RID: 54968
				public static LocString NAME = "Lit";

				// Token: 0x0400D6B9 RID: 54969
				public static LocString TOOLTIP = "This Duplicant was in a well-lit environment and is feeling lit";
			}

			// Token: 0x020036A2 RID: 13986
			public class BONUSTALKER
			{
				// Token: 0x0400D6BA RID: 54970
				public static LocString NAME = "Talker";

				// Token: 0x0400D6BB RID: 54971
				public static LocString TOOLTIP = "This Duplicant engaged in small talk with a coworker and is feeling connected";
			}

			// Token: 0x020036A3 RID: 13987
			public class THRIVER
			{
				// Token: 0x0400D6BC RID: 54972
				public static LocString NAME = "Clutchy";

				// Token: 0x0400D6BD RID: 54973
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is ",
					UI.PRE_KEYWORD,
					"Stressed",
					UI.PST_KEYWORD,
					" and has kicked into hyperdrive"
				});
			}

			// Token: 0x020036A4 RID: 13988
			public class LONER
			{
				// Token: 0x0400D6BE RID: 54974
				public static LocString NAME = "Alone";

				// Token: 0x0400D6BF RID: 54975
				public static LocString TOOLTIP = "This Duplicant is feeling more focused now that they're alone";
			}

			// Token: 0x020036A5 RID: 13989
			public class STARRYEYED
			{
				// Token: 0x0400D6C0 RID: 54976
				public static LocString NAME = "Starry Eyed";

				// Token: 0x0400D6C1 RID: 54977
				public static LocString TOOLTIP = "This Duplicant loves being in space!";
			}

			// Token: 0x020036A6 RID: 13990
			public class WAILEDAT
			{
				// Token: 0x0400D6C2 RID: 54978
				public static LocString NAME = "Disturbed by Wailing";

				// Token: 0x0400D6C3 RID: 54979
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is feeling ",
					UI.PRE_KEYWORD,
					"Stressed",
					UI.PST_KEYWORD,
					" by someone's Banshee Wail"
				});
			}

			// Token: 0x020036A7 RID: 13991
			public class BIONICPILOTINGBOOST
			{
				// Token: 0x0400D6C4 RID: 54980
				public static LocString NAME = "Piloting Boost";

				// Token: 0x0400D6C5 RID: 54981
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant has increased rocket piloting skills thanks to the ",
					UI.PRE_KEYWORD,
					"Rocketry Booster",
					UI.PST_KEYWORD,
					" they have installed"
				});
			}

			// Token: 0x020036A8 RID: 13992
			public class BIONICCONSTRUCTIONBOOST
			{
				// Token: 0x0400D6C6 RID: 54982
				public static LocString NAME = "Construction Boost";

				// Token: 0x0400D6C7 RID: 54983
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant has increased construction skills thanks to the ",
					UI.PRE_KEYWORD,
					"Building Booster",
					UI.PST_KEYWORD,
					" they have installed"
				});
			}

			// Token: 0x020036A9 RID: 13993
			public class BIONICEXCAVATIONBOOST
			{
				// Token: 0x0400D6C8 RID: 54984
				public static LocString NAME = "Excavation Boost";

				// Token: 0x0400D6C9 RID: 54985
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant has increased excavation skills thanks to the ",
					UI.PRE_KEYWORD,
					"Digging Booster",
					UI.PST_KEYWORD,
					" they have installed"
				});
			}

			// Token: 0x020036AA RID: 13994
			public class BIONICMACHINERYBOOST
			{
				// Token: 0x0400D6CA RID: 54986
				public static LocString NAME = "Machinery Boost";

				// Token: 0x0400D6CB RID: 54987
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant has increased machine operating skills thanks to the ",
					UI.PRE_KEYWORD,
					"Operating Booster",
					UI.PST_KEYWORD,
					" they have installed"
				});
			}

			// Token: 0x020036AB RID: 13995
			public class BIONICATHLETICSBOOST
			{
				// Token: 0x0400D6CC RID: 54988
				public static LocString NAME = "Athletics Boost";

				// Token: 0x0400D6CD RID: 54989
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant has extra zip in their step thanks to the ",
					UI.PRE_KEYWORD,
					"Athletics Booster",
					UI.PST_KEYWORD,
					" they have installed"
				});
			}

			// Token: 0x020036AC RID: 13996
			public class BIONICSCIENCEBOOST
			{
				// Token: 0x0400D6CE RID: 54990
				public static LocString NAME = "Science Boost";

				// Token: 0x0400D6CF RID: 54991
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant has increased scientific research skills thanks to the ",
					UI.PRE_KEYWORD,
					"Researching Booster",
					UI.PST_KEYWORD,
					" they have installed"
				});
			}

			// Token: 0x020036AD RID: 13997
			public class BIONICCOOKINGBOOST
			{
				// Token: 0x0400D6D0 RID: 54992
				public static LocString NAME = "Cuisine Boost";

				// Token: 0x0400D6D1 RID: 54993
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant has increased culinary skills thanks to the ",
					UI.PRE_KEYWORD,
					"Cooking Booster",
					UI.PST_KEYWORD,
					" they have installed"
				});
			}

			// Token: 0x020036AE RID: 13998
			public class BIONICMEDICINEBOOST
			{
				// Token: 0x0400D6D2 RID: 54994
				public static LocString NAME = "Medicine Boost";

				// Token: 0x0400D6D3 RID: 54995
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant has increased medical skills thanks to the ",
					UI.PRE_KEYWORD,
					"Doctoring Booster",
					UI.PST_KEYWORD,
					" they have installed"
				});
			}

			// Token: 0x020036AF RID: 13999
			public class BIONICSTRENGTHBOOST
			{
				// Token: 0x0400D6D4 RID: 54996
				public static LocString NAME = "Strength Boost";

				// Token: 0x0400D6D5 RID: 54997
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is extra strong thanks to the ",
					UI.PRE_KEYWORD,
					"Strength Booster",
					UI.PST_KEYWORD,
					" they have installed"
				});
			}

			// Token: 0x020036B0 RID: 14000
			public class BIONICCREATIVITYBOOST
			{
				// Token: 0x0400D6D6 RID: 54998
				public static LocString NAME = "Creativity Boost";

				// Token: 0x0400D6D7 RID: 54999
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant has increased decorating skills thanks to the ",
					UI.PRE_KEYWORD,
					"Creativity Booster",
					UI.PST_KEYWORD,
					" they have installed"
				});
			}

			// Token: 0x020036B1 RID: 14001
			public class BIONICAGRICULTUREBOOST
			{
				// Token: 0x0400D6D8 RID: 55000
				public static LocString NAME = "Agriculture Boost";

				// Token: 0x0400D6D9 RID: 55001
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant has increased agricultural skills thanks to the ",
					UI.PRE_KEYWORD,
					"Farming Booster",
					UI.PST_KEYWORD,
					" they have installed"
				});
			}

			// Token: 0x020036B2 RID: 14002
			public class BIONICHUSBANDRYBOOST
			{
				// Token: 0x0400D6DA RID: 55002
				public static LocString NAME = "Husbandry Boost";

				// Token: 0x0400D6DB RID: 55003
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant has increased critter husbandry skills thanks to the ",
					UI.PRE_KEYWORD,
					"Ranching Booster",
					UI.PST_KEYWORD,
					" they have installed"
				});
			}
		}

		// Token: 0x020036B3 RID: 14003
		public class CONGENITALTRAITS
		{
			// Token: 0x020036B4 RID: 14004
			public class NONE
			{
				// Token: 0x0400D6DC RID: 55004
				public static LocString NAME = "None";

				// Token: 0x0400D6DD RID: 55005
				public static LocString DESC = "This Duplicant seems pretty average overall";
			}

			// Token: 0x020036B5 RID: 14005
			public class JOSHUA
			{
				// Token: 0x0400D6DE RID: 55006
				public static LocString NAME = "Cheery Disposition";

				// Token: 0x0400D6DF RID: 55007
				public static LocString DESC = "This Duplicant brightens others' days wherever he goes";
			}

			// Token: 0x020036B6 RID: 14006
			public class ELLIE
			{
				// Token: 0x0400D6E0 RID: 55008
				public static LocString NAME = "Fastidious";

				// Token: 0x0400D6E1 RID: 55009
				public static LocString DESC = "This Duplicant needs things done in a very particular way";
			}

			// Token: 0x020036B7 RID: 14007
			public class LIAM
			{
				// Token: 0x0400D6E2 RID: 55010
				public static LocString NAME = "Germaphobe";

				// Token: 0x0400D6E3 RID: 55011
				public static LocString DESC = "This Duplicant has an all-consuming fear of bacteria";
			}

			// Token: 0x020036B8 RID: 14008
			public class BANHI
			{
				// Token: 0x0400D6E4 RID: 55012
				public static LocString NAME = "";

				// Token: 0x0400D6E5 RID: 55013
				public static LocString DESC = "";
			}

			// Token: 0x020036B9 RID: 14009
			public class STINKY
			{
				// Token: 0x0400D6E6 RID: 55014
				public static LocString NAME = "Stinkiness";

				// Token: 0x0400D6E7 RID: 55015
				public static LocString DESC = "This Duplicant is genetically cursed by a pungent bodily odor";
			}
		}

		// Token: 0x020036BA RID: 14010
		public class TRAITS
		{
			// Token: 0x0400D6E8 RID: 55016
			public static LocString TRAIT_DESCRIPTION_LIST_ENTRY = "\n• ";

			// Token: 0x0400D6E9 RID: 55017
			public static LocString ATTRIBUTE_MODIFIERS = "{0}: {1}";

			// Token: 0x0400D6EA RID: 55018
			public static LocString CANNOT_DO_TASK = "Cannot do <b>{0} Errands</b>";

			// Token: 0x0400D6EB RID: 55019
			public static LocString CANNOT_DO_TASK_TOOLTIP = "{0}: {1}";

			// Token: 0x0400D6EC RID: 55020
			public static LocString REFUSES_TO_DO_TASK = "Cannot do <b>{0} Errands</b>";

			// Token: 0x0400D6ED RID: 55021
			public static LocString IGNORED_EFFECTS = "Immune to <b>{0}</b>";

			// Token: 0x0400D6EE RID: 55022
			public static LocString IGNORED_EFFECTS_TOOLTIP = "{0}: {1}";

			// Token: 0x0400D6EF RID: 55023
			public static LocString GRANTED_SKILL_SHARED_NAME = "Skilled: ";

			// Token: 0x0400D6F0 RID: 55024
			public static LocString GRANTED_SKILL_SHARED_DESC = string.Concat(new string[]
			{
				"This Duplicant begins with a pre-learned ",
				UI.FormatAsKeyWord("Skill"),
				", but does not have increased ",
				UI.FormatAsKeyWord(DUPLICANTS.NEEDS.QUALITYOFLIFE.NAME),
				".\n\n{0}\n{1}"
			});

			// Token: 0x0400D6F1 RID: 55025
			public static LocString GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP = "This Duplicant receives a free " + UI.FormatAsKeyWord("Skill") + " without the drawback of increased " + UI.FormatAsKeyWord(DUPLICANTS.NEEDS.QUALITYOFLIFE.NAME);

			// Token: 0x020036BB RID: 14011
			public class CHATTY
			{
				// Token: 0x0400D6F2 RID: 55026
				public static LocString NAME = "Charismatic";

				// Token: 0x0400D6F3 RID: 55027
				public static LocString DESC = string.Concat(new string[]
				{
					"This Duplicant's so charming, chatting with them is sometimes enough to trigger an ",
					UI.PRE_KEYWORD,
					"Overjoyed",
					UI.PST_KEYWORD,
					" response"
				});
			}

			// Token: 0x020036BC RID: 14012
			public class NEEDS
			{
				// Token: 0x020036BD RID: 14013
				public class CLAUSTROPHOBIC
				{
					// Token: 0x0400D6F4 RID: 55028
					public static LocString NAME = "Claustrophobic";

					// Token: 0x0400D6F5 RID: 55029
					public static LocString DESC = "This Duplicant feels suffocated in spaces fewer than four tiles high or three tiles wide";
				}

				// Token: 0x020036BE RID: 14014
				public class FASHIONABLE
				{
					// Token: 0x0400D6F6 RID: 55030
					public static LocString NAME = "Fashionista";

					// Token: 0x0400D6F7 RID: 55031
					public static LocString DESC = "This Duplicant dies a bit inside when forced to wear unstylish clothing";
				}

				// Token: 0x020036BF RID: 14015
				public class CLIMACOPHOBIC
				{
					// Token: 0x0400D6F8 RID: 55032
					public static LocString NAME = "Vertigo Prone";

					// Token: 0x0400D6F9 RID: 55033
					public static LocString DESC = "Climbing ladders more than four tiles tall makes this Duplicant's stomach do flips";
				}

				// Token: 0x020036C0 RID: 14016
				public class SOLITARYSLEEPER
				{
					// Token: 0x0400D6FA RID: 55034
					public static LocString NAME = "Solitary Sleeper";

					// Token: 0x0400D6FB RID: 55035
					public static LocString DESC = "This Duplicant prefers to sleep alone";
				}

				// Token: 0x020036C1 RID: 14017
				public class PREFERSWARMER
				{
					// Token: 0x0400D6FC RID: 55036
					public static LocString NAME = "Skinny";

					// Token: 0x0400D6FD RID: 55037
					public static LocString DESC = string.Concat(new string[]
					{
						"This Duplicant doesn't have much ",
						UI.PRE_KEYWORD,
						"Insulation",
						UI.PST_KEYWORD,
						", so they are more ",
						UI.PRE_KEYWORD,
						"Temperature",
						UI.PST_KEYWORD,
						" sensitive than others"
					});
				}

				// Token: 0x020036C2 RID: 14018
				public class PREFERSCOOLER
				{
					// Token: 0x0400D6FE RID: 55038
					public static LocString NAME = "Pudgy";

					// Token: 0x0400D6FF RID: 55039
					public static LocString DESC = string.Concat(new string[]
					{
						"This Duplicant has some extra ",
						UI.PRE_KEYWORD,
						"Insulation",
						UI.PST_KEYWORD,
						", so the room ",
						UI.PRE_KEYWORD,
						"Temperature",
						UI.PST_KEYWORD,
						" affects them a little less"
					});
				}

				// Token: 0x020036C3 RID: 14019
				public class SENSITIVEFEET
				{
					// Token: 0x0400D700 RID: 55040
					public static LocString NAME = "Delicate Feetsies";

					// Token: 0x0400D701 RID: 55041
					public static LocString DESC = "This Duplicant is a sensitive sole and would rather walk on tile than raw bedrock";
				}

				// Token: 0x020036C4 RID: 14020
				public class WORKAHOLIC
				{
					// Token: 0x0400D702 RID: 55042
					public static LocString NAME = "Workaholic";

					// Token: 0x0400D703 RID: 55043
					public static LocString DESC = "This Duplicant gets antsy when left idle";
				}
			}

			// Token: 0x020036C5 RID: 14021
			public class ANCIENTKNOWLEDGE
			{
				// Token: 0x0400D704 RID: 55044
				public static LocString NAME = "Ancient Knowledge";

				// Token: 0x0400D705 RID: 55045
				public static LocString DESC = "This Duplicant has knowledge from the before times\n• Starts with 3 skill points";
			}

			// Token: 0x020036C6 RID: 14022
			public class CANTRESEARCH
			{
				// Token: 0x0400D706 RID: 55046
				public static LocString NAME = "Yokel";

				// Token: 0x0400D707 RID: 55047
				public static LocString DESC = "This Duplicant isn't the brightest star in the solar system";
			}

			// Token: 0x020036C7 RID: 14023
			public class CANTBUILD
			{
				// Token: 0x0400D708 RID: 55048
				public static LocString NAME = "Unconstructive";

				// Token: 0x0400D709 RID: 55049
				public static LocString DESC = "This Duplicant is incapable of building even the most basic of structures";
			}

			// Token: 0x020036C8 RID: 14024
			public class CANTCOOK
			{
				// Token: 0x0400D70A RID: 55050
				public static LocString NAME = "Gastrophobia";

				// Token: 0x0400D70B RID: 55051
				public static LocString DESC = "This Duplicant has a deep-seated distrust of the culinary arts";
			}

			// Token: 0x020036C9 RID: 14025
			public class CANTDIG
			{
				// Token: 0x0400D70C RID: 55052
				public static LocString NAME = "Trypophobia";

				// Token: 0x0400D70D RID: 55053
				public static LocString DESC = "This Duplicant's fear of holes makes it impossible for them to dig";
			}

			// Token: 0x020036CA RID: 14026
			public class HEMOPHOBIA
			{
				// Token: 0x0400D70E RID: 55054
				public static LocString NAME = "Squeamish";

				// Token: 0x0400D70F RID: 55055
				public static LocString DESC = "This Duplicant is of delicate disposition and cannot tend to the sick";
			}

			// Token: 0x020036CB RID: 14027
			public class BEDSIDEMANNER
			{
				// Token: 0x0400D710 RID: 55056
				public static LocString NAME = "Caregiver";

				// Token: 0x0400D711 RID: 55057
				public static LocString DESC = "This Duplicant has good bedside manner and a healing touch";
			}

			// Token: 0x020036CC RID: 14028
			public class MOUTHBREATHER
			{
				// Token: 0x0400D712 RID: 55058
				public static LocString NAME = "Mouth Breather";

				// Token: 0x0400D713 RID: 55059
				public static LocString DESC = "This Duplicant sucks up way more than their fair share of " + ELEMENTS.OXYGEN.NAME;
			}

			// Token: 0x020036CD RID: 14029
			public class FUSSY
			{
				// Token: 0x0400D714 RID: 55060
				public static LocString NAME = "Fussy";

				// Token: 0x0400D715 RID: 55061
				public static LocString DESC = "Nothing's ever quite good enough for this Duplicant";
			}

			// Token: 0x020036CE RID: 14030
			public class TWINKLETOES
			{
				// Token: 0x0400D716 RID: 55062
				public static LocString NAME = "Twinkletoes";

				// Token: 0x0400D717 RID: 55063
				public static LocString DESC = "This Duplicant is light as a feather on their feet";
			}

			// Token: 0x020036CF RID: 14031
			public class STRONGARM
			{
				// Token: 0x0400D718 RID: 55064
				public static LocString NAME = "Buff";

				// Token: 0x0400D719 RID: 55065
				public static LocString DESC = "This Duplicant has muscles on their muscles";
			}

			// Token: 0x020036D0 RID: 14032
			public class NOODLEARMS
			{
				// Token: 0x0400D71A RID: 55066
				public static LocString NAME = "Noodle Arms";

				// Token: 0x0400D71B RID: 55067
				public static LocString DESC = "This Duplicant's arms have all the tensile strength of overcooked linguine";
			}

			// Token: 0x020036D1 RID: 14033
			public class AGGRESSIVE
			{
				// Token: 0x0400D71C RID: 55068
				public static LocString NAME = "Destructive";

				// Token: 0x0400D71D RID: 55069
				public static LocString DESC = "This Duplicant handles stress by taking their frustrations out on defenseless machines";

				// Token: 0x0400D71E RID: 55070
				public static LocString NOREPAIR = "• Will not repair buildings while above 60% " + UI.PRE_KEYWORD + "Stress" + UI.PST_KEYWORD;
			}

			// Token: 0x020036D2 RID: 14034
			public class UGLYCRIER
			{
				// Token: 0x0400D71F RID: 55071
				public static LocString NAME = "Ugly Crier";

				// Token: 0x0400D720 RID: 55072
				public static LocString DESC = string.Concat(new string[]
				{
					"If this Duplicant gets too ",
					UI.PRE_KEYWORD,
					"Stressed",
					UI.PST_KEYWORD,
					" it won't be pretty"
				});
			}

			// Token: 0x020036D3 RID: 14035
			public class BINGEEATER
			{
				// Token: 0x0400D721 RID: 55073
				public static LocString NAME = "Binge Eater";

				// Token: 0x0400D722 RID: 55074
				public static LocString DESC = "This Duplicant will dangerously overeat when " + UI.PRE_KEYWORD + "Stressed" + UI.PST_KEYWORD;
			}

			// Token: 0x020036D4 RID: 14036
			public class ANXIOUS
			{
				// Token: 0x0400D723 RID: 55075
				public static LocString NAME = "Anxious";

				// Token: 0x0400D724 RID: 55076
				public static LocString DESC = "This Duplicant collapses when put under too much pressure";
			}

			// Token: 0x020036D5 RID: 14037
			public class STRESSVOMITER
			{
				// Token: 0x0400D725 RID: 55077
				public static LocString NAME = "Vomiter";

				// Token: 0x0400D726 RID: 55078
				public static LocString DESC = "This Duplicant is liable to puke everywhere when " + UI.PRE_KEYWORD + "Stressed" + UI.PST_KEYWORD;
			}

			// Token: 0x020036D6 RID: 14038
			public class STRESSSHOCKER
			{
				// Token: 0x0400D727 RID: 55079
				public static LocString NAME = "Stunner";

				// Token: 0x0400D728 RID: 55080
				public static LocString DESC = "This Duplicant emits electrical shocks when " + UI.PRE_KEYWORD + "Stressed" + UI.PST_KEYWORD;

				// Token: 0x0400D729 RID: 55081
				public static LocString DRAIN_ATTRIBUTE = "Stress Zapping";
			}

			// Token: 0x020036D7 RID: 14039
			public class BANSHEE
			{
				// Token: 0x0400D72A RID: 55082
				public static LocString NAME = "Banshee";

				// Token: 0x0400D72B RID: 55083
				public static LocString DESC = "This Duplicant wails uncontrollably when " + UI.PRE_KEYWORD + "Stressed" + UI.PST_KEYWORD;
			}

			// Token: 0x020036D8 RID: 14040
			public class BALLOONARTIST
			{
				// Token: 0x0400D72C RID: 55084
				public static LocString NAME = "Balloon Artist";

				// Token: 0x0400D72D RID: 55085
				public static LocString DESC = "This Duplicant hands out balloons when they are " + UI.PRE_KEYWORD + "Overjoyed" + UI.PST_KEYWORD;
			}

			// Token: 0x020036D9 RID: 14041
			public class SPARKLESTREAKER
			{
				// Token: 0x0400D72E RID: 55086
				public static LocString NAME = "Sparkle Streaker";

				// Token: 0x0400D72F RID: 55087
				public static LocString DESC = "This Duplicant leaves a trail of happy sparkles when they are " + UI.PRE_KEYWORD + "Overjoyed" + UI.PST_KEYWORD;
			}

			// Token: 0x020036DA RID: 14042
			public class STICKERBOMBER
			{
				// Token: 0x0400D730 RID: 55088
				public static LocString NAME = "Sticker Bomber";

				// Token: 0x0400D731 RID: 55089
				public static LocString DESC = "This Duplicant will spontaneously redecorate a room when they are " + UI.PRE_KEYWORD + "Overjoyed" + UI.PST_KEYWORD;
			}

			// Token: 0x020036DB RID: 14043
			public class SUPERPRODUCTIVE
			{
				// Token: 0x0400D732 RID: 55090
				public static LocString NAME = "Super Productive";

				// Token: 0x0400D733 RID: 55091
				public static LocString DESC = "This Duplicant is super productive when they are " + UI.PRE_KEYWORD + "Overjoyed" + UI.PST_KEYWORD;
			}

			// Token: 0x020036DC RID: 14044
			public class HAPPYSINGER
			{
				// Token: 0x0400D734 RID: 55092
				public static LocString NAME = "Yodeler";

				// Token: 0x0400D735 RID: 55093
				public static LocString DESC = "This Duplicant belts out catchy tunes when they are " + UI.PRE_KEYWORD + "Overjoyed" + UI.PST_KEYWORD;
			}

			// Token: 0x020036DD RID: 14045
			public class DATARAINER
			{
				// Token: 0x0400D736 RID: 55094
				public static LocString NAME = "Rainmaker";

				// Token: 0x0400D737 RID: 55095
				public static LocString DESC = "This Duplicant distributes microchips when they are " + UI.PRE_KEYWORD + "Overjoyed" + UI.PST_KEYWORD;
			}

			// Token: 0x020036DE RID: 14046
			public class ROBODANCER
			{
				// Token: 0x0400D738 RID: 55096
				public static LocString NAME = "Flash Mobber";

				// Token: 0x0400D739 RID: 55097
				public static LocString DESC = "This Duplicant breaks into dance when they are " + UI.PRE_KEYWORD + "Overjoyed" + UI.PST_KEYWORD;
			}

			// Token: 0x020036DF RID: 14047
			public class IRONGUT
			{
				// Token: 0x0400D73A RID: 55098
				public static LocString NAME = "Iron Gut";

				// Token: 0x0400D73B RID: 55099
				public static LocString DESC = "This Duplicant can eat just about anything without getting sick";

				// Token: 0x0400D73C RID: 55100
				public static LocString SHORT_DESC = "Immune to <b>" + DUPLICANTS.DISEASES.FOODSICKNESS.NAME + "</b>";

				// Token: 0x0400D73D RID: 55101
				public static LocString SHORT_DESC_TOOLTIP = "Eating food contaminated with " + DUPLICANTS.DISEASES.FOODSICKNESS.NAME + " Germs will not affect this Duplicant";
			}

			// Token: 0x020036E0 RID: 14048
			public class STRONGIMMUNESYSTEM
			{
				// Token: 0x0400D73E RID: 55102
				public static LocString NAME = "Germ Resistant";

				// Token: 0x0400D73F RID: 55103
				public static LocString DESC = "This Duplicant's immune system bounces back faster than most";
			}

			// Token: 0x020036E1 RID: 14049
			public class SCAREDYCAT
			{
				// Token: 0x0400D740 RID: 55104
				public static LocString NAME = "Pacifist";

				// Token: 0x0400D741 RID: 55105
				public static LocString DESC = "This Duplicant abhors violence";
			}

			// Token: 0x020036E2 RID: 14050
			public class ALLERGIES
			{
				// Token: 0x0400D742 RID: 55106
				public static LocString NAME = "Allergies";

				// Token: 0x0400D743 RID: 55107
				public static LocString DESC = "This Duplicant will sneeze uncontrollably when exposed to the pollen present in " + DUPLICANTS.DISEASES.POLLENGERMS.NAME;

				// Token: 0x0400D744 RID: 55108
				public static LocString SHORT_DESC = "Allergic reaction to <b>" + DUPLICANTS.DISEASES.POLLENGERMS.NAME + "</b>";

				// Token: 0x0400D745 RID: 55109
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.DISEASES.ALLERGIES.DESCRIPTIVE_SYMPTOMS;
			}

			// Token: 0x020036E3 RID: 14051
			public class WEAKIMMUNESYSTEM
			{
				// Token: 0x0400D746 RID: 55110
				public static LocString NAME = "Biohazardous";

				// Token: 0x0400D747 RID: 55111
				public static LocString DESC = "All the vitamin C in space couldn't stop this Duplicant from getting sick";
			}

			// Token: 0x020036E4 RID: 14052
			public class IRRITABLEBOWEL
			{
				// Token: 0x0400D748 RID: 55112
				public static LocString NAME = "Irritable Bowel";

				// Token: 0x0400D749 RID: 55113
				public static LocString DESC = "This Duplicant needs a little extra time to \"do their business\"";
			}

			// Token: 0x020036E5 RID: 14053
			public class CALORIEBURNER
			{
				// Token: 0x0400D74A RID: 55114
				public static LocString NAME = "Bottomless Stomach";

				// Token: 0x0400D74B RID: 55115
				public static LocString DESC = "This Duplicant might actually be several black holes in a trench coat";
			}

			// Token: 0x020036E6 RID: 14054
			public class SMALLBLADDER
			{
				// Token: 0x0400D74C RID: 55116
				public static LocString NAME = "Small Bladder";

				// Token: 0x0400D74D RID: 55117
				public static LocString DESC = string.Concat(new string[]
				{
					"This Duplicant has a tiny, pea-sized ",
					UI.PRE_KEYWORD,
					"Bladder",
					UI.PST_KEYWORD,
					". Adorable!"
				});
			}

			// Token: 0x020036E7 RID: 14055
			public class ANEMIC
			{
				// Token: 0x0400D74E RID: 55118
				public static LocString NAME = "Anemic";

				// Token: 0x0400D74F RID: 55119
				public static LocString DESC = "This Duplicant has trouble keeping up with the others";
			}

			// Token: 0x020036E8 RID: 14056
			public class GREASEMONKEY
			{
				// Token: 0x0400D750 RID: 55120
				public static LocString NAME = "Grease Monkey";

				// Token: 0x0400D751 RID: 55121
				public static LocString DESC = "This Duplicant likes to throw a wrench into the colony's plans... in a good way";
			}

			// Token: 0x020036E9 RID: 14057
			public class MOLEHANDS
			{
				// Token: 0x0400D752 RID: 55122
				public static LocString NAME = "Mole Hands";

				// Token: 0x0400D753 RID: 55123
				public static LocString DESC = "They're great for tunneling, but finding good gloves is a nightmare";
			}

			// Token: 0x020036EA RID: 14058
			public class FASTLEARNER
			{
				// Token: 0x0400D754 RID: 55124
				public static LocString NAME = "Quick Learner";

				// Token: 0x0400D755 RID: 55125
				public static LocString DESC = "This Duplicant's sharp as a tack and learns new skills with amazing speed";
			}

			// Token: 0x020036EB RID: 14059
			public class SLOWLEARNER
			{
				// Token: 0x0400D756 RID: 55126
				public static LocString NAME = "Slow Learner";

				// Token: 0x0400D757 RID: 55127
				public static LocString DESC = "This Duplicant's a little slow on the uptake, but gosh do they try";
			}

			// Token: 0x020036EC RID: 14060
			public class DIVERSLUNG
			{
				// Token: 0x0400D758 RID: 55128
				public static LocString NAME = "Diver's Lungs";

				// Token: 0x0400D759 RID: 55129
				public static LocString DESC = "This Duplicant could have been a talented opera singer in another life";
			}

			// Token: 0x020036ED RID: 14061
			public class FLATULENCE
			{
				// Token: 0x0400D75A RID: 55130
				public static LocString NAME = "Flatulent";

				// Token: 0x0400D75B RID: 55131
				public static LocString DESC = "Some Duplicants are just full of it";

				// Token: 0x0400D75C RID: 55132
				public static LocString SHORT_DESC = "Farts frequently";

				// Token: 0x0400D75D RID: 55133
				public static LocString SHORT_DESC_TOOLTIP = "This Duplicant will periodically \"output\" " + ELEMENTS.METHANE.NAME;
			}

			// Token: 0x020036EE RID: 14062
			public class SNORER
			{
				// Token: 0x0400D75E RID: 55134
				public static LocString NAME = "Loud Sleeper";

				// Token: 0x0400D75F RID: 55135
				public static LocString DESC = "In space, everyone can hear you snore";

				// Token: 0x0400D760 RID: 55136
				public static LocString SHORT_DESC = "Snores loudly";

				// Token: 0x0400D761 RID: 55137
				public static LocString SHORT_DESC_TOOLTIP = "This Duplicant's snoring will rudely awake nearby friends";
			}

			// Token: 0x020036EF RID: 14063
			public class NARCOLEPSY
			{
				// Token: 0x0400D762 RID: 55138
				public static LocString NAME = "Narcoleptic";

				// Token: 0x0400D763 RID: 55139
				public static LocString DESC = "This Duplicant can and will fall asleep anytime, anyplace";

				// Token: 0x0400D764 RID: 55140
				public static LocString SHORT_DESC = "Falls asleep periodically";

				// Token: 0x0400D765 RID: 55141
				public static LocString SHORT_DESC_TOOLTIP = "This Duplicant's work will be periodically interrupted by naps";
			}

			// Token: 0x020036F0 RID: 14064
			public class INTERIORDECORATOR
			{
				// Token: 0x0400D766 RID: 55142
				public static LocString NAME = "Interior Decorator";

				// Token: 0x0400D767 RID: 55143
				public static LocString DESC = "\"Move it a little to the left...\"";
			}

			// Token: 0x020036F1 RID: 14065
			public class UNCULTURED
			{
				// Token: 0x0400D768 RID: 55144
				public static LocString NAME = "Uncultured";

				// Token: 0x0400D769 RID: 55145
				public static LocString DESC = "This Duplicant has simply no appreciation for the arts";
			}

			// Token: 0x020036F2 RID: 14066
			public class EARLYBIRD
			{
				// Token: 0x0400D76A RID: 55146
				public static LocString NAME = "Early Bird";

				// Token: 0x0400D76B RID: 55147
				public static LocString DESC = "This Duplicant always wakes up feeling fresh and efficient!";

				// Token: 0x0400D76C RID: 55148
				public static LocString EXTENDED_DESC = string.Concat(new string[]
				{
					"• Morning: <b>{0}</b> bonus to all ",
					UI.PRE_KEYWORD,
					"Attributes",
					UI.PST_KEYWORD,
					"\n• Duration: 5 Schedule Blocks"
				});

				// Token: 0x0400D76D RID: 55149
				public static LocString SHORT_DESC = "Gains morning Attribute bonuses";

				// Token: 0x0400D76E RID: 55150
				public static LocString SHORT_DESC_TOOLTIP = string.Concat(new string[]
				{
					"Morning: <b>+2</b> bonus to all ",
					UI.PRE_KEYWORD,
					"Attributes",
					UI.PST_KEYWORD,
					"\n• Duration: 5 Schedule Blocks"
				});
			}

			// Token: 0x020036F3 RID: 14067
			public class NIGHTOWL
			{
				// Token: 0x0400D76F RID: 55151
				public static LocString NAME = "Night Owl";

				// Token: 0x0400D770 RID: 55152
				public static LocString DESC = "This Duplicant does their best work when they'd ought to be sleeping";

				// Token: 0x0400D771 RID: 55153
				public static LocString EXTENDED_DESC = string.Concat(new string[]
				{
					"• Nighttime: <b>{0}</b> bonus to all ",
					UI.PRE_KEYWORD,
					"Attributes",
					UI.PST_KEYWORD,
					"\n• Duration: All Night"
				});

				// Token: 0x0400D772 RID: 55154
				public static LocString SHORT_DESC = "Gains nighttime Attribute bonuses";

				// Token: 0x0400D773 RID: 55155
				public static LocString SHORT_DESC_TOOLTIP = string.Concat(new string[]
				{
					"Nighttime: <b>+3</b> bonus to all ",
					UI.PRE_KEYWORD,
					"Attributes",
					UI.PST_KEYWORD,
					"\n• Duration: All Night"
				});
			}

			// Token: 0x020036F4 RID: 14068
			public class METEORPHILE
			{
				// Token: 0x0400D774 RID: 55156
				public static LocString NAME = "Rock Fan";

				// Token: 0x0400D775 RID: 55157
				public static LocString DESC = "Meteor showers get this Duplicant really, really hyped";

				// Token: 0x0400D776 RID: 55158
				public static LocString EXTENDED_DESC = "• During meteor showers: <b>{0}</b> bonus to all " + UI.PRE_KEYWORD + "Attributes" + UI.PST_KEYWORD;

				// Token: 0x0400D777 RID: 55159
				public static LocString SHORT_DESC = "Gains Attribute bonuses during meteor showers.";

				// Token: 0x0400D778 RID: 55160
				public static LocString SHORT_DESC_TOOLTIP = "During meteor showers: <b>+3</b> bonus to all " + UI.PRE_KEYWORD + "Attributes" + UI.PST_KEYWORD;
			}

			// Token: 0x020036F5 RID: 14069
			public class REGENERATION
			{
				// Token: 0x0400D779 RID: 55161
				public static LocString NAME = "Regenerative";

				// Token: 0x0400D77A RID: 55162
				public static LocString DESC = "This robust Duplicant is constantly regenerating health";
			}

			// Token: 0x020036F6 RID: 14070
			public class DEEPERDIVERSLUNGS
			{
				// Token: 0x0400D77B RID: 55163
				public static LocString NAME = "Deep Diver's Lungs";

				// Token: 0x0400D77C RID: 55164
				public static LocString DESC = "This Duplicant has a frankly impressive ability to hold their breath";
			}

			// Token: 0x020036F7 RID: 14071
			public class SUNNYDISPOSITION
			{
				// Token: 0x0400D77D RID: 55165
				public static LocString NAME = "Sunny Disposition";

				// Token: 0x0400D77E RID: 55166
				public static LocString DESC = "This Duplicant has an unwaveringly positive outlook on life";
			}

			// Token: 0x020036F8 RID: 14072
			public class ROCKCRUSHER
			{
				// Token: 0x0400D77F RID: 55167
				public static LocString NAME = "Beefsteak";

				// Token: 0x0400D780 RID: 55168
				public static LocString DESC = "This Duplicant's got muscles on their muscles!";
			}

			// Token: 0x020036F9 RID: 14073
			public class SIMPLETASTES
			{
				// Token: 0x0400D781 RID: 55169
				public static LocString NAME = "Shrivelled Tastebuds";

				// Token: 0x0400D782 RID: 55170
				public static LocString DESC = "This Duplicant could lick a Puft's backside and taste nothing";
			}

			// Token: 0x020036FA RID: 14074
			public class FOODIE
			{
				// Token: 0x0400D783 RID: 55171
				public static LocString NAME = "Gourmet";

				// Token: 0x0400D784 RID: 55172
				public static LocString DESC = "This Duplicant's refined palate demands only the most luxurious dishes the colony can offer";
			}

			// Token: 0x020036FB RID: 14075
			public class ARCHAEOLOGIST
			{
				// Token: 0x0400D785 RID: 55173
				public static LocString NAME = "Relic Hunter";

				// Token: 0x0400D786 RID: 55174
				public static LocString DESC = "This Duplicant was never taught the phrase \"take only pictures, leave only footprints\"";
			}

			// Token: 0x020036FC RID: 14076
			public class DECORUP
			{
				// Token: 0x0400D787 RID: 55175
				public static LocString NAME = "Innately Stylish";

				// Token: 0x0400D788 RID: 55176
				public static LocString DESC = "This Duplicant's radiant self-confidence makes even the rattiest outfits look trendy";
			}

			// Token: 0x020036FD RID: 14077
			public class DECORDOWN
			{
				// Token: 0x0400D789 RID: 55177
				public static LocString NAME = "Shabby Dresser";

				// Token: 0x0400D78A RID: 55178
				public static LocString DESC = "This Duplicant's clearly never heard of ironing";
			}

			// Token: 0x020036FE RID: 14078
			public class THRIVER
			{
				// Token: 0x0400D78B RID: 55179
				public static LocString NAME = "Duress to Impress";

				// Token: 0x0400D78C RID: 55180
				public static LocString DESC = "This Duplicant kicks into hyperdrive when the stress is on";

				// Token: 0x0400D78D RID: 55181
				public static LocString SHORT_DESC = "Attribute bonuses while stressed";

				// Token: 0x0400D78E RID: 55182
				public static LocString SHORT_DESC_TOOLTIP = "More than 60% Stress: <b>+7</b> bonus to all " + UI.FormatAsKeyWord("Attributes");
			}

			// Token: 0x020036FF RID: 14079
			public class LONER
			{
				// Token: 0x0400D78F RID: 55183
				public static LocString NAME = "Loner";

				// Token: 0x0400D790 RID: 55184
				public static LocString DESC = "This Duplicant prefers solitary pursuits";

				// Token: 0x0400D791 RID: 55185
				public static LocString SHORT_DESC = "Attribute bonuses while alone";

				// Token: 0x0400D792 RID: 55186
				public static LocString SHORT_DESC_TOOLTIP = "Only Duplicant on a world: <b>+4</b> bonus to all " + UI.FormatAsKeyWord("Attributes");
			}

			// Token: 0x02003700 RID: 14080
			public class STARRYEYED
			{
				// Token: 0x0400D793 RID: 55187
				public static LocString NAME = "Starry Eyed";

				// Token: 0x0400D794 RID: 55188
				public static LocString DESC = "This Duplicant loves being in space";

				// Token: 0x0400D795 RID: 55189
				public static LocString SHORT_DESC = "Morale bonus while in space";

				// Token: 0x0400D796 RID: 55190
				public static LocString SHORT_DESC_TOOLTIP = "In outer space: <b>+10</b> " + UI.FormatAsKeyWord("Morale");
			}

			// Token: 0x02003701 RID: 14081
			public class GLOWSTICK
			{
				// Token: 0x0400D797 RID: 55191
				public static LocString NAME = "Glow Stick";

				// Token: 0x0400D798 RID: 55192
				public static LocString DESC = "This Duplicant is positively glowing";

				// Token: 0x0400D799 RID: 55193
				public static LocString SHORT_DESC = "Emits low amounts of rads and light";

				// Token: 0x0400D79A RID: 55194
				public static LocString SHORT_DESC_TOOLTIP = "Emits low amounts of rads and light";
			}

			// Token: 0x02003702 RID: 14082
			public class RADIATIONEATER
			{
				// Token: 0x0400D79B RID: 55195
				public static LocString NAME = "Radiation Eater";

				// Token: 0x0400D79C RID: 55196
				public static LocString DESC = "This Duplicant eats radiation for breakfast (and dinner)";

				// Token: 0x0400D79D RID: 55197
				public static LocString SHORT_DESC = "Converts radiation exposure into calories";

				// Token: 0x0400D79E RID: 55198
				public static LocString SHORT_DESC_TOOLTIP = "Converts radiation exposure into calories";
			}

			// Token: 0x02003703 RID: 14083
			public class NIGHTLIGHT
			{
				// Token: 0x0400D79F RID: 55199
				public static LocString NAME = "Nyctophobic";

				// Token: 0x0400D7A0 RID: 55200
				public static LocString DESC = "This Duplicant will imagine scary shapes in the dark all night if no one leaves a light on";

				// Token: 0x0400D7A1 RID: 55201
				public static LocString SHORT_DESC = "Requires light to sleep";

				// Token: 0x0400D7A2 RID: 55202
				public static LocString SHORT_DESC_TOOLTIP = "This Duplicant can't sleep in complete darkness";
			}

			// Token: 0x02003704 RID: 14084
			public class GREENTHUMB
			{
				// Token: 0x0400D7A3 RID: 55203
				public static LocString NAME = "Green Thumb";

				// Token: 0x0400D7A4 RID: 55204
				public static LocString DESC = "This Duplicant regards every plant as a potential friend";
			}

			// Token: 0x02003705 RID: 14085
			public class FROSTPROOF
			{
				// Token: 0x0400D7A5 RID: 55205
				public static LocString NAME = "Frost Proof";

				// Token: 0x0400D7A6 RID: 55206
				public static LocString DESC = "This Duplicant is too cool to be bothered by the cold";
			}

			// Token: 0x02003706 RID: 14086
			public class CONSTRUCTIONUP
			{
				// Token: 0x0400D7A7 RID: 55207
				public static LocString NAME = "Handy";

				// Token: 0x0400D7A8 RID: 55208
				public static LocString DESC = "This Duplicant is a swift and skilled builder";
			}

			// Token: 0x02003707 RID: 14087
			public class RANCHINGUP
			{
				// Token: 0x0400D7A9 RID: 55209
				public static LocString NAME = "Animal Lover";

				// Token: 0x0400D7AA RID: 55210
				public static LocString DESC = "The fuzzy snoots! The little claws! The chitinous exoskeletons! This Duplicant's never met a critter they didn't like";
			}

			// Token: 0x02003708 RID: 14088
			public class CONSTRUCTIONDOWN
			{
				// Token: 0x0400D7AB RID: 55211
				public static LocString NAME = "Building Impaired";

				// Token: 0x0400D7AC RID: 55212
				public static LocString DESC = "This Duplicant has trouble constructing anything besides meaningful friendships";
			}

			// Token: 0x02003709 RID: 14089
			public class RANCHINGDOWN
			{
				// Token: 0x0400D7AD RID: 55213
				public static LocString NAME = "Critter Aversion";

				// Token: 0x0400D7AE RID: 55214
				public static LocString DESC = "This Duplicant just doesn't trust those beady little eyes";
			}

			// Token: 0x0200370A RID: 14090
			public class DIGGINGDOWN
			{
				// Token: 0x0400D7AF RID: 55215
				public static LocString NAME = "Undigging";

				// Token: 0x0400D7B0 RID: 55216
				public static LocString DESC = "This Duplicant couldn't dig themselves out of a paper bag";
			}

			// Token: 0x0200370B RID: 14091
			public class MACHINERYDOWN
			{
				// Token: 0x0400D7B1 RID: 55217
				public static LocString NAME = "Luddite";

				// Token: 0x0400D7B2 RID: 55218
				public static LocString DESC = "This Duplicant always invites friends over just to make them hook up their electronics";
			}

			// Token: 0x0200370C RID: 14092
			public class COOKINGDOWN
			{
				// Token: 0x0400D7B3 RID: 55219
				public static LocString NAME = "Kitchen Menace";

				// Token: 0x0400D7B4 RID: 55220
				public static LocString DESC = "This Duplicant could probably figure out a way to burn ice cream";
			}

			// Token: 0x0200370D RID: 14093
			public class ARTDOWN
			{
				// Token: 0x0400D7B5 RID: 55221
				public static LocString NAME = "Unpracticed Artist";

				// Token: 0x0400D7B6 RID: 55222
				public static LocString DESC = "This Duplicant proudly proclaims they \"can't even draw a stick figure\"";
			}

			// Token: 0x0200370E RID: 14094
			public class CARINGDOWN
			{
				// Token: 0x0400D7B7 RID: 55223
				public static LocString NAME = "Unempathetic";

				// Token: 0x0400D7B8 RID: 55224
				public static LocString DESC = "This Duplicant's lack of bedside manner makes it difficult for them to nurse peers back to health";
			}

			// Token: 0x0200370F RID: 14095
			public class BOTANISTDOWN
			{
				// Token: 0x0400D7B9 RID: 55225
				public static LocString NAME = "Plant Murderer";

				// Token: 0x0400D7BA RID: 55226
				public static LocString DESC = "Never ask this Duplicant to watch your ferns when you go on vacation";
			}

			// Token: 0x02003710 RID: 14096
			public class GRANTSKILL_MINING1
			{
				// Token: 0x0400D7BB RID: 55227
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.JUNIOR_MINER.NAME;

				// Token: 0x0400D7BC RID: 55228
				public static LocString DESC = DUPLICANTS.ROLES.JUNIOR_MINER.DESCRIPTION;

				// Token: 0x0400D7BD RID: 55229
				public static LocString SHORT_DESC = "Starts with a Tier 1 <b>Skill</b>";

				// Token: 0x0400D7BE RID: 55230
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x02003711 RID: 14097
			public class GRANTSKILL_MINING2
			{
				// Token: 0x0400D7BF RID: 55231
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.MINER.NAME;

				// Token: 0x0400D7C0 RID: 55232
				public static LocString DESC = DUPLICANTS.ROLES.MINER.DESCRIPTION;

				// Token: 0x0400D7C1 RID: 55233
				public static LocString SHORT_DESC = "Starts with a Tier 2 <b>Skill</b>";

				// Token: 0x0400D7C2 RID: 55234
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x02003712 RID: 14098
			public class GRANTSKILL_MINING3
			{
				// Token: 0x0400D7C3 RID: 55235
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.SENIOR_MINER.NAME;

				// Token: 0x0400D7C4 RID: 55236
				public static LocString DESC = DUPLICANTS.ROLES.SENIOR_MINER.DESCRIPTION;

				// Token: 0x0400D7C5 RID: 55237
				public static LocString SHORT_DESC = "Starts with a Tier 3 <b>Skill</b>";

				// Token: 0x0400D7C6 RID: 55238
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x02003713 RID: 14099
			public class GRANTSKILL_MINING4
			{
				// Token: 0x0400D7C7 RID: 55239
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.MASTER_MINER.NAME;

				// Token: 0x0400D7C8 RID: 55240
				public static LocString DESC = DUPLICANTS.ROLES.MASTER_MINER.DESCRIPTION;

				// Token: 0x0400D7C9 RID: 55241
				public static LocString SHORT_DESC = "Starts with a Tier 4 <b>Skill</b>";

				// Token: 0x0400D7CA RID: 55242
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x02003714 RID: 14100
			public class GRANTSKILL_BUILDING1
			{
				// Token: 0x0400D7CB RID: 55243
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.JUNIOR_BUILDER.NAME;

				// Token: 0x0400D7CC RID: 55244
				public static LocString DESC = DUPLICANTS.ROLES.JUNIOR_BUILDER.DESCRIPTION;

				// Token: 0x0400D7CD RID: 55245
				public static LocString SHORT_DESC = "Starts with a Tier 1 <b>Skill</b>";

				// Token: 0x0400D7CE RID: 55246
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x02003715 RID: 14101
			public class GRANTSKILL_BUILDING2
			{
				// Token: 0x0400D7CF RID: 55247
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.BUILDER.NAME;

				// Token: 0x0400D7D0 RID: 55248
				public static LocString DESC = DUPLICANTS.ROLES.BUILDER.DESCRIPTION;

				// Token: 0x0400D7D1 RID: 55249
				public static LocString SHORT_DESC = "Starts with a Tier 2 <b>Skill</b>";

				// Token: 0x0400D7D2 RID: 55250
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x02003716 RID: 14102
			public class GRANTSKILL_BUILDING3
			{
				// Token: 0x0400D7D3 RID: 55251
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.SENIOR_BUILDER.NAME;

				// Token: 0x0400D7D4 RID: 55252
				public static LocString DESC = DUPLICANTS.ROLES.SENIOR_BUILDER.DESCRIPTION;

				// Token: 0x0400D7D5 RID: 55253
				public static LocString SHORT_DESC = "Starts with a Tier 3 <b>Skill</b>";

				// Token: 0x0400D7D6 RID: 55254
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x02003717 RID: 14103
			public class GRANTSKILL_FARMING1
			{
				// Token: 0x0400D7D7 RID: 55255
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.JUNIOR_FARMER.NAME;

				// Token: 0x0400D7D8 RID: 55256
				public static LocString DESC = DUPLICANTS.ROLES.JUNIOR_FARMER.DESCRIPTION;

				// Token: 0x0400D7D9 RID: 55257
				public static LocString SHORT_DESC = "Starts with a Tier 1 <b>Skill</b>";

				// Token: 0x0400D7DA RID: 55258
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x02003718 RID: 14104
			public class GRANTSKILL_FARMING2
			{
				// Token: 0x0400D7DB RID: 55259
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.FARMER.NAME;

				// Token: 0x0400D7DC RID: 55260
				public static LocString DESC = DUPLICANTS.ROLES.FARMER.DESCRIPTION;

				// Token: 0x0400D7DD RID: 55261
				public static LocString SHORT_DESC = "Starts with a Tier 2 <b>Skill</b>";

				// Token: 0x0400D7DE RID: 55262
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x02003719 RID: 14105
			public class GRANTSKILL_FARMING3
			{
				// Token: 0x0400D7DF RID: 55263
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.SENIOR_FARMER.NAME;

				// Token: 0x0400D7E0 RID: 55264
				public static LocString DESC = DUPLICANTS.ROLES.SENIOR_FARMER.DESCRIPTION;

				// Token: 0x0400D7E1 RID: 55265
				public static LocString SHORT_DESC = "Starts with a Tier 3 <b>Skill</b>";

				// Token: 0x0400D7E2 RID: 55266
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x0200371A RID: 14106
			public class GRANTSKILL_RANCHING1
			{
				// Token: 0x0400D7E3 RID: 55267
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.RANCHER.NAME;

				// Token: 0x0400D7E4 RID: 55268
				public static LocString DESC = DUPLICANTS.ROLES.RANCHER.DESCRIPTION;

				// Token: 0x0400D7E5 RID: 55269
				public static LocString SHORT_DESC = "Starts with a Tier 2 <b>Skill</b>";

				// Token: 0x0400D7E6 RID: 55270
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x0200371B RID: 14107
			public class GRANTSKILL_RANCHING2
			{
				// Token: 0x0400D7E7 RID: 55271
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.SENIOR_RANCHER.NAME;

				// Token: 0x0400D7E8 RID: 55272
				public static LocString DESC = DUPLICANTS.ROLES.SENIOR_RANCHER.DESCRIPTION;

				// Token: 0x0400D7E9 RID: 55273
				public static LocString SHORT_DESC = "Starts with a Tier 3 <b>Skill</b>";

				// Token: 0x0400D7EA RID: 55274
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x0200371C RID: 14108
			public class GRANTSKILL_RESEARCHING1
			{
				// Token: 0x0400D7EB RID: 55275
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.JUNIOR_RESEARCHER.NAME;

				// Token: 0x0400D7EC RID: 55276
				public static LocString DESC = DUPLICANTS.ROLES.JUNIOR_RESEARCHER.DESCRIPTION;

				// Token: 0x0400D7ED RID: 55277
				public static LocString SHORT_DESC = "Starts with a Tier 1 <b>Skill</b>";

				// Token: 0x0400D7EE RID: 55278
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x0200371D RID: 14109
			public class GRANTSKILL_RESEARCHING2
			{
				// Token: 0x0400D7EF RID: 55279
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.RESEARCHER.NAME;

				// Token: 0x0400D7F0 RID: 55280
				public static LocString DESC = DUPLICANTS.ROLES.RESEARCHER.DESCRIPTION;

				// Token: 0x0400D7F1 RID: 55281
				public static LocString SHORT_DESC = "Starts with a Tier 2 <b>Skill</b>";

				// Token: 0x0400D7F2 RID: 55282
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x0200371E RID: 14110
			public class GRANTSKILL_RESEARCHING3
			{
				// Token: 0x0400D7F3 RID: 55283
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.SENIOR_RESEARCHER.NAME;

				// Token: 0x0400D7F4 RID: 55284
				public static LocString DESC = DUPLICANTS.ROLES.SENIOR_RESEARCHER.DESCRIPTION;

				// Token: 0x0400D7F5 RID: 55285
				public static LocString SHORT_DESC = "Starts with a Tier 3 <b>Skill</b>";

				// Token: 0x0400D7F6 RID: 55286
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x0200371F RID: 14111
			public class GRANTSKILL_RESEARCHING4
			{
				// Token: 0x0400D7F7 RID: 55287
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.NUCLEAR_RESEARCHER.NAME;

				// Token: 0x0400D7F8 RID: 55288
				public static LocString DESC = DUPLICANTS.ROLES.NUCLEAR_RESEARCHER.DESCRIPTION;

				// Token: 0x0400D7F9 RID: 55289
				public static LocString SHORT_DESC = "Starts with a Tier 3 <b>Skill</b>";

				// Token: 0x0400D7FA RID: 55290
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x02003720 RID: 14112
			public class GRANTSKILL_COOKING1
			{
				// Token: 0x0400D7FB RID: 55291
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.JUNIOR_COOK.NAME;

				// Token: 0x0400D7FC RID: 55292
				public static LocString DESC = DUPLICANTS.ROLES.JUNIOR_COOK.DESCRIPTION;

				// Token: 0x0400D7FD RID: 55293
				public static LocString SHORT_DESC = "Starts with a Tier 1 <b>Skill</b>";

				// Token: 0x0400D7FE RID: 55294
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x02003721 RID: 14113
			public class GRANTSKILL_COOKING2
			{
				// Token: 0x0400D7FF RID: 55295
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.COOK.NAME;

				// Token: 0x0400D800 RID: 55296
				public static LocString DESC = DUPLICANTS.ROLES.COOK.DESCRIPTION;

				// Token: 0x0400D801 RID: 55297
				public static LocString SHORT_DESC = "Starts with a Tier 2 <b>Skill</b>";

				// Token: 0x0400D802 RID: 55298
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x02003722 RID: 14114
			public class GRANTSKILL_ARTING1
			{
				// Token: 0x0400D803 RID: 55299
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.JUNIOR_ARTIST.NAME;

				// Token: 0x0400D804 RID: 55300
				public static LocString DESC = DUPLICANTS.ROLES.JUNIOR_ARTIST.DESCRIPTION;

				// Token: 0x0400D805 RID: 55301
				public static LocString SHORT_DESC = "Starts with a Tier 1 <b>Skill</b>";

				// Token: 0x0400D806 RID: 55302
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x02003723 RID: 14115
			public class GRANTSKILL_ARTING2
			{
				// Token: 0x0400D807 RID: 55303
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.ARTIST.NAME;

				// Token: 0x0400D808 RID: 55304
				public static LocString DESC = DUPLICANTS.ROLES.ARTIST.DESCRIPTION;

				// Token: 0x0400D809 RID: 55305
				public static LocString SHORT_DESC = "Starts with a Tier 2 <b>Skill</b>";

				// Token: 0x0400D80A RID: 55306
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x02003724 RID: 14116
			public class GRANTSKILL_ARTING3
			{
				// Token: 0x0400D80B RID: 55307
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.MASTER_ARTIST.NAME;

				// Token: 0x0400D80C RID: 55308
				public static LocString DESC = DUPLICANTS.ROLES.MASTER_ARTIST.DESCRIPTION;

				// Token: 0x0400D80D RID: 55309
				public static LocString SHORT_DESC = "Starts with a Tier 3 <b>Skill</b>";

				// Token: 0x0400D80E RID: 55310
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x02003725 RID: 14117
			public class GRANTSKILL_HAULING1
			{
				// Token: 0x0400D80F RID: 55311
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.HAULER.NAME;

				// Token: 0x0400D810 RID: 55312
				public static LocString DESC = DUPLICANTS.ROLES.HAULER.DESCRIPTION;

				// Token: 0x0400D811 RID: 55313
				public static LocString SHORT_DESC = "Starts with a Tier 1 <b>Skill</b>";

				// Token: 0x0400D812 RID: 55314
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x02003726 RID: 14118
			public class GRANTSKILL_HAULING2
			{
				// Token: 0x0400D813 RID: 55315
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.MATERIALS_MANAGER.NAME;

				// Token: 0x0400D814 RID: 55316
				public static LocString DESC = DUPLICANTS.ROLES.MATERIALS_MANAGER.DESCRIPTION;

				// Token: 0x0400D815 RID: 55317
				public static LocString SHORT_DESC = "Starts with a Tier 2 <b>Skill</b>";

				// Token: 0x0400D816 RID: 55318
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x02003727 RID: 14119
			public class GRANTSKILL_SUITS1
			{
				// Token: 0x0400D817 RID: 55319
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.SUIT_EXPERT.NAME;

				// Token: 0x0400D818 RID: 55320
				public static LocString DESC = DUPLICANTS.ROLES.SUIT_EXPERT.DESCRIPTION;

				// Token: 0x0400D819 RID: 55321
				public static LocString SHORT_DESC = "Starts with a Tier 3 <b>Skill</b>";

				// Token: 0x0400D81A RID: 55322
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x02003728 RID: 14120
			public class GRANTSKILL_TECHNICALS1
			{
				// Token: 0x0400D81B RID: 55323
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.MACHINE_TECHNICIAN.NAME;

				// Token: 0x0400D81C RID: 55324
				public static LocString DESC = DUPLICANTS.ROLES.MACHINE_TECHNICIAN.DESCRIPTION;

				// Token: 0x0400D81D RID: 55325
				public static LocString SHORT_DESC = "Starts with a Tier 1 <b>Skill</b>";

				// Token: 0x0400D81E RID: 55326
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x02003729 RID: 14121
			public class GRANTSKILL_TECHNICALS2
			{
				// Token: 0x0400D81F RID: 55327
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.POWER_TECHNICIAN.NAME;

				// Token: 0x0400D820 RID: 55328
				public static LocString DESC = DUPLICANTS.ROLES.POWER_TECHNICIAN.DESCRIPTION;

				// Token: 0x0400D821 RID: 55329
				public static LocString SHORT_DESC = "Starts with a Tier 2 <b>Skill</b>";

				// Token: 0x0400D822 RID: 55330
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x0200372A RID: 14122
			public class GRANTSKILL_ENGINEERING1
			{
				// Token: 0x0400D823 RID: 55331
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.MECHATRONIC_ENGINEER.NAME;

				// Token: 0x0400D824 RID: 55332
				public static LocString DESC = DUPLICANTS.ROLES.MECHATRONIC_ENGINEER.DESCRIPTION;

				// Token: 0x0400D825 RID: 55333
				public static LocString SHORT_DESC = "Starts with a Tier 3 <b>Skill</b>";

				// Token: 0x0400D826 RID: 55334
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x0200372B RID: 14123
			public class GRANTSKILL_BASEKEEPING1
			{
				// Token: 0x0400D827 RID: 55335
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.HANDYMAN.NAME;

				// Token: 0x0400D828 RID: 55336
				public static LocString DESC = DUPLICANTS.ROLES.HANDYMAN.DESCRIPTION;

				// Token: 0x0400D829 RID: 55337
				public static LocString SHORT_DESC = "Starts with a Tier 1 <b>Skill</b>";

				// Token: 0x0400D82A RID: 55338
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x0200372C RID: 14124
			public class GRANTSKILL_BASEKEEPING2
			{
				// Token: 0x0400D82B RID: 55339
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.PLUMBER.NAME;

				// Token: 0x0400D82C RID: 55340
				public static LocString DESC = DUPLICANTS.ROLES.PLUMBER.DESCRIPTION;

				// Token: 0x0400D82D RID: 55341
				public static LocString SHORT_DESC = "Starts with a Tier 2 <b>Skill</b>";

				// Token: 0x0400D82E RID: 55342
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x0200372D RID: 14125
			public class GRANTSKILL_ASTRONAUTING1
			{
				// Token: 0x0400D82F RID: 55343
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.ASTRONAUTTRAINEE.NAME;

				// Token: 0x0400D830 RID: 55344
				public static LocString DESC = DUPLICANTS.ROLES.ASTRONAUTTRAINEE.DESCRIPTION;

				// Token: 0x0400D831 RID: 55345
				public static LocString SHORT_DESC = "Starts with a Tier 4 <b>Skill</b>";

				// Token: 0x0400D832 RID: 55346
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x0200372E RID: 14126
			public class GRANTSKILL_ASTRONAUTING2
			{
				// Token: 0x0400D833 RID: 55347
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.ASTRONAUT.NAME;

				// Token: 0x0400D834 RID: 55348
				public static LocString DESC = DUPLICANTS.ROLES.ASTRONAUT.DESCRIPTION;

				// Token: 0x0400D835 RID: 55349
				public static LocString SHORT_DESC = "Starts with a Tier 5 <b>Skill</b>";

				// Token: 0x0400D836 RID: 55350
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x0200372F RID: 14127
			public class GRANTSKILL_MEDICINE1
			{
				// Token: 0x0400D837 RID: 55351
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.JUNIOR_MEDIC.NAME;

				// Token: 0x0400D838 RID: 55352
				public static LocString DESC = DUPLICANTS.ROLES.JUNIOR_MEDIC.DESCRIPTION;

				// Token: 0x0400D839 RID: 55353
				public static LocString SHORT_DESC = "Starts with a Tier 1 <b>Skill</b>";

				// Token: 0x0400D83A RID: 55354
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x02003730 RID: 14128
			public class GRANTSKILL_MEDICINE2
			{
				// Token: 0x0400D83B RID: 55355
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.MEDIC.NAME;

				// Token: 0x0400D83C RID: 55356
				public static LocString DESC = DUPLICANTS.ROLES.MEDIC.DESCRIPTION;

				// Token: 0x0400D83D RID: 55357
				public static LocString SHORT_DESC = "Starts with a Tier 2 <b>Skill</b>";

				// Token: 0x0400D83E RID: 55358
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x02003731 RID: 14129
			public class GRANTSKILL_MEDICINE3
			{
				// Token: 0x0400D83F RID: 55359
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.SENIOR_MEDIC.NAME;

				// Token: 0x0400D840 RID: 55360
				public static LocString DESC = DUPLICANTS.ROLES.SENIOR_MEDIC.DESCRIPTION;

				// Token: 0x0400D841 RID: 55361
				public static LocString SHORT_DESC = "Starts with a Tier 3 <b>Skill</b>";

				// Token: 0x0400D842 RID: 55362
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x02003732 RID: 14130
			public class GRANTSKILL_PYROTECHNICS
			{
				// Token: 0x0400D843 RID: 55363
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.PYROTECHNIC.NAME;

				// Token: 0x0400D844 RID: 55364
				public static LocString DESC = DUPLICANTS.ROLES.PYROTECHNIC.DESCRIPTION;

				// Token: 0x0400D845 RID: 55365
				public static LocString SHORT_DESC = "Starts with a Tier 3 <b>Skill</b>";

				// Token: 0x0400D846 RID: 55366
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x02003733 RID: 14131
			public class BIONICBASELINE
			{
				// Token: 0x0400D847 RID: 55367
				public static LocString NAME = "Bionic Bug: Gormless";

				// Token: 0x0400D848 RID: 55368
				public static LocString DESC = "This Duplicant has built-in limitations that cannot be changed";

				// Token: 0x0400D849 RID: 55369
				public static LocString SHORT_DESC = "";

				// Token: 0x0400D84A RID: 55370
				public static LocString SHORT_DESC_TOOLTIP = "Intelligence is one thing, instinct is another. This Duplicant struggles with both";
			}

			// Token: 0x02003734 RID: 14132
			public class DEFAULTBIONICBOOSTDIGGING
			{
				// Token: 0x0400D84B RID: 55371
				public static LocString NAME = "Bionic Booster: Excavation";

				// Token: 0x0400D84C RID: 55372
				public static LocString DESC = "This Duplicant begins with a pre-installed <b>Excavation Booster</b>";

				// Token: 0x0400D84D RID: 55373
				public static LocString SHORT_DESC = "Increased <b>Digging</b>";

				// Token: 0x0400D84E RID: 55374
				public static LocString SHORT_DESC_TOOLTIP = "Bionic Duplicants can install boosters that provide an immediate increase to specific skills\n\nThis Duplicant's pre-installed Excavation Booster gives them increased <b>Digging</b>";
			}

			// Token: 0x02003735 RID: 14133
			public class DEFAULTBIONICBOOSTBUILDING
			{
				// Token: 0x0400D84F RID: 55375
				public static LocString NAME = "Bionic Booster: Building";

				// Token: 0x0400D850 RID: 55376
				public static LocString DESC = "This Duplicant begins with a pre-installed <b>Building Booster</b>";

				// Token: 0x0400D851 RID: 55377
				public static LocString SHORT_DESC = "Increased <b>Construction</b>";

				// Token: 0x0400D852 RID: 55378
				public static LocString SHORT_DESC_TOOLTIP = "Bionic Duplicants can install boosters that provide an immediate increase to specific skills\n\nThis Duplicant's pre-installed Building Booster gives them increased <b>Construction</b>";
			}

			// Token: 0x02003736 RID: 14134
			public class DEFAULTBIONICBOOSTCOOKING
			{
				// Token: 0x0400D853 RID: 55379
				public static LocString NAME = "Bionic Booster: Cooking";

				// Token: 0x0400D854 RID: 55380
				public static LocString DESC = "This Duplicant begins with a pre-installed <b>Cooking Booster</b>";

				// Token: 0x0400D855 RID: 55381
				public static LocString SHORT_DESC = "Increased <b>Cuisine</b>";

				// Token: 0x0400D856 RID: 55382
				public static LocString SHORT_DESC_TOOLTIP = "Bionic Duplicants can install boosters that provide an immediate increase to specific skills\n\nThis Duplicant's pre-installed Cooking Booster gives them increased <b>Cuisine</b>";
			}

			// Token: 0x02003737 RID: 14135
			public class DEFAULTBIONICBOOSTART
			{
				// Token: 0x0400D857 RID: 55383
				public static LocString NAME = "Bionic Booster: Decorating";

				// Token: 0x0400D858 RID: 55384
				public static LocString DESC = "This Duplicant begins with a pre-installed <b>Decorating Booster</b>";

				// Token: 0x0400D859 RID: 55385
				public static LocString SHORT_DESC = "Increased <b>Creativity</b>";

				// Token: 0x0400D85A RID: 55386
				public static LocString SHORT_DESC_TOOLTIP = "Bionic Duplicants can install boosters that provide an immediate increase to specific skills\n\nThis Duplicant's pre-installed Decorating Booster gives them increased <b>Creativity</b>";
			}

			// Token: 0x02003738 RID: 14136
			public class DEFAULTBIONICBOOSTFARMING
			{
				// Token: 0x0400D85B RID: 55387
				public static LocString NAME = "Bionic Booster: Farming";

				// Token: 0x0400D85C RID: 55388
				public static LocString DESC = "This Duplicant begins with a pre-installed <b>Farming Booster</b>";

				// Token: 0x0400D85D RID: 55389
				public static LocString SHORT_DESC = "Increased <b>Agriculture</b>";

				// Token: 0x0400D85E RID: 55390
				public static LocString SHORT_DESC_TOOLTIP = "Bionic Duplicants can install boosters that provide an immediate increase to specific skills\n\nThis Duplicant's pre-installed Farming Booster gives them increased <b>Agriculture</b>";
			}

			// Token: 0x02003739 RID: 14137
			public class DEFAULTBIONICBOOSTRANCHING
			{
				// Token: 0x0400D85F RID: 55391
				public static LocString NAME = "Bionic Booster: Ranching";

				// Token: 0x0400D860 RID: 55392
				public static LocString DESC = "This Duplicant begins with a pre-installed <b>Ranching Booster</b>";

				// Token: 0x0400D861 RID: 55393
				public static LocString SHORT_DESC = "Increased <b>Husbandry</b>";

				// Token: 0x0400D862 RID: 55394
				public static LocString SHORT_DESC_TOOLTIP = "Bionic Duplicants can install boosters that provide an immediate increase to specific skills\n\nThis Duplicant's pre-installed Ranching Booster gives them increased <b>Husbandry</b>";
			}

			// Token: 0x0200373A RID: 14138
			public class DEFAULTBIONICBOOSTMEDICINE
			{
				// Token: 0x0400D863 RID: 55395
				public static LocString NAME = "Bionic Booster: Doctoring";

				// Token: 0x0400D864 RID: 55396
				public static LocString DESC = "This Duplicant begins with a pre-installed <b>Doctoring Booster</b>";

				// Token: 0x0400D865 RID: 55397
				public static LocString SHORT_DESC = "Increased <b>Medicine</b>";

				// Token: 0x0400D866 RID: 55398
				public static LocString SHORT_DESC_TOOLTIP = "Bionic Duplicants can install boosters that provide an immediate increase to specific skills\n\nThis Duplicant's pre-installed Doctoring Booster gives them increased <b>Medicine</b>";
			}

			// Token: 0x0200373B RID: 14139
			public class DEFAULTBIONICBOOSTEXPLORER
			{
				// Token: 0x0400D867 RID: 55399
				public static LocString NAME = "Bionic Booster: Dowsing";

				// Token: 0x0400D868 RID: 55400
				public static LocString DESC = "This Duplicant begins with a pre-installed <b>Dowsing Booster</b>";

				// Token: 0x0400D869 RID: 55401
				public static LocString SHORT_DESC = "Locates undiscovered geysers";

				// Token: 0x0400D86A RID: 55402
				public static LocString SHORT_DESC_TOOLTIP = "Bionic Duplicants can install boosters that provide an immediate increase to specific skills\n\nThis Duplicant's pre-installed Dowsing Booster allows them to locate undiscovered geysers";
			}
		}

		// Token: 0x0200373C RID: 14140
		public class PERSONALITIES
		{
			// Token: 0x0200373D RID: 14141
			public class CATALINA
			{
				// Token: 0x0400D86B RID: 55403
				public static LocString NAME = "Catalina";

				// Token: 0x0400D86C RID: 55404
				public static LocString DESC = "A {0} is admired by all for her seemingly tireless work ethic. Little do people know, she's dying on the inside.";
			}

			// Token: 0x0200373E RID: 14142
			public class NISBET
			{
				// Token: 0x0400D86D RID: 55405
				public static LocString NAME = "Nisbet";

				// Token: 0x0400D86E RID: 55406
				public static LocString DESC = "This {0} likes to punch people to show her affection. Everyone's too afraid of her to tell her it hurts.";
			}

			// Token: 0x0200373F RID: 14143
			public class ELLIE
			{
				// Token: 0x0400D86F RID: 55407
				public static LocString NAME = "Ellie";

				// Token: 0x0400D870 RID: 55408
				public static LocString DESC = "Nothing makes an {0} happier than a big tin of glitter and a pack of unicorn stickers.";
			}

			// Token: 0x02003740 RID: 14144
			public class RUBY
			{
				// Token: 0x0400D871 RID: 55409
				public static LocString NAME = "Ruby";

				// Token: 0x0400D872 RID: 55410
				public static LocString DESC = "This {0} asks the pressing questions, like \"Where can I get a leather jacket in space?\"";
			}

			// Token: 0x02003741 RID: 14145
			public class LEIRA
			{
				// Token: 0x0400D873 RID: 55411
				public static LocString NAME = "Leira";

				// Token: 0x0400D874 RID: 55412
				public static LocString DESC = "{0}s just want everyone to be happy.";
			}

			// Token: 0x02003742 RID: 14146
			public class BUBBLES
			{
				// Token: 0x0400D875 RID: 55413
				public static LocString NAME = "Bubbles";

				// Token: 0x0400D876 RID: 55414
				public static LocString DESC = "This {0} is constantly challenging others to fight her, regardless of whether or not she can actually take them.";
			}

			// Token: 0x02003743 RID: 14147
			public class MIMA
			{
				// Token: 0x0400D877 RID: 55415
				public static LocString NAME = "Mi-Ma";

				// Token: 0x0400D878 RID: 55416
				public static LocString DESC = "Ol' {0} here can't stand lookin' at people's knees.";
			}

			// Token: 0x02003744 RID: 14148
			public class NAILS
			{
				// Token: 0x0400D879 RID: 55417
				public static LocString NAME = "Nails";

				// Token: 0x0400D87A RID: 55418
				public static LocString DESC = "People often expect a Duplicant named \"{0}\" to be tough, but they're all pretty huge wimps.";
			}

			// Token: 0x02003745 RID: 14149
			public class MAE
			{
				// Token: 0x0400D87B RID: 55419
				public static LocString NAME = "Mae";

				// Token: 0x0400D87C RID: 55420
				public static LocString DESC = "There's nothing a {0} can't do if she sets her mind to it.";
			}

			// Token: 0x02003746 RID: 14150
			public class GOSSMANN
			{
				// Token: 0x0400D87D RID: 55421
				public static LocString NAME = "Gossmann";

				// Token: 0x0400D87E RID: 55422
				public static LocString DESC = "{0}s are major goofballs who can make anyone laugh.";
			}

			// Token: 0x02003747 RID: 14151
			public class MARIE
			{
				// Token: 0x0400D87F RID: 55423
				public static LocString NAME = "Marie";

				// Token: 0x0400D880 RID: 55424
				public static LocString DESC = "This {0} is positively glowing! What's her secret? Radioactive isotopes, of course.";
			}

			// Token: 0x02003748 RID: 14152
			public class LINDSAY
			{
				// Token: 0x0400D881 RID: 55425
				public static LocString NAME = "Lindsay";

				// Token: 0x0400D882 RID: 55426
				public static LocString DESC = "A {0} is a charming woman, unless you make the mistake of messing with one of her friends.";
			}

			// Token: 0x02003749 RID: 14153
			public class DEVON
			{
				// Token: 0x0400D883 RID: 55427
				public static LocString NAME = "Devon";

				// Token: 0x0400D884 RID: 55428
				public static LocString DESC = "This {0} dreams of owning their own personal computer so they can start a blog full of pictures of toast.";
			}

			// Token: 0x0200374A RID: 14154
			public class REN
			{
				// Token: 0x0400D885 RID: 55429
				public static LocString NAME = "Ren";

				// Token: 0x0400D886 RID: 55430
				public static LocString DESC = "Every {0} has this unshakable feeling that his life's already happened and he's just watching it unfold from a memory.";
			}

			// Token: 0x0200374B RID: 14155
			public class FRANKIE
			{
				// Token: 0x0400D887 RID: 55431
				public static LocString NAME = "Frankie";

				// Token: 0x0400D888 RID: 55432
				public static LocString DESC = "There's nothing {0}s are more proud of than their thick, dignified eyebrows.";
			}

			// Token: 0x0200374C RID: 14156
			public class BANHI
			{
				// Token: 0x0400D889 RID: 55433
				public static LocString NAME = "Banhi";

				// Token: 0x0400D88A RID: 55434
				public static LocString DESC = "The \"cool loner\" vibes that radiate off a {0} never fail to make the colony swoon.";
			}

			// Token: 0x0200374D RID: 14157
			public class ADA
			{
				// Token: 0x0400D88B RID: 55435
				public static LocString NAME = "Ada";

				// Token: 0x0400D88C RID: 55436
				public static LocString DESC = "{0}s enjoy writing poetry in their downtime. Dark poetry.";
			}

			// Token: 0x0200374E RID: 14158
			public class HASSAN
			{
				// Token: 0x0400D88D RID: 55437
				public static LocString NAME = "Hassan";

				// Token: 0x0400D88E RID: 55438
				public static LocString DESC = "If someone says something nice to a {0} he'll think about it nonstop for no less than three weeks.";
			}

			// Token: 0x0200374F RID: 14159
			public class STINKY
			{
				// Token: 0x0400D88F RID: 55439
				public static LocString NAME = "Stinky";

				// Token: 0x0400D890 RID: 55440
				public static LocString DESC = "This {0} has never been invited to a party, which is a shame. His dance moves are incredible.";
			}

			// Token: 0x02003750 RID: 14160
			public class JOSHUA
			{
				// Token: 0x0400D891 RID: 55441
				public static LocString NAME = "Joshua";

				// Token: 0x0400D892 RID: 55442
				public static LocString DESC = "{0}s are precious goobers. Other Duplicants are strangely incapable of cursing in a {0}'s presence.";
			}

			// Token: 0x02003751 RID: 14161
			public class LIAM
			{
				// Token: 0x0400D893 RID: 55443
				public static LocString NAME = "Liam";

				// Token: 0x0400D894 RID: 55444
				public static LocString DESC = "No matter how much this {0} scrubs, he can never truly feel clean.";
			}

			// Token: 0x02003752 RID: 14162
			public class ABE
			{
				// Token: 0x0400D895 RID: 55445
				public static LocString NAME = "Abe";

				// Token: 0x0400D896 RID: 55446
				public static LocString DESC = "{0}s are sweet, delicate flowers. They need to be treated gingerly, with great consideration for their feelings.";
			}

			// Token: 0x02003753 RID: 14163
			public class BURT
			{
				// Token: 0x0400D897 RID: 55447
				public static LocString NAME = "Burt";

				// Token: 0x0400D898 RID: 55448
				public static LocString DESC = "This {0} always feels great after a bubble bath and a good long cry.";
			}

			// Token: 0x02003754 RID: 14164
			public class TRAVALDO
			{
				// Token: 0x0400D899 RID: 55449
				public static LocString NAME = "Travaldo";

				// Token: 0x0400D89A RID: 55450
				public static LocString DESC = "A {0}'s monotonous voice and lack of facial expression makes it impossible for others to tell when he's messing with them.";
			}

			// Token: 0x02003755 RID: 14165
			public class HAROLD
			{
				// Token: 0x0400D89B RID: 55451
				public static LocString NAME = "Harold";

				// Token: 0x0400D89C RID: 55452
				public static LocString DESC = "Get a bunch of {0}s together in a room, and you'll have... a bunch of {0}s together in a room.";
			}

			// Token: 0x02003756 RID: 14166
			public class MAX
			{
				// Token: 0x0400D89D RID: 55453
				public static LocString NAME = "Max";

				// Token: 0x0400D89E RID: 55454
				public static LocString DESC = "At any given moment a {0} is viscerally reliving ten different humiliating memories.";
			}

			// Token: 0x02003757 RID: 14167
			public class ROWAN
			{
				// Token: 0x0400D89F RID: 55455
				public static LocString NAME = "Rowan";

				// Token: 0x0400D8A0 RID: 55456
				public static LocString DESC = "{0}s have exceptionally large hearts and express their emotions most efficiently by yelling.";
			}

			// Token: 0x02003758 RID: 14168
			public class OTTO
			{
				// Token: 0x0400D8A1 RID: 55457
				public static LocString NAME = "Otto";

				// Token: 0x0400D8A2 RID: 55458
				public static LocString DESC = "{0}s always insult people by accident and generally exist in a perpetual state of deep regret.";
			}

			// Token: 0x02003759 RID: 14169
			public class TURNER
			{
				// Token: 0x0400D8A3 RID: 55459
				public static LocString NAME = "Turner";

				// Token: 0x0400D8A4 RID: 55460
				public static LocString DESC = "This {0} is paralyzed by the knowledge that others have memories and perceptions of them they can't control.";
			}

			// Token: 0x0200375A RID: 14170
			public class NIKOLA
			{
				// Token: 0x0400D8A5 RID: 55461
				public static LocString NAME = "Nikola";

				// Token: 0x0400D8A6 RID: 55462
				public static LocString DESC = "This {0} once claimed he could build a laser so powerful it would rip the colony in half. No one asked him to prove it.";
			}

			// Token: 0x0200375B RID: 14171
			public class MEEP
			{
				// Token: 0x0400D8A7 RID: 55463
				public static LocString NAME = "Meep";

				// Token: 0x0400D8A8 RID: 55464
				public static LocString DESC = "{0}s have a face only a two tonne Printing Pod could love.";
			}

			// Token: 0x0200375C RID: 14172
			public class ARI
			{
				// Token: 0x0400D8A9 RID: 55465
				public static LocString NAME = "Ari";

				// Token: 0x0400D8AA RID: 55466
				public static LocString DESC = "{0}s tend to space out from time to time, but they always pay attention when it counts.";
			}

			// Token: 0x0200375D RID: 14173
			public class JEAN
			{
				// Token: 0x0400D8AB RID: 55467
				public static LocString NAME = "Jean";

				// Token: 0x0400D8AC RID: 55468
				public static LocString DESC = "Just because {0}s are a little slow doesn't mean they can't suffer from soul-crushing existential crises.";
			}

			// Token: 0x0200375E RID: 14174
			public class CAMILLE
			{
				// Token: 0x0400D8AD RID: 55469
				public static LocString NAME = "Camille";

				// Token: 0x0400D8AE RID: 55470
				public static LocString DESC = "This {0} loves anything that makes her feel nostalgic, including things that haven't aged well.";
			}

			// Token: 0x0200375F RID: 14175
			public class ASHKAN
			{
				// Token: 0x0400D8AF RID: 55471
				public static LocString NAME = "Ashkan";

				// Token: 0x0400D8B0 RID: 55472
				public static LocString DESC = "{0}s have what can only be described as a \"seriously infectious giggle\".";
			}

			// Token: 0x02003760 RID: 14176
			public class STEVE
			{
				// Token: 0x0400D8B1 RID: 55473
				public static LocString NAME = "Steve";

				// Token: 0x0400D8B2 RID: 55474
				public static LocString DESC = "This {0} is convinced that he has psychic powers. And he knows exactly what his friends think about that.";
			}

			// Token: 0x02003761 RID: 14177
			public class AMARI
			{
				// Token: 0x0400D8B3 RID: 55475
				public static LocString NAME = "Amari";

				// Token: 0x0400D8B4 RID: 55476
				public static LocString DESC = "{0}s likes to keep the peace. Ironically, they're a riot at parties.";
			}

			// Token: 0x02003762 RID: 14178
			public class PEI
			{
				// Token: 0x0400D8B5 RID: 55477
				public static LocString NAME = "Pei";

				// Token: 0x0400D8B6 RID: 55478
				public static LocString DESC = "Every {0} spends at least half the day pretending that they remember what they came into this room for.";
			}

			// Token: 0x02003763 RID: 14179
			public class QUINN
			{
				// Token: 0x0400D8B7 RID: 55479
				public static LocString NAME = "Quinn";

				// Token: 0x0400D8B8 RID: 55480
				public static LocString DESC = "This {0}'s favorite genre of music is \"festive power ballad\".";
			}

			// Token: 0x02003764 RID: 14180
			public class JORGE
			{
				// Token: 0x0400D8B9 RID: 55481
				public static LocString NAME = "Jorge";

				// Token: 0x0400D8BA RID: 55482
				public static LocString DESC = "{0} loves his new colony, even if their collective body odor makes his eyes water.";
			}

			// Token: 0x02003765 RID: 14181
			public class CALVIN
			{
				// Token: 0x0400D8BB RID: 55483
				public static LocString NAME = "Calvin";

				// Token: 0x0400D8BC RID: 55484
				public static LocString DESC = "This {0} loves the thrill of running head-first into a wall to see what it's made of.";
			}

			// Token: 0x02003766 RID: 14182
			public class FREYJA
			{
				// Token: 0x0400D8BD RID: 55485
				public static LocString NAME = "Freyja";

				// Token: 0x0400D8BE RID: 55486
				public static LocString DESC = "This {0} has never stopped anyone from eating yellow snow.";
			}

			// Token: 0x02003767 RID: 14183
			public class CHIP
			{
				// Token: 0x0400D8BF RID: 55487
				public static LocString NAME = "Chip";

				// Token: 0x0400D8C0 RID: 55488
				public static LocString DESC = "This {0} is extremely good at guessing their friends' passwords.";
			}

			// Token: 0x02003768 RID: 14184
			public class EDWIREDO
			{
				// Token: 0x0400D8C1 RID: 55489
				public static LocString NAME = "Edwiredo";

				// Token: 0x0400D8C2 RID: 55490
				public static LocString DESC = "This {0} once rolled his eye so hard that he powered himself off and on again.";
			}

			// Token: 0x02003769 RID: 14185
			public class GIZMO
			{
				// Token: 0x0400D8C3 RID: 55491
				public static LocString NAME = "Gizmo";

				// Token: 0x0400D8C4 RID: 55492
				public static LocString DESC = "{0}s love nothing more than a big juicy info dump.";
			}

			// Token: 0x0200376A RID: 14186
			public class STEELA
			{
				// Token: 0x0400D8C5 RID: 55493
				public static LocString NAME = "Steela";

				// Token: 0x0400D8C6 RID: 55494
				public static LocString DESC = "{0}s aren't programmed to put up with nonsense, but they do enjoy the occasional shenanigan.";
			}

			// Token: 0x0200376B RID: 14187
			public class SONYAR
			{
				// Token: 0x0400D8C7 RID: 55495
				public static LocString NAME = "Sonyar";

				// Token: 0x0400D8C8 RID: 55496
				public static LocString DESC = "{0}s would sooner burn down the colony than read an instruction manual.";
			}

			// Token: 0x0200376C RID: 14188
			public class ULTI
			{
				// Token: 0x0400D8C9 RID: 55497
				public static LocString NAME = "Ulti";

				// Token: 0x0400D8CA RID: 55498
				public static LocString DESC = "The only dance move this {0} knows is The Robot.";
			}
		}

		// Token: 0x0200376D RID: 14189
		public class NEEDS
		{
			// Token: 0x0200376E RID: 14190
			public class DECOR
			{
				// Token: 0x0400D8CB RID: 55499
				public static LocString NAME = "Decor Expectation";

				// Token: 0x0400D8CC RID: 55500
				public static LocString PROFESSION_NAME = "Critic";

				// Token: 0x0400D8CD RID: 55501
				public static LocString OBSERVED_DECOR = "Current Surroundings";

				// Token: 0x0400D8CE RID: 55502
				public static LocString EXPECTATION_TOOLTIP = string.Concat(new string[]
				{
					"Most objects have ",
					UI.PRE_KEYWORD,
					"Decor",
					UI.PST_KEYWORD,
					" values that alter Duplicants' opinions of their surroundings.\nThis Duplicant desires ",
					UI.PRE_KEYWORD,
					"Decor",
					UI.PST_KEYWORD,
					" values of <b>{0}</b> or higher, and becomes ",
					UI.PRE_KEYWORD,
					"Stressed",
					UI.PST_KEYWORD,
					" in areas with lower ",
					UI.PRE_KEYWORD,
					"Decor",
					UI.PST_KEYWORD,
					"."
				});

				// Token: 0x0400D8CF RID: 55503
				public static LocString EXPECTATION_MOD_NAME = "Job Tier Request";
			}

			// Token: 0x0200376F RID: 14191
			public class FOOD_QUALITY
			{
				// Token: 0x0400D8D0 RID: 55504
				public static LocString NAME = "Food Quality";

				// Token: 0x0400D8D1 RID: 55505
				public static LocString PROFESSION_NAME = "Gourmet";

				// Token: 0x0400D8D2 RID: 55506
				public static LocString EXPECTATION_TOOLTIP = string.Concat(new string[]
				{
					"Each Duplicant has a minimum quality of ",
					UI.PRE_KEYWORD,
					"Food",
					UI.PST_KEYWORD,
					" they'll tolerate eating.\nThis Duplicant desires <b>Tier {0}<b> or better ",
					UI.PRE_KEYWORD,
					"Food",
					UI.PST_KEYWORD,
					", and becomes ",
					UI.PRE_KEYWORD,
					"Stressed",
					UI.PST_KEYWORD,
					" when they eat meals of lower quality."
				});

				// Token: 0x0400D8D3 RID: 55507
				public static LocString BAD_FOOD_MOD = "Food Quality";

				// Token: 0x0400D8D4 RID: 55508
				public static LocString NORMAL_FOOD_MOD = "Food Quality";

				// Token: 0x0400D8D5 RID: 55509
				public static LocString GOOD_FOOD_MOD = "Food Quality";

				// Token: 0x0400D8D6 RID: 55510
				public static LocString EXPECTATION_MOD_NAME = "Job Tier Request";

				// Token: 0x0400D8D7 RID: 55511
				public static LocString ADJECTIVE_FORMAT_POSITIVE = "{0} [{1}]";

				// Token: 0x0400D8D8 RID: 55512
				public static LocString ADJECTIVE_FORMAT_NEGATIVE = "{0} [{1}]";

				// Token: 0x0400D8D9 RID: 55513
				public static LocString FOODQUALITY = "\nFood Quality Score of {0}";

				// Token: 0x0400D8DA RID: 55514
				public static LocString FOODQUALITY_EXPECTATION = string.Concat(new string[]
				{
					"\nThis Duplicant is content to eat ",
					UI.PRE_KEYWORD,
					"Food",
					UI.PST_KEYWORD,
					" with a ",
					UI.PRE_KEYWORD,
					"Food Quality",
					UI.PST_KEYWORD,
					" of <b>{0}</b> or higher"
				});

				// Token: 0x0400D8DB RID: 55515
				public static int ADJECTIVE_INDEX_OFFSET = -1;

				// Token: 0x02003770 RID: 14192
				public class ADJECTIVES
				{
					// Token: 0x0400D8DC RID: 55516
					public static LocString MINUS_1 = "Grisly";

					// Token: 0x0400D8DD RID: 55517
					public static LocString ZERO = "Terrible";

					// Token: 0x0400D8DE RID: 55518
					public static LocString PLUS_1 = "Poor";

					// Token: 0x0400D8DF RID: 55519
					public static LocString PLUS_2 = "Standard";

					// Token: 0x0400D8E0 RID: 55520
					public static LocString PLUS_3 = "Good";

					// Token: 0x0400D8E1 RID: 55521
					public static LocString PLUS_4 = "Great";

					// Token: 0x0400D8E2 RID: 55522
					public static LocString PLUS_5 = "Superb";

					// Token: 0x0400D8E3 RID: 55523
					public static LocString PLUS_6 = "Ambrosial";
				}
			}

			// Token: 0x02003771 RID: 14193
			public class QUALITYOFLIFE
			{
				// Token: 0x0400D8E4 RID: 55524
				public static LocString NAME = "Morale Requirements";

				// Token: 0x0400D8E5 RID: 55525
				public static LocString EXPECTATION_TOOLTIP = string.Concat(new string[]
				{
					"The more responsibilities and stressors a Duplicant has, the more they will desire additional leisure time and improved amenities.\n\nFailing to keep a Duplicant's ",
					UI.PRE_KEYWORD,
					"Morale",
					UI.PST_KEYWORD,
					" at or above their ",
					UI.PRE_KEYWORD,
					"Morale Need",
					UI.PST_KEYWORD,
					" means they will not be able to unwind, causing them ",
					UI.PRE_KEYWORD,
					"Stress",
					UI.PST_KEYWORD,
					" over time."
				});

				// Token: 0x0400D8E6 RID: 55526
				public static LocString EXPECTATION_MOD_NAME = "Skills Learned";

				// Token: 0x0400D8E7 RID: 55527
				public static LocString APTITUDE_SKILLS_MOD_NAME = "Interested Skills Learned";

				// Token: 0x0400D8E8 RID: 55528
				public static LocString TOTAL_SKILL_POINTS = "Total Skill Points: {0}";

				// Token: 0x0400D8E9 RID: 55529
				public static LocString GOOD_MODIFIER = "High Morale";

				// Token: 0x0400D8EA RID: 55530
				public static LocString NEUTRAL_MODIFIER = "Sufficient Morale";

				// Token: 0x0400D8EB RID: 55531
				public static LocString BAD_MODIFIER = "Low Morale";
			}

			// Token: 0x02003772 RID: 14194
			public class NOISE
			{
				// Token: 0x0400D8EC RID: 55532
				public static LocString NAME = "Noise Expectation";
			}
		}

		// Token: 0x02003773 RID: 14195
		public class ATTRIBUTES
		{
			// Token: 0x0400D8ED RID: 55533
			public static LocString VALUE = "{0}: {1}";

			// Token: 0x0400D8EE RID: 55534
			public static LocString TOTAL_VALUE = "\n\nTotal <b>{1}</b>: {0}";

			// Token: 0x0400D8EF RID: 55535
			public static LocString BASE_VALUE = "\nBase: {0}";

			// Token: 0x0400D8F0 RID: 55536
			public static LocString MODIFIER_ENTRY = "\n    • {0}: {1}";

			// Token: 0x0400D8F1 RID: 55537
			public static LocString UNPROFESSIONAL_NAME = "Lump";

			// Token: 0x0400D8F2 RID: 55538
			public static LocString UNPROFESSIONAL_DESC = "This Duplicant has no discernible skills";

			// Token: 0x0400D8F3 RID: 55539
			public static LocString PROFESSION_DESC = string.Concat(new string[]
			{
				"Expertise is determined by a Duplicant's highest ",
				UI.PRE_KEYWORD,
				"Attribute",
				UI.PST_KEYWORD,
				"\n\nDuplicants develop higher expectations as their Expertise level increases"
			});

			// Token: 0x0400D8F4 RID: 55540
			public static LocString STORED_VALUE = "Stored value";

			// Token: 0x02003774 RID: 14196
			public class CONSTRUCTION
			{
				// Token: 0x0400D8F5 RID: 55541
				public static LocString NAME = "Construction";

				// Token: 0x0400D8F6 RID: 55542
				public static LocString DESC = "Determines a Duplicant's building Speed.";

				// Token: 0x0400D8F7 RID: 55543
				public static LocString SPEEDMODIFIER = "{0} Construction Speed";
			}

			// Token: 0x02003775 RID: 14197
			public class SCALDINGTHRESHOLD
			{
				// Token: 0x0400D8F8 RID: 55544
				public static LocString NAME = "Scalding Threshold";

				// Token: 0x0400D8F9 RID: 55545
				public static LocString DESC = string.Concat(new string[]
				{
					"Determines the ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" at which a Duplicant will get burned."
				});
			}

			// Token: 0x02003776 RID: 14198
			public class SCOLDINGTHRESHOLD
			{
				// Token: 0x0400D8FA RID: 55546
				public static LocString NAME = "Frostbite Threshold";

				// Token: 0x0400D8FB RID: 55547
				public static LocString DESC = string.Concat(new string[]
				{
					"Determines the ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" at which a Duplicant will get frostbitten."
				});
			}

			// Token: 0x02003777 RID: 14199
			public class DIGGING
			{
				// Token: 0x0400D8FC RID: 55548
				public static LocString NAME = "Excavation";

				// Token: 0x0400D8FD RID: 55549
				public static LocString DESC = "Determines a Duplicant's mining speed.";

				// Token: 0x0400D8FE RID: 55550
				public static LocString SPEEDMODIFIER = "{0} Digging Speed";

				// Token: 0x0400D8FF RID: 55551
				public static LocString ATTACK_MODIFIER = "{0} Attack Damage";
			}

			// Token: 0x02003778 RID: 14200
			public class MACHINERY
			{
				// Token: 0x0400D900 RID: 55552
				public static LocString NAME = "Machinery";

				// Token: 0x0400D901 RID: 55553
				public static LocString DESC = "Determines how quickly a Duplicant uses machines.";

				// Token: 0x0400D902 RID: 55554
				public static LocString SPEEDMODIFIER = "{0} Machine Operation Speed";

				// Token: 0x0400D903 RID: 55555
				public static LocString TINKER_EFFECT_MODIFIER = "{0} Engie's Tune-Up Effect Duration";
			}

			// Token: 0x02003779 RID: 14201
			public class LIFESUPPORT
			{
				// Token: 0x0400D904 RID: 55556
				public static LocString NAME = "Life Support";

				// Token: 0x0400D905 RID: 55557
				public static LocString DESC = string.Concat(new string[]
				{
					"Determines how efficiently a Duplicant maintains ",
					BUILDINGS.PREFABS.ALGAEHABITAT.NAME,
					"s, ",
					BUILDINGS.PREFABS.AIRFILTER.NAME,
					"s, and ",
					BUILDINGS.PREFABS.WATERPURIFIER.NAME,
					"s"
				});
			}

			// Token: 0x0200377A RID: 14202
			public class TOGGLE
			{
				// Token: 0x0400D906 RID: 55558
				public static LocString NAME = "Toggle";

				// Token: 0x0400D907 RID: 55559
				public static LocString DESC = "Determines how efficiently a Duplicant tunes machinery, flips switches, and sets sensors.";
			}

			// Token: 0x0200377B RID: 14203
			public class ATHLETICS
			{
				// Token: 0x0400D908 RID: 55560
				public static LocString NAME = "Athletics";

				// Token: 0x0400D909 RID: 55561
				public static LocString DESC = "Determines a Duplicant's default runspeed.";

				// Token: 0x0400D90A RID: 55562
				public static LocString SPEEDMODIFIER = "{0} Runspeed";
			}

			// Token: 0x0200377C RID: 14204
			public class LUMINESCENCE
			{
				// Token: 0x0400D90B RID: 55563
				public static LocString NAME = "Luminescence";

				// Token: 0x0400D90C RID: 55564
				public static LocString DESC = "Determines how much light a Duplicant emits.";
			}

			// Token: 0x0200377D RID: 14205
			public class TRANSITTUBETRAVELSPEED
			{
				// Token: 0x0400D90D RID: 55565
				public static LocString NAME = "Transit Speed";

				// Token: 0x0400D90E RID: 55566
				public static LocString DESC = "Determines a Duplicant's default " + BUILDINGS.PREFABS.TRAVELTUBE.NAME + " travel speed.";

				// Token: 0x0400D90F RID: 55567
				public static LocString SPEEDMODIFIER = "{0} Transit Tube Travel Speed";
			}

			// Token: 0x0200377E RID: 14206
			public class DOCTOREDLEVEL
			{
				// Token: 0x0400D910 RID: 55568
				public static LocString NAME = UI.FormatAsLink("Treatment Received", "MEDICINE") + " Effect";

				// Token: 0x0400D911 RID: 55569
				public static LocString DESC = string.Concat(new string[]
				{
					"Duplicants who receive medical care while in a ",
					BUILDINGS.PREFABS.DOCTORSTATION.NAME,
					" or ",
					BUILDINGS.PREFABS.ADVANCEDDOCTORSTATION.NAME,
					" will gain the ",
					UI.PRE_KEYWORD,
					"Treatment Received",
					UI.PST_KEYWORD,
					" effect\n\nThis effect reduces the severity of ",
					UI.PRE_KEYWORD,
					"Disease",
					UI.PST_KEYWORD,
					" symptoms"
				});
			}

			// Token: 0x0200377F RID: 14207
			public class SNEEZYNESS
			{
				// Token: 0x0400D912 RID: 55570
				public static LocString NAME = "Sneeziness";

				// Token: 0x0400D913 RID: 55571
				public static LocString DESC = "Determines how frequently a Duplicant sneezes.";
			}

			// Token: 0x02003780 RID: 14208
			public class GERMRESISTANCE
			{
				// Token: 0x0400D914 RID: 55572
				public static LocString NAME = "Germ Resistance";

				// Token: 0x0400D915 RID: 55573
				public static LocString DESC = string.Concat(new string[]
				{
					"Duplicants with a higher ",
					UI.PRE_KEYWORD,
					"Germ Resistance",
					UI.PST_KEYWORD,
					" rating are less likely to contract germ-based ",
					UI.PRE_KEYWORD,
					"Diseases",
					UI.PST_KEYWORD,
					"."
				});

				// Token: 0x02003781 RID: 14209
				public class MODIFIER_DESCRIPTORS
				{
					// Token: 0x0400D916 RID: 55574
					public static LocString NEGATIVE_LARGE = "{0} (Large Loss)";

					// Token: 0x0400D917 RID: 55575
					public static LocString NEGATIVE_MEDIUM = "{0} (Medium Loss)";

					// Token: 0x0400D918 RID: 55576
					public static LocString NEGATIVE_SMALL = "{0} (Small Loss)";

					// Token: 0x0400D919 RID: 55577
					public static LocString NONE = "No Effect";

					// Token: 0x0400D91A RID: 55578
					public static LocString POSITIVE_SMALL = "{0} (Small Boost)";

					// Token: 0x0400D91B RID: 55579
					public static LocString POSITIVE_MEDIUM = "{0} (Medium Boost)";

					// Token: 0x0400D91C RID: 55580
					public static LocString POSITIVE_LARGE = "{0} (Large Boost)";
				}
			}

			// Token: 0x02003782 RID: 14210
			public class LEARNING
			{
				// Token: 0x0400D91D RID: 55581
				public static LocString NAME = "Science";

				// Token: 0x0400D91E RID: 55582
				public static LocString DESC = string.Concat(new string[]
				{
					"Determines how quickly a Duplicant conducts ",
					UI.PRE_KEYWORD,
					"Research",
					UI.PST_KEYWORD,
					" and gains ",
					UI.PRE_KEYWORD,
					"Skill Points",
					UI.PST_KEYWORD,
					"."
				});

				// Token: 0x0400D91F RID: 55583
				public static LocString SPEEDMODIFIER = "{0} Skill Leveling";

				// Token: 0x0400D920 RID: 55584
				public static LocString RESEARCHSPEED = "{0} Research Speed";

				// Token: 0x0400D921 RID: 55585
				public static LocString GEOTUNER_SPEED_MODIFIER = "{0} Geotuning Speed";
			}

			// Token: 0x02003783 RID: 14211
			public class COOKING
			{
				// Token: 0x0400D922 RID: 55586
				public static LocString NAME = "Cuisine";

				// Token: 0x0400D923 RID: 55587
				public static LocString DESC = string.Concat(new string[]
				{
					"Determines how quickly a Duplicant prepares ",
					UI.PRE_KEYWORD,
					"Food",
					UI.PST_KEYWORD,
					"."
				});

				// Token: 0x0400D924 RID: 55588
				public static LocString SPEEDMODIFIER = "{0} Cooking Speed";
			}

			// Token: 0x02003784 RID: 14212
			public class HAPPINESSDELTA
			{
				// Token: 0x0400D925 RID: 55589
				public static LocString NAME = "Happiness";

				// Token: 0x0400D926 RID: 55590
				public static LocString DESC = "Contented " + UI.FormatAsLink("Critters", "CREATURES") + " produce usable materials with increased frequency.";
			}

			// Token: 0x02003785 RID: 14213
			public class RADIATIONBALANCEDELTA
			{
				// Token: 0x0400D927 RID: 55591
				public static LocString NAME = "Absorbed Radiation Dose";

				// Token: 0x0400D928 RID: 55592
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Duplicants accumulate Rads in areas with ",
					UI.PRE_KEYWORD,
					"Radiation",
					UI.PST_KEYWORD,
					" and recover at very slow rates\n\nOpen the ",
					UI.FormatAsOverlay("Radiation Overlay", global::Action.Overlay15),
					" to view current ",
					UI.PRE_KEYWORD,
					"Rad",
					UI.PST_KEYWORD,
					" readings"
				});
			}

			// Token: 0x02003786 RID: 14214
			public class INSULATION
			{
				// Token: 0x0400D929 RID: 55593
				public static LocString NAME = "Insulation";

				// Token: 0x0400D92A RID: 55594
				public static LocString DESC = string.Concat(new string[]
				{
					"Highly ",
					UI.PRE_KEYWORD,
					"Insulated",
					UI.PST_KEYWORD,
					" Duplicants retain body heat easily, while low ",
					UI.PRE_KEYWORD,
					"Insulation",
					UI.PST_KEYWORD,
					" Duplicants are easier to keep cool."
				});

				// Token: 0x0400D92B RID: 55595
				public static LocString SPEEDMODIFIER = "{0} Temperature Retention";
			}

			// Token: 0x02003787 RID: 14215
			public class STRENGTH
			{
				// Token: 0x0400D92C RID: 55596
				public static LocString NAME = "Strength";

				// Token: 0x0400D92D RID: 55597
				public static LocString DESC = string.Concat(new string[]
				{
					"Determines a Duplicant's ",
					UI.PRE_KEYWORD,
					"Carrying Capacity",
					UI.PST_KEYWORD,
					" and cleaning speed."
				});

				// Token: 0x0400D92E RID: 55598
				public static LocString CARRYMODIFIER = "{0} " + DUPLICANTS.ATTRIBUTES.CARRYAMOUNT.NAME;

				// Token: 0x0400D92F RID: 55599
				public static LocString SPEEDMODIFIER = "{0} Tidying Speed";
			}

			// Token: 0x02003788 RID: 14216
			public class CARING
			{
				// Token: 0x0400D930 RID: 55600
				public static LocString NAME = "Medicine";

				// Token: 0x0400D931 RID: 55601
				public static LocString DESC = "Determines a Duplicant's ability to care for sick peers.";

				// Token: 0x0400D932 RID: 55602
				public static LocString SPEEDMODIFIER = "{0} Treatment Speed";

				// Token: 0x0400D933 RID: 55603
				public static LocString FABRICATE_SPEEDMODIFIER = "{0} Medicine Fabrication Speed";
			}

			// Token: 0x02003789 RID: 14217
			public class IMMUNITY
			{
				// Token: 0x0400D934 RID: 55604
				public static LocString NAME = "Immunity";

				// Token: 0x0400D935 RID: 55605
				public static LocString DESC = string.Concat(new string[]
				{
					"Determines a Duplicant's ",
					UI.PRE_KEYWORD,
					"Disease",
					UI.PST_KEYWORD,
					" susceptibility and recovery time."
				});

				// Token: 0x0400D936 RID: 55606
				public static LocString BOOST_MODIFIER = "{0} Immunity Regen";

				// Token: 0x0400D937 RID: 55607
				public static LocString BOOST_STAT = "Immunity Attribute";
			}

			// Token: 0x0200378A RID: 14218
			public class BOTANIST
			{
				// Token: 0x0400D938 RID: 55608
				public static LocString NAME = "Agriculture";

				// Token: 0x0400D939 RID: 55609
				public static LocString DESC = string.Concat(new string[]
				{
					"Determines how quickly and efficiently a Duplicant cultivates ",
					UI.PRE_KEYWORD,
					"Plants",
					UI.PST_KEYWORD,
					"."
				});

				// Token: 0x0400D93A RID: 55610
				public static LocString HARVEST_SPEED_MODIFIER = "{0} Harvesting Speed";

				// Token: 0x0400D93B RID: 55611
				public static LocString TINKER_MODIFIER = "{0} Tending Speed";

				// Token: 0x0400D93C RID: 55612
				public static LocString BONUS_SEEDS = "{0} Seed Chance";

				// Token: 0x0400D93D RID: 55613
				public static LocString TINKER_EFFECT_MODIFIER = "{0} Farmer's Touch Effect Duration";
			}

			// Token: 0x0200378B RID: 14219
			public class RANCHING
			{
				// Token: 0x0400D93E RID: 55614
				public static LocString NAME = "Husbandry";

				// Token: 0x0400D93F RID: 55615
				public static LocString DESC = "Determines how efficiently a Duplicant tends " + UI.FormatAsLink("Critters", "CREATURES") + ".";

				// Token: 0x0400D940 RID: 55616
				public static LocString EFFECTMODIFIER = "{0} Groom Effect Duration";

				// Token: 0x0400D941 RID: 55617
				public static LocString CAPTURABLESPEED = "{0} Wrangling Speed";
			}

			// Token: 0x0200378C RID: 14220
			public class ART
			{
				// Token: 0x0400D942 RID: 55618
				public static LocString NAME = "Creativity";

				// Token: 0x0400D943 RID: 55619
				public static LocString DESC = string.Concat(new string[]
				{
					"Determines how quickly a Duplicant produces ",
					UI.PRE_KEYWORD,
					"Artwork",
					UI.PST_KEYWORD,
					"."
				});

				// Token: 0x0400D944 RID: 55620
				public static LocString SPEEDMODIFIER = "{0} Decorating Speed";
			}

			// Token: 0x0200378D RID: 14221
			public class DECOR
			{
				// Token: 0x0400D945 RID: 55621
				public static LocString NAME = "Decor";

				// Token: 0x0400D946 RID: 55622
				public static LocString DESC = string.Concat(new string[]
				{
					"Affects a Duplicant's ",
					UI.PRE_KEYWORD,
					"Morale",
					UI.PST_KEYWORD,
					" and their opinion of their surroundings."
				});
			}

			// Token: 0x0200378E RID: 14222
			public class THERMALCONDUCTIVITYBARRIER
			{
				// Token: 0x0400D947 RID: 55623
				public static LocString NAME = "Insulation Thickness";

				// Token: 0x0400D948 RID: 55624
				public static LocString DESC = string.Concat(new string[]
				{
					"Determines how quickly a Duplicant retains or loses body ",
					UI.PRE_KEYWORD,
					"Heat",
					UI.PST_KEYWORD,
					" in any given area.\n\nIt is the sum of a Duplicant's ",
					UI.PRE_KEYWORD,
					"Equipment",
					UI.PST_KEYWORD,
					" and their natural ",
					UI.PRE_KEYWORD,
					"Insulation",
					UI.PST_KEYWORD,
					" values."
				});
			}

			// Token: 0x0200378F RID: 14223
			public class DECORRADIUS
			{
				// Token: 0x0400D949 RID: 55625
				public static LocString NAME = "Decor Radius";

				// Token: 0x0400D94A RID: 55626
				public static LocString DESC = string.Concat(new string[]
				{
					"The influence range of an object's ",
					UI.PRE_KEYWORD,
					"Decor",
					UI.PST_KEYWORD,
					" value."
				});
			}

			// Token: 0x02003790 RID: 14224
			public class DECOREXPECTATION
			{
				// Token: 0x0400D94B RID: 55627
				public static LocString NAME = "Decor Morale Bonus";

				// Token: 0x0400D94C RID: 55628
				public static LocString DESC = string.Concat(new string[]
				{
					"A Decor Morale Bonus allows Duplicants to receive ",
					UI.PRE_KEYWORD,
					"Morale",
					UI.PST_KEYWORD,
					" boosts from lower ",
					UI.PRE_KEYWORD,
					"Decor",
					UI.PST_KEYWORD,
					" values.\n\nMaintaining high ",
					UI.PRE_KEYWORD,
					"Morale",
					UI.PST_KEYWORD,
					" will allow Duplicants to learn more ",
					UI.PRE_KEYWORD,
					"Skills",
					UI.PST_KEYWORD,
					"."
				});
			}

			// Token: 0x02003791 RID: 14225
			public class FOODEXPECTATION
			{
				// Token: 0x0400D94D RID: 55629
				public static LocString NAME = "Food Morale Bonus";

				// Token: 0x0400D94E RID: 55630
				public static LocString DESC = string.Concat(new string[]
				{
					"A Food Morale Bonus allows Duplicants to receive ",
					UI.PRE_KEYWORD,
					"Morale",
					UI.PST_KEYWORD,
					" boosts from lower quality ",
					UI.PRE_KEYWORD,
					"Food",
					UI.PST_KEYWORD,
					".\n\nMaintaining high ",
					UI.PRE_KEYWORD,
					"Morale",
					UI.PST_KEYWORD,
					" will allow Duplicants to learn more ",
					UI.PRE_KEYWORD,
					"Skills",
					UI.PST_KEYWORD,
					"."
				});
			}

			// Token: 0x02003792 RID: 14226
			public class QUALITYOFLIFEEXPECTATION
			{
				// Token: 0x0400D94F RID: 55631
				public static LocString NAME = "Morale Need";

				// Token: 0x0400D950 RID: 55632
				public static LocString DESC = string.Concat(new string[]
				{
					"Dictates how high a Duplicant's ",
					UI.PRE_KEYWORD,
					"Morale",
					UI.PST_KEYWORD,
					" must be kept to prevent them from gaining ",
					UI.PRE_KEYWORD,
					"Stress",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x02003793 RID: 14227
			public class HYGIENE
			{
				// Token: 0x0400D951 RID: 55633
				public static LocString NAME = "Hygiene";

				// Token: 0x0400D952 RID: 55634
				public static LocString DESC = "Affects a Duplicant's sense of cleanliness.";
			}

			// Token: 0x02003794 RID: 14228
			public class CARRYAMOUNT
			{
				// Token: 0x0400D953 RID: 55635
				public static LocString NAME = "Carrying Capacity";

				// Token: 0x0400D954 RID: 55636
				public static LocString DESC = "Determines the maximum weight that a Duplicant can carry.";
			}

			// Token: 0x02003795 RID: 14229
			public class SPACENAVIGATION
			{
				// Token: 0x0400D955 RID: 55637
				public static LocString NAME = "Piloting";

				// Token: 0x0400D956 RID: 55638
				public static LocString DESC = "Determines how long it takes a Duplicant to complete a space mission.";

				// Token: 0x0400D957 RID: 55639
				public static LocString DLC1_DESC = "Determines how much of a speed bonus a Duplicant provides to a rocket they are piloting.";

				// Token: 0x0400D958 RID: 55640
				public static LocString SPEED_MODIFIER = "{0} Rocket Speed";
			}

			// Token: 0x02003796 RID: 14230
			public class QUALITYOFLIFE
			{
				// Token: 0x0400D959 RID: 55641
				public static LocString NAME = "Morale";

				// Token: 0x0400D95A RID: 55642
				public static LocString DESC = string.Concat(new string[]
				{
					"A Duplicant's ",
					UI.PRE_KEYWORD,
					"Morale",
					UI.PST_KEYWORD,
					" must exceed their ",
					UI.PRE_KEYWORD,
					"Morale Need",
					UI.PST_KEYWORD,
					", or they'll begin to accumulate ",
					UI.PRE_KEYWORD,
					"Stress",
					UI.PST_KEYWORD,
					".\n\n",
					UI.PRE_KEYWORD,
					"Morale",
					UI.PST_KEYWORD,
					" can be increased by providing Duplicants higher quality ",
					UI.PRE_KEYWORD,
					"Food",
					UI.PST_KEYWORD,
					", allotting more ",
					UI.PRE_KEYWORD,
					"Downtime",
					UI.PST_KEYWORD,
					" in\nthe colony schedule, or building better ",
					UI.PRE_KEYWORD,
					"Bathrooms",
					UI.PST_KEYWORD,
					" and ",
					UI.PRE_KEYWORD,
					"Bedrooms",
					UI.PST_KEYWORD,
					" for them to live in."
				});

				// Token: 0x0400D95B RID: 55643
				public static LocString DESC_FORMAT = "{0} / {1}";

				// Token: 0x0400D95C RID: 55644
				public static LocString TOOLTIP_EXPECTATION = "Total <b>Morale Need</b>: {0}\n    • Skills Learned: +{0}";

				// Token: 0x0400D95D RID: 55645
				public static LocString TOOLTIP_EXPECTATION_OVER = "This Duplicant has sufficiently high " + UI.PRE_KEYWORD + "Morale" + UI.PST_KEYWORD;

				// Token: 0x0400D95E RID: 55646
				public static LocString TOOLTIP_EXPECTATION_UNDER = string.Concat(new string[]
				{
					"This Duplicant's low ",
					UI.PRE_KEYWORD,
					"Morale",
					UI.PST_KEYWORD,
					" will cause ",
					UI.PRE_KEYWORD,
					"Stress",
					UI.PST_KEYWORD,
					" over time"
				});
			}

			// Token: 0x02003797 RID: 14231
			public class AIRCONSUMPTIONRATE
			{
				// Token: 0x0400D95F RID: 55647
				public static LocString NAME = "Air Consumption Rate";

				// Token: 0x0400D960 RID: 55648
				public static LocString DESC = "Air Consumption determines how much " + ELEMENTS.OXYGEN.NAME + " a Duplicant requires per minute to live.";
			}

			// Token: 0x02003798 RID: 14232
			public class RADIATIONRESISTANCE
			{
				// Token: 0x0400D961 RID: 55649
				public static LocString NAME = "Radiation Resistance";

				// Token: 0x0400D962 RID: 55650
				public static LocString DESC = string.Concat(new string[]
				{
					"Determines how easily a Duplicant repels ",
					UI.PRE_KEYWORD,
					"Radiation Sickness",
					UI.PST_KEYWORD,
					"."
				});
			}

			// Token: 0x02003799 RID: 14233
			public class RADIATIONRECOVERY
			{
				// Token: 0x0400D963 RID: 55651
				public static LocString NAME = "Radiation Absorption";

				// Token: 0x0400D964 RID: 55652
				public static LocString DESC = string.Concat(new string[]
				{
					"The rate at which ",
					UI.PRE_KEYWORD,
					"Radiation",
					UI.PST_KEYWORD,
					" is neutralized within a Duplicant body."
				});
			}

			// Token: 0x0200379A RID: 14234
			public class STRESSDELTA
			{
				// Token: 0x0400D965 RID: 55653
				public static LocString NAME = "Stress";

				// Token: 0x0400D966 RID: 55654
				public static LocString DESC = "Determines how quickly a Duplicant gains or reduces " + UI.PRE_KEYWORD + "Stress" + UI.PST_KEYWORD;
			}

			// Token: 0x0200379B RID: 14235
			public class BREATHDELTA
			{
				// Token: 0x0400D967 RID: 55655
				public static LocString NAME = "Breath";

				// Token: 0x0400D968 RID: 55656
				public static LocString DESC = string.Concat(new string[]
				{
					"Determines how quickly a Duplicant gains or reduces ",
					UI.PRE_KEYWORD,
					"Breath",
					UI.PST_KEYWORD,
					"."
				});
			}

			// Token: 0x0200379C RID: 14236
			public class BIONICOILDELTA
			{
				// Token: 0x0400D969 RID: 55657
				public static LocString NAME = "Oil";

				// Token: 0x0400D96A RID: 55658
				public static LocString DESC = "Determines how quickly a Duplicant's bionic parts gains or reduces " + UI.PRE_KEYWORD + "Oil" + UI.PST_KEYWORD;
			}

			// Token: 0x0200379D RID: 14237
			public class BLADDERDELTA
			{
				// Token: 0x0400D96B RID: 55659
				public static LocString NAME = "Bladder";

				// Token: 0x0400D96C RID: 55660
				public static LocString DESC = string.Concat(new string[]
				{
					"Determines how quickly a Duplicant's ",
					UI.PRE_KEYWORD,
					"Bladder",
					UI.PST_KEYWORD,
					" fills or depletes."
				});
			}

			// Token: 0x0200379E RID: 14238
			public class CALORIESDELTA
			{
				// Token: 0x0400D96D RID: 55661
				public static LocString NAME = "Calories";

				// Token: 0x0400D96E RID: 55662
				public static LocString DESC = string.Concat(new string[]
				{
					"Determines how quickly a Duplicant burns or stores ",
					UI.PRE_KEYWORD,
					"Calories",
					UI.PST_KEYWORD,
					"."
				});
			}

			// Token: 0x0200379F RID: 14239
			public class STAMINADELTA
			{
				// Token: 0x0400D96F RID: 55663
				public static LocString NAME = "Stamina";

				// Token: 0x0400D970 RID: 55664
				public static LocString DESC = "";
			}

			// Token: 0x020037A0 RID: 14240
			public class TOXICITYDELTA
			{
				// Token: 0x0400D971 RID: 55665
				public static LocString NAME = "Toxicity";

				// Token: 0x0400D972 RID: 55666
				public static LocString DESC = "";
			}

			// Token: 0x020037A1 RID: 14241
			public class IMMUNELEVELDELTA
			{
				// Token: 0x0400D973 RID: 55667
				public static LocString NAME = "Immunity";

				// Token: 0x0400D974 RID: 55668
				public static LocString DESC = "";
			}

			// Token: 0x020037A2 RID: 14242
			public class TOILETEFFICIENCY
			{
				// Token: 0x0400D975 RID: 55669
				public static LocString NAME = "Bathroom Use Speed";

				// Token: 0x0400D976 RID: 55670
				public static LocString DESC = "Determines how long a Duplicant needs to do their \"business\".";

				// Token: 0x0400D977 RID: 55671
				public static LocString SPEEDMODIFIER = "{0} Bathroom Use Speed";
			}

			// Token: 0x020037A3 RID: 14243
			public class METABOLISM
			{
				// Token: 0x0400D978 RID: 55672
				public static LocString NAME = "Critter Metabolism";

				// Token: 0x0400D979 RID: 55673
				public static LocString DESC = string.Concat(new string[]
				{
					"Affects the rate at which a critter burns ",
					UI.PRE_KEYWORD,
					"Calories",
					UI.PST_KEYWORD,
					" and produces materials"
				});
			}

			// Token: 0x020037A4 RID: 14244
			public class ROOMTEMPERATUREPREFERENCE
			{
				// Token: 0x0400D97A RID: 55674
				public static LocString NAME = "Temperature Preference";

				// Token: 0x0400D97B RID: 55675
				public static LocString DESC = string.Concat(new string[]
				{
					"Determines the minimum body ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" a Duplicant prefers to maintain."
				});
			}

			// Token: 0x020037A5 RID: 14245
			public class MAXUNDERWATERTRAVELCOST
			{
				// Token: 0x0400D97C RID: 55676
				public static LocString NAME = "Underwater Movement";

				// Token: 0x0400D97D RID: 55677
				public static LocString DESC = "Determines a Duplicant's runspeed when submerged in " + UI.PRE_KEYWORD + "Liquid" + UI.PST_KEYWORD;
			}

			// Token: 0x020037A6 RID: 14246
			public class OVERHEATTEMPERATURE
			{
				// Token: 0x0400D97E RID: 55678
				public static LocString NAME = "Overheat Temperature";

				// Token: 0x0400D97F RID: 55679
				public static LocString DESC = string.Concat(new string[]
				{
					"A building at Overheat ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" will take damage and break down if not cooled"
				});
			}

			// Token: 0x020037A7 RID: 14247
			public class FATALTEMPERATURE
			{
				// Token: 0x0400D980 RID: 55680
				public static LocString NAME = "Break Down Temperature";

				// Token: 0x0400D981 RID: 55681
				public static LocString DESC = string.Concat(new string[]
				{
					"A building at break down ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" will lose functionality and take damage"
				});
			}

			// Token: 0x020037A8 RID: 14248
			public class HITPOINTSDELTA
			{
				// Token: 0x0400D982 RID: 55682
				public static LocString NAME = UI.FormatAsLink("Health", "HEALTH");

				// Token: 0x0400D983 RID: 55683
				public static LocString DESC = "Health regeneration is increased when another Duplicant provides medical care to the patient";
			}

			// Token: 0x020037A9 RID: 14249
			public class DISEASECURESPEED
			{
				// Token: 0x0400D984 RID: 55684
				public static LocString NAME = UI.FormatAsLink("Disease", "DISEASE") + " Recovery Speed Bonus";

				// Token: 0x0400D985 RID: 55685
				public static LocString DESC = "Recovery speed bonus is increased when another Duplicant provides medical care to the patient";
			}

			// Token: 0x020037AA RID: 14250
			public abstract class MACHINERYSPEED
			{
				// Token: 0x0400D986 RID: 55686
				public static LocString NAME = "Machinery Speed";

				// Token: 0x0400D987 RID: 55687
				public static LocString DESC = "Speed Bonus";
			}

			// Token: 0x020037AB RID: 14251
			public abstract class GENERATOROUTPUT
			{
				// Token: 0x0400D988 RID: 55688
				public static LocString NAME = "Power Output";
			}

			// Token: 0x020037AC RID: 14252
			public abstract class ROCKETBURDEN
			{
				// Token: 0x0400D989 RID: 55689
				public static LocString NAME = "Burden";
			}

			// Token: 0x020037AD RID: 14253
			public abstract class ROCKETENGINEPOWER
			{
				// Token: 0x0400D98A RID: 55690
				public static LocString NAME = "Engine Power";
			}

			// Token: 0x020037AE RID: 14254
			public abstract class FUELRANGEPERKILOGRAM
			{
				// Token: 0x0400D98B RID: 55691
				public static LocString NAME = "Range";
			}

			// Token: 0x020037AF RID: 14255
			public abstract class HEIGHT
			{
				// Token: 0x0400D98C RID: 55692
				public static LocString NAME = "Height";
			}

			// Token: 0x020037B0 RID: 14256
			public class WILTTEMPRANGEMOD
			{
				// Token: 0x0400D98D RID: 55693
				public static LocString NAME = "Viable Temperature Range";

				// Token: 0x0400D98E RID: 55694
				public static LocString DESC = "Variance growth temperature relative to the base crop";
			}

			// Token: 0x020037B1 RID: 14257
			public class YIELDAMOUNT
			{
				// Token: 0x0400D98F RID: 55695
				public static LocString NAME = "Yield Amount";

				// Token: 0x0400D990 RID: 55696
				public static LocString DESC = "Plant production relative to the base crop";
			}

			// Token: 0x020037B2 RID: 14258
			public class HARVESTTIME
			{
				// Token: 0x0400D991 RID: 55697
				public static LocString NAME = "Harvest Duration";

				// Token: 0x0400D992 RID: 55698
				public static LocString DESC = "Time it takes an unskilled Duplicant to harvest this plant";
			}

			// Token: 0x020037B3 RID: 14259
			public class DECORBONUS
			{
				// Token: 0x0400D993 RID: 55699
				public static LocString NAME = "Decor Bonus";

				// Token: 0x0400D994 RID: 55700
				public static LocString DESC = "Change in Decor value relative to the base crop";
			}

			// Token: 0x020037B4 RID: 14260
			public class MINLIGHTLUX
			{
				// Token: 0x0400D995 RID: 55701
				public static LocString NAME = "Light";

				// Token: 0x0400D996 RID: 55702
				public static LocString DESC = "Minimum lux this plant requires for growth";
			}

			// Token: 0x020037B5 RID: 14261
			public class FERTILIZERUSAGEMOD
			{
				// Token: 0x0400D997 RID: 55703
				public static LocString NAME = "Fertilizer Usage";

				// Token: 0x0400D998 RID: 55704
				public static LocString DESC = "Fertilizer and irrigation amounts this plant requires relative to the base crop";
			}

			// Token: 0x020037B6 RID: 14262
			public class MINRADIATIONTHRESHOLD
			{
				// Token: 0x0400D999 RID: 55705
				public static LocString NAME = "Minimum Radiation";

				// Token: 0x0400D99A RID: 55706
				public static LocString DESC = "Smallest amount of ambient Radiation required for this plant to grow";
			}

			// Token: 0x020037B7 RID: 14263
			public class MAXRADIATIONTHRESHOLD
			{
				// Token: 0x0400D99B RID: 55707
				public static LocString NAME = "Maximum Radiation";

				// Token: 0x0400D99C RID: 55708
				public static LocString DESC = "Largest amount of ambient Radiation this plant can tolerate";
			}
		}

		// Token: 0x020037B8 RID: 14264
		public class ROLES
		{
			// Token: 0x020037B9 RID: 14265
			public class GROUPS
			{
				// Token: 0x0400D99D RID: 55709
				public static LocString APTITUDE_DESCRIPTION = string.Concat(new string[]
				{
					"This Duplicant will gain <b>{1}</b> ",
					UI.PRE_KEYWORD,
					"Morale",
					UI.PST_KEYWORD,
					" when learning ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" Skills"
				});

				// Token: 0x0400D99E RID: 55710
				public static LocString APTITUDE_DESCRIPTION_CHOREGROUP = string.Concat(new string[]
				{
					"{2}\n\nThis Duplicant will gain <b>+{1}</b> ",
					UI.PRE_KEYWORD,
					"Morale",
					UI.PST_KEYWORD,
					" when learning ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" Skills"
				});

				// Token: 0x0400D99F RID: 55711
				public static LocString SUITS = "Suit Wearing";
			}

			// Token: 0x020037BA RID: 14266
			public class NO_ROLE
			{
				// Token: 0x0400D9A0 RID: 55712
				public static LocString NAME = UI.FormatAsLink("Unemployed", "NO_ROLE");

				// Token: 0x0400D9A1 RID: 55713
				public static LocString DESCRIPTION = "No job assignment";
			}

			// Token: 0x020037BB RID: 14267
			public class JUNIOR_ARTIST
			{
				// Token: 0x0400D9A2 RID: 55714
				public static LocString NAME = UI.FormatAsLink("Art Fundamentals", "ARTING1");

				// Token: 0x0400D9A3 RID: 55715
				public static LocString DESCRIPTION = "Teaches the most basic level of art skill";
			}

			// Token: 0x020037BC RID: 14268
			public class ARTIST
			{
				// Token: 0x0400D9A4 RID: 55716
				public static LocString NAME = UI.FormatAsLink("Aesthetic Design", "ARTING2");

				// Token: 0x0400D9A5 RID: 55717
				public static LocString DESCRIPTION = "Allows moderately attractive art to be created";
			}

			// Token: 0x020037BD RID: 14269
			public class MASTER_ARTIST
			{
				// Token: 0x0400D9A6 RID: 55718
				public static LocString NAME = UI.FormatAsLink("Masterworks", "ARTING3");

				// Token: 0x0400D9A7 RID: 55719
				public static LocString DESCRIPTION = "Enables the painting and sculpting of masterpieces";
			}

			// Token: 0x020037BE RID: 14270
			public class JUNIOR_BUILDER
			{
				// Token: 0x0400D9A8 RID: 55720
				public static LocString NAME = UI.FormatAsLink("Improved Construction I", "BUILDING1");

				// Token: 0x0400D9A9 RID: 55721
				public static LocString DESCRIPTION = "Marginally improves a Duplicant's construction speeds";
			}

			// Token: 0x020037BF RID: 14271
			public class BUILDER
			{
				// Token: 0x0400D9AA RID: 55722
				public static LocString NAME = UI.FormatAsLink("Improved Construction II", "BUILDING2");

				// Token: 0x0400D9AB RID: 55723
				public static LocString DESCRIPTION = "Further increases a Duplicant's construction speeds";
			}

			// Token: 0x020037C0 RID: 14272
			public class SENIOR_BUILDER
			{
				// Token: 0x0400D9AC RID: 55724
				public static LocString NAME = UI.FormatAsLink("Demolition", "BUILDING3");

				// Token: 0x0400D9AD RID: 55725
				public static LocString DESCRIPTION = "Enables a Duplicant to deconstruct Gravitas buildings";
			}

			// Token: 0x020037C1 RID: 14273
			public class JUNIOR_RESEARCHER
			{
				// Token: 0x0400D9AE RID: 55726
				public static LocString NAME = UI.FormatAsLink("Advanced Research", "RESEARCHING1");

				// Token: 0x0400D9AF RID: 55727
				public static LocString DESCRIPTION = "Allows Duplicants to perform research using a " + BUILDINGS.PREFABS.ADVANCEDRESEARCHCENTER.NAME;
			}

			// Token: 0x020037C2 RID: 14274
			public class RESEARCHER
			{
				// Token: 0x0400D9B0 RID: 55728
				public static LocString NAME = UI.FormatAsLink("Field Research", "RESEARCHING2");

				// Token: 0x0400D9B1 RID: 55729
				public static LocString DESCRIPTION = string.Concat(new string[]
				{
					"Duplicants can perform studies on ",
					UI.PRE_KEYWORD,
					"Geysers",
					UI.PST_KEYWORD,
					", ",
					UI.CLUSTERMAP.PLANETOID_KEYWORD,
					", and other geographical phenomena"
				});
			}

			// Token: 0x020037C3 RID: 14275
			public class SENIOR_RESEARCHER
			{
				// Token: 0x0400D9B2 RID: 55730
				public static LocString NAME = UI.FormatAsLink("Astronomy", "ASTRONOMY");

				// Token: 0x0400D9B3 RID: 55731
				public static LocString DESCRIPTION = "Enables Duplicants to study outer space using the " + BUILDINGS.PREFABS.CLUSTERTELESCOPE.NAME;
			}

			// Token: 0x020037C4 RID: 14276
			public class NUCLEAR_RESEARCHER
			{
				// Token: 0x0400D9B4 RID: 55732
				public static LocString NAME = UI.FormatAsLink("Applied Sciences Research", "ATOMICRESEARCH");

				// Token: 0x0400D9B5 RID: 55733
				public static LocString DESCRIPTION = "Enables Duplicants to study matter using the " + BUILDINGS.PREFABS.NUCLEARRESEARCHCENTER.NAME;
			}

			// Token: 0x020037C5 RID: 14277
			public class SPACE_RESEARCHER
			{
				// Token: 0x0400D9B6 RID: 55734
				public static LocString NAME = UI.FormatAsLink("Data Analysis Researcher", "SPACERESEARCH");

				// Token: 0x0400D9B7 RID: 55735
				public static LocString DESCRIPTION = "Enables Duplicants to conduct research using the " + BUILDINGS.PREFABS.DLC1COSMICRESEARCHCENTER.NAME;
			}

			// Token: 0x020037C6 RID: 14278
			public class JUNIOR_COOK
			{
				// Token: 0x0400D9B8 RID: 55736
				public static LocString NAME = UI.FormatAsLink("Grilling", "COOKING1");

				// Token: 0x0400D9B9 RID: 55737
				public static LocString DESCRIPTION = "Allows Duplicants to cook using the " + BUILDINGS.PREFABS.COOKINGSTATION.NAME;
			}

			// Token: 0x020037C7 RID: 14279
			public class COOK
			{
				// Token: 0x0400D9BA RID: 55738
				public static LocString NAME = UI.FormatAsLink("Grilling II", "COOKING2");

				// Token: 0x0400D9BB RID: 55739
				public static LocString DESCRIPTION = "Improves a Duplicant's cooking speed";
			}

			// Token: 0x020037C8 RID: 14280
			public class JUNIOR_MEDIC
			{
				// Token: 0x0400D9BC RID: 55740
				public static LocString NAME = UI.FormatAsLink("Medicine Compounding", "MEDICINE1");

				// Token: 0x0400D9BD RID: 55741
				public static LocString DESCRIPTION = "Allows Duplicants to produce medicines at the " + BUILDINGS.PREFABS.APOTHECARY.NAME;
			}

			// Token: 0x020037C9 RID: 14281
			public class MEDIC
			{
				// Token: 0x0400D9BE RID: 55742
				public static LocString NAME = UI.FormatAsLink("Bedside Manner", "MEDICINE2");

				// Token: 0x0400D9BF RID: 55743
				public static LocString DESCRIPTION = "Trains Duplicants to administer medicine at the " + BUILDINGS.PREFABS.DOCTORSTATION.NAME;
			}

			// Token: 0x020037CA RID: 14282
			public class SENIOR_MEDIC
			{
				// Token: 0x0400D9C0 RID: 55744
				public static LocString NAME = UI.FormatAsLink("Advanced Medical Care", "MEDICINE3");

				// Token: 0x0400D9C1 RID: 55745
				public static LocString DESCRIPTION = "Trains Duplicants to operate the " + BUILDINGS.PREFABS.ADVANCEDDOCTORSTATION.NAME;
			}

			// Token: 0x020037CB RID: 14283
			public class MACHINE_TECHNICIAN
			{
				// Token: 0x0400D9C2 RID: 55746
				public static LocString NAME = UI.FormatAsLink("Improved Tinkering", "TECHNICALS1");

				// Token: 0x0400D9C3 RID: 55747
				public static LocString DESCRIPTION = "Marginally improves a Duplicant's tinkering speeds";
			}

			// Token: 0x020037CC RID: 14284
			public class OIL_TECHNICIAN
			{
				// Token: 0x0400D9C4 RID: 55748
				public static LocString NAME = UI.FormatAsLink("Oil Engineering", "OIL_TECHNICIAN");

				// Token: 0x0400D9C5 RID: 55749
				public static LocString DESCRIPTION = "Allows the extraction and refinement of " + ELEMENTS.CRUDEOIL.NAME;
			}

			// Token: 0x020037CD RID: 14285
			public class HAULER
			{
				// Token: 0x0400D9C6 RID: 55750
				public static LocString NAME = UI.FormatAsLink("Improved Carrying I", "HAULING1");

				// Token: 0x0400D9C7 RID: 55751
				public static LocString DESCRIPTION = "Minorly increase a Duplicant's strength and carrying capacity";
			}

			// Token: 0x020037CE RID: 14286
			public class MATERIALS_MANAGER
			{
				// Token: 0x0400D9C8 RID: 55752
				public static LocString NAME = UI.FormatAsLink("Improved Carrying II", "HAULING2");

				// Token: 0x0400D9C9 RID: 55753
				public static LocString DESCRIPTION = "Further increases a Duplicant's strength and carrying capacity for even swifter deliveries";
			}

			// Token: 0x020037CF RID: 14287
			public class JUNIOR_FARMER
			{
				// Token: 0x0400D9CA RID: 55754
				public static LocString NAME = UI.FormatAsLink("Improved Farming I", "FARMING1");

				// Token: 0x0400D9CB RID: 55755
				public static LocString DESCRIPTION = "Minorly increase a Duplicant's farming skills, increasing their chances of harvesting new plant " + UI.PRE_KEYWORD + "Seeds" + UI.PST_KEYWORD;
			}

			// Token: 0x020037D0 RID: 14288
			public class FARMER
			{
				// Token: 0x0400D9CC RID: 55756
				public static LocString NAME = UI.FormatAsLink("Crop Tending", "FARMING2");

				// Token: 0x0400D9CD RID: 55757
				public static LocString DESCRIPTION = string.Concat(new string[]
				{
					"Enables tending ",
					UI.PRE_KEYWORD,
					"Plants",
					UI.PST_KEYWORD,
					", which will increase their growth speed"
				});
			}

			// Token: 0x020037D1 RID: 14289
			public class SENIOR_FARMER
			{
				// Token: 0x0400D9CE RID: 55758
				public static LocString NAME = UI.FormatAsLink("Improved Farming II", "FARMING3");

				// Token: 0x0400D9CF RID: 55759
				public static LocString DESCRIPTION = "Further increases a Duplicant's farming skills";
			}

			// Token: 0x020037D2 RID: 14290
			public class JUNIOR_MINER
			{
				// Token: 0x0400D9D0 RID: 55760
				public static LocString NAME = UI.FormatAsLink("Hard Digging", "MINING1");

				// Token: 0x0400D9D1 RID: 55761
				public static LocString DESCRIPTION = string.Concat(new string[]
				{
					"Allows the excavation of ",
					UI.PRE_KEYWORD,
					ELEMENTS.HARDNESS.HARDNESS_DESCRIPTOR.VERYFIRM,
					UI.PST_KEYWORD,
					" materials such as ",
					ELEMENTS.GRANITE.NAME
				});
			}

			// Token: 0x020037D3 RID: 14291
			public class MINER
			{
				// Token: 0x0400D9D2 RID: 55762
				public static LocString NAME = UI.FormatAsLink("Superhard Digging", "MINING2");

				// Token: 0x0400D9D3 RID: 55763
				public static LocString DESCRIPTION = "Allows the excavation of the element " + ELEMENTS.KATAIRITE.NAME;
			}

			// Token: 0x020037D4 RID: 14292
			public class SENIOR_MINER
			{
				// Token: 0x0400D9D4 RID: 55764
				public static LocString NAME = UI.FormatAsLink("Super-Duperhard Digging", "MINING3");

				// Token: 0x0400D9D5 RID: 55765
				public static LocString DESCRIPTION = string.Concat(new string[]
				{
					"Allows the excavation of ",
					UI.PRE_KEYWORD,
					ELEMENTS.HARDNESS.HARDNESS_DESCRIPTOR.NEARLYIMPENETRABLE,
					UI.PST_KEYWORD,
					" elements, including ",
					ELEMENTS.DIAMOND.NAME,
					" and ",
					ELEMENTS.OBSIDIAN.NAME
				});
			}

			// Token: 0x020037D5 RID: 14293
			public class MASTER_MINER
			{
				// Token: 0x0400D9D6 RID: 55766
				public static LocString NAME = UI.FormatAsLink("Hazmat Digging", "MINING4");

				// Token: 0x0400D9D7 RID: 55767
				public static LocString DESCRIPTION = "Allows the excavation of dangerous materials like " + ELEMENTS.CORIUM.NAME;
			}

			// Token: 0x020037D6 RID: 14294
			public class SUIT_DURABILITY
			{
				// Token: 0x0400D9D8 RID: 55768
				public static LocString NAME = UI.FormatAsLink("Suit Sustainability Training", "SUITDURABILITY");

				// Token: 0x0400D9D9 RID: 55769
				public static LocString DESCRIPTION = "Suits equipped by this Duplicant lose durability " + GameUtil.GetFormattedPercent(EQUIPMENT.SUITS.SUIT_DURABILITY_SKILL_BONUS * 100f, GameUtil.TimeSlice.None) + " slower.";
			}

			// Token: 0x020037D7 RID: 14295
			public class SUIT_EXPERT
			{
				// Token: 0x0400D9DA RID: 55770
				public static LocString NAME = UI.FormatAsLink("Exosuit Training", "SUITS1");

				// Token: 0x0400D9DB RID: 55771
				public static LocString DESCRIPTION = "Eliminates the runspeed loss experienced while wearing exosuits";
			}

			// Token: 0x020037D8 RID: 14296
			public class POWER_TECHNICIAN
			{
				// Token: 0x0400D9DC RID: 55772
				public static LocString NAME = UI.FormatAsLink("Electrical Engineering", "TECHNICALS2");

				// Token: 0x0400D9DD RID: 55773
				public static LocString DESCRIPTION = string.Concat(new string[]
				{
					"Enables generator ",
					UI.PRE_KEYWORD,
					"Tune-Up",
					UI.PST_KEYWORD,
					", which will temporarily provide improved ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" output"
				});
			}

			// Token: 0x020037D9 RID: 14297
			public class MECHATRONIC_ENGINEER
			{
				// Token: 0x0400D9DE RID: 55774
				public static LocString NAME = UI.FormatAsLink("Mechatronics Engineering", "ENGINEERING1");

				// Token: 0x0400D9DF RID: 55775
				public static LocString DESCRIPTION = "Allows the construction and maintenance of " + BUILDINGS.PREFABS.SOLIDCONDUIT.NAME + " systems";
			}

			// Token: 0x020037DA RID: 14298
			public class HANDYMAN
			{
				// Token: 0x0400D9E0 RID: 55776
				public static LocString NAME = UI.FormatAsLink("Improved Strength", "BASEKEEPING1");

				// Token: 0x0400D9E1 RID: 55777
				public static LocString DESCRIPTION = "Minorly improves a Duplicant's physical strength";
			}

			// Token: 0x020037DB RID: 14299
			public class PLUMBER
			{
				// Token: 0x0400D9E2 RID: 55778
				public static LocString NAME = UI.FormatAsLink("Plumbing", "BASEKEEPING2");

				// Token: 0x0400D9E3 RID: 55779
				public static LocString DESCRIPTION = string.Concat(new string[]
				{
					"Allows a Duplicant to empty ",
					UI.PRE_KEYWORD,
					"Pipes",
					UI.PST_KEYWORD,
					" without making a mess"
				});
			}

			// Token: 0x020037DC RID: 14300
			public class PYROTECHNIC
			{
				// Token: 0x0400D9E4 RID: 55780
				public static LocString NAME = UI.FormatAsLink("Pyrotechnics", "PYROTECHNICS");

				// Token: 0x0400D9E5 RID: 55781
				public static LocString DESCRIPTION = string.Concat(new string[]
				{
					"Allows a Duplicant to make ",
					UI.PRE_KEYWORD,
					"Blastshot",
					UI.PST_KEYWORD,
					" for the ",
					UI.PRE_KEYWORD,
					"Meteor Blaster",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x020037DD RID: 14301
			public class RANCHER
			{
				// Token: 0x0400D9E6 RID: 55782
				public static LocString NAME = UI.FormatAsLink("Critter Ranching I", "RANCHING1");

				// Token: 0x0400D9E7 RID: 55783
				public static LocString DESCRIPTION = "Allows a Duplicant to handle and care for " + UI.FormatAsLink("Critters", "CREATURES");
			}

			// Token: 0x020037DE RID: 14302
			public class SENIOR_RANCHER
			{
				// Token: 0x0400D9E8 RID: 55784
				public static LocString NAME = UI.FormatAsLink("Critter Ranching II", "RANCHING2");

				// Token: 0x0400D9E9 RID: 55785
				public static LocString DESCRIPTION = string.Concat(new string[]
				{
					"Improves a Duplicant's ",
					UI.PRE_KEYWORD,
					"Ranching",
					UI.PST_KEYWORD,
					" skills"
				});
			}

			// Token: 0x020037DF RID: 14303
			public class ASTRONAUTTRAINEE
			{
				// Token: 0x0400D9EA RID: 55786
				public static LocString NAME = UI.FormatAsLink("Rocket Piloting", "ASTRONAUTING1");

				// Token: 0x0400D9EB RID: 55787
				public static LocString DESCRIPTION = "Allows a Duplicant to operate a " + BUILDINGS.PREFABS.COMMANDMODULE.NAME + " to pilot a rocket ship";
			}

			// Token: 0x020037E0 RID: 14304
			public class ASTRONAUT
			{
				// Token: 0x0400D9EC RID: 55788
				public static LocString NAME = UI.FormatAsLink("Rocket Navigation", "ASTRONAUTING2");

				// Token: 0x0400D9ED RID: 55789
				public static LocString DESCRIPTION = "Improves the speed that space missions are completed";
			}

			// Token: 0x020037E1 RID: 14305
			public class ROCKETPILOT
			{
				// Token: 0x0400D9EE RID: 55790
				public static LocString NAME = UI.FormatAsLink("Rocket Piloting", "ROCKETPILOTING1");

				// Token: 0x0400D9EF RID: 55791
				public static LocString DESCRIPTION = "Allows a Duplicant to operate a " + BUILDINGS.PREFABS.ROCKETCONTROLSTATION.NAME + " and pilot rockets";
			}

			// Token: 0x020037E2 RID: 14306
			public class SENIOR_ROCKETPILOT
			{
				// Token: 0x0400D9F0 RID: 55792
				public static LocString NAME = UI.FormatAsLink("Rocket Piloting II", "ROCKETPILOTING2");

				// Token: 0x0400D9F1 RID: 55793
				public static LocString DESCRIPTION = "Allows Duplicants to pilot rockets at faster speeds";
			}

			// Token: 0x020037E3 RID: 14307
			public class USELESSSKILL
			{
				// Token: 0x0400D9F2 RID: 55794
				public static LocString NAME = "W.I.P. Skill";

				// Token: 0x0400D9F3 RID: 55795
				public static LocString DESCRIPTION = "This skill doesn't really do anything right now.";
			}
		}

		// Token: 0x020037E4 RID: 14308
		public class THOUGHTS
		{
			// Token: 0x020037E5 RID: 14309
			public class STARVING
			{
				// Token: 0x0400D9F4 RID: 55796
				public static LocString TOOLTIP = "Starving";
			}

			// Token: 0x020037E6 RID: 14310
			public class HOT
			{
				// Token: 0x0400D9F5 RID: 55797
				public static LocString TOOLTIP = "Hot";
			}

			// Token: 0x020037E7 RID: 14311
			public class COLD
			{
				// Token: 0x0400D9F6 RID: 55798
				public static LocString TOOLTIP = "Cold";
			}

			// Token: 0x020037E8 RID: 14312
			public class BREAKBLADDER
			{
				// Token: 0x0400D9F7 RID: 55799
				public static LocString TOOLTIP = "Washroom Break";
			}

			// Token: 0x020037E9 RID: 14313
			public class FULLBLADDER
			{
				// Token: 0x0400D9F8 RID: 55800
				public static LocString TOOLTIP = "Full Bladder";
			}

			// Token: 0x020037EA RID: 14314
			public class EXPELLGUNKDESIRE
			{
				// Token: 0x0400D9F9 RID: 55801
				public static LocString TOOLTIP = "Expel Gunk";
			}

			// Token: 0x020037EB RID: 14315
			public class EXPELLINGSPOILEDOIL
			{
				// Token: 0x0400D9FA RID: 55802
				public static LocString TOOLTIP = "Spilling Oil";
			}

			// Token: 0x020037EC RID: 14316
			public class HAPPY
			{
				// Token: 0x0400D9FB RID: 55803
				public static LocString TOOLTIP = "Happy";
			}

			// Token: 0x020037ED RID: 14317
			public class UNHAPPY
			{
				// Token: 0x0400D9FC RID: 55804
				public static LocString TOOLTIP = "Unhappy";
			}

			// Token: 0x020037EE RID: 14318
			public class POORDECOR
			{
				// Token: 0x0400D9FD RID: 55805
				public static LocString TOOLTIP = "Poor Decor";
			}

			// Token: 0x020037EF RID: 14319
			public class POOR_FOOD_QUALITY
			{
				// Token: 0x0400D9FE RID: 55806
				public static LocString TOOLTIP = "Lousy Meal";
			}

			// Token: 0x020037F0 RID: 14320
			public class GOOD_FOOD_QUALITY
			{
				// Token: 0x0400D9FF RID: 55807
				public static LocString TOOLTIP = "Delicious Meal";
			}

			// Token: 0x020037F1 RID: 14321
			public class SLEEPY
			{
				// Token: 0x0400DA00 RID: 55808
				public static LocString TOOLTIP = "Sleepy";
			}

			// Token: 0x020037F2 RID: 14322
			public class DREAMY
			{
				// Token: 0x0400DA01 RID: 55809
				public static LocString TOOLTIP = "Dreaming";
			}

			// Token: 0x020037F3 RID: 14323
			public class SUFFOCATING
			{
				// Token: 0x0400DA02 RID: 55810
				public static LocString TOOLTIP = "Suffocating";
			}

			// Token: 0x020037F4 RID: 14324
			public class ANGRY
			{
				// Token: 0x0400DA03 RID: 55811
				public static LocString TOOLTIP = "Angry";
			}

			// Token: 0x020037F5 RID: 14325
			public class RAGING
			{
				// Token: 0x0400DA04 RID: 55812
				public static LocString TOOLTIP = "Raging";
			}

			// Token: 0x020037F6 RID: 14326
			public class GOTINFECTED
			{
				// Token: 0x0400DA05 RID: 55813
				public static LocString TOOLTIP = "Got Infected";
			}

			// Token: 0x020037F7 RID: 14327
			public class PUTRIDODOUR
			{
				// Token: 0x0400DA06 RID: 55814
				public static LocString TOOLTIP = "Smelled Something Putrid";
			}

			// Token: 0x020037F8 RID: 14328
			public class NOISY
			{
				// Token: 0x0400DA07 RID: 55815
				public static LocString TOOLTIP = "Loud Area";
			}

			// Token: 0x020037F9 RID: 14329
			public class NEWROLE
			{
				// Token: 0x0400DA08 RID: 55816
				public static LocString TOOLTIP = "New Skill";
			}

			// Token: 0x020037FA RID: 14330
			public class CHATTY
			{
				// Token: 0x0400DA09 RID: 55817
				public static LocString TOOLTIP = "Greeting";
			}

			// Token: 0x020037FB RID: 14331
			public class ENCOURAGE
			{
				// Token: 0x0400DA0A RID: 55818
				public static LocString TOOLTIP = "Encouraging";
			}

			// Token: 0x020037FC RID: 14332
			public class CONVERSATION
			{
				// Token: 0x0400DA0B RID: 55819
				public static LocString TOOLTIP = "Chatting";
			}

			// Token: 0x020037FD RID: 14333
			public class CATCHYTUNE
			{
				// Token: 0x0400DA0C RID: 55820
				public static LocString TOOLTIP = "WHISTLING";
			}
		}
	}
}
