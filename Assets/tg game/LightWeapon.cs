using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class LightWeapon : BaseSkill
{
	private List<SpriteRenderer> listCircleImages = new List<SpriteRenderer>();

	private List<SpriteRenderer> listLineImages = new List<SpriteRenderer>();

	private List<SpriteRenderer> activeImages = new List<SpriteRenderer>();

	private float addPower;

	public override void startSkill(Player p, GameScene s, int count)
	{
		base.startSkill(p, s, count);
		addPower = SkillData.LightningSkillPower(count);
		GameObject original = Singleton<AssetManager>.Instance.LoadObject("Effect/Skill/SkillSpriteRenderer");
		for (int i = 0; i < 5; i++)
		{
			GameObject gameObject = Object.Instantiate(original);
			gameObject.SetActive(value: false);
			gameObject.transform.parent = base.transform;
			SpriteRenderer component = gameObject.GetComponent<SpriteRenderer>();
			if (i >= 3)
			{
				component.sprite = Singleton<AssetManager>.Instance.LoadSprite("Effect/Skill/Lightning/attack_line");
				listLineImages.Add(component);
			}
			else
			{
				component.sprite = Singleton<AssetManager>.Instance.LoadSprite("Effect/Skill/Lightning/attack_circle");
				listCircleImages.Add(component);
			}
		}
	}

	public override void callAttack(Enemy e)
	{
		for (int i = 0; i < 3; i++)
		{
			listCircleImages[i].color = new Color(1f, 1f, 1f, 0f);
			listCircleImages[i].gameObject.SetActive(value: false);
			listCircleImages[i].DOKill();
		}
		for (int j = 0; j < 2; j++)
		{
			listLineImages[j].color = new Color(1f, 1f, 1f, 0f);
			listLineImages[j].gameObject.SetActive(value: false);
			listLineImages[j].DOKill();
		}
		float direction = player.getLastMoveDirection();
		activeImages.Clear();
		int num = (int)((float)playerManager.getPowerOrigin() * addPower);
		List<Enemy> distanceEnemys = enemyManager.getDistanceEnemys(player.transform.position, 16f);
		distanceEnemys.Sort((Enemy a, Enemy b) => (a.transform.position.x < b.transform.position.x) ? (-1 * (int)direction) : ((int)direction));
		distanceEnemys.RemoveAt(0);
		listCircleImages[0].transform.position = e.transform.position + new Vector3(0.5f * player.getLastMoveDirection(), 0.8f, -9f);
		activeImages.Add(listCircleImages[0]);
		Vector3 position = new Vector3(0f, 0f, 0f);
		for (int k = 0; k < 2; k++)
		{
			if (distanceEnemys.Count >= k + 1)
			{
				position = distanceEnemys[k].transform.position;
				position.x += 0.5f * player.getLastMoveDirection();
				position.y += 0.8f;
				position.z = -9f;
				listCircleImages[k + 1].transform.position = position;
				float num2 = listCircleImages[k + 1].transform.position.x - listCircleImages[k].transform.position.x;
				float x = num2 / 3f * direction;
				position.x -= num2 / 2f;
				position.z = -8f;
				listLineImages[k].transform.position = position;
				listLineImages[k].transform.localScale = new Vector3(x, 1f, 1f);
				activeImages.Add(listLineImages[k]);
				activeImages.Add(listCircleImages[k + 1]);
				if (distanceEnemys[k].addDamage(num, player.getLastMoveDirection()))
				{
					gameScene.enemyDie(distanceEnemys[k]);
				}
				damageManager.createActionDamage(num, Color.white, distanceEnemys[k].transform.position);
			}
		}
		int count = activeImages.Count;
		for (int l = 0; l < count; l++)
		{
			activeImages[l].gameObject.SetActive(value: true);
			activeImages[l].color = new Color(1f, 1f, 1f, 1f);
			activeImages[l].DOFade(0f, 1f).SetDelay(0.5f);
		}
		soundManager.playSound("Skill_Lightning");
	}

	public override void callTimer()
	{
	}
}
