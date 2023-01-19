using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservation.Core.Models
{
    public class Table : BaseEntity
    {
        public int RestaurantId { get; set; }
        public Restaurant? Restaurant { get; set; }

        /// <summary>
        /// Max people for this table
        /// </summary>
        public int Capacity { get; set; }

        public List<Reservation>? Reservations { get; set; } = new List<Reservation>();

        /// <summary>
        /// Name of table so user can understand which table is choosen
        /// </summary>
        public string NameOfTable { get; set; } = null!;
    }
}
