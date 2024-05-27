using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections.Generic;
using UnityEngine;


//标记:配置参数在这里
public class Balance : MonoBehaviour
{
	public static int getLevelByHP_1(int stage, int level)
	{
		if (stage != 7)
		{
			return (int)(280f * Mathf.Pow(1.42f, stage - 1) * Mathf.Pow(2f, level - 1));
		}
		return (int)(280f * Mathf.Pow(1.4f, stage - 1) * Mathf.Pow(2f, level - 1));
	}

	public static int getLevelByPOWER_1(int stage, int level)
	{
		if (stage != 7)
		{
			return (int)(28f * Mathf.Pow(1.32f, stage - 1) * Mathf.Pow(1.8f, level - 1));
		}
		return (int)(28f * Mathf.Pow(1.3f, stage - 1) * Mathf.Pow(1.8f, level - 1));
	}

	public static int getLevelByHP_2(int stage, int level)
	{
		if (stage != 7)
		{
			return (int)(360f * Mathf.Pow(1.42f, stage - 1) * Mathf.Pow(2f, level - 1));
		}
		return (int)(360f * Mathf.Pow(1.4f, stage - 1) * Mathf.Pow(2f, level - 1));
	}

	public static int getLevelByPOWER_2(int stage, int level)
	{
		if (stage != 7)
		{
			return (int)(36f * Mathf.Pow(1.32f, stage - 1) * Mathf.Pow(1.8f, level - 1));
		}
		return (int)(36f * Mathf.Pow(1.3f, stage - 1) * Mathf.Pow(1.8f, level - 1));
	}

	public static int getLevelByHP_3(int stage, int level)
	{
		if (stage != 7)
		{
			return (int)(440f * Mathf.Pow(1.42f, stage - 1) * Mathf.Pow(2f, level - 1));
		}
		return (int)(440f * Mathf.Pow(1.4f, stage - 1) * Mathf.Pow(2f, level - 1));
	}

	public static int getLevelByPOWER_3(int stage, int level)
	{
		if (stage != 7)
		{
			return (int)(44f * Mathf.Pow(1.32f, stage - 1) * Mathf.Pow(1.8f, level - 1));
		}
		return (int)(44f * Mathf.Pow(1.3f, stage - 1) * Mathf.Pow(1.8f, level - 1));
	}

	public static int getLevelByHP_4(int stage, int level)
	{
		if (stage != 7)
		{
			return (int)(520f * Mathf.Pow(1.42f, stage - 1) * Mathf.Pow(2f, level - 1));
		}
		return (int)(520f * Mathf.Pow(1.4f, stage - 1) * Mathf.Pow(2f, level - 1));
	}

	public static int getLevelByPOWER_4(int stage, int level)
	{
		if (stage != 7)
		{
			return (int)(52f * Mathf.Pow(1.32f, stage - 1) * Mathf.Pow(1.8f, level - 1));
		}
		return (int)(52f * Mathf.Pow(1.3f, stage - 1) * Mathf.Pow(1.8f, level - 1));
	}

	public static int getLevelByBossHP(int stage, int level, int bossIndex)
	{
		switch (stage)
		{
		case 8:
			return (int)(55300f * Mathf.Pow(1.48f, stage - 1) * Mathf.Pow(2f, level - 2));
		case 7:
		{
			int num = bossIndex / 5 + 1;
			int result = 0;
			switch (num)
			{
			case 1:
				result = (int)(1300f * Mathf.Pow(1.3f, stage - 1) * Mathf.Pow(2f, level - 2));
				break;
			case 2:
				result = (int)(1300f * Mathf.Pow(1.33f, stage - 1) * Mathf.Pow(2f, level - 2));
				break;
			case 3:
				result = (int)(1300f * Mathf.Pow(1.36f, stage - 1) * Mathf.Pow(2f, level - 2));
				break;
			case 4:
				result = (int)(1300f * Mathf.Pow(1.39f, stage - 1) * Mathf.Pow(2f, level - 2));
				break;
			case 5:
				result = (int)(1300f * Mathf.Pow(1.42f, stage - 1) * Mathf.Pow(2f, level - 2));
				break;
			case 6:
				result = (int)(1300f * Mathf.Pow(1.45f, stage - 1) * Mathf.Pow(2f, level - 2));
				break;
			case 7:
				result = (int)(1300f * Mathf.Pow(1.48f, stage - 1) * Mathf.Pow(2f, level - 2));
				break;
			}
			return result;
		}
		default:
			return (int)(1300f * Mathf.Pow(1.44f, stage - 1) * Mathf.Pow(2f, level - 1));
		}
	}

