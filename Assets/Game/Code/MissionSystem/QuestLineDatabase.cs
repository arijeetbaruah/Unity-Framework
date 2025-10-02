using System.Collections.Generic;
using UnityEngine;

namespace Baruah.Mission
{
    [CreateAssetMenu(fileName = "QuestLine Database", menuName = "Quest System/QuestLine Database")]
    public class QuestLineDatabase : ScriptableObject
    {
        public IEnumerable<QuestLine> QuestLines => _questLines;
        
        [SerializeField] private QuestLine[] _questLines;
    }
}
