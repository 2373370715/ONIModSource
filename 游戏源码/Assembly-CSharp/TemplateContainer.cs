using System;
using System.Collections.Generic;
using System.IO;
using Klei;
using TemplateClasses;
using UnityEngine;

// Token: 0x02001210 RID: 4624
[Serializable]
public class TemplateContainer
{
	// Token: 0x170005A6 RID: 1446
	// (get) Token: 0x06005E72 RID: 24178 RVA: 0x000DDB29 File Offset: 0x000DBD29
	// (set) Token: 0x06005E73 RID: 24179 RVA: 0x000DDB31 File Offset: 0x000DBD31
	public string name { get; set; }

	// Token: 0x170005A7 RID: 1447
	// (get) Token: 0x06005E74 RID: 24180 RVA: 0x000DDB3A File Offset: 0x000DBD3A
	// (set) Token: 0x06005E75 RID: 24181 RVA: 0x000DDB42 File Offset: 0x000DBD42
	public int priority { get; set; }

	// Token: 0x170005A8 RID: 1448
	// (get) Token: 0x06005E76 RID: 24182 RVA: 0x000DDB4B File Offset: 0x000DBD4B
	// (set) Token: 0x06005E77 RID: 24183 RVA: 0x000DDB53 File Offset: 0x000DBD53
	public TemplateContainer.Info info { get; set; }

	// Token: 0x170005A9 RID: 1449
	// (get) Token: 0x06005E78 RID: 24184 RVA: 0x000DDB5C File Offset: 0x000DBD5C
	// (set) Token: 0x06005E79 RID: 24185 RVA: 0x000DDB64 File Offset: 0x000DBD64
	public List<Cell> cells { get; set; }

	// Token: 0x170005AA RID: 1450
	// (get) Token: 0x06005E7A RID: 24186 RVA: 0x000DDB6D File Offset: 0x000DBD6D
	// (set) Token: 0x06005E7B RID: 24187 RVA: 0x000DDB75 File Offset: 0x000DBD75
	public List<Prefab> buildings { get; set; }

	// Token: 0x170005AB RID: 1451
	// (get) Token: 0x06005E7C RID: 24188 RVA: 0x000DDB7E File Offset: 0x000DBD7E
	// (set) Token: 0x06005E7D RID: 24189 RVA: 0x000DDB86 File Offset: 0x000DBD86
	public List<Prefab> pickupables { get; set; }

	// Token: 0x170005AC RID: 1452
	// (get) Token: 0x06005E7E RID: 24190 RVA: 0x000DDB8F File Offset: 0x000DBD8F
	// (set) Token: 0x06005E7F RID: 24191 RVA: 0x000DDB97 File Offset: 0x000DBD97
	public List<Prefab> elementalOres { get; set; }

	// Token: 0x170005AD RID: 1453
	// (get) Token: 0x06005E80 RID: 24192 RVA: 0x000DDBA0 File Offset: 0x000DBDA0
	// (set) Token: 0x06005E81 RID: 24193 RVA: 0x000DDBA8 File Offset: 0x000DBDA8
	public List<Prefab> otherEntities { get; set; }

	// Token: 0x06005E82 RID: 24194 RVA: 0x002A3990 File Offset: 0x002A1B90
	public void Init(List<Cell> _cells, List<Prefab> _buildings, List<Prefab> _pickupables, List<Prefab> _elementalOres, List<Prefab> _otherEntities)
	{
		if (_cells != null && _cells.Count > 0)
		{
			this.cells = _cells;
		}
		if (_buildings != null && _buildings.Count > 0)
		{
			this.buildings = _buildings;
		}
		if (_pickupables != null && _pickupables.Count > 0)
		{
			this.pickupables = _pickupables;
		}
		if (_elementalOres != null && _elementalOres.Count > 0)
		{
			this.elementalOres = _elementalOres;
		}
		if (_otherEntities != null && _otherEntities.Count > 0)
		{
			this.otherEntities = _otherEntities;
		}
		this.info = new TemplateContainer.Info();
		this.RefreshInfo();
	}

	// Token: 0x06005E83 RID: 24195 RVA: 0x000DDBB1 File Offset: 0x000DBDB1
	public RectInt GetTemplateBounds(int padding = 0)
	{
		return this.GetTemplateBounds(Vector2I.zero, padding);
	}

