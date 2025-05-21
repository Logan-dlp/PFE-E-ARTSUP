using UnityEngine;
using System;

public class CauldronVFXController : MonoBehaviour
{
    [SerializeField] private ParticleSystem _bubbleVFX;
    [SerializeField] private ParticleSystem _smokeVFX;
    [SerializeField] private ParticleSystem _burnedVFX;
    [SerializeField] private ParticleSystem[] _fireVFXArray;

    private bool _isFireActive = false;

    public void PlayBubble()
    {
        if (_bubbleVFX != null)
        {
            _bubbleVFX.Play();
        }
        else
        {
            Debug.LogError("Bubble VFX is not assigned!");
        }
    }

    public void PlaySmoke()
    {
        if (_smokeVFX != null)
        {
            _smokeVFX.Play();
        }
        else
        {
            Debug.LogError("Smoke VFX is not assigned!");
        }
    }

    public void PlayBurned()
    {
        if (_burnedVFX != null)
        {
            _burnedVFX.Play();
        }
        else
        {
            Debug.LogError("Burned VFX is not assigned!");
        }
    }

    public void PlayFire()
    {
        if (!_isFireActive)
        {
            if (_fireVFXArray != null && _fireVFXArray.Length > 0)
            {
                foreach (var fx in _fireVFXArray)
                {
                    if (fx != null)
                    {
                        fx.Play();
                    }
                    else
                    {
                        Debug.LogError("One of the fire VFX is not assigned!");
                    }
                }
                _isFireActive = true;
            }
            else
            {
                Debug.LogError("Fire VFX array is not assigned or empty!");
            }
        }
    }

    public void StopFire()
    {
        if (_isFireActive)
        {
            if (_fireVFXArray != null && _fireVFXArray.Length > 0)
            {
                foreach (var fx in _fireVFXArray)
                {
                    if (fx != null)
                    {
                        fx.Stop();
                    }
                }
                _isFireActive = false;
            }
        }
    }
}