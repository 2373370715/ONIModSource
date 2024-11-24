using System;
using Klei.AI;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/Workable/Clinic Dreamable")]
public class ClinicDreamable : Workable
{
	private static GameObject dreamJournalPrefab;

	private static Effect sleepClinic;

	public bool HasStartedThoughts_Dreaming;

	private ChoreDriver dreamer;

	private Equippable equippable;

	private Effects effects;

	private Sleepable sleepable;

	private KSelectable selectable;

	private HashedString dreamAnimName = "portal rocket comp";

	public bool DreamIsDisturbed { get; private set; }

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		resetProgressOnStop = false;
		workerStatusItem = Db.Get().DuplicantStatusItems.Dreaming;
		workingStatusItem = null;
	}

	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (dreamJournalPrefab == null)
		{
			dreamJournalPrefab = Assets.GetPrefab(DreamJournalConfig.ID);
			sleepClinic = Db.Get().effects.Get("SleepClinic");
		}
		equippable = GetComponent<Equippable>();
		Debug.Assert(equippable != null);
		EquipmentDef def = equippable.def;
		def.OnEquipCallBack = (Action<Equippable>)Delegate.Combine(def.OnEquipCallBack, new Action<Equippable>(OnEquipPajamas));
		EquipmentDef def2 = equippable.def;
		def2.OnUnequipCallBack = (Action<Equippable>)Delegate.Combine(def2.OnUnequipCallBack, new Action<Equippable>(OnUnequipPajamas));
		OnEquipPajamas(equippable);
	}

	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		if (!(equippable == null))
		{
			EquipmentDef def = equippable.def;
			def.OnEquipCallBack = (Action<Equippable>)Delegate.Remove(def.OnEquipCallBack, new Action<Equippable>(OnEquipPajamas));
			EquipmentDef def2 = equippable.def;
			def2.OnUnequipCallBack = (Action<Equippable>)Delegate.Remove(def2.OnUnequipCallBack, new Action<Equippable>(OnUnequipPajamas));
		}
	}

	protected override bool OnWorkTick(Worker worker, float dt)
	{
		if (GetPercentComplete() >= 1f)
		{
			Vector3 position = dreamer.transform.position;
			position.y += 1f;
			position.z = Grid.GetLayerZ(Grid.SceneLayer.Ore);
			Util.KInstantiate(dreamJournalPrefab, position, Quaternion.identity).SetActive(value: true);
			workTimeRemaining = GetWorkTime();
		}
		return false;
	}

	public void OnEquipPajamas(Equippable eq)
	{
		if (!(equippable == null) && !(equippable != eq))
		{
			MinionAssignablesProxy minionAssignablesProxy = equippable.assignee as MinionAssignablesProxy;
			if (!(minionAssignablesProxy == null) && !(minionAssignablesProxy.target is StoredMinionIdentity))
			{
				GameObject targetGameObject = minionAssignablesProxy.GetTargetGameObject();
				effects = targetGameObject.GetComponent<Effects>();
				dreamer = targetGameObject.GetComponent<ChoreDriver>();
				selectable = targetGameObject.GetComponent<KSelectable>();
				dreamer.Subscribe(-1283701846, WorkerStartedSleeping);
				dreamer.Subscribe(-2090444759, WorkerStoppedSleeping);
				effects.Add(sleepClinic, should_save: true);
				selectable.AddStatusItem(Db.Get().DuplicantStatusItems.MegaBrainTank_Pajamas_Wearing);
			}
		}
	}

	public void OnUnequipPajamas(Equippable eq)
	{
		if (!(dreamer == null) && !(equippable == null) && !(equippable != eq))
		{
			dreamer.Unsubscribe(-1283701846, WorkerStartedSleeping);
			dreamer.Unsubscribe(-2090444759, WorkerStoppedSleeping);
			selectable.RemoveStatusItem(Db.Get().DuplicantStatusItems.MegaBrainTank_Pajamas_Wearing);
			selectable.RemoveStatusItem(Db.Get().DuplicantStatusItems.MegaBrainTank_Pajamas_Sleeping);
			effects.Remove(sleepClinic.Id);
			StopDreamingThought();
			dreamer = null;
			selectable = null;
			effects = null;
		}
	}

	public void WorkerStartedSleeping(object data)
	{
		SleepChore sleepChore = dreamer.GetCurrentChore() as SleepChore;
		StateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.Parameter<bool>.Context context = sleepChore.smi.sm.isDisturbedByLight.GetContext(sleepChore.smi);
		context.onDirty = (Action<SleepChore.StatesInstance>)Delegate.Combine(context.onDirty, new Action<SleepChore.StatesInstance>(OnSleepDisturbed));
		StateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.Parameter<bool>.Context context2 = sleepChore.smi.sm.isDisturbedByMovement.GetContext(sleepChore.smi);
		context2.onDirty = (Action<SleepChore.StatesInstance>)Delegate.Combine(context2.onDirty, new Action<SleepChore.StatesInstance>(OnSleepDisturbed));
		StateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.Parameter<bool>.Context context3 = sleepChore.smi.sm.isDisturbedByNoise.GetContext(sleepChore.smi);
		context3.onDirty = (Action<SleepChore.StatesInstance>)Delegate.Combine(context3.onDirty, new Action<SleepChore.StatesInstance>(OnSleepDisturbed));
		StateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.Parameter<bool>.Context context4 = sleepChore.smi.sm.isScaredOfDark.GetContext(sleepChore.smi);
		context4.onDirty = (Action<SleepChore.StatesInstance>)Delegate.Combine(context4.onDirty, new Action<SleepChore.StatesInstance>(OnSleepDisturbed));
		sleepable = data as Sleepable;
		sleepable.Dreamable = this;
		StartWork(sleepable.worker);
		progressBar.Retarget(sleepable.gameObject);
		selectable.AddStatusItem(Db.Get().DuplicantStatusItems.MegaBrainTank_Pajamas_Sleeping, this);
		StartDreamingThought();
	}

	public void WorkerStoppedSleeping(object data)
	{
		selectable.RemoveStatusItem(Db.Get().DuplicantStatusItems.MegaBrainTank_Pajamas_Sleeping);
		SleepChore sleepChore = dreamer.GetCurrentChore() as SleepChore;
		if (!sleepChore.IsNullOrDestroyed() && !sleepChore.smi.IsNullOrDestroyed() && !sleepChore.smi.sm.IsNullOrDestroyed())
		{
			StateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.Parameter<bool>.Context context = sleepChore.smi.sm.isDisturbedByLight.GetContext(sleepChore.smi);
			context.onDirty = (Action<SleepChore.StatesInstance>)Delegate.Remove(context.onDirty, new Action<SleepChore.StatesInstance>(OnSleepDisturbed));
			StateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.Parameter<bool>.Context context2 = sleepChore.smi.sm.isDisturbedByMovement.GetContext(sleepChore.smi);
			context2.onDirty = (Action<SleepChore.StatesInstance>)Delegate.Remove(context2.onDirty, new Action<SleepChore.StatesInstance>(OnSleepDisturbed));
			StateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.Parameter<bool>.Context context3 = sleepChore.smi.sm.isDisturbedByNoise.GetContext(sleepChore.smi);
			context3.onDirty = (Action<SleepChore.StatesInstance>)Delegate.Remove(context3.onDirty, new Action<SleepChore.StatesInstance>(OnSleepDisturbed));
			StateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.Parameter<bool>.Context context4 = sleepChore.smi.sm.isScaredOfDark.GetContext(sleepChore.smi);
			context4.onDirty = (Action<SleepChore.StatesInstance>)Delegate.Remove(context4.onDirty, new Action<SleepChore.StatesInstance>(OnSleepDisturbed));
		}
		StopDreamingThought();
		DreamIsDisturbed = false;
		if (base.worker != null)
		{
			StopWork(base.worker, aborted: false);
		}
		if (sleepable != null)
		{
			sleepable.Dreamable = null;
			sleepable = null;
		}
	}

	private void OnSleepDisturbed(SleepChore.StatesInstance smi)
	{
		SleepChore sleepChore = dreamer.GetCurrentChore() as SleepChore;
		bool flag = sleepChore.smi.sm.isDisturbedByLight.Get(sleepChore.smi);
		flag |= sleepChore.smi.sm.isDisturbedByMovement.Get(sleepChore.smi);
		flag |= sleepChore.smi.sm.isDisturbedByNoise.Get(sleepChore.smi);
		if (DreamIsDisturbed = flag | sleepChore.smi.sm.isScaredOfDark.Get(sleepChore.smi))
		{
			StopDreamingThought();
		}
	}

	private void StartDreamingThought()
	{
		if (dreamer != null && !HasStartedThoughts_Dreaming)
		{
			dreamer.GetSMI<Dreamer.Instance>().SetDream(Db.Get().Dreams.CommonDream);
			dreamer.GetSMI<Dreamer.Instance>().StartDreaming();
			HasStartedThoughts_Dreaming = true;
		}
	}

	private void StopDreamingThought()
	{
		if (dreamer != null && HasStartedThoughts_Dreaming)
		{
			dreamer.GetSMI<Dreamer.Instance>().StopDreaming();
			HasStartedThoughts_Dreaming = false;
		}
	}
}
