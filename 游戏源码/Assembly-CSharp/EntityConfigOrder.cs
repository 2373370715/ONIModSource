using System;

public class EntityConfigOrder : Attribute
{
	public int sortOrder;

	public EntityConfigOrder(int sort_order)
	{
		sortOrder = sort_order;
	}
}
