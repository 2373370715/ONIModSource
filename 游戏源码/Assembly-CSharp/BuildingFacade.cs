using System;
using System.Collections.Generic;
using Database;
using KSerialization;
using UnityEngine;

// Token: 0x02000CB7 RID: 3255
[SerializationConfig(MemberSerialization.OptIn)]
public class BuildingFacade : KMonoBehaviour
{
	// Token: 0x170002FE RID: 766
	// (get) Token: 0x06003EFF RID: 16127 RVA: 0x000C8F03 File Offset: 0x000C7103
	public string CurrentFacade
	{
		get
		{
			return this.currentFacade;
		}
	}

	// Token: 0x170002FF RID: 767
	// (get) Token: 0x06003F00 RID: 16128 RVA: 0x000C8F0B File Offset: 0x000C710B
	public bool IsOriginal
	{
		get
		{
			return this.currentFacade.IsNullOrWhiteSpace();
		}
	}

	// Token: 0x06003F01 RID: 16129 RVA: 0x000A5E40 File Offset: 0x000A4040
	protected override void OnPrefabInit()
	{
	}

	// Token: 0x06003F02 RID: 16130 RVA: 0x000C8F18 File Offset: 0x000C7118
	protected override void OnSpawn()
	{
		if (!this.IsOriginal)
		{
			this.ApplyBuildingFacade(Db.GetBuildingFacades().TryGet(this.currentFacade), false);
		}
	}

	// Token: 0x06003F03 RID: 16131 RVA: 0x000C8F39 File Offset: 0x000C7139
	public void ApplyDefaultFacade(bool shouldTryAnimate = false)
	{
		this.currentFacade = "DEFAULT_FACADE";
		this.ClearFacade(shouldTryAnimate);
	}

	// Token: 0x06003F04 RID: 16132 RVA: 0x00235F0C File Offset: 0x0023410C
	public void ApplyBuildingFacade(BuildingFacadeResource facade, bool shouldTryAnimate = false)
	{
		if (facade == null)
		{
			this.ClearFacade(false);
			return;
		}
		this.currentFacade = facade.Id;
		KAnimFile[] array = new KAnimFile[]
		{
			Assets.GetAnim(facade.AnimFile)
		};
		this.ChangeBuilding(array, facade.Name, facade.Description, facade.InteractFile, shouldTryAnimate);
	}

	// Token: 0x06003F05 RID: 16133 RVA: 0x00235F64 File Offset: 0x00234164
	private void ClearFacade(bool shouldTryAnimate = false)
	{
		Building component = base.GetComponent<Building>();
		this.ChangeBuilding(component.Def.AnimFiles, component.Def.Name, component.Def.Desc, null, shouldTryAnimate);
	}

	// Token: 0x06003F06 RID: 16134 RVA: 0x00235FA4 File Offset: 0x002341A4
	private void ChangeBuilding(KAnimFile[] animFiles, string displayName, string desc, Dictionary<string, string> interactAnimsNames = null, bool shouldTryAnimate = false)
	{
		this.interactAnims.Clear();
		if (interactAnimsNames != null && interactAnimsNames.Count > 0)
		{
			this.interactAnims = new Dictionary<string, KAnimFile[]>();
			foreach (KeyValuePair<string, string> keyValuePair in interactAnimsNames)
			{
				this.interactAnims.Add(keyValuePair.Key, new KAnimFile[]
				{
					Assets.GetAnim(keyValuePair.Value)
				});
			}
		}
		Building[] components = base.GetComponents<Building>();
		foreach (Building building in components)
		{
			building.SetDescriptionFlavour(desc);
			KBatchedAnimController component = building.GetComponent<KBatchedAnimController>();
			HashedString batchGroupID = component.batchGroupID;
			component.SwapAnims(animFiles);
			foreach (KBatchedAnimController kbatchedAnimController in building.GetComponentsInChildren<KBatchedAnimController>(true))
			{
				if (kbatchedAnimController.batchGroupID == batchGroupID)
				{
					kbatchedAnimController.SwapAnims(animFiles);
				}
			}
			if (!this.animateIn.IsNullOrDestroyed())
			{
				UnityEngine.Object.Destroy(this.animateIn);
				this.animateIn = null;
			}
			if (shouldTryAnimate)
			{
				this.animateIn = BuildingFacadeAnimateIn.MakeFor(component);
				string parameter = "Unlocked";
				float parameterValue = 1f;
				KFMOD.PlayUISoundWithParameter(GlobalAssets.GetSound(KleiInventoryScreen.GetFacadeItemSoundName(Db.Get().Permits.TryGet(this.currentFacade)) + "_Click", false), parameter, parameterValue);
			}
		}
		base.GetComponent<KSelectable>().SetName(displayName);
		if (base.GetComponent<AnimTileable>() != null && components.Length != 0)
		{
			GameScenePartitioner.Instance.TriggerEvent(components[0].GetExtents(), GameScenePartitioner.Instance.objectLayers[1], null);
		}
	}

	// Token: 0x06003F07 RID: 16135 RVA: 0x0023616C File Offset: 0x0023436C
	public string GetNextFacade()
	{
		BuildingDef def = base.GetComponent<Building>().Def;
		int num = def.AvailableFacades.FindIndex((string s) => s == this.currentFacade) + 1;
		if (num >= def.AvailableFacades.Count)
		{
			num = 0;
		}
		return def.AvailableFacades[num];
	}

	// Token: 0x04002AF7 RID: 10999
	[Serialize]
	private string currentFacade;

	// Token: 0x04002AF8 RID: 11000
	public KAnimFile[] animFiles;

	// Token: 0x04002AF9 RID: 11001
	public Dictionary<string, KAnimFile[]> interactAnims = new Dictionary<string, KAnimFile[]>();

	// Token: 0x04002AFA RID: 11002
	private BuildingFacadeAnimateIn animateIn;
}
