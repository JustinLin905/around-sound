using UnityEngine;

public class TestingTeleport : MonoBehaviour
{
    public GameObject targetLocation; // Drag your TargetLocation GameObject here in the inspector

    // Update is called once per frame
    void Update()
    {
        // Here I'm using the A button on the right Touch controller as an example
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            TeleportToTarget();
        }
    }

    void TeleportToTarget()
    {
        if(targetLocation != null)
        {
            // Get the current position of the OVRPlayerController
            Vector3 playerCurrentPosition = transform.position;

            // Calculate the difference in height to maintain the player's feet on the ground
            float differenceInHeight = playerCurrentPosition.y - transform.GetChild(0).transform.position.y;
            
            Vector3 targetPosition = targetLocation.transform.position;
            targetPosition.y += differenceInHeight;

            // Set the new position
            transform.position = targetPosition;
        }
        else
        {
            Debug.LogWarning("TargetLocation not set. Please set the TargetLocation GameObject.");
        }
    }
}
