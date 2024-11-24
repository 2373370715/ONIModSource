using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02002054 RID: 8276
public class DialogPanel : MonoBehaviour, IDeselectHandler, IEventSystemHandler
{
	// Token: 0x0600B038 RID: 45112 RVA: 0x004238E4 File Offset: 0x00421AE4
	public void OnDeselect(BaseEventData eventData)
	{
		if (this.destroyOnDeselect)
		{
			foreach (object obj in base.transform)
			{
				Util.KDestroyGameObject(((Transform)obj).gameObject);
			}
		}
		base.gameObject.SetActive(false);
	}

	// Token: 0x04008AF3 RID: 35571
	public bool destroyOnDeselect = true;
}
