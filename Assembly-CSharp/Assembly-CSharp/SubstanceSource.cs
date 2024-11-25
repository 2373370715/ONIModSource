using System;
using KSerialization;

[SerializationConfig(MemberSerialization.OptIn)]
public abstract class SubstanceSource : KMonoBehaviour
{
		protected override void OnPrefabInit()
	{
		this.pickupable.SetWorkTime(SubstanceSource.MaxPickupTime);
	}

		protected override void OnSpawn()
	{
		this.pickupable.SetWorkTime(10f);
	}

		protected abstract CellOffset[] GetOffsetGroup();

		protected abstract IChunkManager GetChunkManager();

		public SimHashes GetElementID()
	{
		return this.primaryElement.ElementID;
	}

		public Tag GetElementTag()
	{
		Tag result = Tag.Invalid;
		if (base.gameObject != null && this.primaryElement != null && this.primaryElement.Element != null)
		{
			result = this.primaryElement.Element.tag;
		}
		return result;
	}

		public Tag GetMaterialCategoryTag()
	{
		Tag result = Tag.Invalid;
		if (base.gameObject != null && this.primaryElement != null && this.primaryElement.Element != null)
		{
			result = this.primaryElement.Element.GetMaterialCategoryTag();
		}
		return result;
	}

		private bool enableRefresh;

		private static readonly float MaxPickupTime = 8f;

		[MyCmpReq]
	public Pickupable pickupable;

		[MyCmpReq]
	private PrimaryElement primaryElement;
}
