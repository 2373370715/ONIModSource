using System;
using STRINGS;

// Token: 0x020018D6 RID: 6358
public class ClustercraftInteriorDoor : KMonoBehaviour, ISidescreenButtonControl
{
	// Token: 0x1700088F RID: 2191
	// (get) Token: 0x06008421 RID: 33825 RVA: 0x000F6D77 File Offset: 0x000F4F77
	public string SidescreenButtonText
	{
		get
		{
			return UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.BUTTONVIEWEXTERIOR.LABEL;
		}
	}

	// Token: 0x17000890 RID: 2192
	// (get) Token: 0x06008422 RID: 33826 RVA: 0x000F6D83 File Offset: 0x000F4F83
	public string SidescreenButtonTooltip
	{
		get
		{
			return this.SidescreenButtonInteractable() ? UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.BUTTONVIEWEXTERIOR.LABEL : UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.BUTTONVIEWEXTERIOR.INVALID;
		}
	}

	// Token: 0x06008423 RID: 33827 RVA: 0x000F6D9E File Offset: 0x000F4F9E
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Components.ClusterCraftInteriorDoors.Add(this);
	}

	// Token: 0x06008424 RID: 33828 RVA: 0x000F6DB1 File Offset: 0x000F4FB1
	protected override void OnCleanUp()
	{
		Components.ClusterCraftInteriorDoors.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x06008425 RID: 33829 RVA: 0x000A65EC File Offset: 0x000A47EC
	public bool SidescreenEnabled()
	{
		return true;
	}

	// Token: 0x06008426 RID: 33830 RVA: 0x0034244C File Offset: 0x0034064C
	public bool SidescreenButtonInteractable()
	{
		WorldContainer myWorld = base.gameObject.GetMyWorld();
		return myWorld.ParentWorldId != 255 && myWorld.ParentWorldId != myWorld.id;
	}

	// Token: 0x06008427 RID: 33831 RVA: 0x000F6DC4 File Offset: 0x000F4FC4
	public void OnSidescreenButtonPressed()
	{
		ClusterManager.Instance.SetActiveWorld(base.gameObject.GetMyWorld().ParentWorldId);
	}

	// Token: 0x06008428 RID: 33832 RVA: 0x000ABCBD File Offset: 0x000A9EBD
	public int ButtonSideScreenSortOrder()
	{
		return 20;
	}

	// Token: 0x06008429 RID: 33833 RVA: 0x000ABCB6 File Offset: 0x000A9EB6
	public void SetButtonTextOverride(ButtonMenuTextOverride text)
	{
		throw new NotImplementedException();
	}

	// Token: 0x0600842A RID: 33834 RVA: 0x000ABC75 File Offset: 0x000A9E75
	public int HorizontalGroupID()
	{
		return -1;
	}
}
