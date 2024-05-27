using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineHead : BaseSkill
{
	private GameObject missileObject;

	private SpriteRenderer missileSprite;

	private GameObject tailObject;

	private ParticleSystem tailParticle;

	private GameObject missileParticleObject;

	private ParticleSystem missileParticle;

	private float addDamage = 0.5f;

	private float missileTime = 5f;

	private float missileDistance = 6f;

	private float missileMoveX = 6f;

	public override void startSkill(Player p, GameScene s, int count)
	{
		base.startSkill(p, s, count);
		GameObject original = Singleton<AssetManager>.Instance.LoadObject("Effect/Skill/SkillSpriteRenderer");
		missileObject = Object.Instantiate(original);
		missileObject.SetActive(value: false);
		missileObject.transform.parent = base.transform;
		missileSprite = missileObject.GetComponent<SpriteRenderer>();
		missileSprite.sprite = Singleton<AssetManager>.Instance.LoadSprite("Effect/Skill/Machine/missile");
		tailObject = Object.Instantiate(Singleton<AssetManager>.Instance.LoadObject("Effect/Skill/Machine/machineMissileTail"));
		tailObject.SetActive(value: false);
		tailObject.transform.parent = missileObject.transform;
		tailParticle = tailObject.GetComponent<ParticleSystem>();
		missileParticleObject = Object.Instantiate(Singleton<AssetManager>.Instance.LoadObject("Effect/Skill/Machine/machineHack"));
		missileParticleObject.SetActive(value: false);
		missileParticleObject.transform.parent = base.transform;
		missileParticle = missileParticleObject.GetComponent<ParticleSystem>();
		StartCoroutine(updateMissile());
	}

	public override void callAttack(Enemy e)
	{
	}

	public override void callTimer()
	{
	}

	private IEnumerator updateMissile()
	{
		int damage = (int)((float)playerManager.getPowerOrigin() * addDamage);
		while (true)
		{
			yield return new WaitForSeconds(missileTime);
			missileObject.SetActive(value: true);
			missileObject.transform.position = player.transform.position;
			missileSprite.flipY = false;
			missileObject.transform.DOMoveY(14f, 1f).SetEase(Ease.Flash);
			tailObject.SetActive(value: true);
			tailObject.transform.localPosition = new Vector3(0f, -1f, 0f);
			tailObject.transform.rotation = Quaternion.Euler(-240f, 0f, 0f);
			yield return new WaitForSeconds(1f);
			Vector3 position = player.transform.position;
			position.x += missileMoveX * player.getLastMoveDirection();
			position.y = missileObject.transform.position.y;
			missileObject.transform.position = position;
			missileSprite.flipY = true;
			missileObject.transform.DOMoveY(-1f, 1f).SetEase(Ease.Flash);
			tailObject.transform.localPosition = new Vector3(0f, 1f, 0f);
			tailObject.transform.rotation = Quaternion.Euler(-120f, 0f, 180f);
			yield return new WaitForSeconds(1f);
			Vector3 position2 = missileObject.transform.position;
			missileObject.SetActive(value: false);
			tailObject.SetActive(value: false);
			missileParticleObject.SetActive(value: true);
			missileParticleObject.transform.position = position2;
			missileParticle.Play();
			List<Enemy> distanceEnemys = enemyManager.getDistanceEnemys(position2, 6f);
			int count = distanceEnemys.Count;
			for (int i = 0; i < count; i++)
			{
				if (distanceEnemys[i].addDamage(damage, 0f))
				{
					gameScene.enemyDie(distanceEnemys[i]);
				}
				damageManager.createActionDamage(damage, Color.white, distanceEnemys[i].transform.position);
			}
			yield return new WaitForSeconds(1f);
			missileParticleObject.SetActive(value: false);
		}
	}
}
