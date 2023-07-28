using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace INDO_FIN_NET.Repository.Data
{
    [Keyless]
    [Table("Adm_Cosmos_MasterData")]
    public partial class AdmCosmosMasterData
    {
        public long Id { get; set; }
        public string ref_rec_type { get; set; }
        public string ref_code { get; set; }
        public string ref_desc { get; set; }
        public string ref_Name { get; set; }
        public int isActive { get; set; }
    }

}
