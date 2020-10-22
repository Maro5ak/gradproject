using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Animal : LivingEntity{

    public LayerMask groundMask;
    
    private const float betweenActionCooldown = 2f;
    private const float distance = 1f;
    private const int maxTries = 5;

    private Vector3 target;
    private Transform foodTransform;
    private Action currentAction;
    private float lastAction;
    private bool moving;
    private float thirst = 10f, hunger = 10f;
    private bool recentlyDrank, recentlyAte;
    private float lastFood;
    private float lastDrink;
    private int numOfTries;
    
    //Movement data
    float moveTime;
    float moveSpeed = 1.5f;
    float moveArcHeightFactor;
    float moveSpeedFactor;
    float moveArcHeight = .2f;
    Vector3 startPosition;
    Vector3 lastWaterSensed;

    //math, for better performance

    const float sqrtTwo = 1.4142f;
    const float oneOverSqrtTwo = 1 / sqrtTwo;
    

    System.Random prng;
    

    private void Start() {
        recentlyAte = false;
        recentlyDrank = false;
        prng = new System.Random();
        currentAction = Action.Exploring;
        ChooseNextAction();
    }
    
    protected void Update() {
        thirst -= Time.deltaTime * 0.5f;
        hunger -= Time.deltaTime * 0.5f;
        //Debug.Log("Thirst: " + thirst);

        //cooldown so the entity isn't eating or drinking all the time
        if(Time.time - lastFood > 10f){
            recentlyAte = false;
        }
        if(Time.time - lastDrink > 10f){
            recentlyDrank = false;
        }

        if(moving){
            MoveTo();
        }
        else{
            HandleInteraction();    
            float timeSinceLastAction = Time.time - lastAction;
            if(timeSinceLastAction > betweenActionCooldown && (currentAction != Action.Eating && currentAction != Action.Drinking)){
                ChooseNextAction();
            }
        }
    }

    // chooses next action based on current state of needs; Water > Food
    protected void ChooseNextAction(){
        lastAction = Time.time;

        if(thirst < 3 && !recentlyDrank && (currentAction != Action.Drinking && currentAction != Action.LookingForFood)){
            numOfTries = 0;
            LookForWater();
        }
        else if(hunger < 3f && !recentlyAte && (currentAction != Action.Eating && currentAction != Action.LookingForWater)){
            numOfTries = 0;
            
            LookForFood();
        }

        /*else if(currentAction != Action.Eating || currentAction != Action.Drinking){
            currentAction = Action.Exploring;
        }*/
        DoAction();
        
    }

    // tries to find walkable path; water tiles to be walkable with a penalty
    protected void ChoosePath(){
        target = GetTarget();
        Collider[] cols = Physics.OverlapSphere(target, 0.7f, groundMask);

        if(cols.Length != 0 && cols[0].GetComponent<Environment>().walkable){
            
            StartMove();
        }
        
        else{
            ChoosePath();
        }
    }
    // Looking for water method, finds nearest water source and moves towards the water source
    // remembers the last water source and if new water source is more further away then moves to the last one it remembered
    protected void LookForWater(){
        target = GetTarget();
        Collider[] cols = Physics.OverlapSphere(target, 0.7f, groundMask);
        if(numOfTries == maxTries){
            ChoosePath();
        }
        else if(cols.Length != 0){
            if(cols[0].GetComponent<Environment>().waterTile){
                // if the last water source is further away than a new one, update to the closest one
                lastWaterSensed = Vector3.Distance(target, this.transform.position) < Vector3.Distance(target, lastWaterSensed) ? target : lastWaterSensed;
                target += (startPosition - target).normalized * distance;
                target.y = this.transform.position.y;
                currentAction = Action.LookingForWater;
                StartMove();
            }
            else {
                numOfTries++;
                LookForWater();
            }
        }
        else{
            LookForWater();
        }
    }
    // Looking for food method, creature tries to find a food target and moves over to the tile
    protected void LookForFood(){
        target = GetTarget();
        Collider[] cols = Physics.OverlapSphere(target, 0.7f, groundMask);
        LivingEntity food = null;
        if(numOfTries == maxTries) {
            ChoosePath();
        }
        else if(cols.Length != 0) {
            food = cols[0].GetComponent<Environment>().GetEntity("plant");
            if(food != null){
                target = food.transform.position;
                target += (startPosition - target).normalized * distance;
                target.y = this.transform.position.y;
                foodTransform = food.transform;
                currentAction = Action.LookingForFood;
                StartMove();
            }
            else{
                numOfTries++;
                LookForFood();
            }
        }
        else{
            LookForFood();
        }
        
        
    }

    // starts movement method, calculates jump height and relative movement speed based on movement angle
    protected void StartMove(){
        moving = true;
        transform.LookAt(target);
        startPosition = transform.position;
            
        bool diagonalMove = Mathf.Sqrt(Vector3.Distance(startPosition, target)) > 1;
        moveArcHeightFactor = (diagonalMove) ? sqrtTwo : 1;
        moveSpeedFactor = (diagonalMove) ? oneOverSqrtTwo : 1;
    }

    private void MoveTo(){
        // Move in an arc 
        
        moveTime = Mathf.Min (1, moveTime + Time.deltaTime * moveSpeed * moveSpeedFactor);
        float height = (1 - 4 * (moveTime - 0.5f) * (moveTime - 0.5f)) * moveArcHeight * moveArcHeightFactor;
        transform.position = Vector3.Lerp (startPosition, target, moveTime) + Vector3.up * height;

        // Finished moving
        if (moveTime >= 1) {
            moving = false;
            moveTime = 0;
        }
        
    }

    // determines what to do based on currently chosen action earlier
    protected void DoAction(){
        switch(currentAction){
            
            case Action.LookingForFood:
                if(Vector3.Distance(this.transform.position, foodTransform.position) <= 3f){
                    currentAction = Action.Eating;
                    Debug.Log("Go");
                }
                else{
                    currentAction = Action.LookingForFood;
                }
                break;
            case Action.LookingForWater:
                if(Vector3.Distance(this.transform.position, lastWaterSensed) <= 3f){
                    currentAction = Action.Drinking;
                }
                else{
                    currentAction = Action.LookingForWater;
                }
                break;
            case Action.Exploring:
                ChoosePath();
                break;
        }
    }

    // handles current interacion and keeps on doing until satisfied
    protected void HandleInteraction(){
        
        if(currentAction == Action.Eating){
            if(foodTransform != null){
                if(hunger <= 10){
                    hunger += Time.deltaTime * 1.5f;
                }
                else{
                    recentlyAte = true;
                    lastFood = Time.time;
                    currentAction = Action.Exploring;
                }
            }
        }
        if(currentAction == Action.Drinking){
            if(thirst <= 10){
                thirst += Time.deltaTime * 1.5f;
            }
            else{
                recentlyDrank = true;
                lastDrink = Time.time;
                currentAction = Action.Exploring;
            }
        }
    }

    // finds target based on position and view distance
    private Vector3 GetTarget(){
        return new Vector3
            (
                prng.Next((int)transform.position.x - 5, (int)transform.position.x + 5), 
                transform.position.y, 
                prng.Next((int)transform.position.z - 5, (int)transform.position.z + 5)
            );
    }  

    // debug 
    private void OnDrawGizmos() {
        Handles.Label(transform.position, currentAction.ToString());
        Gizmos.DrawSphere(target, 0.5f);
    }
}
