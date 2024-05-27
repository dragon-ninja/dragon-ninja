using System.Collections.Generic;
using UnityEngine;

public struct BackgroundItem
{
	public Sprite image;

	public float movePercent;

	public float zorder;

	public bool top;

	public ItemAction action;

	public float actionSubData;

	public List<BackgroundItemParticle> listParticles;

	public BackgroundItem(Sprite img, float per, float z, bool t, ItemAction a = ItemAction.ACTION_NOT, float sd = 0f, List<BackgroundItemParticle> particles = null)
	{
		this = default(BackgroundItem);
		image = img;
		movePercent = per;
		zorder = z;
		top = t;
		action = a;
		actionSubData = sd;
		if (particles == null)
		{
			listParticles = null;
		}
		else
		{
			listParticles = new List<BackgroundItemParticle>(particles);
		}
	}
}
