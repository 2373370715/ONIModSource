using System;

namespace STRINGS
{
	// Token: 0x02002FF6 RID: 12278
	public class BUILDING
	{
		// Token: 0x02002FF7 RID: 12279
		public class STATUSITEMS
		{
			// Token: 0x02002FF8 RID: 12280
			public class GUNKEMPTIERFULL
			{
				// Token: 0x0400C616 RID: 50710
				public static LocString NAME = "Storage Full";

				// Token: 0x0400C617 RID: 50711
				public static LocString TOOLTIP = "This building's internal storage is at maximum capacity\n\nIt must be emptied before its next use";
			}

			// Token: 0x02002FF9 RID: 12281
			public class MERCURYLIGHT_CHARGING
			{
				// Token: 0x0400C618 RID: 50712
				public static LocString NAME = "Powering Up: {0}";

				// Token: 0x0400C619 RID: 50713
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building's ",
					UI.PRE_KEYWORD,
					"Light",
					UI.PST_KEYWORD,
					" levels are gradually increasing\n\nIf its ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					" and ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" requirements continue to be met, it will reach maximum brightness in {0}"
				});
			}

			// Token: 0x02002FFA RID: 12282
			public class MERCURYLIGHT_DEPLEATING
			{
				// Token: 0x0400C61A RID: 50714
				public static LocString NAME = "Brightness: {0}";

