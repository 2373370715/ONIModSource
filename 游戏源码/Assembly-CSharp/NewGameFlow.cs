using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001E4E RID: 7758
[AddComponentMenu("KMonoBehaviour/scripts/NewGameFlow")]
public class NewGameFlow : KMonoBehaviour
{
	// Token: 0x0600A28F RID: 41615 RVA: 0x001096DB File Offset: 0x001078DB
	public void BeginFlow()
	{
		this.currentScreenIndex = -1;
		this.Next();
	}

	// Token: 0x0600A290 RID: 41616 RVA: 0x001096EA File Offset: 0x001078EA
	private void Next()
	{
		this.ClearCurrentScreen();
		this.currentScreenIndex++;
		this.ActivateCurrentScreen();
	}

	// Token: 0x0600A291 RID: 41617 RVA: 0x00109706 File Offset: 0x00107906
	private void Previous()
	{
		this.ClearCurrentScreen();
		this.currentScreenIndex--;
		this.ActivateCurrentScreen();
	}

	// Token: 0x0600A292 RID: 41618 RVA: 0x00109722 File Offset: 0x00107922
	private void ClearCurrentScreen()
	{
		if (this.currentScreen != null)
		{
			this.currentScreen.Deactivate();
			this.currentScreen = null;
		}
	}

	// Token: 0x0600A293 RID: 41619 RVA: 0x003DE28C File Offset: 0x003DC48C
	private void ActivateCurrentScreen()
	{
		if (this.currentScreenIndex >= 0 && this.currentScreenIndex < this.newGameFlowScreens.Count)
		{
			NewGameFlowScreen newGameFlowScreen = Util.KInstantiateUI<NewGameFlowScreen>(this.newGameFlowScreens[this.currentScreenIndex].gameObject, base.transform.parent.gameObject, true);
			newGameFlowScreen.OnNavigateForward += this.Next;
			newGameFlowScreen.OnNavigateBackward += this.Previous;
			if (!newGameFlowScreen.IsActive() && !newGameFlowScreen.activateOnSpawn)
			{
				newGameFlowScreen.Activate();
			}
			this.currentScreen = newGameFlowScreen;
		}
	}

	// Token: 0x04007EDC RID: 32476
	public List<NewGameFlowScreen> newGameFlowScreens;

	// Token: 0x04007EDD RID: 32477
	private int currentScreenIndex = -1;

	// Token: 0x04007EDE RID: 32478
	private NewGameFlowScreen currentScreen;
}
