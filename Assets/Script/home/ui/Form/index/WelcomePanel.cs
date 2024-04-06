using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class WelcomePanel : MonoBehaviour
{

    TextMeshProUGUI desc;
    CanvasGroup group;

    // Start is called before the first frame update
    void Start()
    {
        desc = GetComponent<TextMeshProUGUI>();
        group = GetComponent<CanvasGroup>();
    }


    public void show(string userName) {
        desc.text = "Welcome back " + userName;
        StartCoroutine(fade());
    }

    IEnumerator fade() {
        DOTween.Clear();
        group.alpha = 1;
        yield return new WaitForSeconds(0.7f);
        group.DOFade(0, 1f);
    }
}
