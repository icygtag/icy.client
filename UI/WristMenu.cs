using MalachiTemp.Backend;
using MalachiTemp.UI;
using MalachiTemp.Utilities;
using Photon.Pun;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Text = UnityEngine.UI.Text;


namespace MalachiTemp.UI
{
    
    public class ButtonInfo
    {
        public string buttonText = "Error";
        public Action method = null;
        public Action disableMethod = null;
        public bool? enabled = false;
        public bool? nontoggleable = false;
        public string toolTip = "This button doesn't have a tooltip/tutorial";
    }
    internal class WristMenu : MonoBehaviour
    {
        public static string MenuTitle = "<color=Red>I</color><color=Orange>c</color><color=Yellow>y</color><color=Green>.</color><color=Blue>C</color><color=Indigo>l</color><color=Violet>i</color><color=Red>e</color><color=Orange>n</color><color=Yellow>t</color>\r\n"; // this is the menu name, you can change it to whatever you want
        public static Font MenuFont = Resources.GetBuiltinResource<Font>("Arial.ttf"); // font of the text on the menu
        // Double click a grey square to open it, click the - in the box to the left of "#region" to close it

        #region Main
        public static List<ButtonInfo> buttons = new List<ButtonInfo> // main
        {
            // DO NOT ADD OR REMOVE ANY BUTTONS IN HERE OR IT WILL MESS UP THE WHOLE MENU
            // to rename a category, rename the category button u want in here
            new ButtonInfo { buttonText = "Save Buttons", method =() => Mods.Save1(), enabled = false, nontoggleable = true, toolTip = "Saves all currently enabled buttons!"},
            new ButtonInfo { buttonText = "Load Buttons", method =() => Mods.Load1(), enabled = false, nontoggleable = true, toolTip = "Load the buttons you saved!"},
            new ButtonInfo { buttonText = "Settings", method =() => Mods.Settings(), enabled = false, toolTip = "Go to Settings!"},
            // if u dont want categories delete the category buttons below and put what mods u want below
            
            new ButtonInfo { buttonText = "<color=#00008B>Visual Mods</color>", method =() => Mods.Cat1(), enabled = false, toolTip = "Go to Category 1!"},
            new ButtonInfo { buttonText = "<color=#00FF00>Fun Mods</color>", method =() => Mods.Cat2(), enabled = false, toolTip = "Go to Category 2!"},
            new ButtonInfo { buttonText = "<color=#00FF00>Saftey Mods</color>", method =() => Mods.Cat3(), enabled = false, toolTip = "Go to Category 3!"},
            new ButtonInfo { buttonText = "<color=#FF0000>Advantage Mods</color>", method =() => Mods.Cat4(), enabled = false, toolTip = "Go to Category 4!"},
            new ButtonInfo { buttonText = "<color=#00008B>Master Client Mods</color>", method =() => Mods.Cat5(), enabled = false, toolTip = "Go to Category 5!"},
            new ButtonInfo { buttonText = "<color=#00008B>Rig Mods</color>", method =() => Mods.Cat6(), enabled = false, toolTip = "Go to Category 5!"},
            new ButtonInfo { buttonText = "<color=#00008B>Dangerous Mods</color>", method =() => Mods.Cat7(), enabled = false, toolTip = "Go to Category 5!"},
            
            // u can change the button name and the tool tip to whatever u want just make sure that when u change the button name u change the "Exit" thing for that category
        };
        #endregion
        #region Settings
        public static List<ButtonInfo> settingsbuttons = new List<ButtonInfo> // settings
        {
            new ButtonInfo { buttonText = "Exit Settings", method =() => Mods.Settings(), enabled = false, toolTip = "Go To Main!"},
            new ButtonInfo { buttonText = "Save Settings", method =() => Mods.Save(), enabled = false, nontoggleable = true, toolTip = "Save Your Settings!"},
            new ButtonInfo { buttonText = "Load Settings", method =() => Mods.Load(), enabled = false, nontoggleable = true, toolTip = "Load Your Settings!"},
            new ButtonInfo { buttonText = "FPS Boost", method =() => Mods.FPSboost(), disableMethod =() => Mods.fixFPS(), enabled = false, toolTip = "Boost Your FPS!"},
            new ButtonInfo { buttonText = "No Notis", method =() => Mods.Changenoti(), enabled = false, nontoggleable = true, toolTip = "Turn off Notis!"},
            new ButtonInfo { buttonText = "Toggle FPS & Page Counter", method =() => Mods.ChangeFPS(), enabled = false, nontoggleable = true, toolTip = "Turn off or on the FPS & Page counter!"},
            new ButtonInfo { buttonText = "Change Gun & Hand Orb Color", method =() => Mods.ChangeOrbColor(), enabled = false, nontoggleable = true, toolTip = "Change the color of the gun and hand orbs!"},
            new ButtonInfo { buttonText = "Change ESP Color", method =() => Mods.ChangeVisualColor(), enabled = false, nontoggleable = true, toolTip = "Change the color of the ESP mods!"},
            new ButtonInfo { buttonText = "Change Platform Enable", method =() => Mods.Changeplat(), enabled = false, nontoggleable = true, toolTip = "Change how you use platform mods!"},
            new ButtonInfo { buttonText = "Change Disconnect Location", method =() => Mods.Changedisconnect(), enabled = false, nontoggleable = true, toolTip = "Change the location of the disconnect button!"},
            new ButtonInfo { buttonText = "Change Menu Location", method =() => Mods.Changemenu(), enabled = false, nontoggleable = true, toolTip = "Change the location of the menu!"},
            new ButtonInfo { buttonText = "Change Next & Prev", method =() => Mods.Changepagebutton(), enabled = false, nontoggleable = true, toolTip = "Change the location of the next and prev page buttons!"},
            new ButtonInfo { buttonText = "First Menu Color", method =() => Mods.ThemeChangerV1(), enabled = false, nontoggleable = true, toolTip = "Change the first color the menu fades to!"},
            new ButtonInfo { buttonText = "Second Menu Color", method =() => Mods.ThemeChangerV2(), enabled = false, nontoggleable = true, toolTip = "Change the second color the menu fades to!"},
            new ButtonInfo { buttonText = "Disabled Button Color", method =() => Mods.ThemeChangerV3(), enabled = false, nontoggleable = true, toolTip = "Change the color of buttons when they are disabled!"},
            new ButtonInfo { buttonText = "Enabled Button Color", method =() => Mods.ThemeChangerV4(), enabled = false, nontoggleable = true, toolTip = "Change the color of buttons when they are enabled!"},
            new ButtonInfo { buttonText = "Enabled Button Color", method =() => Mods.ThemeChangerV5(), enabled = false, nontoggleable = true, toolTip = "Change the color of the text on buttons when they are enabled!"},
            new ButtonInfo { buttonText = "Disabled Text Color", method =() => Mods.ThemeChangerV6(), enabled = false, nontoggleable = true, toolTip = "Change the color of the text on buttons when they are disabled!"},
            new ButtonInfo { buttonText = "Button Sound", method =() => Mods.ThemeChangerV7(), enabled = false, nontoggleable = true, toolTip = "Change the sound of buttons!"},
            new ButtonInfo { buttonText = "Font Change", method =() => Mods.FonyChangeV1(), enabled = false, nontoggleable = true, toolTip = "Change the sound of buttons!"},
        };
        #endregion
        #region Visuals
        // all your premade mods are in here, you can move them anywhere you would like
        public static List<ButtonInfo> CatButtons1 = new List<ButtonInfo>
        {
            new ButtonInfo { buttonText = "Exit Category 1", method =() => Mods.Cat1(), enabled = false, toolTip = "Go to Main!"}, 
            new ButtonInfo { buttonText = "Tracers", method =() => Mods.Tracers(), enabled = false, toolTip = "Lines towards everyone!"},
            
            new ButtonInfo { buttonText = "Casual Bone ESP.", method =() => Mods.CasualBoneESP(), enabled = false, toolTip = "Bone ESP For Casuals!"},
            

        };
        #endregion
        #region Fun
        public static List<ButtonInfo> CatButtons2 = new List<ButtonInfo>
        {
            new ButtonInfo { buttonText = "Exit Category 2", method =() => Mods.Cat2(), enabled = false, toolTip = "Go to Main!"},
            new ButtonInfo { buttonText = "Zero Gravity", method =() => Mods.ZeroGravity(), enabled = false, toolTip = "That Borthers Floating!"},
            new ButtonInfo { buttonText = "Punch Mod", method =() => Mods.PunchMod(), enabled = false, toolTip = "Owie!"},
            new ButtonInfo { buttonText = "Telekenisis", method =() => Mods.Telekinesis(), enabled = false, toolTip = "whee!!"},
            new ButtonInfo { buttonText = "Rainbow Braclet", method =() => Mods.RainbowBracelet(), enabled = false, toolTip = "whee!!"},
            new ButtonInfo { buttonText = "remove Rainbow Bracelet", method =() => Mods.RemoveRainbowBracelet(), enabled = false, toolTip = "whee!!"},
            new ButtonInfo { buttonText = "Teleport Gun [RG]", method =() => Mods.TeleportGun(), enabled = false, toolTip = "whee!!"},
            new ButtonInfo { buttonText = "Max Out Level Badge", method =() => Mods.MaxQuestScore(), enabled = false, toolTip = "whee!!"},
            new ButtonInfo { buttonText = "Hover Board Gun [RG]", method =() => Mods.HoverBoardGun(), enabled = false, toolTip = "whee!!"},
            new ButtonInfo { buttonText = "Global Hoverboard", method =() => Mods.GlobalHoverboard(), enabled = false, toolTip = "whee!!"},
            new ButtonInfo { buttonText = "Water Splash Gun [RG]", method =() => Mods.WaterSplashGun(), enabled = false, toolTip = "whee!!"},
            new ButtonInfo { buttonText = "Water Bender [GS]", method =() => Mods.WaterSplashHands(), enabled = false, toolTip = "whee!!"},
            new ButtonInfo { buttonText = "BeyBlade [A]", method =() => Mods.Beyblade(), enabled = false, toolTip = "whee!!"},
            new ButtonInfo { buttonText = "Helicopter [Y]", method =() => Mods.Helicopter(), enabled = false, toolTip = "whee!!"},


        };
        #endregion
        #region Saftey
        public static List<ButtonInfo> CatButtons3 = new List<ButtonInfo>
        {
            new ButtonInfo { buttonText = "Exit Category 3", method =() => Mods.Cat3(), enabled = false, toolTip = "Go to Main!"},
            new ButtonInfo { buttonText = "AntiReport", method =() => Mods.AntiReportDisconnect(), enabled = false, toolTip = "Kicks You If Reported!"},
            new ButtonInfo { buttonText = "Fake Meta Menu [B]", method =() => Mods.FakeOculusMenu(), enabled = false, toolTip = "Meta Menu For Checks"},

        };
        #endregion
        #region Advantage
        public static List<ButtonInfo> CatButtons4 = new List<ButtonInfo>
        {
            new ButtonInfo { buttonText = "Exit Category 4", method =() => Mods.Cat4(), enabled = false, toolTip = "Go to Main!"},
            new ButtonInfo { buttonText = "Platforms [GS]", method =() => Mods.Platforms(), enabled = false, toolTip = "Platforms!"},
            new ButtonInfo { buttonText = "Invis Platforms [GS]", method =() => Mods.Invisableplatforms(), enabled = false, toolTip = "Invisable Platforms!"},
            new ButtonInfo { buttonText = "Pull Mod [RG]", method =() => Mods.PullMod(), enabled = false, toolTip = "Pull Mod"},

        };
        #endregion
        #region MasterClient
        public static List<ButtonInfo> CatButtons5 = new List<ButtonInfo>
        {
            new ButtonInfo { buttonText = "Exit Category 5", method =() => Mods.Cat5(), enabled = false, toolTip = "Go to Main!"},
            new ButtonInfo { buttonText = "Spawn Blue Lucy", method =() => Mods.SpawnBlueLucy(), enabled = false, toolTip = "whee!!"},
            new ButtonInfo { buttonText = "Make Lucy Chase You", method =() => Mods.LucyAttackSelf(), enabled = false, toolTip = "whee!!"},
            new ButtonInfo { buttonText = "Spawn Red Lucy", method =() => Mods.SpawnRedLucy(), enabled = false, toolTip = "whee!!"},
            new ButtonInfo { buttonText = "Despawn Lucy", method =() => Mods.DespawnLucy(), enabled = false, toolTip = "whee!!"},
        };
        #endregion
        #region Rig Stuff
        public static List<ButtonInfo> CatButtons6 = new List<ButtonInfo>
        {
            new ButtonInfo { buttonText = "Exit Category 6", method =() => Mods.Cat6(), enabled = false, toolTip = "Go to Main!"},
            new ButtonInfo { buttonText = "RigGun [RG]", method =() => Mods.ExampleOnHowToUseGunLibV2(), enabled = false, toolTip = "Bye"},
            new ButtonInfo { buttonText = "Grab Rig [RG]", method =() => Mods.GrabRig(), enabled = false, toolTip = "Bye"},
            new ButtonInfo { buttonText = "Spaz Rig [B]", method =() => Mods.SpazRig(), enabled = false, toolTip = "Bye"},
            


        };
        #endregion
        #region Dangerous
        public static List<ButtonInfo> CatButtons7 = new List<ButtonInfo>
        {
            new ButtonInfo { buttonText = "Exit Category 7", method =() => Mods.Cat7(), enabled = false, toolTip = "Go to Main!"},
            new ButtonInfo { buttonText = "Ban All In Party", method =() => Mods.BanAllInParty(), enabled = false, toolTip = "Sends Your Party To Code KKK To Ban Them"},
            new ButtonInfo { buttonText = "Crash On Touch [Monke Magic]", method =() => Mods.CrashOnTouch(), enabled = false, toolTip = "Crashes Anyone Who Touches You."},
        };
        #endregion
        #region Category 8
        public static List<ButtonInfo> CatButtons8 = new List<ButtonInfo>
        {
            new ButtonInfo { buttonText = "Exit Category 8", method =() => Mods.Cat8(), enabled = false, toolTip = "Go to Main!"},
            new ButtonInfo { buttonText = "PLACEHOLDER", method =() => Mods.PLACEHOLDER(), enabled = false, toolTip = "PLACEHOLDER!"},
            new ButtonInfo { buttonText = "PLACEHOLDER", method =() => Mods.PLACEHOLDER(), enabled = false, toolTip = "PLACEHOLDER!"},
            new ButtonInfo { buttonText = "PLACEHOLDER", method =() => Mods.PLACEHOLDER(), enabled = false, toolTip = "PLACEHOLDER!"},
        };
        #endregion
        #region Category 9
        public static List<ButtonInfo> CatButtons9 = new List<ButtonInfo>
        {
            new ButtonInfo { buttonText = "Exit Category 9", method =() => Mods.Cat9(), enabled = false, toolTip = "Go to Main!"},
            new ButtonInfo { buttonText = "PLACEHOLDER", method =() => Mods.PLACEHOLDER(), enabled = false, toolTip = "PLACEHOLDER!"},
            new ButtonInfo { buttonText = "PLACEHOLDER", method =() => Mods.PLACEHOLDER(), enabled = false, toolTip = "PLACEHOLDER!"},
            new ButtonInfo { buttonText = "PLACEHOLDER", method =() => Mods.PLACEHOLDER(), enabled = false, toolTip = "PLACEHOLDER!"},
        };
        #endregion
        #region Category 10
        public static List<ButtonInfo> CatButtons10 = new List<ButtonInfo>
        {
            new ButtonInfo { buttonText = "Exit Category 10", method =() => Mods.Cat10(), enabled = false, toolTip = "Go to Main!"},
            new ButtonInfo { buttonText = "PLACEHOLDER", method =() => Mods.PLACEHOLDER(), enabled = false, toolTip = "PLACEHOLDER!"},
            new ButtonInfo { buttonText = "PLACEHOLDER", method =() => Mods.PLACEHOLDER(), enabled = false, toolTip = "PLACEHOLDER!"},
            new ButtonInfo { buttonText = "PLACEHOLDER", method =() => Mods.PLACEHOLDER(), enabled = false, toolTip = "PLACEHOLDER!"},
        };
        #endregion
        #region Other Stuff
        public static string[] CustomBoardTexts = new string[] // make sure to keep them in order
        {
            "Thanks for using Icy.Client!", // motd title
            "Hello :>", // coc title
            "<color=#00008B>Icy.Client</color>  Has  <color=#00FF00>19</color>  Mods So Far! So Thanks For Choosing Us! But,  <color=#FF0000>I, Icy Am Not Responsible For Any Bans Using This Menu.</color>  Have Fun And <color=#FF0000> Even Though Your A Modder, </color> <color=#00FF00>Be Nice And Have Fun!</color>\r\n", // coc description,
            "<color=#00008B>Icy.Client</color>  Has  <color=#00FF00>19</color>  Mods So Far! So Thanks For Choosing Us! But,  <color=#FF0000>I, Icy Am Not Responsible For Any Bans Using This Menu.</color>  Have Fun And <color=#FF0000> Even Though Your A Modder, </color> <color=#00FF00>Be Nice And Have Fun!</color>\r\n" // motd description
        };
        public static string FolderName = "Icy.Client Settings"; // this is the name for the folder that holds ur saved settings and buttons and other stuff
        #endregion 
        #region Colors
        // i recommend keeping all this stuff the same, you can always mess with the theme changers to get the colors and stuff you want
        public static bool ChangingColors = true; // makes the menu fade colors if true. i would keep this on true
        public static Color FirstColor = Color.blue; // if changingcolors is true, this is the main first color the menu will be
        public static Color SecondColor = Color.black; // if changingcolors is true, this is the main second color the menu will be
        public static Color NormalColor = Color.white; // if changingcolors is false, this is the main color that menu will be
        public static Color ButtonColorDisable = Color.yellow; // this is the main color of the buttons when they r disabled
        public static Color ButtonColorEnabled = Color.magenta; // this is the main color of the buttons when they r enabled
        public static Color EnableTextColor = Color.black; // this is the main color of the button text when they r enabled
        public static Color DIsableTextColor = Color.black; // this is the main color of the button text when they r disabled
        public static Color MenuTitleColor = Color.white; // this is the main color of the menu name
        public static Color ToolTipColor = Color.white; // this is the main color of the tooltips
        public static Color DisconnectButtonColor = Color.red; // this is the color of the disconnect button
        public static Color DisconnectTextColor = Color.white; // this is the color of the disconnect text
        public static Color NextPrevButtonColor = Color.black; // this is the color of the Next & Prev buttons
        public static Color NextPrevTextColor = Color.white; // this is the color of the Next & Prev text
        #endregion
        #region Scales
        // DO NOT MESS WITH THESE UNLESS YOU KNOW WHAT YOU ARE DOING
        public static Vector3 MenuScale = new Vector3(0.1f, 1f, 1f) * 1f; // size of the menu
        public static Vector3 MenuPos = new Vector3(0.05f, 0f, 0f) * 1f; // position of the menu
        public static Vector3 PointerScale = new Vector3(0.01f, 0.01f, 0.01f); // size of the button presser
        public static Vector3 PointerPos = new Vector3(0f, -0.1f, 0f); // position of the button presser
        public static Vector3 ToolTipPos = new Vector3(0.06f, 0f, -0.18f) * 1f; // position of the tool tips
        public static Vector2 ToolTipScale = new Vector2(0.2f, 0.03f) * 1f; // size of the tool tips
        public static Vector3 MenuTitlePos = new Vector3(0.06f, 0f, 0.175f); // position of the menu title
        public static Vector2 MenuTitleScale = new Vector2(0.28f, 0.05f); // size of the menu titley
        public static Vector3 ButtonScale = new Vector3(0.09f, 0.8f, 0.08f); // size of the mod buttons
        public static Vector2 ButtonTextScale = new Vector2(0.2f, 0.03f) * 1f; // size of the text on mod buttons
        #endregion
        #region Controller Inputs
        // controller buttons
        public static bool gripDownR; // Right Grip
        public static bool triggerDownR; // Right Trigger
        public static bool abuttonDown; // A
        public static bool bbuttonDown; // B
        public static bool xbuttonDown; // X
        public static bool ybuttonDown; // Y
        public static bool gripDownL; // Left Grip
        public static bool triggerDownL; // Left Trigger
        #endregion
        #region Menu Stuff
        // ingore everything past here
        public static int lastPressedButtonIndex = -1;
        public static GameObject menu = null;
        public static GameObject canvasObj = null;
        public static GameObject reference = null;
        public static int pageNumber = 0;
        public static WristMenu instance = new WristMenu();
        public static GameObject menuObj;
        public static int selectedButton = 1;
        public static Text tooltipText;
        public static string tooltipString;
        public static bool toggle = false;
        public static bool toggle1 = false;
        public static bool toggle2 = false;
        public static bool toggle3 = false;
        public static int pageSize = 4;
        public static bool toggle4 = false;
        public static int ClickCooldown = 10;
        public static int MaxNotis = 5;
        public static bool isrounded = true;
        
