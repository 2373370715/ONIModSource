using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000E4A RID: 3658
[SkipSaveFileSerialization]
public class MoveableLogicGateVisualizer : LogicGateBase
{
	// Token: 0x0600488C RID: 18572 RVA: 0x00256F74 File Offset: 0x00255174
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.cell = -1;
		OverlayScreen instance = OverlayScreen.Instance;
		instance.OnOverlayChanged = (Action<HashedString>)Delegate.Combine(instance.OnOverlayChanged, new Action<HashedString>(this.OnOverlayChanged));
		this.OnOverlayChanged(OverlayScreen.Instance.mode);
		base.Subscribe<MoveableLogicGateVisualizer>(-1643076535, MoveableLogicGateVisualizer.OnRotatedDelegate);
	}

	// Token: 0x0600488D RID: 18573 RVA: 0x000CF17F File Offset: 0x000CD37F
	protected override void OnCleanUp()
	{
		OverlayScreen instance = OverlayScreen.Instance;
		instance.OnOverlayChanged = (Action<HashedString>)Delegate.Remove(instance.OnOverlayChanged, new Action<HashedString>(this.OnOverlayChanged));
		this.Unregister();
		base.OnCleanUp();
	}

	// Token: 0x0600488E RID: 18574 RVA: 0x000CF1B3 File Offset: 0x000CD3B3
	private void OnOverlayChanged(HashedString mode)
	{
		if (mode == OverlayModes.Logic.ID)
		{
			this.Register();
			return;
		}
		this.Unregister();
	}

	// Token: 0x0600488F RID: 18575 RVA: 0x000CF1CF File Offset: 0x000CD3CF
	private void OnRotated(object data)
	{
		this.Unregister();
		this.OnOverlayChanged(OverlayScreen.Instance.mode);
	}

	// Token: 0x06004890 RID: 18576 RVA: 0x00256FD8 File Offset: 0x002551D8
	private void Update()
	{
		if (this.visChildren.Count <= 0)
		{
			return;
		}
		int num = Grid.PosToCell(base.transform.GetPosition());
		if (num == this.cell)
		{
			return;
		}
		this.cell = num;
		this.Unregister();
		this.Register();
	}

	// Token: 0x06004891 RID: 18577 RVA: 0x00257024 File Offset: 0x00255224
	private GameObject CreateUIElem(int cell, bool is_input)
	{
		GameObject gameObject = Util.KInstantiate(LogicGateBase.uiSrcData.prefab, Grid.CellToPosCCC(cell, Grid.SceneLayer.Front), Quaternion.identity, GameScreenManager.Instance.worldSpaceCanvas, null, true, 0);
		Image component = gameObject.GetComponent<Image>();
		component.sprite = (is_input ? LogicGateBase.uiSrcData.inputSprite : LogicGateBase.uiSrcData.outputSprite);
		component.raycastTarget = false;
		return gameObject;
	}

	// Token: 0x06004892 RID: 18578 RVA: 0x00257088 File Offset: 0x00255288
	private void Register()
	{
		if (this.visChildren.Count > 0)
		{
			return;
		}
		base.enabled = true;
		this.visChildren.Add(this.CreateUIElem(base.OutputCellOne, false));
		if (base.RequiresFourOutputs)
		{
			this.visChildren.Add(this.CreateUIElem(base.OutputCellTwo, false));
			this.visChildren.Add(this.CreateUIElem(base.OutputCellThree, false));
			this.visChildren.Add(this.CreateUIElem(base.OutputCellFour, false));
		}
		this.visChildren.Add(this.CreateUIElem(base.InputCellOne, true));
		if (base.RequiresTwoInputs)
		{
			this.visChildren.Add(this.CreateUIElem(base.InputCellTwo, true));
		}
		else if (base.RequiresFourInputs)
		{
			this.visChildren.Add(this.CreateUIElem(base.InputCellTwo, true));
			this.visChildren.Add(this.CreateUIElem(base.InputCellThree, true));
			this.visChildren.Add(this.CreateUIElem(base.InputCellFour, true));
		}
		if (base.RequiresControlInputs)
		{
			this.visChildren.Add(this.CreateUIElem(base.ControlCellOne, true));
			this.visChildren.Add(this.CreateUIElem(base.ControlCellTwo, true));
		}
	}

	// Token: 0x06004893 RID: 18579 RVA: 0x002571D8 File Offset: 0x002553D8
	private void Unregister()
	{
		if (this.visChildren.Count <= 0)
		{
			return;
		}
		base.enabled = false;
		this.cell = -1;
		foreach (GameObject original in this.visChildren)
		{
			Util.KDestroyGameObject(original);
		}
		this.visChildren.Clear();
	}

	// Token: 0x040032BB RID: 12987
	private int cell;

	// Token: 0x040032BC RID: 12988
	protected List<GameObject> visChildren = new List<GameObject>();

	// Token: 0x040032BD RID: 12989
	private static readonly EventSystem.IntraObjectHandler<MoveableLogicGateVisualizer> OnRotatedDelegate = new EventSystem.IntraObjectHandler<MoveableLogicGateVisualizer>(delegate(MoveableLogicGateVisualizer component, object data)
	{
		component.OnRotated(data);
	});
}
