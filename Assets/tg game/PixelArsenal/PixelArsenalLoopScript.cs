using System.Collections;
using UnityEngine;

namespace PixelArsenal
{
	public class PixelArsenalLoopScript : MonoBehaviour
	{
		public GameObject chosenEffect;

		public float loopTimeLimit = 2f;

		private void Start()
		{
			PlayLoopingPEffect();
		}

		public void PlayLoopingPEffect()
		{
			StartCoroutine("EffectLoop");
		}

		private IEnumerator EffectLoop()
		{
			GameObject effectPlayer = UnityEngine.Object.Instantiate(chosenEffect, base.transform.position, base.transform.rotation);
			yield return new WaitForSeconds(loopTimeLimit);
			UnityEngine.Object.Destroy(effectPlayer);
			PlayLoopingPEffect();
		}
	}
}
