using System;
using UnityEngine;

// Token: 0x02001686 RID: 5766
public class OreSizeVisualizerComponents : KGameObjectComponentManager<OreSizeVisualizerData>
{
	// Token: 0x0600771E RID: 30494 RVA: 0x0030D02C File Offset: 0x0030B22C
	public HandleVector<int>.Handle Add(GameObject go)
	{
		HandleVector<int>.Handle handle = base.Add(go, new OreSizeVisualizerData(go));
		this.OnPrefabInit(handle);
		return handle;
	}

	// Token: 0x0600771F RID: 30495 RVA: 0x0030D050 File Offset: 0x0030B250
	public static HashedString GetAnimForMass(float mass)
	{
		for (int i = 0; i < OreSizeVisualizerComponents.MassTiers.Length; i++)
		{
			if (mass <= OreSizeVisualizerComponents.MassTiers[i].massRequired)
			{
				return OreSizeVisualizerComponents.MassTiers[i].animName;
			}
		}
		return HashedString.Invalid;
	}

	// Token: 0x06007720 RID: 30496 RVA: 0x0030D098 File Offset: 0x0030B298
	protected override void OnPrefabInit(HandleVector<int>.Handle handle)
	{
		Action<object> action = delegate(object ev_data)
		{
			OreSizeVisualizerComponents.OnMassChanged(handle, ev_data);
		};
		OreSizeVisualizerData data = base.GetData(handle);
		data.onMassChangedCB = action;
		data.primaryElement.Subscribe(-2064133523, action);
		data.primaryElement.Subscribe(1335436905, action);
		base.SetData(handle, data);
	}

	// Token: 0x06007721 RID: 30497 RVA: 0x0030D108 File Offset: 0x0030B308
	protected override void OnSpawn(HandleVector<int>.Handle handle)
	{
		OreSizeVisualizerData data = base.GetData(handle);
		OreSizeVisualizerComponents.OnMassChanged(handle, data.primaryElement.GetComponent<Pickupable>());
	}

	// Token: 0x06007722 RID: 30498 RVA: 0x0030D130 File Offset: 0x0030B330
	protected override void OnCleanUp(HandleVector<int>.Handle handle)
	{
		OreSizeVisualizerData data = base.GetData(handle);
		if (data.primaryElement != null)
		{
			Action<object> onMassChangedCB = data.onMassChangedCB;
			data.primaryElement.Unsubscribe(-2064133523, onMassChangedCB);
			data.primaryElement.Unsubscribe(1335436905, onMassChangedCB);
		}
	}

	// Token: 0x06007723 RID: 30499 RVA: 0x0030D17C File Offset: 0x0030B37C
	private static void OnMassChanged(HandleVector<int>.Handle handle, object other_data)
	{
		PrimaryElement primaryElement = GameComps.OreSizeVisualizers.GetData(handle).primaryElement;
		float num = primaryElement.Mass;
		if (other_data != null)
		{
			PrimaryElement component = ((Pickupable)other_data).GetComponent<PrimaryElement>();
			num += component.Mass;
		}
		OreSizeVisualizerComponents.MassTier massTier = default(OreSizeVisualizerComponents.MassTier);
		for (int i = 0; i < OreSizeVisualizerComponents.MassTiers.Length; i++)
		{
			if (num <= OreSizeVisualizerComponents.MassTiers[i].massRequired)
			{
				massTier = OreSizeVisualizerComponents.MassTiers[i];
				break;
			}
		}
		primaryElement.GetComponent<KBatchedAnimController>().Play(massTier.animName, KAnim.PlayMode.Once, 1f, 0f);
		KCircleCollider2D component2 = primaryElement.GetComponent<KCircleCollider2D>();
		if (component2 != null)
		{
			component2.radius = massTier.colliderRadius;
		}
		primaryElement.Trigger(1807976145, null);
	}

	// Token: 0x04005915 RID: 22805
	private static readonly OreSizeVisualizerComponents.MassTier[] MassTiers = new OreSizeVisualizerComponents.MassTier[]
	{
		new OreSizeVisualizerComponents.MassTier
		{
			animName = "idle1",
			massRequired = 50f,
			colliderRadius = 0.15f
		},
		new OreSizeVisualizerComponents.MassTier
		{
			animName = "idle2",
			massRequired = 600f,
			colliderRadius = 0.2f
		},
		new OreSizeVisualizerComponents.MassTier
		{
			animName = "idle3",
			massRequired = float.MaxValue,
			colliderRadius = 0.25f
		}
	};

	// Token: 0x02001687 RID: 5767
	private struct MassTier
	{
		// Token: 0x04005916 RID: 22806
		public HashedString animName;

		// Token: 0x04005917 RID: 22807
		public float massRequired;

		// Token: 0x04005918 RID: 22808
		public float colliderRadius;
	}
}
