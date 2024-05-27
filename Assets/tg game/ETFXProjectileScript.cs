using UnityEngine;

public class ETFXProjectileScript : MonoBehaviour
{
	public GameObject impactParticle;

	public GameObject projectileParticle;

	public GameObject muzzleParticle;

	public GameObject[] trailParticles;

	[HideInInspector]
	public Vector3 impactNormal;

	private bool hasCollided;

	private void Start()
	{
		projectileParticle = UnityEngine.Object.Instantiate(projectileParticle, base.transform.position, base.transform.rotation);
		projectileParticle.transform.parent = base.transform;
		if ((bool)muzzleParticle)
		{
			muzzleParticle = UnityEngine.Object.Instantiate(muzzleParticle, base.transform.position, base.transform.rotation);
			UnityEngine.Object.Destroy(muzzleParticle, 1.5f);
		}
	}

	private void OnCollisionEnter(Collision hit)
	{
		if (hasCollided)
		{
			return;
		}
		hasCollided = true;
		impactParticle = UnityEngine.Object.Instantiate(impactParticle, base.transform.position, Quaternion.FromToRotation(Vector3.up, impactNormal));
		if (hit.gameObject.tag == "Destructible")
		{
			UnityEngine.Object.Destroy(hit.gameObject);
		}
		GameObject[] array = trailParticles;
		foreach (GameObject gameObject in array)
		{
			GameObject gameObject2 = base.transform.Find(projectileParticle.name + "/" + gameObject.name).gameObject;
			gameObject2.transform.parent = null;
			UnityEngine.Object.Destroy(gameObject2, 3f);
		}
		UnityEngine.Object.Destroy(projectileParticle, 3f);
		UnityEngine.Object.Destroy(impactParticle, 5f);
		UnityEngine.Object.Destroy(base.gameObject);
		ParticleSystem[] componentsInChildren = GetComponentsInChildren<ParticleSystem>();
		for (int j = 1; j < componentsInChildren.Length; j++)
		{
			ParticleSystem particleSystem = componentsInChildren[j];
			if (particleSystem.gameObject.name.Contains("Trail"))
			{
				particleSystem.transform.SetParent(null);
				UnityEngine.Object.Destroy(particleSystem.gameObject, 2f);
			}
		}
	}
}
