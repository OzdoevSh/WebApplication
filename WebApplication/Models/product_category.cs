//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApplication.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class product_category
    {
        public int id { get; set; }
        public int product_id { get; set; }
        public int category_id { get; set; }
    
        public virtual category category { get; set; }
        public virtual product product { get; set; }
    }
}
