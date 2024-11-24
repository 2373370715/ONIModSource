﻿using System;
using System.Runtime.Serialization;
using Database;
using KSerialization;
using TUNING;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/MonumentPart")]
public class MonumentPart : KMonoBehaviour
{
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.MonumentParts.Add(this);
		if (!string.IsNullOrEmpty(this.chosenState))
		{
			this.SetState(this.chosenState);
		}
		this.UpdateMonumentDecor();
	}

	[OnDeserialized]
	private void OnDeserializedMethod()
	{
		if (Db.GetMonumentParts().TryGet(this.chosenState) == null)
		{
			string id = "";
			if (this.part == MonumentPartResource.Part.Bottom)
			{
				id = "bottom_" + this.chosenState;
			}
			else if (this.part == MonumentPartResource.Part.Middle)
			{
				id = "mid_" + this.chosenState;
			}
			else if (this.part == MonumentPartResource.Part.Top)
			{
				id = "top_" + this.chosenState;
			}
			if (Db.GetMonumentParts().TryGet(id) != null)
			{
				this.chosenState = id;
			}
		}
	}

	protected override void OnCleanUp()
	{
		Components.MonumentParts.Remove(this);
		this.RemoveMonumentPiece();
		base.OnCleanUp();
	}

	public void SetState(string state)
	{
		MonumentPartResource monumentPartResource = Db.GetMonumentParts().Get(state);
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		component.SwapAnims(new KAnimFile[]
		{
			monumentPartResource.AnimFile
		});
		component.Play(monumentPartResource.State, KAnim.PlayMode.Once, 1f, 0f);
		this.chosenState = state;
	}

	public bool IsMonumentCompleted()
	{
		bool flag = this.GetMonumentPart(MonumentPartResource.Part.Top) != null;
		bool flag2 = this.GetMonumentPart(MonumentPartResource.Part.Middle) != null;
		bool flag3 = this.GetMonumentPart(MonumentPartResource.Part.Bottom) != null;
		return flag && flag3 && flag2;
	}

	public void UpdateMonumentDecor()
	{
		GameObject monumentPart = this.GetMonumentPart(MonumentPartResource.Part.Middle);
		if (this.IsMonumentCompleted())
		{
			monumentPart.GetComponent<DecorProvider>().SetValues(BUILDINGS.DECOR.BONUS.MONUMENT.COMPLETE);
			foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(base.GetComponent<AttachableBuilding>()))
			{
				if (gameObject != monumentPart)
				{
					gameObject.GetComponent<DecorProvider>().SetValues(BUILDINGS.DECOR.NONE);
				}
			}
		}
	}

	public void RemoveMonumentPiece()
	{
		if (this.IsMonumentCompleted())
		{
			foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(base.GetComponent<AttachableBuilding>()))
			{
				if (gameObject.GetComponent<MonumentPart>() != this)
				{
					gameObject.GetComponent<DecorProvider>().SetValues(BUILDINGS.DECOR.BONUS.MONUMENT.INCOMPLETE);
				}
			}
		}
	}

	private GameObject GetMonumentPart(MonumentPartResource.Part requestPart)
	{
		foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(base.GetComponent<AttachableBuilding>()))
		{
			MonumentPart component = gameObject.GetComponent<MonumentPart>();
			if (!(component == null) && component.part == requestPart)
			{
				return gameObject;
			}
		}
		return null;
	}

	public MonumentPartResource.Part part;

	public string stateUISymbol;

	[Serialize]
	private string chosenState;
}
