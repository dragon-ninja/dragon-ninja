using System.Collections.Generic;
using UnityEngine;

public class ParticleStartColorChange : MonoBehaviour
{
	public List<ParticleSystem> listParticles = new List<ParticleSystem>();

	public void settingColor(Color color)
	{
		int count = listParticles.Count;
		ParticleSystem.MainModule main;
		for (int i = 0; i < count; i++)
		{
			main = listParticles[i].main;
			main.startColor = color;
		}
	}
}
