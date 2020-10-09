using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment : MonoBehaviour{

    public bool walkable;
    private List<LivingEntity> entitiesOnTop = new List<LivingEntity>();

    
    
   
    private void OnCollisionEnter(Collision collision) {
        entitiesOnTop.Add(collision.gameObject.GetComponent<LivingEntity>());
    }
    private void OnCollisionExit(Collision collision) {
        entitiesOnTop.Remove(entitiesOnTop.Find(x => x.GetName() == collision.transform.GetComponent<LivingEntity>().GetName()));

    }

    public void AddEntity(LivingEntity entity){
        entitiesOnTop.Add(entity);
        Debug.Log("Success");
    }

    
    public void RemoveEntity(LivingEntity entity){
        entitiesOnTop.Remove(entity);
    }

    public LivingEntity GetEntity(string entityName){
        return entitiesOnTop.Find(x => x.GetName() == entityName);

    }

    
}
