using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rabbit : Animal{
    public override string GetName(){
        return "Rabbit";
    }

    

    public override bool GetGender(){
        return base.GetGender();
    }
}
