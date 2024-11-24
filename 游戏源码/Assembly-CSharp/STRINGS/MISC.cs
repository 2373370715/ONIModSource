using System;

namespace STRINGS
{
	// Token: 0x02003969 RID: 14697
	public class MISC
	{
		// Token: 0x0200396A RID: 14698
		public class TAGS
		{
			// Token: 0x0400DD39 RID: 56633
			public static LocString OTHER = "Miscellaneous";

			// Token: 0x0400DD3A RID: 56634
			public static LocString FILTER = UI.FormatAsLink("Filtration Medium", "FILTER");

			// Token: 0x0400DD3B RID: 56635
			public static LocString FILTER_DESC = string.Concat(new string[]
			{
				"Filtration Mediums are materials which are supplied to some filtration buildings that are used in separating purified ",
				UI.FormatAsLink("gases", "ELEMENTS_GASSES"),
				" or ",
				UI.FormatAsLink("liquids", "ELEMENTS_LIQUID"),
				" from their polluted forms.\n\nExamples include filtering ",
				UI.FormatAsLink("Water", "WATER"),
				" from ",
				UI.FormatAsLink("Polluted Water", "DIRTYWATER"),
				" using a ",
				UI.FormatAsLink("Water Sieve", "WATERPURIFIER"),
				", or a ",
				UI.FormatAsLink("Deodorizer", "AIRFILTER"),
				" purifying ",
				UI.FormatAsLink("Oxygen", "OXYGEN"),
				" from ",
				UI.FormatAsLink("Polluted Oxygen", "CONTAMINATEDOXYGEN"),
				".\n\nFiltration Mediums are a consumable that will be transformed by the filtering process to generate a by-product, like when ",
				UI.FormatAsLink("Polluted Dirt", "TOXICSAND"),
				" is the result after ",
				UI.FormatAsLink("Sand", "SAND"),
				" has been used to filter polluted water. The filtering building will cease to function once the filtering material has been consumed. Once the Filtering Material has been resupplied to the filtering building it will start working again."
			});

			// Token: 0x0400DD3C RID: 56636
			public static LocString ICEORE = UI.FormatAsLink("Ice", "ICEORE");

			// Token: 0x0400DD3D RID: 56637
			public static LocString ICEORE_DESC = string.Concat(new string[]
			{
				"Ice is a class of materials made up mostly (if not completely) of ",
				UI.FormatAsLink("Water", "WATER"),
				" in a frozen or partially frozen form.\n\nAs a material in a frigid solid or semi-solid state, these elements are very useful as a low-cost way to cool the environment around them.\n\nWhen heated, ice will melt into its original liquified form (ie.",
				UI.FormatAsLink("Brine Ice", "BRINEICE"),
				" will liquify into ",
				UI.FormatAsLink("Brine", "BRINE"),
				"). Each ice element has a different freezing and melting point based upon its composition and state."
			});

			// Token: 0x0400DD3E RID: 56638
			public static LocString PHOSPHORUS = UI.FormatAsLink("Phosphorus", "PHOSPHORUS");

			// Token: 0x0400DD3F RID: 56639
			public static LocString BUILDABLERAW = UI.FormatAsLink("Raw Mineral", "BUILDABLERAW");

			// Token: 0x0400DD40 RID: 56640
			public static LocString BUILDABLERAW_DESC = string.Concat(new string[]
			{
				"Raw minerals are the unrefined forms of organic solids. Almost all raw minerals can be processed in the ",
				UI.FormatAsLink("Rock Crusher", "ROCKCRUSHER"),
				", although a handful require the use of the ",
				UI.FormatAsLink("Molecular Forge", "SUPERMATERIALREFINERY"),
				"."
			});

			// Token: 0x0400DD41 RID: 56641
			public static LocString BUILDABLEPROCESSED = UI.FormatAsLink("Refined Mineral", "BUILDABLEPROCESSED");

			// Token: 0x0400DD42 RID: 56642
			public static LocString BUILDABLEANY = UI.FormatAsLink("General Buildable", "BUILDABLEANY");

			// Token: 0x0400DD43 RID: 56643
			public static LocString BUILDABLEANY_DESC = "";

			// Token: 0x0400DD44 RID: 56644
			public static LocString DEHYDRATED = "Dehydrated";

			// Token: 0x0400DD45 RID: 56645
			public static LocString PLASTIFIABLELIQUID = UI.FormatAsLink("Plastic Monomer", "PLASTIFIABLELIQUID");

			// Token: 0x0400DD46 RID: 56646
			public static LocString PLASTIFIABLELIQUID_DESC = string.Concat(new string[]
			{
				"Plastic monomers are organic compounds that can be processed into ",
				UI.FormatAsLink("Plastics", "PLASTIC"),
				" that have valuable applications as advanced building materials.\n\nPlastics derived from these monomers can also be used as packaging materials for ",
				UI.FormatAsLink("Food", "FOOD"),
				" preservation."
			});

			// Token: 0x0400DD47 RID: 56647
			public static LocString UNREFINEDOIL = UI.FormatAsLink("Unrefined Oil", "RAWOIL");

			// Token: 0x0400DD48 RID: 56648
			public static LocString UNREFINEDOIL_DESC = "Oils in their raw, minimally processed forms. They can be refined at the " + UI.FormatAsLink("Oil Refinery", "OILREFINERY") + ".";

			// Token: 0x0400DD49 RID: 56649
			public static LocString REFINEDMETAL = UI.FormatAsLink("Refined Metal", "REFINEDMETAL");

			// Token: 0x0400DD4A RID: 56650
			public static LocString REFINEDMETAL_DESC = string.Concat(new string[]
			{
				"Refined metals are purified forms of metal often used in higher-tier electronics due to their tendency to be able to withstand higher temperatures when they are made into wires. Other benefits include the increased decor value for some metals which can greatly improve the well-being of a colony.\n\nMetal ore can be refined in either the ",
				UI.FormatAsLink("Rock Crusher", "ROCKCRUSHER"),
				" or the ",
				UI.FormatAsLink("Metal Refinery", "METALREFINERY"),
				"."
			});

			// Token: 0x0400DD4B RID: 56651
			public static LocString METAL = UI.FormatAsLink("Metal Ore", "METAL");

			// Token: 0x0400DD4C RID: 56652
			public static LocString METAL_DESC = string.Concat(new string[]
			{
				"Metal ore is the raw form of metal, and has a wide variety of practical applications in electronics and general construction.\n\nMetal ore is typically processed into ",
				UI.FormatAsLink("Refined Metal", "REFINEDMETAL"),
				" using the ",
				UI.FormatAsLink("Rock Crusher", "ROCKCRUSHER"),
				" or the ",
				UI.FormatAsLink("Metal Refinery", "METALREFINERY"),
				".\n\nSome rare metal ores can also be refined in the ",
				UI.FormatAsLink("Molecular Forge", "SUPERMATERIALREFINERY"),
				"."
			});

			// Token: 0x0400DD4D RID: 56653
			public static LocString PRECIOUSMETAL = UI.FormatAsLink("Precious Metal", "PRECIOUSMETAL");

			// Token: 0x0400DD4E RID: 56654
			public static LocString RAWPRECIOUSMETAL = "Precious Metal Ore";

			// Token: 0x0400DD4F RID: 56655
			public static LocString PRECIOUSROCK = UI.FormatAsLink("Precious Rock", "PRECIOUSROCK");

			// Token: 0x0400DD50 RID: 56656
			public static LocString PRECIOUSROCK_DESC = "Precious rocks are raw minerals. Their extreme hardness produces durable " + UI.FormatAsLink("Decor", "DECOR") + ".\n\nSome precious rocks are inherently attractive even in their natural, unfinished form.";

			// Token: 0x0400DD51 RID: 56657
			public static LocString ALLOY = UI.FormatAsLink("Alloy", "ALLOY");

			// Token: 0x0400DD52 RID: 56658
			public static LocString BUILDINGFIBER = UI.FormatAsLink("Fiber", "BUILDINGFIBER");

			// Token: 0x0400DD53 RID: 56659
			public static LocString BUILDINGFIBER_DESC = "Fibers are organically sourced polymers which are both sturdy and sensorially pleasant, making them suitable in the construction of " + UI.FormatAsLink("Morale", "MORALE") + "-boosting buildings.";

			// Token: 0x0400DD54 RID: 56660
			public static LocString BUILDINGWOOD = UI.FormatAsLink("Wood", "BUILDINGWOOD");

			// Token: 0x0400DD55 RID: 56661
			public static LocString BUILDINGWOOD_DESC = string.Concat(new string[]
			{
				"Wood is a renewable building material which can also be used as a valuable source of fuel and electricity when refined at the ",
				UI.FormatAsLink("Wood Burner", "WOODGASGENERATOR"),
				" or the ",
				UI.FormatAsLink("Ethanol Distiller", "ETHANOLDISTILLERY"),
				"."
			});

			// Token: 0x0400DD56 RID: 56662
			public static LocString CRUSHABLE = "Crushable";

			// Token: 0x0400DD57 RID: 56663
			public static LocString CROPSEEDS = "Crop Seeds";

			// Token: 0x0400DD58 RID: 56664
			public static LocString CERAMIC = UI.FormatAsLink("Ceramic", "CERAMIC");

			// Token: 0x0400DD59 RID: 56665
			public static LocString POLYPROPYLENE = UI.FormatAsLink("Plastic", "POLYPROPYLENE");

			// Token: 0x0400DD5A RID: 56666
			public static LocString BAGABLECREATURE = UI.FormatAsLink("Critter", "CREATURES");

			// Token: 0x0400DD5B RID: 56667
			public static LocString SWIMMINGCREATURE = "Aquatic Critter";

			// Token: 0x0400DD5C RID: 56668
			public static LocString LIFE = "Life";

			// Token: 0x0400DD5D RID: 56669
			public static LocString LIQUIFIABLE = "Liquefiable";

			// Token: 0x0400DD5E RID: 56670
			public static LocString LIQUID = UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID");

			// Token: 0x0400DD5F RID: 56671
			public static LocString LUBRICATINGOIL = "Gear Oil";

			// Token: 0x0400DD60 RID: 56672
			public static LocString LUBRICATINGOIL_DESC = "Gear oils are lubricating fluids useful in the maintenance of complex machinery, protecting gear systems from damage and minimizing friction between moving parts to support optimal performance.";

			// Token: 0x0400DD61 RID: 56673
			public static LocString SLIPPERY = "Slippery";

			// Token: 0x0400DD62 RID: 56674
			public static LocString LEAD = UI.FormatAsLink("Lead", "LEAD");

			// Token: 0x0400DD63 RID: 56675
			public static LocString CHARGEDPORTABLEBATTERY = UI.FormatAsLink("Power Banks", "ELECTROBANK");

			// Token: 0x0400DD64 RID: 56676
			public static LocString EMPTYPORTABLEBATTERY = UI.FormatAsLink("Empty Eco Power Banks", "ELECTROBANK_EMPTY");

			// Token: 0x0400DD65 RID: 56677
			public static LocString SPECIAL = "Special";

			// Token: 0x0400DD66 RID: 56678
			public static LocString FARMABLE = UI.FormatAsLink("Cultivable Soil", "FARMABLE");

			// Token: 0x0400DD67 RID: 56679
			public static LocString FARMABLE_DESC = "Cultivable soil is a fundamental building block of basic agricultural systems and can also be useful in the production of clean " + UI.FormatAsLink("Oxygen", "OXYGEN") + ".";

			// Token: 0x0400DD68 RID: 56680
			public static LocString AGRICULTURE = UI.FormatAsLink("Agriculture", "AGRICULTURE");

			// Token: 0x0400DD69 RID: 56681
			public static LocString COAL = "Coal";

			// Token: 0x0400DD6A RID: 56682
			public static LocString BLEACHSTONE = "Bleach Stone";

			// Token: 0x0400DD6B RID: 56683
			public static LocString ORGANICS = "Organic";

			// Token: 0x0400DD6C RID: 56684
			public static LocString CONSUMABLEORE = "Consumable Ore";

			// Token: 0x0400DD6D RID: 56685
			public static LocString SUBLIMATING = "Sublimators";

			// Token: 0x0400DD6E RID: 56686
			public static LocString ORE = "Ore";

			// Token: 0x0400DD6F RID: 56687
			public static LocString BREATHABLE = "Breathable Gas";

			// Token: 0x0400DD70 RID: 56688
			public static LocString UNBREATHABLE = "Unbreathable Gas";

			// Token: 0x0400DD71 RID: 56689
			public static LocString GAS = "Gas";

			// Token: 0x0400DD72 RID: 56690
			public static LocString BURNS = "Flammable";

			// Token: 0x0400DD73 RID: 56691
			public static LocString UNSTABLE = "Unstable";

			// Token: 0x0400DD74 RID: 56692
			public static LocString TOXIC = "Toxic";

			// Token: 0x0400DD75 RID: 56693
			public static LocString MIXTURE = "Mixture";

			// Token: 0x0400DD76 RID: 56694
			public static LocString SOLID = UI.FormatAsLink("Solid", "ELEMENTS_SOLID");

			// Token: 0x0400DD77 RID: 56695
			public static LocString FLYINGCRITTEREDIBLE = "Bait";

			// Token: 0x0400DD78 RID: 56696
			public static LocString INDUSTRIALPRODUCT = "Industrial Product";

			// Token: 0x0400DD79 RID: 56697
			public static LocString INDUSTRIALINGREDIENT = UI.FormatAsLink("Industrial Ingredient", "INDUSTRIALINGREDIENT");

			// Token: 0x0400DD7A RID: 56698
			public static LocString MEDICALSUPPLIES = "Medical Supplies";

			// Token: 0x0400DD7B RID: 56699
			public static LocString CLOTHES = UI.FormatAsLink("Clothing", "EQUIPMENT");

			// Token: 0x0400DD7C RID: 56700
			public static LocString EMITSLIGHT = UI.FormatAsLink("Light Emitter", "LIGHT");

			// Token: 0x0400DD7D RID: 56701
			public static LocString BED = "Beds";

			// Token: 0x0400DD7E RID: 56702
			public static LocString MESSSTATION = "Dining Table";

			// Token: 0x0400DD7F RID: 56703
			public static LocString TOY = "Toy";

			// Token: 0x0400DD80 RID: 56704
			public static LocString SUIT = "Suits";

