using System.Collections.Generic;
using RsTransferPort;
using TUNING;
using UnityEngine;



public class RadiantParticlesTransferSenderConfig : RadiantParticlesTransferConfig {
    public const  string ID   = "RsRadiantParticlesTransferSender";
    private const string Anim = "rs_radiant_particles_transfer_sender_kanim";
    public RadiantParticlesTransferSenderConfig() : base(ID, Anim, InOutType.Sender) { }
}

public class RadiantParticlesTransferReceiverConfig : RadiantParticlesTransferConfig {
    public const  string ID   = "RsRadiantParticlesTransferReceiver";
    private const string Anim = "rs_radiant_particles_transfer_receiver_kanim";
    public RadiantParticlesTransferReceiverConfig() : base(ID, Anim, InOutType.Receiver) { }
}

/// <summary>
///     辐射粒子
/// </summary>
public abstract class RadiantParticlesTransferConfig : IBuildingConfig {
    protected string    anim;
    protected string    id;
    protected InOutType inOutType;

    public RadiantParticlesTransferConfig(string id, string anim, InOutType inOutType) {
        this.id        = id;
        this.anim      = anim;
        this.inOutType = inOutType;
    }

    public override string[] GetDlcIds() { return DlcManager.AVAILABLE_EXPANSION1_ONLY; }

    public override BuildingDef CreateBuildingDef() {
        var buildingDef
            = MyUtils.CreateTransferBuildingDef(id, anim, BUILDINGS.CONSTRUCTION_MASS_KG.TIER6, MATERIALS.RAW_MINERALS);

        buildingDef.DefaultAnimState = "off";

        if (inOutType == InOutType.Sender) {
            buildingDef.UseHighEnergyParticleInputPort = true;
            buildingDef.HighEnergyParticleInputOffset  = new CellOffset(0, 0);
            buildingDef.LogicOutputPorts = new List<LogicPorts.Port> {
                LogicPorts.Port.OutputPort(RadiantParticlesTransferSender.PORT_ID,
                                           new CellOffset(0, 0),
                                           MYSTRINGS.BUILDINGS.PREFABS.RSRADIANTPARTICLESTRANSFERSENDER.LOGIC_PORT,
                                           MYSTRINGS.BUILDINGS.PREFABS.RSRADIANTPARTICLESTRANSFERSENDER.LOGIC_PORT_ACTIVE,
                                           MYSTRINGS.BUILDINGS.PREFABS.RSRADIANTPARTICLESTRANSFERSENDER
                                                    .LOGIC_PORT_INACTIVE)
            };
        } else {
            buildingDef.UseHighEnergyParticleOutputPort = true;
            buildingDef.HighEnergyParticleOutputOffset  = new CellOffset(0, 0);
            buildingDef.LogicInputPorts = new List<LogicPorts.Port> {
                LogicPorts.Port.InputPort(LogicOperationalController.PORT_ID,
                                          new CellOffset(0, 0),
                                          MYSTRINGS.BUILDINGS.PREFABS.RSRADIANTPARTICLESTRANSFERRECEIVER.LOGIC_PORT,
                                          MYSTRINGS.BUILDINGS.PREFABS.RSRADIANTPARTICLESTRANSFERRECEIVER
                                                   .LOGIC_PORT_ACTIVE,
                                          MYSTRINGS.BUILDINGS.PREFABS.RSRADIANTPARTICLESTRANSFERRECEIVER
                                                   .LOGIC_PORT_INACTIVE)
            };
        }

        GeneratedBuildings.RegisterWithOverlay(OverlayScreen.RadiationIDs, id);
        buildingDef.Deprecated = !Sim.IsRadiationEnabled();
        return buildingDef;
    }

    public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag) {
        go.AddOrGet<ShowOverlaySelf>();
        go.AddOrGet<CopyBuildingSettings>().copyGroupTag = Tags.TransferRadianParticles;

        var energyParticleStorage = go.AddOrGet<HighEnergyParticleStorage>();
        energyParticleStorage.autoStore = true;
        energyParticleStorage.showInUI  = true;
        energyParticleStorage.capacity  = 501f;

        if (inOutType == InOutType.Sender) {
            go.AddOrGet<RadiantParticlesTransferSender>();
            go.AddOrGet<Operational>().SetFlag(RadiantParticlesTransferSender.receiverFlag, false);
        } else {
            go.AddOrGet<LogicOperationalController>();
            go.AddOrGet<RadiantParticlesTransferReceiver>();
            go.AddOrGet<Operational>().SetFlag(LogicOperationalController.LogicOperationalFlag, false);
        }

        var channel = go.AddOrGet<TransferPortChannel>();
        channel.BuildingType = BuildingType.HEP;
        channel.InOutType    = inOutType;
    }

    public override void DoPostConfigureComplete(GameObject go) {
        Object.DestroyImmediate(go.GetComponent<BuildingEnabledButton>());
    }
}