using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B0D RID: 2829
public class SpriteSheetAnimator
{
	// Token: 0x06003506 RID: 13574 RVA: 0x0020CAF8 File Offset: 0x0020ACF8
	public SpriteSheetAnimator(SpriteSheet sheet)
	{
		this.sheet = sheet;
		this.mesh = new Mesh();
		this.mesh.name = "SpriteSheetAnimator";
		this.mesh.MarkDynamic();
		this.materialProperties = new MaterialPropertyBlock();
		this.materialProperties.SetTexture("_MainTex", sheet.texture);
	}

	// Token: 0x06003507 RID: 13575 RVA: 0x0020CB70 File Offset: 0x0020AD70
	public void Play(Vector3 pos, Quaternion rotation, Vector2 size, Color colour)
	{
		if (rotation == Quaternion.identity)
		{
			this.anims.Add(new SpriteSheetAnimator.AnimInfo
			{
				elapsedTime = 0f,
				pos = pos,
				rotation = rotation,
				size = size,
				colour = colour
			});
			return;
		}
		this.rotatedAnims.Add(new SpriteSheetAnimator.AnimInfo
		{
			elapsedTime = 0f,
			pos = pos,
			rotation = rotation,
			size = size,
			colour = colour
		});
	}

	// Token: 0x06003508 RID: 13576 RVA: 0x0020CC18 File Offset: 0x0020AE18
	private void GetUVs(int frame, out Vector2 uv_bl, out Vector2 uv_br, out Vector2 uv_tl, out Vector2 uv_tr)
	{
		int num = frame / this.sheet.numXFrames;
		int num2 = frame % this.sheet.numXFrames;
		float x = (float)num2 * this.sheet.uvFrameSize.x;
		float x2 = (float)(num2 + 1) * this.sheet.uvFrameSize.x;
		float y = 1f - (float)(num + 1) * this.sheet.uvFrameSize.y;
		float y2 = 1f - (float)num * this.sheet.uvFrameSize.y;
		uv_bl = new Vector2(x, y);
		uv_br = new Vector2(x2, y);
		uv_tl = new Vector2(x, y2);
		uv_tr = new Vector2(x2, y2);
	}

	// Token: 0x06003509 RID: 13577 RVA: 0x000C2954 File Offset: 0x000C0B54
	public int GetFrameFromElapsedTime(float elapsed_time)
	{
		return Mathf.Min(this.sheet.numFrames, (int)(elapsed_time / 0.033333335f));
	}

	// Token: 0x0600350A RID: 13578 RVA: 0x0020CCD8 File Offset: 0x0020AED8
	public int GetFrameFromElapsedTimeLooping(float elapsed_time)
	{
		int num = (int)(elapsed_time / 0.033333335f);
		if (num > this.sheet.numFrames)
		{
			num %= this.sheet.numFrames;
		}
		return num;
	}

	// Token: 0x0600350B RID: 13579 RVA: 0x000C296E File Offset: 0x000C0B6E
	public void UpdateAnims(float dt)
	{
		this.UpdateAnims(dt, this.anims);
		this.UpdateAnims(dt, this.rotatedAnims);
	}

	// Token: 0x0600350C RID: 13580 RVA: 0x0020CD0C File Offset: 0x0020AF0C
	private void UpdateAnims(float dt, IList<SpriteSheetAnimator.AnimInfo> anims)
	{
		int num = anims.Count;
		int i = 0;
		while (i < num)
		{
			SpriteSheetAnimator.AnimInfo animInfo = anims[i];
			animInfo.elapsedTime += dt;
			animInfo.frame = Mathf.Min(this.sheet.numFrames, (int)(animInfo.elapsedTime / 0.033333335f));
			if (animInfo.frame >= this.sheet.numFrames)
			{
				num--;
				anims[i] = anims[num];
				anims.RemoveAt(num);
			}
			else
			{
				anims[i] = animInfo;
				i++;
			}
		}
	}

