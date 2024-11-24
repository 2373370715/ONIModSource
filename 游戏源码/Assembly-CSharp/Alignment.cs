using System;
using UnityEngine;

// Token: 0x02001D66 RID: 7526
public readonly struct Alignment
{
	// Token: 0x06009D2C RID: 40236 RVA: 0x00106388 File Offset: 0x00104588
	public Alignment(float x, float y)
	{
		this.x = x;
		this.y = y;
	}

	// Token: 0x06009D2D RID: 40237 RVA: 0x00106398 File Offset: 0x00104598
	public static Alignment Custom(float x, float y)
	{
		return new Alignment(x, y);
	}

	// Token: 0x06009D2E RID: 40238 RVA: 0x001063A1 File Offset: 0x001045A1
	public static Alignment TopLeft()
	{
		return new Alignment(0f, 1f);
	}

	// Token: 0x06009D2F RID: 40239 RVA: 0x001063B2 File Offset: 0x001045B2
	public static Alignment Top()
	{
		return new Alignment(0.5f, 1f);
	}

	// Token: 0x06009D30 RID: 40240 RVA: 0x001063C3 File Offset: 0x001045C3
	public static Alignment TopRight()
	{
		return new Alignment(1f, 1f);
	}

	// Token: 0x06009D31 RID: 40241 RVA: 0x001063D4 File Offset: 0x001045D4
	public static Alignment Left()
	{
		return new Alignment(0f, 0.5f);
	}

	// Token: 0x06009D32 RID: 40242 RVA: 0x001063E5 File Offset: 0x001045E5
	public static Alignment Center()
	{
		return new Alignment(0.5f, 0.5f);
	}

	// Token: 0x06009D33 RID: 40243 RVA: 0x001063F6 File Offset: 0x001045F6
	public static Alignment Right()
	{
		return new Alignment(1f, 0.5f);
	}

	// Token: 0x06009D34 RID: 40244 RVA: 0x00106407 File Offset: 0x00104607
	public static Alignment BottomLeft()
	{
		return new Alignment(0f, 0f);
	}

	// Token: 0x06009D35 RID: 40245 RVA: 0x00106418 File Offset: 0x00104618
	public static Alignment Bottom()
	{
		return new Alignment(0.5f, 0f);
	}

	// Token: 0x06009D36 RID: 40246 RVA: 0x00106429 File Offset: 0x00104629
	public static Alignment BottomRight()
	{
		return new Alignment(1f, 0f);
	}

	// Token: 0x06009D37 RID: 40247 RVA: 0x0010643A File Offset: 0x0010463A
	public static implicit operator Vector2(Alignment a)
	{
		return new Vector2(a.x, a.y);
	}

	// Token: 0x06009D38 RID: 40248 RVA: 0x0010644D File Offset: 0x0010464D
	public static implicit operator Alignment(Vector2 v)
	{
		return new Alignment(v.x, v.y);
	}

	// Token: 0x04007B2D RID: 31533
	public readonly float x;

	// Token: 0x04007B2E RID: 31534
	public readonly float y;
}
