using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B18 RID: 2840
public class StatusItemRenderer
{
	// Token: 0x17000246 RID: 582
	// (get) Token: 0x0600356C RID: 13676 RVA: 0x000C2D9E File Offset: 0x000C0F9E
	// (set) Token: 0x0600356D RID: 13677 RVA: 0x000C2DA6 File Offset: 0x000C0FA6
	public int layer { get; private set; }

	// Token: 0x17000247 RID: 583
	// (get) Token: 0x0600356E RID: 13678 RVA: 0x000C2DAF File Offset: 0x000C0FAF
	// (set) Token: 0x0600356F RID: 13679 RVA: 0x000C2DB7 File Offset: 0x000C0FB7
	public int selectedHandle { get; private set; }

	// Token: 0x17000248 RID: 584
	// (get) Token: 0x06003570 RID: 13680 RVA: 0x000C2DC0 File Offset: 0x000C0FC0
	// (set) Token: 0x06003571 RID: 13681 RVA: 0x000C2DC8 File Offset: 0x000C0FC8
	public int highlightHandle { get; private set; }

	// Token: 0x17000249 RID: 585
	// (get) Token: 0x06003572 RID: 13682 RVA: 0x000C2DD1 File Offset: 0x000C0FD1
	// (set) Token: 0x06003573 RID: 13683 RVA: 0x000C2DD9 File Offset: 0x000C0FD9
	public Color32 backgroundColor { get; private set; }

	// Token: 0x1700024A RID: 586
	// (get) Token: 0x06003574 RID: 13684 RVA: 0x000C2DE2 File Offset: 0x000C0FE2
	// (set) Token: 0x06003575 RID: 13685 RVA: 0x000C2DEA File Offset: 0x000C0FEA
	public Color32 selectedColor { get; private set; }

	// Token: 0x1700024B RID: 587
	// (get) Token: 0x06003576 RID: 13686 RVA: 0x000C2DF3 File Offset: 0x000C0FF3
	// (set) Token: 0x06003577 RID: 13687 RVA: 0x000C2DFB File Offset: 0x000C0FFB
	public Color32 neutralColor { get; private set; }

	// Token: 0x1700024C RID: 588
	// (get) Token: 0x06003578 RID: 13688 RVA: 0x000C2E04 File Offset: 0x000C1004
	// (set) Token: 0x06003579 RID: 13689 RVA: 0x000C2E0C File Offset: 0x000C100C
	public Sprite arrowSprite { get; private set; }

	// Token: 0x1700024D RID: 589
	// (get) Token: 0x0600357A RID: 13690 RVA: 0x000C2E15 File Offset: 0x000C1015
	// (set) Token: 0x0600357B RID: 13691 RVA: 0x000C2E1D File Offset: 0x000C101D
	public Sprite backgroundSprite { get; private set; }

	// Token: 0x1700024E RID: 590
	// (get) Token: 0x0600357C RID: 13692 RVA: 0x000C2E26 File Offset: 0x000C1026
	// (set) Token: 0x0600357D RID: 13693 RVA: 0x000C2E2E File Offset: 0x000C102E
	public float scale { get; private set; }

	// Token: 0x0600357E RID: 13694 RVA: 0x0020E8E0 File Offset: 0x0020CAE0
	public StatusItemRenderer()
	{
		this.layer = LayerMask.NameToLayer("UI");
		this.entries = new StatusItemRenderer.Entry[100];
		this.shader = Shader.Find("Klei/StatusItem");
		for (int i = 0; i < this.entries.Length; i++)
		{
			StatusItemRenderer.Entry entry = default(StatusItemRenderer.Entry);
			entry.Init(this.shader);
			this.entries[i] = entry;
		}
		this.backgroundColor = new Color32(244, 74, 71, byte.MaxValue);
		this.selectedColor = new Color32(225, 181, 180, byte.MaxValue);
		this.neutralColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		this.arrowSprite = Assets.GetSprite("StatusBubbleTop");
		this.backgroundSprite = Assets.GetSprite("StatusBubble");
		this.scale = 1f;
		Game.Instance.Subscribe(2095258329, new Action<object>(this.OnHighlightObject));
	}

