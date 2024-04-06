using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*public class expCrystal : MonoBehaviour
{
    public DungeonManager mgr;
    public int exp = 1;
    Transform target;

    public void init() {
        target = null;
    }

    public void absorb() { 
        target = DungeonManager.player.transform;
    }

    private void Update()
    {
        if (Vector2.Distance(DungeonManager.player.transform.position, 
            transform.position) < DungeonManager.player.drawSize)
        {
            absorb();
        }

        if (Vector2.Distance(DungeonManager.player.transform.position, transform.position) < 1) {

            DungeonManager.player.expUp(exp);
            //DungeonManager.desExp(this);
        }

        if(target != null)
            transform.position += (target.position - transform.position).normalized * Time.deltaTime * 20;
    }
}*/


public class expCrystalfb
{
    public GameObject obj;
    public Transform transform;
    public DungeonManager mgr;
    public int exp = 1;
    Transform target;
    bool hide;

    public void init()
    {
        hide = false;
        target = null;
        transform = obj.GetComponent<Transform>();
    }

    public void absorb()
    {
        if (hide)
            return;

        target = DungeonManager.player.transform;
    }


    public void Update()
    {
        if (hide)
            return;

        if (Vector2.Distance(DungeonManager.player.transform.position,
            transform.position) < DungeonManager.player.drawSize)
        {
            absorb();
        }

        if (Vector2.Distance(DungeonManager.player.transform.position, transform.position) < 1)
        {
            DungeonManager.player.expUp(exp);
            hide = true;
            target = null;
            DungeonManager.desExp(this);
        }

        if (target != null)
            transform.position += (target.position - transform.position).normalized * Time.deltaTime * 20;
    }
}
