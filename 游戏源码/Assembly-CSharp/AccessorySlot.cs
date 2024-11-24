using System.Collections.Generic;

public class AccessorySlot : Resource
{
	private KAnimFile file;

	public KAnimHashedString targetSymbolId { get; private set; }

	public List<Accessory> accessories { get; private set; }

	public KAnimFile AnimFile => file;

	public KAnimFile defaultAnimFile { get; private set; }

	public int overrideLayer { get; private set; }

	public AccessorySlot(string id, ResourceSet parent, KAnimFile swap_build, int overrideLayer = 0)
		: base(id, parent)
	{
		if (swap_build == null)
		{
			Debug.LogErrorFormat("AccessorySlot {0} missing swap_build", id);
		}
		targetSymbolId = new KAnimHashedString("snapTo_" + id.ToLower());
		accessories = new List<Accessory>();
		file = swap_build;
		this.overrideLayer = overrideLayer;
		defaultAnimFile = swap_build;
	}

	public AccessorySlot(string id, ResourceSet parent, KAnimHashedString target_symbol_id, KAnimFile swap_build, KAnimFile defaultAnimFile = null, int overrideLayer = 0)
		: base(id, parent)
	{
		if (swap_build == null)
		{
			Debug.LogErrorFormat("AccessorySlot {0} missing swap_build", id);
		}
		targetSymbolId = target_symbol_id;
		accessories = new List<Accessory>();
		file = swap_build;
		this.defaultAnimFile = ((defaultAnimFile != null) ? defaultAnimFile : swap_build);
		this.overrideLayer = overrideLayer;
	}

	public void AddAccessories(KAnimFile default_build, ResourceSet parent)
	{
		KAnim.Build build = default_build.GetData().build;
		default_build.GetData().build.GetSymbol(targetSymbolId);
		string value = Id.ToLower();
		for (int i = 0; i < build.symbols.Length; i++)
		{
			string text = HashCache.Get().Get(build.symbols[i].hash);
			if (text.StartsWith(value))
			{
				Accessory accessory = new Accessory(text, parent, this, file.batchTag, build.symbols[i], default_build);
				accessories.Add(accessory);
				HashCache.Get().Add(accessory.IdHash.HashValue, accessory.Id);
			}
		}
	}

	public Accessory Lookup(string id)
	{
		return Lookup(new HashedString(id));
	}

	public Accessory Lookup(HashedString full_id)
	{
		if (!full_id.IsValid)
		{
			return null;
		}
		return accessories.Find((Accessory a) => a.IdHash == full_id);
	}
}
