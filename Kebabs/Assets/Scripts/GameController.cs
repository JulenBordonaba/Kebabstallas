﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
    private string[] map;

    public enum MapType { TEMPLE, HELL, PRISION, CLIFF, BRIDGE};
    public static MapType myMap;
    public List<GameObject> Maps = new List<GameObject>();
    public GameObject[] Soldiers;
    public GameObject[] Collectables;
    public static bool onGame = false;
    public static List<int> players = new List<int>();
    public static List<int> enemies = new List<int>();
    public static LevelsInfo Nivel;
    public bool GamePaused = false;
    public GameObject LoseImage;    //Pantalla de derrota
    public GameObject VictoryImage; //Pantalla de victoria
    public GameObject PauseImage; //Pantalla de pausa

    public static LevelData ld;
    public static int currentLevel;

    public static RecordsData rd;
    public static int currentLider;
    public static int currentKebabs;

    public GameObject kebabs;
    public GameObject record;

    public static CharactersData cd;
    private static int character;


    public GameObject var1;
    private float time;


    public Camera MainCamera;
    private bool CanSound = true;
    private bool timeEnding = false;
    private int GreenTeamCount = 0;
    private int RedTeamCount = 0;
    public Text GreenTeamCountText;
    public Text RedTeamCountText;
    public static Soldier Selected = null;
    public float SelectedDistance = int.MaxValue;
    public float EnemySelectedDistance = int.MaxValue;
    public GameObject SeleccionadoPrefab;
    public GameObject FollowArrowPrefab;
    private GameObject Seleccionado;
    public LevelsInfo[] levelsInfo;
    
    public static UnityEvent OnCollectablePlaced = new UnityEvent();

    private void Start()
    {
        Resume();
        if (onGame)
        {
            LoadLevel();
            LoadLevelInfo();

            if (var1 != null)
            {
                time = 150;
                var1.GetComponent<TextMeshProUGUI>().text = Mathf.RoundToInt(time / 60) + " : " + (Mathf.RoundToInt(time) - Mathf.RoundToInt(time / 60 * 60));
            }
        }
        else
        {
            CancelInvoke("DropCollectable");
            
            if (ld == null)
            {
                ld = SaveSystem.LoadLevels();
                if (ld == null)
                {
                    int[] levels = new int[50];
                    for (int i = 1; i < 50; i++)
                    {
                        levels[i] = 0;
                    }
                    levels[0] = 2;
                    ld = new LevelData(levels);
                }
            }

            if (cd == null)
            {
                cd = SaveSystem.LoadCharacters();
                if (cd == null)
                {
                    int[] characters = new int[16];
                    for (int i = 0; i < 16; i++)
                    {
                        characters[i] = 0;
                    }
                    if (ld.levels[0] == 1)
                        characters[3] = 1;
                    if (ld.levels[2] == 1)
                        characters[2] = 1;
                    if (ld.levels[11] == 1)
                        characters[6] = 1;
                    if (ld.levels[18] == 1)
                        characters[0] = 1;
                    if (ld.levels[25] == 1)
                        characters[4] = 1;
                    if (ld.levels[35] == 1)
                        characters[7] = 1;
                    if (ld.levels[45] == 1)
                        characters[13] = 1;
                    cd = new CharactersData(characters);
                }
            }

            if (rd == null)
            {
                rd = SaveSystem.LoadRecords();
                if (rd == null)
                {
                    Dictionary<int, int> records = new Dictionary<int, int>();
                    for (int i = 0; i < 16; i++)
                    {
                        records[i] = 0;
                    }
                    rd = new RecordsData(records);
                }
            }

            if (SceneManager.GetActiveScene().name == "PersonajeDesbloqueado")
            {
                GameObject newcharacter = Instantiate(Soldiers[character], new Vector2(0, -0.9f), Quaternion.identity);
                newcharacter.GetComponent<NuevoPersonaje>().Nombre = var1;
                newcharacter.transform.localScale *= 2;
                //var1.GetComponent<TextMeshProUGUI>().text = newcharacter.name;
            }
            else if (SceneManager.GetActiveScene().name == "TeamMatch")
            {
                FillTeams();
            }
            else if (SceneManager.GetActiveScene().name == "Survival")
            {
                for (int i = 0; i < cd.characters.Length; i++)
                {
                    GameObject selectable = GameObject.Find(Soldiers[i].name);
                    if (cd.characters[i] == 0)
                    {
                        selectable.GetComponent<SpriteRenderer>().color = Color.black;
                        selectable.GetComponent<Selectable>().unlocked = false;
                        selectable.GetComponent<Animator>().enabled = false;
                        selectable.transform.Find("record").gameObject.SetActive(false);
                    }
                    else
                    {
                        selectable.GetComponent<SpriteRenderer>().color = Color.white;
                        selectable.GetComponent<Selectable>().unlocked = true;
                        selectable.GetComponent<Animator>().enabled = true;
                        selectable.transform.Find("record").GetComponent<TextMeshProUGUI>().text = rd.records[i] + "";
                    }
                }
                if (currentKebabs > rd.records[currentLider])
                    StartCoroutine("SumCurrentKebabs");
                else
                {
                    int sum = 0;
                    foreach (int num in rd.records.Values)
                        sum += num;
                    record.GetComponent<TextMeshProUGUI>().text = sum+ "";
                    currentKebabs = 0;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        map = UpdateMap();
        if (Seleccionado == null && SeleccionadoPrefab != null)
        {
            Seleccionado = Instantiate(SeleccionadoPrefab);
        }
        if (onGame)
        {
            

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (GamePaused)
                    Resume();
                else
                    Pause();
            }

            if (var1 != null)
            {
                if (time > 0)
                {
                    
                    int aux = (Mathf.FloorToInt(time) - Mathf.FloorToInt(time / 60) * 60);
                    string aux2 = "";
                    if (aux < 10)
                    {
                        aux2 = "0";
                    }
                    var1.GetComponent<TextMeshProUGUI>().text = Mathf.FloorToInt(time / 60) + " : " + aux2 + (Mathf.FloorToInt(time) - Mathf.FloorToInt(time / 60) * 60);
                    time -= Time.deltaTime;
                    if (time < 10 && !timeEnding)
                    {
                        timeEnding = true;
                        StartCoroutine("Blink");
                    }
                }
                else
                {
                    foreach (GameObject persona in GameObject.FindGameObjectsWithTag("Player"))
                    {
                        persona.SetActive(false);
                    }
                }

            }
            
            if (GameObject.FindGameObjectsWithTag("Player").Length == 0)
            {
                
                if (LoseImage != null)
                {
                    if (kebabs != null)
                    {
                        kebabs.GetComponent<TextMeshProUGUI>().text = currentKebabs + "";
                        if (currentKebabs > rd.records[currentLider])
                        {
                            record.SetActive(true);
                            //rd.records[currentLider] = currentKebabs;
                            SaveSystem.SaveRecords(rd.records);
                            
                        }
                    }
                    Time.timeScale = 0f;
                    LoseImage.SetActive(true); //Pierde
                    MainCamera.GetComponent<AudioSource>().enabled = false;
                    if (CanSound)
                    {
                        CanSound = false;
                        AudioManager.PlaySound(AudioManager.Sound.DERROTA);
                    }
                    //onGame = false;
                }
                    
            }
            else if(GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
            {
                if (VictoryImage != null)
                {
                    if (record != null)
                    {
                        currentKebabs += 1; 
                    }
                    Time.timeScale = 0f;
                    VictoryImage.SetActive(true); //Gana
                    MainCamera.GetComponent<AudioSource>().enabled = false;
                    if (CanSound)
                    {
                        CanSound = false;
                        AudioManager.PlaySound(AudioManager.Sound.VICTORIA);
                    }
                    ld.levels[currentLevel] = 1;
                    if (currentLevel < 49 && ld.levels[currentLevel + 1] == 0)
                        ld.levels[currentLevel+1] = 2;
                    onGame = false;

                    if(SceneManager.GetActiveScene().name == "Level1")
                    {
                        if (currentLevel == 0 && cd.characters[3] == 0)
                            PersonajeDesbloqueado(3);
                        else if (currentLevel == 2 && cd.characters[2] == 0)
                            PersonajeDesbloqueado(2);
                        else if (currentLevel == 11 && cd.characters[6] == 0)
                            PersonajeDesbloqueado(6);
                        else if (currentLevel == 18 && cd.characters[0] == 0)
                            PersonajeDesbloqueado(0);
                        else if (currentLevel == 25 && cd.characters[25] == 0)
                            PersonajeDesbloqueado(4);
                        else if (currentLevel == 35 && cd.characters[7] == 0)
                            PersonajeDesbloqueado(7);
                        else if (currentLevel == 45 && cd.characters[45] == 0)
                            PersonajeDesbloqueado(13);
                    }
                    
                }
            }
        }
    }

    private IEnumerator SumCurrentKebabs()
    {
        
        int aux = currentKebabs - rd.records[currentLider]; //5
        int aux2 = currentKebabs; //10
        int aux3 = rd.records[currentLider];  //5
        rd.records[currentLider] = currentKebabs;    //10
        int sum = 0;
        foreach (int num in rd.records.Values)
        {
            sum += num;    //60
        }
        record.GetComponent<TextMeshProUGUI>().text = (sum - currentKebabs + aux3) + "";
        yield return new WaitForSeconds(0.5f);
        while (aux > 0)
        {
            GameObject lider = GameObject.Find(Soldiers[currentLider].name);
            
            GameObject bola = Instantiate(kebabs, lider.transform.position, Quaternion.identity);
            bola.GetComponent<SpriteRenderer>().sortingOrder = 100;
            StartCoroutine("SumToRecord", aux3);
            aux--;
            lider.transform.Find("record").GetComponent<TextMeshProUGUI>().text = currentKebabs - aux + "";
            yield return new WaitForSeconds(0.3f);
            
        }
        yield return new WaitForSeconds(1.1f);
        rd.records[currentLider] = aux2;
        currentKebabs = 0;

    }

    public IEnumerator SumToRecord(int aux3)
    {
        yield return new WaitForSeconds(1);
        currentKebabs--;
        int sum = 0;
        foreach (int num in rd.records.Values)
        {
            sum += num;
        }
        record.GetComponent<TextMeshProUGUI>().text = "-   " + (sum - currentKebabs + aux3) + "   -";
        SaveSystem.SaveRecords(rd.records);
    }

    private void DropCollectable()
    {
        int X = 0;
        int Y = 0;
        while (map[X][Y] == 'X')
        {
            X = Random.Range(1, 19);
            Y = Random.Range(1, 19);
        }
        int maxRand = Collectables.Length;
        if (Nivel != null)
        {
            maxRand = Nivel.nPildoras;
        }
        if (maxRand != 0)
        {
            Instantiate(Collectables[Random.Range(0, maxRand)], new Vector2(X / 10f, Y / 10f), Quaternion.identity);
            OnCollectablePlaced.Invoke();
        }
    }

    public void Resume()
    {
        if (PauseImage)
            PauseImage.SetActive(false);
        if (MainCamera)
            MainCamera.GetComponent<AudioSource>().UnPause();
        Time.timeScale = 1f;
        GamePaused = false;
    }

    public void Pause()
    {
        PauseImage.SetActive(true);
        MainCamera.GetComponent<AudioSource>().Pause();
        Time.timeScale = 0f;
        GamePaused = true;
    }

    public void MainMenuScene()
    {
        GameManager.LoadScene("MainTitle");
    }

    public void PersonajeDesbloqueado(int personaje)
    {
        cd.characters[personaje] = 1;
        character = personaje;
        GameManager.LoadScene("PersonajeDesbloqueado");
        SaveSystem.SaveCharacters(cd.characters);
    }

    public void SelectionScene()
    {
        onGame = false;
        GameManager.LoadScene("Selection");
        Resume();
    }

    public void TutorialScene()
    {
        onGame = false;
        Nivel = null;
        GameManager.LoadScene("Tutorial");
        Resume();
    }

    public void LevelsScene()
    {
        SaveSystem.SaveLevels(ld.levels);
        onGame = false;
        GameManager.LoadScene("Levels");
        Resume();
    }

    public void BossScene()
    {
        onGame = true;
        Nivel = null;
        GameManager.LoadScene("Level4");
        Resume();
    }

    public void SurvivalScene()
    {
        onGame = false;
        //currentKebabs = 0;
        GameManager.LoadScene("Survival");
        Resume();
    }

    public void LoadLevelScene(int level)
    {
        players = new List<int>();
        enemies = new List<int>();
        currentLevel = level - 1;
        GameManager.LoadScene("Level1");
        Nivel = levelsInfo[level - 1];

        onGame = true;
    }

    private void LoadLevel()
    {
        VictoryImage.SetActive(false);
        LoseImage.SetActive(false);
        float posx = 1;
        float posy = 0.1f;
        float inc = 0;

        InvokeRepeating("DropCollectable", 2, 10f);

        foreach (int number in players)
        {
            GameObject soldier = Instantiate(Soldiers[number]);
            posx += inc * 0.2f * Mathf.Pow(-1, inc);
            soldier.transform.position = new Vector2(posx, posy);
            soldier.tag = "Player";
            soldier.layer = 8;
            inc++;
            if (inc > 4)
            {
                inc = 0;
                posx = 1.1f - posy + 0.1f;
                posy += 0.1f;
            }
        }
        posx = 1;
        posy = 1.9f;
        inc = 0;

        foreach (int number in enemies)
        {
            GameObject soldier = Instantiate(Soldiers[number]);
            posx += inc * 0.2f * Mathf.Pow(-1, inc);
            soldier.transform.position = new Vector2(posx, posy);
            soldier.tag = "Enemy";
            soldier.layer = 9;
            inc++;
            if (inc > 4)
            {
                inc = 0;
                posx = 1.1f + posy - 1.9f;
                posy -= 0.1f;
            }
        }
    }

    private void LoadLevelInfo()
    {
        if (Nivel != null)
        {
            GameObject mapa;
            if (Nivel.Map == MapType.TEMPLE)
            {
                mapa = Instantiate(Maps[0]);
            }

            else if (Nivel.Map == MapType.PRISION)
            {
                mapa = Instantiate(Maps[1]);
            }
            else if (Nivel.Map == MapType.HELL)
            {
                mapa = Instantiate(Maps[2]);
            }
            else if (Nivel.Map == MapType.CLIFF)
            {
                mapa = Instantiate(Maps[3]);
            }
            else if (Nivel.Map == MapType.BRIDGE)
            {
                mapa = Instantiate(Maps[4]);
            }
            myMap = Nivel.Map;
            //mapa.transform.position = new Vector3(0.98f, 1.008f);
            Camera.main.GetComponent<AudioSource>().clip = Nivel.Music;
            Camera.main.GetComponent<AudioSource>().Play();

            foreach (SoldierInfo persona in Nivel.perosnas)
            {
                GameObject pers = Instantiate(persona.prefab);
                pers.transform.position = persona.position;
                pers.tag = persona.Tag;
                if (pers.tag == "Player")
                {
                    pers.layer = 8;
                }
                else
                {
                    pers.layer = 9;
                }
                Stats stats = pers.GetComponent<Stats>();
                stats.vida = persona.vida;
                stats.baseSpeed = persona.baseSpeed;
                stats.baseAttackSpeed = persona.baseAttackSpeed;
                stats.baseAttackDistance = persona.baseAttackDistance;
                stats.baseAttackDamage = persona.baseAttackDamage;
            }
        }
        else
        {
            if (SceneManager.GetActiveScene().name == "SurvivalBattle")
            {
                print("holi");
                float rand = Random.value;
                if (rand < 0.33f)
                {
                    myMap = MapType.TEMPLE;
                    Instantiate(Maps[0]);
                }
                    
                else if (rand < 0.66f)
                {
                    myMap = MapType.PRISION;
                    Instantiate(Maps[1]);
                }
                    
                else {
                    myMap = MapType.BRIDGE;
                    Instantiate(Maps[4]);
                }
                    
            }
            else
            {
                print("adios");
                myMap = MapType.TEMPLE;
            }
                
        }
    }

    public void ReloadLevel()
    {
        GameManager.ReLoadLevel();
    }

    public void Portada()
    {
        GameManager.LoadScene("Portada");
    }

    public void Configuracion()
    {
        GameManager.LoadScene("Configuracion");
    }

    public void Clear()
    {
        foreach (GameObject persona in GameObject.FindGameObjectsWithTag("Player"))
        {
            persona.tag = "Selectable";
            persona.GetComponent<SpriteRenderer>().color = Color.white;
        }
        foreach (GameObject persona in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            persona.tag = "Selectable";
            persona.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    public void ChangeGreenCount(int change)
    {
        if (GreenTeamCount + change >= 0 && GreenTeamCount + change + RedTeamCount <= 16)
        {
            GreenTeamCount += change;
        }
        GreenTeamCountText.text = "" + GreenTeamCount;
    }

    public void ChangeRedCount(int change)
    {
        if (RedTeamCount + change >= 0 && GreenTeamCount + change + RedTeamCount <= 16)
        {
            RedTeamCount += change;
        }
        RedTeamCountText.text = "" + RedTeamCount;
    }

    public void RandomizeTeams()
    {
        Clear();
        List<GameObject> personas = new List<GameObject>(GameObject.FindGameObjectsWithTag("Selectable"));
        int nEnemigos = RedTeamCount;
        if (RedTeamCount == 0)
        {
            nEnemigos = Random.Range(1, personas.Count - GreenTeamCount);
        }
        
        for (int i = 0; i < nEnemigos; i++)
        {
            int ind = Random.Range(0, personas.Count);
            personas[ind].tag = "Enemy";
            personas[ind].GetComponent<SpriteRenderer>().color = new Color32(255, 83, 83, 255);
            personas.Remove(personas[ind]);

        }
        personas = new List<GameObject>(GameObject.FindGameObjectsWithTag("Selectable")); ;

        int nPlayers = GreenTeamCount;
        if (GreenTeamCount == 0)
        {
            nPlayers = Random.Range(1, personas.Count);
        }
        for (int i = 0; i < nPlayers; i++)
        {
            int ind = Random.Range(0, personas.Count);
            personas[ind].tag = "Player";
            personas[ind].GetComponent<SpriteRenderer>().color = new Color32(129, 255, 133, 255);
            personas.Remove(personas[ind]);
        }
    }

    public void StartGameScene()
    {
        players = new List<int>();
        enemies = new List<int>();
        foreach (GameObject persona in GameObject.FindGameObjectsWithTag("Player"))
        {
            players.Add(persona.GetComponent<Selectable>().myNumber);
        }
        foreach (GameObject persona in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemies.Add(persona.GetComponent<Selectable>().myNumber);
        }


        if (players.Count > 0 && enemies.Count > 0)
        {
            onGame = true;
            Nivel = null;
            GameManager.LoadScene("CustomizedBattle");
        }

    }

    public static void LiderSelected(GameObject lider)
    {
        currentLider = lider.GetComponent<Selectable>().myNumber;
        foreach (GameObject persona in GameObject.FindGameObjectsWithTag("Lider"))
        {
            if (persona != lider)
            {
                persona.SetActive(false);
            }
        }
    }

    public void ShowConfirmation(int num)
    {
        var1.transform.Find("Record").GetComponent<TextMeshProUGUI>().text = "" + rd.records[num];
        var1.SetActive(true);
    }

    public void HideConfirmation()
    {
        var1.SetActive(false);
    }

    public void FillTeams()
    {
        
        
        kebabs.GetComponent<TextMeshProUGUI>().text = "" + currentKebabs;
        record.GetComponent<TextMeshProUGUI>().text = "" + rd.records[currentLider];

        players = new List<int>();
        enemies = new List<int>();

        GameObject lider = Instantiate(Soldiers[currentLider], new Vector2(0, -3f), Quaternion.identity);
        Destroy(lider.GetComponent<NuevoPersonaje>());
        lider.GetComponent<Animator>().enabled = true;
        lider.GetComponent<SpriteRenderer>().color = Color.white;
        players.Add(currentLider);
        int num = Random.Range(0, 16);
        Instantiate(Soldiers[num], new Vector2(-1.3f, -3f), Quaternion.identity);
        players.Add(num);

        num = Random.Range(0, 16);
        Instantiate(Soldiers[num], new Vector2(1.3f, -3f), Quaternion.identity);
        players.Add(num);

        for (int i = -1; i < 2; i++)
        {
            num = Random.Range(0, 16);
            Instantiate(Soldiers[num], new Vector2(i*1.3f, -0.5f), Quaternion.identity);
            enemies.Add(num);
        }
    }

    public void TeamMatchScene()
    {
        onGame = false;
        GameManager.LoadScene("TeamMatch");
        Resume();
    }

    public void StartSurvivalGameScene()
    {
        onGame = true;
        Nivel = null;
        GameManager.LoadScene("SurvivalBattle");
    }

    public void FollowArrow(GameObject soldier)
    {
        GameObject arrow = Instantiate(FollowArrowPrefab);
        StartCoroutine(arrow.GetComponent<DownArrow>().Select(soldier));
    }

    public void AvtivarIA()
    {
        
        if (Selected != null)
        {
            Selected.IA = true;
            Selected.GetComponent<Soldier>().StateMachineLogic();
        }
    }

    public void DesvtivarIA()
    {
        if (Selected != null)
        {
            Selected.IA = false;
            Selected.state = null;
        }
    }

    public void ChangeSelected(Soldier player)
    {
        Selected = player;
        if (Selected != null)
        {
            Seleccionado.transform.parent = player.transform;
            Seleccionado.transform.position = player.transform.position + Vector3.down * 0.02f;
        }
        else
        {
            Seleccionado.transform.parent = this.transform;
            Seleccionado.transform.position = new Vector3(-3, -10);
        }
        
    }

    private IEnumerator Blink()
    {
        TextMeshProUGUI sr = var1.GetComponent<TextMeshProUGUI>();
        for (int i = 0; i < 10; i++)
        {
            sr.color = Color.red;
            yield return new WaitForSeconds(0.5f);
            sr.color = Color.white;
            yield return new WaitForSeconds(0.5f);
        }
        
        
    }

    public void IAParaTodosHijosDePuta()
    {
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (player.GetComponent<Soldier>())
            {
                player.GetComponent<Soldier>().IA = true;
                player.GetComponent<Soldier>().StateMachineLogic();
            }
        }
    }


    public string[] GetMap()
    {
        return map;
    }

    private string[] GetOriginalMap()
    {

        switch (myMap)
        {
            case MapType.TEMPLE:
                {
                    return new string[]
                    {  //0         1         2      
                       //012345678901234567890
                        "XXXXXXXXXXXXXXXXXXXXX",
                        "XXXXXXXXXXXXXXXXXXXXX",
                        "XXXXXXXXXXXXXXXXXXXXX",
                        "XXXXXXXXXXXXXXXXXXXXX",
                        "XXXXXXXXXXXXXXXXXXXXX",
                        "X                   X",
                        "X   X    X  X   X   X",
                        "X   XX  XX   X  X   X",
                        "X    XXXX     XX    X",
                        "X    XXXX     XX    X",
                        "X                   X",
                        "X    XXXX     XX    X",
                        "X    XXXX     XX    X",
                        "X   XX  XX   X  X   X",
                        "X   X    X  X   X   X",
                        "X                   X",
                        "XXXXXXXXXXXXXXXXXXXXX",
                        "XXXXXXXXXXXXXXXXXXXXX",
                        "XXXXXXXXXXXXXXXXXXXXX",
                        "XXXXXXXXXXXXXXXXXXXXX",
                        "XXXXXXXXXXXXXXXXXXXXX",
                    };
                }
            case MapType.HELL:
                {
                    return new string[]
                    {  //0         1         2      
                       //012345678901234567890
                        "XXXXXXXXXXXXXXXXXXXXX",
                        "XXXXXXXXXXXXXXXXXXXXX",
                        "XXXXXXXXXXXXXXXXXXXXX",
                        "XXXXXXXXXXXXXXXXXXXXX",
                        "XXXXXXXXXXXXXXXXXXXXX",
                        "XXXXXXXXX   XXXXXXXXX",
                        "XXXXXXX       XXXXXXX",
                        "XXXXXX   X X   XXXXXX",
                        "XXXXXX  XX XX  XXXXXX",
                        "XXXXX  XX   XX  XXXXX",
                        "X                   X",
                        "XXXXX  XX   XX  XXXXX",
                        "XXXXXX  XX XX  XXXXXX",
                        "XXXXXX   X X   XXXXXX",
                        "XXXXXXX       XXXXXXX",
                        "XXXXXXXXX   XXXXXXXXX",
                        "XXXXXXXXXXXXXXXXXXXXX",
                        "XXXXXXXXXXXXXXXXXXXXX",
                        "XXXXXXXXXXXXXXXXXXXXX",
                        "XXXXXXXXXXXXXXXXXXXXX",
                        "XXXXXXXXXXXXXXXXXXXXX",
                    };
                }
            case MapType.PRISION:
                {
                    return new string[]
                    {  //0         1         2      
                       //012345678901234567890
                        "XXXXXXXXXXXXXXXXXXXXX",
                        "XXXXXXXXXXXXXXXXXXXXX",
                        "XXXXXXXXXXXXXXXXXXXXX",
                        "XXXXXXXXXXXXXXXXXXXXX",
                        "XXXXXXXXXXXXXXXXXXXXX",
                        "XXXXXXXX  X         X",
                        "XXXXXXXX  X  XXXXXX X",
                        "XXXXXXXX  X  XXXXXX X",
                        "X         X         X",
                        "X                   X",
                        "X                   X",
                        "X                   X",
                        "X         X         X",
                        "X XXXXXX  XXXXXXXXXXX",
                        "X XXXXXX  XXXXXXXXXXX",
                        "X         XXXXXXXXXXX",
                        "XXXXXXXXXXXXXXXXXXXXX",
                        "XXXXXXXXXXXXXXXXXXXXX",
                        "XXXXXXXXXXXXXXXXXXXXX",
                        "XXXXXXXXXXXXXXXXXXXXX",
                        "XXXXXXXXXXXXXXXXXXXXX",
                    };
                }
            case MapType.CLIFF:
                {
                    return new string[]
                    {  //0         1         2      
                       //012345678901234567890
                        "XXXXXXXXXXXXXXXXXXXXX",
                        "XXXXXXXXXXXXXXXXXXXXX",
                        "XXXXXXXXXXXXXXXXXXXXX",
                        "XXXXXXXXXXXXXXXXXXXXX",
                        "XXXXXXXXXXXXXXXXXXXXX",
                        "XXXX          XXXXXXX",
                        "XXXX          XXXXXXX",
                        "XXXX          XXXXXXX",
                        "XXXX           XXXXXX",
                        "XXXX           XXXXXX",
                        "XXXX           XXXXXX",
                        "XXXX           XXXXXX",
                        "XXXX           XXXXXX",
                        "XXXX          XXXXXXX",
                        "XXXX          XXXXXXX",
                        "XXXX          XXXXXXX",
                        "XXXXXXXXXXXXXXXXXXXXX",
                        "XXXXXXXXXXXXXXXXXXXXX",
                        "XXXXXXXXXXXXXXXXXXXXX",
                        "XXXXXXXXXXXXXXXXXXXXX",
                        "XXXXXXXXXXXXXXXXXXXXX",
                    };
                }
            case MapType.BRIDGE:
                {
                    return new string[]
                    {  //0         1         2      
                       //012345678901234567890
                        "XXXXXXXXXXXXXXXXXXXXX",
                        "XXXXXXXXXXXXXXXXXXXXX",
                        "XXXXXXXXXXXXXXXXXXXXX",
                        "XXXXXXXXXXXXXXXXXXXXX",
                        "XXXXXXXXXXXXXXXXXXXXX",
                        "XXXXXXXXXXXXXXXXXXXXX",
                        "XXXXXXXXXXXXXXXXXXXXX",
                        "X                   X",
                        "X                   X",
                        "X                   X",
                        "X                   X",
                        "X                   X",
                        "X      XXXXXXX      X",
                        "X     XXXXXXXXX     X",
                        "XXXXXXXXXXXXXXXXXXXXX",
                        "XXXXXXXXXXXXXXXXXXXXX",
                        "XXXXXXXXXXXXXXXXXXXXX",
                        "XXXXXXXXXXXXXXXXXXXXX",
                        "XXXXXXXXXXXXXXXXXXXXX",
                        "XXXXXXXXXXXXXXXXXXXXX",
                        "XXXXXXXXXXXXXXXXXXXXX",
                    };
                }
            default:
                {
                    return new string[]
                    {  //0         1         2      
                       //012345678901234567890
                        "XXXXXXXXXXXXXXXXXXXXX",
                        "X   X    XX X   X   X",
                        "X   X           X   X",
                        "X   X    XX X   X   X",
                        "X   X    X  X   X   X",
                        "X        X  X       X",
                        "X   X    X  X   X   X",
                        "X   XX  XX   X  X   X",
                        "X    XXXX     XX    X",
                        "X    XXXX     XX    X",
                        "X                   X",
                        "X    XXXX     XX    X",
                        "X    XXXX     XX    X",
                        "X   XX  XX   X  X   X",
                        "X   X    X  X   X   X",
                        "X        X  X       X",
                        "X   X    X  X   X   X",
                        "X   X    XX X   X   X",
                        "X   X           X   X",
                        "X   X    XX X   X   X",
                        "XXXXXXXXXXXXXXXXXXXXX",
                    };
                }
        }
        /*return new string[]
        {  //0         1         2      
           //012345678901234567890
            "XXXXXXXXXXXXXXXXXXXXX",
            "X                   X",
            "X                   X",
            "X                   X",
            "XXXXX XX     XX XXXXX",
            "X       XX XX       X",
            "X       XX XX       X",
            "X      X     X      X",
            "XX XXXX       XXXX XX",
            "X                   X",
            "XX X             X XX",
            "XX XXXXX     XXXXX XX",
            "X      XXX XXX      X",
            "X       XX XX       X",
            "X       XX XX       X",
            "X      XXX XXX      X",
            "XXXXX XX     XX XXXXX",
            "X                   X",
            "X                   X",
            "X                   X",
            "XXXXXXXXXXXXXXXXXXXXX",
        };*/



        //return new string[]
        //{  //0         1         2      
        //     //012345678901234567890
        //    "XXXXXXXXXXXXXXXXXXXXX",
        //    "X                   X",
        //    "X                   X",
        //    "X                   X",
        //    "X                   X",
        //    "X                   X",
        //    "X                   X",
        //    "X                   X",
        //    "X                   X",
        //    "X                   X",
        //    "X                   X",
        //    "X                   X",
        //    "X                   X",
        //    "X                   X",
        //    "X                   X",
        //    "X                   X",
        //    "X                   X",
        //    "X                   X",
        //    "X                   X",
        //    "X                   X",
        //    "XXXXXXXXXXXXXXXXXXXXX",
        //};






    }

    private string[] UpdateMap()
    {
        string[] mapa = GetOriginalMap();
        List<GameObject> gente = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
        foreach (GameObject soldier in GameObject.FindGameObjectsWithTag("Player"))
        {
            gente.Add(soldier);
        }
        foreach (GameObject soldier in GameObject.FindGameObjectsWithTag("Miniom"))
        {
            gente.Add(soldier);
        }

        foreach (GameObject enemy in gente)
        {
            int X = Mathf.RoundToInt(enemy.transform.position.x * 10f);
            int Y = Mathf.RoundToInt(enemy.transform.position.y * 10f);
            mapa = TransformMap(mapa, X, Y);
        }
        return mapa;
    }

    private string[] TransformMap(string[] map, int x, int y)
    {
        string[] newMap = new string[21];
        char[] newRowMap = new char[21];
        StringBuilder sb;
        for (int i = 0; i < 21; i++)
        {
            sb = new StringBuilder();
            for (int j = 0; j < map[0].Length; j++)
            {
                if (i == x && j == y)
                    newRowMap[j] = 'X';
                else
                    newRowMap[j] = map[i][j];
            }
            sb.Append(newRowMap);
            newMap[i] += sb;
        }
        return newMap;
    }

    
}
