using System.Collections.Generic;
using UnityEngine;

public class CrackPortalOpen : MonoBehaviour
{
	public List<GameObject> particles;

	public void onPortal()
	{
		foreach (GameObject particle in particles)
		{
			particle.SetActive(value: true);
		}
	}
}
