using System;
using STRINGS;

namespace Database
{
	// Token: 0x02002126 RID: 8486
	public class ChoreTypes : ResourceSet<ChoreType>
	{
		// Token: 0x0600B4B0 RID: 46256 RVA: 0x00445508 File Offset: 0x00443708
		public ChoreType GetByHash(HashedString id_hash)
		{
			int num = this.resources.FindIndex((ChoreType item) => item.IdHash == id_hash);
			if (num != -1)
			{
				return this.resources[num];
			}
			return null;
		}

		// Token: 0x0600B4B1 RID: 46257 RVA: 0x0044554C File Offset: 0x0044374C
		private ChoreType Add(string id, string[] chore_groups, string urge, string[] interrupt_exclusion, string name, string status_message, string tooltip, bool skip_implicit_priority_change, int explicit_priority = -1, string report_name = null)
		{
			ListPool<Tag, ChoreTypes>.PooledList pooledList = ListPool<Tag, ChoreTypes>.Allocate();
			for (int i = 0; i < interrupt_exclusion.Length; i++)
			{
				pooledList.Add(TagManager.Create(interrupt_exclusion[i]));
			}
			if (explicit_priority == -1)
			{
				explicit_priority = this.nextImplicitPriority;
			}
			ChoreType choreType = new ChoreType(id, this, chore_groups, urge, name, status_message, tooltip, pooledList, this.nextImplicitPriority, explicit_priority);
			pooledList.Recycle();
			if (!skip_implicit_priority_change)
			{
				this.nextImplicitPriority -= 50;
			}
			if (report_name != null)
			{
				choreType.reportName = report_name;
			}
			return choreType;
		}