	public static int getLevelByBossPOWER(int stage, int level, int bossIndex)
	{
		switch (stage)
		{
		case 8:
			return (int)(20f * Mathf.Pow(1.48f, stage - 1) * Mathf.Pow(2f, level - 2));
		case 7:
		{
			int num = bossIndex / 5 + 1;
			int result = 0;
			switch (num)
			{
			case 1:
				result = (int)(100f * Mathf.Pow(1.3f, stage - 1) * Mathf.Pow(2f, level - 2));
				break;
			case 2:
				result = (int)(100f * Mathf.Pow(1.33f, stage - 1) * Mathf.Pow(2f, level - 2));
				break;
			case 3:
				result = (int)(100f * Mathf.Pow(1.36f, stage - 1) * Mathf.Pow(2f, level - 2));
				break;
			case 4:
				result = (int)(100f * Mathf.Pow(1.39f, stage - 1) * Mathf.Pow(2f, level - 2));
				break;
			case 5:
				result = (int)(100f * Mathf.Pow(1.42f, stage - 1) * Mathf.Pow(2f, level - 2));
				break;
			case 6:
				result = (int)(100f * Mathf.Pow(1.45f, stage - 1) * Mathf.Pow(2f, level - 2));
				break;
			case 7:
				result = (int)(100f * Mathf.Pow(1.48f, stage - 1) * Mathf.Pow(2f, level - 2));
				break;
			case 8:
				result = (int)(100f * Mathf.Pow(1.48f, stage - 1) * Mathf.Pow(2f, level - 2));
				break;
			}
			return result;
		}
		default:
			return (int)(100f * Mathf.Pow(1.32f, stage - 1) * Mathf.Pow(2f, level - 1));
		}
	}

	public static int getKnightAttackPower(int level)
	{
		return (int)(70f * Mathf.Pow(1.32f, 9f) * Mathf.Pow(1.8f, level - 1));
	}

	public static int getKnightDropPower(int level)
	{
		return (int)(40f * Mathf.Pow(1.32f, 9f) * Mathf.Pow(1.8f, level - 1));
	}

	public static int getLevelByDropCoin(int stage, int level)
	{
		int min = (int)(4f * (Mathf.Pow(1.9f, level - 1) * Mathf.Pow(1.42f, stage - 1)));
		int num = (int)(9f * (Mathf.Pow(1.9f, level - 1) * Mathf.Pow(1.42f, stage - 1)));
		return UnityEngine.Random.Range(min, num + 1);
	}

	public static bool getDropItem(float allPer)
	{
		return UnityEngine.Random.Range(0f, 100f) <= 5f + allPer;
	}

	public static bool getDropEquipment(int stage, int level, bool boss)
	{
		float num = 16f * Mathf.Pow(1.03f, stage - 1) * Mathf.Pow(1.126f, level - 1);
		if (stage == 8)
		{
			num = 110f;
		}
		return UnityEngine.Random.Range(0f, 100f) <= num;
	}

	public static bool getDropEgg()
	{
		return UnityEngine.Random.Range(0f, 100f) <= 0.4f;
	}

