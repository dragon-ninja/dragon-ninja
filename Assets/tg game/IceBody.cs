using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBody : BaseSkill
{
	private GameObject particleObject;

	private ParticleSystem particleObjectComponent;

	private float distance = 4f;

	private float addPower = 0.5f;

	private float time = 1f;

	private bool state;

	private float nowTime;

	public override void startSkill(Player p, GameScene s, int count)
	{
		base.startSkill(p, s, count);
		particleObject = Object.Instantiate(Singleton<AssetManager>.Instance.LoadObject("Effect/Skill/Ice/iceBody"));
		particleObject.transform.parent = base.transform;
		particleObject.SetActive(value: false);
		particleObjectComponent = particleObject.GetComponent<ParticleSystem>();
		StartCoroutine(updatePosition());
		StartCoroutine(updateTime());
	}

	private IEnumerator updatePosition()
	{
		while (true)
		{
			particleObject.transform.position = player.transform.position;
			yield return null;
		}
	}

	private IEnumerator updateTime()
	{
		while (true)
		{
			if (!state)
			{
				nowTime += Time.deltaTime;
				if (nowTime >= time)
				{
					nowTime = 0f;
					state = true;
				}
			}
			yield return null;
		}
	}

	public override void callAttack(Enemy e)
	{
		if (!state)
		{
			return;
		}
		int num = (int)((float)playerManager.getPowerOrigin() + (float)playerManager.getPowerOrigin() * addPower);
		Vector3 position = player.transform.position;
		position.y += 0.5f;
		position.z = -8f;
		particleObject.transform.position = position;
		particleObject.SetActive(value: true);
		particleObjectComponent.Play();
		List<Enemy> distanceEnemys = enemyManager.getDistanceEnemys(player.transform.position, distance);
		int count = distanceEnemys.Count;
		for (int i = 0; i < count; i++)
		{
			float direction = (player.transform.position.x < distanceEnemys[i].transform.position.x) ? 1 : (-1);
			if (distanceEnemys[i].addDamage(num, direction))
			{
				gameScene.enemyDie(distanceEnemys[i]);
			}
			damageManager.createActionDamage(num, Color.white, distanceEnemys[i].transform.position);
		}
		state = false;
	}

	public override void callTimer()
	{
	}
}
