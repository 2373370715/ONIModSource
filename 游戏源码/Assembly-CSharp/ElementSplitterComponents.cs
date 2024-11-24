using System;
using UnityEngine;

// Token: 0x02000A46 RID: 2630
public class ElementSplitterComponents : KGameObjectComponentManager<ElementSplitter>
{
	// Token: 0x06003052 RID: 12370 RVA: 0x000BF6D1 File Offset: 0x000BD8D1
	public HandleVector<int>.Handle Add(GameObject go)
	{
		return base.Add(go, new ElementSplitter(go));
	}

	// Token: 0x06003053 RID: 12371 RVA: 0x001FB6E8 File Offset: 0x001F98E8
	protected override void OnPrefabInit(HandleVector<int>.Handle handle)
	{
		ElementSplitter data = base.GetData(handle);
		Pickupable component = data.primaryElement.GetComponent<Pickupable>();
		Func<Pickupable, float, Pickupable> func = (Pickupable obj, float amount) => ElementSplitterComponents.OnTake(obj, handle, amount);
		component.OnTake = (Func<Pickupable, float, Pickupable>)Delegate.Combine(component.OnTake, func);
		Func<Pickupable, bool> func2 = delegate(Pickupable other)
		{
			HandleVector<int>.Handle handle2 = this.GetHandle(other.gameObject);
			return ElementSplitterComponents.CanFirstAbsorbSecond(handle, handle2);
		};
		component.CanAbsorb = (Func<Pickupable, bool>)Delegate.Combine(component.CanAbsorb, func2);
		component.absorbable = true;
		data.onTakeCB = func;
		data.canAbsorbCB = func2;
		base.SetData(handle, data);
	}

	// Token: 0x06003054 RID: 12372 RVA: 0x000A5E40 File Offset: 0x000A4040
	protected override void OnSpawn(HandleVector<int>.Handle handle)
	{
	}

	// Token: 0x06003055 RID: 12373 RVA: 0x001FB78C File Offset: 0x001F998C
	protected override void OnCleanUp(HandleVector<int>.Handle handle)
	{
		ElementSplitter data = base.GetData(handle);
		if (data.primaryElement != null)
		{
			Pickupable component = data.primaryElement.GetComponent<Pickupable>();
			if (component != null)
			{
				Pickupable pickupable = component;
				pickupable.OnTake = (Func<Pickupable, float, Pickupable>)Delegate.Remove(pickupable.OnTake, data.onTakeCB);
				Pickupable pickupable2 = component;
				pickupable2.CanAbsorb = (Func<Pickupable, bool>)Delegate.Remove(pickupable2.CanAbsorb, data.canAbsorbCB);
			}
		}
	}

	// Token: 0x06003056 RID: 12374 RVA: 0x001FB7FC File Offset: 0x001F99FC
	private static bool CanFirstAbsorbSecond(HandleVector<int>.Handle first, HandleVector<int>.Handle second)
	{
		if (first == HandleVector<int>.InvalidHandle || second == HandleVector<int>.InvalidHandle)
		{
			return false;
		}
		ElementSplitter data = GameComps.ElementSplitters.GetData(first);
		ElementSplitter data2 = GameComps.ElementSplitters.GetData(second);
		return data.primaryElement.ElementID == data2.primaryElement.ElementID && data.primaryElement.Units + data2.primaryElement.Units < 25000f && !data.kPrefabID.HasTag(GameTags.MarkedForMove) && !data2.kPrefabID.HasTag(GameTags.MarkedForMove);
	}

	// Token: 0x06003057 RID: 12375 RVA: 0x001FB89C File Offset: 0x001F9A9C
	private static Pickupable OnTake(Pickupable pickupable, HandleVector<int>.Handle handle, float amount)
	{
		ElementSplitter data = GameComps.ElementSplitters.GetData(handle);
		Storage storage = pickupable.storage;
		PrimaryElement primaryElement = pickupable.PrimaryElement;
		Pickupable component = primaryElement.Element.substance.SpawnResource(pickupable.transform.GetPosition(), amount, primaryElement.Temperature, byte.MaxValue, 0, true, false, false).GetComponent<Pickupable>();
		pickupable.TotalAmount -= amount;
		component.Trigger(1335436905, pickupable);
		ElementSplitterComponents.CopyRenderSettings(pickupable.GetComponent<KBatchedAnimController>(), component.GetComponent<KBatchedAnimController>());
		if (storage != null)
		{
			storage.Trigger(-1697596308, data.primaryElement.gameObject);
			storage.Trigger(-778359855, storage);
		}
		return component;
	}

	// Token: 0x06003058 RID: 12376 RVA: 0x000BF6E0 File Offset: 0x000BD8E0
	private static void CopyRenderSettings(KBatchedAnimController src, KBatchedAnimController dest)
	{
		if (src != null && dest != null)
		{
			dest.OverlayColour = src.OverlayColour;
		}
	}

	// Token: 0x0400208E RID: 8334
	private const float MAX_STACK_SIZE = 25000f;
}
