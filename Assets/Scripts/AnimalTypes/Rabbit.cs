using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rabbit : Animal{


    //currently static setting up of genes, will be automated later
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
        return this.age;
    }

    public override bool GetGender(){
        return this.male;
    }
    public override bool GetMating(){
        return this.mating;
    }

    public override string GetAction(){
        return this.currentAction.ToString();
    }
}
