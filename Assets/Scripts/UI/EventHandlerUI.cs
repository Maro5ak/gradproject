using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventHandlerUI : MonoBehaviour{
    public delegate void ActionChangeHandler();
    public static event ActionChangeHandler OnActionChange;

    public delegate void AgeChangeHandler();
    public static event AgeChangeHandler OnAgeChange;

    public delegate void LoadingScreenHandler();
    public static event LoadingScreenHandler OnLoading;

    public static void ActionChanged(){
        if(OnActionChange != null) {
            OnActionChange();
        }
    }

    public static void AgeChanged(){
        if(OnAgeChange != null){
            OnAgeChange();
        }
    }

    public static void Loading(){
        if(OnLoading != null){
            OnLoading();
        }
    }





}
