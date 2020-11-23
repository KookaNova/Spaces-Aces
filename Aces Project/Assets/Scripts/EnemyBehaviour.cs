using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class EnemyBehaviour : MonoBehaviour
{
    private enum ShipStates
    {
        RandomFlight,
        Escorting,
        ChasingPlayer,
        Attacking,
        Evading
        
    }

    public int detectRange = 100, seeking = 5;

    private GameObject _player;
    private Rigidbody _rb;
    private int _decisionTime = 50;

    private ShipStates _currentState = ShipStates.RandomFlight;

    private void Start()
    {
        _rb = gameObject.GetComponent<Rigidbody>();
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (_currentState == ShipStates.RandomFlight)
        {
            if (_decisionTime<0)
            {
                _decisionTime = Random.Range(50,200);
                //code:decide a new random direction to point or point back to the player if too far away
            }
            else
            {
                _decisionTime--;
            }
 
            //use Vector3.Distance to check if player is close
            if (Vector3.Distance(gameObject.transform.position, _player.transform.position) < detectRange)
            {
                _currentState = ShipStates.ChasingPlayer;
            }  
        }

        if (_currentState == ShipStates.ChasingPlayer)
        {
            var toTarget = _player.transform.position - _rb.position;
            var targetRotation = Quaternion.LookRotation(toTarget);
            _rb.rotation = Quaternion.RotateTowards(_rb.rotation, targetRotation, seeking);
            
        }
    }
}
