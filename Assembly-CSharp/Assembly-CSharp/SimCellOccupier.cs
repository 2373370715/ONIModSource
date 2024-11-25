﻿using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/SimCellOccupier")]
public class SimCellOccupier : KMonoBehaviour, IGameObjectEffectDescriptor
{
			public bool IsVisuallySolid
	{
		get
		{
			return this.doReplaceElement;
		}
	}

		protected override void OnPrefabInit()
	{
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.Normal, null);
		if (this.building.Def.IsFoundation)
		{
			this.setConstructedTile = true;
		}
	}

		protected override void OnSpawn()
	{
		HandleVector<Game.CallbackInfo>.Handle callbackHandle = Game.Instance.callbackManager.Add(new Game.CallbackInfo(new System.Action(this.OnModifyComplete), false));
		int num = this.building.Def.PlacementOffsets.Length;
		float mass_per_cell = this.primaryElement.Mass / (float)num;
		this.building.RunOnArea(delegate(int offset_cell)
		{
			if (this.doReplaceElement)
			{
				SimMessages.ReplaceAndDisplaceElement(offset_cell, this.primaryElement.ElementID, CellEventLogger.Instance.SimCellOccupierOnSpawn, mass_per_cell, this.primaryElement.Temperature, this.primaryElement.DiseaseIdx, this.primaryElement.DiseaseCount, callbackHandle.index);
				callbackHandle = HandleVector<Game.CallbackInfo>.InvalidHandle;
				SimMessages.SetStrength(offset_cell, 0, this.strengthMultiplier);
				Game.Instance.RemoveSolidChangedFilter(offset_cell);
			}
			else
			{
				if (SaveGame.Instance.sandboxEnabled && Grid.Element[offset_cell].IsSolid)
				{
					SimMessages.Dig(offset_cell, -1, false);
				}
				this.ForceSetGameCellData(offset_cell);
				Game.Instance.AddSolidChangedFilter(offset_cell);
			}
			Sim.Cell.Properties simCellProperties = this.GetSimCellProperties();
			SimMessages.SetCellProperties(offset_cell, (byte)simCellProperties);
			Grid.RenderedByWorld[offset_cell] = false;
			Game.Instance.GetComponent<EntombedItemVisualizer>().ForceClear(offset_cell);
		});
		base.Subscribe(675471409, new Action<object>(this.OnMelted));
		base.Subscribe<SimCellOccupier>(-1699355994, SimCellOccupier.OnBuildingRepairedDelegate);
	}

		private void OnMelted(object o)
	{
		Building.CreateBuildingMeltedNotification(base.gameObject);
	}

		protected override void OnCleanUp()
	{
		if (this.callDestroy)
		{
			this.DestroySelf(null);
		}
	}

		private Sim.Cell.Properties GetSimCellProperties()
	{
		Sim.Cell.Properties properties = Sim.Cell.Properties.SolidImpermeable;
		if (this.setGasImpermeable)
		{
			properties |= Sim.Cell.Properties.GasImpermeable;
		}
		if (this.setLiquidImpermeable)
		{
			properties |= Sim.Cell.Properties.LiquidImpermeable;
		}
		if (this.setTransparent)
		{
			properties |= Sim.Cell.Properties.Transparent;
		}
		if (this.setOpaque)
		{
			properties |= Sim.Cell.Properties.Opaque;
		}
		if (this.setConstructedTile)
		{
			properties |= Sim.Cell.Properties.ConstructedTile;
		}
		if (this.notifyOnMelt)
		{
			properties |= Sim.Cell.Properties.NotifyOnMelt;
		}
		return properties;
	}

		public void DestroySelf(System.Action onComplete)
	{
		this.callDestroy = false;
		for (int i = 0; i < this.building.PlacementCells.Length; i++)
		{
			int num = this.building.PlacementCells[i];
			Game.Instance.RemoveSolidChangedFilter(num);
			Sim.Cell.Properties simCellProperties = this.GetSimCellProperties();
			SimMessages.ClearCellProperties(num, (byte)simCellProperties);
			if (this.doReplaceElement && Grid.Element[num].id == this.primaryElement.ElementID)
			{
				HandleVector<int>.Handle handle = GameComps.DiseaseContainers.GetHandle(base.gameObject);
				if (handle.IsValid())
				{
					DiseaseHeader header = GameComps.DiseaseContainers.GetHeader(handle);
					header.diseaseIdx = Grid.DiseaseIdx[num];
					header.diseaseCount = Grid.DiseaseCount[num];
					GameComps.DiseaseContainers.SetHeader(handle, header);
				}
				if (onComplete != null)
				{
					HandleVector<Game.CallbackInfo>.Handle handle2 = Game.Instance.callbackManager.Add(new Game.CallbackInfo(onComplete, false));
					SimMessages.ReplaceElement(num, SimHashes.Vacuum, CellEventLogger.Instance.SimCellOccupierDestroySelf, 0f, -1f, byte.MaxValue, 0, handle2.index);
				}
				else
				{
					SimMessages.ReplaceElement(num, SimHashes.Vacuum, CellEventLogger.Instance.SimCellOccupierDestroySelf, 0f, -1f, byte.MaxValue, 0, -1);
				}
				SimMessages.SetStrength(num, 1, 1f);
			}
			else
			{
				Grid.SetSolid(num, false, CellEventLogger.Instance.SimCellOccupierDestroy);
				onComplete.Signal();
				World.Instance.OnSolidChanged(num);
				GameScenePartitioner.Instance.TriggerEvent(num, GameScenePartitioner.Instance.solidChangedLayer, null);
			}
		}
	}

		public bool IsReady()
	{
		return this.isReady;
	}

		private void OnModifyComplete()
	{
		if (this == null || base.gameObject == null)
		{
			return;
		}
		this.isReady = true;
		base.GetComponent<PrimaryElement>().SetUseSimDiseaseInfo(true);
		Vector2I vector2I = Grid.PosToXY(base.transform.GetPosition());
		GameScenePartitioner.Instance.TriggerEvent(vector2I.x, vector2I.y, 1, 1, GameScenePartitioner.Instance.solidChangedLayer, null);
	}

		private void ForceSetGameCellData(int cell)
	{
		bool solid = !Grid.DupePassable[cell];
		Grid.SetSolid(cell, solid, CellEventLogger.Instance.SimCellOccupierForceSolid);
		Pathfinding.Instance.AddDirtyNavGridCell(cell);
		GameScenePartitioner.Instance.TriggerEvent(cell, GameScenePartitioner.Instance.solidChangedLayer, null);
		Grid.Damage[cell] = 0f;
	}

		public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = null;
		if (this.movementSpeedMultiplier != 1f)
		{
			list = new List<Descriptor>();
			Descriptor item = default(Descriptor);
			item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.DUPLICANTMOVEMENTBOOST, GameUtil.AddPositiveSign(GameUtil.GetFormattedPercent(this.movementSpeedMultiplier * 100f - 100f, GameUtil.TimeSlice.None), this.movementSpeedMultiplier - 1f >= 0f)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.DUPLICANTMOVEMENTBOOST, GameUtil.GetFormattedPercent(this.movementSpeedMultiplier * 100f - 100f, GameUtil.TimeSlice.None)), Descriptor.DescriptorType.Effect);
			list.Add(item);
		}
		return list;
	}

		private void OnBuildingRepaired(object data)
	{
		BuildingHP buildingHP = (BuildingHP)data;
		float damage = 1f - (float)buildingHP.HitPoints / (float)buildingHP.MaxHitPoints;
		this.building.RunOnArea(delegate(int offset_cell)
		{
			WorldDamage.Instance.RestoreDamageToValue(offset_cell, damage);
		});
	}

		[MyCmpReq]
	private Building building;

		[MyCmpReq]
	private PrimaryElement primaryElement;

		[SerializeField]
	public bool doReplaceElement = true;

		[SerializeField]
	public bool setGasImpermeable;

		[SerializeField]
	public bool setLiquidImpermeable;

		[SerializeField]
	public bool setTransparent;

		[SerializeField]
	public bool setOpaque;

		[SerializeField]
	public bool notifyOnMelt;

		[SerializeField]
	private bool setConstructedTile;

		[SerializeField]
	public float strengthMultiplier = 1f;

		[SerializeField]
	public float movementSpeedMultiplier = 1f;

		private bool isReady;

		private bool callDestroy = true;

		private static readonly EventSystem.IntraObjectHandler<SimCellOccupier> OnBuildingRepairedDelegate = new EventSystem.IntraObjectHandler<SimCellOccupier>(delegate(SimCellOccupier component, object data)
	{
		component.OnBuildingRepaired(data);
	});
}
