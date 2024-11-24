using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020019A8 RID: 6568
public class StatusItem : Resource
{
	// Token: 0x060088CE RID: 35022 RVA: 0x00355300 File Offset: 0x00353500
	private StatusItem(string id, string composed_prefix) : base(id, Strings.Get(composed_prefix + ".NAME"))
	{
		this.composedPrefix = composed_prefix;
		this.tooltipText = Strings.Get(composed_prefix + ".TOOLTIP");
	}

	// Token: 0x060088CF RID: 35023 RVA: 0x00355354 File Offset: 0x00353554
	private void SetIcon(string icon, StatusItem.IconType icon_type, NotificationType notification_type, bool allow_multiples, HashedString render_overlay, bool show_world_icon = true, int status_overlays = 129022, Func<string, object, string> resolve_string_callback = null)
	{
		switch (icon_type)
		{
		case StatusItem.IconType.Info:
			icon = "dash";
			break;
		case StatusItem.IconType.Exclamation:
			icon = "status_item_exclamation";
			break;
		}
		this.iconName = icon;
		this.notificationType = notification_type;
		this.sprite = Assets.GetTintedSprite(icon);
		if (this.sprite == null)
		{
			this.sprite = new TintedSprite();
			this.sprite.sprite = Assets.GetSprite(icon);
			this.sprite.color = new Color(0f, 0f, 0f, 255f);
		}
		this.iconType = icon_type;
		this.allowMultiples = allow_multiples;
		this.render_overlay = render_overlay;
		this.showShowWorldIcon = show_world_icon;
		this.status_overlays = status_overlays;
		this.resolveStringCallback = resolve_string_callback;
		if (this.sprite == null)
		{
			global::Debug.LogWarning("Status item '" + this.Id + "' references a missing icon: " + icon);
		}
	}

	// Token: 0x060088D0 RID: 35024 RVA: 0x00355440 File Offset: 0x00353640
	public StatusItem(string id, string prefix, string icon, StatusItem.IconType icon_type, NotificationType notification_type, bool allow_multiples, HashedString render_overlay, bool showWorldIcon = true, int status_overlays = 129022, Func<string, object, string> resolve_string_callback = null) : this(id, "STRINGS." + prefix + ".STATUSITEMS." + id.ToUpper())
	{
		this.SetIcon(icon, icon_type, notification_type, allow_multiples, render_overlay, showWorldIcon, status_overlays, resolve_string_callback);
	}

	// Token: 0x060088D1 RID: 35025 RVA: 0x00355480 File Offset: 0x00353680
	public StatusItem(string id, string name, string tooltip, string icon, StatusItem.IconType icon_type, NotificationType notification_type, bool allow_multiples, HashedString render_overlay, int status_overlays = 129022, bool showWorldIcon = true, Func<string, object, string> resolve_string_callback = null) : base(id, name)
	{
		this.tooltipText = tooltip;
		this.SetIcon(icon, icon_type, notification_type, allow_multiples, render_overlay, showWorldIcon, status_overlays, resolve_string_callback);
	}

	// Token: 0x060088D2 RID: 35026 RVA: 0x003554BC File Offset: 0x003536BC
	public void AddNotification(string sound_path = null, string notification_text = null, string notification_tooltip = null)
	{
		this.shouldNotify = true;
		if (sound_path == null)
		{
			if (this.notificationType == NotificationType.Bad)
			{
				this.soundPath = "Warning";
			}
			else
			{
				this.soundPath = "Notification";
			}
		}
		else
		{
			this.soundPath = sound_path;
		}
		if (notification_text != null)
		{
			this.notificationText = notification_text;
		}
		else
		{
			DebugUtil.Assert(this.composedPrefix != null, "When adding a notification, either set the status prefix or specify strings!");
			this.notificationText = Strings.Get(this.composedPrefix + ".NOTIFICATION_NAME");
		}
		if (notification_tooltip != null)
		{
			this.notificationTooltipText = notification_tooltip;
			return;
		}
		DebugUtil.Assert(this.composedPrefix != null, "When adding a notification, either set the status prefix or specify strings!");
		this.notificationTooltipText = Strings.Get(this.composedPrefix + ".NOTIFICATION_TOOLTIP");
	}

	// Token: 0x060088D3 RID: 35027 RVA: 0x000F97D9 File Offset: 0x000F79D9
	public virtual string GetName(object data)
	{
		return this.ResolveString(this.Name, data);
	}

	// Token: 0x060088D4 RID: 35028 RVA: 0x000F97E8 File Offset: 0x000F79E8
	public virtual string GetTooltip(object data)
	{
		return this.ResolveTooltip(this.tooltipText, data);
	}

