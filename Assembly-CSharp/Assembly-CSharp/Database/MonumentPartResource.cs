using System;
using UnityEngine;

namespace Database
{
		public class MonumentPartResource : PermitResource
	{
								public KAnimFile AnimFile { get; private set; }

								public string SymbolName { get; private set; }

								public string State { get; private set; }

				public MonumentPartResource(string id, string name, string desc, PermitRarity rarity, string animFilename, string state, string symbolName, MonumentPartResource.Part part, string[] dlcIds) : base(id, name, desc, PermitCategory.Artwork, rarity, dlcIds)
		{
			this.AnimFile = Assets.GetAnim(animFilename);
			this.SymbolName = symbolName;
			this.State = state;
			this.part = part;
		}

				public global::Tuple<Sprite, Color> GetUISprite()
		{
			Sprite sprite = Assets.GetSprite("unknown");
			return new global::Tuple<Sprite, Color>(sprite, (sprite != null) ? Color.white : Color.clear);
		}

				public override PermitPresentationInfo GetPermitPresentationInfo()
		{
			PermitPresentationInfo result = default(PermitPresentationInfo);
			result.sprite = this.GetUISprite().first;
			result.SetFacadeForText("_monument part");
			return result;
		}

				public MonumentPartResource.Part part;

				public enum Part
		{
						Bottom,
						Middle,
						Top
		}
	}
}
