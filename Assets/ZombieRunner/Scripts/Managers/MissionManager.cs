using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Runner 
{
    [Serializable]
    public class MissionManager : ComponentManager
    {
        public Mission[] Missions;
        [SerializeField]
        private List<Mission> _queueMissions = new List<Mission>();
        [SerializeField]
        private List<Mission> _currentMissions = new List<Mission>(3);
        [SerializeField]
        private int _stack = 3;
        [SerializeField]
        private List<Mission> _completedMissions = new List<Mission>();

        public event Action<Mission[], MissionManager> OnComplete;

        public int Stack
        {
            get { return _stack; }
            set
            {
                _stack = value;
                if (_stack <= 0) _stack = 1;
                if (_currentMissions.Count < _stack)
                {
                    var diff = _stack - _currentMissions.Count;
                    for (var i = 0; i < diff && _queueMissions.Count > 0; i++)
                    {
                        _currentMissions.Add(_queueMissions[0]);
                        _queueMissions.RemoveAt(0);
                    }
                }
                else if(_currentMissions.Count > _stack)
                {
                    var diff = _currentMissions.Count - _stack;
                    for (var i = 0; i < diff; i++)
                    {
                        _queueMissions.Add(_currentMissions[_currentMissions.Count - 1]);
                        _currentMissions.RemoveAt(_currentMissions.Count - 1);
                    }
                }
            }
        }

        public void Dispatch(string id, float value)
        {
            List<Mission> missions = null;
            foreach (var mission in _currentMissions)
            {
                if (mission.Id == id)
                {
                    mission.Current = value;
                    if (mission.Current >= mission.Target)
                    {
                        mission.IsCompleted = true;
                        if (missions == null)
                        {
                            missions = new List<Mission>(1) {mission};
                        }
                    }
                }
            }
            if (missions != null && OnComplete != null)
            {
                foreach (var m in missions)
                {
                    if (_currentMissions.Remove(m) && _queueMissions.Count > 0)
                    {
                        _currentMissions.Add(_queueMissions[0]);
                        _queueMissions.RemoveAt(0);
                    }
                    if (_completedMissions.Contains(m) == false)
                    {
                        _currentMissions.Add(m);
                    }
                }
                OnComplete.Invoke(missions.ToArray(), this);
            }
        }

        public void Load(Mission[] missions)
        {

        }
    }
}
