using System;
using UnityEngine;

// Token: 0x0200142A RID: 5162
public class DisinfectTool : DragTool
{
	// Token: 0x06006AA7 RID: 27303 RVA: 0x000E6069 File Offset: 0x000E4269
	public static void DestroyInstance()
	{
		DisinfectTool.Instance = null;
	}

	// Token: 0x06006AA8 RID: 27304 RVA: 0x000E6071 File Offset: 0x000E4271
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		DisinfectTool.Instance = this;
		this.interceptNumberKeysForPriority = true;
		this.viewMode = OverlayModes.Disease.ID;
	}

	// Token: 0x06006AA9 RID: 27305 RVA: 0x000E5D27 File Offset: 0x000E3F27
	public void Activate()
	{
		PlayerController.Instance.ActivateTool(this);
	}

	// Token: 0x06006AAA RID: 27306 RVA: 0x002DFA24 File Offset: 0x002DDC24
	protected override void OnDragTool(int cell, int distFromOrigin)
	{
		for (int i = 0; i < 45; i++)
		{
			GameObject gameObject = Grid.Objects[cell, i];
			if (gameObject != null)
			{
				Disinfectable component = gameObject.GetComponent<Disinfectable>();
				if (component != null && component.GetComponent<PrimaryElement>().DiseaseCount > 0)
				{
					component.MarkForDisinfect(false);
				}
			}
		}
	}

	// Token: 0x04005062 RID: 20578
	public static DisinfectTool Instance;
}
