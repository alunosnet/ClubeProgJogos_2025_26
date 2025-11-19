using UnityEngine;

public class Atirar : MonoBehaviour
{
    public GameObject ModeloBola;
    public Transform PosicaoAtirar; //preencher no inspector
    public float forcaAtirar = 10;
    public float tempoVidaBola = 10;
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
            GameObject bola=Instantiate(ModeloBola, PosicaoAtirar.position, Quaternion.identity);
            //Referência para o componente rigidbody da bola
            Rigidbody rb = bola.GetComponent<Rigidbody>();
            //aplicar uma força na bola
            rb.AddForce(transform.forward * forcaAtirar);
            //destroi a bola ao fim de x segundos
            Destroy(bola, tempoVidaBola);
        }
    }
}
