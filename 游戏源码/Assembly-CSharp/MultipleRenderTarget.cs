using System;
using System.Collections;
using UnityEngine;

// Token: 0x02001789 RID: 6025
public class MultipleRenderTarget : MonoBehaviour
{
	// Token: 0x14000022 RID: 34
	// (add) Token: 0x06007BED RID: 31725 RVA: 0x0031E8B4 File Offset: 0x0031CAB4
	// (remove) Token: 0x06007BEE RID: 31726 RVA: 0x0031E8EC File Offset: 0x0031CAEC
	public event Action<Camera> onSetupComplete;

	// Token: 0x06007BEF RID: 31727 RVA: 0x000F1A02 File Offset: 0x000EFC02
	private void Start()
	{
		base.StartCoroutine(this.SetupProxy());
	}

	// Token: 0x06007BF0 RID: 31728 RVA: 0x000F1A11 File Offset: 0x000EFC11
	private IEnumerator SetupProxy()
	{
		yield return null;
		Camera component = base.GetComponent<Camera>();
		Camera camera = new GameObject().AddComponent<Camera>();
		camera.CopyFrom(component);
		this.renderProxy = camera.gameObject.AddComponent<MultipleRenderTargetProxy>();
		camera.name = component.name + " MRT";
		camera.transform.parent = component.transform;
		camera.transform.SetLocalPosition(Vector3.zero);
		camera.depth = component.depth - 1f;
		component.cullingMask = 0;
		component.clearFlags = CameraClearFlags.Color;
		this.quad = new FullScreenQuad("MultipleRenderTarget", component, true);
		if (this.onSetupComplete != null)
		{
			this.onSetupComplete(camera);
		}
		yield break;
	}

	// Token: 0x06007BF1 RID: 31729 RVA: 0x000F1A20 File Offset: 0x000EFC20
	private void OnPreCull()
	{
		if (this.renderProxy != null)
		{
			this.quad.Draw(this.renderProxy.Textures[0]);
		}
	}

	// Token: 0x06007BF2 RID: 31730 RVA: 0x000F1A48 File Offset: 0x000EFC48
	public void ToggleColouredOverlayView(bool enabled)
	{
		if (this.renderProxy != null)
		{
			this.renderProxy.ToggleColouredOverlayView(enabled);
		}
	}

	// Token: 0x04005DB1 RID: 23985
	private MultipleRenderTargetProxy renderProxy;

	// Token: 0x04005DB2 RID: 23986
	private FullScreenQuad quad;

	// Token: 0x04005DB4 RID: 23988
	public bool isFrontEnd;
}
