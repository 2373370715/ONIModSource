using System;

namespace TUNING
{
	// Token: 0x02002286 RID: 8838
	public class EQUIPMENT
	{
		// Token: 0x02002287 RID: 8839
		public class TOYS
		{
			// Token: 0x04009B14 RID: 39700
			public static string SLOT = "Toy";

			// Token: 0x04009B15 RID: 39701
			public static float BALLOON_MASS = 1f;
		}

		// Token: 0x02002288 RID: 8840
		public class ATTRIBUTE_MOD_IDS
		{
			// Token: 0x04009B16 RID: 39702
			public static string DECOR = "Decor";

			// Token: 0x04009B17 RID: 39703
			public static string INSULATION = "Insulation";

			// Token: 0x04009B18 RID: 39704
			public static string ATHLETICS = "Athletics";

			// Token: 0x04009B19 RID: 39705
			public static string DIGGING = "Digging";

			// Token: 0x04009B1A RID: 39706
			public static string MAX_UNDERWATER_TRAVELCOST = "MaxUnderwaterTravelCost";

			// Token: 0x04009B1B RID: 39707
			public static string THERMAL_CONDUCTIVITY_BARRIER = "ThermalConductivityBarrier";
		}

		// Token: 0x02002289 RID: 8841
		public class TOOLS
		{
			// Token: 0x04009B1C RID: 39708
			public static string TOOLSLOT = "Multitool";

			// Token: 0x04009B1D RID: 39709
			public static string TOOLFABRICATOR = "MultitoolWorkbench";

			// Token: 0x04009B1E RID: 39710
			public static string TOOL_ANIM = "constructor_gun_kanim";
		}

		// Token: 0x0200228A RID: 8842
		public class CLOTHING
		{
			// Token: 0x04009B1F RID: 39711
			public static string SLOT = "Outfit";
		}

		// Token: 0x0200228B RID: 8843
		public class SUITS
		{
			// Token: 0x04009B20 RID: 39712
			public static string SLOT = "Suit";

			// Token: 0x04009B21 RID: 39713
			public static string FABRICATOR = "SuitFabricator";

			// Token: 0x04009B22 RID: 39714
			public static string ANIM = "clothing_kanim";

			// Token: 0x04009B23 RID: 39715
			public static string SNAPON = "snapTo_neck";

			// Token: 0x04009B24 RID: 39716
			public static float SUIT_DURABILITY_SKILL_BONUS = 0.25f;

			// Token: 0x04009B25 RID: 39717
			public static int OXYMASK_FABTIME = 20;

			// Token: 0x04009B26 RID: 39718
			public static int ATMOSUIT_FABTIME = 40;

			// Token: 0x04009B27 RID: 39719
			public static int ATMOSUIT_INSULATION = 50;

			// Token: 0x04009B28 RID: 39720
			public static int ATMOSUIT_ATHLETICS = -6;

			// Token: 0x04009B29 RID: 39721
			public static float ATMOSUIT_THERMAL_CONDUCTIVITY_BARRIER = 0.2f;

			// Token: 0x04009B2A RID: 39722
			public static int ATMOSUIT_DIGGING = 10;

			// Token: 0x04009B2B RID: 39723
			public static int ATMOSUIT_CONSTRUCTION = 10;

			// Token: 0x04009B2C RID: 39724
			public static float ATMOSUIT_BLADDER = -0.18333334f;

			// Token: 0x04009B2D RID: 39725
			public static int ATMOSUIT_MASS = 200;

			// Token: 0x04009B2E RID: 39726
			public static int ATMOSUIT_SCALDING = 1000;

			// Token: 0x04009B2F RID: 39727
			public static int ATMOSUIT_SCOLDING = -1000;

			// Token: 0x04009B30 RID: 39728
			public static float ATMOSUIT_DECAY = -0.1f;

			// Token: 0x04009B31 RID: 39729
			public static float LEADSUIT_THERMAL_CONDUCTIVITY_BARRIER = 0.3f;

			// Token: 0x04009B32 RID: 39730
			public static int LEADSUIT_SCALDING = 1000;

			// Token: 0x04009B33 RID: 39731
			public static int LEADSUIT_SCOLDING = -1000;

			// Token: 0x04009B34 RID: 39732
			public static int LEADSUIT_INSULATION = 50;

			// Token: 0x04009B35 RID: 39733
			public static int LEADSUIT_STRENGTH = 10;

			// Token: 0x04009B36 RID: 39734
			public static int LEADSUIT_ATHLETICS = -8;

			// Token: 0x04009B37 RID: 39735
			public static float LEADSUIT_RADIATION_SHIELDING = 0.66f;

			// Token: 0x04009B38 RID: 39736
			public static int AQUASUIT_FABTIME = EQUIPMENT.SUITS.ATMOSUIT_FABTIME;