		// Token: 0x0600B4B2 RID: 46258 RVA: 0x004455CC File Offset: 0x004437CC
		public ChoreTypes(ResourceSet parent) : base("ChoreTypes", parent)
		{
			this.Die = this.Add("Die", new string[0], "", new string[0], DUPLICANTS.CHORES.DIE.NAME, DUPLICANTS.CHORES.DIE.STATUS, DUPLICANTS.CHORES.DIE.TOOLTIP, false, -1, null);
			this.Entombed = this.Add("Entombed", new string[0], "", new string[0], DUPLICANTS.CHORES.ENTOMBED.NAME, DUPLICANTS.CHORES.ENTOMBED.STATUS, DUPLICANTS.CHORES.ENTOMBED.TOOLTIP, false, -1, null);
			this.SuitMarker = this.Add("SuitMarker", new string[0], "", new string[0], DUPLICANTS.CHORES.WASHHANDS.NAME, DUPLICANTS.CHORES.WASHHANDS.STATUS, DUPLICANTS.CHORES.WASHHANDS.TOOLTIP, false, -1, null);
			this.Slip = this.Add("Slip", new string[0], "", new string[0], DUPLICANTS.CHORES.SLIP.NAME, DUPLICANTS.CHORES.SLIP.STATUS, DUPLICANTS.CHORES.SLIP.TOOLTIP, false, -1, null);
			this.Checkpoint = this.Add("Checkpoint", new string[0], "", new string[0], DUPLICANTS.CHORES.CHECKPOINT.NAME, DUPLICANTS.CHORES.CHECKPOINT.STATUS, DUPLICANTS.CHORES.CHECKPOINT.TOOLTIP, false, -1, null);
			this.TravelTubeEntrance = this.Add("TravelTubeEntrance", new string[0], "", new string[0], DUPLICANTS.CHORES.TRAVELTUBEENTRANCE.NAME, DUPLICANTS.CHORES.TRAVELTUBEENTRANCE.STATUS, DUPLICANTS.CHORES.TRAVELTUBEENTRANCE.TOOLTIP, false, -1, null);
			this.WashHands = this.Add("WashHands", new string[0], "", new string[0], DUPLICANTS.CHORES.WASHHANDS.NAME, DUPLICANTS.CHORES.WASHHANDS.STATUS, DUPLICANTS.CHORES.WASHHANDS.TOOLTIP, false, -1, null);
			this.HealCritical = this.Add("HealCritical", new string[0], "HealCritical", new string[]
			{
				"Vomit",
				"Cough",
				"EmoteHighPriority"
			}, DUPLICANTS.CHORES.HEAL.NAME, DUPLICANTS.CHORES.HEAL.STATUS, DUPLICANTS.CHORES.HEAL.TOOLTIP, false, -1, null);
			this.BeIncapacitated = this.Add("BeIncapacitated", new string[0], "BeIncapacitated", new string[0], DUPLICANTS.CHORES.BEINCAPACITATED.NAME, DUPLICANTS.CHORES.BEINCAPACITATED.STATUS, DUPLICANTS.CHORES.BEINCAPACITATED.TOOLTIP, false, -1, null);
			this.BeOffline = this.Add("BeOffline", new string[0], "BeOffline", new string[0], DUPLICANTS.CHORES.BEOFFLINE.NAME, DUPLICANTS.CHORES.BEOFFLINE.STATUS, DUPLICANTS.CHORES.BEOFFLINE.TOOLTIP, false, -1, null);
			this.BeBatterySaveMode = this.Add("BeBatterySaveMode", new string[0], "", new string[0], DUPLICANTS.CHORES.BEBATTERYSAVEMODE.NAME, DUPLICANTS.CHORES.BEBATTERYSAVEMODE.STATUS, DUPLICANTS.CHORES.BEBATTERYSAVEMODE.TOOLTIP, false, -1, null);
			this.GeneShuffle = this.Add("GeneShuffle", new string[0], "", new string[0], DUPLICANTS.CHORES.GENESHUFFLE.NAME, DUPLICANTS.CHORES.GENESHUFFLE.STATUS, DUPLICANTS.CHORES.GENESHUFFLE.TOOLTIP, false, -1, null);
			this.Migrate = this.Add("Migrate", new string[0], "", new string[0], DUPLICANTS.CHORES.MIGRATE.NAME, DUPLICANTS.CHORES.MIGRATE.STATUS, DUPLICANTS.CHORES.MIGRATE.TOOLTIP, false, -1, null);
			this.DebugGoTo = this.Add("DebugGoTo", new string[0], "", new string[0], DUPLICANTS.CHORES.DEBUGGOTO.NAME, DUPLICANTS.CHORES.DEBUGGOTO.STATUS, DUPLICANTS.CHORES.MOVETO.TOOLTIP, false, -1, null);
			this.MoveTo = this.Add("MoveTo", new string[0], "", new string[0], DUPLICANTS.CHORES.MOVETO.NAME, DUPLICANTS.CHORES.MOVETO.STATUS, DUPLICANTS.CHORES.MOVETO.TOOLTIP, false, -1, null);
			this.RocketEnterExit = this.Add("RocketEnterExit", new string[0], "", new string[0], DUPLICANTS.CHORES.ROCKETENTEREXIT.NAME, DUPLICANTS.CHORES.ROCKETENTEREXIT.STATUS, DUPLICANTS.CHORES.ROCKETENTEREXIT.TOOLTIP, false, -1, null);
			this.DropUnusedInventory = this.Add("DropUnusedInventory", new string[0], "", new string[0], DUPLICANTS.CHORES.DROPUNUSEDINVENTORY.NAME, DUPLICANTS.CHORES.DROPUNUSEDINVENTORY.STATUS, DUPLICANTS.CHORES.DROPUNUSEDINVENTORY.TOOLTIP, false, -1, null);
			this.Pee = this.Add("Pee", new string[0], "Pee", new string[0], DUPLICANTS.CHORES.PEE.NAME, DUPLICANTS.CHORES.PEE.STATUS, DUPLICANTS.CHORES.PEE.TOOLTIP, false, -1, null);
			this.ExpellGunk = this.Add("ExpellGunk", new string[0], "GunkPee", new string[0], DUPLICANTS.CHORES.EXPELLGUNK.NAME, DUPLICANTS.CHORES.EXPELLGUNK.STATUS, DUPLICANTS.CHORES.EXPELLGUNK.TOOLTIP, false, -1, null);
			this.OilChange = this.Add("OilChange", new string[0], "OilRefill", new string[0], DUPLICANTS.CHORES.OILCHANGE.NAME, DUPLICANTS.CHORES.OILCHANGE.STATUS, DUPLICANTS.CHORES.OILCHANGE.TOOLTIP, false, -1, null);
			this.RecoverBreath = this.Add("RecoverBreath", new string[0], "RecoverBreath", new string[0], DUPLICANTS.CHORES.RECOVERBREATH.NAME, DUPLICANTS.CHORES.RECOVERBREATH.STATUS, DUPLICANTS.CHORES.RECOVERBREATH.TOOLTIP, false, -1, null);
			this.RecoverWarmth = this.Add("RecoverWarmth", new string[0], "", new string[0], DUPLICANTS.CHORES.RECOVERWARMTH.NAME, DUPLICANTS.CHORES.RECOVERWARMTH.STATUS, DUPLICANTS.CHORES.RECOVERWARMTH.TOOLTIP, false, -1, null);
			this.RecoverFromHeat = this.Add("RecoverFromHeat", new string[0], "", new string[0], DUPLICANTS.CHORES.RECOVERFROMHEAT.NAME, DUPLICANTS.CHORES.RECOVERFROMHEAT.STATUS, DUPLICANTS.CHORES.RECOVERFROMHEAT.TOOLTIP, false, -1, null);
			this.Flee = this.Add("Flee", new string[0], "", new string[0], DUPLICANTS.CHORES.FLEE.NAME, DUPLICANTS.CHORES.FLEE.STATUS, DUPLICANTS.CHORES.FLEE.TOOLTIP, false, -1, null);
			this.MoveToQuarantine = this.Add("MoveToQuarantine", new string[0], "MoveToQuarantine", new string[0], DUPLICANTS.CHORES.MOVETOQUARANTINE.NAME, DUPLICANTS.CHORES.MOVETOQUARANTINE.STATUS, DUPLICANTS.CHORES.MOVETOQUARANTINE.TOOLTIP, false, -1, null);
			this.EmoteIdle = this.Add("EmoteIdle", new string[0], "EmoteIdle", new string[0], DUPLICANTS.CHORES.EMOTEHIGHPRIORITY.NAME, DUPLICANTS.CHORES.EMOTEHIGHPRIORITY.STATUS, DUPLICANTS.CHORES.EMOTEHIGHPRIORITY.TOOLTIP, false, -1, null);
			this.Emote = this.Add("Emote", new string[0], "Emote", new string[0], DUPLICANTS.CHORES.EMOTEHIGHPRIORITY.NAME, DUPLICANTS.CHORES.EMOTEHIGHPRIORITY.STATUS, DUPLICANTS.CHORES.EMOTEHIGHPRIORITY.TOOLTIP, false, -1, null);
			this.EmoteHighPriority = this.Add("EmoteHighPriority", new string[0], "EmoteHighPriority", new string[0], DUPLICANTS.CHORES.EMOTEHIGHPRIORITY.NAME, DUPLICANTS.CHORES.EMOTEHIGHPRIORITY.STATUS, DUPLICANTS.CHORES.EMOTEHIGHPRIORITY.TOOLTIP, false, -1, null);
			this.StressEmote = this.Add("StressEmote", new string[0], "EmoteHighPriority", new string[0], DUPLICANTS.CHORES.EMOTEHIGHPRIORITY.NAME, DUPLICANTS.CHORES.EMOTEHIGHPRIORITY.STATUS, DUPLICANTS.CHORES.EMOTEHIGHPRIORITY.TOOLTIP, false, -1, null);
			this.Hug = this.Add("Hug", new string[0], "", new string[0], DUPLICANTS.CHORES.HUG.NAME, DUPLICANTS.CHORES.HUG.STATUS, DUPLICANTS.CHORES.HUG.TOOLTIP, false, -1, null);
			this.StressVomit = this.Add("StressVomit", new string[0], "", new string[0], DUPLICANTS.CHORES.STRESSVOMIT.NAME, DUPLICANTS.CHORES.STRESSVOMIT.STATUS, DUPLICANTS.CHORES.STRESSVOMIT.TOOLTIP, false, -1, null);
			this.UglyCry = this.Add("UglyCry", new string[0], "", new string[]
			{
				"MoveTo"
			}, DUPLICANTS.CHORES.UGLY_CRY.NAME, DUPLICANTS.CHORES.UGLY_CRY.STATUS, DUPLICANTS.CHORES.UGLY_CRY.TOOLTIP, false, -1, null);
			this.BansheeWail = this.Add("BansheeWail", new string[0], "", new string[]
			{
				"MoveTo"
			}, DUPLICANTS.CHORES.BANSHEE_WAIL.NAME, DUPLICANTS.CHORES.BANSHEE_WAIL.STATUS, DUPLICANTS.CHORES.BANSHEE_WAIL.TOOLTIP, false, -1, null);
			this.StressShock = this.Add("StressShock", new string[0], "", new string[]
			{
				"MoveTo"
			}, DUPLICANTS.CHORES.STRESSSHOCK.NAME, DUPLICANTS.CHORES.STRESSSHOCK.STATUS, DUPLICANTS.CHORES.STRESSSHOCK.TOOLTIP, false, -1, null);
			this.BingeEat = this.Add("BingeEat", new string[0], "", new string[]
			{
				"MoveTo"
			}, DUPLICANTS.CHORES.BINGE_EAT.NAME, DUPLICANTS.CHORES.BINGE_EAT.STATUS, DUPLICANTS.CHORES.BINGE_EAT.TOOLTIP, false, -1, null);
			this.StressActingOut = this.Add("StressActingOut", new string[0], "", new string[]
			{
				"MoveTo"
			}, DUPLICANTS.CHORES.STRESSACTINGOUT.NAME, DUPLICANTS.CHORES.STRESSACTINGOUT.STATUS, DUPLICANTS.CHORES.STRESSACTINGOUT.TOOLTIP, false, -1, null);
			this.Vomit = this.Add("Vomit", new string[0], "EmoteHighPriority", new string[0], DUPLICANTS.CHORES.VOMIT.NAME, DUPLICANTS.CHORES.VOMIT.STATUS, DUPLICANTS.CHORES.VOMIT.TOOLTIP, false, -1, null);
			this.Cough = this.Add("Cough", new string[0], "EmoteHighPriority", new string[0], DUPLICANTS.CHORES.COUGH.NAME, DUPLICANTS.CHORES.COUGH.STATUS, DUPLICANTS.CHORES.COUGH.TOOLTIP, false, -1, null);
			this.WaterDamageZap = this.Add("WaterDamageZap", new string[0], "EmoteHighPriority", new string[0], DUPLICANTS.CHORES.WATERDAMAGEZAP.NAME, DUPLICANTS.CHORES.WATERDAMAGEZAP.STATUS, DUPLICANTS.CHORES.WATERDAMAGEZAP.TOOLTIP, false, -1, null);
			this.RadiationPain = this.Add("RadiationPain", new string[0], "EmoteHighPriority", new string[0], DUPLICANTS.CHORES.RADIATIONPAIN.NAME, DUPLICANTS.CHORES.RADIATIONPAIN.STATUS, DUPLICANTS.CHORES.RADIATIONPAIN.TOOLTIP, false, -1, null);
			this.SwitchHat = this.Add("SwitchHat", new string[0], "", new string[0], DUPLICANTS.CHORES.LEARNSKILL.NAME, DUPLICANTS.CHORES.LEARNSKILL.STATUS, DUPLICANTS.CHORES.LEARNSKILL.TOOLTIP, false, -1, null);
			this.StressIdle = this.Add("StressIdle", new string[0], "", new string[0], DUPLICANTS.CHORES.STRESSIDLE.NAME, DUPLICANTS.CHORES.STRESSIDLE.STATUS, DUPLICANTS.CHORES.STRESSIDLE.TOOLTIP, false, -1, null);
			this.RescueIncapacitated = this.Add("RescueIncapacitated", new string[0], "", new string[0], DUPLICANTS.CHORES.RESCUEINCAPACITATED.NAME, DUPLICANTS.CHORES.RESCUEINCAPACITATED.STATUS, DUPLICANTS.CHORES.RESCUEINCAPACITATED.TOOLTIP, false, -1, null);
			this.BreakPee = this.Add("BreakPee", new string[0], "Pee", new string[0], DUPLICANTS.CHORES.BREAK_PEE.NAME, DUPLICANTS.CHORES.BREAK_PEE.STATUS, DUPLICANTS.CHORES.BREAK_PEE.TOOLTIP, false, -1, null);
			this.Eat = this.Add("Eat", new string[0], "Eat", new string[0], DUPLICANTS.CHORES.EAT.NAME, DUPLICANTS.CHORES.EAT.STATUS, DUPLICANTS.CHORES.EAT.TOOLTIP, false, -1, null);
			this.ReloadElectrobank = this.Add("ReloadElectrobank", new string[0], "ReloadElectrobank", new string[0], DUPLICANTS.CHORES.RELOADELECTROBANK.NAME, DUPLICANTS.CHORES.RELOADELECTROBANK.STATUS, DUPLICANTS.CHORES.RELOADELECTROBANK.TOOLTIP, false, -1, null);
			this.FindOxygenSourceItem = this.Add("FindOxygenCanister", new string[0], "FindOxygenRefill", new string[0], DUPLICANTS.CHORES.FINDOXYGENSOURCEITEM.NAME, DUPLICANTS.CHORES.FINDOXYGENSOURCEITEM.STATUS, DUPLICANTS.CHORES.FINDOXYGENSOURCEITEM.TOOLTIP, false, -1, null);
			this.BionicAbsorbOxygen = this.Add("BionicAbsorbOxygen", new string[0], "FindOxygenRefill", new string[0], DUPLICANTS.CHORES.BIONICABSORBOXYGEN.NAME, DUPLICANTS.CHORES.BIONICABSORBOXYGEN.STATUS, DUPLICANTS.CHORES.BIONICABSORBOXYGEN.TOOLTIP, false, -1, null);
			this.UnloadElectrobank = this.Add("UnloadElectrobank", new string[0], "RemoveDischargedElectrobank", new string[0], DUPLICANTS.CHORES.UNLOADELECTROBANK.NAME, DUPLICANTS.CHORES.UNLOADELECTROBANK.STATUS, DUPLICANTS.CHORES.UNLOADELECTROBANK.TOOLTIP, false, -1, null);
			this.SeekAndInstallUpgrade = this.Add("SeekAndInstallUpgrade", new string[0], "", new string[0], DUPLICANTS.CHORES.SEEKANDINSTALLUPGRADE.NAME, DUPLICANTS.CHORES.SEEKANDINSTALLUPGRADE.STATUS, DUPLICANTS.CHORES.SEEKANDINSTALLUPGRADE.TOOLTIP, false, -1, null);
			this.Narcolepsy = this.Add("Narcolepsy", new string[0], "Narcolepsy", new string[0], DUPLICANTS.CHORES.NARCOLEPSY.NAME, DUPLICANTS.CHORES.NARCOLEPSY.STATUS, DUPLICANTS.CHORES.NARCOLEPSY.TOOLTIP, false, -1, null);
			this.ReturnSuitUrgent = this.Add("ReturnSuitUrgent", new string[0], "", new string[0], DUPLICANTS.CHORES.RETURNSUIT.NAME, DUPLICANTS.CHORES.RETURNSUIT.STATUS, DUPLICANTS.CHORES.RETURNSUIT.TOOLTIP, false, -1, null);
			this.SleepDueToDisease = this.Add("SleepDueToDisease", new string[0], "Sleep", new string[]
			{
				"Vomit",
				"Cough",
				"EmoteHighPriority"
			}, DUPLICANTS.CHORES.RESTDUETODISEASE.NAME, DUPLICANTS.CHORES.RESTDUETODISEASE.STATUS, DUPLICANTS.CHORES.RESTDUETODISEASE.TOOLTIP, false, -1, null);
			this.Sleep = this.Add("Sleep", new string[0], "Sleep", new string[0], DUPLICANTS.CHORES.SLEEP.NAME, DUPLICANTS.CHORES.SLEEP.STATUS, DUPLICANTS.CHORES.SLEEP.TOOLTIP, false, -1, null);
			this.TakeMedicine = this.Add("TakeMedicine", new string[0], "", new string[0], DUPLICANTS.CHORES.TAKEMEDICINE.NAME, DUPLICANTS.CHORES.TAKEMEDICINE.STATUS, DUPLICANTS.CHORES.TAKEMEDICINE.TOOLTIP, false, -1, null);
			this.GetDoctored = this.Add("GetDoctored", new string[0], "", new string[0], DUPLICANTS.CHORES.GETDOCTORED.NAME, DUPLICANTS.CHORES.GETDOCTORED.STATUS, DUPLICANTS.CHORES.GETDOCTORED.TOOLTIP, false, -1, null);
			this.RestDueToDisease = this.Add("RestDueToDisease", new string[0], "RestDueToDisease", new string[]
			{
				"Vomit",
				"Cough",
				"EmoteHighPriority"
			}, DUPLICANTS.CHORES.RESTDUETODISEASE.NAME, DUPLICANTS.CHORES.RESTDUETODISEASE.STATUS, DUPLICANTS.CHORES.RESTDUETODISEASE.TOOLTIP, false, -1, null);
			this.ScrubOre = this.Add("ScrubOre", new string[0], "", new string[0], DUPLICANTS.CHORES.SCRUBORE.NAME, DUPLICANTS.CHORES.SCRUBORE.STATUS, DUPLICANTS.CHORES.SCRUBORE.TOOLTIP, false, -1, null);
			this.DeliverFood = this.Add("DeliverFood", new string[0], "", new string[0], DUPLICANTS.CHORES.DELIVERFOOD.NAME, DUPLICANTS.CHORES.DELIVERFOOD.STATUS, DUPLICANTS.CHORES.DELIVERFOOD.TOOLTIP, false, -1, null);
			this.Sigh = this.Add("Sigh", new string[0], "Emote", new string[0], DUPLICANTS.CHORES.SIGH.NAME, DUPLICANTS.CHORES.SIGH.STATUS, DUPLICANTS.CHORES.SIGH.TOOLTIP, false, -1, null);
			this.Heal = this.Add("Heal", new string[0], "Heal", new string[]
			{
				"Vomit",
				"Cough",
				"EmoteHighPriority"
			}, DUPLICANTS.CHORES.HEAL.NAME, DUPLICANTS.CHORES.HEAL.STATUS, DUPLICANTS.CHORES.HEAL.TOOLTIP, false, -1, null);
			this.Shower = this.Add("Shower", new string[0], "Shower", new string[0], DUPLICANTS.CHORES.SHOWER.NAME, DUPLICANTS.CHORES.SHOWER.STATUS, DUPLICANTS.CHORES.SHOWER.TOOLTIP, false, -1, null);
			this.LearnSkill = this.Add("LearnSkill", new string[0], "LearnSkill", new string[0], DUPLICANTS.CHORES.LEARNSKILL.NAME, DUPLICANTS.CHORES.LEARNSKILL.STATUS, DUPLICANTS.CHORES.LEARNSKILL.TOOLTIP, false, -1, null);
			this.UnlearnSkill = this.Add("UnlearnSkill", new string[0], "", new string[0], DUPLICANTS.CHORES.UNLEARNSKILL.NAME, DUPLICANTS.CHORES.UNLEARNSKILL.STATUS, DUPLICANTS.CHORES.UNLEARNSKILL.TOOLTIP, false, -1, null);
			this.Equip = this.Add("Equip", new string[0], "", new string[0], DUPLICANTS.CHORES.EQUIP.NAME, DUPLICANTS.CHORES.EQUIP.STATUS, DUPLICANTS.CHORES.EQUIP.TOOLTIP, false, -1, null);
			this.JoyReaction = this.Add("JoyReaction", new string[0], "", new string[0], DUPLICANTS.CHORES.JOYREACTION.NAME, DUPLICANTS.CHORES.JOYREACTION.STATUS, DUPLICANTS.CHORES.JOYREACTION.TOOLTIP, false, -1, null);
			this.RocketControl = this.Add("RocketControl", new string[]
			{
				"Rocketry"
			}, "", new string[0], DUPLICANTS.CHORES.ROCKETCONTROL.NAME, DUPLICANTS.CHORES.ROCKETCONTROL.STATUS, DUPLICANTS.CHORES.ROCKETCONTROL.TOOLTIP, false, -1, null);
			this.StressHeal = this.Add("StressHeal", new string[0], "", new string[]
			{
				""
			}, DUPLICANTS.CHORES.STRESSHEAL.NAME, DUPLICANTS.CHORES.STRESSHEAL.STATUS, DUPLICANTS.CHORES.STRESSHEAL.TOOLTIP, false, -1, null);
			this.Party = this.Add("Party", new string[0], "", new string[0], DUPLICANTS.CHORES.PARTY.NAME, DUPLICANTS.CHORES.PARTY.STATUS, DUPLICANTS.CHORES.PARTY.TOOLTIP, false, -1, null);
			this.Relax = this.Add("Relax", new string[]
			{
				"Recreation"
			}, "", new string[]
			{
				"Sleep"
			}, DUPLICANTS.CHORES.RELAX.NAME, DUPLICANTS.CHORES.RELAX.STATUS, DUPLICANTS.CHORES.RELAX.TOOLTIP, false, -1, null);
			this.Recharge = this.Add("Recharge", new string[0], "", new string[0], DUPLICANTS.CHORES.RECHARGE.NAME, DUPLICANTS.CHORES.RECHARGE.STATUS, DUPLICANTS.CHORES.RECHARGE.TOOLTIP, false, -1, null);
			this.Unequip = this.Add("Unequip", new string[0], "", new string[0], DUPLICANTS.CHORES.UNEQUIP.NAME, DUPLICANTS.CHORES.UNEQUIP.STATUS, DUPLICANTS.CHORES.UNEQUIP.TOOLTIP, false, -1, null);
			this.Mourn = this.Add("Mourn", new string[0], "", new string[0], DUPLICANTS.CHORES.MOURN.NAME, DUPLICANTS.CHORES.MOURN.STATUS, DUPLICANTS.CHORES.MOURN.TOOLTIP, false, -1, null);
			this.TopPriority = this.Add("TopPriority", new string[0], "", new string[0], "", "", "", false, -1, null);
			this.Attack = this.Add("Attack", new string[]
			{
				"Combat"
			}, "", new string[0], DUPLICANTS.CHORES.ATTACK.NAME, DUPLICANTS.CHORES.ATTACK.STATUS, DUPLICANTS.CHORES.ATTACK.TOOLTIP, false, 5000, null);
			this.Doctor = this.Add("DoctorChore", new string[]
			{
				"MedicalAid"
			}, "Doctor", new string[0], DUPLICANTS.CHORES.DOCTOR.NAME, DUPLICANTS.CHORES.DOCTOR.STATUS, DUPLICANTS.CHORES.DOCTOR.TOOLTIP, false, 5000, null);
			this.Toggle = this.Add("Toggle", new string[]
			{
				"Toggle"
			}, "", new string[0], DUPLICANTS.CHORES.TOGGLE.NAME, DUPLICANTS.CHORES.TOGGLE.STATUS, DUPLICANTS.CHORES.TOGGLE.TOOLTIP, true, 5000, null);
			this.Capture = this.Add("Capture", new string[]
			{
				"Ranching"
			}, "", new string[0], DUPLICANTS.CHORES.CAPTURE.NAME, DUPLICANTS.CHORES.CAPTURE.STATUS, DUPLICANTS.CHORES.CAPTURE.TOOLTIP, false, 5000, null);
			this.CreatureFetch = this.Add("CreatureFetch", new string[]
			{
				"Ranching"
			}, "", new string[0], DUPLICANTS.CHORES.FETCHCREATURE.NAME, DUPLICANTS.CHORES.FETCHCREATURE.STATUS, DUPLICANTS.CHORES.FETCHCREATURE.TOOLTIP, false, 5000, null);
			this.RanchingFetch = this.Add("RanchingFetch", new string[]
			{
				"Ranching",
				"Hauling"
			}, "", new string[0], DUPLICANTS.CHORES.FETCHRANCHING.NAME, DUPLICANTS.CHORES.FETCHRANCHING.STATUS, DUPLICANTS.CHORES.FETCHRANCHING.TOOLTIP, false, 5000, null);
			this.EggSing = this.Add("EggSing", new string[]
			{
				"Ranching"
			}, "", new string[0], DUPLICANTS.CHORES.SINGTOEGG.NAME, DUPLICANTS.CHORES.SINGTOEGG.STATUS, DUPLICANTS.CHORES.SINGTOEGG.TOOLTIP, false, 5000, null);
			this.Astronaut = this.Add("Astronaut", new string[]
			{
				"MachineOperating"
			}, "", new string[0], DUPLICANTS.CHORES.ASTRONAUT.NAME, DUPLICANTS.CHORES.ASTRONAUT.STATUS, DUPLICANTS.CHORES.ASTRONAUT.TOOLTIP, false, 5000, null);
			this.FetchCritical = this.Add("FetchCritical", new string[]
			{
				"Hauling",
				"LifeSupport"
			}, "", new string[0], DUPLICANTS.CHORES.FETCHCRITICAL.NAME, DUPLICANTS.CHORES.FETCHCRITICAL.STATUS, DUPLICANTS.CHORES.FETCHCRITICAL.TOOLTIP, false, 5000, DUPLICANTS.CHORES.FETCHCRITICAL.REPORT_NAME);
			this.Art = this.Add("Art", new string[]
			{
				"Art"
			}, "", new string[0], DUPLICANTS.CHORES.ART.NAME, DUPLICANTS.CHORES.ART.STATUS, DUPLICANTS.CHORES.ART.TOOLTIP, false, 5000, null);
			this.EmptyStorage = this.Add("EmptyStorage", new string[]
			{
				"Basekeeping",
				"Hauling"
			}, "", new string[0], DUPLICANTS.CHORES.EMPTYSTORAGE.NAME, DUPLICANTS.CHORES.EMPTYSTORAGE.STATUS, DUPLICANTS.CHORES.EMPTYSTORAGE.TOOLTIP, false, 5000, null);
			this.Mop = this.Add("Mop", new string[]
			{
				"Basekeeping"
			}, "", new string[0], DUPLICANTS.CHORES.MOP.NAME, DUPLICANTS.CHORES.MOP.STATUS, DUPLICANTS.CHORES.MOP.TOOLTIP, true, 5000, null);
			this.Relocate = this.Add("Relocate", new string[0], "", new string[0], DUPLICANTS.CHORES.RELOCATE.NAME, DUPLICANTS.CHORES.RELOCATE.STATUS, DUPLICANTS.CHORES.RELOCATE.TOOLTIP, true, 5000, null);
			this.Disinfect = this.Add("Disinfect", new string[]
			{
				"Basekeeping"
			}, "", new string[0], DUPLICANTS.CHORES.DISINFECT.NAME, DUPLICANTS.CHORES.DISINFECT.STATUS, DUPLICANTS.CHORES.DISINFECT.TOOLTIP, true, 5000, null);
			this.Repair = this.Add("Repair", new string[]
			{
				"Basekeeping"
			}, "", new string[0], DUPLICANTS.CHORES.REPAIR.NAME, DUPLICANTS.CHORES.REPAIR.STATUS, DUPLICANTS.CHORES.REPAIR.TOOLTIP, false, 5000, null);
			this.RepairFetch = this.Add("RepairFetch", new string[]
			{
				"Basekeeping",
				"Hauling"
			}, "", new string[0], DUPLICANTS.CHORES.REPAIRFETCH.NAME, DUPLICANTS.CHORES.REPAIRFETCH.STATUS, DUPLICANTS.CHORES.REPAIRFETCH.TOOLTIP, false, 5000, null);
			this.Deconstruct = this.Add("Deconstruct", new string[]
			{
				"Build"
			}, "", new string[0], DUPLICANTS.CHORES.DECONSTRUCT.NAME, DUPLICANTS.CHORES.DECONSTRUCT.STATUS, DUPLICANTS.CHORES.DECONSTRUCT.TOOLTIP, false, 5000, null);
			this.Demolish = this.Add("Demolish", new string[]
			{
				"Build"
			}, "", new string[0], DUPLICANTS.CHORES.DEMOLISH.NAME, DUPLICANTS.CHORES.DEMOLISH.STATUS, DUPLICANTS.CHORES.DEMOLISH.TOOLTIP, false, 5000, null);
			this.Research = this.Add("Research", new string[]
			{
				"Research"
			}, "", new string[0], DUPLICANTS.CHORES.RESEARCH.NAME, DUPLICANTS.CHORES.RESEARCH.STATUS, DUPLICANTS.CHORES.RESEARCH.TOOLTIP, false, 5000, null);
			this.AnalyzeArtifact = this.Add("AnalyzeArtifact", new string[]
			{
				"Research",
				"Art"
			}, "", new string[0], DUPLICANTS.CHORES.ANALYZEARTIFACT.NAME, DUPLICANTS.CHORES.ANALYZEARTIFACT.STATUS, DUPLICANTS.CHORES.ANALYZEARTIFACT.TOOLTIP, false, 5000, null);
			this.AnalyzeSeed = this.Add("AnalyzeSeed", new string[]
			{
				"Research",
				"Farming"
			}, "", new string[0], DUPLICANTS.CHORES.ANALYZESEED.NAME, DUPLICANTS.CHORES.ANALYZESEED.STATUS, DUPLICANTS.CHORES.ANALYZESEED.TOOLTIP, false, 5000, null);
			this.ExcavateFossil = this.Add("ExcavateFossil", new string[]
			{
				"Research",
				"Art",
				"Dig"
			}, "", new string[0], DUPLICANTS.CHORES.EXCAVATEFOSSIL.NAME, DUPLICANTS.CHORES.EXCAVATEFOSSIL.STATUS, DUPLICANTS.CHORES.EXCAVATEFOSSIL.TOOLTIP, false, 5000, null);
			this.ResearchFetch = this.Add("ResearchFetch", new string[]
			{
				"Research",
				"Hauling"
			}, "", new string[0], DUPLICANTS.CHORES.RESEARCHFETCH.NAME, DUPLICANTS.CHORES.RESEARCHFETCH.STATUS, DUPLICANTS.CHORES.RESEARCHFETCH.TOOLTIP, false, 5000, null);
			this.GeneratePower = this.Add("GeneratePower", new string[]
			{
				"MachineOperating"
			}, "", new string[]
			{
				"StressHeal"
			}, DUPLICANTS.CHORES.GENERATEPOWER.NAME, DUPLICANTS.CHORES.GENERATEPOWER.STATUS, DUPLICANTS.CHORES.GENERATEPOWER.TOOLTIP, false, 5000, null);
			this.CropTend = this.Add("CropTend", new string[]
			{
				"Farming"
			}, "", new string[0], DUPLICANTS.CHORES.CROP_TEND.NAME, DUPLICANTS.CHORES.CROP_TEND.STATUS, DUPLICANTS.CHORES.CROP_TEND.TOOLTIP, false, 5000, null);
			this.PowerTinker = this.Add("PowerTinker", new string[]
			{
				"MachineOperating"
			}, "", new string[0], DUPLICANTS.CHORES.POWER_TINKER.NAME, DUPLICANTS.CHORES.POWER_TINKER.STATUS, DUPLICANTS.CHORES.POWER_TINKER.TOOLTIP, false, 5000, null);
			this.RemoteOperate = this.Add("RemoteOperate", new string[]
			{
				"MachineOperating"
			}, "", new string[0], DUPLICANTS.CHORES.REMOTEWORK.NAME, DUPLICANTS.CHORES.REMOTEWORK.STATUS, DUPLICANTS.CHORES.REMOTEWORK.TOOLTIP, false, 5000, null);
			this.MachineTinker = this.Add("MachineTinker", new string[]
			{
				"MachineOperating"
			}, "", new string[0], DUPLICANTS.CHORES.POWER_TINKER.NAME, DUPLICANTS.CHORES.POWER_TINKER.STATUS, DUPLICANTS.CHORES.POWER_TINKER.TOOLTIP, false, 5000, null);
			this.MachineFetch = this.Add("MachineFetch", new string[]
			{
				"MachineOperating",
				"Hauling"
			}, "", new string[0], DUPLICANTS.CHORES.MACHINEFETCH.NAME, DUPLICANTS.CHORES.MACHINEFETCH.STATUS, DUPLICANTS.CHORES.MACHINEFETCH.TOOLTIP, false, 5000, DUPLICANTS.CHORES.MACHINEFETCH.REPORT_NAME);
			this.Harvest = this.Add("Harvest", new string[]
			{
				"Farming"
			}, "", new string[0], DUPLICANTS.CHORES.HARVEST.NAME, DUPLICANTS.CHORES.HARVEST.STATUS, DUPLICANTS.CHORES.HARVEST.TOOLTIP, false, 5000, null);
			this.FarmFetch = this.Add("FarmFetch", new string[]
			{
				"Farming",
				"Hauling"
			}, "", new string[0], DUPLICANTS.CHORES.FARMFETCH.NAME, DUPLICANTS.CHORES.FARMFETCH.STATUS, DUPLICANTS.CHORES.FARMFETCH.TOOLTIP, false, 5000, null);
			this.Uproot = this.Add("Uproot", new string[]
			{
				"Farming"
			}, "", new string[0], DUPLICANTS.CHORES.UPROOT.NAME, DUPLICANTS.CHORES.UPROOT.STATUS, DUPLICANTS.CHORES.UPROOT.TOOLTIP, false, 5000, null);
			this.CleanToilet = this.Add("CleanToilet", new string[]
			{
				"Basekeeping"
			}, "", new string[0], DUPLICANTS.CHORES.CLEANTOILET.NAME, DUPLICANTS.CHORES.CLEANTOILET.STATUS, DUPLICANTS.CHORES.CLEANTOILET.TOOLTIP, false, 5000, null);
			this.EmptyDesalinator = this.Add("EmptyDesalinator", new string[]
			{
				"Basekeeping"
			}, "", new string[0], DUPLICANTS.CHORES.EMPTYDESALINATOR.NAME, DUPLICANTS.CHORES.EMPTYDESALINATOR.STATUS, DUPLICANTS.CHORES.EMPTYDESALINATOR.TOOLTIP, false, 5000, null);
			this.LiquidCooledFan = this.Add("LiquidCooledFan", new string[]
			{
				"MachineOperating"
			}, "", new string[0], DUPLICANTS.CHORES.LIQUIDCOOLEDFAN.NAME, DUPLICANTS.CHORES.LIQUIDCOOLEDFAN.STATUS, DUPLICANTS.CHORES.LIQUIDCOOLEDFAN.TOOLTIP, false, 5000, null);
			this.IceCooledFan = this.Add("IceCooledFan", new string[]
			{
				"MachineOperating"
			}, "", new string[0], DUPLICANTS.CHORES.ICECOOLEDFAN.NAME, DUPLICANTS.CHORES.ICECOOLEDFAN.STATUS, DUPLICANTS.CHORES.ICECOOLEDFAN.TOOLTIP, false, 5000, null);
			this.Train = this.Add("Train", new string[]
			{
				"MachineOperating"
			}, "", new string[0], DUPLICANTS.CHORES.TRAIN.NAME, DUPLICANTS.CHORES.TRAIN.STATUS, DUPLICANTS.CHORES.TRAIN.TOOLTIP, false, 5000, null);
			this.ProcessCritter = this.Add("ProcessCritter", new string[]
			{
				"Ranching"
			}, "", new string[0], DUPLICANTS.CHORES.PROCESSCRITTER.NAME, DUPLICANTS.CHORES.PROCESSCRITTER.STATUS, DUPLICANTS.CHORES.PROCESSCRITTER.TOOLTIP, false, 5000, null);
			this.Cook = this.Add("Cook", new string[]
			{
				"Cook"
			}, "", new string[0], DUPLICANTS.CHORES.COOK.NAME, DUPLICANTS.CHORES.COOK.STATUS, DUPLICANTS.CHORES.COOK.TOOLTIP, false, 5000, null);
			this.CookFetch = this.Add("CookFetch", new string[]
			{
				"Cook",
				"Hauling"
			}, "", new string[0], DUPLICANTS.CHORES.COOKFETCH.NAME, DUPLICANTS.CHORES.COOKFETCH.STATUS, DUPLICANTS.CHORES.COOKFETCH.TOOLTIP, false, 5000, null);
			this.DoctorFetch = this.Add("DoctorFetch", new string[]
			{
				"MedicalAid",
				"Hauling"
			}, "", new string[0], DUPLICANTS.CHORES.DOCTORFETCH.NAME, DUPLICANTS.CHORES.DOCTORFETCH.STATUS, DUPLICANTS.CHORES.DOCTORFETCH.TOOLTIP, false, 5000, DUPLICANTS.CHORES.DOCTORFETCH.REPORT_NAME);
			this.Ranch = this.Add("Ranch", new string[]
			{
				"Ranching"
			}, "", new string[0], DUPLICANTS.CHORES.RANCH.NAME, DUPLICANTS.CHORES.RANCH.STATUS, DUPLICANTS.CHORES.RANCH.TOOLTIP, false, 5000, null);
			this.PowerFetch = this.Add("PowerFetch", new string[]
			{
				"MachineOperating",
				"Hauling"
			}, "", new string[0], DUPLICANTS.CHORES.POWERFETCH.NAME, DUPLICANTS.CHORES.POWERFETCH.STATUS, DUPLICANTS.CHORES.POWERFETCH.TOOLTIP, false, 5000, DUPLICANTS.CHORES.POWERFETCH.REPORT_NAME);
			this.FlipCompost = this.Add("FlipCompost", new string[]
			{
				"Farming"
			}, "", new string[0], DUPLICANTS.CHORES.FLIPCOMPOST.NAME, DUPLICANTS.CHORES.FLIPCOMPOST.STATUS, DUPLICANTS.CHORES.FLIPCOMPOST.TOOLTIP, false, 5000, null);
			this.Depressurize = this.Add("Depressurize", new string[]
			{
				"MachineOperating"
			}, "", new string[0], DUPLICANTS.CHORES.DEPRESSURIZE.NAME, DUPLICANTS.CHORES.DEPRESSURIZE.STATUS, DUPLICANTS.CHORES.DEPRESSURIZE.TOOLTIP, false, 5000, null);
			this.FarmingFabricate = this.Add("FarmingFabricate", new string[]
			{
				"Farming"
			}, "", new string[0], DUPLICANTS.CHORES.FABRICATE.NAME, DUPLICANTS.CHORES.FABRICATE.STATUS, DUPLICANTS.CHORES.FABRICATE.TOOLTIP, false, 5000, null);
			this.PowerFabricate = this.Add("PowerFabricate", new string[]
			{
				"MachineOperating"
			}, "", new string[0], DUPLICANTS.CHORES.FABRICATE.NAME, DUPLICANTS.CHORES.FABRICATE.STATUS, DUPLICANTS.CHORES.FABRICATE.TOOLTIP, false, 5000, null);
			this.Compound = this.Add("Compound", new string[]
			{
				"MedicalAid"
			}, "", new string[0], DUPLICANTS.CHORES.COMPOUND.NAME, DUPLICANTS.CHORES.COMPOUND.STATUS, DUPLICANTS.CHORES.COMPOUND.TOOLTIP, false, 5000, null);
			this.Fabricate = this.Add("Fabricate", new string[]
			{
				"MachineOperating"
			}, "", new string[0], DUPLICANTS.CHORES.FABRICATE.NAME, DUPLICANTS.CHORES.FABRICATE.STATUS, DUPLICANTS.CHORES.FABRICATE.TOOLTIP, false, 5000, null);
			this.FabricateFetch = this.Add("FabricateFetch", new string[]
			{
				"MachineOperating",
				"Hauling"
			}, "", new string[0], DUPLICANTS.CHORES.FABRICATEFETCH.NAME, DUPLICANTS.CHORES.FABRICATEFETCH.STATUS, DUPLICANTS.CHORES.FABRICATEFETCH.TOOLTIP, false, 5000, DUPLICANTS.CHORES.FABRICATEFETCH.REPORT_NAME);
			this.FoodFetch = this.Add("FoodFetch", new string[]
			{
				"Hauling"
			}, "", new string[0], DUPLICANTS.CHORES.FOODFETCH.NAME, DUPLICANTS.CHORES.FOODFETCH.STATUS, DUPLICANTS.CHORES.FOODFETCH.TOOLTIP, false, 5000, DUPLICANTS.CHORES.FOODFETCH.REPORT_NAME);
			this.Transport = this.Add("Transport", new string[]
			{
				"Hauling",
				"Basekeeping"
			}, "", new string[0], DUPLICANTS.CHORES.TRANSPORT.NAME, DUPLICANTS.CHORES.TRANSPORT.STATUS, DUPLICANTS.CHORES.TRANSPORT.TOOLTIP, true, 5000, null);
			this.Build = this.Add("Build", new string[]
			{
				"Build"
			}, "", new string[0], DUPLICANTS.CHORES.BUILD.NAME, DUPLICANTS.CHORES.BUILD.STATUS, DUPLICANTS.CHORES.BUILD.TOOLTIP, true, 5000, null);
			this.BuildDig = this.Add("BuildDig", new string[]
			{
				"Build",
				"Dig"
			}, "", new string[0], DUPLICANTS.CHORES.BUILDDIG.NAME, DUPLICANTS.CHORES.BUILDDIG.STATUS, DUPLICANTS.CHORES.BUILDDIG.TOOLTIP, true, 5000, null);
			this.BuildFetch = this.Add("BuildFetch", new string[]
			{
				"Build",
				"Hauling"
			}, "", new string[0], DUPLICANTS.CHORES.BUILDFETCH.NAME, DUPLICANTS.CHORES.BUILDFETCH.STATUS, DUPLICANTS.CHORES.BUILDFETCH.TOOLTIP, true, 5000, null);
			this.Dig = this.Add("Dig", new string[]
			{
				"Dig"
			}, "", new string[0], DUPLICANTS.CHORES.DIG.NAME, DUPLICANTS.CHORES.DIG.STATUS, DUPLICANTS.CHORES.DIG.TOOLTIP, false, 5000, null);
			this.Fetch = this.Add("Fetch", new string[]
			{
				"Storage"
			}, "", new string[0], DUPLICANTS.CHORES.FETCH.NAME, DUPLICANTS.CHORES.FETCH.STATUS, DUPLICANTS.CHORES.FETCH.TOOLTIP, false, 5000, DUPLICANTS.CHORES.FETCH.REPORT_NAME);
			this.StorageFetch = this.Add("StorageFetch", new string[]
			{
				"Storage"
			}, "", new string[0], DUPLICANTS.CHORES.STORAGEFETCH.NAME, DUPLICANTS.CHORES.STORAGEFETCH.STATUS, DUPLICANTS.CHORES.STORAGEFETCH.TOOLTIP, true, 5000, DUPLICANTS.CHORES.STORAGEFETCH.REPORT_NAME);
			this.EquipmentFetch = this.Add("EquipmentFetch", new string[]
			{
				"Hauling"
			}, "", new string[0], DUPLICANTS.CHORES.EQUIPMENTFETCH.NAME, DUPLICANTS.CHORES.EQUIPMENTFETCH.STATUS, DUPLICANTS.CHORES.EQUIPMENTFETCH.TOOLTIP, false, 5000, DUPLICANTS.CHORES.EQUIPMENTFETCH.REPORT_NAME);
			this.ArmTrap = this.Add("ArmTrap", new string[]
			{
				"Ranching"
			}, "", new string[0], DUPLICANTS.CHORES.ARMTRAP.NAME, DUPLICANTS.CHORES.ARMTRAP.STATUS, DUPLICANTS.CHORES.ARMTRAP.TOOLTIP, false, -1, null);
			this.MoveToSafety = this.Add("MoveToSafety", new string[0], "MoveToSafety", new string[0], DUPLICANTS.CHORES.MOVETOSAFETY.NAME, DUPLICANTS.CHORES.MOVETOSAFETY.STATUS, DUPLICANTS.CHORES.MOVETOSAFETY.TOOLTIP, false, -1, null);
			this.ReturnSuitIdle = this.Add("ReturnSuitIdle", new string[0], "", new string[0], DUPLICANTS.CHORES.RETURNSUIT.NAME, DUPLICANTS.CHORES.RETURNSUIT.STATUS, DUPLICANTS.CHORES.RETURNSUIT.TOOLTIP, false, -1, null);
			this.Idle = this.Add("IdleChore", new string[0], "", new string[0], DUPLICANTS.CHORES.IDLE.NAME, DUPLICANTS.CHORES.IDLE.STATUS, DUPLICANTS.CHORES.IDLE.TOOLTIP, false, -1, null);
			ChoreType[][] array = new ChoreType[][]
			{
				new ChoreType[]
				{
					this.Die
				},
				new ChoreType[]
				{
					this.Entombed
				},
				new ChoreType[]
				{
					this.HealCritical
				},
				new ChoreType[]
				{
					this.BeIncapacitated,
					this.GeneShuffle,
					this.Migrate
				},
				new ChoreType[]
				{
					this.BeOffline
				},
				new ChoreType[]
				{
					this.DebugGoTo
				},
				new ChoreType[]
				{
					this.StressVomit
				},
				new ChoreType[]
				{
					this.MoveTo,
					this.RocketEnterExit
				},
				new ChoreType[]
				{
					this.RecoverBreath,
					this.FindOxygenSourceItem,
					this.BionicAbsorbOxygen
				},
				new ChoreType[]
				{
					this.ReturnSuitUrgent
				},
				new ChoreType[]
				{
					this.UglyCry
				},
				new ChoreType[]
				{
					this.BingeEat,
					this.BansheeWail,
					this.StressShock
				},
				new ChoreType[]
				{
					this.WaterDamageZap
				},
				new ChoreType[]
				{
					this.ExpellGunk
				},
				new ChoreType[]
				{
					this.EmoteHighPriority,
					this.StressActingOut,
					this.Vomit,
					this.Cough,
					this.Pee,
					this.StressIdle,
					this.RescueIncapacitated,
					this.SwitchHat,
					this.RadiationPain,
					this.OilChange
				},
				new ChoreType[]
				{
					this.MoveToQuarantine
				},
				new ChoreType[]
				{
					this.TopPriority
				},
				new ChoreType[]
				{
					this.RocketControl
				},
				new ChoreType[]
				{
					this.JoyReaction
				},
				new ChoreType[]
				{
					this.BeBatterySaveMode
				},
				new ChoreType[]
				{
					this.Attack
				},
				new ChoreType[]
				{
					this.Flee
				},
				new ChoreType[]
				{
					this.LearnSkill,
					this.UnlearnSkill,
					this.Eat,
					this.ReloadElectrobank,
					this.UnloadElectrobank,
					this.BreakPee,
					this.SeekAndInstallUpgrade
				},
				new ChoreType[]
				{
					this.TakeMedicine
				},
				new ChoreType[]
				{
					this.Heal,
					this.SleepDueToDisease,
					this.RestDueToDisease
				},
				new ChoreType[]
				{
					this.Sleep,
					this.Narcolepsy
				},
				new ChoreType[]
				{
					this.Doctor,
					this.GetDoctored
				},
				new ChoreType[]
				{
					this.Emote,
					this.Hug
				},
				new ChoreType[]
				{
					this.Mourn
				},
				new ChoreType[]
				{
					this.StressHeal
				},
				new ChoreType[]
				{
					this.Party
				},
				new ChoreType[]
				{
					this.Relax
				},
				new ChoreType[]
				{
					this.Equip,
					this.Unequip
				},
				new ChoreType[]
				{
					this.DeliverFood,
					this.Sigh,
					this.EmptyStorage,
					this.Repair,
					this.Disinfect,
					this.Shower,
					this.CleanToilet,
					this.LiquidCooledFan,
					this.IceCooledFan,
					this.SuitMarker,
					this.Checkpoint,
					this.Slip,
					this.TravelTubeEntrance,
					this.WashHands,
					this.Recharge,
					this.ScrubOre,
					this.Ranch,
					this.MoveToSafety,
					this.Relocate,
					this.Research,
					this.Mop,
					this.Toggle,
					this.Deconstruct,
					this.Demolish,
					this.Capture,
					this.EggSing,
					this.Art,
					this.GeneratePower,
					this.CropTend,
					this.PowerTinker,
					this.MachineTinker,
					this.DropUnusedInventory,
					this.Harvest,
					this.Uproot,
					this.FarmingFabricate,
					this.PowerFabricate,
					this.Compound,
					this.Fabricate,
					this.Train,
					this.ProcessCritter,
					this.Cook,
					this.Build,
					this.Dig,
					this.BuildDig,
					this.FlipCompost,
					this.Depressurize,
					this.StressEmote,
					this.Astronaut,
					this.EmptyDesalinator,
					this.ArmTrap,
					this.FetchCritical,
					this.ResearchFetch,
					this.ExcavateFossil,
					this.AnalyzeArtifact,
					this.AnalyzeSeed,
					this.CreatureFetch,
					this.RanchingFetch,
					this.Fetch,
					this.Transport,
					this.FarmFetch,
					this.BuildFetch,
					this.CookFetch,
					this.DoctorFetch,
					this.MachineFetch,
					this.PowerFetch,
					this.FabricateFetch,
					this.FoodFetch,
					this.StorageFetch,
					this.RepairFetch,
					this.EquipmentFetch,
					this.RemoteOperate
				},
				new ChoreType[]
				{
					this.RecoverWarmth,
					this.RecoverFromHeat
				},
				new ChoreType[]
				{
					this.ReturnSuitIdle,
					this.EmoteIdle
				},
				new ChoreType[]
				{
					this.Idle
				}
			};
			string text = "";
			int num = 100000;
			foreach (ChoreType[] array3 in array)
			{
				foreach (ChoreType choreType in array3)
				{
					if (choreType.interruptPriority != 0)
					{
						text = text + "Interrupt priority set more than once: " + choreType.Id;
					}
					choreType.interruptPriority = num;
				}
				num -= 100;
			}
			if (!string.IsNullOrEmpty(text))
			{
				Debug.LogError(text);
			}
			string text2 = "";
			foreach (ChoreType choreType2 in this.resources)
			{
				if (choreType2.interruptPriority == 0)
				{
					text2 = text2 + "Interrupt priority missing for: " + choreType2.Id + "\n";
				}
			}
			if (!string.IsNullOrEmpty(text2))
			{
				Debug.LogError(text2);
			}
		}

