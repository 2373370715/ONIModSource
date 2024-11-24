using System;
using KSerialization;

// Token: 0x02000F0B RID: 3851
[SerializationConfig(MemberSerialization.OptIn)]
public class PartialLightBlocking : KMonoBehaviour
{
	// Token: 0x06004DAD RID: 19885 RVA: 0x000D272D File Offset: 0x000D092D
	protected override void OnSpawn()
	{
		this.SetLightBlocking();
		base.OnSpawn();
	}

	// Token: 0x06004DAE RID: 19886 RVA: 0x000D273B File Offset: 0x000D093B
	protected override void OnCleanUp()
	{
		this.ClearLightBlocking();
		base.OnCleanUp();
	}

	// Token: 0x06004DAF RID: 19887 RVA: 0x00265914 File Offset: 0x00263B14
	public void SetLightBlocking()
	{
		int[] placementCells = base.GetComponent<Building>().PlacementCells;
		for (int i = 0; i < placementCells.Length; i++)
		{
			SimMessages.SetCellProperties(placementCells[i], 48);
		}
	}

	// Token: 0x06004DB0 RID: 19888 RVA: 0x00265948 File Offset: 0x00263B48
	public void ClearLightBlocking()
	{
		int[] placementCells = base.GetComponent<Building>().PlacementCells;
		for (int i = 0; i < placementCells.Length; i++)
		{
			SimMessages.ClearCellProperties(placementCells[i], 48);
		}
	}

	// Token: 0x040035F6 RID: 13814
	private const byte PartialLightBlockingProperties = 48;
}
