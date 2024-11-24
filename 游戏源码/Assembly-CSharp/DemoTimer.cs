using System;
using Klei;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001C9C RID: 7324
public class DemoTimer : MonoBehaviour
{
	// Token: 0x060098CE RID: 39118 RVA: 0x00103743 File Offset: 0x00101943
	public static void DestroyInstance()
	{
		DemoTimer.Instance = null;
	}

	// Token: 0x060098CF RID: 39119 RVA: 0x003B1B00 File Offset: 0x003AFD00
	private void Start()
	{
		DemoTimer.Instance = this;
		if (GenericGameSettings.instance != null)
		{
			if (GenericGameSettings.instance.demoMode)
			{
				this.duration = (float)GenericGameSettings.instance.demoTime;
				this.labelText.gameObject.SetActive(GenericGameSettings.instance.showDemoTimer);
				this.clockImage.gameObject.SetActive(GenericGameSettings.instance.showDemoTimer);
			}
			else
			{
				base.gameObject.SetActive(false);
			}
		}
		else
		{
			base.gameObject.SetActive(false);
		}
		this.duration = (float)GenericGameSettings.instance.demoTime;
		this.fadeOutScreen = Util.KInstantiateUI(this.Prefab_FadeOutScreen, GameScreenManager.Instance.ssOverlayCanvas.gameObject, false);
		Image component = this.fadeOutScreen.GetComponent<Image>();
		component.raycastTarget = false;
		this.fadeOutColor = component.color;
		this.fadeOutColor.a = 0f;
		this.fadeOutScreen.GetComponent<Image>().color = this.fadeOutColor;
	}

	// Token: 0x060098D0 RID: 39120 RVA: 0x003B1C00 File Offset: 0x003AFE00
	private void Update()
	{
		if ((Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)) && Input.GetKeyDown(KeyCode.BackQuote))
		{
			this.CountdownActive = !this.CountdownActive;
			this.UpdateLabel();
		}
		if (this.demoOver || !this.CountdownActive)
		{
			return;
		}
		if (this.beginTime == -1f)
		{
			this.beginTime = Time.unscaledTime;
		}
		this.elapsed = Mathf.Clamp(0f, Time.unscaledTime - this.beginTime, this.duration);
		if (this.elapsed + 5f >= this.duration)
		{
			float f = (this.duration - this.elapsed) / 5f;
			this.fadeOutColor.a = Mathf.Min(1f, 1f - Mathf.Sqrt(f));
			this.fadeOutScreen.GetComponent<Image>().color = this.fadeOutColor;
		}
		if (this.elapsed >= this.duration)
		{
			this.EndDemo();
		}
		this.UpdateLabel();
	}

	// Token: 0x060098D1 RID: 39121 RVA: 0x003B1D08 File Offset: 0x003AFF08
	private void UpdateLabel()
	{
		int num = Mathf.RoundToInt(this.duration - this.elapsed);
		int num2 = Mathf.FloorToInt((float)(num / 60));
		int num3 = num % 60;
		this.labelText.text = string.Concat(new string[]
		{
			UI.DEMOOVERSCREEN.TIMEREMAINING,
			" ",
			num2.ToString("00"),
			":",
			num3.ToString("00")
		});
		if (!this.CountdownActive)
		{
			this.labelText.text = UI.DEMOOVERSCREEN.TIMERINACTIVE;
		}
	}

	// Token: 0x060098D2 RID: 39122 RVA: 0x0010374B File Offset: 0x0010194B
	public void EndDemo()
	{
		if (this.demoOver)
		{
			return;
		}
		this.demoOver = true;
		Util.KInstantiateUI(this.Prefab_DemoOverScreen, GameScreenManager.Instance.ssOverlayCanvas.gameObject, false).GetComponent<DemoOverScreen>().Show(true);
	}

	// Token: 0x04007709 RID: 30473
	public static DemoTimer Instance;

	// Token: 0x0400770A RID: 30474
	public LocText labelText;

	// Token: 0x0400770B RID: 30475
	public Image clockImage;

	// Token: 0x0400770C RID: 30476
	public GameObject Prefab_DemoOverScreen;

	// Token: 0x0400770D RID: 30477
	public GameObject Prefab_FadeOutScreen;

	// Token: 0x0400770E RID: 30478
	private float duration;

	// Token: 0x0400770F RID: 30479
	private float elapsed;

	// Token: 0x04007710 RID: 30480
	private bool demoOver;

	// Token: 0x04007711 RID: 30481
	private float beginTime = -1f;

	// Token: 0x04007712 RID: 30482
	public bool CountdownActive;

	// Token: 0x04007713 RID: 30483
	private GameObject fadeOutScreen;

	// Token: 0x04007714 RID: 30484
	private Color fadeOutColor;
}
