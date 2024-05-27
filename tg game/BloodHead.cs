using DG.Tweening;
using System.Collections;
using UnityEngine;

public class BloodHead : BaseSkill
{
	private GameObject attackTop;

	private SpriteRenderer attackTopSprite;

	private GameObject attackBot;

	private SpriteRenderer attackBotSprite;

	private float addDamage = 0.5f;

	private float heal = 0.1f;

	private float time = 3f;

	private float distance = 2f;

	public override void startSkill(Player p, GameScene s, int count)
	{
		base.startSkill(p, s, count);
		GameObject original = Singleton<AssetManager>.Instance.LoadObject("Effect/Skill/SkillSpriteRenderer");
		attackTop = Object.Instantiate(original);
		attackTop.SetActive(value: false);
		attackTop.transform.parent = base.transform;
		attackTopSprite = attackTop.GetComponent<SpriteRenderer>();
		attackTopSprite.sprite = Singleton<AssetManager>.Instance.LoadSprite("Effect/Skill/Blood/attack1");
		attackBot = Object.Instantiate(original);
		attackBot.SetActive(value: false);
		attackBot.transform.parent = base.transform;
		attackBotSprite = attackBot.GetComponent<SpriteRenderer>();
		attackBotSprite.sprite = Singleton<AssetManager>.Instance.LoadSprite("Effect/Skill/Blood/attack2");
		StartCoroutine(updateSkill());
	}

	private IEnumerator updateSkill()
	{
		Vector3 position = player.transform.position;
		int damage = (int)((float)playerManager.getPowerOrigin() * addDamage);
		int addHp = (int)((float)playerManager.getPlayerMaxHP() * heal);
		while (true)
		{
			yield return new WaitForSeconds(time);
			position = player.transform.position;
			position.x += distance * player.getLastMoveDirection();
			position.y += 1f;
			Enemy enemy = enemyManager.crashCheck(position, 1f);
			if ((bool)enemy)
			{
				if (enemy.addDamage(damage, 0f))
				{
					gameScene.enemyDie(enemy);
				}
				damageManager.createActionDamage(damage, Color.white, enemy.transform.position);
				playerManager.addHeal(addHp);
				float gameUIHP = (float)playerManager.getPlayerNowHP() / (float)playerManager.getPlayerMaxHP();
				Singleton<UIControlManager>.Instance.setGameUIHP(gameUIHP);
			}
			attackTopSprite.DOFade(1f, 0.2f);
			attackBotSprite.DOFade(1f, 0.2f);
			attackTop.SetActive(value: true);
			attackBot.SetActive(value: true);
			attackTop.transform.position = new Vector3(position.x, position.y + 0.5f, position.z);
			attackBot.transform.position = new Vector3(position.x, position.y - 0.5f, position.z);
			attackTop.transform.DOMove(position, 0.5f).SetEase(Ease.InExpo);
			attackBot.transform.DOMove(position, 0.5f).SetEase(Ease.InExpo);
			yield return new WaitForSeconds(0.5f);
			attackTopSprite.DOFade(0f, 0.5f);
			attackBotSprite.DOFade(0f, 0.5f);
			yield return new WaitForSeconds(0.5f);
		}
	}

	public override void callAttack(Enemy e)
	{
	}

	public override void callTimer()
	{
	}
}
