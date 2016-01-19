using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class TankAI : NetworkBehaviour
{


    NavMeshAgent nav;
    TankSetup setup;
    TankMovement movement;
    TankShooting shooting;

    public float fireRate;

    private float nextFire;

    Vector3 Objetive;


    [Server]
    void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        setup = GetComponent<TankSetup>();
        movement = GetComponent<TankMovement>();
        shooting = GetComponent<TankShooting>();
        string diff = FindObjectOfType<Awareness>().difficulty;
        if (diff == "Easy" || diff == "easy" || diff == "E" || diff == "e") fireRate = 2;
        else if (diff == "Normal" || diff == "normal" || diff == "n" || diff == "n") fireRate = 1;
        else if (diff == "Hard" || diff == "hard" || diff == "H" || diff == "h") fireRate = 0.5f;

    }
    // Use this for initialization
    [Server]
    void Start()
    {



    }




    // Update is called once per frame
    [Server]
    void Update()
    {
        nav.speed = movement.m_Speed;
        nav.angularSpeed = movement.m_TurnSpeed;
        //nav.SetDestination (destination.transform.position);

        int goal = 0;

        if (setup.m_Team == 1)
            goal = 2;
        else if (setup.m_Team == 2)
            goal = 1;


        Vector3 destination = Vector3.zero;
        bool goingForItem;
        if (GameObject.FindObjectsOfType<PickUpItem>().Length > 0)
        {
            destination=GameObject.FindObjectOfType<PickUpItem>().GetComponent<Transform>().position;
            nav.stoppingDistance = 0;
            goingForItem = true;
            nav.SetDestination(destination);
        }
        else {
            float minDistance = float.MaxValue;
            foreach (TankManager tank in GameManager.m_Tanks)
            {
                if (tank.m_Team == goal && tank.m_Health.getCurrentHeath() > 0)
                {
                    float distance = Vector3.Distance(transform.position, tank.m_Instance.transform.position);
                    if (distance < minDistance){
                        destination = tank.m_Instance.transform.position;
                        minDistance = distance;
                    }

                }
            }
            nav.stoppingDistance = 10;
            goingForItem = false;
            nav.SetDestination(destination);
        }

        if (Vector3.Distance(transform.position, destination) < 10 && Time.time > nextFire && !goingForItem)
        {
            nextFire = Time.time + fireRate;
            //transform.LookAt(destination);

            Vector3 pos = destination - transform.position;
            Quaternion newRot = Quaternion.LookRotation(pos);
            transform.rotation = Quaternion.Lerp(transform.rotation, newRot, nav.angularSpeed * Time.deltaTime);

            shooting.Fire();


        }

        //transform.LookAt (destination);



    }

    


}
