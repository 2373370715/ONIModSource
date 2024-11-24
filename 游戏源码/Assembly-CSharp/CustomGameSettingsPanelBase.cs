using System.Collections.Generic;
using UnityEngine;

public abstract class CustomGameSettingsPanelBase : MonoBehaviour
{
	protected List<CustomGameSettingWidget> widgets = new List<CustomGameSettingWidget>();

	private bool isDirty;

	public virtual void Init()
	{
	}

	public virtual void Uninit()
	{
	}

	private void OnEnable()
	{
		isDirty = true;
	}

	private void Update()
	{
		if (isDirty)
		{
			isDirty = false;
			Refresh();
		}
	}

	protected void AddWidget(CustomGameSettingWidget widget)
	{
		widget.onSettingChanged += OnWidgetChanged;
		widgets.Add(widget);
	}

	private void OnWidgetChanged(CustomGameSettingWidget widget)
	{
		isDirty = true;
	}

	public virtual void Refresh()
	{
		foreach (CustomGameSettingWidget widget in widgets)
		{
			widget.Refresh();
		}
	}
}
