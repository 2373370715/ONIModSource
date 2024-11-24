using System;
using System.Diagnostics;
using Klei.AI;
using UnityEngine;

// Token: 0x02000A27 RID: 2599
public class SolidConsumerMonitor : GameStateMachine<SolidConsumerMonitor, SolidConsumerMonitor.Instance, IStateMachineTarget, SolidConsumerMonitor.Def>
{
	// Token: 0x06002F78 RID: 12152 RVA: 0x001F7EF8 File Offset: 0x001F60F8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		this.root.EventHandler(GameHashes.EatSolidComplete, delegate(SolidConsumerMonitor.Instance smi, object data)
		{
			smi.OnEatSolidComplete(data);
		}).ToggleBehaviour(GameTags.Creatures.WantsToEat, (SolidConsumerMonitor.Instance smi) => smi.targetEdible != null && !smi.targetEdible.HasTag(GameTags.Creatures.ReservedByCreature), null);
		this.satisfied.TagTransition(GameTags.Creatures.Hungry, this.lookingforfood, false);
		this.lookingforfood.TagTransition(GameTags.Creatures.Hungry, this.satisfied, true).PreBrainUpdate(new Action<SolidConsumerMonitor.Instance>(SolidConsumerMonitor.FindFood));
	}

	// Token: 0x06002F79 RID: 12153 RVA: 0x000A5E40 File Offset: 0x000A4040
	[Conditional("DETAILED_SOLID_CONSUMER_MONITOR_PROFILE")]
	private static void BeginDetailedSample(string region_name)
	{
	}

	// Token: 0x06002F7A RID: 12154 RVA: 0x000A5E40 File Offset: 0x000A4040
	[Conditional("DETAILED_SOLID_CONSUMER_MONITOR_PROFILE")]
	private static void EndDetailedSample(string region_name)
	{
	}

	// Token: 0x06002F7B RID: 12155 RVA: 0x001F7FA8 File Offset: 0x001F61A8
	private static void FindFood(SolidConsumerMonitor.Instance smi)
	{
		if (smi.IsTargetEdibleValid())
		{
			return;
		}
		smi.ClearTargetEdible();
		Diet diet = smi.diet;
		int num = 0;
		int num2 = 0;
		Grid.PosToXY(smi.gameObject.transform.GetPosition(), out num, out num2);
		num -= 8;
		num2 -= 8;
		bool flag = false;
		if (!diet.AllConsumablesAreDirectlyEdiblePlants)
		{
			ListPool<Storage, SolidConsumerMonitor>.PooledList pooledList = ListPool<Storage, SolidConsumerMonitor>.Allocate();
			int num3 = 32;
			foreach (CreatureFeeder creatureFeeder in Components.CreatureFeeders.GetItems(smi.GetMyWorldId()))
			{
				Vector2I targetFeederCell = creatureFeeder.GetTargetFeederCell();
				if (targetFeederCell.x >= num && targetFeederCell.x <= num + num3 && targetFeederCell.y >= num2 && targetFeederCell.y <= num2 + num3 && !creatureFeeder.StoragesAreEmpty())
				{
					int cost = smi.GetCost(Grid.XYToCell(targetFeederCell.x, targetFeederCell.y));
					if (smi.IsCloserThanTargetEdible(cost))
					{
						foreach (Storage storage in creatureFeeder.storages)
						{
							if (!(storage == null) && !storage.IsEmpty() && smi.GetCost(Grid.PosToCell(storage.items[0])) != -1)
							{
								foreach (GameObject gameObject in storage.items)
								{
									if (!(gameObject == null))
									{
										KPrefabID component = gameObject.GetComponent<KPrefabID>();
										if (!component.HasAnyTags(SolidConsumerMonitor.creatureTags) && diet.GetDietInfo(component.PrefabTag) != null)
										{
											smi.SetTargetEdible(gameObject, cost);
											smi.targetEdibleOffset = Vector3.zero;
											flag = true;
											break;
										}
									}
								}
								if (flag)
								{
									break;
								}
							}
						}
					}
				}
			}
			pooledList.Recycle();
		}
		bool flag2 = false;
		if (diet.CanEatAnyPlantDirectly)
		{
			ListPool<ScenePartitionerEntry, GameScenePartitioner>.PooledList pooledList2 = ListPool<ScenePartitionerEntry, GameScenePartitioner>.Allocate();
			GameScenePartitioner.Instance.GatherEntries(num, num2, 16, 16, GameScenePartitioner.Instance.plants, pooledList2);
			foreach (ScenePartitionerEntry scenePartitionerEntry in pooledList2)
			{
				KPrefabID kprefabID = (KPrefabID)scenePartitionerEntry.obj;
				Diet.Info dietInfo = diet.GetDietInfo(kprefabID.PrefabTag);
				Vector3 vector = kprefabID.transform.GetPosition();
				bool flag3 = kprefabID.HasTag(GameTags.PlantedOnFloorVessel);
				if (flag3)
				{
					vector += SolidConsumerMonitor.PLANT_ON_FLOOR_VESSEL_OFFSET;
				}
				int num4 = smi.GetCost(Grid.PosToCell(vector));
				Vector3 a = Vector3.zero;
				if (smi.IsCloserThanTargetEdible(num4) && !kprefabID.HasAnyTags(SolidConsumerMonitor.creatureTags) && dietInfo != null)
				{
					if (kprefabID.HasTag(GameTags.Plant))
					{
						IPlantConsumptionInstructions[] plantConsumptionInstructions = GameUtil.GetPlantConsumptionInstructions(kprefabID.gameObject);
						if (plantConsumptionInstructions == null || plantConsumptionInstructions.Length == 0)
						{
							continue;
						}
						bool flag4 = false;
						foreach (IPlantConsumptionInstructions plantConsumptionInstructions2 in plantConsumptionInstructions)
						{
							if (plantConsumptionInstructions2.CanPlantBeEaten() && dietInfo.foodType == plantConsumptionInstructions2.GetDietFoodType())
							{
								CellOffset[] allowedOffsets = plantConsumptionInstructions2.GetAllowedOffsets();
								if (allowedOffsets != null)
								{
									num4 = -1;
									foreach (CellOffset offset in allowedOffsets)
									{
										int cost2 = smi.GetCost(Grid.OffsetCell(Grid.PosToCell(vector), offset));
										if (cost2 != -1 && (num4 == -1 || cost2 < num4))
										{
											num4 = cost2;
											a = offset.ToVector3();
										}
									}
									if (num4 != -1)
									{
										flag4 = true;
										break;
									}
								}
								else
								{
									flag4 = true;
								}
							}
						}
						if (!flag4)
						{
							continue;
						}
					}
					smi.SetTargetEdible(kprefabID.gameObject, num4);
					smi.targetEdibleOffset = a + (flag3 ? SolidConsumerMonitor.PLANT_ON_FLOOR_VESSEL_OFFSET : Vector3.zero);
					flag2 = true;
				}
			}
			pooledList2.Recycle();
		}
		if (!flag2 && diet.CanEatAnyNonDirectlyEdiblePlant && smi.CanSearchForPickupables(flag))
		{
			bool flag5 = false;
			ListPool<ScenePartitionerEntry, GameScenePartitioner>.PooledList pooledList3 = ListPool<ScenePartitionerEntry, GameScenePartitioner>.Allocate();
			GameScenePartitioner.Instance.GatherEntries(num, num2, 16, 16, GameScenePartitioner.Instance.pickupablesLayer, pooledList3);
			foreach (ScenePartitionerEntry scenePartitionerEntry2 in pooledList3)
			{
				Pickupable pickupable = (Pickupable)scenePartitionerEntry2.obj;
				KPrefabID kprefabID2 = pickupable.KPrefabID;
				if (!kprefabID2.HasAnyTags(SolidConsumerMonitor.creatureTags) && diet.GetDietInfo(kprefabID2.PrefabTag) != null)
				{
					bool flag6;
					smi.ProcessEdible(pickupable.gameObject, out flag6);
					smi.targetEdibleOffset = Vector3.zero;
					flag5 = (flag5 || flag6);
				}
			}
			pooledList3.Recycle();
		}
	}

	// Token: 0x04002013 RID: 8211
	public static Vector3 PLANT_ON_FLOOR_VESSEL_OFFSET = Vector3.down;

	// Token: 0x04002014 RID: 8212
	private GameStateMachine<SolidConsumerMonitor, SolidConsumerMonitor.Instance, IStateMachineTarget, SolidConsumerMonitor.Def>.State satisfied;

	// Token: 0x04002015 RID: 8213
	private GameStateMachine<SolidConsumerMonitor, SolidConsumerMonitor.Instance, IStateMachineTarget, SolidConsumerMonitor.Def>.State lookingforfood;

	// Token: 0x04002016 RID: 8214
	private static Tag[] creatureTags = new Tag[]
	{
		GameTags.Creatures.ReservedByCreature,
		GameTags.CreatureBrain
	};

	// Token: 0x02000A28 RID: 2600
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04002017 RID: 8215
		public Diet diet;
	}

	// Token: 0x02000A29 RID: 2601
	public new class Instance : GameStateMachine<SolidConsumerMonitor, SolidConsumerMonitor.Instance, IStateMachineTarget, SolidConsumerMonitor.Def>.GameInstance
	{
		// Token: 0x06002F7F RID: 12159 RVA: 0x000BEDC6 File Offset: 0x000BCFC6
		public Instance(IStateMachineTarget master, SolidConsumerMonitor.Def def) : base(master, def)
		{
			this.diet = DietManager.Instance.GetPrefabDiet(base.gameObject);
		}

		// Token: 0x06002F80 RID: 12160 RVA: 0x000BEDE6 File Offset: 0x000BCFE6
		public bool CanSearchForPickupables(bool foodAtFeeder)
		{
			return !foodAtFeeder;
		}

		// Token: 0x06002F81 RID: 12161 RVA: 0x000BEDEC File Offset: 0x000BCFEC
		public bool IsCloserThanTargetEdible(int cost)
		{
			return cost != -1 && (cost < this.targetEdibleCost || this.targetEdibleCost == -1);
		}

		// Token: 0x06002F82 RID: 12162 RVA: 0x001F84D4 File Offset: 0x001F66D4
		public bool IsTargetEdibleValid()
		{
			if (this.targetEdible == null || this.targetEdible.HasTag(GameTags.Creatures.ReservedByCreature))
			{
				return false;
			}
			int cost = this.GetCost(Grid.PosToCell(this.targetEdible.transform.GetPosition() + this.targetEdibleOffset));
			return cost != -1 && this.targetEdibleCost <= cost + 4;
		}

		// Token: 0x06002F83 RID: 12163 RVA: 0x000BEE08 File Offset: 0x000BD008
		public void ClearTargetEdible()
		{
			this.targetEdibleCost = -1;
			this.targetEdible = null;
			this.targetEdibleOffset = Vector3.zero;
		}

		// Token: 0x06002F84 RID: 12164 RVA: 0x001F853C File Offset: 0x001F673C
		public bool ProcessEdible(GameObject edible, out bool isReachable)
		{
			int cost = this.GetCost(edible);
			isReachable = (cost != -1);
			if (cost != -1 && (cost < this.targetEdibleCost || this.targetEdibleCost == -1))
			{
				this.targetEdibleCost = cost;
				this.targetEdible = edible.gameObject;
				return true;
			}
			return false;
		}

		// Token: 0x06002F85 RID: 12165 RVA: 0x000BEE23 File Offset: 0x000BD023
		public void SetTargetEdible(GameObject gameObject, int cost)
		{
			this.targetEdibleCost = cost;
			this.targetEdible = gameObject;
		}

		// Token: 0x06002F86 RID: 12166 RVA: 0x000BEE33 File Offset: 0x000BD033
		public int GetCost(GameObject edible)
		{
			return this.GetCost(Grid.PosToCell(edible.transform.GetPosition()));
		}

		// Token: 0x06002F87 RID: 12167 RVA: 0x001F8588 File Offset: 0x001F6788
		public int GetCost(int cell)
		{
			if (this.drowningMonitor != null && this.drowningMonitor.canDrownToDeath && !this.drowningMonitor.livesUnderWater && !this.drowningMonitor.IsCellSafe(cell))
			{
				return -1;
			}
			return this.navigator.GetNavigationCost(cell);
		}

		// Token: 0x06002F88 RID: 12168 RVA: 0x001F85E0 File Offset: 0x001F67E0
		public void OnEatSolidComplete(object data)
		{
			KPrefabID kprefabID = data as KPrefabID;
			if (kprefabID == null)
			{
				return;
			}
			PrimaryElement component = kprefabID.GetComponent<PrimaryElement>();
			if (component == null)
			{
				return;
			}
			Diet.Info dietInfo = this.diet.GetDietInfo(kprefabID.PrefabTag);
			if (dietInfo == null)
			{
				return;
			}
			AmountInstance amountInstance = Db.Get().Amounts.Calories.Lookup(base.smi.gameObject);
			string properName = kprefabID.GetProperName();
			PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Negative, properName, kprefabID.transform, 1.5f, false);
			float calories = amountInstance.GetMax() - amountInstance.value;
			float num = dietInfo.ConvertCaloriesToConsumptionMass(calories);
			IPlantConsumptionInstructions plantConsumptionInstructions = null;
			foreach (IPlantConsumptionInstructions plantConsumptionInstructions3 in GameUtil.GetPlantConsumptionInstructions(kprefabID.gameObject))
			{
				if (dietInfo.foodType == plantConsumptionInstructions3.GetDietFoodType())
				{
					plantConsumptionInstructions = plantConsumptionInstructions3;
				}
			}
			if (plantConsumptionInstructions != null)
			{
				num = plantConsumptionInstructions.ConsumePlant(num);
			}
			else
			{
				num = Mathf.Min(num, component.Mass);
				component.Mass -= num;
				Pickupable component2 = component.GetComponent<Pickupable>();
				if (component2.storage != null)
				{
					component2.storage.Trigger(-1452790913, base.gameObject);
					component2.storage.Trigger(-1697596308, base.gameObject);
				}
			}
			float calories2 = dietInfo.ConvertConsumptionMassToCalories(num);
			CreatureCalorieMonitor.CaloriesConsumedEvent caloriesConsumedEvent = new CreatureCalorieMonitor.CaloriesConsumedEvent
			{
				tag = kprefabID.PrefabTag,
				calories = calories2
			};
			base.Trigger(-2038961714, caloriesConsumedEvent);
			this.targetEdible = null;
		}

		// Token: 0x06002F89 RID: 12169 RVA: 0x000BEE4B File Offset: 0x000BD04B
		public string[] GetTargetEdibleEatAnims()
		{
			return this.diet.GetDietInfo(this.targetEdible.PrefabID()).eatAnims;
		}

		// Token: 0x04002018 RID: 8216
		private const int RECALC_THRESHOLD = 4;

		// Token: 0x04002019 RID: 8217
		public GameObject targetEdible;

		// Token: 0x0400201A RID: 8218
		public Vector3 targetEdibleOffset;

		// Token: 0x0400201B RID: 8219
		private int targetEdibleCost;

		// Token: 0x0400201C RID: 8220
		[MyCmpGet]
		private Navigator navigator;

		// Token: 0x0400201D RID: 8221
		[MyCmpGet]
		private DrowningMonitor drowningMonitor;

		// Token: 0x0400201E RID: 8222
		public Diet diet;
	}
}
