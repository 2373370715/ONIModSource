using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001C9E RID: 7326
[AddComponentMenu("KMonoBehaviour/scripts/DescriptorPanel")]
public class DescriptorPanel : KMonoBehaviour
{
	// Token: 0x060098D5 RID: 39125 RVA: 0x00103796 File Offset: 0x00101996
	public bool HasDescriptors()
	{
		return this.labels.Count > 0;
	}

	// Token: 0x060098D6 RID: 39126 RVA: 0x003B1DA4 File Offset: 0x003AFFA4
	public void SetDescriptors(IList<Descriptor> descriptors)
	{
		int i;
		for (i = 0; i < descriptors.Count; i++)
		{
			GameObject gameObject;
			if (i >= this.labels.Count)
			{
				gameObject = Util.KInstantiate((this.customLabelPrefab != null) ? this.customLabelPrefab : ScreenPrefabs.Instance.DescriptionLabel, base.gameObject, null);
				gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
				this.labels.Add(gameObject);
			}
			else
			{
				gameObject = this.labels[i];
			}
			gameObject.GetComponent<LocText>().text = descriptors[i].IndentedText();
			gameObject.GetComponent<ToolTip>().toolTip = descriptors[i].tooltipText;
			gameObject.SetActive(true);
		}
		while (i < this.labels.Count)
		{
			this.labels[i].SetActive(false);
			i++;
		}
	}

	// Token: 0x04007716 RID: 30486
	[SerializeField]
	private GameObject customLabelPrefab;

	// Token: 0x04007717 RID: 30487
	private List<GameObject> labels = new List<GameObject>();
}
