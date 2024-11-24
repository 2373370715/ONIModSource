using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001C64 RID: 7268
public class CodexCollapsibleHeader : CodexWidget<CodexCollapsibleHeader>
{
	// Token: 0x17000A00 RID: 2560
	// (get) Token: 0x06009775 RID: 38773 RVA: 0x0010268F File Offset: 0x0010088F
	// (set) Token: 0x06009776 RID: 38774 RVA: 0x001026B6 File Offset: 0x001008B6
	protected GameObject ContentsGameObject
	{
		get
		{
			if (this.contentsGameObject == null)
			{
				this.contentsGameObject = this.contents.go;
			}
			return this.contentsGameObject;
		}
		set
		{
			this.contentsGameObject = value;
		}
	}

	// Token: 0x06009777 RID: 38775 RVA: 0x001026BF File Offset: 0x001008BF
	public CodexCollapsibleHeader(string label, ContentContainer contents)
	{
		this.label = label;
		this.contents = contents;
	}

	// Token: 0x06009778 RID: 38776 RVA: 0x003ABFE4 File Offset: 0x003AA1E4
	public override void Configure(GameObject contentGameObject, Transform displayPane, Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
	{
		HierarchyReferences component = contentGameObject.GetComponent<HierarchyReferences>();
		LocText reference = component.GetReference<LocText>("Label");
		reference.text = this.label;
		reference.textStyleSetting = textStyles[CodexTextStyle.Subtitle];
		reference.ApplySettings();
		MultiToggle reference2 = component.GetReference<MultiToggle>("ExpandToggle");
		reference2.ChangeState(1);
		reference2.onClick = delegate()
		{
			this.ToggleCategoryOpen(contentGameObject, !this.ContentsGameObject.activeSelf);
		};
	}

	// Token: 0x06009779 RID: 38777 RVA: 0x001026D5 File Offset: 0x001008D5
	private void ToggleCategoryOpen(GameObject header, bool open)
	{
		header.GetComponent<HierarchyReferences>().GetReference<MultiToggle>("ExpandToggle").ChangeState(open ? 1 : 0);
		this.ContentsGameObject.SetActive(open);
	}

	// Token: 0x04007591 RID: 30097
	protected ContentContainer contents;

	// Token: 0x04007592 RID: 30098
	private string label;

	// Token: 0x04007593 RID: 30099
	private GameObject contentsGameObject;
}
