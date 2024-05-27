using CodeStage.AntiCheat.ObscuredTypes;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	public SpriteRenderer spriteRenderer;

	public SpriteRenderer shadowRenderer;

	public SpriteRenderer hpRenderer;

	public SpriteRenderer hpBGRenderer;

	public List<Sprite> listHPsprites = new List<Sprite>();

	private EnemyType type;

	private ObscuredInt maxHP;

	private ObscuredInt hp;

	private ObscuredInt power;

	private Vector3 startPosition;

	private float idleAniTime = 0.5f;

	private float attackAniTime = 0.5f;

	private GameObject idleObject;

	private Vector3 startScale;

	private float maxWidthSize;

	private IEnumerator attackCoroutine;

	private bool powerDebuf;

	private float powerDebufSize;

	public EnemyType GetEnemyType()
	{
		return type;
	}

	public GameObject getIdleObject()
	{
		
		return idleObject;
	}

	public void initEnemy(EnemyType t, ObscuredInt level, Vector3 startpos, int stage, float maxWidth, int imageIndex)
	{
		powerDebuf = false;
		powerDebufSize = 0f;
		type = t;
		startPosition = startpos;
		switch (type)
		{
		case EnemyType.TYPE_1:
			maxHP = Balance.getLevelByHP_1(stage, level);
			power = Balance.getLevelByPOWER_1(stage, level);
			break;
		case EnemyType.TYPE_2:
			maxHP = Balance.getLevelByHP_2(stage, level);
			power = Balance.getLevelByPOWER_2(stage, level);
			break;
		case EnemyType.TYPE_3:
			maxHP = Balance.getLevelByHP_3(stage, level);
			power = Balance.getLevelByPOWER_3(stage, level);
			break;
		case EnemyType.TYPE_4:
			maxHP = Balance.getLevelByHP_4(stage, level);
			power = Balance.getLevelByPOWER_4(stage, level);
			break;
		case EnemyType.TYPE_BOSS:
			maxHP = Balance.getLevelByBossHP(stage, level, imageIndex);
			power = Balance.getLevelByBossPOWER(stage, level, imageIndex);
			break;
		}
		hp = maxHP;
		idleObject = UnityEngine.Object.Instantiate(Singleton<CustomLoadManager>.Instance.LoadSpriteEnemyBody(imageIndex));
		idleObject.transform.parent = base.transform;
		idleObject.transform.localPosition = new Vector3(0f, 0f, 0f);
		idleObject.GetComponent<EnemyAction>().onAction();
		startScale = idleObject.transform.localScale;
		idleObject.transform.localScale = new Vector3(1f, 1f, 1f);
		maxWidthSize = maxWidth;
		spriteRenderer.transform.localScale = startScale;
		spriteRenderer.sprite = Singleton<CustomLoadManager>.Instance.LoadSpriteEnemyAttack(imageIndex);
		spriteRenderer.enabled = false;
		string str = "Objects/Shadow/";
		str = ((t != EnemyType.TYPE_BOSS) ? (str + "shadow") : ((stage != 8) ? (str + "boss_shadow1") : (str + "boss_shadow2")));
		shadowRenderer.sprite = Singleton<AssetManager>.Instance.LoadSprite(str);
		if (type == EnemyType.TYPE_BOSS)
		{
			hpRenderer.sprite = listHPsprites[2];
			hpBGRenderer.sprite = listHPsprites[3];
		}
		else
		{
			hpRenderer.sprite = listHPsprites[0];
			hpBGRenderer.sprite = listHPsprites[1];
		}
		float num = spriteRenderer.sprite.rect.height / spriteRenderer.sprite.pixelsPerUnit;
		float num2 = hpRenderer.sprite.rect.width / hpRenderer.sprite.pixelsPerUnit / 2f;
		hpBGRenderer.transform.localPosition = new Vector3(0f, num + num * 0.1f, -1f);
		hpRenderer.transform.localPosition = new Vector3(0f - num2, num + num * 0.1f, -2f);
		hpBGRenderer.enabled = false;
		hpRenderer.enabled = false;
		StartCoroutine(startIdleAction());
	}

	private IEnumerator startIdleAction()
	{
		yield return new WaitForSeconds(UnityEngine.Random.Range(0f, 0.5f));
		idle();
	}

	public void restart(bool flip)
	{
		powerDebuf = false;
		powerDebufSize = 0f;
		base.transform.position = startPosition;
		spriteRenderer.DOKill();
		shadowRenderer.DOKill();
		spriteRenderer.color = Color.white;
		spriteRenderer.flipX = flip;
		shadowRenderer.color = Color.white;
		shadowRenderer.flipX = flip;
		spriteRenderer.enabled = false;
		idleObject.SetActive(value: true);
		idleObject.transform.localScale = new Vector3((!flip) ? 1 : (-1), 1f, 1f);
		hp = maxHP;
		hpBGRenderer.enabled = false;
		hpRenderer.enabled = false;
		idle();
	}

	public bool addDamage(int damage, float direction)
	{
		hp = (int)hp - damage;
		if ((int)hp <= 0)
		{
			objectDie(direction);
			hpBGRenderer.enabled = false;
			hpRenderer.enabled = false;
			return true;
		}
		refreshHPUI();
		if (type != EnemyType.TYPE_BOSS)
		{
			attack();
		}
		knockBack(direction);
		return false;
	}

	private void refreshHPUI()
	{
		hpBGRenderer.enabled = true;
		hpRenderer.enabled = true;
		int num = hp;
		int num2 = maxHP;
		float x = (float)num / (float)num2;
		hpRenderer.transform.localScale = new Vector3(x, 1f, 1f);
	}

	public void setDebufState(bool s, float v = 0f)
	{
		powerDebuf = s;
		powerDebufSize = v;
	}

	public bool getDebufState()
	{
		return powerDebuf;
	}

	public int getPower()
	{
		int num = power;
		if (powerDebuf)
		{
			num -= (int)((float)(int)power * powerDebufSize);
			if (num < 0)
			{
				num = 0;
			}
		}
		return num;
	}

	public void objectDie(float direction)
	{
		spriteRenderer.enabled = true;
		idleObject.SetActive(value: false);
		spriteRenderer.DOFade(0f, 1f);
		shadowRenderer.DOFade(0f, 1f);
		base.transform.DOKill();
		base.transform.DOMove(new Vector3(direction * 0.5f, 0f, 0f), 0.1f).SetRelative(isRelative: true);
	}

	public bool isLife()
	{
		return (int)hp > 0;
	}

	private void idle()
	{
		spriteRenderer.enabled = false;
		idleObject.SetActive(value: true);
	}

	private void attack()
	{
		spriteRenderer.enabled = true;
		idleObject.SetActive(value: false);
	}

	private void knockBack(float direction)
	{
		if (base.transform.position.x < maxWidthSize - 1f)
		{
			float num = 0.5f;
			if (type == EnemyType.TYPE_BOSS)
			{
				num = 0.1f;
			}
			base.transform.DOMove(new Vector3(direction * num, 0f, 0f), 0.1f).SetRelative(isRelative: true).OnComplete(delegate
			{
				if (type != EnemyType.TYPE_BOSS)
				{
					idle();
				}
			});
		}
	}
}
