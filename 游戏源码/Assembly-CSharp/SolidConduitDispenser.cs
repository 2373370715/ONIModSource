using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02001894 RID: 6292
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/SolidConduitDispenser")]
public class SolidConduitDispenser : KMonoBehaviour, ISaveLoadable, IConduitDispenser
{
	// Token: 0x17000853 RID: 2131
	// (get) Token: 0x06008239 RID: 33337 RVA: 0x000F5A99 File Offset: 0x000F3C99
	public Storage Storage
	{
		get
		{
			return this.storage;
		}
	}

	// Token: 0x17000854 RID: 2132
	// (get) Token: 0x0600823A RID: 33338 RVA: 0x000AD365 File Offset: 0x000AB565
	public ConduitType ConduitType
	{
		get
		{
			return ConduitType.Solid;
		}
	}

	// Token: 0x17000855 RID: 2133
	// (get) Token: 0x0600823B RID: 33339 RVA: 0x000F5AA1 File Offset: 0x000F3CA1
	public SolidConduitFlow.ConduitContents ConduitContents
	{
		get
		{
			return this.GetConduitFlow().GetContents(this.utilityCell);
		}
	}

	// Token: 0x17000856 RID: 2134
	// (get) Token: 0x0600823C RID: 33340 RVA: 0x000F5AB4 File Offset: 0x000F3CB4
	public bool IsDispensing
	{
		get
		{
			return this.dispensing;
		}
	}

	// Token: 0x0600823D RID: 33341 RVA: 0x000D48B5 File Offset: 0x000D2AB5
	public SolidConduitFlow GetConduitFlow()
	{
		return Game.Instance.solidConduitFlow;
	}

