public class ClusterMapIconFixRotation : KMonoBehaviour
{
	[MyCmpGet]
	private KBatchedAnimController animController;

	private float rotation;

	private void Update()
	{
		if (base.transform.parent != null)
		{
			float z = base.transform.parent.rotation.eulerAngles.z;
			rotation = 0f - z;
			animController.Rotation = rotation;
		}
	}
}
