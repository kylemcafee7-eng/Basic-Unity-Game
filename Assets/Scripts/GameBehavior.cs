using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using CustomExtensions;

public class GameBehavior : MonoBehaviour, IManager
{
    private string _state;
    public string State
    {
        get { return _state; }
        set { _state = value; }
    }

    public Stack<string> lootStack = new Stack<string>();

    public string labelText = "Covert the Nascent One's. They are yet uncorrupted.";
    public int maxItems = 2;    

    public bool showWinScreen = false;

    public bool showLossScreen = false;

    private int _itemsCollected = 0;
    private int _playerHP = 3;

    public delegate void DebugDelegate(string newText);
    public DebugDelegate debug = Print;


    public int Items
    {
        get { return _itemsCollected; }

        set { //_itemsCollected = value;
            if(_itemsCollected >= maxItems)
            {
                labelText = "All Nascent Ones converted. Sector Cleansed.";

                showWinScreen = true;

                Time.timeScale = 0f;
            }
            else
            {
                labelText = "Nascent One Converted. " + (maxItems - _itemsCollected) + " remain.";
            }
        }
    }

    public int HP
    {
        get { return _playerHP; }
        set {
            _playerHP = value;
            if(_playerHP <= 0)
            {
                labelText = "Corrupted.";
                showLossScreen = true;
                Time.timeScale = 0;
            }
            else
            {
                labelText = "Corruption detected, Heliotrophic Essence lost.";
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Initialize();

        InventoryList<string> inventoryList = new
            InventoryList<string>();

        inventoryList.SetItem("Potion");
        Debug.Log(inventoryList.item);

        GameObject player = GameObject.Find("Player");
        PlayerBehavior playerBehavior =
            player.GetComponent<PlayerBehavior>();
        playerBehavior.playerJump += HandlePlayerJump;

    }

    public void HandlePlayerJump()
    {
        debug("Player has jumped...");
    }

    public void Initialize()
    {
        _state = "Manager initialized..";
        _state.FancyDebug();
        Debug.Log(_state);

        lootStack.Push("Sword of Doom");
        lootStack.Push("HP+");
        lootStack.Push("Golden Key");
        lootStack.Push("Winged Boot");
        lootStack.Push("Mythril Bracers");
        debug(_state);
        LogWithDelegate(debug);
    }

    public static void Print(string newText)
    {
        Debug.Log(newText);
    }

    public void LogWithDelegate (DebugDelegate del)
    {
        del("Delegating the debug task...");
    }

    void OnGUI()
    {
        GUI.Box(new Rect(20, 20, 250, 25), "Corruption:" + _playerHP);
        GUI.Box(new Rect(20, 50, 250, 25), "Nascent Ones Converted:" + _itemsCollected);
        GUI.Box(new Rect(Screen.width / 2 - 100, Screen.height - 50, 350, 50), labelText);

        if (showWinScreen)
        {
            if(GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 50, 200, 100), "Corruption Averted."))
            {

                Utilities.RestartLevel(0);

            }
            

            
        }

        if(showLossScreen)
        {
            if(GUI.Button(new Rect(Screen.width/2-100,Screen.height/2-50,300,100),"Corruption Detected. Severing Link."))
            {
                try
                {
                    Utilities.RestartLevel(-1);
                    debug("Level restarted successfully...");
                }
                catch(System.ArgumentException e)
                {
                    Utilities.RestartLevel(0);
                    debug("Reverting to scene 0; " + e.ToString());
                }
                finally
                {
                    debug("Restart handled...");
                }
               
            }

        }
    }



    // Update is called once per frame
    void Update()
        {
        
        }

    public void PrintLootReport()
    {
        _itemsCollected += 1;
        var currentItem = lootStack.Pop();
        var nextItem = lootStack.Peek();
        Debug.LogFormat("You got a {0}! You've got a good chance of finding a {1} next!", currentItem, nextItem);
        Debug.LogFormat("There are {0} random loot items waiting for you!", lootStack.Count);

    }

}
