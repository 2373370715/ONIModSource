using System;
using System.Runtime.CompilerServices;
using FMODUnity;
using Klei.AI;
using ProcGenGame;
using UnityEngine;

// Token: 0x02001AF8 RID: 6904
public class NewBaseScreen : KScreen
{
	// Token: 0x060090C9 RID: 37065 RVA: 0x000B4E11 File Offset: 0x000B3011
	public override float GetSortKey()
	{
		return 1f;
	}

	// Token: 0x060090CA RID: 37066 RVA: 0x000FEC3C File Offset: 0x000FCE3C
	protected override void OnPrefabInit()
	{
		NewBaseScreen.Instance = this;
		base.OnPrefabInit();
		TimeOfDay.Instance.SetScale(0f);
	}

	// Token: 0x060090CB RID: 37067 RVA: 0x000FEC59 File Offset: 0x000FCE59
	protected override void OnForcedCleanUp()
	{
		NewBaseScreen.Instance = null;
		base.OnForcedCleanUp();
	}

	// Token: 0x060090CC RID: 37068 RVA: 0x0037DC94 File Offset: 0x0037BE94
	public static Vector2I SetInitialCamera()
	{
		Vector2I vector2I = SaveLoader.Instance.cachedGSD.baseStartPos;
		vector2I += ClusterManager.Instance.GetStartWorld().WorldOffset;
		Vector3 pos = Grid.CellToPosCCC(Grid.OffsetCell(Grid.OffsetCell(0, vector2I.x, vector2I.y), 0, -2), Grid.SceneLayer.Background);
		CameraController.Instance.SetMaxOrthographicSize(40f);
		CameraController.Instance.SnapTo(pos);
		CameraController.Instance.SetTargetPos(pos, 20f, false);
		CameraController.Instance.OrthographicSize = 40f;
		CameraSaveData.valid = false;
		return vector2I;
	}

	// Token: 0x060090CD RID: 37069 RVA: 0x0037DD2C File Offset: 0x0037BF2C
	protected override void OnActivate()
	{
		if (this.disabledUIElements != null)
		{
			foreach (CanvasGroup canvasGroup in this.disabledUIElements)
			{
				if (canvasGroup != null)
				{
					canvasGroup.interactable = false;
				}
			}
		}
		NewBaseScreen.SetInitialCamera();
		if (SpeedControlScreen.Instance.IsPaused)
		{
			SpeedControlScreen.Instance.Unpause(false);
		}
		this.Final();
	}

	// Token: 0x060090CE RID: 37070 RVA: 0x000FEC67 File Offset: 0x000FCE67
	public void Init(Cluster clusterLayout, ITelepadDeliverable[] startingMinionStats)
	{
		this.m_clusterLayout = clusterLayout;
		this.m_minionStartingStats = startingMinionStats;
	}

	// Token: 0x060090CF RID: 37071 RVA: 0x0037DD90 File Offset: 0x0037BF90
	protected override void OnDeactivate()
	{
		Game.Instance.Trigger(-122303817, null);
		if (this.disabledUIElements != null)
		{
			foreach (CanvasGroup canvasGroup in this.disabledUIElements)
			{
				if (canvasGroup != null)
				{
					canvasGroup.interactable = true;
				}
			}
		}
	}

	// Token: 0x060090D0 RID: 37072 RVA: 0x0037DDE0 File Offset: 0x0037BFE0
	public override void OnKeyDown(KButtonEvent e)
	{
		global::Action[] array = new global::Action[4];
		RuntimeHelpers.InitializeArray(array, fieldof(<PrivateImplementationDetails>.46E7A7E6CE942EAE1E13925BEDED6E6321F99918099A108FDB32BB9510B8E88D).FieldHandle);
		global::Action[] array2 = array;
		if (!e.Consumed)
		{
			int num = 0;
			while (num < array2.Length && !e.TryConsume(array2[num]))
			{
				num++;
			}
		}
	}

	// Token: 0x060090D1 RID: 37073 RVA: 0x0037DE20 File Offset: 0x0037C020
	private void Final()
	{
		SpeedControlScreen.Instance.Unpause(false);
		GameObject telepad = GameUtil.GetTelepad(ClusterManager.Instance.GetStartWorld().id);
		if (telepad)
		{
			this.SpawnMinions(Grid.PosToCell(telepad));
		}
		Game.Instance.baseAlreadyCreated = true;
		this.Deactivate();
	}

	// Token: 0x060090D2 RID: 37074 RVA: 0x0037DE74 File Offset: 0x0037C074
	private void SpawnMinions(int headquartersCell)
	{
		if (headquartersCell == -1)
		{
			global::Debug.LogWarning("No headquarters in saved base template. Cannot place minions. Confirm there is a headquarters saved to the base template, or consider creating a new one.");
			return;
		}
		int num;
		int num2;
		Grid.CellToXY(headquartersCell, out num, out num2);
		if (Grid.WidthInCells < 64)
		{
			return;
		}
		int baseLeft = this.m_clusterLayout.currentWorld.BaseLeft;
		int baseRight = this.m_clusterLayout.currentWorld.BaseRight;
		Effect a_new_hope = Db.Get().effects.Get("AnewHope");
		Action<object> <>9__0;
		for (int i = 0; i < this.m_minionStartingStats.Length; i++)
		{
			MinionStartingStats minionStartingStats = (MinionStartingStats)this.m_minionStartingStats[i];
			int x = num + i % (baseRight - baseLeft) + 1;
			int y = num2;
			int cell = Grid.XYToCell(x, y);
			GameObject prefab = Assets.GetPrefab(BaseMinionConfig.GetMinionIDForModel(minionStartingStats.personality.model));
			GameObject gameObject = Util.KInstantiate(prefab, null, null);
			gameObject.name = prefab.name;
			Immigration.Instance.ApplyDefaultPersonalPriorities(gameObject);
			gameObject.transform.SetLocalPosition(Grid.CellToPosCBC(cell, Grid.SceneLayer.Move));
			gameObject.SetActive(true);
			minionStartingStats.Apply(gameObject);
			GameScheduler instance = GameScheduler.Instance;
			string name = "ANewHope";
			float time = 3f + 0.5f * (float)i;
			Action<object> callback;
			if ((callback = <>9__0) == null)
			{
				callback = (<>9__0 = delegate(object m)
				{
					GameObject gameObject2 = m as GameObject;
					if (gameObject2 == null)
					{
						return;
					}
					gameObject2.GetComponent<Effects>().Add(a_new_hope, true);
				});
			}
			instance.Schedule(name, time, callback, gameObject, null);
			if (minionStartingStats.personality.model == GameTags.Minions.Models.Bionic)
			{
				GameScheduler.Instance.Schedule("ExtraPowerBanks", 3f + 4.5f * (float)i, delegate(object m)
				{
					GameUtil.GetTelepad(ClusterManager.Instance.GetStartWorld().id).Trigger(1982288670, null);
				}, gameObject, null);
			}
		}
		ClusterManager.Instance.activeWorld.SetDupeVisited();
	}

	// Token: 0x04006D6F RID: 28015
	public static NewBaseScreen Instance;

	// Token: 0x04006D70 RID: 28016
	[SerializeField]
	private CanvasGroup[] disabledUIElements;

	// Token: 0x04006D71 RID: 28017
	public EventReference ScanSoundMigrated;

	// Token: 0x04006D72 RID: 28018
	public EventReference BuildBaseSoundMigrated;

	// Token: 0x04006D73 RID: 28019
	private ITelepadDeliverable[] m_minionStartingStats;

	// Token: 0x04006D74 RID: 28020
	private Cluster m_clusterLayout;
}
