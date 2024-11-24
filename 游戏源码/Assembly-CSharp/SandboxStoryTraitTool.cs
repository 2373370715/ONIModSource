using System;
using System.Collections.Generic;
using Database;
using STRINGS;
using TemplateClasses;
using UnityEngine;

public class SandboxStoryTraitTool : InterfaceTool
{
	private System.Action setupPreviewFn;

	private StampToolPreview preview;

	private bool isPlacingTemplate;

	private string prevError;

	private const float ERROR_UPDATE_FREQUENCY = 0.1f;

	private float timeUntilNextErrorUpdate = -1f;

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		preview = new StampToolPreview(this, new StampToolPreview_Area(), new StampToolPreview_SolidLiquidGas(), new StampToolPreview_Prefabs());
		setupPreviewFn = delegate
		{
			preview.Cleanup();
			if (TryGetStoryAndTemplate(out var _, out var stampTemplate))
			{
				StartCoroutine(preview.Setup(stampTemplate));
				preview.OnErrorChange(prevError);
			}
		};
	}

	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		SandboxToolParameterMenu.instance.gameObject.SetActive(value: true);
		SandboxToolParameterMenu.instance.DisableParameters();
		SandboxToolParameterMenu.instance.storySelector.row.SetActive(value: true);
		setupPreviewFn();
		SandboxSettings settings = SandboxToolParameterMenu.instance.settings;
		settings.OnChangeStory = (System.Action)Delegate.Remove(settings.OnChangeStory, setupPreviewFn);
		SandboxSettings settings2 = SandboxToolParameterMenu.instance.settings;
		settings2.OnChangeStory = (System.Action)Delegate.Combine(settings2.OnChangeStory, setupPreviewFn);
	}

	public void Update()
	{
		Vector3 cursorPos = PlayerController.GetCursorPos(KInputManager.GetMousePos());
		int originCell = Grid.PosToCell(cursorPos);
		preview.Refresh(originCell);
		timeUntilNextErrorUpdate -= Time.unscaledDeltaTime;
		if (timeUntilNextErrorUpdate <= 0f)
		{
			timeUntilNextErrorUpdate = 0.1f;
			Story story;
			TemplateContainer stampTemplate;
			string error = GetError(cursorPos, out story, out stampTemplate);
			if (prevError != error)
			{
				preview.OnErrorChange(error);
				prevError = error;
			}
		}
	}

	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		SandboxToolParameterMenu.instance.gameObject.SetActive(value: false);
		SandboxSettings settings = SandboxToolParameterMenu.instance.settings;
		settings.OnChangeStory = (System.Action)Delegate.Remove(settings.OnChangeStory, setupPreviewFn);
		preview.Cleanup();
	}

	public override void OnLeftClickDown(Vector3 cursor_pos)
	{
		base.OnLeftClickDown(cursor_pos);
		if (!isPlacingTemplate && GetError(cursor_pos, out var story, out var stampTemplate) == null)
		{
			isPlacingTemplate = true;
			Stamp(cursor_pos, stampTemplate, delegate
			{
				isPlacingTemplate = false;
				StoryInstance storyInstance = StoryManager.Instance.GetStoryInstance(story);
				StoryInstance.State currentState = storyInstance.CurrentState;
				storyInstance.CurrentState = StoryInstance.State.RETROFITTED;
				storyInstance.CurrentState = currentState;
			});
		}
	}

	public static void Stamp(Vector2 pos, TemplateContainer stampTemplate, System.Action onCompleteFn)
	{
		bool shouldPauseOnComplete = SpeedControlScreen.Instance.IsPaused;
		if (SpeedControlScreen.Instance.IsPaused)
		{
			SpeedControlScreen.Instance.Unpause(playSound: false);
		}
		if (stampTemplate.cells != null)
		{
			List<GameObject> list = new List<GameObject>();
			for (int i = 0; i < stampTemplate.cells.Count; i++)
			{
				for (int j = 0; j < 34; j++)
				{
					int cell = Grid.XYToCell((int)(pos.x + (float)stampTemplate.cells[i].location_x), (int)(pos.y + (float)stampTemplate.cells[i].location_y));
					GameObject gameObject = Grid.Objects[cell, j];
					if (gameObject != null && !list.Contains(gameObject))
					{
						list.Add(gameObject);
					}
				}
			}
			foreach (GameObject item in list)
			{
				if (item != null)
				{
					Util.KDestroyGameObject(item);
				}
			}
		}
		TemplateLoader.Stamp(stampTemplate, pos, delegate
		{
			if (shouldPauseOnComplete)
			{
				SpeedControlScreen.Instance.Pause(playSound: false);
			}
			onCompleteFn();
		});
		KFMOD.PlayUISound(GlobalAssets.GetSound("SandboxTool_Stamp"));
	}

	public static bool TryGetStoryAndTemplate(out Story story, out TemplateContainer stampTemplate)
	{
		stampTemplate = null;
		string stringSetting = SandboxToolParameterMenu.instance.settings.GetStringSetting("SandboxTools.SelectedStory");
		story = Db.Get().Stories.TryGet(stringSetting);
		if (story == null)
		{
			return false;
		}
		if (story.sandboxStampTemplateId == null)
		{
			return false;
		}
		stampTemplate = TemplateCache.GetTemplate(story.sandboxStampTemplateId);
		if (stampTemplate == null)
		{
			return false;
		}
		return true;
	}

	public string GetError(Vector3 stampPos, out Story story, out TemplateContainer stampTemplate)
	{
		if (!TryGetStoryAndTemplate(out story, out stampTemplate))
		{
			return "-";
		}
		TemplateContainer _stampTemplate = stampTemplate;
		if (StoryManager.Instance.GetStoryInstance(story) != null)
		{
			return UI.TOOLS.SANDBOX.SPAWN_STORY_TRAIT.ERROR_ALREADY_EXISTS.Replace("{StoryName}", Strings.Get(story.StoryTrait.name));
		}
		int num = Grid.OffsetCell(Grid.PosToCell(stampPos), Mathf.FloorToInt((0f - stampTemplate.info.size.X) / 2f), Mathf.FloorToInt((0f - stampTemplate.info.size.Y) / 2f) + 1);
		int num2 = Grid.OffsetCell(Grid.PosToCell(stampPos), Mathf.FloorToInt(stampTemplate.info.size.X / 2f), Mathf.FloorToInt(stampTemplate.info.size.Y / 2f) + 1);
		if (!Grid.IsValidBuildingCell(num) || ClusterManager.Instance.activeWorldId != Grid.WorldIdx[num] || !Grid.IsValidBuildingCell(num2) || ClusterManager.Instance.activeWorldId != Grid.WorldIdx[num2] || IsTrueForAnyStampCell((Cell cellInfo, int cellIndex) => Grid.Element[cellIndex].id == SimHashes.Unobtanium))
		{
			return UI.TOOLS.SANDBOX.SPAWN_STORY_TRAIT.ERROR_INVALID_LOCATION;
		}
		WorldContainer world = ClusterManager.Instance.GetWorld(ClusterManager.Instance.activeWorldId);
		if (world == null || world.IsModuleInterior)
		{
			return UI.TOOLS.SANDBOX.SPAWN_STORY_TRAIT.ERROR_INVALID_LOCATION;
		}
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		foreach (Brain brain in Components.Brains)
		{
			if (IsTrueForAnyStampCell(delegate(Cell cellInfo, int cellIndex)
			{
				int num3 = Grid.PosToCell(brain.gameObject);
				if (num3 == cellIndex)
				{
					return true;
				}
				for (int i = -1; i <= 1; i++)
				{
					for (int j = -1; j <= 2; j++)
					{
						if (Grid.OffsetCell(num3, i, j) == cellIndex)
						{
							return true;
						}
					}
				}
				return false;
			}))
			{
				if (brain.HasTag(GameTags.Minion))
				{
					flag = true;
				}
				else if (brain.HasTag(GameTags.Robot))
				{
					flag3 = true;
				}
				else if (brain.HasTag(GameTags.Creature))
				{
					flag2 = true;
				}
				break;
			}
		}
		if (flag)
		{
			return UI.TOOLS.SANDBOX.SPAWN_STORY_TRAIT.ERROR_DUPE_HAZARD;
		}
		if (flag3)
		{
			return UI.TOOLS.SANDBOX.SPAWN_STORY_TRAIT.ERROR_ROBOT_HAZARD;
		}
		if (flag2)
		{
			return UI.TOOLS.SANDBOX.SPAWN_STORY_TRAIT.ERROR_CREATURE_HAZARD;
		}
		if (IsTrueForAnyStampCell(delegate(Cell cellInfo, int cellIndex)
		{
			if (!Grid.ObjectLayers[1].TryGetValue(cellIndex, out var value))
			{
				return false;
			}
			return !value.GetComponent<KPrefabID>().HasTag(GameTags.Plant);
		}))
		{
			return UI.TOOLS.SANDBOX.SPAWN_STORY_TRAIT.ERROR_BUILDING_HAZARD;
		}
		return null;
		bool IsTrueForAnyStampCell(Func<Cell, int, bool> isTrueFn)
		{
			foreach (Cell cell in _stampTemplate.cells)
			{
				int arg = Grid.OffsetCell(Grid.PosToCell(stampPos), cell.location_x, cell.location_y);
				if (isTrueFn(cell, arg))
				{
					return true;
				}
			}
			return false;
		}
	}
}
