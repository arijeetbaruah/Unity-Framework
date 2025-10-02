using Baruah.Database;

namespace Baruah.Service
{
    public class GameService : IService
    {
        public int GameId { get; private set; }
        
        private SaveGameTable _gameTable;
        
        public void Initialize()
        {
            _gameTable = new SaveGameTable();
            if (_gameTable.GetTable().Count() == 0)
            {
                _gameTable.InsertData(new SaveGameData());
            }

            var gameData = _gameTable.GetTable().FirstOrDefault();
            GameId = gameData.Id;
        }

        public void Update()
        {
        }

        public void OnDestroy()
        {
            _gameTable = null;
        }
    }
}
