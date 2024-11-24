using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001F12 RID: 7954
[AddComponentMenu("KMonoBehaviour/scripts/ScheduledUIInstantiation")]
public class ScheduledUIInstantiation : KMonoBehaviour
{
	// Token: 0x0600A7CC RID: 42956 RVA: 0x0010CDD8 File Offset: 0x0010AFD8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (this.InstantiateOnAwake)
		{
			this.InstantiateElements(null);
			return;
		}
		Game.Instance.Subscribe((int)this.InstantiationEvent, new Action<object>(this.InstantiateElements));
	}

	// Token: 0x0600A7CD RID: 42957 RVA: 0x003FA5BC File Offset: 0x003F87BC
	public void InstantiateElements(object data)
	{
		if (this.completed)
		{
			return;
		}
		this.completed = true;
		foreach (ScheduledUIInstantiation.Instantiation instantiation in this.UIElements)
		{
			if (SaveLoader.Instance.IsDLCActiveForCurrentSave(instantiation.RequiredDlcId))
			{
				foreach (GameObject gameObject in instantiation.prefabs)
				{
					Vector3 v = gameObject.rectTransform().anchoredPosition;
					GameObject gameObject2 = Util.KInstantiateUI(gameObject, instantiation.parent.gameObject, false);
					gameObject2.rectTransform().anchoredPosition = v;
					gameObject2.rectTransform().localScale = Vector3.one;
					this.instantiatedObjects.Add(gameObject2);
				}
			}
		}
		if (!this.InstantiateOnAwake)
		{
			base.Unsubscribe((int)this.InstantiationEvent, new Action<object>(this.InstantiateElements));
		}
	}

	// Token: 0x0600A7CE RID: 42958 RVA: 0x003FA6A4 File Offset: 0x003F88A4
	public T GetInstantiatedObject<T>() where T : Component
	{
		for (int i = 0; i < this.instantiatedObjects.Count; i++)
		{
			if (this.instantiatedObjects[i].GetComponent(typeof(T)) != null)
			{
				return this.instantiatedObjects[i].GetComponent(typeof(T)) as T;
			}
		}
		return default(T);
	}

	// Token: 0x040083E6 RID: 33766
	public ScheduledUIInstantiation.Instantiation[] UIElements;

	// Token: 0x040083E7 RID: 33767
	public bool InstantiateOnAwake;

	// Token: 0x040083E8 RID: 33768
	public GameHashes InstantiationEvent = GameHashes.StartGameUser;

	// Token: 0x040083E9 RID: 33769
	private bool completed;

	// Token: 0x040083EA RID: 33770
	private List<GameObject> instantiatedObjects = new List<GameObject>();

	// Token: 0x02001F13 RID: 7955
	[Serializable]
	public struct Instantiation
	{
		// Token: 0x040083EB RID: 33771
		public string Name;

		// Token: 0x040083EC RID: 33772
		public string Comment;

		// Token: 0x040083ED RID: 33773
		public GameObject[] prefabs;

		// Token: 0x040083EE RID: 33774
		public Transform parent;

		// Token: 0x040083EF RID: 33775
		public string RequiredDlcId;
	}
}
