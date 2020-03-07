using Assets.Source.Components.Base;
using Assets.Source.Components.UI.Base;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Source.Components.UI
{
    public class CanvasMenuSelectorComponent : ComponentBase
    {

        // todo:  Handling multiple open menus will be tricky but very doable.  We just need to scrape all the buttons and 
        // set IsInteractable or whatever to false, which will disable them.  For now, lets not bother with adding that logic, 
        // and assume only one menu can be open at a time.

        public void ShowMenu<TMenuComponent>() where TMenuComponent : MenuComponentBase 
        {
            CloseAllMenus();
            var menu = GetMenuComponent<TMenuComponent>();
            menu.gameObject.SetActive(true);
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(menu.FirstSelectedItem);
        }

        // I don't think this functionality is needed but here is is just in case (untested)
        //public void CloseMenu<TMenuComponent>() where TMenuComponent : MenuComponentBase
        //{
        //    var menu = GetMenuComponent<TMenuComponent>();
        //    menu.gameObject.SetActive(false);
        //}

        public void CloseAllMenus()
        {
            var menuComponents = GetComponentsInChildren<MenuComponentBase>(true);

            foreach (var menuComponent in menuComponents) 
            {
                menuComponent.gameObject.SetActive(false);            
            }
        }

        private MenuComponentBase GetMenuComponent<TMenuComponent>() where TMenuComponent : MenuComponentBase
        {
            var menuComponents = GetComponentsInChildren<TMenuComponent>(true);

            if (menuComponents.Length > 1)
            {
                throw new UnityException($"More than one menu component found for type {typeof(TMenuComponent).Name}." +
                    $"Make sure that only one copy of each menu exists in the canvas prefab");
            }
            else if (!menuComponents.Any())
            {
                throw new UnityException($"Unable to find a menu component for type type {typeof(TMenuComponent).Name}.");
            }

            return menuComponents.First();
        }

    }
}
