using UnityEngine;

public class Footsteps : MonoBehaviour
{
    public PlayerMovement PlayerMovementScript;
    public void PlayFootSteps()
    {
        if (!PlayerMovementScript.grounded)
            return;
        int i = Random.Range(0, 2);
        FindObjectOfType<AudioManager>().Play($"Footsteps{i}");
    }
}
