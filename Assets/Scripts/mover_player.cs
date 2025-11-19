using UnityEngine;

public class mover_player : MonoBehaviour
{
    public float velocidade_andar = 3;
    public float velocidade_rodar = 15;
    //esta variável tem de ser negativa
    public float velocidade_salto = -3;
    //referência para o character controller
    CharacterController cc;
    // dados de input
    float _vertical = 0; // eixo vertical (ws up down)
    float _horizontal = 0; //eixo horizontal (ad left right)
    Vector3 _velocidade;
    public bool IsGrounded;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cc = GetComponent<CharacterController>();
        if (cc==null)
        {
            Debug.Log("Falta o character controller");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Ler input
        _vertical = Input.GetAxis("Vertical");   //movimento frente/trás
        _horizontal = Input.GetAxis("Horizontal"); //rotação do player
        //calcular e aplicar a rotação
        transform.Rotate(transform.up * _horizontal * velocidade_rodar * Time.deltaTime);

        // calcular o movimento
        Vector3 movimento = transform.forward * _vertical * velocidade_andar * Time.deltaTime;
        //verificar se está a correr
        // aplicar o movimento
        cc.Move(movimento);
        //gravidade e saltar
        if (Input.GetButtonDown("Jump") && IsGrounded )
        {
            _velocidade.y = Mathf.Sqrt(velocidade_salto * Physics.gravity.y);
        }
        else
        {
            //aplicar gravidade
            _velocidade += Physics.gravity * Time.deltaTime;
        }

        //gravidade
        cc.Move(_velocidade * Time.deltaTime);
        IsGrounded = cc.isGrounded;
    }
}
