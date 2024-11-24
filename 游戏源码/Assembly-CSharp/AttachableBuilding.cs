using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000C36 RID: 3126
[AddComponentMenu("KMonoBehaviour/scripts/AttachableBuilding")]
public class AttachableBuilding : KMonoBehaviour
{
	// Token: 0x06003BEF RID: 15343 RVA: 0x0022C5F8 File Offset: 0x0022A7F8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.RegisterWithAttachPoint(true);
		Components.AttachableBuildings.Add(this);
		foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(this))
		{
			AttachableBuilding component = gameObject.GetComponent<AttachableBuilding>();
			if (component != null && component.onAttachmentNetworkChanged != null)
			{
				component.onAttachmentNetworkChanged(this);
			}
		}
	}

	// Token: 0x06003BF0 RID: 15344 RVA: 0x000BFD08 File Offset: 0x000BDF08
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x06003BF1 RID: 15345 RVA: 0x0022C680 File Offset: 0x0022A880
	public void RegisterWithAttachPoint(bool register)
	{
		BuildingDef buildingDef = null;
		BuildingComplete component = base.GetComponent<BuildingComplete>();
		BuildingUnderConstruction component2 = base.GetComponent<BuildingUnderConstruction>();
		if (component != null)
		{
			buildingDef = component.Def;
		}
		else if (component2 != null)
		{
			buildingDef = component2.Def;
		}
		int num = Grid.OffsetCell(Grid.PosToCell(base.gameObject), buildingDef.attachablePosition);
		bool flag = false;
		int num2 = 0;
		while (!flag && num2 < Components.BuildingAttachPoints.Count)
		{
			for (int i = 0; i < Components.BuildingAttachPoints[num2].points.Length; i++)
			{
				if (num == Grid.OffsetCell(Grid.PosToCell(Components.BuildingAttachPoints[num2]), Components.BuildingAttachPoints[num2].points[i].position))
				{
					if (register)
					{
						Components.BuildingAttachPoints[num2].points[i].attachedBuilding = this;
					}
					else if (Components.BuildingAttachPoints[num2].points[i].attachedBuilding == this)
					{
						Components.BuildingAttachPoints[num2].points[i].attachedBuilding = null;
					}
					flag = true;
					break;
				}
			}
			num2++;
		}
	}

	// Token: 0x06003BF2 RID: 15346 RVA: 0x0022C7C8 File Offset: 0x0022A9C8
	public static void GetAttachedBelow(AttachableBuilding searchStart, ref List<GameObject> buildings)
	{
		AttachableBuilding attachableBuilding = searchStart;
		while (attachableBuilding != null)
		{
			BuildingAttachPoint attachedTo = attachableBuilding.GetAttachedTo();
			attachableBuilding = null;
			if (attachedTo != null)
			{
				buildings.Add(attachedTo.gameObject);
				attachableBuilding = attachedTo.GetComponent<AttachableBuilding>();
			}
		}
	}

	// Token: 0x06003BF3 RID: 15347 RVA: 0x0022C808 File Offset: 0x0022AA08
	public static int CountAttachedBelow(AttachableBuilding searchStart)
	{
		int num = 0;
		AttachableBuilding attachableBuilding = searchStart;
		while (attachableBuilding != null)
		{
			BuildingAttachPoint attachedTo = attachableBuilding.GetAttachedTo();
			attachableBuilding = null;
			if (attachedTo != null)
			{
				num++;
				attachableBuilding = attachedTo.GetComponent<AttachableBuilding>();
			}
		}
		return num;
	}

	// Token: 0x06003BF4 RID: 15348 RVA: 0x0022C844 File Offset: 0x0022AA44
	public static void GetAttachedAbove(AttachableBuilding searchStart, ref List<GameObject> buildings)
	{
		BuildingAttachPoint buildingAttachPoint = searchStart.GetComponent<BuildingAttachPoint>();
		while (buildingAttachPoint != null)
		{
			bool flag = false;
			foreach (BuildingAttachPoint.HardPoint hardPoint in buildingAttachPoint.points)
			{
				if (flag)
				{
					break;
				}
				if (hardPoint.attachedBuilding != null)
				{
					foreach (object obj in Components.AttachableBuildings)
					{
						AttachableBuilding attachableBuilding = (AttachableBuilding)obj;
						if (attachableBuilding == hardPoint.attachedBuilding)
						{
							buildings.Add(attachableBuilding.gameObject);
							buildingAttachPoint = attachableBuilding.GetComponent<BuildingAttachPoint>();
							flag = true;
						}
					}
				}
			}
			if (!flag)
			{
				buildingAttachPoint = null;
			}
		}
	}

	// Token: 0x06003BF5 RID: 15349 RVA: 0x0022C920 File Offset: 0x0022AB20
	public static void NotifyBuildingsNetworkChanged(List<GameObject> buildings, AttachableBuilding attachable = null)
	{
		foreach (GameObject gameObject in buildings)
		{
			AttachableBuilding component = gameObject.GetComponent<AttachableBuilding>();
			if (component != null && component.onAttachmentNetworkChanged != null)
			{
				component.onAttachmentNetworkChanged(attachable);
			}
		}
	}

	// Token: 0x06003BF6 RID: 15350 RVA: 0x0022C98C File Offset: 0x0022AB8C
	public static List<GameObject> GetAttachedNetwork(AttachableBuilding searchStart)
	{
		List<GameObject> list = new List<GameObject>();
		list.Add(searchStart.gameObject);
		AttachableBuilding.GetAttachedAbove(searchStart, ref list);
		AttachableBuilding.GetAttachedBelow(searchStart, ref list);
		return list;
	}

	// Token: 0x06003BF7 RID: 15351 RVA: 0x0022C9BC File Offset: 0x0022ABBC
	public BuildingAttachPoint GetAttachedTo()
	{
		for (int i = 0; i < Components.BuildingAttachPoints.Count; i++)
		{
			for (int j = 0; j < Components.BuildingAttachPoints[i].points.Length; j++)
			{
				if (Components.BuildingAttachPoints[i].points[j].attachedBuilding == this && (Components.BuildingAttachPoints[i].points[j].attachedBuilding.GetComponent<Deconstructable>() == null || !Components.BuildingAttachPoints[i].points[j].attachedBuilding.GetComponent<Deconstructable>().HasBeenDestroyed))
				{
					return Components.BuildingAttachPoints[i];
				}
			}
		}
		return null;
	}

	// Token: 0x06003BF8 RID: 15352 RVA: 0x000C6B47 File Offset: 0x000C4D47
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		AttachableBuilding.NotifyBuildingsNetworkChanged(AttachableBuilding.GetAttachedNetwork(this), this);
		this.RegisterWithAttachPoint(false);
		Components.AttachableBuildings.Remove(this);
	}

	// Token: 0x04002902 RID: 10498
	public Tag attachableToTag;

	// Token: 0x04002903 RID: 10499
	public Action<object> onAttachmentNetworkChanged;
}
