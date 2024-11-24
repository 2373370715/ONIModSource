using System;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02003B81 RID: 15233
	public static class ModifiersExtensions
	{
		// Token: 0x0600EA9A RID: 60058 RVA: 0x0013CD8A File Offset: 0x0013AF8A
		public static Attributes GetAttributes(this KMonoBehaviour cmp)
		{
			return cmp.gameObject.GetAttributes();
		}

		// Token: 0x0600EA9B RID: 60059 RVA: 0x004CA7A0 File Offset: 0x004C89A0
		public static Attributes GetAttributes(this GameObject go)
		{
			Modifiers component = go.GetComponent<Modifiers>();
			if (component != null)
			{
				return component.attributes;
			}
			return null;
		}

		// Token: 0x0600EA9C RID: 60060 RVA: 0x0013CD97 File Offset: 0x0013AF97
		public static Amounts GetAmounts(this KMonoBehaviour cmp)
		{
			if (cmp is Modifiers)
			{
				return ((Modifiers)cmp).amounts;
			}
			return cmp.gameObject.GetAmounts();
		}

		// Token: 0x0600EA9D RID: 60061 RVA: 0x004CA7C8 File Offset: 0x004C89C8
		public static Amounts GetAmounts(this GameObject go)
		{
			Modifiers component = go.GetComponent<Modifiers>();
			if (component != null)
			{
				return component.amounts;
			}
			return null;
		}

		// Token: 0x0600EA9E RID: 60062 RVA: 0x0013CDB8 File Offset: 0x0013AFB8
		public static Sicknesses GetSicknesses(this KMonoBehaviour cmp)
		{
			return cmp.gameObject.GetSicknesses();
		}

		// Token: 0x0600EA9F RID: 60063 RVA: 0x004CA7F0 File Offset: 0x004C89F0
		public static Sicknesses GetSicknesses(this GameObject go)
		{
			Modifiers component = go.GetComponent<Modifiers>();
			if (component != null)
			{
				return component.sicknesses;
			}
			return null;
		}
	}
}
