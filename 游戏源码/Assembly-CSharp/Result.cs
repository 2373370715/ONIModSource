using System;

// Token: 0x02000615 RID: 1557
public readonly struct Result<TSuccess, TError>
{
	// Token: 0x06001C57 RID: 7255 RVA: 0x000B29B1 File Offset: 0x000B0BB1
	private Result(TSuccess successValue, TError errorValue)
	{
		this.successValue = successValue;
		this.errorValue = errorValue;
	}

	// Token: 0x06001C58 RID: 7256 RVA: 0x000B29CB File Offset: 0x000B0BCB
	public bool IsOk()
	{
		return this.successValue.IsSome();
	}

	// Token: 0x06001C59 RID: 7257 RVA: 0x000B29D8 File Offset: 0x000B0BD8
	public bool IsErr()
	{
		return this.errorValue.IsSome() || this.successValue.IsNone();
	}

	// Token: 0x06001C5A RID: 7258 RVA: 0x000B29F4 File Offset: 0x000B0BF4
	public TSuccess Unwrap()
	{
		if (this.successValue.IsSome())
		{
			return this.successValue.Unwrap();
		}
		if (this.errorValue.IsSome())
		{
			throw new Exception("Tried to unwrap result that is an Err()");
		}
		throw new Exception("Tried to unwrap result that isn't initialized with an Err() or Ok() value");
	}

	// Token: 0x06001C5B RID: 7259 RVA: 0x000B2A31 File Offset: 0x000B0C31
	public Option<TSuccess> Ok()
	{
		return this.successValue;
	}

	// Token: 0x06001C5C RID: 7260 RVA: 0x000B2A39 File Offset: 0x000B0C39
	public Option<TError> Err()
	{
		return this.errorValue;
	}

	// Token: 0x06001C5D RID: 7261 RVA: 0x001ACFF0 File Offset: 0x001AB1F0
	public static implicit operator Result<TSuccess, TError>(Result.Internal.Value_Ok<TSuccess> value)
	{
		return new Result<TSuccess, TError>(value.value, default(TError));
	}

	// Token: 0x06001C5E RID: 7262 RVA: 0x001AD014 File Offset: 0x001AB214
	public static implicit operator Result<TSuccess, TError>(Result.Internal.Value_Err<TError> value)
	{
		return new Result<TSuccess, TError>(default(TSuccess), value.value);
	}

	// Token: 0x040011B3 RID: 4531
	private readonly Option<TSuccess> successValue;

	// Token: 0x040011B4 RID: 4532
	private readonly Option<TError> errorValue;
}
