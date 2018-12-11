using System;

namespace Store
{
    public sealed class Customer
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public byte[] Photo { get; set; }

        public string CreatedBy { get; set; }

        public string ModifiedBy { get; set; }
    }
}