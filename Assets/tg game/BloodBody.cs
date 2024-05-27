using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodBody : BaseSkill
{
	public GameObject bodyParticle;

	public ParticleSystem bodyParticleSystem;

	private Sprite skillArmorImage;

	private float distance = 4f;

	private float addDamage = 0.5f;

	private float minusHeal = 0.1f;

	private float time = 5f;

	public override void startSkill(Player p, GameScene s, int count)
	{
		base.startSkill(p, s, count);
		bodyParticle = Object.Instantiate(Singleton<AssetManager>.Instance.LoadObject("Effect/Skill/Blood/bloodBody"));
		bodyParticle.transform.parent = base.transform;
		bodyParticle.SetActive(value: false);
		bodyParticleSystem = bodyParticle.GetComponent<ParticleSystem>();
		skillArmorImage = Singleton<AssetManager>.Instance.LoadSprite("Effect/Skill/Blood/skill_body");
		player.getSkillSpriteArmor().sprite = skillArmorImage;
		StartCoroutine(updateSkill());
	}

	private IEnumerator updateSkill()
	{
		int damage = (int)((float)playerManager.getPowerOrigin() * addDamage);
		int minus = (int)((float)playerManager.getPlayerMaxHP() * minusHeal);
		yield return new WaitForSeconds(1f);
		player.getSkillSpriteArmor().sprite = skillArmorImage;
		while (true)
		{
			player.getSkillSpriteArmor().DOFade(1f, time);
			yield return new WaitForSeconds(time);
			player.getSkillSpriteArmor().color = new Color(1f, 1f, 1f, 0f);
			Vector3 position = player.transform.position;
			position.y += 0.5f;
			position.z = -8f;
			bodyParticle.transform.position = position;
			bodyParticle.SetActive(value: true);
			bodyParticleSystem.Play();
			List<Enemy> distanceEnemys = enemyManager.getDistanceEnemys(player.transform.position, distance);
			int count = distanceEnemys.Count;
			for (int i = 0; i < count; i++)
			{
				float direction = (player.transform.position.x < distanceEnemys[i].transform.position.x) ? 1 : (-1);
				if (distanceEnemys[i].addDamage(damage, direction))
				{
					gameScene.enemyDie(distanceEnemys[i]);
				}
				damageManager.createActionDamage(damage, Color.white, distanceEnemys[i].transform.position);
			}
			playerManager.addDamage(minus);
			float gameUIHP = (float)playerManager.getPlayerNowHP() / (float)playerManager.getPlayerMaxHP();
			Singleton<UIControlManager>.Instance.setGameUIHP(gameUIHP);
			if (!playerManager.isLife())
			{
				gameScene.onGameOver();
			}
			yield return new WaitForSeconds(1f);
			bodyParticle.SetActive(value: false);
			yield return null;
		}
	}

	public override void callAttack(Enemy e)
	{
	}

	public override void callTimer()
	{
	}
}
