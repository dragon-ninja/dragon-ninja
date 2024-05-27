using System.Collections;
using UnityEngine;

public class MachineHorse : BaseSkill
{
	private GameObject healObject;

	private ParticleSystem healParticle;

	private float heal = 0.1f;

	private float time = 5f;

	public override void startSkill(Player p, GameScene s, int count)
	{
		base.startSkill(p, s, count);
		healObject = Object.Instantiate(Singleton<AssetManager>.Instance.LoadObject("Effect/Skill/Machine/machineHorse"));
		healObject.SetActive(value: false);
		healObject.transform.parent = base.transform;
		healParticle = healObject.GetComponent<ParticleSystem>();
		StartCoroutine(updatePosition());
		StartCoroutine(updateHeal());
	}

	private IEnumerator updatePosition()
	{
		while (true)
		{
			healObject.transform.position = player.transform.position;
			yield return null;
		}
	}

	private IEnumerator updateHeal()
	{
		int addHp = (int)((float)playerManager.getPlayerMaxHP() * heal);
		while (true)
		{
			yield return new WaitForSeconds(time);
			playerManager.addHeal(addHp);
			float gameUIHP = (float)playerManager.getPlayerNowHP() / (float)playerManager.getPlayerMaxHP();
			Singleton<UIControlManager>.Instance.setGameUIHP(gameUIHP);
			healObject.SetActive(value: true);
			healParticle.Play();
		}
	}

	public override void callAttack(Enemy e)
	{
	}

	public override void callTimer()
	{
	}
}
