using System;
using UnityEngine;

internal class BeckonFromSpaceStates : GameStateMachine<BeckonFromSpaceStates, BeckonFromSpaceStates.Instance, IStateMachineTarget, BeckonFromSpaceStates.Def>
{
	public class Def : BaseDef
	{
		public string prefab;

		public Grid.SceneLayer sceneLayer;

		public HashedString[] choirAnims = new HashedString[1] { "reply_loop" };
	}

	public new class Instance : GameInstance
	{
		public Instance(Chore<Instance> chore, Def def)
			: base((IStateMachineTarget)chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.WantsToBeckon);
		}
	}

	public class BeckoningState : State
	{
		public State pre;

		public State loop;

		public State pst;
	}

	public BeckoningState beckoning;

	public State behaviourcomplete;

	public override void InitializeStates(out BaseState default_state)
	{
		default_state = beckoning;
		beckoning.ToggleMainStatusItem(Db.Get().CreatureStatusItems.Beckoning).DefaultState(beckoning.pre);
		beckoning.pre.PlayAnim("beckoning_pre").OnAnimQueueComplete(beckoning.loop);
		beckoning.loop.PlayAnim("beckoning_loop").Enter(MooEchoFX).OnAnimQueueComplete(beckoning.pst);
		beckoning.pst.PlayAnim("beckoning_pst").OnAnimQueueComplete(behaviourcomplete);
		behaviourcomplete.PlayAnim("idle_loop", KAnim.PlayMode.Loop).Enter(DoBeckon).Enter(MooCheer)
			.BehaviourComplete(GameTags.Creatures.WantsToBeckon);
	}

	private static void MooEchoFX(Instance smi)
	{
		KBatchedAnimController kBatchedAnimController = FXHelpers.CreateEffect("moo_call_fx_kanim", smi.master.transform.position);
		kBatchedAnimController.destroyOnAnimComplete = true;
		kBatchedAnimController.Play("moo_call");
	}

	private static void MooCheer(Instance smi)
	{
		Vector3 position = smi.transform.GetPosition();
		ListPool<ScenePartitionerEntry, BeckonFromSpaceStates>.PooledList pooledList = ListPool<ScenePartitionerEntry, BeckonFromSpaceStates>.Allocate();
		Extents extents = new Extents((int)position.x, (int)position.y, 15);
		GameScenePartitioner.Instance.GatherEntries(extents, GameScenePartitioner.Instance.pickupablesLayer, pooledList);
		foreach (ScenePartitionerEntry item in pooledList)
		{
			KPrefabID kPrefabID = (item.obj as Pickupable).KPrefabID;
			if (!(kPrefabID.gameObject == smi.gameObject) && kPrefabID.HasTag("Moo") && kPrefabID.GetSMI<AnimInterruptMonitor.Instance>() != null)
			{
				kPrefabID.GetSMI<AnimInterruptMonitor.Instance>().PlayAnimSequence(smi.def.choirAnims);
			}
		}
		pooledList.Recycle();
	}

	private static void DoBeckon(Instance smi)
	{
		Db.Get().Amounts.Beckoning.Lookup(smi.gameObject).value = 0f;
		WorldContainer myWorld = smi.GetMyWorld();
		Vector3 position = smi.transform.position;
		float num = myWorld.Height + myWorld.WorldOffset.y - 1;
		float layerZ = Grid.GetLayerZ(smi.def.sceneLayer);
		float num2 = (num - position.y) * Mathf.Tan((float)Math.PI / 12f);
		float num3 = position.x + (float)UnityEngine.Random.Range(-5, 5);
		float num4 = num3 - num2;
		float num5 = num3 + num2;
		float num6 = position.x;
		bool customInitialFlip = false;
		if (num4 > (float)myWorld.WorldOffset.x && num4 < (float)(myWorld.WorldOffset.x + myWorld.Width))
		{
			num6 = num4;
			customInitialFlip = false;
		}
		else if (num4 > (float)myWorld.WorldOffset.x && num4 < (float)(myWorld.WorldOffset.x + myWorld.Width))
		{
			num6 = num5;
			customInitialFlip = true;
		}
		DebugUtil.DevAssert(myWorld.ContainsPoint(new Vector2(num6, num)), "Gassy Moo spawned outside world bounds");
		GameObject obj = Util.KInstantiate(position: new Vector3(num6, num, layerZ), original: Assets.GetPrefab(smi.def.prefab), rotation: Quaternion.identity);
		GassyMooComet component = obj.GetComponent<GassyMooComet>();
		if (component != null)
		{
			component.spawnWithOffset = true;
			if (num6 != position.x)
			{
				component.SetCustomInitialFlip(customInitialFlip);
			}
		}
		obj.SetActive(value: true);
	}
}
