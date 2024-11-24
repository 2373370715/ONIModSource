using System.Collections.Generic;
using UnityEngine;

public class StampTool : InterfaceTool
{
	public static StampTool Instance;

	private StampToolPreview preview;

	public TemplateContainer stampTemplate;

	public GameObject PlacerPrefab;

	private bool ready = true;

	private bool selectAffected;

	private bool deactivateOnStamp;

	public static void DestroyInstance()
	{
		Instance = null;
	}

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Instance = this;
		preview = new StampToolPreview(this, new StampToolPreview_Placers(PlacerPrefab), new StampToolPreview_Area(), new StampToolPreview_SolidLiquidGas(), new StampToolPreview_Prefabs());
	}

	private void Update()
	{
		preview.Refresh(Grid.PosToCell(GetCursorPos()));
	}

	public void Activate(TemplateContainer template, bool SelectAffected = false, bool DeactivateOnStamp = false)
	{
		selectAffected = SelectAffected;
		deactivateOnStamp = DeactivateOnStamp;
		if (stampTemplate != template && template != null && template.cells != null)
		{
			stampTemplate = template;
			PlayerController.Instance.ActivateTool(this);
			StartCoroutine(preview.Setup(template));
		}
	}

	private Vector3 GetCursorPos()
	{
		return PlayerController.GetCursorPos(KInputManager.GetMousePos());
	}

	public override void OnLeftClickDown(Vector3 cursor_pos)
	{
		base.OnLeftClickDown(cursor_pos);
		Stamp(cursor_pos);
	}

	private void Stamp(Vector2 pos)
	{
		if (!ready)
		{
			return;
		}
		int cell = Grid.OffsetCell(Grid.PosToCell(pos), Mathf.FloorToInt((0f - stampTemplate.info.size.X) / 2f), 0);
		int cell2 = Grid.OffsetCell(Grid.PosToCell(pos), Mathf.FloorToInt(stampTemplate.info.size.X / 2f), 0);
		int cell3 = Grid.OffsetCell(Grid.PosToCell(pos), 0, 1 + Mathf.FloorToInt((0f - stampTemplate.info.size.Y) / 2f));
		int cell4 = Grid.OffsetCell(Grid.PosToCell(pos), 0, 1 + Mathf.FloorToInt(stampTemplate.info.size.Y / 2f));
		if (!Grid.IsValidBuildingCell(cell) || !Grid.IsValidBuildingCell(cell2) || !Grid.IsValidBuildingCell(cell4) || !Grid.IsValidBuildingCell(cell3))
		{
			return;
		}
		ready = false;
		bool pauseOnComplete = SpeedControlScreen.Instance.IsPaused;
		if (SpeedControlScreen.Instance.IsPaused)
		{
			SpeedControlScreen.Instance.Unpause();
		}
		if (stampTemplate.cells != null)
		{
			preview.OnPlace();
			List<GameObject> list = new List<GameObject>();
			for (int i = 0; i < stampTemplate.cells.Count; i++)
			{
				for (int j = 0; j < 34; j++)
				{
					GameObject gameObject = Grid.Objects[Grid.XYToCell((int)(pos.x + (float)stampTemplate.cells[i].location_x), (int)(pos.y + (float)stampTemplate.cells[i].location_y)), j];
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
			CompleteStamp(pauseOnComplete);
		});
		if (selectAffected)
		{
			DebugBaseTemplateButton.Instance.ClearSelection();
			if (stampTemplate.cells != null)
			{
				for (int k = 0; k < stampTemplate.cells.Count; k++)
				{
					DebugBaseTemplateButton.Instance.AddToSelection(Grid.XYToCell((int)(pos.x + (float)stampTemplate.cells[k].location_x), (int)(pos.y + (float)stampTemplate.cells[k].location_y)));
				}
			}
		}
		if (deactivateOnStamp)
		{
			DeactivateTool();
		}
	}

	private void CompleteStamp(bool pause)
	{
		if (pause)
		{
			SpeedControlScreen.Instance.Pause();
		}
		ready = true;
		OnDeactivateTool(null);
	}

	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		if (!base.gameObject.activeSelf)
		{
			preview.Cleanup();
			stampTemplate = null;
		}
	}
}
