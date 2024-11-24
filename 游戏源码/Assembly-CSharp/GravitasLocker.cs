using System;
using UnityEngine;

// Token: 0x02001393 RID: 5011
public class GravitasLocker : GameStateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>
{
	// Token: 0x0600670B RID: 26379 RVA: 0x002D3478 File Offset: 0x002D1678
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.close;
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		this.close.ParamTransition<bool>(this.IsOpen, this.open, GameStateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.IsTrue).DefaultState(this.close.idle);
		this.close.idle.PlayAnim("on").ParamTransition<bool>(this.WorkOrderGiven, this.close.work, GameStateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.IsTrue);
		this.close.work.DefaultState(this.close.work.waitingForDupe);
		this.close.work.waitingForDupe.Enter(new StateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.State.Callback(GravitasLocker.StartlWorkChore_OpenLocker)).Exit(new StateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.State.Callback(GravitasLocker.StopWorkChore)).WorkableCompleteTransition((GravitasLocker.Instance smi) => smi.GetWorkable(), this.close.work.complete).ParamTransition<bool>(this.WorkOrderGiven, this.close, GameStateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.IsFalse);
		this.close.work.complete.Enter(delegate(GravitasLocker.Instance smi)
		{
			this.WorkOrderGiven.Set(false, smi, false);
		}).Enter(new StateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.State.Callback(GravitasLocker.Open)).TriggerOnEnter(GameHashes.UIRefresh, null);
		this.open.ParamTransition<bool>(this.IsOpen, this.close, GameStateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.IsFalse).DefaultState(this.open.opening);
		this.open.opening.PlayAnim("working").OnAnimQueueComplete(this.open.idle);
		this.open.idle.PlayAnim("empty").Enter(new StateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.State.Callback(GravitasLocker.SpawnLoot)).ParamTransition<bool>(this.WorkOrderGiven, this.open.work, GameStateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.IsTrue);
		this.open.work.DefaultState(this.open.work.waitingForDupe);
		this.open.work.waitingForDupe.Enter(new StateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.State.Callback(GravitasLocker.StartWorkChore_CloseLocker)).Exit(new StateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.State.Callback(GravitasLocker.StopWorkChore)).WorkableCompleteTransition((GravitasLocker.Instance smi) => smi.GetWorkable(), this.open.work.complete).ParamTransition<bool>(this.WorkOrderGiven, this.open.idle, GameStateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.IsFalse);
		this.open.work.complete.Enter(delegate(GravitasLocker.Instance smi)
		{
			this.WorkOrderGiven.Set(false, smi, false);
		}).Enter(new StateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.State.Callback(GravitasLocker.Close)).TriggerOnEnter(GameHashes.UIRefresh, null);
	}

	// Token: 0x0600670C RID: 26380 RVA: 0x000E35D4 File Offset: 0x000E17D4
	public static void Open(GravitasLocker.Instance smi)
	{
		smi.Open();
	}

	// Token: 0x0600670D RID: 26381 RVA: 0x000E35DC File Offset: 0x000E17DC
	public static void Close(GravitasLocker.Instance smi)
	{
		smi.Close();
	}

	// Token: 0x0600670E RID: 26382 RVA: 0x000E35E4 File Offset: 0x000E17E4
	public static void SpawnLoot(GravitasLocker.Instance smi)
	{
		smi.SpawnLoot();
	}

	// Token: 0x0600670F RID: 26383 RVA: 0x000E35EC File Offset: 0x000E17EC
	public static void StartWorkChore_CloseLocker(GravitasLocker.Instance smi)
	{
		smi.CreateWorkChore_CloseLocker();
	}

	// Token: 0x06006710 RID: 26384 RVA: 0x000E35F4 File Offset: 0x000E17F4
	public static void StartlWorkChore_OpenLocker(GravitasLocker.Instance smi)
	{
		smi.CreateWorkChore_OpenLocker();
	}

	// Token: 0x06006711 RID: 26385 RVA: 0x000E35FC File Offset: 0x000E17FC
	public static void StopWorkChore(GravitasLocker.Instance smi)
	{
		smi.StopWorkChore();
	}

	// Token: 0x04004D60 RID: 19808
	public const float CLOSE_WORKTIME = 1f;

	// Token: 0x04004D61 RID: 19809
	public const float OPEN_WORKTIME = 1.5f;

	// Token: 0x04004D62 RID: 19810
	public const string CLOSED_ANIM_NAME = "on";

	// Token: 0x04004D63 RID: 19811
	public const string OPENING_ANIM_NAME = "working";

	// Token: 0x04004D64 RID: 19812
	public const string OPENED = "empty";

	// Token: 0x04004D65 RID: 19813
	private StateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.BoolParameter IsOpen;

	// Token: 0x04004D66 RID: 19814
	private StateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.BoolParameter WasEmptied;

	// Token: 0x04004D67 RID: 19815
	private StateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.BoolParameter WorkOrderGiven;

	// Token: 0x04004D68 RID: 19816
	public GravitasLocker.CloseStates close;

	// Token: 0x04004D69 RID: 19817
	public GravitasLocker.OpenStates open;

	// Token: 0x02001394 RID: 5012
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04004D6A RID: 19818
		public bool CanBeClosed;

		// Token: 0x04004D6B RID: 19819
		public string SideScreen_OpenButtonText;

		// Token: 0x04004D6C RID: 19820
		public string SideScreen_OpenButtonTooltip;

		// Token: 0x04004D6D RID: 19821
		public string SideScreen_CancelOpenButtonText;

		// Token: 0x04004D6E RID: 19822
		public string SideScreen_CancelOpenButtonTooltip;

		// Token: 0x04004D6F RID: 19823
		public string SideScreen_CloseButtonText;

		// Token: 0x04004D70 RID: 19824
		public string SideScreen_CloseButtonTooltip;

		// Token: 0x04004D71 RID: 19825
		public string SideScreen_CancelCloseButtonText;

		// Token: 0x04004D72 RID: 19826
		public string SideScreen_CancelCloseButtonTooltip;

		// Token: 0x04004D73 RID: 19827
		public string OPEN_INTERACT_ANIM_NAME = "anim_interacts_clothingfactory_kanim";

		// Token: 0x04004D74 RID: 19828
		public string CLOSE_INTERACT_ANIM_NAME = "anim_interacts_clothingfactory_kanim";

		// Token: 0x04004D75 RID: 19829
		public string[] ObjectsToSpawn = new string[0];

		// Token: 0x04004D76 RID: 19830
		public string[] LootSymbols = new string[0];
	}

	// Token: 0x02001395 RID: 5013
	public class WorkStates : GameStateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.State
	{
		// Token: 0x04004D77 RID: 19831
		public GameStateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.State waitingForDupe;

		// Token: 0x04004D78 RID: 19832
		public GameStateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.State complete;
	}

	// Token: 0x02001396 RID: 5014
	public class CloseStates : GameStateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.State
	{
		// Token: 0x04004D79 RID: 19833
		public GameStateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.State idle;

		// Token: 0x04004D7A RID: 19834
		public GravitasLocker.WorkStates work;
	}

	// Token: 0x02001397 RID: 5015
	public class OpenStates : GameStateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.State
	{
		// Token: 0x04004D7B RID: 19835
		public GameStateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.State opening;

		// Token: 0x04004D7C RID: 19836
		public GameStateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.State idle;

		// Token: 0x04004D7D RID: 19837
		public GravitasLocker.WorkStates work;
	}

	// Token: 0x02001398 RID: 5016
	public new class Instance : GameStateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.GameInstance, ISidescreenButtonControl
	{
		// Token: 0x1700066B RID: 1643
		// (get) Token: 0x06006719 RID: 26393 RVA: 0x000E365B File Offset: 0x000E185B
		public bool WorkOrderGiven
		{
			get
			{
				return base.smi.sm.WorkOrderGiven.Get(base.smi);
			}
		}

		// Token: 0x1700066C RID: 1644
		// (get) Token: 0x0600671A RID: 26394 RVA: 0x000E3678 File Offset: 0x000E1878
		public bool IsOpen
		{
			get
			{
				return base.smi.sm.IsOpen.Get(base.smi);
			}
		}

		// Token: 0x1700066D RID: 1645
		// (get) Token: 0x0600671B RID: 26395 RVA: 0x000E3695 File Offset: 0x000E1895
		public bool HasContents
		{
			get
			{
				return !base.smi.sm.WasEmptied.Get(base.smi) && base.def.ObjectsToSpawn.Length != 0;
			}
		}

		// Token: 0x0600671C RID: 26396 RVA: 0x000E36C5 File Offset: 0x000E18C5
		public Workable GetWorkable()
		{
			return this.workable;
		}

		// Token: 0x0600671D RID: 26397 RVA: 0x000E36CD File Offset: 0x000E18CD
		public void Open()
		{
			base.smi.sm.IsOpen.Set(true, base.smi, false);
		}

		// Token: 0x0600671E RID: 26398 RVA: 0x000E36ED File Offset: 0x000E18ED
		public void Close()
		{
			base.smi.sm.IsOpen.Set(false, base.smi, false);
		}

		// Token: 0x0600671F RID: 26399 RVA: 0x000E370D File Offset: 0x000E190D
		public Instance(IStateMachineTarget master, GravitasLocker.Def def) : base(master, def)
		{
		}

		// Token: 0x06006720 RID: 26400 RVA: 0x000E3717 File Offset: 0x000E1917
		public override void StartSM()
		{
			this.DefineDropSpawnPositions();
			base.StartSM();
			this.UpdateContentPreviewSymbols();
		}

		// Token: 0x06006721 RID: 26401 RVA: 0x002D3744 File Offset: 0x002D1944
		public void DefineDropSpawnPositions()
		{
			if (this.dropSpawnPositions == null && base.def.LootSymbols.Length != 0)
			{
				this.dropSpawnPositions = new Vector3[base.def.LootSymbols.Length];
				for (int i = 0; i < this.dropSpawnPositions.Length; i++)
				{
					bool flag;
					Vector3 vector = this.animController.GetSymbolTransform(base.def.LootSymbols[i], out flag).GetColumn(3);
					vector.z = Grid.GetLayerZ(Grid.SceneLayer.Ore);
					this.dropSpawnPositions[i] = (flag ? vector : base.gameObject.transform.GetPosition());
				}
			}
		}

		// Token: 0x06006722 RID: 26402 RVA: 0x002D37F8 File Offset: 0x002D19F8
		public void CreateWorkChore_CloseLocker()
		{
			if (this.chore == null)
			{
				this.workable.SetWorkTime(1f);
				this.chore = new WorkChore<Workable>(Db.Get().ChoreTypes.Repair, this.workable, null, true, null, null, null, true, null, false, true, Assets.GetAnim(base.def.CLOSE_INTERACT_ANIM_NAME), false, true, true, PriorityScreen.PriorityClass.high, 5, false, true);
			}
		}

		// Token: 0x06006723 RID: 26403 RVA: 0x002D3864 File Offset: 0x002D1A64
		public void CreateWorkChore_OpenLocker()
		{
			if (this.chore == null)
			{
				this.workable.SetWorkTime(1.5f);
				this.chore = new WorkChore<Workable>(Db.Get().ChoreTypes.EmptyStorage, this.workable, null, true, null, null, null, true, null, false, true, Assets.GetAnim(base.def.OPEN_INTERACT_ANIM_NAME), false, true, true, PriorityScreen.PriorityClass.high, 5, false, true);
			}
		}

		// Token: 0x06006724 RID: 26404 RVA: 0x000E372B File Offset: 0x000E192B
		public void StopWorkChore()
		{
			if (this.chore != null)
			{
				this.chore.Cancel("Canceled by user");
				this.chore = null;
			}
		}

		// Token: 0x06006725 RID: 26405 RVA: 0x002D38D0 File Offset: 0x002D1AD0
		public void SpawnLoot()
		{
			if (this.HasContents)
			{
				for (int i = 0; i < base.def.ObjectsToSpawn.Length; i++)
				{
					string name = base.def.ObjectsToSpawn[i];
					GameObject gameObject = Scenario.SpawnPrefab(Grid.PosToCell(base.gameObject), 0, 0, name, Grid.SceneLayer.Ore);
					gameObject.SetActive(true);
					if (this.dropSpawnPositions != null && i < this.dropSpawnPositions.Length)
					{
						gameObject.transform.position = this.dropSpawnPositions[i];
					}
				}
				base.smi.sm.WasEmptied.Set(true, base.smi, false);
				this.UpdateContentPreviewSymbols();
			}
		}

		// Token: 0x06006726 RID: 26406 RVA: 0x002D397C File Offset: 0x002D1B7C
		public void UpdateContentPreviewSymbols()
		{
			for (int i = 0; i < base.def.LootSymbols.Length; i++)
			{
				this.animController.SetSymbolVisiblity(base.def.LootSymbols[i], false);
			}
			if (this.HasContents)
			{
				for (int j = 0; j < Mathf.Min(base.def.LootSymbols.Length, base.def.ObjectsToSpawn.Length); j++)
				{
					KAnim.Build.Symbol symbolByIndex = Assets.GetPrefab(base.def.ObjectsToSpawn[j]).GetComponent<KBatchedAnimController>().AnimFiles[0].GetData().build.GetSymbolByIndex(0U);
					SymbolOverrideController component = base.gameObject.GetComponent<SymbolOverrideController>();
					string text = base.def.LootSymbols[j];
					component.AddSymbolOverride(text, symbolByIndex, 0);
					this.animController.SetSymbolVisiblity(text, true);
				}
			}
		}

		// Token: 0x1700066E RID: 1646
		// (get) Token: 0x06006727 RID: 26407 RVA: 0x002D3A64 File Offset: 0x002D1C64
		public string SidescreenButtonText
		{
			get
			{
				if (!this.IsOpen)
				{
					if (!this.WorkOrderGiven)
					{
						return base.def.SideScreen_OpenButtonText;
					}
					return base.def.SideScreen_CancelOpenButtonText;
				}
				else
				{
					if (!this.WorkOrderGiven)
					{
						return base.def.SideScreen_CloseButtonText;
					}
					return base.def.SideScreen_CancelCloseButtonText;
				}
			}
		}

		// Token: 0x1700066F RID: 1647
		// (get) Token: 0x06006728 RID: 26408 RVA: 0x002D3AB8 File Offset: 0x002D1CB8
		public string SidescreenButtonTooltip
		{
			get
			{
				if (!this.IsOpen)
				{
					if (!this.WorkOrderGiven)
					{
						return base.def.SideScreen_OpenButtonTooltip;
					}
					return base.def.SideScreen_CancelOpenButtonTooltip;
				}
				else
				{
					if (!this.WorkOrderGiven)
					{
						return base.def.SideScreen_CloseButtonTooltip;
					}
					return base.def.SideScreen_CancelCloseButtonTooltip;
				}
			}
		}

		// Token: 0x06006729 RID: 26409 RVA: 0x000E374C File Offset: 0x000E194C
		public bool SidescreenEnabled()
		{
			return !this.IsOpen || base.def.CanBeClosed;
		}

		// Token: 0x0600672A RID: 26410 RVA: 0x000E374C File Offset: 0x000E194C
		public bool SidescreenButtonInteractable()
		{
			return !this.IsOpen || base.def.CanBeClosed;
		}

		// Token: 0x0600672B RID: 26411 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
		public int HorizontalGroupID()
		{
			return 0;
		}

		// Token: 0x0600672C RID: 26412 RVA: 0x000ABCBD File Offset: 0x000A9EBD
		public int ButtonSideScreenSortOrder()
		{
			return 20;
		}

		// Token: 0x0600672D RID: 26413 RVA: 0x000ABCB6 File Offset: 0x000A9EB6
		public void SetButtonTextOverride(ButtonMenuTextOverride textOverride)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600672E RID: 26414 RVA: 0x000E3763 File Offset: 0x000E1963
		public void OnSidescreenButtonPressed()
		{
			base.smi.sm.WorkOrderGiven.Set(!base.smi.sm.WorkOrderGiven.Get(base.smi), base.smi, false);
		}

		// Token: 0x04004D7E RID: 19838
		[MyCmpGet]
		private Workable workable;

		// Token: 0x04004D7F RID: 19839
		[MyCmpGet]
		private KBatchedAnimController animController;

		// Token: 0x04004D80 RID: 19840
		private Chore chore;

		// Token: 0x04004D81 RID: 19841
		private Vector3[] dropSpawnPositions;
	}
}
