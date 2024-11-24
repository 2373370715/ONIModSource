using System;

// Token: 0x02000607 RID: 1543
public readonly struct Padding
{
	// Token: 0x170000A0 RID: 160
	// (get) Token: 0x06001C10 RID: 7184 RVA: 0x000B25BB File Offset: 0x000B07BB
	public float Width
	{
		get
		{
			return this.left + this.right;
		}
	}

	// Token: 0x170000A1 RID: 161
	// (get) Token: 0x06001C11 RID: 7185 RVA: 0x000B25CA File Offset: 0x000B07CA
	public float Height
	{
		get
		{
			return this.top + this.bottom;
		}
	}

	// Token: 0x06001C12 RID: 7186 RVA: 0x000B25D9 File Offset: 0x000B07D9
	public Padding(float left, float right, float top, float bottom)
	{
		this.top = top;
		this.bottom = bottom;
		this.left = left;
		this.right = right;
	}

	// Token: 0x06001C13 RID: 7187 RVA: 0x000B25F8 File Offset: 0x000B07F8
	public static Padding All(float padding)
	{
		return new Padding(padding, padding, padding, padding);
	}

	// Token: 0x06001C14 RID: 7188 RVA: 0x000B2603 File Offset: 0x000B0803
	public static Padding Symmetric(float horizontal, float vertical)
	{
		return new Padding(horizontal, horizontal, vertical, vertical);
	}

	// Token: 0x06001C15 RID: 7189 RVA: 0x000B260E File Offset: 0x000B080E
	public static Padding Only(float left = 0f, float right = 0f, float top = 0f, float bottom = 0f)
	{
		return new Padding(left, right, top, bottom);
	}

	// Token: 0x06001C16 RID: 7190 RVA: 0x000B2619 File Offset: 0x000B0819
	public static Padding Vertical(float vertical)
	{
		return new Padding(0f, 0f, vertical, vertical);
	}

	// Token: 0x06001C17 RID: 7191 RVA: 0x000B262C File Offset: 0x000B082C
	public static Padding Horizontal(float horizontal)
	{
		return new Padding(horizontal, horizontal, 0f, 0f);
	}

	// Token: 0x06001C18 RID: 7192 RVA: 0x000B263F File Offset: 0x000B083F
	public static Padding Top(float amount)
	{
		return new Padding(0f, 0f, amount, 0f);
	}

	// Token: 0x06001C19 RID: 7193 RVA: 0x000B2656 File Offset: 0x000B0856
	public static Padding Left(float amount)
	{
		return new Padding(amount, 0f, 0f, 0f);
	}

	// Token: 0x06001C1A RID: 7194 RVA: 0x000B266D File Offset: 0x000B086D
	public static Padding Bottom(float amount)
	{
		return new Padding(0f, 0f, 0f, amount);
	}

	// Token: 0x06001C1B RID: 7195 RVA: 0x000B2684 File Offset: 0x000B0884
	public static Padding Right(float amount)
	{
		return new Padding(0f, amount, 0f, 0f);
	}

	// Token: 0x06001C1C RID: 7196 RVA: 0x000B269B File Offset: 0x000B089B
	public static Padding operator +(Padding a, Padding b)
	{
		return new Padding(a.left + b.left, a.right + b.right, a.top + b.top, a.bottom + b.bottom);
	}

	// Token: 0x06001C1D RID: 7197 RVA: 0x000B26D6 File Offset: 0x000B08D6
	public static Padding operator -(Padding a, Padding b)
	{
		return new Padding(a.left - b.left, a.right - b.right, a.top - b.top, a.bottom - b.bottom);
	}

	// Token: 0x06001C1E RID: 7198 RVA: 0x000B2711 File Offset: 0x000B0911
	public static Padding operator *(float f, Padding p)
	{
		return p * f;
	}

	// Token: 0x06001C1F RID: 7199 RVA: 0x000B271A File Offset: 0x000B091A
	public static Padding operator *(Padding p, float f)
	{
		return new Padding(p.left * f, p.right * f, p.top * f, p.bottom * f);
	}

	// Token: 0x06001C20 RID: 7200 RVA: 0x000B2741 File Offset: 0x000B0941
	public static Padding operator /(Padding p, float f)
	{
		return new Padding(p.left / f, p.right / f, p.top / f, p.bottom / f);
	}

	// Token: 0x04001199 RID: 4505
	public readonly float top;

	// Token: 0x0400119A RID: 4506
	public readonly float bottom;

	// Token: 0x0400119B RID: 4507
	public readonly float left;

	// Token: 0x0400119C RID: 4508
	public readonly float right;
}
