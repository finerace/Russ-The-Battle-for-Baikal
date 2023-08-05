using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class MenuSystem : MonoBehaviour
{
    [SerializeField] private ParentMenuData startMenuData;
    
    [Space]
    
    private string menusPath;
    private readonly List<MenuData> menusDataPath = new List<MenuData>();
    
    [Space]
    private MenuData currentMenuData;

    private bool currentMenuIsParent = true;
    public bool isBackActionLock = false;
    
    private Camera mainCamera;

    private PlayerMain playerMain;
    
    public MenuData CurrentMenuData => currentMenuData;
    
    private void Awake()
    {
        mainCamera = Camera.main;
        playerMain = FindObjectOfType<PlayerMain>();
        
        InitMenusData();
    }

    private void Start()
    {
        OpenStartMenu();
    }

    private void Update()
    {
        if(isBackActionLock)
            return;
        
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Back();
        }
    }

    private void InitMenusData()
    {
        InitMenu(startMenuData);
        
        void InitMenu(MenuData menuData)
        {
            if(menuData.menu != null)
                return;
            
            var spawnedMenu = Instantiate(menuData.menuPrefab);
            spawnedMenu.GetComponent<Canvas>().worldCamera = mainCamera;
            SetMenuSystemRef();
            
            menuData.menu = spawnedMenu;
            
            
            if(!IsMenuDataParent(menuData))
                return;

            var parentMenuData = (ParentMenuData)menuData;
            
            InitChildMenusData(parentMenuData);
            

            void InitChildMenusData(ParentMenuData parentMenuData)
            {
                foreach (var menuChildData in parentMenuData.childsMenusData)
                    InitMenu(menuChildData);

                foreach (var parentChildMenuData in parentMenuData.childsParentsMenusData)
                    InitMenu(parentChildMenuData);
            }

            void SetMenuSystemRef()
            {
                if (spawnedMenu.TryGetComponent(out MenuSystemReference menuSystemReference))
                    menuSystemReference.SetMenuSystemReference(this);
            }
        }

        bool IsMenuDataParent(MenuData menuData)
        {
            return menuData.GetType() == typeof(ParentMenuData);
        }
    }
    
    public void OpenLocalMenu(string menuID)
    {
        if (!currentMenuIsParent)
        {
            print($"Open menu has failed! \n" + $"Current menu {menuID} is not parent!");
            
            return;
        }

        var currentParentMenu = (ParentMenuData)currentMenuData;

        var foundParentMenuData =
            FindLocalParentMenu(currentParentMenu, menuID);

        var foundChildMenuData = 
            FindLocalChildMenu(currentParentMenu, menuID);

        if (foundChildMenuData != null && foundParentMenuData != null)
        {
            throw new InvalidDataException($"Open menu has failed!\n" + $"Menu ID {menuID} is repeated!");
        }
        
        if(foundParentMenuData != null)
        {
            currentMenuData = foundParentMenuData;
            currentMenuIsParent = true;

        }
        else if(foundChildMenuData != null)
        {
            currentMenuData = foundChildMenuData;
            currentMenuIsParent = false;
        }

        ActivateMenu(currentMenuData);
    }

    public void Back()
    {
        if (currentMenuData == startMenuData)
        {
            OpenLocalMenu("PauseMenu");

            return;
        }

        var backMenu = menusDataPath[menusDataPath.Count - 2];

        menusDataPath.Remove(currentMenuData);
        
        CloseAllMenus();
        
        currentMenuData = backMenu;
        currentMenuData.menu.SetActive(true);
        currentMenuIsParent = true;

        UpdateMenuPath();
        SetMenuSpecialSettings(currentMenuData);
    }

    private void OpenStartMenu()
    {
        ActivateMenu(startMenuData);
    }

    private void ActivateMenu(MenuData menuData)
    {
        CloseAllMenus();
        currentMenuData = menuData;
        menuData.menu.SetActive(true);

        menusPath += $"/{menuData.menuID}";
        menusDataPath.Add(menuData);

        SetMenuSpecialSettings(menuData);

        UpdateMenuPath();
    }

    private ParentMenuData FindLocalParentMenu(ParentMenuData parentMenu, string toFindMenuID)
    {
        return parentMenu.childsParentsMenusData.FirstOrDefault(item => item.menuID == toFindMenuID);
    }

    private MenuData FindLocalChildMenu(ParentMenuData parentMenu,string toFindMenuID)
    {
        return parentMenu.childsMenusData.FirstOrDefault(item => item.menuID == toFindMenuID);
    }

    private void UpdateMenuPath()
    {
        menusPath = "";

        foreach (var item in menusDataPath)
        {
            menusPath += $"{item.menuID}/";
        }
    }

    private void SetMenuSpecialSettings(MenuData menuData)
    {
        SetCursorActive(menuData.isCursorActive);

        SetTimeScaleActive(menuData.isTimeNotActive);

        var playerManageActive =
            !menuData.isCursorActive;

        SetPlayerManageActive(playerManageActive);

        void SetCursorActive(bool state)
        {
            if (state)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {

                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        void SetTimeScaleActive(bool state)
        {
            Time.timeScale = state ? 0 : 1;
        }
        
        void SetPlayerManageActive(bool state)
        {
            playerMain.SetManageActive(state);
        }
    }

    private void CloseAllMenus()
    {
        startMenuData.Disable();
    }

}

[System.Serializable]
public class MenuData
{
    public string menuID;
    public GameObject menuPrefab;
    public GameObject menu;
    public bool isCursorActive = true;
    public bool isTimeNotActive = true;
}

[System.Serializable]
public class ParentMenuData : MenuData
{
    public List<MenuData> childsMenusData = new List<MenuData>();
    public List<ParentMenuData> childsParentsMenusData = new List<ParentMenuData>();

    public void Disable()
    {
        foreach (var item in childsMenusData.Where(item => item.menu.activeSelf))
        {
            item.menu.SetActive(false);
        }

        foreach (var item in childsParentsMenusData)
        {
            item.Disable();

            if (item.menu.activeSelf)
                item.menu.SetActive(false);
        }

        if (menu.activeSelf)
            menu.SetActive(false);
    }

}
