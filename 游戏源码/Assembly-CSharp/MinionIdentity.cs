using System;
using System.Collections.Generic;
using Klei.AI;
using Klei.CustomSettings;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000A97 RID: 2711
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/MinionIdentity")]
public class MinionIdentity : KMonoBehaviour, ISaveLoadable, IAssignableIdentity, IListableOption, ISim1000ms
{
	// Token: 0x17000206 RID: 518
	// (get) Token: 0x06003214 RID: 12820 RVA: 0x000C0809 File Offset: 0x000BEA09
	// (set) Token: 0x06003215 RID: 12821 RVA: 0x000C0811 File Offset: 0x000BEA11
	[Serialize]
	public string genderStringKey { get; set; }

	// Token: 0x17000207 RID: 519
	// (get) Token: 0x06003216 RID: 12822 RVA: 0x000C081A File Offset: 0x000BEA1A
	// (set) Token: 0x06003217 RID: 12823 RVA: 0x000C0822 File Offset: 0x000BEA22
	[Serialize]
	public string nameStringKey { get; set; }

	// Token: 0x17000208 RID: 520
	// (get) Token: 0x06003218 RID: 12824 RVA: 0x000C082B File Offset: 0x000BEA2B
	// (set) Token: 0x06003219 RID: 12825 RVA: 0x000C0833 File Offset: 0x000BEA33
	[Serialize]
	public HashedString personalityResourceId { get; set; }

	// Token: 0x0600321A RID: 12826 RVA: 0x000C083C File Offset: 0x000BEA3C
	public static void DestroyStatics()
	{
		MinionIdentity.maleNameList = null;
		MinionIdentity.femaleNameList = null;
	}

	// Token: 0x0600321B RID: 12827 RVA: 0x00201D70 File Offset: 0x001FFF70
	protected override void OnPrefabInit()
	{
		if (this.name == null)
		{
			this.name = MinionIdentity.ChooseRandomName();
		}
		if (GameClock.Instance != null)
		{
			this.arrivalTime = (float)GameClock.Instance.GetCycle();
		}
		KAnimControllerBase component = base.GetComponent<KAnimControllerBase>();
		if (component != null)
		{
			KAnimControllerBase kanimControllerBase = component;
			kanimControllerBase.OnUpdateBounds = (Action<Bounds>)Delegate.Combine(kanimControllerBase.OnUpdateBounds, new Action<Bounds>(this.OnUpdateBounds));
		}
		GameUtil.SubscribeToTags<MinionIdentity>(this, MinionIdentity.OnDeadTagAddedDelegate, true);
	}

