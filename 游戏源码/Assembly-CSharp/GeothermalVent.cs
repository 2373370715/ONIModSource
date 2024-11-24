using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000DBA RID: 3514
public class GeothermalVent : StateMachineComponent<GeothermalVent.StatesInstance>, ISim200ms
{
	// Token: 0x06004517 RID: 17687 RVA: 0x000CCA95 File Offset: 0x000CAC95
	public bool IsQuestEntombed()
	{
		return this.progress == GeothermalVent.QuestProgress.Entombed;
	}

	// Token: 0x06004518 RID: 17688 RVA: 0x0024A5B4 File Offset: 0x002487B4
	public void SetQuestComplete()
	{
		this.progress = GeothermalVent.QuestProgress.Complete;
		this.connectedToggler.showButton = true;
		base.GetComponent<InfoDescription>().description = BUILDINGS.PREFABS.GEOTHERMALVENT.EFFECT + "\n\n" + BUILDINGS.PREFABS.GEOTHERMALVENT.DESC;
		base.Trigger(-1514841199, null);
	}

	// Token: 0x06004519 RID: 17689 RVA: 0x0024A60C File Offset: 0x0024880C
	public static string GenerateName()
	{
		string text = "";
		for (int i = 0; i < 2; i++)
		{
			text += "0123456789"[UnityEngine.Random.Range(0, "0123456789".Length)].ToString();
		}
		return BUILDINGS.PREFABS.GEOTHERMALVENT.NAME_FMT.Replace("{ID}", text);
	}