	private static int getRandomStatusInt(EquipmentRank rank, EquipmentType type, int level, int normalMin, int normalMax, int normalStandard, int uniqueMin, int uniqueMax, int uniqueStandard, int legendaryMin1, int legendaryMax1, int legendaryMin2, int legendaryMax2, int legendaryMin3, int legendaryMax3, int legendaryMin4, int legendaryMax4, int legendaryMin5, int legendaryMax5, int superLegendaryMin, int superLegendaryMax, EquipmentType[] types)
	{
		int num = 0;
		int min = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		switch (rank)
		{
		case EquipmentRank.TYPE_NORMAL:
			min = normalMin;
			num2 = normalMax;
			num3 = normalStandard;
			break;
		case EquipmentRank.TYPE_UNIQUE:
			min = uniqueMin;
			num2 = uniqueMax;
			num3 = uniqueStandard;
			break;
		case EquipmentRank.TYPE_LEGENDARY:
			switch (level)
			{
			case 5:
				min = legendaryMin5;
				num2 = legendaryMax5;
				break;
			case 4:
				min = legendaryMin4;
				num2 = legendaryMax4;
				break;
			case 3:
				min = legendaryMin3;
				num2 = legendaryMax3;
				break;
			case 2:
				min = legendaryMin2;
				num2 = legendaryMax2;
				break;
			default:
				min = legendaryMin1;
				num2 = legendaryMax1;
				break;
			}
			break;
		case EquipmentRank.TYPE_SUPERLEGENDARY:
			min = superLegendaryMin;
			num2 = superLegendaryMax;
			break;
		}
		num4 = UnityEngine.Random.Range(min, num2 + 1);
		num = num3 + num4;
		bool flag = false;
		int num5 = types.Length;
		for (int i = 0; i < num5; i++)
		{
			if (type == types[i])
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			num /= 2;
		}
		return num;
	}

	private static float getRandomStatusFloat(EquipmentRank rank, EquipmentType type, int level, float normalMin, float normalMax, float normalStandard, float uniqueMin, float uniqueMax, float uniqueStandard, float legendaryMin1, float legendaryMax1, float legendaryMin2, float legendaryMax2, float legendaryMin3, float legendaryMax3, float legendaryMin4, float legendaryMax4, float legendaryMin5, float legendaryMax5, float superLegendaryMin, float superLegendaryMax, EquipmentType[] types)
	{
		float num = 0f;
		float min = 0f;
		float max = 0f;
		float num2 = 0f;
		float num3 = 0f;
		switch (rank)
		{
		case EquipmentRank.TYPE_NORMAL:
			min = normalMin;
			max = normalMax;
			num2 = normalStandard;
			break;
		case EquipmentRank.TYPE_UNIQUE:
			min = uniqueMin;
			max = uniqueMax;
			num2 = uniqueStandard;
			break;
		case EquipmentRank.TYPE_LEGENDARY:
			switch (level)
			{
			case 5:
				min = legendaryMin5;
				max = legendaryMax5;
				break;
			case 4:
				min = legendaryMin4;
				max = legendaryMax4;
				break;
			case 3:
				min = legendaryMin3;
				max = legendaryMax3;
				break;
			case 2:
				min = legendaryMin2;
				max = legendaryMax2;
				break;
			default:
				min = legendaryMin1;
				max = legendaryMax1;
				break;
			}
			break;
		case EquipmentRank.TYPE_SUPERLEGENDARY:
			min = superLegendaryMin;
			max = superLegendaryMax;
			break;
		}
		num3 = UnityEngine.Random.Range(min, max);
		num = num2 + num3;
		bool flag = false;
		int num4 = types.Length;
		for (int i = 0; i < num4; i++)
		{
			if (type == types[i])
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			num /= 2f;
		}
		return num;
	}

