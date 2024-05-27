using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceHorse : BaseSkill
{
	private List<SpriteRenderer> listWalkings = new List<SpriteRenderer>();

	public const float minusSpeed = 0.1f;

	private float addShield = 0.5f;

	private int maxWalking = 20;

	private int nowWalking;

	private float walkingTime = 0.1f;

	private float fadeOutTime = 1f;

	public override void startSkill(Player p, GameScene s, int count)
	{
		base.startSkill(p, s, count);
		GameObject original = Singleton<AssetManager>.Instance.LoadObject("Effect/Skill/SkillSpriteRenderer");
		for (int i = 0; i < maxWalking; i++)
		{
			GameObject gameObject = Object.Instantiate(original);
			gameObject.transform.parent = base.transform;
			SpriteRenderer component = gameObject.GetComponent<SpriteRenderer>();
			string str = (i % 2 == 0) ? "2" : "1";
			component.sprite = Singleton<AssetManager>.Instance.LoadSprite("Effect/Skill/Ice/walking" + str);
			component.color = new Color(1f, 1f, 1f, 0f);
			listWalkings.Add(component);
		}
		StartCoroutine(activeShield());
		StartCoroutine(updateSkill());
	}

	private IEnumerator activeShield()
	{
		yield return new WaitForSeconds(0.1f);
		UnityEngine.Debug.Log("s1 : " + playerManager.getShield());
		shield = (int)((float)playerManager.getShieldOrigin() * addShield);
		gameScene.refreshGameUI();
		UnityEngine.Debug.Log("s2 : " + playerManager.getShield());
	}

	private IEnumerator updateSkill()
	{
		while (true)
		{
			yield return new WaitForSeconds(walkingTime);
			listWalkings[nowWalking].transform.position = player.transform.position;
			listWalkings[nowWalking].color = new Color(1f, 1f, 1f, 1f);
			listWalkings[nowWalking].DOFade(0f, fadeOutTime);
			nowWalking++;
			if (nowWalking >= maxWalking)
			{
				nowWalking = 0;
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
