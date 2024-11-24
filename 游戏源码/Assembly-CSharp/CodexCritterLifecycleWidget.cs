using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001C67 RID: 7271
public class CodexCritterLifecycleWidget : CodexWidget<CodexCritterLifecycleWidget>
{
	// Token: 0x06009787 RID: 38791 RVA: 0x001027BC File Offset: 0x001009BC
	private CodexCritterLifecycleWidget()
	{
	}

	// Token: 0x06009788 RID: 38792 RVA: 0x003AC05C File Offset: 0x003AA25C
	public override void Configure(GameObject contentGameObject, Transform displayPane, Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
	{
		HierarchyReferences component = contentGameObject.GetComponent<HierarchyReferences>();
		component.GetReference<Image>("EggIcon").sprite = null;
		component.GetReference<Image>("EggIcon").color = Color.white;
		component.GetReference<LocText>("EggToBabyLabel").text = "";
		component.GetReference<Image>("BabyIcon").sprite = null;
		component.GetReference<Image>("BabyIcon").color = Color.white;
		component.GetReference<LocText>("BabyToAdultLabel").text = "";
		component.GetReference<Image>("AdultIcon").sprite = null;
		component.GetReference<Image>("AdultIcon").color = Color.white;
	}
}
