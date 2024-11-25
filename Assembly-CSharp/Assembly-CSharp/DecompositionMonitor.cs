using Klei.AI;
using STRINGS;
using UnityEngine;

public class DecompositionMonitor : GameStateMachine<DecompositionMonitor, DecompositionMonitor.Instance> {
    public FloatParameter decomposition;

    [SerializeField]
    public int remainingRotMonsters = 3;

    public RottenState rotten;
    public State       satisfied;

    public override void InitializeStates(out BaseState default_state) {
        default_state = satisfied;
        serializable  = SerializeType.Both_DEPRECATED;
        satisfied.Update("UpdateDecomposition", delegate(Instance smi, float dt) { smi.UpdateDecomposition(dt); })
                 .ParamTransition(decomposition, rotten, IsGTEOne)
                 .ToggleAttributeModifier("Dead", smi => smi.satisfiedDecorModifier)
                 .ToggleAttributeModifier("Dead", smi => smi.satisfiedDecorRadiusModifier);

        rotten.DefaultState(rotten.exposed)
              .ToggleStatusItem(Db.Get().DuplicantStatusItems.Rotten, null)
              .ToggleAttributeModifier("Rotten", smi => smi.rottenDecorModifier)
              .ToggleAttributeModifier("Rotten", smi => smi.rottenDecorRadiusModifier);

        rotten.exposed.DefaultState(rotten.exposed.openair)
              .EventTransition(GameHashes.OnStore, rotten.stored, smi => !smi.IsExposed());

        rotten.exposed.openair.Enter(delegate(Instance smi) {
                                         if (smi.spawnsRotMonsters)
                                             smi.ScheduleGoTo(Random.Range(150f, 300f), rotten.spawningmonster);
                                     })
              .Transition(rotten.exposed.submerged, smi => smi.IsSubmerged())
              .ToggleFX(smi => CreateFX(smi));

        rotten.exposed.submerged.DefaultState(rotten.exposed.submerged.idle)
              .Transition(rotten.exposed.openair, smi => !smi.IsSubmerged());

        rotten.exposed.submerged.idle.ScheduleGoTo(0.25f, rotten.exposed.submerged.dirtywater);
        rotten.exposed.submerged.dirtywater
              .Enter("DirtyWater", delegate(Instance smi) { smi.DirtyWater(smi.dirtyWaterMaxRange); })
              .GoTo(rotten.exposed.submerged.idle);

        rotten.spawningmonster.Enter(delegate(Instance smi) {
                                         if (remainingRotMonsters > 0) {
                                             remainingRotMonsters--;
                                             GameUtil.KInstantiate(Assets.GetPrefab(new Tag("Glom")),
                                                                   smi.transform.GetPosition(),
                                                                   Grid.SceneLayer.Creatures)
                                                     .SetActive(true);
                                         }

                                         smi.GoTo(rotten.exposed);
                                     });

        rotten.stored.EventTransition(GameHashes.OnStore, rotten.exposed, smi => smi.IsExposed());
    }

    private FliesFX.Instance CreateFX(Instance smi) {
        if (!smi.isMasterNull) return new FliesFX.Instance(smi.master, new Vector3(0f, 0f, -0.1f));

        return null;
    }

    public class SubmergedState : State {
        public State dirtywater;
        public State idle;
    }

    public class ExposedState : State {
        public State          openair;
        public SubmergedState submerged;
    }

    public class RottenState : State {
        public ExposedState exposed;
        public State        spawningmonster;
        public State        stored;
    }

    public new class Instance : GameInstance {
        public float   decompositionRate;
        public int     dirtyWaterMaxRange = 3;
        public Disease disease;

        public AttributeModifier rottenDecorModifier
            = new AttributeModifier(Db.Get().BuildingAttributes.Decor.Id, -100f, DUPLICANTS.MODIFIERS.ROTTING.NAME);

        public AttributeModifier rottenDecorRadiusModifier
            = new AttributeModifier(Db.Get().BuildingAttributes.DecorRadius.Id, 4f, DUPLICANTS.MODIFIERS.ROTTING.NAME);

        public AttributeModifier satisfiedDecorModifier
            = new AttributeModifier(Db.Get().BuildingAttributes.Decor.Id, -65f, DUPLICANTS.MODIFIERS.DEAD.NAME);

        public AttributeModifier satisfiedDecorRadiusModifier
            = new AttributeModifier(Db.Get().BuildingAttributes.DecorRadius.Id, 4f, DUPLICANTS.MODIFIERS.DEAD.NAME);

        public bool spawnsRotMonsters = true;

        public Instance(IStateMachineTarget master,
                        Disease             disease,
                        float               decompositionRate = 0.00083333335f,
                        bool                spawnRotMonsters  = true) : base(master) {
            gameObject.AddComponent<DecorProvider>();
            this.decompositionRate = decompositionRate;
            this.disease           = disease;
            spawnsRotMonsters      = spawnRotMonsters;
        }

        public void UpdateDecomposition(float dt) {
            var delta_value = dt * decompositionRate;
            sm.decomposition.Delta(delta_value, smi);
        }

        public bool IsExposed() {
            var component = smi.GetComponent<KPrefabID>();
            return component == null || !component.HasTag(GameTags.Preserved);
        }

        public bool IsRotten()    { return IsInsideState(sm.rotten); }
        public bool IsSubmerged() { return PathFinder.IsSubmerged(Grid.PosToCell(master.transform.GetPosition())); }

        public void DirtyWater(int maxCellRange = 3) {
            var num = Grid.PosToCell(master.transform.GetPosition());
            if (Grid.Element[num].id == SimHashes.Water) {
                SimMessages.ReplaceElement(num,
                                           SimHashes.DirtyWater,
                                           CellEventLogger.Instance.DecompositionDirtyWater,
                                           Grid.Mass[num],
                                           Grid.Temperature[num],
                                           Grid.DiseaseIdx[num],
                                           Grid.DiseaseCount[num]);

                return;
            }

            if (Grid.Element[num].id == SimHashes.DirtyWater) {
                var array = new int[4];
                for (var i = 0; i < maxCellRange; i++) {
                    for (var j = 0; j < maxCellRange; j++) {
                        array[0] = Grid.OffsetCell(num, new CellOffset(-i, j));
                        array[1] = Grid.OffsetCell(num, new CellOffset(i,  j));
                        array[2] = Grid.OffsetCell(num, new CellOffset(-i, -j));
                        array[3] = Grid.OffsetCell(num, new CellOffset(i,  -j));
                        array.Shuffle();
                        foreach (var num2 in array)
                            if (Grid.GetCellDistance(num, num2) < maxCellRange - 1 &&
                                Grid.IsValidCell(num2)                             &&
                                Grid.Element[num2].id == SimHashes.Water) {
                                SimMessages.ReplaceElement(num2,
                                                           SimHashes.DirtyWater,
                                                           CellEventLogger.Instance.DecompositionDirtyWater,
                                                           Grid.Mass[num2],
                                                           Grid.Temperature[num2],
                                                           Grid.DiseaseIdx[num2],
                                                           Grid.DiseaseCount[num2]);

                                return;
                            }
                    }
                }
            }
        }
    }
}