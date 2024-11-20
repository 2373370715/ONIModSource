using System;
using UnityEngine;

namespace RsTransferPort
{
    [CreateAssetMenu(fileName = "LineCenterAsset", menuName = "TransferPor/Line Center Asset",order = 1)]
    public class LineCenterAsset : ScriptableObject
    {
        public Sprite gas;
        public Sprite liquid;
        public Sprite solid;
        public Sprite power;
        public Sprite logic;
        public Sprite rp;
        
    }
}