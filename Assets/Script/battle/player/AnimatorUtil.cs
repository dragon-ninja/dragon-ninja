using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorUtil : MonoBehaviour
{
    private Animator animator;

    private HashSet<string> animatorParams;

    public AnimatorUtil(Animator animator)
    {
        this.animator = animator;

        animatorParams = new HashSet<string>();
        foreach (var parameter in animator.parameters)
        {
            animatorParams.Add(parameter.name + ":" + parameter.type);
        }
    }

    public bool TrySet(string param, object value)
    {
        if (value is bool b && animatorParams.Contains(param + ":" + AnimatorControllerParameterType.Bool))
        {
            animator.SetBool(param, b);
            return true;
        }
        if (value is int i && animatorParams.Contains(param + ":" + AnimatorControllerParameterType.Int))
        {
            animator.SetInteger(param, i);
            return true;
        }
        if (value is float f && animatorParams.Contains(param + ":" + AnimatorControllerParameterType.Float))
        {
            animator.SetFloat(param, f);
            return true;
        }
        if (value == null && animatorParams.Contains(param + ":" + AnimatorControllerParameterType.Trigger))
        {
            animator.SetTrigger(param);
            return true;
        }
        return false;
    }

    
}
