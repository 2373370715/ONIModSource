using System;
using Klei.Input;

namespace Klei.Actions;

public class DigToolActionFactory : ActionFactory<DigToolActionFactory, DigAction, DigToolActionFactory.Actions>
{
	public enum Actions
	{
		MarkCell = 145163119,
		Immediate = -1044758767,
		ClearCell = -1011242513,
		Count = -1427607121
	}

	protected override DigAction CreateAction(Actions action)
	{
		return action switch
		{
			Actions.MarkCell => new MarkCellDigAction(), 
			Actions.Immediate => new ImmediateDigAction(), 
			Actions.ClearCell => new ClearCellDigAction(), 
			_ => throw new InvalidOperationException("Can not create DigAction 'Count'. Please provide a valid action."), 
		};
	}
}
