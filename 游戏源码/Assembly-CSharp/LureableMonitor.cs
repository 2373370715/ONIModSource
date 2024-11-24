using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000A1F RID: 2591
public class LureableMonitor : GameStateMachine<LureableMonitor, LureableMonitor.Instance, IStateMachineTarget, LureableMonitor.Def>
{
	// Token: 0x06002F5B RID: 12123 RVA: 0x001F7CE0 File Offset: 0x001F5EE0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.cooldown;
		this.cooldown.ScheduleGoTo((LureableMonitor.Instance smi) => smi.def.cooldown, this.nolure);
		this.nolure.PreBrainUpdate(delegate(LureableMonitor.Instance smi)
		{
			smi.FindLure();
		}).ParamTransition<GameObject>(this.targetLure, this.haslure, (LureableMonitor.Instance smi, GameObject p) => p != null);
		this.haslure.ParamTransition<GameObject>(this.targetLure, this.nolure, (LureableMonitor.Instance smi, GameObject p) => p == null).PreBrainUpdate(delegate(LureableMonitor.Instance smi)
		{
			smi.FindLure();
		}).ToggleBehaviour(GameTags.Creatures.MoveToLure, (LureableMonitor.Instance smi) => smi.HasLure(), delegate(LureableMonitor.Instance smi)
		{
			smi.GoTo(this.cooldown);
		});
	}

	// Token: 0x04001FFF RID: 8191
	public StateMachine<LureableMonitor, LureableMonitor.Instance, IStateMachineTarget, LureableMonitor.Def>.TargetParameter targetLure;

	// Token: 0x04002000 RID: 8192
	public GameStateMachine<LureableMonitor, LureableMonitor.Instance, IStateMachineTarget, LureableMonitor.Def>.State nolure;

	// Token: 0x04002001 RID: 8193
	public GameStateMachine<LureableMonitor, LureableMonitor.Instance, IStateMachineTarget, LureableMonitor.Def>.State haslure;

	// Token: 0x04002002 RID: 8194
	public GameStateMachine<LureableMonitor, LureableMonitor.Instance, IStateMachineTarget, LureableMonitor.Def>.State cooldown;

	// Token: 0x02000A20 RID: 2592
	public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
	{
		// Token: 0x06002F5E RID: 12126 RVA: 0x000BEC1E File Offset: 0x000BCE1E
		public List<Descriptor> GetDescriptors(GameObject go)
		{
			return new List<Descriptor>
			{
				new Descriptor(UI.BUILDINGEFFECTS.CAPTURE_METHOD_LURE, UI.BUILDINGEFFECTS.TOOLTIPS.CAPTURE_METHOD_LURE, Descriptor.DescriptorType.Effect, false)
			};
		}

		// Token: 0x04002003 RID: 8195
		public float cooldown = 20f;

		// Token: 0x04002004 RID: 8196
		public Tag[] lures;
	}

	// Token: 0x02000A21 RID: 2593
	public new class Instance : GameStateMachine<LureableMonitor, LureableMonitor.Instance, IStateMachineTarget, LureableMonitor.Def>.GameInstance
	{
		// Token: 0x06002F60 RID: 12128 RVA: 0x000BEC59 File Offset: 0x000BCE59
		public Instance(IStateMachineTarget master, LureableMonitor.Def def) : base(master, def)
		{
		}

		// Token: 0x06002F61 RID: 12129 RVA: 0x001F7E14 File Offset: 0x001F6014
		public void FindLure()
		{
			int num = -1;
			GameObject value = null;
			foreach (object obj in GameScenePartitioner.Instance.AsyncSafeEnumerate(Grid.PosToCell(base.smi.transform.GetPosition()), 1, GameScenePartitioner.Instance.lure))
			{
				Lure.Instance instance = obj as Lure.Instance;
				if (instance == null || !instance.IsActive() || !instance.HasAnyLure(base.def.lures))
				{
					return;
				}
				int navigationCost = this.navigator.GetNavigationCost(Grid.PosToCell(instance.transform.GetPosition()), instance.LurePoints);
				if (navigationCost != -1 && (num == -1 || navigationCost < num))
				{
					num = navigationCost;
					value = instance.gameObject;
				}
			}
			base.sm.targetLure.Set(value, this, false);
		}

		// Token: 0x06002F62 RID: 12130 RVA: 0x000BEC63 File Offset: 0x000BCE63
		public bool HasLure()
		{
			return base.sm.targetLure.Get(this) != null;
		}

		// Token: 0x06002F63 RID: 12131 RVA: 0x000BEC7C File Offset: 0x000BCE7C
		public GameObject GetTargetLure()
		{
			return base.sm.targetLure.Get(this);
		}

		// Token: 0x04002005 RID: 8197
		[MyCmpReq]
		private Navigator navigator;
	}
}
