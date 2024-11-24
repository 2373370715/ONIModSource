using System;
using STRINGS;
using UnityEngine;

namespace Database
{
	// Token: 0x0200214C RID: 8524
	public struct PermitPresentationInfo
	{
		// Token: 0x17000BB3 RID: 2995
		// (get) Token: 0x0600B59E RID: 46494 RVA: 0x001151FB File Offset: 0x001133FB
		// (set) Token: 0x0600B59F RID: 46495 RVA: 0x00115203 File Offset: 0x00113403
		public string facadeFor { readonly get; private set; }

		// Token: 0x0600B5A0 RID: 46496 RVA: 0x0011520C File Offset: 0x0011340C
		public static Sprite GetUnknownSprite()
		{
			return Assets.GetSprite("unknown");
		}

		// Token: 0x0600B5A1 RID: 46497 RVA: 0x0011521D File Offset: 0x0011341D
		public void SetFacadeForPrefabName(string prefabName)
		{
			this.facadeFor = UI.KLEI_INVENTORY_SCREEN.ITEM_FACADE_FOR.Replace("{ConfigProperName}", prefabName);
		}

		// Token: 0x0600B5A2 RID: 46498 RVA: 0x00452620 File Offset: 0x00450820
		public void SetFacadeForPrefabID(string prefabId)
		{
			if (Assets.TryGetPrefab(prefabId) == null)
			{
				this.facadeFor = UI.KLEI_INVENTORY_SCREEN.ITEM_DLC_REQUIRED;
				return;
			}
			this.facadeFor = UI.KLEI_INVENTORY_SCREEN.ITEM_FACADE_FOR.Replace("{ConfigProperName}", Assets.GetPrefab(prefabId).GetProperName());
		}

		// Token: 0x0600B5A3 RID: 46499 RVA: 0x00115235 File Offset: 0x00113435
		public void SetFacadeForText(string text)
		{
			this.facadeFor = text;
		}

		// Token: 0x04009390 RID: 37776
		public Sprite sprite;
	}
}
