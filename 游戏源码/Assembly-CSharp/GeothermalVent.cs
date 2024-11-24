using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

public class GeothermalVent : StateMachineComponent<GeothermalVent.StatesInstance>, ISim200ms
{
	private enum QuestProgress
	{
		Uninitialized,
		Entombed,
		Complete
	}

	public struct ElementInfo : IComparable
	{
		public bool isSolid;

		public ushort elementIdx;

		public float mass;

		public float temperature;

		public byte diseaseIdx;

		public int diseaseCount;

		public int CompareTo(object obj)
		{
			return -mass.CompareTo(((ElementInfo)obj).mass);
		}
	}

	public struct EmitterInfo
	{
		public int simHandle;

		public int cell;

		public ElementInfo element;

		public bool dirty;
	}

	public class States : GameStateMachine<States, StatesInstance, GeothermalVent>
	{
		public class ActiveStates : State
		{
			public class LoopStates : State
			{
				public State start;

				public State finish;
			}

			public LoopStates loopVent;

			public State preVent;

			public State postVent;
		}

		public class ProblemStates : State
		{
			public State identify;

			public State entombed;

			public State overpressure;
		}

		public class OnlineStates : State
		{
			public State identify;

			public State ready;

			public State disconnected;

			public ActiveStates active;

			public ProblemStates inactive;
		}

		public State questEntombed;

		public OnlineStates online;

