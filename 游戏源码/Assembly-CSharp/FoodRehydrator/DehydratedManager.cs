using System;
using System.Collections.Generic;
using Klei;
using KSerialization;
using STRINGS;
using UnityEngine;

namespace FoodRehydrator
{
	// Token: 0x020020EB RID: 8427
	public class DehydratedManager : KMonoBehaviour, FewOptionSideScreen.IFewOptionSideScreen
	{
		// Token: 0x0600B36C RID: 45932 RVA: 0x0011468B File Offset: 0x0011288B
		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			base.Subscribe<DehydratedManager>(-905833192, DehydratedManager.OnCopySettingsDelegate);
		}

		// Token: 0x17000BA4 RID: 2980
		// (get) Token: 0x0600B36D RID: 45933 RVA: 0x001146A4 File Offset: 0x001128A4
		// (set) Token: 0x0600B36E RID: 45934 RVA: 0x0043A590 File Offset: 0x00438790
		public Tag ChosenContent
		{
			get
			{
				return this.chosenContent;
			}
			set
			{
				if (this.chosenContent != value)
				{
					base.GetComponent<ManualDeliveryKG>().RequestedItemTag = value;
					this.chosenContent = value;
					this.packages.DropUnlessHasTag(this.chosenContent);
					if (this.chosenContent != GameTags.Dehydrated)
					{
						AccessabilityManager component = base.GetComponent<AccessabilityManager>();
						if (component != null)
						{
							component.CancelActiveWorkable();
						}
					}
				}
			}
		}

		// Token: 0x0600B36F RID: 45935 RVA: 0x0043A5F8 File Offset: 0x004387F8
		public void SetFabricatedFoodSymbol(Tag material)
		{
			this.foodKBAC.gameObject.SetActive(true);
			GameObject prefab = Assets.GetPrefab(material);
			this.foodKBAC.SwapAnims(prefab.GetComponent<KBatchedAnimController>().AnimFiles);
			this.foodKBAC.Play("object", KAnim.PlayMode.Loop, 1f, 0f);
		}

