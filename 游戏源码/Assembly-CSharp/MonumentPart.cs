using System.Runtime.Serialization;
using Database;
using KSerialization;
using TUNING;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/MonumentPart")]
public class MonumentPart : KMonoBehaviour
{
	public MonumentPartResource.Part part;

	public string stateUISymbol;

	[Serialize]
	private string chosenState;

	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.MonumentParts.Add(this);
		if (!string.IsNullOrEmpty(chosenState))
		{
			SetState(chosenState);
		}
		UpdateMonumentDecor();
	}

	[OnDeserialized]
	private void OnDeserializedMethod()
	{
		if (Db.GetMonumentParts().TryGet(chosenState) == null)
		{
			string id = "";
			if (part == MonumentPartResource.Part.Bottom)
			{
				id = "bottom_" + chosenState;
			}
			else if (part == MonumentPartResource.Part.Middle)
			{
				id = "mid_" + chosenState;
			}
			else if (part == MonumentPartResource.Part.Top)
			{
				id = "top_" + chosenState;
			}
			if (Db.GetMonumentParts().TryGet(id) != null)
			{
				chosenState = id;
			}
		}
	}

	protected override void OnCleanUp()
	{
		Components.MonumentParts.Remove(this);
		RemoveMonumentPiece();
		base.OnCleanUp();
	}

	public void SetState(string state)
	{
		MonumentPartResource monumentPartResource = Db.GetMonumentParts().Get(state);
		KBatchedAnimController component = GetComponent<KBatchedAnimController>();
		component.SwapAnims(new KAnimFile[1] { monumentPartResource.AnimFile });
		component.Play(monumentPartResource.State);
		chosenState = state;
	}

	public bool IsMonumentCompleted()
	{
		bool num = GetMonumentPart(MonumentPartResource.Part.Top) != null;
		bool flag = GetMonumentPart(MonumentPartResource.Part.Middle) != null;
		bool flag2 = GetMonumentPart(MonumentPartResource.Part.Bottom) != null;
		return num && flag2 && flag;
	}

	public void UpdateMonumentDecor()
	{
		GameObject monumentPart = GetMonumentPart(MonumentPartResource.Part.Middle);
		if (!IsMonumentCompleted())
		{
			return;
		}
		monumentPart.GetComponent<DecorProvider>().SetValues(BUILDINGS.DECOR.BONUS.MONUMENT.COMPLETE);
		foreach (GameObject item in AttachableBuilding.GetAttachedNetwork(GetComponent<AttachableBuilding>()))
		{
			if (item != monumentPart)
			{
				item.GetComponent<DecorProvider>().SetValues(BUILDINGS.DECOR.NONE);
			}
		}
	}

	public void RemoveMonumentPiece()
	{
		if (!IsMonumentCompleted())
		{
			return;
		}
		foreach (GameObject item in AttachableBuilding.GetAttachedNetwork(GetComponent<AttachableBuilding>()))
		{
			if (item.GetComponent<MonumentPart>() != this)
			{
				item.GetComponent<DecorProvider>().SetValues(BUILDINGS.DECOR.BONUS.MONUMENT.INCOMPLETE);
			}
		}
	}

	private GameObject GetMonumentPart(MonumentPartResource.Part requestPart)
	{
		foreach (GameObject item in AttachableBuilding.GetAttachedNetwork(GetComponent<AttachableBuilding>()))
		{
			MonumentPart component = item.GetComponent<MonumentPart>();
			if (!(component == null) && component.part == requestPart)
			{
				return item;
			}
		}
		return null;
	}
}
