using System;

namespace STRINGS
{
	// Token: 0x02003714 RID: 14100
	public class MISC
	{
		// Token: 0x02003715 RID: 14101
		public class TAGS
		{
			// Token: 0x0400D4C9 RID: 54473
			public static LocString OTHER = "Miscellaneous";

			// Token: 0x0400D4CA RID: 54474
			public static LocString FILTER = UI.FormatAsLink("Filtration Medium", "FILTER");

			// Token: 0x0400D4CB RID: 54475
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

			// Token: 0x0400D4CC RID: 54476
			public static LocString ICEORE = UI.FormatAsLink("Ice", "ICEORE");

			// Token: 0x0400D4CD RID: 54477
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

			// Token: 0x0400D4CE RID: 54478
			public static LocString PHOSPHORUS = UI.FormatAsLink("Phosphorus", "PHOSPHORUS");

			// Token: 0x0400D4CF RID: 54479
			public static LocString BUILDABLERAW = UI.FormatAsLink("Raw Mineral", "BUILDABLERAW");

			// Token: 0x0400D4D0 RID: 54480
			public static LocString BUILDABLERAW_DESC = string.Concat(new string[]
			{
				"Raw minerals are the unrefined forms of organic solids. Almost all raw minerals can be processed in the ",
				UI.FormatAsLink("Rock Crusher", "ROCKCRUSHER"),
				", although a handful require the use of the ",
				UI.FormatAsLink("Molecular Forge", "SUPERMATERIALREFINERY"),
				"."
			});

			// Token: 0x0400D4D1 RID: 54481
			public static LocString BUILDABLEPROCESSED = UI.FormatAsLink("Refined Mineral", "BUILDABLEPROCESSED");

			// Token: 0x0400D4D2 RID: 54482
			public static LocString BUILDABLEANY = UI.FormatAsLink("General Buildable", "BUILDABLEANY");

			// Token: 0x0400D4D3 RID: 54483
			public static LocString BUILDABLEANY_DESC = "";

			// Token: 0x0400D4D4 RID: 54484
			public static LocString DEHYDRATED = "Dehydrated";

			// Token: 0x0400D4D5 RID: 54485
			public static LocString PLASTIFIABLELIQUID = UI.FormatAsLink("Plastic Monomer", "PLASTIFIABLELIQUID");

			// Token: 0x0400D4D6 RID: 54486
			public static LocString PLASTIFIABLELIQUID_DESC = string.Concat(new string[]
			{
				"Plastic monomers are organic compounds that can be processed into ",
				UI.FormatAsLink("Plastics", "PLASTIC"),
				" that have valuable applications as advanced building materials. Plastics derived from these monomers can also be used as packaging materials for ",
				UI.FormatAsLink("Food", "FOOD"),
				" preservation."
			});

			// Token: 0x0400D4D7 RID: 54487
			public static LocString REFINEDMETAL = UI.FormatAsLink("Refined Metal", "REFINEDMETAL");

			// Token: 0x0400D4D8 RID: 54488
			public static LocString REFINEDMETAL_DESC = string.Concat(new string[]
			{
				"Refined metals are purified forms of metal often used in higher-tier electronics due to their tendency to be able to withstand higher temperatures when they are made into wires. Other benefits include the increased decor value for some metals which can greatly improve the well-being of a colony.\n\nMetal ore can be refined in either the ",
				UI.FormatAsLink("Rock Crusher", "ROCKCRUSHER"),
				" or the ",
				UI.FormatAsLink("Metal Refinery", "METALREFINERY"),
				"."
			});

			// Token: 0x0400D4D9 RID: 54489
			public static LocString METAL = UI.FormatAsLink("Metal Ore", "METAL");

			// Token: 0x0400D4DA RID: 54490
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

			// Token: 0x0400D4DB RID: 54491
			public static LocString PRECIOUSMETAL = UI.FormatAsLink("Precious Metal", "PRECIOUSMETAL");

			// Token: 0x0400D4DC RID: 54492
			public static LocString RAWPRECIOUSMETAL = "Precious Metal Ore";

			// Token: 0x0400D4DD RID: 54493
			public static LocString PRECIOUSROCK = UI.FormatAsLink("Precious Rock", "PRECIOUSROCK");

			// Token: 0x0400D4DE RID: 54494
			public static LocString PRECIOUSROCK_DESC = "Precious rocks are raw minerals. Their extreme hardness produces durable " + UI.FormatAsLink("Decor", "DECOR") + ".\n\nSome precious rocks are inherently attractive even in their natural, unfinished form.";

			// Token: 0x0400D4DF RID: 54495
			public static LocString ALLOY = UI.FormatAsLink("Alloy", "ALLOY");

			// Token: 0x0400D4E0 RID: 54496
			public static LocString BUILDINGFIBER = UI.FormatAsLink("Fiber", "BUILDINGFIBER");

			// Token: 0x0400D4E1 RID: 54497
			public static LocString BUILDINGFIBER_DESC = "Fibers are organically sourced polymers which are both sturdy and sensorially pleasant, making them suitable in the construction of " + UI.FormatAsLink("Morale", "MORALE") + "-boosting buildings.";

			// Token: 0x0400D4E2 RID: 54498
			public static LocString BUILDINGWOOD = UI.FormatAsLink("Wood", "BUILDINGWOOD");

			// Token: 0x0400D4E3 RID: 54499
			public static LocString BUILDINGWOOD_DESC = string.Concat(new string[]
			{
				"Wood is a renewable building material which can also be used as a valuable source of fuel and electricity when refined at the ",
				UI.FormatAsLink("Wood Burner", "WOODGASGENERATOR"),
				" or the ",
				UI.FormatAsLink("Ethanol Distiller", "ETHANOLDISTILLERY"),
				"."
			});

			// Token: 0x0400D4E4 RID: 54500
			public static LocString CRUSHABLE = "Crushable";

			// Token: 0x0400D4E5 RID: 54501
			public static LocString CROPSEEDS = "Crop Seeds";

			// Token: 0x0400D4E6 RID: 54502
			public static LocString CERAMIC = UI.FormatAsLink("Ceramic", "CERAMIC");

			// Token: 0x0400D4E7 RID: 54503
			public static LocString POLYPROPYLENE = UI.FormatAsLink("Plastic", "POLYPROPYLENE");

			// Token: 0x0400D4E8 RID: 54504
			public static LocString BAGABLECREATURE = UI.FormatAsLink("Critter", "CREATURES");

			// Token: 0x0400D4E9 RID: 54505
			public static LocString SWIMMINGCREATURE = "Aquatic Critter";

			// Token: 0x0400D4EA RID: 54506
			public static LocString LIFE = "Life";

			// Token: 0x0400D4EB RID: 54507
			public static LocString LIQUIFIABLE = "Liquefiable";

			// Token: 0x0400D4EC RID: 54508
			public static LocString LIQUID = UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID");

			// Token: 0x0400D4ED RID: 54509
			public static LocString LEAD = UI.FormatAsLink("Lead", "LEAD");

			// Token: 0x0400D4EE RID: 54510
			public static LocString SPECIAL = "Special";

			// Token: 0x0400D4EF RID: 54511
			public static LocString FARMABLE = UI.FormatAsLink("Cultivable Soil", "FARMABLE");

			// Token: 0x0400D4F0 RID: 54512
			public static LocString FARMABLE_DESC = "Cultivable soil is a fundamental building block of basic agricultural systems and can also be useful in the production of clean " + UI.FormatAsLink("Oxygen", "OXYGEN") + ".";

			// Token: 0x0400D4F1 RID: 54513
			public static LocString AGRICULTURE = UI.FormatAsLink("Agriculture", "AGRICULTURE");

			// Token: 0x0400D4F2 RID: 54514
			public static LocString COAL = "Coal";

			// Token: 0x0400D4F3 RID: 54515
			public static LocString BLEACHSTONE = "Bleach Stone";

			// Token: 0x0400D4F4 RID: 54516
			public static LocString ORGANICS = "Organic";

			// Token: 0x0400D4F5 RID: 54517
			public static LocString CONSUMABLEORE = "Consumable Ore";

			// Token: 0x0400D4F6 RID: 54518
			public static LocString SUBLIMATING = "Sublimators";

			// Token: 0x0400D4F7 RID: 54519
			public static LocString ORE = "Ore";

			// Token: 0x0400D4F8 RID: 54520
			public static LocString BREATHABLE = "Breathable Gas";

			// Token: 0x0400D4F9 RID: 54521
			public static LocString UNBREATHABLE = "Unbreathable Gas";

			// Token: 0x0400D4FA RID: 54522
			public static LocString GAS = "Gas";

			// Token: 0x0400D4FB RID: 54523
			public static LocString BURNS = "Flammable";

			// Token: 0x0400D4FC RID: 54524
			public static LocString UNSTABLE = "Unstable";

			// Token: 0x0400D4FD RID: 54525
			public static LocString TOXIC = "Toxic";

			// Token: 0x0400D4FE RID: 54526
			public static LocString MIXTURE = "Mixture";

			// Token: 0x0400D4FF RID: 54527
			public static LocString SOLID = UI.FormatAsLink("Solid", "ELEMENTS_SOLID");

			// Token: 0x0400D500 RID: 54528
			public static LocString FLYINGCRITTEREDIBLE = "Bait";

			// Token: 0x0400D501 RID: 54529
			public static LocString INDUSTRIALPRODUCT = "Industrial Product";

			// Token: 0x0400D502 RID: 54530
			public static LocString INDUSTRIALINGREDIENT = UI.FormatAsLink("Industrial Ingredient", "INDUSTRIALINGREDIENT");

			// Token: 0x0400D503 RID: 54531
			public static LocString MEDICALSUPPLIES = "Medical Supplies";

			// Token: 0x0400D504 RID: 54532
			public static LocString CLOTHES = UI.FormatAsLink("Clothing", "EQUIPMENT");

			// Token: 0x0400D505 RID: 54533
			public static LocString EMITSLIGHT = UI.FormatAsLink("Light Emitter", "LIGHT");

			// Token: 0x0400D506 RID: 54534
			public static LocString BED = "Bed";

			// Token: 0x0400D507 RID: 54535
			public static LocString MESSSTATION = "Dining Table";

			// Token: 0x0400D508 RID: 54536
			public static LocString TOY = "Toy";

			// Token: 0x0400D509 RID: 54537
			public static LocString SUIT = "Suit";

			// Token: 0x0400D50A RID: 54538
			public static LocString MULTITOOL = "Multitool";

			// Token: 0x0400D50B RID: 54539
			public static LocString CLINIC = "Clinic";

			// Token: 0x0400D50C RID: 54540
			public static LocString RELAXATION_POINT = "Leisure Area";

			// Token: 0x0400D50D RID: 54541
			public static LocString SOLIDMATERIAL = "Solid Material";

			// Token: 0x0400D50E RID: 54542
			public static LocString EXTRUDABLE = "Extrudable";

			// Token: 0x0400D50F RID: 54543
			public static LocString PLUMBABLE = UI.FormatAsLink("Plumbable", "PLUMBABLE");

			// Token: 0x0400D510 RID: 54544
			public static LocString PLUMBABLE_DESC = "";

			// Token: 0x0400D511 RID: 54545
			public static LocString COMPOSTABLE = UI.FormatAsLink("Compostable", "COMPOSTABLE");

