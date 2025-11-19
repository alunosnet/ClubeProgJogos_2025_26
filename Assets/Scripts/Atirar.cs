using UnityEngine;

public class Atirar : MonoBehaviour
{
    public GameObject ModeloBola;
    public Transform PosicaoAtirar; //preencher no inspector
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            //instanciar bola
            Instantiate(ModeloBola, PosicaoAtirar.position, Quaternion.identity);

        }
    }
}
