using System.Collections.Generic;
using Klei.AI;
using KSerialization;

public class AsteroidGridEntity : ClusterGridEntity {
    public static string DEFAULT_ASTEROID_ICON_ANIM = "asteroid_sandstone_start_kanim";

    [Serialize]
    private string m_asteroidAnim;

    [Serialize]
    private string m_name;

    [MyCmpReq]
    private WorldContainer m_worldContainer;

    public override string      Name  => m_name;
    public override EntityLayer Layer => EntityLayer.Asteroid;

    public override List<AnimConfig> AnimConfigs {
        get {
            var list = new List<AnimConfig>();
            var item = new AnimConfig {
                animFile = Assets.GetAnim(m_asteroidAnim.IsNullOrWhiteSpace()
                                              ? DEFAULT_ASTEROID_ICON_ANIM
                                              : m_asteroidAnim),
                initialAnim = "idle_loop"
            };

            list.Add(item);
            item = new AnimConfig { animFile = Assets.GetAnim("orbit_kanim"), initialAnim = "orbit" };
            list.Add(item);
            item = new AnimConfig {
                animFile    = Assets.GetAnim("shower_asteroid_current_kanim"),
                initialAnim = "off",
                playMode    = KAnim.PlayMode.Once
            };

            list.Add(item);
            return list;
        }
    }

    public override bool               IsVisible      => true;
    public override ClusterRevealLevel IsVisibleInFOW => ClusterRevealLevel.Peeked;
    public override bool               ShowName()     { return true; }

    public void Init(string name, AxialI location, string asteroidTypeId) {
        m_name         = name;
        m_location     = location;
        m_asteroidAnim = asteroidTypeId;
    }

    protected override void OnSpawn() {
        KAnimFile kanimFile;
        if (!Assets.TryGetAnim(m_asteroidAnim, out kanimFile)) m_asteroidAnim = DEFAULT_ASTEROID_ICON_ANIM;
        Game.Instance.Subscribe(-1298331547, OnClusterLocationChanged);
        Game.Instance.Subscribe(-1991583975, OnFogOfWarRevealed);
        Game.Instance.Subscribe(78366336,    OnMeteorShowerEventChanged);
        Game.Instance.Subscribe(1749562766,  OnMeteorShowerEventChanged);
        if (ClusterGrid.Instance.IsCellVisible(m_location))
            SaveGame.Instance.GetSMI<ClusterFogOfWarManager.Instance>().RevealLocation(m_location, 1);

        base.OnSpawn();
    }

    protected override void OnCleanUp() {
        Game.Instance.Unsubscribe(-1298331547, OnClusterLocationChanged);
        Game.Instance.Unsubscribe(-1991583975, OnFogOfWarRevealed);
        Game.Instance.Unsubscribe(78366336,    OnMeteorShowerEventChanged);
        Game.Instance.Unsubscribe(1749562766,  OnMeteorShowerEventChanged);
        base.OnCleanUp();
    }

    public void OnClusterLocationChanged(object data) {
        if (m_worldContainer.IsDiscovered) return;

        if (!ClusterGrid.Instance.IsCellVisible(Location)) return;

        var component = ((ClusterLocationChangedEvent)data).entity.GetComponent<Clustercraft>();
        if (component == null) return;

        if (component.GetOrbitAsteroid() == this) m_worldContainer.SetDiscovered(true);
    }

    public override void OnClusterMapIconShown(ClusterRevealLevel levelUsed) {
        base.OnClusterMapIconShown(levelUsed);
        if (levelUsed == ClusterRevealLevel.Visible) RefreshMeteorShowerEffect();
    }

    private void OnMeteorShowerEventChanged(object _worldID) {
        if ((int)_worldID == m_worldContainer.id) RefreshMeteorShowerEffect();
    }

    public void RefreshMeteorShowerEffect() {
        if (ClusterMapScreen.Instance == null) return;

        var entityVisAnim = ClusterMapScreen.Instance.GetEntityVisAnim(this);
        if (entityVisAnim == null) return;

        var animController = entityVisAnim.GetAnimController(2);
        if (animController != null) {
            var list = new List<GameplayEventInstance>();
            GameplayEventManager.Instance.GetActiveEventsOfType<MeteorShowerEvent>(m_worldContainer.id, ref list);
            var flag = false;
            var s    = "off";
            foreach (var gameplayEventInstance in list)
                if (gameplayEventInstance != null && gameplayEventInstance.smi is MeteorShowerEvent.StatesInstance) {
                    var statesInstance = gameplayEventInstance.smi as MeteorShowerEvent.StatesInstance;
                    if (statesInstance.IsInsideState(statesInstance.sm.running.bombarding)) {
                        flag = true;
                        s    = "idle_loop";
                        break;
                    }
                }

            animController.Play(s, flag ? KAnim.PlayMode.Loop : KAnim.PlayMode.Once);
        }
    }

    public void OnFogOfWarRevealed(object data = null) {
        if (data == null) return;

        if ((AxialI)data != m_location) return;

        if (!ClusterGrid.Instance.IsCellVisible(Location)) return;

        if (DlcManager.FeatureClusterSpaceEnabled()) {
            var message = new WorldDetectedMessage(m_worldContainer);
            MusicManager.instance.PlaySong("Stinger_WorldDetected");
            Messenger.Instance.QueueMessage(message);
            if (!m_worldContainer.IsDiscovered) {
                var enumerator = Components.Clustercrafts.GetEnumerator();
                while (enumerator.MoveNext())
                    if (((Clustercraft)enumerator.Current).GetOrbitAsteroid() == this) {
                        m_worldContainer.SetDiscovered(true);
                        break;
                    }
            }
        }
    }
}