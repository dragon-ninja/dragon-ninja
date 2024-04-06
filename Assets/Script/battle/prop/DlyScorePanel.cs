using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DlyScorePanel : MonoBehaviour
{
    float time;

    private void OnEnable()
    {
        time = 1;
    }

    private void Update()
    {
        time -= Time.deltaTime;

        if (time <= 0)
        {
            this.gameObject.SetActive(false);
        }
    }
}
