using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02001990 RID: 6544
[SerializationConfig(MemberSerialization.OptIn)]
public class RequireAttachedComponent : ProcessCondition
{
	// Token: 0x170008FB RID: 2299
	// (get) Token: 0x0600886B RID: 34923 RVA: 0x000F937B File Offset: 0x000F757B
	// (set) Token: 0x0600886C RID: 34924 RVA: 0x000F9383 File Offset: 0x000F7583
	public Type RequiredType
	{
		get
		{
			return this.requiredType;
		}
		set
		{
			this.requiredType = value;
			this.typeNameString = this.requiredType.Name;
		}
	}

	// Token: 0x0600886D RID: 34925 RVA: 0x000F939D File Offset: 0x000F759D
	public RequireAttachedComponent(AttachableBuilding myAttachable, Type required_type, string type_name_string)
	{
		this.myAttachable = myAttachable;
		this.requiredType = required_type;
		this.typeNameString = type_name_string;
	}

	// Token: 0x0600886E RID: 34926 RVA: 0x00353D38 File Offset: 0x00351F38
	public override ProcessCondition.Status EvaluateCondition()
	{
		if (this.myAttachable != null)
		{
			using (List<GameObject>.Enumerator enumerator = AttachableBuilding.GetAttachedNetwork(this.myAttachable).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.GetComponent(this.requiredType))
					{
						return ProcessCondition.Status.Ready;
					}
				}
			}
			return ProcessCondition.Status.Failure;
		}
		return ProcessCondition.Status.Failure;
	}

	// Token: 0x0600886F RID: 34927 RVA: 0x000F93BA File Offset: 0x000F75BA
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		return this.typeNameString;
	}

	// Token: 0x06008870 RID: 34928 RVA: 0x000F93C6 File Offset: 0x000F75C6
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		if (status == ProcessCondition.Status.Ready)
		{
			return string.Format(UI.STARMAP.LAUNCHCHECKLIST.INSTALLED_TOOLTIP, this.typeNameString.ToLower());
		}
		return string.Format(UI.STARMAP.LAUNCHCHECKLIST.MISSING_TOOLTIP, this.typeNameString.ToLower());
	}

	// Token: 0x06008871 RID: 34929 RVA: 0x000A65EC File Offset: 0x000A47EC
	public override bool ShowInUI()
	{
		return true;
	}

	// Token: 0x040066A7 RID: 26279
	private string typeNameString;

	// Token: 0x040066A8 RID: 26280
	private Type requiredType;

	// Token: 0x040066A9 RID: 26281
	private AttachableBuilding myAttachable;
}
