using System.Collections;
using UnityEngine;

public class FireWeapon : BaseSkill
{
	private GameObject fireParticle;

	private ParticleSystem fireParticleSystem;

	private Enemy enemy;

	private float timer;

	public override void startSkill(Player p, GameScene s, int count)
	{
		base.startSkill(p, s, count);
		fireParticle = Object.Instantiate(Singleton<AssetManager>.Instance.LoadObject("Effect/Skill/Fire/fireweapon"));
		fireParticle.transform.parent = base.transform;
		fireParticle.SetActive(value: false);
		fireParticleSystem = fireParticle.GetComponent<ParticleSystem>();
		StartCoroutine(updateSkill());
	}

	public override void callAttack(Enemy e)
	{
		enemy = e;
		fireParticle.SetActive(value: true);
		Vector3 position = enemy.transform.position;
		position.z = -8f;
		fireParticle.transform.position = position;
		fireParticleSystem.Play();
	}

	public override void callTimer()
	{
	}

	private IEnumerator updateSkill()
	{
		while (true)
		{
			if (enemy != null)
			{
				if (enemy.isLife())
				{
					timer += Time.deltaTime;
					fireParticle.transform.position = enemy.transform.position;
					if (timer > 1f)
					{
						timer = 0f;
						if (enemy.addDamage(10, 0f))
						{
							gameScene.enemyDie(enemy);
						}
					}
				}
				else
				{
					enemy = null;
					fireParticle.SetActive(value: false);
				}
			}
			yield return null;
		}
	}
}