		// Token: 0x04009064 RID: 36964
		public ChoreType Attack;

		// Token: 0x04009065 RID: 36965
		public ChoreType Capture;

		// Token: 0x04009066 RID: 36966
		public ChoreType Flee;

		// Token: 0x04009067 RID: 36967
		public ChoreType BeIncapacitated;

		// Token: 0x04009068 RID: 36968
		public ChoreType BeOffline;

		// Token: 0x04009069 RID: 36969
		public ChoreType BeBatterySaveMode;

		// Token: 0x0400906A RID: 36970
		public ChoreType DebugGoTo;

		// Token: 0x0400906B RID: 36971
		public ChoreType DeliverFood;

		// Token: 0x0400906C RID: 36972
		public ChoreType Die;

		// Token: 0x0400906D RID: 36973
		public ChoreType GeneShuffle;

		// Token: 0x0400906E RID: 36974
		public ChoreType Doctor;

		// Token: 0x0400906F RID: 36975
		public ChoreType WashHands;

		// Token: 0x04009070 RID: 36976
		public ChoreType Shower;

		// Token: 0x04009071 RID: 36977
		public ChoreType Eat;

		// Token: 0x04009072 RID: 36978
		public ChoreType ReloadElectrobank;

		// Token: 0x04009073 RID: 36979
		public ChoreType FindOxygenSourceItem;

