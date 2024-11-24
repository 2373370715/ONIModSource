using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

// Token: 0x02000988 RID: 2440
public class AnimEventHandlerManager : KMonoBehaviour
{
	// Token: 0x17000195 RID: 405
	// (get) Token: 0x06002C41 RID: 11329 RVA: 0x000BCA58 File Offset: 0x000BAC58
	// (set) Token: 0x06002C42 RID: 11330 RVA: 0x000BCA5F File Offset: 0x000BAC5F
	public static AnimEventHandlerManager Instance { get; private set; }

	// Token: 0x06002C43 RID: 11331 RVA: 0x000BCA67 File Offset: 0x000BAC67
	public static void DestroyInstance()
	{
		AnimEventHandlerManager.Instance = null;
	}

	// Token: 0x06002C44 RID: 11332 RVA: 0x000BCA6F File Offset: 0x000BAC6F
	protected override void OnPrefabInit()
	{
		AnimEventHandlerManager.Instance = this;
		this.handlers = new List<AnimEventHandler>();
	}

	// Token: 0x06002C45 RID: 11333 RVA: 0x000BCA82 File Offset: 0x000BAC82
	public void Add(AnimEventHandler handler)
	{
		this.handlers.Add(handler);
	}

	// Token: 0x06002C46 RID: 11334 RVA: 0x000BCA90 File Offset: 0x000BAC90
	public void Remove(AnimEventHandler handler)
	{
		this.handlers.Remove(handler);
	}

	// Token: 0x06002C47 RID: 11335 RVA: 0x000BCA9F File Offset: 0x000BAC9F
	private bool IsVisibleToZoom()
	{
		return !(Game.MainCamera == null) && Game.MainCamera.orthographicSize < 40f;
	}

	// Token: 0x06002C48 RID: 11336 RVA: 0x001EB538 File Offset: 0x001E9738
	public void LateUpdate()
	{
		if (!this.IsVisibleToZoom())
		{
			return;
		}
		AnimEventHandlerManager.<>c__DisplayClass11_0 CS$<>8__locals1;
		Grid.GetVisibleCellRangeInActiveWorld(out CS$<>8__locals1.min, out CS$<>8__locals1.max, 4, 1.5f);
		foreach (AnimEventHandler animEventHandler in this.handlers)
		{
			if (AnimEventHandlerManager.<LateUpdate>g__IsVisible|11_0(animEventHandler, ref CS$<>8__locals1))
			{
				animEventHandler.UpdateOffset();
			}
		}
	}

	// Token: 0x06002C4A RID: 11338 RVA: 0x001EB5B8 File Offset: 0x001E97B8
	[CompilerGenerated]
	internal static bool <LateUpdate>g__IsVisible|11_0(AnimEventHandler handler, ref AnimEventHandlerManager.<>c__DisplayClass11_0 A_1)
	{
		int num;
		int num2;
		Grid.CellToXY(handler.GetCachedCell(), out num, out num2);
		return num >= A_1.min.x && num2 >= A_1.min.y && num < A_1.max.x && num2 < A_1.max.y;
	}

	// Token: 0x04001DB9 RID: 7609
	private const float HIDE_DISTANCE = 40f;

	// Token: 0x04001DBB RID: 7611
	private List<AnimEventHandler> handlers;
}