	// Token: 0x0600350D RID: 13581 RVA: 0x0020CD9C File Offset: 0x0020AF9C
	public void Render(List<SpriteSheetAnimator.AnimInfo> anim_infos, bool apply_rotation)
	{
		ListPool<Vector3, SpriteSheetAnimManager>.PooledList pooledList = ListPool<Vector3, SpriteSheetAnimManager>.Allocate();
		ListPool<Vector2, SpriteSheetAnimManager>.PooledList pooledList2 = ListPool<Vector2, SpriteSheetAnimManager>.Allocate();
		ListPool<Color32, SpriteSheetAnimManager>.PooledList pooledList3 = ListPool<Color32, SpriteSheetAnimManager>.Allocate();
		ListPool<int, SpriteSheetAnimManager>.PooledList pooledList4 = ListPool<int, SpriteSheetAnimManager>.Allocate();
		this.mesh.Clear();
		if (apply_rotation)
		{
			int count = anim_infos.Count;
			for (int i = 0; i < count; i++)
			{
				SpriteSheetAnimator.AnimInfo animInfo = anim_infos[i];
				Vector2 vector = animInfo.size * 0.5f;
				Vector3 b = animInfo.rotation * -vector;
				Vector3 b2 = animInfo.rotation * new Vector2(vector.x, -vector.y);
				Vector3 b3 = animInfo.rotation * new Vector2(-vector.x, vector.y);
				Vector3 b4 = animInfo.rotation * vector;
				pooledList.Add(animInfo.pos + b);
				pooledList.Add(animInfo.pos + b2);
				pooledList.Add(animInfo.pos + b4);
				pooledList.Add(animInfo.pos + b3);
				Vector2 item;
				Vector2 item2;
				Vector2 item3;
				Vector2 item4;
				this.GetUVs(animInfo.frame, out item, out item2, out item3, out item4);
				pooledList2.Add(item);
				pooledList2.Add(item2);
				pooledList2.Add(item4);
				pooledList2.Add(item3);
				pooledList3.Add(animInfo.colour);
				pooledList3.Add(animInfo.colour);
				pooledList3.Add(animInfo.colour);
				pooledList3.Add(animInfo.colour);
				int num = i * 4;
				pooledList4.Add(num);
				pooledList4.Add(num + 1);
				pooledList4.Add(num + 2);
				pooledList4.Add(num);
				pooledList4.Add(num + 2);
				pooledList4.Add(num + 3);
			}
		}
		else
		{
			int count2 = anim_infos.Count;
			for (int j = 0; j < count2; j++)
			{
				SpriteSheetAnimator.AnimInfo animInfo2 = anim_infos[j];
				Vector2 vector2 = animInfo2.size * 0.5f;
				Vector3 b5 = -vector2;
				Vector3 b6 = new Vector2(vector2.x, -vector2.y);
				Vector3 b7 = new Vector2(-vector2.x, vector2.y);
				Vector3 b8 = vector2;
				pooledList.Add(animInfo2.pos + b5);
				pooledList.Add(animInfo2.pos + b6);
				pooledList.Add(animInfo2.pos + b8);
				pooledList.Add(animInfo2.pos + b7);
				Vector2 item5;
				Vector2 item6;
				Vector2 item7;
				Vector2 item8;
				this.GetUVs(animInfo2.frame, out item5, out item6, out item7, out item8);
				pooledList2.Add(item5);
				pooledList2.Add(item6);
				pooledList2.Add(item8);
				pooledList2.Add(item7);
				pooledList3.Add(animInfo2.colour);
				pooledList3.Add(animInfo2.colour);
				pooledList3.Add(animInfo2.colour);
				pooledList3.Add(animInfo2.colour);
				int num2 = j * 4;
				pooledList4.Add(num2);
				pooledList4.Add(num2 + 1);
				pooledList4.Add(num2 + 2);
				pooledList4.Add(num2);
				pooledList4.Add(num2 + 2);
				pooledList4.Add(num2 + 3);
			}
		}
		this.mesh.SetVertices(pooledList);
		this.mesh.SetUVs(0, pooledList2);
		this.mesh.SetColors(pooledList3);
		this.mesh.SetTriangles(pooledList4, 0);
		Graphics.DrawMesh(this.mesh, Vector3.zero, Quaternion.identity, this.sheet.material, this.sheet.renderLayer, null, 0, this.materialProperties);
		pooledList4.Recycle();
		pooledList3.Recycle();
		pooledList2.Recycle();
		pooledList.Recycle();
	}

	// Token: 0x0600350E RID: 13582 RVA: 0x000C298A File Offset: 0x000C0B8A
	public void Render()
	{
		this.Render(this.anims, false);
		this.Render(this.rotatedAnims, true);
	}

	// Token: 0x04002410 RID: 9232
	private SpriteSheet sheet;

	// Token: 0x04002411 RID: 9233
	private Mesh mesh;

	// Token: 0x04002412 RID: 9234
	private MaterialPropertyBlock materialProperties;

	// Token: 0x04002413 RID: 9235
	private List<SpriteSheetAnimator.AnimInfo> anims = new List<SpriteSheetAnimator.AnimInfo>();

	// Token: 0x04002414 RID: 9236
	private List<SpriteSheetAnimator.AnimInfo> rotatedAnims = new List<SpriteSheetAnimator.AnimInfo>();

	// Token: 0x02000B0E RID: 2830
	public struct AnimInfo
	{
		// Token: 0x04002415 RID: 9237
		public int frame;

		// Token: 0x04002416 RID: 9238
		public float elapsedTime;

		// Token: 0x04002417 RID: 9239
		public Vector3 pos;

		// Token: 0x04002418 RID: 9240
		public Quaternion rotation;

		// Token: 0x04002419 RID: 9241
		public Vector2 size;

		// Token: 0x0400241A RID: 9242
		public Color32 colour;
	}
}
