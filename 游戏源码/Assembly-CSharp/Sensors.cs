using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200082D RID: 2093
[AddComponentMenu("KMonoBehaviour/scripts/Sensors")]
public class Sensors : KMonoBehaviour
{
	// Token: 0x06002572 RID: 9586 RVA: 0x000B887A File Offset: 0x000B6A7A
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.GetComponent<Brain>().onPreUpdate += this.OnBrainPreUpdate;
	}

	// Token: 0x06002573 RID: 9587 RVA: 0x001CC3A0 File Offset: 0x001CA5A0
	public SensorType GetSensor<SensorType>() where SensorType : Sensor
	{
		foreach (Sensor sensor in this.sensors)
		{
			if (typeof(SensorType).IsAssignableFrom(sensor.GetType()))
			{
				return (SensorType)((object)sensor);
			}
		}
		global::Debug.LogError("Missing sensor of type: " + typeof(SensorType).Name);
		return default(SensorType);
	}

	// Token: 0x06002574 RID: 9588 RVA: 0x000B8899 File Offset: 0x000B6A99
	public void Add(Sensor sensor)
	{
		this.sensors.Add(sensor);
		if (sensor.IsEnabled)
		{
			sensor.Update();
		}
	}

	// Token: 0x06002575 RID: 9589 RVA: 0x001CC438 File Offset: 0x001CA638
	public void UpdateSensors()
	{
		foreach (Sensor sensor in this.sensors)
		{
			if (sensor.IsEnabled)
			{
				sensor.Update();
			}
		}
	}

	// Token: 0x06002576 RID: 9590 RVA: 0x000B88B5 File Offset: 0x000B6AB5
	private void OnBrainPreUpdate()
	{
		this.UpdateSensors();
	}

	// Token: 0x06002577 RID: 9591 RVA: 0x001CC494 File Offset: 0x001CA694
	public void ShowEditor()
	{
		foreach (Sensor sensor in this.sensors)
		{
			sensor.ShowEditor();
		}
	}

	// Token: 0x0400194B RID: 6475
	public List<Sensor> sensors = new List<Sensor>();
}
