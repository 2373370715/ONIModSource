using System;
using System.Collections;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001B49 RID: 6985
public class StoryMessageScreen : KScreen
{
	// Token: 0x17000996 RID: 2454
	// (set) Token: 0x060092B1 RID: 37553 RVA: 0x000FFCD3 File Offset: 0x000FDED3
	public string title
	{
		set
		{
			this.titleLabel.SetText(value);
		}
	}

	// Token: 0x17000997 RID: 2455
	// (set) Token: 0x060092B2 RID: 37554 RVA: 0x000FFCE1 File Offset: 0x000FDEE1
	public string body
	{
		set
		{
			this.bodyLabel.SetText(value);
		}
	}

	// Token: 0x060092B3 RID: 37555 RVA: 0x000EAD7D File Offset: 0x000E8F7D
	public override float GetSortKey()
	{
		return 8f;
	}

	// Token: 0x060092B4 RID: 37556 RVA: 0x000FFCEF File Offset: 0x000FDEEF
	protected override void OnSpawn()
	{
		base.OnSpawn();
		StoryMessageScreen.HideInterface(true);
		CameraController.Instance.FadeOut(0.5f, 1f, null);
	}

	// Token: 0x060092B5 RID: 37557 RVA: 0x000FFD12 File Offset: 0x000FDF12
	private IEnumerator ExpandPanel()
	{
		this.content.gameObject.SetActive(true);
		yield return SequenceUtil.WaitForSecondsRealtime(0.25f);
		float height = 0f;
		while (height < 299f)
		{
			height = Mathf.Lerp(this.dialog.rectTransform().sizeDelta.y, 300f, Time.unscaledDeltaTime * 15f);
			this.dialog.rectTransform().sizeDelta = new Vector2(this.dialog.rectTransform().sizeDelta.x, height);
			yield return 0;
		}
		CameraController.Instance.FadeOut(0.5f, 1f, null);
		yield return null;
		yield break;
	}

	// Token: 0x060092B6 RID: 37558 RVA: 0x000FFD21 File Offset: 0x000FDF21
	private IEnumerator CollapsePanel()
	{
		float height = 300f;
		while (height > 0f)
		{
			height = Mathf.Lerp(this.dialog.rectTransform().sizeDelta.y, -1f, Time.unscaledDeltaTime * 15f);
			this.dialog.rectTransform().sizeDelta = new Vector2(this.dialog.rectTransform().sizeDelta.x, height);
			yield return 0;
		}
		this.content.gameObject.SetActive(false);
		if (this.OnClose != null)
		{
			this.OnClose();
			this.OnClose = null;
		}
		this.Deactivate();
		yield return null;
		yield break;
	}

	// Token: 0x060092B7 RID: 37559 RVA: 0x00389FC8 File Offset: 0x003881C8
	public static void HideInterface(bool hide)
	{
		SelectTool.Instance.Select(null, true);
		NotificationScreen.Instance.Show(!hide);
		OverlayMenu.Instance.Show(!hide);
		if (PlanScreen.Instance != null)
		{
			PlanScreen.Instance.Show(!hide);
		}
		if (BuildMenu.Instance != null)
		{
			BuildMenu.Instance.Show(!hide);
		}
		ManagementMenu.Instance.Show(!hide);
		ToolMenu.Instance.Show(!hide);
		ToolMenu.Instance.PriorityScreen.Show(!hide);
		ColonyDiagnosticScreen.Instance.Show(!hide);
		PinnedResourcesPanel.Instance.Show(!hide);
		TopLeftControlScreen.Instance.Show(!hide);
		if (WorldSelector.Instance != null)
		{
			WorldSelector.Instance.Show(!hide);
		}
		global::DateTime.Instance.Show(!hide);
		if (BuildWatermark.Instance != null)
		{
			BuildWatermark.Instance.Show(!hide);
		}
		PopFXManager.Instance.Show(!hide);
	}

	// Token: 0x060092B8 RID: 37560 RVA: 0x0038A0E0 File Offset: 0x003882E0
	public void Update()
	{
		if (!this.startFade)
		{
			return;
		}
		Color color = this.bg.color;
		color.a -= 0.01f;
		if (color.a <= 0f)
		{
			color.a = 0f;
		}
		this.bg.color = color;
	}

	// Token: 0x060092B9 RID: 37561 RVA: 0x0038A138 File Offset: 0x00388338
	protected override void OnActivate()
	{
		base.OnActivate();
		SelectTool.Instance.Select(null, false);
		this.button.onClick += delegate()
		{
			base.StartCoroutine(this.CollapsePanel());
		};
		this.dialog.GetComponent<KScreen>().Show(false);
		this.startFade = false;
		CameraController.Instance.DisableUserCameraControl = true;
		KFMOD.PlayUISound(this.dialogSound);
		this.dialog.GetComponent<KScreen>().Activate();
		this.dialog.GetComponent<KScreen>().SetShouldFadeIn(true);
		this.dialog.GetComponent<KScreen>().Show(true);
		MusicManager.instance.PlaySong("Music_Victory_01_Message", false);
		base.StartCoroutine(this.ExpandPanel());
	}

	// Token: 0x060092BA RID: 37562 RVA: 0x0038A1EC File Offset: 0x003883EC
	protected override void OnDeactivate()
	{
		base.IsActive();
		base.OnDeactivate();
		MusicManager.instance.StopSong("Music_Victory_01_Message", true, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		if (this.restoreInterfaceOnClose)
		{
			CameraController.Instance.DisableUserCameraControl = false;
			CameraController.Instance.FadeIn(0f, 1f, null);
			StoryMessageScreen.HideInterface(false);
		}
	}

	// Token: 0x060092BB RID: 37563 RVA: 0x000FFD30 File Offset: 0x000FDF30
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.Escape))
		{
			base.StartCoroutine(this.CollapsePanel());
		}
		e.Consumed = true;
	}

	// Token: 0x060092BC RID: 37564 RVA: 0x000FDE18 File Offset: 0x000FC018
	public override void OnKeyUp(KButtonEvent e)
	{
		e.Consumed = true;
	}

	// Token: 0x04006EFB RID: 28411
	private const float ALPHA_SPEED = 0.01f;

	// Token: 0x04006EFC RID: 28412
	[SerializeField]
	private Image bg;

	// Token: 0x04006EFD RID: 28413
	[SerializeField]
	private GameObject dialog;

	// Token: 0x04006EFE RID: 28414
	[SerializeField]
	private KButton button;

	// Token: 0x04006EFF RID: 28415
	[SerializeField]
	private EventReference dialogSound;

	// Token: 0x04006F00 RID: 28416
	[SerializeField]
	private LocText titleLabel;

	// Token: 0x04006F01 RID: 28417
	[SerializeField]
	private LocText bodyLabel;

	// Token: 0x04006F02 RID: 28418
	private const float expandedHeight = 300f;

	// Token: 0x04006F03 RID: 28419
	[SerializeField]
	private GameObject content;

	// Token: 0x04006F04 RID: 28420
	public bool restoreInterfaceOnClose = true;

	// Token: 0x04006F05 RID: 28421
	public System.Action OnClose;

	// Token: 0x04006F06 RID: 28422
	private bool startFade;
}
