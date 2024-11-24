using System.Diagnostics;
using Klei.AI;
using UnityEngine;

public class SolidConsumerMonitor : GameStateMachine<SolidConsumerMonitor, SolidConsumerMonitor.Instance, IStateMachineTarget, SolidConsumerMonitor.Def>
{
	public class Def : BaseDef
	{
		public Diet diet;
	}

	public new class Instance : GameInstance
	{
		private const int RECALC_THRESHOLD = 4;

		public GameObject targetEdible;

		public Vector3 targetEdibleOffset;

		private int targetEdibleCost;

		[MyCmpGet]
		private Navigator navigator;

		[MyCmpGet]
		private DrowningMonitor drowningMonitor;

		public Diet diet;

		public Instance(IStateMachineTarget master, Def def)
			: base(master, def)
		{
			diet = DietManager.Instance.GetPrefabDiet(base.gameObject);
		}

		public bool CanSearchForPickupables(bool foodAtFeeder)
		{
			return !foodAtFeeder;
		}

		public bool IsCloserThanTargetEdible(int cost)
		{
			if (cost != -1)
			{
				if (cost >= targetEdibleCost)
				{
					return targetEdibleCost == -1;
				}
				return true;
			}
			return false;
		}

		public bool IsTargetEdibleValid()
		{
			if (targetEdible == null || targetEdible.HasTag(GameTags.Creatures.ReservedByCreature))
			{
				return false;
			}
			int cost = GetCost(Grid.PosToCell(targetEdible.transform.GetPosition() + targetEdibleOffset));
			if (cost == -1 || targetEdibleCost > cost + 4)
			{
				return false;
			}
			return true;
		}

		public void ClearTargetEdible()
		{
			targetEdibleCost = -1;
			targetEdible = null;
			targetEdibleOffset = Vector3.zero;
		}

		public bool ProcessEdible(GameObject edible, out bool isReachable)
		{
			int cost = GetCost(edible);
			isReachable = cost != -1;
			if (cost != -1 && (cost < targetEdibleCost || targetEdibleCost == -1))
			{
				targetEdibleCost = cost;
				targetEdible = edible.gameObject;
				return true;
			}
			return false;
		}

		public void SetTargetEdible(GameObject gameObject, int cost)
		{
			targetEdibleCost = cost;
			targetEdible = gameObject;
		}

		public int GetCost(GameObject edible)
		{
			return GetCost(Grid.PosToCell(edible.transform.GetPosition()));
		}

		public int GetCost(int cell)
		{
			if (drowningMonitor != null && drowningMonitor.canDrownToDeath && !drowningMonitor.livesUnderWater && !drowningMonitor.IsCellSafe(cell))
			{
				return -1;
			}
			return navigator.GetNavigationCost(cell);
		}

		public void OnEatSolidComplete(object data)
		{
			KPrefabID kPrefabID = data as KPrefabID;
			if (kPrefabID == null)
			{
				return;
			}
			PrimaryElement component = kPrefabID.GetComponent<PrimaryElement>();
			if (component == null)
			{
				return;
			}
			Diet.Info dietInfo = diet.GetDietInfo(kPrefabID.PrefabTag);
			if (dietInfo == null)
			{
				return;
			}
			AmountInstance amountInstance = Db.Get().Amounts.Calories.Lookup(base.smi.gameObject);
			string properName = kPrefabID.GetProperName();
			PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Negative, properName, kPrefabID.transform);
			float calories = amountInstance.GetMax() - amountInstance.value;
			float num = dietInfo.ConvertCaloriesToConsumptionMass(calories);
			IPlantConsumptionInstructions component2 = kPrefabID.GetComponent<IPlantConsumptionInstructions>();
			component2 = ((component2 != null) ? component2 : kPrefabID.GetSMI<IPlantConsumptionInstructions>());
			if (component2 != null)
			{
				num = component2.ConsumePlant(num);
			}
			else
			{
				num = Mathf.Min(num, component.Mass);
				component.Mass -= num;
				Pickupable component3 = component.GetComponent<Pickupable>();
				if (component3.storage != null)
				{
					component3.storage.Trigger(-1452790913, base.gameObject);
					component3.storage.Trigger(-1697596308, base.gameObject);
				}
			}
			float calories2 = dietInfo.ConvertConsumptionMassToCalories(num);
			CreatureCalorieMonitor.CaloriesConsumedEvent caloriesConsumedEvent = default(CreatureCalorieMonitor.CaloriesConsumedEvent);
			caloriesConsumedEvent.tag = kPrefabID.PrefabTag;
			caloriesConsumedEvent.calories = calories2;
			CreatureCalorieMonitor.CaloriesConsumedEvent caloriesConsumedEvent2 = caloriesConsumedEvent;
			Trigger(-2038961714, caloriesConsumedEvent2);
			targetEdible = null;
		}
	}

	public static Vector3 PLANT_ON_FLOOR_VESSEL_OFFSET = Vector3.down;

	private State satisfied;

	private State lookingforfood;

	private static Tag[] creatureTags = new Tag[2]
	{
		GameTags.Creatures.ReservedByCreature,
		GameTags.CreatureBrain
	};

	public override void InitializeStates(out BaseState default_state)
	{
		default_state = satisfied;
		root.EventHandler(GameHashes.EatSolidComplete, delegate(Instance smi, object data)
		{
			smi.OnEatSolidComplete(data);
		}).ToggleBehaviour(GameTags.Creatures.WantsToEat, (Instance smi) => smi.targetEdible != null && !smi.targetEdible.HasTag(GameTags.Creatures.ReservedByCreature));
		satisfied.TagTransition(GameTags.Creatures.Hungry, lookingforfood);
		lookingforfood.TagTransition(GameTags.Creatures.Hungry, satisfied, on_remove: true).PreBrainUpdate(FindFood);
	}

	[Conditional("DETAILED_SOLID_CONSUMER_MONITOR_PROFILE")]
	private static void BeginDetailedSample(string region_name)
	{
	}

	[Conditional("DETAILED_SOLID_CONSUMER_MONITOR_PROFILE")]
	private static void EndDetailedSample(string region_name)
	{
	}

	private static void FindFood(Instance smi)
	{
		if (smi.IsTargetEdibleValid())
		{
			return;
		}
		smi.ClearTargetEdible();
		Diet diet = smi.diet;
		int x = 0;
		int y = 0;
		Grid.PosToXY(smi.gameObject.transform.GetPosition(), out x, out y);
		x -= 8;
		y -= 8;
		bool flag = false;
		if (!diet.AllConsumablesAreDirectlyEdiblePlants)
		{
			ListPool<Storage, SolidConsumerMonitor>.PooledList pooledList = ListPool<Storage, SolidConsumerMonitor>.Allocate();
			int num = 32;
			foreach (CreatureFeeder item in Components.CreatureFeeders.GetItems(smi.GetMyWorldId()))
			{
				Vector2I targetFeederCell = item.GetTargetFeederCell();
				if (targetFeederCell.x < x || targetFeederCell.x > x + num || targetFeederCell.y < y || targetFeederCell.y > y + num || item.StoragesAreEmpty())
				{
					continue;
				}
				int cost = smi.GetCost(Grid.XYToCell(targetFeederCell.x, targetFeederCell.y));
				if (!smi.IsCloserThanTargetEdible(cost))
				{
					continue;
				}
				Storage[] storages = item.storages;
				foreach (Storage storage in storages)
				{
					if (storage == null || storage.IsEmpty() || smi.GetCost(Grid.PosToCell(storage.items[0])) == -1)
					{
						continue;
					}
					foreach (GameObject item2 in storage.items)
					{
						if (!(item2 == null))
						{
							KPrefabID component = item2.GetComponent<KPrefabID>();
							if (!component.HasAnyTags(creatureTags) && diet.GetDietInfo(component.PrefabTag) != null)
							{
								smi.SetTargetEdible(item2, cost);
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
			pooledList.Recycle();
		}
		bool flag2 = false;
		if (diet.CanEatAnyPlantDirectly)
		{
			ListPool<ScenePartitionerEntry, GameScenePartitioner>.PooledList pooledList2 = ListPool<ScenePartitionerEntry, GameScenePartitioner>.Allocate();
			GameScenePartitioner.Instance.GatherEntries(x, y, 16, 16, GameScenePartitioner.Instance.plants, pooledList2);
			foreach (ScenePartitionerEntry item3 in pooledList2)
			{
				KPrefabID kPrefabID = (KPrefabID)item3.obj;
				Vector3 position = kPrefabID.transform.GetPosition();
				bool flag3 = kPrefabID.HasTag(GameTags.PlantedOnFloorVessel);
				if (flag3)
				{
					position += PLANT_ON_FLOOR_VESSEL_OFFSET;
				}
				int num2 = smi.GetCost(Grid.PosToCell(position));
				Vector3 vector = Vector3.zero;
				if (!smi.IsCloserThanTargetEdible(num2) || kPrefabID.HasAnyTags(creatureTags) || diet.GetDietInfo(kPrefabID.PrefabTag) == null)
				{
					continue;
				}
				if (kPrefabID.HasTag(GameTags.Plant))
				{
					IPlantConsumptionInstructions component2 = kPrefabID.GetComponent<IPlantConsumptionInstructions>();
					component2 = ((component2 != null) ? component2 : kPrefabID.GetSMI<IPlantConsumptionInstructions>());
					if (component2 == null || !component2.CanPlantBeEaten())
					{
						continue;
					}
					CellOffset[] allowedOffsets = component2.GetAllowedOffsets();
					if (allowedOffsets != null)
					{
						num2 = -1;
						CellOffset[] array = allowedOffsets;
						for (int i = 0; i < array.Length; i++)
						{
							CellOffset offset = array[i];
							int cost2 = smi.GetCost(Grid.OffsetCell(Grid.PosToCell(position), offset));
							if (cost2 != -1 && (num2 == -1 || cost2 < num2))
							{
								num2 = cost2;
								vector = offset.ToVector3();
							}
						}
						if (num2 == -1)
						{
							continue;
						}
					}
				}
				smi.SetTargetEdible(kPrefabID.gameObject, num2);
				smi.targetEdibleOffset = vector + (flag3 ? PLANT_ON_FLOOR_VESSEL_OFFSET : Vector3.zero);
				flag2 = true;
			}
			pooledList2.Recycle();
		}
		if (flag2 || !diet.CanEatAnyNonDirectlyEdiblePlant || !smi.CanSearchForPickupables(flag))
		{
			return;
		}
		bool flag4 = false;
		ListPool<ScenePartitionerEntry, GameScenePartitioner>.PooledList pooledList3 = ListPool<ScenePartitionerEntry, GameScenePartitioner>.Allocate();
		GameScenePartitioner.Instance.GatherEntries(x, y, 16, 16, GameScenePartitioner.Instance.pickupablesLayer, pooledList3);
		foreach (ScenePartitionerEntry item4 in pooledList3)
		{
			Pickupable pickupable = (Pickupable)item4.obj;
			KPrefabID kPrefabID2 = pickupable.KPrefabID;
			if (!kPrefabID2.HasAnyTags(creatureTags) && diet.GetDietInfo(kPrefabID2.PrefabTag) != null)
			{
				smi.ProcessEdible(pickupable.gameObject, out var isReachable);
				smi.targetEdibleOffset = Vector3.zero;
				flag4 = flag4 || isReachable;
			}
		}
		pooledList3.Recycle();
	}
}
