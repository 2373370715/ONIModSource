using System;
using System.Collections.Generic;
using Klei.AI;

namespace Database
{
	// Token: 0x02002151 RID: 8529
	public class Personalities : ResourceSet<Personality>
	{
		// Token: 0x0600B5AC RID: 46508 RVA: 0x00452974 File Offset: 0x00450B74
		public Personalities()
		{
			foreach (Personalities.PersonalityInfo personalityInfo in AsyncLoadManager<IGlobalAsyncLoader>.AsyncLoader<Personalities.PersonalityLoader>.Get().entries)
			{
				if (string.IsNullOrEmpty(personalityInfo.RequiredDlcId) || DlcManager.IsContentSubscribed(personalityInfo.RequiredDlcId))
				{
					base.Add(new Personality(personalityInfo.Name.ToUpper(), Strings.Get(string.Format("STRINGS.DUPLICANTS.PERSONALITIES.{0}.NAME", personalityInfo.Name.ToUpper())), personalityInfo.Gender.ToUpper(), personalityInfo.PersonalityType, personalityInfo.StressTrait, personalityInfo.JoyTrait, personalityInfo.StickerType, personalityInfo.CongenitalTrait, personalityInfo.HeadShape, personalityInfo.Mouth, personalityInfo.Neck, personalityInfo.Eyes, personalityInfo.Hair, personalityInfo.Body, personalityInfo.Belt, personalityInfo.Cuff, personalityInfo.Foot, personalityInfo.Hand, personalityInfo.Pelvis, personalityInfo.Leg, Strings.Get(string.Format("STRINGS.DUPLICANTS.PERSONALITIES.{0}.DESC", personalityInfo.Name.ToUpper())), personalityInfo.ValidStarter, personalityInfo.Grave, personalityInfo.Model)
					{
						requiredDlcId = personalityInfo.RequiredDlcId
					});
				}
			}
		}

		// Token: 0x0600B5AD RID: 46509 RVA: 0x00452AB4 File Offset: 0x00450CB4
		private void AddTrait(Personality personality, string trait_name)
		{
			Trait trait = Db.Get().traits.TryGet(trait_name);
			if (trait != null)
			{
				personality.AddTrait(trait);
			}
		}

		// Token: 0x0600B5AE RID: 46510 RVA: 0x00452ADC File Offset: 0x00450CDC
		private void SetAttribute(Personality personality, string attribute_name, int value)
		{
			Klei.AI.Attribute attribute = Db.Get().Attributes.TryGet(attribute_name);
			if (attribute == null)
			{
				Debug.LogWarning("Attribute does not exist: " + attribute_name);
				return;
			}
			personality.SetAttribute(attribute, value);
		}

		// Token: 0x0600B5AF RID: 46511 RVA: 0x001152A8 File Offset: 0x001134A8
		public List<Personality> GetStartingPersonalities()
		{
			return this.resources.FindAll((Personality x) => x.startingMinion);
		}

		// Token: 0x0600B5B0 RID: 46512 RVA: 0x00452B18 File Offset: 0x00450D18
		public List<Personality> GetAll(bool onlyEnabledMinions, bool onlyStartingMinions)
		{
			return this.resources.FindAll((Personality personality) => (!onlyStartingMinions || personality.startingMinion) && (!onlyEnabledMinions || !personality.Disabled) && (!(SaveLoader.Instance != null) || !DlcManager.IsDlcId(personality.requiredDlcId) || SaveLoader.Instance.GameInfo.dlcIds.Contains(personality.requiredDlcId)));
		}

		// Token: 0x0600B5B1 RID: 46513 RVA: 0x001152D4 File Offset: 0x001134D4
		public Personality GetRandom(bool onlyEnabledMinions, bool onlyStartingMinions)
		{
			return this.GetAll(onlyEnabledMinions, onlyStartingMinions).GetRandom<Personality>();
		}

		// Token: 0x0600B5B2 RID: 46514 RVA: 0x00452B50 File Offset: 0x00450D50
		public Personality GetRandom(Tag model, bool onlyEnabledMinions, bool onlyStartingMinions)
		{
			return this.GetAll(onlyEnabledMinions, onlyStartingMinions).FindAll((Personality personality) => personality.model == model || model == null).GetRandom<Personality>();
		}

		// Token: 0x0600B5B3 RID: 46515 RVA: 0x00452B88 File Offset: 0x00450D88
		public Personality GetRandom(List<Tag> models, bool onlyEnabledMinions, bool onlyStartingMinions)
		{
			return this.GetAll(onlyEnabledMinions, onlyStartingMinions).FindAll((Personality personality) => models.Contains(personality.model)).GetRandom<Personality>();
		}

		// Token: 0x0600B5B4 RID: 46516 RVA: 0x00452BC0 File Offset: 0x00450DC0
		public Personality GetPersonalityFromNameStringKey(string name_string_key)
		{
			foreach (Personality personality in Db.Get().Personalities.resources)
			{
				if (personality.nameStringKey.Equals(name_string_key, StringComparison.CurrentCultureIgnoreCase))
				{
					return personality;
				}
			}
			return null;
		}

		// Token: 0x02002152 RID: 8530
		public class PersonalityLoader : AsyncCsvLoader<Personalities.PersonalityLoader, Personalities.PersonalityInfo>
		{
			// Token: 0x0600B5B5 RID: 46517 RVA: 0x001152E3 File Offset: 0x001134E3
			public PersonalityLoader() : base(Assets.instance.personalitiesFile)
			{
			}

			// Token: 0x0600B5B6 RID: 46518 RVA: 0x001152F5 File Offset: 0x001134F5
			public override void Run()
			{
				base.Run();
			}
		}

		// Token: 0x02002153 RID: 8531
		public class PersonalityInfo : Resource
		{
			// Token: 0x040093A9 RID: 37801
			public int HeadShape;

			// Token: 0x040093AA RID: 37802
			public int Mouth;

			// Token: 0x040093AB RID: 37803
			public int Neck;

			// Token: 0x040093AC RID: 37804
			public int Eyes;

			// Token: 0x040093AD RID: 37805
			public int Hair;

			// Token: 0x040093AE RID: 37806
			public int Body;

			// Token: 0x040093AF RID: 37807
			public int Belt;

			// Token: 0x040093B0 RID: 37808
			public int Cuff;

			// Token: 0x040093B1 RID: 37809
			public int Foot;

			// Token: 0x040093B2 RID: 37810
			public int Hand;

			// Token: 0x040093B3 RID: 37811
			public int Pelvis;

			// Token: 0x040093B4 RID: 37812
			public int Leg;

			// Token: 0x040093B5 RID: 37813
			public string Gender;

			// Token: 0x040093B6 RID: 37814
			public string PersonalityType;

			// Token: 0x040093B7 RID: 37815
			public string StressTrait;

			// Token: 0x040093B8 RID: 37816
			public string JoyTrait;

			// Token: 0x040093B9 RID: 37817
			public string StickerType;

			// Token: 0x040093BA RID: 37818
			public string CongenitalTrait;

			// Token: 0x040093BB RID: 37819
			public string Design;

			// Token: 0x040093BC RID: 37820
			public bool ValidStarter;

			// Token: 0x040093BD RID: 37821
			public string Grave;

			// Token: 0x040093BE RID: 37822
			public string Model;

			// Token: 0x040093BF RID: 37823
			public string RequiredDlcId;
		}
	}
}
