using System.Collections.Generic;
using UnityEngine;

public class ObstructionHandler : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private LayerMask _obstructionMask;
    private List<Obstructable> _currentObstructions = new();

    void Update()
    {
        ClearPreviousObstructions();

        Vector3 direction = _player.position - transform.position;
        float distance = direction.magnitude;

        Debug.DrawRay(transform.position, direction.normalized * distance, Color.red, 0.1f);

        Ray ray = new(transform.position, direction);

        foreach (RaycastHit hit in Physics.RaycastAll(ray, distance, _obstructionMask))
        {
            if (hit.collider.TryGetComponent(out Obstructable obstructable))
            {
                obstructable.ApplyTransparency();
                _currentObstructions.Add(obstructable);
            }
        }
    }

    private void ClearPreviousObstructions()
    {
        foreach (Obstructable obstructable in _currentObstructions)
            obstructable.ResetMaterial();
        _currentObstructions.Clear();
    }
}