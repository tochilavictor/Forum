using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class SectionEntity:IEntity
    {
        public int ID { get; set; }
        public int Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<SectionModerator> Moderators { get; set; }
        public virtual ICollection<Topic> Topics { get; set; }
    }
}