	private static int getFixStatusInt(EquipmentRank rank, int imageIndex, int[] datas, int legendaryMin1, int legendaryMax1, int legendaryMin2, int legendaryMax2, int legendaryMin3, int legendaryMax3, int legendaryMin4, int legendaryMax4, int legendaryMin5, int legendaryMax5, int super, int level)
	{
		switch (rank)
		{
		case EquipmentRank.TYPE_SUPERLEGENDARY:
			return super;
		case EquipmentRank.TYPE_LEGENDARY:
			switch (level)
			{
			case 5:
				return UnityEngine.Random.Range(legendaryMin5, legendaryMin5 + 1);
			case 4:
				return UnityEngine.Random.Range(legendaryMin4, legendaryMin4 + 1);
			case 3:
				return UnityEngine.Random.Range(legendaryMin3, legendaryMin3 + 1);
			case 2:
				return UnityEngine.Random.Range(legendaryMin2, legendaryMin2 + 1);
			case 1:
				return UnityEngine.Random.Range(legendaryMin1, legendaryMin1 + 1);
			default:
				return UnityEngine.Random.Range(legendaryMin1, legendaryMin1 + 1);
			}
		default:
			if (datas.Length <= imageIndex - 1)
			{
				return 0;
			}
			return datas[imageIndex - 1];
		}
	}

	private static float getFixStatusFloat(EquipmentRank rank, int imageIndex, float[] datas, float legendaryMin1, float legendaryMax1, float legendaryMin2, float legendaryMax2, float legendaryMin3, float legendaryMax3, float legendaryMin4, float legendaryMax4, float legendaryMin5, float legendaryMax5, float super, int level)
	{
		switch (rank)
		{
		case EquipmentRank.TYPE_SUPERLEGENDARY:
			return super;
		case EquipmentRank.TYPE_LEGENDARY:
			switch (level)
			{
			case 5:
				return UnityEngine.Random.Range(legendaryMin5, legendaryMin5);
			case 4:
				return UnityEngine.Random.Range(legendaryMin4, legendaryMin4);
			case 3:
				return UnityEngine.Random.Range(legendaryMin3, legendaryMin3);
			case 2:
				return UnityEngine.Random.Range(legendaryMin2, legendaryMin2);
			case 1:
				return UnityEngine.Random.Range(legendaryMin1, legendaryMin1);
			default:
				return UnityEngine.Random.Range(legendaryMin1, legendaryMin1);
			}
		default:
			if (datas.Length <= imageIndex - 1)
			{
				return 0f;
			}
			return datas[imageIndex - 1];
		}
	}

	private static int getRandomPower(EquipmentRank rank, EquipmentType type, int level)
	{
		return getRandomStatusInt(rank, type, level, 10, 30, 50, 10, 30, 75, 150, 170, 180, 200, 210, 230, 240, 260, 270, 290, 350 + (level - 1) * 10, 400 + (level - 1) * 10, new EquipmentType[2]
		{
			EquipmentType.TYPE_HELMET,
			EquipmentType.TYPE_WEAPON
		});
	}

	private static float getRandomCritial(EquipmentRank rank, EquipmentType type, int level)
	{
		return getRandomStatusFloat(rank, type, level, 1f, 3f, 5f, 1f, 3f, 8f, 15f, 17f, 18f, 20f, 21f, 23f, 24f, 26f, 27f, 29f, 35 + (level - 1), 37 + (level - 1), new EquipmentType[2]
		{
			EquipmentType.TYPE_HELMET,
			EquipmentType.TYPE_WEAPON
		});
	}

	private static int getRandomShield(EquipmentRank rank, EquipmentType type, int level)
	{
		return getRandomStatusInt(rank, type, level, 1, 3, 6, 2, 3, 10, 19, 22, 24, 27, 30, 33, 36, 39, 42, 45, 48 + (level - 1), 50 + (level - 1), new EquipmentType[2]
		{
			EquipmentType.TYPE_HELMET,
			EquipmentType.TYPE_ARMOR
		});
	}

	private static float getRandomSpeed(EquipmentRank rank, EquipmentType type, int level)
	{
		return getRandomStatusFloat(rank, type, level, 0.4f, 0.6f, 2f, 0.4f, 0.6f, 2.6f, 3.5f, 3.6f, 3.7f, 3.9f, 4f, 4.2f, 4.3f, 4.5f, 4.6f, 4.8f, 5.3f + (float)(level - 1) * 0.3f, 5.5f + (float)(level - 1) * 0.3f, new EquipmentType[2]
		{
			EquipmentType.TYPE_ARMOR,
			EquipmentType.TYPE_HORSE
		});
	}

