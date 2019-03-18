using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBehavior : MonoBehaviour
{
    public GameObject ball;
    void OnTriggerExit(Collider other){
        if (other.gameObject == ball){
            
        }
    }
}
