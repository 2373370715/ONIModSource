using System;

[Serializable]
public struct ButtonMenuTextOverride
{
	public LocString Text;

	public LocString CancelText;

	public LocString ToolTip;

	public LocString CancelToolTip;

	public bool IsValid
	{
		get
		{
			if (!string.IsNullOrEmpty(Text))
			{
				return !string.IsNullOrEmpty(ToolTip);
			}
			return false;
		}
	}

	public bool HasCancelText
	{
		get
		{
			if (!string.IsNullOrEmpty(CancelText))
			{
				return !string.IsNullOrEmpty(CancelToolTip);
			}
			return false;
		}
	}
}
