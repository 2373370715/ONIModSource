using System;
using System.Collections.Generic;

public interface IDispenser
{
	event System.Action OnStopWorkEvent;

	List<Tag> DispensedItems();

	Tag SelectedItem();

	void SelectItem(Tag tag);

	void OnOrderDispense();

	void OnCancelDispense();

	bool HasOpenChore();
}