	private static int getRandomHp(EquipmentRank rank, EquipmentType type, int level)
	{
		return getRandomStatusInt(rank, type, level, 0, 100, 200, 50, 100, 300, 550, 600, 600, 700, 700, 800, 800, 900, 900, 1000, 1500 + (level - 1) * 75, 1800 + (level - 1) * 75, new EquipmentType[2]
		{
			EquipmentType.TYPE_ARMOR,
			EquipmentType.TYPE_HORSE
		});
	}

	private static int getIndexStatusPower(EquipmentRank rank, int imageIndex, int level)
	{
		int[] datas = new int[10]
		{
			75,
			85,
			95,
			105,
			115,
			155,
			185,
			215,
			245,
			275
		};
		int legendaryMin = 370;
		int legendaryMin2 = 420;
		int legendaryMin3 = 470;
		int legendaryMin4 = 520;
		int legendaryMin5 = 570;
		int legendaryMax = 400;
		int legendaryMax2 = 450;
		int legendaryMax3 = 500;
		int legendaryMax4 = 550;
		int legendaryMax5 = 600;
		int super = 700 + (level - 1) * 20;
		return getFixStatusInt(rank, imageIndex, datas, legendaryMin, legendaryMax, legendaryMin2, legendaryMax2, legendaryMin3, legendaryMax3, legendaryMin4, legendaryMax4, legendaryMin5, legendaryMax5, super, level);
	}

	private static float getIndexStatusCritical(EquipmentRank rank, int imageIndex, int level)
	{
		float[] datas = new float[10]
		{
			1f,
			2f,
			3f,
			4f,
			5f,
			6.5f,
			7.5f,
			8.5f,
			9.5f,
			10.5f
		};
		float legendaryMin = 10f;
		float legendaryMin2 = 20f;
		float legendaryMin3 = 30f;
		float legendaryMin4 = 40f;
		float legendaryMin5 = 50f;
		float legendaryMax = 20f;
		float legendaryMax2 = 30f;
		float legendaryMax3 = 40f;
		float legendaryMax4 = 50f;
		float legendaryMax5 = 60f;
		float super = 65 + (level - 1) * 10;
		return getFixStatusFloat(rank, imageIndex, datas, legendaryMin, legendaryMax, legendaryMin2, legendaryMax2, legendaryMin3, legendaryMax3, legendaryMin4, legendaryMax4, legendaryMin5, legendaryMax5, super, level);
	}

	private static int getIndexStatusShield(EquipmentRank rank, int imageIndex, int level)
	{
		int[] datas = new int[10]
		{
			7,
			9,
			11,
			13,
			15,
			19,
			22,
			25,
			28,
			31
		};
		int legendaryMin = 35;
		int legendaryMin2 = 40;
		int legendaryMin3 = 45;
		int legendaryMin4 = 51;
		int legendaryMin5 = 57;
		int legendaryMax = 38;
		int legendaryMax2 = 43;
		int legendaryMax3 = 48;
		int legendaryMax4 = 54;
		int legendaryMax5 = 60;
		int super = 64 + (level - 1) * 2;
		return getFixStatusInt(rank, imageIndex, datas, legendaryMin, legendaryMax, legendaryMin2, legendaryMax2, legendaryMin3, legendaryMax3, legendaryMin4, legendaryMax4, legendaryMin5, legendaryMax5, super, level);
	}

	private static float getIndexStatusSpeed(EquipmentRank rank, int imageIndex, int level)
	{
		float[] datas = new float[10]
		{
			4f,
			4.3f,
			4.6f,
			4.9f,
			5.2f,
			6f,
			6.4f,
			6.8f,
			7.2f,
			7.6f
		};
		float legendaryMin = 8f;
		float legendaryMin2 = 8.6f;
		float legendaryMin3 = 9.2f;
		float legendaryMin4 = 9.8f;
		float legendaryMin5 = 10.4f;
		float legendaryMax = 8.3f;
		float legendaryMax2 = 8.9f;
		float legendaryMax3 = 9.5f;
		float legendaryMax4 = 10.1f;
		float legendaryMax5 = 10.7f;
		float super = 11f + (float)(level - 1) * 0.5f;
		return getFixStatusFloat(rank, imageIndex, datas, legendaryMin, legendaryMax, legendaryMin2, legendaryMax2, legendaryMin3, legendaryMax3, legendaryMin4, legendaryMax4, legendaryMin5, legendaryMax5, super, level);
	}

