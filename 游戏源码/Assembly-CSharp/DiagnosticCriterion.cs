using System;

// Token: 0x02001225 RID: 4645
public class DiagnosticCriterion
{
	// Token: 0x170005BF RID: 1471
	// (get) Token: 0x06005F2C RID: 24364 RVA: 0x000DE22E File Offset: 0x000DC42E
	// (set) Token: 0x06005F2D RID: 24365 RVA: 0x000DE236 File Offset: 0x000DC436
	public string id { get; private set; }

	// Token: 0x170005C0 RID: 1472
	// (get) Token: 0x06005F2E RID: 24366 RVA: 0x000DE23F File Offset: 0x000DC43F
	// (set) Token: 0x06005F2F RID: 24367 RVA: 0x000DE247 File Offset: 0x000DC447
	public string name { get; private set; }

	// Token: 0x06005F30 RID: 24368 RVA: 0x000DE250 File Offset: 0x000DC450
	public DiagnosticCriterion(string name, Func<ColonyDiagnostic.DiagnosticResult> action)
	{
		this.name = name;
		this.evaluateAction = action;
	}

	// Token: 0x06005F31 RID: 24369 RVA: 0x000DE266 File Offset: 0x000DC466
	public void SetID(string id)
	{
		this.id = id;
	}

	// Token: 0x06005F32 RID: 24370 RVA: 0x000DE26F File Offset: 0x000DC46F
	public ColonyDiagnostic.DiagnosticResult Evaluate()
	{
		return this.evaluateAction();
	}

	// Token: 0x040043B2 RID: 17330
	private Func<ColonyDiagnostic.DiagnosticResult> evaluateAction;
}
