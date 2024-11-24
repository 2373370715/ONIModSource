using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x020009D8 RID: 2520
public class ClimbableTreeMonitor : GameStateMachine<ClimbableTreeMonitor, ClimbableTreeMonitor.Instance, IStateMachineTarget, ClimbableTreeMonitor.Def>
{
	// Token: 0x06002E52 RID: 11858 RVA: 0x001F46C0 File Offset: 0x001F28C0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.ToggleBehaviour(GameTags.Creatures.WantsToClimbTree, (ClimbableTreeMonitor.Instance smi) => smi.UpdateHasClimbable(), delegate(ClimbableTreeMonitor.Instance smi)
		{
			smi.OnClimbComplete();
		});
	}

	// Token: 0x04001F23 RID: 7971
	private const int MAX_NAV_COST = 2147483647;

	// Token: 0x020009D9 RID: 2521
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04001F24 RID: 7972
		public float searchMinInterval = 60f;

		// Token: 0x04001F25 RID: 7973
		public float searchMaxInterval = 120f;
	}

	// Token: 0x020009DA RID: 2522
	public new class Instance : GameStateMachine<ClimbableTreeMonitor, ClimbableTreeMonitor.Instance, IStateMachineTarget, ClimbableTreeMonitor.Def>.GameInstance
	{
		// Token: 0x06002E55 RID: 11861 RVA: 0x000BE06B File Offset: 0x000BC26B
		public Instance(IStateMachineTarget master, ClimbableTreeMonitor.Def def) : base(master, def)
		{
			this.RefreshSearchTime();
		}

		// Token: 0x06002E56 RID: 11862 RVA: 0x000BE07B File Offset: 0x000BC27B
		private void RefreshSearchTime()
		{
			this.nextSearchTime = Time.time + Mathf.Lerp(base.def.searchMinInterval, base.def.searchMaxInterval, UnityEngine.Random.value);
		}

		// Token: 0x06002E57 RID: 11863 RVA: 0x000BE0A9 File Offset: 0x000BC2A9
		public bool UpdateHasClimbable()
		{
			if (this.climbTarget == null)
			{
				if (Time.time < this.nextSearchTime)
				{
					return false;
				}
				this.FindClimbableTree();
				this.RefreshSearchTime();
			}
			return this.climbTarget != null;
		}

		// Token: 0x06002E58 RID: 11864 RVA: 0x001F4724 File Offset: 0x001F2924
		private void FindClimbableTree()
		{
			this.climbTarget = null;
			ListPool<KMonoBehaviour, ClimbableTreeMonitor>.PooledList pooledList = ListPool<KMonoBehaviour, ClimbableTreeMonitor>.Allocate();
			Vector3 position = base.master.transform.GetPosition();
			Extents extents = new Extents(Grid.PosToCell(position), 10);
			Navigator component = base.GetComponent<Navigator>();
			IEnumerable<object> first = GameScenePartitioner.Instance.AsyncSafeEnumerate(extents.x, extents.y, extents.width, extents.height, GameScenePartitioner.Instance.plants);
			IEnumerable<object> second = GameScenePartitioner.Instance.AsyncSafeEnumerate(extents.x, extents.y, extents.width, extents.height, GameScenePartitioner.Instance.completeBuildings);
			foreach (object obj in first.Concat(second))
			{
				KMonoBehaviour kmonoBehaviour = obj as KMonoBehaviour;
				if (!kmonoBehaviour.HasTag(GameTags.Creatures.ReservedByCreature))
				{
					int cell = Grid.PosToCell(kmonoBehaviour);
					if (component.CanReach(cell))
					{
						ForestTreeSeedMonitor component2 = kmonoBehaviour.GetComponent<ForestTreeSeedMonitor>();
						StorageLocker component3 = kmonoBehaviour.GetComponent<StorageLocker>();
						if (component2 != null)
						{
							if (!component2.ExtraSeedAvailable)
							{
								continue;
							}
						}
						else
						{
							if (!(component3 != null))
							{
								continue;
							}
							Storage component4 = component3.GetComponent<Storage>();
							if (!component4.allowItemRemoval || component4.IsEmpty())
							{
								continue;
							}
						}
						pooledList.Add(kmonoBehaviour);
					}
				}
			}
			if (pooledList.Count > 0)
			{
				int index = UnityEngine.Random.Range(0, pooledList.Count);
				KMonoBehaviour kmonoBehaviour2 = pooledList[index];
				this.climbTarget = kmonoBehaviour2.gameObject;
			}
			pooledList.Recycle();
		}

		// Token: 0x06002E59 RID: 11865 RVA: 0x000BE0E0 File Offset: 0x000BC2E0
		public void OnClimbComplete()
		{
			this.climbTarget = null;
		}

		// Token: 0x04001F26 RID: 7974
		public GameObject climbTarget;

		// Token: 0x04001F27 RID: 7975
		public float nextSearchTime;
	}
}
