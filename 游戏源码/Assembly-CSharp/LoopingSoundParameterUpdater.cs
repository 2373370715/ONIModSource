using System;
using FMOD.Studio;
using UnityEngine;

// Token: 0x02000A85 RID: 2693
public abstract class LoopingSoundParameterUpdater
{
	// Token: 0x17000201 RID: 513
	// (get) Token: 0x060031C4 RID: 12740 RVA: 0x000C0588 File Offset: 0x000BE788
	// (set) Token: 0x060031C5 RID: 12741 RVA: 0x000C0590 File Offset: 0x000BE790
	public HashedString parameter { get; private set; }

	// Token: 0x060031C6 RID: 12742 RVA: 0x000C0599 File Offset: 0x000BE799
	public LoopingSoundParameterUpdater(HashedString parameter)
	{
		this.parameter = parameter;
	}

	// Token: 0x060031C7 RID: 12743
	public abstract void Add(LoopingSoundParameterUpdater.Sound sound);

	// Token: 0x060031C8 RID: 12744
	public abstract void Update(float dt);

	// Token: 0x060031C9 RID: 12745
	public abstract void Remove(LoopingSoundParameterUpdater.Sound sound);

	// Token: 0x02000A86 RID: 2694
	public struct Sound
	{
		// Token: 0x0400216E RID: 8558
		public EventInstance ev;

		// Token: 0x0400216F RID: 8559
		public HashedString path;

		// Token: 0x04002170 RID: 8560
		public Transform transform;

		// Token: 0x04002171 RID: 8561
		public SoundDescription description;

		// Token: 0x04002172 RID: 8562
		public bool objectIsSelectedAndVisible;
	}
}
