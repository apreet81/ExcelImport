using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ExcelImport.Models
{
    public class ImportedData
    {
        [Key]
        public int ImportedDataId { get; set; }
        public string Id_Document { get; set; }
        public string Phone { get; set; }
        public string Alternative_Id { get; set; }
        public string Driving_License { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Sex { get; set; }
        public string Education { get; set; }
        public string Marital_Status { get; set; }
        public string Children { get; set; }
    }
}