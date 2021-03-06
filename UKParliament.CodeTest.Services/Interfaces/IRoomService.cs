﻿
using System;
using System.Threading.Tasks;

namespace UKParliament.CodeTest.Services
{
    public interface IRoomService
    {
        /// <summary>
        /// Get by id
        /// </summary>
        Task<ServiceResult> GetAsync(int id);

        /// <summary>
        ///  Get all with the given filtering parameters
        /// </summary>
        Task<ServiceResult> GetAllAsync(string name);

        /// <summary>
        ///  Get available rooms in the given datetime range
        /// </summary>
        Task<ServiceResult> GetAvailableRoomsAsync(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Add a room
        /// </summary>
        Task<ServiceResult> AddAsync(RoomRequestModel model);

        /// <summary>
        /// Update the room
        /// </summary>
        Task<ServiceResult> UpdateAsync(int id, RoomRequestModel model);

        /// <summary>
        /// Remove the room
        /// </summary>
        Task<ServiceResult> RemoveAsync(int id, RemoveRoomModel model);

        /// <summary>
        /// Remove rooms
        /// </summary>
        Task<ServiceResult> RemoveRangeAsync(RemoveRoomsModel model);
    }
}
