using UnityEngine;

public readonly struct JoyResponseOutfitTarget
{
	public interface Implementation
	{
		Option<string> ReadFacadeId();

		void WriteFacadeId(Option<string> permitId);

		string GetMinionName();

		Personality GetPersonality();
	}

	public readonly struct MinionInstanceTarget : Implementation
	{
		public readonly GameObject minionInstance;

		public readonly WearableAccessorizer wearableAccessorizer;

		public MinionInstanceTarget(GameObject minionInstance)
		{
			this.minionInstance = minionInstance;
			wearableAccessorizer = minionInstance.GetComponent<WearableAccessorizer>();
		}

		public string GetMinionName()
		{
			return minionInstance.GetProperName();
		}

		public Personality GetPersonality()
		{
			return Db.Get().Personalities.Get(minionInstance.GetComponent<MinionIdentity>().personalityResourceId);
		}

		public Option<string> ReadFacadeId()
		{
			return wearableAccessorizer.GetJoyResponseId();
		}

		public void WriteFacadeId(Option<string> permitId)
		{
			wearableAccessorizer.SetJoyResponseId(permitId);
		}
	}

	public readonly struct PersonalityTarget : Implementation
	{
		public readonly Personality personality;

		public PersonalityTarget(Personality personality)
		{
			this.personality = personality;
		}

		public string GetMinionName()
		{
			return personality.Name;
		}

		public Personality GetPersonality()
		{
			return personality;
		}

		public Option<string> ReadFacadeId()
		{
			return personality.GetSelectedTemplateOutfitId(ClothingOutfitUtility.OutfitType.JoyResponse);
		}

		public void WriteFacadeId(Option<string> facadeId)
		{
			personality.SetSelectedTemplateOutfitId(ClothingOutfitUtility.OutfitType.JoyResponse, facadeId);
		}
	}

	private readonly Implementation impl;

	public JoyResponseOutfitTarget(Implementation impl)
	{
		this.impl = impl;
	}

	public Option<string> ReadFacadeId()
	{
		return impl.ReadFacadeId();
	}

	public void WriteFacadeId(Option<string> facadeId)
	{
		impl.WriteFacadeId(facadeId);
	}

	public string GetMinionName()
	{
		return impl.GetMinionName();
	}

	public Personality GetPersonality()
	{
		return impl.GetPersonality();
	}

	public static JoyResponseOutfitTarget FromMinion(GameObject minionInstance)
	{
		return new JoyResponseOutfitTarget(new MinionInstanceTarget(minionInstance));
	}

	public static JoyResponseOutfitTarget FromPersonality(Personality personality)
	{
		return new JoyResponseOutfitTarget(new PersonalityTarget(personality));
	}
}
