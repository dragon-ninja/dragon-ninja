using UnityEngine.Purchasing;

public struct ProductData
{
	public string strID;

	public ProductType type;

	public ProductData(string i, ProductType t)
	{
		this = default(ProductData);
		strID = i;
		type = t;
	}
}
