using OPM.SFS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace OPM.SFS.Web.Shared
{
    public class InstitutionContactItem
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Link { get; set; }
        public string ProgramPage { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string StateName { get; set; }
        public string Postal { get; set; }
        public bool IsAcceptingApps { get; set; }
        public int NumberOfFruits { get; set; }
        public IEnumerable<ContactListItem> Contacts { get; set; } = new List<ContactListItem>();

        public static Expression<Func<Institution, InstitutionContactItem>> Projection
        {
            get
            {
                return m => new InstitutionContactItem()
                {
                    ID = m.InstitutionId,
                    Name = m.Name,
                    Type = m.InstitutionType.Name,
                    Link = m.HomePage,
                    ProgramPage = m.ProgramPage,
                    City = m.City,
                    State = m.State.Abbreviation,
                    StateName = m.State.Name,
                    Postal = m.PostalCode,
                    IsAcceptingApps = m.IsAcceptingApplications,
                    Contacts = m.InstitutionContacts.AsQueryable().Select(ContactListItem.Projection).ToList()
                };
            }
        }
    }

    public class ContactListItem
    {
        public string ContactType { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        public static Expression<Func<InstitutionContact, ContactListItem>> Projection
        {
            get
            {
                return c => new ContactListItem()
                {
                    ContactType = c.InstitutionContactType.Name,
                    Name = $"{c.FirstName} {c.LastName}",
                    Phone = c.Phone,
                    Email = c.Email
                };
            }
        }
    }
}