			// Token: 0x0400DD81 RID: 56705
			public static LocString MULTITOOL = "Multitool";

			// Token: 0x0400DD82 RID: 56706
			public static LocString CLINIC = "Clinic";

			// Token: 0x0400DD83 RID: 56707
			public static LocString RELAXATION_POINT = "Leisure Area";

			// Token: 0x0400DD84 RID: 56708
			public static LocString SOLIDMATERIAL = "Solid Material";

			// Token: 0x0400DD85 RID: 56709
			public static LocString EXTRUDABLE = "Extrudable";

			// Token: 0x0400DD86 RID: 56710
			public static LocString PLUMBABLE = UI.FormatAsLink("Plumbable", "PLUMBABLE");

			// Token: 0x0400DD87 RID: 56711
			public static LocString PLUMBABLE_DESC = "";

			// Token: 0x0400DD88 RID: 56712
			public static LocString COMPOSTABLE = UI.FormatAsLink("Compostable", "COMPOSTABLE");

			// Token: 0x0400DD89 RID: 56713
			public static LocString COMPOSTABLE_DESC = string.Concat(new string[]
			{
				"Compostables are biological materials which can be put into a ",
				UI.FormatAsLink("Compost", "COMPOST"),
				" to generate clean ",
				UI.FormatAsLink("Dirt", "DIRT"),
				".\n\nComposting also generates a small amount of ",
				UI.FormatAsLink("Heat", "HEAT"),
				".\n\nOnce it starts to rot, consumable food should be composted to prevent ",
				UI.FormatAsLink("Food Poisoning", "FOODSICKNESS"),
				"."
			});

			// Token: 0x0400DD8A RID: 56714
			public static LocString COMPOSTBASICPLANTFOOD = "Compost Muckroot";

			// Token: 0x0400DD8B RID: 56715
			public static LocString EDIBLE = "Edible";

			// Token: 0x0400DD8C RID: 56716
			public static LocString OXIDIZER = "Oxidizer";

			// Token: 0x0400DD8D RID: 56717
			public static LocString COOKINGINGREDIENT = "Cooking Ingredient";

			// Token: 0x0400DD8E RID: 56718
			public static LocString MEDICINE = "Medicine";

			// Token: 0x0400DD8F RID: 56719
			public static LocString SEED = "Seed";

			// Token: 0x0400DD90 RID: 56720
			public static LocString ANYWATER = "Water Based";

			// Token: 0x0400DD91 RID: 56721
			public static LocString MARKEDFORCOMPOST = "Marked For Compost";

			// Token: 0x0400DD92 RID: 56722
			public static LocString MARKEDFORCOMPOSTINSTORAGE = "In Compost Storage";

			// Token: 0x0400DD93 RID: 56723
			public static LocString COMPOSTMEAT = "Compost Meat";

			// Token: 0x0400DD94 RID: 56724
			public static LocString PICKLED = "Pickled";

			// Token: 0x0400DD95 RID: 56725
			public static LocString PLASTIC = UI.FormatAsLink("Plastics", "PLASTIC");

			// Token: 0x0400DD96 RID: 56726
			public static LocString PLASTIC_DESC = string.Concat(new string[]
			{
				"Plastics are synthetic ",
				UI.FormatAsLink("Solids", "ELEMENTSSOLID"),
				" that are pliable and minimize the transfer of ",
				UI.FormatAsLink("Heat", "Heat"),
				". They typically have a low melting point, although more advanced plastics have been developed to circumvent this issue."
			});

			// Token: 0x0400DD97 RID: 56727
			public static LocString TOILET = "Toilets";

			// Token: 0x0400DD98 RID: 56728
			public static LocString MASSAGE_TABLE = "Massage Tables";

			// Token: 0x0400DD99 RID: 56729
			public static LocString POWERSTATION = "Power Station";

			// Token: 0x0400DD9A RID: 56730
			public static LocString FARMSTATION = "Farm Station";

			// Token: 0x0400DD9B RID: 56731
			public static LocString MACHINE_SHOP = "Machine Shop";

			// Token: 0x0400DD9C RID: 56732
			public static LocString ANTISEPTIC = "Antiseptic";

			// Token: 0x0400DD9D RID: 56733
			public static LocString OIL = "Hydrocarbon";

			// Token: 0x0400DD9E RID: 56734
			public static LocString DECORATION = "Decoration";

			// Token: 0x0400DD9F RID: 56735
			public static LocString EGG = "Critter Egg";

			// Token: 0x0400DDA0 RID: 56736
			public static LocString EGGSHELL = "Egg Shell";

			// Token: 0x0400DDA1 RID: 56737
			public static LocString MANUFACTUREDMATERIAL = "Manufactured Material";

			// Token: 0x0400DDA2 RID: 56738
			public static LocString STEEL = "Steel";

			// Token: 0x0400DDA3 RID: 56739
			public static LocString RAW = "Raw Animal Product";

			// Token: 0x0400DDA4 RID: 56740
			public static LocString FOSSIL = "Fossil";

			// Token: 0x0400DDA5 RID: 56741
			public static LocString ICE = "Ice";

			// Token: 0x0400DDA6 RID: 56742
			public static LocString ANY = "Any";

			// Token: 0x0400DDA7 RID: 56743
			public static LocString TRANSPARENT = "Transparent";

			// Token: 0x0400DDA8 RID: 56744
			public static LocString TRANSPARENT_DESC = string.Concat(new string[]
			{
				"Transparent materials allow ",
				UI.FormatAsLink("Light", "LIGHT"),
				" to pass through. Illumination boosts Duplicant productivity during working hours, but undermines sleep quality.\n\nTransparency is also important for buildings that require a clear line of sight in order to function correctly, such as the ",
				UI.FormatAsLink("Space Scanner", "COMETDETECTOR"),
				"."
			});

			// Token: 0x0400DDA9 RID: 56745
			public static LocString RAREMATERIALS = "Rare Resource";

			// Token: 0x0400DDAA RID: 56746
			public static LocString FARMINGMATERIAL = "Fertilizer";

			// Token: 0x0400DDAB RID: 56747
			public static LocString INSULATOR = UI.FormatAsLink("Insulator", "INSULATOR");

			// Token: 0x0400DDAC RID: 56748
			public static LocString INSULATOR_DESC = "Insulators have low thermal conductivity, and effectively reduce the speed at which " + UI.FormatAsLink("Heat", "Heat") + " is transferred through them.";

			// Token: 0x0400DDAD RID: 56749
			public static LocString RAILGUNPAYLOADEMPTYABLE = "Payload";

			// Token: 0x0400DDAE RID: 56750
			public static LocString NONCRUSHABLE = "Uncrushable";

			// Token: 0x0400DDAF RID: 56751
			public static LocString STORYTRAITRESOURCE = "Story Trait";

			// Token: 0x0400DDB0 RID: 56752
			public static LocString GLASS = "Glass";

			// Token: 0x0400DDB1 RID: 56753
			public static LocString OBSIDIAN = UI.FormatAsLink("Obsidian", "OBSIDIAN");

			// Token: 0x0400DDB2 RID: 56754
			public static LocString DIAMOND = UI.FormatAsLink("Diamond", "DIAMOND");

			// Token: 0x0400DDB3 RID: 56755
			public static LocString SNOW = UI.FormatAsLink("Snow", "STABLESNOW");

			// Token: 0x0400DDB4 RID: 56756
			public static LocString WOODLOG = UI.FormatAsLink("Wood", "WOODLOG");

			// Token: 0x0400DDB5 RID: 56757
			public static LocString COMMAND_MODULE = "Command Module";

			// Token: 0x0400DDB6 RID: 56758
			public static LocString HABITAT_MODULE = "Habitat Module";

			// Token: 0x0400DDB7 RID: 56759
			public static LocString COMBUSTIBLEGAS = UI.FormatAsLink("Combustible Gas", "COMBUSTIBLEGAS");

			// Token: 0x0400DDB8 RID: 56760
			public static LocString COMBUSTIBLEGAS_DESC = string.Concat(new string[]
			{
				"Combustible Gases can be burned as fuel to be used in the production of ",
				UI.FormatAsLink("Power", "POWER"),
				" and ",
				UI.FormatAsLink("Food", "FOOD"),
				"."
			});

			// Token: 0x0400DDB9 RID: 56761
			public static LocString COMBUSTIBLELIQUID = UI.FormatAsLink("Combustible Liquid", "COMBUSTIBLELIQUID");

			// Token: 0x0400DDBA RID: 56762
			public static LocString COMBUSTIBLELIQUID_DESC = string.Concat(new string[]
			{
				"Combustible Liquids can be burned as fuels to be used in energy production, such as in a ",
				UI.FormatAsLink("Petroleum Generator", "PETROLEUMGENERATOR"),
				" or a ",
				UI.FormatAsLink("Petroleum Engine", "KEROSENEENGINE"),
				".\n\nThough these liquids have other uses, such as fertilizer for growing a ",
				UI.FormatAsLink("Nosh Bean", "BEANPLANTSEED"),
				", their primary usefulness lies in their ability to be burned for ",
				UI.FormatAsLink("Power", "POWER"),
				"."
			});

			// Token: 0x0400DDBB RID: 56763
			public static LocString COMBUSTIBLESOLID = UI.FormatAsLink("Combustible Solid", "COMBUSTIBLESOLID");

			// Token: 0x0400DDBC RID: 56764
			public static LocString COMBUSTIBLESOLID_DESC = "Combustible Solids can be burned as fuel to be used in " + UI.FormatAsLink("Power", "POWER") + " production.";

			// Token: 0x0400DDBD RID: 56765
			public static LocString UNIDENTIFIEDSEED = "Seed (Unidentified Mutation)";

			// Token: 0x0400DDBE RID: 56766
			public static LocString CHARMEDARTIFACT = "Artifact of Interest";

			// Token: 0x0400DDBF RID: 56767
			public static LocString GENE_SHUFFLER = "Neural Vacillator";

			// Token: 0x0400DDC0 RID: 56768
			public static LocString WARP_PORTAL = "Teleportal";

			// Token: 0x0400DDC1 RID: 56769
			public static LocString BIONIC_UPGRADE = "Boosters";

			// Token: 0x0400DDC2 RID: 56770
			public static LocString FARMING = "Farm Build-Delivery";

			// Token: 0x0400DDC3 RID: 56771
			public static LocString RESEARCH = "Research Delivery";

			// Token: 0x0400DDC4 RID: 56772
			public static LocString POWER = "Generator Delivery";

			// Token: 0x0400DDC5 RID: 56773
			public static LocString BUILDING = "Build Dig-Delivery";

			// Token: 0x0400DDC6 RID: 56774
			public static LocString COOKING = "Cook Delivery";

			// Token: 0x0400DDC7 RID: 56775
			public static LocString FABRICATING = "Fabricate Delivery";

			// Token: 0x0400DDC8 RID: 56776
			public static LocString WIRING = "Wire Build-Delivery";

			// Token: 0x0400DDC9 RID: 56777
			public static LocString ART = "Art Build-Delivery";

			// Token: 0x0400DDCA RID: 56778
			public static LocString DOCTORING = "Treatment Delivery";

			// Token: 0x0400DDCB RID: 56779
			public static LocString CONVEYOR = "Shipping Build";

			// Token: 0x0400DDCC RID: 56780
			public static LocString COMPOST_FORMAT = "{Item}";

			// Token: 0x0400DDCD RID: 56781
			public static LocString ADVANCEDDOCTORSTATIONMEDICALSUPPLIES = "Serum Vial";

			// Token: 0x0400DDCE RID: 56782
			public static LocString DOCTORSTATIONMEDICALSUPPLIES = "Medical Pack";
		}

		// Token: 0x0200396B RID: 14699
		public class STATUSITEMS
		{
			// Token: 0x0200396C RID: 14700
			public class ATTENTIONREQUIRED
			{
				// Token: 0x0400DDCF RID: 56783
				public static LocString NAME = "Attention Required!";

				// Token: 0x0400DDD0 RID: 56784
				public static LocString TOOLTIP = "Something in my colony needs to be attended to";
			}

			// Token: 0x0200396D RID: 14701
			public class SUBLIMATIONBLOCKED
			{
				// Token: 0x0400DDD1 RID: 56785
				public static LocString NAME = "{SubElement} emission blocked";

				// Token: 0x0400DDD2 RID: 56786
				public static LocString TOOLTIP = "This {Element} deposit is not exposed to air and cannot emit {SubElement}";
			}

			// Token: 0x0200396E RID: 14702
			public class SUBLIMATIONOVERPRESSURE
			{
				// Token: 0x0400DDD3 RID: 56787
				public static LocString NAME = "Inert";

				// Token: 0x0400DDD4 RID: 56788
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Environmental ",
					UI.PRE_KEYWORD,
					"Gas Pressure",
					UI.PST_KEYWORD,
					" is too high for this {Element} deposit to emit {SubElement}"
				});
			}

			// Token: 0x0200396F RID: 14703
			public class SUBLIMATIONEMITTING
			{
				// Token: 0x0400DDD5 RID: 56789
				public static LocString NAME = BUILDING.STATUSITEMS.EMITTINGGASAVG.NAME;

				// Token: 0x0400DDD6 RID: 56790
				public static LocString TOOLTIP = BUILDING.STATUSITEMS.EMITTINGGASAVG.TOOLTIP;
			}

			// Token: 0x02003970 RID: 14704
			public class SPACE
			{
				// Token: 0x0400DDD7 RID: 56791
				public static LocString NAME = "Space exposure";

