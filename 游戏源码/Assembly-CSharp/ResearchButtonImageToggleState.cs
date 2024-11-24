using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001B38 RID: 6968
public class ResearchButtonImageToggleState : ImageToggleState
{
	// Token: 0x0600922A RID: 37418 RVA: 0x003860C8 File Offset: 0x003842C8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Research.Instance.Subscribe(-1914338957, new Action<object>(this.UpdateActiveResearch));
		Research.Instance.Subscribe(-125623018, new Action<object>(this.RefreshProgressBar));
		this.toggle = base.GetComponent<KToggle>();
	}

	// Token: 0x0600922B RID: 37419 RVA: 0x000FF730 File Offset: 0x000FD930
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.UpdateActiveResearch(null);
		this.RestartCoroutine();
	}

	// Token: 0x0600922C RID: 37420 RVA: 0x00386120 File Offset: 0x00384320
	protected override void OnCleanUp()
	{
		this.AbortCoroutine();
		Research.Instance.Unsubscribe(-1914338957, new Action<object>(this.UpdateActiveResearch));
		Research.Instance.Unsubscribe(-125623018, new Action<object>(this.RefreshProgressBar));
		base.OnCleanUp();
	}

	// Token: 0x0600922D RID: 37421 RVA: 0x000FF745 File Offset: 0x000FD945
	protected override void OnCmpEnable()
	{
		base.OnCmpEnable();
		this.RestartCoroutine();
	}

	// Token: 0x0600922E RID: 37422 RVA: 0x000FF753 File Offset: 0x000FD953
	protected override void OnCmpDisable()
	{
		base.OnCmpDisable();
		this.AbortCoroutine();
	}

	// Token: 0x0600922F RID: 37423 RVA: 0x000FF761 File Offset: 0x000FD961
	private void AbortCoroutine()
	{
		if (this.scrollIconCoroutine != null)
		{
			base.StopCoroutine(this.scrollIconCoroutine);
		}
		this.scrollIconCoroutine = null;
	}

	// Token: 0x06009230 RID: 37424 RVA: 0x000FF77E File Offset: 0x000FD97E
	private void RestartCoroutine()
	{
		this.AbortCoroutine();
		if (base.gameObject.activeInHierarchy)
		{
			this.scrollIconCoroutine = base.StartCoroutine(this.ScrollIcon());
		}
	}

	// Token: 0x06009231 RID: 37425 RVA: 0x00386170 File Offset: 0x00384370
	private void UpdateActiveResearch(object o)
	{
		TechInstance activeResearch = Research.Instance.GetActiveResearch();
		if (activeResearch == null)
		{
			this.currentResearchIcons = null;
		}
		else
		{
			this.currentResearchIcons = new Sprite[activeResearch.tech.unlockedItems.Count];
			for (int i = 0; i < activeResearch.tech.unlockedItems.Count; i++)
			{
				TechItem techItem = activeResearch.tech.unlockedItems[i];
				this.currentResearchIcons[i] = techItem.UISprite();
			}
		}
		this.ResetCoroutineTimers();
		this.RefreshProgressBar(o);
	}

	// Token: 0x06009232 RID: 37426 RVA: 0x003861F8 File Offset: 0x003843F8
	public void RefreshProgressBar(object o)
	{
		TechInstance activeResearch = Research.Instance.GetActiveResearch();
		if (activeResearch == null)
		{
			this.progressBar.fillAmount = 0f;
			return;
		}
		this.progressBar.fillAmount = activeResearch.GetTotalPercentageComplete();
	}

	// Token: 0x06009233 RID: 37427 RVA: 0x000FF7A5 File Offset: 0x000FD9A5
	public void SetProgressBarVisibility(bool viisble)
	{
		this.progressBar.enabled = viisble;
	}

	// Token: 0x06009234 RID: 37428 RVA: 0x000FF7B3 File Offset: 0x000FD9B3
	public override void SetActive()
	{
		base.SetActive();
		this.SetProgressBarVisibility(false);
	}

	// Token: 0x06009235 RID: 37429 RVA: 0x000FF7C2 File Offset: 0x000FD9C2
	public override void SetDisabledActive()
	{
		base.SetDisabledActive();
		this.SetProgressBarVisibility(false);
	}

	// Token: 0x06009236 RID: 37430 RVA: 0x000FF7D1 File Offset: 0x000FD9D1
	public override void SetDisabled()
	{
		base.SetDisabled();
		this.SetProgressBarVisibility(false);
	}

	// Token: 0x06009237 RID: 37431 RVA: 0x000FF7E0 File Offset: 0x000FD9E0
	public override void SetInactive()
	{
		base.SetInactive();
		this.SetProgressBarVisibility(true);
		this.RefreshProgressBar(null);
	}

	// Token: 0x06009238 RID: 37432 RVA: 0x000FF7F6 File Offset: 0x000FD9F6
	private void ResetCoroutineTimers()
	{
		this.mainIconScreenTime = 0f;
		this.itemScreenTime = 0f;
		this.item_idx = -1;
	}

	// Token: 0x17000992 RID: 2450
	// (get) Token: 0x06009239 RID: 37433 RVA: 0x000FF815 File Offset: 0x000FDA15
	private bool ReadyToDisplayIcons
	{
		get
		{
			return this.progressBar.enabled && this.currentResearchIcons != null && this.item_idx >= 0 && this.item_idx < this.currentResearchIcons.Length;
		}
	}

	// Token: 0x0600923A RID: 37434 RVA: 0x000FF847 File Offset: 0x000FDA47
	private IEnumerator ScrollIcon()
	{
		while (Application.isPlaying)
		{
			if (this.mainIconScreenTime < this.researchLogoDuration)
			{
				this.toggle.fgImage.Opacity(1f);
				if (this.toggle.fgImage.overrideSprite != null)
				{
					this.toggle.fgImage.overrideSprite = null;
				}
				this.item_idx = 0;
				this.itemScreenTime = 0f;
				this.mainIconScreenTime += Time.unscaledDeltaTime;
				if (this.progressBar.enabled && this.mainIconScreenTime >= this.researchLogoDuration && this.ReadyToDisplayIcons)
				{
					yield return this.toggle.fgImage.FadeAway(this.fadingDuration, () => this.progressBar.enabled && this.mainIconScreenTime >= this.researchLogoDuration && this.ReadyToDisplayIcons);
				}
				yield return null;
			}
			else if (this.ReadyToDisplayIcons)
			{
				if (this.toggle.fgImage.overrideSprite != this.currentResearchIcons[this.item_idx])
				{
					this.toggle.fgImage.overrideSprite = this.currentResearchIcons[this.item_idx];
				}
				yield return this.toggle.fgImage.FadeToVisible(this.fadingDuration, () => this.ReadyToDisplayIcons);
				while (this.itemScreenTime < this.durationPerResearchItemIcon && this.ReadyToDisplayIcons)
				{
					this.itemScreenTime += Time.unscaledDeltaTime;
					yield return null;
				}
				yield return this.toggle.fgImage.FadeAway(this.fadingDuration, () => this.ReadyToDisplayIcons);
				if (this.ReadyToDisplayIcons)
				{
					this.itemScreenTime = 0f;
					this.item_idx++;
				}
				yield return null;
			}
			else
			{
				this.mainIconScreenTime = 0f;
				yield return null;
			}
		}
		yield break;
	}

	// Token: 0x04006E91 RID: 28305
	public Image progressBar;

	// Token: 0x04006E92 RID: 28306
	private KToggle toggle;

	// Token: 0x04006E93 RID: 28307
	[Header("Scroll Options")]
	public float researchLogoDuration = 5f;

	// Token: 0x04006E94 RID: 28308
	public float durationPerResearchItemIcon = 0.6f;

	// Token: 0x04006E95 RID: 28309
	public float fadingDuration = 0.2f;

	// Token: 0x04006E96 RID: 28310
	private Coroutine scrollIconCoroutine;

	// Token: 0x04006E97 RID: 28311
	private Sprite[] currentResearchIcons;

	// Token: 0x04006E98 RID: 28312
	private float mainIconScreenTime;

	// Token: 0x04006E99 RID: 28313
	private float itemScreenTime;

	// Token: 0x04006E9A RID: 28314
	private int item_idx = -1;
}