	// Token: 0x0600357F RID: 13695 RVA: 0x0020EA14 File Offset: 0x0020CC14
	public int GetIdx(Transform transform)
	{
		int instanceID = transform.GetInstanceID();
		int num = 0;
		if (!this.handleTable.TryGetValue(instanceID, out num))
		{
			int num2 = this.entryCount;
			this.entryCount = num2 + 1;
			num = num2;
			this.handleTable[instanceID] = num;
			StatusItemRenderer.Entry entry = this.entries[num];
			entry.handle = instanceID;
			entry.transform = transform;
			entry.buildingPos = transform.GetPosition();
			entry.building = transform.GetComponent<Building>();
			entry.isBuilding = (entry.building != null);
			entry.selectable = transform.GetComponent<KSelectable>();
			this.entries[num] = entry;
		}
		return num;
	}

	// Token: 0x06003580 RID: 13696 RVA: 0x0020EAC4 File Offset: 0x0020CCC4
	public void Add(Transform transform, StatusItem status_item)
	{
		if (this.entryCount == this.entries.Length)
		{
			StatusItemRenderer.Entry[] array = new StatusItemRenderer.Entry[this.entries.Length * 2];
			for (int i = 0; i < this.entries.Length; i++)
			{
				array[i] = this.entries[i];
			}
			for (int j = this.entries.Length; j < array.Length; j++)
			{
				array[j].Init(this.shader);
			}
			this.entries = array;
		}
		int idx = this.GetIdx(transform);
		StatusItemRenderer.Entry entry = this.entries[idx];
		entry.Add(status_item);
		this.entries[idx] = entry;
	}

	// Token: 0x06003581 RID: 13697 RVA: 0x0020EB74 File Offset: 0x0020CD74
	public void Remove(Transform transform, StatusItem status_item)
	{
		int instanceID = transform.GetInstanceID();
		int num = 0;
		if (!this.handleTable.TryGetValue(instanceID, out num))
		{
			return;
		}
		StatusItemRenderer.Entry entry = this.entries[num];
		if (entry.statusItems.Count == 0)
		{
			return;
		}
		entry.Remove(status_item);
		this.entries[num] = entry;
		if (entry.statusItems.Count == 0)
		{
			this.ClearIdx(num);
		}
	}

	// Token: 0x06003582 RID: 13698 RVA: 0x0020EBE0 File Offset: 0x0020CDE0
	private void ClearIdx(int idx)
	{
		StatusItemRenderer.Entry entry = this.entries[idx];
		this.handleTable.Remove(entry.handle);
		if (idx != this.entryCount - 1)
		{
			entry.Replace(this.entries[this.entryCount - 1]);
			this.entries[idx] = entry;
			this.handleTable[entry.handle] = idx;
		}
		entry = this.entries[this.entryCount - 1];
		entry.Clear();
		this.entries[this.entryCount - 1] = entry;
		this.entryCount--;
	}

	// Token: 0x06003583 RID: 13699 RVA: 0x000C2E37 File Offset: 0x000C1037
	private HashedString GetMode()
	{
		if (OverlayScreen.Instance != null)
		{
			return OverlayScreen.Instance.mode;
		}
		return OverlayModes.None.ID;
	}

	// Token: 0x06003584 RID: 13700 RVA: 0x0020EC90 File Offset: 0x0020CE90
	public void MarkAllDirty()
	{
		for (int i = 0; i < this.entryCount; i++)
		{
			this.entries[i].MarkDirty();
		}
	}

