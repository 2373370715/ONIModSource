using System;
using TUNING;
using UnityEngine;

// Token: 0x02000D47 RID: 3399
[AddComponentMenu("KMonoBehaviour/scripts/EggCracker")]
public class EggCracker : KMonoBehaviour
{
	// Token: 0x06004292 RID: 17042 RVA: 0x00241CBC File Offset: 0x0023FEBC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.refinery.choreType = Db.Get().ChoreTypes.Cook;
		this.refinery.fetchChoreTypeIdHash = Db.Get().ChoreTypes.CookFetch.IdHash;
		this.workable.WorkerStatusItem = Db.Get().DuplicantStatusItems.Processing;
		this.workable.AttributeConverter = Db.Get().AttributeConverters.CookingSpeed;
		this.workable.AttributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;
		this.workable.SkillExperienceSkillGroup = Db.Get().SkillGroups.Cooking.Id;
		this.workable.SkillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;
		ComplexFabricatorWorkable complexFabricatorWorkable = this.workable;
		complexFabricatorWorkable.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Combine(complexFabricatorWorkable.OnWorkableEventCB, new Action<Workable, Workable.WorkableEvent>(this.OnWorkableEvent));
	}

	// Token: 0x06004293 RID: 17043 RVA: 0x000CB03B File Offset: 0x000C923B
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		UnityEngine.Object.Destroy(this.tracker);
		this.tracker = null;
	}

	// Token: 0x06004294 RID: 17044 RVA: 0x00241DA4 File Offset: 0x0023FFA4
	private void OnWorkableEvent(Workable workable, Workable.WorkableEvent e)
	{
		if (e == Workable.WorkableEvent.WorkStarted)
		{
			ComplexRecipe currentWorkingOrder = this.refinery.CurrentWorkingOrder;
			if (currentWorkingOrder != null)
			{
				ComplexRecipe.RecipeElement[] ingredients = currentWorkingOrder.ingredients;
				if (ingredients.Length != 0)
				{
					ComplexRecipe.RecipeElement recipeElement = ingredients[0];
					this.display_egg = this.refinery.buildStorage.FindFirst(recipeElement.material);
					this.PositionActiveEgg();
					return;
				}
			}
		}
		else if (e == Workable.WorkableEvent.WorkCompleted)
		{
			if (this.display_egg)
			{
				this.display_egg.GetComponent<KBatchedAnimController>().Play("hatching_pst", KAnim.PlayMode.Once, 1f, 0f);
				return;
			}
		}
		else if (e == Workable.WorkableEvent.WorkStopped)
		{
			UnityEngine.Object.Destroy(this.tracker);
			this.tracker = null;
			this.display_egg = null;
		}
	}

	// Token: 0x06004295 RID: 17045 RVA: 0x00241E4C File Offset: 0x0024004C
	private void PositionActiveEgg()
	{
		if (!this.display_egg)
		{
			return;
		}
		KBatchedAnimController component = this.display_egg.GetComponent<KBatchedAnimController>();
		component.enabled = true;
		component.SetSceneLayer(Grid.SceneLayer.BuildingUse);
		KSelectable component2 = this.display_egg.GetComponent<KSelectable>();
		if (component2 != null)
		{
			component2.enabled = true;
		}
		this.tracker = this.display_egg.AddComponent<KBatchedAnimTracker>();
		this.tracker.symbol = "snapto_egg";
	}

	// Token: 0x04002D82 RID: 11650
	[MyCmpReq]
	private ComplexFabricator refinery;

	// Token: 0x04002D83 RID: 11651
	[MyCmpReq]
	private ComplexFabricatorWorkable workable;

	// Token: 0x04002D84 RID: 11652
	private KBatchedAnimTracker tracker;

	// Token: 0x04002D85 RID: 11653
	private GameObject display_egg;
}
