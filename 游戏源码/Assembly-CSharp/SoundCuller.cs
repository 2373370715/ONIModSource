using System;
using UnityEngine;

// Token: 0x02000967 RID: 2407
public struct SoundCuller
{
	// Token: 0x06002B64 RID: 11108 RVA: 0x001DF96C File Offset: 0x001DDB6C
	public static bool IsAudibleWorld(Vector2 pos)
	{
		bool result = false;
		int num = Grid.PosToCell(pos);
		if (Grid.IsValidCell(num) && (int)Grid.WorldIdx[num] == ClusterManager.Instance.activeWorldId)
		{
			result = true;
		}
		return result;
	}

	// Token: 0x06002B65 RID: 11109 RVA: 0x000BC37A File Offset: 0x000BA57A
	public bool IsAudible(Vector2 pos)
	{
		return SoundCuller.IsAudibleWorld(pos) && this.min.LessEqual(pos) && pos.LessEqual(this.max);
	}

	// Token: 0x06002B66 RID: 11110 RVA: 0x001DF9A0 File Offset: 0x001DDBA0
	public bool IsAudibleNoCameraScaling(Vector2 pos, float falloff_distance_sq)
	{
		return (pos.x - this.cameraPos.x) * (pos.x - this.cameraPos.x) + (pos.y - this.cameraPos.y) * (pos.y - this.cameraPos.y) < falloff_distance_sq;
	}

	// Token: 0x06002B67 RID: 11111 RVA: 0x000BC3A0 File Offset: 0x000BA5A0
	public bool IsAudible(Vector2 pos, float falloff_distance_sq)
	{
		if (!SoundCuller.IsAudibleWorld(pos))
		{
			return false;
		}
		pos = this.GetVerticallyScaledPosition(pos, false);
		return this.IsAudibleNoCameraScaling(pos, falloff_distance_sq);
	}

	// Token: 0x06002B68 RID: 11112 RVA: 0x000BC3C8 File Offset: 0x000BA5C8
	public bool IsAudible(Vector2 pos, HashedString sound_path)
	{
		return sound_path.IsValid && this.IsAudible(pos, KFMOD.GetSoundEventDescription(sound_path).falloffDistanceSq);
	}

	// Token: 0x06002B69 RID: 11113 RVA: 0x001DF9FC File Offset: 0x001DDBFC
	public Vector3 GetVerticallyScaledPosition(Vector3 pos, bool objectIsSelectedAndVisible = false)
	{
		float num = 1f;
		float num2;
		if (pos.y > this.max.y)
		{
			num2 = Mathf.Abs(pos.y - this.max.y);
		}
		else if (pos.y < this.min.y)
		{
			num2 = Mathf.Abs(pos.y - this.min.y);
			num = -1f;
		}
		else
		{
			num2 = 0f;
		}
		float extraYRange = TuningData<SoundCuller.Tuning>.Get().extraYRange;
		num2 = ((num2 < extraYRange) ? num2 : extraYRange);
		float num3 = num2 * num2 / (4f * this.zoomScaler);
		num3 *= num;
		Vector3 result = new Vector3(pos.x, pos.y + num3, 0f);
		if (objectIsSelectedAndVisible)
		{
			result.z = pos.z;
		}
		return result;
	}

	// Token: 0x06002B6A RID: 11114 RVA: 0x001DFAD0 File Offset: 0x001DDCD0
	public static SoundCuller CreateCuller()
	{
		SoundCuller result = default(SoundCuller);
		Camera main = Camera.main;
		Vector3 vector = main.ViewportToWorldPoint(new Vector3(1f, 1f, Camera.main.transform.GetPosition().z));
		Vector3 vector2 = main.ViewportToWorldPoint(new Vector3(0f, 0f, Camera.main.transform.GetPosition().z));
		result.min = new Vector3(vector2.x, vector2.y, 0f);
		result.max = new Vector3(vector.x, vector.y, 0f);
		result.cameraPos = main.transform.GetPosition();
		Audio audio = Audio.Get();
		float num = CameraController.Instance.OrthographicSize / (audio.listenerReferenceZ - audio.listenerMinZ);
		if (num <= 0f)
		{
			num = 2f;
		}
		else
		{
			num = 1f;
		}
		result.zoomScaler = num;
		return result;
	}

	// Token: 0x04001D2C RID: 7468
	private Vector2 min;

	// Token: 0x04001D2D RID: 7469
	private Vector2 max;

	// Token: 0x04001D2E RID: 7470
	private Vector2 cameraPos;

	// Token: 0x04001D2F RID: 7471
	private float zoomScaler;

	// Token: 0x02000968 RID: 2408
	public class Tuning : TuningData<SoundCuller.Tuning>
	{
		// Token: 0x04001D30 RID: 7472
		public float extraYRange;
	}
}
