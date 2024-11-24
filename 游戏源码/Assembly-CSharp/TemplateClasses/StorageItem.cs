using System;

namespace TemplateClasses
{
	// Token: 0x020020E7 RID: 8423
	[Serializable]
	public class StorageItem
	{
		// Token: 0x0600B347 RID: 45895 RVA: 0x0011447A File Offset: 0x0011267A
		public StorageItem()
		{
			this.rottable = new Rottable();
		}

		// Token: 0x0600B348 RID: 45896 RVA: 0x0043A448 File Offset: 0x00438648
		public StorageItem(string _id, float _units, float _temp, SimHashes _element, string _disease, int _disease_count, bool _isOre)
		{
			this.rottable = new Rottable();
			this.id = _id;
			this.element = _element;
			this.units = _units;
			this.diseaseName = _disease;
			this.diseaseCount = _disease_count;
			this.isOre = _isOre;
			this.temperature = _temp;
		}

		// Token: 0x17000B9C RID: 2972
		// (get) Token: 0x0600B349 RID: 45897 RVA: 0x0011448D File Offset: 0x0011268D
		// (set) Token: 0x0600B34A RID: 45898 RVA: 0x00114495 File Offset: 0x00112695
		public string id { get; set; }

		// Token: 0x17000B9D RID: 2973
		// (get) Token: 0x0600B34B RID: 45899 RVA: 0x0011449E File Offset: 0x0011269E
		// (set) Token: 0x0600B34C RID: 45900 RVA: 0x001144A6 File Offset: 0x001126A6
		public SimHashes element { get; set; }

		// Token: 0x17000B9E RID: 2974
		// (get) Token: 0x0600B34D RID: 45901 RVA: 0x001144AF File Offset: 0x001126AF
		// (set) Token: 0x0600B34E RID: 45902 RVA: 0x001144B7 File Offset: 0x001126B7
		public float units { get; set; }

		// Token: 0x17000B9F RID: 2975
		// (get) Token: 0x0600B34F RID: 45903 RVA: 0x001144C0 File Offset: 0x001126C0
		// (set) Token: 0x0600B350 RID: 45904 RVA: 0x001144C8 File Offset: 0x001126C8
		public bool isOre { get; set; }

		// Token: 0x17000BA0 RID: 2976
		// (get) Token: 0x0600B351 RID: 45905 RVA: 0x001144D1 File Offset: 0x001126D1
		// (set) Token: 0x0600B352 RID: 45906 RVA: 0x001144D9 File Offset: 0x001126D9
		public float temperature { get; set; }

		// Token: 0x17000BA1 RID: 2977
		// (get) Token: 0x0600B353 RID: 45907 RVA: 0x001144E2 File Offset: 0x001126E2
		// (set) Token: 0x0600B354 RID: 45908 RVA: 0x001144EA File Offset: 0x001126EA
		public string diseaseName { get; set; }

		// Token: 0x17000BA2 RID: 2978
		// (get) Token: 0x0600B355 RID: 45909 RVA: 0x001144F3 File Offset: 0x001126F3
		// (set) Token: 0x0600B356 RID: 45910 RVA: 0x001144FB File Offset: 0x001126FB
		public int diseaseCount { get; set; }

		// Token: 0x17000BA3 RID: 2979
		// (get) Token: 0x0600B357 RID: 45911 RVA: 0x00114504 File Offset: 0x00112704
		// (set) Token: 0x0600B358 RID: 45912 RVA: 0x0011450C File Offset: 0x0011270C
		public Rottable rottable { get; set; }

		// Token: 0x0600B359 RID: 45913 RVA: 0x0043A49C File Offset: 0x0043869C
		public StorageItem Clone()
		{
			return new StorageItem(this.id, this.units, this.temperature, this.element, this.diseaseName, this.diseaseCount, this.isOre)
			{
				rottable = 
				{
					rotAmount = this.rottable.rotAmount
				}
			};
		}
	}
}
