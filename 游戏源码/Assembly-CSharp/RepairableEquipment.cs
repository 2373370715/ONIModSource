using KSerialization;

public class RepairableEquipment : KMonoBehaviour
{
	public DefHandle defHandle;

	[Serialize]
	public string facadeID;

	public EquipmentDef def
	{
		get
		{
			return defHandle.Get<EquipmentDef>();
		}
		set
		{
			defHandle.Set(value);
		}
	}

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (def.AdditionalTags != null)
		{
			Tag[] additionalTags = def.AdditionalTags;
			foreach (Tag tag in additionalTags)
			{
				GetComponent<KPrefabID>().AddTag(tag);
			}
		}
	}

	protected override void OnSpawn()
	{
		if (!facadeID.IsNullOrWhiteSpace())
		{
			KAnim.Build.Symbol symbol = Db.GetEquippableFacades().Get(facadeID).AnimFile.GetData().build.GetSymbol("object");
			SymbolOverrideController component = GetComponent<SymbolOverrideController>();
			component.TryRemoveSymbolOverride("object");
			component.AddSymbolOverride("object", symbol);
		}
	}
}
