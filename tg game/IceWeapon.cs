using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceWeapon : BaseSkill
{
	public List<IceDebuff> listDebuffs = new List<IceDebuff>();

	private int nowListIndex;

	private int getListIndex;

	private float debuff;

	public override void startSkill(Player p, GameScene s, int count)
	{
		base.startSkill(p, s, count);
		debuff = SkillData.IceSkillEnemyDebuff(count);
		GameObject original = Singleton<AssetManager>.Instance.LoadObject("Effect/Skill/SkillSpriteRenderer");
		IceDebuff item = default(IceDebuff);
		for (int i = 0; i < 5; i++)
		{
			GameObject gameObject = Object.Instantiate(original);
			gameObject.transform.parent = base.transform;
			gameObject.SetActive(value: false);
			SpriteRenderer component = gameObject.GetComponent<SpriteRenderer>();
			component.sprite = Singleton<AssetManager>.Instance.LoadSprite("Effect/Skill/Ice/ice");
			item.obj = gameObject;
			item.image = component;
			item.target = null;
			listDebuffs.Add(item);
		}
		StartCoroutine(updateSkill());
	}

	private IceDebuff getIceDebuff()
	{
		for (int i = 0; i < 5; i++)
		{
			if (listDebuffs[nowListIndex].obj.activeSelf)
			{
				nowListIndex++;
				if (nowListIndex >= 5)
				{
					nowListIndex = 0;
				}
				continue;
			}
			getListIndex = nowListIndex;
			return listDebuffs[nowListIndex];
		}
		IceDebuff result = default(IceDebuff);
		result.obj = null;
		result.image = null;
		result.target = null;
		return result;
	}

	private IEnumerator updateSkill()
	{
		while (true)
		{
			for (int i = 0; i < 5; i++)
			{
				IceDebuff iceDebuff = listDebuffs[i];
				if (iceDebuff.target != null)
				{
					if (iceDebuff.target.isLife())
					{
						iceDebuff.obj.transform.position = iceDebuff.target.transform.position;
						listDebuffs[i] = iceDebuff;
					}
					else
					{
						iceDebuff.obj.SetActive(value: false);
						iceDebuff.target = null;
						listDebuffs[i] = iceDebuff;
					}
				}
			}
			yield return null;
		}
	}

	public override void callAttack(Enemy e)
	{
		if (!e.getDebufState())
		{
			e.setDebufState(s: true, debuff);
			soundManager.playSound("Skill_Ice");
			IceDebuff iceDebuff = getIceDebuff();
			if (!(iceDebuff.obj == null))
			{
				iceDebuff.target = e;
				iceDebuff.obj.SetActive(value: true);
				listDebuffs[getListIndex] = iceDebuff;
			}
		}
	}

	public override void callTimer()
	{
	}
}
