using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private AudioSource _clickSource;
    [SerializeField] private AudioSource _stepSource;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void PlayClickSound()
    {
        _clickSource.pitch = Random.Range(1.5f, 1.55f);
        _clickSource.Play();
    }



    public void PlayStepSound() => _stepSource.Play();

}
