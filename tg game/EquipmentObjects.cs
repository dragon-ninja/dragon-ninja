using System.Collections.Generic;
using UnityEngine;

public class EquipmentObjects
{
	public const int JOKER_ID = 10000;

	public static PetPositionType getPetPositionType(int imageIndex)
	{
		PetPositionType result = PetPositionType.TYPE_WALK;
		switch (imageIndex)
		{
		case 7:
		case 8:
		case 15:
		case 17:
		case 23:
			result = PetPositionType.TYPE_FLY;
			break;
		}
		return result;
	}

	public static void settingRankByStatus(ref PetData data)
	{
		List<int> list = new List<int>
		{
			1,
			2,
			3,
			4,
			5
		};
		int num = 0;
		switch (data.rank)
		{
		case EquipmentRank.TYPE_NORMAL:
			num = 1;
			break;
		case EquipmentRank.TYPE_UNIQUE:
			num = 2;
			break;
		case EquipmentRank.TYPE_LEGENDARY:
			num = 3;
			break;
		case EquipmentRank.TYPE_SUPERLEGENDARY:
			num = 5;
			break;
		}
		for (int i = 0; i < num; i++)
		{
			int index = Random.Range(0, list.Count);
			switch (list[index])
			{
			case 1:
				data.powerPer = 5f;
				break;
			case 2:
				data.criticalPer = 5f;
				break;
			case 3:
				data.shieldPer = 5f;
				break;
			case 4:
				data.speedPer = 5f;
				break;
			case 5:
				data.hpPer = 5f;
				break;
			}
			list.RemoveAt(index);
		}
	}
}
