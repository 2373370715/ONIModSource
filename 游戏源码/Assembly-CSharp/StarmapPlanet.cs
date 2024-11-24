using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02002016 RID: 8214
[AddComponentMenu("KMonoBehaviour/scripts/StarmapPlanet")]
public class StarmapPlanet : KMonoBehaviour
{
	// Token: 0x0600AEAD RID: 44717 RVA: 0x0041A398 File Offset: 0x00418598
	public void SetSprite(Sprite sprite, Color color)
	{
		foreach (StarmapPlanetVisualizer starmapPlanetVisualizer in this.visualizers)
		{
			starmapPlanetVisualizer.image.sprite = sprite;
			starmapPlanetVisualizer.image.color = color;
		}
	}

	// Token: 0x0600AEAE RID: 44718 RVA: 0x0041A3FC File Offset: 0x004185FC
	public void SetFillAmount(float amount)
	{
		foreach (StarmapPlanetVisualizer starmapPlanetVisualizer in this.visualizers)
		{
			starmapPlanetVisualizer.image.fillAmount = amount;
		}
	}

	// Token: 0x0600AEAF RID: 44719 RVA: 0x0041A454 File Offset: 0x00418654
	public void SetUnknownBGActive(bool active, Color color)
	{
		foreach (StarmapPlanetVisualizer starmapPlanetVisualizer in this.visualizers)
		{
			starmapPlanetVisualizer.unknownBG.gameObject.SetActive(active);
			starmapPlanetVisualizer.unknownBG.color = color;
		}
	}

	// Token: 0x0600AEB0 RID: 44720 RVA: 0x0041A4BC File Offset: 0x004186BC
	public void SetSelectionActive(bool active)
	{
		foreach (StarmapPlanetVisualizer starmapPlanetVisualizer in this.visualizers)
		{
			starmapPlanetVisualizer.selection.gameObject.SetActive(active);
		}
	}

	// Token: 0x0600AEB1 RID: 44721 RVA: 0x0041A518 File Offset: 0x00418718
	public void SetAnalysisActive(bool active)
	{
		foreach (StarmapPlanetVisualizer starmapPlanetVisualizer in this.visualizers)
		{
			starmapPlanetVisualizer.analysisSelection.SetActive(active);
		}
	}

	// Token: 0x0600AEB2 RID: 44722 RVA: 0x0041A570 File Offset: 0x00418770
	public void SetLabel(string text)
	{
		foreach (StarmapPlanetVisualizer starmapPlanetVisualizer in this.visualizers)
		{
			starmapPlanetVisualizer.label.text = text;
			this.ShowLabel(false);
		}
	}

	// Token: 0x0600AEB3 RID: 44723 RVA: 0x0041A5D0 File Offset: 0x004187D0
	public void ShowLabel(bool show)
	{
		foreach (StarmapPlanetVisualizer starmapPlanetVisualizer in this.visualizers)
		{
			starmapPlanetVisualizer.label.gameObject.SetActive(show);
		}
	}

	// Token: 0x0600AEB4 RID: 44724 RVA: 0x0041A62C File Offset: 0x0041882C
	public void SetOnClick(System.Action del)
	{
		foreach (StarmapPlanetVisualizer starmapPlanetVisualizer in this.visualizers)
		{
			starmapPlanetVisualizer.button.onClick = del;
		}
	}

	// Token: 0x0600AEB5 RID: 44725 RVA: 0x0041A684 File Offset: 0x00418884
	public void SetOnEnter(System.Action del)
	{
		foreach (StarmapPlanetVisualizer starmapPlanetVisualizer in this.visualizers)
		{
			starmapPlanetVisualizer.button.onEnter = del;
		}
	}

	// Token: 0x0600AEB6 RID: 44726 RVA: 0x0041A6DC File Offset: 0x004188DC
	public void SetOnExit(System.Action del)
	{
		foreach (StarmapPlanetVisualizer starmapPlanetVisualizer in this.visualizers)
		{
			starmapPlanetVisualizer.button.onExit = del;
		}
	}

	// Token: 0x0600AEB7 RID: 44727 RVA: 0x0041A734 File Offset: 0x00418934
	public void AnimateSelector(float time)
	{
		foreach (StarmapPlanetVisualizer starmapPlanetVisualizer in this.visualizers)
		{
			starmapPlanetVisualizer.selection.anchoredPosition = new Vector2(0f, 25f + Mathf.Sin(time * 4f) * 5f);
		}
	}

	// Token: 0x0600AEB8 RID: 44728 RVA: 0x0041A7AC File Offset: 0x004189AC
	public void ShowAsCurrentRocketDestination(bool show)
	{
		foreach (StarmapPlanetVisualizer starmapPlanetVisualizer in this.visualizers)
		{
			RectTransform rectTransform = starmapPlanetVisualizer.rocketIconContainer.rectTransform();
			if (rectTransform.childCount > 0)
			{
				rectTransform.GetChild(rectTransform.childCount - 1).GetComponent<HierarchyReferences>().GetReference<Image>("fg").color = (show ? new Color(0.11764706f, 0.8627451f, 0.3137255f) : Color.white);
			}
		}
	}

	// Token: 0x0600AEB9 RID: 44729 RVA: 0x0041A84C File Offset: 0x00418A4C
	public void SetRocketIcons(int numRockets, GameObject iconPrefab)
	{
		foreach (StarmapPlanetVisualizer starmapPlanetVisualizer in this.visualizers)
		{
			RectTransform rectTransform = starmapPlanetVisualizer.rocketIconContainer.rectTransform();
			for (int i = rectTransform.childCount; i < numRockets; i++)
			{
				Util.KInstantiateUI(iconPrefab, starmapPlanetVisualizer.rocketIconContainer, true);
			}
			for (int j = rectTransform.childCount; j > numRockets; j--)
			{
				UnityEngine.Object.Destroy(rectTransform.GetChild(j - 1).gameObject);
			}
			int num = 0;
			foreach (object obj in rectTransform)
			{
				((RectTransform)obj).anchoredPosition = new Vector2((float)num * -10f, 0f);
				num++;
			}
		}
	}

	// Token: 0x0400895E RID: 35166
	public List<StarmapPlanetVisualizer> visualizers;
}
