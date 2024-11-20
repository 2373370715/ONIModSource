using System;
using System.Collections.Generic;
using UnityEngine;

namespace Database
{
	public class BuildingFacadeResource : PermitResource
	{
		[Obsolete("Please use constructor with dlcIds parameter")]
		public BuildingFacadeResource(string Id, string Name, string Description, PermitRarity Rarity, string PrefabID, string AnimFile, Dictionary<string, string> workables = null) : this(Id, Name, Description, Rarity, PrefabID, AnimFile, DlcManager.AVAILABLE_ALL_VERSIONS, workables)
		{
		}

		public BuildingFacadeResource(string Id, string Name, string Description, PermitRarity Rarity, string PrefabID, string AnimFile, string[] dlcIds, Dictionary<string, string> workables = null) : base(Id, Name, Description, PermitCategory.Building, Rarity, dlcIds)
		{
			this.Id = Id;
			this.PrefabID = PrefabID;
			this.AnimFile = AnimFile;
			this.InteractFile = workables;
		}

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

		public override PermitPresentationInfo GetPermitPresentationInfo()
		{
			PermitPresentationInfo result = default(PermitPresentationInfo);
			result.sprite = Def.GetUISpriteFromMultiObjectAnim(Assets.GetAnim(this.AnimFile), "ui", false, "");
			result.SetFacadeForPrefabID(this.PrefabID);
			return result;
		}

		public string PrefabID;

		public string AnimFile;

		public Dictionary<string, string> InteractFile;
	}
}
