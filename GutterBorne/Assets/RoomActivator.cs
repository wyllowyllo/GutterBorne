using UnityEngine;

public class RoomActivator : MonoBehaviour
{
    [SerializeField] private Enemy[] _enemies; 
    

    private bool _isActivated = false;

 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (_isActivated) return;

        ActivateRoom();
        _isActivated = true;
    }

    private void ActivateRoom()
    {
        foreach (var enemy in _enemies)
        {
            if (enemy != null)
                enemy.Activate();
        }
    }

}
