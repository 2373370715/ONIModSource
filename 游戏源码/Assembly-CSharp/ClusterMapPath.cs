using System;
using System.Collections.Generic;
using System.Linq;
using ProcGen;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

// Token: 0x02001C1A RID: 7194
public class ClusterMapPath : MonoBehaviour
{
	// Token: 0x0600958E RID: 38286 RVA: 0x00101625 File Offset: 0x000FF825
	public void Init()
	{
		this.lineRenderer = base.gameObject.GetComponentInChildren<UILineRenderer>();
		base.gameObject.SetActive(true);
	}

	// Token: 0x0600958F RID: 38287 RVA: 0x00101644 File Offset: 0x000FF844
	public void Init(List<Vector2> nodes, Color color)
	{
		this.m_nodes = nodes;
		this.m_color = color;
		this.lineRenderer = base.gameObject.GetComponentInChildren<UILineRenderer>();
		this.UpdateColor();
		this.UpdateRenderer();
		base.gameObject.SetActive(true);
	}

	// Token: 0x06009590 RID: 38288 RVA: 0x0010167D File Offset: 0x000FF87D
	public void SetColor(Color color)
	{
		this.m_color = color;
		this.UpdateColor();
	}

	// Token: 0x06009591 RID: 38289 RVA: 0x0010168C File Offset: 0x000FF88C
	private void UpdateColor()
	{
		this.lineRenderer.color = this.m_color;
		this.pathStart.color = this.m_color;
		this.pathEnd.color = this.m_color;
	}

	// Token: 0x06009592 RID: 38290 RVA: 0x001016C1 File Offset: 0x000FF8C1
	public void SetPoints(List<Vector2> points)
	{
		this.m_nodes = points;
		this.UpdateRenderer();
	}

	// Token: 0x06009593 RID: 38291 RVA: 0x0039CE6C File Offset: 0x0039B06C
	private void UpdateRenderer()
	{
		HashSet<Vector2> pointsOnCatmullRomSpline = ProcGen.Util.GetPointsOnCatmullRomSpline(this.m_nodes, 10);
		this.lineRenderer.Points = pointsOnCatmullRomSpline.ToArray<Vector2>();
		if (this.lineRenderer.Points.Length > 1)
		{
			this.pathStart.transform.localPosition = this.lineRenderer.Points[0];
			this.pathStart.gameObject.SetActive(true);
			Vector2 vector = this.lineRenderer.Points[this.lineRenderer.Points.Length - 1];
			Vector2 b = this.lineRenderer.Points[this.lineRenderer.Points.Length - 2];
			this.pathEnd.transform.localPosition = vector;
			Vector2 v = vector - b;
			this.pathEnd.transform.rotation = Quaternion.LookRotation(Vector3.forward, v);
			this.pathEnd.gameObject.SetActive(true);
			return;
		}
		this.pathStart.gameObject.SetActive(false);
		this.pathEnd.gameObject.SetActive(false);
	}

	// Token: 0x06009594 RID: 38292 RVA: 0x0039CF94 File Offset: 0x0039B194
	public float GetRotationForNextSegment()
	{
		if (this.m_nodes.Count > 1)
		{
			Vector2 b = this.m_nodes[0];
			Vector2 to = this.m_nodes[1] - b;
			return Vector2.SignedAngle(Vector2.up, to);
		}
		return 0f;
	}

	// Token: 0x04007424 RID: 29732
	private List<Vector2> m_nodes;

	// Token: 0x04007425 RID: 29733
	private Color m_color;

	// Token: 0x04007426 RID: 29734
	public UILineRenderer lineRenderer;

	// Token: 0x04007427 RID: 29735
	public Image pathStart;

	// Token: 0x04007428 RID: 29736
	public Image pathEnd;
}
