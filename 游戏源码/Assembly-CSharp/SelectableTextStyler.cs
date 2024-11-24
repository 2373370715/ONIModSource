using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02001F15 RID: 7957
public class SelectableTextStyler : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerClickHandler
{
	// Token: 0x0600A7D2 RID: 42962 RVA: 0x0010CE39 File Offset: 0x0010B039
	private void Start()
	{
		this.SetState(this.state, SelectableTextStyler.HoverState.Normal);
	}

	// Token: 0x0600A7D3 RID: 42963 RVA: 0x0010CE48 File Offset: 0x0010B048
	private void SetState(SelectableTextStyler.State state, SelectableTextStyler.HoverState hover_state)
	{
		if (state == SelectableTextStyler.State.Normal)
		{
			if (hover_state != SelectableTextStyler.HoverState.Normal)
			{
				if (hover_state == SelectableTextStyler.HoverState.Hovered)
				{
					this.target.textStyleSetting = this.normalHovered;
				}
			}
			else
			{
				this.target.textStyleSetting = this.normalNormal;
			}
		}
		this.target.ApplySettings();
	}

	// Token: 0x0600A7D4 RID: 42964 RVA: 0x0010CE85 File Offset: 0x0010B085
	public void OnPointerEnter(PointerEventData eventData)
	{
		this.SetState(this.state, SelectableTextStyler.HoverState.Hovered);
	}

	// Token: 0x0600A7D5 RID: 42965 RVA: 0x0010CE39 File Offset: 0x0010B039
	public void OnPointerExit(PointerEventData eventData)
	{
		this.SetState(this.state, SelectableTextStyler.HoverState.Normal);
	}

	// Token: 0x0600A7D6 RID: 42966 RVA: 0x0010CE39 File Offset: 0x0010B039
	public void OnPointerClick(PointerEventData eventData)
	{
		this.SetState(this.state, SelectableTextStyler.HoverState.Normal);
	}

	// Token: 0x040083F0 RID: 33776
	[SerializeField]
	private LocText target;

	// Token: 0x040083F1 RID: 33777
	[SerializeField]
	private SelectableTextStyler.State state;

	// Token: 0x040083F2 RID: 33778
	[SerializeField]
	private TextStyleSetting normalNormal;

	// Token: 0x040083F3 RID: 33779
	[SerializeField]
	private TextStyleSetting normalHovered;

	// Token: 0x02001F16 RID: 7958
	public enum State
	{
		// Token: 0x040083F5 RID: 33781
		Normal
	}

	// Token: 0x02001F17 RID: 7959
	public enum HoverState
	{
		// Token: 0x040083F7 RID: 33783
		Normal,
		// Token: 0x040083F8 RID: 33784
		Hovered
	}
}
