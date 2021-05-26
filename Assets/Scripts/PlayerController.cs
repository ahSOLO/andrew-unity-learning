using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float moveSpeed = 2f;
    float rotationSpeed = 10f;
    bool isMoving = false;
    Vector3 direction;

    [SerializeField] GameObject gimbal;
    
    private void FixedUpdate()
    {
        if (isMoving)
        {
            MoveAndRotateWithCamera();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
    }

    public void MoveAndRotateWithCamera()
    {
        transform.Translate(direction.normalized * moveSpeed * Time.fixedDeltaTime, Space.Self);
        transform.rotation = Quaternion.Euler(new 
            Vector3(transform.rotation.eulerAngles.x, 
            Mathf.LerpAngle(transform.rotation.eulerAngles.y, gimbal.transform.rotation.eulerAngles.y, rotationSpeed * Time.fixedDeltaTime)));
    }
}
