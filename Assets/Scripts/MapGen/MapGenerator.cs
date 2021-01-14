using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour{

    public LayerMask groundLayer;

    private int maxTrees;
    private int maxPopulation;
    private int currentTrees = 0;
    private Vector3 spawnPosition;

    private bool spawning;
    System.Random prng;


    void Start(){
        spawning = false;
        prng = new System.Random();
    }

    // Update is called once per frame
    void Update(){
        /*DEBUG
        if(Input.GetKeyDown(KeyCode.E)){
            ApplicationControl.sceneSwitch = true;
            //GetFreePosition();
            
        }*/
        if(ApplicationControl.sceneSwitch && !spawning){
            StartSpawning();
            
        }
        if(spawning){
            
            if(currentTrees < maxTrees){
                if(GetFreePosition()){
                    
                    Transform tree = Instantiate(Resources.Load<Transform>("Assets/EntityTree"));
                    tree.position = new Vector3(spawnPosition.x, -0.5f, spawnPosition.z);
                    currentTrees++;
                }
           }
        }
    }

    private void StartSpawning(){
        Debug.Log("HH");
        maxTrees = Mathf.RoundToInt(113 * ApplicationControl.maxTrees);
        spawning = true;
    }

    //Tries to find a free position to spawn a tree into
    private bool GetFreePosition(){
        Vector3 position = new Vector3(prng.Next(-35, 35), 0f, prng.Next(-5, 65));
        Collider[] cols = Physics.OverlapSphere(position, 0.5f, groundLayer);
        if(cols.Length > 0){
            Environment tile = cols[0].GetComponent<Environment>();
            if(tile.waterTile) return false;
            else if(tile.GetEntityBool("EntityTree") ) return false;
            else {
                spawnPosition = position;
                tile.walkable = false;
                return true;
            }
        }
        else return false;
    }

    private void OnDrawGizmos() {
        Gizmos.DrawSphere(spawnPosition, 1f);
    }
}