		// Token: 0x04009074 RID: 36980
		public ChoreType BionicAbsorbOxygen;

		// Token: 0x04009075 RID: 36981
		public ChoreType UnloadElectrobank;

		// Token: 0x04009076 RID: 36982
		public ChoreType SeekAndInstallUpgrade;

		// Token: 0x04009077 RID: 36983
		public ChoreType Entombed;

		// Token: 0x04009078 RID: 36984
		public ChoreType Idle;

		// Token: 0x04009079 RID: 36985
		public ChoreType MoveToQuarantine;

		// Token: 0x0400907A RID: 36986
		public ChoreType RescueIncapacitated;

		// Token: 0x0400907B RID: 36987
		public ChoreType RecoverBreath;

		// Token: 0x0400907C RID: 36988
		public ChoreType RecoverWarmth;

		// Token: 0x0400907D RID: 36989
		public ChoreType RecoverFromHeat;

		// Token: 0x0400907E RID: 36990
		public ChoreType Sigh;

		// Token: 0x0400907F RID: 36991
		public ChoreType Sleep;

		// Token: 0x04009080 RID: 36992
		public ChoreType Narcolepsy;

		// Token: 0x04009081 RID: 36993
		public ChoreType Vomit;

		// Token: 0x04009082 RID: 36994
		public ChoreType WaterDamageZap;

