/* ------------------------------------------------------------------------------
GameManager Class
  * This script is the game manager that handles
  1> Game states reference
  2> Changes game states
  3> The flow of game states
--------------------------------------------------------------------------------*/

using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
  // Setup game-state properties and their corresponding public properties
  // Private bool property to setup the scene
  private bool m_setupGameScene = false;
  // Public property other classes can get and set
  public bool SetupGameScene { get => m_setupGameScene; set => m_setupGameScene = value; }
  // Private bool property to to know if the level has started
  private bool m_hasGameStarted = false;
  // Public property other classes can get and set
  public bool HasGameStarted { get => m_hasGameStarted; set => m_hasGameStarted = value; }
  // Private bool property to know if the game is in progress
  private bool m_isGamePlaying = false;
  // Public property other classes can get and set
  public bool IsGamePlaying { get => m_isGamePlaying; set => m_isGamePlaying = value; }
  // Private bool property to know if the game is over
  private bool m_isGameOver = false;
  // Public property other classes can get and set
  public bool IsGameOver { get => m_isGameOver; set => m_isGameOver = value; }
  // Private bool property to know if the game time is over
  private bool m_isTimeOver = false;
  // Public property other classes can get and set
  public bool IsTimeOver { get => m_isTimeOver; set => m_isTimeOver = value; }

  // References to other game objects used in the script
  // Reference to the player space ship game object
  private GameObject m_player;

  // Properties to save coroutine names
  // Run main game loop
  private string RunGameLoopString = "RunGameLoopRoutine";
  // Run setup game scene co-routine
  private string SetupGameSceneString = "SetupGameSceneRoutine";
  // Run play game co-routine
  private string PlayGameString = "PlayGameRoutine";
  // Run end game co-routine
  private string EndGameString = "EndGameRoutine";

  // Events emitted by game manager
  public UnityEvent SetupGameSceneEvent;
  public UnityEvent StartGameEvent;
  public UnityEvent EndGameEvent;

  //Awake method
  private void Awake()
  {
    // Get player reference
    m_player = GameObject.FindGameObjectWithTag("Player");
  }

  // Start method
  private void Start()
  {
    // Check if a player reference has been setup
    if (m_player != null)
    {
      // If yes, start the game loop coroutine
      StartCoroutine(RunGameLoopString);
    } // Else log a warning to console
    else
    {
      Debug.LogWarning("GameManager error: Player script reference missing");
    }
  }

  // Run main game loop
  IEnumerator RunGameLoopRoutine()
  {
    // Coroutines that control flow of game states
    // Coroutine to setup game scene
    yield return StartCoroutine(SetupGameSceneString);
    // Coroutine to play game
    yield return StartCoroutine(PlayGameString);
    // Coroutine to end game
    yield return StartCoroutine(EndGameString);
  }

  // Setup game scene co-routine
  IEnumerator SetupGameSceneRoutine()
  {
    // Setup game scene co-routine is used to set the game scene and display start menu
    Debug.Log("GameManager info: Setup Game Scene");
    // Check if the setup game scene event property is not null
    if (SetupGameSceneEvent != null)
    {
      // If yes then invoke setup game scene event for other game objects to follow 
      SetupGameSceneEvent.Invoke();
      // Check if the player has started the game 
      // and yield null till the start button has been clicked
      while (!m_hasGameStarted)
      {
        yield return null;
      }
    }
    // Once the while loop exits on pressing play button
    // Check if start game event property is not null
    if (StartGameEvent != null)
    {
      // Then invoke start game event for other game objects to follow 
      StartGameEvent.Invoke();
    }
  }

  // Play game co-routine
  IEnumerator PlayGameRoutine()
  {
    // Play game scene co-routine is used to play the game on pressing play button
    Debug.Log("GameManager info: Playing Game");

    // Check if the game or game time is over
    // and yield null till the game is over
    while (!m_isGameOver && !m_isTimeOver)
    {
      yield return null;
    }
    // Check if end game event property is not null
    if (EndGameEvent != null)
    {
      // Then invoke end game event for other game objects to follow 
      EndGameEvent.Invoke();
    }
  }

  // End game co-routine
  IEnumerator EndGameRoutine()
  {
    Debug.Log("GameManager info: Ending Game");
    // Yield null when the game is over
    while (m_isGameOver || m_isTimeOver)
    {
      yield return null;
    }
  }

  // Restart game level method
  public void RestartLevel()
  {
    // Get the current active scene
    Scene scene = SceneManager.GetActiveScene();
    // Load scene again to reboot game
    SceneManager.LoadScene(scene.name);
  }
}