			// Token: 0x0400D512 RID: 54546
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

			// Token: 0x0400D513 RID: 54547
			public static LocString COMPOSTBASICPLANTFOOD = "Compost Muckroot";

			// Token: 0x0400D514 RID: 54548
			public static LocString EDIBLE = "Edible";

			// Token: 0x0400D515 RID: 54549
			public static LocString OXIDIZER = "Oxidizer";

			// Token: 0x0400D516 RID: 54550
			public static LocString COOKINGINGREDIENT = "Cooking Ingredient";

			// Token: 0x0400D517 RID: 54551
			public static LocString MEDICINE = "Medicine";

			// Token: 0x0400D518 RID: 54552
			public static LocString SEED = "Seed";

			// Token: 0x0400D519 RID: 54553
			public static LocString ANYWATER = "Water Based";

			// Token: 0x0400D51A RID: 54554
			public static LocString MARKEDFORCOMPOST = "Marked For Compost";

			// Token: 0x0400D51B RID: 54555
			public static LocString MARKEDFORCOMPOSTINSTORAGE = "In Compost Storage";

			// Token: 0x0400D51C RID: 54556
			public static LocString COMPOSTMEAT = "Compost Meat";

			// Token: 0x0400D51D RID: 54557
			public static LocString PICKLED = "Pickled";

			// Token: 0x0400D51E RID: 54558
			public static LocString PLASTIC = UI.FormatAsLink("Plastics", "PLASTIC");

			// Token: 0x0400D51F RID: 54559
			public static LocString PLASTIC_DESC = string.Concat(new string[]
			{
				"Plastics are synthetic ",
				UI.FormatAsLink("Solids", "ELEMENTSSOLID"),
				" that are pliable and minimize the transfer of ",
				UI.FormatAsLink("Heat", "Heat"),
				". They typically have a low melting point, although more advanced plastics have been developed to circumvent this issue."
			});

			// Token: 0x0400D520 RID: 54560
			public static LocString TOILET = "Toilet";

			// Token: 0x0400D521 RID: 54561
			public static LocString MASSAGE_TABLE = "Massage Table";

			// Token: 0x0400D522 RID: 54562
			public static LocString POWERSTATION = "Power Station";

			// Token: 0x0400D523 RID: 54563
			public static LocString FARMSTATION = "Farm Station";

			// Token: 0x0400D524 RID: 54564
			public static LocString MACHINE_SHOP = "Machine Shop";

			// Token: 0x0400D525 RID: 54565
			public static LocString ANTISEPTIC = "Antiseptic";

			// Token: 0x0400D526 RID: 54566
			public static LocString OIL = "Hydrocarbon";

			// Token: 0x0400D527 RID: 54567
			public static LocString DECORATION = "Decoration";

			// Token: 0x0400D528 RID: 54568
			public static LocString EGG = "Critter Egg";

			// Token: 0x0400D529 RID: 54569
			public static LocString EGGSHELL = "Egg Shell";

			// Token: 0x0400D52A RID: 54570
			public static LocString MANUFACTUREDMATERIAL = "Manufactured Material";

			// Token: 0x0400D52B RID: 54571
			public static LocString STEEL = "Steel";

			// Token: 0x0400D52C RID: 54572
			public static LocString RAW = "Raw Animal Product";

			// Token: 0x0400D52D RID: 54573
			public static LocString FOSSIL = "Fossil";

			// Token: 0x0400D52E RID: 54574
			public static LocString ICE = "Ice";

			// Token: 0x0400D52F RID: 54575
			public static LocString ANY = "Any";

			// Token: 0x0400D530 RID: 54576
			public static LocString TRANSPARENT = "Transparent";

			// Token: 0x0400D531 RID: 54577
			public static LocString TRANSPARENT_DESC = string.Concat(new string[]
			{
				"Transparent materials allow ",
				UI.FormatAsLink("Light", "LIGHT"),
				" to pass through. Illumination boosts Duplicant productivity during working hours, but undermines sleep quality.\n\nTransparency is also important for buildings that require a clear line of sight in order to function correctly, such as the ",
				UI.FormatAsLink("Space Scanner", "COMETDETECTOR"),
				"."
			});

			// Token: 0x0400D532 RID: 54578
			public static LocString RAREMATERIALS = "Rare Resource";

			// Token: 0x0400D533 RID: 54579
			public static LocString FARMINGMATERIAL = "Fertilizer";

			// Token: 0x0400D534 RID: 54580
			public static LocString INSULATOR = UI.FormatAsLink("Insulator", "INSULATOR");

			// Token: 0x0400D535 RID: 54581
			public static LocString INSULATOR_DESC = "Insulators have low thermal conductivity, and effectively reduce the speed at which " + UI.FormatAsLink("Heat", "Heat") + " is transferred through them.";

			// Token: 0x0400D536 RID: 54582
			public static LocString RAILGUNPAYLOADEMPTYABLE = "Payload";

			// Token: 0x0400D537 RID: 54583
			public static LocString NONCRUSHABLE = "Uncrushable";

			// Token: 0x0400D538 RID: 54584
			public static LocString STORYTRAITRESOURCE = "Story Trait";

			// Token: 0x0400D539 RID: 54585
			public static LocString GLASS = "Glass";

			// Token: 0x0400D53A RID: 54586
			public static LocString OBSIDIAN = UI.FormatAsLink("Obsidian", "OBSIDIAN");

			// Token: 0x0400D53B RID: 54587
			public static LocString DIAMOND = UI.FormatAsLink("Diamond", "DIAMOND");

			// Token: 0x0400D53C RID: 54588
			public static LocString SNOW = UI.FormatAsLink("Snow", "STABLESNOW");

			// Token: 0x0400D53D RID: 54589
			public static LocString WOODLOG = UI.FormatAsLink("Wood", "WOODLOG");

			// Token: 0x0400D53E RID: 54590
			public static LocString COMMAND_MODULE = "Command Module";

			// Token: 0x0400D53F RID: 54591
			public static LocString HABITAT_MODULE = "Habitat Module";

			// Token: 0x0400D540 RID: 54592
			public static LocString COMBUSTIBLEGAS = "Combustible Gas";

			// Token: 0x0400D541 RID: 54593
			public static LocString COMBUSTIBLELIQUID = UI.FormatAsLink("Combustible Liquid", "COMBUSTIBLELIQUID");

			// Token: 0x0400D542 RID: 54594
			public static LocString COMBUSTIBLELIQUID_DESC = string.Concat(new string[]
			{
				"Combustible Liquids are liquids that can be burned as fuel to be used in energy production such as in a ",
				UI.FormatAsLink("Petroleum Generator", "PETROLEUMGENERATOR"),
				" or a ",
				UI.FormatAsLink("Petroleum Engine", "KEROSENEENGINE"),
				".\n\nThough these liquids have other uses, such as fertilizer for growing a ",
				UI.FormatAsLink("Nosh Bean", "BEANPLANTSEED"),
				", their primary usefulness lies in their ability to be burned for ",
				UI.FormatAsLink("power", "POWER"),
				"."
			});

			// Token: 0x0400D543 RID: 54595
			public static LocString COMBUSTIBLESOLID = "Combustible Solid";

			// Token: 0x0400D544 RID: 54596
			public static LocString UNIDENTIFIEDSEED = "Seed (Unidentified Mutation)";

			// Token: 0x0400D545 RID: 54597
			public static LocString CHARMEDARTIFACT = "Artifact of Interest";

			// Token: 0x0400D546 RID: 54598
			public static LocString GENE_SHUFFLER = "Neural Vacillator";

			// Token: 0x0400D547 RID: 54599
			public static LocString WARP_PORTAL = "Teleportal";

			// Token: 0x0400D548 RID: 54600
			public static LocString FARMING = "Farm Build-Delivery";

			// Token: 0x0400D549 RID: 54601
			public static LocString RESEARCH = "Research Delivery";

			// Token: 0x0400D54A RID: 54602
			public static LocString POWER = "Generator Delivery";

			// Token: 0x0400D54B RID: 54603
			public static LocString BUILDING = "Build Dig-Delivery";

			// Token: 0x0400D54C RID: 54604
			public static LocString COOKING = "Cook Delivery";

			// Token: 0x0400D54D RID: 54605
			public static LocString FABRICATING = "Fabricate Delivery";

			// Token: 0x0400D54E RID: 54606
			public static LocString WIRING = "Wire Build-Delivery";

			// Token: 0x0400D54F RID: 54607
			public static LocString ART = "Art Build-Delivery";

			// Token: 0x0400D550 RID: 54608
			public static LocString DOCTORING = "Treatment Delivery";

			// Token: 0x0400D551 RID: 54609
			public static LocString CONVEYOR = "Shipping Build";

			// Token: 0x0400D552 RID: 54610
			public static LocString COMPOST_FORMAT = "{Item}";

			// Token: 0x0400D553 RID: 54611
			public static LocString ADVANCEDDOCTORSTATIONMEDICALSUPPLIES = "Serum Vial";

			// Token: 0x0400D554 RID: 54612
			public static LocString DOCTORSTATIONMEDICALSUPPLIES = "Medical Pack";
		}

		// Token: 0x02003716 RID: 14102
		public class STATUSITEMS
		{
			// Token: 0x02003717 RID: 14103
			public class ATTENTIONREQUIRED
			{
				// Token: 0x0400D555 RID: 54613
				public static LocString NAME = "Attention Required!";

				// Token: 0x0400D556 RID: 54614
				public static LocString TOOLTIP = "Something in my colony needs to be attended to";
			}

			// Token: 0x02003718 RID: 14104
			public class SUBLIMATIONBLOCKED
			{
				// Token: 0x0400D557 RID: 54615
				public static LocString NAME = "{SubElement} emission blocked";

				// Token: 0x0400D558 RID: 54616
				public static LocString TOOLTIP = "This {Element} deposit is not exposed to air and cannot emit {SubElement}";
			}

			// Token: 0x02003719 RID: 14105
			public class SUBLIMATIONOVERPRESSURE
			{
				// Token: 0x0400D559 RID: 54617
				public static LocString NAME = "Inert";

				// Token: 0x0400D55A RID: 54618
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Environmental ",
					UI.PRE_KEYWORD,
					"Gas Pressure",
					UI.PST_KEYWORD,
					" is too high for this {Element} deposit to emit {SubElement}"
				});
			}

			// Token: 0x0200371A RID: 14106
			public class SUBLIMATIONEMITTING
			{
				// Token: 0x0400D55B RID: 54619
				public static LocString NAME = BUILDING.STATUSITEMS.EMITTINGGASAVG.NAME;

				// Token: 0x0400D55C RID: 54620
				public static LocString TOOLTIP = BUILDING.STATUSITEMS.EMITTINGGASAVG.TOOLTIP;
			}

			// Token: 0x0200371B RID: 14107
			public class SPACE
			{
				// Token: 0x0400D55D RID: 54621
				public static LocString NAME = "Space exposure";

				// Token: 0x0400D55E RID: 54622
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

			// Token: 0x0200371C RID: 14108
			public class EDIBLE
			{
				// Token: 0x0400D55F RID: 54623
				public static LocString NAME = "Rations: {0}";

				// Token: 0x0400D560 RID: 54624
				public static LocString TOOLTIP = "Can provide " + UI.FormatAsLink("{0}", "KCAL") + " of energy to Duplicants";
			}

