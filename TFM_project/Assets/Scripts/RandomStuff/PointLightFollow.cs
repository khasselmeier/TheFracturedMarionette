using UnityEngine;

public class PointLightFollow : MonoBehaviour
{
    [Header("Follow Settings")]
    public float followSpeed = 5f;        //fhow quickly the light moves toward the player
    public Vector3 offset = new Vector3(0f, 3f, -2f); //position offset from the player

    private Transform player;

    void Start()
    {
        //find the player by tag
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogWarning("PointLightFollowScript: No GameObject with tag 'Player' found in the scene");
        }
    }

    void LateUpdate()
    {
        if (player == null) return;

        //smoothly move the light toward the player + offset
        Vector3 targetPos = player.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetPos, followSpeed * Time.deltaTime);

        //make the light look at the player
        transform.LookAt(player);
    }
}