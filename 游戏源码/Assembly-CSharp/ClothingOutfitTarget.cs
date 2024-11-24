using System;
using System.Collections.Generic;
using System.Linq;
using Database;
using STRINGS;
using UnityEngine;

// Token: 0x02001082 RID: 4226
public readonly struct ClothingOutfitTarget : IEquatable<ClothingOutfitTarget>
{
	// Token: 0x170004F9 RID: 1273
	// (get) Token: 0x06005654 RID: 22100 RVA: 0x000D85BC File Offset: 0x000D67BC
	public string OutfitId
	{
		get
		{
			return this.impl.OutfitId;
		}
	}

	// Token: 0x170004FA RID: 1274
	// (get) Token: 0x06005655 RID: 22101 RVA: 0x000D85C9 File Offset: 0x000D67C9
	public ClothingOutfitUtility.OutfitType OutfitType
	{
		get
		{
			return this.impl.OutfitType;
		}
	}

	// Token: 0x06005656 RID: 22102 RVA: 0x000D85D6 File Offset: 0x000D67D6
	public string[] ReadItems()
	{
		return this.impl.ReadItems(this.OutfitType).Where(new Func<string, bool>(ClothingOutfitTarget.DoesClothingItemExist)).ToArray<string>();
	}

	// Token: 0x06005657 RID: 22103 RVA: 0x000D85FF File Offset: 0x000D67FF
	public void WriteItems(ClothingOutfitUtility.OutfitType outfitType, string[] items)
	{
		this.impl.WriteItems(outfitType, items);
	}

	// Token: 0x170004FB RID: 1275
	// (get) Token: 0x06005658 RID: 22104 RVA: 0x000D860E File Offset: 0x000D680E
	public bool CanWriteItems
	{
		get
		{
			return this.impl.CanWriteItems;
		}
	}

	// Token: 0x06005659 RID: 22105 RVA: 0x000D861B File Offset: 0x000D681B
	public string ReadName()
	{
		return this.impl.ReadName();
	}

	// Token: 0x0600565A RID: 22106 RVA: 0x000D8628 File Offset: 0x000D6828
	public void WriteName(string name)
	{
		this.impl.WriteName(name);
	}

	// Token: 0x170004FC RID: 1276
	// (get) Token: 0x0600565B RID: 22107 RVA: 0x000D8636 File Offset: 0x000D6836
	public bool CanWriteName
	{
		get
		{
			return this.impl.CanWriteName;
		}
	}

	// Token: 0x0600565C RID: 22108 RVA: 0x000D8643 File Offset: 0x000D6843
	public void Delete()
	{
		this.impl.Delete();
	}

	// Token: 0x170004FD RID: 1277
	// (get) Token: 0x0600565D RID: 22109 RVA: 0x000D8650 File Offset: 0x000D6850
	public bool CanDelete
	{
		get
		{
			return this.impl.CanDelete;
		}
	}

	// Token: 0x0600565E RID: 22110 RVA: 0x000D865D File Offset: 0x000D685D
	public bool DoesExist()
	{
		return this.impl.DoesExist();
	}

	// Token: 0x0600565F RID: 22111 RVA: 0x000D866A File Offset: 0x000D686A
	public ClothingOutfitTarget(ClothingOutfitTarget.Implementation impl)
	{
		this.impl = impl;
	}

	// Token: 0x06005660 RID: 22112 RVA: 0x000D8673 File Offset: 0x000D6873
	public bool DoesContainLockedItems()
	{
		return ClothingOutfitTarget.DoesContainLockedItems(this.ReadItems());
	}

	// Token: 0x06005661 RID: 22113 RVA: 0x00282754 File Offset: 0x00280954
	public static bool DoesContainLockedItems(IList<string> itemIds)
	{
		foreach (string id in itemIds)
		{
			PermitResource permitResource = Db.Get().Permits.TryGet(id);
			if (permitResource != null && !permitResource.IsUnlocked())
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06005662 RID: 22114 RVA: 0x000D8680 File Offset: 0x000D6880
	public IEnumerable<ClothingItemResource> ReadItemValues()
	{
		return from i in this.ReadItems()
		select Db.Get().Permits.ClothingItems.Get(i);
	}

	// Token: 0x06005663 RID: 22115 RVA: 0x000D86AC File Offset: 0x000D68AC
	public static bool DoesClothingItemExist(string clothingItemId)
	{
		return !Db.Get().Permits.ClothingItems.TryGet(clothingItemId).IsNullOrDestroyed();
	}

	// Token: 0x06005664 RID: 22116 RVA: 0x000D86CB File Offset: 0x000D68CB
	public bool Is<T>() where T : ClothingOutfitTarget.Implementation
	{
		return this.impl is T;
	}

	// Token: 0x06005665 RID: 22117 RVA: 0x002827B8 File Offset: 0x002809B8
	public bool Is<T>(out T value) where T : ClothingOutfitTarget.Implementation
	{
		ClothingOutfitTarget.Implementation implementation = this.impl;
		if (implementation is T)
		{
			T t = (T)((object)implementation);
			value = t;
			return true;
		}
		value = default(T);
		return false;
	}

	// Token: 0x06005666 RID: 22118 RVA: 0x000D86DB File Offset: 0x000D68DB
	public bool IsTemplateOutfit()
	{
		return this.Is<ClothingOutfitTarget.DatabaseAuthoredTemplate>() || this.Is<ClothingOutfitTarget.UserAuthoredTemplate>();
	}

	// Token: 0x06005667 RID: 22119 RVA: 0x000D86ED File Offset: 0x000D68ED
	public static ClothingOutfitTarget ForNewTemplateOutfit(ClothingOutfitUtility.OutfitType outfitType)
	{
		return new ClothingOutfitTarget(new ClothingOutfitTarget.UserAuthoredTemplate(outfitType, ClothingOutfitTarget.GetUniqueNameIdFrom(UI.OUTFIT_NAME.NEW)));
	}

	// Token: 0x06005668 RID: 22120 RVA: 0x000D870E File Offset: 0x000D690E
	public static ClothingOutfitTarget ForNewTemplateOutfit(ClothingOutfitUtility.OutfitType outfitType, string id)
	{
		if (ClothingOutfitTarget.DoesTemplateExist(id))
		{
			throw new ArgumentException("Can not create a new target with id " + id + ", an outfit with that id already exists");
		}
		return new ClothingOutfitTarget(new ClothingOutfitTarget.UserAuthoredTemplate(outfitType, id));
	}

	// Token: 0x06005669 RID: 22121 RVA: 0x000D873F File Offset: 0x000D693F
	public static ClothingOutfitTarget ForTemplateCopyOf(ClothingOutfitTarget sourceTarget)
	{
		return new ClothingOutfitTarget(new ClothingOutfitTarget.UserAuthoredTemplate(sourceTarget.OutfitType, ClothingOutfitTarget.GetUniqueNameIdFrom(UI.OUTFIT_NAME.COPY_OF.Replace("{OutfitName}", sourceTarget.ReadName()))));
	}

	// Token: 0x0600566A RID: 22122 RVA: 0x000D8772 File Offset: 0x000D6972
	public static ClothingOutfitTarget FromMinion(ClothingOutfitUtility.OutfitType outfitType, GameObject minionInstance)
	{
		return new ClothingOutfitTarget(new ClothingOutfitTarget.MinionInstance(outfitType, minionInstance));
	}

	// Token: 0x0600566B RID: 22123 RVA: 0x002827EC File Offset: 0x002809EC
	public static ClothingOutfitTarget FromTemplateId(string outfitId)
	{
		return ClothingOutfitTarget.TryFromTemplateId(outfitId).Value;
	}

	// Token: 0x0600566C RID: 22124 RVA: 0x00282808 File Offset: 0x00280A08
	public static Option<ClothingOutfitTarget> TryFromTemplateId(string outfitId)
	{
		if (outfitId == null)
		{
			return Option.None;
		}
		SerializableOutfitData.Version2.CustomTemplateOutfitEntry customTemplateOutfitEntry;
		ClothingOutfitUtility.OutfitType outfitType;
		if (CustomClothingOutfits.Instance.Internal_GetOutfitData().OutfitIdToUserAuthoredTemplateOutfit.TryGetValue(outfitId, out customTemplateOutfitEntry) && Enum.TryParse<ClothingOutfitUtility.OutfitType>(customTemplateOutfitEntry.outfitType, true, out outfitType))
		{
			return new ClothingOutfitTarget(new ClothingOutfitTarget.UserAuthoredTemplate(outfitType, outfitId));
		}
		ClothingOutfitResource clothingOutfitResource = Db.Get().Permits.ClothingOutfits.TryGet(outfitId);
		if (!clothingOutfitResource.IsNullOrDestroyed())
		{
			return new ClothingOutfitTarget(new ClothingOutfitTarget.DatabaseAuthoredTemplate(clothingOutfitResource));
		}
		return Option.None;
	}

	// Token: 0x0600566D RID: 22125 RVA: 0x000D8785 File Offset: 0x000D6985
	public static bool DoesTemplateExist(string outfitId)
	{
		return Db.Get().Permits.ClothingOutfits.TryGet(outfitId) != null || CustomClothingOutfits.Instance.Internal_GetOutfitData().OutfitIdToUserAuthoredTemplateOutfit.ContainsKey(outfitId);
	}

	// Token: 0x0600566E RID: 22126 RVA: 0x000D87BA File Offset: 0x000D69BA
	public static IEnumerable<ClothingOutfitTarget> GetAllTemplates()
	{
		foreach (ClothingOutfitResource outfit in Db.Get().Permits.ClothingOutfits.resources)
		{
			yield return new ClothingOutfitTarget(new ClothingOutfitTarget.DatabaseAuthoredTemplate(outfit));
		}
		List<ClothingOutfitResource>.Enumerator enumerator = default(List<ClothingOutfitResource>.Enumerator);
		foreach (KeyValuePair<string, SerializableOutfitData.Version2.CustomTemplateOutfitEntry> keyValuePair in CustomClothingOutfits.Instance.Internal_GetOutfitData().OutfitIdToUserAuthoredTemplateOutfit)
		{
			string text;
			SerializableOutfitData.Version2.CustomTemplateOutfitEntry customTemplateOutfitEntry;
			keyValuePair.Deconstruct(out text, out customTemplateOutfitEntry);
			string outfitId = text;
			ClothingOutfitUtility.OutfitType outfitType;
			if (Enum.TryParse<ClothingOutfitUtility.OutfitType>(customTemplateOutfitEntry.outfitType, true, out outfitType))
			{
				yield return new ClothingOutfitTarget(new ClothingOutfitTarget.UserAuthoredTemplate(outfitType, outfitId));
			}
		}
		Dictionary<string, SerializableOutfitData.Version2.CustomTemplateOutfitEntry>.Enumerator enumerator2 = default(Dictionary<string, SerializableOutfitData.Version2.CustomTemplateOutfitEntry>.Enumerator);
		yield break;
		yield break;
	}

	// Token: 0x0600566F RID: 22127 RVA: 0x000D87C3 File Offset: 0x000D69C3
	public static ClothingOutfitTarget GetRandom()
	{
		return ClothingOutfitTarget.GetAllTemplates().GetRandom<ClothingOutfitTarget>();
	}

	// Token: 0x06005670 RID: 22128 RVA: 0x002828A4 File Offset: 0x00280AA4
	public static Option<ClothingOutfitTarget> GetRandom(ClothingOutfitUtility.OutfitType onlyOfType)
	{
		IEnumerable<ClothingOutfitTarget> enumerable = from t in ClothingOutfitTarget.GetAllTemplates()
		where t.OutfitType == onlyOfType
		select t;
		if (enumerable == null || enumerable.Count<ClothingOutfitTarget>() == 0)
		{
			return Option.None;
		}
		return enumerable.GetRandom<ClothingOutfitTarget>();
	}

	// Token: 0x06005671 RID: 22129 RVA: 0x002828F8 File Offset: 0x00280AF8
	public static string GetUniqueNameIdFrom(string preferredName)
	{
		if (!ClothingOutfitTarget.DoesTemplateExist(preferredName))
		{
			return preferredName;
		}
		string replacement = "testOutfit";
		string a = UI.OUTFIT_NAME.RESOLVE_CONFLICT.Replace("{OutfitName}", replacement).Replace("{ConflictNumber}", 1.ToString());
		string b = UI.OUTFIT_NAME.RESOLVE_CONFLICT.Replace("{OutfitName}", replacement).Replace("{ConflictNumber}", 2.ToString());
		string text;
		if (a != b)
		{
			text = UI.OUTFIT_NAME.RESOLVE_CONFLICT;
		}
		else
		{
			text = "{OutfitName} ({ConflictNumber})";
		}
		for (int i = 1; i < 10000; i++)
		{
			string text2 = text.Replace("{OutfitName}", preferredName).Replace("{ConflictNumber}", i.ToString());
			if (!ClothingOutfitTarget.DoesTemplateExist(text2))
			{
				return text2;
			}
		}
		throw new Exception("Couldn't get a unique name for preferred name: " + preferredName);
	}

	// Token: 0x06005672 RID: 22130 RVA: 0x000D87CF File Offset: 0x000D69CF
	public static bool operator ==(ClothingOutfitTarget a, ClothingOutfitTarget b)
	{
		return a.Equals(b);
	}

	// Token: 0x06005673 RID: 22131 RVA: 0x000D87D9 File Offset: 0x000D69D9
	public static bool operator !=(ClothingOutfitTarget a, ClothingOutfitTarget b)
	{
		return !a.Equals(b);
	}

	// Token: 0x06005674 RID: 22132 RVA: 0x002829C8 File Offset: 0x00280BC8
	public override bool Equals(object obj)
	{
		if (obj is ClothingOutfitTarget)
		{
			ClothingOutfitTarget other = (ClothingOutfitTarget)obj;
			return this.Equals(other);
		}
		return false;
	}

	// Token: 0x06005675 RID: 22133 RVA: 0x000D87E6 File Offset: 0x000D69E6
	public bool Equals(ClothingOutfitTarget other)
	{
		if (this.impl == null || other.impl == null)
		{
			return this.impl == null == (other.impl == null);
		}
		return this.OutfitId == other.OutfitId;
	}

	// Token: 0x06005676 RID: 22134 RVA: 0x000D881F File Offset: 0x000D6A1F
	public override int GetHashCode()
	{
		return Hash.SDBMLower(this.impl.OutfitId);
	}

	// Token: 0x04003C9B RID: 15515
	public readonly ClothingOutfitTarget.Implementation impl;

	// Token: 0x04003C9C RID: 15516
	public static readonly string[] NO_ITEMS = new string[0];

	// Token: 0x04003C9D RID: 15517
	public static readonly ClothingItemResource[] NO_ITEM_VALUES = new ClothingItemResource[0];

	// Token: 0x02001083 RID: 4227
	public interface Implementation
	{
		// Token: 0x170004FE RID: 1278
		// (get) Token: 0x06005678 RID: 22136
		string OutfitId { get; }

		// Token: 0x170004FF RID: 1279
		// (get) Token: 0x06005679 RID: 22137
		ClothingOutfitUtility.OutfitType OutfitType { get; }

		// Token: 0x0600567A RID: 22138
		string[] ReadItems(ClothingOutfitUtility.OutfitType outfitType);

		// Token: 0x0600567B RID: 22139
		void WriteItems(ClothingOutfitUtility.OutfitType outfitType, string[] items);

		// Token: 0x17000500 RID: 1280
		// (get) Token: 0x0600567C RID: 22140
		bool CanWriteItems { get; }

		// Token: 0x0600567D RID: 22141
		string ReadName();

		// Token: 0x0600567E RID: 22142
		void WriteName(string name);

		// Token: 0x17000501 RID: 1281
		// (get) Token: 0x0600567F RID: 22143
		bool CanWriteName { get; }

		// Token: 0x06005680 RID: 22144
		void Delete();

		// Token: 0x17000502 RID: 1282
		// (get) Token: 0x06005681 RID: 22145
		bool CanDelete { get; }

		// Token: 0x06005682 RID: 22146
		bool DoesExist();
	}

	// Token: 0x02001084 RID: 4228
	public readonly struct MinionInstance : ClothingOutfitTarget.Implementation
	{
		// Token: 0x17000503 RID: 1283
		// (get) Token: 0x06005683 RID: 22147 RVA: 0x000A65EC File Offset: 0x000A47EC
		public bool CanWriteItems
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000504 RID: 1284
		// (get) Token: 0x06005684 RID: 22148 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
		public bool CanWriteName
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000505 RID: 1285
		// (get) Token: 0x06005685 RID: 22149 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
		public bool CanDelete
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06005686 RID: 22150 RVA: 0x000D8849 File Offset: 0x000D6A49
		public bool DoesExist()
		{
			return !this.minionInstance.IsNullOrDestroyed();
		}

		// Token: 0x17000506 RID: 1286
		// (get) Token: 0x06005687 RID: 22151 RVA: 0x002829F0 File Offset: 0x00280BF0
		public string OutfitId
		{
			get
			{
				return this.minionInstance.GetInstanceID().ToString() + "_outfit";
			}
		}

		// Token: 0x17000507 RID: 1287
		// (get) Token: 0x06005688 RID: 22152 RVA: 0x000D8859 File Offset: 0x000D6A59
		public ClothingOutfitUtility.OutfitType OutfitType
		{
			get
			{
				return this.m_outfitType;
			}
		}

		// Token: 0x06005689 RID: 22153 RVA: 0x000D8861 File Offset: 0x000D6A61
		public MinionInstance(ClothingOutfitUtility.OutfitType outfitType, GameObject minionInstance)
		{
			this.minionInstance = minionInstance;
			this.m_outfitType = outfitType;
			this.accessorizer = minionInstance.GetComponent<WearableAccessorizer>();
		}

		// Token: 0x0600568A RID: 22154 RVA: 0x000D887D File Offset: 0x000D6A7D
		public string[] ReadItems(ClothingOutfitUtility.OutfitType outfitType)
		{
			return this.accessorizer.GetClothingItemsIds(outfitType);
		}

		// Token: 0x0600568B RID: 22155 RVA: 0x00282A1C File Offset: 0x00280C1C
		public void WriteItems(ClothingOutfitUtility.OutfitType outfitType, string[] items)
		{
			this.accessorizer.ClearClothingItems(new ClothingOutfitUtility.OutfitType?(outfitType));
			this.accessorizer.ApplyClothingItems(outfitType, from i in items
			select Db.Get().Permits.ClothingItems.Get(i));
		}

		// Token: 0x0600568C RID: 22156 RVA: 0x000D888B File Offset: 0x000D6A8B
		public string ReadName()
		{
			return UI.OUTFIT_NAME.MINIONS_OUTFIT.Replace("{MinionName}", this.minionInstance.GetProperName());
		}

		// Token: 0x0600568D RID: 22157 RVA: 0x000D88A7 File Offset: 0x000D6AA7
		public void WriteName(string name)
		{
			throw new InvalidOperationException("Can not change change the outfit id for a minion instance");
		}

		// Token: 0x0600568E RID: 22158 RVA: 0x000D88B3 File Offset: 0x000D6AB3
		public void Delete()
		{
			throw new InvalidOperationException("Can not delete a minion instance outfit");
		}

		// Token: 0x04003C9E RID: 15518
		private readonly ClothingOutfitUtility.OutfitType m_outfitType;

		// Token: 0x04003C9F RID: 15519
		public readonly GameObject minionInstance;

		// Token: 0x04003CA0 RID: 15520
		public readonly WearableAccessorizer accessorizer;
	}

	// Token: 0x02001086 RID: 4230
	public readonly struct UserAuthoredTemplate : ClothingOutfitTarget.Implementation
	{
		// Token: 0x17000508 RID: 1288
		// (get) Token: 0x06005692 RID: 22162 RVA: 0x000A65EC File Offset: 0x000A47EC
		public bool CanWriteItems
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000509 RID: 1289
		// (get) Token: 0x06005693 RID: 22163 RVA: 0x000A65EC File Offset: 0x000A47EC
		public bool CanWriteName
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700050A RID: 1290
		// (get) Token: 0x06005694 RID: 22164 RVA: 0x000A65EC File Offset: 0x000A47EC
		public bool CanDelete
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06005695 RID: 22165 RVA: 0x000D88E2 File Offset: 0x000D6AE2
		public bool DoesExist()
		{
			return CustomClothingOutfits.Instance.Internal_GetOutfitData().OutfitIdToUserAuthoredTemplateOutfit.ContainsKey(this.OutfitId);
		}

		// Token: 0x1700050B RID: 1291
		// (get) Token: 0x06005696 RID: 22166 RVA: 0x000D88FE File Offset: 0x000D6AFE
		public string OutfitId
		{
			get
			{
				return this.m_outfitId[0];
			}
		}

		// Token: 0x1700050C RID: 1292
		// (get) Token: 0x06005697 RID: 22167 RVA: 0x000D8908 File Offset: 0x000D6B08
		public ClothingOutfitUtility.OutfitType OutfitType
		{
			get
			{
				return this.m_outfitType;
			}
		}

		// Token: 0x06005698 RID: 22168 RVA: 0x000D8910 File Offset: 0x000D6B10
		public UserAuthoredTemplate(ClothingOutfitUtility.OutfitType outfitType, string outfitId)
		{
			this.m_outfitId = new string[]
			{
				outfitId
			};
			this.m_outfitType = outfitType;
		}

		// Token: 0x06005699 RID: 22169 RVA: 0x00282A6C File Offset: 0x00280C6C
		public string[] ReadItems(ClothingOutfitUtility.OutfitType outfitType)
		{
			SerializableOutfitData.Version2.CustomTemplateOutfitEntry customTemplateOutfitEntry;
			if (CustomClothingOutfits.Instance.Internal_GetOutfitData().OutfitIdToUserAuthoredTemplateOutfit.TryGetValue(this.OutfitId, out customTemplateOutfitEntry))
			{
				ClothingOutfitUtility.OutfitType outfitType2;
				global::Debug.Assert(Enum.TryParse<ClothingOutfitUtility.OutfitType>(customTemplateOutfitEntry.outfitType, true, out outfitType2) && outfitType2 == this.m_outfitType);
				return customTemplateOutfitEntry.itemIds;
			}
			return ClothingOutfitTarget.NO_ITEMS;
		}

		// Token: 0x0600569A RID: 22170 RVA: 0x000D8929 File Offset: 0x000D6B29
		public void WriteItems(ClothingOutfitUtility.OutfitType outfitType, string[] items)
		{
			CustomClothingOutfits.Instance.Internal_EditOutfit(outfitType, this.OutfitId, items);
		}

		// Token: 0x0600569B RID: 22171 RVA: 0x000D893D File Offset: 0x000D6B3D
		public string ReadName()
		{
			return this.OutfitId;
		}

		// Token: 0x0600569C RID: 22172 RVA: 0x00282AC4 File Offset: 0x00280CC4
		public void WriteName(string name)
		{
			if (this.OutfitId == name)
			{
				return;
			}
			if (ClothingOutfitTarget.DoesTemplateExist(name))
			{
				throw new Exception(string.Concat(new string[]
				{
					"Can not change outfit name from \"",
					this.OutfitId,
					"\" to \"",
					name,
					"\", \"",
					name,
					"\" already exists"
				}));
			}
			if (CustomClothingOutfits.Instance.Internal_GetOutfitData().OutfitIdToUserAuthoredTemplateOutfit.ContainsKey(this.OutfitId))
			{
				CustomClothingOutfits.Instance.Internal_RenameOutfit(this.m_outfitType, this.OutfitId, name);
			}
			else
			{
				CustomClothingOutfits.Instance.Internal_EditOutfit(this.m_outfitType, name, ClothingOutfitTarget.NO_ITEMS);
			}
			this.m_outfitId[0] = name;
		}

		// Token: 0x0600569D RID: 22173 RVA: 0x000D8945 File Offset: 0x000D6B45
		public void Delete()
		{
			CustomClothingOutfits.Instance.Internal_RemoveOutfit(this.m_outfitType, this.OutfitId);
		}

		// Token: 0x04003CA3 RID: 15523
		private readonly string[] m_outfitId;

		// Token: 0x04003CA4 RID: 15524
		private readonly ClothingOutfitUtility.OutfitType m_outfitType;
	}

	// Token: 0x02001087 RID: 4231
	public readonly struct DatabaseAuthoredTemplate : ClothingOutfitTarget.Implementation
	{
		// Token: 0x1700050D RID: 1293
		// (get) Token: 0x0600569E RID: 22174 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
		public bool CanWriteItems
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700050E RID: 1294
		// (get) Token: 0x0600569F RID: 22175 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
		public bool CanWriteName
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700050F RID: 1295
		// (get) Token: 0x060056A0 RID: 22176 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
		public bool CanDelete
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060056A1 RID: 22177 RVA: 0x000A65EC File Offset: 0x000A47EC
		public bool DoesExist()
		{
			return true;
		}

		// Token: 0x17000510 RID: 1296
		// (get) Token: 0x060056A2 RID: 22178 RVA: 0x000D895D File Offset: 0x000D6B5D
		public string OutfitId
		{
			get
			{
				return this.m_outfitId;
			}
		}

		// Token: 0x17000511 RID: 1297
		// (get) Token: 0x060056A3 RID: 22179 RVA: 0x000D8965 File Offset: 0x000D6B65
		public ClothingOutfitUtility.OutfitType OutfitType
		{
			get
			{
				return this.m_outfitType;
			}
		}

		// Token: 0x060056A4 RID: 22180 RVA: 0x000D896D File Offset: 0x000D6B6D
		public DatabaseAuthoredTemplate(ClothingOutfitResource outfit)
		{
			this.m_outfitId = outfit.Id;
			this.m_outfitType = outfit.outfitType;
			this.resource = outfit;
		}

		// Token: 0x060056A5 RID: 22181 RVA: 0x000D898E File Offset: 0x000D6B8E
		public string[] ReadItems(ClothingOutfitUtility.OutfitType outfitType)
		{
			return this.resource.itemsInOutfit;
		}

		// Token: 0x060056A6 RID: 22182 RVA: 0x000D899B File Offset: 0x000D6B9B
		public void WriteItems(ClothingOutfitUtility.OutfitType outfitType, string[] items)
		{
			throw new InvalidOperationException("Can not set items on a Db authored outfit");
		}

		// Token: 0x060056A7 RID: 22183 RVA: 0x000D89A7 File Offset: 0x000D6BA7
		public string ReadName()
		{
			return this.resource.Name;
		}

		// Token: 0x060056A8 RID: 22184 RVA: 0x000D89B4 File Offset: 0x000D6BB4
		public void WriteName(string name)
		{
			throw new InvalidOperationException("Can not set name on a Db authored outfit");
		}

		// Token: 0x060056A9 RID: 22185 RVA: 0x000D89C0 File Offset: 0x000D6BC0
		public void Delete()
		{
			throw new InvalidOperationException("Can not delete a Db authored outfit");
		}

		// Token: 0x04003CA5 RID: 15525
		public readonly ClothingOutfitResource resource;

		// Token: 0x04003CA6 RID: 15526
		private readonly string m_outfitId;

		// Token: 0x04003CA7 RID: 15527
		private readonly ClothingOutfitUtility.OutfitType m_outfitType;
	}
}
