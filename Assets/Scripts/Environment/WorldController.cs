﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour{
    private float startTime;

    //constant value for 60 seconds
    const float yearLength = 60f;

    
    private void Start() {
        startTime = 0f;
    }

    //the counting of a year, set by the yearLentght constant. Each time 60 seconds passes, year advances
    private void Update() {
        float timePassed = Time.time - startTime;
        if(timePassed >= yearLength){
            EventHandler.TimeAdvance();
            startTime = Time.time;
        }
    }
}
