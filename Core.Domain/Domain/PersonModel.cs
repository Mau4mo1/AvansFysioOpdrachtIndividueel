using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
namespace AvansFysioOpdrachtIndividueel.Models
{
    abstract public class PersonModel
    {
        public int Id { get; set; }
        [Required, 
         MaxLength(255)]
        public string Name { get; set; }
        [Required, 
         MaxLength(255),
         EmailAddress]
        public string Email { get; set; }
    
        public PersonModel()
        {

        }
        public PersonModel(int id, string name, string email)
        {
            Id = id;
            Name = name;
            Email = email;
        }
        public PersonModel(string name, string email)
        { 
            Name = name;
            Email = email;
        }
    }
}
