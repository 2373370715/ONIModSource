using System;
using STRINGS;

// Token: 0x0200113D RID: 4413
public class CreatureDiseaseCleaner : GameStateMachine<CreatureDiseaseCleaner, CreatureDiseaseCleaner.Instance, IStateMachineTarget, CreatureDiseaseCleaner.Def>
{
	// Token: 0x06005A4D RID: 23117 RVA: 0x0029426C File Offset: 0x0029246C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.cleaning;
		GameStateMachine<CreatureDiseaseCleaner, CreatureDiseaseCleaner.Instance, IStateMachineTarget, CreatureDiseaseCleaner.Def>.State root = this.root;
		string name = CREATURES.STATUSITEMS.CLEANING.NAME;
		string tooltip = CREATURES.STATUSITEMS.CLEANING.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		root.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main);
		this.cleaning.DefaultState(this.cleaning.clean_pre).ScheduleGoTo((CreatureDiseaseCleaner.Instance smi) => smi.def.cleanDuration, this.cleaning.clean_pst);
		this.cleaning.clean_pre.PlayAnim("clean_water_pre").OnAnimQueueComplete(this.cleaning.clean);
		this.cleaning.clean.Enter(delegate(CreatureDiseaseCleaner.Instance smi)
		{
			smi.EnableDiseaseEmitter(true);
		}).QueueAnim("clean_water_loop", true, null).Transition(this.cleaning.clean_pst, (CreatureDiseaseCleaner.Instance smi) => !smi.GetSMI<CleaningMonitor.Instance>().CanCleanElementState(), UpdateRate.SIM_1000ms).Exit(delegate(CreatureDiseaseCleaner.Instance smi)
		{
			smi.EnableDiseaseEmitter(false);
		});
		this.cleaning.clean_pst.PlayAnim("clean_water_pst").OnAnimQueueComplete(this.behaviourcomplete);
		this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.Cleaning, false);
	}

	// Token: 0x04003FAD RID: 16301
	public GameStateMachine<CreatureDiseaseCleaner, CreatureDiseaseCleaner.Instance, IStateMachineTarget, CreatureDiseaseCleaner.Def>.State behaviourcomplete;

	// Token: 0x04003FAE RID: 16302
	public CreatureDiseaseCleaner.CleaningStates cleaning;

	// Token: 0x04003FAF RID: 16303
	public StateMachine<CreatureDiseaseCleaner, CreatureDiseaseCleaner.Instance, IStateMachineTarget, CreatureDiseaseCleaner.Def>.Signal cellChangedSignal;

	// Token: 0x0200113E RID: 4414
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x06005A4F RID: 23119 RVA: 0x000DAD8D File Offset: 0x000D8F8D
		public Def(float duration)
		{
			this.cleanDuration = duration;
		}

		// Token: 0x04003FB0 RID: 16304
		public float cleanDuration;
	}

	// Token: 0x0200113F RID: 4415
	public class CleaningStates : GameStateMachine<CreatureDiseaseCleaner, CreatureDiseaseCleaner.Instance, IStateMachineTarget, CreatureDiseaseCleaner.Def>.State
	{
		// Token: 0x04003FB1 RID: 16305
		public GameStateMachine<CreatureDiseaseCleaner, CreatureDiseaseCleaner.Instance, IStateMachineTarget, CreatureDiseaseCleaner.Def>.State clean_pre;

		// Token: 0x04003FB2 RID: 16306
		public GameStateMachine<CreatureDiseaseCleaner, CreatureDiseaseCleaner.Instance, IStateMachineTarget, CreatureDiseaseCleaner.Def>.State clean;

		// Token: 0x04003FB3 RID: 16307
		public GameStateMachine<CreatureDiseaseCleaner, CreatureDiseaseCleaner.Instance, IStateMachineTarget, CreatureDiseaseCleaner.Def>.State clean_pst;
	}

	// Token: 0x02001140 RID: 4416
	public new class Instance : GameStateMachine<CreatureDiseaseCleaner, CreatureDiseaseCleaner.Instance, IStateMachineTarget, CreatureDiseaseCleaner.Def>.GameInstance
	{
		// Token: 0x06005A51 RID: 23121 RVA: 0x000DADA4 File Offset: 0x000D8FA4
		public Instance(Chore<CreatureDiseaseCleaner.Instance> chore, CreatureDiseaseCleaner.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.Cleaning);
		}

		// Token: 0x06005A52 RID: 23122 RVA: 0x002943FC File Offset: 0x002925FC
		public void EnableDiseaseEmitter(bool enable = true)
		{
			DiseaseEmitter component = base.GetComponent<DiseaseEmitter>();
			if (component != null)
			{
				component.SetEnable(enable);
			}
		}
	}
}
