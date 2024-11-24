using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using KSerialization;

[Serializable]
[DebuggerDisplay("has_value={hasValue} {value}")]
public readonly struct Option<T> : IEquatable<Option<T>>, IEquatable<T>
{
	[Serialize]
	private readonly bool hasValue;

	[Serialize]
	private readonly T value;

	public bool HasValue => hasValue;

	public T Value => Unwrap();

	public static Option<T> None => default(Option<T>);

	public Option(T value)
	{
		this.value = value;
		hasValue = true;
	}

	public T Unwrap()
	{
		if (!hasValue)
		{
			throw new Exception("Tried to get a value for a Option<" + typeof(T).FullName + ">, but hasValue is false");
		}
		return value;
	}

	public T UnwrapOr(T fallback_value, string warn_on_fallback = null)
	{
		if (!hasValue)
		{
			if (warn_on_fallback != null)
			{
				DebugUtil.DevAssert(test: false, "Failed to unwrap a Option<" + typeof(T).FullName + ">: " + warn_on_fallback);
			}
			return fallback_value;
		}
		return value;
	}

	public T UnwrapOrElse(Func<T> get_fallback_value_fn, string warn_on_fallback = null)
	{
		if (!hasValue)
		{
			if (warn_on_fallback != null)
			{
				DebugUtil.DevAssert(test: false, "Failed to unwrap a Option<" + typeof(T).FullName + ">: " + warn_on_fallback);
			}
			return get_fallback_value_fn();
		}
		return value;
	}

	public T UnwrapOrDefault()
	{
		if (!hasValue)
		{
			return default(T);
		}
		return value;
	}

	public T Expect(string msg_on_fail)
	{
		if (!hasValue)
		{
			throw new Exception(msg_on_fail);
		}
		return value;
	}

	public bool IsSome()
	{
		return hasValue;
	}

	public bool IsNone()
	{
		return !hasValue;
	}

	public Option<U> AndThen<U>(Func<T, U> fn)
	{
		if (IsNone())
		{
			return Option.None;
		}
		return Option.Maybe(fn(value));
	}

	public Option<U> AndThen<U>(Func<T, Option<U>> fn)
	{
		if (IsNone())
		{
			return Option.None;
		}
		return fn(value);
	}

	public static implicit operator Option<T>(T value)
	{
		return Option.Maybe(value);
	}

	public static explicit operator T(Option<T> option)
	{
		return option.Unwrap();
	}

	public static implicit operator Option<T>(Option.Internal.Value_None value)
	{
		return default(Option<T>);
	}

	public static implicit operator Option.Internal.Value_HasValue(Option<T> value)
	{
		return new Option.Internal.Value_HasValue(value.hasValue);
	}

	public void Deconstruct(out bool hasValue, out T value)
	{
		hasValue = this.hasValue;
		value = this.value;
	}

	public bool Equals(Option<T> other)
	{
		if (EqualityComparer<bool>.Default.Equals(hasValue, other.hasValue))
		{
			return EqualityComparer<T>.Default.Equals(value, other.value);
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (obj is Option<T> other)
		{
			return Equals(other);
		}
		return false;
	}

	public static bool operator ==(Option<T> lhs, Option<T> rhs)
	{
		return lhs.Equals(rhs);
	}

	public static bool operator !=(Option<T> lhs, Option<T> rhs)
	{
		return !(lhs == rhs);
	}

	public override int GetHashCode()
	{
		return (-363764631 * -1521134295 + hasValue.GetHashCode()) * -1521134295 + EqualityComparer<T>.Default.GetHashCode(value);
	}

	public override string ToString()
	{
		if (!hasValue)
		{
			return "None";
		}
		return $"{value}";
	}

	public static bool operator ==(Option<T> lhs, T rhs)
	{
		return lhs.Equals(rhs);
	}

	public static bool operator !=(Option<T> lhs, T rhs)
	{
		return !(lhs == rhs);
	}

	public static bool operator ==(T lhs, Option<T> rhs)
	{
		return rhs.Equals(lhs);
	}

	public static bool operator !=(T lhs, Option<T> rhs)
	{
		return !(lhs == rhs);
	}

	public bool Equals(T other)
	{
		if (!HasValue)
		{
			return false;
		}
		return EqualityComparer<T>.Default.Equals(value, other);
	}
}
public static class Option
{
	public static class Internal
	{
		[StructLayout(LayoutKind.Sequential, Size = 1)]
		public readonly struct Value_None
		{
		}

		public readonly struct Value_HasValue
		{
			public readonly bool HasValue;

			public Value_HasValue(bool hasValue)
			{
				HasValue = hasValue;
			}
		}
	}

	public static Internal.Value_None None => default(Internal.Value_None);

	public static Option<T> Some<T>(T value)
	{
		return new Option<T>(value);
	}

	public static Option<T> Maybe<T>(T value)
	{
		if (value.IsNullOrDestroyed())
		{
			return default(Option<T>);
		}
		return new Option<T>(value);
	}

	public static bool AllHaveValues(params Internal.Value_HasValue[] options)
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
}