        void Update()
        {
            try
            {
                if (Time.time > Mods.balll435342111 + 0.1f && Mods.FPSPage)
                {
                    Mods.balll435342111 = Time.time;
                    int num = Mathf.RoundToInt(1f / Time.deltaTime);
                    titiel.text = MenuTitle + string.Format("\n Fps: {0} | Page: {1}", num, pageNumber + 1);
                }
                gripDownL = ControllerInputPoller.instance.leftGrab;
                gripDownR = ControllerInputPoller.instance.rightGrab;
                triggerDownL = ControllerInputPoller.instance.leftControllerIndexFloat == 1f;
                triggerDownR = ControllerInputPoller.instance.rightControllerIndexFloat == 1f;
                abuttonDown = ControllerInputPoller.instance.rightControllerPrimaryButton;
                bbuttonDown = ControllerInputPoller.instance.rightControllerSecondaryButton;
                xbuttonDown = ControllerInputPoller.instance.leftControllerPrimaryButton;
                ybuttonDown = ControllerInputPoller.instance.leftControllerSecondaryButton;
                if (Mods.change7 == 5 && !menu.GetComponent<Rigidbody>())
                {
                    if (triggerDownL)
                    {
                        if (!toggle)
                        {
                            Toggle("PreviousPage");
                            GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(Mods.ButtonSound, false, 0.1f);
                            toggle = true;
                        }
                    }
                    else
                    {
                        toggle = false;
                    }
                    if (triggerDownR)
                    {
                        if (!toggle1)
                        {
                            Toggle("NextPage");
                            GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(Mods.ButtonSound, false, 0.1f);
                            toggle1 = true;
                        }
                    }
                    else
                    {
                        toggle1 = false;
                    }
                }
                // stuff for left hand menu
                if (ybuttonDown && !Mods.right)
                {
                    if (menu == null)
                    {
                        instance.Draw();
                    }
                    else if (!Mods.right)
                    {
                        menu.transform.position = GorillaLocomotion.GTPlayer.Instance.leftControllerTransform.position; // moves the menu to your left hand
                        menu.transform.rotation = GorillaLocomotion.GTPlayer.Instance.leftControllerTransform.rotation;
                        if (reference == null)
                        {
                            reference = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                            reference.name = "buttonPresser";
                        }
                        reference.transform.parent = GorillaLocomotion.GTPlayer.Instance.rightControllerTransform; // attaches the pointer to your right hand
                        reference.transform.localPosition = PointerPos; // moves it out a lil
                        reference.transform.localScale = PointerScale; // makes it small
                        // makes the pointer follow the color of the menu
                        if (ChangingColors)
                        {
                            reference.GetComponent<Renderer>().material.color = FirstColor;
                        }
                        else
                        {
                            reference.GetComponent<Renderer>().material.color = NormalColor;
                        }
                    }
                    if (menu.GetComponent<Rigidbody>())
                    {
                        Destroy(menu.GetComponent<Rigidbody>());
                    }
                }
                else if (!ybuttonDown && !Mods.right && !menu.GetComponent<Rigidbody>())
                {
                    // this makes the menu throwable
                    Destroy(reference);
                    reference = null;
                    menu.AddComponent<Rigidbody>();
                    menu.GetComponent<Rigidbody>().isKinematic = false;
                    menu.GetComponent<Rigidbody>().useGravity = true;
                    menu.GetComponent<Rigidbody>().velocity = GorillaLocomotion.GTPlayer.Instance.leftHandCenterVelocityTracker.GetAverageVelocity(true, 0f, false);
                }
                // stuff for right hand menu
                if (bbuttonDown && Mods.right)
                {
                    if (menu == null)
                    {
                        instance.Draw();
                    }
                    else if (Mods.right)
                    {
                        menu.transform.position = GorillaLocomotion.GTPlayer.Instance.rightControllerTransform.position;
                        menu.transform.rotation = GorillaLocomotion.GTPlayer.Instance.rightControllerTransform.rotation;
                        menu.transform.RotateAround(menu.transform.position, menu.transform.forward, 180f);
                        if (reference == null)
                        {
                            reference = GameObject.CreatePrimitive(0);
                            reference.name = "buttonPresser";
                        }
                        reference.transform.parent = GorillaLocomotion.GTPlayer.Instance.leftControllerTransform;
                        reference.transform.localPosition = PointerPos;
                        reference.transform.localScale = PointerScale;
                        if (ChangingColors)
                        {
                            reference.GetComponent<Renderer>().material.color = FirstColor;
                        }
                        else
                        {
                            reference.GetComponent<Renderer>().material.color = NormalColor;
                        }
                    }
                    if (menu.GetComponent<Rigidbody>())
                    {
                        Destroy(menu.GetComponent<Rigidbody>());
                    }
                }
                else if (!abuttonDown && Mods.right && !menu.GetComponent<Rigidbody>())
                {
                    Destroy(reference);
                    reference = null;
                    menu.AddComponent<Rigidbody>();
                    menu.GetComponent<Rigidbody>().isKinematic = false;
                    menu.GetComponent<Rigidbody>().useGravity = true;
                    menu.GetComponent<Rigidbody>().velocity = GorillaLocomotion.GTPlayer.Instance.rightHandCenterVelocityTracker.GetAverageVelocity(true, 0f, false);
                }
                // random STUFF
                foreach (ButtonInfo buttonInfo in settingsbuttons)
                {
                    if (buttonInfo.method == null) continue;

                    if (buttonInfo.enabled == true && buttonInfo.nontoggleable == false)
                    {
                        buttonInfo.method.Invoke();
                    }
                    if (buttonInfo.enabled == true && buttonInfo.nontoggleable == true)
                    {
                        buttonInfo.method.Invoke();
                        Mods.DisableButton(buttonInfo.buttonText);
                    }
                    if (buttonInfo.enabled == false && buttonInfo.disableMethod != null)
                    {
                        buttonInfo.disableMethod.Invoke();
                    }
                }
                foreach (ButtonInfo buttonInfo in buttons)
                {
                    if (buttonInfo.method == null) continue;

                    if (buttonInfo.enabled == true && buttonInfo.nontoggleable == false)
                    {
                        buttonInfo.method.Invoke();
                    }
                    if (buttonInfo.enabled == true && buttonInfo.nontoggleable == true)
                    {
                        buttonInfo.method.Invoke();
                        Mods.DisableButton(buttonInfo.buttonText);
                    }
                    if (buttonInfo.enabled == false && buttonInfo.disableMethod != null)
                    {
                        buttonInfo.disableMethod.Invoke();
                    }
                }
                foreach (ButtonInfo buttonInfo in CatButtons1)
                {
                    if (buttonInfo.method == null) continue;

                    if (buttonInfo.enabled == true && buttonInfo.nontoggleable == false)
                    {
                        buttonInfo.method.Invoke();
                    }
                    if (buttonInfo.enabled == true && buttonInfo.nontoggleable == true)
                    {
                        buttonInfo.method.Invoke();
                        Mods.DisableButton(buttonInfo.buttonText);
                    }
                    if (buttonInfo.enabled == false && buttonInfo.disableMethod != null)
                    {
                        buttonInfo.disableMethod.Invoke();
                    }
                }
                foreach (ButtonInfo buttonInfo in CatButtons9)
                {
                    if (buttonInfo.method == null) continue;

                    if (buttonInfo.enabled == true && buttonInfo.nontoggleable == false)
                    {
                        buttonInfo.method.Invoke();
                    }
                    if (buttonInfo.enabled == true && buttonInfo.nontoggleable == true)
                    {
                        buttonInfo.method.Invoke();
                        Mods.DisableButton(buttonInfo.buttonText);
                    }
                    if (buttonInfo.enabled == false && buttonInfo.disableMethod != null)
                    {
                        buttonInfo.disableMethod.Invoke();
                    }
                }
                foreach (ButtonInfo buttonInfo in CatButtons2)
                {
                    if (buttonInfo.method == null) continue;

                    if (buttonInfo.enabled == true && buttonInfo.nontoggleable == false)
                    {
                        buttonInfo.method.Invoke();
                    }
                    if (buttonInfo.enabled == true && buttonInfo.nontoggleable == true)
                    {
                        buttonInfo.method.Invoke();
                        Mods.DisableButton(buttonInfo.buttonText);
                    }
                    if (buttonInfo.enabled == false && buttonInfo.disableMethod != null)
                    {
                        buttonInfo.disableMethod.Invoke();
                    }
                }
                foreach (ButtonInfo buttonInfo in CatButtons3)
                {
                    if (buttonInfo.method == null) continue;

                    if (buttonInfo.enabled == true && buttonInfo.nontoggleable == false)
                    {
                        buttonInfo.method.Invoke();
                    }
                    if (buttonInfo.enabled == true && buttonInfo.nontoggleable == true)
                    {
                        buttonInfo.method.Invoke();
                        Mods.DisableButton(buttonInfo.buttonText);
                    }
                    if (buttonInfo.enabled == false && buttonInfo.disableMethod != null)
                    {
                        buttonInfo.disableMethod.Invoke();
                    }
                }
                foreach (ButtonInfo buttonInfo in CatButtons4)
                {
                    if (buttonInfo.method == null) continue;

                    if (buttonInfo.enabled == true && buttonInfo.nontoggleable == false)
                    {
                        buttonInfo.method.Invoke();
                    }
                    if (buttonInfo.enabled == true && buttonInfo.nontoggleable == true)
                    {
                        buttonInfo.method.Invoke();
                        Mods.DisableButton(buttonInfo.buttonText);
                    }
                    if (buttonInfo.enabled == false && buttonInfo.disableMethod != null)
                    {
                        buttonInfo.disableMethod.Invoke();
                    }
                }
                foreach (ButtonInfo buttonInfo in CatButtons5)
                {
                    if (buttonInfo.method == null) continue;

                    if (buttonInfo.enabled == true && buttonInfo.nontoggleable == false)
                    {
                        buttonInfo.method.Invoke();
                    }
                    if (buttonInfo.enabled == true && buttonInfo.nontoggleable == true)
                    {
                        buttonInfo.method.Invoke();
                        Mods.DisableButton(buttonInfo.buttonText);
                    }
                    if (buttonInfo.enabled == false && buttonInfo.disableMethod != null)
                    {
                        buttonInfo.disableMethod.Invoke();
                    }
                }
                foreach (ButtonInfo buttonInfo in CatButtons6)
                {
                    if (buttonInfo.method == null) continue;

                    if (buttonInfo.enabled == true && buttonInfo.nontoggleable == false)
                    {
                        buttonInfo.method.Invoke();
                    }
                    if (buttonInfo.enabled == true && buttonInfo.nontoggleable == true)
                    {
                        buttonInfo.method.Invoke();
                        Mods.DisableButton(buttonInfo.buttonText);
                    }
                    if (buttonInfo.enabled == false && buttonInfo.disableMethod != null)
                    {
                        buttonInfo.disableMethod.Invoke();
                    }
                }
                foreach (ButtonInfo buttonInfo in CatButtons7)
                {
                    if (buttonInfo.method == null) continue;

                    if (buttonInfo.enabled == true && buttonInfo.nontoggleable == false)
                    {
                        buttonInfo.method.Invoke();
                    }
                    if (buttonInfo.enabled == true && buttonInfo.nontoggleable == true)
                    {
                        buttonInfo.method.Invoke();
                        Mods.DisableButton(buttonInfo.buttonText);
                    }
                    if (buttonInfo.enabled == false && buttonInfo.disableMethod != null)
                    {
                        buttonInfo.disableMethod.Invoke();
                    }
                }
                foreach (ButtonInfo buttonInfo in CatButtons8)
                {
                    if (buttonInfo.method == null) continue;

                    if (buttonInfo.enabled == true && buttonInfo.nontoggleable == false)
                    {
                        buttonInfo.method.Invoke();
                    }
                    if (buttonInfo.enabled == true && buttonInfo.nontoggleable == true)
                    {
                        buttonInfo.method.Invoke();
                        Mods.DisableButton(buttonInfo.buttonText);
                    }
                    if (buttonInfo.enabled == false && buttonInfo.disableMethod != null)
                    {
                        buttonInfo.disableMethod.Invoke();
                    }
                }
                foreach (ButtonInfo buttonInfo in CatButtons9)
                {
                    if (buttonInfo.method == null) continue;

                    if (buttonInfo.enabled == true && buttonInfo.nontoggleable == false)
                    {
                        buttonInfo.method.Invoke();
                    }
                    if (buttonInfo.enabled == true && buttonInfo.nontoggleable == true)
                    {
                        buttonInfo.method.Invoke();
                        Mods.DisableButton(buttonInfo.buttonText);
                    }
                    if (buttonInfo.enabled == false && buttonInfo.disableMethod != null)
                    {
                        buttonInfo.disableMethod.Invoke();
                    }
                }
                foreach (ButtonInfo buttonInfo in CatButtons10)
                {
                    if (buttonInfo.method == null) continue;

                    if (buttonInfo.enabled == true && buttonInfo.nontoggleable == false)
                    {
                        buttonInfo.method.Invoke();
                    }
                    if (buttonInfo.enabled == true && buttonInfo.nontoggleable == true)
                    {
                        buttonInfo.method.Invoke();
                        Mods.DisableButton(buttonInfo.buttonText);
                    }
                    if (buttonInfo.enabled == false && buttonInfo.disableMethod != null)
                    {
                        buttonInfo.disableMethod.Invoke();
                    }
                }
                if (!Directory.Exists(FolderName))
                {
                    Directory.CreateDirectory(FolderName);
                }
                if (custom)
                {
                    GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/motdHeadingText").GetComponent<TextMeshPro>().text = CustomBoardTexts[0];
                    GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/CodeOfConductHeadingText").GetComponent<TextMeshPro>().text = CustomBoardTexts[1];
                    GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/COCBodyText_TitleData").GetComponent<TextMeshPro>().text = CustomBoardTexts[2];
                    if (PhotonNetwork.IsConnectedAndReady)
                    {
                        GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/motdBodyText").GetComponent<TextMeshPro>().text = CustomBoardTexts[3];
                        custom = false;
                    }
                }
            }
            catch { }
        }
        public static bool custom = true;
        private static string GetButtonTooltip(int index)
        {
            if (Mods.inSettings) // makes tooltips for mods in settings
            {
                ButtonInfo buttonInfo = settingsbuttons[index];
                return $"{buttonInfo.buttonText}: {buttonInfo.toolTip}";
            }
            else
            {
                if (Mods.inCat1) // makes tooltips for mods in category 1
                {
                    ButtonInfo buttonInfoo = CatButtons1[index];
                    return $"{buttonInfoo.buttonText}: {buttonInfoo.toolTip}";
                }
                if (Mods.inCat2) // makes tooltips for mods in category 2
                {
                    ButtonInfo buttonInfoo = CatButtons2[index];
                    return $"{buttonInfoo.buttonText}: {buttonInfoo.toolTip}";
                }
                if (Mods.inCat3) // makes tooltips for mods in category 3
                {
                    ButtonInfo buttonInfoo = CatButtons3[index];
                    return $"{buttonInfoo.buttonText}: {buttonInfoo.toolTip}";
                }
                if (Mods.inCat4) // makes tooltips for mods in category 4
                {
                    ButtonInfo buttonInfoo = CatButtons4[index];
                    return $"{buttonInfoo.buttonText}: {buttonInfoo.toolTip}";
                }
                if (Mods.inCat5) // makes tooltips for mods in category 5
                {
                    ButtonInfo buttonInfoo = CatButtons5[index];
                    return $"{buttonInfoo.buttonText}: {buttonInfoo.toolTip}";
                }
                if (Mods.inCat6) // makes tooltips for mods in category 6
                {
                    ButtonInfo buttonInfoo = CatButtons6[index];
                    return $"{buttonInfoo.buttonText}: {buttonInfoo.toolTip}";
                }
                if (Mods.inCat7) // makes tooltips for mods in category 7
                {
                    ButtonInfo buttonInfoo = CatButtons7[index];
                    return $"{buttonInfoo.buttonText}: {buttonInfoo.toolTip}";
                }
                if (Mods.inCat8) // makes tooltips for mods in category 8
                {
                    ButtonInfo buttonInfoo = CatButtons8[index];
                    return $"{buttonInfoo.buttonText}: {buttonInfoo.toolTip}";
                }
                if (Mods.inCat9) // makes tooltips for mods in category 9
                {
                    ButtonInfo buttonInfoo = CatButtons9[index];
                    return $"{buttonInfoo.buttonText}: {buttonInfoo.toolTip}";
                }
                if (Mods.inCat10) // makes tooltips for mods in category 10
                {
                    ButtonInfo buttonInfoo = CatButtons10[index];
                    return $"{buttonInfoo.buttonText}: {buttonInfoo.toolTip}";
                }
                // makes tooltips for mods in main
                ButtonInfo buttonInfo = buttons[index];
                return $"{buttonInfo.buttonText}: {buttonInfo.toolTip}";
            }
        }
        public void Draw()
        { 
            menu = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Destroy(menu.GetComponent<Rigidbody>());
            Destroy(menu.GetComponent<BoxCollider>());
            Destroy(menu.GetComponent<Renderer>());
            menu.transform.localScale = new Vector3(0.1f, 0.3f, 0.4f) * GorillaLocomotion.GTPlayer.Instance.scale;
            menuObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Destroy(menuObj.GetComponent<Rigidbody>());
            Destroy(menuObj.GetComponent<BoxCollider>());
            menuObj.transform.parent = menu.transform;
            menuObj.transform.rotation = Quaternion.identity;
            menuObj.transform.localScale = MenuScale;
            if (ChangingColors)
            {
                if (Mods.RGBMenu)
                {
                    GradientColorKey[] array13 = new GradientColorKey[7];
                    array13[0].color = Color.red;
                    array13[0].time = 0f;
                    array13[1].color = Color.yellow;
                    array13[1].time = 0.2f;
                    array13[2].color = Color.green;
                    array13[2].time = 0.3f;
                    array13[3].color = Color.cyan;
                    array13[3].time = 0.5f;
                    array13[4].color = Color.blue;
                    array13[4].time = 0.6f;
                    array13[5].color = Color.magenta;
                    array13[5].time = 0.8f;
                    array13[6].color = Color.red;
                    array13[6].time = 1f;
                    ColorChanger colorChanger2 = menuObj.AddComponent<ColorChanger>();
                    colorChanger2.colors = new Gradient
                    {
                        colorKeys = array13
                    };
                    colorChanger2.Start();
                }
                else
                {
                    GradientColorKey[] array = new GradientColorKey[4];
                    array[0].color = FirstColor;
                    array[0].time = 0f;
                    array[1].color = FirstColor;
                    array[1].time = 0.3f;
                    array[2].color = SecondColor;
                    array[2].time = 0.6f;
                    array[3].color = FirstColor;
                    array[3].time = 1f;
                    ColorChanger colorChanger = menuObj.AddComponent<ColorChanger>();
                    colorChanger.colors = new Gradient
                    {
                        colorKeys = array
                    };
                    colorChanger.Start();
                }
            }
            else
            {
                menuObj.GetComponent<Renderer>().material.color = NormalColor;
            }
            if (Mods.change10 == 10)
            {
                // clear menu
                Destroy(menuObj.GetComponent<Renderer>());
                Destroy(menuObj.GetComponent<ColorChanger>());
            }
            menuObj.transform.position = MenuPos;
            canvasObj = new GameObject();
            canvasObj.transform.parent = menu.transform;
            Canvas canvas = canvasObj.AddComponent<Canvas>();
            CanvasScaler canvasScaler = canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvasScaler.dynamicPixelsPerUnit = 1000f;
            Text text = new GameObject
            {
                transform =
                {
                    parent = canvasObj.transform
                }
            }.AddComponent<Text>();
            text.gameObject.name = "name";
            titiel = text;
            text.font = MenuFont;
            int yau = pageNumber + 1;
            text.text = MenuTitle;
            text.fontSize = 1;
            text.alignment = TextAnchor.MiddleCenter;
            text.color = MenuTitleColor;
            if (FirstColor == Color.white && SecondColor == Color.white)
            {
                // if menu is white, the menu title is black
                text.color = Color.black;
            }
            text.resizeTextForBestFit = true;
            text.resizeTextMinSize = 0;
            RectTransform component = text.GetComponent<RectTransform>();
            component.localPosition = Vector3.zero;
            component.sizeDelta = MenuTitleScale;
            component.position = MenuTitlePos;
            component.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));
            AddPageButtons();
            string[] array2 = buttons.Skip(pageNumber * pageSize).Take(pageSize).Select(button => button.buttonText).ToArray();
            string[] array3 = settingsbuttons.Skip(pageNumber * pageSize).Take(pageSize).Select(button => button.buttonText).ToArray();
            string[] array4 = CatButtons1.Skip(pageNumber * pageSize).Take(pageSize).Select(button => button.buttonText).ToArray();
            string[] array5 = CatButtons2.Skip(pageNumber * pageSize).Take(pageSize).Select(button => button.buttonText).ToArray();
            string[] array6 = CatButtons9.Skip(pageNumber * pageSize).Take(pageSize).Select(button => button.buttonText).ToArray();
            string[] array7 = CatButtons3.Skip(pageNumber * pageSize).Take(pageSize).Select(button => button.buttonText).ToArray();
            string[] array8 = CatButtons4.Skip(pageNumber * pageSize).Take(pageSize).Select(button => button.buttonText).ToArray();
            string[] array9 = CatButtons5.Skip(pageNumber * pageSize).Take(pageSize).Select(button => button.buttonText).ToArray();
            string[] array10 = CatButtons6.Skip(pageNumber * pageSize).Take(pageSize).Select(button => button.buttonText).ToArray();
            string[] array11 = CatButtons7.Skip(pageNumber * pageSize).Take(pageSize).Select(button => button.buttonText).ToArray();
            string[] array12 = CatButtons8.Skip(pageNumber * pageSize).Take(pageSize).Select(button => button.buttonText).ToArray();
            string[] array15 = CatButtons10.Skip(pageNumber * pageSize).Take(pageSize).Select(button => button.buttonText).ToArray();
            if (Mods.inSettings)
            {
                for (int i = 0; i < array3.Length; i++)
                {
                    AddButton(i * 0.13f + 0.26f * 1f, array3[i]);
                }
            }
            else
            {
                if (Mods.inCat1)
                {
                    for (int i = 0; i < array4.Length; i++)
                    {
                        AddButton(i * 0.13f + 0.26f * 1f, array4[i]);
                    }
                }
                if (Mods.inCat2)
                {
                    for (int i = 0; i < array5.Length; i++)
                    {
                        AddButton(i * 0.13f + 0.26f * 1f, array5[i]);
                    }
                }
                if (Mods.inCat9)
                {
                    for (int i = 0; i < array6.Length; i++)
                    {
                        AddButton(i * 0.13f + 0.26f * 1f, array6[i]);
                    }
                }
                if (Mods.inCat3)
                {
                    for (int i = 0; i < array7.Length; i++)
                    {
                        AddButton(i * 0.13f + 0.26f * 1f, array7[i]);
                    }
                }
                if (Mods.inCat4)
                {
                    for (int i = 0; i < array8.Length; i++)
                    {
                        AddButton(i * 0.13f + 0.26f * 1f, array8[i]);
                    }
                }
                if (Mods.inCat5)
                {
                    for (int i = 0; i < array9.Length; i++)
                    {
                        AddButton(i * 0.13f + 0.26f * 1f, array9[i]);
                    }
                }
                if (Mods.inCat6)
                {
                    for (int i = 0; i < array10.Length; i++)
                    {
                        AddButton(i * 0.13f + 0.26f * 1f, array10[i]);
                    }
                }
                if (Mods.inCat7)
                {
                    for (int i = 0; i < array11.Length; i++)
                    {
                        AddButton(i * 0.13f + 0.26f * 1f, array11[i]);
                    }
                }
                if (Mods.inCat8)
                {
                    for (int i = 0; i < array12.Length; i++)
                    {
                        AddButton(i * 0.13f + 0.26f * 1f, array12[i]);
                    }
                }
                if (Mods.inCat10)
                {
                    for (int i = 0; i < array15.Length; i++)
                    {
                        AddButton(i * 0.13f + 0.26f * 1f, array15[i]);
                    }
                }
                if (!Mods.inCat1 && !Mods.inCat2 && !Mods.inCat3 && !Mods.inCat4 && !Mods.inCat10&& !Mods.inCat5 && !Mods.inCat6 && !Mods.inCat7 && !Mods.inCat9 && !Mods.inCat8 && !Mods.inSettings)
                {
                    for (int i = 0; i < array2.Length; i++)
                    {
                        AddButton(i * 0.13f + 0.26f * 1f, array2[i]);
                    }
                }
            }
            GameObject tooltipObj = new GameObject();
            tooltipObj.transform.SetParent(canvasObj.transform);
            tooltipObj.transform.localPosition = new Vector3(0, 0, 1) * 1f;
            tooltipText = tooltipObj.GetComponent<Text>();
            if (tooltipText == null)
                tooltipText = tooltipObj.AddComponent<Text>();
            tooltipText.font = MenuFont;
            tooltipText.text = tooltipString;
            tooltipText.fontSize = 20;
            tooltipText.alignment = TextAnchor.MiddleCenter;
            tooltipText.resizeTextForBestFit = true;
            tooltipText.resizeTextMinSize = 0;
            tooltipText.color = ToolTipColor;
            if (FirstColor == Color.white && SecondColor == Color.white)
            {
                // if menu is white, the tooltip is black
                tooltipText.color = Color.black;
            }
            RectTransform componenttooltip = tooltipObj.GetComponent<RectTransform>();
            componenttooltip.localPosition = Vector3.zero;
            componenttooltip.sizeDelta = ToolTipScale;
            componenttooltip.position = ToolTipPos;
            componenttooltip.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));
        }
        public static Text titiel;
        private static void AddPageButtons()
        {
            int num = (buttons.Count + pageSize - 1) / pageSize;
            int num2 = pageNumber + 1;
            int num3 = pageNumber - 1;
            if (num2 > num - 1)
            {
                num2 = 0;
            }
            if (num3 < 0)
            {
                num3 = num - 1;
            }
            if (Mods.change7 == 1)
            {
                // on menu next & prev
                float num4 = 0f;
                GameObject gameObject = GameObject.CreatePrimitive((PrimitiveType)3);
                Destroy(gameObject.GetComponent<Rigidbody>());
                gameObject.GetComponent<BoxCollider>().isTrigger = true;
                gameObject.transform.parent = menu.transform;
                gameObject.transform.rotation = Quaternion.identity;
                gameObject.transform.localScale = new Vector3(0.09f, 0.8f, 0.08f);
                gameObject.transform.localPosition = new Vector3(0.56f, 0f, 0.28f - num4);
                gameObject.AddComponent<BtnCollider>().relatedText = "PreviousPage";
                gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.grey);
                GradientColorKey[] array = new GradientColorKey[3];
                array[0].color = new Color32(50, 50, 50, 255);
                array[0].time = 0f;
                array[1].color = new Color32(90, 90, 90, 255);
                array[1].time = 0.5f;
                array[2].color = new Color32(50, 50, 50, 255);
                array[2].time = 1f;
                ColorChanger colorChanger = gameObject.AddComponent<ColorChanger>();
                colorChanger.colors = new Gradient
                {
                    colorKeys = array
                };
                colorChanger.Start();
                Text text = new GameObject
                {
                    transform =
                    {
                       parent = canvasObj.transform
                    }
                }.AddComponent<Text>();
                text.font = MenuFont;
                text.text = "[" + num3.ToString() + "] << Prev";
                text.fontSize = 1;
                text.alignment = TextAnchor.MiddleCenter;
                text.resizeTextForBestFit = true;
                text.resizeTextMinSize = 0;
                RectTransform component = text.GetComponent<RectTransform>();
                component.localPosition = Vector3.zero;
                component.sizeDelta = new Vector2(0.2f, 0.03f);
                component.localPosition = new Vector3(0.064f, 0f, 0.111f - num4 / 2.55f);
                component.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));
                num4 = 0.13f;
                GameObject gameObject2 = GameObject.CreatePrimitive((PrimitiveType)3);
                Destroy(gameObject2.GetComponent<Rigidbody>());
                gameObject2.GetComponent<BoxCollider>().isTrigger = true;
                gameObject2.transform.parent = menu.transform;
                gameObject2.transform.rotation = Quaternion.identity;
                gameObject2.transform.localScale = new Vector3(0.09f, 0.8f, 0.08f);
                gameObject2.transform.localPosition = new Vector3(0.56f, 0f, 0.28f - num4);
                gameObject2.AddComponent<BtnCollider>().relatedText = "NextPage";
                gameObject2.GetComponent<Renderer>().material.SetColor("_Color", Color.grey);
                GradientColorKey[] array2 = new GradientColorKey[3];
                array2[0].color = new Color32(50, 50, 50, 255);
                array2[0].time = 0f;
                array2[1].color = new Color32(90, 90, 90, 255);
                array2[1].time = 0.5f;
                array2[2].color = new Color32(50, 50, 50, 255);
                array2[2].time = 1f;
                ColorChanger colorChanger2 = gameObject2.AddComponent<ColorChanger>();
                colorChanger2.colors = new Gradient
                {
                    colorKeys = array2
                };
                colorChanger2.Start();
                Text text2 = new GameObject
                {
                    transform =
                    {
                       parent = canvasObj.transform
                    }
                }.AddComponent<Text>();
                text2.font = MenuFont;
                text2.text = "Next >> [" + num2.ToString() + "]";
                text2.fontSize = 1;
                text2.alignment = TextAnchor.MiddleCenter;
                text2.resizeTextForBestFit = true;
                text2.resizeTextMinSize = 0;
                RectTransform component2 = text2.GetComponent<RectTransform>();
                component2.localPosition = Vector3.zero;
                component2.sizeDelta = new Vector2(0.2f, 0.03f);
                component2.localPosition = new Vector3(0.064f, 0f, 0.111f - num4 / 2.55f);
                component2.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));
                pageSize = 4;
            }
            if (Mods.change7 == 2)
            {
                // top next & prev
                GameObject gameObject7 = GameObject.CreatePrimitive(PrimitiveType.Cube);
                gameObject7.name = "prev";
                Destroy(gameObject7.GetComponent<Rigidbody>());
                gameObject7.GetComponent<BoxCollider>().isTrigger = true;
                gameObject7.transform.parent = menu.transform;
                gameObject7.transform.rotation = Quaternion.identity;
                gameObject7.transform.localScale = new Vector3(0.045f, 0.25f, 0.064295f);
                gameObject7.transform.localPosition = new Vector3(0.56f, 0.37f, 0.541f);
                gameObject7.AddComponent<BtnCollider>().relatedText = "PreviousPage";
                gameObject7.GetComponent<Renderer>().material.color = NextPrevButtonColor;
                GameObject gameObject8 = GameObject.CreatePrimitive(PrimitiveType.Cube);
                Text text60 = new GameObject
                {
                    transform =
                    {
                       parent = canvasObj.transform
                    }
                }.AddComponent<Text>();
                text60.font = MenuFont;
                text60.text = "<";
                text60.fontSize = 1;
                text60.alignment = TextAnchor.MiddleCenter;
                text60.resizeTextForBestFit = true;
                text60.resizeTextMinSize = 0;
                text60.color = NextPrevTextColor;
                RectTransform component21 = text60.GetComponent<RectTransform>();
                component21.localPosition = Vector3.zero;
                component21.sizeDelta = new Vector2(0.2f, 0.03f);
                component21.localPosition = new Vector3(0.064f, 0.11f, 0.215f);
                component21.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));
                gameObject8.name = "next";
                Destroy(gameObject8.GetComponent<Rigidbody>());
                gameObject8.GetComponent<BoxCollider>().isTrigger = true;
                gameObject8.transform.parent = menu.transform;
                gameObject8.transform.rotation = Quaternion.identity;
                gameObject8.transform.localScale = new Vector3(0.045f, 0.25f, 0.064295f);
                gameObject8.transform.localPosition = new Vector3(0.56f, -0.37f, 0.541f);
                gameObject8.AddComponent<BtnCollider>().relatedText = "NextPage";
                gameObject8.GetComponent<Renderer>().material.color = NextPrevButtonColor;
                Text text61 = new GameObject
                {
                    transform =
                    {
                       parent = canvasObj.transform
                    }
                }.AddComponent<Text>();
                text61.font = MenuFont;
                text61.text = ">";
                text61.fontSize = 1;
                text61.alignment = TextAnchor.MiddleCenter;
                text61.resizeTextForBestFit = true;
                text61.resizeTextMinSize = 0;
                text61.color = NextPrevTextColor;
                RectTransform component22 = text61.GetComponent<RectTransform>();
                component22.localPosition = Vector3.zero;
                component22.sizeDelta = new Vector2(0.2f, 0.03f);
                component22.localPosition = new Vector3(0.064f, -0.11f, 0.215f);
                component22.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));
                pageSize = 6;
            }
            if (Mods.change7 == 3)
            {
                // side next & prev
                GameObject gameObject20 = GameObject.CreatePrimitive(PrimitiveType.Cube);
                gameObject20.name = "prev";
                Destroy(gameObject20.GetComponent<Rigidbody>());
                gameObject20.GetComponent<BoxCollider>().isTrigger = true;
                gameObject20.transform.parent = menu.transform;
                gameObject20.transform.rotation = Quaternion.identity;
                gameObject20.transform.localScale = new Vector3(0.045f, 0.25f, 0.8936298f);
                gameObject20.transform.localPosition = new Vector3(0.56f, 0.657f, 0.0063f);
                gameObject20.AddComponent<BtnCollider>().relatedText = "PreviousPage";
                gameObject20.GetComponent<Renderer>().material.color = NextPrevButtonColor;
                Text text65 = new GameObject
                {
                    transform =
                    {
                       parent = canvasObj.transform
                    }
                }.AddComponent<Text>();
                text65.font = MenuFont;
                text65.text = "<";
                text65.fontSize = 1;
                text65.alignment = TextAnchor.MiddleCenter;
                text65.resizeTextForBestFit = true;
                text65.resizeTextMinSize = 0;
                text65.color = NextPrevTextColor;
                RectTransform component24 = text65.GetComponent<RectTransform>();
                component24.localPosition = Vector3.zero;
                component24.sizeDelta = new Vector2(0.2f, 0.03f);
                component24.localPosition = new Vector3(0.064f, 0.20f, 0.0063f);
                component24.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));
                GameObject gameObject21 = GameObject.CreatePrimitive(PrimitiveType.Cube);
                gameObject21.name = "next";
                Destroy(gameObject21.GetComponent<Rigidbody>());
                gameObject21.GetComponent<BoxCollider>().isTrigger = true;
                gameObject21.transform.parent = menu.transform;
                gameObject21.transform.rotation = Quaternion.identity;
                gameObject21.transform.localScale = new Vector3(0.045f, 0.25f, 0.8936298f);
                gameObject21.transform.localPosition = new Vector3(0.56f, -0.657f, 0.0063f);
                gameObject21.AddComponent<BtnCollider>().relatedText = "NextPage";
                gameObject21.GetComponent<Renderer>().material.color = NextPrevButtonColor;
                Text text67 = new GameObject
                {
                    transform =
                    {
                       parent = canvasObj.transform
                    }
                }.AddComponent<Text>();
                text67.font = MenuFont;
                text67.text = ">";
                text67.fontSize = 1;
                text67.alignment = TextAnchor.MiddleCenter;
                text67.resizeTextForBestFit = true;
                text67.resizeTextMinSize = 0;
                text67.color = NextPrevTextColor;
                RectTransform component28 = text67.GetComponent<RectTransform>();
                component28.localPosition = Vector3.zero;
                component28.sizeDelta = new Vector2(0.2f, 0.03f);
                component28.localPosition = new Vector3(0.064f, -0.20f, 0.0063f);
                component28.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));
                pageSize = 6;
            }
            if (Mods.change7 == 4)
            {
                // bottom next & prev
                GameObject gameObject63 = GameObject.CreatePrimitive(PrimitiveType.Cube);
                gameObject63.name = "prev";
                Destroy(gameObject63.GetComponent<Rigidbody>());
                gameObject63.GetComponent<BoxCollider>().isTrigger = true;
                gameObject63.transform.parent = menu.transform;
                gameObject63.transform.rotation = Quaternion.identity;
                gameObject63.transform.localScale = new Vector3(0.045f, 0.25f, 0.064295f);
                gameObject63.transform.localPosition = new Vector3(0.56f, 0.37f, -0.541f);
                gameObject63.AddComponent<BtnCollider>().relatedText = "PreviousPage";
                gameObject63.GetComponent<Renderer>().material.color = NextPrevButtonColor;
                GameObject gameObject80 = GameObject.CreatePrimitive(PrimitiveType.Cube);
                Text text62 = new GameObject
                {
                    transform =
                    {
                       parent = canvasObj.transform
                    }
                }.AddComponent<Text>();
                text62.font = MenuFont;
                text62.text = "<";
                text62.fontSize = 1;
                text62.alignment = TextAnchor.MiddleCenter;
                text62.resizeTextForBestFit = true;
                text62.resizeTextMinSize = 0;
                text62.color = NextPrevTextColor;
                RectTransform component22 = text62.GetComponent<RectTransform>();
                component22.localPosition = Vector3.zero;
                component22.sizeDelta = new Vector2(0.2f, 0.03f);
                component22.localPosition = new Vector3(0.064f, 0.11f, -0.215f);
                component22.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));
                gameObject80.name = "next";
                Destroy(gameObject80.GetComponent<Rigidbody>());
                gameObject80.GetComponent<BoxCollider>().isTrigger = true;
                gameObject80.transform.parent = menu.transform;
                gameObject80.transform.rotation = Quaternion.identity;
                gameObject80.transform.localScale = new Vector3(0.045f, 0.25f, 0.064295f);
                gameObject80.transform.localPosition = new Vector3(0.56f, -0.37f, -0.541f);
                gameObject80.AddComponent<BtnCollider>().relatedText = "NextPage";
                gameObject80.GetComponent<Renderer>().material.color = NextPrevButtonColor;
                Text text63 = new GameObject
                {
                    transform =
                    {
                       parent = canvasObj.transform
                    }
                }.AddComponent<Text>();
                text63.font = MenuFont;
                text63.text = ">";
                text63.fontSize = 1;
                text63.alignment = TextAnchor.MiddleCenter;
                text63.resizeTextForBestFit = true;
                text63.resizeTextMinSize = 0;
                text63.color = NextPrevTextColor;
                RectTransform component23 = text63.GetComponent<RectTransform>();
                component23.localPosition = Vector3.zero;
                component23.sizeDelta = new Vector2(0.2f, 0.03f);
                component23.localPosition = new Vector3(0.064f, -0.11f, -0.215f);
                component23.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));
                pageSize = 6;
            }
            if (Mods.change7 == 5)
            {
                // trigger next & prev
                pageSize = 6;
            }
            if (Mods.change4 == 1)
            {
                // right disconnect
                float num7 = 0.26f;
                GameObject gameObject3 = GameObject.CreatePrimitive(PrimitiveType.Cube);
                gameObject3.name = "disconnect";
                Destroy(gameObject3.GetComponent<Rigidbody>());
                gameObject3.GetComponent<BoxCollider>().isTrigger = true;
                gameObject3.transform.parent = menu.transform;
                gameObject3.transform.rotation = Quaternion.identity;
                gameObject3.transform.localScale = new Vector3(0.045f, 0.55f, 0.16f);
                gameObject3.transform.localPosition = new Vector3(0.56f, -0.8f, 0.35f - num7);
                gameObject3.AddComponent<BtnCollider>().relatedText = "DisconnectingButton";
                gameObject3.GetComponent<Renderer>().material.color = DisconnectButtonColor;
                Text text5 = new GameObject
                {
                    name = "disconnect text",
                    transform =
                    {
                        parent = canvasObj.transform
                    }
                }.AddComponent<Text>();
                text5.font = MenuFont;
                text5.text = "Disconnect";
                text5.fontSize = 1;
                text5.alignment = TextAnchor.MiddleCenter;
                text5.resizeTextForBestFit = true;
                text5.resizeTextMinSize = 0;
                text5.color = DisconnectTextColor;
                RectTransform component5 = text5.GetComponent<RectTransform>();
                component5.localPosition = Vector3.zero;
                component5.sizeDelta = new Vector2(0.2f, 0.03f);
                component5.localPosition = new Vector3(0.06f, -0.24f, 0.14f - num7 / 2.55f);
                component5.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));
                if (Mods.change7 == 3)
                {
                    // if side Next & Prev, move the right disconnect a lil bit to the right
                    gameObject3.transform.localPosition = new Vector3(0.56f, -1.1f, 0.35f - num7);
                    component5.localPosition = new Vector3(0.06f, -0.33f, 0.14f - num7 / 2.55f);
                }
            }
            if (Mods.change4 == 2)
            {
                // left disconnect
                float num63 = 0.26f;
                GameObject gameObject4 = GameObject.CreatePrimitive(PrimitiveType.Cube);
                gameObject4.name = "disconnect";
                Destroy(gameObject4.GetComponent<Rigidbody>());
                gameObject4.GetComponent<BoxCollider>().isTrigger = true;
                gameObject4.transform.parent = menu.transform;
                gameObject4.transform.rotation = Quaternion.identity;
                gameObject4.transform.localScale = new Vector3(0.045f, 0.55f, 0.16f);
                gameObject4.transform.localPosition = new Vector3(0.56f, 0.8f, 0.35f - num63);
                gameObject4.AddComponent<BtnCollider>().relatedText = "DisconnectingButton";
                gameObject4.GetComponent<Renderer>().material.color = DisconnectButtonColor;
                Text text6 = new GameObject
                {
                    name = "disconnect text",
                    transform =
                    {
                        parent = canvasObj.transform
                    }
                }.AddComponent<Text>();
                text6.font = MenuFont;
                text6.text = "Disconnect";
                text6.fontSize = 1;
                text6.alignment = TextAnchor.MiddleCenter;
                text6.resizeTextForBestFit = true;
                text6.resizeTextMinSize = 0;
                text6.color = DisconnectTextColor;
                RectTransform component6 = text6.GetComponent<RectTransform>();
                component6.localPosition = Vector3.zero;
                component6.sizeDelta = new Vector2(0.2f, 0.03f);
                component6.localPosition = new Vector3(0.06f, 0.24f, 0.14f - num63 / 2.55f);
                component6.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));
                if (Mods.change7 == 3)
                {
                    // if side Next & Prev, move the left disconnect a lil bit to the left
                    gameObject4.transform.localPosition = new Vector3(0.56f, 1.1f, 0.35f - num63);
                    component6.localPosition = new Vector3(0.06f, 0.33f, 0.14f - num63 / 2.55f);
                }
            }
            if (Mods.change4 == 3)
            {
                // top disconnect
                GameObject gameObject69 = GameObject.CreatePrimitive(PrimitiveType.Cube);
                gameObject69.name = "disconnect";
                gameObject69.GetComponent<BoxCollider>().isTrigger = true;
                gameObject69.transform.parent = menu.transform;
                gameObject69.transform.rotation = Quaternion.identity;
                gameObject69.transform.localScale = new Vector3(0.12f, 0.9f, 0.1f);
                gameObject69.transform.localPosition = new Vector3(0.56f, 0f, 0.6f);
                gameObject69.AddComponent<BtnCollider>().relatedText = "DisconnectingButton";
                gameObject69.GetComponent<Renderer>().material.color = DisconnectButtonColor;
                Text text30 = new GameObject
                {
                    name = "disconnect text",
                    transform =
                    {
                      parent = canvasObj.transform
                    }
                }.AddComponent<Text>();
                text30.font = MenuFont;
                text30.text = "Disconnect";
                text30.fontSize = 1;
                text30.color = Color.white;
                text30.alignment = TextAnchor.MiddleCenter;
                text30.resizeTextForBestFit = true;
                text30.resizeTextMinSize = 0;
                text30.color = DisconnectTextColor;
                RectTransform compenment10 = text30.GetComponent<RectTransform>();
                compenment10.localPosition = Vector3.zero;
                compenment10.sizeDelta = new Vector2(0.2f, 0.03f);
                compenment10.localPosition = new Vector3(0.064f, 0f, 0.24f);
                compenment10.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));
                if (Mods.change7 == 2)
                {
                    // if top Next & Prev, move the top disconnect a lil bit up
                    gameObject69.transform.localPosition = new Vector3(0.56f, 0f, 0.7f);
                    compenment10.localPosition = new Vector3(0.064f, 0f, 0.28f);
                }
            }
            if (Mods.change4 == 4)
            {
                // bottom disconnect
                GameObject gameObject9 = GameObject.CreatePrimitive(PrimitiveType.Cube);
                gameObject9.name = "disconnect";
                gameObject9.GetComponent<BoxCollider>().isTrigger = true;
                gameObject9.transform.parent = menu.transform;
                gameObject9.transform.rotation = Quaternion.identity;
                gameObject9.transform.localScale = new Vector3(0.12f, 0.9f, 0.1f);
                gameObject9.transform.localPosition = new Vector3(0.56f, 0f, -0.6f);
                gameObject9.AddComponent<BtnCollider>().relatedText = "DisconnectingButton";
                gameObject9.GetComponent<Renderer>().material.color = DisconnectButtonColor;
                Text text12 = new GameObject
                {
                    name = "disconnect text",
                    transform =
                    {
                      parent = canvasObj.transform
                    }
                }.AddComponent<Text>();
                text12.font = MenuFont;
                text12.text = "Disconnect";
                text12.fontSize = 1;
                text12.color = Color.white;
                text12.alignment = TextAnchor.MiddleCenter;
                text12.resizeTextForBestFit = true;
                text12.resizeTextMinSize = 0;
                text12.color = DisconnectTextColor;
                RectTransform compenment20 = text12.GetComponent<RectTransform>();
                compenment20.localPosition = Vector3.zero;
                compenment20.sizeDelta = new Vector2(0.2f, 0.03f);
                compenment20.localPosition = new Vector3(0.064f, 0f, -0.24f);
                compenment20.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));
                if (Mods.change7 == 4)
                {
                    // if bottom Next & Prev, move the bottom disconnect a lil bit down
                    gameObject9.transform.localPosition = new Vector3(0.56f, 0f, -0.7f);
                    compenment20.localPosition = new Vector3(0.064f, 0f, -0.28f);
                }
            }
        }
        public static void DestroyMenu()
        {
            Destroy(menu);
            Destroy(canvasObj);
            Destroy(reference);
            menu = null;
            menuObj = null;
            canvasObj = null;
            reference = null;
        }
        public static GameObject Button;
        public static Text text2;
        private static void AddButton(float offset, string text)
        {
            Button = GameObject.CreatePrimitive((PrimitiveType)3);
            Destroy(Button.GetComponent<Rigidbody>());
            Button.GetComponent<BoxCollider>().isTrigger = true;
            Button.transform.parent = menu.transform;
            Button.transform.rotation = Quaternion.identity;
            Button.transform.localScale = ButtonScale * GorillaLocomotion.GTPlayer.Instance.scale;
            if (Mods.change7 == 1)
            {
                Button.transform.localPosition = new Vector3(0.56f, 0f, 0.28f - offset);
            }
            if (Mods.change7 == 2 | Mods.change7 == 3 | Mods.change7 == 4 | Mods.change7 == 5)
            {
                Button.transform.localPosition = new Vector3(0.56f, 0f, 0.6f - offset);
            }
            Button.AddComponent<BtnCollider>().relatedText = text;
            int num = -1;
            if (Mods.inSettings)
            {
                for (int i = 0; i < settingsbuttons.Count; i++)
                {
                    if (text == settingsbuttons[i].buttonText)
                    {
                        num = i;
                        break;
                    }
                }
            }
            else
            {
                if (Mods.inCat1)
                {
                    for (int i = 0; i < CatButtons1.Count; i++)
                    {
                        if (text == CatButtons1[i].buttonText)
                        {
                            num = i;
                            break;
                        }
                    }
                }
                if (Mods.inCat2)
                {
                    for (int i = 0; i < CatButtons2.Count; i++)
                    {
                        if (text == CatButtons2[i].buttonText)
                        {
                            num = i;
                            break;
                        }
                    }
                }
                if (Mods.inCat9)
                {
                    for (int i = 0; i < CatButtons9.Count; i++)
                    {
                        if (text == CatButtons9[i].buttonText)
                        {
                            num = i;
                            break;
                        }
                    }
                }
                if (Mods.inCat3)
                {
                    for (int i = 0; i < CatButtons3.Count; i++)
                    {
                        if (text == CatButtons3[i].buttonText)
                        {
                            num = i;
                            break;
                        }
                    }
                }
                if (Mods.inCat4)
                {
                    for (int i = 0; i < CatButtons4.Count; i++)
                    {
                        if (text == CatButtons4[i].buttonText)
                        {
                            num = i;
                            break;
                        }
                    }
                }
                if (Mods.inCat5)
                {
                    for (int i = 0; i < CatButtons5.Count; i++)
                    {
                        if (text == CatButtons5[i].buttonText)
                        {
                            num = i;
                            break;
                        }
                    }
                }
                if (Mods.inCat6)
                {
                    for (int i = 0; i < CatButtons6.Count; i++)
                    {
                        if (text == CatButtons6[i].buttonText)
                        {
                            num = i;
                            break;
                        }
                    }
                }
                if (Mods.inCat7)
                {
                    for (int i = 0; i < CatButtons7.Count; i++)
                    {
                        if (text == CatButtons7[i].buttonText)
                        {
                            num = i;
                            break;
                        }
                    }
                }
                if (Mods.inCat8)
                {
                    for (int i = 0; i < CatButtons8.Count; i++)
                    {
                        if (text == CatButtons8[i].buttonText)
                        {
                            num = i;
                            break;
                        }
                    }
                }
                if (Mods.inCat10)
                {
                    for (int i = 0; i < CatButtons10.Count; i++)
                    {
                        if (text == CatButtons10[i].buttonText)
                        {
                            num = i;
                            break;
                        }
                    }
                }
                if (!Mods.inCat1 && !Mods.inCat2 && !Mods.inCat3 && !Mods.inCat10 && !Mods.inCat4 && !Mods.inCat5 && !Mods.inCat6 && !Mods.inCat7 && !Mods.inCat9 && !Mods.inCat8 && !Mods.inSettings)
                {
                    for (int i = 0; i < buttons.Count; i++)
                    {
                        if (text == buttons[i].buttonText)
                        {
                            num = i;
                            break;
                        }
                    }
                }
            }
            text2 = new GameObject
            {
                transform =
                {
                    parent = canvasObj.transform
                }
            }.AddComponent<Text>();
            text2.font = MenuFont;
            text2.text = text;
            text2.fontSize = 1;
            text2.alignment = TextAnchor.MiddleCenter;
            text2.resizeTextForBestFit = true;
            text2.resizeTextMinSize = 0;
            RectTransform component = text2.GetComponent<RectTransform>();
            component.localPosition = Vector3.zero;
            component.sizeDelta = ButtonTextScale;
            if (Mods.change7 == 1)
            {
                component.localPosition = new Vector3(0.064f, 0f, 0.111f - offset / 2.55f);
            }
            if (Mods.change7 == 2 | Mods.change7 == 3 | Mods.change7 == 4 | Mods.change7 == 5)
            {
                component.localPosition = new Vector3(0.064f, 0f, 0.237f - offset / 2.55f);
            }
            component.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));
            if (Mods.inSettings)
            {
                if (settingsbuttons[num].enabled == true)
                {
                    Button.GetComponent<Renderer>().material.color = ButtonColorEnabled;
                    text2.color = EnableTextColor;
                    if (Mods.change13 == 10)
                    {
                        Destroy(Button.GetComponent<Renderer>());
                    }
                }
                else
                {
                    Button.GetComponent<Renderer>().material.color = ButtonColorDisable;
                    text2.color = DIsableTextColor;
                    if (Mods.change12 == 10)
                    {
                        Destroy(Button.GetComponent<Renderer>());
                    }
                }
            }
            else
            {
                if (Mods.inCat1)
                {
                    if (CatButtons1[num].enabled == true)
                    {
                        Button.GetComponent<Renderer>().material.color = ButtonColorEnabled;
                        text2.color = EnableTextColor;
                        if (Mods.change13 == 10)
                        {
                            Destroy(Button.GetComponent<Renderer>());
                        }
                    }
                    else
                    {
                        Button.GetComponent<Renderer>().material.color = ButtonColorDisable;
                        text2.color = DIsableTextColor;
                        if (Mods.change12 == 10)
                        {
                            Destroy(Button.GetComponent<Renderer>());
                        }
                    }
                }
                if (Mods.inCat2)
                {
                    if (CatButtons2[num].enabled == true)
                    {
                        Button.GetComponent<Renderer>().material.color = ButtonColorEnabled;
                        text2.color = EnableTextColor;
                        if (Mods.change13 == 10)
                        {
                            Destroy(Button.GetComponent<Renderer>());
                        }
                    }
                    else
                    {
                        Button.GetComponent<Renderer>().material.color = ButtonColorDisable;
                        text2.color = DIsableTextColor;
                        if (Mods.change12 == 10)
                        {
                            Destroy(Button.GetComponent<Renderer>());
                        }
                    }
                }
                if (Mods.inCat3)
                {
                    if (CatButtons3[num].enabled == true)
                    {
                        Button.GetComponent<Renderer>().material.color = ButtonColorEnabled;
                        text2.color = EnableTextColor;
                        if (Mods.change13 == 10)
                        {
                            Destroy(Button.GetComponent<Renderer>());
                        }
                    }
                    else
                    {
                        Button.GetComponent<Renderer>().material.color = ButtonColorDisable;
                        text2.color = DIsableTextColor;
                        if (Mods.change12 == 10)
                        {
                            Destroy(Button.GetComponent<Renderer>());
                        }
                    }
                }
                if (Mods.inCat4)
                {
                    if (CatButtons4[num].enabled == true)
                    {
                        Button.GetComponent<Renderer>().material.color = ButtonColorEnabled;
                        text2.color = EnableTextColor;
                        if (Mods.change13 == 10)
                        {
                            Destroy(Button.GetComponent<Renderer>());
                        }
                    }
                    else
                    {
                        Button.GetComponent<Renderer>().material.color = ButtonColorDisable;
                        text2.color = DIsableTextColor;
                        if (Mods.change12 == 10)
                        {
                            Destroy(Button.GetComponent<Renderer>());
                        }
                    }
                }
                if (Mods.inCat9)
                {
                    if (CatButtons9[num].enabled == true)
                    {
                        Button.GetComponent<Renderer>().material.color = ButtonColorEnabled;
                        text2.color = EnableTextColor;
                        if (Mods.change13 == 10)
                        {
                            Destroy(Button.GetComponent<Renderer>());
                        }
                    }
                    else
                    {
                        Button.GetComponent<Renderer>().material.color = ButtonColorDisable;
                        text2.color = DIsableTextColor;
                        if (Mods.change12 == 10)
                        {
                            Destroy(Button.GetComponent<Renderer>());
                        }
                    }
                }
                if (Mods.inCat5)
                {
                    if (CatButtons5[num].enabled == true)
                    {
                        Button.GetComponent<Renderer>().material.color = ButtonColorEnabled;
                        text2.color = EnableTextColor;
                        if (Mods.change13 == 10)
                        {
                            Destroy(Button.GetComponent<Renderer>());
                        }
                    }
                    else
                    {
                        Button.GetComponent<Renderer>().material.color = ButtonColorDisable;
                        text2.color = DIsableTextColor;
                        if (Mods.change12 == 10)
                        {
                            Destroy(Button.GetComponent<Renderer>());
                        }
                    }
                }
                if (Mods.inCat6)
                {
                    if (CatButtons6[num].enabled == true)
                    {
                        Button.GetComponent<Renderer>().material.color = ButtonColorEnabled;
                        text2.color = EnableTextColor;
                        if (Mods.change13 == 10)
                        {
                            Destroy(Button.GetComponent<Renderer>());
                        }
                    }
                    else
                    {
                        Button.GetComponent<Renderer>().material.color = ButtonColorDisable;
                        text2.color = DIsableTextColor;
                        if (Mods.change12 == 10)
                        {
                            Destroy(Button.GetComponent<Renderer>());
                        }
                    }
                }
                if (Mods.inCat7)
                {
                    if (CatButtons7[num].enabled == true)
                    {
                        Button.GetComponent<Renderer>().material.color = ButtonColorEnabled;
                        text2.color = EnableTextColor;
                        if (Mods.change13 == 10)
                        {
                            Destroy(Button.GetComponent<Renderer>());
                        }
                    }
                    else
                    {
                        Button.GetComponent<Renderer>().material.color = ButtonColorDisable;
                        text2.color = DIsableTextColor;
                        if (Mods.change12 == 10)
                        {
                            Destroy(Button.GetComponent<Renderer>());
                        }
                    }
                }
                if (Mods.inCat8)
                {
                    if (CatButtons8[num].enabled == true)
                    {
                        Button.GetComponent<Renderer>().material.color = ButtonColorEnabled;
                        text2.color = EnableTextColor;
                        if (Mods.change13 == 10)
                        {
                            Destroy(Button.GetComponent<Renderer>());
                        }
                    }
                    else
                    {
                        Button.GetComponent<Renderer>().material.color = ButtonColorDisable;
                        text2.color = DIsableTextColor;
                        if (Mods.change12 == 10)
                        {
                            Destroy(Button.GetComponent<Renderer>());
                        }
                    }
                }
                if (Mods.inCat10)
                {
                    if (CatButtons10[num].enabled == true)
                    {
                        Button.GetComponent<Renderer>().material.color = ButtonColorEnabled;
                        text2.color = EnableTextColor;
                        if (Mods.change13 == 10)
                        {
                            Destroy(Button.GetComponent<Renderer>());
                        }
                    }
                    else
                    {
                        Button.GetComponent<Renderer>().material.color = ButtonColorDisable;
                        text2.color = DIsableTextColor;
                        if (Mods.change12 == 10)
                        {
                            Destroy(Button.GetComponent<Renderer>());
                        }
                    }
                }
                if (!Mods.inCat1 && !Mods.inCat2 && !Mods.inCat10 && !Mods.inCat3 && !Mods.inCat4 && !Mods.inCat5 && !Mods.inCat6 && !Mods.inCat7 && !Mods.inCat9 && !Mods.inCat8 && !Mods.inSettings)
                {
                    if (buttons[num].enabled == true)
                    {
                        Button.GetComponent<Renderer>().material.color = ButtonColorEnabled;
                        text2.color = EnableTextColor;
                        if (Mods.change13 == 10)
                        {
                            Destroy(Button.GetComponent<Renderer>());
                        }
                    }
                    else
                    {
                        Button.GetComponent<Renderer>().material.color = ButtonColorDisable;
                        text2.color = DIsableTextColor;
                        if (Mods.change12 == 10)
                        {
                            Destroy(Button.GetComponent<Renderer>());
                        }
                    }
                }
            }
        }
        public void Start()
        {
            Draw();
        }
        public static void Toggle(string relatedText)
        {
            if (Mods.inSettings)
            {
                int num = (settingsbuttons.Count + pageSize - 1) / pageSize;
                if (relatedText == "NextPage")
                {
                    if (pageNumber < num - 1)
                    {
                        pageNumber++;
                    }
                    else
                    {
                        pageNumber = 0;
                    }
                    DestroyMenu();
                    instance.Draw();
                }
                else
                {
                    if (relatedText == "PreviousPage")
                    {
                        if (pageNumber > 0)
                        {
                            pageNumber--;
                        }
                        else
                        {
                            pageNumber = num - 1;
                        }
                        DestroyMenu();
                        instance.Draw();
                    }
                    else
                    {
                        if (relatedText == "DisconnectingButton")
                        {
                            PhotonNetwork.Disconnect();
                            GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(Mods.ButtonSound, false, 0.1f);
                        }
                        else
                        {
                            int num2 = -1;
                            for (int i = 0; i < settingsbuttons.Count; i++)
                            {
                                if (relatedText == settingsbuttons[i].buttonText)
                                {
                                    num2 = i;
                                    break;
                                }
                            }
                            if (settingsbuttons[num2].enabled != null)
                            {
                                settingsbuttons[num2].enabled = !settingsbuttons[num2].enabled;
                                lastPressedButtonIndex = num2;
                                if (lastPressedButtonIndex != -1 && lastPressedButtonIndex < settingsbuttons.Count)
                                {
                                    tooltipString = GetButtonTooltip(lastPressedButtonIndex);
                                    tooltipText.text = tooltipString;
                                    lastPressedButtonIndex = -1;
                                }
                                DestroyMenu();
                                instance.Draw();
                            }
                        }
                    }
                }
            }
            else
            {
                if (Mods.inCat1)
                {
                    int num = (CatButtons1.Count + pageSize - 1) / pageSize;
                    if (relatedText == "NextPage")
                    {
                        if (pageNumber < num - 1)
                        {
                            pageNumber++;
                        }
                        else
                        {
                            pageNumber = 0;
                        }
                        DestroyMenu();
                        instance.Draw();
                    }
                    else
                    {
                        if (relatedText == "PreviousPage")
                        {
                            if (pageNumber > 0)
                            {
                                pageNumber--;
                            }
                            else
                            {
                                pageNumber = num - 1;
                            }
                            DestroyMenu();
                            instance.Draw();
                        }
                        else
                        {
                            if (relatedText == "DisconnectingButton")
                            {
                                PhotonNetwork.Disconnect();
                                GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(Mods.ButtonSound, false, 0.1f);
                            }
                            else
                            {
                                int num2 = -1;
                                for (int i = 0; i < CatButtons1.Count; i++)
                                {
                                    if (relatedText == CatButtons1[i].buttonText)
                                    {
                                        num2 = i;
                                        break;
                                    }
                                }
                                if (CatButtons1[num2].enabled != null)
                                {
                                    CatButtons1[num2].enabled = !CatButtons1[num2].enabled;
                                    lastPressedButtonIndex = num2;
                                    if (lastPressedButtonIndex != -1 && lastPressedButtonIndex < CatButtons1.Count)
                                    {
                                        tooltipString = GetButtonTooltip(lastPressedButtonIndex);
                                        tooltipText.text = tooltipString;
                                        lastPressedButtonIndex = -1;
                                    }
                                    DestroyMenu();
                                    instance.Draw();
                                }
                            }
                        }
                    }
                }
                if (Mods.inCat2)
                {
                    int num = (CatButtons2.Count + pageSize - 1) / pageSize;
                    if (relatedText == "NextPage")
                    {
                        if (pageNumber < num - 1)
                        {
                            pageNumber++;
                        }
                        else
                        {
                            pageNumber = 0;
                        }
                        DestroyMenu();
                        instance.Draw();
                    }
                    else
                    {
                        if (relatedText == "PreviousPage")
                        {
                            if (pageNumber > 0)
                            {
                                pageNumber--;
                            }
                            else
                            {
                                pageNumber = num - 1;
                            }
                            DestroyMenu();
                            instance.Draw();
                        }
                        else
                        {
                            if (relatedText == "DisconnectingButton")
                            {
                                PhotonNetwork.Disconnect();
                                GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(Mods.ButtonSound, false, 0.1f);
                            }
                            else
                            {
                                int num2 = -1;
                                for (int i = 0; i < CatButtons2.Count; i++)
                                {
                                    if (relatedText == CatButtons2[i].buttonText)
                                    {
                                        num2 = i;
                                        break;
                                    }
                                }
                                if (CatButtons2[num2].enabled != null)
                                {
                                    CatButtons2[num2].enabled = !CatButtons2[num2].enabled;
                                    lastPressedButtonIndex = num2;
                                    if (lastPressedButtonIndex != -1 && lastPressedButtonIndex < CatButtons2.Count)
                                    {
                                        tooltipString = GetButtonTooltip(lastPressedButtonIndex);
                                        tooltipText.text = tooltipString;
                                        lastPressedButtonIndex = -1;
                                    }
                                    DestroyMenu();
                                    instance.Draw();
                                }
                            }
                        }
                    }
                }
                if (Mods.inCat9)
                {
                    int num = (CatButtons9.Count + pageSize - 1) / pageSize;
                    if (relatedText == "NextPage")
                    {
                        if (pageNumber < num - 1)
                        {
                            pageNumber++;
                        }
                        else
                        {
                            pageNumber = 0;
                        }
                        DestroyMenu();
                        instance.Draw();
                    }
                    else
                    {
                        if (relatedText == "PreviousPage")
                        {
                            if (pageNumber > 0)
                            {
                                pageNumber--;
                            }
                            else
                            {
                                pageNumber = num - 1;
                            }
                            DestroyMenu();
                            instance.Draw();
                        }
                        else
                        {
                            if (relatedText == "DisconnectingButton")
                            {
                                PhotonNetwork.Disconnect();
                                GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(Mods.ButtonSound, false, 0.1f);
                            }
                            else
                            {
                                int num2 = -1;
                                for (int i = 0; i < CatButtons9.Count; i++)
                                {
                                    if (relatedText == CatButtons9[i].buttonText)
                                    {
                                        num2 = i;
                                        break;
                                    }
                                }
                                if (CatButtons9[num2].enabled != null)
                                {
                                    CatButtons9[num2].enabled = !CatButtons9[num2].enabled;
                                    lastPressedButtonIndex = num2;
                                    if (lastPressedButtonIndex != -1 && lastPressedButtonIndex < CatButtons9.Count)
                                    {
                                        tooltipString = GetButtonTooltip(lastPressedButtonIndex);
                                        tooltipText.text = tooltipString;
                                        lastPressedButtonIndex = -1;
                                    }
                                    DestroyMenu();
                                    instance.Draw();
                                }
                            }
                        }
                    }
                }
                if (Mods.inCat3)
                {
                    int num = (CatButtons3.Count + pageSize - 1) / pageSize;
                    if (relatedText == "NextPage")
                    {
                        if (pageNumber < num - 1)
                        {
                            pageNumber++;
                        }
                        else
                        {
                            pageNumber = 0;
                        }
                        DestroyMenu();
                        instance.Draw();
                    }
                    else
                    {
                        if (relatedText == "PreviousPage")
                        {
                            if (pageNumber > 0)
                            {
                                pageNumber--;
                            }
                            else
                            {
                                pageNumber = num - 1;
                            }
                            DestroyMenu();
                            instance.Draw();
                        }
                        else
                        {
                            if (relatedText == "DisconnectingButton")
                            {
                                PhotonNetwork.Disconnect();
                                GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(Mods.ButtonSound, false, 0.1f);
                            }
                            else
                            {
                                int num2 = -1;
                                for (int i = 0; i < CatButtons3.Count; i++)
                                {
                                    if (relatedText == CatButtons3[i].buttonText)
                                    {
                                        num2 = i;
                                        break;
                                    }
                                }
                                if (CatButtons3[num2].enabled != null)
                                {
                                    CatButtons3[num2].enabled = !CatButtons3[num2].enabled;
                                    lastPressedButtonIndex = num2;
                                    if (lastPressedButtonIndex != -1 && lastPressedButtonIndex < CatButtons3.Count)
                                    {
                                        tooltipString = GetButtonTooltip(lastPressedButtonIndex);
                                        tooltipText.text = tooltipString;
                                        lastPressedButtonIndex = -1;
                                    }
                                    DestroyMenu();
                                    instance.Draw();
                                }
                            }
                        }
                    }
                }
                if (Mods.inCat4)
                {
                    int num = (CatButtons4.Count + pageSize - 1) / pageSize;
                    if (relatedText == "NextPage")
                    {
                        if (pageNumber < num - 1)
                        {
                            pageNumber++;
                        }
                        else
                        {
                            pageNumber = 0;
                        }
                        DestroyMenu();
                        instance.Draw();
                    }
                    else
                    {
                        if (relatedText == "PreviousPage")
                        {
                            if (pageNumber > 0)
                            {
                                pageNumber--;
                            }
                            else
                            {
                                pageNumber = num - 1;
                            }
                            DestroyMenu();
                            instance.Draw();
                        }
                        else
                        {
                            if (relatedText == "DisconnectingButton")
                            {
                                PhotonNetwork.Disconnect();
                                GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(Mods.ButtonSound, false, 0.1f);
                            }
                            else
                            {
                                int num2 = -1;
                                for (int i = 0; i < CatButtons4.Count; i++)
                                {
                                    if (relatedText == CatButtons4[i].buttonText)
                                    {
                                        num2 = i;
                                        break;
                                    }
                                }
                                if (CatButtons4[num2].enabled != null)
                                {
                                    CatButtons4[num2].enabled = !CatButtons4[num2].enabled;
                                    lastPressedButtonIndex = num2;
                                    if (lastPressedButtonIndex != -1 && lastPressedButtonIndex < CatButtons4.Count)
                                    {
                                        tooltipString = GetButtonTooltip(lastPressedButtonIndex);
                                        tooltipText.text = tooltipString;
                                        lastPressedButtonIndex = -1;
                                    }
                                    DestroyMenu();
                                    instance.Draw();
                                }
                            }
                        }
                    }
                }
                if (Mods.inCat6)
                {
                    int num = (CatButtons6.Count + pageSize - 1) / pageSize;
                    if (relatedText == "NextPage")
                    {
                        if (pageNumber < num - 1)
                        {
                            pageNumber++;
                        }
                        else
                        {
                            pageNumber = 0;
                        }
                        DestroyMenu();
                        instance.Draw();
                    }
                    else
                    {
                        if (relatedText == "PreviousPage")
                        {
                            if (pageNumber > 0)
                            {
                                pageNumber--;
                            }
                            else
                            {
                                pageNumber = num - 1;
                            }
                            DestroyMenu();
                            instance.Draw();
                        }
                        else
                        {
                            if (relatedText == "DisconnectingButton")
                            {
                                PhotonNetwork.Disconnect();
                                GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(Mods.ButtonSound, false, 0.1f);
                            }
                            else
                            {
                                int num2 = -1;
                                for (int i = 0; i < CatButtons6.Count; i++)
                                {
                                    if (relatedText == CatButtons6[i].buttonText)
                                    {
                                        num2 = i;
                                        break;
                                    }
                                }
                                if (CatButtons6[num2].enabled != null)
                                {
                                    CatButtons6[num2].enabled = !CatButtons6[num2].enabled;
                                    lastPressedButtonIndex = num2;
                                    if (lastPressedButtonIndex != -1 && lastPressedButtonIndex < CatButtons6.Count)
                                    {
                                        tooltipString = GetButtonTooltip(lastPressedButtonIndex);
                                        tooltipText.text = tooltipString;
                                        lastPressedButtonIndex = -1;
                                    }
                                    DestroyMenu();
                                    instance.Draw();
                                }
                            }
                        }
                    }
                }
                if (Mods.inCat7)
                {
                    int num = (CatButtons7.Count + pageSize - 1) / pageSize;
                    if (relatedText == "NextPage")
                    {
                        if (pageNumber < num - 1)
                        {
                            pageNumber++;
                        }
                        else
                        {
                            pageNumber = 0;
                        }
                        DestroyMenu();
                        instance.Draw();
                    }
                    else
                    {
                        if (relatedText == "PreviousPage")
                        {
                            if (pageNumber > 0)
                            {
                                pageNumber--;
                            }
                            else
                            {
                                pageNumber = num - 1;
                            }
                            DestroyMenu();
                            instance.Draw();
                        }
                        else
                        {
                            if (relatedText == "DisconnectingButton")
                            {
                                PhotonNetwork.Disconnect();
                                GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(Mods.ButtonSound, false, 0.1f);
                            }
                            else
                            {
                                int num2 = -1;
                                for (int i = 0; i < CatButtons7.Count; i++)
                                {
                                    if (relatedText == CatButtons7[i].buttonText)
                                    {
                                        num2 = i;
                                        break;
                                    }
                                }
                                if (CatButtons7[num2].enabled != null)
                                {
                                    CatButtons7[num2].enabled = !CatButtons7[num2].enabled;
                                    lastPressedButtonIndex = num2;
                                    if (lastPressedButtonIndex != -1 && lastPressedButtonIndex < CatButtons7.Count)
                                    {
                                        tooltipString = GetButtonTooltip(lastPressedButtonIndex);
                                        tooltipText.text = tooltipString;
                                        lastPressedButtonIndex = -1;
                                    }
                                    DestroyMenu();
                                    instance.Draw();
                                }
                            }
                        }
                    }
                }
                if (Mods.inCat8)
                {
                    int num = (CatButtons8.Count + pageSize - 1) / pageSize;
                    if (relatedText == "NextPage")
                    {
                        if (pageNumber < num - 1)
                        {
                            pageNumber++;
                        }
                        else
                        {
                            pageNumber = 0;
                        }
                        DestroyMenu();
                        instance.Draw();
                    }
                    else
                    {
                        if (relatedText == "PreviousPage")
                        {
                            if (pageNumber > 0)
                            {
                                pageNumber--;
                            }
                            else
                            {
                                pageNumber = num - 1;
                            }
                            DestroyMenu();
                            instance.Draw();
                        }
                        else
                        {
                            if (relatedText == "DisconnectingButton")
                            {
                                PhotonNetwork.Disconnect();
                                GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(Mods.ButtonSound, false, 0.1f);
                            }
                            else
                            {
                                int num2 = -1;
                                for (int i = 0; i < CatButtons8.Count; i++)
                                {
                                    if (relatedText == CatButtons8[i].buttonText)
                                    {
                                        num2 = i;
                                        break;
                                    }
                                }
                                if (CatButtons8[num2].enabled != null)
                                {
                                    CatButtons8[num2].enabled = !CatButtons8[num2].enabled;
                                    lastPressedButtonIndex = num2;
                                    if (lastPressedButtonIndex != -1 && lastPressedButtonIndex < CatButtons8.Count)
                                    {
                                        tooltipString = GetButtonTooltip(lastPressedButtonIndex);
                                        tooltipText.text = tooltipString;
                                        lastPressedButtonIndex = -1;
                                    }
                                    DestroyMenu();
                                    instance.Draw();
                                }
                            }
                        }
                    }
                }
                if (Mods.inCat5)
                {
                    int num = (CatButtons5.Count + pageSize - 1) / pageSize;
                    if (relatedText == "NextPage")
                    {
                        if (pageNumber < num - 1)
                        {
                            pageNumber++;
                        }
                        else
                        {
                            pageNumber = 0;
                        }
                        DestroyMenu();
                        instance.Draw();
                    }
                    else
                    {
                        if (relatedText == "PreviousPage")
                        {
                            if (pageNumber > 0)
                            {
                                pageNumber--;
                            }
                            else
                            {
                                pageNumber = num - 1;
                            }
                            DestroyMenu();
                            instance.Draw();
                        }
                        else
                        {
                            if (relatedText == "DisconnectingButton")
                            {
                                PhotonNetwork.Disconnect();
                                GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(Mods.ButtonSound, false, 0.1f);
                            }
                            else
                            {
                                int num2 = -1;
                                for (int i = 0; i < CatButtons5.Count; i++)
                                {
                                    if (relatedText == CatButtons5[i].buttonText)
                                    {
                                        num2 = i;
                                        break;
                                    }
                                }
                                if (CatButtons5[num2].enabled != null)
                                {
                                    CatButtons5[num2].enabled = !CatButtons5[num2].enabled;
                                    lastPressedButtonIndex = num2;
                                    if (lastPressedButtonIndex != -1 && lastPressedButtonIndex < CatButtons5.Count)
                                    {
                                        tooltipString = GetButtonTooltip(lastPressedButtonIndex);
                                        tooltipText.text = tooltipString;
                                        lastPressedButtonIndex = -1;
                                    }
                                    DestroyMenu();
                                    instance.Draw();
                                }
                            }
                        }
                    }
                }
                if (Mods.inCat10)
                {
                    int num = (CatButtons10.Count + pageSize - 1) / pageSize;
                    if (relatedText == "NextPage")
                    {
                        if (pageNumber < num - 1)
                        {
                            pageNumber++;
                        }
                        else
                        {
                            pageNumber = 0;
                        }
                        DestroyMenu();
                        instance.Draw();
                    }
                    else
                    {
                        if (relatedText == "PreviousPage")
                        {
                            if (pageNumber > 0)
                            {
                                pageNumber--;
                            }
                            else
                            {
                                pageNumber = num - 1;
                            }
                            DestroyMenu();
                            instance.Draw();
                        }
                        else
                        {
                            if (relatedText == "DisconnectingButton")
                            {
                                PhotonNetwork.Disconnect();
                                GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(Mods.ButtonSound, false, 0.1f);
                            }
                            else
                            {
                                int num2 = -1;
                                for (int i = 0; i < CatButtons10.Count; i++)
                                {
                                    if (relatedText == CatButtons10[i].buttonText)
                                    {
                                        num2 = i;
                                        break;
                                    }
                                }
                                if (CatButtons10[num2].enabled != null)
                                {
                                    CatButtons10[num2].enabled = !CatButtons10[num2].enabled;
                                    lastPressedButtonIndex = num2;
                                    if (lastPressedButtonIndex != -1 && lastPressedButtonIndex < CatButtons10.Count)
                                    {
                                        tooltipString = GetButtonTooltip(lastPressedButtonIndex);
                                        tooltipText.text = tooltipString;
                                        lastPressedButtonIndex = -1;
                                    }
                                    DestroyMenu();
                                    instance.Draw();
                                }
                            }
                        }
                    }
                }
                if (!Mods.inCat1 && !Mods.inCat2 && !Mods.inCat10 && !Mods.inCat3 && !Mods.inCat4 && !Mods.inCat5 && !Mods.inCat6 && !Mods.inCat7 && !Mods.inCat9 && !Mods.inCat8 && !Mods.inSettings)
                {
                    int num = (buttons.Count + pageSize - 1) / pageSize;
                    if (relatedText == "NextPage")
                    {
                        if (pageNumber < num - 1)
                        {
                            pageNumber++;
                        }
                        else
                        {
                            pageNumber = 0;
                        }
                        DestroyMenu();
                        instance.Draw();
                    }
                    else
                    {
                        if (relatedText == "PreviousPage")
                        {
                            if (pageNumber > 0)
                            {
                                pageNumber--;
                            }
                            else
                            {
                                pageNumber = num - 1;
                            }
                            DestroyMenu();
                            instance.Draw();
                        }
                        else
                        {
                            if (relatedText == "DisconnectingButton")
                            {
                                PhotonNetwork.Disconnect();
                                GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(Mods.ButtonSound, false, 0.1f);
                            }
                            else
                            {
                                int num2 = -1;
                                for (int i = 0; i < buttons.Count; i++)
                                {
                                    if (relatedText == buttons[i].buttonText)
                                    {
                                        num2 = i;
                                        break;
                                    }
                                }
                                if (buttons[num2].enabled != null)
                                {
                                    buttons[num2].enabled = !buttons[num2].enabled;
                                    lastPressedButtonIndex = num2;
                                    if (lastPressedButtonIndex != -1 && lastPressedButtonIndex < buttons.Count)
                                    {
                                        tooltipString = GetButtonTooltip(lastPressedButtonIndex);
                                        tooltipText.text = tooltipString;
                                        lastPressedButtonIndex = -1;
                                    }
                                    DestroyMenu();
                                    instance.Draw();
                                }
                            }
                        }
                    }
                }
            }
        }
        #endregion
    }
}

internal class BtnCollider : MonoBehaviour
{
    #region Button Press Stuff
    public static int framePressCooldown = 0;
    private void OnTriggerEnter(Collider collider)
    {
        if (Time.frameCount >= framePressCooldown + WristMenu.ClickCooldown && collider.name == "buttonPresser")
        {
            if (!Mods.right)
            {
                GorillaTagger.Instance.StartVibration(false, .01f, 0.001f);
                GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(Mods.ButtonSound, true, 0.1f);
            }
            else
            {
                GorillaTagger.Instance.StartVibration(true, .01f, 0.001f);
                GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(Mods.ButtonSound, false, 0.1f);
            }
            WristMenu.Toggle(relatedText);
            framePressCooldown = Time.frameCount;
        }
    }
    public string relatedText;
    #endregion
}