namespace BITClientServer.Model
{
    public abstract class Staff
    {
        public int StaffId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string Suburb { get; set; }
        public string State { get; set; } = StateList.StateCollection[0];
        public string Postcode { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string AccName { get; set; }
        public string AccBSB { get; set; }
        public string AccNumber { get; set; }
        public char Gender { get; set; } = GenderList.GenderCollection[0];

        public Staff() { }

        public Staff(int staffid, string firstname, string lastname, string street, string suburb, string state, string postcode, string email, string phone, string accname, string accbsb, string accnumber, char gender)
        {
            this.StaffId = staffid;
            this.FirstName = firstname;
            this.LastName = lastname;
            this.Street = street;
            this.Suburb = suburb;
            this.State = state;
            this.Postcode = postcode;
            this.Email = email;
            this.Phone = phone;
            this.AccName = accname;
            this.AccBSB = accbsb;
            this.AccNumber = accnumber;
            this.Gender = gender;
        }

    }
}
