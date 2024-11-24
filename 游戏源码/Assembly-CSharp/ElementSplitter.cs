using System;
using UnityEngine;

public struct ElementSplitter
{
	public PrimaryElement primaryElement;

	public Func<Pickupable, float, Pickupable> onTakeCB;

	public Func<Pickupable, bool> canAbsorbCB;

	public KPrefabID kPrefabID;

	public ElementSplitter(GameObject go)
	{
		primaryElement = go.GetComponent<PrimaryElement>();
		kPrefabID = go.GetComponent<KPrefabID>();
		onTakeCB = null;
		canAbsorbCB = null;
	}
}
