using UnityEngine;

[RequireComponent(typeof(Collider2D))] // Requer um componente Collider2D para funcionar corretamente
public class Passage : MonoBehaviour
{
    public Transform connection; // O ponto de conex�o para onde o objeto ser� movido ao entrar na passagem

    // Chamado quando outro objeto entra na �rea de colis�o desta passagem
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Obt�m a posi��o do ponto de conex�o
        Vector3 position = connection.position;
        // Mant�m a coordenada z do objeto que entrou na passagem
        position.z = other.transform.position.z;
        // Move o objeto que entrou para a posi��o do ponto de conex�o
        other.transform.position = position;
    }
}