using System;

// Token: 0x02000603 RID: 1539
public static class Option
{
	// Token: 0x06001C0B RID: 7179 RVA: 0x000B25AA File Offset: 0x000B07AA
	public static Option<T> Some<T>(T value)
	{
		return new Option<T>(value);
	}

	// Token: 0x06001C0C RID: 7180 RVA: 0x001ACB38 File Offset: 0x001AAD38
	public static Option<T> Maybe<T>(T value)
	{
		if (value.IsNullOrDestroyed())
		{
			return default(Option<T>);
		}
		return new Option<T>(value);
	}

	// Token: 0x1700009F RID: 159
	// (get) Token: 0x06001C0D RID: 7181 RVA: 0x001ACB64 File Offset: 0x001AAD64
	public static Option.Internal.Value_None None
	{
		get
		{
			return default(Option.Internal.Value_None);
		}
	}

	// Token: 0x06001C0E RID: 7182 RVA: 0x001ACB7C File Offset: 0x001AAD7C
	public static bool AllHaveValues(params Option.Internal.Value_HasValue[] options)
	{
		if (options == null || options.Length == 0)
		{
			return false;
		}
		for (int i = 0; i < options.Length; i++)
		{
			if (!options[i].HasValue)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x02000604 RID: 1540
	public static class Internal
	{
		// Token: 0x02000605 RID: 1541
		public readonly struct Value_None
		{
		}

		// Token: 0x02000606 RID: 1542
		public readonly struct Value_HasValue
		{
			// Token: 0x06001C0F RID: 7183 RVA: 0x000B25B2 File Offset: 0x000B07B2
			public Value_HasValue(bool hasValue)
			{
				this.HasValue = hasValue;
			}

			// Token: 0x04001198 RID: 4504
			public readonly bool HasValue;
		}
	}
}
