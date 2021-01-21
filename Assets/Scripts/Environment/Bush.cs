using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bush : LivingEntity, IEnvironment{
    public LayerMask groundLayerMask;
    private Transform tile;

    public override string GetName(){
        return "EntityBush";
    }

    private void Start() {
        AddToTile();
    }


    public void AddToTile(){
        tile = Physics.OverlapSphere(this.transform.position, 1f, groundLayerMask)[0].transform;
        tile.GetComponent<Environment>().AddEntity(this);
    }

    protected override void Die(CauseOfDeath cause){
        tile.GetComponent<Environment>().RemoveEntity(this);
        Destroy(this.gameObject);
    }
}
