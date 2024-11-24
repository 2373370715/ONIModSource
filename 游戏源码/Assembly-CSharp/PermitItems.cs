using System.Collections.Generic;
using System.Linq;
using Database;
using STRINGS;

public class PermitItems
{
	private struct ItemInfo
	{
		public string ItemType;

		public uint TypeId;

		public string PermitId;

		public ItemInfo(string itemType, uint typeId, string permitId)
		{
			ItemType = itemType;
			PermitId = permitId;
			TypeId = typeId;
		}
	}

	private struct BoxInfo
	{
		public string ItemType;

		public string Name;

		public string Description;

		public uint TypeId;

		public string IconName;

		public bool AccountReward;

		public BoxInfo(string type, string name, string desc, uint id, string icon, bool account_reward)
		{
			ItemType = type;
			Name = name;
			Description = desc;
			TypeId = id;
			IconName = icon;
			AccountReward = account_reward;
		}
	}

	private static ItemInfo[] ItemInfos = new ItemInfo[534]
	{
		new ItemInfo("top_basic_black", 1u, "TopBasicBlack"),
		new ItemInfo("top_basic_white", 2u, "TopBasicWhite"),
		new ItemInfo("top_basic_red", 3u, "TopBasicRed"),
		new ItemInfo("top_basic_orange", 4u, "TopBasicOrange"),
		new ItemInfo("top_basic_yellow", 5u, "TopBasicYellow"),
		new ItemInfo("top_basic_green", 6u, "TopBasicGreen"),
		new ItemInfo("top_basic_blue_middle", 7u, "TopBasicAqua"),
		new ItemInfo("top_basic_purple", 8u, "TopBasicPurple"),
		new ItemInfo("top_basic_pink_orchid", 9u, "TopBasicPinkOrchid"),
		new ItemInfo("pants_basic_white", 11u, "BottomBasicWhite"),
		new ItemInfo("pants_basic_red", 12u, "BottomBasicRed"),
		new ItemInfo("pants_basic_orange", 13u, "BottomBasicOrange"),
		new ItemInfo("pants_basic_yellow", 14u, "BottomBasicYellow"),
		new ItemInfo("pants_basic_green", 15u, "BottomBasicGreen"),
		new ItemInfo("pants_basic_blue_middle", 16u, "BottomBasicAqua"),
		new ItemInfo("pants_basic_purple", 17u, "BottomBasicPurple"),
		new ItemInfo("pants_basic_pink_orchid", 18u, "BottomBasicPinkOrchid"),
		new ItemInfo("gloves_basic_black", 19u, "GlovesBasicBlack"),
		new ItemInfo("gloves_basic_white", 20u, "GlovesBasicWhite"),
		new ItemInfo("gloves_basic_red", 21u, "GlovesBasicRed"),
		new ItemInfo("gloves_basic_orange", 22u, "GlovesBasicOrange"),
		new ItemInfo("gloves_basic_yellow", 23u, "GlovesBasicYellow"),
		new ItemInfo("gloves_basic_green", 24u, "GlovesBasicGreen"),
		new ItemInfo("gloves_basic_blue_middle", 25u, "GlovesBasicAqua"),
		new ItemInfo("gloves_basic_purple", 26u, "GlovesBasicPurple"),
		new ItemInfo("gloves_basic_pink_orchid", 27u, "GlovesBasicPinkOrchid"),
		new ItemInfo("shoes_basic_white", 30u, "ShoesBasicWhite"),
		new ItemInfo("shoes_basic_red", 31u, "ShoesBasicRed"),
		new ItemInfo("shoes_basic_orange", 32u, "ShoesBasicOrange"),
		new ItemInfo("shoes_basic_yellow", 33u, "ShoesBasicYellow"),
		new ItemInfo("shoes_basic_green", 34u, "ShoesBasicGreen"),
		new ItemInfo("shoes_basic_blue_middle", 35u, "ShoesBasicAqua"),
		new ItemInfo("shoes_basic_purple", 36u, "ShoesBasicPurple"),
		new ItemInfo("shoes_basic_pink_orchid", 37u, "ShoesBasicPinkOrchid"),
		new ItemInfo("flowervase_retro", 39u, "FlowerVase_retro"),
		new ItemInfo("flowervase_retro_red", 40u, "FlowerVase_retro_red"),
		new ItemInfo("flowervase_retro_white", 41u, "FlowerVase_retro_white"),
		new ItemInfo("flowervase_retro_green", 42u, "FlowerVase_retro_green"),
		new ItemInfo("flowervase_retro_blue", 43u, "FlowerVase_retro_blue"),
		new ItemInfo("elegantbed_boat", 44u, "LuxuryBed_boat"),
		new ItemInfo("elegantbed_bouncy", 45u, "LuxuryBed_bouncy"),
		new ItemInfo("elegantbed_grandprix", 46u, "LuxuryBed_grandprix"),
		new ItemInfo("elegantbed_rocket", 47u, "LuxuryBed_rocket"),
		new ItemInfo("elegantbed_puft", 48u, "LuxuryBed_puft"),
		new ItemInfo("walls_pastel_pink", 49u, "ExteriorWall_pastel_pink"),
		new ItemInfo("walls_pastel_yellow", 50u, "ExteriorWall_pastel_yellow"),
		new ItemInfo("walls_pastel_green", 51u, "ExteriorWall_pastel_green"),
		new ItemInfo("walls_pastel_blue", 52u, "ExteriorWall_pastel_blue"),
		new ItemInfo("walls_pastel_purple", 53u, "ExteriorWall_pastel_purple"),
		new ItemInfo("walls_balm_lily", 54u, "ExteriorWall_balm_lily"),
		new ItemInfo("walls_clouds", 55u, "ExteriorWall_clouds"),
		new ItemInfo("walls_coffee", 56u, "ExteriorWall_coffee"),
		new ItemInfo("walls_mosaic", 57u, "ExteriorWall_mosaic"),
		new ItemInfo("walls_mushbar", 58u, "ExteriorWall_mushbar"),
		new ItemInfo("walls_plaid", 59u, "ExteriorWall_plaid"),
		new ItemInfo("walls_rain", 60u, "ExteriorWall_rain"),
		new ItemInfo("walls_rainbow", 61u, "ExteriorWall_rainbow"),
		new ItemInfo("walls_snow", 62u, "ExteriorWall_snow"),
		new ItemInfo("walls_sun", 63u, "ExteriorWall_sun"),
		new ItemInfo("walls_polka", 64u, "ExteriorWall_polka"),
		new ItemInfo("painting_art_i", 65u, "Canvas_Good7"),
		new ItemInfo("painting_art_j", 66u, "Canvas_Good8"),
		new ItemInfo("painting_art_k", 67u, "Canvas_Good9"),
		new ItemInfo("painting_tall_art_g", 68u, "CanvasTall_Good5"),
		new ItemInfo("painting_tall_art_h", 69u, "CanvasTall_Good6"),
		new ItemInfo("painting_tall_art_i", 70u, "CanvasTall_Good7"),
		new ItemInfo("painting_wide_art_g", 71u, "CanvasWide_Good5"),
		new ItemInfo("painting_wide_art_h", 72u, "CanvasWide_Good6"),
		new ItemInfo("painting_wide_art_i", 73u, "CanvasWide_Good7"),
		new ItemInfo("sculpture_amazing_4", 74u, "Sculpture_Good4"),
		new ItemInfo("sculpture_1x2_amazing_4", 75u, "SmallSculpture_Good4"),
		new ItemInfo("sculpture_metal_amazing_4", 76u, "MetalSculpture_Good4"),
		new ItemInfo("sculpture_marble_amazing_4", 77u, "MarbleSculpture_Good4"),
		new ItemInfo("sculpture_marble_amazing_5", 78u, "MarbleSculpture_Good5"),
		new ItemInfo("icesculpture_idle_2", 79u, "IceSculpture_Average2"),
		new ItemInfo("top_raglan_deep_red", 83u, "TopRaglanDeepRed"),
		new ItemInfo("top_raglan_cobalt", 84u, "TopRaglanCobalt"),
		new ItemInfo("top_raglan_flamingo", 85u, "TopRaglanFlamingo"),
		new ItemInfo("top_raglan_kelly_green", 86u, "TopRaglanKellyGreen"),
		new ItemInfo("top_raglan_charcoal", 87u, "TopRaglanCharcoal"),
		new ItemInfo("top_raglan_lemon", 88u, "TopRaglanLemon"),
		new ItemInfo("top_raglan_satsuma", 89u, "TopRaglanSatsuma"),
		new ItemInfo("shorts_basic_deep_red", 91u, "ShortsBasicDeepRed"),
		new ItemInfo("shorts_basic_satsuma", 92u, "ShortsBasicSatsuma"),
		new ItemInfo("shorts_basic_yellowcake", 93u, "ShortsBasicYellowcake"),
		new ItemInfo("shorts_basic_kelly_green", 94u, "ShortsBasicKellyGreen"),
		new ItemInfo("shorts_basic_blue_cobalt", 95u, "ShortsBasicBlueCobalt"),
		new ItemInfo("shorts_basic_pink_flamingo", 96u, "ShortsBasicPinkFlamingo"),
		new ItemInfo("shorts_basic_charcoal", 97u, "ShortsBasicCharcoal"),
		new ItemInfo("socks_athletic_deep_red", 98u, "SocksAthleticDeepRed"),
		new ItemInfo("socks_athletic_orange_satsuma", 99u, "SocksAthleticOrangeSatsuma"),
		new ItemInfo("socks_athletic_yellow_lemon", 100u, "SocksAthleticYellowLemon"),
		new ItemInfo("socks_athletic_green_kelly", 101u, "SocksAthleticGreenKelly"),
		new ItemInfo("socks_athletic_blue_cobalt", 102u, "SocksAthleticBlueCobalt"),
		new ItemInfo("socks_athletic_pink_flamingo", 103u, "SocksAthleticPinkFlamingo"),
		new ItemInfo("socks_athletic_grey_charcoal", 104u, "SocksAthleticGreyCharcoal"),
		new ItemInfo("gloves_athletic_red_deep", 105u, "GlovesAthleticRedDeep"),
		new ItemInfo("gloves_athletic_orange_satsuma", 106u, "GlovesAthleticOrangeSatsuma"),
		new ItemInfo("gloves_athletic_yellow_lemon", 107u, "GlovesAthleticYellowLemon"),
		new ItemInfo("gloves_athletic_green_kelly", 108u, "GlovesAthleticGreenKelly"),
		new ItemInfo("gloves_athletic_blue_cobalt", 109u, "GlovesAthleticBlueCobalt"),
		new ItemInfo("gloves_athletic_pink_flamingo", 110u, "GlovesAthleticPinkFlamingo"),
		new ItemInfo("gloves_athletic_grey_charcoal", 111u, "GlovesAthleticGreyCharcoal"),
		new ItemInfo("walls_diagonal_red_deep_white", 112u, "ExteriorWall_diagonal_red_deep_white"),
		new ItemInfo("walls_diagonal_orange_satsuma_white", 113u, "ExteriorWall_diagonal_orange_satsuma_white"),
		new ItemInfo("walls_diagonal_yellow_lemon_white", 114u, "ExteriorWall_diagonal_yellow_lemon_white"),
		new ItemInfo("walls_diagonal_green_kelly_white", 115u, "ExteriorWall_diagonal_green_kelly_white"),
		new ItemInfo("walls_diagonal_blue_cobalt_white", 116u, "ExteriorWall_diagonal_blue_cobalt_white"),
		new ItemInfo("walls_diagonal_pink_flamingo_white", 117u, "ExteriorWall_diagonal_pink_flamingo_white"),
		new ItemInfo("walls_diagonal_grey_charcoal_white", 118u, "ExteriorWall_diagonal_grey_charcoal_white"),
		new ItemInfo("walls_circle_red_deep_white", 119u, "ExteriorWall_circle_red_deep_white"),
		new ItemInfo("walls_circle_orange_satsuma_white", 120u, "ExteriorWall_circle_orange_satsuma_white"),
		new ItemInfo("walls_circle_yellow_lemon_white", 121u, "ExteriorWall_circle_yellow_lemon_white"),
		new ItemInfo("walls_circle_green_kelly_white", 122u, "ExteriorWall_circle_green_kelly_white"),
		new ItemInfo("walls_circle_blue_cobalt_white", 123u, "ExteriorWall_circle_blue_cobalt_white"),
		new ItemInfo("walls_circle_pink_flamingo_white", 124u, "ExteriorWall_circle_pink_flamingo_white"),
		new ItemInfo("walls_circle_grey_charcoal_white", 125u, "ExteriorWall_circle_grey_charcoal_white"),
		new ItemInfo("bed_star_curtain", 126u, "Bed_star_curtain"),
		new ItemInfo("bed_canopy", 127u, "Bed_canopy"),
		new ItemInfo("bed_rowan_tropical", 128u, "Bed_rowan_tropical"),
		new ItemInfo("bed_ada_science_lab", 129u, "Bed_ada_science_lab"),
		new ItemInfo("ceilinglight_mining", 130u, "CeilingLight_mining"),
		new ItemInfo("ceilinglight_flower", 131u, "CeilingLight_flower"),
		new ItemInfo("ceilinglight_polka_lamp_shade", 132u, "CeilingLight_polka_lamp_shade"),
		new ItemInfo("ceilinglight_burt_shower", 133u, "CeilingLight_burt_shower"),
		new ItemInfo("ceilinglight_ada_flask_round", 134u, "CeilingLight_ada_flask_round"),
		new ItemInfo("balloon_red_fireengine_long_sparkles_kanim", 135u, "BalloonRedFireEngineLongSparkles"),
		new ItemInfo("balloon_yellow_long_sparkles_kanim", 136u, "BalloonYellowLongSparkles"),
		new ItemInfo("balloon_blue_long_sparkles_kanim", 137u, "BalloonBlueLongSparkles"),
		new ItemInfo("balloon_green_long_sparkles_kanim", 138u, "BalloonGreenLongSparkles"),
		new ItemInfo("balloon_pink_long_sparkles_kanim", 139u, "BalloonPinkLongSparkles"),
		new ItemInfo("balloon_purple_long_sparkles_kanim", 140u, "BalloonPurpleLongSparkles"),
		new ItemInfo("balloon_babypacu_egg_kanim", 141u, "BalloonBabyPacuEgg"),
		new ItemInfo("balloon_babyglossydrecko_egg_kanim", 142u, "BalloonBabyGlossyDreckoEgg"),
		new ItemInfo("balloon_babyhatch_egg_kanim", 143u, "BalloonBabyHatchEgg"),
		new ItemInfo("balloon_babypokeshell_egg_kanim", 144u, "BalloonBabyPokeshellEgg"),
		new ItemInfo("balloon_babypuft_egg_kanim", 145u, "BalloonBabyPuftEgg"),
		new ItemInfo("balloon_babyshovole_egg_kanim", 146u, "BalloonBabyShovoleEgg"),
		new ItemInfo("balloon_babypip_egg_kanim", 147u, "BalloonBabyPipEgg"),
		new ItemInfo("top_jellypuffjacket_blueberry", 150u, "TopJellypuffJacketBlueberry"),
		new ItemInfo("top_jellypuffjacket_grape", 151u, "TopJellypuffJacketGrape"),
		new ItemInfo("top_jellypuffjacket_lemon", 152u, "TopJellypuffJacketLemon"),
		new ItemInfo("top_jellypuffjacket_lime", 153u, "TopJellypuffJacketLime"),
		new ItemInfo("top_jellypuffjacket_satsuma", 154u, "TopJellypuffJacketSatsuma"),
		new ItemInfo("top_jellypuffjacket_strawberry", 155u, "TopJellypuffJacketStrawberry"),
		new ItemInfo("top_jellypuffjacket_watermelon", 156u, "TopJellypuffJacketWatermelon"),
		new ItemInfo("gloves_cuffless_blueberry", 157u, "GlovesCufflessBlueberry"),
		new ItemInfo("gloves_cuffless_grape", 158u, "GlovesCufflessGrape"),
		new ItemInfo("gloves_cuffless_lemon", 159u, "GlovesCufflessLemon"),
		new ItemInfo("gloves_cuffless_lime", 160u, "GlovesCufflessLime"),
		new ItemInfo("gloves_cuffless_satsuma", 161u, "GlovesCufflessSatsuma"),
		new ItemInfo("gloves_cuffless_strawberry", 162u, "GlovesCufflessStrawberry"),
		new ItemInfo("gloves_cuffless_watermelon", 163u, "GlovesCufflessWatermelon"),
		new ItemInfo("flowervase_wall_retro_blue", 164u, "FlowerVaseWall_retro_green"),
		new ItemInfo("flowervase_wall_retro_green", 165u, "FlowerVaseWall_retro_yellow"),
		new ItemInfo("flowervase_wall_retro_red", 166u, "FlowerVaseWall_retro_red"),
		new ItemInfo("flowervase_wall_retro_white", 167u, "FlowerVaseWall_retro_blue"),
		new ItemInfo("flowervase_wall_retro_yellow", 168u, "FlowerVaseWall_retro_white"),
		new ItemInfo("walls_basic_blue_cobalt", 169u, "ExteriorWall_basic_blue_cobalt"),
		new ItemInfo("walls_basic_green_kelly", 170u, "ExteriorWall_basic_green_kelly"),
		new ItemInfo("walls_basic_grey_charcoal", 171u, "ExteriorWall_basic_grey_charcoal"),
		new ItemInfo("walls_basic_orange_satsuma", 172u, "ExteriorWall_basic_orange_satsuma"),
		new ItemInfo("walls_basic_pink_flamingo", 173u, "ExteriorWall_basic_pink_flamingo"),
		new ItemInfo("walls_basic_red_deep", 174u, "ExteriorWall_basic_red_deep"),
		new ItemInfo("walls_basic_yellow_lemon", 175u, "ExteriorWall_basic_yellow_lemon"),
		new ItemInfo("walls_blueberries", 176u, "ExteriorWall_blueberries"),
		new ItemInfo("walls_grapes", 177u, "ExteriorWall_grapes"),
		new ItemInfo("walls_lemon", 178u, "ExteriorWall_lemon"),
		new ItemInfo("walls_lime", 179u, "ExteriorWall_lime"),
		new ItemInfo("walls_satsuma", 180u, "ExteriorWall_satsuma"),
		new ItemInfo("walls_strawberry", 181u, "ExteriorWall_strawberry"),
		new ItemInfo("walls_watermelon", 182u, "ExteriorWall_watermelon"),
		new ItemInfo("balloon_candy_blueberry", 183u, "BalloonCandyBlueberry"),
		new ItemInfo("balloon_candy_grape", 184u, "BalloonCandyGrape"),
		new ItemInfo("balloon_candy_lemon", 185u, "BalloonCandyLemon"),
		new ItemInfo("balloon_candy_lime", 186u, "BalloonCandyLime"),
		new ItemInfo("balloon_candy_orange", 187u, "BalloonCandyOrange"),
		new ItemInfo("balloon_candy_strawberry", 188u, "BalloonCandyStrawberry"),
		new ItemInfo("balloon_candy_watermelon", 189u, "BalloonCandyWatermelon"),
		new ItemInfo("atmo_helmet_puft", 191u, "AtmoHelmetPuft"),
		new ItemInfo("atmo_suit_puft", 192u, "AtmoSuitPuft"),
		new ItemInfo("atmo_gloves_puft", 193u, "AtmoGlovesPuft"),
		new ItemInfo("atmo_belt_puft", 194u, "AtmoBeltPuft"),
		new ItemInfo("atmo_shoes_puft", 195u, "AtmoShoesPuft"),
		new ItemInfo("top_tshirt_white", 197u, "TopTShirtWhite"),
		new ItemInfo("top_tshirt_magenta", 198u, "TopTShirtMagenta"),
		new ItemInfo("top_athlete", 199u, "TopAthlete"),
		new ItemInfo("top_circuit_green", 200u, "TopCircuitGreen"),
		new ItemInfo("gloves_basic_bluegrey", 201u, "GlovesBasicBlueGrey"),
		new ItemInfo("gloves_basic_brown_khaki", 202u, "GlovesBasicBrownKhaki"),
		new ItemInfo("gloves_athlete", 203u, "GlovesAthlete"),
		new ItemInfo("gloves_circuit_green", 204u, "GlovesCircuitGreen"),
		new ItemInfo("pants_basic_redorange", 205u, "PantsBasicRedOrange"),
		new ItemInfo("pants_basic_lightbrown", 206u, "PantsBasicLightBrown"),
		new ItemInfo("pants_athlete", 207u, "PantsAthlete"),
		new ItemInfo("pants_circuit_green", 208u, "PantsCircuitGreen"),
		new ItemInfo("shoes_basic_bluegrey", 209u, "ShoesBasicBlueGrey"),
		new ItemInfo("shoes_basic_tan", 210u, "ShoesBasicTan"),
		new ItemInfo("atmo_helmet_sparkle_red", 211u, "AtmoHelmetSparkleRed"),
		new ItemInfo("atmo_helmet_sparkle_green", 212u, "AtmoHelmetSparkleGreen"),
		new ItemInfo("atmo_helmet_sparkle_blue", 213u, "AtmoHelmetSparkleBlue"),
		new ItemInfo("atmo_helmet_sparkle_purple", 214u, "AtmoHelmetSparklePurple"),
		new ItemInfo("atmosuit_sparkle_red", 215u, "AtmoSuitSparkleRed"),
		new ItemInfo("atmosuit_sparkle_green", 216u, "AtmoSuitSparkleGreen"),
		new ItemInfo("atmosuit_sparkle_blue", 217u, "AtmoSuitSparkleBlue"),
		new ItemInfo("atmosuit_sparkle_lavender", 218u, "AtmoSuitSparkleLavender"),
		new ItemInfo("atmo_gloves_sparkle_red", 219u, "AtmoGlovesSparkleRed"),
		new ItemInfo("atmo_gloves_sparkle_green", 220u, "AtmoGlovesSparkleGreen"),
		new ItemInfo("atmo_gloves_sparkle_blue", 221u, "AtmoGlovesSparkleBlue"),
		new ItemInfo("atmo_gloves_sparkle_lavender", 222u, "AtmoGlovesSparkleLavender"),
		new ItemInfo("atmo_belt_sparkle_red", 223u, "AtmoBeltSparkleRed"),
		new ItemInfo("atmo_belt_sparkle_green", 224u, "AtmoBeltSparkleGreen"),
		new ItemInfo("atmo_belt_sparkle_blue", 225u, "AtmoBeltSparkleBlue"),
		new ItemInfo("atmo_belt_sparkle_lavender", 226u, "AtmoBeltSparkleLavender"),
		new ItemInfo("atmo_shoes_sparkle_black", 227u, "AtmoShoesSparkleBlack"),
		new ItemInfo("flowervase_hanging_retro_red", 228u, "FlowerVaseHanging_retro_red"),
		new ItemInfo("flowervase_hanging_retro_green", 229u, "FlowerVaseHanging_retro_green"),
		new ItemInfo("flowervase_hanging_retro_blue", 230u, "FlowerVaseHanging_retro_blue"),
		new ItemInfo("flowervase_hanging_retro_yellow", 231u, "FlowerVaseHanging_retro_yellow"),
		new ItemInfo("flowervase_hanging_retro_white", 232u, "FlowerVaseHanging_retro_white"),
		new ItemInfo("walls_toiletpaper", 233u, "ExteriorWall_toiletpaper"),
		new ItemInfo("walls_plunger", 234u, "ExteriorWall_plunger"),
		new ItemInfo("walls_tropical", 235u, "ExteriorWall_tropical"),
		new ItemInfo("painting_art_l", 236u, "Canvas_Good10"),
		new ItemInfo("painting_art_m", 237u, "Canvas_Good11"),
		new ItemInfo("painting_tall_art_j", 238u, "CanvasTall_Good8"),
		new ItemInfo("painting_tall_art_k", 239u, "CanvasTall_Good9"),
		new ItemInfo("painting_wide_art_j", 240u, "CanvasWide_Good8"),
		new ItemInfo("painting_wide_art_k", 241u, "CanvasWide_Good9"),
		new ItemInfo("top_denim_blue", 242u, "TopDenimBlue"),
		new ItemInfo("top_undershirt_executive", 243u, "TopUndershirtExecutive"),
		new ItemInfo("top_undershirt_underling", 244u, "TopUndershirtUnderling"),
		new ItemInfo("top_undershirt_groupthink", 245u, "TopUndershirtGroupthink"),
		new ItemInfo("top_undershirt_stakeholder", 246u, "TopUndershirtStakeholder"),
		new ItemInfo("top_undershirt_admin", 247u, "TopUndershirtAdmin"),
		new ItemInfo("top_undershirt_buzzword", 248u, "TopUndershirtBuzzword"),
		new ItemInfo("top_undershirtshirt_synergy", 249u, "TopUndershirtSynergy"),
		new ItemInfo("top_researcher", 250u, "TopResearcher"),
		new ItemInfo("top_rebel_gi", 251u, "TopRebelGi"),
		new ItemInfo("bottom_briefs_executive", 252u, "BottomBriefsExecutive"),
		new ItemInfo("bottom_briefs_underling", 253u, "BottomBriefsUnderling"),
		new ItemInfo("bottom_briefs_groupthink", 254u, "BottomBriefsGroupthink"),
		new ItemInfo("bottom_briefs_stakeholder", 255u, "BottomBriefsStakeholder"),
		new ItemInfo("bottom_briefs_admin", 256u, "BottomBriefsAdmin"),
		new ItemInfo("bottom_briefs_buzzword", 257u, "BottomBriefsBuzzword"),
		new ItemInfo("bottom_briefs_synergy", 258u, "BottomBriefsSynergy"),
		new ItemInfo("pants_jeans", 259u, "PantsJeans"),
		new ItemInfo("pants_rebel_gi", 260u, "PantsRebelGi"),
		new ItemInfo("pants_research", 261u, "PantsResearch"),
		new ItemInfo("shoes_basic_gray", 262u, "ShoesBasicGray"),
		new ItemInfo("shoes_denim_blue", 263u, "ShoesDenimBlue"),
		new ItemInfo("socks_legwarmers_blueberry", 264u, "SocksLegwarmersBlueberry"),
		new ItemInfo("socks_legwarmers_grape", 265u, "SocksLegwarmersGrape"),
		new ItemInfo("socks_legwarmers_lemon", 266u, "SocksLegwarmersLemon"),
		new ItemInfo("socks_legwarmers_lime", 267u, "SocksLegwarmersLime"),
		new ItemInfo("socks_legwarmers_satsuma", 268u, "SocksLegwarmersSatsuma"),
		new ItemInfo("socks_legwarmers_strawberry", 269u, "SocksLegwarmersStrawberry"),
		new ItemInfo("socks_legwarmers_watermelon", 270u, "SocksLegwarmersWatermelon"),
		new ItemInfo("gloves_cuffless_black", 271u, "GlovesCufflessBlack"),
		new ItemInfo("gloves_denim_blue", 272u, "GlovesDenimBlue"),
		new ItemInfo("atmo_gloves_gold", 273u, "AtmoGlovesGold"),
		new ItemInfo("atmo_gloves_eggplant", 274u, "AtmoGlovesEggplant"),
		new ItemInfo("atmo_helmet_eggplant", 275u, "AtmoHelmetEggplant"),
		new ItemInfo("atmo_helmet_confetti", 276u, "AtmoHelmetConfetti"),
		new ItemInfo("atmo_shoes_stealth", 277u, "AtmoShoesStealth"),
		new ItemInfo("atmo_shoes_eggplant", 278u, "AtmoShoesEggplant"),
		new ItemInfo("atmosuit_crisp_eggplant", 279u, "AtmoSuitCrispEggplant"),
		new ItemInfo("atmosuit_confetti", 280u, "AtmoSuitConfetti"),
		new ItemInfo("atmo_belt_basic_gold", 281u, "AtmoBeltBasicGold"),
		new ItemInfo("atmo_belt_eggplant", 282u, "AtmoBeltEggplant"),
		new ItemInfo("item_pedestal_hand", 283u, "ItemPedestal_hand"),
		new ItemInfo("massage_table_shiatsu", 284u, "MassageTable_shiatsu"),
		new ItemInfo("rock_crusher_hands", 285u, "RockCrusher_hands"),
		new ItemInfo("rock_crusher_teeth", 286u, "RockCrusher_teeth"),
		new ItemInfo("water_cooler_round_body", 287u, "WaterCooler_round_body"),
		new ItemInfo("walls_stripes_blue", 288u, "ExteriorWall_stripes_blue"),
		new ItemInfo("walls_stripes_diagonal_blue", 289u, "ExteriorWall_stripes_diagonal_blue"),
		new ItemInfo("walls_stripes_circle_blue", 290u, "ExteriorWall_stripes_circle_blue"),
		new ItemInfo("walls_squares_red_deep_white", 291u, "ExteriorWall_squares_red_deep_white"),
		new ItemInfo("walls_squares_orange_satsuma_white", 292u, "ExteriorWall_squares_orange_satsuma_white"),
		new ItemInfo("walls_squares_yellow_lemon_white", 293u, "ExteriorWall_squares_yellow_lemon_white"),
		new ItemInfo("walls_squares_green_kelly_white", 294u, "ExteriorWall_squares_green_kelly_white"),
		new ItemInfo("walls_squares_blue_cobalt_white", 295u, "ExteriorWall_squares_blue_cobalt_white"),
		new ItemInfo("walls_squares_pink_flamingo_white", 296u, "ExteriorWall_squares_pink_flamingo_white"),
		new ItemInfo("walls_squares_grey_charcoal_white", 297u, "ExteriorWall_squares_grey_charcoal_white"),
		new ItemInfo("canvas_good_13", 298u, "Canvas_Good13"),
		new ItemInfo("canvas_wide_good_10", 299u, "CanvasWide_Good10"),
		new ItemInfo("canvas_tall_good_11", 300u, "CanvasTall_Good11"),
		new ItemInfo("sculpture_good_5", 301u, "Sculpture_Good5"),
		new ItemInfo("small_sculpture_good_5", 302u, "SmallSculpture_Good5"),
		new ItemInfo("small_sculpture_good_6", 303u, "SmallSculpture_Good6"),
		new ItemInfo("metal_sculpture_good_5", 304u, "MetalSculpture_Good5"),
		new ItemInfo("ice_sculpture_average_3", 305u, "IceSculpture_Average3"),
		new ItemInfo("skirt_basic_blue_middle", 306u, "SkirtBasicBlueMiddle"),
		new ItemInfo("skirt_basic_purple", 307u, "SkirtBasicPurple"),
		new ItemInfo("skirt_basic_green", 308u, "SkirtBasicGreen"),
		new ItemInfo("skirt_basic_orange", 309u, "SkirtBasicOrange"),
		new ItemInfo("skirt_basic_pink_orchid", 310u, "SkirtBasicPinkOrchid"),
		new ItemInfo("skirt_basic_red", 311u, "SkirtBasicRed"),
		new ItemInfo("skirt_basic_yellow", 312u, "SkirtBasicYellow"),
		new ItemInfo("skirt_basic_polkadot", 313u, "SkirtBasicPolkadot"),
		new ItemInfo("skirt_basic_watermelon", 314u, "SkirtBasicWatermelon"),
		new ItemInfo("skirt_denim_blue", 315u, "SkirtDenimBlue"),
		new ItemInfo("skirt_leopard_print_blue_pink", 316u, "SkirtLeopardPrintBluePink"),
		new ItemInfo("skirt_sparkle_blue", 317u, "SkirtSparkleBlue"),
		new ItemInfo("atmo_belt_basic_grey", 318u, "AtmoBeltBasicGrey"),
		new ItemInfo("atmo_belt_basic_neon_pink", 319u, "AtmoBeltBasicNeonPink"),
		new ItemInfo("atmo_gloves_white", 320u, "AtmoGlovesWhite"),
		new ItemInfo("atmo_gloves_stripes_lavender", 321u, "AtmoGlovesStripesLavender"),
		new ItemInfo("atmo_helmet_cummerbund_red", 322u, "AtmoHelmetCummerbundRed"),
		new ItemInfo("atmo_helmet_workout_lavender", 323u, "AtmoHelmetWorkoutLavender"),
		new ItemInfo("atmo_shoes_basic_lavender", 324u, "AtmoShoesBasicLavender"),
		new ItemInfo("atmosuit_basic_neon_pink", 325u, "AtmoSuitBasicNeonPink"),
		new ItemInfo("atmosuit_multi_red_black", 326u, "AtmoSuitMultiRedBlack"),
		new ItemInfo("egg_cracker_beaker", 327u, "EggCracker_beaker"),
		new ItemInfo("egg_cracker_flower", 328u, "EggCracker_flower"),
		new ItemInfo("egg_cracker_hands", 329u, "EggCracker_hands"),
		new ItemInfo("ceilinglight_rubiks", 330u, "CeilingLight_rubiks"),
		new ItemInfo("flowervase_hanging_beaker", 331u, "FlowerVaseHanging_beaker"),
		new ItemInfo("flowervase_hanging_rubiks", 332u, "FlowerVaseHanging_rubiks"),
		new ItemInfo("elegantbed_hand", 333u, "LuxuryBed_hand"),
		new ItemInfo("elegantbed_rubiks", 334u, "LuxuryBed_rubiks"),
		new ItemInfo("rock_crusher_roundstamp", 335u, "RockCrusher_roundstamp"),
		new ItemInfo("rock_crusher_spikebeds", 336u, "RockCrusher_spikebeds"),
		new ItemInfo("storagelocker_green_mush", 337u, "StorageLocker_green_mush"),
		new ItemInfo("storagelocker_red_rose", 338u, "StorageLocker_red_rose"),
		new ItemInfo("storagelocker_blue_babytears", 339u, "StorageLocker_blue_babytears"),
		new ItemInfo("storagelocker_purple_brainfat", 340u, "StorageLocker_purple_brainfat"),
		new ItemInfo("storagelocker_yellow_tartar", 341u, "StorageLocker_yellow_tartar"),
		new ItemInfo("planterbox_mealwood", 342u, "PlanterBox_mealwood"),
		new ItemInfo("planterbox_bristleblossom", 343u, "PlanterBox_bristleblossom"),
		new ItemInfo("planterbox_wheezewort", 344u, "PlanterBox_wheezewort"),
		new ItemInfo("planterbox_sleetwheat", 345u, "PlanterBox_sleetwheat"),
		new ItemInfo("planterbox_salmon_pink", 346u, "PlanterBox_salmon_pink"),
		new ItemInfo("gasstorage_lightgold", 347u, "GasReservoir_lightgold"),
		new ItemInfo("gasstorage_peagreen", 348u, "GasReservoir_peagreen"),
		new ItemInfo("gasstorage_lightcobalt", 349u, "GasReservoir_lightcobalt"),
		new ItemInfo("gasstorage_polka_darkpurpleresin", 350u, "GasReservoir_polka_darkpurpleresin"),
		new ItemInfo("gasstorage_polka_darknavynookgreen", 351u, "GasReservoir_polka_darknavynookgreen"),
		new ItemInfo("walls_kitchen_retro1", 352u, "ExteriorWall_kitchen_retro1"),
		new ItemInfo("walls_plus_red_deep_white", 353u, "ExteriorWall_plus_red_deep_white"),
		new ItemInfo("walls_plus_orange_satsuma_white", 354u, "ExteriorWall_plus_orange_satsuma_white"),
		new ItemInfo("walls_plus_yellow_lemon_white", 355u, "ExteriorWall_plus_yellow_lemon_white"),
		new ItemInfo("walls_plus_green_kelly_white", 356u, "ExteriorWall_plus_green_kelly_white"),
		new ItemInfo("walls_plus_blue_cobalt_white", 357u, "ExteriorWall_plus_blue_cobalt_white"),
		new ItemInfo("walls_plus_pink_flamingo_white", 358u, "ExteriorWall_plus_pink_flamingo_white"),
		new ItemInfo("walls_plus_grey_charcoal_white", 359u, "ExteriorWall_plus_grey_charcoal_white"),
		new ItemInfo("painting_art_n", 360u, "Canvas_Good12"),
		new ItemInfo("painting_art_p", 361u, "Canvas_Good14"),
		new ItemInfo("painting_wide_art_m", 362u, "CanvasWide_Good11"),
		new ItemInfo("painting_tall_art_l", 363u, "CanvasTall_Good10"),
		new ItemInfo("sculpture_amazing_6", 364u, "Sculpture_Good6"),
		new ItemInfo("balloon_hand_gold", 365u, "BalloonHandGold"),
		new ItemInfo("atmo_belt_cantaloupe", 367u, "AtmoBeltRocketmelon"),
		new ItemInfo("atmosuit_cantaloupe", 368u, "AtmoSuitRocketmelon"),
		new ItemInfo("atmo_gloves_cantaloupe", 369u, "AtmoGlovesRocketmelon"),
		new ItemInfo("atmo_helmet_cantaloupe", 370u, "AtmoHelmetRocketmelon"),
		new ItemInfo("atmo_shoes_cantaloupe", 371u, "AtmoBootsRocketmelon"),
		new ItemInfo("pants_basic_orange_satsuma", 372u, "PantsBasicOrangeSatsuma"),
		new ItemInfo("pants_pinstripe_slate", 373u, "PantsPinstripeSlate"),
		new ItemInfo("pants_velour_black", 374u, "PantsVelourBlack"),
		new ItemInfo("pants_velour_blue", 375u, "PantsVelourBlue"),
		new ItemInfo("pants_velour_pink", 376u, "PantsVelourPink"),
		new ItemInfo("skirt_ballerina_pink", 377u, "SkirtBallerinaPink"),
		new ItemInfo("skirt_tweed_pink_orchid", 378u, "SkirtTweedPinkOrchid"),
		new ItemInfo("shoes_ballerina_pink", 379u, "ShoesBallerinaPink"),
		new ItemInfo("shoes_maryjane_socks_bw", 380u, "ShoesMaryjaneSocksBw"),
		new ItemInfo("shoes_classicflats_cream_charcoal", 381u, "ShoesClassicFlatsCreamCharcoal"),
		new ItemInfo("shoes_velour_blue", 382u, "ShoesVelourBlue"),
		new ItemInfo("shoes_velour_pink", 383u, "ShoesVelourPink"),
		new ItemInfo("shoes_velour_black", 384u, "ShoesVelourBlack"),
		new ItemInfo("gloves_basic_grey", 385u, "GlovesBasicGrey"),
		new ItemInfo("gloves_basic_pinksalmon", 386u, "GlovesBasicPinksalmon"),
		new ItemInfo("gloves_basic_tan", 387u, "GlovesBasicTan"),
		new ItemInfo("gloves_ballerina_pink", 388u, "GlovesBallerinaPink"),
		new ItemInfo("gloves_formal_white", 389u, "GlovesFormalWhite"),
		new ItemInfo("gloves_long_white", 390u, "GlovesLongWhite"),
		new ItemInfo("gloves_2tone_cream_charcoal", 391u, "Gloves2ToneCreamCharcoal"),
		new ItemInfo("top_jacket_smoking_burgundy", 392u, "TopJacketSmokingBurgundy"),
		new ItemInfo("top_mechanic", 393u, "TopMechanic"),
		new ItemInfo("top_velour_black", 394u, "TopVelourBlack"),
		new ItemInfo("top_velour_blue", 395u, "TopVelourBlue"),
		new ItemInfo("top_velour_pink", 396u, "TopVelourPink"),
		new ItemInfo("top_waistcoat_pinstripe_slate", 397u, "TopWaistcoatPinstripeSlate"),
		new ItemInfo("top_water", 398u, "TopWater"),
		new ItemInfo("top_tweed_pink_orchid", 399u, "TopTweedPinkOrchid"),
		new ItemInfo("dress_sleeveless_bow_bw", 400u, "DressSleevelessBowBw"),
		new ItemInfo("bodysuit_ballerina_pink", 401u, "BodysuitBallerinaPink"),
		new ItemInfo("rock_crusher_chomp", 402u, "RockCrusher_chomp"),
		new ItemInfo("rock_crusher_gears", 403u, "RockCrusher_gears"),
		new ItemInfo("rock_crusher_balloon", 404u, "RockCrusher_balloon"),
		new ItemInfo("storagelocker_polka_darknavynookgreen", 405u, "StorageLocker_polka_darknavynookgreen"),
		new ItemInfo("storagelocker_polka_darkpurpleresin", 406u, "StorageLocker_polka_darkpurpleresin"),
		new ItemInfo("gasstorage_blue_babytears", 407u, "GasReservoir_blue_babytears"),
		new ItemInfo("gasstorage_yellow_tartar", 408u, "GasReservoir_yellow_tartar"),
		new ItemInfo("gasstorage_green_mush", 409u, "GasReservoir_green_mush"),
		new ItemInfo("gasstorage_red_rose", 410u, "GasReservoir_red_rose"),
		new ItemInfo("gasstorage_purple_brainfat", 411u, "GasReservoir_purple_brainfat"),
		new ItemInfo("masseur_balloon", 412u, "MassageTable_balloon"),
		new ItemInfo("watercooler_balloon", 413u, "WaterCooler_balloon"),
		new ItemInfo("top_x_sporchid", 415u, "TopXSporchid"),
		new ItemInfo("top_x1_pinchapeppernutbells", 416u, "TopX1Pinchapeppernutbells"),
		new ItemInfo("top_pompom_shinebugs_pink_peppernut", 417u, "TopPompomShinebugsPinkPeppernut"),
		new ItemInfo("top_snowflake_blue", 418u, "TopSnowflakeBlue"),
		new ItemInfo("bed_stringlights", 419u, "Bed_stringlights"),
		new ItemInfo("corner_tile_shineornaments", 420u, "CornerMoulding_shineornaments"),
		new ItemInfo("crown_moulding_shineornaments", 421u, "CrownMoulding_shineornaments"),
		new ItemInfo("floorlamp_leg", 422u, "FloorLamp_leg"),
		new ItemInfo("floorlamp_bristle_blossom", 423u, "FloorLamp_bristle_blossom"),
		new ItemInfo("storagelocker_stripes_red_white", 424u, "StorageLocker_stripes_red_white"),
		new ItemInfo("fridge_stripes_red_white", 425u, "Refrigerator_stripes_red_white"),
		new ItemInfo("walls_stripes_rose", 426u, "ExteriorWall_stripes_rose"),
		new ItemInfo("walls_stripes_diagonal_rose", 427u, "ExteriorWall_stripes_diagonal_rose"),
		new ItemInfo("walls_stripes_circle_rose", 428u, "ExteriorWall_stripes_circle_rose"),
		new ItemInfo("walls_stripes_mush", 429u, "ExteriorWall_stripes_mush"),
		new ItemInfo("walls_stripes_diagonal_mush", 430u, "ExteriorWall_stripes_diagonal_mush"),
		new ItemInfo("walls_stripes_circle_mush", 431u, "ExteriorWall_stripes_circle_mush"),
		new ItemInfo("painting_art_q", 432u, "Canvas_Good15"),
		new ItemInfo("painting_tall_art_p", 433u, "CanvasTall_Good14"),
		new ItemInfo("atmo_belt_2tone_brown", 434u, "AtmoBeltTwoToneBrown"),
		new ItemInfo("atmosuit_multi_blue_grey_black", 435u, "AtmoSuitMultiBlueGreyBlack"),
		new ItemInfo("atmosuit_multi_blue_yellow_red", 436u, "AtmoSuitMultiBlueYellowRed"),
		new ItemInfo("atmo_gloves_brown", 437u, "AtmoGlovesBrown"),
		new ItemInfo("atmo_helmet_mondrian_blue_red_yellow", 438u, "AtmoHelmetMondrianBlueRedYellow"),
		new ItemInfo("atmo_helmet_overalls_red", 439u, "AtmoHelmetOverallsRed"),
		new ItemInfo("pj_clovers_glitch_kelly", 440u, "PjCloversGlitchKelly"),
		new ItemInfo("pj_hearts_chilli_strawberry", 441u, "PjHeartsChilliStrawberry"),
		new ItemInfo("bottom_ginch_pink_gluon", 442u, "BottomGinchPinkGluon"),
		new ItemInfo("bottom_ginch_purple_cortex", 443u, "BottomGinchPurpleCortex"),
		new ItemInfo("bottom_ginch_blue_frosty", 444u, "BottomGinchBlueFrosty"),
		new ItemInfo("bottom_ginch_teal_locus", 445u, "BottomGinchTealLocus"),
		new ItemInfo("bottom_ginch_green_goop", 446u, "BottomGinchGreenGoop"),
		new ItemInfo("bottom_ginch_yellow_bile", 447u, "BottomGinchYellowBile"),
		new ItemInfo("bottom_ginch_orange_nybble", 448u, "BottomGinchOrangeNybble"),
		new ItemInfo("bottom_ginch_red_ironbow", 449u, "BottomGinchRedIronbow"),
		new ItemInfo("bottom_ginch_grey_phlegm", 450u, "BottomGinchGreyPhlegm"),
		new ItemInfo("bottom_ginch_grey_obelus", 451u, "BottomGinchGreyObelus"),
		new ItemInfo("pants_knit_polkadot_turq", 452u, "PantsKnitPolkadotTurq"),
		new ItemInfo("pants_gi_belt_white_black", 453u, "PantsGiBeltWhiteBlack"),
		new ItemInfo("pants_belt_khaki_tan", 454u, "PantsBeltKhakiTan"),
		new ItemInfo("shoes_flashy", 455u, "ShoesFlashy"),
		new ItemInfo("socks_ginch_pink_saltrock", 456u, "SocksGinchPinkSaltrock"),
		new ItemInfo("socks_ginch_purple_dusky", 457u, "SocksGinchPurpleDusky"),
		new ItemInfo("socks_ginch_blue_basin", 458u, "SocksGinchBlueBasin"),
		new ItemInfo("socks_ginch_teal_balmy", 459u, "SocksGinchTealBalmy"),
		new ItemInfo("socks_ginch_green_lime", 460u, "SocksGinchGreenLime"),
		new ItemInfo("socks_ginch_yellow_yellowcake", 461u, "SocksGinchYellowYellowcake"),
		new ItemInfo("socks_ginch_orange_atomic", 462u, "SocksGinchOrangeAtomic"),
		new ItemInfo("socks_ginch_red_magma", 463u, "SocksGinchRedMagma"),
		new ItemInfo("socks_ginch_grey_grey", 464u, "SocksGinchGreyGrey"),
		new ItemInfo("socks_ginch_grey_charcoal", 465u, "SocksGinchGreyCharcoal"),
		new ItemInfo("gloves_basic_slate", 466u, "GlovesBasicSlate"),
		new ItemInfo("gloves_knit_gold", 467u, "GlovesKnitGold"),
		new ItemInfo("gloves_knit_magenta", 468u, "GlovesKnitMagenta"),
		new ItemInfo("gloves_sparkle_white", 469u, "GlovesSparkleWhite"),
		new ItemInfo("gloves_ginch_pink_saltrock", 470u, "GlovesGinchPinkSaltrock"),
		new ItemInfo("gloves_ginch_purple_dusky", 471u, "GlovesGinchPurpleDusky"),
		new ItemInfo("gloves_ginch_blue_basin", 472u, "GlovesGinchBlueBasin"),
		new ItemInfo("gloves_ginch_teal_balmy", 473u, "GlovesGinchTealBalmy"),
		new ItemInfo("gloves_ginch_green_lime", 474u, "GlovesGinchGreenLime"),
		new ItemInfo("gloves_ginch_yellow_yellowcake", 475u, "GlovesGinchYellowYellowcake"),
		new ItemInfo("gloves_ginch_orange_atomic", 476u, "GlovesGinchOrangeAtomic"),
		new ItemInfo("gloves_ginch_red_magma", 477u, "GlovesGinchRedMagma"),
		new ItemInfo("gloves_ginch_grey_grey", 478u, "GlovesGinchGreyGrey"),
		new ItemInfo("gloves_ginch_grey_charcoal", 479u, "GlovesGinchGreyCharcoal"),
		new ItemInfo("top_builder", 480u, "TopBuilder"),
		new ItemInfo("top_floral_pink", 481u, "TopFloralPink"),
		new ItemInfo("top_ginch_pink_saltrock", 482u, "TopGinchPinkSaltrock"),
		new ItemInfo("top_ginch_purple_dusky", 483u, "TopGinchPurpleDusky"),
		new ItemInfo("top_ginch_blue_basin", 484u, "TopGinchBlueBasin"),
		new ItemInfo("top_ginch_teal_balmy", 485u, "TopGinchTealBalmy"),
		new ItemInfo("top_ginch_green_lime", 486u, "TopGinchGreenLime"),
		new ItemInfo("top_ginch_yellow_yellowcake", 487u, "TopGinchYellowYellowcake"),
		new ItemInfo("top_ginch_orange_atomic", 488u, "TopGinchOrangeAtomic"),
		new ItemInfo("top_ginch_red_magma", 489u, "TopGinchRedMagma"),
		new ItemInfo("top_ginch_grey_grey", 490u, "TopGinchGreyGrey"),
		new ItemInfo("top_ginch_grey_charcoal", 491u, "TopGinchGreyCharcoal"),
		new ItemInfo("top_knit_polkadot_turq", 492u, "TopKnitPolkadotTurq"),
		new ItemInfo("top_flashy", 493u, "TopFlashy"),
		new ItemInfo("fridge_blue_babytears", 494u, "Refrigerator_blue_babytears"),
		new ItemInfo("fridge_green_mush", 495u, "Refrigerator_green_mush"),
		new ItemInfo("fridge_red_rose", 496u, "Refrigerator_red_rose"),
		new ItemInfo("fridge_yellow_tartar", 497u, "Refrigerator_yellow_tartar"),
		new ItemInfo("fridge_purple_brainfat", 498u, "Refrigerator_purple_brainfat"),
		new ItemInfo("microbemusher_purple_brainfat", 499u, "MicrobeMusher_purple_brainfat"),
		new ItemInfo("microbemusher_yellow_tartar", 500u, "MicrobeMusher_yellow_tartar"),
		new ItemInfo("microbemusher_red_rose", 501u, "MicrobeMusher_red_rose"),
		new ItemInfo("microbemusher_green_mush", 502u, "MicrobeMusher_green_mush"),
		new ItemInfo("microbemusher_blue_babytears", 503u, "MicrobeMusher_blue_babytears"),
		new ItemInfo("wash_sink_purple_brainfat", 504u, "WashSink_purple_brainfat"),
		new ItemInfo("wash_sink_blue_babytears", 505u, "WashSink_blue_babytears"),
		new ItemInfo("wash_sink_green_mush", 506u, "WashSink_green_mush"),
		new ItemInfo("wash_sink_yellow_tartar", 507u, "WashSink_yellow_tartar"),
		new ItemInfo("wash_sink_red_rose", 508u, "WashSink_red_rose"),
		new ItemInfo("toiletflush_polka_darkpurpleresin", 509u, "FlushToilet_polka_darkpurpleresin"),
		new ItemInfo("toiletflush_polka_darknavynookgreen", 510u, "FlushToilet_polka_darknavynookgreen"),
		new ItemInfo("toiletflush_purple_brainfat", 511u, "FlushToilet_purple_brainfat"),
		new ItemInfo("toiletflush_yellow_tartar", 512u, "FlushToilet_yellow_tartar"),
		new ItemInfo("toiletflush_red_rose", 513u, "FlushToilet_red_rose"),
		new ItemInfo("toiletflush_green_mush", 514u, "FlushToilet_green_mush"),
		new ItemInfo("toiletflush_blue_babytears", 515u, "FlushToilet_blue_babytears"),
		new ItemInfo("elegantbed_red_rose", 516u, "LuxuryBed_red_rose"),
		new ItemInfo("elegantbed_green_mush", 517u, "LuxuryBed_green_mush"),
		new ItemInfo("elegantbed_yellow_tartar", 518u, "LuxuryBed_yellow_tartar"),
		new ItemInfo("elegantbed_purple_brainfat", 519u, "LuxuryBed_purple_brainfat"),
		new ItemInfo("watercooler_yellow_tartar", 520u, "WaterCooler_yellow_tartar"),
		new ItemInfo("watercooler_red_rose", 521u, "WaterCooler_red_rose"),
		new ItemInfo("watercooler_green_mush", 522u, "WaterCooler_green_mush"),
		new ItemInfo("watercooler_purple_brainfat", 523u, "WaterCooler_purple_brainfat"),
		new ItemInfo("watercooler_blue_babytears", 524u, "WaterCooler_blue_babytears"),
		new ItemInfo("walls_stripes_yellow_tartar", 525u, "ExteriorWall_stripes_yellow_tartar"),
		new ItemInfo("walls_stripes_diagonal_yellow_tartar", 526u, "ExteriorWall_stripes_diagonal_yellow_tartar"),
		new ItemInfo("walls_stripes_circle_yellow_tartar", 527u, "ExteriorWall_stripes_circle_yellow_tartar"),
		new ItemInfo("walls_stripes_purple_brainfat", 528u, "ExteriorWall_stripes_purple_brainfat"),
		new ItemInfo("walls_stripes_diagonal_purple_brainfat", 529u, "ExteriorWall_stripes_diagonal_purple_brainfat"),
		new ItemInfo("walls_stripes_circle_purple_brainfat", 530u, "ExteriorWall_stripes_circle_purple_brainfat"),
		new ItemInfo("walls_floppy_azulene_vitro", 531u, "ExteriorWall_floppy_azulene_vitro"),
		new ItemInfo("walls_floppy_black_white", 532u, "ExteriorWall_floppy_black_white"),
		new ItemInfo("walls_floppy_peagreen_balmy", 533u, "ExteriorWall_floppy_peagreen_balmy"),
		new ItemInfo("walls_floppy_satsuma_yellowcake", 534u, "ExteriorWall_floppy_satsuma_yellowcake"),
		new ItemInfo("walls_floppy_magma_amino", 535u, "ExteriorWall_floppy_magma_amino"),
		new ItemInfo("walls_orange_juice", 536u, "ExteriorWall_orange_juice"),
		new ItemInfo("walls_paint_blots", 537u, "ExteriorWall_paint_blots"),
		new ItemInfo("walls_telescope", 538u, "ExteriorWall_telescope"),
		new ItemInfo("walls_tictactoe_o", 539u, "ExteriorWall_tictactoe_o"),
		new ItemInfo("walls_tictactoe_x", 540u, "ExteriorWall_tictactoe_x"),
		new ItemInfo("walls_dice_1", 541u, "ExteriorWall_dice_1"),
		new ItemInfo("walls_dice_2", 542u, "ExteriorWall_dice_2"),
		new ItemInfo("walls_dice_3", 543u, "ExteriorWall_dice_3"),
		new ItemInfo("walls_dice_4", 544u, "ExteriorWall_dice_4"),
		new ItemInfo("walls_dice_5", 545u, "ExteriorWall_dice_5"),
		new ItemInfo("walls_dice_6", 546u, "ExteriorWall_dice_6"),
		new ItemInfo("painting_art_r", 547u, "Canvas_Good16"),
		new ItemInfo("painting_wide_art_o", 548u, "CanvasWide_Good13")
	};

