namespace Database;

public class Shirts : ResourceSet<Shirt>
{
	public Shirt Hot00;

	public Shirt Decor00;

	public Shirts()
	{
		Hot00 = Add(new Shirt("body_shirt_hot_shearling"));
		Decor00 = Add(new Shirt("body_shirt_decor01"));
	}
}
