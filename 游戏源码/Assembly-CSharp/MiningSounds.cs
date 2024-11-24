using FMODUnity;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/MiningSounds")]
public class MiningSounds : KMonoBehaviour
{
	private static HashedString HASH_PERCENTCOMPLETE = "percentComplete";

	[MyCmpGet]
	private LoopingSounds loopingSounds;

	private FMODAsset miningSound;

	private EventReference miningSoundEvent;

	private static readonly EventSystem.IntraObjectHandler<MiningSounds> OnStartMiningSoundDelegate = new EventSystem.IntraObjectHandler<MiningSounds>(delegate(MiningSounds component, object data)
	{
		component.OnStartMiningSound(data);
	});

	private static readonly EventSystem.IntraObjectHandler<MiningSounds> OnStopMiningSoundDelegate = new EventSystem.IntraObjectHandler<MiningSounds>(delegate(MiningSounds component, object data)
	{
		component.OnStopMiningSound(data);
	});

	protected override void OnPrefabInit()
	{
		Subscribe(-1762453998, OnStartMiningSoundDelegate);
		Subscribe(939543986, OnStopMiningSoundDelegate);
	}

	private void OnStartMiningSound(object data)
	{
		if (!(miningSound == null) || !(data is Element element))
		{
			return;
		}
		string text = element.substance.GetMiningSound();
		if (text != null && !(text == ""))
		{
			text = "Mine_" + text;
			string sound = GlobalAssets.GetSound(text);
			miningSoundEvent = RuntimeManager.PathToEventReference(sound);
			if (!miningSoundEvent.IsNull)
			{
				loopingSounds.StartSound(miningSoundEvent);
			}
		}
	}

	private void OnStopMiningSound(object data)
	{
		if (!miningSoundEvent.IsNull)
		{
			loopingSounds.StopSound(miningSoundEvent);
			miningSound = null;
		}
	}

	public void SetPercentComplete(float progress)
	{
		if (!miningSoundEvent.IsNull)
		{
			loopingSounds.SetParameter(miningSoundEvent, HASH_PERCENTCOMPLETE, progress);
		}
	}
}
