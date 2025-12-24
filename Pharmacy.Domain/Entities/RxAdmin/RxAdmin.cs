namespace Pharmacy.Domain.Entities.RxAdmin
{
    public class RxAdmin
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
    }
}
