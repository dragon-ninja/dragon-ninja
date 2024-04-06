using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageMeters : MonoBehaviour
{
    public static Dictionary<string, int> damageMap;

    public void Awake()
    {
        if (DataManager.Get()?.userData?.towerData?.damageMap != null)
            damageMap = DataManager.Get().userData.towerData.damageMap;
        else
            damageMap = new Dictionary<string, int>();
    }
}
