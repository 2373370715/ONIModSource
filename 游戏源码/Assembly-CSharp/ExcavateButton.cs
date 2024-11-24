using System;
using STRINGS;

// Token: 0x02000404 RID: 1028
public class ExcavateButton : KMonoBehaviour, ISidescreenButtonControl
{
	// Token: 0x17000058 RID: 88
	// (get) Token: 0x0600115F RID: 4447 RVA: 0x000ADD43 File Offset: 0x000ABF43
	public string SidescreenButtonText
	{
		get
		{
			if (this.isMarkedForDig == null || !this.isMarkedForDig())
			{
				return CODEX.STORY_TRAITS.FOSSILHUNT.UISIDESCREENS.DIG_SITE_EXCAVATE_BUTTON;
			}
			return CODEX.STORY_TRAITS.FOSSILHUNT.UISIDESCREENS.DIG_SITE_CANCEL_EXCAVATION_BUTTON;
		}
	}

	// Token: 0x17000059 RID: 89
	// (get) Token: 0x06001160 RID: 4448 RVA: 0x000ADD6F File Offset: 0x000ABF6F
	public string SidescreenButtonTooltip
	{
		get
		{
			if (this.isMarkedForDig == null || !this.isMarkedForDig())
			{
				return CODEX.STORY_TRAITS.FOSSILHUNT.UISIDESCREENS.DIG_SITE_EXCAVATE_BUTTON_TOOLTIP;
			}
			return CODEX.STORY_TRAITS.FOSSILHUNT.UISIDESCREENS.DIG_SITE_CANCEL_EXCAVATION_BUTTON_TOOLTIP;
		}
	}

	// Token: 0x06001161 RID: 4449 RVA: 0x000ABC75 File Offset: 0x000A9E75
	public int HorizontalGroupID()
	{
		return -1;
	}

	// Token: 0x06001162 RID: 4450 RVA: 0x000ABCB6 File Offset: 0x000A9EB6
	public void SetButtonTextOverride(ButtonMenuTextOverride textOverride)
	{
		throw new NotImplementedException();
	}

	// Token: 0x06001163 RID: 4451 RVA: 0x000A65EC File Offset: 0x000A47EC
	public bool SidescreenEnabled()
	{
		return true;
	}

	// Token: 0x06001164 RID: 4452 RVA: 0x000A65EC File Offset: 0x000A47EC
	public bool SidescreenButtonInteractable()
	{
		return true;
	}

	// Token: 0x06001165 RID: 4453 RVA: 0x000ADD9B File Offset: 0x000ABF9B
	public void OnSidescreenButtonPressed()
	{
		System.Action onButtonPressed = this.OnButtonPressed;
		if (onButtonPressed == null)
		{
			return;
		}
		onButtonPressed();
	}

	// Token: 0x06001166 RID: 4454 RVA: 0x000ABCBD File Offset: 0x000A9EBD
	public int ButtonSideScreenSortOrder()
	{
		return 20;
	}

	// Token: 0x04000BD5 RID: 3029
	public Func<bool> isMarkedForDig;

	// Token: 0x04000BD6 RID: 3030
	public System.Action OnButtonPressed;
}
