/* ------------------------------------------------------------------------------
<GameManager Class>
  This script is the game manager that handles
  1> Game states reference
  2> Changes game states
  3> The flow of game states
<GameManager Class>
--------------------------------------------------------------------------------*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
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
  public bool IsGameOver { get => m_isGameOver; set => m_isGameOver = value; }
  private bool m_isTimeOver = false;
  public bool IsTimeOver { get => m_isTimeOver; set => m_isTimeOver = value; }


  // References to other game objects used in the script
  private GameObject m_player;


  // Properties to save coroutine names
  private string RunGameLoopString = "RunGameLoopRoutine";
  private string SetupGameSceneString = "SetupGameSceneRoutine";

  private string PlayGameString = "PlayGameRoutine";
  private string EndGameString = "EndGameRoutine";

  // Events emitted by game manager
  public UnityEvent SetupGameSceneEvent;
  public UnityEvent StartGameEvent;
  public UnityEvent EndGameEvent;

  private void Awake()
  {
    // Player Reference
    m_player = GameObject.FindGameObjectWithTag("Player");
  }

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

  IEnumerator RunGameLoopRoutine()
  {
    // Coroutines that control flow of game states
    // Coroutine to setup game scene
    yield return StartCoroutine(SetupGameSceneString);
    yield return StartCoroutine(PlayGameString);
    yield return StartCoroutine(EndGameString);

  }

  IEnumerator SetupGameSceneRoutine()
  {
    // Setup game scene method is used to set the game scene and display start menu
    Debug.Log("GameManager info: Setup Game Scene");
    // If the setup game scene event property is not null
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
    // Once the while loop exits on starting the game
    // Check if start game event property is not null
    if (StartGameEvent != null)
    {
      //Then invoke start game event for other game objects to follow 
      StartGameEvent.Invoke();
    }
  }

  IEnumerator PlayGameRoutine()
  {
    Debug.Log("GameManager info: Playing Game");
    // Set the game has started bool to true
    m_hasGameStarted = true;
    // Check if the game is over
    // and yield null till the game is over
    while (!m_isGameOver && !m_isTimeOver)
    {
      yield return null;
    }
    if (EndGameEvent != null)
    {
      EndGameEvent.Invoke();
    }
  }

  IEnumerator EndGameRoutine()
  {
    Debug.Log("GameManager info: Ending Game");
    while (m_isGameOver || m_isTimeOver)
    {
      yield return null;
    }
  }

  public void RestartLevel()
  {
    Scene scene = SceneManager.GetActiveScene();
    SceneManager.LoadScene(scene.name);
  }
}
