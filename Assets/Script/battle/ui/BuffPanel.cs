using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffPanel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        List<string> buffList = DataManager.Get().userData.towerData.buffList;


        for (int i = 0; i < buffList.Count; i++)
        {
            Debug.Log("buff:   " + buffList[i].Replace("buff_", ""));

        }
    }
}
