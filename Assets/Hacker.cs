using System;
using System.Collections;
using UnityEngine;

public class Hacker : MonoBehaviour
{

    private TouchScreenKeyboard keyboard;

    // User Name
    public new string name = "";

    private int currLevel;
    private int currDiff;

    private enum Screen {Settings, MainMenu, Difficulty, PasswordScreen, Win };

    private Screen currentScreen = Screen.MainMenu;

    private int trys = 0;
    private string key;
    private const string hint = "type menu to go to the application base";
    private readonly string[] lvlone = { "dr slim", "cs4", "random signal", "morgo" };
    private readonly string[] lvltwo = { "zabota", "sarsag", "mo5alfa", "box" };
    private readonly string[] lvlthree = { "amisi", "na5l", "bala7", "GET ME OUT OF EGYPT" };
    private readonly string[] lvl = { "GUC Library", "the police station", "The Egyptian Governement" };
    // Start is called before the first frame update
    private void Start()
    {
        ShowMainMenu(name);
        OnGUI();
    }

    void OnGUI()
    {
        TouchScreenKeyboard.Open("",TouchScreenKeyboardType.Default);
       /* if (GUI.Button(new Rect(10, 50, 200, 100), "Default"))
        {
            keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);
        }
        if (GUI.Button(new Rect(10, 150, 200, 100), "ASCIICapable"))
        {
            keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.ASCIICapable);
        }
        if (GUI.Button(new Rect(10, 250, 200, 100), "Numbers and Punctuation"))
        {
            keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.NumbersAndPunctuation);
        }
        if (GUI.Button(new Rect(10, 350, 200, 100), "URL"))
        {
            keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.URL);
        }
        if (GUI.Button(new Rect(10, 450, 200, 100), "NumberPad"))
        {
            keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.NumberPad);
        }
        if (GUI.Button(new Rect(10, 550, 200, 100), "PhonePad"))
        {
            keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.PhonePad);
        }
        if (GUI.Button(new Rect(10, 650, 200, 100), "NamePhonePad"))
        {
            keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.NamePhonePad);
        }
        if (GUI.Button(new Rect(10, 750, 200, 100), "EmailAddress"))
        {
            keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.EmailAddress);
        }
        if (GUI.Button(new Rect(10, 850, 200, 100), "Social"))
        {
            keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Social);
        }
        if (GUI.Button(new Rect(10, 950, 200, 100), "Search"))
        {
            keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Search);
        }*/
    }

    private void ShowMainMenu(string name)
    {
        trys = 0;
        currentScreen = Screen.MainMenu;
        Terminal.ClearScreen();
        Terminal.WriteLine("Hello "+ name + "\nWhat would you like to hack into?\n\nPress 1 for " + lvl[0] + "\nPress 2 for " + lvl[1] + "\nPress 3 for " + lvl[2] + "\n\nEnter your selection:");
    }

    private void OnUserInput(string input)
    {
        if (input == "menu")
        {
            ShowMainMenu(name);
        }
        else if(input == "quit" || input == "exit")
        {
            Terminal.WriteLine("Shutting down");
            Application.Quit();
        }
        else if (input == "set_name")
        {
            ShowSetNameScreen();
            ShowMainMenu(name);
        }
        else if (currentScreen == Screen.MainMenu)
        {
            MainMenuScreen(input);
        }
        else if (currentScreen == Screen.Difficulty)
        {
            SetDifficultyScreen(input);
        }
        else if (currentScreen == Screen.PasswordScreen)
        {
            PasswordScreen(input, currDiff);
        }
        else if(currentScreen == Screen.Settings)
        {
            Terminal.ClearScreen();
            name = input;
            Terminal.WriteLine("Name set to " + name);
            Terminal.WriteLine(hint);
        }
    }

    private void ShowSetNameScreen()
    {
        currentScreen = Screen.Settings;
        Terminal.ClearScreen();
        Terminal.WriteLine("Enter your name");
    }

