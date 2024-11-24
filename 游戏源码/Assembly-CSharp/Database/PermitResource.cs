namespace Database;

public abstract class PermitResource : Resource
{
	public string Description;

	public PermitCategory Category;

	public PermitRarity Rarity;

	public string[] DlcIds;

	public PermitResource(string id, string Name, string Desc, PermitCategory permitCategory, PermitRarity rarity, string[] DLCIds)
		: base(id, Name)
	{
		DebugUtil.DevAssert(Name != null, "Name must be provided for permit with id \"" + id + "\" of type " + GetType().Name);
		DebugUtil.DevAssert(Desc != null, "Description must be provided for permit with id \"" + id + "\" of type " + GetType().Name);
		Description = Desc;
		Category = permitCategory;
		Rarity = rarity;
		DlcIds = DLCIds;
	}

	public abstract PermitPresentationInfo GetPermitPresentationInfo();

	public bool IsOwnableOnServer()
	{
		if (Rarity != PermitRarity.Universal)
		{
			return Rarity != PermitRarity.UniversalLocked;
		}
		return false;
	}

	public bool IsUnlocked()
	{
		if (Rarity != PermitRarity.Universal)
		{
			return PermitItems.IsPermitUnlocked(this);
		}
		return true;
	}

	public string GetDlcIdFrom()
	{
		if (DlcIds == DlcManager.AVAILABLE_ALL_VERSIONS || DlcIds == DlcManager.AVAILABLE_VANILLA_ONLY)
		{
			return null;
		}
		if (DlcIds.Length == 0)
		{
			return null;
		}
		return DlcIds[0];
	}
}
