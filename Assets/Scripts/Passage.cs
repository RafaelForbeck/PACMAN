using UnityEngine;

[RequireComponent(typeof(Collider2D))] // Requer um componente Collider2D para funcionar corretamente
public class Passage : MonoBehaviour
{
    public Transform connection; // O ponto de conexão para onde o objeto será movido ao entrar na passagem

    // Chamado quando outro objeto entra na área de colisão desta passagem
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Obtém a posição do ponto de conexão
        Vector3 position = connection.position;
        // Mantém a coordenada z do objeto que entrou na passagem
        position.z = other.transform.position.z;
        // Move o objeto que entrou para a posição do ponto de conexão
        other.transform.position = position;
    }
}