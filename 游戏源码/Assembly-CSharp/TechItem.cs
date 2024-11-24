using System;
using UnityEngine;

// Token: 0x020017AD RID: 6061
public class TechItem : Resource
{
	// Token: 0x06007CCD RID: 31949 RVA: 0x000F2254 File Offset: 0x000F0454
	[Obsolete("Use constructor with requiredDlcIds and forbiddenDlcIds")]
	public TechItem(string id, ResourceSet parent, string name, string description, Func<string, bool, Sprite> getUISprite, string parentTechId, string[] dlcIds, bool isPOIUnlock = false) : base(id, parent, name)
	{
		this.description = description;
		this.getUISprite = getUISprite;
		this.parentTechId = parentTechId;
		this.isPOIUnlock = isPOIUnlock;
		DlcManager.ConvertAvailableToRequireAndForbidden(dlcIds, out this.requiredDlcIds, out this.forbiddenDlcIds);
	}

	// Token: 0x06007CCE RID: 31950 RVA: 0x000F2292 File Offset: 0x000F0492
	public TechItem(string id, ResourceSet parent, string name, string description, Func<string, bool, Sprite> getUISprite, string parentTechId, string[] requiredDlcIds = null, string[] forbiddenDlcIds = null, bool isPOIUnlock = false) : base(id, parent, name)
	{
		this.description = description;
		this.getUISprite = getUISprite;
		this.parentTechId = parentTechId;
		this.isPOIUnlock = isPOIUnlock;
		this.requiredDlcIds = requiredDlcIds;
		this.forbiddenDlcIds = forbiddenDlcIds;
	}

	// Token: 0x170007EC RID: 2028
	// (get) Token: 0x06007CCF RID: 31951 RVA: 0x000F22CD File Offset: 0x000F04CD
	public Tech ParentTech
	{
		get
		{
			return Db.Get().Techs.Get(this.parentTechId);
		}
	}

	// Token: 0x06007CD0 RID: 31952 RVA: 0x000F22E4 File Offset: 0x000F04E4
	public Sprite UISprite()
	{
		return this.getUISprite("ui", false);
	}

	// Token: 0x06007CD1 RID: 31953 RVA: 0x000F22F7 File Offset: 0x000F04F7
	public bool IsComplete()
	{
		return this.ParentTech.IsComplete() || this.IsPOIUnlocked();
	}

	// Token: 0x06007CD2 RID: 31954 RVA: 0x00322FC4 File Offset: 0x003211C4
	private bool IsPOIUnlocked()
	{
		if (this.isPOIUnlock)
		{
			TechInstance techInstance = Research.Instance.Get(this.ParentTech);
			if (techInstance != null)
			{
				return techInstance.UnlockedPOITechIds.Contains(this.Id);
			}
		}
		return false;
	}

	// Token: 0x06007CD3 RID: 31955 RVA: 0x00323000 File Offset: 0x00321200
	public void POIUnlocked()
	{
		DebugUtil.DevAssert(this.isPOIUnlock, "Trying to unlock tech item " + this.Id + " via POI and it's not marked as POI unlockable.", null);
		if (this.isPOIUnlock && !this.IsComplete())
		{
			Research.Instance.Get(this.ParentTech).UnlockPOITech(this.Id);
		}
	}

	// Token: 0x04005E76 RID: 24182
	public string description;

	// Token: 0x04005E77 RID: 24183
	public Func<string, bool, Sprite> getUISprite;

	// Token: 0x04005E78 RID: 24184
	public string parentTechId;

	// Token: 0x04005E79 RID: 24185
	public bool isPOIUnlock;

	// Token: 0x04005E7A RID: 24186
	[Obsolete("Use required/forbidden instead")]
	public string[] dlcIds;

	// Token: 0x04005E7B RID: 24187
	public string[] requiredDlcIds;

	// Token: 0x04005E7C RID: 24188
	public string[] forbiddenDlcIds;
}
