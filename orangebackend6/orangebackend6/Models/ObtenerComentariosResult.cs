﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace orangebackend6.Models
{
    public partial class ObtenerComentariosResult
    {
        public string nombre { get; set; }
        public string email { get; set; }
        public int? estadoComentario { get; set; }
        public DateTime? feccrea { get; set; }
        public string comentario { get; set; }
        public int idcomentario { get; set; }
    }
}