			// Token: 0x0200371D RID: 14109
			public class REHYDRATEDFOOD
			{
				// Token: 0x0400D561 RID: 54625
				public static LocString NAME = "Rehydrated Food";

				// Token: 0x0400D562 RID: 54626
				public static LocString TOOLTIP = string.Format(string.Concat(new string[]
				{
					"This food has been carefully re-moistened for consumption\n\n",
					UI.PRE_KEYWORD,
					"{1}",
					UI.PST_KEYWORD,
					": {0}"
				}), -1f, UI.FormatAsLink(DUPLICANTS.ATTRIBUTES.QUALITYOFLIFE.NAME, DUPLICANTS.ATTRIBUTES.QUALITYOFLIFE.NAME));
			}

			// Token: 0x0200371E RID: 14110
			public class MARKEDFORDISINFECTION
			{
				// Token: 0x0400D563 RID: 54627
				public static LocString NAME = "Disinfect Errand";

				// Token: 0x0400D564 RID: 54628
				public static LocString TOOLTIP = "Building will be disinfected once a Duplicant is available";
			}

			// Token: 0x0200371F RID: 14111
			public class PENDINGCLEAR
			{
				// Token: 0x0400D565 RID: 54629
				public static LocString NAME = "Sweep Errand";

				// Token: 0x0400D566 RID: 54630
				public static LocString TOOLTIP = "Debris will be swept once a Duplicant is available";
			}

			// Token: 0x02003720 RID: 14112
			public class PENDINGCLEARNOSTORAGE
			{
				// Token: 0x0400D567 RID: 54631
				public static LocString NAME = "Storage Unavailable";

				// Token: 0x0400D568 RID: 54632
				public static LocString TOOLTIP = "No available " + BUILDINGS.PREFABS.STORAGELOCKER.NAME + " can accept this item\n\nMake sure the filter on your storage is correctly set and there is sufficient space remaining";
			}

			// Token: 0x02003721 RID: 14113
			public class MARKEDFORCOMPOST
			{
				// Token: 0x0400D569 RID: 54633
				public static LocString NAME = "Compost Errand";

				// Token: 0x0400D56A RID: 54634
				public static LocString TOOLTIP = "Object is marked and will be moved to " + BUILDINGS.PREFABS.COMPOST.NAME + " once a Duplicant is available";
			}

			// Token: 0x02003722 RID: 14114
			public class NOCLEARLOCATIONSAVAILABLE
			{
				// Token: 0x0400D56B RID: 54635
				public static LocString NAME = "No Sweep Destination";

				// Token: 0x0400D56C RID: 54636
				public static LocString TOOLTIP = "There are no valid destinations for this object to be swept to";
			}

			// Token: 0x02003723 RID: 14115
			public class PENDINGHARVEST
			{
				// Token: 0x0400D56D RID: 54637
				public static LocString NAME = "Harvest Errand";

				// Token: 0x0400D56E RID: 54638
				public static LocString TOOLTIP = "Plant will be harvested once a Duplicant is available";
			}

			// Token: 0x02003724 RID: 14116
			public class PENDINGUPROOT
			{
				// Token: 0x0400D56F RID: 54639
				public static LocString NAME = "Uproot Errand";

				// Token: 0x0400D570 RID: 54640
				public static LocString TOOLTIP = "Plant will be uprooted once a Duplicant is available";
			}

			// Token: 0x02003725 RID: 14117
			public class WAITINGFORDIG
			{
				// Token: 0x0400D571 RID: 54641
				public static LocString NAME = "Dig Errand";

				// Token: 0x0400D572 RID: 54642
				public static LocString TOOLTIP = "Tile will be dug out once a Duplicant is available";
			}

			// Token: 0x02003726 RID: 14118
			public class WAITINGFORMOP
			{
				// Token: 0x0400D573 RID: 54643
				public static LocString NAME = "Mop Errand";

				// Token: 0x0400D574 RID: 54644
				public static LocString TOOLTIP = "Spill will be mopped once a Duplicant is available";
			}

			// Token: 0x02003727 RID: 14119
			public class NOTMARKEDFORHARVEST
			{
				// Token: 0x0400D575 RID: 54645
				public static LocString NAME = "No Harvest Pending";

				// Token: 0x0400D576 RID: 54646
				public static LocString TOOLTIP = "Use the " + UI.FormatAsTool("Harvest Tool", global::Action.Harvest) + " to mark this plant for harvest";
			}

			// Token: 0x02003728 RID: 14120
			public class GROWINGBRANCHES
			{
				// Token: 0x0400D577 RID: 54647
				public static LocString NAME = "Growing Branches";

				// Token: 0x0400D578 RID: 54648
				public static LocString TOOLTIP = "This tree is working hard to grow new branches right now";
			}

			// Token: 0x02003729 RID: 14121
			public class CLUSTERMETEORREMAININGTRAVELTIME
			{
				// Token: 0x0400D579 RID: 54649
				public static LocString NAME = "Time to collision: {time}";

				// Token: 0x0400D57A RID: 54650
				public static LocString TOOLTIP = "The time remaining before this meteor reaches its destination";
			}

			// Token: 0x0200372A RID: 14122
			public class ELEMENTALCATEGORY
			{
				// Token: 0x0400D57B RID: 54651
				public static LocString NAME = "{Category}";

				// Token: 0x0400D57C RID: 54652
				public static LocString TOOLTIP = "The selected object belongs to the <b>{Category}</b> resource category";
			}

			// Token: 0x0200372B RID: 14123
			public class ELEMENTALMASS
			{
				// Token: 0x0400D57D RID: 54653
				public static LocString NAME = "{Mass}";

				// Token: 0x0400D57E RID: 54654
				public static LocString TOOLTIP = "The selected object has a mass of <b>{Mass}</b>";
			}

			// Token: 0x0200372C RID: 14124
			public class ELEMENTALDISEASE
			{
				// Token: 0x0400D57F RID: 54655
				public static LocString NAME = "{Disease}";

				// Token: 0x0400D580 RID: 54656
				public static LocString TOOLTIP = "Current disease: {Disease}";
			}

			// Token: 0x0200372D RID: 14125
			public class ELEMENTALTEMPERATURE
			{
				// Token: 0x0400D581 RID: 54657
				public static LocString NAME = "{Temp}";

				// Token: 0x0400D582 RID: 54658
				public static LocString TOOLTIP = "The selected object is currently <b>{Temp}</b>";
			}

			// Token: 0x0200372E RID: 14126
			public class MARKEDFORCOMPOSTINSTORAGE
			{
				// Token: 0x0400D583 RID: 54659
				public static LocString NAME = "Composted";

				// Token: 0x0400D584 RID: 54660
				public static LocString TOOLTIP = "The selected object is currently in the compost";
			}

			// Token: 0x0200372F RID: 14127
			public class BURIEDITEM
			{
				// Token: 0x0400D585 RID: 54661
				public static LocString NAME = "Buried Object";

				// Token: 0x0400D586 RID: 54662
				public static LocString TOOLTIP = "Something seems to be hidden here";

				// Token: 0x0400D587 RID: 54663
				public static LocString NOTIFICATION = "Buried object discovered";

				// Token: 0x0400D588 RID: 54664
				public static LocString NOTIFICATION_TOOLTIP = "My Duplicants have uncovered a {Uncoverable}!\n\n" + UI.CLICK(UI.ClickType.Click) + " to jump to its location.";
			}

			// Token: 0x02003730 RID: 14128
			public class GENETICANALYSISCOMPLETED
			{
				// Token: 0x0400D589 RID: 54665
				public static LocString NAME = "Genome Sequenced";

				// Token: 0x0400D58A RID: 54666
				public static LocString TOOLTIP = "This Station has sequenced a new seed mutation";
			}

			// Token: 0x02003731 RID: 14129
			public class HEALTHSTATUS
			{
				// Token: 0x02003732 RID: 14130
				public class PERFECT
				{
					// Token: 0x0400D58B RID: 54667
					public static LocString NAME = "None";

					// Token: 0x0400D58C RID: 54668
					public static LocString TOOLTIP = "This Duplicant is in peak condition";
				}

				// Token: 0x02003733 RID: 14131
				public class ALRIGHT
				{
					// Token: 0x0400D58D RID: 54669
					public static LocString NAME = "None";

					// Token: 0x0400D58E RID: 54670
					public static LocString TOOLTIP = "This Duplicant is none the worse for wear";
				}

				// Token: 0x02003734 RID: 14132
				public class SCUFFED
				{
					// Token: 0x0400D58F RID: 54671
					public static LocString NAME = "Minor";

					// Token: 0x0400D590 RID: 54672
					public static LocString TOOLTIP = "This Duplicant has a few scrapes and bruises";
				}

				// Token: 0x02003735 RID: 14133
				public class INJURED
				{
					// Token: 0x0400D591 RID: 54673
					public static LocString NAME = "Moderate";

					// Token: 0x0400D592 RID: 54674
					public static LocString TOOLTIP = "This Duplicant needs some patching up";
				}

				// Token: 0x02003736 RID: 14134
				public class CRITICAL
				{
					// Token: 0x0400D593 RID: 54675
					public static LocString NAME = "Severe";

					// Token: 0x0400D594 RID: 54676
					public static LocString TOOLTIP = "This Duplicant is in serious need of medical attention";
				}

				// Token: 0x02003737 RID: 14135
				public class INCAPACITATED
				{
					// Token: 0x0400D595 RID: 54677
					public static LocString NAME = "Paralyzing";

					// Token: 0x0400D596 RID: 54678
					public static LocString TOOLTIP = "This Duplicant will die if they do not receive medical attention";
				}

				// Token: 0x02003738 RID: 14136
				public class DEAD
				{
					// Token: 0x0400D597 RID: 54679
					public static LocString NAME = "Conclusive";

					// Token: 0x0400D598 RID: 54680
					public static LocString TOOLTIP = "This Duplicant won't be getting back up";
				}
			}

			// Token: 0x02003739 RID: 14137
			public class HIT
			{
				// Token: 0x0400D599 RID: 54681
				public static LocString NAME = "{targetName} took {damageAmount} damage from {attackerName}'s attack!";
			}

			// Token: 0x0200373A RID: 14138
			public class OREMASS
			{
				// Token: 0x0400D59A RID: 54682
				public static LocString NAME = MISC.STATUSITEMS.ELEMENTALMASS.NAME;

				// Token: 0x0400D59B RID: 54683
				public static LocString TOOLTIP = MISC.STATUSITEMS.ELEMENTALMASS.TOOLTIP;
			}

			// Token: 0x0200373B RID: 14139
			public class ORETEMP
			{
				// Token: 0x0400D59C RID: 54684
				public static LocString NAME = MISC.STATUSITEMS.ELEMENTALTEMPERATURE.NAME;

				// Token: 0x0400D59D RID: 54685
				public static LocString TOOLTIP = MISC.STATUSITEMS.ELEMENTALTEMPERATURE.TOOLTIP;
			}

			// Token: 0x0200373C RID: 14140
			public class TREEFILTERABLETAGS
			{
				// Token: 0x0400D59E RID: 54686
				public static LocString NAME = "{Tags}";

				// Token: 0x0400D59F RID: 54687
				public static LocString TOOLTIP = "{Tags}";
			}

			// Token: 0x0200373D RID: 14141
			public class SPOUTOVERPRESSURE
			{
				// Token: 0x0400D5A0 RID: 54688
				public static LocString NAME = "Overpressure {StudiedDetails}";

