using System;

// Token: 0x02001F3D RID: 7997
public interface ICheckboxListGroupControl
{
	// Token: 0x17000ACC RID: 2764
	// (get) Token: 0x0600A8D5 RID: 43221
	string Title { get; }

	// Token: 0x17000ACD RID: 2765
	// (get) Token: 0x0600A8D6 RID: 43222
	string Description { get; }

	// Token: 0x0600A8D7 RID: 43223
	ICheckboxListGroupControl.ListGroup[] GetData();

	// Token: 0x0600A8D8 RID: 43224
	bool SidescreenEnabled();

	// Token: 0x0600A8D9 RID: 43225
	int CheckboxSideScreenSortOrder();

	// Token: 0x02001F3E RID: 7998
	public struct ListGroup
	{
		// Token: 0x0600A8DA RID: 43226 RVA: 0x0010DA6A File Offset: 0x0010BC6A
		public ListGroup(string title, ICheckboxListGroupControl.CheckboxItem[] checkboxItems, Func<string, string> resolveTitleCallback = null, System.Action onItemClicked = null)
		{
			this.title = title;
			this.checkboxItems = checkboxItems;
			this.resolveTitleCallback = resolveTitleCallback;
			this.onItemClicked = onItemClicked;
		}

		// Token: 0x040084B4 RID: 33972
		public Func<string, string> resolveTitleCallback;

		// Token: 0x040084B5 RID: 33973
		public System.Action onItemClicked;

		// Token: 0x040084B6 RID: 33974
		public string title;

		// Token: 0x040084B7 RID: 33975
		public ICheckboxListGroupControl.CheckboxItem[] checkboxItems;
	}

	// Token: 0x02001F3F RID: 7999
	public struct CheckboxItem
	{
		// Token: 0x040084B8 RID: 33976
		public string text;

		// Token: 0x040084B9 RID: 33977
		public string tooltip;

		// Token: 0x040084BA RID: 33978
		public bool isOn;

		// Token: 0x040084BB RID: 33979
		public Func<string, bool> overrideLinkActions;

		// Token: 0x040084BC RID: 33980
		public Func<string, object, string> resolveTooltipCallback;
	}
}
