using Project.Controllers;
using UnityEditor.Localization.Plugins.XLIFF.V12;

namespace UnityUtilities.UnityResources{
    
    
    public static class UnityResources{
        
        static UnityResources(){
            NotifyController = "Notifies/PlanningStageNotifyPrefab".LoadResource<NotifyMessageController>();
        }

        public readonly static NotifyMessageController NotifyController;
    }
}
