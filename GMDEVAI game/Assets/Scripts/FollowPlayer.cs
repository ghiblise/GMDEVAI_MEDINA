using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] Transform player;
    private float travelTime = 3f;
    private Vector3 playerLocation;
    private Vector3 petLocation;
    private float elapsedTime;

    void LateUpdate()
    {
        playerLocation = player.position;
        petLocation = transform.position;

        elapsedTime += Time.deltaTime;
        float percentageComplete = elapsedTime / travelTime;

        Vector3 lookAtPlayer = new Vector3(player.position.x, this.transform.position.y, player.position.z);

        Vector3 direction = lookAtPlayer - transform.position;

        // Code from lecture video
        // this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * rotSpeed);

        if (Vector3.Distance(lookAtPlayer, transform.position) > 1)
        {
            // Bonus item #2, using Vector3.Lerp instead of Quaternion.Slerp
            transform.position = Vector3.Lerp(petLocation, playerLocation, percentageComplete * Time.deltaTime);
            transform.rotation = Quaternion.LookRotation(direction);
        }    
        else if (Vector3.Distance(lookAtPlayer, transform.position) < 1)
            elapsedTime = 0;
    }
}
