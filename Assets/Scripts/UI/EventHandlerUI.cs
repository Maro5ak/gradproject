using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventHandlerUI : MonoBehaviour{
    public delegate void ActionChangeHandler(Transform entity, string action);
    public static event ActionChangeHandler OnActionChange;

    public static void ActionChanged(Transform entity, string action){
        if(OnActionChange != null) {
            OnActionChange(entity, action);
        }
    }





}