		// Token: 0x04009083 RID: 36995
		public ChoreType Cough;

		// Token: 0x04009084 RID: 36996
		public ChoreType Pee;

		// Token: 0x04009085 RID: 36997
		public ChoreType ExpellGunk;

		// Token: 0x04009086 RID: 36998
		public ChoreType BreakPee;

		// Token: 0x04009087 RID: 36999
		public ChoreType TakeMedicine;

		// Token: 0x04009088 RID: 37000
		public ChoreType GetDoctored;

		// Token: 0x04009089 RID: 37001
		public ChoreType RestDueToDisease;

		// Token: 0x0400908A RID: 37002
		public ChoreType SleepDueToDisease;

		// Token: 0x0400908B RID: 37003
		public ChoreType Heal;

		// Token: 0x0400908C RID: 37004
		public ChoreType HealCritical;

		// Token: 0x0400908D RID: 37005
		public ChoreType EmoteIdle;

		// Token: 0x0400908E RID: 37006
		public ChoreType Emote;

		// Token: 0x0400908F RID: 37007
		public ChoreType EmoteHighPriority;

		// Token: 0x04009090 RID: 37008
		public ChoreType StressEmote;

		// Token: 0x04009091 RID: 37009
		public ChoreType StressActingOut;

		// Token: 0x04009092 RID: 37010
		public ChoreType Relax;