				// Token: 0x0400D5A1 RID: 54689
				public static LocString TOOLTIP = "Spout cannot vent due to high environmental pressure";

				// Token: 0x0400D5A2 RID: 54690
				public static LocString STUDIED = "(idle in <b>{Time}</b>)";
			}

			// Token: 0x0200373E RID: 14142
			public class SPOUTEMITTING
			{
				// Token: 0x0400D5A3 RID: 54691
				public static LocString NAME = "Venting {StudiedDetails}";

				// Token: 0x0400D5A4 RID: 54692
				public static LocString TOOLTIP = "This geyser is erupting";

				// Token: 0x0400D5A5 RID: 54693
				public static LocString STUDIED = "(idle in <b>{Time}</b>)";
			}

			// Token: 0x0200373F RID: 14143
			public class SPOUTPRESSUREBUILDING
			{
				// Token: 0x0400D5A6 RID: 54694
				public static LocString NAME = "Rising pressure {StudiedDetails}";

				// Token: 0x0400D5A7 RID: 54695
				public static LocString TOOLTIP = "This geyser's internal pressure is steadily building";

				// Token: 0x0400D5A8 RID: 54696
				public static LocString STUDIED = "(erupts in <b>{Time}</b>)";
			}

			// Token: 0x02003740 RID: 14144
			public class SPOUTIDLE
			{
				// Token: 0x0400D5A9 RID: 54697
				public static LocString NAME = "Idle {StudiedDetails}";

				// Token: 0x0400D5AA RID: 54698
				public static LocString TOOLTIP = "This geyser is not currently erupting";

				// Token: 0x0400D5AB RID: 54699
				public static LocString STUDIED = "(erupts in <b>{Time}</b>)";
			}

			// Token: 0x02003741 RID: 14145
			public class SPOUTDORMANT
			{
				// Token: 0x0400D5AC RID: 54700
				public static LocString NAME = "Dormant";

				// Token: 0x0400D5AD RID: 54701
				public static LocString TOOLTIP = "This geyser's geoactivity has halted\n\nIt won't erupt again for some time";
			}

			// Token: 0x02003742 RID: 14146
			public class SPICEDFOOD
			{
				// Token: 0x0400D5AE RID: 54702
				public static LocString NAME = "Seasoned";

				// Token: 0x0400D5AF RID: 54703
				public static LocString TOOLTIP = "This food has been improved with spice from the " + BUILDINGS.PREFABS.SPICEGRINDER.NAME;
			}

			// Token: 0x02003743 RID: 14147
			public class PICKUPABLEUNREACHABLE
			{
				// Token: 0x0400D5B0 RID: 54704
				public static LocString NAME = "Unreachable";

				// Token: 0x0400D5B1 RID: 54705
				public static LocString TOOLTIP = "Duplicants cannot reach this object";
			}

			// Token: 0x02003744 RID: 14148
			public class PRIORITIZED
			{
				// Token: 0x0400D5B2 RID: 54706
				public static LocString NAME = "High Priority";

				// Token: 0x0400D5B3 RID: 54707
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

			// Token: 0x02003745 RID: 14149
			public class USING
			{
				// Token: 0x0400D5B4 RID: 54708
				public static LocString NAME = "Using {Target}";

				// Token: 0x0400D5B5 RID: 54709
				public static LocString TOOLTIP = "{Target} is currently in use";
			}

			// Token: 0x02003746 RID: 14150
			public class ORDERATTACK
			{
				// Token: 0x0400D5B6 RID: 54710
				public static LocString NAME = "Pending Attack";

				// Token: 0x0400D5B7 RID: 54711
				public static LocString TOOLTIP = "Waiting for a Duplicant to murderize this defenseless " + UI.PRE_KEYWORD + "Critter" + UI.PST_KEYWORD;
			}

			// Token: 0x02003747 RID: 14151
			public class ORDERCAPTURE
			{
				// Token: 0x0400D5B8 RID: 54712
				public static LocString NAME = "Pending Wrangle";

				// Token: 0x0400D5B9 RID: 54713
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

			// Token: 0x02003748 RID: 14152
			public class OPERATING
			{
				// Token: 0x0400D5BA RID: 54714
				public static LocString NAME = "In Use";

				// Token: 0x0400D5BB RID: 54715
				public static LocString TOOLTIP = "This object is currently being used";
			}

			// Token: 0x02003749 RID: 14153
			public class CLEANING
			{
				// Token: 0x0400D5BC RID: 54716
				public static LocString NAME = "Cleaning";

				// Token: 0x0400D5BD RID: 54717
				public static LocString TOOLTIP = "This building is currently being cleaned";
			}

			// Token: 0x0200374A RID: 14154
			public class REGIONISBLOCKED
			{
				// Token: 0x0400D5BE RID: 54718
				public static LocString NAME = "Blocked";

				// Token: 0x0400D5BF RID: 54719
				public static LocString TOOLTIP = "Undug material is blocking off an essential tile";
			}

			// Token: 0x0200374B RID: 14155
			public class STUDIED
			{
				// Token: 0x0400D5C0 RID: 54720
				public static LocString NAME = "Analysis Complete";

				// Token: 0x0400D5C1 RID: 54721
				public static LocString TOOLTIP = "Information on this Natural Feature has been compiled below.";
			}

			// Token: 0x0200374C RID: 14156
			public class AWAITINGSTUDY
			{
				// Token: 0x0400D5C2 RID: 54722
				public static LocString NAME = "Analysis Pending";

				// Token: 0x0400D5C3 RID: 54723
				public static LocString TOOLTIP = "New information on this Natural Feature will be compiled once the field study is complete";
			}

			// Token: 0x0200374D RID: 14157
			public class DURABILITY
			{
				// Token: 0x0400D5C4 RID: 54724
				public static LocString NAME = "Durability: {durability}";

				// Token: 0x0400D5C5 RID: 54725
				public static LocString TOOLTIP = "Items lose durability each time they are equipped, and can no longer be put on by a Duplicant once they reach 0% durability\n\nRepair of this item can be done in the appropriate fabrication station";
			}

			// Token: 0x0200374E RID: 14158
			public class STOREDITEMDURABILITY
			{
				// Token: 0x0400D5C6 RID: 54726
				public static LocString NAME = "Durability: {durability}";

				// Token: 0x0400D5C7 RID: 54727
				public static LocString TOOLTIP = "Items lose durability each time they are equipped, and can no longer be put on by a Duplicant once they reach 0% durability\n\nRepair of this item can be done in the appropriate fabrication station";
			}

			// Token: 0x0200374F RID: 14159
			public class ARTIFACTENTOMBED
			{
				// Token: 0x0400D5C8 RID: 54728
				public static LocString NAME = "Entombed Artifact";

				// Token: 0x0400D5C9 RID: 54729
				public static LocString TOOLTIP = "This artifact is trapped in an obscuring shell limiting its decor. A skilled artist can remove it at the " + BUILDINGS.PREFABS.ARTIFACTANALYSISSTATION.NAME;
			}

			// Token: 0x02003750 RID: 14160
			public class TEAROPEN
			{
				// Token: 0x0400D5CA RID: 54730
				public static LocString NAME = "Temporal Tear open";

				// Token: 0x0400D5CB RID: 54731
				public static LocString TOOLTIP = "An open passage through spacetime";
			}

			// Token: 0x02003751 RID: 14161
			public class TEARCLOSED
			{
				// Token: 0x0400D5CC RID: 54732
				public static LocString NAME = "Temporal Tear closed";

				// Token: 0x0400D5CD RID: 54733
				public static LocString TOOLTIP = "Perhaps some technology could open the passage";
			}

			// Token: 0x02003752 RID: 14162
			public class MARKEDFORMOVE
			{
				// Token: 0x0400D5CE RID: 54734
				public static LocString NAME = "Pending Move";

				// Token: 0x0400D5CF RID: 54735
				public static LocString TOOLTIP = "Waiting for a Duplicant to move this object";
			}

			// Token: 0x02003753 RID: 14163
			public class MOVESTORAGEUNREACHABLE
			{
				// Token: 0x0400D5D0 RID: 54736
				public static LocString NAME = "Unreachable Move";

				// Token: 0x0400D5D1 RID: 54737
				public static LocString TOOLTIP = "Duplicants cannot reach this object to move it";
			}

			// Token: 0x02003754 RID: 14164
			public class PENDINGCARVE
			{
				// Token: 0x0400D5D2 RID: 54738
				public static LocString NAME = "Carve Errand";

				// Token: 0x0400D5D3 RID: 54739
				public static LocString TOOLTIP = "Rock will be carved once a Duplicant is available";
			}
		}

		// Token: 0x02003755 RID: 14165
		public class POPFX
		{
			// Token: 0x0400D5D4 RID: 54740
			public static LocString RESOURCE_EATEN = "Resource Eaten";

			// Token: 0x0400D5D5 RID: 54741
			public static LocString RESOURCE_SELECTION_CHANGED = "Changed to {0}";
		}

		// Token: 0x02003756 RID: 14166
		public class NOTIFICATIONS
		{
			// Token: 0x02003757 RID: 14167
			public class BASICCONTROLS
			{
				// Token: 0x0400D5D6 RID: 54742
				public static LocString NAME = "Tutorial: Basic Controls";

				// Token: 0x0400D5D7 RID: 54743
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

				// Token: 0x0400D5D8 RID: 54744
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

				// Token: 0x0400D5D9 RID: 54745
				public static LocString TOOLTIP = "Notes on using my HUD";
			}

			// Token: 0x02003758 RID: 14168
			public class CODEXUNLOCK
			{
				// Token: 0x0400D5DA RID: 54746
				public static LocString NAME = "New Log Entry";

				// Token: 0x0400D5DB RID: 54747
				public static LocString MESSAGEBODY = "I've added a new log entry to my Database";

				// Token: 0x0400D5DC RID: 54748
				public static LocString TOOLTIP = "I've added a new log entry to my Database";
			}

			// Token: 0x02003759 RID: 14169
			public class WELCOMEMESSAGE
			{
				// Token: 0x0400D5DD RID: 54749
				public static LocString NAME = "Tutorial: Colony Management";

				// Token: 0x0400D5DE RID: 54750
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					"I can use the ",
					UI.FormatAsTool("Dig Tool", global::Action.Dig),
					" and the ",
					UI.FormatAsBuildMenuTab("Build Menu"),
					" in the lower left of the screen to begin planning my first construction tasks.\n\nOnce I've placed a few errands my Duplicants will automatically get to work, without me needing to direct them individually."
				});

				// Token: 0x0400D5DF RID: 54751
				public static LocString TOOLTIP = "Notes on getting Duplicants to do my bidding";
			}

			// Token: 0x0200375A RID: 14170
			public class STRESSMANAGEMENTMESSAGE
			{
				// Token: 0x0400D5E0 RID: 54752
				public static LocString NAME = "Tutorial: Stress Management";

				// Token: 0x0400D5E1 RID: 54753
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

				// Token: 0x0400D5E2 RID: 54754
				public static LocString TOOLTIP = "Notes on keeping Duplicants happy and productive";
			}

			// Token: 0x0200375B RID: 14171
			public class TASKPRIORITIESMESSAGE
			{
				// Token: 0x0400D5E3 RID: 54755
				public static LocString NAME = "Tutorial: Priority";

