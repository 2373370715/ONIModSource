using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02002063 RID: 8291
[AddComponentMenu("KMonoBehaviour/scripts/MultiToggle")]
public class MultiToggle : KMonoBehaviour, IPointerClickHandler, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
	// Token: 0x17000B3C RID: 2876
	// (get) Token: 0x0600B072 RID: 45170 RVA: 0x00112B77 File Offset: 0x00110D77
	public int CurrentState
	{
		get
		{
			return this.state;
		}
	}

	// Token: 0x0600B073 RID: 45171 RVA: 0x00112B7F File Offset: 0x00110D7F
	public void NextState()
	{
		this.ChangeState((this.state + 1) % this.states.Length);
	}

	// Token: 0x0600B074 RID: 45172 RVA: 0x00112B98 File Offset: 0x00110D98
	protected virtual void Update()
	{
		if (this.clickHeldDown)
		{
			this.totalHeldTime += Time.unscaledDeltaTime;
			if (this.totalHeldTime > this.heldTimeThreshold && this.onHold != null)
			{
				this.onHold();
			}
		}
	}

	// Token: 0x0600B075 RID: 45173 RVA: 0x00112BD5 File Offset: 0x00110DD5
	protected override void OnDisable()
	{
		if (!base.gameObject.activeInHierarchy)
		{
			this.RefreshHoverColor();
			this.pointerOver = false;
			this.StopHolding();
		}
	}

	// Token: 0x0600B076 RID: 45174 RVA: 0x00112BF7 File Offset: 0x00110DF7
	public void ChangeState(int new_state_index, bool forceRefreshState)
	{
		if (forceRefreshState)
		{
			this.stateDirty = true;
		}
		this.ChangeState(new_state_index);
	}

	// Token: 0x0600B077 RID: 45175 RVA: 0x00424B7C File Offset: 0x00422D7C
	public void ChangeState(int new_state_index)
	{
		if (!this.stateDirty && new_state_index == this.state)
		{
			return;
		}
		this.stateDirty = false;
		this.state = new_state_index;
		try
		{
			this.toggle_image.sprite = this.states[new_state_index].sprite;
			this.toggle_image.color = this.states[new_state_index].color;
			if (this.states[new_state_index].use_rect_margins)
			{
				this.toggle_image.rectTransform().sizeDelta = this.states[new_state_index].rect_margins;
			}
		}
		catch
		{
			string text = base.gameObject.name;
			Transform transform = base.transform;
			while (transform.parent != null)
			{
				text = text.Insert(0, transform.name + ">");
				transform = transform.parent;
			}
			global::Debug.LogError("Multi Toggle state index out of range: " + text + " idx:" + new_state_index.ToString(), base.gameObject);
		}
		foreach (StatePresentationSetting statePresentationSetting in this.states[this.state].additional_display_settings)
		{
			if (!(statePresentationSetting.image_target == null))
			{
				statePresentationSetting.image_target.sprite = statePresentationSetting.sprite;
				statePresentationSetting.image_target.color = statePresentationSetting.color;
			}
		}
		this.RefreshHoverColor();
	}

	// Token: 0x0600B078 RID: 45176 RVA: 0x00424CF8 File Offset: 0x00422EF8
	public virtual void OnPointerClick(PointerEventData eventData)
	{
		if (!this.allowRightClick && eventData.button == PointerEventData.InputButton.Right)
		{
			return;
		}
		if (this.states.Length - 1 < this.state)
		{
			global::Debug.LogWarning("Multi toggle has too few / no states");
		}
		bool flag = false;
		if (this.onDoubleClick != null && eventData.clickCount == 2)
		{
			flag = this.onDoubleClick();
		}
		if (this.onClick != null && !flag)
		{
			this.onClick();
		}
		this.RefreshHoverColor();
	}

	// Token: 0x0600B079 RID: 45177 RVA: 0x00424D70 File Offset: 0x00422F70
	public void OnPointerEnter(PointerEventData eventData)
	{
		this.pointerOver = true;
		if (!KInputManager.isFocused)
		{
			return;
		}
		KInputManager.SetUserActive();
		if (this.states.Length == 0)
		{
			return;
		}
		if (this.states[this.state].use_color_on_hover && this.states[this.state].color_on_hover != this.states[this.state].color)
		{
			this.toggle_image.color = this.states[this.state].color_on_hover;
		}
		if (this.states[this.state].use_rect_margins)
		{
			this.toggle_image.rectTransform().sizeDelta = this.states[this.state].rect_margins;
		}
		foreach (StatePresentationSetting statePresentationSetting in this.states[this.state].additional_display_settings)
		{
			if (!(statePresentationSetting.image_target == null) && statePresentationSetting.use_color_on_hover)
			{
				statePresentationSetting.image_target.color = statePresentationSetting.color_on_hover;
			}
		}
		if (this.onEnter != null)
		{
			this.onEnter();
		}
	}

	// Token: 0x0600B07A RID: 45178 RVA: 0x00424EAC File Offset: 0x004230AC
	protected void RefreshHoverColor()
	{
		if (base.gameObject.activeInHierarchy)
		{
			if (this.pointerOver)
			{
				if (this.states[this.state].use_color_on_hover && this.states[this.state].color_on_hover != this.states[this.state].color)
				{
					this.toggle_image.color = this.states[this.state].color_on_hover;
				}
				foreach (StatePresentationSetting statePresentationSetting in this.states[this.state].additional_display_settings)
				{
					if (!(statePresentationSetting.image_target == null) && !(statePresentationSetting.image_target == null) && statePresentationSetting.use_color_on_hover)
					{
						statePresentationSetting.image_target.color = statePresentationSetting.color_on_hover;
					}
				}
			}
			return;
		}
		if (this.states.Length == 0)
		{
			return;
		}
		if (this.states[this.state].use_color_on_hover && this.states[this.state].color_on_hover != this.states[this.state].color)
		{
			this.toggle_image.color = this.states[this.state].color;
		}
		foreach (StatePresentationSetting statePresentationSetting2 in this.states[this.state].additional_display_settings)
		{
			if (!(statePresentationSetting2.image_target == null) && statePresentationSetting2.use_color_on_hover)
			{
				statePresentationSetting2.image_target.color = statePresentationSetting2.color;
			}
		}
	}

	// Token: 0x0600B07B RID: 45179 RVA: 0x00425070 File Offset: 0x00423270
	public void OnPointerExit(PointerEventData eventData)
	{
		this.pointerOver = false;
		if (!KInputManager.isFocused)
		{
			return;
		}
		KInputManager.SetUserActive();
		if (this.states.Length == 0)
		{
			return;
		}
		if (this.states[this.state].use_color_on_hover && this.states[this.state].color_on_hover != this.states[this.state].color)
		{
			this.toggle_image.color = this.states[this.state].color;
		}
		if (this.states[this.state].use_rect_margins)
		{
			this.toggle_image.rectTransform().sizeDelta = this.states[this.state].rect_margins;
		}
		foreach (StatePresentationSetting statePresentationSetting in this.states[this.state].additional_display_settings)
		{
			if (!(statePresentationSetting.image_target == null) && statePresentationSetting.use_color_on_hover)
			{
				statePresentationSetting.image_target.color = statePresentationSetting.color;
			}
		}
		if (this.onExit != null)
		{
			this.onExit();
		}
	}

	// Token: 0x0600B07C RID: 45180 RVA: 0x004251AC File Offset: 0x004233AC
	public virtual void OnPointerDown(PointerEventData eventData)
	{
		if (!this.allowRightClick && eventData.button == PointerEventData.InputButton.Right)
		{
			return;
		}
		this.clickHeldDown = true;
		if (this.play_sound_on_click)
		{
			ToggleState toggleState = this.states[this.state];
			string on_click_override_sound_path = toggleState.on_click_override_sound_path;
			bool has_sound_parameter = toggleState.has_sound_parameter;
			if (on_click_override_sound_path == "")
			{
				KFMOD.PlayUISound(GlobalAssets.GetSound("HUD_Click", false));
				return;
			}
			if (on_click_override_sound_path != "" && has_sound_parameter)
			{
				KFMOD.PlayUISoundWithParameter(GlobalAssets.GetSound("General_Item_Click", false), toggleState.sound_parameter_name, toggleState.sound_parameter_value);
				KFMOD.PlayUISoundWithParameter(GlobalAssets.GetSound(on_click_override_sound_path, false), toggleState.sound_parameter_name, toggleState.sound_parameter_value);
				return;
			}
			KFMOD.PlayUISound(GlobalAssets.GetSound(on_click_override_sound_path, false));
		}
	}

	// Token: 0x0600B07D RID: 45181 RVA: 0x00112C0A File Offset: 0x00110E0A
	public virtual void OnPointerUp(PointerEventData eventData)
	{
		if (!this.allowRightClick && eventData.button == PointerEventData.InputButton.Right)
		{
			return;
		}
		this.StopHolding();
	}

	// Token: 0x0600B07E RID: 45182 RVA: 0x0042526C File Offset: 0x0042346C
	private void StopHolding()
	{
		if (this.clickHeldDown)
		{
			if (this.play_sound_on_release && this.states[this.state].on_release_override_sound_path != "")
			{
				KFMOD.PlayUISound(GlobalAssets.GetSound(this.states[this.state].on_release_override_sound_path, false));
			}
			this.clickHeldDown = false;
			if (this.onStopHold != null)
			{
				this.onStopHold();
			}
		}
		this.totalHeldTime = 0f;
	}

	// Token: 0x04008B52 RID: 35666
	[Header("Settings")]
	[SerializeField]
	public ToggleState[] states;

	// Token: 0x04008B53 RID: 35667
	public bool play_sound_on_click = true;

	// Token: 0x04008B54 RID: 35668
	public bool play_sound_on_release;

	// Token: 0x04008B55 RID: 35669
	public Image toggle_image;

	// Token: 0x04008B56 RID: 35670
	protected int state;

	// Token: 0x04008B57 RID: 35671
	public System.Action onClick;

	// Token: 0x04008B58 RID: 35672
	private bool stateDirty = true;

	// Token: 0x04008B59 RID: 35673
	public Func<bool> onDoubleClick;

	// Token: 0x04008B5A RID: 35674
	public System.Action onEnter;

	// Token: 0x04008B5B RID: 35675
	public System.Action onExit;

	// Token: 0x04008B5C RID: 35676
	public System.Action onHold;

	// Token: 0x04008B5D RID: 35677
	public System.Action onStopHold;

	// Token: 0x04008B5E RID: 35678
	public bool allowRightClick = true;

	// Token: 0x04008B5F RID: 35679
	protected bool clickHeldDown;

	// Token: 0x04008B60 RID: 35680
	protected float totalHeldTime;

	// Token: 0x04008B61 RID: 35681
	protected float heldTimeThreshold = 0.4f;

	// Token: 0x04008B62 RID: 35682
	private bool pointerOver;
}
