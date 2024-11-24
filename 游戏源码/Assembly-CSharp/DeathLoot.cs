using STRINGS;
using UnityEngine;

public class DeathLoot : GameStateMachine<DeathLoot, DeathLoot.Instance, IStateMachineTarget, DeathLoot.Def>
{
	public class Loot
	{
		public float Quantity;

		public Tag Id { get; private set; } = Tag.Invalid;


		public bool IsElement { get; private set; }

		public Loot(Tag tag)
		{
			Id = tag;
			IsElement = false;
			Quantity = 1f;
		}

		public Loot(SimHashes element, float quantity)
		{
			Id = element.CreateTag();
			IsElement = true;
			Quantity = quantity;
		}
	}

	public class Def : BaseDef
	{
		public Loot[] loot;

		public CellOffset lootSpawnOffset;
	}

	public new class Instance : GameInstance
	{
		public bool WasLoopDropped => base.sm.WasLoopDropped.Get(base.smi);

		public Instance(IStateMachineTarget master, Def def)
			: base(master, def)
		{
			Subscribe(1623392196, OnDeath);
		}

		private void OnDeath(object obj)
		{
			if (!WasLoopDropped)
			{
				base.sm.WasLoopDropped.Set(value: true, this);
				CreateLoot();
			}
		}

		public GameObject[] CreateLoot()
		{
			if (base.def.loot == null)
			{
				return null;
			}
			GameObject[] array = new GameObject[base.def.loot.Length];
			for (int i = 0; i < base.def.loot.Length; i++)
			{
				Loot loot = base.def.loot[i];
				if (loot.Id == Tag.Invalid)
				{
					continue;
				}
				GameObject gameObject = Scenario.SpawnPrefab(GetLootSpawnCell(), 0, 0, loot.Id.ToString());
				gameObject.SetActive(value: true);
				Edible component = gameObject.GetComponent<Edible>();
				if ((bool)component)
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
			return array;
		}

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

		protected override void OnCleanUp()
		{
			Unsubscribe(1623392196, OnDeath);
		}
	}

	private BoolParameter WasLoopDropped;

	public override void InitializeStates(out BaseState default_state)
	{
		base.serializable = SerializeType.ParamsOnly;
		default_state = root;
	}
}
