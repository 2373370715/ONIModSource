using RsLib.Components;
using UnityEngine;

namespace RsTransferPort.Assets;

[CreateAssetMenu(fileName = "BodyAsset", menuName = "TransferPor/BodyAssett", order = 1)]
public class BodyAsset : ScriptableObject {
    public Sprite                globalConnectivityIcon;
    public LineArrow             lineArrow;
    public LineCenterImage       lineCenterImage;
    public Sprite                planetaryIsolationIcon;
    public RsHierarchyReferences portChannelName;

    [Header("Prefabs")]
    public PortChannelSideScreen portChannelSideScreen;

    public Sprite portOverlayButton;
    public Sprite showOverlayButton;

    [Header("Sprites")]
    public Sprite unconnectedChannelIcon;
}