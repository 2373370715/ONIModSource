using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001C8C RID: 7308
public abstract class CustomGameSettingsPanelBase : MonoBehaviour
{
	// Token: 0x06009860 RID: 39008 RVA: 0x000A5E40 File Offset: 0x000A4040
	public virtual void Init()
	{
	}

	// Token: 0x06009861 RID: 39009 RVA: 0x000A5E40 File Offset: 0x000A4040
	public virtual void Uninit()
	{
	}

	// Token: 0x06009862 RID: 39010 RVA: 0x001031E1 File Offset: 0x001013E1
	private void OnEnable()
	{
		this.isDirty = true;
	}

	// Token: 0x06009863 RID: 39011 RVA: 0x001031EA File Offset: 0x001013EA
	private void Update()
	{
		if (this.isDirty)
		{
			this.isDirty = false;
			this.Refresh();
		}
	}

	// Token: 0x06009864 RID: 39012 RVA: 0x00103201 File Offset: 0x00101401
	protected void AddWidget(CustomGameSettingWidget widget)
	{
		widget.onSettingChanged += this.OnWidgetChanged;
		this.widgets.Add(widget);
	}

	// Token: 0x06009865 RID: 39013 RVA: 0x001031E1 File Offset: 0x001013E1
	private void OnWidgetChanged(CustomGameSettingWidget widget)
	{
		this.isDirty = true;
	}

	// Token: 0x06009866 RID: 39014 RVA: 0x003AFA74 File Offset: 0x003ADC74
	public virtual void Refresh()
	{
		foreach (CustomGameSettingWidget customGameSettingWidget in this.widgets)
		{
			customGameSettingWidget.Refresh();
		}
	}

	// Token: 0x040076A7 RID: 30375
	protected List<CustomGameSettingWidget> widgets = new List<CustomGameSettingWidget>();

	// Token: 0x040076A8 RID: 30376
	private bool isDirty;
}
