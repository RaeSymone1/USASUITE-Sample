using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPM.SFS.Data
{
    [Table("EmailTemplate")]
    public class EmailTemplate
    {
        public int EmailTemplateID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Subject { get; set; }
        public string Template { get; set; }
        public DateTime DateInserted { get; set; }
        public DateTime LastModified { get; set; }
    }
}
