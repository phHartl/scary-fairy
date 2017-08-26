using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedTrial : MonoBehaviour {

    public GameObject finish;
    public const float TRIAL_TIME = 7f;
    public FinishLine finishLine;
    private AudioSource clock;

    private void Start()
    {
        clock = gameObject.GetComponent<AudioSource>();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(startCountdown());
            clock.Play();
        }
    }

    private IEnumerator startCountdown()
    {
        finish.SetActive(false);
        yield return new WaitForSeconds(TRIAL_TIME);
        if (!finishLine.finishReached)
        {
            finish.SetActive(true);
        }
    }
}
