using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02002066 RID: 8294
public class OptionSelector : MonoBehaviour
{
	// Token: 0x0600B080 RID: 45184 RVA: 0x00112C4C File Offset: 0x00110E4C
	private void Start()
	{
		this.selectedItem.GetComponent<KButton>().onBtnClick += this.OnClick;
	}

	// Token: 0x0600B081 RID: 45185 RVA: 0x00112C6A File Offset: 0x00110E6A
	public void Initialize(object id)
	{
		this.id = id;
	}

	// Token: 0x0600B082 RID: 45186 RVA: 0x00112C73 File Offset: 0x00110E73
	private void OnClick(KKeyCode button)
	{
		if (button == KKeyCode.Mouse0)
		{
			this.OnChangePriority(this.id, 1);
			return;
		}
		if (button != KKeyCode.Mouse1)
		{
			return;
		}
		this.OnChangePriority(this.id, -1);
	}

	// Token: 0x0600B083 RID: 45187 RVA: 0x004252F4 File Offset: 0x004234F4
	public void ConfigureItem(bool disabled, OptionSelector.DisplayOptionInfo display_info)
	{
		HierarchyReferences component = this.selectedItem.GetComponent<HierarchyReferences>();
		KImage kimage = component.GetReference("BG") as KImage;
		if (display_info.bgOptions == null)
		{
			kimage.gameObject.SetActive(false);
		}
		else
		{
			kimage.sprite = display_info.bgOptions[display_info.bgIndex];
		}
		KImage kimage2 = component.GetReference("FG") as KImage;
		if (display_info.fgOptions == null)
		{
			kimage2.gameObject.SetActive(false);
		}
		else
		{
			kimage2.sprite = display_info.fgOptions[display_info.fgIndex];
		}
		KImage kimage3 = component.GetReference("Fill") as KImage;
		if (kimage3 != null)
		{
			kimage3.enabled = !disabled;
			kimage3.color = display_info.fillColour;
		}
		KImage kimage4 = component.GetReference("Outline") as KImage;
		if (kimage4 != null)
		{
			kimage4.enabled = !disabled;
		}
	}

	// Token: 0x04008B76 RID: 35702
	private object id;

	// Token: 0x04008B77 RID: 35703
	public Action<object, int> OnChangePriority;

	// Token: 0x04008B78 RID: 35704
	[SerializeField]
	private KImage selectedItem;

	// Token: 0x04008B79 RID: 35705
	[SerializeField]
	private KImage itemTemplate;

	// Token: 0x02002067 RID: 8295
	public class DisplayOptionInfo
	{
		// Token: 0x04008B7A RID: 35706
		public IList<Sprite> bgOptions;

		// Token: 0x04008B7B RID: 35707
		public IList<Sprite> fgOptions;

		// Token: 0x04008B7C RID: 35708
		public int bgIndex;

		// Token: 0x04008B7D RID: 35709
		public int fgIndex;

		// Token: 0x04008B7E RID: 35710
		public Color32 fillColour;
	}
}
