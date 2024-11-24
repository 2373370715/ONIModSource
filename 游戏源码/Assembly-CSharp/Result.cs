using System;

public readonly struct Result<TSuccess, TError>
{
	private readonly Option<TSuccess> successValue;

	private readonly Option<TError> errorValue;

	private Result(TSuccess successValue, TError errorValue)
	{
		this.successValue = successValue;
		this.errorValue = errorValue;
	}

	public bool IsOk()
	{
		return successValue.IsSome();
	}

	public bool IsErr()
	{
		if (!errorValue.IsSome())
		{
			return successValue.IsNone();
		}
		return true;
	}

	public TSuccess Unwrap()
	{
		if (successValue.IsSome())
		{
			return successValue.Unwrap();
		}
		if (errorValue.IsSome())
		{
			throw new Exception("Tried to unwrap result that is an Err()");
		}
		throw new Exception("Tried to unwrap result that isn't initialized with an Err() or Ok() value");
	}

	public Option<TSuccess> Ok()
	{
		return successValue;
	}

	public Option<TError> Err()
	{
		return errorValue;
	}

	public static implicit operator Result<TSuccess, TError>(Result.Internal.Value_Ok<TSuccess> value)
	{
		return new Result<TSuccess, TError>(value.value, default(TError));
	}

	public static implicit operator Result<TSuccess, TError>(Result.Internal.Value_Err<TError> value)
	{
		return new Result<TSuccess, TError>(default(TSuccess), value.value);
	}
}
public static class Result
{
	public static class Internal
	{
		public readonly struct Value_Ok<T>
		{
			public readonly T value;

			public Value_Ok(T value)
			{
				this.value = value;
			}
		}

		public readonly struct Value_Err<T>
		{
			public readonly T value;

			public Value_Err(T value)
			{
				this.value = value;
			}
		}
	}

	public static Internal.Value_Ok<T> Ok<T>(T value)
	{
		return new Internal.Value_Ok<T>(value);
	}

	public static Internal.Value_Err<T> Err<T>(T value)
	{
		return new Internal.Value_Err<T>(value);
	}
}
