using System;
using UnityEngine;

// Token: 0x0200082C RID: 2092
public class Sensor
{
	// Token: 0x17000117 RID: 279
	// (get) Token: 0x06002566 RID: 9574 RVA: 0x000B87CD File Offset: 0x000B69CD
	// (set) Token: 0x06002565 RID: 9573 RVA: 0x000B87C4 File Offset: 0x000B69C4
	public bool IsEnabled { get; private set; } = true;

	// Token: 0x17000118 RID: 280
	// (get) Token: 0x06002567 RID: 9575 RVA: 0x000B87D5 File Offset: 0x000B69D5
	// (set) Token: 0x06002568 RID: 9576 RVA: 0x000B87DD File Offset: 0x000B69DD
	public string Name { get; private set; }

	// Token: 0x06002569 RID: 9577 RVA: 0x000B87E6 File Offset: 0x000B69E6
	public Sensor(Sensors sensors, bool active)
	{
		this.sensors = sensors;
		this.SetActive(active);
		this.Name = base.GetType().Name;
	}

	// Token: 0x0600256A RID: 9578 RVA: 0x000B8814 File Offset: 0x000B6A14
	public Sensor(Sensors sensors)
	{
		this.sensors = sensors;
		this.Name = base.GetType().Name;
	}

	// Token: 0x0600256B RID: 9579 RVA: 0x000B883B File Offset: 0x000B6A3B
	public ComponentType GetComponent<ComponentType>()
	{
		return this.sensors.GetComponent<ComponentType>();
	}

	// Token: 0x17000119 RID: 281
	// (get) Token: 0x0600256C RID: 9580 RVA: 0x000B8848 File Offset: 0x000B6A48
	public GameObject gameObject
	{
		get
		{
			return this.sensors.gameObject;
		}
	}

	// Token: 0x1700011A RID: 282
	// (get) Token: 0x0600256D RID: 9581 RVA: 0x000B8855 File Offset: 0x000B6A55
	public Transform transform
	{
		get
		{
			return this.gameObject.transform;
		}
	}

	// Token: 0x0600256E RID: 9582 RVA: 0x000B8862 File Offset: 0x000B6A62
	public void SetActive(bool enabled)
	{
		this.IsEnabled = enabled;
	}

	// Token: 0x0600256F RID: 9583 RVA: 0x000B886B File Offset: 0x000B6A6B
	public void Trigger(int hash, object data = null)
	{
		this.sensors.Trigger(hash, data);
	}

	// Token: 0x06002570 RID: 9584 RVA: 0x000A5E40 File Offset: 0x000A4040
	public virtual void Update()
	{
	}

	// Token: 0x06002571 RID: 9585 RVA: 0x000A5E40 File Offset: 0x000A4040
	public virtual void ShowEditor()
	{
	}

	// Token: 0x04001949 RID: 6473
	protected Sensors sensors;
}
