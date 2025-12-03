using UnityEngine;
/// <summary>
/// Toca um som sempre que colide com qualquer coisa
/// </summary>
public class SomBola : MonoBehaviour
{
    //Referência para o AudioSource
    AudioSource _audioSource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        _audioSource.Play();
    }
}
