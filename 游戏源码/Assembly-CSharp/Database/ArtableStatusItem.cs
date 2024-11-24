namespace Database;

public class ArtableStatusItem : StatusItem
{
	public ArtableStatuses.ArtableStatusType StatusType;

	public ArtableStatusItem(string id, ArtableStatuses.ArtableStatusType statusType)
		: base(id, "BUILDING", "", IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID)
	{
		StatusType = statusType;
	}
}
