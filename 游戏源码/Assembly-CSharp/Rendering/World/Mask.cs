using System;
using UnityEngine;

namespace Rendering.World
{
	// Token: 0x020020D2 RID: 8402
	public struct Mask
	{
		// Token: 0x17000B71 RID: 2929
		// (get) Token: 0x0600B2B0 RID: 45744 RVA: 0x00114077 File Offset: 0x00112277
		// (set) Token: 0x0600B2B1 RID: 45745 RVA: 0x0011407F File Offset: 0x0011227F
		public Vector2 UV0 { readonly get; private set; }

		// Token: 0x17000B72 RID: 2930
		// (get) Token: 0x0600B2B2 RID: 45746 RVA: 0x00114088 File Offset: 0x00112288
		// (set) Token: 0x0600B2B3 RID: 45747 RVA: 0x00114090 File Offset: 0x00112290
		public Vector2 UV1 { readonly get; private set; }

		// Token: 0x17000B73 RID: 2931
		// (get) Token: 0x0600B2B4 RID: 45748 RVA: 0x00114099 File Offset: 0x00112299
		// (set) Token: 0x0600B2B5 RID: 45749 RVA: 0x001140A1 File Offset: 0x001122A1
		public Vector2 UV2 { readonly get; private set; }

		// Token: 0x17000B74 RID: 2932
		// (get) Token: 0x0600B2B6 RID: 45750 RVA: 0x001140AA File Offset: 0x001122AA
		// (set) Token: 0x0600B2B7 RID: 45751 RVA: 0x001140B2 File Offset: 0x001122B2
		public Vector2 UV3 { readonly get; private set; }

		// Token: 0x17000B75 RID: 2933
		// (get) Token: 0x0600B2B8 RID: 45752 RVA: 0x001140BB File Offset: 0x001122BB
		// (set) Token: 0x0600B2B9 RID: 45753 RVA: 0x001140C3 File Offset: 0x001122C3
		public bool IsOpaque { readonly get; private set; }

		// Token: 0x0600B2BA RID: 45754 RVA: 0x00438148 File Offset: 0x00436348
		public Mask(TextureAtlas atlas, int texture_idx, bool transpose, bool flip_x, bool flip_y, bool is_opaque)
		{
			this = default(Mask);
			this.atlas = atlas;
			this.texture_idx = texture_idx;
			this.transpose = transpose;
			this.flip_x = flip_x;
			this.flip_y = flip_y;
			this.atlas_offset = 0;
			this.IsOpaque = is_opaque;
			this.Refresh();
		}

		// Token: 0x0600B2BB RID: 45755 RVA: 0x001140CC File Offset: 0x001122CC
		public void SetOffset(int offset)
		{
			this.atlas_offset = offset;
			this.Refresh();
		}

		// Token: 0x0600B2BC RID: 45756 RVA: 0x00438198 File Offset: 0x00436398
		public void Refresh()
		{
			int num = this.atlas_offset * 4 + this.atlas_offset;
			if (num + this.texture_idx >= this.atlas.items.Length)
			{
				num = 0;
			}
			Vector4 uvBox = this.atlas.items[num + this.texture_idx].uvBox;
			Vector2 zero = Vector2.zero;
			Vector2 zero2 = Vector2.zero;
			Vector2 zero3 = Vector2.zero;
			Vector2 zero4 = Vector2.zero;
			if (this.transpose)
			{
				float x = uvBox.x;
				float x2 = uvBox.z;
				if (this.flip_x)
				{
					x = uvBox.z;
					x2 = uvBox.x;
				}
				zero.x = x;
				zero2.x = x;
				zero3.x = x2;
				zero4.x = x2;
				float y = uvBox.y;
				float y2 = uvBox.w;
				if (this.flip_y)
				{
					y = uvBox.w;
					y2 = uvBox.y;
				}
				zero.y = y;
				zero2.y = y2;
				zero3.y = y;
				zero4.y = y2;
			}
			else
			{
				float x3 = uvBox.x;
				float x4 = uvBox.z;
				if (this.flip_x)
				{
					x3 = uvBox.z;
					x4 = uvBox.x;
				}
				zero.x = x3;
				zero2.x = x4;
				zero3.x = x3;
				zero4.x = x4;
				float y3 = uvBox.y;
				float y4 = uvBox.w;
				if (this.flip_y)
				{
					y3 = uvBox.w;
					y4 = uvBox.y;
				}
				zero.y = y4;
				zero2.y = y4;
				zero3.y = y3;
				zero4.y = y3;
			}
			this.UV0 = zero;
			this.UV1 = zero2;
			this.UV2 = zero3;
			this.UV3 = zero4;
		}

		// Token: 0x04008D45 RID: 36165
		private TextureAtlas atlas;

		// Token: 0x04008D46 RID: 36166
		private int texture_idx;

		// Token: 0x04008D47 RID: 36167
		private bool transpose;

		// Token: 0x04008D48 RID: 36168
		private bool flip_x;

		// Token: 0x04008D49 RID: 36169
		private bool flip_y;

		// Token: 0x04008D4A RID: 36170
		private int atlas_offset;

		// Token: 0x04008D4B RID: 36171
		private const int TILES_PER_SET = 4;
	}
}
