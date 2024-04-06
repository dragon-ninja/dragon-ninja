//=====================================================
// - FileName:      SoundButtonTool.cs
// - Author:       Autumn
// - CreateTime:    2019/05/28 10:51:42
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

namespace GameWish.Game
{
    public enum GameSound
    {
        EFFECT_CLICK_BTN,
        EFFECT_CLOSE_BTN,
    }


    public enum ButtonClickSound
    {
        Normal,
        Close,
    }

    [RequireComponent(typeof(Button))]
    public class SoundButtonTool : MonoBehaviour
    {
        public ButtonClickSound clickSound = ButtonClickSound.Normal;
        public AudioSource ac;
        void ClickSound()
        {
            switch (clickSound)
            {
                case ButtonClickSound.Normal:
                    ac.Play();
                    break;
                case ButtonClickSound.Close:
                    //ac.PlaySound(GameSound.EFFECT_CLOSE_BTN);
                    break;
            }
        }
        private void Awake()
        {
            Button btn = GetComponent<Button>();
            if (btn != null)
            {
                btn.onClick.AddListener(ClickSound);
            }
            ac = GetComponent<AudioSource>();
        }
    }
}




