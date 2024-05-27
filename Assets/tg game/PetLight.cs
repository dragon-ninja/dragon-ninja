using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PetLight : MonoBehaviour
{
	[Header("SPRITES")]
	public List<Sprite> listLightSprites = new List<Sprite>();

	public List<Sprite> listDotSprites = new List<Sprite>();

	public List<Sprite> listRankSprites = new List<Sprite>();

	[Header("OBJECTS")]
	public List<PetLightLine> listLightImages = new List<PetLightLine>();

	public List<PetLightDot> listDotImages = new List<PetLightDot>();

	public Image lightBack;

	public Image rankText;

	public void openLight(EquipmentRank rank)
	{
		base.gameObject.SetActive(value: true);
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		int num4 = 0;
		int num5 = 0;
		bool active = false;
		int count = listLightImages.Count;
		int count2 = listDotImages.Count;
		switch (rank)
		{
		case EquipmentRank.TYPE_NORMAL:
			num4 = 7;
			num5 = 0;
			active = false;
			num = 255f;
			num2 = 255f;
			num3 = 255f;
			break;
		case EquipmentRank.TYPE_UNIQUE:
			num4 = count;
			num5 = 0;
			active = true;
			num = 111f;
			num2 = 58f;
			num3 = 192f;
			break;
		case EquipmentRank.TYPE_LEGENDARY:
			num4 = count;
			num5 = 8;
			active = true;
			num = 200f;
			num2 = 80f;
			num3 = 51f;
			break;
		case EquipmentRank.TYPE_SUPERLEGENDARY:
			num4 = count;
			num5 = count2;
			active = true;
			num = 255f;
			num2 = 208f;
			num3 = 64f;
			break;
		}
		Color color = new Color(num / 255f, num2 / 255f, num3 / 255f, 1f);
		for (int i = 0; i < count; i++)
		{
			if (i < num4)
			{
				listLightImages[i].startAction(listLightSprites, color);
			}
			else
			{
				listLightImages[i].gameObject.SetActive(value: false);
			}
		}
		for (int j = 0; j < count2; j++)
		{
			if (j < num5)
			{
				listDotImages[j].startAction(listDotSprites, color, new Vector3(0f, 0f, 0f), 300f, 300f);
			}
			else
			{
				listDotImages[j].gameObject.SetActive(value: false);
			}
		}
		lightBack.color = color;
		lightBack.gameObject.SetActive(active);
		rankText.sprite = listRankSprites[(int)rank];
		rankText.SetNativeSize();
		rankText.color = new Color(1f, 1f, 1f, 0f);
		rankText.DOFade(1f, 0.5f).SetDelay(1f);
	}

	public void closeLight()
	{
		base.gameObject.SetActive(value: false);
	}
}
