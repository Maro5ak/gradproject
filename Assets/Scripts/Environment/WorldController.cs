using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour{
    private float startTime;

    private void Awake() {
        Application.targetFrameRate = 60;
    }
    private void Start() {
        startTime = 0f;
    }

    private void Update() {
        float timePassed = Time.time - startTime;
        if(timePassed >= 10){
            EventHandler.TimeAdvance();
            startTime = Time.time;
        }
    }
}