	// Token: 0x0600321C RID: 12828 RVA: 0x00201DEC File Offset: 0x001FFFEC
	protected override void OnSpawn()
	{
		if (this.addToIdentityList)
		{
			this.ValidateProxy();
			this.CleanupLimboMinions();
		}
		PathProber component = base.GetComponent<PathProber>();
		if (component != null)
		{
			component.SetGroupProber(MinionGroupProber.Get());
		}
		this.SetName(this.name);
		if (this.nameStringKey == null)
		{
			this.nameStringKey = this.name;
		}
		this.SetGender(this.gender);
		if (this.genderStringKey == null)
		{
			this.genderStringKey = "NB";
		}
		if (this.personalityResourceId == HashedString.Invalid)
		{
			Personality personalityFromNameStringKey = Db.Get().Personalities.GetPersonalityFromNameStringKey(this.nameStringKey);
			if (personalityFromNameStringKey != null)
			{
				this.personalityResourceId = personalityFromNameStringKey.Id;
			}
		}
		if (!this.model.IsValid)
		{
			Personality personalityFromNameStringKey2 = Db.Get().Personalities.GetPersonalityFromNameStringKey(this.nameStringKey);
			if (personalityFromNameStringKey2 != null)
			{
				this.model = personalityFromNameStringKey2.model;
			}
		}
		if (this.addToIdentityList)
		{
			Components.MinionIdentities.Add(this);
			if (!Components.MinionIdentitiesByModel.ContainsKey(this.model))
			{
				Components.MinionIdentitiesByModel[this.model] = new Components.Cmps<MinionIdentity>();
			}
			Components.MinionIdentitiesByModel[this.model].Add(this);
			if (!base.gameObject.HasTag(GameTags.Dead))
			{
				Components.LiveMinionIdentities.Add(this);
				if (!Components.LiveMinionIdentitiesByModel.ContainsKey(this.model))
				{
					Components.LiveMinionIdentitiesByModel[this.model] = new Components.Cmps<MinionIdentity>();
				}
				Components.LiveMinionIdentitiesByModel[this.model].Add(this);
				Game.Instance.Trigger(2144209314, this);
			}
		}
		SymbolOverrideController component2 = base.GetComponent<SymbolOverrideController>();
		if (component2 != null)
		{
			Accessorizer component3 = base.gameObject.GetComponent<Accessorizer>();
			if (component3 != null)
			{
				string str = HashCache.Get().Get(component3.GetAccessory(Db.Get().AccessorySlots.Mouth).symbol.hash).Replace("mouth", "cheek");
				component2.AddSymbolOverride("snapto_cheek", Assets.GetAnim("head_swap_kanim").GetData().build.GetSymbol(str), 1);
				component2.AddSymbolOverride("snapto_hair_always", component3.GetAccessory(Db.Get().AccessorySlots.Hair).symbol, 1);
				component2.AddSymbolOverride(Db.Get().AccessorySlots.HatHair.targetSymbolId, Db.Get().AccessorySlots.HatHair.Lookup("hat_" + HashCache.Get().Get(component3.GetAccessory(Db.Get().AccessorySlots.Hair).symbol.hash)).symbol, 1);
			}
		}
		this.voiceId = (this.voiceIdx + 1).ToString("D2");
		Prioritizable component4 = base.GetComponent<Prioritizable>();
		if (component4 != null)
		{
			component4.showIcon = false;
		}
		Pickupable component5 = base.GetComponent<Pickupable>();
		if (component5 != null)
		{
			component5.carryAnimOverride = Assets.GetAnim("anim_incapacitated_carrier_kanim");
		}
		this.ApplyCustomGameSettings();
	}

	// Token: 0x0600321D RID: 12829 RVA: 0x000C084A File Offset: 0x000BEA4A
	public void ValidateProxy()
	{
		this.assignableProxy = MinionAssignablesProxy.InitAssignableProxy(this.assignableProxy, this);
	}

