using System.Collections.Generic;
using FMOD.Studio;
using Rendering;
using STRINGS;
using UnityEngine;

public class BuildTool : DragTool
{
	[SerializeField]
	private TextStyleSetting tooltipStyle;

	private int lastCell = -1;

	private int lastDragCell = -1;

	private Orientation lastDragOrientation;

	private IList<Tag> selectedElements;

	private BuildingDef def;

	private Orientation buildingOrientation;

	private string facadeID;

	private ToolTip tooltip;

	public static BuildTool Instance;

	private bool active;

	private int buildingCount;

	public int GetLastCell => lastCell;

	public Orientation GetBuildingOrientation => buildingOrientation;

	public static void DestroyInstance()
	{
		Instance = null;
	}

	protected override void OnPrefabInit()
	{
		Instance = this;
		tooltip = GetComponent<ToolTip>();
		buildingCount = Random.Range(1, 14);
		canChangeDragAxis = false;
	}

	protected override void OnActivateTool()
	{
		lastDragCell = -1;
		if (visualizer != null)
		{
			ClearTilePreview();
			Object.Destroy(visualizer);
		}
		active = true;
		base.OnActivateTool();
		Vector3 vector = ClampPositionToWorld(PlayerController.GetCursorPos(KInputManager.GetMousePos()), ClusterManager.Instance.activeWorld);
		visualizer = GameUtil.KInstantiate(def.BuildingPreview, vector, Grid.SceneLayer.Ore, null, LayerMask.NameToLayer("Place"));
		KBatchedAnimController component = visualizer.GetComponent<KBatchedAnimController>();
		if (component != null)
		{
			component.visibilityType = KAnimControllerBase.VisibilityType.Always;
			component.isMovable = true;
			component.Offset = def.GetVisualizerOffset();
			component.name = component.GetComponent<KPrefabID>().GetDebugName() + "_visualizer";
		}
		if (!facadeID.IsNullOrWhiteSpace() && facadeID != "DEFAULT_FACADE")
		{
			visualizer.GetComponent<BuildingFacade>().ApplyBuildingFacade(Db.GetBuildingFacades().Get(facadeID));
		}
		Rotatable component2 = visualizer.GetComponent<Rotatable>();
		if (component2 != null)
		{
			buildingOrientation = def.InitialOrientation;
			component2.SetOrientation(buildingOrientation);
		}
		visualizer.SetActive(value: true);
		UpdateVis(vector);
		GetComponent<BuildToolHoverTextCard>().currentDef = def;
		ResourceRemainingDisplayScreen.instance.ActivateDisplay(visualizer);
		if (component == null)
		{
			visualizer.SetLayerRecursively(LayerMask.NameToLayer("Place"));
		}
		else
		{
			component.SetLayer(LayerMask.NameToLayer("Place"));
		}
		GridCompositor.Instance.ToggleMajor(on: true);
	}

	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		lastDragCell = -1;
		if (active)
		{
			active = false;
			GridCompositor.Instance.ToggleMajor(on: false);
			buildingOrientation = Orientation.Neutral;
			HideToolTip();
			ResourceRemainingDisplayScreen.instance.DeactivateDisplay();
			ClearTilePreview();
			Object.Destroy(visualizer);
			if (new_tool == SelectTool.Instance)
			{
				Game.Instance.Trigger(-1190690038);
			}
			base.OnDeactivateTool(new_tool);
		}
	}

	public void Activate(BuildingDef def, IList<Tag> selected_elements)
	{
		selectedElements = selected_elements;
		this.def = def;
		viewMode = def.ViewMode;
		ResourceRemainingDisplayScreen.instance.SetResources(selected_elements, def.CraftRecipe);
		PlayerController.Instance.ActivateTool(this);
		OnActivateTool();
	}

	public void Activate(BuildingDef def, IList<Tag> selected_elements, string facadeID)
	{
		this.facadeID = facadeID;
		Activate(def, selected_elements);
	}

	public void Deactivate()
	{
		selectedElements = null;
		SelectTool.Instance.Activate();
		def = null;
		facadeID = null;
		ResourceRemainingDisplayScreen.instance.DeactivateDisplay();
	}

	private void ClearTilePreview()
	{
		if (!Grid.IsValidBuildingCell(lastCell) || !def.IsTilePiece)
		{
			return;
		}
		GameObject gameObject = Grid.Objects[lastCell, (int)def.TileLayer];
		if (visualizer == gameObject)
		{
			Grid.Objects[lastCell, (int)def.TileLayer] = null;
		}
		if (def.isKAnimTile)
		{
			GameObject gameObject2 = null;
			if (def.ReplacementLayer != ObjectLayer.NumLayers)
			{
				gameObject2 = Grid.Objects[lastCell, (int)def.ReplacementLayer];
			}
			if ((gameObject == null || gameObject.GetComponent<Constructable>() == null) && (gameObject2 == null || gameObject2 == visualizer))
			{
				World.Instance.blockTileRenderer.RemoveBlock(def, isReplacement: false, SimHashes.Void, lastCell);
				World.Instance.blockTileRenderer.RemoveBlock(def, isReplacement: true, SimHashes.Void, lastCell);
				TileVisualizer.RefreshCell(lastCell, def.TileLayer, def.ReplacementLayer);
			}
		}
	}

	public override void OnMouseMove(Vector3 cursorPos)
	{
		base.OnMouseMove(cursorPos);
		cursorPos = ClampPositionToWorld(cursorPos, ClusterManager.Instance.activeWorld);
		UpdateVis(cursorPos);
	}

	private void UpdateVis(Vector3 pos)
	{
		bool flag = def.IsValidPlaceLocation(visualizer, pos, buildingOrientation, out var _);
		bool flag2 = def.IsValidReplaceLocation(pos, buildingOrientation, def.ReplacementLayer, def.ObjectLayer);
		flag = flag || flag2;
		if (visualizer != null)
		{
			Color c = Color.white;
			float strength = 0f;
			if (!flag)
			{
				c = Color.red;
				strength = 1f;
			}
			SetColor(visualizer, c, strength);
		}
		int num = Grid.PosToCell(pos);
		if (!(def != null))
		{
			return;
		}
		Vector3 vector = Grid.CellToPosCBC(num, def.SceneLayer);
		visualizer.transform.SetPosition(vector);
		base.transform.SetPosition(vector - Vector3.up * 0.5f);
		if (def.IsTilePiece)
		{
			ClearTilePreview();
			if (Grid.IsValidBuildingCell(num))
			{
				GameObject gameObject = Grid.Objects[num, (int)def.TileLayer];
				if (gameObject == null)
				{
					Grid.Objects[num, (int)def.TileLayer] = visualizer;
				}
				if (def.isKAnimTile)
				{
					GameObject gameObject2 = null;
					if (def.ReplacementLayer != ObjectLayer.NumLayers)
					{
						gameObject2 = Grid.Objects[num, (int)def.ReplacementLayer];
					}
					if (gameObject == null || (gameObject.GetComponent<Constructable>() == null && gameObject2 == null))
					{
						TileVisualizer.RefreshCell(num, def.TileLayer, def.ReplacementLayer);
						if (def.BlockTileAtlas != null)
						{
							int renderLayer = LayerMask.NameToLayer("Overlay");
							BlockTileRenderer blockTileRenderer = World.Instance.blockTileRenderer;
							blockTileRenderer.SetInvalidPlaceCell(num, !flag);
							if (lastCell != num)
							{
								blockTileRenderer.SetInvalidPlaceCell(lastCell, enabled: false);
							}
							blockTileRenderer.AddBlock(renderLayer, def, flag2, SimHashes.Void, num);
						}
					}
				}
			}
		}
		if (lastCell != num)
		{
			lastCell = num;
		}
	}

	public PermittedRotations? GetPermittedRotations()
	{
		if (visualizer == null)
		{
			return null;
		}
		Rotatable component = visualizer.GetComponent<Rotatable>();
		if (component == null)
		{
			return null;
		}
		return component.permittedRotations;
	}

	public bool CanRotate()
	{
		if (visualizer == null)
		{
			return false;
		}
		if (visualizer.GetComponent<Rotatable>() == null)
		{
			return false;
		}
		return true;
	}

	public void TryRotate()
	{
		if (visualizer == null)
		{
			return;
		}
		Rotatable component = visualizer.GetComponent<Rotatable>();
		if (!(component == null))
		{
			KFMOD.PlayUISound(GlobalAssets.GetSound("HUD_Rotate"));
			buildingOrientation = component.Rotate();
			if (Grid.IsValidBuildingCell(lastCell))
			{
				Vector3 pos = Grid.CellToPosCCC(lastCell, Grid.SceneLayer.Building);
				UpdateVis(pos);
			}
			if (base.Dragging && lastDragCell != -1)
			{
				TryBuild(lastDragCell);
			}
		}
	}

	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(Action.RotateBuilding))
		{
			TryRotate();
		}
		else
		{
			base.OnKeyDown(e);
		}
	}

	protected override void OnDragTool(int cell, int distFromOrigin)
	{
		TryBuild(cell);
	}

	private void TryBuild(int cell)
	{
		if (visualizer == null || (cell == lastDragCell && buildingOrientation == lastDragOrientation) || (Grid.PosToCell(visualizer) != cell && ((bool)def.BuildingComplete.GetComponent<LogicPorts>() || (bool)def.BuildingComplete.GetComponent<LogicGateBase>())))
		{
			return;
		}
		lastDragCell = cell;
		lastDragOrientation = buildingOrientation;
		ClearTilePreview();
		Vector3 pos = Grid.CellToPosCBC(cell, Grid.SceneLayer.Building);
		GameObject gameObject = null;
		PlanScreen.Instance.LastSelectedBuildingFacade = facadeID;
		bool flag = DebugHandler.InstantBuildMode || (Game.Instance.SandboxModeActive && SandboxToolParameterMenu.instance.settings.InstantBuild);
		string fail_reason;
		if (!flag)
		{
			gameObject = def.TryPlace(visualizer, pos, buildingOrientation, selectedElements, facadeID);
		}
		else if (def.IsValidBuildLocation(visualizer, pos, buildingOrientation) && def.IsValidPlaceLocation(visualizer, pos, buildingOrientation, out fail_reason))
		{
			float b = ElementLoader.GetMinMeltingPointAmongElements(selectedElements) - 10f;
			gameObject = def.Build(cell, buildingOrientation, null, selectedElements, Mathf.Min(def.Temperature, b), facadeID, playsound: false, GameClock.Instance.GetTime());
		}
		if (gameObject == null && def.ReplacementLayer != ObjectLayer.NumLayers)
		{
			GameObject replacementCandidate = def.GetReplacementCandidate(cell);
			if (replacementCandidate != null && !def.IsReplacementLayerOccupied(cell))
			{
				BuildingComplete component = replacementCandidate.GetComponent<BuildingComplete>();
				if (component != null && component.Def.Replaceable && def.CanReplace(replacementCandidate))
				{
					Tag tag = replacementCandidate.GetComponent<PrimaryElement>().Element.tag;
					if (tag.GetHash() == 1542131326)
					{
						tag = SimHashes.Snow.CreateTag();
					}
					if (component.Def != def || selectedElements[0] != tag)
					{
						string fail_reason2;
						if (!flag)
						{
							gameObject = def.TryReplaceTile(visualizer, pos, buildingOrientation, selectedElements, facadeID);
							Grid.Objects[cell, (int)def.ReplacementLayer] = gameObject;
						}
						else if (def.IsValidBuildLocation(visualizer, pos, buildingOrientation, replace_tile: true) && def.IsValidPlaceLocation(visualizer, pos, buildingOrientation, replace_tile: true, out fail_reason2))
						{
							gameObject = InstantBuildReplace(cell, pos, replacementCandidate);
						}
					}
				}
			}
		}
		PostProcessBuild(flag, pos, gameObject);
	}

	private GameObject InstantBuildReplace(int cell, Vector3 pos, GameObject tile)
	{
		if (tile.GetComponent<SimCellOccupier>() == null)
		{
			Object.Destroy(tile);
			float b = ElementLoader.GetMinMeltingPointAmongElements(selectedElements) - 10f;
			return def.Build(cell, buildingOrientation, null, selectedElements, Mathf.Min(def.Temperature, b), facadeID, playsound: false, GameClock.Instance.GetTime());
		}
		tile.GetComponent<SimCellOccupier>().DestroySelf(delegate
		{
			Object.Destroy(tile);
			float b2 = ElementLoader.GetMinMeltingPointAmongElements(selectedElements) - 10f;
			GameObject builtItem = def.Build(cell, buildingOrientation, null, selectedElements, Mathf.Min(def.Temperature, b2), facadeID, playsound: false, GameClock.Instance.GetTime());
			PostProcessBuild(instantBuild: true, pos, builtItem);
		});
		return null;
	}

	private void PostProcessBuild(bool instantBuild, Vector3 pos, GameObject builtItem)
	{
		if (builtItem == null)
		{
			return;
		}
		if (!instantBuild)
		{
			Prioritizable component = builtItem.GetComponent<Prioritizable>();
			if (component != null)
			{
				if (BuildMenu.Instance != null)
				{
					component.SetMasterPriority(BuildMenu.Instance.GetBuildingPriority());
				}
				if (PlanScreen.Instance != null)
				{
					component.SetMasterPriority(PlanScreen.Instance.GetBuildingPriority());
				}
			}
		}
		if (def.MaterialsAvailable(selectedElements, ClusterManager.Instance.activeWorld) || DebugHandler.InstantBuildMode)
		{
			placeSound = GlobalAssets.GetSound("Place_Building_" + def.AudioSize);
			if (placeSound != null)
			{
				buildingCount = buildingCount % 14 + 1;
				Vector3 pos2 = pos;
				pos2.z = 0f;
				EventInstance instance = SoundEvent.BeginOneShot(placeSound, pos2);
				if (def.AudioSize == "small")
				{
					instance.setParameterByName("tileCount", buildingCount);
				}
				SoundEvent.EndOneShot(instance);
			}
		}
		else
		{
			PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, UI.TOOLTIPS.NOMATERIAL, null, pos);
		}
		if (def.OnePerWorld)
		{
			PlayerController.Instance.ActivateTool(SelectTool.Instance);
		}
	}

	protected override Mode GetMode()
	{
		return Mode.Brush;
	}

	private void SetColor(GameObject root, Color c, float strength)
	{
		KBatchedAnimController component = root.GetComponent<KBatchedAnimController>();
		if (component != null)
		{
			component.TintColour = c;
		}
	}

	private void ShowToolTip()
	{
		ToolTipScreen.Instance.SetToolTip(tooltip);
	}

	private void HideToolTip()
	{
		ToolTipScreen.Instance.ClearToolTip(tooltip);
	}

	public void Update()
	{
		if (active)
		{
			KBatchedAnimController component = visualizer.GetComponent<KBatchedAnimController>();
			if (component != null)
			{
				component.SetLayer(LayerMask.NameToLayer("Place"));
			}
		}
	}

	public override string GetDeactivateSound()
	{
		return "HUD_Click_Deselect";
	}

	public override void OnLeftClickDown(Vector3 cursor_pos)
	{
		base.OnLeftClickDown(cursor_pos);
	}

	public override void OnLeftClickUp(Vector3 cursor_pos)
	{
		base.OnLeftClickUp(cursor_pos);
	}

	public void SetToolOrientation(Orientation orientation)
	{
		if (!(visualizer != null))
		{
			return;
		}
		Rotatable component = visualizer.GetComponent<Rotatable>();
		if (component != null)
		{
			buildingOrientation = orientation;
			component.SetOrientation(orientation);
			if (Grid.IsValidBuildingCell(lastCell))
			{
				Vector3 pos = Grid.CellToPosCCC(lastCell, Grid.SceneLayer.Building);
				UpdateVis(pos);
			}
			if (base.Dragging && lastDragCell != -1)
			{
				TryBuild(lastDragCell);
			}
		}
	}
}
