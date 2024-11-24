using System;

// Token: 0x020019D9 RID: 6617
public interface IReadonlyTags
{
	// Token: 0x060089D5 RID: 35285
	bool HasTag(string tag);

	// Token: 0x060089D6 RID: 35286
	bool HasTag(int hashtag);

	// Token: 0x060089D7 RID: 35287
	bool HasTags(int[] tags);
}
