 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using zFrame.UI;
using Spine.Unity;


public class PlayerController : MonoBehaviour
{
    public Player player;

    [SerializeField] Joystick joysticks;

    public SpriteRenderer BQ;


    public int moveState = 2;  //0是静止 1是移动
    float moveTime = 0;
    float oldMoveTime = 0;
    float idleTime = 0;
    public Animator animator_move;
    public Animator animator_idle;
    public AnimatorUtil animatorUtil_move;
    public AnimatorUtil animatorUtil_idle;
    int oldfacingDirection = -1;

    Transform attackIndicator;
    public Vector2 moveVec;

    Transform shotDirTra;


    private void Awake()
    {
        if (DungeonManager.zb_mode == 0) { 
            joysticks = GameObject.Find("Joystick-Left").GetComponent<Joystick>();
            joysticks.OnValueChanged.AddListener(v =>
            {
                if (v.magnitude != 0)
                {
                    move(v.x, v.y);
                }
            });
        }
        /* string path = "GunSpine";// "KatanaSpine";

         animator_move = this.transform.Find(path+"Spine").GetComponent<Animator>();
         animator_idle = this.transform.Find(path+"Spine_1").GetComponent<Animator>();

         animatorUtil_move = new AnimatorUtil(animator_move);
         animator_move.gameObject.SetActive(true);
         animatorUtil_idle = new AnimatorUtil(animator_idle);
         animator_idle.gameObject.SetActive(false);

         BQ = transform.Find("BQ").GetComponent<SpriteRenderer>();
         BQ.transform.localScale = new Vector3(0,0,1);

         attackIndicator = transform.Find("attackIndicator");
         moveVec = new Vector2(-1,0);

         */
        initSpine("GunSpine");
    }

    public void initSpine(string path)
    {
        Debug.Log(" path:" + path);
        if (path == "GunSpine") {
            this.transform.Find("GunSpine").gameObject.SetActive(true);
            this.transform.Find("KatanaSpine").gameObject.SetActive(false);
        }
        else if (path == "KatanaSpine")
        {
            this.transform.Find("GunSpine").gameObject.SetActive(false);
            this.transform.Find("KatanaSpine").gameObject.SetActive(true);
        }

        animator_move = this.transform.Find(path).Find("Spine").GetComponent<Animator>();
        animator_idle = this.transform.Find(path).Find("Spine_1").GetComponent<Animator>();

        animatorUtil_move = new AnimatorUtil(animator_move);
        animator_move.gameObject.SetActive(true);
        animatorUtil_idle = new AnimatorUtil(animator_idle);
        animator_idle.gameObject.SetActive(false);

        BQ = transform.Find("BQ").GetComponent<SpriteRenderer>();
        BQ.transform.localScale = new Vector3(0, 0, 1);

        attackIndicator = transform.Find("attackIndicator");
        moveVec = new Vector2(-1, 0);

        shotDirTra = transform.Find("GunSpine").Find("shotDir");
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
        oldfacingDirection = player.facingDirection;
        moveState = 2;
    }

  

    private void Update()
    {
        if (DlySkill.dlyIngFlag || DlySkill.dlyReadyEndFlag)
            return;

        //#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A)) {
            move(-0.75f, 0.75f);
        }
        else if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D))
        {
            move(0.75f, 0.75f);
        }
        else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A))
        {
            move(-0.75f, -0.75f);
        }
        else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D))
        {
            move(0.75f, -0.75f);
        }
        else if (Input.GetKey(KeyCode.W))
        {
            move(0, 1);
        }
        else if(Input.GetKey(KeyCode.S))
        {
            move(0, -1);
        }
        else if(Input.GetKey(KeyCode.A))
        {
            move(-1, 0);
        }
        else if(Input.GetKey(KeyCode.D))
        {
            move(1, 0);
        }
