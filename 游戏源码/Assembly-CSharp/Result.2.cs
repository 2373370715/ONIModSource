using System;

// Token: 0x02000616 RID: 1558
public static class Result
{
	// Token: 0x06001C5F RID: 7263 RVA: 0x000B2A41 File Offset: 0x000B0C41
	public static Result.Internal.Value_Ok<T> Ok<T>(T value)
	{
		return new Result.Internal.Value_Ok<T>(value);
	}

	// Token: 0x06001C60 RID: 7264 RVA: 0x000B2A49 File Offset: 0x000B0C49
	public static Result.Internal.Value_Err<T> Err<T>(T value)
	{
		return new Result.Internal.Value_Err<T>(value);
	}

	// Token: 0x02000617 RID: 1559
	public static class Internal
	{
		// Token: 0x02000618 RID: 1560
		public readonly struct Value_Ok<T>
		{
			// Token: 0x06001C61 RID: 7265 RVA: 0x000B2A51 File Offset: 0x000B0C51
			public Value_Ok(T value)
			{
				this.value = value;
			}

			// Token: 0x040011B5 RID: 4533
			public readonly T value;
		}

		// Token: 0x02000619 RID: 1561
		public readonly struct Value_Err<T>
		{
			// Token: 0x06001C62 RID: 7266 RVA: 0x000B2A5A File Offset: 0x000B0C5A
			public Value_Err(T value)
			{
				this.value = value;
			}

			// Token: 0x040011B6 RID: 4534
			public readonly T value;
		}
	}
}
