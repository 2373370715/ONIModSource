using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RsLib.Adapter
{
    public class LocTextAdapter : TextMeshProUGUI
    {
        public string textStyleSettingName;
        
        // Token: 0x04003AD7 RID: 15063
        public string key;
    
        // Token: 0x04003AD9 RID: 15065
        public bool allowOverride;
    
        // Token: 0x04003ADA RID: 15066
        public bool staticLayout;
    
        // Token: 0x04003ADC RID: 15068
        private string originalString = string.Empty;
    
        // Token: 0x04003ADD RID: 15069
        [SerializeField]
        private bool allowLinksInternal;
        
    }
    
}
