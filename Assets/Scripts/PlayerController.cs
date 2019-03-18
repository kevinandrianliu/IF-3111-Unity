using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

	public float speed;
	private Rigidbody rb; 

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Mouse X");
        //float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, 0.0f);
        rb.AddForce(movement * 10.0f);


        if(Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)) {

            movement = new Vector3(0.0f, 0.0f, 50.0f);
            rb.AddForce(movement * speed);
            rb.angularVelocity = Random.insideUnitSphere * 15.0f;

         }

     }

}
