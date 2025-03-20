using UnityEngine;

namespace MoonlitMixes.ObjectSpwan
{
    public class InteractableObject : MonoBehaviour
    {
        [SerializeField] InteractableObjectType _interactableObjectType;

        public void Interaction(/*Ici mettre le type d'outil*/)
        {
            /*if (_interactableObjectType == InteractableObjectType.Machete && tool == Machete)
            {
                GiveItem();
                DeleteObject();
                Break;
            }
            else if (_interactableObjectType == InteractableObjectType.Pickaxe && tool == Pickaxe)
            {
                GiveItem();
                DeleteObject();
                Break;
            }
            else if (_interactableObjectType == InteractableObjectType.NoTool && tool == NoTool)
            {
                GiveItem();
                DeleteObject();
                Break;
            }
            else
            {
                Debug.Log("DoNothing");
                break;
            }*/
        }

        private void DeleteObject()
        {
        
        }

        private void GiveItem()
        {
        
        }
    }
}