	// Token: 0x0600321E RID: 12830 RVA: 0x00202130 File Offset: 0x00200330
	private void CleanupLimboMinions()
	{
		KPrefabID component = base.GetComponent<KPrefabID>();
		if (component.InstanceID == -1)
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				"Minion with an invalid kpid! Attempting to recover...",
				this.name
			});
			if (KPrefabIDTracker.Get().GetInstance(component.InstanceID) != null)
			{
				KPrefabIDTracker.Get().Unregister(component);
			}
			component.InstanceID = KPrefabID.GetUniqueID();
			KPrefabIDTracker.Get().Register(component);
			DebugUtil.LogWarningArgs(new object[]
			{
				"Restored as:",
				component.InstanceID
			});
		}
		if (component.conflicted)
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				"Minion with a conflicted kpid! Attempting to recover... ",
				component.InstanceID,
				this.name
			});
			if (KPrefabIDTracker.Get().GetInstance(component.InstanceID) != null)
			{
				KPrefabIDTracker.Get().Unregister(component);
			}
			component.InstanceID = KPrefabID.GetUniqueID();
			KPrefabIDTracker.Get().Register(component);
			DebugUtil.LogWarningArgs(new object[]
			{
				"Restored as:",
				component.InstanceID
			});
		}
		this.assignableProxy.Get().SetTarget(this, base.gameObject);
	}

	// Token: 0x0600321F RID: 12831 RVA: 0x000C085E File Offset: 0x000BEA5E
	public string GetProperName()
	{
		return base.gameObject.GetProperName();
	}

	// Token: 0x06003220 RID: 12832 RVA: 0x000C086B File Offset: 0x000BEA6B
	public string GetVoiceId()
	{
		return this.voiceId;
	}

	// Token: 0x06003221 RID: 12833 RVA: 0x000C0873 File Offset: 0x000BEA73
	public void SetName(string name)
	{
		this.name = name;
		if (this.selectable != null)
		{
			this.selectable.SetName(name);
		}
		base.gameObject.name = name;
		NameDisplayScreen.Instance.UpdateName(base.gameObject);
	}

	// Token: 0x06003222 RID: 12834 RVA: 0x000C08B2 File Offset: 0x000BEAB2
	public void SetStickerType(string stickerType)
	{
		this.stickerType = stickerType;
	}

	// Token: 0x06003223 RID: 12835 RVA: 0x000C08BB File Offset: 0x000BEABB
	public bool IsNull()
	{
		return this == null;
	}

	// Token: 0x06003224 RID: 12836 RVA: 0x000C08C4 File Offset: 0x000BEAC4
	public void SetGender(string gender)
	{
		this.gender = gender;
		this.selectable.SetGender(gender);
	}

	// Token: 0x06003225 RID: 12837 RVA: 0x0020226C File Offset: 0x0020046C
	public static string ChooseRandomName()
	{
		if (MinionIdentity.femaleNameList == null)
		{
			MinionIdentity.maleNameList = new MinionIdentity.NameList(Game.Instance.maleNamesFile);
			MinionIdentity.femaleNameList = new MinionIdentity.NameList(Game.Instance.femaleNamesFile);
		}
		if (UnityEngine.Random.value > 0.5f)
		{
			return MinionIdentity.maleNameList.Next();
		}
		return MinionIdentity.femaleNameList.Next();
	}

	// Token: 0x06003226 RID: 12838 RVA: 0x002022CC File Offset: 0x002004CC
	protected override void OnCleanUp()
	{
		if (this.assignableProxy != null)
		{
			MinionAssignablesProxy minionAssignablesProxy = this.assignableProxy.Get();
			if (minionAssignablesProxy && minionAssignablesProxy.target == this)
			{
				Util.KDestroyGameObject(minionAssignablesProxy.gameObject);
			}
		}
		Components.MinionIdentities.Remove(this);
		if (Components.MinionIdentitiesByModel.ContainsKey(this.model))
		{
			Components.MinionIdentitiesByModel[this.model].Remove(this);
		}
		Components.LiveMinionIdentities.Remove(this);
		if (Components.LiveMinionIdentitiesByModel.ContainsKey(this.model))
		{
			Components.LiveMinionIdentitiesByModel[this.model].Remove(this);
		}
		Game.Instance.Trigger(2144209314, this);
	}

	// Token: 0x06003227 RID: 12839 RVA: 0x000C08D9 File Offset: 0x000BEAD9
	private void OnUpdateBounds(Bounds bounds)
	{
		KBoxCollider2D component = base.GetComponent<KBoxCollider2D>();
		component.offset = bounds.center;
		component.size = bounds.extents;
	}

	// Token: 0x06003228 RID: 12840 RVA: 0x00202380 File Offset: 0x00200580
	private void OnDied(object data)
	{
		this.GetSoleOwner().UnassignAll();
		this.GetEquipment().UnequipAll();
		Components.LiveMinionIdentities.Remove(this);
		if (Components.LiveMinionIdentitiesByModel.ContainsKey(this.model))
		{
			Components.LiveMinionIdentitiesByModel[this.model].Remove(this);
		}
		Game.Instance.Trigger(-1523247426, this);
		Game.Instance.Trigger(2144209314, this);
	}

	// Token: 0x06003229 RID: 12841 RVA: 0x000C0904 File Offset: 0x000BEB04
	public List<Ownables> GetOwners()
	{
		return this.assignableProxy.Get().ownables;
	}

	// Token: 0x0600322A RID: 12842 RVA: 0x000C0916 File Offset: 0x000BEB16
	public Ownables GetSoleOwner()
	{
		return this.assignableProxy.Get().GetComponent<Ownables>();
	}

	// Token: 0x0600322B RID: 12843 RVA: 0x000C0928 File Offset: 0x000BEB28
	public bool HasOwner(Assignables owner)
	{
		return this.GetOwners().Contains(owner as Ownables);
	}

	// Token: 0x0600322C RID: 12844 RVA: 0x000C093B File Offset: 0x000BEB3B
	public int NumOwners()
	{
		return this.GetOwners().Count;
	}

	// Token: 0x0600322D RID: 12845 RVA: 0x000C0948 File Offset: 0x000BEB48
	public Equipment GetEquipment()
	{
		return this.assignableProxy.Get().GetComponent<Equipment>();
	}

	// Token: 0x0600322E RID: 12846 RVA: 0x002023F8 File Offset: 0x002005F8
	public void Sim1000ms(float dt)
	{
		if (this == null)
		{
			return;
		}
		if (this.navigator == null)
		{
			this.navigator = base.GetComponent<Navigator>();
		}
		if (this.navigator != null && !this.navigator.IsMoving())
		{
			return;
		}
		if (this.choreDriver == null)
		{
			this.choreDriver = base.GetComponent<ChoreDriver>();
		}
		if (this.choreDriver != null)
		{
			Chore currentChore = this.choreDriver.GetCurrentChore();
			if (currentChore != null && currentChore is FetchAreaChore)
			{
				MinionResume component = base.GetComponent<MinionResume>();
				if (component != null)
				{
					component.AddExperienceWithAptitude(Db.Get().SkillGroups.Hauling.Id, dt, SKILLS.ALL_DAY_EXPERIENCE);
				}
			}
		}
	}

	// Token: 0x0600322F RID: 12847 RVA: 0x002024B4 File Offset: 0x002006B4
	private void ApplyCustomGameSettings()
	{
		SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.ImmuneSystem);
		if (currentQualitySetting.id == "Compromised")
		{
			Db.Get().Attributes.DiseaseCureSpeed.Lookup(this).Add(new AttributeModifier(Db.Get().Attributes.DiseaseCureSpeed.Id, -0.3333f, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.IMMUNESYSTEM.LEVELS.COMPROMISED.ATTRIBUTE_MODIFIER_NAME, false, false, true));
			Db.Get().Attributes.GermResistance.Lookup(this).Add(new AttributeModifier(Db.Get().Attributes.GermResistance.Id, -2f, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.IMMUNESYSTEM.LEVELS.COMPROMISED.ATTRIBUTE_MODIFIER_NAME, false, false, true));
		}
		else if (currentQualitySetting.id == "Weak")
		{
			Db.Get().Attributes.GermResistance.Lookup(this).Add(new AttributeModifier(Db.Get().Attributes.GermResistance.Id, -1f, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.IMMUNESYSTEM.LEVELS.WEAK.ATTRIBUTE_MODIFIER_NAME, false, false, true));
		}
		else if (currentQualitySetting.id == "Strong")
		{
			Db.Get().Attributes.DiseaseCureSpeed.Lookup(this).Add(new AttributeModifier(Db.Get().Attributes.DiseaseCureSpeed.Id, 2f, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.IMMUNESYSTEM.LEVELS.STRONG.ATTRIBUTE_MODIFIER_NAME, false, false, true));
			Db.Get().Attributes.GermResistance.Lookup(this).Add(new AttributeModifier(Db.Get().Attributes.GermResistance.Id, 2f, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.IMMUNESYSTEM.LEVELS.STRONG.ATTRIBUTE_MODIFIER_NAME, false, false, true));
		}
		else if (currentQualitySetting.id == "Invincible")
		{
			Db.Get().Attributes.DiseaseCureSpeed.Lookup(this).Add(new AttributeModifier(Db.Get().Attributes.DiseaseCureSpeed.Id, 100000000f, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.IMMUNESYSTEM.LEVELS.INVINCIBLE.ATTRIBUTE_MODIFIER_NAME, false, false, true));
			Db.Get().Attributes.GermResistance.Lookup(this).Add(new AttributeModifier(Db.Get().Attributes.GermResistance.Id, 200f, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.IMMUNESYSTEM.LEVELS.INVINCIBLE.ATTRIBUTE_MODIFIER_NAME, false, false, true));
		}
		SettingLevel currentQualitySetting2 = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.Stress);
		if (currentQualitySetting2.id == "Doomed")
		{
			Db.Get().Amounts.Stress.deltaAttribute.Lookup(this).Add(new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, 0.033333335f, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.STRESS.LEVELS.DOOMED.ATTRIBUTE_MODIFIER_NAME, false, false, true));
		}
		else if (currentQualitySetting2.id == "Pessimistic")
		{
			Db.Get().Amounts.Stress.deltaAttribute.Lookup(this).Add(new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, 0.016666668f, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.STRESS.LEVELS.PESSIMISTIC.ATTRIBUTE_MODIFIER_NAME, false, false, true));
		}
		else if (currentQualitySetting2.id == "Optimistic")
		{
			Db.Get().Amounts.Stress.deltaAttribute.Lookup(this).Add(new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, -0.016666668f, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.STRESS.LEVELS.OPTIMISTIC.ATTRIBUTE_MODIFIER_NAME, false, false, true));
		}
		else if (currentQualitySetting2.id == "Indomitable")
		{
			Db.Get().Amounts.Stress.deltaAttribute.Lookup(this).Add(new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, float.NegativeInfinity, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.STRESS.LEVELS.INDOMITABLE.ATTRIBUTE_MODIFIER_NAME, false, false, true));
		}
		SettingLevel currentQualitySetting3 = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.CalorieBurn);
		if (currentQualitySetting3.id == "VeryHard")
		{
			Db.Get().Amounts.Calories.deltaAttribute.Lookup(this).Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, DUPLICANTSTATS.STANDARD.BaseStats.CALORIES_BURNED_PER_SECOND * 1f, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.CALORIE_BURN.LEVELS.VERYHARD.ATTRIBUTE_MODIFIER_NAME, false, false, true));
			return;
		}
		if (currentQualitySetting3.id == "Hard")
		{
			Db.Get().Amounts.Calories.deltaAttribute.Lookup(this).Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, DUPLICANTSTATS.STANDARD.BaseStats.CALORIES_BURNED_PER_SECOND * 0.5f, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.CALORIE_BURN.LEVELS.HARD.ATTRIBUTE_MODIFIER_NAME, false, false, true));
			return;
		}
		if (currentQualitySetting3.id == "Easy")
		{
			Db.Get().Amounts.Calories.deltaAttribute.Lookup(this).Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, DUPLICANTSTATS.STANDARD.BaseStats.CALORIES_BURNED_PER_SECOND * -0.5f, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.CALORIE_BURN.LEVELS.EASY.ATTRIBUTE_MODIFIER_NAME, false, false, true));
			return;
		}
		if (currentQualitySetting3.id == "Disabled")
		{
			Db.Get().Amounts.Calories.deltaAttribute.Lookup(this).Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, float.PositiveInfinity, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.CALORIE_BURN.LEVELS.DISABLED.ATTRIBUTE_MODIFIER_NAME, false, false, true));
		}
	}

	// Token: 0x06003230 RID: 12848 RVA: 0x00202A7C File Offset: 0x00200C7C
	public static float GetCalorieBurnMultiplier()
	{
		float result = 1f;
		SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.CalorieBurn);
		if (currentQualitySetting.id == "VeryHard")
		{
			result = 2f;
		}
		else if (currentQualitySetting.id == "Hard")
		{
			result = 1.5f;
		}
		else if (currentQualitySetting.id == "Easy")
		{
			result = 0.5f;
		}
		else if (currentQualitySetting.id == "Disabled")
		{
			result = 0f;
		}
		return result;
	}

	// Token: 0x040021AC RID: 8620
	public const string HairAlwaysSymbol = "snapto_hair_always";

	// Token: 0x040021AD RID: 8621
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x040021AE RID: 8622
	[MyCmpReq]
	public Modifiers modifiers;

	// Token: 0x040021AF RID: 8623
	public int femaleVoiceCount;

	// Token: 0x040021B0 RID: 8624
	public int maleVoiceCount;

	// Token: 0x040021B1 RID: 8625
	[Serialize]
	public Tag model;

	// Token: 0x040021B2 RID: 8626
	[Serialize]
	private new string name;

	// Token: 0x040021B3 RID: 8627
	[Serialize]
	public string gender;

	// Token: 0x040021B7 RID: 8631
	[Serialize]
	public string stickerType;

	// Token: 0x040021B8 RID: 8632
	[Serialize]
	[ReadOnly]
	public float arrivalTime;

	// Token: 0x040021B9 RID: 8633
	[Serialize]
	public int voiceIdx;

	// Token: 0x040021BA RID: 8634
	[Serialize]
	public Ref<MinionAssignablesProxy> assignableProxy;

	// Token: 0x040021BB RID: 8635
	private Navigator navigator;

	// Token: 0x040021BC RID: 8636
	private ChoreDriver choreDriver;

	// Token: 0x040021BD RID: 8637
	public float timeLastSpoke;

	// Token: 0x040021BE RID: 8638
	private string voiceId;

	// Token: 0x040021BF RID: 8639
	private KAnimHashedString overrideExpression;

	// Token: 0x040021C0 RID: 8640
	private KAnimHashedString expression;

	// Token: 0x040021C1 RID: 8641
	public bool addToIdentityList = true;

	// Token: 0x040021C2 RID: 8642
	private static MinionIdentity.NameList maleNameList;

	// Token: 0x040021C3 RID: 8643
	private static MinionIdentity.NameList femaleNameList;

	// Token: 0x040021C4 RID: 8644
	private static readonly EventSystem.IntraObjectHandler<MinionIdentity> OnDeadTagAddedDelegate = GameUtil.CreateHasTagHandler<MinionIdentity>(GameTags.Dead, delegate(MinionIdentity component, object data)
	{
		component.OnDied(data);
	});

	// Token: 0x02000A98 RID: 2712
	private class NameList
	{
		// Token: 0x06003233 RID: 12851 RVA: 0x00202B08 File Offset: 0x00200D08
		public NameList(TextAsset file)
		{
			string[] array = file.text.Replace("  ", " ").Replace("\r\n", "\n").Split('\n', StringSplitOptions.None);
			for (int i = 0; i < array.Length; i++)
			{
				string[] array2 = array[i].Split(' ', StringSplitOptions.None);
				if (array2[array2.Length - 1] != "" && array2[array2.Length - 1] != null)
				{
					this.names.Add(array2[array2.Length - 1]);
				}
			}
			this.names.Shuffle<string>();
		}

		// Token: 0x06003234 RID: 12852 RVA: 0x00202BA8 File Offset: 0x00200DA8
		public string Next()
		{
			List<string> list = this.names;
			int num = this.idx;
			this.idx = num + 1;
			return list[num % this.names.Count];
		}

		// Token: 0x040021C5 RID: 8645
		private List<string> names = new List<string>();

		// Token: 0x040021C6 RID: 8646
		private int idx;
	}
}
