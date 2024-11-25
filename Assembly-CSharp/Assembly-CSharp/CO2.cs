using System;
using KSerialization;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn), AddComponentMenu("KMonoBehaviour/scripts/CO2")]
public class CO2 : KMonoBehaviour {
    [Serialize, NonSerialized]
    public float lifetimeRemaining;

    [Serialize, NonSerialized]
    public float mass;

    [Serialize, NonSerialized]
    public float temperature;

    [Serialize, NonSerialized]
    public Vector3 velocity = Vector3.zero;

    public void StartLoop() {
        var component = GetComponent<KBatchedAnimController>();
        component.Play("exhale_pre");
        component.Play("exhale_loop", KAnim.PlayMode.Loop);
    }

    public void TriggerDestroy() { GetComponent<KBatchedAnimController>().Play("exhale_pst"); }
}