using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x020010D9 RID: 4313
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/ConduitDispenser")]
public class ConduitDispenser : KMonoBehaviour, ISaveLoadable, IConduitDispenser
{
	// Token: 0x1700054F RID: 1359
	// (get) Token: 0x0600588D RID: 22669 RVA: 0x000D9BD3 File Offset: 0x000D7DD3
	public Storage Storage
	{
		get
		{
			return this.storage;
		}
	}

	// Token: 0x17000550 RID: 1360
	// (get) Token: 0x0600588E RID: 22670 RVA: 0x000D9BDB File Offset: 0x000D7DDB
	public ConduitType ConduitType
	{
		get
		{
			return this.conduitType;
		}
	}

	// Token: 0x17000551 RID: 1361
	// (get) Token: 0x0600588F RID: 22671 RVA: 0x000D9BE3 File Offset: 0x000D7DE3
	public ConduitFlow.ConduitContents ConduitContents
	{
		get
		{
			return this.GetConduitManager().GetContents(this.utilityCell);
		}
	}

	// Token: 0x06005890 RID: 22672 RVA: 0x000D9BF6 File Offset: 0x000D7DF6
	public void SetConduitData(ConduitType type)
	{
		this.conduitType = type;
	}

	// Token: 0x06005891 RID: 22673 RVA: 0x0028C514 File Offset: 0x0028A714
	public ConduitFlow GetConduitManager()
	{
		ConduitType conduitType = this.conduitType;
		if (conduitType == ConduitType.Gas)
		{
			return Game.Instance.gasConduitFlow;
		}
		if (conduitType != ConduitType.Liquid)
		{
			return null;
		}
		return Game.Instance.liquidConduitFlow;
	}

	// Token: 0x06005892 RID: 22674 RVA: 0x000D9BFF File Offset: 0x000D7DFF
	private void OnConduitConnectionChanged(object data)
	{
		base.Trigger(-2094018600, this.IsConnected);
	}

