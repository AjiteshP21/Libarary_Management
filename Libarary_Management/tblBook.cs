//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Libarary_Management
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblBook
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string CategoryType { get; set; }
        public string AuthorName { get; set; }
        public string PublicationName { get; set; }
        public string ISBN { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
    }
}
