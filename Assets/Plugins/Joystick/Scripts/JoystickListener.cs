// Copyright (c) Bian Shanghai
// https://github.com/Bian-Sh/UniJoystick
// Licensed under the MIT license. See the LICENSE.md file in the project root for more information.

using zFrame.UI;
using UnityEngine;
public class JoystickListener : MonoBehaviour
{
    [SerializeField] Joystick joysticks ;

    private void Awake()
    {

        joysticks = GameObject.Find("Joystick-Left").GetComponent<Joystick>();

        joysticks.OnValueChanged.AddListener(v =>
        {
            if (v.magnitude != 0)
            {

            }
        });
        joysticks.OnPointerDown.AddListener(v =>
        {

        });
        joysticks.OnPointerUp.AddListener(v =>
        {

        });
    }
}
