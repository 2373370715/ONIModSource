using System;
using System.Collections.Generic;
using Klei;
using UnityEngine;

// Token: 0x02001EB0 RID: 7856
public class PopFXManager : KScreen
{
	// Token: 0x0600A4F5 RID: 42229 RVA: 0x0010AFDA File Offset: 0x001091DA
	public static void DestroyInstance()
	{
		PopFXManager.Instance = null;
	}

	// Token: 0x0600A4F6 RID: 42230 RVA: 0x0010AFE2 File Offset: 0x001091E2
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		PopFXManager.Instance = this;
	}

	// Token: 0x0600A4F7 RID: 42231 RVA: 0x003EA5D4 File Offset: 0x003E87D4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.ready = true;
		if (GenericGameSettings.instance.disablePopFx)
		{
			return;
		}
		for (int i = 0; i < 20; i++)
		{
			PopFX item = this.CreatePopFX();
			this.Pool.Add(item);
		}
	}

	// Token: 0x0600A4F8 RID: 42232 RVA: 0x0010AFF0 File Offset: 0x001091F0
	public bool Ready()
	{
		return this.ready;
	}

	// Token: 0x0600A4F9 RID: 42233 RVA: 0x003EA61C File Offset: 0x003E881C
	public PopFX SpawnFX(Sprite icon, string text, Transform target_transform, Vector3 offset, float lifetime = 1.5f, bool track_target = false, bool force_spawn = false)
	{
		if (GenericGameSettings.instance.disablePopFx)
		{
			return null;
		}
		if (Game.IsQuitting())
		{
			return null;
		}
		Vector3 vector = offset;
		if (target_transform != null)
		{
			vector += target_transform.GetPosition();
		}
		if (!force_spawn)
		{
			int cell = Grid.PosToCell(vector);
			if (!Grid.IsValidCell(cell) || !Grid.IsVisible(cell))
			{
				return null;
			}
		}
		PopFX popFX;
		if (this.Pool.Count > 0)
		{
			popFX = this.Pool[0];
			this.Pool[0].gameObject.SetActive(true);
			this.Pool[0].Spawn(icon, text, target_transform, offset, lifetime, track_target);
			this.Pool.RemoveAt(0);
		}
		else
		{
			popFX = this.CreatePopFX();
			popFX.gameObject.SetActive(true);
			popFX.Spawn(icon, text, target_transform, offset, lifetime, track_target);
		}
		return popFX;
	}

	// Token: 0x0600A4FA RID: 42234 RVA: 0x0010AFF8 File Offset: 0x001091F8
	public PopFX SpawnFX(Sprite icon, string text, Transform target_transform, float lifetime = 1.5f, bool track_target = false)
	{
		return this.SpawnFX(icon, text, target_transform, Vector3.zero, lifetime, track_target, false);
	}

	// Token: 0x0600A4FB RID: 42235 RVA: 0x0010B00D File Offset: 0x0010920D
	private PopFX CreatePopFX()
	{
		GameObject gameObject = Util.KInstantiate(this.Prefab_PopFX, base.gameObject, "Pooled_PopFX");
		gameObject.transform.localScale = Vector3.one;
		return gameObject.GetComponent<PopFX>();
	}

	// Token: 0x0600A4FC RID: 42236 RVA: 0x0010B03A File Offset: 0x0010923A
	public void RecycleFX(PopFX fx)
	{
		this.Pool.Add(fx);
	}

	// Token: 0x0400811D RID: 33053
	public static PopFXManager Instance;

	// Token: 0x0400811E RID: 33054
	public GameObject Prefab_PopFX;

	// Token: 0x0400811F RID: 33055
	public List<PopFX> Pool = new List<PopFX>();

	// Token: 0x04008120 RID: 33056
	public Sprite sprite_Plus;

	// Token: 0x04008121 RID: 33057
	public Sprite sprite_Negative;

	// Token: 0x04008122 RID: 33058
	public Sprite sprite_Resource;

	// Token: 0x04008123 RID: 33059
	public Sprite sprite_Building;

	// Token: 0x04008124 RID: 33060
	public Sprite sprite_Research;

	// Token: 0x04008125 RID: 33061
	private bool ready;
}
