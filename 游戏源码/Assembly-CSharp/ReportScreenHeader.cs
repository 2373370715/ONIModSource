using System;
using UnityEngine;

// Token: 0x02001EC8 RID: 7880
[AddComponentMenu("KMonoBehaviour/scripts/ReportScreenHeader")]
public class ReportScreenHeader : KMonoBehaviour
{
	// Token: 0x0600A59A RID: 42394 RVA: 0x0010B7DA File Offset: 0x001099DA
	public void SetMainEntry(ReportManager.ReportGroup reportGroup)
	{
		if (this.mainRow == null)
		{
			this.mainRow = Util.KInstantiateUI(this.rowTemplate.gameObject, base.gameObject, true).GetComponent<ReportScreenHeaderRow>();
		}
		this.mainRow.SetLine(reportGroup);
	}

	// Token: 0x040081C3 RID: 33219
	[SerializeField]
	private ReportScreenHeaderRow rowTemplate;

	// Token: 0x040081C4 RID: 33220
	private ReportScreenHeaderRow mainRow;
}
