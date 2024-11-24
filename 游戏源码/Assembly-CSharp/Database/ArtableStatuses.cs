namespace Database;

public class ArtableStatuses : ResourceSet<ArtableStatusItem>
{
	public enum ArtableStatusType
	{
		AwaitingArting,
		LookingUgly,
		LookingOkay,
		LookingGreat
	}

	public ArtableStatusItem AwaitingArting;

	public ArtableStatusItem LookingUgly;

	public ArtableStatusItem LookingOkay;

	public ArtableStatusItem LookingGreat;

	public ArtableStatuses(ResourceSet parent)
		: base("ArtableStatuses", parent)
	{
		AwaitingArting = Add("AwaitingArting", ArtableStatusType.AwaitingArting);
		LookingUgly = Add("LookingUgly", ArtableStatusType.LookingUgly);
		LookingOkay = Add("LookingOkay", ArtableStatusType.LookingOkay);
		LookingGreat = Add("LookingGreat", ArtableStatusType.LookingGreat);
	}

	public ArtableStatusItem Add(string id, ArtableStatusType statusType)
	{
		ArtableStatusItem artableStatusItem = new ArtableStatusItem(id, statusType);
		resources.Add(artableStatusItem);
		return artableStatusItem;
	}
}
