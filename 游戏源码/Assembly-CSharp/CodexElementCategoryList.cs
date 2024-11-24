using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001C68 RID: 7272
public class CodexElementCategoryList : CodexCollapsibleHeader
{
	// Token: 0x17000A05 RID: 2565
	// (get) Token: 0x06009789 RID: 38793 RVA: 0x001027C4 File Offset: 0x001009C4
	// (set) Token: 0x0600978A RID: 38794 RVA: 0x001027CC File Offset: 0x001009CC
	public Tag categoryTag { get; set; }

	// Token: 0x0600978B RID: 38795 RVA: 0x001027D5 File Offset: 0x001009D5
	public CodexElementCategoryList() : base(UI.CODEX.CATEGORYNAMES.ELEMENTS, null)
	{
	}

	// Token: 0x0600978C RID: 38796 RVA: 0x003AC10C File Offset: 0x003AA30C
	public override void Configure(GameObject contentGameObject, Transform displayPane, Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
	{
		HierarchyReferences component = contentGameObject.GetComponent<HierarchyReferences>();
		base.ContentsGameObject = component.GetReference<RectTransform>("ContentContainer").gameObject;
		base.Configure(contentGameObject, displayPane, textStyles);
		Component reference = component.GetReference<RectTransform>("HeaderLabel");
		RectTransform reference2 = component.GetReference<RectTransform>("PrefabLabelWithIcon");
		this.ClearPanel(reference2.transform.parent, reference2);
		reference.GetComponent<LocText>().SetText(UI.CODEX.CATEGORYNAMES.ELEMENTS);
		foreach (Element element in ElementLoader.elements)
		{
			if (element.HasTag(this.categoryTag) && !element.disabled)
			{
				GameObject gameObject = Util.KInstantiateUI(reference2.gameObject, reference2.parent.gameObject, true);
				Image componentInChildren = gameObject.GetComponentInChildren<Image>();
				global::Tuple<Sprite, Color> uisprite = Def.GetUISprite(element, "ui", false);
				componentInChildren.sprite = uisprite.first;
				componentInChildren.color = uisprite.second;
				gameObject.GetComponentInChildren<LocText>().SetText(element.tag.ProperName());
				this.rows.Add(gameObject);
			}
		}
	}

	// Token: 0x0600978D RID: 38797 RVA: 0x003AC244 File Offset: 0x003AA444
	private void ClearPanel(Transform containerToClear, Transform skipDestroyingPrefab)
	{
		skipDestroyingPrefab.SetAsFirstSibling();
		for (int i = containerToClear.childCount - 1; i >= 1; i--)
		{
			UnityEngine.Object.Destroy(containerToClear.GetChild(i).gameObject);
		}
		for (int j = this.rows.Count - 1; j >= 0; j--)
		{
			UnityEngine.Object.Destroy(this.rows[j].gameObject);
		}
		this.rows.Clear();
	}

	// Token: 0x0400759A RID: 30106
	private List<GameObject> rows = new List<GameObject>();
}
