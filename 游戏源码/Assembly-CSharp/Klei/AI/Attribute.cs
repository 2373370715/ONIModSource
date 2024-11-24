using System;
using System.Collections.Generic;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02003B01 RID: 15105
	public class Attribute : Resource
	{
		// Token: 0x0600E855 RID: 59477 RVA: 0x004C0B68 File Offset: 0x004BED68
		public Attribute(string id, bool is_trainable, Attribute.Display show_in_ui, bool is_profession, float base_value = 0f, string uiSprite = null, string thoughtSprite = null, string uiFullColourSprite = null, string[] overrideDLCIDs = null) : base(id, null, null)
		{
			string str = "STRINGS.DUPLICANTS.ATTRIBUTES." + id.ToUpper();
			this.Name = Strings.Get(new StringKey(str + ".NAME"));
			this.ProfessionName = Strings.Get(new StringKey(str + ".NAME"));
			this.Description = Strings.Get(new StringKey(str + ".DESC"));
			this.IsTrainable = is_trainable;
			this.IsProfession = is_profession;
			this.ShowInUI = show_in_ui;
			this.BaseValue = base_value;
			this.formatter = Attribute.defaultFormatter;
			this.uiSprite = uiSprite;
			this.thoughtSprite = thoughtSprite;
			this.uiFullColourSprite = uiFullColourSprite;
			if (overrideDLCIDs != null)
			{
				this.DLCIds = overrideDLCIDs;
			}
		}

		// Token: 0x0600E856 RID: 59478 RVA: 0x004C0C54 File Offset: 0x004BEE54
		public Attribute(string id, string name, string profession_name, string attribute_description, float base_value, Attribute.Display show_in_ui, bool is_trainable, string uiSprite = null, string thoughtSprite = null, string uiFullColourSprite = null) : base(id, name)
		{
			this.Description = attribute_description;
			this.ProfessionName = profession_name;
			this.BaseValue = base_value;
			this.ShowInUI = show_in_ui;
			this.IsTrainable = is_trainable;
			this.uiSprite = uiSprite;
			this.thoughtSprite = thoughtSprite;
			this.uiFullColourSprite = uiFullColourSprite;
			if (this.ProfessionName == "")
			{
				this.ProfessionName = null;
			}
		}

		// Token: 0x0600E857 RID: 59479 RVA: 0x0013B698 File Offset: 0x00139898
		public void SetFormatter(IAttributeFormatter formatter)
		{
			this.formatter = formatter;
		}

		// Token: 0x0600E858 RID: 59480 RVA: 0x0013B6A1 File Offset: 0x001398A1
		public AttributeInstance Lookup(Component cmp)
		{
			return this.Lookup(cmp.gameObject);
		}

		// Token: 0x0600E859 RID: 59481 RVA: 0x004C0CD8 File Offset: 0x004BEED8
		public AttributeInstance Lookup(GameObject go)
		{
			Attributes attributes = go.GetAttributes();
			if (attributes != null)
			{
				return attributes.Get(this);
			}
			return null;
		}

		// Token: 0x0600E85A RID: 59482 RVA: 0x0013B6AF File Offset: 0x001398AF
		public string GetDescription(AttributeInstance instance)
		{
			return instance.GetDescription();
		}

		// Token: 0x0600E85B RID: 59483 RVA: 0x0013B6B7 File Offset: 0x001398B7
		public string GetTooltip(AttributeInstance instance)
		{
			return this.formatter.GetTooltip(this, instance);
		}

		// Token: 0x0400E41F RID: 58399
		private static readonly StandardAttributeFormatter defaultFormatter = new StandardAttributeFormatter(GameUtil.UnitClass.SimpleFloat, GameUtil.TimeSlice.None);

		// Token: 0x0400E420 RID: 58400
		public string Description;

		// Token: 0x0400E421 RID: 58401
		public float BaseValue;

		// Token: 0x0400E422 RID: 58402
		public Attribute.Display ShowInUI;

		// Token: 0x0400E423 RID: 58403
		public bool IsTrainable;

		// Token: 0x0400E424 RID: 58404
		public bool IsProfession;

		// Token: 0x0400E425 RID: 58405
		public string ProfessionName;

		// Token: 0x0400E426 RID: 58406
		public List<AttributeConverter> converters = new List<AttributeConverter>();

		// Token: 0x0400E427 RID: 58407
		public string uiSprite;

		// Token: 0x0400E428 RID: 58408
		public string thoughtSprite;

		// Token: 0x0400E429 RID: 58409
		public string uiFullColourSprite;

		// Token: 0x0400E42A RID: 58410
		public string[] DLCIds = DlcManager.AVAILABLE_ALL_VERSIONS;

		// Token: 0x0400E42B RID: 58411
		public IAttributeFormatter formatter;

		// Token: 0x02003B02 RID: 15106
		public enum Display
		{
			// Token: 0x0400E42D RID: 58413
			Normal,
			// Token: 0x0400E42E RID: 58414
			Skill,
			// Token: 0x0400E42F RID: 58415
			Expectation,
			// Token: 0x0400E430 RID: 58416
			General,
			// Token: 0x0400E431 RID: 58417
			Details,
			// Token: 0x0400E432 RID: 58418
			Never
		}
	}
}