		// Token: 0x04009093 RID: 37011
		public ChoreType RadiationPain;

		// Token: 0x04009094 RID: 37012
		public ChoreType StressHeal;

		// Token: 0x04009095 RID: 37013
		public ChoreType MoveToSafety;

		// Token: 0x04009096 RID: 37014
		public ChoreType Equip;

		// Token: 0x04009097 RID: 37015
		public ChoreType Recharge;

		// Token: 0x04009098 RID: 37016
		public ChoreType Unequip;

		// Token: 0x04009099 RID: 37017
		public ChoreType Warmup;

		// Token: 0x0400909A RID: 37018
		public ChoreType Cooldown;

		// Token: 0x0400909B RID: 37019
		public ChoreType Mop;

		// Token: 0x0400909C RID: 37020
		public ChoreType Relocate;

		// Token: 0x0400909D RID: 37021
		public ChoreType Toggle;

		// Token: 0x0400909E RID: 37022
		public ChoreType Mourn;

		// Token: 0x0400909F RID: 37023
		public ChoreType Migrate;

		// Token: 0x040090A0 RID: 37024
		public ChoreType Fetch;

		// Token: 0x040090A1 RID: 37025
		public ChoreType FetchCritical;

		// Token: 0x040090A2 RID: 37026
		public ChoreType StorageFetch;

