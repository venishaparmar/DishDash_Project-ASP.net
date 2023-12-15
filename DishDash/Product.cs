//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DishDash
{
    using System;
    using System.Collections.Generic;
    
    public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            this.AddToCarts = new HashSet<AddToCart>();
            this.Ordered_Items = new HashSet<Ordered_Items>();
        }
    
        public int id { get; set; }
        public bool active { get; set; }
        public string title { get; set; }
        public decimal price { get; set; }
        public byte[] image { get; set; }
        public System.DateTime create_at { get; set; }
        public Nullable<System.DateTime> update_at { get; set; }
        public Nullable<int> category_id { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AddToCart> AddToCarts { get; set; }
        public virtual Category Category { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ordered_Items> Ordered_Items { get; set; }
    }
}