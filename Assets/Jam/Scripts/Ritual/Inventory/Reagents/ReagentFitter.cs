using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Jam.Scripts.Ritual.Inventory.Reagents
{
    public class ReagentFitter : MonoBehaviour
    {
        [SerializeField] private List<ReagentRoom> _rooms;
        public int OccupiedRooms => _rooms.Count(room => room.ReagentInside != null);
        public bool HaveFreeRooms => _rooms.Any(room => room.ReagentInside == null);

        public event Action OnAnyRoomChanged;

        public void SetReagent(ReagentDefinition reagentToAdd, out ReagentRoom reagentRoom)
        {
            reagentRoom = _rooms.First(room => room.ReagentInside == null);
            reagentRoom.SetReagent(reagentToAdd);
        }

        public bool HaveReagent(ReagentDefinition checkReagent) =>
            _rooms.Any(room => room.ReagentInside == checkReagent);

        public List<ReagentRoom> GetOccupiedRooms() => 
            _rooms.Where(room => room.ReagentInside != null).ToList();

        public void ReleaseRooms(bool consumeReagents)
        {
            foreach (var room in _rooms.Where(room => room.ReagentInside != null)) 
                room.ReleaseReagent(consumeReagents);
        }

        private void RoomChanged() => 
            OnAnyRoomChanged?.Invoke();

        private void Awake()
        {
            foreach (var reagentRoom in _rooms) reagentRoom.OnRoomChanged += RoomChanged;
        }

        private void OnDestroy()
        {
            foreach (var reagentRoom in _rooms) reagentRoom.OnRoomChanged -= RoomChanged;
        }
    }
}