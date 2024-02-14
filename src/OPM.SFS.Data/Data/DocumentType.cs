using System;
using System.Collections.Generic;

#nullable disable

namespace OPM.SFS.Data
{
    public partial class DocumentType
    {
        public int DocumentTypeId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool IsDisabled { get; set; }
    }
}
