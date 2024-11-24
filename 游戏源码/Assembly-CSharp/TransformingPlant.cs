using System;
using Klei.AI;
using UnityEngine;

// Token: 0x020002E0 RID: 736
public class TransformingPlant : KMonoBehaviour
{
	// Token: 0x06000B84 RID: 2948 RVA: 0x000AB748 File Offset: 0x000A9948
	public void SubscribeToTransformEvent(GameHashes eventHash)
	{
		base.Subscribe<TransformingPlant>((int)eventHash, TransformingPlant.OnTransformationEventDelegate);
	}

	// Token: 0x06000B85 RID: 2949 RVA: 0x000AB757 File Offset: 0x000A9957
	public void UnsubscribeToTransformEvent(GameHashes eventHash)
	{
		base.Unsubscribe<TransformingPlant>((int)eventHash, TransformingPlant.OnTransformationEventDelegate, false);
	}

	// Token: 0x06000B86 RID: 2950 RVA: 0x0016F638 File Offset: 0x0016D838
	private void DoPlantTransform(object data)
	{
		if (this.eventDataCondition != null && !this.eventDataCondition(data))
		{
			return;
		}
		GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab(this.transformPlantId.ToTag()), Grid.SceneLayer.BuildingBack, null, 0);
		gameObject.transform.SetPosition(base.transform.GetPosition());
		MutantPlant component = base.GetComponent<MutantPlant>();
		MutantPlant component2 = gameObject.GetComponent<MutantPlant>();
		if (component != null && gameObject != null)
		{
			component.CopyMutationsTo(component2);
			PlantSubSpeciesCatalog.Instance.IdentifySubSpecies(component2.SubSpeciesID);
		}
		gameObject.SetActive(true);
		Growing component3 = base.GetComponent<Growing>();
		Growing component4 = gameObject.GetComponent<Growing>();
		if (component3 != null && component4 != null)
		{
			float num = component3.PercentGrown();
			if (this.useGrowthTimeRatio)
			{
				AmountInstance amountInstance = component3.GetAmounts().Get(Db.Get().Amounts.Maturity);
				AmountInstance amountInstance2 = component4.GetAmounts().Get(Db.Get().Amounts.Maturity);
				float num2 = amountInstance.GetMax() / amountInstance2.GetMax();
				num = Mathf.Clamp01(num * num2);
			}
			component4.OverrideMaturityLevel(num);
		}
		PrimaryElement component5 = gameObject.GetComponent<PrimaryElement>();
		PrimaryElement component6 = base.GetComponent<PrimaryElement>();
		component5.Temperature = component6.Temperature;
		component5.AddDisease(component6.DiseaseIdx, component6.DiseaseCount, "TransformedPlant");
		gameObject.GetComponent<Effects>().CopyEffects(base.GetComponent<Effects>());
		HarvestDesignatable component7 = base.GetComponent<HarvestDesignatable>();
		HarvestDesignatable component8 = gameObject.GetComponent<HarvestDesignatable>();
		if (component7 != null && component8 != null)
		{
			component8.SetHarvestWhenReady(component7.HarvestWhenReady);
		}
		Prioritizable component9 = base.GetComponent<Prioritizable>();
		Prioritizable component10 = gameObject.GetComponent<Prioritizable>();
		if (component9 != null && component10 != null)
		{
			component10.SetMasterPriority(component9.GetMasterPriority());
		}
		PlantablePlot receptacle = base.GetComponent<ReceptacleMonitor>().GetReceptacle();
		if (receptacle != null)
		{
			receptacle.ReplacePlant(gameObject, this.keepPlantablePlotStorage);
		}
		Util.KDestroyGameObject(base.gameObject);
		if (this.fxKAnim != null)
		{
			KBatchedAnimController kbatchedAnimController = FXHelpers.CreateEffect(this.fxKAnim, gameObject.transform.position, null, false, Grid.SceneLayer.FXFront, false);
			kbatchedAnimController.Play(this.fxAnim, KAnim.PlayMode.Once, 1f, 0f);
			kbatchedAnimController.destroyOnAnimComplete = true;
		}
	}

	// Token: 0x040008D7 RID: 2263
	public string transformPlantId;

	// Token: 0x040008D8 RID: 2264
	public Func<object, bool> eventDataCondition;

	// Token: 0x040008D9 RID: 2265
	public bool useGrowthTimeRatio;

	// Token: 0x040008DA RID: 2266
	public bool keepPlantablePlotStorage = true;

	// Token: 0x040008DB RID: 2267
	public string fxKAnim;

	// Token: 0x040008DC RID: 2268
	public string fxAnim;

	// Token: 0x040008DD RID: 2269
	private static readonly EventSystem.IntraObjectHandler<TransformingPlant> OnTransformationEventDelegate = new EventSystem.IntraObjectHandler<TransformingPlant>(delegate(TransformingPlant component, object data)
	{
		component.DoPlantTransform(data);
	});
}
