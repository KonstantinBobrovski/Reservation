using Core.Models;
using Microsoft.AspNetCore.Mvc;
using Reservation.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace Reservation.Areas.Reservations.Models
{
    public class ReservationVM
    {
        public Reservation.Core.Models.Reservation Reservation { get; set; }
        public ReservationState NewState { get; set; }
    }
}
