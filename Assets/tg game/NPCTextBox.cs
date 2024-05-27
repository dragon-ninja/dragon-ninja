using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

public class NPCTextBox : MonoBehaviour
{
	public TextMeshPro textObject;

	public string strText;

	public bool clearState;

	public string strClearText;

	private int strSize;

	private void Start()
	{
		base.transform.localScale = new Vector3(0f, 0f, 0f);
		if (clearState && (int)Singleton<DataManager>.Instance.clearStage >= 7)
		{
			strText = strClearText;
		}
		strSize = strText.Length;
		StartCoroutine(startText());
	}

	private IEnumerator startText()
	{
		while (true)
		{
			yield return new WaitForSeconds(UnityEngine.Random.Range(1f, 2f));
			onTextBox();
			yield return new WaitForSeconds(0.5f);
			StartCoroutine(updateText());
			yield return new WaitForSeconds(3f);
			offTextBox();
			yield return new WaitForSeconds(UnityEngine.Random.Range(3f, 5f));
		}
	}

	private IEnumerator updateText()
	{
		for (int i = 0; i < strSize; i++)
		{
			yield return new WaitForSeconds(0.1f);
			textObject.text += strText[i].ToString();
		}
	}

	private void onTextBox()
	{
		textObject.text = "";
		base.transform.localScale = new Vector3(0f, 0f, 0f);
		base.transform.DOScale(new Vector3(1f, 1f, 1f), 0.5f).SetEase(Ease.OutBack);
	}

	private void offTextBox()
	{
		base.transform.DOScale(new Vector3(0f, 0f, 0f), 0.5f).SetEase(Ease.InBack);
	}
}
