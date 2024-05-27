using UnityEngine;

namespace Percent
{
	public class Deleter : MonoBehaviour
	{
		public bool playerPref;

		public bool imageCache;

		private void Awake()
		{
			if (playerPref)
			{
				PlayerPrefs.DeleteAll();
			}
			if (imageCache)
			{
				new ImageTool().deleteAllCache();
			}
		}
	}
}
