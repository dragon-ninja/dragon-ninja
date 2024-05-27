using UnityEngine;

public class Price : MonoBehaviour
{
	public static int getUpgradeEquipment(EquipmentRank rank, int level)
	{
		return (int)(2700f * Mathf.Pow(1.5f, level));
	}

	public static int getSaleEquipment(EquipmentRank rank, int level, int image)
	{
		int result = 0;
		switch (rank)
		{
		case EquipmentRank.TYPE_NORMAL:
			switch (image)
			{
			case 1:
				result = 300;
				break;
			case 2:
				result = 400;
				break;
			case 3:
				result = 500;
				break;
			case 4:
				result = 600;
				break;
			case 5:
				result = 700;
				break;
			}
			break;
		case EquipmentRank.TYPE_UNIQUE:
			switch (image)
			{
			case 6:
				result = 1000;
				break;
			case 7:
				result = 2000;
				break;
			case 8:
				result = 3000;
				break;
			case 9:
				result = 4000;
				break;
			case 10:
				result = 5000;
				break;
			}
			break;
		case EquipmentRank.TYPE_LEGENDARY:
			switch (level)
			{
			case 1:
				result = 7000;
				break;
			case 2:
				result = 8000;
				break;
			case 3:
				result = 9000;
				break;
			case 4:
				result = 10000;
				break;
			case 5:
				result = 11000;
				break;
			}
			break;
		default:
			result = 12000 + level * 1000;
			break;
		}
		return result;
	}

	public static int getUpgradePetEquipment(EquipmentRank rank, int level)
	{
		int num = 0;
		switch (rank)
		{
		case EquipmentRank.TYPE_NORMAL:
			num = 300;
			break;
		case EquipmentRank.TYPE_UNIQUE:
			num = 500;
			break;
		case EquipmentRank.TYPE_LEGENDARY:
			num = 900;
			break;
		case EquipmentRank.TYPE_SUPERLEGENDARY:
			num = 2700;
			break;
		}
		return (int)((float)num * Mathf.Pow(1.5f, level));
	}

	public static int getSalePetEquipment(EquipmentRank rank, int level)
	{
		return (int)((float)getUpgradePetEquipment(rank, level) * 0.3f);
	}

	public static float upgradePower(EquipmentRank rank, int level, float nowState)
	{
		float result = 0f;
		switch (rank)
		{
		case EquipmentRank.TYPE_NORMAL:
			result = 1f;
			break;
		case EquipmentRank.TYPE_UNIQUE:
			result = 1f;
			break;
		case EquipmentRank.TYPE_LEGENDARY:
			result = 1.2f;
			break;
		case EquipmentRank.TYPE_SUPERLEGENDARY:
			result = 10f;
			break;
		}
		return result;
	}

	public static float upgradeCritical(EquipmentRank rank, int level, float nowState)
	{
		float result = 0f;
		switch (rank)
		{
		case EquipmentRank.TYPE_NORMAL:
			result = 1f;
			break;
		case EquipmentRank.TYPE_UNIQUE:
			result = 1f;
			break;
		case EquipmentRank.TYPE_LEGENDARY:
			result = 1.2f;
			break;
		case EquipmentRank.TYPE_SUPERLEGENDARY:
			result = 1f;
			break;
		}
		return result;
	}

	public static float upgradeShield(EquipmentRank rank, int level, float nowState)
	{
		float result = 0f;
		switch (rank)
		{
		case EquipmentRank.TYPE_NORMAL:
			result = 1f;
			break;
		case EquipmentRank.TYPE_UNIQUE:
			result = 1f;
			break;
		case EquipmentRank.TYPE_LEGENDARY:
			result = 1.2f;
			break;
		case EquipmentRank.TYPE_SUPERLEGENDARY:
			result = 1.5f;
			break;
		}
		return result;
	}

	public static float upgradeSpeed(EquipmentRank rank, int level, float nowState)
	{
		float result = 0f;
		switch (rank)
		{
		case EquipmentRank.TYPE_NORMAL:
			result = 1f;
			break;
		case EquipmentRank.TYPE_UNIQUE:
			result = 1f;
			break;
		case EquipmentRank.TYPE_LEGENDARY:
			result = 1.2f;
			break;
		case EquipmentRank.TYPE_SUPERLEGENDARY:
			result = 0.5f;
			break;
		}
		return result;
	}

	public static float upgradeHP(EquipmentRank rank, int level, float nowState)
	{
		float result = 0f;
		switch (rank)
		{
		case EquipmentRank.TYPE_NORMAL:
			result = 1f;
			break;
		case EquipmentRank.TYPE_UNIQUE:
			result = 1f;
			break;
		case EquipmentRank.TYPE_LEGENDARY:
			result = 1.2f;
			break;
		case EquipmentRank.TYPE_SUPERLEGENDARY:
			result = 75f;
			break;
		}
		return result;
	}

	public static float upgradePetPower(EquipmentRank rank, int level, float nowState)
	{
		float result = 0f;
		switch (rank)
		{
		case EquipmentRank.TYPE_NORMAL:
			result = 1f;
			break;
		case EquipmentRank.TYPE_UNIQUE:
			result = 1f;
			break;
		case EquipmentRank.TYPE_LEGENDARY:
			result = 1.2f;
			break;
		case EquipmentRank.TYPE_SUPERLEGENDARY:
			result = 1.2f;
			break;
		}
		return result;
	}

	public static float upgradePetCritical(EquipmentRank rank, int level, float nowState)
	{
		float result = 0f;
		switch (rank)
		{
		case EquipmentRank.TYPE_NORMAL:
			result = 1f;
			break;
		case EquipmentRank.TYPE_UNIQUE:
			result = 1f;
			break;
		case EquipmentRank.TYPE_LEGENDARY:
			result = 1.2f;
			break;
		case EquipmentRank.TYPE_SUPERLEGENDARY:
			result = 1.2f;
			break;
		}
		return result;
	}

	public static float upgradePetShield(EquipmentRank rank, int level, float nowState)
	{
		float result = 0f;
		switch (rank)
		{
		case EquipmentRank.TYPE_NORMAL:
			result = 1f;
			break;
		case EquipmentRank.TYPE_UNIQUE:
			result = 1f;
			break;
		case EquipmentRank.TYPE_LEGENDARY:
			result = 1.2f;
			break;
		case EquipmentRank.TYPE_SUPERLEGENDARY:
			result = 1.2f;
			break;
		}
		return result;
	}

	public static float upgradePetSpeed(EquipmentRank rank, int level, float nowState)
	{
		float result = 0f;
		switch (rank)
		{
		case EquipmentRank.TYPE_NORMAL:
			result = 1f;
			break;
		case EquipmentRank.TYPE_UNIQUE:
			result = 1f;
			break;
		case EquipmentRank.TYPE_LEGENDARY:
			result = 1.2f;
			break;
		case EquipmentRank.TYPE_SUPERLEGENDARY:
			result = 1.2f;
			break;
		}
		return result;
	}

	public static float upgradePetHP(EquipmentRank rank, int level, float nowState)
	{
		float result = 0f;
		switch (rank)
		{
		case EquipmentRank.TYPE_NORMAL:
			result = 1f;
			break;
		case EquipmentRank.TYPE_UNIQUE:
			result = 1f;
			break;
		case EquipmentRank.TYPE_LEGENDARY:
			result = 1.2f;
			break;
		case EquipmentRank.TYPE_SUPERLEGENDARY:
			result = 1.2f;
			break;
		}
		return result;
	}
}
