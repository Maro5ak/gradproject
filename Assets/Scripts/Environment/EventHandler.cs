using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventHandler : MonoBehaviour{
    public delegate void TimeAdvanceHandler();
    public static event TimeAdvanceHandler OnTimeAdvanced;

    public static void TimeAdvance(){
        if(OnTimeAdvanced != null) {
            OnTimeAdvanced();
        }
    }
}
