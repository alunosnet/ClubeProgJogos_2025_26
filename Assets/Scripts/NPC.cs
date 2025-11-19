using UnityEngine;
using UnityEngine.AI;
/// <summary>
/// Gere o comportamento de um NPC
/// </summary>
public class NPC : MonoBehaviour
{
    public bool Inimigo = true;
    public enum NPCEstados { Idle = 0, Patrulha = 1, Atacar = 2, Morto = 3 }
    public NPCEstados Estado = NPCEstados.Idle; //Estado inicial
    public Transform[] Pontos;  //array de pontos de patrulha
    public int ProximoPonto = 0;
    public int Velocidade = 3;
    public float DistanciaMinima = 1;
    public float DistanciaAtacar = 1;
    public float IntervaloAtacar = 3;
    public int ValorTiraVida = 10;
    public float DistanciaVisao = 10;
    public float IntervaloAtual = 0;
    NavMeshAgent Agente;
    //Vida Vida;
    GameObject Jogador;
    public GameObject efeitoAtaque;
    //Alterado para o nível terreno
    public Transform Olhos;
    public float AnguloVisao = 90;
    public float TempoEspera = 5;
    public float TempoAEspera = 0;
    public bool Atacou = false;
    public bool vePlayer = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Jogador = GameObject.FindGameObjectWithTag("Player");
        if (Jogador == null)
        {
            Debug.LogError("Jogador não encontrado");
        }
        Agente = GetComponent<NavMeshAgent>();
        //Vida = GetComponent<Vida>();
        TempoAEspera = TempoEspera;
    }
    void Estado_Morto()
    {
        if (Agente==null)
        {
            return;
        }
        Agente.isStopped = true;
        Agente.speed = 0;
        Agente.velocity = Vector3.zero;
        Estado = NPCEstados.Morto;
    }
    void Estado_Idle()
    {
        if (Agente == null)
        {
            return;
        }
        Agente.isStopped = true;
        Agente.speed = 0;
        Agente.velocity = Vector3.zero;
        Estado = NPCEstados.Idle;
    }
    void Estado_Patrulha()
    {
        if (Agente == null)
        {
            return;
        }
        if (Pontos.Length == 0)
        {
            Estado = NPCEstados.Idle;
            return;
        }
        if (Agente.isOnNavMesh)
            Agente.isStopped = false;
        Agente.speed = Velocidade;
        if (Vector3.Distance(transform.position, Pontos[ProximoPonto].position) < DistanciaMinima)
        {
            ProximoPonto++; //avança para o próximo ponto
            if (ProximoPonto >= Pontos.Length)
            {
                ProximoPonto = 0;
            }

        }
        Agente.SetDestination(Pontos[ProximoPonto].position);

    }
    void Estado_Atacar()
    {
        Agente.speed = Velocidade * 1.5f;
        
        Vector3 OlharPara = new Vector3(Jogador.transform.position.x,
                                transform.position.y,
                                Jogador.transform.position.z);
        transform.LookAt(OlharPara);
        //pode atacar
        if (Vector3.Distance(transform.position, Jogador.transform.position) < DistanciaAtacar)
        {
            Agente.isStopped = true;
            Agente.velocity = Vector3.zero;
            if (Time.time > IntervaloAtual)
            {
                IntervaloAtual = Time.time + IntervaloAtacar;
                //Isto devia ser uma referência para o script Vida do jogador
                //Jogador.GetComponent<Vida>().PerdeVida(ValorTiraVida);
                if (efeitoAtaque != null)
                {
                    MostraEfeito();
                }
                Atacou = true;  //fazer a animação de ataque
            }
        }
        else
        {
            Agente.isStopped = false;
            Agente.SetDestination(Jogador.transform.position);
        }
    }
    //Devolve True se vê o player
    bool VePlayer()
    {
        //if (Vector3.Distance(transform.position, Jogador.transform.position) < DistanciaVisao)
        //{
        //    return true;
        //}
        if (Utils.CanYouSeeThis(Olhos,Jogador.transform,"Player",AnguloVisao,DistanciaVisao))
        {
            vePlayer = true;
            return true;
        }
        vePlayer = false;
        return false;
    }
    public void MostraEfeito()
    {
        if (efeitoAtaque != null)
        {
            efeitoAtaque.SetActive(true);
            Invoke(nameof(EscondeEfeito), 0.5f);
        }
    }
    void EscondeEfeito()
    {
        if (efeitoAtaque!=null)
        {
            efeitoAtaque.SetActive(false);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Estado == NPCEstados.Morto)
        {
            return;
        }
        //if (Vida !=null && Vida.Morreu())
        //{
        //    Estado_Morto();
        //    return;
        //}
        switch(Estado)
        {
            case NPCEstados.Idle:
                Estado_Idle();
                if (Inimigo && VePlayer())
                {
                    Estado = NPCEstados.Atacar;
                }
                break;
            case NPCEstados.Patrulha:
                Estado_Patrulha();
                if (Inimigo && VePlayer())
                {
                    Estado = NPCEstados.Atacar;
                }
                break;
            case NPCEstados.Atacar:
                if (Inimigo==false )
                {
                    Estado = NPCEstados.Patrulha;
                    return;
                }
                if (VePlayer())
                {
                    Estado_Atacar();
                    TempoAEspera = TempoEspera;
                }
                else
                {
                    TempoAEspera -= Time.deltaTime;
                    if (TempoAEspera <= 0)
                    {
                        Estado = NPCEstados.Patrulha;
                        TempoAEspera = TempoEspera;
                    }
                }
                break;
        }
    }
}
