using System;
using UnityEngine;

// Token: 0x02001413 RID: 5139
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/Insulator")]
public class Insulator : KMonoBehaviour
{
	// Token: 0x060069E3 RID: 27107 RVA: 0x000E5774 File Offset: 0x000E3974
	protected override void OnSpawn()
	{
		SimMessages.SetInsulation(Grid.OffsetCell(Grid.PosToCell(base.transform.GetPosition()), this.offset), this.building.Def.ThermalConductivity);
	}

	// Token: 0x060069E4 RID: 27108 RVA: 0x000E57A6 File Offset: 0x000E39A6
	protected override void OnCleanUp()
	{
		SimMessages.SetInsulation(Grid.OffsetCell(Grid.PosToCell(base.transform.GetPosition()), this.offset), 1f);
	}

	// Token: 0x04004FFD RID: 20477
	[MyCmpReq]
	private Building building;

	// Token: 0x04004FFE RID: 20478
	[SerializeField]
	public CellOffset offset = CellOffset.none;
}
