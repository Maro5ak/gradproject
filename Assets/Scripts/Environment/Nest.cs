using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nest : LivingEntity, IEnvironment{
    public LayerMask groundLayerMask;
    private Transform tile;
    private bool nestFull;
    private Transform owner;

    public override string GetName(){
        return "nest";
    }

    private void Start() {
        owner = null;
        nestFull = false;
        AddToTile();
    }

    


    public void AddOwner(Transform transform){
        owner = transform;
    }

    public void RemoveOwner(){
        owner = null;
    }

    public Transform GetOwner(){
        return owner;
    }



    public void AddToTile(){
        tile = Physics.OverlapSphere(this.transform.position, 1f, groundLayerMask)[0].transform;
        tile.GetComponent<Environment>().AddEntity(this);
    }
}
