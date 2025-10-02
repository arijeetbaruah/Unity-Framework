using Baruah.Requirements;
using Baruah.Service;
using UnityEngine;

namespace Baruah.Mission
{
    [System.Serializable]
    public class MissionRequirement : BaseRequirement
    {
        [SerializeField] private string _missionId;
        [SerializeField] private MissionData.MissionStatus _status;
        
        public override bool CheckRequirements()
        {
            var mission = ServiceManager.Get<MissionService>().GetMission("mission 1");
            
            return mission.Status == _status;
        }
    }
}
