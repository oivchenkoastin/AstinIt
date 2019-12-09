using AstinIt.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace AstinIt
{
    public class BaseDbObject : IDBObject
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
    }

    public class LocalizableDbObject : BaseDbValueObject
    {
        public Guid LanguageId { get; set; }
        public Language Language { get; set; }
    }

    public class BaseDbValueObject : BaseDbObject //description, value, name
    {
        public string Description { get; set; }
        public string Value { get; set; }
    }

    public class Project : BaseDbValueObject
    {
        public ApplicationUser ApplicationUser { get; set; }
        public string ApplicationUserId { get; set; }
        public ICollection<ProjectCost> ProjectCost { get; set; }
    }

    public class ProjectCost : BaseDbValueObject
    {
        public Currency Currency { get; set; }
        public Guid CurrencyId { get; set; }   
        public Project Project { get; set; }
        public Guid ProjectId { get; set; }   
    }

    public class Currency : BaseDbValueObject
    {
        public decimal Ratio { get; set; }
    }
    

    public class City : BaseDbValueObject
    {
        public Country Country { get; set; }     
        public Guid CountryId { get; set; }   
    }


    public class Country : BaseDbValueObject
    {
    }

    public class Info : LocalizableDbObject
    {
    }
    
    public class PageData : LocalizableDbObject
    {
        public string Title { get; set; }
        public string Header { get; set; }
        public string BodyText { get; set; }
    }

    public class Language : BaseDbValueObject
    {
        public string Code { get; set; }
    }

    public class Article : LocalizableDbObject
    {
        public string Title { get; set; }
        //public string Image { get; set; }
        public string Header { get; set; }
        public string BodyText { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
    }

    public class Vacancy : LocalizableDbObject
    {
        public string PositionName { get; set; }
        public string Technologies { get; set; }
    }


    public class UserTask : BaseDbValueObject
    {
        public UserTaskStatus UserTaskStatus { get; set; }

        public ApplicationUser Employee { get; set; }
        public string EmployeeId { get; set; }
        public ApplicationUser Customer { get; set; }
        public string CustomerId { get; set; }
        public int Priority { get; set; }
    }

    public enum UserTaskStatus
    {
        Backlog = 0,
        Active = 1,
        InProgress = 2,
        WaitingForReview = 3,
        Done = 4
    }

}
