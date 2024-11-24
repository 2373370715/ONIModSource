using System;
using UnityEngine;

// Token: 0x02000125 RID: 293
public class BeeMakeHiveStates : GameStateMachine<BeeMakeHiveStates, BeeMakeHiveStates.Instance, IStateMachineTarget, BeeMakeHiveStates.Def>
{
	// Token: 0x06000461 RID: 1121 RVA: 0x0015680C File Offset: 0x00154A0C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.findBuildLocation;
		this.root.DoNothing();
		this.findBuildLocation.Enter(delegate(BeeMakeHiveStates.Instance smi)
		{
			this.FindBuildLocation(smi);
			if (smi.targetBuildCell != Grid.InvalidCell)
			{
				smi.GoTo(this.moveToBuildLocation);
				return;
			}
			smi.GoTo(this.behaviourcomplete);
		});
		this.moveToBuildLocation.MoveTo((BeeMakeHiveStates.Instance smi) => smi.targetBuildCell, this.doBuild, this.behaviourcomplete, false);
		this.doBuild.PlayAnim("hive_grow_pre").EventHandler(GameHashes.AnimQueueComplete, delegate(BeeMakeHiveStates.Instance smi)
		{
			if (smi.gameObject.GetComponent<Bee>().FindHiveInRoom() == null)
			{
				smi.builtHome = true;
				smi.BuildHome();
			}
			smi.GoTo(this.behaviourcomplete);
		});
		this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.WantsToMakeHome, false).Exit(delegate(BeeMakeHiveStates.Instance smi)
		{
			if (smi.builtHome)
			{
				Util.KDestroyGameObject(smi.master.gameObject);
			}
		});
	}

	// Token: 0x06000462 RID: 1122 RVA: 0x001568DC File Offset: 0x00154ADC
	private void FindBuildLocation(BeeMakeHiveStates.Instance smi)
	{
		smi.targetBuildCell = Grid.InvalidCell;
		GameObject prefab = Assets.GetPrefab("BeeHive".ToTag());
		BuildingPlacementQuery buildingPlacementQuery = PathFinderQueries.buildingPlacementQuery.Reset(1, prefab);
		smi.GetComponent<Navigator>().RunQuery(buildingPlacementQuery);
		if (buildingPlacementQuery.result_cells.Count > 0)
		{
			smi.targetBuildCell = buildingPlacementQuery.result_cells[UnityEngine.Random.Range(0, buildingPlacementQuery.result_cells.Count)];
		}
	}

	// Token: 0x0400032B RID: 811
	public GameStateMachine<BeeMakeHiveStates, BeeMakeHiveStates.Instance, IStateMachineTarget, BeeMakeHiveStates.Def>.State findBuildLocation;

	// Token: 0x0400032C RID: 812
	public GameStateMachine<BeeMakeHiveStates, BeeMakeHiveStates.Instance, IStateMachineTarget, BeeMakeHiveStates.Def>.State moveToBuildLocation;

	// Token: 0x0400032D RID: 813
	public GameStateMachine<BeeMakeHiveStates, BeeMakeHiveStates.Instance, IStateMachineTarget, BeeMakeHiveStates.Def>.State doBuild;

	// Token: 0x0400032E RID: 814
	public GameStateMachine<BeeMakeHiveStates, BeeMakeHiveStates.Instance, IStateMachineTarget, BeeMakeHiveStates.Def>.State behaviourcomplete;

	// Token: 0x02000126 RID: 294
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02000127 RID: 295
	public new class Instance : GameStateMachine<BeeMakeHiveStates, BeeMakeHiveStates.Instance, IStateMachineTarget, BeeMakeHiveStates.Def>.GameInstance
	{
		// Token: 0x06000467 RID: 1127 RVA: 0x000A77EE File Offset: 0x000A59EE
		public Instance(Chore<BeeMakeHiveStates.Instance> chore, BeeMakeHiveStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.WantsToMakeHome);
		}

		// Token: 0x06000468 RID: 1128 RVA: 0x00156950 File Offset: 0x00154B50
		public void BuildHome()
		{
			Vector3 position = Grid.CellToPos(this.targetBuildCell, CellAlignment.Bottom, Grid.SceneLayer.Creatures);
			GameObject gameObject = Util.KInstantiate(Assets.GetPrefab("BeeHive".ToTag()), position, Quaternion.identity, null, null, true, 0);
			PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
			component.ElementID = SimHashes.Creature;
			component.Temperature = base.gameObject.GetComponent<PrimaryElement>().Temperature;
			gameObject.SetActive(true);
			gameObject.GetSMI<BeeHive.StatesInstance>().SetUpNewHive();
		}

		// Token: 0x0400032F RID: 815
		public int targetBuildCell;

		// Token: 0x04000330 RID: 816
		public bool builtHome;
	}
}
