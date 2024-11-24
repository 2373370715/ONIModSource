using System.Collections.Generic;
using Database;
using KSerialization;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class BuildingFacade : KMonoBehaviour
{
	[Serialize]
	private string currentFacade;

	public KAnimFile[] animFiles;

	public Dictionary<string, KAnimFile[]> interactAnims = new Dictionary<string, KAnimFile[]>();

	private BuildingFacadeAnimateIn animateIn;

	public string CurrentFacade => currentFacade;

	public bool IsOriginal => currentFacade.IsNullOrWhiteSpace();

	protected override void OnPrefabInit()
	{
	}

	protected override void OnSpawn()
	{
		if (!IsOriginal)
		{
			ApplyBuildingFacade(Db.GetBuildingFacades().TryGet(currentFacade));
		}
	}

	public void ApplyDefaultFacade(bool shouldTryAnimate = false)
	{
		currentFacade = "DEFAULT_FACADE";
		ClearFacade(shouldTryAnimate);
	}

	public void ApplyBuildingFacade(BuildingFacadeResource facade, bool shouldTryAnimate = false)
	{
		if (facade == null)
		{
			ClearFacade();
			return;
		}
		currentFacade = facade.Id;
		KAnimFile[] array = new KAnimFile[1] { Assets.GetAnim(facade.AnimFile) };
		ChangeBuilding(array, facade.Name, facade.Description, facade.InteractFile, shouldTryAnimate);
	}

	private void ClearFacade(bool shouldTryAnimate = false)
	{
		Building component = GetComponent<Building>();
		ChangeBuilding(component.Def.AnimFiles, component.Def.Name, component.Def.Desc, null, shouldTryAnimate);
	}

	private void ChangeBuilding(KAnimFile[] animFiles, string displayName, string desc, Dictionary<string, string> interactAnimsNames = null, bool shouldTryAnimate = false)
	{
		interactAnims.Clear();
		if (interactAnimsNames != null && interactAnimsNames.Count > 0)
		{
			interactAnims = new Dictionary<string, KAnimFile[]>();
			foreach (KeyValuePair<string, string> interactAnimsName in interactAnimsNames)
			{
				interactAnims.Add(interactAnimsName.Key, new KAnimFile[1] { Assets.GetAnim(interactAnimsName.Value) });
			}
		}
		Building[] components = GetComponents<Building>();
		Building[] array = components;
		foreach (Building obj in array)
		{
			obj.SetDescriptionFlavour(desc);
			KBatchedAnimController component = obj.GetComponent<KBatchedAnimController>();
			HashedString batchGroupID = component.batchGroupID;
			component.SwapAnims(animFiles);
			KBatchedAnimController[] componentsInChildren = obj.GetComponentsInChildren<KBatchedAnimController>(includeInactive: true);
			foreach (KBatchedAnimController kBatchedAnimController in componentsInChildren)
			{
				if (kBatchedAnimController.batchGroupID == batchGroupID)
				{
					kBatchedAnimController.SwapAnims(animFiles);
				}
			}
			if (!animateIn.IsNullOrDestroyed())
			{
				Object.Destroy(animateIn);
				animateIn = null;
			}
			if (shouldTryAnimate)
			{
				animateIn = BuildingFacadeAnimateIn.MakeFor(component);
				string parameter = "Unlocked";
				float parameterValue = 1f;
				KFMOD.PlayUISoundWithParameter(GlobalAssets.GetSound(KleiInventoryScreen.GetFacadeItemSoundName(Db.Get().Permits.TryGet(currentFacade)) + "_Click"), parameter, parameterValue);
			}
		}
		GetComponent<KSelectable>().SetName(displayName);
		if (GetComponent<AnimTileable>() != null && components.Length != 0)
		{
			GameScenePartitioner.Instance.TriggerEvent(components[0].GetExtents(), GameScenePartitioner.Instance.objectLayers[1], null);
		}
	}

	public string GetNextFacade()
	{
		BuildingDef def = GetComponent<Building>().Def;
		int num = def.AvailableFacades.FindIndex((string s) => s == currentFacade) + 1;
		if (num >= def.AvailableFacades.Count)
		{
			num = 0;
		}
		return def.AvailableFacades[num];
	}
}
