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

    /// <summary>
    /// 重写BeginTransition方法以自定义角色在不同导航类型之间的过渡效果。
    /// </summary>
    /// <param name="navigator">执行过渡的导航器对象。</param>
    /// <param name="transition">当前的过渡状态。</param>
    public override void BeginTransition(Navigator navigator, Navigator.ActiveTransition transition) {
        // 调用基类的BeginTransition方法
        base.BeginTransition(navigator, transition);
    
        // 初始化速度调整系数
        var num = 1f;
    
        // 判断是否在使用杆子向下移动
        var flag = (transition.start == NavType.Pole || transition.end == NavType.Pole) &&
                   transition.y < 0                                                     &&
                   transition.x == 0;
    
        // 判断是否在使用管道移动
        var flag2 = transition.start == NavType.Tube  || transition.end == NavType.Tube;
    
        // 判断是否在使用悬浮移动
        var flag3 = transition.start == NavType.Hover || transition.end == NavType.Hover;
    
        // 如果不在特殊移动状态下，则根据角色是否在行走来调整速度
        if (!flag && !flag2 && !flag3) {
            if (isWalking) return;
    
            num = GetMovementSpeedMultiplier(navigator);
        }
    
        // 获取导航器所在的单元格
        var cell = Grid.PosToCell(navigator);
    
        // 初始化速度调整系数
        var num2 = 1f;
    
        // 判断导航器是否装备了大气套装
        var flag4 = (navigator.flags & PathFinder.PotentialPath.Flags.HasAtmoSuit) >
                    PathFinder.PotentialPath.Flags.None;
    
        // 判断导航器是否装备了喷气背包
        var flag5 = (navigator.flags & PathFinder.PotentialPath.Flags.HasJetPack) > PathFinder.PotentialPath.Flags.None;
    
        // 判断导航器是否装备了铅制套装
        var flag6 = (navigator.flags & PathFinder.PotentialPath.Flags.HasLeadSuit) >
                    PathFinder.PotentialPath.Flags.None;
    
        // 如果没有装备特定的套装且在实质性的液体中，则减慢速度
        if (!flag5 && !flag4 && !flag6 && Grid.IsSubstantialLiquid(cell)) num2 = 0.5f;
    
        // 结合两种速度调整系数
        num *= num2;
    
        // 根据不同的过渡类型来设置速度
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
    
        // 调整动画速度
        var num4 = num - 1f;
        transition.animSpeed += transition.animSpeed * num4 / 2f;
    
        // 如果在地板上移动，则根据下面的单元格状态来调整速度
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
    
        // 记录开始时间
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

        return 18f;
    }

    public float GetMovementSpeedMultiplier(Navigator navigator) {
        var num                        = 1f;
        if (movementSpeed != null) num += movementSpeed.Evaluate();

        return Mathf.Max(0.1f, num);
    }
}