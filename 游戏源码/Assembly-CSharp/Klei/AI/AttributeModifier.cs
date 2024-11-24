using System;
using System.Diagnostics;

namespace Klei.AI
{
	// Token: 0x02003B0A RID: 15114
	[DebuggerDisplay("{AttributeId}")]
	public class AttributeModifier
	{
		// Token: 0x17000C15 RID: 3093
		// (get) Token: 0x0600E891 RID: 59537 RVA: 0x0013B8F1 File Offset: 0x00139AF1
		// (set) Token: 0x0600E892 RID: 59538 RVA: 0x0013B8F9 File Offset: 0x00139AF9
		public string AttributeId { get; private set; }

		// Token: 0x17000C16 RID: 3094
		// (get) Token: 0x0600E893 RID: 59539 RVA: 0x0013B902 File Offset: 0x00139B02
		// (set) Token: 0x0600E894 RID: 59540 RVA: 0x0013B90A File Offset: 0x00139B0A
		public float Value { get; private set; }

		// Token: 0x17000C17 RID: 3095
		// (get) Token: 0x0600E895 RID: 59541 RVA: 0x0013B913 File Offset: 0x00139B13
		// (set) Token: 0x0600E896 RID: 59542 RVA: 0x0013B91B File Offset: 0x00139B1B
		public bool IsMultiplier { get; private set; }

		// Token: 0x17000C18 RID: 3096
		// (get) Token: 0x0600E897 RID: 59543 RVA: 0x0013B924 File Offset: 0x00139B24
		// (set) Token: 0x0600E898 RID: 59544 RVA: 0x0013B92C File Offset: 0x00139B2C
		public GameUtil.TimeSlice? OverrideTimeSlice { get; set; }

		// Token: 0x17000C19 RID: 3097
		// (get) Token: 0x0600E899 RID: 59545 RVA: 0x0013B935 File Offset: 0x00139B35
		// (set) Token: 0x0600E89A RID: 59546 RVA: 0x0013B93D File Offset: 0x00139B3D
		public bool UIOnly { get; private set; }

		// Token: 0x17000C1A RID: 3098
		// (get) Token: 0x0600E89B RID: 59547 RVA: 0x0013B946 File Offset: 0x00139B46
		// (set) Token: 0x0600E89C RID: 59548 RVA: 0x0013B94E File Offset: 0x00139B4E
		public bool IsReadonly { get; private set; }

		// Token: 0x0600E89D RID: 59549 RVA: 0x004C175C File Offset: 0x004BF95C
		public AttributeModifier(string attribute_id, float value, string description = null, bool is_multiplier = false, bool uiOnly = false, bool is_readonly = true)
		{
			this.AttributeId = attribute_id;
			this.Value = value;
			this.Description = ((description == null) ? attribute_id : description);
			this.DescriptionCB = null;
			this.IsMultiplier = is_multiplier;
			this.UIOnly = uiOnly;
			this.IsReadonly = is_readonly;
			this.OverrideTimeSlice = null;
		}

		// Token: 0x0600E89E RID: 59550 RVA: 0x004C17B8 File Offset: 0x004BF9B8
		public AttributeModifier(string attribute_id, float value, Func<string> description_cb, bool is_multiplier = false, bool uiOnly = false)
		{
			this.AttributeId = attribute_id;
			this.Value = value;
			this.DescriptionCB = description_cb;
			this.Description = null;
			this.IsMultiplier = is_multiplier;
			this.UIOnly = uiOnly;
			this.OverrideTimeSlice = null;
			if (description_cb == null)
			{
				global::Debug.LogWarning("AttributeModifier being constructed without a description callback: " + attribute_id);
			}
		}

		// Token: 0x0600E89F RID: 59551 RVA: 0x0013B957 File Offset: 0x00139B57
		public void SetValue(float value)
		{
			this.Value = value;
		}

		// Token: 0x0600E8A0 RID: 59552 RVA: 0x004C181C File Offset: 0x004BFA1C
		public string GetName()
		{
			Attribute attribute = Db.Get().Attributes.TryGet(this.AttributeId);
			if (attribute != null && attribute.ShowInUI != Attribute.Display.Never)
			{
				return attribute.Name;
			}
			return "";
		}

		// Token: 0x0600E8A1 RID: 59553 RVA: 0x0013B960 File Offset: 0x00139B60
		public string GetDescription()
		{
			if (this.DescriptionCB == null)
			{
				return this.Description;
			}
			return this.DescriptionCB();
		}

		// Token: 0x0600E8A2 RID: 59554 RVA: 0x004C1858 File Offset: 0x004BFA58
		public string GetFormattedString()
		{
			IAttributeFormatter attributeFormatter = null;
			Attribute attribute = Db.Get().Attributes.TryGet(this.AttributeId);
			if (!this.IsMultiplier)
			{
				if (attribute != null)
				{
					attributeFormatter = attribute.formatter;
				}
				else
				{
					attribute = Db.Get().BuildingAttributes.TryGet(this.AttributeId);
					if (attribute != null)
					{
						attributeFormatter = attribute.formatter;
					}
					else
					{
						attribute = Db.Get().PlantAttributes.TryGet(this.AttributeId);
						if (attribute != null)
						{
							attributeFormatter = attribute.formatter;
						}
					}
				}
			}
			string text = "";
			if (attributeFormatter != null)
			{
				text = attributeFormatter.GetFormattedModifier(this);
			}
			else if (this.IsMultiplier)
			{
				text += GameUtil.GetFormattedPercent(this.Value * 100f, GameUtil.TimeSlice.None);
			}
			else
			{
				text += GameUtil.GetFormattedSimple(this.Value, GameUtil.TimeSlice.None, null);
			}
			if (text != null && text.Length > 0 && text[0] != '-')
			{
				GameUtil.TimeSlice? overrideTimeSlice = this.OverrideTimeSlice;
				GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None;
				if (!(overrideTimeSlice.GetValueOrDefault() == timeSlice & overrideTimeSlice != null))
				{
					text = GameUtil.AddPositiveSign(text, this.Value > 0f);
				}
			}
			return text;
		}

		// Token: 0x0600E8A3 RID: 59555 RVA: 0x0013B97C File Offset: 0x00139B7C
		public AttributeModifier Clone()
		{
			return new AttributeModifier(this.AttributeId, this.Value, this.Description, false, false, true);
		}

		// Token: 0x0400E44F RID: 58447
		public string Description;

		// Token: 0x0400E450 RID: 58448
		public Func<string> DescriptionCB;
	}
}
