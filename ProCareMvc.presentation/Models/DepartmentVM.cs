﻿namespace ProCareMvc.presentation.Models
{
    public class DepartmentVM
    {
        
        public Guid Id { get; set; }
        public string Name { get; set; }   
        public Guid ManagerId { get; set; }
        public Guid HospitalId { get; set; }
       
    }
}
