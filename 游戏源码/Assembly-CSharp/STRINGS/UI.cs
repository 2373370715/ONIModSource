using System;
using System.Collections.Generic;

namespace STRINGS
{
	// Token: 0x02002A41 RID: 10817
	public class UI
	{
		// Token: 0x0600C85E RID: 51294 RVA: 0x000E2135 File Offset: 0x000E0335
		public static string FormatAsBuildMenuTab(string text)
		{
			return "<b>" + text + "</b>";
		}

		// Token: 0x0600C85F RID: 51295 RVA: 0x001232FB File Offset: 0x001214FB
		public static string FormatAsBuildMenuTab(string text, string hotkey)
		{
			return "<b>" + text + "</b> " + UI.FormatAsHotkey(hotkey);
		}

		// Token: 0x0600C860 RID: 51296 RVA: 0x00123313 File Offset: 0x00121513
		public static string FormatAsBuildMenuTab(string text, global::Action a)
		{
			return "<b>" + text + "</b> " + UI.FormatAsHotKey(a);
		}

		// Token: 0x0600C861 RID: 51297 RVA: 0x000E2135 File Offset: 0x000E0335
		public static string FormatAsOverlay(string text)
		{
			return "<b>" + text + "</b>";
		}

		// Token: 0x0600C862 RID: 51298 RVA: 0x001232FB File Offset: 0x001214FB
		public static string FormatAsOverlay(string text, string hotkey)
		{
			return "<b>" + text + "</b> " + UI.FormatAsHotkey(hotkey);
		}

		// Token: 0x0600C863 RID: 51299 RVA: 0x00123313 File Offset: 0x00121513
		public static string FormatAsOverlay(string text, global::Action a)
		{
			return "<b>" + text + "</b> " + UI.FormatAsHotKey(a);
		}

		// Token: 0x0600C864 RID: 51300 RVA: 0x000E2135 File Offset: 0x000E0335
		public static string FormatAsManagementMenu(string text)
		{
			return "<b>" + text + "</b>";
		}

		// Token: 0x0600C865 RID: 51301 RVA: 0x001232FB File Offset: 0x001214FB
		public static string FormatAsManagementMenu(string text, string hotkey)
		{
			return "<b>" + text + "</b> " + UI.FormatAsHotkey(hotkey);
		}

		// Token: 0x0600C866 RID: 51302 RVA: 0x00123313 File Offset: 0x00121513
		public static string FormatAsManagementMenu(string text, global::Action a)
		{
			return "<b>" + text + "</b> " + UI.FormatAsHotKey(a);
		}

		// Token: 0x0600C867 RID: 51303 RVA: 0x0012332B File Offset: 0x0012152B
		public static string FormatAsKeyWord(string text)
		{
			return UI.PRE_KEYWORD + text + UI.PST_KEYWORD;
		}

		// Token: 0x0600C868 RID: 51304 RVA: 0x0012333D File Offset: 0x0012153D
		public static string FormatAsHotkey(string text)
		{
			return "<b><color=#F44A4A>" + text + "</b></color>";
		}

		// Token: 0x0600C869 RID: 51305 RVA: 0x0012334F File Offset: 0x0012154F
		public static string FormatAsHotKey(global::Action a)
		{
			return "{Hotkey/" + a.ToString() + "}";
		}

		// Token: 0x0600C86A RID: 51306 RVA: 0x001232FB File Offset: 0x001214FB
		public static string FormatAsTool(string text, string hotkey)
		{
			return "<b>" + text + "</b> " + UI.FormatAsHotkey(hotkey);
		}

		// Token: 0x0600C86B RID: 51307 RVA: 0x00123313 File Offset: 0x00121513
		public static string FormatAsTool(string text, global::Action a)
		{
			return "<b>" + text + "</b> " + UI.FormatAsHotKey(a);
		}

		// Token: 0x0600C86C RID: 51308 RVA: 0x0012336D File Offset: 0x0012156D
		public static string FormatAsLink(string text, string linkID)
		{
			text = UI.StripLinkFormatting(text);
			linkID = CodexCache.FormatLinkID(linkID);
			return string.Concat(new string[]
			{
				"<link=\"",
				linkID,
				"\">",
				text,
				"</link>"
			});
		}

		// Token: 0x0600C86D RID: 51309 RVA: 0x001233AA File Offset: 0x001215AA
		public static string FormatAsPositiveModifier(string text)
		{
			return UI.PRE_POS_MODIFIER + text + UI.PST_POS_MODIFIER;
		}

		// Token: 0x0600C86E RID: 51310 RVA: 0x001233BC File Offset: 0x001215BC
		public static string FormatAsNegativeModifier(string text)
		{
			return UI.PRE_NEG_MODIFIER + text + UI.PST_NEG_MODIFIER;
		}

		// Token: 0x0600C86F RID: 51311 RVA: 0x001233CE File Offset: 0x001215CE
		public static string FormatAsPositiveRate(string text)
		{
			return UI.PRE_RATE_POSITIVE + text + UI.PST_RATE;
		}

		// Token: 0x0600C870 RID: 51312 RVA: 0x001233E0 File Offset: 0x001215E0
		public static string FormatAsNegativeRate(string text)
		{
			return UI.PRE_RATE_NEGATIVE + text + UI.PST_RATE;
		}

		// Token: 0x0600C871 RID: 51313 RVA: 0x001233F2 File Offset: 0x001215F2
		public static string CLICK(UI.ClickType c)
		{
			return "(ClickType/" + c.ToString() + ")";
		}

		// Token: 0x0600C872 RID: 51314 RVA: 0x00123410 File Offset: 0x00121610
		public static string FormatAsAutomationState(string text, UI.AutomationState state)
		{
			if (state == UI.AutomationState.Active)
			{
				return UI.PRE_AUTOMATION_ACTIVE + text + UI.PST_AUTOMATION;
			}
			return UI.PRE_AUTOMATION_STANDBY + text + UI.PST_AUTOMATION;
		}

		// Token: 0x0600C873 RID: 51315 RVA: 0x00123436 File Offset: 0x00121636
		public static string FormatAsCaps(string text)
		{
			return text.ToUpper();
		}

		// Token: 0x0600C874 RID: 51316 RVA: 0x00484B08 File Offset: 0x00482D08
		public static string ExtractLinkID(string text)
		{
			string text2 = text;
			int num = text2.IndexOf("<link=");
			if (num != -1)
			{
				int num2 = num + 7;
				int num3 = text2.IndexOf(">") - 1;
				text2 = text.Substring(num2, num3 - num2);
			}
			return text2;
		}

		// Token: 0x0600C875 RID: 51317 RVA: 0x00484B48 File Offset: 0x00482D48
		public static string StripLinkFormatting(string text)
		{
			string text2 = text;
			try
			{
				while (text2.Contains("<link="))
				{
					int num = text2.IndexOf("</link>");
					if (num > -1)
					{
						text2 = text2.Remove(num, 7);
					}
					else
					{
						Debug.LogWarningFormat("String has no closing link tag: {0}", new object[]
						{
							text
						});
					}
					int num2 = text2.IndexOf("<link=");
					if (num2 != -1)
					{
						int num3 = text2.IndexOf("\">", num2);
						if (num3 != -1)
						{
							text2 = text2.Remove(num2, num3 - num2 + 2);
						}
						else
						{
							text2 = text2.Remove(num2, "<link=".Length);
							Debug.LogWarningFormat("String has no open link closure: {0}", new object[]
							{
								text
							});
						}
					}
					else
					{
						Debug.LogWarningFormat("String has no open link tag: {0}", new object[]
						{
							text
						});
					}
				}
			}
			catch
			{
				Debug.Log("STRIP LINK FORMATTING FAILED ON: " + text);
				text2 = text;
			}
			return text2;
		}

		// Token: 0x0400AF36 RID: 44854
		public static string PRE_KEYWORD = "<style=\"KKeyword\">";

		// Token: 0x0400AF37 RID: 44855
		public static string PST_KEYWORD = "</style>";

		// Token: 0x0400AF38 RID: 44856
		public static string PRE_POS_MODIFIER = "<b>";

		// Token: 0x0400AF39 RID: 44857
		public static string PST_POS_MODIFIER = "</b>";

		// Token: 0x0400AF3A RID: 44858
		public static string PRE_NEG_MODIFIER = "<b>";

		// Token: 0x0400AF3B RID: 44859
		public static string PST_NEG_MODIFIER = "</b>";

		// Token: 0x0400AF3C RID: 44860
		public static string PRE_RATE_NEGATIVE = "<style=\"consumed\">";

		// Token: 0x0400AF3D RID: 44861
		public static string PRE_RATE_POSITIVE = "<style=\"produced\">";

		// Token: 0x0400AF3E RID: 44862
		public static string PST_RATE = "</style>";

		// Token: 0x0400AF3F RID: 44863
		public static string CODEXLINK = "BUILDCATEGORYREQUIREMENTCLASS";

		// Token: 0x0400AF40 RID: 44864
		public static string PRE_AUTOMATION_ACTIVE = "<b><style=\"logic_on\">";

		// Token: 0x0400AF41 RID: 44865
		public static string PRE_AUTOMATION_STANDBY = "<b><style=\"logic_off\">";

		// Token: 0x0400AF42 RID: 44866
		public static string PST_AUTOMATION = "</style></b>";

		// Token: 0x0400AF43 RID: 44867
		public static string YELLOW_PREFIX = "<color=#ffff00ff>";

		// Token: 0x0400AF44 RID: 44868
		public static string COLOR_SUFFIX = "</color>";

		// Token: 0x0400AF45 RID: 44869
		public static string HORIZONTAL_RULE = "------------------";

		// Token: 0x0400AF46 RID: 44870
		public static string HORIZONTAL_BR_RULE = "\n" + UI.HORIZONTAL_RULE + "\n";

		// Token: 0x0400AF47 RID: 44871
		public static LocString POS_INFINITY = "Infinity";

		// Token: 0x0400AF48 RID: 44872
		public static LocString NEG_INFINITY = "-Infinity";

		// Token: 0x0400AF49 RID: 44873
		public static LocString PROCEED_BUTTON = "PROCEED";

		// Token: 0x0400AF4A RID: 44874
		public static LocString COPY_BUILDING = "Copy";

		// Token: 0x0400AF4B RID: 44875
		public static LocString COPY_BUILDING_TOOLTIP = "Create new build orders using the most recent building selection as a template. {Hotkey}";

		// Token: 0x0400AF4C RID: 44876
		public static LocString NAME_WITH_UNITS = "{0} x {1}";

		// Token: 0x0400AF4D RID: 44877
		public static LocString NA = "N/A";

		// Token: 0x0400AF4E RID: 44878
		public static LocString POSITIVE_FORMAT = "+{0}";

		// Token: 0x0400AF4F RID: 44879
		public static LocString NEGATIVE_FORMAT = "-{0}";

		// Token: 0x0400AF50 RID: 44880
		public static LocString FILTER = "Filter";

		// Token: 0x0400AF51 RID: 44881
		public static LocString SPEED_SLOW = "SLOW";

		// Token: 0x0400AF52 RID: 44882
		public static LocString SPEED_MEDIUM = "MEDIUM";

		// Token: 0x0400AF53 RID: 44883
		public static LocString SPEED_FAST = "FAST";

		// Token: 0x0400AF54 RID: 44884
		public static LocString RED_ALERT = "RED ALERT";

		// Token: 0x0400AF55 RID: 44885
		public static LocString JOBS = "PRIORITIES";

		// Token: 0x0400AF56 RID: 44886
		public static LocString CONSUMABLES = "CONSUMABLES";

		// Token: 0x0400AF57 RID: 44887
		public static LocString VITALS = "VITALS";

		// Token: 0x0400AF58 RID: 44888
		public static LocString RESEARCH = "RESEARCH";

		// Token: 0x0400AF59 RID: 44889
		public static LocString ROLES = "JOB ASSIGNMENTS";

		// Token: 0x0400AF5A RID: 44890
		public static LocString RESEARCHPOINTS = "Research points";

		// Token: 0x0400AF5B RID: 44891
		public static LocString SCHEDULE = "SCHEDULE";

		// Token: 0x0400AF5C RID: 44892
		public static LocString REPORT = "REPORTS";

		// Token: 0x0400AF5D RID: 44893
		public static LocString SKILLS = "SKILLS";

		// Token: 0x0400AF5E RID: 44894
		public static LocString OVERLAYSTITLE = "OVERLAYS";

		// Token: 0x0400AF5F RID: 44895
		public static LocString ALERTS = "ALERTS";

		// Token: 0x0400AF60 RID: 44896
		public static LocString MESSAGES = "MESSAGES";

		// Token: 0x0400AF61 RID: 44897
		public static LocString ACTIONS = "ACTIONS";

		// Token: 0x0400AF62 RID: 44898
		public static LocString QUEUE = "Queue";

		// Token: 0x0400AF63 RID: 44899
		public static LocString BASECOUNT = "Base {0}";

		// Token: 0x0400AF64 RID: 44900
		public static LocString CHARACTERCONTAINER_SKILLS_TITLE = "ATTRIBUTES";

		// Token: 0x0400AF65 RID: 44901
		public static LocString CHARACTERCONTAINER_TRAITS_TITLE = "TRAITS";

		// Token: 0x0400AF66 RID: 44902
		public static LocString CHARACTERCONTAINER_TRAITS_TITLE_BIONIC = "BIONIC SYSTEMS";

		// Token: 0x0400AF67 RID: 44903
		public static LocString CHARACTERCONTAINER_APTITUDES_TITLE = "INTERESTS";

		// Token: 0x0400AF68 RID: 44904
		public static LocString CHARACTERCONTAINER_APTITUDES_TITLE_TOOLTIP = "A Duplicant's starting Attributes are determined by their Interests\n\nLearning Skills related to their Interests will give Duplicants a Morale boost";

		// Token: 0x0400AF69 RID: 44905
		public static LocString CHARACTERCONTAINER_EXPECTATIONS_TITLE = "ADDITIONAL INFORMATION";

		// Token: 0x0400AF6A RID: 44906
		public static LocString CHARACTERCONTAINER_SKILL_VALUE = " {0} {1}";

		// Token: 0x0400AF6B RID: 44907
		public static LocString CHARACTERCONTAINER_NEED = "{0}: {1}";

		// Token: 0x0400AF6C RID: 44908
		public static LocString CHARACTERCONTAINER_STRESSTRAIT = "Stress Reaction: {0}";

		// Token: 0x0400AF6D RID: 44909
		public static LocString CHARACTERCONTAINER_JOYTRAIT = "Overjoyed Response: {0}";

		// Token: 0x0400AF6E RID: 44910
		public static LocString CHARACTERCONTAINER_CONGENITALTRAIT = "Genetic Trait: {0}";

		// Token: 0x0400AF6F RID: 44911
		public static LocString CHARACTERCONTAINER_NOARCHETYPESELECTED = "Random";

		// Token: 0x0400AF70 RID: 44912
		public static LocString CHARACTERCONTAINER_ARCHETYPESELECT_TOOLTIP = "Change the type of Duplicant the reroll button will produce";

		// Token: 0x0400AF71 RID: 44913
		public static LocString CAREPACKAGECONTAINER_INFORMATION_TITLE = "CARE PACKAGE";

		// Token: 0x0400AF72 RID: 44914
		public static LocString CHARACTERCONTAINER_ALL_MODELS = "Any";

		// Token: 0x0400AF73 RID: 44915
		public static LocString CHARACTERCONTAINER_ATTRIBUTEMODIFIER_INCREASED = "Increased <b>{0}</b>";

		// Token: 0x0400AF74 RID: 44916
		public static LocString CHARACTERCONTAINER_ATTRIBUTEMODIFIER_DECREASED = "Decreased <b>{0}</b>";

		// Token: 0x0400AF75 RID: 44917
		public static LocString CHARACTERCONTAINER_FILTER_STANDARD = "Check box to allow standard Duplicants";

		// Token: 0x0400AF76 RID: 44918
		public static LocString CHARACTERCONTAINER_FILTER_BIONIC = "Check box to allow Bionic Duplicants";

		// Token: 0x0400AF77 RID: 44919
		public static LocString PRODUCTINFO_SELECTMATERIAL = "Select {0}:";

		// Token: 0x0400AF78 RID: 44920
		public static LocString PRODUCTINFO_RESEARCHREQUIRED = "Research required...";

		// Token: 0x0400AF79 RID: 44921
		public static LocString PRODUCTINFO_REQUIRESRESEARCHDESC = "Requires {0} Research";

		// Token: 0x0400AF7A RID: 44922
		public static LocString PRODUCTINFO_APPLICABLERESOURCES = "Required resources:";

		// Token: 0x0400AF7B RID: 44923
		public static LocString PRODUCTINFO_MISSINGRESOURCES_TITLE = "Requires {0}: {1}";

		// Token: 0x0400AF7C RID: 44924
		public static LocString PRODUCTINFO_MISSINGRESOURCES_HOVER = "Missing resources";

		// Token: 0x0400AF7D RID: 44925
		public static LocString PRODUCTINFO_MISSINGRESOURCES_DESC = "{0} has yet to be discovered";

		// Token: 0x0400AF7E RID: 44926
		public static LocString PRODUCTINFO_UNIQUE_PER_WORLD = "Limit one per " + UI.CLUSTERMAP.PLANETOID_KEYWORD;

		// Token: 0x0400AF7F RID: 44927
		public static LocString PRODUCTINFO_ROCKET_INTERIOR = "Rocket interior only";

		// Token: 0x0400AF80 RID: 44928
		public static LocString PRODUCTINFO_ROCKET_NOT_INTERIOR = "Cannot build inside rocket";

		// Token: 0x0400AF81 RID: 44929
		public static LocString BUILDTOOL_ROTATE = "Rotate this building";

		// Token: 0x0400AF82 RID: 44930
		public static LocString BUILDTOOL_ROTATE_CURRENT_DEGREES = "Currently rotated {Degrees} degrees";

		// Token: 0x0400AF83 RID: 44931
		public static LocString BUILDTOOL_ROTATE_CURRENT_LEFT = "Currently facing left";

		// Token: 0x0400AF84 RID: 44932
		public static LocString BUILDTOOL_ROTATE_CURRENT_RIGHT = "Currently facing right";

		// Token: 0x0400AF85 RID: 44933
		public static LocString BUILDTOOL_ROTATE_CURRENT_UP = "Currently facing up";

		// Token: 0x0400AF86 RID: 44934
		public static LocString BUILDTOOL_ROTATE_CURRENT_DOWN = "Currently facing down";

		// Token: 0x0400AF87 RID: 44935
		public static LocString BUILDTOOL_ROTATE_CURRENT_UPRIGHT = "Currently upright";

		// Token: 0x0400AF88 RID: 44936
		public static LocString BUILDTOOL_ROTATE_CURRENT_ON_SIDE = "Currently on its side";

		// Token: 0x0400AF89 RID: 44937
		public static LocString BUILDTOOL_CANT_ROTATE = "This building cannot be rotated";

		// Token: 0x0400AF8A RID: 44938
		public static LocString EQUIPMENTTAB_OWNED = "Owned Items";

		// Token: 0x0400AF8B RID: 44939
		public static LocString EQUIPMENTTAB_HELD = "Held Items";

		// Token: 0x0400AF8C RID: 44940
		public static LocString EQUIPMENTTAB_ROOM = "Assigned Rooms";

		// Token: 0x0400AF8D RID: 44941
		public static LocString JOBSCREEN_PRIORITY = "Priority";

		// Token: 0x0400AF8E RID: 44942
		public static LocString JOBSCREEN_HIGH = "High";

		// Token: 0x0400AF8F RID: 44943
		public static LocString JOBSCREEN_LOW = "Low";

		// Token: 0x0400AF90 RID: 44944
		public static LocString JOBSCREEN_EVERYONE = "Everyone";

		// Token: 0x0400AF91 RID: 44945
		public static LocString JOBSCREEN_DEFAULT = "New Duplicants";

		// Token: 0x0400AF92 RID: 44946
		public static LocString BUILD_REQUIRES_SKILL = "Skill: {Skill}";

		// Token: 0x0400AF93 RID: 44947
		public static LocString BUILD_REQUIRES_SKILL_TOOLTIP = "At least one Duplicant must have the {Skill} Skill to construct this building";

		// Token: 0x0400AF94 RID: 44948
		public static LocString VITALSSCREEN_NAME = "Name";

		// Token: 0x0400AF95 RID: 44949
		public static LocString VITALSSCREEN_STRESS = "Stress";

		// Token: 0x0400AF96 RID: 44950
		public static LocString VITALSSCREEN_HEALTH = "Health";

		// Token: 0x0400AF97 RID: 44951
		public static LocString VITALSSCREEN_SICKNESS = "Disease";

		// Token: 0x0400AF98 RID: 44952
		public static LocString VITALSSCREEN_CALORIES = "Fullness";

		// Token: 0x0400AF99 RID: 44953
		public static LocString VITALSSCREEN_RATIONS = "Calories / Cycle";

		// Token: 0x0400AF9A RID: 44954
		public static LocString VITALSSCREEN_EATENTODAY = "Eaten Today";

		// Token: 0x0400AF9B RID: 44955
		public static LocString VITALSSCREEN_RATIONS_TOOLTIP = "Set how many calories this Duplicant may consume daily";

		// Token: 0x0400AF9C RID: 44956
		public static LocString VITALSSCREEN_EATENTODAY_TOOLTIP = "The amount of food this Duplicant has eaten this cycle";

		// Token: 0x0400AF9D RID: 44957
		public static LocString VITALSSCREEN_UNTIL_FULL = "Until Full";

		// Token: 0x0400AF9E RID: 44958
		public static LocString RESEARCHSCREEN_UNLOCKSTOOLTIP = "Unlocks: {0}";

		// Token: 0x0400AF9F RID: 44959
		public static LocString RESEARCHSCREEN_FILTER = "Search Tech";

		// Token: 0x0400AFA0 RID: 44960
		public static LocString ATTRIBUTELEVEL = "Expertise: Level {0} {1}";

		// Token: 0x0400AFA1 RID: 44961
		public static LocString ATTRIBUTELEVEL_SHORT = "Level {0} {1}";

		// Token: 0x0400AFA2 RID: 44962
		public static LocString NEUTRONIUMMASS = "Immeasurable";

		// Token: 0x0400AFA3 RID: 44963
		public static LocString CALCULATING = "Calculating...";

		// Token: 0x0400AFA4 RID: 44964
		public static LocString FORMATDAY = "{0} cycles";

		// Token: 0x0400AFA5 RID: 44965
		public static LocString FORMATSECONDS = "{0}s";

		// Token: 0x0400AFA6 RID: 44966
		public static LocString DELIVERED = "Delivered: {0} {1}";

		// Token: 0x0400AFA7 RID: 44967
		public static LocString PICKEDUP = "Picked Up: {0} {1}";

		// Token: 0x0400AFA8 RID: 44968
		public static LocString COPIED_SETTINGS = "Settings Applied";

		// Token: 0x0400AFA9 RID: 44969
		public static LocString WELCOMEMESSAGETITLE = "- ALERT -";

		// Token: 0x0400AFAA RID: 44970
		public static LocString WELCOMEMESSAGEBODY = "I've awoken at the target location, but colonization efforts have already hit a hitch. I was supposed to land on the planet's surface, but became trapped many miles underground instead.\n\nAlthough the conditions are not ideal, it's imperative that I establish a colony here and begin mounting efforts to escape.";

		// Token: 0x0400AFAB RID: 44971
		public static LocString WELCOMEMESSAGEBODY_SPACEDOUT = "The asteroid we call home has collided with an anomalous planet, decimating our colony. Rebuilding it is of the utmost importance.\n\nI've detected a new cluster of material-rich planetoids in nearby space. If I can guide the Duplicants through the perils of space travel, we could build a colony even bigger and better than before.";

		// Token: 0x0400AFAC RID: 44972
		public static LocString WELCOMEMESSAGEBODY_KF23 = "This asteroid is oddly tilted, as though a powerful external force once knocked it off its axis.\n\nI'll need to recalibrate my approach to colony-building in order to make the most of this unusual distribution of resources.";

		// Token: 0x0400AFAD RID: 44973
		public static LocString WELCOMEMESSAGEBODY_DLC2_CERES = "The ambient temperatures of this planet are inhospitably low.\n\nI've detected the ruins of a scientifically advanced settlement buried deep beneath our landing site.\n\nIf my Duplicants can survive the journey into this frosty planet's core, we could use this newfound technology to build a colony like no other.";

		// Token: 0x0400AFAE RID: 44974
		public static LocString WELCOMEMESSAGEBEGIN = "BEGIN";

		// Token: 0x0400AFAF RID: 44975
		public static LocString VIEWDUPLICANTS = "Choose a Blueprint";

		// Token: 0x0400AFB0 RID: 44976
		public static LocString DUPLICANTPRINTING = "Duplicant Printing";

		// Token: 0x0400AFB1 RID: 44977
		public static LocString ASSIGNDUPLICANT = "Assign Duplicant";

		// Token: 0x0400AFB2 RID: 44978
		public static LocString CRAFT = "ADD TO QUEUE";

		// Token: 0x0400AFB3 RID: 44979
		public static LocString CLEAR_COMPLETED = "CLEAR COMPLETED ORDERS";

		// Token: 0x0400AFB4 RID: 44980
		public static LocString CRAFT_CONTINUOUS = "CONTINUOUS";

		// Token: 0x0400AFB5 RID: 44981
		public static LocString INCUBATE_CONTINUOUS_TOOLTIP = "When checked, this building will continuously incubate eggs of the selected type";

		// Token: 0x0400AFB6 RID: 44982
		public static LocString PLACEINRECEPTACLE = "Plant";

		// Token: 0x0400AFB7 RID: 44983
		public static LocString REMOVEFROMRECEPTACLE = "Uproot";

		// Token: 0x0400AFB8 RID: 44984
		public static LocString CANCELPLACEINRECEPTACLE = "Cancel";

		// Token: 0x0400AFB9 RID: 44985
		public static LocString CANCELREMOVALFROMRECEPTACLE = "Cancel";

		// Token: 0x0400AFBA RID: 44986
		public static LocString CHANGEPERSECOND = "Change per second: {0}";

		// Token: 0x0400AFBB RID: 44987
		public static LocString CHANGEPERCYCLE = "Total change per cycle: {0}";

		// Token: 0x0400AFBC RID: 44988
		public static LocString MODIFIER_ITEM_TEMPLATE = "    • {0}: {1}";

		// Token: 0x0400AFBD RID: 44989
		public static LocString LISTENTRYSTRING = "     {0}\n";

		// Token: 0x0400AFBE RID: 44990
		public static LocString LISTENTRYSTRINGNOLINEBREAK = "     {0}";

		// Token: 0x02002A42 RID: 10818
		public static class PLATFORMS
		{
			// Token: 0x0400AFBF RID: 44991
			public static LocString UNKNOWN = "Your game client";

			// Token: 0x0400AFC0 RID: 44992
			public static LocString STEAM = "Steam";

			// Token: 0x0400AFC1 RID: 44993
			public static LocString EPIC = "Epic Games Store";

			// Token: 0x0400AFC2 RID: 44994
			public static LocString WEGAME = "Wegame";
		}

		// Token: 0x02002A43 RID: 10819
		private enum KeywordType
		{
			// Token: 0x0400AFC4 RID: 44996
			Hotkey,
			// Token: 0x0400AFC5 RID: 44997
			BuildMenu,
			// Token: 0x0400AFC6 RID: 44998
			Attribute,
			// Token: 0x0400AFC7 RID: 44999
			Generic
		}

		// Token: 0x02002A44 RID: 10820
		public enum ClickType
		{
			// Token: 0x0400AFC9 RID: 45001
			Click,
			// Token: 0x0400AFCA RID: 45002
			Clicked,
			// Token: 0x0400AFCB RID: 45003
			Clicking,
			// Token: 0x0400AFCC RID: 45004
			Clickable,
			// Token: 0x0400AFCD RID: 45005
			Clicks,
			// Token: 0x0400AFCE RID: 45006
			click,
			// Token: 0x0400AFCF RID: 45007
			clicked,
			// Token: 0x0400AFD0 RID: 45008
			clicking,
			// Token: 0x0400AFD1 RID: 45009
			clickable,
			// Token: 0x0400AFD2 RID: 45010
			clicks,
			// Token: 0x0400AFD3 RID: 45011
			CLICK,
			// Token: 0x0400AFD4 RID: 45012
			CLICKED,
			// Token: 0x0400AFD5 RID: 45013
			CLICKING,
			// Token: 0x0400AFD6 RID: 45014
			CLICKABLE,
			// Token: 0x0400AFD7 RID: 45015
			CLICKS
		}

		// Token: 0x02002A45 RID: 10821
		public enum AutomationState
		{
			// Token: 0x0400AFD9 RID: 45017
			Active,
			// Token: 0x0400AFDA RID: 45018
			Standby
		}

		// Token: 0x02002A46 RID: 10822
		public class VANILLA
		{
			// Token: 0x0400AFDB RID: 45019
			public static LocString NAME = "Base Game";

			// Token: 0x0400AFDC RID: 45020
			public static LocString NAME_ITAL = "<i>" + UI.VANILLA.NAME + "</i>";
		}

		// Token: 0x02002A47 RID: 10823
		public class DLC1
		{
			// Token: 0x0400AFDD RID: 45021
			public static LocString NAME = "Spaced Out!";

			// Token: 0x0400AFDE RID: 45022
			public static LocString NAME_ITAL = "<i>" + UI.DLC1.NAME + "</i>";
		}

		// Token: 0x02002A48 RID: 10824
		public class DLC2
		{
			// Token: 0x0400AFDF RID: 45023
			public static LocString NAME = "The Frosty Planet Pack";

			// Token: 0x0400AFE0 RID: 45024
			public static LocString NAME_ITAL = "<i>" + UI.DLC2.NAME + "</i>";

			// Token: 0x0400AFE1 RID: 45025
			public static LocString MIXING_TOOLTIP = "<b><i>The Frosty Planet Pack</i></b> features frozen biomes and elements useful in thermal regulation";
		}

		// Token: 0x02002A49 RID: 10825
		public class DLC3
		{
			// Token: 0x0400AFE2 RID: 45026
			public static LocString NAME = "SUPER-SECRET DLC3 NAME";

			// Token: 0x0400AFE3 RID: 45027
			public static LocString NAME_ITAL = "<i>" + UI.DLC3.NAME + "</i>";

			// Token: 0x0400AFE4 RID: 45028
			public static LocString MIXING_TOOLTIP = "";
		}

		// Token: 0x02002A4A RID: 10826
		public class DIAGNOSTICS_SCREEN
		{
			// Token: 0x0400AFE5 RID: 45029
			public static LocString TITLE = "Diagnostics";

			// Token: 0x0400AFE6 RID: 45030
			public static LocString DIAGNOSTIC = "Diagnostic";

			// Token: 0x0400AFE7 RID: 45031
			public static LocString TOTAL = "Total";

			// Token: 0x0400AFE8 RID: 45032
			public static LocString RESERVED = "Reserved";

			// Token: 0x0400AFE9 RID: 45033
			public static LocString STATUS = "Status";

			// Token: 0x0400AFEA RID: 45034
			public static LocString SEARCH = "Search";

			// Token: 0x0400AFEB RID: 45035
			public static LocString CRITERIA_HEADER_TOOLTIP = "Expand or collapse diagnostic criteria panel";

			// Token: 0x0400AFEC RID: 45036
			public static LocString SEE_ALL = "+ See All ({0})";

			// Token: 0x0400AFED RID: 45037
			public static LocString CRITERIA_TOOLTIP = "Toggle the <b>{0}</b> diagnostics evaluation of the <b>{1}</b> criteria";

			// Token: 0x0400AFEE RID: 45038
			public static LocString CRITERIA_ENABLED_COUNT = "{0}/{1} criteria enabled";

			// Token: 0x02002A4B RID: 10827
			public class CLICK_TOGGLE_MESSAGE
			{
				// Token: 0x0400AFEF RID: 45039
				public static LocString ALWAYS = UI.CLICK(UI.ClickType.Click) + " to pin this diagnostic to the sidebar - Current State: <b>Visible On Alert Only</b>";

				// Token: 0x0400AFF0 RID: 45040
				public static LocString ALERT_ONLY = UI.CLICK(UI.ClickType.Click) + " to subscribe to this diagnostic - Current State: <b>Never Visible</b>";

				// Token: 0x0400AFF1 RID: 45041
				public static LocString NEVER = UI.CLICK(UI.ClickType.Click) + " to mute this diagnostic on the sidebar - Current State: <b>Always Visible</b>";

				// Token: 0x0400AFF2 RID: 45042
				public static LocString TUTORIAL_DISABLED = UI.CLICK(UI.ClickType.Click) + " to enable this diagnostic -  Current State: <b>Temporarily disabled</b>";
			}
		}

		// Token: 0x02002A4C RID: 10828
		public class WORLD_SELECTOR_SCREEN
		{
			// Token: 0x0400AFF3 RID: 45043
			public static LocString TITLE = UI.CLUSTERMAP.PLANETOID;
		}

		// Token: 0x02002A4D RID: 10829
		public class COLONY_DIAGNOSTICS
		{
			// Token: 0x0400AFF4 RID: 45044
			public static LocString NO_MINIONS_PLANETOID = "    • There are no Duplicants on this planetoid";

			// Token: 0x0400AFF5 RID: 45045
			public static LocString NO_MINIONS_ROCKET = "    • There are no Duplicants aboard this rocket";

			// Token: 0x0400AFF6 RID: 45046
			public static LocString ROCKET = "rocket";

			// Token: 0x0400AFF7 RID: 45047
			public static LocString NO_MINIONS_REQUESTED = "    • Crew must be requested to update this diagnostic";

			// Token: 0x0400AFF8 RID: 45048
			public static LocString NO_DATA = "    • Not enough data for evaluation";

			// Token: 0x0400AFF9 RID: 45049
			public static LocString NO_DATA_SHORT = "    • No data";

			// Token: 0x0400AFFA RID: 45050
			public static LocString MUTE_TUTORIAL = "Diagnostic can be muted in the <b><color=#E5B000>See All</color></b> panel";

			// Token: 0x0400AFFB RID: 45051
			public static LocString GENERIC_STATUS_NORMAL = "All values nominal";

			// Token: 0x0400AFFC RID: 45052
			public static LocString PLACEHOLDER_CRITERIA_NAME = "Placeholder Criteria Name";

			// Token: 0x0400AFFD RID: 45053
			public static LocString GENERIC_CRITERIA_PASS = "Criteria met";

			// Token: 0x0400AFFE RID: 45054
			public static LocString GENERIC_CRITERIA_FAIL = "Criteria not met";

			// Token: 0x02002A4E RID: 10830
			public class GENERIC_CRITERIA
			{
				// Token: 0x0400AFFF RID: 45055
				public static LocString CHECKWORLDHASMINIONS = "Check world has Duplicants";
			}

			// Token: 0x02002A4F RID: 10831
			public class IDLEDIAGNOSTIC
			{
				// Token: 0x0400B000 RID: 45056
				public static LocString ALL_NAME = "Idleness";

				// Token: 0x0400B001 RID: 45057
				public static LocString TOOLTIP_NAME = "<b>Idleness</b>";

				// Token: 0x0400B002 RID: 45058
				public static LocString NORMAL = "    • All Duplicants currently have tasks";

				// Token: 0x0400B003 RID: 45059
				public static LocString IDLE = "    • One or more Duplicants are idle";

				// Token: 0x02002A50 RID: 10832
				public static class CRITERIA
				{
					// Token: 0x0400B004 RID: 45060
					public static LocString CHECKIDLE = "Check idle";

					// Token: 0x0400B005 RID: 45061
					public static LocString CHECKIDLESEVERE = "Use high severity idle warning";
				}
			}

			// Token: 0x02002A51 RID: 10833
			public class CHOREGROUPDIAGNOSTIC
			{
				// Token: 0x0400B006 RID: 45062
				public static LocString ALL_NAME = UI.COLONY_DIAGNOSTICS.ALLCHORESDIAGNOSTIC.ALL_NAME;

				// Token: 0x02002A52 RID: 10834
				public static class CRITERIA
				{
				}
			}

			// Token: 0x02002A53 RID: 10835
			public class ALLCHORESDIAGNOSTIC
			{
				// Token: 0x0400B007 RID: 45063
				public static LocString ALL_NAME = "Errands";

				// Token: 0x0400B008 RID: 45064
				public static LocString TOOLTIP_NAME = "<b>Errands</b>";

				// Token: 0x0400B009 RID: 45065
				public static LocString NORMAL = "    • {0} errands pending or in progress";

				// Token: 0x02002A54 RID: 10836
				public static class CRITERIA
				{
				}
			}

			// Token: 0x02002A55 RID: 10837
			public class WORKTIMEDIAGNOSTIC
			{
				// Token: 0x0400B00A RID: 45066
				public static LocString ALL_NAME = UI.COLONY_DIAGNOSTICS.ALLCHORESDIAGNOSTIC.ALL_NAME;

				// Token: 0x02002A56 RID: 10838
				public static class CRITERIA
				{
				}
			}

			// Token: 0x02002A57 RID: 10839
			public class ALLWORKTIMEDIAGNOSTIC
			{
				// Token: 0x0400B00B RID: 45067
				public static LocString ALL_NAME = "Work Time";

				// Token: 0x0400B00C RID: 45068
				public static LocString TOOLTIP_NAME = "<b>Work Time</b>";

				// Token: 0x0400B00D RID: 45069
				public static LocString NORMAL = "    • {0} of Duplicant time spent working";

				// Token: 0x02002A58 RID: 10840
				public static class CRITERIA
				{
				}
			}

			// Token: 0x02002A59 RID: 10841
			public class TRAVEL_TIME
			{
				// Token: 0x0400B00E RID: 45070
				public static LocString ALL_NAME = "Travel Time";

				// Token: 0x0400B00F RID: 45071
				public static LocString TOOLTIP_NAME = "<b>Travel Time</b>";

				// Token: 0x0400B010 RID: 45072
				public static LocString NORMAL = "    • {0} of Duplicant time spent traveling between errands";

				// Token: 0x02002A5A RID: 10842
				public static class CRITERIA
				{
				}
			}

			// Token: 0x02002A5B RID: 10843
			public class TRAPPEDDUPLICANTDIAGNOSTIC
			{
				// Token: 0x0400B011 RID: 45073
				public static LocString ALL_NAME = "Trapped";

				// Token: 0x0400B012 RID: 45074
				public static LocString TOOLTIP_NAME = "<b>Trapped</b>";

				// Token: 0x0400B013 RID: 45075
				public static LocString NORMAL = "    • No Duplicants are trapped";

				// Token: 0x0400B014 RID: 45076
				public static LocString STUCK = "    • One or more Duplicants are trapped";

				// Token: 0x02002A5C RID: 10844
				public static class CRITERIA
				{
					// Token: 0x0400B015 RID: 45077
					public static LocString CHECKTRAPPED = "Check Trapped";
				}
			}

			// Token: 0x02002A5D RID: 10845
			public class FLOODEDDIAGNOSTIC
			{
				// Token: 0x0400B016 RID: 45078
				public static LocString ALL_NAME = "Flooded";

				// Token: 0x0400B017 RID: 45079
				public static LocString TOOLTIP_NAME = "<b>Flooded</b>";

				// Token: 0x0400B018 RID: 45080
				public static LocString NORMAL = "    • No buildings are flooded";

				// Token: 0x0400B019 RID: 45081
				public static LocString BUILDING_FLOODED = "    • One or more buildings are flooded";

				// Token: 0x02002A5E RID: 10846
				public static class CRITERIA
				{
					// Token: 0x0400B01A RID: 45082
					public static LocString CHECKFLOODED = "Check Flooded";
				}
			}

			// Token: 0x02002A5F RID: 10847
			public class BREATHABILITYDIAGNOSTIC
			{
				// Token: 0x0400B01B RID: 45083
				public static LocString ALL_NAME = "Breathability";

				// Token: 0x0400B01C RID: 45084
				public static LocString TOOLTIP_NAME = "<b>Breathability</b>";

				// Token: 0x0400B01D RID: 45085
				public static LocString NORMAL = "    • Oxygen levels are satisfactory";

				// Token: 0x0400B01E RID: 45086
				public static LocString POOR = "    • Oxygen is becoming scarce or low pressure";

				// Token: 0x0400B01F RID: 45087
				public static LocString SUFFOCATING = "    • One or more Duplicants are suffocating";

				// Token: 0x02002A60 RID: 10848
				public static class CRITERIA
				{
					// Token: 0x0400B020 RID: 45088
					public static LocString CHECKSUFFOCATION = "Check suffocation";

					// Token: 0x0400B021 RID: 45089
					public static LocString CHECKLOWBREATHABILITY = "Check low breathability";
				}
			}

			// Token: 0x02002A61 RID: 10849
			public class STRESSDIAGNOSTIC
			{
				// Token: 0x0400B022 RID: 45090
				public static LocString ALL_NAME = "Max Stress";

				// Token: 0x0400B023 RID: 45091
				public static LocString TOOLTIP_NAME = "<b>Max Stress</b>";

				// Token: 0x0400B024 RID: 45092
				public static LocString HIGH_STRESS = "    • One or more Duplicants is suffering high stress";

				// Token: 0x0400B025 RID: 45093
				public static LocString NORMAL = "    • Duplicants have acceptable stress levels";

				// Token: 0x02002A62 RID: 10850
				public static class CRITERIA
				{
					// Token: 0x0400B026 RID: 45094
					public static LocString CHECKSTRESSED = "Check stressed";
				}
			}

			// Token: 0x02002A63 RID: 10851
			public class DECORDIAGNOSTIC
			{
				// Token: 0x0400B027 RID: 45095
				public static LocString ALL_NAME = "Decor";

				// Token: 0x0400B028 RID: 45096
				public static LocString TOOLTIP_NAME = "<b>Decor</b>";

				// Token: 0x0400B029 RID: 45097
				public static LocString LOW = "    • Decor levels are low";

				// Token: 0x0400B02A RID: 45098
				public static LocString NORMAL = "    • Decor levels are satisfactory";

				// Token: 0x02002A64 RID: 10852
				public static class CRITERIA
				{
					// Token: 0x0400B02B RID: 45099
					public static LocString CHECKDECOR = "Check decor";
				}
			}

			// Token: 0x02002A65 RID: 10853
			public class TOILETDIAGNOSTIC
			{
				// Token: 0x0400B02C RID: 45100
				public static LocString ALL_NAME = "Toilets";

				// Token: 0x0400B02D RID: 45101
				public static LocString TOOLTIP_NAME = "<b>Toilets</b>";

				// Token: 0x0400B02E RID: 45102
				public static LocString NO_TOILETS = "    • Colony has no toilets";

				// Token: 0x0400B02F RID: 45103
				public static LocString NO_WORKING_TOILETS = "    • Colony has no working toilets";

				// Token: 0x0400B030 RID: 45104
				public static LocString TOILET_URGENT = "    • Duplicants urgently need to use a toilet";

				// Token: 0x0400B031 RID: 45105
				public static LocString FEW_TOILETS = "    • Toilet-to-Duplicant ratio is low";

				// Token: 0x0400B032 RID: 45106
				public static LocString INOPERATIONAL = "    • One or more toilets are out of order";

				// Token: 0x0400B033 RID: 45107
				public static LocString NORMAL = "    • Colony has adequate working toilets";

				// Token: 0x0400B034 RID: 45108
				public static LocString NO_MINIONS_PLANETOID = "    • There are no Duplicants with a bladder on this planetoid";

				// Token: 0x0400B035 RID: 45109
				public static LocString NO_MINIONS_ROCKET = "    • There are no Duplicants with a bladder aboard this rocket";

				// Token: 0x02002A66 RID: 10854
				public static class CRITERIA
				{
					// Token: 0x0400B036 RID: 45110
					public static LocString CHECKHASANYTOILETS = "Check has any toilets";

					// Token: 0x0400B037 RID: 45111
					public static LocString CHECKENOUGHTOILETS = "Check enough toilets";

					// Token: 0x0400B038 RID: 45112
					public static LocString CHECKBLADDERS = "Check Duplicants really need to use the toilet";
				}
			}

			// Token: 0x02002A67 RID: 10855
			public class BEDDIAGNOSTIC
			{
				// Token: 0x0400B039 RID: 45113
				public static LocString ALL_NAME = "Beds";

				// Token: 0x0400B03A RID: 45114
				public static LocString TOOLTIP_NAME = "<b>Beds</b>";

				// Token: 0x0400B03B RID: 45115
				public static LocString NORMAL = "    • Colony has adequate bedding";

				// Token: 0x0400B03C RID: 45116
				public static LocString NOT_ENOUGH_BEDS = "    • One or more Duplicants are missing a bed";

				// Token: 0x0400B03D RID: 45117
				public static LocString MISSING_ASSIGNMENT = "    • One or more Duplicants don't have an assigned bed";

				// Token: 0x0400B03E RID: 45118
				public static LocString CANT_REACH = "    • One or more Duplicants can't reach their bed";

				// Token: 0x0400B03F RID: 45119
				public static LocString NO_MINIONS_PLANETOID = "    • There are no Duplicants on this planetoid who need sleep";

				// Token: 0x0400B040 RID: 45120
				public static LocString NO_MINIONS_ROCKET = "    • There are no Duplicants aboard this rocket who need sleep";

				// Token: 0x02002A68 RID: 10856
				public static class CRITERIA
				{
					// Token: 0x0400B041 RID: 45121
					public static LocString CHECKENOUGHBEDS = "Check enough beds";

					// Token: 0x0400B042 RID: 45122
					public static LocString CHECKREACHABILITY = "Check beds are reachable";
				}
			}

			// Token: 0x02002A69 RID: 10857
			public class FOODDIAGNOSTIC
			{
				// Token: 0x0400B043 RID: 45123
				public static LocString ALL_NAME = "Food";

				// Token: 0x0400B044 RID: 45124
				public static LocString TOOLTIP_NAME = "<b>Food</b>";

				// Token: 0x0400B045 RID: 45125
				public static LocString NORMAL = "    • Food supply is currently adequate";

				// Token: 0x0400B046 RID: 45126
				public static LocString LOW_CALORIES = "    • Food-to-Duplicant ratio is low";

				// Token: 0x0400B047 RID: 45127
				public static LocString HUNGRY = "    • One or more Duplicants are very hungry";

				// Token: 0x0400B048 RID: 45128
				public static LocString NO_FOOD = "    • Duplicants have no food";

				// Token: 0x02002A6A RID: 10858
				public class CRITERIA_HAS_FOOD
				{
					// Token: 0x0400B049 RID: 45129
					public static LocString PASS = "    • Duplicants have food";

					// Token: 0x0400B04A RID: 45130
					public static LocString FAIL = "    • Duplicants have no food";
				}

				// Token: 0x02002A6B RID: 10859
				public static class CRITERIA
				{
					// Token: 0x0400B04B RID: 45131
					public static LocString CHECKENOUGHFOOD = "Check enough food";

					// Token: 0x0400B04C RID: 45132
					public static LocString CHECKSTARVATION = "Check starvation";
				}
			}

			// Token: 0x02002A6C RID: 10860
			public class FARMDIAGNOSTIC
			{
				// Token: 0x0400B04D RID: 45133
				public static LocString ALL_NAME = "Crops";

				// Token: 0x0400B04E RID: 45134
				public static LocString TOOLTIP_NAME = "<b>Crops</b>";

				// Token: 0x0400B04F RID: 45135
				public static LocString NORMAL = "    • Crops are being grown in sufficient quantity";

				// Token: 0x0400B050 RID: 45136
				public static LocString NONE = "    • No farm plots";

				// Token: 0x0400B051 RID: 45137
				public static LocString NONE_PLANTED = "    • No crops planted";

				// Token: 0x0400B052 RID: 45138
				public static LocString WILTING = "    • One or more crops are wilting";

				// Token: 0x0400B053 RID: 45139
				public static LocString INOPERATIONAL = "    • One or more farm plots are inoperable";

				// Token: 0x02002A6D RID: 10861
				public static class CRITERIA
				{
					// Token: 0x0400B054 RID: 45140
					public static LocString CHECKHASFARMS = "Check colony has farms";

					// Token: 0x0400B055 RID: 45141
					public static LocString CHECKPLANTED = "Check farms are planted";

					// Token: 0x0400B056 RID: 45142
					public static LocString CHECKWILTING = "Check crops wilting";

					// Token: 0x0400B057 RID: 45143
					public static LocString CHECKOPERATIONAL = "Check farm plots operational";
				}
			}

			// Token: 0x02002A6E RID: 10862
			public class POWERUSEDIAGNOSTIC
			{
				// Token: 0x0400B058 RID: 45144
				public static LocString ALL_NAME = "Power use";

				// Token: 0x0400B059 RID: 45145
				public static LocString TOOLTIP_NAME = "<b>Power use</b>";

				// Token: 0x0400B05A RID: 45146
				public static LocString NORMAL = "    • Power supply is satisfactory";

				// Token: 0x0400B05B RID: 45147
				public static LocString OVERLOADED = "    • One or more power grids are damaged";

				// Token: 0x0400B05C RID: 45148
				public static LocString SIGNIFICANT_POWER_CHANGE_DETECTED = "Significant power use change detected. (Average:{0}, Current:{1})";

				// Token: 0x0400B05D RID: 45149
				public static LocString CIRCUIT_OVER_CAPACITY = "Circuit overloaded {0}/{1}";

				// Token: 0x02002A6F RID: 10863
				public static class CRITERIA
				{
					// Token: 0x0400B05E RID: 45150
					public static LocString CHECKOVERWATTAGE = "Check circuit overloaded";

					// Token: 0x0400B05F RID: 45151
					public static LocString CHECKPOWERUSECHANGE = "Check power use change";
				}
			}

			// Token: 0x02002A70 RID: 10864
			public class HEATDIAGNOSTIC
			{
				// Token: 0x0400B060 RID: 45152
				public static LocString ALL_NAME = UI.COLONY_DIAGNOSTICS.BATTERYDIAGNOSTIC.ALL_NAME;

				// Token: 0x02002A71 RID: 10865
				public static class CRITERIA
				{
					// Token: 0x0400B061 RID: 45153
					public static LocString CHECKHEAT = "Check heat";
				}
			}

			// Token: 0x02002A72 RID: 10866
			public class BATTERYDIAGNOSTIC
			{
				// Token: 0x0400B062 RID: 45154
				public static LocString ALL_NAME = "Battery";

				// Token: 0x0400B063 RID: 45155
				public static LocString TOOLTIP_NAME = "<b>Battery</b>";

				// Token: 0x0400B064 RID: 45156
				public static LocString NORMAL = "    • All batteries functional";

				// Token: 0x0400B065 RID: 45157
				public static LocString NONE = "    • No batteries are connected to a power grid";

				// Token: 0x0400B066 RID: 45158
				public static LocString DEAD_BATTERY = "    • One or more batteries have died";

				// Token: 0x0400B067 RID: 45159
				public static LocString LIMITED_CAPACITY = "    • Low battery capacity relative to power use";

				// Token: 0x02002A73 RID: 10867
				public class CRITERIA_CHECK_CAPACITY
				{
					// Token: 0x0400B068 RID: 45160
					public static LocString PASS = "";

					// Token: 0x0400B069 RID: 45161
					public static LocString FAIL = "";
				}

				// Token: 0x02002A74 RID: 10868
				public static class CRITERIA
				{
					// Token: 0x0400B06A RID: 45162
					public static LocString CHECKCAPACITY = "Check capacity";

					// Token: 0x0400B06B RID: 45163
					public static LocString CHECKDEAD = "Check dead";
				}
			}

			// Token: 0x02002A75 RID: 10869
			public class RADIATIONDIAGNOSTIC
			{
				// Token: 0x0400B06C RID: 45164
				public static LocString ALL_NAME = "Radiation";

				// Token: 0x0400B06D RID: 45165
				public static LocString TOOLTIP_NAME = "<b>Radiation</b>";

				// Token: 0x0400B06E RID: 45166
				public static LocString NORMAL = "    • No Radiation concerns";

				// Token: 0x0400B06F RID: 45167
				public static LocString AVERAGE_RADS = "Avg. {0}";

				// Token: 0x02002A76 RID: 10870
				public class CRITERIA_RADIATION_SICKNESS
				{
					// Token: 0x0400B070 RID: 45168
					public static LocString PASS = "Healthy";

					// Token: 0x0400B071 RID: 45169
					public static LocString FAIL = "Sick";
				}

				// Token: 0x02002A77 RID: 10871
				public class CRITERIA_RADIATION_EXPOSURE
				{
					// Token: 0x0400B072 RID: 45170
					public static LocString PASS = "Safe exposure levels";

					// Token: 0x0400B073 RID: 45171
					public static LocString FAIL_CONCERN = "Exposure levels are above safe limits for one or more Duplicants";

					// Token: 0x0400B074 RID: 45172
					public static LocString FAIL_WARNING = "One or more Duplicants are being exposed to extreme levels of radiation";
				}

				// Token: 0x02002A78 RID: 10872
				public static class CRITERIA
				{
					// Token: 0x0400B075 RID: 45173
					public static LocString CHECKSICK = "Check sick";

					// Token: 0x0400B076 RID: 45174
					public static LocString CHECKEXPOSED = "Check exposed";
				}
			}

			// Token: 0x02002A79 RID: 10873
			public class METEORDIAGNOSTIC
			{
				// Token: 0x0400B077 RID: 45175
				public static LocString ALL_NAME = "Meteor Showers";

				// Token: 0x0400B078 RID: 45176
				public static LocString TOOLTIP_NAME = "<b>Meteor Showers</b>";

				// Token: 0x0400B079 RID: 45177
				public static LocString NORMAL = "    • No meteor showers in progress";

				// Token: 0x0400B07A RID: 45178
				public static LocString SHOWER_UNDERWAY = "    • Meteor bombardment underway! {0} remaining";

				// Token: 0x02002A7A RID: 10874
				public static class CRITERIA
				{
					// Token: 0x0400B07B RID: 45179
					public static LocString CHECKUNDERWAY = "Check meteor bombardment";
				}
			}

			// Token: 0x02002A7B RID: 10875
			public class ENTOMBEDDIAGNOSTIC
			{
				// Token: 0x0400B07C RID: 45180
				public static LocString ALL_NAME = "Entombed";

				// Token: 0x0400B07D RID: 45181
				public static LocString TOOLTIP_NAME = "<b>Entombed</b>";

				// Token: 0x0400B07E RID: 45182
				public static LocString NORMAL = "    • No buildings are entombed";

				// Token: 0x0400B07F RID: 45183
				public static LocString BUILDING_ENTOMBED = "    • One or more buildings are entombed";

				// Token: 0x02002A7C RID: 10876
				public static class CRITERIA
				{
					// Token: 0x0400B080 RID: 45184
					public static LocString CHECKENTOMBED = "Check entombed";
				}
			}

			// Token: 0x02002A7D RID: 10877
			public class ROCKETFUELDIAGNOSTIC
			{
				// Token: 0x0400B081 RID: 45185
				public static LocString ALL_NAME = "Rocket Fuel";

				// Token: 0x0400B082 RID: 45186
				public static LocString TOOLTIP_NAME = "<b>Rocket Fuel</b>";

				// Token: 0x0400B083 RID: 45187
				public static LocString NORMAL = "    • This rocket has sufficient fuel";

				// Token: 0x0400B084 RID: 45188
				public static LocString WARNING = "    • This rocket has no fuel";

				// Token: 0x02002A7E RID: 10878
				public static class CRITERIA
				{
				}
			}

			// Token: 0x02002A7F RID: 10879
			public class ROCKETOXIDIZERDIAGNOSTIC
			{
				// Token: 0x0400B085 RID: 45189
				public static LocString ALL_NAME = "Rocket Oxidizer";

				// Token: 0x0400B086 RID: 45190
				public static LocString TOOLTIP_NAME = "<b>Rocket Oxidizer</b>";

				// Token: 0x0400B087 RID: 45191
				public static LocString NORMAL = "    • This rocket has sufficient oxidizer";

				// Token: 0x0400B088 RID: 45192
				public static LocString WARNING = "    • This rocket has insufficient oxidizer";

				// Token: 0x02002A80 RID: 10880
				public static class CRITERIA
				{
				}
			}

			// Token: 0x02002A81 RID: 10881
			public class REACTORDIAGNOSTIC
			{
				// Token: 0x0400B089 RID: 45193
				public static LocString ALL_NAME = BUILDINGS.PREFABS.NUCLEARREACTOR.NAME;

				// Token: 0x0400B08A RID: 45194
				public static LocString TOOLTIP_NAME = BUILDINGS.PREFABS.NUCLEARREACTOR.NAME;

				// Token: 0x0400B08B RID: 45195
				public static LocString NORMAL = "    • Safe";

				// Token: 0x0400B08C RID: 45196
				public static LocString CRITERIA_TEMPERATURE_WARNING = "    • Temperature dangerously high";

				// Token: 0x0400B08D RID: 45197
				public static LocString CRITERIA_COOLANT_WARNING = "    • Coolant tank low";

				// Token: 0x02002A82 RID: 10882
				public static class CRITERIA
				{
					// Token: 0x0400B08E RID: 45198
					public static LocString CHECKTEMPERATURE = "Check temperature";

					// Token: 0x0400B08F RID: 45199
					public static LocString CHECKCOOLANT = "Check coolant";
				}
			}

			// Token: 0x02002A83 RID: 10883
			public class FLOATINGROCKETDIAGNOSTIC
			{
				// Token: 0x0400B090 RID: 45200
				public static LocString ALL_NAME = "Flight Status";

				// Token: 0x0400B091 RID: 45201
				public static LocString TOOLTIP_NAME = "<b>Flight Status</b>";

				// Token: 0x0400B092 RID: 45202
				public static LocString NORMAL_FLIGHT = "    • This rocket is in flight towards its destination";

				// Token: 0x0400B093 RID: 45203
				public static LocString NORMAL_UTILITY = "    • This rocket is performing a task at its destination";

				// Token: 0x0400B094 RID: 45204
				public static LocString NORMAL_LANDED = "    • This rocket is currently landed on a " + UI.PRE_KEYWORD + "Rocket Platform" + UI.PST_KEYWORD;

				// Token: 0x0400B095 RID: 45205
				public static LocString WARNING_NO_DESTINATION = "    • This rocket is suspended in space with no set destination";

				// Token: 0x0400B096 RID: 45206
				public static LocString WARNING_NO_SPEED = "    • This rocket's flight has been halted";

				// Token: 0x02002A84 RID: 10884
				public static class CRITERIA
				{
				}
			}

			// Token: 0x02002A85 RID: 10885
			public class ROCKETINORBITDIAGNOSTIC
			{
				// Token: 0x0400B097 RID: 45207
				public static LocString ALL_NAME = "Rockets in Orbit";

				// Token: 0x0400B098 RID: 45208
				public static LocString TOOLTIP_NAME = "<b>Rockets in Orbit</b>";

				// Token: 0x0400B099 RID: 45209
				public static LocString NORMAL_ONE_IN_ORBIT = "    • {0} is in orbit waiting to land";

				// Token: 0x0400B09A RID: 45210
				public static LocString NORMAL_IN_ORBIT = "    • There are {0} rockets in orbit waiting to land";

				// Token: 0x0400B09B RID: 45211
				public static LocString WARNING_ONE_ROCKETS_STRANDED = "    • No " + BUILDINGS.PREFABS.LAUNCHPAD.NAME + " present. {0} stranded";

				// Token: 0x0400B09C RID: 45212
				public static LocString WARNING_ROCKETS_STRANDED = "    • No " + BUILDINGS.PREFABS.LAUNCHPAD.NAME + " present. {0} rockets stranded";

				// Token: 0x0400B09D RID: 45213
				public static LocString NORMAL_NO_ROCKETS = "    • No rockets waiting to land";

				// Token: 0x02002A86 RID: 10886
				public static class CRITERIA
				{
					// Token: 0x0400B09E RID: 45214
					public static LocString CHECKORBIT = "Check Orbiting Rockets";
				}
			}

			// Token: 0x02002A87 RID: 10887
			public class BIONICBATTERYDIAGNOSTIC
			{
				// Token: 0x0400B09F RID: 45215
				public static LocString ALL_NAME = "Bionic Power";

				// Token: 0x0400B0A0 RID: 45216
				public static LocString TOOLTIP_NAME = "<b>Power Banks</b>";

				// Token: 0x0400B0A1 RID: 45217
				public static LocString NORMAL = "    • Power Bank supply is currently adequate";

				// Token: 0x0400B0A2 RID: 45218
				public static LocString LOW_CALORIES = "    • Power-to-Duplicant ratio is low";

				// Token: 0x0400B0A3 RID: 45219
				public static LocString HUNGRY = "    • One or more Duplicants in desparate need of power banks";

				// Token: 0x0400B0A4 RID: 45220
				public static LocString NO_FOOD = "    • Duplicants have no Power Banks";

				// Token: 0x02002A88 RID: 10888
				public class CRITERIA_BATTERIES
				{
					// Token: 0x0400B0A5 RID: 45221
					public static LocString PASS = "    • Bionics have batteries";

					// Token: 0x0400B0A6 RID: 45222
					public static LocString FAIL = "    • Bionics have no batteries";
				}

				// Token: 0x02002A89 RID: 10889
				public static class CRITERIA
				{
					// Token: 0x0400B0A7 RID: 45223
					public static LocString CHECKENOUGHBATTERIES = "Check enough batteries";

					// Token: 0x0400B0A8 RID: 45224
					public static LocString CHECKPOWERLEVEL = "Check critical power level";
				}
			}
		}

		// Token: 0x02002A8A RID: 10890
		public class TRACKERS
		{
			// Token: 0x0400B0A9 RID: 45225
			public static LocString BREATHABILITY = "Breathability";

			// Token: 0x0400B0AA RID: 45226
			public static LocString FOOD = "Food";

			// Token: 0x0400B0AB RID: 45227
			public static LocString STRESS = "Max Stress";

			// Token: 0x0400B0AC RID: 45228
			public static LocString IDLE = "Idle Duplicants";
		}

		// Token: 0x02002A8B RID: 10891
		public class CONTROLS
		{
			// Token: 0x0400B0AD RID: 45229
			public static LocString PRESS = "Press";

			// Token: 0x0400B0AE RID: 45230
			public static LocString PRESSLOWER = "press";

			// Token: 0x0400B0AF RID: 45231
			public static LocString PRESSUPPER = "PRESS";

			// Token: 0x0400B0B0 RID: 45232
			public static LocString PRESSING = "Pressing";

			// Token: 0x0400B0B1 RID: 45233
			public static LocString PRESSINGLOWER = "pressing";

			// Token: 0x0400B0B2 RID: 45234
			public static LocString PRESSINGUPPER = "PRESSING";

			// Token: 0x0400B0B3 RID: 45235
			public static LocString PRESSED = "Pressed";

			// Token: 0x0400B0B4 RID: 45236
			public static LocString PRESSEDLOWER = "pressed";

			// Token: 0x0400B0B5 RID: 45237
			public static LocString PRESSEDUPPER = "PRESSED";

			// Token: 0x0400B0B6 RID: 45238
			public static LocString PRESSES = "Presses";

			// Token: 0x0400B0B7 RID: 45239
			public static LocString PRESSESLOWER = "presses";

			// Token: 0x0400B0B8 RID: 45240
			public static LocString PRESSESUPPER = "PRESSES";

			// Token: 0x0400B0B9 RID: 45241
			public static LocString PRESSABLE = "Pressable";

			// Token: 0x0400B0BA RID: 45242
			public static LocString PRESSABLELOWER = "pressable";

			// Token: 0x0400B0BB RID: 45243
			public static LocString PRESSABLEUPPER = "PRESSABLE";

			// Token: 0x0400B0BC RID: 45244
			public static LocString CLICK = "Click";

			// Token: 0x0400B0BD RID: 45245
			public static LocString CLICKLOWER = "click";

			// Token: 0x0400B0BE RID: 45246
			public static LocString CLICKUPPER = "CLICK";

			// Token: 0x0400B0BF RID: 45247
			public static LocString CLICKING = "Clicking";

			// Token: 0x0400B0C0 RID: 45248
			public static LocString CLICKINGLOWER = "clicking";

			// Token: 0x0400B0C1 RID: 45249
			public static LocString CLICKINGUPPER = "CLICKING";

			// Token: 0x0400B0C2 RID: 45250
			public static LocString CLICKED = "Clicked";

			// Token: 0x0400B0C3 RID: 45251
			public static LocString CLICKEDLOWER = "clicked";

			// Token: 0x0400B0C4 RID: 45252
			public static LocString CLICKEDUPPER = "CLICKED";

			// Token: 0x0400B0C5 RID: 45253
			public static LocString CLICKS = "Clicks";

			// Token: 0x0400B0C6 RID: 45254
			public static LocString CLICKSLOWER = "clicks";

			// Token: 0x0400B0C7 RID: 45255
			public static LocString CLICKSUPPER = "CLICKS";

			// Token: 0x0400B0C8 RID: 45256
			public static LocString CLICKABLE = "Clickable";

			// Token: 0x0400B0C9 RID: 45257
			public static LocString CLICKABLELOWER = "clickable";

			// Token: 0x0400B0CA RID: 45258
			public static LocString CLICKABLEUPPER = "CLICKABLE";
		}

		// Token: 0x02002A8C RID: 10892
		public class MATH_PICTURES
		{
			// Token: 0x02002A8D RID: 10893
			public class AXIS_LABELS
			{
				// Token: 0x0400B0CB RID: 45259
				public static LocString CYCLES = "Cycles";
			}
		}

		// Token: 0x02002A8E RID: 10894
		public class SPACEDESTINATIONS
		{
			// Token: 0x02002A8F RID: 10895
			public class WORMHOLE
			{
				// Token: 0x0400B0CC RID: 45260
				public static LocString NAME = "Temporal Tear";

				// Token: 0x0400B0CD RID: 45261
				public static LocString DESCRIPTION = "The source of our misfortune, though it may also be our shot at freedom. Traces of Neutronium are detectable in my readings.";
			}

			// Token: 0x02002A90 RID: 10896
			public class RESEARCHDESTINATION
			{
				// Token: 0x0400B0CE RID: 45262
				public static LocString NAME = "Alluring Anomaly";

				// Token: 0x0400B0CF RID: 45263
				public static LocString DESCRIPTION = "Our researchers would have a field day with this if they could only get close enough.";
			}

			// Token: 0x02002A91 RID: 10897
			public class DEBRIS
			{
				// Token: 0x02002A92 RID: 10898
				public class SATELLITE
				{
					// Token: 0x0400B0D0 RID: 45264
					public static LocString NAME = "Satellite";

					// Token: 0x0400B0D1 RID: 45265
					public static LocString DESCRIPTION = "An artificial construct that has escaped its orbit. It no longer appears to be monitored.";
				}
			}

			// Token: 0x02002A93 RID: 10899
			public class NONE
			{
				// Token: 0x0400B0D2 RID: 45266
				public static LocString NAME = "Unselected";
			}

			// Token: 0x02002A94 RID: 10900
			public class ORBIT
			{
				// Token: 0x0400B0D3 RID: 45267
				public static LocString NAME_FMT = "Orbiting {Name}";
			}

			// Token: 0x02002A95 RID: 10901
			public class EMPTY_SPACE
			{
				// Token: 0x0400B0D4 RID: 45268
				public static LocString NAME = "Empty Space";
			}

			// Token: 0x02002A96 RID: 10902
			public class FOG_OF_WAR_SPACE
			{
				// Token: 0x0400B0D5 RID: 45269
				public static LocString NAME = "Unexplored Space";
			}

			// Token: 0x02002A97 RID: 10903
			public class ARTIFACT_POI
			{
				// Token: 0x02002A98 RID: 10904
				public class GRAVITASSPACESTATION1
				{
					// Token: 0x0400B0D6 RID: 45270
					public static LocString NAME = "Destroyed Satellite";

					// Token: 0x0400B0D7 RID: 45271
					public static LocString DESC = "The remnants of a bygone era, lost in time.\n\nHarvesting space junk requires a rocket equipped with a " + UI.FormatAsLink("Drillcone", "NOSECONEHARVEST") + ".";
				}

				// Token: 0x02002A99 RID: 10905
				public class GRAVITASSPACESTATION2
				{
					// Token: 0x0400B0D8 RID: 45272
					public static LocString NAME = "Demolished Rocket";

					// Token: 0x0400B0D9 RID: 45273
					public static LocString DESC = "A defunct rocket from a corporation that vanished long ago.\n\nHarvesting space junk requires a rocket equipped with a " + UI.FormatAsLink("Drillcone", "NOSECONEHARVEST") + ".";
				}

				// Token: 0x02002A9A RID: 10906
				public class GRAVITASSPACESTATION3
				{
					// Token: 0x0400B0DA RID: 45274
					public static LocString NAME = "Ruined Rocket";

					// Token: 0x0400B0DB RID: 45275
					public static LocString DESC = "The ruins of a rocket that stopped functioning ages ago.\n\nHarvesting space junk requires a rocket equipped with a " + UI.FormatAsLink("Drillcone", "NOSECONEHARVEST") + ".";
				}

				// Token: 0x02002A9B RID: 10907
				public class GRAVITASSPACESTATION4
				{
					// Token: 0x0400B0DC RID: 45276
					public static LocString NAME = "Retired Planetary Excursion Module";

					// Token: 0x0400B0DD RID: 45277
					public static LocString DESC = "A rocket part from a society that has been wiped out.\n\nHarvesting space junk requires a rocket equipped with a " + UI.FormatAsLink("Drillcone", "NOSECONEHARVEST") + ".";
				}

				// Token: 0x02002A9C RID: 10908
				public class GRAVITASSPACESTATION5
				{
					// Token: 0x0400B0DE RID: 45278
					public static LocString NAME = "Destroyed Satellite";

					// Token: 0x0400B0DF RID: 45279
					public static LocString DESC = "A destroyed Gravitas satellite.\n\nHarvesting space junk requires a rocket equipped with a " + UI.FormatAsLink("Drillcone", "NOSECONEHARVEST") + ".";
				}

				// Token: 0x02002A9D RID: 10909
				public class GRAVITASSPACESTATION6
				{
					// Token: 0x0400B0E0 RID: 45280
					public static LocString NAME = "Annihilated Satellite";

					// Token: 0x0400B0E1 RID: 45281
					public static LocString DESC = "The remains of a satellite made some time in the past.\n\nHarvesting space junk requires a rocket equipped with a " + UI.FormatAsLink("Drillcone", "NOSECONEHARVEST") + ".";
				}

				// Token: 0x02002A9E RID: 10910
				public class GRAVITASSPACESTATION7
				{
					// Token: 0x0400B0E2 RID: 45282
					public static LocString NAME = "Wrecked Space Shuttle";

					// Token: 0x0400B0E3 RID: 45283
					public static LocString DESC = "A defunct space shuttle that floats through space unattended.\n\nHarvesting space junk requires a rocket equipped with a " + UI.FormatAsLink("Drillcone", "NOSECONEHARVEST") + ".";
				}

				// Token: 0x02002A9F RID: 10911
				public class GRAVITASSPACESTATION8
				{
					// Token: 0x0400B0E4 RID: 45284
					public static LocString NAME = "Obsolete Space Station Module";

					// Token: 0x0400B0E5 RID: 45285
					public static LocString DESC = "The module from a space station that ceased to exist ages ago.\n\nHarvesting space junk requires a rocket equipped with a " + UI.FormatAsLink("Drillcone", "NOSECONEHARVEST") + ".";
				}

				// Token: 0x02002AA0 RID: 10912
				public class RUSSELLSTEAPOT
				{
					// Token: 0x0400B0E6 RID: 45286
					public static LocString NAME = "Russell's Teapot";

					// Token: 0x0400B0E7 RID: 45287
					public static LocString DESC = "Has never been disproven to not exist.";
				}
			}

			// Token: 0x02002AA1 RID: 10913
			public class HARVESTABLE_POI
			{
				// Token: 0x0400B0E8 RID: 45288
				public static LocString POI_PRODUCTION = "{0}";

				// Token: 0x0400B0E9 RID: 45289
				public static LocString POI_PRODUCTION_TOOLTIP = "{0}";

				// Token: 0x02002AA2 RID: 10914
				public class CARBONASTEROIDFIELD
				{
					// Token: 0x0400B0EA RID: 45290
					public static LocString NAME = "Carbon Asteroid Field";

					// Token: 0x0400B0EB RID: 45291
					public static LocString DESC = string.Concat(new string[]
					{
						"An asteroid containing ",
						UI.FormatAsLink("Refined Carbon", "REFINEDCARBON"),
						" and ",
						UI.FormatAsLink("Coal", "CARBON"),
						".\n\nHarvesting resources requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						"."
					});
				}

				// Token: 0x02002AA3 RID: 10915
				public class METALLICASTEROIDFIELD
				{
					// Token: 0x0400B0EC RID: 45292
					public static LocString NAME = "Metallic Asteroid Field";

					// Token: 0x0400B0ED RID: 45293
					public static LocString DESC = string.Concat(new string[]
					{
						"An asteroid field containing ",
						UI.FormatAsLink("Iron", "IRON"),
						", ",
						UI.FormatAsLink("Copper", "COPPER"),
						" and ",
						UI.FormatAsLink("Obsidian", "OBSIDIAN"),
						".\n\nHarvesting resources requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						"."
					});
				}

				// Token: 0x02002AA4 RID: 10916
				public class SATELLITEFIELD
				{
					// Token: 0x0400B0EE RID: 45294
					public static LocString NAME = "Space Debris";

					// Token: 0x0400B0EF RID: 45295
					public static LocString DESC = "Space junk from a forgotten age.\n\nHarvesting resources requires a rocket equipped with a " + UI.FormatAsLink("Drillcone", "NOSECONEHARVEST") + ".";
				}

				// Token: 0x02002AA5 RID: 10917
				public class ROCKYASTEROIDFIELD
				{
					// Token: 0x0400B0F0 RID: 45296
					public static LocString NAME = "Rocky Asteroid Field";

					// Token: 0x0400B0F1 RID: 45297
					public static LocString DESC = string.Concat(new string[]
					{
						"An asteroid field containing ",
						UI.FormatAsLink("Copper Ore", "CUPRITE"),
						", ",
						UI.FormatAsLink("Sedimentary Rock", "SEDIMENTARYROCK"),
						" and ",
						UI.FormatAsLink("Igneous Rock", "IGNEOUSROCK"),
						".\n\nHarvesting resources requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						"."
					});
				}

				// Token: 0x02002AA6 RID: 10918
				public class INTERSTELLARICEFIELD
				{
					// Token: 0x0400B0F2 RID: 45298
					public static LocString NAME = "Ice Asteroid Field";

					// Token: 0x0400B0F3 RID: 45299
					public static LocString DESC = string.Concat(new string[]
					{
						"An asteroid field containing ",
						UI.FormatAsLink("Ice", "ICE"),
						", ",
						UI.FormatAsLink("Carbon Dioxide", "CARBONDIOXIDE"),
						" and ",
						UI.FormatAsLink("Oxygen", "OXYGEN"),
						".\n\nHarvesting resources requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						"."
					});
				}

				// Token: 0x02002AA7 RID: 10919
				public class ORGANICMASSFIELD
				{
					// Token: 0x0400B0F4 RID: 45300
					public static LocString NAME = "Organic Mass Field";

					// Token: 0x0400B0F5 RID: 45301
					public static LocString DESC = string.Concat(new string[]
					{
						"A mass of harvestable resources containing ",
						UI.FormatAsLink("Algae", "ALGAE"),
						", ",
						UI.FormatAsLink("Slime", "SLIMEMOLD"),
						", ",
						UI.FormatAsLink("Polluted Oxygen", "CONTAMINATEDOXYGEN"),
						" and ",
						UI.FormatAsLink("Dirt", "DIRT"),
						".\n\nHarvesting resources requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						"."
					});
				}

				// Token: 0x02002AA8 RID: 10920
				public class ICEASTEROIDFIELD
				{
					// Token: 0x0400B0F6 RID: 45302
					public static LocString NAME = "Exploded Ice Giant";

					// Token: 0x0400B0F7 RID: 45303
					public static LocString DESC = string.Concat(new string[]
					{
						"A cloud of planetary remains containing ",
						UI.FormatAsLink("Ice", "ICE"),
						", ",
						UI.FormatAsLink("Carbon Dioxide", "CARBONDIOXIDE"),
						", ",
						UI.FormatAsLink("Oxygen", "OXYGEN"),
						" and ",
						UI.FormatAsLink("Natural Gas", "METHANE"),
						".\n\nHarvesting resources requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						"."
					});
				}

				// Token: 0x02002AA9 RID: 10921
				public class GASGIANTCLOUD
				{
					// Token: 0x0400B0F8 RID: 45304
					public static LocString NAME = "Exploded Gas Giant";

					// Token: 0x0400B0F9 RID: 45305
					public static LocString DESC = string.Concat(new string[]
					{
						"The harvestable remains of a planet containing ",
						UI.FormatAsLink("Hydrogen", "HYDROGEN"),
						" in ",
						UI.FormatAsLink("gas", "ELEMENTS_GAS"),
						" form, and ",
						UI.FormatAsLink("Methane", "SOLIDMETHANE"),
						" in ",
						UI.FormatAsLink("solid", "ELEMENTS_SOLID"),
						" and ",
						UI.FormatAsLink("liquid", "ELEMENTS_LIQUID"),
						" form.\n\nHarvesting resources requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						"."
					});
				}

				// Token: 0x02002AAA RID: 10922
				public class CHLORINECLOUD
				{
					// Token: 0x0400B0FA RID: 45306
					public static LocString NAME = "Chlorine Cloud";

					// Token: 0x0400B0FB RID: 45307
					public static LocString DESC = string.Concat(new string[]
					{
						"A cloud of harvestable debris containing ",
						UI.FormatAsLink("Chlorine Gas", "CHLORINEGAS"),
						" and ",
						UI.FormatAsLink("Bleach Stone", "BLEACHSTONE"),
						".\n\nHarvesting resources requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						"."
					});
				}

				// Token: 0x02002AAB RID: 10923
				public class GILDEDASTEROIDFIELD
				{
					// Token: 0x0400B0FC RID: 45308
					public static LocString NAME = "Gilded Asteroid Field";

					// Token: 0x0400B0FD RID: 45309
					public static LocString DESC = string.Concat(new string[]
					{
						"An asteroid field containing ",
						UI.FormatAsLink("Gold", "GOLD"),
						", ",
						UI.FormatAsLink("Fullerene", "FULLERENE"),
						", ",
						UI.FormatAsLink("Regolith", "REGOLITH"),
						" and more.\n\nHarvesting resources requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						"."
					});
				}

				// Token: 0x02002AAC RID: 10924
				public class GLIMMERINGASTEROIDFIELD
				{
					// Token: 0x0400B0FE RID: 45310
					public static LocString NAME = "Glimmering Asteroid Field";

					// Token: 0x0400B0FF RID: 45311
					public static LocString DESC = string.Concat(new string[]
					{
						"An asteroid field containing ",
						UI.FormatAsLink("Tungsten", "TUNGSTEN"),
						", ",
						UI.FormatAsLink("Wolframite", "WOLFRAMITE"),
						" and more.\n\nHarvesting resources requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						"."
					});
				}

				// Token: 0x02002AAD RID: 10925
				public class HELIUMCLOUD
				{
					// Token: 0x0400B100 RID: 45312
					public static LocString NAME = "Helium Cloud";

					// Token: 0x0400B101 RID: 45313
					public static LocString DESC = string.Concat(new string[]
					{
						"A cloud of resources containing ",
						UI.FormatAsLink("Water", "WATER"),
						" and ",
						UI.FormatAsLink("Hydrogen Gas", "HYDROGEN"),
						".\n\nHarvesting resources requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						"."
					});
				}

				// Token: 0x02002AAE RID: 10926
				public class OILYASTEROIDFIELD
				{
					// Token: 0x0400B102 RID: 45314
					public static LocString NAME = "Oily Asteroid Field";

					// Token: 0x0400B103 RID: 45315
					public static LocString DESC = string.Concat(new string[]
					{
						"An asteroid field containing ",
						UI.FormatAsLink("Solid Methane", "SOLIDMETHANE"),
						", ",
						UI.FormatAsLink("Carbon Dioxide", "CARBONDIOXIDE"),
						" and ",
						UI.FormatAsLink("Crude Oil", "CRUDEOIL"),
						".\n\nHarvesting resources requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						"."
					});
				}

				// Token: 0x02002AAF RID: 10927
				public class OXIDIZEDASTEROIDFIELD
				{
					// Token: 0x0400B104 RID: 45316
					public static LocString NAME = "Oxidized Asteroid Field";

					// Token: 0x0400B105 RID: 45317
					public static LocString DESC = string.Concat(new string[]
					{
						"An asteroid field containing ",
						UI.FormatAsLink("Carbon Dioxide", "CARBONDIOXIDE"),
						" and ",
						UI.FormatAsLink("Rust", "RUST"),
						".\n\nHarvesting resources requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						"."
					});
				}

				// Token: 0x02002AB0 RID: 10928
				public class SALTYASTEROIDFIELD
				{
					// Token: 0x0400B106 RID: 45318
					public static LocString NAME = "Salty Asteroid Field";

					// Token: 0x0400B107 RID: 45319
					public static LocString DESC = string.Concat(new string[]
					{
						"A field of harvestable resources containing ",
						UI.FormatAsLink("Salt Water", "SALTWATER"),
						",",
						UI.FormatAsLink("Brine", "BRINE"),
						" and ",
						UI.FormatAsLink("Carbon Dioxide", "CARBONDIOXIDE"),
						".\n\nHarvesting resources requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						"."
					});
				}

				// Token: 0x02002AB1 RID: 10929
				public class FROZENOREFIELD
				{
					// Token: 0x0400B108 RID: 45320
					public static LocString NAME = "Frozen Ore Asteroid Field";

					// Token: 0x0400B109 RID: 45321
					public static LocString DESC = string.Concat(new string[]
					{
						"An asteroid field containing ",
						UI.FormatAsLink("Polluted Ice", "DIRTYICE"),
						", ",
						UI.FormatAsLink("Ice", "ICE"),
						", ",
						UI.FormatAsLink("Snow", "SNOW"),
						" and ",
						UI.FormatAsLink("Aluminum Ore", "ALUMINUMORE"),
						".\n\nHarvesting resources requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						"."
					});
				}

				// Token: 0x02002AB2 RID: 10930
				public class FORESTYOREFIELD
				{
					// Token: 0x0400B10A RID: 45322
					public static LocString NAME = "Forested Ore Field";

					// Token: 0x0400B10B RID: 45323
					public static LocString DESC = string.Concat(new string[]
					{
						"A field of harvestable resources containing ",
						UI.FormatAsLink("Carbon Dioxide", "CARBONDIOXIDE"),
						", ",
						UI.FormatAsLink("Igneous Rock", "IGNEOUSROCK"),
						" and ",
						UI.FormatAsLink("Aluminum Ore", "ALUMINUMORE"),
						".\n\nHarvesting resources requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						"."
					});
				}

				// Token: 0x02002AB3 RID: 10931
				public class SWAMPYOREFIELD
				{
					// Token: 0x0400B10C RID: 45324
					public static LocString NAME = "Swampy Ore Field";

					// Token: 0x0400B10D RID: 45325
					public static LocString DESC = string.Concat(new string[]
					{
						"An asteroid field containing ",
						UI.FormatAsLink("Mud", "MUD"),
						", ",
						UI.FormatAsLink("Polluted Dirt", "TOXICSAND"),
						" and ",
						UI.FormatAsLink("Cobalt Ore", "COBALTITE"),
						".\n\nHarvesting resources requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						"."
					});
				}

				// Token: 0x02002AB4 RID: 10932
				public class SANDYOREFIELD
				{
					// Token: 0x0400B10E RID: 45326
					public static LocString NAME = "Sandy Ore Field";

					// Token: 0x0400B10F RID: 45327
					public static LocString DESC = string.Concat(new string[]
					{
						"An asteroid field containing ",
						UI.FormatAsLink("Sandstone", "SANDSTONE"),
						", ",
						UI.FormatAsLink("Algae", "ALGAE"),
						", ",
						UI.FormatAsLink("Copper Ore", "CUPRITE"),
						" and ",
						UI.FormatAsLink("Sand", "SAND"),
						".\n\nHarvesting resources requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						"."
					});
				}

				// Token: 0x02002AB5 RID: 10933
				public class RADIOACTIVEGASCLOUD
				{
					// Token: 0x0400B110 RID: 45328
					public static LocString NAME = "Radioactive Gas Cloud";

					// Token: 0x0400B111 RID: 45329
					public static LocString DESC = string.Concat(new string[]
					{
						"A cloud of resources containing ",
						UI.FormatAsLink("Chlorine Gas", "CHLORINEGAS"),
						", ",
						UI.FormatAsLink("Uranium Ore", "URANIUMORE"),
						" and ",
						UI.FormatAsLink("Carbon Dioxide", "CARBONDIOXIDE"),
						".\n\nHarvesting resources requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						"."
					});
				}

				// Token: 0x02002AB6 RID: 10934
				public class RADIOACTIVEASTEROIDFIELD
				{
					// Token: 0x0400B112 RID: 45330
					public static LocString NAME = "Radioactive Asteroid Field";

					// Token: 0x0400B113 RID: 45331
					public static LocString DESC = string.Concat(new string[]
					{
						"An asteroid field containing ",
						UI.FormatAsLink("Bleach Stone", "BLEACHSTONE"),
						", ",
						UI.FormatAsLink("Rust", "RUST"),
						", ",
						UI.FormatAsLink("Uranium Ore", "URANIUMORE"),
						" and ",
						UI.FormatAsLink("Sulfur", "SULFUR"),
						".\n\nHarvesting resources requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						"."
					});
				}

				// Token: 0x02002AB7 RID: 10935
				public class OXYGENRICHASTEROIDFIELD
				{
					// Token: 0x0400B114 RID: 45332
					public static LocString NAME = "Oxygen Rich Asteroid Field";

					// Token: 0x0400B115 RID: 45333
					public static LocString DESC = string.Concat(new string[]
					{
						"An asteroid field containing ",
						UI.FormatAsLink("Ice", "ICE"),
						", ",
						UI.FormatAsLink("Polluted Oxygen", "CONTAMINATEDOXYGEN"),
						" and ",
						UI.FormatAsLink("Water", "WATER"),
						".\n\nHarvesting resources requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						"."
					});
				}

				// Token: 0x02002AB8 RID: 10936
				public class INTERSTELLAROCEAN
				{
					// Token: 0x0400B116 RID: 45334
					public static LocString NAME = "Interstellar Ocean";

					// Token: 0x0400B117 RID: 45335
					public static LocString DESC = string.Concat(new string[]
					{
						"An interplanetary body that consists of ",
						UI.FormatAsLink("Salt Water", "SALTWATER"),
						", ",
						UI.FormatAsLink("Brine", "BRINE"),
						", ",
						UI.FormatAsLink("Salt", "SALT"),
						" and ",
						UI.FormatAsLink("Ice", "ICE"),
						".\n\nHarvesting resources requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						"."
					});
				}

				// Token: 0x02002AB9 RID: 10937
				public class DLC2CERESFIELD
				{
					// Token: 0x0400B118 RID: 45336
					public static LocString NAME = "Frozen Cinnabar Asteroid Field";

					// Token: 0x0400B119 RID: 45337
					public static LocString DESC = string.Concat(new string[]
					{
						"The harvestable remains of a planet containing ",
						UI.FormatAsLink("Cinnabar", "Cinnabar"),
						", ",
						UI.FormatAsLink("Ice", "ICE"),
						" and ",
						UI.FormatAsLink("Mercury", "MERCURY"),
						".\n\nHarvesting resources requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						"."
					});
				}

				// Token: 0x02002ABA RID: 10938
				public class DLC2CERESOREFIELD
				{
					// Token: 0x0400B11A RID: 45338
					public static LocString NAME = "Frozen Mercury Asteroid Field";

					// Token: 0x0400B11B RID: 45339
					public static LocString DESC = string.Concat(new string[]
					{
						"An asteroid field containing ",
						UI.FormatAsLink("Cinnabar", "Cinnabar"),
						", ",
						UI.FormatAsLink("Ice", "ICE"),
						" and ",
						UI.FormatAsLink("Mercury", "MERCURY"),
						".\n\nHarvesting resources requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						"."
					});
				}
			}

			// Token: 0x02002ABB RID: 10939
			public class GRAVITAS_SPACE_POI
			{
				// Token: 0x0400B11C RID: 45340
				public static LocString STATION = "Destroyed Gravitas Space Station";
			}

			// Token: 0x02002ABC RID: 10940
			public class TELESCOPE_TARGET
			{
				// Token: 0x0400B11D RID: 45341
				public static LocString NAME = "Telescope Target";
			}

			// Token: 0x02002ABD RID: 10941
			public class ASTEROIDS
			{
				// Token: 0x02002ABE RID: 10942
				public class ROCKYASTEROID
				{
					// Token: 0x0400B11E RID: 45342
					public static LocString NAME = "Rocky Asteroid";

					// Token: 0x0400B11F RID: 45343
					public static LocString DESCRIPTION = "A minor mineral planet. Unlike a comet, it does not possess a tail.";
				}

				// Token: 0x02002ABF RID: 10943
				public class METALLICASTEROID
				{
					// Token: 0x0400B120 RID: 45344
					public static LocString NAME = "Metallic Asteroid";

					// Token: 0x0400B121 RID: 45345
					public static LocString DESCRIPTION = "A shimmering conglomerate of various metals.";
				}

				// Token: 0x02002AC0 RID: 10944
				public class CARBONACEOUSASTEROID
				{
					// Token: 0x0400B122 RID: 45346
					public static LocString NAME = "Carbon Asteroid";

					// Token: 0x0400B123 RID: 45347
					public static LocString DESCRIPTION = "A common asteroid containing several useful resources.";
				}

				// Token: 0x02002AC1 RID: 10945
				public class OILYASTEROID
				{
					// Token: 0x0400B124 RID: 45348
					public static LocString NAME = "Oily Asteroid";

					// Token: 0x0400B125 RID: 45349
					public static LocString DESCRIPTION = "A viscous asteroid that is only loosely held together. Contains fossil fuel resources.";
				}

				// Token: 0x02002AC2 RID: 10946
				public class GOLDASTEROID
				{
					// Token: 0x0400B126 RID: 45350
					public static LocString NAME = "Gilded Asteroid";

					// Token: 0x0400B127 RID: 45351
					public static LocString DESCRIPTION = "A rich asteroid with thin gold coating and veins of gold deposits throughout.";
				}
			}

			// Token: 0x02002AC3 RID: 10947
			public class CLUSTERMAPMETEORSHOWERS
			{
				// Token: 0x02002AC4 RID: 10948
				public class UNIDENTIFIED
				{
					// Token: 0x0400B128 RID: 45352
					public static LocString NAME = "Unidentified Object";

					// Token: 0x0400B129 RID: 45353
					public static LocString DESCRIPTION = "A cosmic anomaly is traveling through the galaxy.\n\nIts origins and purpose are currently unknown, though a " + BUILDINGS.PREFABS.CLUSTERTELESCOPE.NAME + " could change that.";
				}

				// Token: 0x02002AC5 RID: 10949
				public class SLIME
				{
					// Token: 0x0400B12A RID: 45354
					public static LocString NAME = "Slimy Meteor Shower";

					// Token: 0x0400B12B RID: 45355
					public static LocString DESCRIPTION = "A shower of slimy, biodynamic meteors on a collision course with the surface of an asteroid.";
				}

				// Token: 0x02002AC6 RID: 10950
				public class SNOW
				{
					// Token: 0x0400B12C RID: 45356
					public static LocString NAME = "Blizzard Meteor Shower";

					// Token: 0x0400B12D RID: 45357
					public static LocString DESCRIPTION = "A shower of cold, cold meteors on a collision course with the surface of an asteroid.";
				}

				// Token: 0x02002AC7 RID: 10951
				public class ICE
				{
					// Token: 0x0400B12E RID: 45358
					public static LocString NAME = "Ice Meteor Shower";

					// Token: 0x0400B12F RID: 45359
					public static LocString DESCRIPTION = "A hailstorm of icy space rocks on a collision course with the surface of an asteroid.";
				}

				// Token: 0x02002AC8 RID: 10952
				public class ICEANDTREES
				{
					// Token: 0x0400B130 RID: 45360
					public static LocString NAME = "Icy Nectar Meteor Shower";

					// Token: 0x0400B131 RID: 45361
					public static LocString DESCRIPTION = "A hailstorm of sweet, icy space rocks on a collision course with the surface of an asteroid";
				}

				// Token: 0x02002AC9 RID: 10953
				public class COPPER
				{
					// Token: 0x0400B132 RID: 45362
					public static LocString NAME = "Copper Meteor Shower";

					// Token: 0x0400B133 RID: 45363
					public static LocString DESCRIPTION = "A shower of metallic meteors on a collision course with the surface of an asteroid.";
				}

				// Token: 0x02002ACA RID: 10954
				public class IRON
				{
					// Token: 0x0400B134 RID: 45364
					public static LocString NAME = "Iron Meteor Shower";

					// Token: 0x0400B135 RID: 45365
					public static LocString DESCRIPTION = "A shower of metallic space rocks on a collision course with the surface of an asteroid.";
				}

				// Token: 0x02002ACB RID: 10955
				public class GOLD
				{
					// Token: 0x0400B136 RID: 45366
					public static LocString NAME = "Gold Meteor Shower";

					// Token: 0x0400B137 RID: 45367
					public static LocString DESCRIPTION = "A shower of shiny metallic space rocks on a collision course with the surface of an asteroid.";
				}

				// Token: 0x02002ACC RID: 10956
				public class URANIUM
				{
					// Token: 0x0400B138 RID: 45368
					public static LocString NAME = "Uranium Meteor Shower";

					// Token: 0x0400B139 RID: 45369
					public static LocString DESCRIPTION = "A toxic shower of radioactive meteors on a collision course with the surface of an asteroid.";
				}

				// Token: 0x02002ACD RID: 10957
				public class LIGHTDUST
				{
					// Token: 0x0400B13A RID: 45370
					public static LocString NAME = "Dust Fluff Meteor Shower";

					// Token: 0x0400B13B RID: 45371
					public static LocString DESCRIPTION = "A cloud-like shower of dust fluff meteors heading towards the surface of an asteroid.";
				}

				// Token: 0x02002ACE RID: 10958
				public class HEAVYDUST
				{
					// Token: 0x0400B13C RID: 45372
					public static LocString NAME = "Dense Dust Meteor Shower";

					// Token: 0x0400B13D RID: 45373
					public static LocString DESCRIPTION = "A dark cloud of heavy dust meteors heading towards the surface of an asteroid.";
				}

				// Token: 0x02002ACF RID: 10959
				public class REGOLITH
				{
					// Token: 0x0400B13E RID: 45374
					public static LocString NAME = "Regolith Meteor Shower";

					// Token: 0x0400B13F RID: 45375
					public static LocString DESCRIPTION = "A shower of rocky meteors on a collision course with the surface of an asteroid.";
				}

				// Token: 0x02002AD0 RID: 10960
				public class OXYLITE
				{
					// Token: 0x0400B140 RID: 45376
					public static LocString NAME = "Oxylite Meteor Shower";

					// Token: 0x0400B141 RID: 45377
					public static LocString DESCRIPTION = "A shower of rocky, oxygen-rich meteors on a collision course with the surface of an asteroid.";
				}

				// Token: 0x02002AD1 RID: 10961
				public class BLEACHSTONE
				{
					// Token: 0x0400B142 RID: 45378
					public static LocString NAME = "Bleach Stone Meteor Shower";

					// Token: 0x0400B143 RID: 45379
					public static LocString DESCRIPTION = "A shower of bleach stone meteors on a collision course with the surface of an asteroid.";
				}

				// Token: 0x02002AD2 RID: 10962
				public class MOO
				{
					// Token: 0x0400B144 RID: 45380
					public static LocString NAME = "Gassy Mooteor Shower";

					// Token: 0x0400B145 RID: 45381
					public static LocString DESCRIPTION = "A herd of methane-infused meteors that cause a bit of a stink, but do no actual damage.";
				}
			}

			// Token: 0x02002AD3 RID: 10963
			public class COMETS
			{
				// Token: 0x02002AD4 RID: 10964
				public class ROCKCOMET
				{
					// Token: 0x0400B146 RID: 45382
					public static LocString NAME = "Rock Meteor";
				}

				// Token: 0x02002AD5 RID: 10965
				public class DUSTCOMET
				{
					// Token: 0x0400B147 RID: 45383
					public static LocString NAME = "Dust Meteor";
				}

				// Token: 0x02002AD6 RID: 10966
				public class IRONCOMET
				{
					// Token: 0x0400B148 RID: 45384
					public static LocString NAME = "Iron Meteor";
				}

				// Token: 0x02002AD7 RID: 10967
				public class COPPERCOMET
				{
					// Token: 0x0400B149 RID: 45385
					public static LocString NAME = "Copper Meteor";
				}

				// Token: 0x02002AD8 RID: 10968
				public class GOLDCOMET
				{
					// Token: 0x0400B14A RID: 45386
					public static LocString NAME = "Gold Meteor";
				}

				// Token: 0x02002AD9 RID: 10969
				public class FULLERENECOMET
				{
					// Token: 0x0400B14B RID: 45387
					public static LocString NAME = "Fullerene Meteor";
				}

				// Token: 0x02002ADA RID: 10970
				public class URANIUMORECOMET
				{
					// Token: 0x0400B14C RID: 45388
					public static LocString NAME = "Uranium Meteor";
				}

				// Token: 0x02002ADB RID: 10971
				public class NUCLEAR_WASTE
				{
					// Token: 0x0400B14D RID: 45389
					public static LocString NAME = "Radioactive Meteor";
				}

				// Token: 0x02002ADC RID: 10972
				public class SATELLITE
				{
					// Token: 0x0400B14E RID: 45390
					public static LocString NAME = "Defunct Satellite";
				}

				// Token: 0x02002ADD RID: 10973
				public class FOODCOMET
				{
					// Token: 0x0400B14F RID: 45391
					public static LocString NAME = "Snack Bomb";
				}

				// Token: 0x02002ADE RID: 10974
				public class GASSYMOOCOMET
				{
					// Token: 0x0400B150 RID: 45392
					public static LocString NAME = "Gassy Mooteor";
				}

				// Token: 0x02002ADF RID: 10975
				public class SLIMECOMET
				{
					// Token: 0x0400B151 RID: 45393
					public static LocString NAME = "Slime Meteor";
				}

				// Token: 0x02002AE0 RID: 10976
				public class SNOWBALLCOMET
				{
					// Token: 0x0400B152 RID: 45394
					public static LocString NAME = "Snow Meteor";
				}

				// Token: 0x02002AE1 RID: 10977
				public class SPACETREESEEDCOMET
				{
					// Token: 0x0400B153 RID: 45395
					public static LocString NAME = "Bonbon Meteor";
				}

				// Token: 0x02002AE2 RID: 10978
				public class HARDICECOMET
				{
					// Token: 0x0400B154 RID: 45396
					public static LocString NAME = "Ice Meteor";
				}

				// Token: 0x02002AE3 RID: 10979
				public class LIGHTDUSTCOMET
				{
					// Token: 0x0400B155 RID: 45397
					public static LocString NAME = "Dust Fluff Meteor";
				}

				// Token: 0x02002AE4 RID: 10980
				public class ALGAECOMET
				{
					// Token: 0x0400B156 RID: 45398
					public static LocString NAME = "Algae Meteor";
				}

				// Token: 0x02002AE5 RID: 10981
				public class PHOSPHORICCOMET
				{
					// Token: 0x0400B157 RID: 45399
					public static LocString NAME = "Phosphoric Meteor";
				}

				// Token: 0x02002AE6 RID: 10982
				public class OXYLITECOMET
				{
					// Token: 0x0400B158 RID: 45400
					public static LocString NAME = "Oxylite Meteor";
				}

				// Token: 0x02002AE7 RID: 10983
				public class BLEACHSTONECOMET
				{
					// Token: 0x0400B159 RID: 45401
					public static LocString NAME = "Bleach Stone Meteor";
				}

				// Token: 0x02002AE8 RID: 10984
				public class MINICOMET
				{
					// Token: 0x0400B15A RID: 45402
					public static LocString NAME = "Debris Projectile";
				}
			}

			// Token: 0x02002AE9 RID: 10985
			public class DWARFPLANETS
			{
				// Token: 0x02002AEA RID: 10986
				public class ICYDWARF
				{
					// Token: 0x0400B15B RID: 45403
					public static LocString NAME = "Interstellar Ice";

					// Token: 0x0400B15C RID: 45404
					public static LocString DESCRIPTION = "A terrestrial destination, frozen completely solid.";
				}

				// Token: 0x02002AEB RID: 10987
				public class ORGANICDWARF
				{
					// Token: 0x0400B15D RID: 45405
					public static LocString NAME = "Organic Mass";

					// Token: 0x0400B15E RID: 45406
					public static LocString DESCRIPTION = "A mass of organic material similar to the ooze used to print Duplicants. This sample is heavily degraded.";
				}

				// Token: 0x02002AEC RID: 10988
				public class DUSTYDWARF
				{
					// Token: 0x0400B15F RID: 45407
					public static LocString NAME = "Dusty Dwarf";

					// Token: 0x0400B160 RID: 45408
					public static LocString DESCRIPTION = "A loosely held together composite of minerals.";
				}

				// Token: 0x02002AED RID: 10989
				public class SALTDWARF
				{
					// Token: 0x0400B161 RID: 45409
					public static LocString NAME = "Salty Dwarf";

					// Token: 0x0400B162 RID: 45410
					public static LocString DESCRIPTION = "A dwarf planet with unusually high sodium concentrations.";
				}

				// Token: 0x02002AEE RID: 10990
				public class REDDWARF
				{
					// Token: 0x0400B163 RID: 45411
					public static LocString NAME = "Red Dwarf";

					// Token: 0x0400B164 RID: 45412
					public static LocString DESCRIPTION = "An M-class star orbited by clusters of extractable aluminum and methane.";
				}
			}

			// Token: 0x02002AEF RID: 10991
			public class PLANETS
			{
				// Token: 0x02002AF0 RID: 10992
				public class TERRAPLANET
				{
					// Token: 0x0400B165 RID: 45413
					public static LocString NAME = "Terrestrial Planet";

					// Token: 0x0400B166 RID: 45414
					public static LocString DESCRIPTION = "A planet with a walkable surface, though it does not possess the resources to sustain long-term life.";
				}

				// Token: 0x02002AF1 RID: 10993
				public class VOLCANOPLANET
				{
					// Token: 0x0400B167 RID: 45415
					public static LocString NAME = "Volcanic Planet";

					// Token: 0x0400B168 RID: 45416
					public static LocString DESCRIPTION = "A large terrestrial object composed mainly of molten rock.";
				}

				// Token: 0x02002AF2 RID: 10994
				public class SHATTEREDPLANET
				{
					// Token: 0x0400B169 RID: 45417
					public static LocString NAME = "Shattered Planet";

					// Token: 0x0400B16A RID: 45418
					public static LocString DESCRIPTION = "A once-habitable planet that has sustained massive damage.\n\nA powerful containment field prevents our rockets from traveling to its surface.";
				}

				// Token: 0x02002AF3 RID: 10995
				public class RUSTPLANET
				{
					// Token: 0x0400B16B RID: 45419
					public static LocString NAME = "Oxidized Asteroid";

					// Token: 0x0400B16C RID: 45420
					public static LocString DESCRIPTION = "A small planet covered in large swathes of brown rust.";
				}

				// Token: 0x02002AF4 RID: 10996
				public class FORESTPLANET
				{
					// Token: 0x0400B16D RID: 45421
					public static LocString NAME = "Living Planet";

					// Token: 0x0400B16E RID: 45422
					public static LocString DESCRIPTION = "A small green planet displaying several markers of primitive life.";
				}

				// Token: 0x02002AF5 RID: 10997
				public class SHINYPLANET
				{
					// Token: 0x0400B16F RID: 45423
					public static LocString NAME = "Glimmering Planet";

					// Token: 0x0400B170 RID: 45424
					public static LocString DESCRIPTION = "A planet composed of rare, shimmering minerals. From the distance, it looks like gem in the sky.";
				}

				// Token: 0x02002AF6 RID: 10998
				public class CHLORINEPLANET
				{
					// Token: 0x0400B171 RID: 45425
					public static LocString NAME = "Chlorine Planet";

					// Token: 0x0400B172 RID: 45426
					public static LocString DESCRIPTION = "A noxious planet permeated by unbreathable chlorine.";
				}

				// Token: 0x02002AF7 RID: 10999
				public class SALTDESERTPLANET
				{
					// Token: 0x0400B173 RID: 45427
					public static LocString NAME = "Arid Planet";

					// Token: 0x0400B174 RID: 45428
					public static LocString DESCRIPTION = "A sweltering, desert-like planet covered in surface salt deposits.";
				}

				// Token: 0x02002AF8 RID: 11000
				public class DLC2CERESSPACEDESTINATION
				{
					// Token: 0x0400B175 RID: 45429
					public static LocString NAME = "Ceres";

					// Token: 0x0400B176 RID: 45430
					public static LocString DESCRIPTION = "A frozen planet peppered with cinnabar deposits.";
				}
			}

			// Token: 0x02002AF9 RID: 11001
			public class GIANTS
			{
				// Token: 0x02002AFA RID: 11002
				public class GASGIANT
				{
					// Token: 0x0400B177 RID: 45431
					public static LocString NAME = "Gas Giant";

					// Token: 0x0400B178 RID: 45432
					public static LocString DESCRIPTION = "A massive volume of " + UI.FormatAsLink("Hydrogen Gas", "HYDROGEN") + " formed around a small solid center.";
				}

				// Token: 0x02002AFB RID: 11003
				public class ICEGIANT
				{
					// Token: 0x0400B179 RID: 45433
					public static LocString NAME = "Ice Giant";

					// Token: 0x0400B17A RID: 45434
					public static LocString DESCRIPTION = "A massive volume of frozen material, primarily composed of " + UI.FormatAsLink("Ice", "ICE") + ".";
				}

				// Token: 0x02002AFC RID: 11004
				public class HYDROGENGIANT
				{
					// Token: 0x0400B17B RID: 45435
					public static LocString NAME = "Helium Giant";

					// Token: 0x0400B17C RID: 45436
					public static LocString DESCRIPTION = "A massive volume of " + UI.FormatAsLink("Helium", "HELIUM") + " formed around a small solid center.";
				}
			}
		}

		// Token: 0x02002AFD RID: 11005
		public class SPACEARTIFACTS
		{
			// Token: 0x02002AFE RID: 11006
			public class ARTIFACTTIERS
			{
				// Token: 0x0400B17D RID: 45437
				public static LocString TIER_NONE = "Nothing";

				// Token: 0x0400B17E RID: 45438
				public static LocString TIER0 = "Rarity 0";

				// Token: 0x0400B17F RID: 45439
				public static LocString TIER1 = "Rarity 1";

				// Token: 0x0400B180 RID: 45440
				public static LocString TIER2 = "Rarity 2";

				// Token: 0x0400B181 RID: 45441
				public static LocString TIER3 = "Rarity 3";

				// Token: 0x0400B182 RID: 45442
				public static LocString TIER4 = "Rarity 4";

				// Token: 0x0400B183 RID: 45443
				public static LocString TIER5 = "Rarity 5";
			}

			// Token: 0x02002AFF RID: 11007
			public class PACUPERCOLATOR
			{
				// Token: 0x0400B184 RID: 45444
				public static LocString NAME = "Percolator";

				// Token: 0x0400B185 RID: 45445
				public static LocString DESCRIPTION = "Don't drink from it! There was a pacu... IN the percolator!";

				// Token: 0x0400B186 RID: 45446
				public static LocString ARTIFACT = "A coffee percolator with the remnants of a blend of coffee that was a personal favorite of Dr. Hassan Aydem.\n\nHe would specifically reserve the consumption of this particular blend for when he was reviewing research papers on Sunday afternoons.";
			}

			// Token: 0x02002B00 RID: 11008
			public class ROBOTARM
			{
				// Token: 0x0400B187 RID: 45447
				public static LocString NAME = "Robot Arm";

				// Token: 0x0400B188 RID: 45448
				public static LocString DESCRIPTION = "It's not functional. Just cool.";

				// Token: 0x0400B189 RID: 45449
				public static LocString ARTIFACT = "A commercially available robot arm that has had a significant amount of modifications made to it.\n\nThe initials B.A. appear on one of the fingers.";
			}

			// Token: 0x02002B01 RID: 11009
			public class HATCHFOSSIL
			{
				// Token: 0x0400B18A RID: 45450
				public static LocString NAME = "Pristine Fossil";

				// Token: 0x0400B18B RID: 45451
				public static LocString DESCRIPTION = "The preserved bones of an early species of Hatch.";

				// Token: 0x0400B18C RID: 45452
				public static LocString ARTIFACT = "The preservation of this skeleton occurred artificially using a technique called the \"The Ali Method\".\n\nIt should be noted that this fossilization technique was pioneered by one Dr. Ashkan Seyed Ali, an employee of Gravitas.";
			}

			// Token: 0x02002B02 RID: 11010
			public class MODERNART
			{
				// Token: 0x0400B18D RID: 45453
				public static LocString NAME = "Modern Art";

				// Token: 0x0400B18E RID: 45454
				public static LocString DESCRIPTION = "I don't get it.";

				// Token: 0x0400B18F RID: 45455
				public static LocString ARTIFACT = "A sculpture of the Neoplastism movement of Modern Art.\n\nGravitas records show that this piece was once used in a presentation called 'Form and Function in Corporate Aesthetic'.";
			}

			// Token: 0x02002B03 RID: 11011
			public class EGGROCK
			{
				// Token: 0x0400B190 RID: 45456
				public static LocString NAME = "Egg-Shaped Rock";

				// Token: 0x0400B191 RID: 45457
				public static LocString DESCRIPTION = "It's unclear whether this is its naturally occurring shape, or if its appearance as been sculpted.";

				// Token: 0x0400B192 RID: 45458
				public static LocString ARTIFACT = "The words \"Happy Farters Day Dad. Love Macy\" appear on the bottom of this rock, written in a childlish scrawl.";
			}

			// Token: 0x02002B04 RID: 11012
			public class RAINBOWEGGROCK
			{
				// Token: 0x0400B193 RID: 45459
				public static LocString NAME = "Egg-Shaped Rock";

				// Token: 0x0400B194 RID: 45460
				public static LocString DESCRIPTION = "It's unclear whether this is its naturally occurring shape, or if its appearance as been sculpted.\n\nThis one is rainbow colored.";

				// Token: 0x0400B195 RID: 45461
				public static LocString ARTIFACT = "The words \"Happy Father's Day, Dad. Love you!\" appear on the bottom of this rock, written in very neat handwriting. The words are surrounded by four hearts drawn in what appears to be a pink gel pen.";
			}

			// Token: 0x02002B05 RID: 11013
			public class OKAYXRAY
			{
				// Token: 0x0400B196 RID: 45462
				public static LocString NAME = "Old X-Ray";

				// Token: 0x0400B197 RID: 45463
				public static LocString DESCRIPTION = "Ew, weird. It has five fingers!";

				// Token: 0x0400B198 RID: 45464
				public static LocString ARTIFACT = "The description on this X-ray indicates that it was taken in the Gravitas Medical Facility.\n\nMost likely this X-ray was performed while investigating an injury that occurred within the facility.";
			}

			// Token: 0x02002B06 RID: 11014
			public class SHIELDGENERATOR
			{
				// Token: 0x0400B199 RID: 45465
				public static LocString NAME = "Shield Generator";

				// Token: 0x0400B19A RID: 45466
				public static LocString DESCRIPTION = "A mechanical prototype capable of producing a small section of shielding.";

				// Token: 0x0400B19B RID: 45467
				public static LocString ARTIFACT = "The energy field produced by this shield generator completely ignores those light behaviors which are wave-like and focuses instead on its particle behaviors.\n\nThis seemingly paradoxical state is possible when light is slowed down to the point at which it stops entirely.";
			}

			// Token: 0x02002B07 RID: 11015
			public class TEAPOT
			{
				// Token: 0x0400B19C RID: 45468
				public static LocString NAME = "Encrusted Teapot";

				// Token: 0x0400B19D RID: 45469
				public static LocString DESCRIPTION = "A teapot from the depths of space, coated in a thick layer of Neutronium.";

				// Token: 0x0400B19E RID: 45470
				public static LocString ARTIFACT = "The amount of Neutronium present in this teapot suggests that it has crossed the threshold of the spacetime continuum on countless occasions, floating through many multiple universes over a plethora of times and spaces.\n\nThough there are, theoretically, an infinite amount of outcomes to any one event over many multi-verses, the homogeneity of the still relatively young multiverse suggests that this is then not the only teapot which has crossed into multiple universes. Despite the infinite possible outcomes of infinite multiverses it appears one high probability constant is that there is, or once was, a teapot floating somewhere in space within every universe.";
			}

			// Token: 0x02002B08 RID: 11016
			public class DNAMODEL
			{
				// Token: 0x0400B19F RID: 45471
				public static LocString NAME = "Double Helix Model";

				// Token: 0x0400B1A0 RID: 45472
				public static LocString DESCRIPTION = "An educational model of genetic information.";

				// Token: 0x0400B1A1 RID: 45473
				public static LocString ARTIFACT = "A physical representation of the building blocks of life.\n\nThis one contains trace amounts of a Genetic Ooze prototype that was once used by Gravitas.";
			}

			// Token: 0x02002B09 RID: 11017
			public class SANDSTONE
			{
				// Token: 0x0400B1A2 RID: 45474
				public static LocString NAME = "Sandstone";

				// Token: 0x0400B1A3 RID: 45475
				public static LocString DESCRIPTION = "A beautiful rock composed of multiple layers of sediment.";

				// Token: 0x0400B1A4 RID: 45476
				public static LocString ARTIFACT = "This sample of sandstone appears to have been processed by the Gravitas Mining Gun that was made available to the general public.\n\nNote: The Gravitas public Mining Gun model is different than ones used by Duplicants in its larger size, and extra precautionary features added in order to be compliant with national safety standards.";
			}

			// Token: 0x02002B0A RID: 11018
			public class MAGMALAMP
			{
				// Token: 0x0400B1A5 RID: 45477
				public static LocString NAME = "Magma Lamp";

				// Token: 0x0400B1A6 RID: 45478
				public static LocString DESCRIPTION = "The sequel to \"Lava Lamp\".";

				// Token: 0x0400B1A7 RID: 45479
				public static LocString ARTIFACT = "Molten lava and obsidian combined in a way that allows the lava to maintain just enough heat to remain in liquid form.\n\nPlans of this lamp found in the Gravitas archives have been attributed to one Robin Nisbet, PhD.";
			}

			// Token: 0x02002B0B RID: 11019
			public class OBELISK
			{
				// Token: 0x0400B1A8 RID: 45480
				public static LocString NAME = "Small Obelisk";

				// Token: 0x0400B1A9 RID: 45481
				public static LocString DESCRIPTION = "A rectangular stone piece.\n\nIts function is unclear.";

				// Token: 0x0400B1AA RID: 45482
				public static LocString ARTIFACT = "On close inspection this rectangle is actually a stone box built with a covert, almost seamless, lid, housing a tiny key.\n\nIt is still unclear what the key unlocks.";
			}

			// Token: 0x02002B0C RID: 11020
			public class RUBIKSCUBE
			{
				// Token: 0x0400B1AB RID: 45483
				public static LocString NAME = "Rubik's Cube";

				// Token: 0x0400B1AC RID: 45484
				public static LocString DESCRIPTION = "This mystery of the universe has already been solved.";

				// Token: 0x0400B1AD RID: 45485
				public static LocString ARTIFACT = "A well-used, competition-compliant version of the popular puzzle cube.\n\nIt's worth noting that Dr. Dylan 'Nails' Winslow was once a regional Rubik's Cube champion.";
			}

			// Token: 0x02002B0D RID: 11021
			public class OFFICEMUG
			{
				// Token: 0x0400B1AE RID: 45486
				public static LocString NAME = "Office Mug";

				// Token: 0x0400B1AF RID: 45487
				public static LocString DESCRIPTION = "An intermediary place to store espresso before you move it to your mouth.";

				// Token: 0x0400B1B0 RID: 45488
				public static LocString ARTIFACT = "An office mug with the Gravitas logo on it. Though their office mugs were all emblazoned with the same logo, Gravitas colored their mugs differently to distinguish between their various departments.\n\nThis one is from the AI department.";
			}

			// Token: 0x02002B0E RID: 11022
			public class AMELIASWATCH
			{
				// Token: 0x0400B1B1 RID: 45489
				public static LocString NAME = "Wrist Watch";

				// Token: 0x0400B1B2 RID: 45490
				public static LocString DESCRIPTION = "It was discovered in a package labeled \"To be entrusted to Dr. Walker\".";

				// Token: 0x0400B1B3 RID: 45491
				public static LocString ARTIFACT = "This watch once belonged to pioneering aviator Amelia Earhart and travelled to space via astronaut Dr. Shannon Walker.\n\nHow it came to be floating in space is a matter of speculation, but perhaps the adventurous spirit of its original stewards became infused within the fabric of this timepiece and compelled the universe to launch it into the great unknown.";
			}

			// Token: 0x02002B0F RID: 11023
			public class MOONMOONMOON
			{
				// Token: 0x0400B1B4 RID: 45492
				public static LocString NAME = "Moonmoonmoon";

				// Token: 0x0400B1B5 RID: 45493
				public static LocString DESCRIPTION = "A moon's moon's moon. It's very small.";

				// Token: 0x0400B1B6 RID: 45494
				public static LocString ARTIFACT = "In contrast to most moons, this object's glowing properties do not come from reflecting an external source of light, but rather from an internal glow of mysterious origin.\n\nThe glow of this object also grants an extraordinary amount of Decor bonus to nearby Duplicants, almost as if it was designed that way.";
			}

			// Token: 0x02002B10 RID: 11024
			public class BIOLUMINESCENTROCK
			{
				// Token: 0x0400B1B7 RID: 45495
				public static LocString NAME = "Bioluminescent Rock";

				// Token: 0x0400B1B8 RID: 45496
				public static LocString DESCRIPTION = "A thriving colony of tiny, microscopic organisms is responsible for giving it its bluish glow.";

				// Token: 0x0400B1B9 RID: 45497
				public static LocString ARTIFACT = "The microscopic organisms within this rock are of a unique variety whose genetic code shows many tell-tale signs of being genetically engineered within a lab.\n\nFurther analysis reveals they share 99.999% of their genetic code with Shine Bugs.";
			}

			// Token: 0x02002B11 RID: 11025
			public class PLASMALAMP
			{
				// Token: 0x0400B1BA RID: 45498
				public static LocString NAME = "Plasma Lamp";

				// Token: 0x0400B1BB RID: 45499
				public static LocString DESCRIPTION = "No space colony is complete without one.";

				// Token: 0x0400B1BC RID: 45500
				public static LocString ARTIFACT = "The bottom of this lamp contains the words 'Property of the Atmospheric Sciences Department'.\n\nIt's worth noting that the Gravitas Atmospheric Sciences Department once simulated an experiment testing the feasibility of survival in an environment filled with noble gasses, similar to the ones contained within this device.";
			}

			// Token: 0x02002B12 RID: 11026
			public class MOLDAVITE
			{
				// Token: 0x0400B1BD RID: 45501
				public static LocString NAME = "Moldavite";

				// Token: 0x0400B1BE RID: 45502
				public static LocString DESCRIPTION = "A unique green stone formed from the impact of a meteorite.";

				// Token: 0x0400B1BF RID: 45503
				public static LocString ARTIFACT = "This extremely rare, museum grade moldavite once sat on the desk of Dr. Ren Sato, but it was stolen by some unknown person.\n\nDr. Sato suspected the perpetrator was none other than Director Stern, but was never able to confirm this theory.";
			}

			// Token: 0x02002B13 RID: 11027
			public class BRICKPHONE
			{
				// Token: 0x0400B1C0 RID: 45504
				public static LocString NAME = "Strange Brick";

				// Token: 0x0400B1C1 RID: 45505
				public static LocString DESCRIPTION = "It still works.";

				// Token: 0x0400B1C2 RID: 45506
				public static LocString ARTIFACT = "This cordless phone once held a direct line to an unknown location in which strange distant voices can be heard but not understood, nor interacted with.\n\nThough Gravitas spent a lot of money and years of study dedicated to discovering its secret, the mystery was never solved.";
			}

			// Token: 0x02002B14 RID: 11028
			public class SOLARSYSTEM
			{
				// Token: 0x0400B1C3 RID: 45507
				public static LocString NAME = "Self-Contained System";

				// Token: 0x0400B1C4 RID: 45508
				public static LocString DESCRIPTION = "A marvel of the cosmos, inside this display is an entirely self-contained solar system.";

				// Token: 0x0400B1C5 RID: 45509
				public static LocString ARTIFACT = "This marvel of a device was built using parts from an old Tornado-in-a-Box science fair project.\n\nVery faint, faded letters are still visible on the display bottom that read 'Camille P. Grade 5'.";
			}

			// Token: 0x02002B15 RID: 11029
			public class SINK
			{
				// Token: 0x0400B1C6 RID: 45510
				public static LocString NAME = "Sink";

				// Token: 0x0400B1C7 RID: 45511
				public static LocString DESCRIPTION = "No collection is complete without it.";

				// Token: 0x0400B1C8 RID: 45512
				public static LocString ARTIFACT = "A small trace of encrusted soap on this sink strongly suggests it was installed in a personal bathroom, rather than a public one which would have used a soap dispenser.\n\nThe soap sliver is light blue and contains a manufactured blueberry fragrance.";
			}

			// Token: 0x02002B16 RID: 11030
			public class ROCKTORNADO
			{
				// Token: 0x0400B1C9 RID: 45513
				public static LocString NAME = "Tornado Rock";

				// Token: 0x0400B1CA RID: 45514
				public static LocString DESCRIPTION = "It's unclear how it formed, although I'm glad it did.";

				// Token: 0x0400B1CB RID: 45515
				public static LocString ARTIFACT = "Speculations about the origin of this rock include a paper written by one Harold P. Moreson, Ph.D. in which he theorized it could be a rare form of hollow geode which failed to form any crystals inside.\n\nThis paper appears in the Gravitas archives, and in all probability, was one of the factors in the hiring of Moreson into the Geology department of the company.";
			}

			// Token: 0x02002B17 RID: 11031
			public class BLENDER
			{
				// Token: 0x0400B1CC RID: 45516
				public static LocString NAME = "Blender";

				// Token: 0x0400B1CD RID: 45517
				public static LocString DESCRIPTION = "Equipment used to conduct experiments answering the age-old question, \"Could that blend\"?";

				// Token: 0x0400B1CE RID: 45518
				public static LocString ARTIFACT = "Trace amounts of edible foodstuffs present in this blender indicate that it was probably used to emulsify the ingredients of a mush bar.\n\nIt is also very likely that it was employed at least once in the production of a peanut butter and banana smoothie.";
			}

			// Token: 0x02002B18 RID: 11032
			public class SAXOPHONE
			{
				// Token: 0x0400B1CF RID: 45519
				public static LocString NAME = "Mangled Saxophone";

				// Token: 0x0400B1D0 RID: 45520
				public static LocString DESCRIPTION = "The name \"Pesquet\" is barely legible on the inside.";

				// Token: 0x0400B1D1 RID: 45521
				public static LocString ARTIFACT = "Though it is often remarked that \"in space, no one can hear you scream\", Thomas Pesquet proved the same cannot be said for the smooth jazzy sounds of a saxophone.\n\nAlthough this instrument once belonged to the eminent French Astronaut its current bumped and bent shape suggests it has seen many adventures beyond that of just being used to perform an out-of-this-world saxophone solo.";
			}

			// Token: 0x02002B19 RID: 11033
			public class STETHOSCOPE
			{
				// Token: 0x0400B1D2 RID: 45522
				public static LocString NAME = "Stethoscope";

				// Token: 0x0400B1D3 RID: 45523
				public static LocString DESCRIPTION = "Listens to Duplicant heartbeats, or gurgly tummies.";

				// Token: 0x0400B1D4 RID: 45524
				public static LocString ARTIFACT = "The size and shape of this stethescope suggests it was not intended to be used by neither a human-sized nor a Duplicant-sized person but something half-way in between the two beings.";
			}

			// Token: 0x02002B1A RID: 11034
			public class VHS
			{
				// Token: 0x0400B1D5 RID: 45525
				public static LocString NAME = "Archaic Tech";

				// Token: 0x0400B1D6 RID: 45526
				public static LocString DESCRIPTION = "Be kind when you handle it. It's very fragile.";

				// Token: 0x0400B1D7 RID: 45527
				public static LocString ARTIFACT = "The label on this VHS tape reads \"Jackie and Olivia's House Warming Party\".\n\nUnfortunately, a device with which to play this recording no longer exists in this universe.";
			}

			// Token: 0x02002B1B RID: 11035
			public class REACTORMODEL
			{
				// Token: 0x0400B1D8 RID: 45528
				public static LocString NAME = "Model Nuclear Power Plant";

				// Token: 0x0400B1D9 RID: 45529
				public static LocString DESCRIPTION = "It's pronounced nu-clear.";

				// Token: 0x0400B1DA RID: 45530
				public static LocString ARTIFACT = "Though this Nuclear Power Plant was never built, this model exists as an artifact to a time early in the life of Gravitas when it was researching all alternatives to solving the global energy problem.\n\nUltimately, the idea of building a Nuclear Power Plant was abandoned in favor of the \"much safer\" alternative of developing the Temporal Bow.";
			}

			// Token: 0x02002B1C RID: 11036
			public class MOODRING
			{
				// Token: 0x0400B1DB RID: 45531
				public static LocString NAME = "Radiation Mood Ring";

				// Token: 0x0400B1DC RID: 45532
				public static LocString DESCRIPTION = "How radioactive are you feeling?";

				// Token: 0x0400B1DD RID: 45533
				public static LocString ARTIFACT = "A wholly unique ring not found anywhere outside of the Gravitas Laboratory.\n\nThough it can't be determined for sure who worked on this extraordinary curiousity it's worth noting that, for his Ph.D. thesis, Dr. Travaldo Farrington wrote a paper entitled \"Novelty Uses for Radiochromatic Dyes\".";
			}

			// Token: 0x02002B1D RID: 11037
			public class ORACLE
			{
				// Token: 0x0400B1DE RID: 45534
				public static LocString NAME = "Useless Machine";

				// Token: 0x0400B1DF RID: 45535
				public static LocString DESCRIPTION = "What does it do?";

				// Token: 0x0400B1E0 RID: 45536
				public static LocString ARTIFACT = "All of the parts for this contraption are recycled from projects abandoned by the Robotics department.\n\nThe design is very close to one published in an amateur DIY magazine that once sat in the lobby of the 'Employees Only' area of Gravitas' facilities.";
			}

			// Token: 0x02002B1E RID: 11038
			public class GRUBSTATUE
			{
				// Token: 0x0400B1E1 RID: 45537
				public static LocString NAME = "Grubgrub Statue";

				// Token: 0x0400B1E2 RID: 45538
				public static LocString DESCRIPTION = "A moving tribute to a tiny plant hugger.";

				// Token: 0x0400B1E3 RID: 45539
				public static LocString ARTIFACT = "It's very likely this statue was placed in a hidden, secluded place in the Gravitas laboratory since the creation of Grubgrubs was a closely held secret that the general public was not privy to.\n\nThis is a shame since the artistic quality of this statue is really quite accomplished.";
			}

			// Token: 0x02002B1F RID: 11039
			public class HONEYJAR
			{
				// Token: 0x0400B1E4 RID: 45540
				public static LocString NAME = "Honey Jar";

				// Token: 0x0400B1E5 RID: 45541
				public static LocString DESCRIPTION = "Sweet golden liquid with just a touch of uranium.";

				// Token: 0x0400B1E6 RID: 45542
				public static LocString ARTIFACT = "Records from the Genetics and Biology Lab of the Gravitas facility show that several early iterations of a radioactive Bee would continue to produce honey and that this honey was once accidentally stored in the employee kitchen which resulted in several incidents of minor radiation poisoning when it was erroneously labled as a sweetener for tea.\n\nEmployees who used this product reported that it was the \"sweetest honey they'd ever tasted\" and expressed no regret at the mix-up.";
			}

			// Token: 0x02002B20 RID: 11040
			public class PLASTICFLOWERS
			{
				// Token: 0x0400B1E7 RID: 45543
				public static LocString NAME = "Plastic Flowers";

				// Token: 0x0400B1E8 RID: 45544
				public static LocString DESCRIPTION = "Maintenance-free blooms that will outlast us all.";

				// Token: 0x0400B1E9 RID: 45545
				public static LocString ARTIFACT = "Manufactured and sold by a home staging company hired by Gravitas to \"make Space feel more like home.\"\n\nThis bouquet is designed to smell like freshly baked cookies.";
			}

			// Token: 0x02002B21 RID: 11041
			public class FOUNTAINPEN
			{
				// Token: 0x0400B1EA RID: 45546
				public static LocString NAME = "Fountain Pen";

				// Token: 0x0400B1EB RID: 45547
				public static LocString DESCRIPTION = "It cuts through red tape better than a sword ever could.";

				// Token: 0x0400B1EC RID: 45548
				public static LocString ARTIFACT = "The handcrafted gold nib features a triangular logo with the letters V and I inside.\n\nIts owner was too proud to report it stolen, and would be shocked to learn of its whereabouts.";
			}
		}

		// Token: 0x02002B22 RID: 11042
		public class KEEPSAKES
		{
			// Token: 0x02002B23 RID: 11043
			public class CRITTER_MANIPULATOR
			{
				// Token: 0x0400B1ED RID: 45549
				public static LocString NAME = "Ceramic Morb";

				// Token: 0x0400B1EE RID: 45550
				public static LocString DESCRIPTION = "A pottery project produced in an HR-mandated art therapy class.\n\nIt's glazed with a substance that once landed a curious lab technician in the ER.";
			}

			// Token: 0x02002B24 RID: 11044
			public class MEGA_BRAIN
			{
				// Token: 0x0400B1EF RID: 45551
				public static LocString NAME = "Model Plane";

				// Token: 0x0400B1F0 RID: 45552
				public static LocString DESCRIPTION = "A treasured souvenir that was once a common accompaniment to children's meals during commercial flights. There's a hole in the bottom from when Dr. Holland had it mounted on a stand.";
			}

			// Token: 0x02002B25 RID: 11045
			public class LONELY_MINION
			{
				// Token: 0x0400B1F1 RID: 45553
				public static LocString NAME = "Rusty Toolbox";

				// Token: 0x0400B1F2 RID: 45554
				public static LocString DESCRIPTION = "On the inside of the lid, someone used a screwdriver to carve a drawing of a group of smiling Duplicants gathered around a massive crater.";
			}

			// Token: 0x02002B26 RID: 11046
			public class FOSSIL_HUNT
			{
				// Token: 0x0400B1F3 RID: 45555
				public static LocString NAME = "Critter Collar";

				// Token: 0x0400B1F4 RID: 45556
				public static LocString DESCRIPTION = "The tag reads \"Molly\".\n\nOn the reverse is \"Designed by B363\" stamped above what appears to be an unusually shaped pawprint.";
			}

			// Token: 0x02002B27 RID: 11047
			public class MORB_ROVER_MAKER
			{
				// Token: 0x0400B1F5 RID: 45557
				public static LocString NAME = "Toy Bot";

				// Token: 0x0400B1F6 RID: 45558
				public static LocString DESCRIPTION = "A custom-made robot programmed to deliver puns in a variety of celebrity voices.\n\nIt is also a paper shredder.";
			}

			// Token: 0x02002B28 RID: 11048
			public class GEOTHERMAL_PLANT
			{
				// Token: 0x0400B1F7 RID: 45559
				public static LocString NAME = "Shiny Coprolite";

				// Token: 0x0400B1F8 RID: 45560
				public static LocString DESCRIPTION = "A spectacular sample of organic material fossilized into lead.\n\nSome things really <i>do</i> get better with age.";
			}
		}

		// Token: 0x02002B29 RID: 11049
		public class SANDBOXTOOLS
		{
			// Token: 0x02002B2A RID: 11050
			public class SETTINGS
			{
				// Token: 0x02002B2B RID: 11051
				public class INSTANT_BUILD
				{
					// Token: 0x0400B1F9 RID: 45561
					public static LocString NAME = "Instant build mode ON";

					// Token: 0x0400B1FA RID: 45562
					public static LocString TOOLTIP = "Toggle between placing construction plans and fully built buildings";
				}

				// Token: 0x02002B2C RID: 11052
				public class BRUSH_SIZE
				{
					// Token: 0x0400B1FB RID: 45563
					public static LocString NAME = "Size";

					// Token: 0x0400B1FC RID: 45564
					public static LocString TOOLTIP = "Adjust brush size";
				}

				// Token: 0x02002B2D RID: 11053
				public class BRUSH_NOISE_SCALE
				{
					// Token: 0x0400B1FD RID: 45565
					public static LocString NAME = "Noise A";

					// Token: 0x0400B1FE RID: 45566
					public static LocString TOOLTIP = "Adjust brush noisiness A";
				}

				// Token: 0x02002B2E RID: 11054
				public class BRUSH_NOISE_DENSITY
				{
					// Token: 0x0400B1FF RID: 45567
					public static LocString NAME = "Noise B";

					// Token: 0x0400B200 RID: 45568
					public static LocString TOOLTIP = "Adjust brush noisiness B";
				}

				// Token: 0x02002B2F RID: 11055
				public class TEMPERATURE
				{
					// Token: 0x0400B201 RID: 45569
					public static LocString NAME = "Temperature";

					// Token: 0x0400B202 RID: 45570
					public static LocString TOOLTIP = "Adjust absolute temperature";
				}

				// Token: 0x02002B30 RID: 11056
				public class TEMPERATURE_ADDITIVE
				{
					// Token: 0x0400B203 RID: 45571
					public static LocString NAME = "Temperature";

					// Token: 0x0400B204 RID: 45572
					public static LocString TOOLTIP = "Adjust additive temperature";
				}

				// Token: 0x02002B31 RID: 11057
				public class RADIATION
				{
					// Token: 0x0400B205 RID: 45573
					public static LocString NAME = "Absolute radiation";

					// Token: 0x0400B206 RID: 45574
					public static LocString TOOLTIP = "Adjust absolute radiation";
				}

				// Token: 0x02002B32 RID: 11058
				public class RADIATION_ADDITIVE
				{
					// Token: 0x0400B207 RID: 45575
					public static LocString NAME = "Additive radiation";

					// Token: 0x0400B208 RID: 45576
					public static LocString TOOLTIP = "Adjust additive radiation";
				}

				// Token: 0x02002B33 RID: 11059
				public class STRESS_ADDITIVE
				{
					// Token: 0x0400B209 RID: 45577
					public static LocString NAME = "Reduce Stress";

					// Token: 0x0400B20A RID: 45578
					public static LocString TOOLTIP = "Adjust stress reduction";
				}

				// Token: 0x02002B34 RID: 11060
				public class MORALE
				{
					// Token: 0x0400B20B RID: 45579
					public static LocString NAME = "Adjust Morale";

					// Token: 0x0400B20C RID: 45580
					public static LocString TOOLTIP = "Bonus Morale adjustment";
				}

				// Token: 0x02002B35 RID: 11061
				public class MASS
				{
					// Token: 0x0400B20D RID: 45581
					public static LocString NAME = "Mass";

					// Token: 0x0400B20E RID: 45582
					public static LocString TOOLTIP = "Adjust mass";
				}

				// Token: 0x02002B36 RID: 11062
				public class DISEASE
				{
					// Token: 0x0400B20F RID: 45583
					public static LocString NAME = "Germ";

					// Token: 0x0400B210 RID: 45584
					public static LocString TOOLTIP = "Adjust type of germ";
				}

				// Token: 0x02002B37 RID: 11063
				public class DISEASE_COUNT
				{
					// Token: 0x0400B211 RID: 45585
					public static LocString NAME = "Germs";

					// Token: 0x0400B212 RID: 45586
					public static LocString TOOLTIP = "Adjust germ count";
				}

				// Token: 0x02002B38 RID: 11064
				public class BRUSH
				{
					// Token: 0x0400B213 RID: 45587
					public static LocString NAME = "Brush";

					// Token: 0x0400B214 RID: 45588
					public static LocString TOOLTIP = "Paint elements into the world simulation {Hotkey}";
				}

				// Token: 0x02002B39 RID: 11065
				public class ELEMENT
				{
					// Token: 0x0400B215 RID: 45589
					public static LocString NAME = "Element";

					// Token: 0x0400B216 RID: 45590
					public static LocString TOOLTIP = "Adjust type of element";
				}

				// Token: 0x02002B3A RID: 11066
				public class SPRINKLE
				{
					// Token: 0x0400B217 RID: 45591
					public static LocString NAME = "Sprinkle";

					// Token: 0x0400B218 RID: 45592
					public static LocString TOOLTIP = "Paint elements into the simulation using noise {Hotkey}";
				}

				// Token: 0x02002B3B RID: 11067
				public class FLOOD
				{
					// Token: 0x0400B219 RID: 45593
					public static LocString NAME = "Fill";

					// Token: 0x0400B21A RID: 45594
					public static LocString TOOLTIP = "Fill a section of the simulation with the chosen element {Hotkey}";
				}

				// Token: 0x02002B3C RID: 11068
				public class SAMPLE
				{
					// Token: 0x0400B21B RID: 45595
					public static LocString NAME = "Sample";

					// Token: 0x0400B21C RID: 45596
					public static LocString TOOLTIP = "Copy the settings from a cell to use with brush tools {Hotkey}";
				}

				// Token: 0x02002B3D RID: 11069
				public class HEATGUN
				{
					// Token: 0x0400B21D RID: 45597
					public static LocString NAME = "Heat Gun";

					// Token: 0x0400B21E RID: 45598
					public static LocString TOOLTIP = "Inject thermal energy into the simulation {Hotkey}";
				}

				// Token: 0x02002B3E RID: 11070
				public class RADSTOOL
				{
					// Token: 0x0400B21F RID: 45599
					public static LocString NAME = "Radiation Tool";

					// Token: 0x0400B220 RID: 45600
					public static LocString TOOLTIP = "Inject or remove radiation from the simulation {Hotkey}";
				}

				// Token: 0x02002B3F RID: 11071
				public class SPAWNER
				{
					// Token: 0x0400B221 RID: 45601
					public static LocString NAME = "Spawner";

					// Token: 0x0400B222 RID: 45602
					public static LocString TOOLTIP = "Spawn critters, food, equipment, and other entities {Hotkey}";
				}

				// Token: 0x02002B40 RID: 11072
				public class STRESS
				{
					// Token: 0x0400B223 RID: 45603
					public static LocString NAME = "Stress";

					// Token: 0x0400B224 RID: 45604
					public static LocString TOOLTIP = "Manage Duplicants' stress levels {Hotkey}";
				}

				// Token: 0x02002B41 RID: 11073
				public class CLEAR_FLOOR
				{
					// Token: 0x0400B225 RID: 45605
					public static LocString NAME = "Clear Debris";

					// Token: 0x0400B226 RID: 45606
					public static LocString TOOLTIP = "Delete loose items cluttering the floor {Hotkey}";
				}

				// Token: 0x02002B42 RID: 11074
				public class DESTROY
				{
					// Token: 0x0400B227 RID: 45607
					public static LocString NAME = "Destroy";

					// Token: 0x0400B228 RID: 45608
					public static LocString TOOLTIP = "Delete everything in the selected cell(s) {Hotkey}";
				}

				// Token: 0x02002B43 RID: 11075
				public class SPAWN_ENTITY
				{
					// Token: 0x0400B229 RID: 45609
					public static LocString NAME = "Spawn";
				}

				// Token: 0x02002B44 RID: 11076
				public class FOW
				{
					// Token: 0x0400B22A RID: 45610
					public static LocString NAME = "Reveal";

					// Token: 0x0400B22B RID: 45611
					public static LocString TOOLTIP = "Dispel the Fog of War shrouding the map {Hotkey}";
				}

				// Token: 0x02002B45 RID: 11077
				public class CRITTER
				{
					// Token: 0x0400B22C RID: 45612
					public static LocString NAME = "Critter Removal";

					// Token: 0x0400B22D RID: 45613
					public static LocString TOOLTIP = "Remove critters! {Hotkey}";
				}

				// Token: 0x02002B46 RID: 11078
				public class SPAWN_STORY_TRAIT
				{
					// Token: 0x0400B22E RID: 45614
					public static LocString NAME = "Story Traits";

					// Token: 0x0400B22F RID: 45615
					public static LocString TOOLTIP = "Spawn story traits {Hotkey}";
				}
			}

			// Token: 0x02002B47 RID: 11079
			public class FILTERS
			{
				// Token: 0x0400B230 RID: 45616
				public static LocString BACK = "Back";

				// Token: 0x0400B231 RID: 45617
				public static LocString COMMON = "Common Substances";

				// Token: 0x0400B232 RID: 45618
				public static LocString SOLID = "Solids";

				// Token: 0x0400B233 RID: 45619
				public static LocString LIQUID = "Liquids";

				// Token: 0x0400B234 RID: 45620
				public static LocString GAS = "Gases";

				// Token: 0x02002B48 RID: 11080
				public class ENTITIES
				{
					// Token: 0x0400B235 RID: 45621
					public static LocString BIONICUPGRADES = "Boosters";

					// Token: 0x0400B236 RID: 45622
					public static LocString SPECIAL = "Special";

					// Token: 0x0400B237 RID: 45623
					public static LocString GRAVITAS = "Gravitas";

					// Token: 0x0400B238 RID: 45624
					public static LocString PLANTS = "Plants";

					// Token: 0x0400B239 RID: 45625
					public static LocString SEEDS = "Seeds";

					// Token: 0x0400B23A RID: 45626
					public static LocString CREATURE = "Critters";

					// Token: 0x0400B23B RID: 45627
					public static LocString CREATURE_EGG = "Eggs";

					// Token: 0x0400B23C RID: 45628
					public static LocString FOOD = "Foods";

					// Token: 0x0400B23D RID: 45629
					public static LocString EQUIPMENT = "Equipment";

					// Token: 0x0400B23E RID: 45630
					public static LocString GEYSERS = "Geysers";

					// Token: 0x0400B23F RID: 45631
					public static LocString EXPERIMENTS = "Experimental";

					// Token: 0x0400B240 RID: 45632
					public static LocString INDUSTRIAL_PRODUCTS = "Industrial";

					// Token: 0x0400B241 RID: 45633
					public static LocString COMETS = "Meteors";

					// Token: 0x0400B242 RID: 45634
					public static LocString ARTIFACTS = "Artifacts";

					// Token: 0x0400B243 RID: 45635
					public static LocString STORYTRAITS = "Story Traits";
				}
			}

			// Token: 0x02002B49 RID: 11081
			public class CLEARFLOOR
			{
				// Token: 0x0400B244 RID: 45636
				public static LocString DELETED = "Deleted";
			}
		}

		// Token: 0x02002B4A RID: 11082
		public class RETIRED_COLONY_INFO_SCREEN
		{
			// Token: 0x0400B245 RID: 45637
			public static LocString SECONDS = "Seconds";

			// Token: 0x0400B246 RID: 45638
			public static LocString CYCLES = "Cycles";

			// Token: 0x0400B247 RID: 45639
			public static LocString CYCLE_COUNT = "Cycle Count: {0}";

			// Token: 0x0400B248 RID: 45640
			public static LocString DUPLICANT_AGE = "Age: {0} cycles";

			// Token: 0x0400B249 RID: 45641
			public static LocString SKILL_LEVEL = "Skill Level: {0}";

			// Token: 0x0400B24A RID: 45642
			public static LocString BUILDING_COUNT = "Count: {0}";

			// Token: 0x0400B24B RID: 45643
			public static LocString PREVIEW_UNAVAILABLE = "Preview\nUnavailable";

			// Token: 0x0400B24C RID: 45644
			public static LocString TIMELAPSE_UNAVAILABLE = "Timelapse\nUnavailable";

			// Token: 0x0400B24D RID: 45645
			public static LocString SEARCH = "SEARCH...";

			// Token: 0x02002B4B RID: 11083
			public class BUTTONS
			{
				// Token: 0x0400B24E RID: 45646
				public static LocString RETURN_TO_GAME = "RETURN TO GAME";

				// Token: 0x0400B24F RID: 45647
				public static LocString VIEW_OTHER_COLONIES = "BACK";

				// Token: 0x0400B250 RID: 45648
				public static LocString QUIT_TO_MENU = "QUIT TO MAIN MENU";

				// Token: 0x0400B251 RID: 45649
				public static LocString CLOSE = "CLOSE";
			}

			// Token: 0x02002B4C RID: 11084
			public class TITLES
			{
				// Token: 0x0400B252 RID: 45650
				public static LocString EXPLORER_HEADER = "COLONIES";

				// Token: 0x0400B253 RID: 45651
				public static LocString RETIRED_COLONIES = "Colony Summaries";

				// Token: 0x0400B254 RID: 45652
				public static LocString COLONY_STATISTICS = "Colony Statistics";

				// Token: 0x0400B255 RID: 45653
				public static LocString DUPLICANTS = "Duplicants";

				// Token: 0x0400B256 RID: 45654
				public static LocString BUILDINGS = "Buildings";

				// Token: 0x0400B257 RID: 45655
				public static LocString CHEEVOS = "Colony Achievements";

				// Token: 0x0400B258 RID: 45656
				public static LocString ACHIEVEMENT_HEADER = "ACHIEVEMENTS";

				// Token: 0x0400B259 RID: 45657
				public static LocString TIMELAPSE = "Timelapse";
			}

			// Token: 0x02002B4D RID: 11085
			public class STATS
			{
				// Token: 0x0400B25A RID: 45658
				public static LocString OXYGEN_CREATED = "Total Oxygen Produced";

				// Token: 0x0400B25B RID: 45659
				public static LocString OXYGEN_CONSUMED = "Total Oxygen Consumed";

				// Token: 0x0400B25C RID: 45660
				public static LocString POWER_CREATED = "Average Power Produced";

				// Token: 0x0400B25D RID: 45661
				public static LocString POWER_WASTED = "Average Power Wasted";

				// Token: 0x0400B25E RID: 45662
				public static LocString TRAVEL_TIME = "Total Travel Time";

				// Token: 0x0400B25F RID: 45663
				public static LocString WORK_TIME = "Total Work Time";

				// Token: 0x0400B260 RID: 45664
				public static LocString AVERAGE_TRAVEL_TIME = "Average Travel Time";

				// Token: 0x0400B261 RID: 45665
				public static LocString AVERAGE_WORK_TIME = "Average Work Time";

				// Token: 0x0400B262 RID: 45666
				public static LocString CALORIES_CREATED = "Calorie Generation";

				// Token: 0x0400B263 RID: 45667
				public static LocString CALORIES_CONSUMED = "Calorie Consumption";

				// Token: 0x0400B264 RID: 45668
				public static LocString LIVE_DUPLICANTS = "Duplicants";

				// Token: 0x0400B265 RID: 45669
				public static LocString AVERAGE_STRESS_CREATED = "Average Stress Created";

				// Token: 0x0400B266 RID: 45670
				public static LocString AVERAGE_STRESS_REMOVED = "Average Stress Removed";

				// Token: 0x0400B267 RID: 45671
				public static LocString NUMBER_DOMESTICATED_CRITTERS = "Domesticated Critters";

				// Token: 0x0400B268 RID: 45672
				public static LocString NUMBER_WILD_CRITTERS = "Wild Critters";

				// Token: 0x0400B269 RID: 45673
				public static LocString AVERAGE_GERMS = "Average Germs";

				// Token: 0x0400B26A RID: 45674
				public static LocString ROCKET_MISSIONS = "Rocket Missions Underway";
			}
		}

		// Token: 0x02002B4E RID: 11086
		public class DROPDOWN
		{
			// Token: 0x0400B26B RID: 45675
			public static LocString NONE = "Unassigned";
		}

		// Token: 0x02002B4F RID: 11087
		public class FRONTEND
		{
			// Token: 0x0400B26C RID: 45676
			public static LocString GAME_VERSION = "Game Version: ";

			// Token: 0x0400B26D RID: 45677
			public static LocString LOADING = "Loading...";

			// Token: 0x0400B26E RID: 45678
			public static LocString DONE_BUTTON = "DONE";

			// Token: 0x02002B50 RID: 11088
			public class DEMO_OVER_SCREEN
			{
				// Token: 0x0400B26F RID: 45679
				public static LocString TITLE = "Thanks for playing!";

				// Token: 0x0400B270 RID: 45680
				public static LocString BODY = "Thank you for playing the demo for Oxygen Not Included!\n\nThis game is still in development.\n\nGo to kleigames.com/o2 or ask one of us if you'd like more information.";

				// Token: 0x0400B271 RID: 45681
				public static LocString BUTTON_EXIT_TO_MENU = "EXIT TO MENU";
			}

			// Token: 0x02002B51 RID: 11089
			public class CUSTOMGAMESETTINGSSCREEN
			{
				// Token: 0x02002B52 RID: 11090
				public class SETTINGS
				{
					// Token: 0x02002B53 RID: 11091
					public class SANDBOXMODE
					{
						// Token: 0x0400B272 RID: 45682
						public static LocString NAME = "Sandbox Mode";

						// Token: 0x0400B273 RID: 45683
						public static LocString TOOLTIP = "Manipulate and customize the simulation with tools that ignore regular game constraints";

						// Token: 0x02002B54 RID: 11092
						public static class LEVELS
						{
							// Token: 0x02002B55 RID: 11093
							public static class DISABLED
							{
								// Token: 0x0400B274 RID: 45684
								public static LocString NAME = "Disabled";

								// Token: 0x0400B275 RID: 45685
								public static LocString TOOLTIP = "Unchecked: Sandbox Mode is turned off (Default)";
							}

							// Token: 0x02002B56 RID: 11094
							public static class ENABLED
							{
								// Token: 0x0400B276 RID: 45686
								public static LocString NAME = "Enabled";

								// Token: 0x0400B277 RID: 45687
								public static LocString TOOLTIP = "Checked: Sandbox Mode is turned on";
							}
						}
					}

					// Token: 0x02002B57 RID: 11095
					public class FASTWORKERSMODE
					{
						// Token: 0x0400B278 RID: 45688
						public static LocString NAME = "Fast Workers Mode";

						// Token: 0x0400B279 RID: 45689
						public static LocString TOOLTIP = "Duplicants will finish most work immediately and require little sleep";

						// Token: 0x02002B58 RID: 11096
						public static class LEVELS
						{
							// Token: 0x02002B59 RID: 11097
							public static class DISABLED
							{
								// Token: 0x0400B27A RID: 45690
								public static LocString NAME = "Disabled";

								// Token: 0x0400B27B RID: 45691
								public static LocString TOOLTIP = "Unchecked: Fast Workers Mode is turned off (Default)";
							}

							// Token: 0x02002B5A RID: 11098
							public static class ENABLED
							{
								// Token: 0x0400B27C RID: 45692
								public static LocString NAME = "Enabled";

								// Token: 0x0400B27D RID: 45693
								public static LocString TOOLTIP = "Checked: Fast Workers Mode is turned on";
							}
						}
					}

					// Token: 0x02002B5B RID: 11099
					public class EXPANSION1ACTIVE
					{
						// Token: 0x0400B27E RID: 45694
						public static LocString NAME = UI.DLC1.NAME_ITAL + " Content Enabled";

						// Token: 0x0400B27F RID: 45695
						public static LocString TOOLTIP = "If checked, content from the " + UI.DLC1.NAME_ITAL + " Expansion will be available";

						// Token: 0x02002B5C RID: 11100
						public static class LEVELS
						{
							// Token: 0x02002B5D RID: 11101
							public static class DISABLED
							{
								// Token: 0x0400B280 RID: 45696
								public static LocString NAME = "Disabled";

								// Token: 0x0400B281 RID: 45697
								public static LocString TOOLTIP = "Unchecked: " + UI.DLC1.NAME_ITAL + " Content is turned off (Default)";
							}

							// Token: 0x02002B5E RID: 11102
							public static class ENABLED
							{
								// Token: 0x0400B282 RID: 45698
								public static LocString NAME = "Enabled";

								// Token: 0x0400B283 RID: 45699
								public static LocString TOOLTIP = "Checked: " + UI.DLC1.NAME_ITAL + " Content is turned on";
							}
						}
					}

					// Token: 0x02002B5F RID: 11103
					public class SAVETOCLOUD
					{
						// Token: 0x0400B284 RID: 45700
						public static LocString NAME = "Save To Cloud";

						// Token: 0x0400B285 RID: 45701
						public static LocString TOOLTIP = "This colony will be created in the cloud saves folder, and synced by the game platform.";

						// Token: 0x0400B286 RID: 45702
						public static LocString TOOLTIP_LOCAL = "This colony will be created in the local saves folder. It will not be a cloud save and will not be synced by the game platform.";

						// Token: 0x0400B287 RID: 45703
						public static LocString TOOLTIP_EXTRA = "This can be changed later with the colony management options in the load screen, from the main menu.";

						// Token: 0x02002B60 RID: 11104
						public static class LEVELS
						{
							// Token: 0x02002B61 RID: 11105
							public static class DISABLED
							{
								// Token: 0x0400B288 RID: 45704
								public static LocString NAME = "Disabled";

								// Token: 0x0400B289 RID: 45705
								public static LocString TOOLTIP = "Unchecked: This colony will be a local save";
							}

							// Token: 0x02002B62 RID: 11106
							public static class ENABLED
							{
								// Token: 0x0400B28A RID: 45706
								public static LocString NAME = "Enabled";

								// Token: 0x0400B28B RID: 45707
								public static LocString TOOLTIP = "Checked: This colony will be a cloud save (Default)";
							}
						}
					}

					// Token: 0x02002B63 RID: 11107
					public class CAREPACKAGES
					{
						// Token: 0x0400B28C RID: 45708
						public static LocString NAME = "Care Packages";

						// Token: 0x0400B28D RID: 45709
						public static LocString TOOLTIP = "Affects what resources can be printed from the Printing Pod";

						// Token: 0x02002B64 RID: 11108
						public static class LEVELS
						{
							// Token: 0x02002B65 RID: 11109
							public static class NORMAL
							{
								// Token: 0x0400B28E RID: 45710
								public static LocString NAME = "All";

								// Token: 0x0400B28F RID: 45711
								public static LocString TOOLTIP = "Checked: The Printing Pod will offer both Duplicant blueprints and care packages (Default)";
							}

							// Token: 0x02002B66 RID: 11110
							public static class DUPLICANTS_ONLY
							{
								// Token: 0x0400B290 RID: 45712
								public static LocString NAME = "Duplicants Only";

								// Token: 0x0400B291 RID: 45713
								public static LocString TOOLTIP = "Unchecked: The Printing Pod will only offer Duplicant blueprints";
							}
						}
					}

					// Token: 0x02002B67 RID: 11111
					public class IMMUNESYSTEM
					{
						// Token: 0x0400B292 RID: 45714
						public static LocString NAME = "Disease";

						// Token: 0x0400B293 RID: 45715
						public static LocString TOOLTIP = "Affects Duplicants' chances of contracting a disease after germ exposure";

						// Token: 0x02002B68 RID: 11112
						public static class LEVELS
						{
							// Token: 0x02002B69 RID: 11113
							public static class COMPROMISED
							{
								// Token: 0x0400B294 RID: 45716
								public static LocString NAME = "Outbreak Prone";

								// Token: 0x0400B295 RID: 45717
								public static LocString TOOLTIP = "The whole colony will be ravaged by plague if a Duplicant so much as sneezes funny";

								// Token: 0x0400B296 RID: 45718
								public static LocString ATTRIBUTE_MODIFIER_NAME = "Outbreak Prone (Highest Difficulty)";
							}

							// Token: 0x02002B6A RID: 11114
							public static class WEAK
							{
								// Token: 0x0400B297 RID: 45719
								public static LocString NAME = "Germ Susceptible";

								// Token: 0x0400B298 RID: 45720
								public static LocString TOOLTIP = "These Duplicants have an increased chance of contracting diseases from germ exposure";

								// Token: 0x0400B299 RID: 45721
								public static LocString ATTRIBUTE_MODIFIER_NAME = "Germ Susceptibility (Difficulty Up)";
							}

							// Token: 0x02002B6B RID: 11115
							public static class DEFAULT
							{
								// Token: 0x0400B29A RID: 45722
								public static LocString NAME = "Default";

								// Token: 0x0400B29B RID: 45723
								public static LocString TOOLTIP = "Default disease chance";
							}

							// Token: 0x02002B6C RID: 11116
							public static class STRONG
							{
								// Token: 0x0400B29C RID: 45724
								public static LocString NAME = "Germ Resistant";

								// Token: 0x0400B29D RID: 45725
								public static LocString TOOLTIP = "These Duplicants have a decreased chance of contracting diseases from germ exposure";

								// Token: 0x0400B29E RID: 45726
								public static LocString ATTRIBUTE_MODIFIER_NAME = "Germ Resistance (Difficulty Down)";
							}

							// Token: 0x02002B6D RID: 11117
							public static class INVINCIBLE
							{
								// Token: 0x0400B29F RID: 45727
								public static LocString NAME = "Total Immunity";

								// Token: 0x0400B2A0 RID: 45728
								public static LocString TOOLTIP = "Like diplomatic immunity, but without the diplomacy. These Duplicants will never get sick";

								// Token: 0x0400B2A1 RID: 45729
								public static LocString ATTRIBUTE_MODIFIER_NAME = "Total Immunity (No Disease)";
							}
						}
					}

					// Token: 0x02002B6E RID: 11118
					public class MORALE
					{
						// Token: 0x0400B2A2 RID: 45730
						public static LocString NAME = "Morale";

						// Token: 0x0400B2A3 RID: 45731
						public static LocString TOOLTIP = "Adjusts the minimum morale Duplicants must maintain to avoid gaining stress";

						// Token: 0x02002B6F RID: 11119
						public static class LEVELS
						{
							// Token: 0x02002B70 RID: 11120
							public static class VERYHARD
							{
								// Token: 0x0400B2A4 RID: 45732
								public static LocString NAME = "Draconian";

								// Token: 0x0400B2A5 RID: 45733
								public static LocString TOOLTIP = "The finest of the finest can barely keep up with these Duplicants' stringent demands";

								// Token: 0x0400B2A6 RID: 45734
								public static LocString ATTRIBUTE_MODIFIER_NAME = "Draconian (Highest Difficulty)";
							}

							// Token: 0x02002B71 RID: 11121
							public static class HARD
							{
								// Token: 0x0400B2A7 RID: 45735
								public static LocString NAME = "A Bit Persnickety";

								// Token: 0x0400B2A8 RID: 45736
								public static LocString TOOLTIP = "Duplicants require higher morale than usual to fend off stress";

								// Token: 0x0400B2A9 RID: 45737
								public static LocString ATTRIBUTE_MODIFIER_NAME = "A Bit Persnickety (Difficulty Up)";
							}

							// Token: 0x02002B72 RID: 11122
							public static class DEFAULT
							{
								// Token: 0x0400B2AA RID: 45738
								public static LocString NAME = "Default";

								// Token: 0x0400B2AB RID: 45739
								public static LocString TOOLTIP = "Default morale needs";
							}

							// Token: 0x02002B73 RID: 11123
							public static class EASY
							{
								// Token: 0x0400B2AC RID: 45740
								public static LocString NAME = "Chill";

								// Token: 0x0400B2AD RID: 45741
								public static LocString TOOLTIP = "Duplicants require lower morale than usual to fend off stress";

								// Token: 0x0400B2AE RID: 45742
								public static LocString ATTRIBUTE_MODIFIER_NAME = "Chill (Difficulty Down)";
							}

							// Token: 0x02002B74 RID: 11124
							public static class DISABLED
							{
								// Token: 0x0400B2AF RID: 45743
								public static LocString NAME = "Totally Blasé";

								// Token: 0x0400B2B0 RID: 45744
								public static LocString TOOLTIP = "These Duplicants have zero standards and will never gain stress, regardless of their morale";

								// Token: 0x0400B2B1 RID: 45745
								public static LocString ATTRIBUTE_MODIFIER_NAME = "Totally Blasé (No Morale)";
							}
						}
					}

					// Token: 0x02002B75 RID: 11125
					public class CALORIE_BURN
					{
						// Token: 0x0400B2B2 RID: 45746
						public static LocString NAME = "Hunger";

						// Token: 0x0400B2B3 RID: 45747
						public static LocString TOOLTIP = "Affects how quickly Duplicants burn calories and become hungry";

						// Token: 0x02002B76 RID: 11126
						public static class LEVELS
						{
							// Token: 0x02002B77 RID: 11127
							public static class VERYHARD
							{
								// Token: 0x0400B2B4 RID: 45748
								public static LocString NAME = "Ravenous";

								// Token: 0x0400B2B5 RID: 45749
								public static LocString TOOLTIP = "Your Duplicants are on a see-food diet... They see food and they eat it";

								// Token: 0x0400B2B6 RID: 45750
								public static LocString ATTRIBUTE_MODIFIER_NAME = "Ravenous (Highest Difficulty)";
							}

							// Token: 0x02002B78 RID: 11128
							public static class HARD
							{
								// Token: 0x0400B2B7 RID: 45751
								public static LocString NAME = "Rumbly Tummies";

								// Token: 0x0400B2B8 RID: 45752
								public static LocString TOOLTIP = "Duplicants burn calories quickly and require more feeding than usual";

								// Token: 0x0400B2B9 RID: 45753
								public static LocString ATTRIBUTE_MODIFIER_NAME = "Rumbly Tummies (Difficulty Up)";
							}

							// Token: 0x02002B79 RID: 11129
							public static class DEFAULT
							{
								// Token: 0x0400B2BA RID: 45754
								public static LocString NAME = "Default";

								// Token: 0x0400B2BB RID: 45755
								public static LocString TOOLTIP = "Default calorie burn rate";
							}

							// Token: 0x02002B7A RID: 11130
							public static class EASY
							{
								// Token: 0x0400B2BC RID: 45756
								public static LocString NAME = "Fasting";

								// Token: 0x0400B2BD RID: 45757
								public static LocString TOOLTIP = "Duplicants burn calories slowly and get by with fewer meals";

								// Token: 0x0400B2BE RID: 45758
								public static LocString ATTRIBUTE_MODIFIER_NAME = "Fasting (Difficulty Down)";
							}

							// Token: 0x02002B7B RID: 11131
							public static class DISABLED
							{
								// Token: 0x0400B2BF RID: 45759
								public static LocString NAME = "Tummyless";

								// Token: 0x0400B2C0 RID: 45760
								public static LocString TOOLTIP = "These Duplicants were printed without tummies and need no food at all";

								// Token: 0x0400B2C1 RID: 45761
								public static LocString ATTRIBUTE_MODIFIER_NAME = "Tummyless (No Hunger)";
							}
						}
					}

					// Token: 0x02002B7C RID: 11132
					public class WORLD_CHOICE
					{
						// Token: 0x0400B2C2 RID: 45762
						public static LocString NAME = "World";

						// Token: 0x0400B2C3 RID: 45763
						public static LocString TOOLTIP = "New worlds added by mods can be selected here";
					}

					// Token: 0x02002B7D RID: 11133
					public class CLUSTER_CHOICE
					{
						// Token: 0x0400B2C4 RID: 45764
						public static LocString NAME = "Asteroid Belt";

						// Token: 0x0400B2C5 RID: 45765
						public static LocString TOOLTIP = "New asteroid belts added by mods can be selected here";
					}

					// Token: 0x02002B7E RID: 11134
					public class STORY_TRAIT_COUNT
					{
						// Token: 0x0400B2C6 RID: 45766
						public static LocString NAME = "Story Traits";

						// Token: 0x0400B2C7 RID: 45767
						public static LocString TOOLTIP = "Determines the number of story traits spawned";

						// Token: 0x02002B7F RID: 11135
						public static class LEVELS
						{
							// Token: 0x02002B80 RID: 11136
							public static class NONE
							{
								// Token: 0x0400B2C8 RID: 45768
								public static LocString NAME = "Zilch";

								// Token: 0x0400B2C9 RID: 45769
								public static LocString TOOLTIP = "Zero story traits. Zip. Nada. None";
							}

							// Token: 0x02002B81 RID: 11137
							public static class FEW
							{
								// Token: 0x0400B2CA RID: 45770
								public static LocString NAME = "Stingy";

								// Token: 0x0400B2CB RID: 45771
								public static LocString TOOLTIP = "Not zero, but not a lot";
							}

							// Token: 0x02002B82 RID: 11138
							public static class LOTS
							{
								// Token: 0x0400B2CC RID: 45772
								public static LocString NAME = "Oodles";

								// Token: 0x0400B2CD RID: 45773
								public static LocString TOOLTIP = "Plenty of story traits to go around";
							}
						}
					}

					// Token: 0x02002B83 RID: 11139
					public class DURABILITY
					{
						// Token: 0x0400B2CE RID: 45774
						public static LocString NAME = "Durability";

						// Token: 0x0400B2CF RID: 45775
						public static LocString TOOLTIP = "Affects how quickly equippable suits wear out";

						// Token: 0x02002B84 RID: 11140
						public static class LEVELS
						{
							// Token: 0x02002B85 RID: 11141
							public static class INDESTRUCTIBLE
							{
								// Token: 0x0400B2D0 RID: 45776
								public static LocString NAME = "Indestructible";

								// Token: 0x0400B2D1 RID: 45777
								public static LocString TOOLTIP = "Duplicants have perfected clothes manufacturing and are able to make suits that last forever";

								// Token: 0x0400B2D2 RID: 45778
								public static LocString ATTRIBUTE_MODIFIER_NAME = "Indestructible Suits (No Durability)";
							}

							// Token: 0x02002B86 RID: 11142
							public static class REINFORCED
							{
								// Token: 0x0400B2D3 RID: 45779
								public static LocString NAME = "Reinforced";

								// Token: 0x0400B2D4 RID: 45780
								public static LocString TOOLTIP = "Suits are more durable than usual";

								// Token: 0x0400B2D5 RID: 45781
								public static LocString ATTRIBUTE_MODIFIER_NAME = "Reinforced Suits (Difficulty Down)";
							}

							// Token: 0x02002B87 RID: 11143
							public static class DEFAULT
							{
								// Token: 0x0400B2D6 RID: 45782
								public static LocString NAME = "Default";

								// Token: 0x0400B2D7 RID: 45783
								public static LocString TOOLTIP = "Default suit durability";
							}

							// Token: 0x02002B88 RID: 11144
							public static class FLIMSY
							{
								// Token: 0x0400B2D8 RID: 45784
								public static LocString NAME = "Flimsy";

								// Token: 0x0400B2D9 RID: 45785
								public static LocString TOOLTIP = "Suits wear out faster than usual";

								// Token: 0x0400B2DA RID: 45786
								public static LocString ATTRIBUTE_MODIFIER_NAME = "Flimsy Suits (Difficulty Up)";
							}

							// Token: 0x02002B89 RID: 11145
							public static class THREADBARE
							{
								// Token: 0x0400B2DB RID: 45787
								public static LocString NAME = "Threadbare";

								// Token: 0x0400B2DC RID: 45788
								public static LocString TOOLTIP = "These Duplicants are no tailors - suits wear out much faster than usual";

								// Token: 0x0400B2DD RID: 45789
								public static LocString ATTRIBUTE_MODIFIER_NAME = "Threadbare Suits (Highest Difficulty)";
							}
						}
					}

					// Token: 0x02002B8A RID: 11146
					public class RADIATION
					{
						// Token: 0x0400B2DE RID: 45790
						public static LocString NAME = "Radiation";

						// Token: 0x0400B2DF RID: 45791
						public static LocString TOOLTIP = "Affects how susceptible Duplicants are to radiation sickness";

						// Token: 0x02002B8B RID: 11147
						public static class LEVELS
						{
							// Token: 0x02002B8C RID: 11148
							public static class HARDEST
							{
								// Token: 0x0400B2E0 RID: 45792
								public static LocString NAME = "Critical Mass";

								// Token: 0x0400B2E1 RID: 45793
								public static LocString TOOLTIP = "Duplicants feel ill at the merest mention of radiation...and may never truly recover";

								// Token: 0x0400B2E2 RID: 45794
								public static LocString ATTRIBUTE_MODIFIER_NAME = "Super Radiation (Highest Difficulty)";
							}

							// Token: 0x02002B8D RID: 11149
							public static class HARDER
							{
								// Token: 0x0400B2E3 RID: 45795
								public static LocString NAME = "Toxic Positivity";

								// Token: 0x0400B2E4 RID: 45796
								public static LocString TOOLTIP = "Duplicants are more sensitive to radiation exposure than usual";

								// Token: 0x0400B2E5 RID: 45797
								public static LocString ATTRIBUTE_MODIFIER_NAME = "Radiation Vulnerable (Difficulty Up)";
							}

							// Token: 0x02002B8E RID: 11150
							public static class DEFAULT
							{
								// Token: 0x0400B2E6 RID: 45798
								public static LocString NAME = "Default";

								// Token: 0x0400B2E7 RID: 45799
								public static LocString TOOLTIP = "Default radiation settings";
							}

							// Token: 0x02002B8F RID: 11151
							public static class EASIER
							{
								// Token: 0x0400B2E8 RID: 45800
								public static LocString NAME = "Healthy Glow";

								// Token: 0x0400B2E9 RID: 45801
								public static LocString TOOLTIP = "Duplicants are more resistant to radiation exposure than usual";

								// Token: 0x0400B2EA RID: 45802
								public static LocString ATTRIBUTE_MODIFIER_NAME = "Radiation Shielded (Difficulty Down)";
							}

							// Token: 0x02002B90 RID: 11152
							public static class EASIEST
							{
								// Token: 0x0400B2EB RID: 45803
								public static LocString NAME = "Nuke-Proof";

								// Token: 0x0400B2EC RID: 45804
								public static LocString TOOLTIP = "Duplicants could bathe in radioactive waste and not even notice";

								// Token: 0x0400B2ED RID: 45805
								public static LocString ATTRIBUTE_MODIFIER_NAME = "Radiation Protection (Lowest Difficulty)";
							}
						}
					}

					// Token: 0x02002B91 RID: 11153
					public class STRESS
					{
						// Token: 0x0400B2EE RID: 45806
						public static LocString NAME = "Stress";

						// Token: 0x0400B2EF RID: 45807
						public static LocString TOOLTIP = "Affects how quickly Duplicant stress rises";

						// Token: 0x02002B92 RID: 11154
						public static class LEVELS
						{
							// Token: 0x02002B93 RID: 11155
							public static class INDOMITABLE
							{
								// Token: 0x0400B2F0 RID: 45808
								public static LocString NAME = "Cloud Nine";

								// Token: 0x0400B2F1 RID: 45809
								public static LocString TOOLTIP = "A strong emotional support system makes these Duplicants impervious to all stress";

								// Token: 0x0400B2F2 RID: 45810
								public static LocString ATTRIBUTE_MODIFIER_NAME = "Cloud Nine (No Stress)";
							}

							// Token: 0x02002B94 RID: 11156
							public static class OPTIMISTIC
							{
								// Token: 0x0400B2F3 RID: 45811
								public static LocString NAME = "Chipper";

								// Token: 0x0400B2F4 RID: 45812
								public static LocString TOOLTIP = "Duplicants gain stress slower than usual";

								// Token: 0x0400B2F5 RID: 45813
								public static LocString ATTRIBUTE_MODIFIER_NAME = "Chipper (Difficulty Down)";
							}

							// Token: 0x02002B95 RID: 11157
							public static class DEFAULT
							{
								// Token: 0x0400B2F6 RID: 45814
								public static LocString NAME = "Default";

								// Token: 0x0400B2F7 RID: 45815
								public static LocString TOOLTIP = "Default stress change rate";
							}

							// Token: 0x02002B96 RID: 11158
							public static class PESSIMISTIC
							{
								// Token: 0x0400B2F8 RID: 45816
								public static LocString NAME = "Glum";

								// Token: 0x0400B2F9 RID: 45817
								public static LocString TOOLTIP = "Duplicants gain stress more quickly than usual";

								// Token: 0x0400B2FA RID: 45818
								public static LocString ATTRIBUTE_MODIFIER_NAME = "Glum (Difficulty Up)";
							}

							// Token: 0x02002B97 RID: 11159
							public static class DOOMED
							{
								// Token: 0x0400B2FB RID: 45819
								public static LocString NAME = "Frankly Depressing";

								// Token: 0x0400B2FC RID: 45820
								public static LocString TOOLTIP = "These Duplicants were never taught coping mechanisms... they're devastated by stress as a result";

								// Token: 0x0400B2FD RID: 45821
								public static LocString ATTRIBUTE_MODIFIER_NAME = "Frankly Depressing (Highest Difficulty)";
							}
						}
					}

					// Token: 0x02002B98 RID: 11160
					public class STRESS_BREAKS
					{
						// Token: 0x0400B2FE RID: 45822
						public static LocString NAME = "Stress Reactions";

						// Token: 0x0400B2FF RID: 45823
						public static LocString TOOLTIP = "Determines whether Duplicants wreak havoc on the colony when they reach maximum stress";

						// Token: 0x02002B99 RID: 11161
						public static class LEVELS
						{
							// Token: 0x02002B9A RID: 11162
							public static class DEFAULT
							{
								// Token: 0x0400B300 RID: 45824
								public static LocString NAME = "Enabled";

								// Token: 0x0400B301 RID: 45825
								public static LocString TOOLTIP = "Checked: Duplicants will wreak havoc when they reach 100% stress (Default)";
							}

							// Token: 0x02002B9B RID: 11163
							public static class DISABLED
							{
								// Token: 0x0400B302 RID: 45826
								public static LocString NAME = "Disabled";

								// Token: 0x0400B303 RID: 45827
								public static LocString TOOLTIP = "Unchecked: Duplicants will not wreak havoc at maximum stress";
							}
						}
					}

					// Token: 0x02002B9C RID: 11164
					public class WORLDGEN_SEED
					{
						// Token: 0x0400B304 RID: 45828
						public static LocString NAME = "Worldgen Seed";

						// Token: 0x0400B305 RID: 45829
						public static LocString TOOLTIP = "This number chooses the procedural parameters that create your unique map\n\nWorldgen seeds can be copied and pasted so others can play a replica of your world configuration";

						// Token: 0x0400B306 RID: 45830
						public static LocString FIXEDSEED = "This is a predetermined seed, and cannot be changed";
					}

					// Token: 0x02002B9D RID: 11165
					public class TELEPORTERS
					{
						// Token: 0x0400B307 RID: 45831
						public static LocString NAME = "Teleporters";

						// Token: 0x0400B308 RID: 45832
						public static LocString TOOLTIP = "Determines whether teleporters will be spawned during Worldgen";

						// Token: 0x02002B9E RID: 11166
						public static class LEVELS
						{
							// Token: 0x02002B9F RID: 11167
							public static class ENABLED
							{
								// Token: 0x0400B309 RID: 45833
								public static LocString NAME = "Enabled";

								// Token: 0x0400B30A RID: 45834
								public static LocString TOOLTIP = "Checked: Teleporters will spawn during Worldgen (Default)";
							}

							// Token: 0x02002BA0 RID: 11168
							public static class DISABLED
							{
								// Token: 0x0400B30B RID: 45835
								public static LocString NAME = "Disabled";

								// Token: 0x0400B30C RID: 45836
								public static LocString TOOLTIP = "Unchecked: No Teleporters will spawn during Worldgen";
							}
						}
					}

					// Token: 0x02002BA1 RID: 11169
					public class METEORSHOWERS
					{
						// Token: 0x0400B30D RID: 45837
						public static LocString NAME = "Meteor Showers";

						// Token: 0x0400B30E RID: 45838
						public static LocString TOOLTIP = "Adjusts the intensity of incoming space rocks";

						// Token: 0x02002BA2 RID: 11170
						public static class LEVELS
						{
							// Token: 0x02002BA3 RID: 11171
							public static class CLEAR_SKIES
							{
								// Token: 0x0400B30F RID: 45839
								public static LocString NAME = "Clear Skies";

								// Token: 0x0400B310 RID: 45840
								public static LocString TOOLTIP = "No meteor damage, no worries";
							}

							// Token: 0x02002BA4 RID: 11172
							public static class INFREQUENT
							{
								// Token: 0x0400B311 RID: 45841
								public static LocString NAME = "Spring Showers";

								// Token: 0x0400B312 RID: 45842
								public static LocString TOOLTIP = "Meteor showers are less frequent and less intense than usual";
							}

							// Token: 0x02002BA5 RID: 11173
							public static class DEFAULT
							{
								// Token: 0x0400B313 RID: 45843
								public static LocString NAME = "Default";

								// Token: 0x0400B314 RID: 45844
								public static LocString TOOLTIP = "Default meteor shower frequency and intensity";
							}

							// Token: 0x02002BA6 RID: 11174
							public static class INTENSE
							{
								// Token: 0x0400B315 RID: 45845
								public static LocString NAME = "Cosmic Storm";

								// Token: 0x0400B316 RID: 45846
								public static LocString TOOLTIP = "Meteor showers are more frequent and more intense than usual";
							}

							// Token: 0x02002BA7 RID: 11175
							public static class DOOMED
							{
								// Token: 0x0400B317 RID: 45847
								public static LocString NAME = "Doomsday";

								// Token: 0x0400B318 RID: 45848
								public static LocString TOOLTIP = "An onslaught of apocalyptic hailstorms that feels almost personal";
							}
						}
					}

					// Token: 0x02002BA8 RID: 11176
					public class DLC_MIXING
					{
						// Token: 0x02002BA9 RID: 11177
						public static class LEVELS
						{
							// Token: 0x02002BAA RID: 11178
							public static class DISABLED
							{
								// Token: 0x0400B319 RID: 45849
								public static LocString NAME = "Disabled";

								// Token: 0x0400B31A RID: 45850
								public static LocString TOOLTIP = "Content from this DLC is currently <b>disabled</b>";
							}

							// Token: 0x02002BAB RID: 11179
							public static class ENABLED
							{
								// Token: 0x0400B31B RID: 45851
								public static LocString NAME = "Enabled";

								// Token: 0x0400B31C RID: 45852
								public static LocString TOOLTIP = "Content from this DLC is currently <b>enabled</b>\n\nThis includes Care Packages, buildings, and space POIs";
							}
						}
					}

					// Token: 0x02002BAC RID: 11180
					public class SUBWORLD_MIXING
					{
						// Token: 0x02002BAD RID: 11181
						public static class LEVELS
						{
							// Token: 0x02002BAE RID: 11182
							public static class DISABLED
							{
								// Token: 0x0400B31D RID: 45853
								public static LocString NAME = "Disabled";

								// Token: 0x0400B31E RID: 45854
								public static LocString TOOLTIP = "This biome will not be mixed into any world";

								// Token: 0x0400B31F RID: 45855
								public static LocString TOOLTIP_BASEGAME = "This biome will not be mixed in";
							}

							// Token: 0x02002BAF RID: 11183
							public static class TRY_MIXING
							{
								// Token: 0x0400B320 RID: 45856
								public static LocString NAME = "Likely";

								// Token: 0x0400B321 RID: 45857
								public static LocString TOOLTIP = "This biome is very likely to be mixed into a world";

								// Token: 0x0400B322 RID: 45858
								public static LocString TOOLTIP_BASEGAME = "This biome is very likely to be mixed in";
							}

							// Token: 0x02002BB0 RID: 11184
							public static class GUARANTEE_MIXING
							{
								// Token: 0x0400B323 RID: 45859
								public static LocString NAME = "Guaranteed";

								// Token: 0x0400B324 RID: 45860
								public static LocString TOOLTIP = "This biome will be mixed into a world, even if it causes a worldgen failure";

								// Token: 0x0400B325 RID: 45861
								public static LocString TOOLTIP_BASEGAME = "This biome will be mixed in, even if it causes a worldgen failure";
							}
						}
					}

					// Token: 0x02002BB1 RID: 11185
					public class WORLD_MIXING
					{
						// Token: 0x02002BB2 RID: 11186
						public static class LEVELS
						{
							// Token: 0x02002BB3 RID: 11187
							public static class DISABLED
							{
								// Token: 0x0400B326 RID: 45862
								public static LocString NAME = "Disabled";

								// Token: 0x0400B327 RID: 45863
								public static LocString TOOLTIP = "This asteroid will not be mixed in";
							}

							// Token: 0x02002BB4 RID: 11188
							public static class TRY_MIXING
							{
								// Token: 0x0400B328 RID: 45864
								public static LocString NAME = "Likely";

								// Token: 0x0400B329 RID: 45865
								public static LocString TOOLTIP = "This asteroid is very likely to be mixed in";
							}

							// Token: 0x02002BB5 RID: 11189
							public static class GUARANTEE_MIXING
							{
								// Token: 0x0400B32A RID: 45866
								public static LocString NAME = "Guaranteed";

								// Token: 0x0400B32B RID: 45867
								public static LocString TOOLTIP = "This asteroid will be mixed in, even if it causes worldgen failure";
							}
						}
					}
				}
			}

			// Token: 0x02002BB6 RID: 11190
			public class MAINMENU
			{
				// Token: 0x0400B32C RID: 45868
				public static LocString STARTDEMO = "START DEMO";

				// Token: 0x0400B32D RID: 45869
				public static LocString NEWGAME = "NEW GAME";

				// Token: 0x0400B32E RID: 45870
				public static LocString RESUMEGAME = "RESUME GAME";

				// Token: 0x0400B32F RID: 45871
				public static LocString LOADGAME = "LOAD GAME";

				// Token: 0x0400B330 RID: 45872
				public static LocString RETIREDCOLONIES = "COLONY SUMMARIES";

				// Token: 0x0400B331 RID: 45873
				public static LocString KLEIINVENTORY = "KLEI INVENTORY";

				// Token: 0x0400B332 RID: 45874
				public static LocString LOCKERMENU = "SUPPLY CLOSET";

				// Token: 0x0400B333 RID: 45875
				public static LocString SCENARIOS = "SCENARIOS";

				// Token: 0x0400B334 RID: 45876
				public static LocString TRANSLATIONS = "TRANSLATIONS";

				// Token: 0x0400B335 RID: 45877
				public static LocString OPTIONS = "OPTIONS";

				// Token: 0x0400B336 RID: 45878
				public static LocString QUITTODESKTOP = "QUIT";

				// Token: 0x0400B337 RID: 45879
				public static LocString RESTARTCONFIRM = "Should I really quit?\nAll unsaved progress will be lost.";

				// Token: 0x0400B338 RID: 45880
				public static LocString QUITCONFIRM = "Should I quit to the main menu?\nAll unsaved progress will be lost.";

				// Token: 0x0400B339 RID: 45881
				public static LocString RETIRECONFIRM = "Should I surrender under the soul-crushing weight of this universe's entropy and retire my colony?";

				// Token: 0x0400B33A RID: 45882
				public static LocString DESKTOPQUITCONFIRM = "Should I really quit?\nAll unsaved progress will be lost.";

				// Token: 0x0400B33B RID: 45883
				public static LocString RESUMEBUTTON_BASENAME = "{0}: Cycle {1}";

				// Token: 0x0400B33C RID: 45884
				public static LocString QUIT = "QUIT WITHOUT SAVING";

				// Token: 0x0400B33D RID: 45885
				public static LocString SAVEANDQUITTITLE = "SAVE AND QUIT";

				// Token: 0x0400B33E RID: 45886
				public static LocString SAVEANDQUITDESKTOP = "SAVE AND QUIT";

				// Token: 0x0400B33F RID: 45887
				public static LocString WISHLIST_AD = "Available now";

				// Token: 0x0400B340 RID: 45888
				public static LocString WISHLIST_AD_TOOLTIP = "<color=#ffff00ff><b>Click to view it in the store</b></color>";

				// Token: 0x02002BB7 RID: 11191
				public class DLC
				{
					// Token: 0x0400B341 RID: 45889
					public static LocString ACTIVATE_EXPANSION1 = "ENABLE DLC";

					// Token: 0x0400B342 RID: 45890
					public static LocString ACTIVATE_EXPANSION1_TOOLTIP = "<b>This DLC is disabled</b>\n\n<color=#ffff00ff><b>Click to enable the <i>Spaced Out!</i> DLC</b></color>";

					// Token: 0x0400B343 RID: 45891
					public static LocString ACTIVATE_EXPANSION1_DESC = "The game will need to restart in order to enable <i>Spaced Out!</i>";

					// Token: 0x0400B344 RID: 45892
					public static LocString ACTIVATE_EXPANSION1_RAIL_DESC = "<i>Spaced Out!</i> will be enabled the next time you launch the game. The game will now close.";

					// Token: 0x0400B345 RID: 45893
					public static LocString DEACTIVATE_EXPANSION1 = "DISABLE DLC";

					// Token: 0x0400B346 RID: 45894
					public static LocString DEACTIVATE_EXPANSION1_TOOLTIP = "<b>This DLC is enabled</b>\n\n<color=#ffff00ff><b>Click to disable the <i>Spaced Out!</i> DLC</b></color>";

					// Token: 0x0400B347 RID: 45895
					public static LocString DEACTIVATE_EXPANSION1_DESC = "The game will need to restart in order to enable the <i>Oxygen Not Included</i> base game.";

					// Token: 0x0400B348 RID: 45896
					public static LocString DEACTIVATE_EXPANSION1_RAIL_DESC = "<i>Spaced Out!</i> will be disabled the next time you launch the game. The game will now close.";

					// Token: 0x0400B349 RID: 45897
					public static LocString AD_DLC1 = "Spaced Out! DLC";

					// Token: 0x0400B34A RID: 45898
					public static LocString CONTENT_INSTALLED_LABEL = "Installed";

					// Token: 0x0400B34B RID: 45899
					public static LocString CONTENT_ACTIVE_TOOLTIP = "<b>This DLC is enabled</b>\n\nFind it in the destination selection screen when starting a new game, or in the Load Game screen for existing DLC-enabled saves";

					// Token: 0x0400B34C RID: 45900
					public static LocString CONTENT_OWNED_NOTINSTALLED_LABEL = "";

					// Token: 0x0400B34D RID: 45901
					public static LocString CONTENT_OWNED_NOTINSTALLED_TOOLTIP = "This DLC is owned but not currently installed";

					// Token: 0x0400B34E RID: 45902
					public static LocString CONTENT_NOTOWNED_LABEL = "Available Now";

					// Token: 0x0400B34F RID: 45903
					public static LocString CONTENT_NOTOWNED_TOOLTIP = "This DLC is available now!";
				}
			}

			// Token: 0x02002BB8 RID: 11192
			public class DEVTOOLS
			{
				// Token: 0x0400B350 RID: 45904
				public static LocString TITLE = "About Dev Tools";

				// Token: 0x0400B351 RID: 45905
				public static LocString WARNING = "DANGER!!\n\nDev Tools are intended for developer use only. Using them may result in your save becoming unplayable, unstable, or severely damaged.\n\nThese tools are completely unsupported and may contain bugs. Are you sure you want to continue?";

				// Token: 0x0400B352 RID: 45906
				public static LocString DONTSHOW = "Do not show this message again";

				// Token: 0x0400B353 RID: 45907
				public static LocString BUTTON = "Show Dev Tools";
			}

			// Token: 0x02002BB9 RID: 11193
			public class NEWGAMESETTINGS
			{
				// Token: 0x0400B354 RID: 45908
				public static LocString HEADER = "GAME SETTINGS";

				// Token: 0x02002BBA RID: 11194
				public class BUTTONS
				{
					// Token: 0x0400B355 RID: 45909
					public static LocString STANDARDGAME = "Standard Game";

					// Token: 0x0400B356 RID: 45910
					public static LocString CUSTOMGAME = "Custom Game";

					// Token: 0x0400B357 RID: 45911
					public static LocString CANCEL = "Cancel";

					// Token: 0x0400B358 RID: 45912
					public static LocString STARTGAME = "Start Game";
				}
			}

			// Token: 0x02002BBB RID: 11195
			public class COLONYDESTINATIONSCREEN
			{
				// Token: 0x0400B359 RID: 45913
				public static LocString TITLE = "CHOOSE A DESTINATION";

				// Token: 0x0400B35A RID: 45914
				public static LocString GENTLE_ZONE = "Habitable Zone";

				// Token: 0x0400B35B RID: 45915
				public static LocString DETAILS = "Destination Details";

				// Token: 0x0400B35C RID: 45916
				public static LocString START_SITE = "Immediate Surroundings";

				// Token: 0x0400B35D RID: 45917
				public static LocString COORDINATE = "Coordinates:";

				// Token: 0x0400B35E RID: 45918
				public static LocString CANCEL = "Back";

				// Token: 0x0400B35F RID: 45919
				public static LocString CUSTOMIZE = "Game Settings";

				// Token: 0x0400B360 RID: 45920
				public static LocString START_GAME = "Start Game";

				// Token: 0x0400B361 RID: 45921
				public static LocString SHUFFLE = "Shuffle";

				// Token: 0x0400B362 RID: 45922
				public static LocString SHUFFLETOOLTIP = "Reroll World Seed\n\nThis will shuffle the layout of your world and the geographical traits listed below";

				// Token: 0x0400B363 RID: 45923
				public static LocString SHUFFLETOOLTIP_DISABLED = "This world's seed is predetermined. It cannot be changed";

				// Token: 0x0400B364 RID: 45924
				public static LocString HEADER_ASTEROID_STARTING = "Starting Asteroid";

				// Token: 0x0400B365 RID: 45925
				public static LocString HEADER_ASTEROID_NEARBY = "Nearby Asteroids";

				// Token: 0x0400B366 RID: 45926
				public static LocString HEADER_ASTEROID_DISTANT = "Distant Asteroids";

				// Token: 0x0400B367 RID: 45927
				public static LocString TRAITS_HEADER = "World Traits";

				// Token: 0x0400B368 RID: 45928
				public static LocString STORY_TRAITS_HEADER = "Story Traits";

				// Token: 0x0400B369 RID: 45929
				public static LocString MIXING_SETTINGS_HEADER = "Scramble DLCs";

				// Token: 0x0400B36A RID: 45930
				public static LocString MIXING_DLC_HEADER = "DLC Content";

				// Token: 0x0400B36B RID: 45931
				public static LocString MIXING_WORLDMIXING_HEADER = "Asteroid Remix";

				// Token: 0x0400B36C RID: 45932
				public static LocString MIXING_SUBWORLDMIXING_HEADER = "Biome Remix";

				// Token: 0x0400B36D RID: 45933
				public static LocString MIXING_NO_OPTIONS = "No additional content currently available for remixing. Don't worry, there's plenty already baked in.";

				// Token: 0x0400B36E RID: 45934
				public static LocString MIXING_WARNING = "Choose additional content to remix into the game. Scrambling realities may cause cosmic collapse.";

				// Token: 0x0400B36F RID: 45935
				public static LocString MIXING_TOOLTIP_DLC_MIXING = "DLC content includes buildings, Care Packages, space POIs, critters, etc\n\nEnabling DLC content allows asteroid and biome remixes from that DLC to be customized in the sections below";

				// Token: 0x0400B370 RID: 45936
				public static LocString MIXING_TOOLTIP_ASTEROID_MIXING = "Asteroid remixing modifies which asteroids appear on the starmap\n\nRemixed asteroids will retain key features of the outer asteroids that they replace";

				// Token: 0x0400B371 RID: 45937
				public static LocString MIXING_TOOLTIP_BIOME_MIXING = "Biome remixing modifies which biomes will be included across multiple asteroids";

				// Token: 0x0400B372 RID: 45938
				public static LocString MIXING_TOOLTIP_TOO_MANY_GUARENTEED_ASTEROID_MIXINGS = UI.FRONTEND.COLONYDESTINATIONSCREEN.MIXING_TOOLTIP_ASTEROID_MIXING + "\n\nMaximum of {1} guaranteed asteroid remixes allowed\n\nTotal currently selected: {0}";

				// Token: 0x0400B373 RID: 45939
				public static LocString MIXING_TOOLTIP_TOO_MANY_GUARENTEED_BIOME_MIXINGS = UI.FRONTEND.COLONYDESTINATIONSCREEN.MIXING_TOOLTIP_BIOME_MIXING + "\n\nMaximum of {1} guaranteed biome remixes allowed\n\nTotal currently selected: {0}";

				// Token: 0x0400B374 RID: 45940
				public static LocString MIXING_TOOLTIP_LOCKED_START_NOT_SUPPORTED = "This destination does not support changing this setting";

				// Token: 0x0400B375 RID: 45941
				public static LocString MIXING_TOOLTIP_LOCKED_REQUIRE_DLC_NOT_ENABLED = "This setting requires the following content to be enabled:\n{0}";

				// Token: 0x0400B376 RID: 45942
				public static LocString MIXING_TOOLTIP_DLC_CONTENT = "This content is from {0}";

				// Token: 0x0400B377 RID: 45943
				public static LocString MIXING_TOOLTIP_MODDED_SETTING = "<i><color=#d6d6d6>This setting was added by a mod</color></i>";

				// Token: 0x0400B378 RID: 45944
				public static LocString MIXING_TOOLTIP_CANNOT_START = "Cannot start a new game with current asteroid and biome remix configuration";

				// Token: 0x0400B379 RID: 45945
				public static LocString NO_TRAITS = "No Traits";

				// Token: 0x0400B37A RID: 45946
				public static LocString SINGLE_TRAIT = "1 Trait";

				// Token: 0x0400B37B RID: 45947
				public static LocString TRAIT_COUNT = "{0} Traits";

				// Token: 0x0400B37C RID: 45948
				public static LocString TOO_MANY_TRAITS_WARNING = UI.YELLOW_PREFIX + "Too many!" + UI.COLOR_SUFFIX;

				// Token: 0x0400B37D RID: 45949
				public static LocString TOO_MANY_TRAITS_WARNING_TOOLTIP = UI.YELLOW_PREFIX + "Squeezing this many story traits into this asteroid may cause worldgen to fail\n\nConsider lowering the number of story traits or changing the selected asteroid" + UI.COLOR_SUFFIX;

				// Token: 0x0400B37E RID: 45950
				public static LocString SHUFFLE_STORY_TRAITS_TOOLTIP = "Randomize Story Traits\n\nThis will select a comfortable number of story traits for the starting asteroid";

				// Token: 0x0400B37F RID: 45951
				public static LocString SELECTED_CLUSTER_TRAITS_HEADER = "Target Details";
			}

			// Token: 0x02002BBC RID: 11196
			public class MODESELECTSCREEN
			{
				// Token: 0x0400B380 RID: 45952
				public static LocString HEADER = "GAME MODE";

				// Token: 0x0400B381 RID: 45953
				public static LocString BLANK_DESC = "Select a playstyle...";

				// Token: 0x0400B382 RID: 45954
				public static LocString SURVIVAL_TITLE = "SURVIVAL";

				// Token: 0x0400B383 RID: 45955
				public static LocString SURVIVAL_DESC = "Stay on your toes and one step ahead of this unforgiving world. One slip up could bring your colony crashing down.";

				// Token: 0x0400B384 RID: 45956
				public static LocString NOSWEAT_TITLE = "NO SWEAT";

				// Token: 0x0400B385 RID: 45957
				public static LocString NOSWEAT_DESC = "When disaster strikes (and it inevitably will), take a deep breath and stay calm. You have ample time to find a solution.";

				// Token: 0x0400B386 RID: 45958
				public static LocString ACTIVE_CONTENT_HEADER = "ACTIVE CONTENT";
			}

			// Token: 0x02002BBD RID: 11197
			public class CLUSTERCATEGORYSELECTSCREEN
			{
				// Token: 0x0400B387 RID: 45959
				public static LocString HEADER = "ASTEROID STYLE";

				// Token: 0x0400B388 RID: 45960
				public static LocString BLANK_DESC = "Select an asteroid style...";

				// Token: 0x0400B389 RID: 45961
				public static LocString VANILLA_TITLE = "Standard";

				// Token: 0x0400B38A RID: 45962
				public static LocString VANILLA_DESC = "Scenarios designed for classic gameplay.";

				// Token: 0x0400B38B RID: 45963
				public static LocString CLASSIC_TITLE = "Classic";

				// Token: 0x0400B38C RID: 45964
				public static LocString CLASSIC_DESC = "Scenarios similar to the <b>classic Oxygen Not Included</b> experience. Large starting asteroids with many resources.\nLess emphasis on space travel.";

				// Token: 0x0400B38D RID: 45965
				public static LocString SPACEDOUT_TITLE = "Spaced Out!";

				// Token: 0x0400B38E RID: 45966
				public static LocString SPACEDOUT_DESC = "Scenarios designed for the <b>Spaced Out! DLC</b>.\nSmaller starting asteroids with resources distributed across the starmap. More emphasis on space travel.";

				// Token: 0x0400B38F RID: 45967
				public static LocString EVENT_TITLE = "The Lab";

				// Token: 0x0400B390 RID: 45968
				public static LocString EVENT_DESC = "Alternative gameplay experiences, including experimental scenarios designed for special events.";
			}

			// Token: 0x02002BBE RID: 11198
			public class PATCHNOTESSCREEN
			{
				// Token: 0x0400B391 RID: 45969
				public static LocString HEADER = "IMPORTANT UPDATE NOTES";

				// Token: 0x0400B392 RID: 45970
				public static LocString OK_BUTTON = "OK";

				// Token: 0x0400B393 RID: 45971
				public static LocString FULLPATCHNOTES_TOOLTIP = "View the full patch notes online";
			}

			// Token: 0x02002BBF RID: 11199
			public class LOADSCREEN
			{
				// Token: 0x0400B394 RID: 45972
				public static LocString TITLE = "LOAD GAME";

				// Token: 0x0400B395 RID: 45973
				public static LocString TITLE_INSPECT = "LOAD GAME";

				// Token: 0x0400B396 RID: 45974
				public static LocString DELETEBUTTON = "DELETE";

				// Token: 0x0400B397 RID: 45975
				public static LocString BACKBUTTON = "< BACK";

				// Token: 0x0400B398 RID: 45976
				public static LocString CONFIRMDELETE = "Are you sure you want to delete {0}?\nYou cannot undo this action.";

				// Token: 0x0400B399 RID: 45977
				public static LocString SAVEDETAILS = "<b>File:</b> {0}\n\n<b>Save Date:</b>\n{1}\n\n<b>Base Name:</b> {2}\n<b>Duplicants Alive:</b> {3}\n<b>Cycle(s) Survived:</b> {4}";

				// Token: 0x0400B39A RID: 45978
				public static LocString AUTOSAVEWARNING = "<color=#F44A47FF>Autosave: This file will get deleted as new autosaves are created</color>";

				// Token: 0x0400B39B RID: 45979
				public static LocString CORRUPTEDSAVE = "<b><color=#F44A47FF>Could not load file {0}. Its data may be corrupted.</color></b>";

				// Token: 0x0400B39C RID: 45980
				public static LocString SAVE_TOO_NEW = "<b><color=#F44A47FF>Could not load file {0}. File is using build {1}, v{2}. This build is {3}, v{4}.</color></b>";

				// Token: 0x0400B39D RID: 45981
				public static LocString TOOLTIP_SAVE_INCOMPATABLE_DLC_CONFIGURATION = "This save file was created with a different DLC configuration\n\nTo load this file:";

				// Token: 0x0400B39E RID: 45982
				public static LocString TOOLTIP_SAVE_INCOMPATABLE_DLC_CONFIGURATION_ASK_TO_ENABLE = "    • Activate {0}";

				// Token: 0x0400B39F RID: 45983
				public static LocString TOOLTIP_SAVE_INCOMPATABLE_DLC_CONFIGURATION_ASK_TO_DISABLE = "    • Deactivate {0}";

				// Token: 0x0400B3A0 RID: 45984
				public static LocString TOOLTIP_SAVE_USES_DLC = "{0} save";

				// Token: 0x0400B3A1 RID: 45985
				public static LocString UNSUPPORTED_SAVE_VERSION = "<b><color=#F44A47FF>This save file is from a previous version of the game and is no longer supported.</color></b>";

				// Token: 0x0400B3A2 RID: 45986
				public static LocString MORE_INFO = "More Info";

				// Token: 0x0400B3A3 RID: 45987
				public static LocString NEWEST_SAVE = "NEWEST";

				// Token: 0x0400B3A4 RID: 45988
				public static LocString BASE_NAME = "Base Name";

				// Token: 0x0400B3A5 RID: 45989
				public static LocString CYCLES_SURVIVED = "Cycles Survived";

				// Token: 0x0400B3A6 RID: 45990
				public static LocString DUPLICANTS_ALIVE = "Duplicants Alive";

				// Token: 0x0400B3A7 RID: 45991
				public static LocString WORLD_NAME = "Asteroid Type";

				// Token: 0x0400B3A8 RID: 45992
				public static LocString NO_FILE_SELECTED = "No file selected";

				// Token: 0x0400B3A9 RID: 45993
				public static LocString COLONY_INFO_FMT = "{0}: {1}";

				// Token: 0x0400B3AA RID: 45994
				public static LocString LOAD_MORE_COLONIES_BUTTON = "Load more...";

				// Token: 0x0400B3AB RID: 45995
				public static LocString VANILLA_RESTART = "Loading this colony will require restarting the game with " + UI.DLC1.NAME_ITAL + " content disabled";

				// Token: 0x0400B3AC RID: 45996
				public static LocString EXPANSION1_RESTART = "Loading this colony will require restarting the game with " + UI.DLC1.NAME_ITAL + " content enabled";

				// Token: 0x0400B3AD RID: 45997
				public static LocString UNSUPPORTED_VANILLA_TEMP = "<b><color=#F44A47FF>This save file is from the base version of the game and currently cannot be loaded while " + UI.DLC1.NAME_ITAL + " is installed.</color></b>";

				// Token: 0x0400B3AE RID: 45998
				public static LocString CONTENT = "Content";

				// Token: 0x0400B3AF RID: 45999
				public static LocString VANILLA_CONTENT = "Vanilla FIXME";

				// Token: 0x0400B3B0 RID: 46000
				public static LocString EXPANSION1_CONTENT = UI.DLC1.NAME_ITAL + " Expansion FIXME";

				// Token: 0x0400B3B1 RID: 46001
				public static LocString SAVE_INFO = "{0} saves  {1} autosaves  {2}";

				// Token: 0x0400B3B2 RID: 46002
				public static LocString COLONIES_TITLE = "Colony View";

				// Token: 0x0400B3B3 RID: 46003
				public static LocString COLONY_TITLE = "Viewing colony '{0}'";

				// Token: 0x0400B3B4 RID: 46004
				public static LocString COLONY_FILE_SIZE = "Size: {0}";

				// Token: 0x0400B3B5 RID: 46005
				public static LocString COLONY_FILE_NAME = "File: '{0}'";

				// Token: 0x0400B3B6 RID: 46006
				public static LocString NO_PREVIEW = "NO PREVIEW";

				// Token: 0x0400B3B7 RID: 46007
				public static LocString LOCAL_SAVE = "local";

				// Token: 0x0400B3B8 RID: 46008
				public static LocString CLOUD_SAVE = "cloud";

				// Token: 0x0400B3B9 RID: 46009
				public static LocString CONVERT_COLONY = "CONVERT COLONY";

				// Token: 0x0400B3BA RID: 46010
				public static LocString CONVERT_ALL_COLONIES = "CONVERT ALL";

				// Token: 0x0400B3BB RID: 46011
				public static LocString CONVERT_ALL_WARNING = UI.PRE_KEYWORD + "\nWarning:" + UI.PST_KEYWORD + " Converting all colonies may take some time.";

				// Token: 0x0400B3BC RID: 46012
				public static LocString SAVE_INFO_DIALOG_TITLE = "SAVE INFORMATION";

				// Token: 0x0400B3BD RID: 46013
				public static LocString SAVE_INFO_DIALOG_TEXT = "Access your save files using the options below.";

				// Token: 0x0400B3BE RID: 46014
				public static LocString SAVE_INFO_DIALOG_TOOLTIP = "Access your save file locations from here.";

				// Token: 0x0400B3BF RID: 46015
				public static LocString CONVERT_ERROR_TITLE = "SAVE CONVERSION UNSUCCESSFUL";

				// Token: 0x0400B3C0 RID: 46016
				public static LocString CONVERT_ERROR = string.Concat(new string[]
				{
					"Converting the colony ",
					UI.PRE_KEYWORD,
					"{Colony}",
					UI.PST_KEYWORD,
					" was unsuccessful!\nThe error was:\n\n<b>{Error}</b>\n\nPlease try again, or post a bug in the forums if this problem keeps happening."
				});

				// Token: 0x0400B3C1 RID: 46017
				public static LocString CONVERT_TO_CLOUD = "CONVERT TO CLOUD SAVES";

				// Token: 0x0400B3C2 RID: 46018
				public static LocString CONVERT_TO_LOCAL = "CONVERT TO LOCAL SAVES";

				// Token: 0x0400B3C3 RID: 46019
				public static LocString CONVERT_COLONY_TO_CLOUD = "Convert colony to use cloud saves";

				// Token: 0x0400B3C4 RID: 46020
				public static LocString CONVERT_COLONY_TO_LOCAL = "Convert to colony to use local saves";

				// Token: 0x0400B3C5 RID: 46021
				public static LocString CONVERT_ALL_TO_CLOUD = "Convert <b>all</b> colonies below to use cloud saves";

				// Token: 0x0400B3C6 RID: 46022
				public static LocString CONVERT_ALL_TO_LOCAL = "Convert <b>all</b> colonies below to use local saves";

				// Token: 0x0400B3C7 RID: 46023
				public static LocString CONVERT_ALL_TO_CLOUD_SUCCESS = string.Concat(new string[]
				{
					UI.PRE_KEYWORD,
					"SUCCESS!",
					UI.PST_KEYWORD,
					"\nAll existing colonies have been converted into ",
					UI.PRE_KEYWORD,
					"cloud",
					UI.PST_KEYWORD,
					" saves.\nNew colonies will use ",
					UI.PRE_KEYWORD,
					"cloud",
					UI.PST_KEYWORD,
					" saves by default.\n\n{Client} may take longer than usual to sync the next time you exit the game as a result of this change."
				});

				// Token: 0x0400B3C8 RID: 46024
				public static LocString CONVERT_ALL_TO_LOCAL_SUCCESS = string.Concat(new string[]
				{
					UI.PRE_KEYWORD,
					"SUCCESS!",
					UI.PST_KEYWORD,
					"\nAll existing colonies have been converted into ",
					UI.PRE_KEYWORD,
					"local",
					UI.PST_KEYWORD,
					" saves.\nNew colonies will use ",
					UI.PRE_KEYWORD,
					"local",
					UI.PST_KEYWORD,
					" saves by default.\n\n{Client} may take longer than usual to sync the next time you exit the game as a result of this change."
				});

				// Token: 0x0400B3C9 RID: 46025
				public static LocString CONVERT_TO_CLOUD_DETAILS = "Converting a colony to use cloud saves will move all of the save files for that colony into the cloud saves folder.\n\nThis allows your game platform to sync this colony to the cloud for your account, so it can be played on multiple machines.";

				// Token: 0x0400B3CA RID: 46026
				public static LocString CONVERT_TO_LOCAL_DETAILS = "Converting a colony to NOT use cloud saves will move all of the save files for that colony into the local saves folder.\n\n" + UI.PRE_KEYWORD + "These save files will no longer be synced to the cloud." + UI.PST_KEYWORD;

				// Token: 0x0400B3CB RID: 46027
				public static LocString OPEN_SAVE_FOLDER = "LOCAL SAVES";

				// Token: 0x0400B3CC RID: 46028
				public static LocString OPEN_CLOUDSAVE_FOLDER = "CLOUD SAVES";

				// Token: 0x0400B3CD RID: 46029
				public static LocString MIGRATE_TITLE = "SAVE FILE MIGRATION";

				// Token: 0x0400B3CE RID: 46030
				public static LocString MIGRATE_SAVE_FILES = "MIGRATE SAVE FILES";

				// Token: 0x0400B3CF RID: 46031
				public static LocString MIGRATE_COUNT = string.Concat(new string[]
				{
					"\nFound ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" saves and ",
					UI.PRE_KEYWORD,
					"{1}",
					UI.PST_KEYWORD,
					" autosaves that require migration."
				});

				// Token: 0x0400B3D0 RID: 46032
				public static LocString MIGRATE_RESULT = string.Concat(new string[]
				{
					UI.PRE_KEYWORD,
					"SUCCESS!",
					UI.PST_KEYWORD,
					"\nMigration moved ",
					UI.PRE_KEYWORD,
					"{0}/{1}",
					UI.PST_KEYWORD,
					" saves and ",
					UI.PRE_KEYWORD,
					"{2}/{3}",
					UI.PST_KEYWORD,
					" autosaves",
					UI.PST_KEYWORD,
					"."
				});

				// Token: 0x0400B3D1 RID: 46033
				public static LocString MIGRATE_RESULT_FAILURES = string.Concat(new string[]
				{
					UI.PRE_KEYWORD,
					"<b>WARNING:</b> Not all saves could be migrated.",
					UI.PST_KEYWORD,
					"\nMigration moved ",
					UI.PRE_KEYWORD,
					"{0}/{1}",
					UI.PST_KEYWORD,
					" saves and ",
					UI.PRE_KEYWORD,
					"{2}/{3}",
					UI.PST_KEYWORD,
					" autosaves.\n\nThe file ",
					UI.PRE_KEYWORD,
					"{ErrorColony}",
					UI.PST_KEYWORD,
					" encountered this error:\n\n<b>{ErrorMessage}</b>"
				});

				// Token: 0x0400B3D2 RID: 46034
				public static LocString MIGRATE_RESULT_FAILURES_MORE_INFO_TITLE = "MIGRATION INCOMPLETE";

				// Token: 0x0400B3D3 RID: 46035
				public static LocString MIGRATE_RESULT_FAILURES_MORE_INFO_PRE = "<b>The game was unable to move all save files to their new location.\nTo fix this, please:</b>\n\n";

				// Token: 0x0400B3D4 RID: 46036
				public static LocString MIGRATE_RESULT_FAILURES_MORE_INFO_ITEM1 = "    1. Try temporarily disabling virus scanners and malware\n         protection programs.";

				// Token: 0x0400B3D5 RID: 46037
				public static LocString MIGRATE_RESULT_FAILURES_MORE_INFO_ITEM2 = "    2. Turn off file sync services such as OneDrive and DropBox.";

				// Token: 0x0400B3D6 RID: 46038
				public static LocString MIGRATE_RESULT_FAILURES_MORE_INFO_ITEM3 = "    3. Restart the game to retry file migration.";

				// Token: 0x0400B3D7 RID: 46039
				public static LocString MIGRATE_RESULT_FAILURES_MORE_INFO_POST = "\n<b>If this still doesn't solve the problem, please post a bug in the forums and we will attempt to assist with your issue.</b>";

				// Token: 0x0400B3D8 RID: 46040
				public static LocString MIGRATE_INFO = "We've changed how save files are organized!\nPlease " + UI.CLICK(UI.ClickType.click) + " the button below to automatically update your save file storage.";

				// Token: 0x0400B3D9 RID: 46041
				public static LocString MIGRATE_DONE = "CONTINUE";

				// Token: 0x0400B3DA RID: 46042
				public static LocString MIGRATE_FAILURES_FORUM_BUTTON = "VISIT FORUMS";

				// Token: 0x0400B3DB RID: 46043
				public static LocString MIGRATE_FAILURES_DONE = "MORE INFO";

				// Token: 0x0400B3DC RID: 46044
				public static LocString CLOUD_TUTORIAL_BOUNCER = "Upload Saves to Cloud";
			}

			// Token: 0x02002BC0 RID: 11200
			public class SAVESCREEN
			{
				// Token: 0x0400B3DD RID: 46045
				public static LocString TITLE = "SAVE SLOTS";

				// Token: 0x0400B3DE RID: 46046
				public static LocString NEWSAVEBUTTON = "New Save";

				// Token: 0x0400B3DF RID: 46047
				public static LocString OVERWRITEMESSAGE = "Are you sure you want to overwrite {0}?";

				// Token: 0x0400B3E0 RID: 46048
				public static LocString SAVENAMETITLE = "SAVE NAME";

				// Token: 0x0400B3E1 RID: 46049
				public static LocString CONFIRMNAME = "Confirm";

				// Token: 0x0400B3E2 RID: 46050
				public static LocString CANCELNAME = "Cancel";

				// Token: 0x0400B3E3 RID: 46051
				public static LocString IO_ERROR = "An error occurred trying to save your game. Please ensure there is sufficient disk space.\n\n{0}";

				// Token: 0x0400B3E4 RID: 46052
				public static LocString REPORT_BUG = "Report Bug";

				// Token: 0x0400B3E5 RID: 46053
				public static LocString SAVE_COMPLETE_MESSAGE = "Save Complete";
			}

			// Token: 0x02002BC1 RID: 11201
			public class RAILFORCEQUIT
			{
				// Token: 0x0400B3E6 RID: 46054
				public static LocString SAVE_EXIT = "Play time has expired and the game is exiting. Would you like to overwrite {0}?";

				// Token: 0x0400B3E7 RID: 46055
				public static LocString WARN_EXIT = "Play time has expired and the game will now exit.";

				// Token: 0x0400B3E8 RID: 46056
				public static LocString DLC_NOT_PURCHASED = "The <i>Spaced Out!</i> DLC has not yet been purchased in the WeGame store. Purchase <i>Spaced Out!</i> to support <i>Oxygen Not Included</i> and enjoy the new content!";
			}

			// Token: 0x02002BC2 RID: 11202
			public class MOD_ERRORS
			{
				// Token: 0x0400B3E9 RID: 46057
				public static LocString TITLE = "MOD ERRORS";

				// Token: 0x0400B3EA RID: 46058
				public static LocString DETAILS = "DETAILS";

				// Token: 0x0400B3EB RID: 46059
				public static LocString CLOSE = "CLOSE";
			}

			// Token: 0x02002BC3 RID: 11203
			public class MODS
			{
				// Token: 0x0400B3EC RID: 46060
				public static LocString TITLE = "MODS";

				// Token: 0x0400B3ED RID: 46061
				public static LocString MANAGE = "Subscription";

				// Token: 0x0400B3EE RID: 46062
				public static LocString MANAGE_LOCAL = "Browse";

				// Token: 0x0400B3EF RID: 46063
				public static LocString WORKSHOP = "STEAM WORKSHOP";

				// Token: 0x0400B3F0 RID: 46064
				public static LocString ENABLE_ALL = "ENABLE ALL";

				// Token: 0x0400B3F1 RID: 46065
				public static LocString DISABLE_ALL = "DISABLE ALL";

				// Token: 0x0400B3F2 RID: 46066
				public static LocString DRAG_TO_REORDER = "Drag to reorder";

				// Token: 0x0400B3F3 RID: 46067
				public static LocString REQUIRES_RESTART = "Mod changes require restart";

				// Token: 0x0400B3F4 RID: 46068
				public static LocString FAILED_TO_LOAD = "A mod failed to load and is being disabled:\n\n{0}: {1}\n\n{2}";

				// Token: 0x0400B3F5 RID: 46069
				public static LocString DB_CORRUPT = "An error occurred trying to load the Mod Database.\n\n{0}";

				// Token: 0x02002BC4 RID: 11204
				public class CONTENT_FAILURE
				{
					// Token: 0x0400B3F6 RID: 46070
					public static LocString DISABLED_CONTENT = " - <b>Not compatible with <i>{Content}</i></b>";

					// Token: 0x0400B3F7 RID: 46071
					public static LocString NO_CONTENT = " - <b>No compatible mod found</b>";

					// Token: 0x0400B3F8 RID: 46072
					public static LocString OLD_API = " - <b>Mod out-of-date</b>";
				}

				// Token: 0x02002BC5 RID: 11205
				public class TOOLTIPS
				{
					// Token: 0x0400B3F9 RID: 46073
					public static LocString ENABLED = "Enabled";

					// Token: 0x0400B3FA RID: 46074
					public static LocString DISABLED = "Disabled";

					// Token: 0x0400B3FB RID: 46075
					public static LocString MANAGE_STEAM_SUBSCRIPTION = "Manage Steam Subscription";

					// Token: 0x0400B3FC RID: 46076
					public static LocString MANAGE_RAIL_SUBSCRIPTION = "Manage Subscription";

					// Token: 0x0400B3FD RID: 46077
					public static LocString MANAGE_LOCAL_MOD = "Manage Local Mod";
				}

				// Token: 0x02002BC6 RID: 11206
				public class RAILMODUPLOAD
				{
					// Token: 0x0400B3FE RID: 46078
					public static LocString TITLE = "Upload Mod";

					// Token: 0x0400B3FF RID: 46079
					public static LocString NAME = "Mod Name";

					// Token: 0x0400B400 RID: 46080
					public static LocString DESCRIPTION = "Mod Description";

					// Token: 0x0400B401 RID: 46081
					public static LocString VERSION = "Version Number";

					// Token: 0x0400B402 RID: 46082
					public static LocString PREVIEW_IMAGE = "Preview Image Path";

					// Token: 0x0400B403 RID: 46083
					public static LocString CONTENT_FOLDER = "Content Folder Path";

					// Token: 0x0400B404 RID: 46084
					public static LocString SHARE_TYPE = "Share Type";

					// Token: 0x0400B405 RID: 46085
					public static LocString SUBMIT = "Submit";

					// Token: 0x0400B406 RID: 46086
					public static LocString SUBMIT_READY = "This mod is ready to submit";

					// Token: 0x0400B407 RID: 46087
					public static LocString SUBMIT_NOT_READY = "The mod cannot be submitted. Check that all fields are properly entered and that the paths are valid.";

					// Token: 0x02002BC7 RID: 11207
					public static class MOD_SHARE_TYPE
					{
						// Token: 0x0400B408 RID: 46088
						public static LocString PRIVATE = "Private";

						// Token: 0x0400B409 RID: 46089
						public static LocString TOOLTIP_PRIVATE = "This mod will only be visible to its creator";

						// Token: 0x0400B40A RID: 46090
						public static LocString FRIEND = "Friend";

						// Token: 0x0400B40B RID: 46091
						public static LocString TOOLTIP_FRIEND = "Friend";

						// Token: 0x0400B40C RID: 46092
						public static LocString PUBLIC = "Public";

						// Token: 0x0400B40D RID: 46093
						public static LocString TOOLTIP_PUBLIC = "This mod will be available to all players after publishing. It may be subject to review before being allowed to be published.";
					}

					// Token: 0x02002BC8 RID: 11208
					public static class MOD_UPLOAD_RESULT
					{
						// Token: 0x0400B40E RID: 46094
						public static LocString SUCCESS = "Mod upload succeeded.";

						// Token: 0x0400B40F RID: 46095
						public static LocString FAILURE = "Mod upload failed.";
					}
				}
			}

			// Token: 0x02002BC9 RID: 11209
			public class MOD_EVENTS
			{
				// Token: 0x0400B410 RID: 46096
				public static LocString REQUIRED = "REQUIRED";

				// Token: 0x0400B411 RID: 46097
				public static LocString NOT_FOUND = "NOT FOUND";

				// Token: 0x0400B412 RID: 46098
				public static LocString INSTALL_INFO_INACCESSIBLE = "INACCESSIBLE";

				// Token: 0x0400B413 RID: 46099
				public static LocString OUT_OF_ORDER = "ORDERING CHANGED";

				// Token: 0x0400B414 RID: 46100
				public static LocString ACTIVE_DURING_CRASH = "ACTIVE DURING CRASH";

				// Token: 0x0400B415 RID: 46101
				public static LocString EXPECTED_ENABLED = "NEWLY DISABLED";

				// Token: 0x0400B416 RID: 46102
				public static LocString EXPECTED_DISABLED = "NEWLY ENABLED";

				// Token: 0x0400B417 RID: 46103
				public static LocString VERSION_UPDATE = "VERSION UPDATE";

				// Token: 0x0400B418 RID: 46104
				public static LocString AVAILABLE_CONTENT_CHANGED = "CONTENT CHANGED";

				// Token: 0x0400B419 RID: 46105
				public static LocString INSTALL_FAILED = "INSTALL FAILED";

				// Token: 0x0400B41A RID: 46106
				public static LocString DOWNLOAD_FAILED = "STEAM DOWNLOAD FAILED";

				// Token: 0x0400B41B RID: 46107
				public static LocString INSTALLED = "INSTALLED";

				// Token: 0x0400B41C RID: 46108
				public static LocString UNINSTALLED = "UNINSTALLED";

				// Token: 0x0400B41D RID: 46109
				public static LocString REQUIRES_RESTART = "RESTART REQUIRED";

				// Token: 0x0400B41E RID: 46110
				public static LocString BAD_WORLD_GEN = "LOAD FAILED";

				// Token: 0x0400B41F RID: 46111
				public static LocString DEACTIVATED = "DEACTIVATED";

				// Token: 0x0400B420 RID: 46112
				public static LocString ALL_MODS_DISABLED_EARLY_ACCESS = "DEACTIVATED";

				// Token: 0x02002BCA RID: 11210
				public class TOOLTIPS
				{
					// Token: 0x0400B421 RID: 46113
					public static LocString REQUIRED = "The current save game couldn't load this mod. Unexpected things may happen!";

					// Token: 0x0400B422 RID: 46114
					public static LocString NOT_FOUND = "This mod isn't installed";

					// Token: 0x0400B423 RID: 46115
					public static LocString INSTALL_INFO_INACCESSIBLE = "Mod files are inaccessible";

					// Token: 0x0400B424 RID: 46116
					public static LocString OUT_OF_ORDER = "Active mod has changed order with respect to some other active mod";

					// Token: 0x0400B425 RID: 46117
					public static LocString ACTIVE_DURING_CRASH = "Mod was active during a crash and may be the cause";

					// Token: 0x0400B426 RID: 46118
					public static LocString EXPECTED_ENABLED = "This mod needs to be enabled";

					// Token: 0x0400B427 RID: 46119
					public static LocString EXPECTED_DISABLED = "This mod needs to be disabled";

					// Token: 0x0400B428 RID: 46120
					public static LocString VERSION_UPDATE = "New version detected";

					// Token: 0x0400B429 RID: 46121
					public static LocString AVAILABLE_CONTENT_CHANGED = "Content added or removed";

					// Token: 0x0400B42A RID: 46122
					public static LocString INSTALL_FAILED = "Installation failed";

					// Token: 0x0400B42B RID: 46123
					public static LocString DOWNLOAD_FAILED = "Steam failed to download the mod";

					// Token: 0x0400B42C RID: 46124
					public static LocString INSTALLED = "Installation succeeded";

					// Token: 0x0400B42D RID: 46125
					public static LocString UNINSTALLED = "Uninstalled";

					// Token: 0x0400B42E RID: 46126
					public static LocString BAD_WORLD_GEN = "Encountered an error while loading file";

					// Token: 0x0400B42F RID: 46127
					public static LocString DEACTIVATED = "Deactivated due to errors";

					// Token: 0x0400B430 RID: 46128
					public static LocString ALL_MODS_DISABLED_EARLY_ACCESS = "Deactivated due to Early Access for " + UI.DLC1.NAME_ITAL;
				}
			}

			// Token: 0x02002BCB RID: 11211
			public class MOD_DIALOGS
			{
				// Token: 0x0400B431 RID: 46129
				public static LocString ADDITIONAL_MOD_EVENTS = "(...additional entries omitted)";

				// Token: 0x02002BCC RID: 11212
				public class INSTALL_INFO_INACCESSIBLE
				{
					// Token: 0x0400B432 RID: 46130
					public static LocString TITLE = "STEAM CONTENT ERROR";

					// Token: 0x0400B433 RID: 46131
					public static LocString MESSAGE = "Failed to access local Steam files for mod {0}.\nTry restarting Oxygen not Included.\nIf that doesn't work, try re-subscribing to the mod via Steam.";
				}

				// Token: 0x02002BCD RID: 11213
				public class STEAM_SUBSCRIBED
				{
					// Token: 0x0400B434 RID: 46132
					public static LocString TITLE = "STEAM MOD SUBSCRIBED";

					// Token: 0x0400B435 RID: 46133
					public static LocString MESSAGE = "Subscribed to Steam mod: {0}";
				}

				// Token: 0x02002BCE RID: 11214
				public class STEAM_UPDATED
				{
					// Token: 0x0400B436 RID: 46134
					public static LocString TITLE = "STEAM MOD UPDATE";

					// Token: 0x0400B437 RID: 46135
					public static LocString MESSAGE = "Updating version of Steam mod: {0}";
				}

				// Token: 0x02002BCF RID: 11215
				public class STEAM_UNSUBSCRIBED
				{
					// Token: 0x0400B438 RID: 46136
					public static LocString TITLE = "STEAM MOD UNSUBSCRIBED";

					// Token: 0x0400B439 RID: 46137
					public static LocString MESSAGE = "Unsubscribed from Steam mod: {0}";
				}

				// Token: 0x02002BD0 RID: 11216
				public class STEAM_REFRESH
				{
					// Token: 0x0400B43A RID: 46138
					public static LocString TITLE = "STEAM MODS REFRESHED";

					// Token: 0x0400B43B RID: 46139
					public static LocString MESSAGE = "Refreshed Steam mods:\n{0}";
				}

				// Token: 0x02002BD1 RID: 11217
				public class ALL_MODS_DISABLED_EARLY_ACCESS
				{
					// Token: 0x0400B43C RID: 46140
					public static LocString TITLE = "ALL MODS DISABLED";

					// Token: 0x0400B43D RID: 46141
					public static LocString MESSAGE = "Mod support is temporarily suspended for the initial launch of " + UI.DLC1.NAME_ITAL + " into Early Access:\n{0}";
				}

				// Token: 0x02002BD2 RID: 11218
				public class LOAD_FAILURE
				{
					// Token: 0x0400B43E RID: 46142
					public static LocString TITLE = "LOAD FAILURE";

					// Token: 0x0400B43F RID: 46143
					public static LocString MESSAGE = "Failed to load one or more mods:\n{0}\nThey will be re-installed when the game is restarted.\nGame may be unstable until restarted.";
				}

				// Token: 0x02002BD3 RID: 11219
				public class SAVE_GAME_MODS_DIFFER
				{
					// Token: 0x0400B440 RID: 46144
					public static LocString TITLE = "MOD DIFFERENCES";

					// Token: 0x0400B441 RID: 46145
					public static LocString MESSAGE = "Save game mods differ from currently active mods:\n{0}";
				}

				// Token: 0x02002BD4 RID: 11220
				public class MOD_ERRORS_ON_BOOT
				{
					// Token: 0x0400B442 RID: 46146
					public static LocString TITLE = "MOD ERRORS";

					// Token: 0x0400B443 RID: 46147
					public static LocString MESSAGE = "An error occurred during start-up with mods active.\nAll mods have been disabled to ensure a clean restart.\n{0}";

					// Token: 0x0400B444 RID: 46148
					public static LocString DEV_MESSAGE = "An error occurred during start-up with mods active.\n{0}\nDisable all mods and restart, or continue in an unstable state?";
				}

				// Token: 0x02002BD5 RID: 11221
				public class MODS_SCREEN_CHANGES
				{
					// Token: 0x0400B445 RID: 46149
					public static LocString TITLE = "MODS CHANGED";

					// Token: 0x0400B446 RID: 46150
					public static LocString MESSAGE = "{0}\nRestart required to reload mods.\nGame may be unstable until restarted.";
				}

				// Token: 0x02002BD6 RID: 11222
				public class MOD_EVENTS
				{
					// Token: 0x0400B447 RID: 46151
					public static LocString TITLE = "MOD EVENTS";

					// Token: 0x0400B448 RID: 46152
					public static LocString MESSAGE = "{0}";

					// Token: 0x0400B449 RID: 46153
					public static LocString DEV_MESSAGE = "{0}\nCheck Player.log for details.";
				}

				// Token: 0x02002BD7 RID: 11223
				public class RESTART
				{
					// Token: 0x0400B44A RID: 46154
					public static LocString OK = "RESTART";

					// Token: 0x0400B44B RID: 46155
					public static LocString CANCEL = "CONTINUE";

					// Token: 0x0400B44C RID: 46156
					public static LocString MESSAGE = "{0}\nRestart required.";

					// Token: 0x0400B44D RID: 46157
					public static LocString DEV_MESSAGE = "{0}\nRestart required.\nGame may be unstable until restarted.";
				}
			}

			// Token: 0x02002BD8 RID: 11224
			public class PAUSE_SCREEN
			{
				// Token: 0x0400B44E RID: 46158
				public static LocString TITLE = "PAUSED";

				// Token: 0x0400B44F RID: 46159
				public static LocString RESUME = "Resume";

				// Token: 0x0400B450 RID: 46160
				public static LocString LOGBOOK = "Logbook";

				// Token: 0x0400B451 RID: 46161
				public static LocString OPTIONS = "Options";

				// Token: 0x0400B452 RID: 46162
				public static LocString SAVE = "Save";

				// Token: 0x0400B453 RID: 46163
				public static LocString ALREADY_SAVED = "<i><color=#CAC8C8>Already Saved</color></i>";

				// Token: 0x0400B454 RID: 46164
				public static LocString SAVEAS = "Save As";

				// Token: 0x0400B455 RID: 46165
				public static LocString COLONY_SUMMARY = "Colony Summary";

				// Token: 0x0400B456 RID: 46166
				public static LocString LOCKERMENU = "Supply Closet";

				// Token: 0x0400B457 RID: 46167
				public static LocString LOAD = "Load";

				// Token: 0x0400B458 RID: 46168
				public static LocString QUIT = "Main Menu";

				// Token: 0x0400B459 RID: 46169
				public static LocString DESKTOPQUIT = "Quit to Desktop";

				// Token: 0x0400B45A RID: 46170
				public static LocString WORLD_SEED = "Coordinates: {0}";

				// Token: 0x0400B45B RID: 46171
				public static LocString WORLD_SEED_TOOLTIP = "Share coordinates with a friend and they can start a colony on an identical asteroid!\n\n{0} - The asteroid\n\n{1} - The world seed\n\n{2} - Difficulty and Custom settings\n\n{3} - Story Trait settings\n\n{4} - Scramble DLC settings";

				// Token: 0x0400B45C RID: 46172
				public static LocString WORLD_SEED_COPY_TOOLTIP = "Copy Coordinates to clipboard\n\nShare coordinates with a friend and they can start a colony on an identical asteroid!";

				// Token: 0x0400B45D RID: 46173
				public static LocString MANAGEMENT_BUTTON = "Pause Menu";

				// Token: 0x02002BD9 RID: 11225
				public class ADD_DLC_MENU
				{
					// Token: 0x0400B45E RID: 46174
					public static LocString ENABLE_QUESTION = "Enable DLC content on this save?\n\nThis will create a new copy of the save game. It will no longer be possible to load this copy without the DLC enabled.";

					// Token: 0x0400B45F RID: 46175
					public static LocString CONFIRM = "CONFIRM";

					// Token: 0x0400B460 RID: 46176
					public static LocString DLC_ENABLED_TOOLTIP = "This save has content from <b>{0}</b> DLC enabled";

					// Token: 0x0400B461 RID: 46177
					public static LocString DLC_DISABLED_TOOLTIP = "This save does not currently have content from <b>{0}</b> DLC enabled \n\n<b>Click to enable it</b>";

					// Token: 0x0400B462 RID: 46178
					public static LocString DLC_DISABLED_NOT_EDITABLE_TOOLTIP = "This save does not have content from the <b>{0}</b> DLC enabled";
				}
			}

			// Token: 0x02002BDA RID: 11226
			public class OPTIONS_SCREEN
			{
				// Token: 0x0400B463 RID: 46179
				public static LocString TITLE = "OPTIONS";

				// Token: 0x0400B464 RID: 46180
				public static LocString GRAPHICS = "Graphics";

				// Token: 0x0400B465 RID: 46181
				public static LocString AUDIO = "Audio";

				// Token: 0x0400B466 RID: 46182
				public static LocString GAME = "Game";

				// Token: 0x0400B467 RID: 46183
				public static LocString CONTROLS = "Controls";

				// Token: 0x0400B468 RID: 46184
				public static LocString UNITS = "Temperature Units";

				// Token: 0x0400B469 RID: 46185
				public static LocString METRICS = "Data Communication";

				// Token: 0x0400B46A RID: 46186
				public static LocString LANGUAGE = "Change Language";

				// Token: 0x0400B46B RID: 46187
				public static LocString WORLD_GEN = "World Generation Key";

				// Token: 0x0400B46C RID: 46188
				public static LocString RESET_TUTORIAL = "Reset Tutorial Messages";

				// Token: 0x0400B46D RID: 46189
				public static LocString RESET_TUTORIAL_WARNING = "All tutorial messages will be reset, and\nwill show up again the next time you play the game.";

				// Token: 0x0400B46E RID: 46190
				public static LocString FEEDBACK = "Feedback";

				// Token: 0x0400B46F RID: 46191
				public static LocString CREDITS = "Credits";

				// Token: 0x0400B470 RID: 46192
				public static LocString BACK = "Done";

				// Token: 0x0400B471 RID: 46193
				public static LocString UNLOCK_SANDBOX = "Unlock Sandbox Mode";

				// Token: 0x0400B472 RID: 46194
				public static LocString MODS = "MODS";

				// Token: 0x0400B473 RID: 46195
				public static LocString SAVE_OPTIONS = "Save Options";

				// Token: 0x02002BDB RID: 11227
				public class TOGGLE_SANDBOX_SCREEN
				{
					// Token: 0x0400B474 RID: 46196
					public static LocString UNLOCK_SANDBOX_WARNING = "Sandbox Mode will be enabled for this save file";

					// Token: 0x0400B475 RID: 46197
					public static LocString CONFIRM = "Enable Sandbox Mode";

					// Token: 0x0400B476 RID: 46198
					public static LocString CANCEL = "Cancel";

					// Token: 0x0400B477 RID: 46199
					public static LocString CONFIRM_SAVE_BACKUP = "Enable Sandbox Mode, but save a backup first";

					// Token: 0x0400B478 RID: 46200
					public static LocString BACKUP_SAVE_GAME_APPEND = " (BACKUP)";
				}
			}

			// Token: 0x02002BDC RID: 11228
			public class INPUT_BINDINGS_SCREEN
			{
				// Token: 0x0400B479 RID: 46201
				public static LocString TITLE = "CUSTOMIZE KEYS";

				// Token: 0x0400B47A RID: 46202
				public static LocString RESET = "Reset";

				// Token: 0x0400B47B RID: 46203
				public static LocString APPLY = "Done";

				// Token: 0x0400B47C RID: 46204
				public static LocString DUPLICATE = "{0} was already bound to {1} and is now unbound.";

				// Token: 0x0400B47D RID: 46205
				public static LocString UNBOUND_ACTION = "{0} is unbound. Are you sure you want to continue?";

				// Token: 0x0400B47E RID: 46206
				public static LocString MULTIPLE_UNBOUND_ACTIONS = "You have multiple unbound actions, this may result in difficulty playing the game. Are you sure you want to continue?";

				// Token: 0x0400B47F RID: 46207
				public static LocString WAITING_FOR_INPUT = "???";
			}

			// Token: 0x02002BDD RID: 11229
			public class TRANSLATIONS_SCREEN
			{
				// Token: 0x0400B480 RID: 46208
				public static LocString TITLE = "TRANSLATIONS";

				// Token: 0x0400B481 RID: 46209
				public static LocString UNINSTALL = "Uninstall";

				// Token: 0x0400B482 RID: 46210
				public static LocString PREINSTALLED_HEADER = "Preinstalled Language Packs";

				// Token: 0x0400B483 RID: 46211
				public static LocString UGC_HEADER = "Subscribed Workshop Language Packs";

				// Token: 0x0400B484 RID: 46212
				public static LocString UGC_MOD_TITLE_FORMAT = "{0} (workshop)";

				// Token: 0x0400B485 RID: 46213
				public static LocString ARE_YOU_SURE = "Are you sure you want to uninstall this language pack?";

				// Token: 0x0400B486 RID: 46214
				public static LocString PLEASE_REBOOT = "Please restart your game for these changes to take effect.";

				// Token: 0x0400B487 RID: 46215
				public static LocString NO_PACKS = "Steam Workshop";

				// Token: 0x0400B488 RID: 46216
				public static LocString DOWNLOAD = "Start Download";

				// Token: 0x0400B489 RID: 46217
				public static LocString INSTALL = "Install";

				// Token: 0x0400B48A RID: 46218
				public static LocString INSTALLED = "Installed";

				// Token: 0x0400B48B RID: 46219
				public static LocString NO_STEAM = "Unable to retrieve language list from Steam";

				// Token: 0x0400B48C RID: 46220
				public static LocString RESTART = "RESTART";

				// Token: 0x0400B48D RID: 46221
				public static LocString CANCEL = "CANCEL";

				// Token: 0x0400B48E RID: 46222
				public static LocString MISSING_LANGUAGE_PACK = "Selected language pack ({0}) not found.\nReverting to default language.";

				// Token: 0x0400B48F RID: 46223
				public static LocString UNKNOWN = "Unknown";

				// Token: 0x02002BDE RID: 11230
				public class PREINSTALLED_LANGUAGES
				{
					// Token: 0x0400B490 RID: 46224
					public static LocString EN = "English (Klei)";

					// Token: 0x0400B491 RID: 46225
					public static LocString ZH_KLEI = "Chinese (Klei)";

					// Token: 0x0400B492 RID: 46226
					public static LocString KO_KLEI = "Korean (Klei)";

					// Token: 0x0400B493 RID: 46227
					public static LocString RU_KLEI = "Russian (Klei)";
				}
			}

			// Token: 0x02002BDF RID: 11231
			public class SCENARIOS_MENU
			{
				// Token: 0x0400B494 RID: 46228
				public static LocString TITLE = "Scenarios";

				// Token: 0x0400B495 RID: 46229
				public static LocString UNSUBSCRIBE = "Unsubscribe";

				// Token: 0x0400B496 RID: 46230
				public static LocString UNSUBSCRIBE_CONFIRM = "Are you sure you want to unsubscribe from this scenario?";

				// Token: 0x0400B497 RID: 46231
				public static LocString LOAD_SCENARIO_CONFIRM = "Load the \"{SCENARIO_NAME}\" scenario?";

				// Token: 0x0400B498 RID: 46232
				public static LocString LOAD_CONFIRM_TITLE = "LOAD";

				// Token: 0x0400B499 RID: 46233
				public static LocString SCENARIO_NAME = "Name:";

				// Token: 0x0400B49A RID: 46234
				public static LocString SCENARIO_DESCRIPTION = "Description";

				// Token: 0x0400B49B RID: 46235
				public static LocString BUTTON_DONE = "Done";

				// Token: 0x0400B49C RID: 46236
				public static LocString BUTTON_LOAD = "Load";

				// Token: 0x0400B49D RID: 46237
				public static LocString BUTTON_WORKSHOP = "Steam Workshop";

				// Token: 0x0400B49E RID: 46238
				public static LocString NO_SCENARIOS_AVAILABLE = "No scenarios available.\n\nSubscribe to some in the Steam Workshop.";
			}

			// Token: 0x02002BE0 RID: 11232
			public class AUDIO_OPTIONS_SCREEN
			{
				// Token: 0x0400B49F RID: 46239
				public static LocString TITLE = "AUDIO OPTIONS";

				// Token: 0x0400B4A0 RID: 46240
				public static LocString HEADER_VOLUME = "VOLUME";

				// Token: 0x0400B4A1 RID: 46241
				public static LocString HEADER_SETTINGS = "SETTINGS";

				// Token: 0x0400B4A2 RID: 46242
				public static LocString DONE_BUTTON = "Done";

				// Token: 0x0400B4A3 RID: 46243
				public static LocString MUSIC_EVERY_CYCLE = "Play background music each morning";

				// Token: 0x0400B4A4 RID: 46244
				public static LocString MUSIC_EVERY_CYCLE_TOOLTIP = "If enabled, background music will play every cycle instead of every few cycles";

				// Token: 0x0400B4A5 RID: 46245
				public static LocString AUTOMATION_SOUNDS_ALWAYS = "Always play automation sounds";

				// Token: 0x0400B4A6 RID: 46246
				public static LocString AUTOMATION_SOUNDS_ALWAYS_TOOLTIP = "If enabled, automation sound effects will play even when outside of the " + UI.FormatAsOverlay("Automation Overlay");

				// Token: 0x0400B4A7 RID: 46247
				public static LocString MUTE_ON_FOCUS_LOST = "Mute when unfocused";

				// Token: 0x0400B4A8 RID: 46248
				public static LocString MUTE_ON_FOCUS_LOST_TOOLTIP = "If enabled, the game will be muted while minimized or if the application loses focus";

				// Token: 0x0400B4A9 RID: 46249
				public static LocString AUDIO_BUS_MASTER = "Master";

				// Token: 0x0400B4AA RID: 46250
				public static LocString AUDIO_BUS_SFX = "SFX";

				// Token: 0x0400B4AB RID: 46251
				public static LocString AUDIO_BUS_MUSIC = "Music";

				// Token: 0x0400B4AC RID: 46252
				public static LocString AUDIO_BUS_AMBIENCE = "Ambience";

				// Token: 0x0400B4AD RID: 46253
				public static LocString AUDIO_BUS_UI = "UI";
			}

			// Token: 0x02002BE1 RID: 11233
			public class GAME_OPTIONS_SCREEN
			{
				// Token: 0x0400B4AE RID: 46254
				public static LocString TITLE = "GAME OPTIONS";

				// Token: 0x0400B4AF RID: 46255
				public static LocString GENERAL_GAME_OPTIONS = "GENERAL";

				// Token: 0x0400B4B0 RID: 46256
				public static LocString DISABLED_WARNING = "More options available in-game";

				// Token: 0x0400B4B1 RID: 46257
				public static LocString DEFAULT_TO_CLOUD_SAVES = "Default to cloud saves";

				// Token: 0x0400B4B2 RID: 46258
				public static LocString DEFAULT_TO_CLOUD_SAVES_TOOLTIP = "When a new colony is created, this controls whether it will be saved into the cloud saves folder for syncing or not.";

				// Token: 0x0400B4B3 RID: 46259
				public static LocString RESET_TUTORIAL_DESCRIPTION = "Mark all tutorial messages \"unread\"";

				// Token: 0x0400B4B4 RID: 46260
				public static LocString SANDBOX_DESCRIPTION = "Enable sandbox tools";

				// Token: 0x0400B4B5 RID: 46261
				public static LocString CONTROLS_DESCRIPTION = "Change key bindings";

				// Token: 0x0400B4B6 RID: 46262
				public static LocString TEMPERATURE_UNITS = "TEMPERATURE UNITS";

				// Token: 0x0400B4B7 RID: 46263
				public static LocString SAVE_OPTIONS = "SAVE";

				// Token: 0x0400B4B8 RID: 46264
				public static LocString CAMERA_SPEED_LABEL = "Camera Pan Speed: {0}%";
			}

			// Token: 0x02002BE2 RID: 11234
			public class METRIC_OPTIONS_SCREEN
			{
				// Token: 0x0400B4B9 RID: 46265
				public static LocString TITLE = "DATA COMMUNICATION";

				// Token: 0x0400B4BA RID: 46266
				public static LocString HEADER_METRICS = "USER DATA";
			}

			// Token: 0x02002BE3 RID: 11235
			public class COLONY_SAVE_OPTIONS_SCREEN
			{
				// Token: 0x0400B4BB RID: 46267
				public static LocString TITLE = "COLONY SAVE OPTIONS";

				// Token: 0x0400B4BC RID: 46268
				public static LocString DESCRIPTION = "Note: These values are configured per save file";

				// Token: 0x0400B4BD RID: 46269
				public static LocString AUTOSAVE_FREQUENCY = "Autosave frequency:";

				// Token: 0x0400B4BE RID: 46270
				public static LocString AUTOSAVE_FREQUENCY_DESCRIPTION = "Every: {0} cycle(s)";

				// Token: 0x0400B4BF RID: 46271
				public static LocString AUTOSAVE_NEVER = "Never";

				// Token: 0x0400B4C0 RID: 46272
				public static LocString TIMELAPSE_RESOLUTION = "Timelapse resolution:";

				// Token: 0x0400B4C1 RID: 46273
				public static LocString TIMELAPSE_RESOLUTION_DESCRIPTION = "{0}x{1}";

				// Token: 0x0400B4C2 RID: 46274
				public static LocString TIMELAPSE_DISABLED_DESCRIPTION = "Disabled";
			}

			// Token: 0x02002BE4 RID: 11236
			public class FEEDBACK_SCREEN
			{
				// Token: 0x0400B4C3 RID: 46275
				public static LocString TITLE = "FEEDBACK";

				// Token: 0x0400B4C4 RID: 46276
				public static LocString HEADER = "We would love to hear from you!";

				// Token: 0x0400B4C5 RID: 46277
				public static LocString DESCRIPTION = "Let us know if you encounter any problems or how we can improve your Oxygen Not Included experience.\n\nWhen reporting a bug, please include your log and colony save file. The buttons to the right will help you find those files on your local drive.\n\nThank you for being part of the Oxygen Not Included community!";

				// Token: 0x0400B4C6 RID: 46278
				public static LocString ALT_DESCRIPTION = "Let us know if you encounter any problems or how we can improve your Oxygen Not Included experience.\n\nWhen reporting a bug, please include your log and colony save file.\n\nThank you for being part of the Oxygen Not Included community!";

				// Token: 0x0400B4C7 RID: 46279
				public static LocString BUG_FORUMS_BUTTON = "Report a Bug";

				// Token: 0x0400B4C8 RID: 46280
				public static LocString SUGGESTION_FORUMS_BUTTON = "Suggestions Forum";

				// Token: 0x0400B4C9 RID: 46281
				public static LocString LOGS_DIRECTORY_BUTTON = "Browse Log Files";

				// Token: 0x0400B4CA RID: 46282
				public static LocString SAVE_FILES_DIRECTORY_BUTTON = "Browse Save Files";
			}

			// Token: 0x02002BE5 RID: 11237
			public class WORLD_GEN_OPTIONS_SCREEN
			{
				// Token: 0x0400B4CB RID: 46283
				public static LocString TITLE = "WORLD GENERATION OPTIONS";

				// Token: 0x0400B4CC RID: 46284
				public static LocString USE_SEED = "Set Worldgen Seed";

				// Token: 0x0400B4CD RID: 46285
				public static LocString DONE_BUTTON = "Done";

				// Token: 0x0400B4CE RID: 46286
				public static LocString RANDOM_BUTTON = "Randomize";

				// Token: 0x0400B4CF RID: 46287
				public static LocString RANDOM_BUTTON_TOOLTIP = "Randomize a new worldgen seed";

				// Token: 0x0400B4D0 RID: 46288
				public static LocString TOOLTIP = "This will override the current worldgen seed";
			}

			// Token: 0x02002BE6 RID: 11238
			public class METRICS_OPTIONS_SCREEN
			{
				// Token: 0x0400B4D1 RID: 46289
				public static LocString TITLE = "DATA COMMUNICATION OPTIONS";

				// Token: 0x0400B4D2 RID: 46290
				public static LocString ENABLE_BUTTON = "Enable Data Communication";

				// Token: 0x0400B4D3 RID: 46291
				public static LocString DESCRIPTION = "Collecting user data helps us improve the game.\n\nPlayers who opt out of data communication will no longer send us crash reports and play data.\n\nThey will also be unable to receive new item unlocks from our servers, though existing unlocked items will continue to function.\n\nFor more details on our privacy policy and how we use the data we collect, please visit our <color=#ECA6C9><u><b>privacy center</b></u></color>.";

				// Token: 0x0400B4D4 RID: 46292
				public static LocString DONE_BUTTON = "Done";

				// Token: 0x0400B4D5 RID: 46293
				public static LocString RESTART_BUTTON = "Restart Game";

				// Token: 0x0400B4D6 RID: 46294
				public static LocString TOOLTIP = "Uncheck to disable data communication";

				// Token: 0x0400B4D7 RID: 46295
				public static LocString RESTART_WARNING = "A game restart is required to apply settings.";
			}

			// Token: 0x02002BE7 RID: 11239
			public class UNIT_OPTIONS_SCREEN
			{
				// Token: 0x0400B4D8 RID: 46296
				public static LocString TITLE = "TEMPERATURE UNITS";

				// Token: 0x0400B4D9 RID: 46297
				public static LocString CELSIUS = "Celsius";

				// Token: 0x0400B4DA RID: 46298
				public static LocString CELSIUS_TOOLTIP = "Change temperature unit to Celsius (°C)";

				// Token: 0x0400B4DB RID: 46299
				public static LocString KELVIN = "Kelvin";

				// Token: 0x0400B4DC RID: 46300
				public static LocString KELVIN_TOOLTIP = "Change temperature unit to Kelvin (K)";

				// Token: 0x0400B4DD RID: 46301
				public static LocString FAHRENHEIT = "Fahrenheit";

				// Token: 0x0400B4DE RID: 46302
				public static LocString FAHRENHEIT_TOOLTIP = "Change temperature unit to Fahrenheit (°F)";
			}

			// Token: 0x02002BE8 RID: 11240
			public class GRAPHICS_OPTIONS_SCREEN
			{
				// Token: 0x0400B4DF RID: 46303
				public static LocString TITLE = "GRAPHICS OPTIONS";

				// Token: 0x0400B4E0 RID: 46304
				public static LocString FULLSCREEN = "Fullscreen";

				// Token: 0x0400B4E1 RID: 46305
				public static LocString RESOLUTION = "Resolution:";

				// Token: 0x0400B4E2 RID: 46306
				public static LocString LOWRES = "Low Resolution Textures";

				// Token: 0x0400B4E3 RID: 46307
				public static LocString APPLYBUTTON = "Apply";

				// Token: 0x0400B4E4 RID: 46308
				public static LocString REVERTBUTTON = "Revert";

				// Token: 0x0400B4E5 RID: 46309
				public static LocString DONE_BUTTON = "Done";

				// Token: 0x0400B4E6 RID: 46310
				public static LocString UI_SCALE = "UI Scale";

				// Token: 0x0400B4E7 RID: 46311
				public static LocString HEADER_DISPLAY = "DISPLAY";

				// Token: 0x0400B4E8 RID: 46312
				public static LocString HEADER_UI = "INTERFACE";

				// Token: 0x0400B4E9 RID: 46313
				public static LocString COLORMODE = "Color Mode:";

				// Token: 0x0400B4EA RID: 46314
				public static LocString COLOR_MODE_DEFAULT = "Default";

				// Token: 0x0400B4EB RID: 46315
				public static LocString COLOR_MODE_PROTANOPIA = "Protanopia";

				// Token: 0x0400B4EC RID: 46316
				public static LocString COLOR_MODE_DEUTERANOPIA = "Deuteranopia";

				// Token: 0x0400B4ED RID: 46317
				public static LocString COLOR_MODE_TRITANOPIA = "Tritanopia";

				// Token: 0x0400B4EE RID: 46318
				public static LocString ACCEPT_CHANGES = "Accept Changes?";

				// Token: 0x0400B4EF RID: 46319
				public static LocString ACCEPT_CHANGES_STRING_COLOR = "Interface changes will be visible immediately, but applying color changes to in-game text will require a restart.\n\nAccept Changes?";

				// Token: 0x0400B4F0 RID: 46320
				public static LocString COLORBLIND_FEEDBACK = "Color blindness options are currently in progress.\n\nIf you would benefit from an alternative color mode or have had difficulties with any of the default colors, please visit the forums and let us know about your experiences.\n\nYour feedback is extremely helpful to us!";

				// Token: 0x0400B4F1 RID: 46321
				public static LocString COLORBLIND_FEEDBACK_BUTTON = "Provide Feedback";
			}

			// Token: 0x02002BE9 RID: 11241
			public class WORLDGENSCREEN
			{
				// Token: 0x0400B4F2 RID: 46322
				public static LocString TITLE = "NEW GAME";

				// Token: 0x0400B4F3 RID: 46323
				public static LocString GENERATINGWORLD = "GENERATING WORLD";

				// Token: 0x0400B4F4 RID: 46324
				public static LocString SELECTSIZEPROMPT = "A new world is about to be created. Please select its size.";

				// Token: 0x0400B4F5 RID: 46325
				public static LocString LOADINGGAME = "LOADING WORLD...";

				// Token: 0x02002BEA RID: 11242
				public class SIZES
				{
					// Token: 0x0400B4F6 RID: 46326
					public static LocString TINY = "Tiny";

					// Token: 0x0400B4F7 RID: 46327
					public static LocString SMALL = "Small";

					// Token: 0x0400B4F8 RID: 46328
					public static LocString STANDARD = "Standard";

					// Token: 0x0400B4F9 RID: 46329
					public static LocString LARGE = "Big";

					// Token: 0x0400B4FA RID: 46330
					public static LocString HUGE = "Colossal";
				}
			}

			// Token: 0x02002BEB RID: 11243
			public class MINSPECSCREEN
			{
				// Token: 0x0400B4FB RID: 46331
				public static LocString TITLE = "WARNING!";

				// Token: 0x0400B4FC RID: 46332
				public static LocString SIMFAILEDTOLOAD = "A problem occurred loading Oxygen Not Included. This is usually caused by the Visual Studio C++ 2015 runtime being improperly installed on the system. Please exit the game, run Windows Update, and try re-launching Oxygen Not Included.";

				// Token: 0x0400B4FD RID: 46333
				public static LocString BODY = "We've detected that this computer does not meet the minimum requirements to run Oxygen Not Included. While you may continue with your current specs, the game might not run smoothly for you.\n\nPlease be aware that your experience may suffer as a result.";

				// Token: 0x0400B4FE RID: 46334
				public static LocString OKBUTTON = "Okay, thanks!";

				// Token: 0x0400B4FF RID: 46335
				public static LocString QUITBUTTON = "Quit";
			}

			// Token: 0x02002BEC RID: 11244
			public class SUPPORTWARNINGS
			{
				// Token: 0x0400B500 RID: 46336
				public static LocString AUDIO_DRIVERS = "A problem occurred initializing your audio device.\nSorry about that!\n\nThis is usually caused by outdated audio drivers.\n\nPlease visit your audio device manufacturer's website to download the latest drivers.";

				// Token: 0x0400B501 RID: 46337
				public static LocString AUDIO_DRIVERS_MORE_INFO = "More Info";

				// Token: 0x0400B502 RID: 46338
				public static LocString DUPLICATE_KEY_BINDINGS = "<b>Duplicate key bindings were detected.\nThis may be because your custom key bindings conflicted with a new feature's default key.\nPlease visit the controls screen to ensure your key bindings are set how you like them.</b>\n{0}";

				// Token: 0x0400B503 RID: 46339
				public static LocString SAVE_DIRECTORY_READ_ONLY = "A problem occurred while accessing your save directory.\nThis may be because your directory is set to read-only.\n\nPlease ensure your save directory is readable as well as writable and re-launch the game.\n{0}";

				// Token: 0x0400B504 RID: 46340
				public static LocString SAVE_DIRECTORY_INSUFFICIENT_SPACE = "There is insufficient disk space to write to your save directory.\n\nPlease free at least 15 MB to give your saves some room to breathe.\n{0}";

				// Token: 0x0400B505 RID: 46341
				public static LocString WORLD_GEN_FILES = "A problem occurred while accessing certain game files that will prevent starting new games.\n\nPlease ensure that the directory and files are readable as well as writable and re-launch the game:\n\n{0}";

				// Token: 0x0400B506 RID: 46342
				public static LocString WORLD_GEN_FAILURE = "A problem occurred while generating a world from this seed:\n{0}.\n\nUnfortunately, not all seeds germinate. Please try again with a different seed.";

				// Token: 0x0400B507 RID: 46343
				public static LocString WORLD_GEN_FAILURE_MIXING = "A problem occurred while trying to mix a world from this seed:\n{0}.\n\nUnfortunately, not all seeds germinate. Please try again with different remix settings or a different seed.";

				// Token: 0x0400B508 RID: 46344
				public static LocString WORLD_GEN_FAILURE_STORY = "A problem occurred while generating a world from this seed:\n{0}.\n\nNot all story traits were able to be placed. Please try again with a different seed or fewer story traits.";

				// Token: 0x0400B509 RID: 46345
				public static LocString PLAYER_PREFS_CORRUPTED = "A problem occurred while loading your game options.\nThey have been reset to their default settings.\n\n";

				// Token: 0x0400B50A RID: 46346
				public static LocString IO_UNAUTHORIZED = "An Unauthorized Access Error occurred when trying to write to disk.\n\nPlease check that you have permissions to write to:\n{0}\n\nThis may prevent the game from saving.";

				// Token: 0x0400B50B RID: 46347
				public static LocString IO_UNAUTHORIZED_ONEDRIVE = "An Unauthorized Access Error occurred when trying to write to disk.\n\nOneDrive may be interfering with the game.\n\nPlease check that you have permissions to write to:\n{0}\n\nThis may prevent the game from saving.";

				// Token: 0x0400B50C RID: 46348
				public static LocString IO_SUFFICIENT_SPACE = "An Insufficient Space Error occurred when trying to write to disk. \n\nPlease free up some space.\n{0}";

				// Token: 0x0400B50D RID: 46349
				public static LocString IO_UNKNOWN = "An unknown error occurred when trying to write or access a file.\n{0}";

				// Token: 0x0400B50E RID: 46350
				public static LocString MORE_INFO_BUTTON = "More Info";
			}

			// Token: 0x02002BED RID: 11245
			public class SAVEUPGRADEWARNINGS
			{
				// Token: 0x0400B50F RID: 46351
				public static LocString SUDDENMORALEHELPER_TITLE = "MORALE CHANGES";

				// Token: 0x0400B510 RID: 46352
				public static LocString SUDDENMORALEHELPER = "Welcome to the Expressive Upgrade! This update introduces a new Morale system that replaces Food and Decor Expectations that were found in previous versions of the game.\n\nThe game you are trying to load was created before this system was introduced, and will need to be updated. You may either:\n\n\n1) Enable the new Morale system in this save, removing Food and Decor Expectations. It's possible that when you load your save your old colony won't meet your Duplicants' new Morale needs, so they'll receive a 5 cycle Morale boost to give you time to adjust.\n\n2) Disable Morale in this save. The new Morale mechanics will still be visible, but won't affect your Duplicants' stress. Food and Decor expectations will no longer exist in this save.";

				// Token: 0x0400B511 RID: 46353
				public static LocString SUDDENMORALEHELPER_BUFF = "1) Bring on Morale!";

				// Token: 0x0400B512 RID: 46354
				public static LocString SUDDENMORALEHELPER_DISABLE = "2) Disable Morale";

				// Token: 0x0400B513 RID: 46355
				public static LocString NEWAUTOMATIONWARNING_TITLE = "AUTOMATION CHANGES";

				// Token: 0x0400B514 RID: 46356
				public static LocString NEWAUTOMATIONWARNING = "The following buildings have acquired new automation ports!\n\nTake a moment to check whether these buildings in your colony are now unintentionally connected to existing " + BUILDINGS.PREFABS.LOGICWIRE.NAME + "s.";

				// Token: 0x0400B515 RID: 46357
				public static LocString MERGEDOWNCHANGES_TITLE = "BREATH OF FRESH AIR UPDATE CHANGES";

				// Token: 0x0400B516 RID: 46358
				public static LocString MERGEDOWNCHANGES = "Oxygen Not Included has had a <b>major update</b> since this save file was created! In addition to the <b>multitude of bug fixes and quality-of-life features</b>, please pay attention to these changes which may affect your existing colony:";

				// Token: 0x0400B517 RID: 46359
				public static LocString MERGEDOWNCHANGES_FOOD = "•<indent=20px>Fridges are more effective for early-game food storage</indent>\n•<indent=20px><b>Both</b> freezing temperatures and a sterile gas are needed for <b>total food preservation</b>.</indent>";

				// Token: 0x0400B518 RID: 46360
				public static LocString MERGEDOWNCHANGES_AIRFILTER = "•<indent=20px>" + BUILDINGS.PREFABS.AIRFILTER.NAME + " now requires <b>5w Power</b>.</indent>\n•<indent=20px>Duplicants will get <b>Stinging Eyes</b> from gasses such as chlorine and hydrogen.</indent>";

				// Token: 0x0400B519 RID: 46361
				public static LocString MERGEDOWNCHANGES_SIMULATION = "•<indent=20px>Many <b>simulation bugs</b> have been fixed.</indent>\n•<indent=20px>This may <b>change the effectiveness</b> of certain contraptions and " + BUILDINGS.PREFABS.STEAMTURBINE2.NAME + " setups.</indent>";

				// Token: 0x0400B51A RID: 46362
				public static LocString MERGEDOWNCHANGES_BUILDINGS = "•<indent=20px>The <b>" + BUILDINGS.PREFABS.OXYGENMASKSTATION.NAME + "</b> has been added to aid early-game exploration.</indent>\n•<indent=20px>Use the new <b>Meter Valves</b> for precise control of resources in pipes.</indent>";

				// Token: 0x0400B51B RID: 46363
				public static LocString SPACESCANNERANDTELESCOPECHANGES_TITLE = "JUNE 2023 QoL UPDATE CHANGES";

				// Token: 0x0400B51C RID: 46364
				public static LocString SPACESCANNERANDTELESCOPECHANGES_SUMMARY = "There have been significant changes to <b>Space Scanners</b> and <b>Telescopes</b> since this save file was created!\n\nMeteor showers have been disabled for 20 cycles to provide time to adapt.";

				// Token: 0x0400B51D RID: 46365
				public static LocString SPACESCANNERANDTELESCOPECHANGES_WARNING = "Please note these changes which may affect your existing colony:\n\n";

				// Token: 0x0400B51E RID: 46366
				public static LocString SPACESCANNERANDTELESCOPECHANGES_SPACESCANNERS = "•<indent=20px>Automation is synced between all Space Scanners targeting the same object.</indent>\n•<indent=20px>Network quality based on the total percentage of sky covered.</indent>\n•<indent=20px>Industrial machinery no longer impacts network quality.</indent>";

				// Token: 0x0400B51F RID: 46367
				public static LocString SPACESCANNERANDTELESCOPECHANGES_TELESCOPES = "•<indent=20px>Telescopes have a symmetrical scanning range.</indent>\n•<indent=20px>Obstructions block visibility from the blocked tile out toward the outer edge of scanning range.</indent>";

				// Token: 0x0400B520 RID: 46368
				public static LocString U50_CHANGES_TITLE = "IMPORTANT CHANGES";

				// Token: 0x0400B521 RID: 46369
				public static LocString U50_CHANGES_SUMMARY = "There have been significant changes to critters since this save file was created! Please check on your ranches.";

				// Token: 0x0400B522 RID: 46370
				public static LocString U50_CHANGES_MOOD = "•<indent=20px>Critter moods have been expanded to include miserable and satisfied states: Miserable stops reproduction. Satisfied gives full metabolism and default reproduction.</indent>";

				// Token: 0x0400B523 RID: 46371
				public static LocString U50_CHANGES_PACU = "•<indent=20px>Pacus have received a number of bug fixes and changes affecting their reproduction: Now correctly Confined when flopping or in less than 8 tiles of liquid. Easier to feed due to a rebalanced diet.</indent>";

				// Token: 0x0400B524 RID: 46372
				public static LocString U50_CHANGES_SUITCHECKPOINTS = "•<indent=20px>Suit checkpoints now have an automation port to disable them so Duplicants can pass through. Some checkpoints may now be unintentionally connected to existing " + BUILDINGS.PREFABS.LOGICWIRE.NAME + "s.";

				// Token: 0x0400B525 RID: 46373
				public static LocString U50_CHANGES_METER_VALVES = "•<indent=20px>Meter valves no longer continuously reset when receiving a green signal.</indent>";
			}
		}

		// Token: 0x02002BEE RID: 11246
		public class SANDBOX_TOGGLE
		{
			// Token: 0x0400B526 RID: 46374
			public static LocString TOOLTIP_LOCKED = "<b>Sandbox Mode</b> must be unlocked in the options menu before it can be used. {Hotkey}";

			// Token: 0x0400B527 RID: 46375
			public static LocString TOOLTIP_UNLOCKED = "Toggle <b>Sandbox Mode</b> {Hotkey}";
		}

		// Token: 0x02002BEF RID: 11247
		public class SKILLS_SCREEN
		{
			// Token: 0x0400B528 RID: 46376
			public static LocString CURRENT_MORALE = "Current Morale: {0}\nMorale Need: {1}";

			// Token: 0x0400B529 RID: 46377
			public static LocString SORT_BY_DUPLICANT = "Duplicants";

			// Token: 0x0400B52A RID: 46378
			public static LocString SORT_BY_MORALE = "Morale";

			// Token: 0x0400B52B RID: 46379
			public static LocString SORT_BY_EXPERIENCE = "Skill Points";

			// Token: 0x0400B52C RID: 46380
			public static LocString SORT_BY_SKILL_AVAILABLE = "Skill Points";

			// Token: 0x0400B52D RID: 46381
			public static LocString SORT_BY_HAT = "Hat";

			// Token: 0x0400B52E RID: 46382
			public static LocString SELECT_HAT = "<b>SELECT HAT</b>";

			// Token: 0x0400B52F RID: 46383
			public static LocString POINTS_AVAILABLE = "<b>SKILL POINTS AVAILABLE</b>";

			// Token: 0x0400B530 RID: 46384
			public static LocString MORALE = "<b>Morale</b>";

			// Token: 0x0400B531 RID: 46385
			public static LocString MORALE_EXPECTATION = "<b>Morale Need</b>";

			// Token: 0x0400B532 RID: 46386
			public static LocString EXPERIENCE = "EXPERIENCE TO NEXT LEVEL";

			// Token: 0x0400B533 RID: 46387
			public static LocString EXPERIENCE_TOOLTIP = "{0}exp to next Skill Point";

			// Token: 0x0400B534 RID: 46388
			public static LocString NOT_AVAILABLE = "Not available";

			// Token: 0x02002BF0 RID: 11248
			public class ASSIGNMENT_REQUIREMENTS
			{
				// Token: 0x0400B535 RID: 46389
				public static LocString EXPECTATION_TARGET_SKILL = "Current Morale: {0}\nSkill Morale Needs: {1}";

				// Token: 0x0400B536 RID: 46390
				public static LocString EXPECTATION_ALERT_TARGET_SKILL = "{2}'s Current: {0} Morale\n{3} Minimum Morale: {1}";

				// Token: 0x0400B537 RID: 46391
				public static LocString EXPECTATION_ALERT_DESC_EXPECTATION = "This Duplicant's Morale is too low to handle the rigors of this position, which will cause them Stress over time.";

				// Token: 0x02002BF1 RID: 11249
				public class SKILLGROUP_ENABLED
				{
					// Token: 0x0400B538 RID: 46392
					public static LocString NAME = "Can perform {0}";

					// Token: 0x0400B539 RID: 46393
					public static LocString DESCRIPTION = "Capable of performing <b>{0}</b> skills";
				}

				// Token: 0x02002BF2 RID: 11250
				public class MASTERY
				{
					// Token: 0x0400B53A RID: 46394
					public static LocString CAN_MASTER = "{0} <b>can learn</b> {1}";

					// Token: 0x0400B53B RID: 46395
					public static LocString HAS_MASTERED = "{0} has <b>already learned</b> {1}";

					// Token: 0x0400B53C RID: 46396
					public static LocString CANNOT_MASTER = "{0} <b>cannot learn</b> {1}";

					// Token: 0x0400B53D RID: 46397
					public static LocString STRESS_WARNING_MESSAGE = string.Concat(new string[]
					{
						"Learning {0} will put {1} into a ",
						UI.PRE_KEYWORD,
						"Morale",
						UI.PST_KEYWORD,
						" deficit and cause unnecessary ",
						UI.PRE_KEYWORD,
						"Stress",
						UI.PST_KEYWORD,
						"!"
					});

					// Token: 0x0400B53E RID: 46398
					public static LocString REQUIRES_MORE_SKILL_POINTS = "    • Not enough " + UI.PRE_KEYWORD + "Skill Points" + UI.PST_KEYWORD;

					// Token: 0x0400B53F RID: 46399
					public static LocString REQUIRES_PREVIOUS_SKILLS = "    • Missing prerequisite " + UI.PRE_KEYWORD + "Skill" + UI.PST_KEYWORD;

					// Token: 0x0400B540 RID: 46400
					public static LocString PREVENTED_BY_TRAIT = string.Concat(new string[]
					{
						"    • This Duplicant possesses the ",
						UI.PRE_KEYWORD,
						"{0}",
						UI.PST_KEYWORD,
						" Trait and cannot learn this Skill"
					});

					// Token: 0x0400B541 RID: 46401
					public static LocString SKILL_APTITUDE = string.Concat(new string[]
					{
						"{0} is interested in {1} and will receive a ",
						UI.PRE_KEYWORD,
						"Morale",
						UI.PST_KEYWORD,
						" bonus for learning it!"
					});

					// Token: 0x0400B542 RID: 46402
					public static LocString SKILL_GRANTED = "{0} has been granted {1} by a Trait, but does not have increased " + UI.FormatAsKeyWord("Morale Requirements") + " from learning it";
				}
			}
		}

		// Token: 0x02002BF3 RID: 11251
		public class KLEI_INVENTORY_SCREEN
		{
			// Token: 0x0400B543 RID: 46403
			public static LocString OPEN_INVENTORY_BUTTON = "Open Klei Inventory";

			// Token: 0x0400B544 RID: 46404
			public static LocString ITEM_FACADE_FOR = "This blueprint works with any {ConfigProperName}.";

			// Token: 0x0400B545 RID: 46405
			public static LocString ARTABLE_ITEM_FACADE_FOR = "This blueprint works with any {ConfigProperName} of {ArtableQuality} quality.";

			// Token: 0x0400B546 RID: 46406
			public static LocString CLOTHING_ITEM_FACADE_FOR = "This blueprint can be used in any outfit.";

			// Token: 0x0400B547 RID: 46407
			public static LocString BALLOON_ARTIST_FACADE_FOR = "This blueprint can be used by any Balloon Artist.";

			// Token: 0x0400B548 RID: 46408
			public static LocString COLLECTION = "Part of {Collection} collection.";

			// Token: 0x0400B549 RID: 46409
			public static LocString COLLECTION_COMING_SOON = "Part of {Collection} collection. Coming soon!";

			// Token: 0x0400B54A RID: 46410
			public static LocString ITEM_RARITY_DETAILS = "{RarityName} quality.";

			// Token: 0x0400B54B RID: 46411
			public static LocString ITEM_PLAYER_OWNED_AMOUNT = "My colony has {OwnedCount} of these blueprints.";

			// Token: 0x0400B54C RID: 46412
			public static LocString ITEM_PLAYER_OWN_NONE = "My colony doesn't have any of these yet.";

			// Token: 0x0400B54D RID: 46413
			public static LocString ITEM_PLAYER_OWNED_AMOUNT_ICON = "x{OwnedCount}";

			// Token: 0x0400B54E RID: 46414
			public static LocString ITEM_PLAYER_UNLOCKED_BUT_UNOWNABLE = "This blueprint is part of my colony's permanent collection.";

			// Token: 0x0400B54F RID: 46415
			public static LocString ITEM_DLC_REQUIRED = "This blueprint is designed for the <i>Spaced Out!</i> DLC.";

			// Token: 0x0400B550 RID: 46416
			public static LocString ITEM_UNKNOWN_NAME = "Uh oh!";

			// Token: 0x0400B551 RID: 46417
			public static LocString ITEM_UNKNOWN_DESCRIPTION = "Hmm. Looks like this blueprint is missing from the supply closet. Perhaps due to a temporal anomaly...";

			// Token: 0x0400B552 RID: 46418
			public static LocString SEARCH_PLACEHOLDER = "Search";

			// Token: 0x0400B553 RID: 46419
			public static LocString CLEAR_SEARCH_BUTTON_TOOLTIP = "Clear search";

			// Token: 0x0400B554 RID: 46420
			public static LocString TOOLTIP_VIEW_ALL_ITEMS = "Filter: Showing all items\n\n" + UI.CLICK(UI.ClickType.Click) + " to toggle";

			// Token: 0x0400B555 RID: 46421
			public static LocString TOOLTIP_VIEW_OWNED_ONLY = "Filter: Showing owned items only\n\n" + UI.CLICK(UI.ClickType.Click) + " to toggle";

			// Token: 0x0400B556 RID: 46422
			public static LocString TOOLTIP_VIEW_DOUBLES_ONLY = "Filter: Showing multiples owned only\n\n" + UI.CLICK(UI.ClickType.Click) + " to toggle";

			// Token: 0x02002BF4 RID: 11252
			public static class BARTERING
			{
				// Token: 0x0400B557 RID: 46423
				public static LocString TOOLTIP_ACTION_INVALID_OFFLINE = "Currently unavailable";

				// Token: 0x0400B558 RID: 46424
				public static LocString BUY = "PRINT";

				// Token: 0x0400B559 RID: 46425
				public static LocString TOOLTIP_BUY_ACTIVE = "This item requires {0} spools of Filament to print";

				// Token: 0x0400B55A RID: 46426
				public static LocString TOOLTIP_UNBUYABLE = "This item is unprintable";

				// Token: 0x0400B55B RID: 46427
				public static LocString TOOLTIP_UNBUYABLE_BETA = "This item may be printable after the public testing period";

				// Token: 0x0400B55C RID: 46428
				public static LocString TOOLTIP_UNBUYABLE_ALREADY_OWNED = "My colony already owns one of these blueprints";

				// Token: 0x0400B55D RID: 46429
				public static LocString TOOLTIP_BUY_CANT_AFFORD = "Filament supply is too low";

				// Token: 0x0400B55E RID: 46430
				public static LocString SELL = "RECYCLE";

				// Token: 0x0400B55F RID: 46431
				public static LocString TOOLTIP_SELL_ACTIVE = "Recycle this blueprint for {0} spools of Filament";

				// Token: 0x0400B560 RID: 46432
				public static LocString TOOLTIP_UNSELLABLE = "This item is non-recyclable";

				// Token: 0x0400B561 RID: 46433
				public static LocString TOOLTIP_NONE_TO_SELL = "My colony does not own any of these blueprints";

				// Token: 0x0400B562 RID: 46434
				public static LocString CANCEL = "CANCEL";

				// Token: 0x0400B563 RID: 46435
				public static LocString CONFIRM_RECYCLE_HEADER = "RECYCLE INTO FILAMENT?";

				// Token: 0x0400B564 RID: 46436
				public static LocString CONFIRM_PRINT_HEADER = "PRINT ITEM?";

				// Token: 0x0400B565 RID: 46437
				public static LocString OFFLINE_LABEL = "Not connected to Klei server";

				// Token: 0x0400B566 RID: 46438
				public static LocString LOADING = "Connecting to server...";

				// Token: 0x0400B567 RID: 46439
				public static LocString TRANSACTION_ERROR = "Whoops! Something's gone wrong.";

				// Token: 0x0400B568 RID: 46440
				public static LocString ACTION_DESCRIPTION_RECYCLE = "Recycling this blueprint will recover Filament that my colony can use to print other items.\n\nOne copy of this blueprint will be removed from my colony's supply closet.";

				// Token: 0x0400B569 RID: 46441
				public static LocString ACTION_DESCRIPTION_PRINT = "Producing this blueprint requires Filament from my colony's supply.\n\nOne copy of this blueprint will be extruded at a time.";

				// Token: 0x0400B56A RID: 46442
				public static LocString WALLET_TOOLTIP = "{0} spool of Filament available";

				// Token: 0x0400B56B RID: 46443
				public static LocString WALLET_PLURAL_TOOLTIP = "{0} spools of Filament available";

				// Token: 0x0400B56C RID: 46444
				public static LocString TRANSACTION_COMPLETE_HEADER = "SUCCESS!";

				// Token: 0x0400B56D RID: 46445
				public static LocString TRANSACTION_INCOMPLETE_HEADER = "ERROR";

				// Token: 0x0400B56E RID: 46446
				public static LocString PURCHASE_SUCCESS = "One copy of this blueprint has been added to my colony's supply closet.";

				// Token: 0x0400B56F RID: 46447
				public static LocString SELL_SUCCESS = "The Filament recovered from recycling this item can now be used to print other items.";
			}

			// Token: 0x02002BF5 RID: 11253
			public static class CATEGORIES
			{
				// Token: 0x0400B570 RID: 46448
				public static LocString EQUIPMENT = "Equipment";

				// Token: 0x0400B571 RID: 46449
				public static LocString DUPE_TOPS = "Tops & Onesies";

				// Token: 0x0400B572 RID: 46450
				public static LocString DUPE_BOTTOMS = "Bottoms";

				// Token: 0x0400B573 RID: 46451
				public static LocString DUPE_GLOVES = "Gloves";

				// Token: 0x0400B574 RID: 46452
				public static LocString DUPE_SHOES = "Footwear";

				// Token: 0x0400B575 RID: 46453
				public static LocString DUPE_HATS = "Headgear";

				// Token: 0x0400B576 RID: 46454
				public static LocString DUPE_ACCESSORIES = "Accessories";

				// Token: 0x0400B577 RID: 46455
				public static LocString ATMO_SUIT_HELMET = "Atmo Helmets";

				// Token: 0x0400B578 RID: 46456
				public static LocString ATMO_SUIT_BODY = "Atmo Suits";

				// Token: 0x0400B579 RID: 46457
				public static LocString ATMO_SUIT_GLOVES = "Atmo Gloves";

				// Token: 0x0400B57A RID: 46458
				public static LocString ATMO_SUIT_BELT = "Atmo Belts";

				// Token: 0x0400B57B RID: 46459
				public static LocString ATMO_SUIT_SHOES = "Atmo Boots";

				// Token: 0x0400B57C RID: 46460
				public static LocString PRIMOGARB = "Primo Garb";

				// Token: 0x0400B57D RID: 46461
				public static LocString ATMOSUITS = "Atmo Suits";

				// Token: 0x0400B57E RID: 46462
				public static LocString BUILDINGS = "Buildings";

				// Token: 0x0400B57F RID: 46463
				public static LocString CRITTERS = "Critters";

				// Token: 0x0400B580 RID: 46464
				public static LocString SWEEPYS = "Sweepys";

				// Token: 0x0400B581 RID: 46465
				public static LocString DUPLICANTS = "Duplicants";

				// Token: 0x0400B582 RID: 46466
				public static LocString ARTWORKS = "Artwork";

				// Token: 0x0400B583 RID: 46467
				public static LocString MONUMENTPARTS = "Monuments";

				// Token: 0x0400B584 RID: 46468
				public static LocString JOY_RESPONSE = "Overjoyed Responses";

				// Token: 0x02002BF6 RID: 11254
				public static class JOY_RESPONSES
				{
					// Token: 0x0400B585 RID: 46469
					public static LocString BALLOON_ARTIST = "Balloon Artist";
				}
			}

			// Token: 0x02002BF7 RID: 11255
			public static class TOP_LEVEL_CATEGORIES
			{
				// Token: 0x0400B586 RID: 46470
				public static LocString UNRELEASED = "DEBUG UNRELEASED";

				// Token: 0x0400B587 RID: 46471
				public static LocString CLOTHING_TOPS = "Tops & Onesies";

				// Token: 0x0400B588 RID: 46472
				public static LocString CLOTHING_BOTTOMS = "Bottoms";

				// Token: 0x0400B589 RID: 46473
				public static LocString CLOTHING_GLOVES = "Gloves";

				// Token: 0x0400B58A RID: 46474
				public static LocString CLOTHING_SHOES = "Footwear";

				// Token: 0x0400B58B RID: 46475
				public static LocString ATMOSUITS = "Atmo Suits";

				// Token: 0x0400B58C RID: 46476
				public static LocString BUILDINGS = "Buildings";

				// Token: 0x0400B58D RID: 46477
				public static LocString WALLPAPERS = "Wallpapers";

				// Token: 0x0400B58E RID: 46478
				public static LocString ARTWORK = "Artwork";

				// Token: 0x0400B58F RID: 46479
				public static LocString JOY_RESPONSES = "Joy Responses";
			}

			// Token: 0x02002BF8 RID: 11256
			public static class SUBCATEGORIES
			{
				// Token: 0x0400B590 RID: 46480
				public static LocString UNRELEASED = "DEBUG UNRELEASED";

				// Token: 0x0400B591 RID: 46481
				public static LocString UNCATEGORIZED = "BUG: UNCATEGORIZED";

				// Token: 0x0400B592 RID: 46482
				public static LocString YAML = "YAML";

				// Token: 0x0400B593 RID: 46483
				public static LocString DEFAULT = "Default";

				// Token: 0x0400B594 RID: 46484
				public static LocString JOY_BALLOON = "Balloons";

				// Token: 0x0400B595 RID: 46485
				public static LocString JOY_STICKER = "Stickers";

				// Token: 0x0400B596 RID: 46486
				public static LocString PRIMO_GARB = "Primo Garb";

				// Token: 0x0400B597 RID: 46487
				public static LocString CLOTHING_TOPS_BASIC = "Standard Shirts";

				// Token: 0x0400B598 RID: 46488
				public static LocString CLOTHING_TOPS_TSHIRT = "Tees";

				// Token: 0x0400B599 RID: 46489
				public static LocString CLOTHING_TOPS_FANCY = "Specialty Tops";

				// Token: 0x0400B59A RID: 46490
				public static LocString CLOTHING_TOPS_JACKET = "Jackets";

				// Token: 0x0400B59B RID: 46491
				public static LocString CLOTHING_TOPS_UNDERSHIRT = "Undershirts";

				// Token: 0x0400B59C RID: 46492
				public static LocString CLOTHING_TOPS_DRESS = "Dresses and Onesies";

				// Token: 0x0400B59D RID: 46493
				public static LocString CLOTHING_BOTTOMS_BASIC = "Standard Pants";

				// Token: 0x0400B59E RID: 46494
				public static LocString CLOTHING_BOTTOMS_FANCY = "Fancy Pants";

				// Token: 0x0400B59F RID: 46495
				public static LocString CLOTHING_BOTTOMS_SHORTS = "Shorts";

				// Token: 0x0400B5A0 RID: 46496
				public static LocString CLOTHING_BOTTOMS_SKIRTS = "Skirts";

				// Token: 0x0400B5A1 RID: 46497
				public static LocString CLOTHING_BOTTOMS_UNDERWEAR = "Underwear";

				// Token: 0x0400B5A2 RID: 46498
				public static LocString CLOTHING_GLOVES_BASIC = "Standard Gloves";

				// Token: 0x0400B5A3 RID: 46499
				public static LocString CLOTHING_GLOVES_FORMAL = "Fancy Gloves";

				// Token: 0x0400B5A4 RID: 46500
				public static LocString CLOTHING_GLOVES_SHORT = "Short Gloves";

				// Token: 0x0400B5A5 RID: 46501
				public static LocString CLOTHING_GLOVES_PRINTS = "Specialty Gloves";

				// Token: 0x0400B5A6 RID: 46502
				public static LocString CLOTHING_SHOES_BASIC = "Standard Shoes";

				// Token: 0x0400B5A7 RID: 46503
				public static LocString CLOTHING_SHOE_SOCKS = "Socks";

				// Token: 0x0400B5A8 RID: 46504
				public static LocString CLOTHING_SHOES_FANCY = "Fancy Shoes";

				// Token: 0x0400B5A9 RID: 46505
				public static LocString ATMOSUIT_HELMETS_BASIC = "Atmo Helmets";

				// Token: 0x0400B5AA RID: 46506
				public static LocString ATMOSUIT_HELMETS_FANCY = "Fancy Atmo Helmets";

				// Token: 0x0400B5AB RID: 46507
				public static LocString ATMOSUIT_BODIES_BASIC = "Atmo Suits";

				// Token: 0x0400B5AC RID: 46508
				public static LocString ATMOSUIT_BODIES_FANCY = "Fancy Atmo Suits";

				// Token: 0x0400B5AD RID: 46509
				public static LocString ATMOSUIT_GLOVES_BASIC = "Atmo Gloves";

				// Token: 0x0400B5AE RID: 46510
				public static LocString ATMOSUIT_GLOVES_FANCY = "Fancy Atmo Gloves";

				// Token: 0x0400B5AF RID: 46511
				public static LocString ATMOSUIT_BELTS_BASIC = "Atmo Belts";

				// Token: 0x0400B5B0 RID: 46512
				public static LocString ATMOSUIT_BELTS_FANCY = "Fancy Atmo Belts";

				// Token: 0x0400B5B1 RID: 46513
				public static LocString ATMOSUIT_SHOES_BASIC = "Atmo Boots";

				// Token: 0x0400B5B2 RID: 46514
				public static LocString ATMOSUIT_SHOES_FANCY = "Fancy Atmo Boots";

				// Token: 0x0400B5B3 RID: 46515
				public static LocString BUILDING_WALLPAPER_BASIC = "Solid Wallpapers";

				// Token: 0x0400B5B4 RID: 46516
				public static LocString BUILDING_WALLPAPER_FANCY = "Geometric Wallpapers";

				// Token: 0x0400B5B5 RID: 46517
				public static LocString BUILDING_WALLPAPER_PRINTS = "Patterned Wallpapers";

				// Token: 0x0400B5B6 RID: 46518
				public static LocString BUILDING_CANVAS_STANDARD = "Standard Canvas";

				// Token: 0x0400B5B7 RID: 46519
				public static LocString BUILDING_CANVAS_PORTRAIT = "Portrait Canvas";

				// Token: 0x0400B5B8 RID: 46520
				public static LocString BUILDING_CANVAS_LANDSCAPE = "Landscape Canvas";

				// Token: 0x0400B5B9 RID: 46521
				public static LocString BUILDING_SCULPTURE = "Sculptures";

				// Token: 0x0400B5BA RID: 46522
				public static LocString MONUMENT_PARTS = "Monuments";

				// Token: 0x0400B5BB RID: 46523
				public static LocString BUILDINGS_FLOWER_VASE = "Pots and Planters";

				// Token: 0x0400B5BC RID: 46524
				public static LocString BUILDINGS_BED_COT = "Cots";

				// Token: 0x0400B5BD RID: 46525
				public static LocString BUILDINGS_BED_LUXURY = "Comfy Beds";

				// Token: 0x0400B5BE RID: 46526
				public static LocString BUILDING_CEILING_LIGHT = "Lights";

				// Token: 0x0400B5BF RID: 46527
				public static LocString BUILDINGS_STORAGE = "Storage";

				// Token: 0x0400B5C0 RID: 46528
				public static LocString BUILDINGS_INDUSTRIAL = "Industrial";

				// Token: 0x0400B5C1 RID: 46529
				public static LocString BUILDINGS_FOOD = "Cooking";

				// Token: 0x0400B5C2 RID: 46530
				public static LocString BUILDINGS_WASHROOM = "Sanitation";

				// Token: 0x0400B5C3 RID: 46531
				public static LocString BUILDINGS_RANCHING = "Agricultural";

				// Token: 0x0400B5C4 RID: 46532
				public static LocString BUILDINGS_RECREATION = "Recreation and Decor";

				// Token: 0x0400B5C5 RID: 46533
				public static LocString BUILDINGS_PRINTING_POD = "Printing Pods";
			}

			// Token: 0x02002BF9 RID: 11257
			public static class COLUMN_HEADERS
			{
				// Token: 0x0400B5C6 RID: 46534
				public static LocString CATEGORY_HEADER = "BLUEPRINTS";

				// Token: 0x0400B5C7 RID: 46535
				public static LocString ITEMS_HEADER = "Items";

				// Token: 0x0400B5C8 RID: 46536
				public static LocString DETAILS_HEADER = "Details";
			}
		}

		// Token: 0x02002BFA RID: 11258
		public class ITEM_DROP_SCREEN
		{
			// Token: 0x0400B5C9 RID: 46537
			public static LocString THANKS_FOR_PLAYING = "New blueprints unlocked!";

			// Token: 0x0400B5CA RID: 46538
			public static LocString WEB_REWARDS_AVAILABLE = "Rewards available online!";

			// Token: 0x0400B5CB RID: 46539
			public static LocString NOTHING_AVAILABLE = "All available blueprints claimed";

			// Token: 0x0400B5CC RID: 46540
			public static LocString OPEN_URL_BUTTON = "CLAIM";

			// Token: 0x0400B5CD RID: 46541
			public static LocString PRINT_ITEM_BUTTON = "PRINT";

			// Token: 0x0400B5CE RID: 46542
			public static LocString DISMISS_BUTTON = "DISMISS";

			// Token: 0x0400B5CF RID: 46543
			public static LocString ERROR_CANNOTLOADITEM = "Whoops! Something's gone wrong.";

			// Token: 0x0400B5D0 RID: 46544
			public static LocString UNOPENED_ITEM_COUNT = "{0} unclaimed blueprints";

			// Token: 0x0400B5D1 RID: 46545
			public static LocString UNOPENED_ITEM = "{0} unclaimed blueprint";

			// Token: 0x02002BFB RID: 11259
			public static class IN_GAME_BUTTON
			{
				// Token: 0x0400B5D2 RID: 46546
				public static LocString TOOLTIP_ITEMS_AVAILABLE = "Unlock new blueprints";

				// Token: 0x0400B5D3 RID: 46547
				public static LocString TOOLTIP_ERROR_NO_ITEMS = "No new blueprints to unlock";
			}
		}

		// Token: 0x02002BFC RID: 11260
		public class OUTFIT_BROWSER_SCREEN
		{
			// Token: 0x0400B5D4 RID: 46548
			public static LocString BUTTON_ADD_OUTFIT = "New Outfit";

			// Token: 0x0400B5D5 RID: 46549
			public static LocString BUTTON_PICK_OUTFIT = "Assign Outfit";

			// Token: 0x0400B5D6 RID: 46550
			public static LocString TOOLTIP_PICK_OUTFIT_ERROR_LOCKED = "Cannot assign this outfit to {MinionName} because my colony doesn't have all of these blueprints yet";

			// Token: 0x0400B5D7 RID: 46551
			public static LocString BUTTON_EDIT_OUTFIT = "Restyle Outfit";

			// Token: 0x0400B5D8 RID: 46552
			public static LocString BUTTON_COPY_OUTFIT = "Copy Outfit";

			// Token: 0x0400B5D9 RID: 46553
			public static LocString TOOLTIP_DELETE_OUTFIT = "Delete Outfit";

			// Token: 0x0400B5DA RID: 46554
			public static LocString TOOLTIP_DELETE_OUTFIT_ERROR_READONLY = "This outfit cannot be deleted";

			// Token: 0x0400B5DB RID: 46555
			public static LocString TOOLTIP_RENAME_OUTFIT = "Rename Outfit";

			// Token: 0x0400B5DC RID: 46556
			public static LocString TOOLTIP_RENAME_OUTFIT_ERROR_READONLY = "This outfit cannot be renamed";

			// Token: 0x0400B5DD RID: 46557
			public static LocString TOOLTIP_FILTER_BY_CLOTHING = "View your Clothing Outfits";

			// Token: 0x0400B5DE RID: 46558
			public static LocString TOOLTIP_FILTER_BY_ATMO_SUITS = "View your Atmo Suit Outfits";

			// Token: 0x02002BFD RID: 11261
			public static class COLUMN_HEADERS
			{
				// Token: 0x0400B5DF RID: 46559
				public static LocString GALLERY_HEADER = "OUTFITS";

				// Token: 0x0400B5E0 RID: 46560
				public static LocString MINION_GALLERY_HEADER = "WARDROBE";

				// Token: 0x0400B5E1 RID: 46561
				public static LocString DETAILS_HEADER = "Preview";
			}

			// Token: 0x02002BFE RID: 11262
			public class DELETE_WARNING_POPUP
			{
				// Token: 0x0400B5E2 RID: 46562
				public static LocString HEADER = "Delete \"{OutfitName}\"?";

				// Token: 0x0400B5E3 RID: 46563
				public static LocString BODY = "Are you sure you want to delete \"{OutfitName}\"?\n\nAny Duplicants assigned to wear this outfit on spawn will be printed wearing their default outfit instead. Existing Duplicants in saved games won't be affected.\n\nThis <b>cannot</b> be undone.";

				// Token: 0x0400B5E4 RID: 46564
				public static LocString BUTTON_YES_DELETE = "Yes, delete outfit";

				// Token: 0x0400B5E5 RID: 46565
				public static LocString BUTTON_DONT_DELETE = "Cancel";
			}

			// Token: 0x02002BFF RID: 11263
			public class RENAME_POPUP
			{
				// Token: 0x0400B5E6 RID: 46566
				public static LocString HEADER = "RENAME OUTFIT";
			}
		}

		// Token: 0x02002C00 RID: 11264
		public class LOCKER_MENU
		{
			// Token: 0x0400B5E7 RID: 46567
			public static LocString TITLE = "SUPPLY CLOSET";

			// Token: 0x0400B5E8 RID: 46568
			public static LocString BUTTON_INVENTORY = "All";

			// Token: 0x0400B5E9 RID: 46569
			public static LocString BUTTON_INVENTORY_DESCRIPTION = "View all of my colony's blueprints";

			// Token: 0x0400B5EA RID: 46570
			public static LocString BUTTON_DUPLICANTS = "Duplicants";

			// Token: 0x0400B5EB RID: 46571
			public static LocString BUTTON_DUPLICANTS_DESCRIPTION = "Manage individual Duplicants' outfits";

			// Token: 0x0400B5EC RID: 46572
			public static LocString BUTTON_OUTFITS = "Wardrobe";

			// Token: 0x0400B5ED RID: 46573
			public static LocString BUTTON_OUTFITS_DESCRIPTION = "Manage my colony's collection of outfits";

			// Token: 0x0400B5EE RID: 46574
			public static LocString DEFAULT_DESCRIPTION = "Select a screen";

			// Token: 0x0400B5EF RID: 46575
			public static LocString BUTTON_CLAIM = "Claim Blueprints";

			// Token: 0x0400B5F0 RID: 46576
			public static LocString BUTTON_CLAIM_DESCRIPTION = "Claim any available blueprints";

			// Token: 0x0400B5F1 RID: 46577
			public static LocString BUTTON_CLAIM_NONE_DESCRIPTION = "All available blueprints claimed";

			// Token: 0x0400B5F2 RID: 46578
			public static LocString UNOPENED_ITEMS_TOOLTIP = "New blueprints available";

			// Token: 0x0400B5F3 RID: 46579
			public static LocString UNOPENED_ITEMS_NONE_TOOLTIP = "All available blueprints claimed";

			// Token: 0x0400B5F4 RID: 46580
			public static LocString OFFLINE_ICON_TOOLTIP = "Not connected to Klei server";
		}

		// Token: 0x02002C01 RID: 11265
		public class LOCKER_NAVIGATOR
		{
			// Token: 0x0400B5F5 RID: 46581
			public static LocString BUTTON_BACK = "BACK";

			// Token: 0x0400B5F6 RID: 46582
			public static LocString BUTTON_CLOSE = "CLOSE";

			// Token: 0x02002C02 RID: 11266
			public class DATA_COLLECTION_WARNING_POPUP
			{
				// Token: 0x0400B5F7 RID: 46583
				public static LocString HEADER = "Data Communication is Disabled";

				// Token: 0x0400B5F8 RID: 46584
				public static LocString BODY = "Data Communication must be enabled in order to access newly unlocked items. This setting can be found in the Options menu.\n\nExisting item unlocks can still be used while Data Communication is disabled.";

				// Token: 0x0400B5F9 RID: 46585
				public static LocString BUTTON_OK = "Continue";

				// Token: 0x0400B5FA RID: 46586
				public static LocString BUTTON_OPEN_SETTINGS = "Options Menu";
			}
		}

		// Token: 0x02002C03 RID: 11267
		public class JOY_RESPONSE_DESIGNER_SCREEN
		{
			// Token: 0x0400B5FB RID: 46587
			public static LocString CATEGORY_HEADER = "OVERJOYED RESPONSES";

			// Token: 0x0400B5FC RID: 46588
			public static LocString BUTTON_APPLY_TO_MINION = "Assign to {MinionName}";

			// Token: 0x0400B5FD RID: 46589
			public static LocString TOOLTIP_NO_FACADES_FOR_JOY_TRAIT = "There aren't any blueprints for {JoyResponseType} Duplicants yet";

			// Token: 0x0400B5FE RID: 46590
			public static LocString TOOLTIP_PICK_JOY_RESPONSE_ERROR_LOCKED = "This Overjoyed Response blueprint cannot be assigned because my colony doesn't own it yet";

			// Token: 0x02002C04 RID: 11268
			public class CHANGES_NOT_SAVED_WARNING_POPUP
			{
				// Token: 0x0400B5FF RID: 46591
				public static LocString HEADER = "Discard changes to {MinionName}'s Overjoyed Response?";
			}
		}

		// Token: 0x02002C05 RID: 11269
		public class OUTFIT_DESIGNER_SCREEN
		{
			// Token: 0x0400B600 RID: 46592
			public static LocString CATEGORY_HEADER = "CLOTHING";

			// Token: 0x02002C06 RID: 11270
			public class MINION_INSTANCE
			{
				// Token: 0x0400B601 RID: 46593
				public static LocString BUTTON_APPLY_TO_MINION = "Assign to {MinionName}";

				// Token: 0x0400B602 RID: 46594
				public static LocString BUTTON_APPLY_TO_TEMPLATE = "Apply to Template";

				// Token: 0x02002C07 RID: 11271
				public class APPLY_TEMPLATE_POPUP
				{
					// Token: 0x0400B603 RID: 46595
					public static LocString HEADER = "SAVE AS TEMPLATE";

					// Token: 0x0400B604 RID: 46596
					public static LocString DESC_SAVE_EXISTING = "\"{OutfitName}\" will be updated and applied to {MinionName} on save.";

					// Token: 0x0400B605 RID: 46597
					public static LocString DESC_SAVE_NEW = "A new outfit named \"{OutfitName}\" will be created and assigned to {MinionName} on save.";

					// Token: 0x0400B606 RID: 46598
					public static LocString BUTTON_SAVE_EXISTING = "Update Outfit";

					// Token: 0x0400B607 RID: 46599
					public static LocString BUTTON_SAVE_NEW = "Save New Outfit";
				}
			}

			// Token: 0x02002C08 RID: 11272
			public class OUTFIT_TEMPLATE
			{
				// Token: 0x0400B608 RID: 46600
				public static LocString BUTTON_SAVE = "Save Template";

				// Token: 0x0400B609 RID: 46601
				public static LocString BUTTON_COPY = "Save a Copy";

				// Token: 0x0400B60A RID: 46602
				public static LocString TOOLTIP_SAVE_ERROR_LOCKED = "Cannot save this outfit because my colony doesn't own all of its blueprints yet";

				// Token: 0x0400B60B RID: 46603
				public static LocString TOOLTIP_SAVE_ERROR_READONLY = "This wardrobe staple cannot be altered\n\nMake a copy to save your changes";
			}

			// Token: 0x02002C09 RID: 11273
			public class CHANGES_NOT_SAVED_WARNING_POPUP
			{
				// Token: 0x0400B60C RID: 46604
				public static LocString HEADER = "Discard changes to \"{OutfitName}\"?";

				// Token: 0x0400B60D RID: 46605
				public static LocString BODY = "There are unsaved changes which will be lost if you exit now.\n\nAre you sure you want to discard your changes?";

				// Token: 0x0400B60E RID: 46606
				public static LocString BUTTON_DISCARD = "Yes, discard changes";

				// Token: 0x0400B60F RID: 46607
				public static LocString BUTTON_RETURN = "Cancel";
			}

			// Token: 0x02002C0A RID: 11274
			public class COPY_POPUP
			{
				// Token: 0x0400B610 RID: 46608
				public static LocString HEADER = "RENAME COPY";
			}
		}

		// Token: 0x02002C0B RID: 11275
		public class OUTFIT_NAME
		{
			// Token: 0x0400B611 RID: 46609
			public static LocString NEW = "Custom Outfit";

			// Token: 0x0400B612 RID: 46610
			public static LocString COPY_OF = "Copy of {OutfitName}";

			// Token: 0x0400B613 RID: 46611
			public static LocString RESOLVE_CONFLICT = "{OutfitName} ({ConflictNumber})";

			// Token: 0x0400B614 RID: 46612
			public static LocString ERROR_NAME_EXISTS = "There's already an outfit named \"{OutfitName}\"";

			// Token: 0x0400B615 RID: 46613
			public static LocString MINIONS_OUTFIT = "{MinionName}'s Current Outfit";

			// Token: 0x0400B616 RID: 46614
			public static LocString NONE = "Default Outfit";

			// Token: 0x0400B617 RID: 46615
			public static LocString NONE_JOY_RESPONSE = "Default Overjoyed Response";

			// Token: 0x0400B618 RID: 46616
			public static LocString NONE_ATMO_SUIT = "Default Atmo Suit";
		}

		// Token: 0x02002C0C RID: 11276
		public class OUTFIT_DESCRIPTION
		{
			// Token: 0x0400B619 RID: 46617
			public static LocString CONTAINS_NON_OWNED_ITEMS = "This outfit can only be worn once my colony has access to all of its blueprints.";

			// Token: 0x0400B61A RID: 46618
			public static LocString NO_JOY_RESPONSE_NAME = "Default Overjoyed Response";

			// Token: 0x0400B61B RID: 46619
			public static LocString NO_JOY_RESPONSE_DESC = "Default response to an overjoyed state.";
		}

		// Token: 0x02002C0D RID: 11277
		public class MINION_BROWSER_SCREEN
		{
			// Token: 0x0400B61C RID: 46620
			public static LocString CATEGORY_HEADER = "DUPLICANTS";

			// Token: 0x0400B61D RID: 46621
			public static LocString BUTTON_CHANGE_OUTFIT = "Open Wardrobe";

			// Token: 0x0400B61E RID: 46622
			public static LocString BUTTON_EDIT_OUTFIT_ITEMS = "Restyle Outfit";

			// Token: 0x0400B61F RID: 46623
			public static LocString BUTTON_EDIT_ATMO_SUIT_OUTFIT_ITEMS = "Restyle Atmo Suit";

			// Token: 0x0400B620 RID: 46624
			public static LocString BUTTON_EDIT_JOY_RESPONSE = "Restyle Overjoyed Response";

			// Token: 0x0400B621 RID: 46625
			public static LocString OUTFIT_TYPE_CLOTHING = "CLOTHING";

			// Token: 0x0400B622 RID: 46626
			public static LocString OUTFIT_TYPE_JOY_RESPONSE = "OVERJOYED RESPONSE";

			// Token: 0x0400B623 RID: 46627
			public static LocString OUTFIT_TYPE_ATMOSUIT = "ATMO SUIT";

			// Token: 0x0400B624 RID: 46628
			public static LocString TOOLTIP_FROM_DLC = "This Duplicant is part of {0} DLC";
		}

		// Token: 0x02002C0E RID: 11278
		public class PERMIT_RARITY
		{
			// Token: 0x0400B625 RID: 46629
			public static readonly LocString UNKNOWN = "Unknown";

			// Token: 0x0400B626 RID: 46630
			public static readonly LocString UNIVERSAL = "Universal";

			// Token: 0x0400B627 RID: 46631
			public static readonly LocString LOYALTY = "<color=#FFB037>Loyalty</color>";

			// Token: 0x0400B628 RID: 46632
			public static readonly LocString COMMON = "<color=#97B2B9>Common</color>";

			// Token: 0x0400B629 RID: 46633
			public static readonly LocString DECENT = "<color=#81EBDE>Decent</color>";

			// Token: 0x0400B62A RID: 46634
			public static readonly LocString NIFTY = "<color=#71E379>Nifty</color>";

			// Token: 0x0400B62B RID: 46635
			public static readonly LocString SPLENDID = "<color=#FF6DE7>Splendid</color>";
		}

		// Token: 0x02002C0F RID: 11279
		public class OUTFITS
		{
			// Token: 0x02002C10 RID: 11280
			public class BASIC_BLACK
			{
				// Token: 0x0400B62C RID: 46636
				public static LocString NAME = "Basic Black Outfit";
			}

			// Token: 0x02002C11 RID: 11281
			public class BASIC_WHITE
			{
				// Token: 0x0400B62D RID: 46637
				public static LocString NAME = "Basic White Outfit";
			}

			// Token: 0x02002C12 RID: 11282
			public class BASIC_RED
			{
				// Token: 0x0400B62E RID: 46638
				public static LocString NAME = "Basic Red Outfit";
			}

			// Token: 0x02002C13 RID: 11283
			public class BASIC_ORANGE
			{
				// Token: 0x0400B62F RID: 46639
				public static LocString NAME = "Basic Orange Outfit";
			}

			// Token: 0x02002C14 RID: 11284
			public class BASIC_YELLOW
			{
				// Token: 0x0400B630 RID: 46640
				public static LocString NAME = "Basic Yellow Outfit";
			}

			// Token: 0x02002C15 RID: 11285
			public class BASIC_GREEN
			{
				// Token: 0x0400B631 RID: 46641
				public static LocString NAME = "Basic Green Outfit";
			}

			// Token: 0x02002C16 RID: 11286
			public class BASIC_AQUA
			{
				// Token: 0x0400B632 RID: 46642
				public static LocString NAME = "Basic Aqua Outfit";
			}

			// Token: 0x02002C17 RID: 11287
			public class BASIC_PURPLE
			{
				// Token: 0x0400B633 RID: 46643
				public static LocString NAME = "Basic Purple Outfit";
			}

			// Token: 0x02002C18 RID: 11288
			public class BASIC_PINK_ORCHID
			{
				// Token: 0x0400B634 RID: 46644
				public static LocString NAME = "Basic Bubblegum Outfit";
			}

			// Token: 0x02002C19 RID: 11289
			public class BASIC_DEEPRED
			{
				// Token: 0x0400B635 RID: 46645
				public static LocString NAME = "Team Captain Outfit";
			}

			// Token: 0x02002C1A RID: 11290
			public class BASIC_BLUE_COBALT
			{
				// Token: 0x0400B636 RID: 46646
				public static LocString NAME = "True Blue Outfit";
			}

			// Token: 0x02002C1B RID: 11291
			public class BASIC_PINK_FLAMINGO
			{
				// Token: 0x0400B637 RID: 46647
				public static LocString NAME = "Pep Rally Outfit";
			}

			// Token: 0x02002C1C RID: 11292
			public class BASIC_GREEN_KELLY
			{
				// Token: 0x0400B638 RID: 46648
				public static LocString NAME = "Go Team! Outfit";
			}

			// Token: 0x02002C1D RID: 11293
			public class BASIC_GREY_CHARCOAL
			{
				// Token: 0x0400B639 RID: 46649
				public static LocString NAME = "Underdog Outfit";
			}

			// Token: 0x02002C1E RID: 11294
			public class BASIC_LEMON
			{
				// Token: 0x0400B63A RID: 46650
				public static LocString NAME = "Team Hype Outfit";
			}

			// Token: 0x02002C1F RID: 11295
			public class BASIC_SATSUMA
			{
				// Token: 0x0400B63B RID: 46651
				public static LocString NAME = "Superfan Outfit";
			}

			// Token: 0x02002C20 RID: 11296
			public class JELLYPUFF_BLUEBERRY
			{
				// Token: 0x0400B63C RID: 46652
				public static LocString NAME = "Blueberry Jelly Outfit";
			}

			// Token: 0x02002C21 RID: 11297
			public class JELLYPUFF_GRAPE
			{
				// Token: 0x0400B63D RID: 46653
				public static LocString NAME = "Grape Jelly Outfit";
			}

			// Token: 0x02002C22 RID: 11298
			public class JELLYPUFF_LEMON
			{
				// Token: 0x0400B63E RID: 46654
				public static LocString NAME = "Lemon Jelly Outfit";
			}

			// Token: 0x02002C23 RID: 11299
			public class JELLYPUFF_LIME
			{
				// Token: 0x0400B63F RID: 46655
				public static LocString NAME = "Lime Jelly Outfit";
			}

			// Token: 0x02002C24 RID: 11300
			public class JELLYPUFF_SATSUMA
			{
				// Token: 0x0400B640 RID: 46656
				public static LocString NAME = "Satsuma Jelly Outfit";
			}

			// Token: 0x02002C25 RID: 11301
			public class JELLYPUFF_STRAWBERRY
			{
				// Token: 0x0400B641 RID: 46657
				public static LocString NAME = "Strawberry Jelly Outfit";
			}

			// Token: 0x02002C26 RID: 11302
			public class JELLYPUFF_WATERMELON
			{
				// Token: 0x0400B642 RID: 46658
				public static LocString NAME = "Watermelon Jelly Outfit";
			}

			// Token: 0x02002C27 RID: 11303
			public class ATHLETE
			{
				// Token: 0x0400B643 RID: 46659
				public static LocString NAME = "Racing Outfit";
			}

			// Token: 0x02002C28 RID: 11304
			public class CIRCUIT
			{
				// Token: 0x0400B644 RID: 46660
				public static LocString NAME = "LED Party Outfit";
			}

			// Token: 0x02002C29 RID: 11305
			public class ATMOSUIT_LIMONE
			{
				// Token: 0x0400B645 RID: 46661
				public static LocString NAME = "Citrus Atmo Outfit";
			}

			// Token: 0x02002C2A RID: 11306
			public class ATMOSUIT_SPARKLE_RED
			{
				// Token: 0x0400B646 RID: 46662
				public static LocString NAME = "Red Glitter Atmo Outfit";
			}

			// Token: 0x02002C2B RID: 11307
			public class ATMOSUIT_SPARKLE_BLUE
			{
				// Token: 0x0400B647 RID: 46663
				public static LocString NAME = "Blue Glitter Atmo Outfit";
			}

			// Token: 0x02002C2C RID: 11308
			public class ATMOSUIT_SPARKLE_GREEN
			{
				// Token: 0x0400B648 RID: 46664
				public static LocString NAME = "Green Glitter Atmo Outfit";
			}

			// Token: 0x02002C2D RID: 11309
			public class ATMOSUIT_SPARKLE_LAVENDER
			{
				// Token: 0x0400B649 RID: 46665
				public static LocString NAME = "Violet Glitter Atmo Outfit";
			}

			// Token: 0x02002C2E RID: 11310
			public class ATMOSUIT_PUFT
			{
				// Token: 0x0400B64A RID: 46666
				public static LocString NAME = "Puft Atmo Outfit";
			}

			// Token: 0x02002C2F RID: 11311
			public class ATMOSUIT_CONFETTI
			{
				// Token: 0x0400B64B RID: 46667
				public static LocString NAME = "Confetti Atmo Outfit";
			}

			// Token: 0x02002C30 RID: 11312
			public class ATMOSUIT_BASIC_PURPLE
			{
				// Token: 0x0400B64C RID: 46668
				public static LocString NAME = "Eggplant Atmo Outfit";
			}

			// Token: 0x02002C31 RID: 11313
			public class ATMOSUIT_PINK_PURPLE
			{
				// Token: 0x0400B64D RID: 46669
				public static LocString NAME = "Pink Punch Atmo Outfit";
			}

			// Token: 0x02002C32 RID: 11314
			public class ATMOSUIT_RED_GREY
			{
				// Token: 0x0400B64E RID: 46670
				public static LocString NAME = "Blastoff Atmo Outfit";
			}

			// Token: 0x02002C33 RID: 11315
			public class CANUXTUX
			{
				// Token: 0x0400B64F RID: 46671
				public static LocString NAME = "Canadian Tuxedo Outfit";
			}

			// Token: 0x02002C34 RID: 11316
			public class GONCHIES_STRAWBERRY
			{
				// Token: 0x0400B650 RID: 46672
				public static LocString NAME = "Executive Undies Outfit";
			}

			// Token: 0x02002C35 RID: 11317
			public class GONCHIES_SATSUMA
			{
				// Token: 0x0400B651 RID: 46673
				public static LocString NAME = "Underling Undies Outfit";
			}

			// Token: 0x02002C36 RID: 11318
			public class GONCHIES_LEMON
			{
				// Token: 0x0400B652 RID: 46674
				public static LocString NAME = "Groupthink Undies Outfit";
			}

			// Token: 0x02002C37 RID: 11319
			public class GONCHIES_LIME
			{
				// Token: 0x0400B653 RID: 46675
				public static LocString NAME = "Stakeholder Undies Outfit";
			}

			// Token: 0x02002C38 RID: 11320
			public class GONCHIES_BLUEBERRY
			{
				// Token: 0x0400B654 RID: 46676
				public static LocString NAME = "Admin Undies Outfit";
			}

			// Token: 0x02002C39 RID: 11321
			public class GONCHIES_GRAPE
			{
				// Token: 0x0400B655 RID: 46677
				public static LocString NAME = "Buzzword Undies Outfit";
			}

			// Token: 0x02002C3A RID: 11322
			public class GONCHIES_WATERMELON
			{
				// Token: 0x0400B656 RID: 46678
				public static LocString NAME = "Synergy Undies Outfit";
			}

			// Token: 0x02002C3B RID: 11323
			public class NERD
			{
				// Token: 0x0400B657 RID: 46679
				public static LocString NAME = "Research Outfit";
			}

			// Token: 0x02002C3C RID: 11324
			public class REBELGI
			{
				// Token: 0x0400B658 RID: 46680
				public static LocString NAME = "Rebel Gi Outfit";
			}

			// Token: 0x02002C3D RID: 11325
			public class DONOR
			{
				// Token: 0x0400B659 RID: 46681
				public static LocString NAME = "Donor Outfit";
			}

			// Token: 0x02002C3E RID: 11326
			public class MECHANIC
			{
				// Token: 0x0400B65A RID: 46682
				public static LocString NAME = "Engineer Coveralls";
			}

			// Token: 0x02002C3F RID: 11327
			public class VELOUR_BLACK
			{
				// Token: 0x0400B65B RID: 46683
				public static LocString NAME = "PhD Velour Outfit";
			}

			// Token: 0x02002C40 RID: 11328
			public class SLEEVELESS_BOW_BW
			{
				// Token: 0x0400B65C RID: 46684
				public static LocString NAME = "PhD Dress Outfit";
			}

			// Token: 0x02002C41 RID: 11329
			public class VELOUR_BLUE
			{
				// Token: 0x0400B65D RID: 46685
				public static LocString NAME = "Shortwave Velour Outfit";
			}

			// Token: 0x02002C42 RID: 11330
			public class VELOUR_PINK
			{
				// Token: 0x0400B65E RID: 46686
				public static LocString NAME = "Gamma Velour Outfit";
			}

			// Token: 0x02002C43 RID: 11331
			public class WATER
			{
				// Token: 0x0400B65F RID: 46687
				public static LocString NAME = "HVAC Coveralls";
			}

			// Token: 0x02002C44 RID: 11332
			public class WAISTCOAT_PINSTRIPE_SLATE
			{
				// Token: 0x0400B660 RID: 46688
				public static LocString NAME = "Nobel Pinstripe Outfit";
			}

			// Token: 0x02002C45 RID: 11333
			public class TWEED_PINK_ORCHID
			{
				// Token: 0x0400B661 RID: 46689
				public static LocString NAME = "Power Brunch Outfit";
			}

			// Token: 0x02002C46 RID: 11334
			public class BALLET
			{
				// Token: 0x0400B662 RID: 46690
				public static LocString NAME = "Ballet Outfit";
			}

			// Token: 0x02002C47 RID: 11335
			public class ATMOSUIT_CANTALOUPE
			{
				// Token: 0x0400B663 RID: 46691
				public static LocString NAME = "Rocketmelon Atmo Outfit";
			}

			// Token: 0x02002C48 RID: 11336
			public class PAJAMAS_SNOW
			{
				// Token: 0x0400B664 RID: 46692
				public static LocString NAME = "Crystal-Iced Jammies";
			}

			// Token: 0x02002C49 RID: 11337
			public class X_SPORCHID
			{
				// Token: 0x0400B665 RID: 46693
				public static LocString NAME = "Sporefest Outfit";
			}

			// Token: 0x02002C4A RID: 11338
			public class X1_PINCHAPEPPERNUTBELLS
			{
				// Token: 0x0400B666 RID: 46694
				public static LocString NAME = "Pinchabell Outfit";
			}

			// Token: 0x02002C4B RID: 11339
			public class POMPOM_SHINEBUGS_PINK_PEPPERNUT
			{
				// Token: 0x0400B667 RID: 46695
				public static LocString NAME = "Pom Bug Outfit";
			}

			// Token: 0x02002C4C RID: 11340
			public class SNOWFLAKE_BLUE
			{
				// Token: 0x0400B668 RID: 46696
				public static LocString NAME = "Crystal-Iced Outfit";
			}

			// Token: 0x02002C4D RID: 11341
			public class POLKADOT_TRACKSUIT
			{
				// Token: 0x0400B669 RID: 46697
				public static LocString NAME = "Polka Dot Tracksuit";
			}

			// Token: 0x02002C4E RID: 11342
			public class SUPERSTAR
			{
				// Token: 0x0400B66A RID: 46698
				public static LocString NAME = "Superstar Outfit";
			}

			// Token: 0x02002C4F RID: 11343
			public class ATMOSUIT_SPIFFY
			{
				// Token: 0x0400B66B RID: 46699
				public static LocString NAME = "Spiffy Atmo Outfit";
			}

			// Token: 0x02002C50 RID: 11344
			public class ATMOSUIT_CUBIST
			{
				// Token: 0x0400B66C RID: 46700
				public static LocString NAME = "Cubist Atmo Outfit";
			}

			// Token: 0x02002C51 RID: 11345
			public class LUCKY
			{
				// Token: 0x0400B66D RID: 46701
				public static LocString NAME = "Lucky Jammies Outfit";
			}

			// Token: 0x02002C52 RID: 11346
			public class SWEETHEART
			{
				// Token: 0x0400B66E RID: 46702
				public static LocString NAME = "Sweetheart Jammies Outfit";
			}

			// Token: 0x02002C53 RID: 11347
			public class GINCH_GLUON
			{
				// Token: 0x0400B66F RID: 46703
				public static LocString NAME = "Frilly Saltrock Outfit";
			}

			// Token: 0x02002C54 RID: 11348
			public class GINCH_CORTEX
			{
				// Token: 0x0400B670 RID: 46704
				public static LocString NAME = "Dusk Undies Outfit";
			}

			// Token: 0x02002C55 RID: 11349
			public class GINCH_FROSTY
			{
				// Token: 0x0400B671 RID: 46705
				public static LocString NAME = "Frostbasin Undies Outfit";
			}

			// Token: 0x02002C56 RID: 11350
			public class GINCH_LOCUS
			{
				// Token: 0x0400B672 RID: 46706
				public static LocString NAME = "Balmy Undies Outfit";
			}

			// Token: 0x02002C57 RID: 11351
			public class GINCH_GOOP
			{
				// Token: 0x0400B673 RID: 46707
				public static LocString NAME = "Leachy Undies Outfit";
			}

			// Token: 0x02002C58 RID: 11352
			public class GINCH_BILE
			{
				// Token: 0x0400B674 RID: 46708
				public static LocString NAME = "Yellowcake Undies Outfit";
			}

			// Token: 0x02002C59 RID: 11353
			public class GINCH_NYBBLE
			{
				// Token: 0x0400B675 RID: 46709
				public static LocString NAME = "Atomic Undies Outfit";
			}

			// Token: 0x02002C5A RID: 11354
			public class GINCH_IRONBOW
			{
				// Token: 0x0400B676 RID: 46710
				public static LocString NAME = "Magma Undies Outfit";
			}

			// Token: 0x02002C5B RID: 11355
			public class GINCH_PHLEGM
			{
				// Token: 0x0400B677 RID: 46711
				public static LocString NAME = "Slate Undies Outfit";
			}

			// Token: 0x02002C5C RID: 11356
			public class GINCH_OBELUS
			{
				// Token: 0x0400B678 RID: 46712
				public static LocString NAME = "Charcoal Undies Outfit";
			}

			// Token: 0x02002C5D RID: 11357
			public class HIVIS
			{
				// Token: 0x0400B679 RID: 46713
				public static LocString NAME = "Hi-Vis Outfit";
			}

			// Token: 0x02002C5E RID: 11358
			public class DOWNTIME
			{
				// Token: 0x0400B67A RID: 46714
				public static LocString NAME = "Downtime Outfit";
			}

			// Token: 0x02002C5F RID: 11359
			public class FLANNEL_RED
			{
				// Token: 0x0400B67B RID: 46715
				public static LocString NAME = "Classic Flannel Outfit";
			}

			// Token: 0x02002C60 RID: 11360
			public class FLANNEL_ORANGE
			{
				// Token: 0x0400B67C RID: 46716
				public static LocString NAME = "Cadmium Flannel Outfit";
			}

			// Token: 0x02002C61 RID: 11361
			public class FLANNEL_YELLOW
			{
				// Token: 0x0400B67D RID: 46717
				public static LocString NAME = "Flax Flannel Outfit";
			}

			// Token: 0x02002C62 RID: 11362
			public class FLANNEL_GREEN
			{
				// Token: 0x0400B67E RID: 46718
				public static LocString NAME = "Swampy Flannel Outfit";
			}

			// Token: 0x02002C63 RID: 11363
			public class FLANNEL_BLUE_MIDDLE
			{
				// Token: 0x0400B67F RID: 46719
				public static LocString NAME = "Scrub Flannel Outfit";
			}

			// Token: 0x02002C64 RID: 11364
			public class FLANNEL_PURPLE
			{
				// Token: 0x0400B680 RID: 46720
				public static LocString NAME = "Fusion Flannel Outfit";
			}

			// Token: 0x02002C65 RID: 11365
			public class FLANNEL_PINK_ORCHID
			{
				// Token: 0x0400B681 RID: 46721
				public static LocString NAME = "Flare Flannel Outfit";
			}

			// Token: 0x02002C66 RID: 11366
			public class FLANNEL_WHITE
			{
				// Token: 0x0400B682 RID: 46722
				public static LocString NAME = "White Flannel Outfit";
			}

			// Token: 0x02002C67 RID: 11367
			public class FLANNEL_BLACK
			{
				// Token: 0x0400B683 RID: 46723
				public static LocString NAME = "Monochrome Flannel Outfit";
			}
		}

		// Token: 0x02002C68 RID: 11368
		public class ROLES_SCREEN
		{
			// Token: 0x0400B684 RID: 46724
			public static LocString MANAGEMENT_BUTTON = "JOBS";

			// Token: 0x0400B685 RID: 46725
			public static LocString ROLE_PROGRESS = "<b>Job Experience: {0}/{1}</b>\nDuplicants can become eligible for specialized jobs by maxing their current job experience";

			// Token: 0x0400B686 RID: 46726
			public static LocString NO_JOB_STATION_WARNING = string.Concat(new string[]
			{
				"Build a ",
				UI.PRE_KEYWORD,
				"Printing Pod",
				UI.PST_KEYWORD,
				" to unlock this menu\n\nThe ",
				UI.PRE_KEYWORD,
				"Printing Pod",
				UI.PST_KEYWORD,
				" can be found in the ",
				UI.FormatAsBuildMenuTab("Base Tab", global::Action.Plan1),
				" of the Build Menu"
			});

			// Token: 0x0400B687 RID: 46727
			public static LocString AUTO_PRIORITIZE = "Auto-Prioritize:";

			// Token: 0x0400B688 RID: 46728
			public static LocString AUTO_PRIORITIZE_ENABLED = "Duplicant priorities are automatically reconfigured when they are assigned a new job";

			// Token: 0x0400B689 RID: 46729
			public static LocString AUTO_PRIORITIZE_DISABLED = "Duplicant priorities can only be changed manually";

			// Token: 0x0400B68A RID: 46730
			public static LocString EXPECTATION_ALERT_EXPECTATION = "Current Morale: {0}\nJob Morale Needs: {1}";

			// Token: 0x0400B68B RID: 46731
			public static LocString EXPECTATION_ALERT_JOB = "Current Morale: {0}\n{2} Minimum Morale: {1}";

			// Token: 0x0400B68C RID: 46732
			public static LocString EXPECTATION_ALERT_TARGET_JOB = "{2}'s Current: {0} Morale\n{3} Minimum Morale: {1}";

			// Token: 0x0400B68D RID: 46733
			public static LocString EXPECTATION_ALERT_DESC_EXPECTATION = "This Duplicant's Morale is too low to handle the rigors of this position, which will cause them Stress over time.";

			// Token: 0x0400B68E RID: 46734
			public static LocString EXPECTATION_ALERT_DESC_JOB = "This Duplicant's Morale is too low to handle the assigned job, which will cause them Stress over time.";

			// Token: 0x0400B68F RID: 46735
			public static LocString EXPECTATION_ALERT_DESC_TARGET_JOB = "This Duplicant's Morale is too low to handle the rigors of this position, which will cause them Stress over time.";

			// Token: 0x0400B690 RID: 46736
			public static LocString HIGHEST_EXPECTATIONS_TIER = "<b>Highest Expectations</b>";

			// Token: 0x0400B691 RID: 46737
			public static LocString ADDED_EXPECTATIONS_AMOUNT = " (+{0} Expectation)";

			// Token: 0x02002C69 RID: 11369
			public class WIDGET
			{
				// Token: 0x0400B692 RID: 46738
				public static LocString NUMBER_OF_MASTERS_TOOLTIP = "<b>Duplicants who have mastered this job:</b>{0}";

				// Token: 0x0400B693 RID: 46739
				public static LocString NO_MASTERS_TOOLTIP = "<b>No Duplicants have mastered this job</b>";
			}

			// Token: 0x02002C6A RID: 11370
			public class TIER_NAMES
			{
				// Token: 0x0400B694 RID: 46740
				public static LocString ZERO = "Tier 0";

				// Token: 0x0400B695 RID: 46741
				public static LocString ONE = "Tier 1";

				// Token: 0x0400B696 RID: 46742
				public static LocString TWO = "Tier 2";

				// Token: 0x0400B697 RID: 46743
				public static LocString THREE = "Tier 3";

				// Token: 0x0400B698 RID: 46744
				public static LocString FOUR = "Tier 4";

				// Token: 0x0400B699 RID: 46745
				public static LocString FIVE = "Tier 5";

				// Token: 0x0400B69A RID: 46746
				public static LocString SIX = "Tier 6";

				// Token: 0x0400B69B RID: 46747
				public static LocString SEVEN = "Tier 7";

				// Token: 0x0400B69C RID: 46748
				public static LocString EIGHT = "Tier 8";

				// Token: 0x0400B69D RID: 46749
				public static LocString NINE = "Tier 9";
			}

			// Token: 0x02002C6B RID: 11371
			public class SLOTS
			{
				// Token: 0x0400B69E RID: 46750
				public static LocString UNASSIGNED = "Vacant Position";

				// Token: 0x0400B69F RID: 46751
				public static LocString UNASSIGNED_TOOLTIP = UI.CLICK(UI.ClickType.Click) + " to assign a Duplicant to this job opening";

				// Token: 0x0400B6A0 RID: 46752
				public static LocString NOSLOTS = "No slots available";

				// Token: 0x0400B6A1 RID: 46753
				public static LocString NO_ELIGIBLE_DUPLICANTS = "No Duplicants meet the requirements for this job";

				// Token: 0x0400B6A2 RID: 46754
				public static LocString ASSIGNMENT_PENDING = "(Pending)";

				// Token: 0x0400B6A3 RID: 46755
				public static LocString PICK_JOB = "No Job";

				// Token: 0x0400B6A4 RID: 46756
				public static LocString PICK_DUPLICANT = "None";
			}

			// Token: 0x02002C6C RID: 11372
			public class DROPDOWN
			{
				// Token: 0x0400B6A5 RID: 46757
				public static LocString NAME_AND_ROLE = "{0} <color=#F44A47FF>({1})</color>";

				// Token: 0x0400B6A6 RID: 46758
				public static LocString ALREADY_ROLE = "(Currently {0})";
			}

			// Token: 0x02002C6D RID: 11373
			public class SIDEBAR
			{
				// Token: 0x0400B6A7 RID: 46759
				public static LocString ASSIGNED_DUPLICANTS = "Assigned Duplicants";

				// Token: 0x0400B6A8 RID: 46760
				public static LocString UNASSIGNED_DUPLICANTS = "Unassigned Duplicants";

				// Token: 0x0400B6A9 RID: 46761
				public static LocString UNASSIGN = "Unassign job";
			}

			// Token: 0x02002C6E RID: 11374
			public class PRIORITY
			{
				// Token: 0x0400B6AA RID: 46762
				public static LocString TITLE = "Job Priorities";

				// Token: 0x0400B6AB RID: 46763
				public static LocString DESCRIPTION = "{0}s prioritize these work errands: ";

				// Token: 0x0400B6AC RID: 46764
				public static LocString NO_EFFECT = "This job does not affect errand prioritization";
			}

			// Token: 0x02002C6F RID: 11375
			public class RESUME
			{
				// Token: 0x0400B6AD RID: 46765
				public static LocString TITLE = "Qualifications";

				// Token: 0x0400B6AE RID: 46766
				public static LocString PREVIOUS_ROLES = "PREVIOUS DUTIES";

				// Token: 0x0400B6AF RID: 46767
				public static LocString UNASSIGNED = "Unassigned";

				// Token: 0x0400B6B0 RID: 46768
				public static LocString NO_SELECTION = "No Duplicant selected";
			}

			// Token: 0x02002C70 RID: 11376
			public class PERKS
			{
				// Token: 0x0400B6B1 RID: 46769
				public static LocString TITLE_BASICTRAINING = "Basic Job Training";

				// Token: 0x0400B6B2 RID: 46770
				public static LocString TITLE_MORETRAINING = "Additional Job Training";

				// Token: 0x0400B6B3 RID: 46771
				public static LocString NO_PERKS = "This job comes with no training";

				// Token: 0x0400B6B4 RID: 46772
				public static LocString ATTRIBUTE_EFFECT_FMT = "<b>{0}</b> " + UI.PRE_KEYWORD + "{1}" + UI.PST_KEYWORD;

				// Token: 0x02002C71 RID: 11377
				public class CAN_DIG_VERY_FIRM
				{
					// Token: 0x0400B6B5 RID: 46773
					public static LocString DESCRIPTION = UI.FormatAsLink(ELEMENTS.HARDNESS.HARDNESS_DESCRIPTOR.VERYFIRM + " Material", "HARDNESS") + " Mining";
				}

				// Token: 0x02002C72 RID: 11378
				public class CAN_DIG_NEARLY_IMPENETRABLE
				{
					// Token: 0x0400B6B6 RID: 46774
					public static LocString DESCRIPTION = UI.FormatAsLink("Abyssalite", "KATAIRITE") + " Mining";
				}

				// Token: 0x02002C73 RID: 11379
				public class CAN_DIG_SUPER_SUPER_HARD
				{
					// Token: 0x0400B6B7 RID: 46775
					public static LocString DESCRIPTION = UI.FormatAsLink("Diamond", "DIAMOND") + " and " + UI.FormatAsLink("Obsidian", "OBSIDIAN") + " Mining";
				}

				// Token: 0x02002C74 RID: 11380
				public class CAN_DIG_RADIOACTIVE_MATERIALS
				{
					// Token: 0x0400B6B8 RID: 46776
					public static LocString DESCRIPTION = UI.FormatAsLink("Corium", "CORIUM") + " Mining";
				}

				// Token: 0x02002C75 RID: 11381
				public class CAN_DIG_UNOBTANIUM
				{
					// Token: 0x0400B6B9 RID: 46777
					public static LocString DESCRIPTION = UI.FormatAsLink("Neutronium", "UNOBTANIUM") + " Mining";
				}

				// Token: 0x02002C76 RID: 11382
				public class CAN_ART
				{
					// Token: 0x0400B6BA RID: 46778
					public static LocString DESCRIPTION = "Can produce artwork using " + BUILDINGS.PREFABS.CANVAS.NAME + " and " + BUILDINGS.PREFABS.SCULPTURE.NAME;
				}

				// Token: 0x02002C77 RID: 11383
				public class CAN_ART_UGLY
				{
					// Token: 0x0400B6BB RID: 46779
					public static LocString DESCRIPTION = UI.PRE_KEYWORD + "Crude" + UI.PST_KEYWORD + " artwork quality";
				}

				// Token: 0x02002C78 RID: 11384
				public class CAN_ART_OKAY
				{
					// Token: 0x0400B6BC RID: 46780
					public static LocString DESCRIPTION = UI.PRE_KEYWORD + "Mediocre" + UI.PST_KEYWORD + " artwork quality";
				}

				// Token: 0x02002C79 RID: 11385
				public class CAN_ART_GREAT
				{
					// Token: 0x0400B6BD RID: 46781
					public static LocString DESCRIPTION = UI.PRE_KEYWORD + "Master" + UI.PST_KEYWORD + " artwork quality";
				}

				// Token: 0x02002C7A RID: 11386
				public class CAN_FARM_TINKER
				{
					// Token: 0x0400B6BE RID: 46782
					public static LocString DESCRIPTION = UI.FormatAsLink("Crop Tending", "PLANTS") + " and " + ITEMS.INDUSTRIAL_PRODUCTS.FARM_STATION_TOOLS.NAME + " Crafting";
				}

				// Token: 0x02002C7B RID: 11387
				public class CAN_IDENTIFY_MUTANT_SEEDS
				{
					// Token: 0x0400B6BF RID: 46783
					public static LocString DESCRIPTION = string.Concat(new string[]
					{
						"Can identify ",
						UI.PRE_KEYWORD,
						"Mutant Seeds",
						UI.PST_KEYWORD,
						" at the ",
						BUILDINGS.PREFABS.GENETICANALYSISSTATION.NAME
					});
				}

				// Token: 0x02002C7C RID: 11388
				public class CAN_WRANGLE_CREATURES
				{
					// Token: 0x0400B6C0 RID: 46784
					public static LocString DESCRIPTION = "Critter Wrangling";
				}

				// Token: 0x02002C7D RID: 11389
				public class CAN_USE_RANCH_STATION
				{
					// Token: 0x0400B6C1 RID: 46785
					public static LocString DESCRIPTION = BUILDINGS.PREFABS.RANCHSTATION.NAME + " Usage";
				}

				// Token: 0x02002C7E RID: 11390
				public class CAN_USE_MILKING_STATION
				{
					// Token: 0x0400B6C2 RID: 46786
					public static LocString DESCRIPTION = BUILDINGS.PREFABS.MILKINGSTATION.NAME + " Usage";
				}

				// Token: 0x02002C7F RID: 11391
				public class CAN_POWER_TINKER
				{
					// Token: 0x0400B6C3 RID: 46787
					public static LocString DESCRIPTION = UI.FormatAsLink("Generator Tuning", "POWER") + " usage and " + ITEMS.INDUSTRIAL_PRODUCTS.POWER_STATION_TOOLS.NAME + " Crafting";
				}

				// Token: 0x02002C80 RID: 11392
				public class CAN_ELECTRIC_GRILL
				{
					// Token: 0x0400B6C4 RID: 46788
					public static LocString DESCRIPTION = BUILDINGS.PREFABS.COOKINGSTATION.NAME + " Usage";
				}

				// Token: 0x02002C81 RID: 11393
				public class CAN_SPICE_GRINDER
				{
					// Token: 0x0400B6C5 RID: 46789
					public static LocString DESCRIPTION = BUILDINGS.PREFABS.SPICEGRINDER.NAME + " Usage";
				}

				// Token: 0x02002C82 RID: 11394
				public class CAN_MAKE_MISSILES
				{
					// Token: 0x0400B6C6 RID: 46790
					public static LocString DESCRIPTION = BUILDINGS.PREFABS.MISSILEFABRICATOR.NAME + " Usage";
				}

				// Token: 0x02002C83 RID: 11395
				public class CAN_CRAFT_ELECTRONICS
				{
					// Token: 0x0400B6C7 RID: 46791
					public static LocString DESCRIPTION = BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.NAME + " Usage";
				}

				// Token: 0x02002C84 RID: 11396
				public class ADVANCED_RESEARCH
				{
					// Token: 0x0400B6C8 RID: 46792
					public static LocString DESCRIPTION = BUILDINGS.PREFABS.ADVANCEDRESEARCHCENTER.NAME + " Usage";
				}

				// Token: 0x02002C85 RID: 11397
				public class INTERSTELLAR_RESEARCH
				{
					// Token: 0x0400B6C9 RID: 46793
					public static LocString DESCRIPTION = BUILDINGS.PREFABS.COSMICRESEARCHCENTER.NAME + " Usage";
				}

				// Token: 0x02002C86 RID: 11398
				public class NUCLEAR_RESEARCH
				{
					// Token: 0x0400B6CA RID: 46794
					public static LocString DESCRIPTION = BUILDINGS.PREFABS.NUCLEARRESEARCHCENTER.NAME + " Usage";
				}

				// Token: 0x02002C87 RID: 11399
				public class ORBITAL_RESEARCH
				{
					// Token: 0x0400B6CB RID: 46795
					public static LocString DESCRIPTION = BUILDINGS.PREFABS.DLC1COSMICRESEARCHCENTER.NAME + " Usage";
				}

				// Token: 0x02002C88 RID: 11400
				public class GEYSER_TUNING
				{
					// Token: 0x0400B6CC RID: 46796
					public static LocString DESCRIPTION = BUILDINGS.PREFABS.GEOTUNER.NAME + " Usage";
				}

				// Token: 0x02002C89 RID: 11401
				public class CAN_CLOTHING_ALTERATION
				{
					// Token: 0x0400B6CD RID: 46797
					public static LocString DESCRIPTION = BUILDINGS.PREFABS.CLOTHINGALTERATIONSTATION.NAME + " Usage";
				}

				// Token: 0x02002C8A RID: 11402
				public class CAN_STUDY_WORLD_OBJECTS
				{
					// Token: 0x0400B6CE RID: 46798
					public static LocString DESCRIPTION = "Geographical Analysis";
				}

				// Token: 0x02002C8B RID: 11403
				public class CAN_STUDY_ARTIFACTS
				{
					// Token: 0x0400B6CF RID: 46799
					public static LocString DESCRIPTION = "Artifact Analysis";
				}

				// Token: 0x02002C8C RID: 11404
				public class CAN_USE_CLUSTER_TELESCOPE
				{
					// Token: 0x0400B6D0 RID: 46800
					public static LocString DESCRIPTION = BUILDINGS.PREFABS.CLUSTERTELESCOPE.NAME + " Usage";
				}

				// Token: 0x02002C8D RID: 11405
				public class EXOSUIT_EXPERTISE
				{
					// Token: 0x0400B6D1 RID: 46801
					public static LocString DESCRIPTION = UI.FormatAsLink("Exosuit", "EXOSUIT") + " Penalty Reduction";
				}

				// Token: 0x02002C8E RID: 11406
				public class EXOSUIT_DURABILITY
				{
					// Token: 0x0400B6D2 RID: 46802
					public static LocString DESCRIPTION = "Slows " + UI.FormatAsLink("Exosuit", "EXOSUIT") + " Durability Damage";
				}

				// Token: 0x02002C8F RID: 11407
				public class CONVEYOR_BUILD
				{
					// Token: 0x0400B6D3 RID: 46803
					public static LocString DESCRIPTION = BUILDINGS.PREFABS.SOLIDCONDUIT.NAME + " Construction";
				}

				// Token: 0x02002C90 RID: 11408
				public class CAN_DO_PLUMBING
				{
					// Token: 0x0400B6D4 RID: 46804
					public static LocString DESCRIPTION = "Pipe Emptying";
				}

				// Token: 0x02002C91 RID: 11409
				public class CAN_USE_ROCKETS
				{
					// Token: 0x0400B6D5 RID: 46805
					public static LocString DESCRIPTION = BUILDINGS.PREFABS.COMMANDMODULE.NAME + " Usage";
				}

				// Token: 0x02002C92 RID: 11410
				public class CAN_DO_ASTRONAUT_TRAINING
				{
					// Token: 0x0400B6D6 RID: 46806
					public static LocString DESCRIPTION = BUILDINGS.PREFABS.ASTRONAUTTRAININGCENTER.NAME + " Usage";
				}

				// Token: 0x02002C93 RID: 11411
				public class CAN_MISSION_CONTROL
				{
					// Token: 0x0400B6D7 RID: 46807
					public static LocString DESCRIPTION = BUILDINGS.PREFABS.MISSIONCONTROL.NAME + " Usage";
				}

				// Token: 0x02002C94 RID: 11412
				public class CAN_PILOT_ROCKET
				{
					// Token: 0x0400B6D8 RID: 46808
					public static LocString DESCRIPTION = BUILDINGS.PREFABS.ROCKETCONTROLSTATION.NAME + " Usage";
				}

				// Token: 0x02002C95 RID: 11413
				public class CAN_COMPOUND
				{
					// Token: 0x0400B6D9 RID: 46809
					public static LocString DESCRIPTION = BUILDINGS.PREFABS.APOTHECARY.NAME + " Usage";
				}

				// Token: 0x02002C96 RID: 11414
				public class CAN_DOCTOR
				{
					// Token: 0x0400B6DA RID: 46810
					public static LocString DESCRIPTION = BUILDINGS.PREFABS.DOCTORSTATION.NAME + " Usage";
				}

				// Token: 0x02002C97 RID: 11415
				public class CAN_ADVANCED_MEDICINE
				{
					// Token: 0x0400B6DB RID: 46811
					public static LocString DESCRIPTION = BUILDINGS.PREFABS.ADVANCEDDOCTORSTATION.NAME + " Usage";
				}

				// Token: 0x02002C98 RID: 11416
				public class CAN_DEMOLISH
				{
					// Token: 0x0400B6DC RID: 46812
					public static LocString DESCRIPTION = "Demolish Gravitas Buildings";
				}
			}

			// Token: 0x02002C99 RID: 11417
			public class ASSIGNMENT_REQUIREMENTS
			{
				// Token: 0x0400B6DD RID: 46813
				public static LocString TITLE = "Qualifications";

				// Token: 0x0400B6DE RID: 46814
				public static LocString NONE = "This position has no qualification requirements";

				// Token: 0x0400B6DF RID: 46815
				public static LocString ALREADY_IS_ROLE = "{0} <b>is already</b> assigned to the {1} position";

				// Token: 0x0400B6E0 RID: 46816
				public static LocString ALREADY_IS_JOBLESS = "{0} <b>is already</b> unemployed";

				// Token: 0x0400B6E1 RID: 46817
				public static LocString MASTERED = "{0} has mastered the {1} position";

				// Token: 0x0400B6E2 RID: 46818
				public static LocString WILL_BE_UNASSIGNED = "Note: Assigning {0} to {1} will <color=#F44A47FF>unassign</color> them from {2}";

				// Token: 0x0400B6E3 RID: 46819
				public static LocString RELEVANT_ATTRIBUTES = "Relevant skills:";

				// Token: 0x0400B6E4 RID: 46820
				public static LocString APTITUDES = "Interests";

				// Token: 0x0400B6E5 RID: 46821
				public static LocString RELEVANT_APTITUDES = "Relevant Interests:";

				// Token: 0x0400B6E6 RID: 46822
				public static LocString NO_APTITUDE = "None";

				// Token: 0x02002C9A RID: 11418
				public class ELIGIBILITY
				{
					// Token: 0x0400B6E7 RID: 46823
					public static LocString ELIGIBLE = "{0} is qualified for the {1} position";

					// Token: 0x0400B6E8 RID: 46824
					public static LocString INELIGIBLE = "{0} is <color=#F44A47FF>not qualified</color> for the {1} position";
				}

				// Token: 0x02002C9B RID: 11419
				public class UNEMPLOYED
				{
					// Token: 0x0400B6E9 RID: 46825
					public static LocString NAME = "Unassigned";

					// Token: 0x0400B6EA RID: 46826
					public static LocString DESCRIPTION = "Duplicant must not already have a job assignment";
				}

				// Token: 0x02002C9C RID: 11420
				public class HAS_COLONY_LEADER
				{
					// Token: 0x0400B6EB RID: 46827
					public static LocString NAME = "Has colony leader";

					// Token: 0x0400B6EC RID: 46828
					public static LocString DESCRIPTION = "A colony leader must be assigned";
				}

				// Token: 0x02002C9D RID: 11421
				public class HAS_ATTRIBUTE_DIGGING_BASIC
				{
					// Token: 0x0400B6ED RID: 46829
					public static LocString NAME = "Basic Digging";

					// Token: 0x0400B6EE RID: 46830
					public static LocString DESCRIPTION = "Must have at least {0} digging skill";
				}

				// Token: 0x02002C9E RID: 11422
				public class HAS_ATTRIBUTE_COOKING_BASIC
				{
					// Token: 0x0400B6EF RID: 46831
					public static LocString NAME = "Basic Cooking";

					// Token: 0x0400B6F0 RID: 46832
					public static LocString DESCRIPTION = "Must have at least {0} cooking skill";
				}

				// Token: 0x02002C9F RID: 11423
				public class HAS_ATTRIBUTE_LEARNING_BASIC
				{
					// Token: 0x0400B6F1 RID: 46833
					public static LocString NAME = "Basic Learning";

					// Token: 0x0400B6F2 RID: 46834
					public static LocString DESCRIPTION = "Must have at least {0} learning skill";
				}

				// Token: 0x02002CA0 RID: 11424
				public class HAS_ATTRIBUTE_LEARNING_MEDIUM
				{
					// Token: 0x0400B6F3 RID: 46835
					public static LocString NAME = "Medium Learning";

					// Token: 0x0400B6F4 RID: 46836
					public static LocString DESCRIPTION = "Must have at least {0} learning skill";
				}

				// Token: 0x02002CA1 RID: 11425
				public class HAS_EXPERIENCE
				{
					// Token: 0x0400B6F5 RID: 46837
					public static LocString NAME = "{0} Experience";

					// Token: 0x0400B6F6 RID: 46838
					public static LocString DESCRIPTION = "Mastery of the <b>{0}</b> job";
				}

				// Token: 0x02002CA2 RID: 11426
				public class HAS_COMPLETED_ANY_OTHER_ROLE
				{
					// Token: 0x0400B6F7 RID: 46839
					public static LocString NAME = "General Experience";

					// Token: 0x0400B6F8 RID: 46840
					public static LocString DESCRIPTION = "Mastery of <b>at least one</b> job";
				}

				// Token: 0x02002CA3 RID: 11427
				public class CHOREGROUP_ENABLED
				{
					// Token: 0x0400B6F9 RID: 46841
					public static LocString NAME = "Can perform {0}";

					// Token: 0x0400B6FA RID: 46842
					public static LocString DESCRIPTION = "Capable of performing <b>{0}</b> jobs";
				}
			}

			// Token: 0x02002CA4 RID: 11428
			public class EXPECTATIONS
			{
				// Token: 0x0400B6FB RID: 46843
				public static LocString TITLE = "Special Provisions Request";

				// Token: 0x0400B6FC RID: 46844
				public static LocString NO_EXPECTATIONS = "No additional provisions are required to perform this job";

				// Token: 0x02002CA5 RID: 11429
				public class PRIVATE_ROOM
				{
					// Token: 0x0400B6FD RID: 46845
					public static LocString NAME = "Private Bedroom";

					// Token: 0x0400B6FE RID: 46846
					public static LocString DESCRIPTION = "Duplicants in this job would appreciate their own place to unwind";
				}

				// Token: 0x02002CA6 RID: 11430
				public class FOOD_QUALITY
				{
					// Token: 0x02002CA7 RID: 11431
					public class MINOR
					{
						// Token: 0x0400B6FF RID: 46847
						public static LocString NAME = "Standard Food";

						// Token: 0x0400B700 RID: 46848
						public static LocString DESCRIPTION = "Duplicants employed in this Tier desire food that meets basic living standards";
					}

					// Token: 0x02002CA8 RID: 11432
					public class MEDIUM
					{
						// Token: 0x0400B701 RID: 46849
						public static LocString NAME = "Good Food";

						// Token: 0x0400B702 RID: 46850
						public static LocString DESCRIPTION = "Duplicants employed in this Tier desire decent food for their efforts";
					}

					// Token: 0x02002CA9 RID: 11433
					public class HIGH
					{
						// Token: 0x0400B703 RID: 46851
						public static LocString NAME = "Great Food";

						// Token: 0x0400B704 RID: 46852
						public static LocString DESCRIPTION = "Duplicants employed in this Tier desire better than average food";
					}

					// Token: 0x02002CAA RID: 11434
					public class VERY_HIGH
					{
						// Token: 0x0400B705 RID: 46853
						public static LocString NAME = "Superb Food";

						// Token: 0x0400B706 RID: 46854
						public static LocString DESCRIPTION = "Duplicants employed in this Tier have a refined taste for food";
					}

					// Token: 0x02002CAB RID: 11435
					public class EXCEPTIONAL
					{
						// Token: 0x0400B707 RID: 46855
						public static LocString NAME = "Ambrosial Food";

						// Token: 0x0400B708 RID: 46856
						public static LocString DESCRIPTION = "Duplicants employed in this Tier expect only the best cuisine";
					}
				}

				// Token: 0x02002CAC RID: 11436
				public class DECOR
				{
					// Token: 0x02002CAD RID: 11437
					public class MINOR
					{
						// Token: 0x0400B709 RID: 46857
						public static LocString NAME = "Minor Decor";

						// Token: 0x0400B70A RID: 46858
						public static LocString DESCRIPTION = "Duplicants employed in this Tier desire slightly improved colony decor";
					}

					// Token: 0x02002CAE RID: 11438
					public class MEDIUM
					{
						// Token: 0x0400B70B RID: 46859
						public static LocString NAME = "Medium Decor";

						// Token: 0x0400B70C RID: 46860
						public static LocString DESCRIPTION = "Duplicants employed in this Tier desire reasonably improved colony decor";
					}

					// Token: 0x02002CAF RID: 11439
					public class HIGH
					{
						// Token: 0x0400B70D RID: 46861
						public static LocString NAME = "High Decor";

						// Token: 0x0400B70E RID: 46862
						public static LocString DESCRIPTION = "Duplicants employed in this Tier desire a decent increase in colony decor";
					}

					// Token: 0x02002CB0 RID: 11440
					public class VERY_HIGH
					{
						// Token: 0x0400B70F RID: 46863
						public static LocString NAME = "Superb Decor";

						// Token: 0x0400B710 RID: 46864
						public static LocString DESCRIPTION = "Duplicants employed in this Tier desire majorly improved colony decor";
					}

					// Token: 0x02002CB1 RID: 11441
					public class UNREASONABLE
					{
						// Token: 0x0400B711 RID: 46865
						public static LocString NAME = "Decadent Decor";

						// Token: 0x0400B712 RID: 46866
						public static LocString DESCRIPTION = "Duplicants employed in this Tier desire unrealistically luxurious improvements to decor";
					}
				}

				// Token: 0x02002CB2 RID: 11442
				public class QUALITYOFLIFE
				{
					// Token: 0x02002CB3 RID: 11443
					public class TIER0
					{
						// Token: 0x0400B713 RID: 46867
						public static LocString NAME = "Morale Requirements";

						// Token: 0x0400B714 RID: 46868
						public static LocString DESCRIPTION = "Tier 0";
					}

					// Token: 0x02002CB4 RID: 11444
					public class TIER1
					{
						// Token: 0x0400B715 RID: 46869
						public static LocString NAME = "Morale Requirements";

						// Token: 0x0400B716 RID: 46870
						public static LocString DESCRIPTION = "Tier 1";
					}

					// Token: 0x02002CB5 RID: 11445
					public class TIER2
					{
						// Token: 0x0400B717 RID: 46871
						public static LocString NAME = "Morale Requirements";

						// Token: 0x0400B718 RID: 46872
						public static LocString DESCRIPTION = "Tier 2";
					}

					// Token: 0x02002CB6 RID: 11446
					public class TIER3
					{
						// Token: 0x0400B719 RID: 46873
						public static LocString NAME = "Morale Requirements";

						// Token: 0x0400B71A RID: 46874
						public static LocString DESCRIPTION = "Tier 3";
					}

					// Token: 0x02002CB7 RID: 11447
					public class TIER4
					{
						// Token: 0x0400B71B RID: 46875
						public static LocString NAME = "Morale Requirements";

						// Token: 0x0400B71C RID: 46876
						public static LocString DESCRIPTION = "Tier 4";
					}

					// Token: 0x02002CB8 RID: 11448
					public class TIER5
					{
						// Token: 0x0400B71D RID: 46877
						public static LocString NAME = "Morale Requirements";

						// Token: 0x0400B71E RID: 46878
						public static LocString DESCRIPTION = "Tier 5";
					}

					// Token: 0x02002CB9 RID: 11449
					public class TIER6
					{
						// Token: 0x0400B71F RID: 46879
						public static LocString NAME = "Morale Requirements";

						// Token: 0x0400B720 RID: 46880
						public static LocString DESCRIPTION = "Tier 6";
					}

					// Token: 0x02002CBA RID: 11450
					public class TIER7
					{
						// Token: 0x0400B721 RID: 46881
						public static LocString NAME = "Morale Requirements";

						// Token: 0x0400B722 RID: 46882
						public static LocString DESCRIPTION = "Tier 7";
					}

					// Token: 0x02002CBB RID: 11451
					public class TIER8
					{
						// Token: 0x0400B723 RID: 46883
						public static LocString NAME = "Morale Requirements";

						// Token: 0x0400B724 RID: 46884
						public static LocString DESCRIPTION = "Tier 8";
					}
				}
			}
		}

		// Token: 0x02002CBC RID: 11452
		public class GAMEPLAY_EVENT_INFO_SCREEN
		{
			// Token: 0x0400B725 RID: 46885
			public static LocString WHERE = "WHERE: {0}";

			// Token: 0x0400B726 RID: 46886
			public static LocString WHEN = "WHEN: {0}";
		}

		// Token: 0x02002CBD RID: 11453
		public class DEBUG_TOOLS
		{
			// Token: 0x0400B727 RID: 46887
			public static LocString ENTER_TEXT = "";

			// Token: 0x0400B728 RID: 46888
			public static LocString DEBUG_ACTIVE = "Debug tools active";

			// Token: 0x0400B729 RID: 46889
			public static LocString INVALID_LOCATION = "Invalid Location";

			// Token: 0x02002CBE RID: 11454
			public class PAINT_ELEMENTS_SCREEN
			{
				// Token: 0x0400B72A RID: 46890
				public static LocString TITLE = "CELL PAINTER";

				// Token: 0x0400B72B RID: 46891
				public static LocString ELEMENT = "Element";

				// Token: 0x0400B72C RID: 46892
				public static LocString MASS_KG = "Mass (kg)";

				// Token: 0x0400B72D RID: 46893
				public static LocString TEMPERATURE_KELVIN = "Temperature (K)";

				// Token: 0x0400B72E RID: 46894
				public static LocString DISEASE = "Disease";

				// Token: 0x0400B72F RID: 46895
				public static LocString DISEASE_COUNT = "Disease Count";

				// Token: 0x0400B730 RID: 46896
				public static LocString BUILDINGS = "Buildings:";

				// Token: 0x0400B731 RID: 46897
				public static LocString CELLS = "Cells:";

				// Token: 0x0400B732 RID: 46898
				public static LocString ADD_FOW_MASK = "Prevent FoW Reveal";

				// Token: 0x0400B733 RID: 46899
				public static LocString REMOVE_FOW_MASK = "Allow FoW Reveal";

				// Token: 0x0400B734 RID: 46900
				public static LocString PAINT = "Paint";

				// Token: 0x0400B735 RID: 46901
				public static LocString SAMPLE = "Sample";

				// Token: 0x0400B736 RID: 46902
				public static LocString STORE = "Store";

				// Token: 0x0400B737 RID: 46903
				public static LocString FILL = "Fill";

				// Token: 0x0400B738 RID: 46904
				public static LocString SPAWN_ALL = "Spawn All (Slow)";
			}

			// Token: 0x02002CBF RID: 11455
			public class SAVE_BASE_TEMPLATE
			{
				// Token: 0x0400B739 RID: 46905
				public static LocString TITLE = "Base and World Tools";

				// Token: 0x0400B73A RID: 46906
				public static LocString SAVE_TITLE = "Save Selection";

				// Token: 0x0400B73B RID: 46907
				public static LocString CLEAR_BUTTON = "Clear Floor";

				// Token: 0x0400B73C RID: 46908
				public static LocString DESTROY_BUTTON = "Destroy";

				// Token: 0x0400B73D RID: 46909
				public static LocString DECONSTRUCT_BUTTON = "Deconstruct";

				// Token: 0x0400B73E RID: 46910
				public static LocString CLEAR_SELECTION_BUTTON = "Clear Selection";

				// Token: 0x0400B73F RID: 46911
				public static LocString DEFAULT_SAVE_NAME = "TemplateSaveName";

				// Token: 0x0400B740 RID: 46912
				public static LocString MORE = "More";

				// Token: 0x0400B741 RID: 46913
				public static LocString BASE_GAME_FOLDER_NAME = "Base Game";

				// Token: 0x02002CC0 RID: 11456
				public class SELECTION_INFO_PANEL
				{
					// Token: 0x0400B742 RID: 46914
					public static LocString TOTAL_MASS = "Total mass: {0}";

					// Token: 0x0400B743 RID: 46915
					public static LocString AVERAGE_MASS = "Average cell mass: {0}";

					// Token: 0x0400B744 RID: 46916
					public static LocString AVERAGE_TEMPERATURE = "Average temperature: {0}";

					// Token: 0x0400B745 RID: 46917
					public static LocString TOTAL_JOULES = "Total joules: {0}";

					// Token: 0x0400B746 RID: 46918
					public static LocString JOULES_PER_KILOGRAM = "Joules per kilogram: {0}";

					// Token: 0x0400B747 RID: 46919
					public static LocString TOTAL_RADS = "Total rads: {0}";

					// Token: 0x0400B748 RID: 46920
					public static LocString AVERAGE_RADS = "Average rads: {0}";
				}
			}
		}

		// Token: 0x02002CC1 RID: 11457
		public class WORLDGEN
		{
			// Token: 0x0400B749 RID: 46921
			public static LocString NOHEADERS = "";

			// Token: 0x0400B74A RID: 46922
			public static LocString COMPLETE = "Success! Space adventure awaits.";

			// Token: 0x0400B74B RID: 46923
			public static LocString FAILED = "Goodness, has this ever gone terribly wrong!";

			// Token: 0x0400B74C RID: 46924
			public static LocString RESTARTING = "Rebooting...";

			// Token: 0x0400B74D RID: 46925
			public static LocString LOADING = "Loading world...";

			// Token: 0x0400B74E RID: 46926
			public static LocString GENERATINGWORLD = "The Galaxy Synthesizer";

			// Token: 0x0400B74F RID: 46927
			public static LocString CHOOSEWORLDSIZE = "Select the magnitude of your new galaxy.";

			// Token: 0x0400B750 RID: 46928
			public static LocString USING_PLAYER_SEED = "Using selected worldgen seed: {0}";

			// Token: 0x0400B751 RID: 46929
			public static LocString CLEARINGLEVEL = "Staring into the void...";

			// Token: 0x0400B752 RID: 46930
			public static LocString GENERATESOLARSYSTEM = "Catalyzing Big Bang...";

			// Token: 0x0400B753 RID: 46931
			public static LocString GENERATESOLARSYSTEM1 = "Catalyzing Big Bang...";

			// Token: 0x0400B754 RID: 46932
			public static LocString GENERATESOLARSYSTEM2 = "Catalyzing Big Bang...";

			// Token: 0x0400B755 RID: 46933
			public static LocString GENERATESOLARSYSTEM3 = "Catalyzing Big Bang...";

			// Token: 0x0400B756 RID: 46934
			public static LocString GENERATESOLARSYSTEM4 = "Catalyzing Big Bang...";

			// Token: 0x0400B757 RID: 46935
			public static LocString GENERATESOLARSYSTEM5 = "Catalyzing Big Bang...";

			// Token: 0x0400B758 RID: 46936
			public static LocString GENERATESOLARSYSTEM6 = "Approaching event horizon...";

			// Token: 0x0400B759 RID: 46937
			public static LocString GENERATESOLARSYSTEM7 = "Approaching event horizon...";

			// Token: 0x0400B75A RID: 46938
			public static LocString GENERATESOLARSYSTEM8 = "Approaching event horizon...";

			// Token: 0x0400B75B RID: 46939
			public static LocString GENERATESOLARSYSTEM9 = "Approaching event horizon...";

			// Token: 0x0400B75C RID: 46940
			public static LocString SETUPNOISE = "BANG!";

			// Token: 0x0400B75D RID: 46941
			public static LocString BUILDNOISESOURCE = "Sorting quadrillions of atoms...";

			// Token: 0x0400B75E RID: 46942
			public static LocString BUILDNOISESOURCE1 = "Sorting quadrillions of atoms...";

			// Token: 0x0400B75F RID: 46943
			public static LocString BUILDNOISESOURCE2 = "Sorting quadrillions of atoms...";

			// Token: 0x0400B760 RID: 46944
			public static LocString BUILDNOISESOURCE3 = "Ironing the fabric of creation...";

			// Token: 0x0400B761 RID: 46945
			public static LocString BUILDNOISESOURCE4 = "Ironing the fabric of creation...";

			// Token: 0x0400B762 RID: 46946
			public static LocString BUILDNOISESOURCE5 = "Ironing the fabric of creation...";

			// Token: 0x0400B763 RID: 46947
			public static LocString BUILDNOISESOURCE6 = "Taking hot meteor shower...";

			// Token: 0x0400B764 RID: 46948
			public static LocString BUILDNOISESOURCE7 = "Tightening asteroid belts...";

			// Token: 0x0400B765 RID: 46949
			public static LocString BUILDNOISESOURCE8 = "Tightening asteroid belts...";

			// Token: 0x0400B766 RID: 46950
			public static LocString BUILDNOISESOURCE9 = "Tightening asteroid belts...";

			// Token: 0x0400B767 RID: 46951
			public static LocString GENERATENOISE = "Baking igneous rock...";

			// Token: 0x0400B768 RID: 46952
			public static LocString GENERATENOISE1 = "Multilayering sediment...";

			// Token: 0x0400B769 RID: 46953
			public static LocString GENERATENOISE2 = "Multilayering sediment...";

			// Token: 0x0400B76A RID: 46954
			public static LocString GENERATENOISE3 = "Multilayering sediment...";

			// Token: 0x0400B76B RID: 46955
			public static LocString GENERATENOISE4 = "Superheating gases...";

			// Token: 0x0400B76C RID: 46956
			public static LocString GENERATENOISE5 = "Superheating gases...";

			// Token: 0x0400B76D RID: 46957
			public static LocString GENERATENOISE6 = "Superheating gases...";

			// Token: 0x0400B76E RID: 46958
			public static LocString GENERATENOISE7 = "Vacuuming out vacuums...";

			// Token: 0x0400B76F RID: 46959
			public static LocString GENERATENOISE8 = "Vacuuming out vacuums...";

			// Token: 0x0400B770 RID: 46960
			public static LocString GENERATENOISE9 = "Vacuuming out vacuums...";

			// Token: 0x0400B771 RID: 46961
			public static LocString NORMALISENOISE = "Interpolating suffocating gas...";

			// Token: 0x0400B772 RID: 46962
			public static LocString WORLDLAYOUT = "Freezing ice formations...";

			// Token: 0x0400B773 RID: 46963
			public static LocString WORLDLAYOUT1 = "Freezing ice formations...";

			// Token: 0x0400B774 RID: 46964
			public static LocString WORLDLAYOUT2 = "Freezing ice formations...";

			// Token: 0x0400B775 RID: 46965
			public static LocString WORLDLAYOUT3 = "Freezing ice formations...";

			// Token: 0x0400B776 RID: 46966
			public static LocString WORLDLAYOUT4 = "Melting magma...";

			// Token: 0x0400B777 RID: 46967
			public static LocString WORLDLAYOUT5 = "Melting magma...";

			// Token: 0x0400B778 RID: 46968
			public static LocString WORLDLAYOUT6 = "Melting magma...";

			// Token: 0x0400B779 RID: 46969
			public static LocString WORLDLAYOUT7 = "Sprinkling sand...";

			// Token: 0x0400B77A RID: 46970
			public static LocString WORLDLAYOUT8 = "Sprinkling sand...";

			// Token: 0x0400B77B RID: 46971
			public static LocString WORLDLAYOUT9 = "Sprinkling sand...";

			// Token: 0x0400B77C RID: 46972
			public static LocString WORLDLAYOUT10 = "Sprinkling sand...";

			// Token: 0x0400B77D RID: 46973
			public static LocString COMPLETELAYOUT = "Cooling glass...";

			// Token: 0x0400B77E RID: 46974
			public static LocString COMPLETELAYOUT1 = "Cooling glass...";

			// Token: 0x0400B77F RID: 46975
			public static LocString COMPLETELAYOUT2 = "Cooling glass...";

			// Token: 0x0400B780 RID: 46976
			public static LocString COMPLETELAYOUT3 = "Cooling glass...";

			// Token: 0x0400B781 RID: 46977
			public static LocString COMPLETELAYOUT4 = "Digging holes...";

			// Token: 0x0400B782 RID: 46978
			public static LocString COMPLETELAYOUT5 = "Digging holes...";

			// Token: 0x0400B783 RID: 46979
			public static LocString COMPLETELAYOUT6 = "Digging holes...";

			// Token: 0x0400B784 RID: 46980
			public static LocString COMPLETELAYOUT7 = "Adding buckets of dirt...";

			// Token: 0x0400B785 RID: 46981
			public static LocString COMPLETELAYOUT8 = "Adding buckets of dirt...";

			// Token: 0x0400B786 RID: 46982
			public static LocString COMPLETELAYOUT9 = "Adding buckets of dirt...";

			// Token: 0x0400B787 RID: 46983
			public static LocString COMPLETELAYOUT10 = "Adding buckets of dirt...";

			// Token: 0x0400B788 RID: 46984
			public static LocString PROCESSRIVERS = "Pouring rivers...";

			// Token: 0x0400B789 RID: 46985
			public static LocString CONVERTTERRAINCELLSTOEDGES = "Hardening diamonds...";

			// Token: 0x0400B78A RID: 46986
			public static LocString PROCESSING = "Embedding metals...";

			// Token: 0x0400B78B RID: 46987
			public static LocString PROCESSING1 = "Embedding metals...";

			// Token: 0x0400B78C RID: 46988
			public static LocString PROCESSING2 = "Embedding metals...";

			// Token: 0x0400B78D RID: 46989
			public static LocString PROCESSING3 = "Burying precious ore...";

			// Token: 0x0400B78E RID: 46990
			public static LocString PROCESSING4 = "Burying precious ore...";

			// Token: 0x0400B78F RID: 46991
			public static LocString PROCESSING5 = "Burying precious ore...";

			// Token: 0x0400B790 RID: 46992
			public static LocString PROCESSING6 = "Burying precious ore...";

			// Token: 0x0400B791 RID: 46993
			public static LocString PROCESSING7 = "Excavating tunnels...";

			// Token: 0x0400B792 RID: 46994
			public static LocString PROCESSING8 = "Excavating tunnels...";

			// Token: 0x0400B793 RID: 46995
			public static LocString PROCESSING9 = "Excavating tunnels...";

			// Token: 0x0400B794 RID: 46996
			public static LocString BORDERS = "Just adding water...";

			// Token: 0x0400B795 RID: 46997
			public static LocString BORDERS1 = "Just adding water...";

			// Token: 0x0400B796 RID: 46998
			public static LocString BORDERS2 = "Staring at the void...";

			// Token: 0x0400B797 RID: 46999
			public static LocString BORDERS3 = "Staring at the void...";

			// Token: 0x0400B798 RID: 47000
			public static LocString BORDERS4 = "Staring at the void...";

			// Token: 0x0400B799 RID: 47001
			public static LocString BORDERS5 = "Avoiding awkward eye contact with the void...";

			// Token: 0x0400B79A RID: 47002
			public static LocString BORDERS6 = "Avoiding awkward eye contact with the void...";

			// Token: 0x0400B79B RID: 47003
			public static LocString BORDERS7 = "Avoiding awkward eye contact with the void...";

			// Token: 0x0400B79C RID: 47004
			public static LocString BORDERS8 = "Avoiding awkward eye contact with the void...";

			// Token: 0x0400B79D RID: 47005
			public static LocString BORDERS9 = "Avoiding awkward eye contact with the void...";

			// Token: 0x0400B79E RID: 47006
			public static LocString DRAWWORLDBORDER = "Establishing personal boundaries...";

			// Token: 0x0400B79F RID: 47007
			public static LocString PLACINGTEMPLATES = "Generating interest...";

			// Token: 0x0400B7A0 RID: 47008
			public static LocString SETTLESIM = "Infusing oxygen...";

			// Token: 0x0400B7A1 RID: 47009
			public static LocString SETTLESIM1 = "Infusing oxygen...";

			// Token: 0x0400B7A2 RID: 47010
			public static LocString SETTLESIM2 = "Too much oxygen. Removing...";

			// Token: 0x0400B7A3 RID: 47011
			public static LocString SETTLESIM3 = "Too much oxygen. Removing...";

			// Token: 0x0400B7A4 RID: 47012
			public static LocString SETTLESIM4 = "Ideal oxygen levels achieved...";

			// Token: 0x0400B7A5 RID: 47013
			public static LocString SETTLESIM5 = "Ideal oxygen levels achieved...";

			// Token: 0x0400B7A6 RID: 47014
			public static LocString SETTLESIM6 = "Planting space flora...";

			// Token: 0x0400B7A7 RID: 47015
			public static LocString SETTLESIM7 = "Planting space flora...";

			// Token: 0x0400B7A8 RID: 47016
			public static LocString SETTLESIM8 = "Releasing wildlife...";

			// Token: 0x0400B7A9 RID: 47017
			public static LocString SETTLESIM9 = "Releasing wildlife...";

			// Token: 0x0400B7AA RID: 47018
			public static LocString ANALYZINGWORLD = "Shuffling DNA Blueprints...";

			// Token: 0x0400B7AB RID: 47019
			public static LocString ANALYZINGWORLDCOMPLETE = "Tidying up for the Duplicants...";

			// Token: 0x0400B7AC RID: 47020
			public static LocString PLACINGCREATURES = "Building the suspense...";
		}

		// Token: 0x02002CC2 RID: 11458
		public class TOOLTIPS
		{
			// Token: 0x0400B7AD RID: 47021
			public static LocString MANAGEMENTMENU_JOBS = string.Concat(new string[]
			{
				"Manage my Duplicant Priorities {Hotkey}\n\nDuplicant Priorities",
				UI.PST_KEYWORD,
				" are calculated <i>before</i> the ",
				UI.PRE_KEYWORD,
				"Building Priorities",
				UI.PST_KEYWORD,
				" set by the ",
				UI.FormatAsTool("Priority Tool", global::Action.Prioritize)
			});

			// Token: 0x0400B7AE RID: 47022
			public static LocString MANAGEMENTMENU_CONSUMABLES = "Manage my Duplicants' diets and medications {Hotkey}";

			// Token: 0x0400B7AF RID: 47023
			public static LocString MANAGEMENTMENU_VITALS = "View my Duplicants' vitals {Hotkey}";

			// Token: 0x0400B7B0 RID: 47024
			public static LocString MANAGEMENTMENU_RESEARCH = "View the Research Tree {Hotkey}";

			// Token: 0x0400B7B1 RID: 47025
			public static LocString MANAGEMENTMENU_RESEARCH_NO_RESEARCH = "No active research projects";

			// Token: 0x0400B7B2 RID: 47026
			public static LocString MANAGEMENTMENU_RESEARCH_CARD_NAME = "Currently researching: {0}";

			// Token: 0x0400B7B3 RID: 47027
			public static LocString MANAGEMENTMENU_RESEARCH_ITEM_LINE = "• {0}";

			// Token: 0x0400B7B4 RID: 47028
			public static LocString MANAGEMENTMENU_REQUIRES_RESEARCH = string.Concat(new string[]
			{
				"Build a Research Station to unlock this menu\n\nThe ",
				BUILDINGS.PREFABS.RESEARCHCENTER.NAME,
				" can be found in the ",
				UI.FormatAsBuildMenuTab("Stations Tab", global::Action.Plan10),
				" of the Build Menu"
			});

			// Token: 0x0400B7B5 RID: 47029
			public static LocString MANAGEMENTMENU_DAILYREPORT = "View each cycle's Colony Report {Hotkey}";

			// Token: 0x0400B7B6 RID: 47030
			public static LocString MANAGEMENTMENU_CODEX = "Browse entries in my Database {Hotkey}";

			// Token: 0x0400B7B7 RID: 47031
			public static LocString MANAGEMENTMENU_SCHEDULE = "Adjust the colony's time usage {Hotkey}";

			// Token: 0x0400B7B8 RID: 47032
			public static LocString MANAGEMENTMENU_STARMAP = "Manage astronaut rocket missions {Hotkey}";

			// Token: 0x0400B7B9 RID: 47033
			public static LocString MANAGEMENTMENU_REQUIRES_TELESCOPE = string.Concat(new string[]
			{
				"Build a Telescope to unlock this menu\n\nThe ",
				BUILDINGS.PREFABS.TELESCOPE.NAME,
				" can be found in the ",
				UI.FormatAsBuildMenuTab("Stations Tab", global::Action.Plan10),
				" of the Build Menu"
			});

			// Token: 0x0400B7BA RID: 47034
			public static LocString MANAGEMENTMENU_REQUIRES_TELESCOPE_CLUSTER = string.Concat(new string[]
			{
				"Build a Telescope to unlock this menu\n\nThe ",
				BUILDINGS.PREFABS.TELESCOPE.NAME,
				" can be found in the ",
				UI.FormatAsBuildMenuTab("Rocketry Tab", global::Action.Plan14),
				" of the Build Menu"
			});

			// Token: 0x0400B7BB RID: 47035
			public static LocString MANAGEMENTMENU_SKILLS = "Manage Duplicants' Skill assignments {Hotkey}";

			// Token: 0x0400B7BC RID: 47036
			public static LocString MANAGEMENTMENU_REQUIRES_SKILL_STATION = string.Concat(new string[]
			{
				"Build a Printing Pod to unlock this menu\n\nThe ",
				BUILDINGS.PREFABS.HEADQUARTERSCOMPLETE.NAME,
				" can be found in the ",
				UI.FormatAsBuildMenuTab("Base Tab", global::Action.Plan1),
				" of the Build Menu"
			});

			// Token: 0x0400B7BD RID: 47037
			public static LocString MANAGEMENTMENU_PAUSEMENU = "Open the game menu {Hotkey}";

			// Token: 0x0400B7BE RID: 47038
			public static LocString MANAGEMENTMENU_RESOURCES = "Open the resource management screen {Hotkey}";

			// Token: 0x0400B7BF RID: 47039
			public static LocString OPEN_CODEX_ENTRY = "View full entry in database";

			// Token: 0x0400B7C0 RID: 47040
			public static LocString NO_CODEX_ENTRY = "No database entry available";

			// Token: 0x0400B7C1 RID: 47041
			public static LocString OPEN_RESOURCE_INFO = "{0} of {1} available for the Duplicants on this asteroid to use\n\nClick to open Resources menu";

			// Token: 0x0400B7C2 RID: 47042
			public static LocString CHANGE_OUTFIT = "Change this Duplicant's outfit";

			// Token: 0x0400B7C3 RID: 47043
			public static LocString CHANGE_MATERIAL = "Change this building's construction material";

			// Token: 0x0400B7C4 RID: 47044
			public static LocString METERSCREEN_AVGSTRESS = "Highest Stress: {0}";

			// Token: 0x0400B7C5 RID: 47045
			public static LocString METERSCREEN_MEALHISTORY = "Calories Available: {0}\n\nDuplicants consume a minimum of {1} calories each per cycle";

			// Token: 0x0400B7C6 RID: 47046
			public static LocString METERSCREEN_ELECTROBANK_JOULES = "Joules Available: {0}\n\nBionic Duplicants use a minimum of X each per cycle\n\nPower Banks Available: Y\n";

			// Token: 0x0400B7C7 RID: 47047
			public static LocString METERSCREEN_POPULATION = "Population: {0}";

			// Token: 0x0400B7C8 RID: 47048
			public static LocString METERSCREEN_POPULATION_CLUSTER = UI.PRE_KEYWORD + "{0}" + UI.PST_KEYWORD + " Population: {1}\nTotal Population: {2}";

			// Token: 0x0400B7C9 RID: 47049
			public static LocString METERSCREEN_SICK_DUPES = "Sick Duplicants: {0}";

			// Token: 0x0400B7CA RID: 47050
			public static LocString METERSCREEN_INVALID_FOOD_TYPE = "Invalid Food Type: {0}";

			// Token: 0x0400B7CB RID: 47051
			public static LocString METERSCREEN_INVALID_ELECTROBANK_TYPE = "Invalid Power Bank Type: {0}";

			// Token: 0x0400B7CC RID: 47052
			public static LocString PLAYBUTTON = "Start";

			// Token: 0x0400B7CD RID: 47053
			public static LocString PAUSEBUTTON = "Pause";

			// Token: 0x0400B7CE RID: 47054
			public static LocString PAUSE = "Pause {Hotkey}";

			// Token: 0x0400B7CF RID: 47055
			public static LocString UNPAUSE = "Unpause {Hotkey}";

			// Token: 0x0400B7D0 RID: 47056
			public static LocString SPEEDBUTTON_SLOW = "Slow speed {Hotkey}";

			// Token: 0x0400B7D1 RID: 47057
			public static LocString SPEEDBUTTON_MEDIUM = "Medium speed {Hotkey}";

			// Token: 0x0400B7D2 RID: 47058
			public static LocString SPEEDBUTTON_FAST = "Fast speed {Hotkey}";

			// Token: 0x0400B7D3 RID: 47059
			public static LocString RED_ALERT_TITLE = "Toggle Red Alert";

			// Token: 0x0400B7D4 RID: 47060
			public static LocString RED_ALERT_CONTENT = "Duplicants will work, ignoring schedules and their basic needs\n\nUse in case of emergency";

			// Token: 0x0400B7D5 RID: 47061
			public static LocString DISINFECTBUTTON = "Disinfect buildings {Hotkey}";

			// Token: 0x0400B7D6 RID: 47062
			public static LocString MOPBUTTON = "Mop liquid spills {Hotkey}";

			// Token: 0x0400B7D7 RID: 47063
			public static LocString DIGBUTTON = "Set dig errands {Hotkey}";

			// Token: 0x0400B7D8 RID: 47064
			public static LocString CANCELBUTTON = "Cancel errands {Hotkey}";

			// Token: 0x0400B7D9 RID: 47065
			public static LocString DECONSTRUCTBUTTON = "Demolish buildings {Hotkey}";

			// Token: 0x0400B7DA RID: 47066
			public static LocString ATTACKBUTTON = "Attack poor, wild critters {Hotkey}";

			// Token: 0x0400B7DB RID: 47067
			public static LocString CAPTUREBUTTON = "Capture critters {Hotkey}";

			// Token: 0x0400B7DC RID: 47068
			public static LocString CLEARBUTTON = "Move debris into storage {Hotkey}";

			// Token: 0x0400B7DD RID: 47069
			public static LocString HARVESTBUTTON = "Harvest plants {Hotkey}";

			// Token: 0x0400B7DE RID: 47070
			public static LocString PRIORITIZEMAINBUTTON = "";

			// Token: 0x0400B7DF RID: 47071
			public static LocString PRIORITIZEBUTTON = string.Concat(new string[]
			{
				"Set Building Priority {Hotkey}\n\nDuplicant Priorities",
				UI.PST_KEYWORD,
				" ",
				UI.FormatAsHotKey(global::Action.ManagePriorities),
				" are calculated <i>before</i> the ",
				UI.PRE_KEYWORD,
				"Building Priorities",
				UI.PST_KEYWORD,
				" set by this tool"
			});

			// Token: 0x0400B7E0 RID: 47072
			public static LocString CLEANUPMAINBUTTON = "Mop and sweep messy floors {Hotkey}";

			// Token: 0x0400B7E1 RID: 47073
			public static LocString CANCELDECONSTRUCTIONBUTTON = "Cancel queued orders or deconstruct existing buildings {Hotkey}";

			// Token: 0x0400B7E2 RID: 47074
			public static LocString HELP_ROTATE_KEY = "Press " + UI.FormatAsHotKey(global::Action.RotateBuilding) + " to Rotate";

			// Token: 0x0400B7E3 RID: 47075
			public static LocString HELP_BUILDLOCATION_INVALID_CELL = "Invalid Cell";

			// Token: 0x0400B7E4 RID: 47076
			public static LocString HELP_BUILDLOCATION_MISSING_TELEPAD = "World has no " + BUILDINGS.PREFABS.HEADQUARTERSCOMPLETE.NAME + " or " + BUILDINGS.PREFABS.EXOBASEHEADQUARTERS.NAME;

			// Token: 0x0400B7E5 RID: 47077
			public static LocString HELP_BUILDLOCATION_FLOOR = "Must be built on solid ground";

			// Token: 0x0400B7E6 RID: 47078
			public static LocString HELP_BUILDLOCATION_WALL = "Must be built against a wall";

			// Token: 0x0400B7E7 RID: 47079
			public static LocString HELP_BUILDLOCATION_FLOOR_OR_ATTACHPOINT = "Must be built on solid ground or overlapping an {0}";

			// Token: 0x0400B7E8 RID: 47080
			public static LocString HELP_BUILDLOCATION_OCCUPIED = "Must be built in unoccupied space";

			// Token: 0x0400B7E9 RID: 47081
			public static LocString HELP_BUILDLOCATION_CEILING = "Must be built on the ceiling";

			// Token: 0x0400B7EA RID: 47082
			public static LocString HELP_BUILDLOCATION_INSIDEGROUND = "Must be built in the ground";

			// Token: 0x0400B7EB RID: 47083
			public static LocString HELP_BUILDLOCATION_ATTACHPOINT = "Must be built overlapping a {0}";

			// Token: 0x0400B7EC RID: 47084
			public static LocString HELP_BUILDLOCATION_SPACE = "Must be built on the surface in space";

			// Token: 0x0400B7ED RID: 47085
			public static LocString HELP_BUILDLOCATION_CORNER = "Must be built in a corner";

			// Token: 0x0400B7EE RID: 47086
			public static LocString HELP_BUILDLOCATION_CORNER_FLOOR = "Must be built in a corner on the ground";

			// Token: 0x0400B7EF RID: 47087
			public static LocString HELP_BUILDLOCATION_BELOWROCKETCEILING = "Must be placed further from the edge of space";

			// Token: 0x0400B7F0 RID: 47088
			public static LocString HELP_BUILDLOCATION_ONROCKETENVELOPE = "Must be built on the interior wall of a rocket";

			// Token: 0x0400B7F1 RID: 47089
			public static LocString HELP_BUILDLOCATION_LIQUID_CONDUIT_FORBIDDEN = "Obstructed by a building";

			// Token: 0x0400B7F2 RID: 47090
			public static LocString HELP_BUILDLOCATION_NOT_IN_TILES = "Cannot be built inside tile";

			// Token: 0x0400B7F3 RID: 47091
			public static LocString HELP_BUILDLOCATION_GASPORTS_OVERLAP = "Gas ports cannot overlap";

			// Token: 0x0400B7F4 RID: 47092
			public static LocString HELP_BUILDLOCATION_LIQUIDPORTS_OVERLAP = "Liquid ports cannot overlap";

			// Token: 0x0400B7F5 RID: 47093
			public static LocString HELP_BUILDLOCATION_SOLIDPORTS_OVERLAP = "Solid ports cannot overlap";

			// Token: 0x0400B7F6 RID: 47094
			public static LocString HELP_BUILDLOCATION_LOGIC_PORTS_OBSTRUCTED = "Automation ports cannot overlap";

			// Token: 0x0400B7F7 RID: 47095
			public static LocString HELP_BUILDLOCATION_WIRECONNECTORS_OVERLAP = "Power connectors cannot overlap";

			// Token: 0x0400B7F8 RID: 47096
			public static LocString HELP_BUILDLOCATION_HIGHWATT_NOT_IN_TILE = "Heavi-Watt connectors cannot be built inside tile";

			// Token: 0x0400B7F9 RID: 47097
			public static LocString HELP_BUILDLOCATION_WIRE_OBSTRUCTION = "Obstructed by Heavi-Watt Wire";

			// Token: 0x0400B7FA RID: 47098
			public static LocString HELP_BUILDLOCATION_BACK_WALL = "Obstructed by back wall";

			// Token: 0x0400B7FB RID: 47099
			public static LocString HELP_TUBELOCATION_NO_UTURNS = "Can't U-Turn";

			// Token: 0x0400B7FC RID: 47100
			public static LocString HELP_TUBELOCATION_STRAIGHT_BRIDGES = "Can't Turn Here";

			// Token: 0x0400B7FD RID: 47101
			public static LocString HELP_REQUIRES_ROOM = "Must be in a " + UI.PRE_KEYWORD + "Room" + UI.PST_KEYWORD;

			// Token: 0x0400B7FE RID: 47102
			public static LocString OXYGENOVERLAYSTRING = "Displays ambient oxygen density {Hotkey}";

			// Token: 0x0400B7FF RID: 47103
			public static LocString POWEROVERLAYSTRING = "Displays power grid components {Hotkey}";

			// Token: 0x0400B800 RID: 47104
			public static LocString TEMPERATUREOVERLAYSTRING = "Displays ambient temperature {Hotkey}";

			// Token: 0x0400B801 RID: 47105
			public static LocString HEATFLOWOVERLAYSTRING = "Displays comfortable temperatures for Duplicants {Hotkey}";

			// Token: 0x0400B802 RID: 47106
			public static LocString SUITOVERLAYSTRING = "Displays Exosuits and related buildings {Hotkey}";

			// Token: 0x0400B803 RID: 47107
			public static LocString LOGICOVERLAYSTRING = "Displays automation grid components {Hotkey}";

			// Token: 0x0400B804 RID: 47108
			public static LocString ROOMSOVERLAYSTRING = "Displays special purpose rooms and bonuses {Hotkey}";

			// Token: 0x0400B805 RID: 47109
			public static LocString JOULESOVERLAYSTRING = "Displays the thermal energy in each cell";

			// Token: 0x0400B806 RID: 47110
			public static LocString LIGHTSOVERLAYSTRING = "Displays the visibility radius of light sources {Hotkey}";

			// Token: 0x0400B807 RID: 47111
			public static LocString LIQUIDVENTOVERLAYSTRING = "Displays liquid pipe system components {Hotkey}";

			// Token: 0x0400B808 RID: 47112
			public static LocString GASVENTOVERLAYSTRING = "Displays gas pipe system components {Hotkey}";

			// Token: 0x0400B809 RID: 47113
			public static LocString DECOROVERLAYSTRING = "Displays areas with Morale-boosting decor values {Hotkey}";

			// Token: 0x0400B80A RID: 47114
			public static LocString PRIORITIESOVERLAYSTRING = "Displays work priority values {Hotkey}";

			// Token: 0x0400B80B RID: 47115
			public static LocString DISEASEOVERLAYSTRING = "Displays areas of disease risk {Hotkey}";

			// Token: 0x0400B80C RID: 47116
			public static LocString NOISE_POLLUTION_OVERLAY_STRING = "Displays ambient noise levels {Hotkey}";

			// Token: 0x0400B80D RID: 47117
			public static LocString CROPS_OVERLAY_STRING = "Displays plant growth progress {Hotkey}";

			// Token: 0x0400B80E RID: 47118
			public static LocString CONVEYOR_OVERLAY_STRING = "Displays conveyor transport components {Hotkey}";

			// Token: 0x0400B80F RID: 47119
			public static LocString TILEMODE_OVERLAY_STRING = "Displays material information {Hotkey}";

			// Token: 0x0400B810 RID: 47120
			public static LocString REACHABILITYOVERLAYSTRING = "Displays areas accessible by Duplicants";

			// Token: 0x0400B811 RID: 47121
			public static LocString RADIATIONOVERLAYSTRING = "Displays radiation levels {Hotkey}";

			// Token: 0x0400B812 RID: 47122
			public static LocString ENERGYREQUIRED = UI.FormatAsLink("Power", "POWER") + " Required";

			// Token: 0x0400B813 RID: 47123
			public static LocString ENERGYGENERATED = UI.FormatAsLink("Power", "POWER") + " Produced";

			// Token: 0x0400B814 RID: 47124
			public static LocString INFOPANEL = "The Info Panel contains an overview of the basic information about my Duplicant";

			// Token: 0x0400B815 RID: 47125
			public static LocString VITALSPANEL = "The Vitals Panel monitors the status and well being of my Duplicant";

			// Token: 0x0400B816 RID: 47126
			public static LocString STRESSPANEL = "The Stress Panel offers a detailed look at what is affecting my Duplicant psychologically";

			// Token: 0x0400B817 RID: 47127
			public static LocString STATSPANEL = "The Stats Panel gives me an overview of my Duplicant's individual stats";

			// Token: 0x0400B818 RID: 47128
			public static LocString ITEMSPANEL = "The Items Panel displays everything this Duplicant is in possession of";

			// Token: 0x0400B819 RID: 47129
			public static LocString STRESSDESCRIPTION = string.Concat(new string[]
			{
				"Accommodate my Duplicant's needs to manage their ",
				UI.FormatAsLink("Stress", "STRESS"),
				".\n\nLow ",
				UI.FormatAsLink("Stress", "STRESS"),
				" can provide a productivity boost, while high ",
				UI.FormatAsLink("Stress", "STRESS"),
				" can impair production or even lead to a nervous breakdown."
			});

			// Token: 0x0400B81A RID: 47130
			public static LocString ALERTSTOOLTIP = "Alerts provide important information about what's happening in the colony right now";

			// Token: 0x0400B81B RID: 47131
			public static LocString MESSAGESTOOLTIP = "Messages are events that have happened and tips to help me manage my colony";

			// Token: 0x0400B81C RID: 47132
			public static LocString NEXTMESSAGESTOOLTIP = "Next message";

			// Token: 0x0400B81D RID: 47133
			public static LocString CLOSETOOLTIP = "Close";

			// Token: 0x0400B81E RID: 47134
			public static LocString DISMISSMESSAGE = "Dismiss message";

			// Token: 0x0400B81F RID: 47135
			public static LocString RECIPE_QUEUE = "Queue {0} for continuous fabrication";

			// Token: 0x0400B820 RID: 47136
			public static LocString RED_ALERT_BUTTON_ON = "Enable Red Alert";

			// Token: 0x0400B821 RID: 47137
			public static LocString RED_ALERT_BUTTON_OFF = "Disable Red Alert";

			// Token: 0x0400B822 RID: 47138
			public static LocString JOBSSCREEN_PRIORITY = "High priority tasks are always performed before low priority tasks.\n\nHowever, a busy Duplicant will continue to work on their current work errand until it's complete, even if a more important errand becomes available.";

			// Token: 0x0400B823 RID: 47139
			public static LocString JOBSSCREEN_ATTRIBUTES = "The following attributes affect a Duplicant's efficiency at this errand:";

			// Token: 0x0400B824 RID: 47140
			public static LocString JOBSSCREEN_CANNOTPERFORMTASK = "{0} cannot perform this errand.";

			// Token: 0x0400B825 RID: 47141
			public static LocString JOBSSCREEN_RELEVANT_ATTRIBUTES = "Relevant Attributes:";

			// Token: 0x0400B826 RID: 47142
			public static LocString SORTCOLUMN = UI.CLICK(UI.ClickType.Click) + " to sort";

			// Token: 0x0400B827 RID: 47143
			public static LocString NOMATERIAL = "Not enough materials";

			// Token: 0x0400B828 RID: 47144
			public static LocString SELECTAMATERIAL = "There are insufficient materials to construct this building";

			// Token: 0x0400B829 RID: 47145
			public static LocString EDITNAME = "Give this Duplicant a new name";

			// Token: 0x0400B82A RID: 47146
			public static LocString RANDOMIZENAME = "Randomize this Duplicant's name";

			// Token: 0x0400B82B RID: 47147
			public static LocString EDITNAMEGENERIC = "Rename {0}";

			// Token: 0x0400B82C RID: 47148
			public static LocString EDITNAMEROCKET = "Rename this rocket";

			// Token: 0x0400B82D RID: 47149
			public static LocString BASE_VALUE = "Base Value";

			// Token: 0x0400B82E RID: 47150
			public static LocString MATIERIAL_MOD = "Made out of {0}";

			// Token: 0x0400B82F RID: 47151
			public static LocString VITALS_CHECKBOX_TEMPERATURE = string.Concat(new string[]
			{
				"This plant's internal ",
				UI.PRE_KEYWORD,
				"Temperature",
				UI.PST_KEYWORD,
				" is <b>{temperature}</b>"
			});

			// Token: 0x0400B830 RID: 47152
			public static LocString VITALS_CHECKBOX_PRESSURE = string.Concat(new string[]
			{
				"The current ",
				UI.PRE_KEYWORD,
				"Gas",
				UI.PST_KEYWORD,
				" pressure is <b>{pressure}</b>"
			});

			// Token: 0x0400B831 RID: 47153
			public static LocString VITALS_CHECKBOX_ATMOSPHERE = "This plant is immersed in {element}";

			// Token: 0x0400B832 RID: 47154
			public static LocString VITALS_CHECKBOX_ILLUMINATION_DARK = "This plant is currently in the dark";

			// Token: 0x0400B833 RID: 47155
			public static LocString VITALS_CHECKBOX_ILLUMINATION_LIGHT = "This plant is currently lit";

			// Token: 0x0400B834 RID: 47156
			public static LocString VITALS_CHECKBOX_SPACETREE_ILLUMINATION_DARK = "This plant must be lit in order to produce " + UI.PRE_KEYWORD + "Nectar" + UI.PST_KEYWORD;

			// Token: 0x0400B835 RID: 47157
			public static LocString VITALS_CHECKBOX_SPACETREE_ILLUMINATION_LIGHT = string.Concat(new string[]
			{
				"This plant is currently lit, and will produce ",
				UI.PRE_KEYWORD,
				"Nectar",
				UI.PST_KEYWORD,
				" when fully grown"
			});

			// Token: 0x0400B836 RID: 47158
			public static LocString VITALS_CHECKBOX_FERTILIZER = string.Concat(new string[]
			{
				"<b>{mass}</b> of ",
				UI.PRE_KEYWORD,
				"Fertilizer",
				UI.PST_KEYWORD,
				" is currently available"
			});

			// Token: 0x0400B837 RID: 47159
			public static LocString VITALS_CHECKBOX_IRRIGATION = string.Concat(new string[]
			{
				"<b>{mass}</b> of ",
				UI.PRE_KEYWORD,
				"Liquid",
				UI.PST_KEYWORD,
				" is currently available"
			});

			// Token: 0x0400B838 RID: 47160
			public static LocString VITALS_CHECKBOX_SUBMERGED_TRUE = "This plant is fully submerged in " + UI.PRE_KEYWORD + "Liquid" + UI.PRE_KEYWORD;

			// Token: 0x0400B839 RID: 47161
			public static LocString VITALS_CHECKBOX_SUBMERGED_FALSE = "This plant must be submerged in " + UI.PRE_KEYWORD + "Liquid" + UI.PST_KEYWORD;

			// Token: 0x0400B83A RID: 47162
			public static LocString VITALS_CHECKBOX_DROWNING_TRUE = "This plant is not drowning";

			// Token: 0x0400B83B RID: 47163
			public static LocString VITALS_CHECKBOX_DROWNING_FALSE = "This plant is drowning in " + UI.PRE_KEYWORD + "Liquid" + UI.PST_KEYWORD;

			// Token: 0x0400B83C RID: 47164
			public static LocString VITALS_CHECKBOX_RECEPTACLE_OPERATIONAL = "This plant is housed in an operational farm plot";

			// Token: 0x0400B83D RID: 47165
			public static LocString VITALS_CHECKBOX_RECEPTACLE_INOPERATIONAL = "This plant is not housed in an operational farm plot";

			// Token: 0x0400B83E RID: 47166
			public static LocString VITALS_CHECKBOX_RADIATION = string.Concat(new string[]
			{
				"This plant is sitting in <b>{rads}</b> of ambient ",
				UI.PRE_KEYWORD,
				"Radiation",
				UI.PST_KEYWORD,
				". It needs at between {minRads} and {maxRads} to grow"
			});

			// Token: 0x0400B83F RID: 47167
			public static LocString VITALS_CHECKBOX_RADIATION_NO_MIN = string.Concat(new string[]
			{
				"This plant is sitting in <b>{rads}</b> of ambient ",
				UI.PRE_KEYWORD,
				"Radiation",
				UI.PST_KEYWORD,
				". It needs less than {maxRads} to grow"
			});
		}

		// Token: 0x02002CC3 RID: 11459
		public class CLUSTERMAP
		{
			// Token: 0x0400B840 RID: 47168
			public static LocString PLANETOID = "Planetoid";

			// Token: 0x0400B841 RID: 47169
			public static LocString PLANETOID_KEYWORD = UI.PRE_KEYWORD + UI.CLUSTERMAP.PLANETOID + UI.PST_KEYWORD;

			// Token: 0x0400B842 RID: 47170
			public static LocString TITLE = "STARMAP";

			// Token: 0x0400B843 RID: 47171
			public static LocString LANDING_SITES = "LANDING SITES";

			// Token: 0x0400B844 RID: 47172
			public static LocString DESTINATION = "DESTINATION";

			// Token: 0x0400B845 RID: 47173
			public static LocString OCCUPANTS = "CREW";

			// Token: 0x0400B846 RID: 47174
			public static LocString ELEMENTS = "ELEMENTS";

			// Token: 0x0400B847 RID: 47175
			public static LocString UNKNOWN_DESTINATION = "Unknown";

			// Token: 0x0400B848 RID: 47176
			public static LocString TILES = "Tiles";

			// Token: 0x0400B849 RID: 47177
			public static LocString TILES_PER_CYCLE = "Tiles per cycle";

			// Token: 0x0400B84A RID: 47178
			public static LocString CHANGE_DESTINATION = UI.CLICK(UI.ClickType.Click) + " to change destination";

			// Token: 0x0400B84B RID: 47179
			public static LocString SELECT_DESTINATION = "Select a new destination on the map";

			// Token: 0x0400B84C RID: 47180
			public static LocString TOOLTIP_INVALID_DESTINATION_FOG_OF_WAR = "Rockets cannot travel to this hex until it has been analyzed\n\nSpace can be analyzed with a " + BUILDINGS.PREFABS.CLUSTERTELESCOPE.NAME + " or " + BUILDINGS.PREFABS.SCANNERMODULE.NAME;

			// Token: 0x0400B84D RID: 47181
			public static LocString TOOLTIP_INVALID_DESTINATION_NO_PATH = string.Concat(new string[]
			{
				"There is no navigable rocket path to this ",
				UI.CLUSTERMAP.PLANETOID_KEYWORD,
				"\n\nSpace can be analyzed with a ",
				BUILDINGS.PREFABS.CLUSTERTELESCOPE.NAME,
				" or ",
				BUILDINGS.PREFABS.SCANNERMODULE.NAME,
				" to clear the way"
			});

			// Token: 0x0400B84E RID: 47182
			public static LocString TOOLTIP_INVALID_DESTINATION_NO_LAUNCH_PAD = string.Concat(new string[]
			{
				"There is no ",
				BUILDINGS.PREFABS.LAUNCHPAD.NAME,
				" on this ",
				UI.CLUSTERMAP.PLANETOID_KEYWORD,
				" for a rocket to land on\n\nUse a ",
				BUILDINGS.PREFABS.PIONEERMODULE.NAME,
				" or ",
				BUILDINGS.PREFABS.SCOUTMODULE.NAME,
				" to deploy a scout and make first contact"
			});

			// Token: 0x0400B84F RID: 47183
			public static LocString TOOLTIP_INVALID_DESTINATION_REQUIRE_ASTEROID = "Must select a " + UI.CLUSTERMAP.PLANETOID_KEYWORD + " destination";

			// Token: 0x0400B850 RID: 47184
			public static LocString TOOLTIP_INVALID_DESTINATION_OUT_OF_RANGE = "This destination is further away than the rocket's maximum range of {0}.";

			// Token: 0x0400B851 RID: 47185
			public static LocString TOOLTIP_HIDDEN_HEX = "???";

			// Token: 0x0400B852 RID: 47186
			public static LocString TOOLTIP_PEEKED_HEX_WITH_OBJECT = "UNKNOWN OBJECT DETECTED!";

			// Token: 0x0400B853 RID: 47187
			public static LocString TOOLTIP_EMPTY_HEX = "EMPTY SPACE";

			// Token: 0x0400B854 RID: 47188
			public static LocString TOOLTIP_PATH_LENGTH = "Trip Distance: {0}/{1}";

			// Token: 0x0400B855 RID: 47189
			public static LocString TOOLTIP_PATH_LENGTH_RETURN = "Trip Distance: {0}/{1} (Return Trip)";

			// Token: 0x02002CC4 RID: 11460
			public class STATUS
			{
				// Token: 0x0400B856 RID: 47190
				public static LocString NORMAL = "Normal";

				// Token: 0x02002CC5 RID: 11461
				public class ROCKET
				{
					// Token: 0x0400B857 RID: 47191
					public static LocString GROUNDED = "Normal";

					// Token: 0x0400B858 RID: 47192
					public static LocString TRAVELING = "Traveling";

					// Token: 0x0400B859 RID: 47193
					public static LocString STRANDED = "Stranded";

					// Token: 0x0400B85A RID: 47194
					public static LocString IDLE = "Idle";
				}
			}

			// Token: 0x02002CC6 RID: 11462
			public class ASTEROIDS
			{
				// Token: 0x02002CC7 RID: 11463
				public class ELEMENT_AMOUNTS
				{
					// Token: 0x0400B85B RID: 47195
					public static LocString LOTS = "Plentiful";

					// Token: 0x0400B85C RID: 47196
					public static LocString SOME = "Significant amount";

					// Token: 0x0400B85D RID: 47197
					public static LocString LITTLE = "Small amount";

					// Token: 0x0400B85E RID: 47198
					public static LocString VERY_LITTLE = "Trace amount";
				}

				// Token: 0x02002CC8 RID: 11464
				public class SURFACE_CONDITIONS
				{
					// Token: 0x0400B85F RID: 47199
					public static LocString LIGHT = "Peak Light";

					// Token: 0x0400B860 RID: 47200
					public static LocString RADIATION = "Cosmic Radiation";
				}
			}

			// Token: 0x02002CC9 RID: 11465
			public class POI
			{
				// Token: 0x0400B861 RID: 47201
				public static LocString TITLE = "POINT OF INTEREST";

				// Token: 0x0400B862 RID: 47202
				public static LocString MASS_REMAINING = "<b>Total Mass Remaining</b>";

				// Token: 0x0400B863 RID: 47203
				public static LocString ROCKETS_AT_THIS_LOCATION = "<b>Rockets at this location</b>";

				// Token: 0x0400B864 RID: 47204
				public static LocString ARTIFACTS = "Artifact";

				// Token: 0x0400B865 RID: 47205
				public static LocString ARTIFACTS_AVAILABLE = "Available";

				// Token: 0x0400B866 RID: 47206
				public static LocString ARTIFACTS_DEPLETED = "Collected\nRecharge: {0}";
			}

			// Token: 0x02002CCA RID: 11466
			public class ROCKETS
			{
				// Token: 0x02002CCB RID: 11467
				public class SPEED
				{
					// Token: 0x0400B867 RID: 47207
					public static LocString NAME = "Rocket Speed: ";

					// Token: 0x0400B868 RID: 47208
					public static LocString TOOLTIP = "<b>Rocket Speed</b> is calculated by dividing <b>Engine Power</b> by <b>Burden</b>.\n\nRockets operating on autopilot will have a reduced speed.\n\nRocket speed can be further increased by the skill of the Duplicant flying the rocket.";
				}

				// Token: 0x02002CCC RID: 11468
				public class FUEL_REMAINING
				{
					// Token: 0x0400B869 RID: 47209
					public static LocString NAME = "Fuel Remaining: ";

					// Token: 0x0400B86A RID: 47210
					public static LocString TOOLTIP = "This rocket has {0} fuel in its tank";
				}

				// Token: 0x02002CCD RID: 11469
				public class OXIDIZER_REMAINING
				{
					// Token: 0x0400B86B RID: 47211
					public static LocString NAME = "Oxidizer Power Remaining: ";

					// Token: 0x0400B86C RID: 47212
					public static LocString TOOLTIP = "This rocket has enough oxidizer in its tank for {0} of fuel";
				}

				// Token: 0x02002CCE RID: 11470
				public class RANGE
				{
					// Token: 0x0400B86D RID: 47213
					public static LocString NAME = "Range Remaining: ";

					// Token: 0x0400B86E RID: 47214
					public static LocString TOOLTIP = "<b>Range remaining</b> is calculated by dividing the lesser of <b>fuel remaining</b> and <b>oxidizer power remaining</b> by <b>fuel consumed per tile</b>";
				}

				// Token: 0x02002CCF RID: 11471
				public class FUEL_PER_HEX
				{
					// Token: 0x0400B86F RID: 47215
					public static LocString NAME = "Fuel consumed per Tile: {0}";

					// Token: 0x0400B870 RID: 47216
					public static LocString TOOLTIP = "This rocket can travel one tile per {0} of fuel";
				}

				// Token: 0x02002CD0 RID: 11472
				public class BURDEN_TOTAL
				{
					// Token: 0x0400B871 RID: 47217
					public static LocString NAME = "Rocket burden: ";

					// Token: 0x0400B872 RID: 47218
					public static LocString TOOLTIP = "The combined burden of all the modules in this rocket";
				}

				// Token: 0x02002CD1 RID: 11473
				public class BURDEN_MODULE
				{
					// Token: 0x0400B873 RID: 47219
					public static LocString NAME = "Module Burden: ";

					// Token: 0x0400B874 RID: 47220
					public static LocString TOOLTIP = "The selected module adds {0} to the rocket's total " + DUPLICANTS.ATTRIBUTES.ROCKETBURDEN.NAME;
				}

				// Token: 0x02002CD2 RID: 11474
				public class POWER_TOTAL
				{
					// Token: 0x0400B875 RID: 47221
					public static LocString NAME = "Rocket engine power: ";

					// Token: 0x0400B876 RID: 47222
					public static LocString TOOLTIP = "The total engine power added by all the modules in this rocket";
				}

				// Token: 0x02002CD3 RID: 11475
				public class POWER_MODULE
				{
					// Token: 0x0400B877 RID: 47223
					public static LocString NAME = "Module Engine Power: ";

					// Token: 0x0400B878 RID: 47224
					public static LocString TOOLTIP = "The selected module adds {0} to the rocket's total " + DUPLICANTS.ATTRIBUTES.ROCKETENGINEPOWER.NAME;
				}

				// Token: 0x02002CD4 RID: 11476
				public class MODULE_STATS
				{
					// Token: 0x0400B879 RID: 47225
					public static LocString NAME = "Module Stats: ";

					// Token: 0x0400B87A RID: 47226
					public static LocString NAME_HEADER = "Module Stats";

					// Token: 0x0400B87B RID: 47227
					public static LocString TOOLTIP = "Properties of the selected module";
				}

				// Token: 0x02002CD5 RID: 11477
				public class MAX_MODULES
				{
					// Token: 0x0400B87C RID: 47228
					public static LocString NAME = "Max Modules: ";

					// Token: 0x0400B87D RID: 47229
					public static LocString TOOLTIP = "The {0} can support {1} rocket modules, plus itself";
				}

				// Token: 0x02002CD6 RID: 11478
				public class MAX_HEIGHT
				{
					// Token: 0x0400B87E RID: 47230
					public static LocString NAME = "Height: {0}/{1}";

					// Token: 0x0400B87F RID: 47231
					public static LocString NAME_RAW = "Height: ";

					// Token: 0x0400B880 RID: 47232
					public static LocString NAME_MAX_SUPPORTED = "Maximum supported rocket height: ";

					// Token: 0x0400B881 RID: 47233
					public static LocString TOOLTIP = "The {0} can support a total rocket height {1}";
				}

				// Token: 0x02002CD7 RID: 11479
				public class ARTIFACT_MODULE
				{
					// Token: 0x0400B882 RID: 47234
					public static LocString EMPTY = "Empty";
				}
			}
		}

		// Token: 0x02002CD8 RID: 11480
		public class STARMAP
		{
			// Token: 0x0400B883 RID: 47235
			public static LocString TITLE = "STARMAP";

			// Token: 0x0400B884 RID: 47236
			public static LocString MANAGEMENT_BUTTON = "STARMAP";

			// Token: 0x0400B885 RID: 47237
			public static LocString SUBROW = "•  {0}";

			// Token: 0x0400B886 RID: 47238
			public static LocString UNKNOWN_DESTINATION = "Destination Unknown";

			// Token: 0x0400B887 RID: 47239
			public static LocString ANALYSIS_AMOUNT = "Analysis {0} Complete";

			// Token: 0x0400B888 RID: 47240
			public static LocString ANALYSIS_COMPLETE = "ANALYSIS COMPLETE";

			// Token: 0x0400B889 RID: 47241
			public static LocString NO_ANALYZABLE_DESTINATION_SELECTED = "No destination selected";

			// Token: 0x0400B88A RID: 47242
			public static LocString UNKNOWN_TYPE = "Type Unknown";

			// Token: 0x0400B88B RID: 47243
			public static LocString DISTANCE = "{0} km";

			// Token: 0x0400B88C RID: 47244
			public static LocString MODULE_MASS = "+ {0} t";

			// Token: 0x0400B88D RID: 47245
			public static LocString MODULE_STORAGE = "{0} / {1}";

			// Token: 0x0400B88E RID: 47246
			public static LocString ANALYSIS_DESCRIPTION = "Use a Telescope to analyze space destinations.\n\nCompleting analysis on an object will unlock rocket missions to that destination.";

			// Token: 0x0400B88F RID: 47247
			public static LocString RESEARCH_DESCRIPTION = "Gather Interstellar Research Data using Research Modules.";

			// Token: 0x0400B890 RID: 47248
			public static LocString ROCKET_RENAME_BUTTON_TOOLTIP = "Rename this rocket";

			// Token: 0x0400B891 RID: 47249
			public static LocString NO_ROCKETS_HELP_TEXT = "Rockets allow you to visit nearby celestial bodies.\n\nEach rocket must have a Command Module, an Engine, and Fuel.\n\nYou can also carry other modules that allow you to gather specific resources from the places you visit.\n\nRemember the more weight a rocket has, the more limited it'll be on the distance it can travel. You can add more fuel to fix that, but fuel will add weight as well.";

			// Token: 0x0400B892 RID: 47250
			public static LocString CONTAINER_REQUIRED = "{0} installation required to retrieve material";

			// Token: 0x0400B893 RID: 47251
			public static LocString CAN_CARRY_ELEMENT = "Gathered by: {1}";

			// Token: 0x0400B894 RID: 47252
			public static LocString CANT_CARRY_ELEMENT = "{0} installation required to retrieve material";

			// Token: 0x0400B895 RID: 47253
			public static LocString STATUS = "SELECTED";

			// Token: 0x0400B896 RID: 47254
			public static LocString DISTANCE_OVERLAY = "TOO FAR FOR THIS ROCKET";

			// Token: 0x0400B897 RID: 47255
			public static LocString COMPOSITION_UNDISCOVERED = "?????????";

			// Token: 0x0400B898 RID: 47256
			public static LocString COMPOSITION_UNDISCOVERED_TOOLTIP = "Further research required to identify resource\n\nSend a Research Module to this destination for more information";

			// Token: 0x0400B899 RID: 47257
			public static LocString COMPOSITION_UNDISCOVERED_AMOUNT = "???";

			// Token: 0x0400B89A RID: 47258
			public static LocString COMPOSITION_SMALL_AMOUNT = "Trace Amount";

			// Token: 0x0400B89B RID: 47259
			public static LocString CURRENT_MASS = "Current Mass";

			// Token: 0x0400B89C RID: 47260
			public static LocString CURRENT_MASS_TOOLTIP = "Warning: Missions to this destination will not return a full cargo load to avoid depleting the destination for future explorations\n\nDestination: {0} Resources Available\nRocket Capacity: {1}";

			// Token: 0x0400B89D RID: 47261
			public static LocString MAXIMUM_MASS = "Maximum Mass";

			// Token: 0x0400B89E RID: 47262
			public static LocString MINIMUM_MASS = "Minimum Mass";

			// Token: 0x0400B89F RID: 47263
			public static LocString MINIMUM_MASS_TOOLTIP = "This destination must retain at least this much mass in order to prevent depletion and allow the future regeneration of resources.\n\nDuplicants will always maintain a destination's minimum mass requirements, potentially returning with less cargo than their rocket can hold";

			// Token: 0x0400B8A0 RID: 47264
			public static LocString REPLENISH_RATE = "Replenished/Cycle:";

			// Token: 0x0400B8A1 RID: 47265
			public static LocString REPLENISH_RATE_TOOLTIP = "The rate at which this destination regenerates resources";

			// Token: 0x0400B8A2 RID: 47266
			public static LocString ROCKETLIST = "Rocket Hangar";

			// Token: 0x0400B8A3 RID: 47267
			public static LocString NO_ROCKETS_TITLE = "NO ROCKETS";

			// Token: 0x0400B8A4 RID: 47268
			public static LocString ROCKET_COUNT = "ROCKETS: {0}";

			// Token: 0x0400B8A5 RID: 47269
			public static LocString LAUNCH_MISSION = "LAUNCH MISSION";

			// Token: 0x0400B8A6 RID: 47270
			public static LocString CANT_LAUNCH_MISSION = "CANNOT LAUNCH";

			// Token: 0x0400B8A7 RID: 47271
			public static LocString LAUNCH_ROCKET = "Launch Rocket";

			// Token: 0x0400B8A8 RID: 47272
			public static LocString LAND_ROCKET = "Land Rocket";

			// Token: 0x0400B8A9 RID: 47273
			public static LocString SEE_ROCKETS_LIST = "See Rockets List";

			// Token: 0x0400B8AA RID: 47274
			public static LocString DEFAULT_NAME = "Rocket";

			// Token: 0x0400B8AB RID: 47275
			public static LocString ANALYZE_DESTINATION = "ANALYZE OBJECT";

			// Token: 0x0400B8AC RID: 47276
			public static LocString SUSPEND_DESTINATION_ANALYSIS = "PAUSE ANALYSIS";

			// Token: 0x0400B8AD RID: 47277
			public static LocString DESTINATIONTITLE = "Destination Status";

			// Token: 0x02002CD9 RID: 11481
			public class DESTINATIONSTUDY
			{
				// Token: 0x0400B8AE RID: 47278
				public static LocString UPPERATMO = "Study upper atmosphere";

				// Token: 0x0400B8AF RID: 47279
				public static LocString LOWERATMO = "Study lower atmosphere";

				// Token: 0x0400B8B0 RID: 47280
				public static LocString MAGNETICFIELD = "Study magnetic field";

				// Token: 0x0400B8B1 RID: 47281
				public static LocString SURFACE = "Study surface";

				// Token: 0x0400B8B2 RID: 47282
				public static LocString SUBSURFACE = "Study subsurface";
			}

			// Token: 0x02002CDA RID: 11482
			public class COMPONENT
			{
				// Token: 0x0400B8B3 RID: 47283
				public static LocString FUEL_TANK = "Fuel Tank";

				// Token: 0x0400B8B4 RID: 47284
				public static LocString ROCKET_ENGINE = "Rocket Engine";

				// Token: 0x0400B8B5 RID: 47285
				public static LocString CARGO_BAY = "Cargo Bay";

				// Token: 0x0400B8B6 RID: 47286
				public static LocString OXIDIZER_TANK = "Oxidizer Tank";
			}

			// Token: 0x02002CDB RID: 11483
			public class MISSION_STATUS
			{
				// Token: 0x0400B8B7 RID: 47287
				public static LocString GROUNDED = "Grounded";

				// Token: 0x0400B8B8 RID: 47288
				public static LocString LAUNCHING = "Launching";

				// Token: 0x0400B8B9 RID: 47289
				public static LocString WAITING_TO_LAND = "Waiting To Land";

				// Token: 0x0400B8BA RID: 47290
				public static LocString LANDING = "Landing";

				// Token: 0x0400B8BB RID: 47291
				public static LocString UNDERWAY = "Underway";

				// Token: 0x0400B8BC RID: 47292
				public static LocString UNDERWAY_BOOSTED = "Underway <color=#5FDB37FF>(Boosted)</color>";

				// Token: 0x0400B8BD RID: 47293
				public static LocString DESTROYED = "Destroyed";

				// Token: 0x0400B8BE RID: 47294
				public static LocString GO = "ALL SYSTEMS GO";
			}

			// Token: 0x02002CDC RID: 11484
			public class LISTTITLES
			{
				// Token: 0x0400B8BF RID: 47295
				public static LocString MISSIONSTATUS = "Mission Status";

				// Token: 0x0400B8C0 RID: 47296
				public static LocString LAUNCHCHECKLIST = "Launch Checklist";

				// Token: 0x0400B8C1 RID: 47297
				public static LocString MAXRANGE = "Max Range";

				// Token: 0x0400B8C2 RID: 47298
				public static LocString MASS = "Mass";

				// Token: 0x0400B8C3 RID: 47299
				public static LocString STORAGE = "Storage";

				// Token: 0x0400B8C4 RID: 47300
				public static LocString FUEL = "Fuel";

				// Token: 0x0400B8C5 RID: 47301
				public static LocString OXIDIZER = "Oxidizer";

				// Token: 0x0400B8C6 RID: 47302
				public static LocString PASSENGERS = "Passengers";

				// Token: 0x0400B8C7 RID: 47303
				public static LocString RESEARCH = "Research";

				// Token: 0x0400B8C8 RID: 47304
				public static LocString ARTIFACTS = "Artifacts";

				// Token: 0x0400B8C9 RID: 47305
				public static LocString ANALYSIS = "Analysis";

				// Token: 0x0400B8CA RID: 47306
				public static LocString WORLDCOMPOSITION = "World Composition";

				// Token: 0x0400B8CB RID: 47307
				public static LocString RESOURCES = "Resources";

				// Token: 0x0400B8CC RID: 47308
				public static LocString MODULES = "Modules";

				// Token: 0x0400B8CD RID: 47309
				public static LocString TYPE = "Type";

				// Token: 0x0400B8CE RID: 47310
				public static LocString DISTANCE = "Distance";

				// Token: 0x0400B8CF RID: 47311
				public static LocString DESTINATION_MASS = "World Mass Available";

				// Token: 0x0400B8D0 RID: 47312
				public static LocString STORAGECAPACITY = "Storage Capacity";
			}

			// Token: 0x02002CDD RID: 11485
			public class ROCKETWEIGHT
			{
				// Token: 0x0400B8D1 RID: 47313
				public static LocString MASS = "Mass: ";

				// Token: 0x0400B8D2 RID: 47314
				public static LocString MASSPENALTY = "Mass Penalty: ";

				// Token: 0x0400B8D3 RID: 47315
				public static LocString CURRENTMASS = "Current Rocket Mass: ";

				// Token: 0x0400B8D4 RID: 47316
				public static LocString CURRENTMASSPENALTY = "Current Weight Penalty: ";
			}

			// Token: 0x02002CDE RID: 11486
			public class DESTINATIONSELECTION
			{
				// Token: 0x0400B8D5 RID: 47317
				public static LocString REACHABLE = "Destination set";

				// Token: 0x0400B8D6 RID: 47318
				public static LocString UNREACHABLE = "Destination set";

				// Token: 0x0400B8D7 RID: 47319
				public static LocString NOTSELECTED = "Destination set";
			}

			// Token: 0x02002CDF RID: 11487
			public class DESTINATIONSELECTION_TOOLTIP
			{
				// Token: 0x0400B8D8 RID: 47320
				public static LocString REACHABLE = "Viable destination selected, ready for launch";

				// Token: 0x0400B8D9 RID: 47321
				public static LocString UNREACHABLE = "The selected destination is beyond rocket reach";

				// Token: 0x0400B8DA RID: 47322
				public static LocString NOTSELECTED = "Select the rocket's Command Module to set a destination";
			}

			// Token: 0x02002CE0 RID: 11488
			public class HASFOOD
			{
				// Token: 0x0400B8DB RID: 47323
				public static LocString NAME = "Food Loaded";

				// Token: 0x0400B8DC RID: 47324
				public static LocString TOOLTIP = "Sufficient food stores have been loaded, ready for launch";
			}

			// Token: 0x02002CE1 RID: 11489
			public class HASSUIT
			{
				// Token: 0x0400B8DD RID: 47325
				public static LocString NAME = "Has " + EQUIPMENT.PREFABS.ATMO_SUIT.NAME;

				// Token: 0x0400B8DE RID: 47326
				public static LocString TOOLTIP = "An " + EQUIPMENT.PREFABS.ATMO_SUIT.NAME + " has been loaded";
			}

			// Token: 0x02002CE2 RID: 11490
			public class NOSUIT
			{
				// Token: 0x0400B8DF RID: 47327
				public static LocString NAME = "Missing " + EQUIPMENT.PREFABS.ATMO_SUIT.NAME;

				// Token: 0x0400B8E0 RID: 47328
				public static LocString TOOLTIP = "Rocket cannot launch without an " + EQUIPMENT.PREFABS.ATMO_SUIT.NAME + " loaded";
			}

			// Token: 0x02002CE3 RID: 11491
			public class NOFOOD
			{
				// Token: 0x0400B8E1 RID: 47329
				public static LocString NAME = "Insufficient Food";

				// Token: 0x0400B8E2 RID: 47330
				public static LocString TOOLTIP = "Rocket cannot launch without adequate food stores for passengers";
			}

			// Token: 0x02002CE4 RID: 11492
			public class CARGOEMPTY
			{
				// Token: 0x0400B8E3 RID: 47331
				public static LocString NAME = "Emptied Cargo Bay";

				// Token: 0x0400B8E4 RID: 47332
				public static LocString TOOLTIP = "Cargo Bays must be emptied of all materials before launch";
			}

			// Token: 0x02002CE5 RID: 11493
			public class LAUNCHCHECKLIST
			{
				// Token: 0x0400B8E5 RID: 47333
				public static LocString ASTRONAUT_TITLE = "Astronaut";

				// Token: 0x0400B8E6 RID: 47334
				public static LocString HASASTRONAUT = "Astronaut ready for liftoff";

				// Token: 0x0400B8E7 RID: 47335
				public static LocString ASTRONAUGHT = "No Astronaut assigned";

				// Token: 0x0400B8E8 RID: 47336
				public static LocString INSTALLED = "Installed";

				// Token: 0x0400B8E9 RID: 47337
				public static LocString INSTALLED_TOOLTIP = "A suitable {0} has been installed";

				// Token: 0x0400B8EA RID: 47338
				public static LocString REQUIRED = "Required";

				// Token: 0x0400B8EB RID: 47339
				public static LocString REQUIRED_TOOLTIP = "A {0} must be installed before launch";

				// Token: 0x0400B8EC RID: 47340
				public static LocString MISSING_TOOLTIP = "No {0} installed\n\nThis rocket cannot launch without a completed {0}";

				// Token: 0x0400B8ED RID: 47341
				public static LocString NO_DESTINATION = "No destination selected";

				// Token: 0x0400B8EE RID: 47342
				public static LocString MINIMUM_MASS = "Resources available {0}";

				// Token: 0x0400B8EF RID: 47343
				public static LocString RESOURCE_MASS_TOOLTIP = "{0} has {1} resources available\nThis rocket has capacity for {2}";

				// Token: 0x0400B8F0 RID: 47344
				public static LocString INSUFFICENT_MASS_TOOLTIP = "Launching to this destination will not return a full cargo load";

				// Token: 0x02002CE6 RID: 11494
				public class CONSTRUCTION_COMPLETE
				{
					// Token: 0x02002CE7 RID: 11495
					public class STATUS
					{
						// Token: 0x0400B8F1 RID: 47345
						public static LocString READY = "No active construction";

						// Token: 0x0400B8F2 RID: 47346
						public static LocString FAILURE = "No active construction";

						// Token: 0x0400B8F3 RID: 47347
						public static LocString WARNING = "No active construction";
					}

					// Token: 0x02002CE8 RID: 11496
					public class TOOLTIP
					{
						// Token: 0x0400B8F4 RID: 47348
						public static LocString READY = "Construction of all modules is complete";

						// Token: 0x0400B8F5 RID: 47349
						public static LocString FAILURE = "In-progress module construction is preventing takeoff";

						// Token: 0x0400B8F6 RID: 47350
						public static LocString WARNING = "Construction warning";
					}
				}

				// Token: 0x02002CE9 RID: 11497
				public class PILOT_BOARDED
				{
					// Token: 0x0400B8F7 RID: 47351
					public static LocString READY = "Pilot boarded";

					// Token: 0x0400B8F8 RID: 47352
					public static LocString FAILURE = "Pilot boarded";

					// Token: 0x0400B8F9 RID: 47353
					public static LocString WARNING = "Pilot boarded";

					// Token: 0x0400B8FA RID: 47354
					public static LocString ROBO_PILOT_WARNING = "Robo-Pilot solo flight";

					// Token: 0x02002CEA RID: 11498
					public class TOOLTIP
					{
						// Token: 0x0400B8FB RID: 47355
						public static LocString READY = "A Duplicant with the " + DUPLICANTS.ROLES.ROCKETPILOT.NAME + " skill is currently onboard";

						// Token: 0x0400B8FC RID: 47356
						public static LocString FAILURE = "At least one crew member aboard the rocket must possess the " + DUPLICANTS.ROLES.ROCKETPILOT.NAME + " skill to launch\n\nQualified Duplicants must be assigned to the rocket crew, and have access to the module's hatch";

						// Token: 0x0400B8FD RID: 47357
						public static LocString WARNING = "Pilot warning";

						// Token: 0x0400B8FE RID: 47358
						public static LocString ROBO_PILOT_WARNING = "This rocket is being piloted by a Robo-Pilot\n\nThere are no Duplicants with the " + DUPLICANTS.ROLES.ROCKETPILOT.NAME + " skill currently onboard";
					}
				}

				// Token: 0x02002CEB RID: 11499
				public class CREW_BOARDED
				{
					// Token: 0x0400B8FF RID: 47359
					public static LocString READY = "All crew boarded";

					// Token: 0x0400B900 RID: 47360
					public static LocString FAILURE = "All crew boarded";

					// Token: 0x0400B901 RID: 47361
					public static LocString WARNING = "All crew boarded";

					// Token: 0x02002CEC RID: 11500
					public class TOOLTIP
					{
						// Token: 0x0400B902 RID: 47362
						public static LocString READY = "All Duplicants assigned to the rocket crew are boarded and ready for launch\n\n    • {0}/{1} Boarded";

						// Token: 0x0400B903 RID: 47363
						public static LocString FAILURE = "No crew members have boarded this rocket\n\nDuplicants must be assigned to the rocket crew and have access to the module's hatch to board\n\n    • {0}/{1} Boarded";

						// Token: 0x0400B904 RID: 47364
						public static LocString WARNING = "Some Duplicants assigned to this rocket crew have not yet boarded\n    • {0}/{1} Boarded";

						// Token: 0x0400B905 RID: 47365
						public static LocString NONE = "There are no Duplicants assigned to this rocket crew\n    • {0}/{1} Boarded";
					}
				}

				// Token: 0x02002CED RID: 11501
				public class NO_EXTRA_PASSENGERS
				{
					// Token: 0x0400B906 RID: 47366
					public static LocString READY = "Non-crew exited";

					// Token: 0x0400B907 RID: 47367
					public static LocString FAILURE = "Non-crew exited";

					// Token: 0x0400B908 RID: 47368
					public static LocString WARNING = "Non-crew exited";

					// Token: 0x02002CEE RID: 11502
					public class TOOLTIP
					{
						// Token: 0x0400B909 RID: 47369
						public static LocString READY = "All non-crew Duplicants have disembarked";

						// Token: 0x0400B90A RID: 47370
						public static LocString FAILURE = "Non-crew Duplicants must exit the rocket before launch";

						// Token: 0x0400B90B RID: 47371
						public static LocString WARNING = "Non-crew warning";
					}
				}

				// Token: 0x02002CEF RID: 11503
				public class FLIGHT_PATH_CLEAR
				{
					// Token: 0x02002CF0 RID: 11504
					public class STATUS
					{
						// Token: 0x0400B90C RID: 47372
						public static LocString READY = "Clear launch path";

						// Token: 0x0400B90D RID: 47373
						public static LocString FAILURE = "Clear launch path";

						// Token: 0x0400B90E RID: 47374
						public static LocString WARNING = "Clear launch path";
					}

					// Token: 0x02002CF1 RID: 11505
					public class TOOLTIP
					{
						// Token: 0x0400B90F RID: 47375
						public static LocString READY = "The rocket's launch path is clear for takeoff";

						// Token: 0x0400B910 RID: 47376
						public static LocString FAILURE = "This rocket does not have a clear line of sight to space, preventing launch\n\nThe rocket's launch path can be cleared by excavating undug tiles and deconstructing any buildings above the rocket";

						// Token: 0x0400B911 RID: 47377
						public static LocString WARNING = "";
					}
				}

				// Token: 0x02002CF2 RID: 11506
				public class HAS_FUEL_TANK
				{
					// Token: 0x02002CF3 RID: 11507
					public class STATUS
					{
						// Token: 0x0400B912 RID: 47378
						public static LocString READY = "Fuel Tank";

						// Token: 0x0400B913 RID: 47379
						public static LocString FAILURE = "Fuel Tank";

						// Token: 0x0400B914 RID: 47380
						public static LocString WARNING = "Fuel Tank";
					}

					// Token: 0x02002CF4 RID: 11508
					public class TOOLTIP
					{
						// Token: 0x0400B915 RID: 47381
						public static LocString READY = "A fuel tank has been installed";

						// Token: 0x0400B916 RID: 47382
						public static LocString FAILURE = "No fuel tank installed\n\nThis rocket cannot launch without a completed fuel tank";

						// Token: 0x0400B917 RID: 47383
						public static LocString WARNING = "Fuel tank warning";
					}
				}

				// Token: 0x02002CF5 RID: 11509
				public class HAS_ENGINE
				{
					// Token: 0x02002CF6 RID: 11510
					public class STATUS
					{
						// Token: 0x0400B918 RID: 47384
						public static LocString READY = "Engine";

						// Token: 0x0400B919 RID: 47385
						public static LocString FAILURE = "Engine";

						// Token: 0x0400B91A RID: 47386
						public static LocString WARNING = "Engine";
					}

					// Token: 0x02002CF7 RID: 11511
					public class TOOLTIP
					{
						// Token: 0x0400B91B RID: 47387
						public static LocString READY = "A suitable engine has been installed";

						// Token: 0x0400B91C RID: 47388
						public static LocString FAILURE = "No engine installed\n\nThis rocket cannot launch without a completed engine";

						// Token: 0x0400B91D RID: 47389
						public static LocString WARNING = "Engine warning";
					}
				}

				// Token: 0x02002CF8 RID: 11512
				public class HAS_NOSECONE
				{
					// Token: 0x02002CF9 RID: 11513
					public class STATUS
					{
						// Token: 0x0400B91E RID: 47390
						public static LocString READY = "Nosecone";

						// Token: 0x0400B91F RID: 47391
						public static LocString FAILURE = "Nosecone";

						// Token: 0x0400B920 RID: 47392
						public static LocString WARNING = "Nosecone";
					}

					// Token: 0x02002CFA RID: 11514
					public class TOOLTIP
					{
						// Token: 0x0400B921 RID: 47393
						public static LocString READY = "A suitable nosecone has been installed";

						// Token: 0x0400B922 RID: 47394
						public static LocString FAILURE = "No nosecone installed\n\nThis rocket cannot launch without a completed nosecone";

						// Token: 0x0400B923 RID: 47395
						public static LocString WARNING = "Nosecone warning";
					}
				}

				// Token: 0x02002CFB RID: 11515
				public class HAS_CARGO_BAY_FOR_NOSECONE_HARVEST
				{
					// Token: 0x02002CFC RID: 11516
					public class STATUS
					{
						// Token: 0x0400B924 RID: 47396
						public static LocString READY = "Drillcone Cargo Bay";

						// Token: 0x0400B925 RID: 47397
						public static LocString FAILURE = "Drillcone Cargo Bay";

						// Token: 0x0400B926 RID: 47398
						public static LocString WARNING = "Drillcone Cargo Bay";
					}

					// Token: 0x02002CFD RID: 11517
					public class TOOLTIP
					{
						// Token: 0x0400B927 RID: 47399
						public static LocString READY = "A suitable cargo bay has been installed";

						// Token: 0x0400B928 RID: 47400
						public static LocString FAILURE = "No cargo bay installed\n\nThis rocket has a " + UI.FormatAsLink("Drillcone", "NOSECONEHARVEST") + " installed but nowhere to store the materials";

						// Token: 0x0400B929 RID: 47401
						public static LocString WARNING = "No cargo bay installed\n\nThis rocket has a " + UI.FormatAsLink("Drillcone", "NOSECONEHARVEST") + " installed but nowhere to store the materials";
					}
				}

				// Token: 0x02002CFE RID: 11518
				public class HAS_CONTROLSTATION
				{
					// Token: 0x02002CFF RID: 11519
					public class STATUS
					{
						// Token: 0x0400B92A RID: 47402
						public static LocString READY = "Control Station";

						// Token: 0x0400B92B RID: 47403
						public static LocString FAILURE = "Control Station";

						// Token: 0x0400B92C RID: 47404
						public static LocString WARNING = "Control Station";
					}

					// Token: 0x02002D00 RID: 11520
					public class TOOLTIP
					{
						// Token: 0x0400B92D RID: 47405
						public static LocString READY = "The control station is installed and waiting for the pilot";

						// Token: 0x0400B92E RID: 47406
						public static LocString FAILURE = "No Control Station\n\nA new Rocket Control Station must be installed inside the rocket";

						// Token: 0x0400B92F RID: 47407
						public static LocString WARNING = "Control Station warning";
					}
				}

				// Token: 0x02002D01 RID: 11521
				public class LOADING_COMPLETE
				{
					// Token: 0x02002D02 RID: 11522
					public class STATUS
					{
						// Token: 0x0400B930 RID: 47408
						public static LocString READY = "Cargo Loading Complete";

						// Token: 0x0400B931 RID: 47409
						public static LocString FAILURE = "";

						// Token: 0x0400B932 RID: 47410
						public static LocString WARNING = "Cargo Loading Complete";
					}

					// Token: 0x02002D03 RID: 11523
					public class TOOLTIP
					{
						// Token: 0x0400B933 RID: 47411
						public static LocString READY = "All possible loading and unloading has been completed";

						// Token: 0x0400B934 RID: 47412
						public static LocString FAILURE = "";

						// Token: 0x0400B935 RID: 47413
						public static LocString WARNING = "The " + BUILDINGS.PREFABS.LAUNCHPAD.NAME + " could still transfer cargo to or from this rocket";
					}
				}

				// Token: 0x02002D04 RID: 11524
				public class CARGO_TRANSFER_COMPLETE
				{
					// Token: 0x02002D05 RID: 11525
					public class STATUS
					{
						// Token: 0x0400B936 RID: 47414
						public static LocString READY = "Cargo Transfer Complete";

						// Token: 0x0400B937 RID: 47415
						public static LocString FAILURE = "";

						// Token: 0x0400B938 RID: 47416
						public static LocString WARNING = "Cargo Transfer Complete";
					}

					// Token: 0x02002D06 RID: 11526
					public class TOOLTIP
					{
						// Token: 0x0400B939 RID: 47417
						public static LocString READY = "All possible loading and unloading has been completed";

						// Token: 0x0400B93A RID: 47418
						public static LocString FAILURE = "";

						// Token: 0x0400B93B RID: 47419
						public static LocString WARNING = "The " + BUILDINGS.PREFABS.LAUNCHPAD.NAME + " could still transfer cargo to or from this rocket";
					}
				}

				// Token: 0x02002D07 RID: 11527
				public class INTERNAL_CONSTRUCTION_COMPLETE
				{
					// Token: 0x02002D08 RID: 11528
					public class STATUS
					{
						// Token: 0x0400B93C RID: 47420
						public static LocString READY = "Landers Ready";

						// Token: 0x0400B93D RID: 47421
						public static LocString FAILURE = "Landers Ready";

						// Token: 0x0400B93E RID: 47422
						public static LocString WARNING = "";
					}

					// Token: 0x02002D09 RID: 11529
					public class TOOLTIP
					{
						// Token: 0x0400B93F RID: 47423
						public static LocString READY = "All requested landers have been built and are ready for deployment";

						// Token: 0x0400B940 RID: 47424
						public static LocString FAILURE = "Additional landers must be constructed to fulfill the lander requests of this rocket";

						// Token: 0x0400B941 RID: 47425
						public static LocString WARNING = "";
					}
				}

				// Token: 0x02002D0A RID: 11530
				public class MAX_MODULES
				{
					// Token: 0x02002D0B RID: 11531
					public class STATUS
					{
						// Token: 0x0400B942 RID: 47426
						public static LocString READY = "Module limit";

						// Token: 0x0400B943 RID: 47427
						public static LocString FAILURE = "Module limit";

						// Token: 0x0400B944 RID: 47428
						public static LocString WARNING = "Module limit";
					}

					// Token: 0x02002D0C RID: 11532
					public class TOOLTIP
					{
						// Token: 0x0400B945 RID: 47429
						public static LocString READY = "The rocket's engine can support the number of installed rocket modules";

						// Token: 0x0400B946 RID: 47430
						public static LocString FAILURE = "The number of installed modules exceeds the engine's module limit\n\nExcess modules must be removed";

						// Token: 0x0400B947 RID: 47431
						public static LocString WARNING = "Module limit warning";
					}
				}

				// Token: 0x02002D0D RID: 11533
				public class HAS_RESOURCE
				{
					// Token: 0x02002D0E RID: 11534
					public class STATUS
					{
						// Token: 0x0400B948 RID: 47432
						public static LocString READY = "{0} {1} supplied";

						// Token: 0x0400B949 RID: 47433
						public static LocString FAILURE = "{0} missing {1}";

						// Token: 0x0400B94A RID: 47434
						public static LocString WARNING = "{0} missing {1}";
					}

					// Token: 0x02002D0F RID: 11535
					public class TOOLTIP
					{
						// Token: 0x0400B94B RID: 47435
						public static LocString READY = "{0} {1} supplied";

						// Token: 0x0400B94C RID: 47436
						public static LocString FAILURE = "{0} has less than {1} {2}";

						// Token: 0x0400B94D RID: 47437
						public static LocString WARNING = "{0} has less than {1} {2}";
					}
				}

				// Token: 0x02002D10 RID: 11536
				public class MAX_HEIGHT
				{
					// Token: 0x02002D11 RID: 11537
					public class STATUS
					{
						// Token: 0x0400B94E RID: 47438
						public static LocString READY = "Height limit";

						// Token: 0x0400B94F RID: 47439
						public static LocString FAILURE = "Height limit";

						// Token: 0x0400B950 RID: 47440
						public static LocString WARNING = "Height limit";
					}

					// Token: 0x02002D12 RID: 11538
					public class TOOLTIP
					{
						// Token: 0x0400B951 RID: 47441
						public static LocString READY = "The rocket's engine can support the height of the rocket";

						// Token: 0x0400B952 RID: 47442
						public static LocString FAILURE = "The height of the rocket exceeds the engine's limit\n\nExcess modules must be removed";

						// Token: 0x0400B953 RID: 47443
						public static LocString WARNING = "Height limit warning";
					}
				}

				// Token: 0x02002D13 RID: 11539
				public class PROPERLY_FUELED
				{
					// Token: 0x02002D14 RID: 11540
					public class STATUS
					{
						// Token: 0x0400B954 RID: 47444
						public static LocString READY = "Fueled";

						// Token: 0x0400B955 RID: 47445
						public static LocString FAILURE = "Fueled";

						// Token: 0x0400B956 RID: 47446
						public static LocString WARNING = "Fueled";
					}

					// Token: 0x02002D15 RID: 11541
					public class TOOLTIP
					{
						// Token: 0x0400B957 RID: 47447
						public static LocString READY = "The rocket is sufficiently fueled for a roundtrip to its destination and back";

						// Token: 0x0400B958 RID: 47448
						public static LocString READY_NO_DESTINATION = "This rocket's fuel tanks have been filled to capacity, but it has no destination";

						// Token: 0x0400B959 RID: 47449
						public static LocString FAILURE = "This rocket does not have enough fuel to reach its destination\n\nIf the tanks are full, a different Fuel Tank Module may be required";

						// Token: 0x0400B95A RID: 47450
						public static LocString WARNING = "The rocket has enough fuel for a one-way trip to its destination, but will not be able to make it back";
					}
				}

				// Token: 0x02002D16 RID: 11542
				public class SUFFICIENT_OXIDIZER
				{
					// Token: 0x02002D17 RID: 11543
					public class STATUS
					{
						// Token: 0x0400B95B RID: 47451
						public static LocString READY = "Sufficient Oxidizer";

						// Token: 0x0400B95C RID: 47452
						public static LocString FAILURE = "Sufficient Oxidizer";

						// Token: 0x0400B95D RID: 47453
						public static LocString WARNING = "Warning: Limited oxidizer";
					}

					// Token: 0x02002D18 RID: 11544
					public class TOOLTIP
					{
						// Token: 0x0400B95E RID: 47454
						public static LocString READY = "This rocket has sufficient oxidizer for a roundtrip to its destination and back";

						// Token: 0x0400B95F RID: 47455
						public static LocString FAILURE = "This rocket does not have enough oxidizer to reach its destination\n\nIf the oxidizer tanks are full, a different Oxidizer Tank Module may be required";

						// Token: 0x0400B960 RID: 47456
						public static LocString WARNING = "The rocket has enough oxidizer for a one-way trip to its destination, but will not be able to make it back";
					}
				}

				// Token: 0x02002D19 RID: 11545
				public class ON_LAUNCHPAD
				{
					// Token: 0x02002D1A RID: 11546
					public class STATUS
					{
						// Token: 0x0400B961 RID: 47457
						public static LocString READY = "On a launch pad";

						// Token: 0x0400B962 RID: 47458
						public static LocString FAILURE = "Not on a launch pad";

						// Token: 0x0400B963 RID: 47459
						public static LocString WARNING = "No launch pad";
					}

					// Token: 0x02002D1B RID: 11547
					public class TOOLTIP
					{
						// Token: 0x0400B964 RID: 47460
						public static LocString READY = "On a launch pad";

						// Token: 0x0400B965 RID: 47461
						public static LocString FAILURE = "Not on a launch pad";

						// Token: 0x0400B966 RID: 47462
						public static LocString WARNING = "No launch pad";
					}
				}

				// Token: 0x02002D1C RID: 11548
				public class ROBOT_PILOT_DATA_REQUIREMENTS
				{
					// Token: 0x02002D1D RID: 11549
					public class STATUS
					{
						// Token: 0x0400B967 RID: 47463
						public static LocString WARNING_NO_DATA_BANKS_HUMAN_PILOT = "Robo-Pilot programmed";

						// Token: 0x0400B968 RID: 47464
						public static LocString READY = "Robo-Pilot programmed";

						// Token: 0x0400B969 RID: 47465
						public static LocString FAILURE = "Robo-Pilot programmed";

						// Token: 0x0400B96A RID: 47466
						public static LocString WARNING = "Robo-Pilot programmed";
					}

					// Token: 0x02002D1E RID: 11550
					public class TOOLTIP
					{
						// Token: 0x0400B96B RID: 47467
						public static LocString READY = string.Concat(new string[]
						{
							UI.PRE_KEYWORD,
							"Robo-Pilot",
							UI.PST_KEYWORD,
							" has sufficient ",
							UI.PRE_KEYWORD,
							"Data Banks",
							UI.PST_KEYWORD,
							" to reach its destination"
						});

						// Token: 0x0400B96C RID: 47468
						public static LocString READY_NO_DESTINATION = string.Concat(new string[]
						{
							UI.PRE_KEYWORD,
							"Robo-Pilot",
							UI.PST_KEYWORD,
							" has sufficient ",
							UI.PRE_KEYWORD,
							"Data Banks",
							UI.PST_KEYWORD,
							", but no destination has been set"
						});

						// Token: 0x0400B96D RID: 47469
						public static LocString FAILURE = string.Concat(new string[]
						{
							UI.PRE_KEYWORD,
							"Robo-Pilot",
							UI.PST_KEYWORD,
							" requires at least one ",
							UI.PRE_KEYWORD,
							"Data Bank",
							UI.PST_KEYWORD,
							" for launch"
						});

						// Token: 0x0400B96E RID: 47470
						public static LocString WARNING = string.Concat(new string[]
						{
							UI.PRE_KEYWORD,
							"Robo-Pilot",
							UI.PST_KEYWORD,
							" has insufficient ",
							UI.PRE_KEYWORD,
							"Data Banks",
							UI.PST_KEYWORD,
							" to reach its destination\n\nTravel speed will be reduced"
						});

						// Token: 0x0400B96F RID: 47471
						public static LocString WARNING_NO_DATA_BANKS_HUMAN_PILOT = string.Concat(new string[]
						{
							UI.PRE_KEYWORD,
							"Robo-Pilot",
							UI.PST_KEYWORD,
							" requires ",
							UI.PRE_KEYWORD,
							"Data Banks",
							UI.PST_KEYWORD,
							" to function\n\nThis rocket is currently operated by a Duplicant who possesses the ",
							DUPLICANTS.ROLES.ROCKETPILOT.NAME,
							" skill"
						});
					}
				}

				// Token: 0x02002D1F RID: 11551
				public class ROBOT_PILOT_POWER_SOUCRE
				{
					// Token: 0x02002D20 RID: 11552
					public class STATUS
					{
						// Token: 0x0400B970 RID: 47472
						public static LocString READY = "Robo-Pilot has power";

						// Token: 0x0400B971 RID: 47473
						public static LocString WARNING = "Robo-Pilot has power";

						// Token: 0x0400B972 RID: 47474
						public static LocString FAILURE = "Robo-Pilot has power";
					}

					// Token: 0x02002D21 RID: 11553
					public class TOOLTIP
					{
						// Token: 0x0400B973 RID: 47475
						public static LocString READY = string.Concat(new string[]
						{
							UI.PRE_KEYWORD,
							"Robo-Pilot",
							UI.PST_KEYWORD,
							" has a ",
							UI.PRE_KEYWORD,
							"Power",
							UI.PST_KEYWORD,
							" source"
						});

						// Token: 0x0400B974 RID: 47476
						public static LocString WARNING = string.Concat(new string[]
						{
							UI.PRE_KEYWORD,
							"Robo-Pilot",
							UI.PST_KEYWORD,
							" has insufficient  ",
							UI.PRE_KEYWORD,
							"Power",
							UI.PST_KEYWORD,
							" for a round-trip to its destination"
						});

						// Token: 0x0400B975 RID: 47477
						public static LocString FAILURE = string.Concat(new string[]
						{
							UI.PRE_KEYWORD,
							"Robo-Pilot",
							UI.PST_KEYWORD,
							" requires a ",
							UI.PRE_KEYWORD,
							"Power",
							UI.PST_KEYWORD,
							" source for launch"
						});
					}
				}
			}

			// Token: 0x02002D22 RID: 11554
			public class FULLTANK
			{
				// Token: 0x0400B976 RID: 47478
				public static LocString NAME = "Fuel Tank full";

				// Token: 0x0400B977 RID: 47479
				public static LocString TOOLTIP = "Tank is full, ready for launch";
			}

			// Token: 0x02002D23 RID: 11555
			public class EMPTYTANK
			{
				// Token: 0x0400B978 RID: 47480
				public static LocString NAME = "Fuel Tank not full";

				// Token: 0x0400B979 RID: 47481
				public static LocString TOOLTIP = "Fuel tank must be filled before launch";
			}

			// Token: 0x02002D24 RID: 11556
			public class FULLOXIDIZERTANK
			{
				// Token: 0x0400B97A RID: 47482
				public static LocString NAME = "Oxidizer Tank full";

				// Token: 0x0400B97B RID: 47483
				public static LocString TOOLTIP = "Tank is full, ready for launch";
			}

			// Token: 0x02002D25 RID: 11557
			public class EMPTYOXIDIZERTANK
			{
				// Token: 0x0400B97C RID: 47484
				public static LocString NAME = "Oxidizer Tank not full";

				// Token: 0x0400B97D RID: 47485
				public static LocString TOOLTIP = "Oxidizer tank must be filled before launch";
			}

			// Token: 0x02002D26 RID: 11558
			public class ROCKETSTATUS
			{
				// Token: 0x0400B97E RID: 47486
				public static LocString STATUS_TITLE = "Rocket Status";

				// Token: 0x0400B97F RID: 47487
				public static LocString NONE = "NONE";

				// Token: 0x0400B980 RID: 47488
				public static LocString SELECTED = "SELECTED";

				// Token: 0x0400B981 RID: 47489
				public static LocString LOCKEDIN = "LOCKED IN";

				// Token: 0x0400B982 RID: 47490
				public static LocString NODESTINATION = "No destination selected";

				// Token: 0x0400B983 RID: 47491
				public static LocString DESTINATIONVALUE = "None";

				// Token: 0x0400B984 RID: 47492
				public static LocString NOPASSENGERS = "No passengers";

				// Token: 0x0400B985 RID: 47493
				public static LocString STATUS = "Status";

				// Token: 0x0400B986 RID: 47494
				public static LocString TOTAL = "Total";

				// Token: 0x0400B987 RID: 47495
				public static LocString WEIGHTPENALTY = "Weight Penalty";

				// Token: 0x0400B988 RID: 47496
				public static LocString TIMEREMAINING = "Time Remaining";

				// Token: 0x0400B989 RID: 47497
				public static LocString BOOSTED_TIME_MODIFIER = "Less Than ";
			}

			// Token: 0x02002D27 RID: 11559
			public class ROCKETSTATS
			{
				// Token: 0x0400B98A RID: 47498
				public static LocString TOTAL_OXIDIZABLE_FUEL = "Total oxidizable fuel";

				// Token: 0x0400B98B RID: 47499
				public static LocString TOTAL_OXIDIZER = "Total oxidizer";

				// Token: 0x0400B98C RID: 47500
				public static LocString TOTAL_FUEL = "Total fuel";

				// Token: 0x0400B98D RID: 47501
				public static LocString NO_ENGINE = "NO ENGINE";

				// Token: 0x0400B98E RID: 47502
				public static LocString ENGINE_EFFICIENCY = "Main engine efficiency";

				// Token: 0x0400B98F RID: 47503
				public static LocString OXIDIZER_EFFICIENCY = "Average oxidizer efficiency";

				// Token: 0x0400B990 RID: 47504
				public static LocString SOLID_BOOSTER = "Solid boosters";

				// Token: 0x0400B991 RID: 47505
				public static LocString TOTAL_THRUST = "Total thrust";

				// Token: 0x0400B992 RID: 47506
				public static LocString TOTAL_RANGE = "Total range";

				// Token: 0x0400B993 RID: 47507
				public static LocString DRY_MASS = "Dry mass";

				// Token: 0x0400B994 RID: 47508
				public static LocString WET_MASS = "Wet mass";
			}

			// Token: 0x02002D28 RID: 11560
			public class STORAGESTATS
			{
				// Token: 0x0400B995 RID: 47509
				public static LocString STORAGECAPACITY = "{0} / {1}";
			}
		}

		// Token: 0x02002D29 RID: 11561
		public class RESEARCHSCREEN
		{
			// Token: 0x02002D2A RID: 11562
			public class FILTER_BUTTONS
			{
				// Token: 0x0400B996 RID: 47510
				public static LocString HEADER = "Preset Filters";

				// Token: 0x0400B997 RID: 47511
				public static LocString ALL = "All";

				// Token: 0x0400B998 RID: 47512
				public static LocString AVAILABLE = "Next";

				// Token: 0x0400B999 RID: 47513
				public static LocString COMPLETED = "Completed";

				// Token: 0x0400B99A RID: 47514
				public static LocString OXYGEN = "Oxygen";

				// Token: 0x0400B99B RID: 47515
				public static LocString FOOD = "Food";

				// Token: 0x0400B99C RID: 47516
				public static LocString WATER = "Water";

				// Token: 0x0400B99D RID: 47517
				public static LocString POWER = "Power";

				// Token: 0x0400B99E RID: 47518
				public static LocString MORALE = "Morale";

				// Token: 0x0400B99F RID: 47519
				public static LocString RANCHING = "Ranching";

				// Token: 0x0400B9A0 RID: 47520
				public static LocString FILTER = "Filters";

				// Token: 0x0400B9A1 RID: 47521
				public static LocString TILE = "Tiles";

				// Token: 0x0400B9A2 RID: 47522
				public static LocString TRANSPORT = "Transport";

				// Token: 0x0400B9A3 RID: 47523
				public static LocString AUTOMATION = "Automation";

				// Token: 0x0400B9A4 RID: 47524
				public static LocString MEDICINE = "Medicine";

				// Token: 0x0400B9A5 RID: 47525
				public static LocString ROCKET = "Rockets";

				// Token: 0x0400B9A6 RID: 47526
				public static LocString RADIATION = "Radiation";
			}
		}

		// Token: 0x02002D2B RID: 11563
		public class CODEX
		{
			// Token: 0x0400B9A7 RID: 47527
			public static LocString SEARCH_HEADER = "Search Database";

			// Token: 0x0400B9A8 RID: 47528
			public static LocString BACK_BUTTON = "Back ({0})";

			// Token: 0x0400B9A9 RID: 47529
			public static LocString TIPS = "Tips";

			// Token: 0x0400B9AA RID: 47530
			public static LocString GAME_SYSTEMS = "Systems";

			// Token: 0x0400B9AB RID: 47531
			public static LocString DETAILS = "Details";

			// Token: 0x0400B9AC RID: 47532
			public static LocString RECIPE_ITEM = "{0} x {1}{2}";

			// Token: 0x0400B9AD RID: 47533
			public static LocString RECIPE_FABRICATOR = "{1} ({0} seconds)";

			// Token: 0x0400B9AE RID: 47534
			public static LocString RECIPE_FABRICATOR_HEADER = "Produced by";

			// Token: 0x0400B9AF RID: 47535
			public static LocString BACK_BUTTON_TOOLTIP = UI.CLICK(UI.ClickType.Click) + " to go back:\n{0}";

			// Token: 0x0400B9B0 RID: 47536
			public static LocString BACK_BUTTON_NO_HISTORY_TOOLTIP = UI.CLICK(UI.ClickType.Click) + " to go back:\nN/A";

			// Token: 0x0400B9B1 RID: 47537
			public static LocString FORWARD_BUTTON_TOOLTIP = UI.CLICK(UI.ClickType.Click) + " to go forward:\n{0}";

			// Token: 0x0400B9B2 RID: 47538
			public static LocString FORWARD_BUTTON_NO_HISTORY_TOOLTIP = UI.CLICK(UI.ClickType.Click) + " to go forward:\nN/A";

			// Token: 0x0400B9B3 RID: 47539
			public static LocString TITLE = "DATABASE";

			// Token: 0x0400B9B4 RID: 47540
			public static LocString MANAGEMENT_BUTTON = "DATABASE";

			// Token: 0x02002D2C RID: 11564
			public class CODEX_DISCOVERED_MESSAGE
			{
				// Token: 0x0400B9B5 RID: 47541
				public static LocString TITLE = "New Log Entry";

				// Token: 0x0400B9B6 RID: 47542
				public static LocString BODY = "I've added a new entry to my log: {codex}\n";
			}

			// Token: 0x02002D2D RID: 11565
			public class SUBWORLDS
			{
				// Token: 0x0400B9B7 RID: 47543
				public static LocString ELEMENTS = "Elements";

				// Token: 0x0400B9B8 RID: 47544
				public static LocString PLANTS = "Plants";

				// Token: 0x0400B9B9 RID: 47545
				public static LocString CRITTERS = "Critters";

				// Token: 0x0400B9BA RID: 47546
				public static LocString NONE = "None";
			}

			// Token: 0x02002D2E RID: 11566
			public class GEYSERS
			{
				// Token: 0x0400B9BB RID: 47547
				public static LocString DESC = "Geysers and Fumaroles emit elements at variable intervals. They provide a sustainable source of material, albeit in typically low volumes.\n\nThe variable factors of a geyser are:\n\n    • Emission element \n    • Emission temperature \n    • Emission mass \n    • Cycle length \n    • Dormancy duration \n    • Disease emitted";
			}

			// Token: 0x02002D2F RID: 11567
			public class EQUIPMENT
			{
				// Token: 0x0400B9BC RID: 47548
				public static LocString DESC = "Equipment description";
			}

			// Token: 0x02002D30 RID: 11568
			public class FOOD
			{
				// Token: 0x0400B9BD RID: 47549
				public static LocString QUALITY = "Quality: {0}";

				// Token: 0x0400B9BE RID: 47550
				public static LocString CALORIES = "Calories: {0}";

				// Token: 0x0400B9BF RID: 47551
				public static LocString SPOILPROPERTIES = "Refrigeration temperature: {0}\nDeep Freeze temperature: {1}\nSpoil time: {2}";

				// Token: 0x0400B9C0 RID: 47552
				public static LocString NON_PERISHABLE = "Spoil time: Never";
			}

			// Token: 0x02002D31 RID: 11569
			public class CATEGORYNAMES
			{
				// Token: 0x0400B9C1 RID: 47553
				public static LocString ROOT = UI.FormatAsLink("Index", "HOME");

				// Token: 0x0400B9C2 RID: 47554
				public static LocString PLANTS = UI.FormatAsLink("Plants", "PLANTS");

				// Token: 0x0400B9C3 RID: 47555
				public static LocString CREATURES = UI.FormatAsLink("Critters", "CREATURES");

				// Token: 0x0400B9C4 RID: 47556
				public static LocString EMAILS = UI.FormatAsLink("E-mail", "EMAILS");

				// Token: 0x0400B9C5 RID: 47557
				public static LocString JOURNALS = UI.FormatAsLink("Journals", "JOURNALS");

				// Token: 0x0400B9C6 RID: 47558
				public static LocString MYLOG = UI.FormatAsLink("My Log", "MYLOG");

				// Token: 0x0400B9C7 RID: 47559
				public static LocString INVESTIGATIONS = UI.FormatAsLink("Investigations", "Investigations");

				// Token: 0x0400B9C8 RID: 47560
				public static LocString RESEARCHNOTES = UI.FormatAsLink("Research Notes", "RESEARCHNOTES");

				// Token: 0x0400B9C9 RID: 47561
				public static LocString NOTICES = UI.FormatAsLink("Notices", "NOTICES");

				// Token: 0x0400B9CA RID: 47562
				public static LocString FOOD = UI.FormatAsLink("Food", "FOOD");

				// Token: 0x0400B9CB RID: 47563
				public static LocString MINION_MODIFIERS = UI.FormatAsLink("Duplicant Effects (EDITOR ONLY)", "MINION_MODIFIERS");

				// Token: 0x0400B9CC RID: 47564
				public static LocString BUILDINGS = UI.FormatAsLink("Buildings", "BUILDINGS");

				// Token: 0x0400B9CD RID: 47565
				public static LocString ROOMS = UI.FormatAsLink("Rooms", "ROOMS");

				// Token: 0x0400B9CE RID: 47566
				public static LocString TECH = UI.FormatAsLink("Research", "TECH");

				// Token: 0x0400B9CF RID: 47567
				public static LocString TIPS = UI.FormatAsLink("Tutorials", "LESSONS");

				// Token: 0x0400B9D0 RID: 47568
				public static LocString EQUIPMENT = UI.FormatAsLink("Equipment", "EQUIPMENT");

				// Token: 0x0400B9D1 RID: 47569
				public static LocString BIOMES = UI.FormatAsLink("Biomes", "BIOMES");

				// Token: 0x0400B9D2 RID: 47570
				public static LocString STORYTRAITS = UI.FormatAsLink("Story Traits", "STORYTRAITS");

				// Token: 0x0400B9D3 RID: 47571
				public static LocString VIDEOS = UI.FormatAsLink("Videos", "VIDEOS");

				// Token: 0x0400B9D4 RID: 47572
				public static LocString MISCELLANEOUSTIPS = UI.FormatAsLink("Tips", "MISCELLANEOUSTIPS");

				// Token: 0x0400B9D5 RID: 47573
				public static LocString MISCELLANEOUSITEMS = UI.FormatAsLink("Items", "MISCELLANEOUSITEMS");

				// Token: 0x0400B9D6 RID: 47574
				public static LocString ELEMENTS = UI.FormatAsLink("Elements", "ELEMENTS");

				// Token: 0x0400B9D7 RID: 47575
				public static LocString ELEMENTSSOLID = UI.FormatAsLink("Solids", "ELEMENTS_SOLID");

				// Token: 0x0400B9D8 RID: 47576
				public static LocString ELEMENTSGAS = UI.FormatAsLink("Gases", "ELEMENTS_GAS");

				// Token: 0x0400B9D9 RID: 47577
				public static LocString ELEMENTSLIQUID = UI.FormatAsLink("Liquids", "ELEMENTS_LIQUID");

				// Token: 0x0400B9DA RID: 47578
				public static LocString ELEMENTSOTHER = UI.FormatAsLink("Other", "ELEMENTS_OTHER");

				// Token: 0x0400B9DB RID: 47579
				public static LocString BUILDINGMATERIALCLASSES = UI.FormatAsLink("Building Materials", "BUILDING_MATERIAL_CLASSES");

				// Token: 0x0400B9DC RID: 47580
				public static LocString INDUSTRIALINGREDIENTS = UI.FormatAsLink("Industrial Ingredients", "INDUSTRIALINGREDIENTS");

				// Token: 0x0400B9DD RID: 47581
				public static LocString GEYSERS = UI.FormatAsLink("Geysers", "GEYSERS");

				// Token: 0x0400B9DE RID: 47582
				public static LocString SYSTEMS = UI.FormatAsLink("Systems", "SYSTEMS");

				// Token: 0x0400B9DF RID: 47583
				public static LocString ROLES = UI.FormatAsLink("Duplicant Skills", "ROLES");

				// Token: 0x0400B9E0 RID: 47584
				public static LocString DISEASE = UI.FormatAsLink("Disease", "DISEASE");

				// Token: 0x0400B9E1 RID: 47585
				public static LocString SICKNESS = UI.FormatAsLink("Sickness", "SICKNESS");

				// Token: 0x0400B9E2 RID: 47586
				public static LocString MEDIA = UI.FormatAsLink("Media", "MEDIA");
			}
		}

		// Token: 0x02002D32 RID: 11570
		public class DEVELOPMENTBUILDS
		{
			// Token: 0x0400B9E3 RID: 47587
			public static LocString WATERMARK = "BUILD: {0}";

			// Token: 0x0400B9E4 RID: 47588
			public static LocString TESTING_WATERMARK = "TESTING BUILD: {0}";

			// Token: 0x0400B9E5 RID: 47589
			public static LocString TESTING_TOOLTIP = "This game is currently running a Test version.\n\n" + UI.CLICK(UI.ClickType.Click) + " for more info.";

			// Token: 0x0400B9E6 RID: 47590
			public static LocString TESTING_MESSAGE_TITLE = "TESTING BUILD";

			// Token: 0x0400B9E7 RID: 47591
			public static LocString TESTING_MESSAGE = "This game is running a Test version of Oxygen Not Included. This means that some features may be in development or buggier than normal, and require more testing before they can be moved into the Release build.\n\nIf you encounter any bugs or strange behavior, please add a report to the bug forums. We appreciate it!";

			// Token: 0x0400B9E8 RID: 47592
			public static LocString TESTING_MORE_INFO = "BUG FORUMS";

			// Token: 0x0400B9E9 RID: 47593
			public static LocString FULL_PATCH_NOTES = "Full Patch Notes";

			// Token: 0x0400B9EA RID: 47594
			public static LocString PREVIOUS_VERSION = "Previous Version";

			// Token: 0x02002D33 RID: 11571
			public class ALPHA
			{
				// Token: 0x02002D34 RID: 11572
				public class MESSAGES
				{
					// Token: 0x0400B9EB RID: 47595
					public static LocString FORUMBUTTON = "FORUMS";

					// Token: 0x0400B9EC RID: 47596
					public static LocString MAILINGLIST = "MAILING LIST";

					// Token: 0x0400B9ED RID: 47597
					public static LocString PATCHNOTES = "PATCH NOTES";

					// Token: 0x0400B9EE RID: 47598
					public static LocString FEEDBACK = "FEEDBACK";
				}

				// Token: 0x02002D35 RID: 11573
				public class LOADING
				{
					// Token: 0x0400B9EF RID: 47599
					public static LocString TITLE = "<b>Welcome to Oxygen Not Included!</b>";

					// Token: 0x0400B9F0 RID: 47600
					public static LocString BODY = "This game is in the early stages of development which means you're likely to encounter strange, amusing, and occasionally just downright frustrating bugs.\n\nDuring this time Oxygen Not Included will be receiving regular updates to fix bugs, add features, and introduce additional content, so if you encounter issues or just have suggestions to share, please let us know on our forums: <u>http://forums.kleientertainment.com</u>\n\nA special thanks to those who joined us during our time in Alpha. We value your feedback and thank you for joining us in the development process. We couldn't do this without you.\n\nEnjoy your time in deep space!\n\n- Klei";

					// Token: 0x0400B9F1 RID: 47601
					public static LocString BODY_NOLINKS = "This DLC is currently in active development, which means you're likely to encounter strange, amusing, and occasionally just downright frustrating bugs.\n\n During this time Spaced Out! will be receiving regular updates to fix bugs, add features, and introduce additional content.\n\n We've got lots of content old and new to add to this DLC before it's ready, and we're happy to have you along with us. Enjoy your time in deep space!\n\n - The Team at Klei";

					// Token: 0x0400B9F2 RID: 47602
					public static LocString FORUMBUTTON = "Visit Forums";
				}

				// Token: 0x02002D36 RID: 11574
				public class HEALTHY_MESSAGE
				{
					// Token: 0x0400B9F3 RID: 47603
					public static LocString CONTINUEBUTTON = "Thanks!";
				}
			}

			// Token: 0x02002D37 RID: 11575
			public class PREVIOUS_UPDATE
			{
				// Token: 0x0400B9F4 RID: 47604
				public static LocString TITLE = "<b>Welcome to Oxygen Not Included</b>";

				// Token: 0x0400B9F5 RID: 47605
				public static LocString BODY = "Whoops!\n\nYou're about to opt in to the <b>Previous Update branch</b>. That means opting out of all new features, fixes and content from the live branch.\n\nThis branch is temporary. It will be replaced when the next update is released. It's also completely unsupported: please don't report bugs or issues you find here.\n\nAre you sure you want to opt in?";

				// Token: 0x0400B9F6 RID: 47606
				public static LocString CONTINUEBUTTON = "Play Old Version";

				// Token: 0x0400B9F7 RID: 47607
				public static LocString FORUMBUTTON = "More Information";

				// Token: 0x0400B9F8 RID: 47608
				public static LocString QUITBUTTON = "Quit";
			}

			// Token: 0x02002D38 RID: 11576
			public class DLC_BETA
			{
				// Token: 0x0400B9F9 RID: 47609
				public static LocString TITLE = "<b>Welcome to Oxygen Not Included</b>";

				// Token: 0x0400B9FA RID: 47610
				public static LocString BODY = "You're about to opt in to the beta for <b>The Frosty Planet Pack</b> DLC.\nThis free beta is a work in progress, and will be discontinued before the paid DLC is released. \n\nAre you sure you want to opt in?";

				// Token: 0x0400B9FB RID: 47611
				public static LocString CONTINUEBUTTON = "Play Beta";

				// Token: 0x0400B9FC RID: 47612
				public static LocString FORUMBUTTON = "More Information";

				// Token: 0x0400B9FD RID: 47613
				public static LocString QUITBUTTON = "Quit";
			}

			// Token: 0x02002D39 RID: 11577
			public class UPDATES
			{
				// Token: 0x0400B9FE RID: 47614
				public static LocString UPDATES_HEADER = "NEXT UPGRADE LIVE IN";

				// Token: 0x0400B9FF RID: 47615
				public static LocString NOW = "Less than a day";

				// Token: 0x0400BA00 RID: 47616
				public static LocString TWENTY_FOUR_HOURS = "Less than a day";

				// Token: 0x0400BA01 RID: 47617
				public static LocString FINAL_WEEK = "{0} days";

				// Token: 0x0400BA02 RID: 47618
				public static LocString BIGGER_TIMES = "{1} weeks {0} days";
			}
		}

		// Token: 0x02002D3A RID: 11578
		public class UNITSUFFIXES
		{
			// Token: 0x0400BA03 RID: 47619
			public static LocString SECOND = " s";

			// Token: 0x0400BA04 RID: 47620
			public static LocString PERSECOND = "/s";

			// Token: 0x0400BA05 RID: 47621
			public static LocString PERCYCLE = "/cycle";

			// Token: 0x0400BA06 RID: 47622
			public static LocString UNIT = " unit";

			// Token: 0x0400BA07 RID: 47623
			public static LocString UNITS = " units";

			// Token: 0x0400BA08 RID: 47624
			public static LocString PERCENT = "%";

			// Token: 0x0400BA09 RID: 47625
			public static LocString DEGREES = " degrees";

			// Token: 0x0400BA0A RID: 47626
			public static LocString CRITTERS = " critters";

			// Token: 0x0400BA0B RID: 47627
			public static LocString GROWTH = "growth";

			// Token: 0x0400BA0C RID: 47628
			public static LocString SECONDS = "Seconds";

			// Token: 0x0400BA0D RID: 47629
			public static LocString DUPLICANTS = "Duplicants";

			// Token: 0x0400BA0E RID: 47630
			public static LocString GERMS = "Germs";

			// Token: 0x0400BA0F RID: 47631
			public static LocString ROCKET_MISSIONS = "Missions";

			// Token: 0x0400BA10 RID: 47632
			public static LocString TILES = "Tiles";

			// Token: 0x02002D3B RID: 11579
			public class MASS
			{
				// Token: 0x0400BA11 RID: 47633
				public static LocString TONNE = " t";

				// Token: 0x0400BA12 RID: 47634
				public static LocString KILOGRAM = " kg";

				// Token: 0x0400BA13 RID: 47635
				public static LocString GRAM = " g";

				// Token: 0x0400BA14 RID: 47636
				public static LocString MILLIGRAM = " mg";

				// Token: 0x0400BA15 RID: 47637
				public static LocString MICROGRAM = " mcg";

				// Token: 0x0400BA16 RID: 47638
				public static LocString POUND = " lb";

				// Token: 0x0400BA17 RID: 47639
				public static LocString DRACHMA = " dr";

				// Token: 0x0400BA18 RID: 47640
				public static LocString GRAIN = " gr";
			}

			// Token: 0x02002D3C RID: 11580
			public class TEMPERATURE
			{
				// Token: 0x0400BA19 RID: 47641
				public static LocString CELSIUS = " " + 'º'.ToString() + "C";

				// Token: 0x0400BA1A RID: 47642
				public static LocString FAHRENHEIT = " " + 'º'.ToString() + "F";

				// Token: 0x0400BA1B RID: 47643
				public static LocString KELVIN = " K";
			}

			// Token: 0x02002D3D RID: 11581
			public class CALORIES
			{
				// Token: 0x0400BA1C RID: 47644
				public static LocString CALORIE = " cal";

				// Token: 0x0400BA1D RID: 47645
				public static LocString KILOCALORIE = " kcal";
			}

			// Token: 0x02002D3E RID: 11582
			public class ELECTRICAL
			{
				// Token: 0x0400BA1E RID: 47646
				public static LocString JOULE = " J";

				// Token: 0x0400BA1F RID: 47647
				public static LocString KILOJOULE = " kJ";

				// Token: 0x0400BA20 RID: 47648
				public static LocString MEGAJOULE = " MJ";

				// Token: 0x0400BA21 RID: 47649
				public static LocString WATT = " W";

				// Token: 0x0400BA22 RID: 47650
				public static LocString KILOWATT = " kW";
			}

			// Token: 0x02002D3F RID: 11583
			public class HEAT
			{
				// Token: 0x0400BA23 RID: 47651
				public static LocString DTU = " DTU";

				// Token: 0x0400BA24 RID: 47652
				public static LocString KDTU = " kDTU";

				// Token: 0x0400BA25 RID: 47653
				public static LocString DTU_S = " DTU/s";

				// Token: 0x0400BA26 RID: 47654
				public static LocString KDTU_S = " kDTU/s";
			}

			// Token: 0x02002D40 RID: 11584
			public class DISTANCE
			{
				// Token: 0x0400BA27 RID: 47655
				public static LocString METER = " m";

				// Token: 0x0400BA28 RID: 47656
				public static LocString KILOMETER = " km";
			}

			// Token: 0x02002D41 RID: 11585
			public class DISEASE
			{
				// Token: 0x0400BA29 RID: 47657
				public static LocString UNITS = " germs";
			}

			// Token: 0x02002D42 RID: 11586
			public class NOISE
			{
				// Token: 0x0400BA2A RID: 47658
				public static LocString UNITS = " dB";
			}

			// Token: 0x02002D43 RID: 11587
			public class INFORMATION
			{
				// Token: 0x0400BA2B RID: 47659
				public static LocString BYTE = "B";

				// Token: 0x0400BA2C RID: 47660
				public static LocString KILOBYTE = "kB";

				// Token: 0x0400BA2D RID: 47661
				public static LocString MEGABYTE = "MB";

				// Token: 0x0400BA2E RID: 47662
				public static LocString GIGABYTE = "GB";

				// Token: 0x0400BA2F RID: 47663
				public static LocString TERABYTE = "TB";
			}

			// Token: 0x02002D44 RID: 11588
			public class LIGHT
			{
				// Token: 0x0400BA30 RID: 47664
				public static LocString LUX = " lux";
			}

			// Token: 0x02002D45 RID: 11589
			public class RADIATION
			{
				// Token: 0x0400BA31 RID: 47665
				public static LocString RADS = " rads";
			}

			// Token: 0x02002D46 RID: 11590
			public class HIGHENERGYPARTICLES
			{
				// Token: 0x0400BA32 RID: 47666
				public static LocString PARTRICLE = " Radbolt";

				// Token: 0x0400BA33 RID: 47667
				public static LocString PARTRICLES = " Radbolts";
			}
		}

		// Token: 0x02002D47 RID: 11591
		public class OVERLAYS
		{
			// Token: 0x02002D48 RID: 11592
			public class TILEMODE
			{
				// Token: 0x0400BA34 RID: 47668
				public static LocString NAME = "MATERIALS OVERLAY";

				// Token: 0x0400BA35 RID: 47669
				public static LocString BUTTON = "Materials Overlay";
			}

			// Token: 0x02002D49 RID: 11593
			public class OXYGEN
			{
				// Token: 0x0400BA36 RID: 47670
				public static LocString NAME = "OXYGEN OVERLAY";

				// Token: 0x0400BA37 RID: 47671
				public static LocString BUTTON = "Oxygen Overlay";

				// Token: 0x0400BA38 RID: 47672
				public static LocString LEGEND1 = "Very Breathable";

				// Token: 0x0400BA39 RID: 47673
				public static LocString LEGEND2 = "Breathable";

				// Token: 0x0400BA3A RID: 47674
				public static LocString LEGEND3 = "Barely Breathable";

				// Token: 0x0400BA3B RID: 47675
				public static LocString LEGEND4 = "Unbreathable";

				// Token: 0x0400BA3C RID: 47676
				public static LocString LEGEND5 = "Barely Breathable";

				// Token: 0x0400BA3D RID: 47677
				public static LocString LEGEND6 = "Unbreathable";

				// Token: 0x02002D4A RID: 11594
				public class TOOLTIPS
				{
					// Token: 0x0400BA3E RID: 47678
					public static LocString LEGEND1 = string.Concat(new string[]
					{
						"<b>Very Breathable</b>\nHigh ",
						UI.PRE_KEYWORD,
						"Oxygen",
						UI.PST_KEYWORD,
						" concentrations"
					});

					// Token: 0x0400BA3F RID: 47679
					public static LocString LEGEND2 = string.Concat(new string[]
					{
						"<b>Breathable</b>\nSufficient ",
						UI.PRE_KEYWORD,
						"Oxygen",
						UI.PST_KEYWORD,
						" concentrations"
					});

					// Token: 0x0400BA40 RID: 47680
					public static LocString LEGEND3 = string.Concat(new string[]
					{
						"<b>Barely Breathable</b>\nLow ",
						UI.PRE_KEYWORD,
						"Oxygen",
						UI.PST_KEYWORD,
						" concentrations"
					});

					// Token: 0x0400BA41 RID: 47681
					public static LocString LEGEND4 = string.Concat(new string[]
					{
						"<b>Unbreathable</b>\nExtremely low or absent ",
						UI.PRE_KEYWORD,
						"Oxygen",
						UI.PST_KEYWORD,
						" concentrations\n\nDuplicants will suffocate if trapped in these areas"
					});

					// Token: 0x0400BA42 RID: 47682
					public static LocString LEGEND5 = "<b>Slightly Toxic</b>\nHarmful gas concentration";

					// Token: 0x0400BA43 RID: 47683
					public static LocString LEGEND6 = "<b>Very Toxic</b>\nLethal gas concentration";
				}
			}

			// Token: 0x02002D4B RID: 11595
			public class ELECTRICAL
			{
				// Token: 0x0400BA44 RID: 47684
				public static LocString NAME = "POWER OVERLAY";

				// Token: 0x0400BA45 RID: 47685
				public static LocString BUTTON = "Power Overlay";

				// Token: 0x0400BA46 RID: 47686
				public static LocString LEGEND1 = "<b>BUILDING POWER</b>";

				// Token: 0x0400BA47 RID: 47687
				public static LocString LEGEND2 = "Consumer";

				// Token: 0x0400BA48 RID: 47688
				public static LocString LEGEND3 = "Producer";

				// Token: 0x0400BA49 RID: 47689
				public static LocString LEGEND4 = "<b>CIRCUIT POWER HEALTH</b>";

				// Token: 0x0400BA4A RID: 47690
				public static LocString LEGEND5 = "Inactive";

				// Token: 0x0400BA4B RID: 47691
				public static LocString LEGEND6 = "Safe";

				// Token: 0x0400BA4C RID: 47692
				public static LocString LEGEND7 = "Strained";

				// Token: 0x0400BA4D RID: 47693
				public static LocString LEGEND8 = "Overloaded";

				// Token: 0x0400BA4E RID: 47694
				public static LocString DIAGRAM_HEADER = "Energy from the <b>Left Outlet</b> is used by the <b>Right Outlet</b>";

				// Token: 0x0400BA4F RID: 47695
				public static LocString LEGEND_SWITCH = "Switch";

				// Token: 0x02002D4C RID: 11596
				public class TOOLTIPS
				{
					// Token: 0x0400BA50 RID: 47696
					public static LocString LEGEND1 = "Displays whether buildings use or generate " + UI.FormatAsLink("Power", "POWER");

					// Token: 0x0400BA51 RID: 47697
					public static LocString LEGEND2 = "<b>Consumer</b>\nThese buildings draw power from a circuit";

					// Token: 0x0400BA52 RID: 47698
					public static LocString LEGEND3 = "<b>Producer</b>\nThese buildings generate power for a circuit";

					// Token: 0x0400BA53 RID: 47699
					public static LocString LEGEND4 = "Displays the health of wire systems";

					// Token: 0x0400BA54 RID: 47700
					public static LocString LEGEND5 = "<b>Inactive</b>\nThere is no power activity on these circuits";

					// Token: 0x0400BA55 RID: 47701
					public static LocString LEGEND6 = "<b>Safe</b>\nThese circuits are not in danger of overloading";

					// Token: 0x0400BA56 RID: 47702
					public static LocString LEGEND7 = "<b>Strained</b>\nThese circuits are close to consuming more power than their wires support";

					// Token: 0x0400BA57 RID: 47703
					public static LocString LEGEND8 = "<b>Overloaded</b>\nThese circuits are consuming more power than their wires support";

					// Token: 0x0400BA58 RID: 47704
					public static LocString LEGEND_SWITCH = "<b>Switch</b>\nActivates or deactivates connected circuits";
				}
			}

			// Token: 0x02002D4D RID: 11597
			public class TEMPERATURE
			{
				// Token: 0x0400BA59 RID: 47705
				public static LocString NAME = "TEMPERATURE OVERLAY";

				// Token: 0x0400BA5A RID: 47706
				public static LocString BUTTON = "Temperature Overlay";

				// Token: 0x0400BA5B RID: 47707
				public static LocString EXTREMECOLD = "Absolute Zero";

				// Token: 0x0400BA5C RID: 47708
				public static LocString VERYCOLD = "Cold";

				// Token: 0x0400BA5D RID: 47709
				public static LocString COLD = "Chilled";

				// Token: 0x0400BA5E RID: 47710
				public static LocString TEMPERATE = "Temperate";

				// Token: 0x0400BA5F RID: 47711
				public static LocString HOT = "Warm";

				// Token: 0x0400BA60 RID: 47712
				public static LocString VERYHOT = "Hot";

				// Token: 0x0400BA61 RID: 47713
				public static LocString EXTREMEHOT = "Scorching";

				// Token: 0x0400BA62 RID: 47714
				public static LocString MAXHOT = "Molten";

				// Token: 0x0400BA63 RID: 47715
				public static LocString HEATSOURCES = "Heat Source";

				// Token: 0x0400BA64 RID: 47716
				public static LocString HEATSINK = "Heat Sink";

				// Token: 0x0400BA65 RID: 47717
				public static LocString DEFAULT_TEMPERATURE_BUTTON = "Default";

				// Token: 0x02002D4E RID: 11598
				public class TOOLTIPS
				{
					// Token: 0x0400BA66 RID: 47718
					public static LocString TEMPERATURE = "Temperatures reaching {0}";

					// Token: 0x0400BA67 RID: 47719
					public static LocString HEATSOURCES = "Elements displaying this symbol can produce heat";

					// Token: 0x0400BA68 RID: 47720
					public static LocString HEATSINK = "Elements displaying this symbol can absorb heat";
				}
			}

			// Token: 0x02002D4F RID: 11599
			public class STATECHANGE
			{
				// Token: 0x0400BA69 RID: 47721
				public static LocString LOWPOINT = "Low energy state change";

				// Token: 0x0400BA6A RID: 47722
				public static LocString STABLE = "Stable";

				// Token: 0x0400BA6B RID: 47723
				public static LocString HIGHPOINT = "High energy state change";

				// Token: 0x02002D50 RID: 11600
				public class TOOLTIPS
				{
					// Token: 0x0400BA6C RID: 47724
					public static LocString LOWPOINT = "Nearing a low energy state change";

					// Token: 0x0400BA6D RID: 47725
					public static LocString STABLE = "Not near any state changes";

					// Token: 0x0400BA6E RID: 47726
					public static LocString HIGHPOINT = "Nearing high energy state change";
				}
			}

			// Token: 0x02002D51 RID: 11601
			public class HEATFLOW
			{
				// Token: 0x0400BA6F RID: 47727
				public static LocString NAME = "THERMAL TOLERANCE OVERLAY";

				// Token: 0x0400BA70 RID: 47728
				public static LocString HOVERTITLE = "THERMAL TOLERANCE";

				// Token: 0x0400BA71 RID: 47729
				public static LocString BUTTON = "Thermal Tolerance Overlay";

				// Token: 0x0400BA72 RID: 47730
				public static LocString COOLING = "Body Heat Loss";

				// Token: 0x0400BA73 RID: 47731
				public static LocString NEUTRAL = "Comfort Zone";

				// Token: 0x0400BA74 RID: 47732
				public static LocString HEATING = "Body Heat Retention";

				// Token: 0x0400BA75 RID: 47733
				public static LocString COOLING_DUPE = "Body Heat Loss {0}\n\nUncomfortably chilly surroundings";

				// Token: 0x0400BA76 RID: 47734
				public static LocString NEUTRAL_DUPE = "Comfort Zone {0}";

				// Token: 0x0400BA77 RID: 47735
				public static LocString HEATING_DUPE = "Body Heat Loss {0}\n\nUncomfortably toasty surroundings";

				// Token: 0x02002D52 RID: 11602
				public class TOOLTIPS
				{
					// Token: 0x0400BA78 RID: 47736
					public static LocString COOLING = "<b>Body Heat Loss</b>\nUncomfortably cold\n\nDuplicants lose more heat in chilly surroundings than they can absorb\n    • Warm Coats help Duplicants retain body heat";

					// Token: 0x0400BA79 RID: 47737
					public static LocString NEUTRAL = "<b>Comfort Zone</b>\nComfortable area\n\nDuplicants can regulate their internal temperatures in these areas";

					// Token: 0x0400BA7A RID: 47738
					public static LocString HEATING = "<b>Body Heat Retention</b>\nUncomfortably warm\n\nDuplicants absorb more heat in toasty surroundings than they can release";
				}
			}

			// Token: 0x02002D53 RID: 11603
			public class RELATIVETEMPERATURE
			{
				// Token: 0x0400BA7B RID: 47739
				public static LocString NAME = "RELATIVE TEMPERATURE";

				// Token: 0x0400BA7C RID: 47740
				public static LocString HOVERTITLE = "RELATIVE TEMPERATURE";

				// Token: 0x0400BA7D RID: 47741
				public static LocString BUTTON = "Relative Temperature Overlay";
			}

			// Token: 0x02002D54 RID: 11604
			public class ROOMS
			{
				// Token: 0x0400BA7E RID: 47742
				public static LocString NAME = "ROOM OVERLAY";

				// Token: 0x0400BA7F RID: 47743
				public static LocString BUTTON = "Room Overlay";

				// Token: 0x0400BA80 RID: 47744
				public static LocString ROOM = "Room {0}";

				// Token: 0x0400BA81 RID: 47745
				public static LocString HOVERTITLE = "ROOMS";

				// Token: 0x02002D55 RID: 11605
				public static class NOROOM
				{
					// Token: 0x0400BA82 RID: 47746
					public static LocString HEADER = "No Room";

					// Token: 0x0400BA83 RID: 47747
					public static LocString DESC = "Enclose this space with walls and doors to make a room";

					// Token: 0x0400BA84 RID: 47748
					public static LocString TOO_BIG = "<color=#F44A47FF>    • Size: {0} Tiles\n    • Maximum room size: {1} Tiles</color>";
				}

				// Token: 0x02002D56 RID: 11606
				public class TOOLTIPS
				{
					// Token: 0x0400BA85 RID: 47749
					public static LocString ROOM = "Completed Duplicant bedrooms";

					// Token: 0x0400BA86 RID: 47750
					public static LocString NOROOMS = "Duplicants have nowhere to sleep";
				}
			}

			// Token: 0x02002D57 RID: 11607
			public class JOULES
			{
				// Token: 0x0400BA87 RID: 47751
				public static LocString NAME = "JOULES";

				// Token: 0x0400BA88 RID: 47752
				public static LocString HOVERTITLE = "JOULES";

				// Token: 0x0400BA89 RID: 47753
				public static LocString BUTTON = "Joules Overlay";
			}

			// Token: 0x02002D58 RID: 11608
			public class LIGHTING
			{
				// Token: 0x0400BA8A RID: 47754
				public static LocString NAME = "LIGHT OVERLAY";

				// Token: 0x0400BA8B RID: 47755
				public static LocString BUTTON = "Light Overlay";

				// Token: 0x0400BA8C RID: 47756
				public static LocString LITAREA = "Lit Area";

				// Token: 0x0400BA8D RID: 47757
				public static LocString DARK = "Unlit Area";

				// Token: 0x0400BA8E RID: 47758
				public static LocString HOVERTITLE = "LIGHT";

				// Token: 0x0400BA8F RID: 47759
				public static LocString DESC = "{0} Lux";

				// Token: 0x02002D59 RID: 11609
				public class RANGES
				{
					// Token: 0x0400BA90 RID: 47760
					public static LocString NO_LIGHT = "Pitch Black";

					// Token: 0x0400BA91 RID: 47761
					public static LocString VERY_LOW_LIGHT = "Very Dim";

					// Token: 0x0400BA92 RID: 47762
					public static LocString LOW_LIGHT = "Dim";

					// Token: 0x0400BA93 RID: 47763
					public static LocString MEDIUM_LIGHT = "Well Lit";

					// Token: 0x0400BA94 RID: 47764
					public static LocString HIGH_LIGHT = "Bright";

					// Token: 0x0400BA95 RID: 47765
					public static LocString VERY_HIGH_LIGHT = "Brilliant";

					// Token: 0x0400BA96 RID: 47766
					public static LocString MAX_LIGHT = "Blinding";
				}

				// Token: 0x02002D5A RID: 11610
				public class TOOLTIPS
				{
					// Token: 0x0400BA97 RID: 47767
					public static LocString NAME = "LIGHT OVERLAY";

					// Token: 0x0400BA98 RID: 47768
					public static LocString LITAREA = "<b>Lit Area</b>\nWorking in well-lit areas improves Duplicant " + UI.PRE_KEYWORD + "Morale" + UI.PST_KEYWORD;

					// Token: 0x0400BA99 RID: 47769
					public static LocString DARK = "<b>Unlit Area</b>\nWorking in the dark has no effect on Duplicants";
				}
			}

			// Token: 0x02002D5B RID: 11611
			public class CROP
			{
				// Token: 0x0400BA9A RID: 47770
				public static LocString NAME = "FARMING OVERLAY";

				// Token: 0x0400BA9B RID: 47771
				public static LocString BUTTON = "Farming Overlay";

				// Token: 0x0400BA9C RID: 47772
				public static LocString GROWTH_HALTED = "Halted Growth";

				// Token: 0x0400BA9D RID: 47773
				public static LocString GROWING = "Growing";

				// Token: 0x0400BA9E RID: 47774
				public static LocString FULLY_GROWN = "Fully Grown";

				// Token: 0x02002D5C RID: 11612
				public class TOOLTIPS
				{
					// Token: 0x0400BA9F RID: 47775
					public static LocString GROWTH_HALTED = "<b>Halted Growth</b>\nSubstandard conditions prevent these plants from growing";

					// Token: 0x0400BAA0 RID: 47776
					public static LocString GROWING = "<b>Growing</b>\nThese plants are thriving in their current conditions";

					// Token: 0x0400BAA1 RID: 47777
					public static LocString FULLY_GROWN = "<b>Fully Grown</b>\nThese plants have reached maturation\n\nSelect the " + UI.FormatAsTool("Harvest Tool", global::Action.Harvest) + " to batch harvest";
				}
			}

			// Token: 0x02002D5D RID: 11613
			public class LIQUIDPLUMBING
			{
				// Token: 0x0400BAA2 RID: 47778
				public static LocString NAME = "PLUMBING OVERLAY";

				// Token: 0x0400BAA3 RID: 47779
				public static LocString BUTTON = "Plumbing Overlay";

				// Token: 0x0400BAA4 RID: 47780
				public static LocString CONSUMER = "Output Pipe";

				// Token: 0x0400BAA5 RID: 47781
				public static LocString FILTERED = "Filtered Output Pipe";

				// Token: 0x0400BAA6 RID: 47782
				public static LocString PRODUCER = "Building Intake";

				// Token: 0x0400BAA7 RID: 47783
				public static LocString CONNECTED = "Connected";

				// Token: 0x0400BAA8 RID: 47784
				public static LocString DISCONNECTED = "Disconnected";

				// Token: 0x0400BAA9 RID: 47785
				public static LocString NETWORK = "Liquid Network {0}";

				// Token: 0x0400BAAA RID: 47786
				public static LocString DIAGRAM_BEFORE_ARROW = "Liquid flows from <b>Output Pipe</b>";

				// Token: 0x0400BAAB RID: 47787
				public static LocString DIAGRAM_AFTER_ARROW = "<b>Building Intake</b>";

				// Token: 0x02002D5E RID: 11614
				public class TOOLTIPS
				{
					// Token: 0x0400BAAC RID: 47788
					public static LocString CONNECTED = "Connected to a " + UI.FormatAsLink("Liquid Pipe", "LIQUIDCONDUIT");

					// Token: 0x0400BAAD RID: 47789
					public static LocString DISCONNECTED = "Not connected to a " + UI.FormatAsLink("Liquid Pipe", "LIQUIDCONDUIT");

					// Token: 0x0400BAAE RID: 47790
					public static LocString CONSUMER = "<b>Output Pipe</b>\nOutputs send liquid into pipes\n\nMust be on the same network as at least one " + UI.FormatAsLink("Intake", "LIQUIDPIPING");

					// Token: 0x0400BAAF RID: 47791
					public static LocString FILTERED = "<b>Filtered Output Pipe</b>\nFiltered Outputs send filtered liquid into pipes\n\nMust be on the same network as at least one " + UI.FormatAsLink("Intake", "LIQUIDPIPING");

					// Token: 0x0400BAB0 RID: 47792
					public static LocString PRODUCER = "<b>Building Intake</b>\nIntakes send liquid into buildings\n\nMust be on the same network as at least one " + UI.FormatAsLink("Output", "LIQUIDPIPING");

					// Token: 0x0400BAB1 RID: 47793
					public static LocString NETWORK = "Liquid network {0}";
				}
			}

			// Token: 0x02002D5F RID: 11615
			public class GASPLUMBING
			{
				// Token: 0x0400BAB2 RID: 47794
				public static LocString NAME = "VENTILATION OVERLAY";

				// Token: 0x0400BAB3 RID: 47795
				public static LocString BUTTON = "Ventilation Overlay";

				// Token: 0x0400BAB4 RID: 47796
				public static LocString CONSUMER = "Output Pipe";

				// Token: 0x0400BAB5 RID: 47797
				public static LocString FILTERED = "Filtered Output Pipe";

				// Token: 0x0400BAB6 RID: 47798
				public static LocString PRODUCER = "Building Intake";

				// Token: 0x0400BAB7 RID: 47799
				public static LocString CONNECTED = "Connected";

				// Token: 0x0400BAB8 RID: 47800
				public static LocString DISCONNECTED = "Disconnected";

				// Token: 0x0400BAB9 RID: 47801
				public static LocString NETWORK = "Gas Network {0}";

				// Token: 0x0400BABA RID: 47802
				public static LocString DIAGRAM_BEFORE_ARROW = "Gas flows from <b>Output Pipe</b>";

				// Token: 0x0400BABB RID: 47803
				public static LocString DIAGRAM_AFTER_ARROW = "<b>Building Intake</b>";

				// Token: 0x02002D60 RID: 11616
				public class TOOLTIPS
				{
					// Token: 0x0400BABC RID: 47804
					public static LocString CONNECTED = "Connected to a " + UI.FormatAsLink("Gas Pipe", "GASPIPING");

					// Token: 0x0400BABD RID: 47805
					public static LocString DISCONNECTED = "Not connected to a " + UI.FormatAsLink("Gas Pipe", "GASPIPING");

					// Token: 0x0400BABE RID: 47806
					public static LocString CONSUMER = string.Concat(new string[]
					{
						"<b>Output Pipe</b>\nOutputs send ",
						UI.PRE_KEYWORD,
						"Gas",
						UI.PST_KEYWORD,
						" into ",
						UI.PRE_KEYWORD,
						"Pipes",
						UI.PST_KEYWORD,
						"\n\nMust be on the same network as at least one ",
						UI.FormatAsLink("Intake", "GASPIPING")
					});

					// Token: 0x0400BABF RID: 47807
					public static LocString FILTERED = string.Concat(new string[]
					{
						"<b>Filtered Output Pipe</b>\nFiltered Outputs send filtered ",
						UI.PRE_KEYWORD,
						"Gas",
						UI.PST_KEYWORD,
						" into ",
						UI.PRE_KEYWORD,
						"Pipes",
						UI.PST_KEYWORD,
						"\n\nMust be on the same network as at least one ",
						UI.FormatAsLink("Intake", "GASPIPING")
					});

					// Token: 0x0400BAC0 RID: 47808
					public static LocString PRODUCER = "<b>Building Intake</b>\nIntakes send gas into buildings\n\nMust be on the same network as at least one " + UI.FormatAsLink("Output", "GASPIPING");

					// Token: 0x0400BAC1 RID: 47809
					public static LocString NETWORK = "Gas network {0}";
				}
			}

			// Token: 0x02002D61 RID: 11617
			public class SUIT
			{
				// Token: 0x0400BAC2 RID: 47810
				public static LocString NAME = "EXOSUIT OVERLAY";

				// Token: 0x0400BAC3 RID: 47811
				public static LocString BUTTON = "Exosuit Overlay";

				// Token: 0x0400BAC4 RID: 47812
				public static LocString SUIT_ICON = "Exosuit";

				// Token: 0x0400BAC5 RID: 47813
				public static LocString SUIT_ICON_TOOLTIP = "<b>Exosuit</b>\nHighlights the current location of equippable exosuits";
			}

			// Token: 0x02002D62 RID: 11618
			public class LOGIC
			{
				// Token: 0x0400BAC6 RID: 47814
				public static LocString NAME = "AUTOMATION OVERLAY";

				// Token: 0x0400BAC7 RID: 47815
				public static LocString BUTTON = "Automation Overlay";

				// Token: 0x0400BAC8 RID: 47816
				public static LocString INPUT = "Input Port";

				// Token: 0x0400BAC9 RID: 47817
				public static LocString OUTPUT = "Output Port";

				// Token: 0x0400BACA RID: 47818
				public static LocString RIBBON_INPUT = "Ribbon Input Port";

				// Token: 0x0400BACB RID: 47819
				public static LocString RIBBON_OUTPUT = "Ribbon Output Port";

				// Token: 0x0400BACC RID: 47820
				public static LocString RESET_UPDATE = "Reset Port";

				// Token: 0x0400BACD RID: 47821
				public static LocString CONTROL_INPUT = "Control Port";

				// Token: 0x0400BACE RID: 47822
				public static LocString CIRCUIT_STATUS_HEADER = "GRID STATUS";

				// Token: 0x0400BACF RID: 47823
				public static LocString ONE = "Green";

				// Token: 0x0400BAD0 RID: 47824
				public static LocString ZERO = "Red";

				// Token: 0x0400BAD1 RID: 47825
				public static LocString DISCONNECTED = "DISCONNECTED";

				// Token: 0x02002D63 RID: 11619
				public abstract class TOOLTIPS
				{
					// Token: 0x0400BAD2 RID: 47826
					public static LocString INPUT = "<b>Input Port</b>\nReceives a signal from an automation grid";

					// Token: 0x0400BAD3 RID: 47827
					public static LocString OUTPUT = "<b>Output Port</b>\nSends a signal out to an automation grid";

					// Token: 0x0400BAD4 RID: 47828
					public static LocString RIBBON_INPUT = "<b>Ribbon Input Port</b>\nReceives a 4-bit signal from an automation grid";

					// Token: 0x0400BAD5 RID: 47829
					public static LocString RIBBON_OUTPUT = "<b>Ribbon Output Port</b>\nSends a 4-bit signal out to an automation grid";

					// Token: 0x0400BAD6 RID: 47830
					public static LocString RESET_UPDATE = "<b>Reset Port</b>\nReset a " + BUILDINGS.PREFABS.LOGICMEMORY.NAME + "'s internal Memory to " + UI.FormatAsAutomationState("Red", UI.AutomationState.Standby);

					// Token: 0x0400BAD7 RID: 47831
					public static LocString CONTROL_INPUT = "<b>Control Port</b>\nControl the signal selection of a " + BUILDINGS.PREFABS.LOGICGATEMULTIPLEXER.NAME + " or " + BUILDINGS.PREFABS.LOGICGATEDEMULTIPLEXER.NAME;

					// Token: 0x0400BAD8 RID: 47832
					public static LocString ONE = "<b>Green</b>\nThis port is currently " + UI.FormatAsAutomationState("Green", UI.AutomationState.Active);

					// Token: 0x0400BAD9 RID: 47833
					public static LocString ZERO = "<b>Red</b>\nThis port is currently " + UI.FormatAsAutomationState("Red", UI.AutomationState.Standby);

					// Token: 0x0400BADA RID: 47834
					public static LocString DISCONNECTED = "<b>Disconnected</b>\nThis port is not connected to an automation grid";
				}
			}

			// Token: 0x02002D64 RID: 11620
			public class CONVEYOR
			{
				// Token: 0x0400BADB RID: 47835
				public static LocString NAME = "CONVEYOR OVERLAY";

				// Token: 0x0400BADC RID: 47836
				public static LocString BUTTON = "Conveyor Overlay";

				// Token: 0x0400BADD RID: 47837
				public static LocString OUTPUT = "Loader";

				// Token: 0x0400BADE RID: 47838
				public static LocString INPUT = "Receptacle";

				// Token: 0x02002D65 RID: 11621
				public abstract class TOOLTIPS
				{
					// Token: 0x0400BADF RID: 47839
					public static LocString OUTPUT = string.Concat(new string[]
					{
						"<b>Loader</b>\nLoads material onto a ",
						UI.PRE_KEYWORD,
						"Conveyor Rail",
						UI.PST_KEYWORD,
						" for transport to Receptacles"
					});

					// Token: 0x0400BAE0 RID: 47840
					public static LocString INPUT = string.Concat(new string[]
					{
						"<b>Receptacle</b>\nReceives material from a ",
						UI.PRE_KEYWORD,
						"Conveyor Rail",
						UI.PST_KEYWORD,
						" and stores it for Duplicant use"
					});
				}
			}

			// Token: 0x02002D66 RID: 11622
			public class DECOR
			{
				// Token: 0x0400BAE1 RID: 47841
				public static LocString NAME = "DECOR OVERLAY";

				// Token: 0x0400BAE2 RID: 47842
				public static LocString BUTTON = "Decor Overlay";

				// Token: 0x0400BAE3 RID: 47843
				public static LocString TOTAL = "Total Decor: ";

				// Token: 0x0400BAE4 RID: 47844
				public static LocString ENTRY = "{0} {1} {2}";

				// Token: 0x0400BAE5 RID: 47845
				public static LocString COUNT = "({0})";

				// Token: 0x0400BAE6 RID: 47846
				public static LocString VALUE = "{0}{1}";

				// Token: 0x0400BAE7 RID: 47847
				public static LocString VALUE_ZERO = "{0}{1}";

				// Token: 0x0400BAE8 RID: 47848
				public static LocString HEADER_POSITIVE = "Positive Value:";

				// Token: 0x0400BAE9 RID: 47849
				public static LocString HEADER_NEGATIVE = "Negative Value:";

				// Token: 0x0400BAEA RID: 47850
				public static LocString LOWDECOR = "Negative Decor";

				// Token: 0x0400BAEB RID: 47851
				public static LocString HIGHDECOR = "Positive Decor";

				// Token: 0x0400BAEC RID: 47852
				public static LocString CLUTTER = "Debris";

				// Token: 0x0400BAED RID: 47853
				public static LocString LIGHTING = "Lighting";

				// Token: 0x0400BAEE RID: 47854
				public static LocString CLOTHING = "{0}'s Outfit";

				// Token: 0x0400BAEF RID: 47855
				public static LocString CLOTHING_TRAIT_DECORUP = "{0}'s Outfit (Innately Stylish)";

				// Token: 0x0400BAF0 RID: 47856
				public static LocString CLOTHING_TRAIT_DECORDOWN = "{0}'s Outfit (Shabby Dresser)";

				// Token: 0x0400BAF1 RID: 47857
				public static LocString HOVERTITLE = "DECOR";

				// Token: 0x0400BAF2 RID: 47858
				public static LocString MAXIMUM_DECOR = "{0}{1} (Maximum Decor)";

				// Token: 0x02002D67 RID: 11623
				public class TOOLTIPS
				{
					// Token: 0x0400BAF3 RID: 47859
					public static LocString LOWDECOR = string.Concat(new string[]
					{
						"<b>Negative Decor</b>\nArea with insufficient ",
						UI.PRE_KEYWORD,
						"Decor",
						UI.PST_KEYWORD,
						" values\n* Resources on the floor are considered \"debris\" and will decrease decor"
					});

					// Token: 0x0400BAF4 RID: 47860
					public static LocString HIGHDECOR = string.Concat(new string[]
					{
						"<b>Positive Decor</b>\nArea with sufficient ",
						UI.PRE_KEYWORD,
						"Decor",
						UI.PST_KEYWORD,
						" values\n* Lighting and aesthetically pleasing buildings increase decor"
					});
				}
			}

			// Token: 0x02002D68 RID: 11624
			public class PRIORITIES
			{
				// Token: 0x0400BAF5 RID: 47861
				public static LocString NAME = "PRIORITY OVERLAY";

				// Token: 0x0400BAF6 RID: 47862
				public static LocString BUTTON = "Priority Overlay";

				// Token: 0x0400BAF7 RID: 47863
				public static LocString ONE = "1 (Low Urgency)";

				// Token: 0x0400BAF8 RID: 47864
				public static LocString ONE_TOOLTIP = "Priority 1";

				// Token: 0x0400BAF9 RID: 47865
				public static LocString TWO = "2";

				// Token: 0x0400BAFA RID: 47866
				public static LocString TWO_TOOLTIP = "Priority 2";

				// Token: 0x0400BAFB RID: 47867
				public static LocString THREE = "3";

				// Token: 0x0400BAFC RID: 47868
				public static LocString THREE_TOOLTIP = "Priority 3";

				// Token: 0x0400BAFD RID: 47869
				public static LocString FOUR = "4";

				// Token: 0x0400BAFE RID: 47870
				public static LocString FOUR_TOOLTIP = "Priority 4";

				// Token: 0x0400BAFF RID: 47871
				public static LocString FIVE = "5";

				// Token: 0x0400BB00 RID: 47872
				public static LocString FIVE_TOOLTIP = "Priority 5";

				// Token: 0x0400BB01 RID: 47873
				public static LocString SIX = "6";

				// Token: 0x0400BB02 RID: 47874
				public static LocString SIX_TOOLTIP = "Priority 6";

				// Token: 0x0400BB03 RID: 47875
				public static LocString SEVEN = "7";

				// Token: 0x0400BB04 RID: 47876
				public static LocString SEVEN_TOOLTIP = "Priority 7";

				// Token: 0x0400BB05 RID: 47877
				public static LocString EIGHT = "8";

				// Token: 0x0400BB06 RID: 47878
				public static LocString EIGHT_TOOLTIP = "Priority 8";

				// Token: 0x0400BB07 RID: 47879
				public static LocString NINE = "9 (High Urgency)";

				// Token: 0x0400BB08 RID: 47880
				public static LocString NINE_TOOLTIP = "Priority 9";
			}

			// Token: 0x02002D69 RID: 11625
			public class DISEASE
			{
				// Token: 0x0400BB09 RID: 47881
				public static LocString NAME = "GERM OVERLAY";

				// Token: 0x0400BB0A RID: 47882
				public static LocString BUTTON = "Germ Overlay";

				// Token: 0x0400BB0B RID: 47883
				public static LocString HOVERTITLE = "Germ";

				// Token: 0x0400BB0C RID: 47884
				public static LocString INFECTION_SOURCE = "Germ Source";

				// Token: 0x0400BB0D RID: 47885
				public static LocString INFECTION_SOURCE_TOOLTIP = "<b>Germ Source</b>\nAreas where germs are produced\n•  Placing Wash Basins or Hand Sanitizers near these areas may prevent disease spread";

				// Token: 0x0400BB0E RID: 47886
				public static LocString NO_DISEASE = "Zero surface germs";

				// Token: 0x0400BB0F RID: 47887
				public static LocString DISEASE_NAME_FORMAT = "{0}<color=#{1}></color>";

				// Token: 0x0400BB10 RID: 47888
				public static LocString DISEASE_NAME_FORMAT_NO_COLOR = "{0}";

				// Token: 0x0400BB11 RID: 47889
				public static LocString DISEASE_FORMAT = "{1} [{0}]<color=#{2}></color>";

				// Token: 0x0400BB12 RID: 47890
				public static LocString DISEASE_FORMAT_NO_COLOR = "{1} [{0}]";

				// Token: 0x0400BB13 RID: 47891
				public static LocString CONTAINER_FORMAT = "\n    {0}: {1}";

				// Token: 0x02002D6A RID: 11626
				public class DISINFECT_THRESHOLD_DIAGRAM
				{
					// Token: 0x0400BB14 RID: 47892
					public static LocString UNITS = "Germs";

					// Token: 0x0400BB15 RID: 47893
					public static LocString MIN_LABEL = "0";

					// Token: 0x0400BB16 RID: 47894
					public static LocString MAX_LABEL = "1m";

					// Token: 0x0400BB17 RID: 47895
					public static LocString THRESHOLD_PREFIX = "Disinfect At:";

					// Token: 0x0400BB18 RID: 47896
					public static LocString TOOLTIP = "Automatically disinfect any building with more than {NumberOfGerms} germs.";

					// Token: 0x0400BB19 RID: 47897
					public static LocString TOOLTIP_DISABLED = "Automatic building disinfection disabled.";
				}
			}

			// Token: 0x02002D6B RID: 11627
			public class CROPS
			{
				// Token: 0x0400BB1A RID: 47898
				public static LocString NAME = "FARMING OVERLAY";

				// Token: 0x0400BB1B RID: 47899
				public static LocString BUTTON = "Farming Overlay";
			}

			// Token: 0x02002D6C RID: 11628
			public class POWER
			{
				// Token: 0x0400BB1C RID: 47900
				public static LocString WATTS_GENERATED = "Watts Generated";

				// Token: 0x0400BB1D RID: 47901
				public static LocString WATTS_CONSUMED = "Watts Consumed";
			}

			// Token: 0x02002D6D RID: 11629
			public class RADIATION
			{
				// Token: 0x0400BB1E RID: 47902
				public static LocString NAME = "RADIATION";

				// Token: 0x0400BB1F RID: 47903
				public static LocString BUTTON = "Radiation Overlay";

				// Token: 0x0400BB20 RID: 47904
				public static LocString DESC = "{rads} per cycle ({description})";

				// Token: 0x0400BB21 RID: 47905
				public static LocString SHIELDING_DESC = "Radiation Blocking: {radiationAbsorptionFactor}";

				// Token: 0x0400BB22 RID: 47906
				public static LocString HOVERTITLE = "RADIATION";

				// Token: 0x02002D6E RID: 11630
				public class RANGES
				{
					// Token: 0x0400BB23 RID: 47907
					public static LocString NONE = "Completely Safe";

					// Token: 0x0400BB24 RID: 47908
					public static LocString VERY_LOW = "Mostly Safe";

					// Token: 0x0400BB25 RID: 47909
					public static LocString LOW = "Barely Safe";

					// Token: 0x0400BB26 RID: 47910
					public static LocString MEDIUM = "Slight Hazard";

					// Token: 0x0400BB27 RID: 47911
					public static LocString HIGH = "Significant Hazard";

					// Token: 0x0400BB28 RID: 47912
					public static LocString VERY_HIGH = "Extreme Hazard";

					// Token: 0x0400BB29 RID: 47913
					public static LocString MAX = "Maximum Hazard";

					// Token: 0x0400BB2A RID: 47914
					public static LocString INPUTPORT = "Radbolt Input Port";

					// Token: 0x0400BB2B RID: 47915
					public static LocString OUTPUTPORT = "Radbolt Output Port";
				}

				// Token: 0x02002D6F RID: 11631
				public class TOOLTIPS
				{
					// Token: 0x0400BB2C RID: 47916
					public static LocString NONE = "Completely Safe";

					// Token: 0x0400BB2D RID: 47917
					public static LocString VERY_LOW = "Mostly Safe";

					// Token: 0x0400BB2E RID: 47918
					public static LocString LOW = "Barely Safe";

					// Token: 0x0400BB2F RID: 47919
					public static LocString MEDIUM = "Slight Hazard";

					// Token: 0x0400BB30 RID: 47920
					public static LocString HIGH = "Significant Hazard";

					// Token: 0x0400BB31 RID: 47921
					public static LocString VERY_HIGH = "Extreme Hazard";

					// Token: 0x0400BB32 RID: 47922
					public static LocString MAX = "Maximum Hazard";

					// Token: 0x0400BB33 RID: 47923
					public static LocString INPUTPORT = "Radbolt Input Port";

					// Token: 0x0400BB34 RID: 47924
					public static LocString OUTPUTPORT = "Radbolt Output Port";
				}
			}
		}

		// Token: 0x02002D70 RID: 11632
		public class TABLESCREENS
		{
			// Token: 0x0400BB35 RID: 47925
			public static LocString DUPLICANT_PROPERNAME = "<b>{0}</b>";

			// Token: 0x0400BB36 RID: 47926
			public static LocString SELECT_DUPLICANT_BUTTON = UI.CLICK(UI.ClickType.Click) + " to select <b>{0}</b>";

			// Token: 0x0400BB37 RID: 47927
			public static LocString GOTO_DUPLICANT_BUTTON = "Double-" + UI.CLICK(UI.ClickType.click) + " to go to <b>{0}</b>";

			// Token: 0x0400BB38 RID: 47928
			public static LocString COLUMN_SORT_BY_NAME = "Sort by <b>Name</b>";

			// Token: 0x0400BB39 RID: 47929
			public static LocString COLUMN_SORT_BY_STRESS = "Sort by <b>Stress</b>";

			// Token: 0x0400BB3A RID: 47930
			public static LocString COLUMN_SORT_BY_HITPOINTS = "Sort by <b>Health</b>";

			// Token: 0x0400BB3B RID: 47931
			public static LocString COLUMN_SORT_BY_SICKNESSES = "Sort by <b>Disease</b>";

			// Token: 0x0400BB3C RID: 47932
			public static LocString COLUMN_SORT_BY_FULLNESS = "Sort by <b>Fullness</b>";

			// Token: 0x0400BB3D RID: 47933
			public static LocString COLUMN_SORT_BY_EATEN_TODAY = "Sort by number of <b>Calories</b> consumed today";

			// Token: 0x0400BB3E RID: 47934
			public static LocString COLUMN_SORT_BY_EXPECTATIONS = "Sort by <b>Morale</b>";

			// Token: 0x0400BB3F RID: 47935
			public static LocString NA = "N/A";

			// Token: 0x0400BB40 RID: 47936
			public static LocString INFORMATION_NOT_AVAILABLE_TOOLTIP = "Information is not available because {1} is in {0}";

			// Token: 0x0400BB41 RID: 47937
			public static LocString NOBODY_HERE = "Nobody here...";
		}

		// Token: 0x02002D71 RID: 11633
		public class CONSUMABLESSCREEN
		{
			// Token: 0x0400BB42 RID: 47938
			public static LocString TITLE = "CONSUMABLES";

			// Token: 0x0400BB43 RID: 47939
			public static LocString TOOLTIP_TOGGLE_ALL = "Toggle <b>all</b> food permissions <b>colonywide</b>";

			// Token: 0x0400BB44 RID: 47940
			public static LocString TOOLTIP_TOGGLE_COLUMN = "Toggle colonywide <b>{0}</b> permission";

			// Token: 0x0400BB45 RID: 47941
			public static LocString TOOLTIP_TOGGLE_ROW = "Toggle <b>all consumable permissions</b> for <b>{0}</b>";

			// Token: 0x0400BB46 RID: 47942
			public static LocString NEW_MINIONS_TOOLTIP_TOGGLE_ROW = "Toggle <b>all consumable permissions</b> for <b>New Duplicants</b>";

			// Token: 0x0400BB47 RID: 47943
			public static LocString NEW_MINIONS_FOOD_PERMISSION_ON = string.Concat(new string[]
			{
				"<b>New Duplicants</b> are <b>allowed</b> to eat \n",
				UI.PRE_KEYWORD,
				"{0}",
				UI.PST_KEYWORD,
				"</b> by default"
			});

			// Token: 0x0400BB48 RID: 47944
			public static LocString NEW_MINIONS_FOOD_PERMISSION_OFF = string.Concat(new string[]
			{
				"<b>New Duplicants</b> are <b>not allowed</b> to eat \n",
				UI.PRE_KEYWORD,
				"{0}",
				UI.PST_KEYWORD,
				" by default"
			});

			// Token: 0x0400BB49 RID: 47945
			public static LocString FOOD_PERMISSION_ON = "<b>{0}</b> is <b>allowed</b> to eat " + UI.PRE_KEYWORD + "{1}" + UI.PST_KEYWORD;

			// Token: 0x0400BB4A RID: 47946
			public static LocString FOOD_PERMISSION_OFF = "<b>{0}</b> is <b>not allowed</b> to eat " + UI.PRE_KEYWORD + "{1}" + UI.PST_KEYWORD;

			// Token: 0x0400BB4B RID: 47947
			public static LocString FOOD_CANT_CONSUME = "<b>{0}</b> <b>physically cannot</b> eat\n" + UI.PRE_KEYWORD + "{1}" + UI.PST_KEYWORD;

			// Token: 0x0400BB4C RID: 47948
			public static LocString FOOD_REFUSE = "<b>{0}</b> <b>refuses</b> to eat\n" + UI.PRE_KEYWORD + "{1}" + UI.PST_KEYWORD;

			// Token: 0x0400BB4D RID: 47949
			public static LocString FOOD_AVAILABLE = "Available: {0}";

			// Token: 0x0400BB4E RID: 47950
			public static LocString FOOD_MORALE = UI.PRE_KEYWORD + "Morale" + UI.PST_KEYWORD + ": {0}";

			// Token: 0x0400BB4F RID: 47951
			public static LocString FOOD_QUALITY = UI.PRE_KEYWORD + "Quality" + UI.PST_KEYWORD + ": {0}";

			// Token: 0x0400BB50 RID: 47952
			public static LocString FOOD_QUALITY_VS_EXPECTATION = string.Concat(new string[]
			{
				"\nThis food will give ",
				UI.PRE_KEYWORD,
				"Morale",
				UI.PST_KEYWORD,
				" <b>{0}</b> if {1} eats it"
			});

			// Token: 0x0400BB51 RID: 47953
			public static LocString CANNOT_ADJUST_PERMISSIONS = "Cannot adjust consumable permissions because they're in {0}";
		}

		// Token: 0x02002D72 RID: 11634
		public class JOBSSCREEN
		{
			// Token: 0x0400BB52 RID: 47954
			public static LocString TITLE = "MANAGE DUPLICANT PRIORITIES";

			// Token: 0x0400BB53 RID: 47955
			public static LocString TOOLTIP_TOGGLE_ALL = "Set priority of all Errand Types colonywide";

			// Token: 0x0400BB54 RID: 47956
			public static LocString HEADER_TOOLTIP = string.Concat(new string[]
			{
				"<size=16>{Job} Errand Type</size>\n\n{Details}\n\nDuplicants will first choose what ",
				UI.PRE_KEYWORD,
				"Errand Type",
				UI.PST_KEYWORD,
				" to perform based on ",
				UI.PRE_KEYWORD,
				"Duplicant Priorities",
				UI.PST_KEYWORD,
				",\nthen they will choose individual tasks within that type using ",
				UI.PRE_KEYWORD,
				"Building Priorities",
				UI.PST_KEYWORD,
				" set by the ",
				UI.FormatAsLink("Priority Tool", "PRIORITIES"),
				" ",
				UI.FormatAsHotKey(global::Action.ManagePriorities)
			});

			// Token: 0x0400BB55 RID: 47957
			public static LocString HEADER_DETAILS_TOOLTIP = "{Description}\n\nAffected errands: {ChoreList}";

			// Token: 0x0400BB56 RID: 47958
			public static LocString HEADER_CHANGE_TOOLTIP = string.Concat(new string[]
			{
				"Set the priority for the ",
				UI.PRE_KEYWORD,
				"{Job}",
				UI.PST_KEYWORD,
				" Errand Type colonywide\n"
			});

			// Token: 0x0400BB57 RID: 47959
			public static LocString NEW_MINION_ITEM_TOOLTIP = string.Concat(new string[]
			{
				"The ",
				UI.PRE_KEYWORD,
				"{Job}",
				UI.PST_KEYWORD,
				" Errand Type is automatically a {Priority} ",
				UI.PRE_KEYWORD,
				"Priority",
				UI.PST_KEYWORD,
				" for <b>Arriving Duplicants</b>"
			});

			// Token: 0x0400BB58 RID: 47960
			public static LocString ITEM_TOOLTIP = UI.PRE_KEYWORD + "{Job}" + UI.PST_KEYWORD + " Priority for {Name}:\n<b>{Priority} Priority ({PriorityValue})</b>";

			// Token: 0x0400BB59 RID: 47961
			public static LocString MINION_SKILL_TOOLTIP = string.Concat(new string[]
			{
				"{Name}'s ",
				UI.PRE_KEYWORD,
				"{Attribute}",
				UI.PST_KEYWORD,
				" Skill: "
			});

			// Token: 0x0400BB5A RID: 47962
			public static LocString TRAIT_DISABLED = string.Concat(new string[]
			{
				"{Name} possesses the ",
				UI.PRE_KEYWORD,
				"{Trait}",
				UI.PST_KEYWORD,
				" trait and <b>cannot</b> do ",
				UI.PRE_KEYWORD,
				"{Job}",
				UI.PST_KEYWORD,
				" Errands"
			});

			// Token: 0x0400BB5B RID: 47963
			public static LocString INCREASE_ROW_PRIORITY_NEW_MINION_TOOLTIP = string.Concat(new string[]
			{
				"Prioritize ",
				UI.PRE_KEYWORD,
				"All Errands",
				UI.PST_KEYWORD,
				" for <b>New Duplicants</b>"
			});

			// Token: 0x0400BB5C RID: 47964
			public static LocString DECREASE_ROW_PRIORITY_NEW_MINION_TOOLTIP = string.Concat(new string[]
			{
				"Deprioritize ",
				UI.PRE_KEYWORD,
				"All Errands",
				UI.PST_KEYWORD,
				" for <b>New Duplicants</b>"
			});

			// Token: 0x0400BB5D RID: 47965
			public static LocString INCREASE_ROW_PRIORITY_MINION_TOOLTIP = string.Concat(new string[]
			{
				"Prioritize ",
				UI.PRE_KEYWORD,
				"All Errands",
				UI.PST_KEYWORD,
				" for <b>{Name}</b>"
			});

			// Token: 0x0400BB5E RID: 47966
			public static LocString DECREASE_ROW_PRIORITY_MINION_TOOLTIP = string.Concat(new string[]
			{
				"Deprioritize ",
				UI.PRE_KEYWORD,
				"All Errands",
				UI.PST_KEYWORD,
				" for <b>{Name}</b>"
			});

			// Token: 0x0400BB5F RID: 47967
			public static LocString INCREASE_PRIORITY_TUTORIAL = "{Hotkey} Increase Priority";

			// Token: 0x0400BB60 RID: 47968
			public static LocString DECREASE_PRIORITY_TUTORIAL = "{Hotkey} Decrease Priority";

			// Token: 0x0400BB61 RID: 47969
			public static LocString CANNOT_ADJUST_PRIORITY = string.Concat(new string[]
			{
				"Priorities for ",
				UI.PRE_KEYWORD,
				"{0}",
				UI.PST_KEYWORD,
				" cannot be adjusted currently because they're in {1}"
			});

			// Token: 0x0400BB62 RID: 47970
			public static LocString SORT_TOOLTIP = string.Concat(new string[]
			{
				"Sort by the ",
				UI.PRE_KEYWORD,
				"{Job}",
				UI.PST_KEYWORD,
				" Errand Type"
			});

			// Token: 0x0400BB63 RID: 47971
			public static LocString DISABLED_TOOLTIP = string.Concat(new string[]
			{
				"{Name} may not perform ",
				UI.PRE_KEYWORD,
				"{Job}",
				UI.PST_KEYWORD,
				" Errands"
			});

			// Token: 0x0400BB64 RID: 47972
			public static LocString OPTIONS = "Options";

			// Token: 0x0400BB65 RID: 47973
			public static LocString TOGGLE_ADVANCED_MODE = "Enable Proximity";

			// Token: 0x0400BB66 RID: 47974
			public static LocString TOGGLE_ADVANCED_MODE_TOOLTIP = "<b>Errand Proximity Settings</b>\n\nEnabling Proximity settings tells my Duplicants to always choose the closest, most urgent errand to perform.\n\nWhen disabled, Duplicants will choose between two high priority errands based on a hidden priority hierarchy instead.\n\nEnabling Proximity helps cut down on travel time in areas with lots of high priority errands, and is useful for large colonies.";

			// Token: 0x0400BB67 RID: 47975
			public static LocString RESET_SETTINGS = "Reset Priorities";

			// Token: 0x0400BB68 RID: 47976
			public static LocString RESET_SETTINGS_TOOLTIP = "<b>Reset Priorities</b>\n\nReturns all priorities to their default values.\n\nProximity Enabled: Priorities will be adjusted high-to-low.\n\nProximity Disabled: All priorities will be reset to neutral.";

			// Token: 0x02002D73 RID: 11635
			public class PRIORITY
			{
				// Token: 0x0400BB69 RID: 47977
				public static LocString VERYHIGH = "Very High";

				// Token: 0x0400BB6A RID: 47978
				public static LocString HIGH = "High";

				// Token: 0x0400BB6B RID: 47979
				public static LocString STANDARD = "Standard";

				// Token: 0x0400BB6C RID: 47980
				public static LocString LOW = "Low";

				// Token: 0x0400BB6D RID: 47981
				public static LocString VERYLOW = "Very Low";

				// Token: 0x0400BB6E RID: 47982
				public static LocString DISABLED = "Disallowed";
			}

			// Token: 0x02002D74 RID: 11636
			public class PRIORITY_CLASS
			{
				// Token: 0x0400BB6F RID: 47983
				public static LocString IDLE = "Idle";

				// Token: 0x0400BB70 RID: 47984
				public static LocString BASIC = "Normal";

				// Token: 0x0400BB71 RID: 47985
				public static LocString HIGH = "Urgent";

				// Token: 0x0400BB72 RID: 47986
				public static LocString PERSONAL_NEEDS = "Personal Needs";

				// Token: 0x0400BB73 RID: 47987
				public static LocString EMERGENCY = "Emergency";

				// Token: 0x0400BB74 RID: 47988
				public static LocString COMPULSORY = "Involuntary";
			}
		}

		// Token: 0x02002D75 RID: 11637
		public class VITALSSCREEN
		{
			// Token: 0x0400BB75 RID: 47989
			public static LocString HEALTH = "Health";

			// Token: 0x0400BB76 RID: 47990
			public static LocString SICKNESS = "Diseases";

			// Token: 0x0400BB77 RID: 47991
			public static LocString NO_SICKNESSES = "No diseases";

			// Token: 0x0400BB78 RID: 47992
			public static LocString MULTIPLE_SICKNESSES = "Multiple diseases ({0})";

			// Token: 0x0400BB79 RID: 47993
			public static LocString SICKNESS_REMAINING = "{0}\n({1})";

			// Token: 0x0400BB7A RID: 47994
			public static LocString STRESS = "Stress";

			// Token: 0x0400BB7B RID: 47995
			public static LocString EXPECTATIONS = "Expectations";

			// Token: 0x0400BB7C RID: 47996
			public static LocString CALORIES = "Fullness";

			// Token: 0x0400BB7D RID: 47997
			public static LocString EATEN_TODAY = "Eaten Today";

			// Token: 0x0400BB7E RID: 47998
			public static LocString EATEN_TODAY_TOOLTIP = "Consumed {0} of food this cycle";

			// Token: 0x0400BB7F RID: 47999
			public static LocString ATMOSPHERE_CONDITION = "Atmosphere:";

			// Token: 0x0400BB80 RID: 48000
			public static LocString SUBMERSION = "Liquid Level";

			// Token: 0x0400BB81 RID: 48001
			public static LocString NOT_DROWNING = "Liquid Level";

			// Token: 0x0400BB82 RID: 48002
			public static LocString FOOD_EXPECTATIONS = "Food Expectation";

			// Token: 0x0400BB83 RID: 48003
			public static LocString FOOD_EXPECTATIONS_TOOLTIP = "This Duplicant desires food that is {0} quality or better";

			// Token: 0x0400BB84 RID: 48004
			public static LocString DECOR_EXPECTATIONS = "Decor Expectation";

			// Token: 0x0400BB85 RID: 48005
			public static LocString DECOR_EXPECTATIONS_TOOLTIP = "This Duplicant desires decor that is {0} or higher";

			// Token: 0x0400BB86 RID: 48006
			public static LocString QUALITYOFLIFE_EXPECTATIONS = "Morale";

			// Token: 0x0400BB87 RID: 48007
			public static LocString QUALITYOFLIFE_EXPECTATIONS_TOOLTIP = "This Duplicant requires " + UI.FormatAsLink("{0} Morale", "MORALE") + ".\n\nCurrent Morale:";

			// Token: 0x02002D76 RID: 11638
			public class CONDITIONS_GROWING
			{
				// Token: 0x02002D77 RID: 11639
				public class WILD
				{
					// Token: 0x0400BB88 RID: 48008
					public static LocString BASE = "<b>Wild Growth\n[Life Cycle: {0}]</b>";

					// Token: 0x0400BB89 RID: 48009
					public static LocString TOOLTIP = "This plant will take {0} to grow in the wild";
				}

				// Token: 0x02002D78 RID: 11640
				public class DOMESTIC
				{
					// Token: 0x0400BB8A RID: 48010
					public static LocString BASE = "<b>Domestic Growth\n[Life Cycle: {0}]</b>";

					// Token: 0x0400BB8B RID: 48011
					public static LocString TOOLTIP = "This plant will take {0} to grow domestically";
				}

				// Token: 0x02002D79 RID: 11641
				public class ADDITIONAL_DOMESTIC
				{
					// Token: 0x0400BB8C RID: 48012
					public static LocString BASE = "<b>Additional Domestic Growth\n[Life Cycle: {0}]</b>";

					// Token: 0x0400BB8D RID: 48013
					public static LocString TOOLTIP = "This plant will take {0} to grow domestically";
				}

				// Token: 0x02002D7A RID: 11642
				public class WILD_DECOR
				{
					// Token: 0x0400BB8E RID: 48014
					public static LocString BASE = "<b>Wild Growth</b>";

					// Token: 0x0400BB8F RID: 48015
					public static LocString TOOLTIP = "This plant must have these requirements met to grow in the wild";
				}

				// Token: 0x02002D7B RID: 11643
				public class WILD_INSTANT
				{
					// Token: 0x0400BB90 RID: 48016
					public static LocString BASE = "<b>Wild Growth\n[{0}% Throughput]</b>";

					// Token: 0x0400BB91 RID: 48017
					public static LocString TOOLTIP = "This plant must have these requirements met to grow in the wild";
				}

				// Token: 0x02002D7C RID: 11644
				public class ADDITIONAL_DOMESTIC_INSTANT
				{
					// Token: 0x0400BB92 RID: 48018
					public static LocString BASE = "<b>Domestic Growth\n[{0}% Throughput]</b>";

					// Token: 0x0400BB93 RID: 48019
					public static LocString TOOLTIP = "This plant must have these requirements met to grow domestically";
				}
			}
		}

		// Token: 0x02002D7D RID: 11645
		public class SCHEDULESCREEN
		{
			// Token: 0x0400BB94 RID: 48020
			public static LocString SCHEDULE_EDITOR = "SCHEDULE EDITOR";

			// Token: 0x0400BB95 RID: 48021
			public static LocString SCHEDULE_NAME_DEFAULT = "Default Schedule";

			// Token: 0x0400BB96 RID: 48022
			public static LocString SCHEDULE_NAME_FORMAT = "Schedule {0}";

			// Token: 0x0400BB97 RID: 48023
			public static LocString SCHEDULE_DROPDOWN_ASSIGNED = "{0} (Assigned)";

			// Token: 0x0400BB98 RID: 48024
			public static LocString SCHEDULE_DROPDOWN_BLANK = "<i>Move Duplicant...</i>";

			// Token: 0x0400BB99 RID: 48025
			public static LocString SCHEDULE_DOWNTIME_MORALE = "Duplicants will receive {0} Morale from the scheduled Downtime shifts";

			// Token: 0x0400BB9A RID: 48026
			public static LocString RENAME_BUTTON_TOOLTIP = "Rename custom schedule";

			// Token: 0x0400BB9B RID: 48027
			public static LocString ALARM_BUTTON_ON_TOOLTIP = "Toggle Notifications\n\nSounds and notifications will play when shifts change for this schedule.\n\nENABLED\n" + UI.CLICK(UI.ClickType.Click) + " to disable";

			// Token: 0x0400BB9C RID: 48028
			public static LocString ALARM_BUTTON_OFF_TOOLTIP = "Toggle Notifications\n\nNo sounds or notifications will play for this schedule.\n\nDISABLED\n" + UI.CLICK(UI.ClickType.Click) + " to enable";

			// Token: 0x0400BB9D RID: 48029
			public static LocString DELETE_BUTTON_TOOLTIP = "Delete Schedule";

			// Token: 0x0400BB9E RID: 48030
			public static LocString PAINT_TOOLS = "Paint Tools:";

			// Token: 0x0400BB9F RID: 48031
			public static LocString ADD_SCHEDULE = "Add New Schedule";

			// Token: 0x0400BBA0 RID: 48032
			public static LocString POO = "dar";

			// Token: 0x0400BBA1 RID: 48033
			public static LocString DOWNTIME_MORALE = "Downtime Morale: {0}";

			// Token: 0x0400BBA2 RID: 48034
			public static LocString ALARM_TITLE_ENABLED = "Alarm On";

			// Token: 0x0400BBA3 RID: 48035
			public static LocString ALARM_TITLE_DISABLED = "Alarm Off";

			// Token: 0x0400BBA4 RID: 48036
			public static LocString SETTINGS = "Settings";

			// Token: 0x0400BBA5 RID: 48037
			public static LocString ALARM_BUTTON = "Shift Alarms";

			// Token: 0x0400BBA6 RID: 48038
			public static LocString RESET_SETTINGS = "Reset Shifts";

			// Token: 0x0400BBA7 RID: 48039
			public static LocString RESET_SETTINGS_TOOLTIP = "Restore this schedule to default shifts";

			// Token: 0x0400BBA8 RID: 48040
			public static LocString DELETE_SCHEDULE = "Delete Schedule";

			// Token: 0x0400BBA9 RID: 48041
			public static LocString DELETE_SCHEDULE_TOOLTIP = "Remove this schedule and unassign all Duplicants from it";

			// Token: 0x0400BBAA RID: 48042
			public static LocString DUPLICANT_NIGHTOWL_TOOLTIP = string.Concat(new string[]
			{
				DUPLICANTS.TRAITS.NIGHTOWL.NAME,
				"\n• All ",
				UI.PRE_KEYWORD,
				"Attributes",
				UI.PST_KEYWORD,
				" <b>+3</b> at night"
			});

			// Token: 0x0400BBAB RID: 48043
			public static LocString DUPLICANT_EARLYBIRD_TOOLTIP = string.Concat(new string[]
			{
				DUPLICANTS.TRAITS.EARLYBIRD.NAME,
				"\n• All ",
				UI.PRE_KEYWORD,
				"Attributes",
				UI.PST_KEYWORD,
				" <b>+2</b> in the morning"
			});

			// Token: 0x0400BBAC RID: 48044
			public static LocString SHIFT_SCHEDULE_LEFT_TOOLTIP = "Shift all schedule blocks left";

			// Token: 0x0400BBAD RID: 48045
			public static LocString SHIFT_SCHEDULE_RIGHT_TOOLTIP = "Shift all schedule blocks right";

			// Token: 0x0400BBAE RID: 48046
			public static LocString SHIFT_SCHEDULE_UP_TOOLTIP = "Swap this row with the one above it";

			// Token: 0x0400BBAF RID: 48047
			public static LocString SHIFT_SCHEDULE_DOWN_TOOLTIP = "Swap this row with the one below it";

			// Token: 0x0400BBB0 RID: 48048
			public static LocString DUPLICATE_SCHEDULE_TIMETABLE = "Duplicate this row";

			// Token: 0x0400BBB1 RID: 48049
			public static LocString DELETE_SCHEDULE_TIMETABLE = "Delete this row\n\nSchedules must have two or more rows in order for one row to be deleted";

			// Token: 0x0400BBB2 RID: 48050
			public static LocString DUPLICATE_SCHEDULE = "Duplicate this schedule";
		}

		// Token: 0x02002D7E RID: 11646
		public class COLONYLOSTSCREEN
		{
			// Token: 0x0400BBB3 RID: 48051
			public static LocString COLONYLOST = "COLONY LOST";

			// Token: 0x0400BBB4 RID: 48052
			public static LocString COLONYLOSTDESCRIPTION = "All Duplicants are dead or incapacitated.";

			// Token: 0x0400BBB5 RID: 48053
			public static LocString RESTARTPROMPT = "Press <color=#F44A47><b>[ESC]</b></color> to return to a previous colony, or begin a new one.";

			// Token: 0x0400BBB6 RID: 48054
			public static LocString DISMISSBUTTON = "DISMISS";

			// Token: 0x0400BBB7 RID: 48055
			public static LocString QUITBUTTON = "MAIN MENU";
		}

		// Token: 0x02002D7F RID: 11647
		public class VICTORYSCREEN
		{
			// Token: 0x0400BBB8 RID: 48056
			public static LocString HEADER = "SUCCESS: IMPERATIVE ACHIEVED!";

			// Token: 0x0400BBB9 RID: 48057
			public static LocString DESCRIPTION = "I have fulfilled the conditions of one of my Hardwired Imperatives";

			// Token: 0x0400BBBA RID: 48058
			public static LocString RESTARTPROMPT = "Press <color=#F44A47><b>[ESC]</b></color> to retire the colony and begin anew.";

			// Token: 0x0400BBBB RID: 48059
			public static LocString DISMISSBUTTON = "DISMISS";

			// Token: 0x0400BBBC RID: 48060
			public static LocString RETIREBUTTON = "RETIRE COLONY";
		}

		// Token: 0x02002D80 RID: 11648
		public class GENESHUFFLERMESSAGE
		{
			// Token: 0x0400BBBD RID: 48061
			public static LocString HEADER = "NEURAL VACILLATION COMPLETE";

			// Token: 0x0400BBBE RID: 48062
			public static LocString BODY_SUCCESS = "Whew! <b>{0}'s</b> brain is still vibrating, but they've never felt better!\n\n<b>{0}</b> acquired the <b>{1}</b> trait.\n\n<b>{1}:</b>\n{2}";

			// Token: 0x0400BBBF RID: 48063
			public static LocString BODY_FAILURE = "The machine attempted to alter this Duplicant, but there's no improving on perfection.\n\n<b>{0}</b> already has all positive traits!";

			// Token: 0x0400BBC0 RID: 48064
			public static LocString DISMISSBUTTON = "DISMISS";
		}

		// Token: 0x02002D81 RID: 11649
		public class CRASHSCREEN
		{
			// Token: 0x0400BBC1 RID: 48065
			public static LocString TITLE = "\"Whoops! We're sorry, but it seems your game has encountered an error. It's okay though - these errors are how we find and fix problems to make our game more fun for everyone. If you use the box below to submit a crash report to us, we can use this information to get the issue sorted out.\"";

			// Token: 0x0400BBC2 RID: 48066
			public static LocString TITLE_MODS = "\"Oops-a-daisy! We're sorry, but it seems your game has encountered an error. If you uncheck all of the mods below, we will be able to help the next time this happens. Any mods that could be related to this error have already been unchecked.\"";

			// Token: 0x0400BBC3 RID: 48067
			public static LocString HEADER = "OPTIONAL CRASH DESCRIPTION";

			// Token: 0x0400BBC4 RID: 48068
			public static LocString HEADER_MODS = "ACTIVE MODS";

			// Token: 0x0400BBC5 RID: 48069
			public static LocString BODY = "Help! A black hole ate my game!";

			// Token: 0x0400BBC6 RID: 48070
			public static LocString THANKYOU = "Thank you!\n\nYou're making our game better, one crash at a time.";

			// Token: 0x0400BBC7 RID: 48071
			public static LocString UPLOAD_FAILED = "There was an issue in reporting this crash.\n\nPlease submit a bug report at:\n<u>https://forums.kleientertainment.com/klei-bug-tracker/oni/</u>";

			// Token: 0x0400BBC8 RID: 48072
			public static LocString UPLOADINFO = "UPLOAD ADDITIONAL INFO ({0})";

			// Token: 0x0400BBC9 RID: 48073
			public static LocString REPORTBUTTON = "REPORT CRASH";

			// Token: 0x0400BBCA RID: 48074
			public static LocString REPORTING = "REPORTING, PLEASE WAIT...";

			// Token: 0x0400BBCB RID: 48075
			public static LocString CONTINUEBUTTON = "CONTINUE GAME";

			// Token: 0x0400BBCC RID: 48076
			public static LocString MOREINFOBUTTON = "MORE INFO";

			// Token: 0x0400BBCD RID: 48077
			public static LocString COPYTOCLIPBOARDBUTTON = "COPY TO CLIPBOARD";

			// Token: 0x0400BBCE RID: 48078
			public static LocString QUITBUTTON = "QUIT TO DESKTOP";

			// Token: 0x0400BBCF RID: 48079
			public static LocString SAVEFAILED = "Save Failed: {0}";

			// Token: 0x0400BBD0 RID: 48080
			public static LocString LOADFAILED = "Load Failed: {0}\nSave Version: {1}\nExpected: {2}";

			// Token: 0x0400BBD1 RID: 48081
			public static LocString REPORTEDERROR_SUCCESS = "Thank you for reporting this error.";

			// Token: 0x0400BBD2 RID: 48082
			public static LocString REPORTEDERROR_FAILURE_TOO_LARGE = "Unable to report error. Save file is too large. Please contact us using the bug tracker.";

			// Token: 0x0400BBD3 RID: 48083
			public static LocString REPORTEDERROR_FAILURE = "Unable to report error. Please contact us using the bug tracker.";

			// Token: 0x0400BBD4 RID: 48084
			public static LocString UPLOADINPROGRESS = "Submitting {0}";
		}

		// Token: 0x02002D82 RID: 11650
		public class DEMOOVERSCREEN
		{
			// Token: 0x0400BBD5 RID: 48085
			public static LocString TIMEREMAINING = "Demo time remaining:";

			// Token: 0x0400BBD6 RID: 48086
			public static LocString TIMERTOOLTIP = "Demo time remaining";

			// Token: 0x0400BBD7 RID: 48087
			public static LocString TIMERINACTIVE = "Timer inactive";

			// Token: 0x0400BBD8 RID: 48088
			public static LocString DEMOOVER = "END OF DEMO";

			// Token: 0x0400BBD9 RID: 48089
			public static LocString DESCRIPTION = "Thank you for playing <color=#F44A47>Oxygen Not Included</color>!";

			// Token: 0x0400BBDA RID: 48090
			public static LocString DESCRIPTION_2 = "";

			// Token: 0x0400BBDB RID: 48091
			public static LocString QUITBUTTON = "RESET";
		}

		// Token: 0x02002D83 RID: 11651
		public class CREDITSSCREEN
		{
			// Token: 0x0400BBDC RID: 48092
			public static LocString TITLE = "CREDITS";

			// Token: 0x0400BBDD RID: 48093
			public static LocString CLOSEBUTTON = "CLOSE";

			// Token: 0x02002D84 RID: 11652
			public class THIRD_PARTY
			{
				// Token: 0x0400BBDE RID: 48094
				public static LocString FMOD = "FMOD Sound System\nCopyright Firelight Technologies";

				// Token: 0x0400BBDF RID: 48095
				public static LocString HARMONY = "Harmony by Andreas Pardeike";
			}
		}

		// Token: 0x02002D85 RID: 11653
		public class ALLRESOURCESSCREEN
		{
			// Token: 0x0400BBE0 RID: 48096
			public static LocString RESOURCES_TITLE = "RESOURCES";

			// Token: 0x0400BBE1 RID: 48097
			public static LocString RESOURCES = "Resources";

			// Token: 0x0400BBE2 RID: 48098
			public static LocString SEARCH = "Search";

			// Token: 0x0400BBE3 RID: 48099
			public static LocString NAME = "Resource";

			// Token: 0x0400BBE4 RID: 48100
			public static LocString TOTAL = "Total";

			// Token: 0x0400BBE5 RID: 48101
			public static LocString AVAILABLE = "Available";

			// Token: 0x0400BBE6 RID: 48102
			public static LocString RESERVED = "Reserved";

			// Token: 0x0400BBE7 RID: 48103
			public static LocString SEARCH_PLACEHODLER = "Enter text...";

			// Token: 0x0400BBE8 RID: 48104
			public static LocString FIRST_FRAME_NO_DATA = "...";

			// Token: 0x0400BBE9 RID: 48105
			public static LocString PIN_TOOLTIP = "Check to pin resource to side panel";

			// Token: 0x0400BBEA RID: 48106
			public static LocString UNPIN_TOOLTIP = "Unpin resource";
		}

		// Token: 0x02002D86 RID: 11654
		public class PRIORITYSCREEN
		{
			// Token: 0x0400BBEB RID: 48107
			public static LocString BASIC = "Set the order in which specific pending errands should be done\n\n1: Least Urgent\n9: Most Urgent";

			// Token: 0x0400BBEC RID: 48108
			public static LocString HIGH = "";

			// Token: 0x0400BBED RID: 48109
			public static LocString TOP_PRIORITY = "Top Priority\n\nThis priority will override all other priorities and set the colony on Yellow Alert until the errand is completed";

			// Token: 0x0400BBEE RID: 48110
			public static LocString HIGH_TOGGLE = "";

			// Token: 0x0400BBEF RID: 48111
			public static LocString OPEN_JOBS_SCREEN = string.Concat(new string[]
			{
				UI.CLICK(UI.ClickType.Click),
				" to open the Priorities Screen\n\nDuplicants will first decide what to work on based on their ",
				UI.PRE_KEYWORD,
				"Duplicant Priorities",
				UI.PST_KEYWORD,
				", and then decide where to work based on ",
				UI.PRE_KEYWORD,
				"Building Priorities",
				UI.PST_KEYWORD
			});

			// Token: 0x0400BBF0 RID: 48112
			public static LocString DIAGRAM = string.Concat(new string[]
			{
				"Duplicants will first choose what ",
				UI.PRE_KEYWORD,
				"Errand Type",
				UI.PST_KEYWORD,
				" to perform using their ",
				UI.PRE_KEYWORD,
				"Duplicant Priorities",
				UI.PST_KEYWORD,
				" ",
				UI.FormatAsHotKey(global::Action.ManagePriorities),
				"\n\nThey will then choose one ",
				UI.PRE_KEYWORD,
				"Errand",
				UI.PST_KEYWORD,
				" from within that type using the ",
				UI.PRE_KEYWORD,
				"Building Priorities",
				UI.PST_KEYWORD,
				" set by this tool"
			});

			// Token: 0x0400BBF1 RID: 48113
			public static LocString DIAGRAM_TITLE = "BUILDING PRIORITY";
		}

		// Token: 0x02002D87 RID: 11655
		public class RESOURCESCREEN
		{
			// Token: 0x0400BBF2 RID: 48114
			public static LocString HEADER = "RESOURCES";

			// Token: 0x0400BBF3 RID: 48115
			public static LocString CATEGORY_TOOLTIP = "Counts all unallocated resources within reach\n\n" + UI.CLICK(UI.ClickType.Click) + " to expand";

			// Token: 0x0400BBF4 RID: 48116
			public static LocString AVAILABLE_TOOLTIP = "Available: <b>{0}</b>\n({1} of {2} allocated to pending errands)";

			// Token: 0x0400BBF5 RID: 48117
			public static LocString TREND_TOOLTIP = "The available amount of this resource has {0} {1} in the last cycle";

			// Token: 0x0400BBF6 RID: 48118
			public static LocString TREND_TOOLTIP_NO_CHANGE = "The available amount of this resource has NOT CHANGED in the last cycle";

			// Token: 0x0400BBF7 RID: 48119
			public static LocString FLAT_STR = "<b>NOT CHANGED</b>";

			// Token: 0x0400BBF8 RID: 48120
			public static LocString INCREASING_STR = "<color=" + Constants.POSITIVE_COLOR_STR + ">INCREASED</color>";

			// Token: 0x0400BBF9 RID: 48121
			public static LocString DECREASING_STR = "<color=" + Constants.NEGATIVE_COLOR_STR + ">DECREASED</color>";

			// Token: 0x0400BBFA RID: 48122
			public static LocString CLEAR_NEW_RESOURCES = "Clear New";

			// Token: 0x0400BBFB RID: 48123
			public static LocString CLEAR_ALL = "Unpin all resources";

			// Token: 0x0400BBFC RID: 48124
			public static LocString SEE_ALL = "+ See All ({0})";

			// Token: 0x0400BBFD RID: 48125
			public static LocString NEW_TAG = "NEW";
		}

		// Token: 0x02002D88 RID: 11656
		public class CONFIRMDIALOG
		{
			// Token: 0x0400BBFE RID: 48126
			public static LocString OK = "OK";

			// Token: 0x0400BBFF RID: 48127
			public static LocString CANCEL = "CANCEL";

			// Token: 0x0400BC00 RID: 48128
			public static LocString DIALOG_HEADER = "MESSAGE";
		}

		// Token: 0x02002D89 RID: 11657
		public class FACADE_SELECTION_PANEL
		{
			// Token: 0x0400BC01 RID: 48129
			public static LocString HEADER = "Select Blueprint";

			// Token: 0x0400BC02 RID: 48130
			public static LocString STORE_BUTTON_TOOLTIP = "See more Blueprints in the Supply Closet";
		}

		// Token: 0x02002D8A RID: 11658
		public class FILE_NAME_DIALOG
		{
			// Token: 0x0400BC03 RID: 48131
			public static LocString ENTER_TEXT = "Enter Text...";
		}

		// Token: 0x02002D8B RID: 11659
		public class MINION_IDENTITY_SORT
		{
			// Token: 0x0400BC04 RID: 48132
			public static LocString TITLE = "Sort By";

			// Token: 0x0400BC05 RID: 48133
			public static LocString NAME = "Duplicant";

			// Token: 0x0400BC06 RID: 48134
			public static LocString ROLE = "Role";

			// Token: 0x0400BC07 RID: 48135
			public static LocString PERMISSION = "Permission";
		}

		// Token: 0x02002D8C RID: 11660
		public class UISIDESCREENS
		{
			// Token: 0x02002D8D RID: 11661
			public class TABS
			{
				// Token: 0x0400BC08 RID: 48136
				public static LocString HEADER = "Options";

				// Token: 0x0400BC09 RID: 48137
				public static LocString CONFIGURATION = "Config";

				// Token: 0x0400BC0A RID: 48138
				public static LocString MATERIAL = "Material";

				// Token: 0x0400BC0B RID: 48139
				public static LocString SKIN = "Blueprint";
			}

			// Token: 0x02002D8E RID: 11662
			public class BLUEPRINT_TAB
			{
				// Token: 0x0400BC0C RID: 48140
				public static LocString EDIT_OUTFIT_BUTTON = "Restyle";

				// Token: 0x0400BC0D RID: 48141
				public static LocString SUBCATEGORY_OUTFIT = "Clothing";

				// Token: 0x0400BC0E RID: 48142
				public static LocString SUBCATEGORY_ATMOSUIT = "Atmo Suit";

				// Token: 0x0400BC0F RID: 48143
				public static LocString SUBCATEGORY_JOYRESPONSE = "Overjoyed";
			}

			// Token: 0x02002D8F RID: 11663
			public class NOCONFIG
			{
				// Token: 0x0400BC10 RID: 48144
				public static LocString TITLE = "No configuration";

				// Token: 0x0400BC11 RID: 48145
				public static LocString LABEL = "There is no configuration available for this object.";
			}

			// Token: 0x02002D90 RID: 11664
			public class ARTABLESELECTIONSIDESCREEN
			{
				// Token: 0x0400BC12 RID: 48146
				public static LocString TITLE = "Style Selection";

				// Token: 0x0400BC13 RID: 48147
				public static LocString BUTTON = "Redecorate";

				// Token: 0x0400BC14 RID: 48148
				public static LocString BUTTON_TOOLTIP = "Clears current artwork\n\nCreates errand for a skilled Duplicant to create selected style";

				// Token: 0x0400BC15 RID: 48149
				public static LocString CLEAR_BUTTON_TOOLTIP = "Clears current artwork\n\nAllows a skilled Duplicant to create artwork of their choice";
			}

			// Token: 0x02002D91 RID: 11665
			public class ARTIFACTANALYSISSIDESCREEN
			{
				// Token: 0x0400BC16 RID: 48150
				public static LocString NO_ARTIFACTS_DISCOVERED = "No artifacts analyzed";

				// Token: 0x0400BC17 RID: 48151
				public static LocString NO_ARTIFACTS_DISCOVERED_TOOLTIP = "Analyzing artifacts requires a Duplicant with the Masterworks skill";
			}

			// Token: 0x02002D92 RID: 11666
			public class BUTTONMENUSIDESCREEN
			{
				// Token: 0x0400BC18 RID: 48152
				public static LocString TITLE = "Building Menu";

				// Token: 0x0400BC19 RID: 48153
				public static LocString ALLOW_INTERNAL_CONSTRUCTOR = "Enable Auto-Delivery";

				// Token: 0x0400BC1A RID: 48154
				public static LocString ALLOW_INTERNAL_CONSTRUCTOR_TOOLTIP = "Order Duplicants to deliver {0}" + UI.FormatAsLink("s", "NONE") + " to this building automatically when they need replacing";

				// Token: 0x0400BC1B RID: 48155
				public static LocString DISALLOW_INTERNAL_CONSTRUCTOR = "Cancel Auto-Delivery";

				// Token: 0x0400BC1C RID: 48156
				public static LocString DISALLOW_INTERNAL_CONSTRUCTOR_TOOLTIP = "Cancel automatic {0} deliveries to this building";
			}

			// Token: 0x02002D93 RID: 11667
			public class CONFIGURECONSUMERSIDESCREEN
			{
				// Token: 0x0400BC1D RID: 48157
				public static LocString TITLE = "Configure Building";

				// Token: 0x0400BC1E RID: 48158
				public static LocString SELECTION_DESCRIPTION_HEADER = "Description";
			}

			// Token: 0x02002D94 RID: 11668
			public class TREEFILTERABLESIDESCREEN
			{
				// Token: 0x0400BC1F RID: 48159
				public static LocString TITLE = "Element Filter";

				// Token: 0x0400BC20 RID: 48160
				public static LocString TITLE_CRITTER = "Critter Filter";

				// Token: 0x0400BC21 RID: 48161
				public static LocString ALLBUTTON = "All Standard";

				// Token: 0x0400BC22 RID: 48162
				public static LocString ALLBUTTONTOOLTIP = "Allow storage of all standard resources preferred by this building\n\nNon-standard resources must be selected manually\n\nNon-standard resources include:\n    • Clothing\n    • Critter Eggs\n    • Sublimators";

				// Token: 0x0400BC23 RID: 48163
				public static LocString ALLBUTTON_EDIBLES = "All Edibles";

				// Token: 0x0400BC24 RID: 48164
				public static LocString ALLBUTTON_EDIBLES_TOOLTIP = "Allow storage of all edible resources";

				// Token: 0x0400BC25 RID: 48165
				public static LocString ALLBUTTON_CRITTERS = "All Critters";

				// Token: 0x0400BC26 RID: 48166
				public static LocString ALLBUTTON_CRITTERS_TOOLTIP = "Allow storage of all eligible " + UI.PRE_KEYWORD + "Critters" + UI.PST_KEYWORD;

				// Token: 0x0400BC27 RID: 48167
				public static LocString SPECIAL_RESOURCES = "Non-Standard";

				// Token: 0x0400BC28 RID: 48168
				public static LocString SPECIAL_RESOURCES_TOOLTIP = "These objects may not be ideally suited to storage";

				// Token: 0x0400BC29 RID: 48169
				public static LocString CATEGORYBUTTONTOOLTIP = "Allow storage of anything in the {0} resource category";

				// Token: 0x0400BC2A RID: 48170
				public static LocString MATERIALBUTTONTOOLTIP = "Add or remove this material from storage";

				// Token: 0x0400BC2B RID: 48171
				public static LocString ONLYALLOWTRANSPORTITEMSBUTTON = "Sweep Only";

				// Token: 0x0400BC2C RID: 48172
				public static LocString ONLYALLOWTRANSPORTITEMSBUTTONTOOLTIP = "Only store objects marked Sweep <color=#F44A47><b>[K]</b></color> in this container";

				// Token: 0x0400BC2D RID: 48173
				public static LocString ONLYALLOWSPICEDITEMSBUTTON = "Spiced Food Only";

				// Token: 0x0400BC2E RID: 48174
				public static LocString ONLYALLOWSPICEDITEMSBUTTONTOOLTIP = "Only store foods that have been spiced at the " + UI.PRE_KEYWORD + "Spice Grinder" + UI.PST_KEYWORD;

				// Token: 0x0400BC2F RID: 48175
				public static LocString SEARCH_PLACEHOLDER = "Search";
			}

			// Token: 0x02002D95 RID: 11669
			public class TELESCOPESIDESCREEN
			{
				// Token: 0x0400BC30 RID: 48176
				public static LocString TITLE = "Telescope Configuration";

				// Token: 0x0400BC31 RID: 48177
				public static LocString NO_SELECTED_ANALYSIS_TARGET = "No analysis focus selected\nOpen the " + UI.FormatAsManagementMenu("Starmap", global::Action.ManageStarmap) + " to select a focus";

				// Token: 0x0400BC32 RID: 48178
				public static LocString ANALYSIS_TARGET_SELECTED = "Object focus selected\nAnalysis underway";

				// Token: 0x0400BC33 RID: 48179
				public static LocString OPENSTARMAPBUTTON = "OPEN STARMAP";

				// Token: 0x0400BC34 RID: 48180
				public static LocString ANALYSIS_TARGET_HEADER = "Object Analysis";
			}

			// Token: 0x02002D96 RID: 11670
			public class CLUSTERTELESCOPESIDESCREEN
			{
				// Token: 0x0400BC35 RID: 48181
				public static LocString TITLE = "Telescope Configuration";

				// Token: 0x0400BC36 RID: 48182
				public static LocString CHECKBOX_METEORS = "Allow meteor shower identification";

				// Token: 0x0400BC37 RID: 48183
				public static LocString CHECKBOX_TOOLTIP_METEORS = string.Concat(new string[]
				{
					"Prioritizes unidentified meteors that come within range in a previously revealed location\n\nWill interrupt a Duplicant working on revealing a new ",
					UI.PRE_KEYWORD,
					"Starmap",
					UI.PST_KEYWORD,
					" location"
				});
			}

			// Token: 0x02002D97 RID: 11671
			public class TEMPORALTEARSIDESCREEN
			{
				// Token: 0x0400BC38 RID: 48184
				public static LocString TITLE = "Temporal Tear";

				// Token: 0x0400BC39 RID: 48185
				public static LocString BUTTON_OPEN = "Enter Tear";

				// Token: 0x0400BC3A RID: 48186
				public static LocString BUTTON_CLOSED = "Tear Closed";

				// Token: 0x0400BC3B RID: 48187
				public static LocString BUTTON_LABEL = "Enter Temporal Tear";

				// Token: 0x0400BC3C RID: 48188
				public static LocString CONFIRM_POPUP_MESSAGE = "Are you sure you want to fire this?";

				// Token: 0x0400BC3D RID: 48189
				public static LocString CONFIRM_POPUP_CONFIRM = "Yes, I'm ready for a meteor shower.";

				// Token: 0x0400BC3E RID: 48190
				public static LocString CONFIRM_POPUP_CANCEL = "No, I need more time to prepare.";

				// Token: 0x0400BC3F RID: 48191
				public static LocString CONFIRM_POPUP_TITLE = "Temporal Tear Opener";
			}

			// Token: 0x02002D98 RID: 11672
			public class RAILGUNSIDESCREEN
			{
				// Token: 0x0400BC40 RID: 48192
				public static LocString TITLE = "Launcher Configuration";

				// Token: 0x0400BC41 RID: 48193
				public static LocString NO_SELECTED_LAUNCH_TARGET = "No destination selected\nOpen the " + UI.FormatAsManagementMenu("Starmap", global::Action.ManageStarmap) + " to set a course";

				// Token: 0x0400BC42 RID: 48194
				public static LocString LAUNCH_TARGET_SELECTED = "Launcher destination {0} set";

				// Token: 0x0400BC43 RID: 48195
				public static LocString OPENSTARMAPBUTTON = "OPEN STARMAP";

				// Token: 0x0400BC44 RID: 48196
				public static LocString LAUNCH_RESOURCES_HEADER = "Launch Resources:";

				// Token: 0x0400BC45 RID: 48197
				public static LocString MINIMUM_PAYLOAD_MASS = "Minimum launch mass:";
			}

			// Token: 0x02002D99 RID: 11673
			public class CLUSTERWORLDSIDESCREEN
			{
				// Token: 0x0400BC46 RID: 48198
				public static LocString TITLE = UI.CLUSTERMAP.PLANETOID;

				// Token: 0x0400BC47 RID: 48199
				public static LocString VIEW_WORLD = "Oversee " + UI.CLUSTERMAP.PLANETOID;

				// Token: 0x0400BC48 RID: 48200
				public static LocString VIEW_WORLD_DISABLE_TOOLTIP = "Cannot view " + UI.CLUSTERMAP.PLANETOID;

				// Token: 0x0400BC49 RID: 48201
				public static LocString VIEW_WORLD_TOOLTIP = "View this " + UI.CLUSTERMAP.PLANETOID + "'s surface";
			}

			// Token: 0x02002D9A RID: 11674
			public class ROCKETMODULESIDESCREEN
			{
				// Token: 0x0400BC4A RID: 48202
				public static LocString TITLE = "Rocket Module";

				// Token: 0x0400BC4B RID: 48203
				public static LocString CHANGEMODULEPANEL = "Add or Change Module";

				// Token: 0x0400BC4C RID: 48204
				public static LocString ENGINE_MAX_HEIGHT = "This engine allows a <b>Maximum Rocket Height</b> of {0}";

				// Token: 0x02002D9B RID: 11675
				public class MODULESTATCHANGE
				{
					// Token: 0x0400BC4D RID: 48205
					public static LocString TITLE = "Rocket stats on construction:";

					// Token: 0x0400BC4E RID: 48206
					public static LocString BURDEN = "    • " + DUPLICANTS.ATTRIBUTES.ROCKETBURDEN.NAME + ": {0} ({1})";

					// Token: 0x0400BC4F RID: 48207
					public static LocString RANGE = string.Concat(new string[]
					{
						"    • Potential ",
						DUPLICANTS.ATTRIBUTES.FUELRANGEPERKILOGRAM.NAME,
						": {0}/1",
						UI.UNITSUFFIXES.MASS.KILOGRAM,
						" Fuel ({1})"
					});

					// Token: 0x0400BC50 RID: 48208
					public static LocString SPEED = "    • Speed: {0} ({1})";

					// Token: 0x0400BC51 RID: 48209
					public static LocString ENGINEPOWER = "    • " + DUPLICANTS.ATTRIBUTES.ROCKETENGINEPOWER.NAME + ": {0} ({1})";

					// Token: 0x0400BC52 RID: 48210
					public static LocString HEIGHT = "    • " + DUPLICANTS.ATTRIBUTES.HEIGHT.NAME + ": {0}/{2} ({1})";

					// Token: 0x0400BC53 RID: 48211
					public static LocString HEIGHT_NOMAX = "    • " + DUPLICANTS.ATTRIBUTES.HEIGHT.NAME + ": {0} ({1})";

					// Token: 0x0400BC54 RID: 48212
					public static LocString POSITIVEDELTA = UI.FormatAsPositiveModifier("{0}");

					// Token: 0x0400BC55 RID: 48213
					public static LocString NEGATIVEDELTA = UI.FormatAsNegativeModifier("{0}");
				}

				// Token: 0x02002D9C RID: 11676
				public class BUTTONSWAPMODULEUP
				{
					// Token: 0x0400BC56 RID: 48214
					public static LocString DESC = "Swap this rocket module with the one above";

					// Token: 0x0400BC57 RID: 48215
					public static LocString INVALID = "No module above may be swapped.\n\n    • A module above may be unable to have modules placed above it.\n    • A module above may be unable to fit into the space below it.\n    • This module may be unable to fit into the space above it.";
				}

				// Token: 0x02002D9D RID: 11677
				public class BUTTONVIEWINTERIOR
				{
					// Token: 0x0400BC58 RID: 48216
					public static LocString LABEL = "View Interior";

					// Token: 0x0400BC59 RID: 48217
					public static LocString DESC = "What's goin' on in there?";

					// Token: 0x0400BC5A RID: 48218
					public static LocString INVALID = "This module does not have an interior view";
				}

				// Token: 0x02002D9E RID: 11678
				public class BUTTONVIEWEXTERIOR
				{
					// Token: 0x0400BC5B RID: 48219
					public static LocString LABEL = "View Exterior";

					// Token: 0x0400BC5C RID: 48220
					public static LocString DESC = "Switch to external world view";

					// Token: 0x0400BC5D RID: 48221
					public static LocString INVALID = "Not available in flight";
				}

				// Token: 0x02002D9F RID: 11679
				public class BUTTONSWAPMODULEDOWN
				{
					// Token: 0x0400BC5E RID: 48222
					public static LocString DESC = "Swap this rocket module with the one below";

					// Token: 0x0400BC5F RID: 48223
					public static LocString INVALID = "No module below may be swapped.\n\n    • A module below may be unable to have modules placed below it.\n    • A module below may be unable to fit into the space above it.\n    • This module may be unable to fit into the space below it.";
				}

				// Token: 0x02002DA0 RID: 11680
				public class BUTTONCHANGEMODULE
				{
					// Token: 0x0400BC60 RID: 48224
					public static LocString DESC = "Swap this module for a different module";

					// Token: 0x0400BC61 RID: 48225
					public static LocString INVALID = "This module cannot be changed to a different type";
				}

				// Token: 0x02002DA1 RID: 11681
				public class BUTTONREMOVEMODULE
				{
					// Token: 0x0400BC62 RID: 48226
					public static LocString DESC = "Remove this module";

					// Token: 0x0400BC63 RID: 48227
					public static LocString INVALID = "This module cannot be removed";
				}

				// Token: 0x02002DA2 RID: 11682
				public class ADDMODULE
				{
					// Token: 0x0400BC64 RID: 48228
					public static LocString DESC = "Add a new module above this one";

					// Token: 0x0400BC65 RID: 48229
					public static LocString INVALID = "Modules cannot be added above this module, or there is no room above to add a module";
				}
			}

			// Token: 0x02002DA3 RID: 11683
			public class CLUSTERLOCATIONFILTERSIDESCREEN
			{
				// Token: 0x0400BC66 RID: 48230
				public static LocString TITLE = "Location Filter";

				// Token: 0x0400BC67 RID: 48231
				public static LocString HEADER = "Send Green signal at locations";

				// Token: 0x0400BC68 RID: 48232
				public static LocString EMPTY_SPACE_ROW = "In Space";
			}

			// Token: 0x02002DA4 RID: 11684
			public class DISPENSERSIDESCREEN
			{
				// Token: 0x0400BC69 RID: 48233
				public static LocString TITLE = "Dispenser";

				// Token: 0x0400BC6A RID: 48234
				public static LocString BUTTON_CANCEL = "Cancel order";

				// Token: 0x0400BC6B RID: 48235
				public static LocString BUTTON_DISPENSE = "Dispense item";
			}

			// Token: 0x02002DA5 RID: 11685
			public class ROCKETRESTRICTIONSIDESCREEN
			{
				// Token: 0x0400BC6C RID: 48236
				public static LocString TITLE = "Rocket Restrictions";

				// Token: 0x0400BC6D RID: 48237
				public static LocString BUILDING_RESTRICTIONS_LABEL = "Interior Building Restrictions";

				// Token: 0x0400BC6E RID: 48238
				public static LocString NONE_RESTRICTION_BUTTON = "None";

				// Token: 0x0400BC6F RID: 48239
				public static LocString NONE_RESTRICTION_BUTTON_TOOLTIP = "There are no restrictions on buildings inside this rocket";

				// Token: 0x0400BC70 RID: 48240
				public static LocString GROUNDED_RESTRICTION_BUTTON = "Grounded";

				// Token: 0x0400BC71 RID: 48241
				public static LocString GROUNDED_RESTRICTION_BUTTON_TOOLTIP = "Buildings with their access restricted cannot be operated while grounded, though they can still be filled";

				// Token: 0x0400BC72 RID: 48242
				public static LocString AUTOMATION = "Automation Controlled";

				// Token: 0x0400BC73 RID: 48243
				public static LocString AUTOMATION_TOOLTIP = "Building restrictions are managed by automation\n\nBuildings with their access restricted cannot be operated, though they can still be filled";
			}

			// Token: 0x02002DA6 RID: 11686
			public class HABITATMODULESIDESCREEN
			{
				// Token: 0x0400BC74 RID: 48244
				public static LocString TITLE = "Spacefarer Module";

				// Token: 0x0400BC75 RID: 48245
				public static LocString VIEW_BUTTON = "View Interior";

				// Token: 0x0400BC76 RID: 48246
				public static LocString VIEW_BUTTON_TOOLTIP = "What's goin' on in there?";
			}

			// Token: 0x02002DA7 RID: 11687
			public class HARVESTMODULESIDESCREEN
			{
				// Token: 0x0400BC77 RID: 48247
				public static LocString TITLE = "Resource Gathering";

				// Token: 0x0400BC78 RID: 48248
				public static LocString MINING_IN_PROGRESS = "Drilling...";

				// Token: 0x0400BC79 RID: 48249
				public static LocString MINING_STOPPED = "Not drilling";

				// Token: 0x0400BC7A RID: 48250
				public static LocString ENABLE = "Enable Drill";

				// Token: 0x0400BC7B RID: 48251
				public static LocString DISABLE = "Disable Drill";
			}

			// Token: 0x02002DA8 RID: 11688
			public class SELECTMODULESIDESCREEN
			{
				// Token: 0x0400BC7C RID: 48252
				public static LocString TITLE = "Select Module";

				// Token: 0x0400BC7D RID: 48253
				public static LocString BUILDBUTTON = "Build";

				// Token: 0x02002DA9 RID: 11689
				public class CONSTRAINTS
				{
					// Token: 0x02002DAA RID: 11690
					public class RESEARCHED
					{
						// Token: 0x0400BC7E RID: 48254
						public static LocString COMPLETE = "Research Completed";

						// Token: 0x0400BC7F RID: 48255
						public static LocString FAILED = "Research Incomplete";
					}

					// Token: 0x02002DAB RID: 11691
					public class MATERIALS_AVAILABLE
					{
						// Token: 0x0400BC80 RID: 48256
						public static LocString COMPLETE = "Materials available";

						// Token: 0x0400BC81 RID: 48257
						public static LocString FAILED = "• Materials unavailable";
					}

					// Token: 0x02002DAC RID: 11692
					public class ONE_COMMAND_PER_ROCKET
					{
						// Token: 0x0400BC82 RID: 48258
						public static LocString COMPLETE = "";

						// Token: 0x0400BC83 RID: 48259
						public static LocString FAILED = "• Command module already installed";
					}

					// Token: 0x02002DAD RID: 11693
					public class ONE_ENGINE_PER_ROCKET
					{
						// Token: 0x0400BC84 RID: 48260
						public static LocString COMPLETE = "";

						// Token: 0x0400BC85 RID: 48261
						public static LocString FAILED = "• Engine module already installed";
					}

					// Token: 0x02002DAE RID: 11694
					public class ENGINE_AT_BOTTOM
					{
						// Token: 0x0400BC86 RID: 48262
						public static LocString COMPLETE = "";

						// Token: 0x0400BC87 RID: 48263
						public static LocString FAILED = "• Must install at bottom of rocket";
					}

					// Token: 0x02002DAF RID: 11695
					public class TOP_ONLY
					{
						// Token: 0x0400BC88 RID: 48264
						public static LocString COMPLETE = "";

						// Token: 0x0400BC89 RID: 48265
						public static LocString FAILED = "• Must install at top of rocket";
					}

					// Token: 0x02002DB0 RID: 11696
					public class SPACE_AVAILABLE
					{
						// Token: 0x0400BC8A RID: 48266
						public static LocString COMPLETE = "";

						// Token: 0x0400BC8B RID: 48267
						public static LocString FAILED = "• Space above rocket blocked";
					}

					// Token: 0x02002DB1 RID: 11697
					public class PASSENGER_MODULE_AVAILABLE
					{
						// Token: 0x0400BC8C RID: 48268
						public static LocString COMPLETE = "";

						// Token: 0x0400BC8D RID: 48269
						public static LocString FAILED = "• Max number of passenger modules installed";
					}

					// Token: 0x02002DB2 RID: 11698
					public class MAX_MODULES
					{
						// Token: 0x0400BC8E RID: 48270
						public static LocString COMPLETE = "";

						// Token: 0x0400BC8F RID: 48271
						public static LocString FAILED = "• Max module limit of engine reached";
					}

					// Token: 0x02002DB3 RID: 11699
					public class MAX_HEIGHT
					{
						// Token: 0x0400BC90 RID: 48272
						public static LocString COMPLETE = "";

						// Token: 0x0400BC91 RID: 48273
						public static LocString FAILED = "• Engine's height limit reached or exceeded";

						// Token: 0x0400BC92 RID: 48274
						public static LocString FAILED_NO_ENGINE = "• Rocket requires space for an engine";
					}

					// Token: 0x02002DB4 RID: 11700
					public class ONE_ROBOPILOT_PER_ROCKET
					{
						// Token: 0x0400BC93 RID: 48275
						public static LocString COMPLETE = "";

						// Token: 0x0400BC94 RID: 48276
						public static LocString FAILED = "• Robo-Pilot module already installed";
					}
				}
			}

			// Token: 0x02002DB5 RID: 11701
			public class FILTERSIDESCREEN
			{
				// Token: 0x0400BC95 RID: 48277
				public static LocString TITLE = "Filter Outputs";

				// Token: 0x0400BC96 RID: 48278
				public static LocString NO_SELECTION = "None";

				// Token: 0x0400BC97 RID: 48279
				public static LocString OUTPUTELEMENTHEADER = "Output 1";

				// Token: 0x0400BC98 RID: 48280
				public static LocString SELECTELEMENTHEADER = "Output 2";

				// Token: 0x0400BC99 RID: 48281
				public static LocString OUTPUTRED = "Output Red";

				// Token: 0x0400BC9A RID: 48282
				public static LocString OUTPUTGREEN = "Output Green";

				// Token: 0x0400BC9B RID: 48283
				public static LocString NOELEMENTSELECTED = "No element selected";

				// Token: 0x0400BC9C RID: 48284
				public static LocString DRIEDFOOD = "Dried Food";

				// Token: 0x02002DB6 RID: 11702
				public static class UNFILTEREDELEMENTS
				{
					// Token: 0x0400BC9D RID: 48285
					public static LocString GAS = "Gas Output:\nAll";

					// Token: 0x0400BC9E RID: 48286
					public static LocString LIQUID = "Liquid Output:\nAll";

					// Token: 0x0400BC9F RID: 48287
					public static LocString SOLID = "Solid Output:\nAll";
				}

				// Token: 0x02002DB7 RID: 11703
				public static class FILTEREDELEMENT
				{
					// Token: 0x0400BCA0 RID: 48288
					public static LocString GAS = "Filtered Gas Output:\n{0}";

					// Token: 0x0400BCA1 RID: 48289
					public static LocString LIQUID = "Filtered Liquid Output:\n{0}";

					// Token: 0x0400BCA2 RID: 48290
					public static LocString SOLID = "Filtered Solid Output:\n{0}";
				}
			}

			// Token: 0x02002DB8 RID: 11704
			public class SINGLEITEMSELECTIONSIDESCREEN
			{
				// Token: 0x0400BCA3 RID: 48291
				public static LocString TITLE = "Element Filter";

				// Token: 0x0400BCA4 RID: 48292
				public static LocString LIST_TITLE = "Options";

				// Token: 0x0400BCA5 RID: 48293
				public static LocString NO_SELECTION = "None";

				// Token: 0x02002DB9 RID: 11705
				public class CURRENT_ITEM_SELECTED_SECTION
				{
					// Token: 0x0400BCA6 RID: 48294
					public static LocString TITLE = "Current Selection";

					// Token: 0x0400BCA7 RID: 48295
					public static LocString NO_ITEM_TITLE = "No Item Selected";

					// Token: 0x0400BCA8 RID: 48296
					public static LocString NO_ITEM_MESSAGE = "Select an item for storage below.";
				}
			}

			// Token: 0x02002DBA RID: 11706
			public class FEWOPTIONSELECTIONSIDESCREEN
			{
				// Token: 0x0400BCA9 RID: 48297
				public static LocString TITLE = "Options";
			}

			// Token: 0x02002DBB RID: 11707
			public class LOGICBROADCASTCHANNELSIDESCREEN
			{
				// Token: 0x0400BCAA RID: 48298
				public static LocString TITLE = "Channel Selector";

				// Token: 0x0400BCAB RID: 48299
				public static LocString HEADER = "Channel Selector";

				// Token: 0x0400BCAC RID: 48300
				public static LocString IN_RANGE = "In Range";

				// Token: 0x0400BCAD RID: 48301
				public static LocString OUT_OF_RANGE = "Out of Range";

				// Token: 0x0400BCAE RID: 48302
				public static LocString NO_SENDERS = "No Channels Available";

				// Token: 0x0400BCAF RID: 48303
				public static LocString NO_SENDERS_DESC = "Build a " + BUILDINGS.PREFABS.LOGICINTERASTEROIDSENDER.NAME + " to transmit a signal.";
			}

			// Token: 0x02002DBC RID: 11708
			public class CONDITIONLISTSIDESCREEN
			{
				// Token: 0x0400BCB0 RID: 48304
				public static LocString TITLE = "Condition List";
			}

			// Token: 0x02002DBD RID: 11709
			public class FABRICATORSIDESCREEN
			{
				// Token: 0x0400BCB1 RID: 48305
				public static LocString TITLE = "Production Orders";

				// Token: 0x0400BCB2 RID: 48306
				public static LocString SUBTITLE = "Recipes";

				// Token: 0x0400BCB3 RID: 48307
				public static LocString NORECIPEDISCOVERED = "No discovered recipes";

				// Token: 0x0400BCB4 RID: 48308
				public static LocString NORECIPEDISCOVERED_BODY = "Discover new ingredients or research new technology to unlock some recipes.";

				// Token: 0x0400BCB5 RID: 48309
				public static LocString NORECIPESELECTED = "No recipe selected";

				// Token: 0x0400BCB6 RID: 48310
				public static LocString SELECTRECIPE = "Select a recipe to fabricate.";

				// Token: 0x0400BCB7 RID: 48311
				public static LocString COST = "<b>Ingredients:</b>\n";

				// Token: 0x0400BCB8 RID: 48312
				public static LocString RESULTREQUIREMENTS = "<b>Requirements:</b>";

				// Token: 0x0400BCB9 RID: 48313
				public static LocString RESULTEFFECTS = "<b>Effects:</b>";

				// Token: 0x0400BCBA RID: 48314
				public static LocString KG = "- {0}: {1}\n";

				// Token: 0x0400BCBB RID: 48315
				public static LocString INFORMATION = "INFORMATION";

				// Token: 0x0400BCBC RID: 48316
				public static LocString CANCEL = "Cancel";

				// Token: 0x0400BCBD RID: 48317
				public static LocString RECIPERQUIREMENT = "{0}: {1} / {2}";

				// Token: 0x0400BCBE RID: 48318
				public static LocString RECIPEPRODUCT = "{0}: {1}";

				// Token: 0x0400BCBF RID: 48319
				public static LocString UNITS_AND_CALS = "{0} [{1}]";

				// Token: 0x0400BCC0 RID: 48320
				public static LocString CALS = "{0}";

				// Token: 0x0400BCC1 RID: 48321
				public static LocString QUEUED_MISSING_INGREDIENTS_TOOLTIP = "Missing {0} of {1}\n";

				// Token: 0x0400BCC2 RID: 48322
				public static LocString CURRENT_ORDER = "Current order: {0}";

				// Token: 0x0400BCC3 RID: 48323
				public static LocString NEXT_ORDER = "Next order: {0}";

				// Token: 0x0400BCC4 RID: 48324
				public static LocString NO_WORKABLE_ORDER = "No workable order";

				// Token: 0x0400BCC5 RID: 48325
				public static LocString RECIPE_DETAILS = "Recipe Details";

				// Token: 0x0400BCC6 RID: 48326
				public static LocString RECIPE_QUEUE = "Order Production Quantity:";

				// Token: 0x0400BCC7 RID: 48327
				public static LocString RECIPE_FOREVER = "Forever";

				// Token: 0x0400BCC8 RID: 48328
				public static LocString CHANGE_RECIPE_ARROW_LABEL = "Change recipe";

				// Token: 0x0400BCC9 RID: 48329
				public static LocString RECIPE_RESEARCH_REQUIRED = "Research Required";

				// Token: 0x0400BCCA RID: 48330
				public static LocString INGREDIENTS = "<b>Ingredients:</b>";

				// Token: 0x0400BCCB RID: 48331
				public static LocString RECIPE_EFFECTS = "<b>Effects:</b>";

				// Token: 0x0400BCCC RID: 48332
				public static LocString ALLOW_MUTANT_SEED_INGREDIENTS = "Building accepts mutant seeds";

				// Token: 0x0400BCCD RID: 48333
				public static LocString ALLOW_MUTANT_SEED_INGREDIENTS_TOOLTIP = "Toggle whether Duplicants will deliver mutant seed species to this building as recipe ingredients.";

				// Token: 0x02002DBE RID: 11710
				public class TOOLTIPS
				{
					// Token: 0x0400BCCE RID: 48334
					public static LocString RECIPE_WORKTIME = "This recipe takes {0} to complete";

					// Token: 0x0400BCCF RID: 48335
					public static LocString RECIPERQUIREMENT_SUFFICIENT = "This recipe consumes {1} of an available {2} of {0}";

					// Token: 0x0400BCD0 RID: 48336
					public static LocString RECIPERQUIREMENT_INSUFFICIENT = "This recipe requires {1} {0}\nAvailable: {2}";

					// Token: 0x0400BCD1 RID: 48337
					public static LocString RECIPEPRODUCT = "This recipe produces {1} {0}";
				}

				// Token: 0x02002DBF RID: 11711
				public class EFFECTS
				{
					// Token: 0x0400BCD2 RID: 48338
					public static LocString OXYGEN_TANK = STRINGS.EQUIPMENT.PREFABS.OXYGEN_TANK.NAME + " ({0})";

					// Token: 0x0400BCD3 RID: 48339
					public static LocString OXYGEN_TANK_UNDERWATER = STRINGS.EQUIPMENT.PREFABS.OXYGEN_TANK_UNDERWATER.NAME + " ({0})";

					// Token: 0x0400BCD4 RID: 48340
					public static LocString JETSUIT_TANK = STRINGS.EQUIPMENT.PREFABS.JET_SUIT.TANK_EFFECT_NAME + " ({0})";

					// Token: 0x0400BCD5 RID: 48341
					public static LocString LEADSUIT_BATTERY = STRINGS.EQUIPMENT.PREFABS.LEAD_SUIT.BATTERY_EFFECT_NAME + " ({0})";

					// Token: 0x0400BCD6 RID: 48342
					public static LocString COOL_VEST = STRINGS.EQUIPMENT.PREFABS.COOL_VEST.NAME + " ({0})";

					// Token: 0x0400BCD7 RID: 48343
					public static LocString WARM_VEST = STRINGS.EQUIPMENT.PREFABS.WARM_VEST.NAME + " ({0})";

					// Token: 0x0400BCD8 RID: 48344
					public static LocString FUNKY_VEST = STRINGS.EQUIPMENT.PREFABS.FUNKY_VEST.NAME + " ({0})";

					// Token: 0x0400BCD9 RID: 48345
					public static LocString RESEARCHPOINT = "{0}: +1";
				}

				// Token: 0x02002DC0 RID: 11712
				public class RECIPE_CATEGORIES
				{
					// Token: 0x0400BCDA RID: 48346
					public static LocString ATMO_SUIT_FACADES = "Atmo Suit Styles";

					// Token: 0x0400BCDB RID: 48347
					public static LocString JET_SUIT_FACADES = "Jet Suit Styles";

					// Token: 0x0400BCDC RID: 48348
					public static LocString LEAD_SUIT_FACADES = "Lead Suit Styles";

					// Token: 0x0400BCDD RID: 48349
					public static LocString PRIMO_GARB_FACADES = "Primo Garb Styles";
				}
			}

			// Token: 0x02002DC1 RID: 11713
			public class ASSIGNMENTGROUPCONTROLLER
			{
				// Token: 0x0400BCDE RID: 48350
				public static LocString TITLE = "Duplicant Assignment";

				// Token: 0x0400BCDF RID: 48351
				public static LocString PILOT = "Pilot";

				// Token: 0x0400BCE0 RID: 48352
				public static LocString OFFWORLD = "Offworld";

				// Token: 0x02002DC2 RID: 11714
				public class TOOLTIPS
				{
					// Token: 0x0400BCE1 RID: 48353
					public static LocString DIFFERENT_WORLD = "This Duplicant is on a different " + UI.CLUSTERMAP.PLANETOID;

					// Token: 0x0400BCE2 RID: 48354
					public static LocString ASSIGN = "<b>Add</b> this Duplicant to rocket crew";

					// Token: 0x0400BCE3 RID: 48355
					public static LocString UNASSIGN = "<b>Remove</b> this Duplicant from rocket crew";
				}
			}

			// Token: 0x02002DC3 RID: 11715
			public class LAUNCHPADSIDESCREEN
			{
				// Token: 0x0400BCE4 RID: 48356
				public static LocString TITLE = "Rocket Platform";

				// Token: 0x0400BCE5 RID: 48357
				public static LocString WAITING_TO_LAND_PANEL = "Waiting to land";

				// Token: 0x0400BCE6 RID: 48358
				public static LocString NO_ROCKETS_WAITING = "No rockets in orbit";

				// Token: 0x0400BCE7 RID: 48359
				public static LocString IN_ORBIT_ABOVE_PANEL = "Rockets in orbit";

				// Token: 0x0400BCE8 RID: 48360
				public static LocString NEW_ROCKET_BUTTON = "NEW ROCKET";

				// Token: 0x0400BCE9 RID: 48361
				public static LocString LAND_BUTTON = "LAND HERE";

				// Token: 0x0400BCEA RID: 48362
				public static LocString CANCEL_LAND_BUTTON = "CANCEL";

				// Token: 0x0400BCEB RID: 48363
				public static LocString LAUNCH_BUTTON = "BEGIN LAUNCH SEQUENCE";

				// Token: 0x0400BCEC RID: 48364
				public static LocString LAUNCH_BUTTON_DEBUG = "BEGIN LAUNCH SEQUENCE (DEBUG ENABLED)";

				// Token: 0x0400BCED RID: 48365
				public static LocString LAUNCH_BUTTON_TOOLTIP = "Blast off!";

				// Token: 0x0400BCEE RID: 48366
				public static LocString LAUNCH_BUTTON_NOT_READY_TOOLTIP = "This rocket is <b>not</b> ready to launch\n\n<b>Review the Launch Checklist in the status panel for more detail</b>";

				// Token: 0x0400BCEF RID: 48367
				public static LocString LAUNCH_WARNINGS_BUTTON = "ACKNOWLEDGE WARNINGS";

				// Token: 0x0400BCF0 RID: 48368
				public static LocString LAUNCH_WARNINGS_BUTTON_TOOLTIP = "Some items in the Launch Checklist require attention\n\n<b>" + UI.CLICK(UI.ClickType.Click) + " to ignore warnings and proceed with launch</b>";

				// Token: 0x0400BCF1 RID: 48369
				public static LocString LAUNCH_REQUESTED_BUTTON = "CANCEL LAUNCH";

				// Token: 0x0400BCF2 RID: 48370
				public static LocString LAUNCH_REQUESTED_BUTTON_TOOLTIP = "This rocket will take off as soon as a Duplicant takes the controls\n\n<b>" + UI.CLICK(UI.ClickType.Click) + " to cancel launch</b>";

				// Token: 0x0400BCF3 RID: 48371
				public static LocString LAUNCH_AUTOMATION_CONTROLLED = "AUTOMATION CONTROLLED";

				// Token: 0x0400BCF4 RID: 48372
				public static LocString LAUNCH_AUTOMATION_CONTROLLED_TOOLTIP = "This " + BUILDINGS.PREFABS.LAUNCHPAD.NAME + "'s launch operation is controlled by automation signals";

				// Token: 0x02002DC4 RID: 11716
				public class STATUS
				{
					// Token: 0x0400BCF5 RID: 48373
					public static LocString STILL_PREPPING = "Launch Checklist Incomplete";

					// Token: 0x0400BCF6 RID: 48374
					public static LocString READY_FOR_LAUNCH = "Ready to Launch";

					// Token: 0x0400BCF7 RID: 48375
					public static LocString LOADING_CREW = "Loading crew...";

					// Token: 0x0400BCF8 RID: 48376
					public static LocString UNLOADING_PASSENGERS = "Unloading non-crew...";

					// Token: 0x0400BCF9 RID: 48377
					public static LocString WAITING_FOR_PILOT = "Pilot requested at control station...";

					// Token: 0x0400BCFA RID: 48378
					public static LocString COUNTING_DOWN = "5... 4... 3... 2... 1...";

					// Token: 0x0400BCFB RID: 48379
					public static LocString TAKING_OFF = "Liftoff!!";
				}
			}

			// Token: 0x02002DC5 RID: 11717
			public class AUTOPLUMBERSIDESCREEN
			{
				// Token: 0x0400BCFC RID: 48380
				public static LocString TITLE = "Automatic Building Configuration";

				// Token: 0x02002DC6 RID: 11718
				public class BUTTONS
				{
					// Token: 0x02002DC7 RID: 11719
					public class POWER
					{
						// Token: 0x0400BCFD RID: 48381
						public static LocString TOOLTIP = "Add Dev Generator and Electrical Wires";
					}

					// Token: 0x02002DC8 RID: 11720
					public class PIPES
					{
						// Token: 0x0400BCFE RID: 48382
						public static LocString TOOLTIP = "Add Dev Pumps and Pipes";
					}

					// Token: 0x02002DC9 RID: 11721
					public class SOLIDS
					{
						// Token: 0x0400BCFF RID: 48383
						public static LocString TOOLTIP = "Spawn solid resources for a relevant recipe or conversions";
					}

					// Token: 0x02002DCA RID: 11722
					public class MINION
					{
						// Token: 0x0400BD00 RID: 48384
						public static LocString TOOLTIP = "Spawn a Duplicant in front of the building";
					}

					// Token: 0x02002DCB RID: 11723
					public class FACADE
					{
						// Token: 0x0400BD01 RID: 48385
						public static LocString TOOLTIP = "Toggle the building blueprint";
					}
				}
			}

			// Token: 0x02002DCC RID: 11724
			public class SELFDESTRUCTSIDESCREEN
			{
				// Token: 0x0400BD02 RID: 48386
				public static LocString TITLE = "Self Destruct";

				// Token: 0x0400BD03 RID: 48387
				public static LocString MESSAGE_TEXT = "EMERGENCY PROCEDURES";

				// Token: 0x0400BD04 RID: 48388
				public static LocString BUTTON_TEXT = "ABANDON SHIP!";

				// Token: 0x0400BD05 RID: 48389
				public static LocString BUTTON_TEXT_CONFIRM = "CONFIRM ABANDON SHIP";

				// Token: 0x0400BD06 RID: 48390
				public static LocString BUTTON_TOOLTIP = "This rocket is equipped with an emergency escape system.\n\nThe rocket's self-destruct sequence can be triggered to destroy it and propel fragments of the ship towards the nearest planetoid.\n\nAny Duplicants on board will be safely delivered in escape pods.";

				// Token: 0x0400BD07 RID: 48391
				public static LocString BUTTON_TOOLTIP_CONFIRM = "<b>This will eject any passengers and destroy the rocket.<b>\n\nThe rocket's self-destruct sequence can be triggered to destroy it and propel fragments of the ship towards the nearest planetoid.\n\nAny Duplicants on board will be safely delivered in escape pods.";
			}

			// Token: 0x02002DCD RID: 11725
			public class GENESHUFFLERSIDESREEN
			{
				// Token: 0x0400BD08 RID: 48392
				public static LocString TITLE = "Neural Vacillator";

				// Token: 0x0400BD09 RID: 48393
				public static LocString COMPLETE = "Something feels different.";

				// Token: 0x0400BD0A RID: 48394
				public static LocString UNDERWAY = "Neural Vacillation in progress.";

				// Token: 0x0400BD0B RID: 48395
				public static LocString CONSUMED = "There are no charges left in this Vacillator.";

				// Token: 0x0400BD0C RID: 48396
				public static LocString CONSUMED_WAITING = "Recharge requested, awaiting delivery by Duplicant.";

				// Token: 0x0400BD0D RID: 48397
				public static LocString BUTTON = "Complete Neural Process";

				// Token: 0x0400BD0E RID: 48398
				public static LocString BUTTON_RECHARGE = "Recharge";

				// Token: 0x0400BD0F RID: 48399
				public static LocString BUTTON_RECHARGE_CANCEL = "Cancel Recharge";
			}

			// Token: 0x02002DCE RID: 11726
			public class MINIONTODOSIDESCREEN
			{
				// Token: 0x0400BD10 RID: 48400
				public static LocString NAME = "Errands";

				// Token: 0x0400BD11 RID: 48401
				public static LocString TOOLTIP = "<b>Errands</b>\nView current and upcoming errands";

				// Token: 0x0400BD12 RID: 48402
				public static LocString CURRENT_TITLE = "Current Errand";

				// Token: 0x0400BD13 RID: 48403
				public static LocString LIST_TITLE = "Upcoming Errands";

				// Token: 0x0400BD14 RID: 48404
				public static LocString CURRENT_SCHEDULE_BLOCK = "Schedule Block: {0}";

				// Token: 0x0400BD15 RID: 48405
				public static LocString CHORE_TARGET = "{Target}";

				// Token: 0x0400BD16 RID: 48406
				public static LocString CHORE_TARGET_AND_GROUP = "{Target} -- {Groups}";

				// Token: 0x0400BD17 RID: 48407
				public static LocString SELF_LABEL = "Self";

				// Token: 0x0400BD18 RID: 48408
				public static LocString TRUNCATED_CHORES = "{0} more";

				// Token: 0x0400BD19 RID: 48409
				public static LocString TOOLTIP_IDLE = string.Concat(new string[]
				{
					"{IdleDescription}\n\nDuplicants will only <b>{Errand}</b> when there is nothing else for them to do\n\nTotal ",
					UI.PRE_KEYWORD,
					"Priority",
					UI.PST_KEYWORD,
					": {TotalPriority}\n    • ",
					UI.JOBSSCREEN.PRIORITY_CLASS.IDLE,
					": {ClassPriority}\n    • All {BestGroup} Errands: {TypePriority}"
				});

				// Token: 0x0400BD1A RID: 48410
				public static LocString TOOLTIP_NORMAL = string.Concat(new string[]
				{
					"{Description}\n\nErrand Type: {Groups}\n\nTotal ",
					UI.PRE_KEYWORD,
					"Priority",
					UI.PST_KEYWORD,
					": {TotalPriority}\n    • {Name}'s {BestGroup} Priority: {PersonalPriorityValue} ({PersonalPriority})\n    • This {Building}'s Priority: {BuildingPriority}\n    • All {BestGroup} Errands: {TypePriority}"
				});

				// Token: 0x0400BD1B RID: 48411
				public static LocString TOOLTIP_PERSONAL = string.Concat(new string[]
				{
					"{Description}\n\n<b>{Errand}</b> is a ",
					UI.JOBSSCREEN.PRIORITY_CLASS.PERSONAL_NEEDS,
					" errand and so will be performed before all Regular errands\n\nTotal ",
					UI.PRE_KEYWORD,
					"Priority",
					UI.PST_KEYWORD,
					": {TotalPriority}\n    • ",
					UI.JOBSSCREEN.PRIORITY_CLASS.PERSONAL_NEEDS,
					": {ClassPriority}\n    • All {BestGroup} Errands: {TypePriority}"
				});

				// Token: 0x0400BD1C RID: 48412
				public static LocString TOOLTIP_EMERGENCY = string.Concat(new string[]
				{
					"{Description}\n\n<b>{Errand}</b> is an ",
					UI.JOBSSCREEN.PRIORITY_CLASS.EMERGENCY,
					" errand and so will be performed before all Regular and Personal errands\n\nTotal ",
					UI.PRE_KEYWORD,
					"Priority",
					UI.PST_KEYWORD,
					": {TotalPriority}\n    • ",
					UI.JOBSSCREEN.PRIORITY_CLASS.EMERGENCY,
					" : {ClassPriority}\n    • This {Building}'s Priority: {BuildingPriority}\n    • All {BestGroup} Errands: {TypePriority}"
				});

				// Token: 0x0400BD1D RID: 48413
				public static LocString TOOLTIP_COMPULSORY = string.Concat(new string[]
				{
					"{Description}\n\n<b>{Errand}</b> is a ",
					UI.JOBSSCREEN.PRIORITY_CLASS.COMPULSORY,
					" action and so will occur immediately\n\nTotal ",
					UI.PRE_KEYWORD,
					"Priority",
					UI.PST_KEYWORD,
					": {TotalPriority}\n    • ",
					UI.JOBSSCREEN.PRIORITY_CLASS.COMPULSORY,
					": {ClassPriority}\n    • All {BestGroup} Errands: {TypePriority}"
				});

				// Token: 0x0400BD1E RID: 48414
				public static LocString TOOLTIP_DESC_ACTIVE = "{Name}'s Current Errand: <b>{Errand}</b>";

				// Token: 0x0400BD1F RID: 48415
				public static LocString TOOLTIP_DESC_INACTIVE = "{Name} could work on <b>{Errand}</b>, but it's not their top priority right now";

				// Token: 0x0400BD20 RID: 48416
				public static LocString TOOLTIP_IDLEDESC_ACTIVE = "{Name} is currently <b>Idle</b>";

				// Token: 0x0400BD21 RID: 48417
				public static LocString TOOLTIP_IDLEDESC_INACTIVE = "{Name} could become <b>Idle</b> when all other errands are canceled or completed";

				// Token: 0x0400BD22 RID: 48418
				public static LocString TOOLTIP_NA = "--";

				// Token: 0x0400BD23 RID: 48419
				public static LocString CHORE_GROUP_SEPARATOR = " or ";
			}

			// Token: 0x02002DCF RID: 11727
			public class MODULEFLIGHTUTILITYSIDESCREEN
			{
				// Token: 0x0400BD24 RID: 48420
				public static LocString TITLE = "Deployables";

				// Token: 0x0400BD25 RID: 48421
				public static LocString DEPLOY_BUTTON = "Deploy";

				// Token: 0x0400BD26 RID: 48422
				public static LocString DEPLOY_BUTTON_TOOLTIP = "Send this module's contents to the surface of the currently orbited " + UI.CLUSTERMAP.PLANETOID_KEYWORD + "\n\nA specific deploy location may need to be chosen for certain modules";

				// Token: 0x0400BD27 RID: 48423
				public static LocString REPEAT_BUTTON_TOOLTIP = "Automatically deploy this module's contents when a destination orbit is reached";

				// Token: 0x0400BD28 RID: 48424
				public static LocString SELECT_DUPLICANT = "Select Duplicant";

				// Token: 0x0400BD29 RID: 48425
				public static LocString PILOT_FMT = "{0} - Pilot";
			}

			// Token: 0x02002DD0 RID: 11728
			public class HIGHENERGYPARTICLEDIRECTIONSIDESCREEN
			{
				// Token: 0x0400BD2A RID: 48426
				public static LocString TITLE = "Emitting Particle Direction";

				// Token: 0x0400BD2B RID: 48427
				public static LocString SELECTED_DIRECTION = "Selected direction: {0}";

				// Token: 0x0400BD2C RID: 48428
				public static LocString DIRECTION_N = "N";

				// Token: 0x0400BD2D RID: 48429
				public static LocString DIRECTION_NE = "NE";

				// Token: 0x0400BD2E RID: 48430
				public static LocString DIRECTION_E = "E";

				// Token: 0x0400BD2F RID: 48431
				public static LocString DIRECTION_SE = "SE";

				// Token: 0x0400BD30 RID: 48432
				public static LocString DIRECTION_S = "S";

				// Token: 0x0400BD31 RID: 48433
				public static LocString DIRECTION_SW = "SW";

				// Token: 0x0400BD32 RID: 48434
				public static LocString DIRECTION_W = "W";

				// Token: 0x0400BD33 RID: 48435
				public static LocString DIRECTION_NW = "NW";
			}

			// Token: 0x02002DD1 RID: 11729
			public class MONUMENTSIDESCREEN
			{
				// Token: 0x0400BD34 RID: 48436
				public static LocString TITLE = "Great Monument";

				// Token: 0x0400BD35 RID: 48437
				public static LocString FLIP_FACING_BUTTON = UI.CLICK(UI.ClickType.CLICK) + " TO ROTATE";
			}

			// Token: 0x02002DD2 RID: 11730
			public class PLANTERSIDESCREEN
			{
				// Token: 0x0400BD36 RID: 48438
				public static LocString TITLE = "{0} Seeds";

				// Token: 0x0400BD37 RID: 48439
				public static LocString INFORMATION = "INFORMATION";

				// Token: 0x0400BD38 RID: 48440
				public static LocString AWAITINGREQUEST = "PLANT: {0}";

				// Token: 0x0400BD39 RID: 48441
				public static LocString AWAITINGDELIVERY = "AWAITING DELIVERY: {0}";

				// Token: 0x0400BD3A RID: 48442
				public static LocString AWAITINGREMOVAL = "AWAITING DIGGING UP: {0}";

				// Token: 0x0400BD3B RID: 48443
				public static LocString ENTITYDEPOSITED = "PLANTED: {0}";

				// Token: 0x0400BD3C RID: 48444
				public static LocString MUTATIONS_HEADER = "Mutations";

				// Token: 0x0400BD3D RID: 48445
				public static LocString DEPOSIT = "Plant";

				// Token: 0x0400BD3E RID: 48446
				public static LocString CANCELDEPOSIT = "Cancel";

				// Token: 0x0400BD3F RID: 48447
				public static LocString REMOVE = "Uproot";

				// Token: 0x0400BD40 RID: 48448
				public static LocString CANCELREMOVAL = "Cancel";

				// Token: 0x0400BD41 RID: 48449
				public static LocString SELECT_TITLE = "SELECT";

				// Token: 0x0400BD42 RID: 48450
				public static LocString SELECT_DESC = "Select a seed to plant.";

				// Token: 0x0400BD43 RID: 48451
				public static LocString LIFECYCLE = "<b>Life Cycle</b>:";

				// Token: 0x0400BD44 RID: 48452
				public static LocString PLANTREQUIREMENTS = "<b>Growth Requirements</b>:";

				// Token: 0x0400BD45 RID: 48453
				public static LocString PLANTEFFECTS = "<b>Effects</b>:";

				// Token: 0x0400BD46 RID: 48454
				public static LocString NUMBEROFHARVESTS = "Harvests: {0}";

				// Token: 0x0400BD47 RID: 48455
				public static LocString YIELD = "{0}: {1} ";

				// Token: 0x0400BD48 RID: 48456
				public static LocString YIELD_NONFOOD = "{0}: {1} ";

				// Token: 0x0400BD49 RID: 48457
				public static LocString YIELD_SINGLE = "{0}";

				// Token: 0x0400BD4A RID: 48458
				public static LocString YIELDPERHARVEST = "{0} {1} per harvest";

				// Token: 0x0400BD4B RID: 48459
				public static LocString TOTALHARVESTCALORIESWITHPERUNIT = "{0} [{1} / unit]";

				// Token: 0x0400BD4C RID: 48460
				public static LocString TOTALHARVESTCALORIES = "{0}";

				// Token: 0x0400BD4D RID: 48461
				public static LocString BONUS_SEEDS = "Base " + UI.FormatAsLink("Seed", "PLANTS") + " Harvest Chance: {0}";

				// Token: 0x0400BD4E RID: 48462
				public static LocString YIELD_SEED = "{1} {0}";

				// Token: 0x0400BD4F RID: 48463
				public static LocString YIELD_SEED_SINGLE = "{0}";

				// Token: 0x0400BD50 RID: 48464
				public static LocString YIELD_SEED_FINAL_HARVEST = "{1} {0} - Final harvest only";

				// Token: 0x0400BD51 RID: 48465
				public static LocString YIELD_SEED_SINGLE_FINAL_HARVEST = "{0} - Final harvest only";

				// Token: 0x0400BD52 RID: 48466
				public static LocString ROTATION_NEED_FLOOR = "<b>Requires upward plot orientation.</b>";

				// Token: 0x0400BD53 RID: 48467
				public static LocString ROTATION_NEED_WALL = "<b>Requires sideways plot orientation.</b>";

				// Token: 0x0400BD54 RID: 48468
				public static LocString ROTATION_NEED_CEILING = "<b>Requires downward plot orientation.</b>";

				// Token: 0x0400BD55 RID: 48469
				public static LocString NO_SPECIES_SELECTED = "Select a seed species above...";

				// Token: 0x0400BD56 RID: 48470
				public static LocString DISEASE_DROPPER_BURST = "{Disease} Burst: {DiseaseAmount}";

				// Token: 0x0400BD57 RID: 48471
				public static LocString DISEASE_DROPPER_CONSTANT = "{Disease}: {DiseaseAmount}";

				// Token: 0x0400BD58 RID: 48472
				public static LocString DISEASE_ON_HARVEST = "{Disease} on crop: {DiseaseAmount}";

				// Token: 0x0400BD59 RID: 48473
				public static LocString AUTO_SELF_HARVEST = "Self-Harvest On Grown";

				// Token: 0x02002DD3 RID: 11731
				public class TOOLTIPS
				{
					// Token: 0x0400BD5A RID: 48474
					public static LocString PLANTLIFECYCLE = "Duration and number of harvests produced by this plant in a lifetime";

					// Token: 0x0400BD5B RID: 48475
					public static LocString PLANTREQUIREMENTS = "Minimum conditions for basic plant growth";

					// Token: 0x0400BD5C RID: 48476
					public static LocString PLANTEFFECTS = "Additional attributes of this plant";

					// Token: 0x0400BD5D RID: 48477
					public static LocString YIELD = UI.FormatAsLink("{2}", "KCAL") + " produced [" + UI.FormatAsLink("{1}", "KCAL") + " / unit]";

					// Token: 0x0400BD5E RID: 48478
					public static LocString YIELD_NONFOOD = "{0} produced per harvest";

					// Token: 0x0400BD5F RID: 48479
					public static LocString NUMBEROFHARVESTS = "This plant can mature {0} times before the end of its life cycle";

					// Token: 0x0400BD60 RID: 48480
					public static LocString YIELD_SEED = "Sow to grow more of this plant";

					// Token: 0x0400BD61 RID: 48481
					public static LocString YIELD_SEED_FINAL_HARVEST = "{0}\n\nProduced in the final harvest of the plant's life cycle";

					// Token: 0x0400BD62 RID: 48482
					public static LocString BONUS_SEEDS = "This plant has a {0} chance to produce new seeds when harvested";

					// Token: 0x0400BD63 RID: 48483
					public static LocString DISEASE_DROPPER_BURST = "At certain points in this plant's lifecycle, it will emit a burst of {DiseaseAmount} {Disease}.";

					// Token: 0x0400BD64 RID: 48484
					public static LocString DISEASE_DROPPER_CONSTANT = "This plant emits {DiseaseAmount} {Disease} while it is alive.";

					// Token: 0x0400BD65 RID: 48485
					public static LocString DISEASE_ON_HARVEST = "The {Crop} produced by this plant will have {DiseaseAmount} {Disease} on it.";

					// Token: 0x0400BD66 RID: 48486
					public static LocString AUTO_SELF_HARVEST = "This plant will instantly drop its crop and begin regrowing when it is matured.";

					// Token: 0x0400BD67 RID: 48487
					public static LocString PLANT_TOGGLE_TOOLTIP = "{0}\n\n{1}\n\n<b>{2}</b> seeds available";
				}
			}

			// Token: 0x02002DD4 RID: 11732
			public class EGGINCUBATOR
			{
				// Token: 0x0400BD68 RID: 48488
				public static LocString TITLE = "Critter Eggs";

				// Token: 0x0400BD69 RID: 48489
				public static LocString AWAITINGREQUEST = "INCUBATE: {0}";

				// Token: 0x0400BD6A RID: 48490
				public static LocString AWAITINGDELIVERY = "AWAITING DELIVERY: {0}";

				// Token: 0x0400BD6B RID: 48491
				public static LocString AWAITINGREMOVAL = "AWAITING REMOVAL: {0}";

				// Token: 0x0400BD6C RID: 48492
				public static LocString ENTITYDEPOSITED = "INCUBATING: {0}";

				// Token: 0x0400BD6D RID: 48493
				public static LocString DEPOSIT = "Incubate";

				// Token: 0x0400BD6E RID: 48494
				public static LocString CANCELDEPOSIT = "Cancel";

				// Token: 0x0400BD6F RID: 48495
				public static LocString REMOVE = "Remove";

				// Token: 0x0400BD70 RID: 48496
				public static LocString CANCELREMOVAL = "Cancel";

				// Token: 0x0400BD71 RID: 48497
				public static LocString SELECT_TITLE = "SELECT";

				// Token: 0x0400BD72 RID: 48498
				public static LocString SELECT_DESC = "Select an egg to incubate.";
			}

			// Token: 0x02002DD5 RID: 11733
			public class BASICRECEPTACLE
			{
				// Token: 0x0400BD73 RID: 48499
				public static LocString TITLE = "Displayed Object";

				// Token: 0x0400BD74 RID: 48500
				public static LocString AWAITINGREQUEST = "SELECT: {0}";

				// Token: 0x0400BD75 RID: 48501
				public static LocString AWAITINGDELIVERY = "AWAITING DELIVERY: {0}";

				// Token: 0x0400BD76 RID: 48502
				public static LocString AWAITINGREMOVAL = "AWAITING REMOVAL: {0}";

				// Token: 0x0400BD77 RID: 48503
				public static LocString ENTITYDEPOSITED = "DISPLAYING: {0}";

				// Token: 0x0400BD78 RID: 48504
				public static LocString DEPOSIT = "Select";

				// Token: 0x0400BD79 RID: 48505
				public static LocString CANCELDEPOSIT = "Cancel";

				// Token: 0x0400BD7A RID: 48506
				public static LocString REMOVE = "Remove";

				// Token: 0x0400BD7B RID: 48507
				public static LocString CANCELREMOVAL = "Cancel";

				// Token: 0x0400BD7C RID: 48508
				public static LocString SELECT_TITLE = "SELECT OBJECT";

				// Token: 0x0400BD7D RID: 48509
				public static LocString SELECT_DESC = "Select an object to display here.";
			}

			// Token: 0x02002DD6 RID: 11734
			public class SPECIALCARGOBAYCLUSTER
			{
				// Token: 0x0400BD7E RID: 48510
				public static LocString TITLE = "Target Critter";

				// Token: 0x0400BD7F RID: 48511
				public static LocString AWAITINGREQUEST = "SELECT: {0}";

				// Token: 0x0400BD80 RID: 48512
				public static LocString AWAITINGDELIVERY = "AWAITING DELIVERY: {0}";

				// Token: 0x0400BD81 RID: 48513
				public static LocString AWAITINGREMOVAL = "AWAITING REMOVAL: {0}";

				// Token: 0x0400BD82 RID: 48514
				public static LocString ENTITYDEPOSITED = "CONTENTS: {0}";

				// Token: 0x0400BD83 RID: 48515
				public static LocString DEPOSIT = "Select";

				// Token: 0x0400BD84 RID: 48516
				public static LocString CANCELDEPOSIT = "Cancel";

				// Token: 0x0400BD85 RID: 48517
				public static LocString REMOVE = "Remove";

				// Token: 0x0400BD86 RID: 48518
				public static LocString CANCELREMOVAL = "Cancel";

				// Token: 0x0400BD87 RID: 48519
				public static LocString SELECT_TITLE = "SELECT CRITTER";

				// Token: 0x0400BD88 RID: 48520
				public static LocString SELECT_DESC = "Select a critter to store in this module.";
			}

			// Token: 0x02002DD7 RID: 11735
			public class LURE
			{
				// Token: 0x0400BD89 RID: 48521
				public static LocString TITLE = "Select Bait";

				// Token: 0x0400BD8A RID: 48522
				public static LocString INFORMATION = "INFORMATION";

				// Token: 0x0400BD8B RID: 48523
				public static LocString AWAITINGREQUEST = "PLANT: {0}";

				// Token: 0x0400BD8C RID: 48524
				public static LocString AWAITINGDELIVERY = "AWAITING DELIVERY: {0}";

				// Token: 0x0400BD8D RID: 48525
				public static LocString AWAITINGREMOVAL = "AWAITING DIGGING UP: {0}";

				// Token: 0x0400BD8E RID: 48526
				public static LocString ENTITYDEPOSITED = "PLANTED: {0}";

				// Token: 0x0400BD8F RID: 48527
				public static LocString ATTRACTS = "Attract {1}s";
			}

			// Token: 0x02002DD8 RID: 11736
			public class ROLESTATION
			{
				// Token: 0x0400BD90 RID: 48528
				public static LocString TITLE = "Duplicant Skills";

				// Token: 0x0400BD91 RID: 48529
				public static LocString OPENROLESBUTTON = "SKILLS";
			}

			// Token: 0x02002DD9 RID: 11737
			public class RESEARCHSIDESCREEN
			{
				// Token: 0x0400BD92 RID: 48530
				public static LocString TITLE = "Select Research";

				// Token: 0x0400BD93 RID: 48531
				public static LocString CURRENTLYRESEARCHING = "Currently Researching";

				// Token: 0x0400BD94 RID: 48532
				public static LocString NOSELECTEDRESEARCH = "No Research selected";

				// Token: 0x0400BD95 RID: 48533
				public static LocString OPENRESEARCHBUTTON = "RESEARCH";
			}

			// Token: 0x02002DDA RID: 11738
			public class REFINERYSIDESCREEN
			{
				// Token: 0x0400BD96 RID: 48534
				public static LocString RECIPE_FROM_TO = "{0} to {1}";

				// Token: 0x0400BD97 RID: 48535
				public static LocString RECIPE_WITH = "{1} ({0})";

				// Token: 0x0400BD98 RID: 48536
				public static LocString RECIPE_FROM_TO_WITH_NEWLINES = "{0}\nto\n{1}";

				// Token: 0x0400BD99 RID: 48537
				public static LocString RECIPE_FROM_TO_COMPOSITE = "{0} to {1} and {2}";

				// Token: 0x0400BD9A RID: 48538
				public static LocString RECIPE_FROM_TO_HEP = "{0} to " + UI.FormatAsLink("Radbolts", "RADIATION") + " and {1}";

				// Token: 0x0400BD9B RID: 48539
				public static LocString RECIPE_SIMPLE_INCLUDE_AMOUNTS = "{0} {1}";

				// Token: 0x0400BD9C RID: 48540
				public static LocString RECIPE_FROM_TO_INCLUDE_AMOUNTS = "{2} {0} to {3} {1}";

				// Token: 0x0400BD9D RID: 48541
				public static LocString RECIPE_WITH_INCLUDE_AMOUNTS = "{3} {1} ({2} {0})";

				// Token: 0x0400BD9E RID: 48542
				public static LocString RECIPE_FROM_TO_COMPOSITE_INCLUDE_AMOUNTS = "{3} {0} to {4} {1} and {5} {2}";

				// Token: 0x0400BD9F RID: 48543
				public static LocString RECIPE_FROM_TO_HEP_INCLUDE_AMOUNTS = "{2} {0} to {3} " + UI.FormatAsLink("Radbolts", "RADIATION") + " and {4} {1}";
			}

			// Token: 0x02002DDB RID: 11739
			public class SEALEDDOORSIDESCREEN
			{
				// Token: 0x0400BDA0 RID: 48544
				public static LocString TITLE = "Sealed Door";

				// Token: 0x0400BDA1 RID: 48545
				public static LocString LABEL = "This door requires a sample to unlock.";

				// Token: 0x0400BDA2 RID: 48546
				public static LocString BUTTON = "SUBMIT BIOSCAN";

				// Token: 0x0400BDA3 RID: 48547
				public static LocString AWAITINGBUTTON = "AWAITING BIOSCAN";
			}

			// Token: 0x02002DDC RID: 11740
			public class ENCRYPTEDLORESIDESCREEN
			{
				// Token: 0x0400BDA4 RID: 48548
				public static LocString TITLE = "Encrypted File";

				// Token: 0x0400BDA5 RID: 48549
				public static LocString LABEL = "This computer contains encrypted files.";

				// Token: 0x0400BDA6 RID: 48550
				public static LocString BUTTON = "ATTEMPT DECRYPTION";

				// Token: 0x0400BDA7 RID: 48551
				public static LocString AWAITINGBUTTON = "AWAITING DECRYPTION";
			}

			// Token: 0x02002DDD RID: 11741
			public class ACCESS_CONTROL_SIDE_SCREEN
			{
				// Token: 0x0400BDA8 RID: 48552
				public static LocString TITLE = "Door Access Control";

				// Token: 0x0400BDA9 RID: 48553
				public static LocString DOOR_DEFAULT = "Default";

				// Token: 0x0400BDAA RID: 48554
				public static LocString MINION_ACCESS = "Duplicant Access Permissions";

				// Token: 0x0400BDAB RID: 48555
				public static LocString GO_LEFT_ENABLED = "Passing Left through this door is permitted\n\n" + UI.CLICK(UI.ClickType.Click) + " to revoke permission";

				// Token: 0x0400BDAC RID: 48556
				public static LocString GO_LEFT_DISABLED = "Passing Left through this door is not permitted\n\n" + UI.CLICK(UI.ClickType.Click) + " to grant permission";

				// Token: 0x0400BDAD RID: 48557
				public static LocString GO_RIGHT_ENABLED = "Passing Right through this door is permitted\n\n" + UI.CLICK(UI.ClickType.Click) + " to revoke permission";

				// Token: 0x0400BDAE RID: 48558
				public static LocString GO_RIGHT_DISABLED = "Passing Right through this door is not permitted\n\n" + UI.CLICK(UI.ClickType.Click) + " to grant permission";

				// Token: 0x0400BDAF RID: 48559
				public static LocString GO_UP_ENABLED = "Passing Up through this door is permitted\n\n" + UI.CLICK(UI.ClickType.Click) + " to revoke permission";

				// Token: 0x0400BDB0 RID: 48560
				public static LocString GO_UP_DISABLED = "Passing Up through this door is not permitted\n\n" + UI.CLICK(UI.ClickType.Click) + " to grant permission";

				// Token: 0x0400BDB1 RID: 48561
				public static LocString GO_DOWN_ENABLED = "Passing Down through this door is permitted\n\n" + UI.CLICK(UI.ClickType.Click) + " to revoke permission";

				// Token: 0x0400BDB2 RID: 48562
				public static LocString GO_DOWN_DISABLED = "Passing Down through this door is not permitted\n\n" + UI.CLICK(UI.ClickType.Click) + " to grant permission";

				// Token: 0x0400BDB3 RID: 48563
				public static LocString SET_TO_DEFAULT = UI.CLICK(UI.ClickType.Click) + " to clear custom permissions";

				// Token: 0x0400BDB4 RID: 48564
				public static LocString SET_TO_CUSTOM = UI.CLICK(UI.ClickType.Click) + " to assign custom permissions";

				// Token: 0x0400BDB5 RID: 48565
				public static LocString USING_DEFAULT = "Default Access";

				// Token: 0x0400BDB6 RID: 48566
				public static LocString USING_CUSTOM = "Custom Access";
			}

			// Token: 0x02002DDE RID: 11742
			public class OWNABLESSIDESCREEN
			{
				// Token: 0x0400BDB7 RID: 48567
				public static LocString TITLE = "Equipment and Amenities";

				// Token: 0x0400BDB8 RID: 48568
				public static LocString NO_ITEM_ASSIGNED = "Assign";

				// Token: 0x0400BDB9 RID: 48569
				public static LocString NO_ITEM_FOUND = "None found";

				// Token: 0x0400BDBA RID: 48570
				public static LocString NO_APPLICABLE = "{0}: Ineligible";

				// Token: 0x02002DDF RID: 11743
				public static class TOOLTIPS
				{
					// Token: 0x0400BDBB RID: 48571
					public static LocString NO_APPLICABLE = "This Duplicant cannot be assigned " + UI.PRE_KEYWORD + "{0}" + UI.PST_KEYWORD;

					// Token: 0x0400BDBC RID: 48572
					public static LocString NO_ITEM_ASSIGNED = string.Concat(new string[]
					{
						"Click to view and assign existing ",
						UI.PRE_KEYWORD,
						"{0}",
						UI.PST_KEYWORD,
						" to this Duplicant"
					});

					// Token: 0x0400BDBD RID: 48573
					public static LocString ITEM_ASSIGNED_GENERIC = "This Duplicant has {0} assigned to them";

					// Token: 0x0400BDBE RID: 48574
					public static LocString ITEM_ASSIGNED = "{0}\n\n{1}";
				}

				// Token: 0x02002DE0 RID: 11744
				public class CATEGORIES
				{
					// Token: 0x0400BDBF RID: 48575
					public static LocString SUITS = "Suits";

					// Token: 0x0400BDC0 RID: 48576
					public static LocString AMENITIES = "Amenities";
				}
			}

			// Token: 0x02002DE1 RID: 11745
			public class OWNABLESSECONDSIDESCREEN
			{
				// Token: 0x0400BDC1 RID: 48577
				public static LocString TITLE = "{0}";

				// Token: 0x0400BDC2 RID: 48578
				public static LocString NONE_ROW_LABEL = "Unequip";

				// Token: 0x0400BDC3 RID: 48579
				public static LocString NONE_ROW_TOOLTIP = "Click to remove any item currently assigned to the selected slot";

				// Token: 0x0400BDC4 RID: 48580
				public static LocString ASSIGNED_TO_OTHER_STATUS = "Assigned to: {0}";

				// Token: 0x0400BDC5 RID: 48581
				public static LocString ASSIGNED_TO_SELF_STATUS = "Assigned";

				// Token: 0x0400BDC6 RID: 48582
				public static LocString NOT_ASSIGNED = "Unassigned";
			}

			// Token: 0x02002DE2 RID: 11746
			public class ASSIGNABLESIDESCREEN
			{
				// Token: 0x0400BDC7 RID: 48583
				public static LocString TITLE = "Assign {0}";

				// Token: 0x0400BDC8 RID: 48584
				public static LocString ASSIGNED = "Assigned";

				// Token: 0x0400BDC9 RID: 48585
				public static LocString UNASSIGNED = "-";

				// Token: 0x0400BDCA RID: 48586
				public static LocString DISABLED = "Ineligible";

				// Token: 0x0400BDCB RID: 48587
				public static LocString SORT_BY_DUPLICANT = "Duplicant";

				// Token: 0x0400BDCC RID: 48588
				public static LocString SORT_BY_ASSIGNMENT = "Assignment";

				// Token: 0x0400BDCD RID: 48589
				public static LocString ASSIGN_TO_TOOLTIP = "Assign to {0}";

				// Token: 0x0400BDCE RID: 48590
				public static LocString UNASSIGN_TOOLTIP = "Assigned to {0}";

				// Token: 0x0400BDCF RID: 48591
				public static LocString DISABLED_TOOLTIP = "{0} is ineligible for this skill assignment";

				// Token: 0x0400BDD0 RID: 48592
				public static LocString PUBLIC = "Public";
			}

			// Token: 0x02002DE3 RID: 11747
			public class COMETDETECTORSIDESCREEN
			{
				// Token: 0x0400BDD1 RID: 48593
				public static LocString TITLE = "Space Scanner";

				// Token: 0x0400BDD2 RID: 48594
				public static LocString HEADER = "Sends automation signal when selected object is detected";

				// Token: 0x0400BDD3 RID: 48595
				public static LocString ASSIGNED = "Assigned";

				// Token: 0x0400BDD4 RID: 48596
				public static LocString UNASSIGNED = "-";

				// Token: 0x0400BDD5 RID: 48597
				public static LocString DISABLED = "Ineligible";

				// Token: 0x0400BDD6 RID: 48598
				public static LocString SORT_BY_DUPLICANT = "Duplicant";

				// Token: 0x0400BDD7 RID: 48599
				public static LocString SORT_BY_ASSIGNMENT = "Assignment";

				// Token: 0x0400BDD8 RID: 48600
				public static LocString ASSIGN_TO_TOOLTIP = "Scanning for {0}";

				// Token: 0x0400BDD9 RID: 48601
				public static LocString UNASSIGN_TOOLTIP = "Scanning for {0}";

				// Token: 0x0400BDDA RID: 48602
				public static LocString NOTHING = "Nothing";

				// Token: 0x0400BDDB RID: 48603
				public static LocString COMETS = "Meteor Showers";

				// Token: 0x0400BDDC RID: 48604
				public static LocString ROCKETS = "Rocket Landing Ping";

				// Token: 0x0400BDDD RID: 48605
				public static LocString DUPEMADE = "Interplanetary Payloads";
			}

			// Token: 0x02002DE4 RID: 11748
			public class GEOTUNERSIDESCREEN
			{
				// Token: 0x0400BDDE RID: 48606
				public static LocString TITLE = "Select Geyser";

				// Token: 0x0400BDDF RID: 48607
				public static LocString DESCRIPTION = "Select an analyzed geyser to transmit amplification data to.";

				// Token: 0x0400BDE0 RID: 48608
				public static LocString NOTHING = "No geyser selected";

				// Token: 0x0400BDE1 RID: 48609
				public static LocString UNSTUDIED_TOOLTIP = "This geyser must be analyzed before it can be selected\n\nDouble-click to view this geyser";

				// Token: 0x0400BDE2 RID: 48610
				public static LocString STUDIED_TOOLTIP = string.Concat(new string[]
				{
					"Increase this geyser's ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" and output"
				});

				// Token: 0x0400BDE3 RID: 48611
				public static LocString GEOTUNER_LIMIT_TOOLTIP = "This geyser cannot be targeted by more " + UI.PRE_KEYWORD + "Geotuners" + UI.PST_KEYWORD;

				// Token: 0x0400BDE4 RID: 48612
				public static LocString STUDIED_TOOLTIP_MATERIAL = "Required resource: {MATERIAL}";

				// Token: 0x0400BDE5 RID: 48613
				public static LocString STUDIED_TOOLTIP_POTENTIAL_OUTPUT = "Potential Output {POTENTIAL_OUTPUT}";

				// Token: 0x0400BDE6 RID: 48614
				public static LocString STUDIED_TOOLTIP_BASE_TEMP = "Base {BASE}";

				// Token: 0x0400BDE7 RID: 48615
				public static LocString STUDIED_TOOLTIP_VISIT_GEYSER = "Double-click to view this geyser";

				// Token: 0x0400BDE8 RID: 48616
				public static LocString STUDIED_TOOLTIP_GEOTUNER_MODIFIER_ROW_TITLE = "Geotuned ";

				// Token: 0x0400BDE9 RID: 48617
				public static LocString STUDIED_TOOLTIP_NUMBER_HOVERED = "This geyser is targeted by {0} Geotuners";
			}

			// Token: 0x02002DE5 RID: 11749
			public class REMOTE_WORK_TERMINAL_SIDE_SCREEN
			{
				// Token: 0x0400BDEA RID: 48618
				public static LocString DOCK_TOOLTIP = "Click to assign this dock to this controller\n\nDouble-click to view this dock";
			}

			// Token: 0x02002DE6 RID: 11750
			public class COMMAND_MODULE_SIDE_SCREEN
			{
				// Token: 0x0400BDEB RID: 48619
				public static LocString TITLE = "Launch Conditions";

				// Token: 0x0400BDEC RID: 48620
				public static LocString DESTINATION_BUTTON = "Show Starmap";

				// Token: 0x0400BDED RID: 48621
				public static LocString DESTINATION_BUTTON_EXPANSION = "Show Starmap";
			}

			// Token: 0x02002DE7 RID: 11751
			public class CLUSTERDESTINATIONSIDESCREEN
			{
				// Token: 0x0400BDEE RID: 48622
				public static LocString TITLE = "Destination";

				// Token: 0x0400BDEF RID: 48623
				public static LocString FIRSTAVAILABLE = "Any " + BUILDINGS.PREFABS.LAUNCHPAD.NAME;

				// Token: 0x0400BDF0 RID: 48624
				public static LocString NONEAVAILABLE = "No landing site";

				// Token: 0x0400BDF1 RID: 48625
				public static LocString NO_TALL_SITES_AVAILABLE = "No landing sites fit the height of this rocket";

				// Token: 0x0400BDF2 RID: 48626
				public static LocString DROPDOWN_TOOLTIP_VALID_SITE = "Land at {0} when the site is clear";

				// Token: 0x0400BDF3 RID: 48627
				public static LocString DROPDOWN_TOOLTIP_FIRST_AVAILABLE = "Select the first available landing site";

				// Token: 0x0400BDF4 RID: 48628
				public static LocString DROPDOWN_TOOLTIP_TOO_SHORT = "This rocket's height exceeds the space available in this landing site";

				// Token: 0x0400BDF5 RID: 48629
				public static LocString DROPDOWN_TOOLTIP_PATH_OBSTRUCTED = "Landing path obstructed";

				// Token: 0x0400BDF6 RID: 48630
				public static LocString DROPDOWN_TOOLTIP_SITE_OBSTRUCTED = "Landing position on the platform is obstructed";

				// Token: 0x0400BDF7 RID: 48631
				public static LocString DROPDOWN_TOOLTIP_PAD_DISABLED = BUILDINGS.PREFABS.LAUNCHPAD.NAME + " is disabled";

				// Token: 0x0400BDF8 RID: 48632
				public static LocString CHANGE_DESTINATION_BUTTON = "Change";

				// Token: 0x0400BDF9 RID: 48633
				public static LocString CHANGE_DESTINATION_BUTTON_TOOLTIP = "Select a new destination for this rocket";

				// Token: 0x0400BDFA RID: 48634
				public static LocString CLEAR_DESTINATION_BUTTON = "Clear";

				// Token: 0x0400BDFB RID: 48635
				public static LocString CLEAR_DESTINATION_BUTTON_TOOLTIP = "Clear this rocket's selected destination";

				// Token: 0x0400BDFC RID: 48636
				public static LocString LOOP_BUTTON_TOOLTIP = "Toggle a roundtrip flight between this rocket's destination and its original takeoff location";

				// Token: 0x02002DE8 RID: 11752
				public class ASSIGNMENTSTATUS
				{
					// Token: 0x0400BDFD RID: 48637
					public static LocString LOCAL = "Current";

					// Token: 0x0400BDFE RID: 48638
					public static LocString DESTINATION = "Destination";
				}
			}

			// Token: 0x02002DE9 RID: 11753
			public class EQUIPPABLESIDESCREEN
			{
				// Token: 0x0400BDFF RID: 48639
				public static LocString TITLE = "Equip {0}";

				// Token: 0x0400BE00 RID: 48640
				public static LocString ASSIGNEDTO = "Assigned to: {Assignee}";

				// Token: 0x0400BE01 RID: 48641
				public static LocString UNASSIGNED = "Unassigned";

				// Token: 0x0400BE02 RID: 48642
				public static LocString GENERAL_CURRENTASSIGNED = "(Owner)";
			}

			// Token: 0x02002DEA RID: 11754
			public class EQUIPPABLE_SIDE_SCREEN
			{
				// Token: 0x0400BE03 RID: 48643
				public static LocString TITLE = "Assign To Duplicant";

				// Token: 0x0400BE04 RID: 48644
				public static LocString CURRENTLY_EQUIPPED = "Currently Equipped:\n{0}";

				// Token: 0x0400BE05 RID: 48645
				public static LocString NONE_EQUIPPED = "None";

				// Token: 0x0400BE06 RID: 48646
				public static LocString EQUIP_BUTTON = "Equip";

				// Token: 0x0400BE07 RID: 48647
				public static LocString DROP_BUTTON = "Drop";

				// Token: 0x0400BE08 RID: 48648
				public static LocString SWAP_BUTTON = "Swap";
			}

			// Token: 0x02002DEB RID: 11755
			public class TELEPADSIDESCREEN
			{
				// Token: 0x0400BE09 RID: 48649
				public static LocString TITLE = "Printables";

				// Token: 0x0400BE0A RID: 48650
				public static LocString NEXTPRODUCTION = "Next Production: {0}";

				// Token: 0x0400BE0B RID: 48651
				public static LocString GAMEOVER = "Colony Lost";

				// Token: 0x0400BE0C RID: 48652
				public static LocString VICTORY_CONDITIONS = "Hardwired Imperatives";

				// Token: 0x0400BE0D RID: 48653
				public static LocString SUMMARY_TITLE = "Colony Summary";

				// Token: 0x0400BE0E RID: 48654
				public static LocString SKILLS_BUTTON = "Duplicant Skills";
			}

			// Token: 0x02002DEC RID: 11756
			public class VALVESIDESCREEN
			{
				// Token: 0x0400BE0F RID: 48655
				public static LocString TITLE = "Flow Control";
			}

			// Token: 0x02002DED RID: 11757
			public class BIONIC_SIDE_SCREEN
			{
				// Token: 0x0400BE10 RID: 48656
				public static LocString TITLE = "Boosters";

				// Token: 0x0400BE11 RID: 48657
				public static LocString UPGRADE_SLOT_EMPTY = "Empty";

				// Token: 0x0400BE12 RID: 48658
				public static LocString UPGRADE_SLOT_ASSIGNED = "Assigned";

				// Token: 0x0400BE13 RID: 48659
				public static LocString UPGRADE_SLOT_WATTAGE = "{0}";

				// Token: 0x0400BE14 RID: 48660
				public static LocString CURRENT_WATTAGE_LABEL = "Current Wattage: <b>{0}</b>";

				// Token: 0x0400BE15 RID: 48661
				public static LocString CURRENT_WATTAGE_LABEL_BATTERY_SAVE_MODE = "Current Wattage: <color=#0303fc><b>{0}</b> {1}</color>";

				// Token: 0x0400BE16 RID: 48662
				public static LocString CURRENT_WATTAGE_LABEL_OFFLINE = "Current Wattage: <color=#GG2222>Offline {0}</color>";

				// Token: 0x0400BE17 RID: 48663
				public const string OFFLINE_MODE_COLOR = "<color=#GG2222>";

				// Token: 0x0400BE18 RID: 48664
				public const string BATTERY_SAVE_MODE_COLOR = "<color=#0303fc>";

				// Token: 0x0400BE19 RID: 48665
				public const string COLOR_END = "</color>";

				// Token: 0x02002DEE RID: 11758
				public class TOOLTIP
				{
					// Token: 0x0400BE1A RID: 48666
					public static LocString CURRENT_WATTAGE = "Wattage is the amount of energy that this Duplicant's bionic parts consume per second\n\nInstalled boosters consume wattage while active";

					// Token: 0x0400BE1B RID: 48667
					public static LocString SLOT_EMPTY = "No booster installed\n\nClick to view available boosters";

					// Token: 0x0400BE1C RID: 48668
					public static LocString SLOT_ASSIGNED = string.Concat(new string[]
					{
						"This ",
						UI.PRE_KEYWORD,
						"{0}",
						UI.PST_KEYWORD,
						" will be installed when it is within this Duplicant's reach"
					});

					// Token: 0x0400BE1D RID: 48669
					public static LocString SLOT_INSTALLED_IN_USE = UI.PRE_KEYWORD + "{0}" + UI.PST_KEYWORD + " installed\n\nStatus: Active\n\nWattage: {1}\n\n{2}";

					// Token: 0x0400BE1E RID: 48670
					public static LocString SLOT_INSTALLED_NOT_IN_USE = UI.PRE_KEYWORD + "{0}" + UI.PST_KEYWORD + " installed\n\nStatus: Idle\n\nPotential Wattage: {1}\n\n{2}";
				}
			}

			// Token: 0x02002DEF RID: 11759
			public class LIMIT_VALVE_SIDE_SCREEN
			{
				// Token: 0x0400BE1F RID: 48671
				public static LocString TITLE = "Meter Control";

				// Token: 0x0400BE20 RID: 48672
				public static LocString AMOUNT = "Amount: {0}";

				// Token: 0x0400BE21 RID: 48673
				public static LocString LIMIT = "Limit:";

				// Token: 0x0400BE22 RID: 48674
				public static LocString RESET_BUTTON = "Reset Amount";

				// Token: 0x0400BE23 RID: 48675
				public static LocString SLIDER_TOOLTIP_UNITS = "The amount of Units or Mass passing through the sensor.";
			}

			// Token: 0x02002DF0 RID: 11760
			public class NUCLEAR_REACTOR_SIDE_SCREEN
			{
				// Token: 0x0400BE24 RID: 48676
				public static LocString TITLE = "Reaction Mass Target";

				// Token: 0x0400BE25 RID: 48677
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Duplicants will attempt to keep the reactor supplied with ",
					UI.PRE_KEYWORD,
					"{0}{1}",
					UI.PST_KEYWORD,
					" of ",
					UI.PRE_KEYWORD,
					"{2}",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x02002DF1 RID: 11761
			public class MANUALGENERATORSIDESCREEN
			{
				// Token: 0x0400BE26 RID: 48678
				public static LocString TITLE = "Battery Recharge Threshold";

				// Token: 0x0400BE27 RID: 48679
				public static LocString CURRENT_THRESHOLD = "Current Threshold: {0}%";

				// Token: 0x0400BE28 RID: 48680
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Duplicants will be requested to operate this generator when the total charge of the connected ",
					UI.PRE_KEYWORD,
					"Batteries",
					UI.PST_KEYWORD,
					" falls below <b>{0}%</b>"
				});
			}

			// Token: 0x02002DF2 RID: 11762
			public class SPACEHEATERSIDESCREEN
			{
				// Token: 0x0400BE29 RID: 48681
				public static LocString TITLE = "Power Consumption";

				// Token: 0x0400BE2A RID: 48682
				public static LocString CURRENT_THRESHOLD = "Current Power Consumption: {0}";

				// Token: 0x0400BE2B RID: 48683
				public static LocString TOOLTIP = "Adjust power consumption to determine how much heat is produced\n\nCurrent heat production: <b>{0}</b>";
			}

			// Token: 0x02002DF3 RID: 11763
			public class MANUALDELIVERYGENERATORSIDESCREEN
			{
				// Token: 0x0400BE2C RID: 48684
				public static LocString TITLE = "Fuel Request Threshold";

				// Token: 0x0400BE2D RID: 48685
				public static LocString CURRENT_THRESHOLD = "Current Threshold: {0}%";

				// Token: 0x0400BE2E RID: 48686
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Duplicants will be requested to deliver ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" when the total charge of the connected ",
					UI.PRE_KEYWORD,
					"Batteries",
					UI.PST_KEYWORD,
					" falls below <b>{1}%</b>"
				});
			}

			// Token: 0x02002DF4 RID: 11764
			public class TIME_OF_DAY_SIDE_SCREEN
			{
				// Token: 0x0400BE2F RID: 48687
				public static LocString TITLE = "Time-of-Day Sensor";

				// Token: 0x0400BE30 RID: 48688
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Will send a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" after the selected Turn On time, and a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" after the selected Turn Off time"
				});

				// Token: 0x0400BE31 RID: 48689
				public static LocString START = "Turn On";

				// Token: 0x0400BE32 RID: 48690
				public static LocString STOP = "Turn Off";
			}

			// Token: 0x02002DF5 RID: 11765
			public class CRITTER_COUNT_SIDE_SCREEN
			{
				// Token: 0x0400BE33 RID: 48691
				public static LocString TITLE = "Critter Count Sensor";

				// Token: 0x0400BE34 RID: 48692
				public static LocString TOOLTIP_ABOVE = string.Concat(new string[]
				{
					"Will send a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if there are more than <b>{0}</b> ",
					UI.PRE_KEYWORD,
					"Critters",
					UI.PST_KEYWORD,
					" or ",
					UI.PRE_KEYWORD,
					"Eggs",
					UI.PST_KEYWORD,
					" in the room"
				});

				// Token: 0x0400BE35 RID: 48693
				public static LocString TOOLTIP_BELOW = string.Concat(new string[]
				{
					"Will send a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if there are fewer than <b>{0}</b> ",
					UI.PRE_KEYWORD,
					"Critters",
					UI.PST_KEYWORD,
					" or ",
					UI.PRE_KEYWORD,
					"Eggs",
					UI.PST_KEYWORD,
					" in the room"
				});

				// Token: 0x0400BE36 RID: 48694
				public static LocString START = "Turn On";

				// Token: 0x0400BE37 RID: 48695
				public static LocString STOP = "Turn Off";

				// Token: 0x0400BE38 RID: 48696
				public static LocString VALUE_NAME = "Count";
			}

			// Token: 0x02002DF6 RID: 11766
			public class OIL_WELL_CAP_SIDE_SCREEN
			{
				// Token: 0x0400BE39 RID: 48697
				public static LocString TITLE = "Backpressure Release Threshold";

				// Token: 0x0400BE3A RID: 48698
				public static LocString TOOLTIP = "Duplicants will be requested to release backpressure buildup when it exceeds <b>{0}%</b>";
			}

			// Token: 0x02002DF7 RID: 11767
			public class MODULAR_CONDUIT_PORT_SIDE_SCREEN
			{
				// Token: 0x0400BE3B RID: 48699
				public static LocString TITLE = "Pump Control";

				// Token: 0x0400BE3C RID: 48700
				public static LocString LABEL_UNLOAD = "Unload Only";

				// Token: 0x0400BE3D RID: 48701
				public static LocString LABEL_BOTH = "Load/Unload";

				// Token: 0x0400BE3E RID: 48702
				public static LocString LABEL_LOAD = "Load Only";

				// Token: 0x0400BE3F RID: 48703
				public static readonly List<LocString> LABELS = new List<LocString>
				{
					UI.UISIDESCREENS.MODULAR_CONDUIT_PORT_SIDE_SCREEN.LABEL_UNLOAD,
					UI.UISIDESCREENS.MODULAR_CONDUIT_PORT_SIDE_SCREEN.LABEL_BOTH,
					UI.UISIDESCREENS.MODULAR_CONDUIT_PORT_SIDE_SCREEN.LABEL_LOAD
				};

				// Token: 0x0400BE40 RID: 48704
				public static LocString TOOLTIP_UNLOAD = "This pump will attempt to <b>Unload</b> cargo from the landed rocket, but not attempt to load new cargo";

				// Token: 0x0400BE41 RID: 48705
				public static LocString TOOLTIP_BOTH = "This pump will both <b>Load</b> and <b>Unload</b> cargo from the landed rocket";

				// Token: 0x0400BE42 RID: 48706
				public static LocString TOOLTIP_LOAD = "This pump will attempt to <b>Load</b> cargo onto the landed rocket, but will not unload it";

				// Token: 0x0400BE43 RID: 48707
				public static readonly List<LocString> TOOLTIPS = new List<LocString>
				{
					UI.UISIDESCREENS.MODULAR_CONDUIT_PORT_SIDE_SCREEN.TOOLTIP_UNLOAD,
					UI.UISIDESCREENS.MODULAR_CONDUIT_PORT_SIDE_SCREEN.TOOLTIP_BOTH,
					UI.UISIDESCREENS.MODULAR_CONDUIT_PORT_SIDE_SCREEN.TOOLTIP_LOAD
				};

				// Token: 0x0400BE44 RID: 48708
				public static LocString DESCRIPTION = "";
			}

			// Token: 0x02002DF8 RID: 11768
			public class LOGIC_BUFFER_SIDE_SCREEN
			{
				// Token: 0x0400BE45 RID: 48709
				public static LocString TITLE = "Buffer Time";

				// Token: 0x0400BE46 RID: 48710
				public static LocString TOOLTIP = "Will continue to send a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " for <b>{0} seconds</b> after receiving a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x02002DF9 RID: 11769
			public class LOGIC_FILTER_SIDE_SCREEN
			{
				// Token: 0x0400BE47 RID: 48711
				public static LocString TITLE = "Filter Time";

				// Token: 0x0400BE48 RID: 48712
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Will only send a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if it receives ",
					UI.FormatAsAutomationState("Green", UI.AutomationState.Active),
					" for longer than <b>{0} seconds</b>"
				});
			}

			// Token: 0x02002DFA RID: 11770
			public class TIME_RANGE_SIDE_SCREEN
			{
				// Token: 0x0400BE49 RID: 48713
				public static LocString TITLE = "Time Schedule";

				// Token: 0x0400BE4A RID: 48714
				public static LocString ON = "Activation Time";

				// Token: 0x0400BE4B RID: 48715
				public static LocString ON_TOOLTIP = string.Concat(new string[]
				{
					"Activation time determines the time of day this sensor should begin sending a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					"\n\nThis sensor sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" {0} through the day"
				});

				// Token: 0x0400BE4C RID: 48716
				public static LocString DURATION = "Active Duration";

				// Token: 0x0400BE4D RID: 48717
				public static LocString DURATION_TOOLTIP = string.Concat(new string[]
				{
					"Active duration determines what percentage of the day this sensor will spend sending a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					"\n\nThis sensor will send a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" for {0} of the day"
				});
			}

			// Token: 0x02002DFB RID: 11771
			public class TIMER_SIDE_SCREEN
			{
				// Token: 0x0400BE4E RID: 48718
				public static LocString TITLE = "Timer";

				// Token: 0x0400BE4F RID: 48719
				public static LocString ON = "Green Duration";

				// Token: 0x0400BE50 RID: 48720
				public static LocString GREEN_DURATION_TOOLTIP = string.Concat(new string[]
				{
					"Green duration determines the amount of time this sensor should send a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					"\n\nThis sensor sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" for {0}"
				});

				// Token: 0x0400BE51 RID: 48721
				public static LocString OFF = "Red Duration";

				// Token: 0x0400BE52 RID: 48722
				public static LocString RED_DURATION_TOOLTIP = string.Concat(new string[]
				{
					"Red duration determines the amount of time this sensor should send a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					"\n\nThis sensor will send a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" for {0}"
				});

				// Token: 0x0400BE53 RID: 48723
				public static LocString CURRENT_TIME = "{0}/{1}";

				// Token: 0x0400BE54 RID: 48724
				public static LocString MODE_LABEL_SECONDS = "Mode: Seconds";

				// Token: 0x0400BE55 RID: 48725
				public static LocString MODE_LABEL_CYCLES = "Mode: Cycles";

				// Token: 0x0400BE56 RID: 48726
				public static LocString RESET_BUTTON = "Reset Timer";

				// Token: 0x0400BE57 RID: 48727
				public static LocString DISABLED = "Timer Disabled";
			}

			// Token: 0x02002DFC RID: 11772
			public class COUNTER_SIDE_SCREEN
			{
				// Token: 0x0400BE58 RID: 48728
				public static LocString TITLE = "Counter";

				// Token: 0x0400BE59 RID: 48729
				public static LocString RESET_BUTTON = "Reset Counter";

				// Token: 0x0400BE5A RID: 48730
				public static LocString DESCRIPTION = "Send " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " when count is reached:";

				// Token: 0x0400BE5B RID: 48731
				public static LocString INCREMENT_MODE = "Mode: Increment";

				// Token: 0x0400BE5C RID: 48732
				public static LocString DECREMENT_MODE = "Mode: Decrement";

				// Token: 0x0400BE5D RID: 48733
				public static LocString ADVANCED_MODE = "Advanced Mode";

				// Token: 0x0400BE5E RID: 48734
				public static LocString CURRENT_COUNT_SIMPLE = "{0} of ";

				// Token: 0x0400BE5F RID: 48735
				public static LocString CURRENT_COUNT_ADVANCED = "{0} % ";

				// Token: 0x02002DFD RID: 11773
				public class TOOLTIPS
				{
					// Token: 0x0400BE60 RID: 48736
					public static LocString ADVANCED_MODE = string.Concat(new string[]
					{
						"In Advanced Mode, the ",
						BUILDINGS.PREFABS.LOGICCOUNTER.NAME,
						" will count from <b>0</b> rather than <b>1</b>. It will reset when the max is reached, and send a ",
						UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
						" as a brief pulse rather than continuously."
					});
				}
			}

			// Token: 0x02002DFE RID: 11774
			public class PASSENGERMODULESIDESCREEN
			{
				// Token: 0x0400BE61 RID: 48737
				public static LocString REQUEST_CREW = "Crew";

				// Token: 0x0400BE62 RID: 48738
				public static LocString REQUEST_CREW_TOOLTIP = "Crew may not leave the module, non crew-must exit";

				// Token: 0x0400BE63 RID: 48739
				public static LocString AUTO_CREW = "Auto";

				// Token: 0x0400BE64 RID: 48740
				public static LocString AUTO_CREW_TOOLTIP = "All Duplicants may enter and exit the module freely until the rocket is ready for launch\n\nBefore launch the crew will automatically be requested";

				// Token: 0x0400BE65 RID: 48741
				public static LocString RELEASE_CREW = "All";

				// Token: 0x0400BE66 RID: 48742
				public static LocString RELEASE_CREW_TOOLTIP = "All Duplicants may enter and exit the module freely";

				// Token: 0x0400BE67 RID: 48743
				public static LocString REQUIRE_SUIT_LABEL = "Atmosuit Required";

				// Token: 0x0400BE68 RID: 48744
				public static LocString REQUIRE_SUIT_LABEL_TOOLTIP = "If checked, Duplicants will be required to wear an Atmo Suit when entering this rocket";

				// Token: 0x0400BE69 RID: 48745
				public static LocString CHANGE_CREW_BUTTON = "Change crew";

				// Token: 0x0400BE6A RID: 48746
				public static LocString CHANGE_CREW_BUTTON_TOOLTIP = "Assign Duplicants to crew this rocket's missions";

				// Token: 0x0400BE6B RID: 48747
				public static LocString ASSIGNED_TO_CREW = "Assigned to crew";

				// Token: 0x0400BE6C RID: 48748
				public static LocString UNASSIGNED = "Unassigned";
			}

			// Token: 0x02002DFF RID: 11775
			public class TIMEDSWITCHSIDESCREEN
			{
				// Token: 0x0400BE6D RID: 48749
				public static LocString TITLE = "Time Schedule";

				// Token: 0x0400BE6E RID: 48750
				public static LocString ONTIME = "On Time:";

				// Token: 0x0400BE6F RID: 48751
				public static LocString OFFTIME = "Off Time:";

				// Token: 0x0400BE70 RID: 48752
				public static LocString TIMETODEACTIVATE = "Time until deactivation: {0}";

				// Token: 0x0400BE71 RID: 48753
				public static LocString TIMETOACTIVATE = "Time until activation: {0}";

				// Token: 0x0400BE72 RID: 48754
				public static LocString WARNING = "Switch must be connected to a " + UI.FormatAsLink("Power", "POWER") + " grid";

				// Token: 0x0400BE73 RID: 48755
				public static LocString CURRENTSTATE = "Current State:";

				// Token: 0x0400BE74 RID: 48756
				public static LocString ON = "On";

				// Token: 0x0400BE75 RID: 48757
				public static LocString OFF = "Off";
			}

			// Token: 0x02002E00 RID: 11776
			public class CAPTURE_POINT_SIDE_SCREEN
			{
				// Token: 0x0400BE76 RID: 48758
				public static LocString TITLE = "Stable Management";

				// Token: 0x0400BE77 RID: 48759
				public static LocString AUTOWRANGLE = "Auto-Wrangle Surplus";

				// Token: 0x0400BE78 RID: 48760
				public static LocString AUTOWRANGLE_TOOLTIP = string.Concat(new string[]
				{
					"A Duplicant will automatically wrangle any critters that exceed the population limit or that do not belong in this stable\n\nDuplicants must possess the ",
					UI.PRE_KEYWORD,
					"Critter Ranching",
					UI.PST_KEYWORD,
					" skill in order to wrangle critters"
				});

				// Token: 0x0400BE79 RID: 48761
				public static LocString LIMIT_TOOLTIP = "Critters exceeding this population limit will automatically be wrangled:";

				// Token: 0x0400BE7A RID: 48762
				public static LocString UNITS_SUFFIX = " Critters";
			}

			// Token: 0x02002E01 RID: 11777
			public class TEMPERATURESWITCHSIDESCREEN
			{
				// Token: 0x0400BE7B RID: 48763
				public static LocString TITLE = "Temperature Threshold";

				// Token: 0x0400BE7C RID: 48764
				public static LocString CURRENT_TEMPERATURE = "Current Temperature:\n{0}";

				// Token: 0x0400BE7D RID: 48765
				public static LocString ACTIVATE_IF = "Send " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if:";

				// Token: 0x0400BE7E RID: 48766
				public static LocString COLDER_BUTTON = "Below";

				// Token: 0x0400BE7F RID: 48767
				public static LocString WARMER_BUTTON = "Above";
			}

			// Token: 0x02002E02 RID: 11778
			public class BRIGHTNESSSWITCHSIDESCREEN
			{
				// Token: 0x0400BE80 RID: 48768
				public static LocString TITLE = "Brightness Threshold";

				// Token: 0x0400BE81 RID: 48769
				public static LocString CURRENT_TEMPERATURE = "Current Brightness:\n{0}";

				// Token: 0x0400BE82 RID: 48770
				public static LocString ACTIVATE_IF = "Send " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if:";

				// Token: 0x0400BE83 RID: 48771
				public static LocString COLDER_BUTTON = "Below";

				// Token: 0x0400BE84 RID: 48772
				public static LocString WARMER_BUTTON = "Above";
			}

			// Token: 0x02002E03 RID: 11779
			public class RADIATIONSWITCHSIDESCREEN
			{
				// Token: 0x0400BE85 RID: 48773
				public static LocString TITLE = "Radiation Threshold";

				// Token: 0x0400BE86 RID: 48774
				public static LocString CURRENT_TEMPERATURE = "Current Radiation:\n{0}/cycle";

				// Token: 0x0400BE87 RID: 48775
				public static LocString ACTIVATE_IF = "Send " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if:";

				// Token: 0x0400BE88 RID: 48776
				public static LocString COLDER_BUTTON = "Below";

				// Token: 0x0400BE89 RID: 48777
				public static LocString WARMER_BUTTON = "Above";
			}

			// Token: 0x02002E04 RID: 11780
			public class WATTAGESWITCHSIDESCREEN
			{
				// Token: 0x0400BE8A RID: 48778
				public static LocString TITLE = "Wattage Threshold";

				// Token: 0x0400BE8B RID: 48779
				public static LocString CURRENT_TEMPERATURE = "Current Wattage:\n{0}";

				// Token: 0x0400BE8C RID: 48780
				public static LocString ACTIVATE_IF = "Send " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if:";

				// Token: 0x0400BE8D RID: 48781
				public static LocString COLDER_BUTTON = "Below";

				// Token: 0x0400BE8E RID: 48782
				public static LocString WARMER_BUTTON = "Above";
			}

			// Token: 0x02002E05 RID: 11781
			public class HEPSWITCHSIDESCREEN
			{
				// Token: 0x0400BE8F RID: 48783
				public static LocString TITLE = "Radbolt Threshold";
			}

			// Token: 0x02002E06 RID: 11782
			public class THRESHOLD_SWITCH_SIDESCREEN
			{
				// Token: 0x0400BE90 RID: 48784
				public static LocString TITLE = "Pressure";

				// Token: 0x0400BE91 RID: 48785
				public static LocString THRESHOLD_SUBTITLE = "Threshold:";

				// Token: 0x0400BE92 RID: 48786
				public static LocString CURRENT_VALUE = "Current {0}:\n{1}";

				// Token: 0x0400BE93 RID: 48787
				public static LocString ACTIVATE_IF = "Send " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if:";

				// Token: 0x0400BE94 RID: 48788
				public static LocString ABOVE_BUTTON = "Above";

				// Token: 0x0400BE95 RID: 48789
				public static LocString BELOW_BUTTON = "Below";

				// Token: 0x0400BE96 RID: 48790
				public static LocString STATUS_ACTIVE = "Switch Active";

				// Token: 0x0400BE97 RID: 48791
				public static LocString STATUS_INACTIVE = "Switch Inactive";

				// Token: 0x0400BE98 RID: 48792
				public static LocString PRESSURE = "Ambient Pressure";

				// Token: 0x0400BE99 RID: 48793
				public static LocString PRESSURE_TOOLTIP_ABOVE = string.Concat(new string[]
				{
					"Will send a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if the ",
					UI.PRE_KEYWORD,
					"Pressure",
					UI.PST_KEYWORD,
					" is above <b>{0}</b>"
				});

				// Token: 0x0400BE9A RID: 48794
				public static LocString PRESSURE_TOOLTIP_BELOW = string.Concat(new string[]
				{
					"Will send a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if the ",
					UI.PRE_KEYWORD,
					"Pressure",
					UI.PST_KEYWORD,
					" is below <b>{0}</b>"
				});

				// Token: 0x0400BE9B RID: 48795
				public static LocString TEMPERATURE = "Ambient Temperature";

				// Token: 0x0400BE9C RID: 48796
				public static LocString TEMPERATURE_TOOLTIP_ABOVE = string.Concat(new string[]
				{
					"Will send a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if the ambient ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" is above <b>{0}</b>"
				});

				// Token: 0x0400BE9D RID: 48797
				public static LocString TEMPERATURE_TOOLTIP_BELOW = string.Concat(new string[]
				{
					"Will send a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if the ambient ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" is below <b>{0}</b>"
				});

				// Token: 0x0400BE9E RID: 48798
				public static LocString CONTENT_TEMPERATURE = "Internal Temperature";

				// Token: 0x0400BE9F RID: 48799
				public static LocString CONTENT_TEMPERATURE_TOOLTIP_ABOVE = string.Concat(new string[]
				{
					"Will send a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if the ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" of its contents is above <b>{0}</b>"
				});

				// Token: 0x0400BEA0 RID: 48800
				public static LocString CONTENT_TEMPERATURE_TOOLTIP_BELOW = string.Concat(new string[]
				{
					"Will send a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if the ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" of its contents is below <b>{0}</b>"
				});

				// Token: 0x0400BEA1 RID: 48801
				public static LocString BRIGHTNESS = "Ambient Brightness";

				// Token: 0x0400BEA2 RID: 48802
				public static LocString BRIGHTNESS_TOOLTIP_ABOVE = string.Concat(new string[]
				{
					"Will send a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if the ambient ",
					UI.PRE_KEYWORD,
					"Brightness",
					UI.PST_KEYWORD,
					" is above <b>{0}</b>"
				});

				// Token: 0x0400BEA3 RID: 48803
				public static LocString BRIGHTNESS_TOOLTIP_BELOW = string.Concat(new string[]
				{
					"Will send a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if the ambient ",
					UI.PRE_KEYWORD,
					"Brightness",
					UI.PST_KEYWORD,
					" is below <b>{0}</b>"
				});

				// Token: 0x0400BEA4 RID: 48804
				public static LocString WATTAGE = "Wattage Reading";

				// Token: 0x0400BEA5 RID: 48805
				public static LocString WATTAGE_TOOLTIP_ABOVE = string.Concat(new string[]
				{
					"Will send a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if the ",
					UI.PRE_KEYWORD,
					"Wattage",
					UI.PST_KEYWORD,
					" consumed is above <b>{0}</b>"
				});

				// Token: 0x0400BEA6 RID: 48806
				public static LocString WATTAGE_TOOLTIP_BELOW = string.Concat(new string[]
				{
					"Will send a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if the ",
					UI.PRE_KEYWORD,
					"Wattage",
					UI.PST_KEYWORD,
					" consumed is below <b>{0}</b>"
				});

				// Token: 0x0400BEA7 RID: 48807
				public static LocString DISEASE_TITLE = "Germ Threshold";

				// Token: 0x0400BEA8 RID: 48808
				public static LocString DISEASE = "Ambient Germs";

				// Token: 0x0400BEA9 RID: 48809
				public static LocString DISEASE_TOOLTIP_ABOVE = string.Concat(new string[]
				{
					"Will send a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if the number of ",
					UI.PRE_KEYWORD,
					"Germs",
					UI.PST_KEYWORD,
					" is above <b>{0}</b>"
				});

				// Token: 0x0400BEAA RID: 48810
				public static LocString DISEASE_TOOLTIP_BELOW = string.Concat(new string[]
				{
					"Will send a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if the number of ",
					UI.PRE_KEYWORD,
					"Germs",
					UI.PST_KEYWORD,
					" is below <b>{0}</b>"
				});

				// Token: 0x0400BEAB RID: 48811
				public static LocString DISEASE_UNITS = "";

				// Token: 0x0400BEAC RID: 48812
				public static LocString CONTENT_DISEASE = "Germ Count";

				// Token: 0x0400BEAD RID: 48813
				public static LocString RADIATION = "Ambient Radiation";

				// Token: 0x0400BEAE RID: 48814
				public static LocString RADIATION_TOOLTIP_ABOVE = string.Concat(new string[]
				{
					"Will send a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if the ambient ",
					UI.PRE_KEYWORD,
					"Radiation",
					UI.PST_KEYWORD,
					" is above <b>{0}</b>"
				});

				// Token: 0x0400BEAF RID: 48815
				public static LocString RADIATION_TOOLTIP_BELOW = string.Concat(new string[]
				{
					"Will send a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if the ambient ",
					UI.PRE_KEYWORD,
					"Radiation",
					UI.PST_KEYWORD,
					" is below <b>{0}</b>"
				});

				// Token: 0x0400BEB0 RID: 48816
				public static LocString HEPS = "Radbolt Reading";

				// Token: 0x0400BEB1 RID: 48817
				public static LocString HEPS_TOOLTIP_ABOVE = string.Concat(new string[]
				{
					"Will send a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if the ",
					UI.PRE_KEYWORD,
					"Radbolts",
					UI.PST_KEYWORD,
					" is above <b>{0}</b>"
				});

				// Token: 0x0400BEB2 RID: 48818
				public static LocString HEPS_TOOLTIP_BELOW = string.Concat(new string[]
				{
					"Will send a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if the ",
					UI.PRE_KEYWORD,
					"Radbolts",
					UI.PST_KEYWORD,
					" is below <b>{0}</b>"
				});
			}

			// Token: 0x02002E07 RID: 11783
			public class CAPACITY_CONTROL_SIDE_SCREEN
			{
				// Token: 0x0400BEB3 RID: 48819
				public static LocString TITLE = "Automated Storage Capacity";

				// Token: 0x0400BEB4 RID: 48820
				public static LocString MAX_LABEL = "Max:";
			}

			// Token: 0x02002E08 RID: 11784
			public class DOOR_TOGGLE_SIDE_SCREEN
			{
				// Token: 0x0400BEB5 RID: 48821
				public static LocString TITLE = "Door Setting";

				// Token: 0x0400BEB6 RID: 48822
				public static LocString OPEN = "Door is open.";

				// Token: 0x0400BEB7 RID: 48823
				public static LocString AUTO = "Door is on auto.";

				// Token: 0x0400BEB8 RID: 48824
				public static LocString CLOSE = "Door is locked.";

				// Token: 0x0400BEB9 RID: 48825
				public static LocString PENDING_FORMAT = "{0} {1}";

				// Token: 0x0400BEBA RID: 48826
				public static LocString OPEN_PENDING = "Awaiting Duplicant to open door.";

				// Token: 0x0400BEBB RID: 48827
				public static LocString AUTO_PENDING = "Awaiting Duplicant to automate door.";

				// Token: 0x0400BEBC RID: 48828
				public static LocString CLOSE_PENDING = "Awaiting Duplicant to lock door.";

				// Token: 0x0400BEBD RID: 48829
				public static LocString ACCESS_FORMAT = "{0}\n\n{1}";

				// Token: 0x0400BEBE RID: 48830
				public static LocString ACCESS_OFFLINE = "Emergency Access Permissions:\nAll Duplicants are permitted to use this door until " + UI.FormatAsLink("Power", "POWER") + " is restored.";

				// Token: 0x0400BEBF RID: 48831
				public static LocString POI_INTERNAL = "This door cannot be manually controlled.";
			}

			// Token: 0x02002E09 RID: 11785
			public class ACTIVATION_RANGE_SIDE_SCREEN
			{
				// Token: 0x0400BEC0 RID: 48832
				public static LocString NAME = "Breaktime Policy";

				// Token: 0x0400BEC1 RID: 48833
				public static LocString ACTIVATE = "Break starts at:";

				// Token: 0x0400BEC2 RID: 48834
				public static LocString DEACTIVATE = "Break ends at:";
			}

			// Token: 0x02002E0A RID: 11786
			public class CAPACITY_SIDE_SCREEN
			{
				// Token: 0x0400BEC3 RID: 48835
				public static LocString TOOLTIP = "Adjust the maximum amount that can be stored here";
			}

			// Token: 0x02002E0B RID: 11787
			public class SUIT_SIDE_SCREEN
			{
				// Token: 0x0400BEC4 RID: 48836
				public static LocString TITLE = "Dock Inventory";

				// Token: 0x0400BEC5 RID: 48837
				public static LocString CONFIGURATION_REQUIRED = "Configuration Required:";

				// Token: 0x0400BEC6 RID: 48838
				public static LocString CONFIG_REQUEST_SUIT = "Deliver Suit";

				// Token: 0x0400BEC7 RID: 48839
				public static LocString CONFIG_REQUEST_SUIT_TOOLTIP = "Duplicants will immediately deliver and dock the nearest unequipped suit";

				// Token: 0x0400BEC8 RID: 48840
				public static LocString CONFIG_NO_SUIT = "Leave Empty";

				// Token: 0x0400BEC9 RID: 48841
				public static LocString CONFIG_NO_SUIT_TOOLTIP = "The next suited Duplicant to pass by will unequip their suit and dock it here";

				// Token: 0x0400BECA RID: 48842
				public static LocString CONFIG_CANCEL_REQUEST = "Cancel Request";

				// Token: 0x0400BECB RID: 48843
				public static LocString CONFIG_CANCEL_REQUEST_TOOLTIP = "Cancel this suit delivery";

				// Token: 0x0400BECC RID: 48844
				public static LocString CONFIG_DROP_SUIT = "Undock Suit";

				// Token: 0x0400BECD RID: 48845
				public static LocString CONFIG_DROP_SUIT_TOOLTIP = "Disconnect this suit, dropping it on the ground";

				// Token: 0x0400BECE RID: 48846
				public static LocString CONFIG_DROP_SUIT_NO_SUIT_TOOLTIP = "There is no suit in this building to undock";
			}

			// Token: 0x02002E0C RID: 11788
			public class AUTOMATABLE_SIDE_SCREEN
			{
				// Token: 0x0400BECF RID: 48847
				public static LocString TITLE = "Automatable Storage";

				// Token: 0x0400BED0 RID: 48848
				public static LocString ALLOWMANUALBUTTON = "Allow Manual Use";

				// Token: 0x0400BED1 RID: 48849
				public static LocString ALLOWMANUALBUTTONTOOLTIP = "Allow Duplicants to manually manage these storage materials";
			}

			// Token: 0x02002E0D RID: 11789
			public class STUDYABLE_SIDE_SCREEN
			{
				// Token: 0x0400BED2 RID: 48850
				public static LocString TITLE = "Analyze Natural Feature";

				// Token: 0x0400BED3 RID: 48851
				public static LocString STUDIED_STATUS = "Researchers have completed their analysis and compiled their findings.";

				// Token: 0x0400BED4 RID: 48852
				public static LocString STUDIED_BUTTON = "ANALYSIS COMPLETE";

				// Token: 0x0400BED5 RID: 48853
				public static LocString SEND_STATUS = "Send a researcher to gather data here.\n\nAnalyzing a feature takes time, but yields useful results.";

				// Token: 0x0400BED6 RID: 48854
				public static LocString SEND_BUTTON = "ANALYZE";

				// Token: 0x0400BED7 RID: 48855
				public static LocString PENDING_STATUS = "A researcher is in the process of studying this feature.";

				// Token: 0x0400BED8 RID: 48856
				public static LocString PENDING_BUTTON = "CANCEL ANALYSIS";
			}

			// Token: 0x02002E0E RID: 11790
			public class MEDICALCOTSIDESCREEN
			{
				// Token: 0x0400BED9 RID: 48857
				public static LocString TITLE = "Severity Requirement";

				// Token: 0x0400BEDA RID: 48858
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"A Duplicant may not use this cot until their ",
					UI.PRE_KEYWORD,
					"Health",
					UI.PST_KEYWORD,
					" falls below <b>{0}%</b>"
				});
			}

			// Token: 0x02002E0F RID: 11791
			public class WARPPORTALSIDESCREEN
			{
				// Token: 0x0400BEDB RID: 48859
				public static LocString TITLE = "Teleporter";

				// Token: 0x0400BEDC RID: 48860
				public static LocString IDLE = "Teleporter online.\nPlease select a passenger:";

				// Token: 0x0400BEDD RID: 48861
				public static LocString WAITING = "Ready to transmit passenger.";

				// Token: 0x0400BEDE RID: 48862
				public static LocString COMPLETE = "Passenger transmitted!";

				// Token: 0x0400BEDF RID: 48863
				public static LocString UNDERWAY = "Transmitting passenger...";

				// Token: 0x0400BEE0 RID: 48864
				public static LocString CONSUMED = "Teleporter recharging:";

				// Token: 0x0400BEE1 RID: 48865
				public static LocString BUTTON = "Teleport!";

				// Token: 0x0400BEE2 RID: 48866
				public static LocString CANCELBUTTON = "Cancel";
			}

			// Token: 0x02002E10 RID: 11792
			public class RADBOLTTHRESHOLDSIDESCREEN
			{
				// Token: 0x0400BEE3 RID: 48867
				public static LocString TITLE = "Radbolt Threshold";

				// Token: 0x0400BEE4 RID: 48868
				public static LocString CURRENT_THRESHOLD = "Current Threshold: {0}%";

				// Token: 0x0400BEE5 RID: 48869
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Releases a ",
					UI.PRE_KEYWORD,
					"Radbolt",
					UI.PST_KEYWORD,
					" when stored Radbolts exceed <b>{0}</b>"
				});

				// Token: 0x0400BEE6 RID: 48870
				public static LocString PROGRESS_BAR_LABEL = "Radbolt Generation";

				// Token: 0x0400BEE7 RID: 48871
				public static LocString PROGRESS_BAR_TOOLTIP = string.Concat(new string[]
				{
					"The building will emit a ",
					UI.PRE_KEYWORD,
					"Radbolt",
					UI.PST_KEYWORD,
					" in the chosen direction when fully charged"
				});
			}

			// Token: 0x02002E11 RID: 11793
			public class LOGICBITSELECTORSIDESCREEN
			{
				// Token: 0x0400BEE8 RID: 48872
				public static LocString RIBBON_READER_TITLE = "Ribbon Reader";

				// Token: 0x0400BEE9 RID: 48873
				public static LocString RIBBON_READER_DESCRIPTION = "Selected <b>Bit's Signal</b> will be read by the <b>Output Port</b>";

				// Token: 0x0400BEEA RID: 48874
				public static LocString RIBBON_WRITER_TITLE = "Ribbon Writer";

				// Token: 0x0400BEEB RID: 48875
				public static LocString RIBBON_WRITER_DESCRIPTION = "Received <b>Signal</b> will be written to selected <b>Bit</b>";

				// Token: 0x0400BEEC RID: 48876
				public static LocString BIT = "Bit {0}";

				// Token: 0x0400BEED RID: 48877
				public static LocString STATE_ACTIVE = "Green";

				// Token: 0x0400BEEE RID: 48878
				public static LocString STATE_INACTIVE = "Red";
			}

			// Token: 0x02002E12 RID: 11794
			public class LOGICALARMSIDESCREEN
			{
				// Token: 0x0400BEEF RID: 48879
				public static LocString TITLE = "Notification Designer";

				// Token: 0x0400BEF0 RID: 48880
				public static LocString DESCRIPTION = "Notification will be sent upon receiving a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + "\n\nMaking modifications will clear any existing notifications being sent by this building.";

				// Token: 0x0400BEF1 RID: 48881
				public static LocString NAME = "<b>Name:</b>";

				// Token: 0x0400BEF2 RID: 48882
				public static LocString NAME_DEFAULT = "Notification";

				// Token: 0x0400BEF3 RID: 48883
				public static LocString TOOLTIP = "<b>Tooltip:</b>";

				// Token: 0x0400BEF4 RID: 48884
				public static LocString TOOLTIP_DEFAULT = "Tooltip";

				// Token: 0x0400BEF5 RID: 48885
				public static LocString TYPE = "<b>Type:</b>";

				// Token: 0x0400BEF6 RID: 48886
				public static LocString PAUSE = "<b>Pause:</b>";

				// Token: 0x0400BEF7 RID: 48887
				public static LocString ZOOM = "<b>Zoom:</b>";

				// Token: 0x02002E13 RID: 11795
				public class TOOLTIPS
				{
					// Token: 0x0400BEF8 RID: 48888
					public static LocString NAME = "Select notification text";

					// Token: 0x0400BEF9 RID: 48889
					public static LocString TOOLTIP = "Select notification hover text";

					// Token: 0x0400BEFA RID: 48890
					public static LocString TYPE = "Select the visual and aural style of the notification";

					// Token: 0x0400BEFB RID: 48891
					public static LocString PAUSE = "Time will pause upon notification when checked";

					// Token: 0x0400BEFC RID: 48892
					public static LocString ZOOM = "The view will zoom to this building upon notification when checked";

					// Token: 0x0400BEFD RID: 48893
					public static LocString BAD = "\"Boing boing!\"";

					// Token: 0x0400BEFE RID: 48894
					public static LocString NEUTRAL = "\"Pop!\"";

					// Token: 0x0400BEFF RID: 48895
					public static LocString DUPLICANT_THREATENING = "AHH!";
				}
			}

			// Token: 0x02002E14 RID: 11796
			public class GENETICANALYSISSIDESCREEN
			{
				// Token: 0x0400BF00 RID: 48896
				public static LocString TITLE = "Genetic Analysis";

				// Token: 0x0400BF01 RID: 48897
				public static LocString NONE_DISCOVERED = "No mutant seeds have been found.";

				// Token: 0x0400BF02 RID: 48898
				public static LocString SELECT_SEEDS = "Select which seed types to analyze:";

				// Token: 0x0400BF03 RID: 48899
				public static LocString SEED_NO_MUTANTS = "</i>No mutants found</i>";

				// Token: 0x0400BF04 RID: 48900
				public static LocString SEED_FORBIDDEN = "</i>Won't analyze</i>";

				// Token: 0x0400BF05 RID: 48901
				public static LocString SEED_ALLOWED = "</i>Will analyze</i>";
			}

			// Token: 0x02002E15 RID: 11797
			public class RELATEDENTITIESSIDESCREEN
			{
				// Token: 0x0400BF06 RID: 48902
				public static LocString TITLE = "Related Objects";
			}
		}

		// Token: 0x02002E16 RID: 11798
		public class USERMENUACTIONS
		{
			// Token: 0x02002E17 RID: 11799
			public class TINKER
			{
				// Token: 0x0400BF07 RID: 48903
				public static LocString ALLOW = "Allow Tinker";

				// Token: 0x0400BF08 RID: 48904
				public static LocString DISALLOW = "Disallow Tinker";

				// Token: 0x0400BF09 RID: 48905
				public static LocString TOOLTIP_DISALLOW = "Disallow Tinker Tool application on this building";

				// Token: 0x0400BF0A RID: 48906
				public static LocString TOOLTIP_ALLOW = "Allow Tinker Tool application on this building";
			}

			// Token: 0x02002E18 RID: 11800
			public class TRANSITTUBEWAX
			{
				// Token: 0x0400BF0B RID: 48907
				public static LocString NAME = "Enable Smooth Ride";

				// Token: 0x0400BF0C RID: 48908
				public static LocString TOOLTIP = "Enables the use of " + ELEMENTS.MILKFAT.NAME + " to boost travel speed";
			}

			// Token: 0x02002E19 RID: 11801
			public class CANCELTRANSITTUBEWAX
			{
				// Token: 0x0400BF0D RID: 48909
				public static LocString NAME = "Disable Smooth Ride";

				// Token: 0x0400BF0E RID: 48910
				public static LocString TOOLTIP = "Disables travel speed boost and refunds stored " + ELEMENTS.MILKFAT.NAME;
			}

			// Token: 0x02002E1A RID: 11802
			public class CLEANTOILET
			{
				// Token: 0x0400BF0F RID: 48911
				public static LocString NAME = "Clean Toilet";

				// Token: 0x0400BF10 RID: 48912
				public static LocString TOOLTIP = "Empty waste from this toilet";
			}

			// Token: 0x02002E1B RID: 11803
			public class CANCELCLEANTOILET
			{
				// Token: 0x0400BF11 RID: 48913
				public static LocString NAME = "Cancel Clean";

				// Token: 0x0400BF12 RID: 48914
				public static LocString TOOLTIP = "Cancel this cleaning order";
			}

			// Token: 0x02002E1C RID: 11804
			public class EMPTYBEEHIVE
			{
				// Token: 0x0400BF13 RID: 48915
				public static LocString NAME = "Enable Autoharvest";

				// Token: 0x0400BF14 RID: 48916
				public static LocString TOOLTIP = "Automatically harvest this hive when full";
			}

			// Token: 0x02002E1D RID: 11805
			public class CANCELEMPTYBEEHIVE
			{
				// Token: 0x0400BF15 RID: 48917
				public static LocString NAME = "Disable Autoharvest";

				// Token: 0x0400BF16 RID: 48918
				public static LocString TOOLTIP = "Do not automatically harvest this hive";
			}

			// Token: 0x02002E1E RID: 11806
			public class EMPTYDESALINATOR
			{
				// Token: 0x0400BF17 RID: 48919
				public static LocString NAME = "Empty Desalinator";

				// Token: 0x0400BF18 RID: 48920
				public static LocString TOOLTIP = "Empty salt from this desalinator";
			}

			// Token: 0x02002E1F RID: 11807
			public class CHANGE_ROOM
			{
				// Token: 0x0400BF19 RID: 48921
				public static LocString REQUEST_OUTFIT = "Request Outfit";

				// Token: 0x0400BF1A RID: 48922
				public static LocString REQUEST_OUTFIT_TOOLTIP = "Request outfit to be delivered to this change room";

				// Token: 0x0400BF1B RID: 48923
				public static LocString CANCEL_REQUEST = "Cancel Request";

				// Token: 0x0400BF1C RID: 48924
				public static LocString CANCEL_REQUEST_TOOLTIP = "Cancel outfit request";

				// Token: 0x0400BF1D RID: 48925
				public static LocString DROP_OUTFIT = "Drop Outfit";

				// Token: 0x0400BF1E RID: 48926
				public static LocString DROP_OUTFIT_TOOLTIP = "Drop outfit on floor";
			}

			// Token: 0x02002E20 RID: 11808
			public class DUMP
			{
				// Token: 0x0400BF1F RID: 48927
				public static LocString NAME = "Empty";

				// Token: 0x0400BF20 RID: 48928
				public static LocString TOOLTIP = "Dump bottle contents onto the floor";

				// Token: 0x0400BF21 RID: 48929
				public static LocString NAME_OFF = "Cancel Empty";

				// Token: 0x0400BF22 RID: 48930
				public static LocString TOOLTIP_OFF = "Cancel this empty order";
			}

			// Token: 0x02002E21 RID: 11809
			public class TAGFILTER
			{
				// Token: 0x0400BF23 RID: 48931
				public static LocString NAME = "Filter Settings";

				// Token: 0x0400BF24 RID: 48932
				public static LocString TOOLTIP = "Assign materials to storage";
			}

			// Token: 0x02002E22 RID: 11810
			public class CANCELCONSTRUCTION
			{
				// Token: 0x0400BF25 RID: 48933
				public static LocString NAME = "Cancel Build";

				// Token: 0x0400BF26 RID: 48934
				public static LocString TOOLTIP = "Cancel this build order";
			}

			// Token: 0x02002E23 RID: 11811
			public class DIG
			{
				// Token: 0x0400BF27 RID: 48935
				public static LocString NAME = "Dig";

				// Token: 0x0400BF28 RID: 48936
				public static LocString TOOLTIP = "Dig out this cell";

				// Token: 0x0400BF29 RID: 48937
				public static LocString TOOLTIP_OFF = "Cancel this dig order";
			}

			// Token: 0x02002E24 RID: 11812
			public class CANCELMOP
			{
				// Token: 0x0400BF2A RID: 48938
				public static LocString NAME = "Cancel Mop";

				// Token: 0x0400BF2B RID: 48939
				public static LocString TOOLTIP = "Cancel this mop order";
			}

			// Token: 0x02002E25 RID: 11813
			public class CANCELDIG
			{
				// Token: 0x0400BF2C RID: 48940
				public static LocString NAME = "Cancel Dig";

				// Token: 0x0400BF2D RID: 48941
				public static LocString TOOLTIP = "Cancel this dig order";
			}

			// Token: 0x02002E26 RID: 11814
			public class UPROOT
			{
				// Token: 0x0400BF2E RID: 48942
				public static LocString NAME = "Uproot";

				// Token: 0x0400BF2F RID: 48943
				public static LocString TOOLTIP = "Convert this plant into a seed";
			}

			// Token: 0x02002E27 RID: 11815
			public class CANCELUPROOT
			{
				// Token: 0x0400BF30 RID: 48944
				public static LocString NAME = "Cancel Uproot";

				// Token: 0x0400BF31 RID: 48945
				public static LocString TOOLTIP = "Cancel this uproot order";
			}

			// Token: 0x02002E28 RID: 11816
			public class HARVEST_WHEN_READY
			{
				// Token: 0x0400BF32 RID: 48946
				public static LocString NAME = "Enable Autoharvest";

				// Token: 0x0400BF33 RID: 48947
				public static LocString TOOLTIP = "Automatically harvest this plant when it matures";
			}

			// Token: 0x02002E29 RID: 11817
			public class CANCEL_HARVEST_WHEN_READY
			{
				// Token: 0x0400BF34 RID: 48948
				public static LocString NAME = "Disable Autoharvest";

				// Token: 0x0400BF35 RID: 48949
				public static LocString TOOLTIP = "Do not automatically harvest this plant";
			}

			// Token: 0x02002E2A RID: 11818
			public class HARVEST
			{
				// Token: 0x0400BF36 RID: 48950
				public static LocString NAME = "Harvest";

				// Token: 0x0400BF37 RID: 48951
				public static LocString TOOLTIP = "Harvest materials from this plant";

				// Token: 0x0400BF38 RID: 48952
				public static LocString TOOLTIP_DISABLED = "This plant has nothing to harvest";
			}

			// Token: 0x02002E2B RID: 11819
			public class CANCELHARVEST
			{
				// Token: 0x0400BF39 RID: 48953
				public static LocString NAME = "Cancel Harvest";

				// Token: 0x0400BF3A RID: 48954
				public static LocString TOOLTIP = "Cancel this harvest order";
			}

			// Token: 0x02002E2C RID: 11820
			public class ATTACK
			{
				// Token: 0x0400BF3B RID: 48955
				public static LocString NAME = "Attack";

				// Token: 0x0400BF3C RID: 48956
				public static LocString TOOLTIP = "Attack this critter";
			}

			// Token: 0x02002E2D RID: 11821
			public class CANCELATTACK
			{
				// Token: 0x0400BF3D RID: 48957
				public static LocString NAME = "Cancel Attack";

				// Token: 0x0400BF3E RID: 48958
				public static LocString TOOLTIP = "Cancel this attack order";
			}

			// Token: 0x02002E2E RID: 11822
			public class CAPTURE
			{
				// Token: 0x0400BF3F RID: 48959
				public static LocString NAME = "Wrangle";

				// Token: 0x0400BF40 RID: 48960
				public static LocString TOOLTIP = "Capture this critter alive";
			}

			// Token: 0x02002E2F RID: 11823
			public class CANCELCAPTURE
			{
				// Token: 0x0400BF41 RID: 48961
				public static LocString NAME = "Cancel Wrangle";

				// Token: 0x0400BF42 RID: 48962
				public static LocString TOOLTIP = "Cancel this wrangle order";
			}

			// Token: 0x02002E30 RID: 11824
			public class RELEASEELEMENT
			{
				// Token: 0x0400BF43 RID: 48963
				public static LocString NAME = "Empty Building";

				// Token: 0x0400BF44 RID: 48964
				public static LocString TOOLTIP = "Refund all resources currently in use by this building";
			}

			// Token: 0x02002E31 RID: 11825
			public class DECONSTRUCT
			{
				// Token: 0x0400BF45 RID: 48965
				public static LocString NAME = "Deconstruct";

				// Token: 0x0400BF46 RID: 48966
				public static LocString TOOLTIP = "Deconstruct this building and refund all resources";

				// Token: 0x0400BF47 RID: 48967
				public static LocString NAME_OFF = "Cancel Deconstruct";

				// Token: 0x0400BF48 RID: 48968
				public static LocString TOOLTIP_OFF = "Cancel this deconstruct order";
			}

			// Token: 0x02002E32 RID: 11826
			public class DEMOLISH
			{
				// Token: 0x0400BF49 RID: 48969
				public static LocString NAME = "Demolish";

				// Token: 0x0400BF4A RID: 48970
				public static LocString TOOLTIP = "Demolish this building";

				// Token: 0x0400BF4B RID: 48971
				public static LocString NAME_OFF = "Cancel Demolition";

				// Token: 0x0400BF4C RID: 48972
				public static LocString TOOLTIP_OFF = "Cancel this demolition order";
			}

			// Token: 0x02002E33 RID: 11827
			public class ROCKETUSAGERESTRICTION
			{
				// Token: 0x0400BF4D RID: 48973
				public static LocString NAME_UNCONTROLLED = "Uncontrolled";

				// Token: 0x0400BF4E RID: 48974
				public static LocString TOOLTIP_UNCONTROLLED = "Do not allow this building to be controlled by a " + BUILDINGS.PREFABS.ROCKETCONTROLSTATION.NAME;

				// Token: 0x0400BF4F RID: 48975
				public static LocString NAME_CONTROLLED = "Controlled";

				// Token: 0x0400BF50 RID: 48976
				public static LocString TOOLTIP_CONTROLLED = "Allow this building's operation to be controlled by a " + BUILDINGS.PREFABS.ROCKETCONTROLSTATION.NAME;
			}

			// Token: 0x02002E34 RID: 11828
			public class MANUAL_DELIVERY
			{
				// Token: 0x0400BF51 RID: 48977
				public static LocString NAME = "Disable Delivery";

				// Token: 0x0400BF52 RID: 48978
				public static LocString TOOLTIP = "Do not deliver materials to this building";

				// Token: 0x0400BF53 RID: 48979
				public static LocString NAME_OFF = "Enable Delivery";

				// Token: 0x0400BF54 RID: 48980
				public static LocString TOOLTIP_OFF = "Deliver materials to this building";
			}

			// Token: 0x02002E35 RID: 11829
			public class SELECTRESEARCH
			{
				// Token: 0x0400BF55 RID: 48981
				public static LocString NAME = "Select Research";

				// Token: 0x0400BF56 RID: 48982
				public static LocString TOOLTIP = "Choose a technology from the " + UI.FormatAsManagementMenu("Research Tree", global::Action.ManageResearch);
			}

			// Token: 0x02002E36 RID: 11830
			public class RECONSTRUCT
			{
				// Token: 0x0400BF57 RID: 48983
				public static LocString REQUEST_RECONSTRUCT = "Order Rebuild";

				// Token: 0x0400BF58 RID: 48984
				public static LocString REQUEST_RECONSTRUCT_TOOLTIP = "Deconstruct this building and rebuild it using the selected material";

				// Token: 0x0400BF59 RID: 48985
				public static LocString CANCEL_RECONSTRUCT = "Cancel Rebuild Order";

				// Token: 0x0400BF5A RID: 48986
				public static LocString CANCEL_RECONSTRUCT_TOOLTIP = "Cancel deconstruction and rebuilding of this building";
			}

			// Token: 0x02002E37 RID: 11831
			public class RELOCATE
			{
				// Token: 0x0400BF5B RID: 48987
				public static LocString NAME = "Relocate";

				// Token: 0x0400BF5C RID: 48988
				public static LocString TOOLTIP = "Move this building to a new location\n\nCosts no additional resources";

				// Token: 0x0400BF5D RID: 48989
				public static LocString NAME_OFF = "Cancel Relocation";

				// Token: 0x0400BF5E RID: 48990
				public static LocString TOOLTIP_OFF = "Cancel this relocation order";
			}

			// Token: 0x02002E38 RID: 11832
			public class ENABLEBUILDING
			{
				// Token: 0x0400BF5F RID: 48991
				public static LocString NAME = "Disable Building";

				// Token: 0x0400BF60 RID: 48992
				public static LocString TOOLTIP = "Halt the use of this building {Hotkey}\n\nDisabled buildings consume no energy or resources";

				// Token: 0x0400BF61 RID: 48993
				public static LocString NAME_OFF = "Enable Building";

				// Token: 0x0400BF62 RID: 48994
				public static LocString TOOLTIP_OFF = "Resume the use of this building {Hotkey}";
			}

			// Token: 0x02002E39 RID: 11833
			public class READLORE
			{
				// Token: 0x0400BF63 RID: 48995
				public static LocString NAME = "Inspect";

				// Token: 0x0400BF64 RID: 48996
				public static LocString ALREADYINSPECTED = "Already inspected";

				// Token: 0x0400BF65 RID: 48997
				public static LocString TOOLTIP = "Recover files from this structure";

				// Token: 0x0400BF66 RID: 48998
				public static LocString TOOLTIP_ALREADYINSPECTED = "This structure has already been inspected";

				// Token: 0x0400BF67 RID: 48999
				public static LocString GOTODATABASE = "View Entry";

				// Token: 0x0400BF68 RID: 49000
				public static LocString SEARCH_DISPLAY = "The display is still functional. I copy its message into my database.\n\nNew Database Entry discovered.";

				// Token: 0x0400BF69 RID: 49001
				public static LocString SEARCH_ELLIESDESK = "All I find on the machine is a curt e-mail from a disgruntled employee.\n\nNew Database Entry discovered.";

				// Token: 0x0400BF6A RID: 49002
				public static LocString SEARCH_POD = "I search my incoming message history and find a single entry. I move the odd message into my database.\n\nNew Database Entry discovered.";

				// Token: 0x0400BF6B RID: 49003
				public static LocString ALREADY_SEARCHED = "I already took everything of interest from this. I can check the Database to re-read what I found.";

				// Token: 0x0400BF6C RID: 49004
				public static LocString SEARCH_CABINET = "One intact document remains - an old yellowing newspaper clipping. It won't be of much use, but I add it to my database nonetheless.\n\nNew Database Entry discovered.";

				// Token: 0x0400BF6D RID: 49005
				public static LocString SEARCH_STERNSDESK = "There's an old magazine article from a publication called the \"Nucleoid\" tucked in the top drawer. I add it to my database.\n\nNew Database Entry discovered.";

				// Token: 0x0400BF6E RID: 49006
				public static LocString ALREADY_SEARCHED_STERNSDESK = "The desk is eerily empty inside.";

				// Token: 0x0400BF6F RID: 49007
				public static LocString SEARCH_TELEPORTER_SENDER = "While scanning the antiquated computer code of this machine I uncovered some research notes. I add them to my database.\n\nNew Database Entry discovered.";

				// Token: 0x0400BF70 RID: 49008
				public static LocString SEARCH_TELEPORTER_RECEIVER = "Incongruously placed research notes are hidden within the operating instructions of this device. I add them to my database.\n\nNew Database Entry discovered.";

				// Token: 0x0400BF71 RID: 49009
				public static LocString SEARCH_CRYO_TANK = "There are some safety instructions included in the operating instructions of this Cryotank. I add them to my database.\n\nNew Database Entry discovered.";

				// Token: 0x0400BF72 RID: 49010
				public static LocString SEARCH_PROPGRAVITASCREATUREPOSTER = "There's a handwritten note taped to the back of this poster. I add it to my database.\n\nNew Database Entry discovered.";

				// Token: 0x02002E3A RID: 11834
				public class SEARCH_COMPUTER_PODIUM
				{
					// Token: 0x0400BF73 RID: 49011
					public static LocString SEARCH1 = "I search through the computer's database and find an unredacted e-mail.\n\nNew Database Entry unlocked.";
				}

				// Token: 0x02002E3B RID: 11835
				public class SEARCH_COMPUTER_SUCCESS
				{
					// Token: 0x0400BF74 RID: 49012
					public static LocString SEARCH1 = "After searching through the computer's database, I managed to piece together some files that piqued my interest.\n\nNew Database Entry unlocked.";

					// Token: 0x0400BF75 RID: 49013
					public static LocString SEARCH2 = "Searching through the computer, I find some recoverable files that are still readable.\n\nNew Database Entry unlocked.";

					// Token: 0x0400BF76 RID: 49014
					public static LocString SEARCH3 = "The computer looks pristine on the outside, but is corrupted internally. Still, I managed to find one uncorrupted file, and have added it to my database.\n\nNew Database Entry unlocked.";

					// Token: 0x0400BF77 RID: 49015
					public static LocString SEARCH4 = "The computer was wiped almost completely clean, except for one file hidden in the recycle bin.\n\nNew Database Entry unlocked.";

					// Token: 0x0400BF78 RID: 49016
					public static LocString SEARCH5 = "I search the computer, storing what useful data I can find in my own memory.\n\nNew Database Entry unlocked.";

					// Token: 0x0400BF79 RID: 49017
					public static LocString SEARCH6 = "This computer is broken and requires some finessing to get working. Still, I recover a handful of interesting files.\n\nNew Database Entry unlocked.";
				}

				// Token: 0x02002E3C RID: 11836
				public class SEARCH_COMPUTER_FAIL
				{
					// Token: 0x0400BF7A RID: 49018
					public static LocString SEARCH1 = "Unfortunately, the computer's hard drive is irreparably corrupted.";

					// Token: 0x0400BF7B RID: 49019
					public static LocString SEARCH2 = "The computer was wiped clean before I got here. There is nothing to recover.";

					// Token: 0x0400BF7C RID: 49020
					public static LocString SEARCH3 = "Some intact files are available on the computer, but nothing I haven't already discovered elsewhere. I find nothing else.";

					// Token: 0x0400BF7D RID: 49021
					public static LocString SEARCH4 = "The computer has nothing of import.";

					// Token: 0x0400BF7E RID: 49022
					public static LocString SEARCH5 = "Someone's left a solitaire game up. There's nothing else of interest on the computer.\n\nAlso, it looks as though they were about to lose.";

					// Token: 0x0400BF7F RID: 49023
					public static LocString SEARCH6 = "The background on this computer depicts two kittens hugging in a field of daisies. There is nothing else of import to be found.";

					// Token: 0x0400BF80 RID: 49024
					public static LocString SEARCH7 = "The user alphabetized the shortcuts on their desktop. There is nothing else of import to be found.";

					// Token: 0x0400BF81 RID: 49025
					public static LocString SEARCH8 = "The background is a picture of a golden retriever in a science lab. It looks very confused. There is nothing else of import to be found.";

					// Token: 0x0400BF82 RID: 49026
					public static LocString SEARCH9 = "This user never changed their default background. There is nothing else of import to be found. How dull.";
				}

				// Token: 0x02002E3D RID: 11837
				public class SEARCH_TECHNOLOGY_SUCCESS
				{
					// Token: 0x0400BF83 RID: 49027
					public static LocString SEARCH1 = "I scour the internal systems and find something of interest.\n\nNew Database Entry discovered.";

					// Token: 0x0400BF84 RID: 49028
					public static LocString SEARCH2 = "I see if I can salvage anything from the electronics. I add what I find to my database.\n\nNew Database Entry discovered.";

					// Token: 0x0400BF85 RID: 49029
					public static LocString SEARCH3 = "I look for anything of interest within the abandoned machinery and add what I find to my database.\n\nNew Database Entry discovered.";
				}

				// Token: 0x02002E3E RID: 11838
				public class SEARCH_OBJECT_SUCCESS
				{
					// Token: 0x0400BF86 RID: 49030
					public static LocString SEARCH1 = "I look around and recover an old file.\n\nNew Database Entry discovered.";

					// Token: 0x0400BF87 RID: 49031
					public static LocString SEARCH2 = "There's a three-ringed binder inside. I scan the surviving documents.\n\nNew Database Entry discovered.";

					// Token: 0x0400BF88 RID: 49032
					public static LocString SEARCH3 = "A discarded journal inside remains mostly intact. I scan the pages of use.\n\nNew Database Entry discovered.";

					// Token: 0x0400BF89 RID: 49033
					public static LocString SEARCH4 = "A single page of a long printout remains legible. I scan it and add it to my database.\n\nNew Database Entry discovered.";

					// Token: 0x0400BF8A RID: 49034
					public static LocString SEARCH5 = "A few loose papers can be found inside. I scan the ones that look interesting.\n\nNew Database Entry discovered.";

					// Token: 0x0400BF8B RID: 49035
					public static LocString SEARCH6 = "I find a memory stick inside and copy its data into my database.\n\nNew Database Entry discovered.";
				}

				// Token: 0x02002E3F RID: 11839
				public class SEARCH_OBJECT_FAIL
				{
					// Token: 0x0400BF8C RID: 49036
					public static LocString SEARCH1 = "I look around but find nothing of interest.";
				}

				// Token: 0x02002E40 RID: 11840
				public class SEARCH_SPACEPOI_SUCCESS
				{
					// Token: 0x0400BF8D RID: 49037
					public static LocString SEARCH1 = "A quick analysis of the hardware of this debris has uncovered some searchable files within.\n\nNew Database Entry unlocked.";

					// Token: 0x0400BF8E RID: 49038
					public static LocString SEARCH2 = "There's an archaic interface I can interact with on this device.\n\nNew Database Entry unlocked.";

					// Token: 0x0400BF8F RID: 49039
					public static LocString SEARCH3 = "While investigating the software of this wreckage, a compelling file comes to my attention.\n\nNew Database Entry unlocked.";

					// Token: 0x0400BF90 RID: 49040
					public static LocString SEARCH4 = "Not much remains of the software that once ran this spacecraft except for one file that piques my interest.\n\nNew Database Entry unlocked.";

					// Token: 0x0400BF91 RID: 49041
					public static LocString SEARCH5 = "I find some noteworthy data hidden amongst the system files of this space junk.\n\nNew Database Entry unlocked.";

					// Token: 0x0400BF92 RID: 49042
					public static LocString SEARCH6 = "Despite being subjected to years of degradation, there are still recoverable files in this machinery.\n\nNew Database Entry unlocked.";
				}

				// Token: 0x02002E41 RID: 11841
				public class SEARCH_SPACEPOI_FAIL
				{
					// Token: 0x0400BF93 RID: 49043
					public static LocString SEARCH1 = "There's nothing of interest left in this old space junk.";

					// Token: 0x0400BF94 RID: 49044
					public static LocString SEARCH2 = "I've salvaged everything I can from this vehicle.";

					// Token: 0x0400BF95 RID: 49045
					public static LocString SEARCH3 = "Years of neglect and radioactive decay have destroyed all the useful data from this derelict spacecraft.";
				}

				// Token: 0x02002E42 RID: 11842
				public class SEARCH_DISPLAY_FAIL
				{
					// Token: 0x0400BF96 RID: 49046
					public static LocString SEARCH1 = "The display is frozen. Whatever information it once contained is long gone.";
				}
			}

			// Token: 0x02002E43 RID: 11843
			public class OPENPOI
			{
				// Token: 0x0400BF97 RID: 49047
				public static LocString NAME = "Rummage";

				// Token: 0x0400BF98 RID: 49048
				public static LocString TOOLTIP = "Scrounge for usable materials";

				// Token: 0x0400BF99 RID: 49049
				public static LocString NAME_OFF = "Cancel Rummage";

				// Token: 0x0400BF9A RID: 49050
				public static LocString TOOLTIP_OFF = "Cancel this rummage order";

				// Token: 0x0400BF9B RID: 49051
				public static LocString ALREADY_RUMMAGED = "Already Rummaged";

				// Token: 0x0400BF9C RID: 49052
				public static LocString TOOLTIP_ALREADYRUMMAGED = "There are no usable materials left to find";
			}

			// Token: 0x02002E44 RID: 11844
			public class OPEN_TECHUNLOCKS
			{
				// Token: 0x0400BF9D RID: 49053
				public static LocString NAME = "Unlock Research";

				// Token: 0x0400BF9E RID: 49054
				public static LocString TOOLTIP = "Retrieve data stored in this building";

				// Token: 0x0400BF9F RID: 49055
				public static LocString NAME_OFF = "Cancel Unlock Research";

				// Token: 0x0400BFA0 RID: 49056
				public static LocString TOOLTIP_OFF = "Cancel this research access order";

				// Token: 0x0400BFA1 RID: 49057
				public static LocString ALREADY_RUMMAGED = "Already Unlocked";

				// Token: 0x0400BFA2 RID: 49058
				public static LocString TOOLTIP_ALREADYRUMMAGED = "All data has been accessed and recorded";
			}

			// Token: 0x02002E45 RID: 11845
			public class EMPTYSTORAGE
			{
				// Token: 0x0400BFA3 RID: 49059
				public static LocString NAME = "Empty Storage";

				// Token: 0x0400BFA4 RID: 49060
				public static LocString TOOLTIP = "Eject all resources from this container";

				// Token: 0x0400BFA5 RID: 49061
				public static LocString NAME_OFF = "Cancel Empty";

				// Token: 0x0400BFA6 RID: 49062
				public static LocString TOOLTIP_OFF = "Cancel this empty order";
			}

			// Token: 0x02002E46 RID: 11846
			public class CLOSESTORAGE
			{
				// Token: 0x0400BFA7 RID: 49063
				public static LocString NAME = "Close Storage";

				// Token: 0x0400BFA8 RID: 49064
				public static LocString TOOLTIP = "Prevent this container from receiving resources for storage";

				// Token: 0x0400BFA9 RID: 49065
				public static LocString NAME_OFF = "Cancel Close";

				// Token: 0x0400BFAA RID: 49066
				public static LocString TOOLTIP_OFF = "Cancel this close order";
			}

			// Token: 0x02002E47 RID: 11847
			public class COPY_BUILDING_SETTINGS
			{
				// Token: 0x0400BFAB RID: 49067
				public static LocString NAME = "Copy Settings";

				// Token: 0x0400BFAC RID: 49068
				public static LocString TOOLTIP = "Apply the settings and priorities of this building to other buildings of the same type {Hotkey}";
			}

			// Token: 0x02002E48 RID: 11848
			public class CLEAR
			{
				// Token: 0x0400BFAD RID: 49069
				public static LocString NAME = "Sweep";

				// Token: 0x0400BFAE RID: 49070
				public static LocString TOOLTIP = "Put this object away in the nearest storage container";

				// Token: 0x0400BFAF RID: 49071
				public static LocString NAME_OFF = "Cancel Sweeping";

				// Token: 0x0400BFB0 RID: 49072
				public static LocString TOOLTIP_OFF = "Cancel this sweep order";
			}

			// Token: 0x02002E49 RID: 11849
			public class COMPOST
			{
				// Token: 0x0400BFB1 RID: 49073
				public static LocString NAME = "Compost";

				// Token: 0x0400BFB2 RID: 49074
				public static LocString TOOLTIP = "Mark this object for compost";

				// Token: 0x0400BFB3 RID: 49075
				public static LocString NAME_OFF = "Cancel Compost";

				// Token: 0x0400BFB4 RID: 49076
				public static LocString TOOLTIP_OFF = "Cancel this compost order";
			}

			// Token: 0x02002E4A RID: 11850
			public class PICKUPABLEMOVE
			{
				// Token: 0x0400BFB5 RID: 49077
				public static LocString NAME = "Relocate To";

				// Token: 0x0400BFB6 RID: 49078
				public static LocString TOOLTIP = "Relocate this object to a specific location";

				// Token: 0x0400BFB7 RID: 49079
				public static LocString NAME_OFF = "Cancel Relocate";

				// Token: 0x0400BFB8 RID: 49080
				public static LocString TOOLTIP_OFF = "Cancel order to relocate this object";
			}

			// Token: 0x02002E4B RID: 11851
			public class UNEQUIP
			{
				// Token: 0x0400BFB9 RID: 49081
				public static LocString NAME = "Unequip {0}";

				// Token: 0x0400BFBA RID: 49082
				public static LocString TOOLTIP = "Take off and drop this equipment";
			}

			// Token: 0x02002E4C RID: 11852
			public class QUARANTINE
			{
				// Token: 0x0400BFBB RID: 49083
				public static LocString NAME = "Quarantine";

				// Token: 0x0400BFBC RID: 49084
				public static LocString TOOLTIP = "Isolate this Duplicant\nThe Duplicant will return to their assigned Cot";

				// Token: 0x0400BFBD RID: 49085
				public static LocString TOOLTIP_DISABLED = "No quarantine zone assigned";

				// Token: 0x0400BFBE RID: 49086
				public static LocString NAME_OFF = "Cancel Quarantine";

				// Token: 0x0400BFBF RID: 49087
				public static LocString TOOLTIP_OFF = "Cancel this quarantine order";
			}

			// Token: 0x02002E4D RID: 11853
			public class DRAWPATHS
			{
				// Token: 0x0400BFC0 RID: 49088
				public static LocString NAME = "Show Navigation";

				// Token: 0x0400BFC1 RID: 49089
				public static LocString TOOLTIP = "Show all areas within this Duplicant's reach";

				// Token: 0x0400BFC2 RID: 49090
				public static LocString NAME_OFF = "Hide Navigation";

				// Token: 0x0400BFC3 RID: 49091
				public static LocString TOOLTIP_OFF = "Hide areas within this Duplicant's reach";
			}

			// Token: 0x02002E4E RID: 11854
			public class MOVETOLOCATION
			{
				// Token: 0x0400BFC4 RID: 49092
				public static LocString NAME = "Move To";

				// Token: 0x0400BFC5 RID: 49093
				public static LocString TOOLTIP = "Move this Duplicant to a specific location";
			}

			// Token: 0x02002E4F RID: 11855
			public class FOLLOWCAM
			{
				// Token: 0x0400BFC6 RID: 49094
				public static LocString NAME = "Follow Cam";

				// Token: 0x0400BFC7 RID: 49095
				public static LocString TOOLTIP = "Track this Duplicant with the camera";
			}

			// Token: 0x02002E50 RID: 11856
			public class WORKABLE_DIRECTION_BOTH
			{
				// Token: 0x0400BFC8 RID: 49096
				public static LocString NAME = "Set Direction: Both";

				// Token: 0x0400BFC9 RID: 49097
				public static LocString TOOLTIP = "Select to make Duplicants wash when passing by in either direction";
			}

			// Token: 0x02002E51 RID: 11857
			public class WORKABLE_DIRECTION_LEFT
			{
				// Token: 0x0400BFCA RID: 49098
				public static LocString NAME = "Set Direction: Left";

				// Token: 0x0400BFCB RID: 49099
				public static LocString TOOLTIP = "Select to make Duplicants wash when passing by from right to left";
			}

			// Token: 0x02002E52 RID: 11858
			public class WORKABLE_DIRECTION_RIGHT
			{
				// Token: 0x0400BFCC RID: 49100
				public static LocString NAME = "Set Direction: Right";

				// Token: 0x0400BFCD RID: 49101
				public static LocString TOOLTIP = "Select to make Duplicants wash when passing by from left to right";
			}

			// Token: 0x02002E53 RID: 11859
			public class MANUAL_PUMP_DELIVERY
			{
				// Token: 0x02002E54 RID: 11860
				public static class ALLOWED
				{
					// Token: 0x0400BFCE RID: 49102
					public static LocString NAME = "Enable Auto-Bottle";

					// Token: 0x0400BFCF RID: 49103
					public static LocString TOOLTIP = "If enabled, Duplicants will deliver bottled liquids to this building directly from these sources:\n";

					// Token: 0x0400BFD0 RID: 49104
					public static LocString ITEM = "\n{0}";
				}

				// Token: 0x02002E55 RID: 11861
				public static class DENIED
				{
					// Token: 0x0400BFD1 RID: 49105
					public static LocString NAME = "Disable Auto-Bottle";

					// Token: 0x0400BFD2 RID: 49106
					public static LocString TOOLTIP = "If disabled, Duplicants will no longer deliver bottled liquids directly from Pitcher Pumps";
				}

				// Token: 0x02002E56 RID: 11862
				public static class ALLOWED_GAS
				{
					// Token: 0x0400BFD3 RID: 49107
					public static LocString NAME = "Enable Auto-Bottle";

					// Token: 0x0400BFD4 RID: 49108
					public static LocString TOOLTIP = "If enabled, Duplicants will deliver gas canisters to this building directly from Canister Fillers";
				}

				// Token: 0x02002E57 RID: 11863
				public static class DENIED_GAS
				{
					// Token: 0x0400BFD5 RID: 49109
					public static LocString NAME = "Disable Auto-Bottle";

					// Token: 0x0400BFD6 RID: 49110
					public static LocString TOOLTIP = "If disabled, Duplicants will no longer deliver gas canisters directly from Canister Fillers";
				}
			}

			// Token: 0x02002E58 RID: 11864
			public class SUIT_MARKER_TRAVERSAL
			{
				// Token: 0x02002E59 RID: 11865
				public static class ONLY_WHEN_ROOM_AVAILABLE
				{
					// Token: 0x0400BFD7 RID: 49111
					public static LocString NAME = "Clearance: Vacancy";

					// Token: 0x0400BFD8 RID: 49112
					public static LocString TOOLTIP = "Suited Duplicants may only pass if there is an available dock to store their suit";
				}

				// Token: 0x02002E5A RID: 11866
				public static class ALWAYS
				{
					// Token: 0x0400BFD9 RID: 49113
					public static LocString NAME = "Clearance: Always";

					// Token: 0x0400BFDA RID: 49114
					public static LocString TOOLTIP = "Suited Duplicants may pass even if there is no room to store their suits\n\nWhen all available docks are full, Duplicants will unequip their suits and drop them on the floor";
				}
			}

			// Token: 0x02002E5B RID: 11867
			public class ACTIVATEBUILDING
			{
				// Token: 0x0400BFDB RID: 49115
				public static LocString ACTIVATE = "Activate";

				// Token: 0x0400BFDC RID: 49116
				public static LocString TOOLTIP_ACTIVATE = "Request a Duplicant to activate this building";

				// Token: 0x0400BFDD RID: 49117
				public static LocString TOOLTIP_ACTIVATED = "This building has already been activated";

				// Token: 0x0400BFDE RID: 49118
				public static LocString ACTIVATE_CANCEL = "Cancel Activation";

				// Token: 0x0400BFDF RID: 49119
				public static LocString ACTIVATED = "Activated";

				// Token: 0x0400BFE0 RID: 49120
				public static LocString TOOLTIP_CANCEL = "Cancel activation of this building";
			}

			// Token: 0x02002E5C RID: 11868
			public class ACCEPT_MUTANT_SEEDS
			{
				// Token: 0x0400BFE1 RID: 49121
				public static LocString ACCEPT = "Allow Mutants";

				// Token: 0x0400BFE2 RID: 49122
				public static LocString REJECT = "Forbid Mutants";

				// Token: 0x0400BFE3 RID: 49123
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Toggle whether or not this building will accept ",
					UI.PRE_KEYWORD,
					"Mutant Seeds",
					UI.PST_KEYWORD,
					" for recipes that could use them"
				});

				// Token: 0x0400BFE4 RID: 49124
				public static LocString FISH_FEEDER_TOOLTIP = string.Concat(new string[]
				{
					"Toggle whether or not this feeder will accept ",
					UI.PRE_KEYWORD,
					"Mutant Seeds",
					UI.PST_KEYWORD,
					" for critters who eat them"
				});
			}

			// Token: 0x02002E5D RID: 11869
			public class CARVE
			{
				// Token: 0x0400BFE5 RID: 49125
				public static LocString NAME = "Carve";

				// Token: 0x0400BFE6 RID: 49126
				public static LocString TOOLTIP = "Carve this rock to enhance its positive effects";
			}

			// Token: 0x02002E5E RID: 11870
			public class CANCELCARVE
			{
				// Token: 0x0400BFE7 RID: 49127
				public static LocString NAME = "Cancel Carve";

				// Token: 0x0400BFE8 RID: 49128
				public static LocString TOOLTIP = "Cancel order to carve this rock";
			}
		}

		// Token: 0x02002E5F RID: 11871
		public class BUILDCATEGORIES
		{
			// Token: 0x02002E60 RID: 11872
			public static class BASE
			{
				// Token: 0x0400BFE9 RID: 49129
				public static LocString NAME = UI.FormatAsLink("Base", "BUILDCATEGORYBASE");

				// Token: 0x0400BFEA RID: 49130
				public static LocString TOOLTIP = "Maintain the colony's infrastructure with these homebase basics. {Hotkey}";
			}

			// Token: 0x02002E61 RID: 11873
			public static class CONVEYANCE
			{
				// Token: 0x0400BFEB RID: 49131
				public static LocString NAME = UI.FormatAsLink("Shipping", "BUILDCATEGORYCONVEYANCE");

				// Token: 0x0400BFEC RID: 49132
				public static LocString TOOLTIP = "Transport ore and solid materials around my base. {Hotkey}";
			}

			// Token: 0x02002E62 RID: 11874
			public static class OXYGEN
			{
				// Token: 0x0400BFED RID: 49133
				public static LocString NAME = UI.FormatAsLink("Oxygen", "BUILDCATEGORYOXYGEN");

				// Token: 0x0400BFEE RID: 49134
				public static LocString TOOLTIP = "Everything I need to keep the colony breathing. {Hotkey}";
			}

			// Token: 0x02002E63 RID: 11875
			public static class POWER
			{
				// Token: 0x0400BFEF RID: 49135
				public static LocString NAME = UI.FormatAsLink("Power", "BUILDCATEGORYPOWER");

				// Token: 0x0400BFF0 RID: 49136
				public static LocString TOOLTIP = "Need to power the colony? Here's how to do it! {Hotkey}";
			}

			// Token: 0x02002E64 RID: 11876
			public static class FOOD
			{
				// Token: 0x0400BFF1 RID: 49137
				public static LocString NAME = UI.FormatAsLink("Food", "BUILDCATEGORYFOOD");

				// Token: 0x0400BFF2 RID: 49138
				public static LocString TOOLTIP = "Keep my Duplicants' spirits high and their bellies full. {Hotkey}";
			}

			// Token: 0x02002E65 RID: 11877
			public static class UTILITIES
			{
				// Token: 0x0400BFF3 RID: 49139
				public static LocString NAME = UI.FormatAsLink("Utilities", "BUILDCATEGORYUTILITIES");

				// Token: 0x0400BFF4 RID: 49140
				public static LocString TOOLTIP = "Heat up and cool down. {Hotkey}";
			}

			// Token: 0x02002E66 RID: 11878
			public static class PLUMBING
			{
				// Token: 0x0400BFF5 RID: 49141
				public static LocString NAME = UI.FormatAsLink("Plumbing", "BUILDCATEGORYPLUMBING");

				// Token: 0x0400BFF6 RID: 49142
				public static LocString TOOLTIP = "Get the colony's water running and its sewage flowing. {Hotkey}";
			}

			// Token: 0x02002E67 RID: 11879
			public static class HVAC
			{
				// Token: 0x0400BFF7 RID: 49143
				public static LocString NAME = UI.FormatAsLink("Ventilation", "BUILDCATEGORYHVAC");

				// Token: 0x0400BFF8 RID: 49144
				public static LocString TOOLTIP = "Control the flow of gas in the base. {Hotkey}";
			}

			// Token: 0x02002E68 RID: 11880
			public static class REFINING
			{
				// Token: 0x0400BFF9 RID: 49145
				public static LocString NAME = UI.FormatAsLink("Refinement", "BUILDCATEGORYREFINING");

				// Token: 0x0400BFFA RID: 49146
				public static LocString TOOLTIP = "Use the resources I want, filter the ones I don't. {Hotkey}";
			}

			// Token: 0x02002E69 RID: 11881
			public static class ROCKETRY
			{
				// Token: 0x0400BFFB RID: 49147
				public static LocString NAME = UI.FormatAsLink("Rocketry", "BUILDCATEGORYROCKETRY");

				// Token: 0x0400BFFC RID: 49148
				public static LocString TOOLTIP = "With rockets, the sky's no longer the limit! {Hotkey}";
			}

			// Token: 0x02002E6A RID: 11882
			public static class MEDICAL
			{
				// Token: 0x0400BFFD RID: 49149
				public static LocString NAME = UI.FormatAsLink("Medicine", "BUILDCATEGORYMEDICAL");

				// Token: 0x0400BFFE RID: 49150
				public static LocString TOOLTIP = "A cure for everything but the common cold. {Hotkey}";
			}

			// Token: 0x02002E6B RID: 11883
			public static class FURNITURE
			{
				// Token: 0x0400BFFF RID: 49151
				public static LocString NAME = UI.FormatAsLink("Furniture", "BUILDCATEGORYFURNITURE");

				// Token: 0x0400C000 RID: 49152
				public static LocString TOOLTIP = "Amenities to keep my Duplicants happy, comfy and efficient. {Hotkey}";
			}

			// Token: 0x02002E6C RID: 11884
			public static class EQUIPMENT
			{
				// Token: 0x0400C001 RID: 49153
				public static LocString NAME = UI.FormatAsLink("Stations", "BUILDCATEGORYEQUIPMENT");

				// Token: 0x0400C002 RID: 49154
				public static LocString TOOLTIP = "Unlock new technologies through the power of science! {Hotkey}";
			}

			// Token: 0x02002E6D RID: 11885
			public static class MISC
			{
				// Token: 0x0400C003 RID: 49155
				public static LocString NAME = UI.FormatAsLink("Decor", "BUILDCATEGORYMISC");

				// Token: 0x0400C004 RID: 49156
				public static LocString TOOLTIP = "Spruce up my colony with some lovely interior decorating. {Hotkey}";
			}

			// Token: 0x02002E6E RID: 11886
			public static class AUTOMATION
			{
				// Token: 0x0400C005 RID: 49157
				public static LocString NAME = UI.FormatAsLink("Automation", "BUILDCATEGORYAUTOMATION");

				// Token: 0x0400C006 RID: 49158
				public static LocString TOOLTIP = "Automate my base with a wide range of sensors. {Hotkey}";
			}

			// Token: 0x02002E6F RID: 11887
			public static class HEP
			{
				// Token: 0x0400C007 RID: 49159
				public static LocString NAME = UI.FormatAsLink("Radiation", "BUILDCATEGORYHEP");

				// Token: 0x0400C008 RID: 49160
				public static LocString TOOLTIP = "Here's where things get rad. {Hotkey}";
			}
		}

		// Token: 0x02002E70 RID: 11888
		public class NEWBUILDCATEGORIES
		{
			// Token: 0x02002E71 RID: 11889
			public static class BASE
			{
				// Token: 0x0400C009 RID: 49161
				public static LocString NAME = UI.FormatAsLink("Base", "BUILD_CATEGORY_BASE");

				// Token: 0x0400C00A RID: 49162
				public static LocString TOOLTIP = "Maintain the colony's infrastructure with these homebase basics. {Hotkey}";
			}

			// Token: 0x02002E72 RID: 11890
			public static class INFRASTRUCTURE
			{
				// Token: 0x0400C00B RID: 49163
				public static LocString NAME = UI.FormatAsLink("Utilities", "BUILD_CATEGORY_INFRASTRUCTURE");

				// Token: 0x0400C00C RID: 49164
				public static LocString TOOLTIP = "Power, plumbing, and ventilation can all be found here. {Hotkey}";
			}

			// Token: 0x02002E73 RID: 11891
			public static class FOODANDAGRICULTURE
			{
				// Token: 0x0400C00D RID: 49165
				public static LocString NAME = UI.FormatAsLink("Food", "BUILD_CATEGORY_FOODANDAGRICULTURE");

				// Token: 0x0400C00E RID: 49166
				public static LocString TOOLTIP = "Keep my Duplicants' spirits high and their bellies full. {Hotkey}";
			}

			// Token: 0x02002E74 RID: 11892
			public static class LOGISTICS
			{
				// Token: 0x0400C00F RID: 49167
				public static LocString NAME = UI.FormatAsLink("Logistics", "BUILD_CATEGORY_LOGISTICS");

				// Token: 0x0400C010 RID: 49168
				public static LocString TOOLTIP = "Devices for base automation and material transport. {Hotkey}";
			}

			// Token: 0x02002E75 RID: 11893
			public static class HEALTHANDHAPPINESS
			{
				// Token: 0x0400C011 RID: 49169
				public static LocString NAME = UI.FormatAsLink("Accommodation", "BUILD_CATEGORY_HEALTHANDHAPPINESS");

				// Token: 0x0400C012 RID: 49170
				public static LocString TOOLTIP = "Everything a Duplicant needs to stay happy, healthy, and fulfilled. {Hotkey}";
			}

			// Token: 0x02002E76 RID: 11894
			public static class INDUSTRIAL
			{
				// Token: 0x0400C013 RID: 49171
				public static LocString NAME = UI.FormatAsLink("Industrials", "BUILD_CATEGORY_INDUSTRIAL");

				// Token: 0x0400C014 RID: 49172
				public static LocString TOOLTIP = "Machinery for oxygen production, heat management, and material refinement. {Hotkey}";
			}

			// Token: 0x02002E77 RID: 11895
			public static class LADDERS
			{
				// Token: 0x0400C015 RID: 49173
				public static LocString NAME = "Ladders";

				// Token: 0x0400C016 RID: 49174
				public static LocString BUILDMENUTITLE = "Ladders";

				// Token: 0x0400C017 RID: 49175
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002E78 RID: 11896
			public static class TILES
			{
				// Token: 0x0400C018 RID: 49176
				public static LocString NAME = "Tiles and Drywall";

				// Token: 0x0400C019 RID: 49177
				public static LocString BUILDMENUTITLE = "Tiles and Drywall";

				// Token: 0x0400C01A RID: 49178
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002E79 RID: 11897
			public static class PRINTINGPODS
			{
				// Token: 0x0400C01B RID: 49179
				public static LocString NAME = "Printing Pods";

				// Token: 0x0400C01C RID: 49180
				public static LocString BUILDMENUTITLE = "Printing Pods";

				// Token: 0x0400C01D RID: 49181
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002E7A RID: 11898
			public static class DOORS
			{
				// Token: 0x0400C01E RID: 49182
				public static LocString NAME = "Doors";

				// Token: 0x0400C01F RID: 49183
				public static LocString BUILDMENUTITLE = "Doors";

				// Token: 0x0400C020 RID: 49184
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002E7B RID: 11899
			public static class STORAGE
			{
				// Token: 0x0400C021 RID: 49185
				public static LocString NAME = "Storage";

				// Token: 0x0400C022 RID: 49186
				public static LocString BUILDMENUTITLE = "Storage";

				// Token: 0x0400C023 RID: 49187
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002E7C RID: 11900
			public static class TRANSPORT
			{
				// Token: 0x0400C024 RID: 49188
				public static LocString NAME = "Transit Tubes";

				// Token: 0x0400C025 RID: 49189
				public static LocString BUILDMENUTITLE = "Transit Tubes";

				// Token: 0x0400C026 RID: 49190
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002E7D RID: 11901
			public static class OPERATIONS
			{
				// Token: 0x0400C027 RID: 49191
				public static LocString NAME = "Operations";

				// Token: 0x0400C028 RID: 49192
				public static LocString BUILDMENUTITLE = "Operations";

				// Token: 0x0400C029 RID: 49193
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002E7E RID: 11902
			public static class PRODUCERS
			{
				// Token: 0x0400C02A RID: 49194
				public static LocString NAME = "Production";

				// Token: 0x0400C02B RID: 49195
				public static LocString BUILDMENUTITLE = "Production";

				// Token: 0x0400C02C RID: 49196
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002E7F RID: 11903
			public static class SCRUBBERS
			{
				// Token: 0x0400C02D RID: 49197
				public static LocString NAME = "Purification";

				// Token: 0x0400C02E RID: 49198
				public static LocString BUILDMENUTITLE = "Purification";

				// Token: 0x0400C02F RID: 49199
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002E80 RID: 11904
			public static class BATTERIES
			{
				// Token: 0x0400C030 RID: 49200
				public static LocString NAME = "Batteries";

				// Token: 0x0400C031 RID: 49201
				public static LocString BUILDMENUTITLE = "Batteries";

				// Token: 0x0400C032 RID: 49202
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002E81 RID: 11905
			public static class SWITCHES
			{
				// Token: 0x0400C033 RID: 49203
				public static LocString NAME = "Switches";

				// Token: 0x0400C034 RID: 49204
				public static LocString BUILDMENUTITLE = "Switches";

				// Token: 0x0400C035 RID: 49205
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002E82 RID: 11906
			public static class COOKING
			{
				// Token: 0x0400C036 RID: 49206
				public static LocString NAME = "Cooking";

				// Token: 0x0400C037 RID: 49207
				public static LocString BUILDMENUTITLE = "Cooking";

				// Token: 0x0400C038 RID: 49208
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002E83 RID: 11907
			public static class FARMING
			{
				// Token: 0x0400C039 RID: 49209
				public static LocString NAME = "Farming";

				// Token: 0x0400C03A RID: 49210
				public static LocString BUILDMENUTITLE = "Farming";

				// Token: 0x0400C03B RID: 49211
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002E84 RID: 11908
			public static class RANCHING
			{
				// Token: 0x0400C03C RID: 49212
				public static LocString NAME = "Ranching";

				// Token: 0x0400C03D RID: 49213
				public static LocString BUILDMENUTITLE = "Ranching";

				// Token: 0x0400C03E RID: 49214
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002E85 RID: 11909
			public static class WASHROOM
			{
				// Token: 0x0400C03F RID: 49215
				public static LocString NAME = "Washroom";

				// Token: 0x0400C040 RID: 49216
				public static LocString BUILDMENUTITLE = "Washroom";

				// Token: 0x0400C041 RID: 49217
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002E86 RID: 11910
			public static class VALVES
			{
				// Token: 0x0400C042 RID: 49218
				public static LocString NAME = "Valves";

				// Token: 0x0400C043 RID: 49219
				public static LocString BUILDMENUTITLE = "Valves";

				// Token: 0x0400C044 RID: 49220
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002E87 RID: 11911
			public static class PUMPS
			{
				// Token: 0x0400C045 RID: 49221
				public static LocString NAME = "Pumps";

				// Token: 0x0400C046 RID: 49222
				public static LocString BUILDMENUTITLE = "Pumps";

				// Token: 0x0400C047 RID: 49223
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002E88 RID: 11912
			public static class SENSORS
			{
				// Token: 0x0400C048 RID: 49224
				public static LocString NAME = "Sensors";

				// Token: 0x0400C049 RID: 49225
				public static LocString BUILDMENUTITLE = "Sensors";

				// Token: 0x0400C04A RID: 49226
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002E89 RID: 11913
			public static class PORTS
			{
				// Token: 0x0400C04B RID: 49227
				public static LocString NAME = "Ports";

				// Token: 0x0400C04C RID: 49228
				public static LocString BUILDMENUTITLE = "Ports";

				// Token: 0x0400C04D RID: 49229
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002E8A RID: 11914
			public static class MATERIALS
			{
				// Token: 0x0400C04E RID: 49230
				public static LocString NAME = "Materials";

				// Token: 0x0400C04F RID: 49231
				public static LocString BUILDMENUTITLE = "Materials";

				// Token: 0x0400C050 RID: 49232
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002E8B RID: 11915
			public static class OIL
			{
				// Token: 0x0400C051 RID: 49233
				public static LocString NAME = "Oil";

				// Token: 0x0400C052 RID: 49234
				public static LocString BUILDMENUTITLE = "Oil";

				// Token: 0x0400C053 RID: 49235
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002E8C RID: 11916
			public static class ADVANCED
			{
				// Token: 0x0400C054 RID: 49236
				public static LocString NAME = "Advanced";

				// Token: 0x0400C055 RID: 49237
				public static LocString BUILDMENUTITLE = "Advanced";

				// Token: 0x0400C056 RID: 49238
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002E8D RID: 11917
			public static class ORGANIC
			{
				// Token: 0x0400C057 RID: 49239
				public static LocString NAME = "Organic";

				// Token: 0x0400C058 RID: 49240
				public static LocString BUILDMENUTITLE = "Organic";

				// Token: 0x0400C059 RID: 49241
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002E8E RID: 11918
			public static class BEDS
			{
				// Token: 0x0400C05A RID: 49242
				public static LocString NAME = "Beds";

				// Token: 0x0400C05B RID: 49243
				public static LocString BUILDMENUTITLE = "Beds";

				// Token: 0x0400C05C RID: 49244
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002E8F RID: 11919
			public static class LIGHTS
			{
				// Token: 0x0400C05D RID: 49245
				public static LocString NAME = "Lights";

				// Token: 0x0400C05E RID: 49246
				public static LocString BUILDMENUTITLE = "Lights";

				// Token: 0x0400C05F RID: 49247
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002E90 RID: 11920
			public static class DINING
			{
				// Token: 0x0400C060 RID: 49248
				public static LocString NAME = "Dining";

				// Token: 0x0400C061 RID: 49249
				public static LocString BUILDMENUTITLE = "Dining";

				// Token: 0x0400C062 RID: 49250
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002E91 RID: 11921
			public static class MANUFACTURING
			{
				// Token: 0x0400C063 RID: 49251
				public static LocString NAME = "Manufacturing";

				// Token: 0x0400C064 RID: 49252
				public static LocString BUILDMENUTITLE = "Manufacturing";

				// Token: 0x0400C065 RID: 49253
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002E92 RID: 11922
			public static class TEMPERATURE
			{
				// Token: 0x0400C066 RID: 49254
				public static LocString NAME = "Temperature";

				// Token: 0x0400C067 RID: 49255
				public static LocString BUILDMENUTITLE = "Temperature";

				// Token: 0x0400C068 RID: 49256
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002E93 RID: 11923
			public static class RESEARCH
			{
				// Token: 0x0400C069 RID: 49257
				public static LocString NAME = "Research";

				// Token: 0x0400C06A RID: 49258
				public static LocString BUILDMENUTITLE = "Research";

				// Token: 0x0400C06B RID: 49259
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002E94 RID: 11924
			public static class GENERATORS
			{
				// Token: 0x0400C06C RID: 49260
				public static LocString NAME = "Generators";

				// Token: 0x0400C06D RID: 49261
				public static LocString BUILDMENUTITLE = "Generators";

				// Token: 0x0400C06E RID: 49262
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002E95 RID: 11925
			public static class WIRES
			{
				// Token: 0x0400C06F RID: 49263
				public static LocString NAME = "Wires";

				// Token: 0x0400C070 RID: 49264
				public static LocString BUILDMENUTITLE = "Wires";

				// Token: 0x0400C071 RID: 49265
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002E96 RID: 11926
			public static class ELECTROBANKBUILDINGS
			{
				// Token: 0x0400C072 RID: 49266
				public static LocString NAME = "Converters";

				// Token: 0x0400C073 RID: 49267
				public static LocString BUILDMENUTITLE = "Converters";

				// Token: 0x0400C074 RID: 49268
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002E97 RID: 11927
			public static class LOGICGATES
			{
				// Token: 0x0400C075 RID: 49269
				public static LocString NAME = "Gates";

				// Token: 0x0400C076 RID: 49270
				public static LocString BUILDMENUTITLE = "Gates";

				// Token: 0x0400C077 RID: 49271
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002E98 RID: 11928
			public static class TRANSMISSIONS
			{
				// Token: 0x0400C078 RID: 49272
				public static LocString NAME = "Transmissions";

				// Token: 0x0400C079 RID: 49273
				public static LocString BUILDMENUTITLE = "Transmissions";

				// Token: 0x0400C07A RID: 49274
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002E99 RID: 11929
			public static class LOGICMANAGER
			{
				// Token: 0x0400C07B RID: 49275
				public static LocString NAME = "Monitoring";

				// Token: 0x0400C07C RID: 49276
				public static LocString BUILDMENUTITLE = "Monitoring";

				// Token: 0x0400C07D RID: 49277
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002E9A RID: 11930
			public static class LOGICAUDIO
			{
				// Token: 0x0400C07E RID: 49278
				public static LocString NAME = "Ambience";

				// Token: 0x0400C07F RID: 49279
				public static LocString BUILDMENUTITLE = "Ambience";

				// Token: 0x0400C080 RID: 49280
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002E9B RID: 11931
			public static class CONVEYANCESTRUCTURES
			{
				// Token: 0x0400C081 RID: 49281
				public static LocString NAME = "Structural";

				// Token: 0x0400C082 RID: 49282
				public static LocString BUILDMENUTITLE = "Structural";

				// Token: 0x0400C083 RID: 49283
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002E9C RID: 11932
			public static class BUILDMENUPORTS
			{
				// Token: 0x0400C084 RID: 49284
				public static LocString NAME = "Ports";

				// Token: 0x0400C085 RID: 49285
				public static LocString BUILDMENUTITLE = "Ports";

				// Token: 0x0400C086 RID: 49286
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002E9D RID: 11933
			public static class POWERCONTROL
			{
				// Token: 0x0400C087 RID: 49287
				public static LocString NAME = "Power\nRegulation";

				// Token: 0x0400C088 RID: 49288
				public static LocString BUILDMENUTITLE = "Power Regulation";

				// Token: 0x0400C089 RID: 49289
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002E9E RID: 11934
			public static class PLUMBINGSTRUCTURES
			{
				// Token: 0x0400C08A RID: 49290
				public static LocString NAME = "Plumbing";

				// Token: 0x0400C08B RID: 49291
				public static LocString BUILDMENUTITLE = "Plumbing";

				// Token: 0x0400C08C RID: 49292
				public static LocString TOOLTIP = "Get the colony's water running and its sewage flowing. {Hotkey}";
			}

			// Token: 0x02002E9F RID: 11935
			public static class PIPES
			{
				// Token: 0x0400C08D RID: 49293
				public static LocString NAME = "Pipes";

				// Token: 0x0400C08E RID: 49294
				public static LocString BUILDMENUTITLE = "Pipes";

				// Token: 0x0400C08F RID: 49295
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002EA0 RID: 11936
			public static class VENTILATIONSTRUCTURES
			{
				// Token: 0x0400C090 RID: 49296
				public static LocString NAME = "Ventilation";

				// Token: 0x0400C091 RID: 49297
				public static LocString BUILDMENUTITLE = "Ventilation";

				// Token: 0x0400C092 RID: 49298
				public static LocString TOOLTIP = "Control the flow of gas in your base. {Hotkey}";
			}

			// Token: 0x02002EA1 RID: 11937
			public static class CONVEYANCE
			{
				// Token: 0x0400C093 RID: 49299
				public static LocString NAME = "Ore\nTransport";

				// Token: 0x0400C094 RID: 49300
				public static LocString BUILDMENUTITLE = "Ore Transport";

				// Token: 0x0400C095 RID: 49301
				public static LocString TOOLTIP = "Transport ore and solid materials around my base. {Hotkey}";
			}

			// Token: 0x02002EA2 RID: 11938
			public static class HYGIENE
			{
				// Token: 0x0400C096 RID: 49302
				public static LocString NAME = "Hygiene";

				// Token: 0x0400C097 RID: 49303
				public static LocString BUILDMENUTITLE = "Hygiene";

				// Token: 0x0400C098 RID: 49304
				public static LocString TOOLTIP = "Keeps my Duplicants clean.";
			}

			// Token: 0x02002EA3 RID: 11939
			public static class MEDICAL
			{
				// Token: 0x0400C099 RID: 49305
				public static LocString NAME = "Medical";

				// Token: 0x0400C09A RID: 49306
				public static LocString BUILDMENUTITLE = "Medical";

				// Token: 0x0400C09B RID: 49307
				public static LocString TOOLTIP = "A cure for everything but the common cold. {Hotkey}";
			}

			// Token: 0x02002EA4 RID: 11940
			public static class WELLNESS
			{
				// Token: 0x0400C09C RID: 49308
				public static LocString NAME = "Wellness";

				// Token: 0x0400C09D RID: 49309
				public static LocString BUILDMENUTITLE = "Wellness";

				// Token: 0x0400C09E RID: 49310
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002EA5 RID: 11941
			public static class RECREATION
			{
				// Token: 0x0400C09F RID: 49311
				public static LocString NAME = "Recreation";

				// Token: 0x0400C0A0 RID: 49312
				public static LocString BUILDMENUTITLE = "Recreation";

				// Token: 0x0400C0A1 RID: 49313
				public static LocString TOOLTIP = "Everything needed to reduce stress and increase fun.";
			}

			// Token: 0x02002EA6 RID: 11942
			public static class FURNITURE
			{
				// Token: 0x0400C0A2 RID: 49314
				public static LocString NAME = "Furniture";

				// Token: 0x0400C0A3 RID: 49315
				public static LocString BUILDMENUTITLE = "Furniture";

				// Token: 0x0400C0A4 RID: 49316
				public static LocString TOOLTIP = "Amenities to keep my Duplicants happy, comfy and efficient. {Hotkey}";
			}

			// Token: 0x02002EA7 RID: 11943
			public static class DECOR
			{
				// Token: 0x0400C0A5 RID: 49317
				public static LocString NAME = "Decor";

				// Token: 0x0400C0A6 RID: 49318
				public static LocString BUILDMENUTITLE = "Decor";

				// Token: 0x0400C0A7 RID: 49319
				public static LocString TOOLTIP = "Spruce up your colony with some lovely interior decorating. {Hotkey}";
			}

			// Token: 0x02002EA8 RID: 11944
			public static class OXYGEN
			{
				// Token: 0x0400C0A8 RID: 49320
				public static LocString NAME = "Oxygen";

				// Token: 0x0400C0A9 RID: 49321
				public static LocString BUILDMENUTITLE = "Oxygen";

				// Token: 0x0400C0AA RID: 49322
				public static LocString TOOLTIP = "Everything I need to keep my colony breathing. {Hotkey}";
			}

			// Token: 0x02002EA9 RID: 11945
			public static class UTILITIES
			{
				// Token: 0x0400C0AB RID: 49323
				public static LocString NAME = "Temperature";

				// Token: 0x0400C0AC RID: 49324
				public static LocString BUILDMENUTITLE = "Temperature";

				// Token: 0x0400C0AD RID: 49325
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002EAA RID: 11946
			public static class REFINING
			{
				// Token: 0x0400C0AE RID: 49326
				public static LocString NAME = "Refinement";

				// Token: 0x0400C0AF RID: 49327
				public static LocString BUILDMENUTITLE = "Refinement";

				// Token: 0x0400C0B0 RID: 49328
				public static LocString TOOLTIP = "Use the resources you want, filter the ones you don't. {Hotkey}";
			}

			// Token: 0x02002EAB RID: 11947
			public static class EQUIPMENT
			{
				// Token: 0x0400C0B1 RID: 49329
				public static LocString NAME = "Equipment";

				// Token: 0x0400C0B2 RID: 49330
				public static LocString BUILDMENUTITLE = "Equipment";

				// Token: 0x0400C0B3 RID: 49331
				public static LocString TOOLTIP = "Unlock new technologies through the power of science! {Hotkey}";
			}

			// Token: 0x02002EAC RID: 11948
			public static class ARCHAEOLOGY
			{
				// Token: 0x0400C0B4 RID: 49332
				public static LocString NAME = "Archaeology";

				// Token: 0x0400C0B5 RID: 49333
				public static LocString BUILDMENUTITLE = "Archaeology";

				// Token: 0x0400C0B6 RID: 49334
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002EAD RID: 11949
			public static class METEORDEFENSE
			{
				// Token: 0x0400C0B7 RID: 49335
				public static LocString NAME = "Meteor Defense";

				// Token: 0x0400C0B8 RID: 49336
				public static LocString BUILDMENUTITLE = "Meteor Defense";

				// Token: 0x0400C0B9 RID: 49337
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002EAE RID: 11950
			public static class INDUSTRIALSTATION
			{
				// Token: 0x0400C0BA RID: 49338
				public static LocString NAME = "Industrial";

				// Token: 0x0400C0BB RID: 49339
				public static LocString BUILDMENUTITLE = "Industrial";

				// Token: 0x0400C0BC RID: 49340
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002EAF RID: 11951
			public static class TELESCOPES
			{
				// Token: 0x0400C0BD RID: 49341
				public static LocString NAME = "Telescopes";

				// Token: 0x0400C0BE RID: 49342
				public static LocString BUILDMENUTITLE = "Telescopes";

				// Token: 0x0400C0BF RID: 49343
				public static LocString TOOLTIP = "Unlock new technologies through the power of science! {Hotkey}";
			}

			// Token: 0x02002EB0 RID: 11952
			public static class MISSILES
			{
				// Token: 0x0400C0C0 RID: 49344
				public static LocString NAME = "Meteor Defense";

				// Token: 0x0400C0C1 RID: 49345
				public static LocString BUILDMENUTITLE = "Meteor Defense";

				// Token: 0x0400C0C2 RID: 49346
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002EB1 RID: 11953
			public static class FITTINGS
			{
				// Token: 0x0400C0C3 RID: 49347
				public static LocString NAME = "Fittings";

				// Token: 0x0400C0C4 RID: 49348
				public static LocString BUILDMENUTITLE = "Fittings";

				// Token: 0x0400C0C5 RID: 49349
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002EB2 RID: 11954
			public static class SANITATION
			{
				// Token: 0x0400C0C6 RID: 49350
				public static LocString NAME = "Sanitation";

				// Token: 0x0400C0C7 RID: 49351
				public static LocString BUILDMENUTITLE = "Sanitation";

				// Token: 0x0400C0C8 RID: 49352
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002EB3 RID: 11955
			public static class AUTOMATED
			{
				// Token: 0x0400C0C9 RID: 49353
				public static LocString NAME = "Automated";

				// Token: 0x0400C0CA RID: 49354
				public static LocString BUILDMENUTITLE = "Automated";

				// Token: 0x0400C0CB RID: 49355
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002EB4 RID: 11956
			public static class ROCKETSTRUCTURES
			{
				// Token: 0x0400C0CC RID: 49356
				public static LocString NAME = "Structural";

				// Token: 0x0400C0CD RID: 49357
				public static LocString BUILDMENUTITLE = "Structural";

				// Token: 0x0400C0CE RID: 49358
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002EB5 RID: 11957
			public static class ROCKETNAV
			{
				// Token: 0x0400C0CF RID: 49359
				public static LocString NAME = "Navigation";

				// Token: 0x0400C0D0 RID: 49360
				public static LocString BUILDMENUTITLE = "Navigation";

				// Token: 0x0400C0D1 RID: 49361
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002EB6 RID: 11958
			public static class CONDUITSENSORS
			{
				// Token: 0x0400C0D2 RID: 49362
				public static LocString NAME = "Pipe Sensors";

				// Token: 0x0400C0D3 RID: 49363
				public static LocString BUILDMENUTITLE = "Pipe Sensors";

				// Token: 0x0400C0D4 RID: 49364
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002EB7 RID: 11959
			public static class ROCKETRY
			{
				// Token: 0x0400C0D5 RID: 49365
				public static LocString NAME = "Rocketry";

				// Token: 0x0400C0D6 RID: 49366
				public static LocString BUILDMENUTITLE = "Rocketry";

				// Token: 0x0400C0D7 RID: 49367
				public static LocString TOOLTIP = "Rocketry {Hotkey}";
			}

			// Token: 0x02002EB8 RID: 11960
			public static class ENGINES
			{
				// Token: 0x0400C0D8 RID: 49368
				public static LocString NAME = "Engines";

				// Token: 0x0400C0D9 RID: 49369
				public static LocString BUILDMENUTITLE = "Engines";

				// Token: 0x0400C0DA RID: 49370
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002EB9 RID: 11961
			public static class TANKS
			{
				// Token: 0x0400C0DB RID: 49371
				public static LocString NAME = "Tanks";

				// Token: 0x0400C0DC RID: 49372
				public static LocString BUILDMENUTITLE = "Tanks";

				// Token: 0x0400C0DD RID: 49373
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002EBA RID: 11962
			public static class CARGO
			{
				// Token: 0x0400C0DE RID: 49374
				public static LocString NAME = "Cargo";

				// Token: 0x0400C0DF RID: 49375
				public static LocString BUILDMENUTITLE = "Cargo";

				// Token: 0x0400C0E0 RID: 49376
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002EBB RID: 11963
			public static class MODULE
			{
				// Token: 0x0400C0E1 RID: 49377
				public static LocString NAME = "Modules";

				// Token: 0x0400C0E2 RID: 49378
				public static LocString BUILDMENUTITLE = "Modules";

				// Token: 0x0400C0E3 RID: 49379
				public static LocString TOOLTIP = "";
			}
		}

		// Token: 0x02002EBC RID: 11964
		public class TOOLS
		{
			// Token: 0x0400C0E4 RID: 49380
			public static LocString TOOL_AREA_FMT = "{0} x {1}\n{2} tiles";

			// Token: 0x0400C0E5 RID: 49381
			public static LocString TOOL_LENGTH_FMT = "{0}";

			// Token: 0x0400C0E6 RID: 49382
			public static LocString FILTER_HOVERCARD_HEADER = "   <style=\"hovercard_element\">({0})</style>";

			// Token: 0x0400C0E7 RID: 49383
			public static LocString CAPITALS = "<uppercase>{0}</uppercase>";

			// Token: 0x02002EBD RID: 11965
			public class SANDBOX
			{
				// Token: 0x02002EBE RID: 11966
				public class SANDBOX_TOGGLE
				{
					// Token: 0x0400C0E8 RID: 49384
					public static LocString NAME = "SANDBOX";
				}

				// Token: 0x02002EBF RID: 11967
				public class BRUSH
				{
					// Token: 0x0400C0E9 RID: 49385
					public static LocString NAME = "Brush";

					// Token: 0x0400C0EA RID: 49386
					public static LocString HOVERACTION = "PAINT SIM";
				}

				// Token: 0x02002EC0 RID: 11968
				public class SPRINKLE
				{
					// Token: 0x0400C0EB RID: 49387
					public static LocString NAME = "Sprinkle";

					// Token: 0x0400C0EC RID: 49388
					public static LocString HOVERACTION = "SPRINKLE SIM";
				}

				// Token: 0x02002EC1 RID: 11969
				public class FLOOD
				{
					// Token: 0x0400C0ED RID: 49389
					public static LocString NAME = "Fill";

					// Token: 0x0400C0EE RID: 49390
					public static LocString HOVERACTION = "PAINT SECTION";
				}

				// Token: 0x02002EC2 RID: 11970
				public class MARQUEE
				{
					// Token: 0x0400C0EF RID: 49391
					public static LocString NAME = "Marquee";
				}

				// Token: 0x02002EC3 RID: 11971
				public class SAMPLE
				{
					// Token: 0x0400C0F0 RID: 49392
					public static LocString NAME = "Sample";

					// Token: 0x0400C0F1 RID: 49393
					public static LocString HOVERACTION = "COPY SELECTION";
				}

				// Token: 0x02002EC4 RID: 11972
				public class HEATGUN
				{
					// Token: 0x0400C0F2 RID: 49394
					public static LocString NAME = "Heat Gun";

					// Token: 0x0400C0F3 RID: 49395
					public static LocString HOVERACTION = "PAINT HEAT";
				}

				// Token: 0x02002EC5 RID: 11973
				public class RADSTOOL
				{
					// Token: 0x0400C0F4 RID: 49396
					public static LocString NAME = "Radiation Tool";

					// Token: 0x0400C0F5 RID: 49397
					public static LocString HOVERACTION = "PAINT RADS";
				}

				// Token: 0x02002EC6 RID: 11974
				public class STRESSTOOL
				{
					// Token: 0x0400C0F6 RID: 49398
					public static LocString NAME = "Happy Tool";

					// Token: 0x0400C0F7 RID: 49399
					public static LocString HOVERACTION = "PAINT CALM";
				}

				// Token: 0x02002EC7 RID: 11975
				public class SPAWNER
				{
					// Token: 0x0400C0F8 RID: 49400
					public static LocString NAME = "Spawner";

					// Token: 0x0400C0F9 RID: 49401
					public static LocString HOVERACTION = "SPAWN";
				}

				// Token: 0x02002EC8 RID: 11976
				public class CLEAR_FLOOR
				{
					// Token: 0x0400C0FA RID: 49402
					public static LocString NAME = "Clear Floor";

					// Token: 0x0400C0FB RID: 49403
					public static LocString HOVERACTION = "DELETE DEBRIS";
				}

				// Token: 0x02002EC9 RID: 11977
				public class DESTROY
				{
					// Token: 0x0400C0FC RID: 49404
					public static LocString NAME = "Destroy";

					// Token: 0x0400C0FD RID: 49405
					public static LocString HOVERACTION = "DELETE";
				}

				// Token: 0x02002ECA RID: 11978
				public class SPAWN_ENTITY
				{
					// Token: 0x0400C0FE RID: 49406
					public static LocString NAME = "Spawn";
				}

				// Token: 0x02002ECB RID: 11979
				public class FOW
				{
					// Token: 0x0400C0FF RID: 49407
					public static LocString NAME = "Reveal";

					// Token: 0x0400C100 RID: 49408
					public static LocString HOVERACTION = "DE-FOG";
				}

				// Token: 0x02002ECC RID: 11980
				public class CRITTER
				{
					// Token: 0x0400C101 RID: 49409
					public static LocString NAME = "Critter Removal";

					// Token: 0x0400C102 RID: 49410
					public static LocString HOVERACTION = "DELETE CRITTERS";
				}

				// Token: 0x02002ECD RID: 11981
				public class SPAWN_STORY_TRAIT
				{
					// Token: 0x0400C103 RID: 49411
					public static LocString NAME = "Story Trait";

					// Token: 0x0400C104 RID: 49412
					public static LocString HOVERACTION = "PLACE";

					// Token: 0x0400C105 RID: 49413
					public static LocString ERROR_ALREADY_EXISTS = "{StoryName} already exists in this save";

					// Token: 0x0400C106 RID: 49414
					public static LocString ERROR_INVALID_LOCATION = "Invalid location";

					// Token: 0x0400C107 RID: 49415
					public static LocString ERROR_DUPE_HAZARD = "One or more Duplicants are in the way";

					// Token: 0x0400C108 RID: 49416
					public static LocString ERROR_ROBOT_HAZARD = "One or more robots are in the way";

					// Token: 0x0400C109 RID: 49417
					public static LocString ERROR_CREATURE_HAZARD = "One or more critters are in the way";

					// Token: 0x0400C10A RID: 49418
					public static LocString ERROR_BUILDING_HAZARD = "One or more buildings are in the way";
				}
			}

			// Token: 0x02002ECE RID: 11982
			public class GENERIC
			{
				// Token: 0x0400C10B RID: 49419
				public static LocString BACK = "Back";

				// Token: 0x0400C10C RID: 49420
				public static LocString UNKNOWN = "UNKNOWN";

				// Token: 0x0400C10D RID: 49421
				public static LocString BUILDING_HOVER_NAME_FMT = "{Name}    <style=\"hovercard_element\">({Element})</style>";

				// Token: 0x0400C10E RID: 49422
				public static LocString LOGIC_INPUT_HOVER_FMT = "{Port}    <style=\"hovercard_element\">({Name})</style>";

				// Token: 0x0400C10F RID: 49423
				public static LocString LOGIC_OUTPUT_HOVER_FMT = "{Port}    <style=\"hovercard_element\">({Name})</style>";

				// Token: 0x0400C110 RID: 49424
				public static LocString LOGIC_MULTI_INPUT_HOVER_FMT = "{Port}    <style=\"hovercard_element\">({Name})</style>";

				// Token: 0x0400C111 RID: 49425
				public static LocString LOGIC_MULTI_OUTPUT_HOVER_FMT = "{Port}    <style=\"hovercard_element\">({Name})</style>";
			}

			// Token: 0x02002ECF RID: 11983
			public class ATTACK
			{
				// Token: 0x0400C112 RID: 49426
				public static LocString NAME = "Attack";

				// Token: 0x0400C113 RID: 49427
				public static LocString TOOLNAME = "Attack tool";

				// Token: 0x0400C114 RID: 49428
				public static LocString TOOLACTION = "DRAG";
			}

			// Token: 0x02002ED0 RID: 11984
			public class CAPTURE
			{
				// Token: 0x0400C115 RID: 49429
				public static LocString NAME = "Wrangle";

				// Token: 0x0400C116 RID: 49430
				public static LocString TOOLNAME = "Wrangle tool";

				// Token: 0x0400C117 RID: 49431
				public static LocString TOOLACTION = "DRAG";

				// Token: 0x0400C118 RID: 49432
				public static LocString NOT_CAPTURABLE = "Cannot Wrangle";
			}

			// Token: 0x02002ED1 RID: 11985
			public class BUILD
			{
				// Token: 0x0400C119 RID: 49433
				public static LocString NAME = "Build {0}";

				// Token: 0x0400C11A RID: 49434
				public static LocString TOOLNAME = "Build tool";

				// Token: 0x0400C11B RID: 49435
				public static LocString TOOLACTION = UI.CLICK(UI.ClickType.CLICK) + " TO BUILD";

				// Token: 0x0400C11C RID: 49436
				public static LocString TOOLACTION_DRAG = "DRAG";
			}

			// Token: 0x02002ED2 RID: 11986
			public class PLACE
			{
				// Token: 0x0400C11D RID: 49437
				public static LocString NAME = "Place {0}";

				// Token: 0x0400C11E RID: 49438
				public static LocString TOOLNAME = "Place tool";

				// Token: 0x0400C11F RID: 49439
				public static LocString TOOLACTION = UI.CLICK(UI.ClickType.CLICK) + " TO PLACE";

				// Token: 0x02002ED3 RID: 11987
				public class REASONS
				{
					// Token: 0x0400C120 RID: 49440
					public static LocString CAN_OCCUPY_AREA = "Location blocked";

					// Token: 0x0400C121 RID: 49441
					public static LocString ON_FOUNDATION = "Must place on the ground";

					// Token: 0x0400C122 RID: 49442
					public static LocString VISIBLE_TO_SPACE = "Must have a clear path to space";

					// Token: 0x0400C123 RID: 49443
					public static LocString RESTRICT_TO_WORLD = "Incorrect " + UI.CLUSTERMAP.PLANETOID;
				}
			}

			// Token: 0x02002ED4 RID: 11988
			public class MOVETOLOCATION
			{
				// Token: 0x0400C124 RID: 49444
				public static LocString NAME = "Relocate";

				// Token: 0x0400C125 RID: 49445
				public static LocString TOOLNAME = "Relocate Tool";

				// Token: 0x0400C126 RID: 49446
				public static LocString TOOLACTION = UI.CLICK(UI.ClickType.CLICK) ?? "";

				// Token: 0x0400C127 RID: 49447
				public static LocString UNREACHABLE = "UNREACHABLE";
			}

			// Token: 0x02002ED5 RID: 11989
			public class COPYSETTINGS
			{
				// Token: 0x0400C128 RID: 49448
				public static LocString NAME = "Paste Settings";

				// Token: 0x0400C129 RID: 49449
				public static LocString TOOLNAME = "Paste Settings Tool";

				// Token: 0x0400C12A RID: 49450
				public static LocString TOOLACTION = "DRAG";
			}

			// Token: 0x02002ED6 RID: 11990
			public class DIG
			{
				// Token: 0x0400C12B RID: 49451
				public static LocString NAME = "Dig";

				// Token: 0x0400C12C RID: 49452
				public static LocString TOOLNAME = "Dig tool";

				// Token: 0x0400C12D RID: 49453
				public static LocString TOOLACTION = "DRAG";
			}

			// Token: 0x02002ED7 RID: 11991
			public class DISINFECT
			{
				// Token: 0x0400C12E RID: 49454
				public static LocString NAME = "Disinfect";

				// Token: 0x0400C12F RID: 49455
				public static LocString TOOLNAME = "Disinfect tool";

				// Token: 0x0400C130 RID: 49456
				public static LocString TOOLACTION = "DRAG";
			}

			// Token: 0x02002ED8 RID: 11992
			public class DISCONNECT
			{
				// Token: 0x0400C131 RID: 49457
				public static LocString NAME = "Disconnect";

				// Token: 0x0400C132 RID: 49458
				public static LocString TOOLTIP = "Sever conduits and connectors {Hotkey}";

				// Token: 0x0400C133 RID: 49459
				public static LocString TOOLNAME = "Disconnect tool";

				// Token: 0x0400C134 RID: 49460
				public static LocString TOOLACTION = "DRAG";
			}

			// Token: 0x02002ED9 RID: 11993
			public class CANCEL
			{
				// Token: 0x0400C135 RID: 49461
				public static LocString NAME = "Cancel";

				// Token: 0x0400C136 RID: 49462
				public static LocString TOOLNAME = "Cancel tool";

				// Token: 0x0400C137 RID: 49463
				public static LocString TOOLACTION = "DRAG";
			}

			// Token: 0x02002EDA RID: 11994
			public class DECONSTRUCT
			{
				// Token: 0x0400C138 RID: 49464
				public static LocString NAME = "Deconstruct";

				// Token: 0x0400C139 RID: 49465
				public static LocString TOOLNAME = "Deconstruct tool";

				// Token: 0x0400C13A RID: 49466
				public static LocString TOOLACTION = "DRAG";
			}

			// Token: 0x02002EDB RID: 11995
			public class CLEANUPCATEGORY
			{
				// Token: 0x0400C13B RID: 49467
				public static LocString NAME = "Clean";

				// Token: 0x0400C13C RID: 49468
				public static LocString TOOLNAME = "Clean Up tools";
			}

			// Token: 0x02002EDC RID: 11996
			public class PRIORITIESCATEGORY
			{
				// Token: 0x0400C13D RID: 49469
				public static LocString NAME = "Priority";
			}

			// Token: 0x02002EDD RID: 11997
			public class MARKFORSTORAGE
			{
				// Token: 0x0400C13E RID: 49470
				public static LocString NAME = "Sweep";

				// Token: 0x0400C13F RID: 49471
				public static LocString TOOLNAME = "Sweep tool";

				// Token: 0x0400C140 RID: 49472
				public static LocString TOOLACTION = "DRAG";
			}

			// Token: 0x02002EDE RID: 11998
			public class MOP
			{
				// Token: 0x0400C141 RID: 49473
				public static LocString NAME = "Mop";

				// Token: 0x0400C142 RID: 49474
				public static LocString TOOLNAME = "Mop tool";

				// Token: 0x0400C143 RID: 49475
				public static LocString TOOLACTION = "DRAG";

				// Token: 0x0400C144 RID: 49476
				public static LocString TOO_MUCH_LIQUID = "Too Much Liquid";

				// Token: 0x0400C145 RID: 49477
				public static LocString NOT_ON_FLOOR = "Not On Floor";
			}

			// Token: 0x02002EDF RID: 11999
			public class HARVEST
			{
				// Token: 0x0400C146 RID: 49478
				public static LocString NAME = "Harvest";

				// Token: 0x0400C147 RID: 49479
				public static LocString TOOLNAME = "Harvest tool";

				// Token: 0x0400C148 RID: 49480
				public static LocString TOOLACTION = "DRAG";
			}

			// Token: 0x02002EE0 RID: 12000
			public class PRIORITIZE
			{
				// Token: 0x0400C149 RID: 49481
				public static LocString NAME = "Priority";

				// Token: 0x0400C14A RID: 49482
				public static LocString TOOLNAME = "Priority tool";

				// Token: 0x0400C14B RID: 49483
				public static LocString TOOLACTION = "DRAG";

				// Token: 0x0400C14C RID: 49484
				public static LocString SPECIFIC_PRIORITY = "Set Priority: {0}";
			}

			// Token: 0x02002EE1 RID: 12001
			public class EMPTY_PIPE
			{
				// Token: 0x0400C14D RID: 49485
				public static LocString NAME = "Empty Pipe";

				// Token: 0x0400C14E RID: 49486
				public static LocString TOOLTIP = "Extract pipe contents {Hotkey}";

				// Token: 0x0400C14F RID: 49487
				public static LocString TOOLNAME = "Empty Pipe tool";

				// Token: 0x0400C150 RID: 49488
				public static LocString TOOLACTION = "DRAG";
			}

			// Token: 0x02002EE2 RID: 12002
			public class FILTERSCREEN
			{
				// Token: 0x0400C151 RID: 49489
				public static LocString OPTIONS = "Tool Filter";
			}

			// Token: 0x02002EE3 RID: 12003
			public class FILTERLAYERS
			{
				// Token: 0x02002EE4 RID: 12004
				public class BUILDINGS
				{
					// Token: 0x0400C152 RID: 49490
					public static LocString NAME = "Buildings";

					// Token: 0x0400C153 RID: 49491
					public static LocString TOOLTIP = "All buildings";
				}

				// Token: 0x02002EE5 RID: 12005
				public class TILES
				{
					// Token: 0x0400C154 RID: 49492
					public static LocString NAME = "Tiles";

					// Token: 0x0400C155 RID: 49493
					public static LocString TOOLTIP = "Tiles only";
				}

				// Token: 0x02002EE6 RID: 12006
				public class WIRES
				{
					// Token: 0x0400C156 RID: 49494
					public static LocString NAME = "Power Wires";

					// Token: 0x0400C157 RID: 49495
					public static LocString TOOLTIP = "Power wires only";
				}

				// Token: 0x02002EE7 RID: 12007
				public class SOLIDCONDUITS
				{
					// Token: 0x0400C158 RID: 49496
					public static LocString NAME = "Conveyor Rails";

					// Token: 0x0400C159 RID: 49497
					public static LocString TOOLTIP = "Conveyor rails only";
				}

				// Token: 0x02002EE8 RID: 12008
				public class DIGPLACER
				{
					// Token: 0x0400C15A RID: 49498
					public static LocString NAME = "Dig Orders";

					// Token: 0x0400C15B RID: 49499
					public static LocString TOOLTIP = "Dig orders only";
				}

				// Token: 0x02002EE9 RID: 12009
				public class CLEANANDCLEAR
				{
					// Token: 0x0400C15C RID: 49500
					public static LocString NAME = "Sweep & Mop Orders";

					// Token: 0x0400C15D RID: 49501
					public static LocString TOOLTIP = "Sweep and mop orders only";
				}

				// Token: 0x02002EEA RID: 12010
				public class HARVEST_WHEN_READY
				{
					// Token: 0x0400C15E RID: 49502
					public static LocString NAME = "Enable Harvest";

					// Token: 0x0400C15F RID: 49503
					public static LocString TOOLTIP = "Enable harvest on selected plants";
				}

				// Token: 0x02002EEB RID: 12011
				public class DO_NOT_HARVEST
				{
					// Token: 0x0400C160 RID: 49504
					public static LocString NAME = "Disable Harvest";

					// Token: 0x0400C161 RID: 49505
					public static LocString TOOLTIP = "Disable harvest on selected plants";
				}

				// Token: 0x02002EEC RID: 12012
				public class ATTACK
				{
					// Token: 0x0400C162 RID: 49506
					public static LocString NAME = "Attack";

					// Token: 0x0400C163 RID: 49507
					public static LocString TOOLTIP = "";
				}

				// Token: 0x02002EED RID: 12013
				public class LOGIC
				{
					// Token: 0x0400C164 RID: 49508
					public static LocString NAME = "Automation";

					// Token: 0x0400C165 RID: 49509
					public static LocString TOOLTIP = "Automation buildings only";
				}

				// Token: 0x02002EEE RID: 12014
				public class BACKWALL
				{
					// Token: 0x0400C166 RID: 49510
					public static LocString NAME = "Background Buildings";

					// Token: 0x0400C167 RID: 49511
					public static LocString TOOLTIP = "Background buildings only";
				}

				// Token: 0x02002EEF RID: 12015
				public class LIQUIDPIPES
				{
					// Token: 0x0400C168 RID: 49512
					public static LocString NAME = "Liquid Pipes";

					// Token: 0x0400C169 RID: 49513
					public static LocString TOOLTIP = "Liquid pipes only";
				}

				// Token: 0x02002EF0 RID: 12016
				public class GASPIPES
				{
					// Token: 0x0400C16A RID: 49514
					public static LocString NAME = "Gas Pipes";

					// Token: 0x0400C16B RID: 49515
					public static LocString TOOLTIP = "Gas pipes only";
				}

				// Token: 0x02002EF1 RID: 12017
				public class ALL
				{
					// Token: 0x0400C16C RID: 49516
					public static LocString NAME = "All";

					// Token: 0x0400C16D RID: 49517
					public static LocString TOOLTIP = "Target all";
				}

				// Token: 0x02002EF2 RID: 12018
				public class ALL_OVERLAY
				{
					// Token: 0x0400C16E RID: 49518
					public static LocString NAME = "All";

					// Token: 0x0400C16F RID: 49519
					public static LocString TOOLTIP = "Show all";
				}

				// Token: 0x02002EF3 RID: 12019
				public class METAL
				{
					// Token: 0x0400C170 RID: 49520
					public static LocString NAME = "Metal";

					// Token: 0x0400C171 RID: 49521
					public static LocString TOOLTIP = "Show only metals";
				}

				// Token: 0x02002EF4 RID: 12020
				public class BUILDABLE
				{
					// Token: 0x0400C172 RID: 49522
					public static LocString NAME = "Mineral";

					// Token: 0x0400C173 RID: 49523
					public static LocString TOOLTIP = "Show only minerals";
				}

				// Token: 0x02002EF5 RID: 12021
				public class FILTER
				{
					// Token: 0x0400C174 RID: 49524
					public static LocString NAME = "Filtration Medium";

					// Token: 0x0400C175 RID: 49525
					public static LocString TOOLTIP = "Show only filtration mediums";
				}

				// Token: 0x02002EF6 RID: 12022
				public class CONSUMABLEORE
				{
					// Token: 0x0400C176 RID: 49526
					public static LocString NAME = "Consumable Ore";

					// Token: 0x0400C177 RID: 49527
					public static LocString TOOLTIP = "Show only consumable ore";
				}

				// Token: 0x02002EF7 RID: 12023
				public class ORGANICS
				{
					// Token: 0x0400C178 RID: 49528
					public static LocString NAME = "Organic";

					// Token: 0x0400C179 RID: 49529
					public static LocString TOOLTIP = "Show only organic materials";
				}

				// Token: 0x02002EF8 RID: 12024
				public class FARMABLE
				{
					// Token: 0x0400C17A RID: 49530
					public static LocString NAME = "Cultivable Soil";

					// Token: 0x0400C17B RID: 49531
					public static LocString TOOLTIP = "Show only cultivable soil";
				}

				// Token: 0x02002EF9 RID: 12025
				public class LIQUIFIABLE
				{
					// Token: 0x0400C17C RID: 49532
					public static LocString NAME = "Liquefiable";

					// Token: 0x0400C17D RID: 49533
					public static LocString TOOLTIP = "Show only liquefiable elements";
				}

				// Token: 0x02002EFA RID: 12026
				public class GAS
				{
					// Token: 0x0400C17E RID: 49534
					public static LocString NAME = "Gas";

					// Token: 0x0400C17F RID: 49535
					public static LocString TOOLTIP = "Show only gases";
				}

				// Token: 0x02002EFB RID: 12027
				public class LIQUID
				{
					// Token: 0x0400C180 RID: 49536
					public static LocString NAME = "Liquid";

					// Token: 0x0400C181 RID: 49537
					public static LocString TOOLTIP = "Show only liquids";
				}

				// Token: 0x02002EFC RID: 12028
				public class MISC
				{
					// Token: 0x0400C182 RID: 49538
					public static LocString NAME = "Miscellaneous";

					// Token: 0x0400C183 RID: 49539
					public static LocString TOOLTIP = "Show only miscellaneous elements";
				}

				// Token: 0x02002EFD RID: 12029
				public class ABSOLUTETEMPERATURE
				{
					// Token: 0x0400C184 RID: 49540
					public static LocString NAME = "Absolute Temperature";

					// Token: 0x0400C185 RID: 49541
					public static LocString TOOLTIP = "<b>Absolute Temperature</b>\nView the default temperature ranges and categories relative to absolute zero";
				}

				// Token: 0x02002EFE RID: 12030
				public class RELATIVETEMPERATURE
				{
					// Token: 0x0400C186 RID: 49542
					public static LocString NAME = "Relative Temperature";

					// Token: 0x0400C187 RID: 49543
					public static LocString TOOLTIP = "<b>Relative Temperature</b>\nCustomize visual map to identify temperatures relative to a selected midpoint\n\nDrag the slider to adjust the relative temperature range";
				}

				// Token: 0x02002EFF RID: 12031
				public class HEATFLOW
				{
					// Token: 0x0400C188 RID: 49544
					public static LocString NAME = "Thermal Tolerance";

					// Token: 0x0400C189 RID: 49545
					public static LocString TOOLTIP = "<b>Thermal Tolerance</b>\nView the impact of ambient temperatures on living beings";
				}

				// Token: 0x02002F00 RID: 12032
				public class STATECHANGE
				{
					// Token: 0x0400C18A RID: 49546
					public static LocString NAME = "State Change";

					// Token: 0x0400C18B RID: 49547
					public static LocString TOOLTIP = "<b>State Change</b>\nView the impact of ambient temperatures on element states";
				}

				// Token: 0x02002F01 RID: 12033
				public class BREATHABLE
				{
					// Token: 0x0400C18C RID: 49548
					public static LocString NAME = "Breathable Gas";

					// Token: 0x0400C18D RID: 49549
					public static LocString TOOLTIP = "Show only breathable gases";
				}

				// Token: 0x02002F02 RID: 12034
				public class UNBREATHABLE
				{
					// Token: 0x0400C18E RID: 49550
					public static LocString NAME = "Unbreathable Gas";

					// Token: 0x0400C18F RID: 49551
					public static LocString TOOLTIP = "Show only unbreathable gases";
				}

				// Token: 0x02002F03 RID: 12035
				public class AGRICULTURE
				{
					// Token: 0x0400C190 RID: 49552
					public static LocString NAME = "Agriculture";

					// Token: 0x0400C191 RID: 49553
					public static LocString TOOLTIP = "";
				}

				// Token: 0x02002F04 RID: 12036
				public class ADAPTIVETEMPERATURE
				{
					// Token: 0x0400C192 RID: 49554
					public static LocString NAME = "Adapt. Temperature";

					// Token: 0x0400C193 RID: 49555
					public static LocString TOOLTIP = "";
				}

				// Token: 0x02002F05 RID: 12037
				public class CONSTRUCTION
				{
					// Token: 0x0400C194 RID: 49556
					public static LocString NAME = "Construction";

					// Token: 0x0400C195 RID: 49557
					public static LocString TOOLTIP = string.Concat(new string[]
					{
						"Target ",
						UI.PRE_KEYWORD,
						"Construction",
						UI.PST_KEYWORD,
						" errands only"
					});
				}

				// Token: 0x02002F06 RID: 12038
				public class DIG
				{
					// Token: 0x0400C196 RID: 49558
					public static LocString NAME = "Digging";

					// Token: 0x0400C197 RID: 49559
					public static LocString TOOLTIP = string.Concat(new string[]
					{
						"Target ",
						UI.PRE_KEYWORD,
						"Digging",
						UI.PST_KEYWORD,
						" errands only"
					});
				}

				// Token: 0x02002F07 RID: 12039
				public class CLEAN
				{
					// Token: 0x0400C198 RID: 49560
					public static LocString NAME = "Cleaning";

					// Token: 0x0400C199 RID: 49561
					public static LocString TOOLTIP = "Target cleaning errands only";
				}

				// Token: 0x02002F08 RID: 12040
				public class OPERATE
				{
					// Token: 0x0400C19A RID: 49562
					public static LocString NAME = "Duties";

					// Token: 0x0400C19B RID: 49563
					public static LocString TOOLTIP = "Target general duties only";
				}
			}
		}

		// Token: 0x02002F09 RID: 12041
		public class DETAILTABS
		{
			// Token: 0x02002F0A RID: 12042
			public class STATS
			{
				// Token: 0x0400C19C RID: 49564
				public static LocString NAME = "Skills";

				// Token: 0x0400C19D RID: 49565
				public static LocString TOOLTIP = "<b>Skills</b>\nView this Duplicant's resume and attributes";

				// Token: 0x0400C19E RID: 49566
				public static LocString GROUPNAME_ATTRIBUTES = "ATTRIBUTES";

				// Token: 0x0400C19F RID: 49567
				public static LocString GROUPNAME_STRESS = "TODAY'S STRESS";

				// Token: 0x0400C1A0 RID: 49568
				public static LocString GROUPNAME_EXPECTATIONS = "EXPECTATIONS";

				// Token: 0x0400C1A1 RID: 49569
				public static LocString GROUPNAME_TRAITS = "TRAITS";
			}

			// Token: 0x02002F0B RID: 12043
			public class SIMPLEINFO
			{
				// Token: 0x0400C1A2 RID: 49570
				public static LocString NAME = "Status";

				// Token: 0x0400C1A3 RID: 49571
				public static LocString TOOLTIP = "<b>Status</b>\nView current status";

				// Token: 0x0400C1A4 RID: 49572
				public static LocString GROUPNAME_STATUS = "STATUS";

				// Token: 0x0400C1A5 RID: 49573
				public static LocString GROUPNAME_DESCRIPTION = "INFORMATION";

				// Token: 0x0400C1A6 RID: 49574
				public static LocString GROUPNAME_CONDITION = "CONDITION";

				// Token: 0x0400C1A7 RID: 49575
				public static LocString GROUPNAME_REQUIREMENTS = "REQUIREMENTS";

				// Token: 0x0400C1A8 RID: 49576
				public static LocString GROUPNAME_EFFECTS = "EFFECTS";

				// Token: 0x0400C1A9 RID: 49577
				public static LocString GROUPNAME_RESEARCH = "RESEARCH";

				// Token: 0x0400C1AA RID: 49578
				public static LocString GROUPNAME_LORE = "RECOVERED FILES";

				// Token: 0x0400C1AB RID: 49579
				public static LocString GROUPNAME_FERTILITY = "EGG CHANCES";

				// Token: 0x0400C1AC RID: 49580
				public static LocString GROUPNAME_ROCKET = "ROCKETRY";

				// Token: 0x0400C1AD RID: 49581
				public static LocString GROUPNAME_CARGOBAY = "CARGO BAYS";

				// Token: 0x0400C1AE RID: 49582
				public static LocString GROUPNAME_ELEMENTS = "RESOURCES";

				// Token: 0x0400C1AF RID: 49583
				public static LocString GROUPNAME_LIFE = "LIFEFORMS";

				// Token: 0x0400C1B0 RID: 49584
				public static LocString GROUPNAME_BIOMES = "BIOMES";

				// Token: 0x0400C1B1 RID: 49585
				public static LocString GROUPNAME_GEYSERS = "GEYSERS";

				// Token: 0x0400C1B2 RID: 49586
				public static LocString GROUPNAME_METEORSHOWERS = "METEOR SHOWERS";

				// Token: 0x0400C1B3 RID: 49587
				public static LocString GROUPNAME_WORLDTRAITS = "WORLD TRAITS";

				// Token: 0x0400C1B4 RID: 49588
				public static LocString GROUPNAME_CLUSTER_POI = "POINT OF INTEREST";

				// Token: 0x0400C1B5 RID: 49589
				public static LocString GROUPNAME_MOVABLE = "MOVING";

				// Token: 0x0400C1B6 RID: 49590
				public static LocString NO_METEORSHOWERS = "No meteor showers forecasted";

				// Token: 0x0400C1B7 RID: 49591
				public static LocString NO_GEYSERS = "No geysers detected";

				// Token: 0x0400C1B8 RID: 49592
				public static LocString UNKNOWN_GEYSERS = "Unknown Geysers ({num})";
			}

			// Token: 0x02002F0C RID: 12044
			public class DETAILS
			{
				// Token: 0x0400C1B9 RID: 49593
				public static LocString NAME = "Properties";

				// Token: 0x0400C1BA RID: 49594
				public static LocString MINION_NAME = "About";

				// Token: 0x0400C1BB RID: 49595
				public static LocString TOOLTIP = "<b>Properties</b>\nView elements, temperature, germs and more";

				// Token: 0x0400C1BC RID: 49596
				public static LocString MINION_TOOLTIP = "More information";

				// Token: 0x0400C1BD RID: 49597
				public static LocString GROUPNAME_DETAILS = "DETAILS";

				// Token: 0x0400C1BE RID: 49598
				public static LocString GROUPNAME_CONTENTS = "CONTENTS";

				// Token: 0x0400C1BF RID: 49599
				public static LocString GROUPNAME_MINION_CONTENTS = "CARRIED ITEMS";

				// Token: 0x0400C1C0 RID: 49600
				public static LocString STORAGE_EMPTY = "None";

				// Token: 0x0400C1C1 RID: 49601
				public static LocString CONTENTS_MASS = "{0}: {1}";

				// Token: 0x0400C1C2 RID: 49602
				public static LocString CONTENTS_TEMPERATURE = "{0} at {1}";

				// Token: 0x0400C1C3 RID: 49603
				public static LocString CONTENTS_ROTTABLE = "\n • {0}";

				// Token: 0x0400C1C4 RID: 49604
				public static LocString CONTENTS_DISEASED = "\n • {0}";

				// Token: 0x0400C1C5 RID: 49605
				public static LocString NET_STRESS = "<b>Today's Net Stress: {0}%</b>";

				// Token: 0x02002F0D RID: 12045
				public class RADIATIONABSORPTIONFACTOR
				{
					// Token: 0x0400C1C6 RID: 49606
					public static LocString NAME = "Radiation Blocking: {0}";

					// Token: 0x0400C1C7 RID: 49607
					public static LocString TOOLTIP = "This object will block approximately {0} of radiation.";
				}
			}

			// Token: 0x02002F0E RID: 12046
			public class PERSONALITY
			{
				// Token: 0x0400C1C8 RID: 49608
				public static LocString NAME = "Bio";

				// Token: 0x0400C1C9 RID: 49609
				public static LocString TOOLTIP = "<b>Bio</b>\nView this Duplicant's personality, skills, traits and amenities";

				// Token: 0x0400C1CA RID: 49610
				public static LocString GROUPNAME_BIO = "ABOUT";

				// Token: 0x0400C1CB RID: 49611
				public static LocString GROUPNAME_RESUME = "{0}'S RESUME";

				// Token: 0x02002F0F RID: 12047
				public class RESUME
				{
					// Token: 0x0400C1CC RID: 49612
					public static LocString MASTERED_SKILLS = "<b><size=13>Learned Skills:</size></b>";

					// Token: 0x0400C1CD RID: 49613
					public static LocString MASTERED_SKILLS_TOOLTIP = string.Concat(new string[]
					{
						"All ",
						UI.PRE_KEYWORD,
						"Traits",
						UI.PST_KEYWORD,
						" and ",
						UI.PRE_KEYWORD,
						"Morale Needs",
						UI.PST_KEYWORD,
						" become permanent once a Duplicant has learned a new ",
						UI.PRE_KEYWORD,
						"Skill",
						UI.PST_KEYWORD,
						"\n\n",
						STRINGS.BUILDINGS.PREFABS.RESETSKILLSSTATION.NAME,
						"s can be built from the ",
						UI.FormatAsBuildMenuTab("Stations Tab", global::Action.Plan10),
						" to completely reset a Duplicant's learned ",
						UI.PRE_KEYWORD,
						"Skills",
						UI.PST_KEYWORD,
						", refunding all ",
						UI.PRE_KEYWORD,
						"Skill Points",
						UI.PST_KEYWORD
					});

					// Token: 0x0400C1CE RID: 49614
					public static LocString JOBTRAINING_TOOLTIP = string.Concat(new string[]
					{
						"{0} learned this ",
						UI.PRE_KEYWORD,
						"Skill",
						UI.PST_KEYWORD,
						" while working as a {1}"
					});

					// Token: 0x02002F10 RID: 12048
					public class APTITUDES
					{
						// Token: 0x0400C1CF RID: 49615
						public static LocString NAME = "<b><size=13>Personal Interests:</size></b>";

						// Token: 0x0400C1D0 RID: 49616
						public static LocString TOOLTIP = "{0} enjoys these types of work";
					}

					// Token: 0x02002F11 RID: 12049
					public class PERKS
					{
						// Token: 0x0400C1D1 RID: 49617
						public static LocString NAME = "<b><size=13>Skill Training:</size></b>";

						// Token: 0x0400C1D2 RID: 49618
						public static LocString TOOLTIP = "These are permanent skills {0} gained from learned skills";
					}

					// Token: 0x02002F12 RID: 12050
					public class CURRENT_ROLE
					{
						// Token: 0x0400C1D3 RID: 49619
						public static LocString NAME = "<size=13><b>Current Job:</b> {0}</size>";

						// Token: 0x0400C1D4 RID: 49620
						public static LocString TOOLTIP = "{0} is currently working as a {1}";

						// Token: 0x0400C1D5 RID: 49621
						public static LocString NOJOB_TOOLTIP = "This {0} is... \"between jobs\" at present";
					}

					// Token: 0x02002F13 RID: 12051
					public class NO_MASTERED_SKILLS
					{
						// Token: 0x0400C1D6 RID: 49622
						public static LocString NAME = "None";

						// Token: 0x0400C1D7 RID: 49623
						public static LocString TOOLTIP = string.Concat(new string[]
						{
							"{0} has not learned any ",
							UI.PRE_KEYWORD,
							"Skills",
							UI.PST_KEYWORD,
							" yet"
						});
					}
				}

				// Token: 0x02002F14 RID: 12052
				public class EQUIPMENT
				{
					// Token: 0x0400C1D8 RID: 49624
					public static LocString GROUPNAME_ROOMS = "AMENITIES";

					// Token: 0x0400C1D9 RID: 49625
					public static LocString GROUPNAME_OWNABLE = "EQUIPMENT";

					// Token: 0x0400C1DA RID: 49626
					public static LocString NO_ASSIGNABLES = "None";

					// Token: 0x0400C1DB RID: 49627
					public static LocString NO_ASSIGNABLES_TOOLTIP = "{0} has not been assigned any buildings of their own";

					// Token: 0x0400C1DC RID: 49628
					public static LocString UNASSIGNED = "Unassigned";

					// Token: 0x0400C1DD RID: 49629
					public static LocString UNASSIGNED_TOOLTIP = "This Duplicant has not been assigned a {0}";

					// Token: 0x0400C1DE RID: 49630
					public static LocString ASSIGNED_TOOLTIP = "{2} has been assigned a {0}\n\nEffects: {1}";

					// Token: 0x0400C1DF RID: 49631
					public static LocString NOEQUIPMENT = "None";

					// Token: 0x0400C1E0 RID: 49632
					public static LocString NOEQUIPMENT_TOOLTIP = "{0}'s wearing their Printday Suit and nothing more";
				}
			}

			// Token: 0x02002F15 RID: 12053
			public class ENERGYCONSUMER
			{
				// Token: 0x0400C1E1 RID: 49633
				public static LocString NAME = "Energy";

				// Token: 0x0400C1E2 RID: 49634
				public static LocString TOOLTIP = "View how much power this building consumes";
			}

			// Token: 0x02002F16 RID: 12054
			public class ENERGYWIRE
			{
				// Token: 0x0400C1E3 RID: 49635
				public static LocString NAME = "Energy";

				// Token: 0x0400C1E4 RID: 49636
				public static LocString TOOLTIP = "View this wire's network";
			}

			// Token: 0x02002F17 RID: 12055
			public class ENERGYGENERATOR
			{
				// Token: 0x0400C1E5 RID: 49637
				public static LocString NAME = "Energy";

				// Token: 0x0400C1E6 RID: 49638
				public static LocString TOOLTIP = "<b>Energy</b>\nMonitor the power this building is generating";

				// Token: 0x0400C1E7 RID: 49639
				public static LocString CIRCUITOVERVIEW = "CIRCUIT OVERVIEW";

				// Token: 0x0400C1E8 RID: 49640
				public static LocString GENERATORS = "POWER GENERATORS";

				// Token: 0x0400C1E9 RID: 49641
				public static LocString CONSUMERS = "POWER CONSUMERS";

				// Token: 0x0400C1EA RID: 49642
				public static LocString BATTERIES = "BATTERIES";

				// Token: 0x0400C1EB RID: 49643
				public static LocString DISCONNECTED = "Not connected to an electrical circuit";

				// Token: 0x0400C1EC RID: 49644
				public static LocString NOGENERATORS = "No generators on this circuit";

				// Token: 0x0400C1ED RID: 49645
				public static LocString NOCONSUMERS = "No consumers on this circuit";

				// Token: 0x0400C1EE RID: 49646
				public static LocString NOBATTERIES = "No batteries on this circuit";

				// Token: 0x0400C1EF RID: 49647
				public static LocString AVAILABLE_JOULES = UI.FormatAsLink("Power", "POWER") + " stored: {0}";

				// Token: 0x0400C1F0 RID: 49648
				public static LocString AVAILABLE_JOULES_TOOLTIP = "Amount of power stored in batteries";

				// Token: 0x0400C1F1 RID: 49649
				public static LocString WATTAGE_GENERATED = UI.FormatAsLink("Power", "POWER") + " produced: {0}";

				// Token: 0x0400C1F2 RID: 49650
				public static LocString WATTAGE_GENERATED_TOOLTIP = "The total amount of power generated by this circuit";

				// Token: 0x0400C1F3 RID: 49651
				public static LocString WATTAGE_CONSUMED = UI.FormatAsLink("Power", "POWER") + " consumed: {0}";

				// Token: 0x0400C1F4 RID: 49652
				public static LocString WATTAGE_CONSUMED_TOOLTIP = "The total amount of power used by this circuit";

				// Token: 0x0400C1F5 RID: 49653
				public static LocString POTENTIAL_WATTAGE_CONSUMED = "Potential power consumed: {0}";

				// Token: 0x0400C1F6 RID: 49654
				public static LocString POTENTIAL_WATTAGE_CONSUMED_TOOLTIP = "The total amount of power that can be used by this circuit if all connected buildings are active";

				// Token: 0x0400C1F7 RID: 49655
				public static LocString MAX_SAFE_WATTAGE = "Maximum safe wattage: {0}";

				// Token: 0x0400C1F8 RID: 49656
				public static LocString MAX_SAFE_WATTAGE_TOOLTIP = "Exceeding this value will overload the circuit and can result in damage to wiring and buildings";
			}

			// Token: 0x02002F18 RID: 12056
			public class DISEASE
			{
				// Token: 0x0400C1F9 RID: 49657
				public static LocString NAME = "Germs";

				// Token: 0x0400C1FA RID: 49658
				public static LocString TOOLTIP = "<b>Germs</b>\nView germ resistance and risk of contagion";

				// Token: 0x0400C1FB RID: 49659
				public static LocString DISEASE_SOURCE = "DISEASE SOURCE";

				// Token: 0x0400C1FC RID: 49660
				public static LocString IMMUNE_SYSTEM = "GERM HOST";

				// Token: 0x0400C1FD RID: 49661
				public static LocString CONTRACTION_RATES = "CONTRACTION RATES";

				// Token: 0x0400C1FE RID: 49662
				public static LocString CURRENT_GERMS = "SURFACE GERMS";

				// Token: 0x0400C1FF RID: 49663
				public static LocString NO_CURRENT_GERMS = "SURFACE GERMS";

				// Token: 0x0400C200 RID: 49664
				public static LocString GERMS_INFO = "GERM LIFE CYCLE";

				// Token: 0x0400C201 RID: 49665
				public static LocString INFECTION_INFO = "INFECTION DETAILS";

				// Token: 0x0400C202 RID: 49666
				public static LocString DISEASE_INFO_POPUP_HEADER = "DISEASE INFO: {0}";

				// Token: 0x0400C203 RID: 49667
				public static LocString DISEASE_INFO_POPUP_BUTTON = "FULL INFO";

				// Token: 0x0400C204 RID: 49668
				public static LocString DISEASE_INFO_POPUP_TOOLTIP = "View detailed germ and infection info for {0}";

				// Token: 0x02002F19 RID: 12057
				public class DETAILS
				{
					// Token: 0x0400C205 RID: 49669
					public static LocString NODISEASE = "No surface germs";

					// Token: 0x0400C206 RID: 49670
					public static LocString NODISEASE_TOOLTIP = "There are no germs present on this object";

					// Token: 0x0400C207 RID: 49671
					public static LocString DISEASE_AMOUNT = "{0}: {1}";

					// Token: 0x0400C208 RID: 49672
					public static LocString DISEASE_AMOUNT_TOOLTIP = "{0} are present on the surface of the selected object";

					// Token: 0x0400C209 RID: 49673
					public static LocString DEATH_FORMAT = "{0} dead/cycle";

					// Token: 0x0400C20A RID: 49674
					public static LocString DEATH_FORMAT_TOOLTIP = "Germ count is being reduced by {0}/cycle";

					// Token: 0x0400C20B RID: 49675
					public static LocString GROWTH_FORMAT = "{0} spawned/cycle";

					// Token: 0x0400C20C RID: 49676
					public static LocString GROWTH_FORMAT_TOOLTIP = "Germ count is being increased by {0}/cycle";

					// Token: 0x0400C20D RID: 49677
					public static LocString NEUTRAL_FORMAT = "No change";

					// Token: 0x0400C20E RID: 49678
					public static LocString NEUTRAL_FORMAT_TOOLTIP = "Germ count is static";

					// Token: 0x02002F1A RID: 12058
					public class GROWTH_FACTORS
					{
						// Token: 0x0400C20F RID: 49679
						public static LocString TITLE = "\nGrowth factors:";

						// Token: 0x0400C210 RID: 49680
						public static LocString TOOLTIP = "These conditions are contributing to the multiplication of germs";

						// Token: 0x0400C211 RID: 49681
						public static LocString RATE_OF_CHANGE = "Change rate: {0}";

						// Token: 0x0400C212 RID: 49682
						public static LocString RATE_OF_CHANGE_TOOLTIP = "Germ count is fluctuating at a rate of {0}";

						// Token: 0x0400C213 RID: 49683
						public static LocString HALF_LIFE_NEG = "Half life: {0}";

						// Token: 0x0400C214 RID: 49684
						public static LocString HALF_LIFE_NEG_TOOLTIP = "In {0} the germ count on this object will be halved";

						// Token: 0x0400C215 RID: 49685
						public static LocString HALF_LIFE_POS = "Doubling time: {0}";

						// Token: 0x0400C216 RID: 49686
						public static LocString HALF_LIFE_POS_TOOLTIP = "In {0} the germ count on this object will be doubled";

						// Token: 0x0400C217 RID: 49687
						public static LocString HALF_LIFE_NEUTRAL = "Static";

						// Token: 0x0400C218 RID: 49688
						public static LocString HALF_LIFE_NEUTRAL_TOOLTIP = "The germ count is neither increasing nor decreasing";

						// Token: 0x02002F1B RID: 12059
						public class SUBSTRATE
						{
							// Token: 0x0400C219 RID: 49689
							public static LocString GROW = "    • Growing on {0}: {1}";

							// Token: 0x0400C21A RID: 49690
							public static LocString GROW_TOOLTIP = "Contact with this substance is causing germs to multiply";

							// Token: 0x0400C21B RID: 49691
							public static LocString NEUTRAL = "    • No change on {0}";

							// Token: 0x0400C21C RID: 49692
							public static LocString NEUTRAL_TOOLTIP = "Contact with this substance has no effect on germ count";

							// Token: 0x0400C21D RID: 49693
							public static LocString DIE = "    • Dying on {0}: {1}";

							// Token: 0x0400C21E RID: 49694
							public static LocString DIE_TOOLTIP = "Contact with this substance is causing germs to die off";
						}

						// Token: 0x02002F1C RID: 12060
						public class ENVIRONMENT
						{
							// Token: 0x0400C21F RID: 49695
							public static LocString TITLE = "    • Surrounded by {0}: {1}";

							// Token: 0x0400C220 RID: 49696
							public static LocString GROW_TOOLTIP = "This atmosphere is causing germs to multiply";

							// Token: 0x0400C221 RID: 49697
							public static LocString DIE_TOOLTIP = "This atmosphere is causing germs to die off";
						}

						// Token: 0x02002F1D RID: 12061
						public class TEMPERATURE
						{
							// Token: 0x0400C222 RID: 49698
							public static LocString TITLE = "    • Current temperature {0}: {1}";

							// Token: 0x0400C223 RID: 49699
							public static LocString GROW_TOOLTIP = "This temperature is allowing germs to multiply";

							// Token: 0x0400C224 RID: 49700
							public static LocString DIE_TOOLTIP = "This temperature is causing germs to die off";
						}

						// Token: 0x02002F1E RID: 12062
						public class PRESSURE
						{
							// Token: 0x0400C225 RID: 49701
							public static LocString TITLE = "    • Current pressure {0}: {1}";

							// Token: 0x0400C226 RID: 49702
							public static LocString GROW_TOOLTIP = "Atmospheric pressure is causing germs to multiply";

							// Token: 0x0400C227 RID: 49703
							public static LocString DIE_TOOLTIP = "Atmospheric pressure is causing germs to die off";
						}

						// Token: 0x02002F1F RID: 12063
						public class RADIATION
						{
							// Token: 0x0400C228 RID: 49704
							public static LocString TITLE = "    • Exposed to {0} Rads: {1}";

							// Token: 0x0400C229 RID: 49705
							public static LocString DIE_TOOLTIP = "Radiation exposure is causing germs to die off";
						}

						// Token: 0x02002F20 RID: 12064
						public class DYING_OFF
						{
							// Token: 0x0400C22A RID: 49706
							public static LocString TITLE = "    • <b>Dying off: {0}</b>";

							// Token: 0x0400C22B RID: 49707
							public static LocString TOOLTIP = "Low germ count in this area is causing germs to die rapidly\n\nFewer than {0} are on this {1} of material.\n({2} germs/" + UI.UNITSUFFIXES.MASS.KILOGRAM + ")";
						}

						// Token: 0x02002F21 RID: 12065
						public class OVERPOPULATED
						{
							// Token: 0x0400C22C RID: 49708
							public static LocString TITLE = "    • <b>Overpopulated: {0}</b>";

							// Token: 0x0400C22D RID: 49709
							public static LocString TOOLTIP = "Too many germs are present in this area, resulting in rapid die-off until the population stabilizes\n\nA maximum of {0} can be on this {1} of material.\n({2} germs/" + UI.UNITSUFFIXES.MASS.KILOGRAM + ")";
						}
					}
				}
			}

			// Token: 0x02002F22 RID: 12066
			public class NEEDS
			{
				// Token: 0x0400C22E RID: 49710
				public static LocString NAME = "Stress";

				// Token: 0x0400C22F RID: 49711
				public static LocString TOOLTIP = "View this Duplicant's psychological status";

				// Token: 0x0400C230 RID: 49712
				public static LocString CURRENT_STRESS_LEVEL = "Current " + UI.FormatAsLink("Stress", "STRESS") + " Level: {0}";

				// Token: 0x0400C231 RID: 49713
				public static LocString OVERVIEW = "Overview";

				// Token: 0x0400C232 RID: 49714
				public static LocString STRESS_CREATORS = UI.FormatAsLink("Stress", "STRESS") + " Creators";

				// Token: 0x0400C233 RID: 49715
				public static LocString STRESS_RELIEVERS = UI.FormatAsLink("Stress", "STRESS") + " Relievers";

				// Token: 0x0400C234 RID: 49716
				public static LocString CURRENT_NEED_LEVEL = "Current Level: {0}";

				// Token: 0x0400C235 RID: 49717
				public static LocString NEXT_NEED_LEVEL = "Next Level: {0}";
			}

			// Token: 0x02002F23 RID: 12067
			public class EGG_CHANCES
			{
				// Token: 0x0400C236 RID: 49718
				public static LocString CHANCE_FORMAT = "{0}: {1}";

				// Token: 0x0400C237 RID: 49719
				public static LocString CHANCE_FORMAT_TOOLTIP = "This critter has a {1} chance of laying {0}s.\n\nThis probability increases when the creature:\n{2}";

				// Token: 0x0400C238 RID: 49720
				public static LocString CHANCE_MOD_FORMAT = "    • {0}\n";

				// Token: 0x0400C239 RID: 49721
				public static LocString CHANCE_FORMAT_TOOLTIP_NOMOD = "This critter has a {1} chance of laying {0}s.";
			}

			// Token: 0x02002F24 RID: 12068
			public class BUILDING_CHORES
			{
				// Token: 0x0400C23A RID: 49722
				public static LocString NAME = "Errands";

				// Token: 0x0400C23B RID: 49723
				public static LocString TOOLTIP = "<b>Errands</b>\nView available errands and current queue";

				// Token: 0x0400C23C RID: 49724
				public static LocString CHORE_TYPE_TOOLTIP = "Errand Type: {0}";

				// Token: 0x0400C23D RID: 49725
				public static LocString AVAILABLE_CHORES = "AVAILABLE ERRANDS";

				// Token: 0x0400C23E RID: 49726
				public static LocString DUPE_TOOLTIP_FAILED = "{Name} cannot currently {Errand}\n\nReason:\n{FailedPrecondition}";

				// Token: 0x0400C23F RID: 49727
				public static LocString DUPE_TOOLTIP_SUCCEEDED = "{Description}\n\n{Errand}'s Type: {Groups}\n\n{Name}'s {BestGroup} Priority: {PersonalPriorityValue} ({PersonalPriority})\n{Building} Priority: {BuildingPriority}\nAll {BestGroup} Errands: {TypePriority}\n\nTotal Priority: {TotalPriority}";

				// Token: 0x0400C240 RID: 49728
				public static LocString DUPE_TOOLTIP_DESC_ACTIVE = "{Name} is currently busy: \"{Errand}\"";

				// Token: 0x0400C241 RID: 49729
				public static LocString DUPE_TOOLTIP_DESC_INACTIVE = "\"{Errand}\" is #{Rank} on {Name}'s To Do list, after they finish their current errand";
			}

			// Token: 0x02002F25 RID: 12069
			public class PROCESS_CONDITIONS
			{
				// Token: 0x0400C242 RID: 49730
				public static LocString NAME = "LAUNCH CHECKLIST";

				// Token: 0x0400C243 RID: 49731
				public static LocString ROCKETPREP = "Rocket Construction";

				// Token: 0x0400C244 RID: 49732
				public static LocString ROCKETPREP_TOOLTIP = "It is recommended that all boxes on the Rocket Construction checklist be ticked before launching";

				// Token: 0x0400C245 RID: 49733
				public static LocString ROCKETSTORAGE = "Cargo Manifest";

				// Token: 0x0400C246 RID: 49734
				public static LocString ROCKETSTORAGE_TOOLTIP = "It is recommended that all boxes on the Cargo Manifest checklist be ticked before launching";

				// Token: 0x0400C247 RID: 49735
				public static LocString ROCKETFLIGHT = "Flight Route";

				// Token: 0x0400C248 RID: 49736
				public static LocString ROCKETFLIGHT_TOOLTIP = "A rocket requires a clear path to a set destination to conduct a mission";

				// Token: 0x0400C249 RID: 49737
				public static LocString ROCKETBOARD = "Crew Manifest";

				// Token: 0x0400C24A RID: 49738
				public static LocString ROCKETBOARD_TOOLTIP = "It is recommended that all boxes on the Crew Manifest checklist be ticked before launching";

				// Token: 0x0400C24B RID: 49739
				public static LocString ALL = "Requirements";

				// Token: 0x0400C24C RID: 49740
				public static LocString ALL_TOOLTIP = "These conditions must be fulfilled in order to launch a rocket mission";
			}

			// Token: 0x02002F26 RID: 12070
			public class COSMETICS
			{
				// Token: 0x0400C24D RID: 49741
				public static LocString NAME = "Blueprint";

				// Token: 0x0400C24E RID: 49742
				public static LocString TOOLTIP = "<b>Blueprint</b>\nView and change assigned blueprints";
			}

			// Token: 0x02002F27 RID: 12071
			public class MATERIAL
			{
				// Token: 0x0400C24F RID: 49743
				public static LocString NAME = "Material";

				// Token: 0x0400C250 RID: 49744
				public static LocString TOOLTIP = "<b>Material</b>\nView and change this building's construction material";

				// Token: 0x0400C251 RID: 49745
				public static LocString SUB_HEADER_CURRENT_MATERIAL = "CURRENT MATERIAL";

				// Token: 0x0400C252 RID: 49746
				public static LocString BUTTON_CHANGE_MATERIAL = "Change Material";
			}

			// Token: 0x02002F28 RID: 12072
			public class CONFIGURATION
			{
				// Token: 0x0400C253 RID: 49747
				public static LocString NAME = "Config";

				// Token: 0x0400C254 RID: 49748
				public static LocString TOOLTIP = "<b>Config</b>\nView and change filters, recipes, production orders and more";
			}
		}

		// Token: 0x02002F29 RID: 12073
		public class BUILDMENU
		{
			// Token: 0x0400C255 RID: 49749
			public static LocString GRID_VIEW_TOGGLE_TOOLTIP = "Toggle Grid View";

			// Token: 0x0400C256 RID: 49750
			public static LocString LIST_VIEW_TOGGLE_TOOLTIP = "Toggle List View";

			// Token: 0x0400C257 RID: 49751
			public static LocString NO_SEARCH_RESULTS = "NO RESULTS FOUND";

			// Token: 0x0400C258 RID: 49752
			public static LocString SEARCH_RESULTS_HEADER = "SEARCH RESULTS";

			// Token: 0x0400C259 RID: 49753
			public static LocString SEARCH_TEXT_PLACEHOLDER = "Search all buildings...";

			// Token: 0x0400C25A RID: 49754
			public static LocString CLEAR_SEARCH_TOOLTIP = "Clear search";
		}

		// Token: 0x02002F2A RID: 12074
		public class BUILDINGEFFECTS
		{
			// Token: 0x0400C25B RID: 49755
			public static LocString OPERATIONREQUIREMENTS = "<b>Requirements:</b>";

			// Token: 0x0400C25C RID: 49756
			public static LocString REQUIRESPOWER = UI.FormatAsLink("Power", "POWER") + ": {0}";

			// Token: 0x0400C25D RID: 49757
			public static LocString REQUIRESELEMENT = "Supply of {0}";

			// Token: 0x0400C25E RID: 49758
			public static LocString REQUIRESLIQUIDINPUT = UI.FormatAsLink("Liquid Intake Pipe", "LIQUIDPIPING");

			// Token: 0x0400C25F RID: 49759
			public static LocString REQUIRESLIQUIDOUTPUT = UI.FormatAsLink("Liquid Output Pipe", "LIQUIDPIPING");

			// Token: 0x0400C260 RID: 49760
			public static LocString REQUIRESLIQUIDOUTPUTS = "Two " + UI.FormatAsLink("Liquid Output Pipes", "LIQUIDPIPING");

			// Token: 0x0400C261 RID: 49761
			public static LocString REQUIRESGASINPUT = UI.FormatAsLink("Gas Intake Pipe", "GASPIPING");

			// Token: 0x0400C262 RID: 49762
			public static LocString REQUIRESGASOUTPUT = UI.FormatAsLink("Gas Output Pipe", "GASPIPING");

			// Token: 0x0400C263 RID: 49763
			public static LocString REQUIRESGASOUTPUTS = "Two " + UI.FormatAsLink("Gas Output Pipes", "GASPIPING");

			// Token: 0x0400C264 RID: 49764
			public static LocString REQUIRESMANUALOPERATION = "Duplicant operation";

			// Token: 0x0400C265 RID: 49765
			public static LocString REQUIRESCREATIVITY = "Duplicant " + UI.FormatAsLink("Creativity", "ARTIST");

			// Token: 0x0400C266 RID: 49766
			public static LocString REQUIRESPOWERGENERATOR = UI.FormatAsLink("Power", "POWER") + " generator";

			// Token: 0x0400C267 RID: 49767
			public static LocString REQUIRESSEED = "1 Unplanted " + UI.FormatAsLink("Seed", "PLANTS");

			// Token: 0x0400C268 RID: 49768
			public static LocString PREFERS_ROOM = "Preferred Room: {0}";

			// Token: 0x0400C269 RID: 49769
			public static LocString REQUIRESROOM = "Dedicated Room: {0}";

			// Token: 0x0400C26A RID: 49770
			public static LocString ALLOWS_FERTILIZER = "Plant " + UI.FormatAsLink("Fertilization", "WILTCONDITIONS");

			// Token: 0x0400C26B RID: 49771
			public static LocString ALLOWS_IRRIGATION = "Plant " + UI.FormatAsLink("Liquid", "WILTCONDITIONS");

			// Token: 0x0400C26C RID: 49772
			public static LocString ASSIGNEDDUPLICANT = "Duplicant assignment";

			// Token: 0x0400C26D RID: 49773
			public static LocString CONSUMESANYELEMENT = "Any Element";

			// Token: 0x0400C26E RID: 49774
			public static LocString ENABLESDOMESTICGROWTH = "Enables " + UI.FormatAsLink("Plant Domestication", "PLANTS");

			// Token: 0x0400C26F RID: 49775
			public static LocString TRANSFORMER_INPUT_WIRE = "Input " + UI.FormatAsLink("Power Wire", "WIRE");

			// Token: 0x0400C270 RID: 49776
			public static LocString TRANSFORMER_OUTPUT_WIRE = "Output " + UI.FormatAsLink("Power Wire", "WIRE") + " (Limited to {0})";

			// Token: 0x0400C271 RID: 49777
			public static LocString OPERATIONEFFECTS = "<b>Effects:</b>";

			// Token: 0x0400C272 RID: 49778
			public static LocString BATTERYCAPACITY = UI.FormatAsLink("Power", "POWER") + " capacity: {0}";

			// Token: 0x0400C273 RID: 49779
			public static LocString BATTERYLEAK = UI.FormatAsLink("Power", "POWER") + " leak: {0}";

			// Token: 0x0400C274 RID: 49780
			public static LocString STORAGECAPACITY = "Storage capacity: {0}";

			// Token: 0x0400C275 RID: 49781
			public static LocString ELEMENTEMITTED_INPUTTEMP = "{0}: {1}";

			// Token: 0x0400C276 RID: 49782
			public static LocString ELEMENTEMITTED_ENTITYTEMP = "{0}: {1}";

			// Token: 0x0400C277 RID: 49783
			public static LocString ELEMENTEMITTED_MINORENTITYTEMP = "{0}: {1}";

			// Token: 0x0400C278 RID: 49784
			public static LocString ELEMENTEMITTED_MINTEMP = "{0}: {1}";

			// Token: 0x0400C279 RID: 49785
			public static LocString ELEMENTEMITTED_FIXEDTEMP = "{0}: {1}";

			// Token: 0x0400C27A RID: 49786
			public static LocString ELEMENTCONSUMED = "{0}: {1}";

			// Token: 0x0400C27B RID: 49787
			public static LocString ELEMENTEMITTED_TOILET = "{0}: {1} per use";

			// Token: 0x0400C27C RID: 49788
			public static LocString ELEMENTEMITTEDPERUSE = "{0}: {1} per use";

			// Token: 0x0400C27D RID: 49789
			public static LocString DISEASEEMITTEDPERUSE = "{0}: {1} per use";

			// Token: 0x0400C27E RID: 49790
			public static LocString DISEASECONSUMEDPERUSE = "All Diseases: -{0} per use";

			// Token: 0x0400C27F RID: 49791
			public static LocString ELEMENTCONSUMEDPERUSE = "{0}: {1} per use";

			// Token: 0x0400C280 RID: 49792
			public static LocString ENERGYCONSUMED = UI.FormatAsLink("Power", "POWER") + " consumed: {0}";

			// Token: 0x0400C281 RID: 49793
			public static LocString ENERGYGENERATED = UI.FormatAsLink("Power", "POWER") + ": +{0}";

			// Token: 0x0400C282 RID: 49794
			public static LocString HEATGENERATED = UI.FormatAsLink("Heat", "HEAT") + ": +{0}/s";

			// Token: 0x0400C283 RID: 49795
			public static LocString HEATCONSUMED = UI.FormatAsLink("Heat", "HEAT") + ": -{0}/s";

			// Token: 0x0400C284 RID: 49796
			public static LocString HEATER_TARGETTEMPERATURE = "Target " + UI.FormatAsLink("Temperature", "HEAT") + ": {0}";

			// Token: 0x0400C285 RID: 49797
			public static LocString HEATGENERATED_AIRCONDITIONER = UI.FormatAsLink("Heat", "HEAT") + ": +{0} (Approximate Value)";

			// Token: 0x0400C286 RID: 49798
			public static LocString HEATGENERATED_LIQUIDCONDITIONER = UI.FormatAsLink("Heat", "HEAT") + ": +{0} (Approximate Value)";

			// Token: 0x0400C287 RID: 49799
			public static LocString FABRICATES = "Fabricates";

			// Token: 0x0400C288 RID: 49800
			public static LocString FABRICATEDITEM = "{1}";

			// Token: 0x0400C289 RID: 49801
			public static LocString PROCESSES = "Refines:";

			// Token: 0x0400C28A RID: 49802
			public static LocString PROCESSEDITEM = "{1} {0}";

			// Token: 0x0400C28B RID: 49803
			public static LocString PLANTERBOX_PENTALTY = "Planter box penalty";

			// Token: 0x0400C28C RID: 49804
			public static LocString DECORPROVIDED = UI.FormatAsLink("Decor", "DECOR") + ": {1} (Radius: {2} tiles)";

			// Token: 0x0400C28D RID: 49805
			public static LocString OVERHEAT_TEMP = "Overheat " + UI.FormatAsLink("Temperature", "HEAT") + ": {0}";

			// Token: 0x0400C28E RID: 49806
			public static LocString MINIMUM_TEMP = "Freeze " + UI.FormatAsLink("Temperature", "HEAT") + ": {0}";

			// Token: 0x0400C28F RID: 49807
			public static LocString OVER_PRESSURE_MASS = "Overpressure: {0}";

			// Token: 0x0400C290 RID: 49808
			public static LocString REFILLOXYGENTANK = "Refills Exosuit " + STRINGS.EQUIPMENT.PREFABS.OXYGEN_TANK.NAME;

			// Token: 0x0400C291 RID: 49809
			public static LocString DUPLICANTMOVEMENTBOOST = "Runspeed: {0}";

			// Token: 0x0400C292 RID: 49810
			public static LocString ELECTROBANKS = UI.FormatAsLink("Charge", "POWER") + ": {0}";

			// Token: 0x0400C293 RID: 49811
			public static LocString STRESSREDUCEDPERMINUTE = UI.FormatAsLink("Stress", "STRESS") + ": {0} per minute";

			// Token: 0x0400C294 RID: 49812
			public static LocString REMOVESEFFECTSUBTITLE = "Cures";

			// Token: 0x0400C295 RID: 49813
			public static LocString REMOVEDEFFECT = "{0}";

			// Token: 0x0400C296 RID: 49814
			public static LocString ADDED_EFFECT = "Added Effect: {0}";

			// Token: 0x0400C297 RID: 49815
			public static LocString GASCOOLING = UI.FormatAsLink("Cooling factor", "HEAT") + ": {0}";

			// Token: 0x0400C298 RID: 49816
			public static LocString LIQUIDCOOLING = UI.FormatAsLink("Cooling factor", "HEAT") + ": {0}";

			// Token: 0x0400C299 RID: 49817
			public static LocString MAX_WATTAGE = "Max " + UI.FormatAsLink("Power", "POWER") + ": {0}";

			// Token: 0x0400C29A RID: 49818
			public static LocString MAX_BITS = UI.FormatAsLink("Bit", "LOGIC") + " Depth: {0}";

			// Token: 0x0400C29B RID: 49819
			public static LocString RESEARCH_MATERIALS = "{0}: {1} per " + UI.FormatAsLink("Research", "RESEARCH") + " point";

			// Token: 0x0400C29C RID: 49820
			public static LocString PRODUCES_RESEARCH_POINTS = "{0}";

			// Token: 0x0400C29D RID: 49821
			public static LocString HIT_POINTS_PER_CYCLE = UI.FormatAsLink("Health", "Health") + " per cycle: {0}";

			// Token: 0x0400C29E RID: 49822
			public static LocString KCAL_PER_CYCLE = UI.FormatAsLink("KCal", "FOOD") + " per cycle: {0}";

			// Token: 0x0400C29F RID: 49823
			public static LocString REMOVES_DISEASE = "Kills germs";

			// Token: 0x0400C2A0 RID: 49824
			public static LocString DOCTORING = "Doctoring";

			// Token: 0x0400C2A1 RID: 49825
			public static LocString RECREATION = "Recreation";

			// Token: 0x0400C2A2 RID: 49826
			public static LocString COOLANT = "Coolant: {1} {0}";

			// Token: 0x0400C2A3 RID: 49827
			public static LocString REFINEMENT_ENERGY = "Heat: {0}";

			// Token: 0x0400C2A4 RID: 49828
			public static LocString IMPROVED_BUILDINGS = "Improved Buildings";

			// Token: 0x0400C2A5 RID: 49829
			public static LocString IMPROVED_PLANTS = "Improved Plants";

			// Token: 0x0400C2A6 RID: 49830
			public static LocString IMPROVED_BUILDINGS_ITEM = "{0}";

			// Token: 0x0400C2A7 RID: 49831
			public static LocString IMPROVED_PLANTS_ITEM = "{0}";

			// Token: 0x0400C2A8 RID: 49832
			public static LocString GEYSER_PRODUCTION = "{0}: {1} at {2}";

			// Token: 0x0400C2A9 RID: 49833
			public static LocString GEYSER_DISEASE = "Germs: {0}";

			// Token: 0x0400C2AA RID: 49834
			public static LocString GEYSER_PERIOD = "Eruption Period: {0} every {1}";

			// Token: 0x0400C2AB RID: 49835
			public static LocString GEYSER_YEAR_UNSTUDIED = "Active Period: (Requires Analysis)";

			// Token: 0x0400C2AC RID: 49836
			public static LocString GEYSER_YEAR_PERIOD = "Active Period: {0} every {1}";

			// Token: 0x0400C2AD RID: 49837
			public static LocString GEYSER_YEAR_NEXT_ACTIVE = "Next Activity: {0}";

			// Token: 0x0400C2AE RID: 49838
			public static LocString GEYSER_YEAR_NEXT_DORMANT = "Next Dormancy: {0}";

			// Token: 0x0400C2AF RID: 49839
			public static LocString GEYSER_YEAR_AVR_OUTPUT_UNSTUDIED = "Average Output: (Requires Analysis)";

			// Token: 0x0400C2B0 RID: 49840
			public static LocString GEYSER_YEAR_AVR_OUTPUT = "Average Output: {0}";

			// Token: 0x0400C2B1 RID: 49841
			public static LocString CAPTURE_METHOD_WRANGLE = "Capture Method: Wrangling";

			// Token: 0x0400C2B2 RID: 49842
			public static LocString CAPTURE_METHOD_LURE = "Capture Method: Lures";

			// Token: 0x0400C2B3 RID: 49843
			public static LocString CAPTURE_METHOD_TRAP = "Capture Method: Traps";

			// Token: 0x0400C2B4 RID: 49844
			public static LocString DIET_HEADER = "Digestion:";

			// Token: 0x0400C2B5 RID: 49845
			public static LocString DIET_CONSUMED = "    • Diet: {Foodlist}";

			// Token: 0x0400C2B6 RID: 49846
			public static LocString DIET_STORED = "    • Stores: {Foodlist}";

			// Token: 0x0400C2B7 RID: 49847
			public static LocString DIET_CONSUMED_ITEM = "{Food}: {Amount}";

			// Token: 0x0400C2B8 RID: 49848
			public static LocString DIET_PRODUCED = "    • Excretion: {Items}";

			// Token: 0x0400C2B9 RID: 49849
			public static LocString DIET_PRODUCED_ITEM = "{Item}: {Percent} of consumed mass";

			// Token: 0x0400C2BA RID: 49850
			public static LocString DIET_PRODUCED_ITEM_FROM_PLANT = "{Item}: {Amount} when properly fed";

			// Token: 0x0400C2BB RID: 49851
			public static LocString SCALE_GROWTH = "Shearable {Item}: {Amount} per {Time}";

			// Token: 0x0400C2BC RID: 49852
			public static LocString SCALE_GROWTH_ATMO = "Shearable {Item}: {Amount} per {Time} ({Atmosphere})";

			// Token: 0x0400C2BD RID: 49853
			public static LocString SCALE_GROWTH_TEMP = "Shearable {Item}: {Amount} per {Time} ({TempMin} - {TempMax})";

			// Token: 0x0400C2BE RID: 49854
			public static LocString ACCESS_CONTROL = "Duplicant Access Permissions";

			// Token: 0x0400C2BF RID: 49855
			public static LocString ROCKETRESTRICTION_HEADER = "Restriction Control:";

			// Token: 0x0400C2C0 RID: 49856
			public static LocString ROCKETRESTRICTION_BUILDINGS = "    • Buildings: {buildinglist}";

			// Token: 0x0400C2C1 RID: 49857
			public static LocString UNSTABLEENTOMBDEFENSEREADY = "Entomb Defense: Ready";

			// Token: 0x0400C2C2 RID: 49858
			public static LocString UNSTABLEENTOMBDEFENSETHREATENED = "Entomb Defense: Threatened";

			// Token: 0x0400C2C3 RID: 49859
			public static LocString UNSTABLEENTOMBDEFENSEREACTING = "Entomb Defense: Reacting";

			// Token: 0x0400C2C4 RID: 49860
			public static LocString UNSTABLEENTOMBDEFENSEOFF = "Entomb Defense: Off";

			// Token: 0x0400C2C5 RID: 49861
			public static LocString ITEM_TEMPERATURE_ADJUST = "Stored " + UI.FormatAsLink("Temperature", "HEAT") + ": {0}";

			// Token: 0x0400C2C6 RID: 49862
			public static LocString NOISE_CREATED = UI.FormatAsLink("Noise", "SOUND") + ": {0} dB (Radius: {1} tiles)";

			// Token: 0x0400C2C7 RID: 49863
			public static LocString MESS_TABLE_SALT = "Table Salt: +{0}";

			// Token: 0x0400C2C8 RID: 49864
			public static LocString ACTIVE_PARTICLE_CONSUMPTION = "Radbolts: {Rate}";

			// Token: 0x0400C2C9 RID: 49865
			public static LocString PARTICLE_PORT_INPUT = "Radbolt Input Port";

			// Token: 0x0400C2CA RID: 49866
			public static LocString PARTICLE_PORT_OUTPUT = "Radbolt Output Port";

			// Token: 0x0400C2CB RID: 49867
			public static LocString IN_ORBIT_REQUIRED = "Active In Space";

			// Token: 0x0400C2CC RID: 49868
			public static LocString KETTLE_MELT_RATE = "Melting Rate: {0}";

			// Token: 0x0400C2CD RID: 49869
			public static LocString FOOD_DEHYDRATOR_WATER_OUTPUT = "Wet Floor";

			// Token: 0x02002F2B RID: 12075
			public class TOOLTIPS
			{
				// Token: 0x0400C2CE RID: 49870
				public static LocString OPERATIONREQUIREMENTS = "All requirements must be met in order for this building to operate";

				// Token: 0x0400C2CF RID: 49871
				public static LocString REQUIRESPOWER = string.Concat(new string[]
				{
					"Must be connected to a power grid with at least ",
					UI.FormatAsNegativeRate("{0}"),
					" of available ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD
				});

				// Token: 0x0400C2D0 RID: 49872
				public static LocString REQUIRESELEMENT = string.Concat(new string[]
				{
					"Must receive deliveries of ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" to function"
				});

				// Token: 0x0400C2D1 RID: 49873
				public static LocString REQUIRESLIQUIDINPUT = string.Concat(new string[]
				{
					"Must receive ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					" from a ",
					STRINGS.BUILDINGS.PREFABS.LIQUIDCONDUIT.NAME,
					" system"
				});

				// Token: 0x0400C2D2 RID: 49874
				public static LocString REQUIRESLIQUIDOUTPUT = string.Concat(new string[]
				{
					"Must expel ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					" through a ",
					STRINGS.BUILDINGS.PREFABS.LIQUIDCONDUIT.NAME,
					" system"
				});

				// Token: 0x0400C2D3 RID: 49875
				public static LocString REQUIRESLIQUIDOUTPUTS = string.Concat(new string[]
				{
					"Must expel ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					" through a ",
					STRINGS.BUILDINGS.PREFABS.LIQUIDCONDUIT.NAME,
					" system"
				});

				// Token: 0x0400C2D4 RID: 49876
				public static LocString REQUIRESGASINPUT = string.Concat(new string[]
				{
					"Must receive ",
					UI.PRE_KEYWORD,
					"Gas",
					UI.PST_KEYWORD,
					" from a ",
					STRINGS.BUILDINGS.PREFABS.GASCONDUIT.NAME,
					" system"
				});

				// Token: 0x0400C2D5 RID: 49877
				public static LocString REQUIRESGASOUTPUT = string.Concat(new string[]
				{
					"Must expel ",
					UI.PRE_KEYWORD,
					"Gas",
					UI.PST_KEYWORD,
					" through a ",
					STRINGS.BUILDINGS.PREFABS.GASCONDUIT.NAME,
					" system"
				});

				// Token: 0x0400C2D6 RID: 49878
				public static LocString REQUIRESGASOUTPUTS = string.Concat(new string[]
				{
					"Must expel ",
					UI.PRE_KEYWORD,
					"Gas",
					UI.PST_KEYWORD,
					" through a ",
					STRINGS.BUILDINGS.PREFABS.GASCONDUIT.NAME,
					" system"
				});

				// Token: 0x0400C2D7 RID: 49879
				public static LocString REQUIRESMANUALOPERATION = "A Duplicant must be present to run this building";

				// Token: 0x0400C2D8 RID: 49880
				public static LocString REQUIRESCREATIVITY = "A Duplicant must work on this object to create " + UI.PRE_KEYWORD + "Art" + UI.PST_KEYWORD;

				// Token: 0x0400C2D9 RID: 49881
				public static LocString REQUIRESPOWERGENERATOR = string.Concat(new string[]
				{
					"Must be connected to a ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" producing generator to function"
				});

				// Token: 0x0400C2DA RID: 49882
				public static LocString REQUIRESSEED = "Must receive a plant " + UI.PRE_KEYWORD + "Seed" + UI.PST_KEYWORD;

				// Token: 0x0400C2DB RID: 49883
				public static LocString PREFERS_ROOM = "This building gains additional effects or functionality when built inside a " + UI.PRE_KEYWORD + "{0}" + UI.PST_KEYWORD;

				// Token: 0x0400C2DC RID: 49884
				public static LocString REQUIRESROOM = string.Concat(new string[]
				{
					"Must be built within a dedicated ",
					UI.PRE_KEYWORD,
					"Room",
					UI.PST_KEYWORD,
					"\n\n",
					UI.PRE_KEYWORD,
					"Room",
					UI.PST_KEYWORD,
					" will become a ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" after construction"
				});

				// Token: 0x0400C2DD RID: 49885
				public static LocString ALLOWS_FERTILIZER = string.Concat(new string[]
				{
					"Allows ",
					UI.PRE_KEYWORD,
					"Fertilizer",
					UI.PST_KEYWORD,
					" to be delivered to plants"
				});

				// Token: 0x0400C2DE RID: 49886
				public static LocString ALLOWS_IRRIGATION = string.Concat(new string[]
				{
					"Allows ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					" to be delivered to plants"
				});

				// Token: 0x0400C2DF RID: 49887
				public static LocString ALLOWS_IRRIGATION_PIPE = string.Concat(new string[]
				{
					"Allows irrigation ",
					UI.PRE_KEYWORD,
					"Pipe",
					UI.PST_KEYWORD,
					" connection"
				});

				// Token: 0x0400C2E0 RID: 49888
				public static LocString ASSIGNEDDUPLICANT = "This amenity may only be used by the Duplicant it is assigned to";

				// Token: 0x0400C2E1 RID: 49889
				public static LocString BUILDINGROOMREQUIREMENTCLASS = "This category of building may be required or prohibited in certain " + UI.PRE_KEYWORD + "Rooms" + UI.PST_KEYWORD;

				// Token: 0x0400C2E2 RID: 49890
				public static LocString OPERATIONEFFECTS = "The building will produce these effects when its requirements are met";

				// Token: 0x0400C2E3 RID: 49891
				public static LocString BATTERYCAPACITY = string.Concat(new string[]
				{
					"Can hold <b>{0}</b> of ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" when connected to a ",
					UI.PRE_KEYWORD,
					"Generator",
					UI.PST_KEYWORD
				});

				// Token: 0x0400C2E4 RID: 49892
				public static LocString BATTERYLEAK = string.Concat(new string[]
				{
					UI.FormatAsNegativeRate("{0}"),
					" of this battery's charge will be lost as ",
					UI.PRE_KEYWORD,
					"Heat",
					UI.PST_KEYWORD
				});

				// Token: 0x0400C2E5 RID: 49893
				public static LocString STORAGECAPACITY = "Holds up to <b>{0}</b> of material";

				// Token: 0x0400C2E6 RID: 49894
				public static LocString ELEMENTEMITTED_INPUTTEMP = string.Concat(new string[]
				{
					"Produces ",
					UI.FormatAsPositiveRate("{1}"),
					" of ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" when in use\n\nIt will be the combined ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" of the input materials."
				});

				// Token: 0x0400C2E7 RID: 49895
				public static LocString ELEMENTEMITTED_ENTITYTEMP = string.Concat(new string[]
				{
					"Produces ",
					UI.FormatAsPositiveRate("{1}"),
					" of ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" when in use\n\nIt will be the ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" of the building at the time of production"
				});

				// Token: 0x0400C2E8 RID: 49896
				public static LocString ELEMENTEMITTED_MINORENTITYTEMP = string.Concat(new string[]
				{
					"Produces ",
					UI.FormatAsPositiveRate("{1}"),
					" of ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" when in use\n\nIt will be at least <b>{2}</b>, or hotter if the building is hotter."
				});

				// Token: 0x0400C2E9 RID: 49897
				public static LocString ELEMENTEMITTED_MINTEMP = string.Concat(new string[]
				{
					"Produces ",
					UI.FormatAsPositiveRate("{1}"),
					" of ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" when in use\n\nIt will be at least <b>{2}</b>, or hotter if the input materials are hotter."
				});

				// Token: 0x0400C2EA RID: 49898
				public static LocString ELEMENTEMITTED_FIXEDTEMP = string.Concat(new string[]
				{
					"Produces ",
					UI.FormatAsPositiveRate("{1}"),
					" of ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" when in use\n\nIt will be produced at <b>{2}</b>."
				});

				// Token: 0x0400C2EB RID: 49899
				public static LocString ELEMENTCONSUMED = string.Concat(new string[]
				{
					"Consumes ",
					UI.FormatAsNegativeRate("{1}"),
					" of ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" when in use"
				});

				// Token: 0x0400C2EC RID: 49900
				public static LocString ELEMENTEMITTED_TOILET = string.Concat(new string[]
				{
					"Produces ",
					UI.FormatAsPositiveRate("{1}"),
					" of ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" per use\n\nDuplicant waste is emitted at <b>{2}</b>."
				});

				// Token: 0x0400C2ED RID: 49901
				public static LocString ELEMENTEMITTEDPERUSE = string.Concat(new string[]
				{
					"Produces ",
					UI.FormatAsPositiveRate("{1}"),
					" of ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" per use\n\nIt will be the ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" of the input materials."
				});

				// Token: 0x0400C2EE RID: 49902
				public static LocString DISEASEEMITTEDPERUSE = string.Concat(new string[]
				{
					"Produces ",
					UI.FormatAsPositiveRate("{1}"),
					" of ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" per use"
				});

				// Token: 0x0400C2EF RID: 49903
				public static LocString DISEASECONSUMEDPERUSE = "Removes " + UI.FormatAsNegativeRate("{0}") + " per use";

				// Token: 0x0400C2F0 RID: 49904
				public static LocString ELEMENTCONSUMEDPERUSE = string.Concat(new string[]
				{
					"Consumes ",
					UI.FormatAsNegativeRate("{1}"),
					" of ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" per use"
				});

				// Token: 0x0400C2F1 RID: 49905
				public static LocString ENERGYCONSUMED = string.Concat(new string[]
				{
					"Draws ",
					UI.FormatAsNegativeRate("{0}"),
					" from the ",
					UI.PRE_KEYWORD,
					"Power Grid",
					UI.PST_KEYWORD,
					" it's connected to"
				});

				// Token: 0x0400C2F2 RID: 49906
				public static LocString ENERGYGENERATED = string.Concat(new string[]
				{
					"Produces ",
					UI.FormatAsPositiveRate("{0}"),
					" for the ",
					UI.PRE_KEYWORD,
					"Power Grid",
					UI.PST_KEYWORD,
					" it's connected to"
				});

				// Token: 0x0400C2F3 RID: 49907
				public static LocString ENABLESDOMESTICGROWTH = string.Concat(new string[]
				{
					"Accelerates ",
					UI.PRE_KEYWORD,
					"Plant",
					UI.PST_KEYWORD,
					" growth and maturation"
				});

				// Token: 0x0400C2F4 RID: 49908
				public static LocString HEATGENERATED = string.Concat(new string[]
				{
					"Generates ",
					UI.FormatAsPositiveRate("{0}"),
					" per second\n\nSum ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" change is affected by the material attributes of the heated substance:\n    • mass\n    • specific heat capacity\n    • surface area\n    • insulation thickness\n    • thermal conductivity"
				});

				// Token: 0x0400C2F5 RID: 49909
				public static LocString HEATCONSUMED = string.Concat(new string[]
				{
					"Dissipates ",
					UI.FormatAsNegativeRate("{0}"),
					" per second\n\nSum ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" change can be affected by the material attributes of the cooled substance:\n    • mass\n    • specific heat capacity\n    • surface area\n    • insulation thickness\n    • thermal conductivity"
				});

				// Token: 0x0400C2F6 RID: 49910
				public static LocString HEATER_TARGETTEMPERATURE = string.Concat(new string[]
				{
					"Stops heating when the surrounding average ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" is above <b>{0}</b>"
				});

				// Token: 0x0400C2F7 RID: 49911
				public static LocString FABRICATES = "Fabrication is the production of items and equipment";

				// Token: 0x0400C2F8 RID: 49912
				public static LocString PROCESSES = "Processes raw materials into refined materials";

				// Token: 0x0400C2F9 RID: 49913
				public static LocString PROCESSEDITEM = "Refining this material produces " + UI.PRE_KEYWORD + "{0}" + UI.PST_KEYWORD;

				// Token: 0x0400C2FA RID: 49914
				public static LocString PLANTERBOX_PENTALTY = "Plants grow more slowly when contained within boxes";

				// Token: 0x0400C2FB RID: 49915
				public static LocString DECORPROVIDED = string.Concat(new string[]
				{
					"Improves ",
					UI.PRE_KEYWORD,
					"Decor",
					UI.PST_KEYWORD,
					" values by ",
					UI.FormatAsPositiveModifier("<b>{0}</b>"),
					" in a <b>{1}</b> tile radius"
				});

				// Token: 0x0400C2FC RID: 49916
				public static LocString DECORDECREASED = string.Concat(new string[]
				{
					"Decreases ",
					UI.PRE_KEYWORD,
					"Decor",
					UI.PST_KEYWORD,
					" values by ",
					UI.FormatAsNegativeModifier("<b>{0}</b>"),
					" in a <b>{1}</b> tile radius"
				});

				// Token: 0x0400C2FD RID: 49917
				public static LocString OVERHEAT_TEMP = "Begins overheating at <b>{0}</b>";

				// Token: 0x0400C2FE RID: 49918
				public static LocString MINIMUM_TEMP = "Ceases to function when temperatures fall below <b>{0}</b>";

				// Token: 0x0400C2FF RID: 49919
				public static LocString OVER_PRESSURE_MASS = "Ceases to function when the surrounding mass is above <b>{0}</b>";

				// Token: 0x0400C300 RID: 49920
				public static LocString REFILLOXYGENTANK = string.Concat(new string[]
				{
					"Refills ",
					UI.PRE_KEYWORD,
					"Exosuit",
					UI.PST_KEYWORD,
					" Oxygen tanks with ",
					UI.PRE_KEYWORD,
					"Oxygen",
					UI.PST_KEYWORD,
					" for reuse"
				});

				// Token: 0x0400C301 RID: 49921
				public static LocString DUPLICANTMOVEMENTBOOST = "Duplicants walk <b>{0}</b> faster on this tile";

				// Token: 0x0400C302 RID: 49922
				public static LocString ELECTROBANKS = string.Concat(new string[]
				{
					"Power Banks store {0} of ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					"\n\nThey can be discharged by circuits, buildings and Bionic Duplicants"
				});

				// Token: 0x0400C303 RID: 49923
				public static LocString STRESSREDUCEDPERMINUTE = string.Concat(new string[]
				{
					"Removes <b>{0}</b> of Duplicants' ",
					UI.PRE_KEYWORD,
					"Stress",
					UI.PST_KEYWORD,
					" for every uninterrupted minute of use"
				});

				// Token: 0x0400C304 RID: 49924
				public static LocString REMOVESEFFECTSUBTITLE = "Use of this building will remove the listed effects";

				// Token: 0x0400C305 RID: 49925
				public static LocString REMOVEDEFFECT = "{0}";

				// Token: 0x0400C306 RID: 49926
				public static LocString ADDED_EFFECT = "Effect being applied:\n\n{0}\n{1}";

				// Token: 0x0400C307 RID: 49927
				public static LocString GASCOOLING = string.Concat(new string[]
				{
					"Reduces the ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" of piped ",
					UI.PRE_KEYWORD,
					"Gases",
					UI.PST_KEYWORD,
					" by <b>{0}</b>"
				});

				// Token: 0x0400C308 RID: 49928
				public static LocString LIQUIDCOOLING = string.Concat(new string[]
				{
					"Reduces the ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" of piped ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					" by <b>{0}</b>"
				});

				// Token: 0x0400C309 RID: 49929
				public static LocString MAX_WATTAGE = string.Concat(new string[]
				{
					"Drawing more than the maximum allowed ",
					UI.PRE_KEYWORD,
					"Watts",
					UI.PST_KEYWORD,
					" can result in damage to the circuit"
				});

				// Token: 0x0400C30A RID: 49930
				public static LocString MAX_BITS = string.Concat(new string[]
				{
					"Sending an ",
					UI.PRE_KEYWORD,
					"Automation Signal",
					UI.PST_KEYWORD,
					" with a higher ",
					UI.PRE_KEYWORD,
					"Bit Depth",
					UI.PST_KEYWORD,
					" than the connected ",
					UI.PRE_KEYWORD,
					"Logic Wire",
					UI.PST_KEYWORD,
					" can result in damage to the circuit"
				});

				// Token: 0x0400C30B RID: 49931
				public static LocString RESEARCH_MATERIALS = string.Concat(new string[]
				{
					"This research station consumes ",
					UI.FormatAsNegativeRate("{1}"),
					" of ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" for each ",
					UI.PRE_KEYWORD,
					"Research Point",
					UI.PST_KEYWORD,
					" produced"
				});

				// Token: 0x0400C30C RID: 49932
				public static LocString PRODUCES_RESEARCH_POINTS = string.Concat(new string[]
				{
					"Produces ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" research"
				});

				// Token: 0x0400C30D RID: 49933
				public static LocString REMOVES_DISEASE = string.Concat(new string[]
				{
					"The cooking process kills all ",
					UI.PRE_KEYWORD,
					"Germs",
					UI.PST_KEYWORD,
					" present in the ingredients, removing the ",
					UI.PRE_KEYWORD,
					"Disease",
					UI.PST_KEYWORD,
					" risk when eating the product"
				});

				// Token: 0x0400C30E RID: 49934
				public static LocString DOCTORING = "Doctoring increases existing health benefits and can allow the treatment of otherwise stubborn " + UI.PRE_KEYWORD + "Diseases" + UI.PST_KEYWORD;

				// Token: 0x0400C30F RID: 49935
				public static LocString RECREATION = string.Concat(new string[]
				{
					"Improves Duplicant ",
					UI.PRE_KEYWORD,
					"Morale",
					UI.PST_KEYWORD,
					" during scheduled ",
					UI.PRE_KEYWORD,
					"Downtime",
					UI.PST_KEYWORD
				});

				// Token: 0x0400C310 RID: 49936
				public static LocString HEATGENERATED_AIRCONDITIONER = string.Concat(new string[]
				{
					"Generates ",
					UI.PRE_KEYWORD,
					"Heat",
					UI.PST_KEYWORD,
					" based on the ",
					UI.PRE_KEYWORD,
					"Volume",
					UI.PST_KEYWORD,
					" and ",
					UI.PRE_KEYWORD,
					"Specific Heat Capacity",
					UI.PST_KEYWORD,
					" of the pumped ",
					UI.PRE_KEYWORD,
					"Gas",
					UI.PST_KEYWORD,
					"\n\nCooling 1",
					UI.UNITSUFFIXES.MASS.KILOGRAM,
					" of ",
					ELEMENTS.OXYGEN.NAME,
					" the entire <b>{1}</b> will output <b>{0}</b>"
				});

				// Token: 0x0400C311 RID: 49937
				public static LocString HEATGENERATED_LIQUIDCONDITIONER = string.Concat(new string[]
				{
					"Generates ",
					UI.PRE_KEYWORD,
					"Heat",
					UI.PST_KEYWORD,
					" based on the ",
					UI.PRE_KEYWORD,
					"Volume",
					UI.PST_KEYWORD,
					" and ",
					UI.PRE_KEYWORD,
					"Specific Heat Capacity",
					UI.PST_KEYWORD,
					" of the pumped ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					"\n\nCooling 10",
					UI.UNITSUFFIXES.MASS.KILOGRAM,
					" of ",
					ELEMENTS.WATER.NAME,
					" the entire <b>{1}</b> will output <b>{0}</b>"
				});

				// Token: 0x0400C312 RID: 49938
				public static LocString MOVEMENT_BONUS = "Increases the Runspeed of Duplicants";

				// Token: 0x0400C313 RID: 49939
				public static LocString COOLANT = string.Concat(new string[]
				{
					"<b>{1}</b> of ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" coolant is required to cool off an item produced by this building\n\nCoolant ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" increase is variable and dictated by the amount of energy needed to cool the produced item"
				});

				// Token: 0x0400C314 RID: 49940
				public static LocString REFINEMENT_ENERGY_HAS_COOLANT = string.Concat(new string[]
				{
					UI.FormatAsPositiveRate("{0}"),
					" of ",
					UI.PRE_KEYWORD,
					"Heat",
					UI.PST_KEYWORD,
					" will be produced to cool off the fabricated item\n\nThis will raise the ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" of the contained ",
					UI.PRE_KEYWORD,
					"{1}",
					UI.PST_KEYWORD,
					" by ",
					UI.FormatAsPositiveModifier("{2}"),
					", and heat the containing building"
				});

				// Token: 0x0400C315 RID: 49941
				public static LocString REFINEMENT_ENERGY_NO_COOLANT = string.Concat(new string[]
				{
					UI.FormatAsPositiveRate("{0}"),
					" of ",
					UI.PRE_KEYWORD,
					"Heat",
					UI.PST_KEYWORD,
					" will be produced to cool off the fabricated item\n\nIf ",
					UI.PRE_KEYWORD,
					"{1}",
					UI.PST_KEYWORD,
					" is used for coolant, its ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" will be raised by ",
					UI.FormatAsPositiveModifier("{2}"),
					", and will heat the containing building"
				});

				// Token: 0x0400C316 RID: 49942
				public static LocString IMPROVED_BUILDINGS = UI.PRE_KEYWORD + "Tune Ups" + UI.PST_KEYWORD + " will improve these buildings:";

				// Token: 0x0400C317 RID: 49943
				public static LocString IMPROVED_BUILDINGS_ITEM = "{0}";

				// Token: 0x0400C318 RID: 49944
				public static LocString IMPROVED_PLANTS = UI.PRE_KEYWORD + "Crop Tending" + UI.PST_KEYWORD + " will improve growth times for these plants:";

				// Token: 0x0400C319 RID: 49945
				public static LocString IMPROVED_PLANTS_ITEM = "{0}";

				// Token: 0x0400C31A RID: 49946
				public static LocString GEYSER_PRODUCTION = string.Concat(new string[]
				{
					"While erupting, this geyser will produce ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" at a rate of ",
					UI.FormatAsPositiveRate("{1}"),
					", and at a ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" of <b>{2}</b>"
				});

				// Token: 0x0400C31B RID: 49947
				public static LocString GEYSER_PRODUCTION_GEOTUNED = string.Concat(new string[]
				{
					"While erupting, this geyser will produce ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" at a rate of ",
					UI.FormatAsPositiveRate("{1}"),
					", and at a ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" of <b>{2}</b>"
				});

				// Token: 0x0400C31C RID: 49948
				public static LocString GEYSER_PRODUCTION_GEOTUNED_COUNT = "<b>{0}</b> of <b>{1}</b> Geotuners targeting this geyser are amplifying it";

				// Token: 0x0400C31D RID: 49949
				public static LocString GEYSER_PRODUCTION_GEOTUNED_TOTAL = "Total geotuning: {0} {1}";

				// Token: 0x0400C31E RID: 49950
				public static LocString GEYSER_PRODUCTION_GEOTUNED_TOTAL_ROW_TITLE = "Geotuned ";

				// Token: 0x0400C31F RID: 49951
				public static LocString GEYSER_DISEASE = UI.PRE_KEYWORD + "{0}" + UI.PST_KEYWORD + " germs are present in the output of this geyser";

				// Token: 0x0400C320 RID: 49952
				public static LocString GEYSER_PERIOD = "This geyser will produce for <b>{0}</b> of every <b>{1}</b>";

				// Token: 0x0400C321 RID: 49953
				public static LocString GEYSER_YEAR_UNSTUDIED = "A researcher must analyze this geyser to determine its geoactive period";

				// Token: 0x0400C322 RID: 49954
				public static LocString GEYSER_YEAR_PERIOD = "This geyser will be active for <b>{0}</b> out of every <b>{1}</b>\n\nIt will be dormant the rest of the time";

				// Token: 0x0400C323 RID: 49955
				public static LocString GEYSER_YEAR_NEXT_ACTIVE = "This geyser will become active in <b>{0}</b>";

				// Token: 0x0400C324 RID: 49956
				public static LocString GEYSER_YEAR_NEXT_DORMANT = "This geyser will become dormant in <b>{0}</b>";

				// Token: 0x0400C325 RID: 49957
				public static LocString GEYSER_YEAR_AVR_OUTPUT_UNSTUDIED = "A researcher must analyze this geyser to determine its average output rate";

				// Token: 0x0400C326 RID: 49958
				public static LocString GEYSER_YEAR_AVR_OUTPUT = "This geyser emits an average of {average} of {element} during its lifetime\n\nThis includes its dormant period";

				// Token: 0x0400C327 RID: 49959
				public static LocString GEYSER_YEAR_AVR_OUTPUT_BREAKDOWN_TITLE = "Total Geotuning ";

				// Token: 0x0400C328 RID: 49960
				public static LocString GEYSER_YEAR_AVR_OUTPUT_BREAKDOWN_ROW = "Geotuned ";

				// Token: 0x0400C329 RID: 49961
				public static LocString CAPTURE_METHOD_WRANGLE = string.Concat(new string[]
				{
					"This critter can be captured\n\nMark critters for capture using the ",
					UI.FormatAsTool("Wrangle Tool", global::Action.Capture),
					"\n\nDuplicants must possess the ",
					UI.PRE_KEYWORD,
					"Critter Ranching",
					UI.PST_KEYWORD,
					" skill in order to wrangle critters"
				});

				// Token: 0x0400C32A RID: 49962
				public static LocString CAPTURE_METHOD_LURE = "This critter can be moved using an " + STRINGS.BUILDINGS.PREFABS.AIRBORNECREATURELURE.NAME;

				// Token: 0x0400C32B RID: 49963
				public static LocString CAPTURE_METHOD_TRAP = "This critter can be captured using a " + STRINGS.BUILDINGS.PREFABS.CREATURETRAP.NAME;

				// Token: 0x0400C32C RID: 49964
				public static LocString NOISE_POLLUTION_INCREASE = "Produces noise at <b>{0} dB</b> in a <b>{1}</b> tile radius";

				// Token: 0x0400C32D RID: 49965
				public static LocString NOISE_POLLUTION_DECREASE = "Dampens noise at <b>{0} dB</b> in a <b>{1}</b> tile radius";

				// Token: 0x0400C32E RID: 49966
				public static LocString ITEM_TEMPERATURE_ADJUST = string.Concat(new string[]
				{
					"Stored items will reach a ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" of <b>{0}</b> over time"
				});

				// Token: 0x0400C32F RID: 49967
				public static LocString DIET_HEADER = "Creatures will eat and digest only specific materials";

				// Token: 0x0400C330 RID: 49968
				public static LocString DIET_CONSUMED = "This critter can typically consume these materials at the following rates:\n\n{Foodlist}";

				// Token: 0x0400C331 RID: 49969
				public static LocString DIET_PRODUCED = "This critter will \"produce\" the following materials:\n\n{Items}";

				// Token: 0x0400C332 RID: 49970
				public static LocString ROCKETRESTRICTION_HEADER = "Controls whether a building is operational within a rocket interior";

				// Token: 0x0400C333 RID: 49971
				public static LocString ROCKETRESTRICTION_BUILDINGS = "This station controls the operational status of the following buildings:\n\n{buildinglist}";

				// Token: 0x0400C334 RID: 49972
				public static LocString UNSTABLEENTOMBDEFENSEREADY = string.Concat(new string[]
				{
					"This plant is ready to shake off ",
					UI.PRE_KEYWORD,
					"Unstable",
					UI.PST_KEYWORD,
					" elements that threaten to entomb it"
				});

				// Token: 0x0400C335 RID: 49973
				public static LocString UNSTABLEENTOMBDEFENSETHREATENED = string.Concat(new string[]
				{
					"This plant is preparing to shake off ",
					UI.PRE_KEYWORD,
					"Unstable",
					UI.PST_KEYWORD,
					" elements that are entombing it"
				});

				// Token: 0x0400C336 RID: 49974
				public static LocString UNSTABLEENTOMBDEFENSEREACTING = string.Concat(new string[]
				{
					"This plant is currently unentombing itself from ",
					UI.PRE_KEYWORD,
					"Unstable",
					UI.PST_KEYWORD,
					" elements"
				});

				// Token: 0x0400C337 RID: 49975
				public static LocString UNSTABLEENTOMBDEFENSEOFF = string.Concat(new string[]
				{
					"This plant's ability to unentomb itself from ",
					UI.PRE_KEYWORD,
					"Unstable",
					UI.PST_KEYWORD,
					" elements is currently disabled"
				});

				// Token: 0x0400C338 RID: 49976
				public static LocString EDIBLE_PLANT_INTERNAL_STORAGE = "{0} of stored {1}";

				// Token: 0x0400C339 RID: 49977
				public static LocString SCALE_GROWTH = string.Concat(new string[]
				{
					"This critter can be sheared every <b>{Time}</b> to produce ",
					UI.FormatAsPositiveModifier("{Amount}"),
					" of ",
					UI.PRE_KEYWORD,
					"{Item}",
					UI.PST_KEYWORD
				});

				// Token: 0x0400C33A RID: 49978
				public static LocString SCALE_GROWTH_ATMO = string.Concat(new string[]
				{
					"This critter can be sheared every <b>{Time}</b> to produce ",
					UI.FormatAsPositiveRate("{Amount}"),
					" of ",
					UI.PRE_KEYWORD,
					"{Item}",
					UI.PST_KEYWORD,
					"\n\nIt must be kept in ",
					UI.PRE_KEYWORD,
					"{Atmosphere}",
					UI.PST_KEYWORD,
					"-rich environments to regrow sheared ",
					UI.PRE_KEYWORD,
					"{Item}",
					UI.PST_KEYWORD
				});

				// Token: 0x0400C33B RID: 49979
				public static LocString SCALE_GROWTH_TEMP = string.Concat(new string[]
				{
					"This critter can be sheared every <b>{Time}</b> to produce ",
					UI.FormatAsPositiveRate("{Amount}"),
					" of ",
					UI.PRE_KEYWORD,
					"{Item}",
					UI.PST_KEYWORD,
					"\n\nIt must eat food between {TempMin} - {TempMax} to regrow sheared ",
					UI.PRE_KEYWORD,
					"{Item}",
					UI.PST_KEYWORD
				});

				// Token: 0x0400C33C RID: 49980
				public static LocString SCALE_GROWTH_FED = string.Concat(new string[]
				{
					"This critter can be sheared every <b>{Time}</b> to produce ",
					UI.FormatAsPositiveModifier("{Amount}"),
					" of ",
					UI.PRE_KEYWORD,
					"{Item}",
					UI.PST_KEYWORD,
					"\n\nIt must be well fed to grow shearable ",
					UI.PRE_KEYWORD,
					"{Item}",
					UI.PST_KEYWORD
				});

				// Token: 0x0400C33D RID: 49981
				public static LocString MESS_TABLE_SALT = string.Concat(new string[]
				{
					"Duplicants gain ",
					UI.FormatAsPositiveModifier("+{0}"),
					" ",
					UI.PRE_KEYWORD,
					"Morale",
					UI.PST_KEYWORD,
					" when using ",
					UI.PRE_KEYWORD,
					"Table Salt",
					UI.PST_KEYWORD,
					" with their food at a ",
					STRINGS.BUILDINGS.PREFABS.DININGTABLE.NAME
				});

				// Token: 0x0400C33E RID: 49982
				public static LocString ACCESS_CONTROL = "Settings to allow or restrict Duplicants from passing through the door.";

				// Token: 0x0400C33F RID: 49983
				public static LocString TRANSFORMER_INPUT_WIRE = string.Concat(new string[]
				{
					"Connect a ",
					UI.PRE_KEYWORD,
					"Wire",
					UI.PST_KEYWORD,
					" to the large ",
					UI.PRE_KEYWORD,
					"Input",
					UI.PST_KEYWORD,
					" with any amount of ",
					UI.PRE_KEYWORD,
					"Watts",
					UI.PST_KEYWORD,
					"."
				});

				// Token: 0x0400C340 RID: 49984
				public static LocString TRANSFORMER_OUTPUT_WIRE = string.Concat(new string[]
				{
					"The ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" provided by the the small ",
					UI.PRE_KEYWORD,
					"Output",
					UI.PST_KEYWORD,
					" will be limited to {0}."
				});

				// Token: 0x0400C341 RID: 49985
				public static LocString FABRICATOR_INGREDIENTS = "Ingredients:\n{0}";

				// Token: 0x0400C342 RID: 49986
				public static LocString ACTIVE_PARTICLE_CONSUMPTION = string.Concat(new string[]
				{
					"This building requires ",
					UI.PRE_KEYWORD,
					"Radbolts",
					UI.PST_KEYWORD,
					" to function, consuming them at a rate of {Rate} while in use"
				});

				// Token: 0x0400C343 RID: 49987
				public static LocString PARTICLE_PORT_INPUT = "A Radbolt Port on this building allows it to receive " + UI.PRE_KEYWORD + "Radbolts" + UI.PST_KEYWORD;

				// Token: 0x0400C344 RID: 49988
				public static LocString PARTICLE_PORT_OUTPUT = string.Concat(new string[]
				{
					"This building has a configurable Radbolt Port for ",
					UI.PRE_KEYWORD,
					"Radbolt",
					UI.PST_KEYWORD,
					" emission"
				});

				// Token: 0x0400C345 RID: 49989
				public static LocString IN_ORBIT_REQUIRED = "This building is only operational while its parent rocket is in flight";

				// Token: 0x0400C346 RID: 49990
				public static LocString FOOD_DEHYDRATOR_WATER_OUTPUT = string.Concat(new string[]
				{
					"This building dumps ",
					UI.PRE_KEYWORD,
					"Water",
					UI.PST_KEYWORD,
					" on the floor while in use"
				});

				// Token: 0x0400C347 RID: 49991
				public static LocString KETTLE_MELT_RATE = string.Concat(new string[]
				{
					"This building melts {0} of ",
					UI.PRE_KEYWORD,
					"Ice",
					UI.PST_KEYWORD,
					" into {0} of cold ({1}) ",
					UI.PRE_KEYWORD,
					"Water",
					UI.PST_KEYWORD,
					"\n\n",
					UI.PRE_KEYWORD,
					"Wood",
					UI.PST_KEYWORD,
					" consumption varies depending on the initial temperature of the ",
					UI.PRE_KEYWORD,
					"Ice",
					UI.PST_KEYWORD
				});
			}
		}

		// Token: 0x02002F2C RID: 12076
		public class LOGIC_PORTS
		{
			// Token: 0x0400C348 RID: 49992
			public static LocString INPUT_PORTS = UI.FormatAsLink("Auto Inputs", "LOGIC");

			// Token: 0x0400C349 RID: 49993
			public static LocString INPUT_PORTS_TOOLTIP = "Input ports change a state on this building when a signal is received";

			// Token: 0x0400C34A RID: 49994
			public static LocString OUTPUT_PORTS = UI.FormatAsLink("Auto Outputs", "LOGIC");

			// Token: 0x0400C34B RID: 49995
			public static LocString OUTPUT_PORTS_TOOLTIP = "Output ports send a signal when this building changes state";

			// Token: 0x0400C34C RID: 49996
			public static LocString INPUT_PORT_TOOLTIP = "Input Behavior:\n• {0}\n• {1}";

			// Token: 0x0400C34D RID: 49997
			public static LocString OUTPUT_PORT_TOOLTIP = "Output Behavior:\n• {0}\n• {1}";

			// Token: 0x0400C34E RID: 49998
			public static LocString CONTROL_OPERATIONAL = "Enable/Disable";

			// Token: 0x0400C34F RID: 49999
			public static LocString CONTROL_OPERATIONAL_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Enable building";

			// Token: 0x0400C350 RID: 50000
			public static LocString CONTROL_OPERATIONAL_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Disable building";

			// Token: 0x0400C351 RID: 50001
			public static LocString PORT_INPUT_DEFAULT_NAME = "INPUT";

			// Token: 0x0400C352 RID: 50002
			public static LocString PORT_OUTPUT_DEFAULT_NAME = "OUTPUT";

			// Token: 0x0400C353 RID: 50003
			public static LocString GATE_MULTI_INPUT_ONE_NAME = "INPUT A";

			// Token: 0x0400C354 RID: 50004
			public static LocString GATE_MULTI_INPUT_ONE_ACTIVE = "Green Signal";

			// Token: 0x0400C355 RID: 50005
			public static LocString GATE_MULTI_INPUT_ONE_INACTIVE = "Red Signal";

			// Token: 0x0400C356 RID: 50006
			public static LocString GATE_MULTI_INPUT_TWO_NAME = "INPUT B";

			// Token: 0x0400C357 RID: 50007
			public static LocString GATE_MULTI_INPUT_TWO_ACTIVE = "Green Signal";

			// Token: 0x0400C358 RID: 50008
			public static LocString GATE_MULTI_INPUT_TWO_INACTIVE = "Red Signal";

			// Token: 0x0400C359 RID: 50009
			public static LocString GATE_MULTI_INPUT_THREE_NAME = "INPUT C";

			// Token: 0x0400C35A RID: 50010
			public static LocString GATE_MULTI_INPUT_THREE_ACTIVE = "Green Signal";

			// Token: 0x0400C35B RID: 50011
			public static LocString GATE_MULTI_INPUT_THREE_INACTIVE = "Red Signal";

			// Token: 0x0400C35C RID: 50012
			public static LocString GATE_MULTI_INPUT_FOUR_NAME = "INPUT D";

			// Token: 0x0400C35D RID: 50013
			public static LocString GATE_MULTI_INPUT_FOUR_ACTIVE = "Green Signal";

			// Token: 0x0400C35E RID: 50014
			public static LocString GATE_MULTI_INPUT_FOUR_INACTIVE = "Red Signal";

			// Token: 0x0400C35F RID: 50015
			public static LocString GATE_SINGLE_INPUT_ONE_NAME = "INPUT";

			// Token: 0x0400C360 RID: 50016
			public static LocString GATE_SINGLE_INPUT_ONE_ACTIVE = "Green Signal";

			// Token: 0x0400C361 RID: 50017
			public static LocString GATE_SINGLE_INPUT_ONE_INACTIVE = "Red Signal";

			// Token: 0x0400C362 RID: 50018
			public static LocString GATE_MULTI_OUTPUT_ONE_NAME = "OUTPUT A";

			// Token: 0x0400C363 RID: 50019
			public static LocString GATE_MULTI_OUTPUT_ONE_ACTIVE = "Green Signal";

			// Token: 0x0400C364 RID: 50020
			public static LocString GATE_MULTI_OUTPUT_ONE_INACTIVE = "Red Signal";

			// Token: 0x0400C365 RID: 50021
			public static LocString GATE_MULTI_OUTPUT_TWO_NAME = "OUTPUT B";

			// Token: 0x0400C366 RID: 50022
			public static LocString GATE_MULTI_OUTPUT_TWO_ACTIVE = "Green Signal";

			// Token: 0x0400C367 RID: 50023
			public static LocString GATE_MULTI_OUTPUT_TWO_INACTIVE = "Red Signal";

			// Token: 0x0400C368 RID: 50024
			public static LocString GATE_MULTI_OUTPUT_THREE_NAME = "OUTPUT C";

			// Token: 0x0400C369 RID: 50025
			public static LocString GATE_MULTI_OUTPUT_THREE_ACTIVE = "Green Signal";

			// Token: 0x0400C36A RID: 50026
			public static LocString GATE_MULTI_OUTPUT_THREE_INACTIVE = "Red Signal";

			// Token: 0x0400C36B RID: 50027
			public static LocString GATE_MULTI_OUTPUT_FOUR_NAME = "OUTPUT D";

			// Token: 0x0400C36C RID: 50028
			public static LocString GATE_MULTI_OUTPUT_FOUR_ACTIVE = "Green Signal";

			// Token: 0x0400C36D RID: 50029
			public static LocString GATE_MULTI_OUTPUT_FOUR_INACTIVE = "Red Signal";

			// Token: 0x0400C36E RID: 50030
			public static LocString GATE_SINGLE_OUTPUT_ONE_NAME = "OUTPUT";

			// Token: 0x0400C36F RID: 50031
			public static LocString GATE_SINGLE_OUTPUT_ONE_ACTIVE = "Green Signal";

			// Token: 0x0400C370 RID: 50032
			public static LocString GATE_SINGLE_OUTPUT_ONE_INACTIVE = "Red Signal";

			// Token: 0x0400C371 RID: 50033
			public static LocString GATE_MULTIPLEXER_CONTROL_ONE_NAME = "CONTROL A";

			// Token: 0x0400C372 RID: 50034
			public static LocString GATE_MULTIPLEXER_CONTROL_ONE_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Set signal path to <b>down</b> position";

			// Token: 0x0400C373 RID: 50035
			public static LocString GATE_MULTIPLEXER_CONTROL_ONE_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Set signal path to <b>up</b> position";

			// Token: 0x0400C374 RID: 50036
			public static LocString GATE_MULTIPLEXER_CONTROL_TWO_NAME = "CONTROL B";

			// Token: 0x0400C375 RID: 50037
			public static LocString GATE_MULTIPLEXER_CONTROL_TWO_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Set signal path to <b>down</b> position";

			// Token: 0x0400C376 RID: 50038
			public static LocString GATE_MULTIPLEXER_CONTROL_TWO_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Set signal path to <b>up</b> position";
		}

		// Token: 0x02002F2D RID: 12077
		public class GAMEOBJECTEFFECTS
		{
			// Token: 0x0400C377 RID: 50039
			public static LocString CALORIES = "+{0}";

			// Token: 0x0400C378 RID: 50040
			public static LocString FOOD_QUALITY = "Quality: {0}";

			// Token: 0x0400C379 RID: 50041
			public static LocString FOOD_MORALE = "Morale: {0}";

			// Token: 0x0400C37A RID: 50042
			public static LocString FORGAVEATTACKER = "Forgiveness";

			// Token: 0x0400C37B RID: 50043
			public static LocString COLDBREATHER = UI.FormatAsLink("Cooling Effect", "HEAT");

			// Token: 0x0400C37C RID: 50044
			public static LocString LIFECYCLETITLE = "Growth:";

			// Token: 0x0400C37D RID: 50045
			public static LocString GROWTHTIME_SIMPLE = "Life Cycle: {0}";

			// Token: 0x0400C37E RID: 50046
			public static LocString GROWTHTIME_REGROWTH = "Domestic growth: {0} / {1}";

			// Token: 0x0400C37F RID: 50047
			public static LocString GROWTHTIME = "Growth: {0}";

			// Token: 0x0400C380 RID: 50048
			public static LocString INITIALGROWTHTIME = "Initial Growth: {0}";

			// Token: 0x0400C381 RID: 50049
			public static LocString REGROWTHTIME = "Regrowth: {0}";

			// Token: 0x0400C382 RID: 50050
			public static LocString REQUIRES_LIGHT = UI.FormatAsLink("Light", "LIGHT") + ": {Lux}";

			// Token: 0x0400C383 RID: 50051
			public static LocString REQUIRES_DARKNESS = UI.FormatAsLink("Darkness", "LIGHT");

			// Token: 0x0400C384 RID: 50052
			public static LocString REQUIRESFERTILIZER = "{0}: {1}";

			// Token: 0x0400C385 RID: 50053
			public static LocString IDEAL_FERTILIZER = "{0}: {1}";

			// Token: 0x0400C386 RID: 50054
			public static LocString EQUIPMENT_MODS = "{Attribute} {Value}";

			// Token: 0x0400C387 RID: 50055
			public static LocString ROTTEN = "Rotten";

			// Token: 0x0400C388 RID: 50056
			public static LocString REQUIRES_ATMOSPHERE = UI.FormatAsLink("Atmosphere", "ATMOSPHERE") + ": {0}";

			// Token: 0x0400C389 RID: 50057
			public static LocString REQUIRES_PRESSURE = UI.FormatAsLink("Air", "ATMOSPHERE") + " Pressure: {0} minimum";

			// Token: 0x0400C38A RID: 50058
			public static LocString IDEAL_PRESSURE = UI.FormatAsLink("Air", "ATMOSPHERE") + " Pressure: {0}";

			// Token: 0x0400C38B RID: 50059
			public static LocString REQUIRES_TEMPERATURE = UI.FormatAsLink("Temperature", "HEAT") + ": {0} to {1}";

			// Token: 0x0400C38C RID: 50060
			public static LocString IDEAL_TEMPERATURE = UI.FormatAsLink("Temperature", "HEAT") + ": {0} to {1}";

			// Token: 0x0400C38D RID: 50061
			public static LocString REQUIRES_SUBMERSION = UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " Submersion";

			// Token: 0x0400C38E RID: 50062
			public static LocString FOOD_EFFECTS = "Effects:";

			// Token: 0x0400C38F RID: 50063
			public static LocString EMITS_LIGHT = UI.FormatAsLink("Light Range", "LIGHT") + ": {0} tiles";

			// Token: 0x0400C390 RID: 50064
			public static LocString EMITS_LIGHT_LUX = UI.FormatAsLink("Brightness", "LIGHT") + ": {0} Lux";

			// Token: 0x0400C391 RID: 50065
			public static LocString AMBIENT_RADIATION = "Ambient Radiation";

			// Token: 0x0400C392 RID: 50066
			public static LocString AMBIENT_RADIATION_FMT = "{minRads} - {maxRads}";

			// Token: 0x0400C393 RID: 50067
			public static LocString AMBIENT_NO_MIN_RADIATION_FMT = "Less than {maxRads}";

			// Token: 0x0400C394 RID: 50068
			public static LocString REQUIRES_NO_MIN_RADIATION = "Maximum " + UI.FormatAsLink("Radiation", "RADIATION") + ": {MaxRads}";

			// Token: 0x0400C395 RID: 50069
			public static LocString REQUIRES_RADIATION = UI.FormatAsLink("Radiation", "RADIATION") + ": {MinRads} to {MaxRads}";

			// Token: 0x0400C396 RID: 50070
			public static LocString MUTANT_STERILE = "Doesn't Drop " + UI.FormatAsLink("Seeds", "PLANTS");

			// Token: 0x0400C397 RID: 50071
			public static LocString DARKNESS = "Darkness";

			// Token: 0x0400C398 RID: 50072
			public static LocString LIGHT = "Light";

			// Token: 0x0400C399 RID: 50073
			public static LocString SEED_PRODUCTION_DIG_ONLY = "Consumes 1 " + UI.FormatAsLink("Seed", "PLANTS");

			// Token: 0x0400C39A RID: 50074
			public static LocString SEED_PRODUCTION_HARVEST = "Harvest yields " + UI.FormatAsLink("Seeds", "PLANTS");

			// Token: 0x0400C39B RID: 50075
			public static LocString SEED_PRODUCTION_FINAL_HARVEST = "Final harvest yields " + UI.FormatAsLink("Seeds", "PLANTS");

			// Token: 0x0400C39C RID: 50076
			public static LocString SEED_PRODUCTION_FRUIT = "Fruit produces " + UI.FormatAsLink("Seeds", "PLANTS");

			// Token: 0x0400C39D RID: 50077
			public static LocString SEED_REQUIREMENT_CEILING = "Plot Orientation: Downward";

			// Token: 0x0400C39E RID: 50078
			public static LocString SEED_REQUIREMENT_WALL = "Plot Orientation: Sideways";

			// Token: 0x0400C39F RID: 50079
			public static LocString REQUIRES_RECEPTACLE = "Farm Plot";

			// Token: 0x0400C3A0 RID: 50080
			public static LocString PLANT_MARK_FOR_HARVEST = "Autoharvest Enabled";

			// Token: 0x0400C3A1 RID: 50081
			public static LocString PLANT_DO_NOT_HARVEST = "Autoharvest Disabled";

			// Token: 0x02002F2E RID: 12078
			public class INSULATED
			{
				// Token: 0x0400C3A2 RID: 50082
				public static LocString NAME = "Insulated";

				// Token: 0x0400C3A3 RID: 50083
				public static LocString TOOLTIP = "Proper insulation drastically reduces thermal conductivity";
			}

			// Token: 0x02002F2F RID: 12079
			public class TOOLTIPS
			{
				// Token: 0x0400C3A4 RID: 50084
				public static LocString CALORIES = "+{0}";

				// Token: 0x0400C3A5 RID: 50085
				public static LocString FOOD_QUALITY = "Quality: {0}";

				// Token: 0x0400C3A6 RID: 50086
				public static LocString FOOD_MORALE = "Morale: {0}";

				// Token: 0x0400C3A7 RID: 50087
				public static LocString COLDBREATHER = "Lowers ambient air temperature";

				// Token: 0x0400C3A8 RID: 50088
				public static LocString GROWTHTIME_SIMPLE = "This plant takes <b>{0}</b> to grow";

				// Token: 0x0400C3A9 RID: 50089
				public static LocString GROWTHTIME_REGROWTH = "This plant initially takes <b>{0}</b> to grow, but only <b>{1}</b> to mature after first harvest";

				// Token: 0x0400C3AA RID: 50090
				public static LocString GROWTHTIME = "This plant takes <b>{0}</b> to grow";

				// Token: 0x0400C3AB RID: 50091
				public static LocString INITIALGROWTHTIME = "This plant takes <b>{0}</b> to mature again once replanted";

				// Token: 0x0400C3AC RID: 50092
				public static LocString REGROWTHTIME = "This plant takes <b>{0}</b> to mature again once harvested";

				// Token: 0x0400C3AD RID: 50093
				public static LocString EQUIPMENT_MODS = "{Attribute} {Value}";

				// Token: 0x0400C3AE RID: 50094
				public static LocString REQUIRESFERTILIZER = string.Concat(new string[]
				{
					"This plant requires <b>{1}</b> ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" for basic growth"
				});

				// Token: 0x0400C3AF RID: 50095
				public static LocString IDEAL_FERTILIZER = string.Concat(new string[]
				{
					"This plant requires <b>{1}</b> of ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" for basic growth"
				});

				// Token: 0x0400C3B0 RID: 50096
				public static LocString REQUIRES_LIGHT = string.Concat(new string[]
				{
					"This plant requires a ",
					UI.PRE_KEYWORD,
					"Light",
					UI.PST_KEYWORD,
					" source bathing it in at least {Lux}"
				});

				// Token: 0x0400C3B1 RID: 50097
				public static LocString REQUIRES_DARKNESS = "This plant requires complete darkness";

				// Token: 0x0400C3B2 RID: 50098
				public static LocString REQUIRES_ATMOSPHERE = "This plant must be submerged in one of the following gases: {0}";

				// Token: 0x0400C3B3 RID: 50099
				public static LocString REQUIRES_ATMOSPHERE_LIQUID = "This plant must be submerged in one of the following liquids: {0}";

				// Token: 0x0400C3B4 RID: 50100
				public static LocString REQUIRES_ATMOSPHERE_MIXED = "This plant must be submerged in one of the following gases or liquids: {0}";

				// Token: 0x0400C3B5 RID: 50101
				public static LocString REQUIRES_PRESSURE = string.Concat(new string[]
				{
					"Ambient ",
					UI.PRE_KEYWORD,
					"Gas",
					UI.PST_KEYWORD,
					" pressure must be at least <b>{0}</b> for basic growth"
				});

				// Token: 0x0400C3B6 RID: 50102
				public static LocString IDEAL_PRESSURE = string.Concat(new string[]
				{
					"This plant requires ",
					UI.PRE_KEYWORD,
					"Gas",
					UI.PST_KEYWORD,
					" pressures above <b>{0}</b> for basic growth"
				});

				// Token: 0x0400C3B7 RID: 50103
				public static LocString REQUIRES_TEMPERATURE = string.Concat(new string[]
				{
					"Internal ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" must be between <b>{0}</b> and <b>{1}</b> for basic growth"
				});

				// Token: 0x0400C3B8 RID: 50104
				public static LocString IDEAL_TEMPERATURE = string.Concat(new string[]
				{
					"This plant requires internal ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" between <b>{0}</b> and <b>{1}</b> for basic growth"
				});

				// Token: 0x0400C3B9 RID: 50105
				public static LocString REQUIRES_SUBMERSION = string.Concat(new string[]
				{
					"This plant must be fully submerged in ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					" for basic growth"
				});

				// Token: 0x0400C3BA RID: 50106
				public static LocString FOOD_EFFECTS = "Duplicants will gain the following effects from eating this food: {0}";

				// Token: 0x0400C3BB RID: 50107
				public static LocString REQUIRES_RECEPTACLE = string.Concat(new string[]
				{
					"This plant must be housed in a ",
					UI.FormatAsLink("Planter Box", "PLANTERBOX"),
					", ",
					UI.FormatAsLink("Farm Tile", "FARMTILE"),
					", or ",
					UI.FormatAsLink("Hydroponic Farm", "HYDROPONICFARM"),
					" farm to grow domestically"
				});

				// Token: 0x0400C3BC RID: 50108
				public static LocString EMITS_LIGHT = string.Concat(new string[]
				{
					"Emits ",
					UI.PRE_KEYWORD,
					"Light",
					UI.PST_KEYWORD,
					"\n\nDuplicants can operate buildings more quickly when they're well lit"
				});

				// Token: 0x0400C3BD RID: 50109
				public static LocString EMITS_LIGHT_LUX = string.Concat(new string[]
				{
					"Emits ",
					UI.PRE_KEYWORD,
					"Light",
					UI.PST_KEYWORD,
					"\n\nDuplicants can operate buildings more quickly when they're well lit"
				});

				// Token: 0x0400C3BE RID: 50110
				public static LocString METEOR_SHOWER_SINGLE_METEOR_PERCENTAGE_TOOLTIP = "Distribution of meteor types in this shower";

				// Token: 0x0400C3BF RID: 50111
				public static LocString SEED_PRODUCTION_DIG_ONLY = "May be replanted, but will produce no further " + UI.PRE_KEYWORD + "Seeds" + UI.PST_KEYWORD;

				// Token: 0x0400C3C0 RID: 50112
				public static LocString SEED_PRODUCTION_HARVEST = "Harvesting this plant will yield new " + UI.PRE_KEYWORD + "Seeds" + UI.PST_KEYWORD;

				// Token: 0x0400C3C1 RID: 50113
				public static LocString SEED_PRODUCTION_FINAL_HARVEST = string.Concat(new string[]
				{
					"Yields new ",
					UI.PRE_KEYWORD,
					"Seeds",
					UI.PST_KEYWORD,
					" on the final harvest of its life cycle"
				});

				// Token: 0x0400C3C2 RID: 50114
				public static LocString SEED_PRODUCTION_FRUIT = "Consuming this plant's fruit will yield new " + UI.PRE_KEYWORD + "Seeds" + UI.PST_KEYWORD;

				// Token: 0x0400C3C3 RID: 50115
				public static LocString SEED_REQUIREMENT_CEILING = "This seed must be planted in a downward facing plot\n\nPress " + UI.FormatAsKeyWord("[O]") + " while building farm plots to rotate them";

				// Token: 0x0400C3C4 RID: 50116
				public static LocString SEED_REQUIREMENT_WALL = "This seed must be planted in a side facing plot\n\nPress " + UI.FormatAsKeyWord("[O]") + " while building farm plots to rotate them";

				// Token: 0x0400C3C5 RID: 50117
				public static LocString REQUIRES_NO_MIN_RADIATION = "This plant will stop growing if exposed to more than {MaxRads} of " + UI.FormatAsLink("Radiation", "RADIATION");

				// Token: 0x0400C3C6 RID: 50118
				public static LocString REQUIRES_RADIATION = "This plant will only grow if it has between {MinRads} and {MaxRads} of " + UI.FormatAsLink("Radiation", "RADIATION");

				// Token: 0x0400C3C7 RID: 50119
				public static LocString MUTANT_SEED_TOOLTIP = "\n\nGrowing near its maximum radiation increases the chance of mutant seeds being produced";

				// Token: 0x0400C3C8 RID: 50120
				public static LocString MUTANT_STERILE = "This plant will not produce seeds of its own due to changes to its DNA";
			}

			// Token: 0x02002F30 RID: 12080
			public class DAMAGE_POPS
			{
				// Token: 0x0400C3C9 RID: 50121
				public static LocString OVERHEAT = "Overheat Damage";

				// Token: 0x0400C3CA RID: 50122
				public static LocString CORROSIVE_ELEMENT = "Corrosive Element Damage";

				// Token: 0x0400C3CB RID: 50123
				public static LocString WRONG_ELEMENT = "Wrong Element Damage";

				// Token: 0x0400C3CC RID: 50124
				public static LocString CIRCUIT_OVERLOADED = "Overload Damage";

				// Token: 0x0400C3CD RID: 50125
				public static LocString LOGIC_CIRCUIT_OVERLOADED = "Signal Overload Damage";

				// Token: 0x0400C3CE RID: 50126
				public static LocString LIQUID_PRESSURE = "Pressure Damage";

				// Token: 0x0400C3CF RID: 50127
				public static LocString MINION_DESTRUCTION = "Tantrum Damage";

				// Token: 0x0400C3D0 RID: 50128
				public static LocString CONDUIT_CONTENTS_FROZE = "Cold Damage";

				// Token: 0x0400C3D1 RID: 50129
				public static LocString CONDUIT_CONTENTS_BOILED = "Heat Damage";

				// Token: 0x0400C3D2 RID: 50130
				public static LocString MICROMETEORITE = "Micrometeorite Damage";

				// Token: 0x0400C3D3 RID: 50131
				public static LocString COMET = "Meteor Damage";

				// Token: 0x0400C3D4 RID: 50132
				public static LocString ROCKET = "Rocket Thruster Damage";
			}
		}

		// Token: 0x02002F31 RID: 12081
		public class ASTEROIDCLOCK
		{
			// Token: 0x0400C3D5 RID: 50133
			public static LocString CYCLE = "Cycle";

			// Token: 0x0400C3D6 RID: 50134
			public static LocString CYCLES_OLD = "This Colony is {0} Cycle(s) Old";

			// Token: 0x0400C3D7 RID: 50135
			public static LocString TIME_PLAYED = "Time Played: {0} hours";

			// Token: 0x0400C3D8 RID: 50136
			public static LocString SCHEDULE_BUTTON_TOOLTIP = "Manage Schedule";

			// Token: 0x0400C3D9 RID: 50137
			public static LocString MILESTONE_TITLE = "Approaching Milestone";

			// Token: 0x0400C3DA RID: 50138
			public static LocString MILESTONE_DESCRIPTION = "This colony is about to hit Cycle {0}!";
		}

		// Token: 0x02002F32 RID: 12082
		public class ENDOFDAYREPORT
		{
			// Token: 0x0400C3DB RID: 50139
			public static LocString REPORT_TITLE = "DAILY REPORTS";

			// Token: 0x0400C3DC RID: 50140
			public static LocString DAY_TITLE = "Cycle {0}";

			// Token: 0x0400C3DD RID: 50141
			public static LocString DAY_TITLE_TODAY = "Cycle {0} - Today";

			// Token: 0x0400C3DE RID: 50142
			public static LocString DAY_TITLE_YESTERDAY = "Cycle {0} - Yesterday";

			// Token: 0x0400C3DF RID: 50143
			public static LocString NOTIFICATION_TITLE = "Cycle {0} report ready";

			// Token: 0x0400C3E0 RID: 50144
			public static LocString NOTIFICATION_TOOLTIP = "The daily report for Cycle {0} is ready to view";

			// Token: 0x0400C3E1 RID: 50145
			public static LocString NEXT = "Next";

			// Token: 0x0400C3E2 RID: 50146
			public static LocString PREV = "Prev";

			// Token: 0x0400C3E3 RID: 50147
			public static LocString ADDED = "Added";

			// Token: 0x0400C3E4 RID: 50148
			public static LocString REMOVED = "Removed";

			// Token: 0x0400C3E5 RID: 50149
			public static LocString NET = "Net";

			// Token: 0x0400C3E6 RID: 50150
			public static LocString DUPLICANT_DETAILS_HEADER = "Duplicant Details:";

			// Token: 0x0400C3E7 RID: 50151
			public static LocString TIME_DETAILS_HEADER = "Total Time Details:";

			// Token: 0x0400C3E8 RID: 50152
			public static LocString BASE_DETAILS_HEADER = "Base Details:";

			// Token: 0x0400C3E9 RID: 50153
			public static LocString AVERAGE_TIME_DETAILS_HEADER = "Average Time Details:";

			// Token: 0x0400C3EA RID: 50154
			public static LocString MY_COLONY = "my colony";

			// Token: 0x0400C3EB RID: 50155
			public static LocString NONE = "None";

			// Token: 0x02002F33 RID: 12083
			public class OXYGEN_CREATED
			{
				// Token: 0x0400C3EC RID: 50156
				public static LocString NAME = UI.FormatAsLink("Oxygen", "OXYGEN") + " Generation:";

				// Token: 0x0400C3ED RID: 50157
				public static LocString POSITIVE_TOOLTIP = "{0} of " + UI.FormatAsLink("Oxygen", "OXYGEN") + " was produced by {1} over the course of the day";

				// Token: 0x0400C3EE RID: 50158
				public static LocString NEGATIVE_TOOLTIP = "{0} of " + UI.FormatAsLink("Oxygen", "OXYGEN") + " was consumed by {1} over the course of the day";
			}

			// Token: 0x02002F34 RID: 12084
			public class CALORIES_CREATED
			{
				// Token: 0x0400C3EF RID: 50159
				public static LocString NAME = "Calorie Generation:";

				// Token: 0x0400C3F0 RID: 50160
				public static LocString POSITIVE_TOOLTIP = "{0} of " + UI.FormatAsLink("Food", "FOOD") + " was produced by {1} over the course of the day";

				// Token: 0x0400C3F1 RID: 50161
				public static LocString NEGATIVE_TOOLTIP = "{0} of " + UI.FormatAsLink("Food", "FOOD") + " was consumed by {1} over the course of the day";
			}

			// Token: 0x02002F35 RID: 12085
			public class NUMBER_OF_DOMESTICATED_CRITTERS
			{
				// Token: 0x0400C3F2 RID: 50162
				public static LocString NAME = "Domesticated Critters:";

				// Token: 0x0400C3F3 RID: 50163
				public static LocString POSITIVE_TOOLTIP = "{0} domestic critters live in {1}";

				// Token: 0x0400C3F4 RID: 50164
				public static LocString NEGATIVE_TOOLTIP = "{0} domestic critters live in {1}";
			}

			// Token: 0x02002F36 RID: 12086
			public class NUMBER_OF_WILD_CRITTERS
			{
				// Token: 0x0400C3F5 RID: 50165
				public static LocString NAME = "Wild Critters:";

				// Token: 0x0400C3F6 RID: 50166
				public static LocString POSITIVE_TOOLTIP = "{0} wild critters live in {1}";

				// Token: 0x0400C3F7 RID: 50167
				public static LocString NEGATIVE_TOOLTIP = "{0} wild critters live in {1}";
			}

			// Token: 0x02002F37 RID: 12087
			public class ROCKETS_IN_FLIGHT
			{
				// Token: 0x0400C3F8 RID: 50168
				public static LocString NAME = "Rocket Missions Underway:";

				// Token: 0x0400C3F9 RID: 50169
				public static LocString POSITIVE_TOOLTIP = "{0} rockets are currently flying missions for {1}";

				// Token: 0x0400C3FA RID: 50170
				public static LocString NEGATIVE_TOOLTIP = "{0} rockets are currently flying missions for {1}";
			}

			// Token: 0x02002F38 RID: 12088
			public class STRESS_DELTA
			{
				// Token: 0x0400C3FB RID: 50171
				public static LocString NAME = UI.FormatAsLink("Stress", "STRESS") + " Change:";

				// Token: 0x0400C3FC RID: 50172
				public static LocString POSITIVE_TOOLTIP = UI.FormatAsLink("Stress", "STRESS") + " increased by a total of {0} for {1}";

				// Token: 0x0400C3FD RID: 50173
				public static LocString NEGATIVE_TOOLTIP = UI.FormatAsLink("Stress", "STRESS") + " decreased by a total of {0} for {1}";
			}

			// Token: 0x02002F39 RID: 12089
			public class TRAVELTIMEWARNING
			{
				// Token: 0x0400C3FE RID: 50174
				public static LocString WARNING_TITLE = "Long Commutes";

				// Token: 0x0400C3FF RID: 50175
				public static LocString WARNING_MESSAGE = "My Duplicants are spending a significant amount of time traveling between their errands (> {0})";
			}

			// Token: 0x02002F3A RID: 12090
			public class TRAVEL_TIME
			{
				// Token: 0x0400C400 RID: 50176
				public static LocString NAME = "Travel Time:";

				// Token: 0x0400C401 RID: 50177
				public static LocString POSITIVE_TOOLTIP = "On average, {1} spent {0} of their time traveling between tasks";
			}

			// Token: 0x02002F3B RID: 12091
			public class WORK_TIME
			{
				// Token: 0x0400C402 RID: 50178
				public static LocString NAME = "Work Time:";

				// Token: 0x0400C403 RID: 50179
				public static LocString POSITIVE_TOOLTIP = "On average, {0} of {1}'s time was spent working";
			}

			// Token: 0x02002F3C RID: 12092
			public class IDLE_TIME
			{
				// Token: 0x0400C404 RID: 50180
				public static LocString NAME = "Idle Time:";

				// Token: 0x0400C405 RID: 50181
				public static LocString POSITIVE_TOOLTIP = "On average, {0} of {1}'s time was spent idling";
			}

			// Token: 0x02002F3D RID: 12093
			public class PERSONAL_TIME
			{
				// Token: 0x0400C406 RID: 50182
				public static LocString NAME = "Personal Time:";

				// Token: 0x0400C407 RID: 50183
				public static LocString POSITIVE_TOOLTIP = "On average, {0} of {1}'s time was spent tending to personal needs";
			}

			// Token: 0x02002F3E RID: 12094
			public class ENERGY_USAGE
			{
				// Token: 0x0400C408 RID: 50184
				public static LocString NAME = UI.FormatAsLink("Power", "POWER") + " Usage:";

				// Token: 0x0400C409 RID: 50185
				public static LocString POSITIVE_TOOLTIP = "{0} of " + UI.FormatAsLink("Power", "POWER") + " was created by {1} over the course of the day";

				// Token: 0x0400C40A RID: 50186
				public static LocString NEGATIVE_TOOLTIP = "{0} of " + UI.FormatAsLink("Power", "POWER") + " was consumed by {1} over the course of the day";
			}

			// Token: 0x02002F3F RID: 12095
			public class ENERGY_WASTED
			{
				// Token: 0x0400C40B RID: 50187
				public static LocString NAME = UI.FormatAsLink("Power", "POWER") + " Wasted:";

				// Token: 0x0400C40C RID: 50188
				public static LocString NEGATIVE_TOOLTIP = "{0} of " + UI.FormatAsLink("Power", "POWER") + " was lost today due to battery runoff and overproduction in {1}";
			}

			// Token: 0x02002F40 RID: 12096
			public class LEVEL_UP
			{
				// Token: 0x0400C40D RID: 50189
				public static LocString NAME = "Skill Increases:";

				// Token: 0x0400C40E RID: 50190
				public static LocString TOOLTIP = "Today {1} gained a total of {0} skill levels";
			}

			// Token: 0x02002F41 RID: 12097
			public class TOILET_INCIDENT
			{
				// Token: 0x0400C40F RID: 50191
				public static LocString NAME = "Restroom Accidents:";

				// Token: 0x0400C410 RID: 50192
				public static LocString TOOLTIP = "{0} Duplicants couldn't quite reach the toilet in time today";
			}

			// Token: 0x02002F42 RID: 12098
			public class DISEASE_ADDED
			{
				// Token: 0x0400C411 RID: 50193
				public static LocString NAME = UI.FormatAsLink("Diseases", "DISEASE") + " Contracted:";

				// Token: 0x0400C412 RID: 50194
				public static LocString POSITIVE_TOOLTIP = "{0} " + UI.FormatAsLink("Disease", "DISEASE") + " were contracted by {1}";

				// Token: 0x0400C413 RID: 50195
				public static LocString NEGATIVE_TOOLTIP = "{0} " + UI.FormatAsLink("Disease", "DISEASE") + " were cured by {1}";
			}

			// Token: 0x02002F43 RID: 12099
			public class CONTAMINATED_OXYGEN_FLATULENCE
			{
				// Token: 0x0400C414 RID: 50196
				public static LocString NAME = UI.FormatAsLink("Flatulence", "CONTAMINATEDOXYGEN") + " Generation:";

				// Token: 0x0400C415 RID: 50197
				public static LocString POSITIVE_TOOLTIP = "{0} of " + UI.FormatAsLink("Polluted Oxygen", "CONTAMINATEDOXYGEN") + " was generated by {1} over the course of the day";

				// Token: 0x0400C416 RID: 50198
				public static LocString NEGATIVE_TOOLTIP = "{0} of " + UI.FormatAsLink("Polluted Oxygen", "CONTAMINATEDOXYGEN") + " was consumed by {1} over the course of the day";
			}

			// Token: 0x02002F44 RID: 12100
			public class CONTAMINATED_OXYGEN_TOILET
			{
				// Token: 0x0400C417 RID: 50199
				public static LocString NAME = UI.FormatAsLink("Toilet Emissions: ", "CONTAMINATEDOXYGEN");

				// Token: 0x0400C418 RID: 50200
				public static LocString POSITIVE_TOOLTIP = "{0} of " + UI.FormatAsLink("Polluted Oxygen", "CONTAMINATEDOXYGEN") + " was generated by {1} over the course of the day";

				// Token: 0x0400C419 RID: 50201
				public static LocString NEGATIVE_TOOLTIP = "{0} of " + UI.FormatAsLink("Polluted Oxygen", "CONTAMINATEDOXYGEN") + " was consumed by {1} over the course of the day";
			}

			// Token: 0x02002F45 RID: 12101
			public class CONTAMINATED_OXYGEN_SUBLIMATION
			{
				// Token: 0x0400C41A RID: 50202
				public static LocString NAME = UI.FormatAsLink("Sublimation", "CONTAMINATEDOXYGEN") + ":";

				// Token: 0x0400C41B RID: 50203
				public static LocString POSITIVE_TOOLTIP = "{0} of " + UI.FormatAsLink("Polluted Oxygen", "CONTAMINATEDOXYGEN") + " was generated by {1} over the course of the day";

				// Token: 0x0400C41C RID: 50204
				public static LocString NEGATIVE_TOOLTIP = "{0} of " + UI.FormatAsLink("Polluted Oxygen", "CONTAMINATEDOXYGEN") + " was consumed by {1} over the course of the day";
			}

			// Token: 0x02002F46 RID: 12102
			public class DISEASE_STATUS
			{
				// Token: 0x0400C41D RID: 50205
				public static LocString NAME = "Disease Status:";

				// Token: 0x0400C41E RID: 50206
				public static LocString TOOLTIP = "There are {0} covering {1}";
			}

			// Token: 0x02002F47 RID: 12103
			public class CHORE_STATUS
			{
				// Token: 0x0400C41F RID: 50207
				public static LocString NAME = "Errands:";

				// Token: 0x0400C420 RID: 50208
				public static LocString POSITIVE_TOOLTIP = "{0} errands are queued for {1}";

				// Token: 0x0400C421 RID: 50209
				public static LocString NEGATIVE_TOOLTIP = "{0} errands were completed over the course of the day by {1}";
			}

			// Token: 0x02002F48 RID: 12104
			public class NOTES
			{
				// Token: 0x0400C422 RID: 50210
				public static LocString NOTE_ENTRY_LINE_ITEM = "{0}\n{1}: {2}";

				// Token: 0x0400C423 RID: 50211
				public static LocString BUTCHERED = "Butchered for {0}";

				// Token: 0x0400C424 RID: 50212
				public static LocString BUTCHERED_CONTEXT = "Butchered";

				// Token: 0x0400C425 RID: 50213
				public static LocString CRAFTED = "Crafted a {0}";

				// Token: 0x0400C426 RID: 50214
				public static LocString CRAFTED_USED = "{0} used as ingredient";

				// Token: 0x0400C427 RID: 50215
				public static LocString CRAFTED_CONTEXT = "Crafted";

				// Token: 0x0400C428 RID: 50216
				public static LocString HARVESTED = "Harvested {0}";

				// Token: 0x0400C429 RID: 50217
				public static LocString HARVESTED_CONTEXT = "Harvested";

				// Token: 0x0400C42A RID: 50218
				public static LocString EATEN = "{0} eaten";

				// Token: 0x0400C42B RID: 50219
				public static LocString ROTTED = "Rotten {0}";

				// Token: 0x0400C42C RID: 50220
				public static LocString ROTTED_CONTEXT = "Rotted";

				// Token: 0x0400C42D RID: 50221
				public static LocString GERMS = "On {0}";

				// Token: 0x0400C42E RID: 50222
				public static LocString TIME_SPENT = "{0}";

				// Token: 0x0400C42F RID: 50223
				public static LocString WORK_TIME = "{0}";

				// Token: 0x0400C430 RID: 50224
				public static LocString PERSONAL_TIME = "{0}";

				// Token: 0x0400C431 RID: 50225
				public static LocString FOODFIGHT_CONTEXT = "{0} ingested in food fight";
			}
		}

		// Token: 0x02002F49 RID: 12105
		public static class SCHEDULEBLOCKTYPES
		{
			// Token: 0x02002F4A RID: 12106
			public static class EAT
			{
				// Token: 0x0400C432 RID: 50226
				public static LocString NAME = "Mealtime";

				// Token: 0x0400C433 RID: 50227
				public static LocString DESCRIPTION = "EAT:\nDuring Mealtime Duplicants will head to their assigned mess halls and eat.";
			}

			// Token: 0x02002F4B RID: 12107
			public static class SLEEP
			{
				// Token: 0x0400C434 RID: 50228
				public static LocString NAME = "Sleep";

				// Token: 0x0400C435 RID: 50229
				public static LocString DESCRIPTION = "SLEEP:\nWhen it's time to sleep, Duplicants will head to their assigned rooms and rest.";
			}

			// Token: 0x02002F4C RID: 12108
			public static class WORK
			{
				// Token: 0x0400C436 RID: 50230
				public static LocString NAME = "Work";

				// Token: 0x0400C437 RID: 50231
				public static LocString DESCRIPTION = "WORK:\nDuring Work hours Duplicants will perform any pending errands in the colony.";
			}

			// Token: 0x02002F4D RID: 12109
			public static class RECREATION
			{
				// Token: 0x0400C438 RID: 50232
				public static LocString NAME = "Recreation";

				// Token: 0x0400C439 RID: 50233
				public static LocString DESCRIPTION = "HAMMER TIME:\nDuring Hammer Time, Duplicants will relieve their " + UI.FormatAsLink("Stress", "STRESS") + " through dance. Please be aware that no matter how hard my Duplicants try, they will absolutely not be able to touch this.";
			}

			// Token: 0x02002F4E RID: 12110
			public static class HYGIENE
			{
				// Token: 0x0400C43A RID: 50234
				public static LocString NAME = "Hygiene";

				// Token: 0x0400C43B RID: 50235
				public static LocString DESCRIPTION = "HYGIENE:\nDuring " + UI.FormatAsLink("Hygiene", "HYGIENE") + " hours Duplicants will head to their assigned washrooms to get cleaned up.";
			}
		}

		// Token: 0x02002F4F RID: 12111
		public static class SCHEDULEGROUPS
		{
			// Token: 0x0400C43C RID: 50236
			public static LocString TOOLTIP_FORMAT = "{0}\n\n{1}";

			// Token: 0x0400C43D RID: 50237
			public static LocString MISSINGBLOCKS = "Warning: Scheduling Issues ({0})";

			// Token: 0x0400C43E RID: 50238
			public static LocString NOTIME = "No {0} shifts allotted";

			// Token: 0x02002F50 RID: 12112
			public static class HYGENE
			{
				// Token: 0x0400C43F RID: 50239
				public static LocString NAME = "Bathtime";

				// Token: 0x0400C440 RID: 50240
				public static LocString DESCRIPTION = "During Bathtime shifts my Duplicants will take care of their hygienic needs, such as going to the bathroom, using the shower or washing their hands.\n\nOnce they're all caught up on personal hygiene, Duplicants will head back to work.";

				// Token: 0x0400C441 RID: 50241
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"During ",
					UI.PRE_KEYWORD,
					"Bathtime",
					UI.PST_KEYWORD,
					" shifts my Duplicants will take care of their hygienic needs, such as going to the bathroom, using the shower or washing their hands."
				});
			}

			// Token: 0x02002F51 RID: 12113
			public static class WORKTIME
			{
				// Token: 0x0400C442 RID: 50242
				public static LocString NAME = "Work";

				// Token: 0x0400C443 RID: 50243
				public static LocString DESCRIPTION = "During Work shifts my Duplicants must perform the errands I have placed for them throughout the colony.\n\nIt's important when scheduling to maintain a good work-life balance for my Duplicants to maintain their health and prevent Morale loss.";

				// Token: 0x0400C444 RID: 50244
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"During ",
					UI.PRE_KEYWORD,
					"Work",
					UI.PST_KEYWORD,
					" shifts my Duplicants must perform the errands I've placed for them throughout the colony."
				});
			}

			// Token: 0x02002F52 RID: 12114
			public static class RECREATION
			{
				// Token: 0x0400C445 RID: 50245
				public static LocString NAME = "Downtime";

				// Token: 0x0400C446 RID: 50246
				public static LocString DESCRIPTION = "During Downtime my Duplicants they may do as they please.\n\nThis may include personal matters like bathroom visits or snacking, or they may choose to engage in leisure activities like socializing with friends.\n\nDowntime increases Duplicant Morale.";

				// Token: 0x0400C447 RID: 50247
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"During ",
					UI.PRE_KEYWORD,
					"Downtime",
					UI.PST_KEYWORD,
					" shifts my Duplicants they may do as they please."
				});
			}

			// Token: 0x02002F53 RID: 12115
			public static class SLEEP
			{
				// Token: 0x0400C448 RID: 50248
				public static LocString NAME = "Bedtime";

				// Token: 0x0400C449 RID: 50249
				public static LocString DESCRIPTION = "My Duplicants use Bedtime shifts to rest up after a hard day's work.\n\nScheduling too few bedtime shifts may prevent my Duplicants from regaining enough Stamina to make it through the following day.";

				// Token: 0x0400C44A RID: 50250
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"My Duplicants use ",
					UI.PRE_KEYWORD,
					"Bedtime",
					UI.PST_KEYWORD,
					" shifts to rest up after a hard day's work."
				});
			}
		}

		// Token: 0x02002F54 RID: 12116
		public class ELEMENTAL
		{
			// Token: 0x02002F55 RID: 12117
			public class AGE
			{
				// Token: 0x0400C44B RID: 50251
				public static LocString NAME = "Age: {0}";

				// Token: 0x0400C44C RID: 50252
				public static LocString TOOLTIP = "The selected object is {0} cycles old";

				// Token: 0x0400C44D RID: 50253
				public static LocString UNKNOWN = "Unknown";

				// Token: 0x0400C44E RID: 50254
				public static LocString UNKNOWN_TOOLTIP = "The age of the selected object is unknown";
			}

			// Token: 0x02002F56 RID: 12118
			public class UPTIME
			{
				// Token: 0x0400C44F RID: 50255
				public static LocString NAME = "Uptime:\n{0}{1}: {2}\n{0}{3}: {4}\n{0}{5}: {6}";

				// Token: 0x0400C450 RID: 50256
				public static LocString THIS_CYCLE = "This Cycle";

				// Token: 0x0400C451 RID: 50257
				public static LocString LAST_CYCLE = "Last Cycle";

				// Token: 0x0400C452 RID: 50258
				public static LocString LAST_X_CYCLES = "Last {0} Cycles";
			}

			// Token: 0x02002F57 RID: 12119
			public class PRIMARYELEMENT
			{
				// Token: 0x0400C453 RID: 50259
				public static LocString NAME = "Primary Element: {0}";

				// Token: 0x0400C454 RID: 50260
				public static LocString TOOLTIP = "The selected object is primarily composed of {0}";
			}

			// Token: 0x02002F58 RID: 12120
			public class UNITS
			{
				// Token: 0x0400C455 RID: 50261
				public static LocString NAME = "Stack Units: {0}";

				// Token: 0x0400C456 RID: 50262
				public static LocString TOOLTIP = "This stack contains {0} units of {1}";
			}

			// Token: 0x02002F59 RID: 12121
			public class MASS
			{
				// Token: 0x0400C457 RID: 50263
				public static LocString NAME = "Mass: {0}";

				// Token: 0x0400C458 RID: 50264
				public static LocString TOOLTIP = "The selected object has a mass of {0}";
			}

			// Token: 0x02002F5A RID: 12122
			public class TEMPERATURE
			{
				// Token: 0x0400C459 RID: 50265
				public static LocString NAME = "Temperature: {0}";

				// Token: 0x0400C45A RID: 50266
				public static LocString TOOLTIP = "The selected object's current temperature is {0}";
			}

			// Token: 0x02002F5B RID: 12123
			public class DISEASE
			{
				// Token: 0x0400C45B RID: 50267
				public static LocString NAME = "Disease: {0}";

				// Token: 0x0400C45C RID: 50268
				public static LocString TOOLTIP = "There are {0} on the selected object";
			}

			// Token: 0x02002F5C RID: 12124
			public class SHC
			{
				// Token: 0x0400C45D RID: 50269
				public static LocString NAME = "Specific Heat Capacity: {0}";

				// Token: 0x0400C45E RID: 50270
				public static LocString TOOLTIP = "{SPECIFIC_HEAT_CAPACITY} is required to heat 1 g of the selected object by 1 {TEMPERATURE_UNIT}";
			}

			// Token: 0x02002F5D RID: 12125
			public class THERMALCONDUCTIVITY
			{
				// Token: 0x0400C45F RID: 50271
				public static LocString NAME = "Thermal Conductivity: {0}";

				// Token: 0x0400C460 RID: 50272
				public static LocString TOOLTIP = "This object can conduct heat to other materials at a rate of {THERMAL_CONDUCTIVITY} W for each degree {TEMPERATURE_UNIT} difference\n\nBetween two objects, the rate of heat transfer will be determined by the object with the lowest Thermal Conductivity";

				// Token: 0x02002F5E RID: 12126
				public class ADJECTIVES
				{
					// Token: 0x0400C461 RID: 50273
					public static LocString VALUE_WITH_ADJECTIVE = "{0} ({1})";

					// Token: 0x0400C462 RID: 50274
					public static LocString VERY_LOW_CONDUCTIVITY = "Highly Insulating";

					// Token: 0x0400C463 RID: 50275
					public static LocString LOW_CONDUCTIVITY = "Insulating";

					// Token: 0x0400C464 RID: 50276
					public static LocString MEDIUM_CONDUCTIVITY = "Conductive";

					// Token: 0x0400C465 RID: 50277
					public static LocString HIGH_CONDUCTIVITY = "Highly Conductive";

					// Token: 0x0400C466 RID: 50278
					public static LocString VERY_HIGH_CONDUCTIVITY = "Extremely Conductive";
				}
			}

			// Token: 0x02002F5F RID: 12127
			public class CONDUCTIVITYBARRIER
			{
				// Token: 0x0400C467 RID: 50279
				public static LocString NAME = "Insulation Thickness: {0}";

				// Token: 0x0400C468 RID: 50280
				public static LocString TOOLTIP = "Thick insulation reduces an object's Thermal Conductivity";
			}

			// Token: 0x02002F60 RID: 12128
			public class VAPOURIZATIONPOINT
			{
				// Token: 0x0400C469 RID: 50281
				public static LocString NAME = "Vaporization Point: {0}";

				// Token: 0x0400C46A RID: 50282
				public static LocString TOOLTIP = "The selected object will evaporate into a gas at {0}";
			}

			// Token: 0x02002F61 RID: 12129
			public class MELTINGPOINT
			{
				// Token: 0x0400C46B RID: 50283
				public static LocString NAME = "Melting Point: {0}";

				// Token: 0x0400C46C RID: 50284
				public static LocString TOOLTIP = "The selected object will melt into a liquid at {0}";
			}

			// Token: 0x02002F62 RID: 12130
			public class OVERHEATPOINT
			{
				// Token: 0x0400C46D RID: 50285
				public static LocString NAME = "Overheat Modifier: {0}";

				// Token: 0x0400C46E RID: 50286
				public static LocString TOOLTIP = "This building will overheat and take damage if its temperature reaches {0}\n\nBuilding with better building materials can increase overheat temperature";
			}

			// Token: 0x02002F63 RID: 12131
			public class FREEZEPOINT
			{
				// Token: 0x0400C46F RID: 50287
				public static LocString NAME = "Freeze Point: {0}";

				// Token: 0x0400C470 RID: 50288
				public static LocString TOOLTIP = "The selected object will cool into a solid at {0}";
			}

			// Token: 0x02002F64 RID: 12132
			public class DEWPOINT
			{
				// Token: 0x0400C471 RID: 50289
				public static LocString NAME = "Condensation Point: {0}";

				// Token: 0x0400C472 RID: 50290
				public static LocString TOOLTIP = "The selected object will condense into a liquid at {0}";
			}
		}

		// Token: 0x02002F65 RID: 12133
		public class IMMIGRANTSCREEN
		{
			// Token: 0x0400C473 RID: 50291
			public static LocString IMMIGRANTSCREENTITLE = "Select a Blueprint";

			// Token: 0x0400C474 RID: 50292
			public static LocString PROCEEDBUTTON = "Print";

			// Token: 0x0400C475 RID: 50293
			public static LocString CANCELBUTTON = "Cancel";

			// Token: 0x0400C476 RID: 50294
			public static LocString REJECTALL = "Reject All";

			// Token: 0x0400C477 RID: 50295
			public static LocString EMBARK = "EMBARK";

			// Token: 0x0400C478 RID: 50296
			public static LocString SELECTDUPLICANTS = "Select {0} Duplicants";

			// Token: 0x0400C479 RID: 50297
			public static LocString SELECTYOURCREW = "CHOOSE THREE DUPLICANTS TO BEGIN";

			// Token: 0x0400C47A RID: 50298
			public static LocString SHUFFLE = "REROLL";

			// Token: 0x0400C47B RID: 50299
			public static LocString SHUFFLETOOLTIP = "Reroll for a different Duplicant";

			// Token: 0x0400C47C RID: 50300
			public static LocString BACK = "BACK";

			// Token: 0x0400C47D RID: 50301
			public static LocString CONFIRMATIONTITLE = "Reject All Printables?";

			// Token: 0x0400C47E RID: 50302
			public static LocString CONFIRMATIONBODY = "The Printing Pod will need time to recharge if I reject these Printables.";

			// Token: 0x0400C47F RID: 50303
			public static LocString NAME_YOUR_COLONY = "NAME THE COLONY";

			// Token: 0x0400C480 RID: 50304
			public static LocString CARE_PACKAGE_ELEMENT_QUANTITY = "{0} of {1}";

			// Token: 0x0400C481 RID: 50305
			public static LocString CARE_PACKAGE_ELEMENT_COUNT = "{0} x {1}";

			// Token: 0x0400C482 RID: 50306
			public static LocString CARE_PACKAGE_ELEMENT_COUNT_ONLY = "x {0}";

			// Token: 0x0400C483 RID: 50307
			public static LocString CARE_PACKAGE_CURRENT_AMOUNT = "Available: {0}";

			// Token: 0x0400C484 RID: 50308
			public static LocString DUPLICATE_COLONY_NAME = "A colony named \"{0}\" already exists";
		}

		// Token: 0x02002F66 RID: 12134
		public class METERS
		{
			// Token: 0x02002F67 RID: 12135
			public class HEALTH
			{
				// Token: 0x0400C485 RID: 50309
				public static LocString TOOLTIP = "Health";
			}

			// Token: 0x02002F68 RID: 12136
			public class BREATH
			{
				// Token: 0x0400C486 RID: 50310
				public static LocString TOOLTIP = "Oxygen";
			}

			// Token: 0x02002F69 RID: 12137
			public class FUEL
			{
				// Token: 0x0400C487 RID: 50311
				public static LocString TOOLTIP = "Fuel";
			}

			// Token: 0x02002F6A RID: 12138
			public class BATTERY
			{
				// Token: 0x0400C488 RID: 50312
				public static LocString TOOLTIP = "Battery Charge";
			}
		}
	}
}
