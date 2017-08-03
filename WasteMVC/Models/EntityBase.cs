using System;

namespace WasteMVC.Models
{
    /// <summary>
    /// Clase Base Entidades
    /// </summary>
    public class EntityBase
    {

        public int Id { get; set; }
        public DateTime Created_At { get; set; } = DateTime.MinValue;
        public DateTime Updated_At { get; set; } = DateTime.MinValue;
        public DateTime Deleted_At { get; set; } = DateTime.MinValue;
    }
    
}