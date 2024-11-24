using System;
using UnityEngine;

// Token: 0x0200067F RID: 1663
public static class ChoreHelpers
{
	// Token: 0x06001E41 RID: 7745 RVA: 0x000B3F39 File Offset: 0x000B2139
	public static GameObject CreateLocator(string name, Vector3 pos)
	{
		GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(ApproachableLocator.ID), null, null);
		gameObject.name = name;
		gameObject.transform.SetPosition(pos);
		gameObject.gameObject.SetActive(true);
		return gameObject;
	}

	// Token: 0x06001E42 RID: 7746 RVA: 0x000B3F71 File Offset: 0x000B2171
	public static GameObject CreateSleepLocator(Vector3 pos)
	{
		GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(SleepLocator.ID), null, null);
		gameObject.name = "SLeepLocator";
		gameObject.transform.SetPosition(pos);
		gameObject.gameObject.SetActive(true);
		return gameObject;
	}

	// Token: 0x06001E43 RID: 7747 RVA: 0x000B3FAD File Offset: 0x000B21AD
	public static void DestroyLocator(GameObject locator)
	{
		if (locator != null)
		{
			locator.gameObject.DeleteObject();
		}
	}
}
