using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class SkillButtonBehaviour : MonoBehaviour {

    public static bool Dashing = false;

    public void SkillButton_OnClick()
    {
        Debug.Log("Clicked Skill button");
        SkillButtonBehaviour.Dashing = true;
    }

	
}
