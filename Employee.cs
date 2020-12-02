using System;

namespace deloveh.david.EfSqlDurableFa
{
    public partial class Employee
    {
        public Employee()
        {

        }

        public Guid EmployeeID { get; set; } = Guid.NewGuid();
        public string EmployeeNumber { get; set; }
        public string EmployeeEmails { get; set; }

        public string EmployeeUpn { get; set; }
        public string EmployeeDisplayName { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    }
}