	// Token: 0x06003585 RID: 13701 RVA: 0x0020ECC0 File Offset: 0x0020CEC0
	public void RenderEveryTick()
	{
		if (DebugHandler.HideUI)
		{
			return;
		}
		this.scale = 1f + Mathf.Sin(Time.unscaledTime * 8f) * 0.1f;
		Shader.SetGlobalVector("_StatusItemParameters", new Vector4(this.scale, 0f, 0f, 0f));
		Vector3 camera_tr = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, Camera.main.transform.GetPosition().z));
		Vector3 camera_bl = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, Camera.main.transform.GetPosition().z));
		this.visibleEntries.Clear();
		Camera worldCamera = GameScreenManager.Instance.worldSpaceCanvas.GetComponent<Canvas>().worldCamera;
		for (int i = 0; i < this.entryCount; i++)
		{
			this.entries[i].Render(this, camera_bl, camera_tr, this.GetMode(), worldCamera);
		}
	}

	// Token: 0x06003586 RID: 13702 RVA: 0x0020EDC4 File Offset: 0x0020CFC4
	public void GetIntersections(Vector2 pos, List<InterfaceTool.Intersection> intersections)
	{
		foreach (StatusItemRenderer.Entry entry in this.visibleEntries)
		{
			entry.GetIntersection(pos, intersections, this.scale);
		}
	}

	// Token: 0x06003587 RID: 13703 RVA: 0x0020EE20 File Offset: 0x0020D020
	public void GetIntersections(Vector2 pos, List<KSelectable> selectables)
	{
		foreach (StatusItemRenderer.Entry entry in this.visibleEntries)
		{
			entry.GetIntersection(pos, selectables, this.scale);
		}
	}

	// Token: 0x06003588 RID: 13704 RVA: 0x0020EE7C File Offset: 0x0020D07C
	public void SetOffset(Transform transform, Vector3 offset)
	{
		int num = 0;
		if (this.handleTable.TryGetValue(transform.GetInstanceID(), out num))
		{
			this.entries[num].offset = offset;
		}
	}

	// Token: 0x06003589 RID: 13705 RVA: 0x0020EEB4 File Offset: 0x0020D0B4
	private void OnSelectObject(object data)
	{
		int num = 0;
		if (this.handleTable.TryGetValue(this.selectedHandle, out num))
		{
			this.entries[num].MarkDirty();
		}
		GameObject gameObject = (GameObject)data;
		if (gameObject != null)
		{
			this.selectedHandle = gameObject.transform.GetInstanceID();
			if (this.handleTable.TryGetValue(this.selectedHandle, out num))
			{
				this.entries[num].MarkDirty();
				return;
			}
		}
		else
		{
			this.highlightHandle = -1;
		}
	}

	// Token: 0x0600358A RID: 13706 RVA: 0x0020EF38 File Offset: 0x0020D138
	private void OnHighlightObject(object data)
	{
		int num = 0;
		if (this.handleTable.TryGetValue(this.highlightHandle, out num))
		{
			StatusItemRenderer.Entry entry = this.entries[num];
			entry.MarkDirty();
			this.entries[num] = entry;
		}
		GameObject gameObject = (GameObject)data;
		if (gameObject != null)
		{
			this.highlightHandle = gameObject.transform.GetInstanceID();
			if (this.handleTable.TryGetValue(this.highlightHandle, out num))
			{
				StatusItemRenderer.Entry entry2 = this.entries[num];
				entry2.MarkDirty();
				this.entries[num] = entry2;
				return;
			}
		}
		else
		{
			this.highlightHandle = -1;
		}
	}

	// Token: 0x0600358B RID: 13707 RVA: 0x0020EFDC File Offset: 0x0020D1DC
	public void Destroy()
	{
		Game.Instance.Unsubscribe(-1503271301, new Action<object>(this.OnSelectObject));
		Game.Instance.Unsubscribe(-1201923725, new Action<object>(this.OnHighlightObject));
		foreach (StatusItemRenderer.Entry entry in this.entries)
		{
			entry.Clear();
			entry.FreeResources();
		}
	}

	// Token: 0x04002454 RID: 9300
	private StatusItemRenderer.Entry[] entries;

	// Token: 0x04002455 RID: 9301
	private int entryCount;

	// Token: 0x04002456 RID: 9302
	private Dictionary<int, int> handleTable = new Dictionary<int, int>();

	// Token: 0x04002460 RID: 9312
	private Shader shader;

	// Token: 0x04002461 RID: 9313
	public List<StatusItemRenderer.Entry> visibleEntries = new List<StatusItemRenderer.Entry>();

	// Token: 0x02000B19 RID: 2841
	public struct Entry
	{
		// Token: 0x0600358C RID: 13708 RVA: 0x000C2E56 File Offset: 0x000C1056
		public void Init(Shader shader)
		{
			this.statusItems = new List<StatusItem>();
			this.mesh = new Mesh();
			this.mesh.name = "StatusItemRenderer";
			this.dirty = true;
			this.material = new Material(shader);
		}

		// Token: 0x0600358D RID: 13709 RVA: 0x0020F04C File Offset: 0x0020D24C
		public void Render(StatusItemRenderer renderer, Vector3 camera_bl, Vector3 camera_tr, HashedString overlay, Camera camera)
		{
			if (this.transform == null)
			{
				string text = "Error cleaning up status items:";
				foreach (StatusItem statusItem in this.statusItems)
				{
					text += statusItem.Id;
				}
				global::Debug.LogWarning(text);
				return;
			}
			Vector3 vector = this.isBuilding ? this.buildingPos : this.transform.GetPosition();
			if (this.isBuilding)
			{
				vector.x += (float)((this.building.Def.WidthInCells - 1) % 2) / 2f;
			}
			if (vector.x < camera_bl.x || vector.x > camera_tr.x || vector.y < camera_bl.y || vector.y > camera_tr.y)
			{
				return;
			}
			int num = Grid.PosToCell(vector);
			if (Grid.IsValidCell(num) && (!Grid.IsVisible(num) || (int)Grid.WorldIdx[num] != ClusterManager.Instance.activeWorldId))
			{
				return;
			}
			if (!this.selectable.IsSelectable)
			{
				return;
			}
			renderer.visibleEntries.Add(this);
			if (this.dirty)
			{
				int num2 = 0;
				StatusItemRenderer.Entry.spritesListedToRender.Clear();
				StatusItemRenderer.Entry.statusItemsToRender_Index.Clear();
				int num3 = -1;
				foreach (StatusItem statusItem2 in this.statusItems)
				{
					num3++;
					if (statusItem2.UseConditionalCallback(overlay, this.transform) || !(overlay != OverlayModes.None.ID) || !(statusItem2.render_overlay != overlay))
					{
						Sprite sprite = statusItem2.sprite.sprite;
						if (!statusItem2.unique)
						{
							if (StatusItemRenderer.Entry.spritesListedToRender.Contains(sprite) || StatusItemRenderer.Entry.spritesListedToRender.Count >= StatusItemRenderer.Entry.spritesListedToRender.Capacity)
							{
								continue;
							}
							StatusItemRenderer.Entry.spritesListedToRender.Add(sprite);
						}
						StatusItemRenderer.Entry.statusItemsToRender_Index.Add(num3);
						num2++;
					}
				}
				this.hasVisibleStatusItems = (num2 != 0);
				StatusItemRenderer.Entry.MeshBuilder meshBuilder = new StatusItemRenderer.Entry.MeshBuilder(num2 + 6, this.material);
				float num4 = 0.25f;
				float z = -5f;
				Vector2 b = new Vector2(0.05f, -0.05f);
				float num5 = 0.02f;
				Color32 c = new Color32(0, 0, 0, byte.MaxValue);
				Color32 c2 = new Color32(0, 0, 0, 75);
				Color32 c3 = renderer.neutralColor;
				if (renderer.selectedHandle == this.handle || renderer.highlightHandle == this.handle)
				{
					c3 = renderer.selectedColor;
				}
				else
				{
					for (int i = 0; i < this.statusItems.Count; i++)
					{
						if (this.statusItems[i].notificationType != NotificationType.Neutral)
						{
							c3 = renderer.backgroundColor;
							break;
						}
					}
				}
				meshBuilder.AddQuad(new Vector2(0f, 0.29f) + b, new Vector2(0.05f, 0.05f), z, renderer.arrowSprite, c2);
				meshBuilder.AddQuad(new Vector2(0f, 0f) + b, new Vector2(num4 * (float)num2, num4), z, renderer.backgroundSprite, c2);
				meshBuilder.AddQuad(new Vector2(0f, 0f), new Vector2(num4 * (float)num2 + num5, num4 + num5), z, renderer.backgroundSprite, c);
				meshBuilder.AddQuad(new Vector2(0f, 0f), new Vector2(num4 * (float)num2, num4), z, renderer.backgroundSprite, c3);
				for (int j = 0; j < StatusItemRenderer.Entry.statusItemsToRender_Index.Count; j++)
				{
					StatusItem statusItem3 = this.statusItems[StatusItemRenderer.Entry.statusItemsToRender_Index[j]];
					float x = (float)j * num4 * 2f - num4 * (float)(num2 - 1);
					if (statusItem3.sprite == null)
					{
						DebugUtil.DevLogError(string.Concat(new string[]
						{
							"Status Item ",
							statusItem3.Id,
							" has null sprite for icon '",
							statusItem3.iconName,
							"', you need to run Collect Sprites or manually add the sprite to the TintedSprites list in the GameAssets prefab."
						}));
						statusItem3.iconName = "status_item_exclamation";
						statusItem3.sprite = Assets.GetTintedSprite("status_item_exclamation");
					}
					Sprite sprite2 = statusItem3.sprite.sprite;
					meshBuilder.AddQuad(new Vector2(x, 0f), new Vector2(num4, num4), z, sprite2, c);
				}
				meshBuilder.AddQuad(new Vector2(0f, 0.29f + num5), new Vector2(0.05f + num5, 0.05f + num5), z, renderer.arrowSprite, c);
				meshBuilder.AddQuad(new Vector2(0f, 0.29f), new Vector2(0.05f, 0.05f), z, renderer.arrowSprite, c3);
				meshBuilder.End(this.mesh);
				this.dirty = false;
			}
			if (this.hasVisibleStatusItems && GameScreenManager.Instance != null)
			{
				Graphics.DrawMesh(this.mesh, vector + this.offset, Quaternion.identity, this.material, renderer.layer, camera, 0, null, false, false);
			}
		}

		// Token: 0x0600358E RID: 13710 RVA: 0x000C2E91 File Offset: 0x000C1091
		public void Add(StatusItem status_item)
		{
			this.statusItems.Add(status_item);
			this.dirty = true;
		}

		// Token: 0x0600358F RID: 13711 RVA: 0x000C2EA6 File Offset: 0x000C10A6
		public void Remove(StatusItem status_item)
		{
			this.statusItems.Remove(status_item);
			this.dirty = true;
		}

		// Token: 0x06003590 RID: 13712 RVA: 0x0020F5D4 File Offset: 0x0020D7D4
		public void Replace(StatusItemRenderer.Entry entry)
		{
			this.handle = entry.handle;
			this.transform = entry.transform;
			this.building = this.transform.GetComponent<Building>();
			this.buildingPos = this.transform.GetPosition();
			this.isBuilding = (this.building != null);
			this.selectable = this.transform.GetComponent<KSelectable>();
			this.offset = entry.offset;
			this.dirty = true;
			this.statusItems.Clear();
			this.statusItems.AddRange(entry.statusItems);
		}

		// Token: 0x06003591 RID: 13713 RVA: 0x0020F670 File Offset: 0x0020D870
		private bool Intersects(Vector2 pos, float scale)
		{
			if (this.transform == null)
			{
				return false;
			}
			Bounds bounds = this.mesh.bounds;
			Vector3 vector = this.buildingPos + this.offset + bounds.center;
			Vector2 a = new Vector2(vector.x, vector.y);
			Vector3 size = bounds.size;
			Vector2 b = new Vector2(size.x * scale * 0.5f, size.y * scale * 0.5f);
			Vector2 vector2 = a - b;
			Vector2 vector3 = a + b;
			return pos.x >= vector2.x && pos.x <= vector3.x && pos.y >= vector2.y && pos.y <= vector3.y;
		}

		// Token: 0x06003592 RID: 13714 RVA: 0x0020F748 File Offset: 0x0020D948
		public void GetIntersection(Vector2 pos, List<InterfaceTool.Intersection> intersections, float scale)
		{
			if (this.Intersects(pos, scale) && this.selectable.IsSelectable)
			{
				intersections.Add(new InterfaceTool.Intersection
				{
					component = this.selectable,
					distance = -100f
				});
			}
		}

		// Token: 0x06003593 RID: 13715 RVA: 0x000C2EBC File Offset: 0x000C10BC
		public void GetIntersection(Vector2 pos, List<KSelectable> selectables, float scale)
		{
			if (this.Intersects(pos, scale) && this.selectable.IsSelectable && !selectables.Contains(this.selectable))
			{
				selectables.Add(this.selectable);
			}
		}

		// Token: 0x06003594 RID: 13716 RVA: 0x000C2EEF File Offset: 0x000C10EF
		public void Clear()
		{
			this.statusItems.Clear();
			this.offset = Vector3.zero;
			this.dirty = false;
		}

		// Token: 0x06003595 RID: 13717 RVA: 0x000C2F0E File Offset: 0x000C110E
		public void FreeResources()
		{
			if (this.mesh != null)
			{
				UnityEngine.Object.DestroyImmediate(this.mesh);
				this.mesh = null;
			}
			if (this.material != null)
			{
				UnityEngine.Object.DestroyImmediate(this.material);
			}
		}

		// Token: 0x06003596 RID: 13718 RVA: 0x000C2F49 File Offset: 0x000C1149
		public void MarkDirty()
		{
			this.dirty = true;
		}

		// Token: 0x04002462 RID: 9314
		public int handle;

		// Token: 0x04002463 RID: 9315
		public Transform transform;

		// Token: 0x04002464 RID: 9316
		public Building building;

		// Token: 0x04002465 RID: 9317
		public Vector3 buildingPos;

		// Token: 0x04002466 RID: 9318
		public KSelectable selectable;

		// Token: 0x04002467 RID: 9319
		public List<StatusItem> statusItems;

		// Token: 0x04002468 RID: 9320
		public Mesh mesh;

		// Token: 0x04002469 RID: 9321
		public bool dirty;

		// Token: 0x0400246A RID: 9322
		public int layer;

		// Token: 0x0400246B RID: 9323
		public Material material;

		// Token: 0x0400246C RID: 9324
		public Vector3 offset;

		// Token: 0x0400246D RID: 9325
		public bool hasVisibleStatusItems;

		// Token: 0x0400246E RID: 9326
		public bool isBuilding;

		// Token: 0x0400246F RID: 9327
		private const int STATUS_ICONS_LIMIT = 12;

		// Token: 0x04002470 RID: 9328
		public static List<Sprite> spritesListedToRender = new List<Sprite>(12);

		// Token: 0x04002471 RID: 9329
		public static List<int> statusItemsToRender_Index = new List<int>(12);

		// Token: 0x02000B1A RID: 2842
		private struct MeshBuilder
		{
			// Token: 0x06003598 RID: 13720 RVA: 0x0020F794 File Offset: 0x0020D994
			public MeshBuilder(int quad_count, Material material)
			{
				this.vertices = new Vector3[4 * quad_count];
				this.uvs = new Vector2[4 * quad_count];
				this.uv2s = new Vector2[4 * quad_count];
				this.colors = new Color32[4 * quad_count];
				this.triangles = new int[6 * quad_count];
				this.material = material;
				this.quadIdx = 0;
			}

			// Token: 0x06003599 RID: 13721 RVA: 0x0020F7F8 File Offset: 0x0020D9F8
			public void AddQuad(Vector2 center, Vector2 half_size, float z, Sprite sprite, Color color)
			{
				if (this.quadIdx == StatusItemRenderer.Entry.MeshBuilder.textureIds.Length)
				{
					return;
				}
				Rect rect = sprite.rect;
				Rect textureRect = sprite.textureRect;
				float num = textureRect.width / rect.width;
				float num2 = textureRect.height / rect.height;
				int num3 = 4 * this.quadIdx;
				this.vertices[num3] = new Vector3((center.x - half_size.x) * num, (center.y - half_size.y) * num2, z);
				this.vertices[1 + num3] = new Vector3((center.x - half_size.x) * num, (center.y + half_size.y) * num2, z);
				this.vertices[2 + num3] = new Vector3((center.x + half_size.x) * num, (center.y - half_size.y) * num2, z);
				this.vertices[3 + num3] = new Vector3((center.x + half_size.x) * num, (center.y + half_size.y) * num2, z);
				float num4 = textureRect.x / (float)sprite.texture.width;
				float num5 = textureRect.y / (float)sprite.texture.height;
				float num6 = textureRect.width / (float)sprite.texture.width;
				float num7 = textureRect.height / (float)sprite.texture.height;
				this.uvs[num3] = new Vector2(num4, num5);
				this.uvs[1 + num3] = new Vector2(num4, num5 + num7);
				this.uvs[2 + num3] = new Vector2(num4 + num6, num5);
				this.uvs[3 + num3] = new Vector2(num4 + num6, num5 + num7);
				this.colors[num3] = color;
				this.colors[1 + num3] = color;
				this.colors[2 + num3] = color;
				this.colors[3 + num3] = color;
				float x = (float)this.quadIdx + 0.5f;
				this.uv2s[num3] = new Vector2(x, 0f);
				this.uv2s[1 + num3] = new Vector2(x, 0f);
				this.uv2s[2 + num3] = new Vector2(x, 0f);
				this.uv2s[3 + num3] = new Vector2(x, 0f);
				int num8 = 6 * this.quadIdx;
				this.triangles[num8] = num3;
				this.triangles[1 + num8] = num3 + 1;
				this.triangles[2 + num8] = num3 + 2;
				this.triangles[3 + num8] = num3 + 2;
				this.triangles[4 + num8] = num3 + 1;
				this.triangles[5 + num8] = num3 + 3;
				this.material.SetTexture(StatusItemRenderer.Entry.MeshBuilder.textureIds[this.quadIdx], sprite.texture);
				this.quadIdx++;
			}

			// Token: 0x0600359A RID: 13722 RVA: 0x0020FB44 File Offset: 0x0020DD44
			public void End(Mesh mesh)
			{
				mesh.Clear();
				mesh.vertices = this.vertices;
				mesh.uv = this.uvs;
				mesh.uv2 = this.uv2s;
				mesh.colors32 = this.colors;
				mesh.SetTriangles(this.triangles, 0);
				mesh.RecalculateBounds();
			}

			// Token: 0x04002472 RID: 9330
			private Vector3[] vertices;

			// Token: 0x04002473 RID: 9331
			private Vector2[] uvs;

			// Token: 0x04002474 RID: 9332
			private Vector2[] uv2s;

			// Token: 0x04002475 RID: 9333
			private int[] triangles;

			// Token: 0x04002476 RID: 9334
			private Color32[] colors;

			// Token: 0x04002477 RID: 9335
			private int quadIdx;

			// Token: 0x04002478 RID: 9336
			private Material material;

			// Token: 0x04002479 RID: 9337
			private static int[] textureIds = new int[]
			{
				Shader.PropertyToID("_Tex0"),
				Shader.PropertyToID("_Tex1"),
				Shader.PropertyToID("_Tex2"),
				Shader.PropertyToID("_Tex3"),
				Shader.PropertyToID("_Tex4"),
				Shader.PropertyToID("_Tex5"),
				Shader.PropertyToID("_Tex6"),
				Shader.PropertyToID("_Tex7"),
				Shader.PropertyToID("_Tex8"),
				Shader.PropertyToID("_Tex9"),
				Shader.PropertyToID("_Tex10")
			};
		}
	}
}