	private static Dictionary<string, ItemInfo> Mappings = ItemInfos.ToDictionary((ItemInfo x) => x.PermitId);

	private static Dictionary<string, string> ItemToPermit = ItemInfos.ToDictionary((ItemInfo x) => x.ItemType, (ItemInfo x) => x.PermitId);

	private static BoxInfo[] BoxInfos = new BoxInfo[8]
	{
		new BoxInfo("MYSTERYBOX_u44_box_a", "Shipment X", "Unaddressed packages have been discovered near the Printing Pod. They bear Gravitas logos, and trace amounts of Neutronium have been detected.", 80u, "ONI_giftbox_u44_box_a", account_reward: true),
		new BoxInfo("MYSTERYBOX_u44_box_b", "Shipment Y", "Unaddressed packages have been discovered near the Printing Pod. They bear Gravitas logos, and trace amounts of Neutronium have been detected.", 81u, "ONI_giftbox_u44_box_b", account_reward: true),
		new BoxInfo("MYSTERYBOX_u44_box_c", "Shipment Z", "Unaddressed packages have been discovered near the Printing Pod. They bear Gravitas logos, and trace amounts of Neutronium have been detected.", 82u, "ONI_giftbox_u44_box_c", account_reward: true),
		new BoxInfo("MYSTERYBOX_u45_box_a", "Team Players Crate", "Unaddressed packages have been discovered near the Printing Pod. They bear Gravitas logos, and trace amounts of Neutronium have been detected.", 148u, "ONI_giftbox_u44_box_b", account_reward: true),
		new BoxInfo("MYSTERYBOX_u45_box_b", "Pizzazz Crate", "Unaddressed packages have been discovered near the Printing Pod. They bear Gravitas logos, and trace amounts of Neutronium have been detected.", 149u, "ONI_giftbox_u44_box_c", account_reward: true),
		new BoxInfo("MYSTERYBOX_u46_box_a", "Superfruits Crate", "Unaddressed packages have been discovered near the Printing Pod. They bear Gravitas logos, and trace amounts of Neutronium have been detected.", 190u, "ONI_giftbox_u44_box_a", account_reward: true),
		new BoxInfo("MYSTERYBOX_u47_klei_fest", EQUIPMENT.PREFABS.ATMO_SUIT_SET.PUFT.NAME, EQUIPMENT.PREFABS.ATMO_SUIT_SET.PUFT.DESC, 196u, "ONI_box_puft_atmo_set", account_reward: false),
		new BoxInfo("MYSTERYBOX_u50_winter_holiday", EQUIPMENT.PREFABS.HOLIDAY_2023_CRATE.NAME, EQUIPMENT.PREFABS.HOLIDAY_2023_CRATE.DESC, 414u, "Holiday_2023_gift_box", account_reward: false)
	};

