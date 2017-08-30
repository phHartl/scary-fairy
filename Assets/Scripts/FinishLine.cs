using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour {

    [HideInInspector]public bool finishReached = false;


    public void OnTriggerEnter2D(Collider2D collision)
    {
        finishReached = true;
    }

}
