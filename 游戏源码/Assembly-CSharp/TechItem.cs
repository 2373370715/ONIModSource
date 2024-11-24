using System;
using UnityEngine;

public class TechItem : Resource
{
	public string description;

	public Func<string, bool, Sprite> getUISprite;

	public string parentTechId;

	public string[] dlcIds;

	public bool isPOIUnlock;

	public Tech ParentTech => Db.Get().Techs.Get(parentTechId);

	public TechItem(string id, ResourceSet parent, string name, string description, Func<string, bool, Sprite> getUISprite, string parentTechId, string[] dlcIds, bool isPOIUnlock = false)
		: base(id, parent, name)
	{
		this.description = description;
		this.getUISprite = getUISprite;
		this.parentTechId = parentTechId;
		this.dlcIds = dlcIds;
		this.isPOIUnlock = isPOIUnlock;
	}

	public Sprite UISprite()
	{
		return getUISprite("ui", arg2: false);
	}

	public bool IsComplete()
	{
		if (!ParentTech.IsComplete())
		{
			return IsPOIUnlocked();
		}
		return true;
	}

	private bool IsPOIUnlocked()
	{
		if (isPOIUnlock)
		{
			TechInstance techInstance = Research.Instance.Get(ParentTech);
			if (techInstance != null)
			{
				return techInstance.UnlockedPOITechIds.Contains(Id);
			}
		}
		return false;
	}

	public void POIUnlocked()
	{
		DebugUtil.DevAssert(isPOIUnlock, "Trying to unlock tech item " + Id + " via POI and it's not marked as POI unlockable.");
		if (isPOIUnlock && !IsComplete())
		{
			Research.Instance.Get(ParentTech).UnlockPOITech(Id);
		}
	}
}
