using System;
using Klei;
using UnityEngine;

// Token: 0x02000E2E RID: 3630
[AddComponentMenu("KMonoBehaviour/Workable/LiquidPumpingStation")]
public class LiquidPumpingStation : Workable, ISim200ms
{
	// Token: 0x06004777 RID: 18295 RVA: 0x000CE5BE File Offset: 0x000CC7BE
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.resetProgressOnStop = true;
		this.showProgressBar = false;
	}

	// Token: 0x06004778 RID: 18296 RVA: 0x002526F4 File Offset: 0x002508F4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.infos = new LiquidPumpingStation.LiquidInfo[LiquidPumpingStation.liquidOffsets.Length * 2];
		this.RefreshStatusItem();
		this.Sim200ms(0f);
		base.SetWorkTime(10f);
		this.RefreshDepthAvailable();
		this.RegisterListenersToCellChanges();
		this.meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Behind, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_target",
			"meter_arrow",
			"meter_scale"
		});
		foreach (GameObject gameObject in base.GetComponent<Storage>().items)
		{
			if (!(gameObject == null) && gameObject != null)
			{
				gameObject.DeleteObject();
			}
		}
	}

	// Token: 0x06004779 RID: 18297 RVA: 0x002527DC File Offset: 0x002509DC
	private void RegisterListenersToCellChanges()
	{
		int widthInCells = base.GetComponent<BuildingComplete>().Def.WidthInCells;
		CellOffset[] array = new CellOffset[widthInCells * 4];
		for (int i = 0; i < 4; i++)
		{
			int y = -(i + 1);
			for (int j = 0; j < widthInCells; j++)
			{
				array[i * widthInCells + j] = new CellOffset(j, y);
			}
		}
		Extents extents = new Extents(Grid.PosToCell(base.transform.GetPosition()), array);
		this.partitionerEntry_solids = GameScenePartitioner.Instance.Add("LiquidPumpingStation", base.gameObject, extents, GameScenePartitioner.Instance.solidChangedLayer, new Action<object>(this.OnLowerCellChanged));
		this.partitionerEntry_buildings = GameScenePartitioner.Instance.Add("LiquidPumpingStation", base.gameObject, extents, GameScenePartitioner.Instance.objectLayers[1], new Action<object>(this.OnLowerCellChanged));
	}

	// Token: 0x0600477A RID: 18298 RVA: 0x000CE5D4 File Offset: 0x000CC7D4
	private void UnregisterListenersToCellChanges()
	{
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry_solids);
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry_buildings);
	}

	// Token: 0x0600477B RID: 18299 RVA: 0x000CE5F6 File Offset: 0x000CC7F6
	private void OnLowerCellChanged(object o)
	{
		this.RefreshDepthAvailable();
	}

	// Token: 0x0600477C RID: 18300 RVA: 0x002528B8 File Offset: 0x00250AB8
	private void RefreshDepthAvailable()
	{
		int num = PumpingStationGuide.GetDepthAvailable(Grid.PosToCell(this), base.gameObject);
		int num2 = 4;
		if (num != this.depthAvailable)
		{
			KAnimControllerBase component = base.GetComponent<KAnimControllerBase>();
			for (int i = 1; i <= num2; i++)
			{
				component.SetSymbolVisiblity("pipe" + i.ToString(), i <= num);
			}
			PumpingStationGuide.OccupyArea(base.gameObject, num);
			this.depthAvailable = num;
		}
	}

	// Token: 0x0600477D RID: 18301 RVA: 0x0025292C File Offset: 0x00250B2C
	public void Sim200ms(float dt)
	{
		if (this.session != null)
		{
			return;
		}
		int num = this.infoCount;
		for (int i = 0; i < this.infoCount; i++)
		{
			this.infos[i].amount = 0f;
		}
		if (base.GetComponent<Operational>().IsOperational)
		{
			int cell = Grid.PosToCell(this);
			for (int j = 0; j < LiquidPumpingStation.liquidOffsets.Length; j++)
			{
				if (this.depthAvailable >= Math.Abs(LiquidPumpingStation.liquidOffsets[j].y))
				{
					int num2 = Grid.OffsetCell(cell, LiquidPumpingStation.liquidOffsets[j]);
					bool flag = false;
					Element element = Grid.Element[num2];
					if (element.IsLiquid)
					{
						float num3 = Grid.Mass[num2];
						for (int k = 0; k < this.infoCount; k++)
						{
							if (this.infos[k].element == element)
							{
								LiquidPumpingStation.LiquidInfo[] array = this.infos;
								int num4 = k;
								array[num4].amount = array[num4].amount + num3;
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							this.infos[this.infoCount].amount = num3;
							this.infos[this.infoCount].element = element;
							this.infoCount++;
						}
					}
				}
			}
		}
		int l = 0;
		while (l < this.infoCount)
		{
			LiquidPumpingStation.LiquidInfo liquidInfo = this.infos[l];
			if (liquidInfo.amount <= 1f)
			{
				if (liquidInfo.source != null)
				{
					liquidInfo.source.DeleteObject();
				}
				this.infos[l] = this.infos[this.infoCount - 1];
				this.infoCount--;
			}
			else
			{
				if (liquidInfo.source == null)
				{
					liquidInfo.source = base.GetComponent<Storage>().AddLiquid(liquidInfo.element.id, liquidInfo.amount, liquidInfo.element.defaultValues.temperature, byte.MaxValue, 0, false, true).GetComponent<SubstanceChunk>();
					Pickupable component = liquidInfo.source.GetComponent<Pickupable>();
					component.KPrefabID.AddTag(GameTags.LiquidSource, false);
					component.SetOffsets(new CellOffset[]
					{
						new CellOffset(0, 1)
					});
					component.targetWorkable = this;
					Pickupable pickupable = component;
					pickupable.OnReservationsChanged = (Action<Pickupable, bool, Pickupable.Reservation>)Delegate.Combine(pickupable.OnReservationsChanged, new Action<Pickupable, bool, Pickupable.Reservation>(this.OnReservationsChanged));
				}
				liquidInfo.source.GetComponent<Pickupable>().TotalAmount = liquidInfo.amount;
				this.infos[l] = liquidInfo;
				l++;
			}
		}
		if (num != this.infoCount)
		{
			this.RefreshStatusItem();
		}
	}

	// Token: 0x0600477E RID: 18302 RVA: 0x00252BFC File Offset: 0x00250DFC
	private void RefreshStatusItem()
	{
		if (this.infoCount > 0)
		{
			base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.PumpingStation, this);
			return;
		}
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.EmptyPumpingStation, this);
	}

	// Token: 0x0600477F RID: 18303 RVA: 0x00252C6C File Offset: 0x00250E6C
	public string ResolveString(string base_string)
	{
		string text = "";
		for (int i = 0; i < this.infoCount; i++)
		{
			if (this.infos[i].source != null)
			{
				text = string.Concat(new string[]
				{
					text,
					"\n",
					this.infos[i].element.name,
					": ",
					GameUtil.GetFormattedMass(this.infos[i].amount, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")
				});
			}
		}
		return base_string.Replace("{Liquids}", text);
	}

	// Token: 0x06004780 RID: 18304 RVA: 0x000A65EC File Offset: 0x000A47EC
	public static bool IsLiquidAccessible(Element element)
	{
		return true;
	}

	// Token: 0x06004781 RID: 18305 RVA: 0x000CE5FE File Offset: 0x000CC7FE
	public override float GetPercentComplete()
	{
		if (this.session != null)
		{
			return this.session.GetPercentComplete();
		}
		return 0f;
	}

	// Token: 0x06004782 RID: 18306 RVA: 0x00252D10 File Offset: 0x00250F10
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		Pickupable.PickupableStartWorkInfo pickupableStartWorkInfo = (Pickupable.PickupableStartWorkInfo)worker.GetStartWorkInfo();
		float amount = pickupableStartWorkInfo.amount;
		Element element = pickupableStartWorkInfo.originalPickupable.PrimaryElement.Element;
		this.session = new LiquidPumpingStation.WorkSession(Grid.PosToCell(this), element.id, pickupableStartWorkInfo.originalPickupable.GetComponent<SubstanceChunk>(), amount, base.gameObject);
		this.meter.SetPositionPercent(0f);
		this.meter.SetSymbolTint(new KAnimHashedString("meter_target"), element.substance.colour);
	}

	// Token: 0x06004783 RID: 18307 RVA: 0x00252DA4 File Offset: 0x00250FA4
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		if (this.session != null)
		{
			Storage component = worker.GetComponent<Storage>();
			float consumedAmount = this.session.GetConsumedAmount();
			if (consumedAmount > 0f)
			{
				SubstanceChunk source = this.session.GetSource();
				SimUtil.DiseaseInfo diseaseInfo = (this.session != null) ? this.session.GetDiseaseInfo() : SimUtil.DiseaseInfo.Invalid;
				PrimaryElement component2 = source.GetComponent<PrimaryElement>();
				Pickupable component3 = LiquidSourceManager.Instance.CreateChunk(component2.Element, consumedAmount, this.session.GetTemperature(), diseaseInfo.idx, diseaseInfo.count, base.transform.GetPosition()).GetComponent<Pickupable>();
				component3.TotalAmount = consumedAmount;
				component3.Trigger(1335436905, source.GetComponent<Pickupable>());
				worker.SetWorkCompleteData(component3);
				this.Sim200ms(0f);
				if (component3 != null)
				{
					component.Store(component3.gameObject, false, false, true, false);
				}
			}
			this.session.Cleanup();
			this.session = null;
		}
		base.GetComponent<KAnimControllerBase>().Play("on", KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x06004784 RID: 18308 RVA: 0x00252EC8 File Offset: 0x002510C8
	private void OnReservationsChanged(Pickupable _ignore, bool _ignore2, Pickupable.Reservation _ignore3)
	{
		bool forceUnfetchable = false;
		for (int i = 0; i < this.infoCount; i++)
		{
			if (this.infos[i].source != null && this.infos[i].source.GetComponent<Pickupable>().ReservedAmount > 0f)
			{
				forceUnfetchable = true;
				break;
			}
		}
		for (int j = 0; j < this.infoCount; j++)
		{
			if (this.infos[j].source != null)
			{
				FetchableMonitor.Instance smi = this.infos[j].source.GetSMI<FetchableMonitor.Instance>();
				if (smi != null)
				{
					smi.SetForceUnfetchable(forceUnfetchable);
				}
			}
		}
	}

	// Token: 0x06004785 RID: 18309 RVA: 0x000CE619 File Offset: 0x000CC819
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		if (this.session != null)
		{
			this.meter.SetPositionPercent(this.session.GetPercentComplete());
			if (this.session.GetLastTickAmount() <= 0f)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06004786 RID: 18310 RVA: 0x00252F74 File Offset: 0x00251174
	protected override void OnCleanUp()
	{
		this.UnregisterListenersToCellChanges();
		base.OnCleanUp();
		if (this.session != null)
		{
			this.session.Cleanup();
			this.session = null;
		}
		for (int i = 0; i < this.infoCount; i++)
		{
			if (this.infos[i].source != null)
			{
				this.infos[i].source.DeleteObject();
			}
		}
	}

	// Token: 0x040031A3 RID: 12707
	private static readonly CellOffset[] liquidOffsets = new CellOffset[]
	{
		new CellOffset(0, 0),
		new CellOffset(1, 0),
		new CellOffset(0, -1),
		new CellOffset(1, -1),
		new CellOffset(0, -2),
		new CellOffset(1, -2),
		new CellOffset(0, -3),
		new CellOffset(1, -3),
		new CellOffset(0, -4),
		new CellOffset(1, -4)
	};

	// Token: 0x040031A4 RID: 12708
	private LiquidPumpingStation.LiquidInfo[] infos;

	// Token: 0x040031A5 RID: 12709
	private int infoCount;

	// Token: 0x040031A6 RID: 12710
	private int depthAvailable = -1;

	// Token: 0x040031A7 RID: 12711
	private HandleVector<int>.Handle partitionerEntry_buildings;

	// Token: 0x040031A8 RID: 12712
	private HandleVector<int>.Handle partitionerEntry_solids;

	// Token: 0x040031A9 RID: 12713
	private LiquidPumpingStation.WorkSession session;

	// Token: 0x040031AA RID: 12714
	private MeterController meter;

	// Token: 0x02000E2F RID: 3631
	private class WorkSession
	{
		// Token: 0x06004789 RID: 18313 RVA: 0x00253094 File Offset: 0x00251294
		public WorkSession(int cell, SimHashes element, SubstanceChunk source, float amount_to_pickup, GameObject pump)
		{
			this.cell = cell;
			this.element = element;
			this.source = source;
			this.amountToPickup = amount_to_pickup;
			this.temperature = ElementLoader.FindElementByHash(element).defaultValues.temperature;
			this.diseaseInfo = SimUtil.DiseaseInfo.Invalid;
			this.amountPerTick = 40f;
			this.pump = pump;
			this.lastTickAmount = this.amountPerTick;
			this.ConsumeMass();
		}

		// Token: 0x0600478A RID: 18314 RVA: 0x000CE65D File Offset: 0x000CC85D
		private void OnSimConsumeCallback(Sim.MassConsumedCallback mass_cb_info, object data)
		{
			((LiquidPumpingStation.WorkSession)data).OnSimConsume(mass_cb_info);
		}

		// Token: 0x0600478B RID: 18315 RVA: 0x0025310C File Offset: 0x0025130C
		private void OnSimConsume(Sim.MassConsumedCallback mass_cb_info)
		{
			if (this.consumedAmount == 0f)
			{
				this.temperature = mass_cb_info.temperature;
			}
			else
			{
				this.temperature = GameUtil.GetFinalTemperature(this.temperature, this.consumedAmount, mass_cb_info.temperature, mass_cb_info.mass);
			}
			this.consumedAmount += mass_cb_info.mass;
			this.lastTickAmount = mass_cb_info.mass;
			this.diseaseInfo = SimUtil.CalculateFinalDiseaseInfo(this.diseaseInfo.idx, this.diseaseInfo.count, mass_cb_info.diseaseIdx, mass_cb_info.diseaseCount);
			if (this.consumedAmount >= this.amountToPickup)
			{
				this.amountPerTick = 0f;
				this.lastTickAmount = 0f;
			}
			this.ConsumeMass();
		}

		// Token: 0x0600478C RID: 18316 RVA: 0x002531D0 File Offset: 0x002513D0
		private void ConsumeMass()
		{
			if (this.amountPerTick > 0f)
			{
				float num = Mathf.Min(this.amountPerTick, this.amountToPickup - this.consumedAmount);
				num = Mathf.Max(num, 1f);
				HandleVector<Game.ComplexCallbackInfo<Sim.MassConsumedCallback>>.Handle handle = Game.Instance.massConsumedCallbackManager.Add(new Action<Sim.MassConsumedCallback, object>(this.OnSimConsumeCallback), this, "LiquidPumpingStation");
				int depthAvailable = PumpingStationGuide.GetDepthAvailable(this.cell, this.pump);
				SimMessages.ConsumeMass(Grid.OffsetCell(this.cell, new CellOffset(0, -depthAvailable)), this.element, num, (byte)(depthAvailable + 1), handle.index);
			}
		}

		// Token: 0x0600478D RID: 18317 RVA: 0x000CE66B File Offset: 0x000CC86B
		public float GetPercentComplete()
		{
			return this.consumedAmount / this.amountToPickup;
		}

		// Token: 0x0600478E RID: 18318 RVA: 0x000CE67A File Offset: 0x000CC87A
		public float GetLastTickAmount()
		{
			return this.lastTickAmount;
		}

		// Token: 0x0600478F RID: 18319 RVA: 0x000CE682 File Offset: 0x000CC882
		public SimUtil.DiseaseInfo GetDiseaseInfo()
		{
			return this.diseaseInfo;
		}

		// Token: 0x06004790 RID: 18320 RVA: 0x000CE68A File Offset: 0x000CC88A
		public SubstanceChunk GetSource()
		{
			return this.source;
		}

		// Token: 0x06004791 RID: 18321 RVA: 0x000CE692 File Offset: 0x000CC892
		public float GetConsumedAmount()
		{
			return this.consumedAmount;
		}

		// Token: 0x06004792 RID: 18322 RVA: 0x000CE69A File Offset: 0x000CC89A
		public float GetTemperature()
		{
			if (this.temperature <= 0f)
			{
				global::Debug.LogWarning("TODO(YOG): Fix bad temperature in liquid pumping station.");
				return ElementLoader.FindElementByHash(this.element).defaultValues.temperature;
			}
			return this.temperature;
		}

		// Token: 0x06004793 RID: 18323 RVA: 0x000CE6CF File Offset: 0x000CC8CF
		public void Cleanup()
		{
			this.amountPerTick = 0f;
			this.diseaseInfo = SimUtil.DiseaseInfo.Invalid;
		}

		// Token: 0x040031AB RID: 12715
		private int cell;

		// Token: 0x040031AC RID: 12716
		private float amountToPickup;

		// Token: 0x040031AD RID: 12717
		private float consumedAmount;

		// Token: 0x040031AE RID: 12718
		private float temperature;

		// Token: 0x040031AF RID: 12719
		private float amountPerTick;

		// Token: 0x040031B0 RID: 12720
		private SimHashes element;

		// Token: 0x040031B1 RID: 12721
		private float lastTickAmount;

		// Token: 0x040031B2 RID: 12722
		private SubstanceChunk source;

		// Token: 0x040031B3 RID: 12723
		private SimUtil.DiseaseInfo diseaseInfo;

		// Token: 0x040031B4 RID: 12724
		private GameObject pump;
	}

	// Token: 0x02000E30 RID: 3632
	private struct LiquidInfo
	{
		// Token: 0x040031B5 RID: 12725
		public float amount;

		// Token: 0x040031B6 RID: 12726
		public Element element;

		// Token: 0x040031B7 RID: 12727
		public SubstanceChunk source;
	}
}