	private const string MYSTERYBOX_U44_DESC = "Unaddressed packages have been discovered near the Printing Pod. They bear Gravitas logos, and trace amounts of Neutronium have been detected.";

	private const string MYSTERYBOX_U45_DESC = "Unaddressed packages have been discovered near the Printing Pod. They bear Gravitas logos, and trace amounts of Neutronium have been detected.";

	private const string MYSTERYBOX_U46_DESC = "Unaddressed packages have been discovered near the Printing Pod. They bear Gravitas logos, and trace amounts of Neutronium have been detected.";

	private static Dictionary<string, BoxInfo> BoxMappings = BoxInfos.ToDictionary((BoxInfo x) => x.ItemType);

	private static HashSet<string> BoxSet = new HashSet<string>(BoxInfos.Select((BoxInfo x) => x.ItemType));

	public static IEnumerable<KleiItems.ItemData> IterateInventory()
	{
		foreach (KleiItems.ItemData item in KleiItems.IterateInventory(ItemToPermit, BoxSet))
		{
			yield return item;
		}
	}

	public static bool HasUnopenedItem()
	{
		return KleiItems.HasUnopenedItem(ItemToPermit, BoxSet);
	}

	public static bool IsPermitUnlocked(PermitResource permit)
	{
		return GetOwnedCount(permit) > 0;
	}

