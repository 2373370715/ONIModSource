using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FMOD.Studio;
using STRINGS;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02001C1E RID: 7198
public class ClusterMapScreen : KScreen
{
	// Token: 0x0600959C RID: 38300 RVA: 0x00101737 File Offset: 0x000FF937
	public static void DestroyInstance()
	{
		ClusterMapScreen.Instance = null;
	}

	// Token: 0x0600959D RID: 38301 RVA: 0x0010173F File Offset: 0x000FF93F
	public ClusterMapVisualizer GetEntityVisAnim(ClusterGridEntity entity)
	{
		if (this.m_gridEntityAnims.ContainsKey(entity))
		{
			return this.m_gridEntityAnims[entity];
		}
		return null;
	}

	// Token: 0x0600959E RID: 38302 RVA: 0x0010175D File Offset: 0x000FF95D
	public override float GetSortKey()
	{
		if (base.isEditing)
		{
			return 50f;
		}
		return 20f;
	}

	// Token: 0x0600959F RID: 38303 RVA: 0x00101772 File Offset: 0x000FF972
	public float CurrentZoomPercentage()
	{
		return (this.m_currentZoomScale - 50f) / 100f;
	}

	// Token: 0x060095A0 RID: 38304 RVA: 0x00101786 File Offset: 0x000FF986
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.m_selectMarker = global::Util.KInstantiateUI<SelectMarker>(this.selectMarkerPrefab, base.gameObject, false);
		this.m_selectMarker.gameObject.SetActive(false);
		ClusterMapScreen.Instance = this;
	}

	// Token: 0x060095A1 RID: 38305 RVA: 0x0039CFE0 File Offset: 0x0039B1E0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		global::Debug.Assert(this.cellVisPrefab.rectTransform().sizeDelta == new Vector2(2f, 2f), "The radius of the cellVisPrefab hex must be 1");
		global::Debug.Assert(this.terrainVisPrefab.rectTransform().sizeDelta == new Vector2(2f, 2f), "The radius of the terrainVisPrefab hex must be 1");
		global::Debug.Assert(this.mobileVisPrefab.rectTransform().sizeDelta == new Vector2(2f, 2f), "The radius of the mobileVisPrefab hex must be 1");
		global::Debug.Assert(this.staticVisPrefab.rectTransform().sizeDelta == new Vector2(2f, 2f), "The radius of the staticVisPrefab hex must be 1");
		int num;
		int num2;
		int num3;
		int num4;
		this.GenerateGridVis(out num, out num2, out num3, out num4);
		this.Show(false);
		this.mapScrollRect.content.sizeDelta = new Vector2((float)(num2 * 4), (float)(num4 * 4));
		this.mapScrollRect.content.localScale = new Vector3(this.m_currentZoomScale, this.m_currentZoomScale, 1f);
		this.m_onDestinationChangedDelegate = new Action<object>(this.OnDestinationChanged);
		this.m_onSelectObjectDelegate = new Action<object>(this.OnSelectObject);
		base.Subscribe(1980521255, new Action<object>(this.UpdateVis));
	}

	// Token: 0x060095A2 RID: 38306 RVA: 0x0039D140 File Offset: 0x0039B340
	protected void MoveToNISPosition()
	{
		if (!this.movingToTargetNISPosition)
		{
			return;
		}
		Vector3 b = new Vector3(-this.targetNISPosition.x * this.mapScrollRect.content.localScale.x, -this.targetNISPosition.y * this.mapScrollRect.content.localScale.y, this.targetNISPosition.z);
		this.m_targetZoomScale = Mathf.Lerp(this.m_targetZoomScale, this.targetNISZoom, Time.unscaledDeltaTime * 2f);
		this.mapScrollRect.content.SetLocalPosition(Vector3.Lerp(this.mapScrollRect.content.GetLocalPosition(), b, Time.unscaledDeltaTime * 2.5f));
		float num = Vector3.Distance(this.mapScrollRect.content.GetLocalPosition(), b);
		if (num < 100f)
		{
			ClusterMapHex component = this.m_cellVisByLocation[this.selectOnMoveNISComplete].GetComponent<ClusterMapHex>();
			if (this.m_selectedHex != component)
			{
				this.SelectHex(component);
			}
			if (num < 10f)
			{
				this.movingToTargetNISPosition = false;
			}
		}
	}

	// Token: 0x060095A3 RID: 38307 RVA: 0x001017BD File Offset: 0x000FF9BD
	public void SetTargetFocusPosition(AxialI targetPosition, float delayBeforeMove = 0.5f)
	{
		if (this.activeMoveToTargetRoutine != null)
		{
			base.StopCoroutine(this.activeMoveToTargetRoutine);
		}
		this.activeMoveToTargetRoutine = base.StartCoroutine(this.MoveToTargetRoutine(targetPosition, delayBeforeMove));
	}

	// Token: 0x060095A4 RID: 38308 RVA: 0x001017E7 File Offset: 0x000FF9E7
	private IEnumerator MoveToTargetRoutine(AxialI targetPosition, float delayBeforeMove)
	{
		delayBeforeMove = Mathf.Max(delayBeforeMove, 0f);
		yield return SequenceUtil.WaitForSecondsRealtime(delayBeforeMove);
		this.targetNISPosition = AxialUtil.AxialToWorld((float)targetPosition.r, (float)targetPosition.q);
		this.targetNISZoom = 150f;
		this.movingToTargetNISPosition = true;
		this.selectOnMoveNISComplete = targetPosition;
		yield break;
	}

	// Token: 0x060095A5 RID: 38309 RVA: 0x0039D25C File Offset: 0x0039B45C
	public override void OnKeyDown(KButtonEvent e)
	{
		if (!e.Consumed && (e.IsAction(global::Action.ZoomIn) || e.IsAction(global::Action.ZoomOut)) && CameraController.IsMouseOverGameWindow)
		{
			List<RaycastResult> list = new List<RaycastResult>();
			PointerEventData pointerEventData = new PointerEventData(UnityEngine.EventSystems.EventSystem.current);
			pointerEventData.position = KInputManager.GetMousePos();
			UnityEngine.EventSystems.EventSystem current = UnityEngine.EventSystems.EventSystem.current;
			if (current != null)
			{
				current.RaycastAll(pointerEventData, list);
				bool flag = false;
				foreach (RaycastResult raycastResult in list)
				{
					if (!raycastResult.gameObject.transform.IsChildOf(base.transform))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					float num;
					if (KInputManager.currentControllerIsGamepad)
					{
						num = 25f;
						num *= (float)(e.IsAction(global::Action.ZoomIn) ? 1 : -1);
					}
					else
					{
						num = Input.mouseScrollDelta.y * 25f;
					}
					this.m_targetZoomScale = Mathf.Clamp(this.m_targetZoomScale + num, 50f, 150f);
					e.TryConsume(global::Action.ZoomIn);
					if (!e.Consumed)
					{
						e.TryConsume(global::Action.ZoomOut);
					}
				}
			}
		}
		CameraController.Instance.ChangeWorldInput(e);
		base.OnKeyDown(e);
	}

	// Token: 0x060095A6 RID: 38310 RVA: 0x00101804 File Offset: 0x000FFA04
	public bool TryHandleCancel()
	{
		if (this.m_mode == ClusterMapScreen.Mode.SelectDestination && !this.m_closeOnSelect)
		{
			this.SetMode(ClusterMapScreen.Mode.Default);
			return true;
		}
		return false;
	}

	// Token: 0x060095A7 RID: 38311 RVA: 0x0039D3B4 File Offset: 0x0039B5B4
	public void ShowInSelectDestinationMode(ClusterDestinationSelector destination_selector)
	{
		this.m_destinationSelector = destination_selector;
		if (!base.gameObject.activeSelf)
		{
			ManagementMenu.Instance.ToggleClusterMap();
			this.m_closeOnSelect = true;
		}
		ClusterGridEntity component = destination_selector.GetComponent<ClusterGridEntity>();
		this.SetSelectedEntity(component, false);
		if (this.m_selectedEntity != null)
		{
			this.m_selectedHex = this.m_cellVisByLocation[this.m_selectedEntity.Location].GetComponent<ClusterMapHex>();
		}
		else
		{
			AxialI myWorldLocation = destination_selector.GetMyWorldLocation();
			ClusterMapHex component2 = this.m_cellVisByLocation[myWorldLocation].GetComponent<ClusterMapHex>();
			this.m_selectedHex = component2;
		}
		this.SetMode(ClusterMapScreen.Mode.SelectDestination);
	}

	// Token: 0x060095A8 RID: 38312 RVA: 0x00101821 File Offset: 0x000FFA21
	private void SetMode(ClusterMapScreen.Mode mode)
	{
		this.m_mode = mode;
		if (this.m_mode == ClusterMapScreen.Mode.Default)
		{
			this.m_destinationSelector = null;
		}
		this.UpdateVis(null);
	}

	// Token: 0x060095A9 RID: 38313 RVA: 0x00101840 File Offset: 0x000FFA40
	public ClusterMapScreen.Mode GetMode()
	{
		return this.m_mode;
	}

	// Token: 0x060095AA RID: 38314 RVA: 0x0039D450 File Offset: 0x0039B650
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (show)
		{
			this.MoveToNISPosition();
			this.UpdateVis(null);
			if (this.m_mode == ClusterMapScreen.Mode.Default)
			{
				this.TrySelectDefault();
			}
			Game.Instance.Subscribe(-1991583975, new Action<object>(this.OnFogOfWarRevealed));
			Game.Instance.Subscribe(-1554423969, new Action<object>(this.OnNewTelescopeTarget));
			Game.Instance.Subscribe(-1298331547, new Action<object>(this.OnClusterLocationChanged));
			ClusterMapSelectTool.Instance.Activate();
			this.SetShowingNonClusterMapHud(false);
			CameraController.Instance.DisableUserCameraControl = true;
			AudioMixer.instance.Start(AudioMixerSnapshots.Get().MENUStarmapNotPausedSnapshot);
			MusicManager.instance.PlaySong("Music_Starmap", false);
			this.UpdateTearStatus();
			return;
		}
		Game.Instance.Unsubscribe(-1554423969, new Action<object>(this.OnNewTelescopeTarget));
		Game.Instance.Unsubscribe(-1991583975, new Action<object>(this.OnFogOfWarRevealed));
		Game.Instance.Unsubscribe(-1298331547, new Action<object>(this.OnClusterLocationChanged));
		this.m_mode = ClusterMapScreen.Mode.Default;
		this.m_closeOnSelect = false;
		this.m_destinationSelector = null;
		SelectTool.Instance.Activate();
		this.SetShowingNonClusterMapHud(true);
		CameraController.Instance.DisableUserCameraControl = false;
		AudioMixer.instance.Stop(AudioMixerSnapshots.Get().MENUStarmapNotPausedSnapshot, STOP_MODE.ALLOWFADEOUT);
		if (MusicManager.instance.SongIsPlaying("Music_Starmap"))
		{
			MusicManager.instance.StopSong("Music_Starmap", true, STOP_MODE.ALLOWFADEOUT);
		}
	}

	// Token: 0x060095AB RID: 38315 RVA: 0x00101848 File Offset: 0x000FFA48
	private void SetShowingNonClusterMapHud(bool show)
	{
		PlanScreen.Instance.gameObject.SetActive(show);
		ToolMenu.Instance.gameObject.SetActive(show);
		OverlayScreen.Instance.gameObject.SetActive(show);
	}

	// Token: 0x060095AC RID: 38316 RVA: 0x0039D5DC File Offset: 0x0039B7DC
	private void SetSelectedEntity(ClusterGridEntity entity, bool frameDelay = false)
	{
		if (this.m_selectedEntity != null)
		{
			this.m_selectedEntity.Unsubscribe(543433792, this.m_onDestinationChangedDelegate);
			this.m_selectedEntity.Unsubscribe(-1503271301, this.m_onSelectObjectDelegate);
		}
		this.m_selectedEntity = entity;
		if (this.m_selectedEntity != null)
		{
			this.m_selectedEntity.Subscribe(543433792, this.m_onDestinationChangedDelegate);
			this.m_selectedEntity.Subscribe(-1503271301, this.m_onSelectObjectDelegate);
		}
		KSelectable new_selected = (this.m_selectedEntity != null) ? this.m_selectedEntity.GetComponent<KSelectable>() : null;
		if (frameDelay)
		{
			ClusterMapSelectTool.Instance.SelectNextFrame(new_selected, false);
			return;
		}
		ClusterMapSelectTool.Instance.Select(new_selected, false);
	}

	// Token: 0x060095AD RID: 38317 RVA: 0x0010187A File Offset: 0x000FFA7A
	private void OnDestinationChanged(object data)
	{
		this.UpdateVis(null);
	}

	// Token: 0x060095AE RID: 38318 RVA: 0x0039D6A0 File Offset: 0x0039B8A0
	private void OnSelectObject(object data)
	{
		if (this.m_selectedEntity == null)
		{
			return;
		}
		KSelectable component = this.m_selectedEntity.GetComponent<KSelectable>();
		if (component == null || component.IsSelected)
		{
			return;
		}
		this.SetSelectedEntity(null, false);
		if (this.m_mode == ClusterMapScreen.Mode.SelectDestination)
		{
			if (this.m_closeOnSelect)
			{
				ManagementMenu.Instance.CloseAll();
			}
			else
			{
				this.SetMode(ClusterMapScreen.Mode.Default);
			}
		}
		this.UpdateVis(null);
	}

	// Token: 0x060095AF RID: 38319 RVA: 0x0010187A File Offset: 0x000FFA7A
	private void OnFogOfWarRevealed(object data = null)
	{
		this.UpdateVis(null);
	}

	// Token: 0x060095B0 RID: 38320 RVA: 0x0010187A File Offset: 0x000FFA7A
	private void OnNewTelescopeTarget(object data = null)
	{
		this.UpdateVis(null);
	}

	// Token: 0x060095B1 RID: 38321 RVA: 0x00101883 File Offset: 0x000FFA83
	private void Update()
	{
		if (KInputManager.currentControllerIsGamepad)
		{
			this.mapScrollRect.AnalogUpdate(KInputManager.steamInputInterpreter.GetSteamCameraMovement() * this.scrollSpeed);
		}
	}

	// Token: 0x060095B2 RID: 38322 RVA: 0x0039D710 File Offset: 0x0039B910
	private void TrySelectDefault()
	{
		if (this.m_selectedHex != null && this.m_selectedEntity != null)
		{
			this.UpdateVis(null);
			return;
		}
		WorldContainer activeWorld = ClusterManager.Instance.activeWorld;
		if (activeWorld == null)
		{
			return;
		}
		ClusterGridEntity component = activeWorld.GetComponent<ClusterGridEntity>();
		if (component == null)
		{
			return;
		}
		this.SelectEntity(component, false);
	}

	// Token: 0x060095B3 RID: 38323 RVA: 0x0039D770 File Offset: 0x0039B970
	private void GenerateGridVis(out int minR, out int maxR, out int minQ, out int maxQ)
	{
		minR = int.MaxValue;
		maxR = int.MinValue;
		minQ = int.MaxValue;
		maxQ = int.MinValue;
		foreach (KeyValuePair<AxialI, List<ClusterGridEntity>> keyValuePair in ClusterGrid.Instance.cellContents)
		{
			ClusterMapVisualizer clusterMapVisualizer = UnityEngine.Object.Instantiate<ClusterMapVisualizer>(this.cellVisPrefab, Vector3.zero, Quaternion.identity, this.cellVisContainer.transform);
			clusterMapVisualizer.rectTransform().SetLocalPosition(keyValuePair.Key.ToWorld());
			clusterMapVisualizer.gameObject.SetActive(true);
			ClusterMapHex component = clusterMapVisualizer.GetComponent<ClusterMapHex>();
			component.SetLocation(keyValuePair.Key);
			this.m_cellVisByLocation.Add(keyValuePair.Key, clusterMapVisualizer);
			minR = Mathf.Min(minR, component.location.R);
			maxR = Mathf.Max(maxR, component.location.R);
			minQ = Mathf.Min(minQ, component.location.Q);
			maxQ = Mathf.Max(maxQ, component.location.Q);
		}
		this.SetupVisGameObjects();
		this.UpdateVis(null);
	}

	// Token: 0x060095B4 RID: 38324 RVA: 0x0039D8C4 File Offset: 0x0039BAC4
	public Transform GetGridEntityNameTarget(ClusterGridEntity entity)
	{
		ClusterMapVisualizer clusterMapVisualizer;
		if (this.m_currentZoomScale >= 115f && this.m_gridEntityVis.TryGetValue(entity, out clusterMapVisualizer))
		{
			return clusterMapVisualizer.nameTarget;
		}
		return null;
	}

	// Token: 0x060095B5 RID: 38325 RVA: 0x0039D8F8 File Offset: 0x0039BAF8
	public override void ScreenUpdate(bool topLevel)
	{
		float t = Mathf.Min(4f * Time.unscaledDeltaTime, 0.9f);
		this.m_currentZoomScale = Mathf.Lerp(this.m_currentZoomScale, this.m_targetZoomScale, t);
		Vector2 v = KInputManager.GetMousePos();
		Vector3 b = this.mapScrollRect.content.InverseTransformPoint(v);
		this.mapScrollRect.content.localScale = new Vector3(this.m_currentZoomScale, this.m_currentZoomScale, 1f);
		Vector3 a = this.mapScrollRect.content.InverseTransformPoint(v);
		this.mapScrollRect.content.localPosition += (a - b) * this.m_currentZoomScale;
		this.MoveToNISPosition();
		this.FloatyAsteroidAnimation();
	}

	// Token: 0x060095B6 RID: 38326 RVA: 0x0039D9CC File Offset: 0x0039BBCC
	private void FloatyAsteroidAnimation()
	{
		float num = 0f;
		foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
		{
			AsteroidGridEntity component = worldContainer.GetComponent<AsteroidGridEntity>();
			if (component != null && this.m_gridEntityVis.ContainsKey(component) && ClusterMapScreen.GetRevealLevel(component) == ClusterRevealLevel.Visible)
			{
				KAnimControllerBase firstAnimController = this.m_gridEntityVis[component].GetFirstAnimController();
				float y = this.floatCycleOffset + this.floatCycleScale * Mathf.Sin(this.floatCycleSpeed * (num + GameClock.Instance.GetTime()));
				firstAnimController.Offset = new Vector2(0f, y);
			}
			num += 1f;
		}
	}

	// Token: 0x060095B7 RID: 38327 RVA: 0x0039DA9C File Offset: 0x0039BC9C
	private void SetupVisGameObjects()
	{
		foreach (KeyValuePair<AxialI, List<ClusterGridEntity>> keyValuePair in ClusterGrid.Instance.cellContents)
		{
			foreach (ClusterGridEntity clusterGridEntity in keyValuePair.Value)
			{
				ClusterGrid.Instance.GetCellRevealLevel(keyValuePair.Key);
				ClusterRevealLevel isVisibleInFOW = clusterGridEntity.IsVisibleInFOW;
				ClusterRevealLevel revealLevel = ClusterMapScreen.GetRevealLevel(clusterGridEntity);
				if (clusterGridEntity.IsVisible && revealLevel != ClusterRevealLevel.Hidden && !this.m_gridEntityVis.ContainsKey(clusterGridEntity))
				{
					ClusterMapVisualizer original = null;
					GameObject gameObject = null;
					switch (clusterGridEntity.Layer)
					{
					case EntityLayer.Asteroid:
						original = this.terrainVisPrefab;
						gameObject = this.terrainVisContainer;
						break;
					case EntityLayer.Craft:
						original = this.mobileVisPrefab;
						gameObject = this.mobileVisContainer;
						break;
					case EntityLayer.POI:
						original = this.staticVisPrefab;
						gameObject = this.POIVisContainer;
						break;
					case EntityLayer.Telescope:
						original = this.staticVisPrefab;
						gameObject = this.telescopeVisContainer;
						break;
					case EntityLayer.Payload:
						original = this.mobileVisPrefab;
						gameObject = this.mobileVisContainer;
						break;
					case EntityLayer.FX:
						original = this.staticVisPrefab;
						gameObject = this.FXVisContainer;
						break;
					}
					ClusterNameDisplayScreen.Instance.AddNewEntry(clusterGridEntity);
					ClusterMapVisualizer clusterMapVisualizer = UnityEngine.Object.Instantiate<ClusterMapVisualizer>(original, gameObject.transform);
					clusterMapVisualizer.Init(clusterGridEntity, this.pathDrawer);
					clusterMapVisualizer.gameObject.SetActive(true);
					this.m_gridEntityAnims.Add(clusterGridEntity, clusterMapVisualizer);
					this.m_gridEntityVis.Add(clusterGridEntity, clusterMapVisualizer);
					clusterGridEntity.positionDirty = false;
					clusterGridEntity.Subscribe(1502190696, new Action<object>(this.RemoveDeletedEntities));
				}
			}
		}
		this.RemoveDeletedEntities(null);
		foreach (KeyValuePair<ClusterGridEntity, ClusterMapVisualizer> keyValuePair2 in this.m_gridEntityVis)
		{
			ClusterGridEntity key = keyValuePair2.Key;
			if (key.Layer == EntityLayer.Asteroid)
			{
				int id = key.GetComponent<WorldContainer>().id;
				keyValuePair2.Value.alertVignette.worldID = id;
			}
		}
	}

	// Token: 0x060095B8 RID: 38328 RVA: 0x0039DD20 File Offset: 0x0039BF20
	private void RemoveDeletedEntities(object obj = null)
	{
		foreach (ClusterGridEntity key in (from x in this.m_gridEntityVis.Keys
		where x == null || x.gameObject == (GameObject)obj
		select x).ToList<ClusterGridEntity>())
		{
			global::Util.KDestroyGameObject(this.m_gridEntityVis[key]);
			this.m_gridEntityVis.Remove(key);
			this.m_gridEntityAnims.Remove(key);
		}
	}

	// Token: 0x060095B9 RID: 38329 RVA: 0x0010187A File Offset: 0x000FFA7A
	private void OnClusterLocationChanged(object data)
	{
		this.UpdateVis(null);
	}

	// Token: 0x060095BA RID: 38330 RVA: 0x0039DDC0 File Offset: 0x0039BFC0
	public static ClusterRevealLevel GetRevealLevel(ClusterGridEntity entity)
	{
		ClusterRevealLevel cellRevealLevel = ClusterGrid.Instance.GetCellRevealLevel(entity.Location);
		ClusterRevealLevel isVisibleInFOW = entity.IsVisibleInFOW;
		if (cellRevealLevel == ClusterRevealLevel.Visible || isVisibleInFOW == ClusterRevealLevel.Visible)
		{
			return ClusterRevealLevel.Visible;
		}
		if (cellRevealLevel == ClusterRevealLevel.Peeked && isVisibleInFOW == ClusterRevealLevel.Peeked)
		{
			return ClusterRevealLevel.Peeked;
		}
		return ClusterRevealLevel.Hidden;
	}

	// Token: 0x060095BB RID: 38331 RVA: 0x0039DDFC File Offset: 0x0039BFFC
	private void UpdateVis(object data = null)
	{
		this.SetupVisGameObjects();
		this.UpdatePaths();
		foreach (KeyValuePair<ClusterGridEntity, ClusterMapVisualizer> keyValuePair in this.m_gridEntityAnims)
		{
			ClusterRevealLevel revealLevel = ClusterMapScreen.GetRevealLevel(keyValuePair.Key);
			keyValuePair.Value.Show(revealLevel);
			bool selected = this.m_selectedEntity == keyValuePair.Key;
			keyValuePair.Value.Select(selected);
			if (keyValuePair.Key.positionDirty)
			{
				Vector3 position = ClusterGrid.Instance.GetPosition(keyValuePair.Key);
				keyValuePair.Value.rectTransform().SetLocalPosition(position);
				keyValuePair.Key.positionDirty = false;
			}
		}
		if (this.m_selectedEntity != null && this.m_gridEntityVis.ContainsKey(this.m_selectedEntity))
		{
			ClusterMapVisualizer clusterMapVisualizer = this.m_gridEntityVis[this.m_selectedEntity];
			this.m_selectMarker.SetTargetTransform(clusterMapVisualizer.transform);
			this.m_selectMarker.gameObject.SetActive(true);
			clusterMapVisualizer.transform.SetAsLastSibling();
		}
		else
		{
			this.m_selectMarker.gameObject.SetActive(false);
		}
		foreach (KeyValuePair<AxialI, ClusterMapVisualizer> keyValuePair2 in this.m_cellVisByLocation)
		{
			ClusterMapHex component = keyValuePair2.Value.GetComponent<ClusterMapHex>();
			AxialI key = keyValuePair2.Key;
			component.SetRevealed(ClusterGrid.Instance.GetCellRevealLevel(key));
		}
		this.UpdateHexToggleStates();
		this.FloatyAsteroidAnimation();
	}

	// Token: 0x060095BC RID: 38332 RVA: 0x001018AC File Offset: 0x000FFAAC
	private void OnEntityDestroyed(object obj)
	{
		this.RemoveDeletedEntities(null);
	}

	// Token: 0x060095BD RID: 38333 RVA: 0x0039DFBC File Offset: 0x0039C1BC
	private void UpdateHexToggleStates()
	{
		bool flag = this.m_hoveredHex != null && ClusterGrid.Instance.GetVisibleEntityOfLayerAtCell(this.m_hoveredHex.location, EntityLayer.Asteroid);
		foreach (KeyValuePair<AxialI, ClusterMapVisualizer> keyValuePair in this.m_cellVisByLocation)
		{
			ClusterMapHex component = keyValuePair.Value.GetComponent<ClusterMapHex>();
			AxialI key = keyValuePair.Key;
			ClusterMapHex.ToggleState state;
			if (this.m_selectedHex != null && this.m_selectedHex.location == key)
			{
				state = ClusterMapHex.ToggleState.Selected;
			}
			else if (flag && this.m_hoveredHex.location.IsAdjacent(key))
			{
				state = ClusterMapHex.ToggleState.OrbitHighlight;
			}
			else
			{
				state = ClusterMapHex.ToggleState.Unselected;
			}
			component.UpdateToggleState(state);
		}
	}

	// Token: 0x060095BE RID: 38334 RVA: 0x0039E094 File Offset: 0x0039C294
	public void SelectEntity(ClusterGridEntity entity, bool frameDelay = false)
	{
		if (entity != null)
		{
			this.SetSelectedEntity(entity, frameDelay);
			ClusterMapHex component = this.m_cellVisByLocation[entity.Location].GetComponent<ClusterMapHex>();
			this.m_selectedHex = component;
		}
		this.UpdateVis(null);
	}

	// Token: 0x060095BF RID: 38335 RVA: 0x0039E0D8 File Offset: 0x0039C2D8
	public void SelectHex(ClusterMapHex newSelectionHex)
	{
		if (this.m_mode == ClusterMapScreen.Mode.Default)
		{
			List<ClusterGridEntity> visibleEntitiesAtCell = ClusterGrid.Instance.GetVisibleEntitiesAtCell(newSelectionHex.location);
			for (int i = visibleEntitiesAtCell.Count - 1; i >= 0; i--)
			{
				KSelectable component = visibleEntitiesAtCell[i].GetComponent<KSelectable>();
				if (component == null || !component.IsSelectable)
				{
					visibleEntitiesAtCell.RemoveAt(i);
				}
			}
			if (visibleEntitiesAtCell.Count == 0)
			{
				this.SetSelectedEntity(null, false);
			}
			else
			{
				int num = visibleEntitiesAtCell.IndexOf(this.m_selectedEntity);
				int index = 0;
				if (num >= 0)
				{
					index = (num + 1) % visibleEntitiesAtCell.Count;
				}
				this.SetSelectedEntity(visibleEntitiesAtCell[index], false);
			}
			this.m_selectedHex = newSelectionHex;
		}
		else if (this.m_mode == ClusterMapScreen.Mode.SelectDestination)
		{
			global::Debug.Assert(this.m_destinationSelector != null, "Selected a hex in SelectDestination mode with no ClusterDestinationSelector");
			if (ClusterGrid.Instance.GetPath(this.m_selectedHex.location, newSelectionHex.location, this.m_destinationSelector) != null)
			{
				this.m_destinationSelector.SetDestination(newSelectionHex.location);
				if (this.m_closeOnSelect)
				{
					ManagementMenu.Instance.CloseAll();
				}
				else
				{
					this.SetMode(ClusterMapScreen.Mode.Default);
				}
			}
		}
		this.UpdateVis(null);
	}

	// Token: 0x060095C0 RID: 38336 RVA: 0x001018B5 File Offset: 0x000FFAB5
	public bool HasCurrentHover()
	{
		return this.m_hoveredHex != null;
	}

	// Token: 0x060095C1 RID: 38337 RVA: 0x001018C3 File Offset: 0x000FFAC3
	public AxialI GetCurrentHoverLocation()
	{
		return this.m_hoveredHex.location;
	}

	// Token: 0x060095C2 RID: 38338 RVA: 0x001018D0 File Offset: 0x000FFAD0
	public void OnHoverHex(ClusterMapHex newHoverHex)
	{
		this.m_hoveredHex = newHoverHex;
		if (this.m_mode == ClusterMapScreen.Mode.SelectDestination)
		{
			this.UpdateVis(null);
		}
		this.UpdateHexToggleStates();
	}

	// Token: 0x060095C3 RID: 38339 RVA: 0x001018EF File Offset: 0x000FFAEF
	public void OnUnhoverHex(ClusterMapHex unhoveredHex)
	{
		if (this.m_hoveredHex == unhoveredHex)
		{
			this.m_hoveredHex = null;
			this.UpdateHexToggleStates();
		}
	}

	// Token: 0x060095C4 RID: 38340 RVA: 0x0010190C File Offset: 0x000FFB0C
	public void SetLocationHighlight(AxialI location, bool highlight)
	{
		this.m_cellVisByLocation[location].GetComponent<ClusterMapHex>().ChangeState(highlight ? 1 : 0);
	}

	// Token: 0x060095C5 RID: 38341 RVA: 0x0039E1F8 File Offset: 0x0039C3F8
	private void UpdatePaths()
	{
		ClusterDestinationSelector clusterDestinationSelector = (this.m_selectedEntity != null) ? this.m_selectedEntity.GetComponent<ClusterDestinationSelector>() : null;
		if (this.m_mode != ClusterMapScreen.Mode.SelectDestination || !(this.m_hoveredHex != null))
		{
			if (this.m_previewMapPath != null)
			{
				global::Util.KDestroyGameObject(this.m_previewMapPath);
				this.m_previewMapPath = null;
			}
			return;
		}
		global::Debug.Assert(this.m_destinationSelector != null, "In SelectDestination mode without a destination selector");
		AxialI myWorldLocation = this.m_destinationSelector.GetMyWorldLocation();
		string text;
		List<AxialI> path = ClusterGrid.Instance.GetPath(myWorldLocation, this.m_hoveredHex.location, this.m_destinationSelector, out text, false);
		if (path != null)
		{
			if (this.m_previewMapPath == null)
			{
				this.m_previewMapPath = this.pathDrawer.AddPath();
			}
			ClusterMapVisualizer clusterMapVisualizer = this.m_gridEntityVis[this.GetSelectorGridEntity(this.m_destinationSelector)];
			this.m_previewMapPath.SetPoints(ClusterMapPathDrawer.GetDrawPathList(clusterMapVisualizer.transform.localPosition, path));
			this.m_previewMapPath.SetColor(this.rocketPreviewPathColor);
		}
		else if (this.m_previewMapPath != null)
		{
			global::Util.KDestroyGameObject(this.m_previewMapPath);
			this.m_previewMapPath = null;
		}
		int num = (path != null) ? path.Count : -1;
		if (this.m_selectedEntity != null)
		{
			int rangeInTiles = this.m_selectedEntity.GetComponent<IClusterRange>().GetRangeInTiles();
			if (num > rangeInTiles && string.IsNullOrEmpty(text))
			{
				text = string.Format(UI.CLUSTERMAP.TOOLTIP_INVALID_DESTINATION_OUT_OF_RANGE, rangeInTiles);
			}
			bool repeat = clusterDestinationSelector.GetComponent<RocketClusterDestinationSelector>().Repeat;
			this.m_hoveredHex.SetDestinationStatus(text, num, rangeInTiles, repeat);
			return;
		}
		this.m_hoveredHex.SetDestinationStatus(text);
	}

	// Token: 0x060095C6 RID: 38342 RVA: 0x0039E3B4 File Offset: 0x0039C5B4
	private ClusterGridEntity GetSelectorGridEntity(ClusterDestinationSelector selector)
	{
		ClusterGridEntity component = selector.GetComponent<ClusterGridEntity>();
		if (component != null && ClusterGrid.Instance.IsVisible(component))
		{
			return component;
		}
		ClusterGridEntity visibleEntityOfLayerAtCell = ClusterGrid.Instance.GetVisibleEntityOfLayerAtCell(selector.GetMyWorldLocation(), EntityLayer.Asteroid);
		global::Debug.Assert(component != null || visibleEntityOfLayerAtCell != null, string.Format("{0} has no grid entity and isn't located at a visible asteroid at {1}", selector, selector.GetMyWorldLocation()));
		if (visibleEntityOfLayerAtCell)
		{
			return visibleEntityOfLayerAtCell;
		}
		return component;
	}

	// Token: 0x060095C7 RID: 38343 RVA: 0x0039E42C File Offset: 0x0039C62C
	private void UpdateTearStatus()
	{
		ClusterPOIManager clusterPOIManager = null;
		if (ClusterManager.Instance != null)
		{
			clusterPOIManager = ClusterManager.Instance.GetComponent<ClusterPOIManager>();
		}
		if (clusterPOIManager != null)
		{
			TemporalTear temporalTear = clusterPOIManager.GetTemporalTear();
			if (temporalTear != null)
			{
				temporalTear.UpdateStatus();
			}
		}
	}

	// Token: 0x04007431 RID: 29745
	public static ClusterMapScreen Instance;

	// Token: 0x04007432 RID: 29746
	public GameObject cellVisContainer;

	// Token: 0x04007433 RID: 29747
	public GameObject terrainVisContainer;

	// Token: 0x04007434 RID: 29748
	public GameObject mobileVisContainer;

	// Token: 0x04007435 RID: 29749
	public GameObject telescopeVisContainer;

	// Token: 0x04007436 RID: 29750
	public GameObject POIVisContainer;

	// Token: 0x04007437 RID: 29751
	public GameObject FXVisContainer;

	// Token: 0x04007438 RID: 29752
	public ClusterMapVisualizer cellVisPrefab;

	// Token: 0x04007439 RID: 29753
	public ClusterMapVisualizer terrainVisPrefab;

	// Token: 0x0400743A RID: 29754
	public ClusterMapVisualizer mobileVisPrefab;

	// Token: 0x0400743B RID: 29755
	public ClusterMapVisualizer staticVisPrefab;

	// Token: 0x0400743C RID: 29756
	public Color rocketPathColor;

	// Token: 0x0400743D RID: 29757
	public Color rocketSelectedPathColor;

	// Token: 0x0400743E RID: 29758
	public Color rocketPreviewPathColor;

	// Token: 0x0400743F RID: 29759
	private ClusterMapHex m_selectedHex;

	// Token: 0x04007440 RID: 29760
	private ClusterMapHex m_hoveredHex;

	// Token: 0x04007441 RID: 29761
	private ClusterGridEntity m_selectedEntity;

	// Token: 0x04007442 RID: 29762
	public KButton closeButton;

	// Token: 0x04007443 RID: 29763
	private const float ZOOM_SCALE_MIN = 50f;

	// Token: 0x04007444 RID: 29764
	private const float ZOOM_SCALE_MAX = 150f;

	// Token: 0x04007445 RID: 29765
	private const float ZOOM_SCALE_INCREMENT = 25f;

	// Token: 0x04007446 RID: 29766
	private const float ZOOM_SCALE_SPEED = 4f;

	// Token: 0x04007447 RID: 29767
	private const float ZOOM_NAME_THRESHOLD = 115f;

	// Token: 0x04007448 RID: 29768
	private float m_currentZoomScale = 75f;

	// Token: 0x04007449 RID: 29769
	private float m_targetZoomScale = 75f;

	// Token: 0x0400744A RID: 29770
	private ClusterMapPath m_previewMapPath;

	// Token: 0x0400744B RID: 29771
	private Dictionary<ClusterGridEntity, ClusterMapVisualizer> m_gridEntityVis = new Dictionary<ClusterGridEntity, ClusterMapVisualizer>();

	// Token: 0x0400744C RID: 29772
	private Dictionary<ClusterGridEntity, ClusterMapVisualizer> m_gridEntityAnims = new Dictionary<ClusterGridEntity, ClusterMapVisualizer>();

	// Token: 0x0400744D RID: 29773
	private Dictionary<AxialI, ClusterMapVisualizer> m_cellVisByLocation = new Dictionary<AxialI, ClusterMapVisualizer>();

	// Token: 0x0400744E RID: 29774
	private Action<object> m_onDestinationChangedDelegate;

	// Token: 0x0400744F RID: 29775
	private Action<object> m_onSelectObjectDelegate;

	// Token: 0x04007450 RID: 29776
	[SerializeField]
	private KScrollRect mapScrollRect;

	// Token: 0x04007451 RID: 29777
	[SerializeField]
	private float scrollSpeed = 15f;

	// Token: 0x04007452 RID: 29778
	public GameObject selectMarkerPrefab;

	// Token: 0x04007453 RID: 29779
	public ClusterMapPathDrawer pathDrawer;

	// Token: 0x04007454 RID: 29780
	private SelectMarker m_selectMarker;

	// Token: 0x04007455 RID: 29781
	private bool movingToTargetNISPosition;

	// Token: 0x04007456 RID: 29782
	private Vector3 targetNISPosition;

	// Token: 0x04007457 RID: 29783
	private float targetNISZoom;

	// Token: 0x04007458 RID: 29784
	private AxialI selectOnMoveNISComplete;

	// Token: 0x04007459 RID: 29785
	private ClusterMapScreen.Mode m_mode;

	// Token: 0x0400745A RID: 29786
	private ClusterDestinationSelector m_destinationSelector;

	// Token: 0x0400745B RID: 29787
	private bool m_closeOnSelect;

	// Token: 0x0400745C RID: 29788
	private Coroutine activeMoveToTargetRoutine;

	// Token: 0x0400745D RID: 29789
	public float floatCycleScale = 4f;

	// Token: 0x0400745E RID: 29790
	public float floatCycleOffset = 0.75f;

	// Token: 0x0400745F RID: 29791
	public float floatCycleSpeed = 0.75f;

	// Token: 0x02001C1F RID: 7199
	public enum Mode
	{
		// Token: 0x04007461 RID: 29793
		Default,
		// Token: 0x04007462 RID: 29794
		SelectDestination
	}
}
