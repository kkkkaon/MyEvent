namespace MyEvent.Models
{
    public class RoleList
    {
        public string RoleID { get; set; } = null!;
        public string RoleName { get; set; } = null!;
        public virtual ICollection<Member> Member { get; set; } = new List<Member>();
    }
}
