using Baruah.Database;
using Baruah.Service;
using SQLite;

namespace Baruah.Mission
{
    public class MissionTable : BaseTable<MissionData>
    {
        public override void CreateTable()
        {
            ServiceManager.Get<DatabaseService>().Execute(@"
        CREATE TABLE IF NOT EXISTS MissionData (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            GameId INTEGER NOT NULL,
            MissionId VARCHAR(100) NOT NULL,
            Status INTEGER,
            Timestamp timestamp NOT NULL,
            CONSTRAINT fk_game FOREIGN KEY (GameId) 
                REFERENCES SaveGameData(Id) 
                ON DELETE CASCADE,
            CONSTRAINT UC_MissionData UNIQUE (MissionId, GameId)
        );
");
        }
    }
    
    [System.Serializable]
    public class MissionData : IData
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        
        public string MissionId { get; set; }
        public int GameId { get; set; }

        public MissionStatus Status { get; set; } = MissionStatus.Locked;
        public System.DateTime Timestamp { get; set; } = System.DateTime.Now;

        public enum MissionStatus
        {
            Locked,
            Available,
            InProgress,
            Completed,
            Failed
        }
    }
}
