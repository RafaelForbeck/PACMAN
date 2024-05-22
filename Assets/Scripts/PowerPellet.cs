using UnityEngine;

public class PowerPellet : Pellet
{
    public float duration = 8f; // A dura��o da Power Pellet em segundos

    // Chamado quando a Power Pellet � comida
    protected override void Eat()
    {
        // Notifica o GameManager que a Power Pellet foi comida e passa esta inst�ncia da Power Pellet como par�metro
        GameManager.Instance.PowerPelletEaten(this);
    }
}