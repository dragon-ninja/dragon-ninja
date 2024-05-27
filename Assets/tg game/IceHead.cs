using DG.Tweening;
using System.Collections;
using UnityEngine;

public class IceHead : BaseSkill
{
	private GameObject particleObject;

	private ParticleSystem particleSystemComponet;

	private Sprite skillHeadImage;

	private float delayTime;

	private float activeTime;

	private float addPower;

	public override void startSkill(Player p, GameScene s, int count)
	{
		base.startSkill(p, s, count);
		delayTime = SkillData.MachineSkillDelayTime(count);
		activeTime = SkillData.MachineSkillActiveTime(count);
		addPower = SkillData.MachineSkillPowerUp(count);
		particleObject = Object.Instantiate(Singleton<AssetManager>.Instance.LoadObject("Effect/Skill/Ice/iceHead"));
		particleObject.transform.parent = base.transform;
		particleObject.SetActive(value: false);
		particleSystemComponet = particleObject.GetComponent<ParticleSystem>();
		skillHeadImage = Singleton<AssetManager>.Instance.LoadSprite("Effect/Skill/Machine/skill_weapon");
		player.getSkillSpriteWeapon().sprite = skillHeadImage;
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
			soundManager.playSound("Skill_Machine");
			power = (int)((float)playerManager.getPowerOrigin() * addPower);
			player.getSkillSpriteWeapon().sprite = skillHeadImage;
			player.getSkillSpriteWeapon().DOFade(1f, 0.3f);
			particleObject.transform.localScale = new Vector3(1f, 1f, 1f);
			particleObject.SetActive(value: true);
			particleSystemComponet.Play();
			gameScene.refreshGameUI();
			yield return new WaitForSeconds(activeTime);
			power = 0;
			player.getSkillSpriteWeapon().DOFade(0f, 0.3f);
			particleObject.transform.DOScale(new Vector3(0f, 0f, 0f), 0.3f).OnComplete(delegate
			{
				particleSystemComponet.Stop();
				particleObject.SetActive(value: false);
			});
			gameScene.refreshGameUI();
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
			particleObject.transform.position = position;
			yield return null;
		}
	}
}
