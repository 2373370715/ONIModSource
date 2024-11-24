using System;
using UnityEngine;

// Token: 0x02001D0A RID: 7434
[AddComponentMenu("KMonoBehaviour/scripts/InstantiateUIPrefabChild")]
public class InstantiateUIPrefabChild : KMonoBehaviour
{
	// Token: 0x06009B37 RID: 39735 RVA: 0x00104FE9 File Offset: 0x001031E9
	protected override void OnPrefabInit()
	{
		if (this.InstantiateOnAwake)
		{
			this.Instantiate();
		}
	}

	// Token: 0x06009B38 RID: 39736 RVA: 0x003BD5EC File Offset: 0x003BB7EC
	public void Instantiate()
	{
		if (this.alreadyInstantiated)
		{
			global::Debug.LogWarning(base.gameObject.name + "trying to instantiate UI prefabs multiple times.");
			return;
		}
		this.alreadyInstantiated = true;
		foreach (GameObject gameObject in this.prefabs)
		{
			if (!(gameObject == null))
			{
				Vector3 v = gameObject.rectTransform().anchoredPosition;
				GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
				gameObject2.transform.SetParent(base.transform);
				gameObject2.rectTransform().anchoredPosition = v;
				gameObject2.rectTransform().localScale = Vector3.one;
				if (this.setAsFirstSibling)
				{
					gameObject2.transform.SetAsFirstSibling();
				}
			}
		}
	}

	// Token: 0x04007954 RID: 31060
	public GameObject[] prefabs;

	// Token: 0x04007955 RID: 31061
	public bool InstantiateOnAwake = true;

	// Token: 0x04007956 RID: 31062
	private bool alreadyInstantiated;

	// Token: 0x04007957 RID: 31063
	public bool setAsFirstSibling;
}
