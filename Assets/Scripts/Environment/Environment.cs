using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment : MonoBehaviour{

    public bool walkable;
    public bool waterTile;
    private List<LivingEntity> entitiesOnTop = new List<LivingEntity>();

    
    
    // self explanatory methods, or I think so at least, actually Unity's internal methods 
    private void OnCollisionEnter(Collision collision) {
        entitiesOnTop.Add(collision.gameObject.GetComponent<LivingEntity>());
    }
    private void OnCollisionExit(Collision collision) {
        entitiesOnTop.Remove(entitiesOnTop.Find(x => x.GetName() == collision.transform.GetComponent<LivingEntity>().GetName()));

    }

    //everytime something "spawns" in the world, adds that to the list of entities on top
    public void AddEntity(LivingEntity entity){
        entitiesOnTop.Add(entity);
        Debug.Log("Successfully added: " + entity.GetName());
    }

    // everytime something "despawns" in the world, remove it from the list of entities on top
    public void RemoveEntity(LivingEntity entity){
        entitiesOnTop.Remove(entity);
    }

    //look for a specific entity in the list
    public LivingEntity GetEntity(string entityName){
        return entitiesOnTop.Find(x => x.GetName() == entityName);

    }

    public bool GetEntityBool(string entityName){
        return entitiesOnTop.Find(x => x.GetName() == entityName);
    }

    
}
