using System;
using UnityEngine;

public class GravitasLocker : GameStateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>
{
	public class Def : BaseDef
	{
		public bool CanBeClosed;

		public string SideScreen_OpenButtonText;

		public string SideScreen_OpenButtonTooltip;

		public string SideScreen_CancelOpenButtonText;

		public string SideScreen_CancelOpenButtonTooltip;

		public string SideScreen_CloseButtonText;

		public string SideScreen_CloseButtonTooltip;

		public string SideScreen_CancelCloseButtonText;

		public string SideScreen_CancelCloseButtonTooltip;

		public string OPEN_INTERACT_ANIM_NAME = "anim_interacts_clothingfactory_kanim";

		public string CLOSE_INTERACT_ANIM_NAME = "anim_interacts_clothingfactory_kanim";

		public string[] ObjectsToSpawn = new string[0];

		public string[] LootSymbols = new string[0];
	}

	public class WorkStates : State
	{
		public State waitingForDupe;

		public State complete;
	}

	public class CloseStates : State
	{
		public State idle;

		public WorkStates work;
	}

	public class OpenStates : State
	{
		public State opening;

		public State idle;

		public WorkStates work;
	}

	public new class Instance : GameInstance, ISidescreenButtonControl
	{
		[MyCmpGet]
		private Workable workable;

		[MyCmpGet]
		private KBatchedAnimController animController;

		private Chore chore;

		private Vector3[] dropSpawnPositions;

		public bool WorkOrderGiven => base.smi.sm.WorkOrderGiven.Get(base.smi);

		public bool IsOpen => base.smi.sm.IsOpen.Get(base.smi);

		public bool HasContents
		{
			get
			{
				if (!base.smi.sm.WasEmptied.Get(base.smi))
				{
					return base.def.ObjectsToSpawn.Length != 0;
				}
				return false;
			}
		}

		public string SidescreenButtonText
		{
			get
			{
				if (!IsOpen)
				{
					if (!WorkOrderGiven)
					{
						return base.def.SideScreen_OpenButtonText;
					}
					return base.def.SideScreen_CancelOpenButtonText;
				}
				if (!WorkOrderGiven)
				{
					return base.def.SideScreen_CloseButtonText;
				}
				return base.def.SideScreen_CancelCloseButtonText;
			}
		}

		public string SidescreenButtonTooltip
		{
			get
			{
				if (!IsOpen)
				{
					if (!WorkOrderGiven)
					{
						return base.def.SideScreen_OpenButtonTooltip;
					}
					return base.def.SideScreen_CancelOpenButtonTooltip;
				}
				if (!WorkOrderGiven)
				{
					return base.def.SideScreen_CloseButtonTooltip;
				}
				return base.def.SideScreen_CancelCloseButtonTooltip;
			}
		}

		public Workable GetWorkable()
		{
			return workable;
		}

		public void Open()
		{
			base.smi.sm.IsOpen.Set(value: true, base.smi);
		}

		public void Close()
		{
			base.smi.sm.IsOpen.Set(value: false, base.smi);
		}

		public Instance(IStateMachineTarget master, Def def)
			: base(master, def)
		{
		}

		public override void StartSM()
		{
			DefineDropSpawnPositions();
			base.StartSM();
			UpdateContentPreviewSymbols();
		}

		public void DefineDropSpawnPositions()
		{
			if (dropSpawnPositions == null && base.def.LootSymbols.Length != 0)
			{
				dropSpawnPositions = new Vector3[base.def.LootSymbols.Length];
				for (int i = 0; i < dropSpawnPositions.Length; i++)
				{
					bool symbolVisible;
					Vector3 vector = animController.GetSymbolTransform(base.def.LootSymbols[i], out symbolVisible).GetColumn(3);
					vector.z = Grid.GetLayerZ(Grid.SceneLayer.Ore);
					dropSpawnPositions[i] = (symbolVisible ? vector : base.gameObject.transform.GetPosition());
				}
			}
		}

		public void CreateWorkChore_CloseLocker()
		{
			if (chore == null)
			{
				workable.SetWorkTime(1f);
				chore = new WorkChore<Workable>(Db.Get().ChoreTypes.Repair, workable, null, run_until_complete: true, null, null, null, allow_in_red_alert: true, null, ignore_schedule_block: false, only_when_operational: true, Assets.GetAnim(base.def.CLOSE_INTERACT_ANIM_NAME), is_preemptable: false, allow_in_context_menu: true, allow_prioritization: true, PriorityScreen.PriorityClass.high);
			}
		}

		public void CreateWorkChore_OpenLocker()
		{
			if (chore == null)
			{
				workable.SetWorkTime(1.5f);
				chore = new WorkChore<Workable>(Db.Get().ChoreTypes.EmptyStorage, workable, null, run_until_complete: true, null, null, null, allow_in_red_alert: true, null, ignore_schedule_block: false, only_when_operational: true, Assets.GetAnim(base.def.OPEN_INTERACT_ANIM_NAME), is_preemptable: false, allow_in_context_menu: true, allow_prioritization: true, PriorityScreen.PriorityClass.high);
			}
		}

		public void StopWorkChore()
		{
			if (chore != null)
			{
				chore.Cancel("Canceled by user");
				chore = null;
			}
		}

		public void SpawnLoot()
		{
			if (!HasContents)
			{
				return;
			}
			for (int i = 0; i < base.def.ObjectsToSpawn.Length; i++)
			{
				string name = base.def.ObjectsToSpawn[i];
				GameObject gameObject = Scenario.SpawnPrefab(Grid.PosToCell(base.gameObject), 0, 0, name);
				gameObject.SetActive(value: true);
				if (dropSpawnPositions != null && i < dropSpawnPositions.Length)
				{
					gameObject.transform.position = dropSpawnPositions[i];
				}
			}
			base.smi.sm.WasEmptied.Set(value: true, base.smi);
			UpdateContentPreviewSymbols();
		}

		public void UpdateContentPreviewSymbols()
		{
			for (int i = 0; i < base.def.LootSymbols.Length; i++)
			{
				animController.SetSymbolVisiblity(base.def.LootSymbols[i], is_visible: false);
			}
			if (HasContents)
			{
				for (int j = 0; j < Mathf.Min(base.def.LootSymbols.Length, base.def.ObjectsToSpawn.Length); j++)
				{
					KAnim.Build.Symbol symbolByIndex = Assets.GetPrefab(base.def.ObjectsToSpawn[j]).GetComponent<KBatchedAnimController>().AnimFiles[0].GetData().build.GetSymbolByIndex(0u);
					SymbolOverrideController component = base.gameObject.GetComponent<SymbolOverrideController>();
					string text = base.def.LootSymbols[j];
					component.AddSymbolOverride(text, symbolByIndex);
					animController.SetSymbolVisiblity(text, is_visible: true);
				}
			}
		}

		public bool SidescreenEnabled()
		{
			if (IsOpen)
			{
				return base.def.CanBeClosed;
			}
			return true;
		}

		public bool SidescreenButtonInteractable()
		{
			if (IsOpen)
			{
				return base.def.CanBeClosed;
			}
			return true;
		}

		public int HorizontalGroupID()
		{
			return 0;
		}

		public int ButtonSideScreenSortOrder()
		{
			return 20;
		}

		public void SetButtonTextOverride(ButtonMenuTextOverride textOverride)
		{
			throw new NotImplementedException();
		}

		public void OnSidescreenButtonPressed()
		{
			base.smi.sm.WorkOrderGiven.Set(!base.smi.sm.WorkOrderGiven.Get(base.smi), base.smi);
		}
	}

