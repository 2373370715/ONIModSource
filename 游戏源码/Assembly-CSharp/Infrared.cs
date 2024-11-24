using System;
using UnityEngine;

// Token: 0x02001A8C RID: 6796
public class Infrared : MonoBehaviour
{
	// Token: 0x06008E0A RID: 36362 RVA: 0x000FCC21 File Offset: 0x000FAE21
	public static void DestroyInstance()
	{
		Infrared.Instance = null;
	}

	// Token: 0x06008E0B RID: 36363 RVA: 0x000FCC29 File Offset: 0x000FAE29
	private void Awake()
	{
		Infrared.temperatureParametersId = Shader.PropertyToID("_TemperatureParameters");
		Infrared.Instance = this;
		this.OnResize();
		this.UpdateState();
	}

	// Token: 0x06008E0C RID: 36364 RVA: 0x000FCC4C File Offset: 0x000FAE4C
	private void OnRenderImage(RenderTexture source, RenderTexture dest)
	{
		Graphics.Blit(source, this.minionTexture);
		Graphics.Blit(source, dest);
	}

	// Token: 0x06008E0D RID: 36365 RVA: 0x0036FA98 File Offset: 0x0036DC98
	private void OnResize()
	{
		if (this.minionTexture != null)
		{
			this.minionTexture.DestroyRenderTexture();
		}
		if (this.cameraTexture != null)
		{
			this.cameraTexture.DestroyRenderTexture();
		}
		int num = 2;
		this.minionTexture = new RenderTexture(Screen.width / num, Screen.height / num, 0, RenderTextureFormat.ARGB32);
		this.cameraTexture = new RenderTexture(Screen.width / num, Screen.height / num, 0, RenderTextureFormat.ARGB32);
		base.GetComponent<Camera>().targetTexture = this.cameraTexture;
	}

	// Token: 0x06008E0E RID: 36366 RVA: 0x0036FB20 File Offset: 0x0036DD20
	public void SetMode(Infrared.Mode mode)
	{
		Vector4 zero;
		if (mode != Infrared.Mode.Disabled)
		{
			if (mode != Infrared.Mode.Disease)
			{
				zero = new Vector4(1f, 0f, 0f, 0f);
			}
			else
			{
				zero = new Vector4(1f, 0f, 0f, 0f);
				GameComps.InfraredVisualizers.ClearOverlayColour();
			}
		}
		else
		{
			zero = Vector4.zero;
		}
		Shader.SetGlobalVector("_ColouredOverlayParameters", zero);
		this.mode = mode;
		this.UpdateState();
	}

	// Token: 0x06008E0F RID: 36367 RVA: 0x000FCC61 File Offset: 0x000FAE61
	private void UpdateState()
	{
		base.gameObject.SetActive(this.mode > Infrared.Mode.Disabled);
		if (base.gameObject.activeSelf)
		{
			this.Update();
		}
	}

	// Token: 0x06008E10 RID: 36368 RVA: 0x0036FB98 File Offset: 0x0036DD98
	private void Update()
	{
		switch (this.mode)
		{
		case Infrared.Mode.Disabled:
			break;
		case Infrared.Mode.Infrared:
			GameComps.InfraredVisualizers.UpdateTemperature();
			return;
		case Infrared.Mode.Disease:
			GameComps.DiseaseContainers.UpdateOverlayColours();
			break;
		default:
			return;
		}
	}

	// Token: 0x04006ABF RID: 27327
	private RenderTexture minionTexture;

	// Token: 0x04006AC0 RID: 27328
	private RenderTexture cameraTexture;

	// Token: 0x04006AC1 RID: 27329
	private Infrared.Mode mode;

	// Token: 0x04006AC2 RID: 27330
	public static int temperatureParametersId;

	// Token: 0x04006AC3 RID: 27331
	public static Infrared Instance;

	// Token: 0x02001A8D RID: 6797
	public enum Mode
	{
		// Token: 0x04006AC5 RID: 27333
		Disabled,
		// Token: 0x04006AC6 RID: 27334
		Infrared,
		// Token: 0x04006AC7 RID: 27335
		Disease
	}
}
