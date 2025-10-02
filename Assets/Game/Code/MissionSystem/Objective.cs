using System.Collections.Generic;
using System.Linq;
using Baruah.Database;
using Baruah.Requirements;
using Baruah.Service;
using SQLite;
using UnityEngine;

namespace Baruah.Mission
{
    [System.Serializable]
    public abstract class Objective
    {
        public string ObjectiveID => _objectiveID;
        public MissionData.MissionStatus Status => _objectiveData.MissionStatus;
        
        [SerializeField] private string _objectiveID;
        [SerializeField, SerializeReference] private List<BaseRequirement> _requirement = new();
        
        private QuestLine _questLine;
        private ObjectiveData _objectiveData;
        
        public abstract void Hookup();
        public abstract void UnHookup();

        public virtual void Initialize(QuestLine questLine)
        {
            _questLine = questLine;
            _objectiveData = ServiceManager.Get<MissionService>().GetObjective(this);
            if (_objectiveData == null)
            {
                bool isUnlocked = _requirement.All(requirement => requirement.CheckRequirements());
                
                _objectiveData = new ObjectiveData
                {
                    MissionId = _questLine.MissionID,
                    ObjectiveId = _objectiveID,
                    GameId = ServiceManager.Get<GameService>().GameId,
                    MissionStatus = isUnlocked ? MissionData.MissionStatus.InProgress : MissionData.MissionStatus.Locked         
                };
                
                ServiceManager.Get<MissionService>().InsertObjective(_objectiveData);
            }
            
            Hookup();
            Update();
        }
        
        public abstract bool Valid();
        public abstract void Update();

        public virtual void MarkAsComplete()
        {
            UnHookup();

            ServiceManager.Get<MissionService>().CompleteObjective(this, _questLine);
        }
    }

    public class ObjectiveData : IData
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        
        public string ObjectiveId { get; set; }
        public string MissionId { get; set; }
        public int GameId { get; set; }

        public MissionData.MissionStatus MissionStatus { get; set; } = MissionData.MissionStatus.Available;
    }

    public class ObjectiveTable : BaseTable<ObjectiveData>
    {
        public override void CreateTable()
        {
            ServiceManager.Get<DatabaseService>().Execute(@"
        CREATE TABLE IF NOT EXISTS ObjectiveData (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            ObjectiveId VARCHAR(100) NOT NULL,
            GameId INTEGER NOT NULL,
            MissionId VARCHAR(100) NOT NULL,
            MissionStatus INTEGER,
            CONSTRAINT UC_ObjectiveData UNIQUE (ObjectiveId, MissionId, GameId)
        );
");
        }
    }
}
