using UnityEngine;

public class PortalParticleColor : MonoBehaviour
{
	public GameObject effect;

	public GameObject cutters;

	public GameObject orbGlow;

	public GameObject dust;

	public GameObject cloudBurst;

	public void settingColor(Color color)
	{
		Color color2 = color;
		if (color2.r < color2.g)
		{
			color2.r = 0f;
			if (color2.g < color2.b)
			{
				color2.g = 0f;
			}
			else
			{
				color2.b = 0f;
			}
		}
		else
		{
			color2.g = 0f;
			if (color2.r < color2.b)
			{
				color2.r = 0f;
			}
			else
			{
				color2.b = 0f;
			}
		}
		startColorChange(effect, color2);
		startColorChange(cutters, color, 139f);
		startColorChange(orbGlow, color);
		startColorChange(dust, color);
		startColorChange(cloudBurst, color, 22f);
		ParticleSystem.ColorOverLifetimeModule colorOverLifetime = dust.GetComponent<ParticleSystem>().colorOverLifetime;
		Gradient gradient = new Gradient();
		gradient.SetKeys(new GradientColorKey[2]
		{
			new GradientColorKey(Color.white, 0f),
			new GradientColorKey(color, 0.988f)
		}, new GradientAlphaKey[2]
		{
			new GradientAlphaKey(1f, 0f),
			new GradientAlphaKey(1f, 0.806f)
		});
		colorOverLifetime.enabled = true;
		colorOverLifetime.color = new ParticleSystem.MinMaxGradient(gradient);
	}

	private void startColorChange(GameObject obj, Color color, float a = 255f)
	{
		var main = obj.GetComponent<ParticleSystem>().main;
		main.startColor = new Color(color.r, color.g, color.b, a / 255f);
	}
}
