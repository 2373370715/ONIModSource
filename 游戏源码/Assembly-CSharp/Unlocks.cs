using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using ProcGen;
using STRINGS;
using UnityEngine;

// Token: 0x020019FB RID: 6651
[AddComponentMenu("KMonoBehaviour/scripts/Unlocks")]
public class Unlocks : KMonoBehaviour
{
	// Token: 0x1700090F RID: 2319
	// (get) Token: 0x06008A90 RID: 35472 RVA: 0x000FAB44 File Offset: 0x000F8D44
	private static string UnlocksFilename
	{
		get
		{
			return System.IO.Path.Combine(global::Util.RootFolder(), "unlocks.json");
		}
	}

	// Token: 0x06008A91 RID: 35473 RVA: 0x000FAB55 File Offset: 0x000F8D55
	protected override void OnPrefabInit()
	{
		this.LoadUnlocks();
	}

	// Token: 0x06008A92 RID: 35474 RVA: 0x0035B8C8 File Offset: 0x00359AC8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.UnlockCycleCodexes();
		GameClock.Instance.Subscribe(631075836, new Action<object>(this.OnNewDay));
		base.Subscribe<Unlocks>(-1277991738, Unlocks.OnLaunchRocketDelegate);
		base.Subscribe<Unlocks>(282337316, Unlocks.OnDuplicantDiedDelegate);
		base.Subscribe<Unlocks>(-818188514, Unlocks.OnDiscoveredSpaceDelegate);
		Components.LiveMinionIdentities.OnAdd += this.OnNewDupe;
	}

	// Token: 0x06008A93 RID: 35475 RVA: 0x000FAB5D File Offset: 0x000F8D5D
	public bool IsUnlocked(string unlockID)
	{
		return !string.IsNullOrEmpty(unlockID) && (DebugHandler.InstantBuildMode || this.unlocked.Contains(unlockID));
	}

	// Token: 0x06008A94 RID: 35476 RVA: 0x000FAB7E File Offset: 0x000F8D7E
	public IReadOnlyList<string> GetAllUnlockedIds()
	{
		return this.unlocked;
	}

	// Token: 0x06008A95 RID: 35477 RVA: 0x000FAB86 File Offset: 0x000F8D86
	public void Lock(string unlockID)
	{
		if (this.unlocked.Contains(unlockID))
		{
			this.unlocked.Remove(unlockID);
			this.SaveUnlocks();
			Game.Instance.Trigger(1594320620, unlockID);
		}
	}

	// Token: 0x06008A96 RID: 35478 RVA: 0x0035B948 File Offset: 0x00359B48
	public void Unlock(string unlockID, bool shouldTryShowCodexNotification = true)
	{
		if (string.IsNullOrEmpty(unlockID))
		{
			DebugUtil.DevAssert(false, "Unlock called with null or empty string", null);
			return;
		}
		if (!this.unlocked.Contains(unlockID))
		{
			this.unlocked.Add(unlockID);
			this.SaveUnlocks();
			Game.Instance.Trigger(1594320620, unlockID);
			if (shouldTryShowCodexNotification)
			{
				MessageNotification messageNotification = this.GenerateCodexUnlockNotification(unlockID);
				if (messageNotification != null)
				{
					base.GetComponent<Notifier>().Add(messageNotification, "");
				}
			}
		}
		this.EvalMetaCategories();
	}

	// Token: 0x06008A97 RID: 35479 RVA: 0x0035B9C0 File Offset: 0x00359BC0
	private void EvalMetaCategories()
	{
		foreach (Unlocks.MetaUnlockCategory metaUnlockCategory in this.MetaUnlockCategories)
		{
			string metaCollectionID = metaUnlockCategory.metaCollectionID;
			Unlocks.<>c__DisplayClass14_0 CS$<>8__locals1;
			CS$<>8__locals1.mesaCollectionID = metaUnlockCategory.mesaCollectionID;
			int mesaUnlockCount = metaUnlockCategory.mesaUnlockCount;
			CS$<>8__locals1.count = 0;
			CS$<>8__locals1.isCollectionReplaced = false;
			if (SaveLoader.Instance != null)
			{
				foreach (LoreCollectionOverride loreUnlock in SaveLoader.Instance.ClusterLayout.clusterUnlocks)
				{
					if (this.<EvalMetaCategories>g__EvaluateCollection|14_0(loreUnlock, ref CS$<>8__locals1))
					{
						break;
					}
				}
				foreach (string name in CustomGameSettings.Instance.GetCurrentDlcMixingIds())
				{
					DlcMixingSettings cachedDlcMixingSettings = SettingsCache.GetCachedDlcMixingSettings(name);
					if (cachedDlcMixingSettings != null)
					{
						foreach (LoreCollectionOverride loreUnlock2 in cachedDlcMixingSettings.globalLoreUnlocks)
						{
							if (this.<EvalMetaCategories>g__EvaluateCollection|14_0(loreUnlock2, ref CS$<>8__locals1))
							{
								break;
							}
						}
					}
				}
			}
			if (!CS$<>8__locals1.isCollectionReplaced)
			{
				foreach (string unlockID in this.lockCollections[CS$<>8__locals1.mesaCollectionID])
				{
					if (this.IsUnlocked(unlockID))
					{
						int count = CS$<>8__locals1.count;
						CS$<>8__locals1.count = count + 1;
					}
				}
			}
			if (CS$<>8__locals1.count >= mesaUnlockCount)
			{
				this.UnlockNext(metaCollectionID, false);
			}
		}
	}

	// Token: 0x06008A98 RID: 35480 RVA: 0x0035BBD0 File Offset: 0x00359DD0
	private void SaveUnlocks()
	{
		if (!Directory.Exists(global::Util.RootFolder()))
		{
			Directory.CreateDirectory(global::Util.RootFolder());
		}
		string s = JsonConvert.SerializeObject(this.unlocked);
		bool flag = false;
		int num = 0;
		while (!flag && num < 5)
		{
			try
			{
				Thread.Sleep(num * 100);
				using (FileStream fileStream = File.Open(Unlocks.UnlocksFilename, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
				{
					flag = true;
					byte[] bytes = new ASCIIEncoding().GetBytes(s);
					fileStream.Write(bytes, 0, bytes.Length);
				}
			}
			catch (Exception ex)
			{
				global::Debug.LogWarningFormat("Failed to save Unlocks attempt {0}: {1}", new object[]
				{
					num + 1,
					ex.ToString()
				});
			}
			num++;
		}
	}

	// Token: 0x06008A99 RID: 35481 RVA: 0x0035BC98 File Offset: 0x00359E98
	public void LoadUnlocks()
	{
		this.unlocked.Clear();
		if (!File.Exists(Unlocks.UnlocksFilename))
		{
			return;
		}
		string text = "";
		bool flag = false;
		int num = 0;
		while (!flag && num < 5)
		{
			try
			{
				Thread.Sleep(num * 100);
				using (FileStream fileStream = File.Open(Unlocks.UnlocksFilename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
				{
					flag = true;
					ASCIIEncoding asciiencoding = new ASCIIEncoding();
					byte[] array = new byte[fileStream.Length];
					if ((long)fileStream.Read(array, 0, array.Length) == fileStream.Length)
					{
						text += asciiencoding.GetString(array);
					}
				}
			}
			catch (Exception ex)
			{
				global::Debug.LogWarningFormat("Failed to load Unlocks attempt {0}: {1}", new object[]
				{
					num + 1,
					ex.ToString()
				});
			}
			num++;
		}
		if (string.IsNullOrEmpty(text))
		{
			return;
		}
		try
		{
			foreach (string text2 in JsonConvert.DeserializeObject<string[]>(text))
			{
				if (!string.IsNullOrEmpty(text2) && !this.unlocked.Contains(text2))
				{
					this.unlocked.Add(text2);
				}
			}
		}
		catch (Exception ex2)
		{
			global::Debug.LogErrorFormat("Error parsing unlocks file [{0}]: {1}", new object[]
			{
				Unlocks.UnlocksFilename,
				ex2.ToString()
			});
		}
	}

	// Token: 0x06008A9A RID: 35482 RVA: 0x0035BE08 File Offset: 0x0035A008
	private string GetNextClusterUnlock(string collectionID, out LoreCollectionOverride.OrderRule orderRule, bool randomize)
	{
		foreach (LoreCollectionOverride loreCollectionOverride in SaveLoader.Instance.ClusterLayout.clusterUnlocks)
		{
			if (!(loreCollectionOverride.id != collectionID))
			{
				if (!this.lockCollections.ContainsKey(collectionID))
				{
					DebugUtil.DevLogError("Lore collection '" + collectionID + "' is missing");
					orderRule = LoreCollectionOverride.OrderRule.Invalid;
					return null;
				}
				string[] array = this.lockCollections[loreCollectionOverride.collection];
				if (randomize)
				{
					array.Shuffle<string>();
				}
				foreach (string text in array)
				{
					if (!this.IsUnlocked(text))
					{
						orderRule = loreCollectionOverride.orderRule;
						return text;
					}
				}
				if (loreCollectionOverride.orderRule == LoreCollectionOverride.OrderRule.Replace)
				{
					orderRule = loreCollectionOverride.orderRule;
					return null;
				}
			}
		}
		orderRule = LoreCollectionOverride.OrderRule.Invalid;
		return null;
	}

	// Token: 0x06008A9B RID: 35483 RVA: 0x0035BF0C File Offset: 0x0035A10C
	private string GetNextGlobalDlcUnlock(string collectionID, out LoreCollectionOverride.OrderRule orderRule, bool randomize)
	{
		foreach (string name in CustomGameSettings.Instance.GetCurrentDlcMixingIds())
		{
			DlcMixingSettings cachedDlcMixingSettings = SettingsCache.GetCachedDlcMixingSettings(name);
			if (cachedDlcMixingSettings != null)
			{
				foreach (LoreCollectionOverride loreCollectionOverride in cachedDlcMixingSettings.globalLoreUnlocks)
				{
					if (!(loreCollectionOverride.id != collectionID))
					{
						if (!this.lockCollections.ContainsKey(collectionID))
						{
							DebugUtil.DevLogError("Lore collection '" + collectionID + "' is missing");
							orderRule = LoreCollectionOverride.OrderRule.Invalid;
							return null;
						}
						string[] array = this.lockCollections[loreCollectionOverride.collection];
						if (randomize)
						{
							array.Shuffle<string>();
						}
						foreach (string text in array)
						{
							if (!this.IsUnlocked(text))
							{
								orderRule = loreCollectionOverride.orderRule;
								return text;
							}
						}
						if (loreCollectionOverride.orderRule == LoreCollectionOverride.OrderRule.Replace)
						{
							orderRule = loreCollectionOverride.orderRule;
							return null;
						}
					}
				}
			}
		}
		orderRule = LoreCollectionOverride.OrderRule.Invalid;
		return null;
	}

	// Token: 0x06008A9C RID: 35484 RVA: 0x0035C078 File Offset: 0x0035A278
	public string UnlockNext(string collectionID, bool randomize = false)
	{
		if (SaveLoader.Instance != null)
		{
			LoreCollectionOverride.OrderRule orderRule;
			string text = this.GetNextClusterUnlock(collectionID, out orderRule, randomize);
			if (text != null && (orderRule == LoreCollectionOverride.OrderRule.Prepend || orderRule == LoreCollectionOverride.OrderRule.Replace))
			{
				this.Unlock(text, true);
				return text;
			}
			LoreCollectionOverride.OrderRule orderRule2;
			text = this.GetNextGlobalDlcUnlock(collectionID, out orderRule2, randomize);
			if (text != null && (orderRule2 == LoreCollectionOverride.OrderRule.Prepend || orderRule2 == LoreCollectionOverride.OrderRule.Replace))
			{
				this.Unlock(text, true);
				return text;
			}
			if (orderRule == LoreCollectionOverride.OrderRule.Replace || orderRule2 == LoreCollectionOverride.OrderRule.Replace)
			{
				return null;
			}
		}
		string[] array = this.lockCollections[collectionID];
		if (randomize)
		{
			array.Shuffle<string>();
		}
		foreach (string text2 in array)
		{
			if (string.IsNullOrEmpty(text2))
			{
				DebugUtil.DevAssertArgs(false, new object[]
				{
					"Found null/empty string in Unlocks collection: ",
					collectionID
				});
			}
			else if (!this.IsUnlocked(text2))
			{
				this.Unlock(text2, true);
				return text2;
			}
		}
		if (SaveLoader.Instance != null)
		{
			LoreCollectionOverride.OrderRule orderRule3;
			string text3 = this.GetNextClusterUnlock(collectionID, out orderRule3, randomize);
			if (text3 != null && orderRule3 == LoreCollectionOverride.OrderRule.Append)
			{
				this.Unlock(text3, true);
				return text3;
			}
			text3 = this.GetNextGlobalDlcUnlock(collectionID, out orderRule3, randomize);
			if (text3 != null && orderRule3 == LoreCollectionOverride.OrderRule.Append)
			{
				this.Unlock(text3, true);
				return text3;
			}
		}
		return null;
	}

	// Token: 0x06008A9D RID: 35485 RVA: 0x0035C198 File Offset: 0x0035A398
	private MessageNotification GenerateCodexUnlockNotification(string lockID)
	{
		string entryForLock = CodexCache.GetEntryForLock(lockID);
		if (string.IsNullOrEmpty(entryForLock))
		{
			return null;
		}
		string text = null;
		if (CodexCache.FindSubEntry(lockID) != null)
		{
			text = CodexCache.FindSubEntry(lockID).title;
		}
		else if (CodexCache.FindSubEntry(entryForLock) != null)
		{
			text = CodexCache.FindSubEntry(entryForLock).title;
		}
		else if (CodexCache.FindEntry(entryForLock) != null)
		{
			text = CodexCache.FindEntry(entryForLock).title;
		}
		string text2 = UI.FormatAsLink(Strings.Get(text), entryForLock);
		if (!string.IsNullOrEmpty(text))
		{
			ContentContainer contentContainer = CodexCache.FindEntry(entryForLock).contentContainers.Find((ContentContainer match) => match.lockID == lockID);
			if (contentContainer != null)
			{
				foreach (ICodexWidget codexWidget in contentContainer.content)
				{
					CodexText codexText = codexWidget as CodexText;
					if (codexText != null)
					{
						text2 = text2 + "\n\n" + codexText.text;
					}
				}
			}
			return new MessageNotification(new CodexUnlockedMessage(lockID, text2));
		}
		return null;
	}

	// Token: 0x06008A9E RID: 35486 RVA: 0x0035C2C4 File Offset: 0x0035A4C4
	private void UnlockCycleCodexes()
	{
		foreach (KeyValuePair<int, string> keyValuePair in this.cycleLocked)
		{
			if (GameClock.Instance.GetCycle() + 1 >= keyValuePair.Key)
			{
				this.Unlock(keyValuePair.Value, true);
			}
		}
	}

	// Token: 0x06008A9F RID: 35487 RVA: 0x000FABB9 File Offset: 0x000F8DB9
	private void OnNewDay(object data)
	{
		this.UnlockCycleCodexes();
	}

	// Token: 0x06008AA0 RID: 35488 RVA: 0x000FABC1 File Offset: 0x000F8DC1
	private void OnLaunchRocket(object data)
	{
		this.Unlock("surfacebreach", true);
		this.Unlock("firstrocketlaunch", true);
	}

	// Token: 0x06008AA1 RID: 35489 RVA: 0x000FABDB File Offset: 0x000F8DDB
	private void OnDuplicantDied(object data)
	{
		this.Unlock("duplicantdeath", true);
		if (Components.LiveMinionIdentities.Count == 1)
		{
			this.Unlock("onedupeleft", true);
		}
	}

	// Token: 0x06008AA2 RID: 35490 RVA: 0x000FAC02 File Offset: 0x000F8E02
	private void OnNewDupe(MinionIdentity minion_identity)
	{
		if (Components.LiveMinionIdentities.Count >= Db.Get().Personalities.GetAll(true, false).Count)
		{
			this.Unlock("fulldupecolony", true);
		}
	}

	// Token: 0x06008AA3 RID: 35491 RVA: 0x000FAC32 File Offset: 0x000F8E32
	private void OnDiscoveredSpace(object data)
	{
		this.Unlock("surfacebreach", true);
	}

	// Token: 0x06008AA4 RID: 35492 RVA: 0x0035C334 File Offset: 0x0035A534
	public void Sim4000ms(float dt)
	{
		int x = int.MinValue;
		int num = int.MinValue;
		int x2 = int.MaxValue;
		int num2 = int.MaxValue;
		foreach (MinionIdentity minionIdentity in Components.MinionIdentities.Items)
		{
			if (!(minionIdentity == null))
			{
				int cell = Grid.PosToCell(minionIdentity);
				if (Grid.IsValidCell(cell))
				{
					int num3;
					int num4;
					Grid.CellToXY(cell, out num3, out num4);
					if (num4 > num)
					{
						num = num4;
						x = num3;
					}
					if (num4 < num2)
					{
						x2 = num3;
						num2 = num4;
					}
				}
			}
		}
		if (num != -2147483648)
		{
			int num5 = num;
			for (int i = 0; i < 30; i++)
			{
				num5++;
				int cell2 = Grid.XYToCell(x, num5);
				if (!Grid.IsValidCell(cell2))
				{
					break;
				}
				if (global::World.Instance.zoneRenderData.GetSubWorldZoneType(cell2) == SubWorld.ZoneType.Space)
				{
					this.Unlock("nearingsurface", true);
					break;
				}
			}
		}
		if (num2 != 2147483647)
		{
			int num6 = num2;
			for (int j = 0; j < 30; j++)
			{
				num6--;
				int num7 = Grid.XYToCell(x2, num6);
				if (!Grid.IsValidCell(num7))
				{
					break;
				}
				if (global::World.Instance.zoneRenderData.GetSubWorldZoneType(num7) == SubWorld.ZoneType.ToxicJungle && Grid.Element[num7].id == SimHashes.Magma)
				{
					this.Unlock("nearingmagma", true);
					return;
				}
			}
		}
	}

	// Token: 0x06008AA7 RID: 35495 RVA: 0x0035CB64 File Offset: 0x0035AD64
	[CompilerGenerated]
	private bool <EvalMetaCategories>g__EvaluateCollection|14_0(LoreCollectionOverride loreUnlock, ref Unlocks.<>c__DisplayClass14_0 A_2)
	{
		if (loreUnlock.id == A_2.mesaCollectionID)
		{
			foreach (string unlockID in this.lockCollections[loreUnlock.collection])
			{
				if (this.IsUnlocked(unlockID))
				{
					int count = A_2.count;
					A_2.count = count + 1;
				}
			}
			if (loreUnlock.orderRule == LoreCollectionOverride.OrderRule.Replace)
			{
				A_2.isCollectionReplaced = true;
				return true;
			}
		}
		return false;
	}

	// Token: 0x04006854 RID: 26708
	private const int FILE_IO_RETRY_ATTEMPTS = 5;

	// Token: 0x04006855 RID: 26709
	private List<string> unlocked = new List<string>();

	// Token: 0x04006856 RID: 26710
	private List<Unlocks.MetaUnlockCategory> MetaUnlockCategories = new List<Unlocks.MetaUnlockCategory>
	{
		new Unlocks.MetaUnlockCategory("dimensionalloreMeta", "dimensionallore", 4)
	};

	// Token: 0x04006857 RID: 26711
	public Dictionary<string, string[]> lockCollections = new Dictionary<string, string[]>
	{
		{
			"emails",
			new string[]
			{
				"email_thermodynamiclaws",
				"email_security2",
				"email_pens2",
				"email_atomiconrecruitment",
				"email_devonsblog",
				"email_researchgiant",
				"email_thejanitor",
				"email_newemployee",
				"email_timeoffapproved",
				"email_security3",
				"email_preliminarycalculations",
				"email_hollandsdog",
				"email_temporalbowupdate",
				"email_retemporalbowupdate",
				"email_memorychip",
				"email_arthistoryrequest",
				"email_AIcontrol",
				"email_AIcontrol2",
				"email_friendlyemail",
				"email_AIcontrol3",
				"email_AIcontrol4",
				"email_engineeringcandidate",
				"email_missingnotes",
				"email_journalistrequest",
				"email_journalistrequest2"
			}
		},
		{
			"dlc2emails",
			new string[]
			{
				"email_newbaby",
				"email_cerestourism1",
				"email_cerestourism2",
				"email_voicemail",
				"email_expelled"
			}
		},
		{
			"dlc3emails",
			new string[]
			{
				"email_ulti"
			}
		},
		{
			"journals",
			new string[]
			{
				"journal_timesarrowthoughts",
				"journal_A046_1",
				"journal_B835_1",
				"journal_sunflowerseeds",
				"journal_B327_1",
				"journal_B556_1",
				"journal_employeeprocessing",
				"journal_B327_2",
				"journal_A046_2",
				"journal_elliesbirthday1",
				"journal_B835_2",
				"journal_ants",
				"journal_pipedream",
				"journal_B556_2",
				"journal_movedrats",
				"journal_B835_3",
				"journal_A046_3",
				"journal_B556_3",
				"journal_B327_3",
				"journal_B835_4",
				"journal_cleanup",
				"journal_A046_4",
				"journal_B327_4",
				"journal_revisitednumbers",
				"journal_B556_4",
				"journal_B835_5",
				"journal_elliesbirthday2",
				"journal_B111_1",
				"journal_revisitednumbers2",
				"journal_timemusings",
				"journal_evil",
				"journal_timesorder",
				"journal_inspace",
				"journal_mysteryaward",
				"journal_courier"
			}
		},
		{
			"dlc3journals",
			new string[]
			{
				"journal_potatobattery1",
				"journal_potatobattery2",
				"journal_potatobattery3"
			}
		},
		{
			"researchnotes",
			new string[]
			{
				"notes_clonedrats",
				"misc_dishbot",
				"notes_agriculture1",
				"notes_husbandry1",
				"notes_hibiscus3",
				"misc_newsecurity",
				"notes_husbandry2",
				"notes_agriculture2",
				"notes_geneticooze",
				"notes_agriculture3",
				"notes_husbandry3",
				"misc_casualfriday",
				"notes_memoryimplantation",
				"notes_husbandry4",
				"notes_agriculture4",
				"notes_neutronium",
				"misc_mailroometiquette",
				"notes_firstsuccess",
				"misc_reminder",
				"notes_neutroniumapplications",
				"notes_teleportation",
				"notes_AI",
				"misc_politerequest",
				"cryotank_warning",
				"misc_unattendedcultures"
			}
		},
		{
			"dlc2researchnotes",
			new string[]
			{
				"notes_cleanup"
			}
		},
		{
			"dlc3researchnotes",
			new string[]
			{
				"notes_talkshow",
				"notes_remoteworkstation"
			}
		},
		{
			"dimensionallore",
			new string[]
			{
				"notes_clonedrabbits",
				"notes_clonedraccoons",
				"journal_movedrabbits",
				"journal_movedraccoons",
				"journal_strawberries",
				"journal_shrimp"
			}
		},
		{
			"dimensionalloreMeta",
			new string[]
			{
				"log9"
			}
		},
		{
			"dlc2dimensionallore",
			new string[]
			{
				"notes_tragicnews",
				"notes_tragicnews2",
				"notes_tragicnews3"
			}
		},
		{
			"dlc2archivebuilding",
			new string[]
			{
				"notes_welcometoceres"
			}
		},
		{
			"dlc2geoplantinput",
			new string[]
			{
				"notes_geoinputs"
			}
		},
		{
			"dlc2geoplantcomplete",
			new string[]
			{
				"notes_earthquake"
			}
		},
		{
			"space",
			new string[]
			{
				"display_spaceprop1",
				"notice_pilot",
				"journal_inspace",
				"notes_firstcolony"
			}
		},
		{
			"storytraits",
			new string[]
			{
				"story_trait_critter_manipulator_initial",
				"story_trait_critter_manipulator_complete",
				"storytrait_crittermanipulator_workiversary",
				"story_trait_mega_brain_tank_initial",
				"story_trait_mega_brain_tank_competed",
				"story_trait_fossilhunt_initial",
				"story_trait_fossilhunt_poi1",
				"story_trait_fossilhunt_poi2",
				"story_trait_fossilhunt_poi3",
				"story_trait_fossilhunt_complete",
				"story_trait_morbrover_initial",
				"story_trait_morbrover_reveal",
				"story_trait_morbrover_reveal_lore",
				"story_trait_morbrover_complete",
				"story_trait_morbrover_complete_lore",
				"story_trait_morbrover_biobot",
				"story_trait_morbrover_locker"
			}
		}
	};

	// Token: 0x04006858 RID: 26712
	public Dictionary<int, string> cycleLocked = new Dictionary<int, string>
	{
		{
			0,
			"log1"
		},
		{
			3,
			"log2"
		},
		{
			15,
			"log3"
		},
		{
			1000,
			"log4"
		},
		{
			1500,
			"log4b"
		},
		{
			2000,
			"log5"
		},
		{
			2500,
			"log5b"
		},
		{
			3000,
			"log6"
		},
		{
			3500,
			"log6b"
		},
		{
			4000,
			"log7"
		},
		{
			4001,
			"log8"
		}
	};

	// Token: 0x04006859 RID: 26713
	private static readonly EventSystem.IntraObjectHandler<Unlocks> OnLaunchRocketDelegate = new EventSystem.IntraObjectHandler<Unlocks>(delegate(Unlocks component, object data)
	{
		component.OnLaunchRocket(data);
	});

	// Token: 0x0400685A RID: 26714
	private static readonly EventSystem.IntraObjectHandler<Unlocks> OnDuplicantDiedDelegate = new EventSystem.IntraObjectHandler<Unlocks>(delegate(Unlocks component, object data)
	{
		component.OnDuplicantDied(data);
	});

	// Token: 0x0400685B RID: 26715
	private static readonly EventSystem.IntraObjectHandler<Unlocks> OnDiscoveredSpaceDelegate = new EventSystem.IntraObjectHandler<Unlocks>(delegate(Unlocks component, object data)
	{
		component.OnDiscoveredSpace(data);
	});

	// Token: 0x020019FC RID: 6652
	private class MetaUnlockCategory
	{
		// Token: 0x06008AA8 RID: 35496 RVA: 0x000FAC40 File Offset: 0x000F8E40
		public MetaUnlockCategory(string metaCollectionID, string mesaCollectionID, int mesaUnlockCount)
		{
			this.metaCollectionID = metaCollectionID;
			this.mesaCollectionID = mesaCollectionID;
			this.mesaUnlockCount = mesaUnlockCount;
		}

		// Token: 0x0400685C RID: 26716
		public string metaCollectionID;

		// Token: 0x0400685D RID: 26717
		public string mesaCollectionID;

		// Token: 0x0400685E RID: 26718
		public int mesaUnlockCount;
	}
}
