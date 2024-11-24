using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001CE2 RID: 7394
[AddComponentMenu("KMonoBehaviour/scripts/GoodnessSlider")]
public class GoodnessSlider : KMonoBehaviour
{
	// Token: 0x06009A63 RID: 39523 RVA: 0x001046EF File Offset: 0x001028EF
	protected override void OnSpawn()
	{
		base.Spawn();
		this.UpdateValues();
	}

	// Token: 0x06009A64 RID: 39524 RVA: 0x003B8F64 File Offset: 0x003B7164
	public void UpdateValues()
	{
		this.text.color = (this.fill.color = this.gradient.Evaluate(this.slider.value));
		for (int i = 0; i < this.gradient.colorKeys.Length; i++)
		{
			if (this.gradient.colorKeys[i].time < this.slider.value)
			{
				this.text.text = this.names[i];
			}
			if (i == this.gradient.colorKeys.Length - 1 && this.gradient.colorKeys[i - 1].time < this.slider.value)
			{
				this.text.text = this.names[i];
			}
		}
	}

	// Token: 0x0400787E RID: 30846
	public Image icon;

	// Token: 0x0400787F RID: 30847
	public Text text;

	// Token: 0x04007880 RID: 30848
	public Slider slider;

	// Token: 0x04007881 RID: 30849
	public Image fill;

	// Token: 0x04007882 RID: 30850
	public Gradient gradient;

	// Token: 0x04007883 RID: 30851
	public string[] names;
}
