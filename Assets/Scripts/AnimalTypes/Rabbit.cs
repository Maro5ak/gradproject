using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rabbit : Animal{

    protected void Start() {
        genes.Add("A");
        genes.Add("B");
        CalculateLiveSpan();
    }

    public override string GetName(){
        return "Rabbit";
    }

    public override int GetAge()
    {
        return base.GetAge();
    }

    public override bool GetGender(){
        return base.GetGender();
    }
}
