﻿using UKParliament.CodeTest.Data.Domain;
using UKParliament.CodeTest.Data.Repositories;
using UKParliament.CodeTest.Services.Interfaces;
using UKParliament.CodeTest.Services.Models;
using UKParliament.CodeTest.Services.Results;

using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using System.Net;

namespace UKParliament.CodeTest.Services.Implementations
{
    public sealed class BookingService : IBookingService
    {
        private readonly IRepository<Booking> _repository;

        public BookingService(IRepository<Booking> repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Add a booking
        /// </summary>
        public async Task<ServiceResult> BookAsync(BookingRequestModel model)
        {
            try
            {
                if (model.StartDate > model.EndDate)
                {
                    return ServiceResult.Error(ErrorMessages.InvalidDates,HttpStatusCode.BadRequest);
                }

                // Check the range of the given datetime
                if ((model.EndDate - model.StartDate).TotalHours > 1)
                {
                    return ServiceResult.Error(ErrorMessages.TimeRangeLimit,HttpStatusCode.BadRequest);
                }

                Booking booking = new Booking(model.PersonId, model.RoomId, model.StartDate, model.EndDate);
                _repository.Add(booking);
                await _repository.SaveChangesAsync();

                return ServiceResult.Success(booking.Id);
            }
            catch (Exception e)
            {
                return ServiceResult.Error(e.Message, HttpStatusCode.InternalServerError);
            }
        }


        /// <summary>
        /// Remove the booking
        /// </summary>
        public async Task<ServiceResult> RemoveAsync(int id)
        {
            try
            {
                Booking booking = await _repository.Table.AsNoTracking()
                                                         .FirstOrDefaultAsync(e => e.Id == id);

                if (booking == null)
                {
                    return ServiceResult.Error(ErrorMessages.NotFound,HttpStatusCode.NotFound);
                }

                _repository.Remove(booking);
                await _repository.SaveChangesAsync();

                return ServiceResult.Success(booking.Id);
            }
            catch (Exception e)
            {
                return ServiceResult.Error(e.Message, HttpStatusCode.InternalServerError);
            }
        }
    }
}