		public override void InitializeStates(out BaseState default_state)
		{
			default_state = root;
			root.EnterTransition(questEntombed, (StatesInstance smi) => smi.master.IsQuestEntombed()).EnterTransition(online, (StatesInstance smi) => !smi.master.IsQuestEntombed());
			questEntombed.PlayAnim("pooped").ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoVentQuestBlockage, (StatesInstance smi) => smi.master).Transition(online, (StatesInstance smi) => smi.master.progress == QuestProgress.Complete);
			online.PlayAnim("on", KAnim.PlayMode.Once).defaultState = online.identify;
			online.identify.EnterTransition(online.inactive, HasProblem).EnterTransition(online.active, (StatesInstance smi) => !HasProblem(smi) && smi.master.HasMaterial()).EnterTransition(online.ready, (StatesInstance smi) => !HasProblem(smi) && !smi.master.HasMaterial() && smi.master.IsVentConnected())
				.EnterTransition(online.disconnected, (StatesInstance smi) => !HasProblem(smi) && !smi.master.HasMaterial() && !smi.master.IsVentConnected());
			online.active.defaultState = online.active.preVent;
			online.active.preVent.PlayAnim("working_pre").OnAnimQueueComplete(online.active.loopVent);
			online.active.loopVent.Enter(delegate(StatesInstance smi)
			{
				smi.master.RecomputeEmissions();
			}).Exit(delegate(StatesInstance smi)
			{
				smi.master.RecomputeEmissions();
			}).Transition(online.active.postVent, (StatesInstance smi) => !smi.master.HasMaterial())
				.Transition(online.inactive.identify, HasProblem)
				.ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoVentsVenting, (StatesInstance smi) => smi.master)
				.Update(delegate(StatesInstance smi, float dt)
				{
					if (dt > 0f)
					{
						smi.master.RecomputeEmissions();
					}
				}, UpdateRate.SIM_4000ms)
				.defaultState = online.active.loopVent.start;
			online.active.loopVent.start.PlayAnim("working1").OnAnimQueueComplete(online.active.loopVent.finish);
			online.active.loopVent.finish.Enter(delegate(StatesInstance smi)
			{
				smi.master.EmitSolidChunk();
			}).PlayAnim("working2").OnAnimQueueComplete(online.active.loopVent.start);
			online.active.postVent.QueueAnim("working_pst").OnAnimQueueComplete(online.ready);
			online.ready.PlayAnim("on", KAnim.PlayMode.Once).Transition(online.active, (StatesInstance smi) => smi.master.HasMaterial()).Transition(online.inactive, HasProblem)
				.Transition(online.disconnected, (StatesInstance smi) => !smi.master.IsVentConnected())
				.ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoVentsReady, (StatesInstance smi) => smi.master);
			online.disconnected.PlayAnim("on", KAnim.PlayMode.Once).Transition(online.active, (StatesInstance smi) => smi.master.HasMaterial()).Transition(online.inactive, HasProblem)
				.Transition(online.ready, (StatesInstance smi) => smi.master.IsVentConnected())
				.ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoVentsDisconnected, (StatesInstance smi) => smi.master);
			online.inactive.PlayAnim("over_pressure", KAnim.PlayMode.Once).Transition(online.identify, (StatesInstance smi) => !HasProblem(smi)).defaultState = online.inactive.identify;
			online.inactive.identify.EnterTransition(online.inactive.entombed, (StatesInstance smi) => smi.master.IsEntombed()).EnterTransition(online.inactive.overpressure, (StatesInstance smi) => smi.master.IsOverPressure());
			online.inactive.entombed.ToggleMainStatusItem(Db.Get().BuildingStatusItems.Entombed).Transition(online.inactive.identify, (StatesInstance smi) => !smi.master.IsEntombed());
			online.inactive.overpressure.ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoVentsOverpressure).EnterTransition(online.inactive.identify, (StatesInstance smi) => !smi.master.IsOverPressure());
		}
	}

	public class StatesInstance : GameStateMachine<States, StatesInstance, GeothermalVent, object>.GameInstance
	{
		public StatesInstance(GeothermalVent smi)
			: base(smi)
		{
		}
	}

	[MyCmpGet]
	private Operational operational;

	[MyCmpAdd]
	private ConnectionManager connectedToggler;

	[MyCmpAdd]
	private EntombVulnerable entombVulnerable;

	[MyCmpReq]
	private LogicPorts logicPorts;

	[Serialize]
	private float recentMass = 1f;

	private MeterController massMeter;

	[Serialize]
	private QuestProgress progress;

	protected EmitterInfo emitterInfo;

	[Serialize]
	protected List<ElementInfo> availableMaterial = new List<ElementInfo>();

	protected bool overpressure;

	protected int debrisEmissionCell;

	private HandleVector<Game.CallbackInfo>.Handle onBlockedHandle = HandleVector<Game.CallbackInfo>.InvalidHandle;

	private HandleVector<Game.CallbackInfo>.Handle onUnblockedHandle = HandleVector<Game.CallbackInfo>.InvalidHandle;

	public bool IsQuestEntombed()
	{
		return progress == QuestProgress.Entombed;
	}

	public void SetQuestComplete()
	{
		progress = QuestProgress.Complete;
		connectedToggler.showButton = true;
		GetComponent<InfoDescription>().description = string.Concat(BUILDINGS.PREFABS.GEOTHERMALVENT.EFFECT, "\n\n", BUILDINGS.PREFABS.GEOTHERMALVENT.DESC);
		Trigger(-1514841199);
	}

	public static string GenerateName()
	{
		string text = "";
		for (int i = 0; i < 2; i++)
		{
			text += "0123456789"[UnityEngine.Random.Range(0, "0123456789".Length)];
		}
		return BUILDINGS.PREFABS.GEOTHERMALVENT.NAME_FMT.Replace("{ID}", text);
	}

	protected override void OnSpawn()
	{
		base.OnSpawn();
		entombVulnerable.SetStatusItem(Db.Get().BuildingStatusItems.Entombed);
		GetComponent<PrimaryElement>().SetElement(SimHashes.Katairite);
		emitterInfo = default(EmitterInfo);
		emitterInfo.cell = Grid.PosToCell(base.gameObject) + Grid.WidthInCells * 3;
		emitterInfo.element = default(ElementInfo);
		emitterInfo.simHandle = -1;
		Components.GeothermalVents.Add(base.gameObject.GetMyWorldId(), this);
		if (progress == QuestProgress.Uninitialized)
		{
			if (Components.GeothermalVents.GetItems(base.gameObject.GetMyWorldId()).Count == 3)
			{
				progress = QuestProgress.Entombed;
			}
			else
			{
				progress = QuestProgress.Complete;
			}
		}
		if (progress == QuestProgress.Complete)
		{
			connectedToggler.showButton = true;
		}
		else
		{
			GetComponent<InfoDescription>().description = string.Concat(BUILDINGS.PREFABS.GEOTHERMALVENT.EFFECT, "\n\n", BUILDINGS.PREFABS.GEOTHERMALVENT.BLOCKED_DESC);
			Trigger(-1514841199);
		}
		massMeter = new MeterController(GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.NoChange, Grid.SceneLayer.NoLayer, GeothermalVentConfig.BAROMETER_SYMBOLS);
		UserNameable component = GetComponent<UserNameable>();
		if (component.savedName == "" || component.savedName == BUILDINGS.PREFABS.GEOTHERMALVENT.NAME)
		{
			component.SetName(GenerateName());
		}
		SimRegister();
		base.smi.StartSM();
	}

	protected void SimRegister()
	{
		onBlockedHandle = Game.Instance.callbackManager.Add(new Game.CallbackInfo(OnSimBlockedCallback, manually_release: true));
		onUnblockedHandle = Game.Instance.callbackManager.Add(new Game.CallbackInfo(OnSimUnblockedCallback, manually_release: true));
		SimMessages.AddElementEmitter(float.MaxValue, Game.Instance.simComponentCallbackManager.Add(OnSimRegisteredCallback, this, "GeothermalVentElementEmitter").index, onBlockedHandle.index, onUnblockedHandle.index);
	}

	protected void OnSimBlockedCallback()
	{
		overpressure = true;
	}

	protected void OnSimUnblockedCallback()
	{
		overpressure = false;
	}

	protected static void OnSimRegisteredCallback(int handle, object data)
	{
		((GeothermalVent)data).OnSimRegisteredImpl(handle);
	}

	protected void OnSimRegisteredImpl(int handle)
	{
		Debug.Assert(emitterInfo.simHandle == -1, "?! too many handles registered");
		emitterInfo.simHandle = handle;
	}

	protected void SimUnregister()
	{
		if (Sim.IsValidHandle(emitterInfo.simHandle))
		{
			SimMessages.RemoveElementEmitter(-1, emitterInfo.simHandle);
		}
		emitterInfo.simHandle = -1;
	}

	protected override void OnCleanUp()
	{
		Game.Instance.ManualReleaseHandle(onBlockedHandle);
		Game.Instance.ManualReleaseHandle(onUnblockedHandle);
		Components.GeothermalVents.Remove(base.gameObject.GetMyWorldId(), this);
		base.OnCleanUp();
	}

	protected void OnMassEmitted(ushort element, float mass)
	{
		bool flag = false;
		for (int i = 0; i < availableMaterial.Count; i++)
		{
			if (availableMaterial[i].elementIdx == element)
			{
				ElementInfo value = availableMaterial[i];
				value.mass -= mass;
				flag |= value.mass <= 0f;
				availableMaterial[i] = value;
				break;
			}
		}
		if (flag)
		{
			RecomputeEmissions();
		}
	}

	public void SpawnKeepsake()
	{
		GameObject keepsakePrefab = Assets.GetPrefab("keepsake_geothermalplant");
		if (keepsakePrefab != null)
		{
			GetComponent<KBatchedAnimController>().Play("pooped");
			GameScheduler.Instance.Schedule("UncorkPoopAnim", 1.5f, delegate
			{
				GetComponent<KBatchedAnimController>().Play("uncork");
			});
			GameScheduler.Instance.Schedule("UncorkPoopFX", 2f, delegate
			{
				Game.Instance.SpawnFX(SpawnFXHashes.MissileExplosion, base.transform.GetPosition() + Vector3.up * 3f, 0f);
			});
			GameScheduler.Instance.Schedule("SpawnGeothermalKeepsake", 3.75f, delegate
			{
				Vector3 position = base.transform.GetPosition();
				position.z = Grid.GetLayerZ(Grid.SceneLayer.BuildingFront);
				GameObject obj = Util.KInstantiate(keepsakePrefab, position);
				obj.SetActive(value: true);
				new UpgradeFX.Instance(obj.GetComponent<KMonoBehaviour>(), new Vector3(0f, -0.5f, -0.1f)).StartSM();
			});
		}
	}

	public bool IsOverPressure()
	{
		return overpressure;
	}

	protected void RecomputeEmissions()
	{
		availableMaterial.Sort();
		while (availableMaterial.Count > 0 && availableMaterial[availableMaterial.Count - 1].mass <= 0f)
		{
			availableMaterial.RemoveAt(availableMaterial.Count - 1);
		}
		int num = 0;
		foreach (ElementInfo item in availableMaterial)
		{
			if (!item.isSolid)
			{
				num++;
			}
		}
		if (num > 0)
		{
			int num2 = UnityEngine.Random.Range(0, availableMaterial.Count);
			while (availableMaterial[num2].isSolid)
			{
				num2 = (num2 + 1) % availableMaterial.Count;
			}
			emitterInfo.element = availableMaterial[num2];
			emitterInfo.element.diseaseCount = (int)((float)availableMaterial[num2].diseaseCount * emitterInfo.element.mass / availableMaterial[num2].mass);
		}
		else
		{
			emitterInfo.element.elementIdx = 0;
			emitterInfo.element.mass = 0f;
		}
		emitterInfo.dirty = true;
	}

	public void addMaterial(ElementInfo info)
	{
		availableMaterial.Add(info);
		recentMass = MaterialAvailable();
	}

	public bool HasMaterial()
	{
		bool flag = availableMaterial.Count != 0;
		if (flag != logicPorts.GetOutputValue("GEOTHERMAL_VENT_STATUS_PORT") > 0)
		{
			logicPorts.SendSignal("GEOTHERMAL_VENT_STATUS_PORT", flag ? 1 : 0);
		}
		return flag;
	}

	public float MaterialAvailable()
	{
		float num = 0f;
		foreach (ElementInfo item in availableMaterial)
		{
			num += item.mass;
		}
		return num;
	}

	public bool IsEntombed()
	{
		return entombVulnerable.GetEntombed;
	}

	public bool CanVent()
	{
		if (!HasMaterial())
		{
			return !IsEntombed();
		}
		return false;
	}

	public bool IsVentConnected()
	{
		if (connectedToggler == null)
		{
			return false;
		}
		return connectedToggler.IsConnected;
	}

	public void EmitSolidChunk()
	{
		int num = 0;
		foreach (ElementInfo item in availableMaterial)
		{
			if (item.isSolid && item.mass > 0f)
			{
				num++;
			}
		}
		if (num == 0)
		{
			return;
		}
		int num2 = UnityEngine.Random.Range(0, availableMaterial.Count);
		while (!availableMaterial[num2].isSolid)
		{
			num2 = (num2 + 1) % availableMaterial.Count;
		}
		ElementInfo value = availableMaterial[num2];
		if (ElementLoader.elements[availableMaterial[num2].elementIdx] != null)
		{
			bool num3 = UnityEngine.Random.value >= 0.5f;
			float f = GeothermalVentConfig.INITIAL_DEBRIS_ANGLE.Get() * (float)Math.PI / 180f;
			Vector2 vector = new Vector2(0f - Mathf.Cos(f), Mathf.Sin(f));
			if (num3)
			{
				vector.x = 0f - vector.x;
			}
			vector = vector.normalized;
			_ = (Vector3)(vector * GeothermalVentConfig.INITIAL_DEBRIS_VELOCIOTY.Get());
			float num4 = Math.Min(GeothermalVentConfig.DEBRIS_MASS_KG.Get(), value.mass);
			if (value.mass - num4 < GeothermalVentConfig.DEBRIS_MASS_KG.min)
			{
				num4 = value.mass;
			}
			if (!(num4 < 0.125f))
			{
				int num5 = (int)((float)value.diseaseCount * num4 / value.mass);
				Vector3 vector2 = Grid.CellToPos(emitterInfo.cell, CellAlignment.Top, Grid.SceneLayer.BuildingFront);
				Game.Instance.SpawnFX(SpawnFXHashes.MeteorImpactDust, vector2, 0f);
				GameObject obj = Util.KInstantiate(Assets.GetPrefab("MiniComet"), vector2);
				PrimaryElement component = obj.GetComponent<PrimaryElement>();
				component.SetElement(ElementLoader.elements[value.elementIdx].id);
				component.Mass = num4;
				component.Temperature = value.temperature;
				MiniComet component2 = obj.GetComponent<MiniComet>();
				component2.diseaseIdx = value.diseaseIdx;
				component2.addDiseaseCount = num5;
				obj.SetActive(value: true);
				value.diseaseCount -= num5;
				value.mass -= num4;
				availableMaterial[num2] = value;
			}
		}
	}

	public void Sim200ms(float dt)
	{
		if (dt > 0f)
		{
			unsafeSim200ms(dt);
		}
	}

	private unsafe void unsafeSim200ms(float dt)
	{
		if (Sim.IsValidHandle(emitterInfo.simHandle))
		{
			if (emitterInfo.dirty)
			{
				SimMessages.ModifyElementEmitter(emitterInfo.simHandle, emitterInfo.cell, 1, ElementLoader.elements[emitterInfo.element.elementIdx].id, 0.2f, Math.Min(3f, emitterInfo.element.mass), emitterInfo.element.temperature, 120f, emitterInfo.element.diseaseIdx, emitterInfo.element.diseaseCount);
				emitterInfo.dirty = false;
			}
			int handleIndex = Sim.GetHandleIndex(emitterInfo.simHandle);
			Sim.EmittedMassInfo emittedMassInfo = Game.Instance.simData.emittedMassEntries[handleIndex];
			if (emittedMassInfo.mass > 0f)
			{
				OnMassEmitted(emittedMassInfo.elemIdx, emittedMassInfo.mass);
			}
		}
		massMeter.SetPositionPercent(MaterialAvailable() / recentMass);
	}

	protected static bool HasProblem(StatesInstance smi)
	{
		if (!smi.master.IsEntombed())
		{
			return smi.master.IsOverPressure();
		}
		return true;
	}
}