				// Token: 0x0400C61B RID: 50715
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building's ",
					UI.PRE_KEYWORD,
					"Light",
					UI.PST_KEYWORD,
					" output is decreasing because its ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					" and ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" requirements are not being met\n\nIt will power off once its stores are depleted"
				});
			}

			// Token: 0x02002FFB RID: 12283
			public class MERCURYLIGHT_DEPLEATED
			{
				// Token: 0x0400C61C RID: 50716
				public static LocString NAME = "Powered Off";

				// Token: 0x0400C61D RID: 50717
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building is non-operational due to a lack of resources\n\nIt will begin to power up when its ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					" and ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" requirements are met"
				});
			}

			// Token: 0x02002FFC RID: 12284
			public class MERCURYLIGHT_CHARGED
			{
				// Token: 0x0400C61E RID: 50718
				public static LocString NAME = "Fully Charged";

				// Token: 0x0400C61F RID: 50719
				public static LocString TOOLTIP = "This building is functioning at maximum capacity";
			}

			// Token: 0x02002FFD RID: 12285
			public class SPECIALCARGOBAYCLUSTERCRITTERSTORED
			{
				// Token: 0x0400C620 RID: 50720
				public static LocString NAME = "Contents: {0}";

				// Token: 0x0400C621 RID: 50721
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002FFE RID: 12286
			public class GEOTUNER_NEEDGEYSER
			{
				// Token: 0x0400C622 RID: 50722
				public static LocString NAME = "No Geyser Selected";

				// Token: 0x0400C623 RID: 50723
				public static LocString TOOLTIP = "Select an analyzed geyser to increase its output";
			}

			// Token: 0x02002FFF RID: 12287
			public class GEOTUNER_CHARGE_REQUIRED
			{
				// Token: 0x0400C624 RID: 50724
				public static LocString NAME = "Experimentation Needed";

				// Token: 0x0400C625 RID: 50725
				public static LocString TOOLTIP = "This building requires a Duplicant to produce amplification data through experimentation";
			}

			// Token: 0x02003000 RID: 12288
			public class GEOTUNER_CHARGING
			{
				// Token: 0x0400C626 RID: 50726
				public static LocString NAME = "Compiling Data";

				// Token: 0x0400C627 RID: 50727
				public static LocString TOOLTIP = "Compiling amplification data through experimentation";
			}

			// Token: 0x02003001 RID: 12289
			public class GEOTUNER_CHARGED
			{
				// Token: 0x0400C628 RID: 50728
				public static LocString NAME = "Data Remaining: {0}";

				// Token: 0x0400C629 RID: 50729
				public static LocString TOOLTIP = "This building consumes amplification data while boosting a geyser\n\nTime remaining: {0} ({1} data per second)";
			}

			// Token: 0x02003002 RID: 12290
			public class GEOTUNER_GEYSER_STATUS
			{
				// Token: 0x0400C62A RID: 50730
				public static LocString NAME = "";

				// Token: 0x0400C62B RID: 50731
				public static LocString NAME_ERUPTING = "Target is Erupting";

				// Token: 0x0400C62C RID: 50732
				public static LocString NAME_DORMANT = "Target is Not Erupting";

				// Token: 0x0400C62D RID: 50733
				public static LocString NAME_IDLE = "Target is Not Erupting";

				// Token: 0x0400C62E RID: 50734
				public static LocString TOOLTIP = "";

				// Token: 0x0400C62F RID: 50735
				public static LocString TOOLTIP_ERUPTING = "The selected geyser is erupting and will receive stored amplification data";

				// Token: 0x0400C630 RID: 50736
				public static LocString TOOLTIP_DORMANT = "The selected geyser is not erupting\n\nIt will not receive stored amplification data in this state";

				// Token: 0x0400C631 RID: 50737
				public static LocString TOOLTIP_IDLE = "The selected geyser is not erupting\n\nIt will not receive stored amplification data in this state";
			}

			// Token: 0x02003003 RID: 12291
			public class GEYSER_GEOTUNED
			{
				// Token: 0x0400C632 RID: 50738
				public static LocString NAME = "Geotuned ({0}/{1})";

				// Token: 0x0400C633 RID: 50739
				public static LocString TOOLTIP = "This geyser is being boosted by {0} out {1} of " + UI.PRE_KEYWORD + "Geotuners" + UI.PST_KEYWORD;
			}

			// Token: 0x02003004 RID: 12292
			public class RADIATOR_ENERGY_CURRENT_EMISSION_RATE
			{
				// Token: 0x0400C634 RID: 50740
				public static LocString NAME = "Currently Emitting: {ENERGY_RATE}";

				// Token: 0x0400C635 RID: 50741
				public static LocString TOOLTIP = "Currently Emitting: {ENERGY_RATE}";
			}

			// Token: 0x02003005 RID: 12293
			public class NOTLINKEDTOHEAD
			{
				// Token: 0x0400C636 RID: 50742
				public static LocString NAME = "Not Linked";

				// Token: 0x0400C637 RID: 50743
				public static LocString TOOLTIP = "This building must be built adjacent to a {headBuilding} or another {linkBuilding} in order to function";
			}

			// Token: 0x02003006 RID: 12294
			public class BAITED
			{
				// Token: 0x0400C638 RID: 50744
				public static LocString NAME = "{0} Bait";

				// Token: 0x0400C639 RID: 50745
				public static LocString TOOLTIP = "This lure is baited with {0}\n\nBait material is set during the construction of the building";
			}

			// Token: 0x02003007 RID: 12295
			public class NOCOOLANT
			{
				// Token: 0x0400C63A RID: 50746
				public static LocString NAME = "No Coolant";

				// Token: 0x0400C63B RID: 50747
				public static LocString TOOLTIP = "This building needs coolant";
			}

			// Token: 0x02003008 RID: 12296
			public class ANGERDAMAGE
			{
				// Token: 0x0400C63C RID: 50748
				public static LocString NAME = "Damage: Duplicant Tantrum";

				// Token: 0x0400C63D RID: 50749
				public static LocString TOOLTIP = "A stressed Duplicant is damaging this building";

				// Token: 0x0400C63E RID: 50750
				public static LocString NOTIFICATION = "Building Damage: Duplicant Tantrum";

				// Token: 0x0400C63F RID: 50751
				public static LocString NOTIFICATION_TOOLTIP = "Stressed Duplicants are damaging these buildings:\n\n{0}";
			}

			// Token: 0x02003009 RID: 12297
			public class PIPECONTENTS
			{
				// Token: 0x0400C640 RID: 50752
				public static LocString EMPTY = "Empty";

				// Token: 0x0400C641 RID: 50753
				public static LocString CONTENTS = "{0} of {1} at {2}";

				// Token: 0x0400C642 RID: 50754
				public static LocString CONTENTS_WITH_DISEASE = "\n  {0}";
			}

			// Token: 0x0200300A RID: 12298
			public class CONVEYOR_CONTENTS
			{
				// Token: 0x0400C643 RID: 50755
				public static LocString EMPTY = "Empty";

				// Token: 0x0400C644 RID: 50756
				public static LocString CONTENTS = "{0} of {1} at {2}";

				// Token: 0x0400C645 RID: 50757
				public static LocString CONTENTS_WITH_DISEASE = "\n  {0}";
			}

			// Token: 0x0200300B RID: 12299
			public class ASSIGNEDTO
			{
				// Token: 0x0400C646 RID: 50758
				public static LocString NAME = "Assigned to: {Assignee}";

				// Token: 0x0400C647 RID: 50759
				public static LocString TOOLTIP = "Only {Assignee} can use this amenity";
			}

			// Token: 0x0200300C RID: 12300
			public class ASSIGNEDPUBLIC
			{
				// Token: 0x0400C648 RID: 50760
				public static LocString NAME = "Assigned to: Public";

				// Token: 0x0400C649 RID: 50761
				public static LocString TOOLTIP = "Any Duplicant can use this amenity";
			}

			// Token: 0x0200300D RID: 12301
			public class ASSIGNEDTOROOM
			{
				// Token: 0x0400C64A RID: 50762
				public static LocString NAME = "Assigned to: {0}";

				// Token: 0x0400C64B RID: 50763
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Any Duplicant assigned to this ",
					UI.PRE_KEYWORD,
					"Room",
					UI.PST_KEYWORD,
					" can use this amenity"
				});
			}

			// Token: 0x0200300E RID: 12302
			public class AWAITINGSEEDDELIVERY
			{
				// Token: 0x0400C64C RID: 50764
				public static LocString NAME = "Awaiting Delivery";

				// Token: 0x0400C64D RID: 50765
				public static LocString TOOLTIP = "Awaiting delivery of selected " + UI.PRE_KEYWORD + "Seed" + UI.PST_KEYWORD;
			}

			// Token: 0x0200300F RID: 12303
			public class AWAITINGBAITDELIVERY
			{
				// Token: 0x0400C64E RID: 50766
				public static LocString NAME = "Awaiting Bait";

				// Token: 0x0400C64F RID: 50767
				public static LocString TOOLTIP = "Awaiting delivery of selected " + UI.PRE_KEYWORD + "Bait" + UI.PST_KEYWORD;
			}

			// Token: 0x02003010 RID: 12304
			public class CLINICOUTSIDEHOSPITAL
			{
				// Token: 0x0400C650 RID: 50768
				public static LocString NAME = "Medical building outside Hospital";

				// Token: 0x0400C651 RID: 50769
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Rebuild this medical equipment in a ",
					UI.PRE_KEYWORD,
					"Hospital",
					UI.PST_KEYWORD,
					" to more effectively quarantine sick Duplicants"
				});
			}

			// Token: 0x02003011 RID: 12305
			public class BOTTLE_EMPTIER
			{
				// Token: 0x02003012 RID: 12306
				public static class ALLOWED
				{
					// Token: 0x0400C652 RID: 50770
					public static LocString NAME = "Auto-Bottle: On";

					// Token: 0x0400C653 RID: 50771
					public static LocString TOOLTIP = string.Concat(new string[]
					{
						"Duplicants may specifically fetch ",
						UI.PRE_KEYWORD,
						"Liquid",
						UI.PST_KEYWORD,
						" from a bottling station to bring to this location"
					});
				}

				// Token: 0x02003013 RID: 12307
				public static class DENIED
				{
					// Token: 0x0400C654 RID: 50772
					public static LocString NAME = "Auto-Bottle: Off";

					// Token: 0x0400C655 RID: 50773
					public static LocString TOOLTIP = string.Concat(new string[]
					{
						"Duplicants may not specifically fetch ",
						UI.PRE_KEYWORD,
						"Liquid",
						UI.PST_KEYWORD,
						" from a bottling station to bring to this location"
					});
				}
			}

			// Token: 0x02003014 RID: 12308
			public class CANISTER_EMPTIER
			{
				// Token: 0x02003015 RID: 12309
				public static class ALLOWED
				{
					// Token: 0x0400C656 RID: 50774
					public static LocString NAME = "Auto-Bottle: On";

					// Token: 0x0400C657 RID: 50775
					public static LocString TOOLTIP = string.Concat(new string[]
					{
						"Duplicants may specifically fetch ",
						UI.PRE_KEYWORD,
						"Gas",
						UI.PST_KEYWORD,
						" from a canister filling station to bring to this location"
					});
				}

				// Token: 0x02003016 RID: 12310
				public static class DENIED
				{
					// Token: 0x0400C658 RID: 50776
					public static LocString NAME = "Auto-Bottle: Off";

					// Token: 0x0400C659 RID: 50777
					public static LocString TOOLTIP = string.Concat(new string[]
					{
						"Duplicants may not specifically fetch ",
						UI.PRE_KEYWORD,
						"Gas",
						UI.PST_KEYWORD,
						" from a canister filling station to bring to this location"
					});
				}
			}

			// Token: 0x02003017 RID: 12311
			public class BROKEN
			{
				// Token: 0x0400C65A RID: 50778
				public static LocString NAME = "Broken";

				// Token: 0x0400C65B RID: 50779
				public static LocString TOOLTIP = "This building received damage from <b>{DamageInfo}</b>\n\nIt will not function until it receives repairs";
			}

			// Token: 0x02003018 RID: 12312
			public class CHANGESTORAGETILETARGET
			{
				// Token: 0x0400C65C RID: 50780
				public static LocString NAME = "Set Storage: {TargetName}";

				// Token: 0x0400C65D RID: 50781
				public static LocString TOOLTIP = "Waiting for a Duplicant to reassign this storage to {TargetName}";

				// Token: 0x0400C65E RID: 50782
				public static LocString EMPTY = "Empty";
			}

			// Token: 0x02003019 RID: 12313
			public class CHANGEDOORCONTROLSTATE
			{
				// Token: 0x0400C65F RID: 50783
				public static LocString NAME = "Pending Door State Change: {ControlState}";

				// Token: 0x0400C660 RID: 50784
				public static LocString TOOLTIP = "Waiting for a Duplicant to change control state";
			}

			// Token: 0x0200301A RID: 12314
			public class DISPENSEREQUESTED
			{
				// Token: 0x0400C661 RID: 50785
				public static LocString NAME = "Dispense Requested";

				// Token: 0x0400C662 RID: 50786
				public static LocString TOOLTIP = "Waiting for a Duplicant to dispense the item";
			}

			// Token: 0x0200301B RID: 12315
			public class SUIT_LOCKER
			{
				// Token: 0x0200301C RID: 12316
				public class NEED_CONFIGURATION
				{
					// Token: 0x0400C663 RID: 50787
					public static LocString NAME = "Current Status: Needs Configuration";

					// Token: 0x0400C664 RID: 50788
					public static LocString TOOLTIP = "Set this dock to store a suit or leave it empty";
				}

				// Token: 0x0200301D RID: 12317
				public class READY
				{
					// Token: 0x0400C665 RID: 50789
					public static LocString NAME = "Current Status: Empty";

					// Token: 0x0400C666 RID: 50790
					public static LocString TOOLTIP = string.Concat(new string[]
					{
						"This dock is ready to receive a ",
						UI.PRE_KEYWORD,
						"Suit",
						UI.PST_KEYWORD,
						", either by manual delivery or from a Duplicant returning the suit they're wearing"
					});
				}

				// Token: 0x0200301E RID: 12318
				public class SUIT_REQUESTED
				{
					// Token: 0x0400C667 RID: 50791
					public static LocString NAME = "Current Status: Awaiting Delivery";

					// Token: 0x0400C668 RID: 50792
					public static LocString TOOLTIP = "Waiting for a Duplicant to deliver a " + UI.PRE_KEYWORD + "Suit" + UI.PST_KEYWORD;
				}

				// Token: 0x0200301F RID: 12319
				public class CHARGING
				{
					// Token: 0x0400C669 RID: 50793
					public static LocString NAME = "Current Status: Charging Suit";

					// Token: 0x0400C66A RID: 50794
					public static LocString TOOLTIP = string.Concat(new string[]
					{
						"This ",
						UI.PRE_KEYWORD,
						"Suit",
						UI.PST_KEYWORD,
						" is docked and refueling"
					});
				}

				// Token: 0x02003020 RID: 12320
				public class NO_OXYGEN
				{
					// Token: 0x0400C66B RID: 50795
					public static LocString NAME = "Current Status: No Oxygen";

					// Token: 0x0400C66C RID: 50796
					public static LocString TOOLTIP = string.Concat(new string[]
					{
						"This dock does not contain enough ",
						ELEMENTS.OXYGEN.NAME,
						" to refill a ",
						UI.PRE_KEYWORD,
						"Suit",
						UI.PST_KEYWORD
					});
				}

				// Token: 0x02003021 RID: 12321
				public class NO_FUEL
				{
					// Token: 0x0400C66D RID: 50797
					public static LocString NAME = "Current Status: No Fuel";

					// Token: 0x0400C66E RID: 50798
					public static LocString TOOLTIP = string.Concat(new string[]
					{
						"This dock does not contain enough ",
						ELEMENTS.PETROLEUM.NAME,
						" to refill a ",
						UI.PRE_KEYWORD,
						"Suit",
						UI.PST_KEYWORD
					});
				}

				// Token: 0x02003022 RID: 12322
				public class NO_COOLANT
				{
					// Token: 0x0400C66F RID: 50799
					public static LocString NAME = "Current Status: No Coolant";

					// Token: 0x0400C670 RID: 50800
					public static LocString TOOLTIP = string.Concat(new string[]
					{
						"This dock does not contain enough ",
						ELEMENTS.WATER.NAME,
						" to refill a ",
						UI.PRE_KEYWORD,
						"Suit",
						UI.PST_KEYWORD
					});
				}

				// Token: 0x02003023 RID: 12323
				public class NOT_OPERATIONAL
				{
					// Token: 0x0400C671 RID: 50801
					public static LocString NAME = "Current Status: Offline";

					// Token: 0x0400C672 RID: 50802
					public static LocString TOOLTIP = "This dock requires " + UI.PRE_KEYWORD + "Power" + UI.PST_KEYWORD;
				}

				// Token: 0x02003024 RID: 12324
				public class FULLY_CHARGED
				{
					// Token: 0x0400C673 RID: 50803
					public static LocString NAME = "Current Status: Full Fueled";

					// Token: 0x0400C674 RID: 50804
					public static LocString TOOLTIP = "This suit is fully refueled and ready for use";
				}
			}

			// Token: 0x02003025 RID: 12325
			public class SUITMARKERTRAVERSALONLYWHENROOMAVAILABLE
			{
				// Token: 0x0400C675 RID: 50805
				public static LocString NAME = "Clearance: Vacancy Only";

				// Token: 0x0400C676 RID: 50806
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Suited Duplicants may pass only if there is room in a ",
					UI.PRE_KEYWORD,
					"Dock",
					UI.PST_KEYWORD,
					" to store their ",
					UI.PRE_KEYWORD,
					"Suit",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x02003026 RID: 12326
			public class SUITMARKERTRAVERSALANYTIME
			{
				// Token: 0x0400C677 RID: 50807
				public static LocString NAME = "Clearance: Always Permitted";

				// Token: 0x0400C678 RID: 50808
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Suited Duplicants may pass even if there is no room to store their ",
					UI.PRE_KEYWORD,
					"Suits",
					UI.PST_KEYWORD,
					"\n\nWhen all available docks are full, Duplicants will unequip their ",
					UI.PRE_KEYWORD,
					"Suits",
					UI.PST_KEYWORD,
					" and drop them on the floor"
				});
			}

			// Token: 0x02003027 RID: 12327
			public class SUIT_LOCKER_NEEDS_CONFIGURATION
			{
				// Token: 0x0400C679 RID: 50809
				public static LocString NAME = "Not Configured";

				// Token: 0x0400C67A RID: 50810
				public static LocString TOOLTIP = "Dock settings not configured";
			}

			// Token: 0x02003028 RID: 12328
			public class CURRENTDOORCONTROLSTATE
			{
				// Token: 0x0400C67B RID: 50811
				public static LocString NAME = "Current State: {ControlState}";

				// Token: 0x0400C67C RID: 50812
				public static LocString TOOLTIP = "Current State: {ControlState}\n\nAuto: Duplicants open and close this door as needed\nLocked: Nothing may pass through\nOpen: This door will remain open";

				// Token: 0x0400C67D RID: 50813
				public static LocString OPENED = "Opened";

				// Token: 0x0400C67E RID: 50814
				public static LocString AUTO = "Auto";

				// Token: 0x0400C67F RID: 50815
				public static LocString LOCKED = "Locked";
			}

			// Token: 0x02003029 RID: 12329
			public class CONDUITBLOCKED
			{
				// Token: 0x0400C680 RID: 50816
				public static LocString NAME = "Pipe Blocked";

				// Token: 0x0400C681 RID: 50817
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Output ",
					UI.PRE_KEYWORD,
					"Pipe",
					UI.PST_KEYWORD,
					" is blocked"
				});
			}

			// Token: 0x0200302A RID: 12330
			public class OUTPUTTILEBLOCKED
			{
				// Token: 0x0400C682 RID: 50818
				public static LocString NAME = "Output Blocked";

				// Token: 0x0400C683 RID: 50819
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Output ",
					UI.PRE_KEYWORD,
					"Pipe",
					UI.PST_KEYWORD,
					" is blocked"
				});
			}

			// Token: 0x0200302B RID: 12331
			public class CONDUITBLOCKEDMULTIPLES
			{
				// Token: 0x0400C684 RID: 50820
				public static LocString NAME = "Pipe Blocked";

				// Token: 0x0400C685 RID: 50821
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Output ",
					UI.PRE_KEYWORD,
					"Pipe",
					UI.PST_KEYWORD,
					" is blocked"
				});
			}

			// Token: 0x0200302C RID: 12332
			public class SOLIDCONDUITBLOCKEDMULTIPLES
			{
				// Token: 0x0400C686 RID: 50822
				public static LocString NAME = "Conveyor Rail Blocked";

				// Token: 0x0400C687 RID: 50823
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Output ",
					UI.PRE_KEYWORD,
					"Conveyor Rail",
					UI.PST_KEYWORD,
					" is blocked"
				});
			}

			// Token: 0x0200302D RID: 12333
			public class OUTPUTPIPEFULL
			{
				// Token: 0x0400C688 RID: 50824
				public static LocString NAME = "Output Pipe Full";

				// Token: 0x0400C689 RID: 50825
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Unable to flush contents, output ",
					UI.PRE_KEYWORD,
					"Pipe",
					UI.PST_KEYWORD,
					" is blocked"
				});
			}

			// Token: 0x0200302E RID: 12334
			public class CONSTRUCTIONUNREACHABLE
			{
				// Token: 0x0400C68A RID: 50826
				public static LocString NAME = "Unreachable Build";

				// Token: 0x0400C68B RID: 50827
				public static LocString TOOLTIP = "Duplicants cannot reach this construction site";
			}

			// Token: 0x0200302F RID: 12335
			public class MOPUNREACHABLE
			{
				// Token: 0x0400C68C RID: 50828
				public static LocString NAME = "Unreachable Mop";

				// Token: 0x0400C68D RID: 50829
				public static LocString TOOLTIP = "Duplicants cannot reach this area";
			}

			// Token: 0x02003030 RID: 12336
			public class DEADREACTORCOOLINGOFF
			{
				// Token: 0x0400C68E RID: 50830
				public static LocString NAME = "Cooling ({CyclesRemaining} cycles remaining)";

				// Token: 0x0400C68F RID: 50831
				public static LocString TOOLTIP = "The radiation coming from this reactor is diminishing";
			}

			// Token: 0x02003031 RID: 12337
			public class DIGUNREACHABLE
			{
				// Token: 0x0400C690 RID: 50832
				public static LocString NAME = "Unreachable Dig";

				// Token: 0x0400C691 RID: 50833
				public static LocString TOOLTIP = "Duplicants cannot reach this area";
			}

			// Token: 0x02003032 RID: 12338
			public class STORAGEUNREACHABLE
			{
				// Token: 0x0400C692 RID: 50834
				public static LocString NAME = "Unreachable Storage";

				// Token: 0x0400C693 RID: 50835
				public static LocString TOOLTIP = "Duplicants cannot reach this storage unit";
			}

			// Token: 0x02003033 RID: 12339
			public class PASSENGERMODULEUNREACHABLE
			{
				// Token: 0x0400C694 RID: 50836
				public static LocString NAME = "Unreachable Module";

				// Token: 0x0400C695 RID: 50837
				public static LocString TOOLTIP = "Duplicants cannot reach this rocket module";
			}

			// Token: 0x02003034 RID: 12340
			public class CONSTRUCTABLEDIGUNREACHABLE
			{
				// Token: 0x0400C696 RID: 50838
				public static LocString NAME = "Unreachable Dig";

				// Token: 0x0400C697 RID: 50839
				public static LocString TOOLTIP = "This construction site contains cells that cannot be dug out";
			}

			// Token: 0x02003035 RID: 12341
			public class EMPTYPUMPINGSTATION
			{
				// Token: 0x0400C698 RID: 50840
				public static LocString NAME = "Empty";

				// Token: 0x0400C699 RID: 50841
				public static LocString TOOLTIP = "This pumping station cannot access any " + UI.PRE_KEYWORD + "Liquid" + UI.PST_KEYWORD;
			}

			// Token: 0x02003036 RID: 12342
			public class ENTOMBED
			{
				// Token: 0x0400C69A RID: 50842
				public static LocString NAME = "Entombed";

				// Token: 0x0400C69B RID: 50843
				public static LocString TOOLTIP = "Must be dug out by a Duplicant";

				// Token: 0x0400C69C RID: 50844
				public static LocString NOTIFICATION_NAME = "Building entombment";

				// Token: 0x0400C69D RID: 50845
				public static LocString NOTIFICATION_TOOLTIP = "These buildings are entombed and need to be dug out:";
			}

			// Token: 0x02003037 RID: 12343
			public class ELECTROBANKJOULESAVAILABLE
			{
				// Token: 0x0400C69E RID: 50846
				public static LocString NAME = "Power Remaining: {JoulesAvailable} / {JoulesCapacity}";

				// Token: 0x0400C69F RID: 50847
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"<b>{JoulesAvailable}</b> of stored ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" available for use\n\nMaximum capacity: {JoulesCapacity}"
				});
			}

			// Token: 0x02003038 RID: 12344
			public class FABRICATORACCEPTSMUTANTSEEDS
			{
				// Token: 0x0400C6A0 RID: 50848
				public static LocString NAME = "Fabricator accepts mutant seeds";

				// Token: 0x0400C6A1 RID: 50849
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This fabricator is allowed to use ",
					UI.PRE_KEYWORD,
					"Mutant Seeds",
					UI.PST_KEYWORD,
					" as recipe ingredients"
				});
			}

			// Token: 0x02003039 RID: 12345
			public class FISHFEEDERACCEPTSMUTANTSEEDS
			{
				// Token: 0x0400C6A2 RID: 50850
				public static LocString NAME = "Fish Feeder accepts mutant seeds";

				// Token: 0x0400C6A3 RID: 50851
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This fish feeder is allowed to use ",
					UI.PRE_KEYWORD,
					"Mutant Seeds",
					UI.PST_KEYWORD,
					" as fish food"
				});
			}

			// Token: 0x0200303A RID: 12346
			public class INVALIDPORTOVERLAP
			{
				// Token: 0x0400C6A4 RID: 50852
				public static LocString NAME = "Invalid Port Overlap";

				// Token: 0x0400C6A5 RID: 50853
				public static LocString TOOLTIP = "Ports on this building overlap those on another building\n\nThis building must be rebuilt in a valid location";

				// Token: 0x0400C6A6 RID: 50854
				public static LocString NOTIFICATION_NAME = "Building has overlapping ports";

				// Token: 0x0400C6A7 RID: 50855
				public static LocString NOTIFICATION_TOOLTIP = "These buildings must be rebuilt with non-overlapping ports:";
			}

			// Token: 0x0200303B RID: 12347
			public class GENESHUFFLECOMPLETED
			{
				// Token: 0x0400C6A8 RID: 50856
				public static LocString NAME = "Vacillation Complete";

				// Token: 0x0400C6A9 RID: 50857
				public static LocString TOOLTIP = "The Duplicant has completed the neural vacillation process and is ready to be released";
			}

			// Token: 0x0200303C RID: 12348
			public class OVERHEATED
			{
				// Token: 0x0400C6AA RID: 50858
				public static LocString NAME = "Damage: Overheating";

				// Token: 0x0400C6AB RID: 50859
				public static LocString TOOLTIP = "This building is taking damage and will break down if not cooled";
			}

			// Token: 0x0200303D RID: 12349
			public class OVERLOADED
			{
				// Token: 0x0400C6AC RID: 50860
				public static LocString NAME = "Damage: Overloading";

				// Token: 0x0400C6AD RID: 50861
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This ",
					UI.PRE_KEYWORD,
					"Wire",
					UI.PST_KEYWORD,
					" is taking damage because there are too many buildings pulling ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" from this circuit\n\nSplit this ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" circuit into multiple circuits, or use higher quality ",
					UI.PRE_KEYWORD,
					"Wires",
					UI.PST_KEYWORD,
					" to prevent overloading"
				});
			}

			// Token: 0x0200303E RID: 12350
			public class LOGICOVERLOADED
			{
				// Token: 0x0400C6AE RID: 50862
				public static LocString NAME = "Damage: Overloading";

				// Token: 0x0400C6AF RID: 50863
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This ",
					UI.PRE_KEYWORD,
					"Logic Wire",
					UI.PST_KEYWORD,
					" is taking damage\n\nLimit the output to one Bit, or replace it with ",
					UI.PRE_KEYWORD,
					"Logic Ribbon",
					UI.PST_KEYWORD,
					" to prevent further damage"
				});
			}

			// Token: 0x0200303F RID: 12351
			public class OPERATINGENERGY
			{
				// Token: 0x0400C6B0 RID: 50864
				public static LocString NAME = "Heat Production: {0}/s";

				// Token: 0x0400C6B1 RID: 50865
				public static LocString TOOLTIP = "This building is producing <b>{0}</b> per second\n\nSources:\n{1}";

				// Token: 0x0400C6B2 RID: 50866
				public static LocString LINEITEM = "    • {0}: {1}\n";

				// Token: 0x0400C6B3 RID: 50867
				public static LocString OPERATING = "Normal operation";

				// Token: 0x0400C6B4 RID: 50868
				public static LocString EXHAUSTING = "Excess produced";

				// Token: 0x0400C6B5 RID: 50869
				public static LocString PIPECONTENTS_TRANSFER = "Transferred from pipes";

				// Token: 0x0400C6B6 RID: 50870
				public static LocString FOOD_TRANSFER = "Internal Cooling";
			}

			// Token: 0x02003040 RID: 12352
			public class FLOODED
			{
				// Token: 0x0400C6B7 RID: 50871
				public static LocString NAME = "Building Flooded";

				// Token: 0x0400C6B8 RID: 50872
				public static LocString TOOLTIP = "Building cannot function at current saturation";

				// Token: 0x0400C6B9 RID: 50873
				public static LocString NOTIFICATION_NAME = "Flooding";

				// Token: 0x0400C6BA RID: 50874
				public static LocString NOTIFICATION_TOOLTIP = "These buildings are flooded:";
			}

			// Token: 0x02003041 RID: 12353
			public class NOTSUBMERGED
			{
				// Token: 0x0400C6BB RID: 50875
				public static LocString NAME = "Building Not Submerged";

				// Token: 0x0400C6BC RID: 50876
				public static LocString TOOLTIP = "Building cannot function unless submerged in liquid";
			}

			// Token: 0x02003042 RID: 12354
			public class GASVENTOBSTRUCTED
			{
				// Token: 0x0400C6BD RID: 50877
				public static LocString NAME = "Gas Vent Obstructed";

				// Token: 0x0400C6BE RID: 50878
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"A ",
					UI.PRE_KEYWORD,
					"Pipe",
					UI.PST_KEYWORD,
					" has been obstructed and is preventing ",
					UI.PRE_KEYWORD,
					"Gas",
					UI.PST_KEYWORD,
					" flow to this vent"
				});
			}

			// Token: 0x02003043 RID: 12355
			public class GASVENTOVERPRESSURE
			{
				// Token: 0x0400C6BF RID: 50879
				public static LocString NAME = "Gas Vent Overpressure";

				// Token: 0x0400C6C0 RID: 50880
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"High ",
					UI.PRE_KEYWORD,
					"Gas",
					UI.PST_KEYWORD,
					" or ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					" pressure in this area is preventing further ",
					UI.PRE_KEYWORD,
					"Gas",
					UI.PST_KEYWORD,
					" emission\nReduce pressure by pumping ",
					UI.PRE_KEYWORD,
					"Gas",
					UI.PST_KEYWORD,
					" away or clearing more space"
				});
			}

			// Token: 0x02003044 RID: 12356
			public class DIRECTION_CONTROL
			{
				// Token: 0x0400C6C1 RID: 50881
				public static LocString NAME = "Use Direction: {Direction}";

				// Token: 0x0400C6C2 RID: 50882
				public static LocString TOOLTIP = "Duplicants will only use this building when walking by it\n\nCurrently allowed direction: <b>{Direction}</b>";

				// Token: 0x02003045 RID: 12357
				public static class DIRECTIONS
				{
					// Token: 0x0400C6C3 RID: 50883
					public static LocString LEFT = "Left";

					// Token: 0x0400C6C4 RID: 50884
					public static LocString RIGHT = "Right";

					// Token: 0x0400C6C5 RID: 50885
					public static LocString BOTH = "Both";
				}
			}

			// Token: 0x02003046 RID: 12358
			public class WATTSONGAMEOVER
			{
				// Token: 0x0400C6C6 RID: 50886
				public static LocString NAME = "Colony Lost";

				// Token: 0x0400C6C7 RID: 50887
				public static LocString TOOLTIP = "All Duplicants are dead or incapacitated";
			}

			// Token: 0x02003047 RID: 12359
			public class INVALIDBUILDINGLOCATION
			{
				// Token: 0x0400C6C8 RID: 50888
				public static LocString NAME = "Invalid Building Location";

				// Token: 0x0400C6C9 RID: 50889
				public static LocString TOOLTIP = "Cannot construct a building in this location";
			}

			// Token: 0x02003048 RID: 12360
			public class LIQUIDVENTOBSTRUCTED
			{
				// Token: 0x0400C6CA RID: 50890
				public static LocString NAME = "Liquid Vent Obstructed";

				// Token: 0x0400C6CB RID: 50891
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"A ",
					UI.PRE_KEYWORD,
					"Pipe",
					UI.PST_KEYWORD,
					" has been obstructed and is preventing ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					" flow to this vent"
				});
			}

			// Token: 0x02003049 RID: 12361
			public class LIQUIDVENTOVERPRESSURE
			{
				// Token: 0x0400C6CC RID: 50892
				public static LocString NAME = "Liquid Vent Overpressure";

				// Token: 0x0400C6CD RID: 50893
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"High ",
					UI.PRE_KEYWORD,
					"Gas",
					UI.PST_KEYWORD,
					" or ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					" pressure in this area is preventing further ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					" emission\nReduce pressure by pumping ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					" away or clearing more space"
				});
			}

			// Token: 0x0200304A RID: 12362
			public class MANUALLYCONTROLLED
			{
				// Token: 0x0400C6CE RID: 50894
				public static LocString NAME = "Manually Controlled";

				// Token: 0x0400C6CF RID: 50895
				public static LocString TOOLTIP = "This Duplicant is under my control";
			}

			// Token: 0x0200304B RID: 12363
			public class LIMITVALVELIMITREACHED
			{
				// Token: 0x0400C6D0 RID: 50896
				public static LocString NAME = "Limit Reached";

				// Token: 0x0400C6D1 RID: 50897
				public static LocString TOOLTIP = "No more Mass can be transferred";
			}

			// Token: 0x0200304C RID: 12364
			public class LIMITVALVELIMITNOTREACHED
			{
				// Token: 0x0400C6D2 RID: 50898
				public static LocString NAME = "Amount remaining: {0}";

				// Token: 0x0400C6D3 RID: 50899
				public static LocString TOOLTIP = "This building will stop transferring Mass when the amount remaining reaches 0";
			}

			// Token: 0x0200304D RID: 12365
			public class MATERIALSUNAVAILABLE
			{
				// Token: 0x0400C6D4 RID: 50900
				public static LocString NAME = "Insufficient Resources\n{ItemsRemaining}";

				// Token: 0x0400C6D5 RID: 50901
				public static LocString TOOLTIP = "Crucial materials for this building are beyond reach or unavailable";

				// Token: 0x0400C6D6 RID: 50902
				public static LocString NOTIFICATION_NAME = "Building lacks resources";

				// Token: 0x0400C6D7 RID: 50903
				public static LocString NOTIFICATION_TOOLTIP = "Crucial materials are unavailable or beyond reach for these buildings:";

				// Token: 0x0400C6D8 RID: 50904
				public static LocString LINE_ITEM_MASS = "• {0}: {1}";

				// Token: 0x0400C6D9 RID: 50905
				public static LocString LINE_ITEM_UNITS = "• {0}";
			}

			// Token: 0x0200304E RID: 12366
			public class MATERIALSUNAVAILABLEFORREFILL
			{
				// Token: 0x0400C6DA RID: 50906
				public static LocString NAME = "Resources Low\n{ItemsRemaining}";

				// Token: 0x0400C6DB RID: 50907
				public static LocString TOOLTIP = "This building will soon require materials that are unavailable";

				// Token: 0x0400C6DC RID: 50908
				public static LocString LINE_ITEM = "• {0}";
			}

			// Token: 0x0200304F RID: 12367
			public class MELTINGDOWN
			{
				// Token: 0x0400C6DD RID: 50909
				public static LocString NAME = "Breaking Down";

				// Token: 0x0400C6DE RID: 50910
				public static LocString TOOLTIP = "This building is collapsing";

				// Token: 0x0400C6DF RID: 50911
				public static LocString NOTIFICATION_NAME = "Building breakdown";

				// Token: 0x0400C6E0 RID: 50912
				public static LocString NOTIFICATION_TOOLTIP = "These buildings are collapsing:";
			}

			// Token: 0x02003050 RID: 12368
			public class MISSINGFOUNDATION
			{
				// Token: 0x0400C6E1 RID: 50913
				public static LocString NAME = "Missing Tile";

				// Token: 0x0400C6E2 RID: 50914
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Build ",
					UI.PRE_KEYWORD,
					"Tile",
					UI.PST_KEYWORD,
					" beneath this building to regain function\n\nTile can be found in the ",
					UI.FormatAsBuildMenuTab("Base Tab", global::Action.Plan1),
					" of the Build Menu"
				});
			}

			// Token: 0x02003051 RID: 12369
			public class NEUTRONIUMUNMINABLE
			{
				// Token: 0x0400C6E3 RID: 50915
				public static LocString NAME = "Cannot Mine";

				// Token: 0x0400C6E4 RID: 50916
				public static LocString TOOLTIP = "This resource cannot be mined by Duplicant tools";
			}

			// Token: 0x02003052 RID: 12370
			public class NEEDGASIN
			{
				// Token: 0x0400C6E5 RID: 50917
				public static LocString NAME = "No Gas Intake\n{GasRequired}";

				// Token: 0x0400C6E6 RID: 50918
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building's ",
					UI.PRE_KEYWORD,
					"Gas Intake",
					UI.PST_KEYWORD,
					" does not have a ",
					BUILDINGS.PREFABS.GASCONDUIT.NAME,
					" connected"
				});

				// Token: 0x0400C6E7 RID: 50919
				public static LocString LINE_ITEM = "• {0}";
			}

			// Token: 0x02003053 RID: 12371
			public class NEEDGASOUT
			{
				// Token: 0x0400C6E8 RID: 50920
				public static LocString NAME = "No Gas Output";

				// Token: 0x0400C6E9 RID: 50921
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building's ",
					UI.PRE_KEYWORD,
					"Gas Output",
					UI.PST_KEYWORD,
					" does not have a ",
					BUILDINGS.PREFABS.GASCONDUIT.NAME,
					" connected"
				});
			}

			// Token: 0x02003054 RID: 12372
			public class NEEDLIQUIDIN
			{
				// Token: 0x0400C6EA RID: 50922
				public static LocString NAME = "No Liquid Intake\n{LiquidRequired}";

				// Token: 0x0400C6EB RID: 50923
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building's ",
					UI.PRE_KEYWORD,
					"Liquid Intake",
					UI.PST_KEYWORD,
					" does not have a ",
					BUILDINGS.PREFABS.LIQUIDCONDUIT.NAME,
					" connected"
				});

				// Token: 0x0400C6EC RID: 50924
				public static LocString LINE_ITEM = "• {0}";
			}

			// Token: 0x02003055 RID: 12373
			public class NEEDLIQUIDOUT
			{
				// Token: 0x0400C6ED RID: 50925
				public static LocString NAME = "No Liquid Output";

				// Token: 0x0400C6EE RID: 50926
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building's ",
					UI.PRE_KEYWORD,
					"Liquid Output",
					UI.PST_KEYWORD,
					" does not have a ",
					BUILDINGS.PREFABS.LIQUIDCONDUIT.NAME,
					" connected"
				});
			}

			// Token: 0x02003056 RID: 12374
			public class LIQUIDPIPEEMPTY
			{
				// Token: 0x0400C6EF RID: 50927
				public static LocString NAME = "Empty Pipe";

				// Token: 0x0400C6F0 RID: 50928
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"There is no ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					" in this pipe"
				});
			}

			// Token: 0x02003057 RID: 12375
			public class LIQUIDPIPEOBSTRUCTED
			{
				// Token: 0x0400C6F1 RID: 50929
				public static LocString NAME = "Not Pumping";

				// Token: 0x0400C6F2 RID: 50930
				public static LocString TOOLTIP = "This pump is not active";
			}

			// Token: 0x02003058 RID: 12376
			public class GASPIPEEMPTY
			{
				// Token: 0x0400C6F3 RID: 50931
				public static LocString NAME = "Empty Pipe";

				// Token: 0x0400C6F4 RID: 50932
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"There is no ",
					UI.PRE_KEYWORD,
					"Gas",
					UI.PST_KEYWORD,
					" in this pipe"
				});
			}

			// Token: 0x02003059 RID: 12377
			public class GASPIPEOBSTRUCTED
			{
				// Token: 0x0400C6F5 RID: 50933
				public static LocString NAME = "Not Pumping";

				// Token: 0x0400C6F6 RID: 50934
				public static LocString TOOLTIP = "This pump is not active";
			}

			// Token: 0x0200305A RID: 12378
			public class NEEDSOLIDIN
			{
				// Token: 0x0400C6F7 RID: 50935
				public static LocString NAME = "No Conveyor Loader";

				// Token: 0x0400C6F8 RID: 50936
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Material cannot be fed onto this Conveyor system for transport\n\nEnter the ",
					UI.FormatAsBuildMenuTab("Shipping Tab", global::Action.Plan13),
					" of the Build Menu to build and connect a ",
					UI.PRE_KEYWORD,
					"Conveyor Loader",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x0200305B RID: 12379
			public class NEEDSOLIDOUT
			{
				// Token: 0x0400C6F9 RID: 50937
				public static LocString NAME = "No Conveyor Receptacle";

				// Token: 0x0400C6FA RID: 50938
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Material cannot be offloaded from this Conveyor system and will backup the rails\n\nEnter the ",
					UI.FormatAsBuildMenuTab("Shipping Tab", global::Action.Plan13),
					" of the Build Menu to build and connect a ",
					UI.PRE_KEYWORD,
					"Conveyor Receptacle",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x0200305C RID: 12380
			public class SOLIDPIPEOBSTRUCTED
			{
				// Token: 0x0400C6FB RID: 50939
				public static LocString NAME = "Conveyor Rail Backup";

				// Token: 0x0400C6FC RID: 50940
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This ",
					UI.PRE_KEYWORD,
					"Conveyor Rail",
					UI.PST_KEYWORD,
					" cannot carry anymore material\n\nRemove material from the ",
					UI.PRE_KEYWORD,
					"Conveyor Receptacle",
					UI.PST_KEYWORD,
					" to free space for more objects"
				});
			}

			// Token: 0x0200305D RID: 12381
			public class NEEDPLANT
			{
				// Token: 0x0400C6FD RID: 50941
				public static LocString NAME = "No Seeds";

				// Token: 0x0400C6FE RID: 50942
				public static LocString TOOLTIP = "Uproot wild plants to obtain seeds";
			}

			// Token: 0x0200305E RID: 12382
			public class NEEDSEED
			{
				// Token: 0x0400C6FF RID: 50943
				public static LocString NAME = "No Seed Selected";

				// Token: 0x0400C700 RID: 50944
				public static LocString TOOLTIP = "Uproot wild plants to obtain seeds";
			}

			// Token: 0x0200305F RID: 12383
			public class NEEDPOWER
			{
				// Token: 0x0400C701 RID: 50945
				public static LocString NAME = "No Power";

				// Token: 0x0400C702 RID: 50946
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"All connected ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" sources have lost charge"
				});
			}

			// Token: 0x02003060 RID: 12384
			public class NOTENOUGHPOWER
			{
				// Token: 0x0400C703 RID: 50947
				public static LocString NAME = "Insufficient Power";

				// Token: 0x0400C704 RID: 50948
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building does not have enough stored ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" to run"
				});
			}

			// Token: 0x02003061 RID: 12385
			public class POWERLOOPDETECTED
			{
				// Token: 0x0400C705 RID: 50949
				public static LocString NAME = "Power Loop Detected";

				// Token: 0x0400C706 RID: 50950
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"A ",
					UI.PRE_KEYWORD,
					"Transformer's",
					UI.PST_KEYWORD,
					" ",
					UI.PRE_KEYWORD,
					"Power Output",
					UI.PST_KEYWORD,
					" has been connected back to its own ",
					UI.PRE_KEYWORD,
					"Input",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x02003062 RID: 12386
			public class NEEDRESOURCE
			{
				// Token: 0x0400C707 RID: 50951
				public static LocString NAME = "Resource Required";

				// Token: 0x0400C708 RID: 50952
				public static LocString TOOLTIP = "This building is missing required materials";
			}

			// Token: 0x02003063 RID: 12387
			public class NEWDUPLICANTSAVAILABLE
			{
				// Token: 0x0400C709 RID: 50953
				public static LocString NAME = "Printables Available";

				// Token: 0x0400C70A RID: 50954
				public static LocString TOOLTIP = "I am ready to print a new colony member or care package";

				// Token: 0x0400C70B RID: 50955
				public static LocString NOTIFICATION_NAME = "New Printables are available";

				// Token: 0x0400C70C RID: 50956
				public static LocString NOTIFICATION_TOOLTIP = "The Printing Pod " + UI.FormatAsHotKey(global::Action.Plan1) + " is ready to print a new Duplicant or care package.\nI'll need to select a blueprint:";
			}

			// Token: 0x02003064 RID: 12388
			public class NOAPPLICABLERESEARCHSELECTED
			{
				// Token: 0x0400C70D RID: 50957
				public static LocString NAME = "Inapplicable Research";

				// Token: 0x0400C70E RID: 50958
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building cannot produce the correct ",
					UI.PRE_KEYWORD,
					"Research Type",
					UI.PST_KEYWORD,
					" for the current ",
					UI.FormatAsLink("Research Focus", "TECH")
				});

				// Token: 0x0400C70F RID: 50959
				public static LocString NOTIFICATION_NAME = UI.FormatAsLink("Research Center", "ADVANCEDRESEARCHCENTER") + " idle";

				// Token: 0x0400C710 RID: 50960
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"These buildings cannot produce the correct ",
					UI.PRE_KEYWORD,
					"Research Type",
					UI.PST_KEYWORD,
					" for the selected ",
					UI.FormatAsLink("Research Focus", "TECH"),
					":"
				});
			}

			// Token: 0x02003065 RID: 12389
			public class NOAPPLICABLEANALYSISSELECTED
			{
				// Token: 0x0400C711 RID: 50961
				public static LocString NAME = "No Analysis Focus Selected";

				// Token: 0x0400C712 RID: 50962
				public static LocString TOOLTIP = "Select an unknown destination from the " + UI.FormatAsManagementMenu("Starmap", global::Action.ManageStarmap) + " to begin analysis";

				// Token: 0x0400C713 RID: 50963
				public static LocString NOTIFICATION_NAME = UI.FormatAsLink("Telescope", "TELESCOPE") + " idle";

				// Token: 0x0400C714 RID: 50964
				public static LocString NOTIFICATION_TOOLTIP = "These buildings require an analysis focus:";
			}

			// Token: 0x02003066 RID: 12390
			public class NOAVAILABLESEED
			{
				// Token: 0x0400C715 RID: 50965
				public static LocString NAME = "No Seed Available";

				// Token: 0x0400C716 RID: 50966
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"The selected ",
					UI.PRE_KEYWORD,
					"Seed",
					UI.PST_KEYWORD,
					" is not available"
				});
			}

			// Token: 0x02003067 RID: 12391
			public class NOSTORAGEFILTERSET
			{
				// Token: 0x0400C717 RID: 50967
				public static LocString NAME = "Filters Not Designated";

				// Token: 0x0400C718 RID: 50968
				public static LocString TOOLTIP = "No resources types are marked for storage in this building";
			}

			// Token: 0x02003068 RID: 12392
			public class NOSUITMARKER
			{
				// Token: 0x0400C719 RID: 50969
				public static LocString NAME = "No Checkpoint";

				// Token: 0x0400C71A RID: 50970
				public static LocString TOOLTIP = "Docks must be placed beside a " + BUILDINGS.PREFABS.CHECKPOINT.NAME + ", opposite the side the checkpoint faces";
			}

			// Token: 0x02003069 RID: 12393
			public class SUITMARKERWRONGSIDE
			{
				// Token: 0x0400C71B RID: 50971
				public static LocString NAME = "Invalid Checkpoint";

				// Token: 0x0400C71C RID: 50972
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building has been built on the wrong side of a ",
					BUILDINGS.PREFABS.CHECKPOINT.NAME,
					"\n\nDocks must be placed beside a ",
					BUILDINGS.PREFABS.CHECKPOINT.NAME,
					", opposite the side the checkpoint faces"
				});
			}

			// Token: 0x0200306A RID: 12394
			public class NOFILTERELEMENTSELECTED
			{
				// Token: 0x0400C71D RID: 50973
				public static LocString NAME = "No Filter Selected";

				// Token: 0x0400C71E RID: 50974
				public static LocString TOOLTIP = "Select a resource to filter";
			}

			// Token: 0x0200306B RID: 12395
			public class NOLUREELEMENTSELECTED
			{
				// Token: 0x0400C71F RID: 50975
				public static LocString NAME = "No Bait Selected";

				// Token: 0x0400C720 RID: 50976
				public static LocString TOOLTIP = "Select a resource to use as bait";
			}

			// Token: 0x0200306C RID: 12396
			public class NOFISHABLEWATERBELOW
			{
				// Token: 0x0400C721 RID: 50977
				public static LocString NAME = "No Fishable Water";

				// Token: 0x0400C722 RID: 50978
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"There are no edible ",
					UI.PRE_KEYWORD,
					"Fish",
					UI.PST_KEYWORD,
					" beneath this structure"
				});
			}

			// Token: 0x0200306D RID: 12397
			public class NOPOWERCONSUMERS
			{
				// Token: 0x0400C723 RID: 50979
				public static LocString NAME = "No Power Consumers";

				// Token: 0x0400C724 RID: 50980
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"No buildings are connected to this ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" source"
				});
			}

			// Token: 0x0200306E RID: 12398
			public class NOWIRECONNECTED
			{
				// Token: 0x0400C725 RID: 50981
				public static LocString NAME = "No Power Wire Connected";

				// Token: 0x0400C726 RID: 50982
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building has not been connected to a ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" grid"
				});
			}

			// Token: 0x0200306F RID: 12399
			public class PENDINGDECONSTRUCTION
			{
				// Token: 0x0400C727 RID: 50983
				public static LocString NAME = "Deconstruction Errand";

				// Token: 0x0400C728 RID: 50984
				public static LocString TOOLTIP = "Building will be deconstructed once a Duplicant is available";
			}

			// Token: 0x02003070 RID: 12400
			public class PENDINGDEMOLITION
			{
				// Token: 0x0400C729 RID: 50985
				public static LocString NAME = "Demolition Errand";

				// Token: 0x0400C72A RID: 50986
				public static LocString TOOLTIP = "Object will be permanently demolished once a Duplicant is available";
			}

			// Token: 0x02003071 RID: 12401
			public class PENDINGFISH
			{
				// Token: 0x0400C72B RID: 50987
				public static LocString NAME = "Fishing Errand";

				// Token: 0x0400C72C RID: 50988
				public static LocString TOOLTIP = "Spot will be fished once a Duplicant is available";
			}

			// Token: 0x02003072 RID: 12402
			public class PENDINGHARVEST
			{
				// Token: 0x0400C72D RID: 50989
				public static LocString NAME = "Harvest Errand";

				// Token: 0x0400C72E RID: 50990
				public static LocString TOOLTIP = "Plant will be harvested once a Duplicant is available";
			}

			// Token: 0x02003073 RID: 12403
			public class PENDINGUPROOT
			{
				// Token: 0x0400C72F RID: 50991
				public static LocString NAME = "Uproot Errand";

				// Token: 0x0400C730 RID: 50992
				public static LocString TOOLTIP = "Plant will be uprooted once a Duplicant is available";
			}

			// Token: 0x02003074 RID: 12404
			public class PENDINGREPAIR
			{
				// Token: 0x0400C731 RID: 50993
				public static LocString NAME = "Repair Errand";

				// Token: 0x0400C732 RID: 50994
				public static LocString TOOLTIP = "Building will be repaired once a Duplicant is available\nReceived damage from {DamageInfo}";
			}

			// Token: 0x02003075 RID: 12405
			public class PENDINGSWITCHTOGGLE
			{
				// Token: 0x0400C733 RID: 50995
				public static LocString NAME = "Settings Errand";

				// Token: 0x0400C734 RID: 50996
				public static LocString TOOLTIP = "Settings will be changed once a Duplicant is available";
			}

			// Token: 0x02003076 RID: 12406
			public class PENDINGWORK
			{
				// Token: 0x0400C735 RID: 50997
				public static LocString NAME = "Work Errand";

				// Token: 0x0400C736 RID: 50998
				public static LocString TOOLTIP = "Building will be operated once a Duplicant is available";
			}

			// Token: 0x02003077 RID: 12407
			public class POWERBUTTONOFF
			{
				// Token: 0x0400C737 RID: 50999
				public static LocString NAME = "Function Suspended";

				// Token: 0x0400C738 RID: 51000
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building has been toggled off\nPress ",
					UI.PRE_KEYWORD,
					"Enable Building",
					UI.PST_KEYWORD,
					" ",
					UI.FormatAsHotKey(global::Action.ToggleEnabled),
					" to resume its use"
				});
			}

			// Token: 0x02003078 RID: 12408
			public class PUMPINGSTATION
			{
				// Token: 0x0400C739 RID: 51001
				public static LocString NAME = "Liquid Available: {Liquids}";

				// Token: 0x0400C73A RID: 51002
				public static LocString TOOLTIP = "This pumping station has access to: {Liquids}";
			}

			// Token: 0x02003079 RID: 12409
			public class PRESSUREOK
			{
				// Token: 0x0400C73B RID: 51003
				public static LocString NAME = "Max Gas Pressure";

				// Token: 0x0400C73C RID: 51004
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"High ambient ",
					UI.PRE_KEYWORD,
					"Gas Pressure",
					UI.PST_KEYWORD,
					" is preventing this building from emitting gas\n\nReduce pressure by pumping ",
					UI.PRE_KEYWORD,
					"Gas",
					UI.PST_KEYWORD,
					" away or clearing more space"
				});
			}

			// Token: 0x0200307A RID: 12410
			public class UNDERPRESSURE
			{
				// Token: 0x0400C73D RID: 51005
				public static LocString NAME = "Low Air Pressure";

				// Token: 0x0400C73E RID: 51006
				public static LocString TOOLTIP = "A minimum atmospheric pressure of <b>{TargetPressure}</b> is needed for this building to operate";
			}

			// Token: 0x0200307B RID: 12411
			public class STORAGELOCKER
			{
				// Token: 0x0400C73F RID: 51007
				public static LocString NAME = "Storing: {Stored} / {Capacity} {Units}";

				// Token: 0x0400C740 RID: 51008
				public static LocString TOOLTIP = "This container is storing <b>{Stored}{Units}</b> of a maximum <b>{Capacity}{Units}</b>";
			}

			// Token: 0x0200307C RID: 12412
			public class CRITTERCAPACITY
			{
				// Token: 0x0400C741 RID: 51009
				public static LocString NAME = "Storing: {Stored} / {Capacity} Critters";

				// Token: 0x0400C742 RID: 51010
				public static LocString TOOLTIP = "This container is storing <b>{Stored} {StoredUnits}</b> of a maximum <b>{Capacity} {CapacityUnits}</b>";

				// Token: 0x0400C743 RID: 51011
				public static LocString UNITS = "Critters";

				// Token: 0x0400C744 RID: 51012
				public static LocString UNIT = "Critter";
			}

			// Token: 0x0200307D RID: 12413
			public class SKILL_POINTS_AVAILABLE
			{
				// Token: 0x0400C745 RID: 51013
				public static LocString NAME = "Skill Points Available";

				// Token: 0x0400C746 RID: 51014
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"A Duplicant has ",
					UI.PRE_KEYWORD,
					"Skill Points",
					UI.PST_KEYWORD,
					" available"
				});
			}

			// Token: 0x0200307E RID: 12414
			public class TANNINGLIGHTSUFFICIENT
			{
				// Token: 0x0400C747 RID: 51015
				public static LocString NAME = "Tanning Light Available";

				// Token: 0x0400C748 RID: 51016
				public static LocString TOOLTIP = "There is sufficient " + UI.FormatAsLink("Light", "LIGHT") + " here to create pleasing skin crisping";
			}

			// Token: 0x0200307F RID: 12415
			public class TANNINGLIGHTINSUFFICIENT
			{
				// Token: 0x0400C749 RID: 51017
				public static LocString NAME = "Insufficient Tanning Light";

				// Token: 0x0400C74A RID: 51018
				public static LocString TOOLTIP = "The " + UI.FormatAsLink("Light", "LIGHT") + " here is not bright enough for that Sunny Day feeling";
			}

			// Token: 0x02003080 RID: 12416
			public class UNASSIGNED
			{
				// Token: 0x0400C74B RID: 51019
				public static LocString NAME = "Unassigned";

				// Token: 0x0400C74C RID: 51020
				public static LocString TOOLTIP = "Assign a Duplicant to use this amenity";
			}

			// Token: 0x02003081 RID: 12417
			public class UNDERCONSTRUCTION
			{
				// Token: 0x0400C74D RID: 51021
				public static LocString NAME = "Under Construction";

				// Token: 0x0400C74E RID: 51022
				public static LocString TOOLTIP = "This building is currently being built";
			}

			// Token: 0x02003082 RID: 12418
			public class UNDERCONSTRUCTIONNOWORKER
			{
				// Token: 0x0400C74F RID: 51023
				public static LocString NAME = "Construction Errand";

				// Token: 0x0400C750 RID: 51024
				public static LocString TOOLTIP = "Building will be constructed once a Duplicant is available";
			}

			// Token: 0x02003083 RID: 12419
			public class WAITINGFORMATERIALS
			{
				// Token: 0x0400C751 RID: 51025
				public static LocString NAME = "Awaiting Delivery\n{ItemsRemaining}";

				// Token: 0x0400C752 RID: 51026
				public static LocString TOOLTIP = "These materials will be delivered once a Duplicant is available";

				// Token: 0x0400C753 RID: 51027
				public static LocString LINE_ITEM_MASS = "• {0}: {1}";

				// Token: 0x0400C754 RID: 51028
				public static LocString LINE_ITEM_UNITS = "• {0}";
			}

			// Token: 0x02003084 RID: 12420
			public class WAITINGFORRADIATION
			{
				// Token: 0x0400C755 RID: 51029
				public static LocString NAME = "Awaiting Radbolts";

				// Token: 0x0400C756 RID: 51030
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building requires Radbolts to function\n\nOpen the ",
					UI.FormatAsOverlay("Radiation Overlay"),
					" ",
					UI.FormatAsHotKey(global::Action.Overlay15),
					" to view this building's radiation port"
				});
			}

			// Token: 0x02003085 RID: 12421
			public class WAITINGFORREPAIRMATERIALS
			{
				// Token: 0x0400C757 RID: 51031
				public static LocString NAME = "Awaiting Repair Delivery\n{ItemsRemaining}\n";

				// Token: 0x0400C758 RID: 51032
				public static LocString TOOLTIP = "These materials must be delivered before this building can be repaired";

				// Token: 0x0400C759 RID: 51033
				public static LocString LINE_ITEM = string.Concat(new string[]
				{
					"• ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					": <b>{1}</b>"
				});
			}

			// Token: 0x02003086 RID: 12422
			public class MISSINGGANTRY
			{
				// Token: 0x0400C75A RID: 51034
				public static LocString NAME = "Missing Gantry";

				// Token: 0x0400C75B RID: 51035
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"A ",
					UI.FormatAsLink("Gantry", "GANTRY"),
					" must be built below ",
					UI.FormatAsLink("Command Capsules", "COMMANDMODULE"),
					" and ",
					UI.FormatAsLink("Sight-Seeing Modules", "TOURISTMODULE"),
					" for Duplicant access"
				});
			}

			// Token: 0x02003087 RID: 12423
			public class DISEMBARKINGDUPLICANT
			{
				// Token: 0x0400C75C RID: 51036
				public static LocString NAME = "Waiting To Disembark";

				// Token: 0x0400C75D RID: 51037
				public static LocString TOOLTIP = "The Duplicant inside this rocket can't come out until the " + UI.FormatAsLink("Gantry", "GANTRY") + " is extended";
			}

			// Token: 0x02003088 RID: 12424
			public class REACTORMELTDOWN
			{
				// Token: 0x0400C75E RID: 51038
				public static LocString NAME = "Reactor Meltdown";

				// Token: 0x0400C75F RID: 51039
				public static LocString TOOLTIP = "This reactor is spilling dangerous radioactive waste and cannot be stopped";
			}

			// Token: 0x02003089 RID: 12425
			public class ROCKETNAME
			{
				// Token: 0x0400C760 RID: 51040
				public static LocString NAME = "Parent Rocket: {0}";

				// Token: 0x0400C761 RID: 51041
				public static LocString TOOLTIP = "This module belongs to the rocket: " + UI.PRE_KEYWORD + "{0}" + UI.PST_KEYWORD;
			}

			// Token: 0x0200308A RID: 12426
			public class HASGANTRY
			{
				// Token: 0x0400C762 RID: 51042
				public static LocString NAME = "Has Gantry";

				// Token: 0x0400C763 RID: 51043
				public static LocString TOOLTIP = "Duplicants may now enter this section of the rocket";
			}

			// Token: 0x0200308B RID: 12427
			public class NORMAL
			{
				// Token: 0x0400C764 RID: 51044
				public static LocString NAME = "Normal";

				// Token: 0x0400C765 RID: 51045
				public static LocString TOOLTIP = "Nothing out of the ordinary here";
			}

			// Token: 0x0200308C RID: 12428
			public class MANUALGENERATORCHARGINGUP
			{
				// Token: 0x0400C766 RID: 51046
				public static LocString NAME = "Charging Up";

				// Token: 0x0400C767 RID: 51047
				public static LocString TOOLTIP = "This power source is being charged";
			}

			// Token: 0x0200308D RID: 12429
			public class MANUALGENERATORRELEASINGENERGY
			{
				// Token: 0x0400C768 RID: 51048
				public static LocString NAME = "Powering";

				// Token: 0x0400C769 RID: 51049
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This generator is supplying energy to ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" consumers"
				});
			}

			// Token: 0x0200308E RID: 12430
			public class GENERATOROFFLINE
			{
				// Token: 0x0400C76A RID: 51050
				public static LocString NAME = "Generator Idle";

				// Token: 0x0400C76B RID: 51051
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" source is idle"
				});
			}

			// Token: 0x0200308F RID: 12431
			public class PIPE
			{
				// Token: 0x0400C76C RID: 51052
				public static LocString NAME = "Contents: {Contents}";

				// Token: 0x0400C76D RID: 51053
				public static LocString TOOLTIP = "This pipe is delivering {Contents}";
			}

			// Token: 0x02003090 RID: 12432
			public class CONVEYOR
			{
				// Token: 0x0400C76E RID: 51054
				public static LocString NAME = "Contents: {Contents}";

				// Token: 0x0400C76F RID: 51055
				public static LocString TOOLTIP = "This conveyor is delivering {Contents}";
			}

			// Token: 0x02003091 RID: 12433
			public class FABRICATORIDLE
			{
				// Token: 0x0400C770 RID: 51056
				public static LocString NAME = "No Fabrications Queued";

				// Token: 0x0400C771 RID: 51057
				public static LocString TOOLTIP = "Select a recipe to begin fabrication";
			}

			// Token: 0x02003092 RID: 12434
			public class FABRICATOREMPTY
			{
				// Token: 0x0400C772 RID: 51058
				public static LocString NAME = "Waiting For Materials";

				// Token: 0x0400C773 RID: 51059
				public static LocString TOOLTIP = "Fabrication will begin once materials have been delivered";
			}

			// Token: 0x02003093 RID: 12435
			public class FABRICATORLACKSHEP
			{
				// Token: 0x0400C774 RID: 51060
				public static LocString NAME = "Waiting For Radbolts ({CurrentHEP}/{HEPRequired})";

				// Token: 0x0400C775 RID: 51061
				public static LocString TOOLTIP = "A queued recipe requires more Radbolts than are currently stored.\n\nCurrently stored: {CurrentHEP}\nRequired for recipe: {HEPRequired}";
			}

			// Token: 0x02003094 RID: 12436
			public class TOILET
			{
				// Token: 0x0400C776 RID: 51062
				public static LocString NAME = "{FlushesRemaining} \"Visits\" Remaining";

				// Token: 0x0400C777 RID: 51063
				public static LocString TOOLTIP = "{FlushesRemaining} more Duplicants can use this amenity before it requires maintenance";
			}

			// Token: 0x02003095 RID: 12437
			public class TOILETNEEDSEMPTYING
			{
				// Token: 0x0400C778 RID: 51064
				public static LocString NAME = "Requires Emptying";

				// Token: 0x0400C779 RID: 51065
				public static LocString TOOLTIP = "This amenity cannot be used while full\n\nEmptying it will produce " + UI.FormatAsLink("Polluted Dirt", "TOXICSAND");
			}

			// Token: 0x02003096 RID: 12438
			public class DESALINATORNEEDSEMPTYING
			{
				// Token: 0x0400C77A RID: 51066
				public static LocString NAME = "Requires Emptying";

				// Token: 0x0400C77B RID: 51067
				public static LocString TOOLTIP = "This building needs to be emptied of " + UI.FormatAsLink("Salt", "SALT") + " to resume function";
			}

			// Token: 0x02003097 RID: 12439
			public class MILKSEPARATORNEEDSEMPTYING
			{
				// Token: 0x0400C77C RID: 51068
				public static LocString NAME = "Requires Emptying";

				// Token: 0x0400C77D RID: 51069
				public static LocString TOOLTIP = "This building needs to be emptied of " + UI.FormatAsLink("Brackwax", "MILKFAT") + " to resume function";
			}

			// Token: 0x02003098 RID: 12440
			public class HABITATNEEDSEMPTYING
			{
				// Token: 0x0400C77E RID: 51070
				public static LocString NAME = "Requires Emptying";

				// Token: 0x0400C77F RID: 51071
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This ",
					UI.FormatAsLink("Algae Terrarium", "ALGAEHABITAT"),
					" needs to be emptied of ",
					UI.FormatAsLink("Polluted Water", "DIRTYWATER"),
					"\n\n",
					UI.FormatAsLink("Bottle Emptiers", "BOTTLEEMPTIER"),
					" can be used to transport and dispose of ",
					UI.FormatAsLink("Polluted Water", "DIRTYWATER"),
					" in designated areas"
				});
			}

			// Token: 0x02003099 RID: 12441
			public class UNUSABLE
			{
				// Token: 0x0400C780 RID: 51072
				public static LocString NAME = "Out of Order";

				// Token: 0x0400C781 RID: 51073
				public static LocString TOOLTIP = "This amenity requires maintenance";
			}

			// Token: 0x0200309A RID: 12442
			public class NORESEARCHSELECTED
			{
				// Token: 0x0400C782 RID: 51074
				public static LocString NAME = "No Research Focus Selected";

				// Token: 0x0400C783 RID: 51075
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Open the ",
					UI.FormatAsManagementMenu("Research Tree", global::Action.ManageResearch),
					" to select a new ",
					UI.FormatAsLink("Research", "TECH"),
					" project"
				});

				// Token: 0x0400C784 RID: 51076
				public static LocString NOTIFICATION_NAME = "No " + UI.FormatAsLink("Research Focus", "TECH") + " selected";

				// Token: 0x0400C785 RID: 51077
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"Open the ",
					UI.FormatAsManagementMenu("Research Tree", global::Action.ManageResearch),
					" to select a new ",
					UI.FormatAsLink("Research", "TECH"),
					" project"
				});
			}

			// Token: 0x0200309B RID: 12443
			public class NORESEARCHORDESTINATIONSELECTED
			{
				// Token: 0x0400C786 RID: 51078
				public static LocString NAME = "No Research Focus or Starmap Destination Selected";

				// Token: 0x0400C787 RID: 51079
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Select a ",
					UI.FormatAsLink("Research", "TECH"),
					" project in the ",
					UI.FormatAsManagementMenu("Research Tree", global::Action.ManageResearch),
					" or a Destination in the ",
					UI.FormatAsManagementMenu("Starmap", global::Action.ManageStarmap)
				});

				// Token: 0x0400C788 RID: 51080
				public static LocString NOTIFICATION_NAME = "No " + UI.FormatAsLink("Research Focus", "TECH") + " or Starmap destination selected";

				// Token: 0x0400C789 RID: 51081
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"Select a ",
					UI.FormatAsLink("Research", "TECH"),
					" project in the ",
					UI.FormatAsManagementMenu("Research Tree", "[R]"),
					" or a Destination in the ",
					UI.FormatAsManagementMenu("Starmap", "[Z]")
				});
			}

			// Token: 0x0200309C RID: 12444
			public class RESEARCHING
			{
				// Token: 0x0400C78A RID: 51082
				public static LocString NAME = "Current " + UI.FormatAsLink("Research", "TECH") + ": {Tech}";

				// Token: 0x0400C78B RID: 51083
				public static LocString TOOLTIP = "Research produced at this station will be invested in {Tech}";
			}

			// Token: 0x0200309D RID: 12445
			public class TINKERING
			{
				// Token: 0x0400C78C RID: 51084
				public static LocString NAME = "Tinkering: {0}";

				// Token: 0x0400C78D RID: 51085
				public static LocString TOOLTIP = "This Duplicant is creating {0} to use somewhere else";
			}

			// Token: 0x0200309E RID: 12446
			public class VALVE
			{
				// Token: 0x0400C78E RID: 51086
				public static LocString NAME = "Max Flow Rate: {MaxFlow}";

				// Token: 0x0400C78F RID: 51087
				public static LocString TOOLTIP = "This valve is allowing flow at a volume of <b>{MaxFlow}</b>";
			}

			// Token: 0x0200309F RID: 12447
			public class VALVEREQUEST
			{
				// Token: 0x0400C790 RID: 51088
				public static LocString NAME = "Requested Flow Rate: {QueuedMaxFlow}";

				// Token: 0x0400C791 RID: 51089
				public static LocString TOOLTIP = "Waiting for a Duplicant to adjust flow rate";
			}

			// Token: 0x020030A0 RID: 12448
			public class EMITTINGLIGHT
			{
				// Token: 0x0400C792 RID: 51090
				public static LocString NAME = "Emitting Light";

				// Token: 0x0400C793 RID: 51091
				public static LocString TOOLTIP = "Open the " + UI.FormatAsOverlay("Light Overlay", global::Action.Overlay5) + " to view this light's visibility radius";
			}

			// Token: 0x020030A1 RID: 12449
			public class KETTLEINSUFICIENTSOLIDS
			{
				// Token: 0x0400C794 RID: 51092
				public static LocString NAME = "Insufficient " + UI.FormatAsLink("Ice", "ICE");

				// Token: 0x0400C795 RID: 51093
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building requires a minimum of {0} ",
					UI.FormatAsLink("Ice", "ICE"),
					" in order to function\n\nDeliver more ",
					UI.FormatAsLink("Ice", "ICE"),
					" to operate this building"
				});
			}

			// Token: 0x020030A2 RID: 12450
			public class KETTLEINSUFICIENTFUEL
			{
				// Token: 0x0400C796 RID: 51094
				public static LocString NAME = "Insufficient " + UI.FormatAsLink("Wood", "WOODLOG");

				// Token: 0x0400C797 RID: 51095
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Colder ",
					UI.FormatAsLink("Ice", "ICE"),
					" increases the amount of ",
					UI.FormatAsLink("Wood", "WOODLOG"),
					" required for melting\n\nCurrent requirement: minimum {0} ",
					UI.FormatAsLink("Wood", "WOODLOG")
				});
			}

			// Token: 0x020030A3 RID: 12451
			public class KETTLEINSUFICIENTLIQUIDSPACE
			{
				// Token: 0x0400C798 RID: 51096
				public static LocString NAME = "Requires Emptying";

				// Token: 0x0400C799 RID: 51097
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This ",
					UI.FormatAsLink("Ice Liquefier", "ICEKETTLE"),
					" needs to be emptied of ",
					UI.FormatAsLink("Water", "WATER"),
					" in order to resume function\n\nIt requires at least {2} of storage space in order to function properly\n\nCurrently storing {0} of a maximum {1} ",
					UI.FormatAsLink("Water", "WATER")
				});
			}

			// Token: 0x020030A4 RID: 12452
			public class KETTLEMELTING
			{
				// Token: 0x0400C79A RID: 51098
				public static LocString NAME = "Melting Ice";

				// Token: 0x0400C79B RID: 51099
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building is currently melting stored ",
					UI.FormatAsLink("Ice", "ICE"),
					" to produce ",
					UI.FormatAsLink("Water", "WATER"),
					"\n\n",
					UI.FormatAsLink("Water", "WATER"),
					" output temperature: {0}"
				});
			}

			// Token: 0x020030A5 RID: 12453
			public class RATIONBOXCONTENTS
			{
				// Token: 0x0400C79C RID: 51100
				public static LocString NAME = "Storing: {Stored}";

				// Token: 0x0400C79D RID: 51101
				public static LocString TOOLTIP = "This box contains <b>{Stored}</b> of " + UI.PRE_KEYWORD + "Food" + UI.PST_KEYWORD;
			}

			// Token: 0x020030A6 RID: 12454
			public class EMITTINGELEMENT
			{
				// Token: 0x0400C79E RID: 51102
				public static LocString NAME = "Emitting {ElementType}: {FlowRate}";

				// Token: 0x0400C79F RID: 51103
				public static LocString TOOLTIP = "Producing {ElementType} at " + UI.FormatAsPositiveRate("{FlowRate}");
			}

			// Token: 0x020030A7 RID: 12455
			public class EMITTINGCO2
			{
				// Token: 0x0400C7A0 RID: 51104
				public static LocString NAME = "Emitting CO<sub>2</sub>: {FlowRate}";

				// Token: 0x0400C7A1 RID: 51105
				public static LocString TOOLTIP = "Producing " + ELEMENTS.CARBONDIOXIDE.NAME + " at " + UI.FormatAsPositiveRate("{FlowRate}");
			}

			// Token: 0x020030A8 RID: 12456
			public class EMITTINGOXYGENAVG
			{
				// Token: 0x0400C7A2 RID: 51106
				public static LocString NAME = "Emitting " + UI.FormatAsLink("Oxygen", "OXYGEN") + ": {FlowRate}";

				// Token: 0x0400C7A3 RID: 51107
				public static LocString TOOLTIP = "Producing " + ELEMENTS.OXYGEN.NAME + " at a rate of " + UI.FormatAsPositiveRate("{FlowRate}");
			}

			// Token: 0x020030A9 RID: 12457
			public class EMITTINGGASAVG
			{
				// Token: 0x0400C7A4 RID: 51108
				public static LocString NAME = "Emitting {Element}: {FlowRate}";

				// Token: 0x0400C7A5 RID: 51109
				public static LocString TOOLTIP = "Producing {Element} at a rate of " + UI.FormatAsPositiveRate("{FlowRate}");
			}

			// Token: 0x020030AA RID: 12458
			public class EMITTINGBLOCKEDHIGHPRESSURE
			{
				// Token: 0x0400C7A6 RID: 51110
				public static LocString NAME = "Not Emitting: Overpressure";

				// Token: 0x0400C7A7 RID: 51111
				public static LocString TOOLTIP = "Ambient pressure is too high for {Element} to be released";
			}

			// Token: 0x020030AB RID: 12459
			public class EMITTINGBLOCKEDLOWTEMPERATURE
			{
				// Token: 0x0400C7A8 RID: 51112
				public static LocString NAME = "Not Emitting: Too Cold";

				// Token: 0x0400C7A9 RID: 51113
				public static LocString TOOLTIP = "Temperature is too low for {Element} to be released";
			}

			// Token: 0x020030AC RID: 12460
			public class PUMPINGLIQUIDORGAS
			{
				// Token: 0x0400C7AA RID: 51114
				public static LocString NAME = "Average Flow Rate: {FlowRate}";

				// Token: 0x0400C7AB RID: 51115
				public static LocString TOOLTIP = "This building is pumping an average volume of " + UI.FormatAsPositiveRate("{FlowRate}");
			}

			// Token: 0x020030AD RID: 12461
			public class WIRECIRCUITSTATUS
			{
				// Token: 0x0400C7AC RID: 51116
				public static LocString NAME = "Current Load: {CurrentLoadAndColor} / {MaxLoad}";

				// Token: 0x0400C7AD RID: 51117
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"The current ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" load on this wire\n\nOverloading a wire will cause damage to the wire over time and cause it to break"
				});
			}

			// Token: 0x020030AE RID: 12462
			public class WIREMAXWATTAGESTATUS
			{
				// Token: 0x0400C7AE RID: 51118
				public static LocString NAME = "Potential Load: {TotalPotentialLoadAndColor} / {MaxLoad}";

				// Token: 0x0400C7AF RID: 51119
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"How much wattage this network will draw if all ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" consumers on the network become active at once"
				});
			}

			// Token: 0x020030AF RID: 12463
			public class NOLIQUIDELEMENTTOPUMP
			{
				// Token: 0x0400C7B0 RID: 51120
				public static LocString NAME = "Pump Not In Liquid";

				// Token: 0x0400C7B1 RID: 51121
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This pump must be submerged in ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					" to work"
				});
			}

			// Token: 0x020030B0 RID: 12464
			public class NOGASELEMENTTOPUMP
			{
				// Token: 0x0400C7B2 RID: 51122
				public static LocString NAME = "Pump Not In Gas";

				// Token: 0x0400C7B3 RID: 51123
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This pump must be submerged in ",
					UI.PRE_KEYWORD,
					"Gas",
					UI.PST_KEYWORD,
					" to work"
				});
			}

			// Token: 0x020030B1 RID: 12465
			public class INVALIDMASKSTATIONCONSUMPTIONSTATE
			{
				// Token: 0x0400C7B4 RID: 51124
				public static LocString NAME = "Station Not In Oxygen";

				// Token: 0x0400C7B5 RID: 51125
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This station must be submerged in ",
					UI.PRE_KEYWORD,
					"Oxygen",
					UI.PST_KEYWORD,
					" to work"
				});
			}

			// Token: 0x020030B2 RID: 12466
			public class PIPEMAYMELT
			{
				// Token: 0x0400C7B6 RID: 51126
				public static LocString NAME = "High Melt Risk";

				// Token: 0x0400C7B7 RID: 51127
				public static LocString TOOLTIP = "This pipe is in danger of melting at the current " + UI.PRE_KEYWORD + "Temperature" + UI.PST_KEYWORD;
			}

			// Token: 0x020030B3 RID: 12467
			public class ELEMENTEMITTEROUTPUT
			{
				// Token: 0x0400C7B8 RID: 51128
				public static LocString NAME = "Emitting {ElementTypes}: {FlowRate}";

				// Token: 0x0400C7B9 RID: 51129
				public static LocString TOOLTIP = "This object is releasing {ElementTypes} at a rate of " + UI.FormatAsPositiveRate("{FlowRate}");
			}

			// Token: 0x020030B4 RID: 12468
			public class ELEMENTCONSUMER
			{
				// Token: 0x0400C7BA RID: 51130
				public static LocString NAME = "Consuming {ElementTypes}: {FlowRate}";

				// Token: 0x0400C7BB RID: 51131
				public static LocString TOOLTIP = "This object is utilizing ambient {ElementTypes} from the environment";
			}

			// Token: 0x020030B5 RID: 12469
			public class SPACECRAFTREADYTOLAND
			{
				// Token: 0x0400C7BC RID: 51132
				public static LocString NAME = "Spacecraft ready to land";

				// Token: 0x0400C7BD RID: 51133
				public static LocString TOOLTIP = "A spacecraft is ready to land";

				// Token: 0x0400C7BE RID: 51134
				public static LocString NOTIFICATION = "Space mission complete";

				// Token: 0x0400C7BF RID: 51135
				public static LocString NOTIFICATION_TOOLTIP = "Spacecrafts have completed their missions";
			}

			// Token: 0x020030B6 RID: 12470
			public class CONSUMINGFROMSTORAGE
			{
				// Token: 0x0400C7C0 RID: 51136
				public static LocString NAME = "Consuming {ElementTypes}: {FlowRate}";

				// Token: 0x0400C7C1 RID: 51137
				public static LocString TOOLTIP = "This building is consuming {ElementTypes} from storage";
			}

			// Token: 0x020030B7 RID: 12471
			public class ELEMENTCONVERTEROUTPUT
			{
				// Token: 0x0400C7C2 RID: 51138
				public static LocString NAME = "Emitting {ElementTypes}: {FlowRate}";

				// Token: 0x0400C7C3 RID: 51139
				public static LocString TOOLTIP = "This building is releasing {ElementTypes} at a rate of " + UI.FormatAsPositiveRate("{FlowRate}");
			}

			// Token: 0x020030B8 RID: 12472
			public class ELEMENTCONVERTERINPUT
			{
				// Token: 0x0400C7C4 RID: 51140
				public static LocString NAME = "Using {ElementTypes}: {FlowRate}";

				// Token: 0x0400C7C5 RID: 51141
				public static LocString TOOLTIP = "This building is using {ElementTypes} from storage at a rate of " + UI.FormatAsNegativeRate("{FlowRate}");
			}

			// Token: 0x020030B9 RID: 12473
			public class AWAITINGCOMPOSTFLIP
			{
				// Token: 0x0400C7C6 RID: 51142
				public static LocString NAME = "Requires Flipping";

				// Token: 0x0400C7C7 RID: 51143
				public static LocString TOOLTIP = "Compost must be flipped periodically to produce " + UI.FormatAsLink("Dirt", "DIRT");
			}

			// Token: 0x020030BA RID: 12474
			public class AWAITINGWASTE
			{
				// Token: 0x0400C7C8 RID: 51144
				public static LocString NAME = "Awaiting Compostables";

				// Token: 0x0400C7C9 RID: 51145
				public static LocString TOOLTIP = "More waste material is required to begin the composting process";
			}

			// Token: 0x020030BB RID: 12475
			public class BATTERIESSUFFICIENTLYFULL
			{
				// Token: 0x0400C7CA RID: 51146
				public static LocString NAME = "Batteries Sufficiently Full";

				// Token: 0x0400C7CB RID: 51147
				public static LocString TOOLTIP = "All batteries are above the refill threshold";
			}

			// Token: 0x020030BC RID: 12476
			public class NEEDRESOURCEMASS
			{
				// Token: 0x0400C7CC RID: 51148
				public static LocString NAME = "Insufficient Resources\n{ResourcesRequired}";

				// Token: 0x0400C7CD RID: 51149
				public static LocString TOOLTIP = "The mass of material that was delivered to this building was too low\n\nDeliver more material to run this building";

				// Token: 0x0400C7CE RID: 51150
				public static LocString LINE_ITEM = "• <b>{0}</b>";
			}

			// Token: 0x020030BD RID: 12477
			public class JOULESAVAILABLE
			{
				// Token: 0x0400C7CF RID: 51151
				public static LocString NAME = "Power Available: {JoulesAvailable} / {JoulesCapacity}";

				// Token: 0x0400C7D0 RID: 51152
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"<b>{JoulesAvailable}</b> of stored ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" available for use"
				});
			}

			// Token: 0x020030BE RID: 12478
			public class WATTAGE
			{
				// Token: 0x0400C7D1 RID: 51153
				public static LocString NAME = "Wattage: {Wattage}";

				// Token: 0x0400C7D2 RID: 51154
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building is generating ",
					UI.FormatAsPositiveRate("{Wattage}"),
					" of ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x020030BF RID: 12479
			public class SOLARPANELWATTAGE
			{
				// Token: 0x0400C7D3 RID: 51155
				public static LocString NAME = "Current Wattage: {Wattage}";

				// Token: 0x0400C7D4 RID: 51156
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This panel is generating ",
					UI.FormatAsPositiveRate("{Wattage}"),
					" of ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x020030C0 RID: 12480
			public class MODULESOLARPANELWATTAGE
			{
				// Token: 0x0400C7D5 RID: 51157
				public static LocString NAME = "Current Wattage: {Wattage}";

				// Token: 0x0400C7D6 RID: 51158
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This panel is generating ",
					UI.FormatAsPositiveRate("{Wattage}"),
					" of ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x020030C1 RID: 12481
			public class WATTSON
			{
				// Token: 0x0400C7D7 RID: 51159
				public static LocString NAME = "Next Print: {TimeRemaining}";

				// Token: 0x0400C7D8 RID: 51160
				public static LocString TOOLTIP = "The Printing Pod can print out new Duplicants and useful resources over time.\nThe next print will be ready in <b>{TimeRemaining}</b>";

				// Token: 0x0400C7D9 RID: 51161
				public static LocString UNAVAILABLE = "UNAVAILABLE";
			}

			// Token: 0x020030C2 RID: 12482
			public class FLUSHTOILET
			{
				// Token: 0x0400C7DA RID: 51162
				public static LocString NAME = "{toilet} Ready";

				// Token: 0x0400C7DB RID: 51163
				public static LocString TOOLTIP = "This bathroom is ready to receive visitors";
			}

			// Token: 0x020030C3 RID: 12483
			public class FLUSHTOILETINUSE
			{
				// Token: 0x0400C7DC RID: 51164
				public static LocString NAME = "{toilet} In Use";

				// Token: 0x0400C7DD RID: 51165
				public static LocString TOOLTIP = "This bathroom is occupied";
			}

			// Token: 0x020030C4 RID: 12484
			public class WIRECONNECTED
			{
				// Token: 0x0400C7DE RID: 51166
				public static LocString NAME = "Wire Connected";

				// Token: 0x0400C7DF RID: 51167
				public static LocString TOOLTIP = "This wire is connected to a network";
			}

			// Token: 0x020030C5 RID: 12485
			public class WIRENOMINAL
			{
				// Token: 0x0400C7E0 RID: 51168
				public static LocString NAME = "Wire Nominal";

				// Token: 0x0400C7E1 RID: 51169
				public static LocString TOOLTIP = "This wire is able to handle the wattage it is receiving";
			}

			// Token: 0x020030C6 RID: 12486
			public class WIREDISCONNECTED
			{
				// Token: 0x0400C7E2 RID: 51170
				public static LocString NAME = "Wire Disconnected";

				// Token: 0x0400C7E3 RID: 51171
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This wire is not connecting a ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" consumer to a ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" generator"
				});
			}

			// Token: 0x020030C7 RID: 12487
			public class COOLING
			{
				// Token: 0x0400C7E4 RID: 51172
				public static LocString NAME = "Cooling";

				// Token: 0x0400C7E5 RID: 51173
				public static LocString TOOLTIP = "This building is cooling the surrounding area";
			}

			// Token: 0x020030C8 RID: 12488
			public class COOLINGSTALLEDHOTENV
			{
				// Token: 0x0400C7E6 RID: 51174
				public static LocString NAME = "Gas Too Hot";

				// Token: 0x0400C7E7 RID: 51175
				public static LocString TOOLTIP = "Incoming pipe contents cannot be cooled more than <b>{2}</b> below the surrounding environment\n\nEnvironment: {0}\nCurrent Pipe Contents: {1}";
			}

			// Token: 0x020030C9 RID: 12489
			public class COOLINGSTALLEDCOLDGAS
			{
				// Token: 0x0400C7E8 RID: 51176
				public static LocString NAME = "Gas Too Cold";

				// Token: 0x0400C7E9 RID: 51177
				public static LocString TOOLTIP = "This building cannot cool incoming pipe contents below <b>{0}</b>\n\nCurrent Pipe Contents: {0}";
			}

			// Token: 0x020030CA RID: 12490
			public class COOLINGSTALLEDHOTLIQUID
			{
				// Token: 0x0400C7EA RID: 51178
				public static LocString NAME = "Liquid Too Hot";

				// Token: 0x0400C7EB RID: 51179
				public static LocString TOOLTIP = "Incoming pipe contents cannot be cooled more than <b>{2}</b> below the surrounding environment\n\nEnvironment: {0}\nCurrent Pipe Contents: {1}";
			}

			// Token: 0x020030CB RID: 12491
			public class COOLINGSTALLEDCOLDLIQUID
			{
				// Token: 0x0400C7EC RID: 51180
				public static LocString NAME = "Liquid Too Cold";

				// Token: 0x0400C7ED RID: 51181
				public static LocString TOOLTIP = "This building cannot cool incoming pipe contents below <b>{0}</b>\n\nCurrent Pipe Contents: {0}";
			}

			// Token: 0x020030CC RID: 12492
			public class CANNOTCOOLFURTHER
			{
				// Token: 0x0400C7EE RID: 51182
				public static LocString NAME = "Minimum Temperature Reached";

				// Token: 0x0400C7EF RID: 51183
				public static LocString TOOLTIP = "This building cannot cool the surrounding environment below <b>{0}</b>";
			}

			// Token: 0x020030CD RID: 12493
			public class HEATINGSTALLEDHOTENV
			{
				// Token: 0x0400C7F0 RID: 51184
				public static LocString NAME = "Target Temperature Reached";

				// Token: 0x0400C7F1 RID: 51185
				public static LocString TOOLTIP = "This building cannot heat the surrounding environment beyond <b>{0}</b>";
			}

			// Token: 0x020030CE RID: 12494
			public class HEATINGSTALLEDLOWMASS_GAS
			{
				// Token: 0x0400C7F2 RID: 51186
				public static LocString NAME = "Insufficient Atmosphere";

				// Token: 0x0400C7F3 RID: 51187
				public static LocString TOOLTIP = "This building cannot operate in a vacuum";
			}

			// Token: 0x020030CF RID: 12495
			public class HEATINGSTALLEDLOWMASS_LIQUID
			{
				// Token: 0x0400C7F4 RID: 51188
				public static LocString NAME = "Not Submerged In Liquid";

				// Token: 0x0400C7F5 RID: 51189
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building must be submerged in ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					" to function"
				});
			}

			// Token: 0x020030D0 RID: 12496
			public class BUILDINGDISABLED
			{
				// Token: 0x0400C7F6 RID: 51190
				public static LocString NAME = "Building Disabled";

				// Token: 0x0400C7F7 RID: 51191
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Press ",
					UI.PRE_KEYWORD,
					"Enable Building",
					UI.PST_KEYWORD,
					" ",
					UI.FormatAsHotKey(global::Action.ToggleEnabled),
					" to resume use"
				});
			}

			// Token: 0x020030D1 RID: 12497
			public class MISSINGREQUIREMENTS
			{
				// Token: 0x0400C7F8 RID: 51192
				public static LocString NAME = "Missing Requirements";

				// Token: 0x0400C7F9 RID: 51193
				public static LocString TOOLTIP = "There are some problems that need to be fixed before this building is operational";
			}

			// Token: 0x020030D2 RID: 12498
			public class GETTINGREADY
			{
				// Token: 0x0400C7FA RID: 51194
				public static LocString NAME = "Getting Ready";

				// Token: 0x0400C7FB RID: 51195
				public static LocString TOOLTIP = "This building will soon be ready to use";
			}

			// Token: 0x020030D3 RID: 12499
			public class WORKING
			{
				// Token: 0x0400C7FC RID: 51196
				public static LocString NAME = "Nominal";

				// Token: 0x0400C7FD RID: 51197
				public static LocString TOOLTIP = "This building is working as intended";
			}

			// Token: 0x020030D4 RID: 12500
			public class GRAVEEMPTY
			{
				// Token: 0x0400C7FE RID: 51198
				public static LocString NAME = "Empty";

				// Token: 0x0400C7FF RID: 51199
				public static LocString TOOLTIP = "This memorial honors no one.";
			}

			// Token: 0x020030D5 RID: 12501
			public class GRAVE
			{
				// Token: 0x0400C800 RID: 51200
				public static LocString NAME = "RIP {DeadDupe}";

				// Token: 0x0400C801 RID: 51201
				public static LocString TOOLTIP = "{Epitaph}";
			}

			// Token: 0x020030D6 RID: 12502
			public class AWAITINGARTING
			{
				// Token: 0x0400C802 RID: 51202
				public static LocString NAME = "Incomplete Artwork";

				// Token: 0x0400C803 RID: 51203
				public static LocString TOOLTIP = "This building requires a Duplicant's artistic touch";
			}

			// Token: 0x020030D7 RID: 12503
			public class LOOKINGUGLY
			{
				// Token: 0x0400C804 RID: 51204
				public static LocString NAME = "Crude";

				// Token: 0x0400C805 RID: 51205
				public static LocString TOOLTIP = "Honestly, Morbs could've done better";
			}

			// Token: 0x020030D8 RID: 12504
			public class LOOKINGOKAY
			{
				// Token: 0x0400C806 RID: 51206
				public static LocString NAME = "Quaint";

				// Token: 0x0400C807 RID: 51207
				public static LocString TOOLTIP = "Duplicants find this art piece quite charming";
			}

			// Token: 0x020030D9 RID: 12505
			public class LOOKINGGREAT
			{
				// Token: 0x0400C808 RID: 51208
				public static LocString NAME = "Masterpiece";

				// Token: 0x0400C809 RID: 51209
				public static LocString TOOLTIP = "This poignant piece stirs something deep within each Duplicant's soul";
			}

			// Token: 0x020030DA RID: 12506
			public class EXPIRED
			{
				// Token: 0x0400C80A RID: 51210
				public static LocString NAME = "Depleted";

				// Token: 0x0400C80B RID: 51211
				public static LocString TOOLTIP = "This building has no more use";
			}

			// Token: 0x020030DB RID: 12507
			public class COOLINGWATER
			{
				// Token: 0x0400C80C RID: 51212
				public static LocString NAME = "Cooling Water";

				// Token: 0x0400C80D RID: 51213
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building is cooling ",
					UI.PRE_KEYWORD,
					"Water",
					UI.PST_KEYWORD,
					" down to its freezing point"
				});
			}

			// Token: 0x020030DC RID: 12508
			public class EXCAVATOR_BOMB
			{
				// Token: 0x020030DD RID: 12509
				public class UNARMED
				{
					// Token: 0x0400C80E RID: 51214
					public static LocString NAME = "Unarmed";

					// Token: 0x0400C80F RID: 51215
					public static LocString TOOLTIP = "This explosive is currently inactive";
				}

				// Token: 0x020030DE RID: 12510
				public class ARMED
				{
					// Token: 0x0400C810 RID: 51216
					public static LocString NAME = "Armed";

					// Token: 0x0400C811 RID: 51217
					public static LocString TOOLTIP = "Stand back, this baby's ready to blow!";
				}

				// Token: 0x020030DF RID: 12511
				public class COUNTDOWN
				{
					// Token: 0x0400C812 RID: 51218
					public static LocString NAME = "Countdown: {0}";

					// Token: 0x0400C813 RID: 51219
					public static LocString TOOLTIP = "<b>{0}</b> seconds until detonation";
				}

				// Token: 0x020030E0 RID: 12512
				public class DUPE_DANGER
				{
					// Token: 0x0400C814 RID: 51220
					public static LocString NAME = "Duplicant Preservation Override";

					// Token: 0x0400C815 RID: 51221
					public static LocString TOOLTIP = "Explosive disabled due to close Duplicant proximity";
				}

				// Token: 0x020030E1 RID: 12513
				public class EXPLODING
				{
					// Token: 0x0400C816 RID: 51222
					public static LocString NAME = "Exploding";

					// Token: 0x0400C817 RID: 51223
					public static LocString TOOLTIP = "Kaboom!";
				}
			}

			// Token: 0x020030E2 RID: 12514
			public class BURNER
			{
				// Token: 0x020030E3 RID: 12515
				public class BURNING_FUEL
				{
					// Token: 0x0400C818 RID: 51224
					public static LocString NAME = "Consuming Fuel: {0}";

					// Token: 0x0400C819 RID: 51225
					public static LocString TOOLTIP = "<b>{0}</b> fuel remaining";
				}

				// Token: 0x020030E4 RID: 12516
				public class HAS_FUEL
				{
					// Token: 0x0400C81A RID: 51226
					public static LocString NAME = "Fueled: {0}";

					// Token: 0x0400C81B RID: 51227
					public static LocString TOOLTIP = "<b>{0}</b> fuel remaining";
				}
			}

			// Token: 0x020030E5 RID: 12517
			public class CREATURE_REUSABLE_TRAP
			{
				// Token: 0x020030E6 RID: 12518
				public class NEEDS_ARMING
				{
					// Token: 0x0400C81C RID: 51228
					public static LocString NAME = "Waiting to be Armed";

					// Token: 0x0400C81D RID: 51229
					public static LocString TOOLTIP = "Waiting for a Duplicant to arm this trap\n\nOnly Duplicants with the " + DUPLICANTS.ROLES.RANCHER.NAME + " skill can arm traps";
				}

				// Token: 0x020030E7 RID: 12519
				public class READY
				{
					// Token: 0x0400C81E RID: 51230
					public static LocString NAME = "Armed";

					// Token: 0x0400C81F RID: 51231
					public static LocString TOOLTIP = "This trap has been armed and is ready to catch a " + UI.PRE_KEYWORD + "Critter" + UI.PST_KEYWORD;
				}

				// Token: 0x020030E8 RID: 12520
				public class SPRUNG
				{
					// Token: 0x0400C820 RID: 51232
					public static LocString NAME = "Sprung";

					// Token: 0x0400C821 RID: 51233
					public static LocString TOOLTIP = "This trap has caught a {0}!";
				}
			}

			// Token: 0x020030E9 RID: 12521
			public class CREATURE_TRAP
			{
				// Token: 0x020030EA RID: 12522
				public class NEEDSBAIT
				{
					// Token: 0x0400C822 RID: 51234
					public static LocString NAME = "Needs Bait";

					// Token: 0x0400C823 RID: 51235
					public static LocString TOOLTIP = "This trap needs to be baited before it can be set";
				}

				// Token: 0x020030EB RID: 12523
				public class READY
				{
					// Token: 0x0400C824 RID: 51236
					public static LocString NAME = "Set";

					// Token: 0x0400C825 RID: 51237
					public static LocString TOOLTIP = "This trap has been set and is ready to catch a " + UI.PRE_KEYWORD + "Critter" + UI.PST_KEYWORD;
				}

				// Token: 0x020030EC RID: 12524
				public class SPRUNG
				{
					// Token: 0x0400C826 RID: 51238
					public static LocString NAME = "Sprung";

					// Token: 0x0400C827 RID: 51239
					public static LocString TOOLTIP = "This trap has caught a {0}!";
				}
			}

			// Token: 0x020030ED RID: 12525
			public class ACCESS_CONTROL
			{
				// Token: 0x020030EE RID: 12526
				public class ACTIVE
				{
					// Token: 0x0400C828 RID: 51240
					public static LocString NAME = "Access Restrictions";

					// Token: 0x0400C829 RID: 51241
					public static LocString TOOLTIP = "Some Duplicants are prohibited from passing through this door by the current " + UI.PRE_KEYWORD + "Access Permissions" + UI.PST_KEYWORD;
				}

				// Token: 0x020030EF RID: 12527
				public class OFFLINE
				{
					// Token: 0x0400C82A RID: 51242
					public static LocString NAME = "Access Control Offline";

					// Token: 0x0400C82B RID: 51243
					public static LocString TOOLTIP = string.Concat(new string[]
					{
						"This door has granted Emergency ",
						UI.PRE_KEYWORD,
						"Access Permissions",
						UI.PST_KEYWORD,
						"\n\nAll Duplicants are permitted to pass through it until ",
						UI.PRE_KEYWORD,
						"Power",
						UI.PST_KEYWORD,
						" is restored"
					});
				}
			}

			// Token: 0x020030F0 RID: 12528
			public class REQUIRESSKILLPERK
			{
				// Token: 0x0400C82C RID: 51244
				public static LocString NAME = "Skill-Required Operation";

				// Token: 0x0400C82D RID: 51245
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Only Duplicants with one of the following ",
					UI.PRE_KEYWORD,
					"Skills",
					UI.PST_KEYWORD,
					" can operate this building:\n{Skills}"
				});
			}

			// Token: 0x020030F1 RID: 12529
			public class DIGREQUIRESSKILLPERK
			{
				// Token: 0x0400C82E RID: 51246
				public static LocString NAME = "Skill-Required Dig";

				// Token: 0x0400C82F RID: 51247
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Only Duplicants with one of the following ",
					UI.PRE_KEYWORD,
					"Skills",
					UI.PST_KEYWORD,
					" can mine this material:\n{Skills}"
				});
			}

			// Token: 0x020030F2 RID: 12530
			public class COLONYLACKSREQUIREDSKILLPERK
			{
				// Token: 0x0400C830 RID: 51248
				public static LocString NAME = "Colony Lacks {Skills} Skill";

				// Token: 0x0400C831 RID: 51249
				public static LocString TOOLTIP = "{Skills} Skill required to operate\n\nOpen the " + UI.FormatAsManagementMenu("Skills Panel", global::Action.ManageSkills) + " to teach {Skills} to a Duplicant";
			}

			// Token: 0x020030F3 RID: 12531
			public class CLUSTERCOLONYLACKSREQUIREDSKILLPERK
			{
				// Token: 0x0400C832 RID: 51250
				public static LocString NAME = "Local Colony Lacks {Skills} Skill";

				// Token: 0x0400C833 RID: 51251
				public static LocString TOOLTIP = BUILDING.STATUSITEMS.COLONYLACKSREQUIREDSKILLPERK.TOOLTIP + ", or bring a Duplicant with the skill from another " + UI.CLUSTERMAP.PLANETOID;
			}

			// Token: 0x020030F4 RID: 12532
			public class WORKREQUIRESMINION
			{
				// Token: 0x0400C834 RID: 51252
				public static LocString NAME = "Duplicant Operation Required";

				// Token: 0x0400C835 RID: 51253
				public static LocString TOOLTIP = "A Duplicant must be present to complete this operation";
			}

			// Token: 0x020030F5 RID: 12533
			public class SWITCHSTATUSACTIVE
			{
				// Token: 0x0400C836 RID: 51254
				public static LocString NAME = "Active";

				// Token: 0x0400C837 RID: 51255
				public static LocString TOOLTIP = "This switch is currently toggled <b>On</b>";
			}

			// Token: 0x020030F6 RID: 12534
			public class SWITCHSTATUSINACTIVE
			{
				// Token: 0x0400C838 RID: 51256
				public static LocString NAME = "Inactive";

				// Token: 0x0400C839 RID: 51257
				public static LocString TOOLTIP = "This switch is currently toggled <b>Off</b>";
			}

			// Token: 0x020030F7 RID: 12535
			public class LOGICSWITCHSTATUSACTIVE
			{
				// Token: 0x0400C83A RID: 51258
				public static LocString NAME = "Sending a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active);

				// Token: 0x0400C83B RID: 51259
				public static LocString TOOLTIP = "This switch is currently sending a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active);
			}

			// Token: 0x020030F8 RID: 12536
			public class LOGICSWITCHSTATUSINACTIVE
			{
				// Token: 0x0400C83C RID: 51260
				public static LocString NAME = "Sending a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);

				// Token: 0x0400C83D RID: 51261
				public static LocString TOOLTIP = "This switch is currently sending a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x020030F9 RID: 12537
			public class LOGICSENSORSTATUSACTIVE
			{
				// Token: 0x0400C83E RID: 51262
				public static LocString NAME = "Sending a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active);

				// Token: 0x0400C83F RID: 51263
				public static LocString TOOLTIP = "This sensor is currently sending a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active);
			}

			// Token: 0x020030FA RID: 12538
			public class LOGICSENSORSTATUSINACTIVE
			{
				// Token: 0x0400C840 RID: 51264
				public static LocString NAME = "Sending a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);

				// Token: 0x0400C841 RID: 51265
				public static LocString TOOLTIP = "This sensor is currently sending " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x020030FB RID: 12539
			public class PLAYERCONTROLLEDTOGGLESIDESCREEN
			{
				// Token: 0x0400C842 RID: 51266
				public static LocString NAME = "Pending Toggle on Unpause";

				// Token: 0x0400C843 RID: 51267
				public static LocString TOOLTIP = "This will be toggled when time is unpaused";
			}

			// Token: 0x020030FC RID: 12540
			public class FOOD_CONTAINERS_OUTSIDE_RANGE
			{
				// Token: 0x0400C844 RID: 51268
				public static LocString NAME = "Unreachable food";

				// Token: 0x0400C845 RID: 51269
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Recuperating Duplicants must have ",
					UI.PRE_KEYWORD,
					"Food",
					UI.PST_KEYWORD,
					" available within <b>{0}</b> cells"
				});
			}

			// Token: 0x020030FD RID: 12541
			public class TOILETS_OUTSIDE_RANGE
			{
				// Token: 0x0400C846 RID: 51270
				public static LocString NAME = "Unreachable restroom";

				// Token: 0x0400C847 RID: 51271
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Recuperating Duplicants must have ",
					UI.PRE_KEYWORD,
					"Toilets",
					UI.PST_KEYWORD,
					" available within <b>{0}</b> cells"
				});
			}

			// Token: 0x020030FE RID: 12542
			public class BUILDING_DEPRECATED
			{
				// Token: 0x0400C848 RID: 51272
				public static LocString NAME = "Building Deprecated";

				// Token: 0x0400C849 RID: 51273
				public static LocString TOOLTIP = "This building is from an older version of the game and its use is not intended";
			}

			// Token: 0x020030FF RID: 12543
			public class TURBINE_BLOCKED_INPUT
			{
				// Token: 0x0400C84A RID: 51274
				public static LocString NAME = "All Inputs Blocked";

				// Token: 0x0400C84B RID: 51275
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This turbine's ",
					UI.PRE_KEYWORD,
					"Input Vents",
					UI.PST_KEYWORD,
					" are blocked, so it can't intake any ",
					ELEMENTS.STEAM.NAME,
					".\n\nThe ",
					UI.PRE_KEYWORD,
					"Input Vents",
					UI.PST_KEYWORD,
					" are located directly below the foundation ",
					UI.PRE_KEYWORD,
					"Tile",
					UI.PST_KEYWORD,
					" this building is resting on."
				});
			}

			// Token: 0x02003100 RID: 12544
			public class TURBINE_PARTIALLY_BLOCKED_INPUT
			{
				// Token: 0x0400C84C RID: 51276
				public static LocString NAME = "{Blocked}/{Total} Inputs Blocked";

				// Token: 0x0400C84D RID: 51277
				public static LocString TOOLTIP = "<b>{Blocked}</b> of this turbine's <b>{Total}</b> inputs have been blocked, resulting in reduced throughput";
			}

			// Token: 0x02003101 RID: 12545
			public class TURBINE_TOO_HOT
			{
				// Token: 0x0400C84E RID: 51278
				public static LocString NAME = "Turbine Too Hot";

				// Token: 0x0400C84F RID: 51279
				public static LocString TOOLTIP = "This turbine must be below <b>{Overheat_Temperature}</b> to properly process {Src_Element} into {Dest_Element}";
			}

			// Token: 0x02003102 RID: 12546
			public class TURBINE_BLOCKED_OUTPUT
			{
				// Token: 0x0400C850 RID: 51280
				public static LocString NAME = "Output Blocked";

				// Token: 0x0400C851 RID: 51281
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"A blocked ",
					UI.PRE_KEYWORD,
					"Output",
					UI.PST_KEYWORD,
					" has stopped this turbine from functioning"
				});
			}

			// Token: 0x02003103 RID: 12547
			public class TURBINE_INSUFFICIENT_MASS
			{
				// Token: 0x0400C852 RID: 51282
				public static LocString NAME = "Not Enough {Src_Element}";

				// Token: 0x0400C853 RID: 51283
				public static LocString TOOLTIP = "The {Src_Element} present below this turbine must be at least <b>{Min_Mass}</b> in order to turn the turbine";
			}

			// Token: 0x02003104 RID: 12548
			public class TURBINE_INSUFFICIENT_TEMPERATURE
			{
				// Token: 0x0400C854 RID: 51284
				public static LocString NAME = "{Src_Element} Temperature Below {Active_Temperature}";

				// Token: 0x0400C855 RID: 51285
				public static LocString TOOLTIP = "This turbine requires {Src_Element} that is a minimum of <b>{Active_Temperature}</b> in order to produce power";
			}

			// Token: 0x02003105 RID: 12549
			public class TURBINE_ACTIVE_WATTAGE
			{
				// Token: 0x0400C856 RID: 51286
				public static LocString NAME = "Current Wattage: {Wattage}";

				// Token: 0x0400C857 RID: 51287
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This turbine is generating ",
					UI.FormatAsPositiveRate("{Wattage}"),
					" of ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					"\n\nIt is running at <b>{Efficiency}</b> of full capacity\n\nIncrease {Src_Element} ",
					UI.PRE_KEYWORD,
					"Mass",
					UI.PST_KEYWORD,
					" and ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" to improve output"
				});
			}

			// Token: 0x02003106 RID: 12550
			public class TURBINE_SPINNING_UP
			{
				// Token: 0x0400C858 RID: 51288
				public static LocString NAME = "Spinning Up";

				// Token: 0x0400C859 RID: 51289
				public static LocString TOOLTIP = "This turbine is currently spinning up\n\nSpinning up allows a turbine to continue running for a short period if the pressure it needs to run becomes unavailable";
			}

			// Token: 0x02003107 RID: 12551
			public class TURBINE_ACTIVE
			{
				// Token: 0x0400C85A RID: 51290
				public static LocString NAME = "Active";

				// Token: 0x0400C85B RID: 51291
				public static LocString TOOLTIP = "This turbine is running at <b>{0}RPM</b>";
			}

			// Token: 0x02003108 RID: 12552
			public class WELL_PRESSURIZING
			{
				// Token: 0x0400C85C RID: 51292
				public static LocString NAME = "Backpressure: {0}";

				// Token: 0x0400C85D RID: 51293
				public static LocString TOOLTIP = "Well pressure increases with each use and must be periodically relieved to prevent shutdown";
			}

			// Token: 0x02003109 RID: 12553
			public class WELL_OVERPRESSURE
			{
				// Token: 0x0400C85E RID: 51294
				public static LocString NAME = "Overpressure";

				// Token: 0x0400C85F RID: 51295
				public static LocString TOOLTIP = "This well can no longer function due to excessive backpressure";
			}

			// Token: 0x0200310A RID: 12554
			public class NOTINANYROOM
			{
				// Token: 0x0400C860 RID: 51296
				public static LocString NAME = "Outside of room";

				// Token: 0x0400C861 RID: 51297
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building must be built inside a ",
					UI.PRE_KEYWORD,
					"Room",
					UI.PST_KEYWORD,
					" for full functionality\n\nOpen the ",
					UI.FormatAsOverlay("Room Overlay", global::Action.Overlay11),
					" to view full ",
					UI.PRE_KEYWORD,
					"Room",
					UI.PST_KEYWORD,
					" status"
				});
			}

			// Token: 0x0200310B RID: 12555
			public class NOTINREQUIREDROOM
			{
				// Token: 0x0400C862 RID: 51298
				public static LocString NAME = "Outside of {0}";

				// Token: 0x0400C863 RID: 51299
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building must be built inside a {0} for full functionality\n\nOpen the ",
					UI.FormatAsOverlay("Room Overlay", global::Action.Overlay11),
					" to view full ",
					UI.PRE_KEYWORD,
					"Room",
					UI.PST_KEYWORD,
					" status"
				});
			}

			// Token: 0x0200310C RID: 12556
			public class NOTINRECOMMENDEDROOM
			{
				// Token: 0x0400C864 RID: 51300
				public static LocString NAME = "Outside of {0}";

				// Token: 0x0400C865 RID: 51301
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"It is recommended to build this building inside a {0}\n\nOpen the ",
					UI.FormatAsOverlay("Room Overlay", global::Action.Overlay11),
					" to view full ",
					UI.PRE_KEYWORD,
					"Room",
					UI.PST_KEYWORD,
					" status"
				});
			}

			// Token: 0x0200310D RID: 12557
			public class RELEASING_PRESSURE
			{
				// Token: 0x0400C866 RID: 51302
				public static LocString NAME = "Releasing Pressure";

				// Token: 0x0400C867 RID: 51303
				public static LocString TOOLTIP = "Pressure buildup is being safely released";
			}

			// Token: 0x0200310E RID: 12558
			public class LOGIC_FEEDBACK_LOOP
			{
				// Token: 0x0400C868 RID: 51304
				public static LocString NAME = "Feedback Loop";

				// Token: 0x0400C869 RID: 51305
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Feedback loops prevent automation grids from functioning\n\nFeedback loops occur when the ",
					UI.PRE_KEYWORD,
					"Output",
					UI.PST_KEYWORD,
					" of an automated building connects back to its own ",
					UI.PRE_KEYWORD,
					"Input",
					UI.PST_KEYWORD,
					" through the Automation grid"
				});
			}

			// Token: 0x0200310F RID: 12559
			public class ENOUGH_COOLANT
			{
				// Token: 0x0400C86A RID: 51306
				public static LocString NAME = "Awaiting Coolant";

				// Token: 0x0400C86B RID: 51307
				public static LocString TOOLTIP = "<b>{1}</b> of {0} must be present in storage to begin production";
			}

			// Token: 0x02003110 RID: 12560
			public class ENOUGH_FUEL
			{
				// Token: 0x0400C86C RID: 51308
				public static LocString NAME = "Awaiting Fuel";

				// Token: 0x0400C86D RID: 51309
				public static LocString TOOLTIP = "<b>{1}</b> of {0} must be present in storage to begin production";
			}

			// Token: 0x02003111 RID: 12561
			public class LOGIC
			{
				// Token: 0x0400C86E RID: 51310
				public static LocString LOGIC_CONTROLLED_ENABLED = "Enabled by Automation Grid";

				// Token: 0x0400C86F RID: 51311
				public static LocString LOGIC_CONTROLLED_DISABLED = "Disabled by Automation Grid";
			}

			// Token: 0x02003112 RID: 12562
			public class GANTRY
			{
				// Token: 0x0400C870 RID: 51312
				public static LocString AUTOMATION_CONTROL = "Automation Control: {0}";

				// Token: 0x0400C871 RID: 51313
				public static LocString MANUAL_CONTROL = "Manual Control: {0}";

				// Token: 0x0400C872 RID: 51314
				public static LocString EXTENDED = "Extended";

				// Token: 0x0400C873 RID: 51315
				public static LocString RETRACTED = "Retracted";
			}

			// Token: 0x02003113 RID: 12563
			public class OBJECTDISPENSER
			{
				// Token: 0x0400C874 RID: 51316
				public static LocString AUTOMATION_CONTROL = "Automation Control: {0}";

				// Token: 0x0400C875 RID: 51317
				public static LocString MANUAL_CONTROL = "Manual Control: {0}";

				// Token: 0x0400C876 RID: 51318
				public static LocString OPENED = "Opened";

				// Token: 0x0400C877 RID: 51319
				public static LocString CLOSED = "Closed";
			}

			// Token: 0x02003114 RID: 12564
			public class TOO_COLD
			{
				// Token: 0x0400C878 RID: 51320
				public static LocString NAME = "Too Cold";

				// Token: 0x0400C879 RID: 51321
				public static LocString TOOLTIP = "Either this building or its surrounding environment is too cold to operate";
			}

			// Token: 0x02003115 RID: 12565
			public class CHECKPOINT
			{
				// Token: 0x0400C87A RID: 51322
				public static LocString LOGIC_CONTROLLED_OPEN = "Clearance: Permitted";

				// Token: 0x0400C87B RID: 51323
				public static LocString LOGIC_CONTROLLED_CLOSED = "Clearance: Not Permitted";

				// Token: 0x0400C87C RID: 51324
				public static LocString LOGIC_CONTROLLED_DISCONNECTED = "No Automation";

				// Token: 0x02003116 RID: 12566
				public class TOOLTIPS
				{
					// Token: 0x0400C87D RID: 51325
					public static LocString LOGIC_CONTROLLED_OPEN = "Automated Checkpoint is receiving a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ", preventing Duplicants from passing";

					// Token: 0x0400C87E RID: 51326
					public static LocString LOGIC_CONTROLLED_CLOSED = "Automated Checkpoint is receiving a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ", allowing Duplicants to pass";

					// Token: 0x0400C87F RID: 51327
					public static LocString LOGIC_CONTROLLED_DISCONNECTED = string.Concat(new string[]
					{
						"This Checkpoint has not been connected to an ",
						UI.PRE_KEYWORD,
						"Automation",
						UI.PST_KEYWORD,
						" grid"
					});
				}
			}

			// Token: 0x02003117 RID: 12567
			public class HIGHENERGYPARTICLEREDIRECTOR
			{
				// Token: 0x0400C880 RID: 51328
				public static LocString LOGIC_CONTROLLED_STANDBY = "Incoming Radbolts: Ignore";

				// Token: 0x0400C881 RID: 51329
				public static LocString LOGIC_CONTROLLED_ACTIVE = "Incoming Radbolts: Redirect";

				// Token: 0x0400C882 RID: 51330
				public static LocString NORMAL = "Normal";

				// Token: 0x02003118 RID: 12568
				public class TOOLTIPS
				{
					// Token: 0x0400C883 RID: 51331
					public static LocString LOGIC_CONTROLLED_STANDBY = string.Concat(new string[]
					{
						UI.FormatAsKeyWord("Radbolt Reflector"),
						" is receiving a ",
						UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
						", ignoring incoming ",
						UI.PRE_KEYWORD,
						"Radbolts",
						UI.PST_KEYWORD
					});

					// Token: 0x0400C884 RID: 51332
					public static LocString LOGIC_CONTROLLED_ACTIVE = string.Concat(new string[]
					{
						UI.FormatAsKeyWord("Radbolt Reflector"),
						" is receiving a ",
						UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
						", accepting incoming ",
						UI.PRE_KEYWORD,
						"Radbolts",
						UI.PST_KEYWORD
					});

					// Token: 0x0400C885 RID: 51333
					public static LocString NORMAL = "Incoming Radbolts will be accepted and redirected";
				}
			}

			// Token: 0x02003119 RID: 12569
			public class HIGHENERGYPARTICLESPAWNER
			{
				// Token: 0x0400C886 RID: 51334
				public static LocString LOGIC_CONTROLLED_STANDBY = "Launch Radbolt: Off";

				// Token: 0x0400C887 RID: 51335
				public static LocString LOGIC_CONTROLLED_ACTIVE = "Launch Radbolt: On";

				// Token: 0x0400C888 RID: 51336
				public static LocString NORMAL = "Normal";

				// Token: 0x0200311A RID: 12570
				public class TOOLTIPS
				{
					// Token: 0x0400C889 RID: 51337
					public static LocString LOGIC_CONTROLLED_STANDBY = string.Concat(new string[]
					{
						UI.FormatAsKeyWord("Radbolt Generator"),
						" is receiving a ",
						UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
						", ignoring incoming ",
						UI.PRE_KEYWORD,
						"Radbolts",
						UI.PST_KEYWORD
					});

					// Token: 0x0400C88A RID: 51338
					public static LocString LOGIC_CONTROLLED_ACTIVE = string.Concat(new string[]
					{
						UI.FormatAsKeyWord("Radbolt Generator"),
						" is receiving a ",
						UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
						", accepting incoming ",
						UI.PRE_KEYWORD,
						"Radbolts",
						UI.PST_KEYWORD
					});

					// Token: 0x0400C88B RID: 51339
					public static LocString NORMAL = string.Concat(new string[]
					{
						"Incoming ",
						UI.PRE_KEYWORD,
						"Radbolts",
						UI.PST_KEYWORD,
						" will be accepted and redirected"
					});
				}
			}

			// Token: 0x0200311B RID: 12571
			public class AWAITINGFUEL
			{
				// Token: 0x0400C88C RID: 51340
				public static LocString NAME = "Awaiting Fuel: {0}";

				// Token: 0x0400C88D RID: 51341
				public static LocString TOOLTIP = "This building requires <b>{1}</b> of {0} to operate";
			}

			// Token: 0x0200311C RID: 12572
			public class FOSSILHUNT
			{
				// Token: 0x0200311D RID: 12573
				public class PENDING_EXCAVATION
				{
					// Token: 0x0400C88E RID: 51342
					public static LocString NAME = "Awaiting Excavation";

					// Token: 0x0400C88F RID: 51343
					public static LocString TOOLTIP = "Currently awaiting excavation by a Duplicant";
				}

				// Token: 0x0200311E RID: 12574
				public class EXCAVATING
				{
					// Token: 0x0400C890 RID: 51344
					public static LocString NAME = "Excavation In Progress";

					// Token: 0x0400C891 RID: 51345
					public static LocString TOOLTIP = "Currently being excavated by a Duplicant";
				}
			}

			// Token: 0x0200311F RID: 12575
			public class MEGABRAINTANK
			{
				// Token: 0x02003120 RID: 12576
				public class PROGRESS
				{
					// Token: 0x02003121 RID: 12577
					public class PROGRESSIONRATE
					{
						// Token: 0x0400C892 RID: 51346
						public static LocString NAME = "Dream Journals: {ActivationProgress}";

						// Token: 0x0400C893 RID: 51347
						public static LocString TOOLTIP = "Currently awaiting the Dream Journals necessary to restore this building to full functionality";
					}

					// Token: 0x02003122 RID: 12578
					public class DREAMANALYSIS
					{
						// Token: 0x0400C894 RID: 51348
						public static LocString NAME = "Analyzing Dreams: {TimeToComplete}s";

						// Token: 0x0400C895 RID: 51349
						public static LocString TOOLTIP = "Maximum Aptitude effect sustained while dream analysis continues";
					}
				}

				// Token: 0x02003123 RID: 12579
				public class COMPLETE
				{
					// Token: 0x0400C896 RID: 51350
					public static LocString NAME = "Fully Restored";

					// Token: 0x0400C897 RID: 51351
					public static LocString TOOLTIP = "This building is functioning at full capacity";
				}
			}

			// Token: 0x02003124 RID: 12580
			public class MEGABRAINNOTENOUGHOXYGEN
			{
				// Token: 0x0400C898 RID: 51352
				public static LocString NAME = "Lacks Oxygen";

				// Token: 0x0400C899 RID: 51353
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building needs ",
					UI.PRE_KEYWORD,
					"Oxygen",
					UI.PST_KEYWORD,
					" in order to function"
				});
			}

			// Token: 0x02003125 RID: 12581
			public class NOLOGICWIRECONNECTED
			{
				// Token: 0x0400C89A RID: 51354
				public static LocString NAME = "No Automation Wire Connected";

				// Token: 0x0400C89B RID: 51355
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building has not been connected to an ",
					UI.PRE_KEYWORD,
					"Automation",
					UI.PST_KEYWORD,
					" grid"
				});
			}

			// Token: 0x02003126 RID: 12582
			public class NOTUBECONNECTED
			{
				// Token: 0x0400C89C RID: 51356
				public static LocString NAME = "No Tube Connected";

				// Token: 0x0400C89D RID: 51357
				public static LocString TOOLTIP = "The first section of tube extending from a " + BUILDINGS.PREFABS.TRAVELTUBEENTRANCE.NAME + " must connect directly upward";
			}

			// Token: 0x02003127 RID: 12583
			public class NOTUBEEXITS
			{
				// Token: 0x0400C89E RID: 51358
				public static LocString NAME = "No Landing Available";

				// Token: 0x0400C89F RID: 51359
				public static LocString TOOLTIP = "Duplicants can only exit a tube when there is somewhere for them to land within <b>two tiles</b>";
			}

			// Token: 0x02003128 RID: 12584
			public class STOREDCHARGE
			{
				// Token: 0x0400C8A0 RID: 51360
				public static LocString NAME = "Charge Available: {0}/{1}";

				// Token: 0x0400C8A1 RID: 51361
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building has <b>{0}</b> of stored ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					"\n\nIt consumes ",
					UI.FormatAsNegativeRate("{2}"),
					" per use"
				});
			}

			// Token: 0x02003129 RID: 12585
			public class NEEDEGG
			{
				// Token: 0x0400C8A2 RID: 51362
				public static LocString NAME = "No Egg Selected";

				// Token: 0x0400C8A3 RID: 51363
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Collect ",
					UI.PRE_KEYWORD,
					"Eggs",
					UI.PST_KEYWORD,
					" from ",
					UI.FormatAsLink("Critters", "CREATURES"),
					" to incubate"
				});
			}

			// Token: 0x0200312A RID: 12586
			public class NOAVAILABLEEGG
			{
				// Token: 0x0400C8A4 RID: 51364
				public static LocString NAME = "No Egg Available";

				// Token: 0x0400C8A5 RID: 51365
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"The selected ",
					UI.PRE_KEYWORD,
					"Egg",
					UI.PST_KEYWORD,
					" is not currently available"
				});
			}

			// Token: 0x0200312B RID: 12587
			public class AWAITINGEGGDELIVERY
			{
				// Token: 0x0400C8A6 RID: 51366
				public static LocString NAME = "Awaiting Delivery";

				// Token: 0x0400C8A7 RID: 51367
				public static LocString TOOLTIP = "Awaiting delivery of selected " + UI.PRE_KEYWORD + "Egg" + UI.PST_KEYWORD;
			}

			// Token: 0x0200312C RID: 12588
			public class INCUBATORPROGRESS
			{
				// Token: 0x0400C8A8 RID: 51368
				public static LocString NAME = "Incubating: {Percent}";

				// Token: 0x0400C8A9 RID: 51369
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This ",
					UI.PRE_KEYWORD,
					"Egg",
					UI.PST_KEYWORD,
					" incubating cozily\n\nIt will hatch when ",
					UI.PRE_KEYWORD,
					"Incubation",
					UI.PST_KEYWORD,
					" reaches <b>100%</b>"
				});
			}

			// Token: 0x0200312D RID: 12589
			public class NETWORKQUALITY
			{
				// Token: 0x0400C8AA RID: 51370
				public static LocString NAME = "Scan Network Quality: {TotalQuality}";

				// Token: 0x0400C8AB RID: 51371
				public static LocString TOOLTIP = "This scanner network is scanning at <b>{TotalQuality}</b> effectiveness\n\nIt will detect incoming objects <b>{WorstTime}</b> to <b>{BestTime}</b> before they arrive\n\nBuild multiple " + BUILDINGS.PREFABS.COMETDETECTOR.NAME + "s to increase surface coverage and improve network quality\n\n    • Surface Coverage: <b>{Coverage}</b>";
			}

			// Token: 0x0200312E RID: 12590
			public class DETECTORSCANNING
			{
				// Token: 0x0400C8AC RID: 51372
				public static LocString NAME = "Scanning";

				// Token: 0x0400C8AD RID: 51373
				public static LocString TOOLTIP = "This scanner is currently scouring space for anything of interest";
			}

			// Token: 0x0200312F RID: 12591
			public class INCOMINGMETEORS
			{
				// Token: 0x0400C8AE RID: 51374
				public static LocString NAME = "Incoming Object Detected";

				// Token: 0x0400C8AF RID: 51375
				public static LocString TOOLTIP = "Warning!\n\nHigh velocity objects on approach!";
			}

			// Token: 0x02003130 RID: 12592
			public class SPACE_VISIBILITY_NONE
			{
				// Token: 0x0400C8B0 RID: 51376
				public static LocString NAME = "No Line of Sight";

				// Token: 0x0400C8B1 RID: 51377
				public static LocString TOOLTIP = "This building has no view of space\n\nTo properly function, this building requires an unblocked view of space\n    • Efficiency: <b>{VISIBILITY}</b>";
			}

			// Token: 0x02003131 RID: 12593
			public class SPACE_VISIBILITY_REDUCED
			{
				// Token: 0x0400C8B2 RID: 51378
				public static LocString NAME = "Reduced Visibility";

				// Token: 0x0400C8B3 RID: 51379
				public static LocString TOOLTIP = "This building has a partially obstructed view of space\n\nTo operate at maximum speed, this building requires an unblocked view of space\n    • Efficiency: <b>{VISIBILITY}</b>";
			}

			// Token: 0x02003132 RID: 12594
			public class LANDEDROCKETLACKSPASSENGERMODULE
			{
				// Token: 0x0400C8B4 RID: 51380
				public static LocString NAME = "Rocket lacks spacefarer module";

				// Token: 0x0400C8B5 RID: 51381
				public static LocString TOOLTIP = "A rocket must have a spacefarer module";
			}

			// Token: 0x02003133 RID: 12595
			public class PATH_NOT_CLEAR
			{
				// Token: 0x0400C8B6 RID: 51382
				public static LocString NAME = "Launch Path Blocked";

				// Token: 0x0400C8B7 RID: 51383
				public static LocString TOOLTIP = "There are obstructions in the launch trajectory of this rocket:\n    • {0}\n\nThis rocket requires a clear flight path for launch";

				// Token: 0x0400C8B8 RID: 51384
				public static LocString TILE_FORMAT = "Solid {0}";
			}

			// Token: 0x02003134 RID: 12596
			public class RAILGUN_PATH_NOT_CLEAR
			{
				// Token: 0x0400C8B9 RID: 51385
				public static LocString NAME = "Launch Path Blocked";

				// Token: 0x0400C8BA RID: 51386
				public static LocString TOOLTIP = "There are obstructions in the launch trajectory of this " + UI.FormatAsLink("Interplanetary Launcher", "RAILGUN") + "\n\nThis launcher requires a clear path to launch payloads";
			}

			// Token: 0x02003135 RID: 12597
			public class RAILGUN_NO_DESTINATION
			{
				// Token: 0x0400C8BB RID: 51387
				public static LocString NAME = "No Delivery Destination";

				// Token: 0x0400C8BC RID: 51388
				public static LocString TOOLTIP = "A delivery destination has not been set";
			}

			// Token: 0x02003136 RID: 12598
			public class NOSURFACESIGHT
			{
				// Token: 0x0400C8BD RID: 51389
				public static LocString NAME = "No Line of Sight";

				// Token: 0x0400C8BE RID: 51390
				public static LocString TOOLTIP = "This building has no view of space\n\nTo properly function, this building requires an unblocked view of space";
			}

			// Token: 0x02003137 RID: 12599
			public class ROCKETRESTRICTIONACTIVE
			{
				// Token: 0x0400C8BF RID: 51391
				public static LocString NAME = "Access: Restricted";

				// Token: 0x0400C8C0 RID: 51392
				public static LocString TOOLTIP = "This building cannot be operated while restricted, though it can be filled\n\nControlled by a " + BUILDINGS.PREFABS.ROCKETCONTROLSTATION.NAME;
			}

			// Token: 0x02003138 RID: 12600
			public class ROCKETRESTRICTIONINACTIVE
			{
				// Token: 0x0400C8C1 RID: 51393
				public static LocString NAME = "Access: Not Restricted";

				// Token: 0x0400C8C2 RID: 51394
				public static LocString TOOLTIP = "This building's operation is not restricted\n\nControlled by a " + BUILDINGS.PREFABS.ROCKETCONTROLSTATION.NAME;
			}

			// Token: 0x02003139 RID: 12601
			public class NOROCKETRESTRICTION
			{
				// Token: 0x0400C8C3 RID: 51395
				public static LocString NAME = "Not Controlled";

				// Token: 0x0400C8C4 RID: 51396
				public static LocString TOOLTIP = "This building is not controlled by a " + BUILDINGS.PREFABS.ROCKETCONTROLSTATION.NAME;
			}

			// Token: 0x0200313A RID: 12602
			public class BROADCASTEROUTOFRANGE
			{
				// Token: 0x0400C8C5 RID: 51397
				public static LocString NAME = "Broadcaster Out of Range";

				// Token: 0x0400C8C6 RID: 51398
				public static LocString TOOLTIP = "This receiver is too far from the selected broadcaster to get signal updates";
			}

			// Token: 0x0200313B RID: 12603
			public class LOSINGRADBOLTS
			{
				// Token: 0x0400C8C7 RID: 51399
				public static LocString NAME = "Radbolt Decay";

				// Token: 0x0400C8C8 RID: 51400
				public static LocString TOOLTIP = "This building is unable to maintain the integrity of the radbolts it is storing";
			}

			// Token: 0x0200313C RID: 12604
			public class TOP_PRIORITY_CHORE
			{
				// Token: 0x0400C8C9 RID: 51401
				public static LocString NAME = "Top Priority";

				// Token: 0x0400C8CA RID: 51402
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This errand has been set to ",
					UI.PRE_KEYWORD,
					"Top Priority",
					UI.PST_KEYWORD,
					"\n\nThe colony will be in ",
					UI.PRE_KEYWORD,
					"Yellow Alert",
					UI.PST_KEYWORD,
					" until this task is completed"
				});

				// Token: 0x0400C8CB RID: 51403
				public static LocString NOTIFICATION_NAME = "Yellow Alert";

				// Token: 0x0400C8CC RID: 51404
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"The following errands have been set to ",
					UI.PRE_KEYWORD,
					"Top Priority",
					UI.PST_KEYWORD,
					":"
				});
			}

			// Token: 0x0200313D RID: 12605
			public class HOTTUBWATERTOOCOLD
			{
				// Token: 0x0400C8CD RID: 51405
				public static LocString NAME = "Water Too Cold";

				// Token: 0x0400C8CE RID: 51406
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This tub's ",
					UI.PRE_KEYWORD,
					"Water",
					UI.PST_KEYWORD,
					" is below <b>{temperature}</b>\n\nIt is draining so it can be refilled with warmer ",
					UI.PRE_KEYWORD,
					"Water",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x0200313E RID: 12606
			public class HOTTUBTOOHOT
			{
				// Token: 0x0400C8CF RID: 51407
				public static LocString NAME = "Building Too Hot";

				// Token: 0x0400C8D0 RID: 51408
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This tub's ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" is above <b>{temperature}</b>\n\nIt needs to cool before it can safely be used"
				});
			}

			// Token: 0x0200313F RID: 12607
			public class HOTTUBFILLING
			{
				// Token: 0x0400C8D1 RID: 51409
				public static LocString NAME = "Filling Up: ({fullness})";

				// Token: 0x0400C8D2 RID: 51410
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This tub is currently filling with ",
					UI.PRE_KEYWORD,
					"Water",
					UI.PST_KEYWORD,
					"\n\nIt will be available to use when the ",
					UI.PRE_KEYWORD,
					"Water",
					UI.PST_KEYWORD,
					" level reaches <b>100%</b>"
				});
			}

			// Token: 0x02003140 RID: 12608
			public class WINDTUNNELINTAKE
			{
				// Token: 0x0400C8D3 RID: 51411
				public static LocString NAME = "Intake Requires Gas";

				// Token: 0x0400C8D4 RID: 51412
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"A wind tunnel requires ",
					UI.PRE_KEYWORD,
					"Gas",
					UI.PST_KEYWORD,
					" at the top and bottom intakes in order to operate\n\nThe intakes for this wind tunnel don't have enough gas to operate"
				});
			}

			// Token: 0x02003141 RID: 12609
			public class TEMPORAL_TEAR_OPENER_NO_TARGET
			{
				// Token: 0x0400C8D5 RID: 51413
				public static LocString NAME = "Temporal Tear not revealed";

				// Token: 0x0400C8D6 RID: 51414
				public static LocString TOOLTIP = "This machine is meant to target something in space, but the target has not yet been revealed";
			}

			// Token: 0x02003142 RID: 12610
			public class TEMPORAL_TEAR_OPENER_NO_LOS
			{
				// Token: 0x0400C8D7 RID: 51415
				public static LocString NAME = "Line of Sight: Obstructed";

				// Token: 0x0400C8D8 RID: 51416
				public static LocString TOOLTIP = "This device needs a clear view of space to operate";
			}

			// Token: 0x02003143 RID: 12611
			public class TEMPORAL_TEAR_OPENER_INSUFFICIENT_COLONIES
			{
				// Token: 0x0400C8D9 RID: 51417
				public static LocString NAME = "Too few Printing Pods {progress}";

				// Token: 0x0400C8DA RID: 51418
				public static LocString TOOLTIP = "To open the Temporal Tear, this device relies on a network of activated Printing Pods {progress}";
			}

			// Token: 0x02003144 RID: 12612
			public class TEMPORAL_TEAR_OPENER_PROGRESS
			{
				// Token: 0x0400C8DB RID: 51419
				public static LocString NAME = "Charging Progress: {progress}";

				// Token: 0x0400C8DC RID: 51420
				public static LocString TOOLTIP = "This device must be charged with a high number of Radbolts\n\nOperation can commence once this device is fully charged";
			}

			// Token: 0x02003145 RID: 12613
			public class TEMPORAL_TEAR_OPENER_READY
			{
				// Token: 0x0400C8DD RID: 51421
				public static LocString NOTIFICATION = "Temporal Tear Opener fully charged";

				// Token: 0x0400C8DE RID: 51422
				public static LocString NOTIFICATION_TOOLTIP = "Push the red button to activate";
			}

			// Token: 0x02003146 RID: 12614
			public class WARPPORTALCHARGING
			{
				// Token: 0x0400C8DF RID: 51423
				public static LocString NAME = "Recharging: {charge}";

				// Token: 0x0400C8E0 RID: 51424
				public static LocString TOOLTIP = "This teleporter will be ready for use in {cycles} cycles";
			}

			// Token: 0x02003147 RID: 12615
			public class WARPCONDUITPARTNERDISABLED
			{
				// Token: 0x0400C8E1 RID: 51425
				public static LocString NAME = "Teleporter Disabled ({x}/2)";

				// Token: 0x0400C8E2 RID: 51426
				public static LocString TOOLTIP = "This teleporter cannot be used until both the transmitting and receiving sides have been activated";
			}

			// Token: 0x02003148 RID: 12616
			public class COLLECTINGHEP
			{
				// Token: 0x0400C8E3 RID: 51427
				public static LocString NAME = "Collecting Radbolts ({x}/cycle)";

				// Token: 0x0400C8E4 RID: 51428
				public static LocString TOOLTIP = "Collecting Radbolts from ambient radiation";
			}

			// Token: 0x02003149 RID: 12617
			public class INORBIT
			{
				// Token: 0x0400C8E5 RID: 51429
				public static LocString NAME = "In Orbit: {Destination}";

				// Token: 0x0400C8E6 RID: 51430
				public static LocString TOOLTIP = "This rocket is currently in orbit around {Destination}";
			}

			// Token: 0x0200314A RID: 12618
			public class WAITINGTOLAND
			{
				// Token: 0x0400C8E7 RID: 51431
				public static LocString NAME = "Waiting to land on {Destination}";

				// Token: 0x0400C8E8 RID: 51432
				public static LocString TOOLTIP = "This rocket is waiting for an available Rcoket Platform on {Destination}";
			}

			// Token: 0x0200314B RID: 12619
			public class INFLIGHT
			{
				// Token: 0x0400C8E9 RID: 51433
				public static LocString NAME = "In Flight To {Destination_Asteroid}: {ETA}";

				// Token: 0x0400C8EA RID: 51434
				public static LocString TOOLTIP = "This rocket is currently traveling to {Destination_Pad} on {Destination_Asteroid}\n\nIt will arrive in {ETA}";

				// Token: 0x0400C8EB RID: 51435
				public static LocString TOOLTIP_NO_PAD = "This rocket is currently traveling to {Destination_Asteroid}\n\nIt will arrive in {ETA}";
			}

			// Token: 0x0200314C RID: 12620
			public class DESTINATIONOUTOFRANGE
			{
				// Token: 0x0400C8EC RID: 51436
				public static LocString NAME = "Destination Out Of Range";

				// Token: 0x0400C8ED RID: 51437
				public static LocString TOOLTIP = "This rocket lacks the range to reach its destination\n\nRocket Range: {Range}\nDestination Distance: {Distance}";
			}

			// Token: 0x0200314D RID: 12621
			public class ROCKETSTRANDED
			{
				// Token: 0x0400C8EE RID: 51438
				public static LocString NAME = "Stranded";

				// Token: 0x0400C8EF RID: 51439
				public static LocString TOOLTIP = "This rocket has run out of fuel and cannot move";
			}

			// Token: 0x0200314E RID: 12622
			public class SPACEPOIHARVESTING
			{
				// Token: 0x0400C8F0 RID: 51440
				public static LocString NAME = "Extracting Resources: {0}";

				// Token: 0x0400C8F1 RID: 51441
				public static LocString TOOLTIP = "Resources are being mined from this space debris";
			}

			// Token: 0x0200314F RID: 12623
			public class SPACEPOIWASTING
			{
				// Token: 0x0400C8F2 RID: 51442
				public static LocString NAME = "Cannot store resources: {0}";

				// Token: 0x0400C8F3 RID: 51443
				public static LocString TOOLTIP = "Some resources being mined from this space debris cannot be stored in this rocket";
			}

			// Token: 0x02003150 RID: 12624
			public class RAILGUNPAYLOADNEEDSEMPTYING
			{
				// Token: 0x0400C8F4 RID: 51444
				public static LocString NAME = "Ready To Unpack";

				// Token: 0x0400C8F5 RID: 51445
				public static LocString TOOLTIP = "This payload has reached its destination and is ready to be unloaded\n\nIt can be marked for unpacking manually, or automatically unpacked on arrival using a " + BUILDINGS.PREFABS.RAILGUNPAYLOADOPENER.NAME;
			}

			// Token: 0x02003151 RID: 12625
			public class MISSIONCONTROLASSISTINGROCKET
			{
				// Token: 0x0400C8F6 RID: 51446
				public static LocString NAME = "Guidance Signal: {0}";

				// Token: 0x0400C8F7 RID: 51447
				public static LocString TOOLTIP = "Once transmission is complete, Mission Control will boost targeted rocket's speed";
			}

			// Token: 0x02003152 RID: 12626
			public class MISSIONCONTROLBOOSTED
			{
				// Token: 0x0400C8F8 RID: 51448
				public static LocString NAME = "Mission Control Speed Boost: {0}";

				// Token: 0x0400C8F9 RID: 51449
				public static LocString TOOLTIP = "Mission Control has given this rocket a {0} speed boost\n\n{1} remaining";
			}

			// Token: 0x02003153 RID: 12627
			public class TRANSITTUBEENTRANCEWAXREADY
			{
				// Token: 0x0400C8FA RID: 51450
				public static LocString NAME = "Smooth Ride Ready";

				// Token: 0x0400C8FB RID: 51451
				public static LocString TOOLTIP = "This building is stocked with speed-boosting " + ELEMENTS.MILKFAT.NAME + "\n\n{0} per use ({1} remaining)";
			}

			// Token: 0x02003154 RID: 12628
			public class NOROCKETSTOMISSIONCONTROLBOOST
			{
				// Token: 0x0400C8FC RID: 51452
				public static LocString NAME = "No Eligible Rockets in Range";

				// Token: 0x0400C8FD RID: 51453
				public static LocString TOOLTIP = "Rockets must be mid-flight and not targeted by another Mission Control Station, or already boosted";
			}

			// Token: 0x02003155 RID: 12629
			public class NOROCKETSTOMISSIONCONTROLCLUSTERBOOST
			{
				// Token: 0x0400C8FE RID: 51454
				public static LocString NAME = "No Eligible Rockets in Range";

				// Token: 0x0400C8FF RID: 51455
				public static LocString TOOLTIP = "Rockets must be mid-flight, within {0} tiles, and not targeted by another Mission Control Station or already boosted";
			}

			// Token: 0x02003156 RID: 12630
			public class AWAITINGEMPTYBUILDING
			{
				// Token: 0x0400C900 RID: 51456
				public static LocString NAME = "Empty Errand";

				// Token: 0x0400C901 RID: 51457
				public static LocString TOOLTIP = "Building will be emptied once a Duplicant is available";
			}

			// Token: 0x02003157 RID: 12631
			public class DUPLICANTACTIVATIONREQUIRED
			{
				// Token: 0x0400C902 RID: 51458
				public static LocString NAME = "Activation Required";

				// Token: 0x0400C903 RID: 51459
				public static LocString TOOLTIP = "A Duplicant is required to bring this building online";
			}

			// Token: 0x02003158 RID: 12632
			public class PILOTNEEDED
			{
				// Token: 0x0400C904 RID: 51460
				public static LocString NAME = "Switching to Autopilot";

				// Token: 0x0400C905 RID: 51461
				public static LocString TOOLTIP = "Autopilot will engage in {timeRemaining} if a Duplicant pilot does not assume control";
			}

			// Token: 0x02003159 RID: 12633
			public class AUTOPILOTACTIVE
			{
				// Token: 0x0400C906 RID: 51462
				public static LocString NAME = "Autopilot Engaged";

				// Token: 0x0400C907 RID: 51463
				public static LocString TOOLTIP = "This rocket has entered autopilot mode and will fly at reduced speed\n\nIt can resume full speed once a Duplicant pilot takes over";
			}

			// Token: 0x0200315A RID: 12634
			public class ROCKETCHECKLISTINCOMPLETE
			{
				// Token: 0x0400C908 RID: 51464
				public static LocString NAME = "Launch Checklist Incomplete";

				// Token: 0x0400C909 RID: 51465
				public static LocString TOOLTIP = "Critical launch tasks uncompleted\n\nRefer to the Launch Checklist in the status panel";
			}

			// Token: 0x0200315B RID: 12635
			public class ROCKETCARGOEMPTYING
			{
				// Token: 0x0400C90A RID: 51466
				public static LocString NAME = "Unloading Cargo";

				// Token: 0x0400C90B RID: 51467
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Rocket cargo is being unloaded into the ",
					UI.PRE_KEYWORD,
					"Rocket Platform",
					UI.PST_KEYWORD,
					"\n\nLoading of new cargo will begin once unloading is complete"
				});
			}

			// Token: 0x0200315C RID: 12636
			public class ROCKETCARGOFILLING
			{
				// Token: 0x0400C90C RID: 51468
				public static LocString NAME = "Loading Cargo";

				// Token: 0x0400C90D RID: 51469
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Cargo is being loaded onto the rocket from the ",
					UI.PRE_KEYWORD,
					"Rocket Platform",
					UI.PST_KEYWORD,
					"\n\nRocket cargo will be ready for launch once loading is complete"
				});
			}

			// Token: 0x0200315D RID: 12637
			public class ROCKETCARGOFULL
			{
				// Token: 0x0400C90E RID: 51470
				public static LocString NAME = "Platform Ready";

				// Token: 0x0400C90F RID: 51471
				public static LocString TOOLTIP = "All cargo operations are complete";
			}

			// Token: 0x0200315E RID: 12638
			public class FLIGHTALLCARGOFULL
			{
				// Token: 0x0400C910 RID: 51472
				public static LocString NAME = "All cargo bays are full";

				// Token: 0x0400C911 RID: 51473
				public static LocString TOOLTIP = "Rocket cannot store any more materials";
			}

			// Token: 0x0200315F RID: 12639
			public class FLIGHTCARGOREMAINING
			{
				// Token: 0x0400C912 RID: 51474
				public static LocString NAME = "Cargo capacity remaining: {0}";

				// Token: 0x0400C913 RID: 51475
				public static LocString TOOLTIP = "Rocket can store up to {0} more materials";
			}

			// Token: 0x02003160 RID: 12640
			public class ROCKET_PORT_IDLE
			{
				// Token: 0x0400C914 RID: 51476
				public static LocString NAME = "Idle";

				// Token: 0x0400C915 RID: 51477
				public static LocString TOOLTIP = "This port is idle because there is no rocket on the connected " + UI.PRE_KEYWORD + "Rocket Platform" + UI.PST_KEYWORD;
			}

			// Token: 0x02003161 RID: 12641
			public class ROCKET_PORT_UNLOADING
			{
				// Token: 0x0400C916 RID: 51478
				public static LocString NAME = "Unloading Rocket";

				// Token: 0x0400C917 RID: 51479
				public static LocString TOOLTIP = "Resources are being unloaded from the rocket into the local network";
			}

			// Token: 0x02003162 RID: 12642
			public class ROCKET_PORT_LOADING
			{
				// Token: 0x0400C918 RID: 51480
				public static LocString NAME = "Loading Rocket";

				// Token: 0x0400C919 RID: 51481
				public static LocString TOOLTIP = "Resources are being loaded from the local network into the rocket's storage";
			}

			// Token: 0x02003163 RID: 12643
			public class ROCKET_PORT_LOADED
			{
				// Token: 0x0400C91A RID: 51482
				public static LocString NAME = "Cargo Transfer Complete";

				// Token: 0x0400C91B RID: 51483
				public static LocString TOOLTIP = "The connected rocket has either reached max capacity for this resource type, or lacks appropriate storage modules";
			}

			// Token: 0x02003164 RID: 12644
			public class CONNECTED_ROCKET_PORT
			{
				// Token: 0x0400C91C RID: 51484
				public static LocString NAME = "Port Network Attached";

				// Token: 0x0400C91D RID: 51485
				public static LocString TOOLTIP = "This module has been connected to a " + BUILDINGS.PREFABS.MODULARLAUNCHPADPORT.NAME + " and can now load and unload cargo";
			}

			// Token: 0x02003165 RID: 12645
			public class CONNECTED_ROCKET_WRONG_PORT
			{
				// Token: 0x0400C91E RID: 51486
				public static LocString NAME = "Incorrect Port Network";

				// Token: 0x0400C91F RID: 51487
				public static LocString TOOLTIP = "The attached " + BUILDINGS.PREFABS.MODULARLAUNCHPADPORT.NAME + " is not the correct type for this cargo module";
			}

			// Token: 0x02003166 RID: 12646
			public class CONNECTED_ROCKET_NO_PORT
			{
				// Token: 0x0400C920 RID: 51488
				public static LocString NAME = "No Rocket Ports";

				// Token: 0x0400C921 RID: 51489
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This ",
					UI.PRE_KEYWORD,
					"Rocket Platform",
					UI.PST_KEYWORD,
					" has no ",
					BUILDINGS.PREFABS.MODULARLAUNCHPADPORT.NAME,
					" attached\n\n",
					UI.PRE_KEYWORD,
					"Solid",
					UI.PST_KEYWORD,
					", ",
					UI.PRE_KEYWORD,
					"Gas",
					UI.PST_KEYWORD,
					", and ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					" ",
					BUILDINGS.PREFABS.MODULARLAUNCHPADPORT.NAME_PLURAL,
					" can be attached to load and unload cargo from a landed rocket's modules"
				});
			}

			// Token: 0x02003167 RID: 12647
			public class CLUSTERTELESCOPEALLWORKCOMPLETE
			{
				// Token: 0x0400C922 RID: 51490
				public static LocString NAME = "Area Complete";

				// Token: 0x0400C923 RID: 51491
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This ",
					UI.PRE_KEYWORD,
					"Telescope",
					UI.PST_KEYWORD,
					" has analyzed all the space visible from its current location"
				});
			}

			// Token: 0x02003168 RID: 12648
			public class ROCKETPLATFORMCLOSETOCEILING
			{
				// Token: 0x0400C924 RID: 51492
				public static LocString NAME = "Low Clearance: {distance} Tiles";

				// Token: 0x0400C925 RID: 51493
				public static LocString TOOLTIP = "Tall rockets may not be able to land on this " + UI.PRE_KEYWORD + "Rocket Platform" + UI.PST_KEYWORD;
			}

			// Token: 0x02003169 RID: 12649
			public class MODULEGENERATORNOTPOWERED
			{
				// Token: 0x0400C926 RID: 51494
				public static LocString NAME = "Thrust Generation: {ActiveWattage}/{MaxWattage}";

				// Token: 0x0400C927 RID: 51495
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Engine will generate ",
					UI.FormatAsPositiveRate("{MaxWattage}"),
					" of ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" once traveling through space\n\nRight now, it's not doing much of anything"
				});
			}

			// Token: 0x0200316A RID: 12650
			public class MODULEGENERATORPOWERED
			{
				// Token: 0x0400C928 RID: 51496
				public static LocString NAME = "Thrust Generation: {ActiveWattage}/{MaxWattage}";

				// Token: 0x0400C929 RID: 51497
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Engine is extracting ",
					UI.FormatAsPositiveRate("{MaxWattage}"),
					" of ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" from the thruster\n\nIt will continue generating power as long as it travels through space"
				});
			}

			// Token: 0x0200316B RID: 12651
			public class INORBITREQUIRED
			{
				// Token: 0x0400C92A RID: 51498
				public static LocString NAME = "Grounded";

				// Token: 0x0400C92B RID: 51499
				public static LocString TOOLTIP = "This building cannot operate from the surface of a " + UI.CLUSTERMAP.PLANETOID_KEYWORD + " and must be in space to function";
			}

			// Token: 0x0200316C RID: 12652
			public class REACTORREFUELDISABLED
			{
				// Token: 0x0400C92C RID: 51500
				public static LocString NAME = "Refuel Disabled";

				// Token: 0x0400C92D RID: 51501
				public static LocString TOOLTIP = "This building will not be refueled once its active fuel has been consumed";
			}

			// Token: 0x0200316D RID: 12653
			public class RAILGUNCOOLDOWN
			{
				// Token: 0x0400C92E RID: 51502
				public static LocString NAME = "Cleaning Rails: {timeleft}";

				// Token: 0x0400C92F RID: 51503
				public static LocString TOOLTIP = "This building automatically performs routine maintenance every {x} launches";
			}

			// Token: 0x0200316E RID: 12654
			public class FRIDGECOOLING
			{
				// Token: 0x0400C930 RID: 51504
				public static LocString NAME = "Cooling Contents: {UsedPower}";

				// Token: 0x0400C931 RID: 51505
				public static LocString TOOLTIP = "{UsedPower} of {MaxPower} are being used to cool the contents of this food storage";
			}

			// Token: 0x0200316F RID: 12655
			public class FRIDGESTEADY
			{
				// Token: 0x0400C932 RID: 51506
				public static LocString NAME = "Energy Saver: {UsedPower}";

				// Token: 0x0400C933 RID: 51507
				public static LocString TOOLTIP = "The contents of this food storage are at refrigeration temperatures\n\nEnergy Saver mode has been automatically activated using only {UsedPower} of {MaxPower}";
			}

			// Token: 0x02003170 RID: 12656
			public class TELEPHONE
			{
				// Token: 0x02003171 RID: 12657
				public class BABBLE
				{
					// Token: 0x0400C934 RID: 51508
					public static LocString NAME = "Babbling to no one.";

					// Token: 0x0400C935 RID: 51509
					public static LocString TOOLTIP = "{Duplicant} just needed to vent to into the void.";
				}

				// Token: 0x02003172 RID: 12658
				public class CONVERSATION
				{
					// Token: 0x0400C936 RID: 51510
					public static LocString TALKING_TO = "Talking to {Duplicant} on {Asteroid}";

					// Token: 0x0400C937 RID: 51511
					public static LocString TALKING_TO_NUM = "Talking to {0} friends.";
				}
			}

			// Token: 0x02003173 RID: 12659
			public class CREATUREMANIPULATORPROGRESS
			{
				// Token: 0x0400C938 RID: 51512
				public static LocString NAME = "Collected Species Data {0}/{1}";

				// Token: 0x0400C939 RID: 51513
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building requires data from multiple ",
					UI.PRE_KEYWORD,
					"Critter",
					UI.PST_KEYWORD,
					" species to unlock its genetic manipulator\n\nSpecies scanned:"
				});

				// Token: 0x0400C93A RID: 51514
				public static LocString NO_DATA = "No species scanned";
			}

			// Token: 0x02003174 RID: 12660
			public class CREATUREMANIPULATORMORPHMODELOCKED
			{
				// Token: 0x0400C93B RID: 51515
				public static LocString NAME = "Current Status: Offline";

				// Token: 0x0400C93C RID: 51516
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building cannot operate until it collects more ",
					UI.PRE_KEYWORD,
					"Critter",
					UI.PST_KEYWORD,
					" DNA"
				});
			}

			// Token: 0x02003175 RID: 12661
			public class CREATUREMANIPULATORMORPHMODE
			{
				// Token: 0x0400C93D RID: 51517
				public static LocString NAME = "Current Status: Online";

				// Token: 0x0400C93E RID: 51518
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building is ready to manipulate ",
					UI.PRE_KEYWORD,
					"Critter",
					UI.PST_KEYWORD,
					" DNA"
				});
			}

			// Token: 0x02003176 RID: 12662
			public class CREATUREMANIPULATORWAITING
			{
				// Token: 0x0400C93F RID: 51519
				public static LocString NAME = "Waiting for a Critter";

				// Token: 0x0400C940 RID: 51520
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building is waiting for a ",
					UI.PRE_KEYWORD,
					"Critter",
					UI.PST_KEYWORD,
					" to get sucked into its scanning area"
				});
			}

			// Token: 0x02003177 RID: 12663
			public class CREATUREMANIPULATORWORKING
			{
				// Token: 0x0400C941 RID: 51521
				public static LocString NAME = "Poking and Prodding Critter";

				// Token: 0x0400C942 RID: 51522
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building is extracting genetic information from a ",
					UI.PRE_KEYWORD,
					"Critter",
					UI.PST_KEYWORD,
					" "
				});
			}

			// Token: 0x02003178 RID: 12664
			public class SPICEGRINDERNOSPICE
			{
				// Token: 0x0400C943 RID: 51523
				public static LocString NAME = "No Spice Selected";

				// Token: 0x0400C944 RID: 51524
				public static LocString TOOLTIP = "Select a recipe to begin fabrication";
			}

			// Token: 0x02003179 RID: 12665
			public class SPICEGRINDERACCEPTSMUTANTSEEDS
			{
				// Token: 0x0400C945 RID: 51525
				public static LocString NAME = "Spice Grinder accepts mutant seeds";

				// Token: 0x0400C946 RID: 51526
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This spice grinder is allowed to use ",
					UI.PRE_KEYWORD,
					"Mutant Seeds",
					UI.PST_KEYWORD,
					" as recipe ingredients"
				});
			}

			// Token: 0x0200317A RID: 12666
			public class MISSILELAUNCHER_NOSURFACESIGHT
			{
				// Token: 0x0400C947 RID: 51527
				public static LocString NAME = "No Line of Sight";

				// Token: 0x0400C948 RID: 51528
				public static LocString TOOLTIP = "This building has no view of space\n\nTo properly function, this building requires an unblocked view of space";
			}

			// Token: 0x0200317B RID: 12667
			public class MISSILELAUNCHER_PARTIALLYBLOCKED
			{
				// Token: 0x0400C949 RID: 51529
				public static LocString NAME = "Limited Line of Sight";

				// Token: 0x0400C94A RID: 51530
				public static LocString TOOLTIP = "This building has a partially obstructed view of space\n\nTo properly function, this building requires an unblocked view of space";
			}

			// Token: 0x0200317C RID: 12668
			public class COMPLEXFABRICATOR
			{
				// Token: 0x0200317D RID: 12669
				public class COOKING
				{
					// Token: 0x0400C94B RID: 51531
					public static LocString NAME = "Cooking {Item}";

					// Token: 0x0400C94C RID: 51532
					public static LocString TOOLTIP = "This building is currently whipping up a batch of {Item}";
				}

				// Token: 0x0200317E RID: 12670
				public class PRODUCING
				{
					// Token: 0x0400C94D RID: 51533
					public static LocString NAME = "Producing {Item}";

					// Token: 0x0400C94E RID: 51534
					public static LocString TOOLTIP = "This building is carrying out its current production orders";
				}

				// Token: 0x0200317F RID: 12671
				public class RESEARCHING
				{
					// Token: 0x0400C94F RID: 51535
					public static LocString NAME = "Researching {Item}";

					// Token: 0x0400C950 RID: 51536
					public static LocString TOOLTIP = "This building is currently conducting important research";
				}

				// Token: 0x02003180 RID: 12672
				public class ANALYZING
				{
					// Token: 0x0400C951 RID: 51537
					public static LocString NAME = "Analyzing {Item}";

					// Token: 0x0400C952 RID: 51538
					public static LocString TOOLTIP = "This building is currently analyzing a fascinating artifact";
				}

				// Token: 0x02003181 RID: 12673
				public class UNTRAINING
				{
					// Token: 0x0400C953 RID: 51539
					public static LocString NAME = "Untraining {Duplicant}";

					// Token: 0x0400C954 RID: 51540
					public static LocString TOOLTIP = "Restoring {Duplicant} to a blissfully ignorant state";
				}

				// Token: 0x02003182 RID: 12674
				public class TELESCOPE
				{
					// Token: 0x0400C955 RID: 51541
					public static LocString NAME = "Studying Space";

					// Token: 0x0400C956 RID: 51542
					public static LocString TOOLTIP = "This building is currently investigating the mysteries of space";
				}

				// Token: 0x02003183 RID: 12675
				public class CLUSTERTELESCOPEMETEOR
				{
					// Token: 0x0400C957 RID: 51543
					public static LocString NAME = "Studying Meteor";

					// Token: 0x0400C958 RID: 51544
					public static LocString TOOLTIP = "This building is currently studying a meteor";
				}
			}

			// Token: 0x02003184 RID: 12676
			public class REMOTEWORKERDEPOT
			{
				// Token: 0x02003185 RID: 12677
				public class MAKINGWORKER
				{
					// Token: 0x0400C959 RID: 51545
					public static LocString NAME = "Assembling Remote Worker";

					// Token: 0x0400C95A RID: 51546
					public static LocString TOOLTIP = "This building is currently assembling a remote worker drone";
				}
			}

			// Token: 0x02003186 RID: 12678
			public class REMOTEWORKTERMINAL
			{
				// Token: 0x02003187 RID: 12679
				public class NODOCK
				{
					// Token: 0x0400C95B RID: 51547
					public static LocString NAME = "No Dock Assigned";

					// Token: 0x0400C95C RID: 51548
					public static LocString TOOLTIP = string.Concat(new string[]
					{
						"This building must be assigned a ",
						UI.PRE_KEYWORD,
						"Remote Worker Dock",
						UI.PST_KEYWORD,
						" in order to function"
					});
				}
			}

			// Token: 0x02003188 RID: 12680
			public class DATAMINER
			{
				// Token: 0x02003189 RID: 12681
				public class PRODUCTIONRATE
				{
					// Token: 0x0400C95D RID: 51549
					public static LocString NAME = "Production Rate: {RATE}";

					// Token: 0x0400C95E RID: 51550
					public static LocString TOOLTIP = "This building is operating at {RATE} of its maximum speed\n\nProduction rate decreases at higher temperatures\n\nCurrent ambient temperature: {TEMP}";
				}
			}
		}

		// Token: 0x0200318A RID: 12682
		public class DETAILS
		{
			// Token: 0x0400C95F RID: 51551
			public static LocString USE_COUNT = "Uses: {0}";

			// Token: 0x0400C960 RID: 51552
			public static LocString USE_COUNT_TOOLTIP = "This building has been used {0} times";
		}
	}
}
