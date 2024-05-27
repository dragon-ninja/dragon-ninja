using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Version
{
	public abstract string Number
	{
		get;
	}

	public static int Compare(string a, string b)
	{
		int[] array = VersionStringToInts(a);
		int[] array2 = VersionStringToInts(b);
		for (int i = 0; i < Mathf.Max(array.Length, array2.Length); i++)
		{
			if (VersionPiece(array, i) < VersionPiece(array2, i))
			{
				return -1;
			}
			if (VersionPiece(array, i) > VersionPiece(array2, i))
			{
				return 1;
			}
		}
		return 0;
	}

	private static int VersionPiece(IList<int> versionInts, int pieceIndex)
	{
		if (pieceIndex >= versionInts.Count)
		{
			return 0;
		}
		return versionInts[pieceIndex];
	}

	private static int[] VersionStringToInts(string version)
	{
		int piece;
		return (from v in version.Split('.')
			select (!int.TryParse(v, out piece)) ? 0 : piece).ToArray();
	}
}
