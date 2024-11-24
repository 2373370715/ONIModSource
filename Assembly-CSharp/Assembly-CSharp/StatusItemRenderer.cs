﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class StatusItemRenderer
{
			public int layer { get; private set; }

			public int selectedHandle { get; private set; }

			public int highlightHandle { get; private set; }

			public Color32 backgroundColor { get; private set; }

			public Color32 selectedColor { get; private set; }

			public Color32 neutralColor { get; private set; }

			public Sprite arrowSprite { get; private set; }

			public Sprite backgroundSprite { get; private set; }

			public float scale { get; private set; }

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

	private HashedString GetMode()
	{
		if (OverlayScreen.Instance != null)
		{
			return OverlayScreen.Instance.mode;
		}
		return OverlayModes.None.ID;
	}

	public void MarkAllDirty()
	{
		for (int i = 0; i < this.entryCount; i++)
		{
			this.entries[i].MarkDirty();
		}
	}

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

	public void GetIntersections(Vector2 pos, List<InterfaceTool.Intersection> intersections)
	{
		foreach (StatusItemRenderer.Entry entry in this.visibleEntries)
		{
			entry.GetIntersection(pos, intersections, this.scale);
		}
	}

	public void GetIntersections(Vector2 pos, List<KSelectable> selectables)
	{
		foreach (StatusItemRenderer.Entry entry in this.visibleEntries)
		{
			entry.GetIntersection(pos, selectables, this.scale);
		}
	}

	public void SetOffset(Transform transform, Vector3 offset)
	{
		int num = 0;
		if (this.handleTable.TryGetValue(transform.GetInstanceID(), out num))
		{
			this.entries[num].offset = offset;
		}
	}

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

	private StatusItemRenderer.Entry[] entries;

	private int entryCount;

	private Dictionary<int, int> handleTable = new Dictionary<int, int>();

	private Shader shader;

	public List<StatusItemRenderer.Entry> visibleEntries = new List<StatusItemRenderer.Entry>();

	public struct Entry
	{
		public void Init(Shader shader)
		{
			this.statusItems = new List<StatusItem>();
			this.mesh = new Mesh();
			this.mesh.name = "StatusItemRenderer";
			this.dirty = true;
			this.material = new Material(shader);
		}

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

		public void Add(StatusItem status_item)
		{
			this.statusItems.Add(status_item);
			this.dirty = true;
		}

		public void Remove(StatusItem status_item)
		{
			this.statusItems.Remove(status_item);
			this.dirty = true;
		}

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

		public void GetIntersection(Vector2 pos, List<KSelectable> selectables, float scale)
		{
			if (this.Intersects(pos, scale) && this.selectable.IsSelectable && !selectables.Contains(this.selectable))
			{
				selectables.Add(this.selectable);
			}
		}

		public void Clear()
		{
			this.statusItems.Clear();
			this.offset = Vector3.zero;
			this.dirty = false;
		}

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

		public void MarkDirty()
		{
			this.dirty = true;
		}

		public int handle;

		public Transform transform;

		public Building building;

		public Vector3 buildingPos;

		public KSelectable selectable;

		public List<StatusItem> statusItems;

		public Mesh mesh;

		public bool dirty;

		public int layer;

		public Material material;

		public Vector3 offset;

		public bool hasVisibleStatusItems;

		public bool isBuilding;

		private const int STATUS_ICONS_LIMIT = 12;

		public static List<Sprite> spritesListedToRender = new List<Sprite>(12);

		public static List<int> statusItemsToRender_Index = new List<int>(12);

		private struct MeshBuilder
		{
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

			private Vector3[] vertices;

			private Vector2[] uvs;

			private Vector2[] uv2s;

			private int[] triangles;

			private Color32[] colors;

			private int quadIdx;

			private Material material;

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
