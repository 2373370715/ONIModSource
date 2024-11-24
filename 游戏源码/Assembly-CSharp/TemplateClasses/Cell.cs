using System;

namespace TemplateClasses
{
	// Token: 0x020020E6 RID: 8422
	[Serializable]
	public class Cell
	{
		// Token: 0x0600B335 RID: 45877 RVA: 0x000A5E2C File Offset: 0x000A402C
		public Cell()
		{
		}

		// Token: 0x0600B336 RID: 45878 RVA: 0x0043A3F8 File Offset: 0x004385F8
		public Cell(int loc_x, int loc_y, SimHashes _element, float _temperature, float _mass, string _diseaseName, int _diseaseCount, bool _preventFoWReveal = false)
		{
			this.location_x = loc_x;
			this.location_y = loc_y;
			this.element = _element;
			this.temperature = _temperature;
			this.mass = _mass;
			this.diseaseName = _diseaseName;
			this.diseaseCount = _diseaseCount;
			this.preventFoWReveal = _preventFoWReveal;
		}

		// Token: 0x17000B94 RID: 2964
		// (get) Token: 0x0600B337 RID: 45879 RVA: 0x001143F2 File Offset: 0x001125F2
		// (set) Token: 0x0600B338 RID: 45880 RVA: 0x001143FA File Offset: 0x001125FA
		public SimHashes element { get; set; }

		// Token: 0x17000B95 RID: 2965
		// (get) Token: 0x0600B339 RID: 45881 RVA: 0x00114403 File Offset: 0x00112603
		// (set) Token: 0x0600B33A RID: 45882 RVA: 0x0011440B File Offset: 0x0011260B
		public float mass { get; set; }

		// Token: 0x17000B96 RID: 2966
		// (get) Token: 0x0600B33B RID: 45883 RVA: 0x00114414 File Offset: 0x00112614
		// (set) Token: 0x0600B33C RID: 45884 RVA: 0x0011441C File Offset: 0x0011261C
		public float temperature { get; set; }

		// Token: 0x17000B97 RID: 2967
		// (get) Token: 0x0600B33D RID: 45885 RVA: 0x00114425 File Offset: 0x00112625
		// (set) Token: 0x0600B33E RID: 45886 RVA: 0x0011442D File Offset: 0x0011262D
		public string diseaseName { get; set; }

		// Token: 0x17000B98 RID: 2968
		// (get) Token: 0x0600B33F RID: 45887 RVA: 0x00114436 File Offset: 0x00112636
		// (set) Token: 0x0600B340 RID: 45888 RVA: 0x0011443E File Offset: 0x0011263E
		public int diseaseCount { get; set; }

		// Token: 0x17000B99 RID: 2969
		// (get) Token: 0x0600B341 RID: 45889 RVA: 0x00114447 File Offset: 0x00112647
		// (set) Token: 0x0600B342 RID: 45890 RVA: 0x0011444F File Offset: 0x0011264F
		public int location_x { get; set; }

		// Token: 0x17000B9A RID: 2970
		// (get) Token: 0x0600B343 RID: 45891 RVA: 0x00114458 File Offset: 0x00112658
		// (set) Token: 0x0600B344 RID: 45892 RVA: 0x00114460 File Offset: 0x00112660
		public int location_y { get; set; }

		// Token: 0x17000B9B RID: 2971
		// (get) Token: 0x0600B345 RID: 45893 RVA: 0x00114469 File Offset: 0x00112669
		// (set) Token: 0x0600B346 RID: 45894 RVA: 0x00114471 File Offset: 0x00112671
		public bool preventFoWReveal { get; set; }
	}
}
