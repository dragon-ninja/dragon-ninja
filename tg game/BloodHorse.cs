using DG.Tweening;
using System.Collections;
using UnityEngine;

public class BloodHorse : BaseSkill
{
	private GameObject particleObject;

	private ParticleSystem particleObjectComponet;

	private Sprite skillHorseImage;

	public const float addSpeed = 0.1f;

	private float time = 5f;

	private float minusHeal = 0.1f;

	public override void startSkill(Player p, GameScene s, int count)
	{
		base.startSkill(p, s, count);
		particleObject = Object.Instantiate(Singleton<AssetManager>.Instance.LoadObject("Effect/Skill/Blood/bloodHorse"));
		particleObject.transform.parent = base.transform;
		particleObject.SetActive(value: false);
		particleObjectComponet = particleObject.GetComponent<ParticleSystem>();
		skillHorseImage = Singleton<AssetManager>.Instance.LoadSprite("Effect/Skill/Blood/skill_horse");
		player.getSkillSpriteHorse().sprite = skillHorseImage;
		StartCoroutine(updateSkill());
	}

	private IEnumerator updateSkill()
	{
		Vector3 position = player.transform.position;
		int minus = (int)((float)playerManager.getPlayerMaxHP() * minusHeal);
		yield return new WaitForSeconds(1f);
		player.getSkillSpriteHorse().sprite = skillHorseImage;
		while (true)
		{
			player.getSkillSpriteHorse().DOFade(1f, time);
			yield return new WaitForSeconds(time);
			player.getSkillSpriteHorse().color = new Color(1f, 1f, 1f, 0f);
			position = player.transform.position;
			position.y += 1f;
			particleObject.transform.position = position;
			particleObject.transform.rotation = Quaternion.Euler(181.814f, 89.99999f * player.getLastMoveDirection(), -90f);
			particleObject.SetActive(value: true);
			particleObjectComponet.Play();
			playerManager.addDamage(minus);
			float gameUIHP = (float)playerManager.getPlayerNowHP() / (float)playerManager.getPlayerMaxHP();
			Singleton<UIControlManager>.Instance.setGameUIHP(gameUIHP);
			if (!playerManager.isLife())
			{
				gameScene.onGameOver();
			}
		}
	}

	public override void callAttack(Enemy e)
	{
	}

	public override void callTimer()
	{
	}
}
