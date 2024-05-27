using UnityEngine;

public struct BackgroundItemParticle
{
	public GameObject obj;

	public Vector3 position;

	public BackgroundItemParticle(GameObject data, Vector3 p)
	{
		this = default(BackgroundItemParticle);
		obj = data;
		position = p;
	}
}
