using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Klei;
using Klei.AI;
using ProcGen;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02001213 RID: 4627
[Serializable]
public class BuildingDef : Def
{
	// Token: 0x170005B3 RID: 1459
	// (get) Token: 0x06005E94 RID: 24212 RVA: 0x000DDC30 File Offset: 0x000DBE30
	public override string Name
	{
		get
		{
			return Strings.Get("STRINGS.BUILDINGS.PREFABS." + this.PrefabID.ToUpper() + ".NAME");
		}
	}

	// Token: 0x170005B4 RID: 1460
	// (get) Token: 0x06005E95 RID: 24213 RVA: 0x000DDC56 File Offset: 0x000DBE56
	public string Desc
	{
		get
		{
			return Strings.Get("STRINGS.BUILDINGS.PREFABS." + this.PrefabID.ToUpper() + ".DESC");
		}
	}

	// Token: 0x170005B5 RID: 1461
	// (get) Token: 0x06005E96 RID: 24214 RVA: 0x000DDC7C File Offset: 0x000DBE7C
	public string Flavor
	{
		get
		{
			return "\"" + Strings.Get("STRINGS.BUILDINGS.PREFABS." + this.PrefabID.ToUpper() + ".FLAVOR") + "\"";
		}
	}

	// Token: 0x170005B6 RID: 1462
	// (get) Token: 0x06005E97 RID: 24215 RVA: 0x000DDCB1 File Offset: 0x000DBEB1
	public string Effect
	{
		get
		{
			return Strings.Get("STRINGS.BUILDINGS.PREFABS." + this.PrefabID.ToUpper() + ".EFFECT");
		}
	}

	// Token: 0x170005B7 RID: 1463
	// (get) Token: 0x06005E98 RID: 24216 RVA: 0x000DDCD7 File Offset: 0x000DBED7
	public bool IsTilePiece
	{
		get
		{
			return this.TileLayer != ObjectLayer.NumLayers;
		}
	}

	// Token: 0x06005E99 RID: 24217 RVA: 0x000DDCE6 File Offset: 0x000DBEE6
	public bool CanReplace(GameObject go)
	{
		return this.ReplacementTags != null && go.GetComponent<KPrefabID>().HasAnyTags(this.ReplacementTags);
	}

	// Token: 0x06005E9A RID: 24218 RVA: 0x000DDD03 File Offset: 0x000DBF03
	public bool IsAvailable()
	{
		return !this.Deprecated && (!this.DebugOnly || Game.Instance.DebugOnlyBuildingsAllowed);
	}

	// Token: 0x06005E9B RID: 24219 RVA: 0x000DDD23 File Offset: 0x000DBF23
	public bool ShouldShowInBuildMenu()
	{
		return this.ShowInBuildMenu;
	}

	// Token: 0x06005E9C RID: 24220 RVA: 0x002A3BF0 File Offset: 0x002A1DF0
	public bool IsReplacementLayerOccupied(int cell)
	{
		if (Grid.Objects[cell, (int)this.ReplacementLayer] != null)
		{
			return true;
		}
		if (this.EquivalentReplacementLayers != null)
		{
			foreach (ObjectLayer layer in this.EquivalentReplacementLayers)
			{
				if (Grid.Objects[cell, (int)layer] != null)
				{
					return true;
				}
			}
			return false;
		}
		return false;
	}

