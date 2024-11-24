using System;
using UnityEngine;

namespace rendering
{
	// Token: 0x020020D8 RID: 8408
	public class BackWall : MonoBehaviour
	{
		// Token: 0x0600B2D3 RID: 45779 RVA: 0x00114168 File Offset: 0x00112368
		private void Awake()
		{
			this.backwallMaterial.SetTexture("images", this.array);
		}

		// Token: 0x04008D65 RID: 36197
		[SerializeField]
		public Material backwallMaterial;

		// Token: 0x04008D66 RID: 36198
		[SerializeField]
		public Texture2DArray array;
	}
}
