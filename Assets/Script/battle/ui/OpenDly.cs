using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class OpenDly : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public Transform target;



    private void OnEnable()
    {
        target.gameObject.SetActive(false);
        DOTween.Clear();
        canvasGroup.alpha = 0;
        transform.localPosition = new Vector3(0,0,0);
        StartCoroutine(open());
    }

    IEnumerator open() {
        canvasGroup.DOFade(1, 0.5f);
        yield return new WaitForSeconds(0.5f);
        transform.DOMove(target.position, 1.2f);
        yield return new WaitForSeconds(0.5f);
        canvasGroup.DOFade(0, 1f);
        yield return new WaitForSeconds(0.5f);
        target.gameObject.SetActive(true);
    }

}
