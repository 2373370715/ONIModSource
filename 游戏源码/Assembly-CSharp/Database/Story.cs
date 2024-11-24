using System;
using ProcGen;

namespace Database;

public class Story : Resource, IComparable<Story>
{
	public const int MODDED_STORY = -1;

	public int kleiUseOnlyCoordinateOrder;

	public bool autoStart;

	public string keepsakePrefabId;

	public readonly string worldgenStoryTraitKey;

	private readonly int displayOrder;

	private readonly int updateNumber;

	public string sandboxStampTemplateId;

	private WorldTrait _cachedStoryTrait;

	public int HashId { get; private set; }

	public WorldTrait StoryTrait
	{
		get
		{
			if (_cachedStoryTrait == null)
			{
				_cachedStoryTrait = SettingsCache.GetCachedStoryTrait(worldgenStoryTraitKey, assertMissingTrait: false);
			}
			return _cachedStoryTrait;
		}
	}

	public Story(string id, string worldgenStoryTraitKey, int displayOrder)
	{
		Id = id;
		this.worldgenStoryTraitKey = worldgenStoryTraitKey;
		this.displayOrder = displayOrder;
		kleiUseOnlyCoordinateOrder = -1;
		updateNumber = -1;
		sandboxStampTemplateId = null;
		HashId = Hash.SDBMLower(id);
	}

	public Story(string id, string worldgenStoryTraitKey, int displayOrder, int kleiUseOnlyCoordinateOrder, int updateNumber, string sandboxStampTemplateId)
	{
		Id = id;
		this.worldgenStoryTraitKey = worldgenStoryTraitKey;
		this.displayOrder = displayOrder;
		this.updateNumber = updateNumber;
		this.sandboxStampTemplateId = sandboxStampTemplateId;
		this.kleiUseOnlyCoordinateOrder = kleiUseOnlyCoordinateOrder;
		HashId = Hash.SDBMLower(id);
	}

	public int CompareTo(Story other)
	{
		return displayOrder.CompareTo(other.displayOrder);
	}

	public bool IsNew()
	{
		return updateNumber == LaunchInitializer.UpdateNumber();
	}

	public Story AutoStart()
	{
		autoStart = true;
		return this;
	}

	public Story SetKeepsake(string prefabId)
	{
		keepsakePrefabId = prefabId;
		return this;
	}
}
