using System;
using UnityEngine;

// Token: 0x02001F5A RID: 8026
public interface IConfigurableConsumerOption
{
	// Token: 0x0600A96A RID: 43370
	Tag GetID();

	// Token: 0x0600A96B RID: 43371
	string GetName();

	// Token: 0x0600A96C RID: 43372
	string GetDetailedDescription();

	// Token: 0x0600A96D RID: 43373
	string GetDescription();

	// Token: 0x0600A96E RID: 43374
	Sprite GetIcon();

	// Token: 0x0600A96F RID: 43375
	IConfigurableConsumerIngredient[] GetIngredients();
}
