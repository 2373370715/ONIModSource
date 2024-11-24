using System;
using ProcGen;

namespace Database
{
	// Token: 0x0200217D RID: 8573
	public class Story : Resource, IComparable<Story>
	{
		// Token: 0x17000BBF RID: 3007
		// (get) Token: 0x0600B63B RID: 46651 RVA: 0x0011566B File Offset: 0x0011386B
		// (set) Token: 0x0600B63C RID: 46652 RVA: 0x00115673 File Offset: 0x00113873
		public int HashId { get; private set; }

		// Token: 0x17000BC0 RID: 3008
		// (get) Token: 0x0600B63D RID: 46653 RVA: 0x0011567C File Offset: 0x0011387C
		public WorldTrait StoryTrait
		{
			get
			{
				if (this._cachedStoryTrait == null)
				{
					this._cachedStoryTrait = SettingsCache.GetCachedStoryTrait(this.worldgenStoryTraitKey, false);
				}
				return this._cachedStoryTrait;
			}
		}

		// Token: 0x0600B63E RID: 46654 RVA: 0x0011569E File Offset: 0x0011389E
		public Story(string id, string worldgenStoryTraitKey, int displayOrder)
		{
			this.Id = id;
			this.worldgenStoryTraitKey = worldgenStoryTraitKey;
			this.displayOrder = displayOrder;
			this.kleiUseOnlyCoordinateOrder = -1;
			this.updateNumber = -1;
			this.sandboxStampTemplateId = null;
			this.HashId = Hash.SDBMLower(id);
		}

		// Token: 0x0600B63F RID: 46655 RVA: 0x00459BCC File Offset: 0x00457DCC
		public Story(string id, string worldgenStoryTraitKey, int displayOrder, int kleiUseOnlyCoordinateOrder, int updateNumber, string sandboxStampTemplateId)
		{
			this.Id = id;
			this.worldgenStoryTraitKey = worldgenStoryTraitKey;
			this.displayOrder = displayOrder;
			this.updateNumber = updateNumber;
			this.sandboxStampTemplateId = sandboxStampTemplateId;
			this.kleiUseOnlyCoordinateOrder = kleiUseOnlyCoordinateOrder;
			this.HashId = Hash.SDBMLower(id);
		}

		// Token: 0x0600B640 RID: 46656 RVA: 0x00459C18 File Offset: 0x00457E18
		public int CompareTo(Story other)
		{
			return this.displayOrder.CompareTo(other.displayOrder);
		}

		// Token: 0x0600B641 RID: 46657 RVA: 0x001156DC File Offset: 0x001138DC
		public bool IsNew()
		{
			return this.updateNumber == LaunchInitializer.UpdateNumber();
		}

		// Token: 0x0600B642 RID: 46658 RVA: 0x001156EB File Offset: 0x001138EB
		public Story AutoStart()
		{
			this.autoStart = true;
			return this;
		}

		// Token: 0x0600B643 RID: 46659 RVA: 0x001156F5 File Offset: 0x001138F5
		public Story SetKeepsake(string prefabId)
		{
			this.keepsakePrefabId = prefabId;
			return this;
		}

		// Token: 0x040094E9 RID: 38121
		public const int MODDED_STORY = -1;

		// Token: 0x040094EA RID: 38122
		public int kleiUseOnlyCoordinateOrder;

		// Token: 0x040094EC RID: 38124
		public bool autoStart;

		// Token: 0x040094ED RID: 38125
		public string keepsakePrefabId;

		// Token: 0x040094EE RID: 38126
		public readonly string worldgenStoryTraitKey;

		// Token: 0x040094EF RID: 38127
		private readonly int displayOrder;

		// Token: 0x040094F0 RID: 38128
		private readonly int updateNumber;

		// Token: 0x040094F1 RID: 38129
		public string sandboxStampTemplateId;

		// Token: 0x040094F2 RID: 38130
		private WorldTrait _cachedStoryTrait;
	}
}