    private void PasswordScreen(string input, int diff)
    {
        string[] currLevelWords = fetchPasswords(diff);

        StartCoroutine(cracking());


        if (diff == 0)
        {

            foreach (string key in currLevelWords)
            {
                if (input.ToLower() == key.ToLower())
                {
                    HackedScreen(true);
                    return;
                }
            }
            showFirstLevelHints(currLevelWords);
        }
        else if (diff == 1)
        {
            fetchPassword(currLevelWords);
            if (input.ToLower() == key)
            {
                HackedScreen(true);
                return;
            }
            showLevelTwoThreeHint("Wrong decryption key\nDecrypted bits hijacked: " + key.Anagram());

        }

        else
        {
            key = currLevelWords[UnityEngine.Random.Range(0, currLevelWords.Length)];
            if (input.ToLower() == key)
            {
                HackedScreen(true);
                return;
            }
            showLevelTwoThreeHint("Wrong decryption key\nSystem use Denuvo keys\nold patched key is " + key.Anagram());
        }
        
        trys++;
        if (trys >= 3)
        {
            HackedScreen(false);
            Terminal.WriteLine("Your IP Has been recorded and is being traced by the system\nToo many errors activated the system's firewall\nRun!");

        }
    }

    private void showLevelTwoThreeHint(string hint)
    {
        Terminal.WriteLine(hint);
    }

