using System.Collections.Generic;
using System.Linq;
using Baruah.Requirements;
using Baruah.Service;
using UnityEngine;

namespace Baruah.Mission
{
    [CreateAssetMenu(fileName = "QuestLine", menuName = "Quest System/QuestLine")]
    public class QuestLine : ScriptableObject
    {
        public IEnumerable<Objective> Objectives => _objectives;
        
        [field:SerializeField] public string MissionID { get; private set; }
        
        [SerializeField, SerializeReference] private Objective[] _objectives;
        
        [SerializeField, SerializeReference] private BaseRequirement[] _requirements;

        public void StartQuest()
        {
            ServiceManager.Get<MissionService>().StartMission(this);
        }
        
        public void UpdateQuestList()
        {
            if (_objectives.All(objective => objective.Status == MissionData.MissionStatus.Completed))
            {
                MarkAsComplete();
            }
        }

        private void MarkAsComplete()
        {
            ServiceManager.Get<MissionService>().CompleteMission(this);
        }

        public bool IsRequirementMet() => _requirements == null ? true : _requirements.All(requirement => requirement.CheckRequirements());
    }
}
