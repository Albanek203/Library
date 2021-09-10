namespace Library.Data.Models {
    public class AdminUser {
        public          int    Id         { get; set; }
        public          string Name       { get; set; }
        public          string Surname    { get; set; }
        public          string Email      { get; set; }
        public override string ToString() { return Email + " " + Name + " " + Surname; }
    }
}