using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
	private EquipmentData helmetData;

	private EquipmentData armorData;

	private EquipmentData weaponData;

	private EquipmentData horseData;

	private ObscuredInt playerHP;

	private ObscuredInt playerPower;

	private ObscuredFloat playerCriticalPercent;

	private ObscuredInt playerShield;

	private ObscuredFloat playerSpeed;

	private ObscuredInt playerNowHP;

	private ObscuredInt plusHP;

	private ObscuredInt plusPower;

	private ObscuredInt plusCritical;

	private ObscuredInt plusShield;

	private ObscuredInt plusSpeed;

	private ObscuredBool activePower = false;

	private ObscuredBool activeShield = false;

	private ObscuredBool activeSpeed = false;

	private List<BaseSkill> listSkillObjects = new List<BaseSkill>();

	private GameObject skillObjects;

	public void refreshPet(PetData petdata)
	{
		initObjects(helmetData, armorData, weaponData, horseData, petdata, null, null);
	}

	public void initObjects(EquipmentData helmet, EquipmentData armor, EquipmentData weapon, EquipmentData horse, PetData petData, Player p, GameScene gameScene)
	{
		helmetData = helmet;
		armorData = armor;
		weaponData = weapon;
		horseData = horse;
		playerHP = (int)helmetData.hp + (int)armorData.hp + (int)horseData.hp + (int)weaponData.hp;
		playerPower = (int)helmetData.power + (int)armorData.power + (int)horseData.power + (int)weaponData.power;
		playerCriticalPercent = (float)helmetData.critical + (float)armorData.critical + (float)horseData.critical + (float)weaponData.critical;
		playerShield = (int)helmetData.shield + (int)armorData.shield + (int)horseData.shield + (int)weaponData.shield;
		playerSpeed = (float)helmetData.speed + (float)armorData.speed + (float)horseData.speed + (float)weaponData.speed;
		playerHP = (int)playerHP + ((int)helmetData.addHp + (int)armorData.addHp + (int)horseData.addHp + (int)weaponData.addHp);
		playerPower = (int)playerPower + ((int)helmetData.addPower + (int)armorData.addPower + (int)horseData.addPower + (int)weaponData.addPower);
		playerCriticalPercent = (float)playerCriticalPercent + ((float)helmetData.addCritical + (float)armorData.addCritical + (float)horseData.addCritical + (float)weaponData.addCritical);
		playerShield = (int)playerShield + ((int)helmetData.addShield + (int)armorData.addShield + (int)horseData.addShield + (int)weaponData.addShield);
		playerSpeed = (float)playerSpeed + ((float)helmetData.addSpeed + (float)armorData.addSpeed + (float)horseData.addSpeed + (float)weaponData.addSpeed);
		if ((int)playerHP <= 0)
		{
			playerHP = 1;
		}
		if ((float)playerSpeed <= 0.1f)
		{
			playerSpeed = 0.1f;
		}
		if ((int)petData.imageIndex != 0)
		{
			if ((float)petData.powerPer >= 0.1f)
			{
				playerPower = (int)playerPower + (int)((float)(int)playerPower * (float)petData.powerPer * 0.01f);
			}
			if ((float)petData.criticalPer >= 0.1f)
			{
				playerCriticalPercent = (float)playerCriticalPercent + (float)playerCriticalPercent * (float)petData.criticalPer * 0.01f;
			}
			if ((float)petData.shieldPer >= 0.1f)
			{
				playerShield = (int)playerShield + (int)((float)(int)playerShield * (float)petData.shieldPer * 0.01f);
			}
			if ((float)petData.speedPer >= 0.1f)
			{
				playerSpeed = (float)playerSpeed + (float)playerSpeed * (float)petData.speedPer * 0.01f;
			}
			if ((float)petData.hpPer >= 0.1f)
			{
				playerHP = (int)playerHP + (int)((float)(int)playerHP * (float)petData.hpPer * 0.01f);
			}
		}
		plusHP = 0;
		plusPower = 0;
		plusCritical = 0;
		plusShield = 0;
		plusSpeed = 0;
		if (skillObjects == null)
		{
			skillObjects = new GameObject();
			skillObjects.name = "SkillObjects";
			skillObjects.transform.position = new Vector3(0f, 0f, -8f);
		}
		if (p != null)
		{
			settingSkill(p, gameScene);
		}
	}

	private void settingSkill(Player p, GameScene gameScene)
	{
		int count = listSkillObjects.Count;
		for (int i = 0; i < count; i++)
		{
			UnityEngine.Object.Destroy(listSkillObjects[i].gameObject);
		}
		listSkillObjects.Clear();
		List<int> list = new List<int>();
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		switch ((int)helmetData.imageIndex)
		{
		case 11:
			num++;
			break;
		case 12:
			num2++;
			break;
		case 13:
			num3++;
			break;
		case 14:
			num4++;
			break;
		case 15:
			num5++;
			break;
		}
		switch ((int)armorData.imageIndex)
		{
		case 11:
			num++;
			break;
		case 12:
			num2++;
			break;
		case 13:
			num3++;
			break;
		case 14:
			num4++;
			break;
		case 15:
			num5++;
			break;
		}
		switch ((int)weaponData.imageIndex)
		{
		case 11:
			num++;
			break;
		case 12:
			num2++;
			break;
		case 13:
			num3++;
			break;
		case 14:
			num4++;
			break;
		case 15:
			num5++;
			break;
		}
		switch ((int)horseData.imageIndex)
		{
		case 11:
			num++;
			break;
		case 12:
			num2++;
			break;
		case 13:
			num3++;
			break;
		case 14:
			num4++;
			break;
		case 15:
			num5++;
			break;
		}
		GameObject data = null;
		GameObject data2 = null;
		GameObject data3 = null;
		GameObject data4 = null;
		GameObject data5 = null;
		if (num > 0)
		{
			data = Singleton<AssetManager>.Instance.LoadObject("Prefebs/Skill/Fire/SkillFireHorse");
			list.Add(num);
		}
		if (num2 > 0)
		{
			data2 = Singleton<AssetManager>.Instance.LoadObject("Prefebs/Skill/Ice/SkillIceHead");
			list.Add(num2);
		}
		if (num3 > 0)
		{
			data3 = Singleton<AssetManager>.Instance.LoadObject("Prefebs/Skill/Blood/SkillBloodWeapon");
			list.Add(num3);
		}
		if (num4 > 0)
		{
			data4 = Singleton<AssetManager>.Instance.LoadObject("Prefebs/Skill/Ice/SkillIceWeapon");
			list.Add(num4);
		}
		if (num5 > 0)
		{
			data5 = Singleton<AssetManager>.Instance.LoadObject("Prefebs/Skill/Lightning/SkillLightningWeapon");
			list.Add(num5);
		}
		settingSkill(data);
		settingSkill(data2);
		settingSkill(data3);
		settingSkill(data4);
		settingSkill(data5);
		int count2 = listSkillObjects.Count;
		for (int j = 0; j < count2; j++)
		{
			listSkillObjects[j].startSkill(p, gameScene, list[j]);
		}
	}

	private void settingSkill(GameObject data)
	{
		if (data != null)
		{
			GameObject gameObject = Object.Instantiate(data);
			gameObject.transform.parent = skillObjects.transform;
			listSkillObjects.Add(gameObject.GetComponent<BaseSkill>());
		}
	}

	public void skillAttackCallback(Enemy enemy)
	{
		int count = listSkillObjects.Count;
		for (int i = 0; i < count; i++)
		{
			listSkillObjects[i].callAttack(enemy);
		}
	}

	public EquipmentData getHelmetData()
	{
		return helmetData;
	}

	public EquipmentData getArmorData()
	{
		return armorData;
	}

	public EquipmentData getWeaponData()
	{
		return weaponData;
	}

	public EquipmentData getHorseData()
	{
		return horseData;
	}

	public void settingNowHP()
	{
		playerNowHP = playerHP;
	}

	public int getPlayerMaxHP()
	{
		return playerHP;
	}

	public int getPowerOrigin()
	{
		return playerPower;
	}

	public int getPower()
	{
		int num = playerPower;
		int count = listSkillObjects.Count;
		for (int i = 0; i < count; i++)
		{
			num += (int)listSkillObjects[i].getPower();
		}
		if ((bool)activePower)
		{
			num = (int)((float)num * 1.5f);
		}
		return num;
	}

	public float getCriticalOrigin()
	{
		return playerCriticalPercent;
	}

	public float getCritical()
	{
		float num = playerCriticalPercent;
		int count = listSkillObjects.Count;
		for (int i = 0; i < count; i++)
		{
			num += (float)listSkillObjects[i].getCritical();
		}
		return num;
	}

	public int getShieldOrigin()
	{
		return playerShield;
	}

	public int getShield()
	{
		int num = playerShield;
		int count = listSkillObjects.Count;
		for (int i = 0; i < count; i++)
		{
			num += (int)listSkillObjects[i].getShield();
		}
		if ((bool)activeShield)
		{
			num = (int)((float)num * 1.5f);
		}
		return num;
	}

	public float getSpeed()
	{
		float num = playerSpeed;
		if ((bool)activeSpeed)
		{
			num *= 1.5f;
		}
		return num;
	}

	public int getPlayerNowHP()
	{
		return playerNowHP;
	}

	public bool getCricialActive()
	{
		return Random.Range(0f, 100f) <= (float)playerCriticalPercent;
	}

	public int addDamage(int damage)
	{
		int num = damage - getShield();
		if (num < 0)
		{
			num = 0;
		}
		playerNowHP = (int)playerNowHP - num;
		return num;
	}

	public int addHeal(int heal)
	{
		playerNowHP = (int)playerNowHP + heal;
		if ((int)playerNowHP > (int)playerHP)
		{
			playerNowHP = playerHP;
		}
		return heal;
	}

	public bool isLife()
	{
		return (int)playerNowHP > 0;
	}

	public void setActivePower(bool state)
	{
		activePower = state;
	}

	public void setActiveShield(bool state)
	{
		activeShield = state;
	}

	public void setActiveSpeed(bool state)
	{
		activeSpeed = state;
	}
}