	// Token: 0x060088D5 RID: 35029 RVA: 0x000F97F7 File Offset: 0x000F79F7
	private string ResolveString(string str, object data)
	{
		if (this.resolveStringCallback != null && (data != null || this.resolveStringCallback_shouldStillCallIfDataIsNull))
		{
			return this.resolveStringCallback(str, data);
		}
		return str;
	}

	// Token: 0x060088D6 RID: 35030 RVA: 0x0035557C File Offset: 0x0035377C
	private string ResolveTooltip(string str, object data)
	{
		if (data != null)
		{
			if (this.resolveTooltipCallback != null)
			{
				return this.resolveTooltipCallback(str, data);
			}
			if (this.resolveStringCallback != null)
			{
				return this.resolveStringCallback(str, data);
			}
		}
		else
		{
			if (this.resolveStringCallback_shouldStillCallIfDataIsNull && this.resolveStringCallback != null)
			{
				return this.resolveStringCallback(str, data);
			}
			if (this.resolveTooltipCallback_shouldStillCallIfDataIsNull && this.resolveTooltipCallback != null)
			{
				return this.resolveTooltipCallback(str, data);
			}
		}
		return str;
	}

	// Token: 0x060088D7 RID: 35031 RVA: 0x000F981B File Offset: 0x000F7A1B
	public bool ShouldShowIcon()
	{
		return this.iconType == StatusItem.IconType.Custom && this.showShowWorldIcon;
	}

	// Token: 0x060088D8 RID: 35032 RVA: 0x003555F8 File Offset: 0x003537F8
	public virtual void ShowToolTip(ToolTip tooltip_widget, object data, TextStyleSetting property_style)
	{
		tooltip_widget.ClearMultiStringTooltip();
		string tooltip = this.GetTooltip(data);
		tooltip_widget.AddMultiStringTooltip(tooltip, property_style);
	}

	// Token: 0x060088D9 RID: 35033 RVA: 0x000F982E File Offset: 0x000F7A2E
	public void SetIcon(Image image, object data)
	{
		if (this.sprite == null)
		{
			return;
		}
		image.color = this.sprite.color;
		image.sprite = this.sprite.sprite;
	}

	// Token: 0x060088DA RID: 35034 RVA: 0x000F985B File Offset: 0x000F7A5B
	public bool UseConditionalCallback(HashedString overlay, Transform transform)
	{
		return overlay != OverlayModes.None.ID && this.conditionalOverlayCallback != null && this.conditionalOverlayCallback(overlay, transform);
	}

	// Token: 0x060088DB RID: 35035 RVA: 0x000F9881 File Offset: 0x000F7A81
	public StatusItem SetResolveStringCallback(Func<string, object, string> cb)
	{
		this.resolveStringCallback = cb;
		return this;
	}

	// Token: 0x060088DC RID: 35036 RVA: 0x000F988B File Offset: 0x000F7A8B
	public void OnClick(object data)
	{
		if (this.statusItemClickCallback != null)
		{
			this.statusItemClickCallback(data);
		}
	}

	// Token: 0x060088DD RID: 35037 RVA: 0x0035561C File Offset: 0x0035381C
	public static StatusItem.StatusItemOverlays GetStatusItemOverlayBySimViewMode(HashedString mode)
	{
		StatusItem.StatusItemOverlays result;
		if (!StatusItem.overlayBitfieldMap.TryGetValue(mode, out result))
		{
			string str = "ViewMode ";
			HashedString hashedString = mode;
			global::Debug.LogWarning(str + hashedString.ToString() + " has no StatusItemOverlay value");
			result = StatusItem.StatusItemOverlays.None;
		}
		return result;
	}

	// Token: 0x040066D7 RID: 26327
	public string tooltipText;

	// Token: 0x040066D8 RID: 26328
	public string notificationText;

	// Token: 0x040066D9 RID: 26329
	public string notificationTooltipText;

	// Token: 0x040066DA RID: 26330
	public string soundPath;

	// Token: 0x040066DB RID: 26331
	public string iconName;

	// Token: 0x040066DC RID: 26332
	public bool unique;

	// Token: 0x040066DD RID: 26333
	public TintedSprite sprite;

	// Token: 0x040066DE RID: 26334
	public bool shouldNotify;

	// Token: 0x040066DF RID: 26335
	public StatusItem.IconType iconType;

	// Token: 0x040066E0 RID: 26336
	public NotificationType notificationType;

	// Token: 0x040066E1 RID: 26337
	public Notification.ClickCallback notificationClickCallback;

	// Token: 0x040066E2 RID: 26338
	public Func<string, object, string> resolveStringCallback;

	// Token: 0x040066E3 RID: 26339
	public Func<string, object, string> resolveTooltipCallback;