	// Token: 0x0600823E RID: 33342 RVA: 0x0033BA8C File Offset: 0x00339C8C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.utilityCell = this.GetOutputCell();
		ScenePartitionerLayer layer = GameScenePartitioner.Instance.objectLayers[20];
		this.partitionerEntry = GameScenePartitioner.Instance.Add("SolidConduitConsumer.OnSpawn", base.gameObject, this.utilityCell, layer, new Action<object>(this.OnConduitConnectionChanged));
		this.GetConduitFlow().AddConduitUpdater(new Action<float>(this.ConduitUpdate), ConduitFlowPriority.Dispense);
		this.OnConduitConnectionChanged(null);
	}

	// Token: 0x0600823F RID: 33343 RVA: 0x000F5ABC File Offset: 0x000F3CBC
	protected override void OnCleanUp()
	{
		this.GetConduitFlow().RemoveConduitUpdater(new Action<float>(this.ConduitUpdate));
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		base.OnCleanUp();
	}

	// Token: 0x06008240 RID: 33344 RVA: 0x000F5AEB File Offset: 0x000F3CEB
	private void OnConduitConnectionChanged(object data)
	{
		this.dispensing = (this.dispensing && this.IsConnected);
		base.Trigger(-2094018600, this.IsConnected);
	}

	// Token: 0x06008241 RID: 33345 RVA: 0x0033BB08 File Offset: 0x00339D08
	private void ConduitUpdate(float dt)
	{
		bool flag = false;
		this.operational.SetFlag(SolidConduitDispenser.outputConduitFlag, this.IsConnected);
		if (this.operational.IsOperational || this.alwaysDispense)
		{
			SolidConduitFlow conduitFlow = this.GetConduitFlow();
			if (conduitFlow.HasConduit(this.utilityCell) && conduitFlow.IsConduitEmpty(this.utilityCell))
			{
				Pickupable pickupable = this.FindSuitableItem();
				if (pickupable)
				{
					if (pickupable.PrimaryElement.Mass > 20f)
					{
						pickupable = pickupable.Take(20f);
					}
					conduitFlow.AddPickupable(this.utilityCell, pickupable);
					flag = true;
				}
			}
		}
		this.storage.storageNetworkID = this.GetConnectedNetworkID();
		this.dispensing = flag;
	}

	// Token: 0x06008242 RID: 33346 RVA: 0x0033BBBC File Offset: 0x00339DBC
	private bool isSolid(GameObject o)
	{
		PrimaryElement component = o.GetComponent<PrimaryElement>();
		return component == null || component.Element.IsLiquid || component.Element.IsGas;
	}

	// Token: 0x06008243 RID: 33347 RVA: 0x0033BBF4 File Offset: 0x00339DF4
	private Pickupable FindSuitableItem()
	{
		List<GameObject> list = this.storage.items;
		if (this.solidOnly)
		{
			List<GameObject> list2 = new List<GameObject>(list);
			list2.RemoveAll(new Predicate<GameObject>(this.isSolid));
			list = list2;
		}
		if (list.Count < 1)
		{
			return null;
		}
		this.round_robin_index %= list.Count;
		GameObject gameObject = list[this.round_robin_index];
		this.round_robin_index++;
		if (!gameObject)
		{
			return null;
		}
		return gameObject.GetComponent<Pickupable>();
	}

	// Token: 0x17000857 RID: 2135
	// (get) Token: 0x06008244 RID: 33348 RVA: 0x0033BC78 File Offset: 0x00339E78
	public bool IsConnected
	{
		get
		{
			GameObject gameObject = Grid.Objects[this.utilityCell, 20];
			return gameObject != null && gameObject.GetComponent<BuildingComplete>() != null;
		}
	}

	// Token: 0x06008245 RID: 33349 RVA: 0x0033BCB0 File Offset: 0x00339EB0
	private int GetConnectedNetworkID()
	{
		GameObject gameObject = Grid.Objects[this.utilityCell, 20];
		SolidConduit solidConduit = (gameObject != null) ? gameObject.GetComponent<SolidConduit>() : null;
		UtilityNetwork utilityNetwork = (solidConduit != null) ? solidConduit.GetNetwork() : null;
		if (utilityNetwork == null)
		{
			return -1;
		}
		return utilityNetwork.id;
	}

	// Token: 0x06008246 RID: 33350 RVA: 0x0033BD04 File Offset: 0x00339F04
	private int GetOutputCell()
	{
		Building component = base.GetComponent<Building>();
		if (this.useSecondaryOutput)
		{
			foreach (ISecondaryOutput secondaryOutput in base.GetComponents<ISecondaryOutput>())
			{
				if (secondaryOutput.HasSecondaryConduitType(ConduitType.Solid))
				{
					return Grid.OffsetCell(component.NaturalBuildingCell(), secondaryOutput.GetSecondaryConduitOffset(ConduitType.Solid));
				}
			}
			return Grid.OffsetCell(component.NaturalBuildingCell(), CellOffset.none);
		}
		return component.GetUtilityOutputCell();
	}

	// Token: 0x040062D2 RID: 25298
	[SerializeField]
	public SimHashes[] elementFilter;

	// Token: 0x040062D3 RID: 25299
	[SerializeField]
	public bool invertElementFilter;

	// Token: 0x040062D4 RID: 25300
	[SerializeField]
	public bool alwaysDispense;

	// Token: 0x040062D5 RID: 25301
	[SerializeField]
	public bool useSecondaryOutput;

	// Token: 0x040062D6 RID: 25302
	[SerializeField]
	public bool solidOnly;

	// Token: 0x040062D7 RID: 25303
	private static readonly Operational.Flag outputConduitFlag = new Operational.Flag("output_conduit", Operational.Flag.Type.Functional);

	// Token: 0x040062D8 RID: 25304
	[MyCmpReq]
	private Operational operational;

	// Token: 0x040062D9 RID: 25305
	[MyCmpReq]
	public Storage storage;

	// Token: 0x040062DA RID: 25306
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x040062DB RID: 25307
	private int utilityCell = -1;

	// Token: 0x040062DC RID: 25308
	private bool dispensing;

	// Token: 0x040062DD RID: 25309
	private int round_robin_index;
}
