using System;
using UnityEngine;

// Token: 0x02001D48 RID: 7496
public class PrefabDefinedUIPosition
{
	// Token: 0x06009C98 RID: 40088 RVA: 0x00105E59 File Offset: 0x00104059
	public void SetOn(GameObject gameObject)
	{
		if (this.position.HasValue)
		{
			gameObject.rectTransform().anchoredPosition = this.position.Value;
			return;
		}
		this.position = gameObject.rectTransform().anchoredPosition;
	}

	// Token: 0x06009C99 RID: 40089 RVA: 0x00105E95 File Offset: 0x00104095
	public void SetOn(Component component)
	{
		if (this.position.HasValue)
		{
			component.rectTransform().anchoredPosition = this.position.Value;
			return;
		}
		this.position = component.rectTransform().anchoredPosition;
	}

	// Token: 0x04007ACE RID: 31438
	private Option<Vector2> position;
}