	// Token: 0x040066E4 RID: 26340
	public bool resolveStringCallback_shouldStillCallIfDataIsNull;

	// Token: 0x040066E5 RID: 26341
	public bool resolveTooltipCallback_shouldStillCallIfDataIsNull;

	// Token: 0x040066E6 RID: 26342
	public bool allowMultiples;

	// Token: 0x040066E7 RID: 26343
	public Func<HashedString, object, bool> conditionalOverlayCallback;

	// Token: 0x040066E8 RID: 26344
	public HashedString render_overlay;

	// Token: 0x040066E9 RID: 26345
	public int status_overlays;

	// Token: 0x040066EA RID: 26346
	public Action<object> statusItemClickCallback;

	// Token: 0x040066EB RID: 26347
	private string composedPrefix;

	// Token: 0x040066EC RID: 26348
	private bool showShowWorldIcon = true;

	// Token: 0x040066ED RID: 26349
	public const int ALL_OVERLAYS = 129022;

	// Token: 0x040066EE RID: 26350
	private static Dictionary<HashedString, StatusItem.StatusItemOverlays> overlayBitfieldMap = new Dictionary<HashedString, StatusItem.StatusItemOverlays>
	{
		{
			OverlayModes.None.ID,
			StatusItem.StatusItemOverlays.None
		},
		{
			OverlayModes.Power.ID,
			StatusItem.StatusItemOverlays.PowerMap
		},
		{
			OverlayModes.Temperature.ID,
			StatusItem.StatusItemOverlays.Temperature
		},
		{
			OverlayModes.ThermalConductivity.ID,
			StatusItem.StatusItemOverlays.ThermalComfort
		},
		{
			OverlayModes.Light.ID,
			StatusItem.StatusItemOverlays.Light
		},
		{
			OverlayModes.LiquidConduits.ID,
			StatusItem.StatusItemOverlays.LiquidPlumbing
		},
		{
			OverlayModes.GasConduits.ID,
			StatusItem.StatusItemOverlays.GasPlumbing
		},
		{
			OverlayModes.SolidConveyor.ID,
			StatusItem.StatusItemOverlays.Conveyor
		},
		{
			OverlayModes.Decor.ID,
			StatusItem.StatusItemOverlays.Decor
		},
		{
			OverlayModes.Disease.ID,
			StatusItem.StatusItemOverlays.Pathogens
		},
		{
			OverlayModes.Crop.ID,
			StatusItem.StatusItemOverlays.Farming
		},
		{
			OverlayModes.Rooms.ID,
			StatusItem.StatusItemOverlays.Rooms
		},
		{
			OverlayModes.Suit.ID,
			StatusItem.StatusItemOverlays.Suits
		},
		{
			OverlayModes.Logic.ID,
			StatusItem.StatusItemOverlays.Logic
		},
		{
			OverlayModes.Oxygen.ID,
			StatusItem.StatusItemOverlays.None
		},
		{
			OverlayModes.TileMode.ID,
			StatusItem.StatusItemOverlays.None
		},
		{
			OverlayModes.Radiation.ID,
			StatusItem.StatusItemOverlays.Radiation
		}
	};

	// Token: 0x020019A9 RID: 6569
	public enum IconType
	{
		// Token: 0x040066F0 RID: 26352
		Info,
		// Token: 0x040066F1 RID: 26353
		Exclamation,
		// Token: 0x040066F2 RID: 26354
		Custom
	}

	// Token: 0x020019AA RID: 6570
	[Flags]
	public enum StatusItemOverlays
	{
		// Token: 0x040066F4 RID: 26356
		None = 2,
		// Token: 0x040066F5 RID: 26357
		PowerMap = 4,
		// Token: 0x040066F6 RID: 26358
		Temperature = 8,
		// Token: 0x040066F7 RID: 26359
		ThermalComfort = 16,
		// Token: 0x040066F8 RID: 26360
		Light = 32,
		// Token: 0x040066F9 RID: 26361
		LiquidPlumbing = 64,
		// Token: 0x040066FA RID: 26362
		GasPlumbing = 128,
		// Token: 0x040066FB RID: 26363
		Decor = 256,
		// Token: 0x040066FC RID: 26364
		Pathogens = 512,
		// Token: 0x040066FD RID: 26365
		Farming = 1024,
		// Token: 0x040066FE RID: 26366
		Rooms = 4096,
		// Token: 0x040066FF RID: 26367
		Suits = 8192,
		// Token: 0x04006700 RID: 26368
		Logic = 16384,
		// Token: 0x04006701 RID: 26369
		Conveyor = 32768,
		// Token: 0x04006702 RID: 26370
		Radiation = 65536
	}
}
