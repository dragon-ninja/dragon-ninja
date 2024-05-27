using System.Collections;
using UnityEngine;

public class BloodWeapon : BaseSkill
{
	private GameObject particleObject;

	private ParticleSystem particleObjectComponent;

	private float heal;

	private float time;

	private bool state;

	private float nowTime;

	public override void startSkill(Player p, GameScene s, int count)
	{
		base.startSkill(p, s, count);
		heal = SkillData.BloodSkillHeal(count);
		time = SkillData.BloodSkillDelayTime(count);
		particleObject = Object.Instantiate(Singleton<AssetManager>.Instance.LoadObject("Effect/Skill/Blood/bloodSword"));
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
		if (state)
		{
			playerManager.addHeal((int)((float)playerManager.getPlayerMaxHP() * heal));
			float gameUIHP = (float)playerManager.getPlayerNowHP() / (float)playerManager.getPlayerMaxHP();
			Singleton<UIControlManager>.Instance.setGameUIHP(gameUIHP);
			particleObject.SetActive(value: true);
			particleObjectComponent.Play();
			state = false;
		}
	}

	public override void callTimer()
	{
	}
}
