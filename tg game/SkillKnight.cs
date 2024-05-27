using CodeStage.AntiCheat.ObscuredTypes;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillKnight : MonoBehaviour
{
	[Header("Objects")]
	public GameObject mainCamera;

	public GameObject gameScene;

	public GameObject player;

	public GameObject attackRed;

	public List<SpriteRenderer> listKnifeObjects;

	public List<SpriteRenderer> listKnifeReds;

	public List<Animator> listKnifeEffectObjects;

	[Header("Images")]
	public List<Sprite> listAttackSkillImages;

	public List<Sprite> listRainEffectImages;

	public Sprite rainBlackImage;

	public Sprite knifeSkillImage;

	private int knifeAnimatorEffectCount;

	private List<GameObject> listDropKnifes = new List<GameObject>();

	private GameScene gameSceneComponent;

	private Player playerComponent;

	private Enemy targetEnemy;

	private SpriteRenderer targetImage;

	private SpriteRenderer attackRedRenderer;

	private SpriteRenderer rainBlack;

	private SpriteRenderer rainEffect;

	private ObscuredInt level;

	private SoundManager soundManager;

	private PlayerManager playerManager;

	private UIControlManager uiControlManager;

	private void Start()
	{
		soundManager = Singleton<SoundManager>.Instance;
		playerManager = Singleton<PlayerManager>.Instance;
		uiControlManager = Singleton<UIControlManager>.Instance;
		gameSceneComponent = gameScene.GetComponent<GameScene>();
		playerComponent = player.GetComponent<Player>();
		attackRedRenderer = attackRed.GetComponent<SpriteRenderer>();
		level = Singleton<DataManager>.Instance.selectLevel;
	}

	public void settingEnemy(Enemy t)
	{
		targetEnemy = t;
	}

	public void startSkill(float delay)
	{
		StartCoroutine(updateSkill(delay));
		StartCoroutine(updateKnife());
	}

	private IEnumerator updateSkill(float delay)
	{
		GameObject gameObject = new GameObject();
		gameObject.transform.parent = targetEnemy.transform;
		gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
		gameObject.transform.localScale = new Vector3(2f, 2f, 2f);
		targetImage = gameObject.AddComponent<SpriteRenderer>();
		targetImage.sortingLayerName = "Background";
		targetImage.enabled = false;
		GameObject gameObject2 = new GameObject();
		gameObject2.transform.parent = targetEnemy.transform;
		gameObject2.transform.localPosition = new Vector3(1.533f, 6.397f, 0f);
		rainBlack = gameObject2.AddComponent<SpriteRenderer>();
		rainBlack.sprite = rainBlackImage;
		rainBlack.color = new Color(1f, 1f, 1f, 0f);
		GameObject gameObject3 = new GameObject();
		gameObject3.transform.parent = targetEnemy.transform;
		gameObject3.transform.localPosition = new Vector3(1.592f, 3.457f, 0f);
		rainEffect = gameObject3.AddComponent<SpriteRenderer>();
		rainEffect.enabled = false;
		attackRed.transform.parent = targetEnemy.transform;
		attackRed.transform.localPosition = new Vector3(0f, 0f, 111f);
		yield return new WaitForSeconds(delay);
		while (!skillOutCheck())
		{
			float seconds;
			if (UnityEngine.Random.Range(0, 100) % 2 == 0)
			{
				StartCoroutine(dropKnifeSkill());
				seconds = 8f;
			}
			else
			{
				StartCoroutine(attackSkill());
				seconds = 5f;
			}
			yield return new WaitForSeconds(seconds);
		}
	}

	private bool crashCheck(float minX, float maxX)
	{
		float x = player.transform.position.x;
		if (minX <= x && x <= maxX)
		{
			return true;
		}
		return false;
	}

	private IEnumerator attackSkill()
	{
		bool ended = false;
		float frameTime = 0.1f;
		targetEnemy.getIdleObject().SetActive(value: false);
		targetImage.enabled = true;
		attackRedRenderer.DOFade(1f, 0.3f);
		int size = listAttackSkillImages.Count;
		for (int i = 0; i < size; i++)
		{
			if (skillOutCheck())
			{
				ended = true;
				break;
			}
			targetImage.sprite = listAttackSkillImages[i];
			if (i == 2)
			{
				soundManager.playSound("knight_attack_start");
			}
			if (i == 11)
			{
				soundManager.playSound("knight_attack");
				attackRedRenderer.DOFade(0f, 0.3f);
				checkAttackDamage();
				mainCamera.transform.DOShakePosition(0.4f, 1f, 40);
			}
			if (i == 10)
			{
				yield return new WaitForSeconds(frameTime * 4f);
			}
			else
			{
				yield return new WaitForSeconds(frameTime);
			}
		}
		if (!ended)
		{
			targetEnemy.getIdleObject().SetActive(value: true);
			targetImage.enabled = false;
		}
	}

	private void checkAttackDamage()
	{
		if (!targetEnemy.isLife())
		{
			return;
		}
		float x = targetEnemy.transform.position.x;
		if (crashCheck(x - 13.47f, x))
		{
			playerManager.addDamage(Balance.getKnightAttackPower(level));
			float gameUIHP = (float)playerManager.getPlayerNowHP() / (float)playerManager.getPlayerMaxHP();
			uiControlManager.setGameUIHP(gameUIHP);
			if (!playerManager.isLife())
			{
				gameSceneComponent.onGameOver();
			}
		}
	}

	private IEnumerator dropKnifeSkill()
	{
		knifeAnimatorEffectCount = 0;
		targetEnemy.getIdleObject().SetActive(value: false);
		targetImage.enabled = true;
		targetImage.sprite = knifeSkillImage;
		listDropKnifes.Clear();
		float x = player.transform.position.x;
		int count2 = listKnifeObjects.Count;
		for (int j = 0; j < count2; j++)
		{
			float x2 = UnityEngine.Random.Range(x - 7f, x + 7f);
			listKnifeObjects[j].transform.position = new Vector3(x2, 20f, 101f);
			listKnifeObjects[j].color = Color.white;
			listKnifeReds[j].transform.position = new Vector3(x2, -1.03f, -2f);
			listKnifeReds[j].color = new Color(1f, 1f, 1f, 0f);
			float num = (float)j * 0.5f;
			listKnifeReds[j].DOKill();
			listKnifeReds[j].DOFade(1f, 0.5f).SetDelay(num);
			listKnifeReds[j].DOFade(0f, 0.3f).SetDelay(num + 1.8f);
			listKnifeObjects[j].transform.DOKill();
			listKnifeObjects[j].transform.DOMoveY(-1.03f, 0.5f).SetDelay(num + 1.8f);
			listKnifeObjects[j].DOKill();
			listKnifeObjects[j].DOFade(0f, 0.5f).SetDelay(num + 2.3f);
			listDropKnifes.Add(listKnifeObjects[j].gameObject);
		}
		soundManager.playSound("knight_drop_start");
		bool ended = false;
		float frameTime = 0.05f;
		int count = listRainEffectImages.Count;
		float num2 = frameTime * (float)(count / 2);
		rainBlack.DOFade(1f, num2);
		rainBlack.DOFade(0f, num2).SetDelay(num2);
		rainEffect.enabled = true;
		for (int i = 0; i < count; i++)
		{
			if (skillOutCheck())
			{
				ended = true;
				break;
			}
			rainEffect.sprite = listRainEffectImages[i];
			yield return new WaitForSeconds(frameTime);
		}
		rainEffect.enabled = false;
		yield return new WaitForSeconds(0.2f);
		if (!ended)
		{
			targetEnemy.getIdleObject().SetActive(value: true);
			targetImage.enabled = false;
		}
	}

	private IEnumerator updateKnife()
	{
		while (!skillOutCheck() && playerManager.isLife())
		{
			int count = listDropKnifes.Count;
			for (int i = 0; i < count; i++)
			{
				if (!(listDropKnifes[i].transform.position.y < -1f))
				{
					continue;
				}
				float x = listDropKnifes[i].transform.position.x;
				if (crashCheck(x - 1.12f, x + 1.12f) && targetEnemy.isLife())
				{
					playerManager.addDamage(Balance.getKnightDropPower(level));
					float gameUIHP = (float)playerManager.getPlayerNowHP() / (float)playerManager.getPlayerMaxHP();
					uiControlManager.setGameUIHP(gameUIHP);
					if (!playerManager.isLife())
					{
						gameSceneComponent.onGameOver();
					}
				}
				listKnifeEffectObjects[knifeAnimatorEffectCount].transform.position = listDropKnifes[i].transform.position;
				listKnifeEffectObjects[knifeAnimatorEffectCount].Play(0);
				listDropKnifes.RemoveAt(i);
				mainCamera.transform.DOShakePosition(0.1f, 1f, 40);
				soundManager.playSound("knight_drop");
				break;
			}
			yield return null;
		}
	}

	private bool skillOutCheck()
	{
		if (targetEnemy.isLife())
		{
			return false;
		}
		targetImage.enabled = false;
		rainEffect.enabled = false;
		rainBlack.enabled = false;
		attackRed.SetActive(value: false);
		int count = listKnifeReds.Count;
		for (int i = 0; i < count; i++)
		{
			listKnifeReds[i].gameObject.SetActive(value: false);
		}
		return true;
	}
}
