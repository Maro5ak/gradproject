using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventHandlerUI : MonoBehaviour{
    public delegate void ActionChangeHandler(string action);
    public static event ActionChangeHandler OnActionChange;

    public static void ActionChanged(string action){
        if(OnActionChange != null) {
            OnActionChange(action);
        }
    }





}
