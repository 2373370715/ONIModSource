using System;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02003B2E RID: 15150
	public class PeriodicEmoteSickness : Sickness.SicknessComponent
	{
		// Token: 0x0600E92E RID: 59694 RVA: 0x0013BDB1 File Offset: 0x00139FB1
		public PeriodicEmoteSickness(Emote emote, float cooldown)
		{
			this.emote = emote;
			this.cooldown = cooldown;
		}

		// Token: 0x0600E92F RID: 59695 RVA: 0x0013BDC7 File Offset: 0x00139FC7
		public override object OnInfect(GameObject go, SicknessInstance diseaseInstance)
		{
			PeriodicEmoteSickness.StatesInstance statesInstance = new PeriodicEmoteSickness.StatesInstance(diseaseInstance, this);
			statesInstance.StartSM();
			return statesInstance;
		}

		// Token: 0x0600E930 RID: 59696 RVA: 0x0013BDD6 File Offset: 0x00139FD6
		public override void OnCure(GameObject go, object instance_data)
		{
			((PeriodicEmoteSickness.StatesInstance)instance_data).StopSM("Cured");
		}

		// Token: 0x0400E4C1 RID: 58561
		private Emote emote;

		// Token: 0x0400E4C2 RID: 58562
		private float cooldown;

		// Token: 0x02003B2F RID: 15151
		public class StatesInstance : GameStateMachine<PeriodicEmoteSickness.States, PeriodicEmoteSickness.StatesInstance, SicknessInstance, object>.GameInstance
		{
			// Token: 0x0600E931 RID: 59697 RVA: 0x0013BDE8 File Offset: 0x00139FE8
			public StatesInstance(SicknessInstance master, PeriodicEmoteSickness periodicEmoteSickness) : base(master)
			{
				this.periodicEmoteSickness = periodicEmoteSickness;
			}

			// Token: 0x0600E932 RID: 59698 RVA: 0x004C56FC File Offset: 0x004C38FC
			public Reactable GetReactable()
			{
				return new SelfEmoteReactable(base.master.gameObject, "PeriodicEmoteSickness", Db.Get().ChoreTypes.Emote, 0f, this.periodicEmoteSickness.cooldown, float.PositiveInfinity, 0f).SetEmote(this.periodicEmoteSickness.emote).SetOverideAnimSet("anim_sneeze_kanim");
			}

			// Token: 0x0400E4C3 RID: 58563
			public PeriodicEmoteSickness periodicEmoteSickness;
		}

		// Token: 0x02003B30 RID: 15152
		public class States : GameStateMachine<PeriodicEmoteSickness.States, PeriodicEmoteSickness.StatesInstance, SicknessInstance>
		{
			// Token: 0x0600E933 RID: 59699 RVA: 0x0013BDF8 File Offset: 0x00139FF8
			public override void InitializeStates(out StateMachine.BaseState default_state)
			{
				default_state = this.root;
				this.root.ToggleReactable((PeriodicEmoteSickness.StatesInstance smi) => smi.GetReactable());
			}
		}
	}
}
