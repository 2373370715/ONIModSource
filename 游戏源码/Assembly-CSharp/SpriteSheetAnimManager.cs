using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B0F RID: 2831
[AddComponentMenu("KMonoBehaviour/scripts/SpriteSheetAnimManager")]
public class SpriteSheetAnimManager : KMonoBehaviour, IRenderEveryTick
{
	// Token: 0x0600350F RID: 13583 RVA: 0x000C29A6 File Offset: 0x000C0BA6
	public static void DestroyInstance()
	{
		SpriteSheetAnimManager.instance = null;
	}

	// Token: 0x06003510 RID: 13584 RVA: 0x000C29AE File Offset: 0x000C0BAE
	protected override void OnPrefabInit()
	{
		SpriteSheetAnimManager.instance = this;
	}

	// Token: 0x06003511 RID: 13585 RVA: 0x0020D184 File Offset: 0x0020B384
	protected override void OnSpawn()
	{
		for (int i = 0; i < this.sheets.Length; i++)
		{
			int key = Hash.SDBMLower(this.sheets[i].name);
			this.nameIndexMap[key] = new SpriteSheetAnimator(this.sheets[i]);
		}
	}

	// Token: 0x06003512 RID: 13586 RVA: 0x0020D1D8 File Offset: 0x0020B3D8
	public void Play(string name, Vector3 pos, Vector2 size, Color32 colour)
	{
		int name_hash = Hash.SDBMLower(name);
		this.Play(name_hash, pos, Quaternion.identity, size, colour);
	}

	// Token: 0x06003513 RID: 13587 RVA: 0x0020D1FC File Offset: 0x0020B3FC
	public void Play(string name, Vector3 pos, Quaternion rotation, Vector2 size, Color32 colour)
	{
		int name_hash = Hash.SDBMLower(name);
		this.Play(name_hash, pos, rotation, size, colour);
	}

	// Token: 0x06003514 RID: 13588 RVA: 0x000C29B6 File Offset: 0x000C0BB6
	public void Play(int name_hash, Vector3 pos, Quaternion rotation, Vector2 size, Color32 colour)
	{
		this.nameIndexMap[name_hash].Play(pos, rotation, size, colour);
	}

	// Token: 0x06003515 RID: 13589 RVA: 0x000C29D4 File Offset: 0x000C0BD4
	public void RenderEveryTick(float dt)
	{
		this.UpdateAnims(dt);
		this.Render();
	}

	// Token: 0x06003516 RID: 13590 RVA: 0x0020D220 File Offset: 0x0020B420
	public void UpdateAnims(float dt)
	{
		foreach (KeyValuePair<int, SpriteSheetAnimator> keyValuePair in this.nameIndexMap)
		{
			keyValuePair.Value.UpdateAnims(dt);
		}
	}

	// Token: 0x06003517 RID: 13591 RVA: 0x0020D27C File Offset: 0x0020B47C
	public void Render()
	{
		Vector3 zero = Vector3.zero;
		foreach (KeyValuePair<int, SpriteSheetAnimator> keyValuePair in this.nameIndexMap)
		{
			keyValuePair.Value.Render();
		}
	}

	// Token: 0x06003518 RID: 13592 RVA: 0x000C29E3 File Offset: 0x000C0BE3
	public SpriteSheetAnimator GetSpriteSheetAnimator(HashedString name)
	{
		return this.nameIndexMap[name.HashValue];
	}

	// Token: 0x0400241B RID: 9243
	public const float SECONDS_PER_FRAME = 0.033333335f;

	// Token: 0x0400241C RID: 9244
	[SerializeField]
	private SpriteSheet[] sheets;

	// Token: 0x0400241D RID: 9245
	private Dictionary<int, SpriteSheetAnimator> nameIndexMap = new Dictionary<int, SpriteSheetAnimator>();

	// Token: 0x0400241E RID: 9246
	public static SpriteSheetAnimManager instance;
}
