using System;
using System.Linq;
using Cine.Reservations.Application.DTO;
using Cine.Reservations.Core.Aggregates;
using Cine.Reservations.Core.Types;
using Cine.Reservations.Core.ValueObjects;

namespace Cine.Reservations.Infrastructure.Mongo.Documents
{
    internal static class Extensions
    {
        public static ReservationDocument AsDocument(this Reservation reservation)
            => new ReservationDocument
            {
                Id = reservation.Id,
                CinemaId = reservation.CinemaId,
                MovieId = reservation.MovieId,
                HallId = reservation.HallId,
                DateTime = reservation.DateTime,
                Reservee = new ReserveeDocument
                {
                    FullName = reservation.Reservee.FullName,
                    Email = reservation.Reservee.Email,
                    PhoneNumber = reservation.Reservee.PhoneNumber
                },
                Seats = reservation.Seats.Select(s => new SeatDocument
                {
                    Row = s.Row,
                    Number = s.Number,
                    Price = s.Price,
                    IsVip = s.IsVip
                }),
                Status = reservation.Status.ToString(),
                Version = reservation.Version
            };

        public static Reservation AsEntity(this ReservationDocument document)
        {
            var reservee = new Reservee(document.Reservee.FullName, document.Reservee.Email, document.Reservee.PhoneNumber);
            var seats = document.Seats.Select(s => new Seat(s.Row, s.Number, s.Price, s.IsVip));
            var status = Enum.Parse<ReservationStatus>(document.Status, ignoreCase: true);

            return new Reservation(document.Id, document.CinemaId, document.HallId, document.HallId, document.DateTime,
                status, reservee, seats, document.Version);
        }

        public static ReservationDto AsDocument(this ReservationDocument document)
            => new ReservationDto
            {
                Id = document.Id,
                CinemaId = document.CinemaId,
                MovieId = document.MovieId,
                HallId = document.HallId,
                DateTime = document.DateTime,
                Reservee = new ReserveeDto
                {
                    FullName = document.Reservee.FullName,
                    Email = document.Reservee.Email,
                    PhoneNumber = document.Reservee.PhoneNumber
                },
                Seats = document.Seats.Select(s => new SeatDto
                {
                    Row = s.Row,
                    Number = s.Number,
                    Price = s.Price,
                    IsVip = s.IsVip
                }),
                Status = document.Status
            };
    }
}
