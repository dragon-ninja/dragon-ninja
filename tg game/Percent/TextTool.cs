using System.IO;
using UnityEngine;

namespace Percent
{
	public class TextTool
	{
		internal static readonly string CACHE_PATH = Application.persistentDataPath + "/cache";

		private readonly string SLASH = "/";

		private readonly string EX_PRIVACY_POLICY = ".text";

		internal TextTool()
		{
			if (!Directory.Exists(CACHE_PATH))
			{
				Directory.CreateDirectory(CACHE_PATH);
			}
		}

		internal bool isCacheExist(string resourceId)
		{
			string path = resolvePath(resourceId);
			if (isFileExist(path))
			{
				return true;
			}
			return false;
		}

		private bool isFileExist(string path)
		{
			if (new FileInfo(path).Exists)
			{
				return true;
			}
			return false;
		}

		internal void saveText(string resourceId, string text)
		{
			try
			{
				StreamWriter streamWriter = File.CreateText(resolvePath(resourceId));
				streamWriter.Write(text);
				streamWriter.Close();
			}
			catch
			{
				Logger.error("Save cache text FAIL.");
			}
		}

		private string resolvePath(string fileName)
		{
			return CACHE_PATH + SLASH + fileName + EX_PRIVACY_POLICY;
		}

		internal string loadTextFromCache(string resourceId)
		{
			string text = resolvePath(resourceId);
			if (!isFileExist(text))
			{
				Logger.error("There is no cached file.");
				return string.Empty;
			}
			return loadText(text);
		}

		internal string loadText(string filePath)
		{
			if (isFileExist(filePath))
			{
				try
				{
					return new StreamReader(filePath).ReadToEnd();
				}
				catch
				{
					Logger.error("Cached file load FAIL");
				}
			}
			return string.Empty;
		}

		internal void deleteAllCache()
		{
			if (Directory.Exists(CACHE_PATH))
			{
				Directory.Delete(CACHE_PATH, recursive: true);
			}
		}
	}
}
