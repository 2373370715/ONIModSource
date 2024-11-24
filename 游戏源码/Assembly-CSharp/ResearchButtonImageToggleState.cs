using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ResearchButtonImageToggleState : ImageToggleState
{
	public Image progressBar;

	private KToggle toggle;

	[Header("Scroll Options")]
	public float researchLogoDuration = 5f;

	public float durationPerResearchItemIcon = 0.6f;

	public float fadingDuration = 0.2f;

	private Coroutine scrollIconCoroutine;

	private Sprite[] currentResearchIcons;

	private float mainIconScreenTime;

	private float itemScreenTime;

	private int item_idx = -1;

	private bool ReadyToDisplayIcons
	{
		get
		{
			if (progressBar.enabled && currentResearchIcons != null && item_idx >= 0)
			{
				return item_idx < currentResearchIcons.Length;
			}
			return false;
		}
	}

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Research.Instance.Subscribe(-1914338957, UpdateActiveResearch);
		Research.Instance.Subscribe(-125623018, RefreshProgressBar);
		toggle = GetComponent<KToggle>();
	}

	protected override void OnSpawn()
	{
		base.OnSpawn();
		UpdateActiveResearch(null);
		scrollIconCoroutine = StartCoroutine(ScrollIcon());
	}

	protected override void OnCleanUp()
	{
		StopCoroutine(scrollIconCoroutine);
		Research.Instance.Unsubscribe(-1914338957, RefreshProgressBar);
		Research.Instance.Unsubscribe(-125623018, RefreshProgressBar);
		base.OnCleanUp();
	}

	private void UpdateActiveResearch(object o)
	{
		TechInstance activeResearch = Research.Instance.GetActiveResearch();
		if (activeResearch == null)
		{
			currentResearchIcons = null;
		}
		else
		{
			currentResearchIcons = new Sprite[activeResearch.tech.unlockedItems.Count];
			for (int i = 0; i < activeResearch.tech.unlockedItems.Count; i++)
			{
				TechItem techItem = activeResearch.tech.unlockedItems[i];
				currentResearchIcons[i] = techItem.UISprite();
			}
		}
		ResetCoroutineTimers();
		RefreshProgressBar(o);
	}

	public void RefreshProgressBar(object o)
	{
		TechInstance activeResearch = Research.Instance.GetActiveResearch();
		if (activeResearch == null)
		{
			progressBar.fillAmount = 0f;
		}
		else
		{
			progressBar.fillAmount = activeResearch.GetTotalPercentageComplete();
		}
	}

	public void SetProgressBarVisibility(bool viisble)
	{
		progressBar.enabled = viisble;
	}

	public override void SetActive()
	{
		base.SetActive();
		SetProgressBarVisibility(viisble: false);
	}

	public override void SetDisabledActive()
	{
		base.SetDisabledActive();
		SetProgressBarVisibility(viisble: false);
	}

	public override void SetDisabled()
	{
		base.SetDisabled();
		SetProgressBarVisibility(viisble: false);
	}

	public override void SetInactive()
	{
		base.SetInactive();
		SetProgressBarVisibility(viisble: true);
		RefreshProgressBar(null);
	}

	private void ResetCoroutineTimers()
	{
		mainIconScreenTime = 0f;
		itemScreenTime = 0f;
		item_idx = -1;
	}

	private IEnumerator ScrollIcon()
	{
		while (Application.isPlaying)
		{
			if (mainIconScreenTime < researchLogoDuration)
			{
				toggle.fgImage.Opacity(1f);
				if (toggle.fgImage.overrideSprite != null)
				{
					toggle.fgImage.overrideSprite = null;
				}
				item_idx = 0;
				itemScreenTime = 0f;
				mainIconScreenTime += Time.unscaledDeltaTime;
				if (progressBar.enabled && mainIconScreenTime >= researchLogoDuration && ReadyToDisplayIcons)
				{
					yield return toggle.fgImage.FadeAway(fadingDuration, () => progressBar.enabled && mainIconScreenTime >= researchLogoDuration && ReadyToDisplayIcons);
				}
				yield return null;
			}
			else if (ReadyToDisplayIcons)
			{
				if (toggle.fgImage.overrideSprite != currentResearchIcons[item_idx])
				{
					toggle.fgImage.overrideSprite = currentResearchIcons[item_idx];
				}
				yield return toggle.fgImage.FadeToVisible(fadingDuration, () => ReadyToDisplayIcons);
				while (itemScreenTime < durationPerResearchItemIcon && ReadyToDisplayIcons)
				{
					itemScreenTime += Time.unscaledDeltaTime;
					yield return null;
				}
				yield return toggle.fgImage.FadeAway(fadingDuration, () => ReadyToDisplayIcons);
				if (ReadyToDisplayIcons)
				{
					itemScreenTime = 0f;
					item_idx++;
				}
				yield return null;
			}
			else
			{
				mainIconScreenTime = 0f;
				yield return null;
			}
		}
	}
}
