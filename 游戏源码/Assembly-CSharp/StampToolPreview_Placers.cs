using System;
using System.Collections.Generic;
using TemplateClasses;
using UnityEngine;

public class StampToolPreview_Placers : IStampToolPreviewPlugin
{
	private List<GameObject> inUse = new List<GameObject>();

	private GameObjectPool pool;

	private Transform poolParent;

	public StampToolPreview_Placers(GameObject placerPrefab)
	{
		StampToolPreview_Placers stampToolPreview_Placers = this;
		pool = new GameObjectPool(delegate
		{
			if (stampToolPreview_Placers.poolParent == null)
			{
				stampToolPreview_Placers.poolParent = new GameObject("StampToolPreview::PlacerPool").transform;
			}
			GameObject gameObject = Util.KInstantiate(placerPrefab, stampToolPreview_Placers.poolParent.gameObject);
			gameObject.SetActive(value: false);
			return gameObject;
		});
	}

	public void Setup(StampToolPreviewContext context)
	{
		for (int i = 0; i < context.stampTemplate.cells.Count; i++)
		{
			Cell cell = context.stampTemplate.cells[i];
			GameObject instance = pool.GetInstance();
			instance.transform.SetParent(context.previewParent.transform, worldPositionStays: false);
			instance.transform.localPosition = new Vector3(cell.location_x, cell.location_y);
			instance.SetActive(value: true);
			inUse.Add(instance);
		}
		context.onErrorChangeFn = (Action<string>)Delegate.Combine(context.onErrorChangeFn, (Action<string>)delegate(string error)
		{
			foreach (GameObject item in inUse)
			{
				if (!item.IsNullOrDestroyed())
				{
					item.GetComponentInChildren<MeshRenderer>().sharedMaterial.color = ((error != null) ? StampToolPreviewUtil.COLOR_ERROR : StampToolPreviewUtil.COLOR_OK);
				}
			}
		});
		context.cleanupFn = (System.Action)Delegate.Combine(context.cleanupFn, (System.Action)delegate
		{
			foreach (GameObject item2 in inUse)
			{
				if (!item2.IsNullOrDestroyed())
				{
					item2.SetActive(value: false);
					item2.transform.SetParent(poolParent);
					pool.ReleaseInstance(item2);
				}
			}
			inUse.Clear();
		});
	}
}
