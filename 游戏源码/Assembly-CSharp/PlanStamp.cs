using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001EAC RID: 7852
[AddComponentMenu("KMonoBehaviour/scripts/PlanStamp")]
public class PlanStamp : KMonoBehaviour
{
	// Token: 0x0600A4EA RID: 42218 RVA: 0x0010AF31 File Offset: 0x00109131
	public void SetStamp(Sprite sprite, string Text)
	{
		this.StampImage.sprite = sprite;
		this.StampText.text = Text.ToUpper();
	}

	// Token: 0x04008100 RID: 33024
	public PlanStamp.StampArt stampSprites;

	// Token: 0x04008101 RID: 33025
	[SerializeField]
	private Image StampImage;

	// Token: 0x04008102 RID: 33026
	[SerializeField]
	private Text StampText;

	// Token: 0x02001EAD RID: 7853
	[Serializable]
	public struct StampArt
	{
		// Token: 0x04008103 RID: 33027
		public Sprite UnderConstruction;

		// Token: 0x04008104 RID: 33028
		public Sprite NeedsResearch;

		// Token: 0x04008105 RID: 33029
		public Sprite SelectResource;

		// Token: 0x04008106 RID: 33030
		public Sprite NeedsRepair;

		// Token: 0x04008107 RID: 33031
		public Sprite NeedsPower;

		// Token: 0x04008108 RID: 33032
		public Sprite NeedsResource;

		// Token: 0x04008109 RID: 33033
		public Sprite NeedsGasPipe;

		// Token: 0x0400810A RID: 33034
		public Sprite NeedsLiquidPipe;
	}
}
