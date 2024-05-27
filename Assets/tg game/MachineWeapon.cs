using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineWeapon : BaseSkill
{
	private GameObject laserFront;

	private SpriteRenderer laserFrontSprite;

	private GameObject laserLine;

	private SpriteRenderer laserLineSprite;

	private float attackTimer = 3f;

	private float addDamange = 0.5f;

	private float distance = 10f;

	public override void startSkill(Player p, GameScene s, int count)
	{
		base.startSkill(p, s, count);
		GameObject original = Singleton<AssetManager>.Instance.LoadObject("Effect/Skill/SkillSpriteRenderer");
		laserFront = Object.Instantiate(original);
		laserFront.transform.parent = base.transform;
		laserFrontSprite = laserFront.GetComponent<SpriteRenderer>();
		laserFrontSprite.sprite = Singleton<AssetManager>.Instance.LoadSprite("Effect/Skill/Machine/laser_front");
		laserFrontSprite.color = new Color(1f, 1f, 1f, 0f);
		laserLine = Object.Instantiate(original);
		laserLine.transform.parent = base.transform;
		laserLineSprite = laserLine.GetComponent<SpriteRenderer>();
		laserLineSprite.sprite = Singleton<AssetManager>.Instance.LoadSprite("Effect/Skill/Machine/laser_line");
		laserLineSprite.color = new Color(1f, 1f, 1f, 0f);
		StartCoroutine(updateTimer());
		StartCoroutine(updatePosition());
	}

	public override void callAttack(Enemy e)
	{
	}

	public override void callTimer()
	{
	}

	private IEnumerator updateTimer()
	{
		int damage = (int)((float)playerManager.getPowerOrigin() * addDamange);
		float laserScale = distance / 3.7f;
		while (true)
		{
			yield return new WaitForSeconds(attackTimer);
			laserFrontSprite.color = Color.white;
			laserLineSprite.color = Color.white;
			bool flipX = (player.getLastMoveDirection() < 0f) ? true : false;
			laserFrontSprite.flipX = flipX;
			laserLineSprite.flipX = flipX;
			laserFront.transform.localScale = new Vector3(0f, 0f, 0f);
			laserFront.transform.DOScale(new Vector3(1f, 1f, 1f), 0.1f);
			laserLine.transform.localScale = new Vector3(laserScale, 0f, 0f);
			laserLine.transform.DOScale(new Vector3(laserScale, 1f, 1f), 0.1f);
			Vector3 position = player.transform.position;
			Vector3 playerOldPos = position;
			playerOldPos.x += player.getLastMoveDirection() * (1.5f + distance);
			List<Enemy> list = enemyManager.crashOldPositionMultiple(position, playerOldPos);
			int count = list.Count;
			for (int i = 0; i < count; i++)
			{
				if (list[i].addDamage(damage, player.getLastMoveDirection()))
				{
					gameScene.enemyDie(list[i]);
				}
				damageManager.createActionDamage(damage, Color.white, list[i].transform.position);
			}
			yield return new WaitForSeconds(0.1f);
			laserLineSprite.DOFade(0f, 0.3f);
			laserFrontSprite.DOFade(0f, 0.3f);
		}
	}

	private IEnumerator updatePosition()
	{
		while (true)
		{
			Vector3 position = player.transform.position;
			position.x += player.getLastMoveDirection() * 1.5f;
			position.y += 1f;
			position.z = -8f;
			laserFront.transform.position = position;
			laserLine.transform.position = position;
			yield return null;
		}
	}
}
