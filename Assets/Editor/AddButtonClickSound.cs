//=====================================================
// - FileName:      AddButtonClickSound.cs
// - Author:       Autumn
// - CreateTime:    2019/05/28 10:59:23
// - Email:         543761701@qq.com
// - Description:   
// -  (C) Copyright 2019, webeye,Inc.
// -  All Rights Reserved.
//======================================================
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.EventSystems;

namespace GameWish.Game
{
    public class AddButtonClickSound : ScriptableObject
    {
        [MenuItem("Tools/ButtonAudio/AddButtonSoundInScene")]
        static void AddSoundForButton()
        {
            /*GetAllSelectGo((child) =>
            {
                if (child.GetComponent<Button>() != null)
                {
                    Debug.Log("{0} 按钮添加音效{1}！" + child.name + " " + child.GetComponent<SoundButtonTool>() == null ? "成功" : "失败（已经添加过，建议先clearAll）");
                    child.AddComponent<SoundButtonTool>();
                    if (child.name.Contains("Close") || child.name.Contains("close") || child.name.Contains("Back") || child.name.Contains("back"))
                    {
                        child.GetComponent<SoundButtonTool>().clickSound = ButtonClickSound.Close;
                    }
                }
            });*/

            GameObject[] go;
            go = FindObjectsOfType(typeof(GameObject)) as GameObject[];
            foreach (GameObject child in go) 
            {
                if (child.GetComponent<Button>() != null) 
                {
                    Debug.Log("{0} 按钮添加音效{1}！" + child.name + " " + child.GetComponent<SoundButtonTool>() == null ? "成功" : "失败（已经添加过，建议先clearAll）");
                    child.AddComponent<SoundButtonTool>();
                    if (child.name.Contains("Close") 
                        || child.name.Contains("close") 
                        || child.name.Contains("Back") 
                        || child.name.Contains("back")
                        || child.name.Contains("panel")
                        || child.name.Contains("Panel")) 
                    {
                       child.GetComponent<SoundButtonTool>().clickSound = ButtonClickSound.Close;
                    }
               }
            }
        }

        [MenuItem("Tools/ButtonAudio/ClearAllButtonSoundInScene")]
        static void DeleteSoundForButton()
        {
           /* GetAllSelectGo((child) =>
            {
                if (child.GetComponent<Button>() != null)
                {
                    if (child.GetComponent<SoundButtonTool>() != null)
                    {
                        DestroyImmediate(child.GetComponent<SoundButtonTool>());
                        Debug.Log("{0} 按钮移除音效成功！"+ child.name);
                    }
                }
            });*/
            GameObject[] go;
            go = FindObjectsOfType(typeof(GameObject)) as GameObject[];
            foreach (GameObject child in go)
            {
                if (child.GetComponent<SoundButtonTool>() != null)
                {
                    DestroyImmediate(child.GetComponent<SoundButtonTool>());
                    Debug.Log("{0} 按钮移除音效成功！" + child.name);
                }
            }
        }

       /* static void GetAllSelectGo(Action<GameObject> handle)
        {
            GameObject[] go = Selection.gameObjects;
            foreach (GameObject child in go)
            {
                Debug.Log("parent_{0}"+ child.name);
                child.IterateGameObject(handle);
            }
        }*/
    }
}