				// Token: 0x0400DDD8 RID: 56792
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This region is exposed to the vacuum of space and will result in the loss of ",
					UI.PRE_KEYWORD,
					"Gas",
					UI.PST_KEYWORD,
					" and ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					" resources"
				});
			}

			// Token: 0x02003971 RID: 14705
			public class EDIBLE
			{
				// Token: 0x0400DDD9 RID: 56793
				public static LocString NAME = "Rations: {0}";

				// Token: 0x0400DDDA RID: 56794
				public static LocString TOOLTIP = "Can provide " + UI.FormatAsLink("{0}", "KCAL") + " of energy to Duplicants";
			}

			// Token: 0x02003972 RID: 14706
			public class REHYDRATEDFOOD
			{
				// Token: 0x0400DDDB RID: 56795
				public static LocString NAME = "Rehydrated Food";

				// Token: 0x0400DDDC RID: 56796
				public static LocString TOOLTIP = string.Format(string.Concat(new string[]
				{
					"This food has been carefully re-moistened for consumption\n\n",
					UI.PRE_KEYWORD,
					"{1}",
					UI.PST_KEYWORD,
					": {0}"
				}), -1f, UI.FormatAsLink(DUPLICANTS.ATTRIBUTES.QUALITYOFLIFE.NAME, DUPLICANTS.ATTRIBUTES.QUALITYOFLIFE.NAME));
			}

			// Token: 0x02003973 RID: 14707
			public class MARKEDFORDISINFECTION
			{
				// Token: 0x0400DDDD RID: 56797
				public static LocString NAME = "Disinfect Errand";

				// Token: 0x0400DDDE RID: 56798
				public static LocString TOOLTIP = "Building will be disinfected once a Duplicant is available";
			}

			// Token: 0x02003974 RID: 14708
			public class PENDINGCLEAR
			{
				// Token: 0x0400DDDF RID: 56799
				public static LocString NAME = "Sweep Errand";

				// Token: 0x0400DDE0 RID: 56800
				public static LocString TOOLTIP = "Debris will be swept once a Duplicant is available";
			}

			// Token: 0x02003975 RID: 14709
			public class PENDINGCLEARNOSTORAGE
			{
				// Token: 0x0400DDE1 RID: 56801
				public static LocString NAME = "Storage Unavailable";

				// Token: 0x0400DDE2 RID: 56802
				public static LocString TOOLTIP = "No available " + BUILDINGS.PREFABS.STORAGELOCKER.NAME + " can accept this item\n\nMake sure the filter on your storage is correctly set and there is sufficient space remaining";
			}

			// Token: 0x02003976 RID: 14710
			public class MARKEDFORCOMPOST
			{
				// Token: 0x0400DDE3 RID: 56803
				public static LocString NAME = "Compost Errand";

				// Token: 0x0400DDE4 RID: 56804
				public static LocString TOOLTIP = "Object is marked and will be moved to " + BUILDINGS.PREFABS.COMPOST.NAME + " once a Duplicant is available";
			}

			// Token: 0x02003977 RID: 14711
			public class NOCLEARLOCATIONSAVAILABLE
			{
				// Token: 0x0400DDE5 RID: 56805
				public static LocString NAME = "No Sweep Destination";

				// Token: 0x0400DDE6 RID: 56806
				public static LocString TOOLTIP = "There are no valid destinations for this object to be swept to";
			}

			// Token: 0x02003978 RID: 14712
			public class PENDINGHARVEST
			{
				// Token: 0x0400DDE7 RID: 56807
				public static LocString NAME = "Harvest Errand";

				// Token: 0x0400DDE8 RID: 56808
				public static LocString TOOLTIP = "Plant will be harvested once a Duplicant is available";
			}

			// Token: 0x02003979 RID: 14713
			public class PENDINGUPROOT
			{
				// Token: 0x0400DDE9 RID: 56809
				public static LocString NAME = "Uproot Errand";

				// Token: 0x0400DDEA RID: 56810
				public static LocString TOOLTIP = "Plant will be uprooted once a Duplicant is available";
			}

			// Token: 0x0200397A RID: 14714
			public class WAITINGFORDIG
			{
				// Token: 0x0400DDEB RID: 56811
				public static LocString NAME = "Dig Errand";

				// Token: 0x0400DDEC RID: 56812
				public static LocString TOOLTIP = "Tile will be dug out once a Duplicant is available";
			}

			// Token: 0x0200397B RID: 14715
			public class WAITINGFORMOP
			{
				// Token: 0x0400DDED RID: 56813
				public static LocString NAME = "Mop Errand";

				// Token: 0x0400DDEE RID: 56814
				public static LocString TOOLTIP = "Spill will be mopped once a Duplicant is available";
			}

			// Token: 0x0200397C RID: 14716
			public class NOTMARKEDFORHARVEST
			{
				// Token: 0x0400DDEF RID: 56815
				public static LocString NAME = "No Harvest Pending";

				// Token: 0x0400DDF0 RID: 56816
				public static LocString TOOLTIP = "Use the " + UI.FormatAsTool("Harvest Tool", global::Action.Harvest) + " to mark this plant for harvest";
			}

			// Token: 0x0200397D RID: 14717
			public class GROWINGBRANCHES
			{
				// Token: 0x0400DDF1 RID: 56817
				public static LocString NAME = "Growing Branches";

				// Token: 0x0400DDF2 RID: 56818
				public static LocString TOOLTIP = "This tree is working hard to grow new branches right now";
			}

			// Token: 0x0200397E RID: 14718
			public class CLUSTERMETEORREMAININGTRAVELTIME
			{
				// Token: 0x0400DDF3 RID: 56819
				public static LocString NAME = "Time to collision: {time}";

				// Token: 0x0400DDF4 RID: 56820
				public static LocString TOOLTIP = "The time remaining before this meteor reaches its destination";
			}

			// Token: 0x0200397F RID: 14719
			public class ELEMENTALCATEGORY
			{
				// Token: 0x0400DDF5 RID: 56821
				public static LocString NAME = "{Category}";

				// Token: 0x0400DDF6 RID: 56822
				public static LocString TOOLTIP = "The selected object belongs to the <b>{Category}</b> resource category";
			}

			// Token: 0x02003980 RID: 14720
			public class ELEMENTALMASS
			{
				// Token: 0x0400DDF7 RID: 56823
				public static LocString NAME = "{Mass}";

				// Token: 0x0400DDF8 RID: 56824
				public static LocString TOOLTIP = "The selected object has a mass of <b>{Mass}</b>";
			}

			// Token: 0x02003981 RID: 14721
			public class ELEMENTALDISEASE
			{
				// Token: 0x0400DDF9 RID: 56825
				public static LocString NAME = "{Disease}";

				// Token: 0x0400DDFA RID: 56826
				public static LocString TOOLTIP = "Current disease: {Disease}";
			}

			// Token: 0x02003982 RID: 14722
			public class ELEMENTALTEMPERATURE
			{
				// Token: 0x0400DDFB RID: 56827
				public static LocString NAME = "{Temp}";

				// Token: 0x0400DDFC RID: 56828
				public static LocString TOOLTIP = "The selected object is currently <b>{Temp}</b>";
			}

			// Token: 0x02003983 RID: 14723
			public class MARKEDFORCOMPOSTINSTORAGE
			{
				// Token: 0x0400DDFD RID: 56829
				public static LocString NAME = "Composted";

				// Token: 0x0400DDFE RID: 56830
				public static LocString TOOLTIP = "The selected object is currently in the compost";
			}

			// Token: 0x02003984 RID: 14724
			public class BURIEDITEM
			{
				// Token: 0x0400DDFF RID: 56831
				public static LocString NAME = "Buried Object";

				// Token: 0x0400DE00 RID: 56832
				public static LocString TOOLTIP = "Something seems to be hidden here";

				// Token: 0x0400DE01 RID: 56833
				public static LocString NOTIFICATION = "Buried object discovered";

				// Token: 0x0400DE02 RID: 56834
				public static LocString NOTIFICATION_TOOLTIP = "My Duplicants have uncovered a {Uncoverable}!\n\n" + UI.CLICK(UI.ClickType.Click) + " to jump to its location.";
			}

			// Token: 0x02003985 RID: 14725
			public class GENETICANALYSISCOMPLETED
			{
				// Token: 0x0400DE03 RID: 56835
				public static LocString NAME = "Genome Sequenced";

				// Token: 0x0400DE04 RID: 56836
				public static LocString TOOLTIP = "This Station has sequenced a new seed mutation";
			}

			// Token: 0x02003986 RID: 14726
			public class HEALTHSTATUS
			{
				// Token: 0x02003987 RID: 14727
				public class PERFECT
				{
					// Token: 0x0400DE05 RID: 56837
					public static LocString NAME = "None";

					// Token: 0x0400DE06 RID: 56838
					public static LocString TOOLTIP = "This Duplicant is in peak condition";
				}

				// Token: 0x02003988 RID: 14728
				public class ALRIGHT
				{
					// Token: 0x0400DE07 RID: 56839
					public static LocString NAME = "None";

					// Token: 0x0400DE08 RID: 56840
					public static LocString TOOLTIP = "This Duplicant is none the worse for wear";
				}

				// Token: 0x02003989 RID: 14729
				public class SCUFFED
				{
					// Token: 0x0400DE09 RID: 56841
					public static LocString NAME = "Minor";

					// Token: 0x0400DE0A RID: 56842
					public static LocString TOOLTIP = "This Duplicant has a few scrapes and bruises";
				}

				// Token: 0x0200398A RID: 14730
				public class INJURED
				{
					// Token: 0x0400DE0B RID: 56843
					public static LocString NAME = "Moderate";

					// Token: 0x0400DE0C RID: 56844
					public static LocString TOOLTIP = "This Duplicant needs some patching up";
				}

				// Token: 0x0200398B RID: 14731
				public class CRITICAL
				{
					// Token: 0x0400DE0D RID: 56845
					public static LocString NAME = "Severe";

					// Token: 0x0400DE0E RID: 56846
					public static LocString TOOLTIP = "This Duplicant is in serious need of medical attention";
				}

				// Token: 0x0200398C RID: 14732
				public class INCAPACITATED
				{
					// Token: 0x0400DE0F RID: 56847
					public static LocString NAME = "Paralyzing";

					// Token: 0x0400DE10 RID: 56848
					public static LocString TOOLTIP = "This Duplicant will die if they do not receive medical attention";
				}

				// Token: 0x0200398D RID: 14733
				public class DEAD
				{
					// Token: 0x0400DE11 RID: 56849
					public static LocString NAME = "Conclusive";

					// Token: 0x0400DE12 RID: 56850
					public static LocString TOOLTIP = "This Duplicant won't be getting back up";
				}
			}

			// Token: 0x0200398E RID: 14734
			public class HIT
			{
				// Token: 0x0400DE13 RID: 56851
				public static LocString NAME = "{targetName} took {damageAmount} damage from {attackerName}'s attack!";
			}

			// Token: 0x0200398F RID: 14735
			public class OREMASS
			{
				// Token: 0x0400DE14 RID: 56852
				public static LocString NAME = MISC.STATUSITEMS.ELEMENTALMASS.NAME;

				// Token: 0x0400DE15 RID: 56853
				public static LocString TOOLTIP = MISC.STATUSITEMS.ELEMENTALMASS.TOOLTIP;
			}

			// Token: 0x02003990 RID: 14736
			public class ORETEMP
			{
				// Token: 0x0400DE16 RID: 56854
				public static LocString NAME = MISC.STATUSITEMS.ELEMENTALTEMPERATURE.NAME;

				// Token: 0x0400DE17 RID: 56855
				public static LocString TOOLTIP = MISC.STATUSITEMS.ELEMENTALTEMPERATURE.TOOLTIP;
			}

			// Token: 0x02003991 RID: 14737
			public class TREEFILTERABLETAGS
			{
				// Token: 0x0400DE18 RID: 56856
				public static LocString NAME = "{Tags}";

				// Token: 0x0400DE19 RID: 56857
				public static LocString TOOLTIP = "{Tags}";
			}

			// Token: 0x02003992 RID: 14738
			public class SPOUTOVERPRESSURE
			{
				// Token: 0x0400DE1A RID: 56858
				public static LocString NAME = "Overpressure {StudiedDetails}";

				// Token: 0x0400DE1B RID: 56859
				public static LocString TOOLTIP = "Spout cannot vent due to high environmental pressure";

				// Token: 0x0400DE1C RID: 56860
				public static LocString STUDIED = "(idle in <b>{Time}</b>)";
			}

			// Token: 0x02003993 RID: 14739
			public class SPOUTEMITTING
			{
				// Token: 0x0400DE1D RID: 56861
				public static LocString NAME = "Venting {StudiedDetails}";

				// Token: 0x0400DE1E RID: 56862
				public static LocString TOOLTIP = "This geyser is erupting";

				// Token: 0x0400DE1F RID: 56863
				public static LocString STUDIED = "(idle in <b>{Time}</b>)";
			}

			// Token: 0x02003994 RID: 14740
			public class SPOUTPRESSUREBUILDING
			{
				// Token: 0x0400DE20 RID: 56864
				public static LocString NAME = "Rising pressure {StudiedDetails}";

				// Token: 0x0400DE21 RID: 56865
				public static LocString TOOLTIP = "This geyser's internal pressure is steadily building";

				// Token: 0x0400DE22 RID: 56866
				public static LocString STUDIED = "(erupts in <b>{Time}</b>)";
			}

			// Token: 0x02003995 RID: 14741
			public class SPOUTIDLE
			{
				// Token: 0x0400DE23 RID: 56867
				public static LocString NAME = "Idle {StudiedDetails}";

				// Token: 0x0400DE24 RID: 56868
				public static LocString TOOLTIP = "This geyser is not currently erupting";

				// Token: 0x0400DE25 RID: 56869
				public static LocString STUDIED = "(erupts in <b>{Time}</b>)";
			}

			// Token: 0x02003996 RID: 14742
			public class SPOUTDORMANT
			{
				// Token: 0x0400DE26 RID: 56870
				public static LocString NAME = "Dormant";

				// Token: 0x0400DE27 RID: 56871
				public static LocString TOOLTIP = "This geyser's geoactivity has halted\n\nIt won't erupt again for some time";
			}

			// Token: 0x02003997 RID: 14743
			public class SPICEDFOOD
			{
				// Token: 0x0400DE28 RID: 56872
				public static LocString NAME = "Seasoned";

				// Token: 0x0400DE29 RID: 56873
				public static LocString TOOLTIP = "This food has been improved with spice from the " + BUILDINGS.PREFABS.SPICEGRINDER.NAME;
			}

			// Token: 0x02003998 RID: 14744
			public class PICKUPABLEUNREACHABLE
			{
				// Token: 0x0400DE2A RID: 56874
				public static LocString NAME = "Unreachable";

				// Token: 0x0400DE2B RID: 56875
				public static LocString TOOLTIP = "Duplicants cannot reach this object";
			}

			// Token: 0x02003999 RID: 14745
			public class PRIORITIZED
			{
				// Token: 0x0400DE2C RID: 56876
				public static LocString NAME = "High Priority";

				// Token: 0x0400DE2D RID: 56877
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This ",
					UI.PRE_KEYWORD,
					"Errand",
					UI.PST_KEYWORD,
					" has been marked as important and will be preferred over other pending ",
					UI.PRE_KEYWORD,
					"Errands",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x0200399A RID: 14746
			public class USING
			{
				// Token: 0x0400DE2E RID: 56878
				public static LocString NAME = "Using {Target}";

				// Token: 0x0400DE2F RID: 56879
				public static LocString TOOLTIP = "{Target} is currently in use";
			}

			// Token: 0x0200399B RID: 14747
			public class ORDERATTACK
			{
				// Token: 0x0400DE30 RID: 56880
				public static LocString NAME = "Pending Attack";

				// Token: 0x0400DE31 RID: 56881
				public static LocString TOOLTIP = "Waiting for a Duplicant to murderize this defenseless " + UI.PRE_KEYWORD + "Critter" + UI.PST_KEYWORD;
			}

			// Token: 0x0200399C RID: 14748
			public class ORDERCAPTURE
			{
				// Token: 0x0400DE32 RID: 56882
				public static LocString NAME = "Pending Wrangle";

				// Token: 0x0400DE33 RID: 56883
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Waiting for a Duplicant to capture this ",
					UI.PRE_KEYWORD,
					"Critter",
					UI.PST_KEYWORD,
					"\n\nOnly Duplicants with the ",
					DUPLICANTS.ROLES.RANCHER.NAME,
					" skill can catch critters without traps"
				});
			}

			// Token: 0x0200399D RID: 14749
			public class OPERATING
			{
				// Token: 0x0400DE34 RID: 56884
				public static LocString NAME = "In Use";

				// Token: 0x0400DE35 RID: 56885
				public static LocString TOOLTIP = "This object is currently being used";
			}

			// Token: 0x0200399E RID: 14750
			public class CLEANING
			{
				// Token: 0x0400DE36 RID: 56886
				public static LocString NAME = "Cleaning";

				// Token: 0x0400DE37 RID: 56887
				public static LocString TOOLTIP = "This building is currently being cleaned";
			}

			// Token: 0x0200399F RID: 14751
			public class REGIONISBLOCKED
			{
				// Token: 0x0400DE38 RID: 56888
				public static LocString NAME = "Blocked";

				// Token: 0x0400DE39 RID: 56889
				public static LocString TOOLTIP = "Undug material is blocking off an essential tile";
			}

			// Token: 0x020039A0 RID: 14752
			public class STUDIED
			{
				// Token: 0x0400DE3A RID: 56890
				public static LocString NAME = "Analysis Complete";

				// Token: 0x0400DE3B RID: 56891
				public static LocString TOOLTIP = "Information on this Natural Feature has been compiled below.";
			}

			// Token: 0x020039A1 RID: 14753
			public class AWAITINGSTUDY
			{
				// Token: 0x0400DE3C RID: 56892
				public static LocString NAME = "Analysis Pending";

				// Token: 0x0400DE3D RID: 56893
				public static LocString TOOLTIP = "New information on this Natural Feature will be compiled once the field study is complete";
			}

			// Token: 0x020039A2 RID: 14754
			public class DURABILITY
			{
				// Token: 0x0400DE3E RID: 56894
				public static LocString NAME = "Durability: {durability}";

				// Token: 0x0400DE3F RID: 56895
				public static LocString TOOLTIP = "Items lose durability each time they are equipped, and can no longer be put on by a Duplicant once they reach 0% durability\n\nRepair of this item can be done in the appropriate fabrication station";
			}

			// Token: 0x020039A3 RID: 14755
			public class BIONICEXPLORERBOOSTER
			{
				// Token: 0x0400DE40 RID: 56896
				public static LocString NAME = "Stored Geodata: {0}";

				// Token: 0x0400DE41 RID: 56897
				public static LocString TOOLTIP = UI.PRE_KEYWORD + "Dowsing Boosters" + UI.PST_KEYWORD + " retain geodata gathered by Bionic Duplicants\n\nWhen dowsing is complete and this booster is installed in a Bionic Duplicant, a new geyser will be revealed";
			}

			// Token: 0x020039A4 RID: 14756
			public class BIONICEXPLORERBOOSTERREADY
			{
				// Token: 0x0400DE42 RID: 56898
				public static LocString NAME = "Dowsing Complete";

				// Token: 0x0400DE43 RID: 56899
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This ",
					UI.PRE_KEYWORD,
					"Dowsing Booster",
					UI.PST_KEYWORD,
					" has sufficient geodata stored to reveal a new geyser\n\nIt must be installed in a Bionic Duplicant in order to function"
				});
			}

			// Token: 0x020039A5 RID: 14757
			public class STOREDITEMDURABILITY
			{
				// Token: 0x0400DE44 RID: 56900
				public static LocString NAME = "Durability: {durability}";

				// Token: 0x0400DE45 RID: 56901
				public static LocString TOOLTIP = "Items lose durability each time they are equipped, and can no longer be put on by a Duplicant once they reach 0% durability\n\nRepair of this item can be done in the appropriate fabrication station";
			}

			// Token: 0x020039A6 RID: 14758
			public class ARTIFACTENTOMBED
			{
				// Token: 0x0400DE46 RID: 56902
				public static LocString NAME = "Entombed Artifact";

				// Token: 0x0400DE47 RID: 56903
				public static LocString TOOLTIP = "This artifact is trapped in an obscuring shell limiting its decor. A skilled artist can remove it at the " + BUILDINGS.PREFABS.ARTIFACTANALYSISSTATION.NAME;
			}

			// Token: 0x020039A7 RID: 14759
			public class TEAROPEN
			{
				// Token: 0x0400DE48 RID: 56904
				public static LocString NAME = "Temporal Tear open";

				// Token: 0x0400DE49 RID: 56905
				public static LocString TOOLTIP = "An open passage through spacetime";
			}

			// Token: 0x020039A8 RID: 14760
			public class TEARCLOSED
			{
				// Token: 0x0400DE4A RID: 56906
				public static LocString NAME = "Temporal Tear closed";

				// Token: 0x0400DE4B RID: 56907
				public static LocString TOOLTIP = "Perhaps some technology could open the passage";
			}

			// Token: 0x020039A9 RID: 14761
			public class MARKEDFORMOVE
			{
				// Token: 0x0400DE4C RID: 56908
				public static LocString NAME = "Pending Move";

				// Token: 0x0400DE4D RID: 56909
				public static LocString TOOLTIP = "Waiting for a Duplicant to move this object";
			}

			// Token: 0x020039AA RID: 14762
			public class MOVESTORAGEUNREACHABLE
			{
				// Token: 0x0400DE4E RID: 56910
				public static LocString NAME = "Unreachable Move";

				// Token: 0x0400DE4F RID: 56911
				public static LocString TOOLTIP = "Duplicants cannot reach this object to move it";
			}

			// Token: 0x020039AB RID: 14763
			public class PENDINGCARVE
			{
				// Token: 0x0400DE50 RID: 56912
				public static LocString NAME = "Carve Errand";

				// Token: 0x0400DE51 RID: 56913
				public static LocString TOOLTIP = "Rock will be carved once a Duplicant is available";
			}
		}

		// Token: 0x020039AC RID: 14764
		public class POPFX
		{
			// Token: 0x0400DE52 RID: 56914
			public static LocString RESOURCE_EATEN = "Resource Eaten";

			// Token: 0x0400DE53 RID: 56915
			public static LocString RESOURCE_SELECTION_CHANGED = "Changed to {0}";

			// Token: 0x0400DE54 RID: 56916
			public static LocString EXTRA_POWERBANKS_BIONIC = "Extra Power Banks";
		}

		// Token: 0x020039AD RID: 14765
		public class NOTIFICATIONS
		{
			// Token: 0x020039AE RID: 14766
			public class BASICCONTROLS
			{
				// Token: 0x0400DE55 RID: 56917
				public static LocString NAME = "Tutorial: Basic Controls";

				// Token: 0x0400DE56 RID: 56918
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					"• I can use ",
					UI.FormatAsHotKey(global::Action.PanLeft),
					" and ",
					UI.FormatAsHotKey(global::Action.PanRight),
					" to pan my view left and right, and ",
					UI.FormatAsHotKey(global::Action.PanUp),
					" and ",
					UI.FormatAsHotKey(global::Action.PanDown),
					" to pan up and down.\n\n• ",
					UI.FormatAsHotKey(global::Action.ZoomIn),
					" lets me zoom in, and ",
					UI.FormatAsHotKey(global::Action.ZoomOut),
					" zooms out.\n\n• ",
					UI.FormatAsHotKey(global::Action.CameraHome),
					" returns my view to the Printing Pod.\n\n• I can speed or slow my perception of time using the top left corner buttons, or by pressing ",
					UI.FormatAsHotKey(global::Action.SpeedUp),
					" or ",
					UI.FormatAsHotKey(global::Action.SlowDown),
					". Pressing ",
					UI.FormatAsHotKey(global::Action.TogglePause),
					" will pause the flow of time entirely.\n\n• I'll keep records of everything I discover in my personal DATABASE ",
					UI.FormatAsHotKey(global::Action.ManageDatabase),
					" to refer back to if I forget anything important."
				});

				// Token: 0x0400DE57 RID: 56919
				public static LocString MESSAGEBODYALT = string.Concat(new string[]
				{
					"• I can use ",
					UI.FormatAsHotKey(global::Action.AnalogCamera),
					" to pan my view.\n\n• ",
					UI.FormatAsHotKey(global::Action.ZoomIn),
					" lets me zoom in, and ",
					UI.FormatAsHotKey(global::Action.ZoomOut),
					" zooms out.\n\n• I can speed or slow my perception of time using the top left corner buttons, or by pressing ",
					UI.FormatAsHotKey(global::Action.CycleSpeed),
					". Pressing ",
					UI.FormatAsHotKey(global::Action.TogglePause),
					" will pause the flow of time entirely.\n\n• I'll keep records of everything I discover in my personal DATABASE ",
					UI.FormatAsHotKey(global::Action.ManageDatabase),
					" to refer back to if I forget anything important."
				});

				// Token: 0x0400DE58 RID: 56920
				public static LocString TOOLTIP = "Notes on using my HUD";
			}

			// Token: 0x020039AF RID: 14767
			public class CODEXUNLOCK
			{
				// Token: 0x0400DE59 RID: 56921
				public static LocString NAME = "New Log Entry";

				// Token: 0x0400DE5A RID: 56922
				public static LocString MESSAGEBODY = "I've added a new log entry to my Database";

				// Token: 0x0400DE5B RID: 56923
				public static LocString TOOLTIP = "I've added a new log entry to my Database";
			}

			// Token: 0x020039B0 RID: 14768
			public class WELCOMEMESSAGE
			{
				// Token: 0x0400DE5C RID: 56924
				public static LocString NAME = "Tutorial: Colony Management";

				// Token: 0x0400DE5D RID: 56925
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					"I can use the ",
					UI.FormatAsTool("Dig Tool", global::Action.Dig),
					" and the ",
					UI.FormatAsBuildMenuTab("Build Menu"),
					" in the lower left of the screen to begin planning my first construction tasks.\n\nOnce I've placed a few errands my Duplicants will automatically get to work, without me needing to direct them individually."
				});

				// Token: 0x0400DE5E RID: 56926
				public static LocString TOOLTIP = "Notes on getting Duplicants to do my bidding";
			}

			// Token: 0x020039B1 RID: 14769
			public class STRESSMANAGEMENTMESSAGE
			{
				// Token: 0x0400DE5F RID: 56927
				public static LocString NAME = "Tutorial: Stress Management";

				// Token: 0x0400DE60 RID: 56928
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					"At 100% ",
					UI.FormatAsLink("Stress", "STRESS"),
					", a Duplicant will have a nervous breakdown and be unable to work.\n\nBreakdowns can manifest in different colony-threatening ways, such as the destruction of buildings or the binge eating of food.\n\nI can help my Duplicants manage stressful situations by giving them access to good ",
					UI.FormatAsLink("Food", "FOOD"),
					", fancy ",
					UI.FormatAsLink("Decor", "DECOR"),
					" and comfort items which boost their ",
					UI.FormatAsLink("Morale", "MORALE"),
					".\n\nI can select a Duplicant and mouse over ",
					UI.FormatAsLink("Stress", "STRESS"),
					" or ",
					UI.FormatAsLink("Morale", "MORALE"),
					" in their CONDITION TAB to view current statuses, and hopefully manage things before they become a problem.\n\nRelated ",
					UI.FormatAsLink("Video: Duplicant Morale", "VIDEOS13"),
					" "
				});

				// Token: 0x0400DE61 RID: 56929
				public static LocString TOOLTIP = "Notes on keeping Duplicants happy and productive";
			}

			// Token: 0x020039B2 RID: 14770
			public class TASKPRIORITIESMESSAGE
			{
				// Token: 0x0400DE62 RID: 56930
				public static LocString NAME = "Tutorial: Priority";

				// Token: 0x0400DE63 RID: 56931
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					"Duplicants always perform errands in order of highest to lowest priority. They will harvest ",
					UI.FormatAsLink("Food", "FOOD"),
					" before they build, for example, or always build new structures before they mine materials.\n\nI can open the ",
					UI.FormatAsManagementMenu("Priorities Screen", global::Action.ManagePriorities),
					" to set which Errand Types Duplicants may or may not perform, or to specialize skilled Duplicants for particular Errand Types."
				});

				// Token: 0x0400DE64 RID: 56932
				public static LocString TOOLTIP = "Notes on managing Duplicants' errands";
			}

			// Token: 0x020039B3 RID: 14771
			public class MOPPINGMESSAGE
			{
				// Token: 0x0400DE65 RID: 56933
				public static LocString NAME = "Tutorial: Polluted Water";

				// Token: 0x0400DE66 RID: 56934
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					UI.FormatAsLink("Polluted Water", "DIRTYWATER"),
					" slowly emits ",
					UI.FormatAsLink("Polluted Oxygen", "CONTAMINATEDOXYGEN"),
					" which accelerates the spread of ",
					UI.FormatAsLink("Disease", "DISEASE"),
					".\n\nDuplicants will also be ",
					UI.FormatAsLink("Stressed", "STRESS"),
					" by walking through Polluted Water, so I should have my Duplicants clean up spills by ",
					UI.CLICK(UI.ClickType.clicking),
					" and dragging the ",
					UI.FormatAsTool("Mop Tool", global::Action.Mop)
				});

				// Token: 0x0400DE67 RID: 56935
				public static LocString TOOLTIP = "Notes on handling polluted materials";
			}

			// Token: 0x020039B4 RID: 14772
			public class LOCOMOTIONMESSAGE
			{
				// Token: 0x0400DE68 RID: 56936
				public static LocString NAME = "Video: Duplicant Movement";

				// Token: 0x0400DE69 RID: 56937
				public static LocString MESSAGEBODY = "Duplicants have limited jumping and climbing abilities. They can only climb two tiles high and cannot fit into spaces shorter than two tiles, or cross gaps wider than one tile. I should keep this in mind while placing errands.\n\nTo check if an errand I've placed is accessible, I can select a Duplicant and " + UI.CLICK(UI.ClickType.click) + " <b>Show Navigation</b> to view all areas within their reach.";

				// Token: 0x0400DE6A RID: 56938
				public static LocString TOOLTIP = "Notes on my Duplicants' maneuverability";
			}

			// Token: 0x020039B5 RID: 14773
			public class PRIORITIESMESSAGE
			{
				// Token: 0x0400DE6B RID: 56939
				public static LocString NAME = "Tutorial: Errand Priorities";

				// Token: 0x0400DE6C RID: 56940
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					"Duplicants will choose where to work based on the priority of the errands that I give them. I can open the ",
					UI.FormatAsManagementMenu("Priorities Screen", global::Action.ManagePriorities),
					" to set their ",
					UI.PRE_KEYWORD,
					"Duplicant Priorities",
					UI.PST_KEYWORD,
					", and the ",
					UI.FormatAsTool("Priority Tool", global::Action.Prioritize),
					" to fine tune ",
					UI.PRE_KEYWORD,
					"Building Priority",
					UI.PST_KEYWORD,
					". Many buildings will also let me change their Priority level when I select them."
				});

				// Token: 0x0400DE6D RID: 56941
				public static LocString TOOLTIP = "Notes on my Duplicants' priorities";
			}

			// Token: 0x020039B6 RID: 14774
			public class FETCHINGWATERMESSAGE
			{
				// Token: 0x0400DE6E RID: 56942
				public static LocString NAME = "Tutorial: Fetching Water";

				// Token: 0x0400DE6F RID: 56943
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					"By building a ",
					UI.FormatAsLink("Pitcher Pump", "LIQUIDPUMPINGSTATION"),
					" from the ",
					UI.FormatAsBuildMenuTab("Plumbing Tab", global::Action.Plan5),
					" over a pool of liquid, my Duplicants will be able to bottle it up and manually deliver it wherever it needs to go."
				});

				// Token: 0x0400DE70 RID: 56944
				public static LocString TOOLTIP = "Notes on liquid resource gathering";
			}

			// Token: 0x020039B7 RID: 14775
			public class SCHEDULEMESSAGE
			{
				// Token: 0x0400DE71 RID: 56945
				public static LocString NAME = "Tutorial: Scheduling";

				// Token: 0x0400DE72 RID: 56946
				public static LocString MESSAGEBODY = "My Duplicants will only eat, sleep, work, or bathe during the times I allot for such activities.\n\nTo make the best use of their time, I can open the " + UI.FormatAsManagementMenu("Schedule Tab", global::Action.ManageSchedule) + " to adjust the colony's schedule and plan how they should utilize their day.";

				// Token: 0x0400DE73 RID: 56947
				public static LocString TOOLTIP = "Notes on scheduling my Duplicants' time";
			}

			// Token: 0x020039B8 RID: 14776
			public class THERMALCOMFORT
			{
				// Token: 0x0400DE74 RID: 56948
				public static LocString NAME = "Tutorial: Duplicant Temperature";

				// Token: 0x0400DE75 RID: 56949
				public static LocString TOOLTIP = "Notes on helping Duplicants keep their cool";

				// Token: 0x0400DE76 RID: 56950
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					"Environments that are extremely ",
					UI.FormatAsLink("Hot", "HEAT"),
					" or ",
					UI.FormatAsLink("Cold", "HEAT"),
					" affect my Duplicants' internal body temperature and cause undue ",
					UI.FormatAsLink("Stress", "STRESS"),
					" or unscheduled naps.\n\nOpening the ",
					UI.FormatAsOverlay("Temperature Overlay", global::Action.Overlay3),
					" and checking the <b>Thermal Tolerance</b> box allows me to view all areas where my Duplicants will feel discomfort and be unable to regulate their internal body temperature.\n\nRelated ",
					UI.FormatAsLink("Video: Insulation", "VIDEOS17")
				});
			}

			// Token: 0x020039B9 RID: 14777
			public class TUTORIAL_OVERHEATING
			{
				// Token: 0x0400DE77 RID: 56951
				public static LocString NAME = "Tutorial: Building Temperature";

				// Token: 0x0400DE78 RID: 56952
				public static LocString TOOLTIP = "Notes on preventing building from breaking";

				// Token: 0x0400DE79 RID: 56953
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					"When constructing buildings, I should always take note of their ",
					UI.FormatAsLink("Overheat Temperature", "HEAT"),
					" and plan their locations accordingly. Maintaining low ambient temperatures and good ventilation in the colony will also help keep building temperatures down.\n\nThe <b>Relative Temperature</b> slider tool in the ",
					UI.FormatAsOverlay("Temperature Overlay", global::Action.Overlay3),
					" allows me to change adjust the overlay's color-coding in order to highlight specific temperature ranges.\n\nIf I allow buildings to exceed their Overheat Temperature they will begin to take damage, and if left unattended, they will break down and be unusable until repaired."
				});
			}

			// Token: 0x020039BA RID: 14778
			public class LOTS_OF_GERMS
			{
				// Token: 0x0400DE7A RID: 56954
				public static LocString NAME = "Tutorial: Germs and Disease";

				// Token: 0x0400DE7B RID: 56955
				public static LocString TOOLTIP = "Notes on Duplicant disease risks";

				// Token: 0x0400DE7C RID: 56956
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					UI.FormatAsLink("Germs", "DISEASE"),
					" such as ",
					UI.FormatAsLink("Food Poisoning", "FOODSICKNESS"),
					" and ",
					UI.FormatAsLink("Slimelung", "SLIMESICKNESS"),
					" can cause ",
					UI.FormatAsLink("Disease", "DISEASE"),
					" in my Duplicants. I can use the ",
					UI.FormatAsOverlay("Germ Overlay", global::Action.Overlay9),
					" to view all germ concentrations in my colony, and even detect the sources spawning them.\n\nBuilding Wash Basins from the ",
					UI.FormatAsBuildMenuTab("Medicine Tab", global::Action.Plan8),
					" near colony toilets will tell my Duplicants they need to wash up.\n\nRelated ",
					UI.FormatAsLink("Video: Plumbing and Ventilation", "VIDEOS18")
				});
			}

			// Token: 0x020039BB RID: 14779
			public class BEING_INFECTED
			{
				// Token: 0x0400DE7D RID: 56957
				public static LocString NAME = "Tutorial: Immune Systems";

				// Token: 0x0400DE7E RID: 56958
				public static LocString TOOLTIP = "Notes on keeping Duplicants in peak health";

				// Token: 0x0400DE7F RID: 56959
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					"When Duplicants come into contact with various ",
					UI.FormatAsLink("Germs", "DISEASE"),
					", they'll need to expend points of ",
					UI.FormatAsLink("Immunity", "IMMUNE SYSTEM"),
					" to resist them and remain healthy. If repeated exposes causes their Immunity to drop to 0%, they'll be unable to resist germs and will contract the next disease they encounter.\n\nDoors with Access Permissions can be built from the BASE TAB<color=#F44A47> <b>[1]</b></color> of the ",
					UI.FormatAsLink("Build menu", "misc"),
					" to block Duplicants from entering biohazardous areas while they recover their spent immunity points."
				});
			}

			// Token: 0x020039BC RID: 14780
			public class DISEASE_COOKING
			{
				// Token: 0x0400DE80 RID: 56960
				public static LocString NAME = "Tutorial: Food Safety";

				// Token: 0x0400DE81 RID: 56961
				public static LocString TOOLTIP = "Notes on managing food contamination";

				// Token: 0x0400DE82 RID: 56962
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					"The ",
					UI.FormatAsLink("Food", "FOOD"),
					" my Duplicants cook will only ever be as clean as the ingredients used to make it. Storing food in sterile or ",
					UI.FormatAsLink("Refrigerated", "REFRIGERATOR"),
					" environments will keep food free of ",
					UI.FormatAsLink("Germs", "DISEASE"),
					", while carefully placed hygiene stations like ",
					BUILDINGS.PREFABS.WASHBASIN.NAME,
					" or ",
					BUILDINGS.PREFABS.SHOWER.NAME,
					" will prevent the cooks from infecting the food by handling it.\n\nDangerously contaminated food can be sent to compost by ",
					UI.CLICK(UI.ClickType.clicking),
					" the <b>Compost</b> button on the selected item."
				});
			}

			// Token: 0x020039BD RID: 14781
			public class SUITS
			{
				// Token: 0x0400DE83 RID: 56963
				public static LocString NAME = "Tutorial: Atmo Suits";

				// Token: 0x0400DE84 RID: 56964
				public static LocString TOOLTIP = "Notes on using atmo suits";

				// Token: 0x0400DE85 RID: 56965
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					UI.FormatAsLink("Atmo Suits", "ATMO_SUIT"),
					" can be equipped to protect my Duplicants from environmental hazards like extreme ",
					UI.FormatAsLink("Heat", "Heat"),
					", airborne ",
					UI.FormatAsLink("Germs", "DISEASE"),
					", or unbreathable ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					". In order to utilize these suits, I'll need to hook up an ",
					UI.FormatAsLink("Atmo Suit Dock", "SUITLOCKER"),
					" to an ",
					UI.FormatAsLink("Atmo Suit Checkpoint", "SUITMARKER"),
					" , then store one of the suits inside.\n\nDuplicants will equip a suit when they walk past the checkpoint in the chosen direction, and will unequip their suit when walking back the opposite way."
				});
			}

			// Token: 0x020039BE RID: 14782
			public class RADIATION
			{
				// Token: 0x0400DE86 RID: 56966
				public static LocString NAME = "Tutorial: Radiation";

				// Token: 0x0400DE87 RID: 56967
				public static LocString TOOLTIP = "Notes on managing radiation";

				// Token: 0x0400DE88 RID: 56968
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					"Objects such as ",
					UI.FormatAsLink("Uranium Ore", "URANIUMORE"),
					" and ",
					UI.FormatAsLink("Beeta Hives", "BEE"),
					" emit a ",
					UI.FormatAsLink("Radioactive", "RADIOACTIVE"),
					" energy that can be toxic to my Duplicants.\n\nI can use the ",
					UI.FormatAsOverlay("Radiation Overlay"),
					" ",
					UI.FormatAsHotKey(global::Action.Overlay15),
					" to check the scope of the Radiation field. Building thick walls around radiation emitters will dampen the field and protect my Duplicants from getting ",
					UI.FormatAsLink("Radiation Sickness", "RADIATIONSICKNESS"),
					" ."
				});
			}

			// Token: 0x020039BF RID: 14783
			public class SPACETRAVEL
			{
				// Token: 0x0400DE89 RID: 56969
				public static LocString NAME = "Tutorial: Space Travel";

				// Token: 0x0400DE8A RID: 56970
				public static LocString TOOLTIP = "Notes on traveling in space";

				// Token: 0x0400DE8B RID: 56971
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					"Building a rocket first requires constructing a ",
					UI.FormatAsLink("Rocket Platform", "LAUNCHPAD"),
					" and adding modules from the menu. All components of the Rocket Checklist will need to be complete before being capable of launching.\n\nA ",
					UI.FormatAsLink("Telescope", "CLUSTERTELESCOPE"),
					" needs to be built on the surface of a Planetoid in order to use the ",
					UI.PRE_KEYWORD,
					"Starmap Screen",
					UI.PST_KEYWORD,
					" ",
					UI.FormatAsHotKey(global::Action.ManageStarmap),
					" to see and set course for new destinations."
				});
			}

			// Token: 0x020039C0 RID: 14784
			public class MORALE
			{
				// Token: 0x0400DE8C RID: 56972
				public static LocString NAME = "Video: Duplicant Morale";

				// Token: 0x0400DE8D RID: 56973
				public static LocString TOOLTIP = "Notes on Duplicant expectations";

				// Token: 0x0400DE8E RID: 56974
				public static LocString MESSAGEBODY = "Food, Rooms, Decor, and Recreation all have an effect on Duplicant Morale. Good experiences improve their Morale, while poor experiences lower it. When a Duplicant's Morale is below their Expectations, they will become Stressed.\n\nDuplicants' Expectations will get higher as they are given new Skills, and the colony will have to be improved to keep up their Morale. An overview of Morale and Stress can be viewed on the Vitals screen.\n\nRelated " + UI.FormatAsLink("Tutorial: Stress Management", "MISCELLANEOUSTIPS");
			}

			// Token: 0x020039C1 RID: 14785
			public class POWER
			{
				// Token: 0x0400DE8F RID: 56975
				public static LocString NAME = "Video: Power Circuits";

				// Token: 0x0400DE90 RID: 56976
				public static LocString TOOLTIP = "Notes on managing electricity";

				// Token: 0x0400DE91 RID: 56977
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					"Generators are considered \"Producers\" of Power, while the various buildings and machines in the colony are considered \"Consumers\". Each Consumer will pull a certain wattage from the power circuit it is connected to, which can be checked at any time by ",
					UI.CLICK(UI.ClickType.clicking),
					" the building and going to the Energy Tab.\n\nI can use the Power Overlay ",
					UI.FormatAsHotKey(global::Action.Overlay2),
					" to quickly check the status of all my circuits. If the Consumers are taking more wattage than the Generators are creating, the Batteries will drain and there will be brownouts.\n\nAdditionally, if the Consumers are pulling more wattage through the Wires than the Wires can handle, they will overload and burn out. To correct both these situations, I will need to reorganize my Consumers onto separate circuits."
				});
			}

			// Token: 0x020039C2 RID: 14786
			public class BIONICBATTERY
			{
				// Token: 0x0400DE92 RID: 56978
				public static LocString NAME = "Tutorial: Powering Bionics";

				// Token: 0x0400DE93 RID: 56979
				public static LocString TOOLTIP = "Notes on Duplicant power bank needs";

				// Token: 0x0400DE94 RID: 56980
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					"Bionic Duplicants require ",
					UI.FormatAsLink("Power Banks", "ELECTROBANK"),
					" to function. Bionic Duplicants who run out of ",
					UI.FormatAsLink("Power", "POWER"),
					" will become incapacitated and require another Duplicant to reboot them.\n\nBasic power banks can be made at the ",
					UI.FormatAsLink("Crafting Station", "CRAFTINGTABLE"),
					"."
				});
			}

			// Token: 0x020039C3 RID: 14787
			public class GUNKEDTOILET
			{
				// Token: 0x0400DE95 RID: 56981
				public static LocString NAME = "Tutorial: Clogged Toilets";

				// Token: 0x0400DE96 RID: 56982
				public static LocString TOOLTIP = "Notes on unclogging toilets";

				// Token: 0x0400DE97 RID: 56983
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					"Bionic Duplicants can dump built-up ",
					UI.FormatAsLink("Liquid Gunk", "LIQUIDGUNK"),
					" into ",
					UI.FormatAsLink("Toilets", "BUILDCATEGORYREQUIREMENTCLASSTOILETTYPE"),
					" if no other options are available. This invariably clogs the plumbing, however, and must be removed before facilities can be used by other Duplicants.\n\nBuilding a ",
					UI.FormatAsLink("Gunk Extractor", "GUNKEMPTIER"),
					" from the ",
					UI.FormatAsBuildMenuTab("Plumbing Tab", global::Action.Plan5),
					" will ensure that Bionic Duplicants can dispose of their waste appropriately."
				});
			}

			// Token: 0x020039C4 RID: 14788
			public class SLIPPERYSURFACE
			{
				// Token: 0x0400DE98 RID: 56984
				public static LocString NAME = "Tutorial: Wet Surfaces";

				// Token: 0x0400DE99 RID: 56985
				public static LocString TOOLTIP = "Notes on slipping hazards";

				// Token: 0x0400DE9A RID: 56986
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					"My Duplicants may slip and fall on wet surfaces. I can help them avoid undue ",
					UI.FormatAsLink("Stress", "STRESS"),
					" and potential injury by using the ",
					UI.FormatAsTool("Mop Tool", global::Action.Mop),
					" to clean up spills. Building ",
					UI.FormatAsLink("Toilets", "BUILDCATEGORYREQUIREMENTCLASSTOILETTYPE"),
					" can help minimize the incidence of spills."
				});
			}

			// Token: 0x020039C5 RID: 14789
			public class BIONICOIL
			{
				// Token: 0x0400DE9B RID: 56987
				public static LocString NAME = "Tutorial: Oiling Bionics";

				// Token: 0x0400DE9C RID: 56988
				public static LocString TOOLTIP = "Notes on keeping Bionics working efficiently";

				// Token: 0x0400DE9D RID: 56989
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					"Bionic Duplicants with insufficient ",
					UI.FormatAsLink("Gear Oil", "LUBRICATINGOIL"),
					" will slow down significantly to avoid grinding their gears.\n\nI can keep them running smoothly by producing ",
					UI.FormatAsLink("Phyto Oil", "PHYTOOIL"),
					" out of ",
					UI.FormatAsLink("Slime", "SLIME"),
					" and building a ",
					UI.FormatAsLink("Lubrication Station", "OILCHANGER"),
					" from the ",
					UI.FormatAsBuildMenuTab("Medicine Tab", global::Action.Plan8),
					"."
				});
			}

			// Token: 0x020039C6 RID: 14790
			public class DIGGING
			{
				// Token: 0x0400DE9E RID: 56990
				public static LocString NAME = "Video: Digging for Resources";

				// Token: 0x0400DE9F RID: 56991
				public static LocString TOOLTIP = "Notes on buried riches";

				// Token: 0x0400DEA0 RID: 56992
				public static LocString MESSAGEBODY = "Everything a colony needs to get going is found in the ground. Instructing Duplicants to dig out areas means we can find food, mine resources to build infrastructure, and clear space for the colony to grow. I can access the Dig Tool with " + UI.FormatAsHotKey(global::Action.Dig) + ", which allows me to select the area where I want my Duplicants to dig.\n\nDuplicants will need to gain the Superhard Digging skill to mine Abyssalite and the Superduperhard Digging skill to mine Diamond and Obsidian. Without the proper skills, these materials will be undiggable.";
			}

			// Token: 0x020039C7 RID: 14791
			public class INSULATION
			{
				// Token: 0x0400DEA1 RID: 56993
				public static LocString NAME = "Video: Insulation";

				// Token: 0x0400DEA2 RID: 56994
				public static LocString TOOLTIP = "Notes on effective temperature management";

				// Token: 0x0400DEA3 RID: 56995
				public static LocString MESSAGEBODY = "The temperature of an environment can have positive or negative effects on the well-being of my Duplicants, as well as the plants and critters in my colony. Selecting " + UI.FormatAsHotKey(global::Action.Overlay3) + " will open the Temperature Overlay where I can check for any hot or cold spots.\n\nI can use a Utility building like an Ice-E Fan or a Space Heater to make an area colder or warmer. However, I will have limited success changing the temperature of a room unless I build the area with insulating tiles to prevent cold or warm air from escaping.";
			}

			// Token: 0x020039C8 RID: 14792
			public class PLUMBING
			{
				// Token: 0x0400DEA4 RID: 56996
				public static LocString NAME = "Video: Plumbing and Ventilation";

				// Token: 0x0400DEA5 RID: 56997
				public static LocString TOOLTIP = "Notes on connecting buildings with pipes";

				// Token: 0x0400DEA6 RID: 56998
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					"When connecting pipes for plumbing, it is useful to have the Plumbing Overlay ",
					UI.FormatAsHotKey(global::Action.Overlay6),
					" selected. Each building which requires plumbing must have their Building Intake connected to the Output Pipe from a source such as a Liquid Pump. Liquid Pumps must be submerged in liquid and attached to a power source to function.\n\nBuildings often output contaminated water which must flow out of the building through piping from the Output Pipe. The water can then be expelled through a Liquid Vent, or filtered through a Water Sieve for reuse.\n\nVentilation applies the same principles to gases. Select the Ventilation Overlay ",
					UI.FormatAsHotKey(global::Action.Overlay7),
					" to see how gases are being moved around the colony."
				});
			}

			// Token: 0x020039C9 RID: 14793
			public class NEW_AUTOMATION_WARNING
			{
				// Token: 0x0400DEA7 RID: 56999
				public static LocString NAME = "New Automation Port";

				// Token: 0x0400DEA8 RID: 57000
				public static LocString TOOLTIP = "This building has a new automation port and is unintentionally connected to an existing " + BUILDINGS.PREFABS.LOGICWIRE.NAME;
			}

			// Token: 0x020039CA RID: 14794
			public class DTU
			{
				// Token: 0x0400DEA9 RID: 57001
				public static LocString NAME = "Tutorial: Duplicant Thermal Units";

				// Token: 0x0400DEAA RID: 57002
				public static LocString TOOLTIP = "Notes on measuring heat energy";

				// Token: 0x0400DEAB RID: 57003
				public static LocString MESSAGEBODY = "My Duplicants measure heat energy in Duplicant Thermal Units or DTU.\n\n1 DTU = 1055.06 J";
			}

			// Token: 0x020039CB RID: 14795
			public class NOMESSAGES
			{
				// Token: 0x0400DEAC RID: 57004
				public static LocString NAME = "";

				// Token: 0x0400DEAD RID: 57005
				public static LocString TOOLTIP = "";
			}

			// Token: 0x020039CC RID: 14796
			public class NOALERTS
			{
				// Token: 0x0400DEAE RID: 57006
				public static LocString NAME = "";

				// Token: 0x0400DEAF RID: 57007
				public static LocString TOOLTIP = "";
			}

			// Token: 0x020039CD RID: 14797
			public class NEWTRAIT
			{
				// Token: 0x0400DEB0 RID: 57008
				public static LocString NAME = "{0} has developed a trait";

				// Token: 0x0400DEB1 RID: 57009
				public static LocString TOOLTIP = "{0} has developed the trait(s):\n    • {1}";
			}

			// Token: 0x020039CE RID: 14798
			public class RESEARCHCOMPLETE
			{
				// Token: 0x0400DEB2 RID: 57010
				public static LocString NAME = "Research Complete";

				// Token: 0x0400DEB3 RID: 57011
				public static LocString MESSAGEBODY = "Eureka! We've discovered {0} Technology.\n\nNew buildings have become available:\n  • {1}";

				// Token: 0x0400DEB4 RID: 57012
				public static LocString TOOLTIP = "{0} research complete!";
			}

			// Token: 0x020039CF RID: 14799
			public class WORLDDETECTED
			{
				// Token: 0x0400DEB5 RID: 57013
				public static LocString NAME = "New " + UI.CLUSTERMAP.PLANETOID + " detected";

				// Token: 0x0400DEB6 RID: 57014
				public static LocString MESSAGEBODY = "My Duplicants' astronomical efforts have uncovered a new " + UI.CLUSTERMAP.PLANETOID + ":\n{0}";

				// Token: 0x0400DEB7 RID: 57015
				public static LocString TOOLTIP = "{0} discovered";
			}

			// Token: 0x020039D0 RID: 14800
			public class SKILL_POINT_EARNED
			{
				// Token: 0x0400DEB8 RID: 57016
				public static LocString NAME = "{Duplicant} earned a skill point!";

				// Token: 0x0400DEB9 RID: 57017
				public static LocString MESSAGEBODY = "These Duplicants have Skill Points that can be spent on new abilities:\n{0}";

				// Token: 0x0400DEBA RID: 57018
				public static LocString LINE = "\n• <b>{0}</b>";

				// Token: 0x0400DEBB RID: 57019
				public static LocString TOOLTIP = "{Duplicant} has been working hard and is ready to learn a new skill";
			}

			// Token: 0x020039D1 RID: 14801
			public class DUPLICANTABSORBED
			{
				// Token: 0x0400DEBC RID: 57020
				public static LocString NAME = "Printables have been reabsorbed";

				// Token: 0x0400DEBD RID: 57021
				public static LocString MESSAGEBODY = "The Printing Pod is no longer available for printing.\nCountdown to the next production has been rebooted.";

				// Token: 0x0400DEBE RID: 57022
				public static LocString TOOLTIP = "Printing countdown rebooted";
			}

			// Token: 0x020039D2 RID: 14802
			public class DUPLICANTDIED
			{
				// Token: 0x0400DEBF RID: 57023
				public static LocString NAME = "Duplicants have died";

				// Token: 0x0400DEC0 RID: 57024
				public static LocString TOOLTIP = "These Duplicants have died:";
			}

			// Token: 0x020039D3 RID: 14803
			public class FOODROT
			{
				// Token: 0x0400DEC1 RID: 57025
				public static LocString NAME = "Food has decayed";

				// Token: 0x0400DEC2 RID: 57026
				public static LocString TOOLTIP = "These " + UI.FormatAsLink("Food", "FOOD") + " items have rotted and are no longer edible:{0}";
			}

			// Token: 0x020039D4 RID: 14804
			public class FOODSTALE
			{
				// Token: 0x0400DEC3 RID: 57027
				public static LocString NAME = "Food has become stale";

				// Token: 0x0400DEC4 RID: 57028
				public static LocString TOOLTIP = "These " + UI.FormatAsLink("Food", "FOOD") + " items have become stale and could rot if not stored:";
			}

			// Token: 0x020039D5 RID: 14805
			public class YELLOWALERT
			{
				// Token: 0x0400DEC5 RID: 57029
				public static LocString NAME = "Yellow Alert";

				// Token: 0x0400DEC6 RID: 57030
				public static LocString TOOLTIP = "The colony has some top priority tasks to complete before resuming a normal schedule";
			}

			// Token: 0x020039D6 RID: 14806
			public class REDALERT
			{
				// Token: 0x0400DEC7 RID: 57031
				public static LocString NAME = "Red Alert";

				// Token: 0x0400DEC8 RID: 57032
				public static LocString TOOLTIP = "The colony is prioritizing work over their individual well-being";
			}

			// Token: 0x020039D7 RID: 14807
			public class REACTORMELTDOWN
			{
				// Token: 0x0400DEC9 RID: 57033
				public static LocString NAME = "Reactor Meltdown";

				// Token: 0x0400DECA RID: 57034
				public static LocString TOOLTIP = "A Research Reactor has overheated and is melting down! Extreme radiation is flooding the area";
			}

			// Token: 0x020039D8 RID: 14808
			public class HEALING
			{
				// Token: 0x0400DECB RID: 57035
				public static LocString NAME = "Healing";

				// Token: 0x0400DECC RID: 57036
				public static LocString TOOLTIP = "This Duplicant is recovering from an injury";
			}

			// Token: 0x020039D9 RID: 14809
			public class UNREACHABLEITEM
			{
				// Token: 0x0400DECD RID: 57037
				public static LocString NAME = "Unreachable resources";

				// Token: 0x0400DECE RID: 57038
				public static LocString TOOLTIP = "Duplicants cannot retrieve these resources:";
			}

			// Token: 0x020039DA RID: 14810
			public class INVALIDCONSTRUCTIONLOCATION
			{
				// Token: 0x0400DECF RID: 57039
				public static LocString NAME = "Invalid construction location";

				// Token: 0x0400DED0 RID: 57040
				public static LocString TOOLTIP = "These buildings cannot be constructed in the planned areas:";
			}

			// Token: 0x020039DB RID: 14811
			public class MISSINGMATERIALS
			{
				// Token: 0x0400DED1 RID: 57041
				public static LocString NAME = "Missing materials";

				// Token: 0x0400DED2 RID: 57042
				public static LocString TOOLTIP = "These resources are not available:";
			}

			// Token: 0x020039DC RID: 14812
			public class BUILDINGOVERHEATED
			{
				// Token: 0x0400DED3 RID: 57043
				public static LocString NAME = "Damage: Overheated";

				// Token: 0x0400DED4 RID: 57044
				public static LocString TOOLTIP = "Extreme heat is damaging these buildings:";
			}

			// Token: 0x020039DD RID: 14813
			public class TILECOLLAPSE
			{
				// Token: 0x0400DED5 RID: 57045
				public static LocString NAME = "Ceiling Collapse!";

				// Token: 0x0400DED6 RID: 57046
				public static LocString TOOLTIP = "Falling material fell on these Duplicants and displaced them:";
			}

			// Token: 0x020039DE RID: 14814
			public class NO_OXYGEN_GENERATOR
			{
				// Token: 0x0400DED7 RID: 57047
				public static LocString NAME = "No " + UI.FormatAsLink("Oxygen Generator", "OXYGEN") + " built";

				// Token: 0x0400DED8 RID: 57048
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"My colony is not producing any new ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					"\n\n",
					UI.FormatAsLink("Oxygen Diffusers", "MINERALDEOXIDIZER"),
					" can be built from the ",
					UI.FormatAsBuildMenuTab("Oxygen Tab", global::Action.Plan2)
				});
			}

			// Token: 0x020039DF RID: 14815
			public class INSUFFICIENTOXYGENLASTCYCLE
			{
				// Token: 0x0400DED9 RID: 57049
				public static LocString NAME = "Insufficient Oxygen generation";

				// Token: 0x0400DEDA RID: 57050
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"My colony is consuming more ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					" than it is producing, and will run out air if I do not increase production.\n\nI should check my existing oxygen production buildings to ensure they're operating correctly\n\n• ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					" produced last cycle: {EmittingRate}\n• Consumed last cycle: {ConsumptionRate}"
				});
			}

			// Token: 0x020039E0 RID: 14816
			public class UNREFRIGERATEDFOOD
			{
				// Token: 0x0400DEDB RID: 57051
				public static LocString NAME = "Unrefrigerated Food";

				// Token: 0x0400DEDC RID: 57052
				public static LocString TOOLTIP = "These " + UI.FormatAsLink("Food", "FOOD") + " items are stored but not refrigerated:\n";
			}

			// Token: 0x020039E1 RID: 14817
			public class FOODLOW
			{
				// Token: 0x0400DEDD RID: 57053
				public static LocString NAME = "Food shortage";

				// Token: 0x0400DEDE RID: 57054
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"The colony's ",
					UI.FormatAsLink("Food", "FOOD"),
					" reserves are low:\n\n    • {0} are currently available\n    • {1} is being consumed per cycle\n\n",
					UI.FormatAsLink("Microbe Mushers", "MICROBEMUSHER"),
					" can be built from the ",
					UI.FormatAsBuildMenuTab("Food Tab", global::Action.Plan4)
				});
			}

			// Token: 0x020039E2 RID: 14818
			public class NO_MEDICAL_COTS
			{
				// Token: 0x0400DEDF RID: 57055
				public static LocString NAME = "No " + UI.FormatAsLink("Sick Bay", "DOCTORSTATION") + " built";

				// Token: 0x0400DEE0 RID: 57056
				public static LocString TOOLTIP = "There is nowhere for sick Duplicants receive medical care\n\n" + UI.FormatAsLink("Sick Bays", "DOCTORSTATION") + " can be built from the " + UI.FormatAsBuildMenuTab("Medicine Tab", global::Action.Plan8);
			}

			// Token: 0x020039E3 RID: 14819
			public class NEEDTOILET
			{
				// Token: 0x0400DEE1 RID: 57057
				public static LocString NAME = "No " + UI.FormatAsLink("Outhouse", "OUTHOUSE") + " built";

				// Token: 0x0400DEE2 RID: 57058
				public static LocString TOOLTIP = "My Duplicants have nowhere to relieve themselves\n\n" + UI.FormatAsLink("Outhouses", "OUTHOUSE") + " can be built from the " + UI.FormatAsBuildMenuTab("Plumbing Tab", global::Action.Plan5);
			}

			// Token: 0x020039E4 RID: 14820
			public class NEEDFOOD
			{
				// Token: 0x0400DEE3 RID: 57059
				public static LocString NAME = "Colony requires a food source";

				// Token: 0x0400DEE4 RID: 57060
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"The colony will exhaust their supplies without a new ",
					UI.FormatAsLink("Food", "FOOD"),
					" source\n\n",
					UI.FormatAsLink("Microbe Mushers", "MICROBEMUSHER"),
					" can be built from the ",
					UI.FormatAsBuildMenuTab("Food Tab", global::Action.Plan4)
				});
			}

			// Token: 0x020039E5 RID: 14821
			public class HYGENE_NEEDED
			{
				// Token: 0x0400DEE5 RID: 57061
				public static LocString NAME = "No " + UI.FormatAsLink("Wash Basin", "WASHBASIN") + " built";

				// Token: 0x0400DEE6 RID: 57062
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					UI.FormatAsLink("Germs", "DISEASE"),
					" are spreading in the colony because my Duplicants have nowhere to clean up\n\n",
					UI.FormatAsLink("Wash Basins", "WASHBASIN"),
					" can be built from the ",
					UI.FormatAsBuildMenuTab("Medicine Tab", global::Action.Plan8)
				});
			}

			// Token: 0x020039E6 RID: 14822
			public class NEEDSLEEP
			{
				// Token: 0x0400DEE7 RID: 57063
				public static LocString NAME = "No " + UI.FormatAsLink("Cots", "BED") + " built";

				// Token: 0x0400DEE8 RID: 57064
				public static LocString TOOLTIP = "My Duplicants would appreciate a place to sleep\n\n" + UI.FormatAsLink("Cots", "BED") + " can be built from the " + UI.FormatAsBuildMenuTab("Furniture Tab", global::Action.Plan9);
			}

			// Token: 0x020039E7 RID: 14823
			public class NEEDENERGYSOURCE
			{
				// Token: 0x0400DEE9 RID: 57065
				public static LocString NAME = "Colony requires a " + UI.FormatAsLink("Power", "POWER") + " source";

				// Token: 0x0400DEEA RID: 57066
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					UI.FormatAsLink("Power", "POWER"),
					" is required to operate electrical buildings\n\n",
					UI.FormatAsLink("Manual Generators", "MANUALGENERATOR"),
					" and ",
					UI.FormatAsLink("Wire", "WIRE"),
					" can be built from the ",
					UI.FormatAsLink("Power Tab", "[3]")
				});
			}

			// Token: 0x020039E8 RID: 14824
			public class RESOURCEMELTED
			{
				// Token: 0x0400DEEB RID: 57067
				public static LocString NAME = "Resources melted";

				// Token: 0x0400DEEC RID: 57068
				public static LocString TOOLTIP = "These resources have melted:";
			}

			// Token: 0x020039E9 RID: 14825
			public class VENTOVERPRESSURE
			{
				// Token: 0x0400DEED RID: 57069
				public static LocString NAME = "Vent overpressurized";

				// Token: 0x0400DEEE RID: 57070
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"These ",
					UI.PRE_KEYWORD,
					"Pipe",
					UI.PST_KEYWORD,
					" systems have exited the ideal ",
					UI.PRE_KEYWORD,
					"Pressure",
					UI.PST_KEYWORD,
					" range:"
				});
			}

			// Token: 0x020039EA RID: 14826
			public class VENTBLOCKED
			{
				// Token: 0x0400DEEF RID: 57071
				public static LocString NAME = "Vent blocked";

				// Token: 0x0400DEF0 RID: 57072
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Blocked ",
					UI.PRE_KEYWORD,
					"Pipes",
					UI.PST_KEYWORD,
					" have stopped these systems from functioning:"
				});
			}

			// Token: 0x020039EB RID: 14827
			public class OUTPUTBLOCKED
			{
				// Token: 0x0400DEF1 RID: 57073
				public static LocString NAME = "Output blocked";

				// Token: 0x0400DEF2 RID: 57074
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Blocked ",
					UI.PRE_KEYWORD,
					"Pipes",
					UI.PST_KEYWORD,
					" have stopped these systems from functioning:"
				});
			}

			// Token: 0x020039EC RID: 14828
			public class BROKENMACHINE
			{
				// Token: 0x0400DEF3 RID: 57075
				public static LocString NAME = "Building broken";

				// Token: 0x0400DEF4 RID: 57076
				public static LocString TOOLTIP = "These buildings have taken significant damage and are nonfunctional:";
			}

			// Token: 0x020039ED RID: 14829
			public class STRUCTURALDAMAGE
			{
				// Token: 0x0400DEF5 RID: 57077
				public static LocString NAME = "Structural damage";

				// Token: 0x0400DEF6 RID: 57078
				public static LocString TOOLTIP = "These buildings' structural integrity has been compromised";
			}

			// Token: 0x020039EE RID: 14830
			public class STRUCTURALCOLLAPSE
			{
				// Token: 0x0400DEF7 RID: 57079
				public static LocString NAME = "Structural collapse";

				// Token: 0x0400DEF8 RID: 57080
				public static LocString TOOLTIP = "These buildings have collapsed:";
			}

			// Token: 0x020039EF RID: 14831
			public class GASCLOUDWARNING
			{
				// Token: 0x0400DEF9 RID: 57081
				public static LocString NAME = "A gas cloud approaches";

				// Token: 0x0400DEFA RID: 57082
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"A toxic ",
					UI.PRE_KEYWORD,
					"Gas",
					UI.PST_KEYWORD,
					" cloud will soon envelop the colony"
				});
			}

			// Token: 0x020039F0 RID: 14832
			public class GASCLOUDARRIVING
			{
				// Token: 0x0400DEFB RID: 57083
				public static LocString NAME = "The colony is entering a cloud of gas";

				// Token: 0x0400DEFC RID: 57084
				public static LocString TOOLTIP = "";
			}

			// Token: 0x020039F1 RID: 14833
			public class GASCLOUDPEAK
			{
				// Token: 0x0400DEFD RID: 57085
				public static LocString NAME = "The gas cloud is at its densest point";

				// Token: 0x0400DEFE RID: 57086
				public static LocString TOOLTIP = "";
			}

			// Token: 0x020039F2 RID: 14834
			public class GASCLOUDDEPARTING
			{
				// Token: 0x0400DEFF RID: 57087
				public static LocString NAME = "The gas cloud is receding";

				// Token: 0x0400DF00 RID: 57088
				public static LocString TOOLTIP = "";
			}

			// Token: 0x020039F3 RID: 14835
			public class GASCLOUDGONE
			{
				// Token: 0x0400DF01 RID: 57089
				public static LocString NAME = "The colony is once again in open space";

				// Token: 0x0400DF02 RID: 57090
				public static LocString TOOLTIP = "";
			}

			// Token: 0x020039F4 RID: 14836
			public class AVAILABLE
			{
				// Token: 0x0400DF03 RID: 57091
				public static LocString NAME = "Resource available";

				// Token: 0x0400DF04 RID: 57092
				public static LocString TOOLTIP = "These resources have become available:";
			}

			// Token: 0x020039F5 RID: 14837
			public class ALLOCATED
			{
				// Token: 0x0400DF05 RID: 57093
				public static LocString NAME = "Resource allocated";

				// Token: 0x0400DF06 RID: 57094
				public static LocString TOOLTIP = "These resources are reserved for a planned building:";
			}

			// Token: 0x020039F6 RID: 14838
			public class INCREASEDEXPECTATIONS
			{
				// Token: 0x0400DF07 RID: 57095
				public static LocString NAME = "Duplicants' expectations increased";

				// Token: 0x0400DF08 RID: 57096
				public static LocString TOOLTIP = "Duplicants require better amenities over time.\nThese Duplicants have increased their expectations:";
			}

			// Token: 0x020039F7 RID: 14839
			public class NEARLYDRY
			{
				// Token: 0x0400DF09 RID: 57097
				public static LocString NAME = "Nearly dry";

				// Token: 0x0400DF0A RID: 57098
				public static LocString TOOLTIP = "These Duplicants will dry off soon:";
			}

			// Token: 0x020039F8 RID: 14840
			public class IMMIGRANTSLEFT
			{
				// Token: 0x0400DF0B RID: 57099
				public static LocString NAME = "Printables have been reabsorbed";

				// Token: 0x0400DF0C RID: 57100
				public static LocString TOOLTIP = "The care packages have been disintegrated and printable Duplicants have been Oozed";
			}

			// Token: 0x020039F9 RID: 14841
			public class LEVELUP
			{
				// Token: 0x0400DF0D RID: 57101
				public static LocString NAME = "Attribute increase";

				// Token: 0x0400DF0E RID: 57102
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"These Duplicants' ",
					UI.PRE_KEYWORD,
					"Attributes",
					UI.PST_KEYWORD,
					" have improved:"
				});

				// Token: 0x0400DF0F RID: 57103
				public static LocString SUFFIX = " - {0} Skill Level modifier raised to +{1}";
			}

			// Token: 0x020039FA RID: 14842
			public class RESETSKILL
			{
				// Token: 0x0400DF10 RID: 57104
				public static LocString NAME = "Skills reset";

				// Token: 0x0400DF11 RID: 57105
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"These Duplicants have had their ",
					UI.PRE_KEYWORD,
					"Skill Points",
					UI.PST_KEYWORD,
					" refunded:"
				});
			}

			// Token: 0x020039FB RID: 14843
			public class BADROCKETPATH
			{
				// Token: 0x0400DF12 RID: 57106
				public static LocString NAME = "Flight Path Obstructed";

				// Token: 0x0400DF13 RID: 57107
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"A rocket's flight path has been interrupted by a new astronomical discovery.\nOpen the ",
					UI.PRE_KEYWORD,
					"Starmap Screen",
					UI.PST_KEYWORD,
					" ",
					UI.FormatAsHotKey(global::Action.ManageStarmap),
					" to reassign rocket paths"
				});
			}

			// Token: 0x020039FC RID: 14844
			public class SCHEDULE_CHANGED
			{
				// Token: 0x0400DF14 RID: 57108
				public static LocString NAME = "{0}: {1}!";

				// Token: 0x0400DF15 RID: 57109
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Duplicants assigned to ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" have started their <b>{1}</b> block.\n\n{2}\n\nOpen the ",
					UI.PRE_KEYWORD,
					"Schedule Screen",
					UI.PST_KEYWORD,
					" ",
					UI.FormatAsHotKey(global::Action.ManageSchedule),
					" to change blocks or assignments"
				});
			}

			// Token: 0x020039FD RID: 14845
			public class GENESHUFFLER
			{
				// Token: 0x0400DF16 RID: 57110
				public static LocString NAME = "Genes Shuffled";

				// Token: 0x0400DF17 RID: 57111
				public static LocString TOOLTIP = "These Duplicants had their genetic makeup modified:";

				// Token: 0x0400DF18 RID: 57112
				public static LocString SUFFIX = " has developed " + UI.PRE_KEYWORD + "{0}" + UI.PST_KEYWORD;
			}

			// Token: 0x020039FE RID: 14846
			public class HEALINGTRAITGAIN
			{
				// Token: 0x0400DF19 RID: 57113
				public static LocString NAME = "New trait";

				// Token: 0x0400DF1A RID: 57114
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"These Duplicants' injuries weren't set and healed improperly.\nThey developed ",
					UI.PRE_KEYWORD,
					"Traits",
					UI.PST_KEYWORD,
					" as a result:"
				});

				// Token: 0x0400DF1B RID: 57115
				public static LocString SUFFIX = " has developed " + UI.PRE_KEYWORD + "{0}" + UI.PST_KEYWORD;
			}

			// Token: 0x020039FF RID: 14847
			public class COLONYLOST
			{
				// Token: 0x0400DF1C RID: 57116
				public static LocString NAME = "Colony Lost";

				// Token: 0x0400DF1D RID: 57117
				public static LocString TOOLTIP = "All Duplicants are dead or incapacitated";
			}

			// Token: 0x02003A00 RID: 14848
			public class FABRICATOREMPTY
			{
				// Token: 0x0400DF1E RID: 57118
				public static LocString NAME = "Fabricator idle";

				// Token: 0x0400DF1F RID: 57119
				public static LocString TOOLTIP = "These fabricators have no recipes queued:";
			}

			// Token: 0x02003A01 RID: 14849
			public class BUILDING_MELTED
			{
				// Token: 0x0400DF20 RID: 57120
				public static LocString NAME = "Building melted";

				// Token: 0x0400DF21 RID: 57121
				public static LocString TOOLTIP = "Extreme heat has melted these buildings:";
			}

			// Token: 0x02003A02 RID: 14850
			public class SUIT_DROPPED
			{
				// Token: 0x0400DF22 RID: 57122
				public static LocString NAME = "No Docks available";

				// Token: 0x0400DF23 RID: 57123
				public static LocString TOOLTIP = "An exosuit was dropped because there were no empty docks available";
			}

			// Token: 0x02003A03 RID: 14851
			public class DEATH_SUFFOCATION
			{
				// Token: 0x0400DF24 RID: 57124
				public static LocString NAME = "Duplicants suffocated";

				// Token: 0x0400DF25 RID: 57125
				public static LocString TOOLTIP = "These Duplicants died from a lack of " + ELEMENTS.OXYGEN.NAME + ":";
			}

			// Token: 0x02003A04 RID: 14852
			public class DEATH_FROZENSOLID
			{
				// Token: 0x0400DF26 RID: 57126
				public static LocString NAME = "Duplicants have frozen";

				// Token: 0x0400DF27 RID: 57127
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"These Duplicants died from extremely low ",
					UI.PRE_KEYWORD,
					"Temperatures",
					UI.PST_KEYWORD,
					":"
				});
			}

			// Token: 0x02003A05 RID: 14853
			public class DEATH_OVERHEATING
			{
				// Token: 0x0400DF28 RID: 57128
				public static LocString NAME = "Duplicants have overheated";

				// Token: 0x0400DF29 RID: 57129
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"These Duplicants died from extreme ",
					UI.PRE_KEYWORD,
					"Heat",
					UI.PST_KEYWORD,
					":"
				});
			}

			// Token: 0x02003A06 RID: 14854
			public class DEATH_STARVATION
			{
				// Token: 0x0400DF2A RID: 57130
				public static LocString NAME = "Duplicants have starved";

				// Token: 0x0400DF2B RID: 57131
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"These Duplicants died from a lack of ",
					UI.PRE_KEYWORD,
					"Food",
					UI.PST_KEYWORD,
					":"
				});
			}

			// Token: 0x02003A07 RID: 14855
			public class DEATH_FELL
			{
				// Token: 0x0400DF2C RID: 57132
				public static LocString NAME = "Duplicants splattered";

				// Token: 0x0400DF2D RID: 57133
				public static LocString TOOLTIP = "These Duplicants fell to their deaths:";
			}

			// Token: 0x02003A08 RID: 14856
			public class DEATH_CRUSHED
			{
				// Token: 0x0400DF2E RID: 57134
				public static LocString NAME = "Duplicants crushed";

				// Token: 0x0400DF2F RID: 57135
				public static LocString TOOLTIP = "These Duplicants have been crushed:";
			}

			// Token: 0x02003A09 RID: 14857
			public class DEATH_SUFFOCATEDTANKEMPTY
			{
				// Token: 0x0400DF30 RID: 57136
				public static LocString NAME = "Duplicants have suffocated";

				// Token: 0x0400DF31 RID: 57137
				public static LocString TOOLTIP = "These Duplicants were unable to reach " + UI.FormatAsLink("Oxygen", "OXYGEN") + " and died:";
			}

			// Token: 0x02003A0A RID: 14858
			public class DEATH_SUFFOCATEDAIRTOOHOT
			{
				// Token: 0x0400DF32 RID: 57138
				public static LocString NAME = "Duplicants have suffocated";

				// Token: 0x0400DF33 RID: 57139
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"These Duplicants have asphyxiated in ",
					UI.PRE_KEYWORD,
					"Hot",
					UI.PST_KEYWORD,
					" air:"
				});
			}

			// Token: 0x02003A0B RID: 14859
			public class DEATH_SUFFOCATEDAIRTOOCOLD
			{
				// Token: 0x0400DF34 RID: 57140
				public static LocString NAME = "Duplicants have suffocated";

				// Token: 0x0400DF35 RID: 57141
				public static LocString TOOLTIP = "These Duplicants have asphyxiated in " + UI.FormatAsLink("Cold", "HEAT") + " air:";
			}

			// Token: 0x02003A0C RID: 14860
			public class DEATH_DROWNED
			{
				// Token: 0x0400DF36 RID: 57142
				public static LocString NAME = "Duplicants have drowned";

				// Token: 0x0400DF37 RID: 57143
				public static LocString TOOLTIP = "These Duplicants have drowned:";
			}

			// Token: 0x02003A0D RID: 14861
			public class DEATH_ENTOUMBED
			{
				// Token: 0x0400DF38 RID: 57144
				public static LocString NAME = "Duplicants have been entombed";

				// Token: 0x0400DF39 RID: 57145
				public static LocString TOOLTIP = "These Duplicants are trapped and need assistance:";
			}

			// Token: 0x02003A0E RID: 14862
			public class DEATH_RAPIDDECOMPRESSION
			{
				// Token: 0x0400DF3A RID: 57146
				public static LocString NAME = "Duplicants pressurized";

				// Token: 0x0400DF3B RID: 57147
				public static LocString TOOLTIP = "These Duplicants died in a low pressure environment:";
			}

			// Token: 0x02003A0F RID: 14863
			public class DEATH_OVERPRESSURE
			{
				// Token: 0x0400DF3C RID: 57148
				public static LocString NAME = "Duplicants pressurized";

				// Token: 0x0400DF3D RID: 57149
				public static LocString TOOLTIP = "These Duplicants died in a high pressure environment:";
			}

			// Token: 0x02003A10 RID: 14864
			public class DEATH_POISONED
			{
				// Token: 0x0400DF3E RID: 57150
				public static LocString NAME = "Duplicants poisoned";

				// Token: 0x0400DF3F RID: 57151
				public static LocString TOOLTIP = "These Duplicants died as a result of poisoning:";
			}

			// Token: 0x02003A11 RID: 14865
			public class DEATH_DISEASE
			{
				// Token: 0x0400DF40 RID: 57152
				public static LocString NAME = "Duplicants have succumbed to disease";

				// Token: 0x0400DF41 RID: 57153
				public static LocString TOOLTIP = "These Duplicants died from an untreated " + UI.FormatAsLink("Disease", "DISEASE") + ":";
			}

			// Token: 0x02003A12 RID: 14866
			public class CIRCUIT_OVERLOADED
			{
				// Token: 0x0400DF42 RID: 57154
				public static LocString NAME = "Circuit Overloaded";

				// Token: 0x0400DF43 RID: 57155
				public static LocString TOOLTIP = "These " + BUILDINGS.PREFABS.WIRE.NAME + "s melted due to excessive current demands on their circuits";
			}

			// Token: 0x02003A13 RID: 14867
			public class LOGIC_CIRCUIT_OVERLOADED
			{
				// Token: 0x0400DF44 RID: 57156
				public static LocString NAME = "Logic Circuit Overloaded";

				// Token: 0x0400DF45 RID: 57157
				public static LocString TOOLTIP = "These " + BUILDINGS.PREFABS.LOGICWIRE.NAME + "s melted due to more bits of data being sent over them than they can support";
			}

			// Token: 0x02003A14 RID: 14868
			public class DISCOVERED_SPACE
			{
				// Token: 0x0400DF46 RID: 57158
				public static LocString NAME = "ALERT - Surface Breach";

				// Token: 0x0400DF47 RID: 57159
				public static LocString TOOLTIP = "Amazing!\n\nMy Duplicants have managed to breach the surface of our rocky prison.\n\nI should be careful; the region is extremely inhospitable and I could easily lose resources to the vacuum of space.";
			}

			// Token: 0x02003A15 RID: 14869
			public class COLONY_ACHIEVEMENT_EARNED
			{
				// Token: 0x0400DF48 RID: 57160
				public static LocString NAME = "Colony Achievement earned";

				// Token: 0x0400DF49 RID: 57161
				public static LocString TOOLTIP = "The colony has earned a new achievement.";
			}

			// Token: 0x02003A16 RID: 14870
			public class WARP_PORTAL_DUPE_READY
			{
				// Token: 0x0400DF4A RID: 57162
				public static LocString NAME = "Duplicant warp ready";

				// Token: 0x0400DF4B RID: 57163
				public static LocString TOOLTIP = "{dupe} is ready to warp from the " + BUILDINGS.PREFABS.WARPPORTAL.NAME;
			}

			// Token: 0x02003A17 RID: 14871
			public class GENETICANALYSISCOMPLETE
			{
				// Token: 0x0400DF4C RID: 57164
				public static LocString NAME = "Seed Analysis Complete";

				// Token: 0x0400DF4D RID: 57165
				public static LocString MESSAGEBODY = "Deeply probing the genes of the {Plant} plant have led to the discovery of a promising new cultivatable mutation:\n\n<b>{Subspecies}</b>\n\n{Info}";

				// Token: 0x0400DF4E RID: 57166
				public static LocString TOOLTIP = "{Plant} Analysis complete!";
			}

			// Token: 0x02003A18 RID: 14872
			public class NEWMUTANTSEED
			{
				// Token: 0x0400DF4F RID: 57167
				public static LocString NAME = "New Mutant Seed Discovered";

				// Token: 0x0400DF50 RID: 57168
				public static LocString TOOLTIP = "A new mutant variety of the {Plant} has been found. Analyze it at the " + BUILDINGS.PREFABS.GENETICANALYSISSTATION.NAME + " to learn more!";
			}

			// Token: 0x02003A19 RID: 14873
			public class DUPLICANT_CRASH_LANDED
			{
				// Token: 0x0400DF51 RID: 57169
				public static LocString NAME = "Duplicant Crash Landed!";

				// Token: 0x0400DF52 RID: 57170
				public static LocString TOOLTIP = "A Duplicant has successfully crashed an Escape Pod onto the surface of a nearby Planetoid.";
			}

			// Token: 0x02003A1A RID: 14874
			public class POIRESEARCHUNLOCKCOMPLETE
			{
				// Token: 0x0400DF53 RID: 57171
				public static LocString NAME = "Research Discovered";

				// Token: 0x0400DF54 RID: 57172
				public static LocString MESSAGEBODY = "Eureka! We've decrypted the Research Portal's final transmission. New buildings have become available:\n  {0}\n\nOne file was labeled \"Open This First.\" New Database Entry unlocked.";

				// Token: 0x0400DF55 RID: 57173
				public static LocString TOOLTIP = "{0} unlocked!";

				// Token: 0x0400DF56 RID: 57174
				public static LocString BUTTON_VIEW_LORE = "View entry";
			}

			// Token: 0x02003A1B RID: 14875
			public class BIONICRESEARCHUNLOCK
			{
				// Token: 0x0400DF57 RID: 57175
				public static LocString NAME = "Research Discovered";

				// Token: 0x0400DF58 RID: 57176
				public static LocString MESSAGEBODY = "My new Bionic Duplicant came programmed with {0} technology. How crafty!";
			}

			// Token: 0x02003A1C RID: 14876
			public class BIONICLIQUIDDAMAGE
			{
				// Token: 0x0400DF59 RID: 57177
				public static LocString NAME = "Liquid Damage";

				// Token: 0x0400DF5A RID: 57178
				public static LocString TOOLTIP = "This Duplicant stepped in liquid and damaged their bionic systems!";
			}
		}

		// Token: 0x02003A1D RID: 14877
		public class TUTORIAL
		{
			// Token: 0x0400DF5B RID: 57179
			public static LocString DONT_SHOW_AGAIN = "Don't Show Again";
		}

		// Token: 0x02003A1E RID: 14878
		public class PLACERS
		{
			// Token: 0x02003A1F RID: 14879
			public class DIGPLACER
			{
				// Token: 0x0400DF5C RID: 57180
				public static LocString NAME = "Dig";
			}

			// Token: 0x02003A20 RID: 14880
			public class MOPPLACER
			{
				// Token: 0x0400DF5D RID: 57181
				public static LocString NAME = "Mop";
			}

			// Token: 0x02003A21 RID: 14881
			public class MOVEPICKUPABLEPLACER
			{
				// Token: 0x0400DF5E RID: 57182
				public static LocString NAME = "Relocate Here";

				// Token: 0x0400DF5F RID: 57183
				public static LocString PLACER_STATUS = "Next Destination";

				// Token: 0x0400DF60 RID: 57184
				public static LocString PLACER_STATUS_TOOLTIP = "Click to see where this item will be relocated to";
			}
		}

		// Token: 0x02003A22 RID: 14882
		public class MONUMENT_COMPLETE
		{
			// Token: 0x0400DF61 RID: 57185
			public static LocString NAME = "Great Monument";

			// Token: 0x0400DF62 RID: 57186
			public static LocString DESC = "A feat of artistic vision and expert engineering that will doubtless inspire Duplicants for thousands of cycles to come";
		}
	}
}