	// Token: 0x06005893 RID: 22675 RVA: 0x0028C54C File Offset: 0x0028A74C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		GameScheduler.Instance.Schedule("PlumbingTutorial", 2f, delegate(object obj)
		{
			Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Plumbing, true);
		}, null, null);
		ConduitFlow conduitManager = this.GetConduitManager();
		this.utilityCell = this.GetOutputCell(conduitManager.conduitType);
		ScenePartitionerLayer layer = GameScenePartitioner.Instance.objectLayers[(this.conduitType == ConduitType.Gas) ? 12 : 16];
		this.partitionerEntry = GameScenePartitioner.Instance.Add("ConduitConsumer.OnSpawn", base.gameObject, this.utilityCell, layer, new Action<object>(this.OnConduitConnectionChanged));
		this.GetConduitManager().AddConduitUpdater(new Action<float>(this.ConduitUpdate), ConduitFlowPriority.Dispense);
		this.OnConduitConnectionChanged(null);
	}

	// Token: 0x06005894 RID: 22676 RVA: 0x000D9C17 File Offset: 0x000D7E17
	protected override void OnCleanUp()
	{
		this.GetConduitManager().RemoveConduitUpdater(new Action<float>(this.ConduitUpdate));
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		base.OnCleanUp();
	}

	// Token: 0x06005895 RID: 22677 RVA: 0x000D9C46 File Offset: 0x000D7E46
	public void SetOnState(bool onState)
	{
		this.isOn = onState;
	}

	// Token: 0x06005896 RID: 22678 RVA: 0x000D9C4F File Offset: 0x000D7E4F
	private void ConduitUpdate(float dt)
	{
		if (this.operational != null)
		{
			this.operational.SetFlag(ConduitDispenser.outputConduitFlag, this.IsConnected);
		}
		this.blocked = false;
		if (this.isOn)
		{
			this.Dispense(dt);
		}
	}

	// Token: 0x06005897 RID: 22679 RVA: 0x0028C618 File Offset: 0x0028A818
	private void Dispense(float dt)
	{
		if ((this.operational != null && this.operational.IsOperational) || this.alwaysDispense)
		{
			if (this.building != null && this.building.Def.CanMove)
			{
				this.utilityCell = this.GetOutputCell(this.GetConduitManager().conduitType);
			}
			PrimaryElement primaryElement = this.FindSuitableElement();
			if (primaryElement != null)
			{
				primaryElement.KeepZeroMassObject = true;
				this.empty = false;
				float num = this.GetConduitManager().AddElement(this.utilityCell, primaryElement.ElementID, primaryElement.Mass, primaryElement.Temperature, primaryElement.DiseaseIdx, primaryElement.DiseaseCount);
				if (num > 0f)
				{
					int num2 = (int)(num / primaryElement.Mass * (float)primaryElement.DiseaseCount);
					primaryElement.ModifyDiseaseCount(-num2, "ConduitDispenser.ConduitUpdate");
					primaryElement.Mass -= num;
					this.storage.Trigger(-1697596308, primaryElement.gameObject);
					return;
				}
				this.blocked = true;
				return;
			}
			else
			{
				this.empty = true;
			}
		}
	}

	// Token: 0x06005898 RID: 22680 RVA: 0x0028C730 File Offset: 0x0028A930
	private PrimaryElement FindSuitableElement()
	{
		List<GameObject> items = this.storage.items;
		int count = items.Count;
		for (int i = 0; i < count; i++)
		{
			int index = (i + this.elementOutputOffset) % count;
			PrimaryElement component = items[index].GetComponent<PrimaryElement>();
			if (component != null && component.Mass > 0f && ((this.conduitType == ConduitType.Liquid) ? component.Element.IsLiquid : component.Element.IsGas) && (this.elementFilter == null || this.elementFilter.Length == 0 || (!this.invertElementFilter && this.IsFilteredElement(component.ElementID)) || (this.invertElementFilter && !this.IsFilteredElement(component.ElementID))))
			{
				this.elementOutputOffset = (this.elementOutputOffset + 1) % count;
				return component;
			}
		}
		return null;
	}

	// Token: 0x06005899 RID: 22681 RVA: 0x0028C810 File Offset: 0x0028AA10
	private bool IsFilteredElement(SimHashes element)
	{
		for (int num = 0; num != this.elementFilter.Length; num++)
		{
			if (this.elementFilter[num] == element)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x17000552 RID: 1362
	// (get) Token: 0x0600589A RID: 22682 RVA: 0x0028C840 File Offset: 0x0028AA40
	public bool IsConnected
	{
		get
		{
			GameObject gameObject = Grid.Objects[this.utilityCell, (this.conduitType == ConduitType.Gas) ? 12 : 16];
			return gameObject != null && gameObject.GetComponent<BuildingComplete>() != null;
		}
	}

	// Token: 0x0600589B RID: 22683 RVA: 0x0028C884 File Offset: 0x0028AA84
	private int GetOutputCell(ConduitType outputConduitType)
	{
		Building component = base.GetComponent<Building>();
		if (!(component != null))
		{
			return Grid.OffsetCell(Grid.PosToCell(this), this.noBuildingOutputCellOffset);
		}
		if (this.useSecondaryOutput)
		{
			ISecondaryOutput[] components = base.GetComponents<ISecondaryOutput>();
			foreach (ISecondaryOutput secondaryOutput in components)
			{
				if (secondaryOutput.HasSecondaryConduitType(outputConduitType))
				{
					return Grid.OffsetCell(component.NaturalBuildingCell(), secondaryOutput.GetSecondaryConduitOffset(outputConduitType));
				}
			}
			return Grid.OffsetCell(component.NaturalBuildingCell(), components[0].GetSecondaryConduitOffset(outputConduitType));
		}
		return component.GetUtilityOutputCell();
	}

	// Token: 0x04003E6F RID: 15983
	[SerializeField]
	public ConduitType conduitType;

	// Token: 0x04003E70 RID: 15984
	[SerializeField]
	public SimHashes[] elementFilter;

	// Token: 0x04003E71 RID: 15985
	[SerializeField]
	public bool invertElementFilter;

	// Token: 0x04003E72 RID: 15986
	[SerializeField]
	public bool alwaysDispense;

	// Token: 0x04003E73 RID: 15987
	[SerializeField]
	public bool isOn = true;

	// Token: 0x04003E74 RID: 15988
	[SerializeField]
	public bool blocked;

	// Token: 0x04003E75 RID: 15989
	[SerializeField]
	public bool empty = true;

	// Token: 0x04003E76 RID: 15990
	[SerializeField]
	public bool useSecondaryOutput;

	// Token: 0x04003E77 RID: 15991
	[SerializeField]
	public CellOffset noBuildingOutputCellOffset;

	// Token: 0x04003E78 RID: 15992
	private static readonly Operational.Flag outputConduitFlag = new Operational.Flag("output_conduit", Operational.Flag.Type.Functional);

	// Token: 0x04003E79 RID: 15993
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04003E7A RID: 15994
	[MyCmpReq]
	public Storage storage;

	// Token: 0x04003E7B RID: 15995
	[MyCmpGet]
	private Building building;

	// Token: 0x04003E7C RID: 15996
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x04003E7D RID: 15997
	private int utilityCell = -1;

	// Token: 0x04003E7E RID: 15998
	private int elementOutputOffset;
}