	// Token: 0x06005E84 RID: 24196 RVA: 0x000DDBBF File Offset: 0x000DBDBF
	public RectInt GetTemplateBounds(Vector2 position, int padding = 0)
	{
		return this.GetTemplateBounds(new Vector2I((int)position.x, (int)position.y), padding);
	}

	// Token: 0x06005E85 RID: 24197 RVA: 0x002A3A14 File Offset: 0x002A1C14
	public RectInt GetTemplateBounds(Vector2I position, int padding = 0)
	{
		if ((this.info.min - new Vector2f(0, 0)).sqrMagnitude <= 1E-06f)
		{
			this.RefreshInfo();
		}
		return this.info.GetBounds(position, padding);
	}

	// Token: 0x06005E86 RID: 24198 RVA: 0x002A3A5C File Offset: 0x002A1C5C
	public void RefreshInfo()
	{
		if (this.cells == null)
		{
			return;
		}
		int num = 1;
		int num2 = -1;
		int num3 = 1;
		int num4 = -1;
		foreach (Cell cell in this.cells)
		{
			if (cell.location_x < num)
			{
				num = cell.location_x;
			}
			if (cell.location_x > num2)
			{
				num2 = cell.location_x;
			}
			if (cell.location_y < num3)
			{
				num3 = cell.location_y;
			}
			if (cell.location_y > num4)
			{
				num4 = cell.location_y;
			}
		}
		this.info.size = new Vector2((float)(1 + (num2 - num)), (float)(1 + (num4 - num3)));
		this.info.min = new Vector2((float)num, (float)num3);
		this.info.area = this.cells.Count;
	}

	// Token: 0x06005E87 RID: 24199 RVA: 0x002A3B54 File Offset: 0x002A1D54
	public void SaveToYaml(string save_name)
	{
		string text = TemplateCache.RewriteTemplatePath(save_name);
		if (!Directory.Exists(Path.GetDirectoryName(text)))
		{
			Directory.CreateDirectory(Path.GetDirectoryName(text));
		}
		YamlIO.Save<TemplateContainer>(this, text + ".yaml", null);
	}

	// Token: 0x02001211 RID: 4625
	[Serializable]
	public class Info
	{
		// Token: 0x170005AE RID: 1454
		// (get) Token: 0x06005E88 RID: 24200 RVA: 0x000DDBDB File Offset: 0x000DBDDB
		// (set) Token: 0x06005E89 RID: 24201 RVA: 0x000DDBE3 File Offset: 0x000DBDE3
		public Vector2f size { get; set; }

		// Token: 0x170005AF RID: 1455
		// (get) Token: 0x06005E8A RID: 24202 RVA: 0x000DDBEC File Offset: 0x000DBDEC
		// (set) Token: 0x06005E8B RID: 24203 RVA: 0x000DDBF4 File Offset: 0x000DBDF4
		public Vector2f min { get; set; }

		// Token: 0x170005B0 RID: 1456
		// (get) Token: 0x06005E8C RID: 24204 RVA: 0x000DDBFD File Offset: 0x000DBDFD
		// (set) Token: 0x06005E8D RID: 24205 RVA: 0x000DDC05 File Offset: 0x000DBE05
		public int area { get; set; }

		// Token: 0x170005B1 RID: 1457
		// (get) Token: 0x06005E8E RID: 24206 RVA: 0x000DDC0E File Offset: 0x000DBE0E
		// (set) Token: 0x06005E8F RID: 24207 RVA: 0x000DDC16 File Offset: 0x000DBE16
		public Tag[] tags { get; set; }

		// Token: 0x170005B2 RID: 1458
		// (get) Token: 0x06005E90 RID: 24208 RVA: 0x000DDC1F File Offset: 0x000DBE1F
		// (set) Token: 0x06005E91 RID: 24209 RVA: 0x000DDC27 File Offset: 0x000DBE27
		public Tag[] discover_tags { get; set; }

		// Token: 0x06005E92 RID: 24210 RVA: 0x002A3B94 File Offset: 0x002A1D94
		public RectInt GetBounds(Vector2I position, int padding)
		{
			return new RectInt(position.x + (int)this.min.x - padding, position.y + (int)this.min.y - padding, (int)this.size.x + padding * 2, (int)this.size.y + padding * 2);
		}
	}
}
