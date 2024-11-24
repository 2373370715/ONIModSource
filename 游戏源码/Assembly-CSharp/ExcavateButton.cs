using System;
using STRINGS;

public class ExcavateButton : KMonoBehaviour, ISidescreenButtonControl
{
	public Func<bool> isMarkedForDig;

	public System.Action OnButtonPressed;

	public string SidescreenButtonText
	{
		get
		{
			if (isMarkedForDig == null || !isMarkedForDig())
			{
				return CODEX.STORY_TRAITS.FOSSILHUNT.UISIDESCREENS.DIG_SITE_EXCAVATE_BUTTON;
			}
			return CODEX.STORY_TRAITS.FOSSILHUNT.UISIDESCREENS.DIG_SITE_CANCEL_EXCAVATION_BUTTON;
		}
	}

	public string SidescreenButtonTooltip
	{
		get
		{
			if (isMarkedForDig == null || !isMarkedForDig())
			{
				return CODEX.STORY_TRAITS.FOSSILHUNT.UISIDESCREENS.DIG_SITE_EXCAVATE_BUTTON_TOOLTIP;
			}
			return CODEX.STORY_TRAITS.FOSSILHUNT.UISIDESCREENS.DIG_SITE_CANCEL_EXCAVATION_BUTTON_TOOLTIP;
		}
	}

	public int HorizontalGroupID()
	{
		return -1;
	}

	public void SetButtonTextOverride(ButtonMenuTextOverride textOverride)
	{
		throw new NotImplementedException();
	}

	public bool SidescreenEnabled()
	{
		return true;
	}

	public bool SidescreenButtonInteractable()
	{
		return true;
	}

	public void OnSidescreenButtonPressed()
	{
		OnButtonPressed?.Invoke();
	}

	public int ButtonSideScreenSortOrder()
	{
		return 20;
	}
}
