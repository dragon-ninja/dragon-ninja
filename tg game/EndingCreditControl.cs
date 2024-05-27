using System.Collections.Generic;
using UnityEngine;

public class EndingCreditControl : MonoBehaviour
{
	public List<EndingCredit> listCreditItems = new List<EndingCredit>();

	public void startAction()
	{
		base.gameObject.SetActive(value: true);
		int count = listCreditItems.Count;
		for (int i = 0; i < count; i++)
		{
			listCreditItems[i].onCredit();
		}
	}
}
