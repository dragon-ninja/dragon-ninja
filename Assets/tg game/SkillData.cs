using UnityEngine;

public class SkillData : MonoBehaviour
{
	public static float FireSkillPower(int count)
	{
		return 0.085f * Mathf.Pow(2.3f, count - 1);
	}

	public static float MachineSkillDelayTime(int count)
	{
		return 7f - (float)(count - 1) * 0.2f;
	}

	public static float MachineSkillActiveTime(int count)
	{
		return 3f + (float)(count - 1) * 0.1f;
	}

	public static float MachineSkillPowerUp(int count)
	{
		return 0.065f * Mathf.Pow(2.3f, count - 1);
	}

	public static float BloodSkillDelayTime(int count)
	{
		return 1f;
	}

	public static float BloodSkillHeal(int count)
	{
		return 0.0172f * Mathf.Pow(2.3f, count - 1);
	}

	public static float IceSkillEnemyDebuff(int count)
	{
		return 0.075f * Mathf.Pow(2.3f, count - 1);
	}

	public static float LightningSkillPower(int count)
	{
		return 0.09f * Mathf.Pow(2.3f, count - 1);
	}
}
