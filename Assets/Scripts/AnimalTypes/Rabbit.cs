using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rabbit : Animal{


    //gene randomization for each animal
    protected void Start() {
        for(int i = 0; i < 2; i++){
            switch(Random.Range(0, 2)){
                case 0: 
                    genes.Add("A");
                    break;
                case 1:
                    genes.Add("B");
                    break;
            }
        }        
        CalculateLiveSpan();
    }

    public override string GetName(){
        return "Rabbit";
    }

    public override void SetGender(bool gender)
    {
        this.male = gender;
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
