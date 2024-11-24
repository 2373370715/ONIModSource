using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000C23 RID: 3107
[DebuggerDisplay("{name}")]
[Serializable]
public class TintedSprite : ISerializationCallbackReceiver
{
	// Token: 0x06003B44 RID: 15172 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnAfterDeserialize()
	{
	}

	// Token: 0x06003B45 RID: 15173 RVA: 0x000C646E File Offset: 0x000C466E
	public void OnBeforeSerialize()
	{
		if (this.sprite != null)
		{
			this.name = this.sprite.name;
		}
	}

	// Token: 0x04002881 RID: 10369
	[ReadOnly]
	public string name;

	// Token: 0x04002882 RID: 10370
	public Sprite sprite;

	// Token: 0x04002883 RID: 10371
	public Color color;
}