//#endif
    }

    private void LateUpdate()
    {
        if (DlySkill.dlyIngFlag || DlySkill.dlyReadyEndFlag)
            return;

        if (moveTime == oldMoveTime)
        {
            //animator_move.Play("idle");
            //animator_idle.Play("idle");
            animatorUtil_move.TrySet("move", false);
            animatorUtil_idle.TrySet("move", false);
            idleTime += Time.deltaTime;
            if (idleTime > 0.5f && moveState!=0)
            {
                moveTime = 0;
                oldMoveTime = 0;
                idleTime = 0;
                moveState = 0;
                //animator_move.gameObject.SetActive(false);
                //animator_idle.gameObject.SetActive(true);
                //player.skm.skeleton.SetColor(new Color32(255, 255, 255, 255));
                /*idle_change_flag = true;
                move_change_flag = false;*/
            }
        }
        else {
            animatorUtil_move.TrySet("move", true);
            animatorUtil_idle.TrySet("move", true);
            oldMoveTime = moveTime;
            //if (moveTime > 0.5f && moveState != 1)
            {
                moveTime = 0;
                oldMoveTime = 0;
                idleTime = 0;
                moveState = 1;
                //animator_move.gameObject.SetActive(true);
                //animator_idle.gameObject.SetActive(false);
                /* idle_change_flag = false;
                 move_change_flag = true;
                 BQ.transform.localScale = new Vector3(1.5f, 1.5f, 1);*/
                // player.skm.skeleton.SetColor(new Color32(200, 255, 255, 255));
            }
        }

        changeAnim();
    }

    public void move(float x,float y)
    {
        if (player.stiffTime > 0 )
            return;

        moveTime += Time.deltaTime;

        if (!Physics2D.Linecast(transform.position,
            transform.position + (new Vector3(x, y).normalized * 1f), LayerMask.GetMask("wall"))&&
            !Physics2D.Linecast(transform.position,
            transform.position + (new Vector3(x, y).normalized * 1f), LayerMask.GetMask("bound")))
        {
            transform.position += new Vector3(x, y).normalized
               * Time.deltaTime
               * player.speed_now * 0.07f;
        }
     



        if (x != 0)
            player.facingDirection = x > 0 ? 1 : -1;
        if (y != 0)
            player.updownDirection = y > 0 ? 1 : -1;


        if (oldfacingDirection != player.facingDirection) { 
            transform.localScale = new Vector3
                (transform.localScale.x * -1,
                 transform.localScale.y, transform.localScale.z);
        }

        oldfacingDirection = player.facingDirection;

        moveVec = new Vector2(x, y).normalized;
        float boxAngle = Vector2.Angle(transform.up, new Vector3(x, y).normalized);
        attackIndicator.localEulerAngles = new Vector3(0, 0, (float)boxAngle);

       /* foreach (EntourageZB e in player.EntourageList) {
            e.UpdatePos();
        }*/
    }

    public void action() { 
    }


    public bool idle_change_flag;
    public bool move_change_flag;
    //形态切换效果表现
    public void changeAnim() {
        if (idle_change_flag) {
            if (BQ.transform.localScale.x < 1.5)
            {
                BQ.transform.localScale = new Vector3(
                     BQ.transform.localScale.x + 0.07f * 150 *Time.deltaTime,
                      BQ.transform.localScale.y + 0.07f * 150 * Time.deltaTime,
                       1
                    );
                BQ.transform.Rotate(new Vector3(0, 0, -1) * Time.deltaTime * 100);
            }
            else {
                idle_change_flag = false;
                BQ.transform.localScale = new Vector3(0, 0, 1);
            }
        }
        if (move_change_flag)
        {
            if (BQ.transform.localScale.x >0)
            {
                BQ.transform.localScale = new Vector3(
                     BQ.transform.localScale.x - 0.07f * 150 * Time.deltaTime,
                      BQ.transform.localScale.y - 0.07f * 150 * Time.deltaTime,
                       1
                    );
                BQ.transform.Rotate(new Vector3(0, 0, 1) * Time.deltaTime * 100);
            }
            else
            {
                move_change_flag = false;
                BQ.transform.localScale = new Vector3(0, 0, 1);
            }
        }
    }


    public void setShotDir(Vector3 dir) {
        float angle = Vector3.Angle(Vector3.up, dir);
        float r = ((-1 * angle) - 40);
        shotDirTra.localEulerAngles =  (new Vector3(0,0,r));
    }
}
