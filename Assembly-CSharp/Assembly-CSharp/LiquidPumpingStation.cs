﻿using System;
using Klei;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/Workable/LiquidPumpingStation")]
public class LiquidPumpingStation : Workable, ISim200ms
{
		protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.resetProgressOnStop = true;
		this.showProgressBar = false;
	}

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

		private void UnregisterListenersToCellChanges()
	{
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry_solids);
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry_buildings);
	}

		private void OnLowerCellChanged(object o)
	{
		this.RefreshDepthAvailable();
	}

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

		private void RefreshStatusItem()
	{
		if (this.infoCount > 0)
		{
			base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.PumpingStation, this);
			return;
		}
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.EmptyPumpingStation, this);
	}

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

		public static bool IsLiquidAccessible(Element element)
	{
		return true;
	}

		public override float GetPercentComplete()
	{
		if (this.session != null)
		{
			return this.session.GetPercentComplete();
		}
		return 0f;
	}

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

		private LiquidPumpingStation.LiquidInfo[] infos;

		private int infoCount;

		private int depthAvailable = -1;

		private HandleVector<int>.Handle partitionerEntry_buildings;

		private HandleVector<int>.Handle partitionerEntry_solids;

		private LiquidPumpingStation.WorkSession session;

		private MeterController meter;

		private class WorkSession
	{
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

				private void OnSimConsumeCallback(Sim.MassConsumedCallback mass_cb_info, object data)
		{
			((LiquidPumpingStation.WorkSession)data).OnSimConsume(mass_cb_info);
		}

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

				public float GetPercentComplete()
		{
			return this.consumedAmount / this.amountToPickup;
		}

				public float GetLastTickAmount()
		{
			return this.lastTickAmount;
		}

				public SimUtil.DiseaseInfo GetDiseaseInfo()
		{
			return this.diseaseInfo;
		}

				public SubstanceChunk GetSource()
		{
			return this.source;
		}

				public float GetConsumedAmount()
		{
			return this.consumedAmount;
		}

				public float GetTemperature()
		{
			if (this.temperature <= 0f)
			{
				global::Debug.LogWarning("TODO(YOG): Fix bad temperature in liquid pumping station.");
				return ElementLoader.FindElementByHash(this.element).defaultValues.temperature;
			}
			return this.temperature;
		}

				public void Cleanup()
		{
			this.amountPerTick = 0f;
			this.diseaseInfo = SimUtil.DiseaseInfo.Invalid;
		}

				private int cell;

				private float amountToPickup;

				private float consumedAmount;

				private float temperature;

				private float amountPerTick;

				private SimHashes element;

				private float lastTickAmount;

				private SubstanceChunk source;

				private SimUtil.DiseaseInfo diseaseInfo;

				private GameObject pump;
	}

		private struct LiquidInfo
	{
				public float amount;

				public Element element;

				public SubstanceChunk source;
	}
}
