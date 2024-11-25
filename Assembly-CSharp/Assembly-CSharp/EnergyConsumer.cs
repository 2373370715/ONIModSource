﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using FMOD.Studio;
using FMODUnity;
using KSerialization;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
[DebuggerDisplay("{name} {WattsUsed}W")]
[AddComponentMenu("KMonoBehaviour/scripts/EnergyConsumer")]
public class EnergyConsumer : KMonoBehaviour, ISaveLoadable, IEnergyConsumer, ICircuitConnected, IGameObjectEffectDescriptor
{
			public int PowerSortOrder
	{
		get
		{
			return this.powerSortOrder;
		}
	}

				public int PowerCell { get; private set; }

			public bool HasWire
	{
		get
		{
			return Grid.Objects[this.PowerCell, 26] != null;
		}
	}

				public virtual bool IsPowered
	{
		get
		{
			return this.operational.GetFlag(EnergyConsumer.PoweredFlag);
		}
		protected set
		{
			this.operational.SetFlag(EnergyConsumer.PoweredFlag, value);
		}
	}

			public bool IsConnected
	{
		get
		{
			return this.CircuitID != ushort.MaxValue;
		}
	}

			public string Name
	{
		get
		{
			return this.selectable.GetName();
		}
	}

				public bool IsVirtual { get; private set; }

				public object VirtualCircuitKey { get; private set; }

				public ushort CircuitID { get; private set; }

				public float BaseWattageRating
	{
		get
		{
			return this._BaseWattageRating;
		}
		set
		{
			this._BaseWattageRating = value;
		}
	}

			public float WattsUsed
	{
		get
		{
			if (this.operational.IsActive)
			{
				return this.BaseWattageRating;
			}
			return 0f;
		}
	}

			public float WattsNeededWhenActive
	{
		get
		{
			return this.building.Def.EnergyConsumptionWhenActive;
		}
	}

		protected override void OnPrefabInit()
	{
		this.CircuitID = ushort.MaxValue;
		this.IsPowered = false;
		this.BaseWattageRating = this.building.Def.EnergyConsumptionWhenActive;
	}

		protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.EnergyConsumers.Add(this);
		Building component = base.GetComponent<Building>();
		this.PowerCell = component.GetPowerInputCell();
		Game.Instance.circuitManager.Connect(this);
		Game.Instance.energySim.AddEnergyConsumer(this);
	}

		protected override void OnCleanUp()
	{
		Game.Instance.energySim.RemoveEnergyConsumer(this);
		Game.Instance.circuitManager.Disconnect(this, true);
		Components.EnergyConsumers.Remove(this);
		base.OnCleanUp();
	}

		public virtual void EnergySim200ms(float dt)
	{
		this.CircuitID = Game.Instance.circuitManager.GetCircuitID(this);
		if (!this.IsConnected)
		{
			this.IsPowered = false;
		}
		this.circuitOverloadTime = Mathf.Max(0f, this.circuitOverloadTime - dt);
	}

		public virtual void SetConnectionStatus(CircuitManager.ConnectionStatus connection_status)
	{
		switch (connection_status)
		{
		case CircuitManager.ConnectionStatus.NotConnected:
			this.IsPowered = false;
			return;
		case CircuitManager.ConnectionStatus.Unpowered:
			if (this.IsPowered && base.GetComponent<Battery>() == null)
			{
				this.IsPowered = false;
				this.circuitOverloadTime = 6f;
				this.PlayCircuitSound("overdraw");
				return;
			}
			break;
		case CircuitManager.ConnectionStatus.Powered:
			if (!this.IsPowered && this.circuitOverloadTime <= 0f)
			{
				this.IsPowered = true;
				this.PlayCircuitSound("powered");
			}
			break;
		default:
			return;
		}
	}

		protected void PlayCircuitSound(string state)
	{
		EventReference event_ref;
		if (state == "powered")
		{
			event_ref = Sounds.Instance.BuildingPowerOnMigrated;
		}
		else if (state == "overdraw")
		{
			event_ref = Sounds.Instance.ElectricGridOverloadMigrated;
		}
		else
		{
			event_ref = default(EventReference);
			global::Debug.Log("Invalid state for sound in EnergyConsumer.");
		}
		if (!CameraController.Instance.IsAudibleSound(base.transform.GetPosition()))
		{
			return;
		}
		float num;
		if (!this.lastTimeSoundPlayed.TryGetValue(state, out num))
		{
			num = 0f;
		}
		float value = (Time.time - num) / this.soundDecayTime;
		Vector3 position = base.transform.GetPosition();
		position.z = 0f;
		FMOD.Studio.EventInstance instance = KFMOD.BeginOneShot(event_ref, CameraController.Instance.GetVerticallyScaledPosition(position, false), 1f);
		instance.setParameterByName("timeSinceLast", value, false);
		KFMOD.EndOneShot(instance);
		this.lastTimeSoundPlayed[state] = Time.time;
	}

		public List<Descriptor> GetDescriptors(GameObject go)
	{
		return null;
	}

		[MyCmpReq]
	private Building building;

		[MyCmpGet]
	protected Operational operational;

		[MyCmpGet]
	private KSelectable selectable;

		[SerializeField]
	public int powerSortOrder;

		[Serialize]
	protected float circuitOverloadTime;

		public static readonly Operational.Flag PoweredFlag = new Operational.Flag("powered", Operational.Flag.Type.Requirement);

		private Dictionary<string, float> lastTimeSoundPlayed = new Dictionary<string, float>();

		private float soundDecayTime = 10f;

		private float _BaseWattageRating;
}