	// Token: 0x06005E9D RID: 24221 RVA: 0x002A3C7C File Offset: 0x002A1E7C
	public GameObject GetReplacementCandidate(int cell)
	{
		if (this.ReplacementCandidateLayers != null)
		{
			using (List<ObjectLayer>.Enumerator enumerator = this.ReplacementCandidateLayers.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ObjectLayer objectLayer = enumerator.Current;
					if (Grid.ObjectLayers[(int)objectLayer].ContainsKey(cell))
					{
						GameObject gameObject = Grid.ObjectLayers[(int)objectLayer][cell];
						if (gameObject != null && gameObject.GetComponent<BuildingComplete>() != null)
						{
							return gameObject;
						}
					}
				}
				goto IL_96;
			}
		}
		if (Grid.ObjectLayers[(int)this.TileLayer].ContainsKey(cell))
		{
			return Grid.ObjectLayers[(int)this.TileLayer][cell];
		}
		IL_96:
		return null;
	}

	// Token: 0x06005E9E RID: 24222 RVA: 0x002A3D34 File Offset: 0x002A1F34
	public GameObject Create(Vector3 pos, Storage resource_storage, IList<Tag> selected_elements, Recipe recipe, float temperature, GameObject obj)
	{
		SimUtil.DiseaseInfo diseaseInfo = SimUtil.DiseaseInfo.Invalid;
		if (resource_storage != null)
		{
			Recipe.Ingredient[] allIngredients = recipe.GetAllIngredients(selected_elements);
			if (allIngredients != null)
			{
				foreach (Recipe.Ingredient ingredient in allIngredients)
				{
					float num;
					SimUtil.DiseaseInfo b;
					float num2;
					resource_storage.ConsumeAndGetDisease(ingredient.tag, ingredient.amount, out num, out b, out num2);
					diseaseInfo = SimUtil.CalculateFinalDiseaseInfo(diseaseInfo, b);
				}
			}
		}
		GameObject gameObject = GameUtil.KInstantiate(obj, pos, this.SceneLayer, null, 0);
		Element element = ElementLoader.GetElement(selected_elements[0]);
		global::Debug.Assert(element != null);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.ElementID = element.id;
		component.Temperature = temperature;
		component.AddDisease(diseaseInfo.idx, diseaseInfo.count, "BuildingDef.Create");
		gameObject.name = obj.name;
		gameObject.SetActive(true);
		return gameObject;
	}

	// Token: 0x06005E9F RID: 24223 RVA: 0x002A3E04 File Offset: 0x002A2004
	public List<Tag> DefaultElements()
	{
		List<Tag> list = new List<Tag>();
		string[] materialCategory = this.MaterialCategory;
		for (int i = 0; i < materialCategory.Length; i++)
		{
			List<Tag> validMaterials = MaterialSelector.GetValidMaterials(materialCategory[i], false);
			if (validMaterials.Count != 0)
			{
				list.Add(validMaterials[0]);
			}
		}
		return list;
	}

	// Token: 0x06005EA0 RID: 24224 RVA: 0x002A3E54 File Offset: 0x002A2054
	public GameObject Build(int cell, Orientation orientation, Storage resource_storage, IList<Tag> selected_elements, float temperature, string facadeID, bool playsound = true, float timeBuilt = -1f)
	{
		GameObject gameObject = this.Build(cell, orientation, resource_storage, selected_elements, temperature, playsound, timeBuilt);
		if (facadeID != null && facadeID != "DEFAULT_FACADE")
		{
			gameObject.GetComponent<BuildingFacade>().ApplyBuildingFacade(Db.GetBuildingFacades().Get(facadeID), false);
		}
		return gameObject;
	}

	// Token: 0x06005EA1 RID: 24225 RVA: 0x002A3EA0 File Offset: 0x002A20A0
	public GameObject Build(int cell, Orientation orientation, Storage resource_storage, IList<Tag> selected_elements, float temperature, bool playsound = true, float timeBuilt = -1f)
	{
		Vector3 pos = Grid.CellToPosCBC(cell, this.SceneLayer);
		GameObject gameObject = this.Create(pos, resource_storage, selected_elements, this.CraftRecipe, temperature, this.BuildingComplete);
		Rotatable component = gameObject.GetComponent<Rotatable>();
		if (component != null)
		{
			component.SetOrientation(orientation);
		}
		this.MarkArea(cell, orientation, this.ObjectLayer, gameObject);
		if (this.IsTilePiece)
		{
			this.MarkArea(cell, orientation, this.TileLayer, gameObject);
			this.RunOnArea(cell, orientation, delegate(int c)
			{
				TileVisualizer.RefreshCell(c, this.TileLayer, this.ReplacementLayer);
			});
		}
		if (this.PlayConstructionSounds)
		{
			string sound = GlobalAssets.GetSound("Finish_Building_" + this.AudioSize, false);
			if (playsound && sound != null)
			{
				Vector3 position = gameObject.transform.GetPosition();
				position.z = 0f;
				KFMOD.PlayOneShot(sound, position, 1f);
			}
		}
		Deconstructable component2 = gameObject.GetComponent<Deconstructable>();
		if (component2 != null)
		{
			component2.constructionElements = new Tag[selected_elements.Count];
			for (int i = 0; i < selected_elements.Count; i++)
			{
				component2.constructionElements[i] = selected_elements[i];
			}
		}
		BuildingComplete component3 = gameObject.GetComponent<BuildingComplete>();
		if (component3)
		{
			component3.SetCreationTime(timeBuilt);
		}
		Game.Instance.Trigger(-1661515756, gameObject);
		gameObject.Trigger(-1661515756, gameObject);
		return gameObject;
	}

	// Token: 0x06005EA2 RID: 24226 RVA: 0x000DDD2B File Offset: 0x000DBF2B
	public GameObject TryPlace(GameObject src_go, Vector3 pos, Orientation orientation, IList<Tag> selected_elements, int layer = 0)
	{
		return this.TryPlace(src_go, pos, orientation, selected_elements, null, 0);
	}

	// Token: 0x06005EA3 RID: 24227 RVA: 0x000DDD3A File Offset: 0x000DBF3A
	public GameObject TryPlace(GameObject src_go, Vector3 pos, Orientation orientation, IList<Tag> selected_elements, string facadeID, int layer = 0)
	{
		return this.TryPlace(src_go, pos, orientation, selected_elements, facadeID, true, layer);
	}

	// Token: 0x06005EA4 RID: 24228 RVA: 0x002A3FF8 File Offset: 0x002A21F8
	public GameObject TryPlace(GameObject src_go, Vector3 pos, Orientation orientation, IList<Tag> selected_elements, string facadeID, bool restrictToActiveWorld, int layer = 0)
	{
		GameObject gameObject = null;
		string text;
		if (this.IsValidPlaceLocation(src_go, Grid.PosToCell(pos), orientation, false, out text, restrictToActiveWorld))
		{
			gameObject = this.Instantiate(pos, orientation, selected_elements, layer);
			if (orientation != Orientation.Neutral)
			{
				Rotatable component = gameObject.GetComponent<Rotatable>();
				if (component != null)
				{
					component.SetOrientation(orientation);
				}
			}
		}
		if (gameObject != null && facadeID != null && facadeID != "DEFAULT_FACADE")
		{
			gameObject.GetComponent<BuildingFacade>().ApplyBuildingFacade(Db.GetBuildingFacades().Get(facadeID), false);
			gameObject.GetComponent<KBatchedAnimController>().Play("place", KAnim.PlayMode.Once, 1f, 0f);
		}
		return gameObject;
	}

	// Token: 0x06005EA5 RID: 24229 RVA: 0x002A4098 File Offset: 0x002A2298
	public GameObject TryReplaceTile(GameObject src_go, Vector3 pos, Orientation orientation, IList<Tag> selected_elements, int layer = 0)
	{
		GameObject gameObject = null;
		string text;
		if (this.IsValidPlaceLocation(src_go, pos, orientation, true, out text))
		{
			Constructable component = this.BuildingUnderConstruction.GetComponent<Constructable>();
			component.IsReplacementTile = true;
			gameObject = this.Instantiate(pos, orientation, selected_elements, layer);
			component.IsReplacementTile = false;
			if (orientation != Orientation.Neutral)
			{
				Rotatable component2 = gameObject.GetComponent<Rotatable>();
				if (component2 != null)
				{
					component2.SetOrientation(orientation);
				}
			}
		}
		return gameObject;
	}

	// Token: 0x06005EA6 RID: 24230 RVA: 0x002A40F8 File Offset: 0x002A22F8
	public GameObject TryReplaceTile(GameObject src_go, Vector3 pos, Orientation orientation, IList<Tag> selected_elements, string facadeID, int layer = 0)
	{
		GameObject gameObject = this.TryReplaceTile(src_go, pos, orientation, selected_elements, layer);
		if (gameObject != null)
		{
			if (facadeID != null && facadeID != "DEFAULT_FACADE")
			{
				gameObject.GetComponent<BuildingFacade>().ApplyBuildingFacade(Db.GetBuildingFacades().Get(facadeID), false);
			}
			if (orientation != Orientation.Neutral)
			{
				Rotatable component = gameObject.GetComponent<Rotatable>();
				if (component != null)
				{
					component.SetOrientation(orientation);
				}
			}
		}
		return gameObject;
	}

	// Token: 0x06005EA7 RID: 24231 RVA: 0x002A4164 File Offset: 0x002A2364
	public GameObject Instantiate(Vector3 pos, Orientation orientation, IList<Tag> selected_elements, int layer = 0)
	{
		float num = -0.15f;
		pos.z += num;
		GameObject gameObject = GameUtil.KInstantiate(this.BuildingUnderConstruction, pos, Grid.SceneLayer.Front, null, layer);
		Element element = ElementLoader.GetElement(selected_elements[0]);
		global::Debug.Assert(element != null, "Missing primary element for BuildingDef");
		gameObject.GetComponent<PrimaryElement>().ElementID = element.id;
		gameObject.GetComponent<Constructable>().SelectedElementsTags = selected_elements;
		gameObject.SetActive(true);
		return gameObject;
	}

	// Token: 0x06005EA8 RID: 24232 RVA: 0x002A41D8 File Offset: 0x002A23D8
	private bool IsAreaClear(GameObject source_go, int cell, Orientation orientation, ObjectLayer layer, ObjectLayer tile_layer, bool replace_tile, out string fail_reason)
	{
		return this.IsAreaClear(source_go, cell, orientation, layer, tile_layer, replace_tile, true, out fail_reason);
	}

	// Token: 0x06005EA9 RID: 24233 RVA: 0x002A41F8 File Offset: 0x002A23F8
	private bool IsAreaClear(GameObject source_go, int cell, Orientation orientation, ObjectLayer layer, ObjectLayer tile_layer, bool replace_tile, bool restrictToActiveWorld, out string fail_reason)
	{
		bool flag = true;
		fail_reason = null;
		int i = 0;
		while (i < this.PlacementOffsets.Length)
		{
			CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(this.PlacementOffsets[i], orientation);
			if (!Grid.IsCellOffsetValid(cell, rotatedCellOffset))
			{
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_INVALID_CELL;
				flag = false;
				break;
			}
			int num = Grid.OffsetCell(cell, rotatedCellOffset);
			if (restrictToActiveWorld && (int)Grid.WorldIdx[num] != ClusterManager.Instance.activeWorldId)
			{
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_INVALID_CELL;
				return false;
			}
			if (!Grid.IsValidBuildingCell(num))
			{
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_INVALID_CELL;
				flag = false;
				break;
			}
			if (Grid.Element[num].id == SimHashes.Unobtanium)
			{
				fail_reason = null;
				flag = false;
				break;
			}
			bool flag2 = this.BuildLocationRule == BuildLocationRule.LogicBridge || this.BuildLocationRule == BuildLocationRule.Conduit || this.BuildLocationRule == BuildLocationRule.WireBridge;
			GameObject x = null;
			if (replace_tile)
			{
				x = this.GetReplacementCandidate(num);
			}
			if (!flag2)
			{
				GameObject gameObject = Grid.Objects[num, (int)layer];
				bool flag3 = false;
				if (gameObject != null)
				{
					Building component = gameObject.GetComponent<Building>();
					if (component != null)
					{
						flag3 = (component.Def.BuildLocationRule == BuildLocationRule.LogicBridge || component.Def.BuildLocationRule == BuildLocationRule.Conduit || component.Def.BuildLocationRule == BuildLocationRule.WireBridge);
					}
				}
				if (!flag3)
				{
					if (gameObject != null && gameObject != source_go && (x == null || x != gameObject) && (gameObject.GetComponent<Wire>() == null || this.BuildingComplete.GetComponent<Wire>() == null))
					{
						fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_OCCUPIED;
						flag = false;
						break;
					}
					if (tile_layer != ObjectLayer.NumLayers && (x == null || x == source_go) && Grid.Objects[num, (int)tile_layer] != null && Grid.Objects[num, (int)tile_layer].GetComponent<BuildingPreview>() == null)
					{
						fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_OCCUPIED;
						flag = false;
						break;
					}
				}
			}
			if (layer == ObjectLayer.Building && this.AttachmentSlotTag != GameTags.Rocket && Grid.Objects[num, 39] != null)
			{
				if (this.BuildingComplete.GetComponent<Wire>() == null)
				{
					fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_OCCUPIED;
					flag = false;
					break;
				}
				break;
			}
			else
			{
				if (layer == ObjectLayer.Gantry)
				{
					bool flag4 = false;
					MakeBaseSolid.Def def = source_go.GetDef<MakeBaseSolid.Def>();
					for (int j = 0; j < def.solidOffsets.Length; j++)
					{
						CellOffset rotatedCellOffset2 = Rotatable.GetRotatedCellOffset(def.solidOffsets[j], orientation);
						flag4 |= (rotatedCellOffset2 == rotatedCellOffset);
					}
					if (flag4 && !this.IsValidTileLocation(source_go, num, replace_tile, ref fail_reason))
					{
						flag = false;
						break;
					}
					GameObject gameObject2 = Grid.Objects[num, 1];
					if (gameObject2 != null && gameObject2.GetComponent<BuildingPreview>() == null)
					{
						Building component2 = gameObject2.GetComponent<Building>();
						if (flag4 || component2 == null || component2.Def.AttachmentSlotTag != GameTags.Rocket)
						{
							fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_OCCUPIED;
							flag = false;
							break;
						}
					}
				}
				if (this.BuildLocationRule == BuildLocationRule.Tile)
				{
					if (!this.IsValidTileLocation(source_go, num, replace_tile, ref fail_reason))
					{
						flag = false;
						break;
					}
				}
				else if (this.BuildLocationRule == BuildLocationRule.OnFloorOverSpace && global::World.Instance.zoneRenderData.GetSubWorldZoneType(num) != SubWorld.ZoneType.Space)
				{
					fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_SPACE;
					flag = false;
					break;
				}
				i++;
			}
		}
		if (!flag)
		{
			return false;
		}
		if (layer == ObjectLayer.LiquidConduit)
		{
			GameObject gameObject3 = Grid.Objects[cell, 19];
			if (gameObject3 != null)
			{
				Building component3 = gameObject3.GetComponent<Building>();
				if (component3 != null && component3.Def.BuildLocationRule == BuildLocationRule.NoLiquidConduitAtOrigin && component3.GetCell() == cell)
				{
					fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_LIQUID_CONDUIT_FORBIDDEN;
					return false;
				}
			}
		}
		BuildLocationRule buildLocationRule = this.BuildLocationRule;
		switch (buildLocationRule)
		{
		case BuildLocationRule.NotInTiles:
		{
			GameObject x2 = Grid.Objects[cell, 9];
			if (!replace_tile && x2 != null && x2 != source_go)
			{
				flag = false;
			}
			else if (Grid.HasDoor[cell])
			{
				flag = false;
			}
			else
			{
				GameObject gameObject4 = Grid.Objects[cell, (int)this.ObjectLayer];
				if (gameObject4 != null)
				{
					if (this.ReplacementLayer == ObjectLayer.NumLayers)
					{
						if (gameObject4 != source_go)
						{
							flag = false;
						}
					}
					else
					{
						Building component4 = gameObject4.GetComponent<Building>();
						if (component4 != null && component4.Def.ReplacementLayer != this.ReplacementLayer)
						{
							flag = false;
						}
					}
				}
			}
			if (!flag)
			{
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_NOT_IN_TILES;
			}
			break;
		}
		case BuildLocationRule.Conduit:
		case BuildLocationRule.LogicBridge:
			break;
		case BuildLocationRule.WireBridge:
			return this.IsValidWireBridgeLocation(source_go, cell, orientation, out fail_reason);
		case BuildLocationRule.HighWattBridgeTile:
			flag = (this.IsValidTileLocation(source_go, cell, replace_tile, ref fail_reason) && this.IsValidHighWattBridgeLocation(source_go, cell, orientation, out fail_reason));
			break;
		case BuildLocationRule.BuildingAttachPoint:
		{
			flag = false;
			int num2 = 0;
			while (num2 < Components.BuildingAttachPoints.Count && !flag)
			{
				for (int k = 0; k < Components.BuildingAttachPoints[num2].points.Length; k++)
				{
					if (Components.BuildingAttachPoints[num2].AcceptsAttachment(this.AttachmentSlotTag, Grid.OffsetCell(cell, this.attachablePosition)))
					{
						flag = true;
						break;
					}
				}
				num2++;
			}
			if (!flag)
			{
				fail_reason = string.Format(UI.TOOLTIPS.HELP_BUILDLOCATION_ATTACHPOINT, this.AttachmentSlotTag);
			}
			break;
		}
		default:
			if (buildLocationRule == BuildLocationRule.NoLiquidConduitAtOrigin)
			{
				flag = (Grid.Objects[cell, 16] == null && (Grid.Objects[cell, 19] == null || Grid.Objects[cell, 19] == source_go));
			}
			break;
		}
		flag = (flag && this.ArePowerPortsInValidPositions(source_go, cell, orientation, out fail_reason));
		flag = (flag && this.AreConduitPortsInValidPositions(source_go, cell, orientation, out fail_reason));
		return flag && this.AreLogicPortsInValidPositions(source_go, cell, out fail_reason);
	}

	// Token: 0x06005EAA RID: 24234 RVA: 0x002A4804 File Offset: 0x002A2A04
	private bool IsValidTileLocation(GameObject source_go, int cell, bool replacement_tile, ref string fail_reason)
	{
		GameObject gameObject = Grid.Objects[cell, 27];
		if (gameObject != null && gameObject != source_go && gameObject.GetComponent<Building>().Def.BuildLocationRule == BuildLocationRule.NotInTiles)
		{
			fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_WIRE_OBSTRUCTION;
			return false;
		}
		gameObject = Grid.Objects[cell, 29];
		if (gameObject != null && gameObject != source_go && gameObject.GetComponent<Building>().Def.BuildLocationRule == BuildLocationRule.HighWattBridgeTile)
		{
			fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_WIRE_OBSTRUCTION;
			return false;
		}
		gameObject = Grid.Objects[cell, 2];
		if (gameObject != null && gameObject != source_go)
		{
			Building component = gameObject.GetComponent<Building>();
			if (!replacement_tile && component != null && component.Def.BuildLocationRule == BuildLocationRule.NotInTiles)
			{
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_BACK_WALL;
				return false;
			}
		}
		return true;
	}

	// Token: 0x06005EAB RID: 24235 RVA: 0x002A48E8 File Offset: 0x002A2AE8
	public void RunOnArea(int cell, Orientation orientation, Action<int> callback)
	{
		for (int i = 0; i < this.PlacementOffsets.Length; i++)
		{
			CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(this.PlacementOffsets[i], orientation);
			int obj = Grid.OffsetCell(cell, rotatedCellOffset);
			callback(obj);
		}
	}

	// Token: 0x06005EAC RID: 24236 RVA: 0x002A492C File Offset: 0x002A2B2C
	public void MarkArea(int cell, Orientation orientation, ObjectLayer layer, GameObject go)
	{
		if (this.BuildLocationRule != BuildLocationRule.Conduit && this.BuildLocationRule != BuildLocationRule.WireBridge && this.BuildLocationRule != BuildLocationRule.LogicBridge)
		{
			for (int i = 0; i < this.PlacementOffsets.Length; i++)
			{
				CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(this.PlacementOffsets[i], orientation);
				int cell2 = Grid.OffsetCell(cell, rotatedCellOffset);
				Grid.Objects[cell2, (int)layer] = go;
			}
		}
		if (this.InputConduitType != ConduitType.None)
		{
			CellOffset rotatedCellOffset2 = Rotatable.GetRotatedCellOffset(this.UtilityInputOffset, orientation);
			int cell3 = Grid.OffsetCell(cell, rotatedCellOffset2);
			ObjectLayer objectLayerForConduitType = Grid.GetObjectLayerForConduitType(this.InputConduitType);
			this.MarkOverlappingPorts(Grid.Objects[cell3, (int)objectLayerForConduitType], go);
			Grid.Objects[cell3, (int)objectLayerForConduitType] = go;
		}
		if (this.OutputConduitType != ConduitType.None)
		{
			CellOffset rotatedCellOffset3 = Rotatable.GetRotatedCellOffset(this.UtilityOutputOffset, orientation);
			int cell4 = Grid.OffsetCell(cell, rotatedCellOffset3);
			ObjectLayer objectLayerForConduitType2 = Grid.GetObjectLayerForConduitType(this.OutputConduitType);
			this.MarkOverlappingPorts(Grid.Objects[cell4, (int)objectLayerForConduitType2], go);
			Grid.Objects[cell4, (int)objectLayerForConduitType2] = go;
		}
		if (this.RequiresPowerInput)
		{
			CellOffset rotatedCellOffset4 = Rotatable.GetRotatedCellOffset(this.PowerInputOffset, orientation);
			int cell5 = Grid.OffsetCell(cell, rotatedCellOffset4);
			this.MarkOverlappingPorts(Grid.Objects[cell5, 29], go);
			Grid.Objects[cell5, 29] = go;
		}
		if (this.RequiresPowerOutput)
		{
			CellOffset rotatedCellOffset5 = Rotatable.GetRotatedCellOffset(this.PowerOutputOffset, orientation);
			int cell6 = Grid.OffsetCell(cell, rotatedCellOffset5);
			this.MarkOverlappingPorts(Grid.Objects[cell6, 29], go);
			Grid.Objects[cell6, 29] = go;
		}
		if (this.BuildLocationRule == BuildLocationRule.WireBridge || this.BuildLocationRule == BuildLocationRule.HighWattBridgeTile)
		{
			int cell7;
			int cell8;
			go.GetComponent<UtilityNetworkLink>().GetCells(cell, orientation, out cell7, out cell8);
			this.MarkOverlappingPorts(Grid.Objects[cell7, 29], go);
			this.MarkOverlappingPorts(Grid.Objects[cell8, 29], go);
			Grid.Objects[cell7, 29] = go;
			Grid.Objects[cell8, 29] = go;
		}
		if (this.BuildLocationRule == BuildLocationRule.LogicBridge)
		{
			LogicPorts component = go.GetComponent<LogicPorts>();
			if (component != null && component.inputPortInfo != null)
			{
				LogicPorts.Port[] inputPortInfo = component.inputPortInfo;
				for (int j = 0; j < inputPortInfo.Length; j++)
				{
					CellOffset rotatedCellOffset6 = Rotatable.GetRotatedCellOffset(inputPortInfo[j].cellOffset, orientation);
					int cell9 = Grid.OffsetCell(cell, rotatedCellOffset6);
					this.MarkOverlappingLogicPorts(Grid.Objects[cell9, (int)layer], go, cell9);
					Grid.Objects[cell9, (int)layer] = go;
				}
			}
		}
		ISecondaryInput[] components = this.BuildingComplete.GetComponents<ISecondaryInput>();
		if (components != null)
		{
			foreach (ISecondaryInput secondaryInput in components)
			{
				for (int k = 0; k < 4; k++)
				{
					ConduitType conduitType = (ConduitType)k;
					if (conduitType != ConduitType.None && secondaryInput.HasSecondaryConduitType(conduitType))
					{
						ObjectLayer objectLayerForConduitType3 = Grid.GetObjectLayerForConduitType(conduitType);
						CellOffset rotatedCellOffset7 = Rotatable.GetRotatedCellOffset(secondaryInput.GetSecondaryConduitOffset(conduitType), orientation);
						int cell10 = Grid.OffsetCell(cell, rotatedCellOffset7);
						this.MarkOverlappingPorts(Grid.Objects[cell10, (int)objectLayerForConduitType3], go);
						Grid.Objects[cell10, (int)objectLayerForConduitType3] = go;
					}
				}
			}
		}
		ISecondaryOutput[] components2 = this.BuildingComplete.GetComponents<ISecondaryOutput>();
		if (components2 != null)
		{
			foreach (ISecondaryOutput secondaryOutput in components2)
			{
				for (int l = 0; l < 4; l++)
				{
					ConduitType conduitType2 = (ConduitType)l;
					if (conduitType2 != ConduitType.None && secondaryOutput.HasSecondaryConduitType(conduitType2))
					{
						ObjectLayer objectLayerForConduitType4 = Grid.GetObjectLayerForConduitType(conduitType2);
						CellOffset rotatedCellOffset8 = Rotatable.GetRotatedCellOffset(secondaryOutput.GetSecondaryConduitOffset(conduitType2), orientation);
						int cell11 = Grid.OffsetCell(cell, rotatedCellOffset8);
						this.MarkOverlappingPorts(Grid.Objects[cell11, (int)objectLayerForConduitType4], go);
						Grid.Objects[cell11, (int)objectLayerForConduitType4] = go;
					}
				}
			}
		}
	}

	// Token: 0x06005EAD RID: 24237 RVA: 0x000DDD4C File Offset: 0x000DBF4C
	public void MarkOverlappingPorts(GameObject existing, GameObject replaced)
	{
		if (existing == null)
		{
			if (replaced != null)
			{
				replaced.RemoveTag(GameTags.HasInvalidPorts);
				return;
			}
		}
		else if (existing != replaced)
		{
			existing.AddTag(GameTags.HasInvalidPorts);
		}
	}

	// Token: 0x06005EAE RID: 24238 RVA: 0x002A4D00 File Offset: 0x002A2F00
	public void MarkOverlappingLogicPorts(GameObject existing, GameObject replaced, int cell)
	{
		if (existing == null)
		{
			if (replaced != null)
			{
				replaced.RemoveTag(GameTags.HasInvalidPorts);
				return;
			}
		}
		else if (existing != replaced)
		{
			LogicGate component = existing.GetComponent<LogicGate>();
			LogicPorts component2 = existing.GetComponent<LogicPorts>();
			LogicPorts.Port port;
			bool flag;
			LogicGateBase.PortId portId;
			if ((component2 != null && component2.TryGetPortAtCell(cell, out port, out flag)) || (component != null && component.TryGetPortAtCell(cell, out portId)))
			{
				existing.AddTag(GameTags.HasInvalidPorts);
			}
		}
	}

	// Token: 0x06005EAF RID: 24239 RVA: 0x002A4D78 File Offset: 0x002A2F78
	public void UnmarkArea(int cell, Orientation orientation, ObjectLayer layer, GameObject go)
	{
		if (cell == Grid.InvalidCell)
		{
			return;
		}
		for (int i = 0; i < this.PlacementOffsets.Length; i++)
		{
			CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(this.PlacementOffsets[i], orientation);
			int cell2 = Grid.OffsetCell(cell, rotatedCellOffset);
			if (Grid.Objects[cell2, (int)layer] == go)
			{
				Grid.Objects[cell2, (int)layer] = null;
			}
		}
		if (this.InputConduitType != ConduitType.None)
		{
			CellOffset rotatedCellOffset2 = Rotatable.GetRotatedCellOffset(this.UtilityInputOffset, orientation);
			int cell3 = Grid.OffsetCell(cell, rotatedCellOffset2);
			ObjectLayer objectLayerForConduitType = Grid.GetObjectLayerForConduitType(this.InputConduitType);
			if (Grid.Objects[cell3, (int)objectLayerForConduitType] == go)
			{
				Grid.Objects[cell3, (int)objectLayerForConduitType] = null;
			}
		}
		if (this.OutputConduitType != ConduitType.None)
		{
			CellOffset rotatedCellOffset3 = Rotatable.GetRotatedCellOffset(this.UtilityOutputOffset, orientation);
			int cell4 = Grid.OffsetCell(cell, rotatedCellOffset3);
			ObjectLayer objectLayerForConduitType2 = Grid.GetObjectLayerForConduitType(this.OutputConduitType);
			if (Grid.Objects[cell4, (int)objectLayerForConduitType2] == go)
			{
				Grid.Objects[cell4, (int)objectLayerForConduitType2] = null;
			}
		}
		if (this.RequiresPowerInput)
		{
			CellOffset rotatedCellOffset4 = Rotatable.GetRotatedCellOffset(this.PowerInputOffset, orientation);
			int cell5 = Grid.OffsetCell(cell, rotatedCellOffset4);
			if (Grid.Objects[cell5, 29] == go)
			{
				Grid.Objects[cell5, 29] = null;
			}
		}
		if (this.RequiresPowerOutput)
		{
			CellOffset rotatedCellOffset5 = Rotatable.GetRotatedCellOffset(this.PowerOutputOffset, orientation);
			int cell6 = Grid.OffsetCell(cell, rotatedCellOffset5);
			if (Grid.Objects[cell6, 29] == go)
			{
				Grid.Objects[cell6, 29] = null;
			}
		}
		if (this.BuildLocationRule == BuildLocationRule.HighWattBridgeTile)
		{
			int cell7;
			int cell8;
			go.GetComponent<UtilityNetworkLink>().GetCells(cell, orientation, out cell7, out cell8);
			if (Grid.Objects[cell7, 29] == go)
			{
				Grid.Objects[cell7, 29] = null;
			}
			if (Grid.Objects[cell8, 29] == go)
			{
				Grid.Objects[cell8, 29] = null;
			}
		}
		ISecondaryInput[] components = this.BuildingComplete.GetComponents<ISecondaryInput>();
		if (components != null)
		{
			foreach (ISecondaryInput secondaryInput in components)
			{
				for (int k = 0; k < 4; k++)
				{
					ConduitType conduitType = (ConduitType)k;
					if (conduitType != ConduitType.None && secondaryInput.HasSecondaryConduitType(conduitType))
					{
						ObjectLayer objectLayerForConduitType3 = Grid.GetObjectLayerForConduitType(conduitType);
						CellOffset rotatedCellOffset6 = Rotatable.GetRotatedCellOffset(secondaryInput.GetSecondaryConduitOffset(conduitType), orientation);
						int cell9 = Grid.OffsetCell(cell, rotatedCellOffset6);
						if (Grid.Objects[cell9, (int)objectLayerForConduitType3] == go)
						{
							Grid.Objects[cell9, (int)objectLayerForConduitType3] = null;
						}
					}
				}
			}
		}
		ISecondaryOutput[] components2 = this.BuildingComplete.GetComponents<ISecondaryOutput>();
		if (components2 != null)
		{
			foreach (ISecondaryOutput secondaryOutput in components2)
			{
				for (int l = 0; l < 4; l++)
				{
					ConduitType conduitType2 = (ConduitType)l;
					if (conduitType2 != ConduitType.None && secondaryOutput.HasSecondaryConduitType(conduitType2))
					{
						ObjectLayer objectLayerForConduitType4 = Grid.GetObjectLayerForConduitType(conduitType2);
						CellOffset rotatedCellOffset7 = Rotatable.GetRotatedCellOffset(secondaryOutput.GetSecondaryConduitOffset(conduitType2), orientation);
						int cell10 = Grid.OffsetCell(cell, rotatedCellOffset7);
						if (Grid.Objects[cell10, (int)objectLayerForConduitType4] == go)
						{
							Grid.Objects[cell10, (int)objectLayerForConduitType4] = null;
						}
					}
				}
			}
		}
	}

	// Token: 0x06005EB0 RID: 24240 RVA: 0x000DDD80 File Offset: 0x000DBF80
	public int GetBuildingCell(int cell)
	{
		return cell + (this.WidthInCells - 1) / 2;
	}

	// Token: 0x06005EB1 RID: 24241 RVA: 0x000DDD8E File Offset: 0x000DBF8E
	public Vector3 GetVisualizerOffset()
	{
		return Vector3.right * (0.5f * (float)((this.WidthInCells + 1) % 2));
	}

	// Token: 0x06005EB2 RID: 24242 RVA: 0x002A50BC File Offset: 0x002A32BC
	public bool IsValidPlaceLocation(GameObject source_go, Vector3 pos, Orientation orientation, out string fail_reason)
	{
		int cell = Grid.PosToCell(pos);
		return this.IsValidPlaceLocation(source_go, cell, orientation, false, out fail_reason);
	}

	// Token: 0x06005EB3 RID: 24243 RVA: 0x002A50DC File Offset: 0x002A32DC
	public bool IsValidPlaceLocation(GameObject source_go, Vector3 pos, Orientation orientation, bool replace_tile, out string fail_reason)
	{
		int cell = Grid.PosToCell(pos);
		return this.IsValidPlaceLocation(source_go, cell, orientation, replace_tile, out fail_reason);
	}

	// Token: 0x06005EB4 RID: 24244 RVA: 0x000DDDAB File Offset: 0x000DBFAB
	public bool IsValidPlaceLocation(GameObject source_go, int cell, Orientation orientation, out string fail_reason)
	{
		return this.IsValidPlaceLocation(source_go, cell, orientation, false, out fail_reason);
	}

	// Token: 0x06005EB5 RID: 24245 RVA: 0x000DDDB9 File Offset: 0x000DBFB9
	public bool IsValidPlaceLocation(GameObject source_go, int cell, Orientation orientation, bool replace_tile, out string fail_reason)
	{
		return this.IsValidPlaceLocation(source_go, cell, orientation, replace_tile, out fail_reason, false);
	}

	// Token: 0x06005EB6 RID: 24246 RVA: 0x002A5100 File Offset: 0x002A3300
	public bool IsValidPlaceLocation(GameObject source_go, int cell, Orientation orientation, bool replace_tile, out string fail_reason, bool restrictToActiveWorld)
	{
		if (!Grid.IsValidBuildingCell(cell))
		{
			fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_INVALID_CELL;
			return false;
		}
		if (restrictToActiveWorld && (int)Grid.WorldIdx[cell] != ClusterManager.Instance.activeWorldId)
		{
			fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_INVALID_CELL;
			return false;
		}
		if (this.BuildLocationRule == BuildLocationRule.OnRocketEnvelope)
		{
			if (!BuildingDef.CheckFoundation(cell, orientation, this.BuildLocationRule, this.WidthInCells, this.HeightInCells, GameTags.RocketEnvelopeTile))
			{
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_ONROCKETENVELOPE;
				return false;
			}
		}
		else if (this.BuildLocationRule == BuildLocationRule.OnWall)
		{
			if (!BuildingDef.CheckFoundation(cell, orientation, this.BuildLocationRule, this.WidthInCells, this.HeightInCells, default(Tag)))
			{
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_WALL;
				return false;
			}
		}
		else if (this.BuildLocationRule == BuildLocationRule.InCorner)
		{
			if (!BuildingDef.CheckFoundation(cell, orientation, this.BuildLocationRule, this.WidthInCells, this.HeightInCells, default(Tag)))
			{
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_CORNER;
				return false;
			}
		}
		else if (this.BuildLocationRule == BuildLocationRule.WallFloor)
		{
			if (!BuildingDef.CheckFoundation(cell, orientation, this.BuildLocationRule, this.WidthInCells, this.HeightInCells, default(Tag)))
			{
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_CORNER_FLOOR;
				return false;
			}
		}
		else if (this.BuildLocationRule == BuildLocationRule.BelowRocketCeiling)
		{
			WorldContainer world = ClusterManager.Instance.GetWorld((int)Grid.WorldIdx[cell]);
			if ((float)(Grid.CellToXY(cell).y + 35 + source_go.GetComponent<Building>().Def.HeightInCells) >= world.maximumBounds.y - (float)Grid.TopBorderHeight)
			{
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_BELOWROCKETCEILING;
				return false;
			}
		}
		return this.IsAreaClear(source_go, cell, orientation, this.ObjectLayer, this.TileLayer, replace_tile, restrictToActiveWorld, out fail_reason);
	}

	// Token: 0x06005EB7 RID: 24247 RVA: 0x002A52BC File Offset: 0x002A34BC
	public bool IsValidReplaceLocation(Vector3 pos, Orientation orientation, ObjectLayer replace_layer, ObjectLayer obj_layer)
	{
		if (replace_layer == ObjectLayer.NumLayers)
		{
			return false;
		}
		bool result = true;
		int cell = Grid.PosToCell(pos);
		for (int i = 0; i < this.PlacementOffsets.Length; i++)
		{
			CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(this.PlacementOffsets[i], orientation);
			int cell2 = Grid.OffsetCell(cell, rotatedCellOffset);
			if (!Grid.IsValidBuildingCell(cell2))
			{
				return false;
			}
			if (Grid.Objects[cell2, (int)obj_layer] == null || Grid.Objects[cell2, (int)replace_layer] != null)
			{
				result = false;
				break;
			}
		}
		return result;
	}

	// Token: 0x06005EB8 RID: 24248 RVA: 0x002A5344 File Offset: 0x002A3544
	public bool IsValidBuildLocation(GameObject source_go, Vector3 pos, Orientation orientation, bool replace_tile = false)
	{
		string text = "";
		return this.IsValidBuildLocation(source_go, pos, orientation, out text, replace_tile);
	}

	// Token: 0x06005EB9 RID: 24249 RVA: 0x002A5364 File Offset: 0x002A3564
	public bool IsValidBuildLocation(GameObject source_go, Vector3 pos, Orientation orientation, out string reason, bool replace_tile = false)
	{
		int cell = Grid.PosToCell(pos);
		return this.IsValidBuildLocation(source_go, cell, orientation, replace_tile, out reason);
	}

	// Token: 0x06005EBA RID: 24250 RVA: 0x002A5388 File Offset: 0x002A3588
	public bool IsValidBuildLocation(GameObject source_go, int cell, Orientation orientation, bool replace_tile, out string fail_reason)
	{
		if (!Grid.IsValidBuildingCell(cell))
		{
			fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_INVALID_CELL;
			return false;
		}
		if (!this.IsAreaValid(cell, orientation, out fail_reason))
		{
			return false;
		}
		bool flag = true;
		fail_reason = null;
		switch (this.BuildLocationRule)
		{
		case BuildLocationRule.Anywhere:
		case BuildLocationRule.Conduit:
		case BuildLocationRule.OnFloorOrBuildingAttachPoint:
			flag = true;
			break;
		case BuildLocationRule.OnFloor:
		case BuildLocationRule.OnCeiling:
		case BuildLocationRule.OnFoundationRotatable:
			if (!BuildingDef.CheckFoundation(cell, orientation, this.BuildLocationRule, this.WidthInCells, this.HeightInCells, default(Tag)))
			{
				flag = false;
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_FLOOR;
			}
			break;
		case BuildLocationRule.OnFloorOverSpace:
			if (!BuildingDef.CheckFoundation(cell, orientation, this.BuildLocationRule, this.WidthInCells, this.HeightInCells, default(Tag)))
			{
				flag = false;
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_FLOOR;
			}
			else if (!BuildingDef.AreAllCellsValid(cell, orientation, this.WidthInCells, this.HeightInCells, (int check_cell) => global::World.Instance.zoneRenderData.GetSubWorldZoneType(check_cell) == SubWorld.ZoneType.Space))
			{
				flag = false;
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_SPACE;
			}
			break;
		case BuildLocationRule.OnWall:
			if (!BuildingDef.CheckFoundation(cell, orientation, this.BuildLocationRule, this.WidthInCells, this.HeightInCells, default(Tag)))
			{
				flag = false;
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_WALL;
			}
			break;
		case BuildLocationRule.InCorner:
			if (!BuildingDef.CheckFoundation(cell, orientation, this.BuildLocationRule, this.WidthInCells, this.HeightInCells, default(Tag)))
			{
				flag = false;
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_CORNER;
			}
			break;
		case BuildLocationRule.Tile:
		{
			flag = true;
			GameObject gameObject = Grid.Objects[cell, 27];
			if (gameObject != null)
			{
				Building component = gameObject.GetComponent<Building>();
				if (component != null && component.Def.BuildLocationRule == BuildLocationRule.NotInTiles)
				{
					flag = false;
				}
			}
			gameObject = Grid.Objects[cell, 2];
			if (gameObject != null)
			{
				Building component2 = gameObject.GetComponent<Building>();
				if (component2 != null && component2.Def.BuildLocationRule == BuildLocationRule.NotInTiles)
				{
					flag = replace_tile;
				}
			}
			break;
		}
		case BuildLocationRule.NotInTiles:
		{
			GameObject x = Grid.Objects[cell, 9];
			flag = (replace_tile || x == null || x == source_go);
			flag = (flag && !Grid.HasDoor[cell]);
			if (flag)
			{
				GameObject gameObject2 = Grid.Objects[cell, (int)this.ObjectLayer];
				if (gameObject2 != null)
				{
					if (this.ReplacementLayer == ObjectLayer.NumLayers)
					{
						flag = (flag && (gameObject2 == null || gameObject2 == source_go));
					}
					else
					{
						Building component3 = gameObject2.GetComponent<Building>();
						flag = (component3 == null || component3.Def.ReplacementLayer == this.ReplacementLayer);
					}
				}
			}
			fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_NOT_IN_TILES;
			break;
		}
		case BuildLocationRule.BuildingAttachPoint:
		{
			flag = false;
			int num = 0;
			while (num < Components.BuildingAttachPoints.Count && !flag)
			{
				for (int i = 0; i < Components.BuildingAttachPoints[num].points.Length; i++)
				{
					if (Components.BuildingAttachPoints[num].AcceptsAttachment(this.AttachmentSlotTag, Grid.OffsetCell(cell, this.attachablePosition)))
					{
						flag = true;
						break;
					}
				}
				num++;
			}
			fail_reason = string.Format(UI.TOOLTIPS.HELP_BUILDLOCATION_ATTACHPOINT, this.AttachmentSlotTag);
			break;
		}
		case BuildLocationRule.OnRocketEnvelope:
			if (!BuildingDef.CheckFoundation(cell, orientation, this.BuildLocationRule, this.WidthInCells, this.HeightInCells, GameTags.RocketEnvelopeTile))
			{
				flag = false;
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_ONROCKETENVELOPE;
			}
			break;
		case BuildLocationRule.WallFloor:
			if (!BuildingDef.CheckFoundation(cell, orientation, this.BuildLocationRule, this.WidthInCells, this.HeightInCells, default(Tag)))
			{
				flag = false;
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_CORNER_FLOOR;
			}
			break;
		}
		flag = (flag && this.ArePowerPortsInValidPositions(source_go, cell, orientation, out fail_reason));
		return flag && this.AreConduitPortsInValidPositions(source_go, cell, orientation, out fail_reason);
	}

	// Token: 0x06005EBB RID: 24251 RVA: 0x002A57BC File Offset: 0x002A39BC
	private bool IsAreaValid(int cell, Orientation orientation, out string fail_reason)
	{
		bool result = true;
		fail_reason = null;
		for (int i = 0; i < this.PlacementOffsets.Length; i++)
		{
			CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(this.PlacementOffsets[i], orientation);
			if (!Grid.IsCellOffsetValid(cell, rotatedCellOffset))
			{
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_INVALID_CELL;
				result = false;
				break;
			}
			int num = Grid.OffsetCell(cell, rotatedCellOffset);
			if (!Grid.IsValidBuildingCell(num))
			{
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_INVALID_CELL;
				result = false;
				break;
			}
			if (Grid.Element[num].id == SimHashes.Unobtanium)
			{
				fail_reason = null;
				result = false;
				break;
			}
		}
		return result;
	}

	// Token: 0x06005EBC RID: 24252 RVA: 0x002A5848 File Offset: 0x002A3A48
	private bool ArePowerPortsInValidPositions(GameObject source_go, int cell, Orientation orientation, out string fail_reason)
	{
		fail_reason = null;
		if (source_go == null)
		{
			return true;
		}
		if (this.RequiresPowerInput)
		{
			CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(this.PowerInputOffset, orientation);
			int cell2 = Grid.OffsetCell(cell, rotatedCellOffset);
			GameObject x = Grid.Objects[cell2, 29];
			if (x != null && x != source_go)
			{
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_WIRECONNECTORS_OVERLAP;
				return false;
			}
		}
		if (this.RequiresPowerOutput)
		{
			CellOffset rotatedCellOffset2 = Rotatable.GetRotatedCellOffset(this.PowerOutputOffset, orientation);
			int cell3 = Grid.OffsetCell(cell, rotatedCellOffset2);
			GameObject x2 = Grid.Objects[cell3, 29];
			if (x2 != null && x2 != source_go)
			{
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_WIRECONNECTORS_OVERLAP;
				return false;
			}
		}
		return true;
	}

	// Token: 0x06005EBD RID: 24253 RVA: 0x002A5904 File Offset: 0x002A3B04
	private bool AreConduitPortsInValidPositions(GameObject source_go, int cell, Orientation orientation, out string fail_reason)
	{
		fail_reason = null;
		if (source_go == null)
		{
			return true;
		}
		bool flag = true;
		if (this.InputConduitType != ConduitType.None)
		{
			CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(this.UtilityInputOffset, orientation);
			int utility_cell = Grid.OffsetCell(cell, rotatedCellOffset);
			flag = this.IsValidConduitConnection(source_go, this.InputConduitType, utility_cell, ref fail_reason);
		}
		if (flag && this.OutputConduitType != ConduitType.None)
		{
			CellOffset rotatedCellOffset2 = Rotatable.GetRotatedCellOffset(this.UtilityOutputOffset, orientation);
			int utility_cell2 = Grid.OffsetCell(cell, rotatedCellOffset2);
			flag = this.IsValidConduitConnection(source_go, this.OutputConduitType, utility_cell2, ref fail_reason);
		}
		Building component = source_go.GetComponent<Building>();
		if (flag && component)
		{
			ISecondaryInput[] components = component.Def.BuildingComplete.GetComponents<ISecondaryInput>();
			if (components != null)
			{
				foreach (ISecondaryInput secondaryInput in components)
				{
					for (int j = 0; j < 4; j++)
					{
						ConduitType conduitType = (ConduitType)j;
						if (conduitType != ConduitType.None && secondaryInput.HasSecondaryConduitType(conduitType))
						{
							CellOffset rotatedCellOffset3 = Rotatable.GetRotatedCellOffset(secondaryInput.GetSecondaryConduitOffset(conduitType), orientation);
							int utility_cell3 = Grid.OffsetCell(cell, rotatedCellOffset3);
							flag = this.IsValidConduitConnection(source_go, conduitType, utility_cell3, ref fail_reason);
						}
					}
				}
			}
		}
		if (flag)
		{
			ISecondaryOutput[] components2 = component.Def.BuildingComplete.GetComponents<ISecondaryOutput>();
			if (components2 != null)
			{
				foreach (ISecondaryOutput secondaryOutput in components2)
				{
					for (int k = 0; k < 4; k++)
					{
						ConduitType conduitType2 = (ConduitType)k;
						if (conduitType2 != ConduitType.None && secondaryOutput.HasSecondaryConduitType(conduitType2))
						{
							CellOffset rotatedCellOffset4 = Rotatable.GetRotatedCellOffset(secondaryOutput.GetSecondaryConduitOffset(conduitType2), orientation);
							int utility_cell4 = Grid.OffsetCell(cell, rotatedCellOffset4);
							flag = this.IsValidConduitConnection(source_go, conduitType2, utility_cell4, ref fail_reason);
						}
					}
				}
			}
		}
		return flag;
	}

	// Token: 0x06005EBE RID: 24254 RVA: 0x002A5AA4 File Offset: 0x002A3CA4
	private bool IsValidWireBridgeLocation(GameObject source_go, int cell, Orientation orientation, out string fail_reason)
	{
		if (source_go == null)
		{
			fail_reason = null;
			return true;
		}
		UtilityNetworkLink component = source_go.GetComponent<UtilityNetworkLink>();
		if (component != null)
		{
			int cell2;
			int cell3;
			component.GetCells(out cell2, out cell3);
			if (Grid.Objects[cell2, 29] != null || Grid.Objects[cell3, 29] != null)
			{
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_WIRECONNECTORS_OVERLAP;
				return false;
			}
		}
		fail_reason = null;
		return true;
	}

	// Token: 0x06005EBF RID: 24255 RVA: 0x002A5B18 File Offset: 0x002A3D18
	private bool IsValidHighWattBridgeLocation(GameObject source_go, int cell, Orientation orientation, out string fail_reason)
	{
		if (source_go == null)
		{
			fail_reason = null;
			return true;
		}
		UtilityNetworkLink component = source_go.GetComponent<UtilityNetworkLink>();
		if (component != null)
		{
			if (!component.AreCellsValid(cell, orientation))
			{
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_INVALID_CELL;
				return false;
			}
			int num;
			int num2;
			component.GetCells(out num, out num2);
			if (Grid.Objects[num, 29] != null || Grid.Objects[num2, 29] != null)
			{
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_WIRECONNECTORS_OVERLAP;
				return false;
			}
			if (Grid.Objects[num, 9] != null || Grid.Objects[num2, 9] != null)
			{
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_HIGHWATT_NOT_IN_TILE;
				return false;
			}
			if (Grid.HasDoor[num] || Grid.HasDoor[num2])
			{
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_HIGHWATT_NOT_IN_TILE;
				return false;
			}
			GameObject gameObject = Grid.Objects[num, 1];
			GameObject gameObject2 = Grid.Objects[num2, 1];
			if (gameObject != null || gameObject2 != null)
			{
				BuildingUnderConstruction buildingUnderConstruction = gameObject ? gameObject.GetComponent<BuildingUnderConstruction>() : null;
				BuildingUnderConstruction buildingUnderConstruction2 = gameObject2 ? gameObject2.GetComponent<BuildingUnderConstruction>() : null;
				if ((buildingUnderConstruction && buildingUnderConstruction.Def.BuildingComplete.GetComponent<Door>()) || (buildingUnderConstruction2 && buildingUnderConstruction2.Def.BuildingComplete.GetComponent<Door>()))
				{
					fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_HIGHWATT_NOT_IN_TILE;
					return false;
				}
			}
		}
		fail_reason = null;
		return true;
	}

	// Token: 0x06005EC0 RID: 24256 RVA: 0x002A5CB4 File Offset: 0x002A3EB4
	private bool AreLogicPortsInValidPositions(GameObject source_go, int cell, out string fail_reason)
	{
		fail_reason = null;
		if (source_go == null)
		{
			return true;
		}
		ReadOnlyCollection<ILogicUIElement> visElements = Game.Instance.logicCircuitManager.GetVisElements();
		LogicPorts component = source_go.GetComponent<LogicPorts>();
		if (component != null)
		{
			component.HackRefreshVisualizers();
			if (this.DoLogicPortsConflict(component.inputPorts, visElements) || this.DoLogicPortsConflict(component.outputPorts, visElements))
			{
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_LOGIC_PORTS_OBSTRUCTED;
				return false;
			}
		}
		else
		{
			LogicGateBase component2 = source_go.GetComponent<LogicGateBase>();
			if (component2 != null && (this.IsLogicPortObstructed(component2.InputCellOne, visElements) || this.IsLogicPortObstructed(component2.OutputCellOne, visElements) || ((component2.RequiresTwoInputs || component2.RequiresFourInputs) && this.IsLogicPortObstructed(component2.InputCellTwo, visElements)) || (component2.RequiresFourInputs && (this.IsLogicPortObstructed(component2.InputCellThree, visElements) || this.IsLogicPortObstructed(component2.InputCellFour, visElements))) || (component2.RequiresFourOutputs && (this.IsLogicPortObstructed(component2.OutputCellTwo, visElements) || this.IsLogicPortObstructed(component2.OutputCellThree, visElements) || this.IsLogicPortObstructed(component2.OutputCellFour, visElements))) || (component2.RequiresControlInputs && (this.IsLogicPortObstructed(component2.ControlCellOne, visElements) || this.IsLogicPortObstructed(component2.ControlCellTwo, visElements)))))
			{
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_LOGIC_PORTS_OBSTRUCTED;
				return false;
			}
		}
		return true;
	}

	// Token: 0x06005EC1 RID: 24257 RVA: 0x002A5E10 File Offset: 0x002A4010
	private bool DoLogicPortsConflict(IList<ILogicUIElement> ports_a, IList<ILogicUIElement> ports_b)
	{
		if (ports_a == null || ports_b == null)
		{
			return false;
		}
		foreach (ILogicUIElement logicUIElement in ports_a)
		{
			int logicUICell = logicUIElement.GetLogicUICell();
			foreach (ILogicUIElement logicUIElement2 in ports_b)
			{
				if (logicUIElement != logicUIElement2 && logicUICell == logicUIElement2.GetLogicUICell())
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06005EC2 RID: 24258 RVA: 0x002A5EAC File Offset: 0x002A40AC
	private bool IsLogicPortObstructed(int cell, IList<ILogicUIElement> ports)
	{
		int num = 0;
		using (IEnumerator<ILogicUIElement> enumerator = ports.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.GetLogicUICell() == cell)
				{
					num++;
				}
			}
		}
		return num > 0;
	}

	// Token: 0x06005EC3 RID: 24259 RVA: 0x002A5F00 File Offset: 0x002A4100
	private bool IsValidConduitConnection(GameObject source_go, ConduitType conduit_type, int utility_cell, ref string fail_reason)
	{
		bool result = true;
		switch (conduit_type)
		{
		case ConduitType.Gas:
		{
			GameObject x = Grid.Objects[utility_cell, 15];
			if (x != null && x != source_go)
			{
				result = false;
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_GASPORTS_OVERLAP;
			}
			break;
		}
		case ConduitType.Liquid:
		{
			GameObject x2 = Grid.Objects[utility_cell, 19];
			if (x2 != null && x2 != source_go)
			{
				result = false;
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_LIQUIDPORTS_OVERLAP;
			}
			break;
		}
		case ConduitType.Solid:
		{
			GameObject x3 = Grid.Objects[utility_cell, 23];
			if (x3 != null && x3 != source_go)
			{
				result = false;
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_SOLIDPORTS_OVERLAP;
			}
			break;
		}
		}
		return result;
	}

	// Token: 0x06005EC4 RID: 24260 RVA: 0x000DDDC9 File Offset: 0x000DBFC9
	public static int GetXOffset(int width)
	{
		return -(width - 1) / 2;
	}

	// Token: 0x06005EC5 RID: 24261 RVA: 0x002A5FBC File Offset: 0x002A41BC
	public static bool CheckFoundation(int cell, Orientation orientation, BuildLocationRule location_rule, int width, int height, Tag optionalFoundationRequiredTag = default(Tag))
	{
		if (location_rule == BuildLocationRule.OnWall)
		{
			return BuildingDef.CheckWallFoundation(cell, width, height, orientation != Orientation.FlipH);
		}
		if (location_rule == BuildLocationRule.InCorner)
		{
			return BuildingDef.CheckBaseFoundation(cell, orientation, BuildLocationRule.OnCeiling, width, height, optionalFoundationRequiredTag) && BuildingDef.CheckWallFoundation(cell, width, height, orientation != Orientation.FlipH);
		}
		if (location_rule == BuildLocationRule.WallFloor)
		{
			return BuildingDef.CheckBaseFoundation(cell, orientation, BuildLocationRule.OnFloor, width, height, optionalFoundationRequiredTag) && BuildingDef.CheckWallFoundation(cell, width, height, orientation != Orientation.FlipH);
		}
		return BuildingDef.CheckBaseFoundation(cell, orientation, location_rule, width, height, optionalFoundationRequiredTag);
	}

	// Token: 0x06005EC6 RID: 24262 RVA: 0x002A6038 File Offset: 0x002A4238
	public static bool CheckBaseFoundation(int cell, Orientation orientation, BuildLocationRule location_rule, int width, int height, Tag optionalFoundationRequiredTag = default(Tag))
	{
		int num = -(width - 1) / 2;
		int num2 = width / 2;
		for (int i = num; i <= num2; i++)
		{
			CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset((location_rule == BuildLocationRule.OnCeiling) ? new CellOffset(i, height) : new CellOffset(i, -1), orientation);
			int num3 = Grid.OffsetCell(cell, rotatedCellOffset);
			if (!Grid.IsValidBuildingCell(num3) || !Grid.Solid[num3])
			{
				return false;
			}
			if (optionalFoundationRequiredTag.IsValid && (!Grid.ObjectLayers[9].ContainsKey(num3) || !Grid.ObjectLayers[9][num3].HasTag(optionalFoundationRequiredTag)))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06005EC7 RID: 24263 RVA: 0x002A60C8 File Offset: 0x002A42C8
	public static bool CheckWallFoundation(int cell, int width, int height, bool leftWall)
	{
		for (int i = 0; i < height; i++)
		{
			CellOffset offset = new CellOffset(leftWall ? (-(width - 1) / 2 - 1) : (width / 2 + 1), i);
			int num = Grid.OffsetCell(cell, offset);
			GameObject gameObject = Grid.Objects[num, 1];
			bool flag = false;
			if (gameObject != null)
			{
				BuildingUnderConstruction component = gameObject.GetComponent<BuildingUnderConstruction>();
				if (component != null && component.Def.IsFoundation)
				{
					flag = true;
				}
			}
			if (!Grid.IsValidBuildingCell(num) || (!Grid.Solid[num] && !flag))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06005EC8 RID: 24264 RVA: 0x002A6164 File Offset: 0x002A4364
	public static bool AreAllCellsValid(int base_cell, Orientation orientation, int width, int height, Func<int, bool> valid_cell_check)
	{
		int num = -(width - 1) / 2;
		int num2 = width / 2;
		if (orientation == Orientation.FlipH)
		{
			int num3 = num;
			num = -num2;
			num2 = -num3;
		}
		for (int i = 0; i < height; i++)
		{
			for (int j = num; j <= num2; j++)
			{
				int arg = Grid.OffsetCell(base_cell, j, i);
				if (!valid_cell_check(arg))
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x06005EC9 RID: 24265 RVA: 0x000DDDD1 File Offset: 0x000DBFD1
	public Sprite GetUISprite(string animName = "ui", bool centered = false)
	{
		return Def.GetUISpriteFromMultiObjectAnim(this.AnimFiles[0], animName, centered, "");
	}

	// Token: 0x06005ECA RID: 24266 RVA: 0x000DDDE7 File Offset: 0x000DBFE7
	public void GenerateOffsets()
	{
		this.GenerateOffsets(this.WidthInCells, this.HeightInCells);
	}

	// Token: 0x06005ECB RID: 24267 RVA: 0x002A61B8 File Offset: 0x002A43B8
	public void GenerateOffsets(int width, int height)
	{
		if (!BuildingDef.placementOffsetsCache.TryGetValue(new CellOffset(width, height), out this.PlacementOffsets))
		{
			int num = width / 2 - width + 1;
			this.PlacementOffsets = new CellOffset[width * height];
			for (int num2 = 0; num2 != height; num2++)
			{
				int num3 = num2 * width;
				for (int num4 = 0; num4 != width; num4++)
				{
					int num5 = num3 + num4;
					this.PlacementOffsets[num5].x = num4 + num;
					this.PlacementOffsets[num5].y = num2;
				}
			}
			BuildingDef.placementOffsetsCache.Add(new CellOffset(width, height), this.PlacementOffsets);
		}
	}

	// Token: 0x06005ECC RID: 24268 RVA: 0x002A6254 File Offset: 0x002A4454
	public void PostProcess()
	{
		this.CraftRecipe = new Recipe(this.BuildingComplete.PrefabID().Name, 1f, (SimHashes)0, this.Name, null, 0);
		this.CraftRecipe.Icon = this.UISprite;
		for (int i = 0; i < this.MaterialCategory.Length; i++)
		{
			TagManager.Create(this.MaterialCategory[i], MATERIALS.GetMaterialString(this.MaterialCategory[i]));
			Recipe.Ingredient item = new Recipe.Ingredient(this.MaterialCategory[i], (float)((int)this.Mass[i]));
			this.CraftRecipe.Ingredients.Add(item);
		}
		if (this.DecorBlockTileInfo != null)
		{
			this.DecorBlockTileInfo.PostProcess();
		}
		if (this.DecorPlaceBlockTileInfo != null)
		{
			this.DecorPlaceBlockTileInfo.PostProcess();
		}
		if (!this.Deprecated)
		{
			Db.Get().TechItems.AddTechItem(this.PrefabID, this.Name, this.Effect, new Func<string, bool, Sprite>(this.GetUISprite), this.RequiredDlcIds, this.ForbiddenDlcIds, this.POIUnlockable);
		}
	}

	// Token: 0x06005ECD RID: 24269 RVA: 0x002A6370 File Offset: 0x002A4570
	public bool MaterialsAvailable(IList<Tag> selected_elements, WorldContainer world)
	{
		bool result = true;
		foreach (Recipe.Ingredient ingredient in this.CraftRecipe.GetAllIngredients(selected_elements))
		{
			if (world.worldInventory.GetAmount(ingredient.tag, true) < ingredient.amount)
			{
				result = false;
				break;
			}
		}
		return result;
	}

	// Token: 0x06005ECE RID: 24270 RVA: 0x002A63C0 File Offset: 0x002A45C0
	public bool CheckRequiresBuildingCellVisualizer()
	{
		return this.CheckRequiresPowerInput() || this.CheckRequiresPowerOutput() || this.CheckRequiresGasInput() || this.CheckRequiresGasOutput() || this.CheckRequiresLiquidInput() || this.CheckRequiresLiquidOutput() || this.CheckRequiresSolidInput() || this.CheckRequiresSolidOutput() || this.CheckRequiresHighEnergyParticleInput() || this.CheckRequiresHighEnergyParticleOutput() || this.SelfHeatKilowattsWhenActive != 0f || this.ExhaustKilowattsWhenActive != 0f || this.DiseaseCellVisName != null;
	}

	// Token: 0x06005ECF RID: 24271 RVA: 0x000DDDFB File Offset: 0x000DBFFB
	public bool CheckRequiresPowerInput()
	{
		return this.RequiresPowerInput;
	}

	// Token: 0x06005ED0 RID: 24272 RVA: 0x000DDE03 File Offset: 0x000DC003
	public bool CheckRequiresPowerOutput()
	{
		return this.RequiresPowerOutput;
	}

	// Token: 0x06005ED1 RID: 24273 RVA: 0x000DDE0B File Offset: 0x000DC00B
	public bool CheckRequiresGasInput()
	{
		return this.InputConduitType == ConduitType.Gas;
	}

	// Token: 0x06005ED2 RID: 24274 RVA: 0x000DDE16 File Offset: 0x000DC016
	public bool CheckRequiresGasOutput()
	{
		return this.OutputConduitType == ConduitType.Gas;
	}

	// Token: 0x06005ED3 RID: 24275 RVA: 0x000DDE21 File Offset: 0x000DC021
	public bool CheckRequiresLiquidInput()
	{
		return this.InputConduitType == ConduitType.Liquid;
	}

	// Token: 0x06005ED4 RID: 24276 RVA: 0x000DDE2C File Offset: 0x000DC02C
	public bool CheckRequiresLiquidOutput()
	{
		return this.OutputConduitType == ConduitType.Liquid;
	}

	// Token: 0x06005ED5 RID: 24277 RVA: 0x000DDE37 File Offset: 0x000DC037
	public bool CheckRequiresSolidInput()
	{
		return this.InputConduitType == ConduitType.Solid;
	}

	// Token: 0x06005ED6 RID: 24278 RVA: 0x000DDE42 File Offset: 0x000DC042
	public bool CheckRequiresSolidOutput()
	{
		return this.OutputConduitType == ConduitType.Solid;
	}

	// Token: 0x06005ED7 RID: 24279 RVA: 0x000DDE4D File Offset: 0x000DC04D
	public bool CheckRequiresHighEnergyParticleInput()
	{
		return this.UseHighEnergyParticleInputPort;
	}

	// Token: 0x06005ED8 RID: 24280 RVA: 0x000DDE55 File Offset: 0x000DC055
	public bool CheckRequiresHighEnergyParticleOutput()
	{
		return this.UseHighEnergyParticleOutputPort;
	}

	// Token: 0x06005ED9 RID: 24281 RVA: 0x000DDE5D File Offset: 0x000DC05D
	public void AddFacade(string db_facade_id)
	{
		if (this.AvailableFacades == null)
		{
			this.AvailableFacades = new List<string>();
		}
		if (!this.AvailableFacades.Contains(db_facade_id))
		{
			this.AvailableFacades.Add(db_facade_id);
		}
	}

	// Token: 0x06005EDA RID: 24282 RVA: 0x000DDE8C File Offset: 0x000DC08C
	public bool IsValidDLC()
	{
		return SaveLoader.Instance.IsCorrectDlcActiveForCurrentSave(this.RequiredDlcIds, this.ForbiddenDlcIds);
	}

	// Token: 0x04004307 RID: 17159
	public string[] RequiredDlcIds;

	// Token: 0x04004308 RID: 17160
	public string[] ForbiddenDlcIds;

	// Token: 0x04004309 RID: 17161
	public float EnergyConsumptionWhenActive;

	// Token: 0x0400430A RID: 17162
	public float GeneratorWattageRating;

	// Token: 0x0400430B RID: 17163
	public float GeneratorBaseCapacity;

	// Token: 0x0400430C RID: 17164
	public float MassForTemperatureModification;

	// Token: 0x0400430D RID: 17165
	public float ExhaustKilowattsWhenActive;

	// Token: 0x0400430E RID: 17166
	public float SelfHeatKilowattsWhenActive;

	// Token: 0x0400430F RID: 17167
	public float BaseMeltingPoint;

	// Token: 0x04004310 RID: 17168
	public float ConstructionTime;

	// Token: 0x04004311 RID: 17169
	public float WorkTime;

	// Token: 0x04004312 RID: 17170
	public float ThermalConductivity = 1f;

	// Token: 0x04004313 RID: 17171
	public int WidthInCells;

	// Token: 0x04004314 RID: 17172
	public int HeightInCells;

	// Token: 0x04004315 RID: 17173
	public int HitPoints;

	// Token: 0x04004316 RID: 17174
	public float Temperature = 293.15f;

	// Token: 0x04004317 RID: 17175
	public bool RequiresPowerInput;

	// Token: 0x04004318 RID: 17176
	public bool AddLogicPowerPort = true;

	// Token: 0x04004319 RID: 17177
	public bool RequiresPowerOutput;

	// Token: 0x0400431A RID: 17178
	public bool UseWhitePowerOutputConnectorColour;

	// Token: 0x0400431B RID: 17179
	public CellOffset ElectricalArrowOffset;

	// Token: 0x0400431C RID: 17180
	public ConduitType InputConduitType;

	// Token: 0x0400431D RID: 17181
	public ConduitType OutputConduitType;

	// Token: 0x0400431E RID: 17182
	public bool ModifiesTemperature;

	// Token: 0x0400431F RID: 17183
	public bool Floodable = true;

	// Token: 0x04004320 RID: 17184
	public bool Disinfectable = true;

	// Token: 0x04004321 RID: 17185
	public bool Entombable = true;

	// Token: 0x04004322 RID: 17186
	public bool Replaceable = true;

	// Token: 0x04004323 RID: 17187
	public bool Invincible;

	// Token: 0x04004324 RID: 17188
	public bool Overheatable = true;

	// Token: 0x04004325 RID: 17189
	public bool Repairable = true;

	// Token: 0x04004326 RID: 17190
	public float OverheatTemperature = 348.15f;

	// Token: 0x04004327 RID: 17191
	public float FatalHot = 533.15f;

	// Token: 0x04004328 RID: 17192
	public bool Breakable;

	// Token: 0x04004329 RID: 17193
	public bool ContinuouslyCheckFoundation;

	// Token: 0x0400432A RID: 17194
	public bool IsFoundation;

	// Token: 0x0400432B RID: 17195
	[Obsolete]
	public bool isSolidTile;

	// Token: 0x0400432C RID: 17196
	public bool DragBuild;

	// Token: 0x0400432D RID: 17197
	public bool UseStructureTemperature = true;

	// Token: 0x0400432E RID: 17198
	public global::Action HotKey = global::Action.NumActions;

	// Token: 0x0400432F RID: 17199
	public CellOffset attachablePosition = new CellOffset(0, 0);

	// Token: 0x04004330 RID: 17200
	public bool CanMove;

	// Token: 0x04004331 RID: 17201
	public bool Cancellable = true;

	// Token: 0x04004332 RID: 17202
	public bool OnePerWorld;

	// Token: 0x04004333 RID: 17203
	public bool PlayConstructionSounds = true;

	// Token: 0x04004334 RID: 17204
	public Func<CodexEntry, CodexEntry> ExtendCodexEntry;

	// Token: 0x04004335 RID: 17205
	public bool POIUnlockable;

	// Token: 0x04004336 RID: 17206
	public List<Tag> ReplacementTags;

	// Token: 0x04004337 RID: 17207
	public List<ObjectLayer> ReplacementCandidateLayers;

	// Token: 0x04004338 RID: 17208
	public List<ObjectLayer> EquivalentReplacementLayers;

	// Token: 0x04004339 RID: 17209
	[HashedEnum]
	[NonSerialized]
	public HashedString ViewMode = OverlayModes.None.ID;

	// Token: 0x0400433A RID: 17210
	public BuildLocationRule BuildLocationRule;

	// Token: 0x0400433B RID: 17211
	public ObjectLayer ObjectLayer = ObjectLayer.Building;

	// Token: 0x0400433C RID: 17212
	public ObjectLayer TileLayer = ObjectLayer.NumLayers;

	// Token: 0x0400433D RID: 17213
	public ObjectLayer ReplacementLayer = ObjectLayer.NumLayers;

	// Token: 0x0400433E RID: 17214
	public string DiseaseCellVisName;

	// Token: 0x0400433F RID: 17215
	public string[] MaterialCategory;

	// Token: 0x04004340 RID: 17216
	public string AudioCategory = "Metal";

	// Token: 0x04004341 RID: 17217
	public string AudioSize = "medium";

	// Token: 0x04004342 RID: 17218
	public float[] Mass;

	// Token: 0x04004343 RID: 17219
	public bool AlwaysOperational;

	// Token: 0x04004344 RID: 17220
	public List<LogicPorts.Port> LogicInputPorts;

	// Token: 0x04004345 RID: 17221
	public List<LogicPorts.Port> LogicOutputPorts;

	// Token: 0x04004346 RID: 17222
	public bool Upgradeable;

	// Token: 0x04004347 RID: 17223
	public float BaseTimeUntilRepair = 600f;

	// Token: 0x04004348 RID: 17224
	public bool ShowInBuildMenu = true;

	// Token: 0x04004349 RID: 17225
	public bool DebugOnly;

	// Token: 0x0400434A RID: 17226
	public PermittedRotations PermittedRotations;

	// Token: 0x0400434B RID: 17227
	public Orientation InitialOrientation;

	// Token: 0x0400434C RID: 17228
	public bool Deprecated;

	// Token: 0x0400434D RID: 17229
	public bool UseHighEnergyParticleInputPort;

	// Token: 0x0400434E RID: 17230
	public bool UseHighEnergyParticleOutputPort;

	// Token: 0x0400434F RID: 17231
	public CellOffset HighEnergyParticleInputOffset;

	// Token: 0x04004350 RID: 17232
	public CellOffset HighEnergyParticleOutputOffset;

	// Token: 0x04004351 RID: 17233
	public CellOffset PowerInputOffset;

	// Token: 0x04004352 RID: 17234
	public CellOffset PowerOutputOffset;

	// Token: 0x04004353 RID: 17235
	public CellOffset UtilityInputOffset = new CellOffset(0, 1);

	// Token: 0x04004354 RID: 17236
	public CellOffset UtilityOutputOffset = new CellOffset(1, 0);

	// Token: 0x04004355 RID: 17237
	public Grid.SceneLayer SceneLayer = Grid.SceneLayer.Building;

	// Token: 0x04004356 RID: 17238
	public Grid.SceneLayer ForegroundLayer = Grid.SceneLayer.BuildingFront;

	// Token: 0x04004357 RID: 17239
	public string RequiredAttribute = "";

	// Token: 0x04004358 RID: 17240
	public int RequiredAttributeLevel;

	// Token: 0x04004359 RID: 17241
	public List<Descriptor> EffectDescription;

	// Token: 0x0400435A RID: 17242
	public float MassTier;

	// Token: 0x0400435B RID: 17243
	public float HeatTier;

	// Token: 0x0400435C RID: 17244
	public float ConstructionTimeTier;

	// Token: 0x0400435D RID: 17245
	public string PrimaryUse;

	// Token: 0x0400435E RID: 17246
	public string SecondaryUse;

	// Token: 0x0400435F RID: 17247
	public string PrimarySideEffect;

	// Token: 0x04004360 RID: 17248
	public string SecondarySideEffect;

	// Token: 0x04004361 RID: 17249
	public Recipe CraftRecipe;

	// Token: 0x04004362 RID: 17250
	public Sprite UISprite;

	// Token: 0x04004363 RID: 17251
	public bool isKAnimTile;

	// Token: 0x04004364 RID: 17252
	public bool isUtility;

	// Token: 0x04004365 RID: 17253
	public KAnimFile[] AnimFiles;

	// Token: 0x04004366 RID: 17254
	public string DefaultAnimState = "off";

	// Token: 0x04004367 RID: 17255
	public bool BlockTileIsTransparent;

	// Token: 0x04004368 RID: 17256
	public TextureAtlas BlockTileAtlas;

	// Token: 0x04004369 RID: 17257
	public TextureAtlas BlockTilePlaceAtlas;

	// Token: 0x0400436A RID: 17258
	public TextureAtlas BlockTileShineAtlas;

	// Token: 0x0400436B RID: 17259
	public Material BlockTileMaterial;

	// Token: 0x0400436C RID: 17260
	public BlockTileDecorInfo DecorBlockTileInfo;

	// Token: 0x0400436D RID: 17261
	public BlockTileDecorInfo DecorPlaceBlockTileInfo;

	// Token: 0x0400436E RID: 17262
	public List<Klei.AI.Attribute> attributes = new List<Klei.AI.Attribute>();

	// Token: 0x0400436F RID: 17263
	public List<AttributeModifier> attributeModifiers = new List<AttributeModifier>();

	// Token: 0x04004370 RID: 17264
	public Tag AttachmentSlotTag;

	// Token: 0x04004371 RID: 17265
	public bool PreventIdleTraversalPastBuilding;

	// Token: 0x04004372 RID: 17266
	public GameObject BuildingComplete;

	// Token: 0x04004373 RID: 17267
	public GameObject BuildingPreview;

	// Token: 0x04004374 RID: 17268
	public GameObject BuildingUnderConstruction;

	// Token: 0x04004375 RID: 17269
	public CellOffset[] PlacementOffsets;

	// Token: 0x04004376 RID: 17270
	public CellOffset[] ConstructionOffsetFilter;

	// Token: 0x04004377 RID: 17271
	public static CellOffset[] ConstructionOffsetFilter_OneDown = new CellOffset[]
	{
		new CellOffset(0, -1)
	};

	// Token: 0x04004378 RID: 17272
	public float BaseDecor;

	// Token: 0x04004379 RID: 17273
	public float BaseDecorRadius;

	// Token: 0x0400437A RID: 17274
	public int BaseNoisePollution;

	// Token: 0x0400437B RID: 17275
	public int BaseNoisePollutionRadius;

	// Token: 0x0400437C RID: 17276
	public List<string> AvailableFacades = new List<string>();

	// Token: 0x0400437D RID: 17277
	private static Dictionary<CellOffset, CellOffset[]> placementOffsetsCache = new Dictionary<CellOffset, CellOffset[]>();
}