	private static int getIndexStatusHp(EquipmentRank rank, int imageIndex, int level)
	{
		int[] datas = new int[10]
		{
			300,
			350,
			400,
			450,
			500,
			650,
			740,
			830,
			920,
			1010
		};
		int legendaryMin = 1250;
		int legendaryMin2 = 1450;
		int legendaryMin3 = 1650;
		int legendaryMin4 = 1850;
		int legendaryMin5 = 2050;
		int legendaryMax = 1300;
		int legendaryMax2 = 1500;
		int legendaryMax3 = 1700;
		int legendaryMax4 = 1900;
		int legendaryMax5 = 2100;
		int super = 2500 + (level - 1) * 150;
		return getFixStatusInt(rank, imageIndex, datas, legendaryMin, legendaryMax, legendaryMin2, legendaryMax2, legendaryMin3, legendaryMax3, legendaryMin4, legendaryMax4, legendaryMin5, legendaryMax5, super, level);
	}

	private static void randomEquipemntRank(ref EquipmentData data, int stage, int level)
	{
		float num = UnityEngine.Random.Range(0f, 1000f);
		float num2 = 1011f - 5f * Mathf.Pow(1.95f, stage - 1) * Mathf.Pow(2.08f, level - 1);
		float num3 = 1067f - 5f * Mathf.Pow(1.82f, stage - 1) * Mathf.Pow(2.08f, level - 1);
		float num4 = 1321f - 5f * Mathf.Pow(1.7f, stage - 1) * Mathf.Pow(2.08f, level - 1);
		if (stage == 8)
		{
			num2 = -1f;
			num3 = -1f;
		}
		if (num < num2)
		{
			data.rank = EquipmentRank.TYPE_NORMAL;
			data.grade = getRandomGrade(num, num2);
		}
		else if (num < num3)
		{
			data.rank = EquipmentRank.TYPE_UNIQUE;
			data.grade = getRandomGrade(num, num3);
		}
		else if (num < num4)
		{
			data.rank = EquipmentRank.TYPE_LEGENDARY;
			data.grade = EquipmentGrade.TYPE_S;
			data.level = 1;
		}
		else
		{
			data.rank = EquipmentRank.TYPE_SUPERLEGENDARY;
			data.grade = EquipmentGrade.TYPE_SS;
			data.level = 1;
		}
	}

	private static void selectImageIndex(ref EquipmentData data, int stage, int level)
	{
		int num = 0;
		float num2 = 0f;
		float num3 = 0f;
		float num4 = 0f;
		float num5 = 0f;
		if (data.rank == EquipmentRank.TYPE_NORMAL)
		{
			float num6 = UnityEngine.Random.Range(0f, 1000f);
			num2 = 500f - (float)((stage - 1) * 60);
			num3 = 700f - (float)((stage - 1) * 60);
			num4 = 850f - (float)((stage - 1) * 60);
			num5 = 950f - (float)((stage - 1) * 60);
			num = ((num6 < num2) ? 1 : ((num6 < num3) ? 2 : ((num6 < num4) ? 3 : ((!(num6 < num5)) ? 5 : 4))));
		}
		else if (data.rank == EquipmentRank.TYPE_UNIQUE)
		{
			float num7 = UnityEngine.Random.Range(0f, 1000f);
			num2 = 700f - (float)((stage - 1) * 25);
			num3 = 800f - (float)((stage - 1) * 25);
			num4 = 900f - (float)((stage - 1) * 25);
			num5 = 990f - (float)((stage - 1) * 25);
			num = ((num7 < num2) ? 6 : ((num7 < num3) ? 7 : ((num7 < num4) ? 8 : ((!(num7 < num5)) ? 10 : 9))));
		}
		else
		{
			num = UnityEngine.Random.Range(0, 5) + 10 + 1;
		}
		data.imageIndex = num;
	}

