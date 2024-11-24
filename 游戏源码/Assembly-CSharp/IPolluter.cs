using System;
using UnityEngine;

// Token: 0x02001663 RID: 5731
public interface IPolluter
{
	// Token: 0x06007642 RID: 30274
	int GetRadius();

	// Token: 0x06007643 RID: 30275
	int GetNoise();

	// Token: 0x06007644 RID: 30276
	GameObject GetGameObject();

	// Token: 0x06007645 RID: 30277
	void SetAttributes(Vector2 pos, int dB, GameObject go, string name = null);

	// Token: 0x06007646 RID: 30278
	string GetName();

	// Token: 0x06007647 RID: 30279
	Vector2 GetPosition();

	// Token: 0x06007648 RID: 30280
	void Clear();

	// Token: 0x06007649 RID: 30281
	void SetSplat(NoiseSplat splat);
}
