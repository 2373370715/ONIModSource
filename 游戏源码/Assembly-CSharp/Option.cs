using System;
using System.Collections.Generic;
using System.Diagnostics;
using KSerialization;

// Token: 0x02000602 RID: 1538
[DebuggerDisplay("has_value={hasValue} {value}")]
[Serializable]
public readonly struct Option<T> : IEquatable<Option<T>>, IEquatable<T>
{
	// Token: 0x06001BEE RID: 7150 RVA: 0x000B2388 File Offset: 0x000B0588
	public Option(T value)
	{
		this.value = value;
		this.hasValue = true;
	}

	// Token: 0x1700009C RID: 156
	// (get) Token: 0x06001BEF RID: 7151 RVA: 0x000B2398 File Offset: 0x000B0598
	public bool HasValue
	{
		get
		{
			return this.hasValue;
		}
	}

	// Token: 0x1700009D RID: 157
	// (get) Token: 0x06001BF0 RID: 7152 RVA: 0x000B23A0 File Offset: 0x000B05A0
	public T Value
	{
		get
		{
			return this.Unwrap();
		}
	}

	// Token: 0x06001BF1 RID: 7153 RVA: 0x000B23A8 File Offset: 0x000B05A8
	public T Unwrap()
	{
		if (!this.hasValue)
		{
			throw new Exception("Tried to get a value for a Option<" + typeof(T).FullName + ">, but hasValue is false");
		}
		return this.value;
	}

	// Token: 0x06001BF2 RID: 7154 RVA: 0x000B23DC File Offset: 0x000B05DC
	public T UnwrapOr(T fallback_value, string warn_on_fallback = null)
	{
		if (!this.hasValue)
		{
			if (warn_on_fallback != null)
			{
				DebugUtil.DevAssert(false, "Failed to unwrap a Option<" + typeof(T).FullName + ">: " + warn_on_fallback, null);
			}
			return fallback_value;
		}
		return this.value;
	}

	// Token: 0x06001BF3 RID: 7155 RVA: 0x000B2417 File Offset: 0x000B0617
	public T UnwrapOrElse(Func<T> get_fallback_value_fn, string warn_on_fallback = null)
	{
		if (!this.hasValue)
		{
			if (warn_on_fallback != null)
			{
				DebugUtil.DevAssert(false, "Failed to unwrap a Option<" + typeof(T).FullName + ">: " + warn_on_fallback, null);
			}
			return get_fallback_value_fn();
		}
		return this.value;
	}

	// Token: 0x06001BF4 RID: 7156 RVA: 0x001ACA90 File Offset: 0x001AAC90
	public T UnwrapOrDefault()
	{
		if (!this.hasValue)
		{
			return default(T);
		}
		return this.value;
	}

	// Token: 0x06001BF5 RID: 7157 RVA: 0x000B2457 File Offset: 0x000B0657
	public T Expect(string msg_on_fail)
	{
		if (!this.hasValue)
		{
			throw new Exception(msg_on_fail);
		}
		return this.value;
	}

	// Token: 0x06001BF6 RID: 7158 RVA: 0x000B2398 File Offset: 0x000B0598
	public bool IsSome()
	{
		return this.hasValue;
	}

	// Token: 0x06001BF7 RID: 7159 RVA: 0x000B246E File Offset: 0x000B066E
	public bool IsNone()
	{
		return !this.hasValue;
	}

	// Token: 0x06001BF8 RID: 7160 RVA: 0x000B2479 File Offset: 0x000B0679
	public Option<U> AndThen<U>(Func<T, U> fn)
	{
		if (this.IsNone())
		{
			return Option.None;
		}
		return Option.Maybe<U>(fn(this.value));
	}

	// Token: 0x06001BF9 RID: 7161 RVA: 0x000B249F File Offset: 0x000B069F
	public Option<U> AndThen<U>(Func<T, Option<U>> fn)
	{
		if (this.IsNone())
		{
			return Option.None;
		}
		return fn(this.value);
	}

	// Token: 0x06001BFA RID: 7162 RVA: 0x000B24C0 File Offset: 0x000B06C0
	public static implicit operator Option<T>(T value)
	{
		return Option.Maybe<T>(value);
	}

	// Token: 0x06001BFB RID: 7163 RVA: 0x000B24C8 File Offset: 0x000B06C8
	public static explicit operator T(Option<T> option)
	{
		return option.Unwrap();
	}

	// Token: 0x06001BFC RID: 7164 RVA: 0x001ACAB8 File Offset: 0x001AACB8
	public static implicit operator Option<T>(Option.Internal.Value_None value)
	{
		return default(Option<T>);
	}

	// Token: 0x06001BFD RID: 7165 RVA: 0x000B24D1 File Offset: 0x000B06D1
	public static implicit operator Option.Internal.Value_HasValue(Option<T> value)
	{
		return new Option.Internal.Value_HasValue(value.hasValue);
	}

	// Token: 0x06001BFE RID: 7166 RVA: 0x000B24DE File Offset: 0x000B06DE
	public void Deconstruct(out bool hasValue, out T value)
	{
		hasValue = this.hasValue;
		value = this.value;
	}

	// Token: 0x06001BFF RID: 7167 RVA: 0x000B24F4 File Offset: 0x000B06F4
	public bool Equals(Option<T> other)
	{
		return EqualityComparer<bool>.Default.Equals(this.hasValue, other.hasValue) && EqualityComparer<T>.Default.Equals(this.value, other.value);
	}

	// Token: 0x06001C00 RID: 7168 RVA: 0x001ACAD0 File Offset: 0x001AACD0
	public override bool Equals(object obj)
	{
		if (obj is Option<T>)
		{
			Option<T> other = (Option<T>)obj;
			return this.Equals(other);
		}
		return false;
	}

	// Token: 0x06001C01 RID: 7169 RVA: 0x000B2526 File Offset: 0x000B0726
	public static bool operator ==(Option<T> lhs, Option<T> rhs)
	{
		return lhs.Equals(rhs);
	}

	// Token: 0x06001C02 RID: 7170 RVA: 0x000B2530 File Offset: 0x000B0730
	public static bool operator !=(Option<T> lhs, Option<T> rhs)
	{
		return !(lhs == rhs);
	}

	// Token: 0x06001C03 RID: 7171 RVA: 0x001ACAF8 File Offset: 0x001AACF8
	public override int GetHashCode()
	{
		return (-363764631 * -1521134295 + this.hasValue.GetHashCode()) * -1521134295 + EqualityComparer<T>.Default.GetHashCode(this.value);
	}

	// Token: 0x06001C04 RID: 7172 RVA: 0x000B253C File Offset: 0x000B073C
	public override string ToString()
	{
		if (!this.hasValue)
		{
			return "None";
		}
		return string.Format("{0}", this.value);
	}

	// Token: 0x06001C05 RID: 7173 RVA: 0x000B2561 File Offset: 0x000B0761
	public static bool operator ==(Option<T> lhs, T rhs)
	{
		return lhs.Equals(rhs);
	}

	// Token: 0x06001C06 RID: 7174 RVA: 0x000B256B File Offset: 0x000B076B
	public static bool operator !=(Option<T> lhs, T rhs)
	{
		return !(lhs == rhs);
	}

	// Token: 0x06001C07 RID: 7175 RVA: 0x000B2577 File Offset: 0x000B0777
	public static bool operator ==(T lhs, Option<T> rhs)
	{
		return rhs.Equals(lhs);
	}

	// Token: 0x06001C08 RID: 7176 RVA: 0x000B2581 File Offset: 0x000B0781
	public static bool operator !=(T lhs, Option<T> rhs)
	{
		return !(lhs == rhs);
	}

	// Token: 0x06001C09 RID: 7177 RVA: 0x000B258D File Offset: 0x000B078D
	public bool Equals(T other)
	{
		return this.HasValue && EqualityComparer<T>.Default.Equals(this.value, other);
	}

	// Token: 0x1700009E RID: 158
	// (get) Token: 0x06001C0A RID: 7178 RVA: 0x001ACAB8 File Offset: 0x001AACB8
	public static Option<T> None
	{
		get
		{
			return default(Option<T>);
		}
	}

	// Token: 0x04001196 RID: 4502
	[Serialize]
	private readonly bool hasValue;

	// Token: 0x04001197 RID: 4503
	[Serialize]
	private readonly T value;
}
