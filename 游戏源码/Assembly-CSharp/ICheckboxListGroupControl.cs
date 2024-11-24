using System;

public interface ICheckboxListGroupControl
{
	public struct ListGroup
	{
		public Func<string, string> resolveTitleCallback;

		public System.Action onItemClicked;

		public string title;

		public CheckboxItem[] checkboxItems;

		public ListGroup(string title, CheckboxItem[] checkboxItems, Func<string, string> resolveTitleCallback = null, System.Action onItemClicked = null)
		{
			this.title = title;
			this.checkboxItems = checkboxItems;
			this.resolveTitleCallback = resolveTitleCallback;
			this.onItemClicked = onItemClicked;
		}
	}

	public struct CheckboxItem
	{
		public string text;

		public string tooltip;

		public bool isOn;

		public Func<string, bool> overrideLinkActions;

		public Func<string, object, string> resolveTooltipCallback;
	}

	string Title { get; }

	string Description { get; }

	ListGroup[] GetData();

	bool SidescreenEnabled();

	int CheckboxSideScreenSortOrder();
}
