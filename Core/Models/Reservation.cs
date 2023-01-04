using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservation.Core.Models
{
    public class Reservation : BaseEntity
    {
        public int TableId { get; set; }

        public string ClientId { get; set; }

        /// <summary>
        /// Start date for reservation
        /// </summary>
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
