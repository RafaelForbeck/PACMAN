using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }  // Inst�ncia singleton do GameManager.

    [SerializeField] private Ghost[] ghosts;  // Array de fantasmas no jogo.
    [SerializeField] private Pacman pacman;  // Refer�ncia ao objeto Pacman.
    [SerializeField] private Transform pellets;  // Refer�ncia ao objeto que cont�m todos os pellets.
    [SerializeField] private Text gameOverText;  // Texto de "Game Over" na UI.
    [SerializeField] private Text scoreText;  // Texto que exibe a pontua��o na UI.
    [SerializeField] private Text livesText;  // Texto que exibe o n�mero de vidas na UI.
    [SerializeField] private Text playGameText;  // Texto que exibe o n�mero de vidas na UI.

    private int ghostMultiplier = 1;  // Multiplicador de pontos ao comer fantasmas.
    private int lives = 3;  // N�mero inicial de vidas.
    private int score = 0;  // Pontua��o inicial.

    public int Lives => lives;  // Propriedade p�blica para obter o n�mero de vidas.
    public int Score => score;  // Propriedade p�blica para obter a pontua��o.

    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);  // Se uma inst�ncia j� existe, destr�i este objeto.
        }
        else
        {
            Instance = this;  // Define a inst�ncia singleton.
            DontDestroyOnLoad(gameObject);  // Impede que o objeto seja destru�do ao carregar uma nova cena.
        }
    }

    private void Start()
    {
        NewGame();  // Inicia um novo jogo quando o script come�a.
        playGameText.enabled = false;  // Desativa o texto de "Play Game".
    }

    private void Update()
    {
        if (lives <= 0 && Input.anyKeyDown)
        {
            NewGame();  // Reinicia o jogo se todas as vidas foram perdidas e qualquer tecla � pressionada.
        }
    }

    private void NewGame()
    {
        SetScore(0);  // Reseta a pontua��o.
        SetLives(3);  // Reseta o n�mero de vidas.
        NewRound();  // Inicia uma nova rodada.
    }

    private void NewRound()
    {
        gameOverText.enabled = false;  // Desativa o texto de "Game Over".
      

        foreach (Transform pellet in pellets)
        {
            pellet.gameObject.SetActive(true);  // Ativa todos os pellets.
        }

        ResetState();  // Reseta o estado dos personagens.
    }

    private void ResetState()
    {
        for (int i = 0; i < ghosts.Length; i++)
        {
            ghosts[i].ResetState();  // Reseta o estado de cada fantasma.
        }

        pacman.ResetState();  // Reseta o estado do Pacman.
    }

    private void GameOver()
    {
        gameOverText.enabled = true;  // Ativa o texto de "Game Over".

        for (int i = 0; i < ghosts.Length; i++)
        {
            ghosts[i].gameObject.SetActive(false);  // Desativa todos os fantasmas.
        }

        pacman.gameObject.SetActive(false);  // Desativa o Pacman.
    }

    private void PlayGame()
    {
        playGameText.enabled = true;  // Ativa o texto de "Play Game".

    }

    private void SetLives(int lives)
    {
        this.lives = lives;  // Atualiza o n�mero de vidas.
        livesText.text = "x" + lives.ToString();  // Atualiza o texto de vidas na UI.
    }

    private void SetScore(int score)
    {
        this.score = score;  // Atualiza a pontua��o.
        scoreText.text = score.ToString().PadLeft(2, '0');  // Atualiza o texto de pontua��o na UI.
    }

    public void PacmanEaten()
    {
        pacman.DeathSequence();  // Inicia a sequ�ncia de morte do Pacman.

        SetLives(lives - 1);  // Reduz uma vida.

        if (lives > 0)
        {
            Invoke(nameof(ResetState), 3f);  // Reseta o estado ap�s 3 segundos se ainda houver vidas.
        }
        else
        {
            GameOver();  // Termina o jogo se n�o houver mais vidas.
        }
    }

    public void GhostEaten(Ghost ghost)
    {
        int points = ghost.points * ghostMultiplier;  // Calcula os pontos com o multiplicador.
        SetScore(score + points);  // Atualiza a pontua��o.

        ghostMultiplier++;  // Aumenta o multiplicador de pontos.
    }

    public void PelletEaten(Pellet pellet)
    {
        pellet.gameObject.SetActive(false);  // Desativa o pellet.

        SetScore(score + pellet.points);  // Atualiza a pontua��o.

        if (!HasRemainingPellets())
        {
            pacman.gameObject.SetActive(false);  // Desativa o Pacman.
            Invoke(nameof(NewRound), 3f);  // Inicia uma nova rodada ap�s 3 segundos.
        }
    }

    public void PowerPelletEaten(PowerPellet pellet)
    {
        for (int i = 0; i < ghosts.Length; i++)
        {
            ghosts[i].frightened.Enable(pellet.duration);  // Ativa o modo assustado para cada fantasma.
        }

        PelletEaten(pellet);  // Trate o Power Pellet como um Pellet regular.
        CancelInvoke(nameof(ResetGhostMultiplier));  // Cancela o reset do multiplicador.
        Invoke(nameof(ResetGhostMultiplier), pellet.duration);  // Reseta o multiplicador ap�s a dura��o do Power Pellet.
    }

    private bool HasRemainingPellets()
    {
        foreach (Transform pellet in pellets)
        {
            if (pellet.gameObject.activeSelf)
            {
                return true;  // Retorna true se ainda houver pellets ativos.
            }
        }

        return false;  // Retorna false se todos os pellets foram comidos.
    }

    private void ResetGhostMultiplier()
    {
        ghostMultiplier = 1;  // Reseta o multiplicador de pontos.
    }
}