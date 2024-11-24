using UnityEngine;

public class KleiItemDropScreen_PermitVis_DupeEquipment : KMonoBehaviour
{
	[SerializeField]
	private KBatchedAnimController droppedItemKAnim;

	[SerializeField]
	private KBatchedAnimController dupeKAnim;

	public void ConfigureWith(DropScreenPresentationInfo info)
	{
		dupeKAnim.GetComponent<UIDupeRandomizer>().Randomize();
		KAnimFile anim = Assets.GetAnim(info.BuildOverride);
		dupeKAnim.AddAnimOverrides(anim);
		KAnimHashedString kAnimHashedString = new KAnimHashedString("snapto_neck");
		KAnim.Build.Symbol symbol = anim.GetData().build.GetSymbol(kAnimHashedString);
		if (symbol != null)
		{
			dupeKAnim.GetComponent<SymbolOverrideController>().AddSymbolOverride(kAnimHashedString, symbol, 6);
			dupeKAnim.SetSymbolVisiblity(kAnimHashedString, is_visible: true);
		}
		else
		{
			dupeKAnim.GetComponent<SymbolOverrideController>().RemoveSymbolOverride(kAnimHashedString, 6);
			dupeKAnim.SetSymbolVisiblity(kAnimHashedString, is_visible: false);
		}
		dupeKAnim.Play("idle_default", KAnim.PlayMode.Loop);
		dupeKAnim.Queue("cheer_pre");
		dupeKAnim.Queue("cheer_loop");
		dupeKAnim.Queue("cheer_pst");
		dupeKAnim.Queue("idle_default", KAnim.PlayMode.Loop);
	}
}
