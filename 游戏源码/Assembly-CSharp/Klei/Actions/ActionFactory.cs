using System;
using System.Collections.Generic;

namespace Klei.Actions
{
	// Token: 0x02003BAD RID: 15277
	public class ActionFactory<ActionFactoryType, ActionType, EnumType> where ActionFactoryType : ActionFactory<ActionFactoryType, ActionType, EnumType>
	{
		// Token: 0x0600EB60 RID: 60256 RVA: 0x004CCD70 File Offset: 0x004CAF70
		public static ActionType GetOrCreateAction(EnumType actionType)
		{
			ActionType result;
			if (!ActionFactory<ActionFactoryType, ActionType, EnumType>.actionInstances.TryGetValue(actionType, out result))
			{
				ActionFactory<ActionFactoryType, ActionType, EnumType>.EnsureFactoryInstance();
				result = (ActionFactory<ActionFactoryType, ActionType, EnumType>.actionInstances[actionType] = ActionFactory<ActionFactoryType, ActionType, EnumType>.actionFactory.CreateAction(actionType));
			}
			return result;
		}

		// Token: 0x0600EB61 RID: 60257 RVA: 0x0013D5CA File Offset: 0x0013B7CA
		private static void EnsureFactoryInstance()
		{
			if (ActionFactory<ActionFactoryType, ActionType, EnumType>.actionFactory != null)
			{
				return;
			}
			ActionFactory<ActionFactoryType, ActionType, EnumType>.actionFactory = (Activator.CreateInstance(typeof(ActionFactoryType)) as ActionFactoryType);
		}

		// Token: 0x0600EB62 RID: 60258 RVA: 0x0013D5F7 File Offset: 0x0013B7F7
		protected virtual ActionType CreateAction(EnumType actionType)
		{
			throw new InvalidOperationException("Can not call InterfaceToolActionFactory<T1, T2>.CreateAction()! This function must be called from a deriving class!");
		}

		// Token: 0x0400E675 RID: 58997
		private static Dictionary<EnumType, ActionType> actionInstances = new Dictionary<EnumType, ActionType>();

		// Token: 0x0400E676 RID: 58998
		private static ActionFactoryType actionFactory = default(ActionFactoryType);
	}
}
