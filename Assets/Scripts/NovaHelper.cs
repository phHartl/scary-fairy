using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class is needed, because an animation event is always called to the object, and cannot be called to a parent obj
public class NovaHelper : MonoBehaviour {

    private Fairy fairy;

    public void Awake()
    {
        fairy = GetComponentInParent<Fairy>();
    }
    //Calls the function in fairy parent to close animation
    private void AttackOver()
    {
        fairy.AttackOver();
    }
}
