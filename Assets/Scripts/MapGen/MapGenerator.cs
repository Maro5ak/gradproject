using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour{

    public LayerMask groundLayer;
    private ApplicationControl aplication = new ApplicationControl();

    private int maxTrees;
    private int maxBushes;
    private int maxRabbitPopulation;
    private int maxSheepPopulation;
    private int maxPlants;
    private int currentSheepPopulation = 0;
    private int currentSheepMales = 0;
    private int currentSheepFemales = 0;
    private int currentRabbitPopulation = 0;
    private int currentRabbitMales = 0;
    private int currentRabbitFemales = 0;
    private int maxNests;
    private int currentNests;
    private int currentPlants = 0;
    private int currentTrees = 0;
    private int currentBushes = 0;
    private Vector3 spawnPosition;

    private bool spawning;
    private bool respawning;
    System.Random prng;


    void Start(){
        EventHandler.OnTimeAdvanced += RespawnPlants;

        respawning = false;
        spawning = false;
        prng = new System.Random();
    }

    // Update is called once per frame
    void Update(){
        //DEBUG
        /*if(Input.GetKeyDown(KeyCode.E)){
            ApplicationControl.sceneSwitch = true;
            ApplicationControl.maxBushes = 0.3f;
            ApplicationControl.maxTrees = 0.2f;
            ApplicationControl.maxPlants = 0.5f;
            ApplicationControl.maxPopulation = 10;
            ApplicationControl.malePop = 5;
            ApplicationControl.femalePop = 5;
            //spawning = true;
            //GetFreePosition();
            
        }
        */
        if(currentTrees == maxTrees && currentBushes == maxBushes && currentPlants == maxPlants && currentNests == maxNests && currentRabbitPopulation == maxRabbitPopulation && currentSheepPopulation == maxSheepPopulation && spawning){
            EventHandlerUI.Loading();
            spawning = false;
        }

        if(ApplicationControl.sceneSwitch && !spawning){
            StartSpawning();
            
        }

        if(respawning){
            if(currentPlants == maxPlants){
                respawning = false;
            }
            else{
               if(GetFreePlantPosition()){
                   Transform plant = Instantiate(Resources.Load<Transform>("Assets/EntityPlant"));
                   plant.position = new Vector3(spawnPosition.x, 0f, spawnPosition.z);
                   currentPlants++;
               }
            }
        }

        if(spawning){
            
            if(currentTrees < maxTrees){
                if(GetFreeTreePosition()){
                    Transform tree = null;
                    if(prng.Next(0, 2) == 1) tree = Instantiate(Resources.Load<Transform>("Assets/EntityTree2")); 
                    else tree = Instantiate(Resources.Load<Transform>("Assets/EntityTree")); 
                    tree.position = new Vector3(spawnPosition.x, -0.5f, spawnPosition.z);
                    currentTrees++;
                }
           }
           else if(currentBushes < maxBushes){
               if(GetFreeBushPosition()){
                   Transform bush = Instantiate(Resources.Load<Transform>("Assets/EntityBush"));
                   bush.position = new Vector3(spawnPosition.x, 0.5f, spawnPosition.z);
                   currentBushes++;
               }
           }
           else if(currentPlants < maxPlants){
               if(GetFreePlantPosition()){
                   Transform plant = Instantiate(Resources.Load<Transform>("Assets/EntityPlant"));
                   plant.position = new Vector3(spawnPosition.x, 0f, spawnPosition.z);
                   currentPlants++;
               }
           }
           else if(currentRabbitPopulation < maxRabbitPopulation){
               if(GetFreeAnimalPosition()){
                   Transform rabbit = Resources.Load<Transform>("Assets/AnimalRabbit");
                   Instantiate(rabbit, spawnPosition, default);                    
                   if(currentRabbitMales < ApplicationControl.rabbitMalePop){    
                       rabbit.GetComponent<Rabbit>().SetGender(true);
                       currentRabbitMales++;
                   }
                   else if(currentRabbitFemales < ApplicationControl.rabbitFemalePop){
                       rabbit.GetComponent<Rabbit>().SetGender(false);
                       currentRabbitFemales++;
                   }
                   currentRabbitPopulation++;
               }
           }
           else if(currentSheepPopulation < maxSheepPopulation){
               if(GetFreeAnimalPosition()){
                   Transform sheep = Resources.Load<Transform>("Assets/AnimalSheep");
                   Instantiate(sheep, spawnPosition, default);                    
                   if(currentSheepMales < ApplicationControl.sheepMalePop){    
                       sheep.GetComponent<Sheep>().SetGender(true);
                       currentRabbitMales++;
                   }
                   else if(currentSheepFemales < ApplicationControl.sheepFemalePop){
                       sheep.GetComponent<Sheep>().SetGender(false);
                       currentSheepFemales++;
                   }
                   currentSheepPopulation++;
               }
           }
           else if(currentNests < maxNests){
               if(GetFreeNestPosition()){
                   Transform nest = Instantiate(Resources.Load<Transform>("Assets/EntityNest"));
                   nest.position = new Vector3(spawnPosition.x, 0f, spawnPosition.z);
                   currentNests++;
               }
           }
        }
    }

    private void StartSpawning(){
        Debug.Log("HH");
        maxTrees = Mathf.RoundToInt(113 * ApplicationControl.maxTrees);
        maxBushes = Mathf.RoundToInt(200 * ApplicationControl.maxBushes);
        maxPlants = Mathf.RoundToInt(800 * ApplicationControl.maxPlants);
        maxRabbitPopulation = (int)ApplicationControl.rabbitMaxPopulation;
        maxSheepPopulation = (int)ApplicationControl.sheepMaxPopulation;

        maxNests = (ApplicationControl.rabbitFemalePop + ApplicationControl.sheepFemalePop) / 2;
        spawning = true;
    }

    //Tries to find a free position to spawn a tree into
    private bool GetFreeTreePosition(){
        Vector3 position = new Vector3(prng.Next(-35, 35), 0f, prng.Next(-5, 65));
        Collider[] cols = Physics.OverlapSphere(position, 0.5f, groundLayer);
        if(cols.Length > 0){
            Environment tile = cols[0].GetComponent<Environment>();
            if(tile.waterTile) return false;
            else if(tile.GetEntityBool("EntityTree") || tile.GetEntityBool("EntityTree2")) return false;
            else {
                spawnPosition = position;
                tile.walkable = false;
                return true;
            }
        }
        else return false;
    }

    private bool GetFreeBushPosition(){
        Vector3 position = new Vector3(prng.Next(-35, 35), 0f, prng.Next(-5, 65));
        Collider[] cols = Physics.OverlapSphere(position, 0.5f, groundLayer);
        if(cols.Length > 0){
            Environment tile = cols[0].GetComponent<Environment>();
            if(tile.waterTile) return false;
            else if(tile.GetEntityCount("EntityBush") > 2) return false;
            else{
                spawnPosition = position;
                return true;
            }
        }    
        else return false;
    }

    private bool GetFreePlantPosition(){
        Vector3 position = new Vector3(prng.Next(-35, 35), 0f, prng.Next(-5, 65));
        Collider[] cols = Physics.OverlapSphere(position, 0.5f, groundLayer);
        if(cols.Length > 0){
            Environment tile = cols[0].GetComponent<Environment>();
            if(tile.waterTile) return false;
            else if(tile.GetEntityCount("EntityPlant") > 4) return false;
            else{
                spawnPosition = position;
                return true;
            }
        }    
        else return false;
    }
    
    private bool GetFreeAnimalPosition(){
        Vector3 position = new Vector3(prng.Next(-25, 25), 0.3f, prng.Next(5, 55));
        Debug.Log("DAMIANY: " + position);
        Collider[] cols = Physics.OverlapSphere(position, 0.5f, groundLayer);
        if(cols.Length > 0){
            Environment tile = cols[0].GetComponent<Environment>();
            if(tile.waterTile) return false;
            else if(!tile.walkable) return false;
            else if(tile.GetEntity("Rabbit") || tile.GetEntity("Sheep")) return false;
            else{
                spawnPosition = position;
                return true;
            }
        }
        else return false;
    }

    private bool GetFreeNestPosition(){
        Vector3 position = new Vector3(prng.Next(-25, 25), 0f, prng.Next(5, 55));
        Collider[] cols = Physics.OverlapSphere(position, 0.5f, groundLayer);
        if(cols.Length > 0){
            Environment tile = cols[0].GetComponent<Environment>();
            if(tile.waterTile) return false;
            else if(!tile.walkable) return false;
            else if(tile.GetEntity("EntityNest")) return false;
            else{
                spawnPosition = position;
                return true;
            }
        }
        else return false;
    }

    private void RespawnPlants(){
        respawning = true;
        currentPlants = Mathf.RoundToInt(maxPlants / 5);
    }

    private void OnDrawGizmos() {
        Gizmos.DrawSphere(spawnPosition, 1f);
    }
}
