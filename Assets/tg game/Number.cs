using UnityEngine;

public class Number : MonoBehaviour
{
	public SpriteRenderer render;

	public void setNumber(int i, Color color)
	{
		if (i < 10)
		{
			Sprite sprite = Singleton<AssetManager>.Instance.LoadSprite("Numbers/" + i.ToString());
			render.sprite = sprite;
			render.color = color;
		}
	}
}
