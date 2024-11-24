using System;
using System.Collections.Generic;

// Token: 0x020009C6 RID: 2502
public class CreatureThoughtGraph : GameStateMachine<CreatureThoughtGraph, CreatureThoughtGraph.Instance, IStateMachineTarget, CreatureThoughtGraph.Def>
{
	// Token: 0x06002E03 RID: 11779 RVA: 0x001F3970 File Offset: 0x001F1B70
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.initialdelay;
		this.initialdelay.ScheduleGoTo(1f, this.nothoughts);
		this.nothoughts.OnSignal(this.thoughtsChanged, this.displayingthought, (CreatureThoughtGraph.Instance smi) => smi.HasThoughts()).OnSignal(this.thoughtsChangedImmediate, this.displayingthought, (CreatureThoughtGraph.Instance smi) => smi.HasThoughts());
		this.displayingthought.Enter("CreateBubble", delegate(CreatureThoughtGraph.Instance smi)
		{
			smi.CreateBubble();
		}).Exit("DestroyBubble", delegate(CreatureThoughtGraph.Instance smi)
		{
			smi.DestroyBubble();
		}).ScheduleGoTo((CreatureThoughtGraph.Instance smi) => this.thoughtDisplayTime.Get(smi), this.cooldown);
		this.cooldown.OnSignal(this.thoughtsChangedImmediate, this.displayingthought, (CreatureThoughtGraph.Instance smi) => smi.HasImmediateThought()).ScheduleGoTo(20f, this.nothoughts);
	}

	// Token: 0x04001EE3 RID: 7907
	public StateMachine<CreatureThoughtGraph, CreatureThoughtGraph.Instance, IStateMachineTarget, CreatureThoughtGraph.Def>.Signal thoughtsChanged;

	// Token: 0x04001EE4 RID: 7908
	public StateMachine<CreatureThoughtGraph, CreatureThoughtGraph.Instance, IStateMachineTarget, CreatureThoughtGraph.Def>.Signal thoughtsChangedImmediate;

	// Token: 0x04001EE5 RID: 7909
	public StateMachine<CreatureThoughtGraph, CreatureThoughtGraph.Instance, IStateMachineTarget, CreatureThoughtGraph.Def>.FloatParameter thoughtDisplayTime;

	// Token: 0x04001EE6 RID: 7910
	public GameStateMachine<CreatureThoughtGraph, CreatureThoughtGraph.Instance, IStateMachineTarget, CreatureThoughtGraph.Def>.State initialdelay;

	// Token: 0x04001EE7 RID: 7911
	public GameStateMachine<CreatureThoughtGraph, CreatureThoughtGraph.Instance, IStateMachineTarget, CreatureThoughtGraph.Def>.State nothoughts;

	// Token: 0x04001EE8 RID: 7912
	public GameStateMachine<CreatureThoughtGraph, CreatureThoughtGraph.Instance, IStateMachineTarget, CreatureThoughtGraph.Def>.State displayingthought;

	// Token: 0x04001EE9 RID: 7913
	public GameStateMachine<CreatureThoughtGraph, CreatureThoughtGraph.Instance, IStateMachineTarget, CreatureThoughtGraph.Def>.State cooldown;

	// Token: 0x020009C7 RID: 2503
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x020009C8 RID: 2504
	public new class Instance : GameStateMachine<CreatureThoughtGraph, CreatureThoughtGraph.Instance, IStateMachineTarget, CreatureThoughtGraph.Def>.GameInstance
	{
		// Token: 0x06002E07 RID: 11783 RVA: 0x000BDC6E File Offset: 0x000BBE6E
		public Instance(IStateMachineTarget master, CreatureThoughtGraph.Def def) : base(master, def)
		{
			NameDisplayScreen.Instance.RegisterComponent(base.gameObject, this, false);
		}

		// Token: 0x06002E08 RID: 11784 RVA: 0x000BDC95 File Offset: 0x000BBE95
		protected override void OnCleanUp()
		{
			base.OnCleanUp();
		}

		// Token: 0x06002E09 RID: 11785 RVA: 0x000BDC9D File Offset: 0x000BBE9D
		public bool HasThoughts()
		{
			return this.thoughts.Count > 0;
		}

		// Token: 0x06002E0A RID: 11786 RVA: 0x001F3ABC File Offset: 0x001F1CBC
		public bool HasImmediateThought()
		{
			bool result = false;
			for (int i = 0; i < this.thoughts.Count; i++)
			{
				if (this.thoughts[i].showImmediately)
				{
					result = true;
					break;
				}
			}
			return result;
		}

		// Token: 0x06002E0B RID: 11787 RVA: 0x001F3AFC File Offset: 0x001F1CFC
		public void AddThought(Thought thought)
		{
			if (this.thoughts.Contains(thought))
			{
				return;
			}
			this.thoughts.Add(thought);
			if (thought.showImmediately)
			{
				base.sm.thoughtsChangedImmediate.Trigger(base.smi);
				return;
			}
			base.sm.thoughtsChanged.Trigger(base.smi);
		}

		// Token: 0x06002E0C RID: 11788 RVA: 0x000BDCAD File Offset: 0x000BBEAD
		public void RemoveThought(Thought thought)
		{
			if (!this.thoughts.Contains(thought))
			{
				return;
			}
			this.thoughts.Remove(thought);
			base.sm.thoughtsChanged.Trigger(base.smi);
		}

		// Token: 0x06002E0D RID: 11789 RVA: 0x000BDCE1 File Offset: 0x000BBEE1
		private int SortThoughts(Thought a, Thought b)
		{
			if (a.showImmediately == b.showImmediately)
			{
				return b.priority.CompareTo(a.priority);
			}
			if (!a.showImmediately)
			{
				return 1;
			}
			return -1;
		}

		// Token: 0x06002E0E RID: 11790 RVA: 0x001F3B5C File Offset: 0x001F1D5C
		public void CreateBubble()
		{
			if (this.thoughts.Count == 0)
			{
				return;
			}
			this.thoughts.Sort(new Comparison<Thought>(this.SortThoughts));
			Thought thought = this.thoughts[0];
			if (thought.modeSprite != null)
			{
				NameDisplayScreen.Instance.SetThoughtBubbleConvoDisplay(base.gameObject, true, thought.hoverText, thought.bubbleSprite, thought.sprite, thought.modeSprite);
			}
			else
			{
				NameDisplayScreen.Instance.SetThoughtBubbleDisplay(base.gameObject, true, thought.hoverText, thought.bubbleSprite, thought.sprite);
			}
			base.sm.thoughtDisplayTime.Set(thought.showTime, this, false);
			this.currentThought = thought;
			if (thought.showImmediately)
			{
				this.thoughts.RemoveAt(0);
			}
		}

		// Token: 0x06002E0F RID: 11791 RVA: 0x000BDD0E File Offset: 0x000BBF0E
		public void DestroyBubble()
		{
			NameDisplayScreen.Instance.SetThoughtBubbleDisplay(base.gameObject, false, null, null, null);
			NameDisplayScreen.Instance.SetThoughtBubbleConvoDisplay(base.gameObject, false, null, null, null, null);
		}

		// Token: 0x04001EEA RID: 7914
		private List<Thought> thoughts = new List<Thought>();

		// Token: 0x04001EEB RID: 7915
		public Thought currentThought;
	}
}
