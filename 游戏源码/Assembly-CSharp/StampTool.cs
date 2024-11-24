using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001455 RID: 5205
public class StampTool : InterfaceTool
{
	// Token: 0x06006C0F RID: 27663 RVA: 0x000E7007 File Offset: 0x000E5207
	public static void DestroyInstance()
	{
		StampTool.Instance = null;
	}

	// Token: 0x06006C10 RID: 27664 RVA: 0x002E5014 File Offset: 0x002E3214
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		StampTool.Instance = this;
		this.preview = new StampToolPreview(this, new IStampToolPreviewPlugin[]
		{
			new StampToolPreview_Placers(this.PlacerPrefab),
			new StampToolPreview_Area(),
			new StampToolPreview_SolidLiquidGas(),
			new StampToolPreview_Prefabs()
		});
	}

	// Token: 0x06006C11 RID: 27665 RVA: 0x000E700F File Offset: 0x000E520F
	private void Update()
	{
		this.preview.Refresh(Grid.PosToCell(this.GetCursorPos()));
	}

	// Token: 0x06006C12 RID: 27666 RVA: 0x002E5068 File Offset: 0x002E3268
	public void Activate(TemplateContainer template, bool SelectAffected = false, bool DeactivateOnStamp = false)
	{
		this.selectAffected = SelectAffected;
		this.deactivateOnStamp = DeactivateOnStamp;
		if (this.stampTemplate == template || template == null || template.cells == null)
		{
			return;
		}
		this.stampTemplate = template;
		PlayerController.Instance.ActivateTool(this);
		base.StartCoroutine(this.preview.Setup(template));
	}

	// Token: 0x06006C13 RID: 27667 RVA: 0x000E7027 File Offset: 0x000E5227
	private Vector3 GetCursorPos()
	{
		return PlayerController.GetCursorPos(KInputManager.GetMousePos());
	}

	// Token: 0x06006C14 RID: 27668 RVA: 0x000E7033 File Offset: 0x000E5233
	public override void OnLeftClickDown(Vector3 cursor_pos)
	{
		base.OnLeftClickDown(cursor_pos);
		this.Stamp(cursor_pos);
	}

	// Token: 0x06006C15 RID: 27669 RVA: 0x002E50C0 File Offset: 0x002E32C0
	private void Stamp(Vector2 pos)
	{
		if (!this.ready)
		{
			return;
		}
		int cell = Grid.OffsetCell(Grid.PosToCell(pos), Mathf.FloorToInt(-this.stampTemplate.info.size.X / 2f), 0);
		int cell2 = Grid.OffsetCell(Grid.PosToCell(pos), Mathf.FloorToInt(this.stampTemplate.info.size.X / 2f), 0);
		int cell3 = Grid.OffsetCell(Grid.PosToCell(pos), 0, 1 + Mathf.FloorToInt(-this.stampTemplate.info.size.Y / 2f));
		int cell4 = Grid.OffsetCell(Grid.PosToCell(pos), 0, 1 + Mathf.FloorToInt(this.stampTemplate.info.size.Y / 2f));
		if (!Grid.IsValidBuildingCell(cell) || !Grid.IsValidBuildingCell(cell2) || !Grid.IsValidBuildingCell(cell4) || !Grid.IsValidBuildingCell(cell3))
		{
			return;
		}
		this.ready = false;
		bool pauseOnComplete = SpeedControlScreen.Instance.IsPaused;
		if (SpeedControlScreen.Instance.IsPaused)
		{
			SpeedControlScreen.Instance.Unpause(true);
		}
		if (this.stampTemplate.cells != null)
		{
			this.preview.OnPlace();
			List<GameObject> list = new List<GameObject>();
			for (int i = 0; i < this.stampTemplate.cells.Count; i++)
			{
				for (int j = 0; j < 34; j++)
				{
					GameObject gameObject = Grid.Objects[Grid.XYToCell((int)(pos.x + (float)this.stampTemplate.cells[i].location_x), (int)(pos.y + (float)this.stampTemplate.cells[i].location_y)), j];
					if (gameObject != null && !list.Contains(gameObject))
					{
						list.Add(gameObject);
					}
				}
			}
			foreach (GameObject gameObject2 in list)
			{
				if (gameObject2 != null)
				{
					Util.KDestroyGameObject(gameObject2);
				}
			}
		}
		TemplateLoader.Stamp(this.stampTemplate, pos, delegate
		{
			this.CompleteStamp(pauseOnComplete);
		});
		if (this.selectAffected)
		{
			DebugBaseTemplateButton.Instance.ClearSelection();
			if (this.stampTemplate.cells != null)
			{
				for (int k = 0; k < this.stampTemplate.cells.Count; k++)
				{
					DebugBaseTemplateButton.Instance.AddToSelection(Grid.XYToCell((int)(pos.x + (float)this.stampTemplate.cells[k].location_x), (int)(pos.y + (float)this.stampTemplate.cells[k].location_y)));
				}
			}
		}
		if (this.deactivateOnStamp)
		{
			base.DeactivateTool(null);
		}
	}

	// Token: 0x06006C16 RID: 27670 RVA: 0x000E7048 File Offset: 0x000E5248
	private void CompleteStamp(bool pause)
	{
		if (pause)
		{
			SpeedControlScreen.Instance.Pause(true, false);
		}
		this.ready = true;
		this.OnDeactivateTool(null);
	}

	// Token: 0x06006C17 RID: 27671 RVA: 0x000E7067 File Offset: 0x000E5267
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		if (base.gameObject.activeSelf)
		{
			return;
		}
		this.preview.Cleanup();
		this.stampTemplate = null;
	}

	// Token: 0x0400510D RID: 20749
	public static StampTool Instance;

	// Token: 0x0400510E RID: 20750
	private StampToolPreview preview;

	// Token: 0x0400510F RID: 20751
	public TemplateContainer stampTemplate;

	// Token: 0x04005110 RID: 20752
	public GameObject PlacerPrefab;

	// Token: 0x04005111 RID: 20753
	private bool ready = true;

	// Token: 0x04005112 RID: 20754
	private bool selectAffected;

	// Token: 0x04005113 RID: 20755
	private bool deactivateOnStamp;
}
