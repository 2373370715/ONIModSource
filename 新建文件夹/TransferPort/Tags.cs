namespace RsTransferPort;

public class Tags {
    // 定义无线电源相关的标签，用于标识和管理无线电源组件或功能
    public static readonly Tag WirelessPower = TagManager.Create(nameof(WirelessPower));
    
    // 定义传输管道相关的标签，用于标识和管理传输管道组件或功能
    public static readonly Tag TransferConduit = TagManager.Create(nameof(TransferConduit));
    
    // 定义无线逻辑相关的标签，用于标识和管理无线逻辑组件或功能
    public static readonly Tag WirelessLogic = TagManager.Create(nameof(WirelessLogic));
    
    // 定义传输弧光粒子相关的标签，用于标识和管理传输弧光粒子组件或功能
    public static readonly Tag TransferRadianParticles = TagManager.Create(nameof(TransferRadianParticles));
}