using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001D24 RID: 7460
[AddComponentMenu("KMonoBehaviour/scripts/KCanvasScaler")]
public class KCanvasScaler : KMonoBehaviour
{
	// Token: 0x06009BB2 RID: 39858 RVA: 0x003C0AA4 File Offset: 0x003BECA4
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (KPlayerPrefs.HasKey(KCanvasScaler.UIScalePrefKey))
		{
			this.SetUserScale(KPlayerPrefs.GetFloat(KCanvasScaler.UIScalePrefKey) / 100f);
		}
		else
		{
			this.SetUserScale(1f);
		}
		ScreenResize instance = ScreenResize.Instance;
		instance.OnResize = (System.Action)Delegate.Combine(instance.OnResize, new System.Action(this.OnResize));
	}

	// Token: 0x06009BB3 RID: 39859 RVA: 0x001054E5 File Offset: 0x001036E5
	private void OnResize()
	{
		this.SetUserScale(this.userScale);
	}

	// Token: 0x06009BB4 RID: 39860 RVA: 0x001054F3 File Offset: 0x001036F3
	public void SetUserScale(float scale)
	{
		if (this.canvasScaler == null)
		{
			this.canvasScaler = base.GetComponent<CanvasScaler>();
		}
		this.userScale = scale;
		this.canvasScaler.scaleFactor = this.GetCanvasScale();
	}

	// Token: 0x06009BB5 RID: 39861 RVA: 0x00105527 File Offset: 0x00103727
	public float GetUserScale()
	{
		return this.userScale;
	}

	// Token: 0x06009BB6 RID: 39862 RVA: 0x0010552F File Offset: 0x0010372F
	public float GetCanvasScale()
	{
		return this.userScale * this.ScreenRelativeScale();
	}

	// Token: 0x06009BB7 RID: 39863 RVA: 0x003C0B0C File Offset: 0x003BED0C
	private float ScreenRelativeScale()
	{
		float dpi = Screen.dpi;
		Camera x = Camera.main;
		if (x == null)
		{
			x = UnityEngine.Object.FindObjectOfType<Camera>();
		}
		x != null;
		if ((float)Screen.height <= this.scaleSteps[0].maxRes_y || (float)Screen.width / (float)Screen.height < 1.6777778f)
		{
			return this.scaleSteps[0].scale;
		}
		if ((float)Screen.height > this.scaleSteps[this.scaleSteps.Length - 1].maxRes_y)
		{
			return this.scaleSteps[this.scaleSteps.Length - 1].scale;
		}
		for (int i = 0; i < this.scaleSteps.Length; i++)
		{
			if ((float)Screen.height > this.scaleSteps[i].maxRes_y && (float)Screen.height <= this.scaleSteps[i + 1].maxRes_y)
			{
				float t = ((float)Screen.height - this.scaleSteps[i].maxRes_y) / (this.scaleSteps[i + 1].maxRes_y - this.scaleSteps[i].maxRes_y);
				return Mathf.Lerp(this.scaleSteps[i].scale, this.scaleSteps[i + 1].scale, t);
			}
		}
		return 1f;
	}

	// Token: 0x040079FA RID: 31226
	[MyCmpReq]
	private CanvasScaler canvasScaler;

	// Token: 0x040079FB RID: 31227
	public static string UIScalePrefKey = "UIScalePref";

	// Token: 0x040079FC RID: 31228
	private float userScale = 1f;

	// Token: 0x040079FD RID: 31229
	[Range(0.75f, 2f)]
	private KCanvasScaler.ScaleStep[] scaleSteps = new KCanvasScaler.ScaleStep[]
	{
		new KCanvasScaler.ScaleStep(720f, 0.86f),
		new KCanvasScaler.ScaleStep(1080f, 1f),
		new KCanvasScaler.ScaleStep(2160f, 1.33f)
	};

	// Token: 0x02001D25 RID: 7461
	[Serializable]
	public struct ScaleStep
	{
		// Token: 0x06009BBA RID: 39866 RVA: 0x0010554A File Offset: 0x0010374A
		public ScaleStep(float maxRes_y, float scale)
		{
			this.maxRes_y = maxRes_y;
			this.scale = scale;
		}

		// Token: 0x040079FE RID: 31230
		public float scale;

		// Token: 0x040079FF RID: 31231
		public float maxRes_y;
	}
}
