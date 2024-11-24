using System;
using System.Collections.Generic;

namespace TemplateClasses
{
	// Token: 0x020020E3 RID: 8419
	[Serializable]
	public class Prefab
	{
		// Token: 0x0600B30B RID: 45835 RVA: 0x0011427A File Offset: 0x0011247A
		public Prefab()
		{
			this.type = Prefab.Type.Other;
		}

		// Token: 0x0600B30C RID: 45836 RVA: 0x0043A254 File Offset: 0x00438454
		public Prefab(string _id, Prefab.Type _type, int loc_x, int loc_y, SimHashes _element, float _temperature = -1f, float _units = 1f, string _disease = null, int _disease_count = 0, Orientation _rotation = Orientation.Neutral, Prefab.template_amount_value[] _amount_values = null, Prefab.template_amount_value[] _other_values = null, int _connections = 0, string facadeIdId = null)
		{
			this.id = _id;
			this.type = _type;
			this.location_x = loc_x;
			this.location_y = loc_y;
			this.connections = _connections;
			this.element = _element;
			this.temperature = _temperature;
			this.units = _units;
			this.diseaseName = _disease;
			this.diseaseCount = _disease_count;
			this.facadeId = facadeIdId;
			this.rotationOrientation = _rotation;
			if (_amount_values != null && _amount_values.Length != 0)
			{
				this.amounts = _amount_values;
			}
			if (_other_values != null && _other_values.Length != 0)
			{
				this.other_values = _other_values;
			}
		}

		// Token: 0x0600B30D RID: 45837 RVA: 0x0043A2E8 File Offset: 0x004384E8
		public Prefab Clone(Vector2I offset)
		{
			Prefab prefab = new Prefab(this.id, this.type, offset.x + this.location_x, offset.y + this.location_y, this.element, this.temperature, this.units, this.diseaseName, this.diseaseCount, this.rotationOrientation, this.amounts, this.other_values, this.connections, this.facadeId);
			if (this.rottable != null)
			{
				prefab.rottable = new Rottable();
				prefab.rottable.rotAmount = this.rottable.rotAmount;
			}
			if (this.storage != null && this.storage.Count > 0)
			{
				prefab.storage = new List<StorageItem>();
				foreach (StorageItem storageItem in this.storage)
				{
					prefab.storage.Add(storageItem.Clone());
				}
			}
			return prefab;
		}

		// Token: 0x0600B30E RID: 45838 RVA: 0x00114289 File Offset: 0x00112489
		public void AssignStorage(StorageItem _storage)
		{
			if (this.storage == null)
			{
				this.storage = new List<StorageItem>();
			}
			this.storage.Add(_storage);
		}

		// Token: 0x17000B82 RID: 2946
		// (get) Token: 0x0600B30F RID: 45839 RVA: 0x001142AA File Offset: 0x001124AA
		// (set) Token: 0x0600B310 RID: 45840 RVA: 0x001142B2 File Offset: 0x001124B2
		public string id { get; set; }

		// Token: 0x17000B83 RID: 2947
		// (get) Token: 0x0600B311 RID: 45841 RVA: 0x001142BB File Offset: 0x001124BB
		// (set) Token: 0x0600B312 RID: 45842 RVA: 0x001142C3 File Offset: 0x001124C3
		public int location_x { get; set; }

		// Token: 0x17000B84 RID: 2948
		// (get) Token: 0x0600B313 RID: 45843 RVA: 0x001142CC File Offset: 0x001124CC
		// (set) Token: 0x0600B314 RID: 45844 RVA: 0x001142D4 File Offset: 0x001124D4
		public int location_y { get; set; }

		// Token: 0x17000B85 RID: 2949
		// (get) Token: 0x0600B315 RID: 45845 RVA: 0x001142DD File Offset: 0x001124DD
		// (set) Token: 0x0600B316 RID: 45846 RVA: 0x001142E5 File Offset: 0x001124E5
		public SimHashes element { get; set; }

		// Token: 0x17000B86 RID: 2950
		// (get) Token: 0x0600B317 RID: 45847 RVA: 0x001142EE File Offset: 0x001124EE
		// (set) Token: 0x0600B318 RID: 45848 RVA: 0x001142F6 File Offset: 0x001124F6
		public float temperature { get; set; }

		// Token: 0x17000B87 RID: 2951
		// (get) Token: 0x0600B319 RID: 45849 RVA: 0x001142FF File Offset: 0x001124FF
		// (set) Token: 0x0600B31A RID: 45850 RVA: 0x00114307 File Offset: 0x00112507
		public float units { get; set; }

