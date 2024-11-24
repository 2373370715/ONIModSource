using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02003B0B RID: 15115
	public class Attributes
	{
		// Token: 0x0600E8A4 RID: 59556 RVA: 0x0013B998 File Offset: 0x00139B98
		public IEnumerator<AttributeInstance> GetEnumerator()
		{
			return this.AttributeTable.GetEnumerator();
		}

		// Token: 0x17000C1B RID: 3099
		// (get) Token: 0x0600E8A5 RID: 59557 RVA: 0x0013B9AA File Offset: 0x00139BAA
		public int Count
		{
			get
			{
				return this.AttributeTable.Count;
			}
		}

		// Token: 0x0600E8A6 RID: 59558 RVA: 0x0013B9B7 File Offset: 0x00139BB7
		public Attributes(GameObject game_object)
		{
			this.gameObject = game_object;
		}

		// Token: 0x0600E8A7 RID: 59559 RVA: 0x004C1968 File Offset: 0x004BFB68
		public AttributeInstance Add(Attribute attribute)
		{
			AttributeInstance attributeInstance = this.Get(attribute.Id);
			if (attributeInstance == null)
			{
				attributeInstance = new AttributeInstance(this.gameObject, attribute);
				this.AttributeTable.Add(attributeInstance);
			}
			return attributeInstance;
		}

		// Token: 0x0600E8A8 RID: 59560 RVA: 0x004C19A0 File Offset: 0x004BFBA0
		public void Add(AttributeModifier modifier)
		{
			AttributeInstance attributeInstance = this.Get(modifier.AttributeId);
			if (attributeInstance != null)
			{
				attributeInstance.Add(modifier);
			}
		}

		// Token: 0x0600E8A9 RID: 59561 RVA: 0x004C19C4 File Offset: 0x004BFBC4
		public void Remove(AttributeModifier modifier)
		{
			if (modifier == null)
			{
				return;
			}
			AttributeInstance attributeInstance = this.Get(modifier.AttributeId);
			if (attributeInstance != null)
			{
				attributeInstance.Remove(modifier);
			}
		}

		// Token: 0x0600E8AA RID: 59562 RVA: 0x004C19EC File Offset: 0x004BFBEC
		public float GetValuePercent(string attribute_id)
		{
			float result = 1f;
			AttributeInstance attributeInstance = this.Get(attribute_id);
			if (attributeInstance != null)
			{
				result = attributeInstance.GetTotalValue() / attributeInstance.GetBaseValue();
			}
			else
			{
				global::Debug.LogError("Could not find attribute " + attribute_id);
			}
			return result;
		}

		// Token: 0x0600E8AB RID: 59563 RVA: 0x004C1A2C File Offset: 0x004BFC2C
		public AttributeInstance Get(string attribute_id)
		{
			for (int i = 0; i < this.AttributeTable.Count; i++)
			{
				if (this.AttributeTable[i].Id == attribute_id)
				{
					return this.AttributeTable[i];
				}
			}
			return null;
		}

		// Token: 0x0600E8AC RID: 59564 RVA: 0x0013B9D1 File Offset: 0x00139BD1
		public AttributeInstance Get(Attribute attribute)
		{
			return this.Get(attribute.Id);
		}

		// Token: 0x0600E8AD RID: 59565 RVA: 0x004C1A78 File Offset: 0x004BFC78
		public float GetValue(string id)
		{
			float result = 0f;
			AttributeInstance attributeInstance = this.Get(id);
			if (attributeInstance != null)
			{
				result = attributeInstance.GetTotalValue();
			}
			else
			{
				global::Debug.LogError("Could not find attribute " + id);
			}
			return result;
		}

		// Token: 0x0600E8AE RID: 59566 RVA: 0x004C1AB0 File Offset: 0x004BFCB0
		public AttributeInstance GetProfession()
		{
			AttributeInstance attributeInstance = null;
			foreach (AttributeInstance attributeInstance2 in this)
			{
				if (attributeInstance2.modifier.IsProfession)
				{
					if (attributeInstance == null)
					{
						attributeInstance = attributeInstance2;
					}
					else if (attributeInstance.GetTotalValue() < attributeInstance2.GetTotalValue())
					{
						attributeInstance = attributeInstance2;
					}
				}
			}
			return attributeInstance;
		}

		// Token: 0x0600E8AF RID: 59567 RVA: 0x004C1B18 File Offset: 0x004BFD18
		public string GetProfessionString(bool longform = true)
		{
			AttributeInstance profession = this.GetProfession();
			if ((int)profession.GetTotalValue() == 0)
			{
				return string.Format(longform ? UI.ATTRIBUTELEVEL : UI.ATTRIBUTELEVEL_SHORT, 0, DUPLICANTS.ATTRIBUTES.UNPROFESSIONAL_NAME);
			}
			return string.Format(longform ? UI.ATTRIBUTELEVEL : UI.ATTRIBUTELEVEL_SHORT, (int)profession.GetTotalValue(), profession.modifier.ProfessionName);
		}

		// Token: 0x0600E8B0 RID: 59568 RVA: 0x004C1B8C File Offset: 0x004BFD8C
		public string GetProfessionDescriptionString()
		{
			AttributeInstance profession = this.GetProfession();
			if ((int)profession.GetTotalValue() == 0)
			{
				return DUPLICANTS.ATTRIBUTES.UNPROFESSIONAL_DESC;
			}
			return string.Format(DUPLICANTS.ATTRIBUTES.PROFESSION_DESC, profession.modifier.Name);
		}

		// Token: 0x0400E451 RID: 58449
		public List<AttributeInstance> AttributeTable = new List<AttributeInstance>();

		// Token: 0x0400E452 RID: 58450
		public GameObject gameObject;
	}
}
