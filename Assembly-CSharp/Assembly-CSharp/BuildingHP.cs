﻿using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/Workable/BuildingHP")]
public class BuildingHP : Workable
{
		public int HitPoints
	{
		get
		{
			return this.hitpoints;
		}
	}

	public void SetHitPoints(int hp)
	{
		this.hitpoints = hp;
	}

		public int MaxHitPoints
	{
		get
		{
			return this.building.Def.HitPoints;
		}
	}

	public BuildingHP.DamageSourceInfo GetDamageSourceInfo()
	{
		return this.damageSourceInfo;
	}

	protected override void OnLoadLevel()
	{
		this.smi = null;
		base.OnLoadLevel();
	}

	public void DoDamage(int damage)
	{
		if (!this.invincible)
		{
			damage = Math.Max(0, damage);
			this.hitpoints = Math.Max(0, this.hitpoints - damage);
			base.Trigger(-1964935036, this);
		}
	}

	public void Repair(int repair_amount)
	{
		if (this.hitpoints + repair_amount < this.hitpoints)
		{
			this.hitpoints = this.building.Def.HitPoints;
		}
		else
		{
			this.hitpoints = Math.Min(this.hitpoints + repair_amount, this.building.Def.HitPoints);
		}
		base.Trigger(-1699355994, this);
		if (this.hitpoints >= this.building.Def.HitPoints)
		{
			base.Trigger(-1735440190, this);
		}
	}

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.SetWorkTime(10f);
		this.multitoolContext = "build";
		this.multitoolHitEffectTag = EffectConfigs.BuildSplashId;
	}

	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.smi = new BuildingHP.SMInstance(this);
		this.smi.StartSM();
		base.Subscribe<BuildingHP>(-794517298, BuildingHP.OnDoBuildingDamageDelegate);
		if (this.destroyOnDamaged)
		{
			base.Subscribe<BuildingHP>(774203113, BuildingHP.DestroyOnDamagedDelegate);
		}
		if (this.hitpoints <= 0)
		{
			base.Trigger(774203113, this);
		}
	}

	private void DestroyOnDamaged(object data)
	{
		Util.KDestroyGameObject(base.gameObject);
	}

	protected override void OnCompleteWork(Worker worker)
	{
		int num = (int)Db.Get().Attributes.Machinery.Lookup(worker).GetTotalValue();
		int repair_amount = 10 + Math.Max(0, num * 10);
		this.Repair(repair_amount);
	}

	private void OnDoBuildingDamage(object data)
	{
		if (this.invincible)
		{
			return;
		}
		this.damageSourceInfo = (BuildingHP.DamageSourceInfo)data;
		this.DoDamage(this.damageSourceInfo.damage);
		this.DoDamagePopFX(this.damageSourceInfo);
		this.DoTakeDamageFX(this.damageSourceInfo);
	}

	private void DoTakeDamageFX(BuildingHP.DamageSourceInfo info)
	{
		if (info.takeDamageEffect != SpawnFXHashes.None)
		{
			BuildingDef def = base.GetComponent<BuildingComplete>().Def;
			int cell = Grid.OffsetCell(Grid.PosToCell(this), 0, def.HeightInCells - 1);
			Game.Instance.SpawnFX(info.takeDamageEffect, cell, 0f);
		}
	}

	private void DoDamagePopFX(BuildingHP.DamageSourceInfo info)
	{
		if (info.popString != null && Time.time > this.lastPopTime + this.minDamagePopInterval)
		{
			PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Building, info.popString, base.gameObject.transform, 1.5f, false);
			this.lastPopTime = Time.time;
		}
	}

		public bool IsBroken
	{
		get
		{
			return this.hitpoints == 0;
		}
	}

		public bool NeedsRepairs
	{
		get
		{
			return this.HitPoints < this.building.Def.HitPoints;
		}
	}

	[Serialize]
	[SerializeField]
	private int hitpoints;

	[Serialize]
	private BuildingHP.DamageSourceInfo damageSourceInfo;

	private static readonly EventSystem.IntraObjectHandler<BuildingHP> OnDoBuildingDamageDelegate = new EventSystem.IntraObjectHandler<BuildingHP>(delegate(BuildingHP component, object data)
	{
		component.OnDoBuildingDamage(data);
	});

	private static readonly EventSystem.IntraObjectHandler<BuildingHP> DestroyOnDamagedDelegate = new EventSystem.IntraObjectHandler<BuildingHP>(delegate(BuildingHP component, object data)
	{
		component.DestroyOnDamaged(data);
	});

	public static List<Meter> kbacQueryList = new List<Meter>();

	public bool destroyOnDamaged;

	public bool invincible;

	[MyCmpGet]
	private Building building;

	private BuildingHP.SMInstance smi;

	private float minDamagePopInterval = 4f;

	private float lastPopTime;

	public struct DamageSourceInfo
	{
		public override string ToString()
		{
			return this.source;
		}

		public int damage;

		public string source;

		public string popString;

		public SpawnFXHashes takeDamageEffect;

		public string fullDamageEffectName;

		public string statusItemID;
	}

	public class SMInstance : GameStateMachine<BuildingHP.States, BuildingHP.SMInstance, BuildingHP, object>.GameInstance
	{
		public SMInstance(BuildingHP master) : base(master)
		{
		}

		public Notification CreateBrokenMachineNotification()
		{
			return new Notification(MISC.NOTIFICATIONS.BROKENMACHINE.NAME, NotificationType.BadMinor, (List<Notification> notificationList, object data) => MISC.NOTIFICATIONS.BROKENMACHINE.TOOLTIP + notificationList.ReduceMessages(false), "/t• " + base.master.damageSourceInfo.source, false, 0f, null, null, null, true, false, false);
		}

		public void ShowProgressBar(bool show)
		{
			if (show && Grid.IsValidCell(Grid.PosToCell(base.gameObject)) && Grid.IsVisible(Grid.PosToCell(base.gameObject)))
			{
				this.CreateProgressBar();
				return;
			}
			if (this.progressBar != null)
			{
				this.progressBar.gameObject.DeleteObject();
				this.progressBar = null;
			}
		}

		public void UpdateMeter()
		{
			if (this.progressBar == null)
			{
				this.ShowProgressBar(true);
			}
			if (this.progressBar)
			{
				this.progressBar.Update();
			}
		}

		private float HealthPercent()
		{
			return (float)base.smi.master.HitPoints / (float)base.smi.master.building.Def.HitPoints;
		}

		private void CreateProgressBar()
		{
			if (this.progressBar != null)
			{
				return;
			}
			this.progressBar = Util.KInstantiateUI<ProgressBar>(ProgressBarsConfig.Instance.progressBarPrefab, null, false);
			this.progressBar.transform.SetParent(GameScreenManager.Instance.worldSpaceCanvas.transform);
			this.progressBar.name = base.smi.master.name + "." + base.smi.master.GetType().Name + " ProgressBar";
			this.progressBar.transform.Find("Bar").GetComponent<Image>().color = ProgressBarsConfig.Instance.GetBarColor("ProgressBar");
			this.progressBar.SetUpdateFunc(new Func<float>(this.HealthPercent));
			this.progressBar.barColor = ProgressBarsConfig.Instance.GetBarColor("HealthBar");
			CanvasGroup component = this.progressBar.GetComponent<CanvasGroup>();
			component.interactable = false;
			component.blocksRaycasts = false;
			this.progressBar.Update();
			float d = 0.15f;
			Vector3 vector = base.gameObject.transform.GetPosition() + Vector3.down * d;
			vector.z += 0.05f;
			Rotatable component2 = base.GetComponent<Rotatable>();
			if (component2 == null || component2.GetOrientation() == Orientation.Neutral || base.smi.master.building.Def.WidthInCells < 2 || base.smi.master.building.Def.HeightInCells < 2)
			{
				vector -= Vector3.right * 0.5f * (float)(base.smi.master.building.Def.WidthInCells % 2);
			}
			else
			{
				vector += Vector3.left * (1f + 0.5f * (float)(base.smi.master.building.Def.WidthInCells % 2));
			}
			this.progressBar.transform.SetPosition(vector);
			this.progressBar.SetVisibility(true);
		}

		private static string ToolTipResolver(List<Notification> notificationList, object data)
		{
			string text = "";
			for (int i = 0; i < notificationList.Count; i++)
			{
				Notification notification = notificationList[i];
				text += string.Format(BUILDINGS.DAMAGESOURCES.NOTIFICATION_TOOLTIP, notification.NotifierName, (string)notification.tooltipData);
				if (i < notificationList.Count - 1)
				{
					text += "\n";
				}
			}
			return text;
		}

		public void ShowDamagedEffect()
		{
			if (base.master.damageSourceInfo.takeDamageEffect != SpawnFXHashes.None)
			{
				BuildingDef def = base.master.GetComponent<BuildingComplete>().Def;
				int cell = Grid.OffsetCell(Grid.PosToCell(base.master), 0, def.HeightInCells - 1);
				Game.Instance.SpawnFX(base.master.damageSourceInfo.takeDamageEffect, cell, 0f);
			}
		}

		public FXAnim.Instance InstantiateDamageFX()
		{
			if (base.master.damageSourceInfo.fullDamageEffectName == null)
			{
				return null;
			}
			BuildingDef def = base.master.GetComponent<BuildingComplete>().Def;
			Vector3 zero = Vector3.zero;
			if (def.HeightInCells > 1)
			{
				zero = new Vector3(0f, (float)(def.HeightInCells - 1), 0f);
			}
			else
			{
				zero = new Vector3(0f, 0.5f, 0f);
			}
			return new FXAnim.Instance(base.smi.master, base.master.damageSourceInfo.fullDamageEffectName, "idle", KAnim.PlayMode.Loop, zero, Color.white);
		}

		public void SetCrackOverlayValue(float value)
		{
			KBatchedAnimController component = base.master.GetComponent<KBatchedAnimController>();
			if (component == null)
			{
				return;
			}
			component.SetBlendValue(value);
			BuildingHP.kbacQueryList.Clear();
			base.master.GetComponentsInChildren<Meter>(BuildingHP.kbacQueryList);
			for (int i = 0; i < BuildingHP.kbacQueryList.Count; i++)
			{
				BuildingHP.kbacQueryList[i].GetComponent<KBatchedAnimController>().SetBlendValue(value);
			}
		}

		private ProgressBar progressBar;
	}

	public class States : GameStateMachine<BuildingHP.States, BuildingHP.SMInstance, BuildingHP>
	{
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			default_state = this.healthy;
			this.healthy.DefaultState(this.healthy.imperfect).EventTransition(GameHashes.BuildingReceivedDamage, this.damaged, (BuildingHP.SMInstance smi) => smi.master.HitPoints <= 0);
			this.healthy.imperfect.Enter(delegate(BuildingHP.SMInstance smi)
			{
				smi.ShowProgressBar(true);
			}).DefaultState(this.healthy.imperfect.playEffect).EventTransition(GameHashes.BuildingPartiallyRepaired, this.healthy.perfect, (BuildingHP.SMInstance smi) => smi.master.HitPoints == smi.master.building.Def.HitPoints).EventHandler(GameHashes.BuildingPartiallyRepaired, delegate(BuildingHP.SMInstance smi)
			{
				smi.UpdateMeter();
			}).ToggleStatusItem(delegate(BuildingHP.SMInstance smi)
			{
				if (smi.master.damageSourceInfo.statusItemID == null)
				{
					return null;
				}
				return Db.Get().BuildingStatusItems.Get(smi.master.damageSourceInfo.statusItemID);
			}, null).Exit(delegate(BuildingHP.SMInstance smi)
			{
				smi.ShowProgressBar(false);
			});
			this.healthy.imperfect.playEffect.Transition(this.healthy.imperfect.waiting, (BuildingHP.SMInstance smi) => true, UpdateRate.SIM_200ms);
			this.healthy.imperfect.waiting.ScheduleGoTo((BuildingHP.SMInstance smi) => UnityEngine.Random.Range(15f, 30f), this.healthy.imperfect.playEffect);
			this.healthy.perfect.EventTransition(GameHashes.BuildingReceivedDamage, this.healthy.imperfect, (BuildingHP.SMInstance smi) => smi.master.HitPoints < smi.master.building.Def.HitPoints);
			this.damaged.Enter(delegate(BuildingHP.SMInstance smi)
			{
				Operational component = smi.GetComponent<Operational>();
				if (component != null)
				{
					component.SetFlag(BuildingHP.States.healthyFlag, false);
				}
				smi.ShowProgressBar(true);
				smi.master.Trigger(774203113, smi.master);
				smi.SetCrackOverlayValue(1f);
			}).ToggleNotification((BuildingHP.SMInstance smi) => smi.CreateBrokenMachineNotification()).ToggleStatusItem(Db.Get().BuildingStatusItems.Broken, null).ToggleFX((BuildingHP.SMInstance smi) => smi.InstantiateDamageFX()).EventTransition(GameHashes.BuildingPartiallyRepaired, this.healthy.perfect, (BuildingHP.SMInstance smi) => smi.master.HitPoints == smi.master.building.Def.HitPoints).EventHandler(GameHashes.BuildingPartiallyRepaired, delegate(BuildingHP.SMInstance smi)
			{
				smi.UpdateMeter();
			}).Exit(delegate(BuildingHP.SMInstance smi)
			{
				Operational component = smi.GetComponent<Operational>();
				if (component != null)
				{
					component.SetFlag(BuildingHP.States.healthyFlag, true);
				}
				smi.ShowProgressBar(false);
				smi.SetCrackOverlayValue(0f);
			});
		}

		private Chore CreateRepairChore(BuildingHP.SMInstance smi)
		{
			return new WorkChore<BuildingHP>(Db.Get().ChoreTypes.Repair, smi.master, null, true, null, null, null, true, null, false, false, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		}

		private static readonly Operational.Flag healthyFlag = new Operational.Flag("healthy", Operational.Flag.Type.Functional);

		public GameStateMachine<BuildingHP.States, BuildingHP.SMInstance, BuildingHP, object>.State damaged;

		public BuildingHP.States.Healthy healthy;

		public class Healthy : GameStateMachine<BuildingHP.States, BuildingHP.SMInstance, BuildingHP, object>.State
		{
			public BuildingHP.States.ImperfectStates imperfect;

			public GameStateMachine<BuildingHP.States, BuildingHP.SMInstance, BuildingHP, object>.State perfect;
		}

		public class ImperfectStates : GameStateMachine<BuildingHP.States, BuildingHP.SMInstance, BuildingHP, object>.State
		{
			public GameStateMachine<BuildingHP.States, BuildingHP.SMInstance, BuildingHP, object>.State playEffect;

			public GameStateMachine<BuildingHP.States, BuildingHP.SMInstance, BuildingHP, object>.State waiting;
		}
	}
}