	public const float CLOSE_WORKTIME = 1f;

	public const float OPEN_WORKTIME = 1.5f;

	public const string CLOSED_ANIM_NAME = "on";

	public const string OPENING_ANIM_NAME = "working";

	public const string OPENED = "empty";

	private BoolParameter IsOpen;

	private BoolParameter WasEmptied;

	private BoolParameter WorkOrderGiven;

	public CloseStates close;

	public OpenStates open;

	public override void InitializeStates(out BaseState default_state)
	{
		default_state = close;
		base.serializable = SerializeType.ParamsOnly;
		close.ParamTransition(IsOpen, open, GameStateMachine<GravitasLocker, Instance, IStateMachineTarget, Def>.IsTrue).DefaultState(close.idle);
		close.idle.PlayAnim("on").ParamTransition(WorkOrderGiven, close.work, GameStateMachine<GravitasLocker, Instance, IStateMachineTarget, Def>.IsTrue);
		close.work.DefaultState(close.work.waitingForDupe);
		close.work.waitingForDupe.Enter(StartlWorkChore_OpenLocker).Exit(StopWorkChore).WorkableCompleteTransition((Instance smi) => smi.GetWorkable(), close.work.complete)
			.ParamTransition(WorkOrderGiven, close, GameStateMachine<GravitasLocker, Instance, IStateMachineTarget, Def>.IsFalse);
		close.work.complete.Enter(delegate(Instance smi)
		{
			WorkOrderGiven.Set(value: false, smi);
		}).Enter(Open).TriggerOnEnter(GameHashes.UIRefresh);
		open.ParamTransition(IsOpen, close, GameStateMachine<GravitasLocker, Instance, IStateMachineTarget, Def>.IsFalse).DefaultState(open.opening);
		open.opening.PlayAnim("working").OnAnimQueueComplete(open.idle);
		open.idle.PlayAnim("empty").Enter(SpawnLoot).ParamTransition(WorkOrderGiven, open.work, GameStateMachine<GravitasLocker, Instance, IStateMachineTarget, Def>.IsTrue);
		open.work.DefaultState(open.work.waitingForDupe);
		open.work.waitingForDupe.Enter(StartWorkChore_CloseLocker).Exit(StopWorkChore).WorkableCompleteTransition((Instance smi) => smi.GetWorkable(), open.work.complete)
			.ParamTransition(WorkOrderGiven, open.idle, GameStateMachine<GravitasLocker, Instance, IStateMachineTarget, Def>.IsFalse);
		open.work.complete.Enter(delegate(Instance smi)
		{
			WorkOrderGiven.Set(value: false, smi);
		}).Enter(Close).TriggerOnEnter(GameHashes.UIRefresh);
	}

	public static void Open(Instance smi)
	{
		smi.Open();
	}

	public static void Close(Instance smi)
	{
		smi.Close();
	}

	public static void SpawnLoot(Instance smi)
	{
		smi.SpawnLoot();
	}

	public static void StartWorkChore_CloseLocker(Instance smi)
	{
		smi.CreateWorkChore_CloseLocker();
	}

	public static void StartlWorkChore_OpenLocker(Instance smi)
	{
		smi.CreateWorkChore_OpenLocker();
	}

	public static void StopWorkChore(Instance smi)
	{
		smi.StopWorkChore();
	}
}
