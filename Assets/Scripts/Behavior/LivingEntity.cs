using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : MonoBehaviour{
    public virtual string GetName(){
        return null;
    }

    protected virtual void Die(CauseOfDeath cause){
        Destroy(gameObject);
    }
}
