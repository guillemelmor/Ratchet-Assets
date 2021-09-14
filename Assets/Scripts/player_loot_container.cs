using UnityEngine;

///<summary> 
/// Behaviour of the loot trigger
///</summary>
public class player_loot_container : MonoBehaviour
{
    ///<summary> 
    /// Destroys the loot object once he contacts with the player
    ///</summary>
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<player_input>(out player_input player))
            Destroy(gameObject);
    }
}
