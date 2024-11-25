namespace RsTransferPort;

public class TransferConduitChannel : SingleChannelController {
    public PriorityChannelItemList receiverPriorityList = new();
    public PriorityChannelItemList senderPriorityList   = new();

    public TransferConduitChannel(BuildingType buildingType, string channelName, int worldIdAG) :
        base(buildingType, channelName, worldIdAG) { }

    //方法临时变量

    // private int tempSenderIndex = 0;
    // private int tempReceiverIndex = 0;
    // private int receiverIndexCount = 0;

    protected override void OnAdd(TransferPortChannel item) {
        if (item.InOutType == InOutType.Sender)
            senderPriorityList.AddChannelItem(item);
        else
            receiverPriorityList.AddChannelItem(item);
    }

    protected override void OnRemove(TransferPortChannel item) {
        if (item.InOutType == InOutType.Sender)
            senderPriorityList.RemoveChannelItem(item);
        else
            receiverPriorityList.RemoveChannelItem(item);
    }

    public IConduitFlow GetConduitManager() {
        switch (BuildingType) {
            case BuildingType.Gas:
                return Game.Instance.gasConduitFlow;
            case BuildingType.Liquid:
                return Game.Instance.liquidConduitFlow;
            case BuildingType.Solid:
                return Game.Instance.solidConduitFlow;
            default:
                return null;
        }
    }

    public void ConduitUpdate(float dt) {
        if (IsInvalid()) return;

        if (senders.Count == 0 || receivers.Count == 0) return;

        ConduitUpdate1();
    }

    private void ConduitUpdate1() {
        var cpReceiverEachCount = 0; //循环次数计算
        var rpIndex             = 0; //接收端的优先级信息的索引
        var senderPriorityIndex = 0;

        //设置一次只能传送一种液体
        for (; senderPriorityIndex < senderPriorityList.Count; senderPriorityIndex++) {
            // Debug.Log("prioritySenderInfo start===============");
            var prioritySenderInfo = senderPriorityList[senderPriorityIndex];

            // Debug.Log("prioritySenderInfo no null");
            //纠正值
            // prioritySenderInfo.PollIndexRedress();
            for (var i = 0; i < prioritySenderInfo.items.Count; i++) {
                //获取当前权重值的循环索引
                // Debug.Log("prioritySenderInfo.GetItemByPollIndex start===============");
                prioritySenderInfo.PollIndexRedress();
                var sender = prioritySenderInfo.GetItemByPollIndex().GetComponent<TransferConduit>();

                // Debug.Log("prioritySenderInfo.GetItemByPollIndex no null");
                var inputCell = sender.GetCell();
                if (IsConduitEmpty(inputCell)) {
                    prioritySenderInfo.PollIndexUp();
                    continue;
                }

                if (!ConduitUpdate2(inputCell, ref cpReceiverEachCount, senderPriorityIndex, ref rpIndex))

                    //找不到就退出去
                    return;
            }
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="inputCell"></param>
    /// <param name="cpReceiverEachCount"></param>
    /// <param name="senderPriorityIndex"></param>
    /// <param name="rpIndex"></param>
    /// <returns>有出口接收传送了</returns>
    private bool ConduitUpdate2(int inputCell, ref int cpReceiverEachCount, int senderPriorityIndex, ref int rpIndex) {
        for (; rpIndex < receiverPriorityList.Count; rpIndex++) {
            var priorityReceiverInfo = receiverPriorityList[rpIndex];
            while (cpReceiverEachCount <= priorityReceiverInfo.items.Count) {
                cpReceiverEachCount++;
                priorityReceiverInfo.PollIndexRedress();
                var outputCell = priorityReceiverInfo.GetItemByPollIndex().GetComponent<TransferConduit>().GetCell();
                priorityReceiverInfo.PollIndexUp();
                if (ConduitTransfer(inputCell, outputCell)) {
                    senderPriorityList[senderPriorityIndex].PollIndexUp();
                    return true;
                }
            }

            cpReceiverEachCount = 0;
        }

        return false;
    }

    /// <summary>
    /// </summary>
    /// <param name="inputCell"></param>
    /// <param name="outputCell"></param>
    /// <returns>是否传送了</returns>
    private bool ConduitTransfer(int inputCell, int outputCell) {
        if (BuildingType == BuildingType.Solid) {
            var flow = (SolidConduitFlow)GetConduitManager();
            if (flow.HasConduit(outputCell) && flow.IsConduitEmpty(outputCell)) {
                var pickupable = flow.RemovePickupable(inputCell);
                if (pickupable) flow.AddPickupable(outputCell, pickupable);
                return true; //直接返回
            }
        } else {
            var flow = (ConduitFlow)GetConduitManager();
            if (flow.HasConduit(outputCell) && flow.IsConduitEmpty(outputCell)) {
                var inputContents = flow.GetContents(inputCell);
                var useMass = flow.AddElement(outputCell,
                                              inputContents.element,
                                              inputContents.mass,
                                              inputContents.temperature,
                                              inputContents.diseaseIdx,
                                              inputContents.diseaseCount);

                flow.RemoveElement(inputCell, useMass);
                return true; //直接返回
            }
        }

        return false;
    }

    /// <summary>
    ///     输入端管道判断
    /// </summary>
    /// <param name="cell"></param>
    /// <returns></returns>
    private bool IsConduitEmpty(int cell) {
        if (BuildingType == BuildingType.Solid) {
            var flow = (SolidConduitFlow)GetConduitManager();
            return !flow.HasConduit(cell) || flow.IsConduitEmpty(cell);
        } else {
            var flow = (ConduitFlow)GetConduitManager();
            return !flow.HasConduit(cell) || flow.IsConduitEmpty(cell);
        }
    }

    public void OnCleanup() { }
}