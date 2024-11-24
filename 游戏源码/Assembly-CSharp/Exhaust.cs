using System;
using UnityEngine;

// Token: 0x020012D2 RID: 4818
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/Exhaust")]
public class Exhaust : KMonoBehaviour, ISim200ms
{
	// Token: 0x060062ED RID: 25325 RVA: 0x000E09B3 File Offset: 0x000DEBB3
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<Exhaust>(-592767678, Exhaust.OnConduitStateChangedDelegate);
		base.Subscribe<Exhaust>(-111137758, Exhaust.OnConduitStateChangedDelegate);
		base.GetComponent<RequireInputs>().visualizeRequirements = RequireInputs.Requirements.NoWire;
		this.simRenderLoadBalance = true;
	}

	// Token: 0x060062EE RID: 25326 RVA: 0x000E09F0 File Offset: 0x000DEBF0
	protected override void OnSpawn()
	{
		this.OnConduitStateChanged(null);
	}

	// Token: 0x060062EF RID: 25327 RVA: 0x000E09F9 File Offset: 0x000DEBF9
	private void OnConduitStateChanged(object data)
	{
		this.operational.SetActive(this.operational.IsOperational && !this.vent.IsBlocked, false);
	}

	// Token: 0x060062F0 RID: 25328 RVA: 0x000E0A25 File Offset: 0x000DEC25
	private void CalculateDiseaseTransfer(PrimaryElement item1, PrimaryElement item2, float transfer_rate, out int disease_to_item1, out int disease_to_item2)
	{
		disease_to_item1 = (int)((float)item2.DiseaseCount * transfer_rate);
		disease_to_item2 = (int)((float)item1.DiseaseCount * transfer_rate);
	}

	// Token: 0x060062F1 RID: 25329 RVA: 0x002B8478 File Offset: 0x002B6678
	public void Sim200ms(float dt)
	{
		this.operational.SetFlag(Exhaust.canExhaust, !this.vent.IsBlocked);
		if (!this.operational.IsOperational)
		{
			if (this.isAnimating)
			{
				this.isAnimating = false;
				this.recentlyExhausted = false;
				base.Trigger(-793429877, null);
			}
			return;
		}
		this.UpdateEmission();
		this.elapsedSwitchTime -= dt;
		if (this.elapsedSwitchTime <= 0f)
		{
			this.elapsedSwitchTime = 1f;
			if (this.recentlyExhausted != this.isAnimating)
			{
				this.isAnimating = this.recentlyExhausted;
				base.Trigger(-793429877, null);
			}
			this.recentlyExhausted = false;
		}
	}

	// Token: 0x060062F2 RID: 25330 RVA: 0x000E0A41 File Offset: 0x000DEC41
	public bool IsAnimating()
	{
		return this.isAnimating;
	}

	// Token: 0x060062F3 RID: 25331 RVA: 0x002B852C File Offset: 0x002B672C
	private void UpdateEmission()
	{
		if (this.consumer.ConsumptionRate == 0f)
		{
			return;
		}
		if (this.storage.items.Count == 0)
		{
			return;
		}
		int num = Grid.PosToCell(base.transform.GetPosition());
		if (Grid.Solid[num])
		{
			return;
		}
		ConduitType typeOfConduit = this.consumer.TypeOfConduit;
		if (typeOfConduit != ConduitType.Gas)
		{
			if (typeOfConduit == ConduitType.Liquid)
			{
				this.EmitLiquid(num);
				return;
			}
		}
		else
		{
			this.EmitGas(num);
		}
	}

	// Token: 0x060062F4 RID: 25332 RVA: 0x002B85A4 File Offset: 0x002B67A4
	private bool EmitCommon(int cell, PrimaryElement primary_element, Exhaust.EmitDelegate emit)
	{
		if (primary_element.Mass <= 0f)
		{
			return false;
		}
		int num;
		int num2;
		this.CalculateDiseaseTransfer(this.exhaustPE, primary_element, 0.05f, out num, out num2);
		primary_element.ModifyDiseaseCount(-num, "Exhaust transfer");
		primary_element.AddDisease(this.exhaustPE.DiseaseIdx, num2, "Exhaust transfer");
		this.exhaustPE.ModifyDiseaseCount(-num2, "Exhaust transfer");
		this.exhaustPE.AddDisease(primary_element.DiseaseIdx, num, "Exhaust transfer");
		emit(cell, primary_element);
		if (this.vent != null)
		{
			this.vent.UpdateVentedMass(primary_element.ElementID, primary_element.Mass);
		}
		primary_element.KeepZeroMassObject = true;
		primary_element.Mass = 0f;
		primary_element.ModifyDiseaseCount(int.MinValue, "Exhaust.SimUpdate");
		if (this.lastElementEmmited != primary_element.ElementID)
		{
			this.lastElementEmmited = primary_element.ElementID;
			if (primary_element.Element != null && primary_element.Element.substance != null)
			{
				base.Trigger(-793429877, primary_element.Element.substance.colour);
			}
		}
		this.recentlyExhausted = true;
		return true;
	}

	// Token: 0x060062F5 RID: 25333 RVA: 0x002B86CC File Offset: 0x002B68CC
	private void EmitLiquid(int cell)
	{
		int num = Grid.CellBelow(cell);
		Exhaust.EmitDelegate emit = (Grid.IsValidCell(num) && !Grid.Solid[num]) ? Exhaust.emit_particle : Exhaust.emit_element;
		foreach (GameObject gameObject in this.storage.items)
		{
			PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
			if (component.Element.IsLiquid && this.EmitCommon(cell, component, emit))
			{
				break;
			}
		}
	}

	// Token: 0x060062F6 RID: 25334 RVA: 0x002B8768 File Offset: 0x002B6968
	private void EmitGas(int cell)
	{
		foreach (GameObject gameObject in this.storage.items)
		{
			PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
			if (component.Element.IsGas && this.EmitCommon(cell, component, Exhaust.emit_element))
			{
				break;
			}
		}
	}

	// Token: 0x04004696 RID: 18070
	[MyCmpGet]
	private Vent vent;

	// Token: 0x04004697 RID: 18071
	[MyCmpGet]
	private Storage storage;

	// Token: 0x04004698 RID: 18072
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04004699 RID: 18073
	[MyCmpGet]
	private ConduitConsumer consumer;

	// Token: 0x0400469A RID: 18074
	[MyCmpGet]
	private PrimaryElement exhaustPE;

	// Token: 0x0400469B RID: 18075
	private static readonly Operational.Flag canExhaust = new Operational.Flag("canExhaust", Operational.Flag.Type.Requirement);

	// Token: 0x0400469C RID: 18076
	private bool isAnimating;

	// Token: 0x0400469D RID: 18077
	private bool recentlyExhausted;

	// Token: 0x0400469E RID: 18078
	private const float MinSwitchTime = 1f;

	// Token: 0x0400469F RID: 18079
	private float elapsedSwitchTime;

	// Token: 0x040046A0 RID: 18080
	private SimHashes lastElementEmmited;

	// Token: 0x040046A1 RID: 18081
	private static readonly EventSystem.IntraObjectHandler<Exhaust> OnConduitStateChangedDelegate = new EventSystem.IntraObjectHandler<Exhaust>(delegate(Exhaust component, object data)
	{
		component.OnConduitStateChanged(data);
	});

	// Token: 0x040046A2 RID: 18082
	private static Exhaust.EmitDelegate emit_element = delegate(int cell, PrimaryElement primary_element)
	{
		SimMessages.AddRemoveSubstance(cell, primary_element.ElementID, CellEventLogger.Instance.ExhaustSimUpdate, primary_element.Mass, primary_element.Temperature, primary_element.DiseaseIdx, primary_element.DiseaseCount, true, -1);
	};

	// Token: 0x040046A3 RID: 18083
	private static Exhaust.EmitDelegate emit_particle = delegate(int cell, PrimaryElement primary_element)
	{
		FallingWater.instance.AddParticle(cell, primary_element.Element.idx, primary_element.Mass, primary_element.Temperature, primary_element.DiseaseIdx, primary_element.DiseaseCount, true, false, true, false);
	};

	// Token: 0x020012D3 RID: 4819
	// (Invoke) Token: 0x060062FA RID: 25338
	private delegate void EmitDelegate(int cell, PrimaryElement primary_element);
}
