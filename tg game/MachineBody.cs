using System.Collections;
using UnityEngine;

public class MachineBody : BaseSkill
{
	private GameObject turret;

	private SpriteRenderer turretSprite;

	private GameObject bullet;

	private SpriteRenderer bulletSprite;

	private float time = 5f;

	private float addDamage = 0.5f;

	private float bulletSpeed = 20f;

	public override void startSkill(Player p, GameScene s, int count)
	{
		base.startSkill(p, s, count);
		GameObject original = Singleton<AssetManager>.Instance.LoadObject("Effect/Skill/SkillSpriteRenderer");
		turret = Object.Instantiate(original);
		turret.transform.parent = base.transform;
		turretSprite = turret.GetComponent<SpriteRenderer>();
		turretSprite.sprite = Singleton<AssetManager>.Instance.LoadSprite("Effect/Skill/Machine/turret");
		bullet = Object.Instantiate(original);
		bullet.transform.parent = base.transform;
		bullet.SetActive(value: false);
		bulletSprite = bullet.GetComponent<SpriteRenderer>();
		bulletSprite.sprite = Singleton<AssetManager>.Instance.LoadSprite("Effect/Skill/Machine/turret_bullet");
		StartCoroutine(updateTurretPosition());
		StartCoroutine(updateFire());
	}

	private IEnumerator updateTurretPosition()
	{
		Vector3 position = player.transform.position;
		while (true)
		{
			float num = player.getLastMoveDirection();
			if (num <= -0.1f && 0.1f <= num)
			{
				num = 1f;
			}
			position = player.transform.position;
			position.x -= 1f * num;
			position.y += 3f;
			turret.transform.localScale = new Vector3(num, 1f, 1f);
			turret.transform.position = position;
			yield return null;
		}
	}

	private IEnumerator updateFire()
	{
		int damage = (int)((float)playerManager.getPowerOrigin() * addDamage);
		while (true)
		{
			yield return new WaitForSeconds(time);
			float direction = player.getLastMoveDirection();
			if (-0.1f <= direction && direction <= 0.1f)
			{
				direction = 1f;
			}
			bullet.SetActive(value: true);
			Vector3 position = turret.transform.position;
			position.x += 0.8f * direction;
			position.y += -0.05f;
			Vector3 angleVector = new Vector3(Mathf.Sin(1.83259571f * direction), Mathf.Cos(1.83259571f * direction), 0f);
			angleVector.Normalize();
			bullet.transform.position = position;
			bullet.transform.rotation = Quaternion.Euler(0f, 0f, -105f * direction);
			while (true)
			{
				bullet.transform.position += angleVector * Time.deltaTime * bulletSpeed;
				Enemy vectorDistanceEnemy = enemyManager.getVectorDistanceEnemy(bullet.transform.position, 2f);
				if ((bool)vectorDistanceEnemy)
				{
					if (vectorDistanceEnemy.addDamage(damage, direction))
					{
						gameScene.enemyDie(vectorDistanceEnemy);
					}
					damageManager.createActionDamage(damage, Color.white, vectorDistanceEnemy.transform.position);
					break;
				}
				if (bullet.transform.position.y <= -1.05f)
				{
					break;
				}
				yield return null;
			}
			bullet.SetActive(value: false);
		}
	}

	public override void callAttack(Enemy e)
	{
	}

	public override void callTimer()
	{
	}
}
