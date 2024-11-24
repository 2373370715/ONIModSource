using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x0200151F RID: 5407
public class BionicWaterDamageMonitor : GameStateMachine<BionicWaterDamageMonitor, BionicWaterDamageMonitor.Instance, IStateMachineTarget, BionicWaterDamageMonitor.Def>
{
	// Token: 0x060070D4 RID: 28884 RVA: 0x002F9138 File Offset: 0x002F7338
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.safe;
		this.safe.Transition(this.threat, new StateMachine<BionicWaterDamageMonitor, BionicWaterDamageMonitor.Instance, IStateMachineTarget, BionicWaterDamageMonitor.Def>.Transition.ConditionCallback(BionicWaterDamageMonitor.IsThreatened), UpdateRate.SIM_200ms);
		this.threat.Transition(this.safe, GameStateMachine<BionicWaterDamageMonitor, BionicWaterDamageMonitor.Instance, IStateMachineTarget, BionicWaterDamageMonitor.Def>.Not(new StateMachine<BionicWaterDamageMonitor, BionicWaterDamageMonitor.Instance, IStateMachineTarget, BionicWaterDamageMonitor.Def>.Transition.ConditionCallback(BionicWaterDamageMonitor.IsThreatened)), UpdateRate.SIM_200ms).Update(new Action<BionicWaterDamageMonitor.Instance, float>(BionicWaterDamageMonitor.ApplyDebuff), UpdateRate.SIM_200ms, false).Exit(new StateMachine<BionicWaterDamageMonitor, BionicWaterDamageMonitor.Instance, IStateMachineTarget, BionicWaterDamageMonitor.Def>.State.Callback(BionicWaterDamageMonitor.ClearNotification)).DefaultState(this.threat.idle).ToggleAnims("anim_bionic_hits_kanim", 3f);
		this.threat.idle.ParamTransition<float>(this.DamageCooldown, this.threat.damage, new StateMachine<BionicWaterDamageMonitor, BionicWaterDamageMonitor.Instance, IStateMachineTarget, BionicWaterDamageMonitor.Def>.Parameter<float>.Callback(BionicWaterDamageMonitor.IsTimeToZap)).ToggleReactable(new Func<BionicWaterDamageMonitor.Instance, Reactable>(BionicWaterDamageMonitor.ZapReactable)).Update(new Action<BionicWaterDamageMonitor.Instance, float>(BionicWaterDamageMonitor.DamageTimerUpdate), UpdateRate.SIM_200ms, false).Exit(new StateMachine<BionicWaterDamageMonitor, BionicWaterDamageMonitor.Instance, IStateMachineTarget, BionicWaterDamageMonitor.Def>.State.Callback(BionicWaterDamageMonitor.ResetDamageCooldown));
		this.threat.damage.Enter(new StateMachine<BionicWaterDamageMonitor, BionicWaterDamageMonitor.Instance, IStateMachineTarget, BionicWaterDamageMonitor.Def>.State.Callback(BionicWaterDamageMonitor.ApplyDamage)).Enter(new StateMachine<BionicWaterDamageMonitor, BionicWaterDamageMonitor.Instance, IStateMachineTarget, BionicWaterDamageMonitor.Def>.State.Callback(BionicWaterDamageMonitor.PlayDamageNotification)).GoTo(this.threat.idle).DoNotification((BionicWaterDamageMonitor.Instance smi) => smi.CreateLiquidDamageNotification());
	}

	// Token: 0x060070D5 RID: 28885 RVA: 0x000E9D07 File Offset: 0x000E7F07
	private static Reactable ZapReactable(BionicWaterDamageMonitor.Instance smi)
	{
		return smi.GetReactable();
	}

	// Token: 0x060070D6 RID: 28886 RVA: 0x000E9D0F File Offset: 0x000E7F0F
	private static bool IsThreatened(BionicWaterDamageMonitor.Instance smi)
	{
		return BionicWaterDamageMonitor.IsFloorWetWithIntolerantSubstance(smi);
	}

	// Token: 0x060070D7 RID: 28887 RVA: 0x000E9D17 File Offset: 0x000E7F17
	private static bool IsTimeToZap(BionicWaterDamageMonitor.Instance smi, float time)
	{
		return time > smi.def.ZapInterval;
	}

	// Token: 0x060070D8 RID: 28888 RVA: 0x000E9D27 File Offset: 0x000E7F27
	private static void ApplyDebuff(BionicWaterDamageMonitor.Instance smi, float dt)
	{
		smi.effects.Add("WaterDamage", true);
	}

	// Token: 0x060070D9 RID: 28889 RVA: 0x000E9D3B File Offset: 0x000E7F3B
	private static void ApplyDamage(BionicWaterDamageMonitor.Instance smi)
	{
		smi.ApplyDamage();
	}

	// Token: 0x060070DA RID: 28890 RVA: 0x000E9D43 File Offset: 0x000E7F43
	private static void ResetDamageCooldown(BionicWaterDamageMonitor.Instance smi)
	{
		smi.sm.DamageCooldown.Set(0f, smi, false);
	}

	// Token: 0x060070DB RID: 28891 RVA: 0x000E9D5D File Offset: 0x000E7F5D
	private static void PlayDamageNotification(BionicWaterDamageMonitor.Instance smi)
	{
		smi.PlayDamageNotification();
	}

	// Token: 0x060070DC RID: 28892 RVA: 0x000E9D65 File Offset: 0x000E7F65
	private static void ClearNotification(BionicWaterDamageMonitor.Instance smi)
	{
		smi.ClearNotification(null);
	}

	// Token: 0x060070DD RID: 28893 RVA: 0x002F92A0 File Offset: 0x002F74A0
	private static void DamageTimerUpdate(BionicWaterDamageMonitor.Instance smi, float dt)
	{
		float damageCooldown = smi.DamageCooldown;
		smi.sm.DamageCooldown.Set(damageCooldown + dt, smi, false);
	}

	// Token: 0x060070DE RID: 28894 RVA: 0x002F92CC File Offset: 0x002F74CC
	private static bool IsFloorWetWithIntolerantSubstance(BionicWaterDamageMonitor.Instance smi)
	{
		int num = Grid.PosToCell(smi);
		return Grid.IsValidCell(num) && Grid.Element[num].IsLiquid && !smi.kpid.HasTag(GameTags.HasAirtightSuit) && smi.def.IsElementIntolerable(Grid.Element[num].id);
	}

	// Token: 0x060070DF RID: 28895 RVA: 0x000E9D6E File Offset: 0x000E7F6E
	private static string GetZapAnimName(BionicWaterDamageMonitor.Instance smi)
	{
		if (smi.GetComponent<Navigator>().CurrentNavType != NavType.Ladder)
		{
			return "zapped";
		}
		return "ladder_zapped";
	}

	// Token: 0x0400544A RID: 21578
	public const string EFFECT_NAME = "WaterDamage";

	// Token: 0x0400544B RID: 21579
	public GameStateMachine<BionicWaterDamageMonitor, BionicWaterDamageMonitor.Instance, IStateMachineTarget, BionicWaterDamageMonitor.Def>.State safe;

	// Token: 0x0400544C RID: 21580
	public BionicWaterDamageMonitor.DamageStates threat;

	// Token: 0x0400544D RID: 21581
	public StateMachine<BionicWaterDamageMonitor, BionicWaterDamageMonitor.Instance, IStateMachineTarget, BionicWaterDamageMonitor.Def>.FloatParameter DamageCooldown;

	// Token: 0x02001520 RID: 5408
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x060070E1 RID: 28897 RVA: 0x002F9324 File Offset: 0x002F7524
		public bool IsElementIntolerable(SimHashes element)
		{
			for (int i = 0; i < this.IntolerantToElements.Length; i++)
			{
				if (this.IntolerantToElements[i] == element)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0400544E RID: 21582
		public readonly SimHashes[] IntolerantToElements = new SimHashes[]
		{
			SimHashes.Water,
			SimHashes.DirtyWater,
			SimHashes.SaltWater,
			SimHashes.Brine
		};

		// Token: 0x0400544F RID: 21583
		public float DamagePointsTakenPerShock = 20f;

		// Token: 0x04005450 RID: 21584
		public float ZapInterval = 15f;
	}

	// Token: 0x02001521 RID: 5409
	public class DamageStates : GameStateMachine<BionicWaterDamageMonitor, BionicWaterDamageMonitor.Instance, IStateMachineTarget, BionicWaterDamageMonitor.Def>.State
	{
		// Token: 0x04005451 RID: 21585
		public GameStateMachine<BionicWaterDamageMonitor, BionicWaterDamageMonitor.Instance, IStateMachineTarget, BionicWaterDamageMonitor.Def>.State idle;

		// Token: 0x04005452 RID: 21586
		public GameStateMachine<BionicWaterDamageMonitor, BionicWaterDamageMonitor.Instance, IStateMachineTarget, BionicWaterDamageMonitor.Def>.State damage;
	}

	// Token: 0x02001522 RID: 5410
	public new class Instance : GameStateMachine<BionicWaterDamageMonitor, BionicWaterDamageMonitor.Instance, IStateMachineTarget, BionicWaterDamageMonitor.Def>.GameInstance
	{
		// Token: 0x1700074D RID: 1869
		// (get) Token: 0x060070E4 RID: 28900 RVA: 0x000E9DD0 File Offset: 0x000E7FD0
		public float DamageCooldown
		{
			get
			{
				return base.sm.DamageCooldown.Get(this);
			}
		}

		// Token: 0x1700074E RID: 1870
		// (get) Token: 0x060070E5 RID: 28901 RVA: 0x000E9DE3 File Offset: 0x000E7FE3
		public bool IsAffectedByWaterDamage
		{
			get
			{
				return this.effects.HasEffect("WaterDamage");
			}
		}

		// Token: 0x060070E6 RID: 28902 RVA: 0x000E9DF5 File Offset: 0x000E7FF5
		public Instance(IStateMachineTarget master, BionicWaterDamageMonitor.Def def) : base(master, def)
		{
			this.health = base.GetComponent<Health>();
			this.effects = base.GetComponent<Effects>();
		}

		// Token: 0x060070E7 RID: 28903 RVA: 0x002F9354 File Offset: 0x002F7554
		public void ApplyDamage()
		{
			this.health.Damage(base.def.DamagePointsTakenPerShock);
			int num = Grid.PosToCell(base.gameObject);
			if (Grid.IsValidCell(num))
			{
				this.lastElementDamagedBy = (base.smi.def.IsElementIntolerable(Grid.Element[num].id) ? Grid.Element[num] : null);
			}
		}

		// Token: 0x060070E8 RID: 28904 RVA: 0x002F93BC File Offset: 0x002F75BC
		public Reactable GetReactable()
		{
			SelfEmoteReactable selfEmoteReactable = new SelfEmoteReactable(base.master.gameObject, Db.Get().Emotes.Minion.WaterDamage.Id, Db.Get().ChoreTypes.WaterDamageZap, 0f, base.def.ZapInterval, float.PositiveInfinity, 0f);
			Emote waterDamage = Db.Get().Emotes.Minion.WaterDamage;
			selfEmoteReactable.SetEmote(waterDamage);
			selfEmoteReactable.preventChoreInterruption = true;
			return selfEmoteReactable;
		}

		// Token: 0x060070E9 RID: 28905 RVA: 0x002F9444 File Offset: 0x002F7644
		public Notification CreateLiquidDamageNotification()
		{
			KSelectable component = base.GetComponent<KSelectable>();
			return new Notification(MISC.NOTIFICATIONS.BIONICLIQUIDDAMAGE.NAME, NotificationType.Bad, (List<Notification> notificationList, object data) => MISC.NOTIFICATIONS.BIONICLIQUIDDAMAGE.TOOLTIP + notificationList.ReduceMessages(false), "/t• " + component.GetProperName(), true, 0f, new Notification.ClickCallback(this.OnNotificationClicked), base.gameObject, null, true, false, false);
		}

		// Token: 0x060070EA RID: 28906 RVA: 0x002F94B4 File Offset: 0x002F76B4
		private void OnNotificationClicked(object data)
		{
			if (data != null)
			{
				GameObject gameObject = (GameObject)data;
				if (gameObject != null)
				{
					GameUtil.FocusCamera(gameObject.transform, true);
				}
			}
		}

		// Token: 0x060070EB RID: 28907 RVA: 0x002F94E0 File Offset: 0x002F76E0
		public void ClearNotification(Notifier _notifier = null)
		{
			Notifier notifier = (_notifier == null) ? base.gameObject.AddOrGet<Notifier>() : _notifier;
			if (this.lastNotificationPlayed != null)
			{
				notifier.Remove(this.lastNotificationPlayed);
				this.lastNotificationPlayed = null;
			}
		}

		// Token: 0x060070EC RID: 28908 RVA: 0x002F9520 File Offset: 0x002F7720
		public void PlayDamageNotification()
		{
			Notifier notifier = base.gameObject.AddOrGet<Notifier>();
			this.ClearNotification(notifier);
			Notification notification = this.CreateLiquidDamageNotification();
			notifier.Add(notification, "");
			this.lastNotificationPlayed = notification;
		}

		// Token: 0x04005453 RID: 21587
		public Effects effects;

		// Token: 0x04005454 RID: 21588
		private Health health;

		// Token: 0x04005455 RID: 21589
		public Element lastElementDamagedBy;

		// Token: 0x04005456 RID: 21590
		private Notification lastNotificationPlayed;

		// Token: 0x04005457 RID: 21591
		[MyCmpGet]
		public KPrefabID kpid;
	}
}
