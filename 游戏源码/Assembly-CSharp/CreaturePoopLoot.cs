using UnityEngine;

public class CreaturePoopLoot : GameStateMachine<CreaturePoopLoot, CreaturePoopLoot.Instance, IStateMachineTarget, CreaturePoopLoot.Def>
{
	public struct LootData
	{
		public Tag tag;

		public float probability;
	}

	public class Def : BaseDef
	{
		public LootData[] Loot;
	}

	public new class Instance : GameInstance
	{
		public Instance(IStateMachineTarget master, Def def)
			: base(master, def)
		{
		}
	}

	public State idle;

	public State roll;

	public override void InitializeStates(out BaseState default_state)
	{
		base.serializable = SerializeType.ParamsOnly;
		default_state = idle;
		idle.EventTransition(GameHashes.Poop, roll);
		roll.Enter(RollForLoot).GoTo(idle);
	}

	public static void RollForLoot(Instance smi)
	{
		for (int i = 0; i < smi.def.Loot.Length; i++)
		{
			float value = Random.value;
			LootData lootData = smi.def.Loot[i];
			if (lootData.probability > 0f && value <= lootData.probability)
			{
				Tag tag = lootData.tag;
				Vector3 position = smi.transform.position;
				position.z = Grid.GetLayerZ(Grid.SceneLayer.Ore);
				Util.KInstantiate(Assets.GetPrefab(tag), position).SetActive(value: true);
			}
		}
	}
}
