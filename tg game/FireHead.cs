using DG.Tweening;
using System.Collections;
using UnityEngine;

public class FireHead : BaseSkill
{
	private GameObject fireParticle;

	private ParticleSystem fireParticleSystem;

	private Sprite skillHeadImage;

	private float delayTime = 5f;

	private float activeTime = 5f;

	private int addPower = 10;

	public override void startSkill(Player p, GameScene s, int count)
	{
		base.startSkill(p, s, count);
		fireParticle = Object.Instantiate(Singleton<AssetManager>.Instance.LoadObject("Effect/Skill/Fire/firehead"));
		fireParticle.transform.parent = base.transform;
		fireParticle.SetActive(value: false);
		fireParticleSystem = fireParticle.GetComponent<ParticleSystem>();
		skillHeadImage = Singleton<AssetManager>.Instance.LoadSprite("Effect/Skill/Fire/skill_head");
		player.getSkillSpriteHead().sprite = skillHeadImage;
		StartCoroutine(updateSkill());
		StartCoroutine(positionUpdate());
	}

	public override void callAttack(Enemy e)
	{
	}

	private IEnumerator updateSkill()
	{
		while (true)
		{
			yield return new WaitForSeconds(delayTime);
			player.getSkillSpriteHead().sprite = skillHeadImage;
			power = addPower;
			player.getSkillSpriteHead().DOFade(1f, 0.3f);
			fireParticle.transform.localScale = new Vector3(1f, 1f, 1f);
			fireParticle.SetActive(value: true);
			fireParticleSystem.Play();
			yield return new WaitForSeconds(activeTime);
			power = 0;
			player.getSkillSpriteHead().DOFade(0f, 0.3f);
			fireParticle.transform.DOScale(new Vector3(0f, 0f, 0f), 0.3f).OnComplete(delegate
			{
				fireParticleSystem.Stop();
				fireParticle.SetActive(value: false);
			});
			yield return null;
		}
	}

	private IEnumerator positionUpdate()
	{
		Vector3 position = player.transform.position;
		while (true)
		{
			position = player.transform.position;
			position.z = -8f;
			fireParticle.transform.position = position;
			yield return null;
		}
	}
}
