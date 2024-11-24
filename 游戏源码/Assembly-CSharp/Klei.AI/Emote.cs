using System.Collections.Generic;

namespace Klei.AI;

public class Emote : Resource
{
	private HashedString animSetName = null;

	private KAnimFile animSet;

	private List<EmoteStep> emoteSteps = new List<EmoteStep>();

	public int StepCount
	{
		get
		{
			if (emoteSteps != null)
			{
				return emoteSteps.Count;
			}
			return 0;
		}
	}

	public KAnimFile AnimSet
	{
		get
		{
			if (animSetName != HashedString.Invalid && animSet == null)
			{
				animSet = Assets.GetAnim(animSetName);
			}
			return animSet;
		}
	}

	public EmoteStep this[int stepIdx]
	{
		get
		{
			if (!IsValidStep(stepIdx))
			{
				return null;
			}
			return emoteSteps[stepIdx];
		}
	}

	public Emote(ResourceSet parent, string emoteId, EmoteStep[] defaultSteps, string animSetName = null)
		: base(emoteId, parent)
	{
		emoteSteps.AddRange(defaultSteps);
		this.animSetName = animSetName;
	}

	public bool IsValidForController(KBatchedAnimController animController)
	{
		bool flag = true;
		int num = 0;
		while (flag && num < StepCount)
		{
			flag = animController.HasAnimation(emoteSteps[num].anim);
			num++;
		}
		KAnimFileData kAnimFileData = ((animSet == null) ? null : animSet.GetData());
		int num2 = 0;
		while (kAnimFileData != null && flag && num2 < StepCount)
		{
			bool flag2 = false;
			int num3 = 0;
			while (!flag2 && num3 < kAnimFileData.animCount)
			{
				flag2 = kAnimFileData.GetAnim(num2).id == emoteSteps[num2].anim;
				num3++;
			}
			flag = flag2;
			num2++;
		}
		return flag;
	}

	public void ApplyAnimOverrides(KBatchedAnimController animController, KAnimFile overrideSet)
	{
		KAnimFile kAnimFile = ((overrideSet != null) ? overrideSet : AnimSet);
		if (!(kAnimFile == null) && !(animController == null))
		{
			animController.AddAnimOverrides(kAnimFile);
		}
	}

	public void RemoveAnimOverrides(KBatchedAnimController animController, KAnimFile overrideSet)
	{
		KAnimFile kAnimFile = ((overrideSet != null) ? overrideSet : AnimSet);
		if (!(kAnimFile == null) && !(animController == null))
		{
			animController.RemoveAnimOverrides(kAnimFile);
		}
	}

	public void CollectStepAnims(out HashedString[] emoteAnims, int iterations)
	{
		emoteAnims = new HashedString[emoteSteps.Count * iterations];
		for (int i = 0; i < emoteAnims.Length; i++)
		{
			emoteAnims[i] = emoteSteps[i % emoteSteps.Count].anim;
		}
	}

	public bool IsValidStep(int stepIdx)
	{
		if (stepIdx >= 0)
		{
			return stepIdx < emoteSteps.Count;
		}
		return false;
	}

	public int GetStepIndex(HashedString animName)
	{
		int i = 0;
		bool condition = false;
		for (; i < emoteSteps.Count; i++)
		{
			if (emoteSteps[i].anim == animName)
			{
				condition = true;
				break;
			}
		}
		Debug.Assert(condition, $"Could not find emote step {animName} for emote {Id}!");
		return i;
	}
}
