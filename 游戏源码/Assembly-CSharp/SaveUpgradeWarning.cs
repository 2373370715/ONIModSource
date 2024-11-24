using System;
using System.Collections;
using System.Collections.Generic;
using Database;
using Klei.AI;
using Klei.CustomSettings;
using STRINGS;
using UnityEngine;

// Token: 0x0200181A RID: 6170
[AddComponentMenu("KMonoBehaviour/scripts/SaveUpgradeWarning")]
public class SaveUpgradeWarning : KMonoBehaviour
{
	// Token: 0x06007F55 RID: 32597 RVA: 0x000F3E9F File Offset: 0x000F209F
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Game game = this.game;
		game.OnLoad = (Action<Game.GameSaveData>)Delegate.Combine(game.OnLoad, new Action<Game.GameSaveData>(this.OnLoad));
	}

	// Token: 0x06007F56 RID: 32598 RVA: 0x000F3ECE File Offset: 0x000F20CE
	protected override void OnCleanUp()
	{
		Game game = this.game;
		game.OnLoad = (Action<Game.GameSaveData>)Delegate.Remove(game.OnLoad, new Action<Game.GameSaveData>(this.OnLoad));
		base.OnCleanUp();
	}

	// Token: 0x06007F57 RID: 32599 RVA: 0x0032FB40 File Offset: 0x0032DD40
	private void OnLoad(Game.GameSaveData data)
	{
		List<SaveUpgradeWarning.Upgrade> list = new List<SaveUpgradeWarning.Upgrade>
		{
			new SaveUpgradeWarning.Upgrade(7, 5, new System.Action(this.SuddenMoraleHelper)),
			new SaveUpgradeWarning.Upgrade(7, 13, new System.Action(this.BedAndBathHelper)),
			new SaveUpgradeWarning.Upgrade(7, 16, new System.Action(this.NewAutomationWarning)),
			new SaveUpgradeWarning.Upgrade(7, 32, new System.Action(this.SpaceScannersAndTelescopeUpdateWarning)),
			new SaveUpgradeWarning.Upgrade(7, 33, new System.Action(this.U50CritterWarning))
		};
		if (DlcManager.IsPureVanilla())
		{
			list.Add(new SaveUpgradeWarning.Upgrade(7, 25, new System.Action(this.MergedownWarning)));
		}
		foreach (SaveUpgradeWarning.Upgrade upgrade in list)
		{
			if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(upgrade.major, upgrade.minor))
			{
				upgrade.action();
			}
		}
	}

	// Token: 0x06007F58 RID: 32600 RVA: 0x0032FC5C File Offset: 0x0032DE5C
	private void SuddenMoraleHelper()
	{
		Effect morale_effect = Db.Get().effects.Get("SuddenMoraleHelper");
		CustomizableDialogScreen screen = Util.KInstantiateUI<CustomizableDialogScreen>(ScreenPrefabs.Instance.CustomizableDialogScreen.gameObject, GameScreenManager.Instance.ssOverlayCanvas.gameObject, true);
		screen.AddOption(UI.FRONTEND.SAVEUPGRADEWARNINGS.SUDDENMORALEHELPER_BUFF, delegate
		{
			foreach (MinionIdentity minionIdentity in Components.LiveMinionIdentities.Items)
			{
				minionIdentity.GetComponent<Effects>().Add(morale_effect, true);
			}
			screen.Deactivate();
		});
		screen.AddOption(UI.FRONTEND.SAVEUPGRADEWARNINGS.SUDDENMORALEHELPER_DISABLE, delegate
		{
			SettingConfig morale = CustomGameSettingConfigs.Morale;
			CustomGameSettings.Instance.customGameMode = CustomGameSettings.CustomGameMode.Custom;
			CustomGameSettings.Instance.SetQualitySetting(morale, morale.GetLevel("Disabled").id);
			screen.Deactivate();
		});
		screen.PopupConfirmDialog(string.Format(UI.FRONTEND.SAVEUPGRADEWARNINGS.SUDDENMORALEHELPER, Mathf.RoundToInt(morale_effect.duration / 600f)), UI.FRONTEND.SAVEUPGRADEWARNINGS.SUDDENMORALEHELPER_TITLE, null);
	}

	// Token: 0x06007F59 RID: 32601 RVA: 0x0032FD38 File Offset: 0x0032DF38
	private void BedAndBathHelper()
	{
		if (SaveGame.Instance == null)
		{
			return;
		}
		ColonyAchievementTracker colonyAchievementTracker = SaveGame.Instance.ColonyAchievementTracker;
		if (colonyAchievementTracker == null)
		{
			return;
		}
		ColonyAchievement basicComforts = Db.Get().ColonyAchievements.BasicComforts;
		ColonyAchievementStatus colonyAchievementStatus = null;
		if (colonyAchievementTracker.achievements.TryGetValue(basicComforts.Id, out colonyAchievementStatus))
		{
			colonyAchievementStatus.failed = false;
		}
	}

	// Token: 0x06007F5A RID: 32602 RVA: 0x0032FD98 File Offset: 0x0032DF98
	private void NewAutomationWarning()
	{
		SpriteListDialogScreen screen = Util.KInstantiateUI<SpriteListDialogScreen>(ScreenPrefabs.Instance.SpriteListDialogScreen.gameObject, GameScreenManager.Instance.ssOverlayCanvas.gameObject, true);
		screen.AddOption(UI.CONFIRMDIALOG.OK, delegate
		{
			screen.Deactivate();
		});
		string[] array = SaveUpgradeWarning.buildingIDsWithNewPorts;
		for (int i = 0; i < array.Length; i++)
		{
			BuildingDef buildingDef = Assets.GetBuildingDef(array[i]);
			screen.AddListRow(buildingDef.GetUISprite("ui", false), buildingDef.Name, 150f, 50f);
		}
		screen.PopupConfirmDialog(UI.FRONTEND.SAVEUPGRADEWARNINGS.NEWAUTOMATIONWARNING, UI.FRONTEND.SAVEUPGRADEWARNINGS.NEWAUTOMATIONWARNING_TITLE);
		base.StartCoroutine(this.SendAutomationWarningNotifications());
	}

	// Token: 0x06007F5B RID: 32603 RVA: 0x000F3EFD File Offset: 0x000F20FD
	private IEnumerator SendAutomationWarningNotifications()
	{
		yield return SequenceUtil.WaitForEndOfFrame;
		if (Components.BuildingCompletes.Count == 0)
		{
			global::Debug.LogWarning("Could not send automation warnings because buildings have not yet loaded");
		}
		using (IEnumerator enumerator = Components.BuildingCompletes.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				BuildingComplete buildingComplete = (BuildingComplete)obj;
				foreach (string text in SaveUpgradeWarning.buildingIDsWithNewPorts)
				{
					BuildingDef buildingDef = Assets.GetBuildingDef(text);
					if (buildingComplete.Def == buildingDef)
					{
						List<ILogicUIElement> list = new List<ILogicUIElement>();
						LogicPorts component = buildingComplete.GetComponent<LogicPorts>();
						if (component.outputPorts != null)
						{
							list.AddRange(component.outputPorts);
						}
						if (component.inputPorts != null)
						{
							list.AddRange(component.inputPorts);
						}
						foreach (ILogicUIElement logicUIElement in list)
						{
							if (Grid.Objects[logicUIElement.GetLogicUICell(), 31] != null)
							{
								global::Debug.Log("Triggering automation warning for building of type " + text);
								GenericMessage message = new GenericMessage(MISC.NOTIFICATIONS.NEW_AUTOMATION_WARNING.NAME, MISC.NOTIFICATIONS.NEW_AUTOMATION_WARNING.TOOLTIP, MISC.NOTIFICATIONS.NEW_AUTOMATION_WARNING.TOOLTIP, buildingComplete);
								Messenger.Instance.QueueMessage(message);
							}
						}
					}
				}
			}
			yield break;
		}
		yield break;
	}

	// Token: 0x06007F5C RID: 32604 RVA: 0x000F3F05 File Offset: 0x000F2105
	private IEnumerator TemporaryDisableMeteorShowers(float timeOffDurationInCycles)
	{
		yield return SequenceUtil.WaitForEndOfFrame;
		float sleepTimer = GameUtil.GetCurrentTimeInCycles() + timeOffDurationInCycles;
		foreach (GameplayEvent gameplayEvent in Db.Get().GameplayEvents.resources)
		{
			if (gameplayEvent is MeteorShowerEvent && !(gameplayEvent.Id == Db.Get().GameplayEvents.GassyMooteorEvent.Id))
			{
				gameplayEvent.SetSleepTimer(sleepTimer);
			}
		}
		if (DlcManager.IsPureVanilla())
		{
			List<GameplayEventInstance> list = new List<GameplayEventInstance>();
			GameplayEventManager.Instance.GetActiveEventsOfType<MeteorShowerEvent>(ref list);
			using (List<GameplayEventInstance>.Enumerator enumerator2 = list.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					GameplayEventInstance eventInstance = enumerator2.Current;
					GameplayEventManager.Instance.RemoveActiveEvent(eventInstance, "Cancelled by SaveUpgradeWarning for player's convenience by providing a window of time without meteors to allow players to adapt to new updates made to relevant buildings");
				}
				yield break;
			}
		}
		yield break;
	}

	// Token: 0x06007F5D RID: 32605 RVA: 0x0032FE68 File Offset: 0x0032E068
	private void SpaceScannersAndTelescopeUpdateWarning()
	{
		SpriteListDialogScreen screen = Util.KInstantiateUI<SpriteListDialogScreen>(ScreenPrefabs.Instance.SpriteListDialogScreen.gameObject, GameScreenManager.Instance.ssOverlayCanvas.gameObject, true);
		screen.AddOption(UI.CONFIRMDIALOG.OK, delegate
		{
			screen.Deactivate();
		});
		screen.AddListRow(Assets.GetSprite("space_scanner_range"), UI.FRONTEND.SAVEUPGRADEWARNINGS.SPACESCANNERANDTELESCOPECHANGES_SPACESCANNERS, 150f, 120f);
		screen.AddListRow(Assets.GetSprite("telescope_range"), UI.FRONTEND.SAVEUPGRADEWARNINGS.SPACESCANNERANDTELESCOPECHANGES_TELESCOPES, 150f, 120f);
		screen.PopupConfirmDialog(UI.FRONTEND.SAVEUPGRADEWARNINGS.SPACESCANNERANDTELESCOPECHANGES_SUMMARY + "\n\n" + UI.FRONTEND.SAVEUPGRADEWARNINGS.SPACESCANNERANDTELESCOPECHANGES_WARNING, UI.FRONTEND.SAVEUPGRADEWARNINGS.SPACESCANNERANDTELESCOPECHANGES_TITLE);
		base.StartCoroutine(this.TemporaryDisableMeteorShowers(20f));
	}

	// Token: 0x06007F5E RID: 32606 RVA: 0x0032FF68 File Offset: 0x0032E168
	private void U50CritterWarning()
	{
		SpriteListDialogScreen screen = Util.KInstantiateUI<SpriteListDialogScreen>(ScreenPrefabs.Instance.SpriteListDialogScreen.gameObject, GameScreenManager.Instance.ssOverlayCanvas.gameObject, true);
		screen.AddOption(UI.CONFIRMDIALOG.OK, delegate
		{
			screen.Deactivate();
		});
		screen.AddListRow(Assets.GetSprite("u50_critter_moods"), UI.FRONTEND.SAVEUPGRADEWARNINGS.U50_CHANGES_MOOD, 150f, 120f);
		screen.AddListRow(Assets.GetSprite("u50_pacu"), UI.FRONTEND.SAVEUPGRADEWARNINGS.U50_CHANGES_PACU, 150f, 120f);
		screen.AddListRow(Assets.GetSprite("u50_suit_checkpoints"), UI.FRONTEND.SAVEUPGRADEWARNINGS.U50_CHANGES_SUITCHECKPOINTS, 150f, 120f);
		screen.PopupConfirmDialog(UI.FRONTEND.SAVEUPGRADEWARNINGS.U50_CHANGES_SUMMARY, UI.FRONTEND.SAVEUPGRADEWARNINGS.U50_CHANGES_TITLE);
	}

	// Token: 0x06007F5F RID: 32607 RVA: 0x00330070 File Offset: 0x0032E270
	private void MergedownWarning()
	{
		SpriteListDialogScreen screen = Util.KInstantiateUI<SpriteListDialogScreen>(ScreenPrefabs.Instance.SpriteListDialogScreen.gameObject, GameScreenManager.Instance.ssOverlayCanvas.gameObject, true);
		screen.AddOption(UI.DEVELOPMENTBUILDS.FULL_PATCH_NOTES, delegate
		{
			App.OpenWebURL("https://forums.kleientertainment.com/game-updates/oni-alpha/");
		});
		screen.AddOption(UI.CONFIRMDIALOG.OK, delegate
		{
			screen.Deactivate();
		});
		screen.AddListRow(Assets.GetSprite("upgrade_mergedown_fridge"), UI.FRONTEND.SAVEUPGRADEWARNINGS.MERGEDOWNCHANGES_FOOD, 150f, 120f);
		screen.AddListRow(Assets.GetSprite("upgrade_mergedown_deodorizer"), UI.FRONTEND.SAVEUPGRADEWARNINGS.MERGEDOWNCHANGES_AIRFILTER, 150f, 120f);
		screen.AddListRow(Assets.GetSprite("upgrade_mergedown_steamturbine"), UI.FRONTEND.SAVEUPGRADEWARNINGS.MERGEDOWNCHANGES_SIMULATION, 150f, 120f);
		screen.AddListRow(Assets.GetSprite("upgrade_mergedown_oxygen_meter"), UI.FRONTEND.SAVEUPGRADEWARNINGS.MERGEDOWNCHANGES_BUILDINGS, 150f, 120f);
		screen.PopupConfirmDialog(UI.FRONTEND.SAVEUPGRADEWARNINGS.MERGEDOWNCHANGES, UI.FRONTEND.SAVEUPGRADEWARNINGS.MERGEDOWNCHANGES_TITLE);
		base.StartCoroutine(this.SendAutomationWarningNotifications());
	}

	// Token: 0x0400609A RID: 24730
	[MyCmpReq]
	private Game game;

	// Token: 0x0400609B RID: 24731
	private static string[] buildingIDsWithNewPorts = new string[]
	{
		"LiquidVent",
		"GasVent",
		"GasVentHighPressure",
		"SolidVent",
		"LiquidReservoir",
		"GasReservoir"
	};

	// Token: 0x0200181B RID: 6171
	private struct Upgrade
	{
		// Token: 0x06007F62 RID: 32610 RVA: 0x000F3F51 File Offset: 0x000F2151
		public Upgrade(int major, int minor, System.Action action)
		{
			this.major = major;
			this.minor = minor;
			this.action = action;
		}

		// Token: 0x0400609C RID: 24732
		public int major;

		// Token: 0x0400609D RID: 24733
		public int minor;

		// Token: 0x0400609E RID: 24734
		public System.Action action;
	}
}
