using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
	private static T _instance = null;

	private static object _syncobj = new object();

	private static bool appIsClosing = false;

	public static T Instance
	{
		get
		{
			if (appIsClosing)
			{
				return null;
			}
			lock (_syncobj)
			{
				if ((Object)_instance == (Object)null)
				{
					T[] array = UnityEngine.Object.FindObjectsOfType<T>();
					if (array.Length != 0)
					{
						_instance = array[0];
					}
					if (array.Length > 1)
					{
						UnityEngine.Debug.LogError("There is more than one " + typeof(T).Name + " in the scene.");
					}
					if ((Object)_instance == (Object)null)
					{
						string name = typeof(T).ToString();
						GameObject gameObject = GameObject.Find(name);
						if (gameObject == null)
						{
							gameObject = new GameObject(name);
						}
						_instance = gameObject.AddComponent<T>();
					}
				}
				return _instance;
			}
		}
	}

	protected virtual void OnApplicationQuit()
	{
		appIsClosing = true;
	}
}