		// Token: 0x040090A3 RID: 37027
		public ChoreType Transport;

		// Token: 0x040090A4 RID: 37028
		public ChoreType RepairFetch;

		// Token: 0x040090A5 RID: 37029
		public ChoreType MachineFetch;

		// Token: 0x040090A6 RID: 37030
		public ChoreType ResearchFetch;

		// Token: 0x040090A7 RID: 37031
		public ChoreType FarmFetch;

		// Token: 0x040090A8 RID: 37032
		public ChoreType FabricateFetch;

		// Token: 0x040090A9 RID: 37033
		public ChoreType CookFetch;

		// Token: 0x040090AA RID: 37034
		public ChoreType PowerFetch;

		// Token: 0x040090AB RID: 37035
		public ChoreType BuildFetch;

		// Token: 0x040090AC RID: 37036
		public ChoreType CreatureFetch;

		// Token: 0x040090AD RID: 37037
		public ChoreType RanchingFetch;

		// Token: 0x040090AE RID: 37038
		public ChoreType FoodFetch;

		// Token: 0x040090AF RID: 37039
		public ChoreType DoctorFetch;

		// Token: 0x040090B0 RID: 37040
		public ChoreType EquipmentFetch;

		// Token: 0x040090B1 RID: 37041
		public ChoreType ArmTrap;