		// Token: 0x17000B88 RID: 2952
		// (get) Token: 0x0600B31B RID: 45851 RVA: 0x00114310 File Offset: 0x00112510
		// (set) Token: 0x0600B31C RID: 45852 RVA: 0x00114318 File Offset: 0x00112518
		public string diseaseName { get; set; }

		// Token: 0x17000B89 RID: 2953
		// (get) Token: 0x0600B31D RID: 45853 RVA: 0x00114321 File Offset: 0x00112521
		// (set) Token: 0x0600B31E RID: 45854 RVA: 0x00114329 File Offset: 0x00112529
		public int diseaseCount { get; set; }

		// Token: 0x17000B8A RID: 2954
		// (get) Token: 0x0600B31F RID: 45855 RVA: 0x00114332 File Offset: 0x00112532
		// (set) Token: 0x0600B320 RID: 45856 RVA: 0x0011433A File Offset: 0x0011253A
		public Orientation rotationOrientation { get; set; }

		// Token: 0x17000B8B RID: 2955
		// (get) Token: 0x0600B321 RID: 45857 RVA: 0x00114343 File Offset: 0x00112543
		// (set) Token: 0x0600B322 RID: 45858 RVA: 0x0011434B File Offset: 0x0011254B
		public List<StorageItem> storage { get; set; }

		// Token: 0x17000B8C RID: 2956
		// (get) Token: 0x0600B323 RID: 45859 RVA: 0x00114354 File Offset: 0x00112554
		// (set) Token: 0x0600B324 RID: 45860 RVA: 0x0011435C File Offset: 0x0011255C
		public Prefab.Type type { get; set; }

		// Token: 0x17000B8D RID: 2957
		// (get) Token: 0x0600B325 RID: 45861 RVA: 0x00114365 File Offset: 0x00112565
		// (set) Token: 0x0600B326 RID: 45862 RVA: 0x0011436D File Offset: 0x0011256D
		public string facadeId { get; set; }

		// Token: 0x17000B8E RID: 2958
		// (get) Token: 0x0600B327 RID: 45863 RVA: 0x00114376 File Offset: 0x00112576
		// (set) Token: 0x0600B328 RID: 45864 RVA: 0x0011437E File Offset: 0x0011257E
		public int connections { get; set; }

		// Token: 0x17000B8F RID: 2959
		// (get) Token: 0x0600B329 RID: 45865 RVA: 0x00114387 File Offset: 0x00112587
		// (set) Token: 0x0600B32A RID: 45866 RVA: 0x0011438F File Offset: 0x0011258F
		public Rottable rottable { get; set; }

		// Token: 0x17000B90 RID: 2960
		// (get) Token: 0x0600B32B RID: 45867 RVA: 0x00114398 File Offset: 0x00112598
		// (set) Token: 0x0600B32C RID: 45868 RVA: 0x001143A0 File Offset: 0x001125A0
		public Prefab.template_amount_value[] amounts { get; set; }

		// Token: 0x17000B91 RID: 2961
		// (get) Token: 0x0600B32D RID: 45869 RVA: 0x001143A9 File Offset: 0x001125A9
		// (set) Token: 0x0600B32E RID: 45870 RVA: 0x001143B1 File Offset: 0x001125B1
		public Prefab.template_amount_value[] other_values { get; set; }

		// Token: 0x020020E4 RID: 8420
		public enum Type
		{
			// Token: 0x04008DA3 RID: 36259
			Building,
			// Token: 0x04008DA4 RID: 36260
			Ore,
			// Token: 0x04008DA5 RID: 36261
			Pickupable,
			// Token: 0x04008DA6 RID: 36262
			Other
		}

		// Token: 0x020020E5 RID: 8421
		[Serializable]
		public class template_amount_value
		{
			// Token: 0x0600B32F RID: 45871 RVA: 0x000A5E2C File Offset: 0x000A402C
			public template_amount_value()
			{
			}

			// Token: 0x0600B330 RID: 45872 RVA: 0x001143BA File Offset: 0x001125BA
			public template_amount_value(string id, float value)
			{
				this.id = id;
				this.value = value;
			}

			// Token: 0x17000B92 RID: 2962
			// (get) Token: 0x0600B331 RID: 45873 RVA: 0x001143D0 File Offset: 0x001125D0
			// (set) Token: 0x0600B332 RID: 45874 RVA: 0x001143D8 File Offset: 0x001125D8
			public string id { get; set; }

			// Token: 0x17000B93 RID: 2963
			// (get) Token: 0x0600B333 RID: 45875 RVA: 0x001143E1 File Offset: 0x001125E1
			// (set) Token: 0x0600B334 RID: 45876 RVA: 0x001143E9 File Offset: 0x001125E9
			public float value { get; set; }
		}
	}
}
