using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : LivingEntity{

    public LayerMask groundLayerMask;
    private Transform tile;

    public override string GetName(){
        return "plant";
    }

    private void Start() {
        AddToTile();
    }


    private void AddToTile(){
        tile = Physics.OverlapSphere(this.transform.position, 1f, groundLayerMask)[0].transform;
        tile.GetComponent<Environment>().AddEntity(this);
    }

    protected override void Die(CauseOfDeath cause){
        tile.GetComponent<Environment>().RemoveEntity(this);
        Destroy(this.gameObject);
    }
}