			// Token: 0x04009B39 RID: 39737
			public static int AQUASUIT_INSULATION = 0;

			// Token: 0x04009B3A RID: 39738
			public static int AQUASUIT_ATHLETICS = EQUIPMENT.SUITS.ATMOSUIT_ATHLETICS;

			// Token: 0x04009B3B RID: 39739
			public static int AQUASUIT_MASS = EQUIPMENT.SUITS.ATMOSUIT_MASS;

			// Token: 0x04009B3C RID: 39740
			public static int AQUASUIT_UNDERWATER_TRAVELCOST = 6;

			// Token: 0x04009B3D RID: 39741
			public static int TEMPERATURESUIT_FABTIME = EQUIPMENT.SUITS.ATMOSUIT_FABTIME;

			// Token: 0x04009B3E RID: 39742
			public static float TEMPERATURESUIT_INSULATION = 0.2f;

			// Token: 0x04009B3F RID: 39743
			public static int TEMPERATURESUIT_ATHLETICS = EQUIPMENT.SUITS.ATMOSUIT_ATHLETICS;

			// Token: 0x04009B40 RID: 39744
			public static int TEMPERATURESUIT_MASS = EQUIPMENT.SUITS.ATMOSUIT_MASS;

			// Token: 0x04009B41 RID: 39745
			public const int OXYGEN_MASK_MASS = 15;

			// Token: 0x04009B42 RID: 39746
			public static int OXYGEN_MASK_ATHLETICS = -2;

			// Token: 0x04009B43 RID: 39747
			public static float OXYGEN_MASK_DECAY = -0.2f;

			// Token: 0x04009B44 RID: 39748
			public static float INDESTRUCTIBLE_DURABILITY_MOD = 0f;

			// Token: 0x04009B45 RID: 39749
			public static float REINFORCED_DURABILITY_MOD = 0.5f;

			// Token: 0x04009B46 RID: 39750
			public static float FLIMSY_DURABILITY_MOD = 1.5f;

			// Token: 0x04009B47 RID: 39751
			public static float THREADBARE_DURABILITY_MOD = 2f;

			// Token: 0x04009B48 RID: 39752
			public static float MINIMUM_USABLE_SUIT_CHARGE = 0.95f;
		}

		// Token: 0x0200228C RID: 8844
		public class VESTS
		{
			// Token: 0x04009B49 RID: 39753
			public static string SLOT = "Suit";

			// Token: 0x04009B4A RID: 39754
			public static string FABRICATOR = "ClothingFabricator";

			// Token: 0x04009B4B RID: 39755
			public static string SNAPON0 = "snapTo_body";

			// Token: 0x04009B4C RID: 39756
			public static string SNAPON1 = "snapTo_arm";

			// Token: 0x04009B4D RID: 39757
			public static string WARM_VEST_ANIM0 = "body_shirt_hot_shearling_kanim";

			// Token: 0x04009B4E RID: 39758
			public static string WARM_VEST_ICON0 = "shirt_hot_shearling_kanim";

			// Token: 0x04009B4F RID: 39759
			public static float WARM_VEST_FABTIME = 180f;

			// Token: 0x04009B50 RID: 39760
			public static float WARM_VEST_INSULATION = 0.01f;

			// Token: 0x04009B51 RID: 39761
			public static int WARM_VEST_MASS = 4;

			// Token: 0x04009B52 RID: 39762
			public static float COOL_VEST_FABTIME = EQUIPMENT.VESTS.WARM_VEST_FABTIME;

			// Token: 0x04009B53 RID: 39763
			public static float COOL_VEST_INSULATION = 0.01f;

			// Token: 0x04009B54 RID: 39764
			public static int COOL_VEST_MASS = EQUIPMENT.VESTS.WARM_VEST_MASS;

			// Token: 0x04009B55 RID: 39765
			public static float FUNKY_VEST_FABTIME = EQUIPMENT.VESTS.WARM_VEST_FABTIME;

			// Token: 0x04009B56 RID: 39766
			public static float FUNKY_VEST_DECOR = 1f;

			// Token: 0x04009B57 RID: 39767
			public static int FUNKY_VEST_MASS = EQUIPMENT.VESTS.WARM_VEST_MASS;

			// Token: 0x04009B58 RID: 39768
			public static float CUSTOM_CLOTHING_FABTIME = 180f;

			// Token: 0x04009B59 RID: 39769
			public static float CUSTOM_ATMOSUIT_FABTIME = 15f;

			// Token: 0x04009B5A RID: 39770
			public static int CUSTOM_CLOTHING_MASS = EQUIPMENT.VESTS.WARM_VEST_MASS + 3;
		}
	}
}
