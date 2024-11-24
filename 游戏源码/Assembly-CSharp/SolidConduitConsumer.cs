using System;
using UnityEngine;

// Token: 0x02001893 RID: 6291
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/SolidConduitConsumer")]
public class SolidConduitConsumer : KMonoBehaviour, IConduitConsumer
{
	// Token: 0x1700084F RID: 2127
	// (get) Token: 0x0600822D RID: 33325 RVA: 0x000F5A06 File Offset: 0x000F3C06
	public Storage Storage
	{
		get
		{
			return this.storage;
		}
	}

	// Token: 0x17000850 RID: 2128
	// (get) Token: 0x0600822E RID: 33326 RVA: 0x000AD365 File Offset: 0x000AB565
	public ConduitType ConduitType
	{
		get
		{
			return ConduitType.Solid;
		}
	}

	// Token: 0x17000851 RID: 2129
	// (get) Token: 0x0600822F RID: 33327 RVA: 0x000F5A0E File Offset: 0x000F3C0E
	public bool IsConsuming
	{
		get
		{
			return this.consuming;
		}
	}

	// Token: 0x17000852 RID: 2130
	// (get) Token: 0x06008230 RID: 33328 RVA: 0x0033B7D4 File Offset: 0x003399D4
	public bool IsConnected
	{
		get
		{
			GameObject gameObject = Grid.Objects[this.utilityCell, 20];
			return gameObject != null && gameObject.GetComponent<BuildingComplete>() != null;
		}
	}

	// Token: 0x06008231 RID: 33329 RVA: 0x000D48B5 File Offset: 0x000D2AB5
	private SolidConduitFlow GetConduitFlow()
	{
		return Game.Instance.solidConduitFlow;
	}

	// Token: 0x06008232 RID: 33330 RVA: 0x0033B80C File Offset: 0x00339A0C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.utilityCell = this.GetInputCell();
		ScenePartitionerLayer layer = GameScenePartitioner.Instance.objectLayers[20];
		this.partitionerEntry = GameScenePartitioner.Instance.Add("SolidConduitConsumer.OnSpawn", base.gameObject, this.utilityCell, layer, new Action<object>(this.OnConduitConnectionChanged));
		this.GetConduitFlow().AddConduitUpdater(new Action<float>(this.ConduitUpdate), ConduitFlowPriority.Default);
		this.OnConduitConnectionChanged(null);
	}

	// Token: 0x06008233 RID: 33331 RVA: 0x000F5A16 File Offset: 0x000F3C16
	protected override void OnCleanUp()
	{
		this.GetConduitFlow().RemoveConduitUpdater(new Action<float>(this.ConduitUpdate));
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		base.OnCleanUp();
	}

	// Token: 0x06008234 RID: 33332 RVA: 0x000F5A45 File Offset: 0x000F3C45
	private void OnConduitConnectionChanged(object data)
	{
		this.consuming = (this.consuming && this.IsConnected);
		base.Trigger(-2094018600, this.IsConnected);
	}

	// Token: 0x06008235 RID: 33333 RVA: 0x0033B888 File Offset: 0x00339A88
	private void ConduitUpdate(float dt)
	{
		bool flag = false;
		SolidConduitFlow conduitFlow = this.GetConduitFlow();
		if (this.IsConnected)
		{
			SolidConduitFlow.ConduitContents contents = conduitFlow.GetContents(this.utilityCell);
			if (contents.pickupableHandle.IsValid() && (this.alwaysConsume || this.operational.IsOperational))
			{
				float num = (this.capacityTag != GameTags.Any) ? this.storage.GetMassAvailable(this.capacityTag) : this.storage.MassStored();
				float num2 = Mathf.Min(this.storage.capacityKg, this.capacityKG);
				float num3 = Mathf.Max(0f, num2 - num);
				if (num3 > 0f)
				{
					Pickupable pickupable = conduitFlow.GetPickupable(contents.pickupableHandle);
					if (pickupable.PrimaryElement.Mass <= num3 || pickupable.PrimaryElement.Mass > num2)
					{
						Pickupable pickupable2 = conduitFlow.RemovePickupable(this.utilityCell);
						if (pickupable2)
						{
							this.storage.Store(pickupable2.gameObject, true, false, true, false);
							flag = true;
						}
					}
				}
			}
		}
		if (this.storage != null)
		{
			this.storage.storageNetworkID = this.GetConnectedNetworkID();
		}
		this.consuming = flag;
	}

	// Token: 0x06008236 RID: 33334 RVA: 0x0033B9C8 File Offset: 0x00339BC8
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

	// Token: 0x06008237 RID: 33335 RVA: 0x0033BA1C File Offset: 0x00339C1C
	private int GetInputCell()
	{
		if (this.useSecondaryInput)
		{
			foreach (ISecondaryInput secondaryInput in base.GetComponents<ISecondaryInput>())
			{
				if (secondaryInput.HasSecondaryConduitType(ConduitType.Solid))
				{
					return Grid.OffsetCell(this.building.NaturalBuildingCell(), secondaryInput.GetSecondaryConduitOffset(ConduitType.Solid));
				}
			}
			return Grid.OffsetCell(this.building.NaturalBuildingCell(), CellOffset.none);
		}
		return this.building.GetUtilityInputCell();
	}

	// Token: 0x040062C8 RID: 25288
	[SerializeField]
	public Tag capacityTag = GameTags.Any;

	// Token: 0x040062C9 RID: 25289
	[SerializeField]
	public float capacityKG = float.PositiveInfinity;

	// Token: 0x040062CA RID: 25290
	[SerializeField]
	public bool alwaysConsume;

	// Token: 0x040062CB RID: 25291
	[SerializeField]
	public bool useSecondaryInput;

	// Token: 0x040062CC RID: 25292
	[MyCmpReq]
	private Operational operational;

	// Token: 0x040062CD RID: 25293
	[MyCmpReq]
	private Building building;

	// Token: 0x040062CE RID: 25294
	[MyCmpGet]
	public Storage storage;

	// Token: 0x040062CF RID: 25295
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x040062D0 RID: 25296
	private int utilityCell = -1;

	// Token: 0x040062D1 RID: 25297
	private bool consuming;
}
