using System;

// Token: 0x02001961 RID: 6497
public class ClusterMapIconFixRotation : KMonoBehaviour
{
	// Token: 0x06008788 RID: 34696 RVA: 0x00350E0C File Offset: 0x0034F00C
	private void Update()
	{
		if (base.transform.parent != null)
		{
			float z = base.transform.parent.rotation.eulerAngles.z;
			this.rotation = -z;
			this.animController.Rotation = this.rotation;
		}
	}

	// Token: 0x04006632 RID: 26162
	[MyCmpGet]
	private KBatchedAnimController animController;

	// Token: 0x04006633 RID: 26163
	private float rotation;
}
