using UnityEngine;

namespace Domain.UI.Widgets
{
    public class TurnQueueWidget : MonoBehaviour
    {
        [SerializeField] private Transform ImagesContainer;
        [SerializeField] private TurnQueueElementView QueueElementPrefab;
                
        public TurnQueueElementView AddNewInQueue(){
            return Instantiate(QueueElementPrefab, ImagesContainer);
        }
    }
}