				// Token: 0x0400D5E4 RID: 54756
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					"Duplicants always perform errands in order of highest to lowest priority. They will harvest ",
					UI.FormatAsLink("Food", "FOOD"),
					" before they build, for example, or always build new structures before they mine materials.\n\nI can open the ",
					UI.FormatAsManagementMenu("Priorities Screen", global::Action.ManagePriorities),
					" to set which Errand Types Duplicants may or may not perform, or to specialize skilled Duplicants for particular Errand Types."
				});

				// Token: 0x0400D5E5 RID: 54757
				public static LocString TOOLTIP = "Notes on managing Duplicants' errands";
			}

			// Token: 0x0200375C RID: 14172
			public class MOPPINGMESSAGE
			{
				// Token: 0x0400D5E6 RID: 54758
				public static LocString NAME = "Tutorial: Polluted Water";

				// Token: 0x0400D5E7 RID: 54759
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

				// Token: 0x0400D5E8 RID: 54760
				public static LocString TOOLTIP = "Notes on handling polluted materials";
			}

			// Token: 0x0200375D RID: 14173
			public class LOCOMOTIONMESSAGE
			{
				// Token: 0x0400D5E9 RID: 54761
				public static LocString NAME = "Video: Duplicant Movement";

				// Token: 0x0400D5EA RID: 54762
				public static LocString MESSAGEBODY = "Duplicants have limited jumping and climbing abilities. They can only climb two tiles high and cannot fit into spaces shorter than two tiles, or cross gaps wider than one tile. I should keep this in mind while placing errands.\n\nTo check if an errand I've placed is accessible, I can select a Duplicant and " + UI.CLICK(UI.ClickType.click) + " <b>Show Navigation</b> to view all areas within their reach.";

				// Token: 0x0400D5EB RID: 54763
				public static LocString TOOLTIP = "Notes on my Duplicants' maneuverability";
			}

			// Token: 0x0200375E RID: 14174
			public class PRIORITIESMESSAGE
			{
				// Token: 0x0400D5EC RID: 54764
				public static LocString NAME = "Tutorial: Errand Priorities";

				// Token: 0x0400D5ED RID: 54765
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

				// Token: 0x0400D5EE RID: 54766
				public static LocString TOOLTIP = "Notes on my Duplicants' priorities";
			}

			// Token: 0x0200375F RID: 14175
			public class FETCHINGWATERMESSAGE
			{
				// Token: 0x0400D5EF RID: 54767
				public static LocString NAME = "Tutorial: Fetching Water";

				// Token: 0x0400D5F0 RID: 54768
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					"By building a ",
					UI.FormatAsLink("Pitcher Pump", "LIQUIDPUMPINGSTATION"),
					" from the ",
					UI.FormatAsBuildMenuTab("Plumbing Tab", global::Action.Plan5),
					" over a pool of liquid, my Duplicants will be able to bottle it up and manually deliver it wherever it needs to go."
				});

				// Token: 0x0400D5F1 RID: 54769
				public static LocString TOOLTIP = "Notes on liquid resource gathering";
			}

			// Token: 0x02003760 RID: 14176
			public class SCHEDULEMESSAGE
			{
				// Token: 0x0400D5F2 RID: 54770
				public static LocString NAME = "Tutorial: Scheduling";

				// Token: 0x0400D5F3 RID: 54771
				public static LocString MESSAGEBODY = "My Duplicants will only eat, sleep, work, or bathe during the times I allot for such activities.\n\nTo make the best use of their time, I can open the " + UI.FormatAsManagementMenu("Schedule Tab", global::Action.ManageSchedule) + " to adjust the colony's schedule and plan how they should utilize their day.";

				// Token: 0x0400D5F4 RID: 54772
				public static LocString TOOLTIP = "Notes on scheduling my Duplicants' time";
			}

			// Token: 0x02003761 RID: 14177
			public class THERMALCOMFORT
			{
				// Token: 0x0400D5F5 RID: 54773
				public static LocString NAME = "Tutorial: Duplicant Temperature";

				// Token: 0x0400D5F6 RID: 54774
				public static LocString TOOLTIP = "Notes on helping Duplicants keep their cool";

				// Token: 0x0400D5F7 RID: 54775
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

			// Token: 0x02003762 RID: 14178
			public class TUTORIAL_OVERHEATING
			{
				// Token: 0x0400D5F8 RID: 54776
				public static LocString NAME = "Tutorial: Building Temperature";

				// Token: 0x0400D5F9 RID: 54777
				public static LocString TOOLTIP = "Notes on preventing building from breaking";

				// Token: 0x0400D5FA RID: 54778
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					"When constructing buildings, I should always take note of their ",
					UI.FormatAsLink("Overheat Temperature", "HEAT"),
					" and plan their locations accordingly. Maintaining low ambient temperatures and good ventilation in the colony will also help keep building temperatures down.\n\nThe <b>Relative Temperature</b> slider tool in the ",
					UI.FormatAsOverlay("Temperature Overlay", global::Action.Overlay3),
					" allows me to change adjust the overlay's color-coding in order to highlight specific temperature ranges.\n\nIf I allow buildings to exceed their Overheat Temperature they will begin to take damage, and if left unattended, they will break down and be unusable until repaired."
				});
			}

			// Token: 0x02003763 RID: 14179
			public class LOTS_OF_GERMS
			{
				// Token: 0x0400D5FB RID: 54779
				public static LocString NAME = "Tutorial: Germs and Disease";

				// Token: 0x0400D5FC RID: 54780
				public static LocString TOOLTIP = "Notes on Duplicant disease risks";

				// Token: 0x0400D5FD RID: 54781
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

			// Token: 0x02003764 RID: 14180
			public class BEING_INFECTED
			{
				// Token: 0x0400D5FE RID: 54782
				public static LocString NAME = "Tutorial: Immune Systems";

				// Token: 0x0400D5FF RID: 54783
				public static LocString TOOLTIP = "Notes on keeping Duplicants in peak health";

				// Token: 0x0400D600 RID: 54784
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

			// Token: 0x02003765 RID: 14181
			public class DISEASE_COOKING
			{
				// Token: 0x0400D601 RID: 54785
				public static LocString NAME = "Tutorial: Food Safety";

				// Token: 0x0400D602 RID: 54786
				public static LocString TOOLTIP = "Notes on managing food contamination";

				// Token: 0x0400D603 RID: 54787
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

			// Token: 0x02003766 RID: 14182
			public class SUITS
			{
				// Token: 0x0400D604 RID: 54788
				public static LocString NAME = "Tutorial: Atmo Suits";

				// Token: 0x0400D605 RID: 54789
				public static LocString TOOLTIP = "Notes on using atmo suits";

				// Token: 0x0400D606 RID: 54790
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

			// Token: 0x02003767 RID: 14183
			public class RADIATION
			{
				// Token: 0x0400D607 RID: 54791
				public static LocString NAME = "Tutorial: Radiation";

				// Token: 0x0400D608 RID: 54792
				public static LocString TOOLTIP = "Notes on managing radiation";

				// Token: 0x0400D609 RID: 54793
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

			// Token: 0x02003768 RID: 14184
			public class SPACETRAVEL
			{
				// Token: 0x0400D60A RID: 54794
				public static LocString NAME = "Tutorial: Space Travel";

				// Token: 0x0400D60B RID: 54795
				public static LocString TOOLTIP = "Notes on traveling in space";

				// Token: 0x0400D60C RID: 54796
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

			// Token: 0x02003769 RID: 14185
			public class MORALE
			{
				// Token: 0x0400D60D RID: 54797
				public static LocString NAME = "Video: Duplicant Morale";

				// Token: 0x0400D60E RID: 54798
				public static LocString TOOLTIP = "Notes on Duplicant expectations";

				// Token: 0x0400D60F RID: 54799
				public static LocString MESSAGEBODY = "Food, Rooms, Decor, and Recreation all have an effect on Duplicant Morale. Good experiences improve their Morale, while poor experiences lower it. When a Duplicant's Morale is below their Expectations, they will become Stressed.\n\nDuplicants' Expectations will get higher as they are given new Skills, and the colony will have to be improved to keep up their Morale. An overview of Morale and Stress can be viewed on the Vitals screen.\n\nRelated " + UI.FormatAsLink("Tutorial: Stress Management", "MISCELLANEOUSTIPS");
			}

			// Token: 0x0200376A RID: 14186
			public class POWER
			{
				// Token: 0x0400D610 RID: 54800
				public static LocString NAME = "Video: Power Circuits";

				// Token: 0x0400D611 RID: 54801
				public static LocString TOOLTIP = "Notes on managing electricity";

				// Token: 0x0400D612 RID: 54802
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					"Generators are considered \"Producers\" of Power, while the various buildings and machines in the colony are considered \"Consumers\". Each Consumer will pull a certain wattage from the power circuit it is connected to, which can be checked at any time by ",
					UI.CLICK(UI.ClickType.clicking),
					" the building and going to the Energy Tab.\n\nI can use the Power Overlay ",
					UI.FormatAsHotKey(global::Action.Overlay2),
					" to quickly check the status of all my circuits. If the Consumers are taking more wattage than the Generators are creating, the Batteries will drain and there will be brownouts.\n\nAdditionally, if the Consumers are pulling more wattage through the Wires than the Wires can handle, they will overload and burn out. To correct both these situations, I will need to reorganize my Consumers onto separate circuits."
				});
			}

			// Token: 0x0200376B RID: 14187
			public class DIGGING
			{
				// Token: 0x0400D613 RID: 54803
				public static LocString NAME = "Video: Digging for Resources";

				// Token: 0x0400D614 RID: 54804
				public static LocString TOOLTIP = "Notes on buried riches";

				// Token: 0x0400D615 RID: 54805
				public static LocString MESSAGEBODY = "Everything a colony needs to get going is found in the ground. Instructing Duplicants to dig out areas means we can find food, mine resources to build infrastructure, and clear space for the colony to grow. I can access the Dig Tool with " + UI.FormatAsHotKey(global::Action.Dig) + ", which allows me to select the area where I want my Duplicants to dig.\n\nDuplicants will need to gain the Superhard Digging skill to mine Abyssalite and the Superduperhard Digging skill to mine Diamond and Obsidian. Without the proper skills, these materials will be undiggable.";
			}

			// Token: 0x0200376C RID: 14188
			public class INSULATION
			{
				// Token: 0x0400D616 RID: 54806
				public static LocString NAME = "Video: Insulation";

				// Token: 0x0400D617 RID: 54807
				public static LocString TOOLTIP = "Notes on effective temperature management";

				// Token: 0x0400D618 RID: 54808
				public static LocString MESSAGEBODY = "The temperature of an environment can have positive or negative effects on the well-being of my Duplicants, as well as the plants and critters in my colony. Selecting " + UI.FormatAsHotKey(global::Action.Overlay3) + " will open the Temperature Overlay where I can check for any hot or cold spots.\n\nI can use a Utility building like an Ice-E Fan or a Space Heater to make an area colder or warmer. However, I will have limited success changing the temperature of a room unless I build the area with insulating tiles to prevent cold or warm air from escaping.";
			}

			// Token: 0x0200376D RID: 14189
			public class PLUMBING
			{
				// Token: 0x0400D619 RID: 54809
				public static LocString NAME = "Video: Plumbing and Ventilation";

				// Token: 0x0400D61A RID: 54810
				public static LocString TOOLTIP = "Notes on connecting buildings with pipes";

				// Token: 0x0400D61B RID: 54811
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					"When connecting pipes for plumbing, it is useful to have the Plumbing Overlay ",
					UI.FormatAsHotKey(global::Action.Overlay6),
					" selected. Each building which requires plumbing must have their Building Intake connected to the Output Pipe from a source such as a Liquid Pump. Liquid Pumps must be submerged in liquid and attached to a power source to function.\n\nBuildings often output contaminated water which must flow out of the building through piping from the Output Pipe. The water can then be expelled through a Liquid Vent, or filtered through a Water Sieve for reuse.\n\nVentilation applies the same principles to gases. Select the Ventilation Overlay ",
					UI.FormatAsHotKey(global::Action.Overlay7),
					" to see how gases are being moved around the colony."
				});
			}

			// Token: 0x0200376E RID: 14190
			public class NEW_AUTOMATION_WARNING
			{
				// Token: 0x0400D61C RID: 54812
				public static LocString NAME = "New Automation Port";

				// Token: 0x0400D61D RID: 54813
				public static LocString TOOLTIP = "This building has a new automation port and is unintentionally connected to an existing " + BUILDINGS.PREFABS.LOGICWIRE.NAME;
			}

			// Token: 0x0200376F RID: 14191
			public class DTU
			{
				// Token: 0x0400D61E RID: 54814
				public static LocString NAME = "Tutorial: Duplicant Thermal Units";

				// Token: 0x0400D61F RID: 54815
				public static LocString TOOLTIP = "Notes on measuring heat energy";

				// Token: 0x0400D620 RID: 54816
				public static LocString MESSAGEBODY = "My Duplicants measure heat energy in Duplicant Thermal Units or DTU.\n\n1 DTU = 1055.06 J";
			}

			// Token: 0x02003770 RID: 14192
			public class NOMESSAGES
			{
				// Token: 0x0400D621 RID: 54817
				public static LocString NAME = "";

				// Token: 0x0400D622 RID: 54818
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02003771 RID: 14193
			public class NOALERTS
			{
				// Token: 0x0400D623 RID: 54819
				public static LocString NAME = "";

				// Token: 0x0400D624 RID: 54820
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02003772 RID: 14194
			public class NEWTRAIT
			{
				// Token: 0x0400D625 RID: 54821
				public static LocString NAME = "{0} has developed a trait";

				// Token: 0x0400D626 RID: 54822
				public static LocString TOOLTIP = "{0} has developed the trait(s):\n    • {1}";
			}

			// Token: 0x02003773 RID: 14195
			public class RESEARCHCOMPLETE
			{
				// Token: 0x0400D627 RID: 54823
				public static LocString NAME = "Research Complete";

				// Token: 0x0400D628 RID: 54824
				public static LocString MESSAGEBODY = "Eureka! We've discovered {0} Technology.\n\nNew buildings have become available:\n  • {1}";

				// Token: 0x0400D629 RID: 54825
				public static LocString TOOLTIP = "{0} research complete!";
			}

			// Token: 0x02003774 RID: 14196
			public class WORLDDETECTED
			{
				// Token: 0x0400D62A RID: 54826
				public static LocString NAME = "New " + UI.CLUSTERMAP.PLANETOID + " detected";

				// Token: 0x0400D62B RID: 54827
				public static LocString MESSAGEBODY = "My Duplicants' astronomical efforts have uncovered a new " + UI.CLUSTERMAP.PLANETOID + ":\n{0}";

				// Token: 0x0400D62C RID: 54828
				public static LocString TOOLTIP = "{0} discovered";
			}

			// Token: 0x02003775 RID: 14197
			public class SKILL_POINT_EARNED
			{
				// Token: 0x0400D62D RID: 54829
				public static LocString NAME = "{Duplicant} earned a skill point!";

				// Token: 0x0400D62E RID: 54830
				public static LocString MESSAGEBODY = "These Duplicants have Skill Points that can be spent on new abilities:\n{0}";

				// Token: 0x0400D62F RID: 54831
				public static LocString LINE = "\n• <b>{0}</b>";

				// Token: 0x0400D630 RID: 54832
				public static LocString TOOLTIP = "{Duplicant} has been working hard and is ready to learn a new skill";
			}

			// Token: 0x02003776 RID: 14198
			public class DUPLICANTABSORBED
			{
				// Token: 0x0400D631 RID: 54833
				public static LocString NAME = "Printables have been reabsorbed";

				// Token: 0x0400D632 RID: 54834
				public static LocString MESSAGEBODY = "The Printing Pod is no longer available for printing.\nCountdown to the next production has been rebooted.";

				// Token: 0x0400D633 RID: 54835
				public static LocString TOOLTIP = "Printing countdown rebooted";
			}

			// Token: 0x02003777 RID: 14199
			public class DUPLICANTDIED
			{
				// Token: 0x0400D634 RID: 54836
				public static LocString NAME = "Duplicants have died";

				// Token: 0x0400D635 RID: 54837
				public static LocString TOOLTIP = "These Duplicants have died:";
			}

			// Token: 0x02003778 RID: 14200
			public class FOODROT
			{
				// Token: 0x0400D636 RID: 54838
				public static LocString NAME = "Food has decayed";

				// Token: 0x0400D637 RID: 54839
				public static LocString TOOLTIP = "These " + UI.FormatAsLink("Food", "FOOD") + " items have rotted and are no longer edible:{0}";
			}

			// Token: 0x02003779 RID: 14201
			public class FOODSTALE
			{
				// Token: 0x0400D638 RID: 54840
				public static LocString NAME = "Food has become stale";

				// Token: 0x0400D639 RID: 54841
				public static LocString TOOLTIP = "These " + UI.FormatAsLink("Food", "FOOD") + " items have become stale and could rot if not stored:";
			}

			// Token: 0x0200377A RID: 14202
			public class YELLOWALERT
			{
				// Token: 0x0400D63A RID: 54842
				public static LocString NAME = "Yellow Alert";

				// Token: 0x0400D63B RID: 54843
				public static LocString TOOLTIP = "The colony has some top priority tasks to complete before resuming a normal schedule";
			}

			// Token: 0x0200377B RID: 14203
			public class REDALERT
			{
				// Token: 0x0400D63C RID: 54844
				public static LocString NAME = "Red Alert";

				// Token: 0x0400D63D RID: 54845
				public static LocString TOOLTIP = "The colony is prioritizing work over their individual well-being";
			}

			// Token: 0x0200377C RID: 14204
			public class REACTORMELTDOWN
			{
				// Token: 0x0400D63E RID: 54846
				public static LocString NAME = "Reactor Meltdown";

				// Token: 0x0400D63F RID: 54847
				public static LocString TOOLTIP = "A Research Reactor has overheated and is melting down! Extreme radiation is flooding the area";
			}

			// Token: 0x0200377D RID: 14205
			public class HEALING
			{
				// Token: 0x0400D640 RID: 54848
				public static LocString NAME = "Healing";

				// Token: 0x0400D641 RID: 54849
				public static LocString TOOLTIP = "This Duplicant is recovering from an injury";
			}

			// Token: 0x0200377E RID: 14206
			public class UNREACHABLEITEM
			{
				// Token: 0x0400D642 RID: 54850
				public static LocString NAME = "Unreachable resources";

				// Token: 0x0400D643 RID: 54851
				public static LocString TOOLTIP = "Duplicants cannot retrieve these resources:";
			}

			// Token: 0x0200377F RID: 14207
			public class INVALIDCONSTRUCTIONLOCATION
			{
				// Token: 0x0400D644 RID: 54852
				public static LocString NAME = "Invalid construction location";

				// Token: 0x0400D645 RID: 54853
				public static LocString TOOLTIP = "These buildings cannot be constructed in the planned areas:";
			}

			// Token: 0x02003780 RID: 14208
			public class MISSINGMATERIALS
			{
				// Token: 0x0400D646 RID: 54854
				public static LocString NAME = "Missing materials";

				// Token: 0x0400D647 RID: 54855
				public static LocString TOOLTIP = "These resources are not available:";
			}

			// Token: 0x02003781 RID: 14209
			public class BUILDINGOVERHEATED
			{
				// Token: 0x0400D648 RID: 54856
				public static LocString NAME = "Damage: Overheated";

				// Token: 0x0400D649 RID: 54857
				public static LocString TOOLTIP = "Extreme heat is damaging these buildings:";
			}

			// Token: 0x02003782 RID: 14210
			public class TILECOLLAPSE
			{
				// Token: 0x0400D64A RID: 54858
				public static LocString NAME = "Ceiling Collapse!";

				// Token: 0x0400D64B RID: 54859
				public static LocString TOOLTIP = "Falling material fell on these Duplicants and displaced them:";
			}

			// Token: 0x02003783 RID: 14211
			public class NO_OXYGEN_GENERATOR
			{
				// Token: 0x0400D64C RID: 54860
				public static LocString NAME = "No " + UI.FormatAsLink("Oxygen Generator", "OXYGEN") + " built";

				// Token: 0x0400D64D RID: 54861
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

			// Token: 0x02003784 RID: 14212
			public class INSUFFICIENTOXYGENLASTCYCLE
			{
				// Token: 0x0400D64E RID: 54862
				public static LocString NAME = "Insufficient Oxygen generation";

				// Token: 0x0400D64F RID: 54863
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"My colony is consuming more ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					" than it is producing, and will run out air if I do not increase production.\n\nI should check my existing oxygen production buildings to ensure they're operating correctly\n\n• ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					" produced last cycle: {EmittingRate}\n• Consumed last cycle: {ConsumptionRate}"
				});
			}

			// Token: 0x02003785 RID: 14213
			public class UNREFRIGERATEDFOOD
			{
				// Token: 0x0400D650 RID: 54864
				public static LocString NAME = "Unrefrigerated Food";

				// Token: 0x0400D651 RID: 54865
				public static LocString TOOLTIP = "These " + UI.FormatAsLink("Food", "FOOD") + " items are stored but not refrigerated:\n";
			}

			// Token: 0x02003786 RID: 14214
			public class FOODLOW
			{
				// Token: 0x0400D652 RID: 54866
				public static LocString NAME = "Food shortage";

				// Token: 0x0400D653 RID: 54867
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

			// Token: 0x02003787 RID: 14215
			public class NO_MEDICAL_COTS
			{
				// Token: 0x0400D654 RID: 54868
				public static LocString NAME = "No " + UI.FormatAsLink("Sick Bay", "DOCTORSTATION") + " built";

				// Token: 0x0400D655 RID: 54869
				public static LocString TOOLTIP = "There is nowhere for sick Duplicants receive medical care\n\n" + UI.FormatAsLink("Sick Bays", "DOCTORSTATION") + " can be built from the " + UI.FormatAsBuildMenuTab("Medicine Tab", global::Action.Plan8);
			}

			// Token: 0x02003788 RID: 14216
			public class NEEDTOILET
			{
				// Token: 0x0400D656 RID: 54870
				public static LocString NAME = "No " + UI.FormatAsLink("Outhouse", "OUTHOUSE") + " built";

				// Token: 0x0400D657 RID: 54871
				public static LocString TOOLTIP = "My Duplicants have nowhere to relieve themselves\n\n" + UI.FormatAsLink("Outhouses", "OUTHOUSE") + " can be built from the " + UI.FormatAsBuildMenuTab("Plumbing Tab", global::Action.Plan5);
			}

			// Token: 0x02003789 RID: 14217
			public class NEEDFOOD
			{
				// Token: 0x0400D658 RID: 54872
				public static LocString NAME = "Colony requires a food source";

				// Token: 0x0400D659 RID: 54873
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

			// Token: 0x0200378A RID: 14218
			public class HYGENE_NEEDED
			{
				// Token: 0x0400D65A RID: 54874
				public static LocString NAME = "No " + UI.FormatAsLink("Wash Basin", "WASHBASIN") + " built";

				// Token: 0x0400D65B RID: 54875
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					UI.FormatAsLink("Germs", "DISEASE"),
					" are spreading in the colony because my Duplicants have nowhere to clean up\n\n",
					UI.FormatAsLink("Wash Basins", "WASHBASIN"),
					" can be built from the ",
					UI.FormatAsBuildMenuTab("Medicine Tab", global::Action.Plan8)
				});
			}

			// Token: 0x0200378B RID: 14219
			public class NEEDSLEEP
			{
				// Token: 0x0400D65C RID: 54876
				public static LocString NAME = "No " + UI.FormatAsLink("Cots", "BED") + " built";

				// Token: 0x0400D65D RID: 54877
				public static LocString TOOLTIP = "My Duplicants would appreciate a place to sleep\n\n" + UI.FormatAsLink("Cots", "BED") + " can be built from the " + UI.FormatAsBuildMenuTab("Furniture Tab", global::Action.Plan9);
			}

			// Token: 0x0200378C RID: 14220
			public class NEEDENERGYSOURCE
			{
				// Token: 0x0400D65E RID: 54878
				public static LocString NAME = "Colony requires a " + UI.FormatAsLink("Power", "POWER") + " source";

				// Token: 0x0400D65F RID: 54879
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

			// Token: 0x0200378D RID: 14221
			public class RESOURCEMELTED
			{
				// Token: 0x0400D660 RID: 54880
				public static LocString NAME = "Resources melted";

				// Token: 0x0400D661 RID: 54881
				public static LocString TOOLTIP = "These resources have melted:";
			}

			// Token: 0x0200378E RID: 14222
			public class VENTOVERPRESSURE
			{
				// Token: 0x0400D662 RID: 54882
				public static LocString NAME = "Vent overpressurized";

				// Token: 0x0400D663 RID: 54883
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

			// Token: 0x0200378F RID: 14223
			public class VENTBLOCKED
			{
				// Token: 0x0400D664 RID: 54884
				public static LocString NAME = "Vent blocked";

				// Token: 0x0400D665 RID: 54885
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Blocked ",
					UI.PRE_KEYWORD,
					"Pipes",
					UI.PST_KEYWORD,
					" have stopped these systems from functioning:"
				});
			}

			// Token: 0x02003790 RID: 14224
			public class OUTPUTBLOCKED
			{
				// Token: 0x0400D666 RID: 54886
				public static LocString NAME = "Output blocked";

				// Token: 0x0400D667 RID: 54887
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Blocked ",
					UI.PRE_KEYWORD,
					"Pipes",
					UI.PST_KEYWORD,
					" have stopped these systems from functioning:"
				});
			}

			// Token: 0x02003791 RID: 14225
			public class BROKENMACHINE
			{
				// Token: 0x0400D668 RID: 54888
				public static LocString NAME = "Building broken";

				// Token: 0x0400D669 RID: 54889
				public static LocString TOOLTIP = "These buildings have taken significant damage and are nonfunctional:";
			}

			// Token: 0x02003792 RID: 14226
			public class STRUCTURALDAMAGE
			{
				// Token: 0x0400D66A RID: 54890
				public static LocString NAME = "Structural damage";

				// Token: 0x0400D66B RID: 54891
				public static LocString TOOLTIP = "These buildings' structural integrity has been compromised";
			}

			// Token: 0x02003793 RID: 14227
			public class STRUCTURALCOLLAPSE
			{
				// Token: 0x0400D66C RID: 54892
				public static LocString NAME = "Structural collapse";

				// Token: 0x0400D66D RID: 54893
				public static LocString TOOLTIP = "These buildings have collapsed:";
			}

			// Token: 0x02003794 RID: 14228
			public class GASCLOUDWARNING
			{
				// Token: 0x0400D66E RID: 54894
				public static LocString NAME = "A gas cloud approaches";

				// Token: 0x0400D66F RID: 54895
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"A toxic ",
					UI.PRE_KEYWORD,
					"Gas",
					UI.PST_KEYWORD,
					" cloud will soon envelop the colony"
				});
			}

			// Token: 0x02003795 RID: 14229
			public class GASCLOUDARRIVING
			{
				// Token: 0x0400D670 RID: 54896
				public static LocString NAME = "The colony is entering a cloud of gas";

				// Token: 0x0400D671 RID: 54897
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02003796 RID: 14230
			public class GASCLOUDPEAK
			{
				// Token: 0x0400D672 RID: 54898
				public static LocString NAME = "The gas cloud is at its densest point";

				// Token: 0x0400D673 RID: 54899
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02003797 RID: 14231
			public class GASCLOUDDEPARTING
			{
				// Token: 0x0400D674 RID: 54900
				public static LocString NAME = "The gas cloud is receding";

				// Token: 0x0400D675 RID: 54901
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02003798 RID: 14232
			public class GASCLOUDGONE
			{
				// Token: 0x0400D676 RID: 54902
				public static LocString NAME = "The colony is once again in open space";

				// Token: 0x0400D677 RID: 54903
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02003799 RID: 14233
			public class AVAILABLE
			{
				// Token: 0x0400D678 RID: 54904
				public static LocString NAME = "Resource available";

				// Token: 0x0400D679 RID: 54905
				public static LocString TOOLTIP = "These resources have become available:";
			}

			// Token: 0x0200379A RID: 14234
			public class ALLOCATED
			{
				// Token: 0x0400D67A RID: 54906
				public static LocString NAME = "Resource allocated";

				// Token: 0x0400D67B RID: 54907
				public static LocString TOOLTIP = "These resources are reserved for a planned building:";
			}

			// Token: 0x0200379B RID: 14235
			public class INCREASEDEXPECTATIONS
			{
				// Token: 0x0400D67C RID: 54908
				public static LocString NAME = "Duplicants' expectations increased";

				// Token: 0x0400D67D RID: 54909
				public static LocString TOOLTIP = "Duplicants require better amenities over time.\nThese Duplicants have increased their expectations:";
			}

			// Token: 0x0200379C RID: 14236
			public class NEARLYDRY
			{
				// Token: 0x0400D67E RID: 54910
				public static LocString NAME = "Nearly dry";

				// Token: 0x0400D67F RID: 54911
				public static LocString TOOLTIP = "These Duplicants will dry off soon:";
			}

			// Token: 0x0200379D RID: 14237
			public class IMMIGRANTSLEFT
			{
				// Token: 0x0400D680 RID: 54912
				public static LocString NAME = "Printables have been reabsorbed";

				// Token: 0x0400D681 RID: 54913
				public static LocString TOOLTIP = "The care packages have been disintegrated and printable Duplicants have been Oozed";
			}

			// Token: 0x0200379E RID: 14238
			public class LEVELUP
			{
				// Token: 0x0400D682 RID: 54914
				public static LocString NAME = "Attribute increase";

				// Token: 0x0400D683 RID: 54915
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"These Duplicants' ",
					UI.PRE_KEYWORD,
					"Attributes",
					UI.PST_KEYWORD,
					" have improved:"
				});

				// Token: 0x0400D684 RID: 54916
				public static LocString SUFFIX = " - {0} Skill Level modifier raised to +{1}";
			}

			// Token: 0x0200379F RID: 14239
			public class RESETSKILL
			{
				// Token: 0x0400D685 RID: 54917
				public static LocString NAME = "Skills reset";

				// Token: 0x0400D686 RID: 54918
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"These Duplicants have had their ",
					UI.PRE_KEYWORD,
					"Skill Points",
					UI.PST_KEYWORD,
					" refunded:"
				});
			}

			// Token: 0x020037A0 RID: 14240
			public class BADROCKETPATH
			{
				// Token: 0x0400D687 RID: 54919
				public static LocString NAME = "Flight Path Obstructed";

				// Token: 0x0400D688 RID: 54920
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

			// Token: 0x020037A1 RID: 14241
			public class SCHEDULE_CHANGED
			{
				// Token: 0x0400D689 RID: 54921
				public static LocString NAME = "{0}: {1}!";

				// Token: 0x0400D68A RID: 54922
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

			// Token: 0x020037A2 RID: 14242
			public class GENESHUFFLER
			{
				// Token: 0x0400D68B RID: 54923
				public static LocString NAME = "Genes Shuffled";

				// Token: 0x0400D68C RID: 54924
				public static LocString TOOLTIP = "These Duplicants had their genetic makeup modified:";

				// Token: 0x0400D68D RID: 54925
				public static LocString SUFFIX = " has developed " + UI.PRE_KEYWORD + "{0}" + UI.PST_KEYWORD;
			}

			// Token: 0x020037A3 RID: 14243
			public class HEALINGTRAITGAIN
			{
				// Token: 0x0400D68E RID: 54926
				public static LocString NAME = "New trait";

				// Token: 0x0400D68F RID: 54927
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"These Duplicants' injuries weren't set and healed improperly.\nThey developed ",
					UI.PRE_KEYWORD,
					"Traits",
					UI.PST_KEYWORD,
					" as a result:"
				});

				// Token: 0x0400D690 RID: 54928
				public static LocString SUFFIX = " has developed " + UI.PRE_KEYWORD + "{0}" + UI.PST_KEYWORD;
			}

			// Token: 0x020037A4 RID: 14244
			public class COLONYLOST
			{
				// Token: 0x0400D691 RID: 54929
				public static LocString NAME = "Colony Lost";

				// Token: 0x0400D692 RID: 54930
				public static LocString TOOLTIP = "All Duplicants are dead or incapacitated";
			}

			// Token: 0x020037A5 RID: 14245
			public class FABRICATOREMPTY
			{
				// Token: 0x0400D693 RID: 54931
				public static LocString NAME = "Fabricator idle";

				// Token: 0x0400D694 RID: 54932
				public static LocString TOOLTIP = "These fabricators have no recipes queued:";
			}

			// Token: 0x020037A6 RID: 14246
			public class BUILDING_MELTED
			{
				// Token: 0x0400D695 RID: 54933
				public static LocString NAME = "Building melted";

				// Token: 0x0400D696 RID: 54934
				public static LocString TOOLTIP = "Extreme heat has melted these buildings:";
			}

			// Token: 0x020037A7 RID: 14247
			public class SUIT_DROPPED
			{
				// Token: 0x0400D697 RID: 54935
				public static LocString NAME = "No Docks available";

				// Token: 0x0400D698 RID: 54936
				public static LocString TOOLTIP = "An exosuit was dropped because there were no empty docks available";
			}

			// Token: 0x020037A8 RID: 14248
			public class DEATH_SUFFOCATION
			{
				// Token: 0x0400D699 RID: 54937
				public static LocString NAME = "Duplicants suffocated";

				// Token: 0x0400D69A RID: 54938
				public static LocString TOOLTIP = "These Duplicants died from a lack of " + ELEMENTS.OXYGEN.NAME + ":";
			}

			// Token: 0x020037A9 RID: 14249
			public class DEATH_FROZENSOLID
			{
				// Token: 0x0400D69B RID: 54939
				public static LocString NAME = "Duplicants have frozen";

				// Token: 0x0400D69C RID: 54940
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"These Duplicants died from extremely low ",
					UI.PRE_KEYWORD,
					"Temperatures",
					UI.PST_KEYWORD,
					":"
				});
			}

			// Token: 0x020037AA RID: 14250
			public class DEATH_OVERHEATING
			{
				// Token: 0x0400D69D RID: 54941
				public static LocString NAME = "Duplicants have overheated";

				// Token: 0x0400D69E RID: 54942
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"These Duplicants died from extreme ",
					UI.PRE_KEYWORD,
					"Heat",
					UI.PST_KEYWORD,
					":"
				});
			}

			// Token: 0x020037AB RID: 14251
			public class DEATH_STARVATION
			{
				// Token: 0x0400D69F RID: 54943
				public static LocString NAME = "Duplicants have starved";

				// Token: 0x0400D6A0 RID: 54944
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"These Duplicants died from a lack of ",
					UI.PRE_KEYWORD,
					"Food",
					UI.PST_KEYWORD,
					":"
				});
			}

			// Token: 0x020037AC RID: 14252
			public class DEATH_FELL
			{
				// Token: 0x0400D6A1 RID: 54945
				public static LocString NAME = "Duplicants splattered";

				// Token: 0x0400D6A2 RID: 54946
				public static LocString TOOLTIP = "These Duplicants fell to their deaths:";
			}

			// Token: 0x020037AD RID: 14253
			public class DEATH_CRUSHED
			{
				// Token: 0x0400D6A3 RID: 54947
				public static LocString NAME = "Duplicants crushed";

				// Token: 0x0400D6A4 RID: 54948
				public static LocString TOOLTIP = "These Duplicants have been crushed:";
			}

			// Token: 0x020037AE RID: 14254
			public class DEATH_SUFFOCATEDTANKEMPTY
			{
				// Token: 0x0400D6A5 RID: 54949
				public static LocString NAME = "Duplicants have suffocated";

				// Token: 0x0400D6A6 RID: 54950
				public static LocString TOOLTIP = "These Duplicants were unable to reach " + UI.FormatAsLink("Oxygen", "OXYGEN") + " and died:";
			}

			// Token: 0x020037AF RID: 14255
			public class DEATH_SUFFOCATEDAIRTOOHOT
			{
				// Token: 0x0400D6A7 RID: 54951
				public static LocString NAME = "Duplicants have suffocated";

				// Token: 0x0400D6A8 RID: 54952
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"These Duplicants have asphyxiated in ",
					UI.PRE_KEYWORD,
					"Hot",
					UI.PST_KEYWORD,
					" air:"
				});
			}

			// Token: 0x020037B0 RID: 14256
			public class DEATH_SUFFOCATEDAIRTOOCOLD
			{
				// Token: 0x0400D6A9 RID: 54953
				public static LocString NAME = "Duplicants have suffocated";

				// Token: 0x0400D6AA RID: 54954
				public static LocString TOOLTIP = "These Duplicants have asphyxiated in " + UI.FormatAsLink("Cold", "HEAT") + " air:";
			}

			// Token: 0x020037B1 RID: 14257
			public class DEATH_DROWNED
			{
				// Token: 0x0400D6AB RID: 54955
				public static LocString NAME = "Duplicants have drowned";

				// Token: 0x0400D6AC RID: 54956
				public static LocString TOOLTIP = "These Duplicants have drowned:";
			}

			// Token: 0x020037B2 RID: 14258
			public class DEATH_ENTOUMBED
			{
				// Token: 0x0400D6AD RID: 54957
				public static LocString NAME = "Duplicants have been entombed";

				// Token: 0x0400D6AE RID: 54958
				public static LocString TOOLTIP = "These Duplicants are trapped and need assistance:";
			}

			// Token: 0x020037B3 RID: 14259
			public class DEATH_RAPIDDECOMPRESSION
			{
				// Token: 0x0400D6AF RID: 54959
				public static LocString NAME = "Duplicants pressurized";

				// Token: 0x0400D6B0 RID: 54960
				public static LocString TOOLTIP = "These Duplicants died in a low pressure environment:";
			}

			// Token: 0x020037B4 RID: 14260
			public class DEATH_OVERPRESSURE
			{
				// Token: 0x0400D6B1 RID: 54961
				public static LocString NAME = "Duplicants pressurized";

				// Token: 0x0400D6B2 RID: 54962
				public static LocString TOOLTIP = "These Duplicants died in a high pressure environment:";
			}

			// Token: 0x020037B5 RID: 14261
			public class DEATH_POISONED
			{
				// Token: 0x0400D6B3 RID: 54963
				public static LocString NAME = "Duplicants poisoned";

				// Token: 0x0400D6B4 RID: 54964
				public static LocString TOOLTIP = "These Duplicants died as a result of poisoning:";
			}

			// Token: 0x020037B6 RID: 14262
			public class DEATH_DISEASE
			{
				// Token: 0x0400D6B5 RID: 54965
				public static LocString NAME = "Duplicants have succumbed to disease";

				// Token: 0x0400D6B6 RID: 54966
				public static LocString TOOLTIP = "These Duplicants died from an untreated " + UI.FormatAsLink("Disease", "DISEASE") + ":";
			}

			// Token: 0x020037B7 RID: 14263
			public class CIRCUIT_OVERLOADED
			{
				// Token: 0x0400D6B7 RID: 54967
				public static LocString NAME = "Circuit Overloaded";

				// Token: 0x0400D6B8 RID: 54968
				public static LocString TOOLTIP = "These " + BUILDINGS.PREFABS.WIRE.NAME + "s melted due to excessive current demands on their circuits";
			}

			// Token: 0x020037B8 RID: 14264
			public class LOGIC_CIRCUIT_OVERLOADED
			{
				// Token: 0x0400D6B9 RID: 54969
				public static LocString NAME = "Logic Circuit Overloaded";

				// Token: 0x0400D6BA RID: 54970
				public static LocString TOOLTIP = "These " + BUILDINGS.PREFABS.LOGICWIRE.NAME + "s melted due to more bits of data being sent over them than they can support";
			}

			// Token: 0x020037B9 RID: 14265
			public class DISCOVERED_SPACE
			{
				// Token: 0x0400D6BB RID: 54971
				public static LocString NAME = "ALERT - Surface Breach";

				// Token: 0x0400D6BC RID: 54972
				public static LocString TOOLTIP = "Amazing!\n\nMy Duplicants have managed to breach the surface of our rocky prison.\n\nI should be careful; the region is extremely inhospitable and I could easily lose resources to the vacuum of space.";
			}

			// Token: 0x020037BA RID: 14266
			public class COLONY_ACHIEVEMENT_EARNED
			{
				// Token: 0x0400D6BD RID: 54973
				public static LocString NAME = "Colony Achievement earned";

				// Token: 0x0400D6BE RID: 54974
				public static LocString TOOLTIP = "The colony has earned a new achievement.";
			}

			// Token: 0x020037BB RID: 14267
			public class WARP_PORTAL_DUPE_READY
			{
				// Token: 0x0400D6BF RID: 54975
				public static LocString NAME = "Duplicant warp ready";

				// Token: 0x0400D6C0 RID: 54976
				public static LocString TOOLTIP = "{dupe} is ready to warp from the " + BUILDINGS.PREFABS.WARPPORTAL.NAME;
			}

			// Token: 0x020037BC RID: 14268
			public class GENETICANALYSISCOMPLETE
			{
				// Token: 0x0400D6C1 RID: 54977
				public static LocString NAME = "Seed Analysis Complete";

				// Token: 0x0400D6C2 RID: 54978
				public static LocString MESSAGEBODY = "Deeply probing the genes of the {Plant} plant have led to the discovery of a promising new cultivatable mutation:\n\n<b>{Subspecies}</b>\n\n{Info}";

				// Token: 0x0400D6C3 RID: 54979
				public static LocString TOOLTIP = "{Plant} Analysis complete!";
			}

			// Token: 0x020037BD RID: 14269
			public class NEWMUTANTSEED
			{
				// Token: 0x0400D6C4 RID: 54980
				public static LocString NAME = "New Mutant Seed Discovered";

				// Token: 0x0400D6C5 RID: 54981
				public static LocString TOOLTIP = "A new mutant variety of the {Plant} has been found. Analyze it at the " + BUILDINGS.PREFABS.GENETICANALYSISSTATION.NAME + " to learn more!";
			}

			// Token: 0x020037BE RID: 14270
			public class DUPLICANT_CRASH_LANDED
			{
				// Token: 0x0400D6C6 RID: 54982
				public static LocString NAME = "Duplicant Crash Landed!";

				// Token: 0x0400D6C7 RID: 54983
				public static LocString TOOLTIP = "A Duplicant has successfully crashed an Escape Pod onto the surface of a nearby Planetoid.";
			}

			// Token: 0x020037BF RID: 14271
			public class POIRESEARCHUNLOCKCOMPLETE
			{
				// Token: 0x0400D6C8 RID: 54984
				public static LocString NAME = "Research Discovered";

				// Token: 0x0400D6C9 RID: 54985
				public static LocString MESSAGEBODY = "Eureka! We've decrypted the Research Portal's final transmission. New buildings have become available:\n  {0}\n\nOne file was labeled \"Open This First.\" New Database Entry unlocked.";

				// Token: 0x0400D6CA RID: 54986
				public static LocString TOOLTIP = "{0} unlocked!";

				// Token: 0x0400D6CB RID: 54987
				public static LocString BUTTON_VIEW_LORE = "View entry";
			}
		}

		// Token: 0x020037C0 RID: 14272
		public class TUTORIAL
		{
			// Token: 0x0400D6CC RID: 54988
			public static LocString DONT_SHOW_AGAIN = "Don't Show Again";
		}

		// Token: 0x020037C1 RID: 14273
		public class PLACERS
		{
			// Token: 0x020037C2 RID: 14274
			public class DIGPLACER
			{
				// Token: 0x0400D6CD RID: 54989
				public static LocString NAME = "Dig";
			}

			// Token: 0x020037C3 RID: 14275
			public class MOPPLACER
			{
				// Token: 0x0400D6CE RID: 54990
				public static LocString NAME = "Mop";
			}

			// Token: 0x020037C4 RID: 14276
			public class MOVEPICKUPABLEPLACER
			{
				// Token: 0x0400D6CF RID: 54991
				public static LocString NAME = "Move Here";

				// Token: 0x0400D6D0 RID: 54992
				public static LocString PLACER_STATUS = "Next Destination";

				// Token: 0x0400D6D1 RID: 54993
				public static LocString PLACER_STATUS_TOOLTIP = "Click to see where this item will be moved to";
			}
		}

		// Token: 0x020037C5 RID: 14277
		public class MONUMENT_COMPLETE
		{
			// Token: 0x0400D6D2 RID: 54994
			public static LocString NAME = "Great Monument";

			// Token: 0x0400D6D3 RID: 54995
			public static LocString DESC = "A feat of artistic vision and expert engineering that will doubtless inspire Duplicants for thousands of cycles to come";
		}
	}
}
