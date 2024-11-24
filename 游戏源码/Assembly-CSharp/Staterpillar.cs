using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

public class Staterpillar : KMonoBehaviour
{
	public ObjectLayer conduitLayer;

	public string connectorDefId;

	private IList<Tag> dummyElement;

	private BuildingDef connectorDef;

	[Serialize]
	private Ref<KPrefabID> connectorRef = new Ref<KPrefabID>();

	private AttributeModifier wildMod = new AttributeModifier(Db.Get().Attributes.GeneratorOutput.Id, -75f, BUILDINGS.PREFABS.STATERPILLARGENERATOR.MODIFIERS.WILD);

	private ConduitDispenser cachedConduitDispenser;

	private StaterpillarGenerator cachedGenerator;

	protected override void OnPrefabInit()
	{
		dummyElement = new List<Tag> { SimHashes.Unobtanium.CreateTag() };
		connectorDef = Assets.GetBuildingDef(connectorDefId);
	}

	public void SpawnConnectorBuilding(int targetCell)
	{
		if (conduitLayer == ObjectLayer.Wire)
		{
			SpawnGenerator(targetCell);
		}
		else
		{
			SpawnConduitConnector(targetCell);
		}
	}

	public void DestroyOrphanedConnectorBuilding()
	{
		KPrefabID building = GetConnectorBuilding();
		if (!(building != null))
		{
			return;
		}
		connectorRef.Set(null);
		cachedGenerator = null;
		cachedConduitDispenser = null;
		GameScheduler.Instance.ScheduleNextFrame("Destroy Staterpillar Connector building", delegate
		{
			if (building != null)
			{
				Util.KDestroyGameObject(building.gameObject);
			}
		});
	}

	public void EnableConnector()
	{
		if (conduitLayer == ObjectLayer.Wire)
		{
			EnableGenerator();
		}
		else
		{
			EnableConduitConnector();
		}
	}

	public bool IsConnectorBuildingSpawned()
	{
		return GetConnectorBuilding() != null;
	}

	public bool IsConnected()
	{
		if (conduitLayer == ObjectLayer.Wire)
		{
			if (GetGenerator().CircuitID == ushort.MaxValue)
			{
				return false;
			}
			return true;
		}
		return GetConduitDispenser().IsConnected;
	}

	public KPrefabID GetConnectorBuilding()
	{
		return connectorRef.Get();
	}

	private void SpawnConduitConnector(int targetCell)
	{
		if (GetConduitDispenser() == null)
		{
			GameObject gameObject = connectorDef.Build(targetCell, Orientation.R180, null, dummyElement, base.gameObject.GetComponent<PrimaryElement>().Temperature);
			connectorRef = new Ref<KPrefabID>(gameObject.GetComponent<KPrefabID>());
			gameObject.SetActive(value: true);
			gameObject.GetComponent<BuildingCellVisualizer>().enabled = false;
		}
	}

	private void EnableConduitConnector()
	{
		ConduitDispenser conduitDispenser = GetConduitDispenser();
		conduitDispenser.GetComponent<BuildingCellVisualizer>().enabled = true;
		conduitDispenser.storage = GetComponent<Storage>();
		conduitDispenser.SetOnState(onState: true);
	}

	public ConduitDispenser GetConduitDispenser()
	{
		if (cachedConduitDispenser == null)
		{
			KPrefabID kPrefabID = connectorRef.Get();
			if (kPrefabID != null)
			{
				cachedConduitDispenser = kPrefabID.GetComponent<ConduitDispenser>();
			}
		}
		return cachedConduitDispenser;
	}

	private void DestroyOrphanedConduitDispenserBuilding()
	{
		ConduitDispenser dispenser = GetConduitDispenser();
		if (!(dispenser != null))
		{
			return;
		}
		connectorRef.Set(null);
		GameScheduler.Instance.ScheduleNextFrame("Destroy Staterpillar Dispenser", delegate
		{
			if (dispenser != null)
			{
				Util.KDestroyGameObject(dispenser.gameObject);
			}
		});
	}

	private void SpawnGenerator(int targetCell)
	{
		StaterpillarGenerator generator = GetGenerator();
		GameObject gameObject = null;
		if (generator != null)
		{
			gameObject = generator.gameObject;
		}
		if (!gameObject)
		{
			gameObject = connectorDef.Build(targetCell, Orientation.R180, null, dummyElement, base.gameObject.GetComponent<PrimaryElement>().Temperature);
			StaterpillarGenerator component = gameObject.GetComponent<StaterpillarGenerator>();
			component.parent = new Ref<Staterpillar>(this);
			connectorRef = new Ref<KPrefabID>(component.GetComponent<KPrefabID>());
			gameObject.SetActive(value: true);
			gameObject.GetComponent<BuildingCellVisualizer>().enabled = false;
			component.enabled = false;
		}
		Attributes attributes = gameObject.gameObject.GetAttributes();
		bool flag = base.gameObject.GetSMI<WildnessMonitor.Instance>().wildness.value > 0f;
		if (flag)
		{
			attributes.Add(wildMod);
		}
		bool flag2 = base.gameObject.GetComponent<Effects>().HasEffect("Unhappy");
		CreatureCalorieMonitor.Instance sMI = base.gameObject.GetSMI<CreatureCalorieMonitor.Instance>();
		if (sMI.IsHungry() || flag2)
		{
			float calories0to = sMI.GetCalories0to1();
			float num = 1f;
			if (calories0to <= 0f)
			{
				num = (flag ? 0.1f : 0.025f);
			}
			else if (calories0to <= 0.3f)
			{
				num = 0.5f;
			}
			else if (calories0to <= 0.5f)
			{
				num = 0.75f;
			}
			if (num < 1f)
			{
				AttributeModifier modifier = new AttributeModifier(value: 0f - ((!flag) ? ((1f - num) * 100f) : Mathf.Lerp(0f, 25f, 1f - num)), attribute_id: Db.Get().Attributes.GeneratorOutput.Id, description: BUILDINGS.PREFABS.STATERPILLARGENERATOR.MODIFIERS.HUNGRY);
				attributes.Add(modifier);
			}
		}
	}

	private void EnableGenerator()
	{
		StaterpillarGenerator generator = GetGenerator();
		generator.enabled = true;
		generator.GetComponent<BuildingCellVisualizer>().enabled = true;
	}

	public StaterpillarGenerator GetGenerator()
	{
		if (cachedGenerator == null)
		{
			KPrefabID kPrefabID = connectorRef.Get();
			if (kPrefabID != null)
			{
				cachedGenerator = kPrefabID.GetComponent<StaterpillarGenerator>();
			}
		}
		return cachedGenerator;
	}
}
