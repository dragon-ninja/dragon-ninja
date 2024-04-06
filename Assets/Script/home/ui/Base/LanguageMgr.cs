using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class LanguageMgr
{
    public static LanguageMgr Instance;

    public static Language lge = Language.Arabic;

    private Dictionary<string, string> LanguageMap;

    public LanguageMgr()
    {
        InitLanguage();
    }

    public static LanguageMgr Get()
    {
        if (Instance == null)
            Instance = new LanguageMgr();
        return Instance;
    }

    public void InitLanguage()
    {
        TextAsset ta = Resources.Load<TextAsset>("Language/Arabic");
        LanguageMap = JsonConvert.DeserializeObject<Dictionary<string, string>>(ta.text);
    }

    public string GetText(string TextName)
    {
        return LanguageMap[TextName];
    }
}
