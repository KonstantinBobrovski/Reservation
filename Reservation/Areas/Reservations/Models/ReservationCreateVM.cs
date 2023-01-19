using Microsoft.AspNetCore.Mvc;
using Reservation.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace Reservation.Areas.Reservations.Models
{
    public class ReservationCreateVM
    {
        [Required]
        public int TableId { get; set; }
        [Required]
        
        public DateTime StartTime { get; set; }

        [Required]
       
        public DateTime EndTime { get; set; }

        public string RestaurantName { get; set; }

        public List<Table> PossibleTables { get; set; }

        public ReservationCreateVM(string restaurantName,List<Table> tablesThatAreFreeForTime)
        {
            RestaurantName = restaurantName;
            PossibleTables = tablesThatAreFreeForTime;
        }
        //asp net is compaing if there is no parametrlesss
        public ReservationCreateVM() { }
    }
}
