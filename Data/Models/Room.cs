﻿using stretch_ceilings_app.Interfaces;
using stretch_ceilings_app.Utility.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace stretch_ceilings_app.Models
{
    [Table("Rooms")]
    public class Room : IRoom
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Area { get; set; }
        [Required]
        public RoomType Type { get; set; }
        public int Corners { get; set; }
        [Column("EstateId")]
        public int EstateId { get; set; }
        [Column("EstateId")]
        public Estate Estate { get; set; }
        public DateTime? DateDeleted { get; set; }

        public void Add()
        {
            throw new NotImplementedException();
        }

        public string AddPlane(string path)
        {
            throw new NotImplementedException();
        }

        public string ChangePlane(string path)
        {
            throw new NotImplementedException();
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }

        public string GetPlane()
        {
            throw new NotImplementedException();
        }

        public void Update()
        {
            throw new NotImplementedException();
        }
    }
}