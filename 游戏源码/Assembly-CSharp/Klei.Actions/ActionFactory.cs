using System;
using System.Collections.Generic;

namespace Klei.Actions;

public class ActionFactory<ActionFactoryType, ActionType, EnumType> where ActionFactoryType : ActionFactory<ActionFactoryType, ActionType, EnumType>
{
	private static Dictionary<EnumType, ActionType> actionInstances = new Dictionary<EnumType, ActionType>();

	private static ActionFactoryType actionFactory = null;

	public static ActionType GetOrCreateAction(EnumType actionType)
	{
		if (!actionInstances.TryGetValue(actionType, out var value))
		{
			EnsureFactoryInstance();
			value = (actionInstances[actionType] = actionFactory.CreateAction(actionType));
		}
		return value;
	}

	private static void EnsureFactoryInstance()
	{
		if (actionFactory == null)
		{
			actionFactory = Activator.CreateInstance(typeof(ActionFactoryType)) as ActionFactoryType;
		}
	}

	protected virtual ActionType CreateAction(EnumType actionType)
	{
		throw new InvalidOperationException("Can not call InterfaceToolActionFactory<T1, T2>.CreateAction()! This function must be called from a deriving class!");
	}
}
