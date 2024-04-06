using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//技能指示器
public class Indicator : MonoBehaviour
{
    string IndicatorType;

    Transform child_0;
    Transform child_1;

    

    public void init()
    {
        child_0 = transform.GetChild(0);
        child_1 = child_0.transform.GetChild(0);
    }

    public void showSquare(float now, float max_x, float max_y) {

        now = Mathf.Min(1, now);

        if (child_0 == null)
            init();

        child_0.localScale = new Vector3(max_x, max_y, 1);
        child_0.localPosition = new Vector3(0, max_y / 2, 0);

        child_1.localPosition = new Vector3(0, (1 - now) /2 * -1, 0);
        child_1.localScale = new Vector3(1, now, 1);

        this.gameObject.SetActive(true);
    }

    public void showCircle(float now, float max_x, float max_y)
    {
        if (child_0 == null)
            init();

        now = Mathf.Min(1, now);


        child_0.localScale = new Vector3(max_x, max_y, 1);
        child_1.localScale = new Vector3(now, now, 1);

        this.gameObject.SetActive(true);
    }



    public void hide() {
        this.gameObject.SetActive(false);
    }
}
