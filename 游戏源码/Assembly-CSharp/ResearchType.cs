using System;
using UnityEngine;

// Token: 0x020017A7 RID: 6055
public class ResearchType
{
	// Token: 0x06007CA9 RID: 31913 RVA: 0x000F20F4 File Offset: 0x000F02F4
	public ResearchType(string id, string name, string description, Sprite sprite, Color color, Recipe.Ingredient[] fabricationIngredients, float fabricationTime, HashedString kAnim_ID, string[] fabricators, string recipeDescription)
	{
		this._id = id;
		this._name = name;
		this._description = description;
		this._sprite = sprite;
		this._color = color;
		this.CreatePrefab(fabricationIngredients, fabricationTime, kAnim_ID, fabricators, recipeDescription, color);
	}

	// Token: 0x06007CAA RID: 31914 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06007CAB RID: 31915 RVA: 0x003226C0 File Offset: 0x003208C0
	public GameObject CreatePrefab(Recipe.Ingredient[] fabricationIngredients, float fabricationTime, HashedString kAnim_ID, string[] fabricators, string recipeDescription, Color color)
	{
		GameObject gameObject = EntityTemplates.CreateBasicEntity(this.id, this.name, this.description, 1f, true, Assets.GetAnim(kAnim_ID), "ui", Grid.SceneLayer.BuildingFront, SimHashes.Creature, null, 293f);
		gameObject.AddOrGet<ResearchPointObject>().TypeID = this.id;
		this._recipe = new Recipe(this.id, 1f, (SimHashes)0, this.name, recipeDescription, 0);
		this._recipe.SetFabricators(fabricators, fabricationTime);
		this._recipe.SetIcon(Assets.GetSprite("research_type_icon"), color);
		if (fabricationIngredients != null)
		{
			foreach (Recipe.Ingredient ingredient in fabricationIngredients)
			{
				this._recipe.AddIngredient(ingredient);
			}
		}
		return gameObject;
	}

	// Token: 0x06007CAC RID: 31916 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06007CAD RID: 31917 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x170007E1 RID: 2017
	// (get) Token: 0x06007CAE RID: 31918 RVA: 0x000F2134 File Offset: 0x000F0334
	public string id
	{
		get
		{
			return this._id;
		}
	}

	// Token: 0x170007E2 RID: 2018
	// (get) Token: 0x06007CAF RID: 31919 RVA: 0x000F213C File Offset: 0x000F033C
	public string name
	{
		get
		{
			return this._name;
		}
	}

	// Token: 0x170007E3 RID: 2019
	// (get) Token: 0x06007CB0 RID: 31920 RVA: 0x000F2144 File Offset: 0x000F0344
	public string description
	{
		get
		{
			return this._description;
		}
	}

	// Token: 0x170007E4 RID: 2020
	// (get) Token: 0x06007CB1 RID: 31921 RVA: 0x000F214C File Offset: 0x000F034C
	public string recipe
	{
		get
		{
			return this.recipe;
		}
	}

	// Token: 0x170007E5 RID: 2021
	// (get) Token: 0x06007CB2 RID: 31922 RVA: 0x000F2154 File Offset: 0x000F0354
	public Color color
	{
		get
		{
			return this._color;
		}
	}

	// Token: 0x170007E6 RID: 2022
	// (get) Token: 0x06007CB3 RID: 31923 RVA: 0x000F215C File Offset: 0x000F035C
	public Sprite sprite
	{
		get
		{
			return this._sprite;
		}
	}

	// Token: 0x04005E57 RID: 24151
	private string _id;

	// Token: 0x04005E58 RID: 24152
	private string _name;

	// Token: 0x04005E59 RID: 24153
	private string _description;

	// Token: 0x04005E5A RID: 24154
	private Recipe _recipe;

	// Token: 0x04005E5B RID: 24155
	private Sprite _sprite;

	// Token: 0x04005E5C RID: 24156
	private Color _color;
}
