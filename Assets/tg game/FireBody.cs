using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBody : BaseSkill
{
	private GameObject fireParticle;

	private ParticleSystem fireParticleSystem;

	private Sprite skillArmorImage;

	private float delayTime = 10f;

	private float distance = 4f;

	private float addPower = 0.5f;

	public override void startSkill(Player p, GameScene s, int count)
	{
		base.startSkill(p, s, count);
		fireParticle = Object.Instantiate(Singleton<AssetManager>.Instance.LoadObject("Effect/Skill/Fire/firebody"));
		fireParticle.transform.parent = base.transform;
		fireParticle.SetActive(value: false);
		fireParticleSystem = fireParticle.GetComponent<ParticleSystem>();
		skillArmorImage = Singleton<AssetManager>.Instance.LoadSprite("Effect/Skill/Fire/skill_body");
		player.getSkillSpriteArmor().sprite = skillArmorImage;
		StartCoroutine(updateSkill());
	}

	public override void callAttack(Enemy e)
	{
	}

	private IEnumerator updateSkill()
	{
		int damage = (int)((float)playerManager.getPowerOrigin() + (float)playerManager.getPowerOrigin() * addPower);
		yield return new WaitForSeconds(1f);
		player.getSkillSpriteArmor().sprite = skillArmorImage;
		while (true)
		{
			player.getSkillSpriteArmor().DOFade(1f, delayTime);
			yield return new WaitForSeconds(delayTime);
			player.getSkillSpriteArmor().color = new Color(1f, 1f, 1f, 0f);
			Vector3 position = player.transform.position;
			position.y += 0.5f;
			position.z = -8f;
			fireParticle.transform.position = position;
			fireParticle.SetActive(value: true);
			fireParticleSystem.Play();
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
			yield return new WaitForSeconds(1f);
			fireParticle.SetActive(value: false);
			yield return null;
		}
	}
}
