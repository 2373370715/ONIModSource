using KSerialization;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/Baggable")]
public class Baggable : KMonoBehaviour
{
	[SerializeField]
	private KAnimFile minionAnimOverride;

	public bool mustStandOntopOfTrapForPickup;

	[Serialize]
	public bool wrangled;

	[Serialize]
	public bool keepWrangledNextTimeRemovedFromStorage;

	public bool useGunForPickup;

	private static readonly EventSystem.IntraObjectHandler<Baggable> OnStoreDelegate = new EventSystem.IntraObjectHandler<Baggable>(delegate(Baggable component, object data)
	{
		component.OnStore(data);
	});

	public const string DEFAULT_BAGGED_ANIM_NAME = "trussed";

	protected override void OnSpawn()
	{
		base.OnSpawn();
		minionAnimOverride = Assets.GetAnim("anim_restrain_creature_kanim");
		Pickupable pickupable = base.gameObject.AddOrGet<Pickupable>();
		pickupable.workAnims = new HashedString[2]
		{
			new HashedString("capture"),
			new HashedString("pickup")
		};
		pickupable.workAnimPlayMode = KAnim.PlayMode.Once;
		pickupable.workingPstComplete = null;
		pickupable.workingPstFailed = null;
		pickupable.overrideAnims = new KAnimFile[1] { minionAnimOverride };
		pickupable.trackOnPickup = false;
		pickupable.useGunforPickup = useGunForPickup;
		pickupable.synchronizeAnims = false;
		pickupable.SetWorkTime(3f);
		if (mustStandOntopOfTrapForPickup)
		{
			pickupable.SetOffsets(new CellOffset[2]
			{
				default(CellOffset),
				new CellOffset(0, -1)
			});
		}
		Subscribe(856640610, OnStoreDelegate);
		if (base.transform.parent != null)
		{
			if (base.transform.parent.GetComponent<Trap>() != null || base.transform.parent.GetSMI<ReusableTrap.Instance>() != null)
			{
				GetComponent<KBatchedAnimController>().enabled = true;
			}
			if (base.transform.parent.GetComponent<EggIncubator>() != null)
			{
				wrangled = true;
			}
		}
		if (wrangled)
		{
			SetWrangled();
		}
	}

	private void OnStore(object data)
	{
		Storage storage = data as Storage;
		if (storage != null || (data != null && (bool)data))
		{
			base.gameObject.AddTag(GameTags.Creatures.Bagged);
			if ((bool)storage && storage.IsPrefabID(GameTags.Minion))
			{
				SetVisible(visible: false);
			}
		}
		else
		{
			if (!keepWrangledNextTimeRemovedFromStorage)
			{
				Free();
			}
			keepWrangledNextTimeRemovedFromStorage = false;
		}
	}

	private void SetVisible(bool visible)
	{
		KAnimControllerBase component = base.gameObject.GetComponent<KAnimControllerBase>();
		if (component != null && component.enabled != visible)
		{
			component.enabled = visible;
		}
		KSelectable component2 = base.gameObject.GetComponent<KSelectable>();
		if (component2 != null && component2.enabled != visible)
		{
			component2.enabled = visible;
		}
	}

	public static string GetBaggedAnimName(GameObject baggableObject)
	{
		string result = "trussed";
		Pickupable pickupable = baggableObject.AddOrGet<Pickupable>();
		if (pickupable != null && pickupable.storage != null)
		{
			IBaggedStateAnimationInstructions component = pickupable.storage.GetComponent<IBaggedStateAnimationInstructions>();
			if (component != null)
			{
				string baggedAnimationName = component.GetBaggedAnimationName();
				if (baggedAnimationName != null)
				{
					result = baggedAnimationName;
				}
			}
		}
		return result;
	}

	public void SetWrangled()
	{
		wrangled = true;
		Navigator component = GetComponent<Navigator>();
		if ((bool)component && component.IsValidNavType(NavType.Floor))
		{
			component.SetCurrentNavType(NavType.Floor);
		}
		base.gameObject.AddTag(GameTags.Creatures.Bagged);
		GetComponent<KAnimControllerBase>().Play(GetBaggedAnimName(base.gameObject), KAnim.PlayMode.Loop);
	}

	public void Free()
	{
		base.gameObject.RemoveTag(GameTags.Creatures.Bagged);
		wrangled = false;
		SetVisible(visible: true);
	}
}