	private static EquipmentGrade getRandomGrade(float nowIndex, float maxIndex)
	{
		EquipmentGrade equipmentGrade = EquipmentGrade.TYPE_D;
		if (nowIndex > maxIndex * 0.9f)
		{
			return EquipmentGrade.TYPE_A;
		}
		if (nowIndex > maxIndex * 0.85f)
		{
			return EquipmentGrade.TYPE_B;
		}
		if (nowIndex > maxIndex * 0.65f)
		{
			return EquipmentGrade.TYPE_C;
		}
		return EquipmentGrade.TYPE_D;
	}

	private static void randomEquipmentStatus(ref EquipmentData data)
	{
		int num = UnityEngine.Random.Range(0, 2) + 1;
		List<int> list = new List<int>
		{
			1,
			2,
			3,
			4,
			5
		};
		int num2 = 0;
		switch (data.type)
		{
		case EquipmentType.TYPE_WEAPON:
			num2 = 1;
			data.power = getIndexStatusPower(data.rank, data.imageIndex, data.level);
			break;
		case EquipmentType.TYPE_ARMOR:
			num2 = 5;
			data.hp = getIndexStatusHp(data.rank, data.imageIndex, data.level);
			break;
		case EquipmentType.TYPE_HELMET:
			num2 = 3;
			data.shield = getIndexStatusShield(data.rank, data.imageIndex, data.level);
			break;
		case EquipmentType.TYPE_HORSE:
			num2 = 4;
			data.speed = getIndexStatusSpeed(data.rank, data.imageIndex, data.level);
			break;
		}
		int count = list.Count;
		for (int i = 0; i < count; i++)
		{
			if (list[i] == num2)
			{
				list.RemoveAt(i);
				break;
			}
		}
		for (int j = 0; j < num; j++)
		{
			int index = UnityEngine.Random.Range(0, list.Count);
			switch (list[index])
			{
			case 1:
			{
				int randomPower = getRandomPower(data.rank, data.type, data.level);
				data.power = randomPower;
				break;
			}
			case 2:
			{
				float randomCritial = getRandomCritial(data.rank, data.type, data.level);
				data.critical = randomCritial;
				break;
			}
			case 3:
			{
				int randomShield = getRandomShield(data.rank, data.type, data.level);
				data.shield = randomShield;
				break;
			}
			case 4:
			{
				float randomSpeed = getRandomSpeed(data.rank, data.type, data.level);
				data.speed = randomSpeed;
				break;
			}
			case 5:
			{
				int randomHp = getRandomHp(data.rank, data.type, data.level);
				data.hp = randomHp;
				break;
			}
			}
			list.RemoveAt(index);
		}
	}

	public static EquipmentData getRandomEquipmentData(int stage, int level, bool boss)
	{
		EquipmentData data = default(EquipmentData);
		data.type = (EquipmentType)UnityEngine.Random.Range(0, 4);
		randomEquipemntRank(ref data, stage, level);
		selectImageIndex(ref data, stage, level);
		randomEquipmentStatus(ref data);
		data.level = 1;
		data.objectIndex = DataManager.getEquipmentIndex();
		return data;
	}

	public static float getAttackEquipmentPercent(EquipmentData data)
	{
		float num = 30f;
		float num2 = 45f;
		float num3 = 25f;
		float num4 = 40f;
		float num5 = 1f;
		float num6 = 1f;
		float num7 = 1f;
		float result = 0f;
		switch (data.rank)
		{
		case EquipmentRank.TYPE_NORMAL:
			result = num;
			if ((int)data.imageIndex == 5)
			{
				result = num2;
			}
			break;
		case EquipmentRank.TYPE_UNIQUE:
			result = num3;
			if ((int)data.imageIndex == 10)
			{
				result = num4;
			}
			break;
		case EquipmentRank.TYPE_LEGENDARY:
			result = num5;
			if ((int)data.level == 5)
			{
				result = num6;
			}
			break;
		case EquipmentRank.TYPE_SUPERLEGENDARY:
			result = num7;
			break;
		}
		return result;
	}

