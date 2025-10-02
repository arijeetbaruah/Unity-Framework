using SQLite;

namespace Baruah.Database
{
    public class SaveGameData : IData
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        
        public System.DateTime CreatedAt { get; set; } = System.DateTime.Now;
        public System.DateTime ModifiedAt { get; set; } = System.DateTime.Now;
    }

    public class SaveGameTable : BaseTable<SaveGameData>
    {
    }
}