	public static int GetOwnedCount(PermitResource permit)
	{
		int result = 0;
		if (Mappings.TryGetValue(permit.Id, out var value))
		{
			result = KleiItems.GetOwnedItemCount(value.ItemType);
		}
		return result;
	}

	public static bool TryGetBoxInfo(KleiItems.ItemData item, out string name, out string desc, out string icon_name)
	{
		if (BoxMappings.TryGetValue(item.Id, out var value))
		{
			name = value.Name;
			desc = value.Description;
			icon_name = value.IconName;
			return true;
		}
		name = null;
		desc = null;
		icon_name = null;
		return false;
	}

	public static bool TryGetBarterPrice(string permit_id, out ulong buy_price, out ulong sell_price)
	{
		buy_price = (sell_price = 0uL);
		if (Mappings.TryGetValue(permit_id, out var value) && KleiItems.TryGetBarterPrice(value.ItemType, out buy_price, out sell_price))
		{
			return true;
		}
		return false;
	}

	public static void QueueRequestOpenOrUnboxItem(KleiItems.ItemData item, KleiItems.ResponseCallback cb)
	{
		DebugUtil.DevAssert(!item.IsOpened, "Can't open already opened item.");
		if (!item.IsOpened)
		{
			if (BoxSet.Contains(item.Id))
			{
				KleiItems.AddRequestMysteryBoxOpened(item.ItemId, cb);
			}
			else
			{
				KleiItems.AddRequestItemOpened(item.ItemId, cb);
			}
		}
	}

	public static string GetServerTypeFromPermit(PermitResource resource)
	{
		ItemInfo[] itemInfos = ItemInfos;
		for (int i = 0; i < itemInfos.Length; i++)
		{
			ItemInfo itemInfo = itemInfos[i];
			if (itemInfo.PermitId == resource.Id)
			{
				return itemInfo.ItemType;
			}
		}
		Debug.LogError("No matching server ItemType for requested PermitResource " + resource.Id);
		return null;
	}
}
