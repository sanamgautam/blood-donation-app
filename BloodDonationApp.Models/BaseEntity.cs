using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BloodDonationApp.Models
{
    public class BaseEntity
    {
        public Guid Id { get; set; }
        ////[ForeignKey("CreatedBy")]
        public Guid CreatedById { get; set; }
        //public virtual User CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        //[ForeignKey("ModifiedBy")]
        public Guid? ModifiedById { get; set; }
        //public virtual User ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
