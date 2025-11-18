using Domain.Map;
using UnityEngine;

namespace Project
{
    public class LabRMBWatcher : MonoBehaviour
    {
        private LabMonsterCraftController craftController;

        void Start()
        {
            craftController = LabReferences.Instance().craftController;
        }

        void Update()
        {
            // Обработка ПКМ в любом месте экрана для отмены
            if (Input.GetMouseButtonDown(1) && craftController.isHoldingResource)
            {
                craftController.ReturnHeldResource();
            }
        }
    }
}