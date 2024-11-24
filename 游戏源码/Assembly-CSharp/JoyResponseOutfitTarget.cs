using System;
using UnityEngine;

// Token: 0x02001472 RID: 5234
public readonly struct JoyResponseOutfitTarget
{
	// Token: 0x06006C8D RID: 27789 RVA: 0x000E74D8 File Offset: 0x000E56D8
	public JoyResponseOutfitTarget(JoyResponseOutfitTarget.Implementation impl)
	{
		this.impl = impl;
	}

	// Token: 0x06006C8E RID: 27790 RVA: 0x000E74E1 File Offset: 0x000E56E1
	public Option<string> ReadFacadeId()
	{
		return this.impl.ReadFacadeId();
	}

	// Token: 0x06006C8F RID: 27791 RVA: 0x000E74EE File Offset: 0x000E56EE
	public void WriteFacadeId(Option<string> facadeId)
	{
		this.impl.WriteFacadeId(facadeId);
	}

	// Token: 0x06006C90 RID: 27792 RVA: 0x000E74FC File Offset: 0x000E56FC
	public string GetMinionName()
	{
		return this.impl.GetMinionName();
	}

	// Token: 0x06006C91 RID: 27793 RVA: 0x000E7509 File Offset: 0x000E5709
	public Personality GetPersonality()
	{
		return this.impl.GetPersonality();
	}

	// Token: 0x06006C92 RID: 27794 RVA: 0x000E7516 File Offset: 0x000E5716
	public static JoyResponseOutfitTarget FromMinion(GameObject minionInstance)
	{
		return new JoyResponseOutfitTarget(new JoyResponseOutfitTarget.MinionInstanceTarget(minionInstance));
	}

	// Token: 0x06006C93 RID: 27795 RVA: 0x000E7528 File Offset: 0x000E5728
	public static JoyResponseOutfitTarget FromPersonality(Personality personality)
	{
		return new JoyResponseOutfitTarget(new JoyResponseOutfitTarget.PersonalityTarget(personality));
	}

	// Token: 0x0400515F RID: 20831
	private readonly JoyResponseOutfitTarget.Implementation impl;

	// Token: 0x02001473 RID: 5235
	public interface Implementation
	{
		// Token: 0x06006C94 RID: 27796
		Option<string> ReadFacadeId();

		// Token: 0x06006C95 RID: 27797
		void WriteFacadeId(Option<string> permitId);

		// Token: 0x06006C96 RID: 27798
		string GetMinionName();

		// Token: 0x06006C97 RID: 27799
		Personality GetPersonality();
	}

	// Token: 0x02001474 RID: 5236
	public readonly struct MinionInstanceTarget : JoyResponseOutfitTarget.Implementation
	{
		// Token: 0x06006C98 RID: 27800 RVA: 0x000E753A File Offset: 0x000E573A
		public MinionInstanceTarget(GameObject minionInstance)
		{
			this.minionInstance = minionInstance;
			this.wearableAccessorizer = minionInstance.GetComponent<WearableAccessorizer>();
		}

		// Token: 0x06006C99 RID: 27801 RVA: 0x000E754F File Offset: 0x000E574F
		public string GetMinionName()
		{
			return this.minionInstance.GetProperName();
		}

		// Token: 0x06006C9A RID: 27802 RVA: 0x000E755C File Offset: 0x000E575C
		public Personality GetPersonality()
		{
			return Db.Get().Personalities.Get(this.minionInstance.GetComponent<MinionIdentity>().personalityResourceId);
		}

		// Token: 0x06006C9B RID: 27803 RVA: 0x000E757D File Offset: 0x000E577D
		public Option<string> ReadFacadeId()
		{
			return this.wearableAccessorizer.GetJoyResponseId();
		}

		// Token: 0x06006C9C RID: 27804 RVA: 0x000E758A File Offset: 0x000E578A
		public void WriteFacadeId(Option<string> permitId)
		{
			this.wearableAccessorizer.SetJoyResponseId(permitId);
		}

		// Token: 0x04005160 RID: 20832
		public readonly GameObject minionInstance;

		// Token: 0x04005161 RID: 20833
		public readonly WearableAccessorizer wearableAccessorizer;
	}

	// Token: 0x02001475 RID: 5237
	public readonly struct PersonalityTarget : JoyResponseOutfitTarget.Implementation
	{
		// Token: 0x06006C9D RID: 27805 RVA: 0x000E7598 File Offset: 0x000E5798
		public PersonalityTarget(Personality personality)
		{
			this.personality = personality;
		}

		// Token: 0x06006C9E RID: 27806 RVA: 0x000E75A1 File Offset: 0x000E57A1
		public string GetMinionName()
		{
			return this.personality.Name;
		}

		// Token: 0x06006C9F RID: 27807 RVA: 0x000E75AE File Offset: 0x000E57AE
		public Personality GetPersonality()
		{
			return this.personality;
		}

		// Token: 0x06006CA0 RID: 27808 RVA: 0x000E75B6 File Offset: 0x000E57B6
		public Option<string> ReadFacadeId()
		{
			return this.personality.GetSelectedTemplateOutfitId(ClothingOutfitUtility.OutfitType.JoyResponse);
		}

		// Token: 0x06006CA1 RID: 27809 RVA: 0x000E75C9 File Offset: 0x000E57C9
		public void WriteFacadeId(Option<string> facadeId)
		{
			this.personality.SetSelectedTemplateOutfitId(ClothingOutfitUtility.OutfitType.JoyResponse, facadeId);
		}

		// Token: 0x04005162 RID: 20834
		public readonly Personality personality;
	}
}
