using STRINGS;
using UnityEngine;

namespace Database;

public struct PermitPresentationInfo
{
	public Sprite sprite;

	public string facadeFor { get; private set; }

	public static Sprite GetUnknownSprite()
	{
		return Assets.GetSprite("unknown");
	}

	public void SetFacadeForPrefabName(string prefabName)
	{
		facadeFor = UI.KLEI_INVENTORY_SCREEN.ITEM_FACADE_FOR.Replace("{ConfigProperName}", prefabName);
	}

	public void SetFacadeForPrefabID(string prefabId)
	{
		if (Assets.TryGetPrefab(prefabId) == null)
		{
			facadeFor = UI.KLEI_INVENTORY_SCREEN.ITEM_DLC_REQUIRED;
		}
		else
		{
			facadeFor = UI.KLEI_INVENTORY_SCREEN.ITEM_FACADE_FOR.Replace("{ConfigProperName}", Assets.GetPrefab(prefabId).GetProperName());
		}
	}

	public void SetFacadeForText(string text)
	{
		facadeFor = text;
	}
}
