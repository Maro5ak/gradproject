using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Animal : LivingEntity{

    public LayerMask groundMask;
    
    private float betweenActionCooldown = 2.5f;
    private const float distance = 1f;
    private const int maxTries = 5;

    protected List<string> genes = new List<string>();

    [SerializeField]
    protected bool male;
    protected int age, liveSpan;
    private Vector3 target;
    private Transform foodTransform, nestTransform;
    protected Action currentAction;
    private float lastAction;
    private bool moving;
    [HideInInspector]
    public float thirst = 10f, hunger = 10f;
    protected bool recentlyDrank = false, recentlyAte = false, canMate = true;
    private float lastFood;
    private float lastDrink;
    private float lastMate;
    protected float lastChild;
    private int numOfTries;
    protected bool mating, pregnant;
    
    private EventHandlerUI eventHandler;
    
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
    

    protected void Awake() {

        EventHandler.OnTimeAdvanced += AddAge;

        Physics.IgnoreLayerCollision(9, 9);


        age = 0;
        recentlyAte = false;
        recentlyDrank = false;
        pregnant = false;
        mating = false;
        prng = new System.Random();
        currentAction = Action.Exploring;
        ChooseNextAction();
    }

    protected void CalculateLiveSpan(){
        string dna = "";
        foreach(string s in genes){
            dna += s;
        }
        Debug.Log(dna);
        switch(dna){
            //Rabbit genes

            case "AB":
                liveSpan = 5;
                break;
            case "BA":
                liveSpan = 4;
                break;
            case "AA":
                liveSpan = 3;
                break;
            case "BB":
                liveSpan = 6;
                break;

            //Sheep Genes
            case "CD":
                liveSpan = 6;
                break;
            case "DC":
                liveSpan = 5;
                break;
            case "CC":
                liveSpan = 7;
                break;
            case "DD":
                liveSpan = 8;
                break;

        }
    
    }


    
    
    protected void Update() {
        thirst -= Time.deltaTime * 0.2f;
        hunger -= Time.deltaTime * 0.095f;
        if(thirst < 0f){
            Die(CauseOfDeath.Thirst);
        }

        if(hunger < 0f){
            Die(CauseOfDeath.Hunger);
        }

        if(age > liveSpan){
            Die(CauseOfDeath.OldAge);
        }

        if(!male && pregnant){
            //GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            string currentOwner = nestTransform.GetComponent<Nest>().GetOwner().GetComponent<Animal>().GetName();
            Transform baby;
            switch(currentOwner){
                case "Rabbit":
                    baby = Instantiate(Resources.Load<Transform>("Assets/AnimalRabbit"));
                    baby.transform.position = nestTransform.position;
                    if(Random.Range(0, 2) == 1) baby.GetComponent<Rabbit>().SetGender(true);
                    else baby.GetComponent<Rabbit>().SetGender(false);
                    break;
                case "Sheep":
                    baby = Instantiate(Resources.Load<Transform>("Assets/AnimalSheep"));
                    baby.transform.position = nestTransform.position;
                    if(Random.Range(0, 2) == 1) baby.GetComponent<Sheep>().SetGender(true);
                    else baby.GetComponent<Sheep>().SetGender(false);
                    break;
            }
            pregnant = false;
            
            
            //lastChild = Time.time;

        }
        
        if(Time.time - lastChild > 120f){
            canMate = true;
        }
        

        if(moving){
            MoveTo();
        }
        else{
            EventHandlerUI.ActionChanged();
            HandleInteraction();    
            float timeSinceLastAction = Time.time - lastAction;
            if(timeSinceLastAction > betweenActionCooldown && (currentAction != Action.Eating && currentAction != Action.Drinking && currentAction != Action.Nesting)){
                betweenActionCooldown = Random.Range(2f, 2.6f);
                ChooseNextAction();
            }
        }
    }

    // chooses next action based on current state of needs; Water > Food
    protected void ChooseNextAction(){

        lastAction = Time.time;
        if(hunger >= 8f && thirst >= 8f && !pregnant && canMate){
            if(age >= 1 && age <= 3){
                numOfTries = 0;
                LookForNest();
            }
        }
        else if(thirst < 4f && !recentlyDrank && currentAction != Action.Drinking && currentAction != Action.LookingForFood){
            numOfTries = 0;
            LookForWater();
        }
        else if(hunger < 2f && !recentlyAte && currentAction != Action.Eating && currentAction != Action.LookingForWater){
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

    //potentially solution for looking for mate and reproduction system. THIS HAS TO BE CHANGED FOR BETTER!!
    protected void LookForNest(){
        target = GetTarget();
        Collider[] cols = Physics.OverlapSphere(target, 0.7f, groundMask);
        LivingEntity nest = null;
        if(numOfTries == maxTries) {
            ChoosePath();
        }
        else if(cols.Length != 0) {
            nest = cols[0].GetComponent<Environment>().GetEntity("nest");
            if(nest != null){
                target = nest.transform.position;
                target += (startPosition - target).normalized * distance;
                target.y = this.transform.position.y;
                nestTransform = nest.transform;
                //checks whether nest found is full
                if(nestTransform.GetComponent<Nest>().GetOwner() != null){
                    
                    //if the gender is male, and nest is full.
                    //if female, get outta there
                    if(male && nestTransform.GetComponent<Nest>().GetOwner().GetComponent<Animal>().GetName() == this.transform.GetComponent<Animal>().GetName()){
                        currentAction = Action.LookingForNest;
                        StartMove();
                        
                    }
                    else{
                        currentAction = Action.Exploring;
                    }
                }
                //if nest isn't full and gender is female, go in
                else if(!male){
                    currentAction = Action.LookingForNest;
                    StartMove();
                }
                //if neither, get outta there
                else{
                    currentAction = Action.Exploring;
                }
               
            }
            else{
                numOfTries++;
                LookForNest();
            }
        }
        else{
            LookForNest();
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
            case Action.LookingForNest:
                if(Vector3.Distance(this.transform.position, nestTransform.position) <= 3f){
                    if(!male) nestTransform.GetComponent<Nest>().AddOwner(this.transform);
                    if(male) lastMate = Time.time;
                    currentAction = Action.Nesting;
                }
                else{
                    currentAction = Action.LookingForNest;
                }
                break;
            case Action.LookingForFood:
                if(Vector3.Distance(this.transform.position, foodTransform.position) <= 3f){
                    currentAction = Action.Eating;
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
        if(currentAction == Action.Nesting){
            if(nestTransform != null){
                if(male){
                    mating = true;
                    
                    if(Time.time - lastMate > 10f){
                        currentAction = Action.Exploring;
                        mating = false;
                        nestTransform.GetComponent<Nest>().GetOwner().GetComponent<Animal>().SetState(true);
                        canMate = false;
                    }
                }
                else if(!male){
                    if(thirst < 5f || hunger < 5f){
                        currentAction = Action.Exploring;
                    }
                    mating = true;
                    if(pregnant){
                        currentAction = Action.Exploring;
                        nestTransform.GetComponent<Nest>().RemoveOwner();
                        mating = false;
                        canMate = false;
                    }
                }
            }
        }
        
        
        if(currentAction == Action.Eating){
            if(foodTransform != null){
                if(hunger <= 10){
                    hunger += Time.deltaTime * 1.5f;
                }
                else{
                   // recentlyAte = true;
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
                //recentlyDrank = true;
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


    //increments age each cycle, which is currently 10 seconds for a year. Cycle created inside WorldController class
    private void AddAge(){
        age += 1;
        EventHandlerUI.AgeChanged();
    }

    // debug 
    /*private void OnDrawGizmos() {
        Handles.Label(transform.position, currentAction.ToString());
        Gizmos.DrawSphere(target, 0.5f);
    }*/

    public virtual bool GetGender(){
        return male;
    }
    public virtual void SetGender(bool gender){
        male = gender;
    }
    public virtual int GetAge(){
        return age;
    }
    public virtual bool GetMating(){
        return mating;
    }

    public virtual string GetAction(){
        return currentAction.ToString();
    }

    public virtual void SetState(bool state){
        this.pregnant = state;
    }
}
