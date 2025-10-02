using UnityEngine;

namespace Baruah.Mission
{
    public abstract class IntObjective : Objective
    {
        [SerializeField] private int _goal;

        private int _current;

        public override void Update()
        {
            if (Valid())
            {
                _current++;

                if (_current >= _goal)
                {
                    MarkAsComplete();
                }
            }
        }
    }
}
