using System;
using UnityEngine;

// Token: 0x020009F9 RID: 2553
public class CreaturePoopLoot : GameStateMachine<CreaturePoopLoot, CreaturePoopLoot.Instance, IStateMachineTarget, CreaturePoopLoot.Def>
{
	// Token: 0x06002EE8 RID: 12008 RVA: 0x001F681C File Offset: 0x001F4A1C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.idle;
		this.idle.EventTransition(GameHashes.Poop, this.roll, null);
		this.roll.Enter(new StateMachine<CreaturePoopLoot, CreaturePoopLoot.Instance, IStateMachineTarget, CreaturePoopLoot.Def>.State.Callback(CreaturePoopLoot.RollForLoot)).GoTo(this.idle);
	}

	// Token: 0x06002EE9 RID: 12009 RVA: 0x001F6874 File Offset: 0x001F4A74
	public static void RollForLoot(CreaturePoopLoot.Instance smi)
	{
		for (int i = 0; i < smi.def.Loot.Length; i++)
		{
			float value = UnityEngine.Random.value;
			CreaturePoopLoot.LootData lootData = smi.def.Loot[i];
			if (lootData.probability > 0f && value <= lootData.probability)
			{
				Tag tag = lootData.tag;
				Vector3 position = smi.transform.position;
				position.z = Grid.GetLayerZ(Grid.SceneLayer.Ore);
				Util.KInstantiate(Assets.GetPrefab(tag), position).SetActive(true);
			}
		}
	}

	// Token: 0x04001F91 RID: 8081
	public GameStateMachine<CreaturePoopLoot, CreaturePoopLoot.Instance, IStateMachineTarget, CreaturePoopLoot.Def>.State idle;

	// Token: 0x04001F92 RID: 8082
	public GameStateMachine<CreaturePoopLoot, CreaturePoopLoot.Instance, IStateMachineTarget, CreaturePoopLoot.Def>.State roll;

	// Token: 0x020009FA RID: 2554
	public struct LootData
	{
		// Token: 0x04001F93 RID: 8083
		public Tag tag;

		// Token: 0x04001F94 RID: 8084
		public float probability;
	}

	// Token: 0x020009FB RID: 2555
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04001F95 RID: 8085
		public CreaturePoopLoot.LootData[] Loot;
	}

	// Token: 0x020009FC RID: 2556
	public new class Instance : GameStateMachine<CreaturePoopLoot, CreaturePoopLoot.Instance, IStateMachineTarget, CreaturePoopLoot.Def>.GameInstance
	{
		// Token: 0x06002EEC RID: 12012 RVA: 0x000BE685 File Offset: 0x000BC885
		public Instance(IStateMachineTarget master, CreaturePoopLoot.Def def) : base(master, def)
		{
		}
	}
}
