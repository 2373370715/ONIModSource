using System;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02003B1F RID: 15135
	public class Sicknesses : Modifications<Sickness, SicknessInstance>
	{
		// Token: 0x0600E903 RID: 59651 RVA: 0x0013BC62 File Offset: 0x00139E62
		public Sicknesses(GameObject go) : base(go, Db.Get().Sicknesses)
		{
		}

		// Token: 0x0600E904 RID: 59652 RVA: 0x004C42AC File Offset: 0x004C24AC
		public void Infect(SicknessExposureInfo exposure_info)
		{
			Sickness modifier = Db.Get().Sicknesses.Get(exposure_info.sicknessID);
			if (!base.Has(modifier))
			{
				this.CreateInstance(modifier).ExposureInfo = exposure_info;
			}
		}

		// Token: 0x0600E905 RID: 59653 RVA: 0x004C42E8 File Offset: 0x004C24E8
		public override SicknessInstance CreateInstance(Sickness sickness)
		{
			SicknessInstance sicknessInstance = new SicknessInstance(base.gameObject, sickness);
			this.Add(sicknessInstance);
			base.Trigger(GameHashes.SicknessAdded, sicknessInstance);
			ReportManager.Instance.ReportValue(ReportManager.ReportType.DiseaseAdded, 1f, base.gameObject.GetProperName(), null);
			return sicknessInstance;
		}

		// Token: 0x0600E906 RID: 59654 RVA: 0x0013BC75 File Offset: 0x00139E75
		public bool IsInfected()
		{
			return base.Count > 0;
		}

		// Token: 0x0600E907 RID: 59655 RVA: 0x0013BC80 File Offset: 0x00139E80
		public bool Cure(Sickness sickness)
		{
			return this.Cure(sickness.Id);
		}

		// Token: 0x0600E908 RID: 59656 RVA: 0x004C4334 File Offset: 0x004C2534
		public bool Cure(string sickness_id)
		{
			SicknessInstance sicknessInstance = null;
			foreach (SicknessInstance sicknessInstance2 in this)
			{
				if (sicknessInstance2.modifier.Id == sickness_id)
				{
					sicknessInstance = sicknessInstance2;
					break;
				}
			}
			if (sicknessInstance != null)
			{
				this.Remove(sicknessInstance);
				base.Trigger(GameHashes.SicknessCured, sicknessInstance);
				ReportManager.Instance.ReportValue(ReportManager.ReportType.DiseaseAdded, -1f, base.gameObject.GetProperName(), null);
				return true;
			}
			return false;
		}
	}
}
