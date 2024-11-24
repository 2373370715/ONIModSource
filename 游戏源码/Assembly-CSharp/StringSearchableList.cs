using System.Collections.Generic;

public class StringSearchableList<T>
{
	public delegate bool ShouldFilterOutFn(T candidateValue, in string filter);

	public string filter = "";

	public List<T> allValues;

	public List<T> filteredValues;

	public readonly ShouldFilterOutFn shouldFilterOutFn;

	public bool didUseFilter { get; private set; }

	public StringSearchableList(List<T> allValues, ShouldFilterOutFn shouldFilterOutFn)
	{
		this.allValues = allValues;
		this.shouldFilterOutFn = shouldFilterOutFn;
		filteredValues = new List<T>();
	}

	public StringSearchableList(ShouldFilterOutFn shouldFilterOutFn)
	{
		this.shouldFilterOutFn = shouldFilterOutFn;
		allValues = new List<T>();
		filteredValues = new List<T>();
	}

	public void Refilter()
	{
		if (StringSearchableListUtil.ShouldUseFilter(filter))
		{
			filteredValues.Clear();
			foreach (T allValue in allValues)
			{
				if (!shouldFilterOutFn(allValue, in filter))
				{
					filteredValues.Add(allValue);
				}
			}
			didUseFilter = true;
		}
		else
		{
			if (filteredValues.Count != allValues.Count)
			{
				filteredValues.Clear();
				filteredValues.AddRange(allValues);
			}
			didUseFilter = false;
		}
	}
}
