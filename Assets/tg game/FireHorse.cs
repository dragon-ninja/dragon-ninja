using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireHorse : BaseSkill
{
	private GameObject particle;

	private List<Vector3> listVectors = new List<Vector3>();

	private int max = 60;

	private int now;

	private float createTime = 0.1f;

	private float continuousTime = 5f;

	private float outTimer;

	private float addPower;

	public override void startSkill(Player p, GameScene s, int count)
	{
		base.startSkill(p, s, count);
		addPower = SkillData.FireSkillPower(count);
		GameObject original = Singleton<AssetManager>.Instance.LoadObject("Effect/Skill/Fire/firehorse");
		particle = Object.Instantiate(original);
		particle.transform.parent = base.transform;
		StartCoroutine(updateParticlePosition());
		StartCoroutine(updateSkill());
		StartCoroutine(updateSkillDelete());
		StartCoroutine(updateDamage());
	}

	public override void callAttack(Enemy e)
	{
	}

	public override void callTimer()
	{
	}

	private IEnumerator updateParticlePosition()
	{
		while (true)
		{
			particle.transform.position = player.transform.position;
			yield return null;
		}
	}

	private IEnumerator updateSkill()
	{
		while (true)
		{
			yield return new WaitForSeconds(createTime);
			listVectors.Add(particle.transform.position);
		}
	}

	private IEnumerator updateSkillDelete()
	{
		while (true)
		{
			if (listVectors.Count > 0)
			{
				outTimer += Time.deltaTime;
				if (outTimer >= continuousTime)
				{
					outTimer = continuousTime - createTime;
					listVectors.RemoveAt(0);
				}
			}
			yield return null;
		}
	}

	private IEnumerator updateDamage()
	{
		int damage = (int)((float)playerManager.getPowerOrigin() * addPower);
		UnityEngine.Debug.Log("power : " + addPower + ", damage : " + damage);
		while (true)
		{
			yield return new WaitForSeconds(1f);
			List<Enemy> distanceEnemys = enemyManager.getDistanceEnemys(listVectors, 2f);
			int count = distanceEnemys.Count;
			for (int i = 0; i < count; i++)
			{
				if (distanceEnemys[i].addDamage(damage, 0f))
				{
					gameScene.enemyDie(distanceEnemys[i]);
				}
				damageManager.createActionDamage(damage, Color.white, distanceEnemys[i].transform.position);
			}
		}
	}
}