    private void fetchPassword(string[] currLevelWords)
    {
        if (trys == 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, currLevelWords.Length);
            key = currLevelWords[randomIndex];
            print(randomIndex);
        }
    }

    private static void showFirstLevelHints(string[] currLevelWords)
    {
        Terminal.WriteKey("Wrong decryption key\nLeaked data dump is found: ");
        foreach (string key in currLevelWords)
        {
            Terminal.WriteKey(key.Anagram());
        }
        Terminal.WriteLine("");
    }

    IEnumerator cracking()
    {
        Terminal.WriteLine("Cracking...");
        StartCoroutine(waiter());
        yield return null;
    }

    IEnumerator waiter()
    {
        float counter = 0;
        float waitTime = 3;
        while (counter < waitTime)
        {
            counter += Time.deltaTime;
            print("We have waited for " + counter);
            Debug.Log("We have waited for " + counter);
        }
        yield return null;
    }

    private string[] fetchPasswords(int diff)
    {
        string[] currLevelWords;

        if (currLevel == 1)
        {
            currLevelWords = lvlone;
        }
        else if (currLevel == 2)
        {
            currLevelWords = lvltwo;
        }
        else
        {
            currLevelWords = lvlthree;
        }

        return currLevelWords;

    }

    private void HackedScreen(bool win)
    {
        currentScreen = Screen.Win;
        if (win)
        {
            showAward();
        }
        else
        {
            Terminal.WriteLine("Ma3lesh");
        }


    }

    private void showAward()
    {
        Terminal.ClearScreen();
        switch (currLevel)
        {
            case 1:
                switch (currDiff)
                {
                    case 0:
                        Terminal.WriteLine("                           .-/+oso+.                   ");
                        Terminal.WriteLine("                       .://:-.-+yys:                   ");
                        Terminal.WriteLine("                    `:/:`    `/so::h                   ");
                        Terminal.WriteLine("                   -----..``/o/.  -d                   ");
                        Terminal.WriteLine("                   -::-``..-.d+   /d                   ");
                        Terminal.WriteLine("                    .:-`     yd`  yy                   ");
                        Terminal.WriteLine("                     `.-.` --/mh/ym:                   ");
                        Terminal.WriteLine("                        `.--:-smmms                    ");
                        Terminal.WriteLine("                             ` -:.                     ");
                        Terminal.WriteLine("             `-////:`  -:      ::     -/+++/.          ");
                        Terminal.WriteLine("           -yy/-..-:`  sd      yh  `oh+:...-.          ");
                        Terminal.WriteLine("          :m/          sd      yh  hh`                 ");
                        Terminal.WriteLine("          sm    .ooyy  sd      yh .m+                  ");
                        Terminal.WriteLine("          -mo      sd  +m.    `ds  yd.                 ");
                        Terminal.WriteLine("           .shs////hd  `od+::+hs`   +hy+/:/+:          ");
                        Terminal.WriteLine("              `---.`     `-::-`       `-::-.`          ");
                        Terminal.WriteLine("You found your upcomming final exam!\nGo study it now for an A+");
                        break;
                    case 1:
                        Terminal.WriteLine(@"          /ydhhhhhhhhhhhhddh:           
         /dyo+//////////++oyd:          
        `dho++/:------::/++ohs          
        .myo+//::-----:://+ohy          
        .myoo+//::---://++osys          
        `dyossyyyo::/oyhhyyssh-         
        :hsoosysoo+//oosyssosho         
        -ss+/////++:/o+////+sy+         
        :oy+/:::/+o//s+/://+ys+         
        `:+o+++++oosssoo++oss:`         
          `ooo++osooossso+os/           
           :oo+++oooossooosy.           
           .ssoo+++ooooosyyy            
           `syysooooossshhys:`          
          -/ossyyyyyyyhhhys/+do-        
        `+m/-:+ssssssyyyso:-:sNmh+:.`   
     ./odmd:.``.:+oooso/-...:+NNNNmmmdyo
`:+ydmmNNmd/..``` `:+:. ``.-:smNNNNmNNNN
mmNNNmmNNNm/.```  .:--:-`.-.-hNNNNNNNNNN");
                        Terminal.WriteLine("You hacked the GUC Database\nAll grades changed to A+\nYour GPA is 0.7\nYou can now say you are better than Petter Wagih");
                        break;
                    case 2:

                        Terminal.WriteLine("You own the GUC\nDr Sameh Will find you and will kill you");
                        Terminal.WriteLine(@"                               ``       
                       .-:+osssyyss+-   
                  `-/osyyyyyyyyyyyyys-  
              `-/syyso+/:::/+syyyyyys.  
           .:oss+:-`        `syyyyys:-  
        `:oso/.            .oyyyys:-dh  
      -+so:`             -oyyys+-  :mm  
   ``....``           `:oyyso:`    :mm` 
  -::::::::::--.``  .+sys+:`       :mm. 
  :::::::::::::::::--.-:+y/        /mm. 
  .:::::::.`` ```..-::./mms        omm` 
   .:::::-           ``-mmm`       hmm` 
    .-::::`            `mmmo      .mmh  
      .::::.            ymmm:     ymmo  
       `.::::.`    .--.`:mmmm/  .ymmm-  
          .-:::-.` `:::-`hmmmmddmmmmy   
            `.-:::---:::.-mmmmmmmmmd.   
                `.--:::::`:mmmmmmmd-    
                     ``... .sdmmh+`     
");
                        break;
                }
                
                break;
        }
    
    }

    private void MainMenuScreen(string input)
    {
        input = input.ToLower();
        if (input != "007" && int.TryParse(input, out int menuIndex))
        {
            if (menuIndex < 4 && menuIndex > 0)
            {
                currLevel = menuIndex;
                currentScreen = Screen.Difficulty;
                Terminal.ClearScreen();
                Terminal.WriteLine("Enter the last known patch level (0/2)");
                Terminal.WriteLine(hint);
            }
            else
            {
                Terminal.WriteLine("Unkown Level");
            }
        }
        else if (input == "hint")
        {
            Terminal.WriteLine(hint);
        }
        else if (input == "set name")
        {
            ShowSetNameScreen();
        }
        else if (input == "007")
        {
            Terminal.WriteLine("Enter valid level number please Mr James Bond!");
        }
        else if (input == "ossama" || input == "ossama akram")
        {
            Terminal.WriteLine("Ossama 3alama!");
        }
        else if (input == "osama")
        {
            Terminal.WriteLine("YOUR IP HAS BEEN CRACKED BY O'SS'AMA!\nYOU WILL BE HACKED\nRUN!");
        }
        else if (input == "reem" || input == "recko")
        {
            Terminal.WriteLine("Did you meant shawerma el reem ? shall i order shawerma fera5 for u bruh ?");
        }
        else if (input == "ma queen" || input == "ma kween" || input == "my queen")
        {
            Terminal.WriteLine("You know nothing about hacking");
        }
        else if (input.Contains("jon snow"))
        {
            Terminal.WriteLine("You know nothing about hacking");
        }
        else if (input == "help")
        {
            Terminal.WriteLine("'menu' for showing the availble levels\n'set name' to change your name\n'help' to show the help documantation\nenter the level you desire by entering it's index");
        }
        else
        {
            Terminal.WriteLine("Unkown command input help for list of all commands");
        }
    }

    private void SetDifficultyScreen(string diffLevel)
    {
        if (int.TryParse(diffLevel, out int diff))
        {
            if (diff <= 2 && diff >= 0)
            {
                if (diff == 1)
                    Terminal.WriteLine("The System has improved it's Firewall\nmore security was applied");
                else if (diff == 0)
                    Terminal.WriteLine("The System firewall is running on old unpatched software\nSecurity level is low");
                currDiff = diff;
                StartGame(lvl[currLevel-1]);
            }
            else
            {
                Terminal.WriteLine("Unkown Level");
            }
        }
        else
        {
            Terminal.WriteLine("Enter a valid patch level to procede");
        }

    }

    private void StartGame(string level)
    {
        Terminal.ClearScreen();
        currentScreen = Screen.PasswordScreen;
        Terminal.WriteLine("Penetration Started on "+level);
        Terminal.WriteLine("Please enter System decryption Key");
    }
}
