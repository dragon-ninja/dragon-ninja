using UnityEngine;

public class ParticleLiveCheck : MonoBehaviour
{
	public ParticleSystem myParticle;

	public void Update()
	{
		if (!myParticle.IsAlive())
		{
			base.gameObject.SetActive(value: false);
		}
	}
}
