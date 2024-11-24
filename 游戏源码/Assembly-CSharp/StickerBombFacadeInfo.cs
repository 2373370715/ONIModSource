using System;
using Database;

// Token: 0x02000971 RID: 2417
public class StickerBombFacadeInfo : IBlueprintInfo, IBlueprintDlcInfo
{
	// Token: 0x1700017F RID: 383
	// (get) Token: 0x06002BB4 RID: 11188 RVA: 0x000BC5EB File Offset: 0x000BA7EB
	// (set) Token: 0x06002BB5 RID: 11189 RVA: 0x000BC5F3 File Offset: 0x000BA7F3
	public string id { get; set; }

	// Token: 0x17000180 RID: 384
	// (get) Token: 0x06002BB6 RID: 11190 RVA: 0x000BC5FC File Offset: 0x000BA7FC
	// (set) Token: 0x06002BB7 RID: 11191 RVA: 0x000BC604 File Offset: 0x000BA804
	public string name { get; set; }

	// Token: 0x17000181 RID: 385
	// (get) Token: 0x06002BB8 RID: 11192 RVA: 0x000BC60D File Offset: 0x000BA80D
	// (set) Token: 0x06002BB9 RID: 11193 RVA: 0x000BC615 File Offset: 0x000BA815
	public string desc { get; set; }

	// Token: 0x17000182 RID: 386
	// (get) Token: 0x06002BBA RID: 11194 RVA: 0x000BC61E File Offset: 0x000BA81E
	// (set) Token: 0x06002BBB RID: 11195 RVA: 0x000BC626 File Offset: 0x000BA826
	public PermitRarity rarity { get; set; }

	// Token: 0x17000183 RID: 387
	// (get) Token: 0x06002BBC RID: 11196 RVA: 0x000BC62F File Offset: 0x000BA82F
	// (set) Token: 0x06002BBD RID: 11197 RVA: 0x000BC637 File Offset: 0x000BA837
	public string animFile { get; set; }

	// Token: 0x17000184 RID: 388
	// (get) Token: 0x06002BBE RID: 11198 RVA: 0x000BC640 File Offset: 0x000BA840
	// (set) Token: 0x06002BBF RID: 11199 RVA: 0x000BC648 File Offset: 0x000BA848
	public string[] dlcIds { get; set; }

	// Token: 0x06002BC0 RID: 11200 RVA: 0x000BC651 File Offset: 0x000BA851
	public StickerBombFacadeInfo(string id, string name, string desc, PermitRarity rarity, string animFile, string sticker)
	{
		this.id = id;
		this.name = name;
		this.desc = desc;
		this.rarity = rarity;
		this.animFile = animFile;
		this.sticker = sticker;
		this.dlcIds = DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x04001D62 RID: 7522
	public string sticker;
}
