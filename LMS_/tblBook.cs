//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LMS_
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    public partial class tblBook
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Mandatory")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Mandatory")]
        public string CategoryType { get; set; }
        [Required(ErrorMessage = "Mandatory")]
        public string AuthorName { get; set; }
        [Required(ErrorMessage = "Mandatory")]
        public string PublicationName { get; set; }
        [Required(ErrorMessage = "Mandatory")]
        public string ISBN { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
    }
}
