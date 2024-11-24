using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

// Token: 0x02001ECA RID: 7882
[AddComponentMenu("KMonoBehaviour/scripts/ResearchEntry")]
public class ResearchEntry : KMonoBehaviour
{
	// Token: 0x0600A59E RID: 42398 RVA: 0x003ED460 File Offset: 0x003EB660
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.techLineMap = new Dictionary<Tech, UILineRenderer>();
		this.BG.color = this.defaultColor;
		foreach (Tech tech in this.targetTech.requiredTech)
		{
			float num = this.targetTech.width / 2f + 18f;
			Vector2 zero = Vector2.zero;
			Vector2 zero2 = Vector2.zero;
			if (tech.center.y > this.targetTech.center.y + 2f)
			{
				zero = new Vector2(0f, 20f);
				zero2 = new Vector2(0f, -20f);
			}
			else if (tech.center.y < this.targetTech.center.y - 2f)
			{
				zero = new Vector2(0f, -20f);
				zero2 = new Vector2(0f, 20f);
			}
			UILineRenderer component = Util.KInstantiateUI(this.linePrefab, this.lineContainer.gameObject, true).GetComponent<UILineRenderer>();
			float num2 = 32f;
			component.Points = new Vector2[]
			{
				new Vector2(0f, 0f) + zero,
				new Vector2(-num2, 0f) + zero,
				new Vector2(-num2, tech.center.y - this.targetTech.center.y) + zero2,
				new Vector2(-(this.targetTech.center.x - num - (tech.center.x + num)) + 2f, tech.center.y - this.targetTech.center.y) + zero2
			};
			component.LineThickness = (float)this.lineThickness_inactive;
			component.color = this.inactiveLineColor;
			this.techLineMap.Add(tech, component);
		}
		this.QueueStateChanged(false);
		if (this.targetTech != null)
		{
			using (List<TechInstance>.Enumerator enumerator2 = Research.Instance.GetResearchQueue().GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					if (enumerator2.Current.tech == this.targetTech)
					{
						this.QueueStateChanged(true);
					}
				}
			}
		}
	}

	// Token: 0x0600A59F RID: 42399 RVA: 0x003ED720 File Offset: 0x003EB920
	public void SetTech(Tech newTech)
	{
		if (newTech == null)
		{
			global::Debug.LogError("The research provided is null!");
			return;
		}
		if (this.targetTech == newTech)
		{
			return;
		}
		foreach (ResearchType researchType in Research.Instance.researchTypes.Types)
		{
			if (newTech.costsByResearchTypeID.ContainsKey(researchType.id) && newTech.costsByResearchTypeID[researchType.id] > 0f)
			{
				GameObject gameObject = Util.KInstantiateUI(this.progressBarPrefab, this.progressBarContainer.gameObject, true);
				Image image = gameObject.GetComponentsInChildren<Image>()[2];
				Image component = gameObject.transform.Find("Icon").GetComponent<Image>();
				image.color = researchType.color;
				component.sprite = researchType.sprite;
				this.progressBarsByResearchTypeID[researchType.id] = gameObject;
			}
		}
		if (this.researchScreen == null)
		{
			this.researchScreen = base.transform.parent.GetComponentInParent<ResearchScreen>();
		}
		if (newTech.IsComplete())
		{
			this.ResearchCompleted(false);
		}
		this.targetTech = newTech;
		this.researchName.text = this.targetTech.Name;
		string text = "";
		foreach (TechItem techItem in this.targetTech.unlockedItems)
		{
			if (SaveLoader.Instance.IsCorrectDlcActiveForCurrentSave(techItem.requiredDlcIds, techItem.forbiddenDlcIds))
			{
				HierarchyReferences component2 = this.GetFreeIcon().GetComponent<HierarchyReferences>();
				if (text != "")
				{
					text += ", ";
				}
				text += techItem.Name;
				component2.GetReference<KImage>("Icon").sprite = techItem.UISprite();
				component2.GetReference<KImage>("Background");
				KImage reference = component2.GetReference<KImage>("DLCOverlay");
				bool flag = techItem.requiredDlcIds != null;
				reference.gameObject.SetActive(flag);
				if (flag)
				{
					reference.color = DlcManager.GetDlcBannerColor(techItem.requiredDlcIds[techItem.requiredDlcIds.Length - 1]);
				}
				string text2 = string.Format("{0}\n{1}", techItem.Name, techItem.description);
				if (flag)
				{
					text2 += "\n";
					foreach (string dlcId in techItem.requiredDlcIds)
					{
						text2 += string.Format(RESEARCH.MESSAGING.DLC.DLC_CONTENT, DlcManager.GetDlcTitle(dlcId));
					}
				}
				component2.GetComponent<ToolTip>().toolTip = text2;
			}
		}
		text = string.Format(UI.RESEARCHSCREEN_UNLOCKSTOOLTIP, text);
		this.researchName.GetComponent<ToolTip>().toolTip = string.Format("{0}\n{1}\n\n{2}", this.targetTech.Name, this.targetTech.desc, text);
		this.toggle.ClearOnClick();
		this.toggle.onClick += this.OnResearchClicked;
		this.toggle.onPointerEnter += delegate()
		{
			this.researchScreen.TurnEverythingOff();
			this.OnHover(true, this.targetTech);
		};
		this.toggle.soundPlayer.AcceptClickCondition = (() => !this.targetTech.IsComplete());
		this.toggle.onPointerExit += delegate()
		{
			this.researchScreen.TurnEverythingOff();
		};
	}

	// Token: 0x0600A5A0 RID: 42400 RVA: 0x003EDAC0 File Offset: 0x003EBCC0
	public void SetEverythingOff()
	{
		if (!this.isOn)
		{
			return;
		}
		this.borderHighlight.gameObject.SetActive(false);
		foreach (KeyValuePair<Tech, UILineRenderer> keyValuePair in this.techLineMap)
		{
			keyValuePair.Value.LineThickness = (float)this.lineThickness_inactive;
			keyValuePair.Value.color = this.inactiveLineColor;
		}
		this.isOn = false;
	}

	// Token: 0x0600A5A1 RID: 42401 RVA: 0x003EDB54 File Offset: 0x003EBD54
	public void SetEverythingOn()
	{
		if (this.isOn)
		{
			return;
		}
		this.UpdateProgressBars();
		this.borderHighlight.gameObject.SetActive(true);
		foreach (KeyValuePair<Tech, UILineRenderer> keyValuePair in this.techLineMap)
		{
			keyValuePair.Value.LineThickness = (float)this.lineThickness_active;
			keyValuePair.Value.color = this.activeLineColor;
		}
		base.transform.SetAsLastSibling();
		this.isOn = true;
	}

	// Token: 0x0600A5A2 RID: 42402 RVA: 0x003EDBF8 File Offset: 0x003EBDF8
	public void OnHover(bool entered, Tech hoverSource)
	{
		this.SetEverythingOn();
		foreach (Tech tech in this.targetTech.requiredTech)
		{
			ResearchEntry entry = this.researchScreen.GetEntry(tech);
			if (entry != null)
			{
				entry.OnHover(entered, this.targetTech);
			}
		}
	}

	// Token: 0x0600A5A3 RID: 42403 RVA: 0x003EDC74 File Offset: 0x003EBE74
	private void OnResearchClicked()
	{
		TechInstance activeResearch = Research.Instance.GetActiveResearch();
		if (activeResearch != null && activeResearch.tech != this.targetTech)
		{
			this.researchScreen.CancelResearch();
		}
		Research.Instance.SetActiveResearch(this.targetTech, true);
		if (DebugHandler.InstantBuildMode)
		{
			Research.Instance.CompleteQueue();
		}
		this.UpdateProgressBars();
	}

	// Token: 0x0600A5A4 RID: 42404 RVA: 0x003EDCD0 File Offset: 0x003EBED0
	private void OnResearchCanceled()
	{
		if (this.targetTech.IsComplete())
		{
			return;
		}
		this.toggle.ClearOnClick();
		this.toggle.onClick += this.OnResearchClicked;
		this.researchScreen.CancelResearch();
		Research.Instance.CancelResearch(this.targetTech, true);
	}

	// Token: 0x0600A5A5 RID: 42405 RVA: 0x003EDD2C File Offset: 0x003EBF2C
	public void QueueStateChanged(bool isSelected)
	{
		if (isSelected)
		{
			if (!this.targetTech.IsComplete())
			{
				this.toggle.isOn = true;
				this.BG.color = this.pendingColor;
				this.titleBG.color = this.pendingHeaderColor;
				this.toggle.ClearOnClick();
				this.toggle.onClick += this.OnResearchCanceled;
			}
			else
			{
				this.toggle.isOn = false;
			}
			foreach (KeyValuePair<string, GameObject> keyValuePair in this.progressBarsByResearchTypeID)
			{
				keyValuePair.Value.transform.GetChild(0).GetComponentsInChildren<Image>()[1].color = Color.white;
			}
			Image[] componentsInChildren = this.iconPanel.GetComponentsInChildren<Image>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].material = this.StandardUIMaterial;
			}
			return;
		}
		if (this.targetTech.IsComplete())
		{
			this.toggle.isOn = false;
			this.BG.color = this.completedColor;
			this.titleBG.color = this.completedHeaderColor;
			this.defaultColor = this.completedColor;
			this.toggle.ClearOnClick();
			foreach (KeyValuePair<string, GameObject> keyValuePair2 in this.progressBarsByResearchTypeID)
			{
				keyValuePair2.Value.transform.GetChild(0).GetComponentsInChildren<Image>()[1].color = Color.white;
			}
			Image[] componentsInChildren = this.iconPanel.GetComponentsInChildren<Image>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].material = this.StandardUIMaterial;
			}
			return;
		}
		this.toggle.isOn = false;
		this.BG.color = this.defaultColor;
		this.titleBG.color = this.incompleteHeaderColor;
		this.toggle.ClearOnClick();
		this.toggle.onClick += this.OnResearchClicked;
		foreach (KeyValuePair<string, GameObject> keyValuePair3 in this.progressBarsByResearchTypeID)
		{
			keyValuePair3.Value.transform.GetChild(0).GetComponentsInChildren<Image>()[1].color = new Color(0.52156866f, 0.52156866f, 0.52156866f);
		}
	}

	// Token: 0x0600A5A6 RID: 42406 RVA: 0x003EDFD0 File Offset: 0x003EC1D0
	public void UpdateFilterState(bool state)
	{
		this.filterLowlight.gameObject.SetActive(!state);
	}

	// Token: 0x0600A5A7 RID: 42407 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void SetPercentage(float percent)
	{
	}

	// Token: 0x0600A5A8 RID: 42408 RVA: 0x003EDFF4 File Offset: 0x003EC1F4
	public void UpdateProgressBars()
	{
		foreach (KeyValuePair<string, GameObject> keyValuePair in this.progressBarsByResearchTypeID)
		{
			Transform child = keyValuePair.Value.transform.GetChild(0);
			float fillAmount;
			if (this.targetTech.IsComplete())
			{
				fillAmount = 1f;
				child.GetComponentInChildren<LocText>().text = this.targetTech.costsByResearchTypeID[keyValuePair.Key].ToString() + "/" + this.targetTech.costsByResearchTypeID[keyValuePair.Key].ToString();
			}
			else
			{
				TechInstance orAdd = Research.Instance.GetOrAdd(this.targetTech);
				if (orAdd == null)
				{
					continue;
				}
				child.GetComponentInChildren<LocText>().text = orAdd.progressInventory.PointsByTypeID[keyValuePair.Key].ToString() + "/" + this.targetTech.costsByResearchTypeID[keyValuePair.Key].ToString();
				fillAmount = orAdd.progressInventory.PointsByTypeID[keyValuePair.Key] / this.targetTech.costsByResearchTypeID[keyValuePair.Key];
			}
			child.GetComponentsInChildren<Image>()[2].fillAmount = fillAmount;
			child.GetComponent<ToolTip>().SetSimpleTooltip(Research.Instance.researchTypes.GetResearchType(keyValuePair.Key).description);
		}
	}

	// Token: 0x0600A5A9 RID: 42409 RVA: 0x0010B82B File Offset: 0x00109A2B
	private GameObject GetFreeIcon()
	{
		GameObject gameObject = Util.KInstantiateUI(this.iconPrefab, this.iconPanel, false);
		gameObject.SetActive(true);
		return gameObject;
	}

	// Token: 0x0600A5AA RID: 42410 RVA: 0x0010B846 File Offset: 0x00109A46
	private Image GetFreeLine()
	{
		return Util.KInstantiateUI<Image>(this.linePrefab.gameObject, base.gameObject, false);
	}

	// Token: 0x0600A5AB RID: 42411 RVA: 0x003EE1AC File Offset: 0x003EC3AC
	public void ResearchCompleted(bool notify = true)
	{
		this.BG.color = this.completedColor;
		this.titleBG.color = this.completedHeaderColor;
		this.defaultColor = this.completedColor;
		if (notify)
		{
			this.unlockedTechMetric[ResearchEntry.UnlockedTechKey] = this.targetTech.Id;
			ThreadedHttps<KleiMetrics>.Instance.SendEvent(this.unlockedTechMetric, "ResearchCompleted");
		}
		this.toggle.ClearOnClick();
		if (notify)
		{
			ResearchCompleteMessage message = new ResearchCompleteMessage(this.targetTech);
			MusicManager.instance.PlaySong("Stinger_ResearchComplete", false);
			Messenger.Instance.QueueMessage(message);
		}
	}

	// Token: 0x040081CB RID: 33227
	[Header("Labels")]
	[SerializeField]
	private LocText researchName;

	// Token: 0x040081CC RID: 33228
	[Header("Transforms")]
	[SerializeField]
	private Transform progressBarContainer;

	// Token: 0x040081CD RID: 33229
	[SerializeField]
	private Transform lineContainer;

	// Token: 0x040081CE RID: 33230
	[Header("Prefabs")]
	[SerializeField]
	private GameObject iconPanel;

	// Token: 0x040081CF RID: 33231
	[SerializeField]
	private GameObject iconPrefab;

	// Token: 0x040081D0 RID: 33232
	[SerializeField]
	private GameObject linePrefab;

	// Token: 0x040081D1 RID: 33233
	[SerializeField]
	private GameObject progressBarPrefab;

	// Token: 0x040081D2 RID: 33234
	[Header("Graphics")]
	[SerializeField]
	private Image BG;

	// Token: 0x040081D3 RID: 33235
	[SerializeField]
	private Image titleBG;

	// Token: 0x040081D4 RID: 33236
	[SerializeField]
	private Image borderHighlight;

	// Token: 0x040081D5 RID: 33237
	[SerializeField]
	private Image filterHighlight;

	// Token: 0x040081D6 RID: 33238
	[SerializeField]
	private Image filterLowlight;

	// Token: 0x040081D7 RID: 33239
	[SerializeField]
	private Sprite hoverBG;

	// Token: 0x040081D8 RID: 33240
	[SerializeField]
	private Sprite completedBG;

	// Token: 0x040081D9 RID: 33241
	[Header("Colors")]
	[SerializeField]
	private Color defaultColor = Color.blue;

	// Token: 0x040081DA RID: 33242
	[SerializeField]
	private Color completedColor = Color.yellow;

	// Token: 0x040081DB RID: 33243
	[SerializeField]
	private Color pendingColor = Color.magenta;

	// Token: 0x040081DC RID: 33244
	[SerializeField]
	private Color completedHeaderColor = Color.grey;

	// Token: 0x040081DD RID: 33245
	[SerializeField]
	private Color incompleteHeaderColor = Color.grey;

	// Token: 0x040081DE RID: 33246
	[SerializeField]
	private Color pendingHeaderColor = Color.grey;

	// Token: 0x040081DF RID: 33247
	private Sprite defaultBG;

	// Token: 0x040081E0 RID: 33248
	[MyCmpGet]
	private KToggle toggle;

	// Token: 0x040081E1 RID: 33249
	private ResearchScreen researchScreen;

	// Token: 0x040081E2 RID: 33250
	private Dictionary<Tech, UILineRenderer> techLineMap;

	// Token: 0x040081E3 RID: 33251
	private Tech targetTech;

	// Token: 0x040081E4 RID: 33252
	private bool isOn = true;

	// Token: 0x040081E5 RID: 33253
	private Coroutine fadeRoutine;

	// Token: 0x040081E6 RID: 33254
	public Color activeLineColor;

	// Token: 0x040081E7 RID: 33255
	public Color inactiveLineColor;

	// Token: 0x040081E8 RID: 33256
	public int lineThickness_active = 6;

	// Token: 0x040081E9 RID: 33257
	public int lineThickness_inactive = 2;

	// Token: 0x040081EA RID: 33258
	public Material StandardUIMaterial;

	// Token: 0x040081EB RID: 33259
	private Dictionary<string, GameObject> progressBarsByResearchTypeID = new Dictionary<string, GameObject>();

	// Token: 0x040081EC RID: 33260
	public static readonly string UnlockedTechKey = "UnlockedTech";

	// Token: 0x040081ED RID: 33261
	private Dictionary<string, object> unlockedTechMetric = new Dictionary<string, object>
	{
		{
			ResearchEntry.UnlockedTechKey,
			null
		}
	};
}