	// Token: 0x0600451A RID: 17690 RVA: 0x0024A664 File Offset: 0x00248864
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.entombVulnerable.SetStatusItem(Db.Get().BuildingStatusItems.Entombed);
		base.GetComponent<PrimaryElement>().SetElement(SimHashes.Katairite, true);
		this.emitterInfo = default(GeothermalVent.EmitterInfo);
		this.emitterInfo.cell = Grid.PosToCell(base.gameObject) + Grid.WidthInCells * 3;
		this.emitterInfo.element = default(GeothermalVent.ElementInfo);
		this.emitterInfo.simHandle = -1;
		Components.GeothermalVents.Add(base.gameObject.GetMyWorldId(), this);
		if (this.progress == GeothermalVent.QuestProgress.Uninitialized)
		{
			if (Components.GeothermalVents.GetItems(base.gameObject.GetMyWorldId()).Count == 3)
			{
				this.progress = GeothermalVent.QuestProgress.Entombed;
			}
			else
			{
				this.progress = GeothermalVent.QuestProgress.Complete;
			}
		}
		if (this.progress == GeothermalVent.QuestProgress.Complete)
		{
			this.connectedToggler.showButton = true;
		}
		else
		{
			base.GetComponent<InfoDescription>().description = BUILDINGS.PREFABS.GEOTHERMALVENT.EFFECT + "\n\n" + BUILDINGS.PREFABS.GEOTHERMALVENT.BLOCKED_DESC;
			base.Trigger(-1514841199, null);
		}
		this.massMeter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.NoChange, Grid.SceneLayer.NoLayer, GeothermalVentConfig.BAROMETER_SYMBOLS);
		UserNameable component = base.GetComponent<UserNameable>();
		if (component.savedName == "" || component.savedName == BUILDINGS.PREFABS.GEOTHERMALVENT.NAME)
		{
			component.SetName(GeothermalVent.GenerateName());
		}
		this.SimRegister();
		base.smi.StartSM();
	}

	// Token: 0x0600451B RID: 17691 RVA: 0x0024A7F0 File Offset: 0x002489F0
	protected void SimRegister()
	{
		this.onBlockedHandle = Game.Instance.callbackManager.Add(new Game.CallbackInfo(new System.Action(this.OnSimBlockedCallback), true));
		this.onUnblockedHandle = Game.Instance.callbackManager.Add(new Game.CallbackInfo(new System.Action(this.OnSimUnblockedCallback), true));
		SimMessages.AddElementEmitter(float.MaxValue, Game.Instance.simComponentCallbackManager.Add(new Action<int, object>(GeothermalVent.OnSimRegisteredCallback), this, "GeothermalVentElementEmitter").index, this.onBlockedHandle.index, this.onUnblockedHandle.index);
	}

	// Token: 0x0600451C RID: 17692 RVA: 0x000CCAA0 File Offset: 0x000CACA0
	protected void OnSimBlockedCallback()
	{
		this.overpressure = true;
	}

	// Token: 0x0600451D RID: 17693 RVA: 0x000CCAA9 File Offset: 0x000CACA9
	protected void OnSimUnblockedCallback()
	{
		this.overpressure = false;
	}

	// Token: 0x0600451E RID: 17694 RVA: 0x000CCAB2 File Offset: 0x000CACB2
	protected static void OnSimRegisteredCallback(int handle, object data)
	{
		((GeothermalVent)data).OnSimRegisteredImpl(handle);
	}

	// Token: 0x0600451F RID: 17695 RVA: 0x000CCAC0 File Offset: 0x000CACC0
	protected void OnSimRegisteredImpl(int handle)
	{
		global::Debug.Assert(this.emitterInfo.simHandle == -1, "?! too many handles registered");
		this.emitterInfo.simHandle = handle;
	}

	// Token: 0x06004520 RID: 17696 RVA: 0x000CCAE6 File Offset: 0x000CACE6
	protected void SimUnregister()
	{
		if (Sim.IsValidHandle(this.emitterInfo.simHandle))
		{
			SimMessages.RemoveElementEmitter(-1, this.emitterInfo.simHandle);
		}
		this.emitterInfo.simHandle = -1;
	}

	// Token: 0x06004521 RID: 17697 RVA: 0x000CCB17 File Offset: 0x000CAD17
	protected override void OnCleanUp()
	{
		Game.Instance.ManualReleaseHandle(this.onBlockedHandle);
		Game.Instance.ManualReleaseHandle(this.onUnblockedHandle);
		Components.GeothermalVents.Remove(base.gameObject.GetMyWorldId(), this);
		base.OnCleanUp();
	}

	// Token: 0x06004522 RID: 17698 RVA: 0x0024A894 File Offset: 0x00248A94
	protected void OnMassEmitted(ushort element, float mass)
	{
		bool flag = false;
		for (int i = 0; i < this.availableMaterial.Count; i++)
		{
			if (this.availableMaterial[i].elementIdx == element)
			{
				GeothermalVent.ElementInfo elementInfo = this.availableMaterial[i];
				elementInfo.mass -= mass;
				flag |= (elementInfo.mass <= 0f);
				this.availableMaterial[i] = elementInfo;
				break;
			}
		}
		if (flag)
		{
			this.RecomputeEmissions();
		}
	}

	// Token: 0x06004523 RID: 17699 RVA: 0x0024A914 File Offset: 0x00248B14
	public void SpawnKeepsake()
	{
		GameObject keepsakePrefab = Assets.GetPrefab("keepsake_geothermalplant");
		if (keepsakePrefab != null)
		{
			base.GetComponent<KBatchedAnimController>().Play("pooped", KAnim.PlayMode.Once, 1f, 0f);
			GameScheduler.Instance.Schedule("UncorkPoopAnim", 1.5f, delegate(object data)
			{
				this.GetComponent<KBatchedAnimController>().Play("uncork", KAnim.PlayMode.Once, 1f, 0f);
			}, null, null);
			GameScheduler.Instance.Schedule("UncorkPoopFX", 2f, delegate(object data)
			{
				Game.Instance.SpawnFX(SpawnFXHashes.MissileExplosion, this.transform.GetPosition() + Vector3.up * 3f, 0f);
			}, null, null);
			GameScheduler.Instance.Schedule("SpawnGeothermalKeepsake", 3.75f, delegate(object data)
			{
				Vector3 position = this.transform.GetPosition();
				position.z = Grid.GetLayerZ(Grid.SceneLayer.BuildingFront);
				GameObject gameObject = Util.KInstantiate(keepsakePrefab, position);
				gameObject.SetActive(true);
				new UpgradeFX.Instance(gameObject.GetComponent<KMonoBehaviour>(), new Vector3(0f, -0.5f, -0.1f)).StartSM();
			}, null, null);
		}
	}

	// Token: 0x06004524 RID: 17700 RVA: 0x000CCB55 File Offset: 0x000CAD55
	public bool IsOverPressure()
	{
		return this.overpressure;
	}

	// Token: 0x06004525 RID: 17701 RVA: 0x0024A9E0 File Offset: 0x00248BE0
	protected void RecomputeEmissions()
	{
		this.availableMaterial.Sort();
		while (this.availableMaterial.Count > 0 && this.availableMaterial[this.availableMaterial.Count - 1].mass <= 0f)
		{
			this.availableMaterial.RemoveAt(this.availableMaterial.Count - 1);
		}
		int num = 0;
		using (List<GeothermalVent.ElementInfo>.Enumerator enumerator = this.availableMaterial.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (!enumerator.Current.isSolid)
				{
					num++;
				}
			}
		}
		if (num > 0)
		{
			int num2 = UnityEngine.Random.Range(0, this.availableMaterial.Count);
			while (this.availableMaterial[num2].isSolid)
			{
				num2 = (num2 + 1) % this.availableMaterial.Count;
			}
			this.emitterInfo.element = this.availableMaterial[num2];
			this.emitterInfo.element.diseaseCount = (int)((float)this.availableMaterial[num2].diseaseCount * this.emitterInfo.element.mass / this.availableMaterial[num2].mass);
		}
		else
		{
			this.emitterInfo.element.elementIdx = 0;
			this.emitterInfo.element.mass = 0f;
		}
		this.emitterInfo.dirty = true;
	}

	// Token: 0x06004526 RID: 17702 RVA: 0x000CCB5D File Offset: 0x000CAD5D
	public void addMaterial(GeothermalVent.ElementInfo info)
	{
		this.availableMaterial.Add(info);
		this.recentMass = this.MaterialAvailable();
	}

	// Token: 0x06004527 RID: 17703 RVA: 0x0024AB60 File Offset: 0x00248D60
	public bool HasMaterial()
	{
		bool flag = this.availableMaterial.Count != 0;
		if (flag != this.logicPorts.GetOutputValue("GEOTHERMAL_VENT_STATUS_PORT") > 0)
		{
			this.logicPorts.SendSignal("GEOTHERMAL_VENT_STATUS_PORT", flag ? 1 : 0);
		}
		return flag;
	}

	// Token: 0x06004528 RID: 17704 RVA: 0x0024ABB4 File Offset: 0x00248DB4
	public float MaterialAvailable()
	{
		float num = 0f;
		foreach (GeothermalVent.ElementInfo elementInfo in this.availableMaterial)
		{
			num += elementInfo.mass;
		}
		return num;
	}

	// Token: 0x06004529 RID: 17705 RVA: 0x000CCB77 File Offset: 0x000CAD77
	public bool IsEntombed()
	{
		return this.entombVulnerable.GetEntombed;
	}

	// Token: 0x0600452A RID: 17706 RVA: 0x000CCB84 File Offset: 0x000CAD84
	public bool CanVent()
	{
		return !this.HasMaterial() && !this.IsEntombed();
	}

	// Token: 0x0600452B RID: 17707 RVA: 0x000CCB99 File Offset: 0x000CAD99
	public bool IsVentConnected()
	{
		return !(this.connectedToggler == null) && this.connectedToggler.IsConnected;
	}

	// Token: 0x0600452C RID: 17708 RVA: 0x0024AC10 File Offset: 0x00248E10
	public void EmitSolidChunk()
	{
		int num = 0;
		foreach (GeothermalVent.ElementInfo elementInfo in this.availableMaterial)
		{
			if (elementInfo.isSolid && elementInfo.mass > 0f)
			{
				num++;
			}
		}
		if (num == 0)
		{
			return;
		}
		int num2 = UnityEngine.Random.Range(0, this.availableMaterial.Count);
		while (!this.availableMaterial[num2].isSolid)
		{
			num2 = (num2 + 1) % this.availableMaterial.Count;
		}
		GeothermalVent.ElementInfo elementInfo2 = this.availableMaterial[num2];
		if (ElementLoader.elements[(int)this.availableMaterial[num2].elementIdx] == null)
		{
			return;
		}
		bool flag = UnityEngine.Random.value >= 0.5f;
		float f = GeothermalVentConfig.INITIAL_DEBRIS_ANGLE.Get() * 3.1415927f / 180f;
		Vector2 normalized = new Vector2(-Mathf.Cos(f), Mathf.Sin(f));
		if (flag)
		{
			normalized.x = -normalized.x;
		}
		normalized = normalized.normalized;
		normalized * GeothermalVentConfig.INITIAL_DEBRIS_VELOCIOTY.Get();
		float num3 = Math.Min(GeothermalVentConfig.DEBRIS_MASS_KG.Get(), elementInfo2.mass);
		if (elementInfo2.mass - num3 < GeothermalVentConfig.DEBRIS_MASS_KG.min)
		{
			num3 = elementInfo2.mass;
		}
		if (num3 < 0.01f)
		{
			elementInfo2.mass = 0f;
			this.availableMaterial[num2] = elementInfo2;
			return;
		}
		int num4 = (int)((float)elementInfo2.diseaseCount * num3 / elementInfo2.mass);
		Vector3 vector = Grid.CellToPos(this.emitterInfo.cell, CellAlignment.Top, Grid.SceneLayer.BuildingFront);
		Game.Instance.SpawnFX(SpawnFXHashes.MeteorImpactDust, vector, 0f);
		GameObject gameObject = Util.KInstantiate(Assets.GetPrefab("MiniComet"), vector);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(ElementLoader.elements[(int)elementInfo2.elementIdx].id, true);
		component.Mass = num3;
		component.Temperature = elementInfo2.temperature;
		MiniComet component2 = gameObject.GetComponent<MiniComet>();
		component2.diseaseIdx = elementInfo2.diseaseIdx;
		component2.addDiseaseCount = num4;
		gameObject.SetActive(true);
		elementInfo2.diseaseCount -= num4;
		elementInfo2.mass -= num3;
		this.availableMaterial[num2] = elementInfo2;
	}

	// Token: 0x0600452D RID: 17709 RVA: 0x000CCBB6 File Offset: 0x000CADB6
	public void Sim200ms(float dt)
	{
		if (dt > 0f)
		{
			this.unsafeSim200ms(dt);
		}
	}

	// Token: 0x0600452E RID: 17710 RVA: 0x0024AE78 File Offset: 0x00249078
	private unsafe void unsafeSim200ms(float dt)
	{
		if (Sim.IsValidHandle(this.emitterInfo.simHandle))
		{
			if (this.emitterInfo.dirty)
			{
				SimMessages.ModifyElementEmitter(this.emitterInfo.simHandle, this.emitterInfo.cell, 1, ElementLoader.elements[(int)this.emitterInfo.element.elementIdx].id, 0.2f, Math.Min(3f, this.emitterInfo.element.mass), this.emitterInfo.element.temperature, 120f, this.emitterInfo.element.diseaseIdx, this.emitterInfo.element.diseaseCount);
				this.emitterInfo.dirty = false;
			}
			int handleIndex = Sim.GetHandleIndex(this.emitterInfo.simHandle);
			Sim.EmittedMassInfo emittedMassInfo = Game.Instance.simData.emittedMassEntries[handleIndex];
			if (emittedMassInfo.mass > 0f)
			{
				this.OnMassEmitted(emittedMassInfo.elemIdx, emittedMassInfo.mass);
			}
		}
		this.massMeter.SetPositionPercent(this.MaterialAvailable() / this.recentMass);
	}

	// Token: 0x0600452F RID: 17711 RVA: 0x000CCBC7 File Offset: 0x000CADC7
	protected static bool HasProblem(GeothermalVent.StatesInstance smi)
	{
		return smi.master.IsEntombed() || smi.master.IsOverPressure();
	}

	// Token: 0x04002F91 RID: 12177
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04002F92 RID: 12178
	[MyCmpAdd]
	private ConnectionManager connectedToggler;

	// Token: 0x04002F93 RID: 12179
	[MyCmpAdd]
	private EntombVulnerable entombVulnerable;

	// Token: 0x04002F94 RID: 12180
	[MyCmpReq]
	private LogicPorts logicPorts;

	// Token: 0x04002F95 RID: 12181
	[Serialize]
	private float recentMass = 1f;

	// Token: 0x04002F96 RID: 12182
	private MeterController massMeter;

	// Token: 0x04002F97 RID: 12183
	[Serialize]
	private GeothermalVent.QuestProgress progress;

	// Token: 0x04002F98 RID: 12184
	protected GeothermalVent.EmitterInfo emitterInfo;

	// Token: 0x04002F99 RID: 12185
	[Serialize]
	protected List<GeothermalVent.ElementInfo> availableMaterial = new List<GeothermalVent.ElementInfo>();

	// Token: 0x04002F9A RID: 12186
	protected bool overpressure;

	// Token: 0x04002F9B RID: 12187
	protected int debrisEmissionCell;

	// Token: 0x04002F9C RID: 12188
	private HandleVector<Game.CallbackInfo>.Handle onBlockedHandle = HandleVector<Game.CallbackInfo>.InvalidHandle;

	// Token: 0x04002F9D RID: 12189
	private HandleVector<Game.CallbackInfo>.Handle onUnblockedHandle = HandleVector<Game.CallbackInfo>.InvalidHandle;

	// Token: 0x02000DBB RID: 3515
	private enum QuestProgress
	{
		// Token: 0x04002F9F RID: 12191
		Uninitialized,
		// Token: 0x04002FA0 RID: 12192
		Entombed,
		// Token: 0x04002FA1 RID: 12193
		Complete
	}

	// Token: 0x02000DBC RID: 3516
	public struct ElementInfo : IComparable
	{
		// Token: 0x06004531 RID: 17713 RVA: 0x000CCC17 File Offset: 0x000CAE17
		public int CompareTo(object obj)
		{
			return -this.mass.CompareTo(((GeothermalVent.ElementInfo)obj).mass);
		}

		// Token: 0x04002FA2 RID: 12194
		public bool isSolid;

		// Token: 0x04002FA3 RID: 12195
		public ushort elementIdx;

		// Token: 0x04002FA4 RID: 12196
		public float mass;

		// Token: 0x04002FA5 RID: 12197
		public float temperature;

		// Token: 0x04002FA6 RID: 12198
		public byte diseaseIdx;

		// Token: 0x04002FA7 RID: 12199
		public int diseaseCount;
	}

	// Token: 0x02000DBD RID: 3517
	public struct EmitterInfo
	{
		// Token: 0x04002FA8 RID: 12200
		public int simHandle;

		// Token: 0x04002FA9 RID: 12201
		public int cell;

		// Token: 0x04002FAA RID: 12202
		public GeothermalVent.ElementInfo element;

		// Token: 0x04002FAB RID: 12203
		public bool dirty;
	}

	// Token: 0x02000DBE RID: 3518
	public class States : GameStateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent>
	{
		// Token: 0x06004532 RID: 17714 RVA: 0x0024AFAC File Offset: 0x002491AC
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.root;
			this.root.EnterTransition(this.questEntombed, (GeothermalVent.StatesInstance smi) => smi.master.IsQuestEntombed()).EnterTransition(this.online, (GeothermalVent.StatesInstance smi) => !smi.master.IsQuestEntombed());
			this.questEntombed.PlayAnim("pooped").ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoVentQuestBlockage, (GeothermalVent.StatesInstance smi) => smi.master).Transition(this.online, (GeothermalVent.StatesInstance smi) => smi.master.progress == GeothermalVent.QuestProgress.Complete, UpdateRate.SIM_200ms);
			this.online.PlayAnim("on", KAnim.PlayMode.Once).defaultState = this.online.identify;
			this.online.identify.EnterTransition(this.online.inactive, new StateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.Transition.ConditionCallback(GeothermalVent.HasProblem)).EnterTransition(this.online.active, (GeothermalVent.StatesInstance smi) => !GeothermalVent.HasProblem(smi) && smi.master.HasMaterial()).EnterTransition(this.online.ready, (GeothermalVent.StatesInstance smi) => !GeothermalVent.HasProblem(smi) && !smi.master.HasMaterial() && smi.master.IsVentConnected()).EnterTransition(this.online.disconnected, (GeothermalVent.StatesInstance smi) => !GeothermalVent.HasProblem(smi) && !smi.master.HasMaterial() && !smi.master.IsVentConnected());
			this.online.active.defaultState = this.online.active.preVent;
			this.online.active.preVent.PlayAnim("working_pre").OnAnimQueueComplete(this.online.active.loopVent);
			this.online.active.loopVent.Enter(delegate(GeothermalVent.StatesInstance smi)
			{
				smi.master.RecomputeEmissions();
			}).Exit(delegate(GeothermalVent.StatesInstance smi)
			{
				smi.master.RecomputeEmissions();
			}).Transition(this.online.active.postVent, (GeothermalVent.StatesInstance smi) => !smi.master.HasMaterial(), UpdateRate.SIM_200ms).Transition(this.online.inactive.identify, new StateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.Transition.ConditionCallback(GeothermalVent.HasProblem), UpdateRate.SIM_200ms).ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoVentsVenting, (GeothermalVent.StatesInstance smi) => smi.master).Update(delegate(GeothermalVent.StatesInstance smi, float dt)
			{
				if (dt > 0f)
				{
					smi.master.RecomputeEmissions();
				}
			}, UpdateRate.SIM_4000ms, false).defaultState = this.online.active.loopVent.start;
			this.online.active.loopVent.start.PlayAnim("working1").OnAnimQueueComplete(this.online.active.loopVent.finish);
			this.online.active.loopVent.finish.Enter(delegate(GeothermalVent.StatesInstance smi)
			{
				smi.master.EmitSolidChunk();
			}).PlayAnim("working2").OnAnimQueueComplete(this.online.active.loopVent.start);
			this.online.active.postVent.QueueAnim("working_pst", false, null).OnAnimQueueComplete(this.online.ready);
			this.online.ready.PlayAnim("on", KAnim.PlayMode.Once).Transition(this.online.active, (GeothermalVent.StatesInstance smi) => smi.master.HasMaterial(), UpdateRate.SIM_200ms).Transition(this.online.inactive, new StateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.Transition.ConditionCallback(GeothermalVent.HasProblem), UpdateRate.SIM_200ms).Transition(this.online.disconnected, (GeothermalVent.StatesInstance smi) => !smi.master.IsVentConnected(), UpdateRate.SIM_200ms).ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoVentsReady, (GeothermalVent.StatesInstance smi) => smi.master);
			this.online.disconnected.PlayAnim("on", KAnim.PlayMode.Once).Transition(this.online.active, (GeothermalVent.StatesInstance smi) => smi.master.HasMaterial(), UpdateRate.SIM_200ms).Transition(this.online.inactive, new StateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.Transition.ConditionCallback(GeothermalVent.HasProblem), UpdateRate.SIM_200ms).Transition(this.online.ready, (GeothermalVent.StatesInstance smi) => smi.master.IsVentConnected(), UpdateRate.SIM_200ms).ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoVentsDisconnected, (GeothermalVent.StatesInstance smi) => smi.master);
			this.online.inactive.PlayAnim("over_pressure", KAnim.PlayMode.Once).Transition(this.online.identify, (GeothermalVent.StatesInstance smi) => !GeothermalVent.HasProblem(smi), UpdateRate.SIM_200ms).defaultState = this.online.inactive.identify;
			this.online.inactive.identify.EnterTransition(this.online.inactive.entombed, (GeothermalVent.StatesInstance smi) => smi.master.IsEntombed()).EnterTransition(this.online.inactive.overpressure, (GeothermalVent.StatesInstance smi) => smi.master.IsOverPressure());
			this.online.inactive.entombed.ToggleMainStatusItem(Db.Get().BuildingStatusItems.Entombed, null).Transition(this.online.inactive.identify, (GeothermalVent.StatesInstance smi) => !smi.master.IsEntombed(), UpdateRate.SIM_200ms);
			this.online.inactive.overpressure.ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoVentsOverpressure, null).EnterTransition(this.online.inactive.identify, (GeothermalVent.StatesInstance smi) => !smi.master.IsOverPressure());
		}

		// Token: 0x04002FAC RID: 12204
		public GameStateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.State questEntombed;

		// Token: 0x04002FAD RID: 12205
		public GeothermalVent.States.OnlineStates online;

		// Token: 0x02000DBF RID: 3519
		public class ActiveStates : GameStateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.State
		{
			// Token: 0x04002FAE RID: 12206
			public GeothermalVent.States.ActiveStates.LoopStates loopVent;

			// Token: 0x04002FAF RID: 12207
			public GameStateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.State preVent;

			// Token: 0x04002FB0 RID: 12208
			public GameStateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.State postVent;

			// Token: 0x02000DC0 RID: 3520
			public class LoopStates : GameStateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.State
			{
				// Token: 0x04002FB1 RID: 12209
				public GameStateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.State start;

				// Token: 0x04002FB2 RID: 12210
				public GameStateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.State finish;
			}
		}

		// Token: 0x02000DC1 RID: 3521
		public class ProblemStates : GameStateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.State
		{
			// Token: 0x04002FB3 RID: 12211
			public GameStateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.State identify;

			// Token: 0x04002FB4 RID: 12212
			public GameStateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.State entombed;

			// Token: 0x04002FB5 RID: 12213
			public GameStateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.State overpressure;
		}

		// Token: 0x02000DC2 RID: 3522
		public class OnlineStates : GameStateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.State
		{
			// Token: 0x04002FB6 RID: 12214
			public GameStateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.State identify;

			// Token: 0x04002FB7 RID: 12215
			public GameStateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.State ready;

			// Token: 0x04002FB8 RID: 12216
			public GameStateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.State disconnected;

			// Token: 0x04002FB9 RID: 12217
			public GeothermalVent.States.ActiveStates active;

			// Token: 0x04002FBA RID: 12218
			public GeothermalVent.States.ProblemStates inactive;
		}
	}

	// Token: 0x02000DC4 RID: 3524
	public class StatesInstance : GameStateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.GameInstance
	{
		// Token: 0x06004552 RID: 17746 RVA: 0x000CCD91 File Offset: 0x000CAF91
		public StatesInstance(GeothermalVent smi) : base(smi)
		{
		}
	}
}
