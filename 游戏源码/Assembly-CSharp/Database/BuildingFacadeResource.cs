using System;
using System.Collections.Generic;
using UnityEngine;

namespace Database
{
	// Token: 0x0200211E RID: 8478
	public class BuildingFacadeResource : PermitResource
	{
		// Token: 0x0600B412 RID: 46098 RVA: 0x0043DCD4 File Offset: 0x0043BED4
		[Obsolete("Please use constructor with dlcIds parameter")]
		public BuildingFacadeResource(string Id, string Name, string Description, PermitRarity Rarity, string PrefabID, string AnimFile, Dictionary<string, string> workables = null) : this(Id, Name, Description, Rarity, PrefabID, AnimFile, DlcManager.AVAILABLE_ALL_VERSIONS, workables)
		{
		}

		// Token: 0x0600B413 RID: 46099 RVA: 0x00114BC6 File Offset: 0x00112DC6
		public BuildingFacadeResource(string Id, string Name, string Description, PermitRarity Rarity, string PrefabID, string AnimFile, string[] dlcIds, Dictionary<string, string> workables = null) : base(Id, Name, Description, PermitCategory.Building, Rarity, dlcIds)
		{
			this.Id = Id;
			this.PrefabID = PrefabID;
			this.AnimFile = AnimFile;
			this.InteractFile = workables;
		}

		// Token: 0x0600B414 RID: 46100 RVA: 0x0043DCF8 File Offset: 0x0043BEF8
		public void Init()
		{
			GameObject gameObject = Assets.TryGetPrefab(this.PrefabID);
			if (gameObject == null)
			{
				return;
			}
			gameObject.AddOrGet<BuildingFacade>();
			BuildingDef def = gameObject.GetComponent<Building>().Def;
			if (def != null)
			{
				def.AddFacade(this.Id);
			}
		}

		// Token: 0x0600B415 RID: 46101 RVA: 0x0043DD48 File Offset: 0x0043BF48
		public override PermitPresentationInfo GetPermitPresentationInfo()
		{
			PermitPresentationInfo result = default(PermitPresentationInfo);
			result.sprite = Def.GetUISpriteFromMultiObjectAnim(Assets.GetAnim(this.AnimFile), "ui", false, "");
			result.SetFacadeForPrefabID(this.PrefabID);
			return result;
		}

		// Token: 0x04008E8C RID: 36492
		public string PrefabID;

		// Token: 0x04008E8D RID: 36493
		public string AnimFile;

		// Token: 0x04008E8E RID: 36494
		public Dictionary<string, string> InteractFile;
	}
}
