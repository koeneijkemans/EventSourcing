namespace Kei.EventSourcing.Sandbox.Read
{
    public class SuperSpecialPersonModel
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public int Age { get; set; }

        public override string ToString()
        {
            return $"{Name}, {Age}";
        }
    }
}
