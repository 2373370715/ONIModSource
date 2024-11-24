using System.Collections.Generic;
using UnityEngine;

public class FossilDigsiteLampLight : Light2D
{
	private static readonly EventSystem.IntraObjectHandler<FossilDigsiteLampLight> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<FossilDigsiteLampLight>(delegate(FossilDigsiteLampLight light, object data)
	{
		if (light.independent)
		{
			light.enabled = (bool)data;
		}
	});

	public bool independent { get; private set; }

	protected override void OnPrefabInit()
	{
		Subscribe(-592767678, OnOperationalChangedDelegate);
		base.IntensityAnimation = 1f;
	}

	public void SetIndependentState(bool isIndependent, bool checkOperational = true)
	{
		independent = isIndependent;
		Operational component = GetComponent<Operational>();
		if (component != null && independent && checkOperational && base.enabled != component.IsOperational)
		{
			base.enabled = component.IsOperational;
		}
	}

	public override List<Descriptor> GetDescriptors(GameObject go)
	{
		if (independent || base.enabled)
		{
			return base.GetDescriptors(go);
		}
		return new List<Descriptor>();
	}
}
