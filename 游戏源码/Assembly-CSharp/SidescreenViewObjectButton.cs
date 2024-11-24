using System;
using UnityEngine;

// Token: 0x02000581 RID: 1409
public class SidescreenViewObjectButton : KMonoBehaviour, ISidescreenButtonControl
{
	// Token: 0x060018F2 RID: 6386 RVA: 0x001A14A8 File Offset: 0x0019F6A8
	public bool IsValid()
	{
		SidescreenViewObjectButton.Mode trackMode = this.TrackMode;
		if (trackMode != SidescreenViewObjectButton.Mode.Target)
		{
			return trackMode == SidescreenViewObjectButton.Mode.Cell && Grid.IsValidCell(this.TargetCell);
		}
		return this.Target != null;
	}

	// Token: 0x1700007F RID: 127
	// (get) Token: 0x060018F3 RID: 6387 RVA: 0x000B0750 File Offset: 0x000AE950
	public string SidescreenButtonText
	{
		get
		{
			return this.Text;
		}
	}

	// Token: 0x17000080 RID: 128
	// (get) Token: 0x060018F4 RID: 6388 RVA: 0x000B0758 File Offset: 0x000AE958
	public string SidescreenButtonTooltip
	{
		get
		{
			return this.Tooltip;
		}
	}

	// Token: 0x060018F5 RID: 6389 RVA: 0x000ABCB6 File Offset: 0x000A9EB6
	public void SetButtonTextOverride(ButtonMenuTextOverride textOverride)
	{
		throw new NotImplementedException();
	}

	// Token: 0x060018F6 RID: 6390 RVA: 0x000A65EC File Offset: 0x000A47EC
	public bool SidescreenEnabled()
	{
		return true;
	}

	// Token: 0x060018F7 RID: 6391 RVA: 0x000B0760 File Offset: 0x000AE960
	public bool SidescreenButtonInteractable()
	{
		return this.IsValid();
	}

	// Token: 0x060018F8 RID: 6392 RVA: 0x000B0768 File Offset: 0x000AE968
	public int HorizontalGroupID()
	{
		return this.horizontalGroupID;
	}

	// Token: 0x060018F9 RID: 6393 RVA: 0x001A14E0 File Offset: 0x0019F6E0
	public void OnSidescreenButtonPressed()
	{
		if (this.IsValid())
		{
			SidescreenViewObjectButton.Mode trackMode = this.TrackMode;
			if (trackMode == SidescreenViewObjectButton.Mode.Target)
			{
				CameraController.Instance.CameraGoTo(this.Target.transform.GetPosition(), 2f, true);
				return;
			}
			if (trackMode == SidescreenViewObjectButton.Mode.Cell)
			{
				CameraController.Instance.CameraGoTo(Grid.CellToPos(this.TargetCell), 2f, true);
				return;
			}
		}
		else
		{
			base.gameObject.Trigger(1980521255, null);
		}
	}

	// Token: 0x060018FA RID: 6394 RVA: 0x000ABCBD File Offset: 0x000A9EBD
	public int ButtonSideScreenSortOrder()
	{
		return 20;
	}

	// Token: 0x04000FFD RID: 4093
	public string Text;

	// Token: 0x04000FFE RID: 4094
	public string Tooltip;

	// Token: 0x04000FFF RID: 4095
	public SidescreenViewObjectButton.Mode TrackMode;

	// Token: 0x04001000 RID: 4096
	public GameObject Target;

	// Token: 0x04001001 RID: 4097
	public int TargetCell;

	// Token: 0x04001002 RID: 4098
	public int horizontalGroupID = -1;

	// Token: 0x02000582 RID: 1410
	public enum Mode
	{
		// Token: 0x04001004 RID: 4100
		Target,
		// Token: 0x04001005 RID: 4101
		Cell
	}
}
