using System.Runtime.Serialization;
using KSerialization;

[SerializationConfig(MemberSerialization.OptIn)]
public class ResourceRef<ResourceType> : ISaveLoadable where ResourceType : Resource
{
	[Serialize]
	private ResourceGuid guid;

	private ResourceType resource;

	public ResourceGuid Guid => guid;

	public ResourceRef(ResourceType resource)
	{
		Set(resource);
	}

	public ResourceRef()
	{
	}

	public ResourceType Get()
	{
		return resource;
	}

	public void Set(ResourceType resource)
	{
		guid = null;
		this.resource = resource;
	}

	[OnSerializing]
	private void OnSerializing()
	{
		if (resource == null)
		{
			guid = null;
		}
		else
		{
			guid = resource.Guid;
		}
	}

	[OnDeserialized]
	private void OnDeserialized()
	{
		if (guid != null)
		{
			resource = Db.Get().GetResource<ResourceType>(guid);
			if (resource != null)
			{
				guid = null;
			}
		}
	}
}
