using System;
using KSerialization;

// Token: 0x02001797 RID: 6039
public class RepairableEquipment : KMonoBehaviour
{
	// Token: 0x170007DD RID: 2013
	// (get) Token: 0x06007C54 RID: 31828 RVA: 0x000F1E4A File Offset: 0x000F004A
	// (set) Token: 0x06007C55 RID: 31829 RVA: 0x000F1E57 File Offset: 0x000F0057
	public EquipmentDef def
	{
		get
		{
			return this.defHandle.Get<EquipmentDef>();
		}
		set
		{
			this.defHandle.Set<EquipmentDef>(value);
		}
	}

	// Token: 0x06007C56 RID: 31830 RVA: 0x00320B70 File Offset: 0x0031ED70
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (this.def.AdditionalTags != null)
		{
			foreach (Tag tag in this.def.AdditionalTags)
			{
				base.GetComponent<KPrefabID>().AddTag(tag, false);
			}
		}
	}

	// Token: 0x06007C57 RID: 31831 RVA: 0x00320BC0 File Offset: 0x0031EDC0
	protected override void OnSpawn()
	{
		if (!this.facadeID.IsNullOrWhiteSpace())
		{
			KAnim.Build.Symbol symbol = Db.GetEquippableFacades().Get(this.facadeID).AnimFile.GetData().build.GetSymbol("object");
			SymbolOverrideController component = base.GetComponent<SymbolOverrideController>();
			component.TryRemoveSymbolOverride("object", 0);
			component.AddSymbolOverride("object", symbol, 0);
		}
	}

	// Token: 0x04005E08 RID: 24072
	public DefHandle defHandle;

	// Token: 0x04005E09 RID: 24073
	[Serialize]
	public string facadeID;
}