		// Token: 0x040090B2 RID: 37042
		public ChoreType Research;

		// Token: 0x040090B3 RID: 37043
		public ChoreType AnalyzeArtifact;

		// Token: 0x040090B4 RID: 37044
		public ChoreType AnalyzeSeed;

		// Token: 0x040090B5 RID: 37045
		public ChoreType ExcavateFossil;

		// Token: 0x040090B6 RID: 37046
		public ChoreType Disinfect;

		// Token: 0x040090B7 RID: 37047
		public ChoreType Repair;

		// Token: 0x040090B8 RID: 37048
		public ChoreType EmptyStorage;

		// Token: 0x040090B9 RID: 37049
		public ChoreType Deconstruct;

		// Token: 0x040090BA RID: 37050
		public ChoreType Demolish;

		// Token: 0x040090BB RID: 37051
		public ChoreType Art;

		// Token: 0x040090BC RID: 37052
		public ChoreType GeneratePower;

		// Token: 0x040090BD RID: 37053
		public ChoreType Harvest;

		// Token: 0x040090BE RID: 37054
		public ChoreType Uproot;

		// Token: 0x040090BF RID: 37055
		public ChoreType CleanToilet;

		// Token: 0x040090C0 RID: 37056
		public ChoreType EmptyDesalinator;

		// Token: 0x040090C1 RID: 37057
		public ChoreType LiquidCooledFan;

		// Token: 0x040090C2 RID: 37058
		public ChoreType IceCooledFan;

		// Token: 0x040090C3 RID: 37059
		public ChoreType CompostWorkable;

		// Token: 0x040090C4 RID: 37060
		public ChoreType Fabricate;

		// Token: 0x040090C5 RID: 37061
		public ChoreType FarmingFabricate;

		// Token: 0x040090C6 RID: 37062
		public ChoreType PowerFabricate;

		// Token: 0x040090C7 RID: 37063
		public ChoreType Compound;

		// Token: 0x040090C8 RID: 37064
		public ChoreType Cook;

		// Token: 0x040090C9 RID: 37065
		public ChoreType ProcessCritter;

		// Token: 0x040090CA RID: 37066
		public ChoreType Train;

		// Token: 0x040090CB RID: 37067
		public ChoreType Ranch;

		// Token: 0x040090CC RID: 37068
		public ChoreType Build;

		// Token: 0x040090CD RID: 37069
		public ChoreType BuildDig;

		// Token: 0x040090CE RID: 37070
		public ChoreType Dig;

		// Token: 0x040090CF RID: 37071
		public ChoreType FlipCompost;

		// Token: 0x040090D0 RID: 37072
		public ChoreType PowerTinker;

		// Token: 0x040090D1 RID: 37073
		public ChoreType RemoteOperate;

		// Token: 0x040090D2 RID: 37074
		public ChoreType MachineTinker;

		// Token: 0x040090D3 RID: 37075
		public ChoreType CropTend;

		// Token: 0x040090D4 RID: 37076
		public ChoreType Depressurize;

		// Token: 0x040090D5 RID: 37077
		public ChoreType DropUnusedInventory;

		// Token: 0x040090D6 RID: 37078
		public ChoreType StressVomit;

		// Token: 0x040090D7 RID: 37079
		public ChoreType MoveTo;

		// Token: 0x040090D8 RID: 37080
		public ChoreType RocketEnterExit;

		// Token: 0x040090D9 RID: 37081
		public ChoreType UglyCry;

		// Token: 0x040090DA RID: 37082
		public ChoreType BansheeWail;

		// Token: 0x040090DB RID: 37083
		public ChoreType StressShock;

		// Token: 0x040090DC RID: 37084
		public ChoreType BingeEat;

		// Token: 0x040090DD RID: 37085
		public ChoreType StressIdle;

		// Token: 0x040090DE RID: 37086
		public ChoreType ScrubOre;

		// Token: 0x040090DF RID: 37087
		public ChoreType SuitMarker;

		// Token: 0x040090E0 RID: 37088
		public ChoreType Slip;

		// Token: 0x040090E1 RID: 37089
		public ChoreType ReturnSuitUrgent;

		// Token: 0x040090E2 RID: 37090
		public ChoreType ReturnSuitIdle;

		// Token: 0x040090E3 RID: 37091
		public ChoreType Checkpoint;

		// Token: 0x040090E4 RID: 37092
		public ChoreType TravelTubeEntrance;

		// Token: 0x040090E5 RID: 37093
		public ChoreType LearnSkill;

		// Token: 0x040090E6 RID: 37094
		public ChoreType UnlearnSkill;

		// Token: 0x040090E7 RID: 37095
		public ChoreType SwitchHat;

		// Token: 0x040090E8 RID: 37096
		public ChoreType EggSing;

		// Token: 0x040090E9 RID: 37097
		public ChoreType Astronaut;

		// Token: 0x040090EA RID: 37098
		public ChoreType TopPriority;

		// Token: 0x040090EB RID: 37099
		public ChoreType JoyReaction;

		// Token: 0x040090EC RID: 37100
		public ChoreType RocketControl;

		// Token: 0x040090ED RID: 37101
		public ChoreType Party;

		// Token: 0x040090EE RID: 37102
		public ChoreType Hug;

		// Token: 0x040090EF RID: 37103
		public ChoreType OilChange;

		// Token: 0x040090F0 RID: 37104
		private int nextImplicitPriority = 10000;

		// Token: 0x040090F1 RID: 37105
		private const int INVALID_PRIORITY = -1;
	}
}
