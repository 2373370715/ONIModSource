using System;
using System.Collections.Generic;
using Database;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001F25 RID: 7973
public class ArtableSelectionSideScreen : SideScreenContent
{
	// Token: 0x0600A833 RID: 43059 RVA: 0x003FC018 File Offset: 0x003FA218
	public override bool IsValidForTarget(GameObject target)
	{
		Artable component = target.GetComponent<Artable>();
		return !(component == null) && !(component.CurrentStage == "Default");
	}

	// Token: 0x0600A834 RID: 43060 RVA: 0x0010D2F1 File Offset: 0x0010B4F1
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.applyButton.onClick += delegate()
		{
			this.target.SetUserChosenTargetState(this.selectedStage);
			SelectTool.Instance.Select(null, true);
		};
		this.clearButton.onClick += delegate()
		{
			this.selectedStage = "";
			this.target.SetDefault();
			SelectTool.Instance.Select(null, true);
		};
	}

	// Token: 0x0600A835 RID: 43061 RVA: 0x003FC04C File Offset: 0x003FA24C
	public override void SetTarget(GameObject target)
	{
		if (this.workCompleteSub != -1)
		{
			target.Unsubscribe(this.workCompleteSub);
			this.workCompleteSub = -1;
		}
		base.SetTarget(target);
		this.target = target.GetComponent<Artable>();
		this.workCompleteSub = target.Subscribe(-2011693419, new Action<object>(this.OnRefreshTarget));
		this.OnRefreshTarget(null);
	}

	// Token: 0x0600A836 RID: 43062 RVA: 0x0010D327 File Offset: 0x0010B527
	public override void ClearTarget()
	{
		this.target.Unsubscribe(-2011693419);
		this.workCompleteSub = -1;
		base.ClearTarget();
	}

	// Token: 0x0600A837 RID: 43063 RVA: 0x0010D346 File Offset: 0x0010B546
	private void OnRefreshTarget(object data = null)
	{
		if (this.target == null)
		{
			return;
		}
		this.GenerateStateButtons();
		this.selectedStage = this.target.CurrentStage;
		this.RefreshButtons();
	}

	// Token: 0x0600A838 RID: 43064 RVA: 0x003FC0AC File Offset: 0x003FA2AC
	public void GenerateStateButtons()
	{
		foreach (KeyValuePair<string, MultiToggle> keyValuePair in this.buttons)
		{
			Util.KDestroyGameObject(keyValuePair.Value.gameObject);
		}
		this.buttons.Clear();
		foreach (ArtableStage artableStage in Db.GetArtableStages().GetPrefabStages(this.target.GetComponent<KPrefabID>().PrefabID()))
		{
			if (!(artableStage.id == "Default"))
			{
				GameObject gameObject = Util.KInstantiateUI(this.stateButtonPrefab, this.buttonContainer.gameObject, true);
				Sprite sprite = artableStage.GetPermitPresentationInfo().sprite;
				MultiToggle component = gameObject.GetComponent<MultiToggle>();
				component.GetComponent<ToolTip>().SetSimpleTooltip(artableStage.Name);
				component.GetComponent<HierarchyReferences>().GetReference<Image>("Icon").sprite = sprite;
				this.buttons.Add(artableStage.id, component);
			}
		}
	}

	// Token: 0x0600A839 RID: 43065 RVA: 0x003FC1E4 File Offset: 0x003FA3E4
	private void RefreshButtons()
	{
		List<ArtableStage> prefabStages = Db.GetArtableStages().GetPrefabStages(this.target.GetComponent<KPrefabID>().PrefabID());
		ArtableStage artableStage = prefabStages.Find((ArtableStage match) => match.id == this.target.CurrentStage);
		int num = 0;
		using (Dictionary<string, MultiToggle>.Enumerator enumerator = this.buttons.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ArtableSelectionSideScreen.<>c__DisplayClass16_0 CS$<>8__locals1 = new ArtableSelectionSideScreen.<>c__DisplayClass16_0();
				CS$<>8__locals1.<>4__this = this;
				CS$<>8__locals1.kvp = enumerator.Current;
				ArtableStage stage = prefabStages.Find((ArtableStage match) => match.id == CS$<>8__locals1.kvp.Key);
				if (stage != null && artableStage != null && stage.statusItem.StatusType != artableStage.statusItem.StatusType)
				{
					CS$<>8__locals1.kvp.Value.gameObject.SetActive(false);
				}
				else if (!stage.IsUnlocked())
				{
					CS$<>8__locals1.kvp.Value.gameObject.SetActive(false);
				}
				else
				{
					num++;
					CS$<>8__locals1.kvp.Value.gameObject.SetActive(true);
					CS$<>8__locals1.kvp.Value.ChangeState((this.selectedStage == CS$<>8__locals1.kvp.Key) ? 1 : 0);
					MultiToggle value = CS$<>8__locals1.kvp.Value;
					value.onClick = (System.Action)Delegate.Combine(value.onClick, new System.Action(delegate()
					{
						CS$<>8__locals1.<>4__this.selectedStage = stage.id;
						CS$<>8__locals1.<>4__this.RefreshButtons();
					}));
				}
			}
		}
		this.scrollTransoform.GetComponent<LayoutElement>().preferredHeight = (float)((num > 3) ? 200 : 100);
	}

	// Token: 0x04008439 RID: 33849
	private Artable target;

	// Token: 0x0400843A RID: 33850
	public KButton applyButton;

	// Token: 0x0400843B RID: 33851
	public KButton clearButton;

	// Token: 0x0400843C RID: 33852
	public GameObject stateButtonPrefab;

	// Token: 0x0400843D RID: 33853
	private Dictionary<string, MultiToggle> buttons = new Dictionary<string, MultiToggle>();

	// Token: 0x0400843E RID: 33854
	[SerializeField]
	private RectTransform scrollTransoform;

	// Token: 0x0400843F RID: 33855
	private string selectedStage = "";

	// Token: 0x04008440 RID: 33856
	private const int INVALID_SUBSCRIPTION = -1;

	// Token: 0x04008441 RID: 33857
	private int workCompleteSub = -1;

	// Token: 0x04008442 RID: 33858
	[SerializeField]
	private RectTransform buttonContainer;
}