		// Token: 0x0600B370 RID: 45936 RVA: 0x0043A654 File Offset: 0x00438854
		protected override void OnSpawn()
		{
			base.OnSpawn();
			Storage[] components = base.GetComponents<Storage>();
			global::Debug.Assert(components.Length == 2);
			this.packages = components[0];
			this.water = components[1];
			this.packagesMeter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Vector3.zero, new string[]
			{
				"meter_target"
			});
			base.Subscribe(-1697596308, new Action<object>(this.StorageChangeHandler));
			this.SetupFoodSymbol();
			this.packagesMeter.SetPositionPercent((float)this.packages.items.Count / 5f);
		}

		// Token: 0x0600B371 RID: 45937 RVA: 0x0043A6FC File Offset: 0x004388FC
		public void ConsumeResourcesForRehydration(GameObject package, GameObject food)
		{
			global::Debug.Assert(this.packages.items.Contains(package));
			this.packages.ConsumeIgnoringDisease(package);
			float num;
			SimUtil.DiseaseInfo diseaseInfo;
			float num2;
			this.water.ConsumeAndGetDisease(FoodRehydratorConfig.REHYDRATION_TAG, 1f, out num, out diseaseInfo, out num2);
			PrimaryElement component = food.GetComponent<PrimaryElement>();
			if (component != null)
			{
				component.AddDisease(diseaseInfo.idx, diseaseInfo.count, "rehydrating");
				component.SetMassTemperature(component.Mass, component.Temperature * 0.125f + num2 * 0.875f);
			}
		}

		// Token: 0x0600B372 RID: 45938 RVA: 0x001146AC File Offset: 0x001128AC
		private void StorageChangeHandler(object obj)
		{
			if (((GameObject)obj).GetComponent<DehydratedFoodPackage>() != null)
			{
				this.packagesMeter.SetPositionPercent((float)this.packages.items.Count / 5f);
			}
		}

		// Token: 0x0600B373 RID: 45939 RVA: 0x0043A790 File Offset: 0x00438990
		private void SetupFoodSymbol()
		{
			GameObject gameObject = Util.NewGameObject(base.gameObject, "food_symbol");
			gameObject.SetActive(false);
			KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
			bool flag;
			Vector3 position = component.GetSymbolTransform(DehydratedManager.HASH_FOOD, out flag).GetColumn(3);
			position.z = Grid.GetLayerZ(Grid.SceneLayer.BuildingUse);
			gameObject.transform.SetPosition(position);
			this.foodKBAC = gameObject.AddComponent<KBatchedAnimController>();
			this.foodKBAC.AnimFiles = new KAnimFile[]
			{
				Assets.GetAnim("mushbar_kanim")
			};
			this.foodKBAC.initialAnim = "object";
			component.SetSymbolVisiblity(DehydratedManager.HASH_FOOD, false);
			this.foodKBAC.sceneLayer = Grid.SceneLayer.BuildingUse;
			KBatchedAnimTracker kbatchedAnimTracker = gameObject.AddComponent<KBatchedAnimTracker>();
			kbatchedAnimTracker.symbol = new HashedString("food");
			kbatchedAnimTracker.offset = Vector3.zero;
		}

		// Token: 0x0600B374 RID: 45940 RVA: 0x0043A878 File Offset: 0x00438A78
		public FewOptionSideScreen.IFewOptionSideScreen.Option[] GetOptions()
		{
			HashSet<Tag> discoveredResourcesFromTag = DiscoveredResources.Instance.GetDiscoveredResourcesFromTag(GameTags.Dehydrated);
			FewOptionSideScreen.IFewOptionSideScreen.Option[] array = new FewOptionSideScreen.IFewOptionSideScreen.Option[1 + discoveredResourcesFromTag.Count];
			array[0] = new FewOptionSideScreen.IFewOptionSideScreen.Option(GameTags.Dehydrated, UI.UISIDESCREENS.FILTERSIDESCREEN.DRIEDFOOD, Def.GetUISprite("icon_category_food", "ui", false), "");
			int num = 1;
			foreach (Tag tag in discoveredResourcesFromTag)
			{
				array[num] = new FewOptionSideScreen.IFewOptionSideScreen.Option(tag, tag.ProperName(), Def.GetUISprite(tag, "ui", false), "");
				num++;
			}
			return array;
		}

		// Token: 0x0600B375 RID: 45941 RVA: 0x001146E3 File Offset: 0x001128E3
		public void OnOptionSelected(FewOptionSideScreen.IFewOptionSideScreen.Option option)
		{
			this.ChosenContent = option.tag;
		}

		// Token: 0x0600B376 RID: 45942 RVA: 0x001146A4 File Offset: 0x001128A4
		public Tag GetSelectedOption()
		{
			return this.chosenContent;
		}

		// Token: 0x0600B377 RID: 45943 RVA: 0x0043A944 File Offset: 0x00438B44
		protected void OnCopySettings(object data)
		{
			GameObject gameObject = data as GameObject;
			if (gameObject != null)
			{
				DehydratedManager component = gameObject.GetComponent<DehydratedManager>();
				if (component != null)
				{
					this.ChosenContent = component.ChosenContent;
				}
			}
		}

		// Token: 0x04008DC2 RID: 36290
		[MyCmpAdd]
		private CopyBuildingSettings copyBuildingSettings;

		// Token: 0x04008DC3 RID: 36291
		private Storage packages;

		// Token: 0x04008DC4 RID: 36292
		private Storage water;

		// Token: 0x04008DC5 RID: 36293
		private MeterController packagesMeter;

		// Token: 0x04008DC6 RID: 36294
		private static string HASH_FOOD = "food";

		// Token: 0x04008DC7 RID: 36295
		private KBatchedAnimController foodKBAC;

		// Token: 0x04008DC8 RID: 36296
		private static readonly EventSystem.IntraObjectHandler<DehydratedManager> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<DehydratedManager>(delegate(DehydratedManager component, object data)
		{
			component.OnCopySettings(data);
		});

		// Token: 0x04008DC9 RID: 36297
		[Serialize]
		private Tag chosenContent = GameTags.Dehydrated;
	}
}
