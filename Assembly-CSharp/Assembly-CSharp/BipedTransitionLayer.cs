using Klei.AI;
using TUNING;
using UnityEngine;

public class BipedTransitionLayer : TransitionDriver.OverrideLayer {
    private const    float                      downPoleSpeed       = 15f;
    private const    float                      WATER_SPEED_PENALTY = 0.5f;
    private readonly AttributeLevels            attributeLevels;
    private readonly float                      floorSpeed;
    private          bool                       isWalking;
    private readonly float                      jetPackSpeed;
    private readonly float                      ladderSpeed;
    private readonly AttributeConverterInstance movementSpeed;
    private          float                      startTime;

    public BipedTransitionLayer(Navigator navigator, float floor_speed, float ladder_speed) : base(navigator) {
        navigator.Subscribe(1773898642, delegate { isWalking = true; });
        navigator.Subscribe(1597112836, delegate { isWalking = false; });
        floorSpeed      = floor_speed;
        ladderSpeed     = ladder_speed;
        jetPackSpeed    = floor_speed;
        movementSpeed   = Db.Get().AttributeConverters.MovementSpeed.Lookup(navigator.gameObject);
        attributeLevels = navigator.GetComponent<AttributeLevels>();
    }

    public override void BeginTransition(Navigator navigator, Navigator.ActiveTransition transition) {
        base.BeginTransition(navigator, transition);
        var num = 1f;
        var flag = (transition.start == NavType.Pole || transition.end == NavType.Pole) &&
                   transition.y < 0                                                     &&
                   transition.x == 0;

        var flag2 = transition.start == NavType.Tube  || transition.end == NavType.Tube;
        var flag3 = transition.start == NavType.Hover || transition.end == NavType.Hover;
        if (!flag && !flag2 && !flag3) {
            if (isWalking) return;

            num = GetMovementSpeedMultiplier(navigator);
        }

        var cell = Grid.PosToCell(navigator);
        var num2 = 1f;
        var flag4 = (navigator.flags & PathFinder.PotentialPath.Flags.HasAtmoSuit) >
                    PathFinder.PotentialPath.Flags.None;

        var flag5 = (navigator.flags & PathFinder.PotentialPath.Flags.HasJetPack) > PathFinder.PotentialPath.Flags.None;
        var flag6 = (navigator.flags & PathFinder.PotentialPath.Flags.HasLeadSuit) >
                    PathFinder.PotentialPath.Flags.None;

        if (!flag5 && !flag4 && !flag6 && Grid.IsSubstantialLiquid(cell)) num2 = 0.5f;
        num *= num2;
        if (transition.x == 0                                                        &&
            (transition.start == NavType.Ladder || transition.start == NavType.Pole) &&
            transition.start == transition.end) {
            if (flag)
                transition.speed = 15f * num2;
            else {
                transition.speed = ladderSpeed * num;
                var gameObject = Grid.Objects[cell, 1];
                if (gameObject != null) {
                    var component = gameObject.GetComponent<Ladder>();
                    if (component != null) {
                        var num3                   = component.upwardsMovementSpeedMultiplier;
                        if (transition.y < 0) num3 = component.downwardsMovementSpeedMultiplier;
                        transition.speed     *= num3;
                        transition.animSpeed *= num3;
                    }
                }
            }
        } else if (flag2)
            transition.speed = GetTubeTravellingSpeedMultiplier(navigator);
        else if (flag3)
            transition.speed = jetPackSpeed;
        else
            transition.speed = floorSpeed * num;

        var num4 = num - 1f;
        transition.animSpeed += transition.animSpeed * num4 / 2f;
        if (transition.start == NavType.Floor && transition.end == NavType.Floor) {
            var num5 = Grid.CellBelow(cell);
            if (Grid.Foundation[num5]) {
                var gameObject2 = Grid.Objects[num5, 1];
                if (gameObject2 != null) {
                    var component2 = gameObject2.GetComponent<SimCellOccupier>();
                    if (component2 != null) {
                        transition.speed     *= component2.movementSpeedMultiplier;
                        transition.animSpeed *= component2.movementSpeedMultiplier;
                    }
                }
            }
        }

        startTime = Time.time;
    }

    public override void EndTransition(Navigator navigator, Navigator.ActiveTransition transition) {
        base.EndTransition(navigator, transition);
        var flag = (transition.start == NavType.Pole || transition.end == NavType.Pole) &&
                   transition.y < 0                                                     &&
                   transition.x == 0;

        var flag2 = transition.start == NavType.Tube || transition.end == NavType.Tube;
        if (!isWalking && !flag && !flag2 && attributeLevels != null)
            attributeLevels.AddExperience(Db.Get().Attributes.Athletics.Id,
                                          Time.time - startTime,
                                          DUPLICANTSTATS.ATTRIBUTE_LEVELING.ALL_DAY_EXPERIENCE);
    }

    public float GetTubeTravellingSpeedMultiplier(Navigator navigator) {
        var attributeInstance = Db.Get().Attributes.TransitTubeTravelSpeed.Lookup(navigator.gameObject);
        if (attributeInstance != null) return attributeInstance.GetTotalValue();

        return DUPLICANTSTATS.STANDARD.BaseStats.TRANSIT_TUBE_TRAVEL_SPEED;
    }

    public float GetMovementSpeedMultiplier(Navigator navigator) {
        var num                        = 1f;
        if (movementSpeed != null) num += movementSpeed.Evaluate();
        return Mathf.Max(0.1f, num);
    }
}