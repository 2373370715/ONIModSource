using System;
using STRINGS;
using UnityEngine;

// Token: 0x02001204 RID: 4612
public class DeathLoot : GameStateMachine<DeathLoot, DeathLoot.Instance, IStateMachineTarget, DeathLoot.Def>
{
	// Token: 0x06005E2B RID: 24107 RVA: 0x000DD88F File Offset: 0x000DBA8F
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.root;
	}

	// Token: 0x040042AD RID: 17069
	private StateMachine<DeathLoot, DeathLoot.Instance, IStateMachineTarget, DeathLoot.Def>.BoolParameter WasLoopDropped;

	// Token: 0x02001205 RID: 4613
	public class Loot
	{
		// Token: 0x1700059E RID: 1438
		// (get) Token: 0x06005E2E RID: 24110 RVA: 0x000DD8B1 File Offset: 0x000DBAB1
		// (set) Token: 0x06005E2D RID: 24109 RVA: 0x000DD8A8 File Offset: 0x000DBAA8
		public Tag Id { get; private set; } = Tag.Invalid;

		// Token: 0x1700059F RID: 1439
		// (get) Token: 0x06005E30 RID: 24112 RVA: 0x000DD8C2 File Offset: 0x000DBAC2
		// (set) Token: 0x06005E2F RID: 24111 RVA: 0x000DD8B9 File Offset: 0x000DBAB9
		public bool IsElement { get; private set; }

		// Token: 0x06005E31 RID: 24113 RVA: 0x000DD8CA File Offset: 0x000DBACA
		public Loot(Tag tag)
		{
			this.Id = tag;
			this.IsElement = false;
			this.Quantity = 1f;
		}

		// Token: 0x06005E32 RID: 24114 RVA: 0x000DD8F6 File Offset: 0x000DBAF6
		public Loot(SimHashes element, float quantity)
		{
			this.Id = element.CreateTag();
			this.IsElement = true;
			this.Quantity = quantity;
		}

		// Token: 0x040042B0 RID: 17072
		public float Quantity;
	}

	// Token: 0x02001206 RID: 4614
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x040042B1 RID: 17073
		public DeathLoot.Loot[] loot;

		// Token: 0x040042B2 RID: 17074
		public CellOffset lootSpawnOffset;
	}

	// Token: 0x02001207 RID: 4615
	public new class Instance : GameStateMachine<DeathLoot, DeathLoot.Instance, IStateMachineTarget, DeathLoot.Def>.GameInstance
	{
		// Token: 0x170005A0 RID: 1440
		// (get) Token: 0x06005E34 RID: 24116 RVA: 0x000DD923 File Offset: 0x000DBB23
		public bool WasLoopDropped
		{
			get
			{
				return base.sm.WasLoopDropped.Get(base.smi);
			}
		}

		// Token: 0x06005E35 RID: 24117 RVA: 0x000DD93B File Offset: 0x000DBB3B
		public Instance(IStateMachineTarget master, DeathLoot.Def def) : base(master, def)
		{
			base.Subscribe(1623392196, new Action<object>(this.OnDeath));
		}

		// Token: 0x06005E36 RID: 24118 RVA: 0x000DD95C File Offset: 0x000DBB5C
		private void OnDeath(object obj)
		{
			if (!this.WasLoopDropped)
			{
				base.sm.WasLoopDropped.Set(true, this, false);
				this.CreateLoot();
			}
		}

		// Token: 0x06005E37 RID: 24119 RVA: 0x002A1C5C File Offset: 0x0029FE5C
		public GameObject[] CreateLoot()
		{
			if (base.def.loot == null)
			{
				return null;
			}
			GameObject[] array = new GameObject[base.def.loot.Length];
			for (int i = 0; i < base.def.loot.Length; i++)
			{
				DeathLoot.Loot loot = base.def.loot[i];
				if (!(loot.Id == Tag.Invalid))
				{
					GameObject gameObject = Scenario.SpawnPrefab(this.GetLootSpawnCell(), 0, 0, loot.Id.ToString(), Grid.SceneLayer.Ore);
					gameObject.SetActive(true);
					Edible component = gameObject.GetComponent<Edible>();
					if (component)
					{
						ReportManager.Instance.ReportValue(ReportManager.ReportType.CaloriesCreated, component.Calories, StringFormatter.Replace(UI.ENDOFDAYREPORT.NOTES.BUTCHERED, "{0}", gameObject.GetProperName()), UI.ENDOFDAYREPORT.NOTES.BUTCHERED_CONTEXT);
					}
					if (loot.IsElement)
					{
						PrimaryElement component2 = gameObject.GetComponent<PrimaryElement>();
						if (component2 != null)
						{
							component2.Mass = loot.Quantity;
						}
					}
					array[i] = gameObject;
				}
			}
			return array;
		}

		// Token: 0x06005E38 RID: 24120 RVA: 0x002A1D6C File Offset: 0x0029FF6C
		public int GetLootSpawnCell()
		{
			int num = Grid.PosToCell(base.gameObject);
			int num2 = Grid.OffsetCell(num, base.def.lootSpawnOffset);
			if (Grid.IsWorldValidCell(num2) && Grid.IsValidCellInWorld(num2, base.gameObject.GetMyWorldId()))
			{
				return num2;
			}
			return num;
		}

		// Token: 0x06005E39 RID: 24121 RVA: 0x000DD981 File Offset: 0x000DBB81
		protected override void OnCleanUp()
		{
			base.Unsubscribe(1623392196, new Action<object>(this.OnDeath));
		}
	}
}
