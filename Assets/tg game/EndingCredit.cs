using System.Collections;
using UnityEngine;

public class EndingCredit : MonoBehaviour
{
	private float speed;

	public void onCredit()
	{
		base.gameObject.SetActive(value: true);
		speed = UnityEngine.Random.Range(0.5f, 1.5f);
		StartCoroutine(moveUpdate());
	}

	private IEnumerator moveUpdate()
	{
		while (true)
		{
			base.transform.position -= new Vector3(speed * Time.deltaTime, 0f, 0f);
			if (base.transform.position.x < -90f)
			{
				break;
			}
			yield return null;
		}
		base.gameObject.SetActive(value: false);
	}
}
