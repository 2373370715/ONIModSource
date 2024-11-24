using System;
using System.Collections;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02001ECB RID: 7883
public class ResearchScreen : KModalScreen
{
	// Token: 0x0600A5B1 RID: 42417 RVA: 0x0010B8A2 File Offset: 0x00109AA2
	public bool IsBeingResearched(Tech tech)
	{
		return Research.Instance.IsBeingResearched(tech);
	}

	// Token: 0x0600A5B2 RID: 42418 RVA: 0x0010175D File Offset: 0x000FF95D
	public override float GetSortKey()
	{
		if (base.isEditing)
		{
			return 50f;
		}
		return 20f;
	}

	// Token: 0x0600A5B3 RID: 42419 RVA: 0x003EE2DC File Offset: 0x003EC4DC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.ConsumeMouseScroll = true;
		Transform transform = base.transform;
		while (this.m_Raycaster == null)
		{
			this.m_Raycaster = transform.GetComponent<GraphicRaycaster>();
			if (this.m_Raycaster == null)
			{
				transform = transform.parent;
			}
		}
	}

	// Token: 0x0600A5B4 RID: 42420 RVA: 0x0010B8AF File Offset: 0x00109AAF
	private void ZoomOut()
	{
		this.targetZoom = Mathf.Clamp(this.targetZoom - this.zoomAmountPerButton, this.minZoom, this.maxZoom);
		this.zoomCenterLock = true;
	}

	// Token: 0x0600A5B5 RID: 42421 RVA: 0x0010B8DC File Offset: 0x00109ADC
	private void ZoomIn()
	{
		this.targetZoom = Mathf.Clamp(this.targetZoom + this.zoomAmountPerButton, this.minZoom, this.maxZoom);
		this.zoomCenterLock = true;
	}

	// Token: 0x0600A5B6 RID: 42422 RVA: 0x003EE330 File Offset: 0x003EC530
	public void ZoomToTech(string techID)
	{
		Vector2 a = this.entryMap[Db.Get().Techs.Get(techID)].rectTransform().GetLocalPosition() + new Vector2(-this.foreground.rectTransform().rect.size.x / 2f, this.foreground.rectTransform().rect.size.y / 2f);
		this.forceTargetPosition = -a;
		this.zoomingToTarget = true;
		this.targetZoom = this.maxZoom;
	}

	// Token: 0x0600A5B7 RID: 42423 RVA: 0x003EE3D8 File Offset: 0x003EC5D8
	private void Update()
	{
		if (!base.canvas.enabled)
		{
			return;
		}
		RectTransform component = this.scrollContent.GetComponent<RectTransform>();
		if (this.isDragging && !KInputManager.isFocused)
		{
			this.AbortDragging();
		}
		Vector2 anchoredPosition = component.anchoredPosition;
		float t = Mathf.Min(this.effectiveZoomSpeed * Time.unscaledDeltaTime, 0.9f);
		this.currentZoom = Mathf.Lerp(this.currentZoom, this.targetZoom, t);
		Vector2 b = Vector2.zero;
		Vector2 v = KInputManager.GetMousePos();
		Vector2 b2 = this.zoomCenterLock ? (component.InverseTransformPoint(new Vector2((float)(Screen.width / 2), (float)(Screen.height / 2))) * this.currentZoom) : (component.InverseTransformPoint(v) * this.currentZoom);
		component.localScale = new Vector3(this.currentZoom, this.currentZoom, 1f);
		b = (this.zoomCenterLock ? (component.InverseTransformPoint(new Vector2((float)(Screen.width / 2), (float)(Screen.height / 2))) * this.currentZoom) : (component.InverseTransformPoint(v) * this.currentZoom)) - b2;
		float d = this.keyboardScrollSpeed;
		if (this.panUp)
		{
			this.keyPanDelta -= Vector2.up * Time.unscaledDeltaTime * d;
		}
		else if (this.panDown)
		{
			this.keyPanDelta += Vector2.up * Time.unscaledDeltaTime * d;
		}
		if (this.panLeft)
		{
			this.keyPanDelta += Vector2.right * Time.unscaledDeltaTime * d;
		}
		else if (this.panRight)
		{
			this.keyPanDelta -= Vector2.right * Time.unscaledDeltaTime * d;
		}
		if (KInputManager.currentControllerIsGamepad)
		{
			Vector2 a = KInputManager.steamInputInterpreter.GetSteamCameraMovement();
			a *= -1f;
			this.keyPanDelta = a * Time.unscaledDeltaTime * d * 2f;
		}
		Vector2 b3 = new Vector2(Mathf.Lerp(0f, this.keyPanDelta.x, Time.unscaledDeltaTime * this.keyPanEasing), Mathf.Lerp(0f, this.keyPanDelta.y, Time.unscaledDeltaTime * this.keyPanEasing));
		this.keyPanDelta -= b3;
		Vector2 vector = Vector2.zero;
		if (this.isDragging)
		{
			Vector2 b4 = KInputManager.GetMousePos() - this.dragLastPosition;
			vector += b4;
			this.dragLastPosition = KInputManager.GetMousePos();
			this.dragInteria = Vector2.ClampMagnitude(this.dragInteria + b4, 400f);
		}
		this.dragInteria *= Mathf.Max(0f, 1f - Time.unscaledDeltaTime * 4f);
		Vector2 vector2 = anchoredPosition + b + this.keyPanDelta + vector;
		if (!this.isDragging)
		{
			Vector2 size = base.GetComponent<RectTransform>().rect.size;
			Vector2 vector3 = new Vector2((-component.rect.size.x / 2f - 250f) * this.currentZoom, -250f * this.currentZoom);
			Vector2 vector4 = new Vector2(250f * this.currentZoom, (component.rect.size.y + 250f) * this.currentZoom - size.y);
			Vector2 a2 = new Vector2(Mathf.Clamp(vector2.x, vector3.x, vector4.x), Mathf.Clamp(vector2.y, vector3.y, vector4.y));
			this.forceTargetPosition = new Vector2(Mathf.Clamp(this.forceTargetPosition.x, vector3.x, vector4.x), Mathf.Clamp(this.forceTargetPosition.y, vector3.y, vector4.y));
			Vector2 vector5 = a2 + this.dragInteria - vector2;
			if (!this.panLeft && !this.panRight && !this.panUp && !this.panDown)
			{
				vector2 += vector5 * this.edgeClampFactor * Time.unscaledDeltaTime;
			}
			else
			{
				vector2 += vector5;
				if (vector5.x < 0f)
				{
					this.keyPanDelta.x = Mathf.Min(0f, this.keyPanDelta.x);
				}
				if (vector5.x > 0f)
				{
					this.keyPanDelta.x = Mathf.Max(0f, this.keyPanDelta.x);
				}
				if (vector5.y < 0f)
				{
					this.keyPanDelta.y = Mathf.Min(0f, this.keyPanDelta.y);
				}
				if (vector5.y > 0f)
				{
					this.keyPanDelta.y = Mathf.Max(0f, this.keyPanDelta.y);
				}
			}
		}
		if (this.zoomingToTarget)
		{
			vector2 = Vector2.Lerp(vector2, this.forceTargetPosition, Time.unscaledDeltaTime * 4f);
			if (Vector3.Distance(vector2, this.forceTargetPosition) < 1f || this.isDragging || this.panLeft || this.panRight || this.panUp || this.panDown)
			{
				this.zoomingToTarget = false;
			}
		}
		component.anchoredPosition = vector2;
	}

	// Token: 0x0600A5B8 RID: 42424 RVA: 0x003EE9D4 File Offset: 0x003ECBD4
	protected override void OnSpawn()
	{
		base.Subscribe(Research.Instance.gameObject, -1914338957, new Action<object>(this.OnActiveResearchChanged));
		base.Subscribe(Game.Instance.gameObject, -107300940, new Action<object>(this.OnResearchComplete));
		base.Subscribe(Game.Instance.gameObject, -1974454597, delegate(object o)
		{
			this.Show(false);
		});
		this.pointDisplayMap = new Dictionary<string, LocText>();
		foreach (ResearchType researchType in Research.Instance.researchTypes.Types)
		{
			this.pointDisplayMap[researchType.id] = Util.KInstantiateUI(this.pointDisplayCountPrefab, this.pointDisplayContainer, true).GetComponentInChildren<LocText>();
			this.pointDisplayMap[researchType.id].text = Research.Instance.globalPointInventory.PointsByTypeID[researchType.id].ToString();
			this.pointDisplayMap[researchType.id].transform.parent.GetComponent<ToolTip>().SetSimpleTooltip(researchType.description);
			this.pointDisplayMap[researchType.id].transform.parent.GetComponentInChildren<Image>().sprite = researchType.sprite;
		}
		this.pointDisplayContainer.transform.parent.gameObject.SetActive(Research.Instance.UseGlobalPointInventory);
		this.entryMap = new Dictionary<Tech, ResearchEntry>();
		List<Tech> resources = Db.Get().Techs.resources;
		resources.Sort((Tech x, Tech y) => y.center.y.CompareTo(x.center.y));
		List<TechTreeTitle> resources2 = Db.Get().TechTreeTitles.resources;
		resources2.Sort((TechTreeTitle x, TechTreeTitle y) => y.center.y.CompareTo(x.center.y));
		float x3 = 0f;
		float y3 = 125f;
		Vector2 b = new Vector2(x3, y3);
		for (int i = 0; i < resources2.Count; i++)
		{
			ResearchTreeTitle researchTreeTitle = Util.KInstantiateUI<ResearchTreeTitle>(this.researchTreeTitlePrefab.gameObject, this.treeTitles, false);
			TechTreeTitle techTreeTitle = resources2[i];
			researchTreeTitle.name = techTreeTitle.Name + " Title";
			Vector3 vector = techTreeTitle.center + b;
			researchTreeTitle.transform.rectTransform().anchoredPosition = vector;
			float num = techTreeTitle.height;
			if (i + 1 < resources2.Count)
			{
				TechTreeTitle techTreeTitle2 = resources2[i + 1];
				Vector3 vector2 = techTreeTitle2.center + b;
				num += vector.y - (vector2.y + techTreeTitle2.height);
			}
			else
			{
				num += 600f;
			}
			researchTreeTitle.transform.rectTransform().sizeDelta = new Vector2(techTreeTitle.width, num);
			researchTreeTitle.SetLabel(techTreeTitle.Name);
			researchTreeTitle.SetColor(i);
		}
		List<Vector2> list = new List<Vector2>();
		float x2 = 0f;
		float y2 = 0f;
		Vector2 b2 = new Vector2(x2, y2);
		for (int j = 0; j < resources.Count; j++)
		{
			ResearchEntry researchEntry = Util.KInstantiateUI<ResearchEntry>(this.entryPrefab.gameObject, this.scrollContent, false);
			Tech tech = resources[j];
			researchEntry.name = tech.Name + " Panel";
			Vector3 v = tech.center + b2;
			researchEntry.transform.rectTransform().anchoredPosition = v;
			researchEntry.transform.rectTransform().sizeDelta = new Vector2(tech.width, tech.height);
			this.entryMap.Add(tech, researchEntry);
			if (tech.edges.Count > 0)
			{
				for (int k = 0; k < tech.edges.Count; k++)
				{
					ResourceTreeNode.Edge edge = tech.edges[k];
					if (edge.path == null)
					{
						list.AddRange(edge.SrcTarget);
					}
					else
					{
						ResourceTreeNode.Edge.EdgeType edgeType = edge.edgeType;
						if (edgeType <= ResourceTreeNode.Edge.EdgeType.QuadCurveEdge || edgeType - ResourceTreeNode.Edge.EdgeType.BezierEdge <= 1)
						{
							list.Add(edge.SrcTarget[0]);
							list.Add(edge.path[0]);
							for (int l = 1; l < edge.path.Count; l++)
							{
								list.Add(edge.path[l - 1]);
								list.Add(edge.path[l]);
							}
							list.Add(edge.path[edge.path.Count - 1]);
							list.Add(edge.SrcTarget[1]);
						}
						else
						{
							list.AddRange(edge.path);
						}
					}
				}
			}
		}
		for (int m = 0; m < list.Count; m++)
		{
			list[m] = new Vector2(list[m].x, list[m].y + this.foreground.transform.rectTransform().rect.height);
		}
		foreach (KeyValuePair<Tech, ResearchEntry> keyValuePair in this.entryMap)
		{
			keyValuePair.Value.SetTech(keyValuePair.Key);
		}
		this.CloseButton.soundPlayer.Enabled = false;
		this.CloseButton.onClick += delegate()
		{
			ManagementMenu.Instance.CloseAll();
		};
		base.StartCoroutine(this.WaitAndSetActiveResearch());
		base.OnSpawn();
		this.scrollContent.GetComponent<RectTransform>().anchoredPosition = new Vector2(250f, -250f);
		this.zoomOutButton.onClick += delegate()
		{
			this.ZoomOut();
		};
		this.zoomInButton.onClick += delegate()
		{
			this.ZoomIn();
		};
		base.gameObject.SetActive(true);
		this.Show(false);
	}

	// Token: 0x0600A5B9 RID: 42425 RVA: 0x0010B909 File Offset: 0x00109B09
	public override void OnBeginDrag(PointerEventData eventData)
	{
		base.OnBeginDrag(eventData);
		this.isDragging = true;
	}

	// Token: 0x0600A5BA RID: 42426 RVA: 0x0010B919 File Offset: 0x00109B19
	public override void OnEndDrag(PointerEventData eventData)
	{
		base.OnEndDrag(eventData);
		this.AbortDragging();
	}

	// Token: 0x0600A5BB RID: 42427 RVA: 0x0010B928 File Offset: 0x00109B28
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		base.Unsubscribe(Game.Instance.gameObject, -1974454597, delegate(object o)
		{
			this.Deactivate();
		});
	}

	// Token: 0x0600A5BC RID: 42428 RVA: 0x0010B951 File Offset: 0x00109B51
	private IEnumerator WaitAndSetActiveResearch()
	{
		yield return SequenceUtil.WaitForEndOfFrame;
		TechInstance targetResearch = Research.Instance.GetTargetResearch();
		if (targetResearch != null)
		{
			this.SetActiveResearch(targetResearch.tech);
		}
		yield break;
	}

	// Token: 0x0600A5BD RID: 42429 RVA: 0x0010B960 File Offset: 0x00109B60
	public Vector3 GetEntryPosition(Tech tech)
	{
		if (!this.entryMap.ContainsKey(tech))
		{
			global::Debug.LogError("The Tech provided was not present in the dictionary");
			return Vector3.zero;
		}
		return this.entryMap[tech].transform.GetPosition();
	}

	// Token: 0x0600A5BE RID: 42430 RVA: 0x0010B996 File Offset: 0x00109B96
	public ResearchEntry GetEntry(Tech tech)
	{
		if (this.entryMap == null)
		{
			return null;
		}
		if (!this.entryMap.ContainsKey(tech))
		{
			global::Debug.LogError("The Tech provided was not present in the dictionary");
			return null;
		}
		return this.entryMap[tech];
	}

	// Token: 0x0600A5BF RID: 42431 RVA: 0x003EF07C File Offset: 0x003ED27C
	public void SetEntryPercentage(Tech tech, float percent)
	{
		ResearchEntry entry = this.GetEntry(tech);
		if (entry != null)
		{
			entry.SetPercentage(percent);
		}
	}

	// Token: 0x0600A5C0 RID: 42432 RVA: 0x003EF0A4 File Offset: 0x003ED2A4
	public void TurnEverythingOff()
	{
		foreach (KeyValuePair<Tech, ResearchEntry> keyValuePair in this.entryMap)
		{
			keyValuePair.Value.SetEverythingOff();
		}
	}

	// Token: 0x0600A5C1 RID: 42433 RVA: 0x003EF0FC File Offset: 0x003ED2FC
	public void TurnEverythingOn()
	{
		foreach (KeyValuePair<Tech, ResearchEntry> keyValuePair in this.entryMap)
		{
			keyValuePair.Value.SetEverythingOn();
		}
	}

	// Token: 0x0600A5C2 RID: 42434 RVA: 0x003EF154 File Offset: 0x003ED354
	private void SelectAllEntries(Tech tech, bool isSelected)
	{
		ResearchEntry entry = this.GetEntry(tech);
		if (entry != null)
		{
			entry.QueueStateChanged(isSelected);
		}
		foreach (Tech tech2 in tech.requiredTech)
		{
			this.SelectAllEntries(tech2, isSelected);
		}
	}

	// Token: 0x0600A5C3 RID: 42435 RVA: 0x003EF1C0 File Offset: 0x003ED3C0
	private void OnResearchComplete(object data)
	{
		if (data is Tech)
		{
			Tech tech = (Tech)data;
			ResearchEntry entry = this.GetEntry(tech);
			if (entry != null)
			{
				entry.ResearchCompleted(true);
			}
			this.UpdateProgressBars();
			this.UpdatePointDisplay();
		}
	}

	// Token: 0x0600A5C4 RID: 42436 RVA: 0x003EF200 File Offset: 0x003ED400
	private void UpdatePointDisplay()
	{
		foreach (ResearchType researchType in Research.Instance.researchTypes.Types)
		{
			this.pointDisplayMap[researchType.id].text = string.Format("{0}: {1}", Research.Instance.researchTypes.GetResearchType(researchType.id).name, Research.Instance.globalPointInventory.PointsByTypeID[researchType.id].ToString());
		}
	}

	// Token: 0x0600A5C5 RID: 42437 RVA: 0x003EF2B4 File Offset: 0x003ED4B4
	private void OnActiveResearchChanged(object data)
	{
		List<TechInstance> list = (List<TechInstance>)data;
		foreach (TechInstance techInstance in list)
		{
			ResearchEntry entry = this.GetEntry(techInstance.tech);
			if (entry != null)
			{
				entry.QueueStateChanged(true);
			}
		}
		this.UpdateProgressBars();
		this.UpdatePointDisplay();
		if (list.Count > 0)
		{
			this.currentResearch = list[list.Count - 1].tech;
		}
	}

	// Token: 0x0600A5C6 RID: 42438 RVA: 0x003EF350 File Offset: 0x003ED550
	private void UpdateProgressBars()
	{
		foreach (KeyValuePair<Tech, ResearchEntry> keyValuePair in this.entryMap)
		{
			keyValuePair.Value.UpdateProgressBars();
		}
	}

	// Token: 0x0600A5C7 RID: 42439 RVA: 0x003EF3A8 File Offset: 0x003ED5A8
	public void CancelResearch()
	{
		List<TechInstance> researchQueue = Research.Instance.GetResearchQueue();
		foreach (TechInstance techInstance in researchQueue)
		{
			ResearchEntry entry = this.GetEntry(techInstance.tech);
			if (entry != null)
			{
				entry.QueueStateChanged(false);
			}
		}
		researchQueue.Clear();
	}

	// Token: 0x0600A5C8 RID: 42440 RVA: 0x0010B9C8 File Offset: 0x00109BC8
	private void SetActiveResearch(Tech newResearch)
	{
		if (newResearch != this.currentResearch && this.currentResearch != null)
		{
			this.SelectAllEntries(this.currentResearch, false);
		}
		this.currentResearch = newResearch;
		if (this.currentResearch != null)
		{
			this.SelectAllEntries(this.currentResearch, true);
		}
	}

	// Token: 0x0600A5C9 RID: 42441 RVA: 0x003EF420 File Offset: 0x003ED620
	public override void Show(bool show = true)
	{
		this.mouseOver = false;
		this.scrollContentChildFitter.enabled = show;
		foreach (Canvas canvas in base.GetComponentsInChildren<Canvas>(true))
		{
			if (canvas.enabled != show)
			{
				canvas.enabled = show;
			}
		}
		CanvasGroup component = base.GetComponent<CanvasGroup>();
		if (component != null)
		{
			component.interactable = show;
			component.blocksRaycasts = show;
			component.ignoreParentGroups = true;
		}
		this.OnShow(show);
	}

	// Token: 0x0600A5CA RID: 42442 RVA: 0x003EF498 File Offset: 0x003ED698
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (show)
		{
			this.sideBar.ResetFilter();
		}
		if (show)
		{
			CameraController.Instance.DisableUserCameraControl = true;
			if (DetailsScreen.Instance != null)
			{
				DetailsScreen.Instance.gameObject.SetActive(false);
			}
		}
		else
		{
			CameraController.Instance.DisableUserCameraControl = false;
			if (SelectTool.Instance.selected != null && !DetailsScreen.Instance.gameObject.activeSelf)
			{
				DetailsScreen.Instance.gameObject.SetActive(true);
				DetailsScreen.Instance.Refresh(SelectTool.Instance.selected.gameObject);
			}
		}
		this.UpdateProgressBars();
		this.UpdatePointDisplay();
	}

	// Token: 0x0600A5CB RID: 42443 RVA: 0x0010BA04 File Offset: 0x00109C04
	private void AbortDragging()
	{
		this.isDragging = false;
		this.draggingJustEnded = true;
	}

	// Token: 0x0600A5CC RID: 42444 RVA: 0x0010BA14 File Offset: 0x00109C14
	private void LateUpdate()
	{
		this.draggingJustEnded = false;
	}

	// Token: 0x0600A5CD RID: 42445 RVA: 0x003EF54C File Offset: 0x003ED74C
	public override void OnKeyUp(KButtonEvent e)
	{
		if (!base.canvas.enabled)
		{
			return;
		}
		if (!e.Consumed)
		{
			if (e.IsAction(global::Action.MouseRight) && !this.isDragging && !this.draggingJustEnded)
			{
				ManagementMenu.Instance.CloseAll();
			}
			if (e.IsAction(global::Action.MouseRight) || e.IsAction(global::Action.MouseLeft) || e.IsAction(global::Action.MouseMiddle))
			{
				this.AbortDragging();
			}
			if (this.panUp && e.TryConsume(global::Action.PanUp))
			{
				this.panUp = false;
				return;
			}
			if (this.panDown && e.TryConsume(global::Action.PanDown))
			{
				this.panDown = false;
				return;
			}
			if (this.panRight && e.TryConsume(global::Action.PanRight))
			{
				this.panRight = false;
				return;
			}
			if (this.panLeft && e.TryConsume(global::Action.PanLeft))
			{
				this.panLeft = false;
				return;
			}
		}
		base.OnKeyUp(e);
	}

	// Token: 0x0600A5CE RID: 42446 RVA: 0x003EF634 File Offset: 0x003ED834
	public override void OnKeyDown(KButtonEvent e)
	{
		if (!base.canvas.enabled)
		{
			return;
		}
		if (!e.Consumed)
		{
			if (e.TryConsume(global::Action.MouseRight))
			{
				this.dragStartPosition = KInputManager.GetMousePos();
				this.dragLastPosition = KInputManager.GetMousePos();
				return;
			}
			if (e.TryConsume(global::Action.MouseLeft))
			{
				this.dragStartPosition = KInputManager.GetMousePos();
				this.dragLastPosition = KInputManager.GetMousePos();
				return;
			}
			if (KInputManager.GetMousePos().x > this.sideBar.rectTransform().sizeDelta.x && CameraController.IsMouseOverGameWindow)
			{
				if (e.TryConsume(global::Action.ZoomIn))
				{
					this.targetZoom = Mathf.Clamp(this.targetZoom + this.zoomAmountPerScroll, this.minZoom, this.maxZoom);
					this.zoomCenterLock = false;
					return;
				}
				if (e.TryConsume(global::Action.ZoomOut))
				{
					this.targetZoom = Mathf.Clamp(this.targetZoom - this.zoomAmountPerScroll, this.minZoom, this.maxZoom);
					this.zoomCenterLock = false;
					return;
				}
			}
			if (e.TryConsume(global::Action.Escape))
			{
				ManagementMenu.Instance.CloseAll();
				return;
			}
			if (e.TryConsume(global::Action.PanLeft))
			{
				this.panLeft = true;
				return;
			}
			if (e.TryConsume(global::Action.PanRight))
			{
				this.panRight = true;
				return;
			}
			if (e.TryConsume(global::Action.PanUp))
			{
				this.panUp = true;
				return;
			}
			if (e.TryConsume(global::Action.PanDown))
			{
				this.panDown = true;
				return;
			}
		}
		base.OnKeyDown(e);
	}

	// Token: 0x0600A5CF RID: 42447 RVA: 0x003EF79C File Offset: 0x003ED99C
	public static bool TechPassesSearchFilter(string techID, string filterString)
	{
		if (!string.IsNullOrEmpty(filterString))
		{
			filterString = filterString.ToUpper();
			bool flag = false;
			Tech tech = Db.Get().Techs.Get(techID);
			flag = UI.StripLinkFormatting(tech.Name).ToLower().ToUpper().Contains(filterString);
			if (!flag)
			{
				flag = tech.category.ToUpper().Contains(filterString);
				foreach (TechItem techItem in tech.unlockedItems)
				{
					if (SaveLoader.Instance.IsCorrectDlcActiveForCurrentSave(techItem.requiredDlcIds, techItem.forbiddenDlcIds))
					{
						if (UI.StripLinkFormatting(techItem.Name).ToLower().ToUpper().Contains(filterString))
						{
							flag = true;
							break;
						}
						if (UI.StripLinkFormatting(techItem.description).ToLower().ToUpper().Contains(filterString))
						{
							flag = true;
							break;
						}
					}
				}
			}
			return flag;
		}
		return true;
	}

	// Token: 0x0600A5D0 RID: 42448 RVA: 0x003EF8A0 File Offset: 0x003EDAA0
	public static bool TechItemPassesSearchFilter(string techItemID, string filterString)
	{
		if (!string.IsNullOrEmpty(filterString))
		{
			filterString = filterString.ToUpper();
			TechItem techItem = Db.Get().TechItems.Get(techItemID);
			bool flag = UI.StripLinkFormatting(techItem.Name).ToLower().ToUpper().Contains(filterString);
			if (!flag)
			{
				flag = techItem.Name.ToUpper().Contains(filterString);
				flag = (flag && techItem.description.ToUpper().Contains(filterString));
			}
			return flag;
		}
		return true;
	}

	// Token: 0x040081EE RID: 33262
	private const float SCROLL_BUFFER = 250f;

	// Token: 0x040081EF RID: 33263
	[SerializeField]
	private Image BG;

	// Token: 0x040081F0 RID: 33264
	public ResearchEntry entryPrefab;

	// Token: 0x040081F1 RID: 33265
	public ResearchTreeTitle researchTreeTitlePrefab;

	// Token: 0x040081F2 RID: 33266
	public GameObject foreground;

	// Token: 0x040081F3 RID: 33267
	public GameObject scrollContent;

	// Token: 0x040081F4 RID: 33268
	public GameObject treeTitles;

	// Token: 0x040081F5 RID: 33269
	public GameObject pointDisplayCountPrefab;

	// Token: 0x040081F6 RID: 33270
	public GameObject pointDisplayContainer;

	// Token: 0x040081F7 RID: 33271
	private Dictionary<string, LocText> pointDisplayMap;

	// Token: 0x040081F8 RID: 33272
	private Dictionary<Tech, ResearchEntry> entryMap;

	// Token: 0x040081F9 RID: 33273
	[SerializeField]
	private KButton zoomOutButton;

	// Token: 0x040081FA RID: 33274
	[SerializeField]
	private KButton zoomInButton;

	// Token: 0x040081FB RID: 33275
	[SerializeField]
	private ResearchScreenSideBar sideBar;

	// Token: 0x040081FC RID: 33276
	private Tech currentResearch;

	// Token: 0x040081FD RID: 33277
	public KButton CloseButton;

	// Token: 0x040081FE RID: 33278
	private GraphicRaycaster m_Raycaster;

	// Token: 0x040081FF RID: 33279
	private PointerEventData m_PointerEventData;

	// Token: 0x04008200 RID: 33280
	private Vector3 currentScrollPosition;

	// Token: 0x04008201 RID: 33281
	private bool panUp;

	// Token: 0x04008202 RID: 33282
	private bool panDown;

	// Token: 0x04008203 RID: 33283
	private bool panLeft;

	// Token: 0x04008204 RID: 33284
	private bool panRight;

	// Token: 0x04008205 RID: 33285
	[SerializeField]
	private KChildFitter scrollContentChildFitter;

	// Token: 0x04008206 RID: 33286
	private bool isDragging;

	// Token: 0x04008207 RID: 33287
	private Vector3 dragStartPosition;

	// Token: 0x04008208 RID: 33288
	private Vector3 dragLastPosition;

	// Token: 0x04008209 RID: 33289
	private Vector2 dragInteria;

	// Token: 0x0400820A RID: 33290
	private Vector2 forceTargetPosition;

	// Token: 0x0400820B RID: 33291
	private bool zoomingToTarget;

	// Token: 0x0400820C RID: 33292
	private bool draggingJustEnded;

	// Token: 0x0400820D RID: 33293
	private float targetZoom = 1f;

	// Token: 0x0400820E RID: 33294
	private float currentZoom = 1f;

	// Token: 0x0400820F RID: 33295
	private bool zoomCenterLock;

	// Token: 0x04008210 RID: 33296
	private Vector2 keyPanDelta = Vector3.zero;

	// Token: 0x04008211 RID: 33297
	[SerializeField]
	private float effectiveZoomSpeed = 5f;

	// Token: 0x04008212 RID: 33298
	[SerializeField]
	private float zoomAmountPerScroll = 0.05f;

	// Token: 0x04008213 RID: 33299
	[SerializeField]
	private float zoomAmountPerButton = 0.5f;

	// Token: 0x04008214 RID: 33300
	[SerializeField]
	private float minZoom = 0.15f;

	// Token: 0x04008215 RID: 33301
	[SerializeField]
	private float maxZoom = 1f;

	// Token: 0x04008216 RID: 33302
	[SerializeField]
	private float keyboardScrollSpeed = 200f;

	// Token: 0x04008217 RID: 33303
	[SerializeField]
	private float keyPanEasing = 1f;

	// Token: 0x04008218 RID: 33304
	[SerializeField]
	private float edgeClampFactor = 0.5f;

	// Token: 0x02001ECC RID: 7884
	public enum ResearchState
	{
		// Token: 0x0400821A RID: 33306
		Available,
		// Token: 0x0400821B RID: 33307
		ActiveResearch,
		// Token: 0x0400821C RID: 33308
		ResearchComplete,
		// Token: 0x0400821D RID: 33309
		MissingPrerequisites,
		// Token: 0x0400821E RID: 33310
		StateCount
	}
}
