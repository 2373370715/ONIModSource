using System;
using UnityEngine;

// Token: 0x0200105C RID: 4188
public class CargoDropperStorage : GameStateMachine<CargoDropperStorage, CargoDropperStorage.StatesInstance, IStateMachineTarget, CargoDropperStorage.Def>
{
	// Token: 0x0600556E RID: 21870 RVA: 0x000D7C1E File Offset: 0x000D5E1E
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.EventHandler(GameHashes.JettisonCargo, delegate(CargoDropperStorage.StatesInstance smi, object data)
		{
			smi.JettisonCargo(data);
		});
	}

	// Token: 0x0200105D RID: 4189
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04003BF0 RID: 15344
		public Vector3 dropOffset;
	}

	// Token: 0x0200105E RID: 4190
	public class StatesInstance : GameStateMachine<CargoDropperStorage, CargoDropperStorage.StatesInstance, IStateMachineTarget, CargoDropperStorage.Def>.GameInstance
	{
		// Token: 0x06005571 RID: 21873 RVA: 0x000D7C60 File Offset: 0x000D5E60
		public StatesInstance(IStateMachineTarget master, CargoDropperStorage.Def def) : base(master, def)
		{
		}

		// Token: 0x06005572 RID: 21874 RVA: 0x0027E330 File Offset: 0x0027C530
		public void JettisonCargo(object data)
		{
			Vector3 position = base.master.transform.GetPosition() + base.def.dropOffset;
			Storage component = base.GetComponent<Storage>();
			if (component != null)
			{
				GameObject gameObject = component.FindFirst("ScoutRover");
				if (gameObject != null)
				{
					component.Drop(gameObject, true);
					Vector3 position2 = base.master.transform.GetPosition();
					position2.z = Grid.GetLayerZ(Grid.SceneLayer.Creatures);
					gameObject.transform.SetPosition(position2);
					ChoreProvider component2 = gameObject.GetComponent<ChoreProvider>();
					if (component2 != null)
					{
						KBatchedAnimController component3 = gameObject.GetComponent<KBatchedAnimController>();
						if (component3 != null)
						{
							component3.Play("enter", KAnim.PlayMode.Once, 1f, 0f);
						}
						new EmoteChore(component2, Db.Get().ChoreTypes.EmoteHighPriority, null, new HashedString[]
						{
							"enter"
						}, KAnim.PlayMode.Once, false);
					}
					gameObject.GetMyWorld().SetRoverLanded();
				}
				component.DropAll(position, false, false, default(Vector3), true, null);
			}
		}
	}
}
