using UnityEngine;

public struct BackgroundSubItem
{
	public Sprite image;

	public SubItem state;

	public float[] supData;

	public Vector3 position;

	public Vector2 scrollRandomPosition;

	public BackgroundSubItem(Sprite img, SubItem st, Vector3 pos, float[] sd)
	{
		this = default(BackgroundSubItem);
		image = img;
		state = st;
		position = pos;
		supData = sd;
	}
}
