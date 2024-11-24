using System;
using System.Collections.Generic;
using Klei.Actions;
using UnityEngine;

namespace Klei.Input;

[CreateAssetMenu(fileName = "InterfaceToolConfig", menuName = "Klei/Interface Tools/Config")]
public class InterfaceToolConfig : ScriptableObject
{
	public class Comparer : IComparer<InterfaceToolConfig>
	{
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

	[SerializeField]
	private DigToolActionFactory.Actions digAction;

	public static Comparer ConfigComparer = new Comparer();

	[SerializeField]
	[Tooltip("Defines which config will take priority should multiple configs be activated\n0 is the lower bound for this value.")]
	private int priority;

	[SerializeField]
	[Tooltip("This will serve as a key for activating different configs. Currently, these Actionsare how we indicate that different input modes are desired.\nAssigning Action.Invalid to this field will indicate that this is the \"default\" config")]
	private string inputAction = Action.Invalid.ToString();

	public DigAction DigAction => ActionFactory<DigToolActionFactory, DigAction, DigToolActionFactory.Actions>.GetOrCreateAction(digAction);

	public int Priority => priority;

	public Action InputAction => (Action)Enum.Parse(typeof(Action), inputAction);
}
