using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// POLYMORPHISM BIACH
public class LivingEntity : MonoBehaviour{
    public virtual string GetName(){
        return null;
    }

    protected virtual void Die(CauseOfDeath cause){
        Destroy(gameObject);
    }
}
