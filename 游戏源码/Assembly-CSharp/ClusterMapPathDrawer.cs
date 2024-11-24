using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02001C1B RID: 7195
public class ClusterMapPathDrawer : MonoBehaviour
{
	// Token: 0x06009596 RID: 38294 RVA: 0x001016D0 File Offset: 0x000FF8D0
	public ClusterMapPath AddPath()
	{
		ClusterMapPath clusterMapPath = UnityEngine.Object.Instantiate<ClusterMapPath>(this.pathPrefab, this.pathContainer);
		clusterMapPath.Init();
		return clusterMapPath;
	}

	// Token: 0x06009597 RID: 38295 RVA: 0x001016E9 File Offset: 0x000FF8E9
	public static List<Vector2> GetDrawPathList(Vector2 startLocation, List<AxialI> pathPoints)
	{
		List<Vector2> list = new List<Vector2>();
		list.Add(startLocation);
		list.AddRange(from point in pathPoints
		select point.ToWorld2D());
		return list;
	}

	// Token: 0x04007429 RID: 29737
	public ClusterMapPath pathPrefab;

	// Token: 0x0400742A RID: 29738
	public Transform pathContainer;
}
