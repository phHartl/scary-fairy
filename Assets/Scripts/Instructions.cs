using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instructions : MonoBehaviour {

    public GameObject container;
    // This EventSystem needs to be disabled in order to use to Instructions own EventSystem
    public GameObject[] MenuComponents; // Canvas, EventSystem and Camera

	public void ShowInstructions()
    {
        MenuComponents[0].SetActive(false);
        MenuComponents[1].SetActive(false);
        MenuComponents[2].SetActive(false);
        for (int i = 0; i < container.transform.childCount; i++)
        {
            container.transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    public void HideInstructions()
    {
        for (int i = 0; i < container.transform.childCount; i++)
        {
            container.transform.GetChild(i).gameObject.SetActive(false);
        }
        MenuComponents[0].SetActive(true);
        MenuComponents[1].SetActive(true);
        MenuComponents[2].SetActive(true);
    }
}
