namespace HcmMemberSearch.Modals
{
    public class Physician
    {
        public int PhysicianId { get; set; }
        public String PhysicianName { get; set; }
        public String PhysicianState { get; set; }
        public override bool Equals(object? other)
        {
            var toCompareWith = other as Physician;
            if (toCompareWith == null)
                return false;
            return this.PhysicianId == toCompareWith.PhysicianId &&
                this.PhysicianName == toCompareWith.PhysicianName && this.PhysicianState == toCompareWith.PhysicianState;   
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }
}
