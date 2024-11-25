using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ClusterMapPathDrawer : MonoBehaviour
{
		public ClusterMapPath AddPath()
	{
		ClusterMapPath clusterMapPath = UnityEngine.Object.Instantiate<ClusterMapPath>(this.pathPrefab, this.pathContainer);
		clusterMapPath.Init();
		return clusterMapPath;
	}

		public static List<Vector2> GetDrawPathList(Vector2 startLocation, List<AxialI> pathPoints)
	{
		List<Vector2> list = new List<Vector2>();
		list.Add(startLocation);
		list.AddRange(from point in pathPoints
		select point.ToWorld2D());
		return list;
	}

		public ClusterMapPath pathPrefab;

		public Transform pathContainer;
}
