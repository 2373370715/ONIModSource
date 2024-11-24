using System;
using System.Collections.Generic;
using System.Diagnostics;
using STRINGS;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02003B06 RID: 15110
	[DebuggerDisplay("{Attribute.Id}")]
	public class AttributeInstance : ModifierInstance<Attribute>
	{
		// Token: 0x17000C11 RID: 3089
		// (get) Token: 0x0600E869 RID: 59497 RVA: 0x0013B784 File Offset: 0x00139984
		public string Id
		{
			get
			{
				return this.Attribute.Id;
			}
		}

		// Token: 0x17000C12 RID: 3090
		// (get) Token: 0x0600E86A RID: 59498 RVA: 0x0013B791 File Offset: 0x00139991
		public string Name
		{
			get
			{
				return this.Attribute.Name;
			}
		}

		// Token: 0x17000C13 RID: 3091
		// (get) Token: 0x0600E86B RID: 59499 RVA: 0x0013B79E File Offset: 0x0013999E
		public string Description
		{
			get
			{
				return this.Attribute.Description;
			}
		}

		// Token: 0x0600E86C RID: 59500 RVA: 0x0013B7AB File Offset: 0x001399AB
		public float GetBaseValue()
		{
			return this.Attribute.BaseValue;
		}

		// Token: 0x0600E86D RID: 59501 RVA: 0x004C0F14 File Offset: 0x004BF114
		public float GetTotalDisplayValue()
		{
			float num = this.Attribute.BaseValue;
			float num2 = 0f;
			for (int num3 = 0; num3 != this.Modifiers.Count; num3++)
			{
				AttributeModifier attributeModifier = this.Modifiers[num3];
				if (!attributeModifier.IsMultiplier)
				{
					num += attributeModifier.Value;
				}
				else
				{
					num2 += attributeModifier.Value;
				}
			}
			if (num2 != 0f)
			{
				num += Mathf.Abs(num) * num2;
			}
			return num;
		}

		// Token: 0x0600E86E RID: 59502 RVA: 0x004C0F88 File Offset: 0x004BF188
		public float GetTotalValue()
		{
			float num = this.Attribute.BaseValue;
			float num2 = 0f;
			for (int num3 = 0; num3 != this.Modifiers.Count; num3++)
			{
				AttributeModifier attributeModifier = this.Modifiers[num3];
				if (!attributeModifier.UIOnly)
				{
					if (!attributeModifier.IsMultiplier)
					{
						num += attributeModifier.Value;
					}
					else
					{
						num2 += attributeModifier.Value;
					}
				}
			}
			if (num2 != 0f)
			{
				num += Mathf.Abs(num) * num2;
			}
			return num;
		}

		// Token: 0x0600E86F RID: 59503 RVA: 0x004C1004 File Offset: 0x004BF204
		public static float GetTotalDisplayValue(Attribute attribute, List<AttributeModifier> modifiers)
		{
			float num = attribute.BaseValue;
			float num2 = 0f;
			for (int num3 = 0; num3 != modifiers.Count; num3++)
			{
				AttributeModifier attributeModifier = modifiers[num3];
				if (!attributeModifier.IsMultiplier)
				{
					num += attributeModifier.Value;
				}
				else
				{
					num2 += attributeModifier.Value;
				}
			}
			if (num2 != 0f)
			{
				num += Mathf.Abs(num) * num2;
			}
			return num;
		}

		// Token: 0x0600E870 RID: 59504 RVA: 0x004C1068 File Offset: 0x004BF268
		public static float GetTotalValue(Attribute attribute, List<AttributeModifier> modifiers)
		{
			float num = attribute.BaseValue;
			float num2 = 0f;
			for (int num3 = 0; num3 != modifiers.Count; num3++)
			{
				AttributeModifier attributeModifier = modifiers[num3];
				if (!attributeModifier.UIOnly)
				{
					if (!attributeModifier.IsMultiplier)
					{
						num += attributeModifier.Value;
					}
					else
					{
						num2 += attributeModifier.Value;
					}
				}
			}
			if (num2 != 0f)
			{
				num += Mathf.Abs(num) * num2;
			}
			return num;
		}

		// Token: 0x0600E871 RID: 59505 RVA: 0x004C10D4 File Offset: 0x004BF2D4
		public float GetModifierContribution(AttributeModifier testModifier)
		{
			if (!testModifier.IsMultiplier)
			{
				return testModifier.Value;
			}
			float num = this.Attribute.BaseValue;
			for (int num2 = 0; num2 != this.Modifiers.Count; num2++)
			{
				AttributeModifier attributeModifier = this.Modifiers[num2];
				if (!attributeModifier.IsMultiplier)
				{
					num += attributeModifier.Value;
				}
			}
			return num * testModifier.Value;
		}

		// Token: 0x0600E872 RID: 59506 RVA: 0x0013B7B8 File Offset: 0x001399B8
		public AttributeInstance(GameObject game_object, Attribute attribute) : base(game_object, attribute)
		{
			DebugUtil.Assert(attribute != null);
			this.Attribute = attribute;
		}

		// Token: 0x0600E873 RID: 59507 RVA: 0x0013B7D2 File Offset: 0x001399D2
		public void Add(AttributeModifier modifier)
		{
			this.Modifiers.Add(modifier);
			if (this.OnDirty != null)
			{
				this.OnDirty();
			}
		}

		// Token: 0x0600E874 RID: 59508 RVA: 0x004C1138 File Offset: 0x004BF338
		public void Remove(AttributeModifier modifier)
		{
			int i = 0;
			while (i < this.Modifiers.Count)
			{
				if (this.Modifiers[i] == modifier)
				{
					this.Modifiers.RemoveAt(i);
					if (this.OnDirty != null)
					{
						this.OnDirty();
						return;
					}
					break;
				}
				else
				{
					i++;
				}
			}
		}

		// Token: 0x0600E875 RID: 59509 RVA: 0x0013B7F4 File Offset: 0x001399F4
		public void ClearModifiers()
		{
			if (this.Modifiers.Count > 0)
			{
				this.Modifiers.Clear();
				if (this.OnDirty != null)
				{
					this.OnDirty();
				}
			}
		}

		// Token: 0x0600E876 RID: 59510 RVA: 0x0013B822 File Offset: 0x00139A22
		public string GetDescription()
		{
			return string.Format(DUPLICANTS.ATTRIBUTES.VALUE, this.Name, this.GetFormattedValue());
		}

		// Token: 0x0600E877 RID: 59511 RVA: 0x0013B83F File Offset: 0x00139A3F
		public string GetFormattedValue()
		{
			return this.Attribute.formatter.GetFormattedAttribute(this);
		}

		// Token: 0x0600E878 RID: 59512 RVA: 0x0013B852 File Offset: 0x00139A52
		public string GetAttributeValueTooltip()
		{
			return this.Attribute.GetTooltip(this);
		}

		// Token: 0x0400E43B RID: 58427
		public Attribute Attribute;

		// Token: 0x0400E43C RID: 58428
		public System.Action OnDirty;

		// Token: 0x0400E43D RID: 58429
		public ArrayRef<AttributeModifier> Modifiers;

		// Token: 0x0400E43E RID: 58430
		public bool hide;
	}
}
