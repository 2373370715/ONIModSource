using System;
using KSerialization;

// Token: 0x02000B2A RID: 2858
[SerializationConfig(MemberSerialization.OptIn)]
public abstract class SubstanceSource : KMonoBehaviour
{
	// Token: 0x0600365A RID: 13914 RVA: 0x000C35C4 File Offset: 0x000C17C4
	protected override void OnPrefabInit()
	{
		this.pickupable.SetWorkTime(SubstanceSource.MaxPickupTime);
	}

	// Token: 0x0600365B RID: 13915 RVA: 0x000C35D6 File Offset: 0x000C17D6
	protected override void OnSpawn()
	{
		this.pickupable.SetWorkTime(10f);
	}

	// Token: 0x0600365C RID: 13916
	protected abstract CellOffset[] GetOffsetGroup();

	// Token: 0x0600365D RID: 13917
	protected abstract IChunkManager GetChunkManager();

	// Token: 0x0600365E RID: 13918 RVA: 0x000C35E8 File Offset: 0x000C17E8
	public SimHashes GetElementID()
	{
		return this.primaryElement.ElementID;
	}

	// Token: 0x0600365F RID: 13919 RVA: 0x002134A8 File Offset: 0x002116A8
	public Tag GetElementTag()
	{
		Tag result = Tag.Invalid;
		if (base.gameObject != null && this.primaryElement != null && this.primaryElement.Element != null)
		{
			result = this.primaryElement.Element.tag;
		}
		return result;
	}

	// Token: 0x06003660 RID: 13920 RVA: 0x002134F8 File Offset: 0x002116F8
	public Tag GetMaterialCategoryTag()
	{
		Tag result = Tag.Invalid;
		if (base.gameObject != null && this.primaryElement != null && this.primaryElement.Element != null)
		{
			result = this.primaryElement.Element.GetMaterialCategoryTag();
		}
		return result;
	}

	// Token: 0x040024FF RID: 9471
	private bool enableRefresh;

	// Token: 0x04002500 RID: 9472
	private static readonly float MaxPickupTime = 8f;

	// Token: 0x04002501 RID: 9473
	[MyCmpReq]
	public Pickupable pickupable;

	// Token: 0x04002502 RID: 9474
	[MyCmpReq]
	private PrimaryElement primaryElement;
}
