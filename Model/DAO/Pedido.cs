//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Model.DAO
{
    using System;
    using System.Collections.Generic;
    
    public partial class Pedido
    {
        public System.Guid ClienteID { get; set; }
        public int NroPedido { get; set; }
        public System.DateTime Data { get; set; }
    
        public virtual Cliente Cliente { get; set; }
        public virtual DetalhesPedido DetalhesPedido { get; set; }
    }
}
