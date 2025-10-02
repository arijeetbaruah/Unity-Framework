using System.Linq;
using Baruah.Database;
using Baruah.Service;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Baruah.Mission
{
    public class MissionService : IService
    {
        private MissionTable _missionTable;
        private ObjectiveTable _objectiveTable;
        
        private QuestLineDatabase _questLineDatabase;

        private int GameId => ServiceManager.Get<GameService>().GameId;
        
        public async void Initialize()
        {
            _missionTable = new MissionTable();
            _objectiveTable = new ObjectiveTable();

            var handler = Addressables.LoadAssetAsync<QuestLineDatabase>("QuestLine Database");
            await handler.Task;
            
            _questLineDatabase = handler.Result;
            _questLineDatabase.QuestLines.FirstOrDefault().StartQuest();
        }

        public void Update()
        {
            
        }

        public void OnDestroy()
        {
        }

        public void UpdateQuests()
        {
            foreach (var questLine in _questLineDatabase.QuestLines)
            {
                var mission = _missionTable.GetTable()
                    .Where(mission => mission.MissionId == questLine.MissionID && mission.GameId == GameId)
                    .FirstOrDefault();
                if (mission != null && mission.Status == MissionData.MissionStatus.Completed)
                    continue;

                if (questLine.IsRequirementMet())
                {
                    if (mission != null && mission.Status == MissionData.MissionStatus.Locked)
                    {
                        mission.Status = MissionData.MissionStatus.Available;
                        _missionTable.UpdateData(mission);
                    }
                    else if (mission == null)
                    {
                        mission = new MissionData
                        {
                            MissionId = questLine.MissionID,
                            GameId = GameId,
                            Status = MissionData.MissionStatus.Available
                        };
                        _missionTable.InsertData(mission);
                    }
                }
            }
        }

        public MissionData GetMission(string missionId)
        {
            MissionData missionData = _missionTable.FetchDataByPrimaryKey(missionId);
            if (missionData == null)
            {
                missionData = new MissionData { MissionId = missionId, GameId = GameId, Status = MissionData.MissionStatus.Locked };
            }
            
            return missionData;
        }

        public void CompleteObjective(Objective objective, QuestLine questLine)
        {
            var objectiveData = _objectiveTable.GetTable()
                .Where(obj => obj.ObjectiveId == objective.ObjectiveID)
                .Where(obj => obj.MissionId == questLine.MissionID)
                .Where(obj => obj.GameId == GameId)
                .FirstOrDefault();
            
            objectiveData.MissionStatus = MissionData.MissionStatus.Completed;
            
            _objectiveTable.UpdateData(objectiveData);

            questLine.UpdateQuestList();
        }

        public void UpdateObjective(ObjectiveData objective)
        {
            _objectiveTable.UpdateData(objective);
        }
        
        public void InsertObjective(ObjectiveData objective)
        {
            _objectiveTable.InsertData(objective);
        }
        
        public ObjectiveData GetObjective(string objectiveId)
        {
            return _objectiveTable.GetTable().Where(objective => objective.ObjectiveId == objectiveId).FirstOrDefault();
        }

        public ObjectiveData GetObjective(Objective objective)
        {
            return GetObjective(objective.ObjectiveID);
        }

        public void StartMission(QuestLine questLine)
        {
            var missionQuery = _missionTable.GetTable()
                .Where(mission => mission.MissionId == questLine.MissionID && mission.GameId == GameId);
            
            var mission = missionQuery.FirstOrDefault();

            if (mission != null)
            {
                mission.Status = MissionData.MissionStatus.InProgress;
                _missionTable.UpdateData(mission);
            }
            else
            {
                mission = new MissionData
                {
                    MissionId = questLine.MissionID,
                    GameId = GameId,
                    Status = MissionData.MissionStatus.InProgress
                };
                _missionTable.InsertData(mission);
            }
            

            foreach (var objective in questLine.Objectives)
            {
                objective.Initialize(questLine);
            }
        }

        public void CompleteMission(QuestLine questLine)
        {
            var mission = _missionTable.GetTable().Where(mission => mission.MissionId == questLine.MissionID).FirstOrDefault();
            mission.Status = MissionData.MissionStatus.Completed;
            _missionTable.UpdateData(mission);
            UpdateQuests();
        }
    }
}
