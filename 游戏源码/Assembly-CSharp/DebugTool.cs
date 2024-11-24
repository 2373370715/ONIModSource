using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001423 RID: 5155
public class DebugTool : DragTool
{
	// Token: 0x06006A73 RID: 27251 RVA: 0x000E5E73 File Offset: 0x000E4073
	public static void DestroyInstance()
	{
		DebugTool.Instance = null;
	}

	// Token: 0x06006A74 RID: 27252 RVA: 0x000E5E7B File Offset: 0x000E407B
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		DebugTool.Instance = this;
	}

	// Token: 0x06006A75 RID: 27253 RVA: 0x000E5D27 File Offset: 0x000E3F27
	public void Activate()
	{
		PlayerController.Instance.ActivateTool(this);
	}

	// Token: 0x06006A76 RID: 27254 RVA: 0x000E5E89 File Offset: 0x000E4089
	public void Activate(DebugTool.Type type)
	{
		this.type = type;
		this.Activate();
	}

	// Token: 0x06006A77 RID: 27255 RVA: 0x000E5E98 File Offset: 0x000E4098
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		PlayerController.Instance.ToolDeactivated(this);
	}

	// Token: 0x06006A78 RID: 27256 RVA: 0x002DEB74 File Offset: 0x002DCD74
	protected override void OnDragTool(int cell, int distFromOrigin)
	{
		if (Grid.IsValidCell(cell))
		{
			switch (this.type)
			{
			case DebugTool.Type.ReplaceSubstance:
				this.DoReplaceSubstance(cell);
				return;
			case DebugTool.Type.FillReplaceSubstance:
			{
				GameUtil.FloodFillNext.Value.Clear();
				GameUtil.FloodFillVisited.Value.Clear();
				SimHashes elem_hash = Grid.Element[cell].id;
				GameUtil.FloodFillConditional(cell, delegate(int check_cell)
				{
					bool result = false;
					if (Grid.Element[check_cell].id == elem_hash)
					{
						result = true;
						this.DoReplaceSubstance(check_cell);
					}
					return result;
				}, GameUtil.FloodFillVisited.Value, null);
				return;
			}
			case DebugTool.Type.Clear:
				this.ClearCell(cell);
				return;
			case DebugTool.Type.AddSelection:
				DebugBaseTemplateButton.Instance.AddToSelection(cell);
				return;
			case DebugTool.Type.RemoveSelection:
				DebugBaseTemplateButton.Instance.RemoveFromSelection(cell);
				return;
			case DebugTool.Type.Deconstruct:
				this.DeconstructCell(cell);
				return;
			case DebugTool.Type.Destroy:
				this.DestroyCell(cell);
				return;
			case DebugTool.Type.Sample:
				DebugPaintElementScreen.Instance.SampleCell(cell);
				return;
			case DebugTool.Type.StoreSubstance:
				this.DoStoreSubstance(cell);
				return;
			case DebugTool.Type.Dig:
				SimMessages.Dig(cell, -1, false);
				return;
			case DebugTool.Type.Heat:
				SimMessages.ModifyEnergy(cell, 10000f, 10000f, SimMessages.EnergySourceID.DebugHeat);
				return;
			case DebugTool.Type.Cool:
				SimMessages.ModifyEnergy(cell, -10000f, 10000f, SimMessages.EnergySourceID.DebugCool);
				return;
			case DebugTool.Type.AddPressure:
				SimMessages.ModifyMass(cell, 10000f, byte.MaxValue, 0, CellEventLogger.Instance.DebugToolModifyMass, 293f, SimHashes.Oxygen);
				return;
			case DebugTool.Type.RemovePressure:
				SimMessages.ModifyMass(cell, -10000f, byte.MaxValue, 0, CellEventLogger.Instance.DebugToolModifyMass, 0f, SimHashes.Oxygen);
				break;
			default:
				return;
			}
		}
	}

	// Token: 0x06006A79 RID: 27257 RVA: 0x002DECFC File Offset: 0x002DCEFC
	public void DoReplaceSubstance(int cell)
	{
		if (!Grid.IsValidBuildingCell(cell))
		{
			return;
		}
		Element element = DebugPaintElementScreen.Instance.paintElement.isOn ? ElementLoader.FindElementByHash(DebugPaintElementScreen.Instance.element) : ElementLoader.elements[(int)Grid.ElementIdx[cell]];
		if (element == null)
		{
			element = ElementLoader.FindElementByHash(SimHashes.Vacuum);
		}
		byte b = DebugPaintElementScreen.Instance.paintDisease.isOn ? DebugPaintElementScreen.Instance.diseaseIdx : Grid.DiseaseIdx[cell];
		float num = DebugPaintElementScreen.Instance.paintTemperature.isOn ? DebugPaintElementScreen.Instance.temperature : Grid.Temperature[cell];
		float num2 = DebugPaintElementScreen.Instance.paintMass.isOn ? DebugPaintElementScreen.Instance.mass : Grid.Mass[cell];
		int num3 = DebugPaintElementScreen.Instance.paintDiseaseCount.isOn ? DebugPaintElementScreen.Instance.diseaseCount : Grid.DiseaseCount[cell];
		if (num == -1f)
		{
			num = element.defaultValues.temperature;
		}
		if (num2 == -1f)
		{
			num2 = element.defaultValues.mass;
		}
		if (DebugPaintElementScreen.Instance.affectCells.isOn)
		{
			SimMessages.ReplaceElement(cell, element.id, CellEventLogger.Instance.DebugTool, num2, num, b, num3, -1);
			if (DebugPaintElementScreen.Instance.set_prevent_fow_reveal)
			{
				Grid.Visible[cell] = 0;
				Grid.PreventFogOfWarReveal[cell] = true;
			}
			else if (DebugPaintElementScreen.Instance.set_allow_fow_reveal && Grid.PreventFogOfWarReveal[cell])
			{
				Grid.PreventFogOfWarReveal[cell] = false;
			}
		}
		if (DebugPaintElementScreen.Instance.affectBuildings.isOn)
		{
			foreach (GameObject gameObject in new List<GameObject>
			{
				Grid.Objects[cell, 1],
				Grid.Objects[cell, 2],
				Grid.Objects[cell, 9],
				Grid.Objects[cell, 16],
				Grid.Objects[cell, 12],
				Grid.Objects[cell, 16],
				Grid.Objects[cell, 26]
			})
			{
				if (gameObject != null)
				{
					PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
					if (num > 0f)
					{
						component.Temperature = num;
					}
					if (num3 > 0 && b != 255)
					{
						component.ModifyDiseaseCount(int.MinValue, "DebugTool.DoReplaceSubstance");
						component.AddDisease(b, num3, "DebugTool.DoReplaceSubstance");
					}
				}
			}
		}
	}

	// Token: 0x06006A7A RID: 27258 RVA: 0x000E5EAC File Offset: 0x000E40AC
	public void DeconstructCell(int cell)
	{
		bool instantBuildMode = DebugHandler.InstantBuildMode;
		DebugHandler.InstantBuildMode = true;
		DeconstructTool.Instance.DeconstructCell(cell);
		if (!instantBuildMode)
		{
			DebugHandler.InstantBuildMode = false;
		}
	}

	// Token: 0x06006A7B RID: 27259 RVA: 0x002DEFC0 File Offset: 0x002DD1C0
	public void DestroyCell(int cell)
	{
		foreach (GameObject gameObject in new List<GameObject>
		{
			Grid.Objects[cell, 2],
			Grid.Objects[cell, 1],
			Grid.Objects[cell, 12],
			Grid.Objects[cell, 16],
			Grid.Objects[cell, 20],
			Grid.Objects[cell, 0],
			Grid.Objects[cell, 26],
			Grid.Objects[cell, 31],
			Grid.Objects[cell, 30]
		})
		{
			if (gameObject != null)
			{
				Util.KDestroyGameObject(gameObject);
			}
		}
		this.ClearCell(cell);
		if (ElementLoader.elements[(int)Grid.ElementIdx[cell]].id == SimHashes.Void)
		{
			SimMessages.ReplaceElement(cell, SimHashes.Void, CellEventLogger.Instance.DebugTool, 0f, 0f, byte.MaxValue, 0, -1);
			return;
		}
		SimMessages.ReplaceElement(cell, SimHashes.Vacuum, CellEventLogger.Instance.DebugTool, 0f, 0f, byte.MaxValue, 0, -1);
	}

	// Token: 0x06006A7C RID: 27260 RVA: 0x002DF138 File Offset: 0x002DD338
	public void ClearCell(int cell)
	{
		Vector2I vector2I = Grid.CellToXY(cell);
		ListPool<ScenePartitionerEntry, DebugTool>.PooledList pooledList = ListPool<ScenePartitionerEntry, DebugTool>.Allocate();
		GameScenePartitioner.Instance.GatherEntries(vector2I.x, vector2I.y, 1, 1, GameScenePartitioner.Instance.pickupablesLayer, pooledList);
		for (int i = 0; i < pooledList.Count; i++)
		{
			Pickupable pickupable = pooledList[i].obj as Pickupable;
			if (pickupable != null && pickupable.GetComponent<MinionBrain>() == null)
			{
				Util.KDestroyGameObject(pickupable.gameObject);
			}
		}
		pooledList.Recycle();
	}

	// Token: 0x06006A7D RID: 27261 RVA: 0x002DF1C0 File Offset: 0x002DD3C0
	public void DoStoreSubstance(int cell)
	{
		if (!Grid.IsValidBuildingCell(cell))
		{
			return;
		}
		GameObject gameObject = Grid.Objects[cell, 1];
		if (gameObject == null)
		{
			return;
		}
		Storage component = gameObject.GetComponent<Storage>();
		if (component == null)
		{
			return;
		}
		Element element = DebugPaintElementScreen.Instance.paintElement.isOn ? ElementLoader.FindElementByHash(DebugPaintElementScreen.Instance.element) : ElementLoader.elements[(int)Grid.ElementIdx[cell]];
		if (element == null)
		{
			element = ElementLoader.FindElementByHash(SimHashes.Vacuum);
		}
		byte disease_idx = DebugPaintElementScreen.Instance.paintDisease.isOn ? DebugPaintElementScreen.Instance.diseaseIdx : Grid.DiseaseIdx[cell];
		float num = DebugPaintElementScreen.Instance.paintTemperature.isOn ? DebugPaintElementScreen.Instance.temperature : element.defaultValues.temperature;
		float num2 = DebugPaintElementScreen.Instance.paintMass.isOn ? DebugPaintElementScreen.Instance.mass : element.defaultValues.mass;
		if (num == -1f)
		{
			num = element.defaultValues.temperature;
		}
		if (num2 == -1f)
		{
			num2 = element.defaultValues.mass;
		}
		int disease_count = DebugPaintElementScreen.Instance.paintDiseaseCount.isOn ? DebugPaintElementScreen.Instance.diseaseCount : 0;
		if (element.IsGas)
		{
			component.AddGasChunk(element.id, num2, num, disease_idx, disease_count, false, true);
			return;
		}
		if (element.IsLiquid)
		{
			component.AddLiquid(element.id, num2, num, disease_idx, disease_count, false, true);
			return;
		}
		if (element.IsSolid)
		{
			component.AddOre(element.id, num2, num, disease_idx, disease_count, false, true);
		}
	}

	// Token: 0x04005042 RID: 20546
	public static DebugTool Instance;

	// Token: 0x04005043 RID: 20547
	public DebugTool.Type type;

	// Token: 0x02001424 RID: 5156
	public enum Type
	{
		// Token: 0x04005045 RID: 20549
		ReplaceSubstance,
		// Token: 0x04005046 RID: 20550
		FillReplaceSubstance,
		// Token: 0x04005047 RID: 20551
		Clear,
		// Token: 0x04005048 RID: 20552
		AddSelection,
		// Token: 0x04005049 RID: 20553
		RemoveSelection,
		// Token: 0x0400504A RID: 20554
		Deconstruct,
		// Token: 0x0400504B RID: 20555
		Destroy,
		// Token: 0x0400504C RID: 20556
		Sample,
		// Token: 0x0400504D RID: 20557
		StoreSubstance,
		// Token: 0x0400504E RID: 20558
		Dig,
		// Token: 0x0400504F RID: 20559
		Heat,
		// Token: 0x04005050 RID: 20560
		Cool,
		// Token: 0x04005051 RID: 20561
		AddPressure,
		// Token: 0x04005052 RID: 20562
		RemovePressure,
		// Token: 0x04005053 RID: 20563
		PaintPlant
	}
}
