using System;
using System.Collections.Generic;
using Klei.Actions;
using UnityEngine;

namespace Klei.Input
{
	// Token: 0x02003BA5 RID: 15269
	[CreateAssetMenu(fileName = "InterfaceToolConfig", menuName = "Klei/Interface Tools/Config")]
	public class InterfaceToolConfig : ScriptableObject
	{
		// Token: 0x17000C36 RID: 3126
		// (get) Token: 0x0600EB46 RID: 60230 RVA: 0x0013D4B8 File Offset: 0x0013B6B8
		public DigAction DigAction
		{
			get
			{
				return ActionFactory<DigToolActionFactory, DigAction, DigToolActionFactory.Actions>.GetOrCreateAction(this.digAction);
			}
		}

		// Token: 0x17000C37 RID: 3127
		// (get) Token: 0x0600EB47 RID: 60231 RVA: 0x0013D4C5 File Offset: 0x0013B6C5
		public int Priority
		{
			get
			{
				return this.priority;
			}
		}

		// Token: 0x17000C38 RID: 3128
		// (get) Token: 0x0600EB48 RID: 60232 RVA: 0x0013D4CD File Offset: 0x0013B6CD
		public global::Action InputAction
		{
			get
			{
				return (global::Action)Enum.Parse(typeof(global::Action), this.inputAction);
			}
		}

		// Token: 0x0400E66D RID: 58989
		[SerializeField]
		private DigToolActionFactory.Actions digAction;

		// Token: 0x0400E66E RID: 58990
		public static InterfaceToolConfig.Comparer ConfigComparer = new InterfaceToolConfig.Comparer();

		// Token: 0x0400E66F RID: 58991
		[SerializeField]
		[Tooltip("Defines which config will take priority should multiple configs be activated\n0 is the lower bound for this value.")]
		private int priority;

		// Token: 0x0400E670 RID: 58992
		[SerializeField]
		[Tooltip("This will serve as a key for activating different configs. Currently, these Actionsare how we indicate that different input modes are desired.\nAssigning Action.Invalid to this field will indicate that this is the \"default\" config")]
		private string inputAction = global::Action.Invalid.ToString();

		// Token: 0x02003BA6 RID: 15270
		public class Comparer : IComparer<InterfaceToolConfig>
		{
			// Token: 0x0600EB4B RID: 60235 RVA: 0x0013D4F5 File Offset: 0x0013B6F5
			public int Compare(InterfaceToolConfig lhs, InterfaceToolConfig rhs)
			{
				if (lhs.Priority == rhs.Priority)
				{
					return 0;
				}
				if (lhs.Priority <= rhs.Priority)
				{
					return -1;
				}
				return 1;
			}
		}
	}
}
