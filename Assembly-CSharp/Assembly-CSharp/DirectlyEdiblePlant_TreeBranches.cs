using Klei.AI;
using TUNING;
using UnityEngine;

public class DirectlyEdiblePlant_TreeBranches : KMonoBehaviour, IPlantConsumptionInstructions {
    public  float                      MinimumEdibleMaturity = 0.25f;
    public  string                     overrideCropID;
    private PlantBranchGrower.Instance trunk;
    public  bool                       CanPlantBeEaten() { return GetMaxBranchMaturity() >= MinimumEdibleMaturity; }

    public float ConsumePlant(float desiredUnitsToConsume) {
        var maxBranchMaturity = GetMaxBranchMaturity();
        var num               = Mathf.Min(desiredUnitsToConsume, maxBranchMaturity);
        var mostMatureBranch  = GetMostMatureBranch();
        if (!mostMatureBranch) return 0f;

        var component = mostMatureBranch.GetComponent<Growing>();
        if (component) {
            var component2 = mostMatureBranch.GetComponent<Harvestable>();
            if (component2 != null) component2.Trigger(2127324410, true);
            component.ConsumeMass(num);
            return num;
        }

        mostMatureBranch.GetAmounts().Get(Db.Get().Amounts.Maturity.Id).ApplyDelta(-desiredUnitsToConsume);
        gameObject.Trigger(-1793167409);
        mostMatureBranch.Trigger(-1793167409);
        return desiredUnitsToConsume;
    }

    public float PlantProductGrowthPerCycle() {
        var component                      = GetComponent<Crop>();
        var cropID                         = component.cropId;
        if (overrideCropID != null) cropID = overrideCropID;
        var num                            = CROPS.CROP_TYPES.Find(m => m.cropId == cropID).cropDuration / 600f;
        return 1f / num;
    }

    public string GetFormattedConsumptionPerCycle(float consumer_KGWorthOfCaloriesLostPerSecond) {
        var num = PlantProductGrowthPerCycle();
        return GameUtil.GetFormattedPlantGrowth(consumer_KGWorthOfCaloriesLostPerSecond * num * 100f,
                                                GameUtil.TimeSlice.PerCycle);
    }

    public CellOffset[]       GetAllowedOffsets() { return null; }
    public Diet.Info.FoodType GetDietFoodType()   { return Diet.Info.FoodType.EatPlantDirectly; }

    protected override void OnSpawn() {
        trunk = gameObject.GetSMI<PlantBranchGrower.Instance>();
        base.OnSpawn();
    }

    public float GetMaxBranchMaturity() {
        var        max_maturity = 0f;
        GameObject max_branch   = null;
        trunk.ActionPerBranch(delegate(GameObject branch) {
                                  if (branch != null) {
                                      var amountInstance = Db.Get().Amounts.Maturity.Lookup(branch);
                                      if (amountInstance != null) {
                                          var num = amountInstance.value / amountInstance.GetMax();
                                          if (num > max_maturity) {
                                              max_maturity = num;
                                              max_branch   = branch;
                                          }
                                      }
                                  }
                              });

        return max_maturity;
    }

    private GameObject GetMostMatureBranch() {
        var        max_maturity = 0f;
        GameObject max_branch   = null;
        trunk.ActionPerBranch(delegate(GameObject branch) {
                                  if (branch != null) {
                                      var amountInstance = Db.Get().Amounts.Maturity.Lookup(branch);
                                      if (amountInstance != null) {
                                          var num = amountInstance.value / amountInstance.GetMax();
                                          if (num > max_maturity) {
                                              max_maturity = num;
                                              max_branch   = branch;
                                          }
                                      }
                                  }
                              });

        return max_branch;
    }
}