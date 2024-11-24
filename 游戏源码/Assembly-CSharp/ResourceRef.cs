using System;
using System.Runtime.Serialization;
using KSerialization;

// Token: 0x020017B5 RID: 6069
[SerializationConfig(MemberSerialization.OptIn)]
public class ResourceRef<ResourceType> : ISaveLoadable where ResourceType : Resource
{
	// Token: 0x06007D00 RID: 32000 RVA: 0x000F24D1 File Offset: 0x000F06D1
	public ResourceRef(ResourceType resource)
	{
		this.Set(resource);
	}

	// Token: 0x06007D01 RID: 32001 RVA: 0x000A5E2C File Offset: 0x000A402C
	public ResourceRef()
	{
	}

	// Token: 0x170007F2 RID: 2034
	// (get) Token: 0x06007D02 RID: 32002 RVA: 0x000F24E0 File Offset: 0x000F06E0
	public ResourceGuid Guid
	{
		get
		{
			return this.guid;
		}
	}

	// Token: 0x06007D03 RID: 32003 RVA: 0x000F24E8 File Offset: 0x000F06E8
	public ResourceType Get()
	{
		return this.resource;
	}

	// Token: 0x06007D04 RID: 32004 RVA: 0x000F24F0 File Offset: 0x000F06F0
	public void Set(ResourceType resource)
	{
		this.guid = null;
		this.resource = resource;
	}

	// Token: 0x06007D05 RID: 32005 RVA: 0x000F2500 File Offset: 0x000F0700
	[OnSerializing]
	private void OnSerializing()
	{
		if (this.resource == null)
		{
			this.guid = null;
			return;
		}
		this.guid = this.resource.Guid;
	}

	// Token: 0x06007D06 RID: 32006 RVA: 0x000F252D File Offset: 0x000F072D
	[OnDeserialized]
	private void OnDeserialized()
	{
		if (this.guid != null)
		{
			this.resource = Db.Get().GetResource<ResourceType>(this.guid);
			if (this.resource != null)
			{
				this.guid = null;
			}
		}
	}

	// Token: 0x04005E91 RID: 24209
	[Serialize]
	private ResourceGuid guid;

	// Token: 0x04005E92 RID: 24210
	private ResourceType resource;
}
