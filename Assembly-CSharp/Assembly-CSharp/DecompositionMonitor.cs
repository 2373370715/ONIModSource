﻿using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

public class DecompositionMonitor : GameStateMachine<DecompositionMonitor, DecompositionMonitor.Instance>
{
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.satisfied.Update("UpdateDecomposition", delegate(DecompositionMonitor.Instance smi, float dt)
		{
			smi.UpdateDecomposition(dt);
		}, UpdateRate.SIM_200ms, false).ParamTransition<float>(this.decomposition, this.rotten, GameStateMachine<DecompositionMonitor, DecompositionMonitor.Instance, IStateMachineTarget, object>.IsGTEOne).ToggleAttributeModifier("Dead", (DecompositionMonitor.Instance smi) => smi.satisfiedDecorModifier, null).ToggleAttributeModifier("Dead", (DecompositionMonitor.Instance smi) => smi.satisfiedDecorRadiusModifier, null);
		this.rotten.DefaultState(this.rotten.exposed).ToggleStatusItem(Db.Get().DuplicantStatusItems.Rotten, null).ToggleAttributeModifier("Rotten", (DecompositionMonitor.Instance smi) => smi.rottenDecorModifier, null).ToggleAttributeModifier("Rotten", (DecompositionMonitor.Instance smi) => smi.rottenDecorRadiusModifier, null);
		this.rotten.exposed.DefaultState(this.rotten.exposed.openair).EventTransition(GameHashes.OnStore, this.rotten.stored, (DecompositionMonitor.Instance smi) => !smi.IsExposed());
		this.rotten.exposed.openair.Enter(delegate(DecompositionMonitor.Instance smi)
		{
			if (smi.spawnsRotMonsters)
			{
				smi.ScheduleGoTo(UnityEngine.Random.Range(150f, 300f), this.rotten.spawningmonster);
			}
		}).Transition(this.rotten.exposed.submerged, (DecompositionMonitor.Instance smi) => smi.IsSubmerged(), UpdateRate.SIM_200ms).ToggleFX((DecompositionMonitor.Instance smi) => this.CreateFX(smi));
		this.rotten.exposed.submerged.DefaultState(this.rotten.exposed.submerged.idle).Transition(this.rotten.exposed.openair, (DecompositionMonitor.Instance smi) => !smi.IsSubmerged(), UpdateRate.SIM_200ms);
		this.rotten.exposed.submerged.idle.ScheduleGoTo(0.25f, this.rotten.exposed.submerged.dirtywater);
		this.rotten.exposed.submerged.dirtywater.Enter("DirtyWater", delegate(DecompositionMonitor.Instance smi)
		{
			smi.DirtyWater(smi.dirtyWaterMaxRange);
		}).GoTo(this.rotten.exposed.submerged.idle);
		this.rotten.spawningmonster.Enter(delegate(DecompositionMonitor.Instance smi)
		{
			if (this.remainingRotMonsters > 0)
			{
				this.remainingRotMonsters--;
				GameUtil.KInstantiate(Assets.GetPrefab(new Tag("Glom")), smi.transform.GetPosition(), Grid.SceneLayer.Creatures, null, 0).SetActive(true);
			}
			smi.GoTo(this.rotten.exposed);
		});
		this.rotten.stored.EventTransition(GameHashes.OnStore, this.rotten.exposed, (DecompositionMonitor.Instance smi) => smi.IsExposed());
	}

	private FliesFX.Instance CreateFX(DecompositionMonitor.Instance smi)
	{
		if (!smi.isMasterNull)
		{
			return new FliesFX.Instance(smi.master, new Vector3(0f, 0f, -0.1f));
		}
		return null;
	}

	public StateMachine<DecompositionMonitor, DecompositionMonitor.Instance, IStateMachineTarget, object>.FloatParameter decomposition;

	[SerializeField]
	public int remainingRotMonsters = 3;

	public GameStateMachine<DecompositionMonitor, DecompositionMonitor.Instance, IStateMachineTarget, object>.State satisfied;

	public DecompositionMonitor.RottenState rotten;

	public class SubmergedState : GameStateMachine<DecompositionMonitor, DecompositionMonitor.Instance, IStateMachineTarget, object>.State
	{
		public GameStateMachine<DecompositionMonitor, DecompositionMonitor.Instance, IStateMachineTarget, object>.State idle;

		public GameStateMachine<DecompositionMonitor, DecompositionMonitor.Instance, IStateMachineTarget, object>.State dirtywater;
	}

	public class ExposedState : GameStateMachine<DecompositionMonitor, DecompositionMonitor.Instance, IStateMachineTarget, object>.State
	{
		public DecompositionMonitor.SubmergedState submerged;

		public GameStateMachine<DecompositionMonitor, DecompositionMonitor.Instance, IStateMachineTarget, object>.State openair;
	}

	public class RottenState : GameStateMachine<DecompositionMonitor, DecompositionMonitor.Instance, IStateMachineTarget, object>.State
	{
		public DecompositionMonitor.ExposedState exposed;

		public GameStateMachine<DecompositionMonitor, DecompositionMonitor.Instance, IStateMachineTarget, object>.State stored;

		public GameStateMachine<DecompositionMonitor, DecompositionMonitor.Instance, IStateMachineTarget, object>.State spawningmonster;
	}

	public new class Instance : GameStateMachine<DecompositionMonitor, DecompositionMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		public Instance(IStateMachineTarget master, Disease disease, float decompositionRate = 0.00083333335f, bool spawnRotMonsters = true) : base(master)
		{
			base.gameObject.AddComponent<DecorProvider>();
			this.decompositionRate = decompositionRate;
			this.disease = disease;
			this.spawnsRotMonsters = spawnRotMonsters;
		}

		public void UpdateDecomposition(float dt)
		{
			float delta_value = dt * this.decompositionRate;
			base.sm.decomposition.Delta(delta_value, base.smi);
		}

		public bool IsExposed()
		{
			KPrefabID component = base.smi.GetComponent<KPrefabID>();
			return component == null || !component.HasTag(GameTags.Preserved);
		}

		public bool IsRotten()
		{
			return base.IsInsideState(base.sm.rotten);
		}

		public bool IsSubmerged()
		{
			return PathFinder.IsSubmerged(Grid.PosToCell(base.master.transform.GetPosition()));
		}

		public void DirtyWater(int maxCellRange = 3)
		{
			int num = Grid.PosToCell(base.master.transform.GetPosition());
			if (Grid.Element[num].id == SimHashes.Water)
			{
				SimMessages.ReplaceElement(num, SimHashes.DirtyWater, CellEventLogger.Instance.DecompositionDirtyWater, Grid.Mass[num], Grid.Temperature[num], Grid.DiseaseIdx[num], Grid.DiseaseCount[num], -1);
				return;
			}
			if (Grid.Element[num].id == SimHashes.DirtyWater)
			{
				int[] array = new int[4];
				for (int i = 0; i < maxCellRange; i++)
				{
					for (int j = 0; j < maxCellRange; j++)
					{
						array[0] = Grid.OffsetCell(num, new CellOffset(-i, j));
						array[1] = Grid.OffsetCell(num, new CellOffset(i, j));
						array[2] = Grid.OffsetCell(num, new CellOffset(-i, -j));
						array[3] = Grid.OffsetCell(num, new CellOffset(i, -j));
						array.Shuffle<int>();
						foreach (int num2 in array)
						{
							if (Grid.GetCellDistance(num, num2) < maxCellRange - 1 && Grid.IsValidCell(num2) && Grid.Element[num2].id == SimHashes.Water)
							{
								SimMessages.ReplaceElement(num2, SimHashes.DirtyWater, CellEventLogger.Instance.DecompositionDirtyWater, Grid.Mass[num2], Grid.Temperature[num2], Grid.DiseaseIdx[num2], Grid.DiseaseCount[num2], -1);
								return;
							}
						}
					}
				}
			}
		}

		public float decompositionRate;

		public Disease disease;

		public int dirtyWaterMaxRange = 3;

		public bool spawnsRotMonsters = true;

		public AttributeModifier satisfiedDecorModifier = new AttributeModifier(Db.Get().BuildingAttributes.Decor.Id, -65f, DUPLICANTS.MODIFIERS.DEAD.NAME, false, false, true);

		public AttributeModifier satisfiedDecorRadiusModifier = new AttributeModifier(Db.Get().BuildingAttributes.DecorRadius.Id, 4f, DUPLICANTS.MODIFIERS.DEAD.NAME, false, false, true);

		public AttributeModifier rottenDecorModifier = new AttributeModifier(Db.Get().BuildingAttributes.Decor.Id, -100f, DUPLICANTS.MODIFIERS.ROTTING.NAME, false, false, true);

		public AttributeModifier rottenDecorRadiusModifier = new AttributeModifier(Db.Get().BuildingAttributes.DecorRadius.Id, 4f, DUPLICANTS.MODIFIERS.ROTTING.NAME, false, false, true);
	}
}
