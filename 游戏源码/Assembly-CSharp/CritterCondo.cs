using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000D12 RID: 3346
public class CritterCondo : GameStateMachine<CritterCondo, CritterCondo.Instance, IStateMachineTarget, CritterCondo.Def>
{
	// Token: 0x0600416E RID: 16750 RVA: 0x0023DDC8 File Offset: 0x0023BFC8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.inoperational;
		this.inoperational.PlayAnim("off").EventTransition(GameHashes.UpdateRoom, this.operational, new StateMachine<CritterCondo, CritterCondo.Instance, IStateMachineTarget, CritterCondo.Def>.Transition.ConditionCallback(CritterCondo.IsOperational)).EventTransition(GameHashes.OperationalChanged, this.operational, new StateMachine<CritterCondo, CritterCondo.Instance, IStateMachineTarget, CritterCondo.Def>.Transition.ConditionCallback(CritterCondo.IsOperational));
		this.operational.PlayAnim("on", KAnim.PlayMode.Loop).EventTransition(GameHashes.UpdateRoom, this.inoperational, GameStateMachine<CritterCondo, CritterCondo.Instance, IStateMachineTarget, CritterCondo.Def>.Not(new StateMachine<CritterCondo, CritterCondo.Instance, IStateMachineTarget, CritterCondo.Def>.Transition.ConditionCallback(CritterCondo.IsOperational))).EventTransition(GameHashes.OperationalChanged, this.inoperational, GameStateMachine<CritterCondo, CritterCondo.Instance, IStateMachineTarget, CritterCondo.Def>.Not(new StateMachine<CritterCondo, CritterCondo.Instance, IStateMachineTarget, CritterCondo.Def>.Transition.ConditionCallback(CritterCondo.IsOperational)));
	}

	// Token: 0x0600416F RID: 16751 RVA: 0x000CA542 File Offset: 0x000C8742
	private static bool IsOperational(CritterCondo.Instance smi)
	{
		return smi.def.IsCritterCondoOperationalCb(smi);
	}

	// Token: 0x04002C95 RID: 11413
	public GameStateMachine<CritterCondo, CritterCondo.Instance, IStateMachineTarget, CritterCondo.Def>.State inoperational;

	// Token: 0x04002C96 RID: 11414
	public GameStateMachine<CritterCondo, CritterCondo.Instance, IStateMachineTarget, CritterCondo.Def>.State operational;

	// Token: 0x02000D13 RID: 3347
	public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
	{
		// Token: 0x06004171 RID: 16753 RVA: 0x000C9B47 File Offset: 0x000C7D47
		public List<Descriptor> GetDescriptors(GameObject go)
		{
			return new List<Descriptor>();
		}

		// Token: 0x04002C97 RID: 11415
		public Func<CritterCondo.Instance, bool> IsCritterCondoOperationalCb;

		// Token: 0x04002C98 RID: 11416
		public StatusItem moveToStatusItem;

		// Token: 0x04002C99 RID: 11417
		public StatusItem interactStatusItem;

		// Token: 0x04002C9A RID: 11418
		public Tag condoTag = "CritterCondo";

		// Token: 0x04002C9B RID: 11419
		public string effectId;
	}

	// Token: 0x02000D14 RID: 3348
	public new class Instance : GameStateMachine<CritterCondo, CritterCondo.Instance, IStateMachineTarget, CritterCondo.Def>.GameInstance
	{
		// Token: 0x06004173 RID: 16755 RVA: 0x000CA575 File Offset: 0x000C8775
		public Instance(IStateMachineTarget master, CritterCondo.Def def) : base(master, def)
		{
		}

		// Token: 0x06004174 RID: 16756 RVA: 0x000CA57F File Offset: 0x000C877F
		public override void StartSM()
		{
			base.StartSM();
			Components.CritterCondos.Add(base.smi.GetMyWorldId(), this);
		}

		// Token: 0x06004175 RID: 16757 RVA: 0x000CA59D File Offset: 0x000C879D
		protected override void OnCleanUp()
		{
			Components.CritterCondos.Remove(base.smi.GetMyWorldId(), this);
		}

		// Token: 0x06004176 RID: 16758 RVA: 0x000CA5B5 File Offset: 0x000C87B5
		public bool IsReserved()
		{
			return base.HasTag(GameTags.Creatures.ReservedByCreature);
		}

		// Token: 0x06004177 RID: 16759 RVA: 0x0023DE7C File Offset: 0x0023C07C
		public void SetReserved(bool isReserved)
		{
			if (isReserved)
			{
				base.GetComponent<KPrefabID>().SetTag(GameTags.Creatures.ReservedByCreature, true);
				return;
			}
			if (base.HasTag(GameTags.Creatures.ReservedByCreature))
			{
				base.GetComponent<KPrefabID>().RemoveTag(GameTags.Creatures.ReservedByCreature);
				return;
			}
			global::Debug.LogWarningFormat(base.smi.gameObject, "Tried to unreserve a condo that wasn't reserved", Array.Empty<object>());
		}

		// Token: 0x06004178 RID: 16760 RVA: 0x000CA5C2 File Offset: 0x000C87C2
		public int GetInteractStartCell()
		{
			return Grid.PosToCell(this);
		}

		// Token: 0x06004179 RID: 16761 RVA: 0x000CA5CA File Offset: 0x000C87CA
		public bool CanBeReserved()
		{
			return !this.IsReserved() && CritterCondo.IsOperational(this);
		}
	}
}