	public static EquipmentData getAttachEquipmentData(EquipmentData baseData, bool joker)
	{
		float num = 0f;
		EquipmentData data = default(EquipmentData);
		data.rank = baseData.rank;
		data.grade = baseData.grade;
		data.imageIndex = baseData.imageIndex;
		data.type = baseData.type;
		data.level = baseData.level;
		num = getAttackEquipmentPercent(data);
		float num2 = UnityEngine.Random.Range(0f, 100f);
		if (joker)
		{
			num = -10f;
		}
		if (num <= num2)
		{
			if ((int)data.imageIndex < 11)
			{
				++data.imageIndex;
				if ((int)data.imageIndex >= 11)
				{
					data.rank = EquipmentRank.TYPE_LEGENDARY;
					data.imageIndex = 10 + UnityEngine.Random.Range(0, 5) + 1;
					data.level = 1;
				}
			}
			else
			{
				ref ObscuredInt level = ref data.level;
				level = (int)level + 1;
				if (data.rank == EquipmentRank.TYPE_LEGENDARY && (int)data.level > 5)
				{
					data.rank = EquipmentRank.TYPE_SUPERLEGENDARY;
					data.level = 1;
				}
			}
		}
		if ((int)data.imageIndex <= 5)
		{
			data.rank = EquipmentRank.TYPE_NORMAL;
		}
		else if ((int)data.imageIndex <= 10)
		{
			data.rank = EquipmentRank.TYPE_UNIQUE;
		}
		randomEquipmentStatus(ref data);
		data.objectIndex = DataManager.getEquipmentIndex();
		return data;
	}

	private static EquipmentRank getRandomPetRank()
	{
		EquipmentRank equipmentRank = EquipmentRank.TYPE_NORMAL;
		float num = UnityEngine.Random.Range(0f, 1000f);
		float num2 = 670f;
		float num3 = 970f;
		if (num < num2)
		{
			return EquipmentRank.TYPE_NORMAL;
		}
		if (num < num3)
		{
			return EquipmentRank.TYPE_UNIQUE;
		}
		return EquipmentRank.TYPE_LEGENDARY;
	}

	public static PetData getRandomPetData()
	{
		PetData data = new PetData(getRandomPetRank(), 0, UnityEngine.Random.Range(0, 20) + 1);
		EquipmentObjects.settingRankByStatus(ref data);
		return data;
	}

	public static EquipmentData getBoxEquipmentData(List<int> listTypes, EquipmentRank rank)
	{
		EquipmentData data = default(EquipmentData);
		data.type = (EquipmentType)listTypes[Random.Range(0, listTypes.Count)];
		int num = 0;
		switch (rank)
		{
		case EquipmentRank.TYPE_UNIQUE:
			num = 5;
			break;
		case EquipmentRank.TYPE_LEGENDARY:
			num = 10;
			break;
		}
		data.rank = rank;
		data.imageIndex = UnityEngine.Random.Range(0, 5) + 1 + num;
		randomEquipmentStatus(ref data);
		data.level = 1;
		data.objectIndex = DataManager.getEquipmentIndex();
		return data;
	}

	public static EquipmentData getBoxAllRandomEquipmentData(List<int> listTypes)
	{
		EquipmentRank rank = EquipmentRank.TYPE_NORMAL;
		float num = UnityEngine.Random.Range(0f, 1000f);
		if (num > 990f)
		{
			rank = EquipmentRank.TYPE_LEGENDARY;
		}
		else if (num > 840f)
		{
			rank = EquipmentRank.TYPE_UNIQUE;
		}
		return getBoxEquipmentData(listTypes, rank);
	}
}
