using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public int index;
    //本轮最大个数
    public int maxNum;
    //上一个节点和下一个节点障碍物
    public Obstacle lastObstacle;
    public Obstacle nextObstacle;
    public ObstacleAttr attr;
    float startY;
    public Vector3 startPos;

    public float now_duration;
    Transform sprite;
    SpriteRenderer spriteRenderer;
    List<Transform> lh_list;
    bool end;

    public bool activeFlag;
    public bool notActiveFlag;
    GameObject DlyStartProp;

    //该批次的最后一个障碍物  落地完成后需要生成一个开启dly的地标
    public bool endObstacle;
    //消失时通知组内障碍物一起消失
    public List<Obstacle> obstacleList = new List<Obstacle>();

    GameObject exPf;
    GameObject lineBox;


    private void Start()
    {
        exPf = Resources.Load<GameObject>("skill/dlyLine");
        sprite = transform.Find("sprite");
        spriteRenderer = sprite.GetComponent<SpriteRenderer>();
        startY = sprite.localPosition.y;
        sprite.localPosition = new Vector3(0, startY+10);

        lh_list = new List<Transform>();
        for (int i = 0; i<transform.childCount;i++) {
            Transform  c = transform.GetChild(i);
            if (c.name.IndexOf("裂痕") != -1) { 
                lh_list.Add(c);
                c.gameObject.SetActive(false);
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        //被激活
        if (activeFlag && !notActiveFlag) {
            notActiveFlag = true;
            Color c = spriteRenderer.color;
            c.a = 0.5f;
            spriteRenderer.color = c;
        }

        //坠落效果
        if (sprite.localPosition.y > startY) {

            Vector3 v = new Vector3(0, 1) * Time.deltaTime * attr.speed;
            if ((sprite.localPosition.y - v.y) < startY) {
                v.y = v.y - (startY - (sprite.localPosition.y - v.y));
            }
            sprite.localPosition -= v;
        }
        else if (!end) {
            end = true;
            for (int i = 0; i < lh_list.Count; i++)
            {
                lh_list[i].gameObject.SetActive(true);
            }

            //造成一次伤害判定
            List <Transform>  enemyTras = LockUtil.lockAll(this.transform,attr.size);
            for (int i=0;i < enemyTras.Count;i++) {
                enemyTras[i].GetComponent<Enemy>().hurt(1000);
            }

            //查找有没有触发点   没有则生成一个
            if (endObstacle) { 
                //GameObject[] starts = GameObject.FindGameObjectsWithTag("obstacle_start");
                //if (starts.Length == 0)
                { 
                    DlyStartProp = Instantiate(Resources.Load<GameObject>("prop/DlyStartProp"));
                    float x = Random.Range(1.0f, 1.5f) * (Random.value > 0.5 ? 1 : -1);
                    float y = Random.Range(1.0f, 1.5f) * (Random.value > 0.5 ? 1 : -1);
                    DlyStartProp.transform.position =
                        startPos + new Vector3(x, y, 0);
                }
            }
        }
        //持续时间到,自行消失
        now_duration += Time.deltaTime;
        if (now_duration > attr.time || desFlag)
        {
            foreach (Obstacle obs in obstacleList) {
                obs.desFlag = true;
            }
            obstacleList.Remove(this);
            Destroy(this.gameObject);
        }
    }
    public bool desFlag;

    public void creatLine() {

        if (lastObstacle == null)
            return;

        lineBox = Instantiate(exPf);
        lineBox.transform.position = new Vector3(0, 0, 0);


        List<LineRenderer> lineRendererList = new List<LineRenderer>();
        foreach (LineRenderer i in lineBox.transform.Find("notActive").GetComponentsInChildren<LineRenderer>())
        {
            lineRendererList.Add(i);
        }

        foreach (LineRenderer line in lineRendererList)
        {
            line.SetPosition(0, this.transform.position);
            line.SetPosition(1, lastObstacle.transform.position);
        }
    }

    public void changeLine() {

        lineBox.transform.Find("notActive").gameObject.SetActive(false);
        lineBox.transform.Find("active").gameObject.SetActive(true);

        List<LineRenderer> lineRendererList = new List<LineRenderer>();
        foreach (LineRenderer i in lineBox.transform.Find("active").GetComponentsInChildren<LineRenderer>())
        {
            lineRendererList.Add(i);
        }

        foreach (LineRenderer line in lineRendererList)
        {
            line.SetPosition(0, this.transform.position);
            line.SetPosition(1, lastObstacle.transform.position);
        }

       
    }

    public void hideLine() {
        if (lineBox != null)
            Destroy(lineBox);
    }

    public void OnDestroy()
    {
        if(DlyStartProp != null)
            Destroy(DlyStartProp);
    }
}
